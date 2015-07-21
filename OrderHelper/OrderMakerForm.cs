using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace OrderHelper
{
    public partial class OrderMakerForm : Form
    {
        // variable for responsive interface
        protected readonly int orderMakerFormMinHeight;
        protected readonly int orderMakerFormMinWidth;
        protected readonly int gbCustomerListMinHeight;
        protected readonly int tbContainerMinWidth;
        protected readonly int tbContainerMinHeight;
        protected readonly int tpMinWidth;
        protected readonly int tpMinHeight;
        protected readonly int lbSuggestItemsMinHeight;
        protected readonly int lbSuggestItemsMinWidth;
        protected readonly int btnClearXLocation;
        // variable for application
        protected readonly Space.RouteType route;
        protected List<CustomerInfo> customersList;
        protected List<Product> productsList;
        protected List<Product> productsListAmr;
        protected Session session;
        protected bool lvInhibit;
        protected int defaultTab;

        public OrderMakerForm(Space.RouteType route, Space.SessionCommand command, string note)
        {
            InitializeComponent();

            this.Text = "เดชาพาณิชย์";

            ImageList imageList = new ImageList();
            imageList.Images.Add(Bitmap.FromFile("shopping-cart-empty-icon.png"));
            imageList.Images.Add(Bitmap.FromFile("shopping-cart-icon.png"));
            imageList.Images.Add(Bitmap.FromFile("shopping-cart-skip-icon.png"));

            lvCustomerList.StateImageList = imageList;

            // lvCustomerList.FullRowSelect = true;
            // lvCustomerList.GridLines = true;
            lvCustomerList.Sorting = SortOrder.Ascending;
            lvCustomerList.MultiSelect = false;

            lvCustomerList.Items[0].StateImageIndex = 0;
            lvCustomerList.Items[1].StateImageIndex = 1;

            orderMakerFormMinHeight = this.Size.Height;
            orderMakerFormMinWidth = this.Size.Width;
            gbCustomerListMinHeight = gbCustomerList.Height;
            tbContainerMinHeight = tbContainer.Size.Height;
            tbContainerMinWidth = tbContainer.Size.Width;

            tpMinHeight = tpDcp.Size.Height;
            tpMinWidth = tpDcp.Size.Width;

            lbSuggestItemsMinHeight = this.lbMatchedItems.Height;
            lbSuggestItemsMinWidth = this.lbMatchedItems.Width;

            btnClearXLocation = btnClear.Location.X;

            tsDefaultMerchant.Items.Add("เรา");
            tsDefaultMerchant.Items.Add("อาม่า");
            tsDefaultMerchant.SelectedIndex = 0;
            defaultTab = 0;

            this.route = route;

            lbMatchedItems.SelectionMode = SelectionMode.One;

            ///// DCP
            dgvDcp.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvDcp.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvDcp.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvDcp.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewColumn dgvcPrice = new DataGridViewTextBoxColumn();
            dgvcPrice.Visible = false;
            dgvcPrice.Name = "Price";
            dgvcPrice.HeaderText = "Price";

            DataGridViewColumn dgvcMultiplier = new DataGridViewTextBoxColumn();
            dgvcMultiplier.Visible = false;
            dgvcMultiplier.Name = "Multiplier";
            dgvcMultiplier.HeaderText = "Multiplier";

            dgvDcp.Columns.Add(dgvcPrice);
            dgvDcp.Columns.Add(dgvcMultiplier);

            dgvDcp.RowTemplate.MinimumHeight = 30;

            ///// AMR
            dgvAmr.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvAmr.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvAmr.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvAmr.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewColumn dgvcPricedgvAmr = new DataGridViewTextBoxColumn();
            dgvcPricedgvAmr.Visible = false;
            dgvcPricedgvAmr.Name = "Price";
            dgvcPricedgvAmr.HeaderText = "Price";

            DataGridViewColumn dgvcMultiplierdgvAmr = new DataGridViewTextBoxColumn();
            dgvcMultiplierdgvAmr.Visible = false;
            dgvcMultiplierdgvAmr.Name = "Multiplier";
            dgvcMultiplierdgvAmr.HeaderText = "Multiplier";

            dgvAmr.Columns.Add(dgvcPricedgvAmr);
            dgvAmr.Columns.Add(dgvcMultiplierdgvAmr);

            dgvAmr.RowTemplate.MinimumHeight = 30;

            if (command == Space.SessionCommand.EditSession)
            {
                // Load data from session
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(note, FileMode.Open, FileAccess.Read, FileShare.Read);
                SerialObject serialObj = (SerialObject)formatter.Deserialize(stream);
                stream.Close();

                this.customersList = serialObj.CustomerList;
                this.productsList = serialObj.ProductList;
                this.productsListAmr = serialObj.ProductListAmr;
                this.session = serialObj.Session;
                this.defaultTab = serialObj.DefaultTab;
                this.route = serialObj.Route;

                ///// Set Topline information
                string[] routeName = Space.GetRouteOption();
                this.labelRouteName.Text = "สาย " + routeName[(int)this.route - 1];
                this.labelSessionDate.Text = serialObj.Date;
                
                ///// Restored Customer List
                lvCustomerList.Items.Clear();

                for (int i = 0; i < customersList.Count; i++)
                {
                    lvCustomerList.Items.Add(customersList[i].CustomerName, customersList[i].CustomerName, 0);
                    lvCustomerList.Items[i].StateImageIndex = 0;
                }
                gbCustomerList.Text = "รายชื่อ ลูกค้า " + string.Format("({0}/{1})", session.GetNumberOfCustomerWhoHaveOrdered(), customersList.Count);

                lvCustomerList.Items[0].Selected = true;

                UpdateEveryCustomerIcon();
            }
            else
            {
                setTopLineInfo();
                InitializeCustomerList();
                InitializeProductList();
                // Create a new seesion
                session = new Session(labelSessionDate.Text, route);
            }
        }

        private void setTopLineInfo()
        {
            string[] routeName = Space.GetRouteOption();
            labelRouteName.Text = "สาย " + routeName[(int)route - 1];

            labelSessionDate.Text = string.Format("{0}/{1}/{2}", System.DateTime.Today.Day, System.DateTime.Today.Month, System.DateTime.Today.Year + 543);
        }

        private void OrderMakerForm_SizeChanged(object sender, EventArgs e)
        {
            // Calculate changed height
            int diffHeight = this.Size.Height - orderMakerFormMinHeight;
            int diffWidth = this.Size.Width - orderMakerFormMinWidth;

            // DCP REGION
            gbCustomerList.Height = gbCustomerListMinHeight + diffHeight;
            tbContainer.Height = tbContainerMinHeight + diffHeight;
            tbContainer.Width = tbContainerMinWidth + diffWidth;
            dgvDcp.Location = new Point(tbContainer.Width / 2, dgvDcp.Location.Y);
            dgvDcp.Size = new Size(tbContainer.Width / 2 - 12, tpMinHeight + diffHeight - 10);
            
            // AMR REGION
            dgvAmr.Location = new Point(tbContainer.Width / 2, dgvDcp.Location.Y);
            dgvAmr.Size = new Size(tbContainer.Width / 2 - 12, tpMinHeight + diffHeight - 10);
            
            int diffTcHeight = tbContainerMinHeight - diffHeight;
            int diffTcWidth = tbContainer.Width - tpMinWidth;

            int newWidthForLeftControl = lbSuggestItemsMinWidth + diffTcWidth / 2;
            lbMatchedItems.Width = newWidthForLeftControl;
            lbMatchedItems.Height = lbSuggestItemsMinHeight + diffHeight;
            tbItemInput.Width = newWidthForLeftControl;
            lbMatchedItemsAmr.Width = newWidthForLeftControl;
            lbMatchedItemsAmr.Height = lbSuggestItemsMinHeight + diffHeight;
            tbItemAmrItem.Width = newWidthForLeftControl;
            
            btnClear.Location = new Point(btnClearXLocation + diffTcWidth / 2, btnClear.Location.Y);
            btnClearAmr.Location = new Point(btnClearXLocation + diffTcWidth / 2, btnClear.Location.Y);
        }

        private void InitializeCustomerList()
        {
            if(route == Space.RouteType.None)
                return;

            customersList = null;
            customersList = Space.GetCustomersByRoute(route);
            if (customersList.Count == 0)
                throw new Exception("There was no customer in the list!!!");

            // Update customer order
            if (session != null)
                session.UpdateDeliveryOrder(customersList);
            
            // Update customer in list
            lvCustomerList.Items.Clear();

            for (int i = 0; i < customersList.Count; i++ )
            {
                lvCustomerList.Items.Add(customersList[i].CustomerName, customersList[i].CustomerName, 0);
                lvCustomerList.Items[i].StateImageIndex = 0;
            }
            gbCustomerList.Text = "รายชื่อ ลูกค้า " + string.Format("({0}/{1})", 0, customersList.Count);

            if (customersList == null)
                return;

            if (customersList.Count == 0)
                return;

            lvCustomerList.Items[0].Selected = true;
        }

        private void InitializeProductList()
        {
            if (productsList != null)
                productsList = null;

            productsList = Space.GetProductList();
            productsListAmr = Space.GetProductListAmr();
        }

        private void DisplayCustomerInfo()
        {
            if (lvCustomerList.SelectedItems.Count == 0)
                return;

            CustomerInfo cust = customersList.Where<CustomerInfo>(x => x.CustomerName == lvCustomerList.SelectedItems[0].Text).Single<CustomerInfo>();
            int custIdx = customersList.IndexOf(cust);

            if (custIdx == -1)
                throw new Exception("Customer cannot found in list");

            labelCustName.Text = customersList[custIdx].CustomerName;
            labelCustType.Text = customersList[custIdx].PaymentType;
            labelCustContact.Text = customersList[custIdx].PhoneNumber;
            chbCustomer.Checked = session.GetCustomerSkipNotification(cust);

            /// DCP REGION
            List<OrderedItem> orderedItem = session.GetCustomerOrder(cust);
            dgvDcp.Rows.Clear();
            
            if (orderedItem != null)
                foreach(OrderedItem item in orderedItem)
                    dgvDcp.Rows.Add(item.Name, item.Unit, item.Amount, item.Note, item.Price, item.Multiplier);
            
            /// AMR REGION
            orderedItem = session.GetCustomerOrderAmr(cust);
            dgvAmr.Rows.Clear();

            if (orderedItem != null)
                foreach (OrderedItem item in orderedItem)
                    dgvAmr.Rows.Add(item.Name, item.Unit, item.Amount, item.Note, item.Price, item.Multiplier);

            tbContainer.SelectedIndex = defaultTab;
            UpdateTotalOrderedItems();
        }

        private double GetProductPrice(string productName, string productUnit)
        {
            var res = productsList.Where(e => e.Name == productName &&
                                                e.Unit == productUnit).FirstOrDefault();

            if (res == null)
            {
                return -1;
            }

            return res.Price;
        }

        private double GetProductPriceAmr(string productName, string productUnit)
        {
            var res = productsListAmr.Where(e => e.Name == productName &&
                                                e.Unit == productUnit).FirstOrDefault();


            if (res == null)
            {
                return -1;
            }

            return res.Price;
        }

        private void lvCustomerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!lvInhibit)
            {
                lvInhibit = true;

                ////// EVERTHING MUST BE DONE AS PROCUDURE, ANY CHANGE MIGHT CASE SOME EFFECT
                if (labelCustName.Text != "-")
                {
                    ////// STEP 1
                    if (UpdateCustomerState() && UpdateCustomerStateAmr())
                    {
                        UpdateCustomerIcon();
                        ////// STEP 2
                        SaveCustomerOrder();
                    }
                    else
                        return;
                }
                ////// DISPLAY CUSTOMER INFO MUST BE DONE AFTER UPDATE CUSTOMER STATE
                DisplayCustomerInfo();
            }
            else
            {
                lvInhibit = false;
            }
        }

        private void VerifyAndStoredData()
        {
            ////// STEP 1
            if (UpdateCustomerState() && UpdateCustomerStateAmr())
            {
                UpdateCustomerIcon();
                ////// STEP 2
                SaveCustomerOrder();
            }
            else
                return;
        }

        private void SaveCustomerOrder()
        {
            ///// Get current customer
            var res = customersList.Where(e => e.CustomerName == labelCustName.Text).Single();
            ///// Create a new orderedItemList

            ///// DCP REGION
            List<OrderedItem> orderedItems = new List<OrderedItem>();
            for (int i = 0; i < dgvDcp.Rows.Count - 1; i++)
            {
                string productName = dgvDcp.Rows[i].Cells[0].EditedFormattedValue.ToString().Trim();
                string productUnit = dgvDcp.Rows[i].Cells[1].EditedFormattedValue.ToString().Trim();
                double productPrice = GetValueFromPriceCell(dgvDcp.Rows[i].Cells[4].EditedFormattedValue.ToString().Trim(), productName, productUnit);
                double productMultiplier = GetValueFromMultiplierCell(dgvDcp.Rows[i].Cells[5].EditedFormattedValue.ToString().Trim());

                orderedItems.Add(new OrderedItem(
                    productName, 
                    productUnit,
                    dgvDcp.Rows[i].Cells[3].EditedFormattedValue.ToString().Trim(),
                    double.Parse(dgvDcp.Rows[i].Cells[2].EditedFormattedValue.ToString()),
                    productMultiplier, 
                    productPrice));
            }

            ///// AMR REGION
            List<OrderedItem> orderedItemsAmr = new List<OrderedItem>();
            for (int i = 0; i < dgvAmr.Rows.Count - 1; i++)
            {
                string productName = dgvAmr.Rows[i].Cells[0].EditedFormattedValue.ToString().Trim();
                string productUnit = dgvAmr.Rows[i].Cells[1].EditedFormattedValue.ToString().Trim();
                double productPrice = GetValueFromPriceCellAmr(dgvAmr.Rows[i].Cells[4].EditedFormattedValue.ToString().Trim(), productName, productUnit);
                double productMultiplier = GetValueFromMultiplierCell(dgvAmr.Rows[i].Cells[5].EditedFormattedValue.ToString().Trim());

                orderedItemsAmr.Add(new OrderedItem(
                    productName,
                    productUnit,
                    dgvAmr.Rows[i].Cells[3].EditedFormattedValue.ToString().Trim(),
                    double.Parse(dgvAmr.Rows[i].Cells[2].EditedFormattedValue.ToString()),
                    productMultiplier,
                    productPrice));
            }

            session.SaveCustomerOrder(res, orderedItems, orderedItemsAmr, chbCustomer.Checked);
        }
        
        private double GetValueFromPriceCell(string input, string productName, string productUnit)
        {
            if (input == "")
                return GetProductPrice(productName, productUnit);

            return double.Parse(input);
        }

        private double GetValueFromPriceCellAmr(string input, string productName, string productUnit)
        {
            if (input == "")
                return GetProductPriceAmr(productName, productUnit);

            return double.Parse(input);
        }

        private double GetValueFromMultiplierCell(string input)
        {
            if (input == "")
                return 1d;

            if (Space.IsNumeric(input))
                return double.Parse(input);

            return double.Parse(input);
        }

        private bool UpdateCustomerState()
        {
            // Need to check that user make a valid input before move to other customer
            for (int i = 0; i < dgvDcp.RowCount - 1; i++)
            {
                if (string.IsNullOrWhiteSpace(dgvDcp.Rows[i].Cells[0].EditedFormattedValue.ToString()) ||
                   string.IsNullOrWhiteSpace(dgvDcp.Rows[i].Cells[1].EditedFormattedValue.ToString()) ||
                   !Space.IsNumeric(dgvDcp.Rows[i].Cells[2].EditedFormattedValue.ToString()))
                {
                    string errorMessage =  string.Format("กรณาตรวจสอบข้อมูลให้ถูกต้องใน บรรทัดที่ {0} ของ เรา\r\nเงื่อนไขคือ ชื่อสินค้า, หน่วยของสินค้า และจำนวน ห้ามเว้นว่างและ ช่องจำนวนต้องระบุเป็นตัวเลขเท่านั้น", i + 1);
                    MessageBox.Show(errorMessage, "ข้อมูลไม่ถูกต้อง", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbContainer.SelectedIndex = 0;
                    return false;
                }

                // Check for product
                string productName = dgvDcp.Rows[i].Cells[0].EditedFormattedValue.ToString();
                string productUnit = dgvDcp.Rows[i].Cells[1].EditedFormattedValue.ToString();
                if (GetProductPrice(productName, productUnit) == -1)
                {
                    // There is no such a product in the list, add it manually
                    IntimeNewProductForm inpf = new IntimeNewProductForm(productName + " : " + productUnit);
                    if (inpf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        productsList.Add(new Product(productName, inpf.ProductPrice, productUnit, inpf.StoredLocation, inpf.OperativeType));
                    else
                        return false;
                }
            }

            ///// Check for duplicated items, this can happen when user change product manually
            List<OrderedItem> duplicateCheckList = new List<OrderedItem>();
            for (int i = 0; i < dgvDcp.RowCount - 1; i++)
            {
                string prodName = dgvDcp.Rows[i].Cells[0].EditedFormattedValue.ToString();
                string prodUnit = dgvDcp.Rows[i].Cells[1].EditedFormattedValue.ToString();
                string prodNote = dgvDcp.Rows[i].Cells[3].EditedFormattedValue.ToString();
                
                int res = duplicateCheckList.Where(e => e.Name == prodName &&
                                                e.Unit == prodUnit &&
                                                e.Note == prodNote).Count();

                if (res == 0)
                    duplicateCheckList.Add(new OrderedItem(prodName, prodUnit, prodNote, 1, 1, 1));
                else
                {
                    string errorMessage = string.Format("สินค้า: {0}\r\nหน่วย: {1}\r\nหมายเหตุ: {2}\r\nซ้ำกันในรายการสินค้า กรุณา ลบ ออกอันไดอันหนึ่ง", prodName, prodUnit, (prodNote == ""? "-": prodNote) );
                    MessageBox.Show(errorMessage);
                    return false;
                }
            }

            gbCustomerList.Text = "รายชื่อ ลูกค้า " + string.Format("({0}/{1})", session.GetNumberOfCustomerWhoHaveOrdered(), customersList.Count);

            return true;
        }

        private bool UpdateCustomerStateAmr()
        {
            // Need to check that user make a valid input before move to other customer
            for (int i = 0; i < dgvAmr.RowCount - 1; i++)
            {
                if (string.IsNullOrWhiteSpace(dgvAmr.Rows[i].Cells[0].EditedFormattedValue.ToString()) ||
                   string.IsNullOrWhiteSpace(dgvAmr.Rows[i].Cells[1].EditedFormattedValue.ToString()) ||
                   !Space.IsNumeric(dgvAmr.Rows[i].Cells[2].EditedFormattedValue.ToString()))
                {
                    string errorMessage = string.Format("กรณาตรวจสอบข้อมูลให้ถูกต้องใน บรรทัดที่ {0} ของ อาม่า\r\nเงื่อนไขคือ ชื่อสินค้า, หน่วยของสินค้า และจำนวน ห้ามเว้นว่างและ ช่องจำนวนต้องระบุเป็นตัวเลขเท่านั้น", i + 1);
                    MessageBox.Show(errorMessage, "ข้อมูลไม่ถูกต้อง", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbContainer.SelectedIndex = 1;
                    return false;
                }

                // Check for product
                string productName = dgvAmr.Rows[i].Cells[0].EditedFormattedValue.ToString();
                string productUnit = dgvAmr.Rows[i].Cells[1].EditedFormattedValue.ToString();
                if (GetProductPriceAmr(productName, productUnit) == -1)
                {
                    // There is no such a product in the list, add it manually
                    IntimeNewProductForm inpf = new IntimeNewProductForm(productName + " : " + productUnit);
                    if (inpf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        productsListAmr.Add(new Product(productName, inpf.ProductPrice, productUnit, inpf.StoredLocation, inpf.OperativeType));
                    else
                        return false;
                }
            }

            ///// Check for duplicated items, this can happen when user change product manually
            List<OrderedItem> duplicateCheckList = new List<OrderedItem>();
            for (int i = 0; i < dgvAmr.RowCount - 1; i++)
            {
                string prodName = dgvAmr.Rows[i].Cells[0].EditedFormattedValue.ToString();
                string prodUnit = dgvAmr.Rows[i].Cells[1].EditedFormattedValue.ToString();
                string prodNote = dgvAmr.Rows[i].Cells[3].EditedFormattedValue.ToString();

                int res = duplicateCheckList.Where(e => e.Name == prodName &&
                                                e.Unit == prodUnit &&
                                                e.Note == prodNote).Count();

                if (res == 0)
                    duplicateCheckList.Add(new OrderedItem(prodName, prodUnit, prodNote, 1, 1, 1));
                else
                {
                    string errorMessage = string.Format("สินค้า: {0}\r\nหน่วย: {1}\r\nหมายเหตุ: {2}\r\nซ้ำกันในรายการสินค้า กรุณา ลบ ออกอันไดอันหนึ่ง", prodName, prodUnit, (prodNote == "" ? "-" : prodNote));
                    MessageBox.Show(errorMessage);
                    return false;
                }
            }

            gbCustomerList.Text = "รายชื่อ ลูกค้า " + string.Format("({0}/{1})", session.GetNumberOfCustomerWhoHaveOrdered(), customersList.Count);

            return true;
        }

        private void UpdateCustomerIcon()
        {
            // When there is nothing error about user input we save current customer state icon
            int tmp = lvCustomerList.Items.IndexOfKey(labelCustName.Text);

            if (chbCustomer.Checked == true)
            {
                lvCustomerList.Items[tmp].StateImageIndex = 2;
                return;
            }

            if (dgvDcp.Rows.Count - 1 > 0 || dgvAmr.Rows.Count - 1 > 0)
                lvCustomerList.Items[tmp].StateImageIndex = 1;
            else
                lvCustomerList.Items[tmp].StateImageIndex = 0;
        }

        private void UpdateEveryCustomerIcon()
        {
            for (int i = 0; i < customersList.Count; i++)
            {
                int tmp = lvCustomerList.Items.IndexOfKey(customersList[i].CustomerName);

                if (session.CustomerHasSkip(customersList[i].CustomerName))
                    lvCustomerList.Items[tmp].StateImageIndex = 2;
                else if (session.CustomerHasOrdered(customersList[i].CustomerName))
                    lvCustomerList.Items[tmp].StateImageIndex = 1;
                else
                    lvCustomerList.Items[tmp].StateImageIndex = 0;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbItemInput.Text = "";
        }

        private void tbItemInput_TextChanged(object sender, EventArgs e)
        {
            // Do string matching
            var queryResult = from Product product in productsList
                              where product.Name.Contains(tbItemInput.Text.Trim())
                              select product;

            lbMatchedItems.Items.Clear();
            foreach (Product p in queryResult)
                lbMatchedItems.Items.Add(string.Format("{0} :{1}", p.Name, p.Unit));
            lbMatchedItems.Update();
        }

        private void lbMatchedItems_DoubleClick(object sender, EventArgs e)
        {
            if (lbMatchedItems.SelectedItem == null)
                return;

            string[] items = lbMatchedItems.SelectedItem.ToString().Split(':');
            string itemName = items[0].Trim();
            string itemUnit = items[1].Trim();

            // Check if product is exist
            bool IsExist = dgvDcp.Rows.Cast<DataGridViewRow>()
                                   .Count(c => 
                                       (!string.IsNullOrWhiteSpace(c.Cells[0].EditedFormattedValue.ToString()) &&
                                       c.Cells[0].EditedFormattedValue.ToString().Trim() == itemName) &&
                                       (!string.IsNullOrWhiteSpace(c.Cells[1].EditedFormattedValue.ToString()) &&
                                       c.Cells[1].EditedFormattedValue.ToString().Trim() == itemUnit)
                                    ) == 1;

            try
            {
                if (IsExist)
                {
                    int rowIndex = -1;

                    DataGridViewRow row = dgvDcp.Rows
                        .Cast<DataGridViewRow>()
                        .Where(r => r.Cells[0].Value.ToString().Equals(itemName) &&
                                    r.Cells[1].Value.ToString().Equals(itemUnit))
                        .First();

                    rowIndex = row.Index;

                    dgvDcp.Rows[rowIndex].Cells[2].Value = (int.Parse(dgvDcp.Rows[rowIndex].Cells[2].Value.ToString()) + 1).ToString();
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }

            dgvDcp.Rows.Add(itemName, itemUnit ,1,"");
            dgvDcp.FirstDisplayedScrollingRowIndex = dgvDcp.RowCount - 1;
            UpdateTotalOrderedItems();
        }

        private void dgvDcp_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateTotalOrderedItems();
        }

        private void UpdateTotalOrderedItems()
        {
            label_totalProductsDcp.Text = (dgvDcp.Rows.Count - 1).ToString();
            label_totalProductAmr.Text = (dgvAmr.Rows.Count - 1).ToString();
        }

        private void tsbViewInvoice_Click(object sender, EventArgs e)
        {
            VerifyAndStoredData();

            ///// Get current customer
            var res = customersList.Where(x => x.CustomerName == labelCustName.Text).Single();
                
            if(chbCustomer.Checked)
            {
                MessageBox.Show("ในนามของ " + res.CustomerName + " ขอผ่าน ค่ะ", "ไม่สามารถดำเนินการได้", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (tbContainer.SelectedIndex == 0)
            {
                if (dgvDcp.Rows.Count == 1)
                {
                    MessageBox.Show("ไม่มีรายการสั้งของ ในนามของ " + res.CustomerName + " ในส่วนของ เรา ค่ะ", "ไม่สามารถดำเนินการได้", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                ///// Create a new orderedItemList
                List<OrderedItem> orderedItems = new List<OrderedItem>();
                for (int i = 0; i < dgvDcp.Rows.Count - 1; i++)
                {
                    string productName = dgvDcp.Rows[i].Cells[0].EditedFormattedValue.ToString().Trim();
                    string productUnit = dgvDcp.Rows[i].Cells[1].EditedFormattedValue.ToString().Trim();
                    double productPrice = GetValueFromPriceCell(dgvDcp.Rows[i].Cells[4].EditedFormattedValue.ToString().Trim(), productName, productUnit);
                    double productMultiplier = GetValueFromMultiplierCell(dgvDcp.Rows[i].Cells[5].EditedFormattedValue.ToString().Trim());

                    orderedItems.Add(new OrderedItem(
                        productName,
                        productUnit,
                        dgvDcp.Rows[i].Cells[3].EditedFormattedValue.ToString().Trim(),
                        double.Parse(dgvDcp.Rows[i].Cells[2].EditedFormattedValue.ToString()),
                        productMultiplier,
                        productPrice));
                }

                //// orderedItems will be changed according to referecing
                OrderReviewForm orderReviewForm = new OrderReviewForm(res, ref orderedItems);
                orderReviewForm.ShowDialog();

                orderedItems = orderReviewForm.GetCustomerOrdered();

                dgvDcp.Rows.Clear();
                foreach (OrderedItem item in orderedItems)
                    dgvDcp.Rows.Add(item.Name, item.Unit, item.Amount, item.Note, item.Price, item.Multiplier);
            }
            else if (tbContainer.SelectedIndex == 1)
            {
                if (dgvAmr.Rows.Count == 1)
                {
                    MessageBox.Show("ไม่มีรายการสั้งของ ในนามของ " + res.CustomerName + " ในส่วนของ อาม่า ค่ะ", "ไม่สามารถดำเนินการได้", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                ///// Create a new orderedItemList
                List<OrderedItem> orderedItems = new List<OrderedItem>();
                for (int i = 0; i < dgvAmr.Rows.Count - 1; i++)
                {
                    string productName = dgvAmr.Rows[i].Cells[0].EditedFormattedValue.ToString().Trim();
                    string productUnit = dgvAmr.Rows[i].Cells[1].EditedFormattedValue.ToString().Trim();
                    double productPrice = GetValueFromPriceCellAmr(dgvAmr.Rows[i].Cells[4].EditedFormattedValue.ToString().Trim(), productName, productUnit);
                    double productMultiplier = GetValueFromMultiplierCell(dgvAmr.Rows[i].Cells[5].EditedFormattedValue.ToString().Trim());

                    orderedItems.Add(new OrderedItem(
                        productName,
                        productUnit,
                        dgvAmr.Rows[i].Cells[3].EditedFormattedValue.ToString().Trim(),
                        double.Parse(dgvAmr.Rows[i].Cells[2].EditedFormattedValue.ToString()),
                        productMultiplier,
                        productPrice));
                }

                //// orderedItems will be changed according to referecing
                OrderReviewForm orderReviewForm = new OrderReviewForm(res, ref orderedItems);
                orderReviewForm.ShowDialog();

                orderedItems = orderReviewForm.GetCustomerOrdered();

                dgvAmr.Rows.Clear();
                foreach (OrderedItem item in orderedItems)
                    dgvAmr.Rows.Add(item.Name, item.Unit, item.Amount, item.Note, item.Price, item.Multiplier);
            }
            UpdateTotalOrderedItems();
        }

        private void btnClearAmr_Click(object sender, EventArgs e)
        {
            tbItemAmrItem.Text = "";
        }

        private void tbItemAmrItem_TextChanged(object sender, EventArgs e)
        {
            // Do string matching
            var queryResult = from Product product in productsListAmr
                              where product.Name.Contains(tbItemAmrItem.Text.Trim())
                              select product;

            lbMatchedItemsAmr.Items.Clear();
            foreach (Product p in queryResult)
                lbMatchedItemsAmr.Items.Add(string.Format("{0} :{1}", p.Name, p.Unit));
            lbMatchedItemsAmr.Update();
        }

        private void lbMatchedItemsAmr_DoubleClick(object sender, EventArgs e)
        {
            if (lbMatchedItemsAmr.SelectedItem == null)
                return;

            string[] items = lbMatchedItemsAmr.SelectedItem.ToString().Split(':');
            string itemName = items[0].Trim();
            string itemUnit = items[1].Trim();

            // Check if product is exist
            bool IsExist = dgvAmr.Rows.Cast<DataGridViewRow>()
                                   .Count(c =>
                                       (!string.IsNullOrWhiteSpace(c.Cells[0].EditedFormattedValue.ToString()) &&
                                       c.Cells[0].EditedFormattedValue.ToString().Trim() == itemName) &&
                                       (!string.IsNullOrWhiteSpace(c.Cells[1].EditedFormattedValue.ToString()) &&
                                       c.Cells[1].EditedFormattedValue.ToString().Trim() == itemUnit)
                                    ) == 1;

            try
            {
                if (IsExist)
                {
                    int rowIndex = -1;

                    DataGridViewRow row = dgvAmr.Rows
                        .Cast<DataGridViewRow>()
                        .Where(r => r.Cells[0].Value.ToString().Equals(itemName) &&
                                    r.Cells[1].Value.ToString().Equals(itemUnit))
                        .First();

                    rowIndex = row.Index;

                    dgvAmr.Rows[rowIndex].Cells[2].Value = (int.Parse(dgvAmr.Rows[rowIndex].Cells[2].Value.ToString()) + 1).ToString();
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }

            dgvAmr.Rows.Add(itemName, itemUnit, 1, "");
            dgvAmr.FirstDisplayedScrollingRowIndex = dgvAmr.RowCount - 1;
            UpdateTotalOrderedItems();
        }

        private void tbContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbContainer.SelectedIndex == 0)
                tslTabIndicator.Text = "เรา";
            else if (tbContainer.SelectedIndex == 1)
                tslTabIndicator.Text = "อาม่า";
        }

        private void tsDefaultMerchant_SelectedIndexChanged(object sender, EventArgs e)
        {
            defaultTab = tsDefaultMerchant.SelectedIndex;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            VerifyAndStoredData();

            ItemsPool itemPool;
            ItemsPool itemPoolAmr;

            session.GetItemListRound(out itemPool, out itemPoolAmr);

            // Stored Location and Operative Type must be bind bofore send itemPool to data
            itemPool.UpdateStoredLocationAndOperative(ref productsList);

            // AMR category doesnt need to be bind because there is only one staff who take care about it

            ItemsListForm itemListForm = new ItemsListForm(itemPool, itemPoolAmr, labelSessionDate.Text, labelRouteName.Text);
            itemListForm.ShowDialog();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            SaveDataToPhysicalDrive(true);
        }

        private void SaveDataToPhysicalDrive(bool isInform)
        {
            VerifyAndStoredData();

            ///// NAMING CONVENTION
            ///// ROUTE_NAME . DATE . EXTENSION
            string serialName = string.Format("{0}.{1}.data", route.ToString(), labelSessionDate.Text.Replace("/", "_"));

            // Create a serial object
            SerialObject serialObj = new SerialObject(route,
                                                        customersList,
                                                        productsList,
                                                        productsListAmr,
                                                        session,
                                                        defaultTab,
                                                        labelSessionDate.Text);

            // Serialization
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(serialName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, serialObj);
            stream.Close();

            if(isInform)
                MessageBox.Show("บันทึกข้อมูล เสร็จสิ้น");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("คุณต้องการลบ ข้อมูลของรายการสินค้าที่ลิสท์ไปแล้ว ใช่หรือไม่", "กรุณายืนยัน การทำรายการ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                session.ClearCustomerCache();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            VerifyAndStoredData();

            ItemsPool itemPool;
            ItemsPool itemPoolAmr;

            session.GetItemListAll(out itemPool, out itemPoolAmr);

            // Stored Location and Operative Type must be bind bofore send itemPool to data
            itemPool.UpdateStoredLocationAndOperative(ref productsList);

            // AMR category doesnt need to be bind because there is only one staff who take care about it

            ItemsListForm itemListForm = new ItemsListForm(itemPool, itemPoolAmr, labelSessionDate.Text, labelRouteName.Text);
            itemListForm.ShowDialog();
        }

        private void OrderMakerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prompt user to save his data
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("คุณต้องการออกจากโปรแกรม \r\nใช่ หรือ ไม่", "กรุณายืนยัน", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
                else
                   SaveDataToPhysicalDrive(false);
            }
            else if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                // Autosave and clear up ressources
                SaveDataToPhysicalDrive(false);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            DeliveryViewForm dvForm = new DeliveryViewForm(session.GetCustomersList());
            dvForm.ShowDialog();
            dvForm.Dispose();
        }

        private void เราToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasswordForm passForm = new PasswordForm("(หมายเหตุ!!! สินค้าที่ถูกเพิ่มชั่วคราวจะหายไป)");
            if (passForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ProductEditorForm pdeForm = new ProductEditorForm(Space.InfoType.DCP);
                if (pdeForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // RELOAD PRODUCT DATA
                    List<Product> newProdList = Space.GetProductList();
                    for (int i = 0; i < productsList.Count; i++)
                    {
                        var res = newProdList.Where(ex =>  ex.Name == productsList[i].Name && 
                                                    ex.Unit == productsList[i].Unit).SingleOrDefault();

                        if (res == null)
                            newProdList.Add(productsList[i]);
                    }
                    this.productsList = newProdList;
                }
                pdeForm.Dispose();
            }
            passForm.Dispose();
        }

        private void อามาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasswordForm passForm = new PasswordForm("(หมายเหตุ!!! สินค้าที่ถูกเพิ่มชั่วคราวจะหายไป)");
            if (passForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ProductEditorForm pdeForm = new ProductEditorForm(Space.InfoType.AMR);
                if (pdeForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // RELOAD PRODUCT DATA
                    this.productsListAmr = Space.GetProductListAmr();
                }
                pdeForm.Dispose();
            }
            passForm.Dispose();
        }

        private void แกไขขอมลลกคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ///// MOVE TO OUTSIDE
            PasswordForm passForm = new PasswordForm("(หมายเหตุ!!! กรุณาแก้ไขด้วยความระมัดระวัง)");
            if (passForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CustomerEditorForm custEditerForm = new CustomerEditorForm(route);
                if (custEditerForm.ShowDialog() == DialogResult.OK)
                {
                    // Update customer list
                    InitializeCustomerList();
                    UpdateCustomerState();
                    UpdateCustomerStateAmr();
                    UpdateEveryCustomerIcon();
                    DisplayCustomerInfo();

                    if (lvInhibit)
                        lvInhibit = false;
                    else
                        lvInhibit = true;
                }
                custEditerForm.Close();
            }
            passForm.Close();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            VerifyAndStoredData();

            ViewOrderedItemForm voiForm = new ViewOrderedItemForm(ref session);
            voiForm.ShowDialog();
            voiForm.Dispose();
        }

        private void chbCustomer_CheckedChanged(object sender, EventArgs e)
        {
            // Update Skip
            if (chbCustomer.Checked)
            {
                tbContainer.Enabled = false;
                dgvAmr.Enabled = false;
                dgvDcp.Enabled = false;
            }
            else
            {
                tbContainer.Enabled = true;
                dgvAmr.Enabled = true;
                dgvDcp.Enabled = true;
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            VerifyAndStoredData();

            AllBillsForm abf = new AllBillsForm(ref session, labelRouteName.Text, labelSessionDate.Text);
            abf.Show();
        }
    }
}
