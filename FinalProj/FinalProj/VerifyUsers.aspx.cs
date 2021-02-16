using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class VerifyUsers : System.Web.UI.Page
    {
        protected roles cap;
        protected int capPerm = 0;
        protected Sessionmg sesDeets;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["admin"] == null && Session["subadmin"] == null) // A user has signed in
                {
                    Response.Redirect("/homepage.aspx");
                }
                else
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
                        if (!Convert.ToBoolean(Session["admin"]))
                        {
                            Admins ad = new Admins();
                            roles rl = new roles();
                            string adEmail = Session["subadminEmail"].ToString();
                            Admins adDetails = ad.GetAllAdminWithEmail(adEmail);
                            cap = rl.GetRole(adDetails.adminRole);
                            capPerm = cap.mgCollab;
                        }

                        if (capPerm == 1 || Convert.ToBoolean(Session["admin"]) == true)
                        {
                            Users user = new Users();
                            List<Users> userList = user.getUnverifiedUsers();
                            List<ListItem> Imgs = new List<ListItem>();
                            string path = Path.Combine(Server.MapPath("/Img/User/UserFaceVerification/"));
                            string[] ImagePaths = Directory.GetFiles(path);
                            int j = 0;
                            foreach (var i in userList)
                            {
                                foreach (string imgPath in ImagePaths)
                                {
                                    if (imgPath.Contains(i.email))
                                    {
                                        string ImgName = Path.GetFileName(imgPath);
                                        Imgs.Add(new ListItem(i.email, "~/Img/User/UserFaceVerification/" + ImgName));
                                        j++;
                                    }
                                }
                            }
                            if (Imgs.Count == 0)
                            {
                                lblNoUsers.Visible = true;
                            }
                            else
                            {
                                Gv_imgs.DataSource = Imgs;
                                Gv_imgs.DataBind();
                            }
                        }
                    }
                    else // If a non-admin tries to access the page...
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
            }
        }

        protected void btnVerify_Click(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = Gv_imgs.Rows[index];

            string email = row.Cells[0].Text;
            Users user = new Users();
            user.VerifyOrgByEmail(email);
            Response.Redirect("/VerifyUsers.aspx");
        }
    }
}