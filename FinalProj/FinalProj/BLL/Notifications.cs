using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class Notifications
    {
        // Define class properties
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int User_id { get; set; }
        public int Attend { get; set; }
        public int Feedback { get; set; }

        public Notifications()
        {

        }
        // Define a constructor to initialize all the properties
        public Notifications(int eventId, string eventName, int userId, int attend, int feedback)
        {
            EventId = eventId;
            EventName = eventName;
            User_id = userId;
            Attend = attend;
            Feedback = feedback;
        }

        public List<Notifications> GetEventsEnded()
        {
            notificationsDAO noti = new notificationsDAO();
            return noti.SelectAllEventsEnd();
        }

    }
}