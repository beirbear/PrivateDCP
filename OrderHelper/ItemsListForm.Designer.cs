namespace OrderHelper
{
    partial class ItemsListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemsListForm));
            this.tabControlItems = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabPageFemale = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.tabPageAmr = new System.Windows.Forms.TabPage();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.tabControlItems.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPageFemale.SuspendLayout();
            this.tabPageAmr.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlItems
            // 
            this.tabControlItems.Controls.Add(this.tabPage1);
            this.tabControlItems.Controls.Add(this.tabPageFemale);
            this.tabControlItems.Controls.Add(this.tabPageAmr);
            this.tabControlItems.Font = new System.Drawing.Font("EucrosiaUPC", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlItems.Location = new System.Drawing.Point(12, 12);
            this.tabControlItems.Name = "tabControlItems";
            this.tabControlItems.SelectedIndex = 0;
            this.tabControlItems.Size = new System.Drawing.Size(660, 438);
            this.tabControlItems.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(652, 396);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "พนักงาน ชาย";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(646, 390);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // tabPageFemale
            // 
            this.tabPageFemale.Controls.Add(this.richTextBox2);
            this.tabPageFemale.Location = new System.Drawing.Point(4, 38);
            this.tabPageFemale.Name = "tabPageFemale";
            this.tabPageFemale.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFemale.Size = new System.Drawing.Size(652, 396);
            this.tabPageFemale.TabIndex = 1;
            this.tabPageFemale.Text = "พนักงาน หญิง";
            this.tabPageFemale.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox2.Location = new System.Drawing.Point(3, 3);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(646, 390);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "";
            // 
            // tabPageAmr
            // 
            this.tabPageAmr.Controls.Add(this.richTextBox3);
            this.tabPageAmr.Location = new System.Drawing.Point(4, 38);
            this.tabPageAmr.Name = "tabPageAmr";
            this.tabPageAmr.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAmr.Size = new System.Drawing.Size(652, 396);
            this.tabPageAmr.TabIndex = 2;
            this.tabPageAmr.Text = "อาม่า";
            this.tabPageAmr.UseVisualStyleBackColor = true;
            // 
            // richTextBox3
            // 
            this.richTextBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox3.Location = new System.Drawing.Point(3, 3);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(646, 390);
            this.richTextBox3.TabIndex = 0;
            this.richTextBox3.Text = "";
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopy.Font = new System.Drawing.Font("EucrosiaUPC", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCopy.Location = new System.Drawing.Point(593, 452);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(75, 32);
            this.buttonCopy.TabIndex = 1;
            this.buttonCopy.Text = "คัดลอก";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // ItemsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 496);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.tabControlItems);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 534);
            this.Name = "ItemsListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ItemsListForm";
            this.SizeChanged += new System.EventHandler(this.ItemsListForm_SizeChanged);
            this.tabControlItems.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPageFemale.ResumeLayout(false);
            this.tabPageAmr.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlItems;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPageFemale;
        private System.Windows.Forms.TabPage tabPageAmr;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Button buttonCopy;
    }
}