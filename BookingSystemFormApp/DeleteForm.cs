using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookingSystemFormApp
{
    public partial class DeleteForm : Form
    {
        public static DataGridViewCellEventArgs cellEvent;
        public DeleteForm()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            switch(DGV.Name)
            {
                case "Appointment":
                    Appointment.CheckDelete();
                    break;
                case "Customer":
                    Customer.CheckDelete();
                    break;
                case "Service":
                    Service.CheckDelete();
                    break;
                case "Admin":
                    Admin.CheckDelete();
                    break;
                default:
                    MessageBox.Show("Form is not handled correctly. Please contact system Administrator.");
                    break;
            }
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            cellEvent = new DataGridViewCellEventArgs(0, 0);
            cellEvent = e;
        }
    }
}
