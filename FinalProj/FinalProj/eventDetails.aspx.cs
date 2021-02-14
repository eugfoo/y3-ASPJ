using FinalProj.BLL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
		protected Sessionmg sesDeets;

		protected void Page_Load(object sender, EventArgs e)
        {
			if (Session["user"] != null)
			{
				Sessionmg ses = new Sessionmg();
				blocked bl = new blocked();

				if (Convert.ToBoolean(Session["subadmin"]))
				{
					sesDeets = ses.GetSession(Session["subadminEmail"].ToString());
				}
				else if (Convert.ToBoolean(Session["admin"]))
				{
					sesDeets = ses.GetSession(Session["adminEmail"].ToString());

				}
				else
				{
					sesDeets = ses.GetSession(Session["email"].ToString());
				}
				if (sesDeets.Active == 1)
				{
					Users user = (Users)Session["user"];
					userId = user.id;
				}
				else
				{
					if (bl.GetBlockedAccWithEmail(Session["email"].ToString()) != null)
					{
						Session.Clear();
						string err = "SessionBanned";
						Response.Redirect("homepage.aspx?error=" + err);
					}
				}

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
					Logs lg = new Logs();
					Users us = new Users();
					ActivityLog alg = new ActivityLog();
					DateTime dtLog = DateTime.Now;

					string ipAddr = GetIPAddress();
					CityStateCountByIp(ipAddr);


					string countryLogged = CityStateCountByIp(ipAddr);
				
					alg.AddActivityLog(dtLog, user.name, ipAddr, "Event Joined: " + eventDetail.Title, "-", user.email, countryLogged);

					Session["SessionSSM"] = "You have successfully joined the event!";

					
					Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
					
				}
				else if(result == -1)
				{
					Logs lg = new Logs();
					Users us = new Users();
					ActivityLog alg = new ActivityLog();
					DateTime dtLog = DateTime.Now;

					string ipAddr = GetIPAddress();
					CityStateCountByIp(ipAddr);


					string countryLogged = CityStateCountByIp(ipAddr);

					alg.AddActivityLog(dtLog, user.name, ipAddr, "Tried to join: " + eventDetail.Title, "Event Clash", user.email, countryLogged);

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

		public static string CityStateCountByIp(string IP)
		{
			//var url = "http://freegeoip.net/json/" + IP;
			//var url = "http://freegeoip.net/json/" + IP;
			string url = "http://api.ipstack.com/" + IP + "?access_key=01ca9062c54c48ef1b7d695b008cae00";
			var request = System.Net.WebRequest.Create(url);
			WebResponse myWebResponse = request.GetResponse();
			Stream stream = myWebResponse.GetResponseStream();

			using (StreamReader reader = new StreamReader(stream))
			{
				string json = reader.ReadToEnd();
				JObject objJson = JObject.Parse(json);
				string Country = objJson["country_name"].ToString();
				string Country_code = objJson["country_code"].ToString();
				if (Country == "")
				{
					Country = "-";
				}

				else if (Country_code == "")
				{
					Country = Country;
				}
				else
				{
					Country = Country + " (" + Country_code + ")";
				}
				return Country;

			}

		}



		protected string GetIPAddress()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

			if (!string.IsNullOrEmpty(ipAddress))
			{
				string[] addresses = ipAddress.Split(',');
				if (addresses.Length != 0)
				{
					return addresses[0];
				}
			}
			return context.Request.ServerVariables["REMOTE_ADDR"];
		}

		protected void leaveEvent_Click(object sender, EventArgs e)
		{

			Users user = (Users)Session["user"];
			userId = user.id;
			Events parti = new Events();
			int result = parti.RemoveParticipant(userId, int.Parse(Request.QueryString["eventId"]));
			if (result == 1)
			{

				Logs lg = new Logs();
				Users us = new Users();
				ActivityLog alg = new ActivityLog();
				DateTime dtLog = DateTime.Now;

				string ipAddr = GetIPAddress();
				CityStateCountByIp(ipAddr);


				string countryLogged = CityStateCountByIp(ipAddr);

				alg.AddActivityLog(dtLog, user.name, ipAddr, "Event Left: " + eventDetail.Title, "-", user.email, countryLogged);
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
					Logs lg = new Logs();
					Users us = new Users();
					ActivityLog alg = new ActivityLog();
					DateTime dtLog = DateTime.Now;

					string ipAddr = GetIPAddress();
					CityStateCountByIp(ipAddr);


					string countryLogged = CityStateCountByIp(ipAddr);

					alg.AddActivityLog(dtLog, user.name, ipAddr, "Event Bookmarked: " + eventDetail.Title, "-", user.email, countryLogged);

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
				Logs lg = new Logs();
				Users us = new Users();
				ActivityLog alg = new ActivityLog();
				DateTime dtLog = DateTime.Now;

				string ipAddr = GetIPAddress();
				CityStateCountByIp(ipAddr);


				string countryLogged = CityStateCountByIp(ipAddr);

				alg.AddActivityLog(dtLog, user.name, ipAddr, "Event Bookmark removed: " + eventDetail.Title, "-", user.email, countryLogged);
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