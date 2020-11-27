<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="AttendanceSubmitted.aspx.cs" Inherits="FinalProj.AttendanceSubmitted" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="AttendanceSubmitted.css" />
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
    <script type="text/javascript">

            var endTime = '<%= endTime %>';
            var nowDate;

            Number.prototype.padLeft = function (base, chr) {
                var len = (String(base || 10).length - String(this).length) + 1;
                return len > 0 ? new Array(len).join(chr || '0') + this : this;
            }

            var d = new Date,
                dformat = [d.getDate().padLeft(),
                (d.getMonth() + 1).padLeft(),
                d.getFullYear()].join('/') +
                    ' ' +
                    [d.getHours().padLeft(),
                    d.getMinutes().padLeft(),
                    d.getSeconds().padLeft()].join(':');

            nowDate = dformat.toString();

            function getDateObject(datestr) {
                console.log("This is first", datestr);
                var parts = datestr.split(' ');
                console.log("This is second", parts);
                var dateparts = parts[0].split('/');
                var day = dateparts[0];
                var month = parseInt(dateparts[1]) - 1;
                var year = dateparts[2];
                var timeparts = parts[1].split(':')
                var hh = timeparts[0];
                var mm = timeparts[1];
                var ss = timeparts[2];
                console.log("This is first ymdhhmmss: ", year, month, day, hh, mm, ss);
                var date = new Date(year, month, day, hh, mm, ss);
                console.log("This is date: ", date);
                return date;
            }

            function gettimediff(t1, t2) {
                var secs = (t2 - t1) / 1000;

                function z(n) { return (n < 10 ? '0' : '') + n; }
                var sign = secs < 0 ? '-' : '';
                secs = Math.abs(secs);
                return sign + z(secs / 3600 | 0) + ' Hours   ' + z((secs % 3600) / 60 | 0) + ' Minutes   ' + z(secs % 60) + ' Seconds   ';
            }

            endTime = getDateObject(endTime);
            nowDate = getDateObject(nowDate);

            $("#time").text(gettimediff(nowDate, endTime));

            setInterval(function () {

                var d = new Date,
                    dformat = [d.getDate().padLeft(),
                    (d.getMonth() + 1).padLeft(),
                    d.getFullYear()].join('/') +
                        ' ' +
                        [d.getHours().padLeft(),
                        d.getMinutes().padLeft(),
                        d.getSeconds().padLeft()].join(':');

                nowDate = dformat.toString();

                nowDate = getDateObject(nowDate);

                var difference = gettimediff(nowDate, endTime);
                if (difference[0] == "-") {
                    $("#time").text("00 Hours 00 Minutes 00 Seconds");
                    clearInterval();
                }
                else {
                    $("#time").text(gettimediff(nowDate, endTime));
                }

            }, 1000);

        function myFunction() {
                // Declare variables
                var input, filter, table, tr, td, i, txtValue;
                input = document.getElementById("myInput");
                filter = input.value.toUpperCase();
                table = document.getElementById("attendance");
                tr = table.getElementsByTagName("tr");

                // Loop through all table rows, and hide those who don't match the search query
                for (i = 1; i < tr.length; i++) {
                    td = tr[i].getElementsByTagName("td")[1];
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 86vh">
        <div>
            <div>
                <h2 id="title"><%= title %></h2>
            </div>

            <table id="timer">
                <tr>
                    <td id="attendTitle">Time left to edit attendance:</td>
                    <td id="time"></td>
                </tr>
            </table>

            <input type="text" id="myInput" class="w3-input" onkeyup="myFunction()" placeholder="Search for participant" />


            <table id="attendance">
                <tr>
                    <th style="visibility: hidden">id</th>
                    <th>Names</th>
                    <th>Dietary Requirements (If any)</th>
                    <th>Are they at your event?</th>
                </tr>
                <% for (int i = 0; i < attendList.Count; i++)
                    { %>
                <tr>
                    <td style="visibility: hidden">
                        <%= attendUser[i].id %>
                    </td>
                    <td>
                        <%= attendUser[i].name %>
                    </td>
                    <td>
                        <%= diet[i] %>
                    </td>
                    <td>
                        <%= attending[i] %>
                    </td>
                </tr>

                <% } %>
            </table>

            <div id="container">
                <p id="totalPart">Total Participants: </p>
                <p id="totalPartNum"><%= participant %></p>
                <p id="currentPart">Currently Present: </p>
                <p id="currentPartNum"><%= participantHere %></p>
            </div>

            <a href="/AttendanceEdit.aspx" id="btnEdit" class="btn btn-danger" runat="server">Edit</a>
            <a id="btnEnd" class="btn btn-danger" runat="server">Event has ended!</a>
        </div>
    </div>
</asp:Content>
