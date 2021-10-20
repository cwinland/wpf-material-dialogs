using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using wpf_material_dialogs.Buttons;
using wpf_material_dialogs.Enums;
using wpf_material_dialogs.Interfaces;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace wpf_material_dialogs.Dialogs
{
    /// <summary>
    /// Class DialogBase.
    /// Implements the <see cref="IDialog" />
    /// </summary>
    /// <seealso cref="IDialog" />
    public abstract class DialogBase : IDialog
    {
        private readonly object buttonLock = new();
        private readonly List<Type> buttonTypes = new();
        private HorizontalAlignment buttonAlignment = HorizontalAlignment.Right;
        private IButtons buttons = new OkButtons();
        private DialogButton dialogButtons = DialogButton.OkButtons;
        private Brush iconBrush;
        private PackIconKind iconKind;
        private bool showIcon;
        private string text;
        private double textFontSize = 14;
        private string title;
        private double titleFontSize = 16;

        /// <summary>
        /// Gets the selected buttons based on the <see cref="DialogButtons"/> selection.
        /// </summary>
        /// <value><see cref="IButtons"/>.</value>
        public IButtons SelectedButtons => DialogButtons == DialogButton.Custom
            ? Buttons
            : GetButtons(DialogButtons);

        /// <summary>
        /// Gets or sets the button alignment.
        /// </summary>
        /// <value><see cref="HorizontalAlignment"/>.</value>
        public HorizontalAlignment ButtonAlignment
        {
            get => buttonAlignment;
            set
            {
                buttonAlignment = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the custom buttons to be used. <see cref="DialogButtons"/> must set to custom for this property to apply.
        /// </summary>
        /// <value><see cref="IButtons"/>.</value>
        public IButtons Buttons
        {
            get => buttons;
            set
            {
                buttons = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the buttons to use in the dialog.
        /// </summary>
        /// <value><see cref="DialogButton"/>.</value>
        public DialogButton DialogButtons
        {
            get => dialogButtons;
            set
            {
                dialogButtons = value;
                NotifyOfPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the icon brush.
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

        /// <summary>
        /// Gets or sets the kind of the icon.
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

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether to show the icon.
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

        /// <summary>
        /// Gets or sets the dialog primary text.
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

        /// <summary>
        /// Gets or sets the size of the text font.
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

        /// <summary>
        /// Gets or sets the dialog title.
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

        /// <summary>
        /// Gets or sets the size of the title font.
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

        /// <summary>
        /// Gets the buttons.
        /// </summary>
        /// <param name="button">The button enum to convert to an IButton.</param>
        /// <returns>IButtons.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">button</exception>
        /// <exception cref="ArgumentOutOfRangeException">button</exception>
        /// <exception cref="ArgumentOutOfRangeException">button</exception>
        public IButtons GetButtons(DialogButton button)
        {
            lock (buttonLock)
            {
                var assembly = Assembly.GetExecutingAssembly();

                if (!buttonTypes.Any())
                {
                    buttonTypes.AddRange(assembly
                                         .GetTypes()
                                         .Where(mytype => mytype.GetInterfaces().Contains(typeof(IButtons))));
                }

                var buttons = buttonTypes
                    .First(mytype => mytype.Name.Equals(button.ToString(), StringComparison.CurrentCultureIgnoreCase));

                return buttons?.FullName != null
                    ? assembly.CreateInstance(buttons.FullName) as IButtons
                    : throw new ArgumentOutOfRangeException(nameof(button));
            }
        }

        /// <summary>
        /// Notifies the of property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void NotifyOfPropertyChanged([CallerMemberName]string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Shows the dialog with given configuration.
        /// </summary>
        /// <param name="dialogHosId">The dialog hos identifier.</param>
        /// <param name="openedAction">The opened action.</param>
        /// <param name="closingAction">The closing action.</param>
        /// <returns><see cref="Tuple{T1,T2}" /> containing <see cref="DialogResult" /> and <see cref="bool" />.</returns>
        public async Task<DialogResult> ShowDialog(object dialogHosId = null,
                                                   DialogOpenedEventHandler openedAction = null,
                                                   DialogClosingEventHandler closingAction = null) =>
            await DialogHost.Show(this, dialogHosId, openedAction, closingAction) as DialogResult? ?? DialogResult.None;

        /// <summary>
        /// Shows the dialog with a few customizable options.
        /// </summary>
        /// <param name="dialogText">The dialog text.</param>
        /// <param name="buttons">The footer dialog buttons.</param>
        /// <param name="title">The dialog title.</param>
        /// <param name="dialogHosId">The dialog identifier.</param>
        /// <returns><see cref="DialogResult" />.</returns>
        public async Task<DialogResult> ShowDialog(string dialogText, DialogButton buttons, string title, object dialogHosId = null)
        {
            Title = title;
            Text = dialogText;
            DialogButtons = buttons;

            return await ShowDialog(dialogHosId);
        }

        /// <summary>
        /// Shows the dialog with a few customizable options.
        /// </summary>
        /// <param name="dialogText">The dialog text.</param>
        /// <param name="buttons">The footer dialog buttons.</param>
        /// <param name="iconKind">Kind of the icon.</param>
        /// <param name="iconBrush">The icon brush.</param>
        /// <param name="title">The dialog title.</param>
        /// <param name="dialogHosId">The dialog identifier.</param>
        /// <returns><see cref="DialogResult" />.</returns>
        public async Task<DialogResult> ShowDialog(string dialogText, DialogButton buttons, PackIconKind iconKind, Brush iconBrush, string title, object dialogHosId = null)
        {
            IconKind = iconKind;
            ShowIcon = true;
            IconBrush = iconBrush;

            return await ShowDialog(dialogText, buttons, title, dialogHosId);
        }

        /// <summary>
        /// Shows the dialog with given configuration.
        /// </summary>
        /// <param name="dialogHosId">The dialog hos identifier.</param>
        /// <param name="openedAction">The opened action.</param>
        /// <param name="closingAction">The closing action.</param>
        /// <returns><see cref="Tuple{T1,T2}" /> containing <see cref="DialogResult" /> and <see cref="bool" />.</returns>
        public async Task<object> ShowDialogObject(object dialogHosId = null,
                                                   DialogOpenedEventHandler openedAction = null,
                                                   DialogClosingEventHandler closingAction = null) =>
            await DialogHost.Show(this, dialogHosId, openedAction, closingAction);

        /// <summary>
        /// Shows the destroy alert dialog.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns><c>true</c> if <see cref="DialogResult.Yes" />, <c>false</c> otherwise.</returns>
        public static async Task<DialogResult> ShowAlertDialog(string text, DialogButton buttons) =>
            await new AlertDialog
            {
                Title = "Alert",
                IconBrush = Brushes.Red,
                Text = text,
                DialogButtons = buttons,
            }.ShowDialog();
    }
}
