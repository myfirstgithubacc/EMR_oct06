using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using System.Configuration;

public partial class EMRBILLING_VisitPopup : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //clsExceptionLog objException = new clsExceptionLog();

    //BaseC.ParseData bc = new BaseC.ParseData();
    //BaseC.Hospital baseHc;
    //BaseC.EMRBilling.clsOrderNBill BaseBill;
    //Hashtable hshInput;
    //Hashtable hshOutput;
    //DAL.DAL objDl;
    //BaseC.clsLISMaster objLISMaster;
    //BaseC.EMRBilling objBill;

    //protected void Page_PreInit(object sender, System.EventArgs e)
    //{
    //    Page.Theme = "DefaultControls";

    //    if (!common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("YES"))
    //    {
    //        Page.MasterPageFile = "/Include/Master/BlankMaster.master";
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null || Session["HospitalLocationID"] == null || Session["FacilityId"] == null)
            {
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

            //BaseC.Hospital baseHc = new BaseC.Hospital(sConString);
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
            //BaseC.EMRBilling objBill = new BaseC.EMRBilling(sConString);
            if (!IsPostBack)
            {
                if (common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("YES"))
                {
                    ibtnClose.Visible = false;
                }

                #region set view state values

                ViewState["RegistrationId"] = common.myInt(Request.QueryString["RegId"]);
                ViewState["RegistrationNo"] = common.myLong(Request.QueryString["RegNo"]);
                ViewState["EncounterId"] = common.myInt(Request.QueryString["EncId"]);
                ViewState["EncounterNo"] = common.myStr(Request.QueryString["EncNo"]);
                ViewState["OPIP"] = common.myStr(Request.QueryString["OP_IP"]);
                ViewState["CompanyId"] = common.myInt(Request.QueryString["CompanyId"]);
                ViewState["PayerType"] = common.myStr(Request.QueryString["PayerType"]);
                ViewState["InsuranceId"] = common.myInt(Request.QueryString["InsuranceId"]);
                ViewState["CardId"] = common.myInt(Request.QueryString["CardId"]);
                ViewState["CDocId"] = common.myInt(Request.QueryString["CDocId"]);

                if (common.myInt(ViewState["RegistrationId"]).Equals(0))
                {
                    ViewState["RegistrationId"] = common.myInt(Session["RegistrationId"]);
                }
                if (common.myLong(ViewState["RegistrationNo"]).Equals(0))
                {
                    ViewState["RegistrationNo"] = common.myLong(Session["RegistrationNo"]);
                }
                if (common.myInt(ViewState["EncounterId"]).Equals(0))
                {
                    ViewState["EncounterId"] = common.myInt(Session["EncounterId"]);
                }
                if (common.myStr(ViewState["EncounterNo"]).Equals(string.Empty))
                {
                    ViewState["EncounterNo"] = common.myStr(Session["EncounterNo"]);
                }
                if (common.myStr(ViewState["OPIP"]).Equals(string.Empty))
                {
                    ViewState["OPIP"] = common.myStr(Session["OPIP"]);
                }

                #endregion

                //Session["IPVisitCheck"] = 0;
                ViewState["GridData"] = null;
                clearAll();
                txtRegID.Text = common.myStr(ViewState["RegistrationId"]);
                txtRegNo.Text = common.myStr(ViewState["RegistrationNo"]);
                txtEncId.Text = common.myStr(ViewState["EncounterId"]);
                txtEncNo.Text = common.myStr(ViewState["EncounterNo"]);
                ViewState["IPNumber"] = txtEncId.Text.Trim();
                ViewState["Page"] = common.myStr(Request.QueryString["Page"]);

                BaseC.clsEMRBilling objBilling = new BaseC.clsEMRBilling(sConString);
                BaseC.EMRBilling objBill = new BaseC.EMRBilling(sConString);
                DataSet ds = objBill.GetPatientLastVisit(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(txtRegID.Text.Trim()));
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
                //ibtnSave.Attributes.Add("onclick", "this.disabled=true;"); 
                //PostBackOptions optionsSubmit = new PostBackOptions(ibtnSave);
                //ibtnSave.OnClientClick = "disableButtonOnClick(this, 'Please wait...', 'disabled_button'); ";
                //ibtnSave.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmit);
                // do the same for the asynchronous postback button.

                //PostBackOptions optionsSubmitAsync = new PostBackOptions(btnSubmitAsync);

                //btnSubmitAsync.OnClientClick = "disableButtonOnClick(this, 'Please wait...', 'disabled_button'); ";

                //btnSubmitAsync.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmitAsync);

                //added by bhakti  
                ViewState["isRequireIPBillOfflineMarking"] = common.myStr(objBilling.getHospitalSetupValue("isRequireIPBillOfflineMarking", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));

            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void cmbSpecial_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        fillDoctor();
        fillVisitGrid();
    }

    protected void cmbDoctor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // if(cmbDoctor.SelectedValue.ToString() != "")
        fillVisitGrid();
        //Session["IPVisitCheck"] = 0;
        cmbOpVisit.Enabled = true;
    }

    protected void ibtSave_OnClick(object sender, EventArgs e)
    {
        Hashtable hshInput = new Hashtable();
        Hashtable hshOutput = new Hashtable();
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        BaseC.Patient objBp = new BaseC.Patient(sConString);

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataTable dtNew = new DataTable();

        StringBuilder strVisitXML = new StringBuilder();
        StringBuilder str32 = new StringBuilder();
        StringBuilder str33 = new StringBuilder();
        StringBuilder str34 = new StringBuilder();
        StringBuilder str35 = new StringBuilder();
        ArrayList coll = new ArrayList();
        try
        {
            if (ViewState["isRequireIPBillOfflineMarking"].ToString() == "Y" && common.myStr(Session["OPIP"]).Equals("I") && common.myStr(Session["PaymentType"]).Equals("C") && common.myStr(Session["IsOffline"]).Equals("True"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Patient is Offline, No transaction allow !')", true);
                return;
            }
        }
        catch (Exception ex) 
        {

            throw ex;
        }
        try
        {
            if (ViewState["isRequireIPBillOfflineMarking"].ToString() == "Y" && common.myStr(ViewState["OP_IP"]).Equals("I") && common.myStr(Session["PaymentType"]).Equals("C") && common.myStr(Session["IsOffline"]).Equals("True"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Patient is Offline, No transaction allow !')", true);
                return;
            }
            if (common.myStr(cmbDoctor.SelectedValue) != "0" && common.myStr(cmbDoctor.SelectedValue) != "")
            {
                int sno = 0;

                #region IPVisit Order
                if (common.myStr(ViewState["OPIP"]).Equals("I") && !common.myStr(ViewState["IPNumber"]).Equals(string.Empty))
                {
                    //if (common.myInt(Session["IPVisitCheck"]) == 0)
                    //{
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
                        //string msg = BaseBill.saveOrders(Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(Session["FacilityId"].ToString()), Convert.ToInt32(txtRegID.Text.ToString()), Convert.ToInt32(txtEncId.Text.ToString()), strVisitXML.ToString(), "", Convert.ToInt32(Session["UserID"].ToString()), 0, Convert.ToInt32(Request.QueryString["CompanyId"].ToString()), stype, common.myStr(Request.QueryString["PayerType"].ToString()), common.myStr(Request.QueryString["OP_IP"].ToString()), common.myInt(Request.QueryString["InsuranceId"].ToString()), common.myInt(Request.QueryString["CardId"].ToString()));

                        //if (common.myStr(ViewState["Page"]) == "Ward") // santosh
                        //{
                        string StrBackDate = BaseBill.WardValidateDoctorVisits(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(txtRegID.Text),
                            common.myInt(txtEncId.Text), strVisitXML.ToString(), common.myInt(Session["EmployeeId"]), common.myDate(DateTime.Now), common.myInt(Session["UserID"]));
                        if (StrBackDate != "1")
                        {
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                            lblMsg.Text = StrBackDate;
                            return;
                        }
                        //}

                        Hashtable hshOut = BaseBill.saveOrders(
                            common.myInt(Session["HospitalLocationID"]),
                            common.myInt(Session["FacilityId"]),
                            common.myInt(txtRegID.Text),
                            common.myInt(txtEncId.Text),
                            strVisitXML.ToString(), "",
                            common.myInt(Session["UserID"]),
                            common.myInt(cmbDoctor.SelectedValue),
                            common.myInt(ViewState["CompanyId"]),
                            stype,
                            common.myStr(ViewState["PayerType"]),
                            common.myStr(ViewState["OPIP"]),
                            common.myInt(ViewState["InsuranceId"]),
                            common.myInt(ViewState["CardId"]),
                            common.myDate(DateTime.Now), sChargeCalculationRequired,
                            common.myInt(Session["EntrySite"]), common.myInt(Session["ModuleId"]));

                        if (common.myStr(hshOut["chvErrorStatus"]).Length == 0)
                        {
                            //Session["IPVisitCheck"] = 1;
                            //////if (Convert.ToInt32(hshOut["intNEncounterID"]) > 0)
                            //////{
                            //////    Session["EncounterID"] = Convert.ToInt32(hshOut["intNEncounterID"]);
                            //////}
                            fillVisitGrid();
                            lblMsg.ForeColor = System.Drawing.Color.Green;
                            lblMsg.Text = "Order Saved Successfully";
                            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                        }
                        else
                        {
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                            lblMsg.Text = "There is some error while saving order." + hshOut["chvErrorStatus"].ToString();
                        }
                    }
                    //}
                    //else if (common.myInt(Session["IPVisitCheck"]) == 1)
                    //{
                    //    // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                    //    //
                    //    fillVisitGrid();
                    //    lblMsg.ForeColor = System.Drawing.Color.Green;
                    //    lblMsg.Text = "Order Saved Successfully";
                    //}
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

            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
                    //Check duplicate service------------------------------------------------------------------
                    dvdup = new DataView();
                    dvdup = dt.Copy().DefaultView;

                    dvdup.RowFilter = " ServiceId = " + common.myStr(cmbOpVisit.SelectedValue);

                    if (dvdup.ToTable().Rows.Count > 0)
                    {

                        Alert.ShowAjaxMsg("Service already exist !", Page);
                        //ShowPatientDetails(common.myStr(txtRegistrationNo.Text));
                        return;
                    }
                }
                //----------------------------------------------------------------------------------------------
                hshServiceDetail = new Hashtable();
                //hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                //    common.myInt(Request.QueryString["CompanyId"]), common.myInt(Request.QueryString["InsuranceId"]), common.myInt(Request.QueryString["CardId"]),
                //    common.myStr(Request.QueryString["OP_IP"]), common.myInt(ddlService.SelectedValue), common.myInt(txtRegID.Text), common.myInt(txtEncId.Text), 0);
                int iBedCategorId = 0;
                int iSpecialisationId = 0;
                int iProviderId = 0;
                String sModifierCode = "";
                int iServiceId = common.myInt(cmbOpVisit.SelectedValue);

                if (common.myInt(cmbSpecial.SelectedValue) > 0)
                {
                    iSpecialisationId = common.myInt(cmbSpecial.SelectedValue);
                }
                if (common.myInt(cmbDoctor.SelectedValue) > 0)
                {
                    iProviderId = common.myInt(cmbDoctor.SelectedValue);
                }

                //hshServiceDetail = BaseBill.GetSingleServiceChargeVisit(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                //    common.myInt(Request.QueryString["CompanyId"]), iBedCategorId, iSpecialisationId, iProviderId, iServiceId, sModifierCode);
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

            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
                        cmbSpecial.SelectedValue = common.myStr(hdhSpecialzationId.Value);
                    else
                        cmbSpecial.SelectedIndex = 0;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();

            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
            //cmbDoctor.SelectedIndex = 0;
            if (common.myStr(ViewState["OPIP"]).Equals("I"))
            {

                int CunsultingDoctorId = 0;
                if (common.myInt(ViewState["CDocId"]) > 0)
                    CunsultingDoctorId = common.myInt(ViewState["CDocId"]);
                else
                    CunsultingDoctorId = common.myInt(hdnConsultantId.Value);

                cmbDoctor.SelectedIndex = cmbDoctor.Items.IndexOf(cmbDoctor.Items.FindItemByValue(common.myStr(CunsultingDoctorId)));
            }
            //cmbDoctor.SelectedValue = hdnConsultantId.Value;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();

            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
                hshInput.Add("EmployeeId", common.myInt(Session["EmployeeId"]));

                DataSet ds1 = objDl.FillDataSet(CommandType.StoredProcedure, "UspIPVisit", hshInput);
                if (ds1.Tables.Count > 1)
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        lblMsg.Text = string.Empty;
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

                                ViewState["1Visit"] = common.myStr(ds1.Tables[0].Columns[2]).Trim();
                                ViewState["2Visit"] = common.myStr(ds1.Tables[0].Columns[3]).Trim();
                                ViewState["3Visit"] = common.myStr(ds1.Tables[0].Columns[4]).Trim();
                                ViewState["4Visit"] = common.myStr(ds1.Tables[0].Columns[5]).Trim();
                                gvIpVisit.Columns.FindByUniqueName("col2").Visible = true;
                                gvIpVisit.Columns.FindByUniqueName("col3").Visible = true;
                                gvIpVisit.Columns.FindByUniqueName("col4").Visible = true;

                            }
                            else if (ds1.Tables[0].Columns.Count == 5)
                            {
                                hdnMorningRs.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceRate"]);
                                hdnEveningRs.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceRate"]);
                                hdnEmergencyRs.Value = common.myStr(ds1.Tables[1].Rows[2]["ServiceRate"]);
                                //HdnEmergencyRs2.Value = common.myStr(ds1.Tables[1].Rows[3]["ServiceRate"]);
                                hdnServiceId1.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceId"]);
                                hdnServiceId2.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceId"]);
                                hdnServiceId3.Value = common.myStr(ds1.Tables[1].Rows[2]["ServiceId"]);
                                //hdnServiceId4.Value = common.myStr(ds1.Tables[1].Rows[3]["ServiceId"]);

                                ViewState["1Visit"] = common.myStr(ds1.Tables[0].Columns[2]).Trim();
                                ViewState["2Visit"] = common.myStr(ds1.Tables[0].Columns[3]).Trim();
                                ViewState["3Visit"] = common.myStr(ds1.Tables[0].Columns[4]).Trim();
                                gvIpVisit.Columns.FindByUniqueName("col2").Visible = true;
                                gvIpVisit.Columns.FindByUniqueName("col3").Visible = true;
                                gvIpVisit.Columns.FindByUniqueName("col4").Visible = false;
                            }
                            else if (ds1.Tables[0].Columns.Count == 4)
                            {
                                hdnMorningRs.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceRate"]);
                                hdnEveningRs.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceRate"]);
                                //hdnEmergencyRs.Value = common.myStr(ds1.Tables[1].Rows[2]["ServiceRate"]);
                                //HdnEmergencyRs2.Value = common.myStr(ds1.Tables[1].Rows[3]["ServiceRate"]);
                                hdnServiceId1.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceId"]);
                                hdnServiceId2.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceId"]);
                                //hdnServiceId3.Value = common.myStr(ds1.Tables[1].Rows[2]["ServiceId"]);
                                //hdnServiceId4.Value = common.myStr(ds1.Tables[1].Rows[3]["ServiceId"]);
                                ViewState["1Visit"] = common.myStr(ds1.Tables[0].Columns[2]).Trim();
                                ViewState["2Visit"] = common.myStr(ds1.Tables[0].Columns[3]).Trim();
                                gvIpVisit.Columns.FindByUniqueName("col2").Visible = true;
                                gvIpVisit.Columns.FindByUniqueName("col3").Visible = false;
                                gvIpVisit.Columns.FindByUniqueName("col4").Visible = false;
                            }
                            else if (ds1.Tables[0].Columns.Count == 3)
                            {
                                hdnMorningRs.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceRate"]);
                                //hdnEveningRs.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceRate"]);
                                //hdnEmergencyRs.Value = common.myStr(ds1.Tables[1].Rows[2]["ServiceRate"]);
                                //HdnEmergencyRs2.Value = common.myStr(ds1.Tables[1].Rows[3]["ServiceRate"]);
                                hdnServiceId1.Value = common.myStr(ds1.Tables[1].Rows[0]["ServiceId"]);
                                // hdnServiceId2.Value = common.myStr(ds1.Tables[1].Rows[1]["ServiceId"]);
                                //hdnServiceId3.Value = common.myStr(ds1.Tables[1].Rows[2]["ServiceId"]);
                                //hdnServiceId4.Value = common.myStr(ds1.Tables[1].Rows[3]["ServiceId"]);
                                ViewState["1Visit"] = common.myStr(ds1.Tables[0].Columns[2]).Trim();
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
            }
            else if (common.myStr(ViewState["OPIP"]).Equals("O"))// OP
            {
                ds = new DataSet();
                cmbOpVisit.Items.Clear();
                cmbOpVisit.Text = "";
                //if (ViewState["OPVisit"] != null)
                //{
                //    cmbOpVisit.DataSource = (DataSet)ViewState["OPVisit"];
                //}
                //else
                //{

                ds = objClsBilling.DoctorConsultationVisit(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["facilityId"])
                     , common.myInt(cmbDoctor.SelectedValue));

                if (ds.Tables.Count > 0)
                {
                    //hshInput.Add("intHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
                    //ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOpVisit", hshInput);
                    cmbOpVisit.DataSource = ds;
                    //ViewState["OPVisit"] = ds;
                    //}
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
                    //cmbOpVisit.SelectedValue = hshOutput["cVisitCode"].ToString();
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
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
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
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
        DataView dvIP;
        DataTable dt = new DataTable();

        try
        {
            if (RegistrationNo > 0)
            {
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int EncodedBy = common.myInt(Session["UserId"]);

                if (common.myStr(ViewState["OPIP"]).Equals("O"))
                {
                    ds = bC.getPatientDetails(HospId, FacilityId, 0, RegistrationNo, 0, EncodedBy);
                    lblInfoEncNo.Visible = false;
                    lblInfoAdmissionDt.Visible = false;
                    lblEncounterNo.Visible = false;
                    lblAdmissionDate.Visible = false;
                }
                else
                {
                    ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, 0, RegistrationNo, EncodedBy, 0, "");
                }

                if (ds.Tables.Count > 0)
                {
                    dvIP = new DataView(ds.Tables[0]);
                    //    dvIP.RowFilter = "OPIP = 'I'";
                    dt = new DataTable();
                    dt = dvIP.ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];

                        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                        lblDob.Text = common.myStr(dr["DOB"]);
                        lblMobile.Text = common.myStr(dr["MobileNo"]);
                        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                        lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                        lblMsg.Text = "";
                        RegNo.Text = common.myStr(ViewState["RegistrationNo"]);
                        hdnConsultantId.Value = common.myStr(dr["ConsultingDoctorId"]);
                        hdhSpecialzationId.Value = common.myStr(dr["SpecialisationId"]);
                        hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);

                        if (common.myStr(dr["EncounterId"]) != "")
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
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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

                lblMorningCharges.Text = "Morning Visit (" + hdnMorningRs.Value + ")";
                lblEveningCharges.Text = "Evening Visit (" + hdnEveningRs.Value + ")";
                lblEmergencyCharges.Text = "Emergency/Referral 1st Visit (" + hdnEmergencyRs.Value + ")";
                lblEmergencyCharges2nd.Text = "Emergency/Referral 2nd Visit (" + HdnEmergencyRs2.Value + ")";
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
                //LinkButton lnkChk3 = (LinkButton)e.Item.FindControl("lnkChk3");
                //LinkButton lnkChk2 = (LinkButton)e.Item.FindControl("lnkChk2");
                //LinkButton lnkChk1 = (LinkButton)e.Item.FindControl("lnkChk1");
                //LinkButton lnkChk4 = (LinkButton)e.Item.FindControl("lnkChk4");
                GridDataItem dataItem = (GridDataItem)e.Item;

                if (common.myLen(ViewState["1Visit"]) > 0)
                {
                    hdChk1.Value = common.myInt(DataBinder.Eval(e.Item.DataItem, ViewState["1Visit"].ToString())).ToString();

                    chk1.Checked = (common.myInt(hdChk1.Value) > 0);
                    chk1.Enabled = !(common.myInt(hdChk1.Value) > 0);
                }
                if (common.myLen(ViewState["2Visit"]) > 0)
                {
                    hdChk2.Value = common.myStr(DataBinder.Eval(e.Item.DataItem, ViewState["2Visit"].ToString())).ToString();
                    // hdChk2.Value = common.myStr(ViewState["2Visit"]);
                    chk2.Checked = (common.myInt(hdChk2.Value) > 0);
                    chk2.Enabled = !(common.myInt(hdChk2.Value) > 0);
                }
                if (common.myLen(ViewState["3Visit"]) > 0)
                {
                    hdChk3.Value = common.myInt(DataBinder.Eval(e.Item.DataItem, ViewState["3Visit"].ToString())).ToString();

                    chk3.Checked = (common.myInt(hdChk3.Value) > 0);
                    chk3.Enabled = !(common.myInt(hdChk3.Value) > 0);
                }
                if (common.myLen(ViewState["4Visit"]) > 0)
                {
                    hdChk4.Value = common.myInt(DataBinder.Eval(e.Item.DataItem, ViewState["4Visit"].ToString())).ToString();

                    chk4.Checked = (common.myInt(hdChk4.Value) > 0);
                    chk4.Enabled = !(common.myInt(hdChk4.Value) > 0);
                }
                if (common.myStr(Session["FacilityName"]).ToUpper().Contains("BLK") || common.myStr(Session["FacilityName"]).Trim().ToUpper().Contains("B L KAPUR"))
                {
                    chk3.Enabled = false;
                    chk4.Enabled = false;
                }

                //if (common.myDate(dataItem["VisitDate"].Text).ToString("yyyy/MM/dd").Equals(DateTime.Now.ToString("yyyy/MM/dd")))
                //{
                //    chk1.Enabled = true;
                //    chk2.Enabled = true;
                //    chk3.Enabled = true;
                //    chk4.Enabled = true;
                //}
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objBilling = null;
        }
        return dt;

    }

}
