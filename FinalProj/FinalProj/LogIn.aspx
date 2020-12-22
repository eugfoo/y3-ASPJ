<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="FinalProj.LogIn" %>

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                <a class="ml-2 navbar-brand text-center" href="/homepage.aspx" onclick="deleteAllCookies()">Clear View</a>
            </nav>
            <div style="width:500px; margin:auto; margin-top: 5rem;" class="card">
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
                        &nbsp;<asp:Button ID="btnOTP" runat="server" CssClass="btn btn-secondary" Text="OTP Login" OnClick="btnOTP_Click" ValidateRequestMode="Enabled" Font-Size="10pt" Height="30px" />
                        <asp:TextBox type="password" CssClass="form-control" ID="tbPass" runat="server" ValidationGroup="Credentials"></asp:TextBox>
                        </div>
                    <div class="align-bottom" style="text-align: right;">
                        <asp:Label ID="lblError" CssClass="vError mr-3" runat="server" Visible="False" Font-Italic="False" Font-Size="Small">Incorrect account information. Please try again.</asp:Label>
                        <asp:Button ID="btnSignIn" runat="server" CssClass="btn btn-primary" Text="Sign In" OnClick="btnSignIn_Click" ValidationGroup="Credentials" ValidateRequestMode="Enabled" />
                    &nbsp;</div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
