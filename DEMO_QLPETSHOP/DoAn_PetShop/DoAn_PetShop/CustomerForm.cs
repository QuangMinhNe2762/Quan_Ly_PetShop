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
    public partial class CustomerForm : Form
    {
        string title = "Dogily Petshop Management System";
        CSDL_Customer csdl_cu = new CSDL_Customer();
        public CustomerForm()
        {
            InitializeComponent();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            List<Customer> list = csdl_cu.searchCustomer(txtSearch.Text);
            for (int i = 0; i < list.Count; i++)
            {
                dgvCustomer.Rows.Add(i, list[i].makh.ToString(), list[i].tenkh.ToString(), list[i].diachi.ToString(), list[i].sdt.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModule module = new CustomerModule(this);
            module.txtma.Visible = true;
            module.txtma.Enabled = false;
            module.btnSave.Enabled = true;
            module.btnSave.Visible = true;
            module.btnUpdate.Enabled = false;
            module.btnUpdate.Visible = false;
            module.ShowDialog();
            dgvCustomer.Rows.Clear();
            List<Customer> list = csdl_cu.searchCustomer(txtSearch.Text);
            for (int i = 0; i < list.Count; i++)
            {
                dgvCustomer.Rows.Add(i + 1, list[i].makh.ToString(), list[i].tenkh.ToString(), list[i].diachi.ToString(), list[i].sdt.ToString());
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvCustomer.Rows.Clear();
            List<Customer> list = csdl_cu.searchCustomer(txtSearch.Text);
            for (int i = 0; i < list.Count; i++)
            {
                dgvCustomer.Rows.Add(i + 1, list[i].makh.ToString(), list[i].tenkh.ToString(), list[i].diachi.ToString(), list[i].sdt.ToString());
            }
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                CustomerModule module = new CustomerModule(this);
                module.txtma.Visible = true;
                module.txtma.Enabled = false;
                module.btnUpdate.Visible = true;
                module.btnUpdate.Enabled = true;
                module.btnSave.Enabled = false;
                module.btnSave.Visible = false;
                module.txtma.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtAddress.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtPhone.Text = dgvCustomer.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.btnSave.Enabled = true;
                module.ShowDialog();
                dgvCustomer.Rows.Clear();
                List<Customer> list = csdl_cu.searchCustomer(txtSearch.Text);
                for (int i = 0; i < list.Count; i++)
                {
                    dgvCustomer.Rows.Add(i + 1, list[i].makh.ToString(), list[i].tenkh.ToString(), list[i].diachi.ToString(), list[i].sdt.ToString());
                }
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this customer?", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    csdl_cu.deleteCustomer(dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString());
                    MessageBox.Show("Delete success", title, MessageBoxButtons.OK, MessageBoxIcon.Question);
                    dgvCustomer.Rows.Clear();
                    List<Customer> list = csdl_cu.searchCustomer(txtSearch.Text);
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvCustomer.Rows.Add(i + 1, list[i].makh.ToString(), list[i].tenkh.ToString(), list[i].diachi.ToString(), list[i].sdt.ToString());
                    }
                }
            }
        }
    }
}
