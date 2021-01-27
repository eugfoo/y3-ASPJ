﻿using System;
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
    }
}