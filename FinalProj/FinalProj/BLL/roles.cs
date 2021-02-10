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
		public int viewAppLogs { get; set; }
		public int mgCollab { get; set; }
		public int mgVouch { get; set; }



		public roles() { }
		public roles(string roles, int appLogs, int mgCollaborators, int mgVouchers)
		{
			Roles = roles;
			viewAppLogs = appLogs;
			mgCollab = mgCollaborators;
			mgVouch = mgVouchers;
		}

		public List<roles> GetAllRoles()
		{
			RolesDAO rl = new RolesDAO();
			return rl.SelectAllRoles();
		}
		public int UpdatePermsByRole(string roles, int appLogs, int mgCollaborators, int mgVouchers)
		{
			RolesDAO rl = new RolesDAO();
			return rl.UpdatePerms(roles, appLogs, mgCollaborators, mgVouchers);
		}

		public int UpdateRoleName(string oldRoleName, string newRoleName)
		{
			RolesDAO rl = new RolesDAO();
			return rl.UpdateRoleName(oldRoleName, newRoleName);
		}
	}
}