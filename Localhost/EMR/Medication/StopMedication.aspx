<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StopMedication.aspx.cs" Inherits="EMR_Medication_StopMedication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">


    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />

    <style type="text/css">
        div#UpdatePanel1 {
            overflow-x: hidden;
        }

        td {
            white-space: nowrap;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <script language="javascript" type="text/javascript">
              function OpenCIMSWindow() {
                  var ReportContent = $get('<%=hdnCIMSOutput.ClientID%>')

                  var WindowObject = window.open('', 'PrintWindow2', 'width=1250,height=585,top=72,left=30,toolbars=yes,scrollbars=yes,status=no,resizable=yes');
                  WindowObject.document.writeln(ReportContent.value);
                  WindowObject.document.close();
                  WindowObject.focus();
              }

              function returnToParent() {
                  //create the argument that will be returned to the parent page
                  var oArg = new Object();
                  oArg.IndentOPIPSource = document.getElementById("hndPageOPIPSource").value;
                  oArg.IndentDetailsIds = document.getElementById("hdnPageDetailsIds").value;
                  oArg.IndentIds = document.getElementById("hdnPageIndentIds").value;
                  oArg.ItemIds = document.getElementById("hdnPageItemIds").value;


                  var oWnd = GetRadWindow();
                  oWnd.close(oArg);
              }
              function GetRadWindow() {
                  var oWindow = null;
                  if (window.radWindow) oWindow = window.radWindow;
                  else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                  return oWindow;
              }
        </script>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>


                <div class="container-fluid">
                    <div class="row header_main">
                        <div class="col-md-4 ">
                            <div class="WordProcessorDivText">
                                <h2>
                                    <asp:Label ID="Label1" runat="server" Text="Stop Medication" /></h2>
                            </div>
                        </div>
                        <div class="col-md-4 text-center">
                            <asp:Label ID="lblMessage" runat="server" Text="" /></div>
                        <div class="col-md-4 text-right">
                            <asp:Button ID="btnReOrder" Text="Re Order" runat="server" OnClick="btnReOrder_Onclick" CssClass="btn btn-primary" Visible="false" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="btn btn-primary" OnClientClick="window.close();" />
                        </div>
                    </div>


                    <div class="row m-t">
                        <div class="col-md-12 col-sm-12 col-xs-12 gridview table-responsive">
                            <asp:GridView ID="gvStop" runat="server" Width="100%" Height="100%" AllowPaging="false" AutoGenerateColumns="False" OnRowCreated="gvStop_OnRowCreated" OnRowDataBound="gvStop_OnRowDataBound" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                <Columns>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' ItemStyle-Width="16px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                            <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                            <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                            <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                            <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                            <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                            <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                            <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                            <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId") %>' />
                                            <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId") %>' />
                                            <asp:HiddenField ID="hdnCommentsDrugAllergy" runat="server" Value='<%# Eval("OverrideComments") %>' />
                                            <asp:HiddenField ID="hdnCommentsDrugToDrug" runat="server" Value='<%# Eval("OverrideCommentsDrugToDrug") %>' />
                                            <asp:HiddenField ID="hdnCommentsDrugHealth" runat="server" Value='<%# Eval("OverrideCommentsDrugHealth") %>' />
                                            <asp:HiddenField ID="hdnDetailsId" runat="server" Value='<%# Eval("DetailsId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="true" OnCheckedChanged="chkRow_OnCheckedChanged" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Drug Name" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" runat="server" SkinID="label" Text='<%# Eval("ItemName") %>' Width="100%" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Indent Type" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndentType" runat="server" SkinID="label" Text='<%# Eval("IndentType") %>' Width="100%" />
                                            <asp:HiddenField ID="hdnIndentTypeId" runat="server" Value='<%#Eval("IndentTypeId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Qty." ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalQty" runat="server" Text='<%#Eval("Qty") %>' Width="100%" SkinID="label" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prescription Detail">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>' Width="100%" SkinID="label" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stop Remarks" ItemStyle-Width="190px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStopRemarks" runat="server" Text='<%#Eval("StopRemarks") %>' SkinID="label" Enabled="false" TextMode="MultiLine" Style="min-height: 30px; max-height: 30px; min-width: 190px; max-width: 190px;"></asp:TextBox></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Date" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStartDate" runat="server" SkinID="label" Text='<%# Eval("StartDate") %>' Width="100%" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Date" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEndDate" runat="server" SkinID="label" Text='<%# Eval("EndDate") %>' Width="100%" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stop Date" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStopDate" runat="server" SkinID="label" Text='<%# Eval("StopDate") %>' Width="100%" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stop By" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStopBy" runat="server" SkinID="label" Text='<%# Eval("StopBy") %>' Width="100%" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label51" runat="server" Text="BD" ToolTip="Brand Details" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnBrandDetailsCIMS" runat="server" ToolTip="Click here to view cims brand details" CommandName="BrandDetailsCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>' Text="&nbsp;" Width="100%" Visible="false" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label52" runat="server" Text="MG" ToolTip="Monograph" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnMonographCIMS" runat="server" ToolTip="Click here to view cims monograph" CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>' Text="&nbsp;" Width="100%" Visible="false" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label53" runat="server" Text="DD" ToolTip="Drug to Drug Interaction" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnInteractionCIMS" runat="server" ToolTip="Click here to view cims drug to drug interaction" CommandName="InteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;" Width="100%" Visible="false" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label54" runat="server" Text="DH" ToolTip="Drug Health Interaction" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction" CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76" Text="&nbsp;" Width="100%" Visible="false" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label55" runat="server" Text="DA" ToolTip="Drug Allergy Interaction" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnDAInteractionCIMS" runat="server" ToolTip="Click here to view cims drug allergy interaction" CommandName="DAInteractionCIMS" CausesValidation="false" BackColor="#82CAFA" Text="&nbsp;" Width="100%" Visible="false" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label56" runat="server" Text="BD" ToolTip="Brand Details" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnBrandDetailsVIDAL" runat="server" ToolTip="Click here to view vidal brand details" CommandName="BrandDetailsVIDAL" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>' Text="&nbsp;" Width="100%" Visible="false" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label57" runat="server" Text="MG" ToolTip="Monograph" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnMonographVIDAL" runat="server" ToolTip="Click here to view vidal monograph" CommandName="MonographVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>' Text="&nbsp;" Width="100%" Visible="false" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label58" runat="server" Text="DD" ToolTip="Drug to Drug Interaction" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug to drug interaction" CommandName="InteractionVIDAL" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;" Width="100%" Visible="false" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label59" runat="server" Text="DH" ToolTip="Drug Health Interaction" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnDHInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug health interaction" CommandName="DHInteractionVIDAL" CausesValidation="false" BackColor="#82AB76" Text="&nbsp;" Width="100%" Visible="false" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label60" runat="server" Text="DA" ToolTip="Drug Allergy Interaction" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnDAInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug allergy interaction" CommandName="DAInteractionVIDAL" CausesValidation="false" BackColor="#82CAFA" Text="&nbsp;" Width="100%" Visible="false" /></ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                            <asp:HiddenField ID="hndPageOPIPSource" runat="server" />
                                        <asp:HiddenField ID="hdnPageDetailsIds" runat="server" />
                                        <asp:HiddenField ID="hdnPageIndentIds" runat="server" />
                                        <asp:HiddenField ID="hdnPageItemIds" runat="server" />
                                        <asp:HiddenField ID="hdnItemId" runat="server" />
                                        <asp:HiddenField ID="hdnItemName" runat="server" />
                                        <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnCIMSOutput" runat="server" />
                                        <asp:HiddenField ID="hdnGenericId" runat="server" />
                                        <asp:HiddenField ID="hdnGenericName" runat="server" />
                                        <asp:HiddenField ID="hdnCIMSItemId" runat="server" />
                                        <asp:HiddenField ID="hdnCIMSType" runat="server" />
                                        <asp:HiddenField ID="hdnVIDALItemId" runat="server" />

                    </div>

                    <div id="dvInteraction" runat="server" visible="false" style="width: 700px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 340px; left: 320px; top: 150px">
                        <table width="100%" cellspacing="0" cellpadding="2">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label25" runat="server" SkinID="label" Font-Size="11px" Font-Bold="true" Text="Following drug interaction(s) found !" /></td>
                            </tr>

                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txtInteractionBetweenMessage" runat="server" Font-Size="10px" Font-Bold="true" ForeColor="Maroon" Text="This drug has interaction with prescribed medicines !" TextMode="MultiLine" Style="min-height: 56px; max-height: 56px; min-width: 690px; max-width: 690px;" ReadOnly="true" BackColor="#FFFFCC" /></td>
                            </tr>

                            <tr>
                                <td align="center">
                                    <table cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnBrandDetailsView" SkinID="Button" runat="server" Text="View Brand Details" Width="150px" OnClick="btnBrandDetailsView_OnClick" /></td>
                                            <td>
                                                <asp:Button ID="btnMonographView" SkinID="Button" runat="server" Text="View Monograph" Width="150px" OnClick="btnMonographView_OnClick" /></td>
                                            <td>
                                                <asp:Button ID="btnInteractionView" SkinID="Button" runat="server" Text="View Drug Interaction(s)" Width="150px" OnClick="btnInteractionView_OnClick" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblIntreactionMessage" runat="server" SkinID="label" ForeColor="Red" /></td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label26" runat="server" Text="Reason to continue for Drug Allergy Interaction" ForeColor="Gray" />
                                                <span id="spnCommentsDrugAllergy" runat="server" style="color: Red; font-size: large;" visible="false">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtCommentsDrugAllergy" runat="server" SkinID="textbox" MaxLength="500" TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 500);" Style="min-height: 44px; max-height: 44px; min-width: 650px; max-width: 650px;" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label37" runat="server" Text="Reason to continue for Drug To Drug Interaction" ForeColor="Gray" />
                                                <span id="spnCommentsDrugToDrug" runat="server" style="color: Red; font-size: large;" visible="false">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtCommentsDrugToDrug" runat="server" SkinID="textbox" MaxLength="500" TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 500);" Style="min-height: 44px; max-height: 44px; min-width: 650px; max-width: 650px;" /></td>
                                        </tr>
                                        <tr id="Tr1" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="Label38" runat="server" Text="Reason to continue for Drug Health Interaction" ForeColor="Gray" />
                                                <span id="spnCommentsDrugHealth" runat="server" style="color: Red; font-size: large;" visible="false">*</span>
                                            </td>
                                        </tr>
                                        <tr id="Tr2" runat="server" visible="false">
                                            <td>
                                                <asp:TextBox ID="txtCommentsDrugHealth" runat="server" SkinID="textbox" MaxLength="500" TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 500);" Style="min-height: 44px; max-height: 44px; min-width: 650px; max-width: 650px;" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnInteractionContinue" SkinID="Button" runat="server" Text="Continue" Width="150px" OnClick="btnInteractionContinue_OnClick" />
                                    &nbsp;
                                            <asp:Button ID="btnInteractionCancel" SkinID="Button" runat="server" Text="Cancel" Width="150px" OnClick="btnInteractionCancel_OnClick" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="middle">
                                    <table cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td valign="middle">
                                                <asp:Image ID="Image1" ImageUrl="~/CIMSDatabase/CIMSLogo.PNG" Height="30px" Width="120px" runat="server" /></td>
                                            <td valign="bottom">
                                                <asp:Label ID="Label40" runat="server" SkinID="label" Font-Size="14px" Font-Bold="true" ForeColor="Red" Text="(Powered by CIMS. Copyright MIMS Pte Ltd. All rights reserved.)" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>

                        </div>
                
                
                
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>