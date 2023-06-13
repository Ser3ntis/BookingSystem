using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BookingSystemFormApp
{
    internal class Admin
    {
        #region Edit Stylist

        public static void EditAdmin()
        {
            string employeeName = "";
            DialogResult employeeDialogResult = DBManager.Lookup_DisplayNameForm("Lookup Employee", out employeeName);
            string whereCondition = "Name LIKE \'" + employeeName + "%\'";
            if (employeeDialogResult == DialogResult.OK)
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
                DBManager.SetupDGV(dgv, "Edit Employee");
                DBManager.Initialize_SelectWhere_DGV(dgv, new string[] { "AdminID", "Name", "Phone", "Email" }, "AdminID, Name, Phone, Email", DBManager.Table.ADMIN, whereCondition);
                dgv.Dock = DockStyle.Fill;
                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.Columns[3].FillWeight = 150;
                dgv.CellDoubleClick += new DataGridViewCellEventHandler(EditButton_Click);

                //Create the Edit Customer form to hold the DataGridView
                Form form = new Form();
                form.Name = "Edit Admin";
                form.Text = "Edit Admin";
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
                Form editForm = Application.OpenForms["Edit Admin"];

                //Get the data grid view from the control array of the form and find the row at which the event occured
                DataGridView dgv = (DataGridView)editForm.Controls[0];
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                //Get the name of the customer on that row
                string customerName = (string)row.Cells["Name"].Value;

                //Ask user to confirm the selection
                if (MessageBox.Show(string.Format("Do you want to edit admin \"{0}\" (AdminID: {1})", row.Cells["Name"].Value, row.Cells["AdminID"].Value), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Form form;
                    TextBox nameTextBox, passwordTextBox, phoneTextBox, emailTextBox;
                    CreateAdminForm(out form, out nameTextBox, out passwordTextBox, out phoneTextBox, out emailTextBox);

                    //Auto-Fill text boxes with the existing data
                    nameTextBox.Text = (string)row.Cells["Name"].Value;
                    passwordTextBox.Text = "Leave Blank or Enter new";
                    phoneTextBox.Text = (string)row.Cells["Phone"].Value;
                    emailTextBox.Text = (string)row.Cells["Email"].Value;


                    DialogResult dialogResult = form.ShowDialog();

                    CheckAttributeLengths(nameTextBox.Text, passwordTextBox.Text, phoneTextBox.Text, emailTextBox.Text);
                    if (dialogResult == DialogResult.OK)
                    {
                        bool notTest = true;
                        AlterAdmin((string)row.Cells["AdminID"].Value, nameTextBox.Text, passwordTextBox.Text, phoneTextBox.Text, emailTextBox.Text, notTest);
                    }
                }
            }
        }


        /// <summary>
        /// Executes a SQL Alter command to change the database values. <br></br>
        /// 
        /// <br></br>
        /// Author: Clay Brown | 4/10/2023
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        public static void AlterAdmin(string adminID, string name, string password, string phone, string email, bool notTest)
        {
            string query;
            bool changePass = false;
            query = CheckIfUpdatePassword(password, ref changePass);

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@AdminID", adminID);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Phone", phone.PadRight(10).Substring(0, 10));
                        if (changePass)
                            command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Email", email);
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
            if (notTest)
            {
                //Close the Edit form --> So I don't have to refresh it :P
                Application.OpenForms["Edit Admin"].Close();

                //Confirm updated Values
                if (changePass)
                    MessageBox.Show($"Admin Updated!\nID: {adminID}\nName: {name}\nPhone: {phone}\nPassword: {"Password Updated!"}\nEmail: {email}");
                else
                    MessageBox.Show($"Admin Updated!\nID: {adminID}\nName: {name}\nPhone: {phone}\nPassword: {"Password was not updated."}\nEmail: {email}");
            }
        }

        private static void CreateAdminForm(out Form form, out TextBox nameTextBox, out TextBox passwordTextBox, out TextBox phoneTextBox, out TextBox emailTextBox)
        {
            form = new Form();
            Label nameLabel, passwordLabel, phoneLabel, emailLabel;
            Button submitButton, cancelButton;
            InitAdminUIControls(out nameLabel, out nameTextBox, out passwordLabel, out passwordTextBox, out phoneLabel, out phoneTextBox, out emailLabel, out emailTextBox, out submitButton, out cancelButton);
            InitAdminForm(nameLabel, passwordLabel, phoneLabel, emailLabel, nameTextBox, passwordTextBox, phoneTextBox, emailTextBox, submitButton, cancelButton, form);
        }
        private static string CheckIfUpdatePassword(string password, ref bool changePass)
        {
            string query;
            //Not a great password verification system, needs to be revisited 4/10/23
            if (password.Equals("Leave Blank or Enter new") || password.Equals(""))
            {
                query = " UPDATE Admin" +
                        " SET Name = @Name, Phone = @Phone,Email = @Email" +
                        " WHERE AdminID = @AdminID";
            }
            else
            {
                changePass = true;
                query = " UPDATE Admin" +
                        " SET Name = @Name, Phone = @Phone,Password = @Password,Email = @Email" +
                        " WHERE AdminID = @AdminID";
            }

            return query;
        }

        private static void CheckAttributeLengths(string name, string password, string phone, string email)
        {
            if (name.Length > 30)
                MessageBox.Show($"Warning!\n {name} contains {name.Length} characters and will be truncated to 30 characters.");
            if (password.Length > 255)
                MessageBox.Show($"Warning!\n Password contains {password.Length} characters and will be truncated to 255 characters.");
            if (phone.Length > 10)
                MessageBox.Show($"Warning!\n {phone} contains {phone.Length} characters and will be truncated to 10 characters.");
            if (password.Length > 60)
                MessageBox.Show($"Warning!\n {email} contains {email.Length} characters and will be truncated to 60 characters.");
        }

        private static void InitAdminForm(Label nameLabel, Label passwordLabel, Label phoneLabel, Label emailLabel, TextBox nameTextBox, TextBox passwordTextBox, TextBox phoneTextBox, TextBox emailTextBox, Button submitButton, Button cancelButton, Form customerForm)
        {
            customerForm.Name = "Customer Form";
            customerForm.Text = "Create New Customer";
            customerForm.ClientSize = new Size(350, 225);
            customerForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            customerForm.StartPosition = FormStartPosition.CenterScreen;
            customerForm.MinimizeBox = false;
            customerForm.MaximizeBox = false;
            customerForm.Controls.AddRange(new Control[] { nameLabel, nameTextBox, passwordLabel, passwordTextBox, phoneLabel, phoneTextBox, emailLabel, emailTextBox, submitButton, cancelButton });
            customerForm.AcceptButton = submitButton;
            customerForm.CancelButton = cancelButton;
        }

        private static void InitAdminUIControls(out Label nameLabel, out TextBox nameTextBox, out Label passwordLabel, out TextBox passwordTextBox, out Label phoneLabel, out TextBox phoneTextBox, out Label emailLabel, out TextBox emailTextBox, out Button submitButton, out Button cancelButton)
        {
            //  ---     Name UI             ---
            UIController.Label_TextBox(out nameLabel, out nameTextBox, "Name:", 0);
            UIController.Label_TextBox(out passwordLabel, out passwordTextBox, "Password:", 1);
            UIController.Label_TextBox(out phoneLabel, out phoneTextBox, "Phone #:", 2);
            UIController.Label_TextBox(out emailLabel, out emailTextBox, "Email:", 3);
            UIController.ControlButtons(out submitButton, out cancelButton, 4);
            //  ---                         ---
        }

        #endregion
        #region Delete Stylist
        /// <summary>
        /// Deletes selected entry on DGV PERMANENTLY - Confirms decision twice.
        /// </summary>
        public static void CheckDelete()
        {
            //  !!!     This method will permanently delete an entry from the database.     !!!

            DeleteForm deleteForm = (DeleteForm)Application.OpenForms["Delete Stylist"];
            if (DeleteForm.cellEvent == null)
            {
                MessageBox.Show("No event selected.");
            }
            else
            {
                DataGridViewRow row = deleteForm.DGV.Rows[DeleteForm.cellEvent.RowIndex];
                string adminName = (string)row.Cells["Name"].Value;
                if (MessageBox.Show(string.Format("Do you want to delete stylist \"{0}\" (AdminID: {1})", row.Cells["Name"].Value, row.Cells["AdminID"].Value), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (MessageBox.Show(string.Format("Are you sure you want to permanently delete this entry from the database?"), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(DBManager.Instance.ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM Admin WHERE AdminID = @AdminID", con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@AdminID", row.Cells["AdminID"].Value);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                //Load data and update view on form
                deleteForm.LoadDGV(new string[] { "AdminID", "Name", "Phone", "Email" }, "AdminID, Name, Phone, Email", DBManager.Table.ADMIN, ("Name LIKE \'" + adminName + "%\'"));
            }
        }
        public static DialogResult DeleteAdmin()
        {
            string[] columnNames = { "AdminID", "Name", "Phone", "Email" };
            string adminName = "";
            DialogResult adminDialogResult = DBManager.Lookup_DisplayNameForm("Lookup stylist", out adminName);
            string whereCondition = "Name LIKE \'" + adminName + "%\'";

            if (adminDialogResult == DialogResult.OK)
            {
                DeleteForm deleteForm = DeleteForm.Form;
                DBManager.SetupDGV(deleteForm.DGV, "Admin");
                deleteForm.LoadDGV(columnNames, "AdminID, Name, Phone, Email", DBManager.Table.ADMIN, whereCondition);

                DBManager.CreateViewForm(deleteForm.DGV, deleteForm, "Delete Stylist");
                deleteForm.ClientSize = new Size(900, 600);
                DialogResult dialogResult = deleteForm.ShowDialog();
                return dialogResult;
            }
            return DialogResult.Abort;
        }
        #endregion
        #region View Stylist
        public static DialogResult AdminViewForm()
        {
            string[] columnNames = { "AdminID", "Name", "Phone", "Email" };
            DataGridView dgv = new DataGridView();
            DBManager.SetupDGV(dgv, "Admin View Form");

            DBManager.Initialize_Select_DGV(dgv, columnNames, DBManager.Functions.QueryBuilder(columnNames), DBManager.Table.ADMIN);

            Form viewForm = new Form();
            DBManager.CreateViewForm(dgv, viewForm, "View Admins");

            DialogResult dialogResult = viewForm.ShowDialog();
            return DialogResult.OK;
        }
        #endregion

        #region Add Stylist
        public static DialogResult AdminForm()
        {
            Form adminForm;
            TextBox nameTextBox, passwordTextBox, phoneTextBox, emailTextBox;
            CreateAdminForm(out adminForm, out nameTextBox, out passwordTextBox, out phoneTextBox, out emailTextBox);

            //  ---     Control Buttons     ---
            Button submitButton, cancelButton;
            UIController.ControlButtons(out submitButton, out cancelButton, 4);
            //  ---                         ---

            adminForm.Text = "Create New Admin";
            adminForm.ClientSize = new Size(350, 225);
            adminForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            adminForm.StartPosition = FormStartPosition.CenterScreen;
            adminForm.MinimizeBox = false;
            adminForm.MaximizeBox = false;
            adminForm.AcceptButton = submitButton;
            adminForm.CancelButton = cancelButton;

            DialogResult dialogResult = adminForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
                InsertAdmin(nameTextBox.Text, passwordTextBox.Text, phoneTextBox.Text, emailTextBox.Text);
            return dialogResult;
        }

        /// <summary>
        /// Accepts Admin-related parameters.
        /// Generates a unique ID for the Admin.
        /// Connects to the SQL Database and generates an Insert command.
        /// ??? Throws an exception if input values are not clean. --- Needs more testing: 3/12/23
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        public static int InsertAdmin(string name, string password, string phone, string email)
        {
            //  --- Generate AdminID ---        Needs to be reworked: Issue -> May not return a unique number => Maybe use GUID?
            Random rand = new Random();
            int adminID = rand.Next();

            //TESTING SQL INJECTION PROTECTION
            //string name = "Generic); Delete From dbo.admin Where \"Name\" = 'Generic';GO "; //SQL INJECTION

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "INSERT INTO Admin (AdminID, Name, Phone, Password, Email)"
                            + " VALUES (@AdminID, @Name, @Phone, @Password, @Email)";
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@AdminID", adminID);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Phone", phone.PadRight(10).Substring(0, 10));
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Email", email);
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
            return adminID;
        }
        #endregion
    }
}
