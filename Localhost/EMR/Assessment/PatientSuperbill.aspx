<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="PatientSuperbill.aspx.cs" Inherits="EMR_Assessment_PatientSuperbill"
    Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        
 
        function CloseScreen() {
            window.close();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function OnClientClose(oWnd, args) {
            // $get('<%=btnRefresh.ClientID%>').click();
            window.location.reload();
        }

        function OpenPositionedWindow(oButton, url, windowName) {
            var oWnd = window.radopen(url, windowName);
        }

        function openRadWindow(ID, Pg) {
          
            if (Pg == 'CPT') {
                var oWnd = radopen("PatientDiagnosisCharges.aspx?Mpg=" + '<%=Request.QueryString["Mpg"] %>' + "&ID=" + ID, "RadWindowForNew");
            }
            else if (Pg == 'CNM') {
            var oWnd = radopen("ENMCodes.aspx?Mpg=" + '<%=Request.QueryString["Mpg"] %>' + "&ID=" + ID, "RadWindowForNew");
            }
            else if (Pg == 'MED') {
            var oWnd = radopen("PatientMedicationCharges.aspx?Mpg=" + '<%=Request.QueryString["Mpg"] %>' + "&ID=" + ID, "RadWindowForNew");
            }
            oWnd.setSize(850, 550);
            oWnd.add_close(OnClientClose);
         
            
            oWnd.set_visibleStatusbar(false);
            oWnd.Center();
        }
    </script>

    <table id="tab1" runat="server"  width="100%" class="clsheader">
        <tr>
            <td align="left" style="padding-left: 10px;">
                Patient Superbill
            </td>
            <td align="right" >
                <asp:UpdatePanel ID="upd" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnRefresh" ToolTip="Refresh" runat="server" AccessKey="R" CausesValidation="false"
                            OnClick="btnRefresh_OnClick" Text="Refresh" CssClass="button" Style="visibility: hidden;" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvCPT" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </td>
            <td align="right">
            <b> Order Set</b> &nbsp;
                               <asp:DropDownList ID="ddlOrderSet" runat="server" Width="180px" SkinID="DropDown" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderSubset_SelectedIndexChanged" >
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                
            <asp:Button ID="btnPrint" ToolTip="Print" runat="server" 
                            OnClick="btnPrint_OnClick" Text="Print" SkinID="Button" />
                            
            </td>
        </tr>
    </table>
    <table id="tab2" runat="server" width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="updTodayDiagnosis" runat="server">
                    <ContentTemplate>
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="padding-left: 10px;">
                                    <asp:Label ID="lblTodayDx" runat="server" Text="Diagnosis" Font-Bold="true"></asp:Label>
                                </td>
                                <td >
                                    <asp:Button ID="lnkAddTodayDiagnosis" ToolTip="Add Diagnosis" Text="Add Diagnosis"
                                        runat="server" SkinID="Button" OnClick="lnkAddTodayDiagnosis_OnClick" />
                                </td>
                                <td >
                                    Status &nbsp;
                                    <asp:TextBox ID="txtSuperbillStatus" runat="server" Columns="6" ReadOnly="true"  Width="70px" SkinID="textbox"
                                        Font-Bold="true" ForeColor="Green" Text="Open" />
                                    &nbsp; &nbsp;
                                    <asp:Button ID="btnSuperbillStatus" Text="Finalize" runat="server" SkinID="Button"
                                        OnClick="btnSuperbillStatus_OnClick" />
                                        &nbsp; &nbsp;
                                         <asp:Button ID="btnPullForward" Text="Pull From Prior" runat="server" SkinID="Button"
                                        OnClick="btnPullForward_OnClick" />
                                 
                                 <asp:Button ID="btnOpenOrder" Text="Open Order" runat="server" SkinID="Button"
                                        OnClick="btnOpenOrder_OnClick" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="4">
                                    <telerik:RadGrid ID="gvTodayDiagnosis" runat="server" ClientSettings-EnablePostBackOnRowClick="true"
                                        PageSize="5" Skin="Office2007" Width="100%" AllowSorting="False" AllowMultiRowSelection="False"
                                        AllowPaging="True" ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                        GridLines="none" OnItemDataBound="gvTodayDiagnosis_RowDataBound" OnPageIndexChanged="gvTodayDiagnosis_PageIndexChanging">
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                        <MasterTableView Width="100%">
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="ICDCode" HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblICDCode" SkinID="label" Text='<%#Eval("ICDCode")%>' Width="60px"
                                                            runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="300px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" SkinID="label" Text='<%#Eval("ICDDescription")%>'
                                                            Width="300px" runat="server" />
                                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%#Eval("ID")%>' />
                                                        <asp:HiddenField ID="lblICDId" runat="server" Value='<%#Eval("ICDID")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Condition" ItemStyle-Width="60px" HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblConditions" SkinID="label" Text='<%#Eval("Conditions")%>' Font-Bold="true"
                                                            runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Provider">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("DoctorName")%>' Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Linked" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLinked" runat="server" Text='<%#Eval("Link") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridHyperLinkColumn HeaderStyle-HorizontalAlign="Left" Text="Edit" DataNavigateUrlFields="ID"
                                                    DataNavigateUrlFormatString="Diagnosis.aspx?DiagnosisId={0}" HeaderStyle-Width="40px"
                                                    ItemStyle-Width="40px">
                                                </telerik:GridHyperLinkColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <GroupingSettings ShowUnGroupButton="true" />
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </table>
                          <div id="dvWarning" runat="server" visible="false" style="width: 300px; z-index: 100;
                border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White;
                border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute;
                bottom: 0; height: 75px; left: 350px; top: 200px">
                <asp:UpdatePanel ID="updDelete" runat="server">
                    <ContentTemplate>
                        <table width="99%" border="0" cellpadding="0" cellspacing="2px">
                            <tr>
                                <td colspan="5" align="center">
                                    <asp:Label ID="lblDeleteMessage" Font-Bold="true" runat="server" Text="Please correct the diagnosis codes Or Please check the dates of service."></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            &nbsp;
                            </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Button ID="btnContinue" SkinID="Button" runat="server" Text="Continue" OnClick="btnContinue_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnCancel" SkinID="Button" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
                        
                    </ContentTemplate>
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPullForward" />
                     
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="padding-left: 10px;">
                            <asp:Label ID="lblCpt" runat="server" Text="Charges" Font-Bold="true"></asp:Label>
                        </td>
                        <td valign="top">
                            <asp:Button ID="lnkCPTAdd" SkinID="Button" ToolTip="Add CPT® Codes" Text="Add CPT® Codes"
                                runat="server" OnClientClick="javascript:openRadWindow(0, 'CPT');return false;" />&nbsp;
                            <asp:Button ID="btnCnm" SkinID="Button" ToolTip="Add E&M Codes" Text="Add E&M Codes"
                                runat="server" OnClientClick="javascript:openRadWindow(0, 'CNM');return false;" />
                            &nbsp;
                            <asp:Button ID="btnMedication" SkinID="Button" ToolTip="Add Medication Codes" Text="Add Medication Codes"
                                runat="server" OnClientClick="javascript:openRadWindow(0, 'MED');return false;" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel ID="updCPT" runat="server">
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvCPT" runat="server" ClientSettings-EnablePostBackOnRowClick="true"
                                        PageSize="5" Skin="Office2007" Width="100%" AllowSorting="False" AllowMultiRowSelection="False"
                                        AllowPaging="True" ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                        GridLines="none" OnItemDataBound="gvCPT_RowDataBound" OnPageIndexChanged="gvCPT_PageIndexChanged"
                                        OnItemCommand="gvCPT_ItemCommand">
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                        <MasterTableView Width="100%">
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldName="DepartmentName" FieldAlias=":" SortOrder="None">
                                                        </telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="DepartmentName" SortOrder="None"></telerik:GridGroupByField>
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="CPT®Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCPTCode" runat="server" Text='<%#Eval("CPTCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Description" ItemStyle-Width="300px" HeaderStyle-Width="300px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' ToolTip='<%#Eval("Id") %>'></asp:Label>
                                                        <asp:HiddenField ID="lblID" runat="server" Value='<%#Eval("Id") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                  <telerik:GridTemplateColumn HeaderText="NDCCode">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNDCCode" runat="server" Text='<%#Eval("NDCCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Modifier">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModifier" runat="server" Text='<%#Eval("ModifierCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ICD Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Units">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnits" runat="server" Text='<%#Eval("Units") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Unit Charge">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitAmount" runat="server" Text='<%#Eval("ServiceAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="From">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFromDate" runat="server" Text='<%#Eval("FromDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="To">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblToDate" runat="server" Text='<%#Eval("ToDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Billable">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsBillable" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsBillable"))%>'
                                                            runat="server" Visible="false" />
                                                        <asp:Literal ID="ltrl" runat="server" Text='<%#Eval("yn") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName='<%#Eval("ID") %>'
                                                            CommandArgument='<%#Eval("SubDeptId") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridTemplateColumn  Visible="false" HeaderText="ServiceName" ItemStyle-Width="350px" HeaderStyle-Width="350px">
                                            <ItemTemplate>
                                                
                                                <asp:HiddenField ID="hdnServiceAmount" runat="server" Value='<%#Eval("ServiceAmount") %>' />
                                                <asp:HiddenField ID="hdnServiceDiscountAmount" runat="server" Value='<%#Eval("ServiceDiscountAmount") %>' />
                                                <asp:HiddenField ID="hdnDoctorAmount" runat="server" Value='<%#Eval("DoctorAmount") %>' />
                                                <asp:HiddenField ID="hdnDoctorDiscountAmount" runat="server" Value='<%#Eval("DoctorDiscountAmount") %>' />                                                 
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <GroupingSettings ShowUnGroupButton="true" />
                                    </telerik:RadGrid>
                                    <telerik:RadWindowManager ID="RadWindowManager" runat="server" VisibleStatusbar="false">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" VisibleStatusbar="false" Behaviors="Close,Move">
                                            </telerik:RadWindow>
                                        </Windows>
                                    </telerik:RadWindowManager>  
                                    
                                   
                                                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                                                            <Windows>
                                                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move">
                                                                </telerik:RadWindow>
                                                            </Windows>
                                                        </telerik:RadWindowManager>
                                                                                
                                </ContentTemplate>
                                <Triggers>
                    
                    
                    </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
   
</asp:Content>