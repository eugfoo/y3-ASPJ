using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class GPictures
    {
        public int id { get; set; }
        public string filepath { get; set; }
        public int user { get; set; }
        public string caption { get; set; }
        public int gpevent { get; set; }
        public DateTime date { get; set; }
        public GPictures() { }

        public GPictures(string uGPFilepath, int uGPUser, string uGPCaption, int uGPEvent, DateTime uGPDate)
        {
            filepath = uGPFilepath;
            user = uGPUser;
            caption = uGPCaption;
            gpevent = uGPEvent;
            date = uGPDate;
        }

        public GPictures(int uId, string uGPFilepath, int uGPUser, string uGPCaption, int uGPEvent, DateTime uGPDate)
        {
            id = uId;
            filepath = uGPFilepath;
            user = uGPUser;
            caption = uGPCaption;
            gpevent = uGPEvent;
            date = uGPDate;
        }

        public int addGP()
        {
            gpictureDAO gp = new gpictureDAO();
            return gp.Insert(this);
        }

        public List<GPictures> getAllByUserId(int id)
        {
            gpictureDAO gp = new gpictureDAO();
            return gp.SelectAllByUserId(id);
        }

    }
}