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
    public partial class QL_Nhanvien : Form
    {
        public string manv = "";
        public QL_Nhanvien(string manv)
        {
            InitializeComponent();
            this.manv = manv;
        }

        private void QL_Nhanvien_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Frm_Quanli(this.manv).Show();
            this.Close();
        }
    }
}
