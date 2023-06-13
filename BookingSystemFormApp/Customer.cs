using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace BookingSystemFormApp
{   
    internal class Customer
    {
        #region Delete Customer
        /// <summary>
        /// Deletes selected entry on DGV PERMANENTLY - Confirms decision twice.
        /// </summary>
        public static void CheckDelete()
        {
            //  !!!     This method will permanently delete an entry from the database.     !!!

            DeleteForm deleteForm = (DeleteForm)Application.OpenForms["Delete Customer"];
            if (DeleteForm.cellEvent == null)
            {
                MessageBox.Show("No event selected.");
            }
            else
            {
                DataGridViewRow row = deleteForm.DGV.Rows[DeleteForm.cellEvent.RowIndex];
                string customerName = (string)row.Cells["Name"].Value;
                if (MessageBox.Show(string.Format("Do you want to delete customer \"{0}\" (CustomerID: {1})", row.Cells["Name"].Value, row.Cells["CustomerID"].Value), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (MessageBox.Show(string.Format("Are you sure you want to permanently delete this entry from the database?"), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DeleteCustFromDB(row.Cells["CustomerID"].Value);
                    }
                }
                //Load data and update view on form
                deleteForm.LoadDGV(new string[] { "CustomerID", "Name", "Phone", "Email" }, "CustomerID, Name, Phone, Email", DBManager.Table.CUSTOMER, ("Name LIKE \'" + customerName + "%\'"));
            }
        }

        public static void DeleteCustFromDB(object obj)
        {
            using (SqlConnection con = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Customer WHERE CustomerID = @CustomerID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CustomerID", obj);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DialogResult DeleteCustomer()
        {
            string[] columnNames = { "CustomerID", "Name", "Phone", "Email" };
            string customerName = "";
            DialogResult customerDialogResult = DBManager.Lookup_DisplayNameForm("Lookup Customer", out customerName);
            string whereCondition = "Name LIKE \'" + customerName + "%\'";

            if (customerDialogResult == DialogResult.OK)
            {
                DeleteForm deleteForm = DeleteForm.Form;
                DBManager.SetupDGV(deleteForm.DGV, "Customer");
                deleteForm.LoadDGV(columnNames, "CustomerID, Name, Phone, Email", DBManager.Table.CUSTOMER, whereCondition);

                DBManager.CreateViewForm(deleteForm.DGV, deleteForm, "Delete Customer");
                deleteForm.ClientSize = new Size(900, 600);
                DialogResult dialogResult = deleteForm.ShowDialog();
                return dialogResult;
            }
            return DialogResult.Abort;
        }

        #endregion



        #region View Customer
        public static DialogResult ViewCustomerForm(string name)
        {
            string[] columnNames = { "CustomerID", "Name", "Phone", "Email" };
            DataGridView dgv = new DataGridView();
            DBManager.SetupDGV(dgv, "Customer View Form");

            DBManager.Initialize_Select_DGV(dgv, columnNames, DBManager.Functions.QueryBuilder(columnNames), DBManager.Table.CUSTOMER);
            dgv.Columns[3].FillWeight = 150;

            Form viewForm = new Form();
            DBManager.CreateViewForm(dgv, viewForm, name);

            DialogResult dialogResult = viewForm.ShowDialog();
            return DialogResult.OK;
        }
        #endregion



        #region Edit Customer
        /// <summary>
        /// <br>Asks user to enter a customer name. IF no name is entered all entries are shown.</br>
        /// <br>Creates and Initalizes a DataGridView to display database entries.</br>
        /// <br>Creates Form named Edit Cutomer and adds the DGV as a control.</br>
        /// <br>Author: Clay Brown | 4/10/2023</br>
        /// 
        /// </summary>
        public static void EditCustomer()
        {
            string customerName = "";
            DialogResult customerDialogResult = DBManager.Lookup_DisplayNameForm("Lookup Customer", out customerName);
            string whereCondition = "Name LIKE \'" + customerName + "%\'";
            if (customerDialogResult == DialogResult.OK)
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
                DBManager.SetupDGV(dgv, "Edit Customers");
                DBManager.Initialize_SelectWhere_DGV(dgv, new string[] { "CustomerID", "Name", "Phone", "Email" }, "CustomerID, Name, Phone, Email", DBManager.Table.CUSTOMER, whereCondition);
                dgv.Dock = DockStyle.Fill;
                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.Columns[3].FillWeight = 150; 

                // FUNCTION THAT IS CALLED WHEN THEY CLICK ON CELLS
                dgv.CellDoubleClick += new DataGridViewCellEventHandler(EditButton_Click);

                //Create the Edit Customer form to hold the DataGridView
                Form form = new Form();
                form.Name = "Edit Customer";
                form.Text = "Edit Customer";
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
                Form editForm = Application.OpenForms["Edit Customer"];

                //Get the data grid view from the control array of the form and find the row at which the event occured
                DataGridView dgv = (DataGridView)editForm.Controls[0];
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                
                //Get the name of the customer on that row
                string customerName = (string)row.Cells["Name"].Value;

                //Ask user to confirm the selection
                if (MessageBox.Show(string.Format("Do you want to edit customer \"{0}\" (CustomerID: {1})", row.Cells["Name"].Value, row.Cells["CustomerID"].Value), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Form form;
                    TextBox nameTextBox, passwordTextBox, phoneTextBox, emailTextBox;
                    CreateCustomerForm(out form, out nameTextBox, out passwordTextBox, out phoneTextBox, out emailTextBox);

                    //Auto-Fill text boxes with the existing data
                    nameTextBox.Text = (string)row.Cells["Name"].Value;
                    passwordTextBox.Text = "Leave Blank or Enter new";
                    phoneTextBox.Text = (string)row.Cells["Phone"].Value;
                    emailTextBox.Text = (string)row.Cells["Email"].Value;


                    DialogResult dialogResult = form.ShowDialog();

                    CheckAttributeLengths(nameTextBox.Text, passwordTextBox.Text, phoneTextBox.Text, emailTextBox.Text);
                    if (dialogResult == DialogResult.OK)
                    {
                        AlterCustomer((string)row.Cells["CustomerID"].Value, nameTextBox.Text, passwordTextBox.Text, phoneTextBox.Text, emailTextBox.Text);
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
        private static void AlterCustomer(string customerID, string name, string password, string phone, string email)
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
                        command.Parameters.AddWithValue("@CustomerID", customerID);
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

            //Close the Edit form --> So I don't have to refresh it :P
            Application.OpenForms["Edit Customer"].Close();

            //Confirm updated Values
            if (changePass)
                MessageBox.Show($"Customer Updated!\nID: {customerID}\nName: {name}\nPhone: {phone}\nPassword: {"Password Updated!"}\nEmail: {email}");
            else
                MessageBox.Show($"Customer Updated!\nID: {customerID}\nName: {name}\nPhone: {phone}\nPassword: {"Password was not updated."}\nEmail: {email}");

        }

        #endregion


        #region Add Customer
        public static Form AddCustomerForm(bool showForm)
        {
            Form customerForm;
            TextBox nameTextBox, passwordTextBox, phoneTextBox, emailTextBox;
            CreateCustomerForm(out customerForm, out nameTextBox, out passwordTextBox, out phoneTextBox, out emailTextBox);

            // Do not show the form unless it is explicit
            if (showForm)
            {
                DialogResult dialogResult = customerForm.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    CheckAttributeLengths(nameTextBox.Text, passwordTextBox.Text, phoneTextBox.Text, emailTextBox.Text);
                    InsertCustomer(nameTextBox.Text, passwordTextBox.Text, phoneTextBox.Text, emailTextBox.Text);
                }
            }

            return customerForm;
        }

        /// <summary>
        /// Accepts Customer-related parameters.
        /// Generates a unique ID for the customer.
        /// Connects to the SQL Database and generates an Insert command.
        /// ??? Throws an exception if input values are not clean. --- Needs more testing: 3/12/23
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        public static int InsertCustomer(string name, string password, string phone, string email)
        {
            //  --- Generate AdminID ---        Needs to be reworked: Issue -> May not return a unique number => Maybe use GUID?
            Random rand = new Random();
            int customerID = rand.Next();

            //TESTING SQL INJECTION PROTECTION
            //string name = "Generic); Delete From dbo.admin Where \"Name\" = 'Generic';GO "; //SQL INJECTION

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "INSERT INTO customer (CustomerID, Name, Phone, Password, Email)"
                            + " VALUES (@CustomerID, @Name, @Phone, @Password, @Email)";
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@CustomerID", customerID);
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
            return customerID;
        }
        public static int InsertCustomer(string name, string password, string phone, string email, string salt)
        {
            //  --- Generate AdminID ---        Needs to be reworked: Issue -> May not return a unique number => Maybe use GUID?
            Random rand = new Random();
            int customerID = rand.Next();

            //TESTING SQL INJECTION PROTECTION
            //string name = "Generic); Delete From dbo.admin Where \"Name\" = 'Generic';GO "; //SQL INJECTION

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "INSERT INTO customer (CustomerID, Name, Phone, Password, Email, Salt)"
                            + " VALUES (@CustomerID, @Name, @Phone, @Password, @Email, @Salt)";
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@CustomerID", customerID);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Phone", phone.PadRight(10).Substring(0, 10));
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Salt", salt);
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
            return customerID;
        }
        #endregion


        #region Customer Utility Functions
        /// <summary>
        /// <br>Checks the length of params against the SQL lengths respectively.</br>
        /// <br>NOTE: This does not fix the issue, just lets you know a truncation exception may occur.</br>
        /// <br>Author: Clay Brown | 4/10/2023</br>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
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


        /// <summary>
        /// Checks if the user wants to update their password. <br></br>
        /// Returns a specific query based on whether or not the password is being reset.
        /// <br></br>
        /// Author: Clay Brown | 4/10/2023
        /// </summary>
        /// <param name="password"></param>
        /// <param name="changePass"></param>
        /// <returns></returns>
        private static string CheckIfUpdatePassword(string password, ref bool changePass)
        {
            string query;
            //Not a great password verification system, needs to be revisited 4/10/23
            if (password.Equals("Leave Blank or Enter new") || password.Equals(""))
            {
                query = " UPDATE Customer" +
                        " SET Name = @Name, Phone = @Phone,Email = @Email" +
                        " WHERE CustomerID = @CustomerID";
            }
            else
            {
                changePass = true;
                query = " UPDATE Customer" +
                        " SET Name = @Name, Phone = @Phone,Password = @Password,Email = @Email" +
                        " WHERE CustomerID = @CustomerID";
            }

            return query;
        }


        /// <summary>
        /// <br>Creates a Customer form that is formated and initialized with controls.</br>
        /// <br>Author: Clay Brown | 4/10/2023</br>
        /// </summary>
        /// <param name="form"></param>
        /// <param name="nameTextBox"></param>
        /// <param name="passwordTextBox"></param>
        /// <param name="phoneTextBox"></param>
        /// <param name="emailTextBox"></param>
        private static void CreateCustomerForm(out Form form, out TextBox nameTextBox, out TextBox passwordTextBox, out TextBox phoneTextBox, out TextBox emailTextBox)
        {
            form = new Form();
            Label nameLabel, passwordLabel, phoneLabel, emailLabel;
            Button submitButton, cancelButton;
            InitCustomerUIControls(out nameLabel, out nameTextBox, out passwordLabel, out passwordTextBox, out phoneLabel, out phoneTextBox, out emailLabel, out emailTextBox, out submitButton, out cancelButton);
            InitCustomerForm(nameLabel, passwordLabel, phoneLabel, emailLabel, nameTextBox, passwordTextBox, phoneTextBox, emailTextBox, submitButton, cancelButton, form);
        }


        /// <summary>
        /// <br>Formats the Customer Form.</br>
        /// <br>Adds all UI Controls to the form.</br>
        /// <br>Author: Clay Brown | 4/23/10</br>
        /// </summary>
        /// <param name="nameLabel"></param>
        /// <param name="passwordLabel"></param>
        /// <param name="phoneLabel"></param>
        /// <param name="emailLabel"></param>
        /// <param name="nameTextBox"></param>
        /// <param name="passwordTextBox"></param>
        /// <param name="phoneTextBox"></param>
        /// <param name="emailTextBox"></param>
        /// <param name="submitButton"></param>
        /// <param name="cancelButton"></param>
        /// <param name="customerForm"></param>
        private static void InitCustomerForm(Label nameLabel, Label passwordLabel, Label phoneLabel, Label emailLabel, TextBox nameTextBox, TextBox passwordTextBox, TextBox phoneTextBox, TextBox emailTextBox, Button submitButton, Button cancelButton, Form customerForm)
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


        /// <summary>
        /// Creates Customer UI Controls <br></br>
        /// Use as follows: 
        /// <example>
        /// <code>
        ///     Label nameLabel, passwordLabel, phoneLabel, emailLabel;
        ///     TextBox nameTextBox, passwordTextBox, phoneTextBox, emailTextBox;
        ///     Button submitButton, cancelButton;
        ///     InitCustomerUIControls(out nameLabel, out nameTextBox, out passwordLabel, out passwordTextBox, 
        ///         out phoneLabel, out phoneTextBox, out emailLabel, out emailTextBox, out submitButton, out cancelButton);
        /// </code>
        /// </example>
        /// <br></br>
        /// Author: Clay Brown 4/10/2023
        /// </summary>
        /// <param name="nameLabel"></param>
        /// <param name="nameTextBox"></param>
        /// <param name="passwordLabel"></param>
        /// <param name="passwordTextBox"></param>
        /// <param name="phoneLabel"></param>
        /// <param name="phoneTextBox"></param>
        /// <param name="emailLabel"></param>
        /// <param name="emailTextBox"></param>
        /// <param name="submitButton"></param>
        /// <param name="cancelButton"></param>
        private static void InitCustomerUIControls(out Label nameLabel, out TextBox nameTextBox, out Label passwordLabel, out TextBox passwordTextBox, out Label phoneLabel, out TextBox phoneTextBox, out Label emailLabel, out TextBox emailTextBox, out Button submitButton, out Button cancelButton)
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

    }
}
