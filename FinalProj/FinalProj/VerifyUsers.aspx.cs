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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["admin"] == null) // A user has signed in
                {
                    Response.Redirect("/homepage.aspx");
                }
                else
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