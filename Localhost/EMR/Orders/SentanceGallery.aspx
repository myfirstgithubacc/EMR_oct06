<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SentanceGallery.aspx.cs"
    Theme="DefaultControls" Inherits="EMR_Orders_SentanceGallery" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
 <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Include/JS/Functions.js" language="javascript"></script>

 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <telerik:RadCodeBlock ID="radblock" runat="server">

            <script type="text/javascript">
                function SetCursorToTextEnd(textControlID) {
                    var text = document.getElementById(textControlID);
                    if (text != null && text.value.length > 0) {
                        if (text.createTextRange) {
                            var range = text.createTextRange(); range.moveStart('character', text.value.length); range.collapse();
                            range.select();
                        } else if (text.setSelectionRange) {
                            var textLength = text.value.length;
                            text.setSelectionRange(textLength, textLength);
                        }
                    }
                }
            </script>

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
                    if (document.getElementById('<% =txtSentence.ClientID %>').value.length == 0) {
                        document.getElementById('<% =txtSentence.ClientID %>').value = lblValue.replace(/^\s+/, "")
                    }
                    else {
                        document.getElementById('<% =txtSentence.ClientID %>').value = document.getElementById('<% =txtSentence.ClientID %>').value.replace(/^\s+/, "") + ' ' + lblValue.replace(/^\s+/, "");
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
                    //create the argument that will be returned to the parent page
                    var oArg = new Object();

                    //get the city's name
                    oArg.Sentence = document.getElementById("txtSentence").value;
                    oArg.ControlId = document.getElementById("hdControlId").value
                    oArg.ControlType = document.getElementById("hdControlType").value;
                    $get('<%=btnStore.ClientID%>').click();
                    var oWnd = GetRadWindow();
                    oWnd.close(oArg);
                }
            </script>

        </telerik:RadCodeBlock>
        <asp:ScriptManager ID="_ScriptManager" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
               
                    <div class="container-fluid header_main">
                        
                                <div class="col-xs-1">
                                    <h2 style="line-height:23px;">Search</h2>
                                    <asp:HiddenField ID="hdControlId" runat="server" />
                                    <asp:HiddenField ID="hdControlType" runat="server" Value="T" />
                                </div>
                                
                                <div class="col-xs-3 form-group11">
                                    <asp:DropDownList ID="ddlOptions" runat="server">
                                        <asp:ListItem Text="any where" Value="0" />
                                        <asp:ListItem Text="starts with" Value="1" />
                                        <asp:ListItem Text="ends with" Value="2" />
                                    </asp:DropDownList>
                                </div>
                                
                                <div class="col-xs-3 form-group11">
                                    <asp:TextBox ID="txtSearch" SkinID="textbox" Width="100%" runat="server" MaxLength="50"
                                        onkeydown="return keyDown(this,'btnSearch',event);" />
                                </div>
                                
                                <div class="col-xs-4">
                                    <asp:Button ID="btnSearch" Text="Search" runat="server" ToolTip="Search" CssClass="btn btn-primary"
                                        OnClick="btnSearch_Click"/>
                                    <asp:Button ID="btnAddToFavorites" Text="Add to favorites" runat="server" ToolTip="Search"
                                        ssClass="btn btn-primary" OnClick="btnAddToFavorites_Click" Visible="false" />
                                     <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Search" CssClass="btn btn-primary"
                                        OnClientClick="window.close();" />
                                </div>
                        
                    
                    </div><!-- end of container fluid -->
                
                    
                
                
                
                
                
                
                    <div class="container-fluid text-center">
                        <asp:Label ID="lblMessage" Font-Bold="true" runat="server" ForeColor="Green" />
                    </div>
                   
                    
                    
                    <div class="">
                   
                                        <asp:LinkButton ID="lnkShowAll" Text="Show All" OnClick="lnkShowAll_OnClick" Visible="false"
                                            runat="server" />
                                   
                                        <asp:LinkButton ID="lnkShowFavorites" Text="Show Favorites" Visible="false" OnClick="lnkShowFavorites_OnClick"
                                            runat="server" />
                                  
                        
                            <asp:GridView ID="gvSentenceGallery" SkinID="gridview" AllowPaging="true" PageSize="14"
                                AutoGenerateColumns="false" runat="server" OnPageIndexChanging="gvSentenceGallery_PageIndexChanging"
                                 OnRowCommand="gvSentenceGallery_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sentence(s)" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100%">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnSentenceId" Value='<%#Eval("SentenceId")%>' runat="server" />
                                            <asp:TextBox ID="lblSentence" BorderWidth="0px" ReadOnly="true" Text='<%#Eval("Sentence")%>'
                                                BackColor="Transparent" Width="100%" ForeColor="Black" Font-Underline="false"
                                                runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hdnSentenceId" Value='<%#Eval("Id")%>' runat="server" />
                                            <asp:TextBox ID="lblSentence" ReadOnly="false" Text='<%#Eval("Sentence")%>' BackColor="Transparent"
                                                SkinID="textbox" Width="100%" ForeColor="Black" Font-Underline="false" runat="server" />
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Copy" HeaderStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkCopy" Text="Copy" runat="server" CommandName="Copy" ToolTip="Click to copy the Content" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="2%" Visible="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" ForeColor="DodgerBlue" ToolTip="Click to Modify the Content"
                                                Text="Edit" OnClick="lnkEdit_OnClick" runat="server" CommandArgument='<%#Eval("SentenceId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="2%" HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                                                OnClick="ibtnDelete_OnClick" ToolTip="Click to De-Activate" CommandArgument='<%#Eval("SentenceId")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle Width="2%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                      <br />
                           
                                    <strong>Sentence</strong>
                                    <asp:Label ID="lblTitle" runat="server" SkinID="label" ForeColor="Red" Text=" (Maximum character length is 1000.)"></asp:Label>
                                
                                <div class="row">
                                        <div class="col-xs-6 form-group11">
                                              <asp:TextBox ID="txtSentence"  runat="server" />
                                        </div>
                                      
                                      <div class="col-xs-6 text-left">
                                            
                                                    <asp:Button ID="btnAddSentence" Text="Add New Sentence" runat="server" ToolTip="Add New Sentence to the Gallery"
                                                         CssClass="btn btn-primary" Width="125px" OnClick="btnAddSentence_Click" />
                                             
                                             
                                                    <asp:Button ID="BtnCopy" Text="Paste Sentence" runat="server" ToolTip="Paste Sentence to Parent Control on Parent Page"
                                                        CssClass="btn btn-primary" Width="125px" OnClientClick="returnToParent();" />
                            
                            
                                                
                            <asp:Button ID="btnStore" runat="server" OnClick="btnStore_OnClick" Style="visibility: hidden;" />
                             <asp:HiddenField ID="hdnSelectedSentenceId"  runat="server" />
                                      </div>
                                 </div> 
                                  
                                  
                                  
                   
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
