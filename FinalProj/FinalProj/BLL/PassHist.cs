using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class PassHist
    {
        public string userEmail { get; set; }
        public string passHashHist { get; set; }
        public string userRegDate { get; set; }


        public PassHist() { }

        public PassHist(string email, string passHash, string dateTime)
        {
            userEmail = email;
            passHashHist = passHash;
            userRegDate = dateTime;
        }

        public int AddPass()
        {
            passHistDAO pass = new passHistDAO();
            int result = pass.Insert(this);
            return result;
        }

        public List<PassHist> getAllPassById(string email)
        {
            passHistDAO dao = new passHistDAO();
            return dao.getAllPassById(email);
        }

        public PassHist getLastPassByEmail(string email)
        {
            passHistDAO dao = new passHistDAO();
            return dao.GetLastPassById(email);
        }
    }
}