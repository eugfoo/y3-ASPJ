<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="addCollaborator.aspx.cs" Inherits="FinalProj.WebForm1"  %>
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

.ttInfo:hover {
            cursor: pointer;
        }
        /* The switch - the box around the slider */
        .switch {
            position: relative;
            display: inline-block;
            width: 60px;
            height: 34px;
        }

            /* Hide default HTML checkbox */
            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        /* The slider */
        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 26px;
                width: 26px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        #box {
            display: none;
            margin-top: 4%;
            margin-bottom: 4%;
        }
    </style>
    <div id="adCont">
    
    <% if (result == "true")
        { %>
        <div class="alert alert-success" role="alert">
          Collaborator Invited!
        </div>
    <%}
    else if (result == "false" && collabEmail.Text == "")
    { %>
        <div class="alert alert-danger" role="alert">
            Please enter an email!
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
        <asp:Label ID="label2" style="font-size:50px;" runat="server" Text="Manage Roles"></asp:Label>
    </div>
        <b class="lead">Roles:</b>
        <asp:DropDownList ID="roleDDL" class="btn btn-primary dropdown-toggle" DataSourceID="SqlDataSource1" DataTextField="Roles" DataValueField="Roles" runat="server" AutoPostBack="true" >
        </asp:DropDownList>

        <asp:Button ID="addRole" CssClass="btn btn-primary" runat="server" Text="Add Role" />
         <div class="col-12 col-sm-5 col-md-5">
             <div class="form-group">
                <label for="formGroupExampleInput2">Name:</label>
                <asp:TextBox type="text" CssClass="form-control" ID="tbName" runat="server" CausesValidation="True"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>View Application Logs: </label>
                <i class="ttInfo fas fa-info-circle" data-html='true' data-toggle="tooltip" data-placement="bottom"
                    title="Allow users under this role to view application activity logs"></i>
                <label class="switch">
                    <asp:CheckBox ID="aaLogs" runat="server" CausesValidation="false" AutoPostBack="true" />
                    <span class="slider round"></span>
                </label>
            </div>
            <div class="form-group">
                    <label>Manage Collaborators</label>
                    <i class="ttInfo fas fa-info-circle" data-html='true' data-toggle="tooltip" data-placement="bottom"
                        title="Allow users under this role to manage collaborators and assign specific roles"></i>
                    <label class="switch">
                        <asp:CheckBox ID="mgCollab" runat="server"  AutoPostBack="true" />
                        <span class="slider round"></span>
                    </label>
            </div>
            <div class="form-group">
                <label>Manage Vouchers</label>
                <i class="ttInfo fas fa-info-circle" data-html='true' data-toggle="tooltip" data-placement="bottom"
                    title="Allow users under this role to manage vouchers"></i>
                <label class="switch">
                    <asp:CheckBox ID="mgVouch" runat="server"  AutoPostBack="true" />
                    <span class="slider round"></span>
                </label>
            </div>
            <div class="form-group">
                <div class="mt-2 align-bottom" style="text-align: right;">
                    <span class="text-muted font-italic mr-3">Fields left empty will <b>not</b> be updated</span>
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger mr-3" Text="Cancel" CausesValidation="false" UseSubmitBehavior="False" />
                    <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-success" Text="Save" CausesValidation="false" UseSubmitBehavior="False" />
                </div>
            </div>
        </div>

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
                    

                    <asp:TextBox type="email" ID="collabEmail" class="form-control"  aria-describedby="emailHelp" placeholder="Enter email" runat="server"></asp:TextBox>
                    <p>
                    <asp:Label for="roleChoice" runat="server" style="float:left;">Role:</asp:Label>
                    <asp:DropDownList ID="roleChoice" style="float:left; margin-left:2%; margin-bottom:3%;" class="btn btn-primary dropdown-toggle" runat="server" DataSourceID="SqlDataSource1" DataTextField="Roles" DataValueField="Roles">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [Id], [Roles] FROM [rolesTable]" ></asp:SqlDataSource>
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
