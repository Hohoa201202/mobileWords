using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//mo them thu vien
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MobileWords
{
    public partial class frmEditSupplier : Form
    {

        //Khai bao bien de truy van va cap nhat du lieu
        private DataServices myDataServices;
        //Khai báo biến lưu bản sao của bảng Supplier trong CSDL
        private DataTable dtSupplier;
        //Khai báo biến để kiểm tra đã chọn nút <Thêm mới> hay <Sửa>
        private bool modeNew;
        //Khai báo biên lưu lại Phone
        private string _Phone;
        //Khai báo biên lưu lại CompanyName
        private string _CompanyName;
        public frmEditSupplier()
        {
            InitializeComponent();
        }

        private void Display()
        {
            // Truy vấn dữ liệu
            string sSql = "Select * from tblSuppliers Order By SupplierID";
            dtSupplier = myDataServices.RunQuery(sSql);
            //Hiển thị dữ liệu lên lưới
            dataGridView1.DataSource = dtSupplier;
        }

        private void SetControls(bool edit)
        {
            txtCompanyName.Enabled = edit;
            txtContactName.Enabled = edit;
            txtAddress.Enabled = edit;
            txtPhone.Enabled = edit;
            txtEmail.Enabled = edit;
            txtDescription.Enabled = edit;

            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;

            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }

        private void frmEditSupplier_Load(object sender, EventArgs e)
        {
            myDataServices = new DataServices();
            //Truy vấn dữ liệu lên lưới
            Display();
            //Đặt trạng thái các điều khiển trên Form
            SetControls(false);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            modeNew = false;
            SetControls(true);
            //Chuyển con trỏ về txtCompanyName để nhập
            txtCompanyName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Hiển thị hộp thoại xác nhận chắc chắn xóa không?
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xoá dữ liệu không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            //Lấy dòng dữ liệu hiện thời đã chọn trên lưới
            int r = dataGridView1.CurrentRow.Index;
            //xóa dòng tương ứng trong dtSupplier
            dtSupplier.Rows[r].Delete();
            //cấp nhật lại dữ liệu
            myDataServices.Update(dtSupplier);
            Display();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //1. Kiểm tra dữ liệu
            if (verifyData.checkInputSpace(txtCompanyName, "Ten công ty không được để trống!") == false) return;
            if (verifyData.checkLength(txtCompanyName, 250, "Tên sản phẩm không được quá 250 kí tự!") == false) return;

            if (verifyData.checkInputSpace(txtContactName, "Họ tên người đại diện không được để trống!") == false) return;
            if (verifyData.checkLength(txtContactName, 30, "Ho tên người đại diện không được quá 30 kí tự!") == false) return;

            if (verifyData.checkInputSpace(txtAddress, "Địa chỉ công ty không được để trống!") == false) return;
            if (verifyData.checkLength(txtContactName, 250, "Địa chỉ công ty không được quá 250 kí tự!") == false) return;

            if (verifyData.checkInputSpace(txtPhone, "Số điện thoại không được để trống!") == false) return;
            if (verifyData.checkPhone(txtPhone) == false)
            {
                MessageBox.Show("Yêu cầu nhập số điện thoại 10 số!", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPhone.Focus();
                return;
            }

            if (txtEmail.Text != "")
            {
                if (verifyData.checkEmail(txtEmail) == false)
                {
                    MessageBox.Show("Email không đúng định dạng!", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEmail.Focus();
                    return;
                }
            }

            if (verifyData.checkLength(txtDescription, 250, "Mô tả thêm không được quá 250 kí tự!") == false) return;

            string sSql;
            //Kiểm tra dữ liệu trùng khi <thêm mới> nhà cung cấp
            if ((modeNew == true) || ((modeNew == false) && (txtCompanyName.Text != _CompanyName)))
            {
                //truy vấn dữ liệu và kiểm tra trùng
                sSql = "Select CompanyName from tblSuppliers Where CompanyName = N'" + txtCompanyName.Text + "'";
                //tạo 1 DataServices khác
                DataServices myDataServices1 = new DataServices();
                DataTable dtSearch = myDataServices1.RunQuery(sSql);
                if (dtSearch.Rows.Count > 0)
                {
                    MessageBox.Show("Đã trùng tên công ty!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPhone.Focus();
                    return;
                }
            }

            //Kiểm tra dữ liệu trùng khi <thêm mới> số điện thoại 
            if ((modeNew == true) || ((modeNew == false) && (txtPhone.Text != _Phone)))
            {
                //truy vấn dữ liệu và kiểm tra trùng
                sSql = "Select Phone from tblSuppliers Where Phone = N'" + txtPhone.Text + "'";
                //tạo 1 DataServices khác
                DataServices myDataServices1 = new DataServices();
                DataTable dtSearch = myDataServices1.RunQuery(sSql);
                if (dtSearch.Rows.Count > 0)
                {
                    MessageBox.Show("Đã trùng số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPhone.Focus();
                    return;
                }
            }

            if (modeNew == true)
            {
                //1. tao 1 dòng dữ liệu
                DataRow myDataRow = dtSupplier.NewRow();
                //2. gán dữ liệu
                myDataRow["CompanyName"] = txtCompanyName.Text;
                myDataRow["ContactName"] = txtContactName.Text;
                myDataRow["Address"] = txtAddress.Text;
                myDataRow["Phone"] = txtPhone.Text;
                myDataRow["Email"] = txtEmail.Text;
                myDataRow["Description"] = txtDescription.Text;
                //3. Thêm dòng vào dtCustomer
                dtSupplier.Rows.Add(myDataRow);
                //4. Câp nhật lại CSDL
                myDataServices.Update(dtSupplier);
            }
            else
            {
                //Sửa dữ liệu
                //1. Lấy dòng hiện thời cần sửa
                int r = dataGridView1.CurrentRow.Index;
                //2. tao 1 dòng dữ liệu
                DataRow myDataRow = dtSupplier.Rows[r];
                //3. gán dữ liệu
                myDataRow["CompanyName"] = txtCompanyName.Text;
                myDataRow["ContactName"] = txtContactName.Text;
                myDataRow["Address"] = txtAddress.Text;
                myDataRow["Phone"] = txtPhone.Text;
                myDataRow["Email"] = txtEmail.Text;
                myDataRow["Description"] = txtDescription.Text;
                //4. Câp nhật lại CSDL
                myDataServices.Update(dtSupplier);

            }
            //Hiển thị lại dữ liệu sau khi thêm mới hoặc sửa
            Display();
            SetControls(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControls(false);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtCompanyName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtContactName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            //Lưu biến Phone để kiểm tra trùng
            _Phone = txtPhone.Text;

            //Lưu biến CompanyName để kiểm tra trùng
            _CompanyName = txtCompanyName.Text;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (verifyData.checkInputSpace(txtSearch, "Bạn cần nhập nội dung muốn tìm kiếm!") == false) return;
            if (verifyData.checkLength(txtSearch, 100, "Nội dung tìm kiếm không được quá 100 kí tự!") == false) return;

            //1. Định nghĩa xâu truy vấn
            string sSql;
            if (rbCompanyName.Checked == true)
                sSql = "SELECT * FROM tblSuppliers WHERE CompanyName LIKE N'%" + txtSearch.Text + "%' Order By SupplierID";
            else if (rbContactName.Checked == true)
                sSql = "SELECT * FROM tblSuppliers WHERE ContactName LIKE N'%" + txtSearch.Text + "%' Order By SupplierID";
            else
                sSql = "SELECT * FROM tblSuppliers WHERE Phone LIKE N'%" + txtSearch.Text + "%' Order By SupplierID ";
            dtSupplier = myDataServices.RunQuery(sSql);
            //Hiển thị dữ liệu lên lưới
            dataGridView1.DataSource = dtSupplier;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            verifyData.checkEmail(txtEmail);
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            verifyData.checkPhone(txtPhone);
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtCompanyName_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(null, txtEmail, txtContactName, null, txtContactName, e);

        }

        private void txtContactName_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtCompanyName, txtDescription, txtAddress, txtCompanyName, txtAddress, e);

        }

        private void txtAddress_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtContactName, txtDescription, txtPhone, txtContactName, txtPhone, e);

        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtAddress, txtDescription, txtEmail, txtAddress, txtEmail, e);

        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtPhone, txtDescription, txtDescription, txtCompanyName, txtDescription, e);

        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(null, txtEmail, null, txtContactName, null, e);

        }
    }
}
