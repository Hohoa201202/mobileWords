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
    public partial class frmAEditTrademark : Form
    {
        private DataServices myDataServices;
        private DataTable myDataTable;
        private bool modeNew;  //Biến kiếm tra thêm mới hay sửa
        private string _TrademarkName; //Biến kiểm lưu tên thương hiệu để kiểm tra trùng
        public frmAEditTrademark()
        {
            InitializeComponent();
        }

        private void Display()
        {
            //Truy vấn dữ liệu
            string sSql = "Select * From tblTrademarks Order By TrademarkName";
            myDataServices = new DataServices();
            myDataTable = myDataServices.RunQuery(sSql);

            //hiển lên lưới
            dataGridView1.DataSource = myDataTable;
        }

        //Hàm điều khiển trạng thái Enable của các điều khiển
        private void SetControls(bool edit)
        {
            txtTrademarkName.Enabled = edit;
            txtDescription.Enabled = edit;

            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;

            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }

        private void frmAEditTrademark_Load(object sender, EventArgs e)
        {
            //Hiển thị dữ liệu
            Display();
            //thiết lập các nút ấn
            SetControls(false);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            modeNew = true;
            SetControls(true);
            //chuyển con trỏ về txtProductName
            txtTrademarkName.Focus();
            //Xóa trắng các textBox
            txtTrademarkName.Clear();
            txtDescription.Clear();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            modeNew = false;
            SetControls(true);
            //Chuyển con trỏ về txtCategoryName để nhập
            txtTrademarkName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Hiển thị hộp thoại xác nhận chắc chắn xóa không?
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xoá dữ liệu không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            //Lấy dòng dữ liệu hiện thời đã chọn trên lưới
            int r = dataGridView1.CurrentRow.Index;
            myDataTable.Rows[r].Delete();
            myDataServices.Update(myDataTable);
            Display();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //1. Kiểm tra dữ liệu
            if (verifyData.checkInputSpace(txtTrademarkName, "Tên thương hiệu không được để trống!") == false) return;
            if (verifyData.checkLength(txtTrademarkName, 30, "Tên thương hiệu không được quá 30 kí tự!") == false) return;

            if (verifyData.checkLength(txtDescription, 250, "Mô tả thêm không được quá 250 kí tự!") == false) return;

            //Kiểm tra dữ liệu trùng khi <thêm mới> hoặc <sửa> tên loại sách
            if ((modeNew == true) || ((modeNew == false) && (txtTrademarkName.Text != _TrademarkName)))
            {
                //truy vấn dữ liệu và kiểm tra trùng
                //2. Kiểm tra trùng tên loại măt hàng
                string sSql = "Select CategoryName from tblCategories Where CategoryName = N'" + txtTrademarkName.Text + "'";
                DataServices dsSearch = new DataServices();
                DataTable dtSearch = dsSearch.RunQuery(sSql);
                if (dtSearch.Rows.Count > 0)
                {
                    MessageBox.Show("Tên thương hiệu bị trùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTrademarkName.Focus();
                    return;
                }
            }

            if (modeNew == true)
            {
                //3. Nhập dữ liệu vào bảng tblCategories
                //Khởi tạo một dòng Datarow mới
                DataRow myDataRow = myDataTable.NewRow();
                //Gán giá trị cho các cột
                myDataRow["TrademarkName"] = txtTrademarkName.Text;
                myDataRow["Description"] = txtDescription.Text;
                //Add giá trị vào 
                myDataTable.Rows.Add(myDataRow);
                //Update vào csdl
                myDataServices.Update(myDataTable);

                Display();
                SetControls(false);
            }
            else
            {
                //Sửa dữ liệu
                //1. Lấy dòng hiện thời cần sửa
                int r = dataGridView1.CurrentRow.Index;
                //2. tao 1 dòng dữ liệu
                DataRow myDataRow = myDataTable.Rows[r];
                //3. gán dữ liệu
                myDataRow["TrademarkName"] = txtTrademarkName.Text;
                myDataRow["Description"] = txtDescription.Text;
                //4. Câp nhật lại CSDL
                myDataServices.Update(myDataTable);
            }
            groupBox1.Enabled = true;
            //Hiển thị lại dữ liệu sau khi thêm mới hoặc sửa
            Display();
            SetControls(false);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtTrademarkName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            _TrademarkName = txtTrademarkName.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            SetControls(false);
        }

        private void txtTrademarkName_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtDescription, txtDescription, txtDescription, txtDescription, txtDescription, e);
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtTrademarkName, txtTrademarkName, null, txtTrademarkName, null, e);

        }
    }
}
