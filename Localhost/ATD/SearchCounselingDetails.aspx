<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchCounselingDetails.aspx.cs"
    Inherits="SearchCounselingDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="Counselling" runat="server">Counselling List</title>
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.Counseling = document.getElementById("hdnCounseling").value;
            var oWnd = GetRadWindow();
            oWnd.close(oArg);

        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        
    </script>

    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #868686 1px !important; border-top:none !important; outline:none !important; color:#333; background: 0 -2300px repeat-x #c1e5ef !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {background-color:#fff !important;}
        #ctl00_ContentPlaceHolder1_Panel1 { background-color:#c1e5ef !important;}
        .RadGrid .rgFilterBox {height:20px;}
        .RadGrid_Office2007 .rgFilterRow {background: #c1e5ef !important;}
        .RadComboBox table td.rcbArrowCell{
            right:auto!important;
        }
        td{
            white-space:nowrap!important;
        }
        table#gvCounselingDetails_ctl00{
            table-layout:auto!important;
            display: block!important;
            overflow-x:auto!important;
        }
        colgroup{
            width:100vw;
        }
        div#gvCounselingDetails{
            width:100vw!important;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server" style="overflow-x:hidden;">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>             
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlgrid" runat="server" Height="100%" Width="100%" BorderWidth="1"
                                BorderColor="SkyBlue" ScrollBars="Auto">
                                <telerik:RadGrid ID="gvCounselingDetails" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                    AllowFilteringByColumn="true" AllowMultiRowSelection="false" 
                                    runat="server" 
                                    AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                    GridLines="Both" AllowPaging="True" Style="height:auto;min-height:200px;" PageSize="15" OnPageIndexChanged="gvCounselingDetails_OnPageIndexChanged"
                                    OnItemCommand="gvCounselingDetails_OnItemCommand" 
                                    OnPreRender="gvCounselingDetails_PreRender" 
                                    onitemdatabound="gvCounselingDetails_ItemDataBound">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                            AllowColumnResize="false" />
                                    </ClientSettings>
                                    <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="true">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red;">
                                                No Record Found.</div>
                                        </NoRecordsTemplate>
                                        <ItemStyle Wrap="true" />
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center"
                                                AllowFiltering="false" HeaderText="Select" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="IbtnSelect" runat="server" Text="Select" CommandName="Select" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            
                                          
                                             <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText="UHID" ShowFilterIcon="false"
                                                DefaultInsertValue="" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                                FilterControlWidth="99%" DataField="RegistrationNo" 
                                                SortExpression="RegistrationNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                           <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText="IP No" ShowFilterIcon="false"
                                                DefaultInsertValue="" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                                FilterControlWidth="99%" DataField="EncounterNo" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                                                SortExpression="EncounterNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                              
                                            <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Patient Name" ShowFilterIcon="false"
                                                DefaultInsertValue="" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                                FilterControlWidth="99%" DataField="Name" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                                                SortExpression="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                              
                                            <telerik:GridTemplateColumn UniqueName="Gender" HeaderText="Age/Gender" AllowFiltering="false"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="GenderToShow" SortExpression="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%#Eval("Gender")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                             <telerik:GridTemplateColumn UniqueName="CounselingNo" HeaderText='Counseling No.'
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="CounselingNo" SortExpression="CounselingNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCounselingNo" runat="server" Text='<%#Eval("CounselingNo")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdnCounselingId" runat="server" Value='<%#Eval("CounselingId")%>' />
                                                     <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#Eval("Active")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CounselingDate" HeaderText="Counseling Date"
                                                AllowFiltering="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="CounselingDate"
                                                SortExpression="CounselingDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCounselingDate" runat="server" Text='<%#Eval("CounselingDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                           
                                            <telerik:GridTemplateColumn HeaderStyle-Width="80px" UniqueName="DateofBirth" AllowFiltering="false" HeaderText="Date of Birth"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="DateofBirth" SortExpression="DateofBirth" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateofBirth" runat="server" Text='<%#Eval("DateofBirth")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Address" HeaderText="Address" AllowFiltering="false"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="Address" SortExpression="Address">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("LocalAddress")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                             <telerik:GridTemplateColumn UniqueName="AdmissionDoctor" HeaderText="Admission Doctor" AllowFiltering="false"
                                                ItemStyle-Width="150px" ShowFilterIcon="false" DefaultInsertValue="" DataField="AdmissionDoctor"
                                                SortExpression="AdmissionDoctor">
                                                <ItemTemplate>
                                            <asp:Label ID="lblAdmissionDoctor" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="AdmissionDate" HeaderText="Admission Date"
                                                AllowFiltering="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="AdmissionDate"
                                                SortExpression="AdmissionDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmissionDate" runat="server" Text='<%#Eval("AdmissionDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="LOS" HeaderText="Esti. LOS" AllowFiltering="false"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="LOS" SortExpression="LOS">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLOS" runat="server" Text='<%#Eval("LOS")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                               <telerik:GridTemplateColumn UniqueName="ExceedDays" HeaderText="Exceed Days" AllowFiltering="false"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="ExceedDays" SortExpression="ExceedDays">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExceedDays" runat="server" Text='<%#Eval("ExceedDays")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            
                                            <telerik:GridTemplateColumn UniqueName="BedCategory" HeaderText="Bed Category" AllowFiltering="false"
                                                ItemStyle-Width="150px" ShowFilterIcon="false" DefaultInsertValue="" DataField="BedCategory"
                                                SortExpression="BedCategory">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBedCategory" runat="server" Text='<%#Eval("BedCategory")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CostOfTreatment" AllowFiltering="false" HeaderText="Cost Of Treatment"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="CostOfTreatment" SortExpression="CostOfTreatment">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCostOfTreatment" runat="server" Text='<%#Eval("CostOfTreatment","{0:n}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="AboutIllness" HeaderText="About Illness"
                                                AllowFiltering="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="AboutIllness"
                                                SortExpression="AboutIllness">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAboutIllness" runat="server" Text='<%#Eval("AboutIllness")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Remarks" HeaderText="Remarks" AllowFiltering="false"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="Remarks" SortExpression="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn UniqueName="Print" HeaderStyle-HorizontalAlign="Center"
                                                AllowFiltering="false" HeaderText="Print" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="IbtnPrint" runat="server" Text="Print" CommandName="Print" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn UniqueName="Delete" HeaderText="Delete" AllowFiltering="false"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="" SortExpression="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                        CommandName="ItemDelete" CausesValidation="false" ImageUrl="~/Images/DeleteRow.png"
                                                        Width="16px" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                            </telerik:RadWindow>
                            </Windows>
                            </telerik:RadWindowManager>
                    
                    </tr>
                </table>
                <asp:HiddenField ID="hdnCounseling" runat="server" Value="0" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        <div >
         <div style="text-align: right; margin-right:10px"><p style="font-size: 12px;color: black;font-weight:bold;">Total Row Count :  <asp:Label ID="lbltotcount" runat="server" Text="0"></asp:Label> </p> </div>
            </div>
       <%-- <br />--%>
        <div >
         <div id="EMREstimationOrder" runat="server" style="height: 25px;text-align: center;background-color: snow;width: 8%;  float: left;border:1px black solid; "><p style="font-size: 12px;color: green;font-weight:bold;">Active</p>   </div>
        <div id="EMREstimationOrder1" runat="server" style="height: 25px;text-align: center;background-color: lightgray; width: 8%; float: left;border:1px black solid; "><p style="font-size: 12px;color: red;font-weight:bold;">InActive</p></div>
            </div>
    </form>
</body>
</html>
