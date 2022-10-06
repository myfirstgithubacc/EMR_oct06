<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PaymentMode.ascx.cs" Inherits="Include_Components_PaymentMode" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script language="javascript" type="text/javascript">

    function InitiatePayment(amount) {
        //alert(amount);
        var netAmt = document.getElementById(amount).value
        //alert(netAmt);
        var d = Math.random();
        //alert(d);
        var formattedDate = d;
        //var d = new Date();
        var refno = formattedDate;
        //alert(refno);
        $get('<%=hdnrefno.ClientID%>').value = refno;
        window.location.href = 'PlutusCall:' + refno + '$' + netAmt;
    }

    function iteratePaymentModeGrid() {

        var netAmt = document.getElementById("<%=txtNetBAmount.ClientID%>").value
        var tableElement = $find('<%=grdPaymentMode.ClientID%>');
        var MasterTable = tableElement.get_masterTableView();
        var length = MasterTable.get_dataItems().length;
        var txtBalance = $get('<%=txtBalance.ClientID%>');
        var TotalBalance;


        var balance = 0
        for (var i = 0; i < length; i++) {
            var rowElem = MasterTable.get_dataItems()[i];
            var cellAmount = rowElem.findElement("txtAmount");
            var cellBalance = rowElem.findElement("txtBalance");
            var CardSwipingValue = rowElem.findElement("lblCardSwipingValue");
            var hdnSwipingValue = rowElem.findElement("hdnCardSwipingValue");
            var ddlMode = rowElem.findElement("ddlMode");
            TotalBalance += cellBalance;
            if (i == 0) {                
                balance = (netAmt * 1) - (cellAmount.value * 1);
                cellBalance.value = balance.toFixed(2)
                txtBalance.value = balance;
                CardSwipingValue.innerHTML = CalculateServiceCharges(ddlMode.value, cellAmount.value);
                hdnSwipingValue.value = CardSwipingValue.innerHTML;
            }
            else {
                balance = (balance * 1) - (cellAmount.value * 1);
                cellBalance.value = balance.toFixed(2)
                txtBalance.value = balance;
                CardSwipingValue.innerHTML = CalculateServiceCharges(ddlMode.value, cellAmount.value);
                hdnSwipingValue.value = CardSwipingValue.innerHTML;
            }
        }
        $get('<%=hdnBalance.ClientID%>') = balance;

    }

    function CalculateServiceCharges(Mode, Amount) {
        var hdnDebitCardPercentage = document.getElementById("<%=hdnDebitCardPercentage.ClientID%>").value;
        var hdnCreditCardPercentage = document.getElementById("<%=hdnCreditCardPercentage.ClientID%>").value;

        if (Mode == 3 && hdnCreditCardPercentage > 0) {
            Amount = Math.ceil(parseFloat(Amount) + ((parseFloat(Amount) * parseFloat(hdnCreditCardPercentage)) / parseFloat(100))).toFixed(2);
        }
        else if (Mode == 4 && hdnDebitCardPercentage > 0) {
            Amount = Math.ceil(parseFloat(Amount) + ((parseFloat(Amount) * parseFloat(hdnDebitCardPercentage)) / parseFloat(100))).toFixed(2);
        }
        return Amount.toString() == 'NaN' || Amount.toString() == '' ? '0.00' : parseFloat(Amount).toFixed(0);
    }
    function ReplaceString(txtbox) {
        $get(txtbox).value = $get(txtbox).value.replace(/&/g, " and ");
    }  
</script>

