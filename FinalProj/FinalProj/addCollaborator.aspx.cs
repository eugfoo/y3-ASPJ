using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Web.Security.AntiXss;
using System.Net;
using Newtonsoft.Json.Linq;

namespace FinalProj
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string result ="";
        public string errmsg = "";

        protected Admins adDeets;
        protected roles rlDeets;
        protected List<Admins> adList;
        protected List<roles> rlList;
        protected string currentSelected;
        protected int aaLogsCheck;
        protected int mgBanCheck;
        protected int mgCollabCheck;
        protected int mgVouchCheck;
        protected int mgAdminLgCheck;
        string OldText = string.Empty;
        protected bool capsuleApp;
        protected bool capsuleCollab;
        protected bool capsuleVouch;
        protected bool capsuleBan;
        protected bool capsuleAdminLog;
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
                    if (Request.QueryString["scss"] == "invited") {
                        result = "true";
                    }
                    Admins ad = new Admins();
                    roles rl = new roles();
                    if (!Convert.ToBoolean(Session["admin"])) {

                        string adEmail = Session["subadminEmail"].ToString();
                        Admins adDetails = ad.GetAllAdminWithEmail(adEmail);
                        cap = rl.GetRole(adDetails.adminRole);
                        capPerm = cap.mgCollab;
                    }

                    if (capPerm == 1 || Convert.ToBoolean(Session["admin"]) == true)
                    {

                        if (!IsPostBack)
                        {
                            adList = ad.GetAllAdmins();
                            rlList = rl.GetAllRoles();
                            // Whatever you want here.
                            tbName.Text = rlList[0].Roles;
                            capsuleApp = Convert.ToBoolean(rlList[0].viewAppLogs);
                            capsuleCollab = Convert.ToBoolean(rlList[0].mgCollab);
                            capsuleVouch = Convert.ToBoolean(rlList[0].mgVouch);
                            capsuleBan = Convert.ToBoolean(rlList[0].mgBan);
                            capsuleAdminLog = Convert.ToBoolean(rlList[0].mgAdLg);
                            aaLogs.Checked = Convert.ToBoolean(rlList[0].viewAppLogs);
                            mgCollab.Checked = Convert.ToBoolean(rlList[0].mgCollab);
                            mgVouch.Checked = Convert.ToBoolean(rlList[0].mgVouch);
                            mgBan.Checked = Convert.ToBoolean(rlList[0].mgBan);
                            mgAdLg.Checked = Convert.ToBoolean(rlList[0].mgAdLg);
                            assignDDL.Items.Clear();
                            assignRoleDDL.Items.Clear();
                            foreach (var adminDetail in adList)
                            {
                                if (adminDetail.adminRole != "Main Admin") {
                                    string fullDeets = adminDetail.adminEmail + " (" + adminDetail.adminName + ")";
                                    assignDDL.Items.Add(new ListItem(fullDeets, adminDetail.adminEmail));
                                }


                            }


                            foreach (var roleDetail in rlList)
                            {

                                Admins adDetails = ad.GetAllAdminWithEmail(assignDDL.SelectedValue);
                                if (roleDetail.Roles == adDetails.adminRole) {
                                    assignRoleDDL.Items.Add(new ListItem(roleDetail.Roles + " (Current)", roleDetail.Roles));

                                }
                                else {
                                    assignRoleDDL.Items.Add(new ListItem(roleDetail.Roles, roleDetail.Roles));
                                }

                            }

                            assignRoleDDL.SelectedValue = ad.GetAllAdminWithEmail(assignDDL.SelectedValue).adminRole;
                        }
                        else
                        {
                            rolename.Visible = false;
                            roleUsed.Visible = false;
                            // show configurations for role
                            if (aaLogs.Checked == true)
                            {
                                aaLogsCheck = 1;
                            }
                            else
                            {
                                aaLogsCheck = 0;

                            }

                            if (mgCollab.Checked == true)
                            {
                                mgCollabCheck = 1;

                            }
                            else
                            {
                                mgCollabCheck = 0;

                            }

                            if (mgVouch.Checked == true)
                            {
                                mgVouchCheck = 1;
                            }
                            else
                            {
                                mgVouchCheck = 0;
                            }

                            if (mgBan.Checked == true)
                            {
                                mgBanCheck = 1;
                            }
                            else {
                                mgBanCheck = 0;
                            }

                            if (mgAdLg.Checked == true)
                            {
                                mgAdminLgCheck = 1;
                            }
                            else
                            {
                                mgAdminLgCheck = 0;
                            }

                            if (assignRoleDDL.SelectedValue != ad.GetAllAdminWithEmail(assignDDL.SelectedValue).adminRole) {
                                CancelRoleAssign.Visible = true;
                                updtRoleAssign.Visible = true;
                            }
                            else { 

                                CancelRoleAssign.Visible = false;
                                updtRoleAssign.Visible = false;
                            }

                        }
                        adList = ad.GetAllAdmins();
                        rlList = rl.GetAllRoles();
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
                else
                {

                    Session.Clear();
                    string err = "SessionKicked";
                    Response.Redirect("homepage.aspx?error=" + err);
                }



            } else // If a non-admin tries to access the page...
            {
                
                Response.Redirect("homepage.aspx");
                
            }
        }


        protected void addCollabBtn_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup();", true);
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            Users user = new Users();
            Admins ad = new Admins();
            blocked bl = new blocked();
            Users findCollaborator = user.GetUserByEmail(AntiXssEncoder.HtmlEncode(collabEmail.Text, true));
            for (int i = 0; i < adList.Count; i++) {
                if (collabEmail.Text == adList[i].adminEmail) {
                    errmsg += "Invitation already Sent!<br>";
                }
            }

            if (AntiXssEncoder.HtmlEncode(collabEmail.Text, true) == "") {
                errmsg += "Please enter an email!<br>";

            }

            if (bl.GetBlockedAccWithEmail(AntiXssEncoder.HtmlEncode(collabEmail.Text, true)) != null) {
                errmsg += "Can't invite a banned user as a sub-admin!<br>";
            }

            if (findCollaborator != null)
            { // user exists
                if (errmsg == "")
                {
                    result = "true";
                    Users us = new Users();
                    adminLog adl = new adminLog();
                    DateTime dt = DateTime.Now;
                    MainAdmins mad = new MainAdmins();
                    string ipAddr = GetIPAddress();
                    string countryLogged = CityStateCountByIp(ipAddr);

                    Users subAdmin = us.GetUserByEmail(AntiXssEncoder.HtmlEncode(collabEmail.Text, true));
                    Execute(AntiXssEncoder.HtmlEncode(collabEmail.Text, true), subAdmin.name);
                    if (Session["admin"] != null)
                    {
                        adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, "Invited <b>" + collabEmail.Text + "</b> as a sub-admin under the <b>" + roleChoice.SelectedValue + "</i> role", "-", Session["adminEmail"].ToString(), countryLogged);
                    }
                    else {
                        adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Invited <b>" + collabEmail.Text + "</b> as a sub-admin under the <b>" + roleChoice.SelectedValue + "</i> role", "-", Session["subadminEmail"].ToString(), countryLogged);

                    }
                    ad.AddAdmin(subAdmin.name, AntiXssEncoder.HtmlEncode(collabEmail.Text, true), roleChoice.SelectedValue);
                    Response.Redirect("addCollaborator.aspx?scss=invited");
                }
                else {
                    result = "false";
                    PanelError.Visible = true;
                    errmsglbl.Text = errmsg;
                }
            }
            else {
                result = "false";
                PanelError.Visible = true;
                errmsglbl.Text = "Please enter a valid user!";

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
        static async Task Execute(string useremail, string username)
        {
            Random rnd = new Random();
            string OTPassword = "";
            OTPassword = rnd.Next(100000, 999999).ToString();
            collabOTP cbOTP = new collabOTP();
            cbOTP.AddCollabOTP(useremail, username, OTPassword, 0);
            var client = new SendGridClient("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");
            var from = new EmailAddress("184707d@mymail.nyp.edu.sg", "ASPJ");
            var subject = "Verify your Sub-admin Account";
            var to = new EmailAddress(useremail, username);
            var plainTextContent = "Please login at http://localhost:60329/homepage.aspx & use this OTP to verify your account:" + OTPassword;
            var htmlContent = "Please login at http://localhost:60329/homepage.aspx & use this OTP to verify your account:<br/>" + OTPassword;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            
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

        protected void roleDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedIT = roleDDL.SelectedItem.Text;
            tbName.Text = selectedIT;
            OldText = tbName.Text;
            foreach (var role in rlList)
            {
                if (role.Roles == selectedIT)
                {
                    aaLogs.Checked = Convert.ToBoolean(role.viewAppLogs);
                    mgCollab.Checked = Convert.ToBoolean(role.mgCollab);
                    mgVouch.Checked = Convert.ToBoolean(role.mgVouch);
                    mgBan.Checked = Convert.ToBoolean(role.mgBan);
                    mgAdLg.Checked = Convert.ToBoolean(role.mgAdLg);
                }
            }
            btnCancel.Visible = false;
            btnSave.Visible = false;
            btnDelete.Visible = true;
            btnUpdate.Visible = true;

        }

        
        protected void addRole_Click(object sender, EventArgs e)
        {
            tbName.Text = "";
            aaLogs.Checked = false;
            mgCollab.Checked = false;
            mgVouch.Checked = false;
            mgBan.Checked = false;
            mgAdLg.Checked = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnCancel.Visible = true;
            btnSave.Visible = true;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/addCollaborator.aspx");
        }



        protected void btnUpdate_Click1(object sender, EventArgs e)
        {
            if (tbName.Text != OldText)
            {
                Admins ad = new Admins();
                adminLog adl = new adminLog();
                MainAdmins mad = new MainAdmins();
                Users us = new Users();
                string ipAddr = GetIPAddress();
                string countryLogged = CityStateCountByIp(ipAddr);
                roles rl = new roles();
                DateTime dt = DateTime.Now;
                rlList = rl.GetAllRoles();
                OldText = rlList[0].Roles;
                roles singleRl = rl.GetRole(roleDDL.SelectedValue);
                rl.UpdateRole(singleRl.RoleId, AntiXssEncoder.HtmlEncode(tbName.Text, true), aaLogsCheck, mgCollabCheck, mgVouchCheck, mgBanCheck, mgAdminLgCheck);
                if (Session["admin"] != null)
                {
                    adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, "Updated " + OldText + " role", "-", Session["adminEmail"].ToString(), countryLogged);
                }
                else
                {
                    adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Updated " + OldText + " role", "-", Session["subadminEmail"].ToString(), countryLogged);

                }
                List<Admins> adRlList;
                adRlList = ad.GetAllAdmins();

                foreach (var elmt in adRlList) {
                    if (elmt.adminRole == roleDDL.SelectedValue)
                    {
                        ad.UpdateRoleByEmail(elmt.adminEmail, AntiXssEncoder.HtmlEncode(tbName.Text, true));
                        
                    }
                    
                }
                Response.Redirect("addCollaborator.aspx");
            }
            else {
                if (tbName.Text == "" || tbName.Text == " " || tbName.Text == "   ") {
                    rolename.Visible = true;
                }
                //Response.Redirect("addCollaborator.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Admins ad = new Admins();
            adminLog adl = new adminLog();
            MainAdmins mad = new MainAdmins();
            Users us = new Users();
            
            string ipAddr = GetIPAddress();
            string countryLogged = CityStateCountByIp(ipAddr);
            roles rl = new roles();
            DateTime dt = DateTime.Now;
            string role = AntiXssEncoder.HtmlEncode(tbName.Text, true);
            int applog = Convert.ToInt32(aaLogs.Checked);
            int manageCollab = Convert.ToInt32(mgCollab.Checked);
            int manageVouch = Convert.ToInt32(mgVouch.Checked);
            int manageBan = Convert.ToInt32(mgBan.Checked);
            int manageAdLg = Convert.ToInt32(mgAdLg.Checked);
            List<roles> rlList;
            rlList = rl.GetAllRoles();
            bool tracker = false;

            if (role != "")
            {
                foreach (var elmnt in rlList)
                {
                    if (role == elmnt.Roles)
                    {
                        tracker = true;
                    }
                }
                if (!tracker)
                {
                    rl.InsertRole(role, applog, manageCollab, manageVouch, manageBan, manageAdLg);
                    if (Session["admin"] != null)
                    {
                        adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, "Created " + role + " role", "-", Session["adminEmail"].ToString(), countryLogged);
                    }
                    else
                    {
                        adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Created" + role + " role", "-", Session["subadminEmail"].ToString(), countryLogged);

                    }
                    Response.Redirect("/addCollaborator.aspx");
                }
                else
                {
                    //  input err msg

                    existMsg.Visible = true;
                }
            }
            else {
                enterMsg.Visible = true;

            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<Admins> adList;
            Admins ad = new Admins();
            adminLog adl = new adminLog();
            MainAdmins mad = new MainAdmins();
            Users us = new Users();

            string ipAddr = GetIPAddress();
            string countryLogged = CityStateCountByIp(ipAddr);
            roles rl = new roles();
            DateTime dt = DateTime.Now;
            adList = ad.GetAllAdmins();
            bool tracker = true;
            foreach (var elmt in adList) {
                if (elmt.adminRole == roleDDL.SelectedValue) {
                    tracker = false;
                    break;
                }
            }
            if (tracker)
            {

                rl.DeleteRole(roleDDL.SelectedValue);
                if (Session["admin"] != null)
                {
                    adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, "Deleted " + roleDDL.SelectedValue + " role", "-", Session["adminEmail"].ToString(), countryLogged);
                }
                else
                {
                    adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Deleted" + roleDDL.SelectedValue + " role", "-", Session["subadminEmail"].ToString(), countryLogged);

                }
                Response.Redirect("addCollaborator.aspx");
            }
            else {
                roleUsed.Visible = true;
            }
        }

        protected void assignRoleDDL_SelectedIndexChanged(object sender, EventArgs e)
        {


            Admins ad = new Admins();
            if (assignRoleDDL.SelectedValue == ad.GetAllAdminWithEmail(assignDDL.SelectedValue).adminRole)
            {
                CancelRoleAssign.Visible = false;
                updtRoleAssign.Visible = false;
            }
            else {
                CancelRoleAssign.Visible = true;
                updtRoleAssign.Visible = true;
            }
        }

        protected void updtRoleAssign_Click(object sender, EventArgs e)
        {
            Admins ad = new Admins();
            adminLog adl = new adminLog();
            MainAdmins mad = new MainAdmins();
            Users us = new Users();

            string ipAddr = GetIPAddress();
            string countryLogged = CityStateCountByIp(ipAddr);
            DateTime dt = DateTime.Now;
            ad.UpdateRoleByEmail(assignDDL.SelectedValue, assignRoleDDL.SelectedValue);
            if (Session["admin"] != null)
            {
                adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, "Updated " + assignDDL.SelectedValue + "'s role to <b>" + assignRoleDDL.SelectedValue +"</b>", "-", Session["adminEmail"].ToString(), countryLogged);
            }
            else
            {
                adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Updated" + assignDDL.SelectedValue + "'s role to <b>" + assignRoleDDL.SelectedValue + "</b>", "-", Session["subadminEmail"].ToString(), countryLogged);

            }
            Response.Redirect("addCollaborator.aspx");
        }

        protected void CancelRoleAssign_Click(object sender, EventArgs e)
        {
            Admins ad = new Admins();
            adList = ad.GetAllAdmins();
            assignDDL.SelectedValue = adList[1].adminEmail;
            assignRoleDDL.SelectedValue = adList[1].adminRole;
            CancelRoleAssign.Visible = false;
            updtRoleAssign.Visible = false;
        }

        protected void assignDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            CancelRoleAssign.Visible = false;
            updtRoleAssign.Visible = false;
            Admins ad = new Admins();
            roles rl = new roles();
            assignRoleDDL.Items.Clear();

            rlList = rl.GetAllRoles();

            //assignRoleDDL.SelectedValue = ad.GetAllAdminWithEmail(assignDDL.SelectedValue).adminRole;
            foreach (var roleDetail in rlList)
            {

                Admins adDetails = ad.GetAllAdminWithEmail(assignDDL.SelectedValue);
                if (roleDetail.Roles == adDetails.adminRole)
                {
                    assignRoleDDL.Items.Add(new ListItem(roleDetail.Roles + " (Current)", roleDetail.Roles));

                }
                else
                {
                    assignRoleDDL.Items.Add(new ListItem(roleDetail.Roles, roleDetail.Roles));
                }

            }

            assignRoleDDL.SelectedValue = ad.GetAllAdminWithEmail(assignDDL.SelectedValue).adminRole;
        }

        protected void collabGV_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Admins ad = new Admins();
            collabOTP cbotp = new collabOTP();
            adList = ad.GetAllAdmins();
            DateTime dt = DateTime.Now;
            Sessionmg ses = new Sessionmg();
            ses.UpdateSession(adList[e.RowIndex].adminEmail, 0);
            ad.DeleteByEmail(adList[e.RowIndex].adminEmail, adList[e.RowIndex].adminName);
            cbotp.DeleteByEmail(adList[e.RowIndex].adminEmail, adList[e.RowIndex].adminName);
            adminLog adl = new adminLog();
            MainAdmins mad = new MainAdmins();
            Users us = new Users();

            string ipAddr = GetIPAddress();
            string countryLogged = CityStateCountByIp(ipAddr);
            if (adList[e.RowIndex].adminStatus == "Accepted")
            {
                if (Session["admin"] != null)
                {
                    adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, "Removed " + adList[e.RowIndex].adminEmail + " (" + adList[e.RowIndex].adminName + ") as a sub-admin", "-", Session["adminEmail"].ToString(), countryLogged);
                }
                else
                {
                    adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Removed " + adList[e.RowIndex].adminEmail + " (" + adList[e.RowIndex].adminName + ") as a sub-admin", "-", Session["subadminEmail"].ToString(), countryLogged);

                }
            }
            else {
                if (Session["admin"] != null)
                {
                    adl.AddAdminLog(dt, mad.GetAdminByEmail(Session["adminEmail"].ToString()).MainadminName, ipAddr, "Revoked " + adList[e.RowIndex].adminEmail + " (" + adList[e.RowIndex].adminName + ")'s sub-admin invitation", "-", Session["adminEmail"].ToString(), countryLogged);
                }
                else
                {
                    adl.AddAdminLog(dt, us.GetUserByEmail(Session["subadminEmail"].ToString()).name, ipAddr, "Revoked " + adList[e.RowIndex].adminEmail + " (" + adList[e.RowIndex].adminName + ")'s sub-admin invitation", "-", Session["subadminEmail"].ToString(), countryLogged);

                }
            }
            Response.Redirect("addCollaborator.aspx");
        }
    }
}