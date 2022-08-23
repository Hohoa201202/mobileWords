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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void txtUserName_Enter(object sender, EventArgs e)
        {
            if (txtUserName.Text == "Tên đăng nhập")
            {
                txtUserName.Clear();
            }
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
                txtUserName.Text = "Tên đăng nhập";
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Mật khẩu")
            {
                txtPassword.Clear();
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "Mật khẩu";
            }
        }

        private void iconminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Kiểm tra username và password trong  bảng User
            //Mở kết nối tới CSDL
            DataServices myDataServices = new DataServices();
            if (myDataServices.OpenDB("localhost", "MobileWords", "sa", "sa123") == false) return;
            //Kiểm tra username và password trong  bảng User
            string sSql = "Select * From tblUsers Where (UserName = N'" + txtUserName.Text + "')AND (Password = N'" + txtPassword.Text + "')";
            //Truy vấn dữ liệu
            DataTable dtUser = myDataServices.RunQuery(sSql);
            if (dtUser.Rows.Count == 0)
            {
                MessageBox.Show("Không đúng tên hoặc mật khẩu truy nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserName.Focus();
                return;
            }
            //Nếu đúng thì gọi hàm main
            frmMain MyfrmMain = new frmMain();
            MyfrmMain.ShowDialog();
        }

        private void ckbPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
            }
        }
    }
}
