using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
    public partial class editEventDetails : System.Web.UI.Page
    {
		protected bool bookmark;
		protected int userId;
		protected string userName;
		protected Users profilePic;
		protected Events eventDetail;
		protected List<Users> participantList = new List<Users>();
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
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
				Events ev = new Events();
				eventDetail = ev.getEventDetails(int.Parse(Request.QueryString["eventId"]));
				if (eventDetail.User_id != userId)
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
					PanelError.Visible = true;
					errmsgTb.Text = Session["SessionERM"].ToString();
					Session["SessionERM"] = null;
				}

				desc.Text = AntiXssEncoder.HtmlEncode(eventDetail.Desc.ToString(), true);
				eventAddress.Text = AntiXssEncoder.HtmlEncode(eventDetail.Venue.ToString(), true);
				startTimeEdit.Text = AntiXssEncoder.HtmlEncode(eventDetail.StartTime.ToString(), true);
				endTime.Text = AntiXssEncoder.HtmlEncode(eventDetail.EndTime.ToString(), true);
				eventTitle.Text = AntiXssEncoder.HtmlEncode(eventDetail.Title.ToString(), true);
				noteText.Text = AntiXssEncoder.HtmlEncode(eventDetail.Note.ToString(), true);
				maxAttend.Text = AntiXssEncoder.HtmlEncode(eventDetail.MaxAttendees.ToString(), true);
				eventPic.ImageUrl = "Img/" + eventDetail.Pic.ToString();

				string dateOnly = eventDetail.Date.Substring(0, eventDetail.Date.IndexOf(" "));
				String[] dateList = dateOnly.Split('/');
				string dayFormat = dateList[0].Length == 1 ? 0 + dateList[0] : dateList[0];
				string monthFormat = dateList[1].Length == 1 ? 0 + dateList[1] : dateList[1];
				string dateFinal = dateList[2] + "-" + monthFormat + "-" + dayFormat;
				eventDate.Text = dateFinal;



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

				List<int> listOfUserId = ev.getAllParticipants(eventDetail.EventId);
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
		}

		
		protected void changetoDefaultBorder()
		{
			eventTitle.BorderColor = System.Drawing.Color.LightGray;
			eventAddress.BorderColor = System.Drawing.Color.LightGray;
			eventDate.BorderColor = System.Drawing.Color.LightGray;
			startTimeEdit.BorderColor = System.Drawing.Color.LightGray;
			endTime.BorderColor = System.Drawing.Color.LightGray;
			maxAttend.BorderColor = System.Drawing.Color.LightGray;
			FileUploadControl.BackColor = System.Drawing.Color.White;
			desc.BorderColor = System.Drawing.Color.LightGray;

		}

		protected void saveEdit_Click(object sender, EventArgs e)
		{
			 changetoDefaultBorder();

            Events ev = new Events();
			eventDetail = ev.getEventDetails(int.Parse(Request.QueryString["eventId"]));
			List<int> listOfUserId = ev.getAllParticipants(eventDetail.EventId);
			Users usr = new Users();

			foreach (int usrId in listOfUserId)
			{
				Users name = usr.GetUserById(usrId);

				participantList.Add(name);
			}
			
			profilePic = usr.GetUserById(eventDetail.User_id);
			string errmsg = "";
            PanelError.Visible = false;

            if (eventTitle.Text.ToString() == "")
            {
                errmsg = "Title cannot be empty! <br>";
                eventTitle.BorderColor = System.Drawing.Color.Red;

            }
            if (eventAddress.Text.ToString() == "")
            {
                errmsg += "Address cannot be empty! <br>";
                eventAddress.BorderColor = System.Drawing.Color.Red;
            }
            if (eventDate.Text.ToString() == "")
            {
                errmsg += "Date cannot be empty! <br>";
                eventDate.BorderColor = System.Drawing.Color.Red;
            }
            if (eventDate.Text.ToString() != "")
            {
                string date = eventDate.Text.ToString();
                DateTime dt = Convert.ToDateTime(date);
               
                if (dt < DateTime.Now.Date)
                {
                    errmsg += "Please enter a valid date <br>";
                    eventDate.BorderColor = System.Drawing.Color.Red;
                }

            }
            if (startTimeEdit.Text.ToString() == "")
            {
                errmsg += "StartTime cannot be empty! <br>";
				startTimeEdit.BorderColor = System.Drawing.Color.Red;
            }
            if (endTime.Text.ToString() == "")
            {
                errmsg += "EndTime cannot be empty! <br>";
                endTime.BorderColor = System.Drawing.Color.Red;
            }
            if (startTimeEdit.Text.ToString() != "" && endTime.Text.ToString() != "")
            {
                string startTimeNumber = "";
                string endTimeNumber = "";
                string eventStartTime = AntiXssEncoder.HtmlEncode(startTimeEdit.Text.ToString(), true);
                string eventEndTime = AntiXssEncoder.HtmlEncode(endTime.Text.ToString(), true);
                string startFrontdigits = AntiXssEncoder.HtmlEncode(eventStartTime.Substring(0, 2), true);
                string endFrontdigits = AntiXssEncoder.HtmlEncode(eventEndTime.Substring(0, 2), true);
                string startBackdigits = AntiXssEncoder.HtmlEncode(eventStartTime.Substring(3, 2), true);
                string endBackdigits = AntiXssEncoder.HtmlEncode(eventEndTime.Substring(3, 2), true);
                startTimeNumber = startFrontdigits + startBackdigits;
                endTimeNumber = endFrontdigits + endBackdigits;

                if (int.Parse(startTimeNumber) > int.Parse(endTimeNumber))
                {
                    errmsg += "Please ensure that you entered a valid Start & End Time <br>";
					startTimeEdit.BorderColor = System.Drawing.Color.Red;
                    endTime.BorderColor = System.Drawing.Color.Red;
                }

				if ((int.Parse(endTimeNumber) - int.Parse(startTimeNumber)) < 100)
				{
					errmsg += "Duration must be 1 hour bare minimum";
					startTimeEdit.BorderColor = System.Drawing.Color.Red;
					endTime.BorderColor = System.Drawing.Color.Red;
				}
			}
		

			if (DateTime.Parse(eventDate.Text.ToString() + " " + startTimeEdit.Text.ToString()) <= DateTime.Now)
			{
				errmsg += "Please ensure that you entered a valid Start Time";
				startTimeEdit.BorderColor = System.Drawing.Color.Red;
			}
			if (maxAttend.Text.ToString() == "")
            {
                errmsg += "Maximum number of attendees cannot be empty! <br>";
                maxAttend.BorderColor = System.Drawing.Color.Red;
            }
			if (int.Parse(maxAttend.Text.ToString()) < participantList.Count)
			{
				errmsg += "You cannot have a maximum number attendees less than or eual to the current number of participants ! <br>";
				maxAttend.BorderColor = System.Drawing.Color.Red;
			}
			if (desc.Text.ToString() == "")
            {
                errmsg += "Description cannot be empty! <br>";
                desc.BorderColor = System.Drawing.Color.Red;
            }

            if (desc.Text.ToString() != "")
            {
                int enterCount = 0, index = 0;

                while (index < desc.Text.Length)
                {
                    // check if current char is part of a word
                    if (desc.Text[index] == '\r' && desc.Text[index + 1] == '\n')
                        enterCount++;
						index++;
                }
                if (desc.Text.Length > 3000 + enterCount)
                {
                    errmsg += "Character Limit in Description Exceeded! <br>";
                    desc.BorderColor = System.Drawing.Color.Red;
                }
            }
  
            if (errmsg != "")
            {
                errmsgTb.Text = errmsg;
                PanelError.Visible = true;

            }
            else
            {
				Users user = (Users)Session["user"];
				string eventStartTime = AntiXssEncoder.HtmlEncode(startTimeEdit.Text.ToString(), true);
                string eventEndTime = AntiXssEncoder.HtmlEncode(endTime.Text.ToString(), true);
                string title = AntiXssEncoder.HtmlEncode(eventTitle.Text.ToString(), true);
                string venue = AntiXssEncoder.HtmlEncode(eventAddress.Text.ToString(), true);
                string date = AntiXssEncoder.HtmlEncode(eventDate.Text.ToString(), true);
                int maxAttendees = int.Parse(AntiXssEncoder.HtmlEncode(maxAttend.Text.ToString(), true));
                string description = AntiXssEncoder.HtmlEncode(desc.Text.ToString(), true);
                string picture = "";
                string note = AntiXssEncoder.HtmlEncode(noteText.Text.ToString(), true);
				int user_id = user.id;
				

				if (FileUploadControl.HasFile)
                {

					string filename = Path.GetFileName(FileUploadControl.PostedFile.FileName);
					FileUploadControl.SaveAs(Server.MapPath("~/Img/" + filename));
					picture = filename;
					

				}
                else if (FileUploadControl.HasFile == false)
                {
                    string filename = eventDetail.Pic;
                    picture = filename;
                }

				int rating = 0;
				DateTime dt = DateTime.Now;
				ev = new Events(int.Parse(Request.QueryString["eventId"]), title, venue, date, eventStartTime, eventEndTime, maxAttendees, description, picture, note, rating, user_id, dt);
				int result = ev.updateEvent();
				Debug.WriteLine(result.ToString());
				Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
            }
        }

		protected void cancelEdit_Click(object sender, EventArgs e)
		{
			Response.Redirect("/eventDetails.aspx?eventId=" + Request.QueryString["eventId"]);
		}

		
	}
}