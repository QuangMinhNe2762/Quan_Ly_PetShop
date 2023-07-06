using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_PetShop.CSDL
{
    class CSDL_Dashboard
    {
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlConnection con;
        SqlDataReader dr;
        public CSDL_Dashboard()
        {
            con = dbcon.connection1();
        }
        public int ExtracData(int maLoai)
        {
            cm = new SqlCommand("SELECT ISNULL(SUM(soluongton),0) AS N'Số lượng đang có' FROM SanPham WHERE maloai=" + maLoai + "", con);
            con.Open();
            int data = int.Parse(cm.ExecuteScalar().ToString());
            con.Close();
            return data;
        }
    }
}
