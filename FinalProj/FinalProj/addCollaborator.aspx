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
    <% if (result == "true")
        { %>
        <div class="alert alert-success" role="alert">
          Collaborator Invited!
        </div>
    <%}
    else if (result == "false")
    {%>
    <div class="alert alert-danger" role="alert">
        Collaborator does not exist!
    </div>
    <%}
    else
    { %><%} %>
    <asp:Label ID="Label1" runat="server" Text="Manage Access"></asp:Label>
    <br />
    <asp:Button ID="addCollabBtn" runat="server" class="btn btn-success"  Text="Invite a Collaborator" OnClick="addCollabBtn_Click" />
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
                    <asp:Button ID="submit" runat="server" class="btn btn-success btn-md btn-block"  Text="Submit" OnClick="submit_Click"/>
                    <asp:Label ID="resultMsg" runat="server" style="float:left;" Visible="False"></asp:Label>

                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">
                    Close</button>
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
