using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemFormApp
{
    internal class UIController
    {

        private static int xPos = 50;
        private static int yPos_Label = 10;
        private static int yPos_Text = 25;
        private static int offset = 40;


        public static void Label_TextBox(out Label label, out TextBox textBox, string labelText, int layer)
        {
            
            //  ---     Name UI             ---
            label = new Label();
            textBox = new TextBox();
            label.Text =labelText;
            label.SetBounds(xPos, yPos_Label + (offset * layer), 125, 14);
            textBox.SetBounds(xPos, yPos_Text + (offset * layer), 250, 20);
            //  ---                         ---
        }

        public static void Label_Date(out Label label, out DateTimePicker textBox, string labelText, int layer)
        {

            //  ---     Name UI             ---
            label = new Label();
            textBox = new DateTimePicker();
            label.Text = labelText;
            label.SetBounds(xPos, yPos_Label + (offset * layer), 125, 14);
            textBox.SetBounds(xPos, yPos_Text + (offset * layer), 250, 20);
            //  ---                         ---
        }

        public static void Label_CheckBox(out Label label, out CheckBox checkBox, string labelText, int layer)
        {

            //  ---     Name UI             ---
            label = new Label();
            checkBox = new CheckBox();
            label.Text =labelText;
            label.SetBounds(xPos, yPos_Label + (offset * layer), 125, 14);
            checkBox.SetBounds(xPos, yPos_Text + (offset * layer), 250, 20);
            //  ---                         ---
        }

        public static void Label_ComboBox_Stylists(out Label stylistNameLabel, out ComboBox stylistComboBox, int layer)
        {
            //  ---     Stylist UI         ---
            stylistNameLabel = new Label();
            stylistComboBox = new ComboBox();
            DateTime today = DateTime.Today;
            stylistComboBox.Items.AddRange(DBManager.Functions.GetNamesWorking(today.ToString("yyyy-MM-dd")));

            try
            {
                stylistComboBox.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show(string.Format("No stylists are working today.\nPlease contact your systems administrator."));
            }
            stylistNameLabel.Text = "Select Stylist:";
            stylistNameLabel.SetBounds(xPos, yPos_Label + (offset * layer), 125, 14);
            stylistComboBox.SetBounds(xPos, yPos_Text + (offset * layer), 250, 20);
            //  ---                         ---
        }
        public static void Label_ComboBox_Services(out Label serviceNamesLabel, out ComboBox servicesComboBox, int layer)
        {
            //  ---     Services UI          ---
            serviceNamesLabel = new Label();
            servicesComboBox = new ComboBox();
            servicesComboBox.Items.AddRange(ServiceItems.GetAllServiceNames());
            try
            {
                servicesComboBox.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show(string.Format("No services exist at this location.\nPlease contact your systems administrator."));
            }
            serviceNamesLabel.Text = "Select Service:";
            serviceNamesLabel.SetBounds(xPos, yPos_Label + (offset * layer), 125, 14);
            servicesComboBox.SetBounds(xPos, yPos_Text + (offset * layer), 250, 20);
            //  ---                         ---
        }

        public static void Label_CheckedListBox_Services(out Label serviceNamesLabel, out CheckedListBox servicesComboBox, int layer)
        {
            //  ---     Services UI          ---
            serviceNamesLabel = new Label();
            servicesComboBox = new CheckedListBox();
            servicesComboBox.Items.AddRange(ServiceItems.GetAllServiceNames());
            servicesComboBox.CheckOnClick = true;
            /*
            try
            {
                servicesComboBox.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show(string.Format("No services exist at this location.\nPlease contact your systems administrator."));
            }
            */
            serviceNamesLabel.Text = "Select Service:";
            serviceNamesLabel.SetBounds(xPos, yPos_Label + (offset * layer), 125, 14);
            servicesComboBox.SetBounds(xPos, yPos_Text + (offset * layer), 250, 110);
            //  ---                         ---
        }

        public static void ControlButtons(out Button submitButton, out Button cancelButton, int layer)
        {
            //  ---     Control Buttons     ---
            submitButton = new Button();
            cancelButton = new Button();
            submitButton.Text = "Submit";
            cancelButton.Text = "Cancel";
            submitButton.DialogResult = DialogResult.OK;
            cancelButton.DialogResult = DialogResult.Cancel;
            submitButton.SetBounds(xPos, yPos_Label + (offset * layer), 80, 30);
            cancelButton.SetBounds(xPos + 100, yPos_Label + (offset * layer), 80, 30);
            //  ---                         ---
        }
        public static int GetDGVWidth(DataGridView dgv)
        {
            int width = 0;
            for (int index = 0; index < dgv.Columns.Count; index++)
            {
                DataGridViewColumn column = dgv.Columns[index];
                if (column.Visible)
                    width += column.Width;
            }
            return width + 2;
        }
        public static int GetDGVHeight(DataGridView dgv)
        {
            int height = 0;
            for (int index = 0; index < dgv.Columns.Count; index++)
            {
                DataGridViewRow rows = dgv.Rows[index];
                if (rows.Visible)
                {
                    height += rows.Height;
                }
            }
            return height;
        }
    }
}
