using FinalProj.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProj.BLL
{
    public class Feedback
    {
        public int FdbackId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public int AvgRating { get; set; }
        public string UserReview { get; set; }
        public int FeedbackDone { get; set; }


        public Feedback() { }

        public Feedback(int eventId, int userId, int avgRating, string userReview, int feedbackDone)
        {
            EventId = eventId;
            UserId = userId;
            AvgRating = avgRating;
            UserReview = userReview;
            FeedbackDone = feedbackDone;
        }

        public Feedback(int fdbackId, int eventId, int userId, int avgRating, string userReview, int feedbackDone) // Using this to return details from DAO
        {
            FdbackId = fdbackId;
            EventId = eventId;
            UserId = userId;
            AvgRating = avgRating;
            UserReview = userReview;
            FeedbackDone = feedbackDone;
        }

        public int createFeedback()
        {
            FeedbackDAO dao = new FeedbackDAO();
            int result = dao.Insert(this);
            return result;
        }

        public Feedback getFeedbackById(int fdbackId)  
        {
            FeedbackDAO dao = new FeedbackDAO();
            return dao.SelectByFeedbackId(fdbackId);
        }

        public List<Feedback> getAllByUserId(int userId)
        {
            FeedbackDAO dao = new FeedbackDAO();
            return dao.SelectAllByUserId(userId);
        }

        public List<Feedback> getAllFeedbacksByEventId(int eventId)
        {
            FeedbackDAO dao = new FeedbackDAO();
            return dao.SelectAllByEventId(eventId);
        }

    }

}