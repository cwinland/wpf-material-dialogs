using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using wpf_material_dialogs.Interfaces;
using UserControl = System.Windows.Controls.UserControl;

namespace wpf_material_wpfTest
{
    /// <inheritdoc cref="UserControl" />
    /// <inheritdoc cref="IDialog" />
    /// <summary>
    /// Interaction logic for TestCustomDialog.xaml
    /// </summary>
    public partial class TestCustomDialog : UserControl
    {
        public ICommand CloseCommand => new AsyncRelayCommand<DialogResult>((result) =>
        {
            DialogHost.CloseDialogCommand.Execute(result, this);
            return Task.CompletedTask;
        });

        public TestCustomDialog()
        {
            InitializeComponent();
        }
    }
}
