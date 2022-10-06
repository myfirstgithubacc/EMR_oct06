<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ReferralSlipHistory.aspx.cs"
 Inherits="EMR_ReferralSlipHistory"  %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    
    <link href="../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        th.rgHeader{
            white-space:nowrap;
            padding:7px 9px!important;
        }

    </style>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript">
            function OnClientClose(oWnd, args) {
                $get('<%=btnClientClose.ClientID%>').click();
            }
        </script>
    </telerik:RadCodeBlock>

 

    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>
               
            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3 col-sm-3"><div class="WordProcessorDivText"><h2><asp:Label ID="lblHead" runat="server" Text="Referral History"></asp:Label></h2></div></div>
                        <div class="col-md-6 col-sm-6"><div class="WordProcessorDivText"><h4></h4></div></div>
                        <%--<div class="col-md-3 col-sm-3"><asp:Button ID="btnReferral" runat="server" OnClick="btnReferral_OnClick" Text="Referral Slip" CssClass="PatientBtn01" /></div>--%>
                    </div>                            
                 </div>           
            </div>
            
            
            <div class="VisitHistoryBorderNew">
                <div class="container-fluid">
                    <div class="row">
                        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    </div>                            
                 </div>           
            </div>
            
            <div class="VitalHistory-Div02">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12" style="padding-left:0px;">  
                            <asp:Panel ID="Panel1" ScrollBars="Vertical" runat="server">
                            <telerik:RadGrid ID="gvDetails" Skin="Office2007"  runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false" ShowFooter="false" GridLines="Both" AllowPaging="true" PageSize="5" OnItemCommand="gvDetails_ItemCommand" OnItemDataBound="gvDetails_OnItemDataBound" onpageindexchanged="gvDetails_PageIndexChanged">
                                <HeaderStyle HorizontalAlign="Left"  />
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="false" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                </ClientSettings>
                                <MasterTableView DataKeyNames="ReferralId">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Source" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                            <ItemTemplate><asp:Label ID="lblSource" runat="server"  Text='<%#Eval("Source")%>'></asp:Label>
                                                <asp:HiddenField ID="hdnReferralReplyId" runat="server" Value='<%#Eval("ReferralReplyId")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Id" UniqueName="Id" Visible="false">
                                            <ItemTemplate><asp:Label ID="txtId" runat="server" Text='<%#Eval("ReferralId")%>'></asp:Label></ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Referral Date" UniqueName="ReferralDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                            <ItemTemplate><asp:Label ID="txtReferralDate" runat="server" Text='<%#Eval("ReferralDate")%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Refer From Dr." UniqueName="FromDoctorName" ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate><asp:Label ID="txtFromDoctorName" runat="server" Text='<%#Eval("FromDoctorName")%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Refer To Dr." UniqueName="DoctorName" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                            <ItemTemplate><asp:Label ID="txtDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label></ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Conclusion Date" UniqueName="ReferralToDate" ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate><asp:Label ID="txtConclusionDate" runat="server" Text='<%#Eval("ReferralConclusionDate")%>'></asp:Label></ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="200"  HeaderText="Referral Notes" UniqueName="Note" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate><asp:Label ID="txtNote" runat="server" Text='<%#Eval("Note")%>' ToolTip='<%#Eval("Note")%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="200"  HeaderText="Reply To Referral" UniqueName="DoctorRemark" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate><asp:Label ID="txDoctorRemark" runat="server" Text='<%#Eval("DoctorRemark")%>' ToolTip='<%#Eval("DoctorRemark")%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn HeaderStyle-Width="200" HeaderText="Replied By" UniqueName="DoctorRemark" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate><asp:Label ID="lblReplyBy" runat="server" Text='<%#Eval("ReplyBy")%>' ToolTip='<%#Eval("ReplyBy")%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Type" UniqueName="Urgent" HeaderStyle-Width="60" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate><asp:Label ID="txtUrgent" runat="server" Text='<%#Eval("Urgent")%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Visit Status" UniqueName="ConcludeReferral" HeaderStyle-Width="80" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate><asp:Label ID="txConcludeReferral" runat="server" Text='<%#Eval("ConcludeReferral")%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Edit"  HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                            <ItemTemplate><asp:LinkButton ID="lnkEdit" CommandName="Select" runat="server" Text="Edit"></asp:LinkButton></ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--  <telerik:GridTemplateColumn HeaderText="ReferToDoctorIds" UniqueName="ReferToDoctorId" Visible="false">
                                            <ItemTemplate><asp:Label ID="txReferToDoctorId" runat="server" Text='<%#Eval("ReferToDoctorId")%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>--%>
                                        <telerik:GridTemplateColumn HeaderText="EncodedId" UniqueName="EncodedId" Visible="false">
                                            <ItemTemplate><asp:Label ID="txEncodedId" runat="server" Text='<%#Eval("EncodedId")%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                       <%-- <telerik:GridTemplateColumn HeaderText="SpecialisationId" UniqueName="SpecialisationId" Visible="false">
                                            <ItemTemplate><asp:Label ID="lblSpecialisationId" runat="server" Text='<%#Eval("SpecialisationId")%>'></asp:Label></ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                       <%-- <telerik:GridTemplateColumn HeaderText="EncounterId" UniqueName="EncounterId" Visible="false">
                                            <ItemTemplate><asp:Label ID="txEncounterId" runat="server" Text='<%#Eval("EncounterId")%>'></asp:Label></ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                        <telerik:GridTemplateColumn HeaderText="CompareId" UniqueName="CompareId" Visible="false">
                                            <ItemTemplate><asp:Label ID="lblCompareId" runat="server" Text='<%#Eval("CompareId")%>'></asp:Label></ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </asp:Panel>
                        <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                            <Windows><telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move"></telerik:RadWindow></Windows>
                        </telerik:RadWindowManager>
                        
                        </div>
                    </div>
                </div>                            
            </div>                        
                        
                        
            <div class="VitalHistory-Div02">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">              
                            <asp:TextBox ID="txtStat" runat="server" Text="" BorderWidth="0" BorderColor="LightGreen" ReadOnly="true" Enabled="false" BackColor="LightGreen" Width="20px"></asp:TextBox> Stat Referral
                            <asp:Button ID="btnClientClose" runat="server"  Style="visibility: hidden;"  OnClick ="btnClientClose_OnClick" /> 
                        </div>
                    </div>
                </div>
            </div>    
                
                
            <asp:HiddenField ID = "hdnRegistrationNo" runat = "server" />
            <asp:HiddenField ID = "hdnRegistrationId" runat = "server" />
            <asp:HiddenField ID = "hdnEncounterId" runat = "server" />
                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>