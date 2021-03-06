﻿using System;
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
        protected List<MainAdmins> maList;
        protected Sessionmg sesDeets;



        protected void Page_Load(object sender, EventArgs e)
        {


        string em;
            if (Session["user"] != null)
            {
                Sessionmg ses = new Sessionmg();
                blocked bl = new blocked();

                sesDeets = ses.GetSession(Session["email"].ToString());
                if (sesDeets.Active == 1)
                {
                    Users user = (Users)Session["user"];
                    em = user.email;
                    Logs lg = new Logs();

                    lgList = lg.GetAllLogsOfUser(em);
                }
                else
                {
                    if (bl.GetBlockedAccWithEmail(Session["email"].ToString()) != null)
                    {
                        Session.Clear();
                        string err = "SessionBanned";
                        Response.Redirect("homepage.aspx?error=" + err);
                    }
                }
            }
            else if (Convert.ToBoolean(Session["subadmin"])) {
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
                    Users user = (Users)Session["user"];
                    em = Session["subadminEmail"].ToString();
                    Logs lg = new Logs();

                    lgList = lg.GetAllLogsOfUser(em);
                }
                else {
                    Session.Clear();
                    string err = "SessionKicked";
                    Response.Redirect("homepage.aspx?error=" + err);
                }

            }else if (Convert.ToBoolean(Session["admin"]))
            {
                MainAdmins ma = new MainAdmins();
                maList = ma.SelectAllMainAdmins();

                Logs lg = new Logs();
                lgList = lg.GetAllLogsOfUser(maList[0].MainadminEmail);
            }
            else
            {

                Response.Redirect("homepage.aspx");
            }

        }

    }
}