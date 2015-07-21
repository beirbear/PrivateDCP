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
    public partial class ViewOrderedItemForm : Form
    {
        // RESPONSIVE DESIGN
        private readonly int cbMinWidth;
        private readonly int dgvMinWidth;
        private readonly int dgvMinHeight;
        private readonly int btnYLoc;

        // PROCESSING INSTANCE
        private Session session;
        private string clipBoardText;

        public ViewOrderedItemForm(ref Session session)
        {
            InitializeComponent();

            this.Text = "เดชาพาณิชย์ - รายการสินค้าเฉพาะ";
            this.session = session;

            cbMinWidth = comboBox1.Width;
            dgvMinHeight = dataGridView1.Height;
            dgvMinWidth = dataGridView1.Width;
            btnYLoc = btnCopy.Location.Y;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.RowHeadersVisible = false;

            // INITIALIZE ITEM
            List<string> uniqueNameList = session.GetEveryOrderedItemName();

            foreach (string name in uniqueNameList)
                comboBox1.Items.Add(name);

            if(comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            Dictionary<string, double> tmp = session.GetSpecificOrderedItem(comboBox1.SelectedItem.ToString());

            clipBoardText = "";
            foreach (KeyValuePair<string, double> pair in tmp)
            {
                dataGridView1.Rows.Add(pair.Key, pair.Value.ToString());
                clipBoardText += string.Format("{0}\t{1}\r\n", pair.Key, pair.Value);
            }
        }

        private void ViewOrderedItemForm_SizeChanged(object sender, EventArgs e)
        {
            int diffWidth = this.Width - this.MinimumSize.Width;
            int diffHeight = this.Height - this.MinimumSize.Height;

            comboBox1.Size = new Size(cbMinWidth + diffWidth, comboBox1.Size.Height);
            dataGridView1.Size = new Size(dgvMinWidth + diffWidth, dgvMinHeight + diffHeight);
            btnCopy.Location = new Point(btnCopy.Location.X, btnYLoc + diffHeight);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(clipBoardText);
        }
    }
}
