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
            dateTimePicker1.Value = DateTime.Now;
            ResizeListViewColumns(listView_chitiet);
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
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT TOP 1 SUBSTRING(MAHD, 7, 12) AS ExtractString FROM HOADON ORDER BY MAHD DESC", con);
            sqlDataAdapter.Fill(dataTable);

            string date = dateTimePicker1.Value.ToString("ddMMyy");
            string stt = dataTable.Rows[0][0].ToString();
            string mahd = date + stt;
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


        //CHỨC NĂNG LOAD TỔNG HOÁ ĐƠN HIỆN TẠI
        private void loadTongHD()
        {
            int tonghoadon = 0;
            for (int i = 0; i < listView_chitiet.Items.Count; i++)
            {
                int soluong = Int32.Parse(listView_chitiet.Items[i].SubItems[3].Text);
                int dongia = Int32.Parse(listView_chitiet.Items[i].SubItems[4].Text);
                tonghoadon += soluong * dongia;
            }
            txt_tonghoadon.Text = tonghoadon.ToString();
        }


        //CHỨC NĂNG LOAD TỔNG SỐ TIỀN ĐƯỢC GIẢM HIỆN TẠI
        private void loadGiaGiam()
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


        //CHỨC NĂNG LOAD SỐ TIỀN GIẢM TỪ VOUCHER
        private void loadVoucher()
        {
            DataTable DataTable = new DataTable();
            SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter("SELECT COUNT(*) FROM VOUCHER WHERE MAHD='" + txt_voucher.Text + "'", con);
            SqlDataAdapter1.Fill(DataTable);
            if (DataTable.Rows[0][0].ToString() == "1")
            {
                SqlDataAdapter SqlDataAdapter2 = new SqlDataAdapter("SELECT DONTOITHIEU,TIENGIAMLANSAU FROM VOUCHER WHERE MAHD='" + txt_voucher.Text + "'", con);
                SqlDataAdapter2.Fill(DataTable);
                int tonghoadon = Int32.Parse(txt_tonghoadon.Text);
                int tonggiamtructiep = Int32.Parse(txt_giamtructiep.Text);
                int dontoithieu = Int32.Parse(DataTable.Rows[1][1].ToString());
                if ((tonghoadon - tonggiamtructiep) > dontoithieu)
                {
                    txt_giamvoucher.Text = DataTable.Rows[1][2].ToString();
                    txt_valid.Text = "✓";
                    txt_loadvoucher.Text = "HOÁ ĐƠN ĐỦ ĐIỀU KIỆN ÁP DỤNG VOUCHER";
                }
                else
                {
                    txt_giamvoucher.Text = "0";
                    txt_valid.Text = "X";
                    txt_loadvoucher.Text = "CẦN THÊM " + (dontoithieu - (tonghoadon - tonggiamtructiep)).ToString() + " ĐỂ ÁP DỤNG VOUCHER";
                }
            }
        }


        //CHỨC NĂNG LOAD SỐ TIỀN PHẢI THU HIỆN TẠI
        private void loadThuTien()
        {
            int tonghoadon = int.Parse(txt_tonghoadon.Text);
            int giamtructiep = int.Parse(txt_giamtructiep.Text);
            int giamvoucher = int.Parse(txt_giamvoucher.Text);
            int tienphaithu = tonghoadon - giamtructiep - giamvoucher;
            
            decimal num = (decimal)tienphaithu / 1000;
            num = Math.Round(num, MidpointRounding.AwayFromZero);
            num *= 1000;
            txt_phaithu.Text = num.ToString();
            
        }


        //CHỨC NĂNG LOAD TỔNG SỐ LƯỢNG SẢN PHẨM HIỆN TẠI
        private void loadTongSP()
        {
            int tongsoluong = 0;
            for (int i = 0; i < listView_chitiet.Items.Count; i++)
            {
                int soluong = Int32.Parse(listView_chitiet.Items[i].SubItems[3].Text);
                tongsoluong += soluong;
            }
            txt_tongsp.Text = tongsoluong.ToString();
        }

        //CẬP NHẬT BẢNG THANH TOÁN KHI CÓ THÊM GIẢM VOUCHER
        private void txt_voucher_TextChanged(object sender, EventArgs e)
        {
            DataTable DataTable = new DataTable();
            SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter("SELECT COUNT(*) FROM VOUCHER WHERE MAHD='" + txt_voucher.Text + "'", con);
            SqlDataAdapter1.Fill(DataTable);
            if (DataTable.Rows[0][0].ToString() == "1")
            {
                SqlDataAdapter SqlDataAdapter2 = new SqlDataAdapter("SELECT SUDUNG,YEAR(NGAYVOUCHER),MONTH(NGAYVOUCHER),DAY(NGAYVOUCHER),DONTOITHIEU FROM VOUCHER WHERE MAHD='" + txt_voucher.Text + "'", con);
                SqlDataAdapter2.Fill(DataTable);

                DateTime ngayhethan = new DateTime(Int32.Parse(DataTable.Rows[1][0].ToString()), Int32.Parse(DataTable.Rows[1][2].ToString()), Int32.Parse(DataTable.Rows[1][3].ToString()));
                DateTime ngayhomnay = dateTimePicker1.Value.Date;
                int checkhethan = DateTime.Compare(ngayhomnay, ngayhethan);

                if (DataTable.Rows[1][1].ToString() == "True")
                {
                    txt_loadvoucher.Text = "VOUCHER ĐÃ ĐƯỢC SỬ DỤNG";
                }
                else if (checkhethan > 0)
                {
                    txt_loadvoucher.Text = "VOUCHER HẾT THỜI HẠN SỬ DỤNG";
                }
                else
                {
                    txt_voucher.ReadOnly = true;
                    loadVoucher();
                    loadThuTien();
                }
            }
            else
            {
                txt_loadvoucher.Text = "VOUCHER KHÔNG TỒN TẠI";
            }
        }


        //CẬP NHẬT BẢNG THANH TOÁN KHI CÓ THÊM GIẢM THÀNH VIÊN VIP
        private void txt_giamkhachvip_TextChanged(object sender, EventArgs e)
        {
            loadTongHD();
            loadGiaGiam();
            loadVoucher();
            loadThuTien();
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

                    ListViewItem item = new ListViewItem();
                    item.Text = mathanhtoan;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = tensp });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = tensize });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = soluong.ToString() });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = dongia.ToString() });
                    listView_chitiet.Items.Add(item);

                    txt_mathanhtoan.Text = "";
                    numUD_soluong.Value = 1;
                    loadTongSP();
                    loadTongHD();
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


        //NÚT XOÁ SẢN PHẨM KHỎI LIST THANH TOÁN
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
                loadTongSP();
                loadTongHD();
                loadGiaGiam();
                loadVoucher();
                loadThuTien();
                return;
            }
        }


        //THỰC HIỆN CÂU LỆNH SQL ĐỂ INSERT DỮ LIỆU MỚI VÀO BẢNG HOADON
        public void insertHOADON()
        {
            con.Open();
            string insertHOADON = @"INSERT INTO dbo.HOADON (MAHD,MANV,MATV) VALUES ('" + txt_mahd.Text + "','" + txt_manv.Text + "','" + txt_matv.Text + "')";
            SqlCommand cmd = new SqlCommand(insertHOADON, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }


        //THỰC HIỆN CÂU LỆNH SQL ĐỂ INSERT DỮ LIỆU MỚI VÀO BẢNG CTHD
        public void insertCTHD()
        {
            con.Open();
            string mahd = txt_mahd.Text;
            for (int i = 0; i < listView_chitiet.Items.Count; i++)
            {
                string mathanhtoan = listView_chitiet.Items[i].SubItems[0].Text;
                string masp = mathanhtoan.Substring(0, 7);
                string masize = mathanhtoan.Substring(7);
                int soluong = Int32.Parse(listView_chitiet.Items[i].SubItems[3].Text);
                int dongia = Int32.Parse(listView_chitiet.Items[i].SubItems[4].Text);

                DataTable DataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT PHANTRAMGIAM FROM SANPHAM WHERE MASP = '" + masp + "'", con);
                sqlDataAdapter.Fill(DataTable);

                float phantramgiam = float.Parse(txt_giamkhachvip.Text);
                if (DataTable.Rows[0][0].ToString() != "0")
                {
                    phantramgiam = float.Parse(DataTable.Rows[0][0].ToString());
                }

                float thanhtien = (soluong * dongia) * (1- phantramgiam);

                string insertCTHD = @"INSERT INTO dbo.CTHD (MAHD,MASP,MASIZE,SOLUONG,DONGIA,PHANTRAMGIAM,THANHTIEN) VALUES ('" + mahd + "','" + masp + "','" + masize + "'," + soluong.ToString() + "," + dongia.ToString() + "," + phantramgiam.ToString() + "," + thanhtien.ToString() + ")";
                SqlCommand cmd = new SqlCommand(insertCTHD, con);
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }


        //THỰC HIỆN CÂU LỆNH SQL ĐỂ INSERT DỮ LIỆU MỚI VÀO BẢNG BANGTHANHTOAN
        public void insertBANGTHANHTOAN()
        {
            con.Open();
            string insertBANGTHANHTOAN = @"INSERT INTO dbo.BANGTHANHTOAN (MAHD,TONGHOADON,GIAMTRUCTIEP,GIAMVOUCHER,TIENPHAITHU) VALUES ('" + txt_mahd.Text + "'," + txt_tonghoadon.Text + "," + txt_giamtructiep.Text + "," + txt_giamvoucher.Text + "," + txt_phaithu.Text + ")";
            SqlCommand cmd = new SqlCommand(insertBANGTHANHTOAN, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }


        //THỰC HIỆN CÂU LỆNH SQL ĐỂ INSERT DỮ LIỆU MỚI VÀO BẢNG VOUCHER
        public void insertVOUCHER()
        {
            con.Open();

            decimal dontoithieu = decimal.Parse(txt_phaithu.Text) / 2000;
            dontoithieu = Math.Round(dontoithieu, MidpointRounding.AwayFromZero);
            dontoithieu *= 1000;

            decimal tiengiamlansau = dontoithieu / 10;

            string insertVOUCHER = @"INSERT INTO dbo.VOUCHER (MAHD,DONTOITHIEU,TIENGIAMLANSAU) VALUES ('" + txt_mahd.Text + "'," + dontoithieu.ToString() + "," + tiengiamlansau.ToString() + ")";
            SqlCommand cmd = new SqlCommand(insertVOUCHER, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }


        //THỰC HIỆN CÂU LỆNH SQL ĐỂ UPDATE LẦN SỬ DỤNG BẢNG VOUCHER NẾU CÓ ÁP DỤNG VOUCHER
        public void updateVOUCHER()
        {
            if (txt_valid.Text == "✓")
            {
                con.Open();
                string updateVOUCHER = @"UPDATE dbo.VOUCHER SET SUDUNG = 1 WHERE MAHD ='" + txt_voucher.Text + "'";
                SqlCommand cmd = new SqlCommand(updateVOUCHER, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }


        //THỰC HIỆN CÂU LỆNH SQL ĐỂ UPDATE TIỀN TÍCH LUỸ BẢNG THANHVIEN
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

        //REFESH
        private void btn_refesh_Click(object sender, EventArgs e)
        {
            new Thanhtoan(this.manv, null).Show();
            this.Close();
        }


        //THANH TOÁN TỔNG KẾT BILL
        private void btn_thanhtoan_Click(object sender, EventArgs e)
        {
            if (txt_tongsp.Text != "0")
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
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT TOP 1 SUBSTRING(MAHD, 7, 12) AS ExtractString FROM HOADON ORDER BY MAHD DESC", con);
            sqlDataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            int a = Int32.Parse(dataTable.Rows[0][0].ToString());
            MessageBox.Show(a.ToString());
        }
        private void test_Click(object sender, EventArgs e)
        {
            show();
        }
    }
}
