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
        String line;
        string path = @"C:\Users\Eugene Foo\Documents\y3-ASPJ\FinalProj\FinalProj\EAK.txt";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(Session["admin"])) // If a non-admin tries to access the page...
            {
                Response.Redirect("homepage.aspx"); // Adios Gladios
            }
            else
            {
                // Whatever you want here.


                byte[] fileBytes = File.ReadAllBytes(path);
                string textAs = Encoding.UTF8.GetString(fileBytes);
                



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
                resultMsg.Visible = true;
                resultMsg.Text = "Collaborator Added";
                Execute();

            }
            else {
                result = "false";

                resultMsg.Visible = true;
                resultMsg.Text = "Collaborator Not Found";
            } 
        }


        static async Task Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("mk");
            var client = new SendGridClient("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");
            var from = new EmailAddress("184707d@mymail.nyp.edu.sg", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("eugenefoo9@gmail.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }


    }
}