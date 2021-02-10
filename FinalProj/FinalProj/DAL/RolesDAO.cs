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
    }
}