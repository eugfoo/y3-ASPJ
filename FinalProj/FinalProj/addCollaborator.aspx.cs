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

namespace FinalProj
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string result ="";
        public string errmsg = "";

        protected List<Admins> adList;
        protected List<roles> rlList;



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(Session["admin"])) // If a non-admin tries to access the page...
            {
                
                Response.Redirect("homepage.aspx"); // Adios Gladios
            }
            else
            {
                // Whatever you want here.
                Admins ad = new Admins();
                adList = ad.GetAllAdmins();

                roles rl = new roles();
                rlList = rl.GetAllRoles();

               

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
                    errmsg = "Invitation already Sent!";
                }
            }
            
            if (findCollaborator != null)
            { // user exists
                if (errmsg == "")
                {
                    result = "true";
                    Users us = new Users();
                    Users subAdmin = us.GetUserByEmail(collabEmail.Text);
                    Execute(collabEmail.Text, subAdmin.name);

                    ad.AddAdmin(subAdmin.name, collabEmail.Text);
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
            var to = new EmailAddress("eugenefoo9@gmail.com", "Eugene Foo");
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

        protected void remoteBtn_Click(object sender, EventArgs e)
        {
            string asd = remoteBtn.Text;
            
            //ad.DeleteByEmail();
        }
    }
}