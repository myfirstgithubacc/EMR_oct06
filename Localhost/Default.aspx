<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <link href="/Include/css/all.min.css" rel="Stylesheet" type="text/css" />
    <style>
        a#ctl00_pd1_lnkChangeFacility {
            padding: 2px 0;
        }

        div#ctl00_Radslidingpane4 {
            left: auto !important;
            width: 98% !important;
        }
    </style>
    <script src="/Include/JS/all.min.js"></script>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <div style="text-align: center; color: Red; font-size: medium;" id="dvMsg" runat="server"></div>

    <div id="dvQuerystatus" runat="server" visible="false">
        <asp:GridView ID="gvQuery" runat="server" Width="100%" SkinID="gridview" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="ID" />
                <asp:BoundField DataField="Query" HeaderText="Query" />
                <asp:BoundField DataField="QueryBy" HeaderText="Query By" />
                <asp:BoundField DataField="Encodeddate" HeaderText="Raised By" />
                <asp:BoundField DataField="Urgency" HeaderText="Is Urgent" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnAct" runat="server" Text="Act" CommandName='<%#Eval("InvoiceID") %>' OnClick="btnAct_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" KeepInScreenBounds="true" />
        </Windows>
    </telerik:RadWindowManager>
    <script type="text/javascript">
   
    </script>
</asp:Content>





