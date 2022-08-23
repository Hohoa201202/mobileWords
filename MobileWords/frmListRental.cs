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
    public partial class frmListRental : Form
    {
        DataServices dsPhieuXuat;
        DataTable dtPhieuXuat;
        public frmListRental()
        {
            InitializeComponent();
        }

        private void frmListRental_Load(object sender, EventArgs e)
        {
            string sSql = "select r.RentalID, u.FullName, c.CustomerName, r.RentalDate, r.Discount, r.Description from tblRentals r"
                        + " inner join tblCustomers c on c.CustomerID = r.CustomerID"
                        + " inner join tblUsers u on u.UserID = r.UserID";
            dsPhieuXuat = new DataServices();
            dtPhieuXuat = dsPhieuXuat.RunQuery(sSql);
            dataGridView1.DataSource = dtPhieuXuat;
        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                verifyData.RentalID = dataGridView1.SelectedRows[0].Cells["RentalID"].Value.ToString();
                this.Close();
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            frmAEditRental.ViTri = e.RowIndex;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (verifyData.checkInputSpace(txtSearch, "Bạn cần nhập nội dung muốn tìm kiếm!") == false) return;
            if (verifyData.checkLength(txtSearch, 100, "Nội dung tìm kiếm không được quá 100 kí tự!") == false) return;

            //Truy vấn dữ liệu
            string sSql = "";
            if (rbCustomerName.Checked == true)
                sSql = "select r.RentalID, u.FullName, c.CustomerName, r.RentalDate, r.Discount, r.Description from tblRentals r"
                        + " inner join tblCustomers c on c.CustomerID = r.CustomerID"
                        + " inner join tblUsers u on u.UserID = r.UserID where c.CustomerName LIke N'%" + txtSearch.Text + "%'";
            else if (rbFullName.Checked == true)
                sSql = "select r.RentalID, u.FullName, c.CustomerName, r.RentalDate, r.Discount, r.Description from tblRentals r"
                        + " inner join tblCustomers c on c.CustomerID = r.CustomerID"
                        + " inner join tblUsers u on u.UserID = r.UserID where u.FullName LIke N'%" + txtSearch.Text + "%'";
            else if (rbRentalID.Checked == true)
                sSql = "select r.RentalID, u.FullName, c.CustomerName, r.RentalDate, r.Discount, r.Description from tblRentals r"
                        + " inner join tblCustomers c on c.CustomerID = r.CustomerID"
                        + " inner join tblUsers u on u.UserID = r.UserID where r.RentalID LIke N'%" + txtSearch.Text + "%'";
            dsPhieuXuat = new DataServices();
            dtPhieuXuat = dsPhieuXuat.RunQuery(sSql);

            //Hiển thị lên lưới
            dataGridView1.DataSource = dtPhieuXuat;
        }
    }
}
