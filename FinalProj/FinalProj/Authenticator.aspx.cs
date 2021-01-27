using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class Authenticator : System.Web.UI.Page
    {
        public string viewingUserId;

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

        string AuthenticationTitle
        {
            get
            {
                return "Kovi";
            }
        }


        string AuthenticationBarCodeImage
        {
            get;
            set;
        }

        string AuthenticationManualCode
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblResult.Text = string.Empty;
                lblResult.Visible = false;
                GenerateTwoFactorAuthentication();
                imgQrCode.ImageUrl = AuthenticationBarCodeImage;
                lblManualSetupCode.Text = AuthenticationManualCode;
                lblAccountName.Text = AuthenticationTitle;

                if (Session["user"] == null)
                {
                    Response.Redirect("homepage.aspx");
                }
                else
                {

                }
                }
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            string pin = txtSecurityCode.Text.Trim();
            bool status = ValidateTwoFactorPIN(pin);
            if (status)
            {
                Users usr = new Users();
                Users user = (Users)Session["user"];

                usr.UpdateGoogleAuthByID(user.id, AuthenticationCode, 1);
                lblResult.Visible = true;
                lblResult.Text = "Code Successfully Verified.";

            }
            else
            {
                lblResult.Visible = true;
                lblResult.Text = "Invalid Code.";
            }
        }

        public bool ValidateTwoFactorPIN(string pin)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(AuthenticationCode, pin);
        }

        public bool GenerateTwoFactorAuthentication()
        {
            Guid guid = Guid.NewGuid();
            string uniqueUserKey = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
            AuthenticationCode = uniqueUserKey;

            Dictionary<string, string> result = new Dictionary<string, string>();
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("ClearView", AuthenticationTitle, AuthenticationCode, false, 3); if (setupInfo != null)
            {
                AuthenticationBarCodeImage = setupInfo.QrCodeSetupImageUrl;
                AuthenticationManualCode = setupInfo.ManualEntryKey;
                return true;
            }
            return false;
        }
    }
}