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
using System.Text;




public partial class OTScheduler_AddChargeTimewisOT : System.Web.UI.Page
{

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.EMRBilling.clsOrderNBill BaseBill;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            ViewState["OPServicesInv_"] = null;
            ViewState["RegID"] = common.myStr(Request.QueryString["RegId"]);
            ViewState["RegNo"] = common.myStr(Request.QueryString["RegNo"]);
            ViewState["EncId"] = common.myStr(Request.QueryString["EncId"]);
            ViewState["EncNo"] = common.myStr(Request.QueryString["EncNo"]);

            ViewState["CompanyId"] = common.myStr(Request.QueryString["CompanyId"]);
            ViewState["InsuranceId"] = common.myStr(Request.QueryString["InsuranceId"]);
            ViewState["CardId"] = common.myStr(Request.QueryString["CardId"]);

            dtpfromdate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
            dtpfromdate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now);
            dtpTodate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
            dtpTodate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
            dtpTodate.SelectedDate = common.myDate(DateTime.Now);
            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));
            BindProvider();
            FillService();
            CalculateTime();
            BindGrid();
            BindPatientHiddenDetails(common.myInt(Request.QueryString["RegNo"]));
        }
    }
    protected void BindPatientHiddenDetails(int RegistrationNo)
    {
        // BaseC.ParseData bParse = new BaseC.ParseData();
        // BaseC.Patient bC = new BaseC.Patient(sConString);
        // objLISMaster = new BaseC.clsLISMaster(sConString);
        if (RegistrationNo > 0)
        {
            //x?RegId=12876&EncId=22547&RegNo=300002912&EncNo=14/304&CardId=0&CompanyId=56&InsuranceId=56&OP_IP=I&OTId=3318&rwndrnd=0.18611511302500983
            int HospId = common.myInt(Session["HospitalLocationID"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int RegId = 0;
            int EncounterId = 0;
            int EncodedBy = common.myInt(Session["UserId"]);
            DataSet ds = new DataSet();
            lblregNo.Text = RegistrationNo.ToString();
            //if (common.myStr(ViewState["OP_IP"]) == "O")
            //{
            //    ds = bC.getPatientDetails(HospId, FacilityId, RegId, RegistrationNo, EncounterId, EncodedBy);
            //    lblInfoEncNo.Visible = false;
            //    lblInfoAdmissionDt.Visible = false;
            //    lblEncounterNo.Visible = false;
            //    lblAdmissionDate.Visible = false;
            //}
            //else
            //{
            BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
            ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, RegistrationNo, EncodedBy, 0, "");
            //}
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataRow dr = ds.Tables[0].Rows[0];

                    lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                    lblDob.Text = common.myStr(dr["DOB"]);
                    lblMobile.Text = common.myStr(dr["MobileNo"]);
                    lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                    lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                    lblMessage.Text = "";
                    //if (common.myStr(ViewState["From"]) == "BILL")
                    //{
                    //    hdnCompanyId.Value = common.myStr(ViewState["CompanyId"]);
                    //    hdnInsuranceId.Value = common.myStr(ViewState["InsuranceId"]);
                    //    hdnCardId.Value = common.myStr(ViewState["CardId"]);
                    //}
                    //else
                    //{
                    //    hdnCompanyId.Value = common.myStr(dr["PayorId"]);
                    //    hdnInsuranceId.Value = common.myStr(dr["SponsorId"]);
                    //    hdnCardId.Value = common.myStr(dr["InsuranceCardId"]);
                    //}


                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Patient not found !";
                return;
            }
        }
    }
    public void FillService()
    {
        BaseC.CompanyServiceSetup objCompanyServiceSetup = new BaseC.CompanyServiceSetup(sConString);
        DataSet ds = getEquipmentForOT(common.myInt(Request.QueryString["OTId"]));
        if (ds.Tables.Count > 0)
        {
            ddlService.Items.Clear();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)ds.Tables[0].Rows[i]["EquipmentName"];
                item.Value = ds.Tables[0].Rows[i]["EquipmentId"].ToString();
                //hdServiceType.Value = ;
                item.Attributes.Add("ServiceId", ds.Tables[0].Rows[i]["Serviceid"].ToString());
                item.Attributes.Add("OTEquipmentDetailsId", ds.Tables[0].Rows[i]["OTEquipmentDetailsId"].ToString());
                this.ddlService.Items.Add(item);
                item.DataBind();
            }
            ddlService.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        }
    }

    public DataSet getEquipmentForOT(int OTBookingID)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        HshIn.Add("@intOTBookingID", OTBookingID);

        ds = objDl.FillDataSet(CommandType.Text, "select  t2.EquipmentName + (case when  Isnull(t2.ServiceId ,0)>0 then ' ( Service Tagged )' else '' end) as EquipmentName ,t2.EquipmentId,t2.Serviceid ,t1.ID as OTEquipmentDetailsId from OTEquipmentDetails t1 INNER JOIN OTEquipmentMaster t2 on  t2.EquipmentId = t1.EquipmentID inner join OTBooking t3 on t3.OTBookingID = t1.OTBookingID where t3.OTBookingID=@intOTBookingID", HshIn);
        return ds;
    }
    protected void ddlService_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getCharge();
    }
    protected void dtpfromdate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        CalculateTime();
    }

    protected void dtpTodate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        CalculateTime();
    }

    public void getCharge()
    {
        Hashtable hshServiceDetail = new Hashtable();

        BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                                    common.myInt(Session["FacilityId"]),
                                    common.myInt(ViewState["CompanyId"]),
                                    common.myInt(ViewState["InsuranceId"]),
                                    common.myInt(ViewState["CardId"]),
                                    common.myStr(Request.QueryString["OP_IP"]),
                                    common.myInt(ddlService.SelectedItem.Attributes["ServiceId"]),
                                    common.myInt(ViewState["RegID"]),
                                    common.myInt(ViewState["EncId"]), 0, 0, 0, string.Empty);

        txtCharge.Text = common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        txtDrCharge.Text = common.myDec(common.myDec(hshServiceDetail["DNchr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        hdnChargeType.Value = common.myStr(hshServiceDetail["ChargeType"]);//PH,PD
        hdnDiscount.Value = common.myStr(hshServiceDetail["DiscountPerc"]);
        ddlDoctor.SelectedIndex = 0;
        ViewState["ServiceDiscountAmount"] = common.myStr(hshServiceDetail["DiscountNAmt"]);
        ViewState["AmountPayableByPatient"] = common.myStr(hshServiceDetail["PatientNPayable"]);
        ViewState["AmountPayableByPayer"] = common.myStr(hshServiceDetail["PayorNPayable"]);
        ViewState["ServiceDiscountPercentage"] = common.myStr(hshServiceDetail["DiscountPerc"]);


        if (common.myStr(hshServiceDetail["DoctorRequired"]) == "True")
        {
            hdnDoctorRequired.Value = "1";
            ddlDoctor.Enabled = true;
            lblStarDoctor.Visible = true;
        }
        else
        {
            hdnDoctorRequired.Value = "0";
            ddlDoctor.Enabled = false;
            lblStarDoctor.Visible = false;
        }
    }

    public void CalculateTime()
    {
        getCharge();
        lblMessage.Text = "";
        DateTime dtStart = dtpfromdate.SelectedDate.Value;
        DateTime dtEnd = dtpTodate.SelectedDate.Value;
        if (dtEnd >= dtStart)
        {
            int TotalHoursDiff = common.myInt(Math.Round((dtEnd - dtStart).TotalHours, 0));
            int TotalDay = common.myInt(TotalHoursDiff / 24);
            int TotalHours = TotalHoursDiff - (TotalDay * 24);
            lblUnitDescription.Text = TotalDay.ToString() + " Days" + " " + TotalHours + " Hours";

            if (hdnChargeType.Value == "PD")// per day
            {
                if (TotalHours > 0)
                {
                    txtUnit.Text = common.myStr(TotalDay + 1);
                }
                else
                {
                    txtUnit.Text = common.myStr(TotalDay);
                }
            }
            else if (hdnChargeType.Value == "PH")// per hour
            {
                txtUnit.Text = common.myStr(TotalHoursDiff);
            }
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "From Date should not be greater than To Date !";

            dtpfromdate.SelectedDate = common.myDate(DateTime.Now);
            dtpTodate.SelectedDate = common.myDate(DateTime.Now);
            txtUnit.Text = "0";
            lblUnitDescription.Text = "0 Days" + " 0 Hours";

            return;
        }
    }


    private void BindProvider()
    {

        BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
        DataSet ds = objCM.GetDoctorList(Convert.ToInt16(Session["HospitalLocationID"]), 0, Convert.ToInt16(Session["FacilityId"]));
        ddlDoctor.DataSource = ds;
        ddlDoctor.DataValueField = "DoctorID";
        ddlDoctor.DataTextField = "DoctorName";
        ddlDoctor.DataBind();
        ddlDoctor.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));

    }
    public void btnsave_OnClick(object sender, EventArgs e)
    {
        try
        {

            ////    if (common.myInt(hdnDoctorRequired.Value) == 1 && common.myInt(ddlDoctor.SelectedValue) == 0)
            ////{
            ////    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            ////    lblMessage.Text = "Please Select Doctor !";
            ////    return;
            ////}

            BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();
            DataTable dt = new DataTable();
            foreach (GridViewRow grow in gvService.Rows)
            {
                HiddenField hdnServiceId = (HiddenField)grow.FindControl("hdnServiceId");
                TextBox txtUnits = (TextBox)grow.FindControl("txtUnits");
                //  RadComboBox ddlDoctor = (RadComboBox)grow.FindControl("ddlDoctor");
                //if (((CheckBox)grow.FindControl("chkselect")).Checked == true)
                //{






                coll.Add(common.myInt(((HiddenField)grow.FindControl("hdnServiceId")).Value)); //ServiceId INT,
                coll.Add(common.myDate(((Label)grow.FindControl("lblFromDate")).Text).ToString("yyyy-MM-dd HH:mm:00")); //VisitDate SMALLDATETIME,   common.myDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:00")
                coll.Add(common.myInt(((Label)grow.FindControl("lblUnit")).Text)); //Units TINYINT,dr["Units"]Units
                coll.Add(common.myInt(0)); //DoctorId INT, dr["DoctorID"]
                coll.Add(common.myDec(((Label)grow.FindControl("lblSrCharge")).Text)); //ServiceAmount MONEY,
                coll.Add(common.myDec(0)); //DoctorAmount MONEY,  (Label)grow.FindControl("lblDrCharge")).Text
                coll.Add(common.myDec(((HiddenField)grow.FindControl("hdnServiceDiscountAmount")).Value)); //ServiceDiscountAmount MONEY, dr["ServiceDiscountAmount"]
                coll.Add(common.myDec(0)); //DoctorDiscountAmount MONEY,dr["DoctorDiscountAmount"]
                coll.Add(common.myDec(((HiddenField)grow.FindControl("hdnAmountPayableByPatient")).Value)); //AmountPayableByPatient MONEY,dr["DoctorDiscountAmount"]
                coll.Add(common.myDec(((HiddenField)grow.FindControl("hdnAmountPayableByPayer")).Value)); //AmountPayableByPayer MONEY,dr["AmountPayableByPayer"]
                coll.Add(common.myDec(((HiddenField)grow.FindControl("hdnServiceDiscountPercentage")).Value)); //ServiceDiscountPer MONEY,dr["ServiceDiscountPercentage"]
                coll.Add(common.myDec(0)); //DoctorDiscountPer MONEY,dr["DoctorDiscountPercentage"]
                coll.Add(common.myInt(0)); //PackageId INT,dr["PackageId"]  
                coll.Add(common.myInt(((HiddenField)grow.FindControl("hdnOrderId")).Value)); //OrderId INT,
                coll.Add(common.myInt(0)); //UnderPackage BIT,  dr["UnderPackage"]             
                coll.Add(DBNull.Value); //ICDID VARCHAR(100), 
                coll.Add(DBNull.Value); //ResourceID INT,
                coll.Add(DBNull.Value); //SurgeryAmount MONEY, 
                coll.Add(DBNull.Value); //ProviderPercent MONEY,
                coll.Add(common.myInt(((Literal)grow.FindControl("ltrId")).Text)); //SeQNo INT, 
                coll.Add(DBNull.Value); //Serviceremarks Varchar(150)
                coll.Add(common.myDec(((HiddenField)grow.FindControl("hdnDetailId")).Value)); // DetailId 0 in case of new order
                coll.Add(common.myInt(0));//23//Er Order
                coll.Add(common.myStr(0));//24//pharmacyOrder
                coll.Add(common.myDec(0));//CopayPerc  dr["CopayAmt"]
                coll.Add(common.myDec(0));//DeductableAmount  dr["DeductableAmount"]
                coll.Add(common.myStr(" "));//Approval code
                coll.Add(common.myStr(((HiddenField)grow.FindControl("hdnOTEquipmentDetailsId")).Value));//OTEquipmentDetailsId
                strXML.Append(common.setXmlTable(ref coll));
                //}
            }
            ViewState["CompanyId"] = common.myStr(Request.QueryString["CompanyId"]);
            ViewState["InsuranceId"] = common.myStr(Request.QueryString["InsuranceId"]);
            ViewState["CardId"] = common.myStr(Request.QueryString["CardId"]);
            //if (common.myInt(Session["check"]) == 0)
            //{
            //Session["check"] = 1;
            Hashtable hshOut = BaseBill.saveOrdersForEquipment(
                common.myInt(Session["HospitalLocationID"]),
                common.myInt(Session["FacilityId"]),
                common.myInt(ViewState["RegID"]),
                common.myInt(ViewState["EncId"]),
                strXML.ToString(), "",
                common.myInt(Session["UserID"]),
                common.myInt(ddlDoctor.SelectedValue),
                common.myInt(ViewState["CompanyId"]),
                "",
                common.myStr(ViewState["PayerType"]),
                common.myStr("I"),
                common.myInt(ViewState["InsuranceId"]),
                common.myInt(ViewState["CardId"]),
                common.myDate(DateTime.Now), "N");

            if (common.myStr(hshOut["chvErrorStatus"]).Length == 0)
            {
                if (common.myInt(hshOut["intNEncounterID"]) > 0)
                {
                    Session["EncounterID"] = common.myInt(hshOut["intNEncounterID"]);
                }
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "Order Saved Successfully. Order No " + common.myStr(hshOut["intOrderNo"]);

                ViewState["OPServicesInv_"] = null;
                txtCharge.Text = "0.00";
                txtDrCharge.Text = "0.00";
                txtRemark.Text = "";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                BindGrid();
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "There is some error while saving order." + hshOut["chvErrorStatus"].ToString();
            }
            //}

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void btnAddToGrid_OnClick(object sender, EventArgs e)
    {
        //UpdateDataTable();
        addService();
    }
    protected void BindGrid()
    {
        try
        {
            gvService.DataSource = CreateTable();
            gvService.DataBind();
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ex.Message;
            // objException.HandleException(ex);
        }
    }

    public void addService()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["OPServicesInv_"];
        int maxId = 0;
        lblMessage.Text = "";

        if (common.myInt(ddlService.SelectedItem.Attributes["ServiceId"]) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "No need to add equipment for Un-Tagged services  !!";
            return;
        }
        if (common.myInt(ddlService.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select equipment !!";
            return;
        }
        if (common.myDate(dtpfromdate.SelectedDate) < common.myDate(lblAdmissionDate.Text))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "From Date cannot less than Admission date !!";
            return;
        }
        if (common.myDate(dtpTodate.SelectedDate) > DateTime.Now)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "To Date cannot greater than current date!!";
            return;
        }
        if (dt.Rows.Count > 0)
        {
            DataView dvf = new DataView(dt);
            dvf.RowFilter = "ServiceId = " + ddlService.SelectedValue;
            if (dvf.ToTable().Rows.Count > 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Equipment already added !!";
                return;
            }

            if (common.myInt(dt.Rows[0]["ServiceId"]) == 0) //If serviceid = 0 then remove row
            {
                dt.Rows.Clear();
            }
            else
            {
                DataView dv = new DataView(dt);
                dv.Sort = "Sno Desc";
                maxId = common.myInt(dv[0]["Sno"]);
            }
        }
        DataRow dr = dt.NewRow();
        Hashtable hshServiceDetail = new Hashtable();

        dr["Sno"] = maxId + 1;
        dr["OTEquipmentDetailsId"] = common.myInt(ddlService.SelectedItem.Attributes["OTEquipmentDetailsId"]);
        dr["ServiceId"] = common.myInt(ddlService.SelectedItem.Attributes["ServiceId"]);
        //dr["DoctorID"] = common.myInt(0);
        dr["ServiceName"] = common.myStr(ddlService.SelectedItem.Text);// common.myStr(ViewState["ServiceName"]);
        dr["Units"] = common.myInt(txtUnit.Text);


        dr["ServiceDiscountAmount"] = common.myStr(ViewState["ServiceDiscountAmount"]);
        dr["AmountPayableByPatient"] = common.myStr(ViewState["AmountPayableByPatient"]);
        dr["AmountPayableByPayer"] = common.myStr(ViewState["AmountPayableByPayer"]);
        dr["ServiceDiscountPercentage"] = common.myStr(ViewState["ServiceDiscountPercentage"]);
        dr["ServiceAmount"] = (common.myDec(txtCharge.Text) + common.myDec(txtDrCharge.Text)).ToString("F" + common.myInt(hdnDecimalPlaces.Value));// common.myDec(hshServiceDetail["NChr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        dr["DoctorAmount"] = "0.00";// common.myDec(hshServiceDetail["DNchr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        dr["FromDate"] = (common.myDate(dtpfromdate.SelectedDate)).ToString("dd-MM-yyyy HH:mm");
        dr["ToDate"] = (common.myDate(dtpTodate.SelectedDate)).ToString("dd-MM-yyyy HH:mm");

        //dr["ServiceRemarks"] = "";
        dr["NetAmount"] = ((common.myDec(txtCharge.Text) + common.myDec(txtDrCharge.Text)) * common.myInt(txtUnit.Text)).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

        dt.Rows.Add(dr);

        ViewState["OPServicesInv_"] = dt;
        gvService.DataSource = dt;
        gvService.DataBind();
        ViewState["DuplicateService"] = 0;
        lblMessage.Text = "";
        ddlService.Text = "";
        ddlService.ClearSelection();
        ddlService.EmptyMessage = "";
        txtCharge.Text = "0.00";
        txtDrCharge.Text = "0.00";
        txtUnit.Text = "";
        //dtpfromdate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
        // dtpfromdate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
        dtpfromdate.SelectedDate = common.myDate(DateTime.Now);
        //dtpTodate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
        // dtpTodate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
        dtpTodate.SelectedDate = common.myDate(DateTime.Now);

        //  Page.SetFocus(ddlService);

    }
    private DataTable UpdateDataTable()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["OPServicesInv_"];
        foreach (GridViewRow gitem in gvService.Rows)
        {
            HiddenField hdnServiceId = (HiddenField)gitem.FindControl("hdnServiceId");
            HiddenField hdnDocReq = (HiddenField)gitem.FindControl("hdnDocReq");
            RadComboBox ddlDoctor = (RadComboBox)gitem.FindControl("ddlDoctor");

            if (common.myInt(hdnDocReq.Value) == 1 || common.myBool(hdnDocReq.Value) == true)
            {
                dt.DefaultView.RowFilter = "";
                dt.DefaultView.RowFilter = "ServiceId= " + common.myInt(hdnServiceId.Value);
                if (dt.DefaultView.Count > 0)
                {
                    dt.DefaultView[0]["DoctorID"] = common.myInt(ddlDoctor.SelectedValue);
                    dt.AcceptChanges();
                    dt.DefaultView.RowFilter = "";
                }
            }
        }
        return dt;
    }
    protected DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 0;
        dt.Columns["SNo"].AutoIncrementStep = 1;

        dt.Columns.Add("DetailsId");
        dt.Columns.Add("OTEquipmentDetailsId");
        dt.Columns.Add("ServiceId");
        dt.Columns.Add("OrderId");
        dt.Columns.Add("ServiceName");
        dt.Columns.Add("Units");
        //dt.Columns.Add("Charge");
        dt.Columns.Add("ServiceAmount");
        dt.Columns.Add("AmountPayableByPatient");
        dt.Columns.Add("AmountPayableByPayer");
        dt.Columns.Add("ServiceDiscountAmount");
        dt.Columns.Add("ServiceDiscountPercentage");
        dt.Columns.Add("DoctorAmount");
        dt.Columns.Add("FromDate");
        dt.Columns.Add("ToDate");
        dt.Columns.Add("NetAmount");
        DataRow dr = dt.NewRow();
        dr["ServiceName"] = " ";
        //dr["DeductableAmount"] = 0;
        dt.Rows.Add(dr);
        ViewState["OPServicesInv_"] = dt;
        return dt;
    }

    protected void gvService_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            GridViewRow gvRow = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            DataTable dt = (DataTable)ViewState["OPServicesInv_"];
            // DataRow[] rows = dt.Select("ServiceId <> " + common.myInt(((HiddenField)gvRow.FindControl("hdnServiceId")).Value));            
            DataView dv = dt.DefaultView;
            dv.RowFilter = "ServiceId <> " + common.myInt(((HiddenField)gvRow.FindControl("hdnServiceId")).Value);
            ViewState["OPServicesInv_"] = dv.ToTable();
            if (dv.ToTable().Rows.Count == 0)
            {
                BindGrid();
            }
            else
            {
                gvService.DataSource = dv.ToTable();
                gvService.DataBind();
            }
        }
    }
}
