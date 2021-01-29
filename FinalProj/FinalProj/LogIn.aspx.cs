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

namespace FinalProj
{
    public partial class LogIn : System.Web.UI.Page
    {
        public string result = "";
        public string OTPEmail = "";
        public string OTPassword = "";
        public string userName = "";        public string code = "";
        protected List<Logs> lgList;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                HistoryOTP otp = new HistoryOTP();
                HistoryOTP userTrying = otp.GetUserByEmailOTP(tbEmail.Text);

                MainAdmins mainadmin = new MainAdmins();
                MainAdmins adminlogin = mainadmin.GetAdminByEmail(tbEmail.Text);
                string adminPassHash = ComputeSha256Hash(tbPass.Text);

                if (adminlogin != null) //user exists
                {
                    if (adminlogin.MainAdminPassword == adminPassHash) // hashed version of adminPass41111
                    {
                        string ipAddr = GetIPAddress();
                        string countryLogged = CityStateCountByIp(ipAddr);
                        DateTime dtLog = DateTime.Now;
                        Logs lg = new Logs();
                        lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                        Session["admin"] = true;
                        Response.Redirect("homepage.aspx");
                    }
                    else
                    {
                        string ipAddr = GetIPAddress();
                        string countryLogged = CityStateCountByIp(ipAddr);
                        DateTime dtLog = DateTime.Now;
                        Logs lg = new Logs();
                        lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                    }
                }


                Users user = new Users();
                Users tryingUser = user.GetUserByEmail(tbEmail.Text);

