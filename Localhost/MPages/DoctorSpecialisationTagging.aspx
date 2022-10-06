<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="DoctorSpecialisationTagging.aspx.cs" Inherits="MPages_DoctorSpecialisationTagging" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-4"><h2><asp:Label ID="Label2" runat="server" Text="Doctor Specialisation Tagging" ToolTip="Doctor Specialisation Tagging"></asp:Label></h2></div>
                <div class="col-md-6 col-sm-5 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" Font-Bold="true" /></div>
                <div class="col-md-3 col-sm-3 text-right">
                    <asp:Button ID="btnNew" runat="server" ToolTip="New Record" CssClass="btn btn-default" Text="New" OnClick="btnNew_OnClick" />
                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data" OnClick="btnSaveData_OnClick" CssClass="btn btn-primary" Text="Save" />
                </div>
            </div>


            <div class="container-fluid">
                <div class="row form-group">
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2"><span class="textName"><asp:Label ID="Label1" runat="server" Text="Doctor Name" ToolTip="Doctor Name"></asp:Label>&nbsp;<span style='color: Red'>*</span></span></div>
                            <div class="col-md-8 col-sm-8">
                                <telerik:RadComboBox ID="ddlEmployee" MarkFirstMatch="true" runat="server" AutoPostBack="true" 
                                    Filter="Contains" AllowCustomText="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged" Width="100%">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2"></div>
                            <div class="col-md-8 col-sm-8"></div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2"></div>
                            <div class="col-md-8 col-sm-8"></div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2"></div>
                            <div class="col-md-8 col-sm-8"></div>
                        </div>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-md-3 col-sm-4 PaddingRightSpacing">
                        <div class="PD-TabRadioNew01 margin_z">
                            <asp:CheckBox ID="chkUnchk" runat="server" Text="All&nbsp;Select&nbsp;/&nbsp;Unselect&nbsp;Specialisation(s)"
                                AutoPostBack="true" OnCheckedChanged="chkUnchk_OnCheckedChanged" />
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 PaddingRightSpacing"><strong>Tagged Specialisation(s)</strong></div>
                </div>


                <div class="row form-group">
                    <div class="col-md-5 col-sm-5">
                        <asp:Panel ID="Panel1" runat="server" Height="400px" Width="100%" ScrollBars="Auto">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvSpecialisationList" SkinID="gridviewOrderNew" runat="server" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="false" Height="99%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSpecialisation" runat="server" Checked='<%#Eval("IsTaggedWithDoctor").ToString().Equals("1")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="List of Specialisation(s)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSpecialisationName" runat="server" Text='<%#Eval("SpecialisationName")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdnSpecialisationId" runat="server" Value='<%#Eval("SpecialisationId")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <asp:Panel ID="Panel2" runat="server" Height="400px" Width="100%" ScrollBars="Auto">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvTagged" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                        AllowFilteringByColumn="false" AllowMultiRowSelection="true" runat="server" Width="100%"
                                        AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                        GridLines="Both" AllowPaging="false" Height="99%">
                                        <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                            </NoRecordsTemplate>
                                            <ItemStyle Wrap="true" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="List of Specialisation(s)" 
                                                    SortExpression="SpecialisationName" UniqueName="SpecialisationName" AllowFiltering="False" 
                                                    AutoPostBackOnFilter="False" ShowFilterIcon="false" FilterControlWidth="100%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSpecialisationName" runat="server" SkinID="label" Text='<%#Eval("SpecialisationName")%>'></asp:Label>
                                                        <asp:HiddenField ID="hdnSpecialisationId" runat="server" Value='<%#Eval("SpecialisationId")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
                </div>


            </div>

            <table cellpadding="2" cellspacing="2">
               

               
                <tr>
                    <td width="400px">
                        <table cellpadding="2" cellspacing="2" width="900px">
                            
                            <tr>
                                <td align="left" width="400">
                                    
                                </td>
                                <td>
                                    
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

