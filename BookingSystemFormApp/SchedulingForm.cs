using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextBox = System.Windows.Forms.TextBox;
using ComboBox = System.Windows.Forms.ComboBox;
using Button = System.Windows.Forms.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;
using System.Collections;
using System.Reflection.PortableExecutable;
using System.Security.Permissions;
using System.Reflection;

//Date: 4/16/23
//Author: Sam Reissing
//Create the Schedule Edit Form

namespace BookingSystemFormApp
{
    public partial class SchedulingForm : Form
    {
        ScheduleFormMaker scheduleForm = new ScheduleFormMaker();

        public SchedulingForm()
        {
            InitializeComponent();
            scheduleForm.Location = new Point(0, 0);
            scheduleForm.Size = new Size(600, 400);
            this.Controls.Add(scheduleForm);
        }

        public partial class ScheduleFormMaker : UserControl
        {
            private Panel textBoxPanel = new Panel();
            public TextBox[] tBoxArr = new TextBox[7];
            public ComboBox[] cBoxArr = new ComboBox[21];
            public Label[] labelArr = new Label[7];
            private string[] textArr = new string[3];
            private Button addWeekButton = new Button();
            private Button nextWeekButton = new Button();
            private Button previousWeekButton = new Button();

            //day controler. Starts at yesterday to avoid extra code
            public DateTime day = DateTime.Today;

            //month/year cloning variable
            public int cloneCount = 0;

            //Location control variables
            public int topControl = 2;
            public int leftControl = 0;

            //makes sure the program is always refering to the correct text box
            public int arrControl = 0;

            public int testID = 0;

            //is set to whatever ID is put into the Employee ID Text Box
            string boxID = "";

            public ScheduleFormMaker()
            {
                // Create "Add Week" button
                addWeekButton.Text = "Add Week";
                addWeekButton.Location = new Point(50, 50);
                addWeekButton.Click += addWeekButton_Click;
                this.Controls.Add(addWeekButton);

                // Create panel to hold textboxes
                this.Size = new Size(800, 600);
                textBoxPanel.Visible = true;
                textBoxPanel.Location = new Point(0, 50);
                textBoxPanel.Size = new Size(600, 450);
                this.Controls.Add(textBoxPanel);
            }

            //creates textboxes and auto-fills them if data already exists in database
            public void addWeekButton_Click(object sender, EventArgs e)
            {
                SchedulingForm.textBox1.Text = DBManager.Functions.GetIDByName(DBManager.UserType.ADMIN, DBManager.Table.ADMIN, SchedulingForm.textBox1.Text).ToString();

                boxID = getBoxID(testID);

                if (boxID == "")
                {
                    MessageBox.Show("Please enter an employee ID");
                }
                else if (employeeExists(int.Parse(boxID)) == false)
                {
                    MessageBox.Show("Employee with ID " + boxID + " does not exist");
                }
                else
                {
                    makeReadOnly(testID);

                    for (int j = 0; j < tBoxArr.Length; j++)
                    {
                        TextBox box = new TextBox();
                        box.Top = topControl * 25;
                        box.Left = leftControl + 75;
                        textBoxPanel.Controls.Add(box);
                        if (j == 0)
                            box.Text = getSunday();
                        else
                        {
                            day = day.AddDays(1);
                            box.Text = day.ToString("dddd");
                        }
                        box.ReadOnly = true;
                        tBoxArr[j] = box;

                        // Creates labels left of the text boxes that shows the date
                        Label label = new Label();
                        label.Top = topControl * 25 + 5;
                        label.Left = leftControl + 5;
                        label.Width = 70;
                        label.Text = day.ToString("yyyy-MM-dd");
                        textBoxPanel.Controls.Add(label);
                        labelArr[j] = label;

                        leftControl += 125;
                        arrControl++;
                        databaseTimes();

                        for (int i = 0; i < (cBoxArr.Length / tBoxArr.Length); i++)
                        {
                            // Create Text Boxes for user input
                            ComboBox cbox = new ComboBox();
                            string[] installs = new string[] {"",
                                                              "0000", "0030", "0100", "0130",
                                                              "0200", "0230", "0300", "0330",
                                                              "0400", "0430", "0500", "0530",
                                                              "0600", "0630", "0700", "0730",
                                                              "0800", "0830", "0900", "0930",
                                                              "1000", "1030", "1100", "1130",
                                                              "1200", "1230", "1300", "1330",
                                                              "1400", "1430", "1500", "1530",
                                                              "1600", "1630", "1700", "1730",
                                                              "1800", "1830", "1900", "1930",
                                                              "2000", "2030", "2100", "2130",
                                                              "2200", "2230", "2300", "2330"};
                            cbox.Items.AddRange(installs);

                            //sets TextBox locations (variable, changes with each run of the loop function)
                            cbox.Top = topControl * 25;
                            cbox.Left = leftControl + 75;
                            leftControl += 125;

                            cbox.Text = textArr[i];
                            textBoxPanel.Controls.Add(cbox);
                            cbox.DropDown += new System.EventHandler(comboBox_DropDown);
                            cbox.DropDownStyle = ComboBoxStyle.DropDownList;
                            cBoxArr[arrControl - j - 1] = cbox;

                            arrControl++;
                        }

                        //itterates topControl so new textbox object are made underneath others and resets the leftControl variable
                        leftControl = 0;
                        topControl++;
                    }

                    addWeekButton.Visible = false;

                    //creates "Previous Week" button when "Add Week" is pressed
                    previousWeekButton.Text = "Previous Week";
                    previousWeekButton.Location = new Point(50, 0);
                    previousWeekButton.Size = new Size(100, 23);
                    previousWeekButton.Click += previousWeekButton_Click;
                    textBoxPanel.Controls.Add(previousWeekButton);


                    //creates "Next Week" Button when "Add Week" is pressed
                    nextWeekButton.Text = "Next Week";
                    nextWeekButton.Location = new Point(200, 0);
                    nextWeekButton.Click += nextWeekButton_Click;
                    textBoxPanel.Controls.Add(nextWeekButton);
                }
            }

