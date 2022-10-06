<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login to WEB HIS (Akhil Systems Private Limited)</title>
    <link rel="shortcut icon" type="image/ico" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="Include/main.css" rel="stylesheet" type="text/css" />

    <style>

       button.rcbActionButton {
    background: url(../images/down-arrow.png) no-repeat !important;
    background-size: 10px !important;
    background-position: 30% 80% !important;
}

       button.rcbActionButton *::before { display: none !important;}
    </style>


    <%-- <link href='https://fonts.googleapis.com/css?family=Roboto:400,500' rel='stylesheet' type='text/css'>--%>
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->




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

</head>

<body>
    <form id="form1" runat="server">
        <%--<input id="fullscreen" type="button" value="Submit" />--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
            DecorationZoneID="wrapper" Skin="Metro"></telerik:RadFormDecorator>

        <!-- header ends -->

        <div class="container_fluid body_main">

            <div class="bg1">
                <div class="container">
                    <div class="head_main" id="logo">
                        <div class="col-md-3 col-sm-4 col-xs-6">
                            <div class="logo_main" id="logo_Akhil">
                                <img src="images/akhil_logo.jpg" alt="">
                            </div>
                        </div>
                        <div class="col-md-2 pull-right col-sm-3 col-xs-5">
                            <div class="logo_client relativ">
                                <span class="licence">Licence to</span>
                                <asp:Image ID="imgHospitalLogo" runat="server" ImageUrl="~/Images/Logo/AlkindiLogo.gif" />                               
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="container">


                <div class="col-md-6 col-sm-6">
                    <div class="left_image">
                        <%--<img src="images/main.png" alt="">--%>
                    </div>
                </div>

                <div class="col-md-4 col-sm-6 pull-right" id="right_side">
                    <div class="right_bx_padding">
                        <h1 class="text-center">Miracle HIS</h1>
                        <h3>User Login</h3>
                        <div id="text_box">
                            <div id="box">
                                <div class="form_main">
                                    <div class="form-group">
                                        <%--<asp:Label ID="lblInfoUserName" runat="server" Text="User Name"></asp:Label>--%>
                                        <asp:TextBox ID="txtUserID" runat="server" placeholder="User Name" AutoCompleteType="None" Wrap="true" SkinID="textbox"
                                            MaxLength="50" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <%--<asp:Label ID="Label1" runat="server" Text="Password" CssClass="caption_Gray"></asp:Label>--%>
                                        <asp:TextBox ID="txtPassword" runat="server" AutoPostBack="True" EnableViewState="true"
                                            OnTextChanged="txtPassword_TextChanged" placeholder="Password" TextMode="Password" SkinID="textbox"
                                            MaxLength="100"></asp:TextBox>
                                    </div>
                                    <div class="form-group1">
                                        <%--<asp:Label ID="Label2" runat="server" Text="Facility" CssClass="caption_Gray"></asp:Label>--%>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlFacility" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged" EmptyMessage="Facility" Skin="Simple" Width="100%">
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="form-group1">
                                        <%--<asp:Label ID="Label3" runat="server" Text="Group" CssClass="caption_Gray"></asp:Label>--%>
                                        <telerik:RadComboBox  RenderMode="Lightweight" ID="dropGroup" runat="server" EmptyMessage="Group" Skin="Simple" Width="100%">
                                        </telerik:RadComboBox>

                                    </div>
                                    <div class="form-group1">
                                        <telerik:RadComboBox  RenderMode="Lightweight" ID="ddlFinancial" runat="server" Width="100%" Skin="Simple"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlFinancial_SelectedIndexChanged" Visible="false" />
                                    </div>
                                    <div class="form-group1">
                                        <telerik:RadComboBox  RenderMode="Lightweight" ID="ddlEntrySite" runat="server" Width="100%" Skin="Simple" Visible="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="EntrySite" Value="" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>



                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-md-6 col-sm-6">
                                                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_OnClick" CssClass="btn btn-primary btn-block" />
                                            </div>
                                            <div class="col-md-6 col-sm-6">
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_OnClick" CssClass="btn btn-danger btn-block" />
                                            </div>

                                            <a href="#" class="btn btn-xs btn-link">Forgot Password?</a>
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
            <footer>
		<div class="container-fluid text-center">
			&copy;<%=DateTime.Now.Year %> Akhil Systems Pvt. Ltd. All rights reserved.
		</div>
		
	</footer>

        </div>


    </form>
</body>
</html>
