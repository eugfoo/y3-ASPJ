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
    <h1>Two-factor Authenticaton</h1>
            <p>Your login is protected with an authenticator app. Enter your authenticator code below.</p>
           <div class="form-group">
                        <label for="formGroupExampleInput">Input Google Authenticator code here.</label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="vError" runat="server" ControlToValidate="tbAuthenticator"
                            EnableClientScript="False" ErrorMessage="*" ValidationGroup="Credentials"></asp:RequiredFieldValidator>
                        <asp:TextBox type="number" CssClass="form-control" ID="tbAuthenticator" runat="server"></asp:TextBox>
             </div>
            <div class="align-bottom" style="text-align: right;">
                  <asp:Label ID="lblError" CssClass="vError mr-3" runat="server" Visible="False" Font-Italic="False" Font-Size="Small">Incorrect account information. Please try again.</asp:Label>
                   <asp:Button ID="btnAuthenticate" runat="server" CssClass="btn btn-primary" Text="Authenticate" OnClick="btnAuthenticate_Click" ValidationGroup="Credentials" ValidateRequestMode="Enabled" />
                    &nbsp;
           </div>
    </form>
            
</body>
</html>
