<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ExceptionLog.aspx.cs" Inherits="Admin_ExceptionLog" Title="Exception Log" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" width="100%" cellpadding="0" cellspacing="0" style="padding-left: 0px;">
        <tr>
            <td>
                <table border="0" cellspacing="0" cellpadding="0" width="100%" style="vertical-align: middle;">
                    <tr>
                        <td class="clsheader" style="padding-left: 5px;" align="left" valign="middle">
                            Exception Log
                        </td>
                        <td class="clsheader" align="left">
                            <asp:Button ID="btnprint" runat="server" SkinID="Button" Text="Print" ToolTip="Save"
                                CausesValidation="false" />
                            &nbsp;<asp:Button ID="btn_Export" runat="server" CausesValidation="false" OnClick="btn_Export_Click"
                                SkinID="Button" Text="Export to pdf" Visible="true" />
                        </td>
                        <td align="right" class="clsheader" style="padding-right: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:UpdatePanel ID="upMessage" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lbl_Msg" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" SkinID="label" Text="Hospital&nbsp;Location"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlHospitalLocation" AppendDataBoundItems="true"
                                runat="server" Width="150" DataSourceID="dsHosp" DataTextField="Name" DataValueField="ID">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" SkinID="label" Text="Source"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlSource" AppendDataBoundItems="true" runat="server"
                                Width="150">
                                <Items>
                                 <telerik:RadComboBoxItem Text="[Select]" Value="" Selected="true" />
                                    <telerik:RadComboBoxItem Text="Application" Value="Application" />
                                    <telerik:RadComboBoxItem Text="SQL" Value="SQL" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" SkinID="label" Text="IP Address"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlIPAddress" AppendDataBoundItems="true"
                                runat="server" Width="150" DataSourceID="dsIP" DataTextField="Name" DataValueField="ID">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label ID="Label5" runat="server" SkinID="label" Text="Server"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlServer" AppendDataBoundItems="true" runat="server"
                                Width="150" DataSourceID="dsServer" DataTextField="Name" DataValueField="ID">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" SkinID="label" Text="Browser"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlBrowser" AppendDataBoundItems="true" runat="server"
                                Width="150">
                                <Items>
                                 <telerik:RadComboBoxItem Text="[Select]" Value="" Selected="true" />
                                    <telerik:RadComboBoxItem Text="Mozilla" Value="Mozilla" />
                                    <telerik:RadComboBoxItem Text="Internet Explorer" Value="Internet Explorer" />
                                    <telerik:RadComboBoxItem Text="Chrome" Value="Chrome" />
                                    <telerik:RadComboBoxItem Text="Opera" Value="Opera" />
                                    <telerik:RadComboBoxItem Text="Safari" Value="Safari" />
                                    <telerik:RadComboBoxItem Text="Netscape Navigator" Value="Netscape Navigator" />                                   
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        
                        <td>
                            <asp:Label ID="Label1" runat="server" SkinID="label" Text="Date&nbsp;From"></asp:Label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:UpdatePanel ID="up2" runat="server">
                                            <ContentTemplate>
                                                <telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="1900-01-01 00:00"
                                                    Width="100px">
                                                </telerik:RadDatePicker>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="up3" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTo" runat="server" Text="To" SkinID="label"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="up4" runat="server">
                                            <ContentTemplate>
                                                <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="1900-01-01 00:00" Width="100px">
                                                </telerik:RadDatePicker>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        
                        <td align="right">
                            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" OnClick="btnSearch_Click"
                                SkinID="Button" Text="Filter" ValidationGroup="fndPatient" />
                            &nbsp;<asp:Button ID="btn_ClearFilter" runat="server" CausesValidation="false" OnClick="btn_ClearFilter_Click"
                                SkinID="Button" Text="Clear Filter" />                            
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Panel ID="pnlgrd" ScrollBars="Auto" Width="100%" runat="server">
                    <telerik:RadGrid ID="gvExceptionLog" Skin="Office2007" Width="100%" BorderWidth="0"
                        AllowFilteringByColumn="False" AllowSorting="True" AllowPaging="True" PageSize="20"
                        runat="server" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                        OnPreRender="gvExceptionLog_PreRender" OnPageIndexChanged="gvExceptionLog_PageIndexChanged"
                        OnSortCommand="gvExceptionLog_SortCommand" OnPageSizeChanged="gvExceptionLog_PageSizeChanged">
                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                            <Resizing AllowRowResize="True" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                AllowColumnResize="True"></Resizing>
                        </ClientSettings>
                        <MasterTableView AllowFilteringByColumn="false">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red;">
                                    No Record Found.</div>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="LOGDATETIME" DefaultInsertValue="" HeaderText="Log Date"
                                    SortExpression="LOGDATETIME" DataFormatString="{0:MM/dd/yyyy H:mm:ss}" UniqueName="LOGDATETIME"
                                    DataType="System.DateTime" HeaderStyle-Width="130px" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SOURCE" DefaultInsertValue="" HeaderText="Source"
                                    SortExpression="SOURCE" UniqueName="SOURCE" AllowFiltering="false" HeaderStyle-Width="100px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SERVERNAME" DefaultInsertValue="" HeaderText="Server"
                                    SortExpression="SERVERNAME" UniqueName="SERVERNAME" AllowFiltering="false" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="USERIP" DefaultInsertValue="" HeaderText="IP&nbsp;Address"
                                    SortExpression="USERIP" UniqueName="USERIP" AllowFiltering="false" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MESSAGE" DefaultInsertValue="" HeaderText="Message"
                                    SortExpression="MESSAGE" UniqueName="MESSAGE" AllowFiltering="false" HeaderStyle-Width="200px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TARGETSITE" DefaultInsertValue="" HeaderText="Target&nbsp;Site"
                                    SortExpression="TARGETSITE" UniqueName="TARGETSITE" AllowFiltering="false" HeaderStyle-Width="200px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="STACKTRACE" DefaultInsertValue="" HeaderText="Stack&nbsp;Trace"
                                    SortExpression="STACKTRACE" UniqueName="STACKTRACE" AllowFiltering="false" HeaderStyle-Width="300px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="REQUESTURL" DefaultInsertValue="" HeaderText="Request&nbsp;Url"
                                    SortExpression="REQUESTURL" UniqueName="REQUESTURL" AllowFiltering="false" HeaderStyle-Width="150px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="QUERYSTRING" DefaultInsertValue="" HeaderText="Query&nbsp;String"
                                    SortExpression="QUERYSTRING" UniqueName="QUERYSTRING" AllowFiltering="false"
                                    HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="USERAGENT" DefaultInsertValue="" HeaderText="Browser"
                                    SortExpression="USERAGENT" UniqueName="USERAGENT" AllowFiltering="false" HeaderStyle-Width="120px">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings AllowColumnsReorder="True">
                            <Resizing AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <asp:SqlDataSource ID="dsHosp" runat="server" ConnectionString="<%$ ConnectionStrings:akl%>"
                        EnableCaching="true" SelectCommand="SELECT '0' AS Id,'[Select]' as Name UNION SELECT convert(varchar,Id)Id, Name FROM HospitalLocation Where Active = 1  ORDER BY Name"
                        SelectCommandType="Text"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="dsIP" runat="server" ConnectionString="<%$ ConnectionStrings:akl%>"
                        EnableCaching="true" SelectCommand="SELECT '' AS Id,'[Select]' as Name UNION SELECT DISTINCT USERIP AS Id, USERIP AS Name FROM ExceptionLog WHERE USERIP IS NOT NULL"
                        SelectCommandType="Text"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="dsServer" runat="server" ConnectionString="<%$ ConnectionStrings:akl%>"
                        EnableCaching="true" SelectCommand="SELECT '' AS Id,'[Select]' as Name UNION SELECT DISTINCT SERVERNAME AS Id, SERVERNAME AS Name FROM ExceptionLog WHERE SERVERNAME IS NOT NULL"
                        SelectCommandType="Text"></asp:SqlDataSource>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
