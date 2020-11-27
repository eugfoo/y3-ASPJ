<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="forumPage1.aspx.cs" Inherits="FinalProj.forumPage1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://account.snatchbot.me/script.js"></script>
    <script>window.sntchChat.Init(91154)</script>

    <style type="text/css">
        .forum-col {
            min-width: 16em;
        }

        .last-post-col {
            min-width: 12em;
        }

        .event-topic-col {
            min-width: 23em;
        }

        .auto-style1 {
            height: 62px;
        }
    </style>
    <div class="container my-3">
        <nav class="breadcrumb">
            <span class="breadcrumb-item active">Board Index</span>
        </nav>
        <div class="row">
            <div class="col-12 col-xl-9">
                <div class="container">
                    <div class="row text-white bg-danger mb-0 p-4 rounded-top">
                        <div class="col-md-9">
                            <h2 class="h4">UP-COMING EVENTS</h2>
                        </div>
                        <div class="col-md-3">
                            <a class="btn btn-dark" href="forumCatEventOverview.aspx" role="button">View Threads</a>
                        </div>
                    </div>
                </div>

                <asp:Repeater ID="rptrConfirmedThreads" runat="server">
                    <HeaderTemplate>
                        <table class="table table-striped table-bordered table-responsive-lg">
                            <thead class="thead-light">
                                <tr>
                                    <th scope="col" class="topic-col">Threads</th>
                                    <th scope="col" class="created-col">Created</th>
                                    <th scope="col" style="min-width: 6em;">Total Replies</th>
                                    <th scope="col" class="last-post-col">Last Reply</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="width: 320px;">
                                <h3 class="h6"><span class="badge badge-<%# Eval("threadBadgeColor") %>"><%# Eval("threadPrefix") %></span> <a href="forumPostEvent.aspx?threadId=<%# Eval("Id") %>"><%# Eval("threadTitle") %></a></h3>
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

                <div class="container">
                    <div class="row text-white bg-info mb-0 p-4 rounded-top">
                        <div class="col-md-9">
                            <h2 class="h4">ClearView Forum</h2>
                        </div>
                        <div class="col-md-3">
                            <a class="btn btn-dark" href="forumCatOverview.aspx" role="button">View Threads</a>
                        </div>
                    </div>
                </div>

                <asp:Repeater ID="rptrThreads" runat="server">
                    <HeaderTemplate>
                        <table class="table table-striped table-bordered table-responsive-lg">
                            <thead class="thead-light">
                                <tr>
                                    <th scope="col" class="topic-col">Threads</th>
                                    <th scope="col" class="created-col">Created</th>
                                    <th scope="col" style="min-width: 6em;">Total Replies</th>
                                    <th scope="col" class="last-post-col">Last Reply</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="width: 320px;">
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
            </div>

            <div class="col-12 col-xl-3">
                <aside>
                    <div class="row">
                        <div class="col-12 col-sm-6 col-xl-12">
                            <div class="card mb-3 mb-sm-0 mb-xl-3">
                                <%--<div class="card-body">
                                    <h2 class="h4 card-title">Members online</h2>
                                    <ul class="list-unstyled mb-0">
                                        <li><a href="#0">Forum member name</a></li>
                                        <li><a href="#0">Forum member name</a></li>
                                        <li><a href="#0">Forum member name</a></li>
                                        <li><a href="#0">Forum member name</a></li>
                                        <li><a href="#0">Forum member name</a></li>
                                        <li><a href="#0">Forum member name</a></li>
                                        <li><a href="#0">Forum member name</a></li>
                                    </ul>
                                </div>--%>
                               <%-- <div class="card-footer">
                                    <dl class="row mb-0">
                                        <dt class="col-8">Total:</dt>
                                        <dd class="col-4 mb-0">10</dd>
                                    </dl>
                                    <dl class="row mb-0">
                                        <dt class="col-8">Members:</dt>
                                        <dd class="col-4 mb-0">7</dd>
                                    </dl>
                                    <dl class="row mb-0">
                                        <dt class="col-8">Guests:</dt>
                                        <dd class="col-4 mb-0">3</dd>
                                    </dl>
                                </div>--%>
                            </div>
                        </div>

                        <div class="col-12 col-sm-6 col-xl-12">
                            <div class="card">
                                <div class="card-body">
                                    <h2 class="h4 card-title">Forum statistics</h2>
                                    <dl class="row mb-0">
                                        <dt class="col-8">Total Threads:</dt>
                                        <dd class="col-4 mb-0"><%= allthreadsList.Count() %></dd>
                                    </dl>
                                    <dl class="row mb-0">
                                        <dt class="col-8">Total Replies:</dt>
                                        <dd class="col-4 mb-0"><%= allthreadReplies.Count() %></dd>
                                    </dl>
                                    <dl class="row mb-0">
                                        <dt class="col-8">Total members:</dt>
                                        <dd class="col-4 mb-0"><%= allUsersList.Count() %></dd>
                                    </dl>
                                </div>
                                <div class="card-footer">
                                    <div>Newest Member</div>
                                    <div><a href="PPGallery.aspx?userId=<%= LastUser.id %>"><%= LastUser.name %></a></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </aside>
            </div>
        </div>
    </div>


</asp:Content>
