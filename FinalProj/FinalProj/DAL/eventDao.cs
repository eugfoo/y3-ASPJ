using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Diagnostics;

namespace FinalProj.DAL
{
    public class eventDao
    {
        public List<Events> SelectAllByDate(DateTime date)
        {
            //Step 1 -  Define a connection to the database by getting
            //          the connection string from web.config
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from the database table
            string sqlStmt = "Select * from tdEvent Where eventApproved = 1 AND eventDate = @paraDate Order By eventStartTime";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

			da.SelectCommand.Parameters.AddWithValue("@paraDate", date);

			//Step 3 -  Create a DataSet to store the data to be retrieved
			DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<Events> evList = new List<Events>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
				int eventId = int.Parse(row["eventId"].ToString());
				string eventTitle = row["eventTitle"].ToString();
                string eventVenue = row["eventVenue"].ToString();
                string eventDate = row["eventDate"].ToString();
                string eventStartTime = row["eventStartTime"].ToString();
                string eventEndTime = row["eventEndTime"].ToString();
                int eventMaxAttendees = int.Parse(row["eventMaxAttendees"].ToString());
                string eventDesc = row["eventDesc"].ToString();
                string eventPic = row["eventPic"].ToString();
                string eventNote = row["eventNote"].ToString();
				int avgRating = int.Parse(row["avgRating"].ToString());
				int user_id = int.Parse(row["user_id"].ToString());

				

				Events obj = new Events(eventId, eventTitle, eventVenue, eventDate ,eventStartTime, eventEndTime, eventMaxAttendees, eventDesc, eventPic, eventNote, avgRating, user_id);
                evList.Add(obj);
            }

            return evList;
        }

