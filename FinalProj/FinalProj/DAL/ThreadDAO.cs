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
    public class ThreadDAO
    {
        public int Insert(Thread thread)
        {
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand();

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "INSERT INTO Threads (threadPrefix, threadBadgeColor, threadTitle, threadDate, threadImage1, threadImage2, " +
                "threadImage3, threadImage4, threadContent, user_id, user_name)" +
                "VALUES (@paraThreadPrefix, @paraThreadBadgeColor, @paraThreadTitle, @paraThreadDate, @paraThreadImage1, " +
                "@paraThreadImage2, @paraThreadImage3, @paraThreadImage4, @paraThreadContent, @paraUserId, @paraUserName)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            sqlCmd.Parameters.AddWithValue("@paraThreadPrefix", thread.Prefix);
            sqlCmd.Parameters.AddWithValue("@paraThreadBadgeColor", thread.BadgeColor);
            sqlCmd.Parameters.AddWithValue("@paraThreadTitle", thread.Title);
            sqlCmd.Parameters.AddWithValue("@paraThreadDate", thread.Date);
            sqlCmd.Parameters.AddWithValue("@paraThreadImage1", thread.ThreadImage1);
            sqlCmd.Parameters.AddWithValue("@paraThreadImage2", thread.ThreadImage2);
            sqlCmd.Parameters.AddWithValue("@paraThreadImage3", thread.ThreadImage3);
            sqlCmd.Parameters.AddWithValue("@paraThreadImage4", thread.ThreadImage4);
            sqlCmd.Parameters.AddWithValue("@paraThreadContent", thread.Content);
            sqlCmd.Parameters.AddWithValue("@paraUserId", thread.UserId);
            sqlCmd.Parameters.AddWithValue("@paraUserName", thread.UserName);

            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            myConn.Close();

            return result;
        }

        public int InsertEvent(Thread thread)
        {
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand();

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "INSERT INTO Threads (threadPrefix, threadBadgeColor, threadTitle, threadDate, threadImage1, " +
                "threadContent, user_id, user_name, eventId)" +
                "VALUES (@paraThreadPrefix, @paraThreadBadgeColor, @paraThreadTitle, @paraThreadDate, @paraThreadImage1, " +
                "@paraThreadContent, @paraUserId, @paraUserName, @eventId)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            sqlCmd.Parameters.AddWithValue("@paraThreadPrefix", thread.Prefix);
            sqlCmd.Parameters.AddWithValue("@paraThreadBadgeColor", thread.BadgeColor);
            sqlCmd.Parameters.AddWithValue("@paraThreadTitle", thread.Title);
            sqlCmd.Parameters.AddWithValue("@paraThreadDate", thread.Date);
            sqlCmd.Parameters.AddWithValue("@paraThreadImage1", thread.ThreadImage1);
            sqlCmd.Parameters.AddWithValue("@paraThreadContent", thread.Content);
            sqlCmd.Parameters.AddWithValue("@paraUserId", thread.UserId);
            sqlCmd.Parameters.AddWithValue("@paraUserName", thread.UserName);
            sqlCmd.Parameters.AddWithValue("@eventId", thread.EventId);

            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            myConn.Close();

            return result;

        }

        public int Update(int id, Thread thread)
        {
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand();

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);


            string sqlStmt = "UPDATE Threads SET threadPrefix = @paraThreadPrefix, threadBadgeColor = @paraThreadBadgeColor, threadTitle = @paraThreadTitle, " +
                "threadDate = @paraThreadDate, threadContent = @paraThreadContent, " +
                "user_id = @paraUserId, user_name = @paraUserName WHERE Id = @parathreadId";




            sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd.Parameters.AddWithValue("@paraThreadPrefix", thread.Prefix);
            sqlCmd.Parameters.AddWithValue("@paraThreadBadgeColor", thread.BadgeColor);
            sqlCmd.Parameters.AddWithValue("@paraThreadTitle", thread.Title);
            sqlCmd.Parameters.AddWithValue("@paraThreadDate", thread.Date);
            sqlCmd.Parameters.AddWithValue("@paraThreadContent", thread.Content);
            sqlCmd.Parameters.AddWithValue("@paraUserId", thread.UserId);
            sqlCmd.Parameters.AddWithValue("@paraUserName", thread.UserName);
            sqlCmd.Parameters.AddWithValue("@parathreadId", id);

            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            myConn.Close();

            return result;
        }

        public Thread GetThreadByThreadId(int threadId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from Threads WHERE Id = @paraThreadId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@paraThreadId", threadId);


            DataSet ds = new DataSet();
            da.Fill(ds);

            Thread thread = null;

            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                string threadPrefix = row["threadPrefix"].ToString();
                string threadBadgeColor = row["threadBadgeColor"].ToString();
                string threadTitle = row["threadTitle"].ToString();
                string threadDate = row["threadDate"].ToString();
                string threadImage1 = row["threadImage1"].ToString();
                string threadImage2 = row["threadImage2"].ToString();
                string threadImage3 = row["threadImage3"].ToString();
                string threadImage4 = row["threadImage4"].ToString();
                string threadContent = row["threadContent"].ToString();
                int threadUserId = Convert.ToInt32(row["user_id"]);
                string threadUserName = row["user_name"].ToString();
                int eventId = Convert.ToInt32(row["eventId"]);
                thread = new Thread(threadPrefix, threadBadgeColor, threadTitle, threadDate,
                    threadImage1, threadImage2, threadImage3, threadImage4, threadContent,
                    threadUserId, threadUserName, eventId);
            }
            else
            {
                thread = null;
            }

            return thread;
        }

        public Thread GetThreadByThreadIdWOEventId(int threadId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from Threads WHERE Id = @paraThreadId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@paraThreadId", threadId);


            DataSet ds = new DataSet();
            da.Fill(ds);

            Thread thread = null;

            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                string threadPrefix = row["threadPrefix"].ToString();
                string threadBadgeColor = row["threadBadgeColor"].ToString();
                string threadTitle = row["threadTitle"].ToString();
                string threadDate = row["threadDate"].ToString();
                string threadImage1 = row["threadImage1"].ToString();
                string threadImage2 = row["threadImage2"].ToString();
                string threadImage3 = row["threadImage3"].ToString();
                string threadImage4 = row["threadImage4"].ToString();
                string threadContent = row["threadContent"].ToString();
                int threadUserId = Convert.ToInt32(row["user_id"]);
                string threadUserName = row["user_name"].ToString();
                thread = new Thread(threadPrefix, threadBadgeColor, threadTitle, threadDate,
                    threadImage1, threadImage2, threadImage3, threadImage4, threadContent,
                    threadUserId, threadUserName);
            }
            else
            {
                thread = null;
            }

            return thread;
        }

        public Thread GetThreadByThreadIdW(int threadId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from Threads WHERE Id = @paraThreadId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@paraThreadId", threadId);


            DataSet ds = new DataSet();
            da.Fill(ds);

            Thread thread = null;

            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                string threadPrefix = row["threadPrefix"].ToString();
                string threadBadgeColor = row["threadBadgeColor"].ToString();
                string threadTitle = row["threadTitle"].ToString();
                string threadDate = row["threadDate"].ToString();
                string threadImage1 = row["threadImage1"].ToString();
                string threadImage2 = row["threadImage2"].ToString();
                string threadImage3 = row["threadImage3"].ToString();
                string threadImage4 = row["threadImage4"].ToString();
                string threadContent = row["threadContent"].ToString();
                int threadUserId = Convert.ToInt32(row["user_id"]);
                string threadUserName = row["user_name"].ToString();
                thread = new Thread(threadPrefix, threadBadgeColor, threadTitle, threadDate,
                    threadImage1, threadImage2, threadImage3, threadImage4, threadContent,
                    threadUserId, threadUserName);
            }
            else
            {
                thread = null;
            }

            return thread;
        }

        public Thread GetThreadByeventId(int eventId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from Threads WHERE eventId = @paraEventId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@paraEventId", eventId);


            DataSet ds = new DataSet();
            da.Fill(ds);

            Thread thread = null;

            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                int tId = int.Parse(row["Id"].ToString());
                string threadPrefix = row["threadPrefix"].ToString();
                string threadBadgeColor = row["threadBadgeColor"].ToString();
                string threadTitle = row["threadTitle"].ToString();
                string threadDate = row["threadDate"].ToString();
                string threadImage1 = row["threadImage1"].ToString();
                string threadImage2 = row["threadImage2"].ToString();
                string threadImage3 = row["threadImage3"].ToString();
                string threadImage4 = row["threadImage4"].ToString();
                string threadContent = row["threadContent"].ToString();
                int threadUserId = Convert.ToInt32(row["user_id"]);
                string threadUserName = row["user_name"].ToString();
                thread = new Thread(tId, threadPrefix, threadBadgeColor, threadTitle, threadDate,
                    threadImage1, threadImage2, threadImage3, threadImage4, threadContent,
                    threadUserId, threadUserName);
            }
            else
            {
                thread = null;
            }

            return thread;
        }


        public List<Thread> getAllThreads()
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from Threads WHERE threadPrefix != @paraThreadPrefix ORDER BY Id DESC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@paraThreadPrefix", "[EVENT]");

            DataSet ds = new DataSet();
            da.Fill(ds);


            List<Thread> threadsList = new List<Thread>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int tId = int.Parse(row["Id"].ToString());
                string threadPrefix = row["threadPrefix"].ToString();
                string threadBadgeColor = row["threadBadgeColor"].ToString();
                string threadTitle = row["threadTitle"].ToString();
                string threadDate = row["threadDate"].ToString();
                string threadImage1 = row["threadImage1"].ToString();
                string threadImage2 = row["threadImage2"].ToString();
                string threadImage3 = row["threadImage3"].ToString();
                string threadImage4 = row["threadImage4"].ToString();
                string threadContent = row["threadContent"].ToString();
                int threadUserId = Convert.ToInt32(row["user_id"]);
                string threadUserName = row["user_name"].ToString();

                Thread thread = new Thread(tId, threadPrefix, threadBadgeColor, threadTitle, threadDate,
                    threadImage1, threadImage2, threadImage3, threadImage4, threadContent,
                    threadUserId, threadUserName);
                threadsList.Add(thread);
            }

            return threadsList;
        }

        public List<Thread> getAllEventThreads(string eventPrefix)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from Threads WHERE threadPrefix = @paraThreadPrefix ORDER BY Id DESC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@paraThreadPrefix", eventPrefix);

            DataSet ds = new DataSet();
            da.Fill(ds);


            List<Thread> threadsList = new List<Thread>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int tId = int.Parse(row["Id"].ToString());
                string threadPrefix = row["threadPrefix"].ToString();
                string threadBadgeColor = row["threadBadgeColor"].ToString();
                string threadTitle = row["threadTitle"].ToString();
                string threadDate = row["threadDate"].ToString();
                string threadImage1 = row["threadImage1"].ToString();
                string threadImage2 = row["threadImage2"].ToString();
                string threadImage3 = row["threadImage3"].ToString();
                string threadImage4 = row["threadImage4"].ToString();
                string threadContent = row["threadContent"].ToString();
                int threadUserId = Convert.ToInt32(row["user_id"]);
                string threadUserName = row["user_name"].ToString();

                Thread thread = new Thread(tId, threadPrefix, threadBadgeColor, threadTitle, threadDate,
                    threadImage1, threadImage2, threadImage3, threadImage4, threadContent,
                    threadUserId, threadUserName);
                threadsList.Add(thread);
            }

            return threadsList;
        }


        public int queryCreatedThreadId()
        {
            int result = 0;
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            myConn.Open();
            SqlCommand cmd = new SqlCommand("Select Max(Id) From Threads", myConn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            myConn.Close();

            result = i;

            return result;
        }

        public DataTable GetImagesToLV(string threadId)
        {
            DataTable allImages = new DataTable();

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            myConn.Open();
            string sqlCmd = "Select * From Threads WHERE Id = @paraThreadId";
            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd, myConn);
            sqlDa.SelectCommand.Parameters.AddWithValue("@paraThreadId", threadId);
            sqlDa.Fill(allImages);
            myConn.Close();

            return allImages;
        }

        public List<Thread> getThreadsByUserId(int userId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from Threads WHERE threadPrefix != @paraThreadPrefix AND user_id = @paraUserId ORDER BY Id DESC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@paraThreadPrefix", "[EVENT]");
            da.SelectCommand.Parameters.AddWithValue("@paraUserId", userId);

            DataSet ds = new DataSet();
            da.Fill(ds);

            List<Thread> threadsList = new List<Thread>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int tId = int.Parse(row["Id"].ToString());
                string threadPrefix = row["threadPrefix"].ToString();
                string threadBadgeColor = row["threadBadgeColor"].ToString();
                string threadTitle = row["threadTitle"].ToString();
                string threadDate = row["threadDate"].ToString();
                string threadImage1 = row["threadImage1"].ToString();
                string threadImage2 = row["threadImage2"].ToString();
                string threadImage3 = row["threadImage3"].ToString();
                string threadImage4 = row["threadImage4"].ToString();
                string threadContent = row["threadContent"].ToString();
                int threadUserId = Convert.ToInt32(row["user_id"]);
                string threadUserName = row["user_name"].ToString();

                Thread thread = new Thread(tId, threadPrefix, threadBadgeColor, threadTitle, threadDate,
                    threadImage1, threadImage2, threadImage3, threadImage4, threadContent,
                    threadUserId, threadUserName);
                threadsList.Add(thread);
            }

            return threadsList;
        }

        public DataTable GetRepliesFromDB(string threadId)
        {
            DataTable allReplies = new DataTable();

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            myConn.Open();
            string sqlCmd = "Select * From ThreadReplies WHERE Id = @paraThreadId";
            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd, myConn);
            sqlDa.SelectCommand.Parameters.AddWithValue("@paraThreadId", threadId);
            sqlDa.Fill(allReplies);
            myConn.Close();

            return allReplies;
        }


    }
}