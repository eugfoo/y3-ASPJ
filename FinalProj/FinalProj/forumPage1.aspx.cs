using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
    public partial class forumPage1 : System.Web.UI.Page
    {
        protected Users LastUser;
        protected List<Users> allUsersList;
        protected List<Thread> allthreadsList;
        protected List<Thread> OnlyEventThreadsList;
        protected List<ThreadReply> allthreadReplies;
        protected Dictionary<int, int> threadIdReplies = new Dictionary<int, int>();
        protected Dictionary<int, string> threadIdUserIdReplies = new Dictionary<int, string>();
        protected Dictionary<int, string> threadIdLastReplyDateT = new Dictionary<int, string>();

        Thread thread = new Thread();
        ThreadReply threadReply = new ThreadReply();
        Users user = new Users();

        public class Threads
        {
            public int Id { get; set; }
            public string threadPrefix { get; set; }
            public string threadBadgeColor { get; set; }
            public string threadTitle { get; set; }
            public string threadDate { get; set; }
            public string threadImage1 { get; set; }
            public string threadImage2 { get; set; }
            public string threadImage3 { get; set; }
            public string threadImage4 { get; set; }
            public string threadContent { get; set; }
            public int user_id { get; set; }
            public string user_name { get; set; }
            public string lastReplyUserName { get; set; }

        }

        public class ThreadsEvent
        {
            public int Id { get; set; }
            public string threadPrefix { get; set; }
            public string threadBadgeColor { get; set; }
            public string threadTitle { get; set; }
            public string threadDate { get; set; }
            public string threadImage1 { get; set; }
            public string threadContent { get; set; }
            public int user_id { get; set; }
            public string user_name { get; set; }
            public string lastReplyUserName { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!IsPostBack)
            {
                //ConfirmedThreadsRptr();

                allthreadsList = thread.SelectAllThreads();
                OnlyEventThreadsList = thread.SelectAllEventThreads("[EVENT]");

                allthreadReplies = threadReply.getAllThreadReplies();

                allUsersList = user.getAllUsers();

                int newestUserId = user.getLastUserId();
                LastUser = user.GetUserById(newestUserId);



                List<Threads> threadsList = new List<Threads>();
                List<ThreadsEvent> threadsEventList = new List<ThreadsEvent>();

                foreach(Thread threadEvent in OnlyEventThreadsList.Take(5))
                {
                    threadsEventList.Add(
                        new ThreadsEvent
                        {
                            Id = threadEvent.Id,
                            threadPrefix = threadEvent.Prefix,
                            threadBadgeColor = threadEvent.BadgeColor,
                            threadTitle = threadEvent.Title,
                            threadDate = threadEvent.Date,
                            threadImage1 = threadEvent.ThreadImage1,
                            threadContent = threadEvent.Content,
                            user_id = threadEvent.UserId,
                            user_name = threadEvent.UserName

                        });

                    threadIdReplies.Add(threadEvent.Id, threadReply.getAllThreadRepliesByThreadId(threadEvent.Id).Count());

                    string lastReplierName = "";
                    string lastReplierDate = "";
                    if (threadReply.getMaxUserReplyIdByThreadId(threadEvent.Id) != null)
                    {
                        lastReplierName = threadReply.getLastPersonReplyByMaxId(threadReply.getMaxUserReplyIdByThreadId(threadEvent.Id).trId).UserName;
                        lastReplierDate = threadReply.getLastPersonReplyByMaxId(threadReply.getMaxUserReplyIdByThreadId(threadEvent.Id).trId).PostDate;
  
                      
                    }
                    else
                    {
                        lastReplierName = "No Replies";
                        lastReplierDate = "-";
                    }

                    threadIdUserIdReplies.Add(threadEvent.Id, lastReplierName);
                    threadIdLastReplyDateT.Add(threadEvent.Id, lastReplierDate);
                }

                this.rptrConfirmedThreads.DataSource = threadsEventList;
                this.rptrConfirmedThreads.DataBind();

                foreach (Thread thread in allthreadsList.Take(5))
                {
                    threadsList.Add(
                        new Threads
                        {
                            Id = thread.Id,
                            threadPrefix = thread.Prefix,
                            threadBadgeColor = thread.BadgeColor,
                            threadTitle = thread.Title,
                            threadDate = thread.Date,
                            threadImage1 = thread.ThreadImage1,
                            threadImage2 = thread.ThreadImage2,
                            threadImage3 = thread.ThreadImage3,
                            threadImage4 = thread.ThreadImage4,
                            threadContent = thread.Content,
                            user_id = thread.UserId,
                            user_name = thread.UserName
                        }
                    );

                    threadIdReplies.Add(thread.Id, threadReply.getAllThreadRepliesByThreadId(thread.Id).Count());


                    string lastReplierName = "";
                    string lastReplierDate = "";
                    if (threadReply.getMaxUserReplyIdByThreadId(thread.Id) != null)
                    {
                        lastReplierName = threadReply.getLastPersonReplyByMaxId(threadReply.getMaxUserReplyIdByThreadId(thread.Id).trId).UserName;
                        lastReplierDate = threadReply.getLastPersonReplyByMaxId(threadReply.getMaxUserReplyIdByThreadId(thread.Id).trId).PostDate;
                    }
                    else
                    {
                        lastReplierName = "No Replies";
                        lastReplierDate = "-";
                    }

                    threadIdUserIdReplies.Add(thread.Id, lastReplierName);
                    threadIdLastReplyDateT.Add(thread.Id, lastReplierDate);

                }

                this.rptrThreads.DataSource = threadsList;
                this.rptrThreads.DataBind();

            }



        }

        private void ConfirmedThreadsRptr()
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            using (SqlConnection myConn = new SqlConnection(DBConnect))
            {
                using (SqlCommand cmd = new SqlCommand("Select TOP 5 * From tdEvent ORDER BY eventId DESC", myConn))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable allThreads = new DataTable();
                        sda.Fill(allThreads);
                        //rptrConfirmedThreads.DataSource = allThreads;
                        //rptrConfirmedThreads.DataBind();
                    }
                }
            }
        }
    }
}