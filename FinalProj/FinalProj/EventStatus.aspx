<%@ Page Title="" Language="C#" MasterPageFile="~/ProfilePage.Master" AutoEventWireup="true" CodeBehind="EventStatus.aspx.cs" Inherits="FinalProj.EventStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link rel="stylesheet" type="text/css" href="EventStatus.css" />
    <script>

        function searchFunction() {
            // Declare variables
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("searchEventName");
            filter = input.value.toUpperCase();
            table = document.getElementById("myTable");
            tr = table.getElementsByTagName("tr");

            // Loop through all table rows, and hide those who don't match the search query
            for (i = 0; i < tr.length; i++) {
                td = tr[i].querySelector("td > a");
                if (td) {
                    txtValue = td.textContent || td.innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }

        function sortTable() {
            var table, rows, switching, i, x, y, shouldSwitch;
            table = document.getElementById("myTable");
            switching = true;
            /* Make a loop that will continue until no switching has been done: */
            while (switching) {
                // Start by saying: no switching is done:
                switching = false;
                rows = table.rows;
                /* Loop through all table rows (except the first, which contains table headers): */
                for (i = 1; i < (rows.length - 1); i++) {
                    // Start by saying there should be no switching:
                    shouldSwitch = false;
                    /* Get the two elements you want to compare, one from current row and one from the next: */
                    x = rows[i].getElementsByTagName("TD")[0];
                    y = rows[i + 1].getElementsByTagName("TD")[0];
                    // Check if the two rows should switch place:
                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                        // If so, mark as a switch and break the loop:
                        shouldSwitch = true;
                        break;
                    }
                }
                if (shouldSwitch) {
                    /* If a switch has been marked, make the switch and mark that a switch has been done: */
                    rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                    switching = true;
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div style="height: 100%">
        <div>
            <h2 id="title">Event Overview</h2>
        </div>

        <div class="container">
            <div id="totalStats" class="row">
                <div class="col-sm-12 col-md-12 col-lg-4">
                    Total Events:
                <p id="totalComplete"><%= total %></p>
                </div>
                <div class="col-sm-12 col-md-6 col-lg-4">
                    Events Created:
                <p id="totalCreate"><%= eventCount %></p>
                </div>
                <div class="col-sm-12 col-md-6 col-lg-4">
                    Events Joined:
                <p id="totalParticipate"><%= participated %></p>
                </div>
            </div>
        </div>

        <div class="container">
            <div class="row" id="row2">
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <input type="search" onkeyup="searchFunction()" id="searchEventName" placeholder="Search for event titles.." aria-describedby="button-addon1" class="form-control border-1 bg-light" />
                </div>

                <div id="divRadioList" class="col-sm-12 col-md-6 col-lg-6">
                    <asp:RadioButtonList ID="radioButtonList" runat="server" CssClass="text" RepeatColumns="4" RepeatDirection="Vertical" AutoPostBack="True" OnSelectedIndexChanged="radioButtonList_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="create">Show Created</asp:ListItem>
                        <asp:ListItem Value="participate">Show Joined</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>

        <div style="overflow-x: auto;">
            <h1 id="msg" visible="false" runat="server">No events to show :( <br />Join or create one!</h1>
            <%--style="visibility:hidden"--%>
            <table class="center" id="myTable">
                <% foreach (var element in evStList)
                    { %>
                <tr id="rowRepeat">
                    <td>
                        <img id="eventdp" src="/Img/<%= element.Pic %>" />
                    </td>
                    <td id="tableTD">
                        <a id="eventTitle" href="/eventDetails.aspx?eventId=<%= element.Id %>"><h3 class="title"><%= element.Title %></h3></a>
                        <br />
                        <div id="containerItems">

                            <table id="innerTable">
                                <tr>
                                    <td class="alignRight">
                                        <p style="margin-right: 10px">Organised by: </p>
                                    </td>
                                    <td>
                                        <a class="organiserName" href="/PPGallery.aspx?userId=<%= element.Organiser %>"><img id="dp" src="<%= element.OrganiserPic %>" /></a>
                                        <a class="organiserName" href="/PPGallery.aspx?userId=<%= element.Organiser %>">
                                            <p id="organiser"><%= element.OrganiserName %></p>
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignRight">
                                        <p id="txtDate">Date and time: </p>
                                        <br />
                                    </td>
                                    <td>
                                        <p id="dateTime"><%= element.Date %>, <%= element.StartTime %> to <%= element.EndTime %></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignRight">
                                        <p id="txtStatus">Status: </p>
                                        <br />
                                    </td>
                                    
                                    <td>
                                        <p id="status"><%= element.Completed %></p>
                                    </td>
                                    <script type="text/javascript">
                                        if (document.getElementById("status").innerHTML == "Incomplete") {
                                                document.getElementById("status").style.color = "Red";
                                        }
                                        if (document.getElementById("status").innerHTML == "Complete") {
                                                document.getElementById("status").style.color = "Green";
                                            }
                                    </script>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <% } %>
            </table>
            
        </div>
    </div>
</asp:Content>
