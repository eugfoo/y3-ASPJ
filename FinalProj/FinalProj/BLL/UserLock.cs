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
		public int userAttempts { get; set; }



		public UserLock() { }
		public UserLock(int Id, string ulEmail, string ulName, DateTime dateT, int ulLock, int uAttempts)
		{
			userLockId = Id;
			userEmail = ulEmail;
			userName = ulName;
			dateTime = dateT;
			userLock = ulLock;
			userAttempts = uAttempts;
		}

		public UserLock GetLockStatusByEmail(string email)
		{
			UserLockDAO uld = new UserLockDAO();
			return uld.SelectByEmail(email);
		}
		public List<UserLock> GetAllLockUsers()
		{
			UserLockDAO elg = new UserLockDAO();
			return elg.SelectAllLock();
		}

		public int AddEmail(string userEmail, string userName, DateTime dateTime, int ulLock)
		{
			UserLockDAO lgDao = new UserLockDAO();
			int result = lgDao.InsertEmail(userEmail, userName, dateTime, ulLock);
			return result;
		}
		public int UpdateAttempts(string userEmail,int ulAttempts)
		{
			UserLockDAO lgDao = new UserLockDAO();
			int result = lgDao.UpdateAttempts(userEmail,ulAttempts);
			return result;
		}
		public int UpdateStatus(string userEmail ,DateTime dateTime, int ulLock)
		{
			UserLockDAO lgDao = new UserLockDAO();
			int result = lgDao.UpdateLockStatus(userEmail, dateTime, ulLock);
			return result;
		}
	}
}