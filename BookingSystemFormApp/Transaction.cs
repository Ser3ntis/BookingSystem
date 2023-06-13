using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemFormApp
{
    internal class ServiceItems
    {
        private static void GetServiceInfo(out Object[] serviceIDs, out Object[] serviceNames)
        {
            int count = 0;
            List<(Object, Object)> list = new List<(Object, Object)>();
            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "SELECT ServiceID, Name FROM Service";
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            (Object ServiceID, Object Name) objTuple = (reader["serviceid"], reader["name"]);
                            list.Add(objTuple);
                            count++;
                        }
                    }
                }
            }
            List<Object> iDs = new List<object>();
            List<Object> names = new List<object>();
            for (int i = 0; i < count; i++)
            {
                iDs.Add(list[i].Item1);
                names.Add(list[i].Item2);
            }
            serviceIDs = iDs.ToArray();
            serviceNames = names.ToArray();
        }

        public static string GetNameByID(int id)
        {
            List<Object> list = new List<Object>();
            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "SELECT Name FROM Service WHERE ServiceID="+id;
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["name"]);
                        }
                    }
                }
            }

            try
            {
                return list[0].ToString();
            }
            catch (Exception ex)
            {
                return ("ID "+id+" Error.");
            }
        }

        public static Object[] GetAllServiceIDs()
        {
            Object[] serviceIDs;
            ServiceItems.GetServiceInfo(out serviceIDs, out _);
            return serviceIDs;
        }
        public static Object[] GetAllServiceNames()
        {
            Object[] serviceNames;
            ServiceItems.GetServiceInfo(out _, out serviceNames);
            return serviceNames;
        }
        public static int GetServiceIDByIndex(int index)
        {
            Object[] serviceIDs;
            ServiceItems.GetServiceInfo(out serviceIDs, out _);
            return (int)serviceIDs[index];
        }
        public static int GetServiceNameByIndex(int index)
        {
            Object[] serviceNames;
            ServiceItems.GetServiceInfo(out _, out serviceNames);
            return (int)serviceNames[index];
        }
    }

    internal class Transaction
    {
        public static DialogResult TransactionViewForm()
        {
            string[] columnNames = { "TransactionID", "EmployeeID", "CustomerID", "ServiceID", "StoreID", "Date" };
            DataGridView dgv = new DataGridView();
            DBManager.SetupDGV(dgv, "Transaction View Form");

            DBManager.Initialize_Select_DGV(dgv, columnNames, DBManager.Functions.QueryBuilder(columnNames), DBManager.Table.TRANSACTION);

            Form viewForm = new Form();
            DBManager.CreateViewForm(dgv, viewForm, "View Transactions");

            DialogResult dialogResult = viewForm.ShowDialog();
            return DialogResult.OK;
        }

        public static DialogResult TransactionForm()
        {
            //Position Variables
            int xPos = 50;
            int yPos_Label = 15;
            int yPos_Text = 30;
            int offset = 40;

            

            //  ---     Control Buttons     ---
            Button submitButton = new Button();
            Button cancelButton = new Button();
            submitButton.Text = "Submit";
            cancelButton.Text = "Cancel";
            submitButton.DialogResult = DialogResult.OK;
            cancelButton.DialogResult = DialogResult.Cancel;
            submitButton.SetBounds(xPos, 180, 80, 30);
            cancelButton.SetBounds(xPos + 100, 180, 80, 30);
            //  ---                         ---

            Form transactionForm = new Form();
            transactionForm.Text = "Create New Appointment";
            transactionForm.ClientSize = new Size(350, 225);
            transactionForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            transactionForm.StartPosition = FormStartPosition.CenterScreen;
            transactionForm.MinimizeBox = false;
            transactionForm.MaximizeBox = false;
            transactionForm.Controls.AddRange(new Control[] {  submitButton, cancelButton });
            transactionForm.AcceptButton = submitButton;
            transactionForm.CancelButton = cancelButton;

            DialogResult dialogResult = transactionForm.ShowDialog();

            //if(dialogResult == DialogResult.OK)
            //    InsertTransaction();
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
        private static void InsertTransaction(int employeeID, int customerID, int serviceID)
        {
            //  --- Generate AdminID ---        Needs to be reworked: Issue -> May not return a unique number => Maybe use GUID?
            Random rand = new Random();
            int transactionID = rand.Next();

            //TESTING SQL INJECTION PROTECTION
            //string name = "Generic); Delete From dbo.admin Where \"Name\" = 'Generic';GO "; //SQL INJECTION

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "INSERT INTO Transactions (TransactionID, EmployeeID, CustomerID, ServiceID, StoreID, Date)"
                            + " VALUES (@TransactionID, @EmployeeID, @CustomerID, @ServiceID, @StoreID, @Date)";
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@TransactionID", transactionID);
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);
                        command.Parameters.AddWithValue("@CustomerID", customerID);
                        command.Parameters.AddWithValue("@ServiceID", serviceID);
                        command.Parameters.AddWithValue("@StoreID", FormController.storeID);
                        command.Parameters.AddWithValue("@Date", DateTime.Today);
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
    
    }
}
