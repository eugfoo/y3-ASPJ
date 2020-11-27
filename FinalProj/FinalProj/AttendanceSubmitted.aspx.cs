using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class AttendanceSubmitted : System.Web.UI.Page
    {
        protected Events eventDetail;
        protected List<Attendance> attendList;
        protected List<Users> attendUser = new List<Users>();
        public List<string> diet = new List<string>();
        public List<string> attending = new List<string>();
        public string title;
        public int participant = 0;
        public int participantHere = 0;
        public string endTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["eventTitle"] == null)
            {
                Response.Redirect("homepage.aspx");
            }
            else
            {
                Attendance attend = new Attendance();
                Users user = new Users();
                title = Session["eventTitle"].ToString();


                if (Session["user"] == null)
                {
                    Response.Redirect("homepage.aspx");
                }
                else
                {
                    Events ev = new Events();
                    eventDetail = ev.getEventDetails(int.Parse(Session["eventId"].ToString()));

                    int index = eventDetail.Date.IndexOf(" ");
                    eventDetail.Date = eventDetail.Date.Substring(0, index);
                    eventDetail.StartTime = eventDetail.StartTime.Substring(0, 5);


                    endTime = eventDetail.Date + " " + eventDetail.EndTime;

                    DateTime dt1 = DateTime.Parse(endTime);
                    DateTime dt2 = DateTime.Now;

                    int result = DateTime.Compare(dt1, dt2);

                    if (result < 0 || result == 0)
                    {
                        btnEdit.Visible = false;
                        btnEnd.Visible = true;
                    }
                    else
                    {
                        btnEdit.Visible = true;
                        btnEnd.Visible = false;
                    }


                    int id = int.Parse(Session["eventId"].ToString());
                    attendList = attend.GetAttendanceEvent(id);
                    for (int i = 0; i < attendList.Count; i++)
                    {
                        // Total number of participants
                        participant += 1;

                        int userId = int.Parse(attendList[i].UserId.ToString());
                        attendUser.Add(user.GetUserById(userId)); // Get user's name and diet

                        if (attendUser[i].diet == "")
                        {
                            diet.Add("Unspecified");
                        }
                        else
                        {
                            diet.Add(user.GetUserById(userId).diet);
                        }

                        if (attendList[i].Attend == 0)
                        {
                            attending.Add("No");
                        }
                        if (attendList[i].Attend == 1)
                        {
                            attending.Add("Yes");
                        }

                        // Count number of participants here already
                        if (int.Parse(attendList[i].Attend.ToString()) == 1)
                        {
                            participantHere += 1;
                        }

                    }
                }
            }

        }
    }
}