<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImmunizationDashboardGraphPopUp.aspx.cs" Inherits="EMR_Immunization_ImmunizationDashboardGraphPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:ScriptManager ID="ScriptManager" runat="server">
        </asp:ScriptManager>
         <table cellpadding="0" cellspacing="0" width="100%" style="border: 1px solid #6593CF;">
            <tr>
                <%--<td align="left">
                    <asp:Panel ID="pnlCommon" runat="server" Visible="false">
                        <asp:Label ID="Label2" runat="server" Text="Graph" SkinID="label"></asp:Label>
                        <telerik:RadComboBox ID="ddlCommon" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCommon_SelectedIndexChanged"
                            RepeatDirection="Horizontal">
                        </telerik:RadComboBox>
                    </asp:Panel>
                </td>--%>
                <td>
                    <telerik:RadComboBox ID="ddlMonthYear" runat="server" SkinID="DropDown" EmptyMessage="[ Select ]"
                        Width="50px" AutoPostBack="true" OnSelectedIndexChanged="ddlMonthYear_OnSelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Text="Month" Value="M" />
                            <telerik:RadComboBoxItem Text="Year" Value="Y" Selected="True" />
                        </Items>
                    </telerik:RadComboBox>
                    <telerik:RadComboBox ID="ddlMonth" runat="server" SkinID="DropDown" Width="60px"
                        Visible="false" />
                    <telerik:RadComboBox ID="ddlYear" runat="server" SkinID="DropDown" EmptyMessage="[ Select ]"
                        Width="60px" />
                </td>
                <td>
                    <table runat="server" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="tdLMonth" runat="server" visible="false">
                                <asp:Label ID="Label1" runat="server" Text="Month" SkinID="label"></asp:Label>
                            </td>
                            <td id="tdNMonth" runat="server" visible="false">
                                <telerik:RadComboBox ID="ddlMonth1" runat="server" SkinID="DropDown" EmptyMessage="[ Select ]"
                                    Width="70px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <asp:Panel ID="pnlRegistration" runat="server" Visible="false">
                        <asp:Label ID="Label5" runat="server" SkinID="label" Text="From"></asp:Label>
                        <telerik:RadDatePicker ID="dtpFromDate" runat="server" MinDate="01/01/1900" TabIndex="6"
                            Width="100px">
                            <DateInput ID="ID1" runat="server" DateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                        &nbsp;&nbsp;To&nbsp;&nbsp;
                        <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="01/01/1900" TabIndex="6"
                            Width="100px">
                            <DateInput ID="DateInput1" runat="server" DateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlSurgery" runat="server" Visible="false">
                        <asp:Label ID="lblSurgery" runat="server" Text="Package" SkinID="label"></asp:Label>
                        <telerik:RadComboBox ID="ddlsurgery" runat="server" RepeatDirection="Horizontal">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="A" />
                                <telerik:RadComboBoxItem Text="Under Package" Value="Y" />
                                <telerik:RadComboBoxItem Text="OutSide Package" Value="N" />
                            </Items>
                        </telerik:RadComboBox>
                    </asp:Panel>
                </td>
                 <td>
                    <asp:Panel ID="pnlMTDYTD" runat="server" Visible="false">
                    <table cellpadding="0" cellspacing="0">
                    <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="Date" SkinID="label"></asp:Label>
                         <telerik:RadDatePicker ID="dtpMTDYTDDate" runat="server" MinDate="01/01/1900" TabIndex="6"
                            Width="100px">
                            <DateInput ID="DateInput2" runat="server" DateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </td>
                    <td>
                     <asp:Label ID="Label10" runat="server" Text="Filter" SkinID="label"></asp:Label>
                    <telerik:RadComboBox ID="ddlMTDYTD" runat="server" RepeatDirection="Horizontal">
                            <Items>
                                <telerik:RadComboBoxItem Text="Doctor" Value="DR" />
                                <telerik:RadComboBoxItem Text="Department" Value="DT" />
                                <telerik:RadComboBoxItem Text="Specialisation" Value="SP" />
                                <telerik:RadComboBoxItem Text="Company" Value="CM" />
                                <telerik:RadComboBoxItem Text="Company Type" Value="CT" />
                                <telerik:RadComboBoxItem Text="State" Value="ST" />
                                <telerik:RadComboBoxItem Text="City" Value="C" />
                                <telerik:RadComboBoxItem Text="Week Day" Value="W" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    </tr>
                    </table>
                    
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlReceivable" runat="server" Visible="false">
                        <asp:Label ID="Label3" runat="server" Text="Top N" SkinID="label"></asp:Label>
                        <asp:TextBox ID="txtTopN" runat="server" SkinID="textbox" Text="10"></asp:TextBox>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlRevenueOption" runat="server" Visible="false">
                        <telerik:RadComboBox ID="ddlRevenueOption" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadComboBox>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlRevenue" runat="server" Visible="false">
                        <asp:Label ID="Label4" runat="server" SkinID="label" Text="Year" />
                        <telerik:RadComboBox ID="ddlRYear" runat="server" Skin="Outlook" CheckBoxes="true"
                            AppendDataBoundItems="true" EnableCheckAllItemsCheckBox="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="11-12" Value="2011" />
                                <telerik:RadComboBoxItem Text="12-13" Value="2012" />
                                <telerik:RadComboBoxItem Text="13-14" Value="2013" />
                                <telerik:RadComboBoxItem Text="14-15" Value="2014" />
                                <telerik:RadComboBoxItem Text="15-16" Value="2015" />
                                <telerik:RadComboBoxItem Text="16-17" Value="2016" />
                                <telerik:RadComboBoxItem Text="17-18" Value="2017" />
                                <telerik:RadComboBoxItem Text="18-19" Value="2018" />
                                <telerik:RadComboBoxItem Text="19-20" Value="2019" />
                                <telerik:RadComboBoxItem Text="20-21" Value="2020" />
                            </Items>
                        </telerik:RadComboBox>
                        <asp:Label ID="lable1" runat="server" Text="Month" SkinID="label"></asp:Label>
                        <telerik:RadComboBox ID="ddlRMonth" runat="server" AppendDataBoundItems="true" EnableCheckAllItemsCheckBox="true"
                            CheckBoxes="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Text="January" />
                                <telerik:RadComboBoxItem Value="2" Text="February" />
                                <telerik:RadComboBoxItem Value="3" Text="March" />
                                <telerik:RadComboBoxItem Value="4" Text="April" />
                                <telerik:RadComboBoxItem Value="5" Text="May" />
                                <telerik:RadComboBoxItem Value="6" Text="June" />
                                <telerik:RadComboBoxItem Value="7" Text="July" />
                                <telerik:RadComboBoxItem Value="8" Text="August" />
                                <telerik:RadComboBoxItem Value="9" Text="September" />
                                <telerik:RadComboBoxItem Value="10" Text="October" />
                                <telerik:RadComboBoxItem Value="11" Text="November" />
                                <telerik:RadComboBoxItem Value="12" Text="December" />
                            </Items>
                        </telerik:RadComboBox>
                        <asp:CheckBox ID="ckhCompareOnlyYear" runat="server" Text="Compare Only Year" Visible="false"
                            TextAlign="Right" />
                    </asp:Panel>
                </td>
                <td>
                    <%--<table id="tbComaprision" runat="server">
                        <tr>
                            <td id="tbComaprision1" runat="server">
                                <asp:Label ID="Label8" runat="server" Text="Comparison Criteria" SkinID="label"></asp:Label>
                            </td>
                            <td id="tbComaprision2" runat="server">
                                <telerik:RadComboBox ID="ddlSearchCriteria" runat="server" OnSelectedIndexChanged="ddlSearchCriteria_OnSelectedIndexChanged"
                                    AutoPostBack="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Y" Text="Year wise" />
                                        <telerik:RadComboBoxItem Value="M" Text="Month wise" />
                                        <telerik:RadComboBoxItem Value="Q" Text="Quater wise" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td id="tbComaprision3" runat="server">
                                <asp:Label ID="lblYearComparision" runat="server" Text="Year" SkinID="label"></asp:Label>
                            </td>
                            <td id="tbComaprision4" runat="server">
                                <telerik:RadComboBox ID="ddlYearComparsion" runat="server" Skin="Outlook"
                                 EnableCheckAllItemsCheckBox="true"
                                 CheckBoxes="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="12-13" Value="2012" />
                                        <telerik:RadComboBoxItem Text="13-14" Value="2013" />
                                        <telerik:RadComboBoxItem Text="14-15" Value="2014" />
                                        <telerik:RadComboBoxItem Text="15-16" Value="2015" />
                                        <telerik:RadComboBoxItem Text="16-17" Value="2016" />
                                        <telerik:RadComboBoxItem Text="17-18" Value="2017" />
                                        <telerik:RadComboBoxItem Text="18-19" Value="2018" />
                                        <telerik:RadComboBoxItem Text="19-20" Value="2019" />
                                        <telerik:RadComboBoxItem Text="20-21" Value="2020" />
                                        <telerik:RadComboBoxItem Text="21-22" Value="2021" />
                                        <telerik:RadComboBoxItem Text="22-23" Value="2022" />
                                        <telerik:RadComboBoxItem Text="23-24" Value="2024" />
                                        <telerik:RadComboBoxItem Text="24-25" Value="2024" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td id="tdlblMonth" runat="server">
                                <asp:Label ID="Label6" runat="server" Text="Month" SkinID="label"></asp:Label>
                            </td>
                            <td id="tdddlMonth" runat="server">
                                <telerik:RadComboBox ID="ddlComparisionMonth" runat="server" EnableCheckAllItemsCheckBox="true" CheckBoxes="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="January" />
                                        <telerik:RadComboBoxItem Value="2" Text="February" />
                                        <telerik:RadComboBoxItem Value="3" Text="March" />
                                        <telerik:RadComboBoxItem Value="4" Text="April" />
                                        <telerik:RadComboBoxItem Value="5" Text="May" />
                                        <telerik:RadComboBoxItem Value="6" Text="June" />
                                        <telerik:RadComboBoxItem Value="7" Text="July" />
                                        <telerik:RadComboBoxItem Value="8" Text="August" />
                                        <telerik:RadComboBoxItem Value="9" Text="September" />
                                        <telerik:RadComboBoxItem Value="10" Text="October" />
                                        <telerik:RadComboBoxItem Value="11" Text="November" />
                                        <telerik:RadComboBoxItem Value="12" Text="December" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td id="tdlblQuater" runat="server">
                                <asp:Label ID="Label7" runat="server" Text="Quater" SkinID="label"></asp:Label>
                            </td>
                            <td id="tdddlQuater" runat="server">
                                <telerik:RadComboBox ID="ddlQuater" runat="server" 
                                EnableCheckAllItemsCheckBox="true"
                                    CheckBoxes="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Quater 1" />
                                        <telerik:RadComboBoxItem Value="2" Text="Quater 2" />
                                        <telerik:RadComboBoxItem Value="3" Text="Quater 3" />
                                        <telerik:RadComboBoxItem Value="4" Text="Quater 4" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>--%>
                </td>
                <td>
                  
                   
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
