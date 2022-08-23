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
    public partial class frmAEditCategory : Form
    {
        private DataServices dsProduct;
        private DataTable dtProduct;
        //Biến kiếm tra thêm mới hay sửa
        private bool modeNew;
        private string _CategoryName;
        public frmAEditCategory()
        {
            InitializeComponent();
        }

        private void frmAEditCategory_Load(object sender, EventArgs e)
        {
            //Hiển thị dữ liệu
            Display();
            //thiết lập các nút ấn
            SetControls(false);
        }

        private void Display()
        {
            //Truy vấn dữ liệu
            string sSql = "Select * From tblCategories Order By CategoryName";
            dsProduct = new DataServices();
            dtProduct = dsProduct.RunQuery(sSql);

            //hiển lên lưới
            dataGridView1.DataSource = dtProduct;
        }

        //Hàm điều khiển trạng thái Enable của các điều khiển
        private void SetControls(bool edit)
        {
            txtCategoryName.Enabled = edit;
            txtDescription.Enabled = edit;

            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;

            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            modeNew = true;
            SetControls(true);
            //chuyển con trỏ về txtProductName
            txtCategoryName.Focus();
            //Xóa trắng các textBox
            txtCategoryName.Clear();
            txtDescription.Clear();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            modeNew = false;
            SetControls(true);
            //Chuyển con trỏ về txtCategoryName để nhập
            txtCategoryName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Hiển thị hộp thoại xác nhận chắc chắn xóa không?
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xoá dữ liệu không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            //Lấy dòng dữ liệu hiện thời đã chọn trên lưới
            int r = dataGridView1.CurrentRow.Index;
            dtProduct.Rows[r].Delete();
            dsProduct.Update(dtProduct);
            Display();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //1. Kiểm tra dữ liệu
            if (verifyData.checkInputSpace(txtCategoryName, "Tên loại mặt hàng không được để trống!") == false) return;
            if (verifyData.checkLength(txtCategoryName, 30, "Tên loại mặt hàng không được quá 30 kí tự!") == false) return;

            if (verifyData.checkLength(txtDescription, 250, "Mô tả thêm không được quá 250 kí tự!") == false) return;

            //Kiểm tra dữ liệu trùng khi <thêm mới> hoặc <sửa> tên loại sách
            if ((modeNew == true) || ((modeNew == false) && (txtCategoryName.Text != _CategoryName)))
            {
                //truy vấn dữ liệu và kiểm tra trùng
                //2. Kiểm tra trùng tên loại măt hàng
                string sSql = "Select CategoryName from tblCategories Where CategoryName = N'" + txtCategoryName.Text + "'";
                DataServices dsSearch = new DataServices();
                DataTable dtSearch = dsSearch.RunQuery(sSql);
                if (dtSearch.Rows.Count > 0)
                {
                    MessageBox.Show("Tên loại mặt hàng bị trùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCategoryName.Focus();
                    return;
                }
            }

            if (modeNew == true)
            {
                //3. Nhập dữ liệu vào bảng tblCategories
                //Khởi tạo một dòng Datarow mới
                DataRow myDataRow = dtProduct.NewRow();
                //Gán giá trị cho các cột
                myDataRow["CategoryName"] = txtCategoryName.Text;
                myDataRow["Description"] = txtDescription.Text;
                //Add giá trị vào 
                dtProduct.Rows.Add(myDataRow);
                //Update vào csdl
                dsProduct.Update(dtProduct);

                Display();
                SetControls(false);
            }
            else
            {
                //Sửa dữ liệu
                //1. Lấy dòng hiện thời cần sửa
                int r = dataGridView1.CurrentRow.Index;
                //2. tao 1 dòng dữ liệu
                DataRow myDataRow = dtProduct.Rows[r];
                //3. gán dữ liệu
                myDataRow["CategoryName"] = txtCategoryName.Text;
                myDataRow["Description"] = txtDescription.Text;
                //4. Câp nhật lại CSDL
                dsProduct.Update(dtProduct);
            }
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

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtCategoryName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            _CategoryName = txtCategoryName.Text;
        }

        private void txtCategoryName_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtDescription, txtDescription, txtDescription, txtDescription, txtDescription, e);
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtCategoryName, txtCategoryName, null, txtCategoryName, null, e);

        }
    }
}
