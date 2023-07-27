using FluentAssertions;
using MaterialDesignThemes.Wpf;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using wpf_material_dialogs.Dialogs;
using wpf_material_dialogs.Enums;
using wpf_material_dialogs.Interfaces;

namespace wpf_material_dialogs.test
{
    [NUnit.Framework.Ignore("base class")]
    internal class DialogTestBase<TDialog> : IDisposable where TDialog : IDialog, new()
    {
        private DialogHost dialogHost;
        private IDialog testDialog;

        [SetUp]
        public void SetupTests()
        {
            var mi = typeof(DialogHost).GetMethod("GetInstance", BindingFlags.NonPublic | BindingFlags.Static);
            try
            {
                dialogHost = mi.Invoke(null, new object[] { null }) as DialogHost;
            }
            catch
            {
                // ignore
            }

            dialogHost ??= new DialogHost();
            dialogHost?.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            //_dialogHost.Identifier = hostId;
            testDialog = new TDialog
            {
                Title = "Test Title",
                Text = "Test Text",
                DialogButtons = DialogButton.AbortRetryIgnoreButtons,
                IconBrush = Brushes.Green,
                IconKind = PackIconKind.About,
                ShowIcon = true,
            };
        }

        public void Dispose() => dialogHost?.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent));

        [Test]
        public void CanOpenAndCloseDialogWithShowDialogIsOpen()
        {
            dialogHost.IsOpen = false;
            Assert.False(dialogHost.IsOpen);
            testDialog.ShowDialog();
            var session = dialogHost.CurrentSession;
            session?.IsEnded.Should().BeFalse();
            dialogHost.IsOpen = false;
            dialogHost.IsOpen.Should().BeFalse();
            dialogHost.CurrentSession.Should().BeNull();
            session?.IsEnded.Should().BeTrue();
        }

        [Test]
        public async Task CanOpenAndCloseDialogWithShowMethodDialogResult()
        {
            var id = Guid.NewGuid();
            dialogHost.Identifier = id;

            var result = await OpenTestDialog(testDialog, (sender, args) => args.Session.Close(DialogResult.Ignore));

            result.Should().Be(DialogResult.Ignore);
            dialogHost.IsOpen.Should().BeFalse();
        }

        [Test]
        public async Task CanOpenAndCloseDialogWithShowMethodAnyObject()
        {
            var result = await OpenTestDialogObject(testDialog, (sender, args) => args.Session.Close(42));

            result.Should().Be(42);
            dialogHost.IsOpen.Should().BeFalse();
        }

        [Test]
        public async Task CanOpenDialogWithShowMethodAndCloseWithIsOpen()
        {
            var result = await OpenTestDialogObject(testDialog, (sender, args) => dialogHost.IsOpen = false);

            result.Should().BeNull();
            dialogHost.IsOpen.Should().BeFalse();
        }

        [Test]
        public async Task CanCloseDialogWithRoutedEvent()
        {
            var closeParameter = Guid.NewGuid();
            var showTask = OpenTestDialogObject(testDialog);

            var session = dialogHost.CurrentSession;
            Assert.False(session?.IsEnded ?? false);

            DialogHost.CloseDialogCommand.Execute(closeParameter, dialogHost);
            Thread.Sleep(200);
            Assert.False(dialogHost.IsOpen);
            Assert.Null(dialogHost.CurrentSession);
            Assert.True(session?.IsEnded ?? false);
            Task.WaitAll(showTask);
            Assert.AreEqual(closeParameter, await showTask);
        }

        [Test]
        public async Task DialogHostExposesSessionAsProperty() =>
            await OpenTestDialogObject(testDialog,
                                       (sender, args) =>
                                       {
                                           Assert.True(ReferenceEquals(args.Session, dialogHost.CurrentSession));
                                           args.Session.Close();
                                       });

        [Test]
        public async Task WhenNoIdentifierIsSpecifiedItUsesSingleDialogHost()
        {
            var isOpen = false;
            await OpenTestDialogObject(testDialog,
                                       (sender, args) =>
                                       {
                                           isOpen = dialogHost.IsOpen;
                                           args.Session.Close();
                                       });
            isOpen.Should().BeTrue();
        }

        [Test]
        public async Task WhenContentIsUpdatedClosingEventHandlerIsInvoked()
        {
            var closeInvokeCount = 0;

            void ClosingHandler(object s, DialogClosingEventArgs e)
            {
                closeInvokeCount++;

                if (closeInvokeCount == 1)
                {
                    e.Cancel();
                }
            }

            var dialogTask = OpenTestDialogObject(testDialog, null, ClosingHandler);
            dialogHost.CurrentSession?.Close("FirstResult");
            dialogHost.CurrentSession?.Close("SecondResult");
            var result = await dialogTask;

            Assert.AreEqual("SecondResult", result);
            Assert.AreEqual(2, closeInvokeCount);
        }

        [Test]
        public void WhenOpenDialogsAreOpenIsExist()
        {
            var isExist = false;
            OpenTestDialogObject(testDialog, (sender, arg) => isExist = DialogHost.IsDialogOpen(dialogHost.Identifier));
            Assert.True(isExist);
            DialogHost.Close(dialogHost.Identifier);
            Assert.False(DialogHost.IsDialogOpen(dialogHost.Identifier));
        }

        private async Task<object> OpenTestDialogObject(IDialog dialog, DialogOpenedEventHandler openedAction = null, DialogClosingEventHandler closingAction = null)
        {
            //var id = Guid.NewGuid();
            //_dialogHost.Identifier = id;
            return await dialog.ShowDialogObject(dialogHost.Identifier, openedAction, closingAction);
        }

        private async Task<DialogResult> OpenTestDialog(IDialog dialog, DialogOpenedEventHandler openedAction = null, DialogClosingEventHandler closingAction = null)
        {
            //var id = Guid.NewGuid();
            //_dialogHost.Identifier = id;

            return await dialog.ShowDialog(dialogHost.Identifier, openedAction, closingAction);
        }
    }

    [Apartment(ApartmentState.STA)]
    internal class AlertDialogTests : DialogTestBase<AlertDialog>
    {
    }

    [Apartment(ApartmentState.STA)]
    internal class ErrorDialogTests : DialogTestBase<ErrorDialog>
    {
    }

    [Apartment(ApartmentState.STA)]
    internal class InfoDialogTests : DialogTestBase<InfoDialog>
    {
    }

    [Apartment(ApartmentState.STA)]
    internal class WarningDialogTests : DialogTestBase<WarningDialog>
    {
    }
}
