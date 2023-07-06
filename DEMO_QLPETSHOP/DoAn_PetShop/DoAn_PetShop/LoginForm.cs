using DoAn_PetShop.CSDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn_PetShop
{
    public partial class LoginForm : Form
    {
        CSDL_Login csdl_lo = new CSDL_Login();
        string title = "Dogily Petshop Management System";
        public LoginForm()
        {
            InitializeComponent();          
        }

        private void btnFroget_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact your boss!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (csdl_lo.checklogin(txtName.Text, txtPassword.Text))
            {
                MessageBox.Show("Welcome back " + txtName.Text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
                MainForm form = csdl_lo.main;
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Tên người dùng hoặc mật khẩu không hợp lệ", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
