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
    public partial class frmListInsurance : Form
    {
        DataServices dsPhieuBH;
        DataTable dtPhieuBH;
        public frmListInsurance()
        {
            InitializeComponent();
        }

        private void BarraTitulo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmListInsurance_Load(object sender, EventArgs e)
        {
            string sSql = "select r.InsuranceID, u.FullName, c.CustomerName, r.InsuranceDay, r.Description from tblInsurances r"
                        + " inner join tblCustomers c on c.CustomerID = r.CustomerID"
                        + " inner join tblUsers u on u.UserID = r.UserID";
            dsPhieuBH = new DataServices();
            dtPhieuBH = dsPhieuBH.RunQuery(sSql);
            dataGridView1.DataSource = dtPhieuBH;
        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                verifyData.InsuranceID = dataGridView1.SelectedRows[0].Cells["InsuranceID"].Value.ToString();
                this.Close();
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            frmAEditInsurance.ViTri = e.RowIndex;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (verifyData.checkInputSpace(txtSearch, "Bạn cần nhập nội dung muốn tìm kiếm!") == false) return;
            if (verifyData.checkLength(txtSearch, 100, "Nội dung tìm kiếm không được quá 100 kí tự!") == false) return;

            //Truy vấn dữ liệu
            string sSql = "";
            if (rbCustomerName.Checked == true)
                sSql = "select r.InsuranceID, u.FullName, c.CustomerName, r.InsuranceDay, r.Description from tblInsurances r"
                        + " inner join tblCustomers c on c.CustomerID = r.CustomerID"
                        + " inner join tblUsers u on u.UserID = r.UserID where c.CustomerName LIke N'%" + txtSearch.Text + "%'";
            else if (rbFullName.Checked == true)
                sSql = "select r.InsuranceID, u.FullName, c.CustomerName, r.InsuranceDay, r.Description from tblInsurances r"
                        + " inner join tblCustomers c on c.CustomerID = r.CustomerID"
                        + " inner join tblUsers u on u.UserID = r.UserID where u.FullName LIke N'%" + txtSearch.Text + "%'";
            else if (rbInID.Checked == true)
                sSql = "select r.InsuranceID, u.FullName, c.CustomerName, r.InsuranceDay, r.Description from tblInsurances r"
                        + " inner join tblCustomers c on c.CustomerID = r.CustomerID"
                        + " inner join tblUsers u on u.UserID = r.UserID where r.InsuranceID LIke N'%" + txtSearch.Text + "%'";
            dsPhieuBH = new DataServices();
            dtPhieuBH = dsPhieuBH.RunQuery(sSql);

            //Hiển thị lên lưới
            dataGridView1.DataSource = dtPhieuBH;
        }
    }
}
