using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FinalProj.DAL
{
    public class CookieDAO
    {
        public Cookie SelectByEmail(string email)
        {
            //Step 1 -  Define a connection to the database by getting
            //          the connection string from web.config
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from the database table
            string sqlStmt = "Select * from Cookie where userEmail = @userEmail";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            da.SelectCommand.Parameters.AddWithValue("@userEmail", email);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet.
            Cookie cookies = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];  // Sql command returns only one record\
                int ckId = Convert.ToInt32(row["Id"]);
                string ckUEmail = row["userEmail"].ToString();
                string ckCH = row["userCookieCH"].ToString();
                string ckIE = row["userCookieIE"].ToString();


                cookies = new Cookie(ckId, ckUEmail, ckCH, ckIE);
            }
            else
            {
                cookies = null;
            }

            return cookies;
        }
        public Cookie SelectById(int Id)
        {

            //Step 1 -  Define a connection to the database by getting
            //          the connection string from web.config
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from the database table
            string sqlStmt = "Select * from Cookie where Id = @paraId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            da.SelectCommand.Parameters.AddWithValue("@paraId", Id);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet.
            Cookie cookies = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];  // Sql command returns only one record\
                int ckId = Convert.ToInt32(row["Id"]);
                string ckUEmail = row["userEmail"].ToString();
                string ckCH = row["userCookieCH"].ToString();
                string ckIE = row["userCookieIE"].ToString();


                cookies = new Cookie(ckId, ckUEmail, ckCH, ckIE);
            }
            else
            {
                cookies = null;
            }

            return cookies;
        }

        public int InsertChrome(string userEmail, string userCookieCH)
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
            string sqlStmt = "INSERT INTO Cookie(userEmail, userCookieCH) " + "VALUES (@userEmail, @userCookieCH)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userEmail", userEmail);
            sqlCmd.Parameters.AddWithValue("@userCookieCH", userCookieCH);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }
        public int InsertEmail(string userEmail,string userCookieCH, string userCookieIE)
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
            string sqlStmt = "INSERT INTO Cookie(userEmail,userCookieCH, userCookieIE) " + "VALUES (@userEmail, @userCookieCH,@userCookieIE)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userEmail", userEmail);
            sqlCmd.Parameters.AddWithValue("@userCookieCH", userCookieCH);
            sqlCmd.Parameters.AddWithValue("@userCookieIE", userCookieIE);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }
        public int InsertIE(string userEmail, string userCookieIE)
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
            string sqlStmt = "INSERT INTO Cookie(userEmail,userCookieIE) " + "VALUES (@userEmail, @userCookieIE)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userEmail", userEmail);
            sqlCmd.Parameters.AddWithValue("@userCookieIE", userCookieIE);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }
        public int UpdateCHCookie(string email, string CHCookie)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Cookie SET usercookieCH = @CHCookie WHERE userEmail = @Email";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@Email", email);
            sqlCmd.Parameters.AddWithValue("@CHCookie", CHCookie);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }
        public int UpdateIECookie(string email, string IECookie)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Cookie SET userCookieIE = @CookieIE WHERE userEmail = @email";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@email", email);
            sqlCmd.Parameters.AddWithValue("@CookieIE", IECookie);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }
    }
}