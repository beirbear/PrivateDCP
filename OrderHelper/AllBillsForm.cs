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
    public partial class AllBillsForm : Form
    {
        // RESPONSIVE DATE AREA
        private readonly int rtfMinWidth;
        private readonly int rtfMinHright;
        private readonly int btnMinY;

        // DATE PROCESSING REGION
        private readonly Session session;
        private readonly string routeName;
        private readonly string date;

        public AllBillsForm(ref Session session, string routeName, string date)
        {
            InitializeComponent();

            this.session = session;
            this.routeName = routeName;
            this.date = date;

            rtfMinWidth = rtf1.Width;
            rtfMinHright = rtf1.Height;
            btnMinY = buttonRecalculate.Location.Y;

            this.Text = "เดชาพาณิชย์ - บิลทั้งหมด";

            DisplayAllBills();
        }

        private void DisplayAllBills()
        {
            rtf1.Clear();

            List<CustomerInfo> custInfo = session.GetCustomersList().OrderBy(ex => ex.Order).ToList<CustomerInfo>();

            rtf1.Text = routeName + "\r\nวันที่ " + date + "\r\n";

            foreach (CustomerInfo cust in custInfo)
            {
                if (session.CustomerHasSkip(cust.CustomerName))
                    continue;

                if (session.CustomerHasOrdered(cust.CustomerName))
                {
                    // Append delimit
                    rtf1.Text += "--------------------------------------------------------\r\n";

                    List<OrderedItem> custOrder = session.GetCustomerOrder(cust);
                    if (custOrder.Count > 0)
                    {
                        rtf1.Text += string.Format("ชื่อ:\t{0}\r\n", cust.CustomerName);
                        double total = 0;
                        foreach (OrderedItem item in custOrder)
                        {
                            double inTotal = item.Price * item.Amount * item.Multiplier;
                            rtf1.Text += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\r\n", item.Amount, item.Unit, GetNameInline(item.Name, item.Note), item.Price, inTotal.ToString("#,##0.##"));
                            total += inTotal;
                        }
                        rtf1.Text += "รวม\t\t\t\t" + total.ToString("#,##0.##") + "\r\n";
                    }

                    custOrder = session.GetCustomerOrderAmr(cust);
                    if (custOrder.Count > 0)
                    {
                        rtf1.Text += string.Format("ชื่อ:\t{0} (อาม่า)\r\n", cust.CustomerName);
                        double total = 0;
                        foreach (OrderedItem item in custOrder)
                        {
                            double inTotal = item.Price * item.Amount * item.Multiplier;
                            rtf1.Text += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\r\n", item.Amount, item.Unit, GetNameInline(item.Name, item.Note), item.Price, inTotal.ToString("#,##0.##"));
                            total += inTotal;
                        }
                        rtf1.Text += "รวม\t\t\t\t" + total.ToString("#,##0.##") + "\r\n";
                    }
                }
            }
        }

        private string GetNameInline(string productName, string productNote)
        {
            if (productNote.Trim().Length == 0)
                return productName;

            return string.Format("{0} ({1})", productName, productNote);
        }

        private void buttonRecalculate_Click(object sender, EventArgs e)
        {
            DisplayAllBills();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtf1.Text);
        }

        private void AllBillsForm_SizeChanged(object sender, EventArgs e)
        {
            // Enable Responsive
            int diffHeight = this.Height - this.MinimumSize.Height;
            int diffWidth = this.Width - this.MinimumSize.Width;

            rtf1.Size = new Size(rtfMinWidth + diffWidth, rtfMinHright + diffHeight);
            button1.Location = new Point(button1.Location.X, btnMinY + diffHeight);
            buttonRecalculate.Location = new Point(buttonRecalculate.Location.X, btnMinY + diffHeight);
        }
    }
}
