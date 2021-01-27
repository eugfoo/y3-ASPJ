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
            if(Session["user"] == null)
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
                
                Response.Redirect("homepage.aspx");
            }
            else
            {
                lblError.Visible = true;
            }
        }
       

    }


}
