using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace FinalProj
{
    public partial class AddVoucher : System.Web.UI.Page
    {
        protected List<Voucher> vcherList;
        protected int vchers;
        protected VoucherRedeemed vouchers;
        protected Sessionmg sesDeets;
        protected roles cap;
        protected int capPerm = 0;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (Convert.ToBoolean(Session["admin"]) == true || Convert.ToBoolean(Session["subadmin"]) == true)
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
                    Admins ad = new Admins();
                    roles rl = new roles();
                    if (!Convert.ToBoolean(Session["admin"]))
                    {
                        string adEmail = Session["subadminEmail"].ToString();
                        Admins adDetails = ad.GetAllAdminWithEmail(adEmail);
                        cap = rl.GetRole(adDetails.adminRole);
                        capPerm = cap.mgVouch;
                    }
                    if (capPerm == 1 || Convert.ToBoolean(Session["admin"]) == true)
                    {

                        Voucher vcher = new Voucher();
                        vcherList = vcher.GetAllVouchersByName();

                        if (vcherList.Count == 0)
                        {
                            no.Visible = true;
                        }

                        if (Request.QueryString["voucherId"] != null)
                        {
                            vchers = vcher.DeleteVoucherById(int.Parse(Request.QueryString["voucherId"]));
                            panelSuccess.Visible = true;
                            vcherList = vcher.GetAllVouchersByName();
                            if (vcherList.Count == 0)
                            {
                                no.Visible = true;
                            }
                        }
                    }
                    else {
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
                }else{
                    Session.Clear();
                    string err = "SessionKicked";
                    Response.Redirect("homepage.aspx?error=" + err);
                }
            }else{
                Response.Redirect("homepage.aspx");
            }
        }

        protected void createBtn_Click(object sender, EventArgs e)
        {
            Voucher vcher = new Voucher();
            string errmsg = "";
            panelSuccess.Visible = false;
            PanelError.Visible = false;
            int num = -1;

            if (voucherName.Text == "")
            {
                errmsg = errmsg + "Voucher Name cannot be empty! <br>";
                voucherName.BorderColor = System.Drawing.Color.Red;
            }
            else
            {
                voucherName.BorderColor = System.Drawing.Color.LightGray;
            }

            if (voucherAmt.Text == "" || !int.TryParse(voucherAmt.Text, out num))
            {
                errmsg = errmsg + "Voucher Amount must be an integer and cannot be empty! <br>";
                voucherAmt.BorderColor = System.Drawing.Color.Red;
            }
            else
            {
                voucherAmt.BorderColor = System.Drawing.Color.LightGray;
            }

            if (FileUpload1.HasFile == false)
            {
                errmsg = errmsg + "Select an image for voucher! <br>";
            }


            if (errmsg != "")
            {
                errmsgTb.Text = errmsg;
                PanelError.Visible = true;
            }

            else
            {
                string vcherName = voucherName.Text.ToString();
                string vcherAmt = voucherAmt.Text.ToString();
                string file = "";
                string vcherPoint = (double.Parse(vcherAmt) * 0.5).ToString();

                if (FileUpload1.HasFile)
                {
                    string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    FileUpload1.SaveAs(Server.MapPath("~/Img/" + filename));
                    file = filename;

                }

                vcher = new Voucher(1, vcherName, vcherAmt, file, vcherPoint);
                int result = vcher.AddVoucher();

                no.Visible = false;

                voucherName.Text = "";
                voucherAmt.Text = "";
                vcherList = vcher.GetAllVouchersByName();
            }
        }
    }
}
