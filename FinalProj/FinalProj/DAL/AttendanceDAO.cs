using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.BLL;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FinalProj.DAL
{
    public class AttendanceDAO
    {
        public List<Attendance> SelectAllByAttend()
        {
            //Step 1 -  Define a connection to the database
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from db table
            string sqlStmt = "Select * from Attendance Order By Attend DESC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<Attendance> attendanceList = new List<Attendance>();
            List<Attendance> tempList = new List<Attendance>();

            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                string userId = row["Users_Id"].ToString();
                string eventId = row["Event_Id"].ToString();
                string userName = row["User_Name"].ToString();
                int attend = int.Parse(row["Attend"].ToString());
                int feedback = int.Parse(row["Feedback"].ToString());
                Attendance obj = new Attendance(userId, eventId, userName, attend, feedback);
                attendanceList.Add(obj);
            };

            return attendanceList;
        }

        public List<Attendance> SelectAllByEventId(int id)
        {
            //Step 1 -  Define a connection to the database
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from db table
            string sqlStmt = "Select * from Attendance WHERE Event_Id = '" + id.ToString() + "' ORDER BY User_Name ASC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<Attendance> attendanceListofEvent = new List<Attendance>();
            List<Attendance> tempList = new List<Attendance>();

            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                string userId = row["Users_Id"].ToString();
                string eventId = row["Event_Id"].ToString();
                string userName = row["User_Name"].ToString();
                int attend = int.Parse(row["Attend"].ToString());
                int feedback = int.Parse(row["Feedback"].ToString());
                Attendance obj = new Attendance(userId, eventId, userName, attend, feedback);
                attendanceListofEvent.Add(obj);
            };

            return attendanceListofEvent;
        }

        public List<int> GetEventIdByParticipate(int id)
        {
            //Step 1 -  Define a connection to the database
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from db table
            string sqlStmt = "Select * from Attendance WHERE Users_Id = '" + id.ToString() + "' ORDER BY Event_Id ASC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<int> listOfEventId = new List<int>();

            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int eventId = int.Parse(row["Event_Id"].ToString());

                listOfEventId.Add(eventId);
            };

            foreach( var element in listOfEventId)
            {

                System.Diagnostics.Debug.WriteLine("This is listOfEventId: " + element);
            }

            return listOfEventId;
        }

        public int Insert(Attendance attend)
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
            string sqlStmt = "INSERT INTO Attendance(Users_Id, Event_Id, Attend, Feedback) " +
                             "VALUES (@userId, @eventId, @attend, @feedback)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userId", attend.UserId);
            sqlCmd.Parameters.AddWithValue("@eventId", attend.EventId);
            sqlCmd.Parameters.AddWithValue("@attend", attend.Attend);
            sqlCmd.Parameters.AddWithValue("@feedback", attend.Feedback);


            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }

        public int UpdateAttendance(int id, int attend)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Attendance SET Attend = @attend where Users_Id = @userId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@attend", attend);
            sqlCmd.Parameters.AddWithValue("@userId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateFeedbackByUserIdEventId(int userId, int eventId, int feedback)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Attendance SET Feedback = @feedback WHERE Users_Id = @userId AND Event_Id = @eventId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@feedback", feedback);
            sqlCmd.Parameters.AddWithValue("@eventId", eventId);
            sqlCmd.Parameters.AddWithValue("@userId", userId);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }


    }
}