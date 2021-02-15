using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FinalProj.BLL;

namespace FinalProj.DAL
{
    public class adminLogsDAO
    {
        public List<adminLog> SelectAllAdminLogs()
        {
            //Step 1 -  Define a connection to the database
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from db table
            string sqlStmt = "Select * from adminLogs ORDER BY DateTime DESC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<adminLog> adminLogList = new List<adminLog>();

            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                DateTime adminlgdt = Convert.ToDateTime(row["DateTime"].ToString());
                string adminlgUser = row["Username"].ToString();
                string adminlgIP = row["IPAddress"].ToString();
                string adminlgAction = row["Action"].ToString();
                string adminlgViolation = row["ViolationType"].ToString();
                string adminlgEmail = row["Email"].ToString();
                string adminlgCountry = row["Country"].ToString();

                adminLog obj = new adminLog(adminlgdt, adminlgUser, adminlgIP, adminlgAction, adminlgViolation, adminlgEmail, adminlgCountry);
                adminLogList.Add(obj);
            };

            return adminLogList;
        }

        public int InsertAdminLog(DateTime dtime, string username, string ip, string action, string violation, string email, string country)
        {
            // Execute NonQuery return an integer value
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand();

            //Step 1 -  Define a connection to the database by getting
            //          the connection string from web.config
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            // Step 2 - Instantiate SqlCommand instance to add record 
            //          with INSERT statement
            string sqlStmt = "INSERT INTO adminLogs(DateTime, Username, IPAddress, Action, ViolationType, Email, Country) " + "VALUES (@DateTime, @Username, @ipAddr, @Action, @violation, @email, @Country)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@DateTime", dtime);
            sqlCmd.Parameters.AddWithValue("@Username", username);
            sqlCmd.Parameters.AddWithValue("@ipAddr", ip);
            sqlCmd.Parameters.AddWithValue("@Action", action);
            sqlCmd.Parameters.AddWithValue("@violation", violation);
            sqlCmd.Parameters.AddWithValue("@email", email);
            sqlCmd.Parameters.AddWithValue("@Country", country);



            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }
    }
}