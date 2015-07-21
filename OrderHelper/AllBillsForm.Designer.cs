namespace OrderHelper
{
    partial class AllBillsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllBillsForm));
            this.rtf1 = new System.Windows.Forms.RichTextBox();
            this.buttonRecalculate = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtf1
            // 
            this.rtf1.Font = new System.Drawing.Font("EucrosiaUPC", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtf1.Location = new System.Drawing.Point(12, 12);
            this.rtf1.Name = "rtf1";
            this.rtf1.ReadOnly = true;
            this.rtf1.Size = new System.Drawing.Size(660, 497);
            this.rtf1.TabIndex = 0;
            this.rtf1.Text = "";
            // 
            // buttonRecalculate
            // 
            this.buttonRecalculate.Image = ((System.Drawing.Image)(resources.GetObject("buttonRecalculate.Image")));
            this.buttonRecalculate.Location = new System.Drawing.Point(12, 515);
            this.buttonRecalculate.Name = "buttonRecalculate";
            this.buttonRecalculate.Size = new System.Drawing.Size(35, 35);
            this.buttonRecalculate.TabIndex = 12;
            this.buttonRecalculate.UseVisualStyleBackColor = true;
            this.buttonRecalculate.Click += new System.EventHandler(this.buttonRecalculate_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("EucrosiaUPC", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(587, 515);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 35);
            this.button1.TabIndex = 13;
            this.button1.Text = "คัดลอก";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AllBillsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 562);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonRecalculate);
            this.Controls.Add(this.rtf1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 600);
            this.Name = "AllBillsForm";
            this.Text = "AllBillsForm";
            this.SizeChanged += new System.EventHandler(this.AllBillsForm_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.RichTextBox rtf1;
        private System.Windows.Forms.Button buttonRecalculate;
        private System.Windows.Forms.Button button1;
    }
}