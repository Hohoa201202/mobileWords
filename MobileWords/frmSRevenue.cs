using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace MobileWords
{
    public partial class frmSRevenue : Form
    {
        private int check = -1; //Biến kiểm tra kiểu lọc dữ liệu

        public frmSRevenue()
        {
            InitializeComponent();
        }
        private void Loading(string sSql, Label lbOutput, string C)
        {
            DataServices dsSum = new DataServices();
            DataTable dtSum = dsSum.RunQuery(sSql);
            if (dtSum.Rows.Count > 0)
            {
                if (dtSum.Rows[0][0].ToString() != "")
                {
                    lbOutput.Text = dtSum.Rows[0][0].ToString() + " " + C;
                }
                else
                {
                    lbOutput.Text = "0" + " " + C;
                }
            }
            else
            {
                lbOutput.Text = "0" + " " + C;
            }
            dtSum.Clear();
        }

        private void frmTKDoanhThu_Load(object sender, EventArgs e)
        {
            //string number = "100";
            //Truy vấn dữ liệu
            //string sSql = "EXEC tt_TonKho " + number;
            string sSql = "exec tt_BanChay";
            DataServices dsBanChay = new DataServices();
            DataTable dtBanChay = dsBanChay.RunQuery(sSql);
            //Hiển thị lên lưới
            dataGridViewBanChay.DataSource = dtBanChay;

            sSql = "exec tt_BanIt";
            DataServices dsBanIt = new DataServices();
            DataTable dtBanIt = dsBanIt.RunQuery(sSql);
            //Hiển thị lên lưới
            dataGridViewSPBanIt.DataSource = dtBanIt;

            sSql = "exec tt_ThuongHieu";
            DataServices dsThuongHieu = new DataServices();
            DataTable dtThuongHieu = dsThuongHieu.RunQuery(sSql);
            //Hiển thị lên lưới
            dataGridViewThuongHieu.DataSource = dtThuongHieu;

            //Đếm tổng số lượng khách hàng
            string sSql1 = "Select count(*) from tblCustomers";
            Loading(sSql1, lbSumKH, "");

            //Đếm tổng số lượng nhà cung cấp
            sSql1 = "Select count(*) from tblSuppliers";
            Loading(sSql1, lbSumNcc, "");

            //Đếm tổng số lượng sản phẩm
            sSql1 = "Select count(*) from tblProducts";
            Loading(sSql1, lbSumSp, "");

            //Đếm tổng số thương hiệu
            sSql1 = "Select count(*) from tblTrademarks";
            Loading(sSql1, lbSumThuongHieu, "");

            //Thống kê doanh thu
            //Đếm tổng số lượng bán hàng
            sSql1 = "select sum(Quantity) from tblRentalDetails";
            Loading(sSql1, lbSumBan, "");

            //Đếm tổng doanh thu
            sSql1 = "select sum(p.Price*rd.Quantity) from tblRentalDetails rd  inner join tblProducts p on p.ProductID = rd.ProductID";
            Loading(sSql1, lbSumDoanhThu, "VND");

            //Đếm tổng Lợi nhuận
            sSql1 = "exec tt_LoiNhuan";
            Loading(sSql1, lbLoiNhuan, "VND");

            //Không cho điều chỉnh ngày ở dtmStart và dtmEnd
            pnCustom.Enabled = false;
        }

        private void setBF(Button b1, Button b2, Button b3, Button b4, Button b5)
        {
            b1.BackColor = Color.FromArgb(42, 45, 86);
            b1.ForeColor = Color.White;

            b2.BackColor = b3.BackColor = b4.BackColor = b5.BackColor = Color.White;
            b2.ForeColor = b3.ForeColor = b4.ForeColor = b5.ForeColor = Color.FromArgb(64, 64, 64);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            //Lấy ngày tháng năm hiện tại.
            string YearNow = DateTime.Now.Year.ToString();
            string MonthNow = DateTime.Now.Month.ToString();
            string DayNow = DateTime.Now.Day.ToString();

            //Tách ngày tháng năm từ dtmStart và dtmEnd
            string YearS = dtmStart.Value.Year.ToString(), MonthS = dtmStart.Value.Month.ToString(), DayS = dtmStart.Value.Day.ToString();
            string YearE = dtmEnd.Value.Year.ToString(), MonthE = dtmEnd.Value.Month.ToString(), DayE = dtmEnd.Value.Day.ToString();

            if (check == 5)//Thống kê doanh thu tháng này
            {
                //Đếm tổng số lượng bán hàng
                string sSql = "exec tt_SumBan '" + YearNow + "-" + MonthNow + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbSumBan, "");

                //Đếm tổng doanh thu
                sSql = "exec tt_SumDoanhThu '" + YearNow + "-" + MonthNow + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbSumDoanhThu, "VND");

                //Đếm tổng Lợi nhuận
                sSql = "exec tt_SumLoiNhuan '" + YearNow + "-" + MonthNow + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbLoiNhuan, "VND");

                sSql = "exec tt_BanChay '" + YearNow + "-" + MonthNow + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsBanChay = new DataServices();
                DataTable dtBanChay = dsBanChay.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewBanChay.DataSource = dtBanChay;

                sSql = "exec tt_BanIt '" + YearNow + "-" + MonthNow + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsBanIt = new DataServices();
                DataTable dtBanIt = dsBanIt.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewSPBanIt.DataSource = dtBanIt;

                sSql = "exec tt_ThuongHieu '" + YearNow + "-" + MonthNow + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsThuongHieu = new DataServices();
                DataTable dtThuongHieu = dsThuongHieu.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewThuongHieu.DataSource = dtThuongHieu;
            }
            else if (check == 4)//Thống kê doanh thu của 30 ngày trước.
            {
                //Đếm tổng số lượng bán hàng
                string sSql = "exec tt_SumBan '" + YearNow + "-" + ((DateTime.Now.Month) - 1).ToString() + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbSumBan, "");

                //Đếm tổng doanh thu DateTime.Now.AddMonths(-1).ToString()
                sSql = "exec tt_SumDoanhThu '" + YearNow + "-" + ((DateTime.Now.Month) - 1).ToString() + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbSumDoanhThu, "VND");

                //Đếm tổng Lợi nhuận
                sSql = "exec tt_SumLoiNhuan '" + YearNow + "-" + ((DateTime.Now.Month) - 1).ToString() + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbLoiNhuan, "VND");

                sSql = "exec tt_BanChay '" + YearNow + "-" + ((DateTime.Now.Month) - 1).ToString() + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsBanChay = new DataServices();
                DataTable dtBanChay = dsBanChay.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewBanChay.DataSource = dtBanChay;

                sSql = "exec tt_BanIt '" + YearNow + "-" + ((DateTime.Now.Month) - 1).ToString() + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsBanIt = new DataServices();
                DataTable dtBanIt = dsBanIt.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewSPBanIt.DataSource = dtBanIt;

                sSql = "exec tt_ThuongHieu '" + YearNow + "-" + ((DateTime.Now.Month) - 1).ToString() + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsThuongHieu = new DataServices();
                DataTable dtThuongHieu = dsThuongHieu.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewThuongHieu.DataSource = dtThuongHieu;
            }
            else if (check == 3)//Thống kê doanh thu của năm nay
            {
                //Đếm tổng số lượng bán hàng
                string sSql = "exec tt_SumBan '" + YearNow + "-" + "01" + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbSumBan, "");

                //Đếm tổng doanh thu
                sSql = "exec tt_SumDoanhThu '" + YearNow + "-" + "01" + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbSumDoanhThu, "VND");

                //Đếm tổng Lợi nhuận
                sSql = "exec tt_SumLoiNhuan '" + YearNow + "-" + "01" + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbLoiNhuan, "VND");

                sSql = "exec tt_BanChay '" + YearNow + "-" + "01" + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsBanChay = new DataServices();
                DataTable dtBanChay = dsBanChay.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewBanChay.DataSource = dtBanChay;

                sSql = "exec tt_BanIt '" + YearNow + "-" + "01" + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsBanIt = new DataServices();
                DataTable dtBanIt = dsBanIt.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewSPBanIt.DataSource = dtBanIt;

                sSql = "exec tt_ThuongHieu '" + YearNow + "-" + "01" + "-" + "01" + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsThuongHieu = new DataServices();
                DataTable dtThuongHieu = dsThuongHieu.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewThuongHieu.DataSource = dtThuongHieu;
            }
            else if (check == 2) //Thống kê doanh thu ngày hiện tại
            {
                //Đếm tổng số lượng bán hàng
                string sSql = "exec tt_SumBan '" + YearNow + "-" + MonthNow + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbSumBan, "");

                //Đếm tổng doanh thu
                sSql = "exec tt_SumDoanhThu '" + YearNow + "-" + MonthNow + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbSumDoanhThu, "VND");

                //Đếm tổng Lợi nhuận
                sSql = "exec tt_SumLoiNhuan '" + YearNow + "-" + MonthNow + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                Loading(sSql, lbLoiNhuan, "VND");

                sSql = "exec tt_BanChay '" + YearNow + "-" + MonthNow + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsBanChay = new DataServices();
                DataTable dtBanChay = dsBanChay.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewBanChay.DataSource = dtBanChay;

                sSql = "exec tt_BanIt '" + YearNow + "-" + MonthNow + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsBanIt = new DataServices();
                DataTable dtBanIt = dsBanIt.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewSPBanIt.DataSource = dtBanIt;

                sSql = "exec tt_ThuongHieu '" + YearNow + "-" + MonthNow + "-" + DayNow + "', '" + YearNow + "-" + MonthNow + "-" + DayNow + "'";
                DataServices dsThuongHieu = new DataServices();
                DataTable dtThuongHieu = dsThuongHieu.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewThuongHieu.DataSource = dtThuongHieu;
            }
            else if (check == 1) //Thống kê doanh thu ngày tùy chỉnh
            {
                //Đếm tổng số lượng bán hàng
                string sSql = "exec tt_SumBan '" + YearS + "-" + MonthS + "-" + DayS + "', '" + YearE + "-" + MonthE + "-" + DayE + "'";
                Loading(sSql, lbSumBan, "");

                //Đếm tổng doanh thu
                sSql = "exec tt_SumDoanhThu '" + YearS + "-" + MonthS + "-" + DayS + "', '" + YearE + "-" + MonthE + "-" + DayE + "'";
                Loading(sSql, lbSumDoanhThu, "VND");

                //Đếm tổng Lợi nhuận
                sSql = "exec tt_SumLoiNhuan '" + YearS + "-" + MonthS + "-" + DayS + "', '" + YearE + "-" + MonthE + "-" + DayE + "'";
                Loading(sSql, lbLoiNhuan, "VND");

                sSql = "exec tt_BanChay '" + YearS + "-" + MonthS + "-" + DayS + "', '" + YearE + "-" + MonthE + "-" + DayE + "'";
                DataServices dsBanChay = new DataServices();
                DataTable dtBanChay = dsBanChay.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewBanChay.DataSource = dtBanChay;

                sSql = "exec tt_BanIt '" + YearS + "-" + MonthS + "-" + DayS + "', '" + YearE + "-" + MonthE + "-" + DayE + "'";
                DataServices dsBanIt = new DataServices();
                DataTable dtBanIt = dsBanIt.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewSPBanIt.DataSource = dtBanIt;

                sSql = "exec tt_ThuongHieu '" + YearS + "-" + MonthS + "-" + DayS + "', '" + YearE + "-" + MonthE + "-" + DayE + "'";
                DataServices dsThuongHieu = new DataServices();
                DataTable dtThuongHieu = dsThuongHieu.RunQuery(sSql);
                //Hiển thị lên lưới
                dataGridViewThuongHieu.DataSource = dtThuongHieu;
            }
        }

        private void btnThisMonth_Click(object sender, EventArgs e)
        {
            check = 5;
            pnCustom.Enabled = false;
            setBF(btnThisMonth, btnLast30, btnThisYear, btnToday, btnCustom);
        }
        private void btnLast30_Click(object sender, EventArgs e)
        {
            check = 4;
            pnCustom.Enabled = false;
            setBF(btnLast30, btnThisMonth, btnThisYear, btnToday, btnCustom);
        }

        private void btnThisYear_Click(object sender, EventArgs e)
        {
            check = 3;
            pnCustom.Enabled = false;
            setBF(btnThisYear, btnThisMonth, btnLast30, btnToday, btnCustom);
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            check = 2;
            pnCustom.Enabled = false;
            setBF(btnToday, btnThisMonth, btnLast30, btnThisYear, btnCustom);
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            check = 1;
            pnCustom.Enabled = true;
            setBF(btnCustom, btnThisMonth, btnLast30, btnThisYear, btnToday);
        }
    }
}
