using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using wpf_material_dialogs.Buttons;
using wpf_material_dialogs.Enums;
using wpf_material_dialogs.Interfaces;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace wpf_material_dialogs.Dialogs
{
    /// <inheritdoc />
    /// <summary>
    ///     Class DialogBase.
    ///     Implements the <see cref="T:wpf_material_dialogs.Interfaces.IDialog" />
    /// </summary>
    /// <seealso cref="T:wpf_material_dialogs.Interfaces.IDialog" />
    public abstract class DialogBase : IDialog
    {
        #region Events

        /// <inheritdoc />
        /// <summary>
        ///     Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Fields

        private readonly object buttonLock = new();
        private readonly List<Type> buttonTypes = new();
        private HorizontalAlignment buttonAlignment = HorizontalAlignment.Right;
        private IButtons buttons = new OkButtons();
        private DialogButton dialogButtons = DialogButton.OkButtons;
        private FontWeight fontWeight = FontWeights.Normal;
        private double height = double.NaN;
        private Brush iconBrush;
        private PackIconKind iconKind;
        private bool showIcon;
        private string text;
        private double textFontSize = 14;
        private string title;
        private double titleFontSize = 16;
        private FontWeight titleFontWeight = FontWeights.Bold;
        private double width = double.NaN;
        private bool showButtons = true;

        public bool ShowButtons
        {
            get => showButtons;
            set
            {
                showButtons = value;
                NotifyOfPropertyChanged();
                NotifyOfPropertyChanged(nameof(SelectedButtons));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the selected buttons based on the <see cref="DialogButtons" /> selection.
        /// </summary>
        /// <value><see cref="IButtons" />.</value>
        public IButtons SelectedButtons => ShowButtons ? DialogButtons == DialogButton.Custom
            ? Buttons
            : GetButtons(DialogButtons)
            : null;

        public FontWeight TitleFontWeight
        {
            get => titleFontWeight;
            set
            {
                titleFontWeight = value;
                NotifyOfPropertyChanged();
            }
        }

        public FontWeight FontWeight
        {
            get => fontWeight;
            set
            {
                fontWeight = value;
                NotifyOfPropertyChanged();
            }
        }

        public double Width
        {
            get => width;
            set
            {
                width = value;
                NotifyOfPropertyChanged();
            }
        }

        public double Height
        {
            get => height;
            set
            {
                height = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the button alignment.
        /// </summary>
        /// <value><see cref="T:System.Windows.HorizontalAlignment" />.</value>
        public HorizontalAlignment ButtonAlignment
        {
            get => buttonAlignment;
            set
            {
                buttonAlignment = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the custom buttons to be used. <see cref="P:wpf_material_dialogs.Dialogs.DialogBase.DialogButtons" /> must set to custom for this property to
        ///     apply.
        /// </summary>
        /// <value><see cref="T:wpf_material_dialogs.Interfaces.IButtons" />.</value>
        public IButtons Buttons
        {
            get => buttons;
            set
            {
                buttons = value;
                NotifyOfPropertyChanged();
                NotifyOfPropertyChanged(nameof(SelectedButtons));
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the buttons to use in the dialog.
        /// </summary>
        /// <value><see cref="T:wpf_material_dialogs.Enums.DialogButton" />.</value>
        public DialogButton DialogButtons
        {
            get => dialogButtons;
            set
            {
                dialogButtons = value;
                NotifyOfPropertyChanged();
                NotifyOfPropertyChanged(nameof(SelectedButtons));
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the icon brush.
        /// </summary>
        /// <value>The icon brush.</value>
        public Brush IconBrush
        {
            get => iconBrush;
            set
            {
                iconBrush = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the kind of the icon.
        /// </summary>
        /// <value>The kind of the icon.</value>
        public PackIconKind IconKind
        {
            get => iconKind;
            set
            {
                iconKind = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets a value indicating whether to show the icon.
        /// </summary>
        /// <value><c>true</c> if [show icon]; otherwise, <c>false</c>.</value>
        public bool ShowIcon
        {
            get => showIcon;
            set
            {
                showIcon = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the dialog primary text.
        /// </summary>
        /// <value>The primary text of the dialog.</value>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the size of the text font.
        /// </summary>
        /// <value>The size of the text font.</value>
        public double TextFontSize
        {
            get => textFontSize;
            set
            {
                textFontSize = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the dialog title.
        /// </summary>
        /// <value>The title of the dialog.</value>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the size of the title font.
        /// </summary>
        /// <value>The size of the title font.</value>
        public double TitleFontSize
        {
            get => titleFontSize;
            set
            {
                titleFontSize = value;
                NotifyOfPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        ///     Shows the destroy alert dialog.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns><c>true</c> if <see cref="DialogResult.Yes" />, <c>false</c> otherwise.</returns>
        public static async Task<DialogResult> ShowAlertDialog(string text, DialogButton buttons) => await new AlertDialog
        {
            Title = "Alert",
            IconBrush = Brushes.Red,
            Text = text,
            DialogButtons = buttons,
        }.ShowDialog();

        /// <summary>
        ///     Shows the dialog with a few customizable options.
        /// </summary>
        /// <param name="dialogText">The dialog text.</param>
        /// <param name="buttonType">The footer dialog buttons.</param>
        /// <param name="titleText">The dialog title.</param>
        /// <param name="dialogHosId">The dialog identifier.</param>
        /// <returns><see cref="DialogResult" />.</returns>
        public async Task<DialogResult> ShowDialog(string dialogText, DialogButton buttonType, string titleText,
            object dialogHosId = null)
        {
            Title = titleText;
            Text = dialogText;
            DialogButtons = buttonType;

            return await ShowDialog(dialogHosId);
        }

        /// <summary>
        ///     Shows the dialog with a few customizable options.
        /// </summary>
        /// <param name="dialogText">The dialog text.</param>
        /// <param name="buttonType">The footer dialog buttons.</param>
        /// <param name="packIconKind">Kind of the icon.</param>
        /// <param name="packIconBrush">The icon brush.</param>
        /// <param name="titleText">The dialog title.</param>
        /// <param name="dialogHosId">The dialog identifier.</param>
        /// <returns><see cref="DialogResult" />.</returns>
        public async Task<DialogResult> ShowDialog(string dialogText, DialogButton buttonType, PackIconKind packIconKind,
            Brush packIconBrush, string titleText, object dialogHosId = null)
        {
            IconKind = packIconKind;
            ShowIcon = true;
            IconBrush = packIconBrush;

            return await ShowDialog(dialogText, buttonType, titleText, dialogHosId);
        }

        public static async Task<DialogResult> ShowDialogContent(object content, object dialogHosId = null,
            DialogOpenedEventHandler openedAction = null,
            DialogClosingEventHandler closingAction = null)
            => content == null
                ? throw new ArgumentNullException(nameof(content))
                : await DialogHost.Show(content, dialogHosId, openedAction, closingAction) as DialogResult? ?? DialogResult.None;

        #region IDialog

        /// <inheritdoc />
        /// <summary>
        ///     Gets the buttons.
        /// </summary>
        /// <param name="button">The button enum to convert to an IButton.</param>
        /// <returns>IButtons.</returns>
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

        /// <inheritdoc />
        /// <summary>
        ///     Notifies the of property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void NotifyOfPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <inheritdoc />
        /// <summary>
        ///     Shows the dialog with given configuration.
        /// </summary>
        /// <param name="dialogHosId">The dialog hos identifier.</param>
        /// <param name="openedAction">The opened action.</param>
        /// <param name="closingAction">The closing action.</param>
        /// <returns>
        ///     <see cref="T:System.Tuple`2" /> containing <see cref="T:System.Windows.Forms.DialogResult" /> and
        ///     <see cref="T:System.Boolean" />.
        /// </returns>
        public async Task<DialogResult> ShowDialog(object dialogHosId = null,
            DialogOpenedEventHandler openedAction = null,
            DialogClosingEventHandler closingAction = null) =>
            await DialogHost.Show(this, dialogHosId, openedAction, closingAction) as DialogResult? ??
            DialogResult.None;

        /// <inheritdoc />
        /// <summary>
        ///     Shows the dialog with given configuration.
        /// </summary>
        /// <param name="dialogHosId">The dialog hos identifier.</param>
        /// <param name="openedAction">The opened action.</param>
        /// <param name="closingAction">The closing action.</param>
        /// <returns>
        ///     <see cref="T:System.Tuple`2" /> containing <see cref="T:System.Windows.Forms.DialogResult" /> and
        ///     <see cref="T:System.Boolean" />.
        /// </returns>
        public async Task<object> ShowDialogObject(object dialogHosId = null,
            DialogOpenedEventHandler openedAction = null,
            DialogClosingEventHandler closingAction = null) => await DialogHost.Show(this, dialogHosId, openedAction, closingAction);

        #endregion
    }
}