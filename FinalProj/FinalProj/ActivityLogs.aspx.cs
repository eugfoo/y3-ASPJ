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
        protected List<ActivityLog> aalgList = new List<ActivityLog>();

        protected List<string> picList = new List<string>();
        protected List<string> apicList = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {

            
            if (Convert.ToBoolean(Session["admin"]) == true || Convert.ToBoolean(Session["subadmin"]) == true)
            {
                // Whatever you want here.
                if (!IsPostBack)
                {
                    ActivityLog alg = new ActivityLog();
                    algList = alg.GetAllLogsOfActivities();

                    for (int i = 0; i < algList.Count; i++)
                    {
                        Users us = new Users();
                        string he = algList[i].userEmail;
                        Users lol = us.GetUserByEmail(algList[i].userEmail);

                        picList.Add(us.GetUserByEmail(algList[i].userEmail).DPimage);

                    }
                }else {
                    ActivityLog alg = new ActivityLog();

                    algList = alg.GetAllLogsOfActivities();
                    if (violationType.SelectedValue == "FailedAuthentication")
                    {
                        foreach (var elmt in algList)
                        {
                            if (elmt.ViolationType == "Failed Authentication")
                            {
                                aalgList.Add(elmt);

                            }

                        }
                    }
                    else if (violationType.SelectedValue == "All") {
                        foreach (var elmt in algList)
                        {
                           
                            aalgList.Add(elmt);
                        }
                    }else if (violationType.SelectedValue == "Spamming")
                    {
                        foreach (var elmt in algList)
                        {
                            if (elmt.ViolationType == "Spamming")
                            {
                                aalgList.Add(elmt);
                            }
                        }
                    }
                    else if (violationType.SelectedValue == "XSS")
                    {
                        foreach (var elmt in algList)
                        {
                            if (elmt.ViolationType == "XSS")
                            {
                                aalgList.Add(elmt);
                            }
                        }
                    }
                    else if (violationType.SelectedValue == "Malware")
                    {
                        foreach (var elmt in algList)
                        {
                            if (elmt.ViolationType == "Malware")
                            {
                                aalgList.Add(elmt);
                            }
                        }
                    }

                    foreach (var lement in aalgList)
                    {
                        Users us = new Users();

                        apicList.Add(us.GetUserByEmail(lement.userEmail).DPimage);
                    }
                }
            }else if (!Convert.ToBoolean(Session["admin"])) // If a non-admin tries to access the page...
            {
                Response.Redirect("homepage.aspx"); // Adios Gladios
            }

        }

        //protected void violationType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ActivityLog alg = new ActivityLog();

        //    algList = alg.GetAllLogsOfActivities();
        //    if (violationType.SelectedValue == "Malware")
        //    {
        //        int ss = algList.Count();
        //        for (int i = 0; i < algList.Count; i++)
        //        {
        //            if (algList[i].ViolationType == "Malware")
        //            {
        //                aalgList.Add(algList[i]);
                        
        //            }

        //        }
        //    }
        //}
    }
}