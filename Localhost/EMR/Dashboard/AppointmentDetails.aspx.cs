using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

public partial class EMR_Dashboard_AppointmentDeatils : System.Web.UI.Page
{

    clsExceptionLog objException = new clsExceptionLog();
    string path = string.Empty;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        Page.MasterPageFile = "/Include/Master/AppointmentDetails.master";

        //if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
        //{
        //    Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        //}
        //else
        //{
        //    Page.MasterPageFile = "/Include/Master/EMRMasterWithTopDetails_1.master";
        // }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (common.myInt(Session["EncounterId"]) > 0 && (common.myInt(Session["ModuleId"]) == 3 || common.myStr(Session["ModuleName"]) == "EMR"))
        //{
        if ((common.myInt(Session["ModuleId"]) == 3 || common.myStr(Session["ModuleName"]) == "EMR"))
        {

            imgVitalHistory.Visible = true;
            imgDiagnosticHistory.Visible = true;
            imgXray.Visible = true;
            //imgImmunization.Visible = true;
            imgGrowthChart.Visible = true;
            imgAttachment.Visible = true;
            imgPastClinicalNote.Visible = true;

            if (common.myBool(Session["AllergiesAlert"]))
            {
                imgAllergyAlert.Visible = true;
            }

            if (common.myBool(Session["MedicalAlert"]))
            {
                imgMedicalAlert.Visible = true;
            }
        }
    }
    protected void imgPastClinicalNote_OnClick(object sender, EventArgs e)
    {
        try
        {
            //ifrmpage.Attributes["src"] = path + "WardManagement/VisitHistory.aspx?Regid=" + common.myStr(Session["RegistrationID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&EncId=" + common.myStr(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&FromWard=Y&OP_IP=I&Category=PopUp&CloseButtonShow=No";

            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Past Clinical Notes');", true);

            RadWindowForNew.NavigateUrl = "~/WardManagement/VisitHistory.aspx?Regid=" + common.myStr(Session["RegistrationID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&EncId=" + common.myStr(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&FromWard=Y&OP_IP=I&Category=PopUp&CloseButtonShow=No&CloseButtonShow=No&FromEMR=0";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;

            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }

    }

    protected void imgVitalHistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            //ifrmpage.Attributes["src"] = path + "EMR/Vitals/PreviousVitals.aspx?POPUP=StaticTemplate&CloseButtonShow=No";

            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Vital History');", true);

            RadWindowForNew.NavigateUrl = "~/EMR/Vitals/PreviousVitals.aspx?POPUP=StaticTemplate&CloseButtonShow=No&FromEMR=0";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;

            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }

    }

    protected void imgDiagnosticHistory_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (Session["imgHospitalLogo"] != null)
            {
                if (common.myStr(Session["imgHospitalLogo"]) != string.Empty)
                {
                    if (common.myStr(Session["imgHospitalLogo"]).Contains("AlkindiLogo"))
                    {
                        // ifrmpage.Attributes["src"] = path + "EMR/WindowsApplicationDetails.aspx?CF=&Master=Blank&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&CloseButtonShow = No";

                        RadWindowForNew.NavigateUrl = "~/EMR/WindowsApplicationDetails.aspx?CF=&Master=Blank&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&CloseButtonShow = No&FromEMR=0";
                        RadWindowForNew.Width = 1200;
                        RadWindowForNew.Height = 630;
                        RadWindowForNew.Top = 10;
                        RadWindowForNew.Left = 10;

                        RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;

                    }
                    else
                    {
                        //  ifrmpage.Attributes["src"] = path + "EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=LIS&Station=All&CloseButtonShow=No";

                        RadWindowForNew.NavigateUrl = "~/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=LIS&Station=All&CloseButtonShow=No&FromEMR=0";
                        RadWindowForNew.Width = 1200;
                        RadWindowForNew.Height = 630;
                        RadWindowForNew.Top = 10;
                        RadWindowForNew.Left = 10;

                        RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
                    }
                }
                else
                {
                    // ifrmpage.Attributes["src"] = path + "EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=LIS&Station=All&CloseButtonShow=No";

                    RadWindowForNew.NavigateUrl = "~/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=LIS&Station=All&CloseButtonShow=No&FromEMR=0";
                    RadWindowForNew.Width = 1200;
                    RadWindowForNew.Height = 630;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;

                    RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
                }

            }
            else
            {
                //  ifrmpage.Attributes["src"] = path + "EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=LIS&Station=All&CloseButtonShow=No";

                RadWindowForNew.NavigateUrl = "~/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=LIS&Station=All&CloseButtonShow=No&FromEMR=0";
                RadWindowForNew.Width = 1200;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;

                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
            }
            //ifrmpage.Attributes["src"] = path + "EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=LIS&Station=All&CloseButtonShow=No";
            //  ifrmpage.Attributes["src"] = path + "EMR/WindowsApplicationDetails.aspx?CF=&Master=Blank&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&CloseButtonShow = No";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Lab Results');", true);

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }

    }
    protected void imgXray_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (Session["imgHospitalLogo"] != null)
            {
                if (common.myStr(Session["imgHospitalLogo"]) != string.Empty)
                {
                    if (common.myStr(Session["imgHospitalLogo"]).Contains("AlkindiLogo"))
                    {
                        //  ifrmpage.Attributes["src"] = path + "EMR/WindowsApplicationDetails.aspx?CF=&Master=Blank&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&FlagDepatment=XR" + "&CloseButtonShow = No";

                        RadWindowForNew.NavigateUrl = "~/EMR/WindowsApplicationDetails.aspx?CF=&Master=Blank&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&FlagDepatment=XR" + "&CloseButtonShow = No&FromEMR=0";
                        RadWindowForNew.Width = 1200;
                        RadWindowForNew.Height = 630;
                        RadWindowForNew.Top = 10;
                        RadWindowForNew.Left = 10;

                        RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
                    }
                    else
                    {
                        //    ifrmpage.Attributes["src"] = path + "EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS&Station=All&CloseButtonShow=No";

                        RadWindowForNew.NavigateUrl = "~/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS&Station=All&CloseButtonShow=No&FromEMR=0";
                        RadWindowForNew.Width = 1200;
                        RadWindowForNew.Height = 630;
                        RadWindowForNew.Top = 10;
                        RadWindowForNew.Left = 10;

                        RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
                    }
                }
                else
                {
                    //   ifrmpage.Attributes["src"] = path + "EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS&Station=All&CloseButtonShow=No";

                    RadWindowForNew.NavigateUrl = "~/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS&Station=All&CloseButtonShow=No&FromEMR=0";
                    RadWindowForNew.Width = 1200;
                    RadWindowForNew.Height = 630;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;

                    RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
                }

            }
            else
            {
                //   ifrmpage.Attributes["src"] = path + "EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS&Station=All&CloseButtonShow=No";

                RadWindowForNew.NavigateUrl = "~/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS&Station=All&CloseButtonShow=No&FromEMR=0";
                RadWindowForNew.Width = 1200;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;

                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
            }

            //   ifrmpage.Attributes["src"] = path + "EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS&Station=All&CloseButtonShow=No";
            //    ifrmpage.Attributes["src"] = path + "EMR/WindowsApplicationDetails.aspx?CF=&Master=Blank&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&FlagDepatment=XR" + "&CloseButtonShow = No";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Radiology Results');", true);

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }


    }
    //protected void imgImmunization_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //ifrmpage.Attributes["src"] = path + "EMR/Immunization/ImmunizationBabyDueDate.aspx?From=POPUP&CloseButtonShow=No";

    //        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Injection Details');", true);

    //        RadWindowForNew.NavigateUrl = "~/EMR/Immunization/ImmunizationBabyDueDate.aspx?From=POPUP&CloseButtonShow=No";
    //        RadWindowForNew.Width = 1200;
    //        RadWindowForNew.Height = 630;
    //        RadWindowForNew.Top = 10;
    //        RadWindowForNew.Left = 10;

    //        RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
    //        RadWindowForNew.Modal = true;
    //        RadWindowForNew.VisibleStatusbar = false;
    //        RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;

    //    }
    //    catch (Exception ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + ex.Message;
    //        objException.HandleException(ex);

    //    }


    //}
    protected void imgGrowthChart_OnClick(object sender, EventArgs e)
    {
        try
        {
            //ifrmpage.Attributes["src"] = path + "EMR/Vitals/GrowthChart.aspx?MP=NO&CloseButtonShow=No";

            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Growth Chart');", true);


            RadWindowForNew.NavigateUrl = "~/EMR/Vitals/GrowthChart.aspx?MP=NO&CloseButtonShow=No&FromEMR=0";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;

            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }


    }
    protected void imgAttachment_OnClick(object sender, EventArgs e)
    {
        try
        {
            //ifrmpage.Attributes["src"] = path + "EMR/AttachDocument.aspx?MASTER=No&CloseButtonShow=No&FromEMR=1";

            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Attach Document');", true);

            if (Session["imgHospitalLogo"] != null && !common.myStr(Session["imgHospitalLogo"]).Equals(string.Empty))
            {
                if (common.myStr(Session["imgHospitalLogo"]).Contains("AlkindiLogo"))
                {
                    RadWindowForNew.NavigateUrl = "~/EMR/AttachDocumentAlkindi.aspx?MASTER=No&CloseButtonShow=No&FromEMR=0";
                }
                else
                {
                    //RadWindowForNew.NavigateUrl = "~/EMR/AttachDocument.aspx?MASTER=No&CloseButtonShow=No&FromEMR=0";
                    RadWindowForNew.NavigateUrl = "~/EMR/AttachDocumentFTP.aspx?MASTER=No&CloseButtonShow=No&FromEMR=0";
                }
            }
            else
            {
                //RadWindowForNew.NavigateUrl = "~/EMR/AttachDocument.aspx?MASTER=No&CloseButtonShow=No&FromEMR=0"; 
                RadWindowForNew.NavigateUrl = "~/EMR/AttachDocumentFTP.aspx?MASTER=No&CloseButtonShow=No&FromEMR=0";
            }
         
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;

            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }
    protected void imgAllergyAlert_Click(object sender, EventArgs e)
    {
        try
        {
            //ifrmpage.Attributes["src"] = path + "MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&EId=" + common.myStr(Session["EncounterId"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "&CloseButtonShow=No";

            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Allergy Alert');", true);


            RadWindowForNew.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&EId=" + common.myStr(Session["EncounterId"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "&CloseButtonShow=No&FromEMR=0";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;

            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
    }
    protected void imgMedicalAlert_OnClick(object sender, EventArgs e)
    {
        //ifrmpage.Attributes["src"] = path + "MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "&CloseButtonShow=No";

        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Medical Alert');", true);

        RadWindowForNew.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "&CloseButtonShow=No&FromEMR=0";
        RadWindowForNew.Width = 1200;
        RadWindowForNew.Height = 630;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;

        RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;


    }


}