<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="EmployeeWiseSpecialRightTagging.aspx.cs" Inherits="MPages_EmployeeWiseSpecialRightTagging" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">   
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />        
    <asp:UpdatePanel ID="Update1" runat="server">
        <ContentTemplate>            
            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-4 PaddingRightSpacing"><h2>Employee Special Rights Tagging</h2></div>
                <div class="col-md-7 col-sm-6 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" ForeColor="Green" Font-Bold="true" /></div>
                <div class="col-md-2 col-sm-2 text-right">
    	            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_OnClick"/>
	                 <asp:HiddenField ID="hdnisSaveChk" runat="server" Value="0" />
                </div>
            </div>
            <div class="container-fluid">
                <div class="row form-group">                    
                    <div class="col-md-6 col-sm-6">
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-7">Employee Tagging With</div>
                            <div class="col-md-8 col-sm-5 PaddingLeftSpacing">
                            	<telerik:RadComboBox ID="ddlEmployee" runat="server" Width="100%"
                            	AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true" OnSelectedIndexChanged="ddlTaggedFor_OnSelectedIndexChanged"/> 
                            </div>
                        </div>
                        <asp:Panel ID="pnlDepart" runat="server" BorderColor="Black" Height="420px" Width="100%" DefaultButton="btnSerchLeft">
                            <div class="row">
                                <div class="col-md-4 col-sm-7"><asp:Label ID="lblSearch" align="left" style="display:inline-block" runat="server" Text="Search Flag name:"></asp:Label></div>
                                <div class="col-md-8 col-sm-5 PaddingLeftSpacing"><asp:TextBox ID="txtSearch" runat="server" Width="100%"></asp:TextBox></div>
                            </div>
                            <div class="row">
                            	<asp:Button ID="btnSerchLeft" runat="server" OnClick="btnSerchLeft_Click" style="visibility:hidden; width:1px !important; float:left !important; padding:0 !important; margin:0 !important; height:1px;" />
                                <div style="width:100%; overflow-y:scroll; height:440px; float:left;">
                                    <asp:GridView ID="gvSpecialRightSelectionList" SkinID="gridviewOrderNew" runat="server" BorderWidth="0"
                                        AutoGenerateColumns="False" AllowSorting="true" OnSorting="SortRecords" Width="100%" AlternatingRowStyle-BackColor="Beige"
                                        OnPageIndexChanging="gvSpecialRightSelectionList_OnPageIndexChanging">  <%--AllowPaging="true"   PageSize="20"--%>
                                        <EmptyDataTemplate>
                                            <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                        </EmptyDataTemplate>                                            
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="10px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" Checked="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" HorizontalAlign="center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="20%" ItemStyle-VerticalAlign="Top" HeaderText="Flag" SortExpression="Flag">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblColumnName" runat="server" Text='<%# Eval("Flag") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdnColumnId" runat="server" Value='<%#Eval("ID")%>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>                                                
                                            <asp:TemplateField ItemStyle-Width="70%" ItemStyle-VerticalAlign="Top" HeaderText="Description" SortExpression="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>                                                        
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                <div>
                            </div>
                        </asp:Panel>                           
                        <div class="row form-group">
                            <div class="col-md-3 col-sm-3 margin_Top"><div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="chkUnSelect" runat="server" Text="Select All" Checked="false" AutoPostBack="true" OnCheckedChanged="chkUnSelect_OnCheckedChanged" /></div></div>
                            <div class="col-md-5 col-sm-5 text-center margin_Top"><asp:Label ID="lblMainFacilityCount" runat="server" Font-Bold="true"></asp:Label></div>
                            <div class="col-md-4 col-sm-4 text-right margin_Top"><asp:Button ID="btnSend4WO" runat="server" CssClass="btn btn-primary" Text="Select." OnClick="btnSend4WO_OnClick" /> </div>
                        </div>     
                    </div>
                    <div class="col-md-6 col-sm-6"></div>
                </div>
            </div>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-6 col-sm-6"></div>
                    <div class="col-md-6 col-sm-6">
                    	<asp:Panel ID="Panel1" runat="server" Height="440px" Width="100%" DefaultButton="btnSearchRight">                            
                            <div class="row form-group">
                                <div class="col-md-4 col-sm-7"><asp:Label ID="lblFlagNameRight" align="left" runat="server" Text="Search Flag name:"></asp:Label></div>
                                <div class="col-md-8 col-sm-5"><asp:TextBox ID="txtSearchFlagRight" runat="server" align="left" Width="100%"></asp:TextBox></div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-4 col-sm-6">&nbsp;</div>
                                <div class="col-md-8 col-sm-6">&nbsp;</div>
                            </div>                            
                            <div class="row">
                                <asp:Button ID="btnSearchRight" runat="server" OnClick="btnSearchRight_Click" style="visibility:hidden; width:1px !important; float:left !important; padding:0 !important; margin:0 !important; height:1px;"/>
                                  <div style="width:100%; overflow-y:scroll; height:440px; float:left;">
                                    <asp:GridView ID="grvTaggedSpecialRightsList" SkinID="gridviewOrderNew" runat="server" BorderWidth="0"
                                        AutoGenerateColumns="False" OnSorting="gvTSRL_SortRecords" Width="100%" AlternatingRowStyle-BackColor="Beige"
                                        OnPageIndexChanging="grvTaggedSpecialRightsList_OnPageIndexChanging">  <%-- AllowSorting="true" AllowPaging="true"  PageSize="20"--%>
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="95%" ItemStyle-VerticalAlign="Top" HeaderText="Flag" SortExpression="Flag">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSelColumnName" runat="server" Text='<%# Eval("Flag") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdnID" runat="server" Value='<%#Eval("ID")%>' />                                                 
                                                    <asp:HiddenField ID="hdSpecialRightId" runat="server" Value='<%#Eval("SpecialRightId")%>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField> 
                                            <asp:TemplateField ItemStyle-Width="5%">
                                                <ItemTemplate>                                                            
                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" OnClick="lnkDelete_OnClick" ></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                            	</div>
                            </div>
                        </asp:Panel>
                        <div class="row form-group">			     
                            <div class="col-md-6 col-sm-6 text-left"><asp:Label ID="lblSelFacilityCount" runat="server"></asp:Label></div>
                            <div class="col-md-6 col-sm-6 text-right"><asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary" OnClick="btnClear_Click"/> </div>
                        </div>
                    </div>
                </div>
            </div>
              <%-- Pop Up start--%>
              <div id="dvConfirmCancelOptions" runat="server" visible="false" style="width: 400px;
                z-index: 200; border: 1px solid #60AFC3; background-color: #A8D9E6; position: fixed;
                bottom: 40%; height: 85px; left: 38%;">
                <table width="100%" cellspacing="2">
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="Label19" Style="font-size: 12px; font-weight: bold; margin: 0.5em 0 0;
                                padding: 0; width: 100%; float: left;" runat="server" Text="Do you want to delete EmployeeWise SpecialRightTagging ?"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:Button ID="ButtonOk" CssClass="btn btn-primary"  runat="server" Text="Yes" OnClick="btnDeleteOk_OnClick" />
                            &nbsp;
                            <asp:Button ID="ButtonCancel" CssClass="btn btn-default" runat="server" Text="No" OnClick="btnDeleteCancel_OnClick" />
                        </td>
                        <td align="center">
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>      
    </asp:UpdatePanel>
</asp:Content>
