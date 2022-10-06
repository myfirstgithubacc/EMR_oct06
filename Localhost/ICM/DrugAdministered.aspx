<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="DrugAdministered.aspx.cs" Inherits="ICM_DrugAdministered" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>
<%--<%@ Register Src="../Include/Components/Legend.ascx" TagName="Legend" TagPrefix="uc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel='stylesheet' type='text/css' />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {
            border: solid #a5a5a5 1px !important;
            padding: 0 0 0 5px !important;
            border-top: none !important;
            border-left: none !important;
            outline: none !important;
            color: #333;
            background: 0 -2300px repeat-x #c1e5ef !important;
            height: 25px;
        }
        div#ctl00_ContentPlaceHolder1_UpdatePanel1{
            overflow-x:hidden!important;
        }
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {
            background-color: #fff !important;
        }

        #ctl00_ContentPlaceHolder1_Panel1 {
            background-color: #c1e5ef !important;
        }

        .RadGrid .rgFilterBox {
            height: 20px !important;
        }

        .RadGrid_Office2007 .rgFilterRow {
            background: #c1e5ef !important;
        }

        .RadGrid_Office2007 .rgPager {
            background: #c1e5ef 0 -7000px repeat-x !important;
            color: #00156e !important;
        }
    </style>


    <telerik:RadCodeBlock ID="radblock" runat="server">
        <style type="text/css">
            .wrap {
                white-space: normal;
                width: 68px !important;
            }

            .pinned {
                position: fixed; /* i.e. not scrolled */
                background-color: White; /* prevent the scrolled columns showing through */
                z-index: 100; /* keep the pinned on top of the scrollables */
            }
        </style>

        <script language="javascript" type="text/javascript">
            function wndAddService_OnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var xmlString = arg.xmlString;
                    $get('<%=hdnXmlString.ClientID%>').value = xmlString;
                }
                $get('<%=btnBindGridWithXml.ClientID%>').click();
            }

            function wndAdddosetime_OnClientClose(oWnd, args) {
                $get('<%=btnAddDosageTime.ClientID%>').click();
            }


            function OpenCIMSWindow() {
                var ReportContent = $get('<%=hdnCIMSOutput.ClientID%>')

                var WindowObject = window.open('', 'PrintWindow2', 'width=1250,height=585,top=72,left=30,toolbars=yes,scrollbars=yes,status=no,resizable=yes');
                WindowObject.document.writeln(ReportContent.value);
                WindowObject.document.close();
                WindowObject.focus();
            }

        </script>
    </telerik:RadCodeBlock>



    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main">
                <div class="col-md-3">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" Text="Drug Administration" />
                        <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                        <asp:Button ID="btnBindGridWithXml" runat="server" CausesValidation="false" Style="visibility: hidden; float: left; height: 1px; margin: 0; padding: 0;" Width="1" OnClick="btnBindGridWithXml_OnClick" Text="" />
                        <asp:Button ID="btnAddDosageTime" runat="server" CausesValidation="false" Style="visibility: hidden; float: left; height: 1px; margin: 0; padding: 0;" Width="1" OnClick="btnAddDosageTime_OnClick" Text="" />
                    </h2>
                </div>
                <div class="col-md-6 text-center">
                    <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
                <div class="col-md-3"></div>
            </div>

            <%-- <td align="left" width="85%">
                <table cellpadding="0" cellspacing="0" width="100%" class="notification" runat="server" visible="false">
                    <tr>
                        <td align="left" width="75%">
                            <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient : " SkinID="label" />
                            <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                            <asp:Label ID="Label5" runat="server" Text="DOB : " Visible="false" SkinID="label" />
                            <asp:Label ID="lblDob" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                            <asp:Label ID="Label4" runat="server" Text="Mobile No : " SkinID="label" />
                            <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                            <asp:Label ID="lblIpno" runat="server" Text="IP No : " SkinID="label" />
                            <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                            <asp:Label ID="Label6" runat="server" Text="Admission Date : " SkinID="label" />
                            <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                        </td>
                    </tr>
                </table>
            </td>--%>

            <%--<table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px;
                border-style: solid none solid none; border-width: 1px; border-color: #808080;"
                cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />
                    </td>
                </tr>
            </table>--%>

         
                <div class="row form-group">
                    <div class="col-12">
                    <asplUD:UserDetails ID="asplUD" runat="server" />
                    <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" Visible="false"></asp:Label>
                        </div>
                </div>
          

            <div class="container-fluid">
                <div class="row form-group">
                    <div class="col-md-6">
                        <asp:Label ID="Label1" runat="server" Text=" S - Scheduled Drug Time &nbsp;&nbsp;&nbsp;&nbsp; A - Administered Drug Time &nbsp;&nbsp;&nbsp;&nbsp; C - Continue Infusion Drug Time " Font-Bold="true" /></div>
                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-2 label2 PaddingRightSpacing"><strong>Color Legends</strong></div>
                            <div class="col-md-10 PaddingLeftSpacing">
                                <asp:TextBox ID="txtDueDose" runat="server" Text="Due Dose" Width="15%" CssClass="LegendColor text-center PaddingSpacing02" ReadOnly="true" />
                                <asp:TextBox ID="txtTS" runat="server" Text="Next Due Dose" Width="20%" CssClass="LegendColor text-center PaddingSpacing02" ReadOnly="true" />
                                <%--Slot Time--%>
                                <asp:TextBox ID="txtDelay" runat="server" Text="Delay" Width="10%" CssClass="LegendColor text-center PaddingSpacing02" ReadOnly="true" />
                                <asp:TextBox ID="txtNotAdministratedTime" runat="server" Width="25%" Text="Not Administered" CssClass="LegendColor text-center PaddingSpacing02" ReadOnly="true" />
                                <asp:TextBox ID="txtAdministered" runat="server" Width="20%" Text="Administered" CssClass="LegendColor text-center PaddingSpacing02" ReadOnly="true" />
                                <%-- <asp:TextBox ID="txtGivenTimely" runat="server" Text="" BorderWidth="2" SkinID="label" ReadOnly="true" Width="20px"  /> Given Timely--%>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-md-6">
                        <span class="label2">Date -</span>
                        <asp:Label ID="lblDate" runat="server" Text="" Font-Bold="true" />
                        <asp:Button ID="btnPrevious" runat="server" CssClass="btn btn-primary" Text="Previous" OnClick="btnPrevious_OnClick" />
                        <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" Text="Next" OnClick="btnUpdate_OnClick" />
                        <asp:CheckBox ID="chkShowDetails" runat="server" Text="Show Details" AutoPostBack="true" OnCheckedChanged="chkShowDetails_OnCheckedChanged" />
                        <asp:Button ID="btnHistory" runat="server" CssClass="btn btn-primary" Text="History" OnClick="btnHistory_OnClick" />
                        <%--<telerik:RadButton ID="btnH" runat="server" Text="H\n" />--%>
                    </div>
                </div>

            </div>

            <div class="container-fluid" style="background-color: #fff;">
                <div class="row">
                    <%--<asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>     OnColumnCreated="gvDrugAdministered_ColumnCreated" --%>
                    <telerik:RadGrid ID="gvDrugAdministered" runat="server" BorderWidth="0" ShowGroupPanel="false"
                        Width="100%" Height="450px" AllowPaging="false" Skin="Office2007" OnItemDataBound="gvDrugAdministered_ItemDataBound"
                        AutoGenerateColumns="false" OnItemCommand="gvDrugAdministered_OnItemCommand">
                        <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="false" Scrolling-UseStaticHeaders="true"
                            Scrolling-SaveScrollPosition="false">
                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" EnableDragToSelectRows="false" />
                            <Resizing AllowRowResize="false" EnableRealTimeResize="true" ResizeGridOnColumnResize="true"
                                AllowColumnResize="false" />
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                        </ClientSettings>
                        <%--FrozenColumnsCount="40"--%>
                        <MasterTableView TableLayout="Fixed" Width="100%" Font-Size="7" Font-Bold="true">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="" FieldName="DrugTypeName" HeaderText="" FormatString="" />
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="OrderBy" SortOrder="Ascending" />
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="ItemName" DefaultInsertValue="" HeaderText="Item Name"
                                    ItemStyle-Width="350px" HeaderStyle-Width="350px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" Style="font-size: 8pt" Text='<%#Eval("ItemName") %>' />
                                        <asp:Label ID="lblInfusionOrder" runat="server" Text="" Visible="false" />
                                        <asp:Label ID="lblInstruction" runat="server" Text="" Visible="false" />
                                        <asp:LinkButton ID="lbtnMedicineLink" ForeColor="Red" Font-Bold="true" runat="server"
                                            Text="Linked Medicine" OnClick="lbtnMedicineLink_OnClick" Visible="false" />
                                        <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId") %>' />
                                        <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId") %>' />
                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                        <asp:HiddenField ID="hdnFrequencyId" runat="server" Value='<%#Eval("FrequencyId") %>' />
                                        <asp:HiddenField ID="hdnOrderDateTime" runat="server" Value='<%#Eval("OrderDateTime") %>' />
                                        <asp:HiddenField ID="hdnFrequencyName" runat="server" Value='<%#Eval("FrequencyName") %>' />
                                        <asp:HiddenField ID="hdnFrequency" runat="server" Value='<%#Eval("Frequency") %>' />
                                        <asp:HiddenField ID="hdnDrugType" runat="server" Value='<%#Eval("DrugType") %>' />
                                        <asp:HiddenField ID="hdnDrugTypeName" runat="server" Value='<%#Eval("DrugTypeName") %>' />
                                        <asp:HiddenField ID="hdnDoseName" runat="server" Value='<%#Eval("DoseName") %>' />
                                        <asp:HiddenField ID="hdnIsHighAlert" runat="server" Value='<%#Eval("IsHighAlert") %>' />
                                        <asp:HiddenField ID="hdnIsInfusion" runat="server" Value='<%#Eval("IsInfusion") %>' />
                                        <asp:HiddenField ID="hdnDoseTypeName" runat="server" Value='<%#Eval("DoseTypeName") %>' />
                                        <asp:HiddenField ID="hdnFrequencyDetailId" runat="server" Value='<%#Eval("IndentId") %>' />
                                        <asp:HiddenField ID="hdnIsStop" runat="server" Value='<%#Eval("IsStop") %>' />
                                        <asp:HiddenField ID="hdnInstructions" runat="server" Value='<%#Eval("Instructions") %>' />
                                        <asp:HiddenField ID="hdnInfusionOrder" runat="server" Value='<%#Eval("InfusionOrder") %>' />
                                        <asp:HiddenField ID="hdnDifferentialPlottingTwoHourlyValue" runat="server" Value='<%#Eval("DifferentialPlottingTwoHourlyValue") %>' />
                                        <asp:HiddenField ID="hdnLinkMedicine" runat="server" Value='<%#Eval("LinkMedicine") %>' />
                                        <asp:HiddenField ID="hdnMedComDisTime" runat="server" Value='<%#Eval("MedCompletedDiscontinuedTime") %>' />
                                        <asp:HiddenField ID="hdnFoodRelationship" runat="server" Value='<%#Eval("FoodRelationship") %>' />
                                        </br>
                                            <asp:HiddenField ID="hdnDuration" runat="server" Value='<%#Eval("Duration") %>' />
                                        <asp:LinkButton ID="lbtnFrequencyTime" runat="server" Style="font-size: 7pt" Text="Change Dosage Time"
                                            OnClick="lbtnFrequencyTime_OnClick" />
                                        &nbsp;&nbsp;&nbsp;
                                            <asp:HiddenField ID="hdnShowVariable" runat="server" Value='<%#Eval("IsVariableDoseItem") %>' />
                                        <asp:LinkButton ID="lbtnShowVariableDose" runat="server" OnClick="lbtnShowVariableDose_OnClick"
                                            SkinID="label" Text="Show Variable Dose"></asp:LinkButton>
                                        <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                        <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                        <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                        <asp:HiddenField ID="hdnStopDate" runat="server" Value='<%#Eval("StopDate") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false" HeaderText="BD" HeaderTooltip="Brand Details"
                                    HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnBrandDetailsCIMS" runat="server" ToolTip="Click here to view cims brand details"
                                            CommandName="BrandDetailsCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                            Text="&nbsp;" Font-Underline="false" Width="20px" Visible="true" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false" HeaderText="MG" HeaderTooltip="Monograph"
                                    HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnMonographCIMS" runat="server" ToolTip="Click here to view cims monograph"
                                            CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                            Text="&nbsp;" Width="20px" Font-Underline="false" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false" HeaderText="DD" HeaderTooltip="Drug to Drug Interaction"
                                    HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnInteractionCIMS" runat="server" ToolTip="Click here to view cims drug to drug interaction"
                                            CommandName="InteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                            Width="20px" Font-Underline="false" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false" HeaderText="DH" HeaderTooltip="Drug Health Interaction"
                                    HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction"
                                            CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76"
                                            Text="&nbsp;" Font-Underline="false" Width="20px" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false" HeaderText="DA" HeaderTooltip="Drug Allergy Interaction"
                                    HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnDAInteractionCIMS" runat="server" ToolTip="Click here to view cims drug allergy interaction"
                                            CommandName="DAInteractionCIMS" CausesValidation="false" BackColor="#82CAFA"
                                            Text="&nbsp;" Font-Underline="false" Width="20px" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="BD" HeaderTooltip="Brand Details" Visible="false"
                                    HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnBrandDetailsVIDAL" runat="server" ToolTip="Click here to view vidal brand details"
                                            CommandName="BrandDetailsVIDAL" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                            Text="&nbsp;" Width="20px" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="MG" HeaderTooltip="Monograph" Visible="false"
                                    HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnMonographVIDAL" runat="server" ToolTip="Click here to view vidal monograph"
                                            CommandName="MonographVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                            Text="&nbsp;" Width="20px" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="DD" HeaderTooltip="Drug to Drug Interaction"
                                    Visible="false" HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug to drug interaction"
                                            CommandName="InteractionVIDAL" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                            Width="20px" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="DH" HeaderTooltip="Drug Health Interaction"
                                    Visible="false" HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnDHInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug health interaction"
                                            CommandName="DHInteractionVIDAL" CausesValidation="false" BackColor="#82AB76"
                                            Text="&nbsp;" Width="100%" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="DA" HeaderTooltip="Drug Allergy Interaction"
                                    Visible="false" HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnDAInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug allergy interaction"
                                            CommandName="DAInteractionVIDAL" CausesValidation="false" BackColor="#82CAFA"
                                            Text="&nbsp;" Width="100%" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="OrderDate" DefaultInsertValue="" HeaderText="Order Date"
                                    ItemStyle-Width="120px" HeaderStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="StartDate" DefaultInsertValue="" HeaderText="Start Date"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStartDate" runat="server" Text='<%#Eval("StartDate") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="EndDate" DefaultInsertValue="" HeaderText="End Date"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("EndDate") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Dose1" DefaultInsertValue="" HeaderText="">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <%--  </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
                <div class="row">
                    <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow1" runat="server" Skin="Office2007" InitialBehaviors="Maximize" Behaviors="Close,Move" />
                        </Windows>
                    </telerik:RadWindowManager>
                    <asp:Button ID="btnDrug" runat="server" CausesValidation="false" Style="visibility: hidden;"
                        Width="1" OnClick="btnDrugAdministered_Click" Text="" />
                    <asp:HiddenField ID="hdnIndentId" runat="server" />
                    <asp:HiddenField ID="hdnItemId" runat="server" />
                    <asp:HiddenField ID="hdnFrequencyId" runat="server" />
                    <%--<asp:HiddenField ID="hdnDateTime" runat="server" />--%>
                    <%--<asp:HiddenField ID="hdnTimeSlot" runat="server" />--%>
                    <%-- <asp:HiddenField ID="hdnPrescription" runat="server" />--%>
                    <%--<asp:HiddenField ID="hdnDoseType" runat="server" />--%>
                    <asp:HiddenField ID="hdnDoseTypeName" runat="server" />
                    <asp:HiddenField ID="hdnDoseName" runat="server" />
                    <asp:HiddenField ID="hdnIsHighAlert" runat="server" />
                    <asp:HiddenField ID="hdnIsInfusion" runat="server" />
                    <asp:HiddenField ID="hdnOrderDateTime" runat="server" />
                    <asp:HiddenField ID="hdnWeight" runat="server" />
                    <asp:HiddenField ID="hdnHeight" runat="server" />
                    <asp:HiddenField ID="hdnCIMSOutput" runat="server" />
                </div>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
