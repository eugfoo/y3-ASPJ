<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="bAcc.aspx.cs" Inherits="FinalProj.WebForm5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="blockedGrid" class="table table-bordered table-striped table-hover" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" OnRowDeleting="blockedGrid_RowDeleting">
        <Columns>
            <asp:BoundField DataField="userEmail" HeaderText="userEmail" SortExpression="userEmail" />
            <asp:BoundField DataField="userName" HeaderText="userName" SortExpression="userName" />
            <asp:BoundField DataField="Reason" HeaderText="Reason" SortExpression="Reason" />
            <asp:BoundField DataField="DateTime" HeaderText="DateTime" SortExpression="DateTime" />

            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Delete" Text="Unblock"></asp:LinkButton>
                </ItemTemplate>
                <ControlStyle CssClass="btn btn-danger mr-3" />

            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="text-center" />
        <RowStyle CssClass="text-center" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [userEmail], [userName], [Reason], [DateTime] FROM [Blocked]"></asp:SqlDataSource>
</asp:Content>
