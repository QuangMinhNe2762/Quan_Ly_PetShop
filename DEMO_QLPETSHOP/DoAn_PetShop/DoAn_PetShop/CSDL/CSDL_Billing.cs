using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DoAn_PetShop.Property;

namespace DoAn_PetShop.CSDL
{
    class CSDL_Billing
    {
        SqlConnection con;
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        static string transo = "000000000000";
        public CSDL_Billing()
        {
            con = dbcon.connection1();
        }
        public List<billingPro> LoadProduct(string txtSearch)
        {
            List<billingPro> list = new List<billingPro>();
            cm = new SqlCommand("SELECT sp.masp,tensp, magiong, maloai, giaban FROM SanPham sp, DonGia dg WHERE sp.masp = dg.masp and CONCAT(tensp, magiong, maloai) LIKE N'%" + txtSearch + "%' AND soluongton >0", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                billingPro bp = new billingPro();
                bp.maSP = dr.GetValue(0).ToString();
                bp.tenSP = dr.GetValue(1).ToString();
                bp.maGiong = dr.GetValue(2).ToString();
                bp.maLoai = dr.GetValue(3).ToString();
                bp.giaBan = Convert.ToDouble(dr.GetValue(4).ToString());
                list.Add(bp);
            }
            dr.Close();
            con.Close();
            return list;
        }
        public string getTranso()
        {
            return transo;
        }
        public void Transo()
        {
            string sdate = DateTime.Now.ToString("yyyyMMdd");
            int count;
            string transno;
            cm = new SqlCommand("SELECT TOP 1 transno FROM HoaDon WHERE transno LIKE '" + sdate + "%' ORDER BY mahd DESC", con);
            con.Open();
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                transno = dr[0].ToString();
                count = int.Parse(transno.Substring(8, 4));
                transo = sdate + (count + 1);
            }
            else
            {
                transno = sdate + "1001";
                transo = transno;
            }
            dr.Close();
            con.Close();
        }

        public void setTranso(string n)
        {
            transo = (long.Parse(n) + 1).ToString();
        }

        public void selectProDuct(string masp, double gia, string thungan)
        {
            cm = new SqlCommand("INSERT INTO HoaDon(transno, masp, soluong, gia,thungan) VALUES('" + transo + "','" + masp + "'," + 1 + "," + gia + ",'" + thungan + "')", con);
            con.Open();
            cm.ExecuteNonQuery();
            con.Close();
        }
        public List<Billing> loadBilling()
        {
            List<Billing> list = new List<Billing>();
            cm = new SqlCommand("Exec ShowBill " + transo + " ", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                Billing b = new Billing();
                b.mahd = dr.GetValue(0).ToString();
                b.masp = dr.GetValue(1).ToString();
                b.tensp = dr.GetValue(2).ToString();
                b.soluong = Convert.ToInt32(dr.GetValue(3).ToString());
                b.gia = Convert.ToDouble(dr.GetValue(4).ToString());
                b.thanhtien = Convert.ToDouble(dr.GetValue(5).ToString());
                b.tenkh = dr.GetValue(6).ToString();
                b.thungan = dr.GetValue(7).ToString();
                list.Add(b);
            }
            dr.Close();
            con.Close();
            return list;
        }
        public List<Customer> loadCus(string txtsearch)
        {
            List<Customer> list = new List<Customer>();
            cm = new SqlCommand("SELECT makh,tenkh,dthoai FROM KhachHang WHERE tenkh LIKE '%" + txtsearch + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                Customer cus = new Customer();
                cus.makh = dr.GetValue(0).ToString();
                cus.tenkh = dr.GetValue(1).ToString();
                cus.sdt = dr.GetValue(2).ToString();
                list.Add(cus);
            }
            dr.Close();
            con.Close();
            return list;
        }
        public void selectCustomer(string makh)
        {
            cm = new SqlCommand("UPDATE HoaDon SET makh= '" + makh + "' WHERE transno='" + transo + "'", con);
            con.Open();
            cm.ExecuteNonQuery();
            con.Close();
        }
        public void deleteProFrBill(string mahd)
        {
            cm = new SqlCommand("DELETE FROM HoaDon WHERE mahd LIKE '" + mahd + "'", con);
            con.Open();
            cm.ExecuteNonQuery();
            con.Close();
        }
        public int checkPqty(string pcode)
        {
            int i = 0;
            cm = new SqlCommand("SELECT soluongton FROM SanPham WHERE masp LIKE '" + pcode + "'", con);
            con.Open();
            i = int.Parse(cm.ExecuteScalar().ToString());
            con.Close();
            return i;
        }
        public void updateQtyPro(string mahd)
        {

            cm = new SqlCommand("UPDATE HoaDon SET soluong = soluong + 1 WHERE mahd LIKE " + mahd + "", con);
            con.Open();
            cm.ExecuteNonQuery();
            con.Close();
        }
        public void decreaseQtyPro(string mahd)
        {
            cm = new SqlCommand("UPDATE HoaDon SET soluong = soluong - 1 WHERE mahd LIKE " + mahd + "", con);
            con.Open();
            cm.ExecuteNonQuery();
            con.Close();
        }

        public void UpdateSLT(int sl, string masp)
        {
            con.Open();
            cm = new SqlCommand("UPDATE SanPham SET soluongton = soluongton -" + sl + " WHERE masp LIKE '" + masp + "' ", con);
            cm.ExecuteNonQuery();
            con.Close();
        }
    }
}
