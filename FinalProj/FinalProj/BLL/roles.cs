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
		public int RoleId { get; set; }
		public int viewAppLogs { get; set; }
		public int mgCollab { get; set; }
		public int mgVouch { get; set; }



		public roles() { }
		public roles(int id, string roles, int appLogs, int mgCollaborators, int mgVouchers)
		{
			RoleId = id;
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

		public roles GetRole(string name)
		{
			RolesDAO rl = new RolesDAO();
			return rl.SelectRole(name);
		}



		public int UpdatePermsByRole(string roles, int appLogs, int mgCollaborators, int mgVouchers)
		{
			RolesDAO rl = new RolesDAO();
			return rl.UpdatePerms(roles, appLogs, mgCollaborators, mgVouchers);
		}

		public int UpdateRole(int id, string newRoleName, int appLogs, int mgCollaborators, int mgVouchers)
		{
			RolesDAO rl = new RolesDAO();
			return rl.UpdateRole(id, newRoleName, appLogs, mgCollaborators, mgVouchers);
		}

		public int InsertRole(string roles, int appLogs, int mgCollab, int mgVouch) {
			RolesDAO rl = new RolesDAO();
			return rl.Insert(roles, appLogs, mgCollab, mgVouch);
		}

		public int DeleteRole(string roles)
		{
			RolesDAO rl = new RolesDAO();
			return rl.DeleteRole(roles);
		}
	}
}