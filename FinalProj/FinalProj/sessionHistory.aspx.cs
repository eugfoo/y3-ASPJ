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
            Logs lg = new Logs();

            lgList = lg.GetAllLogsOfUser("123@gmail.com"); 
        }
    }
}