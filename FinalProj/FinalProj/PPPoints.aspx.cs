using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class PPPoints : System.Web.UI.Page
    {
        public List<VoucherRedeemed> vRedList = new List<VoucherRedeemed>();
        protected Sessionmg sesDeets;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Sessionmg ses = new Sessionmg();
                blocked bl = new blocked();

                sesDeets = ses.GetSession(Session["email"].ToString());
                if (sesDeets.Active == 1)
                {
                    VoucherRedeemed vRed = new VoucherRedeemed();
                    Users user = (Users)Session["user"];

                    vRedList = vRed.GetAllByUserId(user.id.ToString());
                    lblPoints.Text = user.points.ToString();
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
        }
    }
}