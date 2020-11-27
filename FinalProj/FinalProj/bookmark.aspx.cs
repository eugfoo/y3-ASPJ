using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
    public partial class bookmark : System.Web.UI.Page
    {
		protected int userId;
		protected bool attendingResult;
		protected Dictionary<int,Users> organiserDict = new Dictionary<int, Users>();
		protected Dictionary<int, bool> checkJoinStatus= new Dictionary<int, bool>();
		protected Dictionary<int, int> participantList = new Dictionary<int, int>();
		protected List<Events> eventsUserBookmarked;
		protected string userName;

		protected void Page_Load(object sender, EventArgs e)
        {
			if (Session["user"] != null)
			{
				Users user = (Users)Session["user"];
				userId = user.id;
				Events ev = new Events();
				eventsUserBookmarked = ev.GetAllBookedmarkedEventsById(userId);
			}
			else
			{
				Response.Redirect("/homepage.aspx");
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

			List<int> attending;

			foreach (Events element in eventsUserBookmarked)                  // loops through each event list and changes formatting of both time and date
			{
				int index = element.Date.IndexOf(" ");
				element.Date = element.Date.Substring(0, index);
				element.StartTime = element.StartTime.Substring(0, 5);
				if (int.Parse(element.StartTime.Substring(0, 2)) >= 12)
				{

					if (int.Parse(element.StartTime.Substring(0, 2)) == 12)
						element.StartTime = (int.Parse(element.StartTime.Substring(0, 2))).ToString() + ":" + element.StartTime.Substring(3, 2) + " PM";
					else
						element.StartTime = (int.Parse(element.StartTime.Substring(0, 2)) - 12).ToString() + ":" + element.StartTime.Substring(3, 2) + " PM";
				}
				else
				{
					element.StartTime = element.StartTime + " AM";
				}

				if (int.Parse(element.EndTime.Substring(0, 2)) >= 12)
				{

					if (int.Parse(element.EndTime.Substring(0, 2)) == 12)
						element.EndTime = (int.Parse(element.EndTime.Substring(0, 2))).ToString() + ":" + element.EndTime.Substring(3, 2) + " PM";
					else
						element.EndTime = (int.Parse(element.EndTime.Substring(0, 2)) - 12).ToString() + ":" + element.EndTime.Substring(3, 2) + " PM";
				}
				else
				{
					element.EndTime = element.EndTime + " AM";
				}
				Events ev = new Events();
				Users usr = new Users();
				
				Users userObj = usr.GetUserById(element.User_id);

				organiserDict.Add(element.EventId, userObj);

				
				attendingResult = ev.VerifyIfUserIsAttendingEvent(userId, element.EventId);
				userName = element.GetAllUserNameByUserId(element.User_id);
				checkJoinStatus.Add(element.EventId, attendingResult);
				attending = element.getAllParticipants(element.EventId);
				participantList.Add(element.EventId, attending.Count);
			}


		}

		protected void leaveBtn_Click(object sender, EventArgs e)
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
	}
}