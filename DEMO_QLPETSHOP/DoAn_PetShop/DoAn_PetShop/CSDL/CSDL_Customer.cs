using DoAn_PetShop.Property;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_PetShop.CSDL
{

    class CSDL_Customer
    {
        DbConnect dbcon = new DbConnect();
        SqlConnection con;
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public CSDL_Customer()
        {
            con = dbcon.connection1();
        }
        public List<Customer> searchCustomer(string txtsearch)
        {
            List<Customer> list = new List<Customer>();
            cm = new SqlCommand("select * from KhachHang where CONCAT(tenkh, diachi, dthoai) LIKE N'%" + txtsearch + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                Customer cu = new Customer();
                cu.makh = dr.GetValue(0).ToString();
                cu.tenkh = dr.GetValue(1).ToString();
                cu.diachi = dr.GetValue(2).ToString();
                cu.sdt = dr.GetValue(3).ToString();
                list.Add(cu);
            }
            dr.Close();
            con.Close();
            return list;
        }
        public bool updateProFileCus(string makh, string tenkh, string diachi, string sdt)
        {
            try
            {
                cm = new SqlCommand("UPDATE KhachHang SET tenKH=N'" + tenkh + "', diachi='" + diachi + "', dthoai='" + sdt + "' WHERE makh = '" + makh + "'", con);
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
        public bool insertCus(string tenkh, string diachi, string sdt)
        {
            try
            {
                string makh = "KH00" + (searchCustomer("").Count + 1);
                cm = new SqlCommand("INSERT INTO KhachHang(makh,tenkh, diachi,dthoai)VALUES('" + makh + "',N'" + tenkh + "',N'" + diachi + "' ,'" + sdt + "')", con);
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
        public void deleteCustomer(string makh)
        {
            cm = new SqlCommand("delete from KhachHang where makh='" + makh + "'", con);
            con.Open();
            cm.ExecuteNonQuery();
            con.Close();
        }
    }
}
