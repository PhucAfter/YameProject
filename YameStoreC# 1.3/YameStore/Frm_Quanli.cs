using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YameStore
{
    public partial class Frm_Quanli : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");

        public string stdUser_home { get; set; }
        public Frm_Quanli()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QL_Nhanvien frm=new QL_Nhanvien();
            frm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QL_Kho frm = new QL_Kho();
            frm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thongke frm=new Thongke();
            frm.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label_home_Click(object sender, EventArgs e)
        {

        }

        private void panel_top_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Frm_Quanli_Load(object sender, EventArgs e)
        {
            txt_nameuser.Text = stdUser_home;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            login lg = new login();
            lg.Show();
            this.Close();
        }
    }
}
