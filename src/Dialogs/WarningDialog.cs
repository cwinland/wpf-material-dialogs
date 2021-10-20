using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Windows.Media;
using wpf_material_dialogs.Enums;

namespace wpf_material_dialogs.Dialogs
{
    /// <summary>
    /// Warning dialog.
    /// Implements the <see cref="DialogBase" />
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="DialogBase" />
    /// <seealso cref="INotifyPropertyChanged" />
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
