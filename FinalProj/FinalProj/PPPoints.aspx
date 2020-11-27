<%@ Page Title="" Language="C#" MasterPageFile="~/ProfilePage.Master" AutoEventWireup="true" CodeBehind="PPPoints.aspx.cs" Inherits="FinalProj.PPPoints" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script> // super fancy code to flip the chevron
        $(document).ready(function () {
            $('#collapseOne').on('show.bs.collapse', function () {
                $("#chevronHow").removeClass("fas fa-chevron-down")
                $("#chevronHow").addClass("fas fa-chevron-up")
            })

            $('#collapseOne').on('hide.bs.collapse', function () {
                $("#chevronHow").removeClass("fas fa-chevron-up")
                $("#chevronHow").addClass("fas fa-chevron-down")
            })
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <br>
    <div class="container">
        <div style="border: 1px solid lightgray; border-radius: 15px; padding: 10px;" class="row text-center">
            <div class="" style="width: 100%; font-size: 20px; margin-bottom: 0px">
                <div>
                    Points:
                    <asp:Label ID="lblPoints" ForeColor="DarkCyan" runat="server"></asp:Label>
                </div>
                <div>
                    <a style="font-size: 14px;" runat="server" autopostback="false" class="p-0 text-center text-muted btn btn-link" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">How to Earn?
                    <i id="chevronHow" style="font-size: 10px;" class="fas fa-chevron-down"></i>
                    </a>
                </div>
                <div style="font-size: 16px;" id="collapseOne" class="text-center collapse" aria-labelledby="headingOne" data-parent="#accordion">
                    Participating in events and giving feedback = <b>1 Point</b><br />
                    Organising an event that reaches an average rating under 2 stars = <b>1 Point</b><br />
                    Organising an event that reaches an average rating exceeding 2 stars = <b>2 Points</b>
                </div>
            </div>
        </div>
        <br>
        <div style="border: 1px solid lightgray; border-radius: 15px; padding: 10px;" class="row ">
            <div style="width: 100%; border-bottom: 1px solid lightgray;" class="row mx-0 pb-2">
                <div style="font-size: 20px;" class="col-7 col-md-4 col-sm-7">
                    Redeemed Vouchers 
                </div>
                <div class="text-right col-5 col-md-8 col-sm-5">
                    <asp:LinkButton href="/VoucherRedemption.aspx" CssClass="btn btn-success" ID="lbRedeem" runat="server">Redeem <i class="fas fa-arrow-right"></i></asp:LinkButton>
                </div>
            </div>
            <% if (vRedList.Count > 0)
                {
            %>
            <table class="table table-striped table-hover">
                <thead>
                    <tr>
                        <th scope="col">Voucher Name</th>
                        <th scope="col">Quantity</th>
                    </tr>
                </thead>
                <tbody>
                    <% for (int i = 0; i < vRedList.Count; i++)
                        {
                    %>
                    <tr>
                        <th scope="row"><%=vRedList[i].VoucherName%></th>
                        <td><%=vRedList[i].Quantity %></td>
                    </tr>
                    <%} %>
                </tbody>
            </table>
            <% }
                else
                { %>
            <div style="font-size: 16px;" class="m-3 font-italic text-muted text-center row">
                <div class="col-12">
                    No vouchers have been redeemed.
                </div>
            </div>
            <%} %>
        </div>
    </div>
</asp:Content>
