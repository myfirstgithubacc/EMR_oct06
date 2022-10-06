<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TemplateMaster1.aspx.cs"
    Inherits="EMR_Masters_TemplateMaster1" MasterPageFile="~/Include/Master/EMRMaster.master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />




    <script type="text/javascript" src="../../Include/JS/Functions.js"></script>

    <style type="text/css">
        .ContextMenu {
            position: absolute;
            width: 140px;
            border: 2px solid #5781AE;
            background-color: #97B1D0;
            font-family: Verdana;
            line-height: 20px;
            cursor: default;
            font-size: 14px;
            z-index: 100;
            visibility: hidden;
        }

        .menuitems {
            padding-left: 10px;
            padding-right: 10px;
        }
        #ctl00_ContentPlaceHolder1_ddlStaticTemplate_DropDown{background:#fff;}
        #ctl00_ContentPlaceHolder1_ddlStaticTemplate_DropDown > div{border:1px solid #5781AE;}
    </style>

    <script type="text/javascript" language="javascript">
        var pos = null;


        function openRadWindow(Tmp, tempid, secid) {
            var oWnd = radopen("ReArrangeTree.aspx?Tmp=" + Tmp + "&tempid=" + tempid + "&secid=" + secid, "Radwindow1");
            oWnd.Height = "900";
            oWnd.Center();
        }
        function openRadWindow1(Tmp, secid, fieldid) {
            var oWnd = radopen("ReArrangeTree.aspx?Tmp=" + Tmp + "&secid=" + secid + "&flid=" + fieldid, "Radwindow1");
            oWnd.Height = "900";
            oWnd.Center();
        }
        function openRadWindowServices(Tmp, tempid, secid) {
            var oWnd = radopen("InvestigationLabServiceTag.aspx", "Radwindow1");
            oWnd.Height = "900";
            oWnd.Center();
        }

        function OnClose(oWnd, args) {

            $get('<%=btnpost.ClientID%>').click();
        }

        function rowsVisible() {
            $get('<%=btnRowsVisible.ClientID%>').click();
        }
        function MaxLenTxt(TXT) {
            var dropdownListId = document.getElementById("<%=ddlPropertyType.ClientID%>");

            var dropdownListvalue = dropdownListId.value;
            if (dropdownListvalue == 'T') {
                if (TXT.value.length > 10) {
                    alert("Maximum length is 10 characters only.");
                }
            }
            else if (dropdownListvalue == 'M') {
                if (TXT.value.length > 5000) {
                    //  TXT.value = TXT.value.Expandstr(0, intMax);
                    alert("Maximum length is 5000 characters only.");
                }
            }
        }
    </script>


    <asp:HiddenField ID="hdnMaxLenght" runat="server" />
    <div>


        <div class="container-fluid header_main">
            <div class="col-md-3">
                <h2>Templates Master</h2>
            </div>
            <div class="col-md-5">
                <asp:Label ID="lblFieldMessage" runat="server" CssClass="relativ alert_new text-center text-success" />
            </div>
            <div class="col-md-3 pull-right text-right">
                <asp:Button ID="btnpost" Style="visibility: hidden;" runat="server" OnClick="btnpost_Click" />
                <asp:HiddenField ID="hdnMenuId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnMenuId2" runat="server" Value="0" />
                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="SaveUpdateValue" />
                <asp:ValidationSummary ID="ValidationSummary7" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="UpdateValue" />
                <asp:Button ID="btnBack" runat="server" Visible="false" Text="Back" CssClass="btn btn-primary"
                    OnClientClick="window.close();" />
                <asp:Button ID="ibtnCategorySave" runat="server" Text="Save" CssClass="btn btn-primary" CausesValidation="true"
                    OnClick="SaveCategory_OnClick" ToolTip="Save Section" ValidationGroup="SaveUpdateCategory" />
                <asp:ValidationSummary ID="ValidationSummary4" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="SaveUpdateCategory" />
                <asp:Button ID="ibtnProtertySave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="SaveProperty_OnClick"
                    ValidationGroup="SaveUpdateProperty" ToolTip="Save Fields" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="SaveUpdateProperty" />
            </div>
        </div>









        <asp:Table ID="tblTabs" runat="server" Width="100%" Height="100%" CellPadding="1"
            CellSpacing="0" BackColor="White">
            <asp:TableRow ID="trDetails" runat="server">
                <asp:TableCell ColumnSpan="8" HorizontalAlign="Left" VerticalAlign="Top">
                    <asp:MultiView ID="mltVW" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vwCategory" runat="server">
                            <div id="ie5menu">
                                <asp:LinkButton ID="lbtnNewCategory" Visible="false" CssClass="menuitems" runat="server"
                                    Text="New Sections" Font-Size="12px" Font-Names="verdana" Width="120" OnClick="lbtnNewCategory_OnClick" />
                                <asp:LinkButton ID="lbtnNewCategorySub" Visible="false" CssClass="menuitems" runat="server"
                                    Text="New Sub-Sections" Font-Size="12px" Font-Names="verdana" Width="120" OnClick="lbtnNewCategorySub_OnClick" />
                            </div>
                            <div id="divSubCategory">
                                <asp:LinkButton ID="lbtnSubNewField" Visible="false" CssClass="menuitems" runat="server"
                                    Text="New Field" Font-Size="12px" Font-Names="verdana" Width="120" OnClick="lbtnSubNewField_OnClick" />
                                <asp:LinkButton ID="lbtnNewSubField" Visible="false" CssClass="menuitems" runat="server"
                                    Text="New Sub-Field" Font-Size="12px" Font-Names="verdana" Width="120" OnClick="lbtnNewSubField_OnClick" />
                            </div>


                            <div class="container-fluid" id="table1" runat="server">
                                <div class="row">

                                    <div class="text-center">
                                        <asp:Label ID="lblTemplateName" runat="server" Font-Bold="true" Font-Size="12px"
                                            ForeColor="navy" />
                                    </div>

                                    <div class="col-md-12">
                                        <div class="radioo">
                                            <asp:RadioButtonList ID="rbocreatesection" RepeatDirection="Horizontal" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="rbocreatesection_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Text="Section" Value="1" />
                                                <asp:ListItem Text="Sub Section" Value="2" />
                                                <asp:ListItem Text="Edit" Value="3" />
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>


                                    <div class="col-md-12 margin_bottom">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <h5 class="h5 margin_bottom">
                                                    <asp:Label ID="lblSection" runat="server" Text="Sections" /></h5>

                                                <asp:Panel ID="pnlCategoryView" runat="server" BackColor="White" Width="100%" Height="270px"
                                                    BorderWidth="1px" BorderColor="Black" ScrollBars="Auto" CssClass="margin_bottom">

                                                    <asp:TreeView ID="tvCategory" runat="server" ImageSet="Msdn" Font-Size="10px" OnSelectedNodeChanged="tvCategory_OnSelectedNodeChanged"
                                                        NodeIndent="10">
                                                        <ParentNodeStyle Font-Bold="False" />
                                                        <HoverNodeStyle Font-Underline="True" BackColor="#97B1D0" BorderColor="#888888" BorderStyle="Solid"
                                                            BorderWidth="0px" />
                                                        <SelectedNodeStyle BackColor="#5078B3" ForeColor="White" Font-Underline="False" HorizontalPadding="3px"
                                                            VerticalPadding="1px" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="0px" />
                                                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                                            NodeSpacing="1px" VerticalPadding="2px" />
                                                    </asp:TreeView>

                                                </asp:Panel>
                                                <asp:Label ID="lblSelectedCategoryID" SkinID="label" runat="server" Visible="false" />
                                                <asp:Label ID="lblSelectedFieldID" SkinID="label" runat="server" Visible="false" />


                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="ss" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnSort" Text="Set Order" runat="server" CssClass="btn btn-primary btn-block" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>

                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnSetFormula" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnSetFormula" Text="Set Formula" runat="server" CssClass="btn btn-primary btn-block"
                                                                OnClick="btnSetFormula_OnClick" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>

                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnSetScoreFormula" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnSetScoreFormula" Text="Set Score Formula" runat="server" CssClass="btn btn-primary btn-block"
                                                                OnClick="btnSetScoreFormula_OnClick" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Width="500" Height="500"
                                                    VisibleStatusbar="false" Top="40" Left="200" Behaviors="Close,Move" OnClientClose="OnClose"
                                                    ReloadOnShow="true">
                                                </telerik:RadWindowManager>
                                                <telerik:RadWindowManager ID="RadWindowManager2" EnableViewState="false" runat="server"
                                                    Behaviors="Close,Move,Pin,Resize,Maximize">
                                                    <Windows>
                                                        <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                                                    </Windows>
                                                </telerik:RadWindowManager>

                                            </div>






                                            <div class="col-md-6">
                                                <h5 class="h5 margin_bottom">
                                                    <asp:Label ID="lblFieldName" runat="server" Text="Fields" /></h5>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlNewEditCategorySubCategory" runat="server" Width="100%">

                                                            <div class="row">

                                                                <div class="col-md-12 form-group" id="trHeaderCategory" visible="false" runat="server">
                                                                    <asp:Literal ID="Literal2" runat="server" Text="Parent Node" />
                                                                    <asp:Label ID="lblParentNode" Font-Bold="true" runat="server" />
                                                                </div>


                                                                <div class="col-md-12 form-group">

                                                                    <div class="col-md-4">
                                                                        <asp:Literal ID="ltrlCategoryName" runat="server" Text="Name" />
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtCategoryName" SkinID="textbox" runat="server" MaxLength="50"
                                                                            Columns="27" />
                                                                        <asp:RequiredFieldValidator ID="RFVtxtCategoryName" runat="server" ErrorMessage="Section Name Cannot Be Blank..."
                                                                            ValidationGroup="SaveUpdateCategory" Display="None" ControlToValidate="txtCategoryName"
                                                                            SetFocusOnError="true" />
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:CheckBox ID="chkSectionTitle" runat="server" Checked="True" Text="Display title in Notes" />
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-12 form-group">
                                                                    <div class="col-md-4">
                                                                        <asp:Literal ID="ltrlSectionCode" runat="server" Text="Code" />
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <asp:TextBox ID="txtSectionCode" runat="server" Text="" MaxLength="50"
                                                                            Columns="27" autocomplete="off" />
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-12 form-group">
                                                                    <div class="col-md-4">
                                                                        <asp:Literal ID="ltrlCategoryGender" runat="server" Text="Gender" />
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <asp:DropDownList ID="ddlCategoryGender" SkinID="DropDown" Width="100%" runat="server">
                                                                            <asp:ListItem Text="Both" Value="B" Selected="True" />
                                                                            <asp:ListItem Text="Male" Value="M" />
                                                                            <asp:ListItem Text="Female" Value="F" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-12 form-group">
                                                                    <div class="col-md-4">
                                                                        <asp:Literal ID="lblTabular" runat="server" Text="Tabular" />
                                                                    </div>
                                                                    <div class="col-md-8 radioo">
                                                                        <div class="row">
                                                                            <div class="col-md-2">
                                                                                <asp:RadioButton ID="rbtnYesTabular" Text="Yes" GroupName="Tabular" runat="server"
                                                                                    onclick="return rowsVisible();" />
                                                                            </div>

                                                                            <div class="col-md-3" style="margin-left: -12px;">
                                                                                <asp:RadioButton ID="rbtnNoTabular" Text="No" GroupName="Tabular" runat="server"
                                                                                    onclick="return rowsVisible();" />
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <asp:Button ID="btnRowsVisible" runat="server" Text="" SkinID="button" OnClick="btnRowsVisible_OnClick"
                                                                                    Style="visibility: hidden;" />
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-12 form-group">
                                                                    <div class="col-md-4">
                                                                        <asp:Literal ID="Literal4" runat="server" Text="Addendum" />
                                                                    </div>
                                                                    <div class="col-md-8 radioo">
                                                                        <asp:RadioButtonList ID="rdoAddendum" runat="server" RepeatDirection="Horizontal">
                                                                            <asp:ListItem Text="Yes" Value="True" />
                                                                            <asp:ListItem Text="No" Value="False" Selected="True" />
                                                                        </asp:RadioButtonList>

                                                                    </div>
                                                                </div>

                                                                <div class="col-md-12 form-group">
                                                                    <div class="col-md-4">
                                                                        <asp:Literal ID="ltrlCategoryStatus" runat="server" Text="Status" />
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <asp:DropDownList ID="ddlCategoryStatus" Width="100%" runat="server" SkinID="DropDown">
                                                                            <asp:ListItem Text="Active" Value="1" Selected="True" />
                                                                            <asp:ListItem Text="In-Active" Value="0" />
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hdntabularcheck" runat="server" />
                                                                    </div>
                                                                </div>


                                                                <div class="col-md-12 form-group" id="trRows" runat="server" visible="false">
                                                                    <div class="col-md-4">
                                                                        <asp:Label ID="lblRows" runat="server" Text="Row(s)" />
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <asp:TextBox ID="txtRows" runat="server" Width="100%" MaxLength="2" SkinID="textbox"
                                                                            Style="text-align: right;" autocomplete="off" />
                                                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                            FilterType="Custom,Numbers" TargetControlID="txtRows" ValidChars="0123456789" />
                                                                    </div>
                                                                </div>


                                                                <div class="col-md-12 form-group" id="trBlankRows" runat="server" visible="false">
                                                                    <div class="col-md-4">
                                                                        <asp:Label ID="Label3" runat="server" Text="No.&nbsp;of&nbsp;Blank&nbsp;Row(s)" />
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <asp:TextBox ID="txtBlankRows" runat="server" Width="100%" MaxLength="2" SkinID="textbox"
                                                                            Style="text-align: right;" autocomplete="off" />
                                                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                            FilterType="Custom,Numbers" TargetControlID="txtBlankRows" ValidChars="0123456789" />
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-12 form-group">
                                                                    <div class="col-md-4">
                                                                        <asp:Label ID="Label6" runat="server" Text="Is&nbsp;Freeze&nbsp;First&nbsp;Column" />
                                                                    </div>
                                                                    <div class="col-md-8 radioo">
                                                                        <asp:RadioButtonList ID="rdoIsFreezeFirstColumn" runat="server" RepeatDirection="Horizontal">
                                                                            <asp:ListItem Text="No" Value="0" Selected="True" />
                                                                            <asp:ListItem Text="Yes" Value="1" />
                                                                        </asp:RadioButtonList>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-12 form-group" id="trIsFreezeFirstColumn" runat="server" visible="false">
                                                                    <div class="col-md-4"></div>
                                                                    <div class="col-md-8"></div>
                                                                </div>




                                                            </div>
                                                            <!-- end of row -->

                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>







                                                <asp:Panel ID="pnlNewEditField" runat="server" Width="100%" DefaultButton="ibtnProtertySave">

                                                    <asp:UpdatePanel ID="updpnlNewEditField" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>



                                                            <div id="trHeader1" runat="server" visible="false">

                                                                <asp:Literal ID="Literal1" runat="server" Text="Parent Node" />

                                                                <asp:Label ID="ltrlParentNode" Font-Bold="true" runat="server" />

                                                            </div>



                                                            <div id="trOptions" visible="false" runat="server">

                                                                <asp:Literal ID="ltrlOptions" runat="server" Text="Parent Values" />


                                                                <asp:DropDownList ID="ddlOptions" Width="165px" runat="server" />
                                                                <asp:HiddenField ID="hdnParentFieldType" runat="server" />

                                                            </div>



                                                            <div class="row  form-group">
                                                                <div class="col-md-3">
                                                                    <asp:Literal ID="ltrlPropertyName" runat="server" Text="Name" />
                                                                </div>
                                                                <div class="col-md-5">
                                                                    <asp:TextBox ID="txtPropertyName" runat="server" Text="" MaxLength="500"
                                                                        Columns="27" Width="100%" />
                                                                    <asp:RequiredFieldValidator ID="RFVtxtPropertyName" runat="server" ErrorMessage="Field Name Cannot Be Blank..."
                                                                        ValidationGroup="SaveUpdateProperty" Display="None" ControlToValidate="txtPropertyName"
                                                                        SetFocusOnError="true" />
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:CheckBox ID="chkTitleField" runat="server" Checked="true" Text="Display title in Notes" />
                                                                </div>
                                                            </div>



                                                            <div class="row  form-group">
                                                                <div class="col-md-3">
                                                                    <asp:Literal ID="ltrlCode" runat="server" Text="Code" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:TextBox ID="txtPropertyCode" runat="server" Text="" MaxLength="20"
                                                                        Columns="27" autocomplete="off" Width="100%" />
                                                                </div>
                                                            </div>



                                                            <div class="row  form-group">
                                                                <div class="col-md-3">
                                                                    <asp:Literal ID="ltrlPropertyType" runat="server" Text="Field Type" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:DropDownList ID="ddlPropertyType" SkinID="DropDown" runat="server" Width="100%"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPropertyType_OnSelectedIndexChanged">
                                                                        <asp:ListItem Text=" [ Select ] " Value="0" Selected="True" />
                                                                        <asp:ListItem Text="Text Single Line" Value="T" />
                                                                        <asp:ListItem Text="Text Multiple Line" Value="M" />
                                                                        <asp:ListItem Text="WordProcessor" Value="W" />
                                                                        <asp:ListItem Text="CheckBox" Value="C" />
                                                                        <asp:ListItem Text="DropDown" Value="D" />
                                                                        <asp:ListItem Text="Boolean" Value="B" />
                                                                        <asp:ListItem Text="Radio Button" Value="R" />
                                                                        <asp:ListItem Text="Date" Value="S" />
                                                                        <asp:ListItem Text="Heading" Value="H" />
                                                                        <asp:ListItem Text="Patient Data Object" Value="O" />
                                                                        <asp:ListItem Text="Button" Value="L" />
                                                                        <asp:ListItem Text="Investigation" Value="I" />
                                                                        <asp:ListItem Text="Investigation Special" Value="IS" />
                                                                        <asp:ListItem Text="Image" Value="IM" />
                                                                        <asp:ListItem Text="Time" Value="ST" />
                                                                        <asp:ListItem Text="DateTime" Value="SB" />
                                                                        <%-- <asp:ListItem Text="Table" Value="R" />--%>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="row form-group">

                                                                <div class="col-md-3">
                                                                    <asp:Label ID="lblDataObject" runat="server" Text="Data Object" />
                                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="lbtnServices" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="lbtnServices" runat="server" Visible="false" Text="  Tag Services "
                                                                                OnClick="lbtnServices_OnClick" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <div class="col-md-3">
                                                                     <asp:DropDownList ID="ddlPatientDataObject" SkinID="DropDown" runat="server" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:RequiredFieldValidator ID="RFVddlPropertyType" runat="server" ControlToValidate="ddlPropertyType"
                                                                        InitialValue="0" SetFocusOnError="true" ErrorMessage="Please Select Property Type"
                                                                        Display="None" ValidationGroup="SaveUpdateProperty" />

                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="lblMaxLength" runat="server" Text="Max Length" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:TextBox ID="txtMaxLength" runat="server" MaxLength="2"
                                                                        Width="100%" ToolTip="Default will take 50 characters" onkeyup="return textboxMaxNumber(50,'txtMaxLength');"
                                                                        autocomplete="off" />
                                                                    <AJAX:FilteredTextBoxExtender ID="FTEtxtMaxLength" runat="server" TargetControlID="txtMaxLength"
                                                                        ValidChars="Numbers" FilterType="Numbers">
                                                                    </AJAX:FilteredTextBoxExtender>
                                                                </div>
                                                            </div>








                                                            <script language="javascript" type="text/javascript">
                                                                function textboxMaxNumber(maxLen, txt) {
                                                                    var txtVal = document.getElementById('<%=txtMaxLength.ClientID %>');
                                                                    if (txtVal.value > (maxLen)) {
                                                                        alert("Maximum length cannot be greater than 50 for single line text !");
                                                                        txtVal.value = '';
                                                                        return false;
                                                                    }
                                                                }
                                                            </script>



                                                            <div class="row form-group" id="trButtonType" runat="server">
                                                                <div>

                                                                    <div class="col-md-3">
                                                                        <asp:Literal ID="Literal3" runat="server" Text="Static Template" />
                                                                    </div>


                                                                    <div class="col-md-3">
                                                                        <telerik:RadComboBox ID="ddlStaticTemplate" SkinID="DropDown" runat="server">
                                                                        </telerik:RadComboBox>
                                                                    </div>

                                                                </div>
                                                            </div>

                                                            <div class="row form-group">
                                                                <div class="col-md-3">
                                                                    <asp:Literal ID="ltrlPropertyGender" runat="server" Text="Gender" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:DropDownList ID="ddlPropertyGender" runat="server" SkinID="DropDown" Width="100%"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPropertyGender_OnSelectedIndexChanged">
                                                                        <asp:ListItem Text="Both" Value="B" Selected="True" />
                                                                        <asp:ListItem Text="Male" Value="M" />
                                                                        <asp:ListItem Text="Female" Value="F" />
                                                                    </asp:DropDownList>
                                                                </div>


                                                            </div>

                                                            <div class="row form-group" id="trMale" runat="server">
                                                                <div>
                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="lblDefaultMaleText" runat="server" Text="Default Text(Male)" />
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <asp:TextBox ID="txtPropertyDefaultMale" TextMode="MultiLine" Width="350px" Height="70px" runat="server" Columns="27" SkinID="textbox"
                                                                            autocomplete="off" onkeyup="return MaxLenTxt(this);" />

                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <div class="row form-group" id="trFemale" runat="server">

                                                                <div>


                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="lblDefaultFemaleText" runat="server" Text="Default Text(Female)" />
                                                                    </div>

                                                                    <div class="col-md-3">
                                                                        <asp:TextBox ID="txtPropertyDefaultFemale" TextMode="MultiLine" Width="350px" Height="70px" runat="server" Columns="27" SkinID="textbox"
                                                                            autocomplete="off" onkeyup="return MaxLenTxt(this);" />

                                                                    </div>
                                                                </div>
                                                            </div>




                                                            <div class="row form-group" id="trHeadingStart" runat="server">
                                                                <div>






                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="Label1" runat="server" Text="Heading Start" />
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <asp:TextBox ID="txtHeadingStart" runat="server" Width="100%" MaxLength="2" SkinID="textbox"
                                                                            Style="text-align: right" autocomplete="off" />
                                                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                            FilterType="Custom,Numbers" TargetControlID="txtHeadingStart" ValidChars="0123456789" />
                                                                    </div>

                                                                </div>
                                                            </div>




                                                            <div class="row form-group" id="trHeadingEnd" runat="server">


                                                                <div>

                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="Label2" runat="server" Text="Heading End" />
                                                                    </div>

                                                                    <div class="col-md-3">
                                                                        <asp:TextBox ID="txtHeadingEnd" runat="server" Width="100%" MaxLength="2" SkinID="textbox"
                                                                            Style="text-align: right" autocomplete="off" />
                                                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                            FilterType="Custom,Numbers" TargetControlID="txtHeadingEnd" ValidChars="0123456789" />
                                                                    </div>
                                                                </div>


                                                            </div>



                                                            <div class="row form-group" id="trFieldStatus" runat="server">

                                                                <div>






                                                                    <div class="col-md-3">
                                                                        <asp:Literal ID="ltrlFieldStatus" runat="server" Text="Status" />
                                                                    </div>

                                                                    <div class="col-md-3">
                                                                        <asp:DropDownList ID="ddlFieldStatus" runat="server" SkinID="DropDown" Width="100%">
                                                                            <asp:ListItem Text="Active" Value="1" Selected="True" />
                                                                            <asp:ListItem Text="InActive" Value="0" Selected="False" />
                                                                        </asp:DropDownList>
                                                                    </div>

                                                                </div>
                                                            </div>


                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>


                                                    <div class="row form-group" id="trFieldFormats" runat="server">

                                                        <div>
                                                            <div class="col-md-3">
                                                                <asp:Literal ID="ltrFieldFormats" runat="server" Text="Options" />
                                                            </div>

                                                            <div class="col-md-3">
                                                                <asp:TextBox ID="txtFormatName" runat="server" SkinID="textbox" Columns="27" MaxLength="50"
                                                                    autocomplete="off" />
                                                                <asp:DropDownList ID="ddlTemplateFieldFormats" runat="server" OnSelectedIndexChanged="ddlTemplateFieldFormats_OnSelectedIndexChanged"
                                                                    SkinID="DropDown" Width="100%" AutoPostBack="true" />
                                                            </div>

                                                            <div class="col-md-3">
                                                                <asp:RadioButtonList ID="rboFormats" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rboFormats_SelectedIndexChanged"
                                                                    RepeatDirection="Horizontal">
                                                                    <asp:ListItem Selected="True" Text="New" Value="1" />
                                                                    <asp:ListItem Text="Edit" Value="2" />
                                                                </asp:RadioButtonList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:CheckBox ID="chkFieldFormatDefault" runat="server" Checked="True" Text="Make Default" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row form-group">
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="Label4" runat="server" Text="Is&nbsp;Mandatory" />
                                                                </div>

                                                                <div class="col-md-3">
                                                                    <asp:DropDownList ID="ddlIsMandatory" SkinID="DropDown" runat="server" Width="100%"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlIsMandatory_OnSelectedIndexChanged">
                                                                        <asp:ListItem Text="Yes" Value="1" />
                                                                        <asp:ListItem Text="No" Value="0" Selected="True" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="row form-group">
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="Label5" runat="server" Text="Mandatory&nbsp;Type" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:DropDownList ID="ddlMandatoryType" SkinID="DropDown" runat="server" Width="100%"
                                                                        Enabled="false">
                                                                        <asp:ListItem Text="[ Select ]" Value="" Selected="True" />
                                                                        <asp:ListItem Text="Informative" Value="I" />
                                                                        <asp:ListItem Text="Restrictive" Value="R" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="row form-group">
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="lbl_IsLink" runat="server" SkinID="label" Text="Is HyerLink Required" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:DropDownList ID="ddlLinkRequired" runat="server" Width="100%" AutoPostBack="true">
                                                                        <asp:ListItem Text="No" Value="0" />
                                                                        <asp:ListItem Text="Yes" Value="1" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="row form-group">
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="lblLinkUrl" runat="server" Text="Link Url" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:TextBox ID="txtLinkUrl" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="row form-group">
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="Label7" runat="server" Text="Display In Note" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:RadioButtonList runat="server" ID="rdoDisplayInNote" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Yes" Value="1" Selected="True" />
                                                                        <asp:ListItem Text="No" Value="0" />
                                                                    </asp:RadioButtonList>
                                                                </div>
                                                            </div>
                                                            <div class="row form-group">
                                                                <div class="col-md-6">
                                                                    <asp:CheckBox ID="chkSortingOnSequenceNo" runat="server" Text="Sorting On Sequence No" Visible="false" />
                                                                    
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:CheckBox ID="chkPrintValueLineWise" runat="server" Text="Print Row Wise" Visible="false" />
                                                                    
                                                                </div>
                                                                
                                                            </div>
                                                            <div class="row form-group">
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="lblCoumnsPerRow" runat="server" Text="Columns Per Row" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:DropDownList ID="ddlCoumnsPerRow" runat="server" Width="100%" AutoPostBack="true" Visible="false">
                                                                        <asp:ListItem Text="0" Value="0" />
                                                                        <asp:ListItem Text="1" Value="1" />
                                                                        <asp:ListItem Text="2" Value="2" />
                                                                        <asp:ListItem Text="3" Value="3" />
                                                                        <asp:ListItem Text="4" Value="4" />
                                                                        <asp:ListItem Text="5" Value="5" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                    <div class="row form-group" id="trWordProcessor" runat="server">
                                                        <div>
                                                            <div class="col-md-3">
                                                                <asp:Label ID="lblDefaultContentWordProcessor" runat="server" Text="Default Content" />
                                                            </div>
                                                            <div class="col-md-3">
                                                                <telerik:RadEditor ID="txtPropertyDefaultContentWordProcessor" ToolbarMode="ShowOnFocus"
                                                                    EnableEmbeddedSkins="true" EnableResize="true" runat="server" Skin="Vista" Height="350px"
                                                                    Width="650px">
                                                                </telerik:RadEditor>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                    <hr class="hr" />
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class=" col-md-12 radioo">
                                                <asp:RadioButtonList ID="rboCreateFields" RepeatDirection="Horizontal" runat="server"
                                                    AutoPostBack="true" OnSelectedIndexChanged="rboCreateFields_SelectedIndexChanged">
                                                    <asp:ListItem Text="Field" Selected="True" Value="1" />
                                                    <asp:ListItem Text="Sub-Fields" Value="2" />
                                                    <asp:ListItem Text="Edit" Value="3" />
                                                </asp:RadioButtonList>
                                            </div>

                                            <div class="col-md-6">
                                                <h5 class="h5">
                                                    <asp:Label ID="lblNewValues" runat="server" Text="Fields" /></h5>
                                                <asp:Panel ID="pnlFieldTree" runat="server" ScrollBars="Auto" Width="100%" Height="270px"
                                                    BorderWidth="1px" BorderColor="Black" CssClass="margin_bottom">

                                                    <asp:TreeView ID="treeProperties" runat="server" Font-Size="10px" OnSelectedNodeChanged="TreeProperties_OnNodeSelected"
                                                        EnableViewState="true">
                                                        <ParentNodeStyle Font-Bold="False" />
                                                        <HoverNodeStyle Font-Underline="True" BackColor="#97B1D0" BorderColor="#888888" BorderStyle="Solid"
                                                            BorderWidth="0px" />
                                                        <SelectedNodeStyle BackColor="#5078B3" ForeColor="White" Font-Underline="False" HorizontalPadding="3px"
                                                            VerticalPadding="1px" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="0px" />
                                                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                                            NodeSpacing="1px" VerticalPadding="2px" />
                                                    </asp:TreeView>

                                                    <asp:ValidationSummary ID="ValidationSummary6" runat="server" ShowMessageBox="True"
                                                        ShowSummary="False" ValidationGroup="UpdateProperty" />

                                                    <script language="javascript" type="text/javascript">
                                                        function textboxMaxNumberGrid(txt) {

                                                            var txtVal = document.getElementById(txt);

                                                            if (txtVal.value > 50) {
                                                                alert("Maximum length cannot be greater than 50 for single line text !");
                                                                txtVal.value = '';
                                                                return false;
                                                            }
                                                        }
                                                    </script>

                                                </asp:Panel>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="updFields" runat="server">


                                                        <ContentTemplate>
                                                            <asp:Button ID="btnFields" Text="Set Order" runat="server" CssClass="btn btn-primary btn-block" />
                                                        </ContentTemplate>

                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Button ID="btnEmployeeTypeTagging" Text="Employee Type Tagging" runat="server"
                                                        CssClass="btn btn-primary btn-block" OnClick="btnEmployeeTypeTagging_OnClick" />
                                                </div>

                                                 <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">


                                                        <ContentTemplate>
                                                            <asp:Button ID="btnSetValueOrder" Text="Set Order(Value)" runat="server" CssClass="btn btn-primary btn-block"  />
                                                        </ContentTemplate>

                                                    </asp:UpdatePanel>
                                                </div>


                                            </div>


                                            <div class="col-md-6">
                                                <asp:Panel ID="pnlNewEditValues" runat="server" Width="100%">



                                                    <h5 class="h5 margin_bottom">
                                                        <asp:Label ID="lblValue" runat="server" Text="Values" Font-Bold="true" /></h5>




                                                    <asp:Panel ID="pnlValueBoolean" runat="server">

                                                        <asp:Label ID="lblValueBoolean" runat="server" Text="Field Type - Boolean" Font-Bold="true"
                                                            Font-Size="12px" />

                                                        <span id="tdMale1" runat="server" style="width: 35%;">&nbsp;
                                                        </span>
                                                        <span id="tdFemale1" runat="server" style="width: 35%;">&nbsp;
                                                        </span>


                                                        <span id="tdMale2" runat="server">Text&nbsp;(Male)
                                                        </span>
                                                        <span id="tdFemale2" runat="server">Text (Female)
                                                        </span>

                                                        Yes<asp:CheckBox ID="chkDefaultYes" AutoPostBack="true" OnCheckedChanged="chkDefaultYes_OnCheckedChanged"
                                                            runat="server" Checked="true" />

                                                        <asp:TextBox ID="txtBooleanYesCode" runat="server" MaxLength="20"
                                                            Width="40px" autocomplete="off" />

                                                        <span id="tdMale3" runat="server">
                                                            <asp:TextBox ID="txtBooleanYesMale" runat="server" SkinID="textbox" Width="130px"
                                                                MaxLength="100" autocomplete="off" />
                                                        </span>
                                                        <span id="tdFemale3" runat="server">
                                                            <asp:TextBox ID="txtBooleanYesFemale" runat="server" SkinID="textbox" Width="130px"
                                                                MaxLength="100" autocomplete="off" />
                                                            <asp:HiddenField ID="hdnYes" runat="server" />
                                                        </span>


                                                        No&nbsp;<asp:CheckBox ID="chkDefaultNo" AutoPostBack="true" OnCheckedChanged="chkDefaultNo_OnCheckedChanged"
                                                            runat="server" />

                                                        <asp:TextBox ID="txtBooleanNoCode" runat="server" SkinID="textbox" Width="40px" MaxLength="20"
                                                            autocomplete="off" />

                                                        <span id="tdMale4" runat="server">
                                                            <asp:TextBox ID="txtBooleanNoMale" runat="server" SkinID="textbox" Width="130px"
                                                                MaxLength="100" autocomplete="off" />
                                                        </span>


                                                        <span id="tdFemale4" runat="server">
                                                            <asp:TextBox ID="txtBooleanNoFemale" runat="server" SkinID="textbox" Width="130px"
                                                                MaxLength="100" autocomplete="off" />
                                                            <asp:HiddenField ID="hdnNo" runat="server" />
                                                        </span>


                                                    </asp:Panel>

                                                    <asp:Panel ID="pnlValueDropDown" runat="server">


                                                        <asp:Label ID="lblValueDropDown" runat="server" Text="Field Type - DropDown" Font-Bold="true"
                                                            Font-Size="12px" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                    <asp:Label ID="lblImageMessage" Visible="false" runat="server" Font-Bold="true" Text="Please Upload JPG Image Only"
                                                                                        BorderColor="Black" BackColor="Coral" />


                                                        <asp:GridView ID="gvValueDropDown" SkinID="gridview" CellPadding="2" runat="server"
                                                            AutoGenerateColumns="false" DataKeyNames="FieldID" ShowHeader="true" OnRowDataBound="gvValueDropDown_RowDataBound"
                                                            ShowFooter="true" FooterStyle-BorderColor="White" OnRowCreated="gvValueDropDown_RowCreated">
                                                            <FooterStyle BackColor="White" CssClass="form-group" />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnIndex" Value='<%#Container.DataItemIndex +1 %>' runat="server" />
                                                                        <asp:CheckBox ID="chkDefault" OnCheckedChanged="chkDropDownList_OnCheckedChanged"
                                                                            AutoPostBack="true" runat="server" />
                                                                        <asp:HiddenField ID="hdnValueId" Value='<%#Eval("ValueId")%>' runat="server" />
                                                                        <asp:HiddenField ID="hdnFieldID" Value='<%#Eval("FieldID")%>' runat="server" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Options" FooterStyle-Width="100px" HeaderStyle-Width="100px"
                                                                    ItemStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtOptionGrid" runat="server" Width="100px" Text='<%#Eval("ValueName")%>'
                                                                            MaxLength="500" autocomplete="off" />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Button ID="btnAddNewRow" runat="server" Text="Add New Row" Width="100px" CssClass="btn btn-primary"
                                                                            OnClick="btnAddNewRow_OnClick" />
                                                                    </FooterTemplate>
                                                                    <FooterStyle BorderColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Code" ItemStyle-Width="40px" ControlStyle-Width="40px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtCodeGrid" runat="server" Width="40px" Text='<%#Eval("Code")%>'
                                                                            MaxLength="20" autocomplete="off" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Text (Male)">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtDefaultMaleGrid" runat="server" Text='<%#Eval("DefaultTextMale")%>'
                                                                            MaxLength="100" Width="130px" autocomplete="off" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Text (Female)">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtDefaultFemaleGrid" runat="server" Text='<%#Eval("DefaultTextFemale")%>'
                                                                            Width="130px" MaxLength="100" autocomplete="off" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Image" ItemStyle-Width="30px" ControlStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="image" runat="server" ImageUrl='<%#Eval("ImagePath")%>' />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Image Name" ItemStyle-Width="80px" ControlStyle-Width="80px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblImageName" runat="server" Text='<%#Eval("ImageFileName")%>' SkinID="label" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Image Upload" ItemStyle-Width="230px" ControlStyle-Width="230px">
                                                                    <ItemTemplate>
                                                                        <asp:FileUpload ID="_FileUpload" runat="server" Width="100px" CssClass="button" />
                                                                        <%--<asp:Button ID="btnUpload" runat="server" Text="Upload" Width="50px" OnClick="btnUpload_OnClick" />--%>
                                                                        <asp:HiddenField ID="hdnImagePath" runat="server" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="78px" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList SkinID="dropdown" ID="ddlStatus" ToolTip='<%#Eval("ActiveText")%>'
                                                                            runat="server">
                                                                            <asp:ListItem Text="Active" Value="1" />
                                                                            <asp:ListItem Text="In-Active" Value="0" />
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Score" FooterStyle-Width="50px" HeaderStyle-Width="50px"
                                                                    ItemStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtScoreValue" runat="server" SkinID="textbox" Width="45px" Text='<%#Eval("ScoreValue")%>'
                                                                            MaxLength="7" autocomplete="off" Style="text-align: right" />
                                                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                            FilterType="Custom,Numbers" TargetControlID="txtScoreValue" ValidChars="0123456789.-" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderColor="White" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>

                                                    </asp:Panel>

                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- main row ends -->
                            </div>
                            <!-- main container ends -->


                        </asp:View>
                    </asp:MultiView>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>
