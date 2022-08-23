using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;

namespace MobileWords
{
    public partial class frmReportOrder : Form
    {
        public static string @OrderID = "";
        private DataServices myDataServices;

        public frmReportOrder()
        {
            InitializeComponent();
        }

        private void frmReportOrder_Load(object sender, EventArgs e)
        {
            myDataServices = new DataServices();
            string sSql = "exec tt_In_HDNhapHang '" + @OrderID + "' ";
            DataSet ds = new DataSet();
            ds = myDataServices.RunQuery_Report(sSql, "Order");

            this.reportViewer1.LocalReport.ReportEmbeddedResource = "MobileWords.rptOrder.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = ds.Tables["Order"];

            this.reportViewer1.LocalReport.DataSources.Add(rds);

            this.reportViewer1.RefreshReport();
        }
    }
}
