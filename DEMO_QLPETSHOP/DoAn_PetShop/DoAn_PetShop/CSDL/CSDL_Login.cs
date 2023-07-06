using DoAn_PetShop.CSDL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_PetShop
{
    class CSDL_Login
    {
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlConnection con;
        SqlDataReader dr;
        static string tk = string.Empty;
        static string role = string.Empty;
        public MainForm main = new MainForm();

        public CSDL_Login()
        {
            con = dbcon.connection1();
        }
        public string getValueTk()
        {
            return tk;
        }
        public string getValueRole()
        {
            return role;
        }

        public bool checklogin(string txtTK, string txtMK)
        {
            cm = new SqlCommand("SELECT tennd, tencv FROM NguoiDung nd, ChucVu cv WHERE nd.macv = cv.macv and tennd = '" + txtTK + "' and password = '" + txtMK + "'", con);
            con.Open();
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {

                role = dr.GetValue(1).ToString();
                tk = txtTK;
                if (role == "Quản trị viên")
                {
                    main.btnUser.Enabled = true;
                }
                dr.Close();
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }
    }
}
