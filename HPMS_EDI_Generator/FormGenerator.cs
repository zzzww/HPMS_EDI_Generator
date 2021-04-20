using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace HPMS_EDI_Generator
{
    public partial class FormGenerator : Form
    {
        private MySQLdb db = new MySQLdb();
        private bool ReportSelected = false;
        private int ExportVouchers = 0;
        private string ExportErrorMessage = "";
        private bool isAutoSubmission = false;
        private bool SubmissionResult = false;

        public FormGenerator()
        {
            InitializeComponent();
        }




        private void txtBatchNo_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = db.query("SELECT IFNULL(BATCH_NO, '') AS 'BATCH_NO', COUNT(VOUCHER_NO) AS 'TotalV', IFNULL(GROUP_CONCAT(distinct company_code SEPARATOR ', '), '') AS 'COMP', IFNULL(GROUP_CONCAT(distinct STATUS SEPARATOR ', '), '') AS 'STATUS' FROM voucher_line where BATCH_NO = '" + txtBatchNo.Text + "';");

            if (dt.Rows.Count > 0)
            {
                txtVoucherStatus.Text = dt.Rows[0]["STATUS"].ToString();
                txtTotalVouchers.Text = dt.Rows[0]["TotalV"].ToString();
                txtCompanyCode.Text = dt.Rows[0]["COMP"].ToString();
                if (dt.Rows[0]["COMP"].ToString().Equals("BLUEX"))
                {
                    lb_submit.Visible = true;
                    cmb_submit.Visible = true;
                    cmb_submit.SelectedIndex = 0;
                }
                else
                {
                    cmb_submit.SelectedIndex = 0;
                    lb_submit.Visible = false;
                    cmb_submit.Visible = false;
                }
            }
        }




        private void btnGenerator_Click(object sender, EventArgs e)
        {
            ReportSelected = false;
            isAutoSubmission = false;
            if (String.IsNullOrEmpty(txtCompanyCode.Text))
            {
                MessageBox.Show("Please provide the correct batch no.", "EDI Generator");
                return;
            }
            if (!txtVoucherStatus.Text.Equals("CONFIRMED"))
            {
                MessageBox.Show("Vouchers are waiting for verification.", "EDI Generator");
                return;
            }
            if (cmb_submit.SelectedIndex == 1)
            {
                var confirmResult = MessageBox.Show("Are you sure you want to upload the result to FTP?", "Are you sure?", MessageBoxButtons.YesNo); 
                if (confirmResult == DialogResult.No)
                {                    
                    return;
                }
                else
                {
                    isAutoSubmission = true;
                }
            }
            AuditLog.Log("User goes to export EDI. (Batch No:"+ txtBatchNo.Text+ ", Company: "+ txtCompanyCode.Text.Trim() + ")");
            if (cmb_submit.SelectedIndex == 1) AuditLog.Log("User requests for auto submission.");
            btnGenerator.Enabled = false;
            btnGenerator.Refresh();
            pb_loading.Visible = true;
            pb_loading.Refresh();
            txtBatchNo.ReadOnly = true;
            dpInvoiceDate.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            try
            {
                string strBatchNo = txtBatchNo.Text.Trim();
                string strCompany = txtCompanyCode.Text.Trim();
                switch (strCompany)
                {
                    case "AIA-MTRC":
                        ReportSelected = true;
                        AIA aia1 = new AIA(strBatchNo);
                        aia1.invoice_date = dpInvoiceDate.Text;
                        aia1.GenerateEDI();
                        ExportVouchers = aia1.TotalVouchers;
                        ExportErrorMessage = aia1.ErrorMessage;
                        break;
                    case "AIA-DISNEY":
                        ReportSelected = true;
                        AIA aia2 = new AIA(strBatchNo);
                        aia2.Med_Split = false;
                        aia2.invoice_date = dpInvoiceDate.Text;
                        aia2.GenerateEDI();
                        ExportVouchers = aia2.TotalVouchers;
                        ExportErrorMessage = aia2.ErrorMessage;
                        break;
                    case "BLUEX":
                        ReportSelected = true;
                        BlueX blue = new BlueX(strBatchNo);
                        blue.invoice_date = dpInvoiceDate.Text;
                        blue.isAutoSubmission = isAutoSubmission;
                        blue.GenerateEDI();
                        ExportVouchers = blue.TotalVouchers;
                        ExportErrorMessage = blue.ErrorMessage;
                        SubmissionResult = blue.GetSubmissionResult();
                        break;
                }
            }
            catch (Exception e0) { }
        }

        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            string message = "";
            txtBatchNo.ReadOnly = false;
            dpInvoiceDate.Enabled = true;
            pb_loading.Visible = false;
            btnGenerator.Enabled = true;
            if (ReportSelected)
            {
                if (String.IsNullOrEmpty(ExportErrorMessage))
                {
                    message = "EDI exported successfully.";
                    message += "\nTotal of exported Vouchers = " + ExportVouchers;
                    AuditLog.Log("UX: EDI exported successfully. Total of exported Vouchers = " + ExportVouchers);

                    if (cmb_submit.SelectedIndex == 1)
                    {
                        message += "\n\nFTP uploaded " + (SubmissionResult ? "successfully" : "failure") + ".";
                        AuditLog.Log("UX: FTP uploaded " + (SubmissionResult ? "successfully" : "failure") + ".");
                    }

                }
                else
                {
                    message = "EDI exported with error as below.\nTotal of exported Vouchers = " + ExportVouchers + "\n-------------------------------------\n" + ExportErrorMessage;
                    AuditLog.Log("UX: EDI exported with error as below. Total of exported Vouchers = " + ExportVouchers);
                    AuditLog.Log("UX: " + ExportErrorMessage.Replace("\n", ", "));

                    if (cmb_submit.SelectedIndex == 1)
                    {
                        message += "\nFTP uploaded is pending because of error.";
                        AuditLog.Log("UX: FTP uploaded is pending because of error.");
                    }
                }
            }
            else
            {
                message = "No EDI format provided for this company.";
            }
            MessageBox.Show(message, "EDI Generator", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
        }

        private void FormGenerator_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                backgroundWorker1.CancelAsync();
            }
            catch (Exception e1) { }
        }
    }
}
