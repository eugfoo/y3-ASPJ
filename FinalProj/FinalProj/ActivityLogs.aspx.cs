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

        protected void Page_Load(object sender, EventArgs e)
        {
            ActivityLog alg = new ActivityLog();
            algList = alg.GetAllLogsOfActivities();

        }
    }
}