<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DailyInjection.aspx.cs" Inherits="EMR_Immunization_DailyInjection"
    Title="Daily Injection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

        <script type="text/javascript">
            function AutoChange() {
                var txt = $get('<%=txtComments.ClientID%>');

                if (txt.value.length > 100) {
                    alert("Text length should not be greater then 100.");
                    txt.value = txt.value.substring(0, 100);
                    txt.focus();

                }

            }
            function Cancelremarks() {
                var txt = $get('<%=txtCancelremarks.ClientID%>');

                if (txt.value.length > 100) {
                    alert("Text length should not be greater then 100.");
                    txt.value = txt.value.substring(0, 100);
                    txt.focus();
                }
            }
        </script>

    </telerik:RadScriptBlock>
    

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-3 col-sm-3 col-xs-3">
                    <h2>Daily Injection</h2>
                </div>
                
                <div class="col-md-9 col-sm-9 col-xs-9 text-right">
                    <asp:Button ID="btnNew" runat="server" ToolTip="Clear Data" CssClass="btn btn-primary" Text="New"
                        CausesValidation="false" OnClick="btnNew_Click" />
                    <asp:Button ID="btnSave" runat="server" ToolTip="Save" CssClass="btn btn-primary" Text="Save"
                        ValidationGroup="Save" OnClick="btnSave_Click" />
                </div>
            </div>

            <div class="row text-center">
                    <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" Font-Bold="True" style="position:relative;margin:0px;width:100%;" />
                </div>
            
                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                        <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                            <asp:Label ID="Literal2" runat="server" Text="Injection" SkinID="label" />
                            <span id="span1" runat="server" style="color: Red;">*</span>
                        </div>
                        <div class="col-md-10 col-sm-10 col-xs-9">
                            <telerik:RadComboBox ID="ddlImmunizationName" runat="server" EmptyMessage="Select"
                                AutoPostBack="true" Filter="Contains" EnableLoadOnDemand="true" Width="100%" Height="300px"
                                DropDownWidth="300px" OnSelectedIndexChanged="ddlImmunizationName_OnSelectedIndexChanged" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlImmunizationName"
                                SetFocusOnError="true" Display="None" ErrorMessage="Please Select Injection Name !"
                                InitialValue="" ValidationGroup="Save" />
                        </div>
                    </div>
                        </div>
                     <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                        <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                            <asp:Label ID="ltrlGivenBy" runat="server" Text="Given By" SkinID="label" />
                            <span id="span2" runat="server" style="color: Red;">*</span>
                        </div>
                        <div class="col-md-9 col-sm-9 col-xs-9">
                            <telerik:RadComboBox ID="ddlProviders" runat="server" EmptyMessage="Select" Width="100%"
                                Filter="Contains" EnableLoadOnDemand="true" DropDownWidth="250px" Height="300px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldVal4" runat="server" ControlToValidate="ddlProviders"
                                SetFocusOnError="true" Display="None" ErrorMessage="Please Select Given By !"
                                InitialValue="" ValidationGroup="Save" />
                        </div>
                    </div>
                    </div>
                   <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                        <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                            <asp:Label ID="Literal1" runat="server" Text="Brand" SkinID="label" Visible="false" />
                            <asp:Label ID="ltrlGivenDate" runat="server" Text="Given Date" SkinID="label" />
                            <span id="span3" runat="server" style="color: Red;">*</span>
                        </div>
                        <div class="col-md-9 col-sm-9 col-xs-9">
                            <div class="row">
                                <div class="col-md-8 col-sm-8 col-xs-6">
                                    <telerik:RadComboBox ID="ddlBrand" runat="server" EmptyMessage="Select" Width="100%"
                                Filter="Contains" EnableLoadOnDemand="true" Visible="false" />

                            <telerik:RadDateTimePicker ID="RadGivenDatetime" runat="server" MinDate="01/01/1900"
                                Width="100%" CssClass="inlin-bl1" AutoPostBackControl="Both" DateInput-ReadOnly="true" DateInput-DateFormat="dd/MM/yyyy HH:mm tt" />
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-6 no-p-l">
                                    <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" Height="300px"
                                OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged" Width="45%">
                            </telerik:RadComboBox>
                                    <asp:Literal ID="ltDateTime" runat="server" Text="HH   MM" />
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="RadGivenDatetime"
                                SetFocusOnError="true" Display="None" ErrorMessage="Please Enter Datetime !"
                                ValidationGroup="Save" />
                        </div>
                    </div>
                    </div>
                </div>
                <!-- end row -->

                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                        <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                            <asp:Label ID="ltrlLot" runat="server" Text="BatchNo   " SkinID="label" />
                            <%--  <span id="spanRed" runat="server" style="color: Red;">*</span>--%>
                        </div>
                         <div class="col-md-10 col-sm-10 col-xs-9">
                            <asp:TextBox ID="txtBatchNo" runat="server" SkinID="textbox" MaxLength="20" Width="100%"
                                Style="text-transform: uppercase" />
                            <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBatchNo"
                            SetFocusOnError="true" Display="None" ErrorMessage="Please Enter Batch No !"
                            ValidationGroup="Save" />--%>
                        </div>
                    </div>
                        </div>
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                        <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                            <asp:Label ID="lblExpiryDate" runat="server" Text="Expiry Date" Font-Size="10.5px" SkinID="label"  />
                            <%--<span id="span4" runat="server" style="color: Red;">*</span>--%>
                        </div>
                        <div class="col-md-9 col-sm-9 col-xs-9">
                           <div class="row">
                                <div class="col-md-8 col-sm-8 col-xs-6">
                            <telerik:RadDateTimePicker ID="RadExpiryDate" runat="server" MinDate="01/01/1900"
                                Width="100%" CssClass="inlin-bl1" AutoPostBackControl="Both" DateInput-ReadOnly="true" DateInput-DateFormat="dd/MM/yyyy HH:mm tt" />
                                    </div>
                                <div class="col-md-4 col-sm-4 col-xs-6 no-p-l">
                            <telerik:RadComboBox ID="RadComboBox3" runat="server" AutoPostBack="True" Height="300px"
                                OnSelectedIndexChanged="RadComboBox3_SelectedIndexChanged" Width="45%">
                            </telerik:RadComboBox>

                            <asp:Literal ID="Literal3" runat="server" Text="HH   MM" />
                           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="RadGivenDatetime"
                                SetFocusOnError="true" Display="None" ErrorMessage="Please Enter Expirytime !"
                                ValidationGroup="Save" />--%>
                                    </div>
                        </div>
                    </div>
                        </div>
                        </div>
                   <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                        <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                            <asp:Label ID="Label1" runat="server" Text="Qty" SkinID="label" />
                        </div>
                        <div class="col-md-9 col-sm-9 col-xs-9">
                            <asp:TextBox ID="txtqty" runat="server" SkinID="textbox" MaxLength="30" Width="100%" />
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtqty"
                                ValidChars=".">
                            </AJAX:FilteredTextBoxExtender>
                        </div>
                    </div>
                       </div>
                </div>
                <!-- end row -->

                <div class="row">
                    <div class="col-md-8 col-sm-8 col-xs-12">
                        <div class="row p-t-b-5">
                        <div class="col-md-1 col-sm-1 col-xs-2 text-nowrap">
                            <asp:Label ID="ltrlComments" runat="server" Text="Remarks" SkinID="label" />
                        </div>
                       <div class="col-md-11 col-sm-11 col-xs-10">
                            <asp:TextBox ID="txtComments" runat="server" Height="40px" TextMode="MultiLine"  MaxLength="100" SkinID="textbox"
                                Width="100%" onkeyup="return AutoChange();" />
                            &nbsp;
                        <asp:LinkButton ID="lnkAlerts" Visible="false" runat="server" Text="Patient Alert" OnClick="lnkAlerts_OnClick" />
                        </div>
                    </div>
                        </div>
                     <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                        <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                    <asp:Label ID="Label2" runat="server" Text="Cancel Remarks" SkinID="label" Visible="false" />
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <asp:TextBox ID="txtCancelremarks" runat="server" Height="40px" MaxLength="100" SkinID="textbox"
                                        TextMode="MultiLine" Width="100%" onkeyup="return Cancelremarks();" Visible="false" />

                                    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" ShowMessageBox="true"
                                        ShowSummary="False" ValidationGroup="Save" />
                                </div>
                            </div>
                        </div>
                    </div>

                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:GridView ID="gvDue" CellPadding="0" runat="server" AutoGenerateColumns="false"
                        ShowHeader="true" AllowPaging="false" PagerSettings-Mode="NumericFirstLast" style="width: 100%; overflow: auto;"
                        ShowFooter="false" PagerSettings-Visible="true" HeaderStyle-HorizontalAlign="Left"
                        OnRowCommand="gvDue_RowCommand" OnRowDataBound="gvDue_OnDataBound" OnSelectedIndexChanged="gvDue_SelectedIndexChanged">
                        <HeaderStyle />
                        <Columns>
                            <asp:TemplateField HeaderText="Injection" HeaderStyle-Width="220px" ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label ID="lblImmunizationName" Text='<%#Eval("ImmunizationName")%>' runat="server" />
                                    <asp:HiddenField ID="hdnImmunizationId" Value='<%#Eval("ImmunizationId")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <%-- <asp:TemplateField HeaderText="Brand Name" HeaderStyle-Width="145px" ItemStyle-Width="145px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBrand" runat="server" Text='<%#Eval("ItemBrandName") %>' />
                                    <asp:HiddenField ID="hdnBrandId" runat="server" Value='<%#Eval("BrandId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="Batch No" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBatchno" Text='<%#Eval("LotNo")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width="145px" ItemStyle-Width="145px">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("ExpiryDate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Given By" HeaderStyle-Width="145px" ItemStyle-Width="145px">
                                <ItemTemplate>
                                    <asp:Label ID="lbGivenByName" runat="server" Text='<%#Eval("GivenByName") %>' />
                                    <asp:HiddenField ID="hdnGivenBy" runat="server" Value='<%#Eval("GivenBy") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Given Date" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label ID="lblGivenDate" Text='<%#Eval("GivenDateTime")%>' runat="server" />
                                    <asp:HiddenField ID="hdnId" Value='<%#Eval("Id")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty Given" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lblQtyGiven" Text='<%#Eval("QtyGiven")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRemarks" Style="min-height: 30px; max-height: 30px; min-width: 250px; max-width: 250px;"
                                        Text='<%#Eval("Remarks")%>' Enabled="false" runat="server"
                                        TextMode="MultiLine" />
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                            
                            <asp:CommandField HeaderText='Edit' ControlStyle-ForeColor="Blue" SelectText="Edit"
                                ShowSelectButton="true" ItemStyle-Width="30px">
                                <ControlStyle ForeColor="Blue" />
                            </asp:CommandField>
                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                                        CommandName="DeActivate" CommandArgument='<%#Eval("ID")%>' ToolTip="DeActivate"
                                        ValidationGroup="Cancel" CausesValidation="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                    <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" Skin="Metro" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Maximize">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
                </div>
                 
            <div class="container-fluid">
                <div style="height: 420px; width: 100%; overflow: auto;">
                    
                </div>
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
