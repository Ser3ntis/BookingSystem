/**************************************************************************************************
 * Name:  Noah Whitridge
 * Class: CSCI 4320 Software Engineering
 * Date:  04/12/23
 * 
 * The purpose of this class is to allow for the "Generate Transactions" button to function. This 
 * button is meant to A) allow the user to select an appointment from a list, B) list out the cost 
 * of each element of the appointment, C) sum the cost of each element, and D) display the total 
 * cost of the appointment.
 *************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookingSystemFormApp
{
	internal class GenerateTransaction
	{
		/******************************************************************************************
		 * Description:
		 * The purpose of this method is to handle when the "Generate Transaction View Form" is 
		 * clicked by the user. This method creates a new form to show the tallied up cost of the 
		 * appointment selected.
		 * 
		 * Parameters:
		 * object sender - The object that is calling this method.
		 * DataGridViewCellEventArgs e - The event that has occurred (double clicking on the form).
		 *****************************************************************************************/
		public static void GenerateTransButton_Click(object? sender, DataGridViewCellEventArgs e)
		{
			if (e == null)
			{
				MessageBox.Show("No appointment selected.");
			}
			else
			{
				// Reference the open generate transaction form
				Form genTransForm = Application.OpenForms["Generate Transaction View Form"];

				// Get the data grid view from the control array of the form and find the row at
				// which the event occurred.
				DataGridView dgv = (DataGridView)genTransForm.Controls[0];
				DataGridViewRow row = dgv.Rows[e.RowIndex];

                // Tally up the cost.
                StringBuilder receiptInformation = new StringBuilder();
                TextBox receiptInformationTextbox = new TextBox();
				receiptInformationTextbox.Multiline = true;
				receiptInformationTextbox.Font = new Font("Courier New", 11);

                string serviceCell = (string) row.Cells["ServiceIDs"].Value;
                string[] services = serviceCell.Split(", ");
                string[] select = new string[] { "Name", "Price" };
                decimal[] prices = new decimal[services.Length];
				decimal totalPrice = 0;

				receiptInformation.AppendLine(String.Format("{0,-25} | {1,-25}\n", "Item:", "Price:"));
                for (int i = 0; i < services.Length; i++)
                {
                    string query = " SELECT Name, Price" +
                                   " FROM Service" +
                                   " WHERE ServiceID =\'" + services[i] + "\'";

                    List<List<Object>> allinfo = new List<List<object>>();
                    DBManager.Functions.ReadSQLData(select, query, allinfo);
                    prices[i] = (decimal)allinfo[0][1];
					totalPrice += prices[i];
					receiptInformation.AppendLine(String.Format("{0,-25} | ${1,-25}\n", 
						                           services[i], prices[i]));
                }
				receiptInformation.AppendLine(String.Format("----------------------------------"));
				receiptInformation.AppendLine(String.Format("{0,-25} | ${1,-25}\n", 
					                          "Total price:", totalPrice));
				receiptInformationTextbox.AppendText(receiptInformation.ToString());
				receiptInformationTextbox.ReadOnly = true;

                // Create form and set up its parameters.
                Form viewSum = new Form();
                viewSum.Name = "View Transaction";
                viewSum.Text = "View Transaction";
                viewSum.ClientSize = new Size(700, 450);
                viewSum.FormBorderStyle = FormBorderStyle.Sizable;
                viewSum.StartPosition = FormStartPosition.CenterScreen;
                viewSum.MinimizeBox = false;
                viewSum.MaximizeBox = false;
				receiptInformationTextbox.Width = viewSum.Width;
				receiptInformationTextbox.Height = viewSum.Height;
				viewSum.Controls.Add(receiptInformationTextbox);
                viewSum.ShowDialog();
            }
		}

        /******************************************************************************************
		 * Description:
		 * The purpose of this function is to facilitate the functionality of the "Generate 
		 * Transactions" button. It works by having the user enter the name of the customer they 
		 * want to find, select the relevant appointment, and then generate the transaction 
		 * information.
		 * 
		 * Parameters:
		 * string[] columnNames - The columns from the database that will be shown to the user.
		 *****************************************************************************************/
        public static DialogResult GenerateTransactionForm(string[]? columnNames)
        {
			// The purpose of this if-statement is to cover the empty possibility.
			if (columnNames == null)
			{
				columnNames = new string[] { "AppointmentID", "Customer Name", "Employee Name", 
					                         "StoreIDs", "ServiceIDs", "Date", "Pending" };
			}

			// Ask user for the name of the customer they're searching for.
			string customerName = "";
			DialogResult customerDialogResult = DBManager.Lookup_DisplayNameForm("Lookup Customer",
				                                out customerName);

			// Show list of customer's appointments.
			if (customerDialogResult == DialogResult.OK)
			{
				DataGridView dgv = new DataGridView();
				dgv.AllowUserToAddRows = false;
				dgv.AllowUserToDeleteRows = false;
				dgv.AllowDrop = false;
				dgv.ReadOnly = true;
                
				dgv.CellDoubleClick += new DataGridViewCellEventHandler(GenerateTransButton_Click);
                
				
				
				DBManager.SetupDGV(dgv, "Generate Transaction View Form");
				DBManager.Initialize_InnerJoin_DGV(dgv, columnNames, 
					new DBManager.InnerJoinStatement(
					"Appointment.AppointmentID, Customer.Name, Admin.Name, Appointment.StoreID, " +
					"Appointment.ServiceIDs, Appointment.Date, Appointment.Pending",
					DBManager.Table.APPOINTMENT, DBManager.Table.CUSTOMER, DBManager.Table.ADMIN, 
					"Appointment.CustomerID=Customer.CustomerID", 
					"Appointment.EmployeeID=Admin.AdminID",
                    "Appointment.Pending = 1 AND Customer.Name LIKE \'" + customerName + "%\';"));

				Form viewForm = new Form();
				DBManager.CreateViewForm(dgv, viewForm, "Generate Transaction View Form");
				DialogResult dialogResult = viewForm.ShowDialog();
                return dialogResult;
			}

			return DialogResult.Abort;
		}

		/******************************************************************************************
		 * Description:
		 * GenerateTransaction class constructor.
		 * 
		 * NOTE: It's unclear why this method is necessary to allow for the functionality in 
		 * FormController.cs; however, it works, so don't touch it.
		 *****************************************************************************************/
		public GenerateTransaction()
		{
			GenerateTransaction.GenerateTransactionForm(null);
		}
    }
}