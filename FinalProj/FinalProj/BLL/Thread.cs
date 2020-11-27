using FinalProj.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FinalProj.BLL
{
    public class Thread
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string BadgeColor { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string ThreadImage1 { get; set; }
        public string ThreadImage2 { get; set; }
        public string ThreadImage3 { get; set; }
        public string ThreadImage4 { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int EventId { get; set; }

        public Thread()
        {

        }

        public Thread(string threadPrefix, string threadBadgeColor, string threadTitle, string threadDate
            , string Image1, string Image2, string Image3, string Image4
            , string threadContent
            , int userId, string userName)
        {
            Prefix = threadPrefix;
            BadgeColor = threadBadgeColor;
            Title = threadTitle;
            Date = threadDate;
            ThreadImage1 = Image1;
            ThreadImage2 = Image2;
            ThreadImage3 = Image3;
            ThreadImage4 = Image4;
            Content = threadContent;
            UserId = userId;
            UserName = userName;
        }

        public Thread(string threadPrefix, string threadBadgeColor, string threadTitle, string threadDate
            , string Image1, string Image2, string Image3, string Image4
            , string threadContent
            , int userId, string userName, int eventId)
        {
            Prefix = threadPrefix;
            BadgeColor = threadBadgeColor;
            Title = threadTitle;
            Date = threadDate;
            ThreadImage1 = Image1;
            ThreadImage2 = Image2;
            ThreadImage3 = Image3;
            ThreadImage4 = Image4;
            Content = threadContent;
            UserId = userId;
            UserName = userName;
            EventId = eventId;
        }

        public Thread(int threadId, string threadPrefix, string threadBadgeColor, string threadTitle, string threadDate
            , string Image1, string Image2, string Image3, string Image4
            , string threadContent
            , int userId, string userName)
        {
            Id = threadId;
            Prefix = threadPrefix;
            BadgeColor = threadBadgeColor;
            Title = threadTitle;
            Date = threadDate;
            ThreadImage1 = Image1;
            ThreadImage2 = Image2;
            ThreadImage3 = Image3;
            ThreadImage4 = Image4;
            Content = threadContent;
            UserId = userId;
            UserName = userName;
        }

        public Thread(int eventId, string eventPrefix, string eventBadgeColor, string eventTitle, string eventDate, string eventImg, string eventDesc, int userId, string userName)
        {
            EventId = eventId;
            Prefix = eventPrefix;
            BadgeColor = eventBadgeColor;
            Title = eventTitle;
            Date = eventDate;
            ThreadImage1 = eventImg;
            Content = eventDesc;
            UserId = userId;
            UserName = userName;
        }

        public int createThreadForEvent()
        {
            ThreadDAO dao = new ThreadDAO();
            int result = dao.InsertEvent(this);
            return result;
        }

        public int CreateThread()
        {
            ThreadDAO dao = new ThreadDAO();
            int result = dao.Insert(this);
            return result;
        }

        public int UpdateThread(int id)
        {
            ThreadDAO dao = new ThreadDAO();
            int result = dao.Update(id, this);
            return result;
        }

        public Thread GetThreadByThreadId(int threadId)
        {
            ThreadDAO thread = new ThreadDAO();
            return thread.GetThreadByThreadId(threadId);
        }

        public Thread GetThreadByThreadIdWOEventId(int threadId)
        {
            ThreadDAO thread = new ThreadDAO();
            return thread.GetThreadByThreadIdWOEventId(threadId);
        }

        public int getMaxThreadId()
        {
            ThreadDAO dao = new ThreadDAO();
            int result = dao.queryCreatedThreadId();
            return result;
        }

        public DataTable GetImagesToLV(string threadId)
        {
            ThreadDAO dao = new ThreadDAO();
            return dao.GetImagesToLV(threadId);
        }

        //public DataTable GetRepliesFromDB(string threadId)
        //{
        //    ThreadDAO dao = new ThreadDAO();
        //    return dao.GetRepliesFromDB(threadId);
        //}

        public List<Thread> getThreadsByUserId(int userId)
        {
            ThreadDAO dao = new ThreadDAO();
            return dao.getThreadsByUserId(userId);
        }


        public List<Thread> SelectAllThreads()
        {
            ThreadDAO dao = new ThreadDAO();
            return dao.getAllThreads();
        }


        public List<Thread> SelectAllEventThreads(string eventPrefix)
        {
            ThreadDAO dao = new ThreadDAO();
            return dao.getAllEventThreads(eventPrefix);
        }

        public Thread getThreadIdByEventId(int eventId)
        {
            ThreadDAO dao = new ThreadDAO();
            return dao.GetThreadByeventId(eventId);
        }


    }


}