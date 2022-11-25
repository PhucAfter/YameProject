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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListView = System.Windows.Forms.ListView;

namespace YameStore
{
    public partial class Doitra : Form
    {
        SqlConnection con = new YameDatabase().getConnection();
        public string manv = "", matv = "", mahd = "", tongGT = "";
        public Doitra(string manv)
        {
            this.manv = manv;
            InitializeComponent();
        }

        private void Baohanh_Load(object sender, EventArgs e)
        {
            ResizeListViewColumns(listView1);
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

                DataTable dtmatv = new DataTable();
                SqlDataAdapter getmatv = new SqlDataAdapter("select MATV FROM HOADON WHERE MAHD = '" + textBox4.Text + "'", con);
                getmatv.Fill(dtmatv);
                this.matv = dtmatv.Rows[0][0].ToString();

                DataTable show = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT CONCAT(CTHD.MASP,CTHD.MASIZE) AS 'Mã Thanh Toán', TENSP AS 'Tên Sản Phẩm', TENSIZE AS 'Tên Size', SOLUONG AS 'Số Lượng Mua', CTHD.DONGIA AS 'Đơn Giá', CTHD.PHANTRAMGIAM AS 'Phần Trăm Giảm', THANHTIEN AS 'Thành Tiền' FROM CTHD, SANPHAM, SIZE WHERE CTHD.MASP = SANPHAM.MASP AND CTHD.MASIZE = SIZE.MASIZE AND MAHD='" + textBox4.Text + "'", con);
                adapter.Fill(show);
                dataGridView1.DataSource = show;

                DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
                checkColumn.HeaderText = "Đổi Trả";
                checkColumn.Name = "check";
                checkColumn.Width = 50;
                checkColumn.ReadOnly = false;
                DataGridViewTextBoxColumn txtColumn = new DataGridViewTextBoxColumn();
                txtColumn.HeaderText = "Số Lượng Đổi";
                txtColumn.Name = "txt";


                dataGridView1.Columns.Add(checkColumn);
                dataGridView1.Columns["check"].ReadOnly = false;

                dataGridView1.Columns.Add(txtColumn);
                dataGridView1.Columns["txt"].ReadOnly = false;

                this.mahd = textBox4.Text;
            }
            else
            {
                MessageBox.Show("Hoá đơn không tồn tại!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reloadListView();

            string nullNum = string.Empty;
            string validNum = string.Empty;
            string rangeNum = string.Empty;
            int tongtien = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["check"].Value);
                Object soluongColumn = row.Cells["txt"].Value;
                string mathanhtoan = row.Cells[0].Value.ToString();
                int soluong = Int32.Parse(row.Cells[3].Value.ToString());
                if (isSelected && soluongColumn == null)
                {
                    nullNum += mathanhtoan + ", ";
                }
                else if (isSelected && soluongColumn != null)
                {
                    int soluongdoi = validNumber(soluongColumn.ToString());
                    if (soluongdoi == 0)
                    {
                        validNum += mathanhtoan + ", ";
                    }
                    else if (soluongdoi != 0 && soluongdoi > soluong)
                    {
                        rangeNum += mathanhtoan + ", ";
                    }
                    else if (soluongdoi != 0 && soluongdoi <= soluong)
                    {
                        int dongia = Int32.Parse(row.Cells[4].Value.ToString());
                        double phantramgiam = Double.Parse(row.Cells[5].Value.ToString());
                        string masp = mathanhtoan.Substring(0, 7);
                        string masize = mathanhtoan.Substring(7);
                        double thanhtien = (soluongdoi * dongia) * (1 - phantramgiam);

                        ListViewItem item = new ListViewItem();
                        
                        item.Text = masp;
                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = masize });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = soluongdoi.ToString() });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = dongia.ToString() });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = phantramgiam.ToString() });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = thanhtien.ToString() });

                        listView1.Items.Add(item);
                        tongtien += (int)thanhtien;
                    }
                }
            }
            textBox1.Text = tongtien.ToString();
            char[] charsToTrim = { ',', ' ' };
            if (nullNum != "")
            {
                MessageBox.Show("Mã thanh toán: " + nullNum.Trim(charsToTrim) + Environment.NewLine + "Lỗi chưa nhập số lượng cần đổi!");
            }
            else if (rangeNum != "")
            {
                MessageBox.Show("Mã thanh toán: " + rangeNum.Trim(charsToTrim) + Environment.NewLine + "Lỗi số lượng cần đổi nhiều hơn số lượng trong hoá đơn!");
            }
            else if (validNum != "")
            {
                MessageBox.Show("Mã thanh toán: " + validNum.Trim(charsToTrim) + Environment.NewLine + "Lỗi số lượng cần đổi phải không phải là số!");
            }
            else if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm cần đổi!");
            }
        }

        private void ResizeListViewColumns(ListView lv)
        {
            foreach (ColumnHeader column in lv.Columns)
            {
                column.Width = -2;
            }
        }

        private void reloadListView()
        {
            int count = listView1.Items.Count;
            for (int i = 0; i < count; i++)
            {
                listView1.Items.RemoveAt(0);
            }
        }

        private int validNumber(string a)
        {
            int parsedValue;
            if (!int.TryParse(a, out parsedValue))
            {
                return 0;
            }
            else
            {
                parsedValue = Int32.Parse(a);
                return parsedValue;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm cần đổi!");
                return;
            }
            this.tongGT = textBox1.Text;
            new Thanhtoan(this.manv, this).Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Frm_Nhanvien(this.manv).Show();
            this.Close();
        }
    }
}
