<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestDetails.aspx.cs" Inherits="LIS_Phlebotomy_TestDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Investigation Details</title>

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

    <link href="../../Include/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { background:#c1e5ef !important;border:1px solid #98abb1 !important; border-top:none !important; color: #333 !important; outline:none !important;}
    </style>

    <script type="text/javascript">
        function checkTextAreaMaxLength(textBox, e, length) {

            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;
            if (!checkSpecialKeys(e)) {
                var maxLength = parseInt(mLen);

                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }
            }

        }
        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main form-group">
            <div class="col-sm-7">
                <h2 style="color:#333;">
                    <asp:Label ID="lblPatientDetails" runat="server"></asp:Label></h2>
            </div>
            <div class="col-sm-3 text-center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label></div>
            <div class="col-sm-2 text-right">
                <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" /></div>
        </div>





        <div class="container-fluid" runat="server">

            <div class="row form-group">
                <div class="col-sm-6">
                    <div class="row">
                        <div class="col-sm-5">
                            <asp:Label ID="Label1" runat="server" Text="Investigation" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtServiceName" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="row">
                        <div class="col-sm-5">
                            <asp:Label ID="Label2" runat="server" Text="Sub Dept." /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtSubName" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
            </div>

            <div class="row form-group">
                <div class="col-sm-6">
                    <div class="row">
                        <div class="col-sm-5">
                            <asp:Label ID="Label5" runat="server" Text="Vacutainer" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtVacutainerName" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="row">
                        <div class="col-sm-5">
                            <asp:Label ID="Label6" runat="server" Text="Vial&nbsp;Color" /></div>
                        <div class="col-sm-7">
                            <asp:Label ID="txtVialColor" Width="100%" runat="server" /></div>
                    </div>
                </div>
            </div>

            <div class="row form-group">
                <div class="col-sm-6">
                    <div class="row">
                        <div class="col-sm-5 PaddingRightSpacing"><%--<asp:Label ID="Label7" runat="server" Text="Instruction for Phlebotomist" /> --%>
                            <asp:Label ID="Label7" runat="server" Text="Instruction Phlebotomist" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtInstructionForPhlebotomy" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="row">
                        
                        <div class="col-sm-5">
                            <asp:LinkButton ID="lbtnNotes" runat="server" CssClass="btn btn-primary" Text="Notes" OnClick="lbtnNotes_OnClick"></asp:LinkButton>
                        </div>
                        <div class="col-sm-7"></div>
                </div>
                  </div>

               </div>

           <div class="row">
               <div class="hrBorder"></div>
           </div>

             <div class="row form-group form-groupTop">
                <div class="col-sm-4">
                    <asp:Label ID="Label9" runat="server" Text="<strong>Activity Log</strong>" /></div>
                <div class="col-sm-8 text-left">
                   </div>
            </div>

            <div class="row">
                <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5">
                            <asp:Label ID="Label16" runat="server" Text="Order By" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtEncodedBy" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="row">
                        <div class="col-sm-5">
                            <asp:Label ID="Label18" runat="server" Width="100%" Text="Order Date" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtEncodedDate" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
            </div>



            <div class="row">
                <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5">
                            <asp:Label ID="Label10" runat="server" Text="Sample Collected By" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtSampleCollectedBy" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5 PaddingRightSpacing">
                            <asp:Label ID="Label11" runat="server" Text="Sample Collected Date" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtSampleCollectedDate" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>

              <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5">
                            <asp:Label ID="Label8" runat="server"  Text="Sample Dispatched By" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtSampleDispatchBy" Width="100%" runat="server"
                        ReadOnly="true" /></div>
                    </div>
                </div>

                <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5 PaddingRightSpacing">
                            <asp:Label ID="Label17" runat="server"  Text="Sample Dispatched Date" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtSampleDispatchedDate"  runat="server"   ReadOnly="true" /></div>
                    </div>
                </div>

                
            </div>

           

            <div class="row form-group">
                <asp:Panel ID="pnlAckGrid" runat="server" SkinID="Panel" BorderColor="#6699CC" BorderWidth="1"
                    BorderStyle="Solid" Width="100%" Height="135px" ScrollBars="None">
                    <asp:UpdatePanel ID="upgvServices" runat="server">
                        <ContentTemplate>
                            <telerik:RadGrid ID="gvAcknowledgementList" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                AllowFilteringByColumn="false" AllowMultiRowSelection="False" runat="server"
                                AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                GridLines="None" AllowPaging="false" PageSize="20" Width="100%" Height="133px" CssClass="outLine">
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                    Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="true" ResizeGridOnColumnResize="true"
                                        AllowColumnResize="false" />
                                </ClientSettings>
                                <MasterTableView TableLayout="Fixed" Width="100%">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="SampleAcknowledgedBy" DefaultInsertValue=""
                                            HeaderText="Sample&nbsp;Acknowledged&nbsp;By" AllowFiltering="false" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSampleAcknowledgedBy" runat="server" Text='<%#Eval("SampleAcknowledgedBy") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="EntrySiteName" DefaultInsertValue="" HeaderText="Entry&nbsp;Site&nbsp;Name"
                                            Visible="True">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntrySiteName" runat="server" Text='<%#Eval("EntrySiteName") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn UniqueName="SampleAcknowledgedDate" DefaultInsertValue=""
                                            HeaderText="Sample&nbsp;Acknowledged&nbsp;Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSampleAcknowledgedDate" runat="server" Text='<%#Eval("SampleAcknowledgedDate") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>

            <div class="row">
                <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5 PaddingRightSpacing">
                            <asp:Label ID="Label4" runat="server"  Text="Result Entered By" /></div>
                        <div class="col-sm-7">
                           <asp:TextBox ID="txtResultEnterBY" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5 PaddingRightSpacing">
                             <asp:Label ID="Label3" runat="server" Text="Result Entered &nbsp;Date" /></div>
                        <div class="col-sm-7">
                          <asp:TextBox ID="txtResultEnteredDate" runat="server"  ReadOnly="true" /></div>
                    </div>
                </div>
                
                  <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5 PaddingRightSpacing">
                            <asp:Label ID="Label12" runat="server" Text="Provisionally Released By" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtProvResultBy" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5">
                            <asp:Label ID="Label13" runat="server" Text="Provisional Result&nbsp;Date" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtProvResultDate" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
              <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5">
                            <asp:Label ID="Label14" runat="server" Text="Finalized By" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtFinalizedBy" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="row form-group">
                        <div class="col-sm-5">
                            <asp:Label ID="Label15" runat="server" Text="Final Result Date" /></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtFinalizedDate" Width="100%" runat="server" ReadOnly="true" /></div>
                    </div>
                </div>

            </div>

            <div class="row form-group">
               <div class="hrBorder"></div>
           </div>

            <div class="row form-group">
                <div class="col-sm-10">
                    <div class="row form-groupTop">
                        <div class="col-sm-3"><asp:Label ID="lblComments" runat="server" Text="Remarks" /></div>
                        <div class="col-sm-8"><asp:TextBox ID="txtFinalRemarks" SkinID="textbox" Width="100%" runat="server" Height="60px" TextMode="MultiLine" onkeyDown="checkTextAreaMaxLength(this,event,'200');" /></div>
                        <div class="col-sm-1"><asp:Button ID="btnUpdateComment" Text="Update Remarks" runat="server" ToolTip="Update Remarks" OnClick="btnUpdateComment_Click" class="btn btn-primary" /></div>
                    </div>
                </div>
                
            </div>

            <div class="row form-group">
                <div class="col-sm-12">
                    <%--<asp:UpdatePanel ID="updatepanel6" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" />
                        </Windows>
                    </telerik:RadWindowManager>
                    <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </div>


        </div>


    </form>
</body>
</html>
