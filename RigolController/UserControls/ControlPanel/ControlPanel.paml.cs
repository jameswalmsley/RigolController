using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RigolController.UserControls.ControlPanel
{
    public class ControlPanel : UserControl
    {
        public ControlPanel()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
