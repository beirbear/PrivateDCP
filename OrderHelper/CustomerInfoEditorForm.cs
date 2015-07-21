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
    public partial class CustomerInfoEditorForm : Form
    {
        private bool isEditMode;
        private List<CustomerInfo> custList;
        
        public CustomerInfoEditorForm(ref List<CustomerInfo> custList)
        {
            InitializeComponent();

            this.Text = "เดชาพาณิชย์ เพิ่่มหรือแก้ไข ข้อมูลลูกค้า";

            cbCustType.Items.Add("เงินสด");
            cbCustType.Items.Add("เก่าไปใหม่มา");
            cbCustType.SelectedIndex = 0;

            isEditMode = false;
            this.custList = custList;
        }

        public CustomerInfoEditorForm(CustomerInfo custInfo)
        {
            InitializeComponent();

            cbCustType.Items.Add("เงินสด");
            cbCustType.Items.Add("เก่าไปใหม่มา");
            cbCustType.SelectedIndex = 0;

            isEditMode = true;

            txtCustName.Text = custInfo.CustomerName;
            txtCustContact.Text = custInfo.PhoneNumber;

            if (custInfo.PaymentType == "เงินสด")
                cbCustType.SelectedIndex = 0;
            else if (custInfo.PaymentType == "เก่าไปใหม่มา")
                cbCustType.SelectedIndex = 1;
            else
                throw new Exception("Shouldn't reach this area");
        }

        private void txtCustOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private bool ValidateForm()
        {
            txtCustName.Text = txtCustName.Text.Trim();
            txtCustContact.Text = txtCustContact.Text.Trim();

             if(txtCustName.Text.Length == 0)
            {
                MessageBox.Show("กรุณาระบุชื่อของลูกค้า");
                return false;
            }
            else if(txtCustContact.Text.Length == 0)
            {
                MessageBox.Show("กรุณาระบุ เบอร์โทรศัพท์ ที่สามารถติดต่อได้");
                return false;
            }
            else if (cbCustType.SelectedIndex == -1)
            {
                MessageBox.Show("กรุณาระบุ ประเภทบัญชี ของลูกค้า");
                return false;
            }

            return true;
        }

        public CustomerInfo GetCustomerObject()
        {
            string custType = "";
            if (cbCustType.SelectedIndex == 0)
                custType = "Cash";
            else if (cbCustType.SelectedIndex == 1)
                custType = "Credit";
            else
                throw new Exception("Sholdn't reach this area");

            return new CustomerInfo(txtCustName.Text, txtCustContact.Text, custType, 99999);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (isEditMode == false)
            {
                int existingItems = custList.Where(ex => ex.CustomerName == txtCustName.Text.Trim()).Count();
                if (existingItems > 0)
                {
                    MessageBox.Show("ลูกค้าชื่อ " + txtCustName.Text.Trim() + " นั้นมีอยู่ในระบบอยู่แล้ว\r\nกรุณาระบุข้อมูลเพิ่มเติม");
                    return;
                }
            }

            if (ValidateForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }
    }
}
