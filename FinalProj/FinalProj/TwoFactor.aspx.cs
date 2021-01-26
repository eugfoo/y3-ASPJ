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
    public partial class TwoFactor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["temp"] == null)
            {
                Response.Redirect("LogIn.aspx");
            }
        }
        protected void btnAuthenticator_Click(object sender, EventArgs e)
        {
            
        }
    }
}