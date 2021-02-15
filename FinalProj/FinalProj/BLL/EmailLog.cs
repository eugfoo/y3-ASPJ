using FinalProj.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProj.BLL
{
    public class EmailLog
    {

		public int emailLogId { get; set; }
		public string userEmail { get; set; }
		public string senderEmail { get; set; }
		public DateTime dateTime { get; set; }
		public string emailTitle { get; set; }



		public EmailLog() { }
		public EmailLog(int Id, string emailTo, string emailFrom, DateTime dateT, string title)
		{
			emailLogId = Id;
			userEmail = emailTo;
			senderEmail = emailFrom;
			dateTime = dateT;
			emailTitle = title;
		}

		public List<EmailLog> GetAllLogsOfUser(string email)
		{
			EmailLogDAO elg = new EmailLogDAO();
			return elg.SelectByEmail(email);
		}
		public List<EmailLog> GetAllLogsFromId(int Id)
		{
			EmailLogDAO elg = new EmailLogDAO();
			return elg.SelectById(Id);
		}

		public int AddEmailLog(string userEmail, string senderEmail, DateTime dateTime, string emailTitle)
		{
			EmailLogDAO lgDao = new EmailLogDAO();
			int result = lgDao.Insert(userEmail,senderEmail, dateTime, emailTitle);
			return result;
		}
	}
}