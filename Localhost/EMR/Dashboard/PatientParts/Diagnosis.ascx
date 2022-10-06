<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Diagnosis.ascx.cs" Inherits="EMR_Dashboard_Parts_Assessment" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc">
    <tr>
        <td style="width: 100%">
            <asp:Panel ID="Panel1" runat="server" ScrollBars="None" Width="100%" Height="90%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
                            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
                        <div id="dvZone1" style="width: 100%">
                            <asp:GridView ID="GDAssessment" runat="server"  ForeColor="#333333"
                                SkinID="gridview" GridLines="Both"  Width="95%" AutoGenerateColumns="false"
                                CellPadding="0" ShowHeader="true" OnRowDataBound="GDAssessment_RowDataBound"
                                >
                                <Columns>
                                    <asp:BoundField HeaderText="ICD Description" DataField="ICDDescription" />
                                    <asp:BoundField HeaderText="Status" DataField="" />
                                    <asp:BoundField HeaderText="Condition1" DataField="DiagnosisCondition1" />
                                    <asp:BoundField HeaderText="Condition2" DataField="DiagnosisCondition2" />
                                    <asp:BoundField HeaderText="Condition3" DataField="DiagnosisCondition3" />
                                    <asp:TemplateField HeaderText="Condition">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCondition" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Date" DataField="EntryDate" />
                                    <asp:BoundField HeaderText="Is Chronic" DataField="ISChronic" />
                                    <asp:BoundField HeaderText="Is Resolved" DataField="ISResolved" />
                                    <asp:BoundField HeaderText="Is Primary" DataField="PrimaryDiagnosis" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td align="left">
           <table border="0" style="height: 17px; width: 100%;" cellpadding="0" cellspacing="0">
                <tr style="background-color: #EFF3FB">
                    <td>
                        <asp:LinkButton ID="lnkAddAssessment" runat="server" Text="Add Diagnosis" Visible="false"  OnClick="lnkDiagnosis_OnClick" ></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:TextBox ID="hdnToDate" runat="server" Style="visibility: hidden; position: absolute;" />
            <asp:TextBox ID="hdnFromDate" runat="server" Style="visibility: hidden; position: absolute;" />
            <asp:TextBox ID="hdnEncounterNumber" runat="server" Visible="false"/>
            <asp:TextBox ID="hdnDateVale" runat="server" Style="visibility: hidden; position: absolute;" />
        </td>
    </tr>
</table>
