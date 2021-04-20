using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace HPMS_EDI_Generator
{
	public class MySQLdb
    {

        private MySqlConnection conn;

        public MySQLdb()
		{
            //****William Development
            //this.conn = new MySqlConnection("server=127.0.0.1;uid=rpt_viewer;pwd=qN##SVyswi9V8TYYdOnR;database=humphrey");
            //****Production
            this.conn = new MySqlConnection("server=192.168.179.122;uid=rpt_viewer;pwd=qN##SVyswi9V8TYYdOnR;database=humphrey");
        }


		public DataTable query(string SQL)
		{
			DataTable dt = new DataTable();
			if (this.conn.State == ConnectionState.Closed)
			{
				this.conn.Open();
			}
			
			try
			{
				MySqlCommand cmd = new MySqlCommand(SQL, conn);
				MySqlDataReader mysqlData = cmd.ExecuteReader();

				if (!mysqlData.HasRows)
				{
					Console.WriteLine("No data.");
                }
                else
                {
					dt.Load(mysqlData);
				}
				mysqlData.Close();
				this.conn.Close();
			}
			catch (MySql.Data.MySqlClient.MySqlException ex)
			{
				Console.WriteLine("Error " + ex.Number + " : " + ex.Message);
			}
			return dt;
		}
	}
}
