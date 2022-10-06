<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMasterWithTopDetails_1.master" AutoEventWireup="true" CodeFile="HealthCheckUpCheckList.aspx.cs" Inherits="ICM_HealthCheckUpCheckList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    
    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />


    <script type="text/javascript">
        function btnPrintOnClick() {
            var popup;
            alert('Y');
            if (hdnAllowPrint.Value == '0') {
                var hdnEncounterId = document.getElementById('<%= hdnEncounterId.ClientID%>').value;
                var hdnDoctorId = document.getElementById('<%= hdnDoctorId.ClientID%>').value;
                var hdnRegistrationId = document.getElementById('<%= hdnRegistrationId.ClientID%>').value;
                var hdnReportId = document.getElementById('<%= hdnReportId.ClientID%>').value;

                popup = window.open("/PrintHealthCheckUp.aspx?page=Ward&EncId=" + hdnEncounterId + "&DoctorId=" + hdnDoctorId + "&RegId=" + hdnRegistrationId + "&ReportId=" + hdnReportId + "&HC=HC", "Popup", "height=550,width=1300,left=10,top=10, status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");
                popup.focus();
                return false
            }
            else {
                alert('Please enter all unchecked templates');
                return false;
            }
        }
    </script>
    
    
    <asp:UpdatePanel ID="UpdatePanel" runat="server"><ContentTemplate>
    
    <div class="WordProcessorDiv">
        <div class="container-fluid">
            <div class="row">
                
                <div class="col-md-8 col-sm-8">
                    <div class="WordProcessorDivText"><h2><asp:Label ID="lblTemplateName" runat="server" Text="Health Check Lists"></asp:Label></h2></div>
                </div>    
                
                <div class="col-md-4 col-sm-4">
                     <asp:Button ID="btnPrintAdvice" runat="server" Text="Print Advice" CssClass="PatientLabBtn01" OnClick="btnPrintAdvice_OnClick"  />    
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="PatientLabBtn01" OnClientClick="window.close();" />    
                    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="PatientLabBtn01" OnClick="btnPrint_OnClick"  />
                </div>
            
            </div>
        </div>
    </div>                                
          
          <div class="VisitHistoryBorder">
        <div class="container-fluid">
            <div class="row">
                 <%--<asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>--%>
                    <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
            
                <%--<div class="HealthBox"></div>--%>
            </div>
        </div>    
    </div>
          
    <div class="VisitHistoryDivText">
        <div class="container-fluid">
            
            <div class="row">        
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <div class="EMR-HealthCheckBox02">
                        <asp:GridView ID="gvCheckListsTemplates" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True" SkinID="gridviewOrder" OnRowDataBound="gvCheckLists_RowDataBound" HeaderStyle-ForeColor="#15428B" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="5%">
                                    <ItemTemplate><asp:CheckBox ID="chkTemplate" runat="server" Enabled="false" /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Template Name" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("TemplateName")%>'></asp:Label>
                                        <asp:HiddenField ID="hdnIsData" runat="server" Value='<%#Eval("IsData")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
    
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <div class="EMR-HealthCheckBox02">
                        <asp:GridView ID="gvCheckListsStaticTemplates" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True" SkinID="gridviewOrder" OnRowDataBound="gvCheckListsStaticTemplates_RowDataBound" HeaderStyle-ForeColor="#15428B" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="5%">
                                <ItemTemplate><asp:CheckBox ID="chkTemplate" runat="server" Enabled="false" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Advicer Template" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("TemplateName")%>'></asp:Label>
                                    <asp:HiddenField ID="hdnIsData" runat="server" Value='<%#Eval("IsData")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </div>
                
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <div class="EMR-HealthCheckBox02">
                        <asp:GridView ID="gvCheckListsSections" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True" SkinID="gridviewOrder" OnRowDataBound="gvCheckListsSections_RowDataBound" HeaderStyle-ForeColor="#15428B" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="5%">
                                <ItemTemplate><asp:CheckBox ID="chkTemplate" runat="server" Enabled="false" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Template Name" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("TemplateName")%>'></asp:Label>
                                    <asp:HiddenField ID="hdnIsData" runat="server" Value='<%#Eval("IsData")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </div>
            </div> 
            
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <asp:HiddenField ID="hdnEncounterId" runat="server" />
                    <asp:HiddenField ID="hdnDoctorId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                    <asp:HiddenField ID="hdnReportId" runat="server" />
                    <asp:HiddenField ID="hdnAllowPrint" runat="server" />
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                            </Windows>
                        </telerik:RadWindowManager>
                </div>
            </div>  
                  
        </div>
    </div>                                            
 
    </ContentTemplate></asp:UpdatePanel>
    
</asp:Content>

