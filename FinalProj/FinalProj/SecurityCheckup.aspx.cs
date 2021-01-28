using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class SecurityCheckup : System.Web.UI.Page
    {
        int recentCounter;
        string recentSecurity = "<br/>";

        protected void Page_Load(object sender, EventArgs e)
        {
            loadPass();
            if (recentCounter >= 1 || recentCounter == 0)
            {
                lblRecentEvents.Text = "<strong>" + recentCounter + "</strong> recent events";
            }
            else
            {
                lblRecentEvents.Text = "<strong>" + recentCounter + "</strong> recent event";
            }
            lblActions.Text = recentSecurity;
        }

        protected void loadPass()
        {
            Users user = (Users)Session["user"];
            PassHist pass = new PassHist();
            
            PassHist ps = pass.getLastPassByEmail(user.email);

            DateTime now = DateTime.Now;
            string DTNow = now.ToString("g");
            string DTReg = ps.userRegDate;

            TimeSpan span = (Convert.ToDateTime(DTNow) - Convert.ToDateTime(DTReg));
            string spantime = String.Format("<strong>{0} Days {1} Hours {2} Minutes</strong>", span.Days, span.Hours, span.Minutes);
            lblPassword.Text = spantime;
            TimeSpan spanBase = new TimeSpan(14,0,0,0);
            if (span < spanBase)
            {
                recentCounter++;
                recentSecurity += "You recently changed your password <br/>";
            }

            ActivityLog log = new ActivityLog();
            ActivityLog logo = log.getLastLogByEmail(user.email);

            DateTime DTRec = logo.DateTime;
            TimeSpan spanRecent = (Convert.ToDateTime(DTNow) - DTRec);
            if (spanRecent < spanBase)
            {
                recentCounter++;
                recentSecurity += "You recently logged a "+ logo.Action +"<br/>";
            }
        }
    }
}