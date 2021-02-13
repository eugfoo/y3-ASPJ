<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="bAcc.aspx.cs" Inherits="FinalProj.WebForm5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        #adCont {
            margin:2%;
        }

    </style>
    <div id="adCont">
        <asp:Panel ID="PanelError" runat="server" Visible="false" CssClass="stuff alert alert-danger ">
            <asp:Label ID="errmsgTb" runat="server"></asp:Label>
        </asp:Panel>
        <div class="card">
            <div class="card-body">
                <h5 class="card-header">Ban Management</h5>
                <div class="row">
                    <div class="col-6" style="margin-top:1%;">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-5">
                                    <label for="formGroupExampleInput2">Email:</label>
                                </div>
                                <div class="col-5">
                                    <label for="formGroupExampleInput2">Reason:</label>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-5">
                                    <asp:TextBox ID="banEmail" type="text" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-5">
                                    <asp:TextBox ID="banreasonTB" type="text" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-2">
                                    <asp:Button ID="banAccBtn" style="margin-left:2%" CssClass="btn btn-danger" runat="server" Text="Ban Account" OnClick="banAccBtn_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <asp:GridView ID="blockedGrid" ShowHeaderWhenEmpty="true" class="table table-bordered table-striped table-hover" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" OnRowDeleting="blockedGrid_RowDeleting" EmptyDataText="No banned accounts">
                            <Columns>
                                <asp:BoundField DataField="userEmail" HeaderText="userEmail" SortExpression="userEmail" />
                                <asp:BoundField DataField="userName" HeaderText="userName" SortExpression="userName" />
                                <asp:BoundField DataField="Reason" HeaderText="Reason" SortExpression="Reason" />
                                <asp:BoundField DataField="DateTime" HeaderText="DateTime" SortExpression="DateTime" />

                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Delete" Text="Unban"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="btn btn-danger mr-3" />

                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="table table-bordered table-striped text-center" />
                            <HeaderStyle CssClass="text-center" />
                            <RowStyle CssClass="text-center" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [userEmail], [userName], [Reason], [DateTime] FROM [Blocked]"></asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
