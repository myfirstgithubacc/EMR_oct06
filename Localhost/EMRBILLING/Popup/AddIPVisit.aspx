<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/EMRMaster.master"
    CodeFile="AddIPVisit.aspx.cs" Inherits="EMRBILLING_Popup_AddIPVisit" Title="Doctor Visit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.xmlString = $get('<%=hdnXmlString.ClientID%>').value;
            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function CalculateEditablePrice(txtServiceAmount,  txtUnits, txtDiscountPercent, txtDiscountAmt, txtNetCharge, txtAmountPayableByPatient) {
            
            var DiscountPerc = parseFloat(document.getElementById(txtDiscountPercent).value);

            if (DiscountPerc > 0) {
                var DiscountAmt = (((DiscountPerc * 1) / 100) * ((document.getElementById(txtUnits).value * 1) * ((totalAmount * 1))).toFixed(2));
            }
            else {
                var DiscountAmt = (0 * 1).toFixed(2);

            }
           
            var ServiceCharge = document.getElementById(txtServiceAmount).value;

           // var DoctorCharge = document.getElementById(txtDoctorAmount).value
           
            var Units = document.getElementById(txtUnits).value
            var totalAmount = (parseFloat(ServiceCharge) ).toFixed(2);

                    
            //            alert(totalAmount);
            //            alert(Units);
            //            alert(ServiceCharge);
            //            alert(DoctorCharge);
          //  alert(DiscountAmt);
            var NetAmount = (parseFloat(Units * 1) * parseFloat(totalAmount));
            
            document.getElementById(txtDiscountAmt).value = parseFloat(DiscountAmt).toFixed(2);
            var NetCharge = parseFloat(NetAmount - DiscountAmt);
            document.getElementById(txtNetCharge).value = parseFloat(NetCharge).toFixed(2);
            //            document.getElementById(txtNetCharge).value = ((parseFloat(Units * 1) * parseFloat(totalAmount)) - parseFloat(DiscountAmt)).toFixed(2);
            //            alert(ServiceCharge);
            document.getElementById(txtAmountPayableByPatient).value = parseFloat(ServiceCharge).toFixed(2);
            document.getElementById('btnGetBalance').click();
        }
        function ddlService_OnClientSelectedIndexChanged(sender, args) {
            document.getElementById('cmdAddtoGrid.ClientID').click();
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
        function executeCode(evt) {
            if (evt == null) {
                evt = window.Event;
            }
            var theKey = parseInt(evt.keyCode, 10);
            switch (theKey) {

                case 114:  // F3
                    $get('<%=ibtnSave.ClientID%>').click();
                    break;

                case 119:  // F8
                    $get('<%=ibtnClose.ClientID%>').click();
                    break;


            }
            evt.returnValue = false;
            return false;

        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 379px;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="tblMain" runat="server" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table id="Table1" width="100%" runat="server" border="0" class="clsheader">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblPageType" runat="server" Text="" Font-Bold="true" SkinID="label" />
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td style="width: 330px;" valign="middle" align="right">
                                                <%-- <asp:UpdatePanel ID="upd2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate> --%>
                                                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearchByUHID">
                                                    <%--<asp:ImageButton ID="ibtnFind" ImageUrl="~/Images/Binoculr.ico" runat="server" 
                                            mageAlign="AbsMiddle" />&nbsp;I--%>
                                                    <telerik:RadComboBox ID="ddlSearchOn" runat="server" Width="100px" Skin="Metro">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, UHID%>" Value="0" />
                                                            <telerik:RadComboBoxItem Selected="True" Text="IP No" Value="1" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                    <%-- <asp:DropDownList ID="ddlSearchOn" runat="server" Width="80px" SkinID="DropDown">                                           
                                            <asp:ListItem Selected="True" Text="<%$ Resources:PRegistration, UHID%>" Value="0" />
                                            <asp:ListItem Text="IP No." Value="1" />
                                        </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtRegistrationNo" runat="server" Width="100px" SkinID="textbox"
                                                        MaxLength="9" Style="padding-left: 1px;" />
                                                    <asp:Button ID="btnSearchByUHID" runat="server" SkinID="Button" Text="Search" CausesValidation="false"
                                                        OnClick="btnSearchByUHID_OnClick" Style="visibility: hidden;" Width="1" />
                                                </asp:Panel>
                                                <%-- </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSearchByUHID" />
                                    <asp:PostBackTrigger ControlID="lnkEncounterStatus" />
                                </Triggers>
                            </asp:UpdatePanel> --%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right" style="width: 250px">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnNew" runat="server" SkinID="Button" ToolTip="New (Ctrl)" Text="New"
                                                OnClick="btnNew_OnClick" />
                                            <asp:Button ID="ibtnSave" runat="server" AccessKey="S" SkinID="Button" Text="Save (Ctrl+F3)"
                                                ToolTip="Save srvices..." ValidationGroup="save" OnClick="ibtSave_OnClick" />
                                            <asp:Button ID="ibtnClose" runat="server" AccessKey="C" SkinID="Button" Text="Close(Ctrl+F8)"
                                                ToolTip="Cancel" OnClientClick="window.close();" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Text="" ForeColor="Green"
                                                Visible="true" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px;
                            border-style: solid none solid none; border-width: 1px; border-color: #808080;"
                            cellpadding="2" cellspacing="2" width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReg" runat="server" Text='<%$ Resources:PRegistration, Regno%>'
                                        SkinID="label" Font-Bold="true" />
                                    <asp:Label ID="lblregNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                        Font-Bold="true" />
                                    <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label"
                                        Font-Bold="true" />
                                    <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                        Font-Bold="true" />
                                    <asp:Label ID="Label5" runat="server" Text="DOB:" SkinID="label" Font-Bold="true" />
                                    <asp:Label ID="lblDob" runat="server" Text="" SkinID="label" />
                                    <asp:Label ID="Label4" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true" />
                                    <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label" />
                                    <asp:Label ID="lblInfoEncNo" runat="server" Text="IP No:" SkinID="label" Font-Bold="true" />
                                    <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                        Font-Bold="true" />
                                    <asp:Label ID="lblInfoAdmissionDt" runat="server" Text="Admission Date:" SkinID="label"
                                        Font-Bold="true" />
                                    <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label" />
                                    <asp:HiddenField ID="hdnCompanyId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnInsuranceId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnCardId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnConfirmValue" runat="server" Value="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left">
                        <table width="100%" cellpadding="0" cellspacing="2" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblAdvisingDoctor" runat="server" Text="Doctor" />
                                    <telerik:RadComboBox ID="ddlAdvisingDoctor" runat="server" EmptyMessage="Select" MarkFirstMatch="true" Filter="Contains"
                                        Width="180px" Skin="Metro" OnSelectedIndexChanged="ddladvisingdoctor_OnSelectedIndexChanged"
                                        AutoPostBack="true" />
                                    <%--  <asp:Label ID="Label1" runat="server" SkinID="label" Text="Service" />
                                                       <telerik:RadComboBox runat="server" ID="ddlService" Width="300px" Height="150px"
                                                        EnableLoadOnDemand="true" ZIndex="50000" HighlightTemplatedItems="true" EmptyMessage="Select Service"
                                                        DropDownWidth="470px" OnItemsRequested="ddlService_OnItemsRequested" ShowMoreResultsBox="true"
                                                        EnableVirtualScrolling="true" OnClientSelectedIndexChanged="ddlService_OnClientSelectedIndexChanged">
                                                        <HeaderTemplate>
                                                            <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td style="width: 150px" align="left">
                                                                        Service Name
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                                <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="width: 150px;" align="left">
                                                                            <%# DataBinder.Eval(Container, "Attributes['ServiceName']")%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                        </ItemTemplate>
                                                    </telerik:RadComboBox>
                                                      <asp:Label ID="Label2" runat="server" Text="Order&nbsp;Date" />
                                                       <telerik:RadDateTimePicker ID="dtOrderDate" runat="server" Skin="Metro" />
                                                        <telerik:RadComboBox ID="ddlOrderMinutes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderMinutes_SelectedIndexChanged"
                                                    Width="50px" Skin="Metro" />
                                                     <asp:Button ID="cmdAddtoGrid" runat="server" OnClick="cmdAddtoGrid_OnClick" SkinID="Button"
                                                        Style="visibility: visible;" Text="Add" />
                                                    <asp:HiddenField ID="hdServiceType" runat="server" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" Width="99%" DefaultButton="cmdAddtoGrid">
                                        <table cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Service" />
                                                    <telerik:RadComboBox runat="server" ID="ddlService" Width="300px" Height="150px"
                                                        EnableLoadOnDemand="true" ZIndex="50000" HighlightTemplatedItems="true" EmptyMessage="Select Service"
                                                        DropDownWidth="470px" OnItemsRequested="ddlService_OnItemsRequested" ShowMoreResultsBox="true"
                                                        EnableVirtualScrolling="true" OnClientSelectedIndexChanged="ddlService_OnClientSelectedIndexChanged">
                                                        <HeaderTemplate>
                                                            <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td style="width: 150px" align="left">
                                                                        Service Name
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                                <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="width: 150px;" align="left">
                                                                            <%# DataBinder.Eval(Container, "Attributes['ServiceName']")%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                        </ItemTemplate>
                                                    </telerik:RadComboBox>
                                                </td>
                                                
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text="Visit&nbsp;Date" />
                                                </td>
                                                <td>
                                                    <telerik:RadDateTimePicker ID="dtOrderDate" runat="server" Skin="Metro" />
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="ddlOrderMinutes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderMinutes_SelectedIndexChanged"
                                                        Width="50px" Skin="Metro" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="cmdAddtoGrid" runat="server" OnClick="cmdAddtoGrid_OnClick" SkinID="Button"
                                                        Style="visibility: visible;" Text="Add" />
                                                    <asp:HiddenField ID="hdServiceType" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="1">
                                    </table>
                                </td>
                                <td id="tdAdviserDoctor" runat="server" visible="false">
                                    <table cellpadding="0" cellspacing="1">
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="background-color: #e0ebfd;">
                                    <asp:Panel ID="pnlgvService" runat="server" Width="99%">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvService" TabIndex="3" runat="server" AutoGenerateColumns="False"
                                                    ShowFooter="true" SkinID="gridview2" OnRowDataBound="gvService_RowDataBound"
                                                    Width="100%" OnRowCommand="gvService_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltrId" runat="server" Text='<%#Eval("SNo") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Service" HeaderStyle-Width="240px">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                                <asp:HiddenField ID="hdnDocReq" runat="server" Value='<%#Eval("DoctorRequired")%>' />
                                                                <asp:HiddenField ID="hdnDeptId" runat="server" Value='<%#Eval("DepartmentId")%>' />
                                                                <asp:Literal ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                                <asp:HiddenField ID="hdnlblOrderId" runat="server" Value='<%#Eval("OrderId") %>' />
                                                                <asp:HiddenField ID="hdispackagemain" runat="server" Value='<%#Eval("ispackagemain")%>' />
                                                                <asp:HiddenField ID="hdUnderPackage" runat="server" Value='<%#Eval("underpackage")%>' />
                                                                <asp:HiddenField ID="hdnlblServType" runat="server" Value='<%# Convert.ToString(Eval("ServiceType")) %>' />
                                                                <asp:HiddenField ID="hdnIsPackageService" runat="server" Value='<%# Convert.ToString(Eval("ispackageservice")) %>' />
                                                                <asp:HiddenField ID="hdnIsPackageMain" runat="server" Value='<%# Convert.ToString(Eval("ispackagemain")) %>' />
                                                                <asp:HiddenField ID="hdnChargePercentage" runat="server" Value='<%# Convert.ToString(Eval("ChargePercentage")) %>' />
                                                                <asp:HiddenField ID="hdnPackageId" runat="server" Value='<%# Convert.ToString(Eval("packageid")) %>' />
                                                                <asp:HiddenField ID="hdnIsPriceEditable" runat="server" Value='<%# Convert.ToString(Eval("IsPriceEditable")) %>' />
                                                                <asp:HiddenField ID="hdnServiceDiscountPercentage" runat="server" Value='<%# Convert.ToString(Eval("ServiceDiscountPercentage")) %>' />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblInfoTotal" runat="server" Font-Bold="true" SkinID="label" Width="99%"
                                                                    ForeColor="White" />
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Units" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtUnits" runat="server" MaxLength="4" SkinID="textbox" Text='<%#Eval("Units","{0:f2}") %>'
                                                                    Width="40px" Style="text-align: center;" />
                                                                <ajax:FilteredTextBoxExtender ID="FTBEUnits" runat="server" ValidChars="." FilterType="Custom,Numbers"
                                                                    TargetControlID="txtUnits" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date Time" HeaderStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="lbldatetime" runat="server" Text='<%#Eval("VisitDatetime") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PRegistration, Provider %>" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDoctorID" runat="server" Text='<%#Eval("DoctorID") %>' />
                                                                <telerik:RadComboBox ID="ddlDoctor" runat="server" Filter="Contains" MarkFirstMatch="true"
                                                                    Width="150px" Skin="Metro" Height="250px" DropDownWidth="300px" Enabled="false"
                                                                    BackColor="ControlDarkDark">
                                                                </telerik:RadComboBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Service Amount" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtServiceAmount" runat="server" Enabled="false" SkinID="textbox"
                                                                    Style="text-align: right;" Width="90%" Text='<%#Eval("ServiceAmount","{0:f2}") %>'  />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Doctor Amount" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDoctorAmount" runat="server" Enabled="false" SkinID="textbox"
                                                                    Style="text-align: right;" Width="90%" Text='<%#Eval("DoctorAmount","{0:f2}") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Discount %" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDiscountPercent" runat="server" Enabled="false" SkinID="textbox"
                                                                    Style="text-align: right;" Width="90%" Text='<%#Eval("ServiceDiscountPercentage","{0:f2}") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Discount Amt" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDiscountAmt" runat="server" Enabled="false" SkinID="textbox"
                                                                    Style="text-align: right;" Width="90%" Text='<%#Eval("TotalDiscount","{0:f2}") %>' />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblTotDiscountAmt" runat="server" SkinID="label" ForeColor="White" />
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Net Charge" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtNetCharge" runat="server" SkinID="textbox" Enabled="false" Width="90%"
                                                                    Style="text-align: right;" Text='<%#Eval("NetCharge","{0:f2}") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Patient Payable" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAmountPayableByPatient" runat="server" Enabled="false" SkinID="textbox"
                                                                    Style="text-align: right;" Width="90%" Text='<%#Eval("AmountPayableByPatient","{0:f2}") %>' />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblAmountPayableByPatient" runat="server" SkinID="label" ForeColor="White" />
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Payer Payable" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAmountPayableByPayer" runat="server" Enabled="false" SkinID="textbox"
                                                                    Style="text-align: right;" Width="90%" Text='<%#Eval("AmountPayableByPayer","{0:f2}") %>' />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblAmountPayableByPayer" runat="server" SkinID="label" ForeColor="White" />
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtndaDelete" runat="server" CommandName="Del" CausesValidation="false"
                                                                    CommandArgument='<%#Eval("ServiceId")%>' ToolTip="DeActivate" ImageUrl="~/Images/DeleteRow.png" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                        <tr>
                                            <td colspan="3">
                                                <asp:Panel ID="Panel2" runat="server" Width="99%">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="Gvdetail" TabIndex="7" runat="server" AutoGenerateColumns="False"
                                                                ShowFooter="false" SkinID="gridview2" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="3%">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="ltrId" runat="server" Text='<%#Eval("SNO") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Date" HeaderStyle-Width="12%" ItemStyle-Width="8%">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="lblFromDate" runat="server" Text='<%#Eval("FromDate") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblInfoTotal" runat="server" Font-Bold="true" SkinID="label" Width="99%"
                                                                                ForeColor="White"></asp:Label>
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="lblorderno" runat="server" Text='<%#Eval("OrderNo") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Doctor Name" HeaderStyle-Width="20%" ItemStyle-Width="5%">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="LblDoctorName" runat="server" Text='<%#Eval("DoctorName") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Service Name" HeaderStyle-Width="25%" ItemStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="BedCategory Name" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblbedcatName" runat="server" Text='<%#Eval("bedcategoryname") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblServiceAmount" runat="server" Text='<%#Eval("ServiceAmount") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Disc Amt" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblServiceDisc" runat="server" Text='<%#Eval("ServiceDisc") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="User Name" HeaderStyle-Width="35%" ItemStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblusername" runat="server" Text='<%#Eval("User") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updDivConfirm" runat="server">
                            <ContentTemplate>
                                <div id="dvConfirmPrintingOptions" runat="server" style="width: 400px; z-index: 200;
                                    border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC;
                                    border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute;
                                    v; bottom: 0; height: 140px; left: 270px; top: 200px;">
                                    <table cellspacing="2" cellpadding="2" width="400px">
                                        <tr>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:Label ID="lblSn" runat="server" Text="Service name :" ForeColor="#990066" />
                                            </td>
                                            <td style="width: 70%; text-align: left;">
                                                <asp:Label ID="lblServiceName" runat="server" ForeColor="#990066" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:Label ID="Label3" runat="server" Text="Already posted by :" ForeColor="#990066" />
                                            </td>
                                            <td style="width: 70%; text-align: left;">
                                                <asp:Label ID="lblEnteredBy" runat="server" ForeColor="#990066" />
                                            </td>
                                        </tr>
                                        <tr style="border-bottom-style: solid; border-bottom-width: 1px;">
                                            <td style="width: 30%; text-align: left;">
                                                <asp:Label ID="Label6" runat="server" Text="Posted date :" ForeColor="#990066" />
                                            </td>
                                            <td style="width: 70%; text-align: left;">
                                                <asp:Label ID="lblEnteredOn" runat="server" ForeColor="#990066" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width: 100%; text-align: center;">
                                                <asp:Label ID="lblAlertMsg" runat="server" Font-Size="12px" Text="Do you wish to continue...?"
                                                    ForeColor="#990066" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width: 100%; text-align: center;">
                                                <asp:Button ID="btnYes" runat="server" Text="Proceed" OnClick="btnAlredyExist_OnClick"
                                                    SkinID="Button" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_OnClick"
                                                    SkinID="Button" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divExcludedService" runat="server" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC;
                            border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC;
                            background-color: #FFF8DC; position: absolute; bottom: 0; height: 140px; left: 270px;
                            top: 200px;">
                            <table cellspacing="2" cellpadding="2" width="400px">
                                <tr>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:Label ID="Label7" runat="server" Text="Service name :" ForeColor="#990066" />
                                    </td>
                                    <td style="width: 70%; text-align: left;">
                                        <asp:Label ID="lblExcludedServiceName" runat="server" ForeColor="#990066" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 100%; text-align: center;">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblExcludedService" runat="server" Text="Selected service is excluded for the payer."
                                                        ForeColor="#990066" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label8" runat="server" Text="  Do you wish to continue ? " ForeColor="#990066" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; text-align: center;" colspan="2">
                                        <asp:Button ID="btnExcludedService" runat="server" Text="Proceed" OnClick="btnExcludedService_OnClick"
                                            SkinID="Button" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnExcludedServiceCancel" runat="server" Text="Cancel" OnClick="btnExcludedServiceCancel_OnClick"
                                            SkinID="Button" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="uphidden" runat="server">
                            <ContentTemplate>
                                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                                    Width="1200" Height="600" Left="10" Top="10">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin"
                                            Width="900" Height="600" />
                                    </Windows>
                                </telerik:RadWindowManager>
                                <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                                <asp:HiddenField ID="hdnUniqueId" runat="server" Value="" />
                                <asp:HiddenField ID="hdnIsSaved" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                                <asp:Button ID="btnGetBalance" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                    Width="1" OnClick="btnGetBalance_OnClick" Text="" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
