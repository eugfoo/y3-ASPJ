<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="Verify.aspx.cs" Inherits="FinalProj.Verify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="https://cdn.rawgit.com/mobomo/sketch.js/master/lib/sketch.min.js" type="text/javascript"></script>
    <script>
        function captureImage() {
            var photo = document.querySelector("canvas");
            var video = document.querySelector("video");
            var context = photo.getContext('2d');
            context.drawImage(video, 0, 0, photo.width, photo.height);
        }
        function ConvertToImage(btnSave) {
            var base64 = $('#canvas1')[0].toDataURL();
            $("[id*=hfImageData]").val(base64);
            __doPostBack(btnSave.name, "");
        };

    </script>

    <style>
        #capturing {
            display: inline-block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <div class="ml-5 mt-3 mr-5">
            <div class="row">
                <div class="col-lg-12 col-sm-6 col-md-6">
                    <video autoplay width="300" height="480"></video>
                    <canvas id="canvas1" width="300" height="480"></canvas>
                </div>
            </div>

        </div>
    </div>


    <asp:HiddenField ID="hfImageData" runat="server" />
    <button onclick="captureImage(); return false;">Capture</button>

    <asp:Button ID="btnSave" Text="Submit Image" runat="server" UseSubmitBehavior="false" OnClick="Save" OnClientClick="return ConvertToImage(this)" />
    <div style="display: none;" class="alert alert-success col-md-12" runat="server" role="alert">
        <asp:Label ID="lblResult" runat="server" Text="" Visible="false"></asp:Label>
    </div>




    <script type="text/javascript">
        navigator.mediaDevices.getUserMedia({ video: true }).then(function (stream) {
            var video = document.querySelector('video');
            video.srcObject = stream;
        })
            .catch(function (err) { alert(err); });
    </script>
</asp:Content>
