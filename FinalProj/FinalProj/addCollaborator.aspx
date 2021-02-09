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
        #adCont {
            margin:2%;
        }
    </style>
    <div id="adCont">
    
    <% if (result == "true")
        { %>
        <div class="alert alert-success" role="alert">
          Collaborator Invited!
        </div>
    <%}
    else if (result == "false" && errmsg != "")
    {%>
    <div class="alert alert-danger" role="alert">
        Collaborator already invited!
    </div>
    <%}
    else if (result == "false")
    { %>
        <div class="alert alert-danger" role="alert">
        Collaborator does not exist!
            </div>
    <%} %>
    <div style="text-align: center;">
        <asp:Label ID="Label1" style="font-size:50px;" runat="server" Text="Manage Access"></asp:Label>
    </div>
        
        <p></p>
       <asp:Button ID="addCollabBtn" runat="server" class="btn btn-success" style="float: left;" Text="Invite a Collaborator" OnClick="addCollabBtn_Click" />
        
        <p></p>

    <table id="myTable" class="table table-bordered">
        <tr class="header">
            <th style="width:50%;">Name</th>
            <th style="width:30%;">Role</th>
            <th style="width:20%;">Status</th>

        </tr>
        <% 
            int index = 0;
            foreach (var element in adList)
            { %>
        <tr>
            <td><%= element.adminName %></td>
            <td><%= element.adminRole %></td>
            <td><%= element.adminStatus %></td>
            <%if (element.adminRole != "Main Admin")
                {%>
            <td>
                <asp:Button ID="remoteBtn" runat="server" class="btn btn-danger" Text="Remove" OnClick="remoteBtn_Click" />
            </td>
            <%} index++;%>


        </tr>
        <%} %>
    </table>

    <br />

    <div id="MyPopup" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">
                    &times;</button>
                <h4 class="modal-title">
                </h4>
            </div>
            <div class="modal-body" style="text-align:center;">
                <div class="form-group">
                <asp:Label runat="server" style="font-size: 28px;">Invite a Collaborator</asp:Label>
                    <br />

                    <asp:Label for="collabEmail" runat="server" style="float:left;">Email address:</asp:Label>
                    <p>

                    <asp:TextBox type="email" ID="collabEmail" class="form-control"  aria-describedby="emailHelp" placeholder="Enter email" runat="server"></asp:TextBox>
                    <p>
                    <asp:Label for="roleChoice" runat="server" style="float:left;">Role:</asp:Label>
                    <asp:DropDownList ID="roleChoice" style="float:left; margin-left:2%; margin-bottom:3%;" class="btn btn-primary dropdown-toggle" runat="server" DataSourceID="SqlDataSource1" DataTextField="Roles" DataValueField="Roles">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [Roles] FROM [rolesTable]" ></asp:SqlDataSource>
                    <asp:Button ID="submit" style="margin-top:3%;" runat="server" class="btn btn-success btn-md btn-block" Text="Submit" OnClick="submit_Click" />

                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">
                    Close</button>
            </div>
        </div>
    </div>
</div>
        </div>

    <script type="text/javascript">
    function ShowPopup(title, body) {
        $("#MyPopup").modal("show");
        }




       
    </script>
</asp:Content>
