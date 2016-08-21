using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace RigolController
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            App.AttachDevTools(this);

            var model = new RigolPSU.RigolPSU(string.Empty);
            model.Connect();

            DataContext = new RigolPSUViewModel(model);                       
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        bool _mouseDown = false;

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if (IsPointerOver && _mouseDown)
            {
                BeginMoveDrag();
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            _mouseDown = true;
            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerEventArgs e)
        {
            _mouseDown = false;
            base.OnPointerReleased(e);
        }
    }
}
