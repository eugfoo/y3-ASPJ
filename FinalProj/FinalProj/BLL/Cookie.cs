using FinalProj.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProj.BLL
{
    public class Cookie
    {

		public int cookieId { get; set; }
		public string userEmail { get; set; }
		public string userCookieCH { get; set; }
		public string userCookieIE { get; set; }



		public Cookie() { }
		public Cookie(int Id, string ckUEmail, string cookieCH, string cookieIE)
		{
			cookieId = Id;
			userEmail = ckUEmail;
			userCookieCH = cookieCH;
			userCookieIE = cookieIE;
		}

		public Cookie GetCookiesFromEmail(string email)
		{
			CookieDAO ck = new CookieDAO();
			return ck.SelectByEmail(email);
		}
		public Cookie GetCookeiesFromId(int Id)
		{
			CookieDAO ck = new CookieDAO();
			return ck.SelectById(Id);
		}

		public int AddCookiesCH(string userEmail, string ckCH)
		{
			CookieDAO ck = new CookieDAO();
			int result = ck.InsertChrome(userEmail, ckCH);
			return result;
		}
		public int AddCookiesIE(string userEmail, string ckIE)
		{
			CookieDAO ck = new CookieDAO();
			int result = ck.InsertIE(userEmail, ckIE);
			return result;
		}
		public int UpdateCookiesCH(string userEmail, string ckCH)
		{
			CookieDAO ck = new CookieDAO();
			int result = ck.UpdateCHCookie(userEmail, ckCH);
			return result;
		}
		public int UpdateCookiesIE(string userEmail, string ckIE)
		{
			CookieDAO ck = new CookieDAO();
			int result = ck.UpdateIECookie(userEmail, ckIE);
			return result;
		}
		public int AddEmail(string userEmail, string ckCH, string ckIE)
		{
			CookieDAO ck = new CookieDAO();
			int result = ck.InsertEmail(userEmail, ckCH, ckIE);
			return result;
		}
	}
}
