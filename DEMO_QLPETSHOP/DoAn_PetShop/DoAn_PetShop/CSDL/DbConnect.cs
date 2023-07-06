using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_PetShop.CSDL
{
    class DbConnect
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        private string con;
        public string connection2()
        {
            con = @"Data Source=DESKTOP-3CBUSTO\SQLEXPRESS;Initial Catalog=QL_DogilyPetShop;Integrated Security=True";
            return con;
        }
        public SqlConnection connection1()
        {
            return new SqlConnection(@"Data Source=DESKTOP-3CBUSTO\SQLEXPRESS;Initial Catalog=QL_DogilyPetShop;Integrated Security=True");
        }
    }
}
