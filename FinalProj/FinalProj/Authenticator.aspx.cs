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
        //Users user = (Users)Session["user"];

        String AuthenticationCode
        {
            get
            {
                if (ViewState["AuthenticationCode"] != null)
                    return ViewState["AuthenticationCode"].ToString().Trim();
                return String.Empty;
            }
            set
            {
                ViewState["AuthenticationCode"] = value.Trim();
            }
        }

        String AuthenticationTitle
        {
            get
            {
                return "Kovi";
            }
        }


        String AuthenticationBarCodeImage
        {
            get;
            set;
        }

        String AuthenticationManualCode
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblResult.Text = String.Empty;
                lblResult.Visible = false;
                GenerateTwoFactorAuthentication();
                imgQrCode.ImageUrl = AuthenticationBarCodeImage;
                lblManualSetupCode.Text = AuthenticationManualCode;
                lblAccountName.Text = AuthenticationTitle;
            }
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            String pin = txtSecurityCode.Text.Trim();
            Boolean status = ValidateTwoFactorPIN(pin);
            if (status)
            {
                lblResult.Visible = true;
                lblResult.Text = "Code Successfully Verified.";
            }
            else
            {
                lblResult.Visible = true;
                lblResult.Text = "Invalid Code.";
            }
        }

        public Boolean ValidateTwoFactorPIN(String pin)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(AuthenticationCode, pin);
        }

        public Boolean GenerateTwoFactorAuthentication()
        {
            Guid guid = Guid.NewGuid();
            String uniqueUserKey = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
            AuthenticationCode = uniqueUserKey;

            Dictionary<String, String> result = new Dictionary<String, String>();
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