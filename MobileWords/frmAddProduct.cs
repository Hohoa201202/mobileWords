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
    public partial class frmAddProduct : Form
    {
        private DataServices dsProduct;
        private DataTable dtProduct;
        public frmAddProduct()
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
            btnNew.Enabled = !edit;
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


        private void frmAddProduct_Load(object sender, EventArgs e)
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            //Xóa trắng các textBox
            txtProductName.Clear();
            txtColor.Clear();
            txtYear.Clear();
            txtMadeIn.Clear();
            txtGuaranteeDay.Clear();
            txtPrice.Clear();
            txtDescription.Clear();

            groupBox1.Enabled = false;
            SetControls(true);
            //chuyển con trỏ về txtProductName
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

            //2. Kiểm tra trùng tên sản phẩm
            string sSql = "Select ProductName from tblProducts Where ProductName = N'" + txtProductName.Text + "'";
            DataServices dsSearch = new DataServices();
            DataTable dtSearch = dsSearch.RunQuery(sSql);
            if (dtSearch.Rows.Count > 0)
            {
                MessageBox.Show("Tên sản phẩm bị trùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtProductName.Focus();
                return;
            }
            //3. Nhập dữ liệu vào bảng Books
            DataRow myDataRow = dtProduct.NewRow();
            myDataRow["ProductName"] = txtProductName.Text;
            myDataRow["CategoryID"] = cboCategoryID.SelectedValue;
            myDataRow["TrademarkID"] = cboTrademarkID.SelectedValue;
            myDataRow["Year"] = txtYear.Text;
            myDataRow["GuaranteeDay"] = txtGuaranteeDay.Text;
            myDataRow["MadeIn"] = txtMadeIn.Text;
            myDataRow["Price"] = txtPrice.Text;
            myDataRow["Description"] = txtDescription.Text;
            dtProduct.Rows.Add(myDataRow);
            dsProduct.Update(dtProduct);

            //4. Đưa dữ liệu vào bảng tblColors
            for (int i = 0; i < txtColor.Lines.Count(); i++)
            {
                //Nhập từng dòng vào bảng tblColors, nếu Color đã tồn tại trong bảng tblColor thì không nhập.
                string Color = txtColor.Lines[i].Trim();
                sSql = "Select Color from tblColors Where Color = N'" + Color + "'";
                dsSearch = new DataServices();
                dtSearch.Clear();
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
                //Đưa ProductID và ColorID vào bảng tblProductColors
                sSql = "Insert Into tblProductColors (ProductID, ColorID) Values (" + ProductID + "," + ColorID + ")";
                dsColor.ExecuteNonQuery(sSql);
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

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtGuaranteeDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
