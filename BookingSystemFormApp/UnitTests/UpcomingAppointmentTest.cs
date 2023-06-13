using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemFormApp.UnitTests
{
    internal class UpcomingAppointmentTest : UnitTest
    {
        public void TestUpcoming()
        {
            //make appointment view
            AppointmentView test = new AppointmentView();
            test.startUp();

            //make customer
            int customerID = Customer.InsertCustomer("test", "pass", "1111111111", "email");
            int serviceID = Service.InsertService("ServiceName", "desc", 1);
            int adminID = Admin.InsertAdmin("name", "pass", "1111111111", "email");

            //calibrate the appointment view
            DateTime date = new DateTime(2022, 1, 1, 1, 0, 0);
            int appointmentID = Appointment.InsertAppointment(customerID, adminID, -1, serviceID.ToString(), date);
            test.setDate(date);

            //view appointment
            List<String> results = test.stringData();
            //test data
            AssertEquals(results[0], $"test | name | {serviceID} | 1/1/2022 1:00:00 AM | True | ");

            //delete data
            using (SqlConnection con = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Appointment WHERE appointmentID = " + appointmentID.ToString(), con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            using (SqlConnection con = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Customer WHERE customerID = " + customerID.ToString(), con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            using (SqlConnection con = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Admin WHERE adminID = " + adminID.ToString(), con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            using (SqlConnection con = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Service WHERE serviceID = " + serviceID.ToString(), con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
