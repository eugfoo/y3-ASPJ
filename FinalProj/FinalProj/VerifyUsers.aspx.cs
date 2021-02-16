using System;
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
    public partial class VerifyUsers : System.Web.UI.Page
    {
        protected roles cap;
        protected int capPerm = 0;
        protected Sessionmg sesDeets;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["admin"] == null && Session["subadmin"] == null) // A user has signed in
                {
                    Response.Redirect("/homepage.aspx");
                }
                else
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
                        if (!Convert.ToBoolean(Session["admin"]))
                        {
                            Admins ad = new Admins();
                            roles rl = new roles();
                            string adEmail = Session["subadminEmail"].ToString();
                            Admins adDetails = ad.GetAllAdminWithEmail(adEmail);
                            cap = rl.GetRole(adDetails.adminRole);
                            capPerm = cap.mgCollab;
                        }

                        if (capPerm == 1 || Convert.ToBoolean(Session["admin"]) == true)
                        {
                            Users user = new Users();
                            List<Users> userList = user.getUnverifiedUsers();
                            List<ListItem> Imgs = new List<ListItem>();
                            string path = Path.Combine(Server.MapPath("/Img/User/UserFaceVerification/"));
                            string[] ImagePaths = Directory.GetFiles(path);
                            int j = 0;
                            foreach (var i in userList)
                            {
                                foreach (string imgPath in ImagePaths)
                                {
                                    if (imgPath.Contains(i.email))
                                    {
                                        string ImgName = Path.GetFileName(imgPath);
                                        Imgs.Add(new ListItem(i.email, "~/Img/User/UserFaceVerification/" + ImgName));
                                        j++;
                                    }
                                }
                            }
                            if (Imgs.Count == 0)
                            {
                                lblNoUsers.Visible = true;
                            }
                            else
                            {
                                Gv_imgs.DataSource = Imgs;
                                Gv_imgs.DataBind();
                            }
                        }
                    }
                    else // If a non-admin tries to access the page...
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
            }
        }

        protected void btnVerify_Click(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            MainAdmins mad = new MainAdmins();
            GridViewRow row = Gv_imgs.Rows[index];
            adminLog adl = new adminLog();
            DateTime dt = DateTime.Now;
            Users us = new Users();
            string ipAddr = GetIPAddress();
            string countryLogged = CityStateCountByIp(ipAddr);
            string email = row.Cells[0].Text;
            Users user = new Users();
            user = user.GetUserByEmail(email);
            if (e.CommandName == "Verify")
            {
                user.VerifyOrgByEmail(email);

                if (Session["admin"] != null)
                {
                    adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, "Verified " + email + " (" + us.GetUserByEmail(email).name + ")'s selfie", "-", Session["adminEmail"].ToString(), countryLogged);
                }
                else
                {
                    adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Verified " + email + " (" + us.GetUserByEmail(email).name + ")'s selfie", "-", Session["subadminEmail"].ToString(), countryLogged);

                }
                Response.Redirect("/VerifyUsers.aspx");
            }
            else if (e.CommandName == "Decline")
            {
                user.UpdateVerifyImage(user.id, "");

                string path = Path.Combine(Server.MapPath("/Img/User/UserFaceVerification/"));
                string fileNameWitPath = path + user.email.ToString() + ".png";
                FileInfo file = new FileInfo(fileNameWitPath);
                if (file.Exists)
                {
                    file.Delete();
                    if (Session["admin"] != null)
                    {
                        adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, "Declined " + email + " (" + us.GetUserByEmail(email).name + ")'s selfie", "-", Session["adminEmail"].ToString(), countryLogged);
                    }
                    else
                    {
                        adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Declined " + email + " (" + us.GetUserByEmail(email).name + ")'s selfie", "-", Session["subadminEmail"].ToString(), countryLogged);

                    }
                    Response.Redirect("/VerifyUsers.aspx");
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
    }
}