<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="LabResults.aspx.cs" Inherits="EMR_Dashboard_ProviderParts_LabResults" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainStyle.css" rel="stylesheet" type="text/css" />

    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">
        function OnClientCloseReviewed(oWnd, args) {
            $get('<%=btnFilter.ClientID%>').click();
        }

    </script>

    <style type="text/css">
        .blink {
            text-decoration: blink;
            color: Green;
        }

        .noblink {
            text-decoration: inherit;
        }

        #ctl00$ContentPlaceHolder1$txtSearchCretria {
            height: 20px !important;
        }
        .RadComboBox table td.rcbArrowCell {
            position: absolute!important;
            right: auto!important;
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
                        <div class="col-md-7 col-sm-7"></div>
                        <div class="col-md-2 col-sm-2"></div>
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
                                    <telerik:RadComboBox ID="ddlProviders" runat="server" Skin="Outlook" Width="140px" DropDownWidth="400px" Filter="Contains" AutoPostBack="True" BorderColor="ActiveBorder" BackColor="AliceBlue" OnSelectedIndexChanged="ddlProviders_SelectedIndexChanged" />
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


                        <asp:Label ID="lblNew" runat="server" CssClass="noblink" Font-Bold="true"></asp:Label>
                        &nbsp;&nbsp;
                    <asp:Label ID="lblResultChanged" runat="server" CssClass="noblink" Font-Bold="true"></asp:Label>
                    </div>

                </div>
            </div>
            </div>                 
            
            
            
    <div class="ImmunizationDD-Div">
        <div class="container-fluid">
            <div class="row">

                <div class="col-md-12">
                    <div class="LabResultsDiv">
                        <h2>
                            <asp:CheckBox ID="chkselectAll" runat="server" Text="Select All" OnCheckedChanged="chkselectAll_ChckChanged" AutoPostBack="true" /></h2>
                        <h3>
                            <asp:Button ID="btnreviewselected" runat="server" Text="Review Selected" CssClass="PatientBtn02" OnClick="ReviewAll" /></h3>


                        <asp:GridView ID="gvResultFinal" runat="server" SkinID="gridviewOrder" AlternatingRowStyle-BackColor="Bisque"
                            GridLines="Both" Width="99%" Height="100%" BorderWidth="1" AllowPaging="true" PageSize="15" AllowSorting="true" AllowMultiRowSelection="false" AutoGenerateColumns="false"
                            ShowStatusBar="true" OnRowDataBound="gvResultFinal_OnRowDataBound" OnRowCommand="gvResultFinal_OnRowCommand" OnPageIndexChanging="gvResultFinal_OnPageIndexChanging">
                            <Columns>
                                <asp:TemplateField Visible="true">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkselect" runat="server" Checked="false" /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Source" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Patient Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultDate" runat="server" Text='<%#Eval("RegistrationNo") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order Date" Visible="True">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, LABNO%>' Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Provider" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, ipno%>' Visible="True">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Investigation" Visible="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkviewresult" runat="server" Text='<%#Eval("ServiceName") %>' CommandName="Print" ToolTip="Click here to Preview Result" Visible="false"></asp:LinkButton>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' Visible="true" />
                                        <%--ForeColor="#0B0080"--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Result" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblresult" runat="server" Visible="false" Text='<%#Eval("Result") %>' />
                                        <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("Result") %>' CommandName="Result" CommandArgument="None" Visible="true" ForeColor="Black"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Print" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkprint" runat="server" CommandName="Print" CommandArgument="None" Text="Print" ForeColor="Black"></asp:LinkButton></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" Visible="true">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnResultId" runat="server" Value='<%#Eval("ResultId") %>' />
                                        <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" CommandArgument="None" Text="Select" ForeColor="Black"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="RegistrationId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AbnormalValue" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAbnormalValue" runat="server" Text='<%#Eval("AbnormalValue") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CriticalValue" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCriticalValue" runat="server" Text='<%#Eval("CriticalValue") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Age/Gender" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status&nbsp;Color" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sample&nbsp;ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status&nbsp;ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusID" runat="server" Text='<%#Eval("StatusID") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="StationId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStationId" runat="server" Text='<%#Eval("StationId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ServiceId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ResultRemarksId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultRemarksId" runat="server" Text='<%#Eval("ResultRemarksId") %>' />
                                        <asp:HiddenField ID="hdnPrescribedTest" runat="server" Value='<%#Eval("PrescribedTest") %>' />
                                        <asp:HiddenField ID="hdnFinalizeResultCount" runat="server" Value='<%#Eval("FinalizeResultCount") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="StatusCode" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusCode" runat="server" Text='<%#Eval("StatusCode") %>' /></ItemTemplate>
                                </asp:TemplateField>


                            </Columns>
                        </asp:GridView>

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
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server" Behaviors="Close">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowPopup" runat="server"></telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server" Behaviors="Close">
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
                    <img id="Img1" src="~/Images/loading.gif" alt="loading" runat="server" /></div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
