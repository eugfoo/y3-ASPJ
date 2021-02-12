using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class collabOTP
    {
		public int userID { get; set; }
		public string userEmail { get; set; }
		public string userName { get; set; }
		public string userOTP { get; set; }
		public int OTPStatus { get; set; }


		public collabOTP() { }
		public collabOTP(int Id, string Email, string name, string OTP, int OTPstatus)
		{
			userID = Id;
			userEmail = Email;
			userName = name;
			userOTP = OTP;
			OTPStatus = OTPstatus;
		}

		public int AddCollabOTP(string userEmail, string userName, string userOTP, int OTPStatus)
		{
			collablOTPDAO cotp = new collablOTPDAO();
			return cotp.InsertOTP(userEmail, userName, userOTP, OTPStatus);
		}

		public collabOTP GetUserByEmailOTP(string email)
		{
			collablOTPDAO cotp = new collablOTPDAO();
			return cotp.SelectByEmailOTP(email);
		}

		public int UpdateOTPByEmail(string email, string otp, int otpcheck)
		{
			collablOTPDAO cotp = new collablOTPDAO();
			return cotp.UpdateOTP(email, otp, otpcheck);
		}

		public int DeleteByEmail(string email, string status)
		{
			collablOTPDAO cotp = new collablOTPDAO();
			return cotp.DeleteOTP(email, status);
		}
	}
}