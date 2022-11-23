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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YameStore
{
    public partial class QL_Kho : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");
        SqlDataAdapter adapter;
        SqlDataAdapter adapterall;
        DataTable dt;
        DataTable dtall;
        public string manv = "";
        public QL_Kho(string manv)
        {
            InitializeComponent();
            this.manv = manv;
        }
        private void QL_Kho_Load(object sender, EventArgs e)
        {
            showAll();
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            new Frm_Quanli(this.manv).Show();
            this.Close();
        }
        public void showData()
        {
            dt = new DataTable();
            adapter = new SqlDataAdapter("SELECT SANPHAM_SIZE.MASP,SANPHAM_SIZE.MASIZE,TENSP,TENSIZE,SOLUONG,DONGIA,DONVI,PHANTRAMGIAM FROM SANPHAM_SIZE,SANPHAM,SIZE WHERE SANPHAM_SIZE.MASP = SANPHAM.MASP AND SANPHAM_SIZE.MASIZE = SIZE.MASIZE AND SANPHAM_SIZE.MASP='" + tab1_txt_masp.Text + "'", con);
            adapter.Fill(dt);
            tab1_gridview.DataSource = dt;
        }

        public void showAll()
        {
            dtall = new DataTable();
            adapterall = new SqlDataAdapter("SELECT SANPHAM_SIZE.MASP,SANPHAM_SIZE.MASIZE,TENSP,TENSIZE,SOLUONG,DONGIA,DONVI,PHANTRAMGIAM FROM SANPHAM_SIZE,SANPHAM,SIZE WHERE SANPHAM_SIZE.MASP = SANPHAM.MASP AND SANPHAM_SIZE.MASIZE = SIZE.MASIZE", con);
            adapterall.Fill(dtall);
            tab1_gridview.DataSource = dtall;
        }

        private void tab1_btn_timkiem_Click(object sender, EventArgs e)
        {
            showData();
        }

        private void tab1_btn_hientatca_Click(object sender, EventArgs e)
        {
            tab1_txt_masp.Text = "";
            showAll();
        }

        private void tab2_btn_them_Click(object sender, EventArgs e)
        {

        }
    }
}
