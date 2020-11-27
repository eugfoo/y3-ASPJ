<%@ Page Title="" Language="C#" MasterPageFile="~/ProfilePage.Master" AutoEventWireup="true" CodeBehind="PPRating.aspx.cs" Inherits="FinalProj.PPRating" %>

<%@ Import Namespace="FinalProj.BLL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style>
        .fdback-link {
            text-decoration: none !important;
            transition: text-shadow 0.2s;
        }

            .fdback-link:hover {
                text-decoration: none !important;
            }

        .fdback-rating {
            font-size: 20px;
            color: yellow;
        }

        .fdback-desc {
            font-size: 16px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div class="container">
        <br />
        <div style="border: 1px solid lightgray; border-radius: 15px; padding: 10px;" class="row ">
            <div style="width: 100%; border-bottom: 2px solid lightgray;" class="row mx-0 pb-2">
                <div style="font-size: 20px;" class="fdback-head col-12 col-md-2 col-sm-2">
                    Feedback
                </div>

                <div class="fdback-head col-12 col-md-10 col-sm-10">
                    <label for="ddlEvents" class="col-form-label">Event:</label>
                    <asp:DropDownList AutoPostBack="True" ID="ddlEvents" OnSelectedIndexChanged="ddlEvents_OnSelectedIndexChanged" runat="server"></asp:DropDownList>
                    <a style="" id="lbEvent" runat="server" class="text-success">
                        <i class="fas fa-arrow-alt-circle-right"></i>
                    </a>
                </div>
            </div>

            <!-- Feedback Card -->
            <% Users user = new Users();
                if (feedbackList.Count > 0)
                { %>
            <% for (int i = 0; i < feedbackList.Count; i++)
                {
                    if (i != feedbackList.Count - 1)
                    {
            %>
            <div style="width: 100%; border-bottom: 1px solid lightgray;" class="pb-2 row mt-2 mx-0 mb-2">
                <div class="col-6 col-sm-6 col-md-4">
                    <div style="position: absolute; top: 50%; -ms-transform: translateY(-50%); transform: translateY(-50%);"
                        class="">
                        <img style="border-radius: 100%; width: 60px; height: 60px;" src="<%=user.GetUserById(feedbackList[i].UserId).DPimage%>" class="img img-thumbnail" />
                        <a href="/PPGallery.aspx?userId=<%=feedbackList[i].UserId%>" class="fdback-link text-dark ml-2"><b><%=user.GetUserById(feedbackList[i].UserId).name%></b></a>
                    </div>
                </div>
                <div class="col-6 col-sm-6 col-md-8">
                    <div class="row">
                        <div class="fdback-rating">
                            <% for (int j = 0; j < feedbackList[i].AvgRating; j++)
                                { %>
                            <span><i class="text-warning fa fa-star"></i></span>
                            <% } %>
                        </div>

                    </div>
                    <div class="row">
                        <div class="fdback-desc">
                            <%=feedbackList[i].UserReview%>
                        </div>
                    </div>
                </div>
            </div>
            <%   }
                else // The last row.
                { %>
            <div style="width: 100%;" class="row mt-2 mx-0 mb-2">
                <div class="col-6 col-sm-6 col-md-4">
                    <div style="position: absolute; top: 50%; -ms-transform: translateY(-50%); transform: translateY(-50%);"
                        class="">
                        <img style="border-radius: 100%; width: 60px; height: 60px;" src="<%=user.GetUserById(feedbackList[i].UserId).DPimage%>" class="img img-thumbnail" />
                        <a href="/PPGallery.aspx?userId=<%=feedbackList[i].UserId%>" class="fdback-link text-dark ml-2"><b><%=user.GetUserById(feedbackList[i].UserId).name%></b></a>
                    </div>
                </div>
                <div class="col-6 col-sm-6 col-md-8">
                    <div class="row">
                        <div class="fdback-rating">
                            <% for (int j = 0; j < feedbackList[i].AvgRating; j++)
                                { %>
                            <span><i class="text-warning fa fa-star"></i></span>
                            <% } %>
                        </div>

                    </div>
                    <div class="row">
                        <div class="fdback-desc">
                            <%=feedbackList[i].UserReview%>
                        </div>
                    </div>
                </div>
            </div>
            <% }
                }
            %>
            <% }
                else
                { %>
            <div style="font-size:16px;" class="m-3 font-italic text-muted text-center row">
                <div class="col-12">
                    No feedback received yet.
                </div>
            </div>

            <%}%>
            <!-- Feedback Card -->

        </div>
        <br />

        <!-- Recommendation Card -->
        <%--<div style="border: 1px solid lightgray; border-radius: 15px; padding: 10px;" class="row ">
            <div style="width: 100%; border-bottom: 1px solid lightgray;" class="row mx-0 pb-2">
                <div class="col-md-6">
                    Recommendations
                    <button class="btn btn-warning">Request</button>
                </div>
            </div>
            <div style="padding: 10px; width: 100%; background-color: lightcoral;" class="row mx-0 mb-2">
                <div class="col-md-4">
                    <img src="" class="img img-thumbnail" />Cove E
                </div>
                <div class="col-md-8">
                    <div class="row">
                        Three to the one from the one to the three
                    </div>
                </div>
            </div>
            <div style="padding: 10px; width: 100%; background-color: lightcoral;" class="row mx-0 mb-2">
                <div class="col-md-4">
                    <img src="" class="img img-thumbnail" />Cove E
                </div>
                <div class="col-md-8">
                    <div class="row">
                        Three to the one from the one to the three
                    </div>
                </div>
            </div>
            <div style="padding: 10px; width: 100%; background-color: lightcoral;" class="row mx-0 mb-2">
                <div class="col-md-4">
                    <img src="" class="img img-thumbnail" />Cove E
                </div>
                <div class="col-md-8">
                    <div class="row">
                        Three to the one from the one to the three
                    </div>
                </div>
            </div>
            <div style="padding: 10px; width: 100%; background-color: lightcoral;" class="row mx-0 mb-2">
                <div class="col-md-4">
                    <img src="" class="img img-thumbnail" />Cove E
                </div>
                <div class="col-md-8">
                    <div class="row">
                        Three to the one from the one to the three
                    </div>
                </div>
            </div>
        </div>--%>
        <!-- Recommendation Card -->

    </div>
</asp:Content>
