﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FinalProj
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Users checkEmail = new Users();

                if (checkEmail.GetUserByEmail(tbEmail.Text) == null) // If the email is unused...
                { 
                    if (tbPass.Text == tbCfmPass.Text)               // If the passwords match...
                    {
                        if (tbPass.Text.Length > 8)                  // If the password is longer than the amount of seconds I wish to live...
                        {
                            if (IsReCaptchaValid())
                            { 
                             //string passHash = ComputeSha256Hash(tbPass.Text);
                            DateTime now = DateTime.Now;
                            string DTNow = now.ToString("g");

                            Users user = new Users(tbEmail.Text, tbName.Text, cbIsOrg.Checked.ToString(), tbPass.Text, DTNow);
                            user.AddUser();
                            Response.Redirect("LogIn.aspx");
                            }
                            else
                            {
                                lblError.Text = "Failed Captcha. Please try again.";
                                lblError.Visible = true;
                            }
                         }
                        else
                        {
                            lblError.Text = "Password should be longer than 8 characters.";
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        lblError.Text = "Passwords do not match.";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Email is already in use.";
                    lblError.Visible = true;
                }
            }
        }

        /*public string ComputeSha256Hash(string data)
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
        }*/
        public bool IsReCaptchaValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6LdJqj4aAAAAAAYEvpiOboFJ8xUV1XHYNpaIdfJz";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }
    }
}