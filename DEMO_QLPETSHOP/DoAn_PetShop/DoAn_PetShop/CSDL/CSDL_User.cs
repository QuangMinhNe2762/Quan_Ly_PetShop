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
    class CSDL_User
    {
        SqlConnection con;
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        static object valueSelectCb = new object();
        static int countListUser = 0;
        public CSDL_User()
        {
            con = dbcon.connection1();
        }
        public void setvalueSelectCb(object n)
        {
            valueSelectCb = n;
        }
        public object getValueSelectCb()
        {
            return valueSelectCb;
        }
        public List<User> LoadUser(string txtSearch)
        {
            List<User> list = new List<User>();
            cm = new SqlCommand("SELECT mand, tennd, dchi, dienthoai, tencv, ngsinh, password FROM NguoiDung nd, ChucVu cv WHERE nd.macv = cv.macv and CONCAT(mand, tennd, dchi, dienthoai, ngsinh, tencv) LIKE '%" + txtSearch + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                User user = new User();
                user.maND = dr.GetValue(0).ToString();
                user.tenND = dr.GetValue(1).ToString();
                user.diaChi = dr.GetValue(2).ToString();
                user.sdt = dr.GetValue(3).ToString();
                user.tenCV = dr.GetValue(4).ToString();
                user.ngSinh = Convert.ToDateTime(dr.GetValue(5).ToString());
                user.password = dr.GetValue(6).ToString();
                list.Add(user);
                countListUser++;
            }
            dr.Close();
            con.Close();
            return list;
        }
        public DataTable LoadCBB()
        {
            DataSet ds = new DataSet();
            string cl = "select * from ChucVu";

            SqlDataAdapter da = new SqlDataAdapter(cl, con);
            da.Fill(ds, "ChucVu");
            return ds.Tables["ChucVu"];
        }
        public bool deleteUser(string maND)
        {
            try
            {
                cm = new SqlCommand("DELETE FROM NguoiDung WHERE mand LIKE '" + maND + "'", con);
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
        public bool updateUser(string mand, string tennd, string dchi, string dienthoai, string macv, string ngsinh, string password)
        {
            try
            {
                cm = new SqlCommand("set dateformat DMY UPDATE NguoiDung SET tennd ='" + tennd + "', dchi='" + dchi + "', dienthoai='" + dienthoai + "',macv=" + macv + ", ngsinh='" + ngsinh + "', password='" + password + "' WHERE mand='" + mand + "'", con);
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
        public bool addUser(string tennd, string dchi, string dienthoai, string macv, string ngsinh, string password)
        {
            string mand = "ND00" + (countListUser + 1);
            try
            {
                cm = new SqlCommand("set dateformat DMY INSERT INTO NguoiDung(mand, tennd, dchi,dienthoai,macv,ngsinh,password)VALUES('" + mand + "', '" + tennd + "', '" + dchi + "', '" + dienthoai + "', " + macv + ", '" + ngsinh + "', '" + password + "')", con);
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
    }
}
