<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="SurgeryEquipmentTagging.aspx.cs" Inherits="OTScheduler_SurgeryEquipmentTagging" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="../Include/EMRStyle.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../Include/css/bootstrap.min.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../Include/css/mainNew.css" media="all" />

    <script type="text/javascript">

        function ddlService_OnClientSelectedIndexChanged(sender, args) {
            $get('<%=btnGetInfoService.ClientID%>').click();
        }

    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row header_main" id="tdHeader" runat="server">
                    <div class="col-md-3 col-sm-3 colxs-12">
                        <h2><asp:Label ID="lblHeader" runat="server" Text="Surgery Equipment Tagging" /></h2>
                    </div>
                    <div class="col-md-9 col-sm-9 col-xs-12 text-center">
                        <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" CssClass="text-center text-success alert_new relativ" />
                    </div>
                </div>
               

            <div class="row">
                <div class="col-md-3 col-sm-3 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap"><asp:Label ID="Label4" runat="server" SkinID="label" Text="Department" /></div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <telerik:RadComboBox ID="ddlDepartment" runat="server" Width="100%" Filter="Contains"
                                MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                            <asp:Label ID="Label2" runat="server" SkinID="label" Text="Sub Department" />
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <telerik:RadComboBox ID="ddlSubDepartment" runat="server" Width="100%" Filter="Contains"
                                MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlSubDepartment_OnSelectedIndexChanged" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                             <asp:Label ID="Label3" runat="server" SkinID="label" Text="Surgery" />
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <telerik:RadComboBox ID="ddlService" runat="server" Width="100%" Height="300px"
                                EmptyMessage="[Type Service Name, Ref Service Name, CPT Code]" AllowCustomText="true" ShowMoreResultsBox="true" EnableLoadOnDemand="true"
                                OnItemsRequested="ddlService_OnItemsRequested" EnableVirtualScrolling="true" DropDownWidth="500px" EnableItemCaching="false"
                                OnClientSelectedIndexChanged="ddlService_OnClientSelectedIndexChanged">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="width: 300px" align="left">Service Name
                                            </td>
                                            <td style="width: 100px" align="left">Ref Service Code
                                            </td>
                                            <td style="width: 100px" align="left">CPT Code
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="width: 300px" align="left">
                                                <%# DataBinder.Eval(Container, "Attributes['ServiceName']")%>
                                            </td>
                                            <td style="width: 100px;" align="left">
                                                <%# DataBinder.Eval(Container, "Attributes['RefServiceCode']")%>
                                            </td>
                                            <td style="width: 100px" align="left">
                                                <%# DataBinder.Eval(Container, "Attributes['CPTCode']")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:RadComboBox>

                            <asp:Button ID="btnGetInfoService" runat="server" Text="" CausesValidation="false" SkinID="button"
                                Style="display: none;" OnClick="btnGetInfoService_OnClick" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                            <asp:Label ID="Label1" runat="server" SkinID="label" Text="Search Equipment " />
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <asp:Panel ID="pnlsearch" runat="server" DefaultButton="btnGetInfoService">
                                <asp:TextBox ID="txtEquipmentSearch" runat="server" MaxLength="30"  Width="100%" />
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                    
            </div>
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <hr style="margin:5px 0px;" />
                    </div>
                </div>

            <div class="row m-b">
                <div class="col-md-6 col-sm-6 col-xs-12 gridview m-t" style="overflow:auto;height:400px;">
                    <asp:GridView ID="gvEquipment" runat="server" CssClass="table table-condensed" Width="100%" AutoGenerateColumns="false"
                        AllowPaging="false" OnRowCommand="gvEquipment_OnRowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="All Equipment List(s)" HeaderStyle-Width="90%" ItemStyle-Width="90%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEquipmentName" runat="server" SkinID="label" Text='<%#Eval("EquipmentName") %>' />
                                    <asp:HiddenField ID="hdnEquipmentId" runat="server" Value='<%#Eval("EquipmentId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnSelect" runat="server" CausesValidation="false"
                                        Text="Select" CommandName="SELECT" ToolTip="Click here to add this record" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12 gridview m-t">
                    <asp:GridView ID="gvEquipmentTagged" runat="server" CssClass="table table-condensed" Width="100%" AutoGenerateColumns="false"
                        AllowPaging="false" OnRowCommand="gvEquipmentTagged_OnRowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Tagged Equipment List(s)">
                                <ItemTemplate>
                                    <asp:Label ID="lblEquipmentName" runat="server" SkinID="label" Text='<%#Eval("EquipmentName") %>' />
                                    <asp:HiddenField ID="hdnEquipmentId" runat="server" Value='<%#Eval("EquipmentId") %>' />
                                    <asp:HiddenField ID="hdnTaggingId" runat="server" Value='<%#Eval("TaggingId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                        CommandName="DEL" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
