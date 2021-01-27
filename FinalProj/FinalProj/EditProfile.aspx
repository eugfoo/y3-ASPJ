<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" ClientIDMode="Static" Inherits="FinalProj.EditProfile" %>

<%@ Import Namespace="FinalProj.BLL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {
            $("#fuBP").change(function (e) {
                $("#btnUploadBP").click();
            });

            $("#fuDP").change(function (e) {
                $("#btnUploadDP").click();
            });

            $(function () {
                $('[data-toggle="tooltip"]').tooltip()
            });
        });
        var modal = null
        function pop() {
            if (modal === null) {
                document.getElementById("box").style.display = "block";
                modal = true
            } else {
                document.getElementById("box").style.display = "none";
                modal = null
            }
        }
    </script>
    <style>
        .ttInfo:hover {
            cursor: pointer;
        }
        /* The switch - the box around the slider */
        .switch {
            position: relative;
            display: inline-block;
            width: 60px;
            height: 34px;
        }

            /* Hide default HTML checkbox */
            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        /* The slider */
        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 26px;
                width: 26px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        #box {
            display: block;
            margin-top: 4%;
            margin-bottom: 4%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <% Users user = (Users)Session["user"];%>

    <div style="min-height: 90vh">
        <div class="ml-5 mt-3 mr-5">
            <p class="" style="border-bottom: 1px solid rgba(0,0,0,.250); font-size: 30px;">Edit Profile</p>
            <div class="row">
                <div class="col-12 col-sm-5 col-md-5">
                    <div class="form-group">
                        <label for="formGroupExampleInput">Background Picture</label>
                        <br />
                        <asp:Image ID="imgBP" CssClass="card-img-top" Style="max-height: 100px; max-width: 1100px; min-height: 100px; min-width: 150px;" runat="server" />
                        <div class="form-control">
                            <asp:FileUpload CssClass="col-md-8" ID="fuBP" runat="server" accept=".png,.jpg,.jpeg" />
                            <asp:Button Style="display: none;" ID="btnUploadBP" runat="server" Text="Display" OnClick="btnUploadBP_Click" CausesValidation="false" UseSubmitBehavior="False" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput">Display Picture</label>
                        <br />
                        <asp:Image ID="imgDP" CssClass="card-img-top" Style="max-width: 100px; max-height: 100px; min-height: 100px; min-width: 100px;" runat="server" />
                        <div class="form-control">
                            <asp:FileUpload CssClass="col-md-8" ID="fuDP" runat="server" accept=".png,.jpg,.jpeg" />
                            <asp:Button Style="display: none;" ID="btnUploadDP" runat="server" Text="Display" OnClick="btnUploadDP_Click" CausesValidation="false" UseSubmitBehavior="False" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput2">Name</label>
                        <asp:TextBox type="text" CssClass="form-control" ID="tbName" runat="server" CausesValidation="True"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput2">Description</label>
                        <asp:TextBox TextMode="MultiLine" Columns="50" Rows="5" type="text"
                            CssClass="form-control" ID="tbDesc" runat="server" CausesValidation="True"></asp:TextBox>
                    </div>
                </div>
                <div class="col-0 col-sm-1 col-md-1"></div>
                <div class="col-12 col-sm-5 col-md-5">
                    <div class="form-group">
                        <label for="formGroupExampleInput2">Connect your Social Media</label>
                        <div class="input-group mb-2">
                            <div style="min-width: 140px;" class="input-group-prepend">
                                <div style="min-width: inherit;" class="input-group-text">facebook.com/</div>
                            </div>
                            <asp:TextBox type="text" CssClass="form-control" ID="tbFacebook" runat="server" CausesValidation="False"></asp:TextBox>
                        </div>
                        <div class="input-group mb-2">
                            <div style="min-width: 140px;" class="input-group-prepend">
                                <div style="min-width: inherit;" class="input-group-text">instagram.com/</div>
                            </div>
                            <asp:TextBox type="text" CssClass="form-control" ID="tbInstagram" runat="server" CausesValidation="False"></asp:TextBox>
                        </div>
                        <div class="input-group mb-2">
                            <div style="min-width: 140px;" class="input-group-prepend">
                                <div style="min-width: inherit;" class="input-group-text">twitter.com/</div>
                            </div>
                            <asp:TextBox type="text" CssClass="form-control" ID="tbTwitter" runat="server" CausesValidation="False"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Dietary Requirements:</label>
                        <asp:DropDownList ID="ddlDiet" OnSelectedIndexChanged="ddlDiet_OnSelectedIndexChanged" runat="server" AutoPostBack="True">
                            <asp:ListItem Selected="True" Value="None">None</asp:ListItem>
                            <asp:ListItem Value="Halal">Halal</asp:ListItem>
                            <asp:ListItem Value="Vegetarian">Vegatarian</asp:ListItem>
                            <asp:ListItem Value="Others">Others</asp:ListItem>
                        </asp:DropDownList>
                        <i class="ttInfo fas fa-info-circle" data-html='true' data-toggle="tooltip" data-placement="bottom"
                            title="This won't show on your profile but only to organisers of events you've joined"></i>
                    </div>
                    <div class="form-group">
                        <label for="exampleFormControlTextarea1">If you chose others, please specify:</label>
                        <asp:TextBox TextMode="MultiLine" Columns="50" Rows="3" type="text"
                            CssClass="form-control" ID="tbOtherDiet" runat="server" CausesValidation="True"></asp:TextBox>
                    </div>

                    <!--change password-->
                    <div class="form-group">
                        <button id="btnChangePwd" type="button" class="btn btn-warning" onclick="pop()">Change Password</button><br />
                        <div id="box" runat="server">
                            <h5 class="card-title text-muted font-italic">Input New Passwords</h5>
                            <asp:Label for="userPassword" runat="server" Style="float: left;">New Password:</asp:Label>
                            <p>
                                <asp:TextBox type="password" CssClass="form-control" ID="userPassword" placeholder="New Password" runat="server" ValidationGroup="Credentials" OnTextChanged="userPassword_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <p>
                                <asp:Label for="userPassword" runat="server" CssClass="fa-pull-left" Width="167px">Confirm New Password:</asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblError" CssClass="vError mr-3" runat="server" Visible="False" Font-Italic="False" Font-Size="Small" ForeColor="Red"></asp:Label>
                            <p>
                                <asp:TextBox type="password" CssClass="form-control" ID="userPassword1" placeholder="Confirm New Password" runat="server" ValidationGroup="Credentials" OnTextChanged="userPassword1_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <p>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSubmit" input="button" runat="server" class="btn btn-success btn-md" Text="Confirm" OnClick="submit_Click"/>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblSuccess" CssClass="vError mr-3" runat="server" Visible="False" Font-Italic="False" Font-Size="Small" ForeColor="Green">Changed Password!</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <br />
                        </div>
                    </div>
                    <%-- End of change pwd --%>

                    <div class="form-group">
                        <label for="formGroup2FA">2 Factor Authentication</label>
                        <i class="ttInfo fas fa-info-circle" data-html='true' data-toggle="tooltip" data-placement="bottom"
                            title="Enable 2 Factor Authentication to receive a verification code whenever you log in!"></i>
                        <label class="switch">
                            <asp:CheckBox ID="CB2FA" runat="server" CausesValidation="false" value="1" />
                            <span class="slider round"></span>
                        </label>
                    </div>
                    <div class="form-group">
                        <label for="formGroupGoogle">Google Authenticator</label>
                        <i class="ttInfo fas fa-info-circle" data-html='true' data-toggle="tooltip" data-placement="bottom"
                            title="Enable Google Authenticator to create events instead of receiving OTPs!"></i>
                        <label class="switch">
                            <asp:CheckBox ID="cbGoogle" runat="server" OnCheckedChanged="cbGoogle_CheckedChanged" AutoPostBack="true" />
                            <span class="slider round"></span>
                        </label>
                    </div>
                </div>
            </div>
            <div style="border-bottom: 1px solid rgba(0,0,0,.250);"></div>
            <div class="mt-2 align-bottom" style="text-align: right;">
                <span class="text-muted font-italic mr-3">Fields left empty will <b>not</b> be updated</span>
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger mr-3" Text="Cancel" CausesValidation="false" UseSubmitBehavior="False" OnClick="btnCancel_Click" />
                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" Text="Update" CausesValidation="false" UseSubmitBehavior="False" OnClick="btnUpdate_Click" />
            </div>
        </div>
    </div>
    <br />

