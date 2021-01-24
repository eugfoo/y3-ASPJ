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
    public class HistoryOTPDAO
    {
        public List<HistoryOTP> SelectAllHistoryOTPs()
        {
            //Step 1 -  Define a connection to the database
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from db table
            string sqlStmt = "Select * from HistoryOTP";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<HistoryOTP> historyOTPList = new List<HistoryOTP>();

            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int userID = int.Parse(row["Id"].ToString());
                string userEmail = row["Email"].ToString();
                string userOTP = row["OTP"].ToString();
                int userOTPCheck = int.Parse(row["OTPCheck"].ToString());



                HistoryOTP obj = new HistoryOTP(userID, userEmail, userOTP, userOTPCheck);
                historyOTPList.Add(obj);
            };

            return historyOTPList;
        }

        public int Insert(string userEmail, string userOTP)
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
            string sqlStmt = "INSERT INTO HistoryOTP(Email, OTP) " + "VALUES (@Email, @OTP)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@OTP", userOTP);
            sqlCmd.Parameters.AddWithValue("@Email", userEmail);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }

        public bool SelectByEmail(string email)
        {
            bool user = false;

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from HistoryOTP where Email = @email";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@email", email);
            DataSet ds = new DataSet();
            da.Fill(ds);

            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                user = true;
            }
            else
            {
                user = false;
            }

            return user;
        }

        public HistoryOTP SelectByEmailOTP(string email)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from HistoryOTP where Email = @email";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@email", email);
            DataSet ds = new DataSet();
            da.Fill(ds);

            HistoryOTP user = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                int userID = Convert.ToInt32(row["Id"]);
                string userEmail = row["Email"].ToString();
                string userOTP = row["OTP"].ToString();
                int userOTPCheck = Convert.ToInt32(row["OTPCheck"]);


                user = new HistoryOTP(userID, userEmail, userOTP, userOTPCheck);
            }
            else
            {
                user = null;
            }

            return user;
        }

        public int UpdateOTP(string email, string otp, int otpcheck)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE HistoryOTP SET OTP = @OTP, OTPCheck = @OTPCheck WHERE Email = @email";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@email", email);
            sqlCmd.Parameters.AddWithValue("@OTP", otp);
            sqlCmd.Parameters.AddWithValue("@OTPCheck", otpcheck);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }
    }
}