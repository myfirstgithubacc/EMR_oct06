<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddTreatmentPlan.aspx.cs" Inherits="EMR_ClinicalPathway_AddTreatmentPlan" Title="Add Treatment Plan" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">


     <link href="../../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../../Include/Style.css" rel="stylesheet" type="text/css" />

    
    <link href="../../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  
    <link href="../../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <link href="../../../Include/css/font-awesome.min.css" rel="stylesheet" />


   <style type="text/css">
       table#rdoPlanType { width: 100%;}
       table#rdoPlanType label { margin: 2px 0 0 4px;}
       table#rdoIsSurgical { width: 100%;}
        table#rdoIsSurgical label { margin: 2px 0 0 4px;}
      table#gvPlanNameLists_ctl00 th {
    padding: 8px !important; background: #d9edf7; font-weight: bold;}

      div#gvPlanNameLists { border: 0;}
      table#gvPlanNameLists_ctl00 { border-collapse: collapse;}
      table#gvPlanNameLists_ctl00 th, table#gvPlanNameLists_ctl00 td { border: 1px solid #ccc;}

      .rwTable tr.rwTitleRow { display: none;}
      .h3_heading {font-size: 13px; text-transform: uppercase; font-weight: bold; margin-top: 0;}
      
   </style>

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     
      <%--<telerik:RadAjaxManager ID="RadAjaxManager4" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ConfigurationPanel1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ConfigurationPanel1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="fFileUpload">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="fFileUpload" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>  --%> 
        <%--<asp:Panel ID="ConfigurationPanel1" runat="server">--%>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row form-group" style="padding-top: 10px;">
                        <div class="col-md-2">
                            <h3 class="text-primary h3_heading">
                                <asp:Label ID="lblHeaderText" runat="server" Text="Add Treatment Plan"></asp:Label></h3>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblMessage" runat="server" CssClass="relativ alert_new text-success text-center"></asp:Label>
                        </div>
                        <div class="col-md-4 text-right">
                            <asp:Button ID="btnSave" CssClass="btn btn-sm btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClose" CssClass="btn btn-sm btn-primary" runat="server" Text="Close" OnClientClick="window.close();" />
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-sm-6 form-group">
                            <div class="row">
                                <div class="col-sm-3 text-nowrap">
                                    <asp:Label ID="Label2" runat="server" SkinID="label" Text="Plan Name"></asp:Label>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtPlanName" SkinID="textbox" runat="server" MaxLength="80"
                                        Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 form-group">
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:Label ID="Label3" runat="server" Text="Plan Type" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:RadioButtonList ID="rdoPlanType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoPlanType_SelectedIndexChanged">
                                        <asp:ListItem Value="O" Text="OPD" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="I" Text="IPD" ></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>

                                <div class="col-sm-5">
                                    <asp:CheckBox ID="chkIsSurgical" runat="server"  Text="Surgical" Visible="false" AutoPostBack="true" OnCheckedChanged="chkIsSurgical_CheckedChanged" />
                                </div>
                            </div>

                        </div>


                        <div class="col-sm-6 form-group" style="clear: both;">
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Department"></asp:Label>
                                </div>
                                <div class="col-sm-6">
                                    <telerik:RadComboBox ID="ddlDepartment" runat="server" AllowCustomText="true" Filter="StartsWith"
                                        Width="100%" AppendDataBoundItems="true"
                                        OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true" MaxHeight="120px">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 form-group">
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:Label ID="lblProvider" runat="server" Text='<%$ Resources:PRegistration, Doctor%>' />
                                </div>
                                <div class="col-sm-6">
                                    <telerik:RadComboBox ID="ddlProvider" runat="server" Filter="Contains"
                                        Width="100%" AutoPostBack="true" AllowCustomText="true">
                                        <%-- <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="" />
                                </Items>--%>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 form-group">
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:Label ID="Label8" runat="server" Text="Template Name" />
                                </div>
                                <div class="col-sm-6">
                                    <telerik:RadComboBox ID="ddlTemplateName" runat="server" Filter="Contains"
                                        Width="100%" AutoPostBack="true" AllowCustomText="true">
                                        <%-- <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="" />
                                </Items>--%>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 form-group">
                            <div class="row">
                                <div class="col-sm-3">
                                   
                                </div>
                                <div class="col-sm-6">
                                    
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 form-group">
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:Label ID="Label6" runat="server" Text="Reference Doc." />
                                </div>
                                <div class="col-sm-6">
                                    <asp:FileUpload ID="fFileUpload" runat="server" /><asp:Label ID="Label7" runat="server" Text="Please upload only pdf document" Font-Bold="true" Font-Size="11px" />
                                </div>
                            </div>
                        </div>
                        
                        <div class="col-sm-6 form-group">
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:Label ID="Label5" runat="server" Text="Tag Diagnosis" />
                                </div>
                                <div class="col-sm-9">
                                    <telerik:RadComboBox ID="ddlDiagnosis" runat="server" MaxHeight="220px" CssClass=""
                                        EmptyMessage="Search Diagnosis by Text"
                                        DataTextField="DISPLAY_NAME" DataValueField="DiagnosisId" EnableLoadOnDemand="true"
                                        HighlightTemplatedItems="true" ShowMoreResultsBox="true" OnItemsRequested="ddlDiagnosiss_OnItemsRequested"
                                        EnableVirtualScrolling="true" Width="85%">
                                        <HeaderTemplate>
                                            Diagnosis Display Name
                                                                                         
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table width="100%" cellpadding="0" cellspacing="0" cssclass="table table-bordered">
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
                                    <%-- <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" Width="100%" Height="150px">--%>

                                    <asp:Button ID="btnAdd" CssClass="btn btn-sm btn-primary" runat="server" Text="Add" OnClick="btnAdd_Click" />
                                    <%--</asp:Panel>--%>

                                   
                                </div>
                            </div>

                        </div>

                        <div class="col-sm-4 form-group" style="clear: left;">
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:Label ID="Label4" runat="server" Text="Day" />
                                </div>
                                <div class="col-sm-12">

                                    <asp:GridView ID="gvPlanTypeDuration" CssClass="table table-bordered" HeaderStyle-CssClass="bg-info" runat="server" AutoGenerateColumns="false"
                                        ShowHeader="true" OnRowDataBound="gvPlanTypeDuration_RowDataBound" PageSize="20"
                                        PageIndex="0" PagerSettings-Mode="NumericFirstLast" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="150px" HeaderText="Duration Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDurationName" runat="server" SkinID="label" Text='<%#Eval("DurationName")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdnDayId" runat="server" Value='<%#Eval("DayId")%>' />
                                                    <asp:HiddenField ID="hdnCode" runat="server" Value='<%#Eval("Code")%>' />
                                                    <asp:HiddenField ID="hdnPlanId" runat="server" Value='<%#Eval("PlanId")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Days" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDays" runat="server" Width="50px" Style="text-align: right;" Text='<%#Eval("Days")%>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                </div>
                            </div>
                        </div>



                        <div class="col-sm-8 form-group">
                            <div class="row">
                                <div class="col-sm-3">&nbsp;</div>
                                <div class="col-sm-12">
                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="100%" Height="150px">
                                        <asp:GridView ID="gvPlanDiagnosis" CssClass="table table-bordered" HeaderStyle-CssClass="bg-info" runat="server" AutoGenerateColumns="false"
                                            ShowHeader="true" OnRowCommand="gvPlanDiagnosis_RowCommand" PageSize="20"
                                            PageIndex="0" Width="100%" PagerSettings-Mode="NumericFirstLast">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="300px" HeaderText="Diagnosis">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" SkinID="label" Text='<%#Eval("Description")%>'></asp:Label>
                                                        <asp:HiddenField ID="hdnDiagnosisId" runat="server" Value='<%#Eval("DiagnosisId")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                                            CommandName="Del" CommandArgument='<%#Eval("DiagnosisId")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="col-md-12">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="3">
                                <asp:Panel ID="pnlPlanName" runat="server" ScrollBars="Auto" Width="100%" Height="230px">
                                    <telerik:RadGrid ID="gvPlanNameLists" runat="server" AutoGenerateColumns="false" AllowFilteringByColumn="true"
                                        Width="100%" ShowHeader="true" OnItemCommand="gvPlanNameLists_RowCommand" PageSize="50" AllowMultiRowSelection="False"
                                        PageIndex="0" PagerSettings-Mode="NumericFirstLast" EnableLinqExpressions="false" GridLines="None" OnPreRender="gvPlanNameLists_PreRender">
                                        <MasterTableView TableLayout="Fixed" Width="100%" AllowFilteringByColumn="true">
                                            <Columns>
                                                <telerik:GridTemplateColumn ItemStyle-Width="100px" CurrentFilterFunction="Contains" UniqueName="PlanName"
                                                    ShowFilterIcon="false" HeaderText="Plan Name" SortExpression="PlanName" DataField="PlanName" AllowFiltering="true" AutoPostBackOnFilter="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPlanName" runat="server" SkinID="label" Text='<%#Eval("PlanName")%>'></asp:Label>
                                                        <asp:HiddenField ID="hdnPlanId" runat="server" Value='<%#Eval("PlanId")%>' />
                                                        <asp:HiddenField ID="hdnDepartmentId" runat="server" Value='<%#Eval("DepartmentId")%>' />
                                                        <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                                        <asp:HiddenField ID="hdnOPIP" runat="server" Value='<%#Eval("OPIP")%>' />
                                                        <asp:HiddenField ID="hdnIsSurgical" runat="server" Value='<%#Eval("IsSurgical")%>' />
                                                        <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="PlanType" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true"
                                                    ItemStyle-Width="60px" ShowFilterIcon="false" SortExpression="PlanType" DataField="PlanType" HeaderText="Plan Type" AllowFiltering="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPlanType" runat="server" SkinID="label" Text='<%#Eval("PlanType")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="DepartmentName" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true"
                                                    ItemStyle-Width="150px" ShowFilterIcon="false" SortExpression="DepartmentName" DataField="DepartmentName" HeaderText="Department Name" AllowFiltering="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label11" runat="server" SkinID="label" Text='<%#Eval("DepartmentName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="DoctorName" CurrentFilterFunction="Contains"
                                                    ItemStyle-Width="150px" ShowFilterIcon="false" SortExpression="DoctorName" DataField="DoctorName" HeaderText="Doctor Name" AllowFiltering="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDoctorName" runat="server" SkinID="label" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="5%" ShowFilterIcon="false" AllowFiltering="false">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" OnClick="lnkEdit_OnClik"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="5%" ShowFilterIcon="false" AllowFiltering="false">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                                            CommandName="Del" CommandArgument='<%#Eval("PlanId")%>' />

                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>

                                </asp:Panel>
                            </td>
                        </tr>

                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
            </Triggers>
        </asp:UpdatePanel>
         <%--   </asp:Panel>--%>
        </div>
   
    </form>
</body>
</html>
