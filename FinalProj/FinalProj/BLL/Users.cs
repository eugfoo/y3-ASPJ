﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
	public class Users
	{
		public int id { get; set; }
		public string email { get; set; }
		public string passHash { get; set; }
		public string name { get; set; }
		public string DPimage { get; set; }
		public string BPimage { get; set; }
		public string desc { get; set; }
		public string participate { get; set; }
		public int rating { get; set; }
		public string isOrg { get; set; }
		public double points { get; set; }
		public int verified { get; set; }
		public string regDate { get; set; }
		public string facebook { get; set; }
		public string instagram { get; set; }
		public string twitter { get; set; }
		public string diet { get; set; }
		public int twofactor { get; set; }
		public int googleauth { get; set; }
		public string googleKey { get; set; }
		public string passSalt { get; set; }
		public string verifyImage { get; set; }


		public Users() { }

		public Users(string uEmail, string uName, string uIsOrg, string uPassHash, string uRegDate, string uPassSalt)
		{
			name = uName;
			email = uEmail;
			isOrg = uIsOrg;
			passHash = uPassHash;
			regDate = uRegDate;
			passSalt = uPassSalt;
		}

		public Users(int uId, string uEmail, string uPassHash, string uName, string uDPImage, string uBPImage,
			string uDesc, int uRating, string uIsOrg, double uPoints, string uParticipate, int uVerified, string uRegDate,
			string uFacebook, string uInstagram, string uTwitter, string uDiet, int uTwoFactor, int googleAut, string googleKe, string uPassSalt, string uVerifyImage)
		{
			id = uId;
			email = uEmail;
			passHash = uPassHash;
			name = uName;
			DPimage = uDPImage;
			BPimage = uBPImage;
			desc = uDesc;
			rating = uRating;
			isOrg = uIsOrg;
			points = uPoints;
			participate = uParticipate;
			verified = uVerified;
			regDate = uRegDate;
			facebook = uFacebook;
			instagram = uInstagram;
			twitter = uTwitter;
            diet = uDiet;
			twofactor = uTwoFactor;
			googleauth = googleAut;
			googleKey = googleKe;
			passSalt = uPassSalt;
			verifyImage = uVerifyImage;
		}

		public int AddUser()
		{
			userDAO user = new userDAO();
			int result = user.Insert(this);
			return result;
		}

		public Users GetUserByEmail(string email)
		{
			userDAO user = new userDAO();
			return user.SelectByEmail(email);
		}


        public Users GetUserById(int id)
        {
            userDAO user = new userDAO();
            return user.SelectById(id);
        }

        public List<Users> getAllUsers()
        {
            userDAO dao = new userDAO();
            return dao.getAllUser();
        }

        public int VerifyOrgByEmail(string email)
        {
            userDAO user = new userDAO();
            return user.VerifyOrgByEmail(email);
        }

        public int UpdateDietByID(int id, string diet)
        {
            userDAO user = new userDAO();
            return user.UpdateDiet(id, diet);
        }

        public int UpdateRatingByID(int id)
        {
            userDAO user = new userDAO();
            return user.UpdateRating(id);
        }

        public int UpdateNameByID(int id, string name)
		{
			userDAO user = new userDAO();
			return user.UpdateName(id, name);
		}

		public int UpdatePassByID(int id, string passHash)
		{
			userDAO user = new userDAO();
			return user.UpdatePass(id, passHash);
		}

		public int UpdateDescByID(int id, string desc)
		{
			userDAO user = new userDAO();
			return user.UpdateDesc(id, desc);
		}

        public int UpdatePointsByID(int id, double points)
        {
            userDAO user = new userDAO();
            return user.UpdatePoints(id, points);
        }

        public int UpdateDPByID(int id, string DP)
		{
			userDAO user = new userDAO();
			return user.UpdateDP(id, DP);
		}

		public int UpdateBPByID(int id, string BP)
		{
			userDAO user = new userDAO();
			return user.UpdateBP(id, BP);
		}

		public int UpdateFacebookByID(int id, string fb)
		{
			userDAO user = new userDAO();
			return user.UpdateFacebook(id, fb);
		}

		public int UpdateInstagramByID(int id, string inst)
		{
			userDAO user = new userDAO();
			return user.UpdateInstagram(id, inst);
		}

		public int UpdateTwitterByID(int id, string twit)
		{
			userDAO user = new userDAO();
			return user.UpdateTwitter(id, twit);
		}
		public int UpdateTwoFactorByID(int id, int twofactor)
		{
			userDAO user = new userDAO();
			return user.UpdateTwoFactor(id, twofactor);
		}

		public int UpdateGoogleAuthByID(int id, string secretKey, int number)
		{
			userDAO user = new userDAO();
			return user.UpdateGoogleAuthenticator(id, secretKey, number);
		}

		public int UpdateVerifyImage(int id, string path)
		{
			userDAO user = new userDAO();
			return user.UpdateVerifyImage(id, path);
		}

		public int getLastUserId()
        {
            userDAO dao = new userDAO();
            int result = dao.getLastUserId();
            return result;
        }

		public List<Users> getUnverifiedUsers()
        {
			userDAO dao = new userDAO();
			return dao.getUnverifiedUsers();
		}

    }
}
