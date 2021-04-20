using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace HPMS_EDI_Generator
{
    public class BlueX
    {
        private const string BlueX_SERVICE_GP = "99212";
        private const string BlueX_SERVICE_SP = "99212";
        private const string BlueX_SERVICE_PHY = "97530";
        private const string BlueX_SERVICE_MED = "RXEX";
        private const string BlueX_SERVICE_LAB = "80050";
        private const string BlueX_SERVICE_MOP = "MOP";
        private const string BlueX_BENEFIT_GP = "PHY";
        private const string BlueX_BENEFIT_SP = "SPL";
        private const string BlueX_BENEFIT_PHY = "PHS";
        private const string BlueX_BENEFIT_MED = "MED";
        private const string BlueX_BENEFIT_LAB = "LAB";
        private const string BlueX_BENEFIT_MOP = "MOP";

        private string filePath = "\\Output\\BlueX\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\\";

        private MySQLdb db = new MySQLdb();
        public int TotalVouchers = 0;
        public string ErrorMessage = "";
        private string Batch_No = "";
        public string invoice_date = "";

        public bool isAutoSubmission = false;
        public bool FTP_RESULT_T = false;
        public bool FTP_RESULT_B = false;


        public bool GetSubmissionResult()
        {
            return (FTP_RESULT_T && FTP_RESULT_B);
        }


        public BlueX(string _batch_no)
        {
            Batch_No = _batch_no;
            TotalVouchers = 0;
            ErrorMessage = "";
        }


        public void GenerateEDI()
        {
            try
            {
                CreateTempPair();
                CreateBillPair();
                OpenFolder();
            }
            catch (Exception exHandle)
            {
                ErrorMessage += "Main:" + exHandle.Message + "\n";

                AuditLog.Log("Exception Line 64 " + exHandle.Message);
            }
            finally
            {
            }
        }

        private void CreateTempPair()
        {
            string trackVoucherNo = "";
            string lastVoucherNo = "";
            System.Data.DataTable dt = db.query("SELECT cd.COMPANY_DR_CODE, cd.LOC_CODE, v.*, d.TERM_DATE AS DR_TERM_DATE, (SELECT FEE FROM voucher_fee_line vf WHERE v.BATCH_NO = vf.BATCH_NO AND v.VOUCHER_NO = vf.VOUCHER_NO AND v.COMPANY_CODE = vf.COMPANY_CODE AND vf.LINE_NO = 1) AS 'FEE', (SELECT COUNT(*) FROM company_doctor d WHERE v.DR_CODE=d.DR_CODE AND d.COMPANY_CODE = 'BLUEX' AND LOC_CODE <> '' AND v.TREATMENT_DATE < IFNULL(TERM_DATE, '2099-12-31') ) AS 'NUMOFDOC' FROM voucher_line v INNER JOIN doctor d ON v.DR_CODE=d.DR_CODE LEFT JOIN company_doctor cd ON v.DR_CODE=cd.DR_CODE AND cd.COMPANY_CODE = 'BLUEX' AND v.TREATMENT_DATE < IFNULL(cd.TERM_DATE, '2099-12-31') WHERE v.FEE_AMT > 0 AND v.BATCH_NO = '" + Batch_No + "' ORDER BY VOUCHER_NO;");

            if (dt.Rows.Count > 0)
            {
                string data = "";
                data += ("HPM").PadRight(5);
                data += DateConvert(invoice_date).PadLeft(8);
                data += " " + dt.Rows.Count.ToString().PadLeft(5, '0');
                data += "T";

                Library.WriteFile("TH" + DateConvert(invoice_date) + "HPM.txt", data);

                data = "";
                foreach (System.Data.DataRow r in dt.Rows)
                {
                    try
                    {
                        trackVoucherNo = r["VOUCHER_NO"].ToString();
                        data += r["POLICY_NO"].ToString().Replace(",", ".").PadRight(10);
                        data += "".PadLeft(3) + " ";
                        string IDtype = CheckIDType(r["INSURED_NO"].ToString());
                        if ((IDtype == "INSUREDNO_4") || (IDtype == "INSUREDNO_7"))
                        {
                            data += r["INSURED_NO"].ToString().PadRight(7);
                            data += "".PadLeft(16);
                        }
                        else
                        {
                            data += "".PadLeft(7);
                            data += r["INSURED_NO"].ToString().PadRight(16);
                        }
                        data += r["MEMBER_E_NAME"].ToString().Trim().PadRight(30);
                        data += r["VOUCHER_NO"].ToString().PadRight(7);
                        data += r["COMPANY_DR_CODE"].ToString().PadRight(5);
                        data += GetServiceCode(r["TYPE"].ToString()).PadRight(8);
                        data += r["DIAG_CODE1"].ToString().PadRight(8);
                        data += r["DIAG_CODE2"].ToString().PadRight(8);
                        data += GetBenefitCode(r["TYPE"].ToString()).PadRight(3);
                        data += ((decimal)decimal.Parse(r["FEE"].ToString()) - (decimal)decimal.Parse(r["CO_PAY"].ToString())).ToString("0.00").PadLeft(11);
                        data += " " + DateConvert(r["TREATMENT_DATE"].ToString()).PadRight(8);
                        data += r["LOC_CODE"].ToString().PadRight(4);
                        data += r["SICK_LEAVE"].ToString().PadLeft(3);
                        data += ("HPM").PadRight(5);
                        data += "\n";

                        //##########################################Checking#################################################
                        if (!String.IsNullOrEmpty(r["POLICY_NO"].ToString()) && r["POLICY_NO"].ToString().Length != 10)
                        {
                            ErrorMessage += "V" + trackVoucherNo + " : Wrong Policy No. \n";
                        }
                        if (!String.IsNullOrEmpty(r["VOUCHER_NO"].ToString()) && r["VOUCHER_NO"].ToString().Length > 7)
                        {
                            ErrorMessage += "V" + trackVoucherNo + " : Wrong Voucher No. \n";
                        }
                        if (!String.IsNullOrEmpty(r["MEMBER_STAFF_NO"].ToString()) || !String.IsNullOrEmpty(r["DP_TYPE"].ToString()))
                        {
                            ErrorMessage += "V" + trackVoucherNo + " : Insured No. should be combined. \n";
                        }
                        if ((r["NUMOFDOC"].ToString() != "1") && (lastVoucherNo != trackVoucherNo))
                        {
                            ErrorMessage += "V" + trackVoucherNo + " : " + (r["NUMOFDOC"].ToString() == "0" ? "Missing" : "Wrong") + " Company Doctor Information. \n";
                        }
                        if (!String.IsNullOrEmpty(r["DR_TERM_DATE"].ToString()))
                        {
                            if ((DateTime.Parse(r["TREATMENT_DATE"].ToString()) - DateTime.Parse(r["DR_TERM_DATE"].ToString())).TotalHours > 1)
                            {
                                ErrorMessage += "V" + trackVoucherNo + " : Doctor terminated. \n";
                            }
                        }
                        //##########################################Checking#################################################

                        lastVoucherNo = trackVoucherNo;
                    }
                    catch (Exception ex)
                    {
                        AuditLog.Log("Exception Line 119 " + ex.Message);
                    }
                }
                Library.WriteFile("TD" + DateConvert(invoice_date) + "HPM.txt", data);

                string zipFile = CompressZip("TEMP");
                CopyToOutput(zipFile);

                if (isAutoSubmission)
                {
                    if (String.IsNullOrEmpty(ErrorMessage))
                    {
                        if (File.Exists(Path.Combine(Path.GetTempPath(), zipFile)))
                        {
                            FTP_RESULT_T = UploadToFtp(zipFile);
                        }
                    }
                }

                TotalVouchers += dt.Rows.Count;
            }
        }



        private void CreateBillPair()
        {
            string trackVoucherNo = "";
            System.Data.DataTable dt = db.query("SELECT cd.COMPANY_DR_CODE, cd.LOC_CODE, v.CO_PAY, v.POLICY_NO,v.INSURED_NO,v.MEMBER_E_NAME,v.TYPE,v.DIAG_CODE1,v.DIAG_CODE2,v.TREATMENT_DATE,v.SICK_LEAVE, f.* FROM voucher_line v LEFT JOIN voucher_fee_line f ON v.COMPANY_CODE=f.COMPANY_CODE AND v.BATCH_NO=f.BATCH_NO AND v.VOUCHER_NO=f.VOUCHER_NO LEFT JOIN company_doctor cd ON v.DR_CODE=cd.DR_CODE AND cd.COMPANY_CODE = 'BLUEX' AND v.TREATMENT_DATE < IFNULL(cd.TERM_DATE, '2099-12-31') WHERE v.BATCH_NO = '" + Batch_No + "' ORDER BY VOUCHER_NO, LINE_NO;");

            if (dt.Rows.Count > 0)
            {
                string data = "";
                data += ("HPM").PadRight(5);
                data += DateConvert(invoice_date).PadLeft(8);
                data += " " + dt.Rows.Count.ToString().PadLeft(5, '0');
                data += "T";

                Library.WriteFile("BH" + DateConvert(invoice_date) + "HPM.txt", data);


                data = "";
                foreach (System.Data.DataRow r in dt.Rows)
                {
                    try
                    {
                        trackVoucherNo = r["VOUCHER_NO"].ToString();
                        if (decimal.Parse(r["FEE"].ToString()) > 0)
                        {
                            data += r["POLICY_NO"].ToString().Replace(",", ".").PadLeft(10);
                            data += "".PadLeft(3) + " ";
                            string IDtype = CheckIDType(r["INSURED_NO"].ToString());
                            if ((IDtype == "INSUREDNO_4") || (IDtype == "INSUREDNO_7"))
                            {
                                data += r["INSURED_NO"].ToString().PadRight(7);
                                data += "".PadLeft(16);
                            }
                            else
                            {
                                data += "".PadLeft(7);
                                data += r["INSURED_NO"].ToString().PadRight(16);
                            }
                            data += r["MEMBER_E_NAME"].ToString().Trim().PadRight(30);
                            data += r["VOUCHER_NO"].ToString().PadRight(7);
                            data += r["COMPANY_DR_CODE"].ToString().PadRight(5);
                            data += GetServiceCode(r["FEE_CODE"].ToString()).PadRight(8);
                            data += r["DIAG_CODE1"].ToString().PadRight(8);
                            data += r["DIAG_CODE2"].ToString().PadRight(8);
                            data += GetBenefitCode(r["FEE_CODE"].ToString()).PadRight(3);
                            if (r["LINE_NO"].ToString().Equals("1"))
                                data += ((decimal)decimal.Parse(r["FEE"].ToString()) - (decimal)decimal.Parse(r["CO_PAY"].ToString())).ToString("0.00").PadLeft(11);
                            else
                                data += ((decimal)decimal.Parse(r["FEE"].ToString())).ToString("0.00").PadLeft(11);
                            data += " " + DateConvert(r["TREATMENT_DATE"].ToString()).PadRight(8);
                            data += r["LOC_CODE"].ToString().PadRight(4);
                            data += r["SICK_LEAVE"].ToString().PadLeft(3);
                            data += ("HPM").PadRight(5);
                            data += "\n";
                        }
                    }
                    catch (Exception ex)
                    {
                        AuditLog.Log("Exception Line 196 " + ex.Message);
                    }
                }
                Library.WriteFile("BD" + DateConvert(invoice_date) + "HPM.txt", data);

                string zipFile = CompressZip("BILL");
                CopyToOutput(zipFile);

                if (isAutoSubmission)
                {
                    if (String.IsNullOrEmpty(ErrorMessage))
                    {
                        if (File.Exists(Path.Combine(Path.GetTempPath(), zipFile)))
                        {
                            FTP_RESULT_B = UploadToFtp(zipFile);
                        }
                    }
                }

            }
        }

        private string CompressZip(string type)
        {
            string prefix = "";
            string suffix = DateConvert(invoice_date) + "HPM";
            switch (type.ToUpper())
            {
                case "TEMP":
                    prefix = "T";
                    break;
                case "BILL":
                    prefix = "B";
                    break;
            }
            string[] filename = new string[2];
            filename[0] = prefix + "H" + suffix + ".txt";
            filename[1] = prefix + "D" + suffix + ".txt";
            Library.Compress(prefix + "Z" + suffix + ".ZIP", filename);
            return prefix + "Z" + suffix + ".ZIP";
        }


        private bool UploadToFtp(string fileName)
        {
            if (Library.FileUploadSFTP(fileName))
            {
                return (Library.CheckFileExistsOnFTP(fileName));
            }
            else
            {
                return false;
            }
        }

        private void CopyToOutput(string fileName)
        {
            try
            {
                string outputPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + filePath;

                if (!Directory.Exists(outputPath)) System.IO.Directory.CreateDirectory(outputPath);

                System.IO.File.Copy(System.IO.Path.Combine(Path.GetTempPath(), fileName), System.IO.Path.Combine(outputPath, fileName), true);

                AuditLog.Log("BlueX File is created. [" + Path.Combine(filePath, fileName) + "]");
            }
            catch (Exception ex)
            {
                AuditLog.Log("Exception Line 261 " + ex.Message);
            }
        }

        private string GetBenefitCode(string type)
        {
            string result = "";
            string t = type;
            if (t.Length >= 3) t = t.Substring(0, 3);
            switch (t.ToUpper())
            {
                case "GP":
                case "GP-":
                    result = BlueX_BENEFIT_GP;
                    break;
                case "SP":
                case "SP-":
                    result = BlueX_BENEFIT_SP;
                    break;
                case "PHY":
                    result = BlueX_BENEFIT_PHY;
                    break;
                case "LAB":
                case "CU":
                    result = BlueX_BENEFIT_LAB;
                    break;
                case "EXT":
                    result = BlueX_BENEFIT_MED;
                    break;
                case "SUR":
                    result = BlueX_BENEFIT_MOP;
                    break;
            }
            if (type.Length > 3 && type.Substring(0, 4) == "PHY2")
            {
                result = BlueX_BENEFIT_LAB;
            }
            return result;
        }


        private string GetServiceCode(string type)
        {
            string result = "";
            string t = type;
            if (t.Length >= 3) t = t.Substring(0, 3);
            switch (t.ToUpper())
            {
                case "GP":
                case "GP-":
                    result = BlueX_SERVICE_GP;
                    break;
                case "SP":
                case "SP-":
                    result = BlueX_SERVICE_SP;
                    break;
                case "PHY":
                    result = BlueX_SERVICE_PHY;
                    break;
                case "LAB":
                case "CU":
                    result = BlueX_SERVICE_LAB;
                    break;
                case "EXT":
                    result = BlueX_SERVICE_MED;
                    break;
                case "SUR":
                    result = BlueX_SERVICE_MOP;
                    break;
            }
            if (type.Length > 3 && type.Substring(0, 4) == "PHY2")
            {
                result = BlueX_SERVICE_LAB;
            }
            return result;
        }



        private string CheckIDType(string t)
        {
            int n;
            string result = "STAFFID";
            try
            {
                if (t.Length == 4)
                {
                    t = t.Replace(" ", "#");
                    bool isNumeric = int.TryParse(t, out n);
                    if (isNumeric) result = "INSUREDNO_4";
                }
                if (t.Length == 7)
                {
                    t = t.Replace(" ", "#");
                    if ((t.Substring(4, 1) == "W") || (t.Substring(4, 1) == "H") || (t.Substring(4, 1) == "C"))
                    {
                        bool isNumeric = int.TryParse(t.Substring(5, 2), out n);
                        if (isNumeric) result = "INSUREDNO_7";
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.Log("Exception Line 350 " + ex.Message);
            }
            return result;
        }


        private String DateConvert(string d)
        {
            string result = "";
            if (!String.IsNullOrEmpty(d))
            {
                try
                {
                    DateTime oDate = DateTime.Parse(d);
                    result = oDate.ToString("yyyyMMdd");
                }
                catch (Exception ex)
                {
                    AuditLog.Log("Exception Line 368 " + ex.Message);
                }
            }
            return result;
        }

        private void OpenFolder()
        {
            string folderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + filePath;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = folderPath,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }
    }
}
