using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Mở thư viện
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace MobileWords
{
    public partial class frmAEditUsers : Form
    {
        //khai báo xâu kết nối tơi database
        //private string conStr = "server = localhost; database = Books; uid = sa; pwd = sa123";
        private string conStr = @"Data Source=HOHOA201202\SQLEXPRESS;Initial Catalog=MobileWords;Integrated Security=True";

        //khai bao bien de ket noi toi CSDL
        private SqlConnection mySqlConnection;

        //Khai bao bien de truy van va cap nhat du lieu
        private SqlCommand mySqlCommand;

        //Khai báo biến để kiểm tra đã chọn nút <Thêm mới> hay <Sửa>
        private bool modeNew;

        //Khai báo biên lưu lại UserName
        private string _UserName;
        public frmAEditUsers()
        {
            InitializeComponent();
        }

        private void Display()
        {
            // Truy vấn dữ liệu - gọi thủ tục trong Sql-server
            string sSql = "EXECUTE DisplayUser";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            //Hiển thị dữ liệu đã truy vấn lên lưới
            DataTable dt = new DataTable();
            dt.Load(mySqlDataReader);
            dataGridNhanVien.DataSource = dt;

            //Đóng SqlDataReader
            mySqlDataReader.Close();
        }
        //Hàm điều khiển trạng thái Enable của các điều khiển
        private void SetControls(bool edit)
        {
            txtUserName.Enabled = edit;
            txtPassword.Enabled = edit;
            txtFullName.Enabled = edit;
            txtIdentification.Enabled = edit;
            txtPhone.Enabled = edit;
            txtAddress.Enabled = edit;
            txtEmail.Enabled = edit;
            dtmBirthYear.Enabled = edit;
            rbActive.Enabled = edit;
            rbInActive.Enabled = edit;
            txtDescription.Enabled = edit;

            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;

            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }


        private void frmAEditUsers_Load(object sender, EventArgs e)
        {
            //Kết nối tới CSDL
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();

            //Truy vấn dữ liệu lên lưới
            Display();

            //Đặt trạng thái các điều khiển trên Form
            SetControls(false);
        }

        private void dataGridNhanVien_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //lấy dòng hiện thời
            int r = e.RowIndex;
            txtUserName.Text = dataGridNhanVien.Rows[r].Cells[1].Value.ToString();
            txtPassword.Text = dataGridNhanVien.Rows[r].Cells[2].Value.ToString();
            txtFullName.Text = dataGridNhanVien.Rows[r].Cells[3].Value.ToString();
            txtIdentification.Text = dataGridNhanVien.Rows[r].Cells[4].Value.ToString();
            txtPhone.Text = dataGridNhanVien.Rows[r].Cells[5].Value.ToString();
            txtAddress.Text = dataGridNhanVien.Rows[r].Cells[6].Value.ToString();
            txtEmail.Text = dataGridNhanVien.Rows[r].Cells[7].Value.ToString();
            if (dataGridNhanVien.Rows[r].Cells[9].Value.ToString().Trim() == "1")
                rbActive.Checked = true;
            else
                rbInActive.Checked = true;

            txtDescription.Text = dataGridNhanVien.Rows[r].Cells[10].Value.ToString();
            try
            {
                dtmBirthYear.Text = Convert.ToDateTime(dataGridNhanVien.Rows[r].Cells[8].Value.ToString()).ToShortDateString();
            }

            catch
            {
            }
            //Lưu lại biến UserName để kiểm tra sửa trùng
            _UserName = txtUserName.Text;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            modeNew = true;
            SetControls(true);
            //Xóa trắng các textBox
            txtUserName.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            txtIdentification.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtDescription.Clear();
            //Chuyển con trỏ về txtFullName để nhập
            txtUserName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            modeNew = false;
            SetControls(true);
            //Chuyển con trỏ về txtFullName để nhập
            txtUserName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            //Hiển thị hộp thoại xác nhận chắc chắn xóa không?
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xoá dữ liệu không. Nếu xóa thì tất cả dữ liệu liên quan đến người dùng sẽ xóa hết?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            //Lấy dòng dữ liệu hiện thời đã chọn trên lưới
            int r = dataGridNhanVien.CurrentRow.Index;
            //Lấy mã 
            string UserID = dataGridNhanVien.Rows[r].Cells[0].Value.ToString();
            //Định nghĩa xâu SQL goij thủ tục trong SQL Server
            string sSql = "EXECUTE DeleteUser @UserID";
            //Chạy lệnh Sql
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            //Định nghĩa tham số
            mySqlCommand.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = UserID;
            //Thự hiện lệnh xóa
            mySqlCommand.ExecuteNonQuery();

            //Hiển thị lại dữ liệu đã xóa
            Display();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //1. Kiểm tra dữ liệu
            if (verifyData.checkInputSpace(txtUserName, "Tên tài khoản không được để trống!") == false) return;
            if (verifyData.checkLength(txtUserName, 30, "Tên tài khoản không vượt được quá 30 kí tự!") == false) return;

            if (verifyData.checkInputSpace(txtPassword, "Mật khẩu không được để trống!") == false) return;
            if (verifyData.checkLength(txtPassword, 30, "Mật khẩu không được vượt quá 30 kí tự!") == false) return;

            if (verifyData.checkInputSpace(txtFullName, "Họ và tên không được để trống!") == false) return;
            if (verifyData.checkLength(txtFullName, 30, "Họ và tên không được vượt quá 30 kí tự!") == false) return;

            if (verifyData.checkInputSpace(txtIdentification, "CMND/CCCD không được để trống!") == false) return;
            if (verifyData.checkIdentification(txtIdentification) == false)
            {
                MessageBox.Show("CMND/CCCD 9 số hoặc 12 số!", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtIdentification.Focus();
                return;
            }

            if (verifyData.checkInputSpace(txtPhone, "Số điện thoại không được để trống!") == false) return;
            if (verifyData.checkPhone(txtPhone) == false)
            {
                MessageBox.Show("Yêu cầu nhập số điện thoại 10 số!", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPhone.Focus();
                return;
            }

            if (verifyData.checkInputSpace(txtAddress, "Quê quán không được để trống!") == false) return;
            if (verifyData.checkLength(txtAddress, 50, "Quê quán không được vượt quá 50 kí tự!") == false) return;

            if (txtEmail.Text != "")
            {
                if (verifyData.checkEmail(txtEmail) == false)
                {
                    MessageBox.Show("Email không đúng định dạng!", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEmail.Focus();
                    return;
                }
            }

            if (verifyData.checkLength(txtDescription, 250, "Mô tả thêm không được vượt quá 250 kí tự!") == false) return;

            string sSql;
            //Kiểm tra dữ liệu trùng khi <thêm mới> hoặc <sửa> tên loại sách
            if ((modeNew == true) || ((modeNew == false) && (txtUserName.Text != _UserName)))
            {
                //truy vấn dữ liệu và kiểm tra trùng
                sSql = "Select UserName from tblUsers Where UserName = @UserName";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                //Định nghĩa tham số
                mySqlCommand.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = txtUserName.Text;
                //Thực hiện lênh
                SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                if (mySqlDataReader.HasRows == true)
                {
                    MessageBox.Show("Đã trùng tên tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserName.Focus();
                    //Đóng SqlDataReader
                    mySqlDataReader.Close();
                    return;
                }
                mySqlDataReader.Close();
            }


            if (modeNew == true)
            {
                //Thêm mới dữ liệu
                sSql = "EXECUTE AddUser @UserName, @Password, @FullName , @Identification, @Phone ,@Address, @Email,  @BirthYear, @Status, @Description";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                //Định nghĩa tham số
                mySqlCommand.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = txtUserName.Text;
                mySqlCommand.Parameters.Add("@Password", SqlDbType.NVarChar, 30).Value = txtPassword.Text;
                mySqlCommand.Parameters.Add("@FullName", SqlDbType.NVarChar, 35).Value = txtFullName.Text;
                mySqlCommand.Parameters.Add("@Identification", SqlDbType.Char, 20).Value = txtIdentification.Text;
                mySqlCommand.Parameters.Add("@Phone", SqlDbType.Char, 11).Value = txtPhone.Text;
                mySqlCommand.Parameters.Add("@Address", SqlDbType.NVarChar, 40).Value = txtAddress.Text;
                mySqlCommand.Parameters.Add("@Email", SqlDbType.Char, 30).Value = txtEmail.Text;
                mySqlCommand.Parameters.Add("@BirthYear", SqlDbType.Date).Value = dtmBirthYear.Text;
                if (rbActive.Checked == true)
                    mySqlCommand.Parameters.Add("@Status", SqlDbType.Int, 4).Value = 1;
                else
                    mySqlCommand.Parameters.Add("@Status", SqlDbType.Int, 4).Value = 0;

                mySqlCommand.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value = txtDescription.Text;
                //thực hiện lệnh insert
                mySqlCommand.ExecuteNonQuery();
            }
            else
            {
                //Sửa dữ liệu
                //1. Lấy dòng hiện thời cần sửa
                int r = dataGridNhanVien.CurrentRow.Index;
                //2. lấy mã cần sửa
                string UserID = dataGridNhanVien.Rows[r].Cells[0].Value.ToString();
                string PersonnelID = dataGridNhanVien.Rows[r].Cells[1].Value.ToString();
                //3. Định nghĩa câu SQL
                sSql = "EXECUTE UpdateUser @UserID,   @UserName, @Password, @FullName , @Identification, @Phone ,@Address, @Email,  @BirthYear, @Status, @Description";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                //Định nghĩa các tham số
                mySqlCommand.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = UserID;
                mySqlCommand.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = txtUserName.Text;
                mySqlCommand.Parameters.Add("@Password", SqlDbType.NVarChar, 30).Value = txtPassword.Text;
                mySqlCommand.Parameters.Add("@FullName", SqlDbType.NVarChar, 35).Value = txtFullName.Text;
                mySqlCommand.Parameters.Add("@Identification", SqlDbType.Char, 20).Value = txtIdentification.Text;
                mySqlCommand.Parameters.Add("@Phone", SqlDbType.Char, 11).Value = txtPhone.Text;
                mySqlCommand.Parameters.Add("@Address", SqlDbType.NVarChar, 40).Value = txtAddress.Text;
                mySqlCommand.Parameters.Add("@Email", SqlDbType.Char, 30).Value = txtEmail.Text;
                mySqlCommand.Parameters.Add("@BirthYear", SqlDbType.Date).Value = dtmBirthYear.Text;
                if (rbActive.Checked == true)
                    mySqlCommand.Parameters.Add("@Status", SqlDbType.Int, 4).Value = 1;
                else
                    mySqlCommand.Parameters.Add("@Status", SqlDbType.Int, 4).Value = 0;

                mySqlCommand.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value = txtDescription.Text;

                //4. Chạy lệnh SQL

                mySqlCommand.ExecuteNonQuery();
            }
            //Hiển thị lại dữ liệu sau khi thêm mới hoặc sửa
            Display();
            groupBox1.Enabled = true;
            SetControls(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            SetControls(false);
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(null, txtPhone, txtPassword, null, txtPassword, e);
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtUserName, txtAddress, txtFullName, txtUserName, txtFullName, e);
        }

        private void txtFullName_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtPassword, txtEmail, txtIdentification, txtPassword, txtIdentification, e);

        }

        private void txtIdentification_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtFullName, txtEmail, txtPhone, txtFullName, txtPhone, e);

        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtIdentification, txtAddress, txtAddress, txtUserName, txtAddress, e);

        }

        private void txtAddress_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtPhone, txtEmail, txtEmail, txtPassword, txtEmail, e);

        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtAddress, txtDescription, txtDescription, txtFullName, txtDescription, e);

        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(null, null, null, txtEmail, null, e);

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            verifyData.checkEmail(txtEmail);

        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtIdentification_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            verifyData.checkPhone(txtPhone);
        }

        private void txtIdentification_TextChanged(object sender, EventArgs e)
        {
            verifyData.checkIdentification(txtIdentification);
        }
    }
}
