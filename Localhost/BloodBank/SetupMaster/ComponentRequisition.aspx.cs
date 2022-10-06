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
using System.IO;
using System.Text;
using System.Data.SqlClient;
using Telerik.Web.UI;

public partial class BloodBank_SetupMaster_ComponentRequisition : System.Web.UI.Page
{
    DAL.DAL dl = new DAL.DAL();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    //Added on 08-08-2014 start 

    DataSet dsSearch;
    DataSet dsComponentRequisitionDetails;

    //Added on 07-08-2014  End

    BaseC.clsBb objBb;
    BaseC.RestFulAPI objCM;
    BaseC.Patient objp;

    StringBuilder strXML;
    ArrayList coll;
    StringBuilder strXMLForQuestion;
    ArrayList collForQuestion;
    StringBuilder strXMLForIndication;
    ArrayList collForIndication;
    UserAuthorisations ua1 = new UserAuthorisations();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["MP"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        objCM = new BaseC.RestFulAPI(sConString);
        if (!IsPostBack)
        {
            if (common.myStr(Request.QueryString["MP"]) != "NO")
            {
                btnclose.Visible = false;
            }

            //Added on 07-08-2014  start  By   Naushad
            Div1.Visible = false;
            //Added on 07-08-2014  End By Naushad


            ViewState["_ID"] = "0";
            Session.Remove("GridData");
            dtpConsentDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            dtpDateOfBirth.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            //  txtRequestDate.Text = Convert.ToDateTime(DateTime.Now.Date).ToString(common.myStr(Session["OutputDateFormat"]));
            // by ashma
            string BBBlooDRequestionDatewithTime = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "BBBlooDRequestionDatewithTime", sConString);
            if (BBBlooDRequestionDatewithTime == "Y")
            {
                txtRequestDate.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");//Convert.ToDateTime(DateTime.Now.Date).ToString("dd/MM/yyyy hh:mm:ss");
            }
            else
            {
                txtRequestDate.Text = Convert.ToDateTime(DateTime.Now.Date).ToString(common.myStr(Session["OutputDateFormat"]));
            }
            bindQuestionList();
            bindBloodGroup();
            dtpDateOfBirth.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            SetPermission();
            BindDoctor();
            fillRemarks();
            BindIndication();
            BindPatientProvisionalDiagnosis();
            bindHospitalList();
            //Added on 08-08-2014 Start Naushad Ali

            dtpRequestDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            dtpRequestDate.SelectedDate = DateTime.Now;
            dtpRequestDate.MinDate = DateTime.Now;
            //if (common.myStr(Session["EncounterStatus"]) == "Close")
            //{
            //    btnSaveData.Visible = false;
            //}


            LoadBloodComponent();
            BindBlankNewComponent();


            if (Request.QueryString["Mpg"] == null && Request.QueryString["MP"] == null)
            {
                LinkButton1.Enabled = true;
                LinkButton2.Enabled = true;

            }
            else
            {
                LinkButton1.Enabled = false;
                LinkButton2.Enabled = false;
            }


            //Added on 08-08-2014  End Naushad Ali


            //Add by Balkishan start  

            if (common.myStr(Request.QueryString["Type"]) == "OP")
            {
                LinkButton1.Enabled = true;
                LinkButton2.Enabled = false;
                Label32.Visible = false;
                otherhospitalDiv.Visible = true;
                string value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "COMPONENTREQUISITIONDISABLEOLDHOSPITAL", sConString);
                if (value == "Y")//for BLK HOSPITAL
                {
                    chkIsOuterRequest.Enabled = false;
                    ddlHospitalList.Enabled = false;
                }

                string value1 = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "COMPONENTREQUISITIONHIDEOLDHOSPITALDOCTORNAME", sConString);
                if (value1 == "Y")//for Common/BLK/OPTIONAL HOSPITAL
                {

                    divoldhospital.Visible = false;
                    divoldDoctorname.Visible = false;
                }
                //chkIsOuterRequest.Checked = true;
                //chkIsOuterRequest.Enabled = false;
            }
            else
            {

                otherhospitalDiv.Visible = false;
                LinkButton1.Enabled = false;
                //LinkButton2.Enabled = false;
            }

            //Add by Balkishan end

            ddlHospitalList.Enabled = false;
            ddlotherhospital.Enabled = false;
            hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);
        }

        if (Request.QueryString["RegNo"] != null)
        {
            hdnEncounterId.Value = Convert.ToString(Request.QueryString["EncId"]).Trim();
            hdnEncounterNo.Value = Convert.ToString(Request.QueryString["EncNo"]).Trim();
            hdnRegistrationId.Value = Convert.ToString(Request.QueryString["Regid"]).Trim();
            hdnRegistrationNo.Value = Convert.ToString(Request.QueryString["RegNo"]).Trim();
            spanPBG.Visible = true;
            spanIT.Visible = true;
        }
        else if (Session["ModuleId"] != null && Session["ModuleId"].ToString() == "3")
        {
            if (Session["EncounterId"] == null)
            {
                Response.Redirect("/default.aspx?RegNo=0", false);
                return;
            }
            hdnEncounterId.Value = Session["EncounterId"].ToString();
            hdnEncounterNo.Value = Session["EncounterNo"].ToString();
            hdnRegistrationId.Value = Session["RegistrationId"].ToString();
            hdnRegistrationNo.Value = Session["RegistrationNo"].ToString();
            spanPBG.Visible = false;
            spanIT.Visible = false;

        }
        PatientInfo();
        if (hdnRegistrationId.Value != "")
        {
            objBb = new BaseC.clsBb(sConString);
            DataSet dset = objBb.GetPatientDetailsUsingRegistrationId(Convert.ToInt32(hdnRegistrationId.Value));
            if (dset.Tables.Count > 0)
            {
                DataRow row = dset.Tables[0].Rows.Count > 0 ? dset.Tables[0].Rows[0] : null;
                if (row != null)
                {
                    txtWard.Text = Convert.ToString(row["WardName"]);
                    // txtDiagnosis.Text = Convert.ToString(row["Diagnosis"]);
                    txtBedNo.Text = Convert.ToString(row["BedNo"]);

                }
            }
        }

        populateControlsForEditing();
        if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
        {
            btnSaveData.Visible = false;
            btnNew.Visible = false;

        }
        //added by bhakti
        string isRequireIPBillOfflineMarking = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "isRequireIPBillOfflineMarking", sConString);
        Session["isRequireIPBillOfflineMarking"] = isRequireIPBillOfflineMarking;

        string valueBBENABLEREQPATIENTBLOODGROUP = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "BBENABLEREQPATIENTBLOODGROUP", sConString);
        if (valueBBENABLEREQPATIENTBLOODGROUP == "Y")//for VKT HOSPITAL
        {
            Session["BBENABLEREQPATIENTBLOODGROUP"] = valueBBENABLEREQPATIENTBLOODGROUP;
            ddlPatientBloodGroup.Enabled = true;
        }
        else
        {
            Session["BBENABLEREQPATIENTBLOODGROUP"] = null;
            ddlPatientBloodGroup.Enabled = false;
        }
    }

    private bool isPatientAdmitted()
    {

        lblMessage.Text = string.Empty;

        BaseC.clsBb objectBb = new BaseC.clsBb(sConString);
        int isAdmittedYes = 0;
        isAdmittedYes = objectBb.IsPatientAdmitted(common.myInt(hdnRegistrationId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
        if (isAdmittedYes.Equals("1"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void bindQuestionList()
    {
        DataSet ds = new DataSet();
        DataView dv;
        objBb = new BaseC.clsBb(sConString);
        try
        {
            gvPreviousHistory.DataSource = objBb.GetComponentRequestionPreviousHistoryQuestionMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(ddlSex.SelectedValue.ToString()) == 1 ? "M" : (common.myInt(ddlSex.SelectedValue.ToString()) == 2 ? "F" : "B"), 1);
            gvPreviousHistory.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
        EnableControl();
    }
    private void isSave()
    {
        if (common.myStr(Request.QueryString["Type"]) == "OP")
        {
            if (isPatientAdmitted())
            {
                Alert.ShowAjaxMsg("Patient is admitted", Page);
                return;
            }
        }
        if (ddlRequestType.SelectedValue == "")
        {
            Alert.ShowAjaxMsg("Please Select Request Type", Page);
            ddlRequestType.Focus();
            return;
        }


        if ((Convert.ToString(txtPatientName.Text).Trim().Equals("")))
        {
            Alert.ShowAjaxMsg("Please enter Patient Name", Page);
            return;
        }



        if (ddlSex.SelectedValue == "")
        {
            Alert.ShowAjaxMsg("Please Select Gender", Page);
            ddlSex.Focus();
            return;
        }
        //if (txtWeight.Text.Trim().Equals(""))
        //{
        //    Alert.ShowAjaxMsg("Please Select Weight", Page);
        //    txtWeight.Focus();
        //    return;
        //}


        //if (ddlPatientBloodGroup.SelectedValue == "")
        //{
        //    Alert.ShowAjaxMsg("Please Select Blood Group", Page);
        //    ddlPatientBloodGroup.Focus();  
        //    return;
        //}






        //New code by vikas 19/09/2021

        string multiple = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "COMPONENTREQUISITIONHIDEOLDHOSPITALDOCTORNAME", sConString);


        if (multiple == "N")
        {
            if (ViewState["GridData"] != null)
            {
                DataTable dsGridData = (DataTable)ViewState["GridData"];
                if (dsGridData.Rows.Count == 1)
                {
                    if (common.myStr(dsGridData.Rows[0]["ComponentName"]) == "")
                    {

                        Alert.ShowAjaxMsg("Please Add The Component... ", Page);
                        //txtWeight.Focus();
                        return;
                    }
                }
                else if (dsGridData.Rows.Count == 2)
                {
                    Alert.ShowAjaxMsg("Only single component is allowed.", Page);
                    return;
                }

            }

        }

        //End code by vikas 19/09/2021

        //Added on 08-08-2014 Start  Naushad Ali

        //if (ViewState["GridData"] != null)
        //{
        //    DataTable dsGridData = (DataTable)ViewState["GridData"];
        //    if (dsGridData.Rows.Count == 1)
        //    {
        //        if (common.myStr(dsGridData.Rows[0]["ComponentName"]) == "")
        //        {

        //            Alert.ShowAjaxMsg("Please Add The Component... ", Page);
        //            //txtWeight.Focus();
        //            return;
        //        }
        //    }
        //    else if (dsGridData.Rows.Count == 2)
        //    {
        //        Alert.ShowAjaxMsg("Only single component is allowed.", Page);
        //        return;
        //    }

        //}


        //Added on 08-08-2014 End Naushad Ali

        if (gvNewComponent.Rows.Count <= 0)
        {

            Alert.ShowAjaxMsg("Please Add The Component... ", Page);
            txtWeight.Focus();
            return;
        }


        if (chbIsExchangeTransformation.Checked)
        {
            if (txtHaematocritRequired.Text.Trim().Equals(""))
            {
                Alert.ShowAjaxMsg("Please Select Haematocrit... ", Page);
                txtHaematocritRequired.Focus();
                return;
            }
        }





        DateTime datet;
        bool AgeIdentity;

        if (dtpDateOfBirth.SelectedDate == null
          && common.myInt(txtYear.Text) == 0 && common.myInt(txtMonth.Text) == 0 && common.myInt(txtDays.Text) == 0)
        {
            Alert.ShowAjaxMsg("Date of Birth or Age either should be mandatory !", Page);
            return;
        }


        if (ddlCounsultant.SelectedValue == "")
        {
            Alert.ShowAjaxMsg("Please Select Consultant Name", Page);
            ddlCounsultant.Focus();
            return;


        }
        // by ashama
        string BBBloodDiagnosisValidation = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "BBBloodDiagnosisValidation", sConString);
        if (BBBloodDiagnosisValidation == "Y")
        {
            if (txtDiagnosis.Text.Trim().Equals("") || txtDiagnosis.Text.Trim().Length.Equals(0))
            {
                Alert.ShowAjaxMsg("Please Enter Diagnosis", Page);
                txtDiagnosis.Focus();
                return;
            }
        }

        //Comment By Himanshu on Date 17/03/2022 Mr. Saifudeen 

        //if (ddlIndicationforTransfusion.SelectedIndex == 0 || ddlIndicationforTransfusion.SelectedIndex == -1)
        //{
        //    Alert.ShowAjaxMsg("Please Select Indication for Transfusion", Page);
        //    ddlIndicationforTransfusion.Focus();
        //    return;
        //}
        //End

        //if (Session["ModuleId"] != null && Session["ModuleId"].ToString() != "3")
        //{
        spanPBG.Visible = true;
        spanIT.Visible = true;
        //by ashama
        string BBBloodIndicationforTransfusionValidation = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "BBBloodIndicationforTransfusionValidation", sConString);
        if (BBBloodIndicationforTransfusionValidation == "Y")
        {
            if (ddlIndicationforTransfusion.SelectedValue == "0" || ddlIndicationforTransfusion.SelectedValue == "-1")
            {
                Alert.ShowAjaxMsg("Please Select Indication for Transfusion", Page);
                ddlIndicationforTransfusion.Focus();
                return;
            }
        }
        string valueBBENABLEREQPATIENTBLOODGROUP = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "BBENABLEREQPATIENTBLOODGROUP", sConString);
        if (valueBBENABLEREQPATIENTBLOODGROUP == "Y" && ddlPatientBloodGroup.Enabled == true)//for VKT HOSPITAL
        {
            ddlPatientBloodGroup.Enabled = true;
            if (ddlPatientBloodGroup.SelectedValue == "")
            {
                Alert.ShowAjaxMsg("Please Select Patient Blood Group.", Page);
                ddlPatientBloodGroup.Focus();
                return;
            }
        }
        else
        {
            ddlPatientBloodGroup.Enabled = false;
        }
        //if (ddlPatientBloodGroup.SelectedValue == "")
        //{
        //    Alert.ShowAjaxMsg("Please Select Patient Blood Group", Page);
        //    ddlPatientBloodGroup.Focus();
        //    return;
        //}

        //}
        //else
        //{
        //    spanPBG.Visible = false;
        //    spanIT.Visible = false;
        //}


        //Commented on 08-08-2014 for Non-Mandatory  start Naushad
        //if (ddlConsentTakenBy.SelectedValue == "0" || ddlConsentTakenBy.SelectedValue == "")
        //{
        //    Alert.ShowAjaxMsg("Please Select Consent Taken By", Page);
        //    ddlConsentTakenBy.Focus();
        //    return;
        //}
        //Commented on 08-08-2014 for Non-Mandatory  End Naushad

        if (ddlRequestType.SelectedValue == "")
        {
            Alert.ShowAjaxMsg("Please Select Request Type", Page);
            ddlRequestType.Focus();
            return;
        }
        //else if (ddlRequestType.SelectedValue.Equals("E"))
        //{
        //    if (ddlRequestType.SelectedValue.Equals("E"))
        //    {

        //    }
        //}

        string value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "COMPONENTREQUISITIONTRANSFUSIONCHKVALIDATE", sConString);
        if (value == "Y")//for VKT HOSPITAL
        {


            if (ChbAnyTransfusion.Checked == false)
            {

                Alert.ShowAjaxMsg("Please Select Transfusion History", Page);
                ChbAnyTransfusion.Focus();
                return;
            }
            if (ChbAnyTransfusionReaction.Checked == false)
            {

                Alert.ShowAjaxMsg("Please Select Transfusion Reaction", Page);
                ChbAnyTransfusionReaction.Focus();
                return;
            }


        }
        string valueIndicationForTransfusion = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "COMPONENTREQUISITIONTRANSFUSIONDROPDOWN", sConString);
        if (valueIndicationForTransfusion == "Y")//for VKT HOSPITAL
        {
            if (ddlIndicationforTransfusion.SelectedValue == "0")
            {
                Alert.ShowAjaxMsg("Please Select Indication for Transfusion", Page);
                ddlIndicationforTransfusion.Focus();
                return;
            }
        }


        if (Page.IsValid)
        {

            if (common.myBool(hdnIsPasswordRequired.Value))
            {
                IsValidPassword();
                return;
            }
            saveRecords();
        }
    }
    private void saveRecords()
    {


        #region Component Requisition Details

        strXML = new StringBuilder();
        coll = new ArrayList();

        if (gvNewComponent != null)
        {

            foreach (GridViewRow dataItem in gvNewComponent.Rows)
            {
                HiddenField hdnComponentId = (HiddenField)dataItem.FindControl("hdnComponentId");
                HiddenField hdnUniqueId = (HiddenField)dataItem.FindControl("hdnUniqueId");
                Label lblQty = (Label)dataItem.FindControl("lblQty");
                Label lblRequestDate = (Label)dataItem.FindControl("lblRequestDate");
                Label lblSizeML = (Label)dataItem.FindControl("lblSizeML");
                coll.Add(common.myInt(hdnComponentId.Value));
                //coll.Add(common.myInt(hdnUniqueId.Value));
                coll.Add(common.myInt(lblQty.Text));
                coll.Add(common.myDate(lblRequestDate.Text));
                coll.Add(common.myDec(lblSizeML.Text));
                coll.Add(1);
                strXML.Append(common.setXmlTable(ref coll));
            }

        }

        #endregion

        #region Component Requisition Question Details

        strXMLForQuestion = new StringBuilder();
        collForQuestion = new ArrayList();

        if (gvNewComponent != null)
        {

            foreach (GridViewRow dataItem in gvPreviousHistory.Rows)
            {
                RadioButtonList radList = (RadioButtonList)dataItem.FindControl("radAnswer");
                if (radList.SelectedIndex == 0)
                {
                    HiddenField hdnQuestionId = (HiddenField)dataItem.FindControl("hdnQuestionId");
                    collForQuestion.Add(common.myInt(hdnQuestionId.Value));
                    collForQuestion.Add(1); // it's for question value
                    collForQuestion.Add(1); // it's for active                
                    strXMLForQuestion.Append(common.setXmlTable(ref collForQuestion));
                }
            }

        }

        #endregion


        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = string.Empty;

            //Added by Rakesh on 06-10-2014 Start 
            string Ptype1 = "";
            if (ViewState["OPIP"] != null)
            {
                Ptype1 = common.myStr(ViewState["OPIP"]);
            }
            else
            {

                Ptype1 = common.myStr(Session["OPIP"]);
            }
            //Added by Rakesh on 06-10-2014 end 

            int emergencyType = 0;
            if (ddlRequestType.SelectedValue.Equals("E"))
            {
                emergencyType = common.myInt(ddlEmergencyType.SelectedValue);
            }
            int SampleSend = 0;
            if (isSampleSend.Checked)
            {
                SampleSend = 1;
            }
            if (Ptype1 == "O" || Ptype1 == "E")
            {
                #region "Commented Code"
                /*
                //if (!(txtReqNo.Text.Trim().Equals(string.Empty)))
                //{
                //    objBb = new BaseC.clsBb(sConString);
                //    string strDOBorAge = string.Empty;
                //    if (!(dtpDateOfBirth.SelectedDate.Equals(string.Empty)))
                //        strDOBorAge = "D";
                //    else
                //        strDOBorAge = "A";

                //    bool Pregnancy = false;
                //    bool Miscarriage = false;

                //    if (rbtnpregnancy.SelectedIndex > -1)
                //        Pregnancy = common.myBool(rbtnpregnancy.SelectedValue);
                //    if (rbtnMiscarriage.SelectedIndex > -1)
                //        Miscarriage = common.myBool(rbtnpregnancy.SelectedValue);




                //    //string strMsg = "";
                //    string strMsg = objBb.SaveComponentRequisition(common.myInt(ViewState["_ID"]), common.myStr(txtReqNo.Text), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                //       common.myStr(ddlRequestType.SelectedValue), common.myInt(common.myInt(chkIsOuterRequest.Checked ? 1 : 0)), common.myInt(hdnRegistrationId.Value), common.myStr(hdnRegistrationNo.Value), common.myInt(hdnEncounterId.Value), common.myStr(hdnEncounterNo.Value),
                //       common.myDate(txtRequestDate.Text.Trim()), common.myStr(txtPatientName.Text.Trim()), common.myStr(txtGuardian.Text.Trim()), common.myInt(ddlSex.SelectedValue), common.myDate(dtpDateOfBirth.SelectedDate), common.myInt(txtYear.Text.Trim()), common.myInt(txtMonth.Text.Trim()), common.myInt(txtDays.Text.Trim()),
                //       common.myDbl(txtWeight.Text.Trim()), common.myInt(chbIsPediatric.Checked ? 1 : 0), common.myStr(txtChargeSlipNo.Text.Trim()), common.myInt(ddlPatientBloodGroup.SelectedValue), common.myInt(ddlMotherBloodGroup.SelectedValue), common.myInt(ddlReason.SelectedValue), common.myInt(chbIsExchangeTransformation.Checked ? 1 : 0),
                //       common.myDbl(txtHaematocritRequired.Text.Trim()), common.myInt(ddlIndicationforTransfusion.SelectedValue), common.myDate(dtpConsentDate.SelectedDate), common.myInt(ddlConsentTakenBy.SelectedValue),
                //       common.myStr(txtRemarks.Text.Trim()), 1, common.myInt(Session["UserID"].ToString()), strXML.ToString(), strXMLForQuestion.ToString(), Convert.ToSingle(common.myDbl(txtHB.Text)), Convert.ToSingle(common.myDbl(txtPltCount.Text)),
                //       Convert.ToSingle(common.myDbl(txtPT.Text)), Convert.ToSingle(common.myDbl(txtAPTT.Text)), Convert.ToSingle(common.myDbl(txtFIBRINOGEN.Text)),
                //       Convert.ToInt32(ddlCounsultant.SelectedValue), txtDiagnosis.Text.Trim(), txtWard.Text.Trim(), txtBedNo.Text.Trim(), Pregnancy, Miscarriage,common.myInt(hdnOrderId.Value), emergencyType
                //       , common.myInt(ddlHospitalList.SelectedValue)
                //       );

                //    if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
                //    {
                //        //Added by rakesh for order creation on 1/10/2014 start

                //        //Added by rakesh for order creation on 1/10/2014 end

                //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //        //clearControl();
                //        ViewState["_ID"] = 0;
                //        Session.Remove("GridData");
                //        SetPermission();
                //        //Added by rakesh start
                //        btnSaveData.Enabled = false;
                //        //Added by rakesh end

                //        //Added on 07-08-2014 Start   By Naushad
                //        if (strMsg.ToUpper().Contains("SAVE"))
                //        {
                //            ViewState["EncoutnerNo"] = common.myStr(Session["EncounterId"]);
                //            Div1.Visible = true;


                //        }
                //        //Added on 07-08-2014  End  By Naushad

                //    }
                //    lblMessage.Text = strMsg;
                //}
                //else
                //{
                if (common.myInt(ViewState["_ID"]) != 0)
                {
                    int isInvoiceGenerated = 0;
                    if (!hdnOrderId.Value.Equals("0"))
                    {
                        BaseC.clsBb objectBb = new BaseC.clsBb(sConString);
                        isInvoiceGenerated = objectBb.IsInvoiceGeneratedForOrder(common.myInt(hdnOrderId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                    }
                    if (isInvoiceGenerated.Equals(0))
                    {


                        BaseC.clsBb objectBb = new BaseC.clsBb(sConString);
                        int isInvoiceCanceled = 0;
                        isInvoiceCanceled = objectBb.IsOrderCancelledOfRequisitionID(common.myInt(hdnOrderId.Value));
                        if (isInvoiceCanceled.Equals("0"))
                        {
                            Alert.ShowAjaxMsg("Order can not be cancelled due to some problem.", Page);
                            return;
                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("You cann't update it because its Invoice has been generated.", Page);
                        return;
                    }
                }


                int orderID = SaveOrder();

                if (orderID > 0)
                {
                    objBb = new BaseC.clsBb(sConString);
                    string strDOBorAge = string.Empty;
                    if (!(dtpDateOfBirth.SelectedDate.Equals(string.Empty)))
                        strDOBorAge = "D";
                    else
                        strDOBorAge = "A";

                    bool Pregnancy = false;
                    bool Miscarriage = false;

                    if (rbtnpregnancy.SelectedIndex > -1)
                        Pregnancy = common.myBool(rbtnpregnancy.SelectedValue);
                    if (rbtnMiscarriage.SelectedIndex > -1)
                        Miscarriage = common.myBool(rbtnpregnancy.SelectedValue);




                    //string strMsg = "";
                    string strMsg = objBb.SaveComponentRequisition(common.myInt(ViewState["_ID"]), common.myStr(txtReqNo.Text), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                           common.myStr(ddlRequestType.SelectedValue), common.myInt(common.myInt(chkIsOuterRequest.Checked ? 1 : 0)), common.myInt(hdnRegistrationId.Value), common.myStr(hdnRegistrationNo.Value), common.myInt(hdnEncounterId.Value), common.myStr(hdnEncounterNo.Value),
                           common.myDate(txtRequestDate.Text.Trim()), common.myStr(txtPatientName.Text.Trim()), common.myStr(txtGuardian.Text.Trim()), common.myInt(ddlSex.SelectedValue), common.myDate(dtpDateOfBirth.SelectedDate), common.myInt(txtYear.Text.Trim()), common.myInt(txtMonth.Text.Trim()), common.myInt(txtDays.Text.Trim()),
                           common.myDbl(txtWeight.Text.Trim()), common.myInt(chbIsPediatric.Checked ? 1 : 0), common.myStr(txtChargeSlipNo.Text.Trim()), common.myInt(ddlPatientBloodGroup.SelectedValue), common.myInt(ddlMotherBloodGroup.SelectedValue), common.myInt(ddlReason.SelectedValue), common.myInt(chbIsExchangeTransformation.Checked ? 1 : 0),
                           common.myDbl(txtHaematocritRequired.Text.Trim()), common.myInt(ddlIndicationforTransfusion.SelectedValue), common.myDate(dtpConsentDate.SelectedDate), common.myInt(ddlConsentTakenBy.SelectedValue),
                           common.myStr(txtRemarks.Text.Trim()), 1, common.myInt(Session["UserID"].ToString()), strXML.ToString(), strXMLForQuestion.ToString(), Convert.ToSingle(common.myDbl(txtHB.Text)), Convert.ToSingle(common.myDbl(txtPltCount.Text)),
                           Convert.ToSingle(common.myDbl(txtPT.Text)), Convert.ToSingle(common.myDbl(txtAPTT.Text)), Convert.ToSingle(common.myDbl(txtFIBRINOGEN.Text)),
                           Convert.ToInt32(ddlCounsultant.SelectedValue), txtDiagnosis.Text.Trim(), txtWard.Text.Trim(), txtBedNo.Text.Trim(), Pregnancy, Miscarriage, orderID, emergencyType
                           , common.myInt(ddlHospitalList.SelectedValue), txtDoctorName.Text.Trim()
                           , common.myInt(SampleSend), common.myInt(ChbAnyTransfusion.Checked ? 1 : 0),
                           common.myInt(ChbAnyTransfusionReaction.Checked ? 1 : 0),
                           common.myInt(ChbIrradiatedComponent.Checked ? 1 : 0)

                           );

                    if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
                    {
                        //Added by rakesh for order creation on 1/10/2014 start

                        //Added by rakesh for order creation on 1/10/2014 end

                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        //clearControl();
                        ViewState["_ID"] = 0;
                        Session.Remove("GridData");
                        SetPermission();
                        //Added by rakesh start
                        btnSaveData.Enabled = false;
                        //Added by rakesh end

                        //Added on 07-08-2014 Start   By Naushad
                        if (strMsg.ToUpper().Contains("SAVE"))
                        {
                            ViewState["EncoutnerNo"] = common.myStr(Session["EncounterId"]);
                            Div1.Visible = true;


                        }
                        //Added on 07-08-2014  End  By Naushad

                    }
                    lblMessage.Text = strMsg;



                }
                else
                {
                    Alert.ShowAjaxMsg("There is some problem.", Page);
                    return;
                }
                //}

             */
                //string sChargeCalculationRequired = "Y";
                #endregion

                string stype = string.Empty;
                string opip = string.Empty;

                int registrationId; Int64 registrationNo;
                //int encounterId;
                if (Session["RegistrationId"] != null)
                {
                    registrationId = common.myInt(Session["RegistrationId"]);
                    registrationNo = common.myLong(Session["RegistrationNo"]);
                }
                else
                {
                    registrationId = common.myInt(hdnRegistrationId.Value);
                    registrationNo = common.myLong(hdnRegistrationNo.Value);
                }
                if (Session["EncounterId"] == null && hdnEncounterId.Value.Equals(string.Empty))
                {
                    BaseC.clsBb objclsBb = new BaseC.clsBb(sConString);
                    Hashtable hshOutObj = new Hashtable();
                    hshOutObj = objclsBb.CreateEncounterIFNotExistsForBloodBank(Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(Session["FacilityId"].ToString()),
                                     registrationNo, registrationId,
                                    common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]) // To swapping between current userid & transaction userid
                                     , common.myInt(ddlCounsultant.SelectedValue));
                    if (hshOutObj["chvErrorStatus"].ToString().Length == 0)
                    {
                        hdnEncounterId.Value = hshOutObj["intEncounterID"].ToString();
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "There is some error while creating encounter";
                    }
                }
                objBb = new BaseC.clsBb(sConString);
                string strDOBorAge = string.Empty;
                if (!(dtpDateOfBirth.SelectedDate.Equals(string.Empty)))
                    strDOBorAge = "D";
                else
                    strDOBorAge = "A";

                bool Pregnancy = false;
                bool Miscarriage = false;

                if (rbtnpregnancy.SelectedIndex > -1)
                    Pregnancy = common.myBool(rbtnpregnancy.SelectedValue);
                if (rbtnMiscarriage.SelectedIndex > -1)
                    Miscarriage = common.myBool(rbtnpregnancy.SelectedValue);


                //string strMsg = "";
                #region"09022018"
                ////string strMsg = objBb.SaveComponentRequisition(common.myInt(ViewState["_ID"]), common.myStr(txtReqNo.Text), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                ////   common.myStr(ddlRequestType.SelectedValue), common.myInt(common.myInt(chkIsOuterRequest.Checked ? 1 : 0)), common.myInt(hdnRegistrationId.Value), common.myStr(hdnRegistrationNo.Value), common.myInt(hdnEncounterId.Value), common.myStr(hdnEncounterNo.Value),
                ////   common.myDate(txtRequestDate.Text.Trim()), common.myStr(txtPatientName.Text.Trim()), common.myStr(txtGuardian.Text.Trim()), common.myInt(ddlSex.SelectedValue), common.myDate(dtpDateOfBirth.SelectedDate), common.myInt(txtYear.Text.Trim()), common.myInt(txtMonth.Text.Trim()), common.myInt(txtDays.Text.Trim()),
                ////   common.myDbl(txtWeight.Text.Trim()), common.myInt(chbIsPediatric.Checked ? 1 : 0), common.myStr(txtChargeSlipNo.Text.Trim()), common.myInt(ddlPatientBloodGroup.SelectedValue), common.myInt(ddlMotherBloodGroup.SelectedValue), common.myInt(ddlReason.SelectedValue), common.myInt(chbIsExchangeTransformation.Checked ? 1 : 0),
                ////   common.myDbl(txtHaematocritRequired.Text.Trim()), common.myInt(ddlIndicationforTransfusion.SelectedValue), common.myDate(dtpConsentDate.SelectedDate), common.myInt(ddlConsentTakenBy.SelectedValue),
                ////   common.myStr(txtRemarks.Text.Trim()), 1, common.myInt(Session["UserID"].ToString()), strXML.ToString(), strXMLForQuestion.ToString(), Convert.ToSingle(common.myDbl(txtHB.Text)), Convert.ToSingle(common.myDbl(txtPltCount.Text)),
                ////   Convert.ToSingle(common.myDbl(txtPT.Text)), Convert.ToSingle(common.myDbl(txtAPTT.Text)), Convert.ToSingle(common.myDbl(txtFIBRINOGEN.Text)),
                ////   Convert.ToInt32(ddlCounsultant.SelectedValue), txtDiagnosis.Text.Trim(), txtWard.Text.Trim(), txtBedNo.Text.Trim(), Pregnancy, Miscarriage, 0, emergencyType, common.myInt(ddlHospitalList.SelectedValue), txtDoctorName.Text.Trim()
                ////   , common.myInt(SampleSend), common.myInt(ChbAnyTransfusion.Checked ? 1 : 0),
                ////           common.myInt(ChbAnyTransfusionReaction.Checked ? 1 : 0),
                ////           common.myInt(ChbIrradiatedComponent.Checked ? 1 : 0)
                ////   );
                #endregion
                #region
                string strMsg = objBb.SaveComponentRequisition(common.myInt(ViewState["_ID"]), common.myStr(txtReqNo.Text), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                 common.myStr(ddlRequestType.SelectedValue), common.myInt(common.myInt(chkIsOuterRequest.Checked ? 1 : 0)), common.myInt(hdnRegistrationId.Value), common.myStr(hdnRegistrationNo.Value), common.myInt(hdnEncounterId.Value), common.myStr(hdnEncounterNo.Value),
                 common.myDate(txtRequestDate.Text.Trim()), common.myStr(txtPatientName.Text.Trim()), common.myStr(txtGuardian.Text.Trim()), common.myInt(ddlSex.SelectedValue), common.myDate(dtpDateOfBirth.SelectedDate), common.myInt(txtYear.Text.Trim()), common.myInt(txtMonth.Text.Trim()), common.myInt(txtDays.Text.Trim()),
                 common.myDbl(txtWeight.Text.Trim()), common.myInt(chbIsPediatric.Checked ? 1 : 0), common.myStr(txtChargeSlipNo.Text.Trim()), common.myInt(ddlPatientBloodGroup.SelectedValue), common.myInt(ddlMotherBloodGroup.SelectedValue), common.myInt(ddlReason.SelectedValue), common.myInt(chbIsExchangeTransformation.Checked ? 1 : 0),
                 common.myDbl(txtHaematocritRequired.Text.Trim()), common.myInt(ddlIndicationforTransfusion.SelectedValue), common.myDate(dtpConsentDate.SelectedDate), common.myInt(ddlConsentTakenBy.SelectedValue),
                 common.myStr(txtRemarks.Text.Trim()), 1,
                 common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                 strXML.ToString(), strXMLForQuestion.ToString(), Convert.ToSingle(common.myDbl(txtHB.Text)), txtPltCount.Text,
                 Convert.ToSingle(common.myDbl(txtPT.Text)), Convert.ToSingle(common.myDbl(txtAPTT.Text)), Convert.ToSingle(common.myDbl(txtFIBRINOGEN.Text)),
                 Convert.ToInt32(ddlCounsultant.SelectedValue), txtDiagnosis.Text.Trim(), txtWard.Text.Trim(), txtBedNo.Text.Trim(), Pregnancy, Miscarriage, 0, emergencyType, common.myInt(ddlHospitalList.SelectedValue), txtDoctorName.Text.Trim()
                 , common.myInt(SampleSend), common.myInt(ChbAnyTransfusion.Checked ? 1 : 0),
                         common.myInt(ChbAnyTransfusionReaction.Checked ? 1 : 0),
                         common.myInt(ChbIrradiatedComponent.Checked ? 1 : 0)
                         , common.myInt(ddlotherhospital.SelectedValue)
                         , common.myStr(txtothermrdno.Text)
                         , common.myStr(txtotherwardname.Text)
                         , common.myStr(txtotherBedNo.Text)
                         , common.myStr(txtotherDoctor.Text)

                 );

                #endregion
                #region"SAVECOMPONENT REQUISITION FOR PLATECOUNT DATATYPE TO BE STRING FOR BLK"

                // string strMsg = objBb.SaveComponentRequisition(common.myInt(ViewState["_ID"]), common.myStr(txtReqNo.Text), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                //common.myStr(ddlRequestType.SelectedValue), common.myInt(common.myInt(chkIsOuterRequest.Checked ? 1 : 0)), common.myInt(hdnRegistrationId.Value), common.myStr(hdnRegistrationNo.Value), common.myInt(hdnEncounterId.Value), common.myStr(hdnEncounterNo.Value),
                //common.myDate(txtRequestDate.Text.Trim()), common.myStr(txtPatientName.Text.Trim()), common.myStr(txtGuardian.Text.Trim()), common.myInt(ddlSex.SelectedValue), common.myDate(dtpDateOfBirth.SelectedDate), common.myInt(txtYear.Text.Trim()), common.myInt(txtMonth.Text.Trim()), common.myInt(txtDays.Text.Trim()),
                //common.myDbl(txtWeight.Text.Trim()), common.myInt(chbIsPediatric.Checked ? 1 : 0), common.myStr(txtChargeSlipNo.Text.Trim()), common.myInt(ddlPatientBloodGroup.SelectedValue), common.myInt(ddlMotherBloodGroup.SelectedValue), common.myInt(ddlReason.SelectedValue), common.myInt(chbIsExchangeTransformation.Checked ? 1 : 0),
                //common.myDbl(txtHaematocritRequired.Text.Trim()), common.myInt(ddlIndicationforTransfusion.SelectedValue), common.myDate(dtpConsentDate.SelectedDate), common.myInt(ddlConsentTakenBy.SelectedValue),
                //common.myStr(txtRemarks.Text.Trim()), 1, common.myInt(Session["UserID"].ToString()), strXML.ToString(), strXMLForQuestion.ToString(), Convert.ToSingle(common.myDbl(txtHB.Text)), Convert.ToSingle(common.myDbl(txtPltCount.Text)),
                //Convert.ToSingle(common.myDbl(txtPT.Text)), Convert.ToSingle(common.myDbl(txtAPTT.Text)), Convert.ToSingle(common.myDbl(txtFIBRINOGEN.Text)),
                //Convert.ToInt32(ddlCounsultant.SelectedValue), txtDiagnosis.Text.Trim(), txtWard.Text.Trim(), txtBedNo.Text.Trim(), Pregnancy, Miscarriage, 0, emergencyType, common.myInt(ddlHospitalList.SelectedValue), txtDoctorName.Text.Trim()
                //, common.myInt(SampleSend), common.myInt(ChbAnyTransfusion.Checked ? 1 : 0),
                //        common.myInt(ChbAnyTransfusionReaction.Checked ? 1 : 0),
                //        common.myInt(ChbIrradiatedComponent.Checked ? 1 : 0)
                //);

                #endregion
                if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    //clearControl();
                    ViewState["_ID"] = 0;
                    Session.Remove("GridData");
                    SetPermission();
                    //Added by rakesh start
                    btnSaveData.Enabled = false;
                    //Added by rakesh end
                    ddlRequestType.SelectedValue = "0";
                    txtDiagnosis.Text = "";
                    //Added on 07-08-2014 Start   By Naushad
                    if (strMsg.ToUpper().Contains("SAVE"))
                    {
                        ViewState["EncoutnerNo"] = common.myStr(Session["EncounterId"]);
                        Div1.Visible = true;

                    }
                    //Added on 07-08-2014  End  By Naushad

                }
                lblMessage.Text = strMsg;

            }
            else
            {
                objBb = new BaseC.clsBb(sConString);
                string strDOBorAge = string.Empty;
                if (!(dtpDateOfBirth.SelectedDate.Equals(string.Empty)))
                    strDOBorAge = "D";
                else
                    strDOBorAge = "A";

                bool Pregnancy = false;
                bool Miscarriage = false;

                if (rbtnpregnancy.SelectedIndex > -1)
                    Pregnancy = common.myBool(rbtnpregnancy.SelectedValue);
                if (rbtnMiscarriage.SelectedIndex > -1)
                    Miscarriage = common.myBool(rbtnpregnancy.SelectedValue);


                //string strMsg = "";
                string strMsg = objBb.SaveComponentRequisition(common.myInt(ViewState["_ID"]), common.myStr(txtReqNo.Text), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                   common.myStr(ddlRequestType.SelectedValue), common.myInt(common.myInt(chkIsOuterRequest.Checked ? 1 : 0)), common.myInt(hdnRegistrationId.Value), common.myStr(hdnRegistrationNo.Value), common.myInt(hdnEncounterId.Value), common.myStr(hdnEncounterNo.Value),
                   common.myDate(txtRequestDate.Text.Trim()), common.myStr(txtPatientName.Text.Trim()), common.myStr(txtGuardian.Text.Trim()), common.myInt(ddlSex.SelectedValue), common.myDate(dtpDateOfBirth.SelectedDate), common.myInt(txtYear.Text.Trim()), common.myInt(txtMonth.Text.Trim()), common.myInt(txtDays.Text.Trim()),
                   common.myDbl(txtWeight.Text.Trim()), common.myInt(chbIsPediatric.Checked ? 1 : 0), common.myStr(txtChargeSlipNo.Text.Trim()), common.myInt(ddlPatientBloodGroup.SelectedValue), common.myInt(ddlMotherBloodGroup.SelectedValue), common.myInt(ddlReason.SelectedValue), common.myInt(chbIsExchangeTransformation.Checked ? 1 : 0),
                   common.myDbl(txtHaematocritRequired.Text.Trim()), common.myInt(ddlIndicationforTransfusion.SelectedValue), common.myDate(dtpConsentDate.SelectedDate), common.myInt(ddlConsentTakenBy.SelectedValue),
                   common.myStr(txtRemarks.Text.Trim()), 1,
                   common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                   strXML.ToString(), strXMLForQuestion.ToString(), Convert.ToSingle(common.myDbl(txtHB.Text)), txtPltCount.Text,
                   Convert.ToSingle(common.myDbl(txtPT.Text)), Convert.ToSingle(common.myDbl(txtAPTT.Text)), Convert.ToSingle(common.myDbl(txtFIBRINOGEN.Text)),
                   Convert.ToInt32(ddlCounsultant.SelectedValue), txtDiagnosis.Text.Trim(), txtWard.Text.Trim(), txtBedNo.Text.Trim(), Pregnancy, Miscarriage, 0, emergencyType, common.myInt(ddlHospitalList.SelectedValue), txtDoctorName.Text.Trim()
                   , common.myInt(SampleSend), common.myInt(ChbAnyTransfusion.Checked ? 1 : 0),
                           common.myInt(ChbAnyTransfusionReaction.Checked ? 1 : 0),
                           common.myInt(ChbIrradiatedComponent.Checked ? 1 : 0)
                           , common.myInt(ddlotherhospital.SelectedValue)
                           , common.myStr(txtothermrdno.Text)
                           , common.myStr(txtotherwardname.Text)
                           , common.myStr(txtotherBedNo.Text)
                           , common.myStr(txtotherDoctor.Text)

                   );
                #region"SAVECOMPONENT REQUISITION FOR PLATECOUNT DATATYPE TO BE STRING FOR BLK"
                //string strMsg = objBb.SaveComponentRequisition(common.myInt(ViewState["_ID"]), common.myStr(txtReqNo.Text), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                //common.myStr(ddlRequestType.SelectedValue), common.myInt(common.myInt(chkIsOuterRequest.Checked ? 1 : 0)), common.myInt(hdnRegistrationId.Value), common.myStr(hdnRegistrationNo.Value), common.myInt(hdnEncounterId.Value), common.myStr(hdnEncounterNo.Value),
                //common.myDate(txtRequestDate.Text.Trim()), common.myStr(txtPatientName.Text.Trim()), common.myStr(txtGuardian.Text.Trim()), common.myInt(ddlSex.SelectedValue), common.myDate(dtpDateOfBirth.SelectedDate), common.myInt(txtYear.Text.Trim()), common.myInt(txtMonth.Text.Trim()), common.myInt(txtDays.Text.Trim()),
                //common.myDbl(txtWeight.Text.Trim()), common.myInt(chbIsPediatric.Checked ? 1 : 0), common.myStr(txtChargeSlipNo.Text.Trim()), common.myInt(ddlPatientBloodGroup.SelectedValue), common.myInt(ddlMotherBloodGroup.SelectedValue), common.myInt(ddlReason.SelectedValue), common.myInt(chbIsExchangeTransformation.Checked ? 1 : 0),
                //common.myDbl(txtHaematocritRequired.Text.Trim()), common.myInt(ddlIndicationforTransfusion.SelectedValue), common.myDate(dtpConsentDate.SelectedDate), common.myInt(ddlConsentTakenBy.SelectedValue),
                //common.myStr(txtRemarks.Text.Trim()), 1, common.myInt(Session["UserID"].ToString()), strXML.ToString(), strXMLForQuestion.ToString(), Convert.ToSingle(common.myDbl(txtHB.Text)), txtPltCount.Text,
                //Convert.ToSingle(common.myDbl(txtPT.Text)), Convert.ToSingle(common.myDbl(txtAPTT.Text)), Convert.ToSingle(common.myDbl(txtFIBRINOGEN.Text)),
                //Convert.ToInt32(ddlCounsultant.SelectedValue), txtDiagnosis.Text.Trim(), txtWard.Text.Trim(), txtBedNo.Text.Trim(), Pregnancy, Miscarriage, 0, emergencyType, common.myInt(ddlHospitalList.SelectedValue), txtDoctorName.Text.Trim()
                //, common.myInt(SampleSend), common.myInt(ChbAnyTransfusion.Checked ? 1 : 0),
                //        common.myInt(ChbAnyTransfusionReaction.Checked ? 1 : 0),
                //        common.myInt(ChbIrradiatedComponent.Checked ? 1 : 0)

                //);
                #endregion
                if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    //clearControl();
                    ViewState["_ID"] = 0;
                    Session.Remove("GridData");
                    SetPermission();
                    //Added by rakesh start
                    btnSaveData.Enabled = false;
                    //Added by rakesh end

                    //Added on 07-08-2014 Start   By Naushad
                    if (strMsg.ToUpper().Contains("SAVE"))
                    {
                        ViewState["EncoutnerNo"] = common.myStr(Session["EncounterId"]);
                        Div1.Visible = true;

                    }
                    //Added on 07-08-2014  End  By Naushad

                }
                lblMessage.Text = strMsg;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        clearControl();
    }
    public void clearControl()
    {
        ddlRequestType.SelectedValue = "0";
        txtDiagnosis.Text = string.Empty;
    }

    //Added on 08-07-2014 Start Naushad
    protected void btnClosepopup_OnClick(object sender, EventArgs e)
    {
        Div1.Visible = false;
    }

    protected void btnPrintDetail_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["EncoutnerNo"]) != "")
        {
            string Act = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "BBBloodRequestPrintLabelByPRN", sConString);
            if (Act == "Y")
            {
                // for (int i = 1; i <= common.myInt(ddlNoofPrint.SelectedValue); i++)
                {
                    string Str = common.myInt(Session["FacilityID"]) + "$" + common.myStr(ViewState["EncoutnerNo"]) + "$" + "0" + "$" + "PrintPRN_BloodBankCompReqLabel";
                    Str = "asplprint:" + Str;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
                }
                return;
            }
            else
            {
                //RadWindow1.NavigateUrl = "/EMRReports/ReportBarCode.aspx?EncNo=" + common.myStr(ViewState["EncoutnerNo"]) + "&ReportName=BloodBarCodeLabel";
                RadWindow1.NavigateUrl = "/EMRReports/ReportBarCode.aspx?EncNo=" + common.myStr(ViewState["EncoutnerNo"]) + "&RequsitionID=0&ReportName=BloodBarCodeLabel";
                RadWindow1.Width = 850;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.VisibleOnPageLoad = true;
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
        }

    }

    private void LoadBloodComponent()
    {
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        try
        {
            hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"])));
            objBb = new BaseC.clsBb(sConString);
            DataSet ds = null;
            switch (RadioButtonList1.SelectedIndex)
            {
                case 0:
                    ds = objBb.GetComponentRequisitionBloodComponentMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, 1);
                    break;
                case 1:
                    ds = objBb.GetComponentRequisitionBloodComponentMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1, 1);
                    break;
            }
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ddlComponent.Items.Clear();

                RadComboBoxItem item1 = new RadComboBoxItem("Select", "0");
                //item.Attributes.Add("sizeinml", common.myDbl(row["DefaultML"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value)));
                ddlComponent.Items.Add(item1);

                //ddlComponent.Items.Add("[Select]", 0);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem(row["ComponentName"].ToString(), row["ComponentId"].ToString());
                    item.Attributes.Add("sizeinml", common.myDbl(row["DefaultML"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value)));
                    //Added by rakesh for order creation on 1/10/2014 start
                    item.Attributes.Add("CrossMatchServiceId", common.myStr(row["CrossMatchServiceId"]));
                    //Added by rakesh for order creation on 1/10/2014 end
                    item.Attributes.Add("IsCrossMatchRequired", common.myStr(row["IsCrossMatchRequired"]));
                    ddlComponent.Items.Add(item);
                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        finally
        {
            objBill = null;
        }

    }

    protected void lnkAddComponent_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (txtNoOfUnit.Text.Trim().Equals(string.Empty) || common.myInt(txtNoOfUnit.Text.Trim()) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please enter no of unit(s).";
            return;
        }

        if (txtSize.Text.Trim().Equals(string.Empty) || common.myInt(txtSize.Text.Trim()) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Quantity can not be blank.";
            return;
        }

        DataTable table = (DataTable)ViewState["GridData"];

        /*
      to remove first blank or default row from grid if qty data column is blank
       */



        foreach (DataRow row in table.Rows)
        {
            if (row["ComponentID"].ToString().Equals(ddlComponent.SelectedValue.ToString()) && lnkAddComponent.Text.Trim().Equals("Add Component"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Duplicate component not allowed.";
                return;
            }
            if (row["CrossMatchServiceId"].ToString().Equals(ddlComponent.SelectedItem.Attributes["CrossMatchServiceId"]) && lnkAddComponent.Text.Trim().Equals("Add Component"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "The single service is mapped for more than one selected component.";
                return;
            }
        }
        //for (int i = 0; i <= table.Rows.Count - 1; i++)
        //{
        //    if (table.Rows[i]["ComponentID"].ToString().Equals(ddlComponent.SelectedValue.ToString()) && lnkAddComponent.Text.Trim().Equals("Add Component"))
        //    {
        //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //        lblMessage.Text = "Duplicate component not allowed.";
        //        return;
        //    }        
        //}
        //for (int i = 0; i <= table.Rows.Count - 1; i++)
        //{
        //    if (table.Rows[i]["CrossMatchServiceId"].ToString().Equals(ddlComponent.SelectedItem.Attributes["CrossMatchServiceId"]) && lnkAddComponent.Text.Trim().Equals("Add Component"))
        //    {
        //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //        lblMessage.Text = "The single service is mapped for more than one selected component.";
        //        return;
        //    }
        //}
        //DR["CrossMatchServiceId"] = common.myInt(ddlComponent.SelectedItem.Attributes["CrossMatchServiceId"]);
        if (dtpRequestDate.SelectedDate == null)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Component request date cannot be empty.";
            return;
        }
        string hold = table.Rows[0]["Qty"].ToString();
        if (hold == "")
        {
            table.Rows.RemoveAt(0);
        }

        switch (lnkAddComponent.Text.Trim())
        {
            case "Add Component":
                DataRow DR = table.NewRow();
                DR["RequisitionId"] = 0;
                DR["ComponentId"] = common.myInt(ddlComponent.SelectedValue);
                DR["ComponentName"] = common.myStr(ddlComponent.SelectedItem.Text.ToString().Trim());
                DR["Qty"] = common.myDbl(txtNoOfUnit.Text.Trim());

                DR["RequestDate"] = Convert.ToDateTime(dtpRequestDate.SelectedDate).ToString("dd/MM/yyyy");

                DR["SizeML"] = common.myDbl(txtSize.Text.Trim());
                //Added by rakesh for order creation on 1/10/2014 start
                DR["CrossMatchServiceId"] = common.myInt(ddlComponent.SelectedItem.Attributes["CrossMatchServiceId"]);
                //Added by rakesh for order creation on 1/10/2014 end
                DR["IsCrossMatchRequired"] = common.myBool(ddlComponent.SelectedItem.Attributes["IsCrossMatchRequired"]);
                table.Rows.Add(DR);
                gvNewComponent.DataSource = table;
                gvNewComponent.DataBind();
                ViewState["GridData"] = table;
                break;
            case "Update Component":
                DR = table.Rows[common.myInt(hdnUniqueIdOuter.Value)];
                DR["RequisitionId"] = 0;
                DR["ComponentId"] = common.myInt(ddlComponent.SelectedValue);

                DR["ComponentName"] = common.myStr(ddlComponent.SelectedItem.Text.ToString().Trim());
                DR["Qty"] = common.myDbl(txtNoOfUnit.Text.Trim());
                DR["RequestDate"] = Convert.ToDateTime(dtpRequestDate.SelectedDate.Value.Date).ToString("dd/MM/yyyy");

                DR["SizeML"] = common.myDbl(txtSize.Text.Trim());

                //Added by rakesh for order creation on 1/10/2014 start
                DR["CrossMatchServiceId"] = common.myInt(ddlComponent.Attributes["CrossMatchServiceId"]);
                //Added by rakesh for order creation on 1/10/2014 end
                DR["IsCrossMatchRequired"] = common.myBool(ddlComponent.SelectedItem.Attributes["IsCrossMatchRequired"]);
                gvNewComponent.DataSource = table;
                gvNewComponent.DataBind();

                ViewState["GridData"] = table;
                ddlComponent.Enabled = true;
                lnkAddComponent.Text = "Add Component";
                break;
        }
    }

    protected void BindBlankNewComponent()
    {
        try
        {
            objBb = new BaseC.clsBb(sConString);
            int RequisitionId = common.myInt(Request.QueryString["RequisitionId"]);
            dsComponentRequisitionDetails = objBb.GetComponentRequisitionDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), RequisitionId == 0 ? -1 : RequisitionId, 0, 1);

            if (dsComponentRequisitionDetails.Tables[0].Rows.Count == 0)
            {
                DataRow DR = dsComponentRequisitionDetails.Tables[0].NewRow();
                DR["RequestDate"] = System.DBNull.Value;
                dsComponentRequisitionDetails.Tables[0].Rows.Add(DR);
                dsComponentRequisitionDetails.AcceptChanges();
            }

            dsComponentRequisitionDetails.Tables[0].Columns.Add("id", typeof(Int32));
            dsComponentRequisitionDetails.Tables[0].Columns["id"].AutoIncrement = true;
            ViewState["GridData"] = dsComponentRequisitionDetails.Tables[0];
            gvNewComponent.DataSource = dsComponentRequisitionDetails.Tables[0];
            gvNewComponent.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadBloodComponent();
    }

    protected void ddlComponent_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        txtSize.Text = common.myStr(common.myInt(ddlComponent.SelectedItem.Attributes["sizeinml"]));
    }

    protected void dtpRequestDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {

    }

    //Added on 08-07-2014 End Naushad  

    protected void btnSaveData_OnClick(object sender, EventArgs e)
    {
        //string multiple = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "COMPONENTREQUISITIONHIDEOLDHOSPITALDOCTORNAME", sConString);
        isSave();
    }
    protected void ibtnUpload_Click(object sender, EventArgs e)
    {

    }

    protected void imgCalYear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (dtpDateOfBirth.SelectedDate != null)
            {
                BaseC.Patient bC = new BaseC.Patient(sConString);
                txtYear.Text = "";
                txtMonth.Text = "";
                txtDays.Text = "";
                DateTime datet = dtpDateOfBirth.SelectedDate.Value;
                // bC.DOB = datet.ToString("dd/MM/yyyy");
                //string[] result = bC.CalculateAge(datet.ToString("MM/dd/yyyy"));

                string[] result = bC.CalculateAge(datet.ToString("yyyy/MM/dd"));

                if (result.Length == 2)
                {
                    if (result[1] == "Yr")
                    {
                        txtYear.Text = result[0];
                    }
                    else if (result[1] == "Mnth")
                    {
                        txtMonth.Text = result[0];

                    }
                    else
                    {
                        txtDays.Text = result[0];
                    }
                }

                if (result.Length == 4)
                {
                    //txtAgeYears.Text = result[0];
                    txtMonth.Text = result[0];
                    txtDays.Text = result[2];
                }
                if (result.Length == 6)
                {
                    txtYear.Text = result[0];
                    txtMonth.Text = result[2];
                    txtDays.Text = result[4];
                }

                if (txtYear.Text == "")
                {
                    txtYear.Text = "0";
                }
                if (txtMonth.Text == "")
                {
                    txtMonth.Text = "0";
                }
                if (txtDays.Text == "")
                {
                    txtDays.Text = "0";
                }
            }
            txtYear.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCalAge_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BaseC.ParseData bc = new BaseC.ParseData();
            Hashtable hshIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            String SqlStr = "select dbo.DOBFromAge(" + val(bc.ParseQ(txtYear.Text)) + "," + val(bc.ParseQ(txtMonth.Text)) + "," + val(bc.ParseQ(txtDays.Text)) + ")";

            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, SqlStr, hshIn);
            dr.Read();
            dtpDateOfBirth.SelectedDate = common.myDate(dr[0].ToString());
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void txtYear_TextChanged(Object sender, EventArgs e)
    {
        DOBCalc();
        if (txtYear.Text != "")
        {
            txtMonth.Focus();
        }
        else
        {
            dtpDateOfBirth.SelectedDate = null;
            txtYear.Text = "";
            txtYear.Focus();
        }
    }
    protected void txtMonth_TextChanged(Object sender, EventArgs e)
    {
        DOBCalc();
        txtDays.Focus();
    }
    protected void txtDays_TextChanged(Object sender, EventArgs e)
    {
        DOBCalc();
        //ddlGuardian.Focus();
    }
    void DOBCalc()
    {
        try
        {
            int year = common.myInt(txtYear.Text);
            int month = common.myInt(txtMonth.Text);
            int day = common.myInt(txtDays.Text);

            DateTime DOB = DateTime.Now.AddDays(-day);
            DOB = DOB.AddMonths(-month);
            DOB = DOB.AddYears(-year);
            if (dtpDateOfBirth.MinDate < common.myDate(DOB))
            {
                ViewState["DOB"] = common.myDate(DOB);
                //dtpDateOfBirth.SelectedDate = common.myDate(DOB); 
            }
            else
            {
                txtYear.Text = "";
                //DOBCalc();
                txtYear.Focus();
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    Double val(String value)
    {
        Double intData = 0;
        //Boolean blnTemp = Double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat, out intData);
        return intData;
    }

    private void bindBloodGroup()
    {
        DataSet ds = new DataSet();
        try
        {
            objBb = new BaseC.clsBb(sConString);
            ds = objBb.GetComponentRequisitionBloodGroup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1);
            ddlPatientBloodGroup.DataSource = ds.Tables[0];
            ddlPatientBloodGroup.DataValueField = "BloodGroupId";
            ddlPatientBloodGroup.DataTextField = "BloodGroupDescription";
            ddlPatientBloodGroup.DataBind();
            ddlPatientBloodGroup.Items.Insert(0, new RadComboBoxItem("", "0"));

            ddlMotherBloodGroup.DataSource = ds.Tables[0];

            ddlMotherBloodGroup.DataValueField = "BloodGroupId";
            ddlMotherBloodGroup.DataTextField = "BloodGroupDescription";
            ddlMotherBloodGroup.DataBind();
            ddlMotherBloodGroup.Items.Insert(0, new RadComboBoxItem("", "0"));

        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            ds.Dispose();
        }
    }
    private void BindDoctor()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = objCM.GetDoctorList(Convert.ToInt16(Session["HospitalLocationID"]), 0, Convert.ToInt16(Session["FacilityID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCounsultant.DataSource = ds.Tables[0];
                ddlCounsultant.DataTextField = "DoctorName";
                ddlCounsultant.DataValueField = "DoctorId";
                ddlCounsultant.DataBind();

                ddlConsentTakenBy.DataSource = ds.Tables[0];
                ddlConsentTakenBy.DataTextField = "DoctorName";
                ddlConsentTakenBy.DataValueField = "DoctorId";
                ddlConsentTakenBy.DataBind();

            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
        }
    }

    //protected void PopulateDoctor()
    // {
    //     try
    //     {
    //         ddlConsentTakenBy.Enabled = true;
    //         ddlConsentTakenBy.Enabled = true;
    //         //BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
    //         //DataSet ds = objEMRVitals.GetDoctors(Convert.ToInt16(Session["HospitalLocationID"]));
    //         Hashtable hstInput;
    //         hstInput = new Hashtable();
    //         hstInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
    //         DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //         DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", hstInput);

    //         if (ds.Tables[0].Rows.Count>0)
    //         {
    //             ddlConsentTakenBy.DataSource = ds;
    //             ddlConsentTakenBy.DataTextField = "EmployeeName";
    //             ddlConsentTakenBy.DataValueField = "EmployeeId";
    //             ddlConsentTakenBy.DataBind();


    //         }
    //         ddlConsentTakenBy.Items.Insert(0, new RadComboBoxItem("[Select]", "0"));

    //     }
    //     catch (Exception Ex)
    //     {
    //         lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //         lblMessage.Text = "Error: " + Ex.Message;
    //         objException.HandleException(Ex);
    //     }
    // }
    private void BindIndication()
    {
        try
        {

            Hashtable hsIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //hsIn.Add("@id", 0);
            hsIn.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetindicationMaster", hsIn);
            ddlIndicationforTransfusion.DataSource = ds;
            ddlIndicationforTransfusion.DataTextField = "IndicationDescription";
            ddlIndicationforTransfusion.DataValueField = "IndicationId";
            ddlIndicationforTransfusion.DataBind();
            //Added by rakesh start
            ddlIndicationforTransfusion.Items.Insert(0, new RadComboBoxItem("[Select]", "0"));
            //Added by rakesh end
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    /* private void PopulateDoctor()
    {
        try
        {
            
            Hashtable hsIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsIn.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", hsIn);
            ddlConsentTakenBy.DataSource = ds;
            ddlConsentTakenBy.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    } */

    protected void fillRemarks()
    {
        common common = new common();
        DataSet ds = common.GetReasonMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "BBREQUEST", sConString);
        ddlReason.DataSource = ds;
        ddlReason.DataTextField = "Reason";
        ddlReason.DataValueField = "Id";
        ddlReason.DataBind();
    }

    protected void lbtnSearchDonor_Click(object sender, EventArgs e)
    {
        ViewState["OPIP"] = "O";

        if (common.myStr(Request.QueryString["Type"]) == "OP")
        {
            RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=O&RegEnc=5";
        }
        else
        {
            RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=O&RegEnc=0";
        }


        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void lbtnSearchDonor2_Click(object sender, EventArgs e)
    {
        ViewState["OPIP"] = "I";
        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=1";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;

    }

    private void PatientInfo()
    {
        try
        {
            if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
            else
            {
                if (common.myInt(hdnRegistrationId.Value) > 0)
                {
                    DataSet ds = new DataSet();
                    string sXSL = "";
                    //txtAccountNo.Text = objCM.GetPatientRegistrationNo(Convert.ToInt32(txtRegNo.Text.Trim()));
                    objCM = new BaseC.RestFulAPI(sConString);
                    ds = objCM.getPatientDetails(common.myLong(hdnRegistrationNo.Value.Trim()), 1, common.myInt(Session["HospitalLocationID"]));
                    sXSL = "/include/xsl/PatientInfo-US.xsl";

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0][0].ToString() != "Patient Is Expired, No Transaction Allowed!")
                            {
                                xmlPatientInfo.DocumentContent = ds.Tables[0].Rows[0][0].ToString();
                                xmlPatientInfo.TransformSource = sXSL;
                                xmlPatientInfo.DataBind();
                            }
                            else
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = ds.Tables[0].Rows[0][0].ToString();
                                xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                                xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                                xmlPatientInfo.DataBind();
                            }
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("No patient record found ", Page);
                            xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                            xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                            xmlPatientInfo.DataBind();
                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("No patient record found ", Page);
                        xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                        xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                        xmlPatientInfo.DataBind();
                    }
                }
                else
                {
                    xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                    xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                    xmlPatientInfo.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        objp = new BaseC.Patient(sConString);
        string Regid = "";
        string EncounterIdAndEncounterNo = "";
        if (Convert.ToInt32(Request.QueryString["RNo"]) != 0)
        {
            hdnRegistrationNo.Value = Convert.ToString(Request.QueryString["RNo"]);
        }


        if ((hdnRegistrationNo.Value != "" && hdnRegistrationNo.Value != "0"))
        {

            //Regid = (string)objp.getRegistrationIDFromRegistrationNo(common.myStr(hdnRegistrationNo.Value), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])); //common.myInt(Session["FacilityId"]));
            /*
            EncounterIdAndEncounterNo = (string)objp.getEncounterIDAndEncounterNoFromRegistrationNo(common.myStr(hdnRegistrationNo.Value), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])); //common.myInt(Session["FacilityId"]));
            string[] store=EncounterIdAndEncounterNo.Split(new char[]{'-'});
            hdnEncounterId.Value = store[0];
            hdnEncounterNo.Value = store[1];
            */
            if (common.myInt(hdnRegistrationId.Value) == 0)
            {
                hdnRegistrationId.Value = Regid;
                Session["registrationId"] = hdnRegistrationId.Value;

                if (common.myInt(hdnRegistrationId.Value) == 0)
                {

                    txtRegNo.Text = "";

                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Patient not found !";
                    return;
                }
            }
            if (common.myStr(Request.QueryString["Type"]) == "OP")
            {
                if (isPatientAdmitted())
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Patient is admitted !";
                    return;

                }
            }
            PatientInfo();
            populateControlsForEditing();
            BindBlankNewComponent();
            BindPatientProvisionalDiagnosis();
        }
        else
        {
            /*
            ClearFields();            
            Mode = "N";
            if (Convert.ToInt32(Request.QueryString["AppID"]) != 0)
            {
                PopulateUnRegisterPatient(Convert.ToInt32(Request.QueryString["AppID"]));
            }
            */

        }
    }

    protected void btnGetInfoForComponent_Click(object sender, EventArgs e)
    {
        DataTable tab = (DataTable)Session["GridData"];
        if (tab != null)
        {
            gvNewComponent.DataSource = tab;
            gvNewComponent.DataBind();
            GetIndicationTransfusionList();
            if (common.myInt(hdnIndication.Value) > 0)
                ddlIndicationforTransfusion.SelectedValue = hdnIndication.Value;
            else
                ddlIndicationforTransfusion.SelectedIndex = 0;


        }
    }

    protected void btnGetRequisitionInfo_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {


            if (ua1.CheckPermissions("E", Request.Url.AbsolutePath))
            {
                ua1.DisableEnableControl(btnSaveData, true);
            }
            else
            {
                ua1.DisableEnableControl(btnSaveData, false);
            }


            ds = objBb.GetComponentRequisition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRequisition.Value.Trim()) == 0 ? -1 : common.myInt(hdnRequisition.Value.Trim()), "", 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable tab = ds.Tables[0];
                BaseC.Patient objPatient = new BaseC.Patient(sConString);
                txtPatientName.Text = common.myStr(tab.Rows[0]["PatientName"]);
                if (tab.Rows[0]["RequestAcknowledged"].Equals(false))
                {
                    btnCancelRequisition.Visible = true;
                }
                else
                {
                    btnCancelRequisition.Visible = false;
                }
                //Added on 30-08-2014 Start  Naushad
                txtRequestDate.Text = tab.Rows[0]["RequestDate"].ToString();

                //Added on 30-08-204 End Naushad 

                if (common.myStr(tab.Rows[0]["GuardianName"]) == "" || common.myStr(tab.Rows[0]["GuardianName"]) == string.Empty)
                {
                    txtGuardian.Enabled = true;
                }
                else
                {
                    txtGuardian.Text = common.myStr(tab.Rows[0]["GuardianName"]);
                    txtGuardian.Enabled = false;
                }

                /*`
                if (Convert.ToString(tab.Rows[0]["Gender"]) != "")
                {
                    ddlSex.SelectedIndex = ddlSex.Items.IndexOf(ddlSex.Items.FindItemByValue(Convert.ToString(tab.Rows[0]["Gender"])));
                }                
                 */
                if (tab.Rows[0]["pregnancy"].ToString() != "" && tab.Rows[0]["miscarriage"].ToString() != "")
                {
                    if (common.myBool(tab.Rows[0]["pregnancy"]))
                        rbtnpregnancy.SelectedValue = "1";
                    else
                        rbtnpregnancy.SelectedValue = "0";
                    if (common.myBool(tab.Rows[0]["miscarriage"]))
                        rbtnMiscarriage.SelectedValue = "1";
                    else
                        rbtnMiscarriage.SelectedValue = "0";

                    //rbtnpregnancy.SelectedValue = common.myStr(tab.Rows[0]["pregnancy"].ToString());
                    //rbtnMiscarriage.SelectedValue = common.myStr(tab.Rows[0]["miscarriage"].ToString());
                    //pnlSearch.Visible = true;
                }
                else
                {
                    chbIsPediatric.Checked = false;
                    rbtnpregnancy.ClearSelection();
                    rbtnMiscarriage.ClearSelection();
                    //pnlSearch.Visible = true;
                }

                txtReqNo.Text = common.myStr(tab.Rows[0]["RequisitionNo"]);
                ddlSex.SelectedValue = common.myStr(tab.Rows[0]["Gender"]);
                ddlSex.SelectedIndex = ddlSex.Items.IndexOf(ddlSex.Items.FindItemByValue(Convert.ToString(tab.Rows[0]["Gender"])));

                //ddlSex.SelectedIndex = ddlSex.Items.IndexOf(ddlSex.Items.FindItemByValue(Convert.ToString(tab.Rows[0]["GenderToShow"])));
                dtpDateOfBirth.SelectedDate = Convert.ToDateTime(objPatient.FormatDate(tab.Rows[0]["DOB"].ToString().Trim(), Application["OutputDateFormat"].ToString(), "dd/MM/yyyy"));
                txtYear.Text = tab.Rows[0]["AgeYear"].ToString().Trim();
                txtMonth.Text = tab.Rows[0]["AgeMonth"].ToString().Trim();
                txtDays.Text = tab.Rows[0]["AgeDay"].ToString().Trim();
                hdnEncounterId.Value = common.myStr(tab.Rows[0]["EncounterId"].ToString().Trim());
                hdnEncounterNo.Value = common.myStr(tab.Rows[0]["EncounterNo"].ToString().Trim());
                hdnRegistrationId.Value = common.myStr(tab.Rows[0]["RegistrationId"].ToString().Trim());
                hdnRegistrationNo.Value = common.myStr(tab.Rows[0]["RegistrationNo"].ToString().Trim());
                //Added by rakesh for Order Creation on 6/10/2014 start
                hdnOrderId.Value = common.myStr(tab.Rows[0]["OrderID"].ToString().Trim());
                //Added by rakesh for Order Creation on 6/10/2014 start
                txtEncounterNo.Text = hdnEncounterNo.Value;
                txtRegNo.Text = hdnRegistrationNo.Value;
                //ddlRequestType.SelectedIndex = ddlRequestType.Items.IndexOf(ddlRequestType.Items.FindItemByValue(Convert.ToString(tab.Rows[0]["RequestType"])));                
                //ddlRequestType.SelectedItem.Text = common.myStr(tab.Rows[0]["RequestType"]);
                string requestType = common.myStr(tab.Rows[0]["RequestType"]).Trim();
                if (requestType.Equals("Routine"))
                {
                    ddlRequestType.SelectedValue = "R";
                }
                else if (requestType.Equals("Urgent"))
                {
                    ddlRequestType.SelectedValue = "U";
                }
                else if (requestType.Equals("Immediate"))
                {
                    ddlRequestType.SelectedValue = "I";
                }
                else if (requestType.Equals("Emergency"))
                {
                    ddlRequestType.SelectedValue = "E";
                    ddlRequestType_SelectedIndexChanged(null, null);
                    if (!(common.myInt(tab.Rows[0]["EmergencyType"]).Equals(0)))
                    {
                        dvEmergencyType.Style["display"] = "block";
                        ddlEmergencyType.SelectedValue = common.myStr(tab.Rows[0]["EmergencyType"]);
                    }
                    else
                    {
                        dvEmergencyType.Style["display"] = "none";
                        ddlEmergencyType.SelectedIndex = -1;
                    }
                }

                chkIsOuterRequest.Checked = common.myInt(tab.Rows[0]["IsOuterRequest"]) == 1 ? true : false;
                txtWeight.Text = common.myStr(tab.Rows[0]["Weight"]);
                //chbIsPediatric.Checked = common.myBool(tab.Rows[0]["IsPrediatric"]) ? true : false;
                chbIsPediatric.Checked = common.myBool(tab.Rows[0]["IsPrediatric"]);
                //txtDiagnosis.Text = common.myStr(tab.Rows[0]["Diagnosis"]);
                txtChargeSlipNo.Text = common.myStr(tab.Rows[0]["ChargeSlipNo"]);

                ddlPatientBloodGroup.SelectedIndex = ddlPatientBloodGroup.Items.IndexOf(ddlPatientBloodGroup.Items.FindItemByValue(Convert.ToString(tab.Rows[0]["PatientBloodGroupId"])));
                ddlMotherBloodGroup.SelectedIndex = ddlMotherBloodGroup.Items.IndexOf(ddlMotherBloodGroup.Items.FindItemByValue(Convert.ToString(tab.Rows[0]["MotherBloodGroupId"])));
                //chbIsExchangeTransformation.Checked = common.myBool(tab.Rows[0]["IsExchangeTransfusion"]) ? true : false;
                chbIsExchangeTransformation.Checked = common.myBool(tab.Rows[0]["IsExchangeTransfusion"]);
                txtHaematocritRequired.Text = common.myDbl(tab.Rows[0]["HaematocritRequired"]).ToString();
                string value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "BBGETCOMPONENTREQUISITIONVALUES", sConString);
                if (value == "Y")//for VKT HOSPITAL
                {
                    txtHB.Text = Convert.ToString(tab.Rows[0]["HB"]);
                    txtPltCount.Text = Convert.ToString(tab.Rows[0]["Platlet"]);
                    txtPT.Text = Convert.ToString(tab.Rows[0]["PT"]);
                    txtAPTT.Text = Convert.ToString(tab.Rows[0]["APTTT"]);
                    txtFIBRINOGEN.Text = Convert.ToString(tab.Rows[0]["FIBRINOGEN"]);
                }
                else
                {
                    txtHB.Text = Convert.ToString(tab.Rows[0]["HB"]);
                    txtPltCount.Text = Convert.ToString(tab.Rows[0]["Platlet"]);
                    txtPT.Text = Convert.ToString(tab.Rows[0]["PT"]);
                    txtAPTT.Text = Convert.ToString(tab.Rows[0]["APTTT"]);
                    txtFIBRINOGEN.Text = Convert.ToString(tab.Rows[0]["FIBRINOGEN"]);
                }
                ddlCounsultant.SelectedIndex = ddlCounsultant.FindItemByValue(Convert.ToString(tab.Rows[0]["ConsultantID"])).Index;
                //txtDiagnosis.Text = Convert.ToString(tab.Rows[0]["Diagnosis"]);
                if (tab.Rows[0]["Diagnosis"].Equals(string.Empty))
                {
                    txtDiagnosis.Text = Convert.ToString(tab.Rows[0]["DiagnosisName"]);
                }
                else
                {
                    txtDiagnosis.Text = Convert.ToString(tab.Rows[0]["Diagnosis"]);
                }

                if (common.myStr(tab.Rows[0]["Ward"]) != string.Empty || common.myStr(tab.Rows[0]["Ward"]) != "")
                {
                    txtWard.Text = Convert.ToString(tab.Rows[0]["Ward"]);
                    txtWard.Enabled = false;
                }
                else
                {
                    txtWard.Enabled = true;
                }

                txtBedNo.Text = Convert.ToString(tab.Rows[0]["BedNo"]);
                //ddlConsentTakenBy.SelectedItem.Text = Convert.ToString(tab.Rows[0]["ConsentTakenBy"]);// ddlCounsultant.FindItemByValue(Convert.ToString(tab.Rows[0]["ConsentTakenBy"])).Index;

                if (common.myInt(tab.Rows[0]["HospitalID"]) > 0)
                {
                    chkIsOuterRequest.Checked = true;
                    ddlHospitalList.Enabled = true;
                    ddlHospitalList.SelectedIndex = ddlHospitalList.Items.IndexOf(ddlHospitalList.Items.FindItemByValue(Convert.ToString(tab.Rows[0]["HospitalID"])));
                    txtDoctorName.Text = common.myStr(tab.Rows[0]["ExternalDoctorName"]);
                }
                if (common.myInt(tab.Rows[0]["OtherHospitalID"]) > 0)
                {
                    chkotherhospitalrequest.Checked = true;
                    ddlotherhospital.Enabled = true;
                    ddlotherhospital.SelectedIndex = ddlotherhospital.Items.IndexOf(ddlotherhospital.Items.FindItemByValue(Convert.ToString(tab.Rows[0]["OtherHospitalID"])));
                    txtotherDoctor.Text = common.myStr(tab.Rows[0]["OtherDOctorName"]);
                    txtothermrdno.Text = common.myStr(tab.Rows[0]["OtherMRDNO"]);
                    txtotherwardname.Text = common.myStr(tab.Rows[0]["OtherWARDNAME"]);
                    txtotherBedNo.Text = common.myStr(tab.Rows[0]["OtherBEDNO"]);
                }


                ViewState["_ID"] = common.myStr(hdnRequisition.Value);
                bindQuestionList();
                FillComponentRequisitionDetailsGrid();
                FillComponentRequestionQuestionMasterGrid();


                //Added by rakesh to bind Indication for transfusion, consent date, consent taken by and remarks  start
                hdnIndication.Value = ddlIndicationforTransfusion.SelectedValue = Convert.ToString(tab.Rows[0]["IndicationId"]);
                dtpConsentDate.SelectedDate = common.myDate(Convert.ToString(tab.Rows[0]["ConsentDate"]));
                //ddlConsentTakenBy.SelectedIndex = ddlConsentTakenBy.FindItemByValue(Convert.ToString(tab.Rows[0]["ConsentTakenBy"])).Index;

                if (Convert.ToString(tab.Rows[0]["ConsentsTakenBy"]) != "0")
                {

                    ddlConsentTakenBy.SelectedIndex = ddlConsentTakenBy.FindItemByValue(Convert.ToString(tab.Rows[0]["ConsentsTakenBy"])).Index;
                }
                txtRemarks.Text = Convert.ToString(tab.Rows[0]["Remarks"]);
                ddlReason.SelectedValue = Convert.ToString(tab.Rows[0]["ReasonId"]);
                hdnReqAck.Value = Convert.ToString(tab.Rows[0]["RequestAcknowledged"]);
                hdnSampleAck.Value = Convert.ToString(tab.Rows[0]["SampleAcknowledged"]);

                //if (tab.Rows[0]["RequestAcknowledged"].Equals(false))
                //{
                //    btnCancelRequisition.Visible = true;
                //}
                //else
                //{
                //    btnCancelRequisition.Visible = false;
                //}


                lblAcknowledge.Text = "";
                if (common.myInt(hdnReqAck.Value) > 0 || common.myStr(hdnReqAck.Value).ToUpper() == "TRUE")
                {
                    btnSaveData.Visible = false;
                    disableControl();
                }
                //Added by rakesh to bind Indication for transfusion, consent date, consent taken by and remarks start
                if (common.myInt(txtYear.Text) == 0 && common.myInt(txtMonth.Text) == 0 && common.myInt(txtDays.Text) == 0)
                {
                    btnCalAge_Click(null, null);
                }
                disableControl();
                BindPatientProvisionalDiagnosis();
                if (Convert.ToString(tab.Rows[0]["Active"]).Equals("False"))
                {
                    disablebuttons(false);
                }
                else
                {
                    disablebuttons(true);
                }
            }
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
            {
                btnSaveData.Visible = false;
                btnNew.Visible = false;


            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void disablebuttons(bool action)
    {
        if (action == false)
        {
            btnCancelRequisition.Visible = false;
        }
        btnSaveData.Visible = action;
        lnkAddComponent.Visible = action;
        btnprint.Visible = action;
    }
    private void disableControl()
    {
        if (hdnRequisition.Value != "")
        {
            //ddlRequestType.Enabled = false;
            txtReqNo.Enabled = false;
            txtEncounterNo.Enabled = false;
            txtPatientName.Enabled = false;
            txtWard.Enabled = false;
            txtRequestDate.Enabled = false;
            txtDiagnosis.Enabled = false;
            txtGuardian.Enabled = false;
            txtBedNo.Enabled = false;
            txtChargeSlipNo.Enabled = false;
            ddlSex.Enabled = false;
            dtpDateOfBirth.Enabled = false;
            //txtHB.Enabled = false;
            //ddlPatientBloodGroup.Enabled = false;
            txtDays.Enabled = false;
            txtMonth.Enabled = false;
            txtYear.Enabled = false;
            if (txtGuardian.Text != "")
                txtGuardian.Enabled = false;

            if (common.myBool(hdnReqAck.Value))
            {
                gvNewComponent.Enabled = false;
                gvPreviousHistory.Enabled = false;
                ddlMotherBloodGroup.Enabled = false;
                //lbtnAddComponent.Enabled = false;
                ddlPatientBloodGroup.Enabled = false;
                ddlIndicationforTransfusion.Enabled = false;
                dtpConsentDate.Enabled = false;
                ddlConsentTakenBy.Enabled = false;
            }


        }

    }

    private void EnableControl()
    {
        ddlRequestType.Enabled = true;
        txtReqNo.Enabled = true;
        txtEncounterNo.Enabled = true;
        txtPatientName.Enabled = true;
        txtWard.Enabled = true;
        txtRequestDate.Enabled = true;
        txtDiagnosis.Enabled = true;
        txtGuardian.Enabled = true;
        txtBedNo.Enabled = true;
        txtChargeSlipNo.Enabled = true;
        ddlSex.Enabled = true;
        dtpDateOfBirth.Enabled = true;
        //txtHB.Enabled = false;
        ddlPatientBloodGroup.Enabled = true;
        txtDays.Enabled = true;
        txtMonth.Enabled = true;
        txtYear.Enabled = true;
        txtDiagnosis.Text = string.Empty;
        ddlRequestType.SelectedValue = "0";
        gvNewComponent.Enabled = true;
        gvPreviousHistory.Enabled = true;
        ddlMotherBloodGroup.Enabled = true;
        // lbtnAddComponent.Enabled = true;
        clearControl();
    }

    private void FillComponentRequisitionDetailsGrid()
    {
        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {
            ds = objBb.GetComponentRequisitionDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRequisition.Value.Trim()), 0, 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("id", typeof(Int32));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["id"] = i;
                }
                //ds.Tables[0].Columns["id"].AutoIncrement = true;
                //  gvNewComponent.DataSource = null;

                gvNewComponent.DataSource = ds.Tables[0];
                gvNewComponent.DataBind();
                Session["GridData"] = ds.Tables[0];
                ViewState["GridData"] = ds.Tables[0];

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void FillComponentRequestionQuestionMasterGrid()
    {
        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {
            ds = objBb.GetComponentRequisitionQuestionDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRequisition.Value.Trim()), 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (gvPreviousHistory.Rows.Count > 0)
                {
                    for (int i = 0; i < gvPreviousHistory.Rows.Count; i++)
                    {
                        int QuestId = common.myInt(((HiddenField)gvPreviousHistory.Rows[i].FindControl("hdnQuestionId")).Value);
                        foreach (DataRow DR in ds.Tables[0].Rows)
                        {
                            if (common.myInt(DR["QuestionId"]) == QuestId)
                            {
                                ((RadioButtonList)gvPreviousHistory.Rows[i].FindControl("radAnswer")).SelectedIndex = 0;
                                break;
                            }
                        }
                    }
                }

            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


    }

    private void GetIndicationTransfusionList()
    {
        DataTable tab = (DataTable)Session["GridData"];
        if (tab != null)
        {
            strXMLForIndication = new StringBuilder();
            collForIndication = new ArrayList();
            foreach (DataRow row in tab.Rows)
            {
                collForIndication.Add(row["ComponentId"]);
                strXMLForIndication.Append(common.setXmlTable(ref collForIndication));
            }
            objBb = new BaseC.clsBb(sConString);
            DataSet ds = objBb.GetComponentRequisitionIndicationTransfusion(strXMLForIndication.ToString());
            ddlIndicationforTransfusion.Items.Clear();
            ddlIndicationforTransfusion.DataSource = ds.Tables[0].Copy();
            ddlIndicationforTransfusion.DataValueField = "IndicationId";
            ddlIndicationforTransfusion.DataTextField = "IndicationDescription";
            ddlIndicationforTransfusion.DataBind();
            ddlIndicationforTransfusion.Items.Insert(0, new RadComboBoxItem("", "0"));
        }
    }

    protected void gvNewComponent_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label TotalQty = (Label)e.Row.FindControl("lblQty");

                TotalQty.Text = common.myDbl(TotalQty.Text).ToString("F0");

                Label lblSizeML = (Label)e.Row.FindControl("lblSizeML");

                lblSizeML.Text = common.myDbl(lblSizeML.Text).ToString("F0");

                Label RequestDate = (Label)e.Row.FindControl("lblRequestDate");
                //DateTime dt = common.myDate(RequestDate.Text);
                //RequestDate.Text =RequestDate.Text.ToString(("dd/MM/yyyy");               
                //  RequestDate.Text = common.myStr(common.myDate(RequestDate.Text)); //dt.ToString("dd/MM/yyyy hh:mm:ss");
            }
        }
        catch
        {
        }
    }

    protected void gvNewComponent_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Select")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                HiddenField ComponentId = (HiddenField)row.FindControl("hdnComponentId");
                HiddenField id = (HiddenField)row.FindControl("hdnUniqueId");
                string uniqueId = id.Value.ToString();
                Label Qty = (Label)row.FindControl("lblQty");
                Label RequestDate = (Label)row.FindControl("lblRequestDate");
                Label SizeML = (Label)row.FindControl("lblSizeML");
                hdnUniqueIdOuter.Value = ((HiddenField)row.FindControl("hdnUniqueId")).Value;

                ddlComponent.SelectedIndex = ddlComponent.Items.IndexOf(ddlComponent.Items.FindItemByValue(ComponentId.Value));
                ddlComponent.Enabled = false;
                txtNoOfUnit.Text = common.myInt(Qty.Text.Trim()).ToString();
                //DateTime dtRequestDate =Convert.ToDateTime(RequestDate.Text).ToString("dd/MM/yyyy hh:mm:ss");

                // dtpRequestDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                string dtpReqDt = "";
                dtpReqDt = Convert.ToDateTime(common.myDate(RequestDate.Text)).ToString("dd/MM/yyyy");
                dtpRequestDate.MinDate = common.myDate(dtpReqDt);
                dtpRequestDate.SelectedDate = common.myDate(RequestDate.Text);
                txtSize.Text = common.myStr(SizeML.Text.Trim()).ToString();
                lblAcknowledge.Text = "";
                lnkAddComponent.Text = "Update Component";
                if (common.myInt(hdnReqAck.Value) > 0 || common.myStr(hdnReqAck.Value).ToUpper() == "TRUE")
                {
                    lblAcknowledge.Text = "Request Acknowledged...";
                    lblAcknowledge.ForeColor = System.Drawing.Color.Green;
                    lnkAddComponent.Enabled = false;
                }
                else
                {
                    ddlComponent.Enabled = true;
                }
            }




            //else if (e.CommandName == "ItemDelete")
            //{
            //    //int ItemId = common.myInt(e.CommandArgument);
            //    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            //    int uniqueId = common.myInt(((HiddenField)row.FindControl("hdnUniqueId")).Value);

            //    if (Session["GridData"] != null)
            //    {
            //        DataTable tbl = (DataTable)Session["GridData"];

            //        DataView DV = tbl.Copy().DefaultView;
            //        DV.RowFilter = "ISNULL(id, 0) <> " + uniqueId;
            //        tbl = DV.ToTable();
            //        Session["GridData"] = tbl;
            //        if (tbl.Rows.Count == 0)
            //        {
            //            /*
            //            DataRow DR = tbl.NewRow();
            //            tbl.Rows.Add(DR);
            //            tbl.AcceptChanges();
            //            */
            //        }
            //        gvNewComponent.DataSource = tbl;
            //        gvNewComponent.DataBind();
            //        GetIndicationTransfusionList();
            //        ddlIndicationforTransfusion.SelectedValue = hdnIndication.Value;
            //    }
            //}



            else if (e.CommandName == "ItemDelete")
            {
                //int ItemId = common.myInt(e.CommandArgument);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int uniqueId = common.myInt(((HiddenField)row.FindControl("hdnUniqueId")).Value);

                if (ViewState["GridData"] != null)
                {
                    DataTable tbl = (DataTable)ViewState["GridData"];

                    DataView DV = tbl.Copy().DefaultView;
                    DV.RowFilter = "ISNULL(id, 0) <> " + uniqueId;
                    tbl = DV.ToTable();
                    ViewState["GridData"] = tbl;
                    if (tbl.Rows.Count == 0)
                    {
                        DataRow DR = tbl.NewRow();
                        tbl.Rows.Add(DR);
                        tbl.AcceptChanges();

                    }
                    gvNewComponent.DataSource = tbl;
                    gvNewComponent.DataBind();

                    lnkAddComponent.Text = "Add Component";
                }
            }



        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        //clearControl();
    }


    public string GetRecordForInvestigation(int Encounterid, string FieldId, int HospitalLocationId, int FacilityId)
    {
        string result = string.Empty;
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hstInput = new Hashtable();
        hstInput.Add("@intEncounterid", Encounterid);
        hstInput.Add("@intFieldId", FieldId);
        hstInput.Add("@inyHospitalLocationId", HospitalLocationId);
        hstInput.Add("@intFacilityId", FacilityId);
        result = (string)objDl.ExecuteScalar(CommandType.StoredProcedure, "uspBBGetRecordForInvestigation", hstInput);
        return result;
    }
    private void populateControlsForEditing()
    {
        objBb = new BaseC.clsBb(sConString);
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        try
        {
            if (hdnRegistrationNo.Value != "")
            {

                //DataSet dset = objBb.GetComponentRequisitionPatientDetails(common.myInt(hdnRegistrationId.Value));
                //Added on 09-08-2014 Startr Naushad
                string Ptype = "";
                if (ViewState["OPIP"] != null)
                {
                    Ptype = common.myStr(ViewState["OPIP"]);
                }
                else
                {

                    Ptype = common.myStr(Session["OPIP"]);
                }
                //Added on 09-08-2014 Startr Naushad
                DataSet tab = objBb.GetComponentRequisitionPatientDetails(common.myLong(hdnRegistrationNo.Value), Ptype, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                if (tab.Tables[0].Rows.Count > 0)
                {
                    txtPatientName.Text = tab.Tables[0].Rows[0]["PatientName"].ToString();

                    //dtpDateOfBirth.SelectedDate =Convert.ToDateTime(tab.Rows[0]["DOB"].ToString());
                    dtpDateOfBirth.SelectedDate = Convert.ToDateTime(objPatient.FormatDate(tab.Tables[0].Rows[0]["DateofBirth"].ToString().Trim(), Application["OutputDateFormat"].ToString(), "dd/MM/yyyy"));
                    txtYear.Text = tab.Tables[0].Rows[0]["AgeYear"].ToString().Trim();
                    txtMonth.Text = tab.Tables[0].Rows[0]["AgeMonth"].ToString().Trim();
                    txtDays.Text = tab.Tables[0].Rows[0]["AgeDays"].ToString().Trim();
                    if (Session["ModuleId"] != null && Session["ModuleId"].ToString() != "3")
                    {
                        hdnEncounterId.Value = common.myStr(tab.Tables[0].Rows[0]["EncounterId"].ToString().Trim());
                        hdnEncounterNo.Value = common.myStr(tab.Tables[0].Rows[0]["EncounterNo"].ToString().Trim());
                    }
                    txtEncounterNo.Text = hdnEncounterNo.Value;
                    txtRegNo.Text = hdnRegistrationNo.Value;
                    if (txtDiagnosis.Text.Trim().Equals(string.Empty))
                    {
                        txtDiagnosis.Text = common.myStr(tab.Tables[0].Rows[0]["DiagnosisName"]);
                    }
                    //txtDiagnosis.Enabled = false;

                    txtWard.Text = common.myStr(tab.Tables[0].Rows[0]["WardName"]);
                    txtBedNo.Text = common.myStr(tab.Tables[0].Rows[0]["BedNo"]);
                    txtGuardian.Text = common.myStr(tab.Tables[0].Rows[0]["kinFName"]);

                    //Added by rakesh start
                    if (!(common.myStr(tab.Tables[0].Rows[0]["ConsultingDoctorId"]).Equals(string.Empty)))
                        ddlCounsultant.SelectedValue = common.myStr(tab.Tables[0].Rows[0]["ConsultingDoctorId"]);
                    //Added by rakesh end


                    if (Convert.ToString(tab.Tables[0].Rows[0]["Gender"]) != "")
                    {
                        ddlSex.SelectedIndex = ddlSex.Items.IndexOf(ddlSex.Items.FindItemByValue(Convert.ToString(tab.Tables[0].Rows[0]["Gender"])));
                    }
                    if (common.myInt(txtYear.Text) == 0 && common.myInt(txtMonth.Text) == 0 && common.myInt(txtDays.Text) == 0)
                    {
                        btnCalAge_Click(null, null);
                    }

                    //Added on 14-08-2014 Start Naushad

                    ViewState["EncoutnerNo"] = common.myStr(tab.Tables[0].Rows[0]["EncounterId"]);

                    string value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "BBGETCOMPONENTREQUISITIONVALUES", sConString);
                    if (value == "Y")//for VKT HOSPITAL
                    {
                        //HB PLATELET APTT PT VALUES SAVE AND GET FROM BBCOMPONENTREQUISITION TABLE NOT FOR LAB.
                        //txtHB.Text = "";
                        //txtPltCount.Text = "";
                        //txtPT.Text = "";
                        //txtAPTT.Text = "";
                    }
                    else
                    {
                        txtHB.Text = GetRecordForInvestigation(common.myInt(tab.Tables[0].Rows[0]["EncounterId"]), "Hb", common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                        txtPltCount.Text = GetRecordForInvestigation(common.myInt(tab.Tables[0].Rows[0]["EncounterId"]), "PC", common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                        txtPT.Text = GetRecordForInvestigation(common.myInt(tab.Tables[0].Rows[0]["EncounterId"]), "PT", common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                        txtAPTT.Text = GetRecordForInvestigation(common.myInt(tab.Tables[0].Rows[0]["EncounterId"]), "APTT", common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                        txtFIBRINOGEN.Text = GetRecordForInvestigation(common.myInt(tab.Tables[0].Rows[0]["EncounterId"]), "FIB", common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                    }

                    //Added on 14-08-2014  End
                }
                if (tab.Tables.Count > 1)
                {
                    if (tab.Tables[1].Rows.Count > 0)
                    {
                        txtIssueDate.Text = tab.Tables[1].Rows[0]["IssueDate"].ToString();
                        txtBloodGroup.Text = tab.Tables[1].Rows[0]["BloodGroup"].ToString();
                        txtBloodComponent.Text = tab.Tables[1].Rows[0]["ComponentName"].ToString();
                        ChbAnyTransfusion.Checked = common.myBool(tab.Tables[1].Rows[0]["AnyTransfusion"]);
                        ChbAnyTransfusionReaction.Checked = common.myBool(tab.Tables[1].Rows[0]["AnyTransfusionReaction"]);
                        ChbIrradiatedComponent.Checked = common.myBool(tab.Tables[1].Rows[0]["IrradiatedComponent"]);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);

        }
        finally
        {
            objBb = null;
            objPatient = null;
        }
    }



    protected void lbtnAddComponent_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "AddComponent.aspx?RequisitionId=" + common.myInt(hdnRequisition.Value);
        RadWindowForNew.Height = 500;
        RadWindowForNew.Width = 600;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "FillComponentGridOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;

    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("GridData");
    }
    protected void gvPreviousHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadioButtonList buttonList = (RadioButtonList)e.Row.FindControl("radAnswer");
            buttonList.SelectedIndex = ((HiddenField)e.Row.FindControl("hdnIsDefault")).Value == "True" ? 0 : 1;
        }
    }
    protected void ddlSex_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        bindQuestionList();
    }
    protected void chbIsExchangeTransformation_CheckedChanged(object sender, EventArgs e)
    {
        if (chbIsExchangeTransformation.Checked)
        {
            RequiredFieldValidator fieldvalidator = new RequiredFieldValidator();
            fieldvalidator.ErrorMessage = "Haematocrit have not entered value for Haematocrit";
            fieldvalidator.ControlToValidate = "txtHaematocritRequired";
            fieldvalidator.ValidationGroup = "Save";
            fieldvalidator.Display = ValidatorDisplay.None;
            fieldvalidator.SetFocusOnError = true;

            RangeValidator rangeValidator = new RangeValidator();
            rangeValidator.ErrorMessage = "Haematocrit must between 0-100";
            rangeValidator.ControlToValidate = "txtHaematocritRequired";
            rangeValidator.ValidationGroup = "Save";
            rangeValidator.MinimumValue = "0";
            rangeValidator.MaximumValue = "100";
            rangeValidator.Display = ValidatorDisplay.None;
            rangeValidator.Type = ValidationDataType.Integer;
            rangeValidator.SetFocusOnError = true;

        }

    }
    protected void lbtnSearchRequisition_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["EncId"] != null)
        {
            hdnEncounterId.Value = Convert.ToString(Request.QueryString["EncId"]).Trim();
            hdnEncounterNo.Value = Convert.ToString(Request.QueryString["EncNo"]).Trim();
            hdnRegistrationId.Value = Convert.ToString(Request.QueryString["Regid"]).Trim();
            hdnRegistrationNo.Value = Convert.ToString(Request.QueryString["RegNo"]).Trim();

            RadWindowForNew.NavigateUrl = "ComponentRequisitionList.aspx?MP=NO&Regid=" + common.myInt(hdnRegistrationId.Value)
                                  + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                                   + "&EncId=" + common.myInt(hdnEncounterId.Value)
                                   + "&EncNo=" + common.myStr(hdnEncounterNo.Value);
        }
        //Added on 11-08-2014 Start Naushad
        else if (Session["EncounterId"] != null)
        {

            hdnEncounterId.Value = Session["EncounterId"].ToString();
            hdnEncounterNo.Value = Session["EncounterNo"].ToString();
            hdnRegistrationId.Value = Session["RegistrationId"].ToString();
            hdnRegistrationNo.Value = Session["RegistrationNo"].ToString();
            RadWindowForNew.NavigateUrl = "ComponentRequisitionList.aspx?MP=NO&Regid=" + common.myInt(hdnRegistrationId.Value)
                               + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                                + "&EncId=" + common.myInt(hdnEncounterId.Value)
                                + "&EncNo=" + common.myStr(hdnEncounterNo.Value);

        }

        //Added on 11-08-2014 End Naushad



        else
            RadWindowForNew.NavigateUrl = "ComponentRequisitionList.aspx";

        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 950;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "FillDetailsForComponentRequisitionOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    private void SetPermission()
    {
        ua1.DisableEnableControl(btnSaveData, false);
        ua1.DisableEnableControl(btnNew, false);

        if (ua1.CheckPermissions("N", Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnSaveData, true);
            ua1.DisableEnableControl(btnNew, true);
        }
        if (ua1.CheckPermissions("E", Request.Url.AbsolutePath))
        {
            // ua1.DisableEnableControl(btnSaveData, true);
            //ua1.DisableEnableControl(btnNew, true);
            // ua1.DisableEnableControl(btnclose, true);
            //ua1.DisableEnableControl(btnPost, true);
        }
        if (ua1.CheckPermissions("C", Request.Url.AbsolutePath))
        {
            //ua1.DisableEnableControl(btnCancel, true);
        }
        if (ua1.CheckPermissions("P", Request.Url.AbsolutePath))
        {
            //ua1.DisableEnableControl(bt, true);
            //ua1.DisableEnableControl(btnPrintPreview, true);
            //ua1.DisableEnableControl(btnPrint, true);
        }
        ua1.Dispose();
        clearControl();
    }
    /*  protected String SaveComponentRequisition(int RequisitionId, string RequisitionNo, int HospitalLocationId, int FacilityId, string RequestType, int IsOuterRequest,
            int RegistrationId, string RegistrationNo, int EncounterId, string EncounterNo, DateTime RequestDate, string PatientName, string GuardianName, int Sex, DateTime DateofBirth,
            int AgeYear, int AgeMonth, int AgeDays, double Weight, int IsPrediatric, string ChargeSlipNo, int PatientBloodGroupId, int MotherBloodGroupId, int ReasonId,
           int IsExchangeTransfusion, double HaematocritRequired, int IndicationId, DateTime ConsentDate, int ConsentTakenBy, string Remarks, int Active, int UserId, string ComponentRequisitionDetailsXml, string ComponentRequisitionQuestionDetailsXml, float HB, float Platlet, float PT, float APTTT, float FIBRINOGEN, int ConsultantID, string Diagnosis, string Ward, int BedNo)
       {
           Hashtable HshIn;
           Hashtable HshOut;
           HshIn = new Hashtable();
           HshOut = new Hashtable();

           HshIn.Add("@intRequisitionId", RequisitionId);
           HshIn.Add("@chvRequisitionNo", RequisitionNo);
           HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
           HshIn.Add("@intFacilityId", FacilityId);
           HshIn.Add("@chvRequestType", RequestType);
           HshIn.Add("@bitIsOuterRequest", IsOuterRequest);
           HshIn.Add("@intRegistrationId", RegistrationId);
           HshIn.Add("@chvRegistrationNo", RegistrationNo);
           HshIn.Add("@intEncounterId", EncounterId);
           HshIn.Add("@chvEncounterNo", EncounterNo);
           HshIn.Add("@dtRequestDate", RequestDate);
           HshIn.Add("@chvPatientName", PatientName);
           HshIn.Add("@chvGuardianName", GuardianName);
           HshIn.Add("@inySex", Sex);
           HshIn.Add("@dtDateofBirth", DateofBirth);
           HshIn.Add("@inyAgeYear", AgeYear);
           HshIn.Add("@inyAgeMonth", AgeMonth);
           HshIn.Add("@inyAgeDays", AgeDays);
           HshIn.Add("@monWeight", Weight);
           HshIn.Add("@bitIsPrediatric", IsPrediatric);
           HshIn.Add("@chvChargeSlipNo", ChargeSlipNo);
           HshIn.Add("@intPatientBloodGroupId", PatientBloodGroupId);
           HshIn.Add("@intMotherBloodGroupId", MotherBloodGroupId);
           HshIn.Add("@intReasonId", ReasonId);
           HshIn.Add("@bitIsExchangeTransfusion", IsExchangeTransfusion);
           HshIn.Add("@decHaematocritRequired", HaematocritRequired);
           HshIn.Add("@intIndicationId", IndicationId);
           HshIn.Add("@dtConsentDate", ConsentDate);
           HshIn.Add("@intConsentTakenBy", ConsentTakenBy);
           HshIn.Add("@chvRemarks", Remarks);
           HshIn.Add("@bitActive", Active);
           HshIn.Add("@intEncodedBy", UserId);
           HshIn.Add("@xmlComponentRequisitionDetails", ComponentRequisitionDetailsXml);
           HshIn.Add("@xmlRequisitionQuestionDetails", ComponentRequisitionQuestionDetailsXml);

           HshIn.Add("@flHB", HB);
           HshIn.Add("@flPlatlets", Platlet);
           HshIn.Add("@flPT", PT);
           HshIn.Add("@flAPTTT", APTTT);
           HshIn.Add("@flFIBRINOGEN", FIBRINOGEN);
           HshIn.Add("@intConsultantID", ConsultantID);
           HshIn.Add("@chvDiagnosis", Diagnosis);
           HshIn.Add("@chvWard", Ward);
           HshIn.Add("@intBedNo", BedNo);



           HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

           DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
           HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentRequisition", HshIn, HshOut);
           return HshOut["@chvErrorStatus"].ToString();

       }*/
    protected void chbIsPediatric_CheckedChanged(object sender, EventArgs e)
    {
        if (chbIsPediatric.Checked)
        {
            pnlpediatric.Visible = true;
        }
        else
            pnlpediatric.Visible = false;

    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (txtReqNo.Text == "" || txtReqNo.Text == string.Empty)
        {
            Alert.ShowAjaxMsg("Select patient first", Page);
            return;
        }
        string Name = "ComponentRequisition";
        RadWindowForNew.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?ReportName=" + Name + "&RegNo=" + common.myStr(txtRegNo.Text) + "&RequisId=" + common.myInt(hdnRequisition.Value);
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1000;
        RadWindowForNew.Top = 40;
        //RadWindowForNew.Left = 100;
        //RadWindowForNew.OnClientClose = "FillDetailsForComponentRequisitionOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.Behavior = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected int SaveOrder()
    {
        int orderID;
        orderID = 0;
        try
        {

            //Added by rakesh for order creation on 1/10/2014 start
            StringBuilder strXMLServiceID = new StringBuilder();
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            bool IsBillingRequired = false;
            foreach (GridViewRow row in gvNewComponent.Rows)
            {
                HiddenField hdnIsCrossMatchRequired = (HiddenField)row.FindControl("hdnIsCrossMatchRequired");
                HiddenField hdnIsBillingRequired = (HiddenField)row.FindControl("hdnIsBillingRequired");
                IsBillingRequired = ((hdnIsCrossMatchRequired != null && common.myBool(hdnIsCrossMatchRequired.Value))
                    //&& (hdnIsBillingRequired != null && common.myBool(hdnIsBillingRequired.Value))
                    ) ? true : false;
                if (IsBillingRequired)
                {
                    //string ServiceId = (row.FindControl("hdnCrossMatchServiceId") as HiddenField).Value.ToString();// 
                    string ServiceId = ((HiddenField)row.FindControl("hdnCrossMatchServiceId")).Value.ToString();
                    string Qty = ((Label)row.FindControl("lblQty")).Text.ToString();
                    string OrderId = "";
                    //if (OrderId == "0")
                    //{
                    string ICDID = "1";
                    string DoctorId = common.myStr(ddlCounsultant.SelectedValue);
                    string FacilityId = common.myStr(Session["FacilityId"]);
                    string Remarks = "";
                    bool Stat = false; //common.myBool(row.Cells[8].Text);
                    //HiddenField hdnExcludedServices = (HiddenField)row.FindControl("hdnExcludedServices");
                    //HiddenField hdnPackageId = (HiddenField)row.FindControl("hdnPackageId");

                    //HiddenField hdnRequestToDepartment = (HiddenField)row.FindControl("hdnRequestToDepartment");
                    //if (hdnRequestToDepartment.Value != "1")
                    //{
                    strXMLServiceID.Append("<Table1><c1>");
                    strXMLServiceID.Append(common.myInt(ServiceId));
                    strXMLServiceID.Append("</c1><c2>");
                    strXMLServiceID.Append("</c2><c3>");
                    strXMLServiceID.Append(common.myInt(Qty));
                    strXMLServiceID.Append("</c3><c4>");
                    //strXMLServiceID.Append(DoctorId == "&nbsp;" ? ddlProvider.SelectedValue : DoctorId);
                    strXMLServiceID.Append(DoctorId);
                    strXMLServiceID.Append("</c4><c5>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c5><c6>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c6><c7>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c7><c8>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c8><c9>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c9><c10>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c10><c11>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c11><c12>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c12><c13>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c13><c14>");
                    strXMLServiceID.Append(OrderId == "&nbsp;" ? "0" : OrderId);
                    strXMLServiceID.Append("</c14><c15>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c15><c16>");
                    strXMLServiceID.Append(ICDID == "&nbsp;" ? null : ICDID);
                    strXMLServiceID.Append("</c16><c17>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c17><c18>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c18><c19>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c19><c20>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c20><c21>");
                    strXMLServiceID.Append(Remarks == "&nbsp;" ? "" : Remarks);
                    strXMLServiceID.Append("</c21><c22>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c22><c23>");
                    strXMLServiceID.Append(0);
                    strXMLServiceID.Append("</c23><c24>");
                    strXMLServiceID.Append(0);
                    strXMLServiceID.Append("</c24><c25>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c25><c26>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c26><c27>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c27><c28>");
                    strXMLServiceID.Append(FacilityId);
                    strXMLServiceID.Append("</c28><c29>");
                    strXMLServiceID.Append(Stat);
                    strXMLServiceID.Append("</c29><c30>");
                    strXMLServiceID.Append(DBNull.Value);
                    strXMLServiceID.Append("</c30></Table1>");
                    //}
                    //}
                }
            }
            //Added by rakesh for order creation on 1/10/2014 end
            ArrayList coll = new ArrayList();
            StringBuilder strXMLAleart = new StringBuilder();
            DataSet ds;
            if (strXML.ToString() != "")
            {
                string sChargeCalculationRequired = "Y";
                string stype = string.Empty;
                string opip = string.Empty;

                int registrationId; Int64 registrationNo;
                int encounterId;
                if (Session["RegistrationId"] != null)
                {
                    registrationId = common.myInt(Session["RegistrationId"]);
                    registrationNo = common.myLong(Session["RegistrationNo"]);
                }
                else
                {
                    registrationId = common.myInt(hdnRegistrationId.Value);
                    registrationNo = common.myLong(hdnRegistrationNo.Value);
                }

                if (Session["EncounterId"] == null && hdnEncounterId.Value.Equals(string.Empty))
                {
                    BaseC.clsBb objclsBb = new BaseC.clsBb(sConString);
                    Hashtable hshOutObj = new Hashtable();
                    hshOutObj = objclsBb.CreateEncounterIFNotExistsForBloodBank(Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(Session["FacilityId"].ToString()),
                                     registrationNo, registrationId, Convert.ToInt32(Session["UserID"].ToString()), common.myInt(ddlCounsultant.SelectedValue));
                    if (hshOutObj["chvErrorStatus"].ToString().Length == 0)
                    {
                        hdnEncounterId.Value = hshOutObj["intEncounterID"].ToString();
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "There is some error while creating encounter";
                    }
                }
                if (Session["EncounterId"] != null)
                {
                    ds = order.GetEncounterCompany(common.myInt(Session["EncounterId"]));
                }
                else
                {
                    ds = order.GetEncounterCompany(common.myInt(hdnEncounterId.Value));
                }
                //string sChargeCalculationRequired = "Y";
                stype = "P" + ds.Tables[0].Rows[0]["opip"].ToString().Trim();
                opip = ds.Tables[0].Rows[0]["opip"].ToString().Trim();

                int CompanyId = 0, InsuranceId = 0, CardId = 0;
                if (ds.Tables[0].Rows[0]["CompanyCode"].ToString().Trim() != "")
                {
                    CompanyId = common.myInt(ds.Tables[0].Rows[0]["CompanyCode"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim() != "")
                {
                    InsuranceId = common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["CardId"].ToString().Trim() != "")
                {
                    CardId = common.myInt(ds.Tables[0].Rows[0]["CardId"].ToString().Trim());
                }
                int RequestId = common.myInt(Request.QueryString["RequestId"]);
                Hashtable hshOut = new Hashtable();
                if (opip == "E")
                {
                    opip = "O";

                }



                if (Session["EncounterId"] != null)
                {
                    encounterId = common.myInt(Session["EncounterId"]);
                }
                else
                {
                    encounterId = common.myInt(hdnEncounterId.Value);
                }

                hshOut = order.saveOrders(Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(Session["FacilityId"].ToString()),
                                    registrationId, encounterId, strXMLServiceID.ToString(),
                                    strXMLAleart.ToString(), string.Empty, Convert.ToInt32(Session["UserID"].ToString()),
                                    common.myInt(ddlCounsultant.SelectedValue), CompanyId, stype, common.myStr("E"), common.myStr(opip), InsuranceId,
                                    CardId, Convert.ToDateTime(DateTime.Now), sChargeCalculationRequired, common.myBool("0"), 1,
                                    RequestId, common.myStr(ViewState["vsTemplateDataDetails"]), common.myInt(Session["EntrySite"]));

                if (hshOut["chvErrorStatus"].ToString().Length == 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    ViewState["OrderId"] = hshOut["intOrderId"];
                    orderID = common.myInt(hshOut["intOrderId"]);
                    lblMessage.Text = "Order No:" + hshOut["intOrderNo"] + " Saved Successfully";
                    lblMessage.Font.Bold = true;

                    Session["TemplateDataDetails"] = null;

                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "There is some error while saving order." + hshOut["chvErrorStatus"].ToString();
                }
                //hdnIsUnSavedData.Value = "0";
                //bindBlankSelectedServices();

                //BindPatientAlert();
            }
            else
            {
                Alert.ShowAjaxMsg("No Service selected", Page);
                //return;
            }

            //ClearForm();

            //if (btnUpdate.Text == "Update")
            //{
            //    btnUpdate.Text = "Save";
            //}
            ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);

        }
        return orderID;
    }

    protected void BindPatientProvisionalDiagnosis()
    {
        try
        {
            DataSet ds;
            BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);

            ds = new DataSet();
            if (!hdnEncounterId.Value.Equals("0"))
            {
                ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value), common.myInt(Session["UserID"]), 0);
            }
            else
            {
                ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["UserID"]), 0);
            }
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtProvisionalDiagnosis.Text = common.clearHTMLTags(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]);
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    public string CancelComponentRequisition(int RequisitionId, int HospitalLocationId, int FacilityId, int UserId)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        HshIn.Add("@intRequisitionId", RequisitionId);
        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@intEncodedBy", UserId);
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //result = Convert.ToInt32((string)objDl.ExecuteScalar(CommandType.StoredProcedure, "uspBBCancelComponentRequisition", hstInput)); //
        //return HshOut["@chvErrorStatus"].ToString();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBCancelComponentRequisition", HshIn, HshOut);
        return HshOut["@chvErrorStatus"].ToString();
    }
    protected void btnCancelRequisition_Click(object sender, EventArgs e)
    {
        //string strMsg = objBb.CancelComponentRequisition(common.myInt(ViewState["_ID"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserID"].ToString()));
        string strMsg = CancelComponentRequisition(common.myInt(ViewState["_ID"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserID"].ToString()));
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        //clearControl();
        ViewState["_ID"] = 0;
        lblMessage.Text = strMsg;
        //string abc = string.Empty;
    }
    private void bindEmergencyBloodRequisitionType()
    {
        DataSet ds = new DataSet();
        try
        {
            objBb = new BaseC.clsBb(sConString);
            ds = objBb.GetEmergencyBloodRequisitionType(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1);
            ddlEmergencyType.DataSource = ds.Tables[0];
            ddlEmergencyType.DataValueField = "BloodRequisitionTypeId";
            ddlEmergencyType.DataTextField = "BloodRequisitionTypeDesc";
            ddlEmergencyType.DataBind();
            //ddlEmergencyType.Items.Insert(0, new RadComboBoxItem("", "0"));

        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            ds.Dispose();
        }
    }
    protected void ddlRequestType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlRequestType.SelectedValue.Equals("E"))
        {
            dvEmergencyType.Style["display"] = "block";
            bindEmergencyBloodRequisitionType();
        }
        else
        {
            dvEmergencyType.Style["display"] = "none";
            ddlEmergencyType.DataSource = null;
            ddlEmergencyType.DataBind();
            //hide this div.
        }
    }
    private void bindHospitalList()
    {
        DataSet ds = new DataSet();
        try
        {
            objBb = new BaseC.clsBb(sConString);
            ds = objBb.GetHospitalList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1);
            ddlHospitalList.DataSource = ds.Tables[0];
            ddlHospitalList.DataValueField = "HospitalID";
            ddlHospitalList.DataTextField = "HospitalName";
            ddlHospitalList.DataBind();
            ddlHospitalList.Items.Insert(0, new RadComboBoxItem("", "0"));

            ddlotherhospital.DataSource = ds.Tables[0];
            ddlotherhospital.DataValueField = "HospitalID";
            ddlotherhospital.DataTextField = "HospitalName";
            ddlotherhospital.DataBind();
            ddlotherhospital.Items.Insert(0, new RadComboBoxItem("", "0"));

        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            ds.Dispose();
        }
    }
    protected void chkIsOuterRequest_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsOuterRequest.Checked)
        {
            bindHospitalList();
            ddlHospitalList.Enabled = true;
            txtDoctorName.Enabled = true;
            //LinkButton22.Visible = false;
        }
        else
        {
            ddlHospitalList.SelectedIndex = 0;
            ddlHospitalList.Enabled = false;
            txtDoctorName.Enabled = false;
            txtDoctorName.Text = string.Empty;

            //LinkButton22.Visible = true;
        }

    }


    protected void chkotherhospitalrequest_CheckedChanged(object sender, EventArgs e)
    {
        if (chkotherhospitalrequest.Checked)
        {
            bindHospitalList();
            ddlotherhospital.Enabled = true;
            //txtDoctorName.Enabled = true;
            //LinkButton22.Visible = false;
        }
        else
        {
            ddlotherhospital.SelectedIndex = 0;
            ddlotherhospital.Enabled = false;
            //txtDoctorName.Enabled = false;
            //txtDoctorName.Text = string.Empty;

            //LinkButton22.Visible = true;
        }
    }



    protected void btn_otherHospital_Click(object sender, EventArgs e)
    {

        //RadWindowForNew.NavigateUrl = "AddComponent.aspx?RequisitionId=" + common.myInt(hdnRequisition.Value);
        RadWindowForNew.NavigateUrl = "/BloodBank/SetupMaster/HospitalMaster.aspx?MP=NO";
        RadWindowForNew.Height = 500;
        RadWindowForNew.Width = 600;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        //RadWindowForNew.OnClientClose = "FillComponentGridOnClientClose";

        RadWindowForNew.OnClientClose = "OTHERHOSPITALCLOSE";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;


    }

    protected void btnGetOTHERHOSPITAL_Click(object sender, EventArgs e)
    {
        bindHospitalList();
    }

    #region Transaction password validation
    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        RadWindow1.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP";
        RadWindow1.Height = 120;
        RadWindow1.Width = 340;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (common.myInt(hdnIsValidPassword.Value).Equals(0))
            {
                lblMessage.Text = "Invalid Username/Password!";
                return;
            }

            saveRecords();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion

}
