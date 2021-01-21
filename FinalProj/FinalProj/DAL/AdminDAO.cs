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
    public class AdminDAO
    {

        public List<Admins> SelectAllAdmins()
        {
            //Step 1 -  Define a connection to the database
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from db table
            string sqlStmt = "Select * from Admins";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<Admins> adminList = new List<Admins>();

            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int adminId = int.Parse(row["Id"].ToString());
                string adminName = row["Name"].ToString();
                string adminRole = row["Role"].ToString();
                string adminStatus = row["Status"].ToString();
                string adminEmail = row["Email"].ToString();


                Admins obj = new Admins(adminId, adminName, adminRole, adminStatus, adminEmail);
                adminList.Add(obj);
            };

            return adminList;
        }

        public int Insert(string adminName, string adminEmail)
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
            string sqlStmt = "INSERT INTO Admins(Name, Role, Status, Email) " + "VALUES (@Name, @Role, @Status, @Email)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@Name", adminName);
            sqlCmd.Parameters.AddWithValue("@Role", "sub admin");
            sqlCmd.Parameters.AddWithValue("@Status", "Pending");
            sqlCmd.Parameters.AddWithValue("@Email", adminEmail);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }

    }
}