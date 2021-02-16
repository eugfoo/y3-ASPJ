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
    public class userProfilePrivacyDAO
    {
        public List<userProfilePrivacy> SelectByEmail(string userEmail)
        {

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from ProfilePrivacy where userEmail = @userEmail";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@userEmail", userEmail);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<userProfilePrivacy> userProfilePrivacyList = new List<userProfilePrivacy>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int profilePrivacyId = Convert.ToInt32(row["Id"]);
                string Uemail = row["userEmail"].ToString();
                int isProfilePublic = Convert.ToInt32(row["userIsProfilePublic"]);



                userProfilePrivacy profilePriv = new userProfilePrivacy(profilePrivacyId, Uemail, isProfilePublic);
                userProfilePrivacyList.Add(profilePriv);

            }

            return userProfilePrivacyList;
        }
        public int UpdateProfileVisibility(int id, int isProfilePublic)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE ProfilePrivacy SET [userIsProfilePublic] = @para_userIsProfilePublic where Id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@para_userIsProfilePublic", isProfilePublic);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int Insert(string userEmail)
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
            string sqlStmt = "INSERT INTO ProfilePrivacy(userEmail)" + "VALUES (@userEmail)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userEmail", userEmail);
            


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }

       
    }
}