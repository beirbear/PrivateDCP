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

        public ItemsListForm(ItemsPool dcpList, ItemsPool amrList, string date, string route)
        {
            InitializeComponent();

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
                    }
                }

                ////// GET ITEMS THAT HAS A SPECIAL UNIT
                List<ItemIdentity> otherSet = dcpList.GetItemsByUnitException(unitIndex, locIndex[j], Space.OperativeType.Male);
                for (int k = 0; k < otherSet.Count; k++)
                {
                    message += otherSet[k].GetTotalAmount();
                    message += string.Format("\t{0}\t{1}{2}\t{3}\r\n", otherSet[k].Unit, otherSet[k].Name, (string.IsNullOrEmpty(otherSet[k].Note) ? "" : " (" + otherSet[k].Note + ")"), otherSet[k].GetItemOrderInline());
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
                    }
                }

                List<ItemIdentity> otherSet = dcpList.GetItemsByUnitException(unitIndex, locIndex[j], Space.OperativeType.Female);
                for (int k = 0; k < otherSet.Count; k++)
                {
                    message += otherSet[k].GetTotalAmount();
                    message += string.Format("\t{0}\t{1}{2}\t{3}\r\n", otherSet[k].Unit, otherSet[k].Name, (string.IsNullOrEmpty(otherSet[k].Note) ? "" : " (" + otherSet[k].Note + ")"), otherSet[k].GetItemOrderInline());
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
            }
            richTextBox3.Text = message;
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if(tabControlItems.SelectedIndex == 0)
                Clipboard.SetText(richTextBox1.Text);
            else if (tabControlItems.SelectedIndex == 1)
                Clipboard.SetText(richTextBox2.Text);
            else if (tabControlItems.SelectedIndex == 2)
                Clipboard.SetText(richTextBox3.Text);
        }

        private void ItemsListForm_SizeChanged(object sender, EventArgs e)
        {
            int diffWidth = this.Size.Width - this.MinimumSize.Width;
            int diffHeight = this.Size.Height - this.MinimumSize.Height;

            tabControlItems.Size = new Size(tbControlMinWidth + diffWidth, tbControlMinHeight + diffHeight);

            buttonCopy.Location = new Point(buttonCopy.Location.X , btnCopyMinYLoc + diffHeight);
        }
    }
}
