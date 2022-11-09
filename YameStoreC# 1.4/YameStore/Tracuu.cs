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

    public partial class Tracuu : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");
        SqlDataAdapter adapter;
        DataTable dt;
        public string stdUser_tracuu { get; set; }

        public Tracuu()
        {
            InitializeComponent();
        }

        private void Tracuu_Load(object sender, EventArgs e)
        {
            textBox1.Text = stdUser_tracuu;
        }

        public void showData()
        {
            dt = new DataTable();
            adapter = new SqlDataAdapter("SELECT SANPHAM_SIZE.MASP,TENSP,TENSIZE,SOLUONG FROM SANPHAM_SIZE,SANPHAM WHERE SANPHAM_SIZE.MASP=SANPHAM.MASP AND SANPHAM_SIZE.MASP='" + textBox4.Text + "'", con);
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showData();
            /*if (textBox4.Text.Length == 7)
            {
                DataTable dt = new DataTable();
                SqlDataAdapter checkexists = new SqlDataAdapter("SELECT COUNT(*) FROM SANPHAM WHERE MASP = '" + textBox4.Text + "'", con);
                checkexists.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    showData();
                }
                else
                {
                    MessageBox.Show("Mã sản phẩm không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Mã sản phẩm không hợp lệ!");
            }*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Frm_Nhanvien nv = new Frm_Nhanvien();
            nv.stdUser_home = textBox1.Text;

            this.Hide();
            nv.Show();
        }
    }
}
