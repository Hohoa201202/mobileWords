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
    public partial class frmListOrder : Form
    {
        DataServices dsPhieuNhap;
        DataTable dtPhieuNhap;
        public frmListOrder()
        {
            InitializeComponent();
        }

        private void frmListOrder_Load(object sender, EventArgs e)
        {
            string sSql = "select r.OrderID, u.FullName, c.CompanyName, r.OrderDate, r.Description from tblOrders r"
                      + " inner join tblSuppliers c on c.SupplierID = r.SupplierID"
                      + " inner join tblUsers u on u.UserID = r.UserID";
            dsPhieuNhap = new DataServices();
            dtPhieuNhap = dsPhieuNhap.RunQuery(sSql);
            dataGridView1.DataSource = dtPhieuNhap;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                verifyData.OrderID = dataGridView1.SelectedRows[0].Cells["OrderID"].Value.ToString();
                this.Close();
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            frmAEditOrder.ViTri = e.RowIndex;
        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (verifyData.checkInputSpace(txtSearch, "Bạn cần nhập nội dung muốn tìm kiếm!") == false) return;
            if (verifyData.checkLength(txtSearch, 100, "Nội dung tìm kiếm không được quá 100 kí tự!") == false) return;

            //Truy vấn dữ liệu
            string sSql = "";
            if (rbCompanyName.Checked == true)
                sSql = "select r.OrderID, u.FullName, c.CompanyName, r.OrderDate, r.Description from tblOrders r"
                        + " inner join tblSuppliers c on c.SupplierID = r.SupplierID"
                        + " inner join tblUsers u on u.UserID = r.UserID where c.CompanyName LIke N'%" + txtSearch.Text + "%'";
            else if (rbFullName.Checked == true)
                sSql = "select r.OrderID, u.FullName, c.CompanyName, r.OrderDate, r.Description from tblOrders r"
                        + " inner join tblSuppliers c on c.SupplierID = r.SupplierID"
                        + " inner join tblUsers u on u.UserID = r.UserID where u.FullName LIke N'%" + txtSearch.Text + "%'";
            else if (rbOrderID.Checked == true)
                sSql = "select r.OrderID, u.FullName, c.CompanyName, r.OrderDate, r.Description from tblOrders r"
                        + " inner join tblSuppliers c on c.SupplierID = r.SupplierID"
                        + " inner join tblUsers u on u.UserID = r.UserID where r.OrderID LIke N'%" + txtSearch.Text + "%'";
            dsPhieuNhap = new DataServices();
            dtPhieuNhap = dsPhieuNhap.RunQuery(sSql);

            //Hiển thị lên lưới
            dataGridView1.DataSource = dtPhieuNhap;
        }
    }
}
