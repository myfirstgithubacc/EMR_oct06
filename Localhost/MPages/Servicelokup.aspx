<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="Servicelokup.aspx.cs" Inherits="MPages_Servicelokup" Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />    
    <link href="../Include/css/mainStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/Administration-Item.css" rel="stylesheet" type="text/css" />
    
    
    
    
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    
    

        
        <div class="VisitHistoryDiv">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12"><div class="SearchServiceText"><h2>Search Service</h2></div></div>
                </div>
            </div>    
        </div>
        
        
        
        
        <div class="AuditTrailDiv">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <div class="AuditMessage">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate><asp:Label ID="lblMessage" runat="server" /></ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>    
             </div>
          </div>
        
        
        
        
        
        <asp:Panel ID="pnlSearch" runat="server" BackColor="White" Width="100%">
            
            <div class="ItemServiceDiv">
                <div class="container-fluid">
                                    
                    <div class="row">
                        <div class="col-md-4">
                            <div class="SS-Department">
                                <h2><asp:Label ID="label44" runat="server" Text="Department" /></h2>
                                <h3>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate><telerik:RadComboBox ID="ddlMainDept" runat="server" AllowCustomText="false" CssClass="SS-DepartmentDown" EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlMainDept_OnSelectedIndexChanged" /></ContentTemplate>
                                    </asp:UpdatePanel>
                                </h3>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="SS-Department">
                                <h2><asp:Label ID="label1" runat="server" Text="Sub Department" /></h2>
                                <h3>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <telerik:RadComboBox ID="ddlsubDept" runat="server" CssClass="SS-DepartmentDown" AllowCustomText="false" EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </h3>
                            </div>
                            
                            
                        </div>
                        <div class="col-md-4">
                            <div class="SS-Department">
                                <asp:LinkButton ID="btnback" runat="server" Text="New Service" CssClass="AdminBtn" CausesValidation="false" Visible="false" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);" PostBackUrl="~/MPages/ItemOfService.aspx" />
                                <asp:Button ID="btnExport" runat="server" CausesValidation="true" CssClass="AdminBtn" OnClick="btnExportToExcel_OnClick" Text="Export To Excel" />
                                <script language="JavaScript" type="text/javascript">
                                    function LinkBtnMouseOver(lnk) {
                                        document.getElementById(lnk).style.color = "white";
                                    }
                                    function LinkBtnMouseOut(lnk) {
                                        document.getElementById(lnk).style.color = "white";
                                    }
                                </script>
                            </div>
                            
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <div class="SS-Department">
                                <h2><asp:Label ID="label3" runat="server" Text="CPT&nbsp;Code" /></h2>
                                <h3>
                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                                <asp:TextBox ID="txtcptcode" CssClass="SS-DepartmentInput" runat="server" MaxLength="8" />
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </h3>
                            </div>
                            
                            <div class="SS-Department">
                                <h2>List</h2>   
                                <h3>
                                    <asp:RadioButton ID="OptStandard" runat="server" CssClass="ItemOfServiceActive" AutoPostBack="true" Checked="true" GroupName="Filter" OnCheckedChanged="RadioButtonAge_CheckedChanged" Text="Standard" />
                                    <asp:RadioButton ID="OptCustom" runat="server" CssClass="ItemOfServiceActive" AutoPostBack="true" GroupName="Filter" OnCheckedChanged="RadioButtonAge_CheckedChanged" Text="Customized" />
                                </h3> 
                            </div>
                        </div>
                        
                        <div class="col-md-4">
                            <div class="SS-Department">
                                <h2><asp:Label ID="label2" runat="server" Text="Service/Ref. Code" /></h2>
                                <h3>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch"><asp:TextBox ID="txtservicename" runat="server" CssClass="SS-DepartmentInput" MaxLength="245" /></asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </h3>
                            </div>
                            
                            
                            
                        </div>
                        
                        
                        <div class="col-md-4">
                            <div class="SS-Department">
                                <asp:UpdatePanel ID="up1" runat="server">
                                    <Triggers><asp:AsyncPostBackTrigger ControlID="gvServiceDetail" /></Triggers>
                                    <ContentTemplate><asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="AdminBtn" OnClick="btnSearch_Click" Text="Filter" /></ContentTemplate>
                                </asp:UpdatePanel>
                                
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvServiceDetail" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlMainDept" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlsubDept" />
                                        <asp:AsyncPostBackTrigger ControlID="txtcptcode" />
                                        <asp:AsyncPostBackTrigger ControlID="txtservicename" />
                                    </Triggers>
                                    <ContentTemplate><asp:Button ID="btnClearFilter" runat="server" CausesValidation="true" CssClass="AdminBtn" OnClick="btnClearFilter_Click" Text="Clear Filter" /></ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            
                        
                        </div>
                    </div>
                    
                    
                </div>
            </div>
            
            
            
            <div class="ItemServiceDiv">
                <div class="container-fluid">
                
                    <div class="row">
                    
                        <div class="col-md-12">
                            <asp:Panel ID="pnlemployee" runat="server" SkinID="Panel" Width="100%" Height="430px">
                                <asp:UpdatePanel ID="up2" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                            <Windows><telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" /></Windows>
                                        </telerik:RadWindowManager>
                                        
                                        <asp:GridView ID="gvServiceDetail" runat="server" BorderWidth="0" Width="100%" SkinID="gridview" PageSize="20" AutoGenerateColumns="False" OnSelectedIndexChanged="gverviceDetail_SelectedIndexChanged"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" 
                                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#D4D4D4" BorderStyle="None" 
                                            OnRowDataBound="gverviceDetail_RowDataBound" AllowPaging="True" OnRowCommand="gverviceDetail_OnRowCommand" AllowSorting="True" OnPageIndexChanging="gvServiceDetail_PageIndexChanging1" onrowcreated="gvServiceDetail_RowCreated">
                                            <Columns>
                                                <asp:BoundField HeaderText="Service ID" DataField="Serviceid" />
                                                <asp:BoundField HeaderText="Ref. Service Code" DataField="RefServiceCode" HeaderStyle-Width="70px" />
                                                <asp:BoundField HeaderText="CPT Code" DataField="CPTCode" ItemStyle-Width="5%" />
                                                <asp:BoundField HeaderText="Service Name" DataField="ServiceName" ItemStyle-Width="45%" />
                                                <asp:BoundField HeaderText="Sub CatagoriesId" DataField="SubDeptId" />
                                                <asp:BoundField HeaderText="Catagories Id" DataField="DepartmentId" />
                                                <asp:BoundField HeaderText="Department" DataField="DepartmentName" ItemStyle-Width="25%" />
                                                <asp:BoundField HeaderText="Sub Department" DataField="SubName" ItemStyle-Width="25%" />
                                                <asp:BoundField DataField="SpecialPrecaution" HeaderText="Special Precaution" />
                                                <asp:BoundField DataField="ServiceType" HeaderText="Service Type" />
                                                <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-Width="5%" ItemStyle-CssClass="text-center" ItemStyle-ForeColor="DodgerBlue" Visible="true"><ItemStyle ForeColor="DodgerBlue" /></asp:CommandField>
                                                <asp:TemplateField HeaderText="" Visible="true">
                                                    <ItemTemplate><asp:LinkButton ID="LnkBtnTag" runat="server" CommandName="Template" Text="Template" CommandArgument='<%# Eval("Serviceid") %>' ForeColor="DodgerBlue" Visible="false" /></ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LnkBtnServiceTag" runat="server" CommandName="ServiceTag" Text="Details" CommandArgument='<%# Eval("Serviceid") %>' Visible="false" />
                                                        <asp:Label ID="lblServiceTag" runat="server" Text="Lab Format" Visible="false" />
                                                        <asp:HiddenField ID="hdnStationId" runat="server" Value='<%#Eval("StationId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                            </Columns>
                                            <EmptyDataTemplate><span style="text-decoration: bold; color: Red;">No Rows Found.</span></EmptyDataTemplate>
                                            
                                            
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        
                        </div>
                    
                    </div>                
                
                </div>
           </div>     
                
            
            
        </asp:Panel>
</asp:Content>