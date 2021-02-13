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
    public class blockedDAO
    {
        public List<blocked> getAllBlockedUsers()
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * FROM Blocked";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            DataSet ds = new DataSet();
            da.Fill(ds);


            List<blocked> allBlockedList = new List<blocked>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int Uid = Convert.ToInt32(row["Id"]);
                string Uemail = row["userEmail"].ToString();
                string Uname = row["userName"].ToString();
                string blockReason = row["Reason"].ToString();
                DateTime dtBlock = Convert.ToDateTime(row["DateTime"].ToString());

                blocked blckAcc = new blocked(Uemail, Uname, blockReason, dtBlock);
                allBlockedList.Add(blckAcc);
            }

            return allBlockedList;
        }

        public int Insert(string userEmail, string userName, string reason, DateTime dt)
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
            string sqlStmt = "INSERT INTO Blocked(userEmail, userName, Reason, DateTime) " + "VALUES (@userEmail, @userName, @reason, @dtb)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userEmail", userEmail);
            sqlCmd.Parameters.AddWithValue("@userName", userName);
            sqlCmd.Parameters.AddWithValue("@reason", reason);
            sqlCmd.Parameters.AddWithValue("@dtb", dt);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }


        public int unBlockAcc(string email)
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
            string sqlStmt = "DELETE from Blocked Where userEmail = @paraEmail";
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