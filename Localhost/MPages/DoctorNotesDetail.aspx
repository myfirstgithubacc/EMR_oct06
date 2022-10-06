<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="DoctorNotesDetail.aspx.cs" Inherits="MPages_DoctorNotesDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script language="javascript" type="text/javascript">
</script>
<asp:UpdatePanel ID="DoctorPreviousNotes" runat="server">
    <ContentTemplate>
         <table cellpadding="0" cellspacing="0" border="0" width="100%">
             <tr>
             <td>
                        <table width="100%" class="clsheader" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="padding-left: 10px; width: 260px;">Doctor Previous Notes
                                </td>
                                <td align="right" style="padding-right: 25px; padding-bottom: 1px;">
                                   
                                </td>
                            </tr>
                        </table>
                    </td>
                 </tr>
                  <tr>
                    <td >
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
             <tr>
                    <td style="width: 50%;">
                         <asp:GridView SkinID="gridview" ID="gvDoctorPreviousDetails" CellPadding="1" CellSpacing="0"
                            runat="server" AutoGenerateColumns="false" ShowHeader="true" 
                            Width="100%" ForeColor="#333333" GridLines="None" PageSize="10" AllowPaging="false"
                            ShowFooter="false" PageIndex="0" PagerSettings-Mode="NumericFirstLast" PagerSettings-Visible="true">
                        <Columns>
                                <asp:BoundField DataField="Notes" HeaderText="Notes" Visible="true"  ReadOnly="true" />
                                <asp:BoundField DataField="Encoded By" HeaderText="Encoded By" ItemStyle-HorizontalAlign="Center" Visible="true" ReadOnly="true" />
                                <asp:BoundField DataField="EncodedDate" HeaderText="Encoded Date" ItemStyle-HorizontalAlign="Center" Visible="true" ReadOnly="true" /> 
                        </Columns>
                    </asp:GridView>

                    </td>
             </tr>

          </table>
   </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>


