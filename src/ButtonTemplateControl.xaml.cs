using System.Windows;
using System.Windows.Controls;
using wpf_material_dialogs.Interfaces;

namespace wpf_material_dialogs
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Interaction logic for ButtonTemplateControl.xaml
    /// </summary>
    public partial class ButtonTemplateControl : UserControl
    {
        /// <summary>
        ///     The button alignment property
        /// </summary>
        public static readonly DependencyProperty ButtonAlignmentProperty = DependencyProperty.Register(
            nameof(ButtonAlignment), typeof(HorizontalAlignment), typeof(ButtonTemplateControl), new PropertyMetadata(HorizontalAlignment.Right));

        public static readonly DependencyProperty SelectedButtonsProperty = DependencyProperty.Register(
            nameof(SelectedButtons), typeof(IButtons), typeof(ButtonTemplateControl), new PropertyMetadata(default(IButtons)));

        public IButtons SelectedButtons
        {
            get => (IButtons) GetValue(SelectedButtonsProperty);
            set => SetValue(SelectedButtonsProperty, value);
        }
        /// <summary>
        ///     Gets or sets the button alignment.
        /// </summary>
        /// <value>The button alignment.</value>
        public HorizontalAlignment ButtonAlignment
        {
            get => (HorizontalAlignment) GetValue(ButtonAlignmentProperty);
            set => SetValue(ButtonAlignmentProperty, value);
        }

        public ButtonTemplateControl()
        {
            InitializeComponent();
        }
    }
}
