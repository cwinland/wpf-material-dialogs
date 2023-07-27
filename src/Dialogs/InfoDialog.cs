using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using wpf_material_dialogs.Enums;

namespace wpf_material_dialogs.Dialogs
{
    /// <inheritdoc />
    /// <summary>
    /// Information Dialog.
    /// Implements the <see cref="T:wpf_material_dialogs.Dialogs.DialogBase" />
    /// Implements the <see cref="T:System.ComponentModel.INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="T:wpf_material_dialogs.Dialogs.DialogBase" />
    /// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged" />
    public class InfoDialog : DialogBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoDialog" /> class.
        /// </summary>
        public InfoDialog()
        {
            Title = "Information";
            DialogButtons = DialogButton.OkButtons;
            IconKind = PackIconKind.InformationCircle;
            IconBrush = Brushes.Blue;
            ShowIcon = true;
        }
    }
}
