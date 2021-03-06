﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class Logs
    {
		public int logId { get; set; }
		public string userEmail { get; set; }
		public DateTime DateTime { get; set; }

		public string ipAddr { get; set; }
		public string Country { get; set; }

		public string result { get; set; }


		public Logs() { }
		public Logs(int Id, string email, DateTime dtime, string ip, string location, string lresult)
		{
			logId = Id;
			userEmail = email;
			DateTime = dtime;
			ipAddr = ip;
			Country = location;
			result = lresult; 
		}

		public List<Logs> GetAllLogsOfUser(string email)
		{
			LogsDAO lg = new LogsDAO();
			return lg.SelectByEmail(email);
		}

		public int AddLog(string userEmail, DateTime DateTime, string ipAddr, string Country, string lresult)
		{
			LogsDAO lgDao = new LogsDAO();
			int result = lgDao.Insert(userEmail, DateTime, ipAddr, Country, lresult) ;
			return result;
		}
	}
}