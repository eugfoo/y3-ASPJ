﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.BLL;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FinalProj.DAL
{
    public class RolesDAO
    {
        public List<roles> SelectAllRoles()
        {
            //Step 1 -  Define a connection to the database
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from db table
            string sqlStmt = "Select * from rolesTable";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<roles> roleList = new List<roles>();

            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                
                string role = row["Roles"].ToString();
                int appLogs = int.Parse(row["viewApplicationLogs"].ToString());
                int mgCollab = int.Parse(row["manageCollaborators"].ToString());
                int mgVouch = int.Parse(row["manageVouchers"].ToString());


                roles obj = new roles(role, appLogs, mgCollab, mgVouch);
                roleList.Add(obj);
            };

            return roleList;
        }

        public int UpdatePerms(string role, int applogs, int mgcollab, int mgvouch)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE rolesTable SET viewApplicationLogs = @applogs, manageCollaborators = @mgcollab, manageVouchers = @mgvouch WHERE Roles = @role";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@applogs", applogs);
            sqlCmd.Parameters.AddWithValue("@mgcollab", mgcollab);
            sqlCmd.Parameters.AddWithValue("@mgvouch", mgvouch);
            sqlCmd.Parameters.AddWithValue("@role", role);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateRoleName(string OldRoleName, string newRoleName)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE rolesTable SET Roles = @newRole WHERE Roles = @oldRole";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@oldRole", OldRoleName);
            sqlCmd.Parameters.AddWithValue("@newRole", newRoleName);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }


        public int Insert(string roleName, int appLog, int mgCollab, int mgVouch)
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
            string sqlStmt = "INSERT INTO rolesTable(Roles, viewApplicationLogs, manageCollaborators, manageVouchers) " + "VALUES (@RoleName, @applog, @mgcollab, @mgvouch)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@RoleName", roleName);
            sqlCmd.Parameters.AddWithValue("@applog", appLog);
            sqlCmd.Parameters.AddWithValue("@mgcollab", mgCollab);
            sqlCmd.Parameters.AddWithValue("@mgvouch", mgVouch);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }

        public int DeleteRole(string roleName)
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
            string sqlStmt = "DELETE from rolesTable Where Roles = @paraRlName";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@paraRlName", roleName);

            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }

    }
}