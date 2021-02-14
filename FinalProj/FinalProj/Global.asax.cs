using FinalProj.BLL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace FinalProj
{
    public class Global : System.Web.HttpApplication
    {
        List<Logs> lgList;
        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex is HttpRequestValidationException)
            {
                Users user = (Users)Session["user"];
                Logs lg = new Logs();
                if (user.GetUserByEmail(user.email) != null)
                {
                    lgList = lg.GetAllLogsOfUser(user.email);
                    string ipAddr = lgList[0].ipAddr;
                    string countryLogged = CityStateCountByIp(ipAddr);
                    DateTime dtLog = DateTime.Now;

                    CityStateCountByIp(ipAddr);
                    ActivityLog alg = new ActivityLog();

                    alg.AddActivityLog(dtLog, user.name, ipAddr, "Suspicious HTML Entry", "XSS", user.email, countryLogged);
                    Response.Clear();
                    Response.StatusCode = 200;
                    Response.Write(@"
                    <html><head><title>HTML Not Allowed</title>
                    <script language='JavaScript'><!--
                    function back() { history.go(-1); } //--></script></head>
                    <body style='font-family: Arial, Sans-serif;'>
                    <h1>Oops!</h1>
                    <p>I'm sorry, but HTML entry is not allowed on that page.</p>
                    <p>Please make sure that your entries do not contain 
                    any angle brackets like &lt; or &gt;.</p>
                    <p><a href='javascript:back()'>Go back</a></p>
                    </body></html>
                    ");
                    Response.End();
                }
                else
                {
                }
            }
            
        }
        protected void Session_End(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Users user = (Users)Session["user"];
                Logs lg = new Logs();
                if (user.GetUserByEmail(user.email) != null)
                {
                    lgList = lg.GetAllLogsOfUser(user.email);
                    string ipAddr = lgList[0].ipAddr;
                    string countryLogged = CityStateCountByIp(ipAddr);
                    DateTime dtLog = DateTime.Now;
                    Sessionmg ses = new Sessionmg();
                    CityStateCountByIp(ipAddr);
                    ActivityLog alg = new ActivityLog();
                    ses.UpdateSession(user.email, 0);
                    lg.AddLog(user.email, dtLog, ipAddr, countryLogged, "Session Timeout");
                    alg.AddActivityLog(dtLog, user.name, ipAddr, "Session Timeout", "-", user.email, countryLogged);
                    collabOTP cbOTP = new collabOTP();
                    collabOTP cbDetails = cbOTP.GetUserByEmailOTP(user.email);
                    if (cbDetails != null)
                    {
                        cbOTP.UpdateOTPByEmail(cbDetails.userEmail, "", 0); ;
                    }
                    Session.Clear();
                }
                else
                {
                    Session.Clear();
                }
            }
            else if (Session["subadmin"] != null)
            {
                Logs lg = new Logs();
                lgList = lg.GetAllLogsOfUser(Session["subadminEmail"].ToString());
                Sessionmg ses = new Sessionmg();
                string ipAddr = lgList[0].ipAddr;
                DateTime dtLog = DateTime.Now;
                string countryLogged = CityStateCountByIp(ipAddr);
                ses.UpdateSession(Session["subadminEmail"].ToString(), 0);
                lg.AddLog(Session["subadminEmail"].ToString(), dtLog, ipAddr, countryLogged, "Session Timeout");
            }
            else {
                Logs lg = new Logs();
                Sessionmg ses = new Sessionmg();
                lgList = lg.GetAllLogsOfUser(Session["adminEmail"].ToString());
                string ipAddr = lgList[0].ipAddr;
                DateTime dtLog = DateTime.Now;
                string countryLogged = CityStateCountByIp(ipAddr);
                ses.UpdateSession(Session["adminEmail"].ToString(), 0);
                lg.AddLog(Session["adminEmail"].ToString(), dtLog, ipAddr, countryLogged, "Session Timeout");
            }

        }

        //protected string GetIPAddress()
        //{
        //    //System.Web.HttpContext context = System.Web.HttpContext.Current;
        //    string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //    if (!string.IsNullOrEmpty(ipAddress))
        //    {
        //        string[] addresses = ipAddress.Split(',');
        //        if (addresses.Length != 0)
        //        {
        //            return addresses[0];
        //        }
        //    }
        //    return context.Request.ServerVariables["REMOTE_ADDR"];
        //}

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

    }
}