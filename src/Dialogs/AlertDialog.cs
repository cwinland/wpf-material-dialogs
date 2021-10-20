using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Windows.Media;
using wpf_material_dialogs.Enums;

namespace wpf_material_dialogs.Dialogs
{
    /// <summary>
    /// Alert dialog.
    /// Implements the <see cref="DialogBase" />
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="DialogBase" />
    /// <seealso cref="INotifyPropertyChanged" />
    public class AlertDialog : DialogBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlertDialog" /> class.
        /// </summary>
        public AlertDialog()
        {
            Title = "Alert";
            DialogButtons = DialogButton.YesNoButtons;
            IconKind = PackIconKind.Alert;
            IconBrush = Brushes.Red;
            ShowIcon = true;
        }
    }
}
