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
		public int mgBan { get; set; }
		public int mgAdLg { get; set; }



		public roles() { }
		public roles(int id, string roles, int appLogs, int mgCollaborators, int mgVouchers, int mgBans, int mgAdmingLog)
		{
			RoleId = id;
			Roles = roles;
			viewAppLogs = appLogs;
			mgCollab = mgCollaborators;
			mgVouch = mgVouchers;
			mgBan = mgBans;
			mgAdLg = mgAdmingLog;


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



		public int UpdatePermsByRole(string roles, int appLogs, int mgCollaborators, int mgVouchers, int mgBans, int mgAdmingLog)
		{
			RolesDAO rl = new RolesDAO();
			return rl.UpdatePerms(roles, appLogs, mgCollaborators, mgVouchers,  mgBans, mgAdmingLog);
		}

		public int UpdateRole(int id, string newRoleName, int appLogs, int mgCollaborators, int mgVouchers, int mgBans, int mgAdmingLog)
		{
			RolesDAO rl = new RolesDAO();
			return rl.UpdateRole(id, newRoleName, appLogs, mgCollaborators, mgVouchers,  mgBans, mgAdmingLog);
		}

		public int InsertRole(string roles, int appLogs, int mgCollab, int mgVouch, int mgBans, int mgAdmingLog) {
			RolesDAO rl = new RolesDAO();
			return rl.Insert(roles, appLogs, mgCollab, mgVouch,  mgBans, mgAdmingLog);
		}

		public int DeleteRole(string roles)
		{
			RolesDAO rl = new RolesDAO();
			return rl.DeleteRole(roles);
		}
	}
}