<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="eventFeedback.aspx.cs" Inherits="FinalProj.eventFeedback" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .flex-wrapper {
            background-image: url("Img/feedbackPG.jpg");
            background-position: center;
            background-size: auto;
        }

        .starRating {
            width: 40px;
            height: 40px;
            cursor: pointer;
            background-repeat: no-repeat;
            background-size: cover;
            display: block;
        }

        .FilledStars {
            background-image: url("Img/filledStar.png");
        }

        .WaitingStars {
            background-image: url("Img/waitingStar.png");
        }

        .EmptyStars {
            background-image: url("Img/emptystar.png");
        }
    </style>
    <div class="container">
    <div class="card" style="margin-top: 45px;">
        <div class="card-header" style="background-color: red;">
            <h2 style="color: white;">Feedback</h2>
        </div>
        <table class="table table-striped">

            <tbody>
                <tr>
                    <td>
                        <h5>How was the meetup?</h5>
                        <small>The coordination was efficient, event organiser was friendly, kind, punctual .etc</small>
                    </td>
                    <td>
                        <div style="margin-top: 8px;">
                            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <ajaxToolkit:Rating ID="Q1Rating" runat="server"
                                        StarCssClass="starRating"
                                        FilledStarCssClass="FilledStars"
                                        EmptyStarCssClass="EmptyStars"
                                        WaitingStarCssClass="WaitingStars"
                                        MaxRating="5">
                                    </ajaxToolkit:Rating>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h5>Are you likely to participate in this event again in the future?
                        </h5>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <ajaxToolkit:Rating ID="Q2Rating" runat="server"
                                    StarCssClass="starRating"
                                    FilledStarCssClass="FilledStars"
                                    EmptyStarCssClass="EmptyStars"
                                    WaitingStarCssClass="WaitingStars"
                                    MaxRating="5">
                                </ajaxToolkit:Rating>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h5>Do you think the event met its goal?
                        </h5>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <ajaxToolkit:Rating ID="Q3Rating" runat="server"
                                    StarCssClass="starRating"
                                    FilledStarCssClass="FilledStars"
                                    EmptyStarCssClass="EmptyStars"
                                    WaitingStarCssClass="WaitingStars"
                                    MaxRating="5">
                                </ajaxToolkit:Rating>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h5>How likely are you to tell a friend about this event?
                        </h5>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <ajaxToolkit:Rating ID="Q4Rating" runat="server"
                                    StarCssClass="starRating"
                                    FilledStarCssClass="FilledStars"
                                    EmptyStarCssClass="EmptyStars"
                                    WaitingStarCssClass="WaitingStars"
                                    MaxRating="5">
                                </ajaxToolkit:Rating>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>


            </tbody>


        </table>
        <div class="form-group" style="background-color: #f2f2f2; padding: 20px;">
            <h5>Please write a review for the Event : 
                <asp:Label ID="LblEventTitle" runat="server" style="font-size:35px; color:red;"></asp:Label>
            </h5>
            <small>What, if anything, did you dislike about this event? What elements of the event did you like the most?
Are you satisfied with the event?</small>
            <asp:TextBox ID="tbFeedbackContent" runat="server" CssClass="form-control" Height="200px" TextMode="MultiLine"></asp:TextBox>

        </div>
        <div class="container mb-4">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />

            <%-- <button type="submit" class="btn btn-primary">Submit</button>--%>
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-danger" OnClick="btnClear_Click" />
        </div>
        </div>
    </div>




</asp:Content>
