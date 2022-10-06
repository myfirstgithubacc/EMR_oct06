<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditDictionary.ascx.cs" Inherits="Controls_EditDictionary" %>
<table width="100%">
    <tr>
        <td align="center" 
            style="font-family: Verdana; color: #009933; font-weight: bold" >
<asp:UpdatePanel ID="updpanel" runat="server" UpdateMode="Conditional" >
<ContentTemplate>

<asp:label id="messageLabel" runat="server" width="265px"></asp:label>
</ContentTemplate>
 <Triggers >
        <asp:AsyncPostBackTrigger ControlID="addButton" />
         <asp:AsyncPostBackTrigger ControlID="findButton" />
         <asp:AsyncPostBackTrigger ControlID="deleteButton" />
         </Triggers>
</asp:UpdatePanel>
</td>
</tr>
</table> 
<table>
<tr>
<td>
  <asp:label id="Label3" runat="server" Text="Import wordlist:" SkinID="label" ></asp:label>
</td>
<td>
 <asp:panel id="importPanel" runat="server" class="module" >
               
              <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
                <input id="importedFile" runat="server" name="importedFile" type="file" />&nbsp;
                <asp:button id="importButton" runat="server" SkinID="Button"  onclick="importButton_Click"
                    text="Import" />
                    </ContentTemplate>
                    </asp:UpdatePanel> 
            </asp:panel>
   </td>
</tr>
<tr>
<td>

<asp:Label ID="lable1" runat="server" Text="Select Dictionary" SkinID="label" ></asp:Label>
</td>
<td>
      
  <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
<asp:dropdownlist id="dictionarySelector" runat="server" SkinID="DropDown" 
        Width="150px" >
</asp:dropdownlist><br />
</ContentTemplate> 
<Triggers >
<asp:AsyncPostBackTrigger ControlID="importButton" />
<asp:AsyncPostBackTrigger ControlID="importedFile" />

</Triggers>
</asp:UpdatePanel> 
</td>

</tr>
  <tr>
        <td >
        <asp:label id="Label1" runat="server" Text="Add a word:" SkinID="label" ></asp:label>
        </td>
        <td>
         <asp:panel id="addPanel" runat="server" class="module" >
         <asp:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="Conditional" >
         <ContentTemplate >
         
                <asp:textbox id="addWordBox" runat="server" SkinID="textbox" Width="142px" ></asp:textbox>&nbsp;
                <asp:button id="addButton" runat="server" SkinID="Button"  onclick="addButton_Click"
                    text="Add" />
                    
                    </ContentTemplate>
                          <Triggers >
        
         <asp:AsyncPostBackTrigger ControlID="addButton" />
         </Triggers>
         </asp:UpdatePanel>
         </asp:panel> 
         </td>
         
         </tr>
         <tr>
            <td>
           
                <asp:label id="Label2" runat="server" Text="Find word:" SkinID="label" ></asp:label>
                </td>
                <td>
                 <asp:panel id="searchPanel" runat="server" class="module">
                 <asp:UpdatePanel ID="updatepanel2" runat="server" UpdateMode="Conditional" >
         <ContentTemplate >
                <asp:textbox id="findWordBox" runat="server" SkinID="textbox" Width="142px" ></asp:textbox>&nbsp;
                <asp:button id="findButton" runat="server" SkinID="Button"  onclick="findButton_Click"
                    text="Find" />
                    </ContentTemplate>
                     <Triggers >
        
         <asp:AsyncPostBackTrigger ControlID="findButton" />
         </Triggers>
                    </asp:UpdatePanel>
                    </asp:panel>
                    </td>
                 
            </tr>
            <tr>
                    <td colspan="2">
                     <asp:UpdatePanel ID="updatepanel3" runat="server" UpdateMode="Conditional" >
         <ContentTemplate >
                <p>
                    <asp:listbox id="wordsFound" runat="server" height="164px" selectionmode="Multiple"
                        width="298px"></asp:listbox><br />
                    <asp:button id="deleteButton" runat="server"  SkinID="Button"  onclick="deleteButton_Click"
                        text="Delete selected" width="298" />
                </p>
         </ContentTemplate>
         <Triggers >
         <asp:AsyncPostBackTrigger ControlID="addButton" />
         <asp:AsyncPostBackTrigger ControlID="findButton" />
         <asp:AsyncPostBackTrigger ControlID="deleteButton" />
         <asp:AsyncPostBackTrigger ControlID="dictionarySelector" />
         </Triggers>
         </asp:UpdatePanel> 
        </td>
       
    </tr>
</table>

<table>
  
</table>
