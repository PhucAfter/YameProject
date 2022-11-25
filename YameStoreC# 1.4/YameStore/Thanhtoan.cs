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
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListView = System.Windows.Forms.ListView;

namespace YameStore
{
    public partial class Thanhtoan : Form
    {
        SqlConnection con = new YameDatabase().getConnection();
        public string manv = "";
        public Doitra reform;

        public Thanhtoan(string manv, Doitra reform)
        {
            InitializeComponent();
            this.manv = manv;
            this.reform = reform;
        }

        private void Thanhtoan_Load(object sender, EventArgs e)
        {
            txt_manv.Text = this.manv;
            if (this.reform != null)
            {
                button3.Visible = false;
                button4.Visible = true;
                txt_voucher.ReadOnly = true;
                txt_matv.Text = reform.matv;
                txt_mahd.Text = reform.mahd;
                txt_giamkhachvip.Text = "0";
                btn_refesh.Enabled = false;
                txt_giamvoucher.Text = reform.tongGT;
            }
            else
            {
                button3.Visible = true;
                button4.Visible = false;
                loadmahd();
            }
            dateTimePicker1.Value = DateTime.Now;
            ResizeListViewColumns(listView_chitiet);
        }

        private void ResizeListViewColumns(ListView lv)
        {
            foreach (ColumnHeader column in lv.Columns)
            {
                column.Width = -2;
            }
        }


        //LOAD DÃY SỐ HOÁ ĐƠN KẾ TIẾP
        private void loadmahd()
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT TOP 1 MAHD FROM HOADON ORDER BY MAHD DESC", con);
            sqlDataAdapter.Fill(dataTable);

            string date = dateTimePicker1.Value.ToString("ddMMyy");
            string mahd_hientai = dataTable.Rows[0][0].ToString();
            string mahd = date + mahd_hientai.Substring(6);
            long mahd_tieptheo = long.Parse(mahd);
            mahd_tieptheo += 1;
            txt_mahd.Text = mahd_tieptheo.ToString();            
        }


        //THAY ĐỔI TEXTBOX NHẬP MÃ THÀNH VIÊN SẼ LOAD TÊN KHÁCH HÀNG VÀ SỐ ĐIỆN THOẠI
        private void txt_matv_TextChanged(object sender, EventArgs e)
        {
            if (txt_matv.Text != "0")
            {
                txt_sdt.ReadOnly = true;

                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter("SELECT COUNT(*) FROM KHACHHANG WHERE MATV='" + txt_matv.Text + "'", con);
                sqlDataAdapter1.Fill(dataTable);

                if (dataTable.Rows[0][0].ToString() == "1")
                {
                    SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter("SELECT HOTEN,SDT,PHANTRAMGIAM FROM KHACHHANG,VITHANHVIEN WHERE KHACHHANG.MATV = VITHANHVIEN.MATV AND KHACHHANG.MATV='" + txt_matv.Text + "'", con);
                    sqlDataAdapter2.Fill(dataTable);
                    txt_hoten.Text = dataTable.Rows[1][1].ToString();
                    txt_sdt.Text = dataTable.Rows[1][2].ToString();
                    txt_giamkhachvip.Text = dataTable.Rows[1][3].ToString();
                }
                else
                {
                    txt_hoten.Text = "Không tồn tại!";
                    txt_giamkhachvip.Text = "0";
                }
            }
            else
            {
                txt_sdt.ReadOnly = false;
                txt_hoten.Text = "Khách Hàng Yame";
            }
        }


