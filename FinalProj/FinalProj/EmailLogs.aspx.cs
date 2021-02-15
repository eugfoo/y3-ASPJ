using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
    public partial class EmailLogs : System.Web.UI.Page
    {
        protected List<EmailLog> elgList = new List<EmailLog>();
        public List<string> complete = new List<string>();
        public string userEmail;

        public string email { get; set; }
        public string senderEmail { get; set; }
        public DateTime dateTime { get; set; }
        public string title { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        { 
            EmailLog elg = new EmailLog();
            Users usr = new Users();


            if (Session["user"] == null)
            {
                Response.Redirect("/homepage.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    Users user = (Users)Session["user"];
                    userEmail = user.email;
                    elgList = elg.GetAllLogsOfUser(userEmail);

                }
                elg.GetAllLogsOfUser(userEmail);
            }
        }
    }
}