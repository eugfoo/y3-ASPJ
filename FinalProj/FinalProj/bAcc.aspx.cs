using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["admin"]) == true || Convert.ToBoolean(Session["subadmin"]) == true)
            {
            }
            else
            {
                Response.Redirect("homepage.aspx");

            }
        }

        protected void blockedGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            blocked bl = new blocked();
            List<blocked> blList;
            blList = bl.getAllBlockedUsers();
            bl.unblock(blList[e.RowIndex].email);
            Response.Redirect("bAcc.aspx");
        }
    }
}