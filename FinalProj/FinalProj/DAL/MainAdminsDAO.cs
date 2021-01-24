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
    public class MainAdminsDAO
    {
        public int Insert(MainAdmins MainAdmin)
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
            string sqlStmt = "INSERT INTO MainAdmins(Email, Name, Password) " +
                "VALUES (@Email, @Name, @Password)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@Email", MainAdmin);
            sqlCmd.Parameters.AddWithValue("@Name", MainAdmin);
            sqlCmd.Parameters.AddWithValue("@Password", MainAdmin);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();
            return result;
        }

        public List<MainAdmins> SelectAllMainAdmins()
        {
            //Step 1 -  Define a connection to the database
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from db table
            string sqlStmt = "Select * from MainAdmins";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<MainAdmins> MainadminList = new List<MainAdmins>();

            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int MainadminId = int.Parse(row["Id"].ToString());
                string MainadminName = row["Name"].ToString();
                string MainadminEmail = row["Email"].ToString();
                string MainadminPassword = row["Password"].ToString();

                MainAdmins obj = new MainAdmins(MainadminId, MainadminName, MainadminEmail, MainadminPassword);
                MainadminList.Add(obj);
            };

            return MainadminList;
        }

        public MainAdmins SelectByEmail(string email)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from MainAdmins where Email = @email";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@email", email);
            DataSet ds = new DataSet();
            da.Fill(ds);

            MainAdmins Mainadmin = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                int Uid = Convert.ToInt32(row["Id"]);
                string MAemail = row["Email"].ToString();
                string MApass = row["Password"].ToString();
                string MAname = row["Name"].ToString();

                Mainadmin = new MainAdmins(Uid, MAemail, MAname, MApass);
            }
            else
            {
                Mainadmin = null;
            }

            return Mainadmin;
        }

    }
}