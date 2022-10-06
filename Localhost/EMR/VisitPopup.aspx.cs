using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMRBILLING_VisitPopup : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        BaseC.EMRBilling objBill = new BaseC.EMRBilling(sConString);
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (Session["UserID"] == null || Session["HospitalLocationID"] == null || Session["FacilityId"] == null)
            {
                //if (Session["LoginDoctorId"] == null || Session["HospitalLocationID"] == null || Session["FacilityId"] == null)
                //{
                Response.Redirect("~/Login.aspx?Logout=1", false);
                return;
            }
            if (common.myInt(Cache["DecimalPlace"]) == 0)
            {
                string DecimalPlaces = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DecimalPlaces", sConString);
                Cache["DecimalPlace"] = common.myStr(common.myInt(DecimalPlaces));
                hdnDecimalPlaces.Value = common.myStr(Cache["DecimalPlace"]);
            }
            else
            {
                hdnDecimalPlaces.Value = common.myStr(Cache["DecimalPlace"]);

            }

            if (!IsPostBack)
            {
                ViewState["IsSetConsultingDoctorInEMRIPVisit"] = "N";

                if (common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("YES"))
                {
                    ibtnClose.Visible = false;

                    ViewState["IsSetConsultingDoctorInEMRIPVisit"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                           common.myInt(Session["FacilityId"]), "IsSetConsultingDoctorInEMRIPVisit", sConString);
                }

                if (common.myStr(Session["OPIP"]).Equals("O"))
                {
                    gvIpVisit.Enabled = false;
                    cmbSpecial.Enabled = false;
                    cmbDoctor.Enabled = false;
                    ibtnSave.Enabled = false;
                    cmbOpVisit.Enabled = false;
                    btnAddDoctor.Enabled = false;
                    gVConsultation.Enabled = false;
                    txtRegNo.Enabled = false;
                    txtRegID.Enabled = false;
                    txtEncNo.Enabled = false;
                    txtEncId.Enabled = false;
                    ibtnClose.Enabled = false;
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "IP Visit option is not enable for OP Patients";
                    return;
                }
                else if (common.myStr(Session["OPIP"]).Equals("E"))
                {
                    gvIpVisit.Enabled = false;
                    cmbSpecial.Enabled = false;
                    cmbDoctor.Enabled = false;
                    ibtnSave.Enabled = false;
                    cmbOpVisit.Enabled = false;
                    btnAddDoctor.Enabled = false;
                    gVConsultation.Enabled = false;
                    txtRegNo.Enabled = false;
                    txtRegID.Enabled = false;
                    txtEncNo.Enabled = false;
                    txtEncId.Enabled = false;
                    ibtnClose.Enabled = false;
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "IP Visit option is not enable for Emergency Patients";
                    return;
                }
                else if (!common.myBool(Session["IsLoginDoctor"]))
                {
                    gvIpVisit.Enabled = false;
                    cmbSpecial.Enabled = false;
                    cmbDoctor.Enabled = false;
                    ibtnSave.Enabled = false;
                    cmbOpVisit.Enabled = false;
                    btnAddDoctor.Enabled = false;
                    gVConsultation.Enabled = false;
                    txtRegNo.Enabled = false;
                    txtRegID.Enabled = false;
                    txtEncNo.Enabled = false;
                    txtEncId.Enabled = false;
                    ibtnClose.Enabled = false;
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "IP Visit option is enable for Doctors only";
                    return;
                }
             


                if (common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("YES"))
                {
                    ibtnClose.Visible = false;
                }


                #region set view state values

                //ViewState["RegistrationId"] = common.myInt(Request.QueryString["RegId"]);
                //ViewState["RegistrationNo"] = common.myInt(Request.QueryString["RegNo"]);
                //ViewState["EncounterId"] = common.myInt(Request.QueryString["EncId"]);
                //ViewState["EncounterNo"] = common.myStr(Request.QueryString["EncNo"]);
                //ViewState["OPIP"] = common.myStr(Request.QueryString["OP_IP"]);
                //ViewState["CompanyId"] = common.myInt(Request.QueryString["CompanyId"]);
                //ViewState["PayerType"] = common.myStr(Request.QueryString["PayerType"]);
                //ViewState["InsuranceId"] = common.myInt(Request.QueryString["InsuranceId"]);
                //ViewState["CardId"] = common.myInt(Request.QueryString["CardId"]);
                //ViewState["CDocId"] = common.myInt(Request.QueryString["CDocId"]);


                ViewState["RegistrationId"] = common.myInt(Session["RegistrationID"]);
                ViewState["RegistrationNo"] = common.myLong(Session["RegistrationNo"]);
                ViewState["EncounterId"] = common.myInt(Session["Encounterid"]);
                ViewState["EncounterNo"] = common.myStr(Session["EncounterNo"]);
                ViewState["OPIP"] = common.myStr(Session["OPIP"]);
                ViewState["PayerType"] = common.myStr(Request.QueryString["PayerType"]);

                if (common.myStr(ViewState["IsSetConsultingDoctorInEMRIPVisit"]).Equals("Y"))
                {
                    ViewState["CDocId"] = common.myInt(Session["DoctorID"]);
                }
                else
                {
                    ViewState["CDocId"] = common.myInt(Session["LoginDoctorId"]);
                }

                DataSet dsEncounterDetails = new DataSet();
                ds = order.GetEncounterCompany(common.myInt(Session["EncounterId"]));

                if (common.myStr(ds.Tables[0].Rows[0]["CompanyCode"].ToString().Trim()) != string.Empty)
                {
                    ViewState["CompanyId"] = common.myInt(Request.QueryString["CompanyId"]);
                }
                if (common.myStr(ds.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim()) != string.Empty)
                {
                    ViewState["InsuranceId"] = common.myInt(Request.QueryString["InsuranceId"]);
                }
                if (common.myStr(ds.Tables[0].Rows[0]["CardId"].ToString().Trim()) != string.Empty)
                {
                    ViewState["CardId"] = common.myInt(Request.QueryString["CardId"]);
                }

                //if (common.myInt(ViewState["RegistrationId"]).Equals(0))
                //{
                //    ViewState["RegistrationId"] = common.myInt(Session["RegistrationId"]);
                //}
                //if (common.myInt(ViewState["RegistrationNo"]).Equals(0))
                //{
                //    ViewState["RegistrationNo"] = common.myInt(Session["RegistrationNo"]);
                //}
                //if (common.myInt(ViewState["EncounterId"]).Equals(0))
                //{
                //    ViewState["EncounterId"] = common.myInt(Session["EncounterId"]);
                //}
                //if (common.myStr(ViewState["EncounterNo"]).Equals(string.Empty))
                //{
                //    ViewState["EncounterNo"] = common.myStr(Session["EncounterNo"]);
                //}
                //if (common.myStr(ViewState["OPIP"]).Equals(string.Empty))
                //{
                //    ViewState["OPIP"] = common.myStr(Session["OPIP"]);
                //}

                #endregion

                Session["IPVisitCheck"] = 0;
                ViewState["GridData"] = null;
                clearAll();
                txtRegID.Text = common.myStr(ViewState["RegistrationId"]);
                txtRegNo.Text = common.myStr(ViewState["RegistrationNo"]);
                txtEncId.Text = common.myStr(ViewState["EncounterId"]);
                txtEncNo.Text = common.myStr(ViewState["EncounterNo"]);
                ViewState["IPNumber"] = txtEncId.Text.Trim();

                ds = objBill.GetPatientLastVisit(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(txtRegID.Text.Trim()));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblByDoctor.Text = common.myStr(ds.Tables[0].Rows[0]["DoctorName"]);
                        lblVisitDate.Text = common.myStr(ds.Tables[0].Rows[0]["OrderDate"]);
                        lblService.Text = common.myStr(ds.Tables[0].Rows[0]["ServiceName"]);
                    }
                    else
                    {
                        lblByService.Text = "";
                        lblvisit.Text = "";
                        lblbyDoc.Text = "";
                        lblByDoctor.Text = "";
                        lblVisitDate.Text = "";
                        lblService.Text = "";
                    }
                }
                else
                {
                    lblByService.Text = "";
                    lblvisit.Text = "";
                    lblbyDoc.Text = "";
                    lblByDoctor.Text = "";
                    lblVisitDate.Text = "";
                    lblService.Text = "";
                }
                ds.Clear();
                if (common.myStr(ViewState["OPIP"]).Equals("O"))
                {
                    updIpVisit.Visible = false;
                    updOpVisit.Visible = true;
                    ibtnSave.Visible = true;
                    ibtnSave.Text = "Proceed";
                    ibtnSave.ToolTip = "Click to proceed to OP Bill Page...";
                    trVisitDetail.Visible = true;
                }
                else
                {
                    trVisitDetail.Visible = false;
                    updIpVisit.Visible = true;
                    updOpVisit.Visible = false;
                    ibtnSave.Visible = true;
                }
                ViewState["Doctor"] = null;
                ViewState["GridData"] = null;
                BindPatientHiddenDetails(common.myInt(txtRegNo.Text));
                fillSpecialization();
                cmbSpecial.Enabled = true;

                fillDoctor();
                bindDefaultConsultationGrid();
                cmbDoctor_OnSelectedIndexChanged(sender, e);

                if (common.myBool(Session["IsLoginDoctor"]))
                {
                    cmbSpecial.Enabled = false;
                    cmbDoctor.Enabled = false;
                }

                if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                {
                    cmbSpecial.Enabled = true;
                    cmbDoctor.Enabled = true;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objBill = null; ds.Dispose(); }
    }

    protected void cmbSpecial_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fillDoctor();
            fillVisitGrid();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void cmbDoctor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fillVisitGrid();
            Session["IPVisitCheck"] = 0;
            cmbOpVisit.Enabled = true;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void ibtSave_OnClick(object sender, EventArgs e)
    {
        Hashtable hshInput = new Hashtable();
        Hashtable hshOutput = new Hashtable();
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        BaseC.Patient objBp = new BaseC.Patient(sConString);
        BaseC.clsEMRBilling OBJclsEMRBilling = new BaseC.clsEMRBilling(sConString);

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataSet dsCheckSentForBilling = new DataSet();
        DataTable dtNew = new DataTable();

        StringBuilder strVisitXML = new StringBuilder();
        StringBuilder str32 = new StringBuilder();
        StringBuilder str33 = new StringBuilder();
        StringBuilder str34 = new StringBuilder();
        StringBuilder str35 = new StringBuilder();
        ArrayList coll = new ArrayList();

        try
        {
            dsCheckSentForBilling = OBJclsEMRBilling.CheckSentForBilling(common.myInt(ViewState["IPNumber"]));

            if (dsCheckSentForBilling.Tables.Count > 0)
            {
                if (dsCheckSentForBilling.Tables[0].Rows.Count > 0)
                {
                    //   ViewState["EncounterStatus"] = common.myStr(dsCheckSentForBilling.Tables[0].Rows[0]["Code"]);
                    //if (common.myStr(dsCheckSentForBilling.Tables[0].Rows[0]["Code"]).Equals("SB")) //O,MD,PC (alloed)
                    //{
                    if (
                        !common.myStr(dsCheckSentForBilling.Tables[0].Rows[0]["Code"]).Equals("O")
                       && !common.myStr(dsCheckSentForBilling.Tables[0].Rows[0]["Code"]).Equals("MD")
                         && !common.myStr(dsCheckSentForBilling.Tables[0].Rows[0]["Code"]).Equals("PC")) //O,MD,PC (alloed)
                    {
                        Alert.ShowAjaxMsg("Encouter is sent for billing. Not able to save !!!", Page);
                        return;
                    }
                }
            }

            //if (common.myStr(ViewState["EncounterStatus"]).Equals("SB"))
            //{
            //    Alert.ShowAjaxMsg("Encouter is sent for billing not able to save !!!", Page);
            //    return;
            //}
            if (common.myStr(cmbDoctor.SelectedValue) != "0" && common.myStr(cmbDoctor.SelectedValue) != "")
            {
                int sno = 0;
                #region IPVisit Order
                if (common.myStr(ViewState["OPIP"]).Equals("I") && !common.myStr(ViewState["IPNumber"]).Equals(string.Empty))
                {
                    if (common.myInt(Session["IPVisitCheck"]) == 0)
                    {
                        foreach (GridTableRow item in gvIpVisit.Items)
                        {
                            CheckBox chk1 = (CheckBox)item.FindControl("chk1");
                            if ((chk1.Checked) && (chk1.Enabled == true))
                            {
                                coll.Add(hdnServiceId1.Value); //ServiceId INT,
                                coll.Add(common.myDate(item.Cells[3].Text).ToString("yyyy-MM-dd HH:mm:00")); //VisitDate SMALLDATETIME,  
                                coll.Add(1); //Units TINYINT,
                                coll.Add(common.myInt(cmbDoctor.SelectedValue)); //DoctorId INT, 
                                coll.Add(DBNull.Value); //ServiceAmount MONEY,
                                coll.Add(DBNull.Value); //DoctorAmount MONEY,  
                                coll.Add(DBNull.Value); //ServiceDiscountAmount MONEY, 
                                coll.Add(DBNull.Value); //DoctorDiscountAmount MONEY,
                                coll.Add(DBNull.Value); //AmountPayableByPatient MONEY,
                                coll.Add(DBNull.Value); //AmountPayableByPayer MONEY,
                                coll.Add(DBNull.Value); //ServiceDiscountPer MONEY,
                                coll.Add(DBNull.Value); //DoctorDiscountPer MONEY,
                                coll.Add(DBNull.Value); //PackageId INT,  
                                coll.Add(0); //UnderPackage BIT,
                                coll.Add(DBNull.Value); //OrderId INT,
                                coll.Add(DBNull.Value); //ICDID VARCHAR(100), 
                                coll.Add(DBNull.Value); //ResourceID INT,
                                coll.Add(DBNull.Value); //SurgeryAmount MONEY, 
                                coll.Add(DBNull.Value); //ProviderPercent MONEY,
                                coll.Add(++sno); //SeQNo INT, 
                                coll.Add(DBNull.Value); //Serviceremarks Varchar(150) 
                                str32.Append(common.setXmlTable(ref coll));
                            }
                            CheckBox chk2 = (CheckBox)item.FindControl("chk2");
                            if ((chk2.Checked) && (chk2.Enabled == true))
                            {
                                coll.Add(hdnServiceId2.Value); //ServiceId INT,
                                coll.Add(common.myDate(item.Cells[3].Text).ToString("yyyy-MM-dd HH:mm:00")); //VisitDate SMALLDATETIME,  
                                coll.Add(1); //Units TINYINT,
                                coll.Add(common.myInt(cmbDoctor.SelectedValue)); //DoctorId INT, 
                                coll.Add(DBNull.Value); //ServiceAmount MONEY,
                                coll.Add(DBNull.Value); //DoctorAmount MONEY,  
                                coll.Add(DBNull.Value); //ServiceDiscountAmount MONEY, 
                                coll.Add(DBNull.Value); //DoctorDiscountAmount MONEY,
                                coll.Add(DBNull.Value); //AmountPayableByPatient MONEY,
                                coll.Add(DBNull.Value); //AmountPayableByPayer MONEY,
                                coll.Add(DBNull.Value); //ServiceDiscountPer MONEY,
                                coll.Add(DBNull.Value); //DoctorDiscountPer MONEY,
                                coll.Add(DBNull.Value); //PackageId INT,  
                                coll.Add(0); //UnderPackage BIT,
                                coll.Add(DBNull.Value); //OrderId INT,
                                coll.Add(DBNull.Value); //ICDID VARCHAR(100), 
                                coll.Add(DBNull.Value); //ResourceID INT,
                                coll.Add(DBNull.Value); //SurgeryAmount MONEY, 
                                coll.Add(DBNull.Value); //ProviderPercent MONEY,
                                coll.Add(++sno); //SeQNo INT, 
                                coll.Add(DBNull.Value); //Serviceremarks Varchar(150) 
                                str33.Append(common.setXmlTable(ref coll));
                            }
                            CheckBox chk3 = (CheckBox)item.FindControl("chk3");
                            if ((chk3.Checked) && (chk3.Enabled == true))
                            {
                                coll.Add(hdnServiceId3.Value); //ServiceId INT,
                                coll.Add(common.myDate(item.Cells[3].Text).ToString("yyyy-MM-dd HH:mm:00")); //VisitDate SMALLDATETIME,  
                                coll.Add(1); //Units TINYINT,
                                coll.Add(common.myInt(cmbDoctor.SelectedValue)); //DoctorId INT, 
                                coll.Add(DBNull.Value); //ServiceAmount MONEY,
                                coll.Add(DBNull.Value); //DoctorAmount MONEY,  
                                coll.Add(DBNull.Value); //ServiceDiscountAmount MONEY, 
                                coll.Add(DBNull.Value); //DoctorDiscountAmount MONEY,
                                coll.Add(DBNull.Value); //AmountPayableByPatient MONEY,
                                coll.Add(DBNull.Value); //AmountPayableByPayer MONEY,
                                coll.Add(DBNull.Value); //ServiceDiscountPer MONEY,
                                coll.Add(DBNull.Value); //DoctorDiscountPer MONEY,
                                coll.Add(DBNull.Value); //PackageId INT,  
                                coll.Add(0); //UnderPackage BIT,
                                coll.Add(DBNull.Value); //OrderId INT,
                                coll.Add(DBNull.Value); //ICDID VARCHAR(100), 
                                coll.Add(DBNull.Value); //ResourceID INT,
                                coll.Add(DBNull.Value); //SurgeryAmount MONEY, 
                                coll.Add(DBNull.Value); //ProviderPercent MONEY,
                                coll.Add(++sno); //SeQNo INT, 
                                coll.Add(DBNull.Value); //Serviceremarks Varchar(150)               
                                str34.Append(common.setXmlTable(ref coll));
                            }
                            CheckBox chk4 = (CheckBox)item.FindControl("chk4");
                            if ((chk4.Checked) && (chk4.Enabled == true))
                            {
                                coll.Add(hdnServiceId4.Value); //ServiceId INT,
                                coll.Add(common.myDate(item.Cells[3].Text).ToString("yyyy-MM-dd HH:mm:00")); //VisitDate SMALLDATETIME,  
                                coll.Add(1); //Units TINYINT,
                                coll.Add(common.myInt(cmbDoctor.SelectedValue)); //DoctorId INT, 
                                coll.Add(DBNull.Value); //ServiceAmount MONEY,
                                coll.Add(DBNull.Value); //DoctorAmount MONEY,  
                                coll.Add(DBNull.Value); //ServiceDiscountAmount MONEY, 
                                coll.Add(DBNull.Value); //DoctorDiscountAmount MONEY,
                                coll.Add(DBNull.Value); //AmountPayableByPatient MONEY,
                                coll.Add(DBNull.Value); //AmountPayableByPayer MONEY,
                                coll.Add(DBNull.Value); //ServiceDiscountPer MONEY,
                                coll.Add(DBNull.Value); //DoctorDiscountPer MONEY,
                                coll.Add(DBNull.Value); //PackageId INT,  
                                coll.Add(0); //UnderPackage BIT,
                                coll.Add(DBNull.Value); //OrderId INT,
                                coll.Add(DBNull.Value); //ICDID VARCHAR(100), 
                                coll.Add(DBNull.Value); //ResourceID INT,
                                coll.Add(DBNull.Value); //SurgeryAmount MONEY, 
                                coll.Add(DBNull.Value); //ProviderPercent MONEY,
                                coll.Add(++sno); //SeQNo INT, 
                                coll.Add(DBNull.Value); //Serviceremarks Varchar(150)               
                                str35.Append(common.setXmlTable(ref coll));
                            }
                        }
                        str32.Append(common.myStr(str33));
                        str32.Append(common.myStr(str34));
                        str32.Append(common.myStr(str35));
                        strVisitXML = str32;

                        if (strVisitXML.ToString().Trim().Length > 1)
                        {
                            string sChargeCalculationRequired = "N";
                            string stype = "V" + common.myStr(ViewState["OPIP"]).ToUpper();
                            Hashtable hshOut = BaseBill.saveOrders(
                                                 common.myInt(Session["HospitalLocationID"]),
                                                 common.myInt(Session["FacilityId"]),
                                                 common.myInt(txtRegID.Text),
                                                 common.myInt(txtEncId.Text),
                                                 strVisitXML.ToString(), "",
                                                  common.myInt(Session["UserID"]),
                                                 //common.myInt(Session["LoginDoctorId"]),
                                                 common.myInt(cmbDoctor.SelectedValue),
                                                 common.myInt(ViewState["CompanyId"]),
                                                 stype,
                                                 common.myStr(ViewState["PayerType"]),
                                                 common.myStr(ViewState["OPIP"]),
                                                 common.myInt(ViewState["InsuranceId"]),
                                                 common.myInt(ViewState["CardId"]),
                                                 common.myDate(DateTime.Now), sChargeCalculationRequired,
                                                 common.myInt(Session["EntrySite"]),common.myInt(Session["ModuleId"]));

                            if (common.myStr(hshOut["chvErrorStatus"]).Length == 0)
                            {
                                Session["IPVisitCheck"] = 1;
                                fillVisitGrid();
                                lblMsg.ForeColor = System.Drawing.Color.Green;
                                lblMsg.Text = "Order Saved Successfully";
                            }
                            else
                            {
                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                lblMsg.Text = "There is some error while saving order." + hshOut["chvErrorStatus"].ToString();
                            }
                        }
                    }
                    else if (common.myInt(Session["IPVisitCheck"]) == 1)
                    {
                        fillVisitGrid();
                        lblMsg.ForeColor = System.Drawing.Color.Green;
                        lblMsg.Text = "Order Saved Successfully";
                    }
                }
                #endregion
                #region OP Visit Order
                else if (common.myStr(ViewState["OPIP"]).Equals("O"))
                {
                    dt = new DataTable();
                    ds = new DataSet();
                    dt = (DataTable)ViewState["GridData"];
                    if (Convert.ToBoolean(hdnExternalReg.Value) == true)
                    {
                        dtNew = getRegistrationService(true);
                        foreach (DataRow dts in dtNew.Rows)
                        {
                            DataRow dr;
                            dr = dt.NewRow();
                            dr["SNo"] = dt.Rows.Count + 1;
                            dr["ServiceId"] = dts["ServiceId"];
                            dr["UnderPackage"] = dts["UnderPackage"];
                            dr["PackageId"] = dts["PackageId"];
                            dr["DoctorID"] = dts["DoctorID"];
                            dr["DoctorRequired"] = common.myStr(dts["DoctorRequired"]);
                            dr["DepartmentId"] = dts["DepartmentId"];
                            dr["ServiceType"] = dts["ServiceType"];
                            dr["ServiceName"] = dts["ServiceName"];
                            dr["Units"] = dts["Units"];
                            dr["ServiceAmount"] = common.myDbl(dts["ServiceAmount"]);
                            dr["DoctorAmount"] = 0;
                            if (common.myDbl(dts["ServiceAmount"]).ToString() != "")
                                dr["Charge"] = common.myDbl(dts["ServiceAmount"]);
                            else
                                dr["Charge"] = 0;
                            dr["ServiceDiscountPercentage"] = common.myDbl(dts["ServiceDiscountPercentage"]);
                            dr["ServiceDiscountAmount"] = common.myDbl(dts["ServiceDiscountAmount"]);
                            dr["AmountPayableByPatient"] = common.myDbl(dts["AmountPayableByPatient"]);
                            dr["AmountPayableByPayer"] = common.myDbl(dts["AmountPayableByPayer"]);
                            dr["IsPackageMain"] = dts["IsPackageMain"];
                            dr["IsPackageService"] = dts["IsPackageService"];
                            dr["MainSurgeryId"] = dts["MainSurgeryId"];
                            dr["IsSurgeryMain"] = dts["IsSurgeryMain"];
                            dr["IsSurgeryService"] = dts["IsSurgeryService"];
                            dr["IsApprovalReq"] = dts["IsApprovalReq"];
                            dr["ChargePercentage"] = "0";
                            dr["CopayAmt"] = dts["CopayAmt"];
                            dr["CopayPerc"] = dts["CopayPerc"];
                            dr["IsCoPayOnNet"] = dts["IsCoPayOnNet"];
                            dr["DeductableAmount"] = dts["DeductableAmount"];
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt != null)
                    {
                        if (dt.Rows.Count == 0)
                        {
                            Alert.ShowAjaxMsg("Please select the provider", Page);
                            return;
                        }
                        ds.Tables.Add(dt.Copy());
                        System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
                        System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.

                        ds.WriteXml(writer);
                        //put schema in string
                        string xmlSchema = writer.ToString();
                        hdnXmlString.Value = xmlSchema;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Please add visit type", Page);
                        return;
                    }
                }
                #endregion
            }
            else
            {
                Alert.ShowAjaxMsg("Please select the provider!!!", Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            hshInput = null;
            hshOutput = null;
            BaseBill = null;
            objBp = null;

            dt.Dispose();
            ds.Dispose();
            dtNew.Dispose();

            strVisitXML = null;
            str32 = null;
            str33 = null;
            str34 = null;
            str35 = null;
            coll = null;
        }
    }

    protected void ibtnNew_OnClick(object sender, EventArgs e)
    {
        clearAll();
    }

    protected void btnAddDoctor_Click(object sender, EventArgs e)
    {
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        DataView dvdup = new DataView();
        Hashtable hshServiceDetail = new Hashtable();
        try
        {
            if (common.myStr(cmbDoctor.SelectedValue) != "0" && common.myStr(cmbDoctor.SelectedValue) != "")
            {
                if (common.myStr(cmbOpVisit.SelectedValue) == "")
                {
                    Alert.ShowAjaxMsg("Service does not exist, Please select from list", Page);
                    return;
                }
                if (ViewState["GridData"] == null)
                {
                    CreateTable();
                }

                int maxId = 0;
                dt = (DataTable)ViewState["GridData"];

                if (dt.Rows.Count > 0)
                {
                    if (common.myInt(dt.Rows[0][2]) == 0) //If serviceid = 0 then remove row
                    {
                        dt.Rows.Clear();
                    }
                    else
                    {
                        dv = new DataView(dt);
                        dv.Sort = "Sno Desc";
                        maxId = common.myInt(dv[0]["Sno"]);
                    }

                    //Check for multiple consultation service (single counsultation can bill at once)
                    if (dt.Rows.Count > 0)
                    {
                        Alert.ShowAjaxMsg("Consultation visit already added !", Page);
                        return;
                    }

                    //Check duplicate service------------------------------------------------------------------
                    dvdup = new DataView();
                    dvdup = dt.Copy().DefaultView;

                    dvdup.RowFilter = " ServiceId = " + common.myStr(cmbOpVisit.SelectedValue);

                    if (dvdup.ToTable().Rows.Count > 0)
                    {

                        Alert.ShowAjaxMsg("Service already exist !", Page);
                        return;
                    }


                }
                //----------------------------------------------------------------------------------------------
                hshServiceDetail = new Hashtable();

                int iBedCategorId = 0;
                int iSpecialisationId = 0;
                int iProviderId = 0;
                string sModifierCode = "";
                int iServiceId = common.myInt(cmbOpVisit.SelectedValue);

                if (common.myInt(cmbSpecial.SelectedValue) > 0)
                {
                    iSpecialisationId = common.myInt(cmbSpecial.SelectedValue);
                }
                if (common.myInt(cmbDoctor.SelectedValue) > 0)
                {
                    iProviderId = common.myInt(cmbDoctor.SelectedValue);
                }

                hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(ViewState["CompanyId"]), common.myInt(ViewState["InsuranceId"]), common.myInt(ViewState["CardId"]),
                                        common.myStr(ViewState["OPIP"]), common.myInt(iServiceId),
                                        common.myInt(txtRegID.Text), common.myInt(txtEncId.Text), iProviderId, 
                                        common.myInt(cmbSpecial.SelectedValue), 0, string.Empty);

                DataRow dr = dt.NewRow();
                dr["SNo"] = maxId + 1;
                dr["ServiceId"] = common.myInt(cmbOpVisit.SelectedValue);
                dr["UnderPackage"] = common.myInt(0);
                dr["PackageId"] = common.myInt(0);
                dr["DoctorID"] = common.myInt(iProviderId);
                dr["DoctorName"] = common.myStr(cmbDoctor.SelectedItem.Text);
                dr["DoctorRequired"] = common.myStr("True");
                dr["DepartmentId"] = common.myInt(hshServiceDetail["DepartmentId"]);
                dr["ServiceType"] = common.myStr(hshServiceDetail["ServiceType"]);
                dr["ServiceName"] = common.myStr(cmbOpVisit.Text);
                dr["Units"] = common.myDec(1);
                dr["Charge"] = common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value)); ;
                dr["ServiceAmount"] = common.myDec(common.myDec(hshServiceDetail["NChr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value)); ;
                dr["DoctorAmount"] = common.myDec(common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value)); ;
                dr["ServiceDiscountPercentage"] = common.myDec(hshServiceDetail["DiscountPerc"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value)); ;
                dr["ServiceDiscountAmount"] = common.myDec(hshServiceDetail["DiscountNAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["DoctorDiscountPercentage"] = common.myDec(0);
                dr["DoctorDiscountAmount"] = common.myDec(hshServiceDetail["DiscountDNAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["TotalDiscount"] = common.myDec(common.myDec(hshServiceDetail["DiscountNAmt"]) + common.myDec(hshServiceDetail["DiscountDNAmt"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["ResourceId"] = " ";
                dr["AmountPayableByPatient"] = common.myDec(hshServiceDetail["PatientNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["AmountPayableByPayer"] = common.myDec(hshServiceDetail["PayorNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["IsPackageMain"] = common.myInt(0);
                dr["IsPackageService"] = common.myInt(0);
                dr["MainSurgeryId"] = common.myInt(0);
                dr["IsSurgeryMain"] = common.myInt(0);
                dr["IsSurgeryService"] = common.myInt(0);
                dr["ServiceRemarks"] = " ";
                dr["ServiceStatus"] = "  ";
                dr["IsExcluded"] = "  ";
                dr["IsApprovalReq"] = common.myBool(hshServiceDetail["IsApprovalReq"]);
                dr["ChargePercentage"] = "0";
                dr["CopayAmt"] = common.myDbl(hshServiceDetail["insCoPayAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["CopayPerc"] = common.myDbl(hshServiceDetail["insCoPayPerc"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["IsCoPayOnNet"] = common.myInt(hshServiceDetail["IsCoPayOnNet"]);
                dr["DeductableAmount"] = common.myDbl(hshServiceDetail["mnDeductibleAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dt.Rows.Add(dr);
                ViewState["GridData"] = dt;
                gVConsultation.DataSource = dt;
                gVConsultation.DataBind();
            }
            else
            {
                Alert.ShowAjaxMsg("Please select the provider", Page);
                return;
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            BaseBill = null;
            dt.Dispose();
            dv.Dispose();
            dvdup.Dispose();
            hshServiceDetail = null;
        }
    }

    #region  PageFunctions
    /// <summary>
    /// Clear All Controls on Load
    /// </summary>
    protected void clearAll()
    {
        gvIpVisit.DataSource = null;
        gvIpVisit.DataBind();

        gVConsultation.DataSource = null;
        gVConsultation.DataBind();

        cmbDoctor.Items.Clear();
        cmbDoctor.Text = "";
        lblMsg.Text = "";
    }
    /// <summary>
    /// Fill Doctors Specialization Details
    /// </summary>
    protected void fillSpecialization()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorSpecialisation");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmbSpecial.DataSource = ds;
                    cmbSpecial.DataTextField = "Specialisation";
                    cmbSpecial.DataValueField = "SpecialisationId";
                    cmbSpecial.DataBind();
                    cmbSpecial.Items.Insert(0, new RadComboBoxItem("All", "0"));
                    cmbSpecial.Items[0].Value = "0";
                    if (common.myInt(hdhSpecialzationId.Value) > 0)
                    {
                        if (common.myStr(ViewState["IsSetConsultingDoctorInEMRIPVisit"]).Equals("Y"))
                        {
                            cmbSpecial.SelectedValue = common.myStr(hdhSpecialzationId.Value);
                        }
                        else
                        {
                            cmbSpecial.SelectedValue = common.myStr(Session["UserSpecialisationId"]);
                        }
                    }
                    else
                    {
                        cmbSpecial.SelectedIndex = 0;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objDl = null;
        }
    }
    /// <summary>
    /// Fill Doctor to Combo on basis of Specialization
    /// </summary>
    protected void fillDoctor()
    {
        BaseC.clsLISMaster lis = new BaseC.clsLISMaster(sConString);
        DataSet ds = new DataSet();
        DataTable dtTmp = new DataTable();
        DataColumn dC;
        DataRow dR;

        try
        {
            cmbDoctor.Items.Clear();
            cmbDoctor.Text = "";

            dC = new DataColumn("DoctorId", typeof(Int32));
            dtTmp.Columns.Add(dC);
            dC = new DataColumn("DoctorName", typeof(String));
            dtTmp.Columns.Add(dC);

            if (ViewState["Doctor"] == null)
            {
                ds = lis.getDoctorList(0, "", common.myInt(Session["HospitalLocationId"]), 0, common.myInt(Session["FacilityId"]), 0, 0);
            }
            else
            {
                ds = (DataSet)ViewState["Doctor"];
            }

            ViewState["Doctor"] = ds;
            if ((common.myStr(cmbSpecial.SelectedValue) != "0") && (common.myStr(cmbSpecial.SelectedValue) != ""))
            {
                foreach (DataRow drt in ds.Tables[0].Select("SpecialisationId = " + common.myStr(cmbSpecial.SelectedValue) + ""))
                {
                    dR = dtTmp.NewRow();
                    dR["DoctorId"] = drt["DoctorId"];
                    dR["DoctorName"] = drt["DoctorName"];
                    dtTmp.Rows.Add(dR);
                }
            }
            else
            {
                foreach (DataRow drt in ds.Tables[0].Select())
                {
                    dR = dtTmp.NewRow();
                    dR["DoctorId"] = drt["DoctorId"];
                    dR["DoctorName"] = drt["DoctorName"];
                    dtTmp.Rows.Add(dR);
                }
            }

            cmbDoctor.DataSource = dtTmp;
            cmbDoctor.DataTextField = "DoctorName";
            cmbDoctor.DataValueField = "DoctorId";
            cmbDoctor.DataBind();
            cmbDoctor.Enabled = true;

            cmbDoctor.Items.Insert(0, new RadComboBoxItem("", "0"));
            if (common.myStr(ViewState["OPIP"]).Equals("I"))
            {
                int CunsultingDoctorId = 0;
                if (common.myInt(ViewState["CDocId"]) > 0)
                {
                    CunsultingDoctorId = common.myInt(ViewState["CDocId"]);
                }
                else
                {
                    CunsultingDoctorId = common.myInt(hdnConsultantId.Value);
                }
                cmbDoctor.SelectedIndex = cmbDoctor.Items.IndexOf(cmbDoctor.Items.FindItemByValue(common.myStr(CunsultingDoctorId)));
                //cmbDoctor.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            lis = null;
            ds.Dispose();
            dtTmp.Dispose();
        }
    }
    /// <summary>
    /// Fill Grid on basis of OP or IP Saved Visit Data as per Encounter
    /// </summary>
    protected void fillVisitGrid()
    {
        ViewState["1Visit"] = string.Empty;
        ViewState["2Visit"] = string.Empty;
        ViewState["3Visit"] = string.Empty;
        ViewState["4Visit"] = string.Empty;
        Hashtable hshInput = new Hashtable();
        Hashtable hshOutput = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.clsEMRBilling objClsBilling = new BaseC.clsEMRBilling(sConString);
        DataSet ds = new DataSet();

        try
        {
            if (common.myStr(ViewState["OPIP"]).Equals("I"))// IP
            {
                hshInput.Add("iHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
                hshInput.Add("iFacilityId", common.myInt(Session["FacilityId"]));
                hshInput.Add("iRegistrationId", common.myInt(hdnRegistrationId.Value));
                hshInput.Add("iEncounterId", common.myStr(ViewState["IPNumber"]));
                hshInput.Add("iDoctorId", common.myInt(cmbDoctor.SelectedValue));
                hshInput.Add("iCompanyId", common.myInt(hdnCompanyCode.Value));
                hshInput.Add("iInsuranceId", common.myInt(hdnInsCode.Value));

                DataSet ds1 = objDl.FillDataSet(CommandType.StoredProcedure, "UspIPVisit", hshInput);
                if (ds1.Tables.Count > 1)
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        gvIpVisit.DataSource = ds1.Tables[0];
                        if (ds1.Tables[0].Columns.Count >= 2)
                        {
                            if (ds1.Tables[0].Columns.Count > 5)
                            {
                                hdnMorningRs.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceRate"]);
                                hdnEveningRs.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceRate"]);
                                hdnEmergencyRs.Value = common.myStr(ds1.Tables[1].Rows[2]["ServiceRate"]);
                                HdnEmergencyRs2.Value = common.myStr(ds1.Tables[1].Rows[3]["ServiceRate"]);
                                hdnServiceId1.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceId"]);
                                hdnServiceId2.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceId"]);
                                hdnServiceId3.Value = common.myStr(ds1.Tables[1].Rows[2]["ServiceId"]);
                                hdnServiceId4.Value = common.myStr(ds1.Tables[1].Rows[3]["ServiceId"]);

                                ViewState["1Visit"] = ds1.Tables[0].Columns[2].ToString();
                                ViewState["2Visit"] = ds1.Tables[0].Columns[3].ToString();
                                ViewState["3Visit"] = ds1.Tables[0].Columns[4].ToString();
                                ViewState["4Visit"] = ds1.Tables[0].Columns[5].ToString();

                            }
                            else if (ds1.Tables[0].Columns.Count == 5)
                            {
                                hdnMorningRs.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceRate"]);
                                hdnEveningRs.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceRate"]);
                                hdnEmergencyRs.Value = common.myStr(ds1.Tables[1].Rows[2]["ServiceRate"]);
                                hdnServiceId1.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceId"]);
                                hdnServiceId2.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceId"]);
                                hdnServiceId3.Value = common.myStr(ds1.Tables[1].Rows[2]["ServiceId"]);
                                ViewState["1Visit"] = ds1.Tables[0].Columns[2].ToString();
                                ViewState["2Visit"] = ds1.Tables[0].Columns[3].ToString();
                                ViewState["3Visit"] = ds1.Tables[0].Columns[4].ToString();
                                gvIpVisit.Columns.FindByUniqueName("col4").Visible = false;
                            }
                            else if (ds1.Tables[0].Columns.Count == 4)
                            {
                                hdnMorningRs.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceRate"]);
                                hdnEveningRs.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceRate"]);
                                hdnServiceId1.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceId"]);
                                hdnServiceId2.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceId"]);
                                ViewState["1Visit"] = ds1.Tables[0].Columns[2].ToString();
                                ViewState["2Visit"] = ds1.Tables[0].Columns[3].ToString();
                                gvIpVisit.Columns.FindByUniqueName("col3").Visible = false;
                                gvIpVisit.Columns.FindByUniqueName("col4").Visible = false;
                            }
                            else if (ds1.Tables[0].Columns.Count == 3)
                            {
                                hdnMorningRs.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceRate"]);
                                hdnServiceId1.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceId"]);
                                ViewState["1Visit"] = ds1.Tables[0].Columns[2].ToString();
                                gvIpVisit.Columns.FindByUniqueName("col2").Visible = false;
                                gvIpVisit.Columns.FindByUniqueName("col3").Visible = false;
                                gvIpVisit.Columns.FindByUniqueName("col4").Visible = false;
                            }
                        }
                        gvIpVisit.DataBind();
                    }
                }
                else
                {
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    if (cmbDoctor.SelectedValue != "0")
                    {
                        lblMsg.Text = "No Service is tagged for the Doctor " + common.myStr(cmbDoctor.Text);
                    }
                }
                ds1.Dispose();
            }
            else if (common.myStr(ViewState["OPIP"]).Equals("O"))// OP
            {
                ds = new DataSet();
                cmbOpVisit.Items.Clear();
                cmbOpVisit.Text = "";
                ds = objClsBilling.DoctorConsultationVisit(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["facilityId"]), common.myInt(cmbDoctor.SelectedValue));

                if (ds.Tables.Count > 0)
                {
                    cmbOpVisit.DataSource = ds;
                    cmbOpVisit.DataTextField = "ServiceName";
                    cmbOpVisit.DataValueField = "serviceid";
                    cmbOpVisit.DataBind();
                    cmbOpVisit.SelectedIndex = 0;
                }
                ///* For Visit Rule followUp*/
                hshInput = new Hashtable();

                hshInput.Add("iHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hshInput.Add("iFacilityId", common.myInt(Session["FacilityId"]));
                hshInput.Add("iCompanyId", common.myInt(ViewState["CompanyId"]));
                hshInput.Add("iInsuranceId", common.myInt(ViewState["InsuranceId"]));
                hshInput.Add("iCardId", common.myInt(ViewState["CardId"]));
                hshInput.Add("iDoctorId", common.myInt(cmbDoctor.SelectedValue));
                hshInput.Add("iRegistration", common.myInt(txtRegID.Text));
                hshOutput.Add("cVisitCode", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspGetPatientVisitCode", hshInput, hshOutput);
                if (common.myStr(hshOutput["cVisitCode"]) != "")
                {
                    cmbOpVisit.SelectedIndex = cmbOpVisit.Items.IndexOf(cmbOpVisit.Items.FindItemByValue(common.myStr(hshOutput["cVisitCode"])));
                    if (cmbOpVisit.SelectedIndex != -1)
                    {
                        cmbOpVisit.SelectedItem.BackColor = System.Drawing.Color.Aquamarine;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { hshInput = null; hshOutput = null; objDl = null; objClsBilling = null; ds.Dispose(); }
    }
    /// <summary>
    /// Bind default grid
    /// </summary>
    private void bindDefaultConsultationGrid()
    {
        try
        {
            DataRow dR;
            DataTable dtTmp = CreateConsultantTable();
            dR = dtTmp.NewRow();
            dR["OrderNo"] = null;
            dR["DoctorId"] = 0;
            dR["DoctorName"] = "";
            dR["ServiceName"] = "";
            dR["ServiceId"] = 0;
            dR["ChargePercentage"] = 0;
            dR["Charge"] = 0;
            dtTmp.Rows.Add(dR);

            gVConsultation.DataSource = dtTmp;
            gVConsultation.DataBind();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    private DataTable CreateConsultantTable()
    {
        DataTable dtTmp = new DataTable();
        DataColumn dC;
        dC = new DataColumn("ID", typeof(Int32));
        dtTmp.Columns.Add(dC);
        dtTmp.Columns["ID"].AutoIncrement = true;
        dtTmp.Columns["ID"].AutoIncrementSeed = 1;
        dtTmp.Columns["ID"].AutoIncrementStep = 1;
        dC = new DataColumn("OrderNo", typeof(String));
        dtTmp.Columns.Add(dC);
        dC = new DataColumn("DoctorId", typeof(Int32));
        dtTmp.Columns.Add(dC);
        dC = new DataColumn("DoctorName", typeof(String));
        dtTmp.Columns.Add(dC);
        dC = new DataColumn("ServiceName", typeof(String));
        dtTmp.Columns.Add(dC);
        dC = new DataColumn("ServiceId", typeof(Int32));
        dtTmp.Columns.Add(dC);
        dC = new DataColumn("ChargePercentage", typeof(Int32));
        dtTmp.Columns.Add(dC);
        dC = new DataColumn("Charge", typeof(Int32));
        dtTmp.Columns.Add(dC);
        return dtTmp;
    }
    #endregion

    void BindPatientHiddenDetails(int RegistrationNo)
    {
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);
        DataSet ds = new DataSet();
        BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
        DataView dvIP = new DataView();
        DataTable dt = new DataTable();
        try
        {
            if (RegistrationNo > 0)
            {
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int EncodedBy = common.myInt(Session["UserId"]);
                //  int EncodedBy = common.myInt(Session["LoginDoctorId"]);

                if (common.myStr(ViewState["OPIP"]).Equals("O"))
                {
                    ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, 0, common.myInt(Session["UserId"]));
                    //  ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, 0, common.myInt(Session["LoginDoctorId"]));

                }
                else
                {
                    ds = objIPBill.GetPatientDetailsIP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, common.myInt(Session["UserId"]), 0, "");
                    // ds = objIPBill.GetPatientDetailsIP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, common.myInt(Session["LoginDoctorId"]), 0, "");

                }
                if (ds.Tables.Count > 0)
                {
                    dvIP = new DataView(ds.Tables[0]);
                    dt = new DataTable();
                    dt = dvIP.ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];

                        lblMsg.Text = "";

                        hdnConsultantId.Value = common.myStr(dr["ConsultingDoctorId"]);
                        hdhSpecialzationId.Value = common.myStr(dr["SpecialisationId"]);
                        hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);

                        if (!common.myStr(dr["EncounterId"]).Equals(""))
                        {
                            hdnCompanyCode.Value = common.myStr(dr["CompanyCode"]);
                            hdnInsCode.Value = common.myStr(dr["InsuranceCode"]);
                        }
                        else
                        {
                            hdnCompanyCode.Value = common.myStr(dr["PatientSponsorId"]);
                            hdnInsCode.Value = common.myStr(dr["PatientPayorId"]);
                        }
                        hdnExternalReg.Value = common.myStr(dr["ExternalPatient"]);
                    }
                }
                else
                {
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "Patient not found !";
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            bParse = null; bC = null; objLISMaster = null; ds.Dispose(); objIPBill = null; dvIP.Dispose(); dt.Dispose();
        }
    }

    protected void gvIpVisit_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                Label lblMorningCharges = (Label)e.Item.FindControl("lblMorningCharges");
                Label lblEveningCharges = (Label)e.Item.FindControl("lblEveningCharges");
                Label lblEmergencyCharges = (Label)e.Item.FindControl("lblEmergencyCharges");
                Label lblEmergencyCharges2nd = (Label)e.Item.FindControl("lblEmergencyCharges2nd");

                lblMorningCharges.Text = "Morning Visit (" + common.myStr(hdnMorningRs.Value) + ")";
                lblEveningCharges.Text = "Evening Visit (" + common.myStr(hdnEveningRs.Value) + ")";
                lblEmergencyCharges.Text = "Emergency 1st Visit (" + common.myStr(hdnEmergencyRs.Value) + ")";
                lblEmergencyCharges2nd.Text = "Emergency 2nd Visit (" + common.myStr(HdnEmergencyRs2.Value) + ")";
            }
            if (e.Item is GridDataItem)
            {
                CheckBox chk1 = (CheckBox)e.Item.FindControl("chk1");
                CheckBox chk2 = (CheckBox)e.Item.FindControl("chk2");
                CheckBox chk3 = (CheckBox)e.Item.FindControl("chk3");
                CheckBox chk4 = (CheckBox)e.Item.FindControl("chk4");

                HiddenField hdChk1 = (HiddenField)e.Item.FindControl("hdChk1");
                HiddenField hdChk2 = (HiddenField)e.Item.FindControl("hdChk2");
                HiddenField hdChk3 = (HiddenField)e.Item.FindControl("hdChk3");
                HiddenField hdChk4 = (HiddenField)e.Item.FindControl("hdChk4");

                if (!common.myStr(ViewState["1Visit"]).Equals(""))
                {
                    hdChk1.Value = common.myStr(DataBinder.Eval(e.Item.DataItem, common.myStr(ViewState["1Visit"])));
                    if (common.myStr(hdChk1.Value).Equals("0") || common.myStr(hdChk1.Value).Equals(""))
                    {
                        chk1.Checked = false;
                        chk1.Enabled = true;
                    }
                    else
                    {
                        chk1.Checked = true;
                        chk1.Enabled = false;
                    }
                }
                if (!common.myStr(ViewState["2Visit"]).Equals(""))
                {
                    hdChk2.Value = common.myStr(DataBinder.Eval(e.Item.DataItem, common.myStr(ViewState["2Visit"])));
                    if (common.myStr(hdChk2.Value).Equals("0") || common.myStr(hdChk2.Value).Equals(""))
                    {
                        chk2.Checked = false;
                        chk2.Enabled = true;
                    }
                    else
                    {
                        chk2.Checked = true;
                        chk2.Enabled = false;
                    }
                }
                if (!common.myStr(ViewState["3Visit"]).Equals(""))
                {
                    hdChk3.Value = common.myStr(DataBinder.Eval(e.Item.DataItem, common.myStr(ViewState["3Visit"])));
                    if (common.myStr(hdChk3.Value).Equals("0") || common.myStr(hdChk3.Value).Equals(""))
                    {
                        chk3.Checked = false;
                        chk3.Enabled = true;
                    }
                    else
                    {
                        chk3.Checked = true;
                        chk3.Enabled = false;
                    }
                }
                if (!common.myStr(ViewState["4Visit"]).Equals(""))
                {
                    hdChk4.Value = common.myStr(DataBinder.Eval(e.Item.DataItem, common.myStr(ViewState["4Visit"])));
                    if (common.myStr(hdChk4.Value).Equals("0") || common.myStr(hdChk4.Value).Equals(""))
                    {
                        chk4.Checked = false;
                        chk4.Enabled = true;
                    }
                    else
                    {
                        chk4.Checked = true;
                        chk4.Enabled = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 1;
        dt.Columns["SNo"].AutoIncrementStep = 1;

        dt.Columns.Add("DetailId");
        dt.Columns.Add("ServiceId");
        dt.Columns.Add("OrderId");
        dt.Columns.Add("UnderPackage");
        dt.Columns.Add("PackageId");
        dt.Columns.Add("ServiceType");
        dt.Columns.Add("DoctorID");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("DoctorRequired");
        dt.Columns.Add("DepartmentId");
        dt.Columns.Add("ServiceName");
        dt.Columns.Add("ResourceId");
        dt.Columns.Add("Units");
        dt.Columns.Add("ServiceAmount");
        dt.Columns.Add("DoctorAmount");
        dt.Columns.Add("Charge");
        dt.Columns.Add("ServiceDiscountPercentage");
        dt.Columns.Add("ServiceDiscountAmount");
        dt.Columns.Add("DoctorDiscountPercentage");
        dt.Columns.Add("DoctorDiscountAmount");
        dt.Columns.Add("TotalDiscount");
        dt.Columns.Add("AmountPayableByPatient");
        dt.Columns.Add("AmountPayableByPayer");
        dt.Columns.Add("IsPackageMain");
        dt.Columns.Add("IsPackageService");
        dt.Columns.Add("MainSurgeryId");
        dt.Columns.Add("IsSurgeryMain");
        dt.Columns.Add("IsSurgeryService");
        dt.Columns.Add("ServiceRemarks");
        dt.Columns.Add("ServiceStatus");
        dt.Columns.Add("IsExcluded");
        dt.Columns.Add("IsApprovalReq");
        dt.Columns.Add("ChargePercentage");
        dt.Columns.Add("CopayAmt");
        dt.Columns.Add("CopayPerc");
        dt.Columns.Add("IsCoPayOnNet");
        dt.Columns.Add("DeductableAmount");

        ViewState["GridData"] = dt;
        return dt;
    }

    public DataTable getRegistrationService(Boolean isForConsultaion)
    {
        BaseC.RestFulAPI objBilling = new BaseC.RestFulAPI(sConString);
        DataTable dt = CreateTable();
        try
        {
            if (hdnExternalReg.Value.ToString() == "0" || isForConsultaion)
            {
                DataSet ds = objBilling.GetPatientRegistartionCharges(common.myInt(Session["HospitalLocationID"]),
                          common.myInt(hdnRegistrationId.Value.Trim()), common.myInt(Session["FacilityId"]), common.myInt(hdnCompanyCode.Value));
                dt = ds.Tables[0];
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            objBilling = null;
        }
        return dt;

    }
}
