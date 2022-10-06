<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopPanel.ascx.cs" Inherits="Include_Components_MasterComponent_TopPanel" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

        <script type="text/javascript">
            function returnToParent() {

            }

        </script>

        <div class="col-md-12">



            <ul class="list-inline top-user-list">
                <li>
                    <asp:Label ID="lblVersionNo" runat="server" CausesValidation="false" style="color:white" />
                </li>
                <li>
                    <asp:TextBox ID="txtSearchwithUTD" Width="80px" Height="20px" runat="server" SkinID="textbox"
                        Visible="false" placeholder="Search On UTD" ToolTip="Type in Textbox and Click here to Search on UpToDate" Style="float: left;" />
                    <asp:ImageButton ID="imgUTD" runat="server" ImageUrl="~/Icons/search-icon.png" Visible="false"
                        ToolTip="Type in Textbox and Click here to Search on UpToDate"
                        OnClick="imgUTD_OnClick" /></li>
                <li>
                    <div class="dropdown">
                        <button class="dropdown-toggle" id="menu1" type="button" data-toggle="dropdown" style="background: transparent; border: 0;">
                            <img src="../../../Images/PImageBackGround.gif" alt="Avatar" width="26" class="avatar img-circle" style="border: 1px solid #ccc;">
                            <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-right" role="menu" aria-labelledby="menu1">
                            <li role="presentation">
                                <asp:Label ID="lblPatCat" runat="server" class="hidden"></asp:Label>
                                <a href="#">
                                    <span id="lnkUser" runat="server" style="color: #333"></span></a></li>
                            <li role="presentation" class="divider"></li>
                            <li role="presentation">
                               <%-- <a id="a1" role="menuitem" tabindex="-1" href="#" runat="server" onclick="window.location='/login.aspx?Logout=1'; return false;" class="text-primary">Logout  </a>--%>
                                    <%--<img src="../../../Images/logout.svg" width="18" alt="logout" data-toggle="tooltip" data-placement="left" title="LogOut" />--%>
                                 <asp:LinkButton ID="btnLogout" runat="server" OnClick="btnLogout_Click">LogOut</asp:LinkButton>
                            </li>


                        </ul>
                    </div>
                </li>
                <li>
                    <asp:LinkButton ID="lnkChangeFacility" runat="server" ForeColor="#bad0ea" CssClass="btn btn-lg" OnClick="lnkChangeFacility_onClick"><i class="fas fa-map-marker-alt"></i></asp:LinkButton></li>

                <li>
                   
                    

                    
                </li>


            </ul>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow1" Skin="Metro" runat="server" Behaviors="Close,Move" />
                        </Windows>
                    </telerik:RadWindowManager>

        </div>

        <table border="0" cellpadding="0" cellspacing="0" style="text-align: right;" class="hidden">
            <tr align="left" valign="top">
                <td align="center" valign="middle"></td>
                <%-- <td align="left" style="width: 110px">
            <asp:LinkButton ID="lbkBtnSpouse" runat="server" CausesValidation="false" Text="Switch-Partner"
                ForeColor="White" Font-Underline="false" ToolTip="Switch to patient partner"
                OnClick="lbkBtnSpouse_OnClick" />
            <asp:Button ID="btnclose" runat="server" OnClick="BtnClose_OnClick" Width="1px" Style="visibility: hidden" />
        </td>--%>

                <td align="center" valign="middle">
                    <table cellpadding="1" cellspacing="1">
                        <tr align="center" valign="middle">
                            <td style="margin-right: 5px;">
                                <asp:ImageButton ID="imgMedicalAlert" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                    OnClick="imgMedicalAlert_OnClick" Width="23px" Height="23px" Visible="false"
                                    ToolTip="Medical Alert" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgAllergyAlert" runat="server" ImageUrl="~/Icons/allergy.gif"
                                    Width="23px" Height="23px" OnClick="imgAllergyAlert_OnClick" Visible="false"
                                    ToolTip="Allergy Alert" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgCaseSheet" runat="server" ImageUrl="~/Icons/CaseSheet.jpg"
                                    Width="23px" Height="23px" OnClick="imgCaseSheet_OnClick" Visible="false" ToolTip="Case Sheet" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgPastClinicalNote" runat="server" ImageUrl="~/Icons/notes.gif"
                                    Width="23px" Height="23px" OnClick="imgPastClinicalNote_OnClick" Visible="false"
                                    ToolTip="Past Clinical Notes" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgVisitHistory" runat="server" ImageUrl="~/Icons/VisitHistory.jpg"
                                    Width="23px" Height="23px" OnClick="imgVisitHistory_OnClick" Visible="false"
                                    ToolTip="Visit History" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgVitalHistory" runat="server" ImageUrl="~/Icons/PatientVitals.jpg"
                                    Width="23px" Height="23px" OnClick="imgVitalHistory_OnClick" Visible="false"
                                    ToolTip="Vital History" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgDiagnosticHistory" runat="server" ImageUrl="~/Icons/Investigation.jpg"
                                    Width="23px" Height="23px" OnClick="imgDiagnosticHistory_OnClick" Visible="false"
                                    ToolTip="Lab Results" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgXray" runat="server" ImageUrl="~/Icons/xray.jpg" Width="23px"
                                    Height="23px" OnClick="imgXray_OnClick" Visible="false" ToolTip="Radiology Results" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgImmunization" runat="server" ImageUrl="~/Icons/Immunization.jpg"
                                    Width="23px" Height="23px" OnClick="imgImmunization_OnClick" Visible="false"
                                    ToolTip="Patient Immunization" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgGrowthChart" runat="server" ImageUrl="~/Icons/GrowthChart.jpg"
                                    Width="23px" Height="23px" OnClick="imgGrowthChart_OnClick" Visible="false" ToolTip="Growth Chart" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgAttachment" runat="server" ImageUrl="~/Icons/Attachment.jpg"
                                    Width="23px" Height="23px" OnClick="imgAttachment_OnClick" Visible="false" ToolTip="Attachment" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgOTScheduler" runat="server" ImageUrl="~/Icons/OTScheduler.jpg"
                                    Width="23px" Height="23px" OnClick="imgOTScheduler_OnClick" Visible="false" ToolTip="OT Scheduler" />
                                <asp:ImageButton ID="imgDiagnosis" runat="server" ImageUrl="~/Icons/Diagnosis.jpg"
                                    Width="23px" Height="23px" OnClick="imgDiagnosis_Click" Visible="false" ToolTip="Diagnosis" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgFollowUpAppointment" runat="server" ImageUrl="~/Icons/FollowUpAppointment2.jpg"
                                    Width="23px" Height="23px" OnClick="imgFollowUpAppointment_OnClick" Visible="false"
                                    ToolTip="Follow Up Appointment" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgReferal" runat="server" ImageUrl="~/Images/icon_medical.jpg"
                                    Width="23px" Height="23px" OnClick="imgReferal_OnClick" Visible="false" ToolTip="New Referal" />
                            </td>
                            <td style="margin-right: 5px; display: inline-block">
                                <asp:ImageButton ID="imgRefrealHistory" runat="server" ImageUrl="~/Images/jcb.jpg"
                                    Width="23px" Height="23px" OnClick="imgRefrealHistory_OnClick" Visible="false"
                                    ToolTip="Referal History" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right" valign="top">
                    <div style="top: -8">
                        <table cellspacing="1">
                            <tr>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                    <%--   <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" Text="Switch-Partner"
                ForeColor="White" Font-Underline="false" ToolTip="Switch to patient partner"
                Visible="false" OnClick="lbkBtnSpouse_OnClick" />--%>
                </td>
                <td align="right" valign="middle" style="width: 60px"></td>
                <td align="center" valign="middle" style="width: 105px"></td>
            </tr>
        </table>


    

    </ContentTemplate>

</asp:UpdatePanel>
