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
        public QL_Nhanvien()
        {
            InitializeComponent();
        }

        private void QL_Nhanvien_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Frm_Quanli frm = new Frm_Quanli();
            frm.Show();
            this.Close();
        }
    }
}
