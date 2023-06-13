using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemFormApp
{
    internal class Service
    {
        #region Delete Service
        /// <summary>
        /// Deletes selected entry on DGV PERMANENTLY - Confirms decision twice.
        /// </summary>
        public static void CheckDelete()
        {
            //  !!!     This method will permanently delete an entry from the database.     !!!

            DeleteForm deleteForm = (DeleteForm)Application.OpenForms["Delete Service"];
            if (DeleteForm.cellEvent == null)
            {
                MessageBox.Show("No event selected.");
            }
            else
            {
                DataGridViewRow row = deleteForm.DGV.Rows[DeleteForm.cellEvent.RowIndex];
                string serviceName = (string)row.Cells["Name"].Value;
                if (MessageBox.Show(string.Format("Do you want to delete customer \"{0}\" (ServiceID: {1})", row.Cells["Name"].Value, row.Cells["ServiceID"].Value), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (MessageBox.Show(string.Format("Are you sure you want to permanently delete this entry from the database?"), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(DBManager.Instance.ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM Service WHERE ServiceID = @ServiceID", con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@ServiceID", row.Cells["ServiceID"].Value);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                //Load data and update view on form
                deleteForm.LoadDGV(new string[] { "ServiceID", "Name", "Description", "Price" }, "ServiceID, Name, Description, Price", DBManager.Table.SERVICE, ("Name LIKE \'" + serviceName + "%\'"));
            }
        }
        public static DialogResult DeleteService()
        {
            string[] columnNames = { "ServiceID", "Name", "Description", "Price" };
            string serviceName = "";
            DialogResult customerDialogResult = DBManager.Lookup_DisplayNameForm("Lookup Customer", out serviceName);
            string whereCondition = "Name LIKE \'" + serviceName + "%\'";

            if (customerDialogResult == DialogResult.OK)
            {
                DeleteForm deleteForm = DeleteForm.Form;
                DBManager.SetupDGV(deleteForm.DGV, "Service");
                deleteForm.LoadDGV(columnNames, "ServiceID, Name, Description, Price", DBManager.Table.SERVICE, whereCondition);

                DBManager.CreateViewForm(deleteForm.DGV, deleteForm, "Delete Service");
                deleteForm.ClientSize = new Size(900, 600);
                DialogResult dialogResult = deleteForm.ShowDialog();
                return dialogResult;
            }
            return DialogResult.Abort;
        }
        #endregion
        #region View Service
        public static DialogResult ServiceViewForm()
        {
            string[] columnNames = { "ServiceID", "Name", "Description", "Price", "Established" };
            DataGridView dgv = new DataGridView();
            DBManager.SetupDGV(dgv, "Service View Form");

            DBManager.Initialize_Select_DGV(dgv, columnNames, DBManager.Functions.QueryBuilder(columnNames), DBManager.Table.SERVICE);

            Form viewForm = new Form();
            DBManager.CreateViewForm(dgv, viewForm, "View Services");

            DialogResult dialogResult = viewForm.ShowDialog();
            return DialogResult.OK;
        }
        #endregion
        #region Edit Service
        /// <summary>
        /// <br>Asks user to enter a service name. IF no name is entered all entries are shown.</br>
        /// <br>Creates and Initalizes a DataGridView to display database entries.</br>
        /// <br>Creates Form named Edit Service and adds the DGV as a control.</br>
        /// <br>Author: Clay Brown | 4/10/2023</br>
        /// 
        /// </summary>
        public static void EditService()
        {
            string serviceName = "";
            DialogResult dialogResult = DBManager.Lookup_DisplayNameForm("Lookup Service", out serviceName);
            string whereCondition = "Name LIKE \'" + serviceName + "%\'";
            if (dialogResult == DialogResult.OK)
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
                editButton.Text = "Edit Selected";
                */

                //Set up the DataGridView to display Database Entries
                DataGridView dgv = new DataGridView();
                DBManager.SetupDGV(dgv, "Edit Customers");
                DBManager.Initialize_SelectWhere_DGV(dgv, new string[] { "ServiceID", "Name", "Description", "Price", "Established" }, "ServiceID, Name, Description, Price, Established", DBManager.Table.SERVICE, whereCondition);
                dgv.Dock = DockStyle.Fill;
                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.Columns[3].FillWeight = 150;
                dgv.CellDoubleClick += new DataGridViewCellEventHandler(EditButton_Click);

                //Create the Edit Customer form to hold the DataGridView
                Form form = new Form();
                form.Name = "Edit Service";
                form.Text = "Edit Service";
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
                Form editForm = Application.OpenForms["Edit Service"];

                //Get the data grid view from the control array of the form and find the row at which the event occured
                DataGridView dgv = (DataGridView)editForm.Controls[0];
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                //Get the name of the customer on that row
                string customerName = (string)row.Cells["Name"].Value;

                //Ask user to confirm the selection
                if (MessageBox.Show(string.Format("Do you want to edit service \"{0}\" (ServiceID: {1})", row.Cells["Name"].Value, row.Cells["ServiceID"].Value), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Form form;
                    TextBox nameTextBox, descriptionTextBox, priceTextBox, establishedTextBox;
                    CreateForm(out form, out nameTextBox, out descriptionTextBox, out priceTextBox, out establishedTextBox);

                    //Auto-Fill text boxes with the existing data
                    nameTextBox.Text = (string)row.Cells["Name"].Value;
                    descriptionTextBox.Text = (string)row.Cells["Description"].Value;
                    priceTextBox.Text = (string)row.Cells["Price"].Value;
                    establishedTextBox.Text = (string)row.Cells["Established"].Value;


                    DialogResult dialogResult = form.ShowDialog();

                    if (dialogResult == DialogResult.OK)
                    {
                        AlterService((string)row.Cells["ServiceID"].Value, nameTextBox.Text, descriptionTextBox.Text, priceTextBox.Text, establishedTextBox.Text);
                    }
                }
            }
        }


        /// <summary>
        /// <br>Executes a SQL Alter command to change the database values. </br>
        /// 
        /// <br></br>
        /// Author: Clay Brown | 4/10/2023
        /// </summary>
        /// <param name="serviceID"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <param name="established"></param>
        private static void AlterService(string serviceID, string name, string description, string price, string established)
        {
            string query;
            query = " UPDATE Service" +
                        " SET Name = @Name, Description = @Description, Price = @Price, Established = @Established" +
                        " WHERE ServiceID = @ServiceID";

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@ServiceID", serviceID);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Established", established);
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
            Application.OpenForms["Edit Service"].Close();

            //Confirm updated Values
            MessageBox.Show($"Service Updated!\nID: {serviceID}\nName: {name}\nDescription: {description}\nPrice: {price}\nEstablished: {established}");

        }

        #endregion
        #region Add Serivce
        public static DialogResult ServiceForm()
        {
            //  ---     Name UI             ---
            Label serviceNameLabel;
            TextBox serviceNameTextBox;
            UIController.Label_TextBox(out serviceNameLabel, out serviceNameTextBox, "Name:", 0);
            //  ---                         ---

            //  ---     Description         ---
            Label descriptionLabel;
            TextBox descriptionTextBox;
            UIController.Label_TextBox(out descriptionLabel, out descriptionTextBox, "Description:", 1);
            //  ---                         ---

            //  ---     Price UI            ---
            Label priceLabel;
            TextBox priceTextBox;
            UIController.Label_TextBox(out priceLabel, out priceTextBox, "Price:", 2);
            //  ---                         ---

            //  ---     Control Buttons     ---
            Button submitButton;
            Button cancelButton;
            UIController.ControlButtons(out submitButton, out cancelButton, 4);
            //  ---                         ---

            Form adminForm = new Form();
            adminForm.Text = "Create New Service";
            adminForm.ClientSize = new Size(350, 225);
            adminForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            adminForm.StartPosition = FormStartPosition.CenterScreen;
            adminForm.MinimizeBox = false;
            adminForm.MaximizeBox = false;
            adminForm.Controls.AddRange(new Control[] { serviceNameLabel, serviceNameTextBox, descriptionLabel, descriptionTextBox, priceLabel, priceTextBox, submitButton, cancelButton });
            adminForm.AcceptButton = submitButton;
            adminForm.CancelButton = cancelButton;

            DialogResult dialogResult = adminForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
                InsertService(serviceNameTextBox.Text, descriptionTextBox.Text, float.Parse(priceTextBox.Text));
            return dialogResult;
        }

        /// <summary>
        /// Accepts Service-related parameters.
        /// Generates a unique ID for the Service.
        /// Connects to the SQL Database and generates an Insert command.
        /// ??? Throws an exception if input values are not clean. --- Needs more testing: 3/12/23
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        public static int InsertService(string serviceName, string description, float price)
        {
            //  --- Generate ServiceID ---        Needs to be reworked: Issue -> May not return a unique number => Maybe use GUID?
            Random rand = new Random();
            int serviceID = rand.Next();

            //TESTING SQL INJECTION PROTECTION
            //string name = "Generic); Delete From dbo.admin Where \"Name\" = 'Generic';GO "; //SQL INJECTION

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "INSERT INTO Service (ServiceID, Name, Description, Price, Established)"
                            + " VALUES (@ServiceID, @Name, @Description, @Price, @Established)";
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@ServiceID", serviceID);
                        command.Parameters.AddWithValue("@Name", serviceName);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Established", DateTime.Today);
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
            return serviceID;
        }
        #endregion

        #region Utilities
        /// <summary>
        /// <br>Creates a Service form that is formated and initialized with controls.</br>
        /// <br>Author: Clay Brown | 4/10/2023</br>
        /// </summary>
        /// <param name="form"></param>
        /// <param name="nameTextBox"></param>
        /// <param name="descriptionTextBox"></param>
        /// <param name="priceTextBox"></param>
        /// <param name="establishedTextBox"></param>
        private static void CreateForm(out Form form, out TextBox nameTextBox, out TextBox descriptionTextBox, out TextBox priceTextBox, out TextBox establishedTextBox)
        {
            form = new Form();
            Label nameLabel, descriptionLabel, priceLabel, establishedLabel;
            Button submitButton, cancelButton;
            InitServiceUIControls(out nameLabel, out nameTextBox, out descriptionLabel, out descriptionTextBox, out priceLabel, out priceTextBox, out establishedLabel, out establishedTextBox, out submitButton, out cancelButton);
            InitCustomerForm(nameLabel, descriptionLabel, priceLabel, establishedLabel, nameTextBox, descriptionTextBox, priceTextBox, establishedTextBox, submitButton, cancelButton, form);
        }


        /// <summary>
        /// <br>Formats the Service Form.</br>
        /// <br>Adds all UI Controls to the form.</br>
        /// <br>Author: Clay Brown | 4/23/10</br>
        /// </summary>
        /// <param name="nameLabel"></param>
        /// <param name="descriptionLabel"></param>
        /// <param name="priceLabel"></param>
        /// <param name="establishedLabel"></param>
        /// <param name="nameTextBox"></param>
        /// <param name="descriptionTextBox"></param>
        /// <param name="priceTextBox"></param>
        /// <param name="establishedTextBox"></param>
        /// <param name="submitButton"></param>
        /// <param name="cancelButton"></param>
        /// <param name="form"></param>
        private static void InitCustomerForm(Label nameLabel, Label descriptionLabel, Label priceLabel, Label establishedLabel, TextBox nameTextBox, TextBox descriptionTextBox, TextBox priceTextBox, TextBox establishedTextBox, Button submitButton, Button cancelButton, Form form)
        {
            form.Name = "Service Form";
            form.Text = "Edit Service";
            form.ClientSize = new Size(350, 225);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.Controls.AddRange(new Control[] { nameLabel, nameTextBox, descriptionLabel, descriptionTextBox, priceLabel, priceTextBox, establishedLabel, establishedTextBox, submitButton, cancelButton });
            form.AcceptButton = submitButton;
            form.CancelButton = cancelButton;
        }


        /// <summary>
        /// Creates Service UI Controls <br></br>
        /// Use as follows: 
        /// <example>
        /// <code>
        ///     Label nameLabel, descriptionLabel, priceLabel, establishedLabel;
        ///     TextBox nameTextBox, descriptionTextBox, priceTextBox, establishedTextBox;
        ///     Button submitButton, cancelButton;
        ///     InitServiceUIControls(out nameLabel, out nameTextBox, out descriptionLabel, out descriptionTextBox, 
        ///         out priceLabel, out priceTextBox, out establishedLabel, out establishedTextBox, out submitButton, out cancelButton);
        /// </code>
        /// </example>
        /// <br></br>
        /// Author: Clay Brown 4/10/2023
        /// </summary>
        /// <param name="nameLabel"></param>
        /// <param name="nameTextBox"></param>
        /// <param name="descriptionLabel"></param>
        /// <param name="descriptionTextBox"></param>
        /// <param name="priceLabel"></param>
        /// <param name="priceTextBox"></param>
        /// <param name="establishedLabel"></param>
        /// <param name="establishedTextBox"></param>
        /// <param name="submitButton"></param>
        /// <param name="cancelButton"></param>
        private static void InitServiceUIControls(out Label nameLabel, out TextBox nameTextBox, out Label descriptionLabel, out TextBox descriptionTextBox, out Label priceLabel, out TextBox priceTextBox, out Label establishedLabel, out TextBox establishedTextBox, out Button submitButton, out Button cancelButton)
        {
            //  ---     Name UI             ---
            UIController.Label_TextBox(out nameLabel, out nameTextBox, "Name:", 0);
            UIController.Label_TextBox(out descriptionLabel, out descriptionTextBox, "Description:", 1);
            UIController.Label_TextBox(out priceLabel, out priceTextBox, "Price $:", 2);
            UIController.Label_TextBox(out establishedLabel, out establishedTextBox, "Date Established:", 3);
            UIController.ControlButtons(out submitButton, out cancelButton, 4);
            //  ---                         ---
        }
        #endregion
    }
}
