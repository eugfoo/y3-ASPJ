<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs"  Inherits="FinalProj.LogIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>ClearView</title>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.0/css/all.css" integrity="sha384-lZN37f5QGtY3VHgisS14W3ExzMWZxybE1SJSEsQp9S+oqd12jhcu+A56Ebc1zFSJ" crossorigin="anonymous">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">

    <style type="text/css">
        .vError {
            color: red;
        }

        #box {
            display: none;
        }

        #closeBtn {
            color: white;
        }

        #btnOTP {
            margin: 2%;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                <a class="ml-2 navbar-brand text-center" href="/homepage.aspx" onclick="deleteAllCookies()">Clear View</a>
            </nav>
            <div style="width: 500px; margin: auto; margin-top: 5rem;" class="card">
                <div class="card-header text-center">
                    <ul class="nav nav-tabs card-header-tabs">
                        <li class="nav-item">
                            <a class="nav-link" href="/Register.aspx">Register</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="/LogIn.aspx">Sign In</a>
                        </li>
                    </ul>
                </div>
                <div class="card-body">
                    <h5 class="card-title text-muted font-italic">Enter your account details</h5>
                    <div class="form-group">
                        <label for="formGroupExampleInput">Email</label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="vError" runat="server" ControlToValidate="tbEmail"
                            EnableClientScript="False" ErrorMessage="*" ValidationGroup="Credentials"></asp:RequiredFieldValidator>
                        <asp:TextBox type="email" CssClass="form-control" ID="tbEmail" runat="server" ValidationGroup="Credentials"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput2">Password</label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="vError" runat="server" ControlToValidate="tbPass"
                            EnableClientScript="False" ErrorMessage="*" ValidationGroup="Credentials"></asp:RequiredFieldValidator>
                        <button id="btnOTP" type="button" class="btn btn-secondary" onclick="pop()">OTP Login</button>
                        <asp:TextBox type="password" CssClass="form-control" ID="tbPass" runat="server" ValidationGroup="Credentials"></asp:TextBox>
                        <div id="ReCaptchContainer"></div>
                    </div>
                    <div class="align-bottom" style="text-align: right;">
                        <asp:Label ID="lblError" CssClass="vError mr-3" runat="server" Visible="False" Font-Italic="False" Font-Size="Small">Incorrect account information. Please try again.</asp:Label>
                        <button id="btnForgotPwd" type="button" class="btn btn-hazard" onclick="pop()">Forgot Password</button>
                        <asp:Button ID="btnSignIn" runat="server" CssClass="btn btn-primary" Text="Sign In" OnClick="btnSignIn_Click" ValidationGroup="Credentials" ValidateRequestMode="Enabled" />
                        <br />
                        &nbsp;
                    </div>
                    <!-- this is the start of the popup-->

                    <div id="box" runat="server">
                        <h5 class="card-title text-muted font-italic">Send One-Time Password</h5>
                        <asp:Label for="userEmail" runat="server" Style="float: left;">Your Email address:</asp:Label>
                        <p>

                            <asp:TextBox type="email" ID="userEmail" class="form-control" aria-describedby="emailHelp" placeholder="Enter your email" runat="server"></asp:TextBox>
                            <p>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="submit" input="button" runat="server" class="btn btn-success btn-md btn-block" Text="Submit" OnClick="submit_Click" OnClientClick="pop()"/>
                                        <asp:Label ID="lblOTP" CssClass="vError mr-3" runat="server" Visible="False" Font-Italic="False" Font-Size="Small" ForeColor="Green">OTP Sent! Check your email</asp:Label>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <br />
                                <a id="closeBtn" class="btn btn-danger" onclick="pop()">Close</a>
                    </div>
                    <!-- this is the end of the popup-->

                </div>
            </div>
        </div>
        <script type="text/javascript">
            var modal = null
            function pop() {
                if (modal === null) {
                    document.getElementById("box").style.display = "block";
                    modal = true
                } else {
                    document.getElementById("box").style.display = "none";
                    modal = null
                }
            }

        </script>

    </form>
    <%--<script src="https://www.google.com/recaptcha/api.js?onload=renderRecaptcha&render=explicit" async defer></script>
     <script type="text/javascript">
         var your_site_key = '6LdJqj4aAAAAADXys_74SixLJ13hcdCH3w-T3vQS';
         var renderRecaptcha = function () {
             grecaptcha.render('ReCaptchContainer', {
                 'sitekey': '6LdJqj4aAAAAADXys_74SixLJ13hcdCH3w-T3vQS',
                 'callback': reCaptchaCallback,
                 theme: 'light', //light or dark
                 type: 'image',// image or audio
                 size: 'normal'//normal or compact
             });
         };
         var reCaptchaCallback = function (response) {
             if (response !== '') {
                 document.getElementById('lblMessage1').innerHTML = "";
             }
         };
     </script>--%>
</body>
</html>
