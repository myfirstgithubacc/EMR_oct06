<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LabRequestDetails.aspx.cs" Inherits="LIS_Phlebotomy_LabRequestDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
   <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    
    
     <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { background:#c1e5ef !important;border:1px solid #98abb1 !important; border-top:none !important; color: #333 !important; outline:none !important;}
        .RadGrid .rgPager .rgStatus { width: 0px !important; padding: 0 !important; margin: 0 !important; float: left !important;}

        .RadGrid_Office2007 td.rgPagerCell { border: 1px solid #5d8cc9 !important; background: #c1e5ef !important; outline:none !important;}
        .RadGrid .rgPager .rgStatus { width: 0px !important; padding: 0 !important; margin: 0 !important; float: left !important;}
    
        @media (min-width: 992px) and (max-width: 1199px) {
            #ctl00_ContentPlaceHolder1_txtFromDate_dateInput_wrapper, 
            #ctl00_ContentPlaceHolder1_txtToDate_dateInput_wrapper{ min-width:58px!important; float:left !important; }
            #ctl00_ContentPlaceHolder1_Label6 { width:15px !important; float:left !important; margin:0 !important; padding:0 !important; }
           
        }
        div#upd1{
            overflow:hidden;
        }
    </style>    
    

    <script type="text/javascript">
        function LinkBtnMouseOver(lnk) {
            document.getElementById(lnk).style.color = "red";
        }
        function LinkBtnMouseOut(lnk) {
            document.getElementById(lnk).style.color = "blue";
        }
    </script>
    </head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpmgr" runat="server"></asp:ScriptManager>

        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
            
            
               
                <div class="container-fluid header_main form-group">
                    <div class="row">
                    <div class="col-md-3 col-4"><h2><asp:Label ID="lblHeader" runat="server" Text="Lab&nbsp;Request&nbsp;List" /></h2></div>
                    <div class="col-md-9 col-8 text-center"><asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional"><ContentTemplate><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></ContentTemplate></asp:UpdatePanel></div>
                        </div>
                </div>     
                
                
                    <div class="container-fluid">
                        <div class="row form-group">
                            <asp:Panel ID="PanelN" runat="server" BorderColor="#6699CC" BorderWidth="1" BorderStyle="Solid"
                                Width="100%" Height="480px" ScrollBars="None">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="gvDetails" Skin="Office2007" Width="100%" BorderWidth="0" Height="470px"
                                            AllowMultiRowSelection="False" runat="server" AutoGenerateColumns="false" ShowStatusBar="true"
                                            EnableLinqExpressions="false" GridLines="None" AllowPaging="true" OnPageIndexChanged="gvDetails_PageIndexChanged"
                                         OnItemCommand="gvDetails_OnItemCommand"   OnItemDataBound="gvDetails_ItemDataBound" 
                                            PageSize="40" >
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                                Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <MasterTableView TableLayout="Fixed">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red; float:left;text-align:center; width:100% !important; margin:1em 0; padding:0; font-size:11px;">
                                                        No Record Found.</div>
                                                </NoRecordsTemplate>
                                                <Columns>
                                                   <%--<telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Facility" HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>--%>
                                                    <telerik:GridTemplateColumn UniqueName="UniqueName1" DefaultInsertValue="" HeaderText="RegistrationId"
                                                        AllowFiltering="false" FilterControlWidth="99%" HeaderStyle-Width="30px" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="UniqueName2" DefaultInsertValue="" HeaderText="EncounterId"
                                                        AllowFiltering="false" FilterControlWidth="99%" HeaderStyle-Width="30px" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterId" runat="server" Text='<%#Eval("EncounterId") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="LabNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, LABNO%>'
                                                        DataField="LabNo" SortExpression="LabNo" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                        ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                        DataField="RegistrationNo" SortExpression="RegistrationNo" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                        FilterControlWidth="99%" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                                        DataField="EncounterNo" SortExpression="EncounterNo" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                        FilterControlWidth="99%" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridBoundColumn UniqueName="EncodedDate" DataField="EncodedDate" DefaultInsertValue=""
                                                       DataFormatString="{0:dd/MM/yyyy}"  HeaderText="Order Date" AllowFiltering="false" DataType="System.DateTime" HeaderStyle-Width="150px" />

                                                    <telerik:GridTemplateColumn UniqueName="PatientName" DefaultInsertValue="" HeaderText="Patient Name"
                                                        DataField="PatientName" SortExpression="PatientName" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                        FilterControlWidth="160px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="UniqueName7" DefaultInsertValue="" HeaderText="Service Name"
                                                        AllowFiltering="false" HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                            <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                             <asp:HiddenField ID="hdnDiagSampleId" runat="server" Value='<%#Eval("DiagSampleId") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="UniqueName8" DefaultInsertValue="" HeaderText="Package Name"
                                                        AllowFiltering="false" HeaderStyle-Width="160px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPackageName" runat="server" Text='<%#Eval("PackageName") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="UniqueName9" DefaultInsertValue="" HeaderText="Status"
                                                        AllowFiltering="false" HeaderStyle-Width="160px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatusName" runat="server" Text='<%#Eval("StatusName") %>' />
                                                             <asp:HiddenField ID="hdnStatusId" runat="server" Value='<%#Eval("StatusId") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Delete" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" UniqueName="DeleteTemplateColumn">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgDelete" CommandName="Delete" runat="server" ImageUrl="~/Images/DeleteRow.png" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </div>    
                        
                    </div>
                
                
                
                    
               
                
                
                
                
                <table border="0">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="updatepanel6" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
   </form>
</body>
</html>
