using System;
using System.IO;
using System.Web.UI;
using FinalProj.BLL;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Web.Security.AntiXss;
using System.Web.UI.WebControls;
using System.Net;
using Newtonsoft.Json.Linq;

namespace FinalProj
{
    public partial class EditProfile : System.Web.UI.Page
    {
        public string linkFB = "https://www.facebook.com/";
        public string linkInst = "https://www.instagram.com/";
        public string linkTwit = "https://www.twitter.com/";
        static string finalHash;
        protected List<PassHist> passList;
        protected Sessionmg sesDeets;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null) // A user has signed in
            {
                Sessionmg ses = new Sessionmg();
                blocked bl = new blocked();

                sesDeets = ses.GetSession(Session["email"].ToString());
                if (sesDeets.Active == 1)
                {
                    loadFunctions();
                }
                else {
                    if (bl.GetBlockedAccWithEmail(Session["email"].ToString()) != null)
                    {
                        Session.Clear();
                        string err = "SessionBanned";
                        Response.Redirect("homepage.aspx?error=" + err);
                    }
                }
            }
            else
            {
                Response.Redirect("/homepage.aspx");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Users user = (Users)Session["user"];
            if (tbName.Text != "")
            {
                user.UpdateNameByID(user.id, AntiXssEncoder.HtmlEncode(tbName.Text, true));
            }
            if (tbDesc.Text != "")
            {
                user.UpdateDescByID(user.id, AntiXssEncoder.HtmlEncode(tbDesc.Text, true));
            }
            if (tbFacebook.Text != "")
            {
                user.UpdateFacebookByID(user.id, linkFB + AntiXssEncoder.HtmlEncode(tbFacebook.Text, true));
            }
            if (tbInstagram.Text != "")
            {
                user.UpdateInstagramByID(user.id, linkInst + AntiXssEncoder.HtmlEncode(tbInstagram.Text, true));
            }
            if (tbTwitter.Text != "")
            {
                user.UpdateTwitterByID(user.id, linkTwit + AntiXssEncoder.HtmlEncode(tbTwitter.Text, true));
            }
            if (Session["tempDP"] != null)
            {
                user.UpdateDPByID(user.id, Session["tempDP"].ToString());
            }
            if (Session["tempBP"] != null)
            {
                user.UpdateBPByID(user.id, Session["tempBP"].ToString());
            }

            if (ddlDiet.SelectedItem.Value != user.diet)
            {
                if (ddlDiet.SelectedItem.Value == "Others")
                {
                    user.UpdateDietByID(user.id, AntiXssEncoder.HtmlEncode(tbOtherDiet.Text, true));
                }
                else
                {
                    user.UpdateDietByID(user.id, ddlDiet.SelectedItem.Value);
                }
            }

            Session["tempDP"] = null;
            Session["tempBP"] = null;
            Session["user"] = user.GetUserByEmail(user.email);
            Response.Redirect("/EventStatus.aspx");
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Verify.aspx");
        }

        protected void ddlDiet_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItemId = ddlDiet.SelectedItem.Value;
            if (selectedItemId == "Others")
            {
                tbOtherDiet.Attributes.Remove("readonly");
            }
            else
            {
                tbOtherDiet.Attributes.Add("readonly", "readonly");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["tempDP"] = null;
            Session["tempBP"] = null;
            Response.Redirect("/EventStatus.aspx");
        }

        protected void btnUploadDP_Click(object sender, EventArgs e)
        {
            if (fuDP.HasFile)
            {
                var uniqueFileName = string.Format(@"{0}.png", Guid.NewGuid());
                string fileName = Path.Combine(Server.MapPath("/Img/User"), uniqueFileName);
                fuDP.SaveAs(fileName);
                imgDP.ImageUrl = "/Img/User/" + uniqueFileName;
                Session["tempDP"] = imgDP.ImageUrl;
            }
        }

