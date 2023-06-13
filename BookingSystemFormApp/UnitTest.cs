using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BookingSystemFormApp
{
    /// <summary>
    /// Author: Timur Maistrenko 
    /// 
    /// <br></br>
    /// 
    /// !!!WIP!!!
    /// 
    /// <br></br>
    /// 
    /// Simulates the Functionality of JUnit Tests
    /// </summary>
    internal abstract class UnitTest
    {
        ArrayList failures = new();

        private void ShowMessage(string message,  int lineNumber)
        {
            MessageBox.Show(message + " at line " + lineNumber);
        }

        /// <summary>
        /// Runs all test functions snd prints out all failed Assert calls.
        /// </summary>
        public ArrayList RunAllTests()
        {
            failures = new();

            var methods = GetType()
               .GetMethods()
               .Where(item => item.Name.StartsWith("Test"));

            foreach (var method in methods)
            {
                try
                {
                    method.Invoke(this, new Object[0]);
                }
                catch (Exception fup)
                {
                    MessageBox.Show(fup.StackTrace);
                }
            }

            return failures;
               
            
        }

        #region Assert Statements
        protected bool AssertEquals(object obj1, object obj2, [CallerMemberName] string callingMethod = "",[CallerLineNumber] int lineNumber = 0)
        {
            if (obj1.Equals(obj2)) return true;

            failures.Add($"AssertEquals Failed. {obj1} is not Equal to {obj2} at Line: {lineNumber} ({callingMethod} at {this.GetType().Name}).");
            return false;
        }
        protected bool AssertNotEquals(object obj1, object obj2, [CallerMemberName] string callingMethod = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (!obj1.Equals(obj2)) return true;

            failures.Add($"AssertNotEquals Failed. {obj1} is Equal to {obj2} at Line: {lineNumber} ({callingMethod} at {this.GetType().Name}).");
            return false;
        }

        protected bool AssertTrue(bool boolean, [CallerMemberName] string callingMethod = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (boolean) return true;

            failures.Add($"AssertTrue Failed. Boolean is False at Line: {lineNumber} ({callingMethod} at {this.GetType().Name}). ");
            return false;
        }

        protected bool AssertFalse(bool boolean, [CallerMemberName] string callingMethod = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (!boolean) return true;

            failures.Add($"AssertFalse Failed. Boolean is True at Line: {lineNumber} ({callingMethod} at {this.GetType().Name}).");
            return false;
        }

        protected bool AssertNull(Object obj, [CallerMemberName] string callingMethod = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (obj.Equals(null)) return true;

            failures.Add($"AssertNull Failed. Object is not Null at Line: {lineNumber} ({callingMethod} at {this.GetType().Name}).");
            return false;
        }

        protected bool AssertNotNull(Object obj, [CallerMemberName] string callingMethod = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (!obj.Equals(null)) return true;

            failures.Add($"AssertNotNull Failed. Object is  Null at Line: {lineNumber} ({callingMethod} at {this.GetType().Name}).");
            return false;
        }

        protected bool Fail([CallerMemberName] string callingMethod = "",[CallerLineNumber] int lineNumber = 0) 
        {
            failures.Add($"Auto-Fail at Line: {lineNumber} ({callingMethod} at {this.GetType().Name}).");
            return false;
        }
        #endregion
    }
}