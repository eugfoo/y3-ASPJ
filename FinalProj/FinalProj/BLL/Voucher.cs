using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class Voucher
    {
        // Define class properties
        public int VoucherId { get; set; }
        public string VoucherName { get; set; }
        public string VoucherPic { get; set; }
        public string VoucherAmount { get; set; }
        public string VoucherPoints { get; set; }

        public Voucher()
        {

        }

        // Define a constructor to initialize all properties
        public Voucher(int voucherId, string voucherName, string voucherAmount, string voucherPic, string voucherPoints)
        {
            VoucherId = voucherId;
            VoucherName = voucherName;
            VoucherPic = voucherPic;
            VoucherAmount = voucherAmount;
            VoucherPoints = voucherPoints;
        }

        public int AddVoucher()
        {
            voucherDAO dao = new voucherDAO();
            int result = dao.Insert(this);
            return result;
        }

        public List<Voucher> GetAllVouchersByName()
        {
            voucherDAO voucher = new voucherDAO();
            return voucher.SelectAllByName();
        }

        public List<Voucher> GetVoucherById(string queryId)
        {
            voucherDAO voucher = new voucherDAO();
            return voucher.SelectById(queryId);
        }

        public int DeleteVoucherById(int voucherId)
        {
            voucherDAO voucher = new voucherDAO();
            return voucher.Delete(voucherId);
        }
    }
}