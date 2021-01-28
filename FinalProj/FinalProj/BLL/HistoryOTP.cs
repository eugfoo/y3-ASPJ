using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class HistoryOTP
    {
		public int userID { get; set; }
		public string userEmail { get; set; }
		public string userOTP { get; set; }
		public int userOTPCheck { get; set; }


		public HistoryOTP() { }
		public HistoryOTP(int Id, string Email, string OTP, int OTPCheck)
		{
			userID = Id;
			userEmail = Email;
			userOTP = OTP;
			userOTPCheck = OTPCheck;
		}

		public List<HistoryOTP> GetAllHistoryOTPs()
		{
			HistoryOTPDAO ad = new HistoryOTPDAO();
			return ad.SelectAllHistoryOTPs();
		}


		public int AddHistoryOTP(string Email, string OTP, int OTPChecked)
		{
			HistoryOTPDAO adDao = new HistoryOTPDAO();
			int result = adDao.Insert(Email, OTP, OTPChecked);
			return result;
		}

		public bool GetUserByEmail(string email)
		{
			HistoryOTPDAO user = new HistoryOTPDAO();
			return user.SelectByEmail(email);
		}

		public HistoryOTP GetUserByEmailOTP(string email)
        {
			HistoryOTPDAO user = new HistoryOTPDAO();
			return user.SelectByEmailOTP(email);
        }

		public int UpdateOTPByEmail(string email, string otp, int otpcheck)
		{
			HistoryOTPDAO user = new HistoryOTPDAO();
			return user.UpdateOTP(email, otp, otpcheck);
		}

	}
}
