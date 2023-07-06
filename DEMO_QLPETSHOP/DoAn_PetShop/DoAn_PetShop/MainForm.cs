using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn_PetShop
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            btnDashboard.PerformClick();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            openChildForm(new Dashboard());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            openChildForm(new BillingForm());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoginForm login = new LoginForm();
                this.Dispose();
                login.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

       

        private void btnOder_Click(object sender, EventArgs e)
        {
            openChildForm(new OderEntry());
        }
        #region Method
        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            lblTitle.Text = childForm.Text;
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        //public void loadDailySale()
        //{
        //    string sdate = DateTime.Now.ToString("yyyyMMdd");
        //    try
        //    {
        //        cn.Open();

        //        cm = new SqlCommand("SELECT ISNULL(SUM(total),0) AS total FROM tbBilling WHERE transno LIKE'" + sdate + "%'", cn);
        //        lblDailySale.Text = double.Parse(cm.ExecuteScalar().ToString()).ToString("#,#0.00");
        //        cn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        cn.Close();
        //        MessageBox.Show(ex.Message);
        //    }

        //}
        #endregion Method
    }
}
