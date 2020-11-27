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
    public class ThreadReplyDAO
    {
        public int Insert(ThreadReply threadReply)
        {
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand();

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "INSERT INTO ThreadReplies (threadId, postDate, postContent, user_id, user_name)" +
                "VALUES (@paraThreadId, @paraPostDate, @paraPostContent, @paraUserId, @paraUserName)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            sqlCmd.Parameters.AddWithValue("@paraThreadId", threadReply.ThreadId);
            sqlCmd.Parameters.AddWithValue("@paraPostDate", threadReply.PostDate);
            sqlCmd.Parameters.AddWithValue("@paraPostContent", threadReply.PostContent);
            sqlCmd.Parameters.AddWithValue("@paraUserId", threadReply.UserId);
            sqlCmd.Parameters.AddWithValue("@paraUserName", threadReply.UserName);

            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            myConn.Close();

            return result;

        }

        public List<ThreadReply> getAllThreadReplies()
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from ThreadReplies";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            DataSet ds = new DataSet();
            da.Fill(ds);


            List<ThreadReply> threadReplies = new List<ThreadReply>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int Id = int.Parse(row["Id"].ToString());
                int tId = int.Parse(row["threadId"].ToString());
                string postDate = row["postDate"].ToString();
                string postContent = row["postContent"].ToString();
                int user_id = int.Parse(row["user_id"].ToString());
                string user_name = row["user_name"].ToString();

                ThreadReply reply = new ThreadReply(tId, postDate, postContent, user_id, user_name);
                threadReplies.Add(reply);
            }

            return threadReplies;
        }

        public List<ThreadReply> getAllThreadRepliesByThreadId(int threadId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from ThreadReplies WHERE threadId = @paraThreadId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            da.SelectCommand.Parameters.AddWithValue("@paraThreadId", threadId);

            DataSet ds = new DataSet();
            da.Fill(ds);


            List<ThreadReply> threadReplies = new List<ThreadReply>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int Id = int.Parse(row["Id"].ToString());
                int tId = int.Parse(row["threadId"].ToString());
                string postDate = row["postDate"].ToString();
                string postContent = row["postContent"].ToString();
                int user_id = int.Parse(row["user_id"].ToString());
                string user_name = row["user_name"].ToString();

                ThreadReply reply = new ThreadReply(tId, postDate, postContent, user_id, user_name);
                threadReplies.Add(reply);
            }

            return threadReplies;
        }

        public ThreadReply getLastPersonReplyObj(int Id)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "SELECT * FROM ThreadReplies WHERE Id = @paraId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@paraId", Id);
            DataSet ds = new DataSet();
            da.Fill(ds);

            ThreadReply threadReply = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];  // Sql command returns only one record
                int iId = int.Parse(row["Id"].ToString());
                int tId = int.Parse(row["threadId"].ToString());
                string postDate = row["postDate"].ToString();
                string postContent = row["postContent"].ToString();
                int user_id = int.Parse(row["user_id"].ToString());
                string user_name = row["user_name"].ToString();

                threadReply = new ThreadReply(tId, postDate, postContent, user_id, user_name);
            }
            else
            {
                threadReply = null;
            }

            return threadReply;

        }

        public ThreadReply getMaxUserReplyIdByThreadId(int threadId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "SELECT TOP 1 * FROM ThreadReplies WHERE threadId = @paraThreadId ORDER BY Id DESC";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@paraThreadId", threadId);
            DataSet ds = new DataSet();
            da.Fill(ds);

            ThreadReply threadReply = new ThreadReply();
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];  // Sql command returns only one record
                int iId = int.Parse(row["Id"].ToString());
                int tId = int.Parse(row["threadId"].ToString());
                string postDate = row["postDate"].ToString();
                string postContent = row["postContent"].ToString();
                int user_id = int.Parse(row["user_id"].ToString());
                string user_name = row["user_name"].ToString();

                threadReply = new ThreadReply(iId, tId, postDate, postContent, user_id, user_name);
            }
            else
            {
                threadReply = null;
            }

            return threadReply;
        }

    }
}