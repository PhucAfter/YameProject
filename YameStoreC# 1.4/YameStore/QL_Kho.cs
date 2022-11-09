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
    public partial class QL_Kho : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");
        SqlDataAdapter adapter;
        SqlDataAdapter adapterall;
        DataTable dt;
        DataTable dtall;
        public QL_Kho()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Frm_Quanli frm = new Frm_Quanli();
            frm.Show();
            this.Close();
        }
        public void showData()
        {
            dt = new DataTable();
            adapter = new SqlDataAdapter("SELECT SANPHAM_SIZE.MASP,TENSP,MASIZE,TENSIZE,DONGIA,SOLUONG,DONVI,GIAMPHANTRAM FROM SANPHAM_SIZE,SANPHAM WHERE SANPHAM_SIZE.MASP=SANPHAM.MASP AND SANPHAM_SIZE.MASP='" + textBox4.Text + "'", con);
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showData();
        }

        public void showAll()
        {
            dtall = new DataTable();
            adapterall = new SqlDataAdapter("SELECT SANPHAM_SIZE.MASP,MASIZE,TENSP,TENSIZE,SOLUONG,DONGIA,DONVI,GIAMPHANTRAM FROM SANPHAM_SIZE,SANPHAM WHERE SANPHAM_SIZE.MASP = SANPHAM.MASP", con);
            adapterall.Fill(dtall);
            dataGridView1.DataSource = dtall;
        }

        private void QL_Kho_Load(object sender, EventArgs e)
        {
            showAll();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            showAll();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }
    }
}