        protected void btnUploadBP_Click(object sender, EventArgs e)
        {
            if (fuBP.HasFile)
            {
                var uniqueFileName = string.Format(@"{0}.png", Guid.NewGuid());
                string fileName = Path.Combine(Server.MapPath("/Img/User"), uniqueFileName);
                fuBP.SaveAs(fileName);
                imgBP.ImageUrl = "/Img/User/" + uniqueFileName;
                Session["tempBP"] = imgBP.ImageUrl;
            }
        }

        public void loadFunctions() // Give the illusion of cleaner code.
        {
            Users user = (Users)Session["user"];
            tbOtherDiet.Attributes.Add("readonly", "readonly");
            if (user.DPimage != "")
            {
                if (Session["tempDP"] != null)
                {
                    imgDP.ImageUrl = Session["tempDP"].ToString();
                }
                else
                {
                    imgDP.ImageUrl = user.DPimage;
                }
            }
            if (user.BPimage != "")
            {
                if (Session["tempBP"] != null)
                {
                    imgBP.ImageUrl = Session["tempBP"].ToString();
                }
                else
                {
                    imgBP.ImageUrl = user.BPimage;
                }
            }
            if (!Page.IsPostBack)
            {
                if (user.diet == "" || user.diet == "None")
                {
                    ddlDiet.SelectedIndex = 0;
                }
                else if (user.diet == "Halal")
                {
                    ddlDiet.SelectedIndex = 1;
                }
                else if (user.diet == "Vegetarian")
                {
                    ddlDiet.SelectedIndex = 2;
                }
                else
                {
                    ddlDiet.SelectedIndex = 3;
                    tbOtherDiet.Attributes.Remove("readonly");
                    tbOtherDiet.Text = user.diet;
                }
            }
            if (!Page.IsPostBack)
            {
                if (user.twofactor == 0)
                {
                    CB2FA.Checked = false;
                }
                else
                {
                    CB2FA.Checked = true;
                }
            }
            if (!Page.IsPostBack)
            {
                if (user.googleauth == 0)
                {
                    cbGoogle.Checked = false;
                }
                else
                {
                    cbGoogle.Checked = true;
                }
            }
            if (!Page.IsPostBack)
            {
                List<ListItem> Imgs = new List<ListItem>();
                string path = Path.Combine(Server.MapPath("~/Img/User/UserFaceVerification/"));
                string[] ImagePaths = Directory.GetFiles(path);

                foreach (string imgPath in ImagePaths)
                {
                    if (imgPath.Contains(user.email))
                    {
                        string ImgName = Path.GetFileName(imgPath);
                        Imgs.Add(new ListItem(user.email, "~/Img/User/UserFaceVerification/" + ImgName));
                    }
                }

                if (user.verified == 1)
                {
                    btnVerify.Visible = false;
                    lblVerified.Visible = false;
                }
                else
                {
                    if (Imgs.Count > 0)
                    {
                        btnVerify.Enabled = false;
                        lblVerified.Visible = true;
                    }
                }
            }
        }

        protected void cbGoogle_CheckedChanged(object sender, EventArgs e)
        {
            if (cbGoogle.Checked == true)
            {
                Response.Redirect("Authenticator.aspx");
            }
            if (cbGoogle.Checked == false)
            {
                Users usr = new Users();
                Users user = (Users)Session["user"];
                usr.UpdateGoogleAuthByID(user.id, "", 0);
            }
        }

