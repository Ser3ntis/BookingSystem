using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BookingSystemFormApp.UnitTests;
using Microsoft.CodeAnalysis;


namespace BookingSystemFormApp
{
    internal class UnitTestDriver
    {
        private ArrayList failures = new();
        private ArrayList tests = new();

        /// <summary>
        /// Utility method to easily wire in the tests.
        /// </summary>
        private void LoadTests()
        {
            //tests.Add(new SecurityTest());
            tests.Add(new AddCustomerTest());
            tests.Add(new AddScheduleTest());
            tests.Add(new UpcomingAppointmentTest());
            tests.Add(new EditAppointmentTest());
            tests.Add(new EditEmployeeTest()); 
            tests.Add(new TimeButtonFormTest());
            tests.Add(new GenerateTransactionTest());
        }





        /// <summary>
        /// Runs all currently wired in tests and displays all errors (if any).
        /// </summary>
        public void RunAllTests()
        {
            failures = new();
            tests = new ArrayList();
            LoadTests();

            /*
            string directoryPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\UnitTests\";

            MessageBox.Show(directoryPath);
            
            string[] unitTests = Directory.GetFiles(directoryPath, "*.cs");


            var types = unitTests
                           .SelectMany(cs => Assembly.LoadFrom(cs).GetExportedTypes())
                           .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(UnitTest)));
            */


            foreach (UnitTest test in tests)
            {
                try 
                {
                    ArrayList curFailures = test.RunAllTests();

                    if (curFailures.Count > 0)  
                    {
                        failures.Add(curFailures);
                    }
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (failures.Count == 0)
                MessageBox.Show("All Tests Passed Successfully.");

            else
            {
                foreach(ArrayList curFailures in failures)
                { 
                    DisplayFailures(curFailures); 
                }
                
            }

 


        }

        /// <summary>
        /// Utility method used to conviniently stitch and display multiple test failures at once.
        /// </summary>
        /// <param name="failures"></param>
        private void DisplayFailures(ArrayList failures)
        {
            string output = "";
            foreach (string str in failures)
                output += (str + "\n");


            MessageBox.Show(output);
        }

    }


}
