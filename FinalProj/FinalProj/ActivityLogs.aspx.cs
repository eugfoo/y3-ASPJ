using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class WebForm3 : System.Web.UI.Page
    {        
        protected List<ActivityLog> algList;
        protected List<string> picList = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Convert.ToBoolean(Session["admin"])) // If a non-admin tries to access the page...
            {
                Response.Redirect("homepage.aspx"); // Adios Gladios
            }
            else
            {
                // Whatever you want here.

                ActivityLog alg = new ActivityLog();
                algList = alg.GetAllLogsOfActivities();

                for (int i = 0; i < algList.Count; i++)
                {
                    Users us = new Users();
                    string he = algList[i].userEmail;
                    Users lol = us.GetUserByEmail(algList[i].userEmail);

                    picList.Add(us.GetUserByEmail(algList[i].userEmail).DPimage);

                }
            }
           
        }
    }
}