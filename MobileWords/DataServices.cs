using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//mở các thư viện
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace MobileWords
{
    class DataServices
    {

        private static SqlConnection mySqlConnection;
        private SqlDataAdapter myDataAdapter;
        //hàm kết nối tới CSDL
        public bool OpenDB(string myComputer, string myDB, string uid, string psw)
        {
            string conStr = "server = '" + myComputer + "'; database = '" + myDB + "'; uid = '" + uid + "'; pwd = '" + psw + "'";
            try
            {
                mySqlConnection = new SqlConnection(conStr);
                mySqlConnection.Open();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error " + ex.Number.ToString());
                mySqlConnection = null;
                return false;
            }
            return true;
        }
        //Hàm truy vấn dữ liệu (SELECT)
        public DataTable RunQuery(string sSql)
        {
            DataTable myDataTable = new DataTable();
            try
            {
                myDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
                SqlCommandBuilder mySqlCommandBuilder = new SqlCommandBuilder(myDataAdapter);
                myDataAdapter.Fill(myDataTable);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error " + ex.Number.ToString());
                return null;
            }
            return myDataTable;
        }
        //Hàm cập nhật dữ liệu từ 1 DataTable
        public void Update(DataTable myDataTable)
        {
            try
            {
                myDataAdapter.Update(myDataTable);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error " + ex.Number.ToString());
            }
        }
        //Hàm thực hiện 1 câu lệnh SQL như INSERT, UPDATE, DELETE
        public void ExecuteNonQuery(string sSql)
        {
            SqlCommand mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error " + ex.Number.ToString());
            }
        }

        //Hàm truy vấn dữ liệu trả về dataset dùng tạo Report
        public DataSet RunQuery_Report(string sSql, string DataSetName)
        {
            DataSet myDataSet = new DataSet();
            try
            {
                myDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
                SqlCommandBuilder mySqlCommandBuilder = new SqlCommandBuilder(myDataAdapter);
                myDataAdapter.Fill(myDataSet, DataSetName);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error " + ex.Number.ToString());
                return null;
            }
            return myDataSet;
        }

    }
}
