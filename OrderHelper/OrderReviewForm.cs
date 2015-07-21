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
    public partial class OrderReviewForm : Form
    {
        // Variable for responsive design
        protected readonly int dgvMinWidth;
        protected readonly int dgvMinHeight;
        protected readonly int btnRefreshDefaultY;
        protected readonly int labelTotalDefaultY;

        // Instance for processing
        protected CustomerInfo custInfo;
        protected List<OrderedItem> custOrder;
        protected List<string> unitIndex;
        
        public OrderReviewForm(CustomerInfo custInfo, ref List<OrderedItem> custOrder)
        {
            InitializeComponent();

            this.Text = "เดชาพาณิชย์ - บิลลูกค้า";

            this.custInfo = custInfo;
            this.custOrder = custOrder;

            this.dgvMinWidth = dataGridView1.Width;
            this.dgvMinHeight = dataGridView1.Height;
            this.btnRefreshDefaultY = buttonRecalculate.Location.Y;
            this.labelTotalDefaultY = labelGrandTotal.Location.Y;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.RowHeadersVisible = false;

            unitIndex = Space.GetUnitIndex();

            SortOrderedItem();
            UpdateDisplay();
            UpdateCalculation();
        }

        private int UnitCount
        {
            get { return unitIndex.Count;  }
        }

        public int GetUnitWeightLevel(string unit)
        {
            
            return unitIndex.IndexOf(unit);
        }

        private void UpdateDisplay()
        {
            labelCustName.Text = custInfo.CustomerName;
            labelCustType.Text = custInfo.PaymentType;
            labelContact.Text = custInfo.PhoneNumber;
            labelDelieryOrder.Text = custInfo.Order.ToString();

            for (int i = 0; i < custOrder.Count; i++)
                dataGridView1.Rows.Add(custOrder[i].Amount, custOrder[i].Unit, GetNameInline(custOrder[i].Name, custOrder[i].Note), custOrder[i].Multiplier, custOrder[i].Price, "");
        }

        private bool UpdateCalculation()
        {
            double grandTotal = 0;
            bool rottenValue = false;

            ///// MAKE SURE THAT TABLE CONTAIN VALID DATA
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                double amount = (Space.IsNumeric(dataGridView1.Rows[i].Cells[0].Value.ToString())? double.Parse(dataGridView1.Rows[i].Cells[0].EditedFormattedValue.ToString()) : -1d);
                double multiplier = (Space.IsNumeric(dataGridView1.Rows[i].Cells[3].Value.ToString()) ? double.Parse(dataGridView1.Rows[i].Cells[3].EditedFormattedValue.ToString()) : -1d);
                double price = (Space.IsNumeric(dataGridView1.Rows[i].Cells[4].Value.ToString()) ? double.Parse(dataGridView1.Rows[i].Cells[4].EditedFormattedValue.ToString()) : -1d);
                if (amount > -1 && multiplier > -1 && price > -1)
                {
                    double total = 0;
                    if (multiplier > 1)
                        total = multiplier * price;
                    else
                        total = amount * price;
                    dataGridView1.Rows[i].Cells[5].Value = total.ToString("#,##0.##");
                    grandTotal += total;
                }
                else
                {
                    rottenValue = true;
                    dataGridView1.Rows[i].Cells[5].Value = "-";
                }
            }
            
            ///// CALCULATE
            if (rottenValue)
                labelGrandTotal.Text = "-";
            else
                labelGrandTotal.Text = grandTotal.ToString("#,##0.00");

            return !rottenValue;
        }

        private string GetNameInline(string productName, string productNote)
        {
            if (productNote.Trim().Length == 0)
                return productName;

            return string.Format("{0} ({1})", productName, productNote);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateCalculation();
        }

        private void buttonRecalculate_Click(object sender, EventArgs e)
        {
            UpdateCalculation();
        }

        private void OrderReviewForm_SizeChanged(object sender, EventArgs e)
        {
            // Implement responsive design
            int diffWidth = this.Size.Width - this.MinimumSize.Width;
            int diffHeight = this.Size.Height - this.MinimumSize.Height;

            dataGridView1.Size = new Size(dgvMinWidth + diffWidth, dgvMinHeight + diffHeight);
            buttonRecalculate.Location = new Point(buttonRecalculate.Location.X, btnRefreshDefaultY + diffHeight);
            label5.Location = new Point(label5.Location.X, labelTotalDefaultY + diffHeight);
            labelGrandTotal.Location = new Point(labelGrandTotal.Location.X, labelTotalDefaultY + diffHeight);
        }

        private void SortOrderedItem()
        {
            List<OrderedItem> orderedBuffer = new List<OrderedItem>();

            ////// ADD SORTED ITEM TO LIST
            for (int i = 0; i < UnitCount; i++)
            {
                for (int j = 0; j < custOrder.Count; j++)
                {
                    if (GetUnitWeightLevel(custOrder[j].Unit) == i)
                    {
                        orderedBuffer.Add(custOrder[j]);
                        custOrder.RemoveAt(j);
                        j--;
                    }
                }
            }


            ///// WHEN THERE IS REMIAINED ITEM IN LIST, APPEND IT
            for (int i = 0; i < custOrder.Count; i++)
                orderedBuffer.Add(custOrder[i]);

            ///// THEN, EVERYTHING IS MOVED TO orderedBuffer
            custOrder = orderedBuffer;
        }

        private double GetValueFromMultiplierCell(string input)
        {
            if (Space.IsNumeric(input))
                return double.Parse(input);

            return 1d;
        }

        public void UpdateCustomerOrdered()
        {
            UpdateCalculation();
            
            List<OrderedItem> orderedItems = new List<OrderedItem>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string productName = dataGridView1.Rows[i].Cells[2].EditedFormattedValue.ToString().Trim();
                string productUnit = dataGridView1.Rows[i].Cells[1].EditedFormattedValue.ToString().Trim();
                double productPrice = double.Parse(dataGridView1.Rows[i].Cells[4].EditedFormattedValue.ToString().Trim());
                double productMultiplier = GetValueFromMultiplierCell(dataGridView1.Rows[i].Cells[3].EditedFormattedValue.ToString().Trim());

                orderedItems.Add(new OrderedItem(
                    GetProductNameInLine(productName),
                    productUnit,
                    GetNoteInLine(productName),
                    double.Parse(dataGridView1.Rows[i].Cells[0].EditedFormattedValue.ToString()),
                    productMultiplier,
                    productPrice));
            }

            custOrder = orderedItems;
        }

        public List<OrderedItem> GetCustomerOrdered()
        {
            return custOrder;
        }

        private string GetNoteInLine(string input)
        {
            string[] tmp = input.Split(new char[] { '(', ')' });
            
            if (tmp.Length == 3)
                return tmp[1];
            
            return "";
        }

        private string GetProductNameInLine(string input)
        {
            string[] tmp = input.Split(new char[] { '(', ')' });

            return tmp[0].Trim();
        }

        private void OrderReviewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prompt user to save his data
            if (e.CloseReason == CloseReason.UserClosing)
            {
                UpdateCustomerOrdered();
            }
            else if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                // Autosave and clear up ressources
                UpdateCustomerOrdered();
            }
        }
    }
}
