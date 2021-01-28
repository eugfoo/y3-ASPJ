<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TwoFactor1.aspx.cs" Inherits="FinalProj.TwoFactor1" %>

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
               <div style="width: 500px; margin: auto; margin-top: 5rem;" class="card">
                    <div class="card-header text-center">
                        <ul class="nav nav-tabs card-header-tabs">
                            <li class="nav-item">
                                <a class="nav-link" >Two-Factor Authentication</a>
                            </li>
                        </ul>
                    </div>
                <div class="card-body">
                    <h5 class="card-title text-muted font-italic">Your login is protected with an authenticator app. Enter your authenticator code below.s</h5>
                    <div class="form-group">
                        <label for="formGroupExampleInput">Email</label>
                            <label for="formGroupExampleInput">Input Google Authenticator code here.</label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="vError" runat="server" ControlToValidate="tbAuthenticator"
                            EnableClientScript="False" ErrorMessage="*" ValidationGroup="Credentials"></asp:RequiredFieldValidator>
                        <asp:TextBox type="number" CssClass="form-control" ID="tbAuthenticator" runat="server" Width="20%" MaxLength="7"></asp:TextBox>
                    </div>
                    <div class="align-bottom" style="text-align: left;">
                           <asp:Button ID="btnAuthenticate" runat="server" CssClass="btn btn-primary" Text="Authenticate" OnClick="btnAuthenticate_Click" ValidationGroup="Credentials" ValidateRequestMode="Enabled" />
                        <asp:Label ID="lblError" CssClass="vError mr-3" runat="server" Visible="False" Font-Italic="False" Font-Size="Small">Incorrect code. Please try again.</asp:Label>
                            &nbsp;
                   </div>
                   </div>
               </div>
    </form>           
</body>
</html>
