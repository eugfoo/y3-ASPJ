<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="sessionHistory.aspx.cs" Inherits="FinalProj.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <script>

        //$("#dtBasicExample").dataTable({
        //    "aaSorting": [0, 'desc']
        //});

        $(document).ready(function () {
            $('#dtBasicExample').DataTable({
                "aaSorting": []
            });
        });  
        var table = $('#dtBasicExample').DataTable({
            columnDefs: [
                {
                    type: 'DateTime',
                    targets: [0],
                }
            ],           
        });
    </script>
    <table id="dtBasicExample" class="table table-striped table-bordered table-sm" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>DateTime</th>
                <th>Ip Address</th>
                <th>Country</th>
                <th>Result</th>

 
            </tr>
        </thead>
        <tbody>
            <% foreach (var element in lgList)
                { %>
        <tr>
            <td><%= Convert.ToDateTime(element.DateTime) %></td>
            <td><%= element.ipAddr %></td>
            <td><%= element.Country %></td>
            <% if (element.result == "Successful Login Attempt")
                { %>
                <td style="color: #006400;"><%= element.result %></td>
            <%}
               else if (element.result == "Failed Login Attempt"){ %>
                <td style="color: #8B0000;"><%= element.result %></td>
            <%} %>

        </tr>        
        <%} %>

        </tbody>

    </table>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
</asp:Content>
