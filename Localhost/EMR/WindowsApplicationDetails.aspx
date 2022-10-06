<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="WindowsApplicationDetails.aspx.cs" Inherits="EMR_WindowsApplicationDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplNewEMR" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelRegAttachDocument.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" type="text/javascript"></script>--%>

    <script src="../Include/JS/jquery1.6.4.min.js" type="text/javascript"></script>
    <script src="../Include/JS/jquery1.11.3.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        function showResultPopup(e, Inv_Result) {
            alert("1");
            $get('<%=hdnShowInv_Result.ClientID%>').value = Inv_Result;
                    $get('<%=btnShowInv_Result.ClientID%>').click();
         }
    </script>

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />




    <div class="VisitHistoryDiv" style="margin-bottom: 1em;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-3 col-sm-3">
                    <div class="WordProcessorDivText">
                        <h2>
                            <asp:Label ID="Label5" runat="server" Text="&nbsp;Lab Results"></asp:Label></h2>
                    </div>
                </div>

                <div class="col-md-4">
                </div>


                <div class="col-md-3 col-sm-3">
                    <div class="WordProcessorDivText">
                        <h4>
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Text="&nbsp;" /></h4>
                    </div>
                </div>

            </div>
        </div>
        <div class="VisitHistoryBorderNew">
            <div class="container-fluid">
                <div class="row">
                    <%--<asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />--%>
                    <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    <asplNewEMR:UserDetailsHeader ID="asplHeaderUDEMR" runat="server" />
                </div>
            </div>
        </div>
    </div>



    <div class="col-md-2">
        <div class="mmk1 radioo">
            <asp:RadioButtonList ID="rdoInvestigations" RepeatDirection="Horizontal" RepeatLayout="Table" runat="server" OnSelectedIndexChanged="rdoInvestigations_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Text="All" Value="0"></asp:ListItem>

                <asp:ListItem Text="Selected" Value="1"></asp:ListItem>

            </asp:RadioButtonList><%--<span class="margin_bottom">( List Of Investigation )</span> --%>
        </div>

    </div>
    <div class="col-md-3">
        <span class="">
            <asp:Label ID="Label4" runat="server" Text="From" />
            <span>
                <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" MinDate="01/01/1900" DateInput-ReadOnly="true">
                    <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                </telerik:RadDatePicker>
            </span>
            <asp:Label ID="Label6" runat="server" Text="To" />
            <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" MinDate="01/01/1900" DateInput-ReadOnly="true">
                <DateInput DateFormat="dd/MM/yyyy"></DateInput>
            </telerik:RadDatePicker>
        </span>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>


            <div class="col-md-5">
                <div class="mmk1 radioo">
                    <asp:RadioButtonList ID="rdoResultView" RepeatDirection="Horizontal" RepeatLayout="Table" runat="server" OnSelectedIndexChanged="rdoResultView_OnSelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Horizontal View" Value="XA" Selected="True" />
                        <asp:ListItem Text="Grid View" Value="YA" />
                    </asp:RadioButtonList>
                </div>
            </div>


            <div class="col-md-1 pull-left">
                <span class="">
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                    <asp:Button ID="btnRefreshData" runat="server" Text="Refresh" CssClass="SearchKeyBtn" OnClick="btnRefreshData_Click" />
                    <%--   </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    <asp:Label ID="Label1" ForeColor="Green" runat="server" />

                </span>
            </div>

            <div class="VitalHistory-Div02">
                <div class="container-fluid">
                    <div class="row">
                        <%-- <asp:UpdatePanel ID="upMessage" runat="server">
                        <ContentTemplate>--%>

                        <style>
                            .mmk1 label {
                                font-weight: normal;
                                font-size: 12px;
                                line-height: 21px;
                                padding-top: 0px;
                            }
                        </style>

                        <br />
                        <div class="col-md-3">
                            <%-- <span class="style1">  
                                <strong>List Of Investigations</strong>  
                            </span>  --%>
                            <style>
                                .mmk label {
                                    font-weight: normal;
                                    font-size: 12px;
                                    line-height: 21px;
                                    padding-top: 0px;
                                    overflow-y: auto;
                                    text-transform: none;
                                }
                            </style>
                            <div class="mmk" style="height: 380px; overflow: scroll">
                                <%--<div  class="mmk">--%>
                                <asp:CheckBoxList ID="ChkInvestigation" AutoPostBack="false" Font-Bold="false" runat="server"></asp:CheckBoxList>
                            </div>

                        </div>
                        <div class="col-md-9">
                            <asp:GridView ID="gvLabDetailsXaxis" runat="server" AutoGenerateColumns="true" Width="100%" SkinID="gridview2" HeaderStyle-Font-Size="7px"
                                RowStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" PageSize="14" AllowPaging="false" OnPageIndexChanging="gvLabDetailsXaxis_OnPageIndexChanging" OnRowDataBound="gvLabDetailsXaxis_OnRowDataBound">
                            </asp:GridView>
                            <telerik:RadGrid ID="gvDetails" Skin="Office2007" Visible="false" Width="100%" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false" ShowFooter="false" GridLines="Both" AllowPaging="true" PageSize="14" OnPageIndexChanged="gvDetails_PageIndexChanged" OnItemDataBound="gvDetails_OnItemDataBound">
                                <HeaderStyle HorizontalAlign="Left" />
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="false" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">

                                    <Selecting UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                </ClientSettings>
                                <MasterTableView AllowFilteringByColumn="false">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Record Found.
                                        </div>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Requisition Date" HeaderStyle-Width="150" UniqueName="RequisitionDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequisitionDate" runat="server" Text='<%#Eval("RequisitionDate")%>' />
                                                <asp:HiddenField ID="hdnReport_type" runat="server" Value='<%#Eval("Report_type")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Investigation Name" HeaderStyle-Width="300" UniqueName="InvestigationName" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvestigationName" runat="server" Text='<%#Eval("InvestigationName")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="150" HeaderText="Investigation Result" UniqueName="Inv_Result" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInv_Result" runat="server" Text='<%#Eval("Inv_Result")%>' />
                                                <asp:LinkButton ID="lnkInv_Result" Visible="false" runat="server" Text="View" CssClass="text-center" OnClick="lnkInv_Result_click" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>


                        <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move" />
                            </Windows>
                        </telerik:RadWindowManager>


                        <asp:HiddenField ID="hdnShowInv_Result" runat="server" />

                    </div>
                </div>
            </div>

        </ContentTemplate>

    </asp:UpdatePanel>
    <asp:Button ID="btnShowInv_Result" runat="server" Style="visibility: hidden;" OnClick="btnShowInv_Result_OnClick" />

</asp:Content>

