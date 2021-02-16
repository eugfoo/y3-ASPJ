<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="FinalProj.AdminPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="min-height: 90vh">
        <!-- Make sure to put all your things under the div with the min height
            Also, Kovi can you please not change the css of the html elements, please use classes and ids.
            Really appreaciate it. I hope you can change it in your EventStatus.aspx so it doesn't interfere with my html-->
        <div class="display-4 text-center">
                Hello World! This is the Admin Page. Here is where only the ADMINS can run the show!<br />
        </div>
    </div>
</asp:Content>

