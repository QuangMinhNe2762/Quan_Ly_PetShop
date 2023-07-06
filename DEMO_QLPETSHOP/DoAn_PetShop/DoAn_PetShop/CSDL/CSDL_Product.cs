using DoAn_PetShop.Property;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_PetShop.CSDL
{
    class CSDL_Product
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        static LoaiPet valueCbLoai = new LoaiPet();
        static GiongPet valueCbGiong = new GiongPet();
        static List<Product> listProDuct = new List<Product>();
        static List<LoaiPet> listLoaiPet = new List<LoaiPet>();
        static List<GiongPet> listGiongPet = new List<GiongPet>();
        SqlDataReader dr;
        public CSDL_Product()
        {
            con = dbcon.connection1();
        }
        public List<LoaiPet> tenLoai()
        {
            List<LoaiPet> list = new List<LoaiPet>();
            cm = new SqlCommand("select * from LoaiPet", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                LoaiPet lp = new LoaiPet();
                lp.maLoai = dr.GetValue(0).ToString();
                lp.tenLoai = dr.GetValue(1).ToString();
                listLoaiPet.Add(lp);
                list.Add(lp);
            }
            dr.Close();
            con.Close();
            return list;
        }
        public List<GiongPet> tenGiong()
        {
            List<GiongPet> list = new List<GiongPet>();
            cm = new SqlCommand("select * from GiongPet", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                GiongPet gp = new GiongPet();
                gp.maGiong = dr.GetValue(0).ToString();
                gp.tenGiong = dr.GetValue(1).ToString();
                list.Add(gp);
                listGiongPet.Add(gp);
            }
            dr.Close();
            con.Close();
            return list;
        }
        public void getValue(string loai, string giong)
        {
            listGiongPet.Clear();
            listLoaiPet.Clear();
            listGiongPet = tenGiong();
            listLoaiPet = tenLoai();
            valueCbGiong = listGiongPet.Find(x => x.tenGiong == giong);
            valueCbLoai = listLoaiPet.Find(x => x.tenLoai == loai);
        }
        public GiongPet returnValueGiong()
        {
            return valueCbGiong;
        }
        public LoaiPet returnValueLoai()
        {
            return valueCbLoai;
        }
        public List<Product> searchProduct(string txtsearch)
        {
            listProDuct.Clear();
            List<Product> list = new List<Product>();
            con.Close();
            cm = new SqlCommand("SELECT DISTINCT sp.masp,tensp,tengiong, tenloai, soluongton, (Select giaban From DonGia dg Where dg.masp = sp.masp) giaban FROM SanPham sp,LoaiPet p, GiongPet g WHERE sp.maloai = p.maloai and sp.magiong = g.magiong  and CONCAT(tensp, tengiong, tenloai) LIKE N'%" + txtsearch + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                Product pro = new Product();
                pro.maSP = dr.GetValue(0).ToString();
                pro.tenSP = dr.GetValue(1).ToString();
                pro.tenGiong = dr.GetValue(2).ToString();
                pro.tenLoai = dr.GetValue(3).ToString();
                pro.soLuongTon = Convert.ToInt32(dr.GetValue(4).ToString());
                pro.giaBan = Convert.ToDouble(dr.GetValue(5).ToString());
                listProDuct.Add(pro);
                list.Add(pro);
            }
            dr.Close();
            con.Close();
            return list;
        }
        public bool editProfileProDuct(string masp, string tenSP, string tengiong, string tenLoai, int soLuongTon, double giaBan)
        {
            valueCbGiong = listGiongPet.Find(x => x.tenGiong == tengiong);
            valueCbLoai = listLoaiPet.Find(x => x.tenLoai == tenLoai);
            cm = new SqlCommand("UPDATE SanPham SET tensp = '" + tenSP + "',magiong=" + valueCbGiong.maGiong + ",maloai=" + valueCbLoai.maLoai + ", soluongton = '" + soLuongTon + "' WHERE masp = '" + masp + "'", con);
            con.Open();
            cm.ExecuteNonQuery();
            cm = new SqlCommand("update DonGia set giaban=" + giaBan + " where masp='" + masp + "'", con);
            cm.ExecuteNonQuery();
            con.Close();
            return true;
        }
        public bool insertProDuct(string tensp, string tengiong, string tenloai, string giaban)
        {
            string masp = "SP00" + (listProDuct.Count + 1);
            getValue(tenloai, tengiong);
            try
            {
                cm = new SqlCommand("Exec Insert_SanPham'" + masp + "','" + tensp + "'," + valueCbGiong.maGiong + "," + valueCbLoai.maLoai + "," + giaban + "", con);
                con.Open();
                cm.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool deletePro(string masp)
        {
            try
            {
                cm = new SqlCommand("exec Xoa_SP '" + masp + "'", con);
                con.Open();
                cm.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
            
            //{
            //    //dbcon.executeQuery("DELETE FROM tbProduct WHERE pcode LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
            //    cn.Open();
            //    cm = new SqlCommand("DELETE FROM tbProduct WHERE pcode LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
            //    cm.ExecuteNonQuery();
            //    cn.Close();
            //    MessageBox.Show("Delete success", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}7
        }                                                                                                                                  
    }
}
