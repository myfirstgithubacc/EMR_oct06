<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true"
    CodeFile="HousekeepingRequest.aspx.cs" Inherits="WardManagement_HousekeepingRequest" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css' />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        th {
            background: #3498db !important;
            color: white !important;
            font-weight: bold !important;
            white-space: nowrap;
        }

        td {
            white-space: nowrap;
        }
        div#ctl00_ContentPlaceHolder1_upd1{
            overflow:hidden;
        }
    </style>
    <script type="text/javascript">
        function MaxLenTxt(textBox, maxLength) {
            if (parseInt(textBox.value.length) >= parseInt(maxLength)) {

                alert("Max characters allowed are " + maxLength);

                textBox.value = textBox.value.substr(0, maxLength);
            }
        }
    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>

            <div class="row" style="background-color: #fff !important;">
                <div class="col-12">
                    <%--<asp:Label ID="lblPatientDetail" runat="server" Text=""></asp:Label>--%>
                    <asplUD:UserDetails ID="asplUD" runat="server" />
                </div>
            </div>
            <div class="container-fluid">
                <div class="row" style="background: #c1e5ef; padding: 6px 6px;">
                    <div class="col-md-4 pt-1" id="tdHeader" runat="server">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Housekeeping Request" Font-Bold="true" />
                    </div>
                    <div class="col-md-5">
                        <asp:Label ID="lblMsg" runat="server" SkinID="label" Font-Bold="true" ForeColor="Green" Text="&nbsp;" />
                    </div>
                    <div class="col-md-3 text-right">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save"
                            OnClick="btnSave_OnClick" />

                        <asp:Button ID="btnClose" runat="server" CssClass="btn btn-primary" Text="Close"
                            OnClientClick="window.close();" />
                    </div>
                </div>
            </div>



            <div class="container-fluid" style="margin-top: 8px;">
                <div class="row">
                    <div class="col-md-12 " id="divWardMenu" runat="server">
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-4 col-6">
                                <div class="row">
                                    <div class="col-md-4 label2">
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:PRegistration, ward %>"></asp:Literal><span style="color: Red">*</span>
                                    </div>
                                    <div class="col-md-8 ">
                                        <telerik:RadComboBox ID="ddlward" runat="server" Filter="Contains" AutoPostBack="True" OnSelectedIndexChanged="ddlward_SelectedIndexChanged" Width="100%"></telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-6">
                                <div class="row">
                                    <div class="col-md-4  label2">
                                        <asp:Literal ID="Literal2" runat="server" Text="Bed No."></asp:Literal><span style="color: Red">*</span>
                                    </div>
                                    <div class="col-md-8 ">
                                        <telerik:RadComboBox ID="ddlBedNo" runat="server" Filter="Contains" AutoPostBack="True" Width="100%"></telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-6">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label1" runat="server" Text="Reason" /><span style="color: Red">*</span>
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlReasonId" runat="server" EmptyMessage="[ Select ]" Width="100%" />
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-4 col-sm-8 col-6">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label2" runat="server" Text="Other Remarks" />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtOtherRemarks" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                            TextMode="MultiLine" Style="min-height: 50px; max-height: 40px; width: 100%; background-color: #fff !important;"
                                            MaxLength="500" onkeyup="return MaxLenTxt(this,500);" onChange="javascript:MaxLenTxt(this, 500);"
                                            Font-Size="10pt" />
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div class="row">
                        </div>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <div class="row">
                    <div class="col-12" style="overflow-x: auto;">
                        <table border="0" width="100%" cellpadding="2" cellspacing="1" align="center">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlgrid" runat="server" Height="390px" Width="100%" BorderWidth="1"
                                        BorderColor="SkyBlue" ScrollBars="Auto">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvEncounter" runat="server" SkinID="gridview" AllowPaging="false"
                                                    PageSize="10" AutoGenerateColumns="False" Width="100%" OnRowCommand="gvEncounter_OnRowCommand"
                                                    OnRowDataBound="gvEncounter_RowDataBound" OnPageIndexChanging="gvEncounter_OnPageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText='Bed No.' HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBedNo" runat="server" SkinID="label" Text='<%#Eval("BedNo") %>' />
                                                                <asp:HiddenField ID="hdnRequestId" runat="server" Value='<%#Eval("RequestId") %>' />
                                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                                                <asp:HiddenField ID="hdnIsAcknowledged" runat="server" Value='<%#Eval("IsAcknowledged") %>' />
                                                                <asp:HiddenField ID="hdnIsClosed" runat="server" Value='<%#Eval("IsClosed") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reason / Remarks">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReason" runat="server" SkinID="label" Text='<%#Eval("Reason") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Requested By" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRequestedBy" runat="server" SkinID="label" Text='<%#Eval("RequestedBy") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Request Date" HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRequestDateTime" runat="server" SkinID="label" Text='<%#Eval("RequestDateTime") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Theatre Name" HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTheatreName" runat="server" SkinID="label" Text='<%#Eval("TheatreName") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to cancel this request"
                                                                    CommandName="REQUESTDELETE" CausesValidation="false" CommandArgument='<%#Eval("RequestId")%>'
                                                                    ImageUrl="~/Images/DeleteRow.png" Height="16px" Width="16px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="gvEncounter" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

