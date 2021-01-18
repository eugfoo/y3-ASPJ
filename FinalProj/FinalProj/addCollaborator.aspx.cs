using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;


namespace FinalProj
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string result ="";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(Session["admin"])) // If a non-admin tries to access the page...
            {
                Response.Redirect("homepage.aspx"); // Adios Gladios
            }
            else
            { 
                // Whatever you want here.
                
            }
        }

        protected void addCollabBtn_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup();", true);
        }

        protected void submit_Click(object sender, EventArgs e)
        {

            Users user = new Users();
            Users findCollaborator = user.GetUserByEmail(collabEmail.Text);
            if (findCollaborator != null)
            { // user exists
                result = "true";
                resultMsg.Visible = true;
                resultMsg.Text = "Collaborator Added";
            }
            else {
                result = "false";

                resultMsg.Visible = true;
                resultMsg.Text = "Collaborator Not Found";
            }
        }
    }
}