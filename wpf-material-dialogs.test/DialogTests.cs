using FluentAssertions;
using MaterialDesignThemes.Wpf;
using NUnit.Framework;
using System;
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
    [Apartment(ApartmentState.STA)]
    public class DialogTestBase<TDialog> : IDisposable where TDialog : IDialog, new()
    {
        private DialogHost _dialogHost;
        private IDialog _testDialog;

        [SetUp]
        public void SetupTests()
        {
            _dialogHost = new DialogHost();
            _dialogHost.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            _testDialog = new TDialog
            {
                Title = "Test Title",
                Text = "Test Text",
                DialogButtons = DialogButton.AbortRetryIgnoreButtons,
                IconBrush = Brushes.Green,
                IconKind = PackIconKind.About,
                ShowIcon = true,
            };
        }

        public void Dispose() => _dialogHost.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent));

        [Test]
        public void CanOpenAndCloseDialogWithShowDialogIsOpen()
        {
            _dialogHost.IsOpen = false;
            Assert.False(_dialogHost.IsOpen);
            _testDialog.ShowDialog();
            var session = _dialogHost.CurrentSession;
            session?.IsEnded.Should().BeFalse();
            _dialogHost.IsOpen = false;
            _dialogHost.IsOpen.Should().BeFalse();
            _dialogHost.CurrentSession.Should().BeNull();
            session?.IsEnded.Should().BeTrue();
        }

        [Test]
        public async Task CanOpenAndCloseDialogWithShowMethodDialogResult()
        {
            var id = Guid.NewGuid();
            _dialogHost.Identifier = id;

            var result = await OpenTestDialog(_testDialog, (sender, args) => args.Session.Close(DialogResult.Ignore));

            result.Should().Be(DialogResult.Ignore);
            _dialogHost.IsOpen.Should().BeFalse();
        }

        [Test]
        public async Task CanOpenAndCloseDialogWithShowMethodAnyObject()
        {
            var result = await OpenTestDialogObject(_testDialog, (sender, args) => args.Session.Close(42));

            result.Should().Be(42);
            _dialogHost.IsOpen.Should().BeFalse();
        }

        [Test]
        public async Task CanOpenDialogWithShowMethodAndCloseWithIsOpen()
        {
            var result = await OpenTestDialogObject(_testDialog, (sender, args) => _dialogHost.IsOpen = false);

            result.Should().BeNull();
            _dialogHost.IsOpen.Should().BeFalse();
        }

        [Test]
        public async Task CanCloseDialogWithRoutedEvent()
        {
            var closeParameter = Guid.NewGuid();
            var showTask = OpenTestDialogObject(_testDialog);

            var session = _dialogHost.CurrentSession;
            Assert.False(session?.IsEnded);

            DialogHost.CloseDialogCommand.Execute(closeParameter, _dialogHost);

            Assert.False(_dialogHost.IsOpen);
            Assert.Null(_dialogHost.CurrentSession);
            Assert.True(session?.IsEnded);
            Assert.AreEqual(closeParameter, await showTask);
        }

        [Test]
        public async Task DialogHostExposesSessionAsProperty() =>
            await OpenTestDialogObject(_testDialog,
                                       (sender, args) =>
                                       {
                                           Assert.True(ReferenceEquals(args.Session, _dialogHost.CurrentSession));
                                           args.Session.Close();
                                       });

        [Test]
        public async Task WhenNoIdentifierIsSpecifiedItUsesSingleDialogHost()
        {
            var isOpen = false;
            await OpenTestDialogObject(_testDialog,
                                       (sender, args) =>
                                       {
                                           isOpen = _dialogHost.IsOpen;
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

            var dialogTask = OpenTestDialogObject(_testDialog, null, ClosingHandler);
            _dialogHost.CurrentSession?.Close("FirstResult");
            _dialogHost.CurrentSession?.Close("SecondResult");
            var result = await dialogTask;

            Assert.AreEqual("SecondResult", result);
            Assert.AreEqual(2, closeInvokeCount);
        }

        [Test]
        public void WhenOpenDialogsAreOpenIsExist()
        {
            var isExist = false;
            OpenTestDialogObject(_testDialog, (sender, arg) => isExist = DialogHost.IsDialogOpen(_dialogHost.Identifier));
            Assert.True(isExist);
            DialogHost.Close(_dialogHost.Identifier);
            Assert.False(DialogHost.IsDialogOpen(_dialogHost.Identifier));
        }

        private async Task<object> OpenTestDialogObject(IDialog dialog, DialogOpenedEventHandler openedAction = null, DialogClosingEventHandler closingAction = null)
        {
            var id = Guid.NewGuid();
            _dialogHost.Identifier = id;

            return await dialog.ShowDialogObject(id, openedAction, closingAction);
        }

        private async Task<DialogResult> OpenTestDialog(IDialog dialog, DialogOpenedEventHandler openedAction = null, DialogClosingEventHandler closingAction = null)
        {
            var id = Guid.NewGuid();
            _dialogHost.Identifier = id;

            return await dialog.ShowDialog(id, openedAction, closingAction);
        }
    }

    public class AlertDialogTests : DialogTestBase<AlertDialog>
    {
    }

    public class ErrorDialogTests : DialogTestBase<ErrorDialog>
    {
    }

    public class InfoDialogTests : DialogTestBase<InfoDialog>
    {
    }

    public class WarningDialogTests : DialogTestBase<WarningDialog>
    {
    }
}


