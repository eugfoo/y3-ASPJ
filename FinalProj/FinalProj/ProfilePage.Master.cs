using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class ProfilePage : System.Web.UI.MasterPage
    {
        protected List<Notifications> notiListTemp;
        protected List<Notifications> notiList = new List<Notifications>();
        public int rating;
        public string viewingUserId;
        public int notiCount = 0;
        public int count = 0;
        public Users viewingUser;
        public userProfilePrivacy userPriv = new userProfilePrivacy();
        protected void Page_Load(object sender, EventArgs e)
        {
            viewingUserId = Request.QueryString["userId"];
            Notifications noti = new Notifications();
            

            if (viewingUserId != null) // A user is viewing another's PP
                if (userPriv.userProfileIsPublic == 1)// checking for profile privacy
                {
                    if (Session["user"] == null) // User is not signed in
                    {
                        ddCaret.Visible = false;
                        ddMenu.Visible = false;
                        lblProfile.Text = "Sign In";
                        lblProfile.NavigateUrl = "/LogIn.aspx";
                        liLogOut.Visible = false;
                        lblBookmark.Visible = false;
                    }
                    else
                    {
                        Users user1 = (Users)Session["user"];
                        user1.UpdateRatingByID(user1.id);
                        if (user1.isOrg.Trim() == "True")
                        {
                            norgItems.Visible = false;
                        }
                        lblProfile.Text = user1.name;
                        liLogOut.Visible = true;
                        lblBookmark.Visible = true;
                        hlFacebook.Visible = false;
                        HyperLink1.Visible = false;
                        hlInstagram.Visible = false;
                        HyperLink2.Visible = false;
                        hlTwitter.Visible = false;
                        HyperLink3.Visible = false;
                        lblUserName.Visible = false;
                        lblRating.Visible = false;
                        lblEventCount.Visible = false;
                        lblDesc.Visible = false;
                        ddlProfileVisibility.Visible = false;
                        showPrivate.Visible = true;
                        lblProfile.Text = user1.name;
                        liLogOut.Visible = false;
                        lblBookmark.Visible = false;
                    }
                    linkPPPoints.Visible = false;
                    Users user = new Users();
                    user.UpdateRatingByID(Convert.ToInt32(viewingUserId));
                    var viewingUser = user.GetUserById(Convert.ToInt32(viewingUserId));
                    initializePPFields(viewingUser);
                }
                else
                {

                    Users user1 = (Users)Session["user"];
                    user1.UpdateRatingByID(user1.id);
                    if (user1.isOrg.Trim() == "True")
                    {
                        norgItems.Visible = false;
                    }
                    lblProfile.Text = user1.name;
                    liLogOut.Visible = true;
                    lblBookmark.Visible = true;
                    hlFacebook.Visible = true;
                    HyperLink1.Visible = true;
                    hlInstagram.Visible = true;
                    HyperLink2.Visible = true;
                    hlTwitter.Visible = true;
                    HyperLink3.Visible = true;
                    lblUserName.Visible = true;
                    lblRating.Visible = true;
                    lblEventCount.Visible = true;
                    lblDesc.Visible = true;
                    ddlProfileVisibility.Visible = false;
                    showPrivate.Visible = false;
                    lblProfile.Text = user1.name;
                    liLogOut.Visible = true;
                    lblBookmark.Visible = true;

                }
          
            else if (Session["user"] != null) // A user is viewing their own PP
            {
                Users user = (Users)Session["user"];
                userProfilePrivacy profilePriv = new userProfilePrivacy();
                if (user.isOrg.Trim() == "True")
                {
                    norgItems.Visible = false;
                }
                initializePPFields(user);
                linkPPPoints.Visible = true;
                lblProfile.Text = user.name;
                liLogOut.Visible = true;
                lblBookmark.Visible = true;
                showPrivate.Visible = false;

                notiListTemp = noti.GetEventsEnded();
                System.Diagnostics.Debug.WriteLine("This is notiListTemp: " + notiListTemp);

                for (int i = 0; i < notiListTemp.Count; i++)
                {
                    if (notiListTemp[i].User_id == user.id)
                    {
                        count += 1;
                        notiCount += 1;
                        notiList.Add(notiListTemp[i]);
                        // System.Diagnostics.Debug.WriteLine("This is notiList" + notiList[i]);
                    }
                }
               


            }

            else
            {
                ddCaret.Visible = false;
                ddMenu.Visible = false;
                lblProfile.Text = "Sign In";
                lblProfile.NavigateUrl = "/LogIn.aspx";
                liLogOut.Visible = false;
                lblBookmark.Visible = false;
                Response.Redirect("homepage.aspx");
            }
        }

        protected void lblLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("/homepage.aspx");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            Users user = (Users)Session["user"];
            user.VerifyOrgByEmail(user.email);
        }
        protected void ddlProfileVisibility_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedValue = Convert.ToInt32(ddlProfileVisibility.SelectedItem.Value);
            if (selectedValue != -1)
            {
                userProfilePrivacy profilePriv = new userProfilePrivacy();
                int selectedEventId = Convert.ToInt32(ddlProfileVisibility.SelectedItem.Value);
                int result = profilePriv.UpdateProfileVisibilityByID(profilePriv.userProfileID, Convert.ToInt32(selectedValue));
            }

        }
        public void initializePPFields(Users userI)
        {
            if (userI == null)
            {
                Response.Redirect("homepage.aspx");
            }
            else
            {
                userI.UpdateRatingByID(userI.id);
                Users user = userI.GetUserById(userI.id);
                Users sUser = (Users)Session["user"];
                if (sUser != null && user.id == sUser.id) // i wanted to comment an explanation but was too lazy halfway.
                {
                    Session["user"] = user;
                }

                EventsStatus events = new EventsStatus();
                var eventList = events.GetAllEventsByName();
                int eventCount = 0;

                for (int i = 0; i < eventList.Count; i++)
                {
                    if (eventList[i].Organiser == user.id.ToString())
                    {
                        eventCount++;
                    }
                }

                if (user.verified == 1)
                {
                    bluetick.Visible = true;
                }

                if (eventCount == 0)
                {
                    lblEventCount.Visible = false;
                }
                else
                {
                    lblEventCount.Text = "(" + eventCount.ToString() + ")";
                }

                lblUserName.Text = user.name;
                if (user.facebook != "") { hlFacebook.NavigateUrl = user.facebook; }
                if (user.instagram != "") { hlInstagram.NavigateUrl = user.instagram; }
                if (user.twitter != "") { hlTwitter.NavigateUrl = user.twitter; }
                hlInstagram.NavigateUrl = user.instagram;
                hlTwitter.NavigateUrl = user.twitter;
                imgDP.ImageUrl = user.DPimage;
                imgBP.ImageUrl = user.BPimage;
                if (user.desc != "") { lblDesc.Text = user.desc; } else { lblDesc.Text = "This user has not added any description."; lblDesc.CssClass += "text-muted font-italic"; }
                rating = user.rating;
            }
        }
    }
}