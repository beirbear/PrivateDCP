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
    public partial class PasswordForm : Form
    {
        public PasswordForm(string note)
        {
            InitializeComponent();
            label2.Text =   "ข้อมูลที่ถูกทำการเปลี่ยนแปลง นั้นจะมีผลกับระบบในการใช้งานรอบนี้\r\n"
                            + "และรอบถัดๆไป (กรุณาแก้ไขด้วยความระมัดระวัง)\r\n"
                            + note + "\r\n"
                            + "กรุณาใส่รหัสเพื่อยืนยันการรับรู้ถึงผลของการเปลี่ยนแปลง";
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Verify())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private bool Verify()
        {
            string pwd = maskedTxt.Text.Trim();

            if (pwd.Length == 0)
            {
                MessageBox.Show("กรุณาป้อนรหัสผ่าน", "");
                return false;
            }

            if (pwd == "เดชา")
                return true;

            MessageBox.Show("รหัสผ่านไม่ถูกต้อง", "");
            maskedTxt.Text = "";
            return false;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void maskedTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (Verify())
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
        }
    }
}