<asp:UpdatePanel ID="UPD" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
         <asp:HiddenField ID="hdnrefno" runat="server" />
        <asp:Panel ID="pnlPaymentDetail" runat="server" Width="99%" DefaultButton="btnAdd">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadGrid ID="grdPaymentMode" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                        OnItemDataBound="grdPaymentMode_ItemDataBound" OnItemCommand="grdPaymentMode_OnItemCommand"
                        ShowFooter="true" Skin="Office2007" Visible="false" Width="100%">
                        <ItemStyle HorizontalAlign="Left" />
                        <MasterTableView TableLayout="Auto" BackColor="#E0EBFD">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Mode" HeaderStyle-Width="12%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="true" CssClass="gridInput"
                                            OnSelectedIndexChanged="ddlMode_OnSelectedIndexChanged" Style="width: 100%;" />
                                        <asp:HiddenField ID="hdnMode" runat="server" Value='<%#Eval("ModeId")%>' />
                                        <asp:HiddenField ID="hdnTypeMappingCode" runat="server" Value="0" />
                                        
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkAddRow" runat="server" Visible="true" Font-Bold="true" Text="Add Row"
                                            OnClick="lnkAddRow_Click"></asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAmount" MaxLength="8" runat="server" CssClass="gridInput" Style="width: 70%; float:left;
                                            text-align: right; text-decoration: none;" Text='<%#Eval("Amount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}")%>' />
                                         <asp:Button ID="btnProcesspayment" runat="server" Style="float:right; margin:0; padding:2px 4px !important;" Text="EDC"   ToolTip="Process Payment To EDC" CssClass="btn btn-primary" Visible="false" />

                                        <asp:HiddenField ID="hdnAmount" runat="server" Value='<%#Eval("Balance","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}")%>' />
                                        <AJAX:FilteredTextBoxExtender ID="FTBEServiceDiscountFooter" runat="server" FilterType="Custom,Numbers"
                                            ValidChars=".1234567890" TargetControlID="txtAmount">
                                        </AJAX:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Balance" HeaderStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBalance" runat="server" CssClass="gridInput" ReadOnly="true"
                                            Style="width: 100%; text-align: right; text-decoration: none;" type="text" Text='<%#Eval("Balance","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%"
                                    HeaderText="Date (dd/MM/YYYY)">
                                    <ItemTemplate>
                                        <telerik:RadDatePicker ID="txtChequeDate" runat="server" Width="100%" Enabled="true"
                                            DateFormat="dd/MM/yyyy" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                        <asp:HiddenField ID="hdnChkDate" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Bank Name" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlBankName" runat="server" CssClass="gridInput" Style="width: 82%;float:left;">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlCreditCard" runat="server" AppendDataBoundItems="true" CssClass="gridInput" Style="width: 82%;float:left;">
                                        </asp:DropDownList>
                                         <asp:Button ID="btnCheckREsponse" runat="server" Text="CR" ToolTip="Check For Response"  Visible="false" Style="float:right; margin:0; padding:2px 4px !important;" CssClass="btn btn-primary" OnClick="btnCheckREsponse_Click"/>
                                        <asp:HiddenField ID="hdnBankID" runat="server" Value='<% #Eval("BankId")%>' />
                                        <asp:HiddenField ID="hdnCreditID" runat="server" Value='<% #Eval("CreditCardId")%>' />
                                        <asp:HiddenField ID="hdnCardSwipingValue" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Beneficiary Name" HeaderStyle-Width="14%">
                                    <ItemTemplate>
                                        <%--  <asp:DropDownList ID="ddlClientBankName" runat="server" CssClass="gridInput" DataSourceID="sqlClientBank"
                                            DataTextField="Name" DataValueField="ID"  Style="width: 100%;">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        </asp:DropDownList>--%>
                                        <asp:DropDownList ID="ddlClientBankName" runat="server" CssClass="gridInput" Style="width: 100%;">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnClientBankId" runat="server" Value='<% #Eval("ClientBankId")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reference No" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="16" CssClass="gridInput"
                                            Text='<%#Eval("ModeNo")%>' Width="100%"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FTBEChequeNo" runat="server" FilterType="Custom,Numbers"
                                            ValidChars="1234567890" TargetControlID="txtChequeNo">
                                        </AJAX:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Description/Card Holder Name" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" CssClass="gridInput"
                                            Text='<%#Eval("Description")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Card No" HeaderStyle-Width="23%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTransactionRefNo" runat="server" MaxLength="16" CssClass="gridInput"
                                            Text='<%#Eval("TransactionRefNo")%>' Style="width: 100%;"></asp:TextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Card Swiping Value" HeaderStyle-Width="20%" UniqueName="CardSwappingValue">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCardSwipingValue" runat="server" CssClass="gridInput" Enabled="false"
                                            Text='<%#Eval("CardSwipingValue","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}")%>' Style="color: #990066; font-weight: bold;" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbtnDelete" runat="server" ImageUrl="/Images/Delete.jpg" CommandName="Delete" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <table style="height: 0px; width: 100%">
                        <tr>
                            <td>
                                <%--Style="visibility: hidden; height: 0px;"--%>
                                <asp:TextBox ID="txtNetBAmount" Text="0.0" Style="visibility: hidden; height: 0px;"
                                    runat="server"></asp:TextBox><%--Style="visibility: hidden; height: 0px;"--%>
                                <asp:HiddenField ID="txtcalculateamt" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBalance" runat="server" Style="visibility: hidden; height: 0px;"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hdnBalance" runat="server" />
                                <asp:HiddenField ID="hdnDebitCardPercentage" runat="server" />
                                <asp:HiddenField ID="hdnCreditCardPercentage" runat="server" />
                                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                            </td>
                            <td>
                                <asp:Button ID="btnAdd" SkinID="Button" runat="server" Style="visibility: hidden;
                                    height: 0px; width: 0px" OnClick="lnkAddRow_Click" Text="Add Row" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
