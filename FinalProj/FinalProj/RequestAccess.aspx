<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="RequestAccess.aspx.cs" Inherits="FinalProj.RequestAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <b class="lead">Request Role from Admin:</b>
    <asp:DropDownList ID="roleDDL" class="btn btn-primary dropdown-toggle" DataSourceID="SqlDataSource1" DataTextField="Roles" DataValueField="Roles" runat="server" AutoPostBack="True">
    </asp:DropDownList>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [Id], [Roles] FROM [rolesTable]"></asp:SqlDataSource>

</asp:Content>
