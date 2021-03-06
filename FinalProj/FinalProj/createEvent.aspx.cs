﻿using System;
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
using Google.Authenticator;
using System.Net;
using Newtonsoft.Json.Linq;
using AvScan.WindowsDefender;
using System.Web.Helpers;
using System.Web.Security.AntiXss;

namespace FinalProj
{
    public partial class Personal : System.Web.UI.Page
    {
        string OTPassword = "";
        int goog;
        protected Sessionmg sesDeets;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null) // A user has signed in
            {
                Response.Redirect("/homepage.aspx");
            }
            else
            {
                Sessionmg ses = new Sessionmg();
                blocked bl = new blocked(); 

                sesDeets = ses.GetSession(Session["email"].ToString());
                if (sesDeets.Active == 1)
                {
                    Users usr = new Users();
                    Users user = (Users)Session["user"];
                    //AntiForgery.GetHtml().ToString();
                    //var exeLocation = "C://Program Files//Windows Defender//MpCmdRun.exe";
                    //var scanner = new WindowsDefenderScanner(exeLocation);
                    //var result = scanner.Scan("C://Users//Eugene Foo//Documents//Digital Forensics//eicar.com.txt");
                    //if (result.ToString() == "ThreatFound")
                    //{
                    //    string ipAddr = GetIPAddress();
                    //    string countryLogged = CityStateCountByIp(ipAddr);
                    //    DateTime dtLog = DateTime.Now;
                    //    CityStateCountByIp(ipAddr);
                    //    ActivityLog alg = new ActivityLog();
                    //    ses.UpdateSession(Session["email"].ToString(), 0); 
                    //    alg.AddActivityLog(dtLog, user.name, ipAddr, "Uploaded Malicious Event Photo", "Malware", user.email, countryLogged);
                    //    bl.AddBlockedAcc(user.email, user.name, "Uploaded Malicious Event Photo", dtLog); // adds account the block table
                    //    alg.AddActivityLog(dtLog, user.name, ipAddr, "Account Blocked", "Malware", user.email, countryLogged); // logs block acc
                    //    Session.Clear();
                    //    Response.Redirect("/homepage.aspx");

                    //}


                    goog = usr.GetUserById(user.id).googleauth;

                    if (goog == 1)
                    {
                        lblSent.Text = "Enter One-Time Password from Google Authenticator";
                    }
                    else
                    {
                        lblSent.Text = "Enter One-Time Password sent to your Email";
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
            if (FileUploadControl.HasFile)
            {
                string filename = Path.GetFileName(FileUploadControl.PostedFile.FileName);
                // check file for malware 
                
            }
            // prevent event spamming
            List<Events> evList;
            Users user = (Users)Session["user"];
            Events ev = new Events();
            evList = ev.GetAllEventsByUserID(user.id);
            if (evList.Count > 1)
            {
                TimeSpan interval = DateTime.Now.Subtract(evList[0].dt);
                TimeSpan intervalA = DateTime.Now.Subtract(evList[1].dt);
                if (interval.Minutes < 5 && intervalA.Minutes < 5)
                {
                    errmsgTb.Text = errmsg;
                    PanelError.Visible = true;

                    string ipAddr = GetIPAddress();

                    string countryLogged = CityStateCountByIp(ipAddr);
                    DateTime dtLog = DateTime.Now;

                    CityStateCountByIp(ipAddr);
                    ActivityLog alg = new ActivityLog();
                    alg.AddActivityLog(dtLog, user.name, ipAddr, "Create Event Attempt: " + eventTitle.Text, "Spamming", user.email, countryLogged);
                }
            }
            

            if (errmsg != "")
            {
                errmsgTb.Text = errmsg;
                PanelError.Visible = true;

            }
            else
            {
                if (goog != 1)
                {
                    HistoryOTP otp = new HistoryOTP();
                    Random rnd = new Random();

                    bool findEmail = otp.GetUserByEmail(user.email);
                    string userEmail = user.email;
                    OTPassword = rnd.Next(100000, 999999).ToString();
                    string userName = user.name;
                    string title = AntiXssEncoder.HtmlEncode(eventTitle.Text.ToString(), true);
                    int OTPCheck = 1;

                    if (findEmail){
                        otp.UpdateOTPByEmail(userEmail, OTPassword, OTPCheck);
                        Enable(userEmail, OTPassword, userName, title);
                    }else{
                        otp.AddHistoryOTP(userEmail, OTPassword, OTPCheck);
                        Enable(userEmail, OTPassword, userName, title);
                    }
                }
            }
        }

        protected void OTPbtn_click(object sender, EventArgs e)
        {
            Users usr = new Users();
            Users user = (Users)Session["user"];
            Events ev = new Events();
            string otpSent = tbOTP.Text;
            string uniqueKey = usr.GetUserById(user.id).googleKey;


            if (goog == 1)
            {
                bool status = ValidateTwoFactorPIN(otpSent, uniqueKey);
                if (status)
                {
                    //Create event
                    string eventStartTime = AntiXssEncoder.HtmlEncode(startTime.Text.ToString(), true);
                    string eventEndTime = AntiXssEncoder.HtmlEncode(endTime.Text.ToString(), true);
                    string title = AntiXssEncoder.HtmlEncode(eventTitle.Text.ToString(), true);
                    string venue = AntiXssEncoder.HtmlEncode(eventAddress.Text.ToString(), true);
                    string date = AntiXssEncoder.HtmlEncode(eventDate.Text.ToString(), true);
                    int maxAttendees = int.Parse(AntiXssEncoder.HtmlEncode(maxAttend.Text.ToString(),true));
                    string description = AntiXssEncoder.HtmlEncode(desc.Text.ToString(), true);
                    string picture = "";
                    string note = AntiXssEncoder.HtmlEncode(noteText.Text.ToString(), true);
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
                    DateTime dt = DateTime.Now;

                    ev = new Events(1, title, venue, date, eventStartTime, eventEndTime, maxAttendees, description, picture, note, rating, user_id, dt);
                    int result = ev.AddEvent();

                    int createdEventId = ev.getMaxEventId();
                    thread = new Thread(createdEventId, "[EVENT]", "success", title, andyDate, picture,
                        description, user_id, user.name);


                    int resultThread = thread.createThreadForEvent();


                    string ipAddr = GetIPAddress();
                    string countryLogged = CityStateCountByIp(ipAddr);
                    DateTime dtLog = DateTime.Now;
                    CityStateCountByIp(ipAddr);
                    ActivityLog alg = new ActivityLog();
                    alg.AddActivityLog(dtLog, user.name, ipAddr, "Event Created: " + title, "-", user.email, countryLogged);
                    Response.Redirect("/homepage.aspx");
                }
                else
                {
                    lblError.Visible = true;
                }
            }
            else
            {
                HistoryOTP otp = new HistoryOTP();
                HistoryOTP userTrying = otp.GetUserByEmailOTP(user.email);
                if (userTrying != null)
                {

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
                                picture = filename;
                                picChosen.Text = filename;
                            }
                            else if (FileUploadControl.HasFile == false)
                            {
                                string filename = "defaultPic.jpg";
                                picture = filename;
                            }

                            int rating = 0;
                            DateTime dt = DateTime.Now;
                            ev = new Events(1, title, venue, date, eventStartTime, eventEndTime, maxAttendees, description, picture, note, rating, user_id, dt);
                            int result = ev.AddEvent();

                            int createdEventId = ev.getMaxEventId();
                            thread = new Thread(createdEventId, "[EVENT]", "success", title, andyDate, picture,
                                description, user_id, user.name);


                            int resultThread = thread.createThreadForEvent();

                            string ipAddr = GetIPAddress();

                            string countryLogged = CityStateCountByIp(ipAddr);
                            DateTime dtLog = DateTime.Now;

                            CityStateCountByIp(ipAddr);
                            ActivityLog alg = new ActivityLog();
                            alg.AddActivityLog(dtLog, user.name, ipAddr, "Event Created: " + title, "-", user.email, countryLogged);
                            Response.Redirect("/homepage.aspx");
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
        }

        public bool ValidateTwoFactorPIN(string pin, string uniqueKey)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(uniqueKey, pin);
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

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static string CityStateCountByIp(string IP)
        {
            //var url = "http://freegeoip.net/json/" + IP;
            //var url = "http://freegeoip.net/json/" + IP;
            string url = "http://api.ipstack.com/" + IP + "?access_key=01ca9062c54c48ef1b7d695b008cae00";
            var request = System.Net.WebRequest.Create(url);
            WebResponse myWebResponse = request.GetResponse();
            Stream stream = myWebResponse.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                JObject objJson = JObject.Parse(json);
                string Country = objJson["country_name"].ToString();
                string Country_code = objJson["country_code"].ToString();
                if (Country == "")
                {
                    Country = "-";
                }
                else if (Country_code == "")
                {
                    Country = Country;
                }
                else
                {
                    Country = Country + " (" + Country_code + ")";
                }
                return Country;
            }
        }
    }
}