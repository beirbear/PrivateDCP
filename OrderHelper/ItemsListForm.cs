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
    public partial class ItemsListForm : Form
    {
        ////// VARIABLES FOR RESPONSIVE
        protected readonly int tbControlMinWidth;
        protected readonly int tbControlMinHeight;
        protected readonly int btnCopyMinYLoc;

        ////// INSTANCE FOR PROCESS
        protected ItemsPool dcpList;
        protected ItemsPool amrList;
        protected List<string> unitIndex;
        protected List<double> locIndex;
        protected string date;
        protected string route;

        // PRINTING REGION
        private PrintDocument printDoc;
        private PageSettings pgSettings;
        private PrinterSettings prtSetting;

        private int currentItem = 0;
        private int pageNumber = 1;

        // For internal use only
        private class ItemSet
        {
            public string itemName = "";
            public string itemUnit = "";
            public double totalAmount = 0;
            public string itemList = "";

            public ItemSet(string itemName, string itemUnit, double totalAmount, string itemList)
            {
                this.itemName = itemName;
                this.itemUnit = itemUnit;
                this.totalAmount = totalAmount;
                this.itemList = itemList;
            }
        }


        // Data Preparation List
        private List<ItemSet> dcpMale;
        private List<ItemSet> dcpFemale;
        private List<ItemSet> amrMale;

        public ItemsListForm(ItemsPool dcpList, ItemsPool amrList, string date, string route)
        {
            InitializeComponent();
            InitializePrinter();

            // Initilize List
            dcpMale = new List<ItemSet>();
            dcpFemale = new List<ItemSet>();
            amrMale = new List<ItemSet>();

            this.Text = "เดชาพาณิชย์ - รายการจัดของ";

            this.dcpList = dcpList;
            this.amrList = amrList;
            this.date = date;
            this.route = route;

            tbControlMinWidth = this.tabControlItems.Width;
            tbControlMinHeight = this.tabControlItems.Height;
            btnCopyMinYLoc = this.buttonCopy.Location.Y;

            ///// PROCESS ITEM FOR MALE DCP
            // 1. DETERMINE WHICH ITEMS ARE FOR MALE EMPLOYEES AND WHICH ARE FEMALE
            // 2. GROUP THEM WITH STORED LOCATION
            // 3. SORT ITEMS WHICH GROUP BY LOCATION BY UNIT WEIGHT PREFERENCE
            ///// FOR FEMALE ITEMS AND ARMAR
            // LOCATION GROUPING IS NO, JUST SORT
            
            ////// STEP 1
            unitIndex = Space.GetUnitIndex();
            locIndex = Space.GetLocationIndex();
            string message = string.Format("รายการของจัด {0} วันที่ {1}\r\nพนักงาน ชาย\r\n", route, date);
            message += string.Format("รวม\tหน่วย\tรายการ\tจำนวน\r\n");

            ///// LOOP FOR STORED LOCATION
            for (int j = 0; j < locIndex.Count; j++)
            {
                ///// LOOP FOR UNIT
                for (int i = 0; i < unitIndex.Count; i++)
                {
                    ////// GET ITEMS AND ORDER BY NAME
                    List<ItemIdentity> resSet = dcpList.GetItemsByUnitLocationAndOperative(unitIndex[i], locIndex[j], Space.OperativeType.Male);
                    for (int k = 0; k < resSet.Count; k++)
                    {
                        message += resSet[k].GetTotalAmount();
                        message += string.Format("\t{0}\t{1}{2}\t{3}\r\n", resSet[k].Unit, resSet[k].Name, (string.IsNullOrEmpty(resSet[k].Note) ? "" : " (" + resSet[k].Note + ")"), resSet[k].GetItemOrderInline());
                        dcpMale.Add(new ItemSet(resSet[k].Name, resSet[k].Unit, resSet[k].GetTotalAmount(), resSet[k].GetItemOrderInline()));
                    }
                }

                ////// GET ITEMS THAT HAS A SPECIAL UNIT
                List<ItemIdentity> otherSet = dcpList.GetItemsByUnitException(unitIndex, locIndex[j], Space.OperativeType.Male);
                for (int k = 0; k < otherSet.Count; k++)
                {
                    message += otherSet[k].GetTotalAmount();
                    message += string.Format("\t{0}\t{1}{2}\t{3}\r\n", otherSet[k].Unit, otherSet[k].Name, (string.IsNullOrEmpty(otherSet[k].Note) ? "" : " (" + otherSet[k].Note + ")"), otherSet[k].GetItemOrderInline());
                    dcpMale.Add(new ItemSet(otherSet[k].Name, otherSet[k].Unit, otherSet[k].GetTotalAmount(), otherSet[k].GetItemOrderInline()));   
                }
            }


            richTextBox1.Text = message;
            message = string.Format("รายการของจัด {0} วันที่ {1}\r\nพนักงาน หญิง\r\n", route, date);
            message += string.Format("รวม\tหน่วย\tรายการ\tจำนวน\r\n");

            
            ///// LOOP FOR STORED LOCATION
            for (int j = 0; j < locIndex.Count; j++)
            {
                ///// LOOP FOR UNIT
                for (int i = 0; i < unitIndex.Count; i++)
                {
                    ////// GET ITEMS AND ORDER BY NAME
                    List<ItemIdentity> resSet = dcpList.GetItemsByUnitLocationAndOperative(unitIndex[i], locIndex[j], Space.OperativeType.Female);
                    for (int k = 0; k < resSet.Count; k++)
                    {
                        message += resSet[k].GetTotalAmount();
                        message += string.Format("\t{0}\t{1}{2}\t{3}\r\n", resSet[k].Unit, resSet[k].Name, (string.IsNullOrEmpty(resSet[k].Note) ? "" : " (" + resSet[k].Note + ")"), resSet[k].GetItemOrderInline());
                        dcpFemale.Add(new ItemSet(resSet[k].Name, resSet[k].Unit, resSet[k].GetTotalAmount(), resSet[k].GetItemOrderInline()));
                    }
                }

                List<ItemIdentity> otherSet = dcpList.GetItemsByUnitException(unitIndex, locIndex[j], Space.OperativeType.Female);
                for (int k = 0; k < otherSet.Count; k++)
                {
                    message += otherSet[k].GetTotalAmount();
                    message += string.Format("\t{0}\t{1}{2}\t{3}\r\n", otherSet[k].Unit, otherSet[k].Name, (string.IsNullOrEmpty(otherSet[k].Note) ? "" : " (" + otherSet[k].Note + ")"), otherSet[k].GetItemOrderInline());
                    dcpFemale.Add(new ItemSet(otherSet[k].Name, otherSet[k].Unit, otherSet[k].GetTotalAmount(), otherSet[k].GetItemOrderInline())); 
                }
            }
            richTextBox2.Text = message;

            ///// AMR
            message = string.Format("รายการของจัด {0} วันที่ {1}\r\nพนักงาน อาม่า\r\n", route, date);
            message += string.Format("รวม\tหน่วย\tรายการ\tจำนวน\r\n");

            List<ItemIdentity> amrItem = amrList.GetItemsOrderByUnitAndName();
            for (int i = 0; i < amrItem.Count; i++)
            {
                message += amrItem[i].GetTotalAmount();
                message += string.Format("\t{0}\t{1}{2}\t{3}\r\n", amrItem[i].Unit, amrItem[i].Name, (string.IsNullOrEmpty(amrItem[i].Note) ? "" : " (" + amrItem[i].Note + ")"), amrItem[i].GetItemOrderInline());
                amrMale.Add(new ItemSet(amrItem[i].Name, amrItem[i].Unit, amrItem[i].GetTotalAmount(), amrItem[i].GetItemOrderInline()));
            }
            richTextBox3.Text = message;
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            // Print document   
            printDoc.DefaultPageSettings = pgSettings;
            PrintDialog dlg = new PrintDialog();
            dlg.Document = printDoc;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                currentItem = 0;
                printDoc.Print();
            }
        }

        private void ItemsListForm_SizeChanged(object sender, EventArgs e)
        {
            int diffWidth = this.Size.Width - this.MinimumSize.Width;
            int diffHeight = this.Size.Height - this.MinimumSize.Height;

            tabControlItems.Size = new Size(tbControlMinWidth + diffWidth, tbControlMinHeight + diffHeight);

            buttonCopy.Location = new Point(buttonCopy.Location.X , btnCopyMinYLoc + diffHeight);
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
            int pageCenter = e.MarginBounds.Size.Width / 2 + e.MarginBounds.Left;

            // DCP M
            SizeF stringSize = new SizeF();
            string printingMessage = string.Format("{0} วันที่ {1}", route, date);
            // Print route title
            stringSize = e.Graphics.MeasureString(printingMessage, printFont);
            e.Graphics.DrawString(printingMessage, printFont, drawBrush, pageCenter - stringSize.Width / 2, linePosition);
            linePosition += lineHeight;

            // Male Tab
            if (tabControlItems.SelectedIndex == 0)
            {
                // Print list (25)
                for (int j = 0; currentItem < dcpMale.Count && j < 25; currentItem++, j++)
                {
                    // Print underscore
                    printingMessage = dcpMale[currentItem].totalAmount.ToString();
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left, linePosition);

                    // Print customer name
                    printingMessage = string.Format("{0} ({1})", dcpMale[currentItem].itemName, dcpMale[currentItem].itemUnit);
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col2Position, linePosition);

                    // Print amount martker
                    printingMessage = dcpMale[currentItem].itemList.Trim().Replace('\t',',');
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col3Position, linePosition);

                    linePosition += lineHeight;
                }

                printingMessage = pageNumber.ToString();
                stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Right, e.MarginBounds.Top);


                if (currentItem < dcpMale.Count)
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
            else if (tabControlItems.SelectedIndex == 1)
            {
                // Female
                for (int j = 0; currentItem < dcpFemale.Count && j < 25; currentItem++, j++)
                {
                    // Print underscore
                    printingMessage = dcpFemale[currentItem].totalAmount.ToString();
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left, linePosition);

                    // Print customer name
                    printingMessage = string.Format("{0} ({1})", dcpFemale[currentItem].itemName, dcpFemale[currentItem].itemUnit);
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col2Position, linePosition);

                    // Print amount martker
                    printingMessage = dcpFemale[currentItem].itemList.Trim().Replace('\t', ',');
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col3Position, linePosition);

                    linePosition += lineHeight;
                }

                printingMessage = pageNumber.ToString();
                stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Right, e.MarginBounds.Top);


                if (currentItem < dcpFemale.Count)
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
            else if (tabControlItems.SelectedIndex == 2)
            {
                // Amr
                // Female
                for (int j = 0; currentItem < amrMale.Count && j < 25; currentItem++, j++)
                {
                    // Print underscore
                    printingMessage = amrMale[currentItem].totalAmount.ToString();
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left, linePosition);

                    // Print customer name
                    printingMessage = string.Format("{0} ({1})", amrMale[currentItem].itemName, amrMale[currentItem].itemUnit);
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col2Position, linePosition);

                    // Print amount martker
                    printingMessage = amrMale[currentItem].itemList.Trim().Replace('\t', ',');
                    stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                    e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Left + col3Position, linePosition);

                    linePosition += lineHeight;
                }

                printingMessage = pageNumber.ToString();
                stringSize = e.Graphics.MeasureString(printingMessage, printFont);
                e.Graphics.DrawString(printingMessage, printFont, drawBrush, e.MarginBounds.Right, e.MarginBounds.Top);


                if (currentItem < amrMale.Count)
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
    }
}
