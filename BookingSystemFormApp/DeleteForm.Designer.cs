using System.Data.SqlClient;
using System.Data;

namespace BookingSystemFormApp
{
    partial class DeleteForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Delete = new Button();
            Refresh = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1 });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(943, 595);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellContentClick;
            // 
            // Column1
            // 
            Column1.HeaderText = "Column1";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            // 
            // Delete
            // 
            Delete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Delete.Location = new Point(12, 533);
            Delete.Name = "Delete";
            Delete.Size = new Size(150, 50);
            Delete.TabIndex = 1;
            Delete.Text = "Delete Selected";
            Delete.UseVisualStyleBackColor = true;
            Delete.Click += Delete_Click;
            // 
            // Refresh
            // 
            Refresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Refresh.Location = new Point(168, 533);
            Refresh.Name = "Refresh";
            Refresh.Size = new Size(150, 50);
            Refresh.TabIndex = 2;
            Refresh.Text = "Refresh View";
            Refresh.UseVisualStyleBackColor = true;
            Refresh.Click += Refresh_Click;
            // 
            // DeleteForm
            // 
            ClientSize = new Size(943, 595);
            Controls.Add(Refresh);
            Controls.Add(Delete);
            Controls.Add(dataGridView1);
            Name = "DeleteForm";
            Text = "DeleteForm";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private static DeleteForm deleteForm = new DeleteForm();
        private DataGridView dataGridView1;
        private Button Delete;
        private DataGridViewTextBoxColumn Column1;
        private Button Refresh;

        public DataGridView DGV
        {
            get => dataGridView1;
        }
        public static DeleteForm Form
        {
            get => deleteForm;
        }

        #region Load Data Grid View Overloads

        /// <summary>
        /// Loads a datagridview with an innerjoin statement
        /// </summary>
        /// <param name="cancelAppointmentForm"></param>
        public void LoadDGV(string[] columnNames, DBManager.InnerJoinStatement innerJoin)
        {
            if (innerJoin.Query == "") { MessageBox.Show("Load DGV has no query."); return; }

            DBManager.Initialize_InnerJoin_DGV(DGV, columnNames, innerJoin);
            DGV.Update();
            DGV.Refresh();
        }
        /// <summary>
        /// Loads a datagridview with a Select/Where statement
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="query"></param>
        /// <param name="table"></param>
        /// <param name="whereCondition"></param>
        public void LoadDGV(string[] columnNames, string query, DBManager.Table table, string whereCondition)
        {
            if (query == "") { MessageBox.Show("Load DGV has no query."); return; }

            DBManager.Initialize_SelectWhere_DGV(DGV, columnNames, query, table, whereCondition);
            DGV.Update();
            DGV.Refresh();
        }

        #endregion
    }
}