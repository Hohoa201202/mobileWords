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
    public partial class frmReportRental : Form
    {
        public static string @RentalID = "";
        private DataServices myDataServices;

        public frmReportRental()
        {
            InitializeComponent();
        }

        private void frmReportRental_Load(object sender, EventArgs e)
        {
            myDataServices = new DataServices();
            string sSql = "exec tt_In_HDBanHang '" + @RentalID + "' ";
            DataSet ds = new DataSet();
            ds = myDataServices.RunQuery_Report(sSql, "Rental");

            this.reportViewer1.LocalReport.ReportEmbeddedResource = "MobileWords.rptRental.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = ds.Tables["Rental"];

            this.reportViewer1.LocalReport.DataSources.Add(rds);

            this.reportViewer1.RefreshReport();
        }
    }
}
