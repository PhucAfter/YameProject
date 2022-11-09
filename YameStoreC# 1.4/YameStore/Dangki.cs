using Microsoft.VisualBasic.ApplicationServices;
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
    public partial class Dangki : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");

        public Dangki()
        {
            InitializeComponent();
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            new login().Show();
            this.Close();
        }

        private bool check_error()
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Mã nhân viên!");
                return true;
            }
            else if (textBox4.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Họ tên!");
                return true;
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Tài khoản!");
                return true;
            }
            else if (txt_password1.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Mật khẩu!");
                return true;
            }
            else if (txt_password1.Text.Length < 6)
            {
                MessageBox.Show("Vui lòng nhập đặt Mật khẩu từ 6 kí tự trở lên!");
                return true;
            }
            else if (txt_password.Text != txt_password1.Text)
            {
                MessageBox.Show("Xác nhận Mật khậu chưa đúng!");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btn_dangki_Click(object sender, EventArgs e)
        {
            if (!check_error())
            {
                string taikhoan = txt_user.Text + textBox1.Text;
                string manv = ma_user.Text + textBox3.Text;
                DataTable dt = new DataTable();
                SqlDataAdapter sda_check_manv = new SqlDataAdapter("SELECT COUNT(*) FROM NHANVIEN WHERE MANV='" + manv + "'", con);
                SqlDataAdapter sda_check_taikhoan = new SqlDataAdapter("SELECT COUNT(*) FROM NHANVIEN WHERE TAIKHOAN='" + taikhoan + "'", con);
                sda_check_manv.Fill(dt);
                sda_check_taikhoan.Fill(dt);
           
                if (dt.Rows[0][0].ToString() != "0")
                {
                    MessageBox.Show("Mã nhân viên đã tồn tại!");
                }
                else if (dt.Rows[1][0].ToString() != "0")
                {
                    MessageBox.Show("Tài khoản đã tồn tại!");
                }
                else
                {
                    con.Open();
                    string insertUser = @"insert into dbo.USERYAME (TAIKHOAN,MATKHAU) values ('" + taikhoan + "','" + txt_password.Text + "')";
                    SqlCommand cmd = new SqlCommand(insertUser, con);
                    cmd.ExecuteNonQuery();

                    string insertNV = @"insert into dbo.NHANVIEN (MANV,TAIKHOAN,HOTEN) values ('" + manv + "','" + taikhoan + "',N'" + textBox4.Text + "')";
                    SqlCommand cmd1 = new SqlCommand(insertNV, con);
                    cmd1.ExecuteNonQuery();


                    MessageBox.Show("Đăng ký thành công");
                    con.Close();
                    new login().Show();
                    this.Close();
                    
                }
            }
        
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                txt_user.Text = "quanli";
                ma_user.Text = "QLYAME";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                txt_user.Text = "banhang";
                ma_user.Text = "BHYAME";
            }
        }

        private void txt_user_TextChanged(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
