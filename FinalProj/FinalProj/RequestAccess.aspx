<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="RequestAccess.aspx.cs" Inherits="FinalProj.RequestAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="adminDiv" runat="server">
        <asp:GridView ID="gvAdmin" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField HeaderText="Sub-Admin Email" />
                <asp:BoundField HeaderText="Requested Role" />
                <asp:BoundField HeaderText="Current Role" />
            </Columns>
        </asp:GridView>
    </div>
    <div id="subAdminDiv" class="card" runat="server">
        <div class="card-body">
            <div class="row">
                <div style="margin-top: 1%;" class="col-6">
                    <div class="row">
                        <div style="margin-top: 1%;" class="col-4">
                            <b class="lead">Request Role from Admin:</b>
                        </div>
                        <div style="margin-top: 1%;" class="col-8">
                            <asp:DropDownList ID="roleDDL" class="btn btn-primary dropdown-toggle" DataSourceID="SqlDataSource1" DataTextField="Roles" DataValueField="Roles" runat="server" AutoPostBack="True" OnSelectedIndexChanged="roleDDL_SelectedIndexChanged">
                                <asp:ListItem Selected="True">Select a Role</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="btnRequest" runat="server" CssClass="btn btn-success" Text="Request" OnClick="btnRequest_Click" />
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [Id], [Roles] FROM [rolesTable]"></asp:SqlDataSource>
                        </div>
                        <asp:Label ID="lblError" runat="server" Text="" CssClass="alert alert-danger" Visible="false"></asp:Label><br />
                        <asp:Label ID="lblSuccess" runat="server" Text="" CssClass="alert alert-success" Visible="false"></asp:Label>
                    </div>
                </div>

                <div style="margin-top: 1%;" class="col-6">
                    <div class="row">
                        <div style="margin-top: 1%;" class="col-4">
                            <b class="lead">Your Current Role:</b>
                        </div>
                        <div style="margin-top: 1%;" class="col-8">
                            <asp:Label ID="lblCurrentRole" runat="server" Text="" CssClass="alert alert-info"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
