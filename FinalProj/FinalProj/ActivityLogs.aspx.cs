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
        protected roles cap;
        protected int capPerm = 0;
        protected DateTime dtStart;
        protected DateTime dtEnd;
        protected Sessionmg sesDeets;

        protected void Page_Load(object sender, EventArgs e)
        {


            if (Convert.ToBoolean(Session["admin"]) == true || Convert.ToBoolean(Session["subadmin"]) == true && Session["subadminEmail"] != null)
            {
                Sessionmg ses = new Sessionmg();
                if (Convert.ToBoolean(Session["subadmin"]))
                {
                    sesDeets = ses.GetSession(Session["subadminEmail"].ToString());
                }
                else
                {
                    sesDeets = ses.GetSession(Session["adminEmail"].ToString());

                }

                if (sesDeets.Active == 1)
                {
                    if (!Convert.ToBoolean(Session["admin"]))
                    {
                        string adEmail = Session["subadminEmail"].ToString();
                        Admins ad = new Admins();
                        Admins adDetails = ad.GetAllAdminWithEmail(adEmail);
                        roles rl = new roles();
                        cap = rl.GetRole(adDetails.adminRole);
                        capPerm = cap.viewAppLogs;
                    }

                    if (capPerm == 1 || Convert.ToBoolean(Session["admin"]) == true)
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
                        }
                        else
                        {
                            PanelError.Visible = false;
                            ActivityLog alg = new ActivityLog();
                            TimeSpan lgTime = new TimeSpan(23, 59, 59);

                            algList = alg.GetAllLogsOfActivities();
                            if (txtStartDate.Text != "" && txtEndDate.Text == "")
                            {
                                dtStart = Convert.ToDateTime(txtStartDate.Text);
                                dtEnd = DateTime.Now;
                            }
                            else if (txtStartDate.Text == "" && txtEndDate.Text != "")
                            {
                                dtStart = algList[algList.Count - 1].DateTime;
                                dtEnd = Convert.ToDateTime(txtEndDate.Text);
                                dtEnd = dtEnd.Add(lgTime);

                            }
                            else if (txtStartDate.Text != "" && txtEndDate.Text != "")
                            {
                                dtStart = Convert.ToDateTime(txtStartDate.Text);
                                dtEnd = Convert.ToDateTime(txtEndDate.Text);
                                dtEnd = dtEnd.Add(lgTime);

                            }

                            if (dtEnd >= dtStart)
                            {
                                if (dtStart != null && dtEnd != null && violationType.SelectedValue == "FailedAuthentication")
                                {
                                    foreach (var elmt in algList)
                                    {
                                        if (elmt.ViolationType == "Failed Authentication" && dtStart <= elmt.DateTime && elmt.DateTime <= dtEnd)
                                        {
                                            aalgList.Add(elmt);
                                        }

                                    }
                                }
                                else if (dtStart != null && dtEnd != null && violationType.SelectedValue == "All")
                                {
                                    foreach (var elmt in algList)
                                    {
                                        if (dtStart <= elmt.DateTime && elmt.DateTime <= dtEnd)
                                        {
                                            aalgList.Add(elmt);
                                        }
                                    }
                                }
                                else if (dtStart != null && dtEnd != null && violationType.SelectedValue == "Spamming")
                                {
                                    foreach (var elmt in algList)
                                    {
                                        if (elmt.ViolationType == "Spamming" && dtStart <= elmt.DateTime && elmt.DateTime <= dtEnd)
                                        {
                                            aalgList.Add(elmt);
                                        }
                                    }
                                }
                                else if (dtStart != null && dtEnd != null && violationType.SelectedValue == "XSS")
                                {
                                    foreach (var elmt in algList)
                                    {
                                        if (elmt.ViolationType == "XSS" && dtStart <= elmt.DateTime && elmt.DateTime <= dtEnd)
                                        {
                                            aalgList.Add(elmt);
                                        }
                                    }
                                }
                                else if (dtStart != null && dtEnd != null && violationType.SelectedValue == "Malware")
                                {
                                    foreach (var elmt in algList)
                                    {
                                        if (elmt.ViolationType == "Malware" && dtStart <= elmt.DateTime && elmt.DateTime <= dtEnd)
                                        {
                                            aalgList.Add(elmt);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                PanelError.Visible = true;
                                errmsgTb.Text = "Please select a valid date range";
                            }

                            foreach (var lement in aalgList)
                            {
                                Users us = new Users();

                                apicList.Add(us.GetUserByEmail(lement.userEmail).DPimage);
                            }
                        }
                    }
                    else
                    {

                        if (Convert.ToBoolean(Session["subadmin"]))
                        {
                            string err = "NoPermission";
                            Response.Redirect("homepage.aspx?error=" + err);
                        }
                        else
                        {
                            Response.Redirect("homepage.aspx");
                        }
                    }

                }
                else {
                    Session.Clear();
                    string err = "SessionKicked";
                    Response.Redirect("homepage.aspx?error=" + err);
                }

            }
            else // If a non-admin tries to access the page...
            {
                Response.Redirect("homepage.aspx"); // Adios Gladios
            }

        }
    }
}