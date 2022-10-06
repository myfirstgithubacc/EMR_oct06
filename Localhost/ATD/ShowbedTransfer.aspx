<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowbedTransfer.aspx.cs"
    Inherits="ATD_ShowbedTransfer" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Bed Transfer Details</title>
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />

    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <style type="text/css">
             
            @media only screen and (min-width: 992px) {
                div#RadWindowWrapper_RadWindowForNew {
                    max-width: 800px;
                }
            }

            @media only screen and (min-width: 576px) and (max-width: 992px) {
                div#RadWindowWrapper_RadWindowForNew {
                    width: 500px;
                    left: 0 !important;
                }
            }

            @media only screen and (min-width: 300px) and (max-width: 576px) {
                div#RadWindowWrapper_RadWindowForNew {
                    width: 350px;
                }
            }

            input#rdoBedReq_1 {
                vertical-align: top;
                margin: 2px 4px;
            }
            input#rdoBedReq_0{
                 vertical-align: top;
                margin: 2px 4px;
            }
         
       div#gvBedtorTransfer{
           overflow-x:auto;
       }
         form#form1{
             overflow-x:hidden;
         }
        </style>
        <script language="javascript" type="text/javascript">

            function OnClientClose(oWnd) {
                oWnd.close();
            }


        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scriptmgr" runat="server">
        </asp:ScriptManager>
        <div class="container-fluid">
            <div class="row header_main">
                <div class="col-md-8 ">
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                </div>
                <div class="col-md-4  text-right">
                    <asp:Button ID="btnfilter" runat="server" Text="Filter" CssClass="btn btn-primary"
                        OnClick="btnfilter_Click" />
                    <asp:Button ID="btnclearfilter" runat="server" Text="Clear Filter" CssClass="btn btn-primary"
                        OnClick="btnclearfilter_Click" />
                    <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                </div>
            </div>
            <div class="row m-t">
                <asp:Panel ID="panel1" runat="server" CssClass="col-lg-12 col-md-12" DefaultButton="btnfilter" ScrollBars="None">
                    <div class="row">
                        <div class="col-lg-3 col-6 ">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap center">
                                    <asp:Label ID="lblregno" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, Regno%>"></asp:Label>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <asp:TextBox ID="txtregno" runat="server" SkinID="textbox" MaxLength="10"></asp:TextBox>
                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                        FilterType="Custom,Numbers" TargetControlID="txtregno" ValidChars="0123456789" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-6 ">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap center">
                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="From Date"></asp:Label>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy">
                                    </telerik:RadDatePicker>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-6 ">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap center">
                                    <asp:Label ID="Label2" runat="server" SkinID="label" Text="To Date"></asp:Label>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy">
                                    </telerik:RadDatePicker>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-6 text-right p-t-b-5 ">
                            <asp:RadioButtonList ID="rdoBedReq" CssClass="radio-align" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Bed Transfer" Value="BT" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Bed Transfer Request" Value="BTR"></asp:ListItem>
                            </asp:RadioButtonList>


                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row m-t">
                <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                   <div class="table-responsive">
                        <telerik:RadGrid ID="gvBedtorTransfer"  runat="server" rendermode="auto" ClientSettings-EnablePostBackOnRowClick="False"
                            Width="100%" Skin="Office2007" AllowSorting="False" AllowMultiRowSelection="False"
                            PageSize="7" AllowFilteringByColumn="false" AllowPaging="false" ShowGroupPanel="false"
                            AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true" GridLines="None" AlternatingItemStyle-BackColor="#E6E6FA">
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="false"
                                Scrolling-UseStaticHeaders="false" Scrolling-SaveScrollPosition="true">
                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <MasterTableView AllowFilteringByColumn="false" >
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red;">
                                        No Record Found.
                                    </div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Transfer Date Time" HeaderStyle-Width="140px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTranferDateTime" runat="server" Text='<%#Eval("TransferDate") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="<%$ Resources:PRegistration, Regno%>" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="IP NO." AllowFiltering="false" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="PatientName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Age/Gender" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGender" runat="server" Text='<%#Eval("Gender") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Form Ward">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFormward" runat="server" Text='<%#Eval("Fromward") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="From Bed">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFrombed" runat="server" Text='<%#Eval("Frombed") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To Ward">
                                        <ItemTemplate>
                                            <asp:Label ID="lblToWard" runat="server" Text='<%#Eval("ToWard") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To Bed">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTobed" runat="server" Text='<%#Eval("Tobed") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                   </div>
                </div>
                <asp:HiddenField ID="hdnregno" runat="server" />
                <asp:HiddenField ID="hdnregId" runat="server" />
                <asp:HiddenField ID="hdnencno" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
