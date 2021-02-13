<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="ActivityLogs.aspx.cs" Inherits="FinalProj.WebForm3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
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
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <style>
        #adCont {
            margin:1%;
        }
    </style>
    <script>
        $(function () {
            $("#<%= txtStartDate.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtEndDate.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

        });

    </script>
    <div class="row" style="margin-top:1%; margin-left:1.5%; margin-right:1.5%; text-align:center;">
        <div class="col-12">
        <asp:Panel ID="PanelError" runat="server" Visible="false" CssClass="stuff alert alert-danger ">
                <asp:Label ID="errmsgTb" runat="server"></asp:Label>
        </asp:Panel>
        </div>
    </div>
    <div id="adCont">
        <div class="card">
            <div class="card-body">
                <h5 class="card-header">Filter System</h5>
                <div class="row" style="margin-top:1%;">
                    <div class="col-2">
                        Start Date:
                        <asp:TextBox class="form-control" ID="txtStartDate" runat="server" AutoPostBack="True" />
                    </div>
                    <div class="col-2">

                        End Date:
                        <asp:TextBox class="form-control" ID="txtEndDate" runat="server" AutoPostBack="True" />
                    </div>
                    <div class="col-2">

                    Violations: 
                        <br>
                    <asp:DropDownList ID="violationType" class="btn btn-primary dropdown-toggle" AutoPostBack="True" runat="server">

                            <asp:ListItem Selected="True" Value="All"> All </asp:ListItem>
                            <asp:ListItem Value="FailedAuthentication">Failed Authentication </asp:ListItem>
                            <asp:ListItem Value="Malware">Malware </asp:ListItem>
                            <asp:ListItem Value="Spamming">Spamming </asp:ListItem>
                            <asp:ListItem>XSS</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="card" style="margin-top:1%;">
            <div class="card-body">
                <h5 class="card-header">User Activity Logs</h5>
                <div class="row" style="margin-top:1%;">
                    <div class="col-12">
                    <table id="dtBasicExample"  class="table table-striped table-bordered table-sm" cellspacing="0">
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
                            <% if (!IsPostBack)
                                {
                                    foreach (var element in algList)
                                    { %>
                            <tr>
                                <td><%= Convert.ToDateTime(element.DateTime) %></td>

                                <td>
                                    <img style="border-radius: 100%; width: 40px; height: 40px; margin-right: 5%;" src="<%=picList[algList.IndexOf(element)] %>"></img><%= element.Username %></td>
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

                                <%}
                                    else
                                    {%><td style="color: #006400;"><%= element.ViolationType %></td>
                                <%} %>

                                <td><%= element.userEmail %></td>
                                <td><%= element.userCountry %></td>

                            </tr>
                            <%}
                                }
                                else
                                {
                                    foreach (var element in aalgList)
                                    { %>

                            <tr>
                                <td><%= Convert.ToDateTime(element.DateTime) %></td>

                                <td>
                                    <img style="border-radius: 100%; width: 40px; height: 40px; margin-right: 5%;" src="<%=apicList[aalgList.IndexOf(element)] %>"></img><%= element.Username %></td>
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

                                <%}
                                    else
                                    {%><td style="color: #006400;"><%= element.ViolationType %></td>
                                <%} %>

                                <td><%= element.userEmail %></td>
                                <td><%= element.userCountry %></td>

                            </tr>

                            <%}
                                }%>
                        </tbody>
                    </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
