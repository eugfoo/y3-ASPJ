using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;


namespace FinalProj
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        collabOTP coll;
        List<Admins> adList;
        bool exist = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Admins ad = new Admins();
            adList = ad.GetAllAdmins();
            if (Session["email"] != null)
            {
                foreach (Admins element in adList)
                {
                    if (Session["email"].ToString() == element.adminEmail && element.adminStatus == "Pending")
                    {
                        exist = true;
                    }
                    else if (Session["email"].ToString() == element.adminEmail && element.adminStatus == "Accepted")
                    {
                        exist = false;
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