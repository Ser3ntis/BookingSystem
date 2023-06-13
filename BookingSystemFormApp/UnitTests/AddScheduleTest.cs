using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

//Date: 4/16/23
//Author: Sam Reissing
//Unit test for SchedulingForm

namespace BookingSystemFormApp.UnitTests
{
    internal class AddScheduleTest : UnitTest
    {
        private object sender;
        private EventArgs e;

        public void NotTestAddSchedule()
        {
            SchedulingForm.ScheduleFormMaker sfm = new SchedulingForm.ScheduleFormMaker();

            AssertEquals(sfm.getSunday(), DateTime.Now.ToString("yyyy-MM-dd"));
            sfm.day = DateTime.Today.AddDays(-1);
            sfm.testID = 1;

            //will need to be changed to employeeID in local database
            AssertTrue(sfm.employeeExists(482040087));

            sfm.addWeekButton_Click(sender, e);

            sfm.tBoxArr[0].Text = "3000-01-01";
            sfm.cBoxArr[0].Text = "0000";
            sfm.cBoxArr[1].Text = "0100";
            sfm.cBoxArr[2].Text = "0200";

            sfm.saveButton_Click(sender, e);

            if (!scheduleExists(sfm.tBoxArr[0].Text))
            {
                MessageBox.Show("Schedule for current date does not exist");
                Fail();
            }

            //sfm.deleteExisting(sfm.tBoxArr[0].Text);

            if (scheduleExists(sfm.tBoxArr[0].Text))
            {
                MessageBox.Show("Schedule for current date does not exist");
                Fail();
            }
        }

        public Boolean scheduleExists(string date)
        {
            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "SELECT * FROM SCHEDULE WHERE Date = @date";
                conn.Open();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@date", SqlDbType.Date).Value = date;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            conn.Close();
                            return true;
                        }
                        else
                        {
                            conn.Close();
                            return false;
                        }
                    }
                }
            }
        }
    }
}
