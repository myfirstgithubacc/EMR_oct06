<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="BedStatus.aspx.cs" Inherits="ATD_BedStatus" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">

        <script type="text/javascript">
            function showMenu(e, wardno, bedcat, bedno, bedstatus, regno, encno) {

                var menu = $find("<%=RadContextMenu1.ClientID %>");
                var lblwardno = document.getElementById('ctl00_ContentPlaceHolder1_lblwardno');
                var lblbedcetogry = document.getElementById('ctl00_ContentPlaceHolder1_lblbedcetogry');
                var lblbeno = document.getElementById('ctl00_ContentPlaceHolder1_lblbeno');
                var txtBedstatus = document.getElementById('ctl00_ContentPlaceHolder1_txtBedstatus');
                var lblregno = document.getElementById('ctl00_ContentPlaceHolder1_lblregno');
                var lblencno = document.getElementById('ctl00_ContentPlaceHolder1_lblencno');

                lblwardno.value = wardno;
                lblbedcetogry.value = bedcat;
                lblbeno.value = bedno;
                txtBedstatus.value = bedstatus;
                lblregno.value = regno;
                lblencno.value = encno;

                var items = menu.get_items();
                //  alert(bedstatus);
                for (var idx = 0; idx < items.get_count() ; idx++) {

                    var VAL = items.getItem(idx).get_value();

                    if (bedstatus == 'R') {

                        if (VAL == 'AD' || VAL == 'BR') {

                            items.getItem(idx).disable();
                        }
                        else {
                            items.getItem(idx).enable();
                        }
                    }
                    else if (bedstatus == 'V') {
                        if (VAL == 'BRL' || VAL == 'BR' || VAL == 'PD') {

                            items.getItem(idx).disable();
                        }
                        else {
                            items.getItem(idx).enable();
                        }

                    }
                    else if (bedstatus == 'H') {
                        if (VAL == 'AD' || VAL == 'BRL' || VAL == 'BR' || VAL == 'PD') {

                            items.getItem(idx).disable();
                        }
                        else {
                            items.getItem(idx).enable();
                        }

                    }

                }
                menu.show(e);

            }

            function showMenu2(e, wardno, bedcat, bedno, regno, encno) {

                var menu = $find("<%=RadContextMenu2.ClientID %>");
                var lblwardno = document.getElementById('ctl00_ContentPlaceHolder1_lblwardno');
                var lblbedcetogry = document.getElementById('ctl00_ContentPlaceHolder1_lblbedcetogry');
                var lblbeno = document.getElementById('ctl00_ContentPlaceHolder1_lblbeno');
                var lblregno = document.getElementById('ctl00_ContentPlaceHolder1_lblregno');
                var lblencno = document.getElementById('ctl00_ContentPlaceHolder1_lblencno');

                lblwardno.value = wardno;
                lblbedcetogry.value = bedcat;
                lblbeno.value = bedno;
                lblregno.value = regno;
                lblencno.value = encno;

                menu.show(e);

            }

            function OnClientClose(oWnd) {
                $get('<%=btnfind.ClientID%>').click();
            }
            function ddlPatient_OnClientSelectedIndexChanged(sender, args) {
                var item = args.get_item();
                $get('<%=hdnRegId.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=btnfind.ClientID%>').click();
            }

        </script>

    </telerik:RadCodeBlock>
    <div oncontextmenu="return false">
        <asp:UpdatePanel ID="updatepanelmain" runat="server">
            <ContentTemplate>
                <table class="clsheader" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <%--Done By Ujjwal 02April2015 commented <td></td> as Directed by Abhishek Sir Start--%>
                        <%--<td width="200px">
                            &nbsp Bed Status
                        </td>--%>
                        <%--Done By Ujjwal 02April2015 commented <td></td> as Directed by Abhishek Sir End--%>
                        <td>
                            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="Lable1" runat="server" Text="Bed Category" SkinID="label"></asp:Label>
                                    </td>
                                    <td>
                                        <%--Done By Ujjwal 02April2015 changed the width from 100px to 250 px and DropDownWidth from 150px to 250px as Directed by Abhishek Sir Start--%>
                                        <telerik:RadComboBox ID="ddlbedcategory" runat="server" Width="250px" Filter="Contains" DropDownWidth="250px" Height="300px"
                                            Skin="Metro" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlbedcategory_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                        <%--Done By Ujjwal 02April2015 changed the width from 100px to 250 px and DropDownWidth from 150px to 250px as Directed by Abhishek Sir End--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" SkinID="label" Text="Bed Status" />
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlBedStatus" runat="server" MarkFirstMatch="true" Width="100px" DropDownWidth="150px"
                                            Skin="Metro" AutoPostBack="true" OnSelectedIndexChanged="ddlbedstatus_SelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" Value="" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label21" runat="server" SkinID="label" Text="Patient" />
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlPatient" runat="server" Width="200px" Height="400px"
                                            Skin="Metro" EnableLoadOnDemand="true" ZIndex="50000" HighlightTemplatedItems="true"
                                            EmptyMessage="Search by regno, ipno, name..." DropDownWidth="650px" OnItemsRequested="ddlPatient_ItemsRequested"
                                            ShowMoreResultsBox="true" AllowCustomText="true" EnableVirtualScrolling="true"
                                            DataValueField="RegistrationId" DataTextField="PatientName" OnClientSelectedIndexChanged="ddlPatient_OnClientSelectedIndexChanged">
                                            <HeaderTemplate>
                                                <table style="width: 600px" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 80px">
                                                            <asp:Literal ID="Literal1" runat="server" Text='<%$ Resources:PRegistration, Regno%>'></asp:Literal>
                                                        </td>
                                                        <td style="width: 80px">
                                                            <asp:Literal ID="Literal2" runat="server" Text="IP No"></asp:Literal>
                                                        </td>
                                                        <td style="width: 200px">Name
                                                        </td>
                                                        <td style="width: 200px">Father Name
                                                        </td>
                                                        <td style="width: 110px">Age/Gender
                                                        </td>
                                                        <td style="width: 110px">Ward / Bed
                                                        </td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table style="width: 600px" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 80px">
                                                            <%# DataBinder.Eval(Container,"Attributes['Account']" )%>
                                                        </td>
                                                        <td style="width: 80px">
                                                            <%# DataBinder.Eval(Container,"Attributes['EncounterNo']" )%>
                                                        </td>
                                                        <td style="width: 200px;">
                                                            <%# DataBinder.Eval(Container, "Text")%>
                                                        </td>
                                                        <td style="width: 200px;">
                                                            <%# DataBinder.Eval(Container, "Attributes['FatherName']")%>
                                                        </td>
                                                        <td style="width: 110px;">
                                                            <%# DataBinder.Eval(Container, "Attributes['AgeGender']")%>
                                                        </td>
                                                        <td style="width: 110px;">
                                                            <%# DataBinder.Eval(Container, "Attributes['Ward/Bed']")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lbtnShowAll" runat="server" Text="Show all" Font-Bold="false" OnClick="lbtnShowAll_OnClick" ForeColor="DodgerBlue"></asp:LinkButton>
                                        <asp:HiddenField ID="hdnRegId" runat="server" Value="0" />

                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkbedTransferReq" Font-Size="11px" Visible="false" runat="server" Text="Bed Transfer Requests"
                                            OnClick="lnkbedTransferReq_OnClick"></asp:LinkButton>
                                        <asp:Label ID="lblNoOfRequest" Font-Size="11px" Visible="false" runat="server"></asp:Label>
                                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                                            <Windows>
                                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                            </Windows>
                                        </telerik:RadWindowManager>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblWard" runat="server" SkinID="label" Text="Ward&nbsp;Name" />
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlWard" runat="server" Skin="Metro" Width="250px" Height="300px"
                                            MarkFirstMatch="true" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlWard_OnSelectedIndexChanged" />
                                    </td>
                                    <td colspan="4">
                                        <asp:RadioButtonList ID="rbtnlstVIP" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlstVIP_SelectedIndexChanged" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Text="All" Selected="True" />
                                            <asp:ListItem Value="1" Text="VIP" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="updatpanel1" runat="server">
                                <ContentTemplate>
                                    <%--<asp:Panel ID="PanelBeds" runat="server" Height="472px" ScrollBars="auto" Width="100%">--%>
                                    <%--BackColor="Gainsboro"--%>
                                    <asp:GridView ID="grvBedStatus" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvBedStatus_RowDataBound"
                                        GridLines="None" ShowHeader="False" OnSelectedIndexChanged="grvBedStatus_SelectedIndexChanged"
                                        OnRowCommand="grvBedStatus_RowCommand">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="div1" runat="server" style="border-style: solid; border-width: 1px; width: 100px;">
                                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="Imagebutton1" runat="server" OnClick="Imagebutton1_OnClick" />
                                                        <asp:HiddenField ID="hdnregno1" runat="server" />
                                                        <asp:HiddenField ID="hdnencounterno1" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="div2" runat="server" style="border-style: solid; border-width: 1px; width: 100px;">
                                                        <asp:Label ID="Label2" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="Imagebutton2" runat="server" />
                                                        <asp:HiddenField ID="hdnregno2" runat="server" />
                                                        <asp:HiddenField ID="hdnencounterno2" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="div3" runat="server" style="border-style: solid; border-width: 1px; width: 100px;">
                                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="Imagebutton3" runat="server" />
                                                        <asp:HiddenField ID="hdnregno3" runat="server" />
                                                        <asp:HiddenField ID="hdnencounterno3" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="div4" runat="server" style="border-style: solid; border-width: 1px; width: 100px;">
                                                        <asp:Label ID="Label4" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="Imagebutton4" runat="server" />
                                                        <asp:HiddenField ID="hdnregno4" runat="server" />
                                                        <asp:HiddenField ID="hdnencounterno4" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="div5" runat="server" style="border-style: solid; border-width: 1px; width: 100px;">
                                                        <asp:Label ID="Label5" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="Imagebutton5" runat="server" />
                                                        <asp:HiddenField ID="hdnregno5" runat="server" />
                                                        <asp:HiddenField ID="hdnencounterno5" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="div6" runat="server" style="border-style: solid; border-width: 1px; width: 100px;">
                                                        <asp:Label ID="Label6" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="Imagebutton6" runat="server" />
                                                        <asp:HiddenField ID="hdnregno6" runat="server" />
                                                        <asp:HiddenField ID="hdnencounterno6" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="div7" runat="server" style="border-style: solid; border-width: 1px; width: 100px;">
                                                        <asp:Label ID="Label7" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="Imagebutton7" runat="server" />
                                                        <asp:HiddenField ID="hdnregno7" runat="server" />
                                                        <asp:HiddenField ID="hdnencounterno7" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="div8" runat="server" style="border-style: solid; border-width: 1px; width: 100px;">
                                                        <asp:Label ID="Label8" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="Imagebutton8" runat="server" />
                                                        <asp:HiddenField ID="hdnregno8" runat="server" />
                                                        <asp:HiddenField ID="hdnencounterno8" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="div9" runat="server" style="border-style: solid; border-width: 1px; width: 100px;">
                                                        <asp:Label ID="Label9" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="Imagebutton9" runat="server" />
                                                        <asp:HiddenField ID="hdnregno9" runat="server" />
                                                        <asp:HiddenField ID="hdnencounterno9" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="div10" runat="server" style="border-style: solid; border-width: 1px; width: 100px;">
                                                        <asp:Label ID="Label10" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="Imagebutton10" runat="server" />
                                                        <asp:HiddenField ID="hdnregno10" runat="server" />
                                                        <asp:HiddenField ID="hdnencounterno10" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <%-- </asp:Panel>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel3" BorderWidth="0" BorderStyle="Solid" ScrollBars="Auto" runat="server">
                                <asp:Table ID="tblLegend" runat="server" border="0" CellPadding="2" CellSpacing="2">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblVaccantColor" runat="server" BackColor="#007F04" BorderStyle="Solid"
                                                BorderWidth="1px" Height="17px" Width="22px" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblVaccant" runat="server" Text="Vacant" SkinID="label" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblOccupiedColor" runat="server" BackColor="#BA4A00" BorderStyle="Solid"
                                                BorderWidth="1px" Height="17px" Width="22px" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblOccupied" runat="server" Text="Occupied" SkinID="label" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblHouseKeepingColor" runat="server" BackColor="#FFFF02" BorderStyle="Solid"
                                                BorderWidth="1px" Height="17px" Width="22px" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblHouseKeeping" runat="server" Text="House Keeping" SkinID="label" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblDualOccupy" runat="server" BackColor="#6C18B8" BorderStyle="Solid"
                                                BorderWidth="1px" Height="17px" Width="22px" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblRetain" runat="server" Text="Retain" SkinID="label" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblBlockedColor" runat="server" BackColor="#F81C02" BorderStyle="Solid"
                                                BorderWidth="1px" Height="17px" Width="22px" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblBlocked" runat="server" Text="Blocked(0)" SkinID="label" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblUnderRepairColor" runat="server" BackColor="#87CEEB" BorderStyle="Solid"
                                                BorderWidth="1px" Height="17px" Width="22px" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblUnderRepair" runat="server" Text="Under Repair(0)" SkinID="label" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblDischargedApprovedColor" runat="server" BackColor="#00FFFF" BorderStyle="Solid"
                                                BorderWidth="1px" Height="17px" Width="22px" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblDischargedApproved" runat="server" Text="" SkinID="label" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" BackColor="Gainsboro">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadContextMenu ID="RadContextMenu1" runat="server" EnableRoundedCorners="true"
                                            EnableShadows="true" OnItemClick="RadContextMenu1_ItemClick" Width="300px">
                                            <%--<Items>
                                               <telerik:RadMenuItem Text="Admission" Value="AD" />
                                                 <telerik:RadMenuItem Text="Bed Retain" Value="BR" />
                                                <telerik:RadMenuItem Text="Bed  Release" Value="BRL" />
                                            </Items>--%>
                                        </telerik:RadContextMenu>
                                        <telerik:RadContextMenu ID="RadContextMenu2" runat="server" EnableRoundedCorners="true"
                                            EnableShadows="true" OnItemClick="RadContextMenu2_ItemClick">
                                            <%--<Items>
                                                <telerik:RadMenuItem Text="Bed Transfer" Value="BT" />
                                                <telerik:RadMenuItem Text="Doctor Transfer" Value="DT" />
                                                <telerik:RadMenuItem Text="Discharge" Value="DS" Visible="false" />
                                                <telerik:RadMenuItem Text="Bed Retain" Value="BR" />
                                                <telerik:RadMenuItem Text="Admission Update" Value="ADU" />--%>
                                            <%-- <telerik:RadMenuItem Text="Payment" Value="PY" />--%>
                                            <%--<telerik:RadMenuItem Text="Patient Details" Value="PD" runat="server" />
                                            </Items>--%>
                                        </telerik:RadContextMenu>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="RadContextMenu1" />
                                        <asp:AsyncPostBackTrigger ControlID="RadContextMenu2" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnfncall" runat="server" Text="callfn" Style="visibility: hidden"
                                            OnClick="btnfncall_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="lblwardno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="lblbedcetogry" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="lblbeno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="lblregno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="lblencno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBedstatus" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnfind" runat="server" Text="Find" Style="visibility: hidden;" OnClick="btnfind_Click" />
                                        <asp:Button ID="btnfindad" runat="server" Text="Find" Style="visibility: hidden;"
                                            OnClick="btnfindad_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updatemodulepanel" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Panel ID="patientdetailspnl" runat="server" Width="300px" BorderStyle="Solid"
                                BackColor="#E6F2FF" BorderColor="Black">
                                <table cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td colspan="2" height="5px"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="Ward NO" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblwardname" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label19" runat="server" Text="Bed Category" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbedcategryname" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label13" runat="server" Text="Bed No" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbednumber" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRegResource" runat="server" Text="<%$ Resources:PRegistration, Regno%>"
                                                SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblregistrationno" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lable11" runat="server" Text="Ip No." SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblipno" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label11" runat="server" Text="Name" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblpatientname" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Text="Age/Gender" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblagegender" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label20" runat="server" Text="Admission Date" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbladmissiondate" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="Doctor Name" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldoctorname" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text="FacilityName" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblfacilityname" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" Text="Bed Retain" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBedRetain" runat="server" SkinID="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnhide" runat="server" Text="Close" SkinID="Button" CausesValidation="false"
                                                OnClientClick="return false;" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnhide"
                    PopupControlID="patientdetailspnl" CancelControlID="btnhide" DropShadow="true" />
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
