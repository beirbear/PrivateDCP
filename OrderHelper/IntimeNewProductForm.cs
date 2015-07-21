using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OrderHelper
{
    public partial class IntimeNewProductForm : Form
    {
        private double storedLocation;
        private Space.OperativeType operativeType;

        public IntimeNewProductForm(string itemName)
        {
            InitializeComponent();

            this.Text = "เดชาพาณิชย์ - เพิ่มสินค้าใหม่ชั่วคราว";

            labelItem.Text = itemName;

            comboBoxStoredLocation.Items.AddRange(Space.GetStoredLocation().OfType<double>().Select(e => e.ToString()).ToArray());
            comboBoxOperativeType.Items.AddRange(new string[] { "ชาย", "หญิง" });

            comboBoxStoredLocation.SelectedIndex = 0;
            comboBoxOperativeType.SelectedIndex = 0;

            textBoxPrice.Text = "0";
        }

        private void buttonOkay_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            if (comboBoxOperativeType.SelectedIndex == 0)
                operativeType = Space.OperativeType.Male;
            else if (comboBoxOperativeType.SelectedIndex == 1)
                operativeType = Space.OperativeType.Female;
            else
                throw new Exception("Impossible option is selected");

            storedLocation = double.Parse(comboBoxStoredLocation.SelectedItem.ToString());

            this.Close();
        }

        public double StoredLocation
        {
            get { return storedLocation;  }
        }

        public Space.OperativeType OperativeType
        {
            get { return this.operativeType;  }
        }

        public double ProductPrice
        {
            get { return double.Parse(textBoxPrice.Text);  }
        }

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // DECIMAL POINT
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
    }
}
