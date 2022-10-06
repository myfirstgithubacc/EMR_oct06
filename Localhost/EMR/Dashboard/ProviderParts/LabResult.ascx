<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LabResult.ascx.cs" Inherits="EMR_Dashboard_ProviderParts_LabResult" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc">
    <tr>
        <td align="left">
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td id="tdDateRange" runat="server" align="left" width="280px">
                        <asp:Label ID="lblFrom" runat="server" SkinID="label" Text="From"></asp:Label>&nbsp;&nbsp;
                        <telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="1900-01-01 00:00"
                            Width="100px" Skin="Outlook">
                        </telerik:RadDatePicker>
                        &nbsp;
                        <asp:Label ID="lblTo" runat="server" SkinID="label" Text="To"></asp:Label>
                        &nbsp;
                        <telerik:RadDatePicker ID="dtpToDate" Skin="Outlook" runat="server" MinDate="1900-01-01 00:00"
                            Width="100px">
                        </telerik:RadDatePicker>
                    </td>
                    <td>
                        <asp:Label ID="Label15" runat="server" SkinID="label" Text="Entry&nbsp;Site" />
                        <telerik:RadComboBox ID="ddlEntrySitesActual" SkinID="DropDown" runat="server" Width="100px"
                            DropDownWidth="200px" EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                    </td>
                    <td align="left">
                        <asp:Button ID="btnFilter" runat="server" SkinID="Button" Text="Filter" OnClick="btnFilter_Click" />
                        &nbsp;&nbsp;<asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
        <td align="left">
            <asp:Label ID="lblMessage" runat="server" SkinID="label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <telerik:RadGrid ID="gvResultFinal" runat="server" Width="100%" CssClass="RadGrid_Custom"
                EnableEmbeddedSkins="false" BorderWidth="0" AllowFilteringByColumn="false" ShowGroupPanel="false"
                AllowPaging="false" Height="230px" AllowMultiRowSelection="true" AutoGenerateColumns="false"
                ShowStatusBar="true" EnableLinqExpressions="false" GridLines="None" PageSize="5"
                OnColumnCreated="gvResultFinal_ColumnCreated" OnPreRender="gvResultFinal_PreRender"
                OnItemDataBound="gvResultFinal_OnItemDataBound" OnItemCommand="gvResultFinal_OnItemCommand"
                OnItemCreated="gvResultFinal_OnItemCreated">
                <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                    Scrolling-SaveScrollPosition="true">
                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                        AllowColumnResize="false" />
                </ClientSettings>
                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                    <NoRecordsTemplate>
                        <div style="font-weight: bold; color: Red;">
                            No Record Found.</div>
                    </NoRecordsTemplate>
                    <GroupHeaderItemStyle Font-Bold="true" />
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldAlias="LabNo" FieldName="LabNo" HeaderText="Lab&nbsp;No.&nbsp;"
                                    FormatString="" />
                                <telerik:GridGroupByField FieldAlias="RegistrationNo" FieldName="RegistrationNo"
                                    HeaderText='<%$ Resources:PRegistration, regno%>&nbsp;' FormatString="" />
                                <telerik:GridGroupByField FieldAlias="EncounterNo" FieldName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>&nbsp;'
                                    FormatString="" />
                                <telerik:GridGroupByField FieldAlias="PatientName" FieldName="PatientName" HeaderText="Patient Name&nbsp;"
                                    FormatString="" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="LabNo" SortOrder="None" />
                                <telerik:GridGroupByField FieldName="RegistrationNo" SortOrder="None" />
                                <telerik:GridGroupByField FieldName="EncounterNo" SortOrder="None" />
                                <telerik:GridGroupByField FieldName="PatientName" SortOrder="None" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <Columns>
                        <%--<telerik:GridClientSelectColumn UniqueName="chkCollection" HeaderStyle-Width="5%" />--%>
                        <telerik:GridTemplateColumn UniqueName="ServiceName" DefaultInsertValue="" HeaderText="Lab Test"
                            DataField="ServiceName" SortExpression="ServiceName" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                            FilterControlWidth="99%" HeaderStyle-Width="100%" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="SampleName" DefaultInsertValue="" HeaderText="Sample"
                            DataField="SampleName" SortExpression="SampleName" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" HeaderStyle-Width="200px" ShowFilterIcon="false"
                            FilterControlWidth="99%" AllowFiltering="true">
                            <ItemTemplate>
                                <asp:Label ID="lblSampleName" runat="server" Text='<%#Eval("SampleName") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Result" DefaultInsertValue="" HeaderText="Result"
                            DataField="Result" SortExpression="Result" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                            ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%" HeaderStyle-Width="120px"
                            Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblresult" runat="server" Text='<%#Eval("Result") %>' />
                                <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("Result") %>' CommandName="lnkResult"
                                    CommandArgument="None" Visible="false"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="UniqueName1" DefaultInsertValue="" HeaderText="RegistrationId"
                            AllowFiltering="false" FilterControlWidth="99%" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="UniqueName2" DefaultInsertValue="" HeaderText="EncounterId"
                            AllowFiltering="false" FilterControlWidth="99%" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEncounterId" runat="server" Text='<%#Eval("EncounterId") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="LabNo" DefaultInsertValue="" HeaderText='Lab&nbsp;No.'
                            DataField="LabNo" SortExpression="LabNo" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                            ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="EncounterNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                            DataField="EncounterNo" SortExpression="EncounterNo" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                            FilterControlWidth="99%" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="RegistrationNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, regno%>'
                            DataField="RegistrationNo" SortExpression="RegistrationNo" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                            FilterControlWidth="99%" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="PatientName" DefaultInsertValue="" HeaderText="Patient Name"
                            DataField="PatientName" SortExpression="PatientName" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                            FilterControlWidth="99%" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="UniqueName7" DefaultInsertValue="" HeaderText="Age/Gender"
                            AllowFiltering="false" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="UniqueName10" DefaultInsertValue="" HeaderText="Status&nbsp;Color"
                            Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="UniqueName11" DefaultInsertValue="" HeaderText="Sample&nbsp;ID"
                            Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="UniqueName12" DefaultInsertValue="" HeaderText="Status&nbsp;ID"
                            Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblStatusID" runat="server" Text='<%#Eval("StatusID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="UniqueName12" DefaultInsertValue="" HeaderText="StationId"
                            Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblStationId" runat="server" Text='<%#Eval("StationId") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn UniqueName="UniqueName13" DefaultInsertValue="" HeaderText="YearId"
                            Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblYearId" runat="server" Text='<%#Eval("YearId") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn UniqueName="UniqueName14" DefaultInsertValue="" HeaderText="ServiceId"
                            Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:HiddenField ID="hf1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="pnlN" BorderWidth="0" BorderStyle="Solid" Width="100%" ScrollBars="Auto"
                runat="server">
                <asp:Table ID="tblLegend" runat="server" border="0" CellPadding="1" CellSpacing="2">
                </asp:Table>
            </asp:Panel>
        </td>
        <td>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                Behaviors="Close">
                <Windows>
                    <telerik:RadWindow ID="RadWindowPopup" runat="server">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        </td>
    </tr>
</table>
