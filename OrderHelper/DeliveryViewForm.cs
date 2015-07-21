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
    public partial class DeliveryViewForm : Form
    {
        public DeliveryViewForm(List<CustomerInfo> custInfo)
        {
            InitializeComponent();

            this.Text = "เดชาพาณิชย์ - ลำดับการจัดส่ง";

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.RowHeadersVisible = false;

            for (int i = 0; i < custInfo.Count; i++ )
                dataGridView1.Rows.Add(i + 1, custInfo[i].CustomerName, custInfo[i].PaymentType);
        }
    }
}
