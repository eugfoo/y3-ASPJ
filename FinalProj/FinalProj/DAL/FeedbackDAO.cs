using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;


namespace FinalProj.DAL
{
    public class FeedbackDAO
    {
        public int Insert(Feedback feedback)
        {
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand();

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "INSERT INTO Feedback (eventId, userId, avgRating, userReview, feedbackDone)" +
                 "VALUES (@paraEventId, @paraUserId, @paraAvgRating, @paraUserReview, @paraFeedbackDone)";

            sqlCmd = new SqlCommand(sqlStmt, myConn);

            sqlCmd.Parameters.AddWithValue("@paraEventId", feedback.EventId);
            sqlCmd.Parameters.AddWithValue("@paraUserId", feedback.UserId);
            sqlCmd.Parameters.AddWithValue("@paraAvgRating", feedback.AvgRating);
            sqlCmd.Parameters.AddWithValue("@paraUserReview", feedback.UserReview);
            sqlCmd.Parameters.AddWithValue("@paraFeedbackDone", feedback.FeedbackDone);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            myConn.Close();

            return result;
        }

        public Feedback SelectByFeedbackId(int fdbackId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from Feedback where Id = @paraId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@paraId", fdbackId);
            DataSet ds = new DataSet();
            da.Fill(ds);

            Feedback fdback = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                int Uid = Convert.ToInt32(row["Id"]);
                int UeventId = Convert.ToInt32(row["eventId"]);
                int UuserId = Convert.ToInt32(row["userId"]);
                int UavgRating = Convert.ToInt32(row["avgRating"]);
                string UuserReview = row["userReview"].ToString();
                int UfeedbackDone = Convert.ToInt32(row["feedbackDone"]);
                fdback = new Feedback(Uid, UeventId, UuserId, UavgRating, UuserReview, UfeedbackDone);
            }
            else
            {
                fdback = null;
            }

            return fdback;
        }

        public List<Feedback> SelectAllByEventId(int eventId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from Feedback Where eventId = @paraEventId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            da.SelectCommand.Parameters.AddWithValue("@paraEventId", eventId);

            DataSet ds = new DataSet();

            da.Fill(ds);

            List<Feedback> fdbackList = new List<Feedback>();
            Feedback fdback = new Feedback();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i]; 
                int fdbackId = int.Parse(row["Id"].ToString());
                fdback = fdback.getFeedbackById(fdbackId);
                fdbackList.Add(fdback);
            }
            return fdbackList;
        }

        public List<Feedback> SelectAllByUserId(int userId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * from Feedback Where userId = @paraUserId";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            da.SelectCommand.Parameters.AddWithValue("@paraUserId", userId);

            DataSet ds = new DataSet();

            da.Fill(ds);

            List<Feedback> fdbackList = new List<Feedback>();
            Feedback fdback = new Feedback();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int fdbackId = int.Parse(row["Id"].ToString());
                fdback = fdback.getFeedbackById(fdbackId);
                fdbackList.Add(fdback);
            }
            return fdbackList;
        }
    }
}