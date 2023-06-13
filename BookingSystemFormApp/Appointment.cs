using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using Button = System.Windows.Forms.Button;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;

namespace BookingSystemFormApp
{
    internal class Appointment
    {
        private static ComboBox stylistComboBox = new ComboBox();
        private static DateTimePicker dateTextBox = new DateTimePicker();
        private static Form availableTimes;
        private static Form appointmentForm;

        #region Delete Appointment
        /// <summary>
        /// Deletes selected entry on DGV PERMANENTLY - Confirms decision twice.
        /// </summary>
        public static void CheckDelete()
        {
            //  !!!     This method will permanently delete an entry from the database.     !!!

            DeleteForm deleteForm = (DeleteForm)Application.OpenForms["Delete Form"];
            if (DeleteForm.cellEvent == null)
            {
                MessageBox.Show("No event selected.");
            }
            else
            {
                DataGridViewRow row = deleteForm.DGV.Rows[DeleteForm.cellEvent.RowIndex];
                if (MessageBox.Show(string.Format("Do you want to delete the appointment with Customer: {0}? (AppointmentID: {1})", row.Cells["Customer Name"].Value, row.Cells["AppointmentID"].Value), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (MessageBox.Show(string.Format("Are you sure you want to permanently delete this entry from the database?"), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(DBManager.Instance.ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM Appointment WHERE AppointmentID = @AppointmentID", con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@AppointmentID", row.Cells["AppointmentID"].Value);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                //Load data and update view on form
                deleteForm.LoadDGV(new string[] { "AppointmentID", "CustomerID", "Customer Name", "Date", "Pending" }, InnerJoinStatement_DeleteAppointment());
            }
        }
        public static DialogResult DeleteAppointmentViewForm()
        {
            DeleteForm deleteForm = DeleteForm.Form;
            DBManager.SetupDGV(deleteForm.DGV, "Appointment");
            deleteForm.LoadDGV(new string[] { "AppointmentID", "CustomerID", "Customer Name", "Date", "Pending" }, InnerJoinStatement_DeleteAppointment());
            DBManager.CreateViewForm(deleteForm.DGV, deleteForm, "Delete Form");
            deleteForm.ClientSize = new Size(900, 600);
            DialogResult dialogResult = deleteForm.ShowDialog();

            return dialogResult;
        }

        private static DBManager.InnerJoinStatement InnerJoinStatement_DeleteAppointment()
        {
            return new DBManager.InnerJoinStatement("Appointment.AppointmentID, Appointment.CustomerID, Customer.Name, Appointment.Date, Appointment.Pending",
                                                    DBManager.Table.APPOINTMENT,
                                                    DBManager.Table.CUSTOMER,
                                                    null,
                                                    "Appointment.CustomerID=Customer.CustomerID",
                                                    null,
                                                    "Appointment.Pending = 1");
        }
        #endregion


        #region View Appointments
        /// <summary>
        /// Builds a form with a DataGridView object to display all pending appointments
        /// </summary>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        /*3/22/23 Commented out while trying to rebuild for Frank's specifications on looking up appointments
         * public static DialogResult AppointmentViewForm(string[]? columnNames)
        {
            if (columnNames == null)
            {
                columnNames = new string [] { "AppointmentID", "CustomerID", "EmployeeID", "StoreID", "ServiceIDs", "Date", "Pending" };
            }

            
            DataGridView dgv = new DataGridView();
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowDrop = false;
            dgv.ReadOnly = true;
            DBManager.SetupDGV(dgv, "Appointment View Form");
            DBManager.Initialize_Select_DGV(dgv, columnNames, DBManager.Functions.QueryBuilder(columnNames), DBManager.Table.APPOINTMENT);

            Form viewForm = new Form();
            DBManager.CreateViewForm(dgv, viewForm, "View Appointments");

            DialogResult dialogResult = viewForm.ShowDialog();
            return dialogResult;
        }*/
        public static DialogResult NewAppointmentViewForm(string[]? columnNames)
        {
            if (columnNames == null)
            {
                columnNames = new string[] { "AppointmentID", "Customer Name", "Employee Name", "StoreID", "ServiceIDs", "Date", "Pending" };
            }
            string customerName = "";
            DialogResult customerDialogResult = DBManager.Lookup_DisplayNameForm("Lookup Customer", out customerName);
            if (customerDialogResult == DialogResult.OK)
            {

                DataGridView dgv = new DataGridView();
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowDrop = false;
                dgv.ReadOnly = true;
                DBManager.SetupDGV(dgv, "Appointment View Form");
                DBManager.Initialize_InnerJoin_DGV(dgv, columnNames, new DBManager.InnerJoinStatement("Appointment.AppointmentID, Customer.Name, Admin.Name, Appointment.StoreID, Appointment.ServiceIDs, Appointment.Date, Appointment.Pending",
                                                        DBManager.Table.APPOINTMENT,
                                                        DBManager.Table.CUSTOMER,
                                                        DBManager.Table.ADMIN,
                                                        "Appointment.CustomerID=Customer.CustomerID",
                                                        "Appointment.EmployeeID = Admin.AdminID",
                                                        "Appointment.Pending = 1 AND Customer.Name LIKE \'" + customerName + "%\';"));

                Form viewForm = new Form();
                DBManager.CreateViewForm(dgv, viewForm, "View Appointments");

                DialogResult dialogResult = viewForm.ShowDialog();
                return dialogResult;
            }
            return DialogResult.Abort;
        }



        #endregion

        #region Edit Appointments

        public static void EditAppointment()
        {
            string customerName = "";
            DialogResult appointmentDialogResult = DBManager.Lookup_DisplayNameForm("Lookup Appointment", out customerName);
            string whereCondition = "CustomerID LIKE \'" + DBManager.Functions.GetIDByName(DBManager.UserType.CUSTOMER, DBManager.Table.CUSTOMER, customerName) + "%\'";
            if (appointmentDialogResult == DialogResult.OK)
            {
                /*
                 * !!! Design Choice !!!
                 * We could use this button or just have user double click the row they want to change.
                 * Double click is currently implemented 4/10/2023
                
                //Make button for editing selected entry
                Button editButton = new Button();
                //editButton.Click += new EventHandler(EditButton_Click);
                editButton.Dock = DockStyle.Bottom;
                editButton.ClientSize = new Size(150, 50);
                editButton.Text = "Edit Selected Customer";
                */

                //Set up the DataGridView to display Database Entries
                DataGridView dgv = new DataGridView();
                DBManager.SetupDGV(dgv, "Edit Appointment");
                string[] columnNames = new string[] { "AppointmentID", "Customer Name", "Employee Name", "StoreID", "ServiceIDs", "Date", "Pending" };

                DBManager.Initialize_InnerJoin_DGV(dgv, columnNames, new DBManager.InnerJoinStatement("Appointment.AppointmentID, Customer.Name, Admin.Name, Appointment.StoreID, Appointment.ServiceIDs, Appointment.Date, Appointment.Pending",
                                                                        DBManager.Table.APPOINTMENT,
                                                                        DBManager.Table.CUSTOMER,
                                                                        DBManager.Table.ADMIN,
                                                                        "Appointment.CustomerID=Customer.CustomerID",
                                                                        "Appointment.EmployeeID = Admin.AdminID",
                                                                        "Customer.Name LIKE \'" + customerName + "%\';"));
                dgv.Dock = DockStyle.Fill;
                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.Columns[3].FillWeight = 150;
                dgv.CellDoubleClick += new DataGridViewCellEventHandler(EditButton_Click);

                //Create the Edit Customer form to hold the DataGridView
                Form form = new Form();
                form.Name = "Edit Appointment";
                form.Text = "Edit Appointment";
                //form.ClientSize = new Size(dgv.ColumnCount * 150, dgv.RowCount * 25);
                form.ClientSize = new Size(700, 450);
                form.FormBorderStyle = FormBorderStyle.Sizable;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.Controls.AddRange(new Control[] { dgv });
                form.ShowDialog();
            }
        }


        /// <summary>
        /// Handles a Data Grid View Cell Event <br></br>
        /// Calls another method to handle the edit
        /// 
        /// <br></br>
        /// Author: Clay Brown
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EditButton_Click(object? sender, DataGridViewCellEventArgs e)
        {
            if (e == null)
            {
                MessageBox.Show("No event selected.");
            }
            else
            {
                //Reference the open Edit form
                Form editForm = Application.OpenForms["Edit Appointment"];

                //Get the data grid view from the control array of the form and find the row at which the event occured
                DataGridView dgv = (DataGridView)editForm.Controls[0];
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                //Get the name of the customer on that row
                string appointmentDate = (string)row.Cells["Date"].Value;

                //Ask user to confirm the selection
                if (MessageBox.Show(string.Format("Do you want to edit appointment \"{0}\" (AppointmentID: {1})", row.Cells["Date"].Value, row.Cells["AppointmentID"].Value), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int AppointmentID = int.Parse((string)row.Cells["AppointmentID"].Value);
                    string customerName = (string)row.Cells["Customer Name"].Value;
                    string adminName = (string)row.Cells["Employee Name"].Value;
                    string serviceIDs = (string)row.Cells["serviceIDs"].Value;
                    string date = (string)row.Cells["Date"].Value;
                    bool pendingValue;
                    if (row.Cells["Pending"].Value.Equals("True"))
                        pendingValue = true;
                    else
                        pendingValue = false;
                    DialogResult dialogResult = ChangeAppointmentForm(AppointmentID, customerName, adminName, serviceIDs, pendingValue, date);

                    
                }
            }
        }
        public static DialogResult ChangeAppointmentForm(int appointmentID, string customerName, string adminName, string serviceIDs, bool pendingValue, string date)
        {
            //Position Variables
            int xPos = 50;
            int yPos_Label = 15;
            int yPos_Text = 30;
            int offset = 40;

            #region UI Components
            //  ---     Customer UI             ---
            Label customerNameLabel;
            TextBox customerNameTextBox;
            UIController.Label_TextBox(out customerNameLabel, out customerNameTextBox, "Customer Name:", 0);
            customerNameTextBox.Text = customerName;
            //  ---                             ---

            //  ---     Date UI                 ---
            Label dateLabel = new Label();
            UIController.Label_Date(out dateLabel, out dateTextBox, "Date:", 1);
            dateTextBox.CustomFormat = "yyyy-MM-dd hh:mm tt";
            dateTextBox.Format = DateTimePickerFormat.Custom;
            dateTextBox.Value = DateTime.Parse(date);
            dateTextBox.ValueChanged += DateTextBox_ValueChanged;
            dateTextBox.DropDown += DateTextBox_DropDown;
            //  ---                             ---

            //  ---     Stylist UI              ---
            Label stylistNameLabel;
            UIController.Label_ComboBox_Stylists(out stylistNameLabel, out stylistComboBox, 2);
            stylistComboBox.SelectedIndexChanged += StylistComboBox_SelectedIndexChanged;
            stylistComboBox.Text = adminName;
            //  ---                             ---

            //  ---     Service UI              ---
            Label serviceNamesLabel;
            CheckedListBox servicesCheckedListBox;
            UIController.Label_CheckedListBox_Services(out serviceNamesLabel, out servicesCheckedListBox, 3);

            string[] ids= serviceIDs.Split(", ");

            foreach (string item in ids)
            {
                string name = ServiceItems.GetNameByID(Int32.Parse(item));
                if (servicesCheckedListBox.Items.Contains(name))
                    servicesCheckedListBox.SetItemChecked(servicesCheckedListBox.Items.IndexOf(name), true);
            }

            //  ---                             ---


            //  ---     Pending checkbox        ---
            Label pendingLabel = new Label();
            CheckBox pendingBox = new CheckBox();
            UIController.Label_CheckBox(out pendingLabel, out pendingBox, "Pending:", 6);
            pendingBox.Checked = pendingValue;

            //  ---     Control Buttons         ---
            Button submitButton = new Button();
            Button cancelButton = new Button();
            UIController.ControlButtons(out submitButton, out cancelButton, 7);
            //  ---                             ---
            #endregion

            appointmentForm = new Form();
            appointmentForm.Text = "Alter Appointment";
            appointmentForm.ClientSize = new Size(350, 375);
            appointmentForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            appointmentForm.StartPosition = FormStartPosition.CenterScreen;
            appointmentForm.MinimizeBox = false;
            appointmentForm.MaximizeBox = false;
            appointmentForm.Controls.AddRange(new Control[] { customerNameLabel, customerNameTextBox, stylistNameLabel, stylistComboBox, serviceNamesLabel, servicesCheckedListBox, pendingLabel, pendingBox, dateLabel, dateTextBox, submitButton, cancelButton });
            appointmentForm.AcceptButton = submitButton;
            appointmentForm.CancelButton = cancelButton;
            DialogResult dialogResult = appointmentForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {

                string newServiceIDs = "";
                var checkedItems = servicesCheckedListBox.CheckedItems;

                if (checkedItems.Count == 0)
                    MessageBox.Show("No Items Selected");
                else
                {
                    for (int i = 0; i < checkedItems.Count; i++)
                    {
                        int id = DBManager.Functions.GetIDByName(DBManager.UserType.SERVICE, DBManager.Table.SERVICE, checkedItems[i].ToString());

                        if (i == checkedItems.Count - 1)
                            newServiceIDs += id;
                        else
                            newServiceIDs += (id + ", ");
                    }

                    ChangeAppointment(DBManager.Functions.GetIDByName(DBManager.UserType.CUSTOMER, DBManager.Table.CUSTOMER, customerNameTextBox.Text), DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, stylistComboBox.Text), FormController.storeID, newServiceIDs, DateTime.Parse(dateTextBox.Text), appointmentID, pendingBox.Checked);
                }
            }
            //Close the form --> So I don't have to refresh it :P
            Application.OpenForms["Edit Appointment"].Close();
            appointmentForm = null;
            return dialogResult;
        }

        public static void ChangeAppointment(int customerID, int employeeID, int storeID, string serviceIDs, DateTime date, int appointmentID, bool pending)
        {
            //  --- Generate AdminID ---        Needs to be reworked: Issue -> May not return a unique number => Maybe use GUID?
            

            //TESTING SQL INJECTION PROTECTION
            //string name = "Generic); Delete From dbo.admin Where \"Name\" = 'Generic';GO "; //SQL INJECTION

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {

                string query = "UPDATE Appointment SET CustomerID = @CustomerID, EmployeeID = @EmployeeID, StoreID = @StoreID," +
                               " ServiceIDs = @ServiceIDs, Date = @Date, Pending = @Pending" +
                               " WHERE appointmentID = @AppointmentID";
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@AppointmentID", appointmentID);
                        command.Parameters.AddWithValue("@CustomerID", customerID);
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);
                        command.Parameters.AddWithValue("@StoreID", storeID);
                        command.Parameters.AddWithValue("@ServiceIDs", serviceIDs);
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@Pending", pending);
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException sqlException)
                    {
                        MessageBox.Show("\t\t\t------ERROR!------ \n\t" +
                                        "An SQL Exception has occured!\n\tPlease contact your system" +
                                        " administrator for assistance." +
                                        "\n\t\t\t------ERROR!------\n\n\t" +
                                        "-------------------------------------------------------------------------\n\n" +
                                        sqlException.ToString());
                    }
                }
            }
        }
        #endregion

        #region Add Appointment

        /// <summary>
        /// Builds a form to prompt user to enter data relavent to an Appointment
        /// </summary>
        /// <returns></returns>
        public static DialogResult AddNewAppointmentForm()
        {
            //Position Variables
            int xPos = 50;
            int yPos_Label = 10;
            int yPos_Text = 30;
            int offset = 40;

            #region UI Components
            //  ---     Customer UI             ---
            Label customerNameLabel;
            TextBox customerNameTextBox;
            UIController.Label_TextBox(out customerNameLabel, out customerNameTextBox, "Customer Name:", 0);
            //  ---                             ---


            //  ---     Date UI                 ---
            Label dateLabel = new Label();
            UIController.Label_Date(out dateLabel, out dateTextBox, "Date:", 1);
            dateTextBox.CustomFormat = "yyyy-MM-dd hh:mm tt";
            dateTextBox.Format = DateTimePickerFormat.Custom;
            dateTextBox.Value = DateTime.Now.Date;
            dateTextBox.ValueChanged += DateTextBox_ValueChanged;
            dateTextBox.DropDown += DateTextBox_DropDown;
            //  ---                             ---


            //  ---     Stylist UI             ---
            Label stylistNameLabel;
            UIController.Label_ComboBox_Stylists(out stylistNameLabel, out stylistComboBox, 2);
            stylistComboBox.SelectedIndexChanged += StylistComboBox_SelectedIndexChanged;
            //  ---                             ---


            //  ---     Service UI             ---
            Label serviceNamesLabel;
            CheckedListBox servicesCheckedListBox;
            UIController.Label_CheckedListBox_Services(out serviceNamesLabel, out servicesCheckedListBox, 3);
            //  ---                             ---


            //  ---     Control Buttons         ---
            Button submitButton = new Button();
            Button cancelButton = new Button();
            UIController.ControlButtons(out submitButton, out cancelButton, 6);
            //  ---                             ---
            #endregion



            appointmentForm = new Form();
            appointmentForm.Text = "Create New Appointment";
            appointmentForm.ClientSize = new Size(350, 300);
            appointmentForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            appointmentForm.StartPosition = FormStartPosition.CenterScreen;
            appointmentForm.MinimizeBox = false;
            appointmentForm.MaximizeBox = false;
            appointmentForm.Controls.AddRange(new Control[] { customerNameLabel, customerNameTextBox, dateLabel, dateTextBox, stylistNameLabel, stylistComboBox, serviceNamesLabel, servicesCheckedListBox, submitButton, cancelButton });
            appointmentForm.AcceptButton = submitButton;
            appointmentForm.CancelButton = cancelButton;

            DialogResult dialogResult = appointmentForm.ShowDialog();
            int customerExists = DBManager.Functions.GetIDByName(DBManager.UserType.CUSTOMER, DBManager.Table.CUSTOMER, customerNameTextBox.Text);
            if (dialogResult == DialogResult.OK)
            {
                if (servicesCheckedListBox.CheckedItems.Count == 0)
                    MessageBox.Show("No Items Selected");
                else
                {
                    if (customerExists == 0)
                        MessageBox.Show(string.Format("Customer " + customerNameTextBox.Text + " does not exist."));
                    else
                    {
                        string serviceIDs = "";
                        var checkedItems = servicesCheckedListBox.CheckedItems;

                        for (int i = 0; i < checkedItems.Count; i++)
                        {
                            int id = DBManager.Functions.GetIDByName(DBManager.UserType.SERVICE, DBManager.Table.SERVICE, checkedItems[i].ToString());

                            if (i == checkedItems.Count - 1)
                                serviceIDs += id;
                            else
                                serviceIDs += (id + ", ");
                        }

                        InsertAppointment(DBManager.Functions.GetIDByName(DBManager.UserType.CUSTOMER, DBManager.Table.CUSTOMER, customerNameTextBox.Text), DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, stylistComboBox.Text), FormController.storeID, serviceIDs, DateTime.Parse(dateTextBox.Text));
                    }
                }
            }
            appointmentForm = null;
            return dialogResult;
        }

