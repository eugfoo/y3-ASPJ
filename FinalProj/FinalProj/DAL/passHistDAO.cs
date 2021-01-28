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
    public class passHistDAO
    {
        public int Insert(PassHist pass)
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
            string sqlStmt = "INSERT INTO passHistory (UserEmail, UserPassHistory, DateTime) " +
                "VALUES (@userEmail, @userPassHist, @userDateTime)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userEmail", pass.userEmail);
            sqlCmd.Parameters.AddWithValue("@userPassHist", pass.passHashHist);
            sqlCmd.Parameters.AddWithValue("@userDateTime", pass.userRegDate);

            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();
            return result;
        }

        public List<PassHist> getAllPassById(string email)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "SELECT * FROM passHistory WHERE UserEmail = @email";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@email", email);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<PassHist> allPassList = new List<PassHist>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                string uEmail = row["UserEmail"].ToString();
                string uPassHist = row["UserPassHistory"].ToString();
                string uDateTime = row["DateTime"].ToString();

                PassHist pass = new PassHist(uEmail, uPassHist, uDateTime);
                allPassList.Add(pass);
            }

            return allPassList;
        }
    }
}