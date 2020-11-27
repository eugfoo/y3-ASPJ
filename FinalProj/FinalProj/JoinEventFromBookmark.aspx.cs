using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
	public partial class JoinEventFromBookmark : System.Web.UI.Page
	{
		protected int userId;
        protected string userName;
		protected void Page_Load(object sender, EventArgs e)
		{

			if (Session["user"] != null)
			{
				Users user = (Users)Session["user"];
				userId = user.id;
                userName = user.name;
				Events ev = new Events();

			}
			else
			{
				Response.Redirect("/homepage.aspx");
			}

			if (Request.QueryString["eventId"] == null)
				Response.Redirect("/bookmark.aspx");
			else
			{

				Events ev = new Events();
				var organiserId = ev.getEventDetails(int.Parse(Request.QueryString["eventId"])).User_id;
				if (organiserId != userId)
				{
					int result = ev.AddParticipant(userId, int.Parse(Request.QueryString["eventId"]), userName);
					if (result == 1)
						Session["SessionSSM"] = "Successfully Joined the event!";
					else if(result == -1)
						Session["SessionERM"] = "You already joined an event within this timeframe!";
					else
						Session["SessionERM"] = "Oops! Something Went Wrong!";
					Response.Redirect("/bookmark.aspx");
				}
				else
				{
					Response.Redirect("/eventDetails?=eventId" + Request.QueryString["eventId"]);
				}
			}
		}
	}
}