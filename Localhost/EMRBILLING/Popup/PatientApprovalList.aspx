<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="PatientApprovalList.aspx.cs" Inherits="EMRBILLING_Popup_PatientApprovalList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <script src="https://code.jquery.com/jquery-1.11.3.js"></script>
    <script type="text/javascript" language="javascript">
        var gridViewId = '#<%= gvOrderApproval.ClientID %>';
        function checkAll(selectAllCheckbox) {
            //get all checkboxes within item rows and select/deselect based on select all checked status
            //:checkbox is jquery selector to get all checkboxes
            $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
        }
        function unCheckSelectAll(selectCheckbox) {
            //if any item is unchecked, uncheck header checkbox as well
            if (!selectCheckbox.checked)
                $('th :checkbox', gridViewId).prop("checked", false);
        }


        function runScript(e) {
            if (e.keyCode == 13) {
                $("#btnFilter").click(); //jquery
                document.getElementById("myButton").click(); //javascript
            }
        }

    </script>
    <style>
     
            #ctl00_ContentPlaceHolder1_txtsearchType {
    width: 100% !important;
}
        
     </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div id="Table1" runat="server">
                <div class="container-fluid header_main">

                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <h2>
                            <asp:Label ID="lblPageType" runat="server" Text="Orders Approval Status" /></h2>

                    </div>
                    <div class="col-lg-4 text-center">
                        <asp:Label ID="lblMessage" runat="server" Text="Orders Approved" Visible="false" Font-Bold="true" ForeColor="Green" />
                    </div>
                   


                </div>
            </div>


            <div class="container-fluid">
                <div class="row">
                    <div class="col-lg-12" style="padding: 0;padding-right: 17px;">
                        <div class="row form-group">
                            <div class="col-md-3 col-sm-3">
                                <div class="row">
                                    <div class="col-md-5 col-sm-3 label2" style="padding-right: 0; text-align: right">
                                        <span class="textName">
                                            <asp:Label ID="lblWard" runat="server" Text="Approval Status" />
                                        </span>
                                    </div>
                                    <div class="col-md-7 col-sm-9">
                                        <telerik:RadComboBox ID="ddlApproval" onkeypress="return runScript(event)" OnSelectedIndexChanged="ddlApproval_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="100%"
                                            Style="padding-left: 5px; padding-right: 2px;">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Value="0" Text="Pending" />
                                                <telerik:RadComboBoxItem runat="server" Value="1" Text="Approved" />
                                            </Items>
                                        </telerik:RadComboBox>

                                    </div>
                                </div>

                            </div>
                            <div class="col-lg-6 col-sm-6" style="padding: 0;">
                                <div class="row">
                                    <div class="col-lg-2 label2" style="padding-right: 0; text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="Order Type" />
                                    </div>
                                    <div class="col-lg-3 label2">
                                        <telerik:RadComboBox ID="ddlOrderType" onkeypress="return runScript(event)" runat="server" Width="100%" Style="padding-left: 5px; padding-right: 2px;">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Text=" " Value="ALL" />
                                                <telerik:RadComboBoxItem runat="server" Value="SO" Text="Service Order" />
                                                <telerik:RadComboBoxItem runat="server" Value="ND" Text="Non Drug Order" />
                                                <telerik:RadComboBoxItem runat="server" Value="PR" Text="Prescription" />
                                                <telerik:RadComboBoxItem runat="server" Value="CD" Text="Clinical Notes" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="col-lg-1 label2" style="padding-right: 0; text-align: right; padding-left: 0;">
                                        <asp:Label ID="lblserchon" runat="server" Text="Search On" />
                                    </div>

                                    <div class="col-lg-3 label2">
                                        <telerik:RadComboBox ID="ddlSearch" runat="server" Width="100%" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged" AutoPostBack="true"
                                            Style="padding-left: 5px; padding-right: 2px;">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Text=" " Value="ALL" />
                                                <telerik:RadComboBoxItem runat="server" Value="UHID" Text='<%$ Resources:PRegistration, Regno%>' />
                                                <telerik:RadComboBoxItem runat="server" Value="PatientName" Text="Patient Name" />
                                                <telerik:RadComboBoxItem runat="server" Value="BedNo" Text="Bed No" />
                                                <telerik:RadComboBoxItem runat="server" Value="Ward" Text="Ward" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>

                                    <div class="col-lg-3 label2">
                                        <asp:TextBox ID="txtsearchType" runat="server" CssClass="findPatientInput-Mobile" onkeypress="return runScript(event)" Height="22px" Width="100%"></asp:TextBox>
                                    </div>

                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="col-lg-7">
                                    <asp:Button ID="btnFilter" runat="server" AccessKey="S" CssClass="btn btn-primary" Text="Filter" ToolTip="Filter..." ValidationGroup="save" OnClick="btnFilter_Click" />
                                    <asp:Button ID="btnFilterReset" runat="server" AccessKey="C" CssClass="btn btn-default" Text="Filter Reset" ToolTip="Filter" OnClick="btnFilterReset_Click" />

                                </div>

                                <div class="col-lg-5 text-right" style="padding-top: 3px; padding-left: 0; padding-right: 0;background-color: #ff06;">
                                    <asp:Label ID="lnkTotalRecord" runat="server" ToolTip="Total Records" Font-Bold="false" ForeColor="Black" Text="Total Records" />
                                    <asp:Label ID="lblTotalRecord" Font-Bold="true" runat="server" ForeColor="Maroon" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="container-fluid">
                <div class="row form-group">
                    <div class="container-fluid">
                        <asp:Panel ID="pnlgvService" runat="server" Width="100%">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>

                                    <telerik:RadGrid ID="gvOrderApproval" runat="server" RenderMode="Lightweight" Skin="Office2007"
                                        AutoGenerateColumns="false" ItemStyle-Height="30px"
                                        BorderWidth="0" PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="true"
                                        ShowStatusBar="true" EnableLinqExpressions="false" AllowPaging="true" AllowAutomaticDeletes="false"
                                        ShowFooter="false" PageSize="50" AllowCustomPaging="false" OnPageIndexChanged="gvOrderApproval_PageIndexChanged"
                                        OnItemDataBound="gvOrderApproval_ItemDataBound" AllowSorting="true" OnItemCommand="gvOrderApproval_ItemCommand" OnPreRender="gvOrderApproval_PreRender">
                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" />

                                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                AllowColumnResize="false" />
                                            <Scrolling AllowScroll="True" ScrollHeight="550px" UseStaticHeaders="true" FrozenColumnsCount="6" />
                                        </ClientSettings>
                                        <MasterTableView TableLayout="Auto">

                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                                            </NoRecordsTemplate>
                                            <ItemStyle Wrap="false" />
                                            <Columns>
                                                

                                                <telerik:GridTemplateColumn HeaderText="Patient Name" SortExpression="PatientName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPatientName" runat="server" Text='<%# Eval("PatientName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("id") %>' />
                                                        <asp:HiddenField ID="hdnOrderType" runat="server" Value='<%# Eval("OrderType") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="180px" HorizontalAlign="Left"></HeaderStyle>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText='<%$ Resources:PRegistration, Regno%>' SortExpression="RegistrationNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationId" runat="server" Text='<%# Eval("RegistrationNo") %>'></asp:Label>
                                                         <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%# Eval("RegistrationId") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" HorizontalAlign="Left"></HeaderStyle>
                                                </telerik:GridTemplateColumn>


                                                <telerik:GridTemplateColumn HeaderText="Bed No" SortExpression="BedNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBedNo" runat="server" Text='<%# Eval("BedNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="150px" HorizontalAlign="Left"></HeaderStyle>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Order Type" SortExpression="OrderType">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblOrderType" runat="server" Text='<%# Eval("OrderName") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Order Name" SortExpression="ServiceName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                                      <asp:ImageButton ID="ibtnForNotes" runat="server" ImageUrl="~/Images/NotesNew.png"
                                                            ToolTip="Click to show  Clinical Notes." Visible="false" OnClick="ibtnForNotes_Click" CommandName="SEL" AlternateText='<%#Eval("EncounterId")%>' CommandArgument='<%#Eval("TemplateId")%>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="320px" HorizontalAlign="Left"></HeaderStyle>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Status" SortExpression="TestStatus">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("TestStatus") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Order Date" SortExpression="OrderDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("OrderDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="130px" HorizontalAlign="Left"></HeaderStyle>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Encoded By" SortExpression="EncodedBy">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncodedBy" runat="server" Text='<%# Eval("EncodedBy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                      <HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Approving Doctors" SortExpression="AdvisingDoctorName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdvisingDoctorName" runat="server" Text='<%# Eval("AdvisingDoctorName") %>'></asp:Label>
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
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