        private static void DateTextBox_DropDown(object? sender, EventArgs e)
        {
            dateTextBox.Text = DateTime.Parse(dateTextBox.Text).ToString("yyyy-MM-dd");
        }

        public static void DateTextBox_ValueChanged(object? sender, EventArgs e)
        {
            stylistComboBox.Items.Clear();
            stylistComboBox.Items.AddRange(DBManager.Functions.GetNamesWorking(DateTime.Parse(dateTextBox.Text).ToString("yyyy-MM-dd")));
            
            if (! stylistComboBox.Items.Contains(stylistComboBox.Text))
                stylistComboBox.Text = ""; 
        }

        public static void StylistComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (appointmentForm == null) return;
            appointmentForm.Enabled = false;
            TimePickerForm();
        }
        
        public static void TimePickerForm()
        {
            availableTimes = new Form();
            availableTimes.Text = "Stylist Availability";
            availableTimes.Visible = true;
            availableTimes.ClientSize = new Size(410, 300);
            availableTimes.StartPosition = FormStartPosition.CenterScreen;
            availableTimes.FormBorderStyle = FormBorderStyle.FixedDialog;
            availableTimes.MinimizeBox = false;
            availableTimes.MaximizeBox = false;
            availableTimes.FormClosed += AvailableTimes_FormClosed;

            Button timeButton;
            //timeButton.Click += TimeButton_Click;
            List<Button> buttons = new List<Button>();

            int[] times;

            times = GetTimeArray(DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, stylistComboBox.Text), dateTextBox.Value.ToString("yyyy-MM-dd"));

