﻿using System.Threading;

namespace wpf_material_dialogs.test.AvalonUnitTesting
{
    /// <summary>
    /// base class for unit test of wpf
    /// </summary>
    public class AvalonTestRunner
    {
        /// <summary>
        /// Runs a set of tests for data binding
        /// </summary>
        /// <param name="objectToTest"></param>
        public static void RunDataBindingTests(object objectToTest)
        {
            //get the window instance. This will create a wrapper window if a window is not passes as parameter
            var windowToTest = AvalonActivatorHelper.VerifyObjectType(objectToTest);
            if (windowToTest != null)
                AvalonDataBindingTraceTester.TestDataBindingsForObject(windowToTest);
        }

        private static readonly STAOperationRunner runner = new();

        /// <summary>
        /// Runs a delegate in a STA thread
        /// </summary>
        /// <param name="userDelegate">The operation to run</param>
        public static void RunInSTA(ThreadStart userDelegate)
        {
            runner.RunInSTA(userDelegate);
        }
    }
}
