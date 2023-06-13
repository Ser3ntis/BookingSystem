using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemFormApp.UnitTests
{
    internal class EditAppointmentTest : UnitTest
    {
        public void TestEdit()
        {
            //make an appointment
            DateTime date = new DateTime(2022, 1, 1, 1, 0, 0);
            int customerID = Customer.InsertCustomer("test", "pass", "1111111111", "email");
            int serviceID = Service.InsertService("ServiceName", "desc", 1);
            int adminID = Admin.InsertAdmin("name", "pass", "1111111111", "email");
            int appointmentID = Appointment.InsertAppointment(customerID, adminID, -1, serviceID.ToString(), date);

            

            //view the appointment
            AppointmentView test = new AppointmentView();
            test.startUp();
            test.setDate(date);
            List<String> results = test.stringData();

            //test what is seen
            AssertEquals(results[0], $"test | name | {serviceID} | 1/1/2022 1:00:00 AM | True | ");

            //change the appointment
            Admin.AlterAdmin(adminID.ToString(), "newName", "pass", "1111111111", "email", false);
            Appointment.ChangeAppointment(customerID, adminID, -1, serviceID.ToString(), date, appointmentID, false);

            //Try again
            results = test.stringData();
            AssertNotEquals(results[0], $"test | name | {serviceID} | 1/1/2022 1:00:00 AM | True | ");


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
