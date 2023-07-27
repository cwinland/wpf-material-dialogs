using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using wpf_material_dialogs.Dialogs;
using wpf_material_dialogs.Enums;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace wpf_material_wpfTest
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        public bool ShowButtons { get; set; } = false;
        public string SubtitleText { get; set; } =
            "This is a custom dialog provided by the test application. It can use regular dialog parameters or custom parameters, colors, buttons or reuse parts of the built-in dialog.";

        public ICommand OpenDialog => new AsyncRelayCommand(async () =>
        {
            var result = await DialogBase.ShowDialogContent(new CustomDialog
            {
                SubTitle = SubtitleText,
            });

            await ShowResultAsync(result);
        });

        public ICommand OpenDialog2 => new AsyncRelayCommand(async () =>
        {
            var result = await DialogBase.ShowDialogContent(new TestCustomDialog());

            await ShowResultAsync(result);
        });

        public ICommand OpenContentDialog => new AsyncRelayCommand(async () =>
        {
            var result = await DialogBase.ShowDialogContent(new ContentDialog(
                new TestCustomDialog() { ShowButtons = ShowButtons})
            {
                DialogButtons = DialogButton.YesNoCancelButtons,
            });

            await ShowResultAsync(result);
        });

        public ICommand OpenContentDialog2 => new AsyncRelayCommand(async () =>
        {
            var result = await DialogBase.ShowDialogContent(new ContentDialog(
                new CustomDialog()
                {
                    ShowButtons = ShowButtons,
                    SubTitle = SubtitleText,
                })
            {
                DialogButtons = DialogButton.YesNoCancelButtons,
            });

            await ShowResultAsync(result);
        });

        #endregion

        public MainWindow() => InitializeComponent();

        private static async Task ShowResultAsync(DialogResult result) => await new InfoDialog
        {
            Text = $"You pressed {result}",
            ButtonAlignment = HorizontalAlignment.Center,
        }.ShowDialog();

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {

        }
    }
}