<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopPanelNew.ascx.cs" Inherits="Include_Components_MasterComponent_TopPanelNew" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
<%--<script type="text/javascript" >
    function ShowPAgeO(doctitl) {
        var str = doctitl;
        var temp = new Array();
        temp = str.split(",");
        var Header = temp[0];
        var ButtonId = temp[1];
        debugger;
        jQuery(function($) {
            $('#pagepopup').dialog({
                //autoopen: true,
                width: '100%',
                height: 650,
                title: Header,
                skin: 'common',
                draggable: false,
                resizable: false,
                closeOnEscape: false,
                model: true,
                dialogClass: "no-close",
                bgiframe: true,
                buttons: [
                     {
                         text: "Close",
                         click: function() {
                             $(this).dialog("close");
                            
                         }

                     }
                   ]
            }).parent().find('.ui-dialog-titlebar-close').hide();

        });
    };
    </script>--%>
        <script language="javascript" type="text/javascript">
            function OpenPageUpToDate() {
                var DocId = $get('<%=hdnNewUploadSite.ClientID%>').value;
                var search = $get('<%=txtUptodateSearch.ClientID%>').value;
                window.open("http://www.uptodate.com/contents/search?srcsys=HMGR374606&id=" + DocId + "&search=" + search);
            }

            function imgAllergyAlert() {
                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
            
                var popup;

                popup = window.open("/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&EId="
                + $get('<%=hdnimgAllergyAlertencounterid.ClientID%>').value + "&PId="
                + $get('<%=hdnimgAllergyAlertRegistrationID.ClientID%>').value + "&PN="
                 + $get('<%=hdnimgAllergyAlertPatientName.ClientID%>').value + "&PNo="
                + $get('<%=hdnimgAllergyAlertRegistrationNo.ClientID%>').value + "&PAG="
                + $get('<%=hdnimgAllergyAlertAgeGender.ClientID%>').value
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status
                popup.focus();
                return false
            }
            function imgMedicalAlert() {
                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
            
                var popup;
                popup = window.open("/MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&EId="
                + $get('<%=hdnimgAllergyAlertencounterid.ClientID%>').value + "&PId="
                + $get('<%=hdnimgAllergyAlertRegistrationID.ClientID%>').value + "&PN="
                 + $get('<%=hdnimgAllergyAlertPatientName.ClientID%>').value + "&PNo="
                + $get('<%=hdnimgAllergyAlertRegistrationNo.ClientID%>').value + "&PAG="
                + $get('<%=hdnimgAllergyAlertAgeGender.ClientID%>').value
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status
                popup.focus();
                return false
            }
            function imgFollowUpAppointment() {
                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
                 var popup;
                    popup = window.open("/Appointment/AppScheduler_New.aspx?MASTER=NO"
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status
                popup.focus();
                return false
            }

            function imgAttachment_OnClick() {
                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
                var popup;

                //popup = window.open("/EMR/AttachDocument.aspx?MASTER=No"
                //, "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                popup = window.open("/EMR/AttachDocumentFTP.aspx?MASTER=No"
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status


                popup.focus();
                return false
            }

            function imgGrowthChart_OnClick() {
                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
                var popup;

                popup = window.open("/EMR/Vitals/GrowthChart.aspx?MP=NO"
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status


                popup.focus();
                return false
            }

            function imgImmunization_OnClick() {
                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;

                var popup;

                popup = window.open("/EMR/Immunization/ImmunizationBabyDueDate.aspx?From=POPUP"
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status


                popup.focus();
                return false
            }
            function imgVitalHistory_OnClick() {

                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
                var popup;

                popup = window.open("/EMR/Vitals/PreviousVitals.aspx?POPUP=StaticTemplate"
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status


                popup.focus();
                return false
            }

            function imgVisitHistory_OnClick() {
                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
                var popup;

                popup = window.open("/emr/Masters/PatientHistory.aspx?MP=NO"
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status
                popup.focus();
                return false
            }

            function imgPastClinicalNote_OnClick() {

                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
                var popup;

                popup = window.open("/WardManagement/VisitHistory.aspx?Regid="
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                popup = window.open("/WardManagement/VisitHistory.aspx?Regid="
                 + $get('<%=hdnimgAllergyAlertRegistrationID.ClientID%>').value + "&RegNo="
                 + $get('<%=hdnimgAllergyAlertRegistrationNo.ClientID%>').value + "&EncId="
                 + $get('<%=hdnimgAllergyAlertencounterid.ClientID%>').value + "&EncNo="
                 + $get('<%=hdnEncounterNo.ClientID%>').value + "&FromWard=Y&OP_IP=I&Category=PopUp"
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status

                popup.focus();
                return false
            }

            function imgCaseSheet_OnClick() {

                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
               

                var popup;
                popup = window.open("/Editor/WordProcessor.aspx?From=POPUP&DoctorId="
                 + $get('<%=hndDoctorID.ClientID%>').value + "&OPIP="
                  + $get('<%=hndOPIP.ClientID%>').value + "&EncounterDate="
                   + $get('<%=hndEncounterDate.ClientID%>').value
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status

                popup.focus();
                return false
            }



            function imgDiagnosticHistory_OnClick() {

                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
                var popup;

                popup = window.open("/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId="
                 + $get('<%=hdnimgAllergyAlertencounterid.ClientID%>').value + "&RegNo="
                 + $get('<%=hdnimgAllergyAlertRegistrationNo.ClientID%>').value + "&Source="
                 + $get('<%=hndOPIP.ClientID%>').value + "&Flag=LIS&Station=All"
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");
                
                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status


                popup.focus();
                return false
            }
            
            
            function imgXray_OnClick() {

                var x = screen.width / 2 - 1200 / 2;
                var y = screen.height / 2 - 550 / 2;
                var popup;
             
                popup = window.open("/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId="
                 + $get('<%=hdnimgAllergyAlertencounterid.ClientID%>').value + "&RegNo="
                 + $get('<%=hdnimgAllergyAlertRegistrationNo.ClientID%>').value + "&Source="
                 + $get('<%=hndOPIP.ClientID%>').value + "&Flag=RIS&Station=All"
                , "Popup", "height=550,width=1200,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");
                
                dom.disable_window_open_feature.location
                dom.disable_window_open_feature.resizable
                dom.disable_window_open_feature.status


                popup.focus();
                return false
            }
            
        </script>


        <table border="0" cellpadding="0" cellspacing="0" width="800px">
            <tr align="left" valign="top">

                    
                <span id="lnkUser" runat="server" visible="false"></span>
                <td align="left" valign="middle">
                    <asp:ImageButton ID="imgPatientAlert" runat="server" ImageUrl="~/Icons/PatientAlert.gif" Width="23px" Height="23px" CssClass="iconEMRimg" Visible="false" ToolTip="Patient Alert" />
                   <%-- <asp:ImageButton ID="imgCaseSheet" runat="server" ImageUrl="~/imagesHTML/CaseSheet.gif" Width="23px" Height="23px" CssClass="iconEMRimg" OnClientClick="imgCaseSheet_OnClick()" Visible="false" ToolTip="Case Sheet" />--%>
                    <asp:ImageButton ID="imgCaseSheet" runat="server" ImageUrl="~/imagesHTML/CaseSheet.gif" Width="23px" Height="23px" CssClass="iconEMRimg"  OnClick="imgCaseSheet_Click" ToolTip="Case Sheet" />
                    <asp:ImageButton ID="imgPastClinicalNote" runat="server" ImageUrl="~/imagesHTML/PastClinicalNotes.png" OnClientClick="imgPastClinicalNote_OnClick()" CssClass="iconEMRimg" Width="23px" Height="23px" Visible="false" ToolTip="Past Clinical Notes"  onclick="imgPastClinicalNote_Click" />
                    <asp:ImageButton ID="imgVitalHistory" runat="server" ImageUrl="~/imagesHTML/Vitals.gif" Width="23px" Height="23px" CssClass="iconEMRimg" OnClientClick="imgVitalHistory_OnClick()" Visible="false" ToolTip="Vital History" />
                    
                    <asp:ImageButton ID="imgDiagnosticHistory" runat="server" ImageUrl="~/Icons/Investigation.jpg" Width="23px" Height="23px" CssClass="iconEMRimg" OnClientClick="imgDiagnosticHistory_OnClick()"  Visible="false" ToolTip="Lab Results" />
                    <asp:ImageButton ID="imgXray" runat="server" ImageUrl="~/imagesHTML/Radiology-Results.png" Width="23px" Height="23px" CssClass="iconEMRimg" OnClientClick="imgXray_OnClick()" Visible="false" ToolTip="Radiology Results" />
                    
<%--                    <asp:ImageButton ID="imgDiagnosticHistory" runat="server" ImageUrl="~/Icons/Investigation.jpg" Width="23px" Height="23px" CssClass="iconEMRimg" OnClick="imgDiagnosticHistory_OnClick" Visible="false" ToolTip="Lab Results" />
                    <asp:ImageButton ID="imgXray" runat="server" ImageUrl="~/imagesHTML/Radiology-Results.png" Width="23px" Height="23px" CssClass="iconEMRimg" OnClick="imgXray_OnClick" Visible="false" ToolTip="Radiology Results" />--%>
                    <asp:ImageButton ID="imgImmunization" runat="server" ImageUrl="~/imagesHTML/Patient-Immunization.png" Width="23px" Height="23px" CssClass="iconEMRimg" OnClientClick="imgImmunization_OnClick()" Visible="false" ToolTip="Patient Immunization" />
                    <asp:ImageButton ID="imgGrowthChart" runat="server" ImageUrl="~/imagesHTML/Growth_Charts.gif" Width="23px" Height="23px" CssClass="iconEMRimg" OnClientClick="imgGrowthChart_OnClick()" Visible="false" ToolTip="Growth Chart" />
                    <asp:ImageButton ID="imgAttachment" runat="server" ImageUrl="~/imagesHTML/Attachments.gif" Width="23px" Height="23px" CssClass="iconEMRimg"  Visible="false" ToolTip="Attachment"   OnClientClick="imgAttachment_OnClick()"  />
                    <asp:ImageButton ID="imgOTScheduler" runat="server" ImageUrl="~/imagesHTML/OT-Scheduler.png" Width="23px" Height="23px" CssClass="iconEMRimg" OnClick="imgOTScheduler_OnClick" Visible="false" ToolTip="OT Scheduler" />
                    <asp:ImageButton ID="imgFollowUpAppointment" runat="server" ImageUrl="~/imagesHTML/FollowUpAppointment.gif" Width="23px" Height="23px" CssClass="iconEMRimg"  Visible="false" ToolTip="Follow-up Appointment"   OnClientClick="imgFollowUpAppointment()" />
                    <asp:HiddenField ID="hdnEncounterStatus" runat="server" Value="" />
                    <asp:TextBox ID="txtUptodateSearch" runat="server" Visible="false" CssClass="searchDiv" AutoComplete="off"></asp:TextBox>
                    <asp:ImageButton ID="imgMedicalAlert" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"  OnClientClick="imgMedicalAlert()" CssClass="iconEMRimg01" Width="23px" Height="23px" Visible="false" ToolTip="Patient Alert" />
                    <asp:ImageButton ID="imgAllergyAlert" runat="server" ImageUrl="~/Icons/allergy.gif"  CssClass="iconEMRimg" Width="23px" Height="23px" Visible="false" ToolTip="Allergy Alert"  OnClientClick="imgAllergyAlert()"  />
                    <asp:HiddenField ID="hndDoctorID" runat="server" Value="" />
                    <asp:HiddenField ID="hndOPIP" runat="server" Value="" />
                    <asp:HiddenField ID="hndEncounterDate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="" />
                    <asp:HiddenField ID="hdnimgAllergyAlertencounterid" runat="server" Value="" />
                    <asp:HiddenField ID="hdnimgAllergyAlertRegistrationID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnimgAllergyAlertPatientName" runat="server" Value="" />
                    <asp:HiddenField ID="hdnimgAllergyAlertRegistrationNo" runat="server" Value="" />
                    <asp:HiddenField ID="hdnimgAllergyAlertAgeGender" runat="server" Value="" />
                    <asp:ImageButton ID="imgGoToUpToDate" runat="server" ImageUrl="~/Icons/GoToUpToDateNew.png" CssClass="btn btn-primary searchDivBtn" OnClientClick="OpenPageUpToDate();" Visible="false" ToolTip="Go To UpToDate" />
                    <asp:HiddenField ID="hdnNewUploadSite" runat="server" Value="" />
                    <asp:LinkButton ID="lbkBtnSpouse" runat="server" CausesValidation="false" Text="Switch-Partner" ForeColor="White" Font-Underline="false" ToolTip="Switch to patient partner" OnClick="lbkBtnSpouse_OnClick" />
                </td>
                
                <td>
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows><telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Maximize, Pin, Move, Reload, Close" /></Windows>
                    </telerik:RadWindowManager>
                </td>
            </tr>
        </table>
     
        <%--<div id="pagepopup" title="ifrmpage.Page.Title"  style="display: none;background-color:lightblue;width:100%;height:1000px; "   >
          <iframe id="ifrmpage" runat ="server" width ="100%" height ="100%" style="border-bottom-style:none; " frameborder="0" >

          </iframe>
          </div>--%>

        <div id="pagepopup"  style="display: none;background-color:lightblue;width:100%;height:1000px; "   >
          <iframe id="ifrmpage" runat ="server" width ="100%" height ="100%" style="border-bottom-style:none; " >

          </iframe>
          </div>
         
    </ContentTemplate>
</asp:UpdatePanel>
