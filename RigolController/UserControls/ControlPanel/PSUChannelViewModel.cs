using System;
using ReactiveUI;
using RigolController.Mvvm;
using Avalonia.Media;
using RigolPSU;
using Avalonia.Threading;

namespace RigolController.UserControls.ControlPanel
{
    public class PSUChannelViewModel : ViewModel<PSUChannel>
    {
        private bool buttonState = false;
        public bool OnOffButtonState
        {
            get { return buttonState;  }
            set
            {
                this.RaiseAndSetIfChanged(ref buttonState, value);

                if (buttonState)
                {
                    OnOffButtonBackground = Brush.Parse("LightGreen");
                }
                else
                {
                    OnOffButtonBackground = Brush.Parse("#444444");
                }
            }
        }

        public PSUChannelViewModel(PSUChannel model) : base(model)
        {
            model.ChannelUpdateReceived += (sender, e) =>
            {
                Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    this.RaisePropertyChanged(nameof(Voltage));
                    this.RaisePropertyChanged(nameof(Current));
                    this.RaisePropertyChanged(nameof(Power));

                    OnOffButtonState = Model.PoweredOn;

                    if (!VoltageFocused)
                    {
                        this.RaisePropertyChanged(nameof(VoltageLimit));
                    }

                    if (!CurrentLimitFocused)
                    {
                        this.RaisePropertyChanged(nameof(CurrentLimit));
                    }
                });
            };
            
            OnOffButtonCommand = ReactiveCommand.Create();
            OnOffButtonCommand.Subscribe(async _ =>
            {
                await Model.SetEnabled(!OnOffButtonState);              
            });
        }        
        
        public bool VoltageFocused { private get; set; }
        public bool CurrentLimitFocused { private get; set; }

        public double Voltage
        {
            get { return Model.Voltage; }            
        }
        
        public double Current
        {
            get { return Model.Current; }            
        }
        
        public double Power
        {
            get { return Model.Power; }            
        }

        public double VoltageLimit
        {
            get { return Model.VoltageLimit; }
            set
            {
                Model.SetVoltage(value);
            }
        }

        public double CurrentLimit
        {
            get { return Model.CurrentLimit; }
            set
            {
                Model.SetCurrentLimit(value);
            }
        }

        private IBrush onOffButtonBackground;
        public IBrush OnOffButtonBackground
        {
            get { return onOffButtonBackground; }
            set { this.RaiseAndSetIfChanged(ref onOffButtonBackground, value); }
        }

        public ReactiveCommand<object> OnOffButtonCommand { get; }

    }
}
