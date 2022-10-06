<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Import.ascx.cs" Inherits="Controls_Import" %>
<p>
    Import files are text files, having one word per line with no leading or trailing
    whitespace.&nbsp; Old TDF files are suitable too.</p>
<span>Pick a file to import:</span>
<input id="importedFile" runat="server" type="file" />
<asp:button id="importButton" runat="server" accesskey="i"  SkinID="Button" text="Import" onclick="importButton_Click" />
<asp:panel id="errorPanel" runat="server" visible="False">
    <asp:label id="errorMessage" runat="server" forecolor="Red"></asp:label>
</asp:panel>
<br />
<br />
<b>Available Dictionaries:</b>
<asp:datagrid id="importedFiles" runat="server" autogeneratecolumns="False" gridlines="none"
    height="40px" onitemcommand="importedFiles_ItemCommand" onitemdatabound="importedFiles_ItemDataBound"
    showheader="False" width="300">
    <columns>
        <asp:templatecolumn>
            <itemtemplate>
                <asp:hyperlink id="fileLink" runat="server" navigateurl="" text='<%# Container.DataItem %>'></asp:hyperlink>
            </itemtemplate>
        </asp:templatecolumn>
        <asp:buttoncolumn commandname="Delete" text="Delete"></asp:buttoncolumn>
    </columns>
</asp:datagrid>