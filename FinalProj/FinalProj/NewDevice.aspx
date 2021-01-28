<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewDevice.aspx.cs" Inherits="FinalProj.NewDevice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 500px; margin: auto; margin-top: 5rem;" class="card">
                <div class="card-header text-center">
                    <ul class="nav nav-tabs card-header-tabs">
                        <li class="nav-item">
                            <a class="nav-link" href="/Login.aspx">&#8592; Back to login</a>
                        </li>
                    </ul>
                </div>
                <div class="card-body">
                    <h5 class="card-title text-muted font-italic">We have detected that you are logging in from a new device. A verification code has been sent to your email.</h5>
                    <div class="form-group">
                        <label for="formGroupExampleInput">Please enter the verification code here.</label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="vError" runat="server" ControlToValidate="tbCode"
                            EnableClientScript="False" ErrorMessage="*" ValidationGroup="Credentials"></asp:RequiredFieldValidator>
                        <asp:TextBox type="number" CssClass="form-control" ID="tbCode" runat="server" ValidationGroup="Credentials"></asp:TextBox>
                    </div>
                    <div class="align-bottom" style="text-align: right;">
                        <asp:Label ID="lblError" CssClass="vError mr-3" runat="server" Visible="False" Font-Italic="False" Font-Size="Small">Invalid code. Please try again.</asp:Label>
                        <asp:Button ID="btnVerify" runat="server" CssClass="btn btn-primary" Text="Verify code" OnClick="btnVerify_Click" ValidationGroup="Credentials" ValidateRequestMode="Enabled" />
                        <br />
                        &nbsp;
                    </div>
    </form>
</body>
</html>
