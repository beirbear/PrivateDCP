using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace OrderHelper
{
    public partial class CustomerEditorForm : Form
    {
        // RESPONSIVE DESIGN REGION
        protected readonly int dgvMinWidth;
        protected readonly int dgvMinHeight;
        protected readonly int txtMinWidth;
        protected readonly int btnAddHeighLoc;

        // PROCESSING INSTANCES
        private Space.RouteType routeType;
        private List<CustomerInfo> customerList;

        public CustomerEditorForm(Space.RouteType routeType)
        {
            InitializeComponent();

            dgvMinHeight = dgvGeneral.Height;
            dgvMinWidth = dgvGeneral.Width;
            txtMinWidth = txtInput.Width;
            btnAddHeighLoc = btnNewProduct.Location.Y;

            this.Text = "เดชาพาณิชย์ - ข้อมูลลูกค้า";
            this.routeType = routeType;

            DataGridViewImageColumn editColumn = new DataGridViewImageColumn();
            editColumn.Image = Image.FromFile("edit-icon.png");
            editColumn.HeaderText = "";
            editColumn.Name = "Edit";
            dgvGeneral.Columns.Add(editColumn);

            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn();
            deleteColumn.Image = Image.FromFile("delete-icon.png");
            deleteColumn.HeaderText = "";
            deleteColumn.Name = "Delete";
            dgvGeneral.Columns.Add(deleteColumn);

            DataGridViewImageColumn orderUpColumn = new DataGridViewImageColumn();
            orderUpColumn.Image = Image.FromFile("arrow_up.png");
            orderUpColumn.HeaderText = "";
            orderUpColumn.Name = "Up";
            dgvGeneral.Columns.Add(orderUpColumn);

            DataGridViewImageColumn orderDownColumn = new DataGridViewImageColumn();
            orderDownColumn.Image = Image.FromFile("arrow_down.png");
            orderDownColumn.HeaderText = "";
            orderDownColumn.Name = "Down";
            dgvGeneral.Columns.Add(orderDownColumn);

            dgvGeneral.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGeneral.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.RowTemplate.Height = 30;
            dgvGeneral.RowHeadersVisible = false;

            customerList = Space.GetCustomersByRoute(routeType);
            ListAllCustomers();
        }

        private void ListAllCustomers()
        {
            txtInput.Text = "";
            dgvGeneral.Rows.Clear();
            foreach (CustomerInfo cust in customerList)
                dgvGeneral.Rows.Add(cust.Order, cust.CustomerName, cust.PhoneNumber, cust.PaymentType);
        }

        private void dgvGeneral_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            string custName = dgvGeneral.Rows[e.RowIndex].Cells[1].Value.ToString();
            CustomerInfo res = customerList.Where(ex => ex.CustomerName == custName).Single();

            if (e.ColumnIndex == 4)
            {
                // EDIT
                CustomerInfoEditorForm custInfoEditForm = new CustomerInfoEditorForm(res);
                if (custInfoEditForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int idx = customerList.IndexOf(res);
                    customerList.RemoveAt(idx);
                    
                    CustomerInfo editedCust = custInfoEditForm.GetCustomerObject();

                    /*
                    // If order is 0, then became the first order
                    if (editedCust.Order <= 1)
                    {
                        // SET TO TOP OF THE LIST
                        int oldOrder = res.Order;
                        editedCust.SetNewOrder(1);

                        // Upper zone
                        for (int i = 0; i < oldOrder - 1; i++ )
                            customerList[i].SetNewOrder(customerList[i].Order + 1);

                        customerList.Insert(idx, editedCust);
                    }
                    else if (editedCust.Order >= customerList.Count)
                    {
                        // SET TO BUTTOM OF THE LIST
                        int oldOrder = res.Order;
                        editedCust.SetNewOrder(customerList.Count + 1);

                        // Lower zone
                        for (int i = oldOrder - 1; i < customerList.Count; i++)
                            customerList[i].SetNewOrder(customerList[i].Order - 1);

                        customerList.Insert(idx, editedCust);
                    }
                    else
                    {
                        // SET TO THE MIDDLE OF THE LIST
                        int oldOrder = res.Order;
                        
                        // Detec to move up or move down
                        if (oldOrder > editedCust.Order)
                        {
                            // Move Up
                            for (int i = editedCust.Order - 1; i < oldOrder - 1; i++)
                                customerList[i].SetNewOrder(customerList[i].Order + 1);
                        }
                        else if (oldOrder < editedCust.Order)
                        {
                            // Move Down
                            for (int i = oldOrder - 1; i < editedCust.Order - 1; i++)
                                customerList[i].SetNewOrder(customerList[i].Order - 1);
                        }

                        customerList.Insert(idx, editedCust);
                    }

                    */


                    // Sort to keep order in the right place
                    customerList = customerList.OrderBy(ex => ex.Order).ToList<CustomerInfo>();

                    ListAllCustomers();
                }
                custInfoEditForm.Dispose();
            }
            else if(e.ColumnIndex == 5)
            {
                // DELETE
                string message = "คุณต้องการลบข้อมูลลูกค้า คุณ " + custName + " ออกจากระบบ\r\nใช่ หรือ ไม่";
                if (MessageBox.Show(message, "กรุณายืนยัน", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    if (customerList.Count == 1)
                    {
                        MessageBox.Show("ขออภัย\r\nข้อมูลลูกค้าในสาย ต้องมีขั้นต่ำ 1 คน");
                        return;
                    }

                    customerList.Remove(res);
                    ReorderCustomersList();
                    ListAllCustomers();
                }
            }
            else if(e.ColumnIndex == 6)
            {
                if (e.RowIndex == -1)
                    return;

                CustomerInfo item = customerList[e.RowIndex];
                if (e.RowIndex > 1)
                {
                    customerList.RemoveAt(e.RowIndex);
                    customerList.Insert(e.RowIndex - 1, item);
                    ReorderCustomersList();
                    ListAllCustomers();
                    dgvGeneral.FirstDisplayedScrollingRowIndex = e.RowIndex - 1;
                }
            }
            else if(e.ColumnIndex == 7)
            {
                if (e.RowIndex == -1)
                    return;

                CustomerInfo item = customerList[e.RowIndex];
                if (e.RowIndex < customerList.Count - 1)
                {
                    customerList.RemoveAt(e.RowIndex);
                    customerList.Insert(e.RowIndex + 1, item);
                    ReorderCustomersList();
                    ListAllCustomers();
                    dgvGeneral.FirstDisplayedScrollingRowIndex = e.RowIndex + 1;
                }
            }
        }

        private void ReorderCustomersList()
        {
            for(int i = 0; i < customerList.Count; i++)
                customerList[i].SetNewOrder(i + 1);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtInput.Text = "";            
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            // Do string matching
            var queryResult = from CustomerInfo cust in customerList
                              where cust.CustomerName.Contains(txtInput.Text.Trim())
                              select cust;

            dgvGeneral.Rows.Clear();
            foreach (CustomerInfo cust in queryResult)
                dgvGeneral.Rows.Add(cust.Order, cust.CustomerName, cust.PhoneNumber, cust.PaymentType);
            dgvGeneral.Update();

            if (dgvGeneral.Rows.Count != customerList.Count)
            {
                dgvGeneral.Columns[6].Visible = false;
                dgvGeneral.Columns[7].Visible = false;
            }
            else
            {
                dgvGeneral.Columns[6].Visible = true;
                dgvGeneral.Columns[7].Visible = true;
            }
        }

        private void btnNewProduct_Click(object sender, EventArgs e)
        {
            CustomerInfoEditorForm custInfoEditForm = new CustomerInfoEditorForm(ref customerList);
            if (custInfoEditForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CustomerInfo newCust = custInfoEditForm.GetCustomerObject();
                newCust.SetNewOrder(customerList.Count + 1);

                // Insert and Update List
                // List<CustomerInfo> res = customerList.Where(ex => ex.Order >= newCust.Order).ToList<CustomerInfo>();
                /*
                // Positive Value
                if (newCust.Order > 0)
                {
                    // Incase Of New
                    if (res.Count == 0)
                        newCust.SetNewOrder(customerList.Count + 1);
                    else
                    {
                        // Incase of Inserted Order
                        for (int i = 0; i < res.Count(); i++)
                            res[i].SetNewOrder(res[i].Order + 1);
                    }
                }
                // Negative Value
                else
                {
                    newCust.SetNewOrder(1);
                    for (int i = 0; i < customerList.Count; i++)
                        customerList[i].SetNewOrder(customerList[i].Order + 1);
                }
                */

                customerList.Add(newCust);
                ListAllCustomers();
                dgvGeneral.FirstDisplayedScrollingRowIndex = dgvGeneral.RowCount - 1;
            }
            custInfoEditForm.Dispose();
        }

        private void CustomerEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prompt user to save his data
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!hasUniqueOrder())
                {
                    MessageBox.Show("กรุณาแก้ไข ลำดับการจัดส่งให้ถูกต้อง\r\nลำดับการจัดส่งไปสามารถซ้ำกันได้ กรุณาทำการไล่แก้ไขข้อมูลให้ถูกต้อง");
                    e.Cancel = true;
                    return;
                }

                if (MessageBox.Show("คุณต้องการบันทึกการเปลี่ยนแปลงของข้อมูลลูกค้า \r\nใช่ หรือ ไม่", "กรุณายืนยัน", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveDataToPhysicalDrive();
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }

        private bool hasUniqueOrder()
        {
            List<int> orderList = new List<int>();
            foreach (CustomerInfo cust in customerList)
            {
                if (orderList.Contains(cust.Order))
                    return false;

                orderList.Add(cust.Order);
            }

            return true;
        }

        private void SaveDataToPhysicalDrive()
        {
            StreamWriter writer = null;

            if (routeType == Space.RouteType.NgaoPhayao)
                writer = new StreamWriter(Definition.CUSTOMER_NGAOPHAYAO_FILE, false, Encoding.UTF8);
            else if (routeType == Space.RouteType.SobprabThoen)
                writer = new StreamWriter(Definition.CUSTOMER_SOBPRABTHOEN_FILE, false, Encoding.UTF8);
            else if (routeType == Space.RouteType.Wanghnua)
                writer = new StreamWriter(Definition.CUSTOMER_WANGHNUA_FILE, false, Encoding.UTF8);
            else if (routeType == Space.RouteType.Jaehom)
                writer = new StreamWriter(Definition.CUSTOMER_JAEHOM_FILE, false, Encoding.UTF8);
            else if (routeType == Space.RouteType.Local)
                writer = new StreamWriter(Definition.CUSTOMER_LOCAL_FILE, false, Encoding.UTF8);
            else
                throw new Exception("Shouldn't reach this area");

            if (writer != null)
                writer.WriteLine("[");

            StringBuilder message = new StringBuilder("");
            for (int i = 0; i < customerList.Count; i++)
            {
                message.Clear();
                message.Append("{ ");
                message.Append(string.Format("\"CustomerName\": \"{0}\", \"Phones\": \"{1}\", \"Type\": \"{2}\", \"Order\": \"{3}\"",
                    customerList[i].CustomerName,
                    customerList[i].PhoneNumber,
                    PaymentRephrase(customerList[i].PaymentType),
                    customerList[i].Order));
                message.Append(" }");

                if (i < customerList.Count - 1)
                    message.Append(",");

                writer.WriteLine(message);
            }
            writer.Write("]");
            writer.Close();
        }

        private string PaymentRephrase(string input)
        {
            if (input == "เก่าไปใหม่มา")
                return "Credit";
            else if (input == "เงินสด")
                return "Cash";
            else
                throw new Exception("Shouldn't reach this area");

        }

        private void CustomerEditorForm_SizeChanged(object sender, EventArgs e)
        {
            int diffWidth = this.Size.Width - this.MinimumSize.Width;
            int diffHeight = this.Size.Height - this.MinimumSize.Height;

            // Apply size changed
            dgvGeneral.Size = new Size(dgvMinWidth + diffWidth, dgvMinHeight + diffHeight);
            txtInput.Size = new Size(txtMinWidth + diffWidth, txtInput.Height);

            // Apply location changed
            btnNewProduct.Location = new Point(btnNewProduct.Location.X, btnAddHeighLoc + diffHeight);
        }
    }
}
