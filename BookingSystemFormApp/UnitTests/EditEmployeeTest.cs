using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemFormApp.UnitTests
{
    internal class EditEmployeeTest : UnitTest
    {
        public void TestEdit()
        {
            //Make Variables
            string name = "AdminTest";
            string pass = "secret";
            string phone = "7064730242";
            string email = "bennyboi225@gmail.com";
            int adminID = Admin.InsertAdmin(name, pass, phone, email); //Inserts customer to database

            //Get Data from database
            string[] columns = new string[] { "AdminID", "Name", "Phone", "Email", "Password" };
            List<List<Object>> allinfo = new List<List<Object>>();
            DBManager.Functions.ReadSQLData(columns, $"Select AdminID, Name, Phone, Email, Password From Admin Where AdminID = \'{adminID}\'", allinfo);

            //Assert
            Object[] checkarr = allinfo[0].ToArray();
            AssertNotNull(checkarr[0]);
            AssertEquals((string)checkarr[1], name);
            AssertEquals((string)checkarr[2], phone);
            AssertEquals((string)checkarr[3], email);
            AssertEquals((string)checkarr[4], pass);

            //Edit the admin
            name = "AdminTestEdit";
            pass = "secretEdit";
            phone = "7064730241";
            email = "bennyboi225Edit@gmail.com";
            bool notTest = false;
            Admin.AlterAdmin(adminID.ToString(), name, pass, phone, email, notTest);

            //Get Data again
            allinfo = new List<List<Object>>();
            DBManager.Functions.ReadSQLData(columns, $"Select AdminID, Name, Phone, Email, Password From Admin Where AdminID = \'{adminID}\'", allinfo);

            //Assert
            checkarr = allinfo[0].ToArray();
            AssertNotNull(checkarr[0]);
            AssertEquals((string)checkarr[1], name);
            AssertEquals((string)checkarr[2], phone);
            AssertEquals((string)checkarr[3], email);
            AssertEquals((string)checkarr[4], pass);

            //Clean up database
            using (SqlConnection con = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Admin WHERE adminID = " + adminID.ToString(), con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
