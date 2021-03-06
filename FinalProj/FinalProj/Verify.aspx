﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="Verify.aspx.cs" Inherits="FinalProj.Verify" %>

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
            document.getElementById("imageSubmitting").style.display = "block";

        }

        function ConvertToImage(btnSave) {
            var base64 = $('#canvas1')[0].toDataURL();
            $("[id*=hfImageData]").val(base64);
            __doPostBack(btnSave.name, "");
        }

        function startCapture() {
            document.getElementById("btnStartCapture").style.display = "none";
            document.getElementById("divBox").style.display = "block";
        }

        function pop() {
            document.getElementsByClassName("btnSave")[0].style.display = "block";
        }
    </script>

    <style>
        #capturing {
            display: inline-block;
        }

        #box {
            margin-left: auto;
            margin-right: auto;
        }

        #div1 {
            padding-left: 30%;
        }

        #div2 {
            padding-left: 20px;
            position: absolute;
            bottom: 0;
            right: 0;
        }

        #btnCapture {
            background-color: #5bc0de;
            border-radius: 5px;
            border: none;
            padding: 5px 15px 5px 15px;
            color: white;
            margin: 5px;
        }

        #divText {
            width: 680px;
            margin: auto;
            margin-top: 50px;
        }

        #divVerification {
            margin: auto;
            margin-top: 80px;
            width: 320px;
        }

        #btnStartCapture {
            margin-top: 20px;
        }

        #divBox {
            display: none;
        }

        .btnSave {
            background-color: #5cb85c;
            border-radius: 5px;
            border: none;
            padding: 5px 15px 5px 15px;
            color: white;
            display: none;
            margin-top: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divText">
        <h3>Steps:</h3>
        <h4>1. Click Start Capture and point your camera towards yourself</h4>
        <h4>2. Follow the pose in the image provided</h4>
        <h4>3. Click Capture</h4>
        <h4>4. Submit the Image for verification!</h4>

    </div>

    <div id="divVerification">
        <asp:Image ID="img" runat="server" ImageUrl="~/Img/User/AdminFaceVerification/OGVerification.png" Width="320" Height="500" />
        <button id="btnStartCapture" class="btn btn-success" onclick="startCapture(); return false;">Start Capture</button>
    </div>

    <div id="divBox" class="ml-5 mt-3 mr-5">
        <div id="box">
            <div class="row">
                <div id="div1" class="col-lg-9 col-sm-9 col-md-9">
                    <h5>Your Camera:</h5>
                    <video autoplay></video>
                    <h5 id="imageSubmitting" style="display: none;">The image you are submitting:</h5>
                    <canvas id="canvas1"></canvas>
                </div>
                <div id="div2" class="col-lg-3 col-sm-3 col-md-3">
                    <button id="btnCapture" onclick="captureImage(); pop(); return false;">Capture</button>
                    <asp:Button ID="btnSave" Text="Submit Image" class="btnSave" runat="server" UseSubmitBehavior="false" OnClick="Save" OnClientClick="return ConvertToImage(this)" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfImageData" runat="server" />

    <script type="text/javascript">
        navigator.mediaDevices.getUserMedia({ video: true }).then(function (stream) {
            var video = document.querySelector('video');
            video.srcObject = stream;
        })
            .catch(function (err) { alert(err); });
    </script>
</asp:Content>
