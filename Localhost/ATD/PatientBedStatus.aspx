<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="PatientBedStatus.aspx.cs" Inherits="ATD_PatientBedStatus" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #868686 1px !important; border-top:none !important; outline:none !important; color:#333; background: 0 -2300px repeat-x #c1e5ef !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {background-color:#fff !important;}
        #ctl00_ContentPlaceHolder1_Panel1 { background-color:#c1e5ef !important;}
        .RadGrid .rgFilterBox {height:20px;}
        .RadGrid_Office2007 .rgFilterRow {background: #c1e5ef;}
    </style>

    <script type="text/javascript">
        function CloseCheckList(OWnd, args) {
            $get('<%=btnSaveAfterChecklist.ClientID%>').click();
        }
    </script>

    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1"></telerik:RadCodeBlock>
        <asp:UpdatePanel ID="updatepanelmain" runat="server">
            <ContentTemplate>
                <div class="container-fluid header_main">
                    <div class="col-md-3 col-sm-3"><h2></h2></div>
                    <div class="col-md-6 col-sm-6 text-center">
                        <div class="PD-TabRadioNew01 margin_z">
                            <asp:RadioButtonList ID="rdlbedlist" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" Width="100%" OnSelectedIndexChanged="rdlbedlist_SelectedIndexChanged">
                                <asp:ListItem Value="I" Text="Transfer / Discharge Approval" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="H" Text="Bed Released"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 text-right">
                        <asp:Button ID="btnSaveAfterChecklist" runat="server" Text="Save" CssClass="btn btn-default" Style="visibility:hidden" OnClick="btnSaveAfterChecklist_Click" />
                    <%--    <asp:Button ID="btnclose" runat="server" Text="Close" CausesValidation="false" SkinID="Button"
                        OnClientClick="window.close();" />--%>
                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnsave_Click" />
                    </div>
                </div>

                <div class="container-fluid">
                    <div class="row form-group">
                        <div class="col-md-12 col-sm-12 text-center">
                            <asp:Label ID="lblmsg" runat="server" Font-Bold="True"></asp:Label>
                        </div>
                    </div>

                    <div class="row form-group">
                        <telerik:RadGrid ID="gvNameList" runat="server" Width="100%" Height="530px" AllowPaging="true"
                            AllowFilteringByColumn="true" PageSize="15" AllowMultiRowSelection="True" Skin="Office2007"
                            ShowStatusBar="true" AutoGenerateColumns="False" OnPreRender="gvNameList_PreRender"
                            OnItemCommand="gvNameList_ItemCommand" OnItemDataBound="gvNameList_OnItemDataBound">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric" />
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <MasterTableView Width="100%" AllowFilteringByColumn="true" TableLayout="Fixed">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridButtonColumn Text="Select" CommandName="Select" HeaderStyle-Width="45px"
                                        HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="<%$ Resources:PRegistration, Regno%>"
                                        AllowFiltering="true" DataField="RegistrationNo" FilterControlWidth="99%" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="IP NO" AllowFiltering="true"
                                        DataField="EncounterNo" FilterControlWidth="99%" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Patient Name" AllowFiltering="true"
                                        DataField="PatientName" FilterControlWidth="99%" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Transfer/Discharge Date" AllowFiltering="true"
                                        DataField="DischargeDate" FilterControlWidth="99%" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDischargeDate" runat="server" Text='<%#Eval("DischargeDate") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Discharge Approval Date" AllowFiltering="true"
                                        DataField="DischargeApprovalDate" FilterControlWidth="99%" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDischargeApprovalDate" runat="server" Text='<%#Eval("DischargeApprovalDate") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Ward Name" AllowFiltering="true"
                                        DataField="WardName" FilterControlWidth="99%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="BedCategory Name" AllowFiltering="true"
                                        DataField="BedCategoryName" FilterControlWidth="99%" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBedCategoryName" runat="server" Text='<%#Eval("BedCategoryName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Bed No" AllowFiltering="true"
                                        DataField="BedNo" FilterControlWidth="99%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                     <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Type" AllowFiltering="true"
                                        DataField="BedNo" FilterControlWidth="99%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Registration Id" AllowFiltering="False"
                                        Visible="false" DataField="RegistrationId" FilterControlWidth="99%" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Encounter Id" AllowFiltering="False"
                                        Visible="false" DataField="EncounterId" FilterControlWidth="99%" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterId" runat="server" Text='<%#Eval("EncounterId") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Bed Id" AllowFiltering="False"
                                        Visible="false" DataField="BedId" FilterControlWidth="99%" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBedId" runat="server" Text='<%#Eval("BedId") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                       <telerik:GridButtonColumn Text="Print" CommandName="Print" HeaderStyle-Width="95px"
                                        HeaderText="Final Gate Pass" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>

                    </div>

                    <div class="row form-group">
                        <asp:HiddenField ID="hdnRegid" runat="server" />
                        <asp:HiddenField ID="hdnEncid" runat="server" />
                        <asp:HiddenField ID="hdnBedid" runat="server" />
                        <asp:HiddenField ID="hdnregno" runat="server" />
                        <asp:HiddenField ID="hdnType" runat="server" />
                        <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>