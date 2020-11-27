using FinalProj.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProj.BLL
{
    public class Attendance
    {
        // Define class properties
        public string UserId { get; set; }
        public string EventId { get; set; }
        public string UserName { get; set; }
        public int Attend { get; set; }
        public int Feedback { get; set; }

        public Attendance()
        {

        }

        // Define a constructor to initialize all properties
        public Attendance(string userId, string eventId, string userName, int attend, int feedback)
        {
            UserId = userId;
            EventId = eventId;
            UserName = userName;
            Attend = attend;
            Feedback = feedback;
        }

        public int AddAttend()
        {
            AttendanceDAO dao = new AttendanceDAO();
            int result = dao.Insert(this);
            return result;
        }

        public List<Attendance> GetAttendances()
        {
            AttendanceDAO voucher = new AttendanceDAO();
            return voucher.SelectAllByAttend();
        }

        public int UpdateAttendanceById(int id, int attend)
        {
            AttendanceDAO attendance = new AttendanceDAO();
            return attendance.UpdateAttendance(id, attend);
        }

        public List<Attendance> GetAttendanceEvent(int id)
        {
            AttendanceDAO voucher = new AttendanceDAO();
            return voucher.SelectAllByEventId(id);
        }

        public int UpdateFeedbackByUserIdEventId(int UserId, int eventId, int feedback)
        {
            AttendanceDAO attendance = new AttendanceDAO();
            return attendance.UpdateFeedbackByUserIdEventId(UserId, eventId, feedback);
        }
    }
}
