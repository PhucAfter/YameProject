using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListView = System.Windows.Forms.ListView;

namespace YameStore
{
    public partial class Thanhtoan : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");
        public string stdName { get; set; }
        int counter = 0;


        public Thanhtoan()
        {
            InitializeComponent();
        }

        private void Thanhtoan_Load(object sender, EventArgs e)
        {
            textBox2.Text = stdName;
            dateTimePicker1.Value = DateTime.Now;
            ResizeListViewColumns(listView1);
            loadmahd();
        }
        private void ResizeListViewColumns(ListView lv)
        {
            foreach (ColumnHeader column in lv.Columns)
            {
                column.Width = -2;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Frm_Nhanvien frM = new Frm_Nhanvien();
            frM.Show();  //hiển thị form main
            this.Close();
        }

        private void loadmahd()
        {
            DataTable getdatetb = new DataTable();
            SqlDataAdapter getdate = new SqlDataAdapter("SELECT TOP 1 MAHD FROM HOADON ORDER BY MAHD DESC", con);
            getdate.Fill(getdatetb);

            string date = dateTimePicker1.Value.ToString("ddMMyy");
            string mahdhientai = getdatetb.Rows[0][0].ToString();
            string mahd = date + mahdhientai.Substring(6);
            long matieptheo = long.Parse(mahd);
            matieptheo += 1;
            textBox1.Text = matieptheo.ToString();
        }

        //MATV
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "0")
            {
                textBox14.ReadOnly = true;

                DataTable infotb = new DataTable();
                SqlDataAdapter checkexists = new SqlDataAdapter("SELECT COUNT(*) FROM KHACHHANG WHERE MATV='" + textBox3.Text + "'", con);
                checkexists.Fill(infotb);

                if (infotb.Rows[0][0].ToString() == "1")
                {
                    SqlDataAdapter checkinfo = new SqlDataAdapter("SELECT HOTEN,SDT FROM KHACHHANG WHERE MATV='" + textBox3.Text + "'", con);
                    checkinfo.Fill(infotb);
                    SqlDataAdapter checkvi = new SqlDataAdapter("SELECT PHANTRAMGIAM FROM VITHANHVIEN WHERE MATV='" + textBox3.Text + "'", con);
                    checkvi.Fill(infotb);
                    textBox7.Text = infotb.Rows[1][1].ToString();
                    textBox14.Text = infotb.Rows[1][2].ToString();
                    textBox15.Text = infotb.Rows[2][3].ToString();
                }
                else
                {
                    textBox7.Text = "Không tồn tại!";
                    textBox15.Text = "0";
                }
            }
            else
            {
                textBox14.ReadOnly = false;
                textBox7.Text = "Khách Hàng Yame";
            }
        }

        //SDT
        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (textBox14.Text != "")
            {
                textBox3.ReadOnly = true;

                DataTable infotb = new DataTable();
                SqlDataAdapter checkexists = new SqlDataAdapter("SELECT COUNT(*) FROM KHACHHANG WHERE SDT='" + textBox14.Text + "'", con);
                checkexists.Fill(infotb);

                if (infotb.Rows[0][0].ToString() == "1")
                {
                    SqlDataAdapter checkinfo = new SqlDataAdapter("SELECT MATV,HOTEN FROM KHACHHANG WHERE SDT='" + textBox14.Text + "'", con);
                    checkinfo.Fill(infotb);
                    SqlDataAdapter checkvi = new SqlDataAdapter("SELECT PHANTRAMGIAM FROM VITHANHVIEN WHERE MATV='" + infotb.Rows[1][1].ToString() + "'", con);
                    checkvi.Fill(infotb);
                    textBox3.Text = infotb.Rows[1][1].ToString();
                    textBox7.Text = infotb.Rows[1][2].ToString();
                    if (float.Parse(infotb.Rows[2][3].ToString()) > 0.10)
                    {
                        textBox15.Text = "0.10";
                    }
                    else
                    {
                        textBox15.Text = infotb.Rows[2][3].ToString();
                    }
                }
                else
                {
                    textBox7.Text = "Không tồn tại!";
                    textBox15.Text = "0";
                }
            }
            else
            {
                textBox3.ReadOnly = false;
                textBox7.Text = "Khách Hàng Yame";
            }
        }

        public float sosanh(float a, float b)
        {
            return a > b ? a : b;
        }

        //THEM
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Length == 10)
            {
                DataTable dt = new DataTable();
                SqlDataAdapter checkexists = new SqlDataAdapter("SELECT COUNT(*) FROM SANPHAM_SIZE WHERE MASP = '" + textBox4.Text.Substring(0, 7) + "' AND MASIZE = '" + textBox4.Text.Substring(7) + "'", con);
                checkexists.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT TENSP,TENSIZE,DONGIA,GIAMPHANTRAM FROM SANPHAM_SIZE,SANPHAM WHERE SANPHAM_SIZE.MASP = SANPHAM.MASP AND SANPHAM_SIZE.MASP = '" + textBox4.Text.Substring(0, 7) + "' AND SANPHAM_SIZE.MASIZE = '" + textBox4.Text.Substring(7) + "'", con);
                    adapter.Fill(dt);
                    string mathanhtoan = textBox4.Text;
                    string tensp = dt.Rows[1][1].ToString();
                    string tensize = dt.Rows[1][2].ToString();
                    string soluong = numericUpDown1.Value.ToString();
                    string dongia = dt.Rows[1][3].ToString();

                    float phantram_khachhang = float.Parse(textBox15.Text);
                    float phantram_sanpham = float.Parse(dt.Rows[1][4].ToString());
                    float float_phantramgiam = sosanh(phantram_khachhang, phantram_sanpham);
                    string phantramgiam = float_phantramgiam.ToString();

                    int int_dongia = Int32.Parse(dongia);
                    int int_soluong = Int32.Parse(soluong);
                    int giagoc = int_soluong * int_dongia;
                    float float_thanhtien = giagoc * (1 - float_phantramgiam);
                    int int_thanhtien = (int)float_thanhtien;
                    string thanhtien = int_thanhtien.ToString();

                    ListViewItem item = new ListViewItem();
                    item.Text = (++counter).ToString();
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = mathanhtoan });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = tensp });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = tensize });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = soluong });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = dongia });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = phantramgiam });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = thanhtien });
                    listView1.Items.Add(item);
                    textBox4.Text = "";
                    loadTongTien();
                    loadGiaGiam();
                    loadVoucher();
                    loadThuTien();
                }
                else
                {
                    MessageBox.Show("Mã thanh toán không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Mã thanh toán không hợp lệ!");
            }
        }

        //XOA
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Chưa chọn sản phẩm cần xoá!");
                return;
            }
            else
            {
                int count = listView1.SelectedItems.Count;
                for (int i = 0; i < count; i++)
                {
                    listView1.Items.Remove(listView1.SelectedItems[0]);
                }
                loadTongTien();
                loadGiaGiam();
                loadVoucher();
                loadThuTien();
                return;
            }
        }

        public void loadThuTien()
        {
            Double tonggiagoc = Double.Parse(textBox8.Text);
            Double tonggiagiam = Double.Parse(textBox9.Text);
            Double giamvoucher = Double.Parse(textBox12.Text);
            Double tienphaithu = tonggiagoc - tonggiagiam - giamvoucher;
            Double num = tienphaithu / 1000;
            num = Math.Round(num, 2);
            string afterdot = num.ToString().Substring(num.ToString().IndexOf(".") + 1, 1);
            if (afterdot == "5")
            {
                num += 0.5;
            }
            else
            {
                num = Math.Round(num);
            }
            num *= 1000;
            textBox13.Text = num.ToString();
        }

        public void loadGiaGiam()
        {
            int tonggiagiam = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                int soluong = Int32.Parse(listView1.Items[i].SubItems[4].Text);
                int dongia = Int32.Parse(listView1.Items[i].SubItems[5].Text);
                int thanhtien = Int32.Parse(listView1.Items[i].SubItems[7].Text);
                tonggiagiam += (soluong * dongia) - thanhtien;
            }
            textBox9.Text = tonggiagiam.ToString();
        }

        public void loadTongTien()
        {
            int tonggiagoc = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                int soluong = Int32.Parse(listView1.Items[i].SubItems[4].Text);
                int dongia = Int32.Parse(listView1.Items[i].SubItems[5].Text);
                tonggiagoc += soluong * dongia;
            }
            textBox8.Text = tonggiagoc.ToString();
        }

        public void loadVoucher()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter checkexists = new SqlDataAdapter("SELECT COUNT(*) FROM VOUCHER WHERE MAHD='" + textBox6.Text + "'", con);
            checkexists.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT DONTOITHIEU,TIENGIAMLANSAU FROM VOUCHER WHERE MAHD='" + textBox6.Text + "'", con);
                adapter.Fill(dt);
                string dontoithieu = dt.Rows[1][1].ToString();
                int int_dontoithieu = Int32.Parse(dontoithieu);
                string tiengiamlansau = dt.Rows[1][2].ToString();
                int tongtiengiampt = Int32.Parse(textBox8.Text) - Int32.Parse(textBox9.Text);
                if (tongtiengiampt >= int_dontoithieu)
                {
                    textBox5.Text = "Đủ điều kiện sử dụng";
                    textBox6.ReadOnly = true;
                    textBox12.Text = tiengiamlansau;
                }
                else
                {
                    textBox5.Text = "Hoá đơn tối thiểu: " + dontoithieu + "!";
                    textBox12.Text = "0";
                }
            }
        }

        //VOUCHER
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter checkexists = new SqlDataAdapter("SELECT COUNT(*) FROM VOUCHER WHERE MAHD='" + textBox6.Text + "'", con);
            checkexists.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT SUDUNG,YEAR(NGAYHETHAN),MONTH(NGAYHETHAN),DAY(NGAYHETHAN),DONTOITHIEU,TIENGIAMLANSAU FROM VOUCHER WHERE MAHD='" + textBox6.Text + "'", con);
                adapter.Fill(dt);

                string dontoithieu = dt.Rows[1][4].ToString();
                int int_dontoithieu = Int32.Parse(dontoithieu);
                int tongtiengiampt = Int32.Parse(textBox8.Text) - Int32.Parse(textBox9.Text);

                DateTime ngayhethan = new DateTime(Int32.Parse(dt.Rows[1][0].ToString()), Int32.Parse(dt.Rows[1][2].ToString()), Int32.Parse(dt.Rows[1][3].ToString()));
                DateTime ngayhomnay = dateTimePicker1.Value.Date;
                int checkhethan = DateTime.Compare(ngayhomnay, ngayhethan);

                if (dt.Rows[1][1].ToString() == "True")
                {
                    textBox5.Text = "Voucher đã được sử dụng!";
                    textBox12.Text = "0";
                }
                else if (checkhethan > 0)
                {
                    textBox5.Text = "Voucher hết hạn sử dụng!";
                    textBox12.Text = "0";
                }
                else if (tongtiengiampt > int_dontoithieu)
                {
                    textBox5.Text = "Đủ điều kiện sử dụng";
                    textBox6.ReadOnly = true;
                    textBox12.Text = dt.Rows[1][5].ToString();
                    loadThuTien();
                }
                else
                {
                    textBox5.Text = "Hoá đơn tối thiểu: " + dt.Rows[1][4].ToString() + "!";
                    textBox6.ReadOnly = true;
                    textBox12.Text = "0";
                }
            }
            else
            {
                textBox5.Text = "không tồn tại mã hoá đơn cũ (voucher) này";
                textBox12.Text = "0";
            }
        }

        public void insertHOADON()
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sdamanv = new SqlDataAdapter("SELECT MANV FROM NHANVIEN WHERE HOTEN=N'" + textBox2.Text + "'", con);
            sdamanv.Fill(dt);

            string getmahd = textBox1.Text;
            string getngaylap = dateTimePicker1.Value.Date.ToString("MM/dd/yyyy");
            string getmanv = dt.Rows[0][0].ToString();
            string getmatv = textBox3.Text;
            string insertHOADON = @"INSERT INTO dbo.HOADON (MAHD,NGAYLAP,MANV,MATV) VALUES ('" + getmahd + "','" + getngaylap + "','" + getmanv + "','" + getmatv + "')";
            SqlCommand cmd = new SqlCommand(insertHOADON, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void insertCTHD()
        {
            con.Open();
            string getmahd = textBox1.Text;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string mathanhvien = listView1.Items[i].SubItems[1].Text;
                string getmasp = mathanhvien.Substring(0, 7);
                string getmasize = mathanhvien.Substring(7);
                string getsoluong = listView1.Items[i].SubItems[4].Text;
                string getdongia = listView1.Items[i].SubItems[5].Text;
                string getphantramgiam = listView1.Items[i].SubItems[6].Text;
                string getthanhtien = listView1.Items[i].SubItems[7].Text;
                string insertCTHD = @"INSERT INTO dbo.CTHD (MAHD,MASP,MASIZE,SOLUONG,DONGIA,PHANTRAMGIAM,THANHTIEN) VALUES ('" + getmahd + "','" + getmasp + "','" + getmasize + "'," + getsoluong + "," + getdongia + "," + getphantramgiam + "," + getthanhtien + ")";
                SqlCommand cmd = new SqlCommand(insertCTHD, con);
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }

        public void insertBANGTHANHTOAN()
        {
            con.Open();
            string getmahd = textBox1.Text;
            string gettonggiagoc = textBox8.Text;
            string getgiamtructiep = textBox9.Text;
            string getgiamvoucher = textBox12.Text;
            string gettienphaithu = textBox13.Text;
            string insertBANGTHANHTOAN = @"INSERT INTO dbo.BANGTHANHTOAN (MAHD,TONGGIAGOC,GIAMTRUCTIEP,GIAMVOUCHER,TIENPHAITHU) VALUES ('" + getmahd + "'," + gettonggiagoc + "," + getgiamtructiep + "," + getgiamvoucher + "," + gettienphaithu + ")";
            SqlCommand cmd = new SqlCommand(insertBANGTHANHTOAN, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void insertVOUCHER()
        {
            con.Open();
            string getmahd = textBox1.Text;

            DateTime thangsau = dateTimePicker1.Value.Date.AddMonths(1);
            string getngayhethan = thangsau.ToString("MM/dd/yyyy");

            Double tiengiamlansau = Double.Parse(textBox13.Text);
            tiengiamlansau = tiengiamlansau * 5 / 100;
            Double num = tiengiamlansau / 1000;
            num = Math.Round(num, 2);
            string afterdot = num.ToString().Substring(num.ToString().IndexOf(".") + 1, 1);
            if (afterdot == "5")
            {
                num += 0.5;
            }
            else
            {
                num = Math.Round(num);
            }
            num *= 1000;

            string gettiengiamlansau = num.ToString();

            Double dontoithieu = num * 10;
            string getdontoithieu = dontoithieu.ToString();

            string insertVOUCHER = @"INSERT INTO dbo.VOUCHER (MAHD,SUDUNG,NGAYHETHAN,DONTOITHIEU,TIENGIAMLANSAU) VALUES ('" + getmahd + "',0,'" + getngayhethan + "'," + getdontoithieu + "," + gettiengiamlansau + ")";
            SqlCommand cmd = new SqlCommand(insertVOUCHER, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void updateVOUCHER()
        {
            if (textBox5.Text == "Đủ điều kiện sử dụng")
            {
                con.Open();
                string updateVOUCHER = @"UPDATE dbo.VOUCHER SET SUDUNG = 1 WHERE MAHD ='" + textBox6.Text + "'";
                SqlCommand cmd = new SqlCommand(updateVOUCHER, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void updateVITHANHVIEN()
        {
            if (textBox3.Text != "0")
            {
                con.Open();
                string updateVITHANHVIEN = @"UPDATE dbo.VITHANHVIEN SET TONGTIENTICHLUY = TONGTIENTICHLUY + " + textBox13.Text + "WHERE MATV = '" + textBox3.Text + "'";
                SqlCommand cmd = new SqlCommand(updateVITHANHVIEN, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void insertDOANHTHU()
        {
            con.Open();
            string getmahd = textBox1.Text;
            string getngaylap = dateTimePicker1.Value.Date.ToString("MM/dd/yyyy");
            string getsotien = textBox13.Text;
            string insertDOANHTHU = @"INSERT INTO dbo.DOANHTHU (MAHD,NGAY,SOTIEN) VALUES ('"+ getmahd + "','" + getngaylap + "'," + getsotien + ")";
            SqlCommand cmd = new SqlCommand(insertDOANHTHU, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //REALOAD
        private void button7_Click(object sender, EventArgs e)
        {
            textBox3.ReadOnly = false;
            textBox3.Text = "0";
            textBox14.ReadOnly = false;
            textBox14.Text = "";
        }

        //XAC NHAN
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox13.Text != "0")
            {
                insertHOADON();
                insertCTHD();
                insertBANGTHANHTOAN();
                insertVOUCHER();
                updateVOUCHER();
                updateVITHANHVIEN();
                insertDOANHTHU();
                MessageBox.Show("Thanh toán hoàn tất");

                Frm_Nhanvien nv = new Frm_Nhanvien();
                nv.stdUser_home = getUserid();

                this.Hide();
                nv.Show();
            }
            else
            {
                MessageBox.Show("Chưa có sản phẩm để thanh toán!");
            }
        }
        public string getUserid()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT TAIKHOAN FROM NHANVIEN WHERE HOTEN=N'" + textBox2.Text + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Frm_Nhanvien nv = new Frm_Nhanvien();
            nv.stdUser_home = getUserid();

            this.Hide();
            nv.Show();
        }
    }
}
