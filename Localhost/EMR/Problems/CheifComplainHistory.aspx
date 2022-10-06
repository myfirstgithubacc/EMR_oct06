<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheifComplainHistory.aspx.cs" Inherits="EMR_Problems_CheifComplainHistory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Chief Complaint History </title>
    
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />     
    <link href="../../Include/css/mainStyle.css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" />
    <link href="../../Include/css/Appointment.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/TreatmentTemplat.css" rel="stylesheet" type="text/css" />
 <%--   <script type="text/javascript" language="javascript">
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
    
    </script>--%>

    <style type="text/css">
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
        .clsGridheader
        {
            border: 1px solid #9DC9F1;
        }
        .clsexta
        {
        }
        .clsGridheader th, .clsGridRowfooter td
        {
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
        .clsGridheader a
        {
            display: block;
            font-size: 12px;
        }
        .clsGridheader a, .clsGridRow a
        {
            text-decoration: none;
        }
        .clsGridheader a:hover, .clsGridRow a:hover
        {
            text-decoration: underline;
        }
        .clsGridRow > td, .clsGridRow-alt > td
        {
            border-bottom: 1px solid #E5ECF9;
            border-right: 1px solid #E5ECF9;
            color: #000000;
            padding: 2px 8px;
        }
        .clsGridRow:hover, .clsGridRow-alt:hover
        {
            background: transparent url(/Images/gridview-gradient.png) repeat-x scroll 0 0;
        }
        .clsGridRow-alt
        {
            background-color: #F5F5F5;
        }
        .clsGridRow-selected
        {
            background-color: #FAFAD2;
        }
        .clsGridRow-edit td
        {
            background-color: #E5ECF9;
        }
    </style>
    <style type="text/css">
        body
        {
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
            <div class="ALPTop EMRLineBorder">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-11 col-xs-11">
                            <span class="EMRfeaturesLeft">
                                <asp:Label ID="lblMessage_2" Visible="false" runat="server" Text=""></asp:Label>
                                <h2>
                                    <%--<asp:Label ID="lblDept" runat="server" Text="Template Name " /><span class="RedText">*</span></h2>
                                <telerik:RadComboBox ID="ddlPlanTemplates" runat="server" MarkFirstMatch="true" Filter="Contains"
                                    EnableLoadOnDemand="true" EmptyMessage="[Select Treatment Plan Name]" Width="180px"
                                    Height="350px" DropDownWidth="400px" EnableVirtualScrolling="true" OnSelectedIndexChanged="ddlPlanTemplates_SelectedIndexChanged"
                                    AutoPostBack="true" />--%>
                               
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </span>
                        </div>
                        <div class="col-md-1 col-xs-1 EMRfeatures">
                         <asp:Button ID="btnClose" runat="server" CssClass="btnSave" Text="Close" OnClientClick="window.close();" />
                        <asp:Button ID="btnSave" ToolTip="Submit" CssClass="btnSave" runat="server" ValidationGroup="Submit"
                                CausesValidation="true" Text="Submit" OnClick="btnSave_OnClick" Visible="false" />
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
                                <h2>
                               &nbsp;&nbsp;
                                    <%--Chief Complaint History Details--%>
                                    </h2>
                                <asp:GridView ID="gvProblemDetails" runat="server" 
                                    HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff"
                                    HeaderStyle-BorderWidth="0" AutoGenerateColumns="False" Width="100%" BackColor="White"
                                    BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                                    <RowStyle BackColor="White" ForeColor="#EEEEEE" />
                                    <Columns>
                               
                                        <asp:TemplateField HeaderText="Encounter No." ItemStyle-Width="10px">
                                            <ItemTemplate>
                                                <asp:Label ID="EncounterId" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                               
                                            </ItemTemplate>
                                            <ItemStyle Width="10px"></ItemStyle>
                                        </asp:TemplateField>
                             
                                        <asp:TemplateField HeaderText="Visit Date" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVisitDate" runat="server" Text='<%#Eval("VisitDate")%>' />
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<% #Eval("EncounterId") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Chief Complaint" ItemStyle-Width="300px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProblemDescription" runat="server" Text='<%#Eval("ProblemDescription")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="300px"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
           
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
