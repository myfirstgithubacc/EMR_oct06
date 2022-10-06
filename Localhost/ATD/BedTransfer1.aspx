<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BedTransfer1.aspx.cs" Inherits="ATD_BedTransfer1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bed Transfer Information</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <%--<link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />--%>
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <%--sanyamtanwar--%>
        <style type="text/css">
            div#grvBedStatus_ctl02_div1 {
                padding: 4px 6px !important;
            }

            #updatpanel1 {
                border: 1px solid #e4e4e4 !important;
            }

            #tdtransfer {
                border: 1px solid #e4e4e4 !important;
            }

            input#txtipno {
                padding: 2px 10px !important;
            }

            input#dtpTransferDate_dateInput {
                height: 27px!important;
            }
            span#dtpTransferDate_dateInput_display{
                padding-top:4px!important
            }
        </style>
         <%--sanyamtanwar--%>
        <script type="text/javascript">
            function showMenu(e, wardno, bedcat, bedno) {
                var menu = $find("<%= RadContextMenu1.ClientID %>");
                var lblwardno = document.getElementById('lblwardno');
                var lblbedcetogry = document.getElementById('lblbedcetogry');
                var lblbeno = document.getElementById('lblbeno');
                lblwardno.value = wardno;
                lblbedcetogry.value = bedcat;
                lblbeno.value = bedno;
                menu.show(e);
            }
            function OnClientClose(oWnd) {
                $get('<%=btnfind.ClientID%>').click();
            }
            function OnClientIsValidPasswordClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();
        }
	
        </script>

        <script type="text/javascript">
            if (window.captureEvents) {
                window.captureEvents(Event.KeyUp);
                window.onkeyup = executeCode;
            }
            else if (window.attachEvent) {
                document.attachEvent('onkeyup', executeCode);
            }

            function executeCode(evt) {
                if (evt == null) {
                    evt = window.event;
                }
                var theKey = parseInt(evt.keyCode, 10);
                switch (theKey) {
                    case 113: //F2
                        $get('<%=btnsave.ClientID%>').click();
                        break;
                    case 119:  // F8
                        $get('<%=btnclose.ClientID%>').click();
                        break;
                }
                evt.returnValue = false;
                return false;
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body oncontextmenu="return false">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="Scriptmanager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="container-fluid">
                        <div class="row header_main">
                            <div class="col-lg-3 col-12">
                                <h2>Bed Transfer Information</h2>
                            </div>
                            <div class="col-lg-6 col-12">
                                <asp:Panel ID="Panelfilter" runat="server" CssClass="row" Width="100%" ScrollBars="None" DefaultButton="btnfilter">
                                    <div class="col-6 ">
                                        <div class="row">
                                            <div class="col-lg-4 col-4">
                                                <asp:Label ID="Literal36" runat="server" Text="<%$ Resources:PRegistration, Regno%>" />
                                            </div>
                                            <div class="col-lg-8 col-8">
                                                <asp:TextBox ID="txtregno" runat="server" MaxLength="10" Columns="10" Width="100%" ReadOnly="true" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="row">
                                            <div class="col-lg-4 col-4">
                                                <asp:Label ID="Label2" runat="server" Text="IP NO." /></div>
                                            <div class="col-lg-8 col-8">
                                                <div class="row">
                                                    <div class="col-lg-8 col-8">
                                                        <asp:TextBox ID="txtipno" runat="server" MaxLength="15" Columns="10" Width="100%" SkinID="textbox" />
                                                    </div>
                                                    <div class="col-lg-4 col-4">
                                                        <asp:Button ID="btnfilter" runat="server" Text="Filter" Width="10px" SkinID="Button" OnClick="btnfilter_Click" CausesValidation="false" Style="visibility: hidden;" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </asp:Panel>
                            </div>
                            <div class="col-lg-3 text-right mt-md-0 mt-2">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="btn btn-default mb-xl-0 mb-lg-2" OnClick="btnew_Click" Visible="false" CausesValidation="false" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" ToolTip="(Ctrl-F2)" CssClass="btn btn-primary mb-xl-0 mb-lg-2" OnClick="btnsave_Click" />
                                <asp:Button ID="btnshowtransfer" runat="server" Text="Show Transfer" CssClass="btn btn-primary mb-xl-0 mb-lg-2"  OnClick="btnshowtransfer_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" ToolTip="(Ctrl+F8)" CssClass="btn btn-primary mb-xl-0 mb-lg-2" OnClientClick="window.close();" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12 col-sm-12 text-center">
                                <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-3 col-6">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-4 center">
                                        <asp:Literal ID="Literal2" runat="server" Text="Facility Name "></asp:Literal>
                                        <%--<span style="color: Red">*</span>--%>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-8">
                                        <asp:DropDownList ID="ddlFacilityName" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="false" OnSelectedIndexChanged="ddlFacilityMater_SelectedIndexChanged" Width="100%"></asp:DropDownList>
                                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="ddlFacilityName" Display="None" ErrorMessage="FacilityMater" MaximumValue="99999" MinimumValue="1" ValidationGroup="Save"></asp:RangeValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-6">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-4 center">
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:PRegistration, ward %>"></asp:Literal><span style="color: Red">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-8">
                                        <asp:DropDownList ID="ddlward" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlward_SelectedIndexChanged" Width="100%"></asp:DropDownList>
                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddlward" Display="None" ErrorMessage="Ward" MaximumValue="99999" MinimumValue="1" ValidationGroup="Save"></asp:RangeValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-6">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-4 text-nowrap center">
                                        <asp:Literal ID="ltrltobedcategory" runat="server" Text="<%$ Resources:PRegistration, bedcategory %>"></asp:Literal><span style="color: Red">*</span></span>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-8">
                                        <asp:DropDownList ID="ddlbedcategory" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlbedcategory_SelectedIndexChanged" Width="100%"></asp:DropDownList>
                                        <asp:RangeValidator ID="rngbedcategory" runat="server" ControlToValidate="ddlbedcategory" Display="None" ErrorMessage="Bed Category" MaximumValue="99999" MinimumValue="1" ValidationGroup="Save"></asp:RangeValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-6">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-4 center">
                                        <asp:Label ID="lblTransferDate" runat="server" Text="Transfer Date"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-8">
                                        <telerik:RadDateTimePicker ID="dtpTransferDate" runat="server" MinDate="01/01/1999"
                                            DateInput-DateFormat="dd/MM/yyyy HH:mm" DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" Width="100%" Enabled="false">
                                        </telerik:RadDateTimePicker>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-lg-3 col-md-6 col-6 p-t-b-5">
                                <asp:Label ID="Label1" runat="server" Text="From Transfer Bed : " Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblward" runat="server" Text="Ward Name -" Font-Bold="True"></asp:Label>
                                <asp:Label ID="txtward" runat="server"></asp:Label>
                            </div>
                            <div class="col-lg-3 col-md-6 col-6   p-t-b-5">
                                <asp:Label ID="lblbedcategory" runat="server" Text="Bed Category Name -" Font-Bold="True"></asp:Label>
                                <asp:Label ID="txtbedcategory" runat="server"></asp:Label>
                            </div>
                            <div class="col-lg-3 col-md-6 col-6  p-t-b-5">
                                <asp:Label ID="lblBillingCat" runat="server" Text="Billing Category -" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblBillingCategory" runat="server"></asp:Label>
                            </div>
                            <div class="col-lg-3 col-md-6 col-6  p-t-b-5">
                                <asp:Label ID="lblbedno" runat="server" Text="Bed No -" Font-Bold="True"></asp:Label>
                                <asp:Label ID="txtbedno" runat="server"></asp:Label>
                                <asp:Label ID="lblDesiredBedCat" runat="server" Text="Desired Bed Category -" Font-Bold="True"></asp:Label>&nbsp;
                                <asp:Label ID="lblDesiredBedCategory" runat="server" SkinID="label"></asp:Label>&nbsp;&nbsp;
                            </div>
                        </div>
                    

                        <div class="row text-center">
                                <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="#008A2D"></asp:Label>
                            </div>


                        <div class="row m-t">
                            <div class="col-lg-9 ">
                                <asp:UpdatePanel ID="updatpanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="PanelBeds" runat="server" Height="450" ScrollBars="Both" Width="100%">
                                            <asp:GridView ID="grvBedStatus" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvBedStatus_RowDataBound" CssClass="box-bed-detail-list"
                                                GridLines="None" ShowHeader="False">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div id="div1" runat="server" class="box-bed-detail">
                                                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                                                <asp:ImageButton ID="Imagebutton1" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div id="div2" runat="server" class="box-bed-detail">
                                                                <asp:Label ID="Label2" runat="server"></asp:Label>
                                                                <asp:ImageButton ID="Imagebutton2" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div id="div3" runat="server" class="box-bed-detail">
                                                                <asp:Label ID="Label3" runat="server"></asp:Label>
                                                                <asp:ImageButton ID="Imagebutton3" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div id="div4" runat="server" class="box-bed-detail">
                                                                <asp:Label ID="Label4" runat="server"></asp:Label>
                                                                <asp:ImageButton ID="Imagebutton4" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div id="div5" runat="server" class="box-bed-detail">
                                                                <asp:Label ID="Label5" runat="server"></asp:Label>
                                                                <asp:ImageButton ID="Imagebutton5" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div id="div6" runat="server" class="box-bed-detail">
                                                                <asp:Label ID="Label6" runat="server"></asp:Label>
                                                                <asp:ImageButton ID="Imagebutton6" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div id="div7" runat="server" class="box-bed-detail">
                                                                <asp:Label ID="Label7" runat="server"></asp:Label>
                                                                <asp:ImageButton ID="Imagebutton7" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <telerik:RadContextMenu ID="RadContextMenu1" runat="server" EnableRoundedCorners="true"
                                            EnableShadows="true" OnItemClick="RadContextMenu1_ItemClick">
                                            <Items>
                                                <telerik:RadMenuItem Text="Transfer" Value="BT" />
                                            </Items>
                                        </telerik:RadContextMenu>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="RadContextMenu1" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>

                            <div id="tdtransfer" runat="server" class="col-lg-3 col-md-12">

                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="background: #e4e4e4; padding: 3px 10px;">
                                        To Bed Detail
                                    </div>
                                </div>


                                <div class="row p-t-b-5">
                                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="ltrltoward" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, ward %>"></asp:Label>
                                            </div>
                                    <div class="col-lg-8 col-md-8 col-sm-8 col-xs-8">
                                                <asp:Label ID="txttowardname" runat="server" SkinID="label" Width="95%"></asp:Label>
                                                <asp:Label ID="txttoward" runat="server" Style="visibility: hidden; width: 10px;"></asp:Label>
                                            </div>
                                </div>
                            
                                <div class="row p-t-b-5">
                                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label8" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, bedcategory %>"></asp:Label>
                                           </div>
                                    <div class="col-lg-8 col-md-8 col-sm-8 col-xs-8">
                                                <asp:Label ID="txttobedcategoryname" runat="server" SkinID="label" Width="95%"></asp:Label>
                                                <asp:Label ID="txttobedcategory" runat="server" Style="visibility: hidden; width: 10px;"></asp:Label>
                                             </div>
                                </div>
                           
                            
                                <div class="row p-t-b-5">
                                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="ltrltobedno" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, bedno %>"></asp:Label>
                                            </div>
                                    <div class="col-lg-8 col-md-8 col-sm-8 col-xs-8">
                                                <asp:Label ID="txttobedname" runat="server" SkinID="label" Width="95%"></asp:Label>
                                                <asp:Label ID="txttobedno" runat="server" Style="visibility: hidden; width: 10px;"></asp:Label>
                                            </div>
                                </div>
                            
                           
                                <div class="row p-t-b-5">
                                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="ltrltobillingcategory" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, billingcategory %>"></asp:Label>
                                            </div>
                                    <div class="col-lg-8 col-md-8 col-sm-8 col-xs-8">
                                                <telerik:RadComboBox ID="ddltobillingcategoryname" Width="95%" runat="server" TabIndex="7" />
                                                </div>
                                </div>
                           
                       
                        <div class="row p-t-b-5">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:Label ID="ltrRemarks" runat="server" SkinID="label" Text="Remarks"></asp:Label>
                                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Width="95%"
                                                    SkinID="textbox"></asp:TextBox>
                                             </div>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:CheckBox ID="chkIsBedRetail" runat="server" Text="Retain Old Bed" TextAlign="Right" />
                                            </div>
                        </div>
                                </div>
                            </div>

                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtwardid" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                                        <asp:TextBox ID="lblwardno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtbedcategoryid" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                                        <asp:TextBox ID="lblbedcetogry" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                                        <asp:TextBox ID="txtCurrentBedCategoryid" runat="server" Style="visibility: hidden;" />
                                                        <asp:TextBox ID="txtCurrentBillCategoryid" runat="server" Style="visibility: hidden;" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtbedid" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                                        <asp:TextBox ID="lblbeno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="lblregno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lblencno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnfind" runat="server" Text="Find" Style="visibility: hidden;" OnClick="btnfind_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hdnregno" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" Skin="Office2007" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close" Skin="Office2007" InitialBehaviors="Maximize">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>
                            </div>
                        </div>
                    

                    <div id="dvConfirm" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000; border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute; bottom: 0; height: 75px; left: 300px; top: 150px">
                        <table width="100%" cellspacing="2">
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Label ID="lblConfirm" Font-Size="12px" runat="server" Font-Bold="true" Text="Bed category and billing category are different. Do you want to save ?"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center"></td>
                                <td align="center">
                                    <asp:Button ID="btnYes" SkinID="Button" runat="server" Text="Yes" OnClick="btnYes_OnClick" />
                                    &nbsp;
                                <asp:Button ID="btnCancel" SkinID="Button" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" />
                                </td>
                                <td align="center"></td>
                            </tr>
                        </table>
                    </div>
                        </div>
                </ContentTemplate>

            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server" ID="up1">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" InitialBehaviors="Maximize">
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                </ContentTemplate>
                
            </asp:UpdatePanel>

            <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
            <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
            <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
        </div>
    </form>
</body>
</html>
