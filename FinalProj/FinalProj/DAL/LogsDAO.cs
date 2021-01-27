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
    public class LogsDAO
    {
        public List<Logs> SelectByEmail(string email)
        {

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from sessionLogs where userEmail = @email ORDER BY DateTime DESC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@email", email);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<Logs> lgList = new List<Logs>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int lgId = Convert.ToInt32(row["Id"]);
                string lgUEmail = row["userEmail"].ToString();
                DateTime lgdt = Convert.ToDateTime(row["DateTime"].ToString());
                string lgIP = row["ipAddr"].ToString();
                string lgCountry = row["Country"].ToString();
                string lgResult = row["Result"].ToString();



                Logs lg = new Logs(lgId, lgUEmail, lgdt, lgIP, lgCountry, lgResult);
                lgList.Add(lg);

            }

            return lgList;
        }

        public int Insert(string userEmail, DateTime DateTime, string ipAddr, string Country, string lresult)
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
            string sqlStmt = "INSERT INTO sessionLogs(userEmail, DateTime, ipAddr, Country, Result) " + "VALUES (@userEmail, @DateTime, @ipAddr, @Country, @result)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userEmail", userEmail);
            sqlCmd.Parameters.AddWithValue("@DateTime", DateTime);
            sqlCmd.Parameters.AddWithValue("@ipAddr", ipAddr);
            sqlCmd.Parameters.AddWithValue("@Country", Country);
            sqlCmd.Parameters.AddWithValue("@result", lresult);



            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }

        public List<ActivityLog> SelectAllActivityLogs()
        {
            //Step 1 -  Define a connection to the database
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from db table
            string sqlStmt = "Select * from activityLogs ORDER BY DateTime DESC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<ActivityLog> activityLogList = new List<ActivityLog>();

            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                DateTime activitylgdt = Convert.ToDateTime(row["DateTime"].ToString());
                string activitylgUser = row["Username"].ToString();
                string activitylgIP = row["IPAddress"].ToString();
                string activitylgAction = row["Action"].ToString();
                string activitylgViolation = row["ViolationType"].ToString();
                string activitylgEmail = row["Email"].ToString();
                string activitylgCountry = row["Country"].ToString();

                ActivityLog obj = new ActivityLog(activitylgdt, activitylgUser, activitylgIP, activitylgAction, activitylgViolation, activitylgEmail, activitylgCountry);
                activityLogList.Add(obj);
            };

            return activityLogList;
        }

        public int InsertActivity(DateTime dtime, string username, string ip, string action, string violation, string email, string country)
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
            string sqlStmt = "INSERT INTO activityLogs(DateTime, Username, IPAddress, Action, ViolationType, Email, Country) " + "VALUES (@DateTime, @Username, @ipAddr, @Action, @violation, @email, @Country)";
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