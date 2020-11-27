using FinalProj.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProj.BLL
{
    public class ThreadReply
    {
        public int trId { get; set; }
        public int ThreadId { get; set; }
        public string PostDate { get; set; }
        public string PostContent { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public ThreadReply()
        {

        }

        public ThreadReply(int threadId, string postDate, string postContent, int userId, string userName)
        {
            ThreadId = threadId;
            PostDate = postDate;
            PostContent = postContent;
            UserId = userId;
            UserName = userName;
        }

        public ThreadReply(int Id, int threadId, string postDate, string postContent, int userId, string userName)
        {
            trId = Id;
            ThreadId = threadId;
            PostDate = postDate;
            PostContent = postContent;
            UserId = userId;
            UserName = userName;
        }

        public int ReplyThread()
        {
            ThreadReplyDAO dao = new ThreadReplyDAO();
            int result = dao.Insert(this);
            return result;
        }

        public List<ThreadReply> getAllThreadReplies()
        {
            ThreadReplyDAO dao = new ThreadReplyDAO();
            return dao.getAllThreadReplies();
        }

        public List<ThreadReply> getAllThreadRepliesByThreadId(int threadId)
        {
            ThreadReplyDAO dao = new ThreadReplyDAO();
            return dao.getAllThreadRepliesByThreadId(threadId);
        }

        public ThreadReply getLastPersonReplyByMaxId(int threadId)
        {
            ThreadReplyDAO dao = new ThreadReplyDAO();
            return dao.getLastPersonReplyObj(threadId);
        }

        public ThreadReply getMaxUserReplyIdByThreadId(int threadId)
        {
            ThreadReplyDAO dao = new ThreadReplyDAO();
            return dao.getMaxUserReplyIdByThreadId(threadId);
        }




    }
}

   