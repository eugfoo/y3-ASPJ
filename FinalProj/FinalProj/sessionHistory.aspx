<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="sessionHistory.aspx.cs" Inherits="FinalProj.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table id="example" class="table table-striped table-bordered" style="width:100%">
        <thead>
            <tr>
                <th>DateTime</th>
                <th>Ip Address</th>
                <th>Country</th>
 
            </tr>
        </thead>
        <tbody>
            <% foreach (var element in lgList)
            { %>
        <tr>
            <td><%= element.DateTime %></td>
            <td><%= element.ipAddr %></td>
            <td><%= element.Country %></td>

        </tr>
        <%} %>

        </tbody>
        <tfoot>
            <tr>
                <th>DateTime</th>
                <th>Ip Address</th>
                <th>Country</th>
            </tr>
        </tfoot>
    </table>
</asp:Content>
