<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="eventDetails.aspx.cs" Inherits="FinalProj.Enquiry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

	<style>
		#ContentPlaceHolder1_joinEvent {
			padding: 2%;
			width: 20%;
		}

		#ContentPlaceHolder1_descriptionTB {
		margin-bottom:6%;
		}
		#ContentPlaceHolder1_ongoing {
			padding: 2%;
			width: 20%;
		}

		#ContentPlaceHolder1_finished {
			padding: 2%;
			width: 25%;
		}

		#ContentPlaceHolder1_eventEnd {
			padding: 2%;
			width: 25%;
		}

		#ContentPlaceHolder1_fullAttendance {
			padding: 2%;
			width: 35%;
		}

		#ContentPlaceHolder1_panelError {
			text-align: center;
		}

		#ContentPlaceHolder1_panelSuccess {
			text-align: center;
		}

		#ContentPlaceHolder1_leaveEvent {
			padding: 2%;
			width: 20%;
		}

		#ContentPlaceHolder1_bookmarkEvent {
			padding: 2%;
			width: 30%;
		}

		#ContentPlaceHolder1_bookmarked {
			padding: 2%;
			width: 30%;
		}

		#editEvent {
			padding: 2%;
			width: 20%;
		}

		#ContentPlaceHolder1_attendance {
			padding: 2%;
			width: 30%;
		}




		#ContentPlaceHolder1_discussion {
			padding: 2%;
			width: 25%;
		}

		#ContentPlaceHolder1_discussion2 {
			padding: 2%;
			width: 25%;
		}

		#eventName {
			text-align: center;
			padding: 0.5%;
			width: 100%;
			color: white;
			margin-right: auto;
			background-color: #CB0000;
		}

		#eventPhoto {
			text-align: center;
		}

		#eventPic {
			width: 100%;
			height: auto;
			border-radius: 5%;
			margin: 2em auto;
		}

		.participants {
			list-style: none;
			margin: auto;
		}

		.participantsListed {
			margin-bottom: 1px;
			border-bottom: 1px solid gray;
			padding-bottom: 5%;
		}

		#eventDetails {
			margin: 2em auto;
		}

		@media only screen and (max-width: 990px) {
			#editEvent {
				width: 100%;
				height: auto
			}

			#ContentPlaceHolder1_bookmarked {
				margin-top: 1em;
				width: 100%;
				height: auto
			}

			#ContentPlaceHolder1_eventEnd {
				width: 100%;
				height: auto
			}

			#ContentPlaceHolder1_finished {
				width: 100%;
				height: auto
			}

			#ContentPlaceHolder1_ongoing {
				width: 100%;
				height: auto
			}

			#ContentPlaceHolder1_fullAttendance {
				width: 100%;
				height: auto
			}



			#ContentPlaceHolder1_attendance {
				margin-top: 1em;
				width: 100%;
				height: auto
			}

			#organiserDiv {
				width: 280px;
				margin: 0 auto;
			}

			#ContentPlaceHolder1_joinEvent {
				width: 100%;
				height: auto
			}

			#ContentPlaceHolder1_leaveEvent {
				width: 100%;
				height: auto
			}

			#ContentPlaceHolder1_bookmarkEvent {
				margin-top: 1em;
				width: 100%;
				height: auto
			}

			#ContentPlaceHolder1_discussion {
				margin-top: 1em;
				width: 100%;
				height: auto
			}

			#ContentPlaceHolder1_discussion2 {
				margin-top: 1em;
				width: 100%;
				height: auto
			}

			#contactOrganiser {
				text-align: center;
				width: 280px;
				margin: 0 auto;
			}

			.attendeeGroup {
				text-align: center;
			}

			#remainingSpots {
				text-align: center;
			}
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div style="min-height: 91vh;">

		<div class="container">

			<br />
			<asp:Panel ID="panelSuccess" Visible="false" runat="server" CssClass="alert alert-dismissable alert-success">
				<asp:Label ID="lb_success" runat="server"></asp:Label>
			</asp:Panel>
			<asp:Panel ID="panelError" Visible="false" runat="server" CssClass="alert alert-dismissable alert-danger">
				<asp:Label ID="lb_error" runat="server"></asp:Label>
			</asp:Panel>
			<div class="row">
				<div class="col-sm-12 col-md-12 col-lg-6" id="eventPhoto">
					<img id="eventPic" src="Img/<% = eventDetail.Pic %>" />
				</div>
				<div class="col-sm-12 col-md-12 col-lg-6" id="eventDetails">
					<p class="h1" id="eventName"><%=eventDetail.Title %></p>
					<div id="organiserDiv">
						<asp:Image Style="border-radius: 100%; width: 60px; height: 60px;" ID="imgDP" runat="server" /><span style="padding-left: 10px">organised by <a href="/PPGallery.aspx?userId=<%=eventDetail.User_id%>" style="text-decoration: none;"><%=userName %></a></span>


						<p style="padding-left: 70px;">
							<i class="fa fa-clock"></i>&nbsp;<span><%= eventDetail.Date%><br />
								<%= eventDetail.StartTime %> - <%= eventDetail.EndTime %></span>
							<br />
						</p>

						<p style="padding-left: 70px;"><i class="fa fa-map-marker-alt"></i>&nbsp;<span><%= eventDetail.Venue %></span></p>
						<p style="padding-left: 70px;"><i>Note:</i>&nbsp;<span><%= eventDetail.Note %></span></p>


						<span>
							<% if (userId != eventDetail.User_id)
								{
							%>
							<%FinalProj.BLL.Users user = (FinalProj.BLL.Users)Session["user"];
								bool btnCreated = false;
								if (user != null)
								{
									foreach (var participant in participantList)
									{
										if (participant.id == user.id)
										{
											btnCreated = true;%>

							<%if (DateTime.Now >= DateTime.Parse(eventDetail.Date.ToString() + " " + eventDetail.EndTime.ToString()))
								{%>
							<asp:Button ID="eventEnd" CssClass="btn btn-secondary" runat="server" Text="EVENT ENDED" Enabled="false" />

							<%}
								else if (DateTime.Now >= DateTime.Parse(eventDetail.Date.ToString() + " " + eventDetail.StartTime.ToString()))
								{%>
							<asp:Button ID="Button1" CssClass="btn btn-secondary" runat="server" Text="ONGOING" Enabled="false" />
							<%}
								else
								{
							%>
							<asp:Button ID="leaveEvent" CssClass="btn btn-danger" runat="server" Text="LEAVE" OnClick="leaveEvent_Click" />
							<%
											}
										}
									}
								}
								if (btnCreated == false)
								{
									if (DateTime.Now >= DateTime.Parse(eventDetail.Date.ToString() + " " + eventDetail.StartTime.ToString()))
									{
										if (DateTime.Now >= DateTime.Parse(eventDetail.Date.ToString() + " " + eventDetail.EndTime.ToString()))
										{%>
							<asp:Button ID="finished" CssClass="btn btn-secondary" runat="server" Text="EVENT ENDED" Enabled="false" />
							<%}
								else
								{ %>
							<asp:Button ID="ongoing" CssClass="btn btn-secondary" runat="server" Text="ONGOING" Enabled="false" />
							<%}
								}
								else
								{
									if (participantList.Count != eventDetail.MaxAttendees)
									{%>
							<asp:Button ID="joinEvent" CssClass="btn btn-warning" runat="server" Text="JOIN" OnClick="joinEvent_Click" />
							<%}
								else
								{ %>
							<asp:Button ID="fullAttendance" CssClass="btn btn-secondary" runat="server" Text="MAX CAPACITY" Enabled="False" />
							<%}
									}
								}
								if (bookmark == false)
								{%>

							<asp:Button ID="bookmarkEvent" CssClass="btn btn-primary" runat="server" Text="BOOKMARK+" OnClick="bookmarkEvent_Click" />
							<%}
								else
								{ %>
							<asp:Button ID="bookmarked" CssClass="btn btn-success" runat="server" Text="BOOKMARKED" OnClick="unbookmarkEvent_Click" />
							<%} %>
							<asp:Button ID="discussion" CssClass="btn btn-primary" runat="server" Text="DISCUSSION" OnClick="discussion_Click" />
							<%}
								else
								{%>
							<%if (DateTime.Now >= DateTime.Parse(eventDetail.Date.ToString() + " " + eventDetail.EndTime.ToString()) || DateTime.Now >= DateTime.Parse(eventDetail.Date.ToString() + " " + eventDetail.EndTime.ToString()))
								{ %>
							<%}
								else
								{ %>
							<a href="/editEventDetails.aspx?eventId=<%=eventDetail.EventId %>" id="editEvent" class="btn btn-warning">EDIT</a>
							<%} %>
							<asp:Button ID="attendance" class="btn btn-primary" runat="server" Text="ATTENDANCE" OnClick="attendance_Click" />

							<asp:Button ID="discussion2" class="btn btn-primary" runat="server" Text="DISCUSSION" OnClick="discussion2_Click" />
						</span>
						<%  
							}%>
					</div>
				</div>

				<div class="col-sm-12 col-md-12 col-lg-12">
					<ul class="nav nav-tabs">
						<li class="nav-item">
							<a class="nav-link active" href="#tab1" id="tab1Link" onclick="toggleTab1()">About</a>
						</li>
						<li class="nav-item">
							<a class="nav-link" href="#tab2" id="tab2Link" onclick="toggleTab2()">Attendees</a>
						</li>

					</ul>
				</div>


			</div>
			<div id="tab1" class="row">
				<div class="col-sm-12 col-md-12 col-lg-6" style="margin-top: 2em;">
					<asp:TextBox ID="descriptionTB" CssClass="form-control" runat="server" Visible="False" Width="100%" Height="250"  TextMode="MultiLine"></asp:TextBox>
					<%=eventDetail.Desc.Replace("\r\n", "<br/>") %>
				</div>
				<div class="col-sm-12 col-md-12 col-lg-6" style="margin-top: 2em;">
					<div id="contactOrganiser">
						<p class="h5">Contact Organiser</p>
						<asp:Image Style="border-radius: 100%; width: 60px; height: 60px;" ID="imgDPOrg" runat="server" /><span style="padding-left: 10px"><a href="/PPGallery.aspx?userId=<%=eventDetail.User_id%>" style="text-decoration: none;"><%=userName %></a><br />
							<br />
							Email: <%=profilePic.email %></span>
					</div>

					<p style="margin-top: 1em;" id="remainingSpots" class="h5">Remaining Spot(s) Available:<b style="color: #EC5D5D">&nbsp; <%= eventDetail.MaxAttendees - participantList.Count %></b></p>
					<div class="row attendeeGroup" style="margin-top: 3em;">
						<div class="col-sm-12 col-md-12 col-lg-12">
							<p class="h4" style="color: #EC5D5D;">Attendees (<%=participantList.Count %>)</p>
						</div>

						<% 
							for (int i = 0; i < participantList.Count; i++)
							{
								if (i == 4)
									break;
								{ %>

						<div class="col-3" style="text-align: center;">
							<a href="/PPGallery.aspx?userId=<%=listOfUserId[i]%>">
								<img style="border-radius: 100%; width: 60px; height: 60px;" src="<%=participantList[i].DPimage %>">

								<p><%= participantList[i].name %></p>
							</a>
						</div>

						<%}
							}%>
					</div>
					<div class="row attendeeGroup" style="margin-top: 3em;">
						<%  
							for (int i = 4; i < participantList.Count; i++)
							{

								if (i == 8)
									break;
								{ %>

						<div class="col-3" style="text-align: center;">
							<a href="/PPGallery.aspx?userId=<%=listOfUserId[i]%>">
								<img style="border-radius: 100%; width: 60px; height: 60px;" src="<%=participantList[i].DPimage %>">
								<p><%= participantList[i].name %></p>
							</a>
						</div>

						<%}
							}%>
					</div>
				</div>

			</div>

			<div id="tab2" style="display: none;" class="row">
				<div style="width: 50%; margin-top: 3em; margin-right: auto; margin-left: auto;">
					<p class="h3">All Attendees</p>

					<div class="p-1 bg-light rounded rounded-pill shadow-sm">
						<div class="input-group">
							<input type="search" placeholder="What're you searching for?" id="mySearchParticipants" onkeyup="mySearchParticipant();" aria-describedby="button-addon1" class="form-control border-1 bg-light">
							<div class="input-group-append">
								<button id="button-addon1" type="submit" class="btn btn-link border-1 text-primary"><i class="fa fa-search"></i></button>
							</div>
						</div>

					</div>


					<div style="margin-top: 2em; text-align: center; margin-bottom: 2em;">

						<% if (participantList.Count == 0)
							{%>
						<div style="margin-top: 10em; margin-bottom: 10em;">
							<i style="font-size: large; color: grey;">No Attendees</i>
						</div>
						<%} %>

						<ul class="participants" style="text-align: center;" id="myUL">

							<%foreach (var participant in participantList)
								{
									var i = 0;
							%>


							<li class="participantsListed" style="margin-bottom: 5%; margin-right: 10%;">
								<img src="<%=participant.DPimage %>" style="border-radius: 100%; width: 60px; height: 60px;" /><span><a href="/PPGallery.aspx?userId=<%=listOfUserId[i] %>" style="text-decoration: none;"><%=participant.name %></a></span></li>

							<% i++;
								} %>
						</ul>
					</div>
				</div>
			</div>
		</div>

	</div>
	<script> 
		function mySearchParticipant() {
			var input, filter, ul, li, a, i, txtValue;
			input = document.getElementById("mySearchParticipants");
			filter = input.value.toUpperCase();
			ul = document.getElementById("myUL");
			li = ul.getElementsByTagName("li");
			for (i = 0; i < li.length; i++) {
				a = li[i].getElementsByTagName("span")[0];
				txtValue = a.innerText;
				if (txtValue.toUpperCase().indexOf(filter) > -1) {
					li[i].style.display = "";
				} else {
					li[i].style.display = "none";
				}
			}
		}

		function toggleTab1() {
			document.getElementById("tab1").removeAttribute("style");
			document.getElementById("tab2").style.display = "none";
			document.getElementById("tab1Link").className = "nav-link active";
			document.getElementById("tab2Link").className = "nav-link";
		}

		function toggleTab2() {
			document.getElementById("tab2").removeAttribute("style");
			document.getElementById("tab1").style.display = "none";
			document.getElementById("tab2Link").className = "nav-link active";
			document.getElementById("tab1Link").className = "nav-link";
		}


	</script>
</asp:Content>
