using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemFormApp.UnitTests
{
    internal class SecurityTest : UnitTest
    {
        /// <summary>
        /// Test basic security functions' functionality.
        /// </summary>
        public void TestSecuritySalt()
        {
            // Make sure salt generation is random. Since salt is technically 16 random bytes in a row, the collision chance is practically zero.
            AssertNotNull(Security.GenerateSalt());
            AssertNotEquals(Security.GenerateSalt(), Security.GenerateSalt());
            AssertNotEquals(Security.GenerateSalt(), Security.GenerateSalt());
            AssertNotEquals(Security.GenerateSalt(), Security.GenerateSalt());

            // Make sure password hashing always produces the same result
            string pass = "password";
            string salt=Security.GenerateSalt();
            AssertNotNull(Security.PasswordHash(pass,salt));
            AssertEquals(Security.PasswordHash(pass, salt), Security.PasswordHash(pass, salt));

            AssertNotEquals(Security.PasswordHash(pass, Security.GenerateSalt()), Security.PasswordHash(pass, Security.GenerateSalt()));

            AssertNotEquals(Security.PasswordHash("pass", salt), Security.PasswordHash("1234", salt));


        }

        /// <summary>
        /// Test the DB insertion procedure and whether the data you insert is the same as the data you retrieve from the DB.
        /// </summary>
        public void TestDBInsertion()
        {
            // Sample data
            string name = "CustomerTest";
            string pass = "secret";
            string salt=Security.GenerateSalt();
            string phone = "2011234567";
            string email = "randomemail@gmail.com";
            int customerID = Customer.InsertCustomer(name, Security.PasswordHash(pass,salt), phone, email, salt); //Inserts customer to database

            //Get Data from database
            string[] columns = new string[] { "CustomerID", "Name", "Phone", "Email", "Password", "Salt" };
            List<List<Object>> allinfo = new List<List<Object>>();
            DBManager.Functions.ReadSQLData(columns, $"Select CustomerID, Name, Phone, Email, Password, Salt From Customer Where CustomerID = \'{customerID}\'", allinfo);

            //Assert
            Object[] checkarr = allinfo[0].ToArray();
            AssertNotEquals(allinfo.Count(), 0);
            AssertNotNull(checkarr[0]);
            AssertEquals((string)checkarr[1], name);
            AssertEquals((string)checkarr[2], phone);
            AssertEquals((string)checkarr[3], email);
            AssertEquals((string)checkarr[4], Security.PasswordHash(pass, salt));

           // MessageBox.Show((string)checkarr[4]);

            //Clean up database
            Customer.DeleteCustFromDB(checkarr[0]);

            allinfo = new List<List<Object>>();

            DBManager.Functions.ReadSQLData(columns, $"Select CustomerID, Name, Phone, Email, Password, Salt From Customer Where CustomerID = \'{customerID}\'", allinfo);
            AssertEquals(allinfo.Count(), 0);

        }
    }
}