            int topControl = 10;
            int leftControl = 10;

            Panel panel = new Panel();
            panel.AutoScroll = true;
            panel.Size = new Size(410, 300);

            availableTimes.Controls.Add(panel);

            int i = 1;
            foreach(int time in times)
            {
                timeButton = new Button();
                timeButton.Text = time.ToString();
                timeButton.Size = new Size(75, 50);
                timeButton.Top = topControl;
                timeButton.Left = leftControl;
                timeButton.Click += TimeButton_Click;
                timeButton.Name = time.ToString();
                timeButton.Enabled = TimeButtonDisable(stylistComboBox.Text, dateTextBox.Value.ToString(), timeButton.Name.ToString());
                panel.Controls.Add(timeButton);

                leftControl += 75;

                if(i % 5 == 0)
                {
                    topControl += 55;
                    leftControl = 10;
                }

                i++;
            }
        }

        public static bool TimeButtonDisable(string empID, string date, string time)
        {
            date = DateTime.Parse(date).ToString("yyyy-MM-dd");

            if (time.Length == 3)
                time = time.Insert(0, "0");
            if (time.Length == 1)
                time += "000";
            if (time.Length == 2)
                time = "00" + time;
            
            time = time.Insert(2, ":");

            string dateTime = DateTime.Parse(date + " " + time).ToString("yyyy-MM-dd h:mm tt");

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "SELECT * FROM APPOINTMENT WHERE EmployeeID = @empID AND Date = @date";

                conn.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.Add("empID", SqlDbType.Int).Value = DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, empID);
                        command.Parameters.Add("@date", SqlDbType.NVarChar).Value = dateTime;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return true;
                }

                conn.Close();
            }

            return true;
        }

        public static int[] GetTimeArray(int id, string date)
        {
            List<int> times = new List<int>();
            string[] dbTimes = new string[3];

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "SELECT * FROM SCHEDULE WHERE EmployeeID = @empID AND Date = @date";
                string query1 = "SELECT * FROM SCHEDULE_EXCEPTION WHERE EmployeeID = @empID AND Date = @date";

                conn.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.Add("@empID", SqlDbType.NVarChar).Value = id;
                        command.Parameters.Add("@date", SqlDbType.NVarChar).Value = DateTime.Parse(date).ToString("dddd");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                                dbTimes = reader["Times"].ToString().Trim().Split(',');
                        }
                    }
                }
                catch
                {
                    dbTimes[0] = "0";
                    dbTimes[1] = "0";
                    dbTimes[2] = "0";
                }

                try
                {
                    using (SqlCommand command = new SqlCommand(query1, conn))
                    {
                        command.Parameters.Add("@empID", SqlDbType.Int).Value = id;
                        command.Parameters.Add("@date", SqlDbType.NVarChar).Value = date;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                                dbTimes = reader["Times"].ToString().Trim().Split(',');
                        }
                    }
                }
                catch{}

                conn.Close();
            }

            for (int i = int.Parse(dbTimes[0]); i <= int.Parse(dbTimes[2]);)
            {
                if (i == int.Parse(dbTimes[1]))
                {
                    i += 30;
                    if (i % 100 == 60)
                    {
                        i += 40;
                    }
                }
                    

                times.Add(i);

                // Add 30 minutes
                i += 30;

                // If 60 minutes have been reached, increment the hour
                if (i % 100 == 60)
                {
                    i += 40;
                }
            }

            return times.ToArray();
        }

        private static void TimeButton_Click(object? sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            string buttonName = clickedButton.Name;

            if(buttonName.Length == 3)
                buttonName = buttonName.Insert(0, "0");

            buttonName = buttonName.Insert(2, ":");

            string dateTime = dateTextBox.Value.ToString("yyyy-MM-dd") + " " + buttonName;

            dateTextBox.Value = DateTime.Parse(dateTime);
            
            availableTimes.Close();
        }

        private static void AvailableTimes_FormClosed(object? sender, FormClosedEventArgs e)
        {
            appointmentForm.Enabled = true;
        }

        
        /// <summary>
        /// Generates a random ID for the appointment.
        /// Connects to the SQL Database and generates an Insert command.
        /// ??? Throws an exception if input values are not clean. --- Needs more testing: 3/12/23
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        public static int InsertAppointment(int customerID, int employeeID, int storeID, string serviceIDs, DateTime date)
        {
            //  --- Generate AdminID ---        Needs to be reworked: Issue -> May not return a unique number => Maybe use GUID?
            Random rand = new Random();
            int appointmentID = rand.Next();

            //TESTING SQL INJECTION PROTECTION
            //string name = "Generic); Delete From dbo.admin Where \"Name\" = 'Generic';GO "; //SQL INJECTION

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "INSERT INTO Appointment (AppointmentID, CustomerID, EmployeeID, StoreID, ServiceIDs, Date, Pending)"
                            + " VALUES (@AppointmentID, @CustomerID, @EmployeeID, @StoreID, @ServiceIDs, @Date, @Pending)";
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@AppointmentID", appointmentID);
                        command.Parameters.AddWithValue("@CustomerID", customerID);
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);
                        command.Parameters.AddWithValue("@StoreID", storeID);
                        command.Parameters.AddWithValue("@ServiceIDs", serviceIDs);
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@Pending", 1);
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException sqlException)
                    {
                        MessageBox.Show("\t\t\t------ERROR!------ \n\t" +
                                        "An SQL Exception has occured!\n\tPlease contact your system" +
                                        " administrator for assistance." +
                                        "\n\t\t\t------ERROR!------\n\n\t" +
                                        "-------------------------------------------------------------------------\n\n" +
                                        sqlException.ToString());
                    }
                }
            }
            return appointmentID;
        }

        #endregion
    }
}
