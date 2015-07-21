namespace OrderHelper
{
    partial class IntimeNewProductForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOkay = new System.Windows.Forms.Button();
            this.labelItem = new System.Windows.Forms.Label();
            this.comboBoxStoredLocation = new System.Windows.Forms.ComboBox();
            this.comboBoxOperativeType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPrice = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("EucrosiaUPC", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(417, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "กรุณาระบุข้อมูลเพิ่มเติ่ม เกี่ยวกับสินค้า";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("EucrosiaUPC", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(56, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "สถานที่จัดเก็บ:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("EucrosiaUPC", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(61, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "กลุ่มพนักงาน:";
            // 
            // buttonOkay
            // 
            this.buttonOkay.Font = new System.Drawing.Font("EucrosiaUPC", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOkay.Location = new System.Drawing.Point(151, 243);
            this.buttonOkay.Name = "buttonOkay";
            this.buttonOkay.Size = new System.Drawing.Size(129, 37);
            this.buttonOkay.TabIndex = 3;
            this.buttonOkay.Text = "ตกลง";
            this.buttonOkay.UseVisualStyleBackColor = true;
            this.buttonOkay.Click += new System.EventHandler(this.buttonOkay_Click);
            // 
            // labelItem
            // 
            this.labelItem.Font = new System.Drawing.Font("EucrosiaUPC", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelItem.Location = new System.Drawing.Point(12, 47);
            this.labelItem.Name = "labelItem";
            this.labelItem.Size = new System.Drawing.Size(417, 29);
            this.labelItem.TabIndex = 4;
            this.labelItem.Text = "สินค้า";
            this.labelItem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxStoredLocation
            // 
            this.comboBoxStoredLocation.Font = new System.Drawing.Font("EucrosiaUPC", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxStoredLocation.FormattingEnabled = true;
            this.comboBoxStoredLocation.Location = new System.Drawing.Point(184, 191);
            this.comboBoxStoredLocation.Name = "comboBoxStoredLocation";
            this.comboBoxStoredLocation.Size = new System.Drawing.Size(146, 34);
            this.comboBoxStoredLocation.TabIndex = 5;
            // 
            // comboBoxOperativeType
            // 
            this.comboBoxOperativeType.Font = new System.Drawing.Font("EucrosiaUPC", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxOperativeType.FormattingEnabled = true;
            this.comboBoxOperativeType.Location = new System.Drawing.Point(184, 138);
            this.comboBoxOperativeType.Name = "comboBoxOperativeType";
            this.comboBoxOperativeType.Size = new System.Drawing.Size(146, 34);
            this.comboBoxOperativeType.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("EucrosiaUPC", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(117, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 29);
            this.label4.TabIndex = 7;
            this.label4.Text = "ราคา:";
            // 
            // textBoxPrice
            // 
            this.textBoxPrice.Font = new System.Drawing.Font("EucrosiaUPC", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPrice.Location = new System.Drawing.Point(184, 90);
            this.textBoxPrice.MaxLength = 9;
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.Size = new System.Drawing.Size(146, 33);
            this.textBoxPrice.TabIndex = 8;
            this.textBoxPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPrice_KeyPress);
            // 
            // IntimeNewProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 302);
            this.Controls.Add(this.textBoxPrice);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxOperativeType);
            this.Controls.Add(this.comboBoxStoredLocation);
            this.Controls.Add(this.labelItem);
            this.Controls.Add(this.buttonOkay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IntimeNewProductForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LocationAndOperativeInputForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOkay;
        private System.Windows.Forms.Label labelItem;
        private System.Windows.Forms.ComboBox comboBoxStoredLocation;
        private System.Windows.Forms.ComboBox comboBoxOperativeType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPrice;
    }
}