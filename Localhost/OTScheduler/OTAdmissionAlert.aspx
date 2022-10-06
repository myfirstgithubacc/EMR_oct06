<%@ Page Language="C#"  MasterPageFile="~/Include/Master/EMRMaster.master"  AutoEventWireup="true" CodeFile="OTAdmissionAlert.aspx.cs" Inherits="OTScheduler_OTAdmissionAlert" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

    
 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #868686 1px !important; border-top:none !important; border-left:none !important; outline:none !important; color:#333; background: 0 -2300px repeat-x #c1e5ef !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {background-color:#fff !important;}
        #ctl00_ContentPlaceHolder1_Panel1 { background-color:#c1e5ef !important;}
        .RadGrid .rgFilterBox {height:20px !important;}
        .RadGrid_Office2007 .rgFilterRow {background: #c1e5ef !important;}
        .RadGrid_Office2007 .rgPager { background: #c1e5ef 0 -7000px repeat-x !important; color: #00156e !important;}
        .RadGrid_Office2007 .rgFooterDiv, .RadGrid_Office2007 .rgFooter { background: #c1e5ef 0 -6500px repeat-x !important;}
    </style> 

    <script type="text/javascript">
        function validateMaxLength() {
            var txt = $get('<%=txtSearchNumeric.ClientID%>');
            if (txt.value > 2147483647) {
                alert("Value should not be more than 2147483647.");
                txt.value = txt.value.substring(0, 9);
                txt.focus();
            }
        }
        </script>




    <div class="container-fluid form-group">
        <div class="row form-group margin_Top01">
            <div class="col-md-12 text-center">
                <asp:Label ID ="lblMessage" runat="server"></asp:Label>
            </div>

        </div>
  
    </div>
    
    <div class="container-fluid">
        <div class="row form-group">
            <asp:UpdatePanel ID ="update3" runat="server" >
                <ContentTemplate>

            <div class="col-md-2 margin_Top">
                <div class="PD-TabRadioNew01 margin_z">
                    <asp:CheckBox ID ="chkSelect" runat="server" Text="Today's  Admission Only" Checked="true" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                </div>
            </div>


              <div class="col-md-2">
                <div class="row">
                    <div class="col-md-4 label2"><asp:Label ID ="lblOT" runat="server" Text="OT" /></div>
                    <div class="col-md-8">
                        <telerik:RadComboBox ID ="ddlOTSearch" runat="server"  AutoPostBack="true" EmptyMessage="Select OT" Width="100%" MarkFirstMatch="true" Filter="Contains" DropDownWidth="250"  OnSelectedIndexChanged="ddlOTSearch_SelectedIndexChanged">
                            
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>

            <div class="col-md-2">
                <div class="row">
                    <div class="col-md-4 label2"><asp:Label ID ="lblWard" runat="server" Text="Ward"></asp:Label></div>
                    <div class="col-md-8">
                        <telerik:RadComboBox ID ="ddlWard" runat="server" AutoPostBack="true" EmptyMessage="Selet Ward" Width="100%" MarkFirstMatch="true" Filter="Contains" DropDownWidth="250"  OnSelectedIndexChanged="ddlWard_SelectedIndexChanged" >
                           
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>
                    </ContentTemplate></asp:UpdatePanel>
                    


            <asp:UpdatePanel ID ="Update2" runat="server">
                <ContentTemplate>
                    
   
                    <div class="col-md-3">
                        <div class="row">
                            <div class="col-md-3 label2"><asp:Label ID ="lblSearch" runat="server" Text="Search"></asp:Label></div>
                            <div class="col-md-9">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ddlSearchOn" runat="server" Width="100%" Height="22px" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOn_SelectedIndexChanged">
                                            <asp:ListItem Text="<%$ Resources:PRegistration, UHID%>" Value="0" />
                                            <asp:ListItem Text="<%$ Resources:PRegistration, EncounterNo%>" Value="1" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6 PaddingLeftSpacing">
                                        <asp:TextBox ID="txtSearch" runat="server" Width="100%" MaxLength="20" Visible="false" />                                            
                                        <asp:TextBox ID="txtSearchNumeric" runat="server" Width="100%" MaxLength="10" Visible="true" onkeyup="return validateMaxLength();" />
                                        <ajax:filteredtextboxextender ID="filteredtextboxextender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtSearchNumeric"  ValidChars="0123456789" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 text-right">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                        <asp:Button ID ="btnReset" runat="server" Text="Reset" CssClass="btn btn-default" OnClick="btnReset_Click" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

           
        </div>

        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server"  Height="100%">
                <ContentTemplate>                   
                    <telerik:RadGrid ID ="gvOTAlertDetail"  runat="server" Skin="Office2007"  BorderWidth="0" GridLines="Both"  Width="100%" Height="450px"
                            PageSize="15" PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false"   AllowCustomPaging="true"
                        AutoGenerateColumns="False" ShowStatusBar="true" ShowFooter="true" EnableLinqExpressions="false"  
                            OnPageIndexChanged="gvOTAlertDetail_PageIndexChanged"  AllowPaging="True">
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True">
                            </Scrolling>
                        </ClientSettings>
                            <ExportSettings>
                            <%--  <Pdf PageTitle="My Page"  PaperSize="A4" />--%>
                        </ExportSettings>
                        <PagerStyle ShowPagerText="true" />
                        <MasterTableView  AllowFilteringByColumn="false" ItemStyle-Wrap="false">
                                    <CommandItemSettings ShowExportToCsvButton="true"/>
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size:11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <Columns>
                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" 
                                    DataField="RegistrationNo" SortExpression="RegistrationNo" HeaderStyle-Width="80px" Aggregate="Count" FooterStyle-Font-Bold ="true"    ItemStyle-Width="80px">
                                         <HeaderTemplate>
 <asp:Label ID="lbl1" runat="server" Text='<%#Session["RegistrationLabelName"]%>'/>
 </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                    DataField="EncounterNo" SortExpression="EncounterNo" HeaderStyle-Width="60px"
                                    ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <%--<asp:LinkButton ID="lnkEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'
                                            OnClick="lnkEncounterNo_OnClick" />--%>
                                        <asp:Label ID="lblEnconterNo" runat="server" Text='<%#Eval("EncounterNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Admission Date" DataField="AdmissionDate"
                                    SortExpression="Name" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("AdmissionDate")%>'></asp:Label>
                                        <asp:HiddenField ID="hdnKinName" runat="server" Value='<%#Eval("AdmissionDate")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Gender/Age" DataField="PatientName"
                                            HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="WardName" HeaderText="WardName"
                                    DataField="WardName" SortExpression="AdmissionDate" HeaderStyle-Width="120px"
                                    ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="BedNo" HeaderText="Bed No" DataField="BedNo"
                                    SortExpression="BedNo" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ConsultantName" HeaderText="Consultant Name"
                                    DataField="ConsultantName" SortExpression="ConsultantName" HeaderStyle-Width="200px"
                                    ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblConsultantName" runat="server" Text='<%#Eval("ConsultantName")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="SurgeryDate" HeaderText="Surgery Date" DataField="SurgeryDate"
                                    SortExpression="DoctorName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSurgeryDate" runat="server" Text='<%#Eval("SurgeryDate")%>'></asp:Label>
                                        <%-- <asp:Label ID="lblSurgeryDate" runat="server" Text='<%#Eval("SurgeryDate")%>'></asp:Label>--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="SurgeryTime" HeaderText="Surgery Time"
                                    DataField="SurgeryTime" SortExpression="SurgeryTime" HeaderStyle-Width="100px"
                                    ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSurgeryTime" runat="server" Text='<%#Eval("SurgeryTime")%>'></asp:Label>
                                        <asp:HiddenField ID="HdnSurgeryTime" runat="server" Value='<%#Eval("SurgeryTime") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="TheatreName" HeaderText="TheatreName" DataField="TheatreName"
                                    SortExpression="TheatreName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTheatreName" runat="server" Text='<%#Eval("TheatreName")%>'></asp:Label>
                                           
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ServiceName" HeaderText="ServiceName" DataField="ServiceName"
                                    SortExpression="ServiceName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                   
                                    <telerik:GridTemplateColumn UniqueName="SurgeonName" HeaderText="SurgeonName" DataField="SurgeonName"
                                    SortExpression="SurgeonName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSurgeonName" runat="server" Text='<%#Eval("SurgeonName")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                </Columns>
                        </MasterTableView>
                        </telerik:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>
      
  
    <asp:HiddenField ID="hdnWardId" runat="server"  Value="0"/>
    <asp:HiddenField ID="hdnTheatreId" runat="server" Value="0" />

</asp:Content>


