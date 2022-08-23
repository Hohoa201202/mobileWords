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
    public partial class frmAddCustomer : Form
    {
        //Khai bao bien de truy van va cap nhat du lieu
        private DataServices myDataServices;
        //Khai báo biến lưu bản sao của bảng tblCustomers trong CSDL
        private DataTable dtCustomer;

        public frmAddCustomer()
        {
            InitializeComponent();
        }

        private void frmAddCustomer_Load(object sender, EventArgs e)
        {
            myDataServices = new DataServices();
            //Truy vấn dữ liệu lên lưới
            Display();
            //Đặt trạng thái các điều khiển trên Form
            SetControls(false);
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

            btnNew.Enabled = !edit;

            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            SetControls(true);

            //Xóa trắng các textBox
            txtIdentification.Clear();
            txtCustomerName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtDescription.Clear();

            //Chuyển con trỏ về txtCustomerName để nhập
            txtCustomerName.Focus();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtCustomerName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtIdentification.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //1. Kiểm tra dữ liệu
            if (verifyData.checkInputSpace(txtCustomerName, "Họ và tên khách hàng không được để trống!") == false) return;
            if (verifyData.checkLength(txtCustomerName, 30, "Họ và tên khách hàng không được quá 30 kí tự!") == false) return;

            if (txtIdentification.Text != "")
            {
                if (verifyData.checkIdentification(txtIdentification) == false)
                {
                    MessageBox.Show("Yêu cầu nhập CMND 9 số hoặc CCCD 12 số!", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtIdentification.Focus();
                    return;
                }
            }

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

            if (verifyData.checkLength(txtDescription, 250, "Mô tả thêm không được quá 250 kí tự!") == false) return;

            string sSql;
            DataServices myDataServices1 = new DataServices();
            DataTable dtSearch;
            /*/Kiểm tra dữ liệu trùng khi thêm mới khách hàng.
            //truy vấn dữ liệu và kiểm tra trùng
            sSql = "select * from tblCustomers where CustomerName = N'" + txtCustomerName.Text + "' AND Phone = '" + txtPhone.Text + "'";
            //tạo 1 DataServices khác
            DataServices myDataServices1 = new DataServices();
            DataTable dtSearch = myDataServices1.RunQuery(sSql);
            if (dtSearch.Rows.Count > 0)
            {
                 MessageBox.Show("Khách hàng đã tồn tại trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 txtPhone.Focus();
                 return;
            }*/

            //Kiểm tra dữ liệu trùng khi thêm mới CMND khách hàng
            //truy vấn dữ liệu và kiểm tra trùng
            if (txtIdentification.Text != "")
            {
                //truy vấn dữ liệu và kiểm tra trùng
                sSql = "select * from tblCustomers where Identification = N'" + txtIdentification.Text + "'";
                //tạo 1 DataServices khác
                myDataServices1 = new DataServices();
                dtSearch = myDataServices1.RunQuery(sSql);
                if (dtSearch.Rows.Count > 0)
                {
                    MessageBox.Show("Khách hàng đã tồn tại, CMND/CCCD bị trùng với một khách hàng khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtIdentification.Focus();
                    return;
                }
            }

            //Kiểm tra dữ liệu trùng khi thêm mới sđt khách hàng
            //truy vấn dữ liệu và kiểm tra trùng
            sSql = "select * from tblCustomers where Phone = '" + txtPhone.Text + "'";

            //dtSearch.Clear();
            myDataServices1 = new DataServices();
            dtSearch = myDataServices1.RunQuery(sSql);
            if (dtSearch.Rows.Count > 0)
            {
                MessageBox.Show("Khách hàng đã tồn tại, Số điện thoại bị trùng với một khách hàng khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPhone.Focus();
                return;
            }

            //Thêm dữ liệu vào DataBase
            //1. tao 1 dòng dữ liệu
            DataRow myDataRow = dtCustomer.NewRow();
            //2. gán dữ liệu
            myDataRow["CustomerName"] = txtCustomerName.Text;
            myDataRow["Identification"] = txtIdentification.Text;
            myDataRow["Address"] = txtAddress.Text;
            myDataRow["Phone"] = txtPhone.Text;
            myDataRow["Email"] = txtEmail.Text;
            myDataRow["Description"] = txtDescription.Text;
            //3. Thêm dòng vào dtCustomer
            dtCustomer.Rows.Add(myDataRow);
            //4. Câp nhật lại CSDL
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

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
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

        private void txtAddress_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtIdentification, txtDescription, txtPhone, txtIdentification, txtPhone, e);
        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtAddress, txtDescription, txtEmail, txtAddress, txtEmail, e);
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtPhone, txtDescription, txtDescription, txtCustomerName, txtDescription, e);

        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(null, txtEmail, null, txtIdentification, null, e);

        }

        private void txtIdentification_TextChanged(object sender, EventArgs e)
        {
            verifyData.checkIdentification(txtIdentification);

        }

        private void txtIdentification_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
