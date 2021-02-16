<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="forumPostEvent.aspx.cs" Inherits="FinalProj.forumPostEvent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://account.snatchbot.me/script.js"></script>
    <script>window.sntchChat.Init(91154)</script>

    <style type="text/css">
        .author-col {
            min-width: 12em;
        }

        .post-col {
            min-width: 20em;
        }

        .auto-style3 {
            width: 233px;
        }

        .auto-style4 {
            min-width: 12em;
            width: 233px;
        }
    </style>
    <div class="container my-3">
        <nav class="breadcrumb">
            <a href="forumPage1.aspx" class="breadcrumb-item">Board index</a>
            <a href="forumCatEventOverview.aspx" class="breadcrumb-item">Forum Event Threads</a>
            <span class="breadcrumb-item active">
                <asp:Label ID="LblPrefix" runat="server"></asp:Label>
                <asp:Label ID="LblTitleBreadcrumb" runat="server"></asp:Label></span>
        </nav>
        <div class="row">
            <div class="col-12">

                <div class="container">
                    <div class="row text-white bg-info mb-0 p-4 rounded-top">
                        <div class="col-md-9">
                            <h2 class="h4">
                                <asp:Label ID="LblTitleBig" runat="server"></asp:Label></h2>
                        </div>

                    </div>
                </div>
                <table class="table table-striped table-bordered table-responsive-lg">
                    <thead class="thead-light">
                        <tr>
                            <th scope="col" class="auto-style3"></th>
                            <th scope="col"></th>
                        </tr>
                    </thead>

                    <tbody>
                        <tr>
                            <td class="auto-style4 table-secondary" style="text-align: center; font-size: 20px;">
                                <div>
                                    <a href="PPGallery.aspx?userId=<%= currentThreadUser.id%>"><strong><%= currentThreadUser.name%></strong></a>
                                </div>
                            </td>
                            <td class="post-col d-lg-flex justify-content-lg-between table-secondary" style="height: 60px;">
                                <div class="container">
                                    <div class="row">
                                        <div class="col-md-8">
                                            <span class="font-weight-bold">Post subject:</span>
                                            <asp:Label ID="LblTitle" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <span class="font-weight-bold">Posted:</span>
                                            <asp:Label ID="LblPostDate" runat="server" Text="02 Apr 2019"></asp:Label>

                                        </div>
                                    </div>
                                </div>

                                <div>
                                </div>
                                <div></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 255px;">
                                <img src="<%=currentThreadUser.DPimage %>" name="aboutme" width="140" height="140" class="rounded-circle mb-4" style="margin-left: 50px;margin-top:15px;">
                                <div>
                                    <asp:Panel ID="organiserPanel" runat="server">
                                        <div style="font-size:20px;font-weight:700;text-align:center;margin:15px;">
                                            <asp:Label ID="orgLabel" runat="server" ForeColor="Green">[Organiser]</asp:Label>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div><%= currentThreadUser.desc %></div>
                                <div style="margin-top: 70px;"><span class="font-weight-bold">Joined: </span><%= currentThreadUser.regDate %></div>
                                <div>
                                    <a href="threadCreaterOverview.aspx?userId=<%= currentThreadUser.id %>"><strong style="font-weight: 700;">Posts </strong></a>:
                                    <asp:Label ID="LblThreadsCount" runat="server"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <asp:ListView ID="LVImages" runat="server">
                                    <ItemTemplate>
                                        <div class="container ml-4">
                                            <div class="row">
                                                <div class="col">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# "~/Img/"+ Eval("threadImage1") %>' Height="246px" Width="329px" onerror="this.style.display='none';" />
                                                </div>
                                                <div class="col">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl='<%# "~/Img/"+ Eval("threadImage2") %>' Height="246px" Width="329px" onerror="this.style.display='none';" />
                                                </div>
                                            </div>
                                            <div class="row mt-3">
                                                <div class="col">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl='<%# "~/Img/"+ Eval("threadImage3") %>' Height="246px" Width="329px" onerror="this.style.display='none';" />
                                                </div>
                                                <div class="col">
                                                    <asp:Image ID="Image4" runat="server" ImageUrl='<%# "~/Img/"+ Eval("threadImage4") %>' Height="246px" Width="329px" onerror="this.style.display='none';" />
                                                </div>
                                            </div>
                                        </div>

                                    </ItemTemplate>
                                </asp:ListView>


                                <br />
                                <asp:Label ID="LblContent" runat="server"></asp:Label>


                                <br />

                                <br />
                                <br />
                                <br />

                                &nbsp;&nbsp;<br />
                                <br />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <asp:Panel ID="panelEdit" Visible="false" runat="server">
                    <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-danger float-right" Text="Back" OnClick="btnGoBack_Click" />
                    <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary float-right mr-1" Text="Edit" OnClick="btnEdit_Click" />
                </asp:Panel>




                <asp:ScriptManager ID="MainScriptManager" runat="server" />



                <asp:HiddenField ID="HFuser_id" runat="server" />



                <asp:UpdatePanel ID="pnlHelloWorld" runat="server">
                    <ContentTemplate>

                        <asp:Repeater ID="rptrComments" runat="server">
                            <HeaderTemplate>
                                <br>
                                <br />
                                <br />
                                <table class="table table-striped table-bordered">

                                    <%--<thead class="thead-light">
                                        <tr>
                                            <th scope="col" class="auto-style3"></th>
                                            <th scope="col"></th>
                                        </tr>
                                    </thead>--%>
                                    <tbody>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr>
                                    <td class="auto-style4 table-active" style="text-align: center; font-size: 15px;">
                                        <div><a href="PPGallery.aspx?userId=<%# Eval("user_id") %>"><strong><%# Eval("user_name")%></strong></a></div>
                                    </td>
                                    <td class="post-col d-lg-flex justify-content-lg-between table-active" style="font-size: 15px;">
                                        <div class="container">
                                            <div class="row">
                                                <div class="col-md-8">
                                                    <span class="font-weight-bold">Replied To : <%# Eval("postTitle") %></span>
                                                    <%--<asp:Label ID="LblTitle" runat="server"></asp:Label>--%>
                                                </div>
                                                <div class="col-md-4">
                                                    <span class="font-weight-bold">Posted: <%# Eval("postDate") %></span>
                                                    <%--<asp:Label ID="LblPostDate" runat="server"><%# Eval("postDate") %></asp:Label>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="max-height: 456px; height: 350px;">
                                    <td style="width: 255px;">
                                        <img src="<%# Eval("userDP") %>" name="aboutme" width="140" height="140" class="rounded-circle mb-4" style="margin-left: 50px;margin-top:15px;">
                                        <div style="font-weight:700;font-size:20px;margin:15px;color:green;text-align:center;"><%# Eval("orgTag") %></div>
                                        <div><%# Eval("userDesc") %></div>
                                        <div style="margin-top: 70px;"><span class="font-weight-bold">Joined: </span><%# Eval("userJoinedDate") %></div>
                                        <div><a href="threadCreaterOverview.aspx?userId=<%# Eval("user_id") %>"><strong style="font-weight: 700;">Posts </strong></a>: <%# Eval("userThreadCount") %></div>

                                    </td>
                                    <td>
                                        <asp:Label ID="LblContent" runat="server"><%# Eval("postContent") %></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>

                            <FooterTemplate>
                                </tbody>
                                    </table>
                            </FooterTemplate>

                        </asp:Repeater>

                        <div style="margin-top: 20px;">

                            <table style="width: 485px; float: right;">
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lbFirst"
                                            Style="padding: 8px; margin: 2px; background: #6b91ab; border: solid 1px #d5e3ed; color: white; font-weight: bold"
                                            runat="server" OnClick="lbFirst_Click">First</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lbPrevious" runat="server"
                                            Style="padding: 8px; margin: 2px; background: #6b91ab; border: solid 1px #d5e3ed; color: white; font-weight: bold"
                                            OnClick="lbPrevious_Click">Previous</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:DataList ID="rptPaging" runat="server"
                                            OnItemCommand="rptPaging_ItemCommand"
                                            OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbPaging" runat="server"
                                                    CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="newPage"
                                                    Text='<%# Eval("PageText") %> ' Width="20px">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lbNext" runat="server"
                                            Style="padding: 8px; margin: 2px; background: #6b91ab; border: solid 1px #d5e3ed; color: white; font-weight: bold"
                                            OnClick="lbNext_Click">Next</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lbLast" runat="server"
                                            Style="padding: 8px; margin: 2px; background: #6b91ab; border: solid 1px #d5e3ed; color: white; font-weight: bold"
                                            OnClick="lbLast_Click">Last</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblpage" runat="server" Text="" Style="background: #6b91ab; padding: 10px; color: white"></asp:Label>
                                    </td>
                                </tr>
                            </table>



                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>







                <style>
                    .threadBox {
                        background-color: #A9A9A9;
                    }
                </style>
            </div>
        </div>

        <asp:Panel ID="replyPanel" runat="server">
            <div class="form-group">
                <label for="comment">Reply to this post:</label>
                 <asp:Label ID="v_spamCheckMessage" runat="server" Text="" class="alert-danger" role="alert"></asp:Label>
                <asp:TextBox ID="tbReplyContent" runat="server" CssClass="form-control" Height="250px" TextMode="MultiLine"></asp:TextBox>
                <asp:Label ID="LblMsg" runat="server" ForeColor="Red"></asp:Label>
            </div>


            <asp:Button ID="btnReply" runat="server" Text="Reply" CssClass="btn btn-primary" OnClick="btnReply_Click" />
            <button type="reset" class="btn btn-danger">Clear</button>
        </asp:Panel>

        <asp:HiddenField ID="HFthreadId" runat="server" />


        <asp:HiddenField ID="HFDate" runat="server" />

        <%--<thead class="thead-light">
                                        <tr>
                                            <th scope="col" class="auto-style3"></th>
                                            <th scope="col"></th>
                                        </tr>
                                    </thead>--%>
    </div>




</asp:Content>