		public Events getEventDetails(int eventId)
		{
			//Step 1 -  Define a connection to the database by getting
			//          the connection string from web.config
			string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
			SqlConnection myConn = new SqlConnection(DBConnect);

			//Step 2 -  Create a DataAdapter to retrieve data from the database table
			string sqlStmt = "Select * from tdEvent where eventId = @paraId";
			SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

			da.SelectCommand.Parameters.AddWithValue("@paraId", eventId);

			//Step 3 -  Create a DataSet to store the data to be retrieved
			DataSet ds = new DataSet();

			//Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
			da.Fill(ds);

			//Step 5 -  Read data from DataSet.
			Events eventDetails = null;
			int rec_cnt = ds.Tables[0].Rows.Count;
			if (rec_cnt == 1)
			{
				DataRow row = ds.Tables[0].Rows[0];  // Sql command returns only one record\
				string eventTitle = row["eventTitle"].ToString();
				string eventVenue = row["eventVenue"].ToString();
				string eventDate = row["eventDate"].ToString();
				string eventStartTime = row["eventStartTime"].ToString();
				string eventEndTime = row["eventEndTime"].ToString();
				int eventMaxAttendees = int.Parse(row["eventMaxAttendees"].ToString());
				string eventDesc = row["eventDesc"].ToString();
				string eventPic = row["eventPic"].ToString();
				string eventNote = row["eventNote"].ToString();
				int avgRating = int.Parse(row["avgRating"].ToString());
				int user_id = int.Parse(row["user_id"].ToString());
				eventDetails = new Events(eventId, eventTitle, eventVenue, eventDate, eventStartTime, eventEndTime, eventMaxAttendees, eventDesc, eventPic, eventNote, avgRating, user_id);
			}
			else
			{
				eventDetails = null;
			}

			return eventDetails;
		}
		public int Insert(Events ev)
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
            string sqlStmt = "INSERT INTO tdEvent(eventTitle, eventVenue, eventDate, eventStartTime, eventEndTime, eventMaxAttendees, eventDesc, eventPic, eventNote, avgRating, user_id, eventApproved) " +
				"VALUES (@eventTitle, @eventVenue, @eventDate, @eventStartTime, @eventEndTime,@eventMaxAttendees,  @eventDesc, @eventPic, @eventNote, @avgRating, @user_id, 1)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@eventTitle", ev.Title);
            sqlCmd.Parameters.AddWithValue("@eventVenue", ev.Venue);
            sqlCmd.Parameters.AddWithValue("@eventDate", ev.Date);
            sqlCmd.Parameters.AddWithValue("@eventStartTime", ev.StartTime);
            sqlCmd.Parameters.AddWithValue("@eventEndTime", ev.EndTime);
            sqlCmd.Parameters.AddWithValue("@eventMaxAttendees", ev.MaxAttendees);
            sqlCmd.Parameters.AddWithValue("@eventDesc", ev.Desc);
            sqlCmd.Parameters.AddWithValue("@eventPic", ev.Pic);
            sqlCmd.Parameters.AddWithValue("@eventNote", ev.Note);
			sqlCmd.Parameters.AddWithValue("@avgRating", ev.AverageRating);
			sqlCmd.Parameters.AddWithValue("@user_id", ev.User_id);
			



			// Step 4 Open connection the execute NonQuery of sql command   
			myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();

            return result;
        }

		public int updateEvent(Events ev)
		{
			// Execute NonQuery return an integer value
			int result = 0;
			SqlCommand sqlCmd = new SqlCommand();

			//Step 1 -  Define a connection to the database by getting
			//          the connection string from web.config
			string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
			SqlConnection myConn = new SqlConnection(DBConnect);

			// Step 2 - Instantiate SqlCommand instance to add record 
			//          with UPDATE statement
			string sqlStmt = "Update tdEvent Set eventTitle = @eventTitle, eventVenue = @eventVenue, eventDate = @eventDate, eventStartTime = @eventStartTime, eventEndTime = @eventEndTime, eventMaxAttendees= @eventMaxAttendees, eventDesc = @eventDesc, eventPic = @eventPic, eventNote = @eventNote WHERE eventId = @eventId "; 
			sqlCmd = new SqlCommand(sqlStmt, myConn);

			// Step 3 : Add each parameterised variable with value
			sqlCmd.Parameters.AddWithValue("@eventTitle", ev.Title);
			sqlCmd.Parameters.AddWithValue("@eventVenue", ev.Venue);
			sqlCmd.Parameters.AddWithValue("@eventDate", ev.Date);
			sqlCmd.Parameters.AddWithValue("@eventStartTime", ev.StartTime);
			sqlCmd.Parameters.AddWithValue("@eventEndTime", ev.EndTime);
			sqlCmd.Parameters.AddWithValue("@eventMaxAttendees", ev.MaxAttendees);
			sqlCmd.Parameters.AddWithValue("@eventDesc", ev.Desc);
			sqlCmd.Parameters.AddWithValue("@eventPic", ev.Pic);
			sqlCmd.Parameters.AddWithValue("@eventNote", ev.Note);
			sqlCmd.Parameters.AddWithValue("@eventId", ev.EventId);



			// Step 4 Open connection the execute NonQuery of sql command   
			myConn.Open();
			result = sqlCmd.ExecuteNonQuery();

			// Step 5 :Close connection
			myConn.Close();

			return result;
		}

		public string SelectUserNameByUserId(int id)
		{
			string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
			SqlConnection myConn = new SqlConnection(DBConnect);
			string sqlStmt = "Select * from Users where id = @paraUserId";
			SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
			da.SelectCommand.Parameters.AddWithValue("@paraUserId", id);
			DataSet ds = new DataSet();
			da.Fill(ds);

			string user = null;
			int rec_cnt = ds.Tables[0].Rows.Count;
			if (rec_cnt == 1)
			{
				DataRow row = ds.Tables[0].Rows[0];
				user = row["userName"].ToString();
			}
			else
			{
				user = null;
			}

			return user;
		
		}

		public List<int> SelectAllParticipants(int eventId)
		{
			//Step 1 -  Define a connection to the database by getting
			//          the connection string from web.config
			string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
			SqlConnection myConn = new SqlConnection(DBConnect);

			//Step 2 -  Create a DataAdapter to retrieve data from the database table
			string sqlStmt = "Select * from Attendance Where Event_Id = @paraEventId";
			SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

			da.SelectCommand.Parameters.AddWithValue("@paraEventId", eventId);

			//Step 3 -  Create a DataSet to store the data to be retrieved
			DataSet ds = new DataSet();

			//Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
			da.Fill(ds);

			//Step 5 -  Read data from DataSet to List
			List<int> pList = new List<int>();
			int rec_cnt = ds.Tables[0].Rows.Count;
			for (int i = 0; i < rec_cnt; i++)
			{
				DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
				int currUser = int.Parse(row["Users_Id"].ToString());
				pList.Add(currUser);
			}

			return pList;
		}

		public int InsertParticipant(int userId, int eventId, string userName)
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
			string sqlStmt = "INSERT INTO Attendance(Users_Id, Event_Id, User_Name) " +
				"VALUES (@userId, @eventId, @userName)";
			sqlCmd = new SqlCommand(sqlStmt, myConn);

			// Step 3 : Add each parameterised variable with value
			sqlCmd.Parameters.AddWithValue("@userId", userId);
			sqlCmd.Parameters.AddWithValue("@eventId", eventId);
            sqlCmd.Parameters.AddWithValue("@userName", userName);

            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
			result = sqlCmd.ExecuteNonQuery();

			// Step 5 :Close connection
			myConn.Close();

			return result;
		}

		public int DeleteParticipant(int userId, int eventId)
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
			string sqlStmt = "DELETE from Attendance Where Event_Id = @paraEventId AND Users_Id = @paraUserId";
			sqlCmd = new SqlCommand(sqlStmt, myConn);

			// Step 3 : Add each parameterised variable with value
			sqlCmd.Parameters.AddWithValue("@paraUserId", userId);
			sqlCmd.Parameters.AddWithValue("@paraEventId", eventId);

			// Step 4 Open connection the execute NonQuery of sql command   
			myConn.Open();
			result = sqlCmd.ExecuteNonQuery();

			// Step 5 :Close connection
			myConn.Close();

			return result;
		}




        public List<Events> SelectAllAttendingEventsByUserId(int UserId)     // EUGENE I AM USING THIS FOR MY PPGALLERY. NEED TO GET ALL THE EVENTS THAT A USER ATTENDS.
        {
            //Step 1 -  Define a connection to the database by getting
            //          the connection string from web.config
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from the database table
            string sqlStmt = "Select * from Attendance Where Users_Id = @paraUserId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            da.SelectCommand.Parameters.AddWithValue("@paraUserId", UserId);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            //Step 5 -  Read data from DataSet to List
            List<Events> evList = new List<Events>();
            Events ev = new Events();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int eventId = int.Parse(row["Event_Id"].ToString());
                ev = ev.getEventDetails(eventId);
                evList.Add(ev);
            }
            return evList;
        }

        public List<Events> SelectAllEventsCreatedByUser(int UserId)     // using for pprating - azim
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from tdEvent Where user_id = @paraUserId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            da.SelectCommand.Parameters.AddWithValue("@paraUserId", UserId);

            DataSet ds = new DataSet();

            da.Fill(ds);

            List<Events> evList = new List<Events>();
            Events ev = new Events();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int eventId = int.Parse(row["eventId"].ToString());
                ev = ev.getEventDetails(eventId);
                evList.Add(ev);
            }
            return evList;
        }

        public List<Events> SelectAllBookmarkedEvents(int UserId)
		{
			//Step 1 -  Define a connection to the database by getting
			//          the connection string from web.config
			string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
			SqlConnection myConn = new SqlConnection(DBConnect);

			//Step 2 -  Create a DataAdapter to retrieve data from the database table
			string sqlStmt = "Select * from bookmark Where Users_Id = @paraUserId";
			SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

			da.SelectCommand.Parameters.AddWithValue("@paraUserId", UserId);
			

			//Step 3 -  Create a DataSet to store the data to be retrieved
			DataSet ds = new DataSet();

			//Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
			da.Fill(ds);

			//Step 5 -  Read data from DataSet to List
			List<Events> evList = new List<Events>();
			Events ev = new Events();
			int rec_cnt = ds.Tables[0].Rows.Count;
			for (int i = 0; i < rec_cnt; i++)
			{
				DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
				int eventId = int.Parse(row["Event_Id"].ToString());
				ev = ev.getEventDetails(eventId);
				evList.Add(ev);
			}
			return evList;
		}

		public bool CheckIfEventIsBookedmarked(int userId, int eventId)
		{
			bool result = false;
			//Step 1 -  Define a connection to the database by getting
			//          the connection string from web.config
			string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
			SqlConnection myConn = new SqlConnection(DBConnect);

			//Step 2 -  Create a DataAdapter to retrieve data from the database table
			string sqlStmt = "Select * from bookmark WHERE Users_Id = @paraUserId AND Event_Id = @paraEventId";
			SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

			da.SelectCommand.Parameters.AddWithValue("@paraUserId", userId);
			da.SelectCommand.Parameters.AddWithValue("@paraEventId", eventId);

			//Step 3 -  Create a DataSet to store the data to be retrieved
			DataSet ds = new DataSet();

			//Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
			da.Fill(ds);

			//Step 5 -  Read data from DataSet to List
			int rec_cnt = ds.Tables[0].Rows.Count;
			if (rec_cnt == 1)
			{
					result = true;
			}

			return result;
		}

		public int insertBookmarkEvent(int userId, int eventId)
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
			string sqlStmt = "INSERT INTO bookmark(Users_Id, Event_Id) " +
				"VALUES (@paraUserId, @paraEventId)";
			sqlCmd = new SqlCommand(sqlStmt, myConn);
			// Step 3 : Add each parameterised variable with value
			sqlCmd.Parameters.AddWithValue("@paraUserId", userId);
			sqlCmd.Parameters.AddWithValue("@paraEventId", eventId);

			// Step 4 Open connection the execute NonQuery of sql command   
			myConn.Open();
			result = sqlCmd.ExecuteNonQuery();

			// Step 5 :Close connection
			myConn.Close();

			return result;
		}

		public int DeleteBookmark(int userId, int eventId)
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
			string sqlStmt = "DELETE from bookmark Where Event_Id = @paraEventId AND Users_Id = @paraUserId";
			sqlCmd = new SqlCommand(sqlStmt, myConn);

			// Step 3 : Add each parameterised variable with value
			sqlCmd.Parameters.AddWithValue("@paraUserId", userId);
			sqlCmd.Parameters.AddWithValue("@paraEventId", eventId);

			// Step 4 Open connection the execute NonQuery of sql command   
			myConn.Open();
			result = sqlCmd.ExecuteNonQuery();

			// Step 5 :Close connection
			myConn.Close();

			return result;
		}

		public int DeleteEvent(int eventId)
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
			string sqlStmt = "DELETE from tdEvent Where eventId = @paraEventId";
			sqlCmd = new SqlCommand(sqlStmt, myConn);

			// Step 3 : Add each parameterised variable with value
			sqlCmd.Parameters.AddWithValue("@paraEventId", eventId);

			// Step 4 Open connection the execute NonQuery of sql command   
			myConn.Open();
			result = sqlCmd.ExecuteNonQuery();

			// Step 5 :Close connection
			myConn.Close();

			return result;
		}

		public bool CheckIfAttending(int userId, int eventId)
		{
			bool result = false;
			//Step 1 -  Define a connection to the database by getting
			//          the connection string from web.config
			string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
			SqlConnection myConn = new SqlConnection(DBConnect);

			//Step 2 -  Create a DataAdapter to retrieve data from the database table
			string sqlStmt = "Select * from Attendance WHERE Users_Id = @paraUserId AND Event_Id = @paraEventId";
			SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

			da.SelectCommand.Parameters.AddWithValue("@paraUserId", userId);
			da.SelectCommand.Parameters.AddWithValue("@paraEventId", eventId);

			//Step 3 -  Create a DataSet to store the data to be retrieved
			DataSet ds = new DataSet();

			//Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
			da.Fill(ds);

			//Step 5 -  Read data from DataSet to List
			int rec_cnt = ds.Tables[0].Rows.Count;
			if (rec_cnt == 1)
			{
				result = true;
			}

			return result;
		}

        public int updateAvgRatingByEventId(int eventId, int avgRating)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE tdEvent SET avgRating = @avgRating where eventId = @eventId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@avgRating", avgRating);
            sqlCmd.Parameters.AddWithValue("@eventId", eventId);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int queryCreatedEventId()
        {
            int result = 0;
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            myConn.Open();
            SqlCommand cmd = new SqlCommand("Select Max(eventId) From tdEvent", myConn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            myConn.Close();

            result = i;

            return result;
        }

		public List<Events> SelectAllEvents()
		{
			//Step 1 -  Define a connection to the database by getting
			//          the connection string from web.config
			string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
			SqlConnection myConn = new SqlConnection(DBConnect);

			//Step 2 -  Create a DataAdapter to retrieve data from the database table
			string sqlStmt = "Select * from tdEvent Where eventDate >= @paraDate Order By eventDate, eventStartTime";
			SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

			da.SelectCommand.Parameters.AddWithValue("@paraDate", DateTime.Today.ToString("yyy/MM/dd"));

			//Step 3 -  Create a DataSet to store the data to be retrieved
			DataSet ds = new DataSet();

			//Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
			da.Fill(ds);

			//Step 5 -  Read data from DataSet to List
			List<Events> evList = new List<Events>();
			int rec_cnt = ds.Tables[0].Rows.Count;
			for (int i = 0; i < rec_cnt; i++)
			{
				DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
				int eventId = int.Parse(row["eventId"].ToString());
				string eventTitle = row["eventTitle"].ToString();
				string eventVenue = row["eventVenue"].ToString();
				string eventDate = row["eventDate"].ToString();
				string eventStartTime = row["eventStartTime"].ToString();
				string eventEndTime = row["eventEndTime"].ToString();
				int eventMaxAttendees = int.Parse(row["eventMaxAttendees"].ToString());
				string eventDesc = row["eventDesc"].ToString();
				string eventPic = row["eventPic"].ToString();
				string eventNote = row["eventNote"].ToString();
				int avgRating = int.Parse(row["avgRating"].ToString());
				int user_id = int.Parse(row["user_id"].ToString());



				Events obj = new Events(eventId, eventTitle, eventVenue, eventDate, eventStartTime, eventEndTime, eventMaxAttendees, eventDesc, eventPic, eventNote, avgRating, user_id);
				evList.Add(obj);
			}

			return evList;
		}



	}

}
