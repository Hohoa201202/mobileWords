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
    public partial class frmAEditRental : Form
    {
        //Dùng Insert bảng tblRentals
        private DataServices dsRental;
        private DataTable dtRental;

        //Dùng Insert bảng tblRentalDetails
        private DataServices dsRentalDetails;
        private DataTable dtRentalDetails;

        //Dùng truy vấn lên lưới DataGridView
        private DataServices dsRental1;
        private DataTable dtRental1;
        public static int ViTri = 0; //Biến lưu vị trí phiếu xuất
        private double GiamGia;//Biến lưu Giảm Giá
        private bool modeNew;
        private string _RentalID;
        public frmAEditRental()
        {
            InitializeComponent();
        }

        private void frmAEditRental_Load(object sender, EventArgs e)
        {
            // 1. Chuyển dữ liệu vào cboCustomerName
            // 1.1. Truy vấn từ bảng tblUsers
            string sSql = "SELECT * FROM tblCustomers Order By CustomerName";
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
            if (dtRental.Rows.Count > 0)
            {
                ViTri = 0;
                DiChuyenDenPhieuI(ViTri);
                Display(txtRentalID.Text); //Hiển thị chi tiết phiếu xuất lên lưới
            }
            else
            {
                MessageBox.Show("Chưa có phiếu xuất nào", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //thiết lập các nút ấn
            SetControls(false);
        }
        void NapDanhSachPhieuXuat()
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblRentals order by RentalID";
            dsRental = new DataServices();
            dtRental = dsRental.RunQuery(sSql);
        }

        void DiChuyenDenPhieuI(int I)
        {
            txtRentalID.Text = dtRental.Rows[I]["RentalID"].ToString();
            dtmRentalDate.Value = Convert.ToDateTime(dtRental.Rows[I]["RentalDate"]);
            cboCustomerID.SelectedValue = dtRental.Rows[I]["CustomerID"].ToString();
            cboUserID.SelectedValue = dtRental.Rows[I]["UserID"].ToString();
            txtDiscount.Text = dtRental.Rows[I]["Discount"].ToString();
            GiamGia = Convert.ToDouble(txtDiscount.Text);
            txtDescription.Text = dtRental.Rows[I]["Description"].ToString();
        }

        private void Display(string @RentalID) //Nạp chi tiết phiếu xuất
        {
            //Nạp lại Mã sản phẩm
            //Chuyển dữ liệu vào ProductID tại DataGridView
            //Truy vấn từ bảng tblProducts 
            string sSql = "SELECT ProductID, ProductName FROM tblProducts Order By ProductName";
            DataServices dsProduct = new DataServices();
            DataTable dtProduct = dsProduct.RunQuery(sSql);
            //Chuyển dữ liệu vào ProductID tại DataGridView
            ProductID.DataSource = dtProduct;
            ProductID.DisplayMember = "ProductName";
            ProductID.ValueMember = "ProductID";

            //Truy vấn chi tiết phiếu xuất lên lưới.
            string sSql1 = "Exec tt_Nap_HDBanHang " + @RentalID;
            dsRental1 = new DataServices();
            dtRental1 = dsRental1.RunQuery(sSql1);

            //Truy vấn chi tiết phiếu xuất dùng Insert
            string sSql2 = "select * from tblRentalDetails where RentalID = '" + @RentalID + "'";
            dsRentalDetails = new DataServices();
            dtRentalDetails = dsRentalDetails.RunQuery(sSql2);
            dataGridView1.DataSource = dtRentalDetails;

            //Tính "Chi tiết thanh toán"
            double TamTinh = 0;
            for (int i = 0; i < dtRental1.Rows.Count; i++)
            {
                Math.Round(TamTinh += Convert.ToDouble(dtRental1.Rows[i]["ThanhTien"]), 0);
            }
            txtTamTinh.Text = TamTinh.ToString();

            double GG = (TamTinh * GiamGia / 100);
            Math.Round(GG, 0);
            txtGiamGia.Text = GG.ToString();

            double VAT = Math.Round((TamTinh - GG) * 0.08, 0);
            txtVAT.Text = VAT.ToString();

            double TT = Math.Round(TamTinh - GG + VAT, 0);
            txtTongTien.Text = TT.ToString();
        }

        void NapMotPhieuXuat(string @RentalID)
        {
            string sSql1 = "SELECT * FROM tblRentals where RentalID = '" + @RentalID + "'";
            DataServices dsPhieuXuat = new DataServices();
            DataTable dtPhieuXuat = dsPhieuXuat.RunQuery(sSql1);
            dataGridView1.DataSource = dtRentalDetails;
            if (dtRental1.Rows.Count > 0)
            {
                txtRentalID.Text = dtPhieuXuat.Rows[0]["RentalID"].ToString();
                dtmRentalDate.Value = Convert.ToDateTime(dtPhieuXuat.Rows[0]["RentalDate"]);
                cboCustomerID.SelectedValue = dtPhieuXuat.Rows[0]["CustomerID"].ToString();
                cboUserID.SelectedValue = dtPhieuXuat.Rows[0]["UserID"].ToString();
                txtDiscount.Text = dtPhieuXuat.Rows[0]["Discount"].ToString();
                GiamGia = Convert.ToDouble(txtDiscount.Text);
                txtDescription.Text = dtPhieuXuat.Rows[0]["Description"].ToString();
            }
        }

        private void SetControls(bool edit)
        {
            txtRentalID.Enabled = edit;
            cboCustomerID.Enabled = edit;
            cboUserID.Enabled = edit;
            dtmRentalDate.Enabled = edit;
            txtDiscount.Enabled = edit;
            txtDescription.Enabled = edit;

            // Thiết lập trạng thái các button
            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;

            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            DiChuyenDenPhieuI(ViTri);
            Display(verifyData.RentalID); //Hiển thị chi tiết phiếu xuất lên lưới
            SetControls(false);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            verifyData.RentalID = txtRentalID.Text;

            groupBox1.Enabled = false;
            //Xóa trắng các textBox
            txtRentalID.Clear();
            txtDiscount.Text = "0";
            txtDescription.Clear();

            SetControls(true);
            // Chuyển con trỏ về txtDiscount
            txtRentalID.Focus();
            modeNew = true;

            dataGridView1.ClearSelection();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _RentalID = txtRentalID.Text;
            groupBox1.Enabled = false;
            SetControls(true);
            //chuyển con trò vể txtDiscount
            txtDiscount.Focus();
            modeNew = false;
            txtRentalID.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Tại sao nó lại báo lỗi thủ tục trong khi đã tạo thủ tục ở trong Database???

            //Hiển thị hộp thoại xác nhận chắc chắn xóa không?
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xoá dữ liệu không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            //Chạy thủ tục delete
            string sSql = "EXEC DeleteRental '" + txtRentalID.Text + "'";
            DataServices dsDELETE = new DataServices();
            dsDELETE.ExecuteNonQuery(sSql);
            MessageBox.Show("Phiếu xuất đã bi xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Hiển thị lại dữ liệu sau khi thêm mới hoặc sửa
            NapDanhSachPhieuXuat();
            DiChuyenDenPhieuI(0);
            Display(txtRentalID.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu nhập vào không được rỗng từ lớp chung verifyData.cs
            if (verifyData.checkInputSpace(txtRentalID, "Mã phiếu xuất không được để trống!") == false) return;
            if (verifyData.checkLength(txtRentalID, 10, "Mã phiếu xuất không được vượt quá 10 kí tự!") == false) return;

            if (verifyData.checkLength(txtDiscount, 3, "Giảm giá không được vượt quá 3 kí tự") == false) return;

            if (verifyData.checkLength(txtDescription, 250, "Mô tả thêm không được vượt quá 250 kí tự!") == false) return;

            string sSql;
            DataServices dsSearch = new DataServices();
            DataTable dtSearch;
            //Kiểm tra dữ liệu trùng khi <thêm mới> hoặc <sửa> tên loại sách
            if (((modeNew == true) || (modeNew == false)) && (txtRentalID.Text != _RentalID))
            {
                sSql = "Select RentalID from tblRentals Where RentalID = N'" + txtRentalID.Text + "'";
                dtSearch = dsSearch.RunQuery(sSql);
                if (dtSearch.Rows.Count > 0)
                {
                    MessageBox.Show("Mã phiếu xuất đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtRentalID.Focus();
                    return;
                }
            }

            if (modeNew == true)
            {
                //1. Nhập dữ liệu vào bảng Rentals 
                DataRow myDataRowRental = dtRental.NewRow();
                myDataRowRental["RentalID"] = txtRentalID.Text;
                myDataRowRental["UserID"] = cboUserID.SelectedValue;
                myDataRowRental["CustomerID"] = cboCustomerID.SelectedValue;
                myDataRowRental["RentalDate"] = dtmRentalDate.Text;
                if (txtDiscount.Text == "")
                {
                    myDataRowRental["Discount"] = "0";
                }
                else
                {
                    myDataRowRental["Discount"] = txtDiscount.Text;
                }
                myDataRowRental["Description"] = txtDescription.Text;
                dtRental.Rows.Add(myDataRowRental);
                dsRental.Update(dtRental);
                MessageBox.Show("Thêm phiếu xuất thành công :)", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //Sửa dữ liệu
                sSql = "exec tt_Update_HDBanHang " + _RentalID + ", " + cboUserID.SelectedValue + ", " + cboCustomerID.SelectedValue + ", '" + dtmRentalDate.Text + "', " + txtDiscount.Text + ", N'" + txtDescription.Text + "'";
                DataServices dsUpdate = new DataServices();
                dsUpdate.RunQuery(sSql);

                MessageBox.Show("Phiếu xuất " + _RentalID.Trim() + " đã được chỉnh sửa :)", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            groupBox1.Enabled = true;

            //Hiển thị lại dữ liệu sau khi thêm mới hoặc sửa
            NapDanhSachPhieuXuat();
            Display(txtRentalID.Text);
            SetControls(false);
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            if (txtDiscount.Text == "")
                txtDiscount.Text = "0";
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (ViTri > 0)
            {
                ViTri -= 1;
                DiChuyenDenPhieuI(ViTri);
                Display(txtRentalID.Text); //Hiển thị chi tiết phiếu xuất lên lưới
            }
            else
            {
                MessageBox.Show("Đã đến số phiếu xuất đầu tiên", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ViTri < dtRental.Rows.Count - 1)
            {
                ViTri += 1;
                DiChuyenDenPhieuI(ViTri);
                Display(txtRentalID.Text); //Hiển thị chi tiết phiếu xuất lên lưới
            }
            else
            {
                MessageBox.Show("Đã đến số phiếu xuất cuối cùng", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void txtTamTinh_TextChanged(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            decimal value = decimal.Parse(txtTamTinh.Text, System.Globalization.NumberStyles.AllowThousands);
            txtTamTinh.Text = String.Format(culture, "{0:N0}", value);
            txtTamTinh.Select(txtTamTinh.Text.Length, 0);
        }

        private void txtVAT_TextChanged(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            decimal value = decimal.Parse(txtVAT.Text, System.Globalization.NumberStyles.AllowThousands);
            txtVAT.Text = String.Format(culture, "{0:N0}", value);
            txtVAT.Select(txtVAT.Text.Length, 0);
        }

        private void txtTongTien_TextChanged(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            decimal value = decimal.Parse(txtTongTien.Text, System.Globalization.NumberStyles.AllowThousands);
            txtTongTien.Text = String.Format(culture, "{0:N0}", value);
            txtTongTien.Select(txtTongTien.Text.Length, 0);
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            decimal value = decimal.Parse(txtGiamGia.Text, System.Globalization.NumberStyles.AllowThousands);
            txtGiamGia.Text = String.Format(culture, "{0:N0}", value);
            txtGiamGia.Select(txtGiamGia.Text.Length, 0);
        }

        private void btnSelectRentalID_Click(object sender, EventArgs e)
        {
            frmListRental frm = new frmListRental();
            frm.ShowDialog();
            if (verifyData.RentalID != "")
            {
                NapMotPhieuXuat(verifyData.RentalID);
                Display(verifyData.RentalID);
            }
        }
        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows[e.RowIndex].Cells.Count; i++)
            {
                dataGridView1[i, e.RowIndex].Style.BackColor = Color.Empty;
            }
        }

        private void dataGridViewHDBanHang_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            dataGridView1.Rows[e.Row.Index - 1].Cells["RentalID"].Value = txtRentalID.Text;
        }

        private void dataGridViewHDBanHang_RowValidated(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                dsRentalDetails.Update(dtRentalDetails);

                string sSql1 = "Exec tt_Nap_HDBanHang " + txtRentalID.Text; //Nạp lại HDBanHang
                dsRental1 = new DataServices();
                dtRental1 = dsRental1.RunQuery(sSql1);

                //Tính "Chi tiết thanh toán" sau khi sửa
                double TamTinh = 0;
                for (int i = 0; i < dtRental1.Rows.Count; i++)
                {
                    Math.Round(TamTinh += Convert.ToDouble(dtRental1.Rows[i]["ThanhTien"]), 0);
                }
                txtTamTinh.Text = TamTinh.ToString();

                double GG = (TamTinh * GiamGia / 100);
                Math.Round(GG, 0);
                txtGiamGia.Text = GG.ToString();

                double VAT = Math.Round((TamTinh - GG) * 0.08, 0);
                txtVAT.Text = VAT.ToString();

                double TT = Math.Round(TamTinh - GG + VAT, 0);
                txtTongTien.Text = TT.ToString();
            }
            catch { }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            frmReportRental.@RentalID = txtRentalID.Text.Trim();
            frmReportRental frm = new frmReportRental();
            frm.ShowDialog();
        }
    }
}
