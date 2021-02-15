<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="EmailLogs.aspx.cs" Inherits="FinalProj.EmailLogs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div style="width: 500px; margin: auto; margin-top: 5rem;" class="card">
        <div class="card-header text-center">
            <ul class="nav nav-tabs card-header-tabs">
                <li class="nav-item" style="margin-left: auto; margin-right: auto;">
                    <h3 class="nav-link">Emails from Clearview</h3>
                </li>
            </ul>
        </div>
        <div class="card-body">
             <% foreach (var element in elgList)
                    { %>
                <div class="form-group">
                    <asp:Label ID="lblTitle" CssClass="lblClass" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="lblDate" CssClass="lblClass" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="lblUserEmail" CssClass="lblClass" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblSeperator" CssClass="lblClass" runat="server" Text=" . "></asp:Label>
                    <asp:Label ID="lblSenderEmail" CssClass="lblClass" runat="server" Text=""></asp:Label>
                </div>
            <% } %>
        </div>
    </div>
</asp:Content>
