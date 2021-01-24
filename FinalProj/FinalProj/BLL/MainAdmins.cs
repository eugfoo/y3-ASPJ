using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
	public class MainAdmins
	{
		public int MainadminId { get; set; }

		public string MainadminName { get; set; }

		public string MainadminEmail { get; set; }

		public string MainAdminPassword { get; set; }

		public MainAdmins() { }

		public MainAdmins(int Id, string Name, string Email, string Password)
		{
			MainadminId = Id;
			MainadminName = Name;
			MainadminEmail = Email;
			MainAdminPassword = Password;
		}
		public int AddMainAdmin()
		{
			MainAdminsDAO dao = new MainAdminsDAO();
			int result = dao.Insert(this);
			return result;
		}


		public List<MainAdmins> SelectAllMainAdmins()
		{
			MainAdminsDAO dao = new MainAdminsDAO();
			return dao.SelectAllMainAdmins();
		}


		public MainAdmins GetAdminByEmail(string Email)
		{
			MainAdminsDAO dao = new MainAdminsDAO();
			return dao.SelectByEmail(Email);
		}
	}
}