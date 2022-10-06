<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="LabResults.aspx.cs" Inherits="EMR_Dashboard_ProviderParts_LabResults" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <link href="../../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../../Include/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../../../Include/css/mainStyle.css" rel="stylesheet" type="text/css" />

    <link href="../../../Include/css/emr.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">
        function OnClientCloseReviewed(oWnd, args) {
            $get('<%=btnFilter.ClientID%>').click();
        }

    </script>

    <style type="text/css">
        #ctl00_ContentPlaceHolder1_gvResultFinal_GridHeader {
            margin-right: 0 !important;
        }

        #ctl00_ContentPlaceHolder1_gvResultFinal_ctl00_ctl02_ctl01_chkselectSelectCheckBox {
            margin-left: 7px !important;
        }

        .rgMasterTable td .rgClipCells td {
            border: 0px !important;
        }

        .blink {
            text-decoration: blink;
            color: Green;
        }

        .noblink {
            text-decoration: inherit;
        }

        .LabResultsDiv label {
            font-weight: normal !important;
        }

        #ctl00$ContentPlaceHolder1$txtSearchCretria {
            height: 20px !important;
        }
    </style>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3 col-sm-3">
                            <div class="WordProcessorDivText">
                                <h2>
                                    <asp:Label ID="Label" runat="server" Text="&nbsp;Lab Result" /></h2>
                            </div>
                        </div>
                        <div class="col-md-7 col-sm-7">
                            <asp:Label ID="lblNew" runat="server" CssClass="noblink" Font-Bold="true" Font-Size="Larger"></asp:Label>
                            &nbsp;&nbsp;
                    <asp:Label ID="lblResultChanged" runat="server" CssClass="noblink" Font-Bold="true" Font-Size="Larger"></asp:Label>
                        </div>
                        <div class="col-md-1 col-sm-1">

                            <asp:Button ID="btnreviewselected" runat="server" Text="Review Selected" CssClass="PatientBtn02" OnClick="ReviewAll" />
                        </div>
                    </div>
                </div>
            </div>


            <div class="VitalHistory-Div02">
                <div class="container-fluid">

                    <div class="row">
                        <div class="col-md-3">
                            <div class="LabResultsDiv">
                                <h2>
                                    <asp:Label ID="lblResultfor" runat="server" Text="Result For"></asp:Label></h2>
                                <h3>
                                    <telerik:RadComboBox ID="ddlProviders" runat="server" Skin="Outlook" Width="150px" DropDownWidth="350px" Filter="Contains" AutoPostBack="True" BorderColor="ActiveBorder" BackColor="AliceBlue" OnSelectedIndexChanged="ddlProviders_SelectedIndexChanged" />
                                </h3>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="LabResultsDiv">
                                <h2>
                                    <asp:Label ID="Label19" runat="server" Text="Reviewed&nbsp;Staus" /></h2>
                                <h3>
                                    <telerik:RadComboBox ID="ddlReviewedStatus" SkinID="DropDown" Width="145px" runat="server" BorderColor="ActiveBorder" BackColor="AliceBlue">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="ALL" Value="2" />
                                            <telerik:RadComboBoxItem Text="Not Reviewed" Value="0" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Reviewed" Value="1" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </h3>
                            </div>
                        </div>


                        <div class="col-md-3">
                            <div class="LabResultsDiv01a">
                                <h2>
                                    <asp:Label ID="Label7" runat="server" Text="Search By" /></h2>
                                <h3>
                                    <telerik:RadComboBox ID="ddlSearch" EmptyMessage="[ Select ]" runat="server" Width="67px">
                                        <Items>
                                            <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="RN" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Patient Name" Value="PN" />
                                            <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, IPNo%>' Value="IP" Visible="false" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:TextBox ID="txtSearchCretria" SkinID="textbox" Width="82px" Height="20px" runat="server" Text="" MaxLength="15" />&nbsp;
                                </h3>
                            </div>
                        </div>


                        <div class="col-md-3">
                            <div class="LabResultsDiv02">
                                <h2>
                                    <asp:CheckBox ID="chkAbnormalValue" runat="server" /></h2>
                                <h3>
                                    <asp:Label ID="Label2" runat="server" Text="Abnormal&nbsp;Result(s)" ForeColor="DarkViolet" /></h3>
                                <h2>
                                    <asp:CheckBox ID="chkCriticalValue" runat="server" /></h2>
                                <h4>
                                    <asp:Label ID="Label1" runat="server" Text="Critical&nbsp;Result(s)" ForeColor="Red" />
                                </h4>
                            </div>
                        </div>

                    </div>


                    <div class="row">
                        <div class="col-md-3">
                            <div class="LabResultsDiv">
                                <h2>
                                    <asp:Label ID="Label20" runat="server" Text="Date Range:"></asp:Label></h2>
                                <h3>
                                    <telerik:RadComboBox ID="ddlTime" SkinID="DropDown" runat="server" AutoPostBack="True" Width="140px" CausesValidation="false" OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="[ Select ]" Value="0" />
                                            <telerik:RadComboBoxItem Text="Today Result" Value="Today" />
                                            <telerik:RadComboBoxItem Text="Last Week Result" Value="LastWeek" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Last Two Weeks Result" Value="LastTwoWeeks" />
                                            <telerik:RadComboBoxItem Text="Last One Month Result" Value="LastOneMonth" />
                                            <telerik:RadComboBoxItem Text="Last Three Months Result" Value="LastThreeMonths" />
                                            <telerik:RadComboBoxItem Text="Last Year Result" Value="LastYear" />
                                            <telerik:RadComboBoxItem Text="Date Range Result" Value="DateRange" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </h3>
                            </div>
                        </div>


                        <div class="col-md-3">
                            <div class="LabResultsDivDate" id="tblDate" runat="server" visible="false">
                                <h2>
                                    <asp:Label ID="Label17" runat="server" Text="From " /></h4>
                                <h3>
                                    <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="96px" DateInput-ReadOnly="true" />
                                </h3>
                                    <h2>
                                        <asp:Label ID="Label18" runat="server" Text="To " /></h4>
                                <h3>
                                    <telerik:RadDatePicker ID="txtToDate" runat="server" Width="96px" DateInput-ReadOnly="true" />
                                </h3>
                            </div>
                        </div>




                        <div class="col-md-3">
                            <div class="LabResultsDiv02">
                                <h5>
                                    <asp:CheckBox ID="chkL" runat="server" SkinID="checkbox" Text="(L)&nbsp;Below&nbsp;Range" /></h5>
                                <h6>
                                    <asp:CheckBox ID="chkH" runat="server" SkinID="checkbox" Text="(H)&nbsp;Above&nbsp;Range" /></h6>
                            </div>
                        </div>



                        <div class="col-md-3">
                            <div class="LabResultsDiv02">
                                <asp:Button ID="btnFilter" CssClass="PatientBtn02" runat="server" OnClick="btnFilter_Click" Text="Filter" />
                            </div>
                        </div>
                    </div>


                </div>
            </div>






            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">

                        <div class="col-md-12">
                            <span style="width: 100%; float: left; margin: 0; padding: 0; font-size: 12px;">

                                <div class="LabbgTopText-Message01">
                                    <h2>
                                        <asp:Label ID="lblMessage" runat="server" Text="" />
                            </span></h2>
                        </div>


                        <asp:TextBox ID="txtProviderId" runat="server" Style="visibility: hidden; position: absolute;" />
                        <asp:TextBox ID="txtRegistrationId" runat="server" Style="visibility: hidden; position: absolute;" />



                    </div>

                </div>
            </div>
            </div>                 
            
            
            
    <div class="ImmunizationDD-Div">
        <div class="container-fluid">
            <div class="row">

                <div class="col-md-12">
                    <div class="LabResultsDiv">
                        <asp:UpdatePanel ID="UpdatePanel100" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvResultFinal" runat="server" ShowGroupPanel="false"
                                    Skin="Office2007" AlternatingRowStyle-BackColor="Bisque" EnableEmbeddedSkins="false"
                                    Width="99%" Height="450px" BorderWidth="0" AllowPaging="true" PageSize="15" AllowSorting="true" AllowMultiRowSelection="true" AutoGenerateColumns="false"
                                    ShowStatusBar="true" OnItemDataBound="gvResultFinal_OnRowDataBound" OnItemCommand="gvResultFinal_OnRowCommand" OnPageIndexChanged="gvResultFinal_OnPageIndexChanging" HeaderStyle-BackColor="#C1E5EF" CellPadding="0" CellSpacing="0" GridLines="None">

                                    <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                        Scrolling-SaveScrollPosition="true">
                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                            AllowColumnResize="false" />
                                    </ClientSettings>
                                    <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">
                                                No Record Found.
                                            </div>
                                        </NoRecordsTemplate>


                                        <Columns>
                                            <telerik:GridClientSelectColumn UniqueName="chkselect" HeaderStyle-Width="50px"
                                                ItemStyle-Width="50px" />
                                            <%--     <telerik:GridClientSelectColumn UniqueName="chkCollection" HeaderStyle-Width="40px"
                                                    ItemStyle-Width="50px" />--%>
                                            <%-- <telerik:GridTemplateColumn Visible="true">
                                                <HeaderTemplate>
                                                <asp:CheckBox ID="chkselectAll" runat="server" Text="All" OnCheckedChanged="chkselectAll_ChckChanged" AutoPostBack="true" />
                                            </HeaderTemplate>
                                                  <ItemTemplate>
                                                    <asp:CheckBox ID="chkselect" runat="server" Checked="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>--%>
                                            <telerik:GridTemplateColumn HeaderText="Source" Visible="true" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                                    <asp:HiddenField ID="hdnReviewedStatus" runat="server" Value='<%#Eval("ReviewedStatus") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Patient Name" Visible="true" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText='<%$ Resources:PRegistration, regno%>' Visible="true" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblResultDate" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Order Date" Visible="True" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText='<%$ Resources:PRegistration, LABNO%>' Visible="true" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Provider" Visible="true" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText='<%$ Resources:PRegistration, ipno%>' Visible="True" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Investigation" Visible="true" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkviewresult" runat="server" Text='<%#Eval("ServiceName") %>' CommandName="Print" ToolTip="Click here to Preview Result" Visible="false"></asp:LinkButton>
                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' Visible="true" />
                                                    <%--ForeColor="#0B0080"--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Result" Visible="true" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblresult" runat="server" Visible="false" Text='<%#Eval("Result") %>' />
                                                    <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("Result") %>' CommandName="Result" CommandArgument="None" Visible="true" ForeColor="Black"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Print" Visible="false" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkprint" runat="server" CommandName="Print" CommandArgument="None" Text="Print" ForeColor="Black"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Review" Visible="true" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnResultId" runat="server" Value='<%#Eval("ResultId") %>' />

                                                    <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" CommandArgument="None" Text="Review" ForeColor="Black"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>



                                            <telerik:GridTemplateColumn HeaderText="RegistrationId" Visible="false" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="AbnormalValue" Visible="False" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAbnormalValue" runat="server" Text='<%#Eval("AbnormalValue") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="CriticalValue" Visible="False" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCriticalValue" runat="server" Text='<%#Eval("CriticalValue") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Age/Gender" Visible="false" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Status&nbsp;Color" Visible="false" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Sample&nbsp;ID" Visible="false" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Status&nbsp;ID" Visible="false" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatusID" runat="server" Text='<%#Eval("StatusID") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="StationId" Visible="false" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStationId" runat="server" Text='<%#Eval("StationId") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="ServiceId" Visible="false" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="ResultRemarksId" Visible="False" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblResultRemarksId" runat="server" Text='<%#Eval("ResultRemarksId") %>' />
                                                    <asp:HiddenField ID="hdnPrescribedTest" runat="server" Value='<%#Eval("PrescribedTest") %>' />
                                                    <asp:HiddenField ID="hdnFinalizeResultCount" runat="server" Value='<%#Eval("FinalizeResultCount") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="StatusCode" Visible="False" HeaderStyle-Width="111px"
                                                ItemStyle-Width="111px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatusCode" runat="server" Text='<%#Eval("StatusCode") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>


                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="orderPopCenterBottomPart02">

                        <h2>
                            <asp:Label ID="lblColorCodeNotReviewed" runat="server" BackColor="White" BorderWidth="1px" Text="&nbsp;" Width="16px" Height="16px" />
                            <asp:Label ID="Label3" runat="server" SkinID="label" Text="Not Reviewed" />
                        </h2>
                        <h2>
                            <asp:Label ID="lblColorCodeNotReviewedPending" runat="server" BackColor="PeachPuff" BorderWidth="1px" Text="&nbsp;" Width="16px" Height="16px" />
                            <asp:Label ID="Label14" runat="server" SkinID="label" Text="Not Reviewed Some Result Still Pending" />
                        </h2>
                        <h2>
                            <asp:Label ID="lblColorCodeReviewed" runat="server" BackColor="LightGray" BorderWidth="1px" Text="&nbsp;" Width="16px" Height="16px" />
                            <asp:Label ID="Label12" runat="server" SkinID="label" Text="Reviewed" />
                        </h2>

                    </div>




                </div>

            </div>
        </div>
    </div>


            <br />

            <asp:Table ID="tblLegend" runat="server" border="0" CellPadding="1" CellSpacing="1" Visible="false" />

            <table>
                <tr>
                    <td>
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server" Behaviors="Close,Move,Maximize"  Skin="Metro">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowPopup" runat="server"></telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server" Behaviors="Close,Move,Maximize"  Skin="Metro">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server"></telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </td>
                </tr>
            </table>

        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFilter" />
            <asp:PostBackTrigger ControlID="gvResultFinal" />
        </Triggers>

    </asp:UpdatePanel>
    <asp:UpdateProgress ID="dvProcess" runat="server" AssociatedUpdatePanelID="pnl" DisplayAfter="2000" DynamicLayout="true">
        <ProgressTemplate>
            <center>
                <div style="width: 154; position: absolute; bottom: 0; height: 60; left: 500px; top: 300px">
                    <img id="Img1" src="~/Images/loading.gif" alt="loading" runat="server" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
