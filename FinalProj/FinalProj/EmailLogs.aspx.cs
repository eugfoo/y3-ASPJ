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
        public int userId;
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
                    userId = user.id;
                    elgList = elg.GetAllLogsFromId(userId);

                    for (int i = 0; i < elgList.Count; i++)
                    {
                        lblUserEmail.Text = elg.userEmail;
                        lblTitle.Text = elg.emailTitle;
                        lblDate.Text = elg.dateTime.ToString();
                        lblSenderEmail.Text = elg.senderEmail;
                    }
                }
                elg.GetAllLogsFromId(userId);
            }
        }
    }
}