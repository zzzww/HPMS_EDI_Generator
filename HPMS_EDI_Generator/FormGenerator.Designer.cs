namespace HPMS_EDI_Generator
{
    partial class FormGenerator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGenerator));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.btnGenerator = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTotalVouchers = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCompanyCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pb_loading = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.txtVoucherStatus = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dpInvoiceDate = new HPMS_EDI_Generator.CenteredDateTimePicker();
            this.lb_submit = new System.Windows.Forms.Label();
            this.cmb_submit = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_loading)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(150, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(305, 156);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(137, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "EDI Generator for insurer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(107, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Batch No : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(107, 397);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "Invoice Date :";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBatchNo.Location = new System.Drawing.Point(292, 223);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(208, 27);
            this.txtBatchNo.TabIndex = 4;
            this.txtBatchNo.TextChanged += new System.EventHandler(this.txtBatchNo_TextChanged);
            // 
            // btnGenerator
            // 
            this.btnGenerator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerator.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerator.Location = new System.Drawing.Point(102, 483);
            this.btnGenerator.Name = "btnGenerator";
            this.btnGenerator.Size = new System.Drawing.Size(401, 45);
            this.btnGenerator.TabIndex = 6;
            this.btnGenerator.Text = "Generate";
            this.btnGenerator.UseVisualStyleBackColor = true;
            this.btnGenerator.Click += new System.EventHandler(this.btnGenerator_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(107, 352);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Total of Vouchers :";
            // 
            // txtTotalVouchers
            // 
            this.txtTotalVouchers.BackColor = System.Drawing.SystemColors.Window;
            this.txtTotalVouchers.Enabled = false;
            this.txtTotalVouchers.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalVouchers.Location = new System.Drawing.Point(292, 348);
            this.txtTotalVouchers.Name = "txtTotalVouchers";
            this.txtTotalVouchers.Size = new System.Drawing.Size(208, 27);
            this.txtTotalVouchers.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(179, 546);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(248, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "© 2021 HKRI GIT developed. All Rights Reserved.";
            // 
            // txtCompanyCode
            // 
            this.txtCompanyCode.BackColor = System.Drawing.SystemColors.Window;
            this.txtCompanyCode.Enabled = false;
            this.txtCompanyCode.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCompanyCode.Location = new System.Drawing.Point(292, 264);
            this.txtCompanyCode.Name = "txtCompanyCode";
            this.txtCompanyCode.Size = new System.Drawing.Size(208, 27);
            this.txtCompanyCode.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(107, 267);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "Company Code :";
            // 
            // pb_loading
            // 
            this.pb_loading.BackColor = System.Drawing.Color.Transparent;
            this.pb_loading.Image = global::HPMS_EDI_Generator.Properties.Resources.loader;
            this.pb_loading.Location = new System.Drawing.Point(192, 251);
            this.pb_loading.Name = "pb_loading";
            this.pb_loading.Size = new System.Drawing.Size(194, 188);
            this.pb_loading.TabIndex = 12;
            this.pb_loading.TabStop = false;
            this.pb_loading.Visible = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork_1);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted_1);
            // 
            // txtVoucherStatus
            // 
            this.txtVoucherStatus.BackColor = System.Drawing.SystemColors.Window;
            this.txtVoucherStatus.Enabled = false;
            this.txtVoucherStatus.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVoucherStatus.Location = new System.Drawing.Point(292, 305);
            this.txtVoucherStatus.Name = "txtVoucherStatus";
            this.txtVoucherStatus.Size = new System.Drawing.Size(208, 27);
            this.txtVoucherStatus.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(107, 309);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 18);
            this.label7.TabIndex = 13;
            this.label7.Text = "Voucher Status :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(542, 1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Version 2.0";
            // 
            // dpInvoiceDate
            // 
            this.dpInvoiceDate.AllowDrop = true;
            this.dpInvoiceDate.CustomFormat = "yyyy/MM/dd";
            this.dpInvoiceDate.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpInvoiceDate.Location = new System.Drawing.Point(292, 392);
            this.dpInvoiceDate.Name = "dpInvoiceDate";
            this.dpInvoiceDate.Size = new System.Drawing.Size(208, 27);
            this.dpInvoiceDate.TabIndex = 5;
            // 
            // lb_submit
            // 
            this.lb_submit.AutoSize = true;
            this.lb_submit.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_submit.Location = new System.Drawing.Point(107, 442);
            this.lb_submit.Name = "lb_submit";
            this.lb_submit.Size = new System.Drawing.Size(158, 18);
            this.lb_submit.TabIndex = 17;
            this.lb_submit.Text = "Auto Submission :";
            this.lb_submit.Visible = false;
            // 
            // cmb_submit
            // 
            this.cmb_submit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_submit.Font = new System.Drawing.Font("Verdana", 12F);
            this.cmb_submit.FormattingEnabled = true;
            this.cmb_submit.Items.AddRange(new object[] {
            "None",
            "FTP"});
            this.cmb_submit.Location = new System.Drawing.Point(292, 437);
            this.cmb_submit.Name = "cmb_submit";
            this.cmb_submit.Size = new System.Drawing.Size(208, 26);
            this.cmb_submit.TabIndex = 18;
            this.cmb_submit.Visible = false;
            // 
            // FormGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 565);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pb_loading);
            this.Controls.Add(this.txtVoucherStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtCompanyCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTotalVouchers);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnGenerator);
            this.Controls.Add(this.dpInvoiceDate);
            this.Controls.Add(this.txtBatchNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cmb_submit);
            this.Controls.Add(this.lb_submit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EDI Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGenerator_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_loading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.Button btnGenerator;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTotalVouchers;
        private System.Windows.Forms.Label label5;
        private CenteredDateTimePicker dpInvoiceDate;
        private System.Windows.Forms.TextBox txtCompanyCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pb_loading;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox txtVoucherStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lb_submit;
        private System.Windows.Forms.ComboBox cmb_submit;
    }
}

