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
    public partial class frmEditProduct : Form
    {
        private DataServices dsProduct;
        private DataTable dtProduct;
        string _ProductName; //Biến lưu tên sản phẩm để kiểm tra trùng
        public frmEditProduct()
        {
            InitializeComponent();
        }

        private void SetControls(bool edit)
        {
            txtProductName.Enabled = edit;
            cboCategoryID.Enabled = edit;
            cboTrademarkID.Enabled = edit;
            txtMadeIn.Enabled = edit;
            txtGuaranteeDay.Enabled = edit;
            txtYear.Enabled = edit;
            txtColor.Enabled = edit;
            txtPrice.Enabled = edit;
            txtDescription.Enabled = edit;
            //cacs nut
            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;
            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }
        private void Display()
        {
            //Truy vấn dữ liệu
            string sSql = "Select * From tblProducts Order By ProductName";
            dsProduct = new DataServices();
            dtProduct = dsProduct.RunQuery(sSql);

            //hiển lên lưới
            dataGridProduct.DataSource = dtProduct;
        }


        private void frmEditProduct_Load(object sender, EventArgs e)
        {
            //1. Chuyển dữ liệu vào cboCategoryID
            //1.1. Truy vấn dữ liệu từ bảng Category
            string sSql = "Select * From tblCategories Order By CategoryName";
            DataServices dsCategory = new DataServices();
            DataTable dtCategory = dsCategory.RunQuery(sSql);
            //1.2. Chuyển dữ liệu vào cboCategory
            cboCategoryID.DataSource = dtCategory;
            cboCategoryID.DisplayMember = "CategoryName";
            cboCategoryID.ValueMember = "CategoryID";

            //2. Chuyển dữ liệu vào cboTrademarkID
            //2.1. Truy vấn dữ liệu từ bảng TrademarkID
            sSql = "Select * From tblTrademarks Order By TrademarkName";
            DataServices dsTrademarkID = new DataServices();
            DataTable dtTrademarkID = dsTrademarkID.RunQuery(sSql);
            //2.2. Chuyển dữ liệu vào cboPublisher
            cboTrademarkID.DataSource = dtTrademarkID;
            cboTrademarkID.DisplayMember = "TrademarkName";
            cboTrademarkID.ValueMember = "TrademarkID";

            //Hiển thị dữ liệu
            Display();
            //thiết lập các nút ấn
            SetControls(false);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            SetControls(true);
            //Chuyển con trỏ về txtFullName để nhập
            txtProductName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //1. Kiểm tra dữ liệu
            if (verifyData.checkInputSpace(txtProductName, "Tên sản phẩm không được để trống!") == false) return;
            if (verifyData.checkLength(txtProductName, 50, "Tên sản phẩm không được quá 50 kí tự!") == false) return;

            if (verifyData.checkInputSpace(txtColor, "Màu sắc không được để trống!") == false) return;
            if (verifyData.checkLength(txtColor, 50, "Màu sắc không được quá 50 kí tự!") == false) return;

            if (verifyData.checkInputSpace(txtPrice, "Giá bán không được để trống!") == false) return;

            if (verifyData.checkInputSpace(txtYear, "Thời điểm ra mắt không được để trống!") == false) return;
            if (verifyData.checkLength(txtYear, 10, "Thời điểm ra mắt không được quá 10 kí tự!") == false) return;

            if (verifyData.checkInputSpace(txtGuaranteeDay, "Thời gian bảo hành không được để trống!") == false) return;

            if (verifyData.checkInputSpace(txtMadeIn, "Xuất xứ không được để trống!") == false) return;
            if (verifyData.checkLength(txtMadeIn, 30, "Xuất xứ không được quá 30 kí tự!") == false) return;

            if (verifyData.checkLength(txtDescription, 250, "Mô tả thêm không được quá 250 kí tự!") == false) return;


            string sSql;
            DataServices dsSearch = new DataServices();
            DataTable dtSearch;
            //2. Kiểm tra trùng tên sản phẩm
            if (txtProductName.Text != _ProductName)
            {
                sSql = "Select ProductName from tblProducts Where ProductName = N'" + txtProductName.Text + "'";
                dtSearch = dsSearch.RunQuery(sSql);
                if (dtSearch.Rows.Count > 0)
                {
                    MessageBox.Show("Tên sản phẩm bị trùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtProductName.Focus();
                    return;
                }
            }
            //3.Sửa dữ liệu
            //Lấy dòng hiện thời cần sửa
            int r = dataGridProduct.CurrentRow.Index;
            //Tạo 1 dòng dữ liệu
            DataRow myDataRow = dtProduct.Rows[r];
            //Gán lại dữ liệu vừa sửa
            myDataRow["ProductName"] = txtProductName.Text;
            myDataRow["CategoryID"] = cboCategoryID.SelectedValue;
            myDataRow["TrademarkID"] = cboTrademarkID.SelectedValue;
            myDataRow["Year"] = txtYear.Text;
            myDataRow["GuaranteeDay"] = txtGuaranteeDay.Text;
            myDataRow["MadeIn"] = txtMadeIn.Text;
            myDataRow["Price"] = txtPrice.Text;
            myDataRow["Description"] = txtDescription.Text;
            //Cập nhật lại csdl bảng tblProducts
            dsProduct.Update(dtProduct);

            //4. Đưa dữ liệu vào bảng tblColors
            for (int i = 0; i < txtColor.Lines.Count(); i++)
            {
                //Nhập từng dòng vào bảng tblColors, nếu Color đã tồn tại trong bảng tblColor thì không nhập.
                string Color = txtColor.Lines[i].Trim();
                sSql = "Select Color from tblColors Where Color = N'" + Color + "'";
                dsSearch = new DataServices();
                dtSearch = dsSearch.RunQuery(sSql);
                if (dtSearch.Rows.Count < 1)
                {
                    sSql = "Insert Into tblColors (Color) Values (N'" + Color + "')";
                    DataServices dsColor = new DataServices();
                    dsColor.ExecuteNonQuery(sSql);
                }
            }

            //5. Nhập dữ liệu vào bảng tblProductColors
            //5.1. Lấy ProductID vừa nhập từ bảng tblProducts
            sSql = "Select ProductID from tblProducts Where ProductName = N'" + txtProductName.Text + "'";
            dtSearch = dsSearch.RunQuery(sSql);
            string ProductID = dtSearch.Rows[0]["ProductID"].ToString();

            //5.2. Lấy các ColorID trong bảng tblColors
            for (int i = 0; i < txtColor.Lines.Count(); i++)
            {
                string Color = txtColor.Lines[i].Trim();
                sSql = "Select ColorID from tblColors Where Color = N'" + Color + "'";
                DataServices dsColor = new DataServices();
                DataTable dtColor = dsColor.RunQuery(sSql);
                string ColorID = dtColor.Rows[0]["ColorID"].ToString();

                //Kiểm tra nếu chưa tồn tại cặp ProductID và ColorID trong bảng tblProductColors thì Insert vào.
                sSql = "Select * from tblProductColors Where ColorID = " + ColorID + " and ProductID = " + ProductID;
                dtSearch.Clear();
                dtSearch = dsSearch.RunQuery(sSql);
                if (dtSearch.Rows.Count < 1)
                {
                    //Đưa ProductID và ColorID vào bảng tblProductColors
                    sSql = "Insert Into tblProductColors (ProductID, ColorID) Values (" + ProductID + "," + ColorID + ")";
                    dsColor.ExecuteNonQuery(sSql);
                }
            }

            groupBox1.Enabled = true;
            Display();
            SetControls(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            Display();
            SetControls(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Hiển thị hộp thoại xác nhận chắc chắn xóa không?
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xoá dữ liệu không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            //Lấy dòng dữ liệu hiện thời đã chọn trên lưới
            int r = dataGridProduct.CurrentRow.Index;
            //2. Lấy mã ProductID của dòng hiện thời
            string ProductID = dataGridProduct.Rows[r].Cells["ProductID"].Value.ToString();

            string sSql = "exec Delete_tblProducts " + ProductID;
            DataServices dsDelete = new DataServices();
            dsDelete.ExecuteNonQuery(sSql);
            Display();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (verifyData.checkInputSpace(txtSearch, "Bạn cần nhập nội dung muốn tìm kiếm!") == false) return;
            if (verifyData.checkLength(txtSearch, 100, "Nội dung tìm kiếm không được quá 100 kí tự!") == false) return;

            //Truy vấn dữ liệu
            string sSql = "";
            if (rbProductName.Checked == true)
                sSql = "Select * From tblProducts where ProductName like N'%" + txtSearch.Text + "%' Order By ProductName";
            else if (rbCategory.Checked == true)
                sSql = "select * from tblProducts inner join tblCategories on tblCategories.CategoryID = tblProducts.CategoryID where tblCategories.CategoryName LIke N'%" + txtSearch.Text + "%'";
            else if (rbTrademark.Checked == true)
                sSql = "select * from tblProducts inner join tblTrademarks on tblTrademarks.TrademarkID = tblProducts.TrademarkID  where tblTrademarks.TrademarkName LIke N'%" + txtSearch.Text + "%'";
            dsProduct = new DataServices();
            dtProduct = dsProduct.RunQuery(sSql);

            //Hiển thị lên lưới
            dataGridProduct.DataSource = dtProduct;
        }

        private void dataGridProduct_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cboCategoryID.SelectedValue = dataGridProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtProductName.Text = dataGridProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
            cboTrademarkID.SelectedValue = dataGridProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtYear.Text = dataGridProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtGuaranteeDay.Text = dataGridProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtMadeIn.Text = dataGridProduct.Rows[e.RowIndex].Cells[6].Value.ToString();
            txtPrice.Text = dataGridProduct.Rows[e.RowIndex].Cells[7].Value.ToString();
            txtDescription.Text = dataGridProduct.Rows[e.RowIndex].Cells[8].Value.ToString();

            //Hiển thị màu sắc
            string sSql = "select c.Color from tblColors c "; //chú ý có dấu cách cuối
            sSql = sSql + "inner join tblProductColors pc on pc.ColorID = c.ColorID ";
            sSql = sSql + "inner join tblProducts p on p.ProductID = pc.ProductID where p.ProductName = N'" + txtProductName.Text + "'";
            DataServices dsColor = new DataServices();
            DataTable dtColor = dsColor.RunQuery(sSql);
            //Hiển thị lên txtColor
            txtColor.Clear();
            for (int i = 0; i < dtColor.Rows.Count; i++)
            {
                // Environment.NewLine - xuống dòng
                txtColor.Text = txtColor.Text + dtColor.Rows[i]["Color"].ToString() + Environment.NewLine;
            }
            _ProductName = txtProductName.Text;
        }

        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(null, txtYear, txtColor, null, txtColor, e);

        }

        private void txtColor_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtProductName, txtPrice, txtPrice, txtProductName, null, e);

        }

        private void txtPrice_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtProductName, txtDescription, txtYear, txtColor, txtYear, e);

        }

        private void txtYear_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtPrice, txtGuaranteeDay, txtGuaranteeDay, txtProductName, txtGuaranteeDay, e);

        }

        private void txtGuaranteeDay_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtYear, txtMadeIn, txtMadeIn, txtProductName, txtMadeIn, e);

        }

        private void txtMadeIn_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtGuaranteeDay, txtDescription, txtDescription, txtPrice, txtDescription, e);

        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            verifyData.KeyDown_Up_Right_Down_Left_Enter(txtMadeIn, null, null, txtPrice, null, e);

        }
    }
}
