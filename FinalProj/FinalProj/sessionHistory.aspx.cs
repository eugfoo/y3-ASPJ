using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected List<Logs> lgList;
        protected void Page_Load(object sender, EventArgs e)

        {
            string em;
            if (Session["user"] != null)
            {
                Users user = (Users)Session["user"];
                em = user.email;
                Logs lg = new Logs();

                lgList = lg.GetAllLogsOfUser(em);
            }
            else {
                Response.Redirect("homepage.aspx");
            }
           
        }
    }
}