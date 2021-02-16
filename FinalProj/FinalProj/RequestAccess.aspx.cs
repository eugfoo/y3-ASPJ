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
    public partial class RequestAccess : System.Web.UI.Page
    {
        Admins ad = new Admins();
        RequestPermission req = new RequestPermission();
        protected Sessionmg sesDeets;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<RequestPermission> listReq = new List<RequestPermission>();
            if (!IsPostBack)
            {
                if (Session["subadmin"] == null && Session["admin"] == null)
                {
                    Response.Redirect("/homepage.aspx");
                }
                else
                {
                    if (Session["admin"] != null)
                    {
                        subAdminDiv.Visible = false;
                        listReq = req.getAllRequests();
                        gvAdmin.DataSource = listReq;
                        gvAdmin.DataBind();
                    }
                    else
                    {
                        Sessionmg ses = new Sessionmg();

                        sesDeets = ses.GetSession(Session["subadminEmail"].ToString());
                        if (sesDeets.Active == 1)
                        {
                            adminDiv.Visible = false;
                            ad = ad.GetAllAdminWithEmail(Session["subadminEmail"].ToString());
                            lblCurrentRole.Text = ad.adminRole;
                            btnRequest.Enabled = false;

                            RequestPermission requesting = new RequestPermission();
                            requesting = req.GetLastRequest(ad.adminEmail);
                            if (requesting != null)
                            {
                                if(requesting.requestStatus == 0)
                                {
                                    lblSuccess.Visible = true;
                                    lblSuccess.Text = "Your request has been sent for review";
                                    roleDDL.Enabled = false;
                                }
                                else if (requesting.requestStatus == 1)
                                {
                                    lblSuccess.Visible = false;
                                    lblError.Visible = true;
                                    lblError.Text = "Your request has been declined";
                                    roleDDL.Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            Session.Clear();
                            string err = "SessionKicked";
                            Response.Redirect("homepage.aspx?error=" + err);
                        }
                    }
                }
            }
        }

        protected void roleDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (roleDDL.SelectedItem.Text == lblCurrentRole.Text)
            {
                lblError.Visible = true;
                lblError.Text = "This is already your role!";
                btnRequest.Enabled = false;
            }
            else if (roleDDL.SelectedItem.Text == "Select a Role")
            {
                btnRequest.Enabled = false;
                lblError.Visible = false;
            }
            else
            {
                btnRequest.Enabled = true;
                lblError.Visible = false;
            }
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            if (roleDDL.SelectedItem.Text == lblCurrentRole.Text)
            {
                lblError.Visible = true;
                lblError.Text = "This is already your role!";
                btnRequest.Enabled = false;
            }
            else
            {
                ad = ad.GetAllAdminWithEmail(Session["subadminEmail"].ToString());
                adminLog adl = new adminLog();
                MainAdmins mad = new MainAdmins();
                Users us = new Users();

                string ipAddr = GetIPAddress();
                string countryLogged = CityStateCountByIp(ipAddr);
                roles rl = new roles();
                DateTime dt = DateTime.Now;
                RequestPermission request = new RequestPermission();
                request.AddRequest(ad.adminEmail, roleDDL.SelectedItem.Text, ad.adminRole);
                adl.AddAdminLog(dt, ad.adminName, ipAddr, "Requested for " + roleDDL.SelectedItem.Text + " permission", "-", ad.adminEmail, countryLogged);

                Response.Redirect("/RequestAccess.aspx");
            }

        }

        protected void btnVerify_Click(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvAdmin.Rows[index];

            string email = row.Cells[0].Text;
            string role = row.Cells[1].Text;
            adminLog adl = new adminLog();

            string ipAddr = GetIPAddress();
            string countryLogged = CityStateCountByIp(ipAddr);
            DateTime dt = DateTime.Now;
            adl.AddAdminLog(dt, "Admin", ipAddr, "Accepted request for " + role + " permission made by " + email, "-", Session["adminEmail"].ToString(), countryLogged); ;
            
            if (e.CommandName == "Verify")
            {
                ad.UpdateRoleByEmail(email, role);
                req.DeleteByIdEmail(email);
                Response.Redirect("/RequestAccess.aspx");
            }
            else if (e.CommandName == "Decline")
            {
                req.UpdateRequestByEmail(email);
                Response.Redirect("/RequestAccess.aspx");
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
        // Get IP Address of user
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