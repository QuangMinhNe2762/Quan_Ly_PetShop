using DoAn_PetShop.Property;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_PetShop.CSDL
{
    class CSDL_OrderEntrys
    {
        DbConnect dbcon = new DbConnect();
        SqlConnection con;
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DataSet ds = new DataSet();
        SqlDataAdapter da;
        public string cmd = "select masp From SanPham";
        public string cmd1 = "select mand From NguoiDung";
        public string cmd2 = "select * From NhaCungCap";
        public CSDL_OrderEntrys()
        {
            con = dbcon.connection1();
        }
        #region method
        public List<OrderEntrys> searchOrderEntry(string txtsearch)
        {
            List<OrderEntrys> list = new List<OrderEntrys>();
            cm = new SqlCommand("select * from PhieuNhap where CONCAT(mapn,ngaynhap) LIKE '%" + txtsearch + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                OrderEntrys ord = new OrderEntrys();
                ord.mapn = dr.GetValue(0).ToString();
                ord.ngaynhap = dr.GetValue(1).ToString();
                ord.mancc = dr.GetValue(2).ToString();
                ord.mand = dr.GetValue(3).ToString();
                ord.tongtien = dr.GetValue(4).ToString();
                list.Add(ord);
            }
            dr.Close();
            con.Close();
            return list;
        }

        public List<OrderEntrys> searchOrderEntryDetails(string mapn)
        {
            List<OrderEntrys> list = new List<OrderEntrys>();
            cm = new SqlCommand("select * from CTPhieuNhap where mapn = '" + mapn + "' ", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                OrderEntrys ord = new OrderEntrys();
                ord.mapn = dr.GetValue(0).ToString();
                ord.masp = dr.GetValue(1).ToString();
                ord.soluong = dr.GetValue(2).ToString();
                ord.gianhap = dr.GetValue(3).ToString();
                ord.thanhtien = dr.GetValue(4).ToString();
                list.Add(ord);
            }
            dr.Close();
            con.Close();
            return list;
        }

        public DataTable LoadData(string cl, string tb)
        {
            da = new SqlDataAdapter(cl, con);
            da.Fill(ds, tb);

            return ds.Tables[tb];
        }

        public bool insertOder(string mapn, string mancc, string mand)
        {
            try
            {
                cm = new SqlCommand("Exec Insert_PN '" + mapn + "','" + mancc + "' ,'" + mand + "'", con);
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

        public bool insertOderDetails(string mapn, string masp, string sl, string gianhap)
        {
            try
            {
                cm = new SqlCommand("Exec Insert_CTPhieuNhap '" + mapn + "','" + masp + "' ,'" + sl + "', '" + gianhap + "'", con);
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

        public bool updateProFileOder(string mapn, string ngaynhap, string mancc, string mand)
        {
            try
            {
                cm = new SqlCommand("Set DATEFORMAT DMY UPDATE PhieuNhap SET ngaynhap = '" + ngaynhap + "', mancc='" + mancc + "', mand='" + mand + "' WHERE mapn = '" + mapn + "'", con);
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

        public void deleteOderEntry(string mapn)
        {
            cm = new SqlCommand("Exec Xoa_PN " + mapn + " ", con);
            con.Open();
            cm.ExecuteNonQuery();
            con.Close();
        }

        public void deleteOderEntryDetail(string mapn, string masp)
        {
            cm = new SqlCommand("Exec Xoa_CTPN " + mapn + ", " + masp + " ", con);
            con.Open();
            cm.ExecuteNonQuery();
            con.Close();
        }
        #endregion method
    }
}
