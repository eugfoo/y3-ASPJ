using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.DAL;

namespace FinalProj.BLL
{
    public class RequestPermission
    {
        public int requestId { get; set; }
        public string subAdminEmail { get; set; }
        public string requestRole { get; set; }
        public int requestStatus { get; set; }

        public RequestPermission() { }
        public RequestPermission(int id, string email, string role, int status)
        {
            requestId = id;
            subAdminEmail = email;
            requestRole = role;
            requestStatus = status;
        }

        public int AddRequest(string email, string role)
        {
            RequestPermissionDAO reqDAO = new RequestPermissionDAO();
            return reqDAO.Insert(email, role);
        }

        public RequestPermission GetLastRequest(string email)
        {
            RequestPermissionDAO ad = new RequestPermissionDAO();
            return ad.getLastRequest(email);
        }

        public List<RequestPermission> getAllRequests(string email)
        {
            RequestPermissionDAO dao = new RequestPermissionDAO();
            return dao.getAllRequestByEmail(email);
        }

        public int DeleteByIdEmail(int id, string email)
        {
            RequestPermissionDAO reqDAO = new RequestPermissionDAO();
            return reqDAO.deleteRequest(id, email);
        }
    }
}