using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;

namespace HPMS_EDI_Generator
{
    public class AIA
    {
        private const string AIA_GP = "99201";
        private const string AIA_SP = "99202";
        private const string AIA_PHY = "97111";
        private const string AIA_CU = "97113S";
        private const string AIA_GP_MED = "99916";
        private const string AIA_GP_MED_S = "99917A";
        private const string AIA_GP_MED_L = "99917D";
        private const string AIA_SP_MED = "99917";
        private const string AIA_SP_MED_S = "99917B";
        private const string AIA_SP_MED_L = "99917C";
        private const string AIA_GP_LAB = "LAB";
        private const string AIA_SP_LAB = "LAB";
        private const string AIA_GP_MOP = "MOP";
        private const string AIA_SP_MOP = "MOP";

        private MySQLdb db = new MySQLdb();
        public bool Med_Split = true;
        public int TotalVouchers = 0;
        public string ErrorMessage = "";
        private string Batch_No = "";
        public string invoice_date = "";
        private Application ExcelApp = null;
        private Workbook ExcelWorkBook = null;
        private string SQL = "SELECT v.*, d.TERM_DATE AS DR_TERM_DATE, i.INVOICE_NO FROM voucher_line v INNER JOIN doctor d ON v.DR_CODE=d.DR_CODE LEFT JOIN invoice_line i ON v.BATCH_NO=i.BATCH_NO ";

        public AIA(string _batch_no)
        {
            Batch_No = _batch_no;
            TotalVouchers = 0;            
            ErrorMessage = "";
        }


        public void GenerateEDI()
        {
            ExcelApp = new Application();   
            ExcelWorkBook = ExcelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

            try
            {
                CreateSheet_GP();
                CreateSheet_GP_MED();
                CreateSheet_GP_LAB();
                CreateSheet_GP_MOP();
                CreateSheet_GP_MED_LAB();
                CreateSheet_GP_MED_MOP();
                CreateSheet_GP_LAB_MOP();
                CreateSheet_GP_MED_LAB_MOP();
                CreateSheet_SP();
                CreateSheet_SP_MED();
                CreateSheet_SP_LAB();
                CreateSheet_SP_MOP();
                CreateSheet_SP_MED_LAB();
                CreateSheet_SP_MED_MOP();
                CreateSheet_SP_LAB_MOP();
                CreateSheet_SP_MED_LAB_MOP();
                CreateSheet_LAB();
                CreateSheet_PHY();
                ExcelWorkBook.Worksheets[1].Delete();
                Marshal.ReleaseComObject(ExcelWorkBook);
                ExcelApp.Visible = true;
                Marshal.ReleaseComObject(ExcelApp);

                AuditLog.Log("AIA File is exported.");
            }
            catch (Exception exHandle)
            {
                ErrorMessage += "Main:" + exHandle.Message + "\n";

                AuditLog.Log("Exception Line 82 " + exHandle.Message);                
            }
            finally
            {
            }

        }


