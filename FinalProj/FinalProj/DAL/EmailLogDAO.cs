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
    public class EmailLogDAO
    {
        public List<EmailLog> SelectByEmail(string email)
        {

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from emailLogs where emailSent = @email ORDER BY DateTime DESC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@email", email);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<EmailLog> elgList = new List<EmailLog>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int elgId = Convert.ToInt32(row["Id"]);
                string elgUEmail = row["userEmail"].ToString();
                string elgFrom = row["senderEmail"].ToString();
                DateTime elgdt = Convert.ToDateTime(row["dateTime"].ToString());
                string elgTitle = row["emailTitle"].ToString();



                EmailLog elg = new EmailLog(elgId, elgUEmail,elgFrom, elgdt, elgTitle);
                elgList.Add(elg);

            }

            return elgList;
        }
        public List<EmailLog> SelectById(int Id)
        {

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from emailLogs where Id = @Id ORDER BY DateTime DESC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@Id", Id);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<EmailLog> elgList = new List<EmailLog>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int elgId = Convert.ToInt32(row["Id"]);
                string elgUEmail = row["userEmail"].ToString();
                string elgFrom = row["senderEmail"].ToString();
                DateTime elgdt = Convert.ToDateTime(row["dateTime"].ToString());
                string elgTitle = row["emailTitle"].ToString();



                EmailLog elg = new EmailLog(elgId, elgUEmail, elgFrom, elgdt, elgTitle);
                elgList.Add(elg);

            }

            return elgList;
        }

        public int Insert(string userEmail, string senderEmail, DateTime dateTime, string emailTitle)
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
            string sqlStmt = "INSERT INTO emailLogs(userEmail, senderEmail, dateTime, emailTitle) " + "VALUES (@userEmail, @senderEmail, @dateTime, @emailTitle)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userEmail", userEmail);
            sqlCmd.Parameters.AddWithValue("@senderEmail", senderEmail);
            sqlCmd.Parameters.AddWithValue("@dateTime", dateTime);
            sqlCmd.Parameters.AddWithValue("@emailTitle", emailTitle);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }

    }
}