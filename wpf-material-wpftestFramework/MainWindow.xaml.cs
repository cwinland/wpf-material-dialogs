using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Media;
using wpf_material_dialogs.Dialogs;
using wpf_material_dialogs.Enums;

namespace wpf_material_wpftestFramework
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
            var dialog = await new AlertDialog
            {
                Title = "Test Alert Title",
                Text = "This is an Alert Dialog. It is customizable and can be overridden by a custom dialog or custom buttons applied.",
                IconKind = PackIconKind.About,
                IconBrush = Brushes.Blue,
                ShowIcon = true,
                DialogButtons = DialogButton.AbortRetryIgnoreButtons,
            }.ShowDialog();

            await new InfoDialog
            {
                Text = $"You pressed {dialog}",
                ButtonAlignment = HorizontalAlignment.Center,
            }.ShowDialog();
        }
    }
}