        private void CreateSheet_GP()
        {
            string condition = " WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'GP' AND `EXTRA_MED` = 0 AND `LAB_XRAY` = 0 AND `SURGICAL` = 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "GP";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "GP");
                             
                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_GP_MED()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'GP' AND `EXTRA_MED` > 0 AND `LAB_XRAY` = 0 AND `SURGICAL` = 0  ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "GP + MED";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "GP");
                                
                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_GP_LAB()
        {
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'GP' AND `EXTRA_MED` = 0 AND `LAB_XRAY` > 0 AND `SURGICAL` = 0 AND `LAB_XRAY` <> `FEE_AMT` ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "GP + LAB";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "GP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_GP_MOP()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'GP' AND `EXTRA_MED` = 0 AND `LAB_XRAY` = 0 AND `SURGICAL` > 0  ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "GP + MOP";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "GP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_GP_MED_LAB()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'GP' AND `EXTRA_MED` > 0 AND `LAB_XRAY` > 0 AND `SURGICAL` = 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "GP + MED + LAB";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "GP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_GP_MED_MOP()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'GP' AND `EXTRA_MED` > 0 AND `LAB_XRAY` = 0 AND `SURGICAL` > 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "GP + MED + MOP";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "GP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_GP_LAB_MOP()
        {
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'GP' AND `EXTRA_MED` = 0 AND `LAB_XRAY` > 0 AND `SURGICAL` > 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "GP + LAB + MOP";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "GP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_GP_MED_LAB_MOP()
        {
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'GP' AND `EXTRA_MED` > 0 AND `LAB_XRAY` > 0 AND `SURGICAL` > 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "GP + MED + LAB + MOP";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "GP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_SP()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'SP' AND `EXTRA_MED` = 0 AND `LAB_XRAY` = 0 AND `SURGICAL` = 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "SP";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "SP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_SP_MED()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'SP' AND `EXTRA_MED` > 0 AND `LAB_XRAY` = 0 AND `SURGICAL` = 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "SP + MED";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "SP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_SP_LAB()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'SP' AND `EXTRA_MED` = 0 AND `LAB_XRAY` > 0 AND `SURGICAL` = 0 AND `LAB_XRAY` <> `FEE_AMT` ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "SP + LAB";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "SP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_SP_MOP()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'SP' AND `EXTRA_MED` = 0 AND `LAB_XRAY` = 0 AND `SURGICAL` > 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "SP + MOP";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "SP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_SP_MED_LAB()
        {
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'SP' AND `EXTRA_MED` > 0 AND `LAB_XRAY` > 0 AND `SURGICAL` = 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "SP + MED + LAB";

                SetupColumnHeader(sheet);
                
                SetupDataContent(sheet, dt, "SP");
                                
                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_SP_MED_MOP()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'SP' AND `EXTRA_MED` > 0 AND `LAB_XRAY` = 0 AND `SURGICAL` > 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "SP + MED + MOP";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "SP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_SP_LAB_MOP()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'SP' AND `EXTRA_MED` = 0 AND `LAB_XRAY` > 0 AND `SURGICAL` > 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "SP + LAB + MOP";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "SP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_SP_MED_LAB_MOP()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE = 'SP' AND `EXTRA_MED` > 0 AND `LAB_XRAY` > 0 AND `SURGICAL` > 0 ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "SP + MED + LAB + MOP";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "SP");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_LAB()
        {            
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND `LAB_XRAY` > 0 AND `LAB_XRAY` = `FEE_AMT` ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "LAB";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "LAB");

                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void CreateSheet_PHY()
        {
            string condition = "WHERE v.BATCH_NO = '" + Batch_No + "' AND TYPE in ('CU','PHY','PHY2') ORDER BY VOUCHER_NO;";
            System.Data.DataTable dt = db.query(SQL + condition);

            if (dt.Rows.Count > 0)
            {
                ExcelWorkBook.Worksheets.Add(After: ExcelWorkBook.Sheets[ExcelWorkBook.Sheets.Count]);

                Worksheet sheet = ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count];

                ExcelWorkBook.Worksheets[ExcelWorkBook.Worksheets.Count].Name = "PHY";

                SetupColumnHeader(sheet);

                SetupDataContent(sheet, dt, "PHY");
                                
                Marshal.ReleaseComObject(sheet);

                TotalVouchers += dt.Rows.Count;
            }
        }

        private void SetupColumnHeader(Worksheet sheet)
        {
            sheet.Cells[1, 1] = "Pro_id";
            var r0 = sheet.get_Range("A1").EntireColumn;
            r0.NumberFormat = "@";
            sheet.Cells[1, 2] = "Pro_name";
            sheet.Cells[1, 3] = "Inv_no";
            sheet.Cells[1, 4] = "Inv_date";
            var r1 = sheet.get_Range("D1").EntireColumn;
            r1.NumberFormat = "MM/DD/YYYY";
            sheet.Cells[1, 5] = "Mem_id";
            sheet.Cells[1, 6] = "Mem_name";
            sheet.Cells[1, 7] = "Inc_date";
            var r2 = sheet.get_Range("G1").EntireColumn;
            r2.NumberFormat = "MM/DD/YYYY";
            sheet.Cells[1, 8] = "Ser_type";
            sheet.Cells[1, 9] = "Diag_code1";
            sheet.Cells[1, 10] = "Diag_desc1";
            sheet.Cells[1, 11] = "Diag_code2";
            sheet.Cells[1, 12] = "Diag_desc2";
            sheet.Cells[1, 13] = "pro_code";
            var r3 = sheet.get_Range("M1").EntireColumn;
            r3.NumberFormat = "@";
            sheet.Cells[1, 14] = "Pro_desc";
            sheet.Cells[1, 15] = "Pre_amt";
            sheet.Cells[1, 16] = "Copay";
            sheet.Cells[1, 17] = "Start_sick";
            sheet.Cells[1, 18] = "End_sick";
            var r4 = sheet.get_Range("Q1").EntireColumn;
            r4.NumberFormat = "MM/DD/YYYY";
            var r5 = sheet.get_Range("R1").EntireColumn;
            r5.NumberFormat = "MM/DD/YYYY";
            sheet.Cells[1, 19] = "Vo_no";
            var r6 = sheet.get_Range("S1").EntireColumn;
            r6.NumberFormat = "@";
            sheet.Cells[1, 20] = "Ref_pro_id";
            sheet.Cells[1, 21] = "Ref_pro_na";
            sheet.Cells[1, 22] = "Ref_no";
            sheet.Cells[1, 23] = "Ref_date";
            sheet.Cells[1, 24] = "Remarks";
            sheet.Cells[1, 25] = "Transaction Code";
            var r7 = sheet.get_Range("Y1").EntireColumn;
            r7.NumberFormat = "@";
            sheet.Cells[1, 26] = "Humphrey Remarks";

            //sheet.get_Range("A2").Select();
            //ExcelApp.ActiveWindow.FreezePanes = true;
        }

        private void SetupDataContent(Worksheet sheet, System.Data.DataTable dt, string strtype)
        {
            string trackVoucherNo = "";                
            int execel_row = 2;
            List<decimal> trx_preAmt = null;
            List<string> trx_proCode = null;
            List<string> trx_proDesc = null;
            List<string> trx_h_remark = null;
            foreach (System.Data.DataRow r in dt.Rows)
            {
                try
                {
                    string voucher_remark = "";
                    trackVoucherNo = r["VOUCHER_NO"].ToString();
                 
                    System.Data.DataTable dt2 = db.query("SELECT * FROM humphrey.voucher_fee_line WHERE BATCH_NO = '" + Batch_No + "' AND COMPANY_CODE = '" + r["COMPANY_CODE"].ToString() + "' AND VOUCHER_NO = '" + r["VOUCHER_NO"].ToString() + "' ORDER BY LINE_NO;");
                    foreach (System.Data.DataRow r2 in dt2.Rows)
                    {
                        trx_preAmt = null;
                        trx_proCode = null;
                        trx_proDesc = null;
                        trx_h_remark = null;
                        if (!strtype.Equals("LAB"))
                        {
                            voucher_remark = GetProDesc(r["TYPE"].ToString()) + ((GetProDesc(r["TYPE"].ToString()) == GetProDesc(r2["FEE_CODE"].ToString())) ? "" : "+" + GetProDesc(r2["FEE_CODE"].ToString()));
                        }
                        else
                        {
                            voucher_remark = GetProDesc(r2["FEE_CODE"].ToString());
                        }

                        trx_preAmt = new List<decimal>();
                        trx_preAmt.Add(Convert.ToDecimal(r2["FEE"].ToString()));

                        if (r2["FEE_CODE"].ToString().ToUpper() == "EXTRA MED")
                        {
                            if (Med_Split)
                            {
                                if (!String.IsNullOrEmpty(r["DP_TYPE"].ToString()))
                                {
                                    try
                                    {
                                        trx_preAmt = new List<decimal>();
                                        decimal longM = Convert.ToDecimal(((String.IsNullOrEmpty(r["DP_TYPE"].ToString()) ? "0" : r["DP_TYPE"].ToString())));
                                        decimal fullM = Convert.ToDecimal(((String.IsNullOrEmpty(r2["FEE"].ToString()) ? "0" : r2["FEE"].ToString())));
                                        trx_preAmt.Add(fullM - longM);  //Short Med
                                        trx_preAmt.Add(longM);          //Long Med
                                    }
                                    catch (Exception ex2) 
                                    {
                                        ErrorMessage += "V" + trackVoucherNo + " : (1)" + ex2.Message + "\n";

                                        AuditLog.Log("Exception Line 588 " + ex2.Message);
                                    }
                                }
                            }
                        }
                        if (r2["FEE_CODE"].ToString().ToUpper() == "LAB/XRAY")
                        {
                            string strLabCode = r["LAB_XRAY_CODE"].ToString();
                            decimal sum = 0;
                            decimal input = Convert.ToDecimal(r2["FEE"].ToString());

                            if (!String.IsNullOrEmpty(strLabCode))
                            {
                                try
                                {
                                    string[] arr_LabCode = strLabCode.Split(',');
                                    trx_proCode = new List<string>();
                                    trx_proDesc = new List<string>();
                                    trx_preAmt  = new List<decimal>();
                                    trx_h_remark = new List<string>();
                                    foreach (string s in arr_LabCode)
                                    {
                                        if (!String.IsNullOrEmpty(s))
                                        {
                                            string cpt_code = s.Trim();
                                            string sub_code = "";
                                            if (cpt_code.IndexOf("-") > 0)
                                            {
                                                string[] arr_code = cpt_code.Split('-');
                                                cpt_code = arr_code[0].Trim();
                                                if (arr_code.Length > 1)
                                                {
                                                    sub_code = arr_code[1].Trim();
                                                }
                                            }

                                            string sql = "SELECT CODE, DESCRIPTION, FEE FROM lab_mop_mapping m WHERE INACTIVE = 0 AND TYPE = 'LAB' AND CODE = '" + cpt_code + "'";
                                            if (!String.IsNullOrEmpty(sub_code))
                                            {
                                                sql += " AND subcode = '" + sub_code + "'";
                                            }
                                            sql += " AND (COMPANY_CODE = '" + r["COMPANY_CODE"].ToString().Substring(0, 3) + "' OR COMPANY_CODE = '" + r["COMPANY_CODE"].ToString() + "')";
                                            sql += " AND (DR_CODE = '' OR DR_CODE like '%@"+ r["DR_CODE"].ToString() + "@%') ORDER BY DR_CODE DESC, COMPANY_CODE DESC";
                                            System.Data.DataTable dt3 = db.query(sql);

                                            if (dt3.Rows.Count > 0)
                                            {
                                                trx_proCode.Add(dt3.Rows[0]["CODE"].ToString());
                                                trx_proDesc.Add(dt3.Rows[0]["DESCRIPTION"].ToString());
                                                trx_preAmt.Add(Convert.ToDecimal(dt3.Rows[0]["FEE"].ToString()));
                                                trx_h_remark.Add("");
                                                sum += Convert.ToDecimal(dt3.Rows[0]["FEE"].ToString());
                                            }
                                            else
                                            {
                                                trx_proCode.Add(s.Trim());
                                                trx_proDesc.Add("");
                                                trx_preAmt.Add(input);
                                                trx_h_remark.Add("＃NO LAB FOUND＃");
                                                sum += input;
                                            }
                                        }
                                    }
                                    if (input != sum) {
                                        for (int i=0; i<trx_h_remark.Count; i++)
                                        {
                                            if (String.IsNullOrEmpty(trx_h_remark[i])) trx_h_remark[i] = "＃TOTAL NOT MATCHED＃";
                                        }
                                    }
                                }
                                catch (Exception ex3) 
                                {
                                    ErrorMessage += "V" + trackVoucherNo + " : (2)" + ex3.Message + "\n";

                                    AuditLog.Log("Exception Line 662 " + ex3.Message);
                                }
                            }
                            else
                            {
                                trx_h_remark = new List<string>();
                                trx_h_remark.Add("＃LAB MISSING＃");
                            }
                        }
                        if (r2["FEE_CODE"].ToString().ToUpper() == "SURGICAL")
                        {
                            string strSurgicalCode = r["SURGICAL_CODE"].ToString();
                            decimal sum = 0;
                            decimal input = Convert.ToDecimal(r2["FEE"].ToString());

                            if (!String.IsNullOrEmpty(strSurgicalCode))
                            {
                                try
                                {
                                    string[] arr_SurgicalCode = strSurgicalCode.Split(',');
                                    trx_proCode = new List<string>();
                                    trx_proDesc = new List<string>();
                                    trx_preAmt = new List<decimal>();
                                    trx_h_remark = new List<string>();
                                    foreach (string s in arr_SurgicalCode)
                                    {
                                        if (!String.IsNullOrEmpty(s))
                                        {
                                            string cpt_code = s.Trim();
                                            string sub_code = "";
                                            if (cpt_code.IndexOf("-") > 0)
                                            {
                                                string[] arr_code = cpt_code.Split('-');
                                                cpt_code = arr_code[0].Trim();
                                                if (arr_code.Length > 1)
                                                {
                                                    sub_code = arr_code[1].Trim();
                                                }
                                            }

                                            string sql = "SELECT CODE, DESCRIPTION, FEE FROM lab_mop_mapping m WHERE INACTIVE = 0 AND TYPE = 'MOP' AND CODE = '" + cpt_code + "'";
                                            if (!String.IsNullOrEmpty(sub_code))
                                            {
                                                sql += " AND subcode = '" + sub_code + "'";
                                            }
                                            sql += " AND (COMPANY_CODE = '" + r["COMPANY_CODE"].ToString().Substring(0, 3) + "' OR COMPANY_CODE = '" + r["COMPANY_CODE"].ToString() + "')";
                                            sql += " AND (DR_CODE = '' OR DR_CODE like '%@" + r["DR_CODE"].ToString() + "@%') ORDER BY DR_CODE DESC, COMPANY_CODE DESC";
                                            System.Data.DataTable dt3 = db.query(sql);

                                            //System.Data.DataTable dt3 = db.query("SELECT CODE, DESCRIPTION, FEE FROM lab_mop_mapping m WHERE TYPE = 'MOP' AND CODE = '" + s.Trim() + "' AND INACTIVE = 0;");

                                            if (dt3.Rows.Count > 0)
                                            {
                                                trx_proCode.Add(dt3.Rows[0]["CODE"].ToString());
                                                trx_proDesc.Add(dt3.Rows[0]["DESCRIPTION"].ToString());
                                                trx_preAmt.Add(Convert.ToDecimal(dt3.Rows[0]["FEE"].ToString()));
                                                trx_h_remark.Add("");
                                                sum += Convert.ToDecimal(dt3.Rows[0]["FEE"].ToString());
                                            }
                                            else
                                            {
                                                trx_proCode.Add(s.Trim());
                                                trx_proDesc.Add("");
                                                trx_preAmt.Add(input);
                                                trx_h_remark.Add("＃NO MOP FOUND＃");
                                                sum += input;
                                            }
                                        }
                                    }
                                    if (input != sum) {
                                        for (int i = 0; i < trx_h_remark.Count; i++)
                                        {
                                            if (String.IsNullOrEmpty(trx_h_remark[i])) trx_h_remark[i] = "＃TOTAL NOT MATCHED＃";
                                        }
                                    }
                                }
                                catch (Exception ex3) 
                                {
                                    ErrorMessage += "V" + trackVoucherNo + " : (3)" + ex3.Message + "\n";

                                    AuditLog.Log("Exception Line 742 " + ex3.Message);
                                }
                            }
                            else
                            {
                                trx_h_remark = new List<string>();
                                trx_h_remark.Add("＃MOP MISSING＃");
                            }
                        }

                        string trx_company_doctor_code = "";
                        string sql2 = "SELECT COMPANY_DR_CODE FROM company_doctor WHERE DR_CODE = '" + r["DR_CODE"].ToString() + "' AND COMPANY_CODE = '" + r["COMPANY_CODE"].ToString().Trim() + "' AND (TYPE1 = '" + r["TYPE"].ToString() + "' OR TYPE2 = '" + r["TYPE"].ToString() + "') AND '"+ DateConvert2(r["TREATMENT_DATE"].ToString())+ "' < IFNULL(TERM_DATE, '2099-12-31')";
                        System.Data.DataTable dt4 = db.query(sql2);
                        if (dt4.Rows.Count == 1)
                        {
                            trx_company_doctor_code = dt4.Rows[0]["COMPANY_DR_CODE"].ToString();
                        }
                        else
                        {
                            trx_h_remark = new List<string>();
                            for (int i = 1; i <= trx_preAmt.Count; i++)
                            {
                                trx_h_remark.Add("＃Wrong Company Doctor Information＃");
                            }
                        }
                        
                        if (!String.IsNullOrEmpty(r["DR_TERM_DATE"].ToString()))
                        {
                            if ((DateTime.Parse(r["TREATMENT_DATE"].ToString()) - DateTime.Parse(r["DR_TERM_DATE"].ToString())).TotalHours > 1)
                            {

                                trx_h_remark = new List<string>();
                                for (int i = 1; i <= trx_preAmt.Count; i++)
                                {
                                    trx_h_remark.Add("＃Doctor terminated＃");
                                }
                            }
                        }


                        for (int i = 1; i <= trx_preAmt.Count; i++)
                        {
                            try
                            {
                                if (trx_preAmt[i - 1] > 0)
                                {
                                    if (trx_proCode == null)
                                    {
                                        sheet.Cells[execel_row, 13] = GetProCode(r2["FEE_CODE"].ToString(), strtype, i.ToString());
                                    }
                                    else
                                    {
                                        if (trx_proCode != null && trx_proCode.Count >= i) sheet.Cells[execel_row, 13] = trx_proCode[i - 1];
                                        if (trx_proDesc != null && trx_proDesc.Count >= i) sheet.Cells[execel_row, 14] = trx_proDesc[i - 1];
                                    }
                                    sheet.Cells[execel_row, 15] = trx_preAmt[i - 1];

                                    sheet.Cells[execel_row, 1] = trx_company_doctor_code;
                                    sheet.Cells[execel_row, 2] = r["DR_E_NAME"].ToString();
                                    sheet.Cells[execel_row, 3] = r["INVOICE_NO"].ToString();
                                    sheet.Cells[execel_row, 4] = DateConvert(invoice_date);
                                    sheet.Cells[execel_row, 5] = r["MEMBER_CODE"].ToString();
                                    sheet.Cells[execel_row, 6] = r["MEMBER_E_NAME"].ToString();
                                    sheet.Cells[execel_row, 7] = DateConvert(r["TREATMENT_DATE"].ToString());
                                    sheet.Cells[execel_row, 8] = ((r["TYPE"].ToString() == "PHY" || r["TYPE"].ToString() == "PHY2" || r["TYPE"].ToString() == "CU") ? "PH" : r["TYPE"].ToString());
                                    sheet.Cells[execel_row, 9] = r["DIAG_CODE1"].ToString();
                                    sheet.Cells[execel_row, 10] = r["DIAG_DESC1"].ToString();
                                    sheet.Cells[execel_row, 11] = r["DIAG_CODE2"].ToString();
                                    sheet.Cells[execel_row, 12] = r["DIAG_DESC2"].ToString();
                                    sheet.Cells[execel_row, 16] = ((r2["LINE_NO"].ToString() == "1") ? r["CO_PAY"].ToString() : "0");
                                    sheet.Cells[execel_row, 17] = DateConvert(r["SL_FROM"].ToString());
                                    sheet.Cells[execel_row, 18] = DateConvert(r["SL_TO"].ToString());
                                    sheet.Cells[execel_row, 19] = r["VOUCHER_NO"].ToString();
                                    sheet.Cells[execel_row, 25] = r["MEMBER_STAFF_NO"].ToString();
                                    sheet.Cells[execel_row, 24] = voucher_remark;
                                    
                                    if (trx_h_remark != null && trx_h_remark.Count >= i)
                                    {
                                        sheet.Cells[execel_row, 26] = trx_h_remark[i - 1];
                                        if (!String.IsNullOrEmpty(trx_h_remark[i - 1])) sheet.Cells[execel_row, 26].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    }
                                    execel_row++;
                                }
                            }
                            catch (Exception ex3)
                            { 
                                ErrorMessage += "V" + trackVoucherNo + " : (4)" + ex3.Message + "\n";

                                AuditLog.Log("Exception Line 801 " + ex3.Message);
                            }
                        }
                    }

                    
                }
                catch (Exception exx) 
                {
                    ErrorMessage += "V" + trackVoucherNo + " : (5)" + exx.Message + "\n";

                    AuditLog.Log("Exception Line 810 " + exx.Message);
                }
            }
            sheet.Columns.AutoFit();
        }


  





        #region Add-on Functions

        private String DateConvert(string d)
        {
            string result = "";
            if (!String.IsNullOrEmpty(d))
            {
                try
                {
                    DateTime oDate = DateTime.Parse(d);
                    result = oDate.ToString("MM/dd/yyyy");
                }
                catch (Exception ex) 
                {
                    ErrorMessage += "DateConvert:" + ex.Message + " ("+d+")\n";

                    AuditLog.Log("Exception Line 834 " + ex.Message);
                }
            }
            return result;
        }

        private String DateConvert2(string d)
        {
            string result = "";
            if (!String.IsNullOrEmpty(d))
            {
                try
                {
                    DateTime oDate = DateTime.Parse(d);
                    result = oDate.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    ErrorMessage += "DateConvert:" + ex.Message + " (" + d + ")\n";

                    AuditLog.Log("Exception Line 834 " + ex.Message);
                }
            }
            return result;
        }

        private string GetProCode(string t, string type, string subtype = "")
        {
            string result = "";
            switch (t.Substring(0, 3).ToUpper())
            {
                case "GP-":
                    result = AIA_GP;
                    break;
                case "SP-":
                    result = AIA_SP;
                    break;
                case "PHY":
                    result = AIA_PHY;
                    break;
                case "CU-":
                    result = AIA_CU;
                    break;
                case "LAB":
                    if (type == "GP") result = AIA_GP_LAB;
                    if (type == "SP") result = AIA_SP_LAB;
                    if (type == "LAB") result = AIA_SP_LAB;
                    break;
                case "EXT":
                    if (type == "GP")
                    {
                        if (Med_Split)
                        {
                            if (subtype == "2")
                                result = AIA_GP_MED_L;
                            else
                                result = AIA_GP_MED_S;
                        }
                        else
                        {
                            result = AIA_GP_MED;
                        }
                    }
                    if (type == "SP")
                    {
                        if (Med_Split)
                        {
                            if (subtype == "2")
                                result = AIA_SP_MED_L;
                            else
                                result = AIA_SP_MED_S;
                        }
                        else
                        {
                            result = AIA_SP_MED;
                        }
                    }
                    break;
                case "SUR":
                    if (type == "GP") result = AIA_GP_MOP;
                    if (type == "SP") result = AIA_SP_MOP;
                    break;
            }
            return result;
        }

        private string GetProDesc(string t)
        {
            string result = "";
            if (t.Length > 3)  t = t.Substring(0, 3);
            switch (t.ToUpper())
            {
                case "GP":
                case "GP-":
                    result = "GP";
                    break;
                case "SP":
                case "SP-":
                    result = "SP";
                    break;
                case "PHY":
                case "CU":
                case "CU-":
                    result = "PHY";
                    break;
                case "LAB":
                    result = "LAB";                    
                    break;
                case "EXT":
                    result = "MED";
                    break;
                case "SUR":
                    result = "MOP";
                    break;
            }
            return result;
        }

        #endregion
    }
}
