namespace BookingSystemFormApp
{
    public partial class FormController : Form
    {
        public static readonly int storeID = -1;
        public FormController()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        AppointmentView sch = new AppointmentView();
        private void Form1_Load(object sender, EventArgs e)
        {
            sch.startUp();
            SetupUpcomingAppointmentsDGV();

        }

        #region Other Buttons

        private void Help_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.ShowDialog();
        }

        private void GenerateReceipt_Click(object sender, EventArgs e)
        {
            GenerateTransaction.GenerateTransactionForm(null);
        }
        private void Settings_Click(object sender, EventArgs e)
        {

        }

        #endregion


        #region Customer Buttons
        private void AddCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer.AddCustomerForm(true);
        }
        private void ViewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer.ViewCustomerForm("View Customers");
        }
        private void EditCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer.EditCustomer();
        }
        private void DeleteCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer.DeleteCustomer();
        }
        #endregion


        #region Service Buttons
        private void AddServiceToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Service.ServiceForm();
        }

        private void ViewServiceToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Service.ServiceViewForm();
        }

        private void EditServiceToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Service.EditService();
        }

        private void DeleteServiceToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Service.DeleteService();
        }
        #endregion


        #region Appointment Buttons 
        private void AddAppointmentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Appointment.AddNewAppointmentForm();
            UpdateUpcomingAppointments();
        }

        private void ViewAppointmentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Appointment.NewAppointmentViewForm(null);
        }

        private void EditAppointmentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Appointment.EditAppointment();
            UpdateUpcomingAppointments();
        }

        private void DeleteAppointmentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Appointment.DeleteAppointmentViewForm();
            UpdateUpcomingAppointments();
        }
        #endregion


        #region Stylist Buttons
        private void AddStylistToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Admin.AdminForm();
        }

        private void ViewStylistToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Admin.AdminViewForm();
        }

        private void EditStylistToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Admin.EditAdmin();
        }

        private void DeleteStylistToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Admin.DeleteAdmin();
        }
        #endregion


        #region ScheduleView components

        private void LookAheadComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lookAhead = lookAheadComboBox.SelectedItem as string;
            sch.setLook(lookAhead);
            UpdateUpcomingAppointments();
        }

        private void UpdateUpcomingAppointments()
        {
            string look = lookAheadComboBox.SelectedItem as string;
            sch.setLook(look);
            SetupUpcomingAppointmentsDGV();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime date = dateTimePicker1.Value;
            sch.setDate(date);
            SetupUpcomingAppointmentsDGV();
        }
        public void SetupUpcomingAppointmentsDGV()
        {
            DataGridView dgv;
            dgv = (DataGridView)this.Controls[0].Controls[1].Controls[0];
            sch.dgvData(dgv);
        }
        #endregion

        #region Unit Test
        private void runUnitTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnitTestDriver utd = new();
            utd.RunAllTests();
        }
        #endregion

        #region Schedule
        private void viewScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScheduleView scheduleView = new ScheduleView();
            scheduleView.ShowDialog();
        }

        private void setScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SchedulingForm setSchedule = new SchedulingForm();
            setSchedule.ShowDialog();
        }
        #endregion







    }
}