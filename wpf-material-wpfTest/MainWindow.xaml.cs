using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using wpf_material_dialogs.Dialogs;
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

        public ICommand OpenDialog => new AsyncRelayCommand(async () =>
        {
            var result = await DialogBase.ShowDialogContent(new CustomDialog
            {
                SubTitle =
                    "This is a custom dialog provided by the test application. It can use regular dialog parameters or custom parameters, colors, buttons or reuse parts of the built-in dialog.",
            });

            await ShowResult(result);
        });

        public ICommand OpenDialog2 => new AsyncRelayCommand(async () =>
        {
            var result = await DialogBase.ShowDialogContent(new TestCustomDialog());

            await ShowResult(result);
        });

        #endregion

        public MainWindow() => InitializeComponent();

        private static async Task ShowResult(DialogResult result) => await new InfoDialog
        {
            Text = $"You pressed {result}",
            ButtonAlignment = HorizontalAlignment.Center,
        }.ShowDialog();
    }
}