            public string getBoxID(int a)
            {
                if (a == 0)
                    return ((SchedulingForm)this.ParentForm).getTextBox1Text();
                else
                    return "482040087";
            }

            //returns true if employee exists, and false otherwise
            public Boolean employeeExists(int id)
            {
                using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
                {
                    string query = "SELECT * FROM ADMIN WHERE AdminID = @empID";
                    conn.Open();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.Add("@empID", SqlDbType.Int).Value = id;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                conn.Close();
                                return true;
                            }
                            else
                            {
                                conn.Close();
                                return false;
                            }
                        }
                    }
                }
            }

            private void comboBox_DropDown(object sender, EventArgs e)
            {

            }

            //Sets Employee ID Text Box to read only so user's cant input another employee and enables the submit button
            private void makeReadOnly(int a)
            {
                if (a == 0)
                {
                    ((SchedulingForm)this.ParentForm).setTextBox1ReadOnly();
                    ((SchedulingForm)this.ParentForm).submitButtonEnable();
                }
            }

            //gets text in textboxes and comboboxes and puts it in the database
            public void saveButton_Click(object sender, EventArgs e)
            {
                string[] BoxText = new string[arrControl];

                int c = 0;
                for (int i = 0; i < arrControl; i++)
                {
                    if (i % 4 == 0)
                        BoxText[i] = tBoxArr[i / 4].Text;
                    else
                        BoxText[i] = cBoxArr[c++].Text;
                }

                using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
                {
                    string query = "INSERT INTO SCHEDULE (EmployeeID, Date, Times)" + " VALUES (@EmployeeID, @Date, @Times)";
                    conn.Open();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.Add("@EmployeeID", SqlDbType.NVarChar);
                        command.Parameters.Add("@Date", SqlDbType.NVarChar);
                        command.Parameters.Add("@Times", SqlDbType.NVarChar);

                        for (int i = 0; i < BoxText.Length; i += 4)
                        {
                            deleteExisting(tBoxArr[i / 4].Text);

                            command.Parameters["@EmployeeID"].Value = boxID;
                            command.Parameters["@Date"].Value = BoxText[i];
                            command.Parameters["@Times"].Value = BoxText[i + 1] + "," + BoxText[i + 2] + "," + BoxText[i + 3];

                            if (!command.Parameters["@Times"].Value.Equals(",,"))
                                command.ExecuteNonQuery();
                        }

                        conn.Close();

                        MessageBox.Show("The schedule was added to the table");
                    }
                }
            }

            //creates schedule exceptions in the database
            public void saveException(object sender, EventArgs e)
            {
                string[] BoxText = new string[arrControl];

                Boolean exists = false;

                int c = 0;
                for (int i = 0; i < arrControl; i++)
                {
                    if (i % 4 == 0)
                        BoxText[i] = tBoxArr[i / 4].Text;
                    else
                        BoxText[i] = cBoxArr[c++].Text;
                }

                using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
                {
                    string query = "SELECT * FROM SCHEDULE WHERE EmployeeID = @EmployeeID AND Date = @Date";
                    string query1 = "INSERT INTO SCHEDULE_EXCEPTION (EmployeeID, Date, Times, working) VALUES (@EmployeeID, @Date, @Times, @Working)";
                    conn.Open();

                    string[] dbText = new string[3];

                    for (int i = 0; i < BoxText.Length; i += 4)
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(query, conn))
                            {
                                command.Parameters.Add("@EmployeeID", SqlDbType.NVarChar);
                                command.Parameters.Add("@Date", SqlDbType.Char);


                                command.Parameters["@EmployeeID"].Value = boxID;
                                command.Parameters["@Date"].Value = BoxText[i];

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        exists = true;
                                        string times = reader["Times"].ToString();
                                        if (!string.IsNullOrEmpty(times))
                                            dbText = times.Trim().Split(',');
                                    }
                                    else
                                    {
                                        exists = false;
                                        dbText[0] = "";
                                        dbText[1] = "";
                                        dbText[2] = "";
                                    }
                                }
                            }
                        }
                        catch (Exception ex) { }

                        try
                        {
                            using (SqlCommand command = new SqlCommand(query1, conn))
                            {
                                command.Parameters.Add("@EmployeeID", SqlDbType.Int);
                                command.Parameters.Add("@Date", SqlDbType.Date);
                                command.Parameters.Add("@Times", SqlDbType.NVarChar);
                                command.Parameters.Add("@Working", SqlDbType.Bit);

                                command.Parameters["@EmployeeID"].Value = Int32.Parse(boxID);
                                command.Parameters["@Date"].Value = DateTime.Parse(labelArr[i / 4].Text);
                                command.Parameters["@Times"].Value = BoxText[i + 1] + "," + BoxText[i + 2] + "," + BoxText[i + 3];

                                if (BoxText[i + 1].Equals("") && BoxText[i + 2].Equals("") && BoxText[i + 3].Equals(""))
                                    command.Parameters["@Working"].Value = 0;
                                else
                                    command.Parameters["@Working"].Value = 1;

                                deleteExistingException(DateTime.Parse(labelArr[i / 4].Text));

                                string dbTextString = dbText[0] + "," + dbText[1] + "," + dbText[2];
                                if (exists && (!command.Parameters["@Times"].Value.Equals(dbTextString)))
                                    command.ExecuteNonQuery();
                                else if (!exists && !command.Parameters["@Times"].Value.Equals(",,"))
                                    command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex) { }
                    }

                    MessageBox.Show("Exceptions were saved to the database");
                    conn.Close();
                }
            }

            //deletes existing entries with the same employee id and date
            public void deleteExisting(string date)
            {
                using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
                {
                    string query = "DELETE FROM SCHEDULE WHERE EmployeeID = @empID AND Date = @date";
                    conn.Open();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.Add("@empID", SqlDbType.NVarChar).Value = boxID;
                        command.Parameters.Add("@date", SqlDbType.NVarChar).Value = date;

                        using (SqlDataReader reader = command.ExecuteReader()) { }
                    }

                    conn.Close();
                }
            }

            public void deleteExistingException(DateTime date)
            {
                using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
                {
                    string query = "DELETE FROM SCHEDULE_Exception WHERE EmployeeID = @empID AND Date = @date";
                    conn.Open();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.Add("@empID", SqlDbType.Int).Value = Int32.Parse(boxID);
                        command.Parameters.Add("@date", SqlDbType.Date).Value = date;

                        using (SqlDataReader reader = command.ExecuteReader()) { }
                    }

                    conn.Close();
                }
            }

            //changes date and data to that of 7 days prior to current date
            public void previousWeekButton_Click(object sender, EventArgs e)
            {
                day = day.AddDays(-14);

                int j = 0;
                arrControl = 0;

                for (int i = 0; i < (tBoxArr.Length + cBoxArr.Length); i++)
                {
                    if (i % 4 == 0)
                    {
                        day = day.AddDays(1);
                        tBoxArr[i / 4].Text = day.ToString("dddd");
                        labelArr[i / 4].Text = day.ToString("yyyy-MM-dd");
                        databaseTimes();
                        j++;
                    }
                    else
                        cBoxArr[i - j].Text = textArr[(i % 4) - 1];
                    arrControl++;
                }
            }

            //changes date and data to that of 7 days ahead of current date
            public void nextWeekButton_Click(object sender, EventArgs e)
            {
                int j = 0;
                arrControl = 0;

                for (int i = 0; i < (tBoxArr.Length + cBoxArr.Length); i++)
                {
                    if (i % 4 == 0)
                    {
                        day = day.AddDays(1);
                        tBoxArr[i / 4].Text = day.ToString("dddd");
                        labelArr[i / 4].Text = day.ToString("yyyy-MM-dd");
                        databaseTimes();
                        j++;
                    }
                    else
                        cBoxArr[i - j].Text = textArr[(i % 4) - 1];

                    arrControl++;
                }
            }

            //sets date for original textboxes
            public string getSunday()
            {
                for (int i = 0; i < 7; i++)
                {
                    if (day.DayOfWeek == DayOfWeek.Sunday)
                        break;
                    else
                        day = day.AddDays(-1);
                }

                return day.ToString("dddd");
            }

            //pulls times from database and fills textArr with times
            public void databaseTimes()
            {
                string times = "";

                using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
                {
                    string query = "SELECT * FROM SCHEDULE WHERE EmployeeID = @empID AND Date = @day";
                    string query1 = "SELECT * FROM SCHEDULE_EXCEPTION WHERE EmployeeID = @empID1 AND Date = @date";

                    conn.Open();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        try
                        {
                            command.Parameters.Add("@empID", SqlDbType.NVarChar).Value = boxID;
                            command.Parameters.Add("@day", SqlDbType.NVarChar).Value = tBoxArr[arrControl / 4].Text;

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    times = reader["Times"].ToString();
                                    if (!string.IsNullOrEmpty(times))
                                        textArr = times.Trim().Split(',');
                                }
                                else
                                {
                                    textArr[0] = "";
                                    textArr[1] = "";
                                    textArr[2] = "";
                                }
                            }
                        }
                        catch (Exception ex) { }
                    }

                    using (SqlCommand command = new SqlCommand(query1, conn))
                    {
                        try
                        {
                            command.Parameters.Add("@empID1", SqlDbType.Int).Value = Int32.Parse(boxID);
                            command.Parameters.Add("@date", SqlDbType.Date).Value = labelArr[arrControl / 4].Text;

                            using (SqlDataReader reader1 = command.ExecuteReader())
                            {
                                if (reader1.Read())
                                {
                                    string working = reader1["working"].ToString();
                                    if (working.Equals("True"))
                                        times = reader1["Times"].ToString();
                                    else
                                    {
                                        times = "";
                                        textArr[0] = "";
                                        textArr[1] = "";
                                        textArr[2] = "";
                                    }

                                    if (!string.IsNullOrEmpty(times))
                                        textArr = times.Trim().Split(',');
                                }
                            }
                        }
                        catch (Exception ex) { }
                    }

                    conn.Close();
                }
            }
        }

        //save week button
        private void submit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Save this Schedule?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                scheduleForm.saveButton_Click(sender, e);

                scheduleForm.arrControl = 0;
                scheduleForm.leftControl = 0;
                scheduleForm.topControl = 2;
                //this.Close();
            }
        }

        private void SaveException_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Save exceptions to this Schedule?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                scheduleForm.saveException(sender, e);

                scheduleForm.arrControl = 0;
                scheduleForm.leftControl = 0;
                scheduleForm.topControl = 2;
                this.Close();
            }
        }
    }
}
