using System.Windows.Controls;
using wpf_material_dialogs.Interfaces;

namespace wpf_material_dialogs.Buttons
{
    /// <summary>
    /// Interaction logic for OKCancelButtons.xaml
    /// </summary>
    public partial class OkCancelButtons : UserControl, IButtons
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OkCancelButtons" /> class.
        /// </summary>
        public OkCancelButtons() => InitializeComponent();
    }
}
