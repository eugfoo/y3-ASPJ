using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class Admins
    {
        public int adminId { get; set; }
        public string adminName { get; set; }
		public string adminEmail { get; set; }

		public string adminRole { get; set; }
        public string adminStatus { get; set; }

		public Admins() { }
		public Admins(int Id, string Name, string Role, string Status, string Email)
		{
			adminId = Id;
			adminName = Name;
			adminRole = Role;
			adminStatus = Status;
			adminEmail = Email;
		}

		public List<Admins> GetAllAdmins()
		{
			AdminDAO ad = new AdminDAO();
			return ad.SelectAllAdmins();
		}


		public int AddAdmin(string Name, string Email)
		{
			AdminDAO adDao = new AdminDAO();
			int result = adDao.Insert(Name, Email);
			return result;
		}
	}
}