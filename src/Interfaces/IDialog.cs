using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using wpf_material_dialogs.Enums;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace wpf_material_dialogs.Interfaces
{
    /// <summary>
    /// Interface IDialog
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public interface IDialog : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the buttons from the library based on the enumeration <see cref="DialogButton" />.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <returns>IButtons.</returns>
        IButtons GetButtons(DialogButton button);

        /// <summary>
        /// Notifies the of property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        void NotifyOfPropertyChanged([CallerMemberName]string propertyName = "");

        /// <summary>
        /// Shows the dialog with given configuration.
        /// </summary>
        /// <param name="dialogHosId">The dialog hos identifier.</param>
        /// <param name="openedAction">The opened action.</param>
        /// <param name="closingAction">The closing action.</param>
        /// <returns><see cref="Tuple" /> containing <see cref="DialogResult" /> and <see cref="bool" />.</returns>
        Task<DialogResult> ShowDialog(object dialogHosId = null, DialogOpenedEventHandler openedAction = null, DialogClosingEventHandler closingAction = null);

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="dialogHosId">The dialog hos identifier.</param>
        /// <param name="openedAction">The opened action.</param>
        /// <param name="closingAction">The closing action.</param>
        /// <returns><see cref="Task{TResult}" /></returns>
        Task<object> ShowDialogObject(object dialogHosId = null, DialogOpenedEventHandler openedAction = null, DialogClosingEventHandler closingAction = null);

        /// <summary>
        /// Gets or sets the button alignment.
        /// </summary>
        /// <value>The button alignment.</value>
        HorizontalAlignment ButtonAlignment { get; set; }

        /// <summary>
        /// Gets the buttons.
        /// </summary>
        /// <value>The buttons.</value>
        IButtons Buttons { get; set; }

        /// <summary>
        /// Gets or sets the dialog buttons.
        /// </summary>
        /// <value>The dialog buttons.</value>
        DialogButton DialogButtons { get; set; }

        /// <summary>
        /// Gets or sets the icon brush.
        /// </summary>
        /// <value>The icon brush.</value>
        Brush IconBrush { get; set; }

        /// <summary>
        /// Gets or sets the kind of the icon.
        /// </summary>
        /// <value>The kind of the icon.</value>
        PackIconKind IconKind { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show icon].
        /// </summary>
        /// <value><c>true</c> if [show icon]; otherwise, <c>false</c>.</value>
        bool ShowIcon { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the size of the text font.
        /// </summary>
        /// <value>The size of the text font.</value>
        double TextFontSize { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the size of the title font.
        /// </summary>
        /// <value>The size of the title font.</value>
        double TitleFontSize { get; set; }
    }
}
