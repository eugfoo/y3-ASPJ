using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AvScan.AVG;
using FinalProj.BLL;
using Nerdicus.VirusTotalNET;
using AvScan.WindowsDefender;

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

                var exeLocation = "C://Program Files//Windows Defender//MpCmdRun.exe";
                var scanner = new WindowsDefenderScanner(exeLocation);
                var result = scanner.Scan("C://Users//Eugene Foo//Documents//Digital Forensics//eicar.com.txt");
                Label1.Text = result.ToString();

            }
            else {

                Response.Redirect("homepage.aspx");
            }

        }



        
        
    }
}