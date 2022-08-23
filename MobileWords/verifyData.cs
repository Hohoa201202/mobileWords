using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileWords
{
    class verifyData
    {

        static public string RentalID = ""; //Biến lưu số phiếu xuất khi chọn phiếu
        static public string OrderID = ""; //Biến lưu số phiếu nhập khi chọn phiếu
        static public string InsuranceID = ""; //Biến lưu số phiếu bảo hành khi chọn phiếu
        //Kiểm tra dữ liệu phải khác null
        public static bool checkInputSpace(TextBox txtInput, string str)
        {
            if (txtInput.Text.Trim() == "")
            {
                MessageBox.Show(str, "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInput.Focus();
                return false;
            }
            return true;
        }

        //Kiểm tra số ký tự 
        public static bool checkLength(TextBox txtInput, int Length, string str)
        {
            if (txtInput.Text.Length > Length)
            {
                MessageBox.Show(str, "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInput.Clear();
                txtInput.Focus();
                return false;
            }
            return true;
        }

        //Hàm kiểm tra Email
        public static bool checkEmail(TextBox txtInput)
        {
            string email = txtInput.Text.Trim();
            Regex regex = new Regex(@"^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-za-za-z]{2,3})+$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                txtInput.ForeColor = Color.Black;
            }
            else
            {
                txtInput.ForeColor = Color.Red;
                return false;
            }
            return true;
        }

        //Hàm kiểm tra Phone
        public static bool checkPhone(TextBox txtInput)
        {
            if (txtInput.Text.Trim().Length == 10)
            {
                txtInput.ForeColor = Color.Black;
                return true;
            }
            txtInput.ForeColor = Color.Red;
            txtInput.Text.Trim();
            return false;
        }

        //Hàm kiểm tra CMND/CCCD
        public static bool checkIdentification(TextBox txtInput)
        {
            if (txtInput.Text.Trim().Length == 9 || txtInput.Text.Trim().Length == 12)
            {
                txtInput.ForeColor = Color.Black;
                return true;
            }
            txtInput.ForeColor = Color.Red;
            return false;
        }

        ////////////////////////////////////////////////////////////
        public static void KeyDown_Up_Right_Down_Left_Enter(TextBox txtUpFocus, TextBox txtRightFocus, TextBox txtDownFocus, TextBox txtLeftFocus, TextBox txtEnter, KeyEventArgs e)
        {
            if (txtUpFocus != null)
            {
                if (e.KeyValue == 38)
                    txtUpFocus.Focus();
            }
            if (txtRightFocus != null)
            {
                if (e.KeyValue == 39)
                    txtRightFocus.Focus();
            }
            if (txtDownFocus != null)
            {
                if (e.KeyValue == 40)
                    txtDownFocus.Focus();
            }
            if (txtLeftFocus != null)
            {
                if (e.KeyValue == 37)
                    txtLeftFocus.Focus();
            }
            if (txtEnter != null)
            {
                if (e.KeyValue == 13)
                    txtEnter.Focus();
            }
        }
    }
}
