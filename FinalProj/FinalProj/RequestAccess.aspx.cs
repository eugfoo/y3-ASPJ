using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class RequestAccess : System.Web.UI.Page
    {
        Admins ad = new Admins();
        RequestPermission req = new RequestPermission();
        List<RequestPermission> listReq = new List<RequestPermission>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["subadmin"] == null && Session["admin"] == null)
            {
                Response.Redirect("/homepage.aspx");
            }
            else
            {
                if (Session["admin"] != null)
                {
                    subAdminDiv.Visible = false;
                    listReq = req.getAllRequests();
                    gvAdmin.DataSource = listReq;
                }
                else
                {
                    adminDiv.Visible = false;
                    ad = ad.GetAllAdminWithEmail(Session["subadminEmail"].ToString());
                    lblCurrentRole.Text = ad.adminRole;
                    listReq = req.getAllRequestsEmail(ad.adminEmail);
                    foreach (var i in listReq)
                    {
                        if (i.subAdminEmail == ad.adminEmail)
                        {
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Your request has been sent for review";
                            btnRequest.Enabled = false;
                            roleDDL.Enabled = false;
                        }
                    }
                }
            }
        }

        protected void roleDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (roleDDL.SelectedItem.Text == lblCurrentRole.Text)
            {
                lblError.Visible = true;
                lblError.Text = "This is already your role!";
                btnRequest.Enabled = false;
            }
            else
            {
                btnRequest.Enabled = true;
                lblError.Visible = false;
            }
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            if (roleDDL.SelectedItem.Text == lblCurrentRole.Text)
            {
                lblError.Visible = true;
                lblError.Text = "This is already your role!";
                btnRequest.Enabled = false;
            }
            else
            {
                RequestPermission request = new RequestPermission();
                request.AddRequest(ad.adminEmail, roleDDL.SelectedItem.Text);
                Response.Redirect("/RequestAccess.aspx");
            }

        }
    }
}