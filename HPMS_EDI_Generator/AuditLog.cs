using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace HPMS_EDI_Generator
{
    class AuditLog
    {
        private static string outputPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        private static string logFileName = "log.txt";
        public static string User = "";

        public static void Log(string t)
        {
            string fileName = Path.Combine(outputPath, logFileName);
            if (!File.Exists(fileName))
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write("");
                }
            }

            string content = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + User.PadRight(12) + t;
            using (StreamWriter w = File.AppendText(fileName))
            {
                w.WriteLine(content);
            }
        }

    }
}
