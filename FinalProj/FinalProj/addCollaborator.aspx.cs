﻿using System;
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

        protected List<Admins> adList;
        protected List<roles> rlList;
        protected string currentSelected;
        string OldText = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(Session["admin"])) // If a non-admin tries to access the page...
            {
                
                Response.Redirect("homepage.aspx"); // Adios Gladios
            }
            else
            {
                if (!IsPostBack)
                {
                    // Whatever you want here.
                    Admins ad = new Admins();
                    adList = ad.GetAllAdmins();

                    roles rl = new roles();
                    rlList = rl.GetAllRoles();
                    tbName.Text = rlList[0].Roles;
                    aaLogs.Checked = Convert.ToBoolean(rlList[0].viewAppLogs);
                    mgCollab.Checked = Convert.ToBoolean(rlList[0].mgCollab);
                    mgVouch.Checked = Convert.ToBoolean(rlList[0].mgVouch);
                }
                else
                {

                    Admins ad = new Admins();
                    adList = ad.GetAllAdmins();
                    roles rl = new roles();
                    rlList = rl.GetAllRoles();
                    // show configurations for role
                }

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

        protected void aaLogs_CheckedChanged(object sender, EventArgs e)
        {
            roles rls = new roles();

            if (aaLogs.Checked == true)
            {
                rls.UpdatePermsByRole(AntiXssEncoder.HtmlEncode(tbName.Text, true), 1, Convert.ToInt32(mgCollab.Checked), Convert.ToInt32(mgVouch.Checked));
            }
            else {
                rls.UpdatePermsByRole(AntiXssEncoder.HtmlEncode(tbName.Text, true), 0, Convert.ToInt32(mgCollab.Checked), Convert.ToInt32(mgVouch.Checked));

            }
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
        }

        protected void mgCollab_CheckedChanged(object sender, EventArgs e)
        {
            roles rls = new roles();

            if (aaLogs.Checked == true)
            {
                rls.UpdatePermsByRole(AntiXssEncoder.HtmlEncode(tbName.Text, true), Convert.ToInt32(aaLogs.Checked), 1 , Convert.ToInt32(mgVouch.Checked));
            }
            else
            {
                rls.UpdatePermsByRole(AntiXssEncoder.HtmlEncode(tbName.Text, true), Convert.ToInt32(aaLogs.Checked), 0, Convert.ToInt32(mgVouch.Checked));

            }
        }

        protected void mgVouch_CheckedChanged(object sender, EventArgs e)
        {
            roles rls = new roles();

            if (aaLogs.Checked == true)
            {
                rls.UpdatePermsByRole(AntiXssEncoder.HtmlEncode(tbName.Text, true), Convert.ToInt32(aaLogs.Checked), Convert.ToInt32(mgCollab.Checked), 1);
            }
            else
            {
                rls.UpdatePermsByRole(AntiXssEncoder.HtmlEncode(tbName.Text, true), Convert.ToInt32(aaLogs.Checked), Convert.ToInt32(mgCollab.Checked), 0);
            }
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
            if (tbName.Text == "") { 
                
            }
            roles rl = new roles();
            rlList = rl.GetAllRoles();
            OldText = rlList[0].Roles;
            rl.UpdateRoleName(OldText, AntiXssEncoder.HtmlEncode(tbName.Text, true));
            Response.Redirect("addCollaborator.aspx");
        }
    }
}