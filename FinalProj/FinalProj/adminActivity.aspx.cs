﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class WebForm6 : System.Web.UI.Page
    {
        protected List<adminLog> admlgList;
        protected List<adminLog> aadmlgList = new List<adminLog>();

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
                        capPerm = cap.mgAdLg;
                    }

                    if (capPerm == 1 || Convert.ToBoolean(Session["admin"]) == true)
                    {
                        // Whatever you want here.
                        if (!IsPostBack)
                        {
                            adminLog admlg = new adminLog();
                            admlgList = admlg.GetAllAdminLogs();
                        }
                        else
                        {
                            PanelError.Visible = false;
                            adminLog admlg = new adminLog();
                            TimeSpan lgTime = new TimeSpan(23, 59, 59);

                            admlgList = admlg.GetAllAdminLogs();
                            if (txtStartDate.Text != "" && txtEndDate.Text == "")
                            {
                                dtStart = Convert.ToDateTime(txtStartDate.Text);
                                dtEnd = DateTime.Now;
                            }
                            else if (txtStartDate.Text == "" && txtEndDate.Text != "")
                            {
                                dtStart = admlgList[admlgList.Count - 1].DateTime;
                                dtEnd = Convert.ToDateTime(txtEndDate.Text);
                                dtEnd = dtEnd.Add(lgTime);

                            }
                            else if (txtStartDate.Text != "" && txtEndDate.Text != "")
                            {
                                dtStart = Convert.ToDateTime(txtStartDate.Text);
                                dtEnd = Convert.ToDateTime(txtEndDate.Text);
                                dtEnd = dtEnd.Add(lgTime);

                            }

                            if (txtStartDate.Text == "" && txtEndDate.Text == "")
                            {
                                if (violationType.SelectedValue == "FailedAuthentication")
                                {
                                    foreach (var elmt in admlgList)
                                    {
                                        if (elmt.ViolationType == "Failed Authentication")
                                        {
                                            aadmlgList.Add(elmt);
                                        }

                                    }
                                }
                                else if (violationType.SelectedValue == "All")
                                {
                                    foreach (var elmt in admlgList)
                                    {

                                        aadmlgList.Add(elmt);

                                    }
                                }
                                else if (violationType.SelectedValue == "XSS")
                                {
                                    foreach (var elmt in admlgList)
                                    {
                                        if (elmt.ViolationType == "XSS")
                                        {
                                            aadmlgList.Add(elmt);
                                        }
                                    }
                                }
                                
                            }
                            else if (dtEnd >= dtStart)
                            {
                                if (dtStart != null && dtEnd != null && violationType.SelectedValue == "FailedAuthentication")
                                {
                                    foreach (var elmt in admlgList)
                                    {
                                        if (elmt.ViolationType == "Failed Authentication" && dtStart <= elmt.DateTime && elmt.DateTime <= dtEnd)
                                        {
                                            aadmlgList.Add(elmt);
                                        }

                                    }
                                }
                                else if (dtStart != null && dtEnd != null && violationType.SelectedValue == "All")
                                {
                                    foreach (var elmt in admlgList)
                                    {
                                        if (dtStart <= elmt.DateTime && elmt.DateTime <= dtEnd)
                                        {
                                            aadmlgList.Add(elmt);
                                        }
                                    }
                                }
                                else if (dtStart != null && dtEnd != null && violationType.SelectedValue == "XSS")
                                {
                                    foreach (var elmt in admlgList)
                                    {
                                        if (elmt.ViolationType == "XSS" && dtStart <= elmt.DateTime && elmt.DateTime <= dtEnd)
                                        {
                                            aadmlgList.Add(elmt);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                PanelError.Visible = true;
                                errmsgTb.Text = "Please select a valid date range";
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
                else
                {
                    blocked bl = new blocked();
                    if (bl.GetBlockedAccWithEmail(Session["subadminEmail"].ToString()) != null)
                    {
                        Session.Clear();
                        string err = "SessionBanned";
                        Response.Redirect("homepage.aspx?error=" + err);
                    }
                    else
                    {
                        Session.Clear();
                        string err = "SessionKicked";
                        Response.Redirect("homepage.aspx?error=" + err);
                    }
                }

            }
            else // If a non-admin tries to access the page...
            {
                Response.Redirect("homepage.aspx"); // Adios Gladios
            }
        }
    }
}