using System.Windows.Controls;
using wpf_material_dialogs.Interfaces;

namespace wpf_material_dialogs.Buttons
{
    /// <summary>
    /// Interaction logic for YesNoCancelButtons.xaml
    /// </summary>
    public partial class YesNoCancelButtons : UserControl, IButtons
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YesNoCancelButtons" /> class.
        /// </summary>
        public YesNoCancelButtons() => InitializeComponent();
    }
}
