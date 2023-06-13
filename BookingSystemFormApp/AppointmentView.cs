using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookingSystemFormApp
{
    internal class AppointmentView
    {
        private string lookAhead;
        private DateTime date;

        public void startUp()
        {
            lookAhead = "Day";
            date = DateTime.Today;
        }

        public void setLook(string look)
        {
            lookAhead = look;
        }

        public void setDate(DateTime d)
        {
            date = d;
        }

        public List<String> stringData()
        {
            List<String> results = new List<string>();
            string resStr = "";
            List<List<Object>> temp = DBManager.Functions.getAppointments(date, lookAhead);
            for (int i = 0; i < temp.Count; i++)
            {
                for (int j = 0; j < temp[i].Count; j++)
                {
                    resStr += temp[i][j].ToString() + " | ";
                }
                results.Add(resStr);
                resStr = "";
            }
            return results;
        }
        public void dgvData(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowDrop = false;
            dgv.ReadOnly = true;
            DBManager.SetupDGV(dgv, "Appointment View");
            dgv.Dock = DockStyle.None;
            

            string[] columnNames = new string[] { "Customer", "Employee", "Services", "Date", "Pending" };
            DBManager.UpcomingAppointmentViewDGV(dgv, columnNames, DBManager.Functions.getAppointments(date, lookAhead));
        }
    }
    
}
