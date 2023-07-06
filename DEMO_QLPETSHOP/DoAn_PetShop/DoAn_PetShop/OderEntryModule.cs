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
    public partial class OderEntryModule : Form
    {
        CSDL_OrderEntrys csdl = new CSDL_OrderEntrys();
        string title = "Dogily PetShop Management System";
        bool check = false;
        OderEntry oderentry;
        public OderEntryModule(OderEntry form)
        {
            InitializeComponent();
            oderentry = form;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();
                if (check)
                {
                    if (MessageBox.Show("Bạn có muốn thêm khách hàng này không ?", "Add Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (csdl.insertOder(txtIDGE.Text, cbIDGoodSupplier.Text, cbIDUser.Text))
                        {
                            MessageBox.Show("Thêm dữ liệu thành công", title);
                        }
                        else
                        {
                            MessageBox.Show("Thêm dữ liệu thất bại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        this.Dispose();
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
            try
            {
                CheckField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to update this customer? ", "Edit Profile", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (csdl.updateProFileOder(txtIDGE.Text, txtDateImport.Text, cbIDGoodSupplier.Text, cbIDUser.Text))
                        {
                            MessageBox.Show("cập nhật dữ liệu thành công", title);
                        }
                        else
                        {
                            MessageBox.Show("cập nhật thất bại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtIDGE.Clear();
            txtDateImport.Clear();
            cbIDGoodSupplier.SelectedItem = null;
            cbIDUser.SelectedItem = null;
        }
        public void LoadCBB()
        {
            cbIDGoodSupplier.DataSource = csdl.LoadData(csdl.cmd2, "NhaCungCap");
            cbIDGoodSupplier.DisplayMember = "mancc";
            //cbIDGoodSupplier.ValueMember = "masp";
            cbIDGoodSupplier.DropDownStyle = ComboBoxStyle.DropDownList;

            cbIDUser.DataSource = csdl.LoadData(csdl.cmd1, "NguoiDung");
            cbIDUser.DisplayMember = "mand";
            //cbIDUser.ValueMember = "mand";
            cbIDUser.DropDownStyle = ComboBoxStyle.DropDown;
        }

        private void OderEntryModule_Load(object sender, EventArgs e)
        {
            LoadCBB();

        }

        public void CheckField()
        {
            if (cbIDGoodSupplier.Text == "" | cbIDUser.Text == "" | txtIDGE.Text == "")
            {
                MessageBox.Show("bạn chưa điền đầy đủ thông tin", "Error");
                return;
            }
            check = true;
        }
    }
}
