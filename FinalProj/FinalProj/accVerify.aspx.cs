using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;


namespace FinalProj
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        collabOTP coll;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] == null) // A user has signed in
            {
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
            if (coll.userOTP == accVerifytb.Text)
            {
                //update here
                cbOTP.UpdateOTPByEmail(Session["email"].ToString(), "", 1);
                ad.UpdateStatusByEmail(Session["email"].ToString(), "Accepted");
                scssmsg.Text = "Successful Verification!";
                PanelSuccess.Visible = true;
                Response.Redirect("/homepage.aspx");
            }
            else {
                // show error message wrong otp
        
                errmsgTb.Text = "Incorrect OTP!";
                PanelError.Visible = true;

            }


        }
    }
}