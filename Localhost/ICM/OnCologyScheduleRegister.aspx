<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="OnCologyScheduleRegister.aspx.cs" Inherits="ICM_OnCologyScheduleRegister" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link id="Link7" href="../Include/EMRStyle.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <style>

        #UpdatePanel3 table tr{ font-size:12px; padding-left:10px;  }
        #UpdatePanel3 table th{ font-size:12px; padding:10px 5px; background:#C1E5EF;   }
        #UpdatePanel3 table td {
            font-size: 12px;
            padding: 3px;
        }
    </style>

</head>

<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3 col-sm-3">
                            <div class="WordProcessorDivText">
                                <h2>Schedule Register</h2>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="WordProcessorDivText">
                                <h5>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </h5>
                            </div>
                        </div>
                        <div class="col-md-5 col-sm-5">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="PatientBtn01" OnClientClick="window.close();" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>

                    <div class="VitalHistory-Div02">
                        <div class="container-fluid">

                            <div class="row">

                                <div class="col-md-12 col-sm-12">
                                    <div class="row">
                                        <div class="col-md-2">

                                            <telerik:RadComboBox ID="ddlName" runat="server" AppendDataBoundItems="true"
                                                Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Reg No" Value="R" />
                                                    <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnRefresh">
                                                <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" Width="150px"
                                                    Visible="false" />
                                                <asp:TextBox ID="txtSearchN" SkinID="textbox" Width="150px" runat="server" Text=""
                                                    MaxLength="10" onkeyup="return validateMaxLength();" />
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                    FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                                            </asp:Panel>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="ComplaintsHistoryBox01">
                                                <h2>
                                                    <asp:Label ID="lblfrndate" runat="server" Text="From Date"></asp:Label></h2>
                                                <h3>
                                                    <telerik:RadDatePicker ID="dtpfrmdate" runat="server" Width="100px" DateInput-DateFormat="dd/MM/yyyy"></telerik:RadDatePicker>
                                                    <h3></h3>
                                                    <h5>
                                                        <asp:Label ID="lbltodate" runat="server" Text="To Date"></asp:Label>
                                                        </h5>
                                                    <h3>
                                                        <telerik:RadDatePicker ID="dtpTodate" runat="server" Width="100px" DateInput-DateFormat="dd/MM/yyyy">
                                                        </telerik:RadDatePicker>
                                                        <h3></h3>
                                                        <h3>
                                                            <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnFilter_Click" Width="70px" />
                                                        </h3
                                                        <h3>
                                                           &nbsp; <asp:Button ID="btnrefresh" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnrefresh_Click" />
                                                        <h3></h3>
                                                        <h3></h3>
                                                        <h3></h3>
                                                       </h3>
                                                       
                                            </div>

                                        </div>
                                        <div class="col-md-2">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnPrintPreview" runat="server" Text="View Report" CssClass="btn btn-primary" OnClick="btnPrintData_OnClick" ToolTip="Click to Print Preview" /></h3>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-1 col-sm-1">
                                    <div class="ComplaintsHistoryBox01">
                                    </div>
                                </div>
                                <div class="col-md-11 col-sm-11">
                                </div>

                            </div>

                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>


            <div class="">
                <div class="container-fluid">
                    <div class="row">

                        <div class="col-md-12 col-sm-12">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grvOncologyScheduleRegister" runat="server" AutoGenerateColumns="False" PageSize="15" PagerStyle-CssClass="pagination"
                                        CellPadding="0" CellSpacing="0" Width="100%" Style="text-align: left" AllowPaging="true" OnPageIndexChanging="grvOncologyScheduleRegister_PageIndexChanging"
                                        HeaderStyle-BackColor="#eeeeee" BackColor="White" BorderColor="#eeeeee" BorderWidth="1px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl.No" HeaderStyle-Width="2%" ItemStyle-Width="2%" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSlno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date" HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Width="78%" Height="20" Text='<%#Eval("Date") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="MRDNO." HeaderStyle-Width="6%" ItemStyle-Width="6%" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMRDNO" runat="server" Text='<%#Eval("RegistrationNo") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Patient Name" HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" MaxLength="3" Text='<%#Eval("Patientname") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name of Drug and Dose" HeaderStyle-Width="50%" ItemStyle-Width="50%" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChemoSchedule" runat="server" Text='<%#Eval("Chemoshedule") %>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Cycle" HeaderStyle-Width="2%" ItemStyle-Width="2%" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCycle" runat="server" Text='<%#Eval("Cycle") %>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Days" HeaderStyle-Width="2%" ItemStyle-Width="2%" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDAY" runat="server" Text='<%#Eval("DAY") %>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>

                                        <EmptyDataTemplate>
                                            <div style="font-weight: bold; color: Red;">
                                                No Record Found.
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <asp:UpdatePanel ID="updatepanel6" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnPrintPreview" />
                    </Triggers>
                    <ContentTemplate>
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForReport" runat="server" Behaviors="Default" />
                            </Windows>
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updatepanelclose" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>
        </div>
    </form>

</body>
</html>

