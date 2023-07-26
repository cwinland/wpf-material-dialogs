using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using wpf_material_dialogs.Enums;

namespace wpf_material_dialogs.Dialogs
{
    /// <inheritdoc />
    /// <summary>
    /// Warning dialog.
    /// Implements the <see cref="T:wpf_material_dialogs.Dialogs.DialogBase" />
    /// Implements the <see cref="T:System.ComponentModel.INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="T:wpf_material_dialogs.Dialogs.DialogBase" />
    /// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged" />
    public class WarningDialog : DialogBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WarningDialog" /> class.
        /// </summary>
        public WarningDialog()
        {
            Title = "Warning";
            DialogButtons = DialogButton.OkButtons;
            IconKind = PackIconKind.Alert;
            IconBrush = Brushes.DarkGoldenrod;
            ShowIcon = true;
        }
    }
}
