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
    public partial class Thongke : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=YAME;Integrated Security=True");
        SqlDataAdapter adapterngay,adapterthang;
        DataTable dtngay,dtthang;
        public Thongke()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Frm_Quanli frm=new Frm_Quanli();
            frm.Show();
            this.Close();
        }

        private void Thongke_Load(object sender, EventArgs e)
        {
            showDoanhthuTheongay();
        }

        public void showDoanhthuTheongay()
        {
            dtngay = new DataTable();
            string getngay = dateTimePicker1.Value.ToString("MM/dd/yyyy");
            adapterngay = new SqlDataAdapter("SELECT * FROM DOANHTHU WHERE NGAY='" + getngay + "'", con);
            adapterngay.Fill(dtngay);
            dataGridView1.DataSource = dtngay;
        }
        public void showDoanhthuTheothang()
        {
            dtthang = new DataTable();
            string getthang = comboBox1.Text;
            adapterthang = new SqlDataAdapter("SELECT * FROM DOANHTHU WHERE NGAY='" + getthang + "'", con);
            adapterthang.Fill(dtthang);
            dataGridView1.DataSource = dtthang;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                dateTimePicker1.Enabled = false;
                comboBox1.Enabled = true;
                showDoanhthuTheothang();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                dateTimePicker1.Enabled = true;
                comboBox1.Enabled = false;
                showDoanhthuTheongay();
            }
        }
    }
}
