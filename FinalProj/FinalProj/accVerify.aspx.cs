using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FinalProj
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        collabOTP coll;
        List<Admins> adList;
        bool exist = false;
        protected Sessionmg sesDeets;

        protected void Page_Load(object sender, EventArgs e)
        {
            Admins ad = new Admins();
            collabOTP clbotp = new collabOTP();

            if (Session["email"] != null)
            {
                Sessionmg ses = new Sessionmg();
                blocked bl = new blocked();

                sesDeets = ses.GetSession(Session["email"].ToString());
                if (sesDeets.Active == 1)
                {
                    collabOTP clbotpDetails = clbotp.GetUserByEmailOTP(Session["email"].ToString());
                    adList = ad.GetAllAdmins();
                    foreach (Admins element in adList)
                    {
                        if (Session["email"].ToString() == element.adminEmail && element.adminStatus == "Pending")
                        {
                            exist = true;
                            if (clbotpDetails.userOTP == "" && clbotpDetails.OTPStatus == 0)
                            {
                                resendOTP.Visible = true;
                                PanelError.Visible = true;
                                errmsgTb.Text = "Your previous OTP has expired, please resend yourself a new OTP";
                                errmsgTb.Visible = true;
                            }
                        }
                        else if (Session["email"].ToString() == element.adminEmail && element.adminStatus == "Accepted")
                        {
                            exist = false;
                        }
                    }
                }
                else {
                    if (bl.GetBlockedAccWithEmail(Session["email"].ToString()) != null)
                    {
                        Session.Clear();
                        string err = "SessionBanned";
                        Response.Redirect("homepage.aspx?error=" + err);
                    }
                    else {
                        string err = "SessionRevoked";
                        Response.Redirect("homepage.aspx?error=" + err);
                    }
                }
            }
            
            if (Session["user"] == null) // A user has signed in
            {
                Response.Redirect("/homepage.aspx");
            } else if (!exist) {
                Response.Redirect("/homepage.aspx");
            }
            else
            {

            }
        }

        protected void submitVerify_Click(object sender, EventArgs e)
        {
            collabOTP cbOTP = new collabOTP();
            Admins ad = new Admins();
            coll = cbOTP.GetUserByEmailOTP(Session["email"].ToString());
            if (coll.userOTP == AntiXssEncoder.HtmlEncode(accVerifytb.Text, true))
            {
                //update here
                cbOTP.UpdateOTPByEmail(Session["email"].ToString(), "", 1);
                ad.UpdateStatusByEmail(Session["email"].ToString(), "Accepted");
                scssmsg.Text = "Successful Verification!";
                PanelSuccess.Visible = true;
                Session.Clear();
                Response.Redirect("/homepage.aspx");
            }
            else {
                // show error message wrong otp
        
                errmsgTb.Text = "Incorrect OTP!";
                PanelError.Visible = true;

            }
        }

        protected void resendOTP_Click(object sender, EventArgs e)
        {
            Users us = new Users();
            Users subAdmin = us.GetUserByEmail(Session["email"].ToString());
            Execute(Session["email"].ToString(), subAdmin.name);
            PanelSuccess.Visible = true;
            scssmsg.Text = "New OTP has been sent";
            scssmsg.Visible = true;
            resendOTP.Visible = false;
            PanelError.Visible = false;
            errmsgTb.Visible = false;
        }

        static async Task Execute(string useremail, string username)
        {
            Random rnd = new Random();
            string OTPassword = "";
            OTPassword = rnd.Next(100000, 999999).ToString();
            collabOTP cbOTP = new collabOTP();
            collabOTP cbDetails = cbOTP.GetUserByEmailOTP(useremail);
            cbOTP.UpdateOTPByEmail(cbDetails.userEmail, OTPassword, 0);
            //cbOTP.AddCollabOTP(useremail, username, OTPassword, 0);
            var client = new SendGridClient("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");
            var from = new EmailAddress("184707d@mymail.nyp.edu.sg", "ASPJ");
            var subject = "Verify your Sub-admin Account";
            var to = new EmailAddress(useremail, username);
            var plainTextContent = "Please login at http://localhost:60329/homepage.aspx & use this OTP to verify your account:" + OTPassword;
            var htmlContent = "Please login at http://localhost:60329/homepage.aspx & use this OTP to verify your account:<br/>" + OTPassword;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);


        }
    }
}