<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="VerifyUsers.aspx.cs" Inherits="FinalProj.VerifyUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #outsideBox {
            margin: auto;
            margin-top: 30px;
            margin-bottom: 50px;
            width: 1100px;
        }

        #btnVerifyCSS, #btnDeclineCSS {
            padding: 5px 0 5px 0;
        }

        #labelBox {
            margin: auto;
            width: 200px;
        }

        .success {
            background-color: #4CAF50;
            border-radius: 5px;
            font-weight: 500;
            font-size: 20px;
        }

            .success:hover {
                background-color: #86d189;
                color: black;
            }

        .label {
            color: white;
            padding: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="labelBox">
        <asp:Label ID="lblNoUsers" runat="server" Text="No users to verify!" Visible="false" CssClass="label success"></asp:Label>
    </div>
    <div id="outsideBox">
        <asp:GridView ID="Gv_imgs" class="table table-bordered table-striped table-hover" runat="server" AutoGenerateColumns="false" ShowHeader="True" OnRowCommand="btnVerify_Click">
            <Columns>
                <asp:BoundField DataField="Text" HeaderText="Name" />
                <asp:TemplateField HeaderText="Admin Verification">
                    <ItemTemplate>
                        <asp:Image ID="img" runat="server" ImageUrl="~/Img/User/AdminFaceVerification/OGVerification.png" Width="320" Height="500" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ImageField DataImageUrlField="Value" ControlStyle-Height="500" ControlStyle-Width="320" HeaderText="User Submitted" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <div id="btnVerifyCSS">
                            <asp:Button ID="btnVerify" CssClass="btn btn-success" runat="server" Text="Verify" CommandName="Verify" CommandArgument='<%# Container.DataItemIndex %>' />
                            <asp:Button ID="btnDecline" CssClass="btn btn-danger" runat="server" Text="Decline" CommandName="Decline" CommandArgument='<%# Container.DataItemIndex %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
