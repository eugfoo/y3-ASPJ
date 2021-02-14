using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
	public partial class RemoveBookmark : System.Web.UI.Page
	{
		protected int userId;
		protected Sessionmg sesDeets;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["user"] != null)
			{
				Sessionmg ses = new Sessionmg();
				blocked bl = new blocked();

				sesDeets = ses.GetSession(Session["email"].ToString());
				if (sesDeets.Active == 1)
				{
					Users user = (Users)Session["user"];
					userId = user.id;
					Events ev = new Events();
				}
				else{
					if (bl.GetBlockedAccWithEmail(Session["email"].ToString()) != null)
					{
						Session.Clear();
						string err = "SessionBanned";
						Response.Redirect("homepage.aspx?error=" + err);
					}
				}

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
				int result = ev.removeBookmark(userId, int.Parse(Request.QueryString["eventId"]));
				if (result == 1)
					Session["SessionSSM"] = "Successfully removed the bookmark!";
				else
					Session["SessionERM"] = "Oops! Something Went Wrong!";
				Response.Redirect("/bookmark.aspx");
			}
		}
	}
}