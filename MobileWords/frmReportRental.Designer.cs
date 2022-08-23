
namespace MobileWords
{
    partial class frmReportRental
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MobileWordsDataSet = new MobileWords.MobileWordsDataSet();
            this.tt_In_HDBanHangBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tt_In_HDBanHangTableAdapter = new MobileWords.MobileWordsDataSetTableAdapters.tt_In_HDBanHangTableAdapter();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.MobileWordsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tt_In_HDBanHangBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // MobileWordsDataSet
            // 
            this.MobileWordsDataSet.DataSetName = "MobileWordsDataSet";
            this.MobileWordsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tt_In_HDBanHangBindingSource
            // 
            this.tt_In_HDBanHangBindingSource.DataMember = "tt_In_HDBanHang";
            this.tt_In_HDBanHangBindingSource.DataSource = this.MobileWordsDataSet;
            // 
            // tt_In_HDBanHangTableAdapter
            // 
            this.tt_In_HDBanHangTableAdapter.ClearBeforeFill = true;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(687, 715);
            this.reportViewer1.TabIndex = 0;
            // 
            // frmReportRental
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 715);
            this.Controls.Add(this.reportViewer1);
            this.Name = "frmReportRental";
            this.Text = "IN HÓA ĐƠN BÁN HÀNG";
            this.Load += new System.EventHandler(this.frmReportRental_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MobileWordsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tt_In_HDBanHangBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource tt_In_HDBanHangBindingSource;
        private MobileWordsDataSet MobileWordsDataSet;
        private MobileWordsDataSetTableAdapters.tt_In_HDBanHangTableAdapter tt_In_HDBanHangTableAdapter;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}