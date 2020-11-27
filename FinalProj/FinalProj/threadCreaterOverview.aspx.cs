using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
    public partial class threadCreaterOverview : System.Web.UI.Page
    {
        protected Users currentThreadUser;
        //repeater control replacement
        protected List<Thread> allthreadsList;
        protected List<ThreadReply> allthreadReplies;
        protected Dictionary<int, int> threadIdReplies = new Dictionary<int, int>();
        protected Dictionary<int, string> threadIdUserIdReplies = new Dictionary<int, string>();
        protected Dictionary<int, string> threadIdLastReplyDateT = new Dictionary<int, string>();

        Thread thread = new Thread();
        ThreadReply threadReply = new ThreadReply();

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

        readonly PagedDataSource _pgsource = new PagedDataSource();
        int _firstIndex, _lastIndex;
        private int _pageSize = 8;
        
        private int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["CurrentPage"]);
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = Request.QueryString["userId"];



            if (userId != null)
            {
                Users usr = new Users();
                currentThreadUser = usr.GetUserById(int.Parse(userId));
                allthreadsList = thread.getThreadsByUserId(int.Parse(userId));
                HFuserId.Value = userId;

                List<Threads> threadsList = new List<Threads>();

                foreach (Thread thread in allthreadsList)
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

            if (Page.IsPostBack) return;
            BindDataIntoRepeater(int.Parse(userId));
        }

        static DataTable GetDataFromDb(int userId)
        {
            DataTable allThreads = new DataTable();

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            myConn.Open();
            string sqlStmt = "Select * from Threads WHERE threadPrefix != @paraThreadPrefix AND user_id = @paraUserId ORDER BY Id DESC";
            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlStmt, myConn);
            sqlDa.SelectCommand.Parameters.AddWithValue("@paraThreadPrefix", "[EVENT]");
            sqlDa.SelectCommand.Parameters.AddWithValue("@paraUserId", userId);
            sqlDa.Fill(allThreads);
            myConn.Close();

            return allThreads;

        }

        private void BindDataIntoRepeater(int userId)
        {

            threadIdReplies.Clear();
            threadIdUserIdReplies.Clear();
            threadIdLastReplyDateT.Clear();

            Users usr = new Users();
            currentThreadUser = usr.GetUserById(userId);
            allthreadsList = thread.getThreadsByUserId(userId);


            List<Threads> threadsList = new List<Threads>();

            foreach (Thread thread in allthreadsList)
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


            var dt = GetDataFromDb(userId);
            _pgsource.DataSource = dt.DefaultView;
            _pgsource.AllowPaging = true;

            _pgsource.PageSize = _pageSize;
            _pgsource.CurrentPageIndex = CurrentPage;

            ViewState["TotalPages"] = _pgsource.PageCount;

            lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;

            lbPrevious.Enabled = !_pgsource.IsFirstPage;
            lbNext.Enabled = !_pgsource.IsLastPage;
            lbFirst.Enabled = !_pgsource.IsFirstPage;
            lbLast.Enabled = !_pgsource.IsLastPage;
            this.rptrThreads.DataSource = _pgsource;
            this.rptrThreads.DataBind();



            HandlePaging();
        }

        private void HandlePaging()
        {
            var dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");

            _firstIndex = CurrentPage - 5;
            if (CurrentPage > 5)
                _lastIndex = CurrentPage + 5;
            else
                _lastIndex = 10;



            if (_lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
            {
                _lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
                _firstIndex = _lastIndex - 10;
            }

            if (_firstIndex < 0)
                _firstIndex = 0;

            for (var i = _firstIndex; i < _lastIndex; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaging.DataSource = dt;
            rptPaging.DataBind();
        }

        protected void lbFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            BindDataIntoRepeater(int.Parse(HFuserId.Value));
        }

        protected void lbLast_Click(object sender, EventArgs e)
        {
            CurrentPage = (Convert.ToInt32(ViewState["TotalPages"]) - 1);
            BindDataIntoRepeater(int.Parse(HFuserId.Value));
        }

        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            BindDataIntoRepeater(int.Parse(HFuserId.Value));
        }

        protected void lbNext_Click(object sender, EventArgs e)
        {
            CurrentPage += 1;
            BindDataIntoRepeater(int.Parse(HFuserId.Value));
        }

        protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("newPage")) return;
            CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
            BindDataIntoRepeater(int.Parse(HFuserId.Value));
        }

        protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPage = (LinkButton)e.Item.FindControl("lbPaging");
            if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
            lnkPage.Enabled = false;
            lnkPage.BackColor = Color.FromName("#8db0c7");
        }
    }
}