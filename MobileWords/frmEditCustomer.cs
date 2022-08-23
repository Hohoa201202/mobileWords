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
    public partial class frmEditCustomer : Form
    {
        //Khai bao bien de truy van va cap nhat du lieu
        private DataServices myDataServices;
        //Khai báo biến lưu bản sao của bảng tblCustomers trong CSDL
        private DataTable dtCustomer;
        //Khai báo biên lưu lại Phone để kiểm tra trùng
        private string _Phone;
        //Khai báo biên lưu lại CMND để kiểm tra trùng
        private string _CMND;

        public frmEditCustomer()
        {
            InitializeComponent();
        }

        private void Display()
        {
            // Truy vấn dữ liệu
            string sSql = "Select * from tblCustomers Order By CustomerName";
            dtCustomer = myDataServices.RunQuery(sSql);
            //Hiển thị dữ liệu lên lưới
            dataGridView1.DataSource = dtCustomer;
        }
        private void SetControls(bool edit)
        {
            txtCustomerName.Enabled = edit;
            txtIdentification.Enabled = edit;
            txtAddress.Enabled = edit;
            txtPhone.Enabled = edit;
            txtEmail.Enabled = edit;
            txtDescription.Enabled = edit;

            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;

            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }


        private void frmEditCustomer_Load(object sender, EventArgs e)
        {
            myDataServices = new DataServices();
            //Truy vấn dữ liệu lên lưới
            Display();
            //Đặt trạng thái các điều khiển trên Form
            SetControls(false);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            groupBox1.Enabled = false;
            SetControls(true);

            //Chuyển con trỏ về txtCustomerName để nhập
            txtCustomerName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Hiển thị hộp thoại xác nhận chắc chắn xóa không?
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xoá dữ liệu không? Nếu xóa khách hàng này thì các dữ liệu liên quan đến hóa đơn bán hàng và hóa đơn bảo hành sản phẩm của khách hàng này sẽ bị xóa hết!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            //Lấy dòng dữ liệu hiện thời đã chọn trên lưới
            int r = dataGridView1.CurrentRow.Index;
            //2. Lấy CustomerID của dòng hiện thời
            string CustomerID = dataGridView1.Rows[r].Cells["CustomerID"].Value.ToString();

            string sSql = "exec tt_Delete_tblCustomer " + CustomerID;
            DataServices dsDelete = new DataServices();
            dsDelete.ExecuteNonQuery(sSql);
            Display();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //1. Kiểm tra dữ liệu
            if (verifyData.checkInputSpace(txtCustomerName, "Họ và tên khách hàng không được để trống!") == false) return;
            if (verifyData.checkLength(txtCustomerName, 30, "Họ và tên khách hàng không được quá 30 kí tự!") == false) return;

            if (verifyData.checkLength(txtAddress, 50, "Địa chỉ không được quá 50 kí tự!") == false) return;

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

            if (txtIdentification.Text != "")
            {
                if (verifyData.checkIdentification(txtIdentification) == false)
                {
                    MessageBox.Show("Yêu cầu nhập CMND 9 số hoặc CCCD 12 số!", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtIdentification.Focus();
                    return;
                }
            }

            if (verifyData.checkLength(txtDescription, 250, "Mô tả thêm không được quá 250 kí tự!") == false) return;

            string sSql;
            //Kiểm tra dữ liệu trùng khi thêm mới CMND khách hàng
            if (txtIdentification.Text != "")
            {
                if (txtIdentification.Text != _CMND)
                {
                    //truy vấn dữ liệu và kiểm tra trùng
                    sSql = "select * from tblCustomers where Identification = N'" + txtIdentification.Text + "' AND Phone = '" + txtPhone.Text + "'";
                    // Tạo 1 DataServices khác
                    DataServices myDataServices1 = new DataServices();
                    DataTable dtSearch = myDataServices1.RunQuery(sSql);
                    if (dtSearch.Rows.Count > 0)
                    {
                        MessageBox.Show("Khách hàng đã tồn tại trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPhone.Focus();
                        return;
                    }
                }
            }

            //Kiểm tra dữ liệu trùng khi thêm mới sđt khách hàng
            if (txtPhone.Text != _Phone)
            {
                //truy vấn dữ liệu và kiểm tra trùng
                sSql = "select * from tblCustomers where Phone = '" + txtPhone.Text + "'";
                //tạo 1 DataServices khác
                DataServices myDataServices1 = new DataServices();
                DataTable dtSearch = myDataServices1.RunQuery(sSql);
                if (dtSearch.Rows.Count > 0)
                {
                    MessageBox.Show("Khách hàng đã tồn tại, Số điện thoại bị trùng với một khách hàng khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPhone.Focus();
                    return;
                }
            }

            //Thêm dữ liệu vào DataBase
            //1. Lấy dòng dữ liệu hiện thời đã chọn trên lưới
            int r = dataGridView1.CurrentRow.Index;
            //2. Tạo 1 dòng dữ liệu là dòng đang chọn
            DataRow myDataRow = dtCustomer.Rows[r];
            //3. Gán lại dữ liệu vừa sửa
            myDataRow["CustomerName"] = txtCustomerName.Text;
            myDataRow["Identification"] = txtIdentification.Text;
            myDataRow["Address"] = txtAddress.Text;
            myDataRow["Phone"] = txtPhone.Text;
            myDataRow["Email"] = txtEmail.Text;
            myDataRow["Description"] = txtDescription.Text;

            //3. Câp nhật vào CSDL
            myDataServices.Update(dtCustomer);

            groupBox1.Enabled = true;
            //Hiển thị lại dữ liệu sau khi thêm mới hoặc sửa
            Display();
            SetControls(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            SetControls(false);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (verifyData.checkInputSpace(txtSearch, "Bạn cần nhập nội dung muốn tìm kiếm!") == false) return;
            if (verifyData.checkLength(txtSearch, 100, "Nội dung tìm kiếm không được quá 100 kí tự!") == false) return;

            //Truy vấn dữ liệu
            string sSql = "";
            if (rbCustomerName.Checked == true)
                sSql = "Select * From tblCustomers where CustomerName like N'%" + txtSearch.Text + "%' Order By CustomerName";
            else if (rbCMND.Checked == true)
                sSql = "Select * From tblCustomers where Identification like N'%" + txtSearch.Text + "%'";
            else if (rbPhone.Checked == true)
                sSql = "Select * From tblCustomers where Phone like N'%" + txtSearch.Text + "%'";
            dtCustomer = myDataServices.RunQuery(sSql);

            //Hiển thị lên lưới
            dataGridView1.DataSource = dtCustomer;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtCustomerName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtIdentification.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

            //Lưu biến Phone để kiểm tra trùng
            _Phone = txtPhone.Text;

            //Lưu biến _CMND để kiểm tra trùng
            _CMND = txtIdentification.Text;
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtAddress, txtDescription, txtEmail, txtAddress, txtEmail, e);

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            verifyData.checkPhone(txtPhone);
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            verifyData.checkEmail(txtEmail);
        }

        private void txtCustomerName_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(null, txtEmail, txtIdentification, null, txtIdentification, e);
        }

        private void txtIdentification_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtCustomerName, txtDescription, txtAddress, txtCustomerName, txtAddress, e);

        }

        private void txtIdentification_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtIdentification_TextChanged(object sender, EventArgs e)
        {
            verifyData.checkIdentification(txtIdentification);
        }

        private void txtAddress_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtIdentification, txtDescription, txtPhone, txtIdentification, txtPhone, e);

        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtPhone, txtDescription, txtDescription, txtCustomerName, txtDescription, e);

        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(null, txtEmail, null, txtIdentification, null, e);

        }
    }
}
