namespace BookingSystemFormApp
{
    partial class FormController
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormController));
            menuStrip1 = new MenuStrip();
            managementToolStripMenuItem = new ToolStripMenuItem();
            runUnitTestsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            viewScheduleToolStripMenuItem = new ToolStripMenuItem();
            setScheduleToolStripMenuItem = new ToolStripMenuItem();
            functionsToolStripMenuItem = new ToolStripMenuItem();
            customerToolStripMenuItem = new ToolStripMenuItem();
            addToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            appointmentToolStripMenuItem = new ToolStripMenuItem();
            addToolStripMenuItem1 = new ToolStripMenuItem();
            viewToolStripMenuItem1 = new ToolStripMenuItem();
            editToolStripMenuItem1 = new ToolStripMenuItem();
            deleteToolStripMenuItem1 = new ToolStripMenuItem();
            stylistToolStripMenuItem = new ToolStripMenuItem();
            addToolStripMenuItem2 = new ToolStripMenuItem();
            viewToolStripMenuItem2 = new ToolStripMenuItem();
            editToolStripMenuItem2 = new ToolStripMenuItem();
            deleteToolStripMenuItem2 = new ToolStripMenuItem();
            serviceToolStripMenuItem = new ToolStripMenuItem();
            addToolStripMenuItem3 = new ToolStripMenuItem();
            viewToolStripMenuItem3 = new ToolStripMenuItem();
            editToolStripMenuItem3 = new ToolStripMenuItem();
            deleteToolStripMenuItem3 = new ToolStripMenuItem();
            generateReceiptToolStripMenuItem = new ToolStripMenuItem();
            appointmentView = new DataGridView();
            splitContainer1 = new SplitContainer();
            pictureBox1 = new PictureBox();
            lookAheadComboBox = new ComboBox();
            dateTimePicker1 = new DateTimePicker();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)appointmentView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { managementToolStripMenuItem, toolStripMenuItem1, functionsToolStripMenuItem, generateReceiptToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(998, 24);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // managementToolStripMenuItem
            // 
            managementToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { runUnitTestsToolStripMenuItem, helpToolStripMenuItem });
            managementToolStripMenuItem.Name = "managementToolStripMenuItem";
            managementToolStripMenuItem.Size = new Size(90, 20);
            managementToolStripMenuItem.Text = "&Management";
            // 
            // runUnitTestsToolStripMenuItem
            // 
            runUnitTestsToolStripMenuItem.Name = "runUnitTestsToolStripMenuItem";
            runUnitTestsToolStripMenuItem.Size = new Size(148, 22);
            runUnitTestsToolStripMenuItem.Text = "Run Unit Tests";
            runUnitTestsToolStripMenuItem.Click += runUnitTestsToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(148, 22);
            helpToolStripMenuItem.Text = "&Help";
            helpToolStripMenuItem.Click += Help_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { viewScheduleToolStripMenuItem, setScheduleToolStripMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(78, 20);
            toolStripMenuItem1.Text = "&Scheduling";
            // 
            // viewScheduleToolStripMenuItem
            // 
            viewScheduleToolStripMenuItem.Name = "viewScheduleToolStripMenuItem";
            viewScheduleToolStripMenuItem.Size = new Size(150, 22);
            viewScheduleToolStripMenuItem.Text = "&View Schedule";
            viewScheduleToolStripMenuItem.Click += viewScheduleToolStripMenuItem_Click;
            // 
            // setScheduleToolStripMenuItem
            // 
            setScheduleToolStripMenuItem.Name = "setScheduleToolStripMenuItem";
            setScheduleToolStripMenuItem.Size = new Size(150, 22);
            setScheduleToolStripMenuItem.Text = "&Set Schedule";
            setScheduleToolStripMenuItem.Click += setScheduleToolStripMenuItem_Click;
            // 
            // functionsToolStripMenuItem
            // 
            functionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { customerToolStripMenuItem, appointmentToolStripMenuItem, stylistToolStripMenuItem, serviceToolStripMenuItem });
            functionsToolStripMenuItem.Name = "functionsToolStripMenuItem";
            functionsToolStripMenuItem.Size = new Size(71, 20);
            functionsToolStripMenuItem.Text = "&Functions";
            // 
            // customerToolStripMenuItem
            // 
            customerToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addToolStripMenuItem, viewToolStripMenuItem, editToolStripMenuItem, deleteToolStripMenuItem });
            customerToolStripMenuItem.Name = "customerToolStripMenuItem";
            customerToolStripMenuItem.Size = new Size(145, 22);
            customerToolStripMenuItem.Text = "&Customer";
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new Size(162, 22);
            addToolStripMenuItem.Text = "&Add Customer";
            addToolStripMenuItem.Click += AddCustomerToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(162, 22);
            viewToolStripMenuItem.Text = "&View Customer";
            viewToolStripMenuItem.Click += ViewCustomerToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(162, 22);
            editToolStripMenuItem.Text = "&Edit Customer";
            editToolStripMenuItem.Click += EditCustomerToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(162, 22);
            deleteToolStripMenuItem.Text = "&Delete Customer";
            deleteToolStripMenuItem.Click += DeleteCustomerToolStripMenuItem_Click;
            // 
            // appointmentToolStripMenuItem
            // 
            appointmentToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addToolStripMenuItem1, viewToolStripMenuItem1, editToolStripMenuItem1, deleteToolStripMenuItem1 });
            appointmentToolStripMenuItem.Name = "appointmentToolStripMenuItem";
            appointmentToolStripMenuItem.Size = new Size(145, 22);
            appointmentToolStripMenuItem.Text = "&Appointment";
            // 
            // addToolStripMenuItem1
            // 
            addToolStripMenuItem1.Name = "addToolStripMenuItem1";
            addToolStripMenuItem1.Size = new Size(181, 22);
            addToolStripMenuItem1.Text = "&Add Appointment";
            addToolStripMenuItem1.Click += AddAppointmentToolStripMenuItem1_Click;
            // 
            // viewToolStripMenuItem1
            // 
            viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            viewToolStripMenuItem1.Size = new Size(181, 22);
            viewToolStripMenuItem1.Text = "&View Appointment";
            viewToolStripMenuItem1.Click += ViewAppointmentToolStripMenuItem1_Click;
            // 
            // editToolStripMenuItem1
            // 
            editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            editToolStripMenuItem1.Size = new Size(181, 22);
            editToolStripMenuItem1.Text = "&Edit Appointment";
            editToolStripMenuItem1.Click += EditAppointmentToolStripMenuItem1_Click;
            // 
            // deleteToolStripMenuItem1
            // 
            deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            deleteToolStripMenuItem1.Size = new Size(181, 22);
            deleteToolStripMenuItem1.Text = "&Delete Appointment";
            deleteToolStripMenuItem1.Click += DeleteAppointmentToolStripMenuItem1_Click;
            // 
            // stylistToolStripMenuItem
            // 
            stylistToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addToolStripMenuItem2, viewToolStripMenuItem2, editToolStripMenuItem2, deleteToolStripMenuItem2 });
            stylistToolStripMenuItem.Name = "stylistToolStripMenuItem";
            stylistToolStripMenuItem.Size = new Size(145, 22);
            stylistToolStripMenuItem.Text = "&Stylist";
            // 
            // addToolStripMenuItem2
            // 
            addToolStripMenuItem2.Name = "addToolStripMenuItem2";
            addToolStripMenuItem2.Size = new Size(141, 22);
            addToolStripMenuItem2.Text = "&Add Stylist";
            addToolStripMenuItem2.Click += AddStylistToolStripMenuItem2_Click;
            // 
            // viewToolStripMenuItem2
            // 
            viewToolStripMenuItem2.Name = "viewToolStripMenuItem2";
            viewToolStripMenuItem2.Size = new Size(141, 22);
            viewToolStripMenuItem2.Text = "&View Stylist";
            viewToolStripMenuItem2.Click += ViewStylistToolStripMenuItem2_Click;
            // 
            // editToolStripMenuItem2
            // 
            editToolStripMenuItem2.Name = "editToolStripMenuItem2";
            editToolStripMenuItem2.Size = new Size(141, 22);
            editToolStripMenuItem2.Text = "&Edit Stylist";
            editToolStripMenuItem2.Click += EditStylistToolStripMenuItem2_Click;
            // 
            // deleteToolStripMenuItem2
            // 
            deleteToolStripMenuItem2.Name = "deleteToolStripMenuItem2";
            deleteToolStripMenuItem2.Size = new Size(141, 22);
            deleteToolStripMenuItem2.Text = "&Delete Stylist";
            deleteToolStripMenuItem2.Click += DeleteStylistToolStripMenuItem2_Click;
            // 
            // serviceToolStripMenuItem
            // 
            serviceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addToolStripMenuItem3, viewToolStripMenuItem3, editToolStripMenuItem3, deleteToolStripMenuItem3 });
            serviceToolStripMenuItem.Name = "serviceToolStripMenuItem";
            serviceToolStripMenuItem.Size = new Size(145, 22);
            serviceToolStripMenuItem.Text = "&Service";
            // 
            // addToolStripMenuItem3
            // 
            addToolStripMenuItem3.Name = "addToolStripMenuItem3";
            addToolStripMenuItem3.Size = new Size(147, 22);
            addToolStripMenuItem3.Text = "&Add Service";
            addToolStripMenuItem3.Click += AddServiceToolStripMenuItem3_Click;
            // 
            // viewToolStripMenuItem3
            // 
            viewToolStripMenuItem3.Name = "viewToolStripMenuItem3";
            viewToolStripMenuItem3.Size = new Size(147, 22);
            viewToolStripMenuItem3.Text = "&View Service";
            viewToolStripMenuItem3.Click += ViewServiceToolStripMenuItem3_Click;
            // 
            // editToolStripMenuItem3
            // 
            editToolStripMenuItem3.Name = "editToolStripMenuItem3";
            editToolStripMenuItem3.Size = new Size(147, 22);
            editToolStripMenuItem3.Text = "&Edit Service";
            editToolStripMenuItem3.Click += EditServiceToolStripMenuItem3_Click;
            // 
            // deleteToolStripMenuItem3
            // 
            deleteToolStripMenuItem3.Name = "deleteToolStripMenuItem3";
            deleteToolStripMenuItem3.Size = new Size(147, 22);
            deleteToolStripMenuItem3.Text = "&Delete Service";
            deleteToolStripMenuItem3.Click += DeleteServiceToolStripMenuItem3_Click;
            // 
            // generateReceiptToolStripMenuItem
            // 
            generateReceiptToolStripMenuItem.Name = "generateReceiptToolStripMenuItem";
            generateReceiptToolStripMenuItem.Size = new Size(108, 20);
            generateReceiptToolStripMenuItem.Text = "&Generate Receipt";
            generateReceiptToolStripMenuItem.Click += GenerateReceipt_Click;
            // 
            // appointmentView
            // 
            appointmentView.AllowUserToAddRows = false;
            appointmentView.AllowUserToDeleteRows = false;
            appointmentView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            appointmentView.Location = new Point(3, 27);
            appointmentView.Name = "appointmentView";
            appointmentView.ReadOnly = true;
            appointmentView.Size = new Size(531, 441);
            appointmentView.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.Fixed3D;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Margin = new Padding(4, 5, 4, 5);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(appointmentView);
            splitContainer1.Panel2.Controls.Add(lookAheadComboBox);
            splitContainer1.Panel2.Controls.Add(dateTimePicker1);
            splitContainer1.Size = new Size(998, 472);
            splitContainer1.SplitterDistance = 453;
            splitContainer1.TabIndex = 5;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(449, 468);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lookAheadComboBox
            // 
            lookAheadComboBox.FormattingEnabled = true;
            lookAheadComboBox.Items.AddRange(new object[] { "Day", "Week", "Month" });
            lookAheadComboBox.Location = new Point(-1, -1);
            lookAheadComboBox.Margin = new Padding(3, 2, 3, 2);
            lookAheadComboBox.Name = "lookAheadComboBox";
            lookAheadComboBox.Size = new Size(113, 23);
            lookAheadComboBox.TabIndex = 2;
            lookAheadComboBox.SelectedIndex = 0;
            lookAheadComboBox.SelectedIndexChanged += LookAheadComboBox_SelectedIndexChanged;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dateTimePicker1.Location = new Point(109, -1);
            dateTimePicker1.Margin = new Padding(3, 2, 3, 2);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(430, 23);
            dateTimePicker1.TabIndex = 1;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // FormController
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(998, 496);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FormController";
            Text = "Salon Booking System";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)appointmentView).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem managementToolStripMenuItem;
        private SplitContainer splitContainer1;
        private ComboBox lookAheadComboBox;
        private DateTimePicker dateTimePicker1;
        //private ListBox upcomingAppointmentsListBox;
        private DataGridView appointmentView;
        private ToolStripMenuItem runUnitTestsToolStripMenuItem;
        private ToolStripMenuItem schedulingToolStripMenuItem;
        private ToolStripMenuItem viewScheduleToolStripMenuItem;
        private ToolStripMenuItem setScheduleToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private PictureBox pictureBox1;
        private ToolStripMenuItem generateReceiptToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem functionsToolStripMenuItem;
        private ToolStripMenuItem customerToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem appointmentToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem1;
        private ToolStripMenuItem viewToolStripMenuItem1;
        private ToolStripMenuItem editToolStripMenuItem1;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private ToolStripMenuItem stylistToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem2;
        private ToolStripMenuItem viewToolStripMenuItem2;
        private ToolStripMenuItem editToolStripMenuItem2;
        private ToolStripMenuItem deleteToolStripMenuItem2;
        private ToolStripMenuItem serviceToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem3;
        private ToolStripMenuItem viewToolStripMenuItem3;
        private ToolStripMenuItem editToolStripMenuItem3;
        private ToolStripMenuItem deleteToolStripMenuItem3;
    }
}