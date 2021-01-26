﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using IpData;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.IO;
using System.Drawing;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace FinalProj
{
    public partial class LogIn : System.Web.UI.Page
    {
        public string result = "";
        public string OTPEmail = "";
        public string OTPassword = "";
        public string userName = "";
        protected List<Logs> lgList;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                HistoryOTP otp = new HistoryOTP();
                HistoryOTP userTrying = otp.GetUserByEmailOTP(tbEmail.Text);

                MainAdmins mainadmin = new MainAdmins();
                MainAdmins adminlogin = mainadmin.GetAdminByEmail(tbEmail.Text);

                string adminPassHash = ComputeSha256Hash(tbPass.Text);
                if (adminlogin != null) //user exists
                {
                    if (adminlogin.MainAdminPassword == adminPassHash) // hashed version of adminPass41111
                    {
                        Session["admin"] = true;
                        Response.Redirect("homepage.aspx");

                    }
                }

                if (userTrying != null)
                {
                    Users user = new Users();
                    Users tryingUser = user.GetUserByEmail(tbEmail.Text);

                    string otpSent = tbPass.Text;

                    if (tryingUser != null) // user exists
                    {
                        if (userTrying.userOTPCheck == 1)
                        {
                            if (userTrying.userOTP == otpSent)
                            {
                                int counter = 0;
                                int OTPChecked = 0;

                                Session["user"] = tryingUser;
                                Session["id"] = tryingUser.id;
                                Session["Name"] = tryingUser.name;
                                Session["Pic"] = tryingUser.DPimage;
                                Session["email"] = tryingUser.email;

                                string ipAddr = GetIPAddress();
                                string countryLogged = CityStateCountByIp(ipAddr);
                                CityStateCountByIp(ipAddr);

                                DateTime dtLog = DateTime.Now;
                                Logs lg = new Logs();

                                //Email function for new sign in
                                lgList = lg.GetAllLogsOfUser(userTrying.userEmail);

                                foreach (var log in lgList)
                                {
                                    if (log.ipAddr == ipAddr)
                                    {
                                        counter++;
                                    }
                                }

                                lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged);
                                otp.UpdateOTPByEmail(userTrying.userEmail, OTPassword, OTPChecked);



                                if (counter == 0)
                                {
                                    EmailFxNew(userTrying.userEmail, tryingUser.name, ipAddr, countryLogged);
                                }

                                Response.Redirect("homepage.aspx");


                            }
                        }
                        else
                        {
                            string passHash = ComputeSha256Hash(tbPass.Text);
                            if (tryingUser != null) // user exists
                            {
                                if (tryingUser.passHash == passHash)
                                {
                                    int OTPChecked = 0;
                                    int counter = 0;

                                    Session["user"] = tryingUser;
                                    Session["id"] = tryingUser.id;
                                    Session["Name"] = tryingUser.name;
                                    Session["Pic"] = tryingUser.DPimage;
                                    Session["email"] = tryingUser.email;


                                    string ipAddr = GetIPAddress();
                                    string countryLogged = CityStateCountByIp(ipAddr);
                                    DateTime dtLog = DateTime.Now;
                                    Logs lg = new Logs();

                                    //Email function for new sign in
                                    lgList = lg.GetAllLogsOfUser(userTrying.userEmail);

                                    foreach (var log in lgList)
                                    {
                                        if (log.ipAddr == ipAddr)
                                        {
                                            counter++;
                                        }
                                    }

                                    lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged);

                                    otp.UpdateOTPByEmail(userTrying.userEmail, OTPassword, OTPChecked);

                                    Response.Redirect("homepage.aspx");



                                    if (counter == 0)
                                    {
                                        EmailFxNew(userTrying.userEmail, tryingUser.name, ipAddr, countryLogged);
                                    }
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
                    else
                    {
                        lblError.Visible = true;
                    }
                }
                else
                {
                    Users user = new Users();
                    Users tryingUser = user.GetUserByEmail(tbEmail.Text);

                    string passHash = ComputeSha256Hash(tbPass.Text);
                    if (tryingUser != null) // user exists
                    {
                        if (tryingUser.passHash == passHash)
                        {

                            Session["user"] = tryingUser;
                            Session["id"] = tryingUser.id;
                            Session["Name"] = tryingUser.name;
                            Session["Pic"] = tryingUser.DPimage;
                            Session["email"] = tryingUser.email;


                            string ipAddr = GetIPAddress();
                            //string countryLogged = "Singapore";
                            string countryLogged = CityStateCountByIp(ipAddr);
                            DateTime dtLog = DateTime.Now;
                            Logs lg = new Logs();
                            lg.AddLog(tryingUser.email, dtLog, ipAddr, countryLogged);
                            Response.Redirect("homepage.aspx");
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

                return Country;
            }

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

        public string ComputeSha256Hash(string data)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        protected void btnOTP_Click(object sender, EventArgs e)
        {
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Users user = new Users();
            HistoryOTP otp = new HistoryOTP();

            Users findUser = user.GetUserByEmail(userEmail.Text);
            bool findEmail = otp.GetUserByEmail(userEmail.Text);


            if (findUser != null)
            { // user exists
                OTPassword = rnd.Next(100000, 999999).ToString();
                OTPEmail = userEmail.Text;
                userName = findUser.name;
                int OTPCheck = 1;


                if (findEmail)
                {
                    result = "true";
                    otp.UpdateOTPByEmail(OTPEmail, OTPassword, OTPCheck);
                    EmailFxOTP(OTPEmail, OTPassword, userName);
                }

                else
                {
                    result = "true";
                    otp.AddHistoryOTP(OTPEmail, OTPassword);
                    EmailFxOTP(OTPEmail, OTPassword, userName);
                }
            }
            else
            {
                result = "false";
            }
        }

        public static async Task EmailFxOTP(string email, string otp, string name)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("kovitwk21@gmail.com", "ClearView21");
            var subject = "OTP Login ClearView";
            var to = new EmailAddress(email, name);
            var plainTextContent = "Your OTP is: ";
            var htmlContent = "Your OTP is: " + otp;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public static async Task EmailFxNew(string email, string name, string ip, string country)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("kovitwk21@gmail.com", "ClearView21");
            var subject = "Is this you? Login ClearView";
            var to = new EmailAddress(email, name);
            var plainTextContent = "";
            var htmlContent = "<strong><h2>We detected a new sign-in to your account</h2></strong><br/><p><strong>New sign-in detected from this IP-Address: </strong> " + ip + "</p><p> <strong>Country:</strong> " + country + "</p><strong><p>If this was you, please ignore this email, otherwise click here to change your password " + "http://localhost:60329/ForgotPassword.aspx" + "</p></strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}