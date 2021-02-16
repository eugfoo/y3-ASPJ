<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="RequestAccess.aspx.cs" Inherits="FinalProj.RequestAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #outsideBox {
            margin: auto;
            margin-top: 50px;
            margin-bottom: 50px;
            width: 700px;
        }

        .gvAdminCSS {
            padding: 5px 10px 5px 10px;
        }

        #btnCSS {
            padding: 0 20px 0 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="outsideBox">
        <div id="adminDiv" runat="server">
            <asp:GridView ID="gvAdmin" CssClass="gvAdminCSS" runat="server" AutoGenerateColumns="False" ShowHeader="True" OnRowCommand="btnVerify_Click">
                <Columns>
                    <asp:BoundField DataField="subAdminEmail" HeaderText="Sub-Admin Email" />
                    <asp:BoundField DataField="requestRole" HeaderText="Requested Role" />
                    <asp:BoundField DataField="currentRole" HeaderText="Current Role" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div id="btnCSS">
                                <asp:Button ID="btnVerify" CssClass="btn btn-success" runat="server" Text="Verify" CommandName="Verify" CommandArgument='<%# Container.DataItemIndex %>' />
                                <asp:Button ID="btnDecline" CssClass="btn btn-danger" runat="server" Text="Decline" CommandName="Decline" CommandArgument='<%# Container.DataItemIndex %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
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
                            <asp:DropDownList ID="roleDDL" class="btn btn-primary dropdown-toggle" DataSourceID="SqlDataSource1" DataTextField="Roles" DataValueField="Roles" runat="server" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="roleDDL_SelectedIndexChanged">
                                <Items>
                                    <asp:ListItem Text="Select a Role" Value=""></asp:ListItem>
                                </Items>
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
