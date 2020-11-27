<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="FinalProj.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>ClearView</title>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.0/css/all.css" integrity="sha384-lZN37f5QGtY3VHgisS14W3ExzMWZxybE1SJSEsQp9S+oqd12jhcu+A56Ebc1zFSJ" crossorigin="anonymous">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
    <style type="text/css">
        .auto-style1 {
            position: relative;
            display: block;
            padding-left: 1.25rem;
            left: -540px;
            top: -269px;
        }
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
            <div style="width: 500px; margin: auto; margin-top: 5rem;" class="card">
                <div class="card-header text-center">
                    <ul class="nav nav-tabs card-header-tabs">
                        <li class="nav-item">
                            <a class="nav-link active" href="/Register.aspx">Register</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="/LogIn.aspx">Sign In</a>
                        </li>
                    </ul>
                </div>
                <div class="card-body">
                    <h5 class="card-title text-muted font-italic">Create a new account</h5>
                    <div class="form-group">
                        <label for="formGroupExampleInput">Email</label>
                        <asp:RequiredFieldValidator CssClass="vError" ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbEmail" EnableClientScript="False" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:TextBox type="email" CssClass="form-control" ID="tbEmail" runat="server" CausesValidation="True"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput">Display Name</label>
                        <asp:RequiredFieldValidator CssClass="vError" ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbName" EnableClientScript="False" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:TextBox type="text" CssClass="form-control" ID="tbName" runat="server" CausesValidation="True"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput2">Password</label>
                        <asp:RequiredFieldValidator CssClass="vError" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbPass" EnableClientScript="False" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:TextBox type="password" CssClass="form-control" ID="tbPass" runat="server" CausesValidation="True"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput2">Confirm Password</label>
                        <asp:RequiredFieldValidator CssClass="vError" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbCfmPass" EnableClientScript="False" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:TextBox type="password" CssClass="form-control" ID="tbCfmPass" runat="server" CausesValidation="True"></asp:TextBox>
                    </div>
                    <div class="form-group auto-style1" style="display:none; left: 0px; top: 0px;">
                        <asp:CheckBox CssClass="form-check-input" ID="cbIsOrg" runat="server" SkinID="gridCheck" />
                        <label class="form-check-label" for="gridCheck">
                            Organization
                        </label>
                    </div>
                    <div class="align-bottom" style="text-align: right;">
                        <asp:Label ID="lblError" CssClass="vError mr-3" runat="server" Visible="False" Font-Italic="False" Font-Size="Small">Email is already in use.</asp:Label>
                        <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-primary" Text="Register" OnClick="btnRegister_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
