<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ReferDoctorMaster.aspx.cs" Inherits="MPages_ReferDoctorMaster" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

   <script language="javascript" type="text/javascript">

       function validate() {

           if (document.getElementById('<%=txtName.ClientID%>').value == "") {
               alert("Please enter name !!");
               document.getElementById('<%=txtName.ClientID%>').focus();
               return false
           }
           if (document.getElementById('<%=txtphonehome.ClientID%>').value == "") {
               alert("Please enter phone no. !!");
               document.getElementById('<%=txtphonehome.ClientID%>').focus();
               return false
           }
           
             // if (document.getElementById('<%=chkrelease_unsettled.ClientID%>').checked == false) {
              // alert("Please select the check box !!");
            //   document.getElementById('<%=chkrelease_unsettled.ClientID%>').focus();
            //   return false
           //}
           
       return true 
       }
   
   
   </script>    

    <div>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr class="clsheader" bgcolor="#e3dcco">
                <td>
                </td>
                <td align="right">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                    <asp:Button ID="btnNew" runat="server" SkinID="Button" Text="New" 
                            CausesValidation="false" onclick="btnNew_Click" />
                    <asp:Button ID="btnSave" runat="server" SkinID="Button" Text="Save" ValidationGroup="S"
                      OnClientClick="javascript:return validate()"    OnClick="btnSave_Click" />
                    <asp:Button ID="btnClose" runat="server" SkinID="Button" Text="Close" OnClientClick="window.close();"
                        CausesValidation="false" Visible="false" />
                           </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td colspan="4" align="center" >
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                     <asp:Label ID="lblmsg" runat="server" Font-Bold="true"></asp:Label>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                   
                </td>
            </tr>
            <tr valign="top">
                                    <td colspan="4" valign="top" align="left" style="width: 100%; background-color: White">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr valign="top">
                                                <td>
                                                                    &nbsp;</td>
                                                <td style="width: 30%">
                                                              <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                    <asp:Label ID="Label22" runat="server" SkinID="label" 
                                                        Text="Refer Type"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                         
                                                                        <telerik:RadComboBox ID="ddlReferType" Width="153px" 
                                                        runat="server" CausesValidation="true"   
                                                                            MarkFirstMatch="True" TabIndex="1" ValidationGroup="S" 
                                                                            onselectedindexchanged="ddlReferType_SelectedIndexChanged" 
                                                                            AutoPostBack="True">
                                                                            
                                                                           <%-- <Items>
                                                                                <telerik:RadComboBoxItem runat="server" Text="Doctor" Value="D" />
                                                                                <telerik:RadComboBoxItem runat="server" Text="Hospital" Value="H" />
                                                                                <telerik:RadComboBoxItem runat="server" Text="Camp" Value="C" />
                                                                            </Items>
                                                                            --%>
                                                                        </telerik:RadComboBox>
                                                                        
                                                                   
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                    <asp:Label ID="Label28" runat="server" SkinID="label" 
                                                        Text="Search"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                    <asp:TextBox ID="txtSearchName" runat="server" MaxLength="150" SkinID="textbox" 
                                                                        TabIndex="2" Width="150px" ValidationGroup="g1"></asp:TextBox>
                                                                   
                                                                                    <asp:Button ID="btnFilter" runat="server" CausesValidation="false" 
                                                                                        onclick="btnFilter_Click" SkinID="Button" Text="Filter" />
                                                                   
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                  </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                   
                                                </td>
                                                <td valign="top">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr valign="top">
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td style="width: 30%">
                                                    <asp:Panel ID="pnlDepartmentShow" runat="server" BackColor="White" 
                                                        Height="485px" ScrollBars="Vertical">
                                                        <table width="93%" cellpadding="0" cellspacing="2">
                                                            <tr valign="top">
                                                                <td style="width: 200px;">
                                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvReferDoctor" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                                                        DataKeyNames="id" PageSize="20" HorizontalAlign="Left"
                                                                        Width="98%" ShowFooter="True" OnRowDataBound="gvReferDoctor_RowDataBound" 
                                                                        OnSelectedIndexChanged="gvReferDoctor_SelectedIndexChanged" AllowPaging="True" 
                                                                        onpageindexchanging="gvReferDoctor_PageIndexChanging" 
                                                                        onrowcommand="gvReferDoctor_RowCommand">
                                                                        <Columns>                                                                          
                                                                            <asp:TemplateField HeaderText="Name">
                                                                                
                                                                                <ItemTemplate>
                                                                                  
                                                                                    <asp:LinkButton id="lb"  runat="server" Text='<%# Bind("name") %>' CommandName="view" CommandArgument='<%# Bind("id") %>'></asp:LinkButton>
                                                                                    
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                           
                                                                        </Columns>
                                                                         <%-- <SelectedRowStyle CssClass="selectedRowStyle" BackColor="LightCyan" ForeColor="DarkBlue"  Font-Bold="true" />--%>
                                                                        <RowStyle Height="25px" />
                                                                    </asp:GridView>
                                                                    </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                                <td valign="top">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                    
                                                    <asp:Panel ID="pnlDepartmentEntry" runat="server" Height="500px" ScrollBars="Auto">
                                                        <table width="100%">
                                                            <tr>
                                                                <td colspan="2">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                </td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Name"></asp:Label>
                                                                    <span style="color: red;">&nbsp;*</span> </td>
                                                                <td style="width: 55%">
                                                                    <asp:TextBox ID="txtName" runat="server" MaxLength="150" SkinID="textbox" 
                                                                        TabIndex="2" Width="150px"></asp:TextBox>
                                                                          <asp:HiddenField ID="hdnId" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                </td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label2" runat="server" SkinID="label" Text="Address"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%">
                                                                    <asp:TextBox ID="txtAddress" runat="server" MaxLength="250" SkinID="textbox" 
                                                                        TabIndex="3" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label15" runat="server" SkinID="label" 
                                                                        Text="<%$ Resources:PRegistration, Country%>"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%" align="left">
                                                                    <asp:UpdatePanel ID="UpdatePanel117" runat="server">
                                                                        <ContentTemplate>
                                                                            <telerik:RadComboBox ID="ddlcountry" runat="server" AutoPostBack="true" 
                                                                                MarkFirstMatch="true" OnSelectedIndexChanged="ddlcountry_TextChanged" 
                                                                                TabIndex="4" Width="153px">
                                                                            </telerik:RadComboBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label16" runat="server" SkinID="label" 
                                                                        Text="<%$ Resources:PRegistration, State%>"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%" align="left">
                                                                    <asp:UpdatePanel ID="UpdatePanel118" runat="server">
                                                                        <ContentTemplate>
                                                                            <telerik:RadComboBox ID="ddlstate" runat="server" AutoPostBack="true" 
                                                                                MarkFirstMatch="true" OnSelectedIndexChanged="ddlstate_TextChanged" 
                                                                                TabIndex="5" Width="153px">
                                                                            </telerik:RadComboBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label17" runat="server" SkinID="label" 
                                                                        Text="<%$ Resources:PRegistration, City%>"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%">
                                                                    <asp:UpdatePanel ID="UpdatePanel119" runat="server">
                                                                        <ContentTemplate>
                                                                            <telerik:RadComboBox ID="ddlcity" runat="server" 
                                                                                MarkFirstMatch="true" OnSelectedIndexChanged="ddlcity_TextChanged" 
                                                                                TabIndex="6" Width="153px">
                                                                            </telerik:RadComboBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label18" runat="server" SkinID="label" 
                                                                        Text="<%$ Resources:PRegistration, Pin%>"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%">
                                                                    <asp:TextBox ID="txtZip" runat="server" MaxLength="20" SkinID="textbox" 
                                                                        TabIndex="7" Width="150px" />
                                                                    <ajax:FilteredTextBoxExtender ID="txtZip_FilteredTextBoxExtender" 
                                                                        runat="server" TargetControlID="txtZip" ValidChars="0123456789-+" />
                                                                    <br />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;</td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label23" runat="server" SkinID="label" Text="Home #"></asp:Label>
                                                                    <span style="color: red;">&nbsp;*</span></td>
                                                                <td style="width: 55%">
                                                                    <asp:TextBox ID="txtphonehome" runat="server" MaxLength="20" SkinID="textbox" 
                                                                        TabIndex="8" Width="150px" />
                                                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                                                        TargetControlID="txtphonehome" ValidChars="0123456789-+" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;</td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label20" runat="server" SkinID="label" Text="Mobile #"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%">
                                                                    <asp:UpdatePanel ID="UpdatePanel1111" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtmobile" runat="server" MaxLength="20" SkinID="textbox" 
                                                                                TabIndex="9" Width="150px" />
                                                                            <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                                                                TargetControlID="txtmobile" ValidChars="0123456789-+" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;</td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label24" runat="server" SkinID="label" Text="Fax #"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%">
                                                                    <asp:TextBox ID="txtfax" runat="server" MaxLength="20" SkinID="textbox" 
                                                                        TabIndex="10" Width="150px" />
                                                                    <ajax:FilteredTextBoxExtender ID="txttxtfax_FilteredTextBoxExtender" 
                                                                        runat="server" TargetControlID="txtfax" ValidChars="0123456789-+" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;</td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label21" runat="server" SkinID="label" Text="Email"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%">
                                                                    <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="29" SkinID="textbox" 
                                                                                TabIndex="11" Width="150px" ValidationGroup="S" />
                                                                            <asp:RegularExpressionValidator ID="REV1" runat="server" 
                                                                                ControlToValidate="txtEmail" Display="None" 
                                                                                ErrorMessage="Invalid Email ID Format" SetFocusOnError="true" 
                                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                                                ValidationGroup="S"></asp:RegularExpressionValidator>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;</td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label3" runat="server" SkinID="label" Text="License No"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%">
                                                                    <asp:TextBox ID="txtlicenceno" runat="server" MaxLength="20" SkinID="textbox" 
                                                                        TabIndex="12" Width="150px" />
                                                                </td>
                                                            </tr>
                                                          <%--  <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;</td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label25" runat="server" SkinID="label" Text="Region"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%">
                                                                    <asp:UpdatePanel ID="UpdatePanel1112" runat="server">
                                                                        <ContentTemplate>
                                                                            <telerik:RadComboBox ID="ddlRegion" runat="server" MarkFirstMatch="true" 
                                                                                TabIndex="13" Width="153px">
                                                                            </telerik:RadComboBox>
                                                                            &nbsp;<asp:ImageButton ID="ibtnRegion" runat="server" CausesValidation="false" 
                                                                                ImageUrl="~/Images/PopUp.jpg" OnClick="ibtnRegion_Click" 
                                                                                ToolTip="Add New Region" Visible="true" Width="15px" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>--%>
                                                            <%--<tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;</td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label26" runat="server" SkinID="label" Text="Release Unsettled Bill Fee"></asp:Label>
                                                                    <span style="color: red;">&nbsp;*</span>
                                                                </td>
                                                                <td style="width: 55%">
                                                                    <asp:CheckBox ID="chkrelease_unsettled" Text="" runat="server" 
                                                                        TabIndex="14" ValidationGroup="S" />
                                                                </td>
                                                            </tr>--%>
                                                             <telerik:RadComboBox ID="ddlRegion" runat="server" MarkFirstMatch="true" 
                                                                                TabIndex="13" Width="153px" Visible="false">
                                                                            </telerik:RadComboBox>
                                                            <asp:CheckBox ID="chkrelease_unsettled" Text="" runat="server" 
                                                                        TabIndex="14" ValidationGroup="S" Visible="false" Checked="true" />
                                                            <tr>
                                                                <td style="width: 6%">
                                                                    &nbsp;</td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="Label27" runat="server" SkinID="label" 
                                                                        Text="Active / Inactive"></asp:Label>
                                                                </td>
                                                                <td style="width: 55%">
                                                                    <telerik:RadComboBox ID="ddlActive" runat="server" MarkFirstMatch="True" 
                                                                        TabIndex="1" ValidationGroup="S" Width="153px">
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem runat="server" Selected="True" Text="Active" 
                                                                                Value="1" />
                                                                            <telerik:RadComboBoxItem runat="server" Text="Inactive" Value="0" />
                                                                        </Items>
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <telerik:RadWindowManager ID="RadWindowManager" runat="server" 
                                                                        EnableViewState="false">
                                                                        <Windows>
                                                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                                                            </telerik:RadWindow>
                                                                        </Windows>
                                                                    </telerik:RadWindowManager>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                                        HeaderText="Following Fields are mandatory." ShowMessageBox="True" 
                                                                        ShowSummary="False" ValidationGroup="S" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
            <tr>
                <td width="80px">
                    &nbsp;</td>
                <td>
                   
                </td>
                <td width="80px">
                    &nbsp;</td>
                <td>
                    
                </td>
            </tr>
            </table>
    </div>
</asp:Content>
