using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class adminLog
    {
		public DateTime DateTime { get; set; }

		public string Username { get; set; }

		public string ipAddr { get; set; }

		public string Action { get; set; }

		public string ViolationType { get; set; }

		public string userEmail { get; set; }

		public string userCountry { get; set; }



		public adminLog() { }
		public adminLog(DateTime dtime, string username, string ip, string action, string violation, string email, string country)
		{
			Username = username;
			userEmail = email;
			DateTime = dtime;
			ipAddr = ip;
			Action = action;
			ViolationType = violation;
			userCountry = country;
		}

		public List<adminLog> GetAllAdminLogs()
		{
			adminLogsDAO alg = new adminLogsDAO();
			return alg.SelectAllAdminLogs();
		}

		public int AddAdminLog(DateTime dtime, string username, string ip, string action, string violation, string email, string country)
		{
			adminLogsDAO algDao = new adminLogsDAO();
			int result = algDao.InsertAdminLog(dtime, username, ip, action, violation, email, country);
			return result;
		}
	}
}