using Microsoft.VisualBasic.ApplicationServices;
using System.Configuration;
using System.Data.SqlClient;
using static BookingSystemFormApp.DBManager;
using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Data;

namespace BookingSystemFormApp
{
    public class DBManager
    {
        private static DBManager instance;
        private static string connectionString = ConfigurationManager.AppSettings.Get("connection_string");
        public string ConnectionString { get => connectionString; }
        public static DBManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBManager();
                }
                return instance;
            }
        }

        private DBManager()
        {
        }

        public enum UserType
        {
            CUSTOMER,
            EMPLOYEE,
            ADMIN,  
            SUPERUSER,
            SERVICE
        }

        public enum Table
        {
            ADMIN, 
            APPOINTMENT, 
            CUSTOMER, 
            EMPLOYEE, 
            SERVICE, 
            STORE, 
            TRANSACTION
        }

        public struct InnerJoinStatement
        {
            private string query;
            private DBManager.Table firstTable;
            private DBManager.Table? secondTable;
            private DBManager.Table? thirdTable;
            private string onCondition1;
            private string? onCondition2;
            private string whereCondition;
            public InnerJoinStatement(string query, DBManager.Table firstTable, DBManager.Table? secondTable, DBManager.Table? thirdTable, string onCondition1, string? onCondition2, string whereCondition)
            {
                this.query = query;
                this.firstTable = firstTable;
                this.secondTable = secondTable;
                this.thirdTable = thirdTable;
                this.onCondition1 = onCondition1;
                this.onCondition2 = onCondition2;
                this.whereCondition = whereCondition;
            }

            public string Query
            {
                get => query;
            }
            public Table FirstTable
            {
                get => firstTable;
            }
            public Table? SecondTable
            {
                get => secondTable;
            }
            public Table? ThirdTable
            {
                get => thirdTable;
            }
            public string OnCondition1
            {
                get => onCondition1;
            }
            public string? OnCondition2
            {
                get => onCondition2;
            }
            public string WhereCondition
            {
                get => whereCondition;
            }
        }

        public struct Functions
        {
            /// <summary>
            /// Select - From - Inner Join - Where
            /// Accepts a InnerJoinStatement struct object which has all of the elements needed for a simple inner join.
            /// 
            /// Returns a 2D List of the elements returned from the sql statement.
            /// * [rows][columns]
            /// </summary>
            /// <param name="innerJoin"></param>
            /// <returns></returns>
            public static List<List<Object>> InnerJoin(InnerJoinStatement innerJoin)
            {
                string[] array = innerJoin.Query.Split(',');
                
                string query = " SELECT " + innerJoin.Query +
                               " FROM " + innerJoin.FirstTable +
                               " INNER JOIN " + innerJoin.SecondTable + " ON " + innerJoin.OnCondition1 +
                               " WHERE " + innerJoin.WhereCondition + ";";

                List<List<Object>> allInfo = new List<List<Object>>();

                ReadSQLData(array, query, allInfo);
                return allInfo;
            }
            
            /// <summary>
            /// Select - From - Inner Join - Where
            /// Accepts a InnerJoinStatement struct object which has all of the elements needed for a double inner join.
            /// 
            /// Returns a 2D List of the elements returned from the sql statement.
            /// * [rows][columns]
            /// </summary>
            /// <param name="innerJoin"></param>
            /// <returns></returns>
            public static List<List<Object>> DoubleInnerJoin(InnerJoinStatement innerJoin)
            {
                string[] array = innerJoin.Query.Split(',');

                string query = " SELECT " + innerJoin.Query +
                               " FROM " + innerJoin.FirstTable +
                               " INNER JOIN " + innerJoin.SecondTable + " ON " + innerJoin.OnCondition1 +
                               " INNER JOIN " + innerJoin.ThirdTable + " ON " + innerJoin.OnCondition2 + 
                               " WHERE " + innerJoin.WhereCondition + ";";

                List<List<Object>> allInfo = new List<List<Object>>();

                ReadSQLData(array, query, allInfo);
                return allInfo;
            }
            
            
            public static List<List<Object>> Select(string columns, Table table)
            {
                string[] array = columns.Split(',');
                string query =  " SELECT " + columns + 
                                " FROM " + table.ToString();

                List<List<Object>> allInfo = new List<List<Object>>();
                ReadSQLData(array, query, allInfo);
                return allInfo;
            }
            
            
            public static List<List<Object>> SelectWhere(string columns, Table table, string whereCondition)
            {
                string[] array = columns.Split(',');
                string query =  " SELECT " + columns + 
                                " FROM " + table.ToString() + 
                                " WHERE " + whereCondition;

                List<List<Object>> allInfo = new List<List<Object>>();
                ReadSQLData(array, query, allInfo);
                return allInfo;
            }

            public static List<List<Object>> getAppointments(DateTime date, string look)
            {
                DateTime nDate = new DateTime();

                if (look == "Day")
                {
                    TimeSpan ts = new TimeSpan(1, 0, 0, 0);
                    nDate = date + ts;
                }
                if (look == "Week")
                {
                    TimeSpan ts = new TimeSpan(7, 0, 0, 0);
                    nDate = date + ts;
                }
                if (look == "Month")
                {
                    TimeSpan ts = new TimeSpan(30, 0, 0, 0);
                    nDate = date + ts;
                }

                string[] array = { "Customer.Name", "Admin.Name", "Appointments.ServiceIDs", "Appointment.Date", "Appointment.Pending" };

                string query = "SELECT c.Name, ad.Name, app.ServiceIDs, app.Date, app.Pending " +
                               "FROM appointment as app, admin as ad, customer as c " +
                               "WHERE app.CustomerID = c.CustomerID " +
                               "AND app.EmployeeID = ad.AdminID " +
                               "AND app.Date BETWEEN \'" + date + "\' AND \'" + nDate + "\' ;" ;

                List<List<Object>> allInfo = new List<List<Object>>();

                ReadSQLData(array, query, allInfo);

                return allInfo;

            }

            public static void ReadSQLData(string[] array, string query, List<List<object>> allInfo)
            {
                using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                allInfo.Add(new List<Object>());
                                for (int i = 0; i < array.Length; i++)
                                {
                                    allInfo[count].Add(reader[i]);
                                }
                                count++;
                            }
                        }
                    }
                }
            }
            
            public static string GetNameByID(UserType user, Table table, int id)
            {

                List<List<Object>> columns = new List<List<Object>>();
                columns = Functions.Select( user.ToString() + "ID, Name", table);
                IDictionary<int, string> Info = new Dictionary<int, string>();
                for (int i = 0; i < columns[0].Count; i++)
                {
                    if (columns[i][1] == null || columns[i][0] == null) continue;
                    Info.Add(int.Parse(columns[i][0].ToString()), (string)columns[i][1]);
                }
                return Info[id];
            }
            
            
            public static int GetIDByName(UserType user, Table table, string name)
            {
                List<List<Object>> columns = new List<List<Object>>();
                columns = Functions.Select(user.ToString() + "ID, Name", table);
                IDictionary<string, int> Info = new Dictionary<string, int>();
                for (int i = 0; i < columns.Count; i++)
                {
                    if (columns[i][1] == null || columns[i][0] == null) continue;
                    Info.TryAdd((string)columns[i][1], int.Parse(columns[i][0].ToString()));
                }
                try
                {
                    return Info[name];
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
            
            
            public static Object[] GetAllIDs(UserType user, Table table)
            {
                List<List<Object>> columns;
                columns = Functions.Select(user.ToString() + "ID", table);
                List<Object> ids = new List<object>();

                foreach (List<Object> row in columns)
                {
                    ids.Add(row[0]);
                }
                return ids.ToArray();
            }
            
            
            public static Object[] GetAllNames(UserType user, Table table)
            {
                List<List<Object>> columns;
                columns = Functions.Select("Name", table);
                List<Object> names = new List<object>();

                foreach(List<Object> row in columns)
                {
                    names.Add(row[0]);
                }
                return names.ToArray();
            }

            public static Object[] GetNamesWorking(string date)
            {
                string[] empty = new string[0];
                try
                {
                    List<string> names = new List<string>();
                    List<string> ids = new List<string>();

                    using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
                    {
                        conn.Open();
                        string query = "SELECT * FROM SCHEDULE WHERE Date = @date";
                        string query1 = "SELECT * FROM SCHEDULE_EXCEPTION WHERE Date = @date";
                        string query2 = "SELECT * FROM ADMIN WHERE AdminID = @empID";

                        try
                        {
                            using (SqlCommand command = new SqlCommand(query, conn))
                            {
                                string day = DateTime.Parse(date).ToString("dddd");
                                command.Parameters.Add("@date", SqlDbType.NVarChar).Value = day;

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    int i = 0;
                                    while (reader.Read())
                                    {
                                        ids.Add(reader["EmployeeID"].ToString());
                                        i++;
                                    }
                                }
                            }
                        }
                        catch { }

                        using (SqlCommand command = new SqlCommand(query1, conn))
                        {                                
                            command.Parameters.Add("@date", SqlDbType.NVarChar).Value = date;

                            try
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        if (reader["Working"].ToString().Equals("True") && !ids.Contains(reader["EmployeeID"].ToString()))
                                            ids.Add(reader["EmployeeID"].ToString());
                                        else if (reader["Working"].ToString().Equals("False") && ids.Contains(reader["EmployeeID"].ToString()))
                                            ids.Remove(reader["EmployeeID"].ToString());
                                    }
                                }
                            }
                            catch {}
                        }
                        
                        int exitVar = 0;
                        int j = 0;

                        while (exitVar == 0)
                        {
                            try
                            {
                                using (SqlCommand command = new SqlCommand(query2, conn))
                                {
                                    command.Parameters.Add("@empID", SqlDbType.NVarChar).Value = ids[j];

                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        if (reader.Read())
                                            names.Add(reader["Name"].ToString());
                                        else
                                            exitVar = 1;
                                        j++;
                                    }
                                }
                            }
                            catch
                            {
                                exitVar = 1;
                            }
                        }

                        conn.Close();
                    }

                    return names.ToArray();
                }
                catch { }

                return empty;
            }


            public static int GetIDByIndex(UserType user, Table table, int index)
            {
                List<List<Object>> columns = new List<List<Object>>();
                columns = Functions.Select(user.ToString() + "ID", table);
                return (int)columns[0][index];
            }
            
            
            public static int GetNameByIndex(UserType user, Table table, int index)
            {
                List<List<Object>> columns = new List<List<Object>>();
                columns = Functions.Select("Name", table);
                return (int)columns[0][index];
            }

            /// <summary>
            /// Returns a comma-separated string built from an array of strings
            /// </summary>
            /// <param name="columnNames"></param>
            /// <returns></returns>
            public static string QueryBuilder(string[] columnNames)
            {
                string query = "";
                foreach (string columnName in columnNames)
                {
                    query += columnName + ", ";
                }
                query = query.Remove(query.LastIndexOf(','), 2);
                return query;
            }
        }

        
        public static void CreateViewForm(DataGridView dgv, Form viewForm, string formName)
        {
            dgv.Parent = viewForm;
            viewForm.Text = formName;
            viewForm.Name = formName;
            viewForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            viewForm.StartPosition = FormStartPosition.CenterScreen;
            viewForm.MinimizeBox = true;
            viewForm.MaximizeBox = true;
            viewForm.AutoSize = true;
            viewForm.FormBorderStyle = FormBorderStyle.Sizable;
            viewForm.Controls.Add(dgv);

            viewForm.ClientSize = new Size(700, 450);
        }

        
        public static void SetupDGV(DataGridView dgv, string name)
        {
            dgv.Name = name;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.ColumnHeadersVisible = true;
            dgv.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewCellStyle columnHeaderStyle =
                new DataGridViewCellStyle();

            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 10,
                FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle =
                columnHeaderStyle; 
            
            dgv.Dock = DockStyle.Fill;
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        
        public static void Initialize_Select_DGV(DataGridView dgv, string[] columnNames, string query, Table table)
        {
            dgv.Columns.Clear();
            dgv.ColumnCount = columnNames.Length;
            for (int i = 0; i < columnNames.Length; i++)
            {
                dgv.Columns[i].Name = columnNames[i];
            }
            List<List<Object>> columns = DBManager.Functions.Select(query, table);
            for (int i = 0; i < columns.Count; i++)
            {
                dgv.Rows.Add(Array.ConvertAll(columns[i].ToArray(), x => x.ToString()));
            }
        }
        
        
        public static void Initialize_SelectWhere_DGV(DataGridView dgv, string[] columnNames, string query, Table table, string whereCondition)
        {
            dgv.Columns.Clear();
            dgv.ColumnCount = columnNames.Length;
            for (int i = 0; i < columnNames.Length; i++)
            {
                dgv.Columns[i].Name = columnNames[i];
            }
            List<List<Object>> columns = DBManager.Functions.SelectWhere(query, table, whereCondition);
            for (int i = 0; i < columns.Count; i++)
            {
                dgv.Rows.Add(Array.ConvertAll(columns[i].ToArray(), x => x.ToString()));
            }
        }

        
        public static void Initialize_InnerJoin_DGV(DataGridView dgv, string[] columnNames, DBManager.InnerJoinStatement query)
        {
            dgv.Columns.Clear();
            dgv.ColumnCount = columnNames.Length;
            for (int i = 0; i < columnNames.Length; i++)
            {
                dgv.Columns[i].Name = columnNames[i];
            }
            List<List<Object>> columns;
            if (query.ThirdTable == null)   //IF only a single join - Third table variable should be null
            {
                columns = DBManager.Functions.InnerJoin(query);
            }
            else                            //ELSE IF Third table is not null the inner join must be a double join
            {
                columns = DBManager.Functions.DoubleInnerJoin(query);
            }

            for (int i = 0; i < columns.Count; i++)
            {
                dgv.Rows.Add(Array.ConvertAll(columns[i].ToArray(), x => x.ToString()));
            }
        }

        public static void UpcomingAppointmentViewDGV(DataGridView dgv, string[] columnNames, List<List<Object>> columns)
        {
            dgv.Columns.Clear();
            dgv.ColumnCount = columnNames.Length;
            for (int i = 0; i < columnNames.Length; i++)
            {
                dgv.Columns[i].Name = columnNames[i];
            }

            for (int i = 0; i < columns.Count; i++)
            {
                string[] row = Array.ConvertAll(columns[i].ToArray(), x => x.ToString());

                string services = "";
                string[] items = row[2].ToString().Split(", ");

                for (int j=0; j<items.Length; j++)
                {
                    services += ServiceItems.GetNameByID(Int32.Parse(items[j]));

                    if (j!=items.Length-1)
                        services +=", ";
                }


                row[2] = services;

                //else
                dgv.Rows.Add(row);

            }
        }

        public static DialogResult Lookup_DisplayNameForm(string formName, out string name)
        {
            name = "";
            #region UI Controls
            //  ---     Name UI             ---
            Label nameLabel;
            TextBox nameTextBox;
            UIController.Label_TextBox(out nameLabel, out nameTextBox, "Name:", 0);
            //  ---                         ---

            //  ---     Control Buttons     ---
            Button submitButton, cancelButton;
            UIController.ControlButtons(out submitButton, out cancelButton, 1);
            //  ---                         ---
            #endregion

            Form customerForm = new Form();
            customerForm.Text = formName;
            customerForm.ClientSize = new Size(350, 125);
            customerForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            customerForm.StartPosition = FormStartPosition.CenterScreen;
            customerForm.MinimizeBox = false;
            customerForm.MaximizeBox = false;
            customerForm.Controls.AddRange(new Control[] { nameLabel, nameTextBox, submitButton, cancelButton });
            customerForm.AcceptButton = submitButton;
            customerForm.CancelButton = cancelButton;

            DialogResult customerDialogResult = customerForm.ShowDialog();
            name = nameTextBox.Text;
            
            return customerDialogResult;
        }



        

    }
}
