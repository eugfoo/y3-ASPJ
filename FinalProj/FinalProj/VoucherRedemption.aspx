<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="VoucherRedemption.aspx.cs" Inherits="FinalProj.VoucherRedemption" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="Voucher.css" />

    <script type="text/javascript">
        function btnClick() {
            var uri = window.location.toString();
            if (uri.indexOf("?") > 0) {
                var clean_uri = uri.substring(0, uri.indexOf("?"));
                window.history.replaceState({}, document.title, clean_uri);
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%">
        <h2 id="title">Voucher Redemption</h2>
        <div id="pointDiv">
            <h3>Your points: </h3>
            <h3 id="numPoints"><%=points %></h3>
        </div>
        <asp:Panel ID="panelSuccess" Visible="false" runat="server" CssClass="alert alert-dismissable alert-success">
            <asp:Label ID="lb_success" runat="server">Redeemed Voucher Successfully!</asp:Label>
        </asp:Panel>

        <asp:Panel ID="panelError" Visible="false" runat="server" CssClass="alert alert-dismissable alert-danger">
            <asp:Label ID="lb_error" runat="server">You have too little points..</asp:Label>
        </asp:Panel>

        <a href="/PPPoints.aspx" id="btnHref" class="btn btn-primary">Back to profile</a>

        <h2 id="no" runat="server" visible="false">Currently there are no vouchers available :( <br /> Come back and check again soon!</h2>

        <%foreach (var element in vcherList)
            { %>
        <table id="voucherTable">

            <tr id="voucherRow">
                <td>
                    <img id="voucherImg" src="/Img/<%=element.VoucherPic %>" />
                </td>
                <td id="tdRepeat">
                    <p id="voucherName"><%=element.VoucherName %></p>
                    <p id="amountP">Amount: </p>
                    <p id="voucherAmount">$<%=element.VoucherAmount %></p>
                    <a id="btnRedeem" href="/VoucherRedemption.aspx?voucherId=<%=element.VoucherId %>" class="btn btn-primary"><%= element.VoucherPoints %> Points&rarr;</a>
                </td>
            </tr>
        </table>
        <% } %>

    </div>
</asp:Content>
