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

		public Admins GetAllAdminWithEmail(string adminEmail)
		{
			AdminDAO ad = new AdminDAO();
			return ad.SelectAdmin(adminEmail);
		}



		public int AddAdmin(string Name, string Email, string Role)
		{
			AdminDAO adDao = new AdminDAO();
			int result = adDao.Insert(Name, Email, Role);
			return result;
		}

		public int UpdateStatusByEmail(string email, string status)
		{
			AdminDAO adDao = new AdminDAO();
			return adDao.UpdateStatus(email, status);
		}

		public int DeleteByEmail(string email, string name)
		{
			AdminDAO adDao = new AdminDAO();
			return adDao.DeleteAdmins(email, name);
		}


		public int UpdateRoleByEmail(string email, string role) {
			AdminDAO ad = new AdminDAO();
			return ad.UpdateRole(email, role);
		}

	}
}