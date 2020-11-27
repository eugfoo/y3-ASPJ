using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class VoucherRedeemed
    {
        // Define class properties
        public int VoucherId { get; set; }
        public string VoucherName { get; set; }
        public string VoucherPic { get; set; }
        public string VoucherAmount { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }

        public VoucherRedeemed()
        {

        }

        // Define a constructor to initialize all properties
        public VoucherRedeemed(int voucherId, string voucherName, string voucherAmount, string voucherPic, string userId, int quantity)
        {
            VoucherId = voucherId;
            VoucherName = voucherName;
            VoucherPic = voucherPic;
            VoucherAmount = voucherAmount;
            UserId = userId;
            Quantity = quantity;
        }

        public List<VoucherRedeemed> GetAllVouchersByName()
        {
            voucherRedeemDAO voucher = new voucherRedeemDAO();
            return voucher.SelectAllByName();
        }

        public List<VoucherRedeemed> GetAllByUserId(string userId)
        {
            voucherRedeemDAO voucher = new voucherRedeemDAO();
            return voucher.SelectAllByUserId(userId);
        }

        public int AddVoucher()
        {
            voucherRedeemDAO dao = new voucherRedeemDAO();
            int result = dao.Insert(this);
            return result;
        }
    }
}