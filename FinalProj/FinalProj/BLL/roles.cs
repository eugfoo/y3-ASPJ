using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class roles
    {

		public string Roles { get; set; }



		public roles() { }
		public roles(string roles)
		{
			Roles = roles;
		}

		public List<roles> GetAllRoles()
		{
			RolesDAO alg = new RolesDAO();
			return alg.SelectAllRoles();
		}
	}
}