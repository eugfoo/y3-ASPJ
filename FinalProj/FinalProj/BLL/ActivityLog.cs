using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class ActivityLog
    {
		public DateTime DateTime { get; set; }

		public string Username { get; set; }

		public string ipAddr { get; set; }

		public string Action { get; set; }

		public string ViolationType { get; set; }

		public string userEmail { get; set; }

		public string userCountry { get; set; }



		public ActivityLog() { }
		public ActivityLog(DateTime dtime, string username, string ip, string action, string violation, string email, string country)
		{
			Username = username;
			userEmail = email;
			DateTime = dtime;
			ipAddr = ip;
			Action = action;
			ViolationType = violation;
			userCountry = country;
		}

		public List<ActivityLog> GetAllLogsOfActivities()
		{
			LogsDAO alg = new LogsDAO();
			return alg.SelectAllActivityLogs();
		}

		public int AddActivityLog(DateTime dtime, string username, string ip, string action, string violation, string email, string country)
		{
			LogsDAO algDao = new LogsDAO();
			int result = algDao.InsertActivity(dtime, username, ip, action, violation, email, country);
			return result;
		}
	}
}