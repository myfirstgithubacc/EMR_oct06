<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ICDPanel.ascx.cs" Inherits="Include_Components_ICDPanel" %>

<script type="text/javascript">
    function HideICDPanel() {
        var tt = $get($get("<%=hdnPnl.ClientID %>").value);
       
        tt.style.visibility = 'hidden';
    }

    function HidePanelOKClick() {
        var browserName = navigator.appName;
       
        var tt = $get($get("<%=hdnPnl.ClientID %>").value);
        tt.style.visibility = 'hidden';
        var ICDCodes = '';
        var grid = document.getElementById("<%=rptrICDCodes.ClientID %>");
        var tableElement = grid;//document.getElementById(ctrlCheckBox);

        if (tableElement != null) {
                  
            for (var i = 1; i < tableElement.rows.length; i++) {
                var rowElem = tableElement.rows[i];
                if (browserName == "Netscape" || browserName == "Opera") {
                    var col = rowElem.cells[0].children[0];
                    var chklabel = rowElem.cells[0].children[1];
                }
                else {
                    var col = rowElem.cells[0].childNodes[0];
                    var chklabel = rowElem.cells[0].childNodes[1];
                }
                    
                if (col.checked == true) {
                  
                    if (ICDCodes == '') {
                        if (browserName == "Netscape") {
                            ICDCodes = chklabel.textContent;
                        }
                        else {
                            ICDCodes = chklabel.innerText;
                        }
                    }
                    else {
                        if (browserName == "Netscape") {
                            ICDCodes = ICDCodes + ',' + chklabel.textContent;
                        }
                        else {
                            ICDCodes = ICDCodes + ',' + chklabel.innerText;
                        }
                    }
                }
            }
        }
      
        $get($get("<%=hdnICDTextBox.ClientID %>").value).value = ICDCodes;
        
        tt.style.visibility = 'hidden';
    }
            
</script>
   
<asp:GridView ID="rptrICDCodes" runat="server" AutoGenerateColumns="false" GridLines="None" >
    <Columns>
        <asp:TemplateField HeaderText="ICDCodes" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                 <asp:CheckBox ID="chkICDCodesz" runat="server" Text='<%#Eval("ICDCode") %>' />
                 <asp:HiddenField ID="hdnICDId" runat="server" Value='<%#Eval("ICDID") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
              <%#Eval("ICDDescription")%>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<div style="float: right; width: 70%;">
    <asp:UpdatePanel ID="update" runat="server">
    <ContentTemplate>
        <img id="imgOk" runat="server" alt="Ok" style="cursor: pointer;" src="/Images/Ok.jpg" onclick="HidePanelOKClick()" />
    &nbsp;
    <img id="imgClose" runat="server" alt="Close" style="cursor: pointer;" src="/Images/close.jpg"
        onclick="HidePanelOKClick()" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hdnPnl" runat="server" />
<asp:HiddenField ID="hdnICDTextBox" runat="server" />
</div>