        //THAY ĐỔI TEXTBOX NHẬP SỐ ĐIỆN THOẠI SẼ LOAD TÊN KHÁCH HÀNG VÀ MÃ THÀNH VIÊN
        private void txt_sdt_TextChanged(object sender, EventArgs e)
        {
            if (txt_sdt.Text != "")
            {
                txt_matv.ReadOnly = true;
                
                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter("SELECT COUNT(*) FROM KHACHHANG WHERE SDT='" + txt_sdt.Text + "'", con);
                sqlDataAdapter1.Fill(dataTable);

                if (dataTable.Rows[0][0].ToString() == "1")
                {
                    SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter("SELECT KHACHHANG.MATV,HOTEN,PHANTRAMGIAM FROM KHACHHANG,VITHANHVIEN WHERE KHACHHANG.MATV = VITHANHVIEN.MATV AND SDT='" + txt_sdt.Text + "'", con);
                    sqlDataAdapter2.Fill(dataTable);
                    txt_matv.Text = dataTable.Rows[1][1].ToString();
                    txt_hoten.Text = dataTable.Rows[1][2].ToString();
                    float giamkhachvip = float.Parse(dataTable.Rows[1][3].ToString());
                    if (giamkhachvip > 0.10)
                    {
                        txt_giamkhachvip.Text = "0.10";
                    }
                    else
                    {
                        txt_giamkhachvip.Text = giamkhachvip.ToString();
                    }
                }
                else
                {
                    txt_hoten.Text = "Không tồn tại!";
                    txt_giamkhachvip.Text = "0";
                }
            }
            else
            {
                txt_matv.ReadOnly = false;
                txt_hoten.Text = "Khách Hàng Yame";
            }
        }

        public float sosanh(float a, float b)
        {
            return a > b ? a : b;
        }


