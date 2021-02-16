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
    public class UserLockDAO
    {
        public List<UserLock> SelectByEmail(string userEmail)
        {

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from UserLock where userEmail = @userEmail";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@userEmail", userEmail);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<UserLock> ulList = new List<UserLock>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int ulId = Convert.ToInt32(row["Id"]);
                string ulEmail = row["userEmail"].ToString();
                string ulName = row["userName"].ToString();
                DateTime ulDt = Convert.ToDateTime(row["dateTime"].ToString());
                int ulLock = Convert.ToInt32(row["userLock"]);



                UserLock ul = new UserLock(ulId, ulEmail, ulName, ulDt, ulLock);
                ulList.Add(ul);

            }

            return ulList;
        }
        public List<UserLock> SelectById(int Id)
        {

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from UserLock where Id = @Id";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@Id", Id);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<UserLock> ulList = new List<UserLock>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int ulId = Convert.ToInt32(row["Id"]);
                string ulEmail = row["userEmail"].ToString();
                string ulName = row["userName"].ToString();
                DateTime ulDt = Convert.ToDateTime(row["dateTime"].ToString());
                int ulLock = Convert.ToInt32(row["userLock"]);



                UserLock ul = new UserLock(ulId, ulEmail, ulName, ulDt, ulLock);
                ulList.Add(ul);

            }

            return ulList;
        }

        public int Insert(string userEmail, string userName, DateTime dateTime, int userLock)
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
            string sqlStmt = "INSERT INTO UserLock(userEmail, userName, dateTime, userLock) " + "VALUES (@userEmail, @userName, @dateTime, @userLock)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userEmail", userEmail);
            sqlCmd.Parameters.AddWithValue("@userName", userName);
            sqlCmd.Parameters.AddWithValue("@dateTime", dateTime);
            sqlCmd.Parameters.AddWithValue("@emailTitle", userLock);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }
    }
}