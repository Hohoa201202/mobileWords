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
    public partial class frmAEditInsurance : Form
    {
        //Dùng Insert bảng tblInsurances
        private DataServices dsInsurances;
        private DataTable dtInsurances;

        //Dùng Insert bảng tblInsurancesDetails
        private DataServices dsInsuranceDetails;
        private DataTable dtInsuranceDetails;

        //Dùng truy vấn lên lưới DataGridView
        private DataServices dsInsurances1;
        private DataTable dtInsurances1;
        public static int ViTri = 0; //Biến lưu vị trí phiếu xuất
        private bool modeNew;
        private string _InsuranceID;
        public frmAEditInsurance()
        {
            InitializeComponent();
        }

        private void frmAEditInsurance_Load(object sender, EventArgs e)
        {
            // 1. Chuyển dữ liệu vào cboCustomerName
            // 1.1. Truy vấn từ bảng tblUsers
            string sSql = "SELECT * FROM tblCustomers c inner join tblRentals r on r.CustomerID = c.CustomerID Order By CustomerName";
            DataServices dsCustomer = new DataServices();
            DataTable dtCustomer = dsCustomer.RunQuery(sSql);
            // 1.2. Chuyển dữ liệu vào cboCustomerName
            cboCustomerID.DataSource = dtCustomer;
            cboCustomerID.DisplayMember = "CustomerName";
            cboCustomerID.ValueMember = "CustomerID";

            // 2. Chuyển dữ liệu vào cboFullName
            // 2.1. Truy vấn từ bảng tblUsers
            sSql = "SELECT * FROM tblUsers Order By FullName";
            DataServices dsUser = new DataServices();
            DataTable dtUser = dsUser.RunQuery(sSql);
            // 2.2. Chuyển dữ liệu vào cboFullName
            cboUserID.DataSource = dtUser;
            cboUserID.DisplayMember = "FullName";
            cboUserID.ValueMember = "UserID";

            //Nạp danh sách phiếu xuất
            NapDanhSachPhieuXuat();
            if (dtInsurances.Rows.Count > 0)
            {
                ViTri = 0;
                DiChuyenDenPhieuI(ViTri);
                Display(txtInsuranceID.Text); //Hiển thị chi tiết phiếu xuất lên lưới
            }
            else
            {
                MessageBox.Show("Chưa có phiếu bảo hành nào", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //thiết lập các nút ấn
            SetControls(false);
        }

        void NapDanhSachPhieuXuat()
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblInsurances order by InsuranceID";
            dsInsurances = new DataServices();
            dtInsurances = dsInsurances.RunQuery(sSql);
        }

        void DiChuyenDenPhieuI(int I)
        {
            txtInsuranceID.Text = dtInsurances.Rows[I]["InsuranceID"].ToString();
            dtmInsuranceDay.Value = Convert.ToDateTime(dtInsurances.Rows[I]["InsuranceDay"]);
            cboCustomerID.SelectedValue = dtInsurances.Rows[I]["CustomerID"].ToString();
            cboUserID.SelectedValue = dtInsurances.Rows[I]["UserID"].ToString();
            txtDescription.Text = dtInsurances.Rows[I]["Description"].ToString();
        }

        private void Display(string @InsuranceID) //Nạp chi tiết phiếu xuất
        {
            //Nạp lại Mã sản phẩm
            //Chuyển dữ liệu vào ProductID tại DataGridView
            //Truy vấn từ bảng tblProducts 
            string sSql = "SELECT p.ProductID, p.ProductName FROM tblProducts p "
                           + "inner join tblRentalDetails rd on rd.ProductID = p.ProductID "
                           + "inner join tblRentals r on r.RentalID = rd.RentalID "
                           + "inner join tblCustomers c on c.CustomerID = r.CustomerID "
                           + "where c.CustomerName = N'" + cboCustomerID.Text + "' "
                           + "Order By p.ProductName";
            DataServices dsProduct = new DataServices();
            DataTable dtProduct = dsProduct.RunQuery(sSql);
            //Chuyển dữ liệu vào ProductID tại DataGridView
            ProductID.DataSource = dtProduct;
            ProductID.DisplayMember = "ProductName";
            ProductID.ValueMember = "ProductID";

            //Truy vấn chi tiết phiếu bảo hành lên lưới.
            string sSql1 = "Exec tt_Nap_HDBaoHanh " + @InsuranceID;
            dsInsurances1 = new DataServices();
            dtInsurances1 = dsInsurances1.RunQuery(sSql1);

            //Truy vấn chi tiết phiếu xuất dùng Insert
            string sSql2 = "select * from tblInsuranceDetails where InsuranceID = '" + @InsuranceID + "'";
            dsInsuranceDetails = new DataServices();
            dtInsuranceDetails = dsInsuranceDetails.RunQuery(sSql2);
            dataGridView1.DataSource = dtInsuranceDetails;
        }

        void NapMotPhieuXuat(string @InsuranceID)
        {
            string sSql1 = "SELECT * FROM tblInsurances where InsuranceID = '" + @InsuranceID + "'";
            DataServices dsPhieuXuat = new DataServices();
            DataTable dtPhieuXuat = dsPhieuXuat.RunQuery(sSql1);
            dataGridView1.DataSource = dtInsuranceDetails;
            if (dtInsurances1.Rows.Count > 0)
            {
                txtInsuranceID.Text = dtPhieuXuat.Rows[0]["InsuranceID"].ToString();
                dtmInsuranceDay.Value = Convert.ToDateTime(dtPhieuXuat.Rows[0]["InsuranceDay"]);
                cboCustomerID.SelectedValue = dtPhieuXuat.Rows[0]["CustomerID"].ToString();
                cboUserID.SelectedValue = dtPhieuXuat.Rows[0]["UserID"].ToString();
                txtDescription.Text = dtPhieuXuat.Rows[0]["Description"].ToString();
            }
        }

        private void SetControls(bool edit)
        {
            txtInsuranceID.Enabled = edit;
            cboCustomerID.Enabled = edit;
            cboUserID.Enabled = edit;
            dtmInsuranceDay.Enabled = edit;
            txtDescription.Enabled = edit;

            // Thiết lập trạng thái các button
            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;

            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu nhập vào không được rỗng từ lớp chung verifyData.cs
            if (verifyData.checkInputSpace(txtInsuranceID, "Mã phiếu bảo hành không được để trống!") == false) return;
            if (verifyData.checkLength(txtInsuranceID, 10, "Mã phiếu bảo hành không được vượt quá 10 kí tự!") == false) return;

            if (verifyData.checkLength(txtDescription, 250, "Mô tả thêm không được vượt quá 250 kí tự!") == false) return;

            string sSql;
            DataServices dsSearch = new DataServices();
            DataTable dtSearch;
            //Kiểm tra dữ liệu trùng khi <thêm mới> hoặc <sửa> phiếu bảo hành
            if (((modeNew == true) || (modeNew == false)) && (txtInsuranceID.Text != _InsuranceID))
            {
                sSql = "Select InsuranceID from tblInsurances Where InsuranceID = N'" + txtInsuranceID.Text + "'";
                dtSearch = dsSearch.RunQuery(sSql);
                if (dtSearch.Rows.Count > 0)
                {
                    MessageBox.Show("Mã phiếu bảo hành đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtInsuranceID.Focus();
                    return;
                }
            }

            if (modeNew == true)
            {
                //1. Nhập dữ liệu vào bảng Rentals 
                DataRow myDataRowRental = dtInsurances.NewRow();
                myDataRowRental["InsuranceID"] = txtInsuranceID.Text;
                myDataRowRental["UserID"] = cboUserID.SelectedValue;
                myDataRowRental["CustomerID"] = cboCustomerID.SelectedValue;
                myDataRowRental["InsuranceDay"] = dtmInsuranceDay.Text;
                myDataRowRental["Description"] = txtDescription.Text;
                dtInsurances.Rows.Add(myDataRowRental);
                dsInsurances.Update(dtInsurances);
                MessageBox.Show("Thêm phiếu bảo hành thành công :)", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //Sửa dữ liệu
                sSql = "exec tt_Update_HDBaoHanh " + _InsuranceID + ", " + cboUserID.SelectedValue + ", " + cboCustomerID.SelectedValue + ", '" + dtmInsuranceDay.Text + "', N'" + txtDescription.Text + "'";
                DataServices dsUpdate = new DataServices();
                dsUpdate.RunQuery(sSql);

                MessageBox.Show("Phiếu bảo hành " + _InsuranceID.Trim() + " đã được chỉnh sửa :)", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            groupBox1.Enabled = true;

            //Hiển thị lại dữ liệu sau khi thêm mới hoặc sửa
            NapDanhSachPhieuXuat();
            Display(txtInsuranceID.Text);
            SetControls(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            DiChuyenDenPhieuI(ViTri);
            Display(verifyData.InsuranceID); //Hiển thị chi tiết phiếu bảo hành lên lưới
            SetControls(false);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            verifyData.InsuranceID = txtInsuranceID.Text;

            groupBox1.Enabled = false;
            //Xóa trắng các textBox
            txtInsuranceID.Clear();
            txtDescription.Clear();

            SetControls(true);
            // Chuyển con trỏ về
            txtInsuranceID.Focus();
            modeNew = true;

            dataGridView1.ClearSelection();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _InsuranceID = txtInsuranceID.Text;
            groupBox1.Enabled = false;
            SetControls(true);
            //chuyển con trò vể
            txtDescription.Focus();
            modeNew = false;
            txtInsuranceID.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Hiển thị hộp thoại xác nhận chắc chắn xóa không?
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xoá dữ liệu không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            //Chạy thủ tục delete
            string sSql = "EXEC DeleteInsurance '" + txtInsuranceID.Text + "'";
            DataServices dsDELETE = new DataServices();
            dsDELETE.ExecuteNonQuery(sSql);
            MessageBox.Show("Phiếu bảo hành đã bị xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Hiển thị lại dữ liệu sau khi xóa
            NapDanhSachPhieuXuat();
            DiChuyenDenPhieuI(0);
            Display(txtInsuranceID.Text);
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (ViTri > 0)
            {
                ViTri -= 1;
                DiChuyenDenPhieuI(ViTri);
                Display(txtInsuranceID.Text); //Hiển thị chi tiết phiếu xuất lên lưới
            }
            else
            {
                ViTri = dtInsurances.Rows.Count - 1;
                DiChuyenDenPhieuI(ViTri);
                Display(txtInsuranceID.Text); //Hiển thị chi tiết phiếu xuất lên lưới
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ViTri < dtInsurances.Rows.Count - 1)
            {
                ViTri += 1;
                DiChuyenDenPhieuI(ViTri);
                Display(txtInsuranceID.Text); //Hiển thị chi tiết phiếu xuất lên lưới
            }
            else
            {
                ViTri = 0;
                DiChuyenDenPhieuI(ViTri);
                Display(txtInsuranceID.Text); //Hiển thị chi tiết phiếu xuất lên lưới
            }
        }

        private void btnSelectInsuranceID_Click(object sender, EventArgs e)
        {
            frmListInsurance frm = new frmListInsurance();
            frm.ShowDialog();
            if (verifyData.InsuranceID != "")
            {
                NapMotPhieuXuat(verifyData.InsuranceID);
                Display(verifyData.InsuranceID);
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows[e.RowIndex].Cells.Count; i++)
            {
                dataGridView1[i, e.RowIndex].Style.BackColor = Color.Yellow;
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            dataGridView1.Rows[e.Row.Index - 1].Cells["InsuranceID"].Value = txtInsuranceID.Text;

        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Nhập dữ liệu vào bảng tblInsuranceDetails
                dsInsuranceDetails.Update(dtInsuranceDetails);
                string sSql1 = "Exec tt_Nap_HDBaoHanh " + txtInsuranceID.Text;
                dsInsurances1 = new DataServices();
                dtInsurances1 = dsInsurances1.RunQuery(sSql1);
            }
            catch { }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            frmReportInsurance.@InsuranceID = txtInsuranceID.Text;
            frmReportInsurance frm = new frmReportInsurance();
            frm.ShowDialog();
        }
    }
}
