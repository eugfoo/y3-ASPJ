using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class EventsStatus
    {
        // Define class properties
        public string Title { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Desc { get; set; }
        public string Pic { get; set; }
        public string Organiser { get; set; }
        public string OrganiserPic { get; set; }
        public string OrganiserName { get; set; }
        public string Completed { get; set; }
        public int NumCompleted { get; set; }
        public int Attendee { get; set; }

        public EventsStatus()
        {

        }
        // Define a constructor to initialize all the properties
        public EventsStatus(string eventTitle, string eventId, string eventVenue, string eventDate, string eventStartTime,
            string eventEndTime, string eventDesc, string eventPic, string eventOrganiser, string eventCompletion,
            string eventOrganiserPic, int eventNum)
        {
            Title = eventTitle;
            Id = eventId;
            Name = eventVenue;
            Date = eventDate;
            StartTime = eventStartTime;
            EndTime = eventEndTime;
            Desc = eventDesc;
            Pic = eventPic;
            Organiser = eventOrganiser;
            OrganiserPic = eventOrganiserPic;
            OrganiserName = "";
            Completed = eventCompletion;
            NumCompleted = eventNum;
            Attendee = 1;
        }

        public List<EventsStatus> GetAllEventsByName()
        {
            eventStatusDAO evst = new eventStatusDAO();
            return evst.SelectAllByName();
        }

        public List<int> GetAllEventsParticipate(int id)
        {

        AttendanceDAO attend = new AttendanceDAO();
            return attend.GetEventIdByParticipate(id);
        }

        public List<EventsStatus> GetAllEventsEventId(List<int> ids)
        {
            eventStatusDAO evst = new eventStatusDAO();
            return evst.SelectAllByEventId(ids);
        }
    }
}