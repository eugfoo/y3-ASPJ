using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Authenticator;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class TwoFactor1 : System.Web.UI.Page
    {
        string AuthenticationCode
        {
            get
            {
                if (ViewState["AuthenticationCode"] != null)
                    return ViewState["AuthenticationCode"].ToString().Trim();
                return string.Empty;
            }
            set
            {
                ViewState["AuthenticationCode"] = value.Trim();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["temp"] == null)
            {
                Response.Redirect("LogIn.aspx");
            }
        }
        protected void BtnAuthenticator_Click(object sender, EventArgs e)
        {
            string pin = tbAuthenticator.Text.Trim();
            bool status = ValidateTwoFactorPIN(pin);
            if (status)
            {
                Users usr = new Users();
                Users user = (Users)Session["user"];

            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Invalid Code.";
            }
        }
        public bool ValidateTwoFactorPIN(string pin)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(AuthenticationCode, pin);
        }

    }
}
}