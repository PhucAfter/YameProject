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
    public partial class Frm_Nhanvien : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");

        public string stdUser_home { get; set; }
        public Frm_Nhanvien()
        {
            InitializeComponent();
        }

        private void Frm_Nhanvien_Load(object sender, EventArgs e)
        {
            textBox1.Text = stdUser_home;
        }

        public string getName()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT HOTEN FROM NHANVIEN WHERE TAIKHOAN='" + textBox1.Text + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Thanhtoan tt = new Thanhtoan();
            tt.stdName = getName();

            tt.Show();  //hiển thị form main
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Tracuu tc = new Tracuu();
            tc.stdUser_tracuu = textBox1.Text;

            tc.Show();  //hiển thị form main
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Baohanh frM = new Baohanh();
            frM.Show();  //hiển thị form main
            this.Close();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            login frM = new login();
            frM.Show();  //hiển thị form main
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void panel_center_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thanhvienvip frm = new Thanhvienvip();
            frm.stdUser_thanhvienvip = textBox1.Text;
            frm.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new Donhangonline().Show();
            this.Close();
        }
    }
}
