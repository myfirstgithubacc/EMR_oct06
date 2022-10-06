<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OTAntibioticEntry.aspx.cs" Inherits="OTScheduler_OTAntibioticEntry" %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script language="javascript" type="text/javascript" src="../Include/JS/Common.js"></script>
<script type="text/javascript">

    $("#btnVAaided").click(function Display() {
        $("popupdiv").modal('show');
    });

</script>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Other Details</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {
            border: solid #868686 1px !important;
            border-top: none !important;
            border-left: none !important;
            outline: none !important;
            color: #333;
            background: 0 -2300px repeat-x #c1e5ef !important;
        }

        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {
            background-color: #fff !important;
        }

        #ctl00_ContentPlaceHolder1_Panel1 {
            background-color: #c1e5ef !important;
        }

        .RadGrid .rgFilterBox {
            height: 20px !important;
        }

        .RadGrid_Office2007 .rgFilterRow {
            background: #c1e5ef !important;
        }

        .RadGrid_Office2007 .rgPager {
            background: #c1e5ef 0 -7000px repeat-x !important;
            color: #00156e !important;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="updatePanel1" runat="server">
            <ContentTemplate>
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Default">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>

                <div class="container-fluid header_main form-group">
                    <div class="col-sm-6 text-center">
                        <asp:Label ID="lblMessage" runat="server" /></div>
                    <div class="col-sm-6 text-right">
                        <asp:Button ID="btnNew" runat="server" ToolTip="New" CssClass="btn btn-default" Text="New" OnClick="btnNew_Click" />
                        <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-default" OnClientClick="window.close();" />
                        <asp:Button ID="btnSaveData" runat="server" ToolTip="Save" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveData_Click" />
                    </div>
                </div>

                <div class="container-fluid" id="divRISScanTimeCapture" runat="server">
                    <div class="row form-groupTop01">
                        <div class="col-md-6 col-sm-6">

                            <div class="row form-groupTop01" id="div3" runat="server">
                                <div class="col-sm-5 col-md-4 PaddingRightSpacing label2">
                                    <asp:Label ID="lblAdmissionDate" runat="server" Text="Admission Date Time :" /></div>
                                <div class="col-sm-5 col-md-6">
                                    <asp:Label ID="txtAdmissionDate" runat="server"></asp:Label></div>
                                <div class="col-sm-2 col-md-2 PaddingLeftSpacing label2"></div>
                            </div>

                            <div class="row form-groupTop01" id="divRISScan" runat="server">
                                <div class="col-sm-5 col-md-4 PaddingRightSpacing label2">
                                    <asp:Label ID="Label4" runat="server" Text="Intubation Start Time :" /><span id="Span1" style='color: Red' runat="server">&nbsp;*</span>
                                </div>
                                <div class="col-sm-5 col-md-6">
                                    <telerik:RadDateTimePicker ID="txtoralstartDate" runat="server" TimeView-Columns="6" DateInput-DateFormat="dd/MM/yyyy HH:mm" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" CssClass="drapDrowHeight" Width="100%">
                                        <TimeView runat="server" Interval="0:30:0" />
                                    </telerik:RadDateTimePicker>


                                </div>
                                <div class="col-sm-2 col-md-2 PaddingLeftSpacing label2">
                                    <asp:Literal ID="ltDateTime" runat="server" Text="HH MM"></asp:Literal>
                                </div>
                            </div>

                            <div class="row form-groupTop01" id="div1" runat="server">
                                <div class="col-sm-5 col-md-4 PaddingRightSpacing label2">
                                    <asp:Label ID="lblscanindt" runat="server" Text="Incision Time :" /><span id="Span3" style='color: Red' runat="server">&nbsp;*</span></div>
                                <div class="col-sm-5 col-md-6">
                                    <telerik:RadDateTimePicker ID="txtScanInDate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-Columns="6" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                        <TimeView runat="server" Interval="0:30:0" />
                                    </telerik:RadDateTimePicker>
                                    <%-- <telerik:RadComboBox ID="RadComboBox2" runat="server" AutoPostBack="True" Height="300px" Skin="Outlook" Width="45px"></telerik:RadComboBox>--%>
                                </div>
                                <div class="col-sm-2 col-md-2 PaddingLeftSpacing label2">
                                    <asp:Literal ID="Literal1" runat="server" Text="HH MM"></asp:Literal></div>
                            </div>

                            <div class="row form-groupTop01" id="div2" runat="server">
                                <div class="col-sm-5 col-md-4 PaddingRightSpacing label2">
                                    <asp:Label ID="Label6" runat="server" Text="Antibiotic Prophylactic Time :" /></div>
                                <div class="col-sm-5 col-md-6">
                                    <telerik:RadDateTimePicker ID="txtScanOutDate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" TimeView-Columns="6">
                                        <TimeView runat="server" Interval="0:30:0" />
                                    </telerik:RadDateTimePicker>
                                </div>
                                <div class="col-sm-2 col-md-2 PaddingLeftSpacing label2">
                                    <asp:Literal ID="Literal2" runat="server" Text="HH MM"></asp:Literal></div>
                            </div>

                            <div class="row form-groupTop01" id="div5" runat="server">
                                <div class="col-sm-5 col-md-4 PaddingRightSpacing label2">
                                    <asp:Label ID="lblIntiTime" runat="server" Text="Ballooning Time:" /></div>
                                <div class="col-sm-5 col-md-6">
                                    <telerik:RadDateTimePicker ID="txtIntiTime" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" TimeView-Columns="6">
                                        <TimeView runat="server" Interval="0:30:0" />
                                    </telerik:RadDateTimePicker>
                                </div>
                                <div class="col-sm-2 col-md-2 PaddingLeftSpacing label2">
                                    <asp:Literal ID="Literal3" runat="server" Text="HH MM"></asp:Literal></div>
                            </div>

                            <div class="row form-groupTop01" id="divRemarks" runat="server">
                                <div class="col-sm-5 col-md-4 PaddingRightSpacing label2">
                                    <asp:Label ID="Label1" runat="server" Text="Remarks:" /></div>
                                <div class="col-sm-7 col-md-8">
                                    <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" MaxLength="1000" Width="300px" Height="50px" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row form-groupTop01" id="Table1" runat="server">
                                <div class="col-sm-5 col-md-4 PaddingRightSpacing label2">
                                    <asp:Label ID="lblOTDetail" runat="server" /></div>
                                <div class="col-sm-7 col-md-8">
                                    <asp:Label ID="lblOTNO" runat="server" ReadOnly="true" /></div>
                            </div>

                        </div>



                        <div class="col-md-6 col-sm-6">
                            <div class="row form-groupTop01" id="divReportTypelbl" runat="server">
                                <div class="col-sm-2 col-md-2 PaddingRightSpacing">
                                    <asp:Label ID="lblAntibiotic" runat="server" Text="Antibiotic"></asp:Label><span id="Span2" style='color: Red' runat="server">&nbsp;*</span></div>
                                <div class="col-sm-8 col-md-8">
                                    <telerik:RadListBox ID="rlbAntibiotic" runat="server" Height="200px" Width="100%" ShowCheckAll="true" SelectionMode="Multiple" CheckBoxes="true" AutoPostBack="True"></telerik:RadListBox>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>


                <div class="container-fluid">
                    <div class="row margin_Top">
                        <asp:Panel ID="Panel1" runat="server" BorderColor="#6699CC" BorderWidth="1" BorderStyle="Solid" Width="100%">
                            <telerik:RadGrid ID="gvOTAntibioticDetail" runat="server" Skin="Office2007" BorderWidth="0"
                                ShowGroupPanel="false" Width="100%" Height="340px" AllowPaging="false" AllowMultiRowSelection="false"
                                EnableLinqExpressions="false" AutoGenerateColumns="false" ClientSettings-EnablePostBackOnRowClick="false" OnItemCommand="gvOTAntibioticDetail_ItemCommand">
                                <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                </ClientSettings>
                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">
                                            No Record Found.
                                        </div>
                                    </NoRecordsTemplate>
                                    <ItemStyle Wrap="true" />
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="OTBookingID" DefaultInsertValue="" HeaderText="OTBookingId" Visible="false">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblOTHeader" runat="server" Text="OTBookingId"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOTBookingId" Text='<%#Eval("OTBookingId") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ServiceId" DefaultInsertValue="" HeaderText="ServiceID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ServiceName" DefaultInsertValue="" HeaderText="Surgery Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="IntubationStartTime" DefaultInsertValue="" HeaderText="Intubation Start Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIntubation" runat="server" Text='<%#Eval("IntubationStartTime") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="IncisionTime" DefaultInsertValue="" HeaderText="Incision Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIncisionTime" runat="server" Text='<%#Eval("IncisionTime") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="AntibioticProphylacticTime" DefaultInsertValue="" HeaderText="Antibiotic Prophylactic Time"
                                            Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAnProphylactic" runat="server" Text='<%#Eval("AntibioticProphylacticTime") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="BallooningTime" DefaultInsertValue="" HeaderText="Ballooning Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBallooningTime" runat="server" Text='<%#Eval("BallooningTime") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Remarks" DefaultInsertValue="" HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Antibiotic" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" UniqueName="YesTemplateColumn" Visible="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkYes" runat="server" Text='<%#Eval("Antibiotics") %>' CommandName="Yes"></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Select" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" UniqueName="SelectTemplateColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSelect" runat="server" CommandArgument='<%#Eval("ServiceId") %>' Text="Select" CommandName="Select" CausesValidation="false"></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </asp:Panel>
                        <asp:HiddenField ID="hdnServiceId" runat="server" />
                        <%--<asp:HiddenField ID ="hdnAnti" runat="server" />--%>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>



