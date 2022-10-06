<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="HPI.aspx.cs" Inherits="EMR_Problems_HPI"  Title="HPI Details" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.css" rel="stylesheet" />
  
<script type="text/javascript">
function ShowError(sender, args) {
                alert("Enter a Valid Date");
                sender.focus();
            }


            function AutoChangeRelieving() {
                //
                var txt = $get('<%=txtRelievingFactors.ClientID%>');
                //alert(txt.value.length);
                if (txt.value.length > 250) {
                    alert("Text length should not be greater then 250.");

                    txt.value = txt.value.substring(0, 250);
                    txt.focus();
                    //return false;

                }
                //txt.focus();
            }

            function AutoChangeAggrevating() {
                //
                var txt = $get('<%=txtAggrevatingFactors.ClientID%>');
                //alert(txt.value.length);
                if (txt.value.length > 500) {
                    alert("Text length should not be greater then 500.");

                    txt.value = txt.value.substring(0, 500);
                    txt.focus();
                    //return false;

                }
                //txt.focus();
            }
            
            
    </script>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-6"><asp:Label ID="lbl_Msg" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label></div>
            <div class="col-md-12 text-right"><asp:UpdatePanel ID="updSave" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-xs btn-primary" Text="Save" ValidationGroup="Save"
                                        ToolTip="Save" OnClick="btnsave_Click" />
                                </ContentTemplate>                                
                            </asp:UpdatePanel></div>
        </div>
    </div>

    <table width="100%" class="clsheader hidden">
        <tr>
            <td width="20%" valign="top" style="padding-top: 3px">
                HPI Details
            </td>
            <td align="right" style="padding-right: 05px;" valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            
                        </td>
                        <td valign="top">
                           &nbsp; <asp:Button ID="btnBack" Text="Close" SkinID="button" OnClick="btnBack_OnClick" runat="server" OnClientClick="window.close();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
    <table width="100%" cellpadding="2" cellspacing="2" class="table table-condensed table-noborder">
        
        <tr>
            <td class="col-md-2 col-sm-4 col-xs-6">
            <span style="font-weight:bold;">Problem</span>               
            </td>
            <td>
                <telerik:RadComboBox ID="ddlProblems" runat="server" Width="50%" MaxHeight="200px" 
                    AutoPostBack="true" OnSelectedIndexChanged="ddlProblems_OnSelectedIndexChanged">
                </telerik:RadComboBox>
            </td>
        </tr>

       
        <tr>
            <td>
                Onset date
            </td>
            <td>
                <telerik:RadDatePicker ID="dtpOnsetDate" runat="server" MinDate="01/01/1900">
                    <DateInput ID="DateInput1" runat="server">
                        <ClientEvents OnError="ShowError" />
                    </DateInput>
                </telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td>
                Number of occurrences
            </td>
            <td>
                <telerik:RadComboBox ID="ddlNoofOccurances" runat="server" Skin="Outlook" Width="50%" MaxHeight="200px" AppendDataBoundItems="true">
                    <Items>
                        <telerik:RadComboBoxItem Text="" Value="0" Selected="true" />
                        <telerik:RadComboBoxItem Text="1" Value="1" />
                        <telerik:RadComboBoxItem Text="2" Value="2" />
                        <telerik:RadComboBoxItem Text="3" Value="3" />
                        <telerik:RadComboBoxItem Text="4" Value="4" />
                        <telerik:RadComboBoxItem Text="5" Value="5" />
                        <telerik:RadComboBoxItem Text="6" Value="6" />
                        <telerik:RadComboBoxItem Text="7" Value="7" />
                        <telerik:RadComboBoxItem Text="8" Value="8" />
                        <telerik:RadComboBoxItem Text="9" Value="9" />
                        <telerik:RadComboBoxItem Text="10" Value="10" />
                        <telerik:RadComboBoxItem Text="11" Value="11" />
                        <telerik:RadComboBoxItem Text="12" Value="12" />
                        <telerik:RadComboBoxItem Text="13" Value="13" />
                        <telerik:RadComboBoxItem Text="14" Value="14" />
                        <telerik:RadComboBoxItem Text="15" Value="15" />
                        <telerik:RadComboBoxItem Text="16" Value="16" />
                        <telerik:RadComboBoxItem Text="17" Value="17" />
                        <telerik:RadComboBoxItem Text="18" Value="18" />
                        <telerik:RadComboBoxItem Text="19" Value="19" />
                        <telerik:RadComboBoxItem Text="20" Value="20" />
                    </Items>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Prior history of related illness date
            </td>
            <td>
                <telerik:RadDatePicker ID="dtpPriorHistoryDate" runat="server" MinDate="01/01/1900">
                    <DateInput ID="DateInput2" runat="server">
                        <ClientEvents OnError="ShowError" />
                    </DateInput>
                </telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td>
                Relieving factors
            </td>
            <td valign="top">
                <asp:TextBox ID="txtRelievingFactors" onkeydown="return AutoChangeRelieving();" TextMode="MultiLine" runat="server" SkinID="textbox"
                    Width="50%" MaxHeight="40px" MaxLength="500"></asp:TextBox>
                    &nbsp;<span style="color:Red;"> (Maximum character length is 250.)</span>
            </td>
        </tr>
        <tr>
            <td>
                Aggravating factors
            </td>
            <td valign="top">
                <asp:TextBox ID="txtAggrevatingFactors" onkeydown="return AutoChangeAggrevating();" TextMode="MultiLine" runat="server" SkinID="textbox"
                    Width="50%" MaxHeight="40px" MaxLength="500"></asp:TextBox>
                    &nbsp;<span style="color:Red;"> (Maximum character length is 250.)</span>
            </td>
        </tr>
        <tr>
            <td>
                Denies symptoms of 1
            </td>
            <td>
                <telerik:RadComboBox ID="cmbAdd1" runat="server" MaxHeight="200px" Skin="Outlook" Width="50%"
                    EmptyMessage="Search by Text" DataTextField="ProblemDescription" DataValueField="ProblemId"
                    EnableLoadOnDemand="true" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                    EnableVirtualScrolling="true" MaxLength="50" OnItemsRequested="cmb_OnItemsRequested">
                    <HeaderTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    Condition/Symptom
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <%# DataBinder.Eval(Container, "Text")%>
                                </td>
                                <td id="Td1" visible="false" runat="server">
                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Denies symptoms of 2
            </td>
            <td>
                <telerik:RadComboBox ID="cmbAdd2" runat="server" Skin="Outlook" MaxHeight="200px" Width="50%"
                    EmptyMessage="Search by Text" DataTextField="ProblemDescription" DataValueField="ProblemId"
                    EnableLoadOnDemand="true" MaxLength="50" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                    EnableVirtualScrolling="true" OnItemsRequested="cmb_OnItemsRequested">
                    <HeaderTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    Condition/Symptom
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <%# DataBinder.Eval(Container, "Text")%>
                                </td>
                                <td id="Td1" visible="false" runat="server">
                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Denies symptoms of 3
            </td>
            <td>
                <telerik:RadComboBox ID="cmbAdd3" runat="server" Skin="Outlook" MaxHeight="200px" Width="50%"
                    EmptyMessage="Search by Text" DataTextField="ProblemDescription" DataValueField="ProblemId"
                    EnableLoadOnDemand="true" MaxLength="50" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                    EnableVirtualScrolling="true" OnItemsRequested="cmb_OnItemsRequested">
                    <HeaderTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    Condition/Symptom
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <%# DataBinder.Eval(Container, "Text")%>
                                </td>
                                <td id="Td1" visible="false" runat="server">
                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Denies symptoms of 4
            </td>
            <td>
                <telerik:RadComboBox ID="cmbAdd4" runat="server" Skin="Outlook" MaxHeight="200px" Width="50%"
                    EmptyMessage="Search by Text" DataTextField="ProblemDescription" DataValueField="ProblemId"
                    EnableLoadOnDemand="true" MaxLength="50" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                    EnableVirtualScrolling="true" OnItemsRequested="cmb_OnItemsRequested">
                    <HeaderTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    Condition/Symptom
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <%# DataBinder.Eval(Container, "Text")%>
                                </td>
                                <td id="Td1" visible="false" runat="server">
                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Denies symptoms of 5
            </td>
            <td>
                <telerik:RadComboBox ID="cmbAdd5" runat="server" Skin="Outlook" MaxHeight="200px" Width="50%"
                    EmptyMessage="Search by Text" DataTextField="ProblemDescription" DataValueField="ProblemId"
                    EnableLoadOnDemand="true" MaxLength="50" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                    EnableVirtualScrolling="true" OnItemsRequested="cmb_OnItemsRequested">
                    <HeaderTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    Condition/Symptom
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <%# DataBinder.Eval(Container, "Text")%>
                                </td>
                                <td id="Td1" visible="false" runat="server">
                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
