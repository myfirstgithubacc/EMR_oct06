<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login to WEB HIS (Akhil Systems Private Limited)</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="shortcut icon" type="image/ico" href="/Images/Logo/HealthHub.ico" />
   <%-- <link href="/Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />--%>
    <link href="/Include/css/main.css" rel="stylesheet" />



    <%-- <link href='https://fonts.googleapis.com/css?family=Roboto:400,500' rel='stylesheet' type='text/css'>--%>
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <style type="text/css">
         @media screen and (min-device-width : 991px){
             .right_bx_padding{
                 width:70%;
             }
         }
           @media screen and (max-device-width : 990px){
             .right_bx_padding{
                 width:100%;
             }
         }
         .logo_main, .logo_client {
             width:170px!important;
         }
         .right_bx_padding{
             float:right;
         }
    </style>
    <script type="text/javascript">

        function clickEnterKey(obj, event) {
            var keyCode;

            if (event.keyCode > 0) {
                keyCode = event.keyCode;

            }
            else if (event.which > 0) {
                keyCode = event.which;

            }
            else {
                keycode = event.charCode;

            }
            if (keyCode == 13) {
                document.getElementById(obj).focus();

                return false;
            }
            else {
                return true;
            }
        }

    </script>
  <%--  <script src='https://servedbydoceree.doceree.com/script/render-header.js'></script>
    <script>
        var hcpContext;

        function docereeLogIn(userObj) {
            if (!hcpContext) {
                hcpContext = userObj;
                if (typeof setDocereeContext === 'function') {
                    setDocereeContext(hcpContext);
                }
            }
        };

        function docereeLogOut() {
            document.cookie = '_docereeContext=; Max-Age=-99999999;';
        };
    </script>--%>

    <%--<script>
        let userObj = {
            firstName: 'John',
            lastName: 'Doe',
            zipCode: '400004',
            specialization: 'Anesthesiology',
            city: 'Mumbai',
            gender: 'Male',
            mciRegistrationNumber: 'ABCDE12345',
            email: 'john.doe@example.com',
            mobile: '9999999999'
        }
        docereeLogIn(userObj);
    </script>--%>
</head>

<body>

     <%--<div id='DOC_1nf40jki03l8b0'>
            <script type='text/javascript'>
                var docCont = {
                    content_id: 'DOC_1nf40jki03l8b0',
                    content_sizes: ['728 x 90'],
                    click: 'DOCEREE_CLICK_URL_UNESC'
                };
                var docereeAds = docereeAds || {};
                docereeAds[docCont.content_id] = docCont;
            </script>
            <script type='text/javascript' src="https://servedbydoceree.doceree.com/resources/p/render.js">
            </script>
        </div>--%>


    <form id="form1" runat="server">

       
        <%--<input id="fullscreen" type="button" value="Submit" />--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
            DecorationZoneID="wrapper" Skin="Metro"></telerik:RadFormDecorator>

        <!-- header ends -->

        <div class="container_fluid body_main" style="overflow:hidden" >

            <div class="bg1">
                <div class="container">
                    <div class="head_main" id="logo">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <div class="logo_main" id="logo_Akhil">
                                    <img src="images/akhil_logo.png" alt=""/>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-5">
                                <div class="logo_client " style="float:right;">
                                    <%--<span class="licence">Licence to</span>--%>
                                    <asp:Image ID="imgHospitalLogo" runat="server" ImageUrl="~/Images/Logo/AlkindiLogo.gif" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="container">
                <div class="row">
                <div class="col-md-6 col-sm-12">
                    <div class="left_image">
                        <img src="images/main.png" alt="">
                    </div>
                </div>
                <div class="col-md-6 col-sm-12" style="float:right;" id="right_side">
                    <div class="right_bx_padding">
                        <h1 class="text-left" style="font-size: 35px;">Miracle HIS</h1>
                        <div id="text_box">
                            <div id="box">
                                <div class="form_main">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtUserID" runat="server" placeholder="User Name" AutoCompleteType="None"
                                            Wrap="true" SkinID="textbox" MaxLength="50" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <%--<asp:Label ID="Label1" runat="server" Text="Password" CssClass="caption_Gray"></asp:Label>--%>
                                        <asp:TextBox ID="txtPassword" runat="server" AutoPostBack="True" EnableViewState="true"
                                            OnTextChanged="txtPassword_TextChanged" placeholder="Password" TextMode="Password"
                                            SkinID="textbox" MaxLength="100"></asp:TextBox>
                                    </div>
                                    <div class="form-group1">
                                        <%--<asp:Label ID="Label2" runat="server" Text="Facility" CssClass="caption_Gray"></asp:Label>--%>
                                        <telerik:RadComboBox ID="ddlFacility" runat="server" AutoPostBack="false" EmptyMessage="Facility"
                                            Skin="Simple" Width="100%">
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="form-group1">
                                        <%--<asp:Label ID="Label3" runat="server" Text="Group" CssClass="caption_Gray"></asp:Label>--%>
                                        <telerik:RadComboBox ID="dropGroup" runat="server" EmptyMessage="Group" Skin="Simple"
                                            Width="100%">
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="form-group1">
                                        <telerik:RadComboBox ID="ddlFinancial" runat="server" Width="100%" Skin="Simple"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlFinancial_SelectedIndexChanged"
                                            Visible="true" />
                                    </div>
                                    <div class="form-group1">
                                        <telerik:RadComboBox ID="ddlEntrySite" runat="server" Width="100%" Skin="Simple"
                                            Visible="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="EntrySite" Value="" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-md-6 col-6">
                                                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_OnClick"
                                                    CssClass="btn btn-primary btn-block" />
                                            </div>
                                            <div class="col-md-6 col-6">
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_OnClick"
                                                    CssClass="btn btn-primary btn-block" />
                                            </div>
                                        </div>


                                        <asp:HiddenField ID="hdnHspId" runat="server" />
                                    </div>


                                    <asp:LinkButton ID="lnkSetUp" runat="server" Visible="false" OnClick="lnkSetUp_Click" CssClass="btnlilink pull-right">New Set Up</asp:LinkButton>

                                    <asp:LinkButton ID="lnkChangePassword" runat="server" Text="Change Password" OnClick="lnkChangePassword_OnClick"
                                        Visible="false"></asp:LinkButton>

                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="caption_Gray"></asp:Label>

                                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

</div>
            </div>
            <footer>
		<div class="container">
			&copy;<%=DateTime.Now.Year %> Akhil Systems Pvt. Ltd., All rights reserved.
			                <div id="dvserverInfo" class="pull-right text-right" runat ="server" ></div>
		</div>
		
	</footer>

        </div>


    </form>
</body>
</html>
