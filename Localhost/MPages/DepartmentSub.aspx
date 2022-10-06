<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DepartmentSub.aspx.cs" Inherits="MPages_DepartmentSub" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function Tab_SelectionChanged(sender) {
            var tabIndx = sender.get_activeTabIndex();
            //alert(sender.id);
            if (tabIndx == 0) {
                document.getElementById('trsubdepartment').style.visibility = 'hidden';
                document.getElementById('trdepartment').style.visibility = 'visible';
                document.getElementById('ibtnSave').style.visibility = 'visible';
                document.getElementById('ibtnSaveSub').style.visibility = 'hidden';
            }
            else {
                document.getElementById('trdepartment').style.visibility = 'hidden';
                document.getElementById('trsubdepartment').style.visibility = 'visible';
                document.getElementById('ibtnSaveSub').style.visibility = 'visible';
                document.getElementById('ibtnSave').style.visibility = 'hidden';
            }
        }

        function UpperTab(tabIndx) {
            if (tabIndx == 0) {
                document.getElementById('trsubdepartment').style.visibility = 'hidden';
                document.getElementById('trdepartment').style.visibility = 'visible';
                document.getElementById('ibtnSave').style.visibility = 'visible';
                document.getElementById('ibtnSaveSub').style.visibility = 'hidden';
            }
            else {
                document.getElementById('trdepartment').style.visibility = 'hidden';
                document.getElementById('trsubdepartment').style.visibility = 'visible';
                document.getElementById('ibtnSaveSub').style.visibility = 'visible';
                document.getElementById('ibtnSave').style.visibility = 'hidden';
            }
        }
    </script>
    
    <script language="javascript" type="text/javascript">
        function openRadWindow1(DeptID, SubDeptid) {

       var SubDepartment = document.getElementById('<%= hdnSubdeptid.ClientID %>');
      
       if (SubDepartment.value != '') {
          // var oWnd = radopen("/MPages/CompanyNonDiscServices.aspx?CID=" + ID + "&PT=N", "Radwindow7");
            var oWnd = radopen("Company/DiscountTagging.aspx?DeptId=" + DeptID + "&SubDeptid=" + SubDeptid, "Radwindow3");
          
                oWnd.setSize(900, 600);
                oWnd.Center();                
            }
            else {
                alert('Please Select The Company First');
                return false;
            }

        }
        </script>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div class="container-fluid header_main form-group" id="trdepartment">
                    <div class="col-md-4 col-sm-5"><h2>Department/Sub Department Information</h2></div>
                    <div class="col-md-2">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="Green" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-md-6 col-sm-7 text-right PaddingLeftSpacing">
                        <asp:Button ID="btnNew" runat="server" OnClick="New_OnClick" CssClass="btn btn-primary" ToolTip="New Department"
                            CausesValidation="false" Text="New" />
                        <asp:Button ID="btnBillingGroup" runat="server" OnClick="btnBillingGroup_OnClick"
                           CssClass="btn btn-primary"  ToolTip="Billing Group Details" CausesValidation="false" Text="Billing Group Details" />
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="SaveDepartmentMain_OnClick"
                            ToolTip="Save Department" ValidationGroup="saveupdate" Text="Save Department" />
                        <asp:Button ID="ibtnSaveSub" runat="server" CssClass="btn btn-primary" OnClick="SaveDepartmentSub_OnClick"
                            ToolTip="Save Sub Department" ValidationGroup="save" Text="Save Sub Department" />

                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
                            ShowSummary="False" ValidationGroup="saveupdate" />
                                    <%-- <AJAX:ConfirmButtonExtender ID="CBE1" runat="server" ConfirmOnFormSubmit="false"
                            ConfirmText="Are You Sure That You Want To Save ? " TargetControlID="btnSave">
                        </AJAX:ConfirmButtonExtender>
                        <AJAX:ConfirmButtonExtender ID="CBE2" runat="server" ConfirmOnFormSubmit="false"
                            ConfirmText="Are You Sure That You Want To Update ? " TargetControlID="btnUpdate">
                        </AJAX:ConfirmButtonExtender>--%>
                    </div>
                </div>



                <div class="container-fluid" id="trsubdepartment" style="top: 32px; visibility: hidden;">
                    <div class="row form-group">
                        <div class="col-md-6 col-sm-6">Sub Department Information</div>
                        <div class="col-md-6 col-sm-6 text-right">
                            <%-- <asp:ImageButton ID="ibtnSaveSub" runat="server" ImageUrl="/images/save.gif" OnClick="SaveDepartmentSub_OnClick"
                                    ValidationGroup="save" />--%>
                                <asp:Button ID="ibtnNew" runat="server" CssClass="btn btn-primary" OnClick="SubNew_OnClick"
                                    ToolTip="New Sub Department" CausesValidation="false" Visible="false" Text="New" />
                                <asp:Button ID="ibtnSave" runat="server" CssClass="btn btn-primary" OnClick="SaveDepartmentMain_OnClick"
                                    ToolTip="Save Sub Department" ValidationGroup="save" Visible="false" Text="Save Department" />

                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                                ShowSummary="False" ValidationGroup="save" />
                        </div>
                    </div>
                </div>


                <AJAX:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                    <AJAX:TabPanel ID="TabDepartmentMaster" runat="server" TabIndex="0">
                        <HeaderTemplate>
                            Department
                        </HeaderTemplate>
                        <ContentTemplate>

                            <div class="container-fluid" style="background-color: White;">
                                <div class="row">

                                    <div class="col-md-4 col-sm-4">
                                        
                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" Height="485">
                                            <div class="row form-group">
                                                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
                                                    <div class="col-md-10 col-sm-9 PaddingSpacing"><asp:TextBox ID="txtdepartment" runat="server" MaxLength="85" Width="100%"></asp:TextBox></div>
                                                    <div class="col-md-2 col-sm-3"><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" CausesValidation="true" OnClick="btnSearch_OnClick" /></div>
                                                </asp:Panel>
                                            </div>
                                            <div class="row form-group">
                                                <asp:Panel ID="pnlDepartmentShow" runat="server" BackColor="White" Height="450" ScrollBars="Vertical">
                                                    <asp:GridView ID="gvDepartmentMain" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false"
                                                        DataKeyNames="departmentid" AllowPaging="false" PageSize="20" HorizontalAlign="Left"
                                                        Width="100%" RowStyle-Height="25px" ShowFooter="true" OnRowDataBound="gvDepartmentMain_OnRowDataBound"
                                                        OnSelectedIndexChanged="gvDepartmentMain_OnSelectedIndexChanged">
                                                        <Columns>
                                                            <asp:BoundField DataField="departmentid" HeaderText="subdeptid" Visible="true" />
                                                            <asp:BoundField DataField="departmentname" HeaderText="DEPARTMENTS" Visible="true" />
                                                            <asp:BoundField DataField="Active" HeaderText="Status" Visible="true" />
                                                            <asp:BoundField DataField="FacilityId" HeaderText="FacilityId" Visible="true" />
                                                            <asp:BoundField DataField="FAConsumptionAccountId" HeaderText="FAConsumptionAccountId"
                                                                Visible="true" />
                                                            <asp:BoundField DataField="FARevenueAccountId" HeaderText="FARevenueAccountId" Visible="true" />
                                                            <asp:BoundField DataField="OracleSubInventoryName" HeaderText="OracleSubInventoryName"
                                                                Visible="true" />
                                                            <asp:BoundField DataField="DepartmentSpecification" HeaderText="DepartmentSpecification"
                                                                Visible="true" />
                                                            <asp:BoundField DataField="DepartmentContactNo" HeaderText="DepartmentContactNo"
                                                                Visible="true" />
                                                            <asp:BoundField DataField="DepartmentEmailId" HeaderText="DepartmentEmailId" Visible="true" />
                                                            <asp:BoundField DataField="IsEncounterWithoutConsultation" HeaderText="Encounter Without Consultation"
                                                                Visible="true" />       
                                                             <asp:BoundField DataField="IsIntimationRequired" HeaderText="Is Intimation Required"
                                                                Visible="true" />                                                                           
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </div>

                                                       
                                        </asp:Panel>
                                        <%-- <AJAX:RoundedCornersExtender ID="RCE_pnlDepartmentShow" runat="server" Enabled="True"
                                                TargetControlID="pnlDepartmentShow" Corners="All" Radius="10">
                                            </AJAX:RoundedCornersExtender>--%>
                                           
                                    </div>

                                    <div class="col-md-offset-1 col-md-7 col-sm-offset-1 col-sm-7">
                                        <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="gvDepartmentMain" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        <ContentTemplate> --%>
                                        <asp:Panel ID="pnlDepartmentEntry" runat="server">
                                            <div class="row">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="ltrlDeptName" runat="server" Text="Department Name"></asp:Literal><span style="color: Red">*</span></div>
                                                <div class="col-md-8 col-sm-7">
                                                    <asp:TextBox ID="txtDeptName" runat="server" MaxLength="100" Width="100%"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDeptName"
                                                        ErrorMessage="Department Name Cannot Be Blank" Display="None" SetFocusOnError="true"
                                                        ValidationGroup="saveupdate">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="ltrlShrtName" Visible="false" runat="server" Text="Short Name"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtShrtName" Visible="false" SkinID="textbox" runat="server" MaxLength="5" Width="100%"></asp:TextBox></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="ltrlHeadName" Visible="false" runat="server" Text="Head Name"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtHeadName" Visible="false" SkinID="textbox" runat="server" MaxLength="100" Width="100%"></asp:TextBox></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Label ID="Label38" runat="server" Text="Facility" /></div>
                                                <div class="col-md-8 col-sm-7"><telerik:RadComboBox ID="ddlFacility" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="true" /></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal1" runat="server" Text="Status "></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7">
                                                    <div class="PD-TabRadioNew01 margin_z">
                                                        <%--<asp:CheckBox ID="chkStatus" runat="server" Text="" />--%>
                                                        <asp:RadioButtonList ID="rblStatusDepartment" runat="server" RepeatDirection="Horizontal"
                                                            CellSpacing="3" CellPadding="3">
                                                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="In Active" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5 PaddingRightSpacing"><asp:Literal ID="Literal6" runat="server" Text="Consumption AccountID "></asp:Literal><span style="color: Red; float:left;">*</span></div>
                                                <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtDeptFAConsumptionAccountId" runat="server" MaxLength="250" Width="100%" /></div>
                                            </div>
                                            
                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal8" runat="server" Text="Revenue AccountID "></asp:Literal><span style="color: Red">*</span></div>
                                                <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtDeptFARevenueAccountId" runat="server" MaxLength="250" Width="100%" /></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal9" runat="server" Text="Inventory Code"></asp:Literal><span style="color: Red">*</span></div>
                                                <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtInventoryCode" runat="server" MaxLength="250" Width="100%" /></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal11" runat="server" Text="Center of Execellence"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtCenterExecellence" runat="server" MaxLength="250" Width="100%" /></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5 PaddingRightSpacing"><asp:Literal ID="Literal12" runat="server" Text="Department Contact No"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtContactno" runat="server" SkinID="textbox" MaxLength="250" Width="100%" /></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal13" runat="server" Text="Department Email ID"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7">
                                                    <span style="float:left; margin:0; padding:0; width:80%;"><asp:TextBox ID="txtEmailID" runat="server" MaxLength="250" Width="100%" /></span>
                                                    <span style="float:left; margin:0; padding:0;width:20%; height:25px;">
                                                        <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtEmailID"
                                                        ValidationGroup="save" SetFocusOnError="true" ErrorMessage="Invalid Email Id" Font-Size="8.5pt"
                                                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                    </span>
                                                </div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal16" runat="server" Text="Encounter Without Consultation"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7">
                                                    <div class="PD-TabRadioNew01 margin_z">
                                                        <asp:RadioButtonList ID="rdoIsEncounterWithoutConsultation" runat="server" RepeatDirection="Horizontal"
                                                            CellSpacing="3" CellPadding="3">
                                                            <asp:ListItem Text="Yes" Value="1" />
                                                            <asp:ListItem Text="No" Value="0" Selected="True" />
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                            </div>
                                              <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal17" runat="server" Text="Is Intimation Required"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7">
                                                    <div class="PD-TabRadioNew01 margin_z">
                                                        <asp:CheckBox ID="chkIsIntimationrequired" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal2" Visible="false" runat="server" Text="Encoded By/Date "></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><asp:Literal ID="Literal3" Visible="false" runat="server" Text=""></asp:Literal></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal4" Visible="false" runat="server" Text="Modified By/Date "></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><asp:Literal ID="Literal5" runat="server" Text=""></asp:Literal></div>
                                            </div>

                                            <div class="row form-group">
                                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ShowSummary="False" BackColor="White" />
                                            </div>
                                            
                                        </asp:Panel>
                                        

                                        <%--  <AJAX:RoundedCornersExtender ID="RCE_pnlDepartmentEntry" runat="server" Enabled="True"
                                        TargetControlID="pnlDepartmentEntry" Corners="All" Radius="10">
                                    </AJAX:RoundedCornersExtender>--%>

                                        <%-- </ContentTemplate></asp:UpdatePanel>--%>
                                    </div>
                                </div>
                            </div>

                           
                                
                                <%--<tr>
                            <td align="left" class="CollapsablePanelTop">
                                Department Info...
                            </td>
                        </tr>--%>
                                
                            
                        </ContentTemplate>
                    </AJAX:TabPanel>



                    <AJAX:TabPanel ID="TabSubDepartment" runat="server" TabIndex="1">
                        <HeaderTemplate>
                            Sub Department
                        </HeaderTemplate>
                        <ContentTemplate>


                            <div class="container-fluid" style="background-color: #FFFFFF">
                                <%--   <asp:UpdatePanel ID="UPD1" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvDepartmentSub" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlMainDept" />
                                    </Triggers>
                                    <ContentTemplate> --%>


                                <div class="row">
                                    <div class="col-md-3 col-sm-4">
                                        <asp:Panel ID="pnlDepartmentSubShow" runat="server">
                                            <asp:Panel ID="pnlDepartmentSubShow1" runat="server">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4"><asp:Literal ID="ltrlMainDept" runat="server" Text="Department"></asp:Literal></div>
                                                    <div class="col-md-8 col-sm-8">
                                                        <asp:DropDownList ID="ddlMainDept" runat="server" Width="100%"
                                                            OnSelectedIndexChanged="ddlMainDept_OnSelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlDepartmentSubShow2" runat="server" Height="450px">
                                                <div class="row form-group">
                                                    <asp:GridView ID="gvDepartmentSub" SkinID="gridviewOrderNew" OnSelectedIndexChanged="grd_SelectedIndexChanged"
                                                        OnRowDataBound="gvDepartmentSub_OnRowDataBound" runat="server" AutoGenerateColumns="false"
                                                        DataKeyNames="subdeptid" AllowPaging="false" PageSize="20" HorizontalAlign="Left"
                                                        HeaderStyle-Height="25px" Width="100%" RowStyle-Height="20px" ShowFooter="false">
                                                        <%-- <SelectedRowStyle BackColor="#BEDBFF" />--%>
                                                        <Columns>
                                                            <asp:BoundField DataField="subdeptid" HeaderText="subdeptid" Visible="true" />
                                                            <asp:BoundField DataField="subname" HeaderText="&nbsp;&nbsp;SUB DEPARTMENTS" Visible="true" />
                                                            <asp:BoundField DataField="Type" HeaderText="&nbsp;&nbsp;Type" Visible="true" />
                                                            <asp:BoundField DataField="Active" HeaderText="&nbsp;&nbsp;Status" Visible="true" />
                                                            <asp:BoundField DataField="FAAccountId" HeaderText="FAAccountId" Visible="true" />
                                                            <asp:BoundField DataField="EmergencyChargesForOPD" HeaderText="EmergencyChargesForOPD"
                                                                Visible="true" />
                                                            <asp:BoundField DataField="EmergencyChargesForIPD" HeaderText="EmergencyChargesForIPD"
                                                                Visible="true" />
                                                            <asp:BoundField DataField="EmergencyChargesFromTime" HeaderText="EmergencyChargesFromTime"
                                                                Visible="true" />
                                                            <asp:BoundField DataField="EmergencyChargesToTime" HeaderText="EmergencyChargesToTime"
                                                                Visible="true" />
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnTaggingId" runat="server" Value='<%#Eval("TaggingId")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </asp:Panel>
                                        </asp:Panel>
                                         <%-- <AJAX:RoundedCornersExtender ID="RCE_pnlDepartmentSubShow" runat="server" Enabled="True"
                                        TargetControlID="pnlDepartmentSubShow" Corners="All" Radius="10">
                                    </AJAX:RoundedCornersExtender>--%>
                                    </div>

                                    <div class="col-md-offset-3 col-md-6 col-sm-8">
                                        <asp:Panel ID="pnlDepartmentSubEntry" runat="server" BackColor="White" Height="440px">

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="ltrlSubDeptName" runat="server" Text="Sub Department Name "></asp:Literal><span style="color: Red">*</span></div>
                                                <div class="col-md-8 col-sm-7">
                                                    <asp:TextBox ID="txtSubDeptName" Width="100%" SkinID="textbox" runat="server" MaxLength="50"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFV1" runat="server" ControlToValidate="txtSubDeptName"
                                                        Display="None" ErrorMessage="Sub Department Name Cannot Be Blank" SetFocusOnError="true"
                                                        ValidationGroup="save">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="ltrlDepartmentType" runat="server" Text="Department Type"></asp:Literal><span style="color: Red">*</span></div>
                                                <div class="col-md-8 col-sm-7">
                                                    <asp:DropDownList ID="ddlDeptType" runat="server" DataTextField="Name" Width="100%" DataValueField="DepartmentType" DataSourceID="sqlddlDeptType"></asp:DropDownList>
                                                    <asp:SqlDataSource ID="sqlddlDeptType" runat="server" ConnectionString="<%$ connectionStrings:akl %>"
                                                        SelectCommand="select ' [ Select ] ' as Name ,'' as DepartmentType union Select Name,DepartmentType from departmenttype">
                                                    </asp:SqlDataSource>
                                                </div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="ltrlStatus" runat="server" Text="Status"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7">
                                                    <div class="PD-TabRadioNew01 margin_z">
                                                        <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" CellSpacing="0" CellPadding="3">
                                                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="In Active" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal7" runat="server" Text="FA AccountID "></asp:Literal><span style="color: Red">*</span></div>
                                                <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtDeptSubFAAccountId" runat="server" MaxLength="250" Width="100%" /></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal10" runat="server" Text="Billing Group"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><telerik:RadComboBox ID="ddlReportTagging" runat="server" Width="100%"></telerik:RadComboBox></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"></div>
                                                <div class="col-md-8 col-sm-7 PaddingRightSpacing"><div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="Chkopemergency" runat="server" Text="Emergency Charges Applicable For OPD" TextAlign="Right" /></div></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"></div>
                                                <div class="col-md-8 col-sm-7 PaddingRightSpacing"><div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="Chkipemergency" runat="server" Text="Emergency Charges Applicable For IPD" TextAlign="Right" /></div></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal14" Visible="true" runat="server" Text="Emergency From Time"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><telerik:RadTimePicker ID="rademergencyfrom" Visible="true" runat="server"></telerik:RadTimePicker></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="Literal15" Visible="true" runat="server" Text="Emergency To Time"></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><telerik:RadTimePicker ID="rademergencyto" Visible="true" runat="server"></telerik:RadTimePicker></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="ltrlEncodedBy" Visible="false" runat="server" Text="Encoded By/Date "></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><asp:Literal ID="ltrlEncodedByDisplay" Visible="false" runat="server" Text=""></asp:Literal></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-4 col-sm-5"><asp:Literal ID="ltrlUpdateBy" Visible="false" runat="server" Text="Modified By/Date "></asp:Literal></div>
                                                <div class="col-md-8 col-sm-7"><asp:Literal ID="ltrlUpdateByDisplay" Visible="false" runat="server" Text=""></asp:Literal></div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-md-12 col-sm-12">
                                                    <asp:ValidationSummary ID="VS1" runat="server" DisplayMode="List" ShowMessageBox="true" ShowSummary="False" />
                                                </div>
                                            </div>

                                              <div class="row form-group">
                                                <div class="col-md-12 col-sm-12">
                                                   <asp:Button ID="btnDistagging" runat="server" CssClass="btn btn-primary" Text="Discount Tagging" OnClick="btnDistagging_Click" />
                                                    <asp:HiddenField ID="hdnSubdeptid" runat="server" />
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <%--<AJAX:RoundedCornersExtender ID="RCE_pnlDepartmentSubEntry" runat="server" Enabled="True"
                                                TargetControlID="pnlDepartmentSubEntry" Corners="All" Radius="10">
                                            </AJAX:RoundedCornersExtender>--%>
                                        <%-- </ContentTemplate>
                                            </asp:UpdatePanel>--%>

                                        <div class="row form-group">
                                            <div class="col-md-12 col-sm-12">
                                                <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                                                    <Windows>
                                                        <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move" />
                                                    </Windows>
                                                </telerik:RadWindowManager>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </ContentTemplate>
                    </AJAX:TabPanel>
                </AJAX:TabContainer>

            </ContentTemplate>
        </asp:UpdatePanel>
  
</asp:Content>