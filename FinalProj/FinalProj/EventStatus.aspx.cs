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
    public partial class EventStatus : System.Web.UI.Page
    {
        protected List<EventsStatus> evStListTemp;
        protected List<EventsStatus> evStList = new List<EventsStatus>();

        protected List<EventsStatus> evStListEventCounter;
        protected List<EventsStatus> evStListParticipateCounter;


        protected List<int> eventIds;
        protected List<int> counterId;

        public List<string> complete = new List<string>();
        public string viewingUserId;
        public int eventCount = 0;
        public int participated = 0;
        public int total = 0;
        int forCount = 0;
        protected Sessionmg sesDeets;




        protected void Page_Load(object sender, EventArgs e)
        {
            viewingUserId = Request.QueryString["userId"];

            EventsStatus evSt = new EventsStatus();
            Users usr = new Users();



            if (Session["user"] == null)
            {
                Response.Redirect("homepage.aspx");
            }
            else
            {
                Sessionmg ses = new Sessionmg();
                blocked bl = new blocked();

                sesDeets = ses.GetSession(Session["email"].ToString());
                if (sesDeets.Active == 1)
                {
                    if (radioButtonList.SelectedIndex == 0)
                    {
                        msg.Visible = false;

                        if (viewingUserId != null)
                        {

                            // This is a counter
                            evStListEventCounter = evSt.GetAllEventsByName();
                            counterId = evSt.GetAllEventsParticipate(int.Parse(viewingUserId));
                            evStListParticipateCounter = evSt.GetAllEventsEventId(counterId);

                            foreach (var element in evStListParticipateCounter)
                            {
                                participated += 1;
                            }

                            foreach (var element in evStListEventCounter)
                            {
                                if (element.Organiser == viewingUserId)
                                {
                                    eventCount += 1;
                                }
                            }

                            if (eventCount == 0)
                            {
                                msg.Visible = true;
                            }

                            evStListTemp = evSt.GetAllEventsByName();
                            Users viewingUser = new Users();
                            usr = viewingUser.GetUserById(int.Parse(viewingUserId));

                            for (int i = 0; i < evStListTemp.Count; i++)
                            {
                                if (evStListTemp[i].Organiser == viewingUserId)
                                {
                                    evStListTemp[i].OrganiserPic = usr.DPimage;
                                    evStListTemp[i].OrganiserName = usr.name;
                                    evStListTemp[i].Attendee = 0;
                                    evStList.Add(evStListTemp[i]);
                                }
                            }

                            foreach (EventsStatus element in evStList)
                            {
                                element.NumCompleted = eventCount;

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


                                string date = element.Date + " " + element.EndTime;
                                DateTime dt1 = DateTime.Parse(date);
                                DateTime dt2 = DateTime.Now;

                                int result = DateTime.Compare(dt1, dt2);

                                if (result < 0 || result == 0)
                                    element.Completed = "Complete";

                                forCount += 1;
                            }
                        }
                        else
                        {

                            // This is a counter
                            evStListEventCounter = evSt.GetAllEventsByName();
                            counterId = evSt.GetAllEventsParticipate(int.Parse(Session["id"].ToString()));
                            evStListParticipateCounter = evSt.GetAllEventsEventId(counterId);

                            foreach (var element in evStListParticipateCounter)
                            {
                                participated += 1;
                            }

                            foreach (var element in evStListEventCounter)
                            {
                                if (element.Organiser == Session["id"].ToString())
                                {
                                    eventCount += 1;
                                }
                            }

                            if (eventCount == 0)
                            {
                                msg.Visible = true;
                            }

                            Users viewingUser = new Users();
                            usr = viewingUser.GetUserById(int.Parse(Session["id"].ToString()));

                            evStListTemp = evSt.GetAllEventsByName();

                            for (int i = 0; i < evStListTemp.Count; i++)
                            {
                                if (evStListTemp[i].Organiser == usr.id.ToString())
                                {
                                    evStListTemp[i].OrganiserPic = usr.DPimage;
                                    evStListTemp[i].OrganiserName = usr.name;
                                    evStList.Add(evStListTemp[i]);
                                }
                            }


                            foreach (EventsStatus element in evStList)
                            {
                                element.NumCompleted = eventCount;

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

                                string date = element.Date + " " + element.EndTime;
                                DateTime dt1 = DateTime.Parse(date);
                                DateTime dt2 = DateTime.Now;

                                int result = DateTime.Compare(dt1, dt2);

                                if (result < 0 || result == 0)
                                    element.Completed = "Complete";

                                forCount += 1;
                            }
                        }
                    }
                    if (radioButtonList.SelectedIndex == 1)
                    {
                        msg.Visible = false;

                        if (viewingUserId != null)
                        {

                            Users viewingUser = new Users();

                            eventIds = evSt.GetAllEventsParticipate(int.Parse(viewingUserId));

                            evStListTemp = evSt.GetAllEventsEventId(eventIds);

                            //this is a counter
                            evStListEventCounter = evSt.GetAllEventsByName();
                            counterId = evSt.GetAllEventsParticipate(int.Parse(viewingUserId));
                            evStListParticipateCounter = evSt.GetAllEventsEventId(counterId);

                            foreach (var element in evStListParticipateCounter)
                            {
                                participated += 1;
                            }

                            if (participated == 0)
                            {
                                msg.Visible = true;
                            }

                            foreach (var element in evStListEventCounter)
                            {
                                if (element.Organiser == viewingUserId)
                                {
                                    eventCount += 1;
                                }
                            }

                            for (int i = 0; i < evStListTemp.Count; i++)
                            {
                                usr = viewingUser.GetUserById(int.Parse(evStListTemp[i].Organiser));

                                evStListTemp[i].Organiser = usr.id.ToString();
                                evStListTemp[i].OrganiserPic = usr.DPimage;
                                evStListTemp[i].OrganiserName = usr.name;
                                evStList.Add(evStListTemp[i]);
                            }

                            foreach (EventsStatus element in evStList)
                            {
                                element.NumCompleted = eventCount;

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


                                string date = element.Date + " " + element.EndTime;
                                DateTime dt1 = DateTime.Parse(date);
                                DateTime dt2 = DateTime.Now;

                                int result = DateTime.Compare(dt1, dt2);

                                if (result < 0 || result == 0)
                                    element.Completed = "Complete";

                                forCount += 1;
                            }
                        }
                        else
                        {
                            Users viewingUser = new Users();

                            eventIds = evSt.GetAllEventsParticipate(int.Parse(Session["id"].ToString()));
                            evStListTemp = evSt.GetAllEventsEventId(eventIds);

                            // This is a counter
                            evStListEventCounter = evSt.GetAllEventsByName();
                            counterId = evSt.GetAllEventsParticipate(int.Parse(Session["id"].ToString()));
                            evStListParticipateCounter = evSt.GetAllEventsEventId(counterId);

                            foreach (var element in evStListParticipateCounter)
                            {
                                participated += 1;
                            }

                            if (participated == 0)
                            {
                                msg.Visible = true;
                            }

                            foreach (var element in evStListEventCounter)
                            {
                                if (element.Organiser == Session["id"].ToString())
                                {
                                    eventCount += 1;
                                }
                            }


                            for (int i = 0; i < evStListTemp.Count; i++)
                            {
                                usr = viewingUser.GetUserById(int.Parse(evStListTemp[i].Organiser));

                                evStListTemp[i].Organiser = usr.id.ToString();
                                evStListTemp[i].OrganiserPic = usr.DPimage;
                                evStListTemp[i].OrganiserName = usr.name;
                                evStList.Add(evStListTemp[i]);
                            }

                            foreach (EventsStatus element in evStList)
                            {
                                element.NumCompleted = eventCount;

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

                                string date = element.Date + " " + element.EndTime;
                                DateTime dt1 = DateTime.Parse(date);
                                DateTime dt2 = DateTime.Now;

                                int result = DateTime.Compare(dt1, dt2);

                                if (result < 0 || result == 0)
                                    element.Completed = "Complete";

                                forCount += 1;
                            }
                        }
                    }
                    total = eventCount + participated;
                }
                else {
                    if (bl.GetBlockedAccWithEmail(Session["email"].ToString()) != null)
                    {
                        Session.Clear();
                        string err = "SessionBanned";
                        Response.Redirect("homepage.aspx?error=" + err);
                    }
                }
            }
        }

        protected void radioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }
    }
}