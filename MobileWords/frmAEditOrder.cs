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
    public partial class frmAEditOrder : Form
    {
        //Dùng Insert bảng tblOrders
        private DataServices dsOrder;
        private DataTable dtOrder;

        //Dùng Insert bảng tblOrderDetails
        private DataServices dsOrderDetails;
        private DataTable dtOrderDetails;

        //Dùng truy vấn lên lưới DataGridView
        private DataServices dsOrder1;
        private DataTable dtOrder1;

        public static int ViTri = 0; //Biến lưu vị trí phiếu nhập
        private bool modeNew;
        private string _OrderID;

        public frmAEditOrder()
        {
            InitializeComponent();
        }

        private void frmAEditOrder_Load(object sender, EventArgs e)
        {
            // 1. Chuyển dữ liệu vào cboCustomerName
            // 1.1. Truy vấn từ bảng tblUsers
            string sSql = "SELECT * FROM tblSuppliers Order By CompanyName";
            DataServices dsSupplier = new DataServices();
            DataTable dtSupplier = dsSupplier.RunQuery(sSql);
            // 1.2. Chuyển dữ liệu vào cboCustomerName
            cboSupplierID.DataSource = dtSupplier;
            cboSupplierID.DisplayMember = "CompanyName";
            cboSupplierID.ValueMember = "SupplierID";

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
            NapDanhSachPhieuNhap();
            if (dtOrder.Rows.Count > 0)
            {
                ViTri = 0;
                DiChuyenDenPhieuI(ViTri);
                Display(txtOrderID.Text); //Hiển thị chi tiết phiếu nhập lên lưới
            }
            else
            {
                MessageBox.Show("Chưa có phiếu nhập nào", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //thiết lập các nút ấn
            SetControls(false);
        }
        void NapDanhSachPhieuNhap()
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblOrders order by OrderID";
            dsOrder = new DataServices();
            dtOrder = dsOrder.RunQuery(sSql);
        }

        void DiChuyenDenPhieuI(int I)
        {
            txtOrderID.Text = dtOrder.Rows[I]["OrderID"].ToString();
            txtOrderIDD.Text = dtOrder.Rows[I]["OrderID"].ToString();
            dtmOrderDate.Value = Convert.ToDateTime(dtOrder.Rows[I]["OrderDate"]);
            cboSupplierID.SelectedValue = dtOrder.Rows[I]["SupplierID"].ToString();
            cboUserID.SelectedValue = dtOrder.Rows[I]["UserID"].ToString();
            txtDescription.Text = dtOrder.Rows[I]["Description"].ToString();
        }

        private void Display(string @OrderID) //Nạp chi tiết phiếu nhập lên DataGridView
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

            //Truy vấn chi tiết phiếu xuất để tính toán giá tiền hóa đơn
            string sSql1 = "Exec tt_Nap_HDNhapHang " + @OrderID;
            dsOrder1 = new DataServices();
            dtOrder1 = dsOrder1.RunQuery(sSql1);

            //Truy vấn chi tiết phiếu xuất dùng Insert và đưa lên DataGridView
            string sSql2 = "select * from tblOrderDetails where OrderID = '" + @OrderID + "'";
            dsOrderDetails = new DataServices();
            dtOrderDetails = dsOrderDetails.RunQuery(sSql2);
            dataGridView1.DataSource = dtOrderDetails;

            //Tính "Chi tiết thanh toán"
            double TT = 0;
            for (int i = 0; i < dtOrder1.Rows.Count; i++)
            {
                Math.Round(TT += Convert.ToDouble(dtOrder1.Rows[i]["ThanhTien"]), 0);
            }

            txtTongTien.Text = TT.ToString();
        }

        void NapMotPhieuNhap(string @OrdeID)
        {
            string sSql1 = "SELECT * FROM tblOrders where OrderID = '" + @OrdeID + "'";
            DataServices dsPhieuNhap = new DataServices();
            DataTable dtPhieuNhap = dsPhieuNhap.RunQuery(sSql1);
            dataGridView1.DataSource = dtOrderDetails;
            if (dtOrder1.Rows.Count > 0)
            {
                txtOrderID.Text = dtPhieuNhap.Rows[0]["OrderID"].ToString();
                txtOrderIDD.Text = dtPhieuNhap.Rows[0]["OrderID"].ToString();
                dtmOrderDate.Value = Convert.ToDateTime(dtPhieuNhap.Rows[0]["OrderDate"]);
                cboSupplierID.SelectedValue = dtPhieuNhap.Rows[0]["SupplierID"].ToString();
                cboUserID.SelectedValue = dtPhieuNhap.Rows[0]["UserID"].ToString();
                txtDescription.Text = dtPhieuNhap.Rows[0]["Description"].ToString();
            }
        }

        private void SetControls(bool edit)
        {
            txtOrderID.Enabled = edit;
            cboSupplierID.Enabled = edit;
            cboUserID.Enabled = edit;
            dtmOrderDate.Enabled = edit;
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
            if (verifyData.checkInputSpace(txtOrderID, "Mã phiếu nhâp không được để trống!") == false) return;
            if (verifyData.checkLength(txtOrderID, 10, "Mã phiếu nhập không được vượt quá 10 kí tự!") == false) return;

            if (verifyData.checkLength(txtDescription, 250, "Mô tả thêm không được vượt quá 250 kí tự!") == false) return;

            string sSql;
            DataServices dsSearch = new DataServices();
            DataTable dtSearch;
            //Kiểm tra dữ liệu trùng khi <thêm mới> hoặc <sửa> tên loại sách
            if (((modeNew == true) || (modeNew == false)) && (txtOrderID.Text != _OrderID))
            {
                sSql = "Select * from tblOrders Where OrderID = N'" + txtOrderID.Text + "'";
                dtSearch = dsSearch.RunQuery(sSql);
                if (dtSearch.Rows.Count > 0)
                {
                    MessageBox.Show("Mã phiếu nhập đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtOrderID.Focus();
                    return;
                }
            }

            if (modeNew == true)
            {
                //1. Nhập dữ liệu vào bảng Rentals 
                DataRow myDataRowRental = dtOrder.NewRow();
                myDataRowRental["OrderID"] = txtOrderID.Text;
                myDataRowRental["UserID"] = cboUserID.SelectedValue;
                myDataRowRental["SupplierID"] = cboSupplierID.SelectedValue;
                myDataRowRental["OrderDate"] = dtmOrderDate.Text;
                myDataRowRental["Description"] = txtDescription.Text;
                dtOrder.Rows.Add(myDataRowRental);
                dsOrder.Update(dtOrder);
                MessageBox.Show("Thêm phiếu nhập thành công :)", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //Sửa dữ liệu
                sSql = "exec tt_Update_HDNhapHang " + _OrderID + ", " + cboUserID.SelectedValue + ", " + cboSupplierID.SelectedValue + ", '" + dtmOrderDate.Text + "', N'" + txtDescription.Text + "'";
                DataServices dsUpdate = new DataServices();
                dsUpdate.RunQuery(sSql);

                MessageBox.Show("Phiếu nhập " + _OrderID.Trim() + " đã được chỉnh sửa :)", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            groupBox1.Enabled = true;

            //Hiển thị lại dữ liệu sau khi thêm mới hoặc sửa
            NapDanhSachPhieuNhap();
            Display(txtOrderID.Text);
            SetControls(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            DiChuyenDenPhieuI(ViTri);
            Display(verifyData.OrderID); //Hiển thị chi tiết phiếu xuất lên lưới
            SetControls(false);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            verifyData.OrderID = txtOrderID.Text;

            groupBox1.Enabled = false;
            //Xóa trắng các textBox
            txtOrderID.Clear();
            txtDescription.Clear();

            SetControls(true);
            // Chuyển con trỏ về txtDiscount
            txtOrderID.Focus();
            modeNew = true;

            dataGridView1.ClearSelection();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _OrderID = txtOrderID.Text;
            _OrderID = txtOrderIDD.Text;
            groupBox1.Enabled = false;
            SetControls(true);
            //chuyển con trò vể txtDiscount
            txtDescription.Focus();
            modeNew = false;
            txtOrderID.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Hiển thị hộp thoại xác nhận chắc chắn xóa không?
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xoá dữ liệu không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            //Chạy thủ tục delete
            string sSql = "EXEC DeleteOrder '" + txtOrderID.Text + "'";
            DataServices dsDELETE = new DataServices();
            dsDELETE.ExecuteNonQuery(sSql);
            MessageBox.Show("Phiếu nhập đã bi xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Hiển thị lại dữ liệu sau khi thêm mới hoặc sửa
            NapDanhSachPhieuNhap();
            DiChuyenDenPhieuI(0);
            Display(txtOrderID.Text);
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (ViTri > 0)
            {
                ViTri -= 1;
                DiChuyenDenPhieuI(ViTri);
                Display(txtOrderID.Text); //Hiển thị chi tiết phiếu xuất lên lưới
            }
            else
            {
                MessageBox.Show("Đã đến số phiếu xuất đầu tiên", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ViTri < dtOrder.Rows.Count - 1)
            {
                ViTri += 1;
                DiChuyenDenPhieuI(ViTri);
                Display(txtOrderID.Text); //Hiển thị chi tiết phiếu xuất lên lưới
            }
            else
            {
                MessageBox.Show("Đã đến số phiếu nhập cuối cùng", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnSelectRentalID_Click(object sender, EventArgs e)
        {
            frmListOrder frm = new frmListOrder();
            frm.ShowDialog();
            if (verifyData.OrderID != "")
            {
                NapMotPhieuNhap(verifyData.OrderID);
                Display(verifyData.OrderID);
            }
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {

            for (int i = 0; i < dataGridView1.Rows[e.RowIndex].Cells.Count; i++)
            {
                dataGridView1[i, e.RowIndex].Style.BackColor = Color.Empty;
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            dataGridView1.Rows[e.Row.Index - 1].Cells["OrderID"].Value = txtOrderID.Text;
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                // Nhập dữ liệu vào bảng tblOrderDetails
                dsOrderDetails.Update(dtOrderDetails);

                string sSql1 = "Exec tt_Nap_HDNhapHang " + txtOrderID.Text;
                dsOrder1 = new DataServices();
                dtOrder1 = dsOrder1.RunQuery(sSql1);

                //Tính "Chi tiết thanh toán" sau khi sửa
                double TT = 0;
                for (int i = 0; i < dtOrder1.Rows.Count; i++)
                {
                    Math.Round(TT += Convert.ToDouble(dtOrder1.Rows[i]["ThanhTien"]), 0);
                }

                txtTongTien.Text = TT.ToString();

            }
            catch { }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmReportOrder.@OrderID = txtOrderID.Text;
            frmReportOrder frm = new frmReportOrder();
            frm.ShowDialog();
        }
    }
}
