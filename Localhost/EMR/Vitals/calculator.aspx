<%@ Page Language="C#" AutoEventWireup="true" CodeFile="calculator.aspx.cs" Inherits="EMR_Vitals_calculator" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Calculator</title>
    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="container-fluid">
            <div class="row header_main">
                <div class="col-md-9 col-sm-9 col-xs-9">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </div>
                <div class="col-md-3 col-sm-3 col-xs-3 text-right">
                    <asp:Button ID="btnclose" runat="server" CssClass="btn btn-primary" Text="Close" OnClientClick="window.close();" />
                </div>
            </div>
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-6 col-sm-6 col-xs-12 p-t-b-5 box-col-checkbox">
                            <asp:RadioButtonList ID="rdohwt" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                    OnSelectedIndexChanged="rdohwt_SelectedIndexChanged">
                                    <asp:ListItem Text="Height (HT)" Value="HT" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Weight (WT)" Value="WT"></asp:ListItem>
                                    <asp:ListItem Text="Temperature (T)" Value="TR"></asp:ListItem>
                                </asp:RadioButtonList>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-12 p-t-b-5 box-col-checkbox">
                            <asp:RadioButtonList ID="rdomeasurment" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rdomeasurment_SelectedIndexChanged">
                                    <asp:ListItem Text="English To Metric" Value="ENG" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Metric To English" Value="MET"></asp:ListItem>
                                </asp:RadioButtonList>
                        </div>
            </div>

                    <div class="row" id="trheight1" runat="server">
                        <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-4">
                                <asp:Label ID="Label2" runat="server" SkinID="label" Text="Feet"></asp:Label>
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-8">
                                <asp:TextBox ID="txtfeet" runat="server" SkinID="textbox" MaxLength="2" Width="100%"
                                                autocomplete="off"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                FilterType="Custom,Numbers" TargetControlID="txtfeet" ValidChars=".">
                                            </cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                    </div>
                        <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-4">
                                <asp:Label ID="Label1" runat="server" SkinID="label" Text="Inches"></asp:Label>
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-8">
                                <asp:TextBox ID="txtInch" runat="server" SkinID="textbox" MaxLength="4" Width="100%"
                                                autocomplete="off"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                FilterType="Custom,Numbers" TargetControlID="txtInch" ValidChars=".">
                                            </cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                    </div>
                        <div class="col-md-4 col-sm-4 col-xs-12" id="trheight2" runat="server">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-4">
                                <asp:Label ID="Label3" runat="server" SkinID="label" Text="Centimeter"></asp:Label>
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-8">
                                <asp:TextBox ID="txtcentimeter" runat="server" SkinID="textbox" MaxLength="6" Width="100%"
                                                autocomplete="off"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                FilterType="Custom,Numbers" TargetControlID="txtcentimeter" ValidChars=".">
                                            </cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                    </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3 col-sm-3 col-xs-12" id="trweight1" runat="server">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                    <asp:Label ID="Label4" runat="server" SkinID="label" Text="Pound"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <asp:TextBox ID="txtPound" runat="server" SkinID="textbox" MaxLength="6" Width="100%"
                                    autocomplete="off"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                    FilterType="Custom,Numbers" TargetControlID="txtPound" ValidChars=".-+">
                                </cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12" id="trweight2" runat="server">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                    <asp:Label ID="Label5" runat="server" SkinID="label" Text="Kilogram"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <asp:TextBox ID="txtkilogram" runat="server" SkinID="textbox" MaxLength="6" Width="100%"
                                    autocomplete="off"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                    FilterType="Custom,Numbers" TargetControlID="txtkilogram" ValidChars=".-+">
                                </cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12" id="trtempreture1" runat="server">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                    <asp:Label ID="Label6" runat="server" SkinID="label" Text="Fahrenheit"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <asp:TextBox ID="txtFahrenheit" runat="server" SkinID="textbox" MaxLength="6" Width="100%"
                                    autocomplete="off"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                    FilterType="Custom,Numbers" TargetControlID="txtFahrenheit" ValidChars=".-+">
                                </cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12" id="trtempreture2" runat="server">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                    <asp:Label ID="Label7" runat="server" SkinID="label" Text="Celcius"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <asp:TextBox ID="txtCelcius" runat="server" SkinID="textbox" MaxLength="6" Width="100%"
                                    autocomplete="off"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                    FilterType="Custom,Numbers" TargetControlID="txtCelcius" ValidChars=".-+">
                                </cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12 text-right">
                             <asp:Button ID="btnCalculate" runat="server" CssClass="btn btn-primary" Text="Calculate" OnClick="btnCalculate_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnnew" runat="server" CssClass="btn btn-primary" Text="Reset" OnClick="btnnew_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
