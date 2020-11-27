using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
	public partial class RemoveEventFromBookmark : System.Web.UI.Page
	{
		protected int userId;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["user"] != null)
			{
				Users user = (Users)Session["user"];
				userId = user.id;
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
				int result = ev.RemoveParticipant(userId, int.Parse(Request.QueryString["eventId"]));
				if (result == 1)
					Session["SessionSSM"] = "You successfully left the event!";
				else
					Session["SessionERM"] = "Oops! Something Went Wrong!";
				Response.Redirect("/bookmark.aspx");
			}
		}
	}
}