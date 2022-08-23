using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileWords
{
    public partial class frmSInventory : Form
    {
        public frmSInventory()
        {
            InitializeComponent();
        }

        private void frmSInventory_Load(object sender, EventArgs e)
        {

            DataServices myDataServices = new DataServices();
            DataTable dtTonKho;
            string sSql = "exec tt_TonKho";
            dtTonKho = myDataServices.RunQuery(sSql);
            dataGridView1.DataSource = dtTonKho;
        }
    }
}
