using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using Newtonsoft.Json.Linq;

namespace FinalProj
{
    public partial class SiteBootstrap : System.Web.UI.MasterPage
    {
        protected List<Notifications> notiListTemp;
        protected List<Admins> adListTemp;
        protected List<RequestPermission> reqListTemp;
        protected Admins adDeets;
        protected roles rlDeets;
        protected List<Notifications> notiList = new List<Notifications>();
        protected List<Admins> adList = new List<Admins>();
        protected List<RequestPermission> reqList = new List<RequestPermission>();
        List<MainAdmins> maList;
        protected int count = 0;
        protected int adcounter = 0;
        protected int reqCounter = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Notifications noti = new Notifications();

            if (Convert.ToBoolean(Session["subadmin"])) // An admin has signed in
            {
                Admins ad = new Admins();
                adDeets = ad.GetAllAdminWithEmail(Session["subadminEmail"].ToString());
                roles rl = new roles();
                rlDeets = rl.GetRole(adDeets.adminRole);
                Div2.Visible = false;
                lisubadmin.Visible = true;
            }
            else if (Convert.ToBoolean(Session["admin"]))
            {
                liAdmin.Visible = true;
                Div2.Visible = false;

                RequestPermission req = new RequestPermission();
                reqListTemp = req.getAllRequests();

                for (int j = 0; j < reqListTemp.Count; j++)
                {
                    reqCounter += 1;
                    reqList.Add(reqListTemp[j]);
                }

            }
            else
            {
                if (Session["user"] != null) // A user has signed in
                {
                    Users user = (Users)Session["user"];
                    if (user.isOrg.Trim() == "True")
                    {
                        //norgItems.Visible = false;
                    }
                    lblProfile.Text = user.name;
                    liLogOut.Visible = true;
                    lblBookmark.Visible = true;

                    notiListTemp = noti.GetEventsEnded();
                    Admins ad = new Admins();
                    adListTemp = ad.GetAllAdmins();

                    System.Diagnostics.Debug.WriteLine("This is notiListTemp: " + notiListTemp);
                    for (int j = 0; j < adListTemp.Count; j++)
                    {
                        if (adListTemp[j].adminEmail == user.email.ToString() && adListTemp[j].adminStatus == "Pending")
                        {
                            adcounter += 1;
                            adList.Add(adListTemp[j]);
                            //System.Diagnostics.Debug.WriteLine("This is notiList" + notiList[i]);
                        }
                    }



                    for (int i = 0; i < notiListTemp.Count; i++)
                    {
                        if (notiListTemp[i].User_id == user.id)
                        {
                            count += 1;
                            notiList.Add(notiListTemp[i]);
                            //System.Diagnostics.Debug.WriteLine("This is notiList" + notiList[i]);
                        }
                    }

                }
                else
                {
                    ddCaret.Visible = false;
                    ddMenu.Visible = false;
                    lblProfile.Text = "Sign In";
                    lblProfile.NavigateUrl = "/LogIn.aspx";
                    liLogOut.Visible = false;
                    lblBookmark.Visible = false;
                }
            }
        }

        protected void lblLogOut_Click(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(Session["admin"]))
            {
                if (!Convert.ToBoolean(Session["subadmin"]))
                {
                    Users user = (Users)Session["user"];
                    string ipAddr = GetIPAddress();
                    string countryLogged = CityStateCountByIp(ipAddr);
                    DateTime dtLog = DateTime.Now;
                    CityStateCountByIp(ipAddr);
                    ActivityLog alg = new ActivityLog();
                    collabOTP cbOTP = new collabOTP();
                    Sessionmg ses = new Sessionmg();
                    collabOTP cbDetails = cbOTP.GetUserByEmailOTP(user.email);
                    ses.UpdateSession(user.email, 0);
                    if (cbDetails != null)
                    {
                        cbOTP.UpdateOTPByEmail(cbDetails.userEmail, "", 0); ;
                    }

                    alg.AddActivityLog(dtLog, user.name, ipAddr, "Successful Log out Attempt ", "-", user.email, countryLogged);
                    Session.Clear();
                    Response.Redirect("/homepage.aspx");
                }
                else
                {
                    Users user = new Users();
                    Admins adUser = new Admins();
                    Sessionmg ses = new Sessionmg();
                    Admins ad = adUser.GetAllAdminWithEmail(Session["subadminEmail"].ToString());
                    ses.UpdateSession(Session["subadminEmail"].ToString(), 0);
                    string ipAddr = GetIPAddress();
                    string countryLogged = CityStateCountByIp(ipAddr);
                    DateTime dtLog = DateTime.Now;
                    CityStateCountByIp(ipAddr);
                    Logs lg = new Logs();
                    adminLog adl = new adminLog();
                    //ActivityLog alg = new ActivityLog();
                    adl.AddAdminLog(dtLog, user.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Successful Log out Attempt", " - ", Session["subadminEmail"].ToString(), countryLogged);
                    lg.AddLog(Session["subadminEmail"].ToString(), dtLog, ipAddr, countryLogged, "Successful Log out Attempt");
                    Session.Clear();
                    //alg.AddActivityLog(dtLog, ad.adminName, ipAddr, "Successful Log out Attempt ", "-", ad.adminEmail, countryLogged);
                    Response.Redirect("/homepage.aspx");
                }
            }
            else
            {
                Sessionmg ses = new Sessionmg();

                ses.UpdateSession(Session["adminEmail"].ToString(), 0);
                string ipAddr = GetIPAddress();
                string countryLogged = CityStateCountByIp(ipAddr);
                DateTime dtLog = DateTime.Now;
                CityStateCountByIp(ipAddr);
                Logs lg = new Logs();
                adminLog adl = new adminLog();
                MainAdmins ma = new MainAdmins();
                maList = ma.SelectAllMainAdmins();

                adl.AddAdminLog(dtLog, maList[0].MainadminName, ipAddr, "Successful Log out Attempt", "-", maList[0].MainadminEmail, countryLogged);

                lg.AddLog(maList[0].MainadminEmail, dtLog, ipAddr, countryLogged, "Successful Log out Attempt");
                Session.Clear();

                Response.Redirect("/homepage.aspx");
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

        protected void btnSend_Click(object sender, EventArgs e)
        {
            Users user = (Users)Session["user"];
            user.VerifyOrgByEmail(user.email);
        }
    }
}