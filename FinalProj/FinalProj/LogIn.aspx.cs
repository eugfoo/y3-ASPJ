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
using System.Text;


namespace FinalProj
{
    public partial class LogIn : System.Web.UI.Page
    {
        public string result = "";
        public string OTPEmail = "";
        public string OTPassword = "";


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (tbEmail.Text == "admin@admin")
                {
                    string adminPassHash = ComputeSha256Hash(tbPass.Text);
                    if (adminPassHash == "ca5ce9636699b4bfa0a3801e2c28842638c13f22e04820b586041a425ce665d9") // hashed version of adminPass41111
                    {
                        Session["admin"] = true;
                        Response.Redirect("homepage.aspx");
                        
                    }
                }
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
                        string countryLogged = Execute(ipAddr).ToString();
                        
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

        static async Task<String>  Execute(string ip)
        {
            var client = new IpDataClient("058ea76069b23c5b3c45cf40558ecd563e0f324f6a8210028747a273");

            // Get IP data from IP
            var ipInfo = await client.Lookup(ip);
            string country = ipInfo.CountryName;
            return country;
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
            Users findUser = user.GetUserByEmail(userEmail.Text);
            if (findUser != null)
            { // user exists
                result = "true";
                OTPassword = rnd.Next(100000, 999999).ToString();

                Execute();
            }
            else
            {
                result = "false";
            }
        }

        static async Task Execute()
        {
            Random rnd = new Random();
            var lgn = new LogIn();
            lgn.OTPassword = rnd.Next(100000, 999999).ToString();
            
            var client = new SendGridClient("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");
            var from = new EmailAddress("kovitwk21@gmail.com", "ClearView21");
            var subject = "OTP";
            var to = new EmailAddress("kovitanwk@gmail.com", "Kovi Tan");
            var plainTextContent = "Your OTP is: ";
            var htmlContent = lgn.OTPassword;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}