<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="addCollaborator.aspx.cs" Inherits="FinalProj.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
    table {
        border-collapse:separate;
        border:solid black 1px;
        border-radius:6px;
        -moz-border-radius:6px;
    }

    td, th {
        border-left:solid black 1px;
        border-top:solid black 1px;
    }


    td:first-child, th:first-child {
         border-left: none;
    }
    </style>
    <asp:Label ID="Label1" runat="server" Text="Manage Access" ></asp:Label>
    <br />
    <asp:Button ID="addCollabBtn" runat="server" class="btn btn-success" Text="Invite a Collaborator" />
    <br />
    <table id="myTable" class="table table-bordered">
        <tr class="header">
            <th style="width:60%;">Name</th>
            <th style="width:40%;"></th>
        </tr>
        <tr>
            <td>Alfreds Futterkiste</td>
            <td>Germany</td>
        </tr>
    </table>

    <br />
</asp:Content>
