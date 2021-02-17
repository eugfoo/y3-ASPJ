using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using IpData;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.IO;
using System.Drawing;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;using System.Web.SessionState;
using System.Web.Security.AntiXss;

namespace FinalProj
{
    public partial class LogIn : System.Web.UI.Page
    {
        public string result = "";
        public string OTPEmail = "";
        public string OTPassword = "";
        public string userName = "";        public string senderEmail = "kovitwk21@gmail.com";        public string title = "";        public string code = "";
        public bool blockedAcc = false;

        public string IECookie = "";
        public string CHCookie = "";

        public string cookieCHstr = "";
        public string cookieIEstr = "";

        public string browser = "";
        public int counter;

        public int counter2;
        public int maximum = 2;
        public int remianing;
        public DateTime dt;

        public string tempUserEmail = "";


        List<blocked> blList;

        protected List<EmailLog> elgList;
        protected List<Logs> lgList;
        protected void Page_Load(object sender, EventArgs e)
        {
            browser = Request.Browser.Type.ToUpper();
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                HistoryOTP otp = new HistoryOTP();
                HistoryOTP userTrying = otp.GetUserByEmailOTP(tbEmail.Text);

                MainAdmins mainadmin = new MainAdmins();
                MainAdmins adminlogin = mainadmin.GetAdminByEmail(tbEmail.Text);

                Admins subadmin = new Admins();
                Admins subadminlogin = subadmin.GetAllAdminWithEmail(tbEmail.Text);
                string adminPassHash = ComputeSha256Hash(tbPass.Text);
                string subAdminPassHash = ComputeSha256Hash(tbPass.Text);

                Users user = new Users();
                Users subAdCreds = user.GetUserByEmail(tbEmail.Text);
                string passHash = tbPass.Text.ToString().Trim();

                if (adminlogin != null) //if admin exist
                {
                    if (adminlogin.MainAdminPassword == adminPassHash) 
                    {
                        string ipAddr = GetIPAddress();
                        string countryLogged = CityStateCountByIp(ipAddr);
                        DateTime dtLog = DateTime.Now;
                        Logs lg = new Logs();
                        Sessionmg ses = new Sessionmg();
                        Admins ad = new Admins();
                        adminLog adl = new adminLog();
                        MainAdmins mad = new MainAdmins();
                        Users us = new Users();


                        if (ses.GetSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true)) != null)
                        {
                            // update
                            ses.UpdateSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                        }
                        else
                        {
                            ses.InsertSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                        }
                        Session["adminEmail"] = AntiXssEncoder.HtmlEncode(tbEmail.Text, true);
                        Session["admin"] = true;

                        adl.AddAdminLog(dtLog, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, "Successful Login Attempt", "-", Session["adminEmail"].ToString(), countryLogged);
                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Successful Login Attempt"); // idk why this is affecting the activity logs
                        //start of cookie check                        
                        BLL.Cookie ck = new BLL.Cookie();
                        BLL.Cookie ck2 = ck.GetCookiesFromEmail(tbEmail.Text);
                        HttpCookie cookieCH = Request.Cookies["SessionIDCH"];
                        HttpCookie cookieIE = Request.Cookies["SessionIDIE"];
                        if (browser.Contains("CHROME"))
                        {
                            if (ck2.userCookieCH != null && cookieCH !=null)
                            {
                                if (ck2.userCookieCH != cookieCH.ToString())
                                {
                                    // edit here change to admin
                                    if (mad.GetAdminByEmail(tbEmail.Text) != null)
                                    {
                                        string name = mad.GetAdminByEmail(tbEmail.Text).MainadminName;
                                        lg.AddLog(adminlogin.MainadminEmail, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                        adl.AddAdminLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                    }
                                    else
                                    {
                                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                    }
                                    EmailLog elg = new EmailLog();
                                    DateTime dtelg = DateTime.Now;
                                    title = "New login from new browser";
                                    EmailNewDevice(adminlogin.MainadminEmail, adminlogin.MainadminName, browser);
                                    elg.AddEmailLog(adminlogin.MainadminEmail, senderEmail, dtelg, title);
                                    //Creates new cookie session
                                    Guid guid = Guid.NewGuid();
                                    string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                    HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                    cookie2["sid"] = uSid;
                                    cookie2.Expires = DateTime.Now.AddYears(1);
                                    Response.Cookies.Add(cookie2);
                                    CHCookie = cookie2.ToString();
                                    ck.UpdateCookiesCH(adminlogin.MainadminEmail, CHCookie);
                                }
                            }
                            else
                            {
                                EmailLog elg = new EmailLog();
                                DateTime dtelg = DateTime.Now;
                                title = "New login from new browser";
                                EmailNewDevice(adminlogin.MainadminEmail, adminlogin.MainadminName, browser);
                                elg.AddEmailLog(adminlogin.MainadminEmail, senderEmail, dtelg, title);
                                //Creates new cookie session
                                Guid guid = Guid.NewGuid();
                                string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                HttpCookie cookie2 = new HttpCookie("SessionIDCH");                               
                                cookie2["sid"] = uSid;
                                cookie2.Expires = DateTime.Now.AddYears(1);
                                Response.Cookies.Add(cookie2);
                                CHCookie = cookie2.ToString();
                                ck.UpdateCookiesCH(adminlogin.MainadminEmail, CHCookie);
                            }
                        }
                        else if (browser.Contains("INTERNETEXPLORER"))
                        {
                            if (ck2.userCookieIE != null && cookieIE != null)
                            {
                                if (ck2.userCookieIE != cookieIE.ToString())
                                {
                                    // edit here
                                    if (mad.GetAdminByEmail(tbEmail.Text) != null)
                                    {
                                        string name = mad.GetAdminByEmail(tbEmail.Text).MainadminName;
                                        lg.AddLog(adminlogin.MainadminEmail, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                        adl.AddAdminLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                    }
                                    else
                                    {
                                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                    }
                                    EmailLog elg = new EmailLog();
                                    DateTime dtelg = DateTime.Now;
                                    title = "New login from new browser";
                                    EmailNewDevice(adminlogin.MainadminEmail, adminlogin.MainadminName, browser);
                                    elg.AddEmailLog(adminlogin.MainadminEmail, senderEmail, dtelg, title);
                                    //Creates new cookie session
                                    Guid guid = Guid.NewGuid();
                                    string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                    HttpCookie cookie2 = new HttpCookie("SessionIDIE");
                                    cookie2["sid"] = uSid;
                                    cookie2.Expires = DateTime.Now.AddYears(1);
                                    IECookie = cookie2.ToString();
                                    ck.UpdateCookiesIE(adminlogin.MainadminEmail, IECookie);
                                    Response.Cookies.Add(cookie2);
                                }
                            }
                            else
                            {
                                EmailLog elg = new EmailLog();
                                DateTime dtelg = DateTime.Now;
                                title = "New login from new browser";
                                EmailNewDevice(adminlogin.MainadminEmail, adminlogin.MainadminName, browser);
                                elg.AddEmailLog(adminlogin.MainadminEmail, senderEmail, dtelg, title);
                                //Creates new cookie session
                                Guid guid = Guid.NewGuid();
                                string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                HttpCookie cookie2 = new HttpCookie("SessionIDIE");
                                cookie2["sid"] = uSid;
                                cookie2.Expires = DateTime.Now.AddYears(1);
                                IECookie = cookie2.ToString();
                                ck.UpdateCookiesIE(adminlogin.MainadminEmail, IECookie);
                                Response.Cookies.Add(cookie2);
                            }
                        }
                        //end
                        Response.Redirect("homepage.aspx");
                    }
                    else
                    {
                        lblError.Visible = true;

                        adminLog adl = new adminLog();
                        MainAdmins mad = new MainAdmins();
                        string ipAddr = GetIPAddress();
                        string countryLogged = CityStateCountByIp(ipAddr);
                        DateTime dtLog = DateTime.Now;
                        Logs lg = new Logs();
                        adl.AddAdminLog(dtLog, mad.GetAdminByEmail(AntiXssEncoder.HtmlEncode(tbEmail.Text, true)).MainadminName, ipAddr, "Failed Login Attempt", "Failed Authentication", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                    }
                }
                else if (subadminlogin != null && subadminlogin.adminStatus == "Accepted") // if sub admin exist and is accepted as sub admin
                {
                    string adminDbHash = subAdCreds.passHash;
                    string adminDbSalt = subAdCreds.passSalt;
                    SHA256Managed adminHashing = new SHA256Managed();
                    blocked bl = new blocked();
                    if (adminDbSalt != null && adminDbSalt.Length > 0 && adminDbHash != null && adminDbHash.Length > 0)
                    {
                        string adminPwdWithSalt = passHash + adminDbSalt;
                        byte[] adminHashWithSalt = adminHashing.ComputeHash(Encoding.UTF8.GetBytes(adminPwdWithSalt));
                        string adminHash = Convert.ToBase64String(adminHashWithSalt);
                        if (adminHash.Equals(adminDbHash))
                        {
                            blList = bl.getAllBlockedUsers();

                            foreach (var blckedAcc in blList)
                            {
                                if (blckedAcc.email == tbEmail.Text)
                                {
                                    blockedAcc = true;
                                }
                            }
                            if (!blockedAcc)
                            {
                                adminLog adl = new adminLog();
                                Users us = new Users();
                                Users tryingUser = user.GetUserByEmail(tbEmail.Text);
                                Session["subadmin"] = true;
                                Session["subadminEmail"] = AntiXssEncoder.HtmlEncode(tbEmail.Text, true);
                                string ipAddr = GetIPAddress();
                                string countryLogged = CityStateCountByIp(ipAddr);
                                DateTime dtLog = DateTime.Now;
                                Logs lg = new Logs();
                                //Email function for new sign in
                                lgList = lg.GetAllLogsOfUser(userTrying.userEmail);
                                foreach (var log in lgList)
                                {
                                    if (log.ipAddr == ipAddr)
                                    {
                                        counter++;
                                    }
                                }
                                if (counter == 0)
                                {
                                    EmailLog elg = new EmailLog();
                                    DateTime dtelg = DateTime.Now;
                                    title = "Is this you? Login Clearview";
                                    EmailFxNew(userTrying.userEmail, tryingUser.name, ipAddr, countryLogged);
                                    elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                }
                                BLL.Cookie ck = new BLL.Cookie();
                                BLL.Cookie ck2 = ck.GetCookiesFromEmail(tbEmail.Text);
                                HttpCookie cookieCH = Request.Cookies["SessionIDCH"];
                                HttpCookie cookieIE = Request.Cookies["SessionIDIE"];
                                if (browser.Contains("CHROME"))
                                {
                                    if (ck2.userCookieCH != null && cookieCH != null)
                                    {
                                        if (ck2.userCookieCH != cookieCH.ToString())
                                        {
                                            // edit here
                                            if (us.GetUserByEmail(tbEmail.Text) != null)
                                            {
                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                adl.AddAdminLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                            }
                                            else
                                            {
                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                            }

                                            EmailLog elg = new EmailLog();
                                            DateTime dtelg = DateTime.Now;
                                            title = "New login from new browser";
                                            EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                            //Creates new cookie session
                                            Guid guid = Guid.NewGuid();
                                            string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                            HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                            cookie2["sid"] = uSid;
                                            cookie2.Expires = DateTime.Now.AddYears(1);
                                            Response.Cookies.Add(cookie2);
                                            CHCookie = cookie2.ToString();
                                            ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                        }
                                    }
                                    else
                                    {
                                        EmailLog elg = new EmailLog();
                                        DateTime dtelg = DateTime.Now;
                                        title = "New login from new browser";
                                        EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                        elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                        //Creates new cookie session
                                        Guid guid = Guid.NewGuid();
                                        string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                        HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                        cookie2["sid"] = uSid;
                                        cookie2.Expires = DateTime.Now.AddYears(1);
                                        Response.Cookies.Add(cookie2);
                                        CHCookie = cookie2.ToString();
                                        ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                    }
                                }
                                else if (browser.Contains("INTERNETEXPLORER"))
                                {
                                    if (ck2.userCookieIE != null && cookieIE != null)
                                    {
                                        if (ck2.userCookieIE != cookieIE.ToString())
                                        {
                                            // edit here
                                            if (us.GetUserByEmail(tbEmail.Text) != null)
                                            {
                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                adl.AddAdminLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                            }
                                            else
                                            {
                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                            }
                                            EmailLog elg = new EmailLog();
                                            DateTime dtelg = DateTime.Now;
                                            title = "New login from new browser";
                                            EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                            //Creates new cookie session
                                            Guid guid = Guid.NewGuid();
                                            string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                            HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                            cookie2["sid"] = uSid;
                                            cookie2.Expires = DateTime.Now.AddYears(1);
                                            Response.Cookies.Add(cookie2);
                                            IECookie = cookie2.ToString();
                                            ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                        }
                                    }
                                    else
                                    {
                                        EmailLog elg = new EmailLog();
                                        DateTime dtelg = DateTime.Now;
                                        title = "New login from new browser";
                                        EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                        elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                        //Creates new cookie session
                                        Guid guid = Guid.NewGuid();
                                        string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                        HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                        cookie2["sid"] = uSid;
                                        cookie2.Expires = DateTime.Now.AddYears(1);
                                        Response.Cookies.Add(cookie2);
                                        IECookie = cookie2.ToString();
                                        ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                    }
                                }
                                Sessionmg ses = new Sessionmg();
                                if (ses.GetSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true)) != null) {
                                    // update
                                    ses.UpdateSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                }
                                else { 
                                ses.InsertSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                }
                                adl.AddAdminLog(dtLog, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Successful Login Attempt", "-", Session["subadminEmail"].ToString(), countryLogged);

                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                Response.Redirect("homepage.aspx");
                            }
                            else{
                                lblError.Visible = true;
                                lblError.Text = "Your Account has been banned.";

                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            string ipAddr = GetIPAddress();
                            string countryLogged = CityStateCountByIp(ipAddr);
                            DateTime dtLog = DateTime.Now;
                            Logs lg = new Logs();
                            adminLog adl = new adminLog();
                            Users us = new Users();
                            adl.AddAdminLog(dtLog, us.GetUserByEmail(AntiXssEncoder.HtmlEncode(tbEmail.Text, true)).name, ipAddr, "Failed Login Attempt", "Failed Authentication", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                        }
                    }
                }
                else
                {
                    Users tryingUser = user.GetUserByEmail(tbEmail.Text);
                    if (userTrying != null)
                    {
                        if (userTrying.userOTPCheck == 0)
                        {
                            if (tryingUser != null) // user exists
                            {
                                UserLock ul = new UserLock();
                                UserLock ulUser = ul.GetLockStatusByEmail(tryingUser.email);
                                if (ulUser != null && ulUser.userLock == 1 && ulUser.dateTime >= DateTime.Now)
                                {
                                    lblError.Visible = true;
                                    lblError.Text = "You have reached the maximum amount of attempts. Please try again after 5 minutes";  
                                }
                                else
                                {
                                    string dbHash = tryingUser.passHash;
                                    string dbSalt = tryingUser.passSalt;
                                    SHA256Managed hashing = new SHA256Managed();
                                    blocked bl = new blocked();


                                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                                    {
                                        string pwdWithSalt = passHash + dbSalt;
                                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                                        string userHash = Convert.ToBase64String(hashWithSalt);
                                        if (IsReCaptchaValid())
                                        {
                                            if (userHash.Equals(dbHash))
                                            {
                                                if (tryingUser.twofactor == 1)
                                                {
                                            
                                                    blList = bl.getAllBlockedUsers();

                                                    foreach (var blckedAcc in blList)
                                                    {
                                                        if (blckedAcc.email == tbEmail.Text)
                                                        {
                                                            blockedAcc = true;
                                                        }
                                                    }
                                                    if (!blockedAcc)
                                                    {
                                                        Session["user"] = tryingUser;
                                                        Session["id"] = tryingUser.id;
                                                        Session["Name"] = tryingUser.name;
                                                        Session["Pic"] = tryingUser.DPimage;
                                                        Session["email"] = tryingUser.email;

                                                        string ipAddr = GetIPAddress();
                                                        string countryLogged = CityStateCountByIp(ipAddr);
                                                        CityStateCountByIp(ipAddr);

                                                        DateTime dtLog = DateTime.Now;
                                                        Logs lg = new Logs();
                                                        ActivityLog alg = new ActivityLog();
                                                        Users us = new Users();
                                                        //Email function for new sign in
                                                        lgList = lg.GetAllLogsOfUser(userTrying.userEmail);
                                                        foreach (var log in lgList)
                                                        {
                                                            if (log.ipAddr == ipAddr)
                                                            {
                                                                counter++;
                                                            }
                                                        }
                                                        if (counter == 0)
                                                        {
                                                            EmailLog elg = new EmailLog();
                                                            DateTime dtelg = DateTime.Now;
                                                            title = "Is this you? Login Clearview";
                                                            EmailFxNew(userTrying.userEmail, tryingUser.name, ipAddr, countryLogged);
                                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                        }
                                                        Sessionmg ses = new Sessionmg();
                                                        if (ses.GetSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true)) != null)
                                                        {
                                                            // update
                                                            ses.UpdateSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                                        }
                                                        else
                                                        {
                                                            ses.InsertSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                                        }
                                                        BLL.Cookie ck = new BLL.Cookie();
                                                        BLL.Cookie ck2 = ck.GetCookiesFromEmail(tbEmail.Text);
                                                        HttpCookie cookieCH = Request.Cookies["SessionIDCH"];
                                                        HttpCookie cookieIE = Request.Cookies["SessionIDIE"];
                                                        if (browser.Contains("CHROME"))
                                                        {
                                                            if (ck2.userCookieCH != null && cookieCH != null)
                                                            {
                                                                if (ck2.userCookieCH != cookieCH.ToString())
                                                                {
                                                                    // edit here
                                                                    if (us.GetUserByEmail(tbEmail.Text) != null)
                                                                    {
                                                                        string name = us.GetUserByEmail(tbEmail.Text).name;
                                                                        lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                                        alg.AddActivityLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                                    }
                                                                    else
                                                                    {
                                                                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                                                    }

                                                                    EmailLog elg = new EmailLog();
                                                                    DateTime dtelg = DateTime.Now;
                                                                    title = "New login from new browser";
                                                                    EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                                    elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                                    //Creates new cookie session
                                                                    Guid guid = Guid.NewGuid();
                                                                    string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                                    HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                                    cookie2["sid"] = uSid;
                                                                    cookie2.Expires = DateTime.Now.AddYears(1);
                                                                    Response.Cookies.Add(cookie2);
                                                                    CHCookie = cookie2.ToString();
                                                                    ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                EmailLog elg = new EmailLog();
                                                                DateTime dtelg = DateTime.Now;
                                                                title = "New login from new browser";
                                                                EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                                elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                                //Creates new cookie session
                                                                Guid guid = Guid.NewGuid();
                                                                string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                                HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                                cookie2["sid"] = uSid;
                                                                cookie2.Expires = DateTime.Now.AddYears(1);
                                                                Response.Cookies.Add(cookie2);
                                                                CHCookie = cookie2.ToString();
                                                                ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                                            }
                                                        }
                                                        else if (browser.Contains("INTERNETEXPLORER"))
                                                        {
                                                            if (ck2.userCookieIE != null && cookieIE != null)
                                                            {
                                                                if (ck2.userCookieIE != cookieIE.ToString())
                                                                {
                                                                    // edit here
                                                                    if (us.GetUserByEmail(tbEmail.Text) != null)
                                                                    {
                                                                        string name = us.GetUserByEmail(tbEmail.Text).name;
                                                                        lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                                        alg.AddActivityLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                                    }
                                                                    else
                                                                    {
                                                                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                                                    }
                                                                    EmailLog elg = new EmailLog();
                                                                    DateTime dtelg = DateTime.Now;
                                                                    title = "New login from new browser";
                                                                    EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                                    elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                                    //Creates new cookie session
                                                                    Guid guid = Guid.NewGuid();
                                                                    string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                                    HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                                    cookie2["sid"] = uSid;
                                                                    cookie2.Expires = DateTime.Now.AddYears(1);
                                                                    Response.Cookies.Add(cookie2);
                                                                    IECookie = cookie2.ToString();
                                                                    ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                EmailLog elg = new EmailLog();
                                                                DateTime dtelg = DateTime.Now;
                                                                title = "New login from new browser";
                                                                EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                                elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                                //Creates new cookie session
                                                                Guid guid = Guid.NewGuid();
                                                                string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                                HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                                cookie2["sid"] = uSid;
                                                                cookie2.Expires = DateTime.Now.AddYears(1);
                                                                Response.Cookies.Add(cookie2);
                                                                IECookie = cookie2.ToString();
                                                                ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                                            }
                                                        }
                                                        //Response.Redirect("homepage.aspx");                                                //end

                                                        if (us.GetUserByEmail(tbEmail.Text) != null) // check if its admin or subadmin
                                                        {

                                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                                            lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                                            alg.AddActivityLog(dtLog, name, ipAddr, "Successful Login Attempt", "-", tbEmail.Text, countryLogged);
                                                        }
                                                        else
                                                        {
                                                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                                        }
                                                        dt = DateTime.Now;
                                                        ulUser.UpdateStatus(tryingUser.email, dt, 0);
                                                        ulUser.UpdateAttempts(tryingUser.email, 0);
                                                        Response.Redirect("TwoFactor1.aspx");

                                                        // end
                                                    }
                                                    else
                                                    {
                                                        lblError.Visible = true;

                                                        lblError.Text = "Your Account has been banned.";

                                                    }
                                                }
                                                else
                                                {                                                   
                                                    blList = bl.getAllBlockedUsers();

                                                    foreach (var blckedAcc in blList)
                                                    {
                                                        if (blckedAcc.email == tbEmail.Text)
                                                        {
                                                            blockedAcc = true;
                                                            // show error msg, dont allow login
                                                        }
                                                    }
                                                    if (!blockedAcc)
                                                    {
                                                        Session["user"] = tryingUser;
                                                        Session["id"] = tryingUser.id;
                                                        Session["Name"] = tryingUser.name;
                                                        Session["Pic"] = tryingUser.DPimage;
                                                        Session["email"] = tryingUser.email;

                                                        string ipAddr = GetIPAddress();
                                                        string countryLogged = CityStateCountByIp(ipAddr);
                                                        CityStateCountByIp(ipAddr);

                                                        DateTime dtLog = DateTime.Now;
                                                        Logs lg = new Logs();
                                                        //Email function for new sign in
                                                        lgList = lg.GetAllLogsOfUser(userTrying.userEmail);
                                                        foreach (var log in lgList)
                                                        {
                                                            if (log.ipAddr == ipAddr)
                                                            {
                                                                counter++;
                                                            }
                                                        }
                                                        if (counter == 0)
                                                        {
                                                            EmailLog elg = new EmailLog();
                                                            DateTime dtelg = DateTime.Now;
                                                            title = "Is this you? Login Clearview";
                                                            EmailFxNew(userTrying.userEmail, tryingUser.name, ipAddr, countryLogged);
                                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                        }
                                                        ActivityLog alg = new ActivityLog();
                                                        Users us = new Users();
                                                        Sessionmg ses = new Sessionmg();
                                                        if (ses.GetSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true)) != null)
                                                        {
                                                            // update
                                                            ses.UpdateSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                                        }
                                                        else
                                                        {
                                                            ses.InsertSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                                        }

                                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                                        {
                                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                                            lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                                            alg.AddActivityLog(dtLog, name, ipAddr, "Successful Login Attempt", "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                        }
                                                        else
                                                        {
                                                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                                        }
                                                        BLL.Cookie ck = new BLL.Cookie();
                                                        BLL.Cookie ck2 = ck.GetCookiesFromEmail(tbEmail.Text);
                                                        HttpCookie cookieCH = Request.Cookies["SessionIDCH"];
                                                        HttpCookie cookieIE = Request.Cookies["SessionIDIE"];
                                                        if (browser.Contains("CHROME"))
                                                        {
                                                            if (ck2.userCookieCH != null && cookieCH != null)
                                                            {
                                                                if (ck2.userCookieCH != cookieCH.ToString())
                                                                {
                                                                    // edit here
                                                                    if (us.GetUserByEmail(tbEmail.Text) != null)
                                                            {
                                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                                lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                                alg.AddActivityLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                            }
                                                            else
                                                            {
                                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                                            }

                                                            EmailLog elg = new EmailLog();
                                                            DateTime dtelg = DateTime.Now;
                                                            title = "New login from new browser";
                                                            EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                            //Creates new cookie session
                                                            Guid guid = Guid.NewGuid();
                                                            string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                            HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                            cookie2["sid"] = uSid;
                                                            cookie2.Expires = DateTime.Now.AddYears(1);
                                                            Response.Cookies.Add(cookie2);
                                                            CHCookie = cookie2.ToString();
                                                            ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        EmailLog elg = new EmailLog();
                                                        DateTime dtelg = DateTime.Now;
                                                        title = "New login from new browser";
                                                        EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                        elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                        //Creates new cookie session
                                                        Guid guid = Guid.NewGuid();
                                                        string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                        HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                        cookie2["sid"] = uSid;
                                                        cookie2.Expires = DateTime.Now.AddYears(1);
                                                        Response.Cookies.Add(cookie2);
                                                        CHCookie = cookie2.ToString();
                                                        ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                                    }
                                                }
                                                        else if (browser.Contains("INTERNETEXPLORER"))
                                                {
                                                    if (ck2.userCookieIE != null && cookieIE != null)
                                                    {
                                                        if (ck2.userCookieIE != cookieIE.ToString())
                                                        {
                                                            // edit here
                                                            if (us.GetUserByEmail(tbEmail.Text) != null)
                                                            {
                                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                                lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                                alg.AddActivityLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                            }
                                                            else
                                                            {
                                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                                            }
                                                            EmailLog elg = new EmailLog();
                                                            DateTime dtelg = DateTime.Now;
                                                            title = "New login from new browser";
                                                            EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                            //Creates new cookie session
                                                            Guid guid = Guid.NewGuid();
                                                            string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                            HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                            cookie2["sid"] = uSid;
                                                            cookie2.Expires = DateTime.Now.AddYears(1);
                                                            Response.Cookies.Add(cookie2);
                                                            IECookie = cookie2.ToString();
                                                            ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        EmailLog elg = new EmailLog();
                                                        DateTime dtelg = DateTime.Now;
                                                        title = "New login from new browser";
                                                        EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                        elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                        //Creates new cookie session
                                                        Guid guid = Guid.NewGuid();
                                                        string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                        HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                        cookie2["sid"] = uSid;
                                                        cookie2.Expires = DateTime.Now.AddYears(1);
                                                        Response.Cookies.Add(cookie2);
                                                        IECookie = cookie2.ToString();
                                                        ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                                    }
                                                }
                                                DateTime dt = DateTime.Now;
                                                ulUser.UpdateStatus(tryingUser.email, dt, 0);
                                                ulUser.UpdateAttempts(tryingUser.email, 0);

                                                Response.Redirect("Homepage.aspx");
                                                    }
                                                    else
                                                    {
                                                        lblError.Visible = true;

                                                        lblError.Text = "Your Account has been banned.";
                                                    }

                                                }
                                                
                                            }
                                            else
                                            { 
                                                counter2 = ulUser.userAttempts;
                                                if (counter2 < 5)
                                                {
                                                    counter2 += 1;
                                                    ul.UpdateAttempts(tryingUser.email, counter2);
                                                    remianing = maximum - counter2;
                                                    lblAttempts.Visible = true;
                                                    lblAttempts.Text = "Attempts remaining: " + remianing;
                                                    lblError.Visible = true;
                                                    lblError.Text = "Incorrect account information. Please try again";
                                                    string ipAddr = GetIPAddress();
                                                    string countryLogged = CityStateCountByIp(ipAddr);
                                                    CityStateCountByIp(ipAddr);
                                                    DateTime dtLog = DateTime.Now;

                                                    Logs lg = new Logs();
                                                    ActivityLog alg = new ActivityLog();
                                                    Users us = new Users();
                                                    if (us.GetUserByEmail(tbEmail.Text) != null)
                                                    {
                                                        string name = us.GetUserByEmail(tbEmail.Text).name;
                                                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                                        alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                    }
                                                    else
                                                    {
                                                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                                    }
                                                }
                                                else if (ulUser.userLock == 1)
                                                {
                                                    lblError.Visible = true;
                                                    lblError.Text = "You have exceeeded the amount of tries. Please reset your password or contact an admin to unlock your account.";
                                                    DateTime dt = DateTime.Now;
                                                    ul.UpdateStatus(tryingUser.email, dt, 2);
                                                }
                                                else
                                                {
                                                    EmailLog elg = new EmailLog();
                                                    DateTime dtelg = DateTime.Now;
                                                    title = "Repeated Failed Logins";
                                                    EmailRepeatedLogin(userTrying.userEmail, tryingUser.name);
                                                    elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                    DateTime dt = DateTime.Now.AddMinutes(5);
                                                    ul.UpdateStatus(tryingUser.email, dt, 1);
                                                }

                                            }
                                        }
                                        else
                                        {
                                            lblError.Visible = true;
                                            lblError.Text = "Failed Captcha please try again";
                                            string ipAddr = GetIPAddress();
                                            string countryLogged = CityStateCountByIp(ipAddr);
                                            CityStateCountByIp(ipAddr);
                                            DateTime dtLog = DateTime.Now;

                                            Logs lg = new Logs();
                                            ActivityLog alg = new ActivityLog();
                                            Users us = new Users();
                                            if (us.GetUserByEmail(tbEmail.Text) != null)
                                            {
                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                                alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                            }
                                            else
                                            {
                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        lblError.Visible = true;
                                        string ipAddr = GetIPAddress();
                                        string countryLogged = CityStateCountByIp(ipAddr);
                                        CityStateCountByIp(ipAddr);
                                        DateTime dtLog = DateTime.Now;

                                        Logs lg = new Logs();
                                        ActivityLog alg = new ActivityLog();
                                        Users us = new Users();
                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                        {
                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                            alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                        }
                                        else
                                        {
                                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                        }
                                    }
                                }
                            }                             
                            else
                            {
                                lblError.Visible = true;
                                string ipAddr = GetIPAddress();
                                string countryLogged = CityStateCountByIp(ipAddr);
                                CityStateCountByIp(ipAddr);
                                DateTime dtLog = DateTime.Now;

                                Logs lg = new Logs();
                                ActivityLog alg = new ActivityLog();
                                Users us = new Users();
                                if (us.GetUserByEmail(tbEmail.Text) != null)
                                {
                                    string name = us.GetUserByEmail(tbEmail.Text).name;
                                    lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                    alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                }
                                else
                                {
                                    lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                }
                            }
                        }
                        if (userTrying.userOTPCheck == 1)
                        {
                            string otpSent = tbPass.Text;

                            if (tryingUser != null) // user exists
                            {
                                int counter = 0;

                                string dbHash = tryingUser.passHash;
                                string dbSalt = tryingUser.passSalt;
                                SHA256Managed hashing = new SHA256Managed();
                                blocked bl = new blocked();
                                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                                {
                                    string pwdWithSalt = passHash + dbSalt;
                                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                                    string userHash = Convert.ToBase64String(hashWithSalt);
                                    if (userHash.Equals(dbHash))
                                    {
                                        blList = bl.getAllBlockedUsers();

                                        foreach (var blckedAcc in blList)
                                        {
                                            if (blckedAcc.email == tbEmail.Text)
                                            {
                                                blockedAcc = true;
                                            }
                                        }
                                        if (!blockedAcc)
                                        {
                                            Session["user"] = tryingUser;
                                            Session["id"] = tryingUser.id;
                                            Session["Name"] = tryingUser.name;
                                            Session["Pic"] = tryingUser.DPimage;
                                            Session["email"] = tryingUser.email;

                                            string ipAddr = GetIPAddress();
                                            string countryLogged = CityStateCountByIp(ipAddr);
                                            CityStateCountByIp(ipAddr);

                                            DateTime dtLog = DateTime.Now;
                                            Logs lg = new Logs();
                                            //Email function for new sign in
                                            lgList = lg.GetAllLogsOfUser(userTrying.userEmail);
                                            foreach (var log in lgList)
                                            {
                                                if (log.ipAddr == ipAddr)
                                                {
                                                    counter++;
                                                }
                                            }
                                            if (counter == 0)
                                            {
                                                EmailLog elg = new EmailLog();
                                                DateTime dtelg = DateTime.Now;
                                                title = "Is this you? Login Clearview";
                                                EmailFxNew(userTrying.userEmail, tryingUser.name, ipAddr, countryLogged);
                                                elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                            }
                                            ActivityLog alg = new ActivityLog();
                                            Users us = new Users();
                                            Sessionmg ses = new Sessionmg();
                                            if (ses.GetSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true)) != null)
                                            {
                                                // update
                                                ses.UpdateSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                            }
                                            else
                                            {
                                                ses.InsertSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                            }
                                            if (us.GetUserByEmail(tbEmail.Text) != null)
                                            {
                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                                alg.AddActivityLog(dtLog, name, ipAddr, "Successful Login Attempt", "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                            }
                                            else
                                            {
                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                            }
                                            BLL.Cookie ck = new BLL.Cookie();
                                            BLL.Cookie ck2 = ck.GetCookiesFromEmail(tbEmail.Text);
                                            HttpCookie cookieCH = Request.Cookies["SessionIDCH"];
                                            HttpCookie cookieIE = Request.Cookies["SessionIDIE"];
                                            if (browser.Contains("CHROME"))
                                            {
                                                if (ck2.userCookieCH != null && cookieCH != null)
                                                {
                                                    if (ck2.userCookieCH != cookieCH.ToString())
                                                    {
                                                        // edit here
                                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                                        {
                                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                                            lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                            alg.AddActivityLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                        }
                                                        else
                                                        {
                                                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                                        }

                                                        EmailLog elg = new EmailLog();
                                                        DateTime dtelg = DateTime.Now;
                                                        title = "New login from new browser";
                                                        EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                        elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                        //Creates new cookie session
                                                        Guid guid = Guid.NewGuid();
                                                        string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                        HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                        cookie2["sid"] = uSid;
                                                        cookie2.Expires = DateTime.Now.AddYears(1);
                                                        Response.Cookies.Add(cookie2);
                                                        CHCookie = cookie2.ToString();
                                                        ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                                    }
                                                }
                                                else
                                                {
                                                    EmailLog elg = new EmailLog();
                                                    DateTime dtelg = DateTime.Now;
                                                    title = "New login from new browser";
                                                    EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                    elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                    //Creates new cookie session
                                                    Guid guid = Guid.NewGuid();
                                                    string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                    HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                    cookie2["sid"] = uSid;
                                                    cookie2.Expires = DateTime.Now.AddYears(1);
                                                    Response.Cookies.Add(cookie2);
                                                    CHCookie = cookie2.ToString();
                                                    ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                                }
                                            }
                                            else if (browser.Contains("INTERNETEXPLORER"))
                                            {
                                                if (ck2.userCookieIE != null && cookieIE != null)
                                                {
                                                    if (ck2.userCookieIE != cookieIE.ToString())
                                                    {
                                                        // edit here
                                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                                        {
                                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                                            lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                            alg.AddActivityLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                        }
                                                        else
                                                        {
                                                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                                        }
                                                        EmailLog elg = new EmailLog();
                                                        DateTime dtelg = DateTime.Now;
                                                        title = "New login from new browser";
                                                        EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                        elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                        //Creates new cookie session
                                                        Guid guid = Guid.NewGuid();
                                                        string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                        HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                        cookie2["sid"] = uSid;
                                                        cookie2.Expires = DateTime.Now.AddYears(1);
                                                        Response.Cookies.Add(cookie2);
                                                        IECookie = cookie2.ToString();
                                                        ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                                    }
                                                }
                                                else
                                                {
                                                    EmailLog elg = new EmailLog();
                                                    DateTime dtelg = DateTime.Now;
                                                    title = "New login from new browser";
                                                    EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                    elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                    //Creates new cookie session
                                                    Guid guid = Guid.NewGuid();
                                                    string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                    HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                    cookie2["sid"] = uSid;
                                                    cookie2.Expires = DateTime.Now.AddYears(1);
                                                    Response.Cookies.Add(cookie2);
                                                    IECookie = cookie2.ToString();
                                                    ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                                }
                                            }
                                            Response.Redirect("Homepage.aspx");
                                        }
                                        else
                                        {
                                            lblError.Visible = true;

                                            lblError.Text = "Your Account has been banned.";
                                        }
                                    }
                                    else
                                    {
                                        if (IsReCaptchaValid())
                                        { 
                                            if (userTrying.userOTP == otpSent)
                                            {                                             
                                                blList = bl.getAllBlockedUsers();

                                                foreach (var blckedAcc in blList)
                                                {
                                                    if (blckedAcc.email == tbEmail.Text)
                                                    {
                                                        blockedAcc = true;
                                                    }
                                                }
                                                if (!blockedAcc)
                                                {
                                                    int OTPChecked = 0;
                                                    Session["user"] = tryingUser;
                                                    Session["id"] = tryingUser.id;
                                                    Session["Name"] = tryingUser.name;
                                                    Session["Pic"] = tryingUser.DPimage;
                                                    Session["email"] = tryingUser.email;

                                                    string ipAddr = GetIPAddress();
                                                    string countryLogged = CityStateCountByIp(ipAddr);
                                                    CityStateCountByIp(ipAddr);

                                                    DateTime dtLog = DateTime.Now;
                                                    Logs lg = new Logs();
                                                    //Email function for new sign in
                                                    lgList = lg.GetAllLogsOfUser(userTrying.userEmail);
                                                    foreach (var log in lgList)
                                                    {
                                                        if (log.ipAddr == ipAddr)
                                                        {
                                                            counter++;
                                                        }
                                                    }

                                                    ActivityLog alg = new ActivityLog();
                                                    Users us = new Users();
                                                    Sessionmg ses = new Sessionmg();
                                                    if (ses.GetSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true)) != null)
                                                    {
                                                        // update
                                                        ses.UpdateSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                                    }
                                                    else
                                                    {
                                                        ses.InsertSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                                    }
                                                    if (us.GetUserByEmail(tbEmail.Text) != null)
                                                    {
                                                        string name = us.GetUserByEmail(tbEmail.Text).name;
                                                        lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                                        alg.AddActivityLog(dtLog, name, ipAddr, "Successful Login Attempt", "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                    }
                                                    else
                                                    {
                                                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                                    }

                                                    otp.UpdateOTPByEmail(userTrying.userEmail, OTPassword, OTPChecked);

                                                    if (counter == 0)
                                                    {
                                                        EmailLog elg = new EmailLog();
                                                        DateTime dtelg = DateTime.Now;
                                                        title = "Is this you? Login Clearview";
                                                        EmailFxNew(userTrying.userEmail, tryingUser.name, ipAddr, countryLogged);
                                                        elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                    }
                                                    BLL.Cookie ck = new BLL.Cookie();
                                                    BLL.Cookie ck2 = ck.GetCookiesFromEmail(tbEmail.Text);
                                                    HttpCookie cookieCH = Request.Cookies["SessionIDCH"];
                                                    HttpCookie cookieIE = Request.Cookies["SessionIDIE"];
                                                    if (browser.Contains("CHROME"))
                                                    {
                                                        if (ck2.userCookieCH != null && cookieCH != null)
                                                        {
                                                            if (ck2.userCookieCH != cookieCH.ToString())
                                                            {
                                                                // edit here
                                                                if (us.GetUserByEmail(tbEmail.Text) != null)
                                                                {
                                                                    string name = us.GetUserByEmail(tbEmail.Text).name;
                                                                    lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                                    alg.AddActivityLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                                }
                                                                else
                                                                {
                                                                    lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                                                }

                                                                EmailLog elg = new EmailLog();
                                                                DateTime dtelg = DateTime.Now;
                                                                title = "New login from new browser";
                                                                EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                                elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                                //Creates new cookie session
                                                                Guid guid = Guid.NewGuid();
                                                                string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                                HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                                cookie2["sid"] = uSid;
                                                                cookie2.Expires = DateTime.Now.AddYears(1);
                                                                Response.Cookies.Add(cookie2);
                                                                CHCookie = cookie2.ToString();
                                                                ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            EmailLog elg = new EmailLog();
                                                            DateTime dtelg = DateTime.Now;
                                                            title = "New login from new browser";
                                                            EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                            //Creates new cookie session
                                                            Guid guid = Guid.NewGuid();
                                                            string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                            HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                            cookie2["sid"] = uSid;
                                                            cookie2.Expires = DateTime.Now.AddYears(1);
                                                            Response.Cookies.Add(cookie2);
                                                            CHCookie = cookie2.ToString();
                                                            ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                                        }
                                                    }
                                                    else if (browser.Contains("INTERNETEXPLORER"))
                                                    {
                                                        if (ck2.userCookieIE != null && cookieIE != null)
                                                        {
                                                            if (ck2.userCookieIE != cookieIE.ToString())
                                                            {
                                                                // edit here
                                                                if (us.GetUserByEmail(tbEmail.Text) != null)
                                                                {
                                                                    string name = us.GetUserByEmail(tbEmail.Text).name;
                                                                    lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                                    alg.AddActivityLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                                }
                                                                else
                                                                {
                                                                    lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                                                }
                                                                EmailLog elg = new EmailLog();
                                                                DateTime dtelg = DateTime.Now;
                                                                title = "New login from new browser";
                                                                EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                                elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                                //Creates new cookie session
                                                                Guid guid = Guid.NewGuid();
                                                                string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                                HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                                cookie2["sid"] = uSid;
                                                                cookie2.Expires = DateTime.Now.AddYears(1);
                                                                Response.Cookies.Add(cookie2);
                                                                IECookie = cookie2.ToString();
                                                                ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            EmailLog elg = new EmailLog();
                                                            DateTime dtelg = DateTime.Now;
                                                            title = "New login from new browser";
                                                            EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                            //Creates new cookie session
                                                            Guid guid = Guid.NewGuid();
                                                            string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                            HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                            cookie2["sid"] = uSid;
                                                            cookie2.Expires = DateTime.Now.AddYears(1);
                                                            Response.Cookies.Add(cookie2);
                                                            IECookie = cookie2.ToString();
                                                            ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                                        }
                                                    }
                                                    UserLock ul = new UserLock();
                                                    UserLock ulUser = ul.GetLockStatusByEmail(userTrying.userEmail);
                                                    DateTime dt = DateTime.Now;
                                                    ulUser.UpdateStatus(tryingUser.email, dt, 0);
                                                    ulUser.UpdateAttempts(tryingUser.email, 0);

                                                    Response.Redirect("homepage.aspx");
                                                    //end
                                                }
                                                else
                                                {
                                                    lblError.Visible = true;

                                                    lblError.Text = "Your Account has been banned.";

                                                }
                                            }                                          
                                            else
                                            {
                                                UserLock ul = new UserLock();
                                                UserLock ulUser = ul.GetLockStatusByEmail(userTrying.userEmail);
                                                counter2 = ulUser.userAttempts;
                                                if (counter2 < 5)
                                                {
                                                    counter2 += 1;
                                                    ul.UpdateAttempts(tryingUser.email, counter2);
                                                    remianing = maximum - counter2;
                                                    lblAttempts.Visible = true;
                                                    lblAttempts.Text = "Attempts remaining: " + remianing;
                                                    lblError.Visible = true;
                                                    lblError.Text = "Incorrect account information. Please try again";
                                                    string ipAddr = GetIPAddress();
                                                    string countryLogged = CityStateCountByIp(ipAddr);
                                                    CityStateCountByIp(ipAddr);
                                                    DateTime dtLog = DateTime.Now;

                                                    Logs lg = new Logs();
                                                    ActivityLog alg = new ActivityLog();
                                                    Users us = new Users();
                                                    if (us.GetUserByEmail(tbEmail.Text) != null)
                                                    {
                                                        string name = us.GetUserByEmail(tbEmail.Text).name;
                                                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                                        alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                    }
                                                    else
                                                    {
                                                        lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                                    }
                                                }
                                                else if (ulUser.userLock == 1)
                                                {
                                                    lblError.Visible = true;
                                                    lblError.Text = "You have exceeeded the amount of tries. Please reset your password or contact an admin to unlock your account.";
                                                    DateTime dt = DateTime.Now;
                                                    ul.UpdateStatus(tryingUser.email, dt, 2);
                                                }
                                                else
                                                {
                                                    EmailLog elg = new EmailLog();
                                                    DateTime dtelg = DateTime.Now;
                                                    title = "Repeated Failed Logins";
                                                    EmailRepeatedLogin(userTrying.userEmail, tryingUser.name);
                                                    elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                    DateTime dt = DateTime.Now.AddMinutes(5);
                                                    ul.UpdateStatus(tryingUser.email, dt, 1);
                                                }

                                            }
                                        }
                                        else if (IsReCaptchaValid() == false)
                                        {
                                            lblError.Text = "Failed Captcha please try again";
                                        }

                                    else
                                    {
                                        lblError.Visible = true;
                                        string ipAddr = GetIPAddress();
                                        string countryLogged = CityStateCountByIp(ipAddr);
                                        CityStateCountByIp(ipAddr);
                                        DateTime dtLog = DateTime.Now;

                                        Logs lg = new Logs();
                                        Users us = new Users();
                                        ActivityLog alg = new ActivityLog();
                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                        {
                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                            alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                        }
                                        else
                                        {
                                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                        }

                                    }
                                    }
                                }
                                else
                                {
                                    if (userTrying.userOTP == otpSent)
                                    {
                                        if (IsReCaptchaValid())
                                        {
                                            blList = bl.getAllBlockedUsers();

                                            foreach (var blckedAcc in blList)
                                            {
                                                if (blckedAcc.email == tbEmail.Text)
                                                {
                                                    blockedAcc = true;
                                                }
                                            }
                                            if (!blockedAcc)
                                            {
                                                int OTPChecked = 0;
                                                Session["user"] = tryingUser;
                                                Session["id"] = tryingUser.id;
                                                Session["Name"] = tryingUser.name;
                                                Session["Pic"] = tryingUser.DPimage;
                                                Session["email"] = tryingUser.email;

                                                string ipAddr = GetIPAddress();
                                                string countryLogged = CityStateCountByIp(ipAddr);
                                                CityStateCountByIp(ipAddr);

                                                DateTime dtLog = DateTime.Now;
                                                Logs lg = new Logs();
                                                //Email function for new sign in
                                                lgList = lg.GetAllLogsOfUser(userTrying.userEmail);
                                                foreach (var log in lgList)
                                                {
                                                    if (log.ipAddr == ipAddr)
                                                    {
                                                        counter++;
                                                    }
                                                }

                                                ActivityLog alg = new ActivityLog();
                                                Users us = new Users();
                                                Sessionmg ses = new Sessionmg();
                                                if (ses.GetSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true)) != null)
                                                {
                                                    // update
                                                    ses.UpdateSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                                }
                                                else
                                                {
                                                    ses.InsertSession(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), 1);
                                                }
                                                if (us.GetUserByEmail(tbEmail.Text) != null)
                                                {
                                                    string name = us.GetUserByEmail(tbEmail.Text).name;
                                                    lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                                    alg.AddActivityLog(dtLog, name, ipAddr, "Successful Login Attempt", "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                }
                                                else
                                                {
                                                    lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                                }

                                                otp.UpdateOTPByEmail(userTrying.userEmail, OTPassword, OTPChecked);

                                                if (counter == 0)
                                                {
                                                    EmailLog elg = new EmailLog();
                                                    DateTime dtelg = DateTime.Now;
                                                    title = "Is this you? Login Clearview";
                                                    EmailFxNew(userTrying.userEmail, tryingUser.name, ipAddr, countryLogged);
                                                    elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                }
                                                BLL.Cookie ck = new BLL.Cookie();
                                                BLL.Cookie ck2 = ck.GetCookiesFromEmail(tbEmail.Text);
                                                HttpCookie cookieCH = Request.Cookies["SessionIDCH"];
                                                HttpCookie cookieIE = Request.Cookies["SessionIDIE"];
                                                if (browser.Contains("CHROME"))
                                                {
                                                    if (ck2.userCookieCH != null && cookieCH != null)
                                                    {
                                                        if (ck2.userCookieCH != cookieCH.ToString())
                                                        {
                                                            // edit here
                                                            if (us.GetUserByEmail(tbEmail.Text) != null)
                                                            {
                                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                                lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                                alg.AddActivityLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                            }
                                                            else
                                                            {
                                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                                            }

                                                            EmailLog elg = new EmailLog();
                                                            DateTime dtelg = DateTime.Now;
                                                            title = "New login from new browser";
                                                            EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                            //Creates new cookie session
                                                            Guid guid = Guid.NewGuid();
                                                            string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                            HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                            cookie2["sid"] = uSid;
                                                            cookie2.Expires = DateTime.Now.AddYears(1);
                                                            Response.Cookies.Add(cookie2);
                                                            CHCookie = cookie2.ToString();
                                                            ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        EmailLog elg = new EmailLog();
                                                        DateTime dtelg = DateTime.Now;
                                                        title = "New login from new browser";
                                                        EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                        elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                        //Creates new cookie session
                                                        Guid guid = Guid.NewGuid();
                                                        string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                        HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                        cookie2["sid"] = uSid;
                                                        cookie2.Expires = DateTime.Now.AddYears(1);
                                                        Response.Cookies.Add(cookie2);
                                                        CHCookie = cookie2.ToString();
                                                        ck.UpdateCookiesCH(userTrying.userEmail, CHCookie);
                                                    }
                                                }
                                                else if (browser.Contains("INTERNETEXPLORER"))
                                                {
                                                    if (ck2.userCookieIE != null && cookieIE != null)
                                                    {
                                                        if (ck2.userCookieIE != cookieIE.ToString())
                                                        {
                                                            // edit here
                                                            if (us.GetUserByEmail(tbEmail.Text) != null)
                                                            {
                                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                                lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);
                                                                alg.AddActivityLog(dtLog, name, ipAddr, "New Browser Detected: " + browser, "-", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                                            }
                                                            else
                                                            {
                                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "New Browser Detected: " + browser);

                                                            }
                                                            EmailLog elg = new EmailLog();
                                                            DateTime dtelg = DateTime.Now;
                                                            title = "New login from new browser";
                                                            EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                            //Creates new cookie session
                                                            Guid guid = Guid.NewGuid();
                                                            string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                            HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                            cookie2["sid"] = uSid;
                                                            cookie2.Expires = DateTime.Now.AddYears(1);
                                                            Response.Cookies.Add(cookie2);
                                                            IECookie = cookie2.ToString();
                                                            ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        EmailLog elg = new EmailLog();
                                                        DateTime dtelg = DateTime.Now;
                                                        title = "New login from new browser";
                                                        EmailNewDevice(userTrying.userEmail, tryingUser.name, browser);
                                                        elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                                        //Creates new cookie session
                                                        Guid guid = Guid.NewGuid();
                                                        string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                                        HttpCookie cookie2 = new HttpCookie("SessionIDCH");
                                                        cookie2["sid"] = uSid;
                                                        cookie2.Expires = DateTime.Now.AddYears(1);
                                                        Response.Cookies.Add(cookie2);
                                                        IECookie = cookie2.ToString();
                                                        ck.UpdateCookiesIE(userTrying.userEmail, IECookie);
                                                    }
                                                }
                                                UserLock ul = new UserLock();
                                                UserLock ulUser = ul.GetLockStatusByEmail(userTrying.userEmail);
                                                DateTime dt = DateTime.Now;
                                                ulUser.UpdateStatus(tryingUser.email, dt, 0);
                                                ulUser.UpdateAttempts(tryingUser.email, 0);

                                                Response.Redirect("homepage.aspx");
                                            }
                                            else
                                            {
                                                lblError.Visible = true;
                                                lblError.Text = "Your Account has been banned.";

                                            }
                                        }
                                        else if (IsReCaptchaValid() == false)
                                        {
                                            lblError.Text = "Failed Captcha please try again";
                                        }


                                    else
                                    {
                                        UserLock ul = new UserLock();
                                        UserLock ulUser = ul.GetLockStatusByEmail(userTrying.userEmail);
                                        counter2 = ulUser.userAttempts;
                                        if (counter2 < 5)
                                        {
                                            counter2 += 1;
                                            ul.UpdateAttempts(tryingUser.email, counter2);
                                            remianing = maximum - counter2;
                                            lblAttempts.Visible = true;
                                            lblAttempts.Text = "Attempts remaining: " + remianing;
                                            lblError.Visible = true;
                                            lblError.Text = "Incorrect account information. Please try again";
                                            string ipAddr = GetIPAddress();
                                            string countryLogged = CityStateCountByIp(ipAddr);
                                            CityStateCountByIp(ipAddr);
                                            DateTime dtLog = DateTime.Now;

                                            Logs lg = new Logs();
                                            ActivityLog alg = new ActivityLog();
                                            Users us = new Users();
                                            if (us.GetUserByEmail(tbEmail.Text) != null)
                                            {
                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                                alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                            }
                                            else
                                            {
                                                lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                            }
                                        }
                                        else if (ulUser.userLock == 1)
                                        {
                                            lblError.Visible = true;
                                            lblError.Text = "You have exceeeded the amount of tries. Please reset your password or contact an admin to unlock your account.";
                                            DateTime dt = DateTime.Now;
                                            ul.UpdateStatus(tryingUser.email, dt, 2);
                                        }
                                        else
                                        {
                                            EmailLog elg = new EmailLog();
                                            DateTime dtelg = DateTime.Now;
                                            title = "Repeated Failed Logins";
                                            EmailRepeatedLogin(userTrying.userEmail, tryingUser.name);
                                            elg.AddEmailLog(userTrying.userEmail, senderEmail, dtelg, title);
                                            DateTime dt = DateTime.Now.AddMinutes(5);
                                            ul.UpdateStatus(tryingUser.email, dt, 1);
                                        }
                                        }
                                    }

                                    else
                                    {
                                        lblError.Visible = true;
                                        string ipAddr = GetIPAddress();
                                        string countryLogged = CityStateCountByIp(ipAddr);
                                        CityStateCountByIp(ipAddr);
                                        DateTime dtLog = DateTime.Now;

                                        Logs lg = new Logs();
                                        Users us = new Users();
                                        ActivityLog alg = new ActivityLog();
                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                        {
                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                            alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", AntiXssEncoder.HtmlEncode(tbEmail.Text, true), countryLogged);
                                        }
                                        else
                                        {
                                            lg.AddLog(AntiXssEncoder.HtmlEncode(tbEmail.Text, true), dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                        }

                                    }
                                }

                            }
                            else
                            {
                                lblError.Visible = true;
                            }

                        }
                    }
                        
                    else
                    {
                        lblError.Visible = true;
                    }                    
                }
            }
        }


        public static string CityStateCountByIp(string IP)
        {
            //var url = "http://freegeoip.net/json/" + IP;
            //var url = "http://freegeoip.net/json/" + IP;
            string url = "http://api.ipstack.com/" + IP + "?access_key=01ca9062c54c48ef1b7d695b008cae00";
            var request = System.Net.WebRequest.Create(url);
            WebResponse myWebResponse = request.GetResponse();
            Stream stream = myWebResponse.GetResponseStream();

            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                JObject objJson = JObject.Parse(json);
                string Country = objJson["country_name"].ToString();
                string Country_code = objJson["country_code"].ToString();
                if (Country == "")
                {
                    Country = "-";
                }

                else if (Country_code == "")
                {
                    Country = Country;
                }
                else
                {
                    Country = Country + " (" + Country_code + ")";
                }
                return Country;

            }

        }



        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public string ComputeSha256Hash(string data)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }



        protected void btnOTP_Click(object sender, EventArgs e)
        {

        }
        public bool IsReCaptchaValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6LdJqj4aAAAAAAYEvpiOboFJ8xUV1XHYNpaIdfJz";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }


        protected void submit_Click(object sender, EventArgs e)
        {

            Random rnd = new Random();
            Users user = new Users();
            EmailLog elg = new EmailLog();
            HistoryOTP otp = new HistoryOTP();
            Users findUser = user.GetUserByEmail(userEmail.Text);
            bool findEmail = otp.GetUserByEmail(userEmail.Text);

            if (findUser != null)
            { // user exists
                OTPassword = rnd.Next(100000, 999999).ToString();
                OTPEmail = userEmail.Text;
                userName = findUser.name;
                int OTPCheck = 1;
                if (findEmail)
                {
                    title = "OTP Login Clearview";
                    DateTime dtelg = DateTime.Now;
                    result = "true";
                    otp.UpdateOTPByEmail(OTPEmail, OTPassword, OTPCheck);
                    lblOTP.Visible = true;
                    EmailFxOTP(OTPEmail, OTPassword, userName);
                    elg.AddEmailLog(OTPEmail, senderEmail, dtelg, title);
                }
                else
                {
                    title = "OTP Login Clearview";
                    DateTime dtelg = DateTime.Now;
                    result = "true";
                    otp.AddHistoryOTP(OTPEmail, OTPassword, OTPCheck);
                    lblOTP.Visible = true;
                    EmailFxOTP(OTPEmail, OTPassword, userName);
                    elg.AddEmailLog(OTPEmail, senderEmail, dtelg, title);
                }
            }
            else
            {
                lblError.Visible = true;
                result = "false";
            }
        }



        public static async Task EmailFxOTP(string email, string otp, string name)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("kovitwk21@gmail.com", "ClearView21");
            var subject = "OTP Login ClearView";
            var to = new EmailAddress(email, name);
            var plainTextContent = "";
            var htmlContent = "<strong><h2>Use this One-Time Password as your Account Password to Login.</h2></strong><h3>Your OTP is: " + otp + "</h3>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public static async Task EmailFxNew(string email, string name, string ip, string country)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("kovitwk21@gmail.com", "ClearView21");
            var subject = "Is this you? Login ClearView";
            var to = new EmailAddress(email, name);
            var plainTextContent = "";
            var htmlContent = "<strong><h2>We detected a new sign-in to your account</h2></strong><br/><p><strong>New sign-in detected from this IP-Address: </strong> " + ip + "</p><p> <strong>Country:</strong> " + country + "</p><strong><p>If this was you, please ignore this email, otherwise click here to change your password " + "http://localhost:60329/EditProfile.aspx" + "</p></strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        public static async Task EmailNewDevice(string email, string name, string browser)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("kovitwk21@gmail.com", "ClearView21");
            var subject = "New login from new browser" ;
            var to = new EmailAddress(email, name);
            var plainTextContent = "";
            var htmlContent = "<strong><h2>We detected a new sign-in to your account from </h2></strong>"+ browser+ "<br/>" +
                "<p>If this was you, please ignore this email, otherwise click here to change your password " + "http://localhost:60329/EditProfile.aspx" + "</p></strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        public static async Task EmailRepeatedLogin(string email, string name)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("kovitwk21@gmail.com", "ClearView21");
            var subject = "Repeated Failed Logins";
            var to = new EmailAddress(email, name);
            var plainTextContent = "";
            var htmlContent = "<strong><h2>We detected a a series of failed logins</h2></strong><br/>" +
                "<p>If this was you, please ignore this email, otherwise click here to change your password " + "http://localhost:60329/EditProfile.aspx" + "</p></strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }
}