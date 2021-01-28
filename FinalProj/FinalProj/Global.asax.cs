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

        protected void Application_Start(object sender, EventArgs e)
        {
        }
        //protected void Application_AuthenticateRequest(object sender, EventArgs e) { 
        //    context = System.Web.HttpContext.Current;
        //}
        protected void Session_End(object sender, EventArgs e)
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
                alg.AddActivityLog(dtLog, user.name, ipAddr, "Session Timeout", "-", user.email, countryLogged);
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