        //NÚT THÊM SẢN PHẨM VÀO LIST THANH TOÁN
        private void btn_themsp_Click(object sender, EventArgs e)
        {
            if (txt_mathanhtoan.Text.Length == 10)
            {
                DataTable dataTable = new DataTable();
                SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter("SELECT COUNT(*) FROM SANPHAM_SIZE WHERE MASP = '" + txt_mathanhtoan.Text.Substring(0, 7) + "' AND MASIZE = '" + txt_mathanhtoan.Text.Substring(7) + "'", con);
                SqlDataAdapter1.Fill(dataTable);

                if (dataTable.Rows[0][0].ToString() == "1")
                {
                    SqlDataAdapter SqlDataAdapter2 = new SqlDataAdapter("SELECT TENSP, TENSIZE, DONGIA FROM SANPHAM_SIZE, SANPHAM, SIZE WHERE SANPHAM_SIZE.MASP = SANPHAM.MASP AND SANPHAM_SIZE.MASIZE = SIZE.MASIZE AND SANPHAM_SIZE.MASP = '" + txt_mathanhtoan.Text.Substring(0, 7) + "' AND SANPHAM_SIZE.MASIZE = '" + txt_mathanhtoan.Text.Substring(7) + "'", con);
                    SqlDataAdapter2.Fill(dataTable);

                    string mathanhtoan = txt_mathanhtoan.Text;
                    string tensp = dataTable.Rows[1][1].ToString();
                    string tensize = dataTable.Rows[1][2].ToString();
                    int soluong = Int32.Parse(numUD_soluong.Value.ToString());
                    int dongia = Int32.Parse(dataTable.Rows[1][3].ToString());
                    
                    int giagoc = soluong * dongia;



                    /*if (giagoc < 100000)
                    {
                        phantramgiam = 0;
                        thanhtien = giagoc;
                        float phantramgiam = float.Parse(txt_giamkhachvip.Text);
                        if (dataTable.Rows[1][4].ToString() != "0")
                        {
                            phantramgiam = float.Parse(dataTable.Rows[1][4].ToString());
                        }
                        float float_thanhtien = giagoc * (1 - phantramgiam);
                        int thanhtien = (int)float_thanhtien;
                    }*/

                    ListViewItem item = new ListViewItem();
                    item.Text = mathanhtoan;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = tensp });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = tensize });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = soluong.ToString() });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = dongia.ToString() });
                    //item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = phantramgiam.ToString() });
                    //item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = thanhtien.ToString() });
                    listView_chitiet.Items.Add(item);
                    txt_mathanhtoan.Text = "";
                    numUD_soluong.Value = 1;
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


        public void loadTongTien()
        {
            int tonggiagoc = 0;
            for (int i = 0; i < listView_chitiet.Items.Count; i++)
            {
                int soluong = Int32.Parse(listView_chitiet.Items[i].SubItems[3].Text);
                int dongia = Int32.Parse(listView_chitiet.Items[i].SubItems[4].Text);
                tonggiagoc += soluong * dongia;
            }
            txt_tonghoadon.Text = tonggiagoc.ToString();
        }


        public void loadGiaGiam()
        {
            int tonggiamSP = 0;
            int tonggiamVIP = 0;
            
            for (int i = 0; i < listView_chitiet.Items.Count; i++)
            {
                string mathanhtoan = listView_chitiet.Items[i].SubItems[0].Text;
                string masp = mathanhtoan.Substring(0, 7);
                int soluong = Int32.Parse(listView_chitiet.Items[i].SubItems[3].Text);
                int dongia = Int32.Parse(listView_chitiet.Items[i].SubItems[4].Text);

                DataTable DataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT PHANTRAMGIAM FROM SANPHAM WHERE MASP = '" + masp + "'", con);
                sqlDataAdapter.Fill(DataTable);
                int giagoc = soluong * dongia;

                if (DataTable.Rows[0][0].ToString() != "0")
                {
                    float giamSP = giagoc * float.Parse(DataTable.Rows[0][0].ToString());
                    tonggiamSP += (int)giamSP;
                    continue;
                }

                float giamVIP = giagoc * float.Parse(txt_giamkhachvip.Text);
                tonggiamVIP += (int)giamVIP;
            }
            if (Int32.Parse(txt_tonghoadon.Text) > 100000)
            {
                txt_giamtructiep.Text = (tonggiamSP + tonggiamVIP).ToString();
            }
            else
            {
                txt_giamtructiep.Text = tonggiamSP.ToString();
            }
        }


        public void loadThuTien()
        {
            Double tonghoadon = Double.Parse(txt_tonghoadon.Text);
            Double giamtructiep = Double.Parse(txt_giamtructiep.Text);
            Double giamvoucher = Double.Parse(txt_giamvoucher.Text);
            Double tienphaithu = tonghoadon - giamtructiep - giamvoucher;
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
            txt_phaithu.Text = num.ToString();
        }


        public void loadVoucher()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter checkexists = new SqlDataAdapter("SELECT COUNT(*) FROM VOUCHER WHERE MAHD='" + txt_voucher.Text + "'", con);
            checkexists.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT DONTOITHIEU,TIENGIAMLANSAU FROM VOUCHER WHERE MAHD='" + txt_voucher.Text + "'", con);
                adapter.Fill(dt);
                string dontoithieu = dt.Rows[1][1].ToString();
                int int_dontoithieu = Int32.Parse(dontoithieu);
                string tiengiamlansau = dt.Rows[1][2].ToString();
                int tongtiengiampt = Int32.Parse(txt_tonghoadon.Text) - Int32.Parse(txt_giamtructiep.Text);
                if (tongtiengiampt >= int_dontoithieu)
                {
                    txt_loadvoucher.Text = "Đủ điều kiện sử dụng";
                    txt_voucher.ReadOnly = true;
                    txt_giamvoucher.Text = tiengiamlansau;
                }
                else
                {
                    txt_loadvoucher.Text = "Hoá đơn tối thiểu: " + dontoithieu + "!";
                    txt_giamvoucher.Text = "0";
                }
            }
        }


        //XOA
        private void btn_xoasp_Click(object sender, EventArgs e)
        {
            if (listView_chitiet.SelectedItems.Count == 0)
            {
                MessageBox.Show("Chưa chọn sản phẩm cần xoá!");
                return;
            }
            else
            {
                int count = listView_chitiet.SelectedItems.Count;
                for (int i = 0; i < count; i++)
                {
                    listView_chitiet.Items.Remove(listView_chitiet.SelectedItems[0]);
                }
                loadTongTien();
                loadGiaGiam();
                loadVoucher();
                loadThuTien();
                return;
            }
        }


        //VOUCHER
        private void txt_voucher_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter checkexists = new SqlDataAdapter("SELECT COUNT(*) FROM VOUCHER WHERE MAHD='" + txt_voucher.Text + "'", con);
            checkexists.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT SUDUNG,YEAR(NGAYHETHAN),MONTH(NGAYHETHAN),DAY(NGAYHETHAN),DONTOITHIEU,TIENGIAMLANSAU FROM VOUCHER WHERE MAHD='" + txt_voucher.Text + "'", con);
                adapter.Fill(dt);

                string dontoithieu = dt.Rows[1][4].ToString();
                int int_dontoithieu = Int32.Parse(dontoithieu);
                int tongtiengiampt = Int32.Parse(txt_tonghoadon.Text) - Int32.Parse(txt_giamtructiep.Text);

                DateTime ngayhethan = new DateTime(Int32.Parse(dt.Rows[1][0].ToString()), Int32.Parse(dt.Rows[1][2].ToString()), Int32.Parse(dt.Rows[1][3].ToString()));
                DateTime ngayhomnay = dateTimePicker1.Value.Date;
                int checkhethan = DateTime.Compare(ngayhomnay, ngayhethan);

                if (dt.Rows[1][1].ToString() == "True")
                {
                    txt_loadvoucher.Text = "Voucher đã được sử dụng!";
                    txt_giamvoucher.Text = "0";
                }
                else if (checkhethan > 0)
                {
                    txt_loadvoucher.Text = "Voucher hết hạn sử dụng!";
                    txt_giamvoucher.Text = "0";
                }
                else if (tongtiengiampt > int_dontoithieu)
                {
                    txt_loadvoucher.Text = "Đủ điều kiện sử dụng";
                    txt_voucher.ReadOnly = true;
                    txt_giamvoucher.Text = dt.Rows[1][5].ToString();
                    loadThuTien();
                }
                else
                {
                    txt_loadvoucher.Text = "Hoá đơn tối thiểu: " + dt.Rows[1][4].ToString() + "!";
                    txt_voucher.ReadOnly = true;
                    txt_giamvoucher.Text = "0";
                }
            }
            else
            {
                txt_loadvoucher.Text = "không tồn tại mã hoá đơn cũ (voucher) này";
                txt_giamvoucher.Text = "0";
            }
        }

        public void insertHOADON()
        {
            con.Open();
            string insertHOADON = @"INSERT INTO dbo.HOADON (MAHD,MANV,MATV) VALUES ('" + txt_mahd.Text + "','" + txt_manv.Text + "','" + txt_matv.Text + "')";
            SqlCommand cmd = new SqlCommand(insertHOADON, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void insertCTHD()
        {
            /*con.Open();
            string getmahd = txt_mahd.Text;
            for (int i = 0; i < listView_chitiet.Items.Count; i++)
            {
                string mathanhtoan = listView_chitiet.Items[i].SubItems[0].Text;
                string getmasp = mathanhtoan.Substring(0, 7);
                string getmasize = mathanhtoan.Substring(7);
                string getsoluong = listView_chitiet.Items[i].SubItems[3].Text;
                string getdongia = listView_chitiet.Items[i].SubItems[4].Text;
                string getphantramgiam = listView_chitiet.Items[i].SubItems[5].Text;
                string getthanhtien = listView_chitiet.Items[i].SubItems[6].Text;
                string insertCTHD = @"INSERT INTO dbo.CTHD (MAHD,MASP,MASIZE,SOLUONG,DONGIA,PHANTRAMGIAM,THANHTIEN) VALUES ('" + getmahd + "','" + getmasp + "','" + getmasize + "'," + getsoluong + "," + getdongia + "," + getphantramgiam + "," + getthanhtien + ")";
                SqlCommand cmd = new SqlCommand(insertCTHD, con);
                cmd.ExecuteNonQuery();
            }
            con.Close();*/
        }

        public void insertBANGTHANHTOAN()
        {
            con.Open();
            string insertBANGTHANHTOAN = @"INSERT INTO dbo.BANGTHANHTOAN (MAHD,TONGHOADON,GIAMTRUCTIEP,GIAMVOUCHER,TIENPHAITHU) VALUES ('" + txt_mahd.Text + "'," + txt_tonghoadon.Text + "," + txt_giamtructiep.Text + "," + txt_giamvoucher.Text + "," + txt_phaithu.Text + ")";
            SqlCommand cmd = new SqlCommand(insertBANGTHANHTOAN, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void insertVOUCHER()
        {
            con.Open();

            DateTime thangsau = dateTimePicker1.Value.Date.AddMonths(1);
            string getngayhethan = thangsau.ToString("MM/dd/yyyy");

            Double tiengiamlansau = Double.Parse(txt_phaithu.Text);
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

            string insertVOUCHER = @"INSERT INTO dbo.VOUCHER (MAHD,NGAYHETHAN,DONTOITHIEU,TIENGIAMLANSAU) VALUES ('" + txt_mahd.Text + "','" + getngayhethan + "'," + getdontoithieu + "," + gettiengiamlansau + ")";
            SqlCommand cmd = new SqlCommand(insertVOUCHER, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void updateVOUCHER()
        {
            if (txt_loadvoucher.Text == "Đủ điều kiện sử dụng")
            {
                con.Open();
                string updateVOUCHER = @"UPDATE dbo.VOUCHER SET SUDUNG = 1 WHERE MAHD ='" + txt_voucher.Text + "'";
                SqlCommand cmd = new SqlCommand(updateVOUCHER, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void updateVITHANHVIEN()
        {
            if (txt_matv.Text != "0")
            {
                con.Open();
                string updateVITHANHVIEN = @"UPDATE dbo.VITHANHVIEN SET TONGTIENTICHLUY = TONGTIENTICHLUY + " + txt_phaithu.Text + "WHERE MATV = '" + txt_matv.Text + "'";
                SqlCommand cmd = new SqlCommand(updateVITHANHVIEN, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //REAFESH
        private void btn_refesh_Click(object sender, EventArgs e)
        {
            new Thanhtoan(this.manv, null).Show();
            this.Close();
        }

        //THANH TOÁN TỔNG KẾT BILL
        private void btn_thanhtoan_Click(object sender, EventArgs e)
        {
            if (txt_phaithu.Text != "0")
            {
                insertHOADON();
                insertCTHD();
                insertBANGTHANHTOAN();
                insertVOUCHER();
                updateVOUCHER();
                updateVITHANHVIEN();
                MessageBox.Show("Thanh toán hoàn tất");

                new Frm_Nhanvien(this.manv).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Chưa có sản phẩm để thanh toán!");
            }
        }


        //THOÁT CHỨC NĂNG THANH TOÁN
        private void button3_Click(object sender, EventArgs e)
        {
            new Frm_Nhanvien(this.manv).Show();
            this.Close();
        }

        //TRỞ VỀ FORM ĐỔI TRẢ
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            this.reform.Show();
        }

        //TEST
        private void show()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter("SELECT COUNT(*) FROM KHACHHANG WHERE MATV='" + txt_matv.Text + "'", con);
            sqlDataAdapter1.Fill(dt);
            SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter("SELECT KHACHHANG.MATV, HOTEN, PHANTRAMGIAM FROM KHACHHANG, VITHANHVIEN WHERE KHACHHANG.MATV = VITHANHVIEN.MATV AND SDT = '" + txt_sdt.Text + "'", con);
            sqlDataAdapter2.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void test_Click(object sender, EventArgs e)
        {
            show();
        }

    }
}
