using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class VoucherRedemption : System.Web.UI.Page
    {
        protected List<Voucher> vcherList;
        protected List<Voucher> vchers;
        public double points = 0;
        public List<decimal> pointCost = new List<decimal>();
        protected VoucherRedeemed vouchers;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null && Session["admin"] == null)
            {
                Response.Redirect("homepage.aspx");
            }
            else if (Session["admin"] != null)
            {
                Response.Redirect("AddVoucher.aspx");
            }
            else
            {
                Users user = (Users)Session["user"];
                points = user.points;
                Voucher vcher = new Voucher();
                vcherList = vcher.GetAllVouchersByName();

                if (vcherList.Count == 0)
                {
                    no.Visible = true;
                }

                VoucherRedeemed voucher = new VoucherRedeemed();
                vchers = vcher.GetVoucherById(Request.QueryString["voucherId"]);

                if (vchers != null)
                {
                    foreach (var element in vchers)
                    {
                        if (points >= double.Parse(element.VoucherPoints))
                        {
                            panelError.Visible = false;
                            voucher.VoucherId = int.Parse(Request.QueryString["voucherId"]);
                            voucher.VoucherAmount = element.VoucherAmount;
                            voucher.VoucherName = element.VoucherName;
                            voucher.VoucherPic = element.VoucherPic;
                            voucher.UserId = user.id.ToString();
                            voucher.Quantity = 1;

                            System.Diagnostics.Debug.WriteLine("This is voucherId: " + voucher.VoucherId);
                            voucher = new VoucherRedeemed(voucher.VoucherId, voucher.VoucherName, voucher.VoucherAmount, voucher.VoucherPic, voucher.UserId, voucher.Quantity);
                            int result = voucher.AddVoucher();

                            user.points = user.points - double.Parse(element.VoucherPoints);
                            user.UpdatePointsByID(user.id, user.points);

                            panelSuccess.Visible = true;

                            points = user.points;
                        }
                        else
                        {
                            panelSuccess.Visible = false;
                            panelError.Visible = true;
                        }

                    }
                }



            }
        }
    }
}
