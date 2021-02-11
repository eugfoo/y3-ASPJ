using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Web.Services;

namespace FinalProj
{
    [ScriptService]
    public partial class Verify : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null) // A user has signed in
            {
                Response.Redirect("/homepage.aspx");
            }
        }


        //protected void btnVerify_Click(object sender, EventArgs e)
        //{
        //        var uniqueFileName = string.Format(@"{0}.png", Guid.NewGuid());
        //        string fileName = Path.Combine(Server.MapPath("/Img/User/UserFaceVerification"), uniqueFileName);
        //        fuDP.SaveAs(fileName);
        //        imgDP.ImageUrl = "/Img/User/" + uniqueFileName;
        //        Session["tempDP"] = imgDP.ImageUrl;
        //}

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

            //string filePath = string.Format("~/Img/User/UserFaceVerification/{0}.png", Path.GetRandomFileName());
            File.WriteAllBytes(fileNameWitPath, bytes);
            lblResult.Visible = true;
            lblResult.Text = "Successfully Submitted for Verification! Please wait while we verify your account. In the meantime, Check out the events!";
            
        }

        [WebMethod()]
        public void UploadImage(string imageData)
        {
            string path = Path.Combine(Server.MapPath("/Img/User/UserFaceVerification"));
            Users user = (Users)Session["user"];

            string fileNameWitPath = path + "kovitest" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".png";
            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }
        }
    }
}