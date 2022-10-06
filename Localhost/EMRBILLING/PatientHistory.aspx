<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="PatientHistory.aspx.cs" Inherits="EMRBILLING_PatientHistory" Title="Patient History" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style>
        #ctl00_ContentPlaceHolder1_lblRegno { float:left; margin:0 0 0 15px;}
       .RegText { width:125px !important; float:left !important;}
       .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #8a8d90;border-width: 0 0 1px 1px;color: #333;background: 0 -2300px repeat-x #c1e5ef;}
       .RadGrid_Office2007 .rgFooterDiv, .RadGrid_Office2007 .rgFooter {background: #c1e5ef 0 -6500px repeat-x;border: none;}

       @-moz-document url-prefix() {
            #ctl00_ContentPlaceHolder1_dtpFromDate_wrapper, #ctl00_ContentPlaceHolder1_dtpToDate_wrapper { margin:-3px 0 0 0 !important; float:left;}
        }
    </style>
    
    
        
    <script type="text/javascript">
        function validateMaxLength() {
            var txt = $get('<%=txtRegNo.ClientID%>');
            if (txt.value > 9223372036854775807) {
                alert("Value should not be more than 9223372036854775807.");
                txt.value = txt.value.substring(0, 12);
                txt.focus();
            }
        }
    </script>

    <div style="overflow-y: hidden; overflow-x: hidden;">


    <div class="container-fluid header_main form-group" id="tblMain" runat="server">
        <div class="col-md-3 col-sm-3"><h2><asp:Label ID="lblheader" runat="server" Text="Patient History"></asp:Label></h2></div>
        <div class="col-md-9 col-sm-9 text-center"><asp:Label ID="lblMsg" runat="server" Font-Bold="true" ForeColor="Green" Text=""></asp:Label></div>
    </div>









        <form defaultbutton="BtnSearch">
            <asp:Panel ID="panel1" runat="server" DefaultButton="BtnSearch">

                <div class="row form-group">
                    <div class="col-md-2 col-sm-2">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 label2"><asp:Label ID="lblRegno" runat="server" Text="<%$ Resources:PRegistration, regno%>"></asp:Label></div>
                            <div class="col-md-6 col-sm-6">
                                <asp:TextBox ID="txtRegNo" runat="server" MaxLength="13" Visible="true" onkeyup="return validateMaxLength();"></asp:TextBox>
                                <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-2">
                        <div class="row">
                            <div class="col-md-5 col-sm-5 PaddingRightSpacing label2"><span class="RegText"><asp:Label ID="lblFromdate" Text="From Date" runat="server" /></span></div>
                            <div class="col-md-6 col-sm-7 PaddingRightSpacing"><telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-2">
                        <div class="row">
                            <div class="col-md-5 col-sm-5 PaddingRightSpacing label2"><asp:Label ID="lblTodate" runat="server" Text="To Date" /></div>
                            <div class="col-md-6 col-sm-7 PaddingRightSpacing"><telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                        </div>
                    </div>

                    <div class="col-md-1 col-sm-1"><asp:Button ID="BtnSearch" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="BtnSearch_OnClick" ValidationGroup="aa" /></div>

                    <div class="col-md-5 col-sm-5">
                        <span class="PD-TabRadioNew01 margin_z">
                            <asp:Label ID="lblDetail" runat="server" Font-Bold="true"></asp:Label>
                            <asp:CheckBox ID="chkShowCoveringLetterService" runat="server" Checked="true" SkinID="checkbox" Visible="false" Text="Show Non Invoice Service" />
                        </span>
                    </div>
                </div>
            </asp:Panel>
            

            

        </form>

            
        

        <div class="container-fluid">
            <div class="row">
                <telerik:RadGrid ID="grvPatientHistory" Skin="Office2007" BorderWidth="1" AllowFilteringByColumn="false"
                    AllowMultiRowSelection="true" runat="server" AutoGenerateColumns="False" ShowStatusBar="true"
                    EnableLinqExpressions="false" AlternatingItemStyle-BackColor="Beige" ItemStyle-Font-Size="11px" OnItemDataBound="grvPatientHistory_OnItemDataBound"
                    AlternatingItemStyle-Font-Size="11px" GridLines="Both" Width="100%" AllowSorting="false" ShowFooter="true"
                    Height="500px" OnCustomAggregate="grvPatientHistory_CustomAggregate">
                    <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true"
                        Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                        <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                            AllowColumnResize="false" />
                    </ClientSettings>
                    <MasterTableView AllowFilteringByColumn="false">
                        <NoRecordsTemplate>
                            <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size:11px;">No Record Found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Invoice No" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPrint" runat="server" Text='<%#Eval("InvoiceNo")%>' OnClick="lnkPrint_OnClick"></asp:LinkButton>
                                    <asp:HiddenField ID="hdnInvoiceId" runat="server" Value='<%#Eval("InvoiceId")%>' />
                                    <asp:HiddenField ID="hdnYearId" runat="server" Value='<%#Eval("YearId")%>' />
                                    <asp:HiddenField ID="hdnBillType" runat="server" Value='<%#Eval("BillType")%>' />
                                    <asp:HiddenField ID="hdnOPIP" runat="server" Value='<%#Eval("OPIP")%>' />
                                    <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                    <asp:HiddenField ID="hdnAdmissionDate" runat="server" Value='<%#Eval("AdmissionDate")%>' />
                                    <asp:HiddenField ID="hdninvoiceDate" runat="server" Value='<%#Eval("InvoiceDate")%>' />
                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                    <asp:HiddenField ID="hdnStoreId" runat="server" Value='<%#Eval("StoreId")%>' />
                                    <asp:HiddenField ID="hdnSaleSetupId" runat="server" Value='<%#Eval("DocTypeId")%>' />
                                    <asp:HiddenField ID="hdnSaleType" runat="server" Value='<%#Eval("SaleType")%>' />
                                    <asp:HiddenField ID="hdnIssueId" runat="server" Value='<%#Eval("IssueId")%>' />
                                    <asp:HiddenField ID="hdnRowType" runat="server" Value='<%#Eval("RowType") %>' />
                                    <asp:HiddenField ID="hdnDetectableID" runat="server" Value='<%#Eval("DetectableID") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Type" DataField="OPIP" HeaderStyle-Width="7%" ItemStyle-Width="7%" />
                            <telerik:GridBoundColumn HeaderText="Date" DataField="InvoiceDate" HeaderStyle-Width="8%" ItemStyle-Width="8%"/>
                            <telerik:GridBoundColumn HeaderText="IP No" DataField="EncounterNo"  HeaderStyle-Width="6%" ItemStyle-Width="6%"/>
                            <telerik:GridBoundColumn HeaderText="Adm/Discharge Date" DataField="AdmDiscDate" HeaderStyle-Width="8%" ItemStyle-Width="8%"/>
                            <telerik:GridBoundColumn HeaderText="Invoice Amt" DataField="InvoiceAmount" DataFormatString="{0:f2}" UniqueName="InvoiceAmount"
                                ItemStyle-HorizontalAlign="Right" DataType="System.Double" Aggregate="Sum" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                            <telerik:GridBoundColumn HeaderText="Disc" DataField="DiscountAmount" DataFormatString="{0:f2}"
                                ItemStyle-HorizontalAlign="Right" DataType="System.Double" Aggregate="Sum" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                            <telerik:GridBoundColumn HeaderText="Receipt" DataField="ReceiptAmt" DataFormatString="{0:f2}"
                                ItemStyle-HorizontalAlign="Right" DataType="System.Double" Aggregate="Sum" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                            <telerik:GridBoundColumn HeaderText="Refund" DataField="RefundAmt" DataFormatString="{0:f2}" UniqueName="RefundAmt"
                                ItemStyle-HorizontalAlign="Right" DataType="System.Double" Aggregate="Custom" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                            <telerik:GridBoundColumn HeaderText="Tds" DataField="TdsAmt" DataFormatString="{0:f2}"
                                ItemStyle-HorizontalAlign="Right" DataType="System.Double" Aggregate="Sum" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                            <telerik:GridBoundColumn HeaderText="CrNote" DataField="CreditNoteAmt" DataFormatString="{0:f2}"
                                ItemStyle-HorizontalAlign="Right" DataType="System.Double" Aggregate="Sum" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                            <telerik:GridBoundColumn HeaderText="Balance" DataField="Balance" DataFormatString="{0:f2}"
                                ItemStyle-HorizontalAlign="Right" DataType="System.Double" Aggregate="Sum" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                            <telerik:GridBoundColumn HeaderText="Status" DataField="InvStatus" HeaderStyle-Width="4%" ItemStyle-Width="4%" />
                           <telerik:GridBoundColumn HeaderText="Payer " DataField="PayerName" />
                            <telerik:GridBoundColumn HeaderText="UserName" DataField="UserName" />
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:Button ID="btnMarkDet" runat="server" Text="Mark Detectable" CssClass="btn btn-primary" CommandName='<%#Eval("InvoiceId")%>' OnClick="btnMarkDet_Click" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                    </Windows>
                </telerik:RadWindowManager>
            </div>
        </div>



    <%-- <div style="overflow: scroll; height: 500px">--%>
    
    <div id="dvx" runat="server" visible="false" style="width: 500px; z-index: 200; border: 4px solid #5083af; background-color: #abe1f1; position: absolute; bottom: 0; height: 160px; padding:20px 0 0 0; left: 450px; top: 20em" class="office">
        
        <div class="container-fluid">
            
            <div class="row form-group margin_Top">
                <div class="col-xs-3">Reason:</div>
                <div class="col-xs-9">
                      <telerik:RadComboBox ID="rndreasoninvoicedispatch" runat="server" AutoPostBack="true" DropDownWidth="340px" Width="340px" OnSelectedIndexChanged="rndreasoninvoicedispatch_SelectedIndexChanged"/>
                    <asp:TextBox ID="txtreason" runat="server" TextMode="MultiLine"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="reqreason" runat="server" ControlToValidate="txtreason"
                                                    SetFocusOnError="true"  ErrorMessage="Reason cannot be left blank!!"
                                                    ValidationGroup="vgSave"></asp:RequiredFieldValidator>

                </div>
            </div>
            
            <div class="row">
                <div class="col-xs-3"></div>
                <div class="col-xs-3">
                    <div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="chkactive" runat="server" Checked="true" Text="Active" /></div>
                </div>
                <div class="col-xs-6 text-left">
                    <asp:Button ID="btnupdate" runat="server" Text="Update" OnClick="btnupdate_Click" CssClass="btn btn-primary"  ValidationGroup="vgSave"/>
                    <asp:Button ID="btnclose" runat="server" Text="Close" OnClick="btnclose_Click" CssClass="btn btn-primary" CausesValidation="false" />
                </div>
            </div>
        </div>
        
        
        
        
    </div>
    <%-- </div>--%>

        </div>
</asp:Content>