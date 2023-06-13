using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookingSystemFormApp.DBManager;

namespace BookingSystemFormApp.UnitTests
{
    internal class AddCustomerTest : UnitTest
    {
        /// <summary>
        /// <br>Test function to Insert a customer, Read that customer back and assert that everything is the same.</br>>
        /// <br>Author: Clay Brown</br>
        /// </summary>
        public void TestCustomer()
        {
            //Make Variables
            string name = "CustomerTest";
            string pass = "secret";
            string phone = "7064730242";
            string email = "coyotekid111@gmail.com";
            int customerID = Customer.InsertCustomer(name, pass, phone, email); //Inserts customer to database

            //Get Data from database
            string[] columns = new string[] { "CustomerID", "Name", "Phone", "Email", "Password"  };
            List<List<Object>> allinfo = new List<List<Object>>();
            DBManager.Functions.ReadSQLData(columns, $"Select CustomerID, Name, Phone, Email, Password From Customer Where CustomerID = \'{customerID}\'", allinfo);
            
            //Assert
            Object[] checkarr = allinfo[0].ToArray();
            AssertNotNull(checkarr[0]);
            AssertEquals((string)checkarr[1], name);
            AssertEquals((string)checkarr[2], phone);
            AssertEquals((string)checkarr[3], email);
            AssertEquals((string)checkarr[4], pass);

            //Clean up database
            Customer.DeleteCustFromDB(checkarr[0]);
        }
    }
}
