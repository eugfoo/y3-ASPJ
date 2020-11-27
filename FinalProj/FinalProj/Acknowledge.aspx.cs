using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
    public partial class Acknowledge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Name"] == "" || Request.QueryString["Email"] == "")
            {
                gcnt.Text = "Please provide your contact!";
            }
            else
            {
                gcnt.Text = "Thank you, " + Request.QueryString["Name"] + ". We will contact you via email " + Request.QueryString["Email"];
            }

        }
    }
}