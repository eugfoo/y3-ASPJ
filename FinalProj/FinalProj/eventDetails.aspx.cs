using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
    public partial class Enquiry : System.Web.UI.Page
    {
		protected bool bookmark;
		protected int userId;
		protected string userName;
		protected Users profilePic;
		protected Events eventDetail;
		protected List<int> listOfUserId;
		protected List<Users> participantList = new List<Users>();
		protected void Page_Load(object sender, EventArgs e)
        {
			if (Session["user"] != null)
			{
				Users user = (Users)Session["user"];
				userId = user.id;
				
			}
			if (Request.QueryString["eventId"] == null)
			{
				Response.Redirect("homepage.aspx");
			}
			if (Session["SessionSSM"] != null)
            {
                panelSuccess.Visible = true;
                lb_success.Text = Session["SessionSSM"].ToString();
                Session["SessionSSM"] = null;
            }
            else
                panelSuccess.Visible = false;
            if (Session["SessionERM"] != null)
            {
                panelError.Visible = true;
                lb_error.Text = Session["SessionERM"].ToString();
                Session["SessionERM"] = null;
            }
			Events ev = new Events();
			eventDetail = ev.getEventDetails(int.Parse(Request.QueryString["eventId"]));
		

			

			int index = eventDetail.Date.IndexOf(" ");
			eventDetail.Date = eventDetail.Date.Substring(0, index);
			eventDetail.StartTime = eventDetail.StartTime.Substring(0, 5);

			if (int.Parse(eventDetail.StartTime.Substring(0, 2)) >= 12)
			{

				if (int.Parse(eventDetail.StartTime.Substring(0, 2)) == 12)
					eventDetail.StartTime = (int.Parse(eventDetail.StartTime.Substring(0, 2))).ToString() + ":" + eventDetail.StartTime.Substring(3, 2) + " PM";
				else
					eventDetail.StartTime = (int.Parse(eventDetail.StartTime.Substring(0, 2)) - 12).ToString() + ":" + eventDetail.StartTime.Substring(3, 2) + " PM";
			}
			else
			{
				eventDetail.StartTime = eventDetail.StartTime + " AM";
			}
            
			if (int.Parse(eventDetail.EndTime.Substring(0, 2)) >= 12)
			{

				if (int.Parse(eventDetail.EndTime.Substring(0, 2)) == 12)
					eventDetail.EndTime = (int.Parse(eventDetail.EndTime.Substring(0, 2))).ToString() + ":" + eventDetail.EndTime.Substring(3, 2) + " PM";
				else
					eventDetail.EndTime = (int.Parse(eventDetail.EndTime.Substring(0, 2)) - 12).ToString() + ":" + eventDetail.EndTime.Substring(3, 2) + " PM";
			}
			else
			{
				eventDetail.EndTime = eventDetail.EndTime + " AM";
			}

			listOfUserId = ev.getAllParticipants(eventDetail.EventId);
			Users usr = new Users();

			foreach (int usrId in listOfUserId)
			{
				Users name = usr.GetUserById(usrId);
				
				participantList.Add(name);
			}
			userName = eventDetail.GetAllUserNameByUserId(eventDetail.User_id);
			bookmark = ev.VerifyIfEventIsBookmarked(userId, int.Parse(Request.QueryString["eventId"]));

			Users organiser = new Users();
			profilePic = organiser.GetUserById(eventDetail.User_id);
			imgDP.ImageUrl = profilePic.DPimage;
			imgDPOrg.ImageUrl = profilePic.DPimage;


		}

		protected void joinEvent_Click(object sender, EventArgs e)
		{
			if (Session["user"] != null)
			{
				Users user = (Users)Session["user"];
				userId = user.id;
                string userName = user.name;
				Events parti = new Events();

				var result = parti.AddParticipant(userId, int.Parse(Request.QueryString["eventId"]), userName);
				if (result == 1)
				{
					Session["SessionSSM"] = "You have successfully joined the event!";
					Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
					
				}
				else if(result == -1)
				{
					Session["SessionERM"] = "You already joined an event within this timeframe!";
					Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
				}
				else
				{
					Session["SessionERM"] = "Oops! Something Went Wrong";
					Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
				}


			}
			else
			{
				Session["SessionERM"] = "Please Log In!";
				Response.Redirect("/eventDetails.aspx?eventId="+ Request.QueryString["eventId"]);
				
			}
		}

		protected void leaveEvent_Click(object sender, EventArgs e)
		{

			Users user = (Users)Session["user"];
			userId = user.id;
			Events parti = new Events();
			int result = parti.RemoveParticipant(userId, int.Parse(Request.QueryString["eventId"]));
			if (result == 1)
			{
				Session["SessionSSM"] = "You have successfully left the event!";
				Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
			}
			else
			{
				Session["SessionERM"] = "Oops! Something Went Wrong";
				Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
			}
		}

		protected void bookmarkEvent_Click(object sender, EventArgs e)
		{
			Users user = (Users)Session["user"];
			if (user != null)
			{
				userId = user.id;
				Events parti = new Events();
				int result = parti.findBookmark(userId, int.Parse(Request.QueryString["eventId"]));
				if (result == 1)
				{
					Session["SessionSSM"] = "You have successfully bookmarked this event!";
					Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
				}
				else
				{
					Session["SessionERM"] = "Oops! Something Went Wrong";
					Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
				}
			}
			else
			{
				Session["SessionERM"] = "Please Log In!";
				Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);

			}
		}

		protected void unbookmarkEvent_Click(object sender, EventArgs e)
		{
			Users user = (Users)Session["user"];
			userId = user.id;
			Events parti = new Events();
			int result = parti.removeBookmark(userId, int.Parse(Request.QueryString["eventId"]));
			if (result == 1)
			{
				Session["SessionSSM"] = "You have successfully removed this bookmark!";
				Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
			}
			else
			{
				Session["SessionERM"] = "Oops! Something Went Wrong";
				Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
			}
		}


        protected void attendance_Click(object sender, EventArgs e)
        {
            Events ev = new Events();
            eventDetail = ev.getEventDetails(int.Parse(Request.QueryString["eventId"]));
            string title = eventDetail.Title;
            Session["eventTitle"] = title;
            Session["eventId"] = int.Parse(Request.QueryString["eventId"]);
            Response.Redirect("/AttendanceSubmitted.aspx");
        }

		protected void discussion2_Click(object sender, EventArgs e)
		{
			Thread thdId = new Thread();
			Thread newthId = thdId.getThreadIdByEventId(int.Parse(Request.QueryString["eventId"]));


			Response.Redirect("/forumPostEvent.aspx?threadId=" + newthId.Id.ToString());
		}

		protected void discussion_Click(object sender, EventArgs e)
		{
			Thread thdId = new Thread();
			Thread newthId = thdId.getThreadIdByEventId(int.Parse(Request.QueryString["eventId"]));
			Response.Redirect("/forumPostEvent.aspx?threadId=" + newthId.Id.ToString());

		}
	}
}