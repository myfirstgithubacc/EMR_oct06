<%@ Page Language="C#"  AutoEventWireup="true"
 CodeFile="LISNotes.aspx.cs" Inherits="LIS_Format_LISNotes"%>
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sample Messages</title>
    
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" /> 
    
    
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            color: #CC6699;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
        DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
    
    <div id="dvZone1">

        <div class="container-fluid header_main">
            <div class="col-md-3"></div>
            <div class="col-md-9 text-right">
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnClose" runat="server" CssClass="btn btn-primary" Text="Close" OnClick="btnClose_Click" Font-Size="Smaller" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>



        <div class="container-fluid">
            <div class="row form-group">
                            
                <div class="table-responsive">          
                    <table class="table table-small-font table-bordered table-striped margin_bottom01">
                        <tr align="center">
                            
                            <td colspan="1" align="left">
                                <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:"  Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblPatientName" runat="server" Text=""></asp:Label>
                            </td>
                                        
                            <td colspan="1" align="left">
                                <asp:Label ID="Label1" runat="server" Text="Reg No." Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblRegNo" runat="server" Text=""></asp:Label>
                            </td>
                                        
                            <td colspan="1" align="left">
                                <asp:Label ID="Label2" runat="server" Text="Lab No." Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblLabNo" runat="server" Text=""></asp:Label>
                            </td>
                            <td colspan="1" align="left">
                                <asp:Label ID="lblInfoEncounterNo" runat="server" Text='<%$ Resources:PRegistration, EncounterNo%>' Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblEncounterNo" runat="server" Text="" Font-Bold="true"></asp:Label>
                            </td>
                                        

                        </tr>    
                    </table>
                </div>                                
            </div>
               
        </div>


        <div class="container-fluid">
            <div class="row form-group">
                <div class="col-sm-12 text-center">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="row form-group">

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Office2010Silver" SelectedIndex="0"
                                MultiPageID="RadMultiPage1" Height="100%"                                 
                                ontabclick="RadTabStrip1_TabClick">
                                <Tabs>
                                    <telerik:RadTab Text="Messages" ToolTip="Patient Messages">
                                    </telerik:RadTab>
                                    <telerik:RadTab Text="Preferences" ToolTip="Patient Preferences">
                                    </telerik:RadTab>
                                </Tabs>
                            </telerik:RadTabStrip>
                            <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Height="400px"
                                ScrollBars="Auto">
                                <telerik:RadPageView ID="rpvMessages" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <%--OnItemDataBound="gvMessages_OnItemDataBound"--%>
                                            <telerik:RadGrid ID="gvMessages" runat="server" AllowFilteringByColumn="false" AutoGenerateColumns="false"
                                                AllowMultiRowSelection="false" EnableLinqExpressions="false" ShowFooter="False"
                                                MasterTableView-ShowHeadersWhenNoRecords="true" Height="380px" GridLines="None"
                                                OnItemCommand="gvMessages_OnItemCommand">
                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                                    Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="true" />
                                                </ClientSettings>
                                                <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed" Width="100%">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No Record Found.</div>
                                                    </NoRecordsTemplate>
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="ID" DefaultInsertValue="" HeaderStyle-Width="3%"
                                                            HeaderText="ID" ReadOnly="True" UniqueName="ID" Visible="false" AllowFiltering="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="NoteType" DefaultInsertValue="" HeaderStyle-Width="3%"
                                                            UniqueName="NoteType" Visible="false" AllowFiltering="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn HeaderText="SNo" DataField="SNo" HeaderStyle-Width="5%"
                                                            UniqueName="SNo" AllowFiltering="false">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltrSno" runat="server" Text='<%#Container.ItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="EncodedDate" HeaderText="Date" DefaultInsertValue=""
                                                            HeaderStyle-Width="16%" ReadOnly="True" UniqueName="EncodedDate" AllowFiltering="false"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="EncodedBy" HeaderText="Encoded By" DefaultInsertValue=""
                                                            HeaderStyle-Width="16%" ReadOnly="True" UniqueName="EncodedBy" AllowFiltering="false"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn HeaderStyle-Width="49%" DataField="Notes" HeaderText="Message"
                                                            UniqueName="Notes" FooterStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltrNotes" runat="server" Text='<%#Eval("Notes")%>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkSelect" Text="Select" CommandName="Select" runat="server"
                                                                    ForeColor="DodgerBlue"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </telerik:RadPageView>
                                <telerik:RadPageView ID="rpvPreferences" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <telerik:RadGrid ID="gvPref" runat="server" AllowFilteringByColumn="false" AutoGenerateColumns="false"
                                                AllowMultiRowSelection="false" EnableLinqExpressions="false" ShowFooter="False"
                                                MasterTableView-ShowHeadersWhenNoRecords="true" Height="380px" GridLines="None"
                                                OnItemCommand="gvPref_OnItemCommand">
                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                                    Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="true" />
                                                </ClientSettings>
                                                <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed" Width="100%">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No Record Found.</div>
                                                    </NoRecordsTemplate>
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="ID" DefaultInsertValue="" HeaderStyle-Width="3%"
                                                            HeaderText="ID" ReadOnly="True" UniqueName="ID" Visible="false" AllowFiltering="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="NoteType" DefaultInsertValue="" HeaderStyle-Width="3%"
                                                            UniqueName="NoteType" Visible="false" AllowFiltering="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn HeaderText="SNo" DataField="SNo" HeaderStyle-Width="5%"
                                                            UniqueName="SNo" AllowFiltering="false">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltrSno" runat="server" Text='<%#Container.ItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="EncodedDate" HeaderText="Date" DefaultInsertValue=""
                                                            HeaderStyle-Width="16%" ReadOnly="True" UniqueName="EncodedDate" AllowFiltering="false"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="EncodedBy" HeaderText="Encoded By" DefaultInsertValue=""
                                                            HeaderStyle-Width="16%" ReadOnly="True" UniqueName="EncodedBy" AllowFiltering="false"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn HeaderStyle-Width="49%" DataField="Notes" HeaderText="Preference"
                                                            UniqueName="Notes" FooterStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltrNotes" runat="server" Text='<%#Eval("Notes")%>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkSelect" Text="Select" CommandName="Select" runat="server"
                                                                    ForeColor="DodgerBlue"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                            <asp:Panel ID="pnlNewMessage" runat="server" DefaultButton="btnSaveMessage" >
                                
                                <div class="col-sm-1 col-xs-2"><asp:Label ID="lblInfoMessage" runat="server" Text="Message"></asp:Label></div>
                                <div class="col-sm-9 col-xs-9">
                                    <asp:HiddenField ID="hdnNoteId" runat="server" Value="0" />
                                    <asp:TextBox ID="txtNotes" runat="server" TextMode="SingleLine" Width="100%" Height="30px"
                                        onkeyup="return ValidateMaxLength();" MaxLength="200"></asp:TextBox>
                                </div>
                                <div class="col-sm-2 col-xs-1"><asp:Button ID="btnSaveMessage" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveMessage_OnClick" /></div>
                                <script language="javascript" type="text/javascript">
                                            function ValidateMaxLength() {
                                                var txt = $get('<%=txtNotes.ClientID%>');
                                                if (txt.value.length > 200) {
                                                    alert("Text length should not be more then 200 characters.");
                                                    txt.value = txt.value.substring(0, 200);
                                                    txt.focus();
                                                }
                                            }
                                        </script>
   
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

            </div>

            <div class="row form-group">
                <div class="col-sm-12">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnDecimalPlaces" runat="server" Value="0" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>

    </div>
    </form>
</body>
</html>