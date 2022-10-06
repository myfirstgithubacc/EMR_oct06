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

public partial class Include_Components_MasterComponent_TopPanel : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string UtdLink = ConfigurationManager.ConnectionStrings["UTDLink"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserID"] != null && Session["HospitalLocationID"] != null)
        {
            lnkUser.InnerHtml = "";
            lnkUser.InnerHtml = lnkUser.InnerHtml + "Welcome!&nbsp;" + common.myStr(Session["EmployeeName"]);

            if (common.myStr(Session["ModuleName"]).ToUpper().Trim().Equals("INVENTORY")
                || common.myStr(Session["ModuleName"]).ToUpper().Trim().Equals("INVENTORY REPORTS")
                || common.myStr(Session["ModuleName"]).ToUpper().Trim().Equals("INVENTORY SETUP"))
            {
                if (!common.myStr(Session["StoreName"]).Equals(string.Empty))
                {
                    lnkUser.InnerHtml = lnkUser.InnerHtml + " (" + common.myStr(Session["StoreName"]) + ")";
                }
            }
            if (!common.myStr(Session["FacilityName"]).Equals(string.Empty))
            {
                lnkUser.InnerHtml = lnkUser.InnerHtml + " - " + common.myStr(Session["FacilityName"]);
            }
        }
        if (!IsPostBack)
        {
            //lbkBtnSpouse.Visible = common.myBool(Session["IsIVFSpecialisation"]);
            lblVersionNo.Text = common.myStr(Session["ApplicationVersion"]);
            if (common.myStr(lblVersionNo.Text).Equals(""))
            {
                DataSet ds;
                ds = new DataSet();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hstInput = new Hashtable();
                hstInput.Add("@sApplicationName", "EMR");
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetApplicationDetail", hstInput);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (!common.myStr(ds.Tables[0].Rows[0]["VersionNo"]).Equals(""))
                        {
                            lblVersionNo.Text = "Ver# " + common.myStr(ds.Tables[0].Rows[0]["VersionNo"]);
                            Session["ApplicationVersion"] = "Ver# " + common.myStr(ds.Tables[0].Rows[0]["VersionNo"]);
                        }
                    }
                }
                objDl = null;
                ds.Dispose();


            }


            Label lbl = lblPatCat;
            lbl.Text = common.myStr(Session["lblpatcat"]);
            if (lbl.Text.ToUpper().Contains("CASH"))
            {
                lbl.BackColor = System.Drawing.Color.AliceBlue;
                lbl.ForeColor = System.Drawing.Color.Black;
            }
            else if (lbl.Text.ToUpper().Contains("PREMIUM"))
            {
                lbl.BackColor = System.Drawing.Color.Goldenrod;
                lbl.ForeColor = System.Drawing.Color.Goldenrod;
            }
            else if (lbl.Text.ToUpper().Contains("PRIVILEGE"))
            {
                lbl.BackColor = System.Drawing.Color.Green;
                lbl.ForeColor = System.Drawing.Color.Green;
            }

            //else if (lbl.Text.ToUpper().Contains("EL"))
            //{
            //    lbl.BackColor = System.Drawing.Color.LightBlue;
            //    lbl.ForeColor = System.Drawing.Color.LightBlue;
            //}
            else if (lbl.Text.ToUpper().Contains("EL"))
            {
                lbl.BackColor = System.Drawing.Color.LightYellow;
                lbl.ForeColor = System.Drawing.Color.LightYellow;
            }
            else if (lbl.Text.ToUpper().Contains("Moderate"))
            {
                lbl.BackColor = System.Drawing.Color.PaleGoldenrod;
                lbl.ForeColor = System.Drawing.Color.PaleGoldenrod;
                //e.Row.Cells[e.Row.Cells.Count - 1].ForeColor = System.Drawing.Color.White;
            }
            else if (lbl.Text.ToUpper().Contains("LOW"))
            {
                lbl.BackColor = System.Drawing.Color.LightBlue;
                lbl.ForeColor = System.Drawing.Color.LightBlue;
            }
            if (common.myInt(Session["EncounterId"]) > 0
                && (common.myInt(Session["ModuleId"]).Equals(3) || common.myStr(Session["ModuleName"]).ToUpper().Trim().Equals("EMR")))
            {


                imgVisitHistory.Visible = true;
                imgVitalHistory.Visible = true;
                imgDiagnosticHistory.Visible = true;
                imgXray.Visible = true;
                imgReferal.Visible = true;
                imgRefrealHistory.Visible = true;
                imgGrowthChart.Visible = true;
                imgAttachment.Visible = true;
                imgOTScheduler.Visible = false;
                imgDiagnosis.Visible = true;
                imgFollowUpAppointment.Visible = true;
                imgCaseSheet.Visible = true;
                txtSearchwithUTD.Visible = true;
                imgUTD.Visible = true;
                imgPastClinicalNote.Visible = true;
                imgImmunization.Visible = true;
                imgOTScheduler.Visible = true;
                //  imgOTRequest.Visible = true;

                if (common.myBool(Session["AllergiesAlert"])
                    || common.myStr(Session["AllergiesAlert"]).Equals("YES"))
                {
                    imgAllergyAlert.Visible = true;
                }

                if (common.myBool(Session["MedicalAlert"])
                    || common.myStr(Session["MedicalAlert"]).Equals("YES"))
                {
                    imgMedicalAlert.Visible = true;
                }
            }


        }
    }
    protected void imgAllergyAlert_OnClick(object sender, EventArgs e)
    {

        //callPopup("/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["encounterid"]) +
        //                            "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=Radiology");

        //callPopup("/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "");
        RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "";
        RadWindow1.Width = 1200;
        RadWindow1.Height = 630;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        //RadWindowForNew.OnClientClose = "";
        RadWindow1.Modal = true;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow1.VisibleStatusbar = false;
    }
    protected void lnkChangeFacility_onClick(object sender, EventArgs e)
    {
        //RadWindow1.NavigateUrl = "~/Include/Components/MasterComponent/ChangeFacility.aspx";
        //RadWindow1.Height = 450;
        //RadWindow1.Width = 550;
        //RadWindow1.Top = 40;
        //RadWindow1.Left = 100;
        //RadWindow1.OnClientClose = "OnClientClose";
        //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindow1.Modal = true;
        //RadWindow1.VisibleStatusbar = false;
        RadWindow1.NavigateUrl = "~/Include/Components/MasterComponent/ChangeFacility.aspx";
        RadWindow1.Width = 550;
        RadWindow1.Height = 450;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "returnToParent";
        RadWindow1.Modal = true;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow1.VisibleStatusbar = false;
    }
    protected void BtnClose_OnClick(object sender, EventArgs e)
    {
        lnkUser.InnerHtml = "";
        lnkUser.InnerHtml = lnkUser.InnerHtml + "Welcome!&nbsp;" + common.myStr(Session["EmployeeName"]);
        if (common.myStr(Session["ModuleName"]) == "Inventory" || common.myStr(Session["ModuleName"]) == "Inventory Reports" || common.myStr(Session["ModuleName"]) == "Inventory Setup")
        {
            if (Convert.ToString(Session["StoreName"]) != "")
                lnkUser.InnerHtml = lnkUser.InnerHtml + " (" + Convert.ToString(Session["StoreName"]) + ")";
        }
        if (common.myStr(Session["FacilityName"]) != "")
            lnkUser.InnerHtml = lnkUser.InnerHtml + " - " + common.myStr(Session["FacilityName"]);

    }
    //protected void lbkBtnSpouse_OnClick(object sender, EventArgs e)
    //{
    //    getSpouseRegistrationId();
    //}

    //protected void getSpouseRegistrationId()
    //{
    //    try
    //    {
    //        if (common.myInt(Session["RegistrationId"]) == 0)
    //        {
    //            Alert.ShowAjaxMsg("Patient not selected !", this.Page);
    //            return;
    //        }

    //        clsIVF objivf = new clsIVF(sConString);

    //        DataSet ds = new DataSet();

    //        ds = objivf.getIVFRegistrationId(common.myInt(Session["IVFId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            Session["RegistrationId"] = common.myInt(ds.Tables[0].Rows[0]["RegistrationId"]);
    //            Session["EncounterId"] = common.myInt(ds.Tables[0].Rows[0]["EncounterId"]);
    //            Session["IVFId"] = common.myInt(ds.Tables[0].Rows[0]["IVFId"]);
    //            Session["IVFNo"] = common.myInt(ds.Tables[0].Rows[0]["IVFNo"]);

    //            Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    protected void imgPastClinicalNote_OnClick(object sender, EventArgs e)
    {
        Session["SelectedCaseSheet"] = "PN";
        callPopup("/WardManagement/VisitHistory.aspx?Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"])
            + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&FromWard=Y&OP_IP=I&Category=PopUp");
    }
    protected void imgVisitHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/emr/Masters/PatientHistory.aspx?MP=NO");
    }

    protected void imgVitalHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/Vitals/PreviousVitals.aspx?POPUP=StaticTemplate");
    }
    protected void imgGrowthChart_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/Vitals/GrowthChart.aspx?MP=NO");
    }

    protected void imgDiagnosticHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["encounterid"]) +
                                        "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) +
                                        "&Flag=LIS&Station=All");
    }

    protected void imgFollowUpAppointment_OnClick(object sender, EventArgs e)
    {
        callPopup("/Appointment/AppScheduler.aspx?MASTER=NO");
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
        callPopup("/OTScheduler/OTAppointment.aspx?MASTER=No");
    }
    protected void imgXray_OnClick(object sender, EventArgs e)
    {
        callPopup("/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["encounterid"]) +
                                        "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) +
                                        "&Flag=RIS&Station=All");
    }
    protected void imgReferal_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/ReferralSlip.aspx?OP_IP=B&MASTER=NO&EncId=" + common.myStr(Session["encounterid"]) +
                                       "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS");
    }
    protected void imgRefrealHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/ReferralSlipHistory.aspx?OP_IP=B&MASTER=NO&EncId=" + common.myStr(Session["encounterid"]) +
                                       "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS");
    }

    protected void imgImmunization_OnClick(object sender, EventArgs e)
    {
        callPopup("~/EMR/Immunization/ImmunizationBabyDueDate.aspx?From=POPUP");
    }

    protected void imgMedicalAlert_OnClick(object sender, EventArgs e)
    {

        //callPopup("/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["encounterid"]) +
        //                            "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=Radiology");

        //callPopup("/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "");
        RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "";
        RadWindow1.Width = 1200;
        RadWindow1.Height = 630;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        //RadWindowForNew.OnClientClose = "";
        RadWindow1.Modal = true;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow1.VisibleStatusbar = false;



    }
    protected void imgUTD_OnClick(object sender, EventArgs e)
    {
        if (txtSearchwithUTD.Text.Trim().Length < 3)
        {
            //Alert.ShowAjaxMsg("Please Type in SearchBox then Continue..", this);
            txtSearchwithUTD.Focus();
            return;
        }
        //RadWindow1.NavigateUrl = UtdLink + "/search?sp=0&source=USER_PREF&search=" + txtSearchwithUTD.Text + "&searchType=PLAIN_TEXT";
        RadWindow1.NavigateUrl = UtdLink + "/contents/search?srcsys=HMGR374606&id=" + common.myStr(Session["EmployeeId"]) + "&search=" + txtSearchwithUTD.Text;
        RadWindow1.Width = 1200;
        RadWindow1.Height = 600;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        //RadWindowForNew.OnClientClose = "";
        RadWindow1.Modal = true;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow1.VisibleStatusbar = false;
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
            if (common.myInt(Session["RegistrationID"]).Equals(0))
            {
                Alert.ShowAjaxMsg("Patient not selected !", this.Page);
                return;
            }
            if (common.myInt(Session["EncounterId"]).Equals(0))
            {
                Alert.ShowAjaxMsg("Patient not selected !", this.Page);
                return;
            }

            RadWindow1.NavigateUrl = url;

            RadWindow1.Width = 1200;
            RadWindow1.Height = 630;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            //RadWindowForNew.OnClientClose = "";
            RadWindow1.Modal = true;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch
        {
        }
    }

    protected void imgDiagnosis_Click(object sender, ImageClickEventArgs e)
    {
        callPopup("/EMR/Assessment/Diagnosis.aspx?From=POPUP");
    }
    protected void btnHelp_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/HelpVideos/sampleindex.html";

        RadWindow1.Width = 1200;
        RadWindow1.Height = 630;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        //RadWindowForNew.OnClientClose = "";
        RadWindow1.Modal = true;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        //DAL.DAL dxl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //DataSet dsx = dxl.FillDataSet(CommandType.Text, "select URI from SecURLMaster with(nolock) where IsDefault=1");

        //string Url = dsx.Tables[0].Rows[0][0].ToString();

        //if (!Url.ToLower().Contains("logout"))
        //{
        //    if (Url.Contains("?"))
        //        Url = Url + "&logout=1&Status=logout";
        //    else
        //        Url = Url + "?logout=1&Status=logout";
        //}
        //Session.Abandon();
        //Session.RemoveAll();
        //Response.Redirect(Url);

        DAL.DAL dxl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet dsx = new DataSet();
        Hashtable hsin = new Hashtable();
        hsin.Add("@redurectionKey", common.myStr(Session["StrO"]));
        if (common.myStr(Session["StrO"]) != "")
            dsx = dxl.FillDataSet(CommandType.Text, "select LoginIPAddress from UserRedirectionLog with(nolock) where redurectionKey=@redurectionKey", hsin);
        else
            dsx = dxl.FillDataSet(CommandType.Text, "select URI from SecURLMaster with(nolock) where IsDefault=1");


        string Url = "";
        if (dsx.Tables.Count > 0 && dsx.Tables[0].Rows.Count > 0)
            Url = dsx.Tables[0].Rows[0][0].ToString();

        if (!Url.ToLower().Contains("login.aspx"))
            Url = Url + "/Login.aspx";

        if (!Url.ToLower().Contains("logout"))
        {
            if (Url.Contains("?"))
                Url = Url + "&logout=1";
            else
                Url = Url + "?logout=1";
        }

        Session.Abandon();
        Session.RemoveAll();
        Response.Redirect(Url.Trim());
    }

}
