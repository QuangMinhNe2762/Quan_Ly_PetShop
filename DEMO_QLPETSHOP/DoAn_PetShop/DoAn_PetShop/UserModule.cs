using DoAn_PetShop.CSDL;
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
    public partial class UserModule : Form
    {
        CSDL_User csdl_user = new CSDL_User();
        string title = "Dogily PetShop Management System";
        UserForm userform;
        bool check = false;
        public UserModule(UserForm user)
        {
            InitializeComponent();
            userform = user;
        }

        private void UserModule_Load(object sender, EventArgs e)
        {
            cbRole.DataSource = csdl_user.LoadCBB();
            cbRole.DisplayMember = "tencv";
            cbRole.ValueMember = "macv";
            txtma.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CheckField();
            if (check)
            {
                if (MessageBox.Show("Are you sure you want to add this user? ", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (csdl_user.addUser(txtName.Text, txtAddress.Text, txtPhone.Text, cbRole.SelectedValue.ToString(), dtDob.Text, txtPass.Text))
                    {
                        MessageBox.Show("Add success", title);
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Add failed", title);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CheckField();
            if (check)
            {
                if (MessageBox.Show("Are you sure you want to update this user? ", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (csdl_user.updateUser(txtma.Text, txtName.Text, txtAddress.Text, txtPhone.Text, cbRole.SelectedValue.ToString(), dtDob.Text, txtPass.Text))
                    {
                        MessageBox.Show("Update success", title);
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Update failed", title);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void CheckField()
        {
            if (txtName.Text == "" | txtAddress.Text == "" | txtPhone.Text == "" | txtPass.Text == "")
            {
                MessageBox.Show("No information entered", "Error");
                return;
            }

            check = true;
        }
    }
}
