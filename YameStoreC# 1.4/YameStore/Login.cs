using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace YameStore
{
    public partial class login : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");
        String randomCode,chucvuhientai;
        public static String to;
        public login()
        {
            InitializeComponent();
        }
        private void login_Load(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Dangki dk = new Dangki();
            dk.Show();
            this.Hide();
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool check_null()
        {
            if (txt_user.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tài khoản");
                return true;
            }
            else if (txt_password.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            if (!check_null())
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM USERYAME WHERE TAIKHOAN='" + txt_user.Text + "' AND MATKHAU='" + txt_password.Text + "'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    DataTable dtchucvu = new DataTable();
                    SqlDataAdapter getchucvu = new SqlDataAdapter("select CHUCVU, MANV FROM USERYAME, NHANVIEN WHERE USERYAME.TAIKHOAN = NHANVIEN.TAIKHOAN AND USERYAME.TAIKHOAN ='" + txt_user.Text + "'", con);
                    getchucvu.Fill(dtchucvu);
                    string manv = dtchucvu.Rows[0][1].ToString();
                    if (dtchucvu.Rows[0][0].ToString() == "Bán hàng")
                    {
                        this.Hide();
                        new Frm_Nhanvien(manv).Show();
                    }
                    else
                    {
                        this.Hide();
                        new Frm_Quanli(manv).Show();
                    }
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                }     
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(txt_password.PasswordChar == '*')
            {
                pictureBox3.BringToFront();
                txt_password.PasswordChar = '\0';
            }    
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if(txt_password.PasswordChar == '\0')
            {
                pictureBox2.BringToFront();
                txt_password.PasswordChar = '*';
            }
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            if (txt_password.PasswordChar == '*')
            {
                pictureBox3.BringToFront();
                txt_password.PasswordChar = '\0';
            }
        }

        private void label6_Click_1(object sender, EventArgs e)
        {
            panel5.Visible = true;
            panel1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel5.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;
            panel5.Visible = false;
        }

        private bool check_error()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda_check_taikhoan = new SqlDataAdapter("SELECT COUNT(*) FROM NHANVIEN WHERE TAIKHOAN='" + textBox9.Text + "'", con);
            sda_check_taikhoan.Fill(dt);
            
            if (textBox2.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Họ tên!");
                return true;
            }
            else if (!radioButton4.Checked && !radioButton3.Checked)
            {
                MessageBox.Show("Vui lòng chọn Chức vụ của bạn!");
                return true;
            }
            else if (textBox9.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Email!");
                return true;
            }
            else if (dt.Rows[0][0].ToString() == "1")
            {
                MessageBox.Show("Email đã tồn tại!");
                return true;
            }
            else if (textBox10.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Mật khẩu!");
                return true;
            }
            else if (textBox10.Text.Length < 6)
            {
                MessageBox.Show("Vui lòng nhập đặt Mật khẩu từ 6 kí tự trở lên!");
                return true;
            }
            else if (textBox10.Text != textBox7.Text)
            {
                MessageBox.Show("Xác nhận Mật khậu chưa đúng!");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!check_error())
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel4.Visible = true;

                String from, pass, messageBody;
                Random rand = new Random();
                randomCode = (rand.Next(999999)).ToString();
                MailMessage message = new MailMessage();
                to = (textBox9.Text).ToString();
                from = "manageryame@gmail.com";
                pass = "vdpnizshdfgzksrq";
                messageBody = "Your reset code is " + randomCode;
                message.To.Add(to);
                message.From = new MailAddress(from);
                message.Body = messageBody;
                message.Subject = "Password Reseting Code";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(from, pass);

                try
                {
                    smtp.Send(message);
                    MessageBox.Show("Chúng tôi đã gửi mã xác nhận về " + textBox9.Text + ", vui lòng kiểm tra trong hộp thư của bạn (bao gồm hộp thư rác)");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btn_dangki_Click(object sender, EventArgs e)
        {
            if (randomCode == (textBox1.Text).ToString())
            {
                con.Open();
                string insertUser = @"insert into dbo.USERYAME (TAIKHOAN,MATKHAU) values ('" + textBox9.Text + "','" + textBox10.Text + "')";
                SqlCommand cmd = new SqlCommand(insertUser, con);
                cmd.ExecuteNonQuery();

                string insertNV = @"insert into dbo.NHANVIEN (MANV,TAIKHOAN,HOTEN,CHUCVU) values ('" + textBox6.Text + "','" + textBox9.Text + "',N'" + textBox2.Text + "',N'" + chucvuhientai + "')";
                SqlCommand cmd1 = new SqlCommand(insertNV, con);
                cmd1.ExecuteNonQuery();


                MessageBox.Show("Đăng ký thành công");
                con.Close();
                new login().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong Code");
            }
        }

        public void loadmanv(string chucvu)
        {
            DataTable getmatvtb = new DataTable();
            SqlDataAdapter getgetmatv = new SqlDataAdapter("SELECT TOP 1 MANV FROM NHANVIEN WHERE CHUCVU = N'"+chucvu+"' ORDER BY MANV DESC", con);
            getgetmatv.Fill(getmatvtb);
            string matv = getmatvtb.Rows[0][0].ToString();
            string stthientai = matv.Substring(6);
            string tongso0 = "";
            int index = 0;
            for (int i = 0; i < stthientai.Length; i++)
            {
                if (stthientai[i] == '0')
                {
                    tongso0 += '0';
                }
                else
                {
                    index = i;
                    break;
                }
            }
            int stt = Int32.Parse(stthientai.Substring(index));
            stt += 1;
            string stttieptheo = stt.ToString();
            if (stthientai.Length - stttieptheo.Length < tongso0.Length)
            {
                stthientai = tongso0.Substring(0, stthientai.Length - stttieptheo.Length) + stttieptheo;
            }
            else
            {
                stthientai = tongso0 + stttieptheo;
            }

            if (chucvu == "Bán hàng")
            {
                matv = "BHYAME" + stthientai;
                textBox6.Text = matv;
            }
            else
            {
                matv = "QLYAME" + stthientai;
                textBox6.Text = matv;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                loadmanv(radioButton4.Text);
                chucvuhientai = radioButton4.Text;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel5.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel4.Visible= true;
            panel5.Visible = false;
        }

        private void btn_huy_Click_1(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                loadmanv(radioButton3.Text);
                chucvuhientai = radioButton3.Text;
            }
        }
    }
}