namespace RigolController
{
    using RigolController.Mvvm;
    using UserControls.ControlPanel;
    using ReactiveUI;
    using Avalonia.Threading;
    using System;
    using RigolPSU;
    using System.Collections.ObjectModel;


    public class RigolPSUViewModel : ViewModel<RigolPSU>
    {
        private RigolPSU powerSupply;

        public RigolPSUViewModel(RigolPSU model) : base(model)
        {
            channels = new ObservableCollection<PSUChannelViewModel>();

            foreach (var channel in model.Channels)
            {
                channels.Add(new PSUChannelViewModel(channel));               
            }
        }


        /*
        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (client.IsConnected)
            {
                timer.Stop();

                if (firstConnect)
                {
                    firstConnect = false;
                    await client.WriteLine(":OUTP? CH1");

                    string result_1 = await client.ReadAsync(TimeSpan.FromMilliseconds(1000));

                    if (result_1 == "OFF\n")
                    {
                        this.ControlPanel.OnOffButtonState = false;
                    }
                    else
                    {
                        this.ControlPanel.OnOffButtonState = true;
                    }
                }

                await client.WriteLine(":APPL? CH1");
                // CH1:30V/3A,12.000,0.500

                string result = await client.ReadAsync(TimeSpan.FromMilliseconds(1000));

                //CH1: 30V / 3A,3.200,3.000

                if (!string.IsNullOrEmpty(result))
                {
                    if (result != "Command error")
                    {
                        var parts = result.Split(new char[] { '\n', ';' })[0].Split(',');

                        double v = Convert.ToDouble(parts[1]);
                        if (this.ControlPanel.VoltageLimit != v)
                        {
                            this.ControlPanel.VoltageLimit = v;
                        }

                        double i = Convert.ToDouble(parts[2]);
                        if (this.ControlPanel.CurrentLimit != i)
                        {
                            this.ControlPanel.CurrentLimit = i;
                        }
                    }
                }

                await client_meas.WriteLine(":MEAS:ALL? CH1");

                result = await client_meas.ReadAsync(TimeSpan.FromMilliseconds(1000));
                
                if(!string.IsNullOrEmpty(result))
                {
                    var resultLines = result.Split(new char[] { '\n', ';' });
                    foreach (var line in resultLines) {
                        if(line == "Command error")
                        {
                            continue;
                        }

                        if (!string.IsNullOrEmpty(line))
                        {
                            var parts = line.Split(',');
                            this.ControlPanel.Voltage = Convert.ToDouble(parts[0]);
                            this.ControlPanel.Current = Convert.ToDouble(parts[1]);
                            this.ControlPanel.Power = Convert.ToDouble(parts[2]);
                        }
                    }
                }

                if (previousState != this.ControlPanel.OnOffButtonState)
                {
                    if (this.ControlPanel.OnOffButtonState)
                    {
                        await client.WriteLine(":OUTP CH1,ON");
                    }
                    else
                    {
                        await client.WriteLine(":OUTP CH1,OFF");
                    }

                    previousState = this.ControlPanel.OnOffButtonState;
                }

                timer.Start();

                //await client.WriteLine(":MEAS:ALL? CH1");
            }
        }*/

        private ObservableCollection<PSUChannelViewModel> channels;
        public ObservableCollection<PSUChannelViewModel> Channels
        {
            get { return channels; }
            set { this.RaiseAndSetIfChanged(ref channels, value); }
        }

    }
}
