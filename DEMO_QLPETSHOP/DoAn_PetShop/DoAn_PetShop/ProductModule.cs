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
    public partial class ProductModule : Form
    {
        CSDL_Product csdl_pro = new CSDL_Product();
        string title = "Dogily Petshop Management System";
        bool check = false;
        ProductForm product;
        public ProductModule(ProductForm form)
        {
            InitializeComponent();
            product = form;                                         
        }

        private void ProductModule_Load(object sender, EventArgs e)
        {
            foreach (var item in csdl_pro.tenLoai())
            {
                cbType.Items.Add(item.tenLoai);
                if (item.maLoai.Equals(csdl_pro.returnValueLoai().maLoai))
                {
                    cbType.SelectedItem = item.tenLoai;
                }
            }
            foreach (var item in csdl_pro.tenGiong())
            {
                cbCategory.Items.Add(item.tenGiong);
                if (item.maGiong.Equals(csdl_pro.returnValueGiong().maGiong))
                {
                    cbCategory.SelectedItem = item.tenGiong;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to add this product? ", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (csdl_pro.insertProDuct(txtName.Text, cbCategory.Text, cbType.Text, txtPrice.Text))
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CheckField();
            if (check)
            {
                if (MessageBox.Show("Are you sure you want to update this product? ", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (csdl_pro.editProfileProDuct(txtcode.Text, txtName.Text, cbCategory.Text, cbType.Text, int.Parse(txtQty.Text), double.Parse(txtPrice.Text)))
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

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            //chỉ cho phép nhập số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            //chỉ cho phép 1 dấu thập phân
            if (e.KeyChar == '.' && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        public void CheckField()
        {
            if (txtName.Text == "" | txtPrice.Text == "" | cbType.Text == "")
            {
                MessageBox.Show("No information entered", title);
                return;
            }

            check = true;
        }
    }
}
