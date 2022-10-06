using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using Telerik.Web.UI;
using Resources;
using System.Configuration;
using System.Collections.Generic;
using System.Web;

public partial class EMRBILLING_Popup_AddIPVisit : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();
    BaseC.Hospital baseHc;
    BaseC.EMRMasters bMstr;
    BaseC.EMRBilling.clsOrderNBill BaseBill;
    BaseC.clsEMRBilling BaseBillnew;
    Hashtable hshInput;
    Hashtable hshOutput;
    DAL.DAL objDl;
    BaseC.EMROrders objEMROrders;
    BaseC.clsLISMaster objLISMaster;
    BaseC.clsLabRequest objLabRequest;
    private const int ItemsPerRequest = 10;
    public string strxmlstring = "";
    public string Getxmlstring
    {
        get { return strxmlstring; }
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["PT"]) == "IPEMR")
        {
            this.MasterPageFile = "~/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
         baseHc = new BaseC.Hospital(sConString);
        bMstr = new BaseC.EMRMasters(sConString);
        BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        BaseBillnew = new BaseC.clsEMRBilling(sConString);
        objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            if (!IsPostBack)
            {
                if (common.myStr(Request.QueryString["PT"]) == "IPEMR"
                    && common.myStr(Session["OPIP"]) == "O")
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                Session["check"] = 0;
                //ViewState["Regid"] = common.myStr(Request.QueryString["Regid"]);
                //ViewState["RegNo"] = common.myStr(Request.QueryString["RegNo"]);
               // ViewState["EncId"] = common.myStr(Request.QueryString["EncId"]);
              //  ViewState["EncNo"] = common.myStr(Request.QueryString["EncNo"]);
              //  ViewState["OP_IP"] = common.myStr(Request.QueryString["OP_IP"]);
               // ViewState["FromWard"] = common.myStr(Request.QueryString["FromWard"]);
                //ViewState["CompanyId"] = common.myStr(Request.QueryString["CompanyId"]);
                //ViewState["InsuranceId"] = common.myStr(Request.QueryString["InsuranceId"]);
                //ViewState["CardId"] = common.myStr(Request.QueryString["CardId"]);
                //ViewState["PayerType"] = common.myStr(Request.QueryString["PayerType"]);
               // ViewState["BType"] = common.myStr(Request.QueryString["BType"]);
                //ViewState["From"] = common.myStr(Request.QueryString["From"]);

                //if (common.myStr(Request.QueryString["PT"]) == "IPEMR")
                //{
                //    ViewState["Regid"] = common.myStr(Session["RegistrationId"]);
                //    ViewState["RegNo"] = common.myStr(Session["RegistrationNo"]);
                //    ViewState["EncId"] = common.myStr(Session["EncounterId"]);
                //    ViewState["EncNo"] = common.myStr(Session["EncounterNo"]);
                //    ViewState["OP_IP"] = "I";
                //    ViewState["FromWard"] = "Y";
                //    ViewState["CompanyId"] = "0";
                //    ViewState["InsuranceId"] = "0";
                //    ViewState["CardId"] = "0";
                //    ViewState["PayerType"] = "";
                //    ViewState["BType"] = "";
                //    ViewState["From"] = "";

                //    ibtnClose.Visible = false;
                //}

                divExcludedService.Visible = false;
                
                
                //if (common.myStr(ViewState["OP_IP"]) == "O")
                //{
                //    lblPageType.Text = "OP Order";
                //    Label2.Text = PRegistration.EncounterNo;
                //    ibtnSave.Text = "Proceed";
                //    ibtnSave.ToolTip = "Click to proceed to OP Bill Page...";
                //}
                //else// I
                //{
                //    lblPageType.Text = "IP Order";
                //    //Label2.Text = PRegistration.admissiondate;
                //}
                //hdnXmlString.Value = "";
                ////HiddenField hdnxmlString = (HiddenField)PreviousPage.FindControl("hdnxmlString");
                ////Cache.Remove("OPServicesInv_" + common.myStr(Session["UserId"]) + "_" + common.myStr(txtEncId.Text));   


                //hdnCompanyId.Value = common.myStr(ViewState["CompanyId"]);
                //hdnInsuranceId.Value = common.myStr(ViewState["InsuranceId"]);
                //hdnCardId.Value = common.myStr(ViewState["CardId"]);

                //dtOrderDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                //dtOrderDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                //dtOrderDate.SelectedDate = common.myDate(DateTime.Now);
                //BindMinutes();
                //if (common.myStr(ViewState["PayerType"]) != "")
                //{
                //    ViewState["PayParty"] = common.myStr(ViewState["PayerType"]);
                //    ViewState["BillType"] = common.myStr(ViewState["BType"]);
                //}

                //if (common.myStr(ViewState["From"]) == "WARD" || common.myStr(ViewState["FromWard"]) == "Y")
                //{
                //    lnkViewLabOrders.Visible = true;
                //    lnkSampleCollection.Visible = true;
                //}
                //else
                //{
                //    lnkSampleCollection.Visible = false;
                //    lnkViewLabOrders.Visible = false;
                //}

                //BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                //hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces",
                //common.myInt(Session["HospitalLocationId"]),
                //common.myInt(Session["FacilityID"])));
                //if (common.myInt(hdnDecimalPlaces.Value) == 0)
                //{
                //    hdnDecimalPlaces.Value = "2";
                //}
                dvConfirmPrintingOptions.Visible = false;

                //BindPatientHiddenDetails(common.myStr(ViewState["RegNo"]));

                BindComboBox();
                

                //if (common.myStr(ViewState["OP_IP"]) == "I")
                //{
                //    tdAdviserDoctor.Visible = true;
                //    objBill = new BaseC.clsEMRBilling(sConString);

                //    int iAdviserDoctor = 0;
                //    try
                //    {
                //        iAdviserDoctor = objBill.GetPatientConsultingDoctor(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                //        common.myInt(ViewState["Regid"]), common.myInt(ViewState["EncId"]));
                //    }
                //    catch
                //    {
                //    }

                //    BindAdviserDoctor();
                //    ddlAdvisingDoctor.SelectedValue = iAdviserDoctor.ToString();
                //}
                //else
                //{
                //    tdAdviserDoctor.Visible = false;
                //}
                BindGrid();
                ddlService_OnItemsRequested(null,null);
                //if (common.myStr(ViewState["OP_IP"]) == "O")
                //{
                //    dtOrderDate.Enabled = false;
                //    ddlOrderMinutes.Enabled = false;
                //}
                //else
                //{
                //    dtOrderDate.Enabled = true;
                //    ddlOrderMinutes.Enabled = true;
                //}
             //   binddata();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
           // lblMessage.Text = "Error: " + Ex.Message;
          //  objException.HandleException(Ex);
        }

    }
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), false);
    }
    protected void btnSearchByUHID_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (txtRegistrationNo.Text.Length > 0)
            {
                lblMessage.Text = "";
                BindPatientHiddenDetails(common.myInt(txtRegistrationNo.Text));
                           
              
            }
           


        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindAdviserDoctor()
    {
        BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
        DataTable tbl = objlis.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityId"]), 0);
        ddlAdvisingDoctor.DataSource = tbl;
        ddlAdvisingDoctor.DataTextField = "DoctorName";
        ddlAdvisingDoctor.DataValueField = "DoctorId";
        ddlAdvisingDoctor.DataBind();

    }
    private void BindMinutes()
    {
        try
        {
            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                {
                    ddlOrderMinutes.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    ddlOrderMinutes.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
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
    protected void ddlOrderMinutes_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(dtOrderDate.SelectedDate.Value.ToString());
        sb.Remove(dtOrderDate.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(dtOrderDate.SelectedDate.Value.ToString().IndexOf(":") + 1, ddlOrderMinutes.Text);
        dtOrderDate.SelectedDate = Convert.ToDateTime(sb.ToString());
    }
    protected void lnkViewLabOrders_OnClick(object sender, EventArgs e)
    {
        //Server.Transfer("/EMRBilling/Popup/Servicedetails.aspx?Deptid=0&EncId=" + ViewState["EncId"] + "&RegNo=" + ViewState["RegNo"] + "&BillId=0&PType=WD");

        RadWindow1.NavigateUrl = "/EMRBilling/Popup/Servicedetails.aspx?Deptid=0&EncId=" + ViewState["EncId"] + "&RegNo=" + ViewState["RegNo"] + "&BillId=0&PType=WD";
        //RadWindow1.Height = 520;
        //RadWindow1.Width = 650;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        //RadWindow1.OnClientClose = "wndAddService_OnClientClose";//
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void lnkSampleCollect_OnClick(object sender, EventArgs e)
    {


        RadWindow1.NavigateUrl = "/LIS/Phlebotomy/Phlebotomy.aspx?Deptid=0&EncId=" + ViewState["EncId"] + "&RegNo=" + ViewState["RegNo"] + "&BillId=0&PType=WD&IpNo= " + lblEncounterNo.Text + "";
        RadWindow1.Height = 520;
        RadWindow1.Width = 650;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        //RadWindow1.OnClientClose = "wndAddService_OnClientClose";//
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void samplecollect(object sender, EventArgs e)
    {

    }
    #region  PageFunctions
    protected void BindComboBox()
    {
        try
        {
            
            bMstr = new BaseC.EMRMasters(sConString);
            string strDepartmentType = "";
            if (common.myStr(ViewState["OP_IP"]) == "O")
            {
                strDepartmentType = "'I','P','HPP','O','OPP','IS','RF'";
            }
            else
            {
                strDepartmentType = "'I','P','HPP','O','OPP','IS'";
            }
            DataSet ds = bMstr.GetHospitalDepartment(common.myInt(Session["HospitalLocationID"].ToString()), strDepartmentType);
            


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    

    
    protected void ddlService_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

        //if (e.Text != "")
        //{
        //    if (e.Text.Trim().Length > 2)
        //    {
                DataTable data = GetData(e.Text);

                int itemOffset = e.NumberOfItems;
                int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
                e.EndOfItems = endOffset == data.Rows.Count;
                ddlService.Items.Clear();

                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)data.Rows[i]["ServiceName"];
                    item.Value = data.Rows[i]["ServiceId"].ToString() + "##" + data.Rows[i]["ServiceType"].ToString();
                    //hdServiceType.Value = ;
                    item.Attributes.Add("ServiceName", data.Rows[i]["ServiceName"].ToString());
                    item.Attributes.Add("RefServiceCode", data.Rows[i]["RefServiceCode"].ToString());
                    this.ddlService.Items.Add(item);
                    item.DataBind();
                }
                e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        //    }
        //}
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    private DataTable GetData(string text)
    {
        string ServiceName = text + "%";
        string strDepartmentType = "";
        if (common.myStr(ViewState["OP_IP"]) == "O")
        {
            strDepartmentType = "'I','IS','P','HPP','C','O','OPP','CL','RF'";
        }
        else
        {
            strDepartmentType = "'VF','VS'";
        }
        bMstr = new BaseC.EMRMasters(sConString);
        objEMROrders = new BaseC.EMROrders(sConString);
        BaseC.RestFulAPI objCommonService = new BaseC.RestFulAPI(sConString);
        DataSet ds = objCommonService.GetHospitalServices(common.myInt(Session["HospitallocationId"]),
            0,
            0,
            strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]),0,0);
        DataTable data = new DataTable();
        data = ds.Tables[0];
        return data;
    }
    private void FillDropDownList(CommandType CT, string strcommandname, RadComboBox RCBControl, string strDataTextField, string strDataValueField, bool blIsListItem, string strListItemText, Hashtable hshIn)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dsObj = null;
            if (hshIn.Count > 0)
            {
                switch (CT)
                {
                    case CommandType.StoredProcedure:
                        dsObj = dl.FillDataSet(CommandType.StoredProcedure, strcommandname, hshIn);
                        break;
                    case CommandType.Text:
                        dsObj = dl.FillDataSet(CommandType.Text, strcommandname, hshIn);
                        break;
                }
            }
            else
            {
                switch (CT)
                {
                    case CommandType.StoredProcedure:
                        dsObj = dl.FillDataSet(CommandType.StoredProcedure, strcommandname);
                        break;
                    case CommandType.Text:
                        dsObj = dl.FillDataSet(CommandType.Text, strcommandname);
                        break;
                }
            }

            RCBControl.DataSource = dsObj;
            RCBControl.DataTextField = strDataTextField;
            RCBControl.DataValueField = strDataValueField;
            RCBControl.DataBind();
            if (blIsListItem == true)
            {
                RCBControl.Items.Insert(0, new RadComboBoxItem(strListItemText, "0"));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
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
            objException.HandleException(ex);
        }
    }
    protected DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 0;
        dt.Columns["SNo"].AutoIncrementStep = 1;

        dt.Columns.Add("DetailId");
        dt.Columns.Add("ServiceId");
        dt.Columns.Add("OrderId");
        dt.Columns.Add("UnderPackage");
        dt.Columns.Add("PackageId");
        dt.Columns.Add("ServiceType");
        dt.Columns.Add("DoctorID");
        dt.Columns.Add("DoctorRequired");
        dt.Columns.Add("DepartmentId");
        dt.Columns.Add("ServiceName");
        dt.Columns.Add("Units");
        dt.Columns.Add("Charge");
        dt.Columns.Add("ServiceAmount");
        dt.Columns.Add("DoctorAmount");
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
        dt.Columns.Add("ResourceId");
        dt.Columns.Add("ServiceStatus");
        dt.Columns.Add("IsExcluded");
        dt.Columns.Add("IsApprovalReq");
        dt.Columns.Add("IsPriceEditable");
        dt.Columns.Add("NetCharge");
        dt.Columns.Add("ChargePercentage");

        dt.Columns.Add("CopayAmt");
        dt.Columns.Add("CopayPerc");
        dt.Columns.Add("IsCoPayOnNet");
        dt.Columns.Add("DeductableAmount");
        dt.Columns.Add("VisitDatetime");
        DataRow dr = dt.NewRow();
        dr["ServiceName"] = " ";
        dr["DeductableAmount"] = 0;
        dt.Rows.Add(dr);

        ViewState["OPServicesInv_"] = dt;
        return dt;
    }
    #endregion
    protected void cmdAddtoGrid_OnClick(object sender, EventArgs e)
    {
        UpdateDataTable();
        if (common.myStr(ddlService.SelectedValue) == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Service does not exist, Please select from list !";
            Alert.ShowAjaxMsg("Service does not exist, Please select from list !", Page.Page);
            return;
        }

        string[] stringSeparators_ShowDia = new string[] { "##" };
        string[] serviceId = common.myStr(ddlService.SelectedValue).Split(stringSeparators_ShowDia, StringSplitOptions.None);

        DataSet datas1 = new DataSet();
        hshInput = new Hashtable();
        DAL.DAL dl1 = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        hshInput.Add("@encounterid", common.myInt(ViewState["EncId"]));
       hshInput.Add("@entrytime", Convert.ToString(dtOrderDate.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm")));
       // hshInput.Add("@entrytime", common.myDate(dtOrderDate.SelectedDate));
        datas1 = dl1.FillDataSet(CommandType.StoredProcedure, "CheckPatientAdmission", hshInput);

    

        if (datas1.Tables[0].Rows.Count == 0)
        {
            Alert.ShowAjaxMsg("Order Date Should be More Than Admission Date !", Page.Page);
            return;
        }
        if (common.myStr(ViewState["OP_IP"]) == "I")
        {
            if (common.myInt(ViewState["EncId"]) > 0)
            {
                DataSet datas = new DataSet();
                hshInput = new Hashtable();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshInput.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
                hshInput.Add("@FacilityId", common.myInt(Session["FacilityId"]));
                hshInput.Add("@ServiceId", common.myInt(serviceId[0]));
                hshInput.Add("@EncounterId", common.myInt(ViewState["EncId"]));
                hshInput.Add("@RegistrationId", common.myInt(ViewState["Regid"]));
                datas = dl.FillDataSet(CommandType.Text, "Select SD.ServiceId, I.ServiceName,ISNULL(EM.FirstName,'') + ' '  + ISNULL(EM.MiddleName,'') + ' ' + ISNULL(EM.LASTNAME,'') AS EnteredBy, dbo.GetDateFormatUTC(s.EncodedDate,'DT', F.TimeZoneOffSetMinutes) OrderDate FROM ServiceOrderMain S  INNER JOIN ServiceOrderDetail SD ON S.Id = SD.OrderId  INNER JOIN ItemOfService I ON SD.ServiceId = I.ServiceId  INNER JOIN FacilityMaster F ON S.FacilityId = F.FacilityID  INNER JOIN Users US ON S.EncodedBy=US.ID INNER JOIN Employee EM ON EM.ID=US.EmpID WHERE ISNULL(S.EncounterId,'') =  @EncounterId  AND S.RegistrationId = @RegistrationId  AND CONVERT(VARCHAR,S.OrderDate,111) = CONVERT(VARCHAR,GETUTCDATE(),111)  AND S.HospitalLocationId = @HospitalLocationId  AND S.FacilityId = @FacilityId  AND SD.ServiceId = @ServiceId And S.ACTIVE = 1 AND SD.ACTIVE = 1 ", hshInput);
                if (datas.Tables.Count > 0)
                {
                    if (datas.Tables[0].Rows.Count > 0)
                    {
                        lblServiceName.Text = common.myStr(datas.Tables[0].Rows[0]["ServiceName"]);
                        lblEnteredBy.Text = common.myStr(datas.Tables[0].Rows[0]["EnteredBy"]);
                        lblEnteredOn.Text = common.myStr(datas.Tables[0].Rows[0]["OrderDate"]);
                       // ViewState["DuplicateService"] = "1";
                        btnYes_OnClick(sender, e);

                    }
                    else
                    {

                        btnYes_OnClick(sender, e);
                    }
                }
            }
        }
        else
        {
            btnYes_OnClick(sender, e);
        }

    }
    protected void gvService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            foreach (TableCell tc in e.Row.Cells)
            {
                tc.Attributes["style"] = "border-color:#5EA0F4";
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ibtndaDelete = (ImageButton)e.Row.FindControl("ibtndaDelete");
            HiddenField hdnIsPackageService = (HiddenField)e.Row.FindControl("hdnIsPackageService");
            HiddenField hdnDocReq = (HiddenField)e.Row.FindControl("hdnDocReq");
            Label lblDoctorID = (Label)e.Row.FindControl("lblDoctorID");
            RadComboBox ddlDoctor = (RadComboBox)e.Row.FindControl("ddlDoctor");
            HiddenField hdnServType = (HiddenField)e.Row.FindControl("hdnlblServType");
            TextBox txtUnits = (TextBox)e.Row.FindControl("txtUnits");
            TextBox txtServiceAmount = (TextBox)e.Row.FindControl("txtServiceAmount");
            TextBox txtDoctorAmount = (TextBox)e.Row.FindControl("txtDoctorAmount");
            TextBox txtDiscountPercent = (TextBox)e.Row.FindControl("txtDiscountPercent");
            TextBox txtDiscountAmt = (TextBox)e.Row.FindControl("txtDiscountAmt");
            TextBox txtNetCharge = (TextBox)e.Row.FindControl("txtNetCharge");
            TextBox txtAmountPayableByPatient = (TextBox)e.Row.FindControl("txtAmountPayableByPatient");


            HiddenField hdnIsPriceEditable = (HiddenField)e.Row.FindControl("hdnIsPriceEditable");
            txtServiceAmount.Attributes.Add("onchange", "javascript:CalculateEditablePrice('" + txtServiceAmount.ClientID + "','" + txtUnits.ClientID + "','" + txtDiscountPercent.ClientID + "','" + txtDiscountAmt.ClientID + "','" + txtNetCharge.ClientID + "','" + txtAmountPayableByPatient.ClientID + "' );");

            txtDoctorAmount.Attributes.Add("onchange", "javascript:CalculateEditablePrice('" + txtServiceAmount.ClientID + "','" + txtUnits.ClientID + "','" + txtDiscountPercent.ClientID + "','" + txtDiscountAmt.ClientID + "','" + txtNetCharge.ClientID + "','" + txtAmountPayableByPatient.ClientID + "');");

            if (common.myInt(hdnIsPackageService.Value) == 1)
            {
                ibtndaDelete.Visible = false;
            }
            //if (common.myStr(ViewState["OP_IP"]) == "O")
            //{
                txtUnits.Enabled = false;
                if (hdnIsPriceEditable.Value == "True")
                {
                    txtServiceAmount.Enabled = true;
                }
            //}
           // if ((hdnServType.Value.Trim() == "I") || (hdnServType.Value.Trim() == "IS"))
           // {
                txtUnits.Enabled = false;
           // }
            if (common.myInt(hdnDocReq.Value) == 1 || common.myBool(hdnDocReq.Value) == true)
            {

                ddlDoctor.Visible = true;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                if ((DataTable)ViewState["EmpClassi"] == null)
                {
                    BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);

                    ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]), 0, 0);
                    DataView dvF = new DataView(ds.Tables[0]);
                    dvF.RowFilter = "Type IN ('D','TM','SR','AN','OD')";
                    ////if (common.myStr(ViewState["OP_IP"]) == "O")
                    ////{
                    ////    dvF.RowFilter = "ProvidingService IN ('O','B')";
                    ////}
                    ////if (common.myStr(ViewState["OP_IP"]) == "I")
                    ////{
                    ////    dvF.RowFilter = "ProvidingService IN ('I','B')";
                    ////}
                    dt = dvF.ToTable();
                    ViewState["EmpClassi"] = dt;
                }
                else
                    dt = (DataTable)ViewState["EmpClassi"];

                if (dt.Rows.Count > 0)
                {
                    ddlDoctor.Items.Clear();
                    ddlDoctor.DataSource = dt;
                    ddlDoctor.DataValueField = "EmployeeId";
                    ddlDoctor.DataTextField = "EmployeeName";
                    ddlDoctor.DataBind();
                    ddlDoctor.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
                    ddlDoctor.SelectedIndex = 0;
                }
                if (common.myStr(ViewState["OP_IP"]) == "O")
                {
                    if (hdnIsPriceEditable.Value == "True")
                    {
                        txtDoctorAmount.Enabled = true;
                    }
                }
                ddlDoctor.SelectedValue = common.myStr(DataBinder.Eval(e.Row.DataItem, "DoctorId")); ;
            }
            else
            {
                ddlDoctor.Visible = false;
            }

            lblDoctorID.Visible = false;
        }


        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblInfoTotal = (Label)e.Row.FindControl("lblInfoTotal");
            Label lblTotDiscountAmt = (Label)e.Row.FindControl("lblTotDiscountAmt");
            Label lblAmountPayableByPatient = (Label)e.Row.FindControl("lblAmountPayableByPatient");
            Label lblAmountPayableByPayer = (Label)e.Row.FindControl("lblAmountPayableByPayer");

            int rowcnt = 0;
            foreach (GridViewRow gvrow in gvService.Rows)
            {
                HiddenField hdnlblServType = (HiddenField)gvrow.FindControl("hdnlblServType");
                HiddenField hdispackagemain = (HiddenField)gvrow.FindControl("hdispackagemain");
                HiddenField hdUnderPackage = (HiddenField)gvrow.FindControl("hdUnderPackage");

                rowcnt = rowcnt + 1;
                if ((hdnlblServType.Value == "OPP"))
                {
                    if (hdispackagemain.Value == "1")
                    {
                        lblInfoTotal.Text = "Total: " + common.myStr(rowcnt) + " Service(s) ";
                        lblTotDiscountAmt.Text = common.FormatNumber(common.myStr(common.myDbl(lblTotDiscountAmt.Text) + common.myDbl(((TextBox)gvrow.FindControl("txtDiscountAmt")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                        lblAmountPayableByPayer.Text = common.FormatNumber(common.myStr(common.myDbl(lblAmountPayableByPayer.Text) + common.myDbl(((TextBox)gvrow.FindControl("txtAmountPayableByPayer")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                        lblAmountPayableByPatient.Text = common.FormatNumber(common.myStr(common.myDbl(lblAmountPayableByPatient.Text) + common.myDbl(((TextBox)gvrow.FindControl("txtAmountPayableByPatient")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                    }
                }
                else
                {
                    if (hdUnderPackage.Value == "0")
                    {
                        lblInfoTotal.Text = "Total: " + common.myStr(rowcnt) + " Service(s) ";
                        lblTotDiscountAmt.Text = common.FormatNumber(common.myStr(common.myDbl(lblTotDiscountAmt.Text) + common.myDbl(((TextBox)gvrow.FindControl("txtDiscountAmt")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                        lblAmountPayableByPayer.Text = common.FormatNumber(common.myStr(common.myDbl(lblAmountPayableByPayer.Text)
                            + common.myDbl(((TextBox)gvrow.FindControl("txtAmountPayableByPayer")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                        lblAmountPayableByPatient.Text = common.FormatNumber(common.myStr(common.myDbl(lblAmountPayableByPatient.Text)
                            + common.myDbl(((TextBox)gvrow.FindControl("txtAmountPayableByPatient")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                    }
                }
            }
        }
    }
    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int intId = common.myInt(e.CommandArgument);
            if (intId != 0)
            {
                UpdateDataTable();
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnIsPackageMain = (HiddenField)row.FindControl("hdnIsPackageMain");
                HiddenField hdnServiceId = (HiddenField)row.FindControl("hdnServiceId");

                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["OPServicesInv_"];

                if (common.myInt(hdnIsPackageMain.Value) == 1)
                {
                    List<DataRow> rowsToRemove = new List<DataRow>();

                    foreach (GridViewRow gvrow in gvService.Rows)
                    {
                        HiddenField hdnIsPackageService = (HiddenField)gvrow.FindControl("hdnIsPackageService");
                        HiddenField hdnPackageId = (HiddenField)gvrow.FindControl("hdnPackageId");

                        if (common.myInt(hdnIsPackageService.Value) == 1)
                        {
                            if (hdnServiceId.Value == hdnPackageId.Value)
                            {
                                rowsToRemove.Add(dt.Rows[gvrow.RowIndex]);
                            }
                        }
                    }
                    rowsToRemove.Add(dt.Rows[row.RowIndex]);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (var dr in rowsToRemove)
                        {
                            dt.Rows.Remove(dr);
                        }
                    }
                }
                else if (common.myInt(hdnIsPackageMain.Value) == 0)
                {
                    dt.Rows.RemoveAt(row.RowIndex);
                }


                dt.AcceptChanges();
                ViewState["OPServices"] = dt;
                //ViewState["Servicetable"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gvService.DataSource = dt;
                }
                else
                {
                    gvService.DataSource = CreateTable();
                }

                gvService.DataBind();
                //ddlAuthorizedBy_OnSelectedIndexChanged(this, null);
            }

        }

    }
    protected void ibtSave_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["OPServicesInv_"] == null)
            {
                return;
            }

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["OPServicesInv_"];
            if (ddlAdvisingDoctor.SelectedValue == "" && common.myStr(ViewState["OP_IP"]) != "O")
            {
                Alert.ShowAjaxMsg("Please select Advising Doctor", Page);
                return;
            }

            foreach (GridViewRow grow in gvService.Rows)
            {
                HiddenField hdnServiceId = (HiddenField)grow.FindControl("hdnServiceId");
                TextBox txtUnits = (TextBox)grow.FindControl("txtUnits");
                RadComboBox ddlDoctor = (RadComboBox)grow.FindControl("ddlDoctor");
                dt.DefaultView.RowFilter = "";
                dt.DefaultView.RowFilter = "ServiceId =" + common.myInt(hdnServiceId.Value);
                if (dt.DefaultView.Count > 0)
                {
                    dt.DefaultView[0]["Units"] = common.myStr(common.myDec(txtUnits.Text));
                    dt.DefaultView[0]["DoctorID"] = common.myInt(ddlDoctor.SelectedValue);
                    dt.AcceptChanges();
                    dt.DefaultView.RowFilter = "";
                    dt.AcceptChanges();
                }

            }

            if (common.myStr(ViewState["OP_IP"]) == "O")
            {
                updateAmount();
                foreach (DataRow dr in dt.Rows)
                {
                    if (common.myInt(dr["DoctorRequired"]) == 1 || common.myBool(dr["DoctorRequired"]) == true)
                    {
                        if (common.myInt(dr["DoctorID"]) == 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Please select doctor !";
                            return;
                        }
                    }
                    if (common.myStr(dr["ServiceType"]).ToUpper() == "CL" || common.myStr(dr["ServiceType"]).ToUpper() == "VF" || common.myStr(dr["ServiceType"]).ToUpper() == "VS")
                    {
                        if (common.myInt(dr["DoctorID"]) == 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Please select doctor !";
                            return;
                        }
                    }
                }
                DataSet xmlDS = new DataSet();
                xmlDS.Tables.Add(dt.Copy());
                xmlDS.Tables[0].DefaultView.RowFilter = "ServiceId > 0";
                if (xmlDS.Tables[0].DefaultView.Count > 0)
                {
                    xmlDS.Tables[0].Rows[0]["ResourceId"] = " ";


                    System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.

                    xmlDS.WriteXml(writer);
                    //put schema in string
                    string xmlSchema = writer.ToString();
                    hdnXmlString.Value = xmlSchema;
                    ViewState["OPServicesInv_"] = null;

                    BindGrid();
                }

                if (common.myStr(Request.QueryString["PT"]) != "IPEMR")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                }

                return;
            }
            else
            {
                // try
                //{
                updateAmount();
                hshInput = new Hashtable();
                hshOutput = new Hashtable();
                StringBuilder strXML = new StringBuilder();
                ArrayList coll = new ArrayList();
                dt = UpdateDataTable();
                dt = applyCoPayment(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    if (common.myInt(dr["DoctorRequired"]) == 1 || common.myBool(dr["DoctorRequired"]) == true)
                    {
                        if (common.myInt(dr["DoctorID"]) == 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Please select doctor !";
                            return;
                        }
                    }

                    if (common.myStr(dr["ServiceType"]).ToUpper() == "CL" || common.myStr(dr["ServiceType"]).ToUpper() == "VF" || common.myStr(dr["ServiceType"]).ToUpper() == "VS")
                    {
                        if (common.myInt(dr["DoctorID"]) == 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Please select doctor !";
                            return;
                        }
                    }

                    coll.Add(common.myInt(dr["ServiceId"])); //ServiceId INT,
                   // coll.Add(DBNull.Value); //VisitDate SMALLDATETIME,   common.myDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:00")
                    coll.Add(common.myDate(dr["VisitDatetime"]).ToString("yyyy-MM-dd HH:mm:00"));
                    
                    coll.Add(common.myInt(dr["Units"])); //Units TINYINT,
                    coll.Add(common.myInt(dr["DoctorID"])); //DoctorId INT, 
                    coll.Add(common.myDec(dr["ServiceAmount"])); //ServiceAmount MONEY,
                    coll.Add(common.myDec(dr["DoctorAmount"])); //DoctorAmount MONEY,  
                    coll.Add(common.myDec(dr["ServiceDiscountAmount"])); //ServiceDiscountAmount MONEY, 
                    coll.Add(common.myDec(dr["DoctorDiscountAmount"])); //DoctorDiscountAmount MONEY,
                    coll.Add(common.myDec(dr["AmountPayableByPatient"])); //AmountPayableByPatient MONEY,
                    coll.Add(common.myDec(dr["AmountPayableByPayer"])); //AmountPayableByPayer MONEY,
                    coll.Add(common.myDec(dr["ServiceDiscountPercentage"])); //ServiceDiscountPer MONEY,
                    coll.Add(common.myDec(dr["DoctorDiscountPercentage"])); //DoctorDiscountPer MONEY,
                    coll.Add(common.myInt(dr["PackageId"])); //PackageId INT,  
                    coll.Add(common.myInt(dr["OrderId"])); //OrderId INT,
                    coll.Add(common.myInt(dr["UnderPackage"])); //UnderPackage BIT,               
                    coll.Add(DBNull.Value); //ICDID VARCHAR(100), 
                    coll.Add(DBNull.Value); //ResourceID INT,
                    coll.Add(DBNull.Value); //SurgeryAmount MONEY, 
                    coll.Add(DBNull.Value); //ProviderPercent MONEY,
                    coll.Add(common.myInt(dr["SNo"])); //SeQNo INT, 
                    coll.Add(DBNull.Value); //Serviceremarks Varchar(150)
                    coll.Add(0);// DetailId 0 in case of new order
                    coll.Add(common.myInt(0));//23//Er Order
                    coll.Add(common.myStr(0));//24//pharmacyOrder
                    coll.Add(common.myDec(dr["CopayAmt"]));//CopayPerc
                    coll.Add(common.myDec(dr["DeductableAmount"]));//DeductableAmount
                    coll.Add(common.myStr(" "));//Approval code
                    strXML.Append(common.setXmlTable(ref coll));
                }
                if (strXML.ToString().Trim().Length > 1)
                {
                    string sChargeCalculationRequired = "N";
                    string stype = "O" + common.myStr(ViewState["OP_IP"]).ToUpper();
                    //string msg = BaseBill.saveOrders(common.myInt(Session["HospitalLocationID"].ToString()), common.myInt(Session["FacilityId"].ToString()), common.myInt(txtRegID.Text.ToString()), common.myInt(txtEncId.Text.ToString()), strXML.ToString(), "", common.myInt(Session["UserID"].ToString()), 0, common.myInt(ViewState["CompanyId"].ToString()), stype, common.myStr(ViewState["PayerType"].ToString()), common.myStr(ViewState["OP_IP"].ToString()), common.myInt(ViewState["InsuranceId"].ToString()), common.myInt(ViewState["CardId"].ToString()));
                    if (common.myInt(Session["check"]) == 0)
                    {
                        Session["check"] = 1;
                        Hashtable hshOut = BaseBill.saveOrders(
                            common.myInt(Session["HospitalLocationID"]),
                            common.myInt(Session["FacilityId"]),
                            common.myInt(ViewState["Regid"]),
                            common.myInt(ViewState["EncId"]),
                            strXML.ToString(), "",
                            common.myInt(Session["UserID"]),
                            common.myInt(ddlAdvisingDoctor.SelectedValue),
                            common.myInt(hdnCompanyId.Value),
                            stype,
                            common.myStr(ViewState["PayerType"]),
                            common.myStr(ViewState["OP_IP"]),
                            common.myInt(hdnInsuranceId.Value),
                            common.myInt(hdnCardId.Value),
                            Convert.ToDateTime(dtOrderDate.SelectedDate), sChargeCalculationRequired, common.myInt(Session["EntrySite"]),common.myInt(Session["ModuleId"]));

                        if (common.myStr(hshOut["chvErrorStatus"]).Length == 0)
                        {
                            if (common.myInt(hshOut["intNEncounterID"]) > 0)
                            {
                                Session["EncounterID"] = common.myInt(hshOut["intNEncounterID"]);
                            }

                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            lblMessage.Text = "Order Saved Successfully";
                            ViewState["OPServicesInv_"] = null;
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                            binddata();
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "There is some error while saving order." + hshOut["chvErrorStatus"].ToString();
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
    protected void ddladvisingdoctor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddata();

    }
    protected void binddata()
    {
        Gvdetail.DataSource = null;
        objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds2 = new DataSet();
        Hashtable hshIn = new Hashtable();
        Hashtable hshOut = new Hashtable();

        
        hshIn.Add("@docid", Convert.ToInt32(ddlAdvisingDoctor.SelectedValue));
        hshIn.Add("@encounterid", common.myInt(ViewState["EncId"]));


        ds2 = objDl.FillDataSet(CommandType.StoredProcedure, "filldoctorvisit", hshIn, hshOut);

        //if (ds2.Tables[0].Rows.Count > 0)
        //{
            Gvdetail.DataSource = ds2.Tables[0];
            Gvdetail.DataBind();

        //}
        ds2.Dispose();

    }
    protected void BindPatientHiddenDetails(int RegistrationNo)
    {
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        objLISMaster = new BaseC.clsLISMaster(sConString);
        if (RegistrationNo > 0)
        {
            int HospId = common.myInt(Session["HospitalLocationID"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int RegId = 0;
            int EncounterId = 0;
            int EncodedBy = common.myInt(Session["UserId"]);
            DataSet ds = new DataSet();
//            lblregNo.Text = RegistrationNo;
            if (common.myStr(ViewState["OP_IP"]) == "O")
            {
                ds = bC.getPatientDetails(HospId, FacilityId, RegId, RegistrationNo, EncounterId, EncodedBy);
                lblInfoEncNo.Visible = false;
                lblInfoAdmissionDt.Visible = false;
                lblEncounterNo.Visible = false;
                lblAdmissionDate.Visible = false;
            }
            else
            {
                BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);

                if (ddlSearchOn.SelectedValue == "0")
                    ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, RegistrationNo, EncodedBy, 0, "");
                else
                    ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, 0, EncodedBy, 0, RegistrationNo.ToString());
              //  ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, RegistrationNo, EncodedBy, 0, "");
            }
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataRow dr = ds.Tables[0].Rows[0];

                    if (common.myStr(dr["encounterstatuscode"]).ToString() != "O" && common.myStr(dr["encounterstatuscode"]).ToString() != "MD")
                    {
                        Alert.ShowAjaxMsg("This Patient Entry Not Allowed !", Page.Page);
                        return;
                    }

                    lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                    lblDob.Text = common.myStr(dr["DOB"]);
                    lblMobile.Text = common.myStr(dr["MobileNo"]);
                    lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                    lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                    lblMessage.Text = "";
                    ViewState["EncId"] = common.myStr(dr["EncounterId"]);

                    ViewState["Regid"] = common.myStr(dr["RegistrationId"]);
                    ViewState["RegNo"] = common.myStr(dr["RegistrationNo"]);
                    lblregNo.Text = ViewState["RegNo"].ToString();
                    ViewState["EncNo"] = common.myStr(dr["EncounterNo"]);
                    ViewState["OP_IP"] = "I";

                    ViewState["CompanyId"] = common.myStr(dr["PayorId"]);
                    ViewState["InsuranceId"] = common.myStr(dr["SponsorId"]);
                    ViewState["CardId"] = common.myStr(dr["InsuranceCardId"]);

                    ViewState["BType"] = common.myStr(ViewState["pyradPayerType"]);
                    if (common.myStr(ViewState["From"]) == "BILL")
                    {
                        hdnCompanyId.Value = common.myStr(ViewState["CompanyId"]);
                        hdnInsuranceId.Value = common.myStr(ViewState["InsuranceId"]);
                        hdnCardId.Value = common.myStr(ViewState["CardId"]);
                    }
                    else
                    {
                        hdnCompanyId.Value = common.myStr(dr["PayorId"]);
                        hdnInsuranceId.Value = common.myStr(dr["SponsorId"]);
                        hdnCardId.Value = common.myStr(dr["InsuranceCardId"]);
                    }

                    divExcludedService.Visible = false;

                    if (common.myStr(ViewState["OP_IP"]) == "O")
                    {
                        lblPageType.Text = "OP Order";
                        Label2.Text = PRegistration.EncounterNo;
                        ibtnSave.Text = "Proceed";
                        ibtnSave.ToolTip = "Click to proceed to OP Bill Page...";
                    }
                    else// I
                    {
                        lblPageType.Text = "IP Order";
                        //Label2.Text = PRegistration.admissiondate;
                    }
                    hdnXmlString.Value = "";
                    //HiddenField hdnxmlString = (HiddenField)PreviousPage.FindControl("hdnxmlString");
                    //Cache.Remove("OPServicesInv_" + common.myStr(Session["UserId"]) + "_" + common.myStr(txtEncId.Text));   


                    hdnCompanyId.Value = common.myStr(ViewState["CompanyId"]);
                    hdnInsuranceId.Value = common.myStr(ViewState["InsuranceId"]);
                    hdnCardId.Value = common.myStr(ViewState["CardId"]);

                    dtOrderDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                    dtOrderDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                    dtOrderDate.SelectedDate = common.myDate(DateTime.Now);
                    BindMinutes();
                    if (common.myStr(ViewState["PayerType"]) != "")
                    {
                        ViewState["PayParty"] = common.myStr(ViewState["PayerType"]);
                        ViewState["BillType"] = common.myStr(ViewState["BType"]);
                    }

                    

                    BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                    hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces",
                    common.myInt(Session["HospitalLocationId"]),
                    common.myInt(Session["FacilityID"])));
                    if (common.myInt(hdnDecimalPlaces.Value) == 0)
                    {
                        hdnDecimalPlaces.Value = "2";
                    }
                    dvConfirmPrintingOptions.Visible = false;

                    

                    BindComboBox();


                    if (common.myStr(ViewState["OP_IP"]) == "I")
                    {
                        tdAdviserDoctor.Visible = true;
                        objBill = new BaseC.clsEMRBilling(sConString);

                        int iAdviserDoctor = 0;
                        try
                        {
                            iAdviserDoctor = objBill.GetPatientConsultingDoctor(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                            common.myInt(ViewState["Regid"]), common.myInt(ViewState["EncId"]));
                        }
                        catch
                        {
                        }

                        BindAdviserDoctor();
                        ddlAdvisingDoctor.SelectedValue = iAdviserDoctor.ToString();
                    }
                    else
                    {
                        tdAdviserDoctor.Visible = false;
                    }
                    BindGrid();
                    if (common.myStr(ViewState["OP_IP"]) == "O")
                    {
                        dtOrderDate.Enabled = false;
                        ddlOrderMinutes.Enabled = false;
                    }
                    else
                    {
                        dtOrderDate.Enabled = true;
                        ddlOrderMinutes.Enabled = true;
                    }
                    binddata();
                    txtRegistrationNo.Enabled = false;

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
    protected void btnYes_OnClick(object sender, EventArgs e)
    {



        DataTable dt = new DataTable();
        int maxId = 0;
        string[] stringSeparators_ShowDia = new string[] { "##" };
        string[] serviceId = common.myStr(ddlService.SelectedValue).Split(stringSeparators_ShowDia, StringSplitOptions.None);


        dt = (DataTable)ViewState["OPServicesInv_"];
        if (dt.Rows.Count > 0)
        {
            if (common.myInt(dt.Rows[0][2]) == 0) //If serviceid = 0 then remove row
            {
                dt.Rows.Clear();
            }
            else
            {
                DataView dv = new DataView(dt);
                dv.Sort = "Sno Desc";
                maxId = common.myInt(dv[0]["Sno"]);
            }
            //Check duplicate service------------------------------------------------------------------
            DataView dvdup = new DataView();
            dvdup = dt.Copy().DefaultView;

            dvdup.RowFilter = " ServiceId = " + common.myStr(serviceId[0]);

            ////if (dvdup.ToTable().Rows.Count > 0)
            ////{
            ////    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            ////    lblMessage.Text = "Service already exist !";
            ////    //ShowPatientDetails(common.myStr(txtRegistrationNo.Text));
            ////    ddlService.Text = "";
            ////    ddlService.ClearSelection();
            ////    ddlService.EmptyMessage = "";
            ////    Page.SetFocus(ddlService);
            ////    return;
            ////}
        }
        if (common.myStr(serviceId[1]) == "OPP")
        {
            BaseC.RestFulAPI objBilling = new BaseC.RestFulAPI(sConString);
            DataSet ds = objBilling.getPackageServiceDetails(common.myInt(serviceId[0]), common.myInt(Session["HospitalLocationID"]),
                common.myInt(Session["FacilityId"]), common.myInt(hdnCompanyId.Value),
                common.myInt(hdnInsuranceId.Value), common.myInt(hdnCardId.Value),
                common.myStr(ViewState["OP_IP"]), common.myInt(ViewState["Regid"]), common.myInt(ViewState["EncId"]), 0);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!ds.Tables[0].Columns.Contains("DeductableAmount"))
                    {
                        ds.Tables[0].Columns.Add("DeductableAmount", typeof(double));
                    }
                    if (!ds.Tables[0].Columns.Contains("IsCoPayOnNet"))
                    {
                        ds.Tables[0].Columns.Add("IsCoPayOnNet", typeof(double));
                    }
                    if (!ds.Tables[0].Columns.Contains("CopayPerc"))
                    {
                        ds.Tables[0].Columns.Add("CopayPerc", typeof(double));
                    }
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["DeductableAmount"] = 0;
                        dr["IsCoPayOnNet"] = 0;
                        dr["CopayPerc"] = 0;
                        dt.ImportRow(dr);
                    }

                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    if (dr.IsNull("DeductableAmount"))
                    //    {
                    //        dr["DeductableAmount"] = 0;
                    //    }
                    //}                                     


                    ViewState["OPServicesInv_"] = dt;
                    gvService.DataSource = dt;
                    gvService.DataBind();
                    ddlService.Text = "";
                    ddlService.ClearSelection();
                    ddlService.EmptyMessage = "";
                    Page.SetFocus(ddlService);
                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Invalid Package Amount !";
            }

            //dt.mer = ds.Tables[0];
        }
        else
        {
            ViewState["ServiceId"] = common.myInt(serviceId[0]);
            ViewState["ServiceName"] = common.myStr(ddlService.Text);
            //----------------------------------------------------------------------------------------------
            Hashtable hshServiceDetail = new Hashtable();
            hshServiceDetail = BaseBillnew.getSingleServiceAmount_WithDate(common.myInt(Session["HospitalLocationID"]),
                common.myInt(Session["FacilityId"]),
                common.myInt(hdnCompanyId.Value),
                common.myInt(hdnInsuranceId.Value),
                common.myInt(hdnCardId.Value),
                common.myStr(ViewState["OP_IP"]),
                common.myInt(serviceId[0]),
                common.myInt(ViewState["Regid"]),
                common.myInt(ViewState["EncId"]), 0, 0, 0,common.myDate( dtOrderDate.SelectedDate));
            if ((common.myStr(ViewState["OP_IP"]) == "I") && (common.myStr(hshServiceDetail["IsExcluded"]) == "True") || (common.myStr(hshServiceDetail["IsExcluded"]) == "1"))
            {
                divExcludedService.Visible = true;
                lblExcludedServiceName.Text = common.myStr(ViewState["ServiceName"]);

            }
            else if (common.myInt(ViewState["DuplicateService"]) == 1)
            {
                dvConfirmPrintingOptions.Visible = true;
            }
            else
            {
                addService();
                dvConfirmPrintingOptions.Visible = false;
                divExcludedService.Visible = false;

            }
        }


    }
    protected void btnExcludedService_OnClick(object sender, EventArgs e)
    {
        divExcludedService.Visible = false;
        DataTable dt = new DataTable();
        int maxId = 0;
        //string[] stringSeparators_ShowDia = new string[] { "##" };
        //string[] serviceId = common.myStr(ViewState["ServiceId"]).Split(stringSeparators_ShowDia, StringSplitOptions.None);

        dt = (DataTable)ViewState["OPServicesInv_"];
        if (common.myInt(ViewState["DuplicateService"]) == 1)
        {
            dvConfirmPrintingOptions.Visible = true;

        }
        else
        {
            addService();
            divExcludedService.Visible = false;
        }
    }
    protected void btnAlredyExist_OnClick(object sender, EventArgs e)
    {
        addService();
    }
    public void addService()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["OPServicesInv_"];
        int maxId = 0;
        if (dt.Rows.Count > 0)
        {
            if (common.myInt(dt.Rows[0][2]) == 0) //If serviceid = 0 then remove row
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
        //string[] stringSeparators_ShowDia = new string[] { "##" };
        //string[] serviceId = common.myStr(ViewState["ServiceId"]).Split(stringSeparators_ShowDia, StringSplitOptions.None);


        DataRow dr = dt.NewRow();
        Hashtable hshServiceDetail = new Hashtable();
        hshServiceDetail = BaseBillnew.getSingleServiceAmount_WithDate(common.myInt(Session["HospitalLocationID"]),
            common.myInt(Session["FacilityId"]), common.myInt(hdnCompanyId.Value), common.myInt(hdnInsuranceId.Value),
            common.myInt(hdnCardId.Value), common.myStr(ViewState["OP_IP"]),
            common.myInt(ViewState["ServiceId"]),
            common.myInt(ViewState["Regid"]), common.myInt(ViewState["EncId"]), 0, 0, 0,common.myDate(dtOrderDate.SelectedDate));

        dr["Sno"] = maxId + 1;
        dr["ServiceId"] = common.myInt(ViewState["ServiceId"]);
        dr["UnderPackage"] = common.myInt(0);
        dr["PackageId"] = common.myInt(0);
        //dr["DoctorID"] = common.myInt(0);
        dr["DoctorID"] = common.myInt(ddlAdvisingDoctor.SelectedValue);
        dr["VisitDatetime"] = Convert.ToString(dtOrderDate.SelectedDate.Value.ToString("dd/MM/yyyy HH:mm"));
        

        if (common.myStr(hshServiceDetail["ServiceType"]).ToUpper() == "CL" || common.myStr(hshServiceDetail["ServiceType"]).ToUpper() == "VF" || common.myStr(hshServiceDetail["ServiceType"]).ToUpper() == "VS")
            dr["DoctorRequired"] = "True";
        else
            dr["DoctorRequired"] = common.myStr(hshServiceDetail["DoctorRequired"]);

        dr["DepartmentId"] = common.myInt(hshServiceDetail["DepartmentId"]);
        dr["ServiceType"] = common.myStr(hshServiceDetail["ServiceType"]);
        dr["ServiceName"] = common.myStr(ViewState["ServiceName"]);
        dr["Units"] = common.myInt(1);
        dr["Charge"] = common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

        dr["ServiceAmount"] = common.myDec(hshServiceDetail["NChr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        dr["DoctorAmount"] = common.myDec(hshServiceDetail["DNchr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

        dr["ServiceDiscountPercentage"] = common.myDec(hshServiceDetail["DiscountPerc"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        dr["ServiceDiscountAmount"] = common.myDec(hshServiceDetail["DiscountNAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        dr["DoctorDiscountPercentage"] = common.myInt(0);
        dr["DoctorDiscountAmount"] = common.myDec(hshServiceDetail["DiscountDNAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        dr["TotalDiscount"] = common.myDec(common.myDec(hshServiceDetail["DiscountNAmt"]) + common.myDec(hshServiceDetail["DiscountDNAmt"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        dr["AmountPayableByPatient"] = common.myDec(hshServiceDetail["PatientNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        dr["AmountPayableByPayer"] = common.myDec(hshServiceDetail["PayorNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
        dr["IsPackageMain"] = common.myInt(0);
        dr["IsPackageService"] = common.myInt(0);
        dr["ResourceId"] = "  ";
        dr["MainSurgeryId"] = common.myInt(0);
        dr["IsSurgeryMain"] = common.myInt(0);
        dr["IsSurgeryService"] = common.myInt(0);
        dr["ServiceRemarks"] = "";

        dr["ServiceStatus"] = "  ";
        dr["IsExcluded"] = "  ";
        dr["IsPriceEditable"] = common.myStr(hshServiceDetail["IsPriceEditable"]);
        dr["IsApprovalReq"] = common.myStr(hshServiceDetail["IsApproval"]);
        decimal totPayable = common.myDec(hshServiceDetail["PatientNPayable"]) + common.myDec(hshServiceDetail["PayorNPayable"]);

        dr["NetCharge"] = common.myDec(totPayable.ToString("F" + common.myInt(hdnDecimalPlaces.Value)));
        dr["ChargePercentage"] = 0;
        dr["CopayAmt"] = common.myDbl(hshServiceDetail["insCoPayAmt"]);
        dr["CopayPerc"] = common.myDbl(hshServiceDetail["insCoPayPerc"]);
        dr["IsCoPayOnNet"] = common.myDbl(hshServiceDetail["IsCoPayOnNet"]);
        dr["DeductableAmount"] = common.myDbl(hshServiceDetail["mnDeductibleAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));


        dt.Rows.Add(dr);



        ViewState["OPServicesInv_"] = dt;
        gvService.DataSource = dt;
        gvService.DataBind();
        ViewState["DuplicateService"] = 0;
        lblMessage.Text = "";
        dvConfirmPrintingOptions.Visible = false;
        divExcludedService.Visible = false;
        ddlService.Text = "";
        ddlService.ClearSelection();
        ddlService.EmptyMessage = "";
        Page.SetFocus(ddlService);
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmPrintingOptions.Visible = false;
        ViewState["DuplicateService"] = 0;
    }
    protected void btnExcludedServiceCancel_OnClick(object sender, EventArgs e)
    {
        divExcludedService.Visible = false;

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
    public void updateAmount()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["OPServicesInv_"];
        foreach (GridViewRow gitem in gvService.Rows)
        {
            HiddenField hdnServiceId = (HiddenField)gitem.FindControl("hdnServiceId");
            HiddenField hdnDocReq = (HiddenField)gitem.FindControl("hdnDocReq");
            RadComboBox ddlDoctor = (RadComboBox)gitem.FindControl("ddlDoctor");
            TextBox txtUnits = (TextBox)gitem.FindControl("txtUnits");
            TextBox txtServiceAmount = (TextBox)gitem.FindControl("txtServiceAmount");
            TextBox txtDoctorAmount = (TextBox)gitem.FindControl("txtDoctorAmount");
            TextBox txtDiscountPercent = (TextBox)gitem.FindControl("txtDiscountPercent");
            TextBox txtDiscountAmt = (TextBox)gitem.FindControl("txtDiscountAmt");
            TextBox txtNetCharge = (TextBox)gitem.FindControl("txtNetCharge");
            TextBox txtAmountPayableByPatient = (TextBox)gitem.FindControl("txtAmountPayableByPatient");
            TextBox txtAmountPayableByPayer = (TextBox)gitem.FindControl("txtAmountPayableByPayer");
            dt.DefaultView.RowFilter = "";
            dt.DefaultView.RowFilter = "ServiceId= " + common.myInt(hdnServiceId.Value);
            if (dt.DefaultView.Count > 0)
            {
                dt.DefaultView[0]["Charge"] = common.myDec(txtServiceAmount.Text) + common.myDec(txtDoctorAmount.Text);
                dt.DefaultView[0]["ServiceAmount"] = common.myDec(txtServiceAmount.Text);
                dt.DefaultView[0]["DoctorAmount"] = common.myDec(txtDoctorAmount.Text);
                dt.DefaultView[0]["NetCharge"] = common.myDec(txtNetCharge.Text);
                dt.DefaultView[0]["TotalDiscount"] = common.myDec(txtDiscountAmt.Text);
                if (common.myInt(ViewState["BillType"]) == 0)
                {
                    dt.DefaultView[0]["AmountPayableByPatient"] = common.myDec(txtAmountPayableByPatient.Text);
                }
                else
                {
                    dt.DefaultView[0]["AmountPayableByPayer"] = common.myDec(txtAmountPayableByPayer.Text);
                }
                if (common.myInt(hdnDocReq.Value) == 1 || common.myBool(hdnDocReq.Value) == true)
                {
                    dt.DefaultView[0]["DoctorID"] = common.myInt(ddlDoctor.SelectedValue);
                }
                dt.AcceptChanges();
                dt.DefaultView.RowFilter = "";
            }

        }
        ViewState["OPServicesInv_"] = dt;
        gvService.DataSource = dt;
        gvService.DataBind();

    }
    protected void btnGetBalance_OnClick(object sender, EventArgs e)
    {
        updateAmount();
    }
    public DataTable applyCoPayment(DataTable dt)
    {


        decimal CopayPercentage = 0, CopayAmt = 0, DeductableAmt = 0, DeductablePerc = 0, TotalNetCharge = 0;
        decimal totalAmount = 0;
        foreach (DataRow dr in dt.Rows)
        {
            if (!dt.Columns.Contains("IsCoPayOnNet"))
            {
                dt.Columns.Add("CopayPerc");
            }
            if (!dt.Columns.Contains("CopayPerc"))
            {
                dt.Columns.Add("IsCoPayOnNet");
            }
            //CopayPerc
            if (common.myInt(dr["IsCoPayOnNet"]) == 0)//Gross
            {
                if (common.myInt(dr["CopayPerc"]) > 0)
                {
                    totalAmount = totalAmount + common.myDec(dr["Charge"]);
                }
            }
            else
            {
                if (common.myInt(dr["CopayPerc"]) > 0)
                {
                    totalAmount = totalAmount + common.myDec(common.myDec(dr["AmountPayableByPayer"]) + common.myDec(dr["AmountPayableByPatient"]));
                }
            }
            if (common.myInt(dr["DeductableAmount"]) > 0)
            {
                TotalNetCharge = TotalNetCharge + common.myDec(common.myDec(dr["AmountPayableByPayer"]) + common.myDec(dr["AmountPayableByPatient"]));
            }
        }

        foreach (DataRow dr1 in dt.Rows)
        {

            decimal TotalAmount = 0;
            if ((common.myInt(dr1["DeductableAmount"]) > 0))
            {
                if (common.myInt(dr1["IsCoPayOnNet"]) == 0)//Gross
                {
                    if (common.myInt(dr1["DeductableAmount"]) > 0)
                    {
                        TotalAmount = common.myDec(dr1["Charge"]);
                    }
                }
                else//net
                {
                    if (common.myInt(dr1["DeductableAmount"]) > 0)
                    {
                        TotalAmount = common.myDec(common.myDec(dr1["AmountPayableByPayer"]) + common.myDec(dr1["AmountPayableByPatient"]));
                    }
                }
                CopayPercentage = common.myDec(dr1["CopayPerc"]);
                CopayAmt = CopayPercentage / 100 * (TotalAmount * common.myDec(dr1["Units"]));
                DeductableAmt = common.myDec(dr1["DeductableAmount"]);
                //if (MaxcoPayamt > 0)
                //{
                //    CopayPercentage = common.myDec(MaxcoPayamt) * 100 / totalAmount;
                //    CopayAmt = (CopayPercentage / 100 * (TotalAmount * common.myDec(dr1["Units"])));
                //}
                //if (MaxDecutableAmt > 0)
                //{
                //    DeductablePerc = common.myDec(MaxDecutableAmt) * 100 / TotalNetCharge;
                //    DeductableAmt = (DeductablePerc / 100 * (TotalAmount * common.myDec(dr1["Units"])));
                //}

                decimal AmountPayableByPatient = 0, AmountPayableByPayer = 0;
                if (common.myInt(dr1["IsCoPayOnNet"]) == 0)//Gross
                {
                    AmountPayableByPatient = CopayAmt + DeductableAmt;
                    AmountPayableByPayer = TotalAmount - DeductableAmt - common.myDec(CopayAmt) - common.myDec(dr1["ServiceDiscountAmount"]);
                }
                else
                {
                    AmountPayableByPatient = CopayAmt + DeductableAmt;
                    AmountPayableByPayer = TotalAmount - common.myDec(CopayAmt) - DeductableAmt;
                }
                dr1["CopayAmt"] = common.myDbl(CopayAmt).ToString("F" + hdnDecimalPlaces.Value);
                dr1["CopayPerc"] = common.myDbl(CopayPercentage).ToString("F" + hdnDecimalPlaces.Value);
                dr1["DeductableAmount"] = common.myDbl(DeductableAmt).ToString("F" + hdnDecimalPlaces.Value);
                dr1["AmountPayableByPatient"] = common.myDbl(AmountPayableByPatient).ToString("F" + hdnDecimalPlaces.Value);
                dr1["AmountPayableByPayer"] = common.myDbl(AmountPayableByPayer).ToString("F" + hdnDecimalPlaces.Value);
                dt.AcceptChanges();
            }

            if ((common.myInt(dr1["CopayPerc"]) > 0))
            {
                if (common.myInt(dr1["IsCoPayOnNet"]) == 0)//Gross
                {
                    if (common.myInt(dr1["CopayPerc"]) > 0)
                    {
                        TotalAmount = common.myDec(dr1["Charge"]);
                    }
                }
                else//net
                {
                    if (common.myInt(dr1["CopayPerc"]) > 0)
                    {
                        TotalAmount = common.myDec(common.myDec(dr1["AmountPayableByPayer"]) + common.myDec(dr1["AmountPayableByPatient"]));
                    }
                }
                CopayPercentage = common.myDec(dr1["CopayPerc"]);
                CopayAmt = CopayPercentage / 100 * (TotalAmount * common.myDec(dr1["Units"]));
                DeductableAmt = common.myDec(dr1["DeductableAmount"]);
                

                decimal AmountPayableByPatient = 0, AmountPayableByPayer = 0;
                if (common.myInt(dr1["IsCoPayOnNet"]) == 0)//Gross
                {
                    AmountPayableByPatient = CopayAmt + DeductableAmt;
                    AmountPayableByPayer = TotalAmount - DeductableAmt - common.myDec(CopayAmt) - common.myDec(dr1["ServiceDiscountAmount"]);
                }
                else
                {
                    AmountPayableByPatient = CopayAmt + DeductableAmt;
                    AmountPayableByPayer = TotalAmount - common.myDec(CopayAmt) - DeductableAmt;
                }
                dr1["CopayAmt"] = common.myDbl(CopayAmt).ToString("F" + hdnDecimalPlaces.Value);
                dr1["CopayPerc"] = common.myDbl(CopayPercentage).ToString("F" + hdnDecimalPlaces.Value);
                dr1["DeductableAmount"] = common.myDbl(DeductableAmt).ToString("F" + hdnDecimalPlaces.Value);
                dr1["AmountPayableByPatient"] = common.myDbl(AmountPayableByPatient).ToString("F" + hdnDecimalPlaces.Value);
                dr1["AmountPayableByPayer"] = common.myDbl(AmountPayableByPayer).ToString("F" + hdnDecimalPlaces.Value);
                dt.AcceptChanges();


            }
        }

        return dt;

    }

   
}
