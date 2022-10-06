using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Text;



public partial class Include_Components_MasterComponent_TopPanelNew : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    string path = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = HttpContext.Current.Request.Url.AbsoluteUri;
        path = url.Replace("http://", "");
        path = "http://" + path.Substring(0, path.IndexOf("/") + 1);

        //+ common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"])
        //    + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&FromWard=Y&OP_IP=I&Category=PopUp");


        //        <asp:HiddenField ID="hndDoctorID" runat="server" Value="" />
        //<asp:HiddenField ID="hndOPIP" runat="server" Value="" />
        //<asp:HiddenField ID="hndEncounterDate" runat="server" Value="" />


        hndDoctorID.Value = common.myStr(Session["DoctorID"]);
        hndOPIP.Value = common.myStr(Session["OPIP"]);
        hndEncounterDate.Value = common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd");

        hdnEncounterNo.Value = common.myStr(Session["EncounterNo"]);
        hdnimgAllergyAlertencounterid.Value = common.myStr(Session["EncounterId"]);
        hdnimgAllergyAlertRegistrationID.Value = common.myStr(Session["RegistrationID"]);
        hdnimgAllergyAlertPatientName.Value = common.myStr(Session["PatientName"]);
        hdnimgAllergyAlertRegistrationNo.Value = common.myStr(Session["RegistrationNo"]);
        hdnimgAllergyAlertAgeGender.Value = common.myStr(Session["AgeGender"]);
        hdnEncounterStatus.Value = common.myStr(Session["EncounterStatus"]);

        if (!IsPostBack)
        {
            lbkBtnSpouse.Visible = common.myBool(Session["IsIVFSpecialisation"]);

            BaseC.EMR objEmr = new BaseC.EMR(sConString);
            if (Session["UserID"] != null && Session["HospitalLocationID"] != null)
            {
                //// SqlDataReader objDr = (SqlDataReader)objEmr.GetEmployeeId(Convert.ToInt32(Session["UserID"]), Convert.ToInt16(Session["HospitalLocationID"]));
                // if (objDr.Read())
                // {                    
                BaseC.User valUser = new BaseC.User(sConString);
                string sUserName = string.Empty;
                sUserName = valUser.GetEmpName(Convert.ToInt32(Session["UserID"]));
                //lnkUser.InnerHtml = lnkUser.InnerHtml + "Welcome!&nbsp;" + common.myStr(Session["EmployeeName"]);
                lnkUser.InnerHtml = lnkUser.InnerHtml + "Welcome!&nbsp;" + common.myStr(sUserName);
                if (common.myStr(Session["ModuleName"]) == "Inventory" || common.myStr(Session["ModuleName"]) == "Inventory Reports" || common.myStr(Session["ModuleName"]) == "Inventory Setup")
                {
                    if (Convert.ToString(Session["StoreName"]) != "")
                        lnkUser.InnerHtml = lnkUser.InnerHtml + " (" + Convert.ToString(Session["StoreName"]) + ")";
                }
                if (common.myStr(Session["FacilityName"]) != "")
                    lnkUser.InnerHtml = lnkUser.InnerHtml + " - " + common.myStr(Session["FacilityName"]);
                //}
                //objDr.Close();

                lnkUser.InnerHtml = lnkUser.InnerHtml + " (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ')' + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  IP Address  " + common.myStr(HttpContext.Current.Request.UserHostAddress);
            }
            if (common.myInt(Session["EncounterId"]) > 0 && (common.myInt(Session["ModuleId"]) == 3 || common.myStr(Session["ModuleName"]) == "EMR"))
            {
                //imgVisitHistory.Visible = true;
                imgVitalHistory.Visible = true;
                imgDiagnosticHistory.Visible = true;
                imgXray.Visible = true;
                imgImmunization.Visible = true;
                imgGrowthChart.Visible = true;
                imgAttachment.Visible = true;
                imgOTScheduler.Visible = false;
                imgFollowUpAppointment.Visible = true;
                imgCaseSheet.Visible = true;
                imgPastClinicalNote.Visible = true;
                imgGoToUpToDate.Visible = false;
                txtUptodateSearch.Visible = false;

                hdnNewUploadSite.Value = common.myStr(Session["EmployeeId"]);

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
    }

    protected void lnkbtnRadWindow_Click(object sender, EventArgs e)
    {

    }

    protected void lbkBtnSpouse_OnClick(object sender, EventArgs e)
    {
        getSpouseRegistrationId();
    }
    protected void imgPastClinicalNote_OnClick(object sender, EventArgs e)
    {
        //Session["SelectedCaseSheet"] = "PN";
        //callPopup("/WardManagement/VisitHistory.aspx?Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"])
        //    + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&FromWard=Y&OP_IP=I&Category=PopUp");

    }


    protected void imgPastClinicalNote_Click(object sender, ImageClickEventArgs e)
    {
        Session["SelectedCaseSheet"] = "PN";
    }

    protected void getSpouseRegistrationId()
    {
        try
        {
            if (common.myInt(Session["RegistrationId"]) == 0)
            {
                Alert.ShowAjaxMsg("Patient not selected !", this.Page);
                return;
            }

            clsIVF objivf = new clsIVF(sConString);

            DataSet ds = new DataSet();

            ds = objivf.getIVFRegistrationId(common.myInt(Session["IVFId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["RegistrationId"] = common.myInt(ds.Tables[0].Rows[0]["RegistrationId"]);
                Session["EncounterId"] = common.myInt(ds.Tables[0].Rows[0]["EncounterId"]);
                Session["IVFId"] = common.myInt(ds.Tables[0].Rows[0]["IVFId"]);
                Session["IVFNo"] = common.myInt(ds.Tables[0].Rows[0]["IVFNo"]);

                Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
            }
        }
        catch
        {
        }
    }

    protected void imgVisitHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/emr/Masters/PatientHistory.aspx?MP=NO");
    }

    protected void imgVitalHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/Vitals/PreviousVitals.aspx?POPUP=StaticTemplate");
    }

    protected void imgImmunization_OnClick(object sender, EventArgs e)
    {
        callPopup("~/EMR/Immunization/ImmunizationBabyDueDate.aspx?From=POPUP");
    }
    protected void imgGrowthChart_OnClick(object sender, EventArgs e)
    {
        // callPopup("/EMR/Vitals/GrowthChart.aspx?MP=NO");
    }

    protected void imgDiagnosticHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["encounterid"]) +
                                        "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=LIS" + "&Station=All");
    }

    protected void imgFollowUpAppointment_OnClick(object sender, EventArgs e)
    {
        //if (!common.myStr(Session["EncounterStatus"]).Trim().ToUpper().Contains("CLOSE"))
        //{
        //    callPopup("/Appointment/AppScheduler_New.aspx?MASTER=NO");
        //}
        //else
        //{
        //    Alert.ShowAjaxMsg("Encounter is Close. Not allowed Follow-up Appointment.", Page);
        //    return;
        //}
    }
    protected void imgRadiology_OnClick(object sender, EventArgs e)
    {
        callPopup("/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["encounterid"]) +
                                        "&RegNo=" + common.myStr(Session["RegistrationNo"]));
    }
    protected void imgAttachment_OnClick(object sender, EventArgs e)
    {
        //callPopup("/EMR/AttachDocument.aspx?MASTER=No");
        callPopup("/EMR/AttachDocumentFTP.aspx?MASTER=No");
    }
    protected void imgOTScheduler_OnClick(object sender, EventArgs e)
    {
        //if (!common.myStr(Session["EncounterStatus"]).Trim().ToUpper().Contains("CLOSE"))
        //{
        callPopup("/OTScheduler/OTAppointment.aspx?From=POPUP");
        //}
        //else
        //{
        //    Alert.ShowAjaxMsg("Encounter is Close. Not allowed OT Schedule.", Page);
        //    return;
        //}
    }
    protected void imgXray_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["encounterid"]) +
                                        "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS&Station=All");
    }

    protected void imgAllergyAlert_OnClick(object sender, EventArgs e)
    {
        //RadWindowForNew.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&EId=" 
        //    + common.myStr(Session["encounterid"]) + "&PId=" 
        //    + common.myStr(Session["RegistrationID"]) + "&PN=" 
        //    + common.myStr(Session["PatientName"]) + "&PNo=" 
        //    + common.myStr(Session["RegistrationNo"]) + "&PAG=" 
        //    + common.myStr(Session["AgeGender"]) + "";
        //RadWindowForNew.Width = 800;
        //RadWindowForNew.Height = 400;
        //RadWindowForNew.Top = 10;
        //RadWindowForNew.Left = 10;

        //RadWindowForNew.Modal = true;
        //RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        //RadWindowForNew.VisibleStatusbar = false;

        //string url = "../MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "";



        /* r& d  start*/
        //string url = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "";

        //string s = "window.open('" + url + "', 'popup_window', 'width=800,height=400,left=10,top=10);";
        ////Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        ////ScriptManager.RegisterClientScriptBlock(this, typeof(string), "OPEN_WINDOW", s, true);
        //ScriptManager.RegisterStartupScript(UpdatePanel, UpdatePanel.GetType(), "OpenWindow", s, true);
        ////ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


        ////Response.Redirect("~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "");
        /* r& d  end*/
    }

    protected void imgMedicalAlert_OnClick(object sender, EventArgs e)
    {
        //RadWindowForNew.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "";
        //RadWindowForNew.Width = 800;
        //RadWindowForNew.Height = 400;
        //RadWindowForNew.Top = 10;
        //RadWindowForNew.Left = 10;

        //RadWindowForNew.Modal = true;
        //RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        //RadWindowForNew.VisibleStatusbar = false;


        string url = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "";
        string s = "window.open('" + url + "', 'popup_window', 'width=1200,height=630,left=10,top=10);";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

    }

    protected void imgCaseSheet_OnClick(object sender, EventArgs e)
    {
        callPopup("/Editor/WordProcessor.aspx?From=POPUP&DoctorId=" + common.myInt(Session["DoctorID"]) + "&OPIP=" + common.myStr(Session["OPIP"])
                                            + "&EncounterDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
    }


    private void callPopup(string url)
    {
        try
        {
            if (common.myInt(Session["RegistrationID"]) == 0)
            {
                Alert.ShowAjaxMsg("Patient not selected !", this.Page);
                return;
            }
            if (common.myInt(Session["encounterid"]) == 0)
            {
                Alert.ShowAjaxMsg("Patient not selected !", this.Page);
                return;
            }

            RadWindowForNew.NavigateUrl = url;

            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            //RadWindowForNew.OnClientClose = "";
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch
        {
        }
    }




    protected void imgCaseSheet_Click(object sender, EventArgs e)
    {
        try
        {
            //////HtmlControl contentPanel1 = (HtmlControl)this.FindControl("ifrmpage");

            //////contentPanel1.Attributes["src"] =path + "Editor/WordProcessor.aspx?From=POPUP&DoctorId=" + common.myStr(Session["DoctorID"]) + "&OPIP=" + common.myStr(Session["OPIP"]) + "&EncounterDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd");
            //////System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Case Sheet');", true);

            //////System.Web.UI.HtmlControls.HtmlGenericControl ifrmpage = null;
            //////Control ctl = this.Parent;
            //////HtmlGenericControl gc = new HtmlGenericControl();

            //System.Web.UI.WebControls.Literal ltMetaTags = null;
            //  Control ctl = this.Parent;


            string url = path + "Editor/WordProcessor.aspx?From=POPUP&DoctorId=" + common.myStr(Session["DoctorID"]) + "&OPIP=" + common.myStr(Session["OPIP"]) + "&EncounterDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd");
            ifrmpage.Attributes["src"] = path + "Editor/WordProcessor.aspx?From=POPUP&DoctorId=" + common.myStr(Session["DoctorID"]) + "&OPIP=" + common.myStr(Session["OPIP"]) + "&EncounterDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Case Sheet');", true);

        }
        catch (Exception ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + ex.Message;
            //objException.HandleException(ex);

        }

    }

}
