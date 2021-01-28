<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="ActivityLogs.aspx.cs" Inherits="FinalProj.WebForm3"%>
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

    <asp:DropDownList id="violationType" AutoPostBack="True" runat="server">

        <asp:ListItem Selected="True" Value="All"> All </asp:ListItem>
        <asp:ListItem Value="FailedAuthentication"> FailedAuthentication </asp:ListItem>
        <asp:ListItem Value="Malware"> Malware </asp:ListItem>
        <asp:ListItem Value="Spamming"> Spamming </asp:ListItem>
    </asp:DropDownList>

    <table id="dtBasicExample" class="table table-striped table-bordered table-sm" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>DateTime</th>
                <th>Username</th>
                <th>Ip Address</th>
                <th>Action</th>
                <th>Violation Type</th>
                <th>Email</th>
                <th>Country</th>


            </tr>
        </thead>
        <tbody>
            <% 
                foreach (var element in algList)
                { %>
        <tr>
            <td><%= Convert.ToDateTime(element.DateTime) %></td>
            <td><img Style="border-radius: 100%; width: 40px; height: 40px; margin-right:5%;" src="<%=picList[algList.IndexOf(element)] %>"></img><%= element.Username %></td>
            <td><%= element.ipAddr %></td>
            <% if (element.Action == "Successful Login Attempt")
                { %>
                <td style="color: #006400;"><%= element.Action %></td>
            <%}
            else if (element.Action == "Failed Login Attempt")
            { %>
                <td style="color: #8B0000;"><%= element.Action %></td>
            <%}
            else if (element.ViolationType == "-")
            { %>
                    <td style="color: #006400;"><%= element.Action %></td>
            <%}
            else
            {%>
                <td style="color: #8B0000;"><%= element.Action %></td>

            <%} %>
            <%if (element.ViolationType != "-")
                    { %><td style="color: #8B0000;"><%= element.ViolationType %></td>
                
            <%}else
                {%><td style="color:#006400;"><%= element.ViolationType %></td>
            <%} %>

            <td><%= element.userEmail %></td>
            <td><%= element.userCountry %></td>

        </tr>        
        <%} %>

        </tbody>

    </table>
</asp:Content>
