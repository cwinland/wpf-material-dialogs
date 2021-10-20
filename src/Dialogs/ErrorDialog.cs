using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Windows.Media;
using wpf_material_dialogs.Enums;

namespace wpf_material_dialogs.Dialogs
{
    /// <summary>
    /// Error dialog.
    /// Implements the <see cref="DialogBase" />
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="DialogBase" />
    /// <seealso cref="INotifyPropertyChanged" />
    public class ErrorDialog : DialogBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDialog" /> class.
        /// </summary>
        public ErrorDialog()
        {
            Title = "Error";
            DialogButtons = DialogButton.OkButtons;
            IconKind = PackIconKind.Error;
            IconBrush = Brushes.Red;
            ShowIcon = true;
        }
    }
}
