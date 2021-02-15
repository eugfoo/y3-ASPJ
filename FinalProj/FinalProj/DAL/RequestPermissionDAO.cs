using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.BLL;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FinalProj.DAL
{
    public class RequestPermissionDAO
    {
        public int Insert(string email, string role, string currentRole)
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
            string sqlStmt = "INSERT INTO RequestTable(subAdminEmail, requestRole, currentRole) " +
                "VALUES (@subAdminEmail, @requestRole, @currentRole)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@subAdminEmail", email);
            sqlCmd.Parameters.AddWithValue("@requestRole", role);
            sqlCmd.Parameters.AddWithValue("@currentRole", currentRole);

            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();
            return result;
        }

        public List<RequestPermission> getAllRequestByEmail(string email)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "SELECT * FROM RequestTable WHERE subAdminEmail = @email";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@email", email);

            DataSet ds = new DataSet();
            da.Fill(ds);


            List<RequestPermission> allReqList = new List<RequestPermission>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int requestId = Convert.ToInt32(row["Id"]);
                string requestEmail = row["subAdminEmail"].ToString();
                string requestRole = row["requestRole"].ToString();
                string currentRole = row["currentRole"].ToString();
                int requestStatus = Convert.ToInt32(row["status"]);

                RequestPermission req = new RequestPermission(requestId, requestEmail, requestRole, currentRole, requestStatus);
                allReqList.Add(req);
            }

            return allReqList;
        }

        public List<RequestPermission> getAllRequest()
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "SELECT * FROM RequestTable";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            DataSet ds = new DataSet();
            da.Fill(ds);


            List<RequestPermission> allReqList = new List<RequestPermission>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int requestId = Convert.ToInt32(row["Id"]);
                string requestEmail = row["subAdminEmail"].ToString();
                string requestRole = row["requestRole"].ToString();
                string currentRole = row["currentRole"].ToString();
                int requestStatus = Convert.ToInt32(row["status"]);

                RequestPermission req = new RequestPermission(requestId, requestEmail, requestRole, currentRole, requestStatus);
                allReqList.Add(req);
            }

            return allReqList;
        }

        public RequestPermission getLastRequest(string email)
        {
            //Step 1 -  Define a connection to the database by getting
            //          the connection string from web.config
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from the database table
            string sqlStmt = "SELECT MAX(Id) FROM RequestTable WHERE subAdminEmail = @subAdminEmail";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            da.SelectCommand.Parameters.AddWithValue("@subAdminEmail", email);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet.
            RequestPermission requestDetail = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];  // Sql command returns only one record\
                int requestId = Convert.ToInt32(row["Id"]);
                string requestEmail = row["subAdminEmail"].ToString();
                string requestRole = row["requestRole"].ToString();
                string currentRole = row["currentRole"].ToString();
                int requestStatus = Convert.ToInt32(row["status"]);

                RequestPermission req = new RequestPermission(requestId, requestEmail, requestRole, currentRole, requestStatus);
            }
            else
            {
                requestDetail = null;
            }

            return requestDetail;
        }

        public int deleteRequest(string email)
        {
            // Execute NonQuery return an integer value
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand();

            //Step 1 -  Define a connection to the database by getting
            //          the connection string from web.config
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            // Step 2 - Instantiate SqlCommand instance to add record 
            //          with DELETE statement
            string sqlStmt = "DELETE from RequestTable WHERE subAdminEmail = @paraEmail";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@paraEmail", email);

            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }
    }
}