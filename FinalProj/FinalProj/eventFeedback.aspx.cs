using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{
    public partial class eventFeedback : System.Web.UI.Page
    {
        protected List<Feedback> allFeedbackListByEventId;
        Feedback feedback = new Feedback();
        protected Sessionmg sesDeets;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("/homepage.aspx");
            }
            else {
                Sessionmg ses = new Sessionmg();
                blocked bl = new blocked();

                sesDeets = ses.GetSession(Session["email"].ToString());
                if (sesDeets.Active == 1)
                {
                }
                else {
                    if (bl.GetBlockedAccWithEmail(Session["email"].ToString()) != null)
                    {
                        Session.Clear();
                        string err = "SessionBanned";
                        Response.Redirect("homepage.aspx?error=" + err);
                    }
                }
            }

            int queriedEventId = Convert.ToInt32(Request.QueryString["eventId"]);
            Events events = new Events();

            Events selectedEvent = events.getEventDetails(queriedEventId);
            LblEventTitle.Text = selectedEvent.Title;


        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Feedback feedback = new Feedback();
                Events events = new Events();
                Users userS = (Users)Session["user"];

                string errorMsg = string.Empty;
                int eventId = Convert.ToInt32(Request.QueryString["eventId"]);
                //int userId = events.getEventDetails(eventId).User_id;          //  andy why u store the user id of the event organiser instead of the
                int userId = userS.id;                                           //  user id of the user who completed the feedback into the feedback
                string Q1Ratings = Q1Rating.CurrentRating.ToString();
                string Q2Ratings = Q2Rating.CurrentRating.ToString();
                string Q3Ratings = Q3Rating.CurrentRating.ToString();
                string Q4Ratings = Q4Rating.CurrentRating.ToString();
                int avgRating = 96;
                string FeedbackContent = tbFeedbackContent.Text;

                if (Q1Rating.CurrentRating.ToString() == "0")
                {
                    errorMsg += "Not all questions are rated ,";

                }
                else if (Q2Rating.CurrentRating.ToString() == "0")
                {
                    errorMsg += "Not all questions are rated ,";
                }
                else if (Q3Rating.CurrentRating.ToString() == "0")
                {
                    errorMsg += "Not all questions are rated ,";
                }
                else if (Q4Rating.CurrentRating.ToString() == "0")
                {
                    errorMsg += "Not all questions are rated ,";
                }


                //avgRating = (Int32.Parse("Q1Ratings") + Int32.Parse("Q2Ratings") + Int32.Parse("Q3Ratings") + Int32.Parse("Q4Ratings")) / 4;

                avgRating = (Convert.ToInt32(Q1Ratings) + Convert.ToInt32(Q2Ratings) + Convert.ToInt32(Q3Ratings) + Convert.ToInt32(Q4Ratings)) / 4;


                if (string.IsNullOrEmpty(tbFeedbackContent.Text))
                {
                    errorMsg += "Review field is empty";
                    tbFeedbackContent.Focus();
                }





                if (!string.IsNullOrEmpty(errorMsg))
                {
                    throw new Exception(errorMsg.TrimEnd(','));
                }
                else
                {
                    feedback = new Feedback(eventId, userId, avgRating, FeedbackContent, 1);
                    Attendance attendance = new Attendance();
                    Users user = (Users)Session["user"];
                    Users usr = new Users();
                    Events ev = new Events();

                    // I am using User session as the user id; diff from upstairs where i use from event detail;


                    int result = feedback.createFeedback();

                    if (result == 1)
                    {

                        // update user who gave feedback the points
                        usr.UpdatePointsByID(user.id, 1);
                        
                        //update 1 to Feedback in Attendance table by Event Id
                        attendance.UpdateFeedbackByUserIdEventId(user.id, eventId, 1);


                        //calculate avg points for the event
                        allFeedbackListByEventId = feedback.getAllFeedbacksByEventId(eventId);

                        int totalAvgRatings = 0;
                        int totalAvgRatingByEvent = 0;

                        for (int i = 0; i < allFeedbackListByEventId.Count; i++)
                        {
                            totalAvgRatings += allFeedbackListByEventId[i].AvgRating;
                        }

                        totalAvgRatingByEvent = totalAvgRatings / allFeedbackListByEventId.Count();

                        ev.updateAvgRatingByEventId(eventId, totalAvgRatingByEvent);


                        // update organiser points
                        int organiserUserId = ev.getEventDetails(eventId).User_id;

                        if (ev.getEventDetails(eventId).AverageRating > 2)
                        {
                            usr.UpdatePointsByID(organiserUserId, 2);
                        }
                        else
                        {
                            usr.UpdatePointsByID(organiserUserId, 1);
                        }


                        string script = "setTimeout(function() { swal ({ title: 'Feedback Submitted!', text: 'Your Feedback Goes a Long Way in Organising Meaningful Events!', type: 'success', confirmButtonText: 'OK'}, function(isConfirm) { if (isConfirm) { window.location.href = 'forumCatOverview.aspx'; } }); }, 1000);";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
                    }


                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>swal('Error!', '" + ex.Message + "!', 'error')</script>");

            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            tbFeedbackContent.Text = String.Empty;
            Q1Rating.CurrentRating = 0;
            Q2Rating.CurrentRating = 0;
            Q3Rating.CurrentRating = 0;
            Q4Rating.CurrentRating = 0;
        }
    }
}