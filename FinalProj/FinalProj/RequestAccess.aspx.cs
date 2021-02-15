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

        protected void Page_Load(object sender, EventArgs e)
        {
            List<RequestPermission> listReq = new List<RequestPermission>();
            if (!IsPostBack)
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
                        gvAdmin.DataBind();
                    }
                    else
                    {
                        adminDiv.Visible = false;
                        ad = ad.GetAllAdminWithEmail(Session["subadminEmail"].ToString());
                        lblCurrentRole.Text = ad.adminRole;
                        listReq = req.getAllRequestsEmail(ad.adminEmail);
                        btnRequest.Enabled = false;
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
        }

        protected void roleDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (roleDDL.SelectedItem.Text == lblCurrentRole.Text)
            {
                lblError.Visible = true;
                lblError.Text = "This is already your role!";
                btnRequest.Enabled = false;
            }
            if (roleDDL.SelectedItem.Text == "Select a Role")
            {
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
                ad = ad.GetAllAdminWithEmail(Session["subadminEmail"].ToString());

                RequestPermission request = new RequestPermission();
                request.AddRequest(ad.adminEmail, roleDDL.SelectedItem.Text, ad.adminRole);
                Response.Redirect("/RequestAccess.aspx");
            }

        }

        protected void btnVerify_Click(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvAdmin.Rows[index];

            string email = row.Cells[0].Text;
            string role = row.Cells[1].Text;

            ad.UpdateRoleByEmail(email, role);
            req.DeleteByIdEmail(email);
            Response.Redirect("/RequestAccess.aspx");
        }
    }
}