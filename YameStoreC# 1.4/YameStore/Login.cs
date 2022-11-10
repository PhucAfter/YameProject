using Microsoft.VisualBasic.ApplicationServices;
using System.Data;
using System.Data.SqlClient;
namespace YameStore
{
    public partial class login : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");
        public login()
        {
            InitializeComponent();
        }
            
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

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
                   if(txt_user.Text.Substring(0,7) == "banhang")
                   {
                        Frm_Nhanvien nv = new Frm_Nhanvien();
                        nv.stdUser_home = txt_user.Text;

                        this.Hide();
                        nv.Show();
                   }
                   if (txt_user.Text.Substring(0,6) == "quanli")
                   {
                        Frm_Quanli nv = new Frm_Quanli();
                        nv.stdUser_home = txt_user.Text;

                        this.Hide();
                        nv.Show();
                   }

                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                }     
            }
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void btn_dangki_Click(object sender, EventArgs e)
        {
            Dangki dk = new Dangki();
            dk.Show();
            this.Hide();
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

        }
    }
}