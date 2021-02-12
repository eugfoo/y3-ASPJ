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
        protected int mgCollabCheck;
        protected int mgVouchCheck;
        string OldText = string.Empty;
        protected bool capsuleApp;
        protected bool capsuleCollab;
        protected bool capsuleVouch;
        protected roles cap;
        protected int capPerm = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Convert.ToBoolean(Session["admin"]) == true || Convert.ToBoolean(Session["subadmin"]) == true)
            {
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
                        aaLogs.Checked = Convert.ToBoolean(rlList[0].viewAppLogs);
                        mgCollab.Checked = Convert.ToBoolean(rlList[0].mgCollab);
                        mgVouch.Checked = Convert.ToBoolean(rlList[0].mgVouch);
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
                            if (roleDetail.Roles == rl.GetRole(adDetails.adminRole).Roles) {
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
                        CancelRoleAssign.Visible = false;
                        updtRoleAssign.Visible = false;

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
           Users findCollaborator = user.GetUserByEmail(collabEmail.Text);
            for (int i = 0; i < adList.Count; i++) {
                if (collabEmail.Text == adList[i].adminEmail) {
                    errmsg += "Invitation already Sent!<br>";
                }
            }

            if (collabEmail.Text == "") {
                errmsg += "Please enter an email!<br>";

            }

            if (findCollaborator != null)
            { // user exists
                if (errmsg == "")
                {
                    result = "true";
                    Users us = new Users();

                    Users subAdmin = us.GetUserByEmail(AntiXssEncoder.HtmlEncode(collabEmail.Text,true));
                    Execute(AntiXssEncoder.HtmlEncode(collabEmail.Text, true), subAdmin.name);

                    ad.AddAdmin(subAdmin.name, AntiXssEncoder.HtmlEncode(collabEmail.Text, true), roleChoice.SelectedValue);
                    Response.Redirect("addCollaborator.aspx");
                }
                else {
                    result = "false";
                }
            }
            else {
                result = "false";

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

        // yet to do
        protected void remoteBtn_Click(object sender, EventArgs e)
        {
            string asd = remoteBtn.Text;
            
            //ad.DeleteByEmail();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            roles rls = new roles();
            rls.UpdatePermsByRole(AntiXssEncoder.HtmlEncode(tbName.Text, true), Convert.ToInt32(aaLogs.Checked), Convert.ToInt32(mgCollab.Checked), Convert.ToInt32(mgVouch.Checked));
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
                roles rl = new roles();
                rlList = rl.GetAllRoles();
                OldText = rlList[0].Roles;
                roles singleRl = rl.GetRole(roleDDL.SelectedValue);
                rl.UpdateRole(singleRl.RoleId, AntiXssEncoder.HtmlEncode(tbName.Text, true), aaLogsCheck, mgCollabCheck, mgVouchCheck);
                List<Admins> adRlList;
                adRlList = ad.GetAllAdmins();
                foreach (var elmt in adRlList) {
                    if (elmt.adminRole == OldText) {
                        ad.UpdateRoleByEmail(elmt.adminEmail, AntiXssEncoder.HtmlEncode(tbName.Text, true));
                    }
                }
                Response.Redirect("addCollaborator.aspx");
            }
            else {
                Response.Redirect("addCollaborator.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string role = AntiXssEncoder.HtmlEncode(tbName.Text, true);
            int applog = Convert.ToInt32(aaLogs.Checked);
            int manageCollab = Convert.ToInt32(mgCollab.Checked);
            int manageVouch = Convert.ToInt32(mgVouch.Checked);
            roles rl = new roles();
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
                    rl.InsertRole(role, applog, manageCollab, manageVouch);
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
                roles rl = new roles();
                rl.DeleteRole(roleDDL.SelectedValue);
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
            ad.UpdateRoleByEmail(assignDDL.SelectedValue, assignRoleDDL.SelectedValue);
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
    }
}