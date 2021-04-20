using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HPMS_EDI_Generator
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsername.Text) || String.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter the username and password.", "EDI Generator");
                return;
            }


            bool isLogin = false;
            MySQLdb db = new MySQLdb();
            DataTable dt = db.query("SELECT * FROM staff where STAFF_CODE = '"+txtUsername.Text+"' AND PWD = '"+txtPassword.Text+"';");

            if (dt.Rows.Count> 0)
            {
                isLogin = true;
            }
            

            if (isLogin)
            {
                AuditLog.User = txtUsername.Text;
                AuditLog.Log("User logged on.");
                FormGenerator f = new FormGenerator(); // This is bad
                f.StartPosition = FormStartPosition.Manual;
                f.Location = new Point(this.Location.X, this.Location.Y);
                this.Hide();
                f.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login Failure. Please check username or password.", "EDI Generator");
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            txtPassword.Text = "";
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}
