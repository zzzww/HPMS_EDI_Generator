using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Renci.SshNet.Connection;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;

namespace HPMS_EDI_Generator
{
    static class Library
    {
        public static void Compress(string zipFileName, string[] importFiles)
        {
            try
            {
                byte[] buffer = new byte[4096];

                using (ZipOutputStream s = new ZipOutputStream(File.Create(Path.Combine(Path.GetTempPath(), zipFileName))))
                {
                    // 設定壓縮比
                    s.Password = "hpm2bluecross";
                    s.SetLevel(7);

                    // 逐一將資料夾內的檔案抓出來壓縮，並寫入至目的檔(.ZIP)
                    foreach (string filename in importFiles)
                    {
                        ZipEntry entry = new ZipEntry(filename);
                        s.PutNextEntry(entry);

                        using (FileStream fs = File.OpenRead(Path.Combine(Path.GetTempPath(), filename)))
                            StreamUtils.Copy(fs, s, buffer);
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.Log("Exception Line 44 " + ex.Message);
            }
        }


        private const string FTP_SERVER = "192.168.179.239";
        private const string FTP_USR_ID = "william.lo";
        private const string FTP_USR_PW = "williamtest";
        //private const string FTP_SERVER = "210.177.12.144";
        //private const string FTP_USR_ID = "sfhpmprd01";
        //private const string FTP_USR_PW = "M8x72bpaT";
        public static bool FileUploadSFTP(string fileName)
        {
            try
            {
                using (var client = new Renci.SshNet.SftpClient(FTP_SERVER, 22, FTP_USR_ID, FTP_USR_PW))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        using (var fileStream = new FileStream(Path.Combine(Path.GetTempPath(), fileName), FileMode.Open))
                        {
                            client.BufferSize = 4 * 1024; // bypass Payload error large files
                            client.UploadFile(fileStream, fileName);
                            AuditLog.Log("BlueX File is uploaded to FTP successfully. ["+ fileName+"]");
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                AuditLog.Log("Exception Line 109 " + e.Message);
                return false;
            }
        }

        public static bool CheckFileExistsOnFTP(string fileName)
        {
            try
            {
                string logMessage = "";
                long ftp_fileLen = 0;
                using (var sftp = new Renci.SshNet.SftpClient(FTP_SERVER, 22, FTP_USR_ID, FTP_USR_PW))
                {
                    try
                    {

                        sftp.Connect();
                        SftpFile file = sftp.Get(fileName);
                        ftp_fileLen = file.Attributes.Size;
                    }
                    catch (Exception Sftpex)
                    {
                        AuditLog.Log("Exception Line 129 " + Sftpex.ToString());
                    }
                }
                long fileLen = new System.IO.FileInfo(Path.Combine(Path.GetTempPath(), fileName)).Length;
                logMessage = "BlueX File is existed to FTP? ";
                if (ftp_fileLen == fileLen)
                {
                    logMessage += "[Yes]";
                }
                else
                {
                    logMessage += "[No]";
                }
                AuditLog.Log(logMessage);
                return true;
            }
            catch (WebException ex)
            {
                AuditLog.Log("Exception Line 148 " + ex.Message);
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return false;
            }
            return false;
        }


        public static void WriteFile(string fileName, string data)
        {
            string myTempFile = Path.Combine(Path.GetTempPath(), fileName);
            using (StreamWriter sw = new StreamWriter(myTempFile))
            {
                sw.WriteLine(data);
            }
        }

        public static Byte[] ToByteArray(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            byte[] chunk = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(chunk, 0, chunk.Length)) > 0)
            {
                ms.Write(chunk, 0, bytesRead);
            }

            return ms.ToArray();
        }

    
    }
}
