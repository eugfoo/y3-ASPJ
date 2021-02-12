<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="accVerify.aspx.cs" Inherits="FinalProj.WebForm4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>

    #bodyCont {
        margin:1%;
        }

</style>
    <div id="bodyCont">
    <asp:Label ID="Label1" runat="server" style="font-size:50px; text-align: center;" Text="Verify your Account"></asp:Label>
    <asp:Panel ID="PanelError" runat="server" Visible="false" CssClass="stuff alert alert-danger ">
        <asp:Label ID="errmsgTb" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="PanelSuccess" runat="server" Visible="false" CssClass="stuff alert alert-success ">
        <asp:Label ID="scssmsg" runat="server"></asp:Label>
    </asp:Panel>

    <div class="form-group">
        <label for="accVerifytb">Verify Account</label>
        <asp:TextBox class="form-control" ID="accVerifytb" runat="server" placeholder="Enter OTP"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="accVerifytb" runat="server" ForeColor="Red" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Font-Bold="True" Font-Italic="True">
        </asp:RegularExpressionValidator>
    </div>
    <asp:Button ID="submitVerify"  class="btn btn-primary" runat="server" Text="Button" OnClick="submitVerify_Click" />

    </div>

</asp:Content>
