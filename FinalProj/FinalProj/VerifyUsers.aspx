<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="VerifyUsers.aspx.cs" Inherits="FinalProj.VerifyUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #outsideBox {
            margin: auto;
            margin-top: 50px;
            width: 1050px;
        }

        #btnCSS {
            padding: 0 20px 0 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="outsideBox">
                <asp:GridView ID="Gv_imgs" CssClass="grid" runat="server" AutoGenerateColumns="false" ShowHeader="True">
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
                                <div id="btnCSS">
                                    <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" OnClick="btnVerify_Click" Text="Verify" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
