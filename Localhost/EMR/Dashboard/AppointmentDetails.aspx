<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/AppointmentDetails.master" AutoEventWireup="true" CodeFile="AppointmentDetails.aspx.cs" Inherits="EMR_Dashboard_AppointmentDeatils" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl1" TagName="UserDetail" Src="~/Include/Components/MasterComponent/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js" type="text/javascript"></script>--%>
    <script src="../../Include/JS/jquery1.6.4.min.js" type="text/javascript"></script>

    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" type="text/javascript"></script>--%>
    <script src="../../Include/JS/jquery1.11.3.min.js" type="text/javascript"></script>

    <%--<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js" type="text/javascript"></script>--%>
    <script src="../../Include/JS/jquery1.10.4.min.js" type="text/javascript"></script>

    <%--<link type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/themes/smoothness/jquery-ui.css"
        rel="stylesheet" />--%>
    
    <link href="../../Include/css/jquery1.10.4.min.css" rel="stylesheet" type="text/css" /> 

    <script src="../../Include/JS/chosen.jquery.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>





            <div class="container-fluid" style="background: #eeeeee;">
                <div class="col-md-4 col-sm-4 features01" style="margin-left:12em; margin-top:2px;">
                    <div class="patient-TopLeft">
                        <%--  <aspl1:UserDetail ID="pd1" runat="server" />--%>
                        <asp:ImageButton ID="imgPastClinicalNote" runat="server" ImageUrl="~/imagesHTML/PastClinicalNotes.png" CssClass="iconEMRimg" Width="23px" Height="23px" ToolTip="Past Clinical Notes" OnClick="imgPastClinicalNote_OnClick" />
                        <asp:ImageButton ID="imgVitalHistory" runat="server" Visible="false" ImageUrl="~/imagesHTML/Vitals.gif" Width="23px" Height="23px" CssClass="iconEMRimg" OnClick="imgVitalHistory_OnClick" ToolTip="Vital History" />
                        <asp:ImageButton ID="imgDiagnosticHistory" runat="server" ImageUrl="~/Icons/Investigation.jpg" Width="23px" Height="23px" CssClass="iconEMRimg" OnClick="imgDiagnosticHistory_OnClick" ToolTip="Lab Results" Visible="false" />
                        <asp:ImageButton ID="imgXray" runat="server" ImageUrl="~/imagesHTML/Radiology-Results.png" Width="23px" Height="23px" CssClass="iconEMRimg" OnClick="imgXray_OnClick" ToolTip="Radiology Results" Visible="false" />
                        <%--<asp:ImageButton ID="imgImmunization" runat="server" Visible="false" ImageUrl="~/imagesHTML/Patient-Immunization.png" Width="23px" Height="23px" CssClass="iconEMRimg" OnClick="imgImmunization_OnClick" ToolTip="Patient Immunization" />--%>
                        <asp:ImageButton ID="imgGrowthChart" runat="server" ImageUrl="~/imagesHTML/Growth_Charts.gif" Width="23px" Height="23px" CssClass="iconEMRimg" OnClick="imgGrowthChart_OnClick" ToolTip="Growth Chart" Visible="false" />
                        <asp:ImageButton ID="imgAttachment" runat="server" ImageUrl="~/imagesHTML/Attachments.gif" Width="23px" Height="23px" CssClass="iconEMRimg" ToolTip="Attachment" OnClick="imgAttachment_OnClick" Visible="false" />
                        <asp:ImageButton ID="imgAllergyAlert" runat="server" ImageUrl="~/Icons/allergy.gif" Visible="false" CssClass="iconEMRimg" Width="23px" Height="23px" ToolTip="Allergy Alert" OnClick="imgAllergyAlert_Click" />
                        <asp:ImageButton ID="imgMedicalAlert" runat="server" ImageUrl="~/Icons/MedicalAlert.gif" OnClick="imgMedicalAlert_OnClick" CssClass="iconEMRimg" Width="23px" Height="23px" Visible="false" ToolTip="Patient Alert" />

                    </div>
                </div>
            </div>



            <span style="width: 100%; float: left; margin: 0; padding: 0; z-index: 1; text-align: left; position: absolute; top: 0;">
                <asp:Label ID="lblMessage" runat="server" Text="" />

            </span>


            <telerik:RadWindowManager runat="server" ID="RadWindowManager1" EnableViewState="false" Width="550px" Height="450px">
                <Windows>
                    <telerik:RadWindow runat="server" ID="Details" NavigateUrl="DisplayImage.aspx" Behaviors="Close,Move" Modal="true"></telerik:RadWindow>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move"></telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

