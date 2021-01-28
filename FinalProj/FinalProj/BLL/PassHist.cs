using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class PassHist
    {
        public int userID { get; set; }
        public string passHashHist { get; set; }

        public PassHist() { }

        public PassHist(int uID, string passHash)
        {
            userID = uID;
            passHashHist = passHash;
        }

        public int AddPass()
        {
            passHistDAO pass = new passHistDAO();
            int result = pass.Insert(this);
            return result;
        }

        public List<PassHist> getAllPassById(int id)
        {
            passHistDAO dao = new passHistDAO();
            return dao.getAllPassById(id);
        }
    }
}