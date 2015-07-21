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
    public partial class ProductEditorForm : Form
    {
        // RESPONSIVE DESIGN REGION
        protected readonly int dgvMinWidth;
        protected readonly int dgvMinHeight;
        protected readonly int txtMinWidth;
        protected readonly int btnAddHeighLoc;

        // Instances for processing
        private readonly Space.InfoType infoType;
        private List<Product> productList;

        public ProductEditorForm(Space.InfoType infoType)
        {
            InitializeComponent();

            dgvMinHeight = dgvGeneral.Height;
            dgvMinWidth = dgvGeneral.Width;
            txtMinWidth = txtInput.Width;
            btnAddHeighLoc = btnNewProduct.Location.Y;

            this.infoType = infoType;

            if (infoType == Space.InfoType.DCP)
                productList = Space.GetProductList();
            else if (infoType == Space.InfoType.AMR)
                productList = Space.GetProductListAmr();

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

            dgvGeneral.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGeneral.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGeneral.RowTemplate.Height = 30;
            dgvGeneral.RowHeadersVisible = false;
            
            ListAllProduct();
        }

        private void ListAllProduct()
        {
            dgvGeneral.Rows.Clear();
            foreach (Product p in productList)
                dgvGeneral.Rows.Add(p.Unit, p.Name, p.Price, OperativeRephraseing(p.Operative), LocationRephrasing(p.Location));    
        }

        private string OperativeRephraseing(Space.OperativeType operative)
        {
            string[] tmp = Space.GetOperativeType();
            if (operative == Space.OperativeType.Male)
                return tmp[0];
            else if (operative == Space.OperativeType.Female)
                return tmp[1];
            else
                throw new Exception("Code should not be reached at this region");
        }

        private string LocationRephrasing(double storedLoc)
        {
            string[] tmp = Space.GetStoredLocationType();
            if(storedLoc == 1)
                return tmp[0];
            else if (storedLoc == 2)
                return tmp[1];
            else if (storedLoc == 3)
                return tmp[2];
            else if (storedLoc == 3.1)
                return tmp[3];
            else
                throw new Exception("Code should not be reached at this region");
        }

        private void dgvGeneral_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            string prodUnit = dgvGeneral.Rows[e.RowIndex].Cells[0].Value.ToString();
            string prodName = dgvGeneral.Rows[e.RowIndex].Cells[1].Value.ToString();
            double prodPrice = double.Parse(dgvGeneral.Rows[e.RowIndex].Cells[2].Value.ToString());
            int pinIdx = -1;

            if (e.ColumnIndex == 5)
            {
                var p = productList.Where(ex => ex.Name == prodName && ex.Unit == prodUnit).SingleOrDefault();

                if(p == null)
                    throw new Exception("How can it reach this regeon");
                
                ProductInfoForm prodInfoForm = new ProductInfoForm(infoType, ref productList, p);
                if (prodInfoForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // REMOVE OLD DATA AND ADD A NEW OBJECT
                    pinIdx = productList.IndexOf(p);

                    if (pinIdx == -1)
                        throw new Exception("How can it reach this regeon");

                    productList.RemoveAt(pinIdx);
                    // ADD A NEW PRODUCT AT A CERTAIN LOCATION
                    Product newProduct = prodInfoForm.GetProductObject();
                    productList.Insert(pinIdx, newProduct);

                    ////// CHECK FOR ROGUE DATA
                    int redundant = productList.Where(ex => ex.Name == newProduct.Name &&
                                            ex.Unit == newProduct.Unit).Count();
                    if (redundant > 1)
                    {
                        MessageBox.Show("คุณใสข้อมูลที่ทับซ้อนกับข้อมูลที่มีอยู่แล้ว", "ข้อมูลไม่ถูกต้อง");
                        productList.RemoveAt(pinIdx);
                        productList.Insert(pinIdx, p);
                    }

                }
                prodInfoForm.Dispose();
                ListAllProduct();

                dgvGeneral.FirstDisplayedScrollingRowIndex = pinIdx;
            }
            else if (e.ColumnIndex == 6)
            {

                string message = "คุณต้องการลบข้อมูลของ " + prodName + " หน่วย" + prodUnit + " ใช่หรือไม่";
                if (MessageBox.Show(message, "กรุณายืนยัน", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    //// DELETE RECORD
                    var delItem = productList.Where(ex => ex.Name == prodName && ex.Unit == prodUnit).SingleOrDefault();

                    if (delItem == null)
                        throw new Exception("How can it reach this regeon");

                    pinIdx = productList.IndexOf(delItem) - 1;

                    if (pinIdx < 0)
                        pinIdx = 0;

                    productList.Remove(delItem);
                    ListAllProduct();

                    dgvGeneral.FirstDisplayedScrollingRowIndex = pinIdx;
                }
            }
                
        }

        private void ListProductByNameFilter(string input)
        {
            var res = productList.Where(e => e.Name.Contains(input));

            if(res == null)
                ListAllProduct();

            dgvGeneral.Rows.Clear();
            foreach(Product p in res)
                dgvGeneral.Rows.Add(p.Unit, p.Name, p.Price, p.Operative, p.Location);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ListProductByNameFilter(txtInput.Text.Trim());
        }

        private void btnNewProduct_Click(object sender, EventArgs e)
        {
            ProductInfoForm newProduct = new ProductInfoForm(infoType, ref productList);
            if (newProduct.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ///// START TO SAVE HERE
                List<Product> prodList = Space.GetProductList();
                if(infoType == Space.InfoType.AMR)
                    prodList = Space.GetProductListAmr();

                prodList.Add(new Product(newProduct.ProductName, newProduct.ProductPrice, newProduct.ProductUnit, newProduct.ProductLocation, newProduct.ProductOperative));
                this.productList = prodList;
                SaveDataToPhysicalDrive();
                ListAllProduct();
                dgvGeneral.FirstDisplayedScrollingRowIndex = dgvGeneral.RowCount - 1;
            }
            newProduct.Dispose();
        }

        private void ProductEditorForm_SizeChanged(object sender, EventArgs e)
        {
            int diffWidth = this.Size.Width - this.MinimumSize.Width;
            int diffHeight = this.Size.Height - this.MinimumSize.Height;

            // Apply size changed
            dgvGeneral.Size = new Size(dgvMinWidth + diffWidth, dgvMinHeight + diffHeight);
            txtInput.Size = new Size(txtMinWidth + diffWidth, txtInput.Height);

            // Apply location changed
            btnNewProduct.Location = new Point(btnNewProduct.Location.X, btnAddHeighLoc + diffHeight);
        }

        private void ProductEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prompt user to save his data
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("คุณต้องการบันทึกการเปลี่ยนแปลงของสินค้า \r\nใช่ หรือ ไม่", "กรุณายืนยัน", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveDataToPhysicalDrive();
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }

        private void SaveDataToPhysicalDrive()
        {
            StreamWriter writer = null;

            if (infoType == Space.InfoType.DCP)
               writer = new StreamWriter(Definition.DCP_PRODUCT_FILE, false, Encoding.UTF8);
            else if(infoType == Space.InfoType.AMR)
               writer = new StreamWriter(Definition.AMR_PRODUCT_FILE, false, Encoding.UTF8);
            
            if(writer != null)
                writer.WriteLine("[");
            
            StringBuilder message = new StringBuilder("");
            for (int i = 0; i < productList.Count; i++)
            {
                message.Clear();
                message.Append("{ ");
                message.Append(string.Format("\"GoodsName\" : \"{0}\", \"Price\" : \"{1}\", \"Unit\" : \"{2}\", \"StoredLocation\" : \"{3}\", \"Operative\" : \"{4}\"",
                    productList[i].Name,
                    productList[i].Price,
                    productList[i].Unit,
                    productList[i].Location,
                    productList[i].OperativeString));
                message.Append(" }");

                if (i < productList.Count - 1)
                    message.Append(",");

                writer.WriteLine(message);
            }
            writer.Write("]");
            writer.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtInput.Text = "";
        }
    }
}
