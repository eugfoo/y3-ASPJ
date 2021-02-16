using FinalProj.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProj.BLL
{
	public class userProfilePrivacy
	{
		public int userProfileID { get; set; }
		public string userEmail { get; set; }
		public int userProfileIsPublic { get; set; }



		public userProfilePrivacy() { }
		public userProfilePrivacy(int Id, string email, int uIsProfilePublic)
		{
			userProfileID = Id;
			userEmail = email;
			userProfileIsPublic = uIsProfilePublic;

		}
		public List<userProfilePrivacy> GetUserByEmail(string email)
		{
			userProfilePrivacyDAO profilePriv = new userProfilePrivacyDAO();
			return profilePriv.SelectByEmail(email);
		}

		public int UpdateProfileVisibilityByID(int id, int userIsProfilePublic)
		{
			userProfilePrivacyDAO profilePriv = new userProfilePrivacyDAO();
			return profilePriv.UpdateProfileVisibility(id, userIsProfilePublic);
		}

		public int AddEmail(string userEmail)
		{
			userProfilePrivacyDAO lgDao = new userProfilePrivacyDAO();
			int result = lgDao.Insert(userEmail);
			return result;
		}
	}
}