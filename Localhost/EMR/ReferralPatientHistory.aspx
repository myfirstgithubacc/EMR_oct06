<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ReferralPatientHistory.aspx.cs" Inherits="EMR_ReferralPatientHistory"
    Title="Patient Referral History" %>



<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

    
        <asp:UpdatePanel ID="Updatepanel1" runat="server">
            <ContentTemplate>
                
                
                <div class="container-fluid header_main">
	                <div class="col-md-3">
		                <h2><asp:Label ID="lblHead" runat="server" Text="Doctor Referral History"></asp:Label></h2>
	                </div>
	            </div>
                
                <div class="container-fluid subheading_main form-group">
                    <div class="col-md-1"><asp:Label ID="lblPatient" runat="server"  Text="Search On" CssClass="label1" /></div>
                    <div class="col-md-1"><telerik:RadComboBox ID="ddlName" runat="server" AppendDataBoundItems="true" Skin="Outlook"
                                Width="100%">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Reg No" Value="R" />
                                    <telerik:RadComboBoxItem Text="Patient Name" Value="N" />                                 
                                </Items>
                            </telerik:RadComboBox></div>
                    <div class="col-md-2"><asp:TextBox ID="txtSearch" runat="server" MaxLength="50" Width="100%"  /></div>
                    <div class="col-md-4">
                        Date From
                            <telerik:RadDatePicker ID="dtpfromDate"  runat="server" Width="110px" />
                            &nbsp;To&nbsp;
                            <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="110px" />
                    
                    </div>
                    
                    <div class="col-md-4">
                        Status
                            <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true" Skin="Outlook"
                                Width="100px">
                                <Items>
                                  <telerik:RadComboBoxItem Text="All" Value="2" Selected="true" />
                                    <telerik:RadComboBoxItem Text="Open" Value="0" />
                                    <telerik:RadComboBoxItem Text="Close" Value="1" />                                 
                                </Items>
                            </telerik:RadComboBox> &nbsp; &nbsp; &nbsp;
                            Referral
                            <telerik:RadComboBox ID="ddlReferral" runat="server" AppendDataBoundItems="true" Skin="Outlook"
                                Width="100px">
                                <Items>
                                  <telerik:RadComboBoxItem Text="All" Value="0" Selected="true" />
                                  <telerik:RadComboBoxItem Text="Refer From Dr." Value="1" />     
                                    <telerik:RadComboBoxItem Text="Refer To Dr." Value="2" />
                                                                
                                </Items>
                            </telerik:RadComboBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search"  CssClass="btn btn-primary" OnClick ="btnSearch_OnClick" />
                    </div>
                    
                </div>
                
                
                   <div class="container-fluid">
                
                            <asp:Panel ScrollBars="Vertical" runat="server" Height="400px">
                                <telerik:RadGrid ID="gvDetails" Skin="Office2007" runat="server" AutoGenerateColumns="false"
                                    AllowMultiRowSelection="false" ShowFooter="false" GridLines="Both" AllowPaging="true"
                                    PageSize="10"  OnItemDataBound="gvDetails_OnItemDataBound"
                                    OnPageIndexChanged="gvDetails_PageIndexChanged">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="false"
                                        Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                    </ClientSettings>
                                    <MasterTableView DataKeyNames="ReferralId" Width="100%" TableLayout="Fixed">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Source" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Id" UniqueName="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtId" runat="server" Text='<%#Eval("ReferralId")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                             <telerik:GridTemplateColumn HeaderText="Patient Name" UniqueName="PatientName" ItemStyle-Width="150px"   HeaderStyle-Width="150px" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Referral Date" UniqueName="ReferralDate"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px"   HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtReferralDate" runat="server" Text='<%#Eval("ReferralDate")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                               
                                            <telerik:GridTemplateColumn HeaderText="Refer From Dr." UniqueName="FromDoctorName"
                                                ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtFromDoctorName" runat="server" Text='<%#Eval("FromDoctorName")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Refer To Dr." UniqueName="DoctorName" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Conclusion Date" UniqueName="ReferralToDate"
                                                ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtConclusionDate" runat="server" Text='<%#Eval("ReferralConclusionDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="200" HeaderText="Referral Notes" UniqueName="Note"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtNote" runat="server" Text='<%#Eval("Note")%>' ToolTip='<%#Eval("Note")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="200" HeaderText="Reply To Referral"
                                                UniqueName="DoctorRemark" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txDoctorRemark" runat="server" Text='<%#Eval("DoctorRemark")%>' ToolTip='<%#Eval("DoctorRemark")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                             <telerik:GridTemplateColumn HeaderStyle-Width="200" HeaderText="Replied By"
                                            UniqueName="DoctorRemark" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReplyBy" runat="server" Text='<%#Eval("ReplyBy")%>' ToolTip='<%#Eval("ReplyBy")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Type" UniqueName="Urgent" HeaderStyle-Width="60"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtUrgent" runat="server" Text='<%#Eval("Urgent")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Visit Status" UniqueName="ConcludeReferral"
                                                HeaderStyle-Width="80" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txConcludeReferral" runat="server" Text='<%#Eval("ConcludeReferral")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="EncodedId" UniqueName="EncodedId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="txEncodedId" runat="server" Text='<%#Eval("EncodedId")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="CompareId" UniqueName="CompareId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompareId" runat="server" Text='<%#Eval("CompareId")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </asp:Panel>
                            <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move">
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                       
                            <asp:TextBox ID="txtStat" runat="server" Text="" BorderWidth="0" BorderColor="LightGreen"
                                ReadOnly="true" Enabled="false" BackColor="LightGreen" Width="20px"></asp:TextBox>
                            Stat Referral
                        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
   
</asp:Content>
