using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

//Date: 4/16/23
//Author: Sam Reissing
//Creates Schedule View

namespace BookingSystemFormApp
{
    public partial class ScheduleView : Form
    {
        private Label[] labelArr;
        private List<string> rowText = new List<string>();
        private List<Label> lines = new List<Label>();
        private Panel panel = new Panel();

        private int topControl = 100;
        private int leftControl = 25;
        public ScheduleView()
        {
            InitializeComponent();

            panel.Dock = DockStyle.Fill;
            panel.Top = topControl;
            panel.Left = leftControl;
            panel.AutoScroll = true;
            this.Controls.Add(panel);

            formMaker();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            formMaker();
        }

        private void formMaker()
        {
            if (labelArr != null)
            {
                foreach (Label label in labelArr)
                {
                    panel.Controls.Remove(label);
                }

                foreach (Label label in lines)
                {
                    panel.Controls.Remove(label);
                }

                rowText.Clear();
                lines.Clear();
            }

            int rows = getRows();
            labelArr = new Label[rows * 4];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Label label = new Label();
                    label.Top = topControl;
                    label.Left = leftControl;
                    label.Text = rowText[(i * 4) + j];
                    labelArr[(i * 4) + j] = label;
                    panel.Controls.Add(label);

                    leftControl += 150;
                }

                leftControl = 25;

                Label line = new Label();
                line.Top = topControl + 10;
                line.Left = 25;
                line.Width = 550;
                line.Height = 20;
                line.Text = "_______________________________________________________________________________________________________";
                panel.Controls.Add(line);
                lines.Add(line);

                topControl += 60;
            }

            leftControl = 25;
            topControl = 100;
        }

        private int getRows()
        {
            int rowCount = 0;

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "SELECT * FROM SCHEDULE WHERE Date = @date";
                conn.Open();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    string date = DateTime.Parse(getDate()).ToString("dddd");
                    command.Parameters.Add("@date", SqlDbType.NVarChar).Value = date;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Increment the row count for each row that contains the desired date
                            rowCount++;
                        }
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        string[] strings = new string[4];
                        int i = 0;
                        while (reader.Read())
                        {
                            string name = getName(reader["EmployeeID"].ToString());

                            string data = name + "," + reader["Times"].ToString();
                            strings = data.Trim().Split(',');

                            int j = 0;
                            foreach (string s in strings)
                            {
                                rowText.Add(s);
                                i++;
                                j++;
                            }
                        }
                    }
                }

                conn.Close();
            }
            return rowCount;
        }

        private string getName(string id)
        {
            string name = "";

            using (SqlConnection conn = new SqlConnection(DBManager.Instance.ConnectionString))
            {
                string query = "SELECT * FROM ADMIN WHERE AdminID = @id";
                conn.Open();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = int.Parse(id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            name = reader["Name"].ToString().Trim();
                    }
                }

                conn.Close();
            }

            return name;
        }
    }
}
