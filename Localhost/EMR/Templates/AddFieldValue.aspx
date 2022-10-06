<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddFieldValue.aspx.cs" Inherits="EMR_Templates_AddFieldValue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Field Value</title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../Include/JS/Functions.js" language="javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <telerik:RadCodeBlock ID="radblock" runat="server">

            <script type="text/javascript">
                function clickButton(btnCtl) {
                    $get(btnCtl).click();
                }
                function getCaretPos(textEl2, text) {
                    // alert("sdfasDF");
                    alert(text);
                    textEl = window.opener.document.getElementById(textEl2);
                    alert(textEl.selection());
                    if (textEl.createTextRange && textEl.caretPos) {
                        alert("inside");
                        alert(textEl.selection());
                        //                var caretPos = textEl.caretPos;
                        //                caretPos.text = caretPos.text.charAt(caretPos.text.length - 1) == ' ' ? text + ' ' : text;
                        //                alert(caretPos.text);
                        //                caretPos.select();
                    }
                }


                function CopyToClipboard(Ctl, lblValue) {
                    //                    document.getElementById(Ctl).focus();
                    //                    document.getElementById(Ctl).select();
                    //                    CopiedTxt = document.selection.createRange();
                    //                    CopiedTxt.execCommand("Copy");
                 
                    // This checking is done to avoid the blank space coming in the first location when copying
                    if (document.getElementById('<% =txtValueName.ClientID %>').value.length == 0) {
                        document.getElementById('<% =txtValueName.ClientID %>').value = lblValue.replace(/^\s+/, "")
                    }
                    else {
                        document.getElementById('<% =txtValueName.ClientID %>').value = document.getElementById('<% =txtValueName.ClientID %>').value.replace(/^\s+/, "") + ' ' + lblValue.replace(/^\s+/, "");
                    }

                    return false;
                }
                function SelectHeaderCheckBox(CheckBox, GridName) {
                    var TargetBaseControl = document.getElementById(GridName);
                    var TargetChildControl = "chkInner";
                    var Inputs = TargetBaseControl.getElementsByTagName("input");
                    for (var iCount = 1; iCount < Inputs.length; ++iCount) {
                        if (Inputs[iCount].type == 'checkbox' && Inputs[iCount].id.indexOf(TargetChildControl, 0) >= 0) {
                            Inputs[iCount].checked = CheckBox.checked;
                        }
                    }
                }

                function GetRadWindow() {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    return oWindow;
                }

                function returnToParent() {
                    var oArg = new Object();
                    
                    oArg.Sentence = document.getElementById("hdnValueId").value;
                    oArg.ControlId = document.getElementById("hdControlId").value
                    oArg.ControlType = document.getElementById("hdControlType").value;
                    var oWnd = GetRadWindow();
                    oWnd.close(oArg);
                }
            </script>

        </telerik:RadCodeBlock>
        <asp:ScriptManager ID="_ScriptManager" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <table class="clsheader" bgcolor="#e3dcco" width="98%">
                    <tr>
                        <td>
                            Field Value
                            <asp:HiddenField ID="hdControlId" runat="server" />
                            <asp:HiddenField ID="hdControlType" runat="server" />
                            <asp:HiddenField ID="hdnSectionId" runat="server" />
                            <asp:HiddenField ID="hdnFieldId" runat="server" />
                        </td>
                        <td>
                          <asp:Label ID="lblMessage" Font-Bold="true" runat="server" ForeColor="Green" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Search"
                                SkinID="Button" OnClick="btnclose_OnClick" Width="50px" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" width="98%">
                    <tr>
                        <td width="200px" colspan="2">
                            <asp:Label ID="Label1"  runat="server" SkinID="label" width="60px" Text="Template :"/>
                        
                            <asp:Label ID="lblTemplate" Font-Bold="true" SkinID="label" runat="server"  />
                        </td>
                    </tr>
                     <tr>
                        <td width="200px" colspan="2">
                            <asp:Label ID="Label3"  runat="server" SkinID="label" width="60px" Text="Section :" />
                       
                            <asp:Label ID="lblSection" Font-Bold="true" runat="server" SkinID="label" />
                        </td>
                    </tr>
                     <tr>
                        <td width="200px" colspan="2">
                            <asp:Label ID="Label5"  runat="server" SkinID="label" width="60px" Text="Field :"/>
                        
                            <asp:Label ID="lblField" Font-Bold="true" runat="server" SkinID="label" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                         <asp:Label ID="Label2"  runat="server" SkinID="label" width="50px" Text="Value"/>&nbsp;&nbsp;
                            <asp:TextBox ID="txtValueName" runat="server" Width="250px" SkinID="textbox" />
                        </td>
                        <td valign="top">
                            <asp:Button ID="btnAddValue" Text="Add Value" runat="server" ToolTip="Add New Field Value"
                                SkinID="Button" Width="125px" OnClick="btnAddValue_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvTemplateField" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                runat="server"  OnSelectedIndexChanged="gvTemplateField_SelectedIndexChanged"
                               OnRowUpdating="gvTemplateField_RowUpdating" OnRowEditing="gvTemplateField_RowEditing" 
                               OnRowCancelingEdit="gvTemplateField_RowCancelingEdit" OnRowDataBound="gvTemplateField_OnRowDataBound"  >
                                <Columns>
                                    <asp:TemplateField HeaderText="Value Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="80%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValueName" runat="server" SkinID="label" Text='<%#Eval("ValueName")%>' />
                                            <asp:HiddenField ID="hdnValueId" Value='<%#Eval("ValueId")%>' runat="server" />
                                                <asp:HiddenField ID="hdnFieldId" Value='<%#Eval("FieldId")%>' runat="server" />
                                            <asp:HiddenField ID="hdnFieldType" Value='<%#Eval("FieldType")%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtValueName" runat="server" SkinID="textbox" Text='<%# Eval("ValueName")%>'
                                                ></asp:TextBox>
                                                <asp:HiddenField ID="hdnValueId" Value='<%#Eval("ValueId")%>' runat="server" />
                                                <asp:HiddenField ID="hdnFieldId" Value='<%#Eval("FieldId")%>' runat="server" />
                                            <asp:HiddenField ID="hdnFieldType" Value='<%#Eval("FieldType")%>' runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActive" runat="server" Text='<%# Eval("Active")%>'> </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlActive" SelectedValue='<%#Eval("Active") %>' SkinID="DropDown"
                                                Width="80px" runat="server">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                     <asp:CommandField ShowEditButton="true" HeaderText="Edit" />
                                     
                                    <asp:CommandField ShowSelectButton="true" HeaderText="Select" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                   
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hdnValueId" runat="server" />
        <asp:Button ID="btnStore" runat="server" OnClick="btnStore_OnClick" Style="visibility: hidden;" />
    </div>
    </form>
</body>
</html>
