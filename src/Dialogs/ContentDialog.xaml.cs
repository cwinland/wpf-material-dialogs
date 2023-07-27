using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using wpf_material_dialogs.Enums;
using wpf_material_dialogs.Interfaces;

namespace wpf_material_dialogs.Dialogs
{
    /// <summary>
    ///     Interaction logic for ContentDialog.xaml
    /// </summary>
    /// <inheritdoc cref="System.Windows.Controls.UserControl" />
    public partial class ContentDialog : UserControl, INotifyPropertyChanged
    {
        #region Events

        /// <inheritdoc />
        /// <summary>
        ///     Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Fields

        /// <summary>
        ///     The content control property
        /// </summary>
        public static readonly DependencyProperty ContentControlProperty = DependencyProperty.Register(
            nameof(ContentControl), typeof(object), typeof(ContentDialog), new FrameworkPropertyMetadata(
                default, FrameworkPropertyMetadataOptions.AffectsParentArrange |
                         FrameworkPropertyMetadataOptions.AffectsParentMeasure |
                         FrameworkPropertyMetadataOptions.AffectsRender |
                         FrameworkPropertyMetadataOptions.AffectsMeasure, Refresh));

        /// <summary>
        ///     The content control property
        /// </summary>
        public static readonly DependencyProperty DialogButtonsProperty = DependencyProperty.Register(
            nameof(DialogButtons), typeof(DialogButton), typeof(ContentDialog), new FrameworkPropertyMetadata(
                DialogButton.OkButtons, FrameworkPropertyMetadataOptions.AffectsParentArrange |
                                        FrameworkPropertyMetadataOptions.AffectsParentMeasure |
                                        FrameworkPropertyMetadataOptions.AffectsRender |
                                        FrameworkPropertyMetadataOptions.AffectsMeasure, Refresh));

        /// <summary>
        ///     The button alignment property
        /// </summary>
        public static readonly DependencyProperty ButtonAlignmentProperty = DependencyProperty.Register(
            nameof(ButtonAlignment), typeof(HorizontalAlignment), typeof(ContentDialog), new PropertyMetadata(HorizontalAlignment.Right, Refresh));

        /// <summary>
        ///     The buttons property
        /// </summary>
        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(
            nameof(Buttons), typeof(IButtons), typeof(ContentDialog), new PropertyMetadata(default(IButtons), Refresh));

        /// <summary>
        ///     The show buttons property
        /// </summary>
        public static readonly DependencyProperty ShowButtonsProperty = DependencyProperty.Register(
            nameof(ShowButtons), typeof(bool), typeof(ContentDialog), new PropertyMetadata(true, Refresh));

        private readonly object buttonLock = new();
        private readonly List<Type> buttonTypes = new();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether [show buttons].
        /// </summary>
        /// <value><c>true</c> if [show buttons]; otherwise, <c>false</c>.</value>
        public bool ShowButtons
        {
            get => (bool) GetValue(ShowButtonsProperty);
            set => SetValue(ShowButtonsProperty, value);
        }

        /// <summary>
        ///     Gets or sets the buttons.
        /// </summary>
        /// <value>The buttons.</value>
        public IButtons Buttons
        {
            get => (IButtons) GetValue(ButtonsProperty);
            set => SetValue(ButtonsProperty, value);
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

        /// <summary>
        ///     Gets the selected buttons based on the <see cref="DialogButtons" /> selection.
        /// </summary>
        /// <value><see cref="IButtons" />.</value>
        public IButtons SelectedButtons => ShowButtons
            ? DialogButtons == DialogButton.Custom
                ? Buttons
                : GetButtons(DialogButtons)
            : null;

        /// <summary>
        ///     Gets or sets the content control.
        /// </summary>
        /// <value>The content control.</value>
        public object ContentControl
        {
            get => GetValue(ContentControlProperty);
            set => SetValue(ContentControlProperty, value);
        }

        /// <summary>
        ///     Gets or sets the dialog buttons.
        /// </summary>
        /// <value>The dialog buttons.</value>
        public DialogButton DialogButtons
        {
            get => (DialogButton) GetValue(DialogButtonsProperty);
            set
            {
                SetValue(DialogButtonsProperty, value);
                NotifyOfPropertyChanged(nameof(SelectedButtons));
            }
        }

        #endregion

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:wpf_material_dialogs.Dialogs.ContentDialog" /> class.
        /// </summary>
        public ContentDialog() => InitializeComponent();

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:wpf_material_dialogs.Dialogs.ContentDialog" /> class.
        /// </summary>
        /// <param name="contentControl">The content control.</param>
        public ContentDialog(UserControl contentControl) : this() => ContentControl = contentControl;

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:wpf_material_dialogs.Dialogs.ContentDialog" /> class.
        /// </summary>
        /// <param name="contentControl">The content control.</param>
        public ContentDialog(IDialog contentControl) : this() => ContentControl = contentControl;

        /// <summary>
        ///     Gets the buttons.
        /// </summary>
        /// <param name="button">The button enum to convert to an IButton.</param>
        /// <returns>IButtons.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">button</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">button</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">button</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">button</exception>
        public IButtons GetButtons(DialogButton button)
        {
            // ReSharper disable once PossibleNullReferenceException
            lock (buttonLock)
            {
                var assembly = Assembly.GetExecutingAssembly();

                if (!buttonTypes?.Any() ?? false)
                {
                    buttonTypes.AddRange(assembly
                        .GetTypes()
                        .Where(mytype => mytype.GetInterfaces().Contains(typeof(IButtons))));
                }

                var selectedButtonType =
                    buttonTypes?.First(mytype => button.ToString().Equals(mytype?.Name, StringComparison.CurrentCultureIgnoreCase));

                return selectedButtonType?.FullName != null
                    ? assembly.CreateInstance(selectedButtonType.FullName) as IButtons
                    : throw new ArgumentOutOfRangeException(nameof(button));
            }
        }

        /// <summary>
        ///     Notifies the of property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <inheritdoc cref="INotifyPropertyChanged" />
        public void NotifyOfPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        internal void Refresh() => NotifyOfPropertyChanged(nameof(SelectedButtons));

        private static void Refresh(DependencyObject o, DependencyPropertyChangedEventArgs _)
        {
            if (o is ContentDialog d)
            {
                d.Refresh();
            }
        }
    }
}