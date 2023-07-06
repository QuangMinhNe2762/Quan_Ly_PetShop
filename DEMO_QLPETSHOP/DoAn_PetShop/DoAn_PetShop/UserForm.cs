using DoAn_PetShop.CSDL;
using DoAn_PetShop.Property;
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
    public partial class UserForm : Form
    {
        CSDL_User csdl_user = new CSDL_User();
        CSDL_Login login = new CSDL_Login();
        string title = "Dogily PetShop Management System";
        public UserForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModule module = new UserModule(this);
            module.btnUpdate.Visible = false;
            module.btnSave.Visible = true;
            module.ShowDialog();
            dgvUser.Rows.Clear();
            List<User> list = csdl_user.LoadUser(txtSearch.Text);
            for (int i = 0; i < list.Count; i++)
            {
                dgvUser.Rows.Add(i, list[i].maND, list[i].tenND, list[i].diaChi, list[i].sdt.ToString(), list[i].tenCV, list[i].ngSinh.ToShortDateString(), list[i].password);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvUser.Rows.Clear();
            List<User> list = csdl_user.LoadUser(txtSearch.Text);
            for (int i = 0; i < list.Count; i++)
            {
                dgvUser.Rows.Add(i, list[i].maND, list[i].tenND, list[i].diaChi, list[i].sdt.ToString(), list[i].tenCV, list[i].ngSinh.ToShortDateString(), list[i].password);
            }
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUser.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                UserModule module = new UserModule(this);
                module.txtma.Text = dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvUser.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtAddress.Text = dgvUser.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtPhone.Text = dgvUser.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.dtDob.Text = dgvUser.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.txtPass.Text = dgvUser.Rows[e.RowIndex].Cells[7].Value.ToString();
                module.btnSave.Visible = false;
                module.btnUpdate.Visible = true;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
                dgvUser.Rows.Clear();
                List<User> list = csdl_user.LoadUser(txtSearch.Text);
                for (int i = 0; i < list.Count; i++)
                {
                    dgvUser.Rows.Add(i, list[i].maND, list[i].tenND, list[i].diaChi, list[i].sdt.ToString(), list[i].tenCV, list[i].ngSinh.ToShortDateString(), list[i].password);
                }
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this user?", "Edit Profile", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (csdl_user.deleteUser(dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString()))
                    {
                        MessageBox.Show("Delete success", title, MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                    else
                    {
                        MessageBox.Show("Delete failed", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }                   
                }
                dgvUser.Rows.Clear();
                List<User> list = csdl_user.LoadUser(txtSearch.Text);
                for (int i = 0; i < list.Count; i++)
                {
                    dgvUser.Rows.Add(i, list[i].maND, list[i].tenND, list[i].diaChi, list[i].sdt.ToString(), list[i].tenCV, list[i].ngSinh.ToShortDateString(), list[i].password);
                }
            }
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            lbUsername.Text = login.getValueTk();
            lbRole.Text = login.getValueRole();
            List<User> list = csdl_user.LoadUser(txtSearch.Text);
            for (int i = 0; i < list.Count; i++)
            {
                dgvUser.Rows.Add(i, list[i].maND, list[i].tenND, list[i].diaChi, list[i].sdt.ToString(), list[i].tenCV, list[i].ngSinh.ToShortDateString(), list[i].password);
            }
        }
    }
}
