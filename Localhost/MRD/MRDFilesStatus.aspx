<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="MRDFilesStatus.aspx.cs" Inherits="MRD_MRDFilesStatus" Title="Untitled Page" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../Include/css/mainNew.css" />
    <link rel="stylesheet" href="../Include/EMRStyle.css" />

    <script type="text/javascript">
        function showMenu(e, menu, requestId, MRDStatusId) {
            $get('<%=hdnGRequestId.ClientID%>').value = $get(requestId).value;           
            var menu = $find(menu);
            menu.show(e);
            if ($get(MRDStatusId).value == 288)
            $get('<%=tblRemarks.ClientID%>').style.display = 'block';
          
        }
  
    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="MRD File Status" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8 text-right">
                         <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnFilter_OnClick" />
                    </div>
                </div>
                <div class="row text-center">
                    <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                </div>
                <div class="row">
                    <div class="col-md-2 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label runat="server" SkinID="label" Text="File Status" />
                             </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddlFileStatus" Width="100%" runat="server" EmptyMessage="[ Select ]"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlFileStatus_OnSelectedIndexChanged" />
                             </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="Label3" runat="server" SkinID="label" Text="Patient Type" />
                             </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddlPatientType" Width="100%" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlFileStatus_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="" Selected="true" />
                                <telerik:RadComboBoxItem Text="OPD" Value="O" />
                                <telerik:RadComboBoxItem Text="IPD" Value="I" />
                            </Items>
                        </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="Label2" runat="server" SkinID="label" Text="Date" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddlTime" runat="server" Width="100%" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="Today" Value="Today" Selected="true" />
                                <telerik:RadComboBoxItem Text="Last Week" Value="LastWeek" />
                                <telerik:RadComboBoxItem Text="Last Two Weeks" Value="LastTwoWeeks" />
                                <telerik:RadComboBoxItem Text="Last One Month" Value="LastOneMonth" />
                                <telerik:RadComboBoxItem Text="Last Three Months" Value="LastThreeMonths" />
                                <telerik:RadComboBoxItem Text="Last Year" Value="LastYear" />
                                <telerik:RadComboBoxItem Text="Date Range" Value="DateRange" />
                            </Items>
                        </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-3 col-xs-12" id="tblDate" runat="server" visible="false">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlTime" />
                            </Triggers>
                            <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label17" runat="server" SkinID="label" ToolTip="From Date" Text="F. Date" />
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8 no-p-l">
                                        <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label18" runat="server" SkinID="label" Text="To&nbsp;Date" />
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8 no-p-l">
                                        <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                                </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="Label4" runat="server" SkinID="label" Text="Search On" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <div class="row">
                                    <div class="col-md-5 col-sm-5 col-xs-5">
                                        <telerik:RadComboBox ID="ddlSearchOn" Width="100%" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlSearchOn_SelectedIndexChanged">
                            <Items>
                                
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, Regno%>' Value="UHID" Selected="true" />
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, EncounterNo%>' Value="Enc" />
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, PatientName%>' Value="PatN" />
                            </Items>
                        </telerik:RadComboBox>
                                    </div>
                                    <div class="col-md7 col-sm-7 col-xs-7">
                                        <asp:TextBox ID ="txtSearchOnForUHID" runat="server" Width="100%" />
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                        FilterType="Custom" TargetControlID="txtSearchOnForUHID" ValidChars="0123456789" />
                        <%-- <asp:TextBox ID ="txtSearchOn" runat="server" Visible="false"/>--%>
                         
                        <asp:TextBox ID ="txtSearchOnEnc" runat="server" Visible="false"/>
                         <asp:TextBox ID ="txtSearchOnPat" runat="server" Visible="false"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                   
                </div>
                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvData" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" Height="400px" Width="100%" ScrollBars="Auto"
                                    BorderWidth="1px" BorderColor="LightBlue">
                                    <asp:GridView ID="gvData" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                        Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="false"
                                        OnRowDataBound="gvData_RowDataBound" OnRowCommand="gvData_OnRowCommand">
                                        <Columns>
                                           <%-- <asp:CommandField HeaderText='Select' ControlStyle-ForeColor="Blue" SelectText="Select"
                                                ShowSelectButton="true" ItemStyle-Width="30px">
                                                <ControlStyle ForeColor="Blue" />
                                            </asp:CommandField>--%>
                                            <asp:TemplateField HeaderText='Request Date' ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileRequestDate" runat="server" Text='<%#Eval("FileRequestDate")%>' />
                                                    <asp:HiddenField ID="hdnRequestId" runat="server" Value='<%#Eval("RequestId")%>' />
                                                    <asp:HiddenField ID="hdnMRDStatusId" runat="server" Value='<%#Eval("MRDStatusId")%>' />
                                                    <asp:HiddenField ID="hdnMRDStatusCode" runat="server" Value='<%#Eval("MRDStatusCode")%>' />
                                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                    <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText='FileNo' ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileNo" runat="server" Width="100%" Text='<%#Eval("FileNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationNo" runat="server" Width="100%" Text='<%#Eval("RegistrationNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterNo" runat="server" Width="100%" Text='<%#Eval("EncounterNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, PatientName%>' ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Width="100%" Text='<%#Eval("PatientName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Age / Gender' ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientAgeGender" runat="server" Width="100%" Text='<%#Eval("PatientAgeGender")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Department' ItemStyle-Width="140px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepartmentName" runat="server" Width="100%" Text='<%#Eval("DepartmentName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Requested For' ItemStyle-Width="110px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestedFor" runat="server" Width="100%" Text='<%#Eval("RequestedFor")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText='Requested By' ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestedBy" runat="server" Width="100%" Text='<%#Eval("RequestedBy")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText='Encoded By' ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncodedBy" runat="server" Width="100%" Text='<%#Eval("EncodedBy") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText='Required Date' ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileRequiredDate" runat="server" Width="100%" Text='<%#Eval("FileRequiredDate")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Remarks' ItemStyle-Width="110px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Width="100%" Text='<%#Eval("Remarks")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                              <asp:TemplateField HeaderText='Rack Number' ItemStyle-Width="110px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRackNumber" runat="server" Width="100%" Text='<%#Eval("RackNumber")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Update Status" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnOPIP" runat="server" Value='<%#Eval("OPIP")%>' />
                                                    <asp:ImageButton ID="btnCategory" runat="server" ImageUrl="~/Images/T.Png" />
                                                    <telerik:RadContextMenu ID="menuStatus" runat="server" EnableRoundedCorners="true"
                                                        EnableShadows="true" Width="100px" OnItemClick="menuStatus_ItemClick" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:HiddenField ID="hdnGRequestId" runat="server" />
                </div>
            </div>
             <table border="0" cellpadding="0" cellspacing="4" runat="server" id="tblRemarks" visible="true" style="margin-top:5px;">
                <tr>
                    <td>
                        <asp:Label runat="server" SkinID="label" Text="File Issue/return remarks" />
                    </td>
                     <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID ="txtFileIssueRemarks" runat="server" Width="822px" AutoComplete="off"/>
                    </td>                
                </tr>
            </table>
           <div runat="server"  id="DivApprovalStatus" visible="false">
                <asp:DropDownList ID="ddlFileApprovalStatus" runat="server">  
                    <asp:ListItem Value="1" Selected="True">Approved</asp:ListItem>
                    <asp:ListItem Value="0">Reject</asp:ListItem>
                </asp:DropDownList>
               <asp:TextBox TextMode="MultiLine" ID="txtApproveRemarks" runat="server"></asp:TextBox>
               <asp:Button ID="btnApproved" Text="Save" runat="server" OnClick="btnApproved_Click"/>
           </div>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
