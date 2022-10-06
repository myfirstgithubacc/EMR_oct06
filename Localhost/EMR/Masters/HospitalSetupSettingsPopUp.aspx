<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HospitalSetupSettingsPopUp.aspx.cs" Inherits="EMR_Masters_HospitalSetupSettingsPopUp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        /*--- Sanayam ---*/
        .checkboxSetting {
            margin-left: 10px !important;
        }
        .preheader{
            margin:4px 0px 4px -10px;
            color:#fff;
        }
        /*----*/
    </style>
    <%------------------------------------Yogesh------------1/04/2022----------------%>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-sm-12">
                    <div class="row">
                        <div class="col-sm-6">
                            <asp:Label ID="lblmsg" runat="server" Visible="false"></asp:Label>

                        </div>
                        <div class="col-sm-6" style="float: right;">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" Style="margin: 2px -10px 0 0;" OnClick="btnSave_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="Report Name:"></asp:Label>
                                </div>
                                <div class="col-sm-12">
                                    <asp:DropDownList ID="ddlName" runat="server" Height="34px" Width="221px">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-sm-12 form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="Label4" runat="server" Text="Type:"></asp:Label>
                                </div>
                                <div class="col-sm-12">
                                    <asp:DropDownList ID="ddlType" runat="server" Height="34px" Width="221px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="Label2" runat="server" Text="Height:"></asp:Label>
                                </div>
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtHeight" runat="server" Height="34px" Width="221px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="Label3" runat="server" Text="Width:"></asp:Label>
                                </div>
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtWidth" runat="server" Height="34px" Width="221px"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>

        <%--------------Sanyam--------------------%>
        <div class="container">
            
            <div class="row" style="background:#4f7ac4; margin-bottom:5px">
                <div class="col-sm-12">
                    <h5 class="preheader">Prescription Header Setup</h5>
                </div>

            </div>
            <div class="row" style="display: inline-flex;">
                <div class="checkboxSetting">
                    <asp:CheckBox ID="chkHospitalLogo" runat="server" Enabled="true" Text="Hospital Logo" AutoPostBack="true" />
                </div>
                <div class="checkboxSetting">
                    <asp:CheckBox ID="chkAddress" runat="server" Enabled="true" Text="Address" AutoPostBack="true" />
                </div>
                <div class="checkboxSetting">
                    <asp:CheckBox ID="chkNBAHLogo" runat="server" Enabled="true" Text="NBAH Logo" AutoPostBack="true" />
                </div>
            </div>
           
        </div>
         <%------------------%>



    </form>
</body>
</html>
<%------------------------------------Yogesh------------1/04/2022----------------%>