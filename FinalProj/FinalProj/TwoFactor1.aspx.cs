using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Authenticator;
using FinalProj.BLL;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace FinalProj
{
    public partial class TwoFactor1 : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("LogIn.aspx");
            }
        }
        public bool ValidateTwoFactorPIN(string pin)
        {
            Users usr = new Users();
            Users user = (Users)Session["user"];
            var AuthenticationCode = user.googleKey;
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(AuthenticationCode, pin);
        }
        protected void btnAuthenticate_Click(object sender, EventArgs e)
        {
            string pin = tbAuthenticator.Text.Trim();
            bool status = ValidateTwoFactorPIN(pin);
            if (status == true)
            {
                HttpCookie cookie = Request.Cookies["SessionID"];
                if (cookie == null)
                {
                    Users usr = new Users();
                    Users user = (Users)Session["user"];
                    EmailNewDevice(user.email, user.name);
                    //Creates new cookie session
                    Guid guid = Guid.NewGuid();
                    string uSid = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
                    HttpCookie cookie2 = new HttpCookie("SessionID");
                    cookie2["sid"] = uSid;
                    cookie2.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(cookie2);
                    Response.Redirect("homepage.aspx");
                }
            }
            else
            {
                lblError.Visible = true;
            }
        }
        public static async Task EmailNewDevice(string email, string name)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("tzhanyu@gmail.com", "ClearView21");
            var subject = "New login from unknown device";
            var to = new EmailAddress(email, name);
            var plainTextContent = "";
            var htmlContent = "<strong><h2>We detected a new sign-in to your account from an unknown device</h2></strong><br/>" +
                "<p>If this was you, please ignore this email, otherwise click here to change your password " + "http://localhost:60329/EditProfile.aspx" + "</p></strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