</asp:Content>

<%--<div style=" margin: auto; margin-top: 3rem;" class="p-0 card col-10 row">
            <div class="card-header text-center">
                Edit Profile
            </div>
            <div class="card-body">
                <div style="max-width: 600px;" class="col-6 ">
                    <div class="form-group">
                        <label for="formGroupExampleInput">Background Picture</label>
                        <br />
                        <asp:Image ID="imgBP" CssClass="card-img-top" Style="max-height: 100px; max-width: 1100px; min-height: 100px; min-width: 150px;" runat="server" />
                        <div class="form-control">
                            <asp:FileUpload CssClass="col-md-8" ID="fuBP" runat="server" accept=".png,.jpg,.jpeg" />
                            <asp:Button ID="btnUploadBP" runat="server" Text="Upload" OnClick="btnUploadBP_Click" UseSubmitBehavior="False" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput">Display Picture</label>
                        <br />
                        <asp:Image ID="imgDP" CssClass="card-img-top" Style="max-width: 100px; max-height: 100px; min-height: 100px; min-width: 100px;" runat="server" />
                        <div class="form-control">
                            <asp:FileUpload CssClass="col-md-8" ID="fuDP" runat="server" accept=".png,.jpg,.jpeg" />
                            <asp:Button ID="btnUploadDP" runat="server" Text="Upload" OnClick="btnUploadDP_Click" UseSubmitBehavior="False" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput2">Name</label>
                        <asp:RequiredFieldValidator CssClass="vError" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbName" EnableClientScript="False" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:TextBox type="text" CssClass="form-control" ID="tbName" runat="server" CausesValidation="True"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput2">Description</label>
                        <asp:RequiredFieldValidator CssClass="vError" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbDesc" EnableClientScript="False" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:TextBox type="text" CssClass="form-control" ID="tbDesc" runat="server" CausesValidation="True"></asp:TextBox>
                    </div>
                </div>
                <div class="dropdown-divider"></div>
                <div class="col-6">
                    <div class="form-group">
                        <label>Dietary Requirements:</label>
                        <asp:DropDownList ID="ddlDiet" runat="server">
                            <asp:ListItem Selected="True" Value="none">None</asp:ListItem>
                            <asp:ListItem Value="halal">Halal</asp:ListItem>
                            <asp:ListItem Value="vegetarian">Vegatarian</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="exampleFormControlTextarea1">If your option is not in the list, please specify:</label>
                        <textarea class="form-control" id="exampleFormControlTextarea1" rows="3"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="formGroupExampleInput2">Connect your Social Media</label>
                        <div class="input-group mb-2">
                            <div style="min-width: 140px;" class="input-group-prepend">
                                <div style="min-width: inherit;" class="input-group-text">facebook.com/</div>
                            </div>
                            <asp:TextBox type="text" CssClass="form-control" ID="TextBox4" runat="server" CausesValidation="False"></asp:TextBox>
                        </div>
                        <div class="input-group mb-2">
                            <div style="min-width: 140px;" class="input-group-prepend">
                                <div style="min-width: inherit;" class="input-group-text">instagram.com/</div>
                            </div>
                            <asp:TextBox type="text" CssClass="form-control" ID="TextBox1" runat="server" CausesValidation="False"></asp:TextBox>
                        </div>
                        <div class="input-group mb-2">
                            <div style="min-width: 140px;" class="input-group-prepend">
                                <div style="min-width: inherit;" class="input-group-text">twitter.com/</div>
                            </div>
                            <asp:TextBox type="text" CssClass="form-control" ID="TextBox2" runat="server" CausesValidation="False"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="align-bottom" style="text-align: right;">
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger mr-3" Text="Cancel" UseSubmitBehavior="False" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" Text="Update" UseSubmitBehavior="False" OnClick="btnUpdate_Click" />
                </div>
            </div>

        </div>--%>
