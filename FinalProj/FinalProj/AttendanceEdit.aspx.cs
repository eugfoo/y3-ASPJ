using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class AttendanceEdit : System.Web.UI.Page
    {
        protected Events eventDetail;
        protected List<Attendance> attendList;
        protected List<Users> attendUser = new List<Users>();
        public List<string> diet = new List<string>();
        public List<string> attending = new List<string>();
        public List<string> check = new List<string>();
        public string title;
        public int participant = 0;
        public int participantHere = 0;
        public string endTime;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] == null || Session["eventTitle"] == null)
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


                Attendance attend = new Attendance();
                Users user = new Users();
                title = Session["eventTitle"].ToString();

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


                    if (attendList[i].Attend == 1)
                    {
                        System.Diagnostics.Debug.WriteLine("This is check true");
                        check.Add("true");
                        attending.Add("Present: Yes");

                        // Count number of participants here already
                        participantHere += 1;
                    }

                    if (attendList[i].Attend == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("This is check false");
                        check.Add("false");
                        attending.Add("Present: No");
                    }

                    foreach (var element1 in check)
                    {
                        System.Diagnostics.Debug.WriteLine("This is element 1: " + element1);

                    }
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Attendance attend = new Attendance();

            string hidden = HiddenField.Value;
            string hidden1 = HiddenField1.Value;

            if (hidden == "" && hidden1 == "")
            {
                Response.Redirect("AttendanceSubmitted.aspx");
            }
            else
            {
                string[] jsArrayChecked = hidden.Split(",".ToCharArray());
                string[] jsArrayUnchecked = hidden1.Split(",".ToCharArray());

                foreach (var element in jsArrayChecked)
                {
                    System.Diagnostics.Debug.WriteLine("This is checked: " + element);
                    if (element != "")
                    {
                        int id = int.Parse(element);
                        attend.UpdateAttendanceById(id, 1);
                    }

                }

                foreach (var element in jsArrayUnchecked)
                {
                    System.Diagnostics.Debug.WriteLine("This is unchecked: " + element);
                    if (element != "")
                    {
                        int id = int.Parse(element);
                        attend.UpdateAttendanceById(id, 0);
                    }

                }
                Response.Redirect("AttendanceSubmitted.aspx");
            }

        }
    }
}