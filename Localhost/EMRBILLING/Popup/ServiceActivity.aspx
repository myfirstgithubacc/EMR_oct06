<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceActivity.aspx.cs" Inherits="EMRBILLING_Popup_ServiceActivity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Service Activity</title>

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        tr:nth-of-type(odd) {
            background: #eee;
        }

        th {
            background: #3498db;
            color: white;
            font-weight: bold;
        }

        td, th {
            padding: 2px 10px;
            border: 1px solid #ccc;
            text-align: left;
            font-size: 14px;
        }
    </style>
</head>

<body style="overflow:hidden;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main" style="padding:6px 6px;">
            <div class="row">
                <div class="col-md-8 col-8 text-center">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-md-4 col-4 text-right">
                    <%--<asp:Button ID="btnsave" OnClick="btnsave_OnClick" runat="server" CssClass="btn btn-primary" ValidationGroup="save" Text="Save" />--%>
                    <asp:Button ID="ibtnClose" runat="server" AccessKey="C" CssClass="btn btn-primary" Text="Close" ToolTip="Close" OnClientClick="window.close();" />

                </div>
            </div>
        </div>

        <div class="container-fluid">

            <div class="row form-group">

                <div class="col-md-5 col-sm-5 margin_Top">
                    <span class="PD-TabRadioNew01 margin_z">
                        <asp:RadioButtonList ID="rdoRegEnc" OnTextChanged="rdoRegEnc_TextChanged" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                            <asp:ListItem Value="0" Text="Package Exclusion"></asp:ListItem>
                            <%--<asp:ListItem Value="3" Text="ER Case"></asp:ListItem>--%>
                            <asp:ListItem Value="1" Text="Package Inclusion" Selected="True"></asp:ListItem>
                            <%--<asp:ListItem Value="2" Text="Discharge"></asp:ListItem>--%>
                        </asp:RadioButtonList>
                    </span>
                </div>
                <%--<div class="col-md-9 col-sm-7"><asp:DropDownList ID="ddlApproved" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlApproved_OnSelectedIndexChanged" /></div>--%>
            </div>
            <div class="row">
                <div class="col-12">
                    <asp:GridView ID="gvServiceActivity" runat="server" SkinID="gridviewOrderNew" Width="100%" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Name" />
                            <asp:BoundField DataField="Group Name" HeaderText="Group Name" />
                            <asp:BoundField DataField="Count" HeaderText="Count" />

                        </Columns>
                    </asp:GridView>   
                </div>
            </div>
        </div>


        <div class="form-group" style="margin-bottom: 5px;">

            <%--  <div class="col-md-6 col-xs-6">
                        <div class="row form-group">
                            <div class="col-md-2 col-xs-4"><asp:Label ID="Label1" runat="server" Text="To"></asp:Label></div>
                            <div class="col-md-10 col-xs-8">
                                <telerik:RadDateTimePicker ID="dtpTodate" runat="server" Width="100%" DateInput-DateFormat="dd/MM/yyyy HH:mm" OnSelectedDateChanged="dtpTodate_SelectedDateChanged" AutoPostBackControl="Both" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <TimeView CellSpacing="-1"></TimeView>
                                    <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                    <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" DateFormat="dd/MM/yyyy HH:mm" LabelWidth="" AutoPostBack="True"></DateInput>
                                </telerik:RadDateTimePicker>
                            </div>
                        </div>
                    </div>--%>
                </div>
                
   

           
     

    </form>
</body>
</html>