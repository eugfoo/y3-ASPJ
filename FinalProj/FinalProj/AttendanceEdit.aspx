<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="AttendanceEdit.aspx.cs" Inherits="FinalProj.AttendanceEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="AttendanceEdit.css" />
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 86vh">

        <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
        <script type="text/javascript">

            var userIdChecked = [];
            var userIdUncheck = [];
            var str;
            var endTime = '<%= endTime %>';
            var nowDate;
            var difference;

            function getDateObject(datestr) {
                //console.log("This is first", datestr);
                var parts = datestr.split(' ');
                //console.log("This is second", parts);
                var dateparts = parts[0].split('/');
                var day = dateparts[0];
                var month = parseInt(dateparts[1]) - 1;
                var year = dateparts[2];
                var timeparts = parts[1].split(':')
                var hh = timeparts[0];
                var mm = timeparts[1];
                var ss = timeparts[2];
                //console.log("This is first ymdhhmmss: ", year, month, day, hh, mm, ss);
                var date = new Date(year, month, day, hh, mm, ss);
                //console.log("This is date: ", date);
                return date;
            }

            function gettimediff(t1, t2) {
                var secs = (t2 - t1) / 1000;

                function z(n) { return (n < 10 ? '0' : '') + n; }

                // If difference is less than 0, put a negative infront, otherwise leave it empty 
                // (Formatting of the time string displayed)
        
                var sign = secs < 0 ? '-' : '';
                secs = Math.abs(secs);
                return sign + z(secs / 3600 | 0) + ' Hours   ' + z((secs % 3600) / 60 | 0) + ' Minutes   ' + z(secs % 60) + ' Seconds   ';
            }
            

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
                    alert("Event has ended! Attendance will be automatically submitted now");
                    window.location.href = "/AttendanceSubmitted.aspx";
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

            function checkChkBox(checkI) {
                //alert("Checked: " + checkI);
                if (checkI == false) {
                    //alert("true");
                    console.log("Checkbox is checked!");
                    $('input[type=checkbox]').attr('checked', false);
                }
                else if (checkI == true) {
                    // alert("false");
                    console.log("Checkbox not checked!");
                    $('input[type=checkbox]').attr('checked', true);
                }
                
            };

            // function to check which row is checked
            function onClick() {
                userIdChecked = [];

                $('#attendance input[type=checkbox]:checked').each(function () {
                    var row = $(this).closest("tr")[0];
                    str = row.cells[0].innerHTML;
                    str = str.replace(/\s+/g, '');
                    userIdChecked.push(str);
                });

                userIdChecked = JSON.parse("[" + userIdChecked + "]")

                document.getElementById('<%= HiddenField.ClientID%>').value = userIdChecked.join(',');

                //alert(userIdChecked);

                return false;
            };

            // function to check which row is unchecked
            function onClick1() {
                userIdUncheck = [];

                $('#attendance input[type=checkbox]:not(:checked)').each(function () {
                    var row = $(this).closest("tr")[0];
                    str = row.cells[0].innerHTML;
                    str = str.replace(/\s+/g, '');
                    userIdUncheck.push(str);
                });

                userIdUncheck = JSON.parse("[" + userIdUncheck + "]")

                document.getElementById('<%= HiddenField1.ClientID%>').value = userIdUncheck.join(',');

                //alert(userIdUncheck);

                return false;
            };

        </script>

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

            <table id="attendance" class="pure-table pure-table-bordered">
                <tr>
                    <th style="visibility: hidden">ID</th>
                    <th>Names</th>
                    <th>Dietary Requirements (If any)</th>
                    <th>Are they at your event?</th>
                </tr>
                <% for (int i = 0; i < attendList.Count; i++)
                    { %>
                <tr id="trRepeat">
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
                        <script type="text/javascript">
                            document.getElementById("ContentProvider");
                        </script>
                        <input type="checkbox" />
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

            <asp:Button ID="Button1" UseSubmitBehavior="true" OnClientClick="onClick();onClick1();" runat="server" OnClick="Button1_Click" Text="Submit" CssClass="btn btn-primary"/>

            <asp:HiddenField ID="HiddenField" runat="server" Value="5" Visible="true" />
            <asp:HiddenField ID="HiddenField1" runat="server" Value="5" Visible="true" />
        </div>
    </div>
</asp:Content>
