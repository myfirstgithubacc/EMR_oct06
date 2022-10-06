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
using Telerik.Web.UI;

public partial class MRD_PatientDiagnosisEntry : System.Web.UI.Page
{
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    BaseC.EMRBilling objEmrBilling;
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ClearAll();
            txtAccountNo.Focus();

            btnCaseSheet.Visible = false;
            lnlMRDTemplate.Visible = false;
            btnOperationNotes.Visible = false;
            btnHistory.Visible = false;
            btnDischargeSummary.Visible = false;

        }

    }

    protected void ClearAll()
    {
        txtAccountNo.Text = "";
        hdnRegistrationId.Value = "";
        hdnRegistrationNo.Value = "";
        lblDischargeDate.Text = "";
        lblEncDate.Text = "";
        lblPayer.Text = "";
        lblEncounterNo.Text = "";
        lblMsg.Text = "";
        lblSponsor.Text = "";
        gvVisists.DataSource = null;
        gvVisists.DataBind();
        this.ifrmDiag.Attributes.Add("src", "");
        lblMsg.Text = "";
        lblDischargeStatus.Text = string.Empty;
    }

    protected void lbtnSearchPatient_Click(object sender, EventArgs e)
    {
        ClearAll();
        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=2&PageFrom=MRD";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void lnkShowDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < gvVisists.Rows.Count; i++)
            {
                gvVisists.Rows[i].BackColor = System.Drawing.Color.White;

            }
            LinkButton lnkBtn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lnkBtn.NamingContainer;

            row.BackColor = System.Drawing.Color.Pink;

            int iEncId = common.myInt(((HiddenField)lnkBtn.FindControl("hdnEncId")).Value);
            int iRegId = common.myInt(((HiddenField)lnkBtn.FindControl("hdnRegId")).Value);
            int hdnDoctorId = common.myInt(((HiddenField)lnkBtn.FindControl("hdnDoctorId")).Value);
            string hdnPayer = common.myStr(((HiddenField)lnkBtn.FindControl("hdnPayer")).Value);
            string hdnSponsor = common.myStr(((HiddenField)lnkBtn.FindControl("hdnSponsor")).Value);
            string hdnDischargeDate = common.myStr(((HiddenField)lnkBtn.FindControl("hdnDischargeDate")).Value);
            string lblEncDt = common.myStr(((Label)lnkBtn.FindControl("lblEncDate")).Text);
            string hdnEncounterDate = common.myStr(((HiddenField)lnkBtn.FindControl("hdnEncounterDate")).Value);
            string hdnBedNo = common.myStr(((HiddenField)lnkBtn.FindControl("hdnBedNo")).Value);
            string hdnBedCategory = common.myStr(((HiddenField)lnkBtn.FindControl("hdnBedCategory")).Value);
            string lblOPIP = common.myStr(((Label)lnkBtn.FindControl("lblOPIP")).Text);
            int hdnIsExpired = common.myInt(((HiddenField)lnkBtn.FindControl("hdnIsExpired")).Value);
            int hdnIsNewBorn = common.myInt(((HiddenField)lnkBtn.FindControl("hdnIsNewBorn")).Value);
            int hdnEncounterStatus = common.myInt(((HiddenField)lnkBtn.FindControl("hdnEncounterStatus")).Value);
            string hdnMLC = common.myStr(((HiddenField)lnkBtn.FindControl("hdnMLC")).Value);
            int  hdnAcknowledmentStatus = common.myInt(((HiddenField)lnkBtn.FindControl("hdnAcknowledmentStatus")).Value);
            string lblMRDNo = common.myStr(((Label)lnkBtn.FindControl("lblMRDNo")).Text);

            Session["hdnAcknowledmentStatus"] = hdnAcknowledmentStatus;
            string hdnRackNumber = common.myStr(((HiddenField)lnkBtn.FindControl("hdnRackNumber")).Value);
            string hdnStausName = common.myStr(((HiddenField)lnkBtn.FindControl("hdnStausName")).Value);

            hdnOPIP.Value = lblOPIP;
            lblPayer.Text = hdnPayer.ToString();
            lblSponsor.Text = hdnSponsor.ToString();
            lblDischargeDate.Text = hdnDischargeDate.ToString();
            lblDischargeStatus.Text = common.myStr(((HiddenField)lnkBtn.FindControl("hdnDischargeStatus")).Value);
            lblEncDate.Text = hdnEncounterDate.ToString();
            lblEncounterNo.Text = lnkBtn.Text;
            txtIPNo.Text = lnkBtn.Text;//f270117
            lblBedCategory.Text = hdnBedNo.ToString() + "/" + hdnBedCategory.ToString();
            TxtMrdNo.Text = lblMRDNo;

            if (lblOPIP.ToString() == "O")
            {
                lblEnc.Text = "Enc#";
                lblEDt.Text = "Enc.Dt.";

                btnCaseSheet.Visible = true;
                btnOperationNotes.Visible = true;
                btnHistory.Visible = true;
                btnDischargeSummary.Visible = false;
            }
            else
            {
                lblEnc.Text = "IP#";
                lblEDt.Text = "Adm.Dt.";

                btnCaseSheet.Visible = true;
                btnOperationNotes.Visible = true;
                btnHistory.Visible = true;
                btnDischargeSummary.Visible = true;
                string FiletransferWardtoMRDAfterEnterDiagnosticEntry = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "FiletransferWardtoMRDAfterEnterDiagnosticEntry", sConString);
                if (FiletransferWardtoMRDAfterEnterDiagnosticEntry.ToUpper().Equals("Y"))
                {
                    txtRackNumber.Text = hdnRackNumber;
                    lblRackNumber.Visible = true;
                    txtRackNumber.Visible = true;
                   
                    BaseC.Security objSecurity = new BaseC.Security(sConString);
                    lblCaptionStatus.Visible = true;
                    lblStatus.Visible = true;
                    lblStatus.Text = hdnStausName;
                    lnlUpdateDischargeInfo.Visible = true;
                        if (common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsUserAllowcloseOpenMrdFile")))
                    {

                        btnclose.Visible = true;
                        lnlMRDTemplate.Visible = true;
                        if (hdnAcknowledmentStatus == 1)
                        {
                            btnclose.Text = "Close File";
                        }
                        else
                        {
                            btnclose.Text = "Open File";
                        }
                    }
                }

            }
            Session["RegistrationID"] = iRegId;            
            Session["RegistrationNo"] = common.myStr(txtAccountNo.Text.Trim());
            Session["encounterid"] = iEncId;
            Session["DoctorID"] = hdnDoctorId;
            Session["AdmissionDate"] = hdnEncounterDate;
            Session["EncounterNo"] = lblEncounterNo.Text;
            Session["EncounterNo"] = txtIPNo.Text;//f27012017
            Session["TypeOPIP"] = lblOPIP;

            this.ifrmDiag.Attributes.Add("src", "../MRD/Diagnosis.aspx?From=POPUP&DI=" + hdnDoctorId + "&CF=MRDM" + "&IsExpire=" + hdnIsExpired + "&IsNewBorn=" + hdnIsNewBorn + "&IsEncStatus=" + hdnEncounterStatus + "&MLC=" + hdnMLC + "&AcknowledmentStatus=" + hdnAcknowledmentStatus + "&OPIP=" + lblOPIP );//cf - Mrd Module
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        //Added by ujjwal 06 July 2015 to validate UHID start
        if (!txtAccountNo.Text.Trim().Length.Equals(0))
        {
            Int64 UHID;
            Int64.TryParse(txtAccountNo.Text, out UHID);
            if ((UHID > 9223372036854775807 || UHID.Equals(0)))
            {
                Alert.ShowAjaxMsg("Value should not be more than 9223372036854775807.", this.Page);
                txtAccountNo.Text = txtAccountNo.Text.Substring(0, 12);
                return;
            }
        }
        //Added by ujjwal 06 July 2015 to validate UHID start
        btnCaseSheet.Visible = false;
        btnOperationNotes.Visible = false;
        btnHistory.Visible = false;
        btnDischargeSummary.Visible = false;

        if (common.myStr(txtAccountNo.Text.Trim()) != "")
        {
            int RegistrationNo = common.myInt(txtAccountNo.Text.Trim());
            ClearAll();
            if (RegistrationNo > 0)
            {
                BaseC.Patient bC = new BaseC.Patient(sConString);
                DataSet ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, 0, 0);
                if (ds.Tables.Count > 0)
                {

                    DataRow dr = ds.Tables[0].Rows[0];
                    hdnRegistrationNo.Value = common.myStr(dr["RegistrationNo"]);
                    txtAccountNo.Text = common.myStr(dr["RegistrationNo"]);
                    hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                    lblPayer.Text = common.myStr(dr["PatientSponsorName"]);
                    lblSponsor.Text = common.myStr(dr["PatientSponsorName"]);
                    if (hdnRegistrationId.Value != string.Empty) //f270117
                    {
                        patientQV.ShowPatientDetails(Convert.ToInt32(hdnRegistrationId.Value));
                    }
                }
                ds.Dispose();
                ds.Clear();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshIn = new Hashtable();

                hshIn.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
                hshIn.Add("@FacilityID", common.myInt(Session["FacilityID"]));
                hshIn.Add("@RegistrationId", common.myInt(hdnRegistrationId.Value));
                ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientEncounterDetails", hshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvVisists.DataSource = ds;
                    gvVisists.DataBind();
                }
                else //f270117
                {
                    Alert.ShowAjaxMsg("Patient Not Found", this.Page);
                    return;
                }
            }
        }
    }
    protected void btnHistory_Click(object sender, EventArgs e)
    {

        RadWindow4.NavigateUrl = "~/EMR/Assessment/DiagnosisHistory.aspx?From=POPUP&callby=mrd";

        RadWindow4.Height = 610;
        RadWindow4.Width = 980;
        RadWindow4.Top = 10;
        RadWindow4.Left = 10;
        RadWindow4.OnClientClose = ""; //"SearchPatientOnClientClose";//
        RadWindow4.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow4.Modal = true;
        // RadWindow4.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        //RadWindow4.Title = RadContextMenu2.SelectedItem.Text;
        RadWindow4.VisibleStatusbar = false;
        // RadWindow4.InitialBehavior = WindowBehaviors.Maximize;

    }
    protected void btnDischargeSummary_Click(object sender, EventArgs e)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Int32 intSummaryId = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, "select top 1 SummaryID from EMRPatientSummaryDetails where EncounterID =" + common.myStr(Session["encounterid"])));
        DataTable dtEncounterFormatFinalizeStatus = new DataTable();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        if (intSummaryId == 0)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Discharge summary is not saved!!";
            return;
        }
        int encounterid = common.myInt(Session["encounterid"]);
        int RegistrationId = common.myInt(Session["RegistrationId"]);


        dtEncounterFormatFinalizeStatus = objEMR.GetEncounterFormatFinalizeStatus(encounterid, RegistrationId);

        if (dtEncounterFormatFinalizeStatus != null)
        {
            if (common.myInt(dtEncounterFormatFinalizeStatus.Rows.Count) > 0)
            {
                RadWindow3.NavigateUrl = "~/EMRReports/PrintPdf1.aspx?page=MRD&EncId=" + common.myStr(Session["encounterid"]) + "&RegId=" +
                               common.myStr(Session["RegistrationId"]) + "&For=DISSUM" + "&ReportId=" + common.myStr(dtEncounterFormatFinalizeStatus.Rows[0]["FormatID"]) + "&Finalize=" + common.myStr(dtEncounterFormatFinalizeStatus.Rows[0]["FinalizeStatus"]);

            }
        }

        RadWindow3.Height = 610;
        RadWindow3.Width = 1080;
        RadWindow3.Top = 10;
        RadWindow3.Left = 10;
        RadWindow3.OnClientClose = ""; //"SearchPatientOnClientClose";//
        RadWindow3.VisibleOnPageLoad = true;
        RadWindow3.Modal = false;
        //RadWindow3.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        //RadWindow3.Title = RadContextMenu2.SelectedItem.Text;
        RadWindow3.VisibleStatusbar = false;
        RadWindow3.InitialBehaviors = WindowBehaviors.Maximize;

    }
    protected void btnOperationNotes_Click(object sender, EventArgs e)
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet dsPatientDetail = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(txtAccountNo.Text),
                common.myStr(Session["EncounterNo"]), common.myInt(Session["UserId"]), 0);
        if (dsPatientDetail.Tables.Count > 0)
        {
            if (dsPatientDetail.Tables[0].Rows.Count > 0)
            {
                string sRegNoTitle = Resources.PRegistration.regno;
                string sDoctorTitle = Resources.PRegistration.Doctor;
                string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date :" : "Encounter Date :";
                Session["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["GenderAge"]) + "</span>"
                 + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                 + "&nbsp;Enc #:&nbsp;" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]) + "</span>"
                 + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"
                 + DateTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(Session["EncounterDate"]) + "</span>"
                 + "&nbsp;Bed:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["BedNo"]) + "</span>"
                 + "&nbsp;Ward:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["WardName"]) + "</span>"
                 + "&nbsp;Mobile:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                  + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]) + "</span>"
                 + "</b>";
            }
        }
        dsPatientDetail.Dispose();

        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Int32 intSummaryId = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, "select top 1 SummaryID from EMRPatientSummaryDetails where EncounterID =" + common.myStr(Session["encounterid"])));

        //if (intSummaryId == 0)
        //{
        //    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMsg.Text = "Discharge summary is not saved!!";
        //    return;
        //}


        RadWindow3.NavigateUrl = "~/MRD/OperationNotes.aspx?From=POPUP&EncId=" + common.myStr(Session["encounterid"]) + "&RegId=" +
                                common.myStr(Session["RegistrationId"]) + "&RegNo=" + txtAccountNo.Text + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&DisplayMenu=1&TemplateId=5960";

        RadWindow3.Height = 610;
        RadWindow3.Width = 1080;
        RadWindow3.Top = 10;
        RadWindow3.Left = 10;
        RadWindow3.OnClientClose = ""; //"SearchPatientOnClientClose";//
        RadWindow3.VisibleOnPageLoad = true;
        RadWindow3.Modal = false;
        //RadWindow3.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        //RadWindow3.Title = RadContextMenu2.SelectedItem.Text;
        RadWindow3.VisibleStatusbar = false;
        RadWindow3.InitialBehavior = WindowBehaviors.Maximize;

    }
    protected void btnCaseSheet_Click(object sender, EventArgs e)
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet dsPatientDetail = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(txtAccountNo.Text),
                common.myStr(Session["EncounterNo"]), common.myInt(Session["UserId"]), 0);
        if (dsPatientDetail.Tables.Count > 0)
        {
            if (dsPatientDetail.Tables[0].Rows.Count > 0)
            {
                string sRegNoTitle = Resources.PRegistration.regno;
                string sDoctorTitle = Resources.PRegistration.Doctor;
                string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date :" : "Encounter Date :";
                Session["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["GenderAge"]) + "</span>"
                 + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                 + "&nbsp;Enc #:&nbsp;" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]) + "</span>"
                 + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"
                 + DateTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(Session["EncounterDate"]) + "</span>"
                 + "&nbsp;Bed:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["BedNo"]) + "</span>"
                 + "&nbsp;Ward:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["WardName"]) + "</span>"
                 + "&nbsp;Mobile:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                 + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]) + "</span>"
                 + "</b>";
            }
        }
        dsPatientDetail.Dispose();

        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        Int32 intFormId = objPatient.GetPatientDefaultFormId(Convert.ToInt32(Session["encounterid"]), Convert.ToInt16(Session["HospitalLocationID"]));
        if (intFormId > 0)
        {
            Session["formId"] = Convert.ToString(intFormId);
        }

        RadWindow2.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&EREncounterId=" + common.myInt(Session["encounterid"])
                                + "&EncounterDate=" + common.myDate(Session["AdmissionDate"]).ToString("yyyy/MM/dd") + "&callby=mrd&OPIP=" + hdnOPIP.Value;
        RadWindow2.Height = 600;
        RadWindow2.Width = 990;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        RadWindow2.OnClientClose = ""; //"SearchPatientOnClientClose";//
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.Modal = false;
        //RadWindow2.Title = RadContextMenu2.SelectedItem.Text;
        RadWindow2.VisibleStatusbar = false;
        RadWindow2.InitialBehavior = WindowBehaviors.Maximize;


    }


    protected void lbtnSearchPatientIP_Click(object sender, EventArgs e)
    {
        ClearAll();
        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=2&PageFrom=MRD";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientOnClientCloseIP";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnGetInfoIP_Click(object sender, EventArgs e)  //f270117
    {
        //if (!txtAccountNo.Text.Trim().Length.Equals(0))
        //{
        //    int UHID;
        //    int.TryParse(txtAccountNo.Text, out UHID);
        //    if ((UHID > 2147483647 || UHID.Equals(0)))
        //    {
        //        Alert.ShowAjaxMsg("Value should not be more than 2147483647.", this.Page);
        //        txtAccountNo.Text = txtAccountNo.Text.Substring(0, 9);
        //        return;
        //    }
        //}

        btnCaseSheet.Visible = false;
        btnOperationNotes.Visible = false;
        btnHistory.Visible = false;
        btnDischargeSummary.Visible = false;

        if (common.myStr(txtIPNo.Text) != "")
        {
            string IPNo = common.myStr(txtIPNo.Text);
            ClearAll();
            if (IPNo != "")
            {
                BaseC.Patient bC = new BaseC.Patient(sConString);
                DataSet ds = bC.getPatientDetailsEnc(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0, IPNo, 0, 0);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        hdnRegistrationNo.Value = common.myStr(dr["RegistrationNo"]);
                        txtAccountNo.Text = common.myStr(dr["RegistrationNo"]);
                        hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                        lblPayer.Text = common.myStr(dr["PatientSponsorName"]);
                        lblSponsor.Text = common.myStr(dr["PatientSponsorName"]);

                        if (hdnRegistrationId.Value != string.Empty)
                        {
                            patientQV.ShowPatientDetails(Convert.ToInt32(hdnRegistrationId.Value));
                        }

                    }
                }
                ds.Dispose();
                ds.Clear();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshIn = new Hashtable();

                hshIn.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
                hshIn.Add("@FacilityID", common.myInt(Session["FacilityID"]));
                hshIn.Add("@RegistrationId", common.myInt(hdnRegistrationId.Value));
                ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientEncounterDetails", hshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "EncounterNo= '" + common.myStr(IPNo) + "'";

                    gvVisists.DataSource = dv.ToTable();
                    gvVisists.DataBind();
                }
                else
                {
                    Alert.ShowAjaxMsg("Patient Not Found", this.Page);
                    return;
                }
            }
        }

    }

    protected void btnclose_Click(object sender, EventArgs e)
    {
        string msg = "";
        objEmrBilling = new BaseC.EMRBilling(sConString);
        if (btnclose.Text == "Close File")
        {
            msg = objEmrBilling.SaveFileCloseMRD(common.myInt(Session["HospitalLocationID"]),
                common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]),
                common.myInt(Session["UserId"]), common.myStr(txtRackNumber.Text), true);

        }
        else
        {
            msg = objEmrBilling.SaveFileCloseMRD(common.myInt(Session["HospitalLocationID"]),
              common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]),
              common.myInt(Session["UserId"]), common.myStr(txtRackNumber.Text), false);
        }
        if (msg.Equals("File Closed"))
        {
            btnGetInfoIP_Click(null, null);
            lblMsg.Text = msg;
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            pnlMain.Enabled = false;
        }
        else if (msg.Equals("File Open"))
        {

            btnGetInfoIP_Click(null, null);
            lblMsg.Text = msg;
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            pnlMain.Enabled = true;
        }
        else
        {

            btnGetInfoIP_Click(null, null);
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = msg;
        }
        }

    protected void lnlMRDTemplate_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.Patient patient = new BaseC.Patient(sConString);

        ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                           common.myStr(Session["RegistrationNo"]), common.myStr(Session["EncounterNo"]), common.myInt(Session["UserId"]));


        Session["PatientDetailString"] = null;
        Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;
        Session["InvoiceId"] = "";
        RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?From=POPUP&EREncounterId=" + 
            common.myInt(Session["encounterid"]) +"&ismrd=1&AdmissionDate=" + lblEncDate.Text;
        RadWindow1.Height = 610;
        RadWindow1.Width = 880;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void lnlUpdateDischargeInfo_Click(object sender, EventArgs e)
    {
       
        RadWindow1.NavigateUrl = "/MRD/discharge.aspx?Regno=" + common.myStr(hdnRegistrationNo.Value.Trim()) + "&encno=" + common.myStr(lblEncounterNo.Text) + "&MASTER=No&AcknowledmentStatus=" + common.myInt(Session["hdnAcknowledmentStatus"]);
        RadWindow1.Height = 610;
        RadWindow1.Width = 880;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientClose"; //"SearchPatientOnClientClose";//
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void lbtnSearchPatientMRD_Click(object sender, EventArgs e)
    {
        ClearAll();
        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=2&PageFrom=MRD";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientOnClientCloseMRDNo";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnGetInfoMRD_Click(object sender, EventArgs e)
    {
        btnCaseSheet.Visible = false;
        btnOperationNotes.Visible = false;
        btnHistory.Visible = false;
        btnDischargeSummary.Visible = false;

        if (common.myStr(TxtMrdNo.Text) != "")
        {
            string MRDNo = common.myStr(TxtMrdNo.Text);
            ClearAll();
            if (MRDNo != "")
            {
                BaseC.Patient bC = new BaseC.Patient(sConString);
                DataSet ds = bC.getPatientDetailsMRDNo(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0, MRDNo, 0, 0);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        hdnRegistrationNo.Value = common.myStr(dr["RegistrationNo"]);
                        txtAccountNo.Text = common.myStr(dr["RegistrationNo"]);
                        hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                        lblPayer.Text = common.myStr(dr["PatientSponsorName"]);
                        lblSponsor.Text = common.myStr(dr["PatientSponsorName"]);

                        if (hdnRegistrationId.Value != string.Empty)
                        {
                            patientQV.ShowPatientDetails(Convert.ToInt32(hdnRegistrationId.Value));
                        }
                    }
                }
                ds.Dispose();
                ds.Clear();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshIn = new Hashtable();

                hshIn.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
                hshIn.Add("@FacilityID", common.myInt(Session["FacilityID"]));
                hshIn.Add("@RegistrationId", common.myInt(hdnRegistrationId.Value));
                ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientEncounterDetails", hshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "MRDFileNo= '" + common.myStr(MRDNo) + "'";

                    gvVisists.DataSource = dv.ToTable();
                    gvVisists.DataBind();
                }
                else
                {
                    Alert.ShowAjaxMsg("Patient Not Found", this.Page);
                    return;
                }
            }
        }

    }
}
