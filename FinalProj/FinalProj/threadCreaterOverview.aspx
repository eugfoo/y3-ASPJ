<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="threadCreaterOverview.aspx.cs" Inherits="FinalProj.threadCreaterOverview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://account.snatchbot.me/script.js"></script>
    <script>window.sntchChat.Init(91154)</script>
    <div class="container mt-4">
        <div class="card card-body bg-light">
            <center>
        <a href="#aboutModal" data-toggle="modal" data-target="#myModal"><img src="<%=currentThreadUser.DPimage%>" name="aboutme" width="140" height="140" class="img-circle"></a>
        <h3><%=currentThreadUser.name%></h3>
        <em>click on my picture for more!</em>
		</center>
        </div>
        <!-- Modal -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                        <h4 class="modal-title" id="myModalLabel">More About <%=currentThreadUser.name%></h4>
                    </div>
                    <div class="modal-body">
                        <center>
                    <img src="<%=currentThreadUser.DPimage%>" name="aboutme" width="140" height="140" border="0" class="img-circle"></a>
                    <h3 class="media-heading"><%=currentThreadUser.name%></h3>
                   <%-- <span><strong>Skills: </strong></span>
                        <span class="label label-warning">HTML5/CSS</span>
                        <span class="label label-info">Adobe CS 5.5</span>
                        <span class="label label-info">Microsoft Office</span>
                        <span class="label label-success">Windows XP, Vista, 7</span>--%>
                    </center>
                        <hr>
                        <center>
                    <p class="text-left"><strong>Bio: </strong><br>
                        <%=currentThreadUser.desc%></p>
                    <br>
                    </center>
                    </div>
                    <div class="modal-footer">
                        <center>
                    <button type="button" class="btn btn-default" data-dismiss="modal">I've heard enough about Joe</button>
                    </center>
                    </div>
                </div>
            </div>
        </div>


        <asp:Repeater ID="rptrThreads" runat="server">
            <HeaderTemplate>
                <table class="table table-striped table-bordered table-responsive-lg">
                    <thead class="thead-light">
                        <tr>
                            <th scope="col" class="topic-col">Topic</th>
                            <th scope="col" class="created-col">Created</th>
                            <th scope="col" style="min-width: 6em;">Total Replies</th>
                            <th scope="col" class="last-post-col">Last Reply</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="width: 600px;">
                        <h3 class="h6"><span class="badge badge-<%# Eval("threadBadgeColor") %>"><%# Eval("threadPrefix") %></span> <a href="forumPost.aspx?threadid=<%# Eval("Id") %>"><%# Eval("threadTitle") %></a></h3>
                        <div class="small">
                            Started by: <%# Eval("user_name") %>
                        </div>
                    </td>

                    <td>
                        <div>by <a href="PPGallery.aspx?userId=<%# Eval("user_id") %>"><%# Eval("user_name") %></a></div>
                        <div><%# Eval("threadDate") %></div>
                    </td>
                    <td style="text-align: center;">
                        <div style="margin-top: 10px;"><%# threadIdReplies[Convert.ToInt32(Eval("Id"))] %> replies</div>
                    </td>
                    <td>
                        <div><%# threadIdUserIdReplies[Convert.ToInt32(Eval("Id"))] %></div>
                        <div><%# threadIdLastReplyDateT[Convert.ToInt32(Eval("Id"))] %></div>
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

    </div>
    <asp:HiddenField ID="HFuserId" runat="server" />
</asp:Content>
