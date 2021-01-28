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

        public PassHist() { }

        public PassHist(string email, string passHash)
        {
            userEmail = email;
            passHashHist = passHash;
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
    }
}