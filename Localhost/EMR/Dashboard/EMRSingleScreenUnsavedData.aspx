<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EMRSingleScreenUnsavedData.aspx.cs"
    Inherits="EMR_Dashboard_EMRSingleScreenUnsavedData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr class="clsheader">
                        <td id="tdHeader" align="left" style="padding-left: 10px; width: 200px" runat="server">
                            <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="EMR Unsaved Data" Font-Bold="true" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnClose" runat="server" SkinID="Button" Text="Close" OnClientClick="window.close();" />
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td align="center" style="font-size: 12px;">
                            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="2" cellspacing="0" width="99%">
                    <tr>
                        <td>
                            <asp:Panel ID="PanelN" runat="server" SkinID="Panel" Width="100%">
                                <asp:GridView ID="gvDetails" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                    AllowPaging="True" PageSize="20" OnRowCommand="gvDetails_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnSelect" runat="server" SkinID="label" Text="Select" CommandName="EncounterSelect"
                                                    CommandArgument='<%#Eval("TransitId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Patient Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPatientName" runat="server" SkinID="label" Text='<%#Eval("PatientName") %>' />
                                                <asp:HiddenField ID="hdnTransitId" runat="server" Value='<%#Eval("TransitId")%>' />
                                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                <asp:HiddenField ID="hdnOPIP" runat="server" Value='<%#Eval("OPIP")%>' />
                                                <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                                <asp:HiddenField ID="hdnAppointmentId" runat="server" Value='<%#Eval("AppointmentId")%>' />
                                                <asp:HiddenField ID="hdnGender" runat="server" Value='<%#Eval("Gender")%>' />
                                                <asp:HiddenField ID="hdnEMRStatus" runat="server" Value='<%#Eval("EMRStatus")%>' />
                                                <asp:HiddenField ID="hdnMedicalAlert" runat="server" Value='<%#Eval("MedicalAlert")%>' />
                                                <asp:HiddenField ID="hdnAllergiesAlert" runat="server" Value='<%#Eval("AllergiesAlert")%>' />
                                                <asp:HiddenField ID="hdnPackageName" runat="server" Value='<%#Eval("PackageName")%>' />
                                                <asp:HiddenField ID="hdnIVFId" runat="server" Value='<%#Eval("IVFId")%>' />
                                                <asp:HiddenField ID="hdnIVFNo" runat="server" Value='<%#Eval("IVFNo")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Age / Gender" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeGender" runat="server" SkinID="label" Text='<%#Eval("AgeGender") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegistrationNo" runat="server" SkinID="label" Text='<%#Eval("RegistrationNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterNo" runat="server" SkinID="label" Text='<%#Eval("EncounterNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterDate%>' HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterDate" runat="server" SkinID="label" Text='<%#Eval("EncounterDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Visit' HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVisit" runat="server" SkinID="label" Text='<%#Eval("Visit") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Date' HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnteredDate" runat="server" SkinID="label" Text='<%#Eval("EnteredDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Entered By' HeaderStyle-Width="240px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnteredBy" runat="server" SkinID="label" Text='<%#Eval("EnteredBy") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="false" CommandName="TransitDelete"
                                                    Width="16px" Height="16px" ImageUrl="~/Images/DeleteRow.png" ToolTip="Delete"
                                                    CommandArgument='<%#Eval("TransitId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td>
                            <div id="dvDelete" runat="server" visible="false" style="width: 250px; z-index: 200;
                                border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC;
                                border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute;
                                bottom: 0; height: 75px; left: 400px; top: 100px">
                                <table width="100%" cellspacing="2" cellpadding="0">
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="Label1" Font-Size="12px" runat="server" Font-Bold="true" Text="Delete this record ?" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnYes" SkinID="Button" runat="server" Text="Yes" Width="60px" OnClick="btnYes_OnClick" />
                                            &nbsp;
                                            <asp:Button ID="btnNo" SkinID="Button" runat="server" Text="No" Width="60px" OnClick="btnNo_OnClick" />
                                        </td>
                                        <td align="center">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="dvProcess" runat="server" AssociatedUpdatePanelID="upd1"
            DisplayAfter="5000" DynamicLayout="true">
            <ProgressTemplate>
                <center>
                    <div style="width: 154; position: absolute; bottom: 0; height: 60; left: 500px; top: 300px">
                        <img id="Img1" src="~/Images/loading.gif" alt="loading" runat="server" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    </form>
</body>
</html>
