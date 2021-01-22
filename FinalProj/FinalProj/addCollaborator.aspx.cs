using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace FinalProj
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string result ="";
        protected List<Admins> adList;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(Session["admin"])) // If a non-admin tries to access the page...
            {
                Admins ad = new Admins();
                adList = ad.GetAllAdmins();

                Response.Redirect("homepage.aspx"); // Adios Gladios
            }
            else
            {
                // Whatever you want here.

                string newIIP = GetIPAddress();
                Label1.Text = newIIP.ToString();

                //byte[] fileBytes = File.ReadAllBytes(path);
                //string textAs = Encoding.UTF8.GetString(fileBytes);
                Admins ad = new Admins();
                adList = ad.GetAllAdmins();
                Console.WriteLine(adList);
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
                Execute();

                Users us = new Users();
                Users subAdmin = us.GetUserByEmail(collabEmail.Text);
                Admins ad = new Admins();
                ad.AddAdmin(subAdmin.name, collabEmail.Text);
                Response.Redirect("addCollaborator.aspx");
            }
            else {
                result = "false";

            } 
        }

        static async Task Execute()
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("184707d@mymail.nyp.edu.sg", "ASPJ");
            var subject = "Verify your Sub-admin Account";
            var to = new EmailAddress("eugenefoo9@gmail.com", "Eugene Foo");
            var plainTextContent = "Click here to accept sub admin role: http://localhost:60329/homepage.aspx";
            var htmlContent = "Click here to accept sub admin role: <br/> http://localhost:60329/homepage.aspx";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }


        // Get IP Address of user
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


    }
}