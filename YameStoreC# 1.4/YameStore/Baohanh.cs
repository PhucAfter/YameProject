using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YameStore
{
    public partial class Baohanh : Form
    {
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
        }
    }
}
