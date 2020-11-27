using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{

    public partial class editForumPost : System.Web.UI.Page
    {
        public string threadImage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null) // A user has signed in
            {
                Response.Redirect("/homepage.aspx");
            }


            if (!Page.IsPostBack)
            {
                string threadId = Request.QueryString["threadid"];

                if (threadId != null)
                {
                    string ImageOne = "";
                    string ImageTwo = "";
                    string ImageThree = "";
                    string ImageFour = "";
                    Session["firstImage"] = "";
                    Session["secondImage"] = "";
                    Session["thirdImage"] = "";
                    Session["fourthImage"] = "";

                    Thread thread = new Thread();
                    Thread currentThread = thread.GetThreadByThreadIdWOEventId(int.Parse(threadId));

                    tbTitle.Text = currentThread.Title;
                    HFDate.Value = currentThread.Date;
                    tbContent.Text = currentThread.Content;
                    HFthreadId.Value = threadId;
                    ImageOne = currentThread.ThreadImage1;
                    ImageTwo = currentThread.ThreadImage2;
                    ImageThree = currentThread.ThreadImage3;
                    ImageFour = currentThread.ThreadImage4;



                    if (Session["LblPrefix"].ToString() == "[Discussion]")
                    {
                        DdlPrefix.SelectedIndex = 1;
                    }
                    if (Session["LblPrefix"].ToString() == "[Info]")
                    {
                        DdlPrefix.SelectedIndex = 2;
                    }
                    if (Session["LblPrefix"].ToString() == "[News]")
                    {
                        DdlPrefix.SelectedIndex = 3;
                    }
                    if (Session["LblPrefix"].ToString() == "[Help]")
                    {
                        DdlPrefix.SelectedIndex = 4;
                    }
                    if (Session["LblPrefix"].ToString() == "[Request]")
                    {
                        DdlPrefix.SelectedIndex = 1;
                    }

                    //Populating pictures;

                    if (ImageOne == "")
                    {
                        Image1.Style.Add("display", "none");

                    }
                    else
                    {
                        Session["firstImage"] = ImageOne;
                        Image1.ImageUrl = "~/Img/" + ImageOne;
                        Image1.Style.Add("display", "block");
                    }

                    if (ImageTwo == "")
                    {
                        Image2.Style.Add("display", "none");

                    }
                    else
                    {
                        Session["secondImage"] = ImageTwo;
                        Image2.ImageUrl = "~/Img/" + ImageTwo;
                        Image2.Style.Add("display", "block");
                    }

                    if (ImageThree == "")
                    {
                        Image3.Style.Add("display", "none");

                    }
                    else
                    {
                        Session["thirdImage"] = ImageThree;
                        Image3.ImageUrl = "~/Img/" + ImageThree;
                        Image3.Style.Add("display", "block");
                    }

                    if (ImageFour == "")
                    {
                        Image4.Style.Add("display", "none");

                    }
                    else
                    {
                        Session["fourthImage"] = ImageFour;
                        Image4.ImageUrl = "~/Img/" + ImageFour;
                        Image4.Style.Add("display", "block");
                    }
                }

            }
        }

        private bool ValidateInput()
        {
            LblMsg.Text = String.Empty;

            if (DdlPrefix.SelectedIndex == 0)
            {
                LblMsg.Text += "Please Select a Prefix! <br/>";
                LblMsg.ForeColor = Color.Red;
            }

            if (tbTitle.Text == "")
            {
                LblMsg.Text += "Title is required! <br/>";
                LblMsg.ForeColor = Color.Red;
            }

            if (String.IsNullOrEmpty(tbContent.Text))
            {
                LblMsg.Text += "Please fill in the content! <br/>";
                LblMsg.ForeColor = Color.Red;
            }

            if (String.IsNullOrEmpty(LblMsg.Text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string BadgeColorIdentifier()
        {
            string badgeColorClass = "";
            if (DdlPrefix.SelectedIndex == 1)
            {
                badgeColorClass = "warning";
            }
            else if (DdlPrefix.SelectedIndex == 2)
            {
                badgeColorClass = "info";
            }
            else if (DdlPrefix.SelectedIndex == 3)
            {
                badgeColorClass = "primary";
            }
            else if (DdlPrefix.SelectedIndex == 4)
            {
                badgeColorClass = "danger";
            }
            else if (DdlPrefix.SelectedIndex == 5)
            {
                badgeColorClass = "secondary";
            }

            return badgeColorClass;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Users user = (Users)Session["user"];
            Thread thread = new Thread();
            string firstImage = "";
            string secondImage = "";
            string thirdImage = "";
            string fourthImage = "";
            int user_id = user.id;
            string user_name = user.name;

            if (ValidateInput())
            {

                DateTime now = DateTime.Now;
                HFDate.Value = now.ToString("g");
                DateTime mDate = Convert.ToDateTime(HFDate.Value);


                thread = new Thread(DdlPrefix.Text, BadgeColorIdentifier(), tbTitle.Text, HFDate.Value,
                    firstImage, secondImage, thirdImage, fourthImage,
                    tbContent.Text, user_id, user_name);

                int result = thread.UpdateThread(Convert.ToInt32(HFthreadId.Value));

                if (result == 1)
                {
                    Response.Redirect("forumPost.aspx?threadid=" + Session["threadId"]);
                }
            }
        }
    }

}