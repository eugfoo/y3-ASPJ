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
        public string currentRole { get; set; }
        public int requestStatus { get; set; }

        public RequestPermission() { }
        public RequestPermission(int id, string email, string role, string currentRol, int status)
        {
            requestId = id;
            subAdminEmail = email;
            requestRole = role;
            currentRole = currentRol;
            requestStatus = status;
        }

        public int AddRequest(string email, string role, string currentRole)
        {
            RequestPermissionDAO reqDAO = new RequestPermissionDAO();
            return reqDAO.Insert(email, role, currentRole);
        }

        public RequestPermission GetLastRequest(string email)
        {
            RequestPermissionDAO ad = new RequestPermissionDAO();
            return ad.getLastRequest(email);
        }

        public List<RequestPermission> getAllRequests()
        {
            RequestPermissionDAO dao = new RequestPermissionDAO();
            return dao.getAllRequest();
        }

        public List<RequestPermission> getAllRequestsEmail(string email)
        {
            RequestPermissionDAO dao = new RequestPermissionDAO();
            return dao.getAllRequestByEmail(email);
        }

        public int DeleteByIdEmail(string email)
        {
            RequestPermissionDAO reqDAO = new RequestPermissionDAO();
            return reqDAO.deleteRequest(email);
        }
    }
}