        protected void CB2FA_CheckedChanged(object sender, EventArgs e)
        {
            Users usr = new Users();
            Users user = (Users)Session["user"];
            if (CB2FA.Checked == true)
            {
                user.UpdateTwoFactorByID(user.id, 1);
                Response.Redirect("Authenticator.aspx");
            }
            if (CB2FA.Checked == false)
            {

                usr.UpdateTwoFactorByID(user.id, 0);
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string passString = AntiXssEncoder.HtmlEncode(userPassword.Text.ToString().Trim(), true);
            string passHash = ComputeSha256Hash(AntiXssEncoder.HtmlEncode(userPassword.Text, true));
            Users user = new Users();
            Users tryingUser = user.GetUserByEmail(Session["email"].ToString());
            PassHist pass = new PassHist();
            passList = pass.getAllPassById(tryingUser.email);

            int counter = 0;

            foreach (var passVar in passList)
            {
                Console.WriteLine(passVar);
                if (passHash == passVar.passHashHist)
                {
                    counter++;
                }
            }
            if (counter > 0)
            {
                lblError.Text = "This is an old password!";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }

            else if (userPassword.Text != userPassword1.Text)
            {
                lblError.Text = "Passwords are not the same!";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }
            else
            {
                string dbSalt = tryingUser.passSalt;
                SHA256Managed hashing = new SHA256Managed();
                if (dbSalt != null && dbSalt.Length > 0)
                {
                    Logs lg = new Logs();
                    ActivityLog alg = new ActivityLog();
                    string pwdWithSalt = passString + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);
                    finalHash = Convert.ToBase64String(hashWithSalt);
                    DateTime now = DateTime.Now;
                    string DTNow = now.ToString("g");
                    string ipAddr = GetIPAddress();
                    string countryLogged = CityStateCountByIp(ipAddr);
                    PassHist pass1 = new PassHist(tryingUser.email, passHash, DTNow);
                    pass1.AddPass();
                    user.UpdatePassByID(tryingUser.id, finalHash);
                    lg.AddLog(Session["email"].ToString(), now, ipAddr, countryLogged, "Successfully Changed Password");
                    alg.AddActivityLog(now, Session["Name"].ToString(), ipAddr, "Successfully Changed Password", "-", Session["email"].ToString(), countryLogged);
                    lblSuccess.Visible = true;
                    lblError.Text = "";
                }
            }
        }
        public static string CityStateCountByIp(string IP)
        {
            //var url = "http://freegeoip.net/json/" + IP;
            //var url = "http://freegeoip.net/json/" + IP;
            string url = "http://api.ipstack.com/" + IP + "?access_key=01ca9062c54c48ef1b7d695b008cae00";
            var request = System.Net.WebRequest.Create(url);
            WebResponse myWebResponse = request.GetResponse();
            Stream stream = myWebResponse.GetResponseStream();

            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                JObject objJson = JObject.Parse(json);
                string Country = objJson["country_name"].ToString();
                string Country_code = objJson["country_code"].ToString();
                if (Country == "")
                {
                    Country = "-";
                }

                else if (Country_code == "")
                {
                    Country = Country;
                }
                else
                {
                    Country = Country + " (" + Country_code + ")";
                }
                return Country;

            }

        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        protected void userPassword_TextChanged(object sender, EventArgs e)
        {
            string passHash = ComputeSha256Hash(AntiXssEncoder.HtmlEncode(userPassword.Text, true));
            Users user = new Users();
            Users tryingUser = user.GetUserByEmail(Session["email"].ToString());
            PassHist pass = new PassHist();
            passList = pass.getAllPassById(tryingUser.email);

            int counter = 0;

            foreach (var passVar in passList)
            {
                if (passHash == passVar.passHashHist)
                {
                    counter++;
                }
            }
            if (counter > 0)
            {
                lblError.Text = "This is an old password!";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }

            else if (userPassword.Text != userPassword1.Text)
            {
                lblError.Text = "Passwords are not the same!";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }
            else
            {
                lblError.Text = "";
            }
        }

        protected void userPassword1_TextChanged(object sender, EventArgs e)
        {
            string passHash = ComputeSha256Hash(AntiXssEncoder.HtmlEncode(userPassword.Text, true));
            Users user = new Users();
            Users tryingUser = user.GetUserByEmail(Session["email"].ToString());
            PassHist pass = new PassHist();
            passList = pass.getAllPassById(tryingUser.email);

            int counter = 0;

            foreach (var passVar in passList)
            {
                if (passHash == passVar.passHashHist)
                {
                    counter++;
                }
            }
            if (counter > 0)
            {
                lblError.Text = "This is an old password!";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }

            if (userPassword.Text != userPassword1.Text)
            {
                lblError.Text = "Passwords are not the same!";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }
            else
            {
                lblError.Text = "";
            }
        }

        public string ComputeSha256Hash(string data)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


    }
}
/*if (CB2FA.Checked == true)
{
    var twofactor = 1;
    user.UpdateTwoFactorByID(user.id, twofactor);
}*/