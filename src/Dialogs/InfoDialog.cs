using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Windows.Media;
using wpf_material_dialogs.Enums;

namespace wpf_material_dialogs.Dialogs
{
    /// <summary>
    /// Information Dialog.
    /// Implements the <see cref="DialogBase" />
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="DialogBase" />
    /// <seealso cref="INotifyPropertyChanged" />
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
