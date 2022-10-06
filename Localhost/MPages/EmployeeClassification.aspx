<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="EmployeeClassification.aspx.cs" Inherits="MPages_EmployeeClassification" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />   
    


    <div class="container-fluid header_main form-group">
        <div class="col-md-3 col-sm-4"><h2>Employee Classification</h2></div>
        <div class="col-md-9 col-sm-8 text-right">
            <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnSave" ToolTip="Save" runat="server" SkinID="Button" OnClick="btnSave_Click"
                            ValidationGroup="save" Text="Save" />
                    </ContentTemplate>
                </asp:UpdatePanel>--%>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="DivMenu" runat="server">
                        <asp:LinkButton ID="lnkEmployee" runat="server" CausesValidation="false" Text="Employee"
                            CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            OnClick="lnkEmployee_OnClick"></asp:LinkButton>
                        <asp:LinkButton ID="lnkEmployeeLookup" runat="server" CausesValidation="false" Text="Employee Look Up"
                            CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            OnClick="lnkEmployeeLookup_OnClick"></asp:LinkButton>
                        <asp:LinkButton ID="lnkProviderProfile" runat="server" CausesValidation="false"
                            Text="Employee Profile" CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);"
                            onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkProviderProfile_OnClick"></asp:LinkButton>
                        <asp:LinkButton  ID="lnkAppointmentTemplate" runat="server" CausesValidation="false" 
                            Text="Appointment Template" CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);"
                            onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkAppointmentTemplate_OnClick"></asp:LinkButton>
                        <asp:LinkButton ID="lnkProviderDetails" runat="server" CausesValidation="false" 
                            CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            Text="Doctor Details" OnClick="lnkProviderDetails_OnClick"></asp:LinkButton>
                        <script language="JavaScript" type="text/javascript">
                            function LinkBtnMouseOver(lnk) {
                                document.getElementById(lnk).style.color = "SteelBlue";
                            }
                            function LinkBtnMouseOut(lnk) {
                                document.getElementById(lnk).style.color = "SteelBlue";
                            }
                        </script>

                                
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>


    <div class="container-fluid">
        <div class="row form-group">
            <div class="col-md-12 col-sm-12 text-center">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" SkinID="label"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    
    



    <div class="container-fluid">
        <div class="row form-group">
            <div class="col-md-4 col-sm-5">
                <div class="row">
                    <div class="col-md-4 col-sm-4 PaddingRightSpacing"><asp:Label ID="lbl1" runat="server" Text="Employee Type"></asp:Label></div>
                    <div class="col-md-8 col-sm-8">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <telerik:RadComboBox ID="ddlEmployeeType" runat="server" Width="100%" Filter="Contains"
                                    MarkFirstMatch="true" OnSelectedIndexChanged="ddlEmployeeType_OnSelectedIndexChanged"
                                    AutoPostBack="true" Skin="Metro">
                                </telerik:RadComboBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-5">
                <div class="row">
                    <div class="col-md-3 col-sm-3"><asp:Label ID="Label1" runat="server" Text="Employee"></asp:Label></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <telerik:RadComboBox ID="ddlEmployee" runat="server" Width="100%" Filter="Contains"
                                    MarkFirstMatch="true" OnSelectedIndexChanged="ddlEmployee_OnSelectedIndexChanged"
                                    AutoPostBack="true" Skin="Metro" >
                                </telerik:RadComboBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div class="col-md-4 col-sm-2"></div>
        </div>

        <div class="row form-group">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <telerik:RadGrid ID="gvDetails" runat="server" AllowMultiRowEdit="false" AllowMultiRowSelection="false"
                        Width="100%" ShowStatusBar="true" EnableLinqExpressions="false" GridLines="None" Skin="Metro"
                        AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnEditCommand="gvDetails_EditCommand"
                        OnUpdateCommand="gvDetails_UpdateCommand" OnCancelCommand="gvDetails_CancelCommand"
                        OnItemDataBound="gvDetails_ItemDataBound" OnPageIndexChanged="gvDetails_PageIndexChanged">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" Scrolling-AllowScroll="false"
                            Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                            <Resizing AllowRowResize="false" EnableRealTimeResize="false" ResizeGridOnColumnResize="false"
                                AllowColumnResize="false" />
                        </ClientSettings>
                        <MasterTableView TableLayout="Fixed" Width="100%">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size:11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <HeaderStyle Font-Bold="true" />
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="SNo" HeaderText="SNo" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <%# Container.ItemIndex+1 %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="EmployeeId" HeaderText="Employee" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("EmployeeName") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="EmployeeType" HeaderText="Employee Type"
                                    HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeeType" runat="server" Text='<%#Eval("EmployeeType") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ClassificationId" HeaderText="Classification"
                                    HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClassificationName" runat="server" Text='<%#Eval("ClassificationName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hdnEmployeeId" runat="server" Value='<%#Eval("EmployeeId") %>' />
                                        <asp:HiddenField ID="hdnClassificationId" runat="server" Value='<%#Eval("ClassificationId") %>' />
                                        <telerik:RadComboBox ID="ddlClassification" runat="server" Width="300px" Filter="Contains"
                                            MarkFirstMatch="true" Skin="Metro" DataSource='<%#BindDropClassification()%>'
                                            DataTextField="Name" DataValueField="ID">
                                        </telerik:RadComboBox>

                                        <asp:HiddenField ID="hdnSpecialisationId" runat="server" Value='<%#Eval("SpecialisationId") %>' />
                                        <telerik:RadComboBox ID="ddlSpecialisation" runat="server" Width="300px" Filter="Contains"
                                            MarkFirstMatch="true" Skin="Metro"  DataSource='<%#BindDropSpecialisation()%>'
                                            DataTextField="SpecialisationName" DataValueField="SpecialisationID">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="SpecialisationId" HeaderText="Specialisation Name"
                                    HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpecialisationName" runat="server" Text='<%#Eval("SpecialisationName") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>


                                <telerik:GridEditCommandColumn UniqueName="editRow" HeaderText="Edit" HeaderStyle-Width="15%" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    
</asp:Content>
