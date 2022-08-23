﻿using System;
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
    public partial class frmReportInsurance : Form
    {
        public static string @InsuranceID = "";
        private DataServices myDataServices;

        public frmReportInsurance()
        {
            InitializeComponent();
        }

        private void frmReportInsurance_Load(object sender, EventArgs e)
        {
            myDataServices = new DataServices();
            string sSql = "exec tt_In_HDBaoHanh '" + @InsuranceID + "' ";
            DataSet ds = new DataSet();
            ds = myDataServices.RunQuery_Report(sSql, "Insurance");

            this.reportViewer1.LocalReport.ReportEmbeddedResource = "MobileWords.rptInsurance.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = ds.Tables["Insurance"];

            this.reportViewer1.LocalReport.DataSources.Add(rds);

            this.reportViewer1.RefreshReport();
        }
    }
}