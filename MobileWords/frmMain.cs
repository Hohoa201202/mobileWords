using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MobileWords
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        //add form con vào panel tại form chính (panelDestop)
        private void openChildForm(object childForm)
        {
            if (this.panelDestop.Controls.Count > 0)
                this.panelDestop.Controls.RemoveAt(0);
            Form cf = childForm as Form;
            cf.TopLevel = false;
            cf.FormBorderStyle = FormBorderStyle.None;
            cf.Dock = DockStyle.Fill;
            cf.AutoSize = true;
            this.panelDestop.Controls.Add(cf);
            this.panelDestop.Tag = cf;
            cf.BringToFront();
            cf.Show();
            lbHome.Text = cf.Text;
        }

        // Ẩn submenu
        private void hideSubMenu()
        {
            if (panelQLSanPham.Visible == true)
                panelQLSanPham.Visible = false;
            if (panelBanHang.Visible == true)
                panelBanHang.Visible = false;
            if (panelDVBaoHanh.Visible == true)
                panelDVBaoHanh.Visible = false;
            if (panelNhapHang.Visible == true)
                panelNhapHang.Visible = false;
            if (panelQLNhanVien.Visible == true)
                panelQLNhanVien.Visible = false;
            if (panelThongKe.Visible == true)
                panelThongKe.Visible = false;
        }

        // Show subMenu
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        //
        private void SetTextName()
        {
            btnMenuBanHang.Text = "Bán hàng";
            btnMenuNhapHang.Text = "Nhập hàng";
            btnMenuQLSanPham.Text = "Quản lý sản phẩm";
            btnQLNhanVien.Text = "Quản lý nhân viên";
            btnDVBaoHanh.Text = "Dịch vụ bảo hành";
            btnThongKe.Text = "Thống kê, báo cáo";
            btnLogout.Text = "Đăng xuất";
            MenuVertical.Width = 250;
        }

        private void customizeDesing()
        {
            panelQLSanPham.Visible = false;
            panelBanHang.Visible = false;
            panelDVBaoHanh.Visible = false;
            panelNhapHang.Visible = false;
            panelQLNhanVien.Visible = false;
            panelThongKe.Visible = false;
        }


        //Ẩn/hiện chi tiết menu
        private void btnMenu_Click(object sender, EventArgs e)
        {

            if (MenuVertical.Width == 250)
            {
                customizeDesing();
                btnMenuBanHang.Text = btnMenuNhapHang.Text = btnMenuQLSanPham.Text = btnQLNhanVien.Text = btnDVBaoHanh.Text = btnThongKe.Text = btnLogout.Text = "";
                MenuVertical.Width = 65;
            }
            else
            {
                //btnMenu.Image = Image.FromFile("close.png");
                //btnMenu.Image = ((System.Drawing.Image)(resources.GetObject("btnMenuBanHang.Image")))
                SetTextName();
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

        private void btnlogoInicio_Click(object sender, EventArgs e)
        {
            openChildForm(new frmHome());
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            btnlogoInicio_Click(null, e);
            customizeDesing();
            btnMenuBanHang.Text = btnMenuNhapHang.Text = btnMenuQLSanPham.Text = btnQLNhanVien.Text = btnDVBaoHanh.Text = btnThongKe.Text = btnLogout.Text = "";
            MenuVertical.Width = 65;
        }

        private void btnMenuNhapHang_Click(object sender, EventArgs e)
        {

            showSubMenu(panelNhapHang);
            SetTextName();
        }

        private void btnMenuQLSanPham_Click(object sender, EventArgs e)
        {
            showSubMenu(panelQLSanPham);
            SetTextName();
        }

        private void btnMenuBanHang_Click(object sender, EventArgs e)
        {
            showSubMenu(panelBanHang);
            SetTextName();
        }

        private void btnQLNhanVien_Click(object sender, EventArgs e)
        {
            showSubMenu(panelQLNhanVien);
            SetTextName();
        }

        private void btnDVBaoHanh_Click(object sender, EventArgs e)
        {

            showSubMenu(panelDVBaoHanh);
            SetTextName();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            showSubMenu(panelThongKe);
            SetTextName();
        }

        private void btnSubMenuTSXNhanVien_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAEditUsers());
        }

        private void btnSubMenuAddSP_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAddProduct());
        }

        private void btnSubMenuTSXLoaiMH_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAEditCategory());
            //
        }

        private void btnSubMenuSXSanPham_Click(object sender, EventArgs e)
        {
           openChildForm(new frmEditProduct());
        }

        private void btnSubMenuTSXThuongHieu_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAEditTrademark());
        }

        private void btnSubMenuHDNH_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAEditOrder());
        }

        private void btnSubMenuAddNCC_Click(object sender, EventArgs e)
        {
             openChildForm(new frmAddSupplier());
        }

        private void btnSubMenuSXNCC_Click(object sender, EventArgs e)
        {
             openChildForm(new frmEditSupplier());
        }

        private void btnSubMenuHDBH_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAEditRental());
        }

        private void btnSubMenuAddKhachHang_Click(object sender, EventArgs e)
        {
             openChildForm(new frmAddCustomer());
        }

        private void btnEditKhachHang_Click(object sender, EventArgs e)
        {
           openChildForm(new frmEditCustomer());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAEditInsurance());
        }

        private void btnSubMenuTKDoanhThu_Click(object sender, EventArgs e)
        {
            openChildForm(new frmSRevenue());
        }

        private void btnSubMenuTonKho_Click(object sender, EventArgs e)
        {
            openChildForm(new frmSInventory());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmReport());

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn chắc chắn muốn đăng xuất khỏi hệ thống ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            this.Close();
        }
    }
}
