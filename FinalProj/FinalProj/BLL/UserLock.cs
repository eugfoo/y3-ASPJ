using FinalProj.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProj.BLL
{
    public class UserLock
    {

		public int userLockId { get; set; }
		public string userEmail { get; set; }
		public string userName { get; set; }
		public DateTime dateTime { get; set; }
		public int userLock { get; set; }



		public UserLock() { }
		public UserLock(int Id, string ulEmail, string ulName, DateTime dateT, int ulLock)
		{
			userLockId = Id;
			userEmail = ulEmail;
			userName = ulName;
			dateTime = dateT;
			userLock = ulLock;
		}

		public List<UserLock> GetAllLogsOfUser(string email)
		{
			UserLockDAO elg = new UserLockDAO();
			return elg.SelectByEmail(email);
		}
		public List<UserLock> GetAllLogsFromId(int Id)
		{
			UserLockDAO elg = new UserLockDAO();
			return elg.SelectById(Id);
		}

		public int AddEmailLog(string userEmail, string userName, DateTime dateTime, int ulLock)
		{
			UserLockDAO lgDao = new UserLockDAO();
			int result = lgDao.Insert(userEmail, userName, dateTime, ulLock);
			return result;
		}
	}
}