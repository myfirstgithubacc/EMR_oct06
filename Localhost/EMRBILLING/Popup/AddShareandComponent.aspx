<%@ Page Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true"
    CodeFile="AddShareandComponent.aspx.cs" Inherits="EMRBILLING_Popup_AddServices" Title="Surgeon Share Details" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.xmlString = $get('<%=hdnXmlString.ClientID%>').value;
            oArg.ServiceID = $get('<%=hdnServiceID.ClientID%>').value;
            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }



        function clickEnterInGrid(obj, event) {
            var keyCode;
            if (event.keyCode > 0) {
                keyCode = event.keyCode;
            }
            else if (event.which > 0) {
                keyCode = event.which;
            }
            else {
                keycode = event.charCode;
            }
            if (keyCode == 13) {
                document.getElementById(obj).focus();
                return false;
            }
            else {
                return true;
            }
        }
        
        
        
        
        

        function CalculateEditablePrice(txtServiceAmount, txtDoctorAmount, txtUnits, txtDiscountPercent, txtDiscountAmt, txtNetCharge, txtAmountPayableByPatient) {

            var DiscountPerc = parseFloat(document.getElementById(txtDiscountPercent).value);

            if (DiscountPerc > 0) {
                var DiscountAmt = (((DiscountPerc * 1) / 100) * ((document.getElementById(txtUnits).value * 1) * ((totalAmount * 1))).toFixed(2));
            }
            else {
                var DiscountAmt = (0 * 1).toFixed(2);

            }
            var ServiceCharge = document.getElementById(txtServiceAmount).value;
            var DoctorCharge = document.getElementById(txtDoctorAmount).value
            var Units = document.getElementById(txtUnits).value
            var totalAmount = (parseFloat(ServiceCharge) + parseFloat(DoctorCharge)).toFixed(2);

            //            alert(DiscountAmt);
            //            alert(totalAmount);
            //            alert(Units);
            //            alert(ServiceCharge);
            //            alert(DoctorCharge);

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

                case 119:  // F3
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
                              
                                <td align="right" >
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="ibtnSave" runat="server" AccessKey="S" SkinID="Button" Text="Add To Grid"
                                                ToolTip="Save srvices..." ValidationGroup="save" OnClick="ibtSave_OnClick" />
                                            <asp:Button ID="ibtnClose" runat="server" AccessKey="C" SkinID="Button" Text="Close(F8)"
                                                ToolTip="Cancel" OnClientClick="window.close();"  />
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
               <%-- <tr>
                    <td>
                        <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px;
                            border-style: solid none solid none; border-width: 1px; border-color: #808080;"
                            cellpadding="2" cellspacing="2" width="100%">
                            <tr>
                            
                            </tr>
                        </table>
                    </td>
                </tr>--%>
                <tr>
                    <td valign="top" align="left">
                        <table width="100%" cellpadding="0" cellspacing="2" border="0">
                           
                            <tr id="trDoctorShare" runat="server" >
                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>--%>
                                <td width="70px" align="left">
                                  
                                    <asp:Label ID="lblSergeonType" runat="server"   SkinID="label"  Text="Sergeon&nbsp;Type" ></asp:Label>
                                </td>
                                <td align="left" class="style1">
                                 
                                     <telerik:RadComboBox ID="radCmbDoctorClassification" runat="server" MarkFirstMatch="true"  OnSelectedIndexChanged="radCmbDoctorClassification_OnSelectedIndexChanged"  
                                           TabIndex="0"     Width="200px"  Skin="Metro" EmptyMessage="[Select]" AutoPostBack="true" >
                                        </telerik:RadComboBox>     
                                </td>
                                 <td>
                                <asp:Label ID="lblResourceId" runat="server"   Text="Surgeon"  SkinID="label"    ></asp:Label>
                                </td>
                                <td>
                                <telerik:RadComboBox ID="ddlResourceName" runat="server"   OnSelectedIndexChanged="ddlResourceName_OnSelectedIndexChanged"  
                                           TabIndex="0"   Width="200px"  Skin="Metro" EmptyMessage="[Select]" AutoPostBack="true" >
                                        </telerik:RadComboBox>     
                                </td>
                                <td colspan="2">
                                    <table cellpadding="0" cellspacing="1" border="0">
                                        <tr>
                                            <td>
                                              
                                                <asp:Label ID="lblPerc" runat="server"  Text="Perc"></asp:Label> 
                                            </td>
                                            <td>
                                               
                                                  <asp:TextBox  ID="txtPerc"  runat="server" TabIndex="1"    SkinID="textbox" ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                  <asp:Button ID="cmdAddtoGrid" runat="server" OnClick="cmdAddtoGrid_OnClick" SkinID="Button" TabIndex="2"
                                                        Style="visibility: visible;" Text="Select" /></td>
                                <%--</ContentTemplate></asp:UpdatePanel>--%>
                            </tr>
                   
                            <tr>
                                <td colspan="4" style="background-color: #e0ebfd;">
                                    <asp:Panel ID="pnlgvService" runat="server" Width="99%">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                         
                                                
                                                <asp:GridView ID="gvShare"  runat="server"  ShowFooter="true" SkinID="gridview2"   OnRowCommand="gvShare_OnRowCommand"   AutoGenerateColumns="false"  
                                                    Width="100%"   >
                                                   <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                               
                                                                <asp:Label ID="lblID" runat="server"    Text='<%#Eval("SNo") %>' ></asp:Label> 
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                      
                                                       <asp:TemplateField HeaderText="SergeonType" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                
                                                                 <asp:Label ID="lblSergeonType" runat="server"    Text='<%#Eval("SergeonType") %>' ></asp:Label> 
                                                            </ItemTemplate>
                                                        </asp:TemplateField>  
                                                        
                                                         <asp:TemplateField HeaderText="Surgeon Name"   >
                                                            <ItemTemplate>
                                                               
                                                                 <asp:Label ID="lblSurgeonName" runat="server"       Text='<%#Eval("SurgeonName") %>' ></asp:Label>
                                                                 <asp:HiddenField  ID="hdnSurgeonID" runat="server"   Value='<%#Eval("SurgeonID") %>' />
                                                                 
                                                            </ItemTemplate>
                                                        </asp:TemplateField>  
                                                        
                                                   
                                                     <asp:TemplateField HeaderText="PercenTage" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                               
                                                                 <asp:Label ID="lblPerc" runat="server"    Text='<%#Eval("Perc") %>' ></asp:Label> 
                                                            </ItemTemplate>
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
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updDivConfirm" runat="server">
                            <ContentTemplate>
                             
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
     
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
                                <asp:HiddenField ID="hdnServiceID" runat="server" Value="" />
                                <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                                <asp:HiddenField ID="hdnUniqueId" runat="server" Value="" />
                                <asp:HiddenField ID="hdnIsSaved" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                               
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
