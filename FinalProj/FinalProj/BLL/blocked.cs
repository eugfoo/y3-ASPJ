using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class blocked
    {
		public int id { get; set; }
		public string email { get; set; }
	
		public string name { get; set; }
		public string reason { get; set; }
		public DateTime dateTimeBlock { get; set; }
		


		public blocked() { }

		public blocked(string uEmail, string uName, string banReason, DateTime dtBlock)
		{
			name = uName;
			email = uEmail;
			reason = banReason;
			dateTimeBlock = dtBlock;

		}

		public List<blocked> getAllBlockedUsers()
		{
			blockedDAO bdao = new blockedDAO();
			return bdao.getAllBlockedUsers();
		}

		public int AddBlockedAcc(string uEmail, string uName, string banReason, DateTime dtBlock)
		{
			blockedDAO bdao = new blockedDAO();
			int result = bdao.Insert(uEmail, uName, banReason, dtBlock);
			return result;
		}

		public int unblock(string uEmail) {
			blockedDAO bdao = new blockedDAO();
			int result = bdao.unBlockAcc(uEmail);
			return result;
		}
		public blocked GetBlockedAccWithEmail(string email)
		{
			blockedDAO bdao = new blockedDAO();
			return bdao.SelectBlockedAcc(email);
		}

	}
}