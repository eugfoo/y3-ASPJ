<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="editEventDetails.aspx.cs" Inherits="FinalProj.editEventDetails"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

	<style>
		#ContentPlaceHolder1_panelError {
			text-align: center;
		}

		#ContentPlaceHolder1_panelSuccess {
			text-align: center;
		}


		#ContentPlaceHolder1_saveEdit {
			padding: 2%;
			width: 100%;
		}


		#ContentPlaceHolder1_cancelEdit {
			padding: 2%;
			width: 100%;
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

		#ContentPlaceHolder1_eventPic {
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
			#ContentPlaceHolder1_saveEdit {
				margin-top: 1em;
				width: 100%;
				height: auto
			}

			#ContentPlaceHolder1_cancelEdit {
				margin-top: 1em;
				width: 100%;
				height: auto
			}

			#organiserDiv {
				width: 280px;
				margin: 0 auto;
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
	<script>
		function countChars(obj) {
			var maxLength = 3000;
			var strLength = obj.value.length;
			var charRemain = (maxLength - strLength);

			if (charRemain < 0) {
				document.getElementById("charNum").innerHTML = '<span style="color: red;">You have exceeded the limit of ' + maxLength + ' characters</span>';
			} else {
				document.getElementById("charNum").innerHTML = charRemain + ' characters remaining';
			}
		}

		var loadFile = function (event) {
			var output = document.getElementById('ContentPlaceHolder1_eventPic');
			output.src = URL.createObjectURL(event.target.files[0]);
		};


	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div style="min-height: 91vh;">

		<div class="container">
			<br />
			<asp:Panel ID="panelSuccess" Visible="false" runat="server" CssClass="alert alert-dismissable alert-success">
				<asp:Label ID="lb_success" runat="server"></asp:Label>
			</asp:Panel>

			<asp:Panel ID="PanelError" runat="server" Visible="false" CssClass="stuff alert alert-danger ">
				<asp:Label ID="errmsgTb" runat="server"></asp:Label>
			</asp:Panel>
			<div class="row">
				<div class="col-sm-12 col-md-12 col-lg-6" id="eventPhoto">
					<asp:Image ID="eventPic" runat="server" />
					<div class="form-control">
						<asp:FileUpload ID="FileUploadControl" CssClass="col-md-8" onchange="loadFile(event)" runat="server" />
					</div>
				</div>
				<div class="col-sm-12 col-md-12 col-lg-6" id="eventDetails">
					<b>Title:</b>
					<asp:TextBox ID="eventTitle" CssClass="form-control" runat="server"></asp:TextBox>
					<div id="organiserDiv">
						<asp:Image Style="border-radius: 100%; width: 60px; height: 60px;" ID="imgDP" runat="server" /><span style="padding-left: 10px">organised by <a href="/PPGallery.aspx?userId=<%=eventDetail.User_id%>" style="text-decoration: none;"><%=userName %></a></span>

						<div style="padding-left: 70px;">
							<i class="fa fa-clock"></i>&nbsp;<b>Date:</b><asp:TextBox ID="eventDate" CssClass="form-control" runat="server" type="date" format="DD-MM-YYYY" min="<%=DateTime.Now.Date %>" Font-Overline="False"></asp:TextBox>
							<br />

							<div class="row">

								<div class="col-sm-12 col-md-12 col-lg-5">
									<b>Start Time:</b>

									<asp:TextBox ID="startTimeEdit" CssClass="form-control" runat="server" min="07:00" max="22:00" format="hh:mm" type="time"></asp:TextBox>

								</div>
								<div class="col-sm-12 col-md-12 col-lg-2" style="text-align: center; margin-top: auto; margin-bottom: auto;">
									to
								</div>
								<div class="col-sm-12 col-md-12 col-lg-5">
									<b>End Time:</b>
									<asp:TextBox ID="endTime" CssClass="form-control" runat="server" min="07:00" max="22:00" format="hh:mm" type="time"></asp:TextBox>
								</div>
							</div>
						</div>
						<br />


						<p style="padding-left: 70px;"><i class="fa fa-map-marker-alt"></i>&nbsp;<b>Location:</b><asp:TextBox ID="eventAddress" CssClass="form-control" runat="server" widht="25%"></asp:TextBox></p>
						<p style="padding-left: 70px;">
							<b>Note:</b>
							<asp:TextBox ID="noteText" CssClass="form-control" runat="server"></asp:TextBox>
						</p>
						<div class="row">
							<div class="col-sm-12 col-md-6 col-lg-6">
								<asp:Button ID="saveEdit" CssClass="btn btn-success" runat="server" Text="SAVE" OnClick="saveEdit_Click" />
							</div>
							<div class="col-sm-12 col-md-6 col-lg-6">

								<asp:Button ID="cancelEdit" CssClass="btn btn-danger" runat="server" Text="CANCEL" OnClick="cancelEdit_Click" />
							</div>

						</div>

					</div>
				</div>

				<div class="col-sm-12 col-md-12 col-lg-12">
					<ul class="nav nav-tabs">
						<li class="nav-item">
							<a class="nav-link active" href="#tab1" id="tab1Link" onclick="toggleTab1()">About</a>
						</li>
						<li class="nav-item">
							<a class="nav-link" id="tab2Link" style="color: grey;">Attendees</a>
						</li>

					</ul>
				</div>


			</div>
			<div id="tab1" class="row">
				<div class="col-sm-12 col-md-12 col-lg-6" style="margin-top: 2em;">
					<b>Description:</b>
					<asp:TextBox ID="desc" onkeyup="countChars(this);" CssClass="form-control" runat="server" Width="100%" Height="250" TextMode="MultiLine"></asp:TextBox>
					<p id="charNum">
						<% if (eventDetail.Desc == "")
							{%> 3000 <%}
										 else
										 {
											 int enterCount = 0, index = 0;

											 while (index < desc.Text.Length)
											 {
												 // check if current char is part of a word
												 if (desc.Text[index] == '\r' && desc.Text[index + 1] == '\n')
													 enterCount++;
												 index++;
											 }

							%> <%=((3000 + enterCount) - eventDetail.Desc.Length) %><%} %>characters remaining
					</p>

				</div>
				<div class="col-sm-12 col-md-12 col-lg-6" style="margin-top: 2em;">
					<div id="contactOrganiser">
						<p class="h5">Contact Organiser</p>
						<asp:Image Style="border-radius: 100%; width: 60px; height: 60px;" ID="imgDPOrg" runat="server" /><span style="padding-left: 10px"><a href="/PPGallery.aspx?userId=<%=eventDetail.User_id%>" style="text-decoration: none;"><%=userName %></a><br />
							<br />
							Email: <%=profilePic.email %></span>
					</div>

					<p style="margin-top: 1em;" id="remainingSpots" class="h5">
						Max Attendees:<asp:TextBox ID="maxAttend" CssClass="form-control" runat="server" Rows="5" type="number"></asp:TextBox>
					</p>
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
							<img style="border-radius: 100%; width: 60px; height: 60px;" src="<%=participantList[i].DPimage %>">

							<p><%= participantList[i].name %></p>
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
							<img style="border-radius: 100%; width: 60px; height: 60px;" src="<%=participantList[i].DPimage %>">
							<p><%= participantList[i].name %></p>
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

						<ul class="participants" id="myUL">

							<%foreach (var participant in participantList)
								{
							%>


							<li class="participantsListed" style="margin-bottom: 5%;">
								<img src="<%=participant.DPimage %>" style="border-radius: 100%; width: 60px; height: 60px;" /><span style="padding-left: 10px"><a href="#" style="text-decoration: none;"><%=participant.name %></a></span></li>

							<%} %>
						</ul>
					</div>
				</div>
			</div>
		</div>

	</div>
	<script> 

		window.addEventListener("load", function () {
			let tdyDate = new Date();
			let tdyMonth = (parseInt(tdyDate.getMonth()) + 1).toString().length == 1 ? "0" + (parseInt(tdyDate.getMonth()) + 1).toString() : (parseInt(tdyDate.getMonth()) + 1).toString();
			let tdyDay = tdyDate.getDate().toString().length == 1 ? "0" + tdyDate.getDate() : tdyDate.getDate();
			let stringDate = tdyDate.getFullYear() + "-" + tdyMonth + "-" + tdyDay;
			document.getElementById("ContentPlaceHolder1_eventDate").setAttribute("min", stringDate);
		});

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




	</script>
</asp:Content>
