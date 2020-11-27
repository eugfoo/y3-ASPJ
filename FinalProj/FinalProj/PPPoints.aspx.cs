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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                VoucherRedeemed vRed = new VoucherRedeemed();
                Users user = (Users)Session["user"];

                vRedList = vRed.GetAllByUserId(user.id.ToString());
                lblPoints.Text = user.points.ToString();
            }
        }
    }
}