                string passHash = tbPass.Text.ToString().Trim();
                if (userTrying.userOTPCheck == 0)
                {
                    if (tryingUser != null) // user exists
                    {
                        string dbHash = tryingUser.passHash;
                        string dbSalt = tryingUser.passSalt;
                        SHA256Managed hashing = new SHA256Managed();
                        if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                        {
                            string pwdWithSalt = passHash + dbSalt;
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                            string userHash = Convert.ToBase64String(hashWithSalt);
                            if (userHash.Equals(dbHash))
                            {
                                if (tryingUser.twofactor == 1)
                                {
                                    if (IsReCaptchaValid())
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
                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                        {
                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                            lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                            alg.AddActivityLog(dtLog, name, ipAddr, "Successful Login Attempt", "-", tbEmail.Text, countryLogged);
                                        }
                                        else
                                        {
                                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                        }
                                        Response.Redirect("TwoFactor1.aspx");
                                    }
                                    else
                                    {
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
                                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                            alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", tbEmail.Text, countryLogged);
                                        }
                                        else
                                        {
                                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                        }
                                    }

                                }
                                else
                                {
                                    if (IsReCaptchaValid())
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
                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                        {
                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                            lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                            alg.AddActivityLog(dtLog, name, ipAddr, "Successful Login Attempt", "-", tbEmail.Text, countryLogged);
                                        }
                                        else
                                        {
                                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                        }
                                        Response.Redirect("Homepage.aspx");
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
                                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                            alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", tbEmail.Text, countryLogged);
                                        }
                                        else
                                        {
                                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
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
                                    lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                    alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", tbEmail.Text, countryLogged);
                                }
                                else
                                {
                                    lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
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
                                lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", tbEmail.Text, countryLogged);
                            }
                            else
                            {
                                lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
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
                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                            alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", tbEmail.Text, countryLogged);
                        }
                        else
                        {
                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                        }
                    }
                }
                else
                {
                    lblError.Visible = true;
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
                        if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                        {
                            string pwdWithSalt = passHash + dbSalt;
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                            string userHash = Convert.ToBase64String(hashWithSalt);
                            if (userHash.Equals(dbHash))
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
                                ActivityLog alg = new ActivityLog();
                                Users us = new Users();
                                if (us.GetUserByEmail(tbEmail.Text) != null)
                                {
                                    string name = us.GetUserByEmail(tbEmail.Text).name;
                                    lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                    alg.AddActivityLog(dtLog, name, ipAddr, "Successful Login Attempt", "-", tbEmail.Text, countryLogged);
                                }
                                else
                                {
                                    lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                }
                                Response.Redirect("Homepage.aspx");
                            }
                            else
                            {
                                if (userTrying.userOTP == otpSent)
                                {
                                    if (IsReCaptchaValid())
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
                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                        {
                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                            lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                            alg.AddActivityLog(dtLog, name, ipAddr, "Successful Login Attempt", "-", tbEmail.Text, countryLogged);
                                        }
                                        else
                                        {
                                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                        }

                                        otp.UpdateOTPByEmail(userTrying.userEmail, OTPassword, OTPChecked);

                                        if (counter == 0)
                                        {
                                            EmailFxNew(userTrying.userEmail, tryingUser.name, ipAddr, countryLogged);
                                        }
                                        HttpCookie cookie = Request.Cookies["SessionID"];
                                        if (cookie == null)
                                        {
                                            // edit here
                                            if (us.GetUserByEmail(tbEmail.Text) != null)
                                            {
                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Device Detected");
                                                alg.AddActivityLog(dtLog, name, ipAddr, "New Device Detected", "-", tbEmail.Text, countryLogged);
                                            }
                                            else
                                            {
                                                lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "New Device Detected");

                                            }
                                            EmailNewDevice(userTrying.userEmail, tryingUser.name);
                                            //Creates new cookie session
                                            Guid guid = Guid.NewGuid();
                                            string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                            HttpCookie cookie2 = new HttpCookie("SessionID");
                                            cookie2["sid"] = uSid;
                                            cookie2.Expires = DateTime.Now.AddYears(1);
                                            Response.Cookies.Add(cookie2);
                                        }

                                        Response.Redirect("homepage.aspx");
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
                                        ActivityLog alg = new ActivityLog();
                                        Users us = new Users();
                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                        {
                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                            alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", tbEmail.Text, countryLogged);
                                        }
                                        else
                                        {
                                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
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
                                        lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                        alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", tbEmail.Text, countryLogged);
                                    }
                                    else
                                    {
                                        lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
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
                                        if (us.GetUserByEmail(tbEmail.Text) != null)
                                        {
                                            string name = us.GetUserByEmail(tbEmail.Text).name;
                                            lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                            alg.AddActivityLog(dtLog, name, ipAddr, "Successful Login Attempt", "-", tbEmail.Text, countryLogged);
                                        }
                                        else
                                        {
                                            lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Successful Login Attempt");
                                        }

                                        otp.UpdateOTPByEmail(userTrying.userEmail, OTPassword, OTPChecked);

                                        if (counter == 0)
                                        {
                                            EmailFxNew(userTrying.userEmail, tryingUser.name, ipAddr, countryLogged);
                                        }
                                        HttpCookie cookie = Request.Cookies["SessionID"];
                                        if (cookie == null)
                                        {
                                            // edit here
                                            if (us.GetUserByEmail(tbEmail.Text) != null)
                                            {
                                                string name = us.GetUserByEmail(tbEmail.Text).name;
                                                lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged, "New Device Detected");
                                                alg.AddActivityLog(dtLog, name, ipAddr, "New Device Detected", "-", tbEmail.Text, countryLogged);
                                            }
                                            else
                                            {
                                                lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "New Device Detected");

                                            }
                                            EmailNewDevice(userTrying.userEmail, tryingUser.name);
                                            //Creates new cookie session
                                            Guid guid = Guid.NewGuid();
                                            string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                                            HttpCookie cookie2 = new HttpCookie("SessionID");
                                            cookie2["sid"] = uSid;
                                            cookie2.Expires = DateTime.Now.AddYears(1);
                                            Response.Cookies.Add(cookie2);
                                        }

                                        Response.Redirect("homepage.aspx");
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
                                    ActivityLog alg = new ActivityLog();
                                    Users us = new Users();
                                    if (us.GetUserByEmail(tbEmail.Text) != null)
                                    {
                                        string name = us.GetUserByEmail(tbEmail.Text).name;
                                        lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                        alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", tbEmail.Text, countryLogged);
                                    }
                                    else
                                    {
                                        lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
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
                                    lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                    alg.AddActivityLog(dtLog, name, ipAddr, "Failed Login Attempt", "Failed Authentication", tbEmail.Text, countryLogged);
                                }
                                else
                                {
                                    lg.AddLog(tbEmail.Text, dtLog, ipAddr, countryLogged, "Failed Login Attempt");
                                }

                            }
                        }


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
                    result = "true";
                    otp.UpdateOTPByEmail(OTPEmail, OTPassword, OTPCheck);
                    lblOTP.Visible = true;
                    EmailFxOTP(OTPEmail, OTPassword, userName);
                }
                else
                {
                    result = "true";
                    otp.AddHistoryOTP(OTPEmail, OTPassword, OTPCheck);
                    lblOTP.Visible = true;
                    EmailFxOTP(OTPEmail, OTPassword, userName);
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
        public static async Task EmailNewDevice(string email, string name)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("kovitwk21@gmail.com", "ClearView21");
            var subject = "New login from unknown device";
            var to = new EmailAddress(email, name);
            var plainTextContent = "";
            var htmlContent = "<strong><h2>We detected a new sign-in to your account from an unknown device</h2></strong><br/>" +
                "<p>If this was you, please ignore this email, otherwise click here to change your password " + "http://localhost:60329/EditProfile.aspx" + "</p></strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }
}