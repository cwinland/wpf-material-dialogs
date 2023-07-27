using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using wpf_material_dialogs.Buttons;
using wpf_material_dialogs.Interfaces;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using UserControl = System.Windows.Controls.UserControl;

namespace wpf_material_wpfTest
{
    /// <inheritdoc cref="System.Windows.Controls.UserControl" />
    /// <inheritdoc cref="IDialog" />
    /// <summary>
    ///     Interaction logic for TestCustomDialog.xaml
    /// </summary>
    public partial class TestCustomDialog : UserControl, INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Fields

        private bool showButtons = true;

        #endregion

        #region Properties

        public ICommand CloseCommand => new AsyncRelayCommand<DialogResult>(result =>
        {
            DialogHost.CloseDialogCommand.Execute(result, this);
            return Task.CompletedTask;
        });

        public bool ShowButtons
        {
            get => showButtons;
            set
            {
                if (value == showButtons)
                {
                    return;
                }

                showButtons = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedButtons));
            }
        }

        public IButtons SelectedButtons => new AbortRetryIgnoreButtons();

        /// <summary>
        ///     Gets or sets the button alignment.
        /// </summary>
        /// <value>The button alignment.</value>
        public HorizontalAlignment ButtonAlignment { get; set; } = HorizontalAlignment.Right;

        #endregion

        public TestCustomDialog() => InitializeComponent();

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}