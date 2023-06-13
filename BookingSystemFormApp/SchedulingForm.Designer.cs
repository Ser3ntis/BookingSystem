using System.Xml;

namespace BookingSystemFormApp
{
    partial class SchedulingForm
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
            button1 = new Button();
            textBox1 = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            SaveException = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.Location = new Point(325, 300);
            button1.Name = "button1";
            button1.Size = new Size(150, 50);
            button1.TabIndex = 0;
            button1.Text = "Save Schedule";
            button1.UseVisualStyleBackColor = true;
            button1.Click += submit_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(112, 17);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 3;
            textBox1.Items.AddRange(DBManager.Functions.GetAllNames(DBManager.UserType.ADMIN, DBManager.Table.ADMIN));
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 20);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 4;
            label1.Text = "Employee ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(75, 84);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 6;
            label2.Text = "Date";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(200, 84);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 7;
            label3.Text = "In Time";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(325, 84);
            label4.Name = "label4";
            label4.Size = new Size(40, 15);
            label4.TabIndex = 8;
            label4.Text = "Lunch";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(450, 84);
            label5.Name = "label5";
            label5.Size = new Size(56, 15);
            label5.TabIndex = 9;
            label5.Text = "Out Time";
            // 
            // SaveException
            // 
            SaveException.Enabled = false;
            SaveException.Location = new Point(150, 300);
            SaveException.Name = "SaveException";
            SaveException.Size = new Size(150, 50);
            SaveException.TabIndex = 10;
            SaveException.Text = "Save Schedule Exception";
            SaveException.UseVisualStyleBackColor = true;
            SaveException.Click += SaveException_Click;
            // 
            // SchedulingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(634, 386);
            Controls.Add(SaveException);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "SchedulingForm";
            Text = "SchedulingForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Button button3;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;

        public string getTextBox1Text()
        {
            return textBox1.Text;
        }

        public void setTextBox1ReadOnly()
        {
            textBox1.Enabled = false;
        }

        public void submitButtonEnable()
        {
            button1.Enabled = true;
            SaveException.Enabled = true;
        }

        private Button SaveException;
        public static ComboBox textBox1;
    }
}