using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
	public partial class Corporate : System.Web.UI.Page
	{ 
        protected List<Events> evList;
		protected Dictionary<int, string> userList = new Dictionary<int, string>();
        protected Dictionary<int, int> userIdList = new Dictionary<int, int>();
        protected Dictionary<int, int> attendingUsers = new Dictionary<int, int>();
		protected List<Users> participantList = new List<Users>();
		protected string setCalendarDate = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			loadDates("no");
			if (Session["user"] == null)
			{
				createEvent.Enabled = false;
			}
			
		}

		protected void loadDates(string all)
		{
			Events ev = new Events();
			DateTime currentDate;

			currentDate = DateTime.Now.Date;

			if (all == "no")
				evList = ev.GetAllEventsByEDate(currentDate);
			else
				evList = ev.GetAllEvents();
			List<int> attending;
			foreach (Events element in evList)                  // loops through each event list and changes formatting of both time and date
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
				userList.Add(element.EventId, ev.GetAllUserNameByUserId(element.User_id));
				userIdList.Add(element.EventId, element.User_id);
				attending = element.getAllParticipants(element.EventId);
				attendingUsers.Add(element.EventId, attending.Count);

			}
		}
		
		protected void DateClicked(object sender, EventArgs e)
		{

			
			Events ev = new Events();
			DateTime currentDate;
			currentDate = Convert.ToDateTime(hidingDate.Text);
			evList = ev.GetAllEventsByEDate(currentDate);

			string dateOnly = hidingDate.Text;
			String[] dateList = dateOnly.Split('/');
			string dateFinal = dateList[2] + "-" + dateList[1] + "-" + dateList[0];
			setCalendarDate = dateFinal;
			List<int> attending;
			userList.Clear();
			userIdList.Clear();
			attendingUsers.Clear();
			foreach (Events element in evList)                  // loops through each event list and changes formatting of both time and date
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
				userList.Add(element.EventId, ev.GetAllUserNameByUserId(element.User_id));
				userIdList.Add(element.EventId, element.User_id);
				attending = element.getAllParticipants(element.EventId);
				attendingUsers.Add(element.EventId, attending.Count);
			}
		}

		protected void createEvent_Click(object sender, EventArgs e)
		{
			Response.Redirect("createEvent.aspx");
		}

        protected void showAvailableEvnts_CheckedChanged(object sender, EventArgs e)
        {
            userList.Clear();
            userIdList.Clear();
            attendingUsers.Clear();
            if (showAvailableEvnts.Checked == true)
            {
                loadDates("yes");
                calPanel.Visible = false;
            }
            else
            {
                loadDates("no");
                calPanel.Visible = true;
            }
        }

    }
}