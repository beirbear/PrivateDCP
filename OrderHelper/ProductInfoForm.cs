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
    public partial class ProductInfoForm : Form
    {
        private Space.InfoType infoType;
        private List<Product> productList;
        private bool isEditMode;

        public ProductInfoForm(Space.InfoType type, ref List<Product> productList)
        {
            ///// NEW PRODUCT
            InitializeComponent();
            this.Text = "เดชาพาณิชย์ - ข้อมูลสินค้า";
            this.infoType = type;
            this.productList = productList;
            isEditMode = false;
            cbOperative.Items.AddRange(Space.GetOperativeType());
            cbLocation.Items.AddRange(Space.GetStoredLocationType());

            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        public ProductInfoForm(Space.InfoType type, ref List<Product> productList, Product product)
        {
            //// EDIT PRODUCT
            InitializeComponent();
            this.Text = "เดชาพาณิชย์ - ข้อมูลสินค้า";
            this.productList = productList;
            this.infoType = type;
            isEditMode = true;
            cbOperative.Items.AddRange(Space.GetOperativeType());
            cbLocation.Items.AddRange(Space.GetStoredLocationType());

            txtProdName.Text = product.Name;
            txtProdUnit.Text = product.Unit;
            txtProdPrice.Text = product.Price.ToString();

            if(product.Operative == Space.OperativeType.Male)
                cbOperative.SelectedIndex = 0;
            else if (product.Operative == Space.OperativeType.Female)
                cbOperative.SelectedIndex = 1;
            else
                throw new Exception("Improper Reached");

            if (product.Location == 1)
                cbLocation.SelectedIndex = 0;
            else if (product.Location == 2)
                cbLocation.SelectedIndex = 1;
            else if (product.Location == 3)
                cbLocation.SelectedIndex = 2;
            else if (product.Location == 3.1)
                cbLocation.SelectedIndex = 3;
            else
                throw new Exception("Improper Reached");

            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        public string ProductName
        {
            get { return this.txtProdName.Text;  }
        }

        public string ProductUnit
        {
            get { return this.txtProdUnit.Text;  }
        }

        public double ProductPrice
        {
            get { return double.Parse(this.txtProdPrice.Text); }
        }

        public double ProductLocation
        {
            get {
                List<double> tmps = Space.GetLocationIndex();
                return tmps[cbLocation.SelectedIndex];
            }
        }

        public Product GetProductObject()
        {
            double storedLoc = 0;
            if(cbLocation.SelectedIndex == 0)
                storedLoc = 1;
            else if(cbLocation.SelectedIndex == 1)
                storedLoc = 2;
            else if(cbLocation.SelectedIndex == 2)
                storedLoc = 3;
            else if(cbLocation.SelectedIndex == 3)
                storedLoc = 3.1;
            else
                 throw new Exception("Improper Reached");

            Space.OperativeType optType = Space.OperativeType.Undefined;
            if(cbOperative.SelectedIndex == 0)
                optType = Space.OperativeType.Male;
            else if(cbOperative.SelectedIndex == 1)
                optType = Space.OperativeType.Female;
            else
                throw new Exception("Improper Reached");

            return new Product(txtProdName.Text, double.Parse(txtProdPrice.Text), txtProdUnit.Text, storedLoc, optType);
        }

        public Space.OperativeType ProductOperative
        {
            get {
                if (cbOperative.SelectedItem.ToString() == "ชาย")
                    return Space.OperativeType.Male;
                else if (cbOperative.SelectedItem.ToString() == "หญิง")
                    return Space.OperativeType.Female;

                throw new Exception("Improper Reached");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInformation())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateInformation()
        {
            txtProdName.Text = txtProdName.Text.Trim();
            txtProdUnit.Text = txtProdUnit.Text.Trim();
            txtProdPrice.Text = txtProdPrice.Text.Trim();

            string prodName = txtProdName.Text;
            string prodUnit = txtProdUnit.Text;
            string prodPrice = txtProdPrice.Text;

            if (string.IsNullOrEmpty(prodName) ||
                string.IsNullOrWhiteSpace(prodUnit) ||
                string.IsNullOrEmpty(prodPrice) ||
                cbOperative.SelectedIndex == -1 ||
                cbLocation.SelectedIndex == -1)
            {
                MessageBox.Show("กรุณากรอกข้อมูล และ เลือกข้อมูลให้ถูกต้อง", "กรุณาตรวจสอบข้อมูล");
                return false;
            }

            if (!isEditMode)
            {
                var res = productList.Where(e => e.Name == prodName && e.Unit == prodUnit).SingleOrDefault();

                if (res != null)
                {
                    MessageBox.Show("ข้อมูลดังกล่าว มีอยู่ในระบบ อยู่แล้ว", "กรุณาตรวจสอบข้อมูล");
                    return false;
                }
            }

            return true;
        }

        private void txtProdPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }
    }
}
