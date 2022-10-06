<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="EMRTreatmentPlan.aspx.cs" Inherits="EMR_Masters_EMRTreatmentPlan" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdnItemId" runat="server" />
    <asp:HiddenField ID="hdnItemName" runat="server" />
    <asp:HiddenField ID="hdnAllergyType" runat="server" />

    <script type="text/javascript">
        function functionUHIDx(evt) {
            if (evt.charCode > 31 && (evt.charCode < 48 || evt.charCode > 57)) {
                alert("Allow Only Numbers");
                return false;
            }
        }
    </script>

    <script type="text/javascript">

        function btnAddPlaneNameClose() {

            $get('<%=btnEnableControl.ClientID%>').click();
        }

        function btnAddPlaneName_OnClick() {


            var x = screen.width / 2 - 1200 / 2;
            var y = screen.height / 2 - 550 / 2;
            var popup;

            popup = window.open("/EMR/Masters/AddTreatmentPlan.aspx?Source=TreatmentMasters", "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");


            popup.focus();
            $get('<%=btnDisableControl.ClientID%>').click();
            popup.onbeforeunload = function () {
                $get('<%=btnEnableControl.ClientID%>').click();

            }

            return false

        }


    </script>


    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <link href="../../Include/css/mainStyle.css" rel="stylesheet" />
    <link href="../../Include/css/TreatmentTemplat.css" rel="stylesheet" type="text/css" />

    <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>

            <table class="clsheader" width="100%">
                <tr>
                    <td style="width: 250px;">Treatment Template</td>
                    <td style="text-align:left;">
                        <asp:Label ID="lblMessage" runat="server" Text="" SkinID="label" />
                    </td>
                </tr>
            </table>


            <div class="emrPart" style="height:600px!important;overflow-x:scroll;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="EMRTreatmentTop">
                            <div class="col-md-4 col-md-offset-8 TreatmentEMRLeft">
                                <span class="MPSpacingDiv01New">
                                    <h2>
                                        <asp:Label ID="lblDept" runat="server" Text="Plan Name" /><span class="RedText">*</span></h2>
                                    <h3>
                                        <telerik:RadComboBox ID="ddlPlanTemplates" runat="server" MarkFirstMatch="true" Filter="Contains" EnableLoadOnDemand="true" EmptyMessage="[Select Treatment Plan Name]" Height="350px" DropDownWidth="400px" EnableVirtualScrolling="true" OnSelectedIndexChanged="ddlPlanTemplates_SelectedIndexChanged" AutoPostBack="true" />
                                    </h3>
                                    <h2>
                                        <%--<asp:LinkButton ID="btnAddPlaneName" runat="server" SkinID="Button" Text="Add" CssClass="btnSave01"
                                        CausesValidation="false" OnClientClick="btnAddPlaneName_OnClick(); return false;"  />--%>
                                        <asp:LinkButton ID="btnAddPlaneName" runat="server" SkinID="Button" Text="Add" CssClass="btnSave01"
                                            CausesValidation="false" OnClick="btnAddPlaneName_OnClick" />
                                        <%--OnClientClick="btnAddPlaneName_OnClick();"--%>

                                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                                            <Windows>
                                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                            </Windows>
                                        </telerik:RadWindowManager>
                                    </h2>
                                </span>
                            </div>
                        </div>
                    </div>

                    <%-- Chief Complaint Start --%>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="MPSpacingDiv03">
                                <h2>Chief Complaint</h2>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-8">
                            <div class="ServiceEMRText01">
                                <h2>
                                    <asp:Label ID="Label4" runat="server" Text="Enter Text" />
                                </h2>
                                <span class="MPSpacingDiv04">
                                    <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Width="100%" onkeyup="return MaxLenTxt(this,8000);" /></span>
                            </div>
                        </div>
                        <div class="col-md-2">
                            Duration
                            <telerik:RadComboBox ID="ddlDuration" Text="Duration" runat="server" ToolTip="Select duration" Width="100%">
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
                            <telerik:RadComboBox ID="ddlDurationType" Text="DurationType" runat="server" ToolTip="Select duration type" Width="100%">
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
                    <%--</div>--%>
                    <%-- Chief Complaint End --%>

                    <%-- History Start --%>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="MPSpacingDiv03">
                                <h2>History</h2>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="ServiceEMRText01">
                                <h2>
                                    <asp:Label ID="Label6" runat="server" Text="Enter Text" />
                                </h2>
                                <span class="MPSpacingDiv04">
                                    <asp:TextBox ID="txtHistory" runat="server" TextMode="MultiLine" Width="100%" onkeyup="return MaxLenTxt(this,8000);" /></span>
                            </div>
                        </div>
                    </div>
                    <%-- History End --%>

                    <%-- Examination Start --%>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="MPSpacingDiv03">
                                <h2>Examination</h2>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="ServiceEMRText01">
                                <h2>
                                    <asp:Label ID="Label5" runat="server" Text="Enter Text" />
                                </h2>
                                <span class="MPSpacingDiv04">
                                    <asp:TextBox ID="txtExamination" runat="server" TextMode="MultiLine" Width="100%" onkeyup="return MaxLenTxt(this,8000);" /></span>
                            </div>
                        </div>
                    </div>
                    <%-- Examination End --%>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="MPSpacingDiv03">
                                <h2>Investigations</h2>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="ServiceEMRText">
                                <h2>
                                    <asp:Label ID="Label1" runat="server" Text="Service Name" /><span class="RedText">*</span></h2>

                                <telerik:RadComboBox ID="cmbServiceName" runat="server" DataTextField="ServiceName"
                                    DataValueField="ServiceID" EmptyMessage="Search by Text" EnableLoadOnDemand="true"
                                    EnableVirtualScrolling="true" Height="130px" HighlightTemplatedItems="true" OnItemsRequested="cmbServiceName_OnItemsRequested"
                                    OnSelectedIndexChanged="cmbServiceName_OnSelectedIndexChanged" ShowMoreResultsBox="true"
                                    Width="210px">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>Service(s)</td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left"><%# DataBinder.Eval(Container, "Text")%></td>
                                                <td id="Td1" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['CPTCode']")%></td>
                                                <td id="Td2" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['LongDescription']")%></td>
                                                <td id="Td3" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['ServiceType']")%></td>
                                                <td id="Td4" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['DoctorRequired']")%></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadComboBox>

                                <asp:Button ID="btnAddListSevice" runat="server" OnClick="btnAddListSevice_Onclick" Text="Add" CssClass="btnSave01" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <span class="MPSpacingDiv01">
                                <asp:GridView ID="gvService" runat="server" SkinID="gridviewOrderNew" OnRowCommand="gvService_RowCommand"
                                    HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                    AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">

                                    <Columns>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Name" ItemStyle-Font-Size="Small">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkDelete" runat="server" ToolTip="Click here to delete this record"
                                                    CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("ServiceiD")%>'
                                                    ImageUrl="~/Images/DeleteRow.png" Width="13px" Height="13px" />
                                                <asp:HiddenField ID="hdnServiceiD" runat="server" Value='<% #Eval("ServiceiD") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </span>
                        </div>
                    </div>

                    <%-- Diagnosis Start --%>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="MPSpacingDiv03">
                                <h2>Diagnosis</h2>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="ServiceEMRText01">
                                <h2>
                                    <asp:Label ID="Label7" runat="server" Text="Select Diagnosis" />
                                </h2>


                                <telerik:RadComboBox ID="ddlDiagnosiss" runat="server" Height="300px" CssClass="block"
                                    AutoPostBack="true" DropDownWidth="500" EmptyMessage="Search Diagnosis by Text"
                                    DataTextField="DISPLAY_NAME" DataValueField="DiagnosisId" EnableLoadOnDemand="true"
                                    HighlightTemplatedItems="true" ShowMoreResultsBox="true" OnItemsRequested="ddlDiagnosiss_OnItemsRequested"
                                    EnableVirtualScrolling="true">
                                    <HeaderTemplate>
                                        Diagnosis Display Name
                                                                                         
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left">
                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                </td>
                                                <td id="Td1" visible="false" runat="server">
                                                    <%# DataBinder.Eval(Container, "Attributes['ICDID']")%>
                                                </td>
                                                <td id="Td2" visible="false" runat="server">
                                                    <%# DataBinder.Eval(Container, "Attributes['ICDCode']")%>
                                                </td>
                                                <td id="Td3" visible="false" runat="server">
                                                    <%# DataBinder.Eval(Container, "Attributes['ICDDescription']")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                                <asp:GridView ID="gvDiagnosisDetails" runat="server"
                                    SkinID="gridviewOrderNew"
                                    HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                    AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" OnRowCommand="gvDiagnosisDetails_OnRowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' FooterStyle-HorizontalAlign="Right" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15px">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ICD Code" ItemStyle-Width="80px" HeaderStyle-Width="80px"
                                            HeaderStyle-CssClass="header-left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' />
                                                <asp:HiddenField ID="hdnICDID" runat="server" Value='<% #Eval("ICDID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Diagnosis" ItemStyle-Wrap="true" HeaderStyle-CssClass="header-left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="Primary" HeaderStyle-CssClass="header-left" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Id" HeaderStyle-CssClass="header-left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="10px" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                    ToolTip="Delete" Width="13px" Height="13px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:GridView>


                            </div>
                        </div>



                    </div>
                    <div class="row">

                        <div class="col-md-1 text-left">
                            <asp:Literal ID="Literal1" runat="server" Text="Group" Visible="false" />
                        </div>
                        <div class="col-md-4">
                            <telerik:RadComboBox ID="ddlCategory" runat="server" Width="100%"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" Visible="false">
                            </telerik:RadComboBox>
                        </div>
                        <div class="col-md-1"></div>
                        <div class="col-md-1">
                            <asp:Literal ID="Literal2" runat="server" Text="Sub&nbsp;Group" Visible="false" />
                        </div>
                        <div class="col-md-4">
                            <telerik:RadComboBox ID="ddlSubCategory" runat="server" Width="100%"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" Visible="false">
                            </telerik:RadComboBox>
                        </div>
                    </div>

                    <%-- Diagnosis End --%>





                    <div class="row">
                        <div class="col-md-12">
                            <div class="MPSpacingDiv03a">
                                <h2>Medicines</h2>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4 TreatmentEMRLeft">
                            <span class="MPSpacingDiv02New">
                                <h2>
                                    <asp:Label ID="lblItemName" runat="server" Text="Brand Name" /><span class="RedText">*</span></h2>
                                <h3>
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlBrand_Prescriptions" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <telerik:RadComboBox ID="ddlBrand_Prescriptions" runat="server" HighlightTemplatedItems="true" EmptyMessage="[ Search Brands By Typing Minimum 2 Characters ]" AllowCustomText="true" MarkFirstMatch="true" EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true" OnItemsRequested="ddlBrand_Prescriptions_OnItemsRequested" AutoPostBack="true">
                                                <HeaderTemplate>
                                                    <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="width: 80%" align="left">
                                                                <asp:Label ID="Label28" runat="server" Text="Item Name" /></td>
                                                            <td style="width: 0%" align="right" visible="false">
                                                                <asp:HiddenField ID="hdnAttCIMSItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSItemId']")%> />
                                                            </td>
                                                            <td style="width: 0%" align="right" visible="false">
                                                                <asp:HiddenField ID="hdnAttCIMSType" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSType']")%> />
                                                            </td>
                                                            <td style="width: 0%" align="right" visible="false">
                                                                <asp:HiddenField ID="hdnAttVIDALItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['VIDALItemId']")%> />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="width: 80%" align="left"><%# DataBinder.Eval(Container, "Text")%></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>

                                            </telerik:RadComboBox>
                                            </span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </h3>
                            </span>
                        </div>
                        <div class="col-md-4 TreatmentEMRCenter">
                            <span class="MPSpacingDiv02NewCenter">
                                <h2>
                                    <asp:Label ID="lblFrequency" runat="server" Text="Frequency" /><span class="RedText">*</span></h2>
                                <h3>
                                    <telerik:RadComboBox CssClass="FrequencyDrowdrop01" ID="ddlFrequencyId" runat="server" MarkFirstMatch="true" Filter="Contains" EmptyMessage="[ Select ]" Height="250px" />
                                </h3>
                            </span>
                        </div>

                        <div class="col-md-4 TreatmentEMRCenter">
                            <span class="MPSpacingDiv01c">
                                <h2>
                                    <asp:Label ID="lblDays" runat="server" Text="Duration" MaxLength="3" /><span class="RedText">*</span></h2>

                                <asp:TextBox ID="txtDuration" runat="server" CssClass="Textbox" Width="46px" Height="20px" MaxLength="3" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtDuration" ValidChars="0123456789" />
                                <telerik:RadComboBox ID="ddlPeriodType" runat="server" Width="230px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Minute(s)" Value="N" />
                                        <telerik:RadComboBoxItem Text="Hour(s)" Value="H" />
                                        <telerik:RadComboBoxItem Text="Day(s)" Value="D" Selected="true" />
                                        <telerik:RadComboBoxItem Text="Week(s)" Value="W" />
                                        <telerik:RadComboBoxItem Text="Month(s)" Value="M" />
                                        <telerik:RadComboBoxItem Text="Year(s)" Value="Y" />
                                    </Items>
                                </telerik:RadComboBox>
                            </span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4 TreatmentEMRLeft">
                            <span class="MPSpacingDiv02New">
                                <h2>
                                    <asp:Label ID="lblDose" runat="server" Text="Dose" /></h2>
                                <asp:TextBox ID="txtDose" Width="50px" Height="20px" runat="server" CssClass="Textbox" MaxLength="4" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                    FilterType="Custom,Numbers" TargetControlID="txtDose" ValidChars="." />
                                <telerik:RadComboBox ID="ddlUnit" runat="server" MarkFirstMatch="true" Filter="Contains" Width="155px" Height="250px" EmptyMessage="[ Select ]" />
                            </span>
                        </div>
                        <div class="col-md-4 TreatmentEMRCenter">
                            <span class="MPSpacingDiv01b">
                                <h2>
                                    <asp:Label ID="lblFoodRelation" runat="server" Text="Food Relation" /></h2>
                                <telerik:RadComboBox ID="ddlFoodRelation" runat="server" Filter="Contains" EmptyMessage="[ Select ]" Width="240px" ToolTip="Relationship to Food" />
                            </span>
                        </div>
                        <div class="col-md-4 TreatmentEMRCenter">
                            <span class="MPSpacingDiv01c">
                                <h2>
                                    <asp:Label ID="lblRemarks" runat="server" Text="Instructions" /></h2>
                                <asp:TextBox ID="txtInstructions" runat="server" TextMode="MultiLine" />
                                <asp:Button ID="btnAddList" runat="server" OnClick="btnAddList_Onclick" CssClass="btnSave" Text="Add" />
                            </span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <span class="MPSpacingDiv01">
                                <asp:GridView ID="gvTreatmentPlan" runat="server" SkinID="gridviewOrderNew" OnRowCreated="gvTreatmentPlan_RowCreated" OnRowDataBound="gvTreatmentPlan_RowDataBound" OnRowCommand="gvTreatmentPlant_RowCommand"
                                    HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                    AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                                    <Columns>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' FooterStyle-HorizontalAlign="Right" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15px">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Brand Name" ItemStyle-Font-Size="Small" ItemStyle-Width="170px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dose" ItemStyle-Font-Size="Small" ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDose" runat="server" Text='<%#Eval("Dose")%>' />
                                                <asp:Label ID="lblDoseUnit" runat="server" Text='<%#Eval("DoseUnit")%>' />
                                                <asp:HiddenField ID="hdnDoseUnitID" runat="server" Value='<% #Eval("DoseUnitID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Frequency" ItemStyle-Font-Size="Small" ItemStyle-Width="110px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFrequency" runat="server" Text='<%#Eval("Frequency")%>' />
                                                <asp:HiddenField ID="hdnFrequencyID" runat="server" Value='<% #Eval("FrequencyID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Duration" ItemStyle-Font-Size="Small" ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDays" runat="server" Text='<%#Eval("Days")%>' />
                                                <asp:Label ID="lblDaysType" runat="server" Text='<%#Eval("DaysType")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Food Relation" ItemStyle-Font-Size="Small" ItemStyle-Width="110px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFoodName" runat="server" Text='<%#Eval("FoodName")%>' />
                                                <asp:HiddenField ID="hdnFoodNameID" runat="server" Value='<% #Eval("FoodNameID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Instructions" ItemStyle-Font-Size="Small" ItemStyle-Width="110px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="13px" HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                    CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                    ImageUrl="~/Images/DeleteRow.png" Width="13px" Height="13px" />
                                                <asp:Label ID="lblBrand_Prescriptions_ID" Visible="false" runat="server" SkinID="label" Text='<%# Eval("ItemId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </span>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="MPSpacingDiv03">
                                <h2>Instructions</h2>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="ServiceEMRText01">
                                <h2>
                                    <asp:Label ID="Label3" runat="server" Text="Enter Text" />
                                </h2>
                                <span class="MPSpacingDiv04">
                                    <asp:TextBox ID="txtTreatmentPlanInstructions" runat="server" TextMode="MultiLine" Width="100%" onkeyup="return MaxLenTxt(this,8000);" /></span>

                            </div>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-12">
                            <div class="MPSpacingDiv03">
                                <h2>Plan of Care</h2>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="ServiceEMRText01">
                                <h2>
                                    <asp:Label ID="Label2" runat="server" Text="Enter Text" />
                                    <span class="RedText">*</span></h2>
                                <span class="MPSpacingDiv04">
                                    <asp:TextBox ID="txtWPlanOfCare" runat="server" TextMode="MultiLine" Width="100%" onkeyup="return MaxLenTxt(this,8000);" /></span>
                                <span class="MPSpacingDiv04a">
                                    <asp:Button ID="btnPlanofcare" runat="server" Visible="false" OnClick="btnPlanofcare_Onclick" Text="Update" CssClass="btnSave01" /></span>
                            </div>
                        </div>
                    </div>





                    <div class="col-md-2">
                        <span class="MPSpacingDiv04a">
                            <asp:Button ID="btnTreatmentPlanUpdate" runat="server" OnClick="btnPlanofcare_Onclick" Text="Update" CssClass="btnSave01" /></span>
                    </div>

                    <asp:Button ID="btnDisableControl" runat="server" Style="visibility: hidden;"
                        OnClick="btnDisableControl_OnClick" />

                    <asp:HiddenField ID="hdnButtonId" runat="server" />

                    <asp:Button ID="btnEnableControl" runat="server" Style="visibility: hidden;"
                        OnClick="btnEnableControl_OnClick" />
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
