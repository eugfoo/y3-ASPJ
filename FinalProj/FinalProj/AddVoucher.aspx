<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="AddVoucher.aspx.cs" Inherits="FinalProj.AddVoucher" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="AddVoucher.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%">
        <br>

        <asp:Panel ID="panelSuccess" Visible="false" runat="server" CssClass="alert alert-dismissable alert-success">
            <asp:Label ID="lb_success" runat="server">Deleted Voucher!</asp:Label>
        </asp:Panel>
        <asp:Panel ID="PanelError" Visible="false" CssClass="stuff alert alert-danger" runat="server">
            <asp:Label ID="errmsgTb" runat="server"></asp:Label>
        </asp:Panel>

        <h2 id="title">Add Vouchers (Admin)</h2>
        <div class="container" id="formContainer">
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-6">
                    <label for="voucherName">Voucher Name: </label>
                    <asp:TextBox ID="voucherName" CssClass="form-control" placeholder="FoodPanda $15 off" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-12 col-md-12 col-lg-6">
                    <label for="voucherAmt">Voucher Amount: </label>
                    <asp:TextBox ID="voucherAmt" CssClass="form-control" placeholder="15" runat="server"></asp:TextBox>
                </div>
                
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <label for="voucherName">Voucher Picture: </label>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </div>
            </div>
        </div>
        <div id="center">
            <asp:Button ID="createBtn" runat="server" Text="Add" OnClick="createBtn_Click"/>
        </div>

        <h2 id="voucherTitle">List of all vouchers</h2>

        <h2 id="no" runat="server" visible="false">No vouchers added!</h2>

        <%foreach (var element in vcherList)
            { %>
        <table id="voucherTable">

            <tr id="voucherRow">
                <td>
                    <img id="voucherImg" src="/Img/<%=element.VoucherPic %>" />
                </td>
                <td id="tdRepeat">
                    <p id="voucherName1"><%=element.VoucherName %></p>
                    <p id="voucherAmount">$<%=element.VoucherAmount %></p>
                    <a id="btnRedeem" href="/AddVoucher.aspx?voucherId=<%=element.VoucherId %>" class="btn btn-primary">Delete &rarr;</a>
                </td>
            </tr>
        </table>
        <% } %>

    </div>
</asp:Content>