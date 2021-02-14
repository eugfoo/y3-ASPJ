using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using System.Collections;
using System.Data;

namespace FinalProj
{
    public partial class PPRating : System.Web.UI.Page
    {
        public string viewingUserId;
        public List<Feedback> feedbackList = new List<Feedback>();
        protected Sessionmg sesDeets;

        protected void Page_Load(object sender, EventArgs e)
        {
            viewingUserId = Request.QueryString["userId"];
            Users user = (Users)Session["user"];

            if (user != null)
            {
                Sessionmg ses = new Sessionmg();
                blocked bl = new blocked();

                sesDeets = ses.GetSession(Session["email"].ToString());
                if (sesDeets.Active == 1)
                {
                    if (viewingUserId != null)
                    {
                        loadDdlEvents(Convert.ToInt32(viewingUserId));
                    }
                    else
                    {
                        loadDdlEvents(user.id);
                    }
                }
                else
                {

                    if (bl.GetBlockedAccWithEmail(Session["email"].ToString()) != null)
                    {
                        Session.Clear();
                        string err = "SessionBanned";
                        Response.Redirect("homepage.aspx?error=" + err);
                    }
                }

            }
            else
            {
                loadDdlEvents(Convert.ToInt32(viewingUserId));
            }
            try
            {
                loadFeedback(Convert.ToInt32(ddlEvents.SelectedItem.Value));
                
            }
            catch { }

        }

        protected void ddlEvents_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedEventId = Convert.ToInt32(ddlEvents.SelectedItem.Value);
            loadFeedback(Convert.ToInt32(ddlEvents.SelectedItem.Value));
        }

        public void loadDdlEvents(int userId)
        {
            if (!Page.IsPostBack)
            {
                ddlEvents.DataSource = CreateDataSource(userId);
                ddlEvents.DataTextField = "EventTextField";
                ddlEvents.DataValueField = "EventValueField";
                ddlEvents.DataBind();
                ddlEvents.SelectedIndex = 0;
            }
        }

        public void loadFeedback(int eventId)
        {
            Feedback fdback = new Feedback();
            feedbackList = fdback.getAllFeedbacksByEventId(eventId);
            lbEvent.HRef = "/eventDetails.aspx?eventId=" + eventId;
        }

        ICollection CreateDataSource(int userId)
        {
            List<Events> ev = new Events().GetAllEventsCreatedByUser(userId);

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("EventTextField", typeof(String)));
            dt.Columns.Add(new DataColumn("EventValueField", typeof(int)));

            for (int i = 0; i < ev.Count; i++)
            {
                dt.Rows.Add(CreateRow(ev[i].Title, ev[i].EventId, dt));
            }

            DataView dv = new DataView(dt);
            return dv;

        }

        DataRow CreateRow(String Text, int Value, DataTable dt)
        {
            DataRow dr = dt.NewRow();

            dr[0] = Text;
            dr[1] = Value;

            return dr;

        }


    }
}