using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemFormApp.UnitTests
{
    internal class TimeButtonFormTest : UnitTest
    {
        public void TestTimeButtonForm()
        {
            Admin.InsertAdmin("TimeButtonTest", "pass", "phone", "email");
            Customer.InsertCustomer("TimeButtonTest", "pass", "phone", "email");
            AssertNotEquals(false, Appointment.TimeButtonDisable(DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, "TimeButtonTest").ToString(), "2000-01-01", "0000"));

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "INSERT INTO SCHEDULE (EmployeeID, Date, Times)" + " VALUES (@EmployeeID, @Date, @Times)";

                conn.Open();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, "TimeButtonTest").ToString();
                    command.Parameters.Add("@Date", SqlDbType.NVarChar).Value = DateTime.Today.ToString("dddd");
                    command.Parameters.Add("@Times", SqlDbType.NVarChar).Value = "1000,1200,1400";

                    command.ExecuteNonQuery();
                }

                conn.Close();
            }

            Appointment.InsertAppointment(DBManager.Functions.GetIDByName(DBManager.UserType.CUSTOMER, DBManager.Table.CUSTOMER, "TimeButtonTest"), DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, "TimeButtonTest"), 0, "0", DateTime.Parse("3000-01-01 10:00"));

            int[] times;

            times = Appointment.GetTimeArray(DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, "TimeButtonTest"), DateTime.Today.ToString("yyyy-MM-dd"));

            AssertEquals(times[0], 1000);
            AssertEquals(times[1], 1030);
            AssertEquals(times[2], 1100);

            AssertEquals(true, Appointment.TimeButtonDisable(DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, "TimeButtonTest").ToString(), "3000-01-01", "1000"));

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                conn.Open();

                string query = "DELETE FROM ADMIN WHERE Name = @name";
                string query1 = "DELETE FROM CUSTOMER WHERE Name = @name1";
                string query2 = "DELETE FROM Schedule WHERE EmployeeID = @empID";

                using (SqlCommand command = new SqlCommand(query2, conn))
                {
                    command.Parameters.Add("@empID", SqlDbType.NVarChar).Value = DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, "TimeButtonTest").ToString();
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = "TimeButtonTest";
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(query1, conn))
                {
                    command.Parameters.Add("@name1", SqlDbType.NVarChar).Value = "TimeButtonTest";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
