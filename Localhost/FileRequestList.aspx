<%@ Page Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true"
    CodeFile="FileRequestList.aspx.cs" Inherits="FileRequestList" Title="File Request List" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Include/css/bootstrap.css" rel="stylesheet" />
    <div>
        <asp:UpdatePanel ID="Updatepanel1" runat="server">
            <ContentTemplate>

                <div class="container-fluid">
                    
                    <div class="row">
                        <div style="display: flex; padding: 10px 0;">
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlSearchCriteria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchCriteria_SelectedIndexChanged">
                                <asp:ListItem Value="RN">Registration No</asp:ListItem>
                                <asp:ListItem Value="EN">Encounter No</asp:ListItem>
                                <asp:ListItem Value="PN">Patient Name</asp:ListItem>
                            </asp:DropDownList>


                            <span style="display: inline-block; width: 50%;">
                            <asp:TextBox ID="txtSearch" runat="server" Visible="false" Width="100%"></asp:TextBox>
                            <asp:TextBox ID="txtRegNo" runat="server" Visible="true" Width="100%"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftxtRegNo" runat="server" Enabled="True" FilterType="Custom,Numbers"
                                TargetControlID="txtRegNo" ValidChars="0123456789" />
                                </span>
                        </div>

                        <div class="col-md-3">
                        <span>Request Status</span>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Value="A">All Request</asp:ListItem>
                                <%--"A" for All--%>
                                <asp:ListItem Value="P" Selected="True">Request Pending</asp:ListItem>
                                <%--"P" for Pending--%>
                                <asp:ListItem Value="G">Approved</asp:ListItem>
                                <%--"G" for Granted--%>
                                <asp:ListItem Value="D">Not Approved</asp:ListItem>
                                <%--"D" for Denied--%>
                        </asp:DropDownList>
                        </div>

                        <div class="col-md-4">
                            Date From
                            <telerik:RadDatePicker ID="dtpfromDate" runat="server" Width="110px" />

                            &nbsp;To&nbsp;
                            <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="110px" />
                            </div>

                         <div class="col-md-2">
                             <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_OnClick" CssClass="btn btn-xs btn-primary"
                                Text="Search" />
                            <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" CssClass="btn btn-xs btn-primary"
                                OnClick="btnClearFilter_Click" />
                             </div>

                            </div>
                        <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="asghdd" />


                    </div>

                 



                    <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadGrid ID="gvDetails" Skin="Office2007" runat="server" AutoGenerateColumns="false"
                                AllowMultiRowSelection="false" ShowFooter="false" GridLines="Both" AllowPaging="true"
                                PageSize="15" OnItemDataBound="gvDetails_OnItemDataBound" OnItemCommand="gvDetails_OnItemCommand"
                                OnPageIndexChanged="gvDetails_PageIndexChanged">
                                <HeaderStyle HorizontalAlign="Left" />
                                <PagerStyle Mode="NumericPages" />
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="false"
                                    Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                </ClientSettings>
                                <MasterTableView DataKeyNames="" Width="100%" TableLayout="Fixed">
                                    <NoRecordsTemplate>
                                        <div style="color: Red;">
                                        No Records to Display
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="SNo" HeaderText="S. No." AllowFiltering="false"
                                            ItemStyle-Width="25px" HeaderStyle-Width="25px">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltrSno" runat="server" Text='<%#(Container.DataSetIndex+1)%>'></asp:Literal>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Registration No." ItemStyle-Width="50px"
                                            HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Encounter No." UniqueName="Id" ItemStyle-Width="50px"
                                            HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Patient Name" UniqueName="PatientName" ItemStyle-Width="100px"
                                            HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Remark" UniqueName="Remarks" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Request By" UniqueName="RequestedBy" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestedBy" runat="server" Text='<%#Eval("RequestedBy")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Request Date" UniqueName="EncodedDate" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="75px" HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncodedDate" runat="server" Text='<%#Eval("EncodedDate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Request Status" UniqueName="PermissionStatus"
                                            ItemStyle-Width="50px" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPermissionStatus" runat="server" Text='<%#Eval("PermissionStatus")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Valid Upto" UniqueName="ValidUpTo"
                                            ItemStyle-Width="50px" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblValidUpTo" runat="server" Text='<%#Eval("Validupto")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="" UniqueName="" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfRequest" runat="server" Value='<%#Eval("RequestAcknowledged")%>' />
                                                <asp:LinkButton ID="linkDelete" runat="server" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this request?');"
                                                    CommandName="DeleteRec" CommandArgument='<%#Eval("RequestID")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>

                </div>


              

                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
