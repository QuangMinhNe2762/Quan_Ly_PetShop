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
    public partial class Dashboard : Form
    {
        CSDL_Dashboard csdl_dashboard = new CSDL_Dashboard();
        public Dashboard()
        {
            InitializeComponent();
        }



        private void Dashboard_Load(object sender, EventArgs e)
        {
            lblDog.Text = csdl_dashboard.ExtracData(1).ToString();
            lblCat.Text = csdl_dashboard.ExtracData(2).ToString();
            lblFish.Text = csdl_dashboard.ExtracData(3).ToString();
            lblBird.Text = csdl_dashboard.ExtracData(4).ToString();
        }
    }
}
