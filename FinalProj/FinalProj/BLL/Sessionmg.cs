using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class Sessionmg
    {
		public string userEmail { get; set; }
		public int Active { get; set; }
		


		public Sessionmg() { }
		public Sessionmg(string email, int activeSes)
		{
			userEmail = email;
			Active = activeSes;
			
		}

		public List<Sessionmg> GetAllSessions()
		{
			SessionMgDAO sesMg = new SessionMgDAO();
			return sesMg.SelectAllSessions();
		}

		public Sessionmg GetSession(string email)
		{
			SessionMgDAO sesMg = new SessionMgDAO();
			return sesMg.SelectSession(email);
		}

		public int InsertSession(string email, int active)
		{
			SessionMgDAO sesMg = new SessionMgDAO();
			return sesMg.Insert(email, active);
		}

		public int UpdateSession(string email, int active)
		{
			SessionMgDAO sesMg = new SessionMgDAO();
			return sesMg.UpdateSession(email, active);
		}
	}
}