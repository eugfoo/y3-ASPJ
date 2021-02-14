using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
    public partial class Verify : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["user"] == null) // A user has signed in
            //{
            //    Response.Redirect("/homepage.aspx");
            //}
        }

        protected void Save(object sender, EventArgs e)
        {
            Users user = (Users)Session["user"];

            //Read the Base64 string from Hidden Field.
            string base64 = Request.Form[hfImageData.UniqueID].Split(',')[1];

            //Convert Base64 string to Byte Array.
            byte[] bytes = Convert.FromBase64String(base64);

            //Save the Byte Array as Image File.
            string path = Path.Combine(Server.MapPath("/Img/User/UserFaceVerification/"));
            string fileNameWitPath = path + user.email.ToString() + ".png";
            string dbPath = "/Img/User/UserFaceVerification/" + user.email.ToString() + ".png";

            //string filePath = string.Format("~/Img/User/UserFaceVerification/{0}.png", Path.GetRandomFileName());
            user.UpdateVerifyImage(user.id, dbPath);
            File.WriteAllBytes(fileNameWitPath, bytes);
            
        }
    }
}