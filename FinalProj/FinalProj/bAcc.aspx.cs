using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using Newtonsoft.Json.Linq;

namespace FinalProj
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        protected roles cap;
        protected int capPerm = 0;
        protected Sessionmg sesDeets;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["admin"]) == true || Convert.ToBoolean(Session["subadmin"]) == true)
            {
                Sessionmg ses = new Sessionmg();
                if (Convert.ToBoolean(Session["subadmin"]))
                {
                    sesDeets = ses.GetSession(Session["subadminEmail"].ToString());
                }
                else
                {
                    sesDeets = ses.GetSession(Session["adminEmail"].ToString());

                }
                if (sesDeets.Active == 1)
                {
                    Admins ad = new Admins();
                    roles rl = new roles();

                    if (!Convert.ToBoolean(Session["admin"]))
                    {
                        string adEmail = Session["subadminEmail"].ToString();
                        Admins adDetails = ad.GetAllAdminWithEmail(adEmail);
                        cap = rl.GetRole(adDetails.adminRole);
                        capPerm = cap.mgBan;
                    }

                    if (capPerm == 1 || Convert.ToBoolean(Session["admin"]) == true)
                    {

                    }
                    else
                    {
                        if (Convert.ToBoolean(Session["subadmin"]))
                        {
                            string err = "NoPermission";
                            Response.Redirect("homepage.aspx?error=" + err);
                        }
                        else
                        {
                            Response.Redirect("homepage.aspx");
                        }
                    }
                }
                else {
                    Session.Clear();
                    string err = "SessionKicked";
                    Response.Redirect("homepage.aspx?error=" + err);
                }
            }
            else
            {
                Response.Redirect("homepage.aspx");
            }
        }

        protected void blockedGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            blocked bl = new blocked();
            List<blocked> blList;
            Users us = new Users();
            adminLog adl = new adminLog();
            DateTime dt = DateTime.Now;
            MainAdmins mad = new MainAdmins();
            string ipAddr = GetIPAddress();
            string countryLogged = CityStateCountByIp(ipAddr);
            blList = bl.getAllBlockedUsers();

            if (Session["admin"] != null)
            {
                adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, blList[e.RowIndex].email + " (" + blList[e.RowIndex].name + ") was unbanned", "-", Session["adminEmail"].ToString(), countryLogged);
            }
            else
            {
                adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, blList[e.RowIndex].email + " (" + blList[e.RowIndex].name + ") was unbanned", "-", Session["subadminEmail"].ToString(), countryLogged);

            }
            bl.unblock(blList[e.RowIndex].email);
            Response.Redirect("bAcc.aspx");
        }

        protected void banAccBtn_Click(object sender, EventArgs e)
        {
            blocked bl = new blocked();
            Users us = new Users();
            Users usDeets;
            string bantb = AntiXssEncoder.HtmlEncode(banEmail.Text, true);
            string banreasontb = AntiXssEncoder.HtmlEncode(banreasonTB.Text, true);
            usDeets = us.GetUserByEmail(bantb);
            DateTime dtn = DateTime.Now;
            if (bantb != "")
            {
                if (banreasontb != "") { 
                    if (usDeets != null)
                    {
                        if (Session["subadminEmail"] != null) {
                            if (bantb == Session["subadminEmail"].ToString())
                            {
                                PanelError.Visible = true;
                                errmsgTb.Text = "You can't ban yourself!";
                            }
                            else
                            {
                                Admins ad = new Admins();
                                if (ad.GetAllAdminWithEmail(bantb) != null)
                                {
                                    PanelError.Visible = true;
                                    errmsgTb.Text = "You can't ban an admin or sub-admin!";
                                }
                                else
                                {
                                    adminLog adl = new adminLog();
                                    DateTime dt = DateTime.Now;
                                    MainAdmins mad = new MainAdmins();
                                    string ipAddr = GetIPAddress();
                                    string countryLogged = CityStateCountByIp(ipAddr);

                                    if (Session["admin"] != null)
                                    {
                                        adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, bantb + " (" + usDeets.name + ") was banned", "-", Session["adminEmail"].ToString(), countryLogged);
                                    }
                                    else
                                    {
                                        adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, bantb + " (" + usDeets.name + ") was banned", "-", Session["subadminEmail"].ToString(), countryLogged);

                                    }
                                    bl.AddBlockedAcc(bantb, usDeets.name, banreasontb, dtn);
                                    Sessionmg ses = new Sessionmg();
                                    ses.UpdateSession(bantb, 0);
                                    Response.Redirect("/bAcc.aspx");
                                }
                            }
                        } else if (Session["adminEmail"] != null) { 
                            if (bantb == Session["adminEmail"].ToString()) {
                                PanelError.Visible = true;
                                errmsgTb.Text = "You can't ban yourself!";
                            }
                            else
                            {
                                Admins ad = new Admins();
                                adminLog adl = new adminLog();
                                DateTime dt = DateTime.Now;
                                MainAdmins mad = new MainAdmins();
                                string ipAddr = GetIPAddress();
                                string countryLogged = CityStateCountByIp(ipAddr);

                                if (Session["admin"] != null)
                                {
                                    adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, bantb + " (" + usDeets.name + ") was banned", "-", Session["adminEmail"].ToString(), countryLogged);
                                }
                                else
                                {
                                    adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, bantb + " (" + usDeets.name + ") was banned", "-", Session["subadminEmail"].ToString(), countryLogged);

                                }
                                if (ad.GetAllAdminWithEmail(bantb) != null)
                                {
                                    PanelError.Visible = true;
                                    errmsgTb.Text = "You can't ban an admin or sub-admin!";
                                }
                                else
                                {
                                    bl.AddBlockedAcc(bantb, usDeets.name, banreasontb, dtn);
                                    Sessionmg ses = new Sessionmg();
                                    ses.UpdateSession(bantb, 0);
                                    Response.Redirect("/bAcc.aspx");
                                }
                            }
                        }
                    }
                    else
                    {
                        // show error msg
                        PanelError.Visible = true;
                        errmsgTb.Text = "User doesn't exist!";
                    }
                }
                else{
                    PanelError.Visible = true;
                    errmsgTb.Text = "Please enter a reason for the ban!";
                }
            }
            else {
                PanelError.Visible = true;
                errmsgTb.Text = "Please enter an Email!";
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
    }
}