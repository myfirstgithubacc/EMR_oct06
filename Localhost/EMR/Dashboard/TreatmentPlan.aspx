<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TreatmentPlan.aspx.cs" Inherits="EMR_Dashboard_TreatmentPlan" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Treatment Template </title>
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainStyle.css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" />
    <link href="../../Include/css/Appointment.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/TreatmentTemplat.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false)
                { return false; }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "ICCAViewerBtn";
                myButton.value = "Processing...";
            }
            return true;
        }

    </script>
    <script type="text/javascript" language="javascript">
        function returnToParentPage() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.Planofcaretext = document.getElementById("txtWPlanOfCare").value;

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
        .FixedHeader {
            position: absolute;
            font-weight: bold;
        }

        .clsGridheader {
            border: 1px solid #9DC9F1;
        }

        .clsexta {
        }

        .clsGridheader th, .clsGridRowfooter td {
            background: transparent url(/Images/extended-button.png) repeat-x scroll 0 0;
            border-bottom: 2px solid #6593CF;
            border-right: 1px solid #6593CF;
            color: #15428B;
            height: 20px;
            cursor: default;
            font-family: Arial,Helvetica,Tahoma,Sans-Serif,Monospace;
            font-size: 18px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            padding: 1px 2px;
        }

        .clsGridheader a {
            display: block;
            font-size: 12px;
        }

        .clsGridheader a, .clsGridRow a {
            text-decoration: none;
        }

            .clsGridheader a:hover, .clsGridRow a:hover {
                text-decoration: underline;
            }

        .clsGridRow > td, .clsGridRow-alt > td {
            border-bottom: 1px solid #E5ECF9;
            border-right: 1px solid #E5ECF9;
            color: #000000;
            padding: 2px 8px;
        }

        .clsGridRow:hover, .clsGridRow-alt:hover {
            background: transparent url(/Images/gridview-gradient.png) repeat-x scroll 0 0;
        }

        .clsGridRow-alt {
            background-color: #F5F5F5;
        }

        .clsGridRow-selected {
            background-color: #FAFAD2;
        }

        .clsGridRow-edit td {
            background-color: #E5ECF9;
        }
    </style>
    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updMain" runat="server">
            <ContentTemplate>

                <asp:HiddenField ID="hdnItemId" runat="server" />
                <asp:HiddenField ID="hdnItemName" runat="server" />
                <asp:HiddenField ID="hdnAllergyType" runat="server" />

                <asp:Label ID="lblPlanofcaretext" runat="server" />
                <div class="VisitHistoryDiv">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-10 col-xs-10">
                                <span class="EMRfeaturesLeft">
                                    <asp:Label ID="lblMessage_2" Visible="false" runat="server" Text=""></asp:Label>
                                    <h2>
                                        <asp:Label ID="lblDept" runat="server" Text="Template Name " /><span class="RedText">*</span></h2>
                                    <telerik:RadComboBox ID="ddlPlanTemplates" runat="server" MarkFirstMatch="true" Filter="Contains"
                                        EnableLoadOnDemand="true" EmptyMessage="[Select Treatment Plan Name]" Width="180px"
                                        Height="350px" DropDownWidth="400px" EnableVirtualScrolling="true" OnSelectedIndexChanged="ddlPlanTemplates_SelectedIndexChanged"
                                        AutoPostBack="true" />
                                    <asp:HiddenField ID="hdnPlanOfCareRecordId" runat="server" />
                                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                </span>
                            </div>
                            <div class="col-md-2 col-xs-2">
                                <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="PatientLabBtn01" CausesValidation="false" OnClientClick="window.close();" />
                                <asp:Button ID="btnSave" ToolTip="Submit" runat="server" ValidationGroup="Submit" CausesValidation="true" Text="Submit" OnClick="btnSave_OnClick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                            </div>
                        </div>
                    </div>
                </div>
                 <div class="emrPart">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-8">
                                <span class="MPSpacingDiv04b">
                                    <h2>Chief Complaint  <asp:CheckBox ID="chkChiefComplaint"  runat="server" /></h2>
                                   
                                    <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" ReadOnly="true" />
                                </span>
                            </div>
                            <div class="col-md-2">
                                Duration
                            <telerik:RadComboBox ID="ddlDuration" Text="Duration" runat="server" ToolTip="Select duration" Width="100%"  Enabled="false">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="" Value="0" />
                                    <telerik:RadComboBoxItem runat="server" Text="1" Value="1" />
                                    <telerik:RadComboBoxItem runat="server" Text="2" Value="2" />
                                    <telerik:RadComboBoxItem runat="server" Text="3" Value="3" />
                                    <telerik:RadComboBoxItem runat="server" Text="4" Value="4" />
                                    <telerik:RadComboBoxItem runat="server" Text="5" Value="5" />
                                    <telerik:RadComboBoxItem runat="server" Text="6" Value="6" />
                                    <telerik:RadComboBoxItem runat="server" Text="7" Value="7" />
                                    <telerik:RadComboBoxItem runat="server" Text="8" Value="8" />
                                    <telerik:RadComboBoxItem runat="server" Text="9" Value="9" />
                                    <telerik:RadComboBoxItem runat="server" Text="10" Value="10" />
                                </Items>
                            </telerik:RadComboBox>
                            </div>
                            <div class="col-md-2">
                                DurationType
                            <telerik:RadComboBox ID="ddlDurationType" Text="DurationType" runat="server" ToolTip="Select duration type" Width="100%"   Enabled="false">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="" Value="" />
                                    <telerik:RadComboBoxItem runat="server" Text="Hour(s)" Value="H" />
                                    <telerik:RadComboBoxItem runat="server" Text="Day(s)" Value="D" />
                                    <telerik:RadComboBoxItem runat="server" Text="Week(s)" Value="W" />
                                    <telerik:RadComboBoxItem runat="server" Text="Month(s)" Value="M" />
                                    <telerik:RadComboBoxItem runat="server" Text="Year(s)" Value="Y" />
                                </Items>
                            </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="emrPart">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <span class="MPSpacingDiv04b">
                                    <h2>History <asp:CheckBox ID="chkHistory" runat="server" /></h2>

                                    <asp:TextBox ID="txtHistory" runat="server" TextMode="MultiLine" ReadOnly="true" />


                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                 <div class="emrPart">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <span class="MPSpacingDiv04b">
                                    <h2>Examination <asp:CheckBox ID="chkExamination" runat="server" /></h2>
                                    <asp:TextBox ID="txtExamination" runat="server" TextMode="MultiLine" ReadOnly="true" />


                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                  <div class="emrPart">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <span class="MPSpacingDiv01">
                                    <h2>Investigations</h2>
                                    <asp:GridView ID="gvService" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Right"
                                        HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false"
                                        HeaderStyle-BackColor="#C1E5EF" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                        Width="100%" OnRowDataBound="gvService_RowDataBound" OnRowCommand="gvService_RowCommand">
                                        <Columns>


                                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkboxgvService" runat="server" Checked="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                                    <asp:HiddenField ID="hdnServiceiD" runat="server" Value='<% #Eval("ServiceiD") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                  <div class="emrPart">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <span class="MPSpacingDiv04b">
                                    <h2>Diagnosis</h2>
                                    <%-- <asp:TextBox ID="txtDiagnosis" runat="server" TextMode="MultiLine" ReadOnly="true" />--%>
                                    <%--OnRowCommand="gvDiagnosisDetails_RowCommand" OnRowDataBound="gvDiagnosisDetails_RowDataBound" OnRowCreated="gvDiagnosisDetails_RowCreated"--%>
                                    <asp:GridView ID="gvDiagnosisDetails" runat="server" OnRowDataBound="gvDiagnosisDetails_RowDataBound"
                                        HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                        HeaderStyle-Wrap="false" HeaderStyle-BackColor="#C1E5EF" HeaderStyle-BorderColor="#ffffff"
                                        HeaderStyle-BorderWidth="0" AutoGenerateColumns="False" Width="100%" BackColor="White"
                                        BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4"   >
                                          
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkgvDiagnosisDetails" runat="server" Checked="true" />
                                                      <asp:HiddenField ID="hdnICDID" runat="server" Value='<% #Eval("ICDID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ICD Code" ItemStyle-Width="80px" HeaderStyle-Width="80px"
                                                HeaderStyle-CssClass="header-left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Diagnosis" ItemStyle-Wrap="true" HeaderStyle-CssClass="header-left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Primary" HeaderStyle-CssClass="header-left" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Id" HeaderStyle-CssClass="header-left" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                          <%--  <asp:TemplateField ItemStyle-Width="10px" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                        ToolTip="Delete" Width="16px" Height="16px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:GridView>

                                </span>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="emrPart">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <%-- <asp:Panel ID="pnlGrd" runat="server" ScrollBars="Horizontal" Width="100%">--%>
                                <span class="MPSpacingDiv01">
                                    <h2>Medicines</h2>
                                    <asp:GridView ID="gvProblemDetails" runat="server" OnRowDataBound="gvProblemDetails_RowDataBound"
                                        HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                        HeaderStyle-Wrap="false" HeaderStyle-BackColor="#C1E5EF" HeaderStyle-BorderColor="#ffffff"
                                        HeaderStyle-BorderWidth="0" AutoGenerateColumns="False" Width="100%" BackColor="White"
                                        BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                                        <RowStyle BackColor="White" ForeColor="#EEEEEE" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="10px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkboxgvTreatmentPlan" runat="server" Checked="true" OnCheckedChanged="chkboxgvTreatmentPlan_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnItemID" runat="server" Value='<% #Eval("ItemID") %>' />
                                                    <asp:HiddenField ID="hdnAttCIMSItemId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnAttCIMSType" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnAttVIDALItemId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnIndentId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnNotToPharmcy" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnStartDate" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnGenericId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnFormulationId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnRouteId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnStrengthId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnStrengthValue" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnXMLData" runat="server" Value="0" />
                                                    <asp:Label ID="lblPrescriptionDetail" runat="server" Visible="false" />
                                                    <asp:TextBox ID="txtTotalQty" runat="server" Visible="false" />
                                                    <asp:HiddenField ID="hdnDetailsId" runat="server" Value="0" />
                                                    <%--  <asp:HiddenField ID="hdnAttCIMSItemId" runat="server" Value='<% #Eval("CIMSItemId") %>'   />
                                                <asp:HiddenField ID="hdnAttCIMSType" runat="server" Value='<% #Eval("CIMSType") %>' />
                                                <asp:HiddenField ID="hdnAttVIDALItemId" runat="server" Value='<% #Eval("VIDALItemId") %>'   />--%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Brand Name" ItemStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="150px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dose" ItemStyle-Width="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDose" runat="server" Text='<%#Eval("Dose")%>' />
                                                    <asp:Label ID="lblDoseUnit" runat="server" Text='<%#Eval("DoseUnit")%>' />
                                                    <asp:HiddenField ID="hdnDoseUnitID" runat="server" Value='<% #Eval("DoseUnitID") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frequency" ItemStyle-Width="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFrequency" runat="server" Text='<%#Eval("Frequency")%>' />
                                                    <asp:HiddenField ID="hdnFrequencyID" runat="server" Value='<% #Eval("FrequencyID") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duration" ItemStyle-Width="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDays" runat="server" Text='<%#Eval("Days")%>' />
                                                    <asp:Label ID="lblDaysType" runat="server" Text='<%#Eval("DaysType")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Food Relation" ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFoodName" runat="server" Text='<%#Eval("FoodName")%>' />
                                                    <asp:HiddenField ID="hdnFoodNameID" runat="server" Value='<% #Eval("FoodNameID") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
               
               <div class="emrPart">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <span class="MPSpacingDiv04b">
                                    <h2>Instructions <asp:CheckBox ID="chkInstructions" runat="server" /></h2>
                                    <asp:TextBox ID="txtInstructions" runat="server" TextMode="MultiLine" ReadOnly="true" />


                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="emrPart">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <span class="MPSpacingDiv04b">
                                    <h2>Plan of Care<asp:CheckBox ID="chkPlanOfCare" runat="server" /></h2>
                                    <asp:TextBox ID="txtWPlanOfCare" runat="server" TextMode="MultiLine" ReadOnly="true" />


                                </span>
                            </div>
                        </div>
                    </div>
                </div>
               
                <%---------------------------%>
               

                

               

               
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
