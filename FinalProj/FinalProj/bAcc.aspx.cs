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
        protected roles cap;
        protected int capPerm = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["admin"]) == true || Convert.ToBoolean(Session["subadmin"]) == true)
            {
                Admins ad = new Admins();
                roles rl = new roles();

                if (!Convert.ToBoolean(Session["admin"]))
                {
                    string adEmail = Session["subadminEmail"].ToString();
                    Admins adDetails = ad.GetAllAdminWithEmail(adEmail);
                    cap = rl.GetRole(adDetails.adminRole);
                    capPerm = cap.mgBan;
                }

                if (capPerm == 1 || Convert.ToBoolean(Session["admin"]) == true)
                {

                } else
                {
                    if (Convert.ToBoolean(Session["subadmin"]))
                    {
                        string err = "NoPermission";
                        Response.Redirect("homepage.aspx?error=" + err);
                    }
                    else
                    {
                        Response.Redirect("homepage.aspx");
                    }
                }
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