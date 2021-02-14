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
    public partial class WebForm5 : System.Web.UI.Page
    {
        protected roles cap;
        protected int capPerm = 0;
        protected Sessionmg sesDeets;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["admin"]) == true || Convert.ToBoolean(Session["subadmin"]) == true)
            {
                Sessionmg ses = new Sessionmg();
                if (Convert.ToBoolean(Session["subadmin"]))
                {
                    sesDeets = ses.GetSession(Session["subadminEmail"].ToString());
                }
                else
                {
                    sesDeets = ses.GetSession(Session["adminEmail"].ToString());

                }
                if (sesDeets.Active == 1)
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

                    }
                    else
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
                else {
                    Session.Clear();
                    string err = "SessionKicked";
                    Response.Redirect("homepage.aspx?error=" + err);
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

        protected void banAccBtn_Click(object sender, EventArgs e)
        {
            blocked bl = new blocked();
            Users us = new Users();
            Users usDeets;
            string bantb = AntiXssEncoder.HtmlEncode(banEmail.Text, true);
            string banreasontb = AntiXssEncoder.HtmlEncode(banreasonTB.Text, true);
            usDeets = us.GetUserByEmail(bantb);
            DateTime dtn = DateTime.Now;
            if (bantb != "")
            {
                if (banreasontb != "") { 
                    if (usDeets != null)
                    {
                        if (Session["subadminEmail"] != null) {
                            if (bantb == Session["subadminEmail"].ToString())
                            {
                                PanelError.Visible = true;
                                errmsgTb.Text = "You can't ban yourself!";
                            }
                            else
                            {
                                Admins ad = new Admins();
                                if (ad.GetAllAdminWithEmail(bantb) != null)
                                {
                                    PanelError.Visible = true;
                                    errmsgTb.Text = "You can't ban an admin or sub-admin!";
                                }
                                else
                                {
                                    bl.AddBlockedAcc(bantb, usDeets.name, banreasontb, dtn);
                                    Sessionmg ses = new Sessionmg();
                                    ses.UpdateSession(bantb, 0);
                                    Response.Redirect("/bAcc.aspx");
                                }
                            }
                        } else if (Session["adminEmail"] != null) { 
                            if (bantb == Session["adminEmail"].ToString()) {
                                PanelError.Visible = true;
                                errmsgTb.Text = "You can't ban yourself!";
                            }
                            else
                            {
                                Admins ad = new Admins();
                                if (ad.GetAllAdminWithEmail(bantb) != null)
                                {
                                    PanelError.Visible = true;
                                    errmsgTb.Text = "You can't ban an admin or sub-admin!";
                                }
                                else
                                {
                                    bl.AddBlockedAcc(bantb, usDeets.name, banreasontb, dtn);
                                    Sessionmg ses = new Sessionmg();
                                    ses.UpdateSession(bantb, 0);
                                    Response.Redirect("/bAcc.aspx");
                                }
                            }
                        
                        }
                    }
                    else
                    {
                        // show error msg
                        PanelError.Visible = true;
                        errmsgTb.Text = "User doesn't exist!";
                    }
                }
                else{
                    PanelError.Visible = true;
                    errmsgTb.Text = "Please enter a reason for the ban!";
                }
            }
            else {
                PanelError.Visible = true;
                errmsgTb.Text = "Please enter an Email!";
            }

        }
    }
}