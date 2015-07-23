using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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

        // PRINTING REGION
        private PrintDocument printDoc;
        private PageSettings pgSettings;
        private PrinterSettings prtSetting;

        // holding tempolary value
        private List<string> dcpCustName;
        private List<double> dcpCustAmount;
        
        private List<string> amrCustName;
        private List<double> amrCustAmount;
        private int currentItem = 0;
        private int pageNumber = 1;
        private bool printDcp = true;

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

            // Tempolary variable initialization
            dcpCustName = new List<string>();
            dcpCustAmount = new List<double>();
            amrCustName = new List<string>();
            amrCustAmount = new List<double>();

            DisplayAllBills();
            InitializePrinter();
        }

        private void InitializePrinter()
        {
            // Initilization
            printDoc = new PrintDocument();
            pgSettings = new PageSettings();
            prtSetting = new PrinterSettings();

            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

            // Page Setup
            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.PageSettings = pgSettings;
            pageSetupDialog.PrinterSettings = prtSetting;

            PaperSize paperSize = new PaperSize();
            paperSize.RawKind = (int)PaperKind.Letter;

            printDoc.DefaultPageSettings.PaperSize = paperSize;
            printDoc.DefaultPageSettings.Landscape = false;
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font printFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);
            StringFormat stringFmt = new StringFormat();
            stringFmt.Alignment = StringAlignment.Center;

            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;

            // Define stringSize to get the size of string
            int linePosition = e.MarginBounds.Top;
            const int lineHeight = 35;
            const int col2Position = 75;
            const int col3Position = 375;
            const int col4Position = 500;
            int pageCenter = e.MarginBounds.Size.Width / 2 + e.MarginBounds.Left;

            // DCP
            if (printDcp)
            {
                SizeF stringSize = new SizeF();
                string printingMessage = routeName;
                // Print route title
                stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                e.Graphics.DrawString(printingMessage, printFont, drawBrush, pageCenter - stringSize.Width / 2, linePosition);
                linePosition += lineHeight;

                // Print order making date
                printingMessage = "รอบวันที่ " + date + " ( หน้า " + pageNumber + "/" + (dcpCustName.Count / 22 + 1) + ")";
                stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                e.Graphics.DrawString(printingMessage, printFont, drawBrush, pageCenter - stringSize.Width / 2, linePosition);
                linePosition += lineHeight;

                // Print printing date
                printingMessage = "พิมพ์เมื่อวันที่ " + string.Format("{0}/{1}/{2}", System.DateTime.Today.Day, System.DateTime.Today.Month, System.DateTime.Today.Year + 543);
                stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                e.Graphics.DrawString(printingMessage, printFont, drawBrush, pageCenter - stringSize.Width / 2, linePosition);
                linePosition += lineHeight + 10;

                // MessageBox.Show(string.Format("{0}/{1}", currentItem, dcpCustName.Count));

                // Print list (22)
                for (int j = 0; currentItem < dcpCustName.Count && j < 22; currentItem++, j++)
                {
                    // Print underscore
                    printingMessage = "_____";
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left, linePosition);

                    // Print customer name
                    printingMessage = dcpCustName[currentItem];
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col2Position, linePosition);

                    // Print amount martker
                    printingMessage = "(              )";
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col3Position, linePosition);

                    // Print total cost1
                    printingMessage = dcpCustAmount[currentItem].ToString("#,##0.##");
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col4Position, linePosition);

                    linePosition += lineHeight;
                }

                if (currentItem < dcpCustName.Count)
                {
                    e.HasMorePages = true;
                    pageNumber++;
                }
                else
                {
                    e.HasMorePages = true;
                    pageNumber = 1;
                    currentItem = 0;
                    printDcp = false;
                }
            }
            else
            {
                // AMR
                SizeF stringSize = new SizeF();
                string printingMessage = routeName + " (อาม่า)";
                // Print route title
                stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                e.Graphics.DrawString(printingMessage, printFont, drawBrush, pageCenter - stringSize.Width / 2, linePosition);
                linePosition += lineHeight;

                // Print order making date
                printingMessage = "รอบวันที่ " + date + " ( หน้า " + pageNumber + "/" + (amrCustName.Count / 22 + 1) + ")";
                stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                e.Graphics.DrawString(printingMessage, printFont, drawBrush, pageCenter - stringSize.Width / 2, linePosition);
                linePosition += lineHeight;

                // Print printing date
                printingMessage = "พิมพ์เมื่อวันที่ " + string.Format("{0}/{1}/{2}", System.DateTime.Today.Day, System.DateTime.Today.Month, System.DateTime.Today.Year + 543);
                stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                e.Graphics.DrawString(printingMessage, printFont, drawBrush, pageCenter - stringSize.Width / 2, linePosition);
                linePosition += lineHeight + 10;

                // MessageBox.Show(string.Format("{0}/{1}", currentItem, amrCustName.Count));

                // Print list (22)
                for (int j = 0; currentItem < amrCustName.Count && j < 22; currentItem++, j++)
                {
                    // Print underscore
                    printingMessage = "_____";
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left, linePosition);

                    // Print customer name
                    printingMessage = amrCustName[currentItem] + " (อาม่า)";
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col2Position, linePosition);

                    // Print amount martker
                    printingMessage = "(              )";
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col3Position, linePosition);

                    // Print total cost1
                    printingMessage = amrCustAmount[currentItem].ToString("#,##0.##");
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col4Position, linePosition);

                    linePosition += lineHeight;
                }

                if (currentItem < amrCustName.Count)
                {
                    e.HasMorePages = true;
                    pageNumber++;
                }
                else
                {
                    e.HasMorePages = false;
                    pageNumber = 1;
                }
            }
        }

        private void DisplayAllBills()
        {
            rtf1.Clear();

            List<CustomerInfo> custInfo = session.GetCustomersList().OrderBy(ex => ex.Order).ToList<CustomerInfo>();

            rtf1.Text = routeName + "\r\nวันที่ " + date + "\r\n" + "พิมพ์เมื่อวันที่ " + string.Format("{0} เดือนที่ {1} ปี พ.ศ.{2}", System.DateTime.Today.Day, System.DateTime.Today.Month, System.DateTime.Today.Year + 543) + "\r\n";
            
            string dcpOrdered = "";
            string amrOrdered = "";

            foreach (CustomerInfo cust in custInfo)
            {
                if (session.CustomerHasSkip(cust.CustomerName))
                    continue;

                if (session.CustomerHasOrdered(cust.CustomerName))
                {
                    List<OrderedItem> custOrder = session.GetCustomerOrder(cust);
                    if (custOrder.Count > 0)
                    {
                        double total = 0;
                        foreach (OrderedItem item in custOrder)
                        {
                            double inTotal = item.Price * item.Amount * item.Multiplier;
                            total += inTotal;
                        }
                        dcpOrdered += string.Format("{0}\t{1}\r\n", cust.CustomerName, total.ToString("#,##0.##"));
                        dcpCustName.Add(cust.CustomerName);
                        dcpCustAmount.Add(total);
                    }

                    custOrder = session.GetCustomerOrderAmr(cust);
                    if (custOrder.Count > 0)
                    {
                        double total = 0;
                        foreach (OrderedItem item in custOrder)
                        {
                            double inTotal = item.Price * item.Amount * item.Multiplier;
                            total += inTotal;
                        }
                        amrOrdered += string.Format("{0}\t{1}\r\n", cust.CustomerName, total.ToString("#,##0.##"));
                        amrCustName.Add(cust.CustomerName);
                        amrCustAmount.Add(total);
                    }
                }
            }

            rtf1.Text += "เรา\r\n" + dcpOrdered;
            rtf1.Text += "อาม่า\r\n" + amrOrdered;
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
            // Clipboard.SetText(rtf1.Text);

            printDoc.DefaultPageSettings = pgSettings;
            PrintDialog dlg = new PrintDialog();
            dlg.Document = printDoc;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
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
