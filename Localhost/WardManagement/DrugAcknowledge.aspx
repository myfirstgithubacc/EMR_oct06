<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DrugAcknowledge.aspx.cs" Inherits="EMR_Medication_MedicationDispense"
    Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock ID="radblock" runat="server">
        <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../../Include/css/mainNew.css" rel="stylesheet" />
        <link href="../../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
        <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .blink {
                text-decoration: blink;
            }

            .blinkNone {
                text-decoration: none;
            }



            tr.clsGridheaderorderNew:nth-of-type(odd) {
                background: #eee;
            }

            th {
                background: #3498db;
                color: white;
            }

            tr.clsGridRoworderNew td {
                padding: 3px 5px;
                border: 1px solid #ccc;
                text-align: left;
                font-size: 14px;
                white-space: nowrap;
            }

            tr.clsGridheaderorderNew th {
                padding: 8px 5px;
                border: 1px solid #ccc;
                text-align: left;
                font-size: 14px;
                white-space: nowrap;
            }

            tr.clsGridheaderorderNew {
                border: 1px solid #ccc;
            }

            tr.pagination1 {
                margin: 2px 0 0px 0;
            }

                tr.pagination1 td {
                    background: #3498db;
                    list-style: none;
                    padding: 1px 2px;
                }

                tr.pagination1 a, tr.pagination1 span {
                    text-decoration: none;
                    color: #fdfdfd !important;
                    height: 22px;
                    width: 22px;
                    font-size: 12px;
                    padding-top: 1px;
                    display: flex;
                    border: 1px solid rgba(0, 0, 0, 0.25);
                    border-right-width: 0px;
                    box-shadow: inset 0px 1px 0px 0px rgba(255, 255, 255, 0.35);
                    padding: 2px 4px;
                }

                tr.pagination1:last-child a {
                    border-right-width: 1px;
                }

                tr.pagination1 a:hover {
                    background: rgba(255, 255, 255, 0.2);
                    border-top-color: rgba(0, 0, 0, 0.35);
                    border-bottom-color: rgba(0, 0, 0, 0.5);
                }

                tr.pagination1 a:focus,
                tr.pagination1 a:active {
                    padding-top: 4px;
                    border-left-width: 1px;
                    background: rgba(255, 255, 255, 0.15);
                    box-shadow: inset 0px 2px 1px 0px rgba(0, 0, 0, 0.25);
                }

                tr.pagination1 td:first-child span {
                    padding-right: 8px;
                }

                tr.pagination1 td:last-child span {
                    padding-left: 8px;
                }

            input[type='text'].Textbox {
                padding: 3px 10px !important;
            }

            td.rcbInputCell.rcbInputCellLeft .rcbInput {
                padding: 3px 8px !important;
            }

            #ctl00_ContentPlaceHolder1_lblMessage {
                margin: 0;
                padding: 0;
                width: 100%;
                position: relative;
            }
        </style>
        <script language="javascript" type="text/javascript">
            function returnToParent() {
                var oArg = new Object();
                oArg.IndentId = $get('<%=hdnSelectedIndentId.ClientID%>').value;
                oArg.IndentNo = $get('<%=hdnSelectedIndentNo.ClientID%>').value;
                oArg.RegistrationId = $get('<%=hdnSelectedRegistrationId.ClientID%>').value;

                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function SelectAllDetails(id) {
                //get reference of GridView control
                var grid = document.getElementById("<%=gvIssueDurgDetail.ClientID%>");
                //variable to contain the cell of the grid
                var cell;


                if (grid.rows.length > 0) {
                    //loop starts from 1. rows[0] points to the header.
                    for (ridx = 1; ridx < grid.rows.length; ridx++) {
                        //get the reference of first column
                        cell = grid.rows[ridx].cells[0];

                        //loop according to the number of childNodes in the cell
                        for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                            //if childNode type is CheckBox
                            if (cell.childNodes[cIdx].type == "checkbox") {
                                //assign the status of the Select All checkbox to the cell checkbox within the grid
                                cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                            }
                        }
                    }
                }

            }
            function Blink() {

                if (document.getElementById("lnkAllergyDetails"))
                    //Here you have to mention control name instead of blinkme
                {

                    var d = document.getElementById("lnkAllergyDetails");
                    //Here you have to mention control name instead of blinkme
                    d.style.color = (d.style.color == 'red' ? 'white' : 'red');

                    setTimeout('Blink()', 1000);
                }
            }
        </script>


    </telerik:RadCodeBlock>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container-fluid header_main">
                <div class="row">
                    <div class="col-md-2">
                        <h2>
                            <asp:Label ID="lblHeader" runat="server" Text="Drug&nbsp;Acknowledge" /></h2>
                    </div>
                    <div class="col-md-6 text-center">
                        <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" />
                    </div>
                    <div class="col-md-4 text-right">
                        <asp:Button ID="BtnAcknowledge" runat="server" Text="Acknowledge" CssClass="btn btn-primary" OnClick="BtnAcknowledge_Click" />

                        <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnFilter_Click" />
                        <asp:Button ID="btnClearFilter" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnClearFilter_Click" />
                        <%-- <asp:Button ID="btnPrint" runat="server" SkinID="Button" Text="Print Prescription" OnClick="btnPrint_Click" CausesValidation="false" />
                    <asp:Button ID="btnPrinLable" runat="server" SkinID="Button" Text="Print Label" OnClick="btnPrinLable_Click" CausesValidation="false" />--%>
                    </div>
                </div>
            </div>

            <table cellspacing="0" class="table table-small-font table-bordered table-striped margin_z">
                <tbody>
                    <tr align="center">
                        <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                            <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>





            <div class="container-fluid" style="background-color: #fff;">
                <div class="row form-groupTop">
                    <div class="col-md-3 col-4">
                        <div class="row">
                            <div class="col-md-3 label2">
                                <asp:Label ID="Label4" runat="server" Text="Status&nbsp" />
                            </div>
                            <div class="col-md-9">
                                <telerik:RadComboBox ID="ddlAcknowledge" runat="server" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="ALL" />
                                        <telerik:RadComboBoxItem Text="Acknowleged" Value="A" />
                                        <telerik:RadComboBoxItem Text="UnAcknowleged" Value="U" Selected="true" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-4">
                        <div class="row">
                            <div class="col-md-3 label2">
                                <asp:Label ID="lblWard" runat="server" Text="Ward" />
                            </div>
                            <div class="col-md-9 mt-1 m-md-0">
                                <telerik:RadComboBox ID="ddlWard" runat="server" Width="100%" AutoPostBack="true"></telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-4">
                        <div class="row">
                            <div class="col-md-3 label2">
                                <asp:Label ID="Label1" runat="server" Text="Issue No" />
                            </div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtIssueNo" runat="server" SkinID="textbox" MaxLength="15" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-4">
                        <asp:Panel ID="Panel3" runat="server" DefaultButton="btnFilter"></asp:Panel>
                    </div>
                </div>
            </div>



            <%-- 
                        <asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>' SkinID="label" />
                        <telerik:RadComboBox ID="ddlProvider" MarkFirstMatch="true" runat="server" Width="150px" DropDownWidth="250px" />
            --%>

            <%-- <telerik:RadComboBox ID="ddlAcknowledgeStatus" SkinID="DropDown" runat="server" Width="120px" Height="22px" >
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="0"   />
                                <telerik:RadComboBoxItem Text="Acknowlege" Value="1"  />
                                <telerik:RadComboBoxItem Text="UnAcknowlege" Value="2" />
                            </Items>
                        </telerik:RadComboBox>
            --%>

            <%-- 
                        <asp:TextBox ID="txtSearch" runat="server" SkinID="textbox" Width="100px" MaxLength="20" />
                        <asp:TextBox ID="txtRegNo" runat="server" SkinID="textbox" Width="100px" MaxLength="20" Visible="false" />
                        <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                        <asp:UpdatePanel ID="btnupd1" runat="server"><ContentTemplate >
                        <asp:Button ID="btnFetchforInventory" runat="server" SkinID="Button" Text="Fetch for Inventory" OnClick="btnFetchforInventory_Click" />
                        </ContentTemplate></asp:UpdatePanel> 
            --%>



            <div class="container-fluid" style="background-color: #fff;">
                <div class="row form-group margin_Top">
                    <div class="col-12">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvPatientDurgStatus" />
                            </Triggers>

                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" Height="235px" Width="100%" ScrollBars="Auto"
                                    BorderWidth="0px" BorderColor="LightBlue">
                                    <asp:GridView ID="gvPatientDurgStatus" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="False"
                                        Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="true"
                                        PageSize="6" OnPageIndexChanging="gvPatientDurgStatus_OnPageIndexChanging" OnSelectedIndexChanged="gvPatientDurgStatus_SelectedIndexChanged"
                                        OnRowDataBound="gvPatientDurgStatus_RowDataBound" OnRowCommand="gvPatientDurgStatus_OnRowCommand">
                                        <Columns>
                                            <asp:CommandField HeaderText='Select' ControlStyle-ForeColor="Blue" SelectText="Select"
                                                ShowSelectButton="true" HeaderStyle-Width="20px">
                                                <ControlStyle ForeColor="Blue" />
                                            </asp:CommandField>
                                            <%--<asp:TemplateField HeaderText='Facility' HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFacilityShortName" runat="server" Width="100%" Text='<%#Eval("FacilityShortName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>


                                        <asp:TemplateField HeaderText='Ward Name.' HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWardName" runat="server" Width="100%" Text='<%#Eval("WardName")%>' />
                                                <asp:HiddenField ID="hdnWardId" runat="server" Value='<%#Eval("WardId")%>' />
                                                <asp:HiddenField ID="hdnLoginStoreId" runat="server" Value='<%#Eval("LoginStoreId")%>' />
                                                <asp:HiddenField ID="hdnIssueId" runat="server" Value='<%#Eval("IssueId")%>' />
                                                <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId")%>' />
                                                <asp:HiddenField ID="hdnAdvisingDoctorId" runat="server" Value='<%#Eval("AdvisingDoctorId")%>' />
                                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText='Store Name' HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStoreID" runat="server" Width="100%" Text='<%#Eval("StoreID")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText='Issue No' HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssueNo" runat="server" Width="100%" Text='<%#Eval("IssueNo")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Issued Date' HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssuedDate" runat="server" Width="100%" Text='<%#Eval("IssuedDate")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText='Issued By' HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssuedBy" runat="server" Width="100%" Text='<%#Eval("IssuedBy")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText='Order No' HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderNo" runat="server" Width="100%" Text='<%#Eval("OrderNo")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText='Order Date' HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderDate" runat="server" Width="100%" Text='<%#Eval("OrderDate")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText='Ordered By' HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderBy" runat="server" Width="100%" Text='<%#Eval("OrderBy")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText='Advising Doctor' HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdvisingDoctor" runat="server" Width="100%" Text='<%#Eval("AdvisingDoctor")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%-- <asp:TemplateField HeaderText='Facility' HeaderStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFacilityName" runat="server" Width="100%" Text='<%#Eval("FacilityName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-12">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvPatientDurgStatus" />
                                <asp:AsyncPostBackTrigger ControlID="BtnAcknowledge" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" Height="210px" Width="100%" ScrollBars="Auto"
                                    BorderWidth="0px" BorderColor="LightBlue">
                                    <asp:GridView ID="gvIssueDurgDetail" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="False"
                                        Height="100%" Width="100%" CellPadding="0" CellSpacing="0" OnRowDataBound="gvIssueDurgDetail_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAllD" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRowD" runat="server" Enabled="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                    <asp:HiddenField ID="hdnIssueDetailsId" runat="server" Value='<%#Eval("IssueDetailsId")%>' />
                                                    <asp:HiddenField ID="hdnIssueID" runat="server" Value='<%# Eval("IssueId") %>' />



                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issue No" Visible="false">
                                            <ItemTemplate>
                                                <%--   <asp:Label ID="lblIssueNo" runat="server" Width="100%" SkinID="label" Text='<%# Eval("IssueNo") %>' />--%>
                                                <%--<asp:Label ID="lblIssueID" runat="server" Width="100%" SkinID="label" Text='<%# Eval("IssueNo") %>' />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Drug">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDurgName" runat="server" Width="50%" Text='<%# Eval("DurgName") %>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issued.Qty" ItemStyle-Width="65px" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" Text='<%#Eval("Qty","{0:f0}")%>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks")%>' MaxLength="100" Width="100%"></asp:TextBox>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Acknowledge By" ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAcknowledgeBy" runat="server" Text='<%#Eval("AcknowledgeBy")%>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Acknowledge Date" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAcknowledgeDate" runat="server" Text='<%#Eval("AcknowledgeDate")%>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>

            <%--  <asp:Table ID="tblLegend" runat="server" border="0" CellPadding="2" CellSpacing="0">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label20" runat="server" BorderWidth="1px" BackColor="Bisque" SkinID="label"
                            Width="22px" Height="14px" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label21" runat="server" SkinID="label" Text="Ac&nbsp;Dispensed" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label22" runat="server" BorderWidth="1px" BackColor="LightSteelBlue"
                            SkinID="label" Width="22px" Height="14px" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label23" runat="server" SkinID="label" Text="Dispensed" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label7" runat="server" BorderWidth="1px" BackColor="PaleTurquoise"
                            SkinID="label" Width="22px" Height="14px" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label8" runat="server" SkinID="label" Text="Hospital Formulary" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label9" runat="server" BorderWidth="1px" BackColor="DarkOliveGreen"
                            SkinID="label" Width="22px" Height="14px" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label10" runat="server" SkinID="label" Text="Billed" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>--%>




            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnSelectedIndentId" runat="server" />
                        <asp:HiddenField ID="hdnSelectedIndentNo" runat="server" />
                        <asp:HiddenField ID="hdnSelectedRegistrationId" runat="server" />
                        <asp:HiddenField ID="hndSelectedIsInsuranceCompany" runat="server" />
                        <asp:HiddenField ID="hdnItemId" runat="server" />
                        <asp:HiddenField ID="hndItemName" runat="server" />
                        <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                        <asp:HiddenField ID="hdnIsValidPassword" runat="server" />


                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                            </Windows>
                        </telerik:RadWindowManager>
                        <div id="divPrescriptionRemarks" runat="server" visible="false" style="width: 350px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: Silver; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 130px; left: 340px; top: 230px">
                            <table width="100%" border="0" cellpadding="0" cellspacing="2">
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="Label13" runat="server" Font-Bold="true" Text="Prescription Remarks" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:TextBox ID="txtPrescriptionRemarks" ReadOnly="true" runat="server" SkinID="textbox"
                                            MaxLength="1000" TextMode="MultiLine" Style="min-height: 75px; max-height: 75px; min-width: 340px; max-width: 340px;"
                                            Width="340px" Height="75px" onkeyup="return MaxLenTxt(this, 250);" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
