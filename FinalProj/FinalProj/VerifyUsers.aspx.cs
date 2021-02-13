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
            //if (Session["admin"] == null) // A user has signed in
            //{
            //    Response.Redirect("/homepage.aspx");
            //}
            //else
            //{
            Users user = new Users();
            List<Users> userList = user.getAllUsers();
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

            Gv_imgs.DataSource = Imgs;
            Gv_imgs.DataBind();



            //}
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            var id = Gv_imgs.SelectedDataKey.Values[0].ToString();
        }
    }
}