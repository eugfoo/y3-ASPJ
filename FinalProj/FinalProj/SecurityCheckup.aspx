<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="SecurityCheckup.aspx.cs" Inherits="FinalProj.SecurityCheckup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.0/css/all.css" integrity="sha384-lZN37f5QGtY3VHgisS14W3ExzMWZxybE1SJSEsQp9S+oqd12jhcu+A56Ebc1zFSJ" crossorigin="anonymous">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
    <style>
        .form-group {
            border-bottom: 1px solid black;
            padding-bottom: 15px;
        }

        .lblClass{
            font-size: 17px;
            color:grey;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 500px; margin: auto; margin-top: 5rem;" class="card">
        <div class="card-header text-center">
            <ul class="nav nav-tabs card-header-tabs">
                <li class="nav-item" style="margin-left: auto; margin-right: auto;">
                    <h3 class="nav-link active">Security Checkup</h3>
                </li>
            </ul>
        </div>
        <div class="card-body">
            <div class="form-group">
                <h5>You haven't changed your password in:</h5><br />
                <asp:Label ID="lblPassword" CssClass="lblClass" runat="server" Text=""></asp:Label>
            </div>

            <div class="form-group">
                <h5>Your Session History</h5><br />
                <strong><a href="/SessionHistory.aspx" <asp:Label ID="lblSessionHistory" CssClass="lblClass" runat="server" Text="See your recent login attempts"></asp:Label></a></strong>
            </div>

            <div class="form-group">
                <h5>Recent security events</h5><br />
                <asp:Label ID="lblRecentEvents" CssClass="lblClass" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblActions" CssClass="lblClass" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
