using System;
using System.Reflection;
using System.Threading;

namespace wpf_material_dialogs.test.AvalonUnitTesting
{
    /// <summary>
    /// Runs a specific job in a specific thread apartment
    /// </summary>
    public class STAOperationRunner
    {
        private Exception lastException;

        /// <summary>
        /// Runs a specific method in Single Threaded apartment
        /// </summary>
        /// <param name="userDelegate">A delegate to run</param>
        public void RunInSTA(ThreadStart userDelegate)
        {
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
                Run(userDelegate, ApartmentState.STA);
            else
                userDelegate.Invoke();
        }

        private void Run(ThreadStart userDelegate, ApartmentState apartmentState)
        {
            lastException = null;

            var thread = new Thread(delegate()
                                    {
                                        try
                                        {
                                            userDelegate.Invoke();
                                        }
                                        catch (Exception e)
                                        {
                                            lastException = e;
                                        }
                                    });
            thread.SetApartmentState(apartmentState);

            thread.Start();
            thread.Join();

            if (ExceptionWasThrown())
                ThrowExceptionPreservingStack(lastException);
        }

        private bool ExceptionWasThrown()
        {
            return lastException != null;
        }

        private static void ThrowExceptionPreservingStack(Exception exception)
        {
            var remoteStackTraceString = typeof(Exception).GetField("_remoteStackTraceString",
                                                                    BindingFlags.Instance | BindingFlags.NonPublic);
            remoteStackTraceString?.SetValue(exception, exception.StackTrace + Environment.NewLine);

            throw exception;
        }
    }
}
