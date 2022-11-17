using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace YameStore
{
    public partial class Baohanh : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");

        public string stdUser_baohanh { get; set; }

        public Baohanh()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Frm_Nhanvien frM = new Frm_Nhanvien();
            frM.stdUser_home = textBox6.Text;

            frM.Show();  //hiển thị form main
            this.Close();
        }

        private void Baohanh_Load(object sender, EventArgs e)
        {
            textBox6.Text = stdUser_baohanh;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter checkexists = new SqlDataAdapter("SELECT COUNT(*) FROM HOADON WHERE MAHD='" + textBox4.Text + "'", con);
            checkexists.Fill(dt);
            
            if (dt.Rows[0][0].ToString() == "1")
            {
                SqlDataAdapter getngaylap = new SqlDataAdapter("SELECT YEAR(NGAYLAP),MONTH(NGAYLAP),DAY(NGAYLAP) FROM HOADON WHERE MAHD='" + textBox4.Text + "'", con);
                getngaylap.Fill(dt);
                DateTime ngaylap = new DateTime(Int32.Parse(dt.Rows[1][0].ToString()), Int32.Parse(dt.Rows[1][1].ToString()), Int32.Parse(dt.Rows[1][2].ToString()));
                DateTime ngaybaohanh = ngaylap.AddDays(7);
                DateTime ngayhomnay = dateTimePicker1.Value.Date;
                int checkhethan = DateTime.Compare(ngayhomnay, ngaybaohanh);
                if (checkhethan > 0)
                {
                    MessageBox.Show("Hoá đơn hết hạn đổi trả");
                    return;
                }

                DataTable show = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT CONCAT(CTHD.MASP,CTHD.MASIZE) AS 'Mã Thanh Toán', TENSP, TENSIZE, SOLUONG, CTHD.DONGIA, CTHD.PHANTRAMGIAM, THANHTIEN FROM CTHD, SANPHAM, SIZE WHERE CTHD.MASP = SANPHAM.MASP AND CTHD.MASIZE = SIZE.MASIZE AND MAHD='" + textBox4.Text + "'", con);
                adapter.Fill(show);
                dataGridView1.DataSource = show;
                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                btn.HeaderText = "Đổi trả";
                btn.Text = "Chọn";
                btn.Name = "btn";
                btn.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(btn);
                textBox4.Text = "";
            }
            else
            {
                MessageBox.Show("Hoá đơn không tồn tại!");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }
    }
}
