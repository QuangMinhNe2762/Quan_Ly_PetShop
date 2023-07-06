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
    public partial class OderEntryDetail : Form
    {
        CSDL_OrderEntrys csdl = new CSDL_OrderEntrys();
        string title = "Dogily PetShop Management System";
        bool check = false;
        OderEntry oderentry;
        public OderEntryDetail(OderEntry form)
        {
            InitializeComponent();
            oderentry = form;
        }
        public void LoaddCBBIDPr()
        {
            cbIDProDuct.DataSource = csdl.LoadData(csdl.cmd, "SanPham");
            cbIDProDuct.DisplayMember = "masp";
        }

        private void OderEntryDetail_Load(object sender, EventArgs e)
        {
            List<OrderEntrys> list = csdl.searchOrderEntryDetails(lblidod.Text);
            for (int i = 0; i < list.Count; i++)
            {
                dgvOderEntryDetails.Rows.Add(list[i].masp.ToString(), list[i].soluong.ToString(), list[i].gianhap.ToString(), list[i].thanhtien.ToString());
            }
            txttotals.Enabled = false;

            LoaddCBBIDPr();
        }

        private void dgvOderEntryDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvOderEntryDetails.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this customer?", "Edit Profile", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    csdl.deleteOderEntryDetail(lblidod.Text, dgvOderEntryDetails.Rows[e.RowIndex].Cells[0].Value.ToString().Trim());
                    MessageBox.Show("Delete success", title, MessageBoxButtons.OK, MessageBoxIcon.Question);
                    dgvOderEntryDetails.Rows.Clear();
                    List<OrderEntrys> list = csdl.searchOrderEntryDetails(lblidod.Text);
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvOderEntryDetails.Rows.Add(list[i].masp.ToString(), list[i].soluong.ToString(), list[i].gianhap.ToString(), list[i].thanhtien.ToString());
                    }
                }
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();
                if (check)
                {
                    if (MessageBox.Show("Bạn có muốn thêm khách hàng này không ?", "Add Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (csdl.insertOderDetails(lblidod.Text, cbIDProDuct.Text, txtquan.Text, txtprpr.Text))
                        {
                            MessageBox.Show("Thêm dữ liệu thành công", title);
                        }
                        else
                        {
                            MessageBox.Show("Thêm dữ liệu thất bại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        dgvOderEntryDetails.Rows.Clear();
                        List<OrderEntrys> list = csdl.searchOrderEntryDetails(lblidod.Text);
                        for (int i = 0; i < list.Count; i++)
                        {
                            dgvOderEntryDetails.Rows.Add(list[i].masp.ToString(), list[i].soluong.ToString(), list[i].gianhap.ToString(), list[i].thanhtien.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        public void CheckField()
        {
            if (cbIDProDuct.Text.Length == 0 | txtquan.TextLength == 0 | txtprpr.TextLength == 0)
            {
                MessageBox.Show("bạn chưa điền đầy đủ thông tin", "Error");
                return;
            }
            check = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
