using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using System.Net.Http;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;


namespace FinalProj
{
    public partial class Personal : System.Web.UI.Page
    {
        string OTPassword = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null) // A user has signed in
            {
                Response.Redirect("/homepage.aspx");
            }
        }

        private static readonly HttpClient client = new HttpClient();


        protected void changetoDefaultBorder()
        {
            eventTitle.BorderColor = System.Drawing.Color.LightGray;
            eventAddress.BorderColor = System.Drawing.Color.LightGray;
            eventDate.BorderColor = System.Drawing.Color.LightGray;
            startTime.BorderColor = System.Drawing.Color.LightGray;
            endTime.BorderColor = System.Drawing.Color.LightGray;
            maxAttend.BorderColor = System.Drawing.Color.LightGray;
            FileUploadControl.BackColor = System.Drawing.Color.White;
            desc.BorderColor = System.Drawing.Color.LightGray;

        }
        protected void createBtn_Click(object sender, EventArgs e)
        {
            changetoDefaultBorder();

            string errmsg = "";
            PanelError.Visible = false;

            if (eventTitle.Text.ToString() == "")
            {
                errmsg = "Title cannot be empty! <br>";
                eventTitle.BorderColor = System.Drawing.Color.Red;

            }
            if (eventAddress.Text.ToString() == "")
            {
                errmsg += "Address cannot be empty! <br>";
                eventAddress.BorderColor = System.Drawing.Color.Red;
            }
            if (eventDate.Text.ToString() == "")
            {
                errmsg += "Date cannot be empty! <br>";
                eventDate.BorderColor = System.Drawing.Color.Red;
            }
            if (eventDate.Text.ToString() != "")
            {
                string date = eventDate.Text.ToString();
                DateTime dt = Convert.ToDateTime(date);
                if (dt < DateTime.Now.Date)
                {
                    errmsg += "Please enter a valid date <br>";
                    eventDate.BorderColor = System.Drawing.Color.Red;
                }

            }
            if (startTime.Text.ToString() == "")
            {
                errmsg += "StartTime cannot be empty! <br>";
                startTime.BorderColor = System.Drawing.Color.Red;
            }
            if (endTime.Text.ToString() == "")
            {
                errmsg += "EndTime cannot be empty! <br>";
                endTime.BorderColor = System.Drawing.Color.Red;
            }
            if (startTime.Text.ToString() != "" && endTime.Text.ToString() != "")
            {
                string startTimeNumber = "";
                string endTimeNumber = "";
                string eventStartTime = startTime.Text.ToString();
                string eventEndTime = endTime.Text.ToString();
                string startFrontdigits = eventStartTime.Substring(0, 2);
                string endFrontdigits = eventEndTime.Substring(0, 2);
                string startBackdigits = eventStartTime.Substring(3, 2);
                string endBackdigits = eventEndTime.Substring(3, 2);
                startTimeNumber = startFrontdigits + startBackdigits;
                endTimeNumber = endFrontdigits + endBackdigits;

                if (int.Parse(startTimeNumber) > int.Parse(endTimeNumber))
                {
                    errmsg += "Please ensure that you entered a valid Start & End Time <br>";
                    startTime.BorderColor = System.Drawing.Color.Red;
                    endTime.BorderColor = System.Drawing.Color.Red;
                }

                if ((int.Parse(endTimeNumber) - int.Parse(startTimeNumber)) < 100)
                {
                    errmsg += "Duration must be 1 hour bare minimum";
                    startTime.BorderColor = System.Drawing.Color.Red;
                    endTime.BorderColor = System.Drawing.Color.Red;
                }
            }

            if (DateTime.Parse(eventDate.Text.ToString() + " " + startTime.Text.ToString()) <= DateTime.Now)
            {
                errmsg += "Please ensure that you entered a valid Start Time";
                startTime.BorderColor = System.Drawing.Color.Red;
            }
            if (maxAttend.Text.ToString() == "")
            {
                errmsg += "Maximum number of attendees cannot be empty! <br>";
                maxAttend.BorderColor = System.Drawing.Color.Red;
            }
            if (desc.Text.ToString() == "")
            {
                errmsg += "Description cannot be empty! <br>";
                desc.BorderColor = System.Drawing.Color.Red;
            }

            if (desc.Text.ToString() != "")
            {
                int enterCount = 0, index = 0;

                while (index < desc.Text.Length)
                {
                    // check if current char is part of a word
                    if (desc.Text[index] == '\r' && desc.Text[index + 1] == '\n')
                        enterCount++;
                    index++;
                }
                if (desc.Text.Length > 3000 + enterCount)
                {
                    errmsg += "Character Limit in Description Exceeded! <br>";
                    desc.BorderColor = System.Drawing.Color.Red;
                }
            }

            if (errmsg != "")
            {
                errmsgTb.Text = errmsg;
                PanelError.Visible = true;

            }
            else
            {
                HistoryOTP otp = new HistoryOTP();
                Random rnd = new Random();
                Users user = (Users)Session["user"];

                bool findEmail = otp.GetUserByEmail(user.email);
                string userEmail = user.email;
                OTPassword = rnd.Next(100000, 999999).ToString();
                string userName = user.name;
                string title = eventTitle.Text.ToString();
                int OTPCheck = 1;

                if (findEmail)
                {
                    otp.UpdateOTPByEmail(userEmail, OTPassword, OTPCheck);
                    Enable(userEmail, OTPassword, userName, title);
                }

                else
                {
                    otp.AddHistoryOTP(userEmail, OTPassword);
                    Enable(userEmail, OTPassword, userName, title);
                }
            }
        }

        protected void OTPbtn_click(object sender, EventArgs e)
        {
            Users user = (Users)Session["user"];
            Events ev = new Events();
            HistoryOTP otp = new HistoryOTP();
            HistoryOTP userTrying = otp.GetUserByEmailOTP(user.email);

            if (userTrying != null)
            {
                string otpSent = tbOTP.Text;

                if (userTrying.userOTPCheck == 1)
                {
                    if (userTrying.userOTP == otpSent)
                    {
                        //OTP update
                        int OTPChecked = 0;
                        otp.UpdateOTPByEmail(userTrying.userEmail, OTPassword, OTPChecked);

                        //Create event
                        string eventStartTime = startTime.Text.ToString();
                        string eventEndTime = endTime.Text.ToString();
                        string title = eventTitle.Text.ToString();
                        string venue = eventAddress.Text.ToString();
                        string date = eventDate.Text.ToString();
                        int maxAttendees = int.Parse(maxAttend.Text.ToString());
                        string description = desc.Text.ToString();
                        string picture = "";
                        string note = noteText.Text.ToString();
                        int user_id = user.id;

                        Thread thread = new Thread();
                        DateTime now = DateTime.Now;
                        string andyDate = now.ToString("g");

                        if (FileUploadControl.HasFile)
                        {
                            string filename = Path.GetFileName(FileUploadControl.PostedFile.FileName);
                            FileUploadControl.SaveAs(Server.MapPath("~/Img/" + filename));
                            // insert malware file checker
                            picture = filename;
                            picChosen.Text = filename;
                        }
                        else if (FileUploadControl.HasFile == false)
                        {
                            string filename = "defaultPic.jpg";
                            picture = filename;
                        }

                        int rating = 0;

                        ev = new Events(1, title, venue, date, eventStartTime, eventEndTime, maxAttendees, description, picture, note, rating, user_id);
                        int result = ev.AddEvent();

                        int createdEventId = ev.getMaxEventId();
                        thread = new Thread(createdEventId, "[EVENT]", "success", title, andyDate, picture,
                            description, user_id, user.name);


                        int resultThread = thread.createThreadForEvent();
                        Response.Redirect("/eventDetails.aspx");
                    }
                    else
                    {
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Visible = true;
                }
            }


        }

        protected void testBtn_Click(object sender, EventArgs e)
        {
            eventTitle.Text = "Project Eco";
            eventAddress.Text = "Tampines Mall";
            eventDate.Text = "2021-02-07";
            startTime.Text = "09:00";
            endTime.Text = "10:00";
            maxAttend.Text = "20";
            desc.Text = "This is a short description";
            noteText.Text = "Be ecofriendly";
        }

        static async Task Enable(string email, string otp, string name, string eventName)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("kovitwk21@gmail.com", "ClearView21");
            var subject = "OTP For Event Creation of " + eventName;
            var to = new EmailAddress(email, name);
            var plainTextContent = "Your OTP is: ";
            var htmlContent = "Your OTP is: " + otp;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}