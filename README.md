# wpf-material-dialogs

Common and customizable dialogs made easier in WPF using [MaterialDesignThemes](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit).

## Targets

- .NET 5 - Windows
- .NET Framework 4.7.2

## How-To Quick-Start

### Define required resources dictionary

Define the required resources.

    <ResourceDictionary Source="pack://application:,,,/wpf-material-dialogs;component/DialogsDictionary.xaml" />

 This example shows the resources in app.xaml to provide these resource to the entire application.

    <Application.Resources>
         <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <materialDesign:CustomColorTheme BaseTheme="Light"PrimaryColor="DimGray"  SecondaryColor="Tomato" />
                <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml" />
                <ResourceDictionary Source="pack://application:,,,/wpf-material-dialogs;component/DialogsDictionary.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>

### Define DialogHost

This implementation uses the static version of DialogHost. The code can have more than one DialogHost, but it can reuse it to show dialogs with only one dialog host. Putting DialogHost on the main window will allow the enter application to use the dialog while the backdrop covers the entire application. Setup the dialog with the desired properties. Set the identifier to the desired name. If multiple hosts are used, this Identifier becomes unique and extrememly important to identify which host to use.

     <materialDesign:DialogHost Identifier="AlertDialog" CloseOnClickAway="False"  DialogTheme="Inherit" Style="{StaticResource MaterialDesignEmbeddedDialogHost}"
                               Foreground="{DynamicResource AttentionToActionBrush}">
        ...
    </materialDesign:DialogHost>

### Create the dialog class and call ShowDialog()

This can be a [Built-in Dialog](src/Dialogs) or a custom dialog.  

            var result = await new AlertDialog
            {
                Title = "Delete File",
                Text = $"This will erase all data in the selected file.  This action cannot be undone. Are you sure you want to continue?",
                DialogButtons = DialogButton.YesNoButtons,
                ShowIcon = true,
                IconBrush = Brushes.Red,
                IconKind = PackIconKind.Alert,
            }.ShowDialog();

            if (result == DialogResult.Yes)
            {
                ...
            }

It's that easy!

## Dialog Properties

- ButtonAlignment - Align dialog buttons left, center, or right (default).
- Buttons - Provide custom buttons (DialogButtons must be set to custom).
- DialogButtons - [DialogButtons](src/Enums/DialogButton.cs) enumeration to choose a predefined button or set to custom to use the Buttons property.
- IconBrush - Icon color using System.Windows.Media.
- IconKind - Material Icon.
- ShowIcon - Show the icon.
- TextFontSize - Text size.
- Title - Title / header text.
- TitleFontSize - Title size.

## Options

- [DialogButtons](src/Enums/DialogButton.cs) indicates the buttons to show on the dialog.  
- [Built-in Dialogs](src/Dialogs) are available and very customizable.
- [Predefined buttons](src/Buttons) can be used with built-in dialogs or custom application dialogs.

## Creating a Custom Dialog

The dialog is not restricted to the built-in dialogs. This extension can also use custom defined dialogs and / or buttons.

- Create your own dialog using [IDialog](src/Interfaces/IDialog.cs) or [DialogBase](src/Dialogs/DialogBase.cs)
- Create your own buttons using [IButtons](src/Interfaces/IButtons.cs)

### Dialog Class

    public class CustomDialog : DialogBase
    {
        public string SubTitle { get; set; } = "Test Subtitle";
    }

### Create Custom Dialog XAML

Create a DataTemplate in a ResourceDictionary.

    <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes"
                    xmlns:wpfMaterialWpfTest="clr-namespace:wpf_material_wpfTest"
                    xmlns:forms="clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms">
        <DataTemplate DataType="{x:Type wpfMaterialWpfTest:CustomDialog}">
        ...
        </DataTemplate>
     </ResourceDictionary>

### Call Custom Dialog Class

    var dialog = await new CustomDialog
            {
                SubTitle =
                    "This is a custom dialog. It can use regular dialog parameters or custom parameters, colors, buttons or reuse parts of the built-in dialog.",
            }.ShowDialog();

## [License - MIT](./License)
