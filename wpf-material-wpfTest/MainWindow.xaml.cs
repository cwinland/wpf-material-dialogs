using System;
using System.Windows;
using wpf_material_dialogs.Dialogs;

namespace wpf_material_wpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        /// <inheritdoc />
        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            var dialog = await new CustomDialog
            {
                SubTitle =
                    "This is a custom dialog provided by the test application. It can use regular dialog parameters or custom parameters, colors, buttons or reuse parts of the built-in dialog.",
            }.ShowDialog();

            await new InfoDialog
            {
                Text = $"You pressed {dialog}",
                ButtonAlignment = HorizontalAlignment.Center,
            }.ShowDialog();
        }
    }
}
