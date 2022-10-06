<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewOutsourcePatientHistory.aspx.cs" MasterPageFile="~/Include/Master/BlankMaster.master" Inherits="LIS_Phlebotomy_ViewOutsourcePatientHistory" %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Src="~/Include/Components/TopPanelNew.ascx" TagPrefix="ucl" TagName="TopPanelNew" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <div>
        <style>
            .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {
                border: solid #868686 1px !important;
                border-top: none !important;
                color: #333;
                background: 0 -2300px repeat-x #c1e5ef !important;
            }
        </style>


        <script type="text/javascript">

            function ClientSideClick(myButton) {
                // Client side validation
                if (typeof (Page_ClientValidate) == 'function') {
                    if (Page_ClientValidate() == false) {
                        return false;
                    }
                }

                //make sure the button is not of type "submit" but "button"
                if (myButton.getAttribute('type') == 'button') {
                    // disable the button
                    myButton.disabled = true;
                    myButton.className = "btn-inactive";
                    myButton.value = "Processing...";
                }

                return true;
            }
            function LinkBtnMouseOver(lnk) {
                document.getElementById(lnk).style.color = "red";
            }
            function LinkBtnMouseOut(lnk) {
                document.getElementById(lnk).style.color = "blue";
            }
        </script>

        <script type="text/javascript">
            if (window.captureEvents) {
                window.captureEvents(Event.KeyUp);
                window.onkeyup = executeCode;
            }
            else if (window.attachEvent) {
                document.attachEvent('onkeyup', executeCode);
            }

        </script>

        <style type="text/css">
            .RadGrid_Office2007 .rgSelectedRow {
                background-color: #ffcb60 !important;
            }
        </style>
        <asp:UpdatePanel ID="upd1cvsdgfsdf" runat="server">
            <ContentTemplate>
                <div class="">
                    <div class="container-fluid">
                        <ucl:TopPanelNew runat="server" ID="TopPanelNew" />
                    </div>
                </div>
                <div class="row">
                    <asp:Label ID="lblMessage" runat="server" SkinID="label" />
                </div>


                <div class="container-fluid">
                    <div class="row">
                        <asp:Panel ID="PanelN" runat="server" BorderColor="#6699CC" BorderWidth="1" BorderStyle="Solid"
                            Width="100%" Height="480px" ScrollBars="None">
                            <telerik:RadGrid ID="gvResultFinal" runat="server" Width="100%" Skin="Office2007"
                                BorderWidth="0px" AllowPaging="True" AllowCustomPaging="True" Height="99%" AllowMultiRowSelection="True"
                                AutoGenerateColumns="False" ShowStatusBar="True" EnableLinqExpressions="False"
                                GridLines="None" PageSize="200" OnItemCommand="gvResultFinal_OnItemCommand" CellSpacing="0">
                                <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                    Scrolling-SaveScrollPosition="true">
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="false" />
                                </ClientSettings>
                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Record Found.
                                        </div>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="Id" ItemStyle-Width="01%" HeaderStyle-Width="01%" />
                                        <telerik:GridTemplateColumn UniqueName="LabNo" DefaultInsertValue="" HeaderText='Lab&nbsp;No.'
                                            DataField="LabNo" SortExpression="LabNo" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="10%" Visible="true" ItemStyle-Width="03%" HeaderStyle-Width="03%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="ResultDate" DefaultInsertValue="" HeaderText="ResultDate"
                                            FilterControlWidth="99%" HeaderStyle-Width="10%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblresultdate" runat="server" Text='<%#Eval("ResultDate") %>' />
                                                 <asp:HiddenField ID="DiagInvResultOPId" runat="server" Value='<%#Eval("DiagInvResultOPId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ServiceName" DefaultInsertValue="" HeaderText="Investigation"
                                            FilterControlWidth="99%" HeaderStyle-Width="10%" Visible="true">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnServiceid" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                <asp:Label ID="lblservicename" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                <asp:HiddenField ID="hdnDiagSampleId" runat="server" Value='<%#Eval("DiagSampleId") %>' />
                                                <asp:HiddenField ID="hdnStatuCode" runat="server" Value='<%#Eval("StatusCode") %>' />
                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="FieldName" DefaultInsertValue="" HeaderText="Field&nbsp;Name"
                                            DataField="FieldName" SortExpression="FieldName" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" HeaderStyle-Width="13%" ShowFilterIcon="false"
                                            FilterControlWidth="99%" AllowFiltering="true" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName") %>' />
                                                <asp:HiddenField ID="hdnFieldId" runat="server" Value='<%#Eval("FieldId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Result" DefaultInsertValue="" HeaderText="Result"
                                            DataField="Result" SortExpression="Result" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%" HeaderStyle-Width="6%"
                                            Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblresult" runat="server" Visible="false" Text='<%#Eval("Result") %>' />
                                                <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("Result") %>' CommandName="Result"
                                                    CommandArgument="None" Visible="true" ForeColor="Black" CausesValidation="false" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="RefRange" DefaultInsertValue="" HeaderText="Ref. Range" ShowFilterIcon="false"
                                            Visible="true" FilterControlWidth="99%" HeaderStyle-Width="6%">
                                            <ItemTemplate>

                                                <asp:Label ID="lblrefrange" runat="server" Text='<%#Eval("refRange") %>' />
                                                <asp:Label ID="lblrefrangetooltip" runat="server" ForeColor="Blue" />
                                                <asp:HiddenField ID="hdnsprangevalue" runat="server" Value='<%#Eval("SpecialReferenceRange") %>' />
                                                <telerik:RadToolTip runat="server" ID="ttSpecialReferenceRange" TargetControlID="lblrefrangetooltip" IsClientID="false" ShowEvent="OnMouseOver" HideEvent="Default" Position="BottomRight" RelativeTo="Mouse" Width="100%" Height="250px" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="UniqueName11" DefaultInsertValue="" HeaderText="Sample&nbsp;ID"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="CriticalValue" DefaultInsertValue="" HeaderText="CriticalValue"
                                            AllowFiltering="false" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCriticalValue" runat="server" Text='<%#Eval("CriticalValue") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                      <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"  HeaderStyle-Width="6%"  
                                       
                                        ImageUrl="~/Images/DeleteRow.png" Text="InActive"   UniqueName="DeleteColumn"  ConfirmText="InActive record?">
                                       
                                       
                                    </telerik:GridButtonColumn>
                                       



                                    </Columns>
                                </MasterTableView>
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                            </telerik:RadGrid>
                            <asp:HiddenField ID="hf1" runat="server" />
                            <asp:HiddenField ID="hdnAssignDiagSampleId" runat="server" />
                        </asp:Panel>
                    </div>
                </div>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel ID="updatepanel6" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                                        Behaviors="Close" ReloadOnShow="true" Skin="Metro">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowPopup" runat="server" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvResultFinal" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="dvProcess" runat="server" AssociatedUpdatePanelID="upd1cvsdgfsdf"
            DisplayAfter="1000" DynamicLayout="true">
            <ProgressTemplate>
                <center>
                    <div style="width: 154px; position: absolute; bottom: 0; height: 60; left: 500px; top: 300px">
                        <img id="Img1" src="~/Images/loading.gif" alt="loading" runat="server" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</asp:Content>
