using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Drawing;

using BaseC;
using System.Web.Script.Services;
using System.Web.Services;

public partial class EMR_Orders_Order : System.Web.UI.Page
{
    // DL_Funs fun = new DL_Funs();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    Hashtable hshIn = null;
    BaseC.EMRBilling.clsOrderNBill BaseBill;
    BaseC.EMROrders order;
    static string str_FavServicID;

    private Hashtable hstInput;
    string Saved_RTF_Content;
    StringBuilder sb = new StringBuilder();
    string Fonts = "";
    static string gBegin = "<u>";
    static string gEnd = "</u>";
    StringBuilder objStrTmp = new StringBuilder();
    private int iPrevId = 0;
    string sFontSize = "";
    string path = string.Empty;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            //Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }
    #region  Private Method
    private void ClearForm()
    {

        if (common.myStr(ViewState["ClearcmbServiceName"]) != "N0")
        {
            cmbServiceName.Text = "";
            hdnServiceId.Value = "";
        }
        txtInstruction.Text = "";

        hdnLongDescription.Value = "";
        hdnID.Value = "";
        //hdnServiceId.Value = "";
        ddlbImgCntr.SelectedIndex = 0;
        ddlOrg.SelectedIndex = 0;
        txtModifier.Text = "";

        foreach (ListItem item in chkStat.Items)
        {
            item.Selected = false;
        }
        //Added By Abhishek Goel
        RadDateTimePicker1.SelectedDate = null;
        //Added By Abhishek Goel
    }
    private void getCurrentICDCodes()
    {
        try
        {
            DataSet ds = new DataSet();

            order = new BaseC.EMROrders(sConString);
            ds = order.GetPatientDiagnosis(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt16(Session["HospitalLocationID"]),
               Convert.ToInt32(Session["EncounterId"]));
            String sICDCodes = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (sICDCodes == "")
                    {
                        sICDCodes += ds.Tables[0].Rows[i]["ICDCode"].ToString();
                    }
                    else
                    {
                        sICDCodes += "," + ds.Tables[0].Rows[i]["ICDCode"].ToString();
                    }
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ICDCodes"] = sICDCodes;
            }
            else
            {
                ViewState["ICDCodes"] = null;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected DataTable CreateTable()
    {
        DataTable Dt = new DataTable();
        DataColumn dc = new DataColumn("ID", typeof(int));
        dc.DefaultValue = 0;
        Dt.Columns.Add(dc);

        Dt.Columns.Add("AlertRequired", typeof(bool));
        Dt.Columns.Add("AlertMessage", typeof(string));
        Dt.Columns.Add("ServiceName", typeof(string));
        Dt.Columns.Add("ServiceId", typeof(int));
        Dt.Columns.Add("ICDID", typeof(string));
        Dt.Columns.Add("DoctorId", typeof(string));
        Dt.Columns.Add("FacilityId", typeof(string));
        Dt.Columns.Add("ServiceType", typeof(string));
        Dt.Columns.Add("Remarks", typeof(string));
        Dt.Columns.Add("Stat", typeof(bool));
        Dt.Columns.Add("Urgent", typeof(bool));
        Dt.Columns.Add("LabStatus", typeof(string));
        Dt.Columns.Add("OrderId", typeof(int));
        Dt.Columns.Add("EncodedBy", typeof(int));
        Dt.Columns.Add("IsExcluded", typeof(bool));
        Dt.Columns.Add("CompanyId", typeof(int));
        Dt.Columns.Add("PlanTypeId", typeof(int));
        Dt.Columns.Add("PackageId", typeof(int));
        Dt.Columns.Add("RequestToDepartment", typeof(bool));
        Dt.Columns.Add("Charges", typeof(double));
        Dt.Columns.Add("result", typeof(int));
        Dt.Columns.Add("CPTCode", typeof(string));
        Dt.Columns.Add("Units", typeof(int));
        Dt.Columns.Add("TestDate", typeof(DateTime));
        Dt.Columns.Add("isServiceRemarkMandatory", typeof(bool));
        Dt.Columns.Add("FreeTest", typeof(bool));
        Dt.Columns.Add("DoctorRequired", typeof(bool));
        Dt.Columns.Add("IsPriceEditableFromEMR", typeof(bool));
        Dt.Columns.Add("ServiceDurationId", typeof(int));
        Dt.Columns.Add("IsBioHazard", typeof(bool));
        Dt.Columns.Add("StationId", typeof(int));
        Dt.Columns.Add("AssignToEmpId", typeof(int));
        Dt.Columns.Add("Providerid", typeof(int));
        return Dt;
    }
    #endregion

    #region All Data Bind Private Method
    private void BindICDPanel()
    {
        try
        {
            DataSet dsTemp = new DataSet();
            if (ViewState["ICDCodes"] != null)
            {
                if (txtICDCode.Text.ToString().Trim().Length == 0)
                {
                    if (hdnExitOrNot.Value == "0")
                    {
                        hdnICDCode.Value = Convert.ToString(ViewState["ICDCod"]);
                        txtICDCode.Text = Convert.ToString(ViewState["ICDCod"]);
                    }
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("ICDCodes");
                dt.Columns.Add("Description");
                dt.Columns["ID"].AutoIncrement = true;
                dt.Columns["ID"].AutoIncrementSeed = 1;
                dt.Columns["ID"].AutoIncrementStep = 1;

                char[] chArray = { ',' };
                string[] serviceIdXml = ViewState["ICDCodes"].ToString().Split(chArray);
                DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                foreach (string item in serviceIdXml)
                {
                    DataRow drdt = dt.NewRow();
                    order = new BaseC.EMROrders(sConString);
                    dsTemp = order.GetICDCode(item.ToString());
                    if (dsTemp.Tables.Count > 0)
                    {
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            drdt["ICDCodes"] = item.ToString();
                            drdt["Description"] = dsTemp.Tables[0].Rows[0]["Description"].ToString();
                            dt.Rows.Add(drdt);
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    txtICDCode.Text = dt.Rows[0]["ICDCodes"].ToString();
                }
            }
            else
            {
                txtICDCode.ReadOnly = true;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindBlnkGrid()
    {
        try
        {
            DataTable dT = CreateTable();
            DataRow dr;
            dr = dT.NewRow();
            dr[1] = DBNull.Value;
            dr[2] = DBNull.Value;
            dr[3] = DBNull.Value;
            dr[4] = DBNull.Value;
            dr[5] = DBNull.Value;
            dr[6] = DBNull.Value;
            dr[7] = DBNull.Value;
            dr[8] = DBNull.Value;
            dr[9] = DBNull.Value;
            dr[10] = DBNull.Value;
            dr[11] = DBNull.Value;
            dr[12] = DBNull.Value;
            dr[13] = DBNull.Value;
            dr[14] = DBNull.Value;
            dr[15] = DBNull.Value;
            dr[16] = DBNull.Value;
            dr[17] = DBNull.Value;
            dr[18] = DBNull.Value;
            dr[19] = DBNull.Value;
            dr[20] = DBNull.Value;
            dr[21] = DBNull.Value;
            dr[22] = DBNull.Value;
            dr[23] = DBNull.Value;
            dT.Rows.Add(dr);
            gvPatientServiceDetail.DataSource = dT;
            gvPatientServiceDetail.DataBind();


            gvPatientServiceDetail.AutoGenerateEditButton = false;

            //foreach (GridViewRow gr in gvPatientServiceDetail.Rows)
            //{
            //    ImageButton ibtnDelete1 = (ImageButton)gr.FindControl("ibtnDelete1");
            //    ibtnDelete1.Enabled = false;
            //    CheckBox chkRow = (CheckBox)gr.FindControl("chkRow");
            //    chkRow.Enabled = false;


            //}


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindBlankServiceGrid()
    {
        try
        {
            cmbServiceName.Items.Clear();

            DataTable Dt = new DataTable();
            Dt.Columns.Add("ServiceID");
            Dt.Columns.Add("SerialNo");
            Dt.Columns.Add("ServiceName");
            Dt.Columns.Add("LongDescription");
            Dt.Columns.Add("SpecialPrecaution");
            Dt.Columns.Add("CPTCode");
            Dt.Columns["SerialNo"].AutoIncrement = true;
            Dt.Columns["SerialNo"].AutoIncrementSeed = 1;
            Dt.Columns["SerialNo"].AutoIncrementStep = 1;
            Dt.Columns.Add("Id");
            Dt.Columns.Add("EncounterDate");
            Dt.Columns.Add("ServiceType");
            Dt.Columns.Add("LonicCode");
            Dt.Columns.Add("IsBioHazard");
            for (int i = 1; i <= 1; i++)
            {
                DataRow Dr = Dt.NewRow();

                Dr["ServiceID"] = DBNull.Value;
                Dr["SerialNo"] = DBNull.Value;
                Dr["ServiceName"] = DBNull.Value;
                Dr["LongDescription"] = DBNull.Value;
                Dr["SpecialPrecaution"] = DBNull.Value;
                Dr["ServiceType"] = DBNull.Value;
                Dr["LonicCode"] = DBNull.Value;
                Dr["IsBioHazard"] = DBNull.Value;

                Dt.Rows.Add(Dr);
            }
            ViewState["BlankServiceGrid"] = "True";
            cmbServiceName.DataSource = Dt;
            cmbServiceName.DataBind();
            ViewState["BlankServiceGrid"] = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindDefaultControls()
    {
        DataTable dt = new DataTable();
        BaseC.EMROrders objEMROrdersnew = new BaseC.EMROrders(sConString);
        try
        {


            //SqlDataReader objDrOrder = objEMROrdersnew.populateInvestigationSetMain(Convert.ToInt16(Session["HospitalLocationID"]),
            //    common.myInt(Session["EmployeeId"]), common.myInt(Session["LoginDepartmentId"]));

            SqlDataReader objDrOrder = objEMROrdersnew.populateInvestigationSetMain(Convert.ToInt16(Session["HospitalLocationID"]),
               common.myInt(Session["EmployeeId"]), common.myInt(Session["LoginDepartmentId"]));


            if (objDrOrder.HasRows == true)
            {

                dt.Load(objDrOrder);
                gvorder.DataSource = dt;
                gvorder.DataBind();
            }
            else
            {
                BindBlankItemGrid();
            }

            objDrOrder.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            objEMROrdersnew = null;
        }
    }

    protected void imgAllergyAlert_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);

            Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                   common.myStr(Session["RegistrationID"]), common.myStr(Session["EncounterId"]), common.myInt(Session["UserId"]), 0);
            //}
            RadWindow2.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&FromEMR=1&EId=" + common.myStr(Session["EncounterId"])
                + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"])
                + "&PAG=" + common.myStr(Session["AgeGender"]) + "" + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&SepPat=Y";
            RadWindow2.Height = 400;
            RadWindow2.Width = 1050;
            RadWindow2.Top = 20;
            RadWindow2.Left = 20;
            //RadWindow2.OnClientClose = "OnClientClose";
            RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow2.Modal = true;
            //RadWindow2.InitialBehavior = WindowBehaviors.Maximize;
            RadWindow2.VisibleStatusbar = false;

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
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);

            Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                   common.myStr(Session["RegistrationID"]), common.myStr(Session["EncounterId"]), common.myInt(Session["UserId"]), 0);

            RadWindow2.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&FromEMR=1&EId=" + common.myStr(Session["EncounterId"])
               + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"])
               + "&PAG=" + common.myStr(Session["AgeGender"]) + "" + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&SepPat=Y";


            RadWindow2.Height = 400;
            RadWindow2.Width = 1050;
            RadWindow2.Top = 20;
            RadWindow2.Left = 20;
            //RadWindow2.OnClientClose = "OnClientClose";
            RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow2.Modal = true;
            //RadWindow2.InitialBehavior = WindowBehaviors.Maximize;
            RadWindow2.VisibleStatusbar = false;

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }
    private void BindBlankItemGrid()
    {

        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();

        try
        {

            dr["SetName"] = string.Empty;
            dr["SetID"] = 0;

            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvorder.DataSource = dt;
            gvorder.DataBind();
        }

        catch (Exception ex)
        {


        }
        finally
        {
            dt.Dispose();


        }

    }

    protected DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("SetName", typeof(string));
            dt.Columns.Add("SetID", typeof(int));
            return dt;
        }
        catch (Exception ex)
        {

            return dt;
        }

    }

    private void BindCategoryTypeDDL(string Type)
    {
        try
        {
            ddlDepartment.Items.Clear();
            //ddlDepartment.Text = "";
            DataSet ds = new DataSet();
            string departmentids = string.Empty;
            BaseC.clsEMRBilling objEmrBilling = new BaseC.clsEMRBilling(sConString);

            ds = objEmrBilling.uspGetDepartmentByType(Type);
            ddlDepartment.DataSource = ds.Tables[0];
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (!departmentids.Equals(string.Empty))
                        {
                            departmentids = departmentids + "," + common.myStr(ds.Tables[0].Rows[i]["DepartmentId"]);
                        }
                        else
                        {
                            departmentids = common.myStr(ds.Tables[0].Rows[i]["DepartmentId"]);
                        }
                    }
                }
            }

            ViewState["departmentids"] = departmentids;



            if (Type == "O")
            {
                ddlDepartment.Items.Insert(0, new RadComboBoxItem("All", "0"));
                ddlDepartment.SelectedIndex = 0;
                // ddlDepartment.SelectedValue = Session["LoginDepartmentId"] != null ? Session["LoginDepartmentId"].ToString() : "";
            }
            clsEMRBilling objclsEMRBilling = new clsEMRBilling(sConString);
            string Defaultdepartment = common.myStr(objclsEMRBilling.getHospitalSetupValue("DefaultLaboratoryDepartment", common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"])));

            if (rdoOrder.SelectedValue == "G" && Defaultdepartment != "")
            {
                // ddlDepartment.SelectedValue = Defaultdepartment;
                ddlDepartment.Items.Insert(0, new RadComboBoxItem("All", "0"));
                ddlDepartment.SelectedIndex = 0;
            }
            else if (rdoOrder.SelectedValue == "X" && Defaultdepartment != "")
            {
                ddlDepartment.Items.Insert(0, new RadComboBoxItem("All", "0"));
                ddlDepartment.SelectedIndex = 0;
            }

            //else
            //{
            //    ddlDepartment.Items.Insert(0, new RadComboBoxItem("", "0"));
            //    ddlDepartment.SelectedIndex = 0;
            //}
            ddlDepartment_OnSelectedIndexChanged(null, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    //Bind ModifierList(For Popup window)
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        gvPatientServiceDetail.Columns[8].Visible = false;

        lblMessage.Text = string.Empty;

        BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        RadDateTimePicker1.MinDate = System.DateTime.Now.Date;
        if (common.myStr(Session["EncounterId"]) == "")
        {
            Response.Redirect("/default.aspx?RegNo=0");
        }

        if (Request.QueryString["Mpg"] != null)
        {
            Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            string pid = Session["CurrentNode"].ToString();
            int len = pid.Length;
            ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, len - 1);
            Session["Orderpid"] = ViewState["PageId"].ToString();
        }
        else
        {
            if (Session["Orderpid"] != null)
                ViewState["PageId"] = Session["Pid"];
            else
                ViewState["PageId"] = "0";
        }
        if (!IsPostBack)
        {
            Session["CHECKED_ITEMS"] = null;
            Session["FavId"] = null;
            setisAllDoctorDisplayOnAddService();

            divConfirmation.Visible = false;

            BindBlnkGrid();
            dvConfirmAlreadyExistOptions.Visible = false;

            setTemplateTableInSection();

            setTemplateData();

            Session["OrderSetServiceIds"] = null;
            if (!common.myBool(Session["IsLoginDoctor"]))
            {
                btnOrderSet.Visible = false;
            }

            //Session["DuplicateCheck"] = 0;
            if (Session["RegistrationId"] != null && Session["EncounterId"] != null)
            {
                BaseC.EMROrders order = new BaseC.EMROrders(sConString);
                bind_order_Favrioute();
                if (common.myStr(Request.QueryString["For"]) == "SDReq")// Request from sub department request page 
                {
                    BaseC.EMROrders objEMROrders = new EMROrders(sConString);

                    DataView dvSub = objEMROrders.GetAllSubDepartment().DefaultView;
                    if (common.myStr(Request.QueryString["For"]) == "SDReq"
                        && common.myInt(Request.QueryString["RequestId"]) > 0)
                    {
                        dvSub.RowFilter = "SendRequestToDepartment=1";
                    }
                    else
                    {
                        dvSub.RowFilter = "SendRequestToDepartment=0";
                    }
                    ViewState["SubDepartment"] = dvSub.ToTable();
                    dvSub.RowFilter = "SubDeptId=" + Convert.ToInt16(Request.QueryString["SDId"]) + " AND DepartmentId=" + Convert.ToInt16(Request.QueryString["DId"]);
                    if (dvSub.ToTable().Rows.Count > 0)
                    {
                        rdoOrder.SelectedValue = dvSub.ToTable().Rows[0]["LabType"].ToString();
                        rdoOrder.Enabled = false;
                    }
                    btnOrderHistory.Visible = false;
                    btnPrint.Visible = false;
                    btnAddRequest.Visible = false;
                    btnRequestList.Visible = false;
                    btnClose1.Visible = true;
                }
                else
                {
                    ShowHideAddInvestigationSpecification();
                    btnClose1.Visible = false;
                    btnOrderHistory.Visible = true;
                    btnPrint.Visible = true;
                    btnAddRequest.Visible = true;
                    btnRequestList.Visible = true;
                }
                lblColorCode.BackColor = System.Drawing.Color.Pink;
                //icd.PanelName = pnlICDCodes.ClientID;
                //icd.ICDTextBox = txtICDCode.ClientID;

                txtICDCode.Attributes.Add("onclick", "javascript:ShowICDPanel('" + pnlICDCodes.ClientID + "','" + txtICDCode.ClientID + "', this )");
                getCurrentICDCodes();
                BindDefaultControls();
                ltrlInvSetName.Visible = false;

                hdnIsUnSavedData.Value = "0";
                BindICDPanel();
                BindPatientProvisionalDiagnosis();
                BindPatientHiddenDetails();

                if (common.myStr(Session["IsMedicalAlert"]) == "")
                {
                    lnkAlerts.Enabled = false;
                    lnkAlerts.CssClass = "blinkNone";
                    lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
                }
                else if (common.myStr(Session["IsMedicalAlert"]).ToUpper() == "YES")
                {
                    lnkAlerts.Enabled = true;
                    lnkAlerts.Font.Bold = true;
                    lnkAlerts.CssClass = "blink";
                    lnkAlerts.Font.Size = 11;
                    lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
                }
                rdoOrder_OnSelectedIndexChanged(sender, e);
                txtICDCode.Attributes.Add("onblur", "nSat=1;");
                txtICDCode.Focus();
                Hashtable hshOut = new Hashtable();
                hshOut = order.GetPatientCompanyId(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt32(Session["EncounterId"]));
                ViewState["CompanyCode"] = hshOut["@intCompanyId"];
                //BindPatientAlert();
                //BindICDGrid();
                btnAddPackage.Visible = false;
                lblPackageId.Text = "";
                if (common.myInt(Session["EncVisitPackageId"]) > 0)
                {
                    btnAddPackage.Visible = true;
                    lblPackageId.Visible = true;
                    lblPackageId.Text = "(" + common.myStr(Session["EncVisitPackageName"]) + " )";
                }
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
                {
                    btnSave.Visible = false;
                    // btnDeleteFavourite.Visible = false;
                    btnUpdate.Visible = false;
                    ddlDepartment.Enabled = false;
                    ddlSubDepartment.Enabled = false;
                    cmbServiceName.Enabled = false;
                    gvPatientServiceDetail.Enabled = false;
                    btnProceedFavourite.Enabled = false;
                    //  gvFavorites.Enabled = false;

                    //foreach (GridViewRow dataItem in gvFavorites.Rows)
                    //{
                    //    LinkButton lnkFAV = (LinkButton)dataItem.FindControl("lnkFAV");
                    //    LinkButton ibtnAddToList = (LinkButton)dataItem.FindControl("ibtnAddToList");
                    //    CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                    //    ImageButton ibtnDelete1 = (ImageButton)dataItem.FindControl("ibtnDelete1");

                    //    ibtnAddToList.Enabled = false;
                    //    lnkFAV.Enabled = false;
                    //    chkRow.Enabled = false;
                    //    ibtnDelete1.Enabled = false;
                    //}
                }

            }
            else
            {
                cmbServiceName.ClearSelection();
                cmbServiceName.Text = string.Empty;
                cmbServiceName.SelectedIndex = -1;
                //btnDeleteFavourite.Visible = false;
                btnAddToFavourite.Visible = true;
                ltrlInvSetName.Visible = false;

                ltrlInvCategory.Visible = true;
                ddlDepartment.Visible = true;

            }


            //Added by Rakesh for fill the color for template Required start
            lblColorCodeForTemplateRequired.BackColor = System.Drawing.Color.Red;
            lblColorCodeForMandatoryTemplate.BackColor = System.Drawing.Color.Blue;
            lblColorStat.BackColor = System.Drawing.Color.Aqua;
            lblFreeTest.BackColor = System.Drawing.Color.Coral;
            //Added by Rakesh for fill the color for template Required end


            //Added By Abhishek Goel for fill the minuts

            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                {
                    RadComboBox1.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    RadComboBox1.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
            }
            int iMinute = DateTime.Now.Minute;
            RadComboBoxItem rcbItem = (RadComboBoxItem)RadComboBox1.Items.FindItemByText(iMinute.ToString());
            if (rcbItem != null)
            {
                rcbItem.Selected = true;
            }

            SetPermission();

            BaseC.Security objSecurity = new BaseC.Security(sConString);
            ViewState["IsAllowToAddBlockedService"] = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowToAddBlockedService");
            objSecurity = null;

            cmbServiceName.Focus();
            cmbServiceName.Text = string.Empty;

            if (Request.QueryString["IsEMRPopUp"] != null && common.myStr(Request.QueryString["IsEMRPopUp"]).ToUpper().Equals("1"))
            {
                btnClose1.Visible = true;
            }
            if (common.myBool(Session["AllergiesAlert"]))
            {
                imgAllergyAlert.Visible = true;
                liAllergyAlert.Visible = true;
            }

            if (common.myBool(Session["MedicalAlert"]))
            {
                imgMedicalAlert.Visible = true;
                liMedicalAlert.Visible = true;
            }
            BindServiceDuration();

            if (common.myStr(Session["OPIP"]) == "I")
            {
                chkApprovalRequired.Visible = true;
            }
        }
        //added by bhakti
        ViewState["isRequireIPBillOfflineMarking"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "isRequireIPBillOfflineMarking", sConString);

        string IsFreeService = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsFreeService", sConString);
        if (IsFreeService.Equals("Y"))
        {
            chkFreeTest.Visible = true;
            lblFreeTest.Visible = true;
            Label9.Visible = true;
        }
        else if (IsFreeService.Equals("N"))
        {
            chkFreeTest.Visible = false;
            lblFreeTest.Visible = false;
            Label9.Visible = false;
        }
    }

    private void bindAssignToDoctor()
    {
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
        DataSet ds = new DataSet();
        StringBuilder strType = new StringBuilder();
        ArrayList coll = new ArrayList();

        try
        {
            ddlAssignToEmpId.ClearSelection();
            ddlAssignToEmpId.Items.Clear();

            coll.Add("LDIR");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("D");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("LD");
            strType.Append(common.setXmlTable(ref coll));

            int StationId = 1;
            if (common.myInt(hdnGlobleStationId.Value) > 0)
            {
                StationId = common.myInt(hdnGlobleStationId.Value);
            }

            ds = objMaster.getEmployeeData(common.myInt(Session["HospitalLocationID"]), StationId, 0, strType.ToString(), "", 0, common.myInt(Session["UserId"]), "", common.myInt(Session["FacilityId"]));

            ddlAssignToEmpId.SelectedIndex = -1;
            ddlAssignToEmpId.DataSource = ds.Tables[0].Copy();
            ddlAssignToEmpId.DataValueField = "EmployeeId";
            ddlAssignToEmpId.DataTextField = "EmployeeNameWithNo";
            ddlAssignToEmpId.DataBind();
            ddlAssignToEmpId.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlAssignToEmpId.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
        }
        finally
        {
            objMaster = null;
            ds.Dispose();
            strType = null;
            coll = null;
        }
    }


    public void setisAllDoctorDisplayOnAddService()
    {
        string setisAllDoctorDisplayOnAddService = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "isAllDoctorDisplayOnAddService", sConString);
        ViewState["setisAllDoctorDisplayOnAddService"] = "Y";
        if (!setisAllDoctorDisplayOnAddService.Equals(""))
        {
            ViewState["setisAllDoctorDisplayOnAddService"] = setisAllDoctorDisplayOnAddService.ToUpper();
        }
    }

    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations(sConString);
        try
        {
            btnSave.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));

            //ViewState["IsAllowEdit"] = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ua1.Dispose();
        }
    }

    void BindPatientHiddenDetails()
    {
        try
        {
            if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
            else
            {
                bindPatientDetails(common.myLong(Request.QueryString["RegNo"]), common.myStr(Request.QueryString["EncNo"]));

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void ShowHideAddInvestigationSpecification()
    {
        btnAddRequiredTemplate.Visible = false;
    }
    private void setTemplateData()
    {
        BaseC.EMRMasters objbc = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();

        try
        {
            //  tdTemplate.InnerHtml = BindEditor(true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objbc = null;
            ds.Dispose();
        }
    }
    private void setTemplateTableInSection()
    {
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        try
        {
            DataColumn col;

            col = new DataColumn("RegistrationId");
            tbl.Columns.Add(col);

            col = new DataColumn("EncounterId");
            tbl.Columns.Add(col);

            col = new DataColumn("xmlTemplateDetails");
            tbl.Columns.Add(col);

            col = new DataColumn("SectionId");
            tbl.Columns.Add(col);

            col = new DataColumn("ServiceId");
            tbl.Columns.Add(col);

            col = new DataColumn("TemplateId");
            tbl.Columns.Add(col);

            ds.Tables.Add(tbl);
            Session["TemplateDataDetails"] = ds;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            tbl.Dispose();
        }
    }
    protected void BindPatientProvisionalDiagnosis()
    {
        try
        {
            DataSet ds;
            BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);

            ds = new DataSet();
            ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["UserID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtProvisionalDiagnosis.Text = common.clearHTMLTags(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #region DDL Selected Event
    protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlSubDepartment.Items.Clear();
            if (common.myStr(Request.QueryString["For"]) == "SDReq")//this comming from sub departrment page 
            {
                if (ViewState["SubDepartment"] != null)
                {
                    BaseC.EMROrders objEMROrders = new EMROrders(sConString);
                    DataView dv = new DataView((DataTable)ViewState["SubDepartment"]);
                    dv.RowFilter = "DepartmentId=" + common.myInt(Request.QueryString["DId"]);
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        ddlSubDepartment.DataSource = dv.ToTable();
                        ddlSubDepartment.DataTextField = "SubName";
                        ddlSubDepartment.DataValueField = "SubDeptId";
                        ddlSubDepartment.DataBind();

                        if (common.myStr(Request.QueryString["For"]) != "SDReq")//this comming from sub departrment page 
                        {
                            ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("All"));
                            ddlSubDepartment.Items[0].Value = "0";
                        }
                    }
                    if (common.myStr(Request.QueryString["For"]) == "SDReq")//this comming from sub departrment page 
                    {
                        ddlSubDepartment.SelectedValue = dv.ToTable().Rows[0]["SubDeptId"].ToString();
                        ddlDepartment.SelectedValue = dv.ToTable().Rows[0]["DepartmentId"].ToString();
                        ddlDepartment.Enabled = false;
                    }
                }
            }
            else
            {
                DataSet ds = new DataSet();
                BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
                if (rdoOrder.SelectedValue == "O")
                    ds = objCommon.GetHospitalSubDepartment(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue), "O", 0);
                else
                    ds = objCommon.GetHospitalSubDepartment(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue), "", 0);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView dvSub = ds.Tables[0].DefaultView;
                    if (common.myStr(Request.QueryString["For"]) != "SDReq")
                    {
                        dvSub.RowFilter = "SendRequestToDepartment=0";
                    }

                    ddlSubDepartment.DataSource = dvSub.ToTable();
                    ddlSubDepartment.DataTextField = "SubName";
                    ddlSubDepartment.DataValueField = "SubDeptId";
                    ddlSubDepartment.DataBind();

                    ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("All"));
                    ddlSubDepartment.Items[0].Value = "0";



                }
                //objCommon.Close();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlSubDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cmbServiceName.Items.Clear();
            cmbServiceName.Text = "";

            cmbServiceName.DataSource = null;
            cmbServiceName.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion
    #region Button Event
    protected void visible(string type)
    {
        try
        {
            if (type == "IS")  ///////IS [ Radiology Services ]
            {
                trContrast.Visible = true;
                ddlAssignToEmpId.Enabled = true;
            }
            else if (type == "I") ////// I [ Lab Services ]
            {
                trContrast.Visible = false;
                ddlAssignToEmpId.Enabled = true;
            }
            else  ////////////////////////// [ Others ]
            {
                trContrast.Visible = false;
                if (common.myLen(type) > 0)
                {
                    ddlAssignToEmpId.Enabled = false;
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
    protected void btnAddToFavourite_Click(object sender, EventArgs e)
    {
        ViewState["ClearcmbServiceName"] = "";
        try
        {
            if (cmbServiceName.SelectedValue != "")
            {
                BaseC.EMROrders order = new BaseC.EMROrders(sConString);
                string sResult = order.SaveFavorite(common.myInt(Session["EmployeeId"]), common.myInt(cmbServiceName.SelectedValue),
                                        common.myInt(Session["UserId"]), common.myInt(ddlServiceDuration.SelectedValue));
                lblMessage.Text = sResult;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //cmbServiceName.Text = "";
                //cmbServiceName.ClearSelection();
                ViewState["ClearcmbServiceName"] = "N0";
                bind_order_Favrioute();
            }
            else
            {
                Alert.ShowAjaxMsg("Select a Service to Add in Favorites", this);
                return;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Mpg"] != null)
        {
            RadWindow2.NavigateUrl = "~/EMR/Orders/PrintOrder.aspx?rptName=POD&PageId=" + Request.QueryString["Mpg"].ToString().Substring(1);

        }
        else
        {
            RadWindow2.NavigateUrl = "~/EMR/Orders/PrintOrder.aspx?rptName=POD";
        }
        RadWindow2.Height = 580;
        RadWindow2.Width = 1000;
        RadWindow2.Top = 40;
        RadWindow2.Left = 100;
        RadWindow2.Title = "Print Order";
        //RadWindow2.OnClientClose = "OnClientCloseNextSlot";
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;
    }
    protected void lnkViewLabOrders_OnClick(object sender, EventArgs e)
    {
        Server.Transfer("ViewLabOrders.aspx");
    }
    private bool isSave()
    {
        bool isSaved = false;

        HiddenField hdnIsTemplateRequired = new HiddenField();
        HiddenField hdnServiceID = new HiddenField();
        StringBuilder strbRequiredServices = new StringBuilder();
        StringBuilder strXML = new StringBuilder();
        StringBuilder strTemplateDataDetailsXML = new StringBuilder();

        ViewState["vsTemplateDataDetails"] = "";

        DataSet dsTemplateDataDetails = new DataSet();
        if (Session["TemplateDataDetails"] != null)
        {
            dsTemplateDataDetails = (DataSet)Session["TemplateDataDetails"];
        }

        for (int i = 0; i < gvPatientServiceDetail.Rows.Count; i++)
        {
            hdnIsTemplateRequired = (HiddenField)gvPatientServiceDetail.Rows[i].FindControl("hdnIsTemplateRequired");
            hdnServiceID = (HiddenField)gvPatientServiceDetail.Rows[i].FindControl("hdnServiceID");

            if (hdnIsTemplateRequired.Value.Equals("2"))
            {
                if (dsTemplateDataDetails.Tables.Count > 0)
                {
                    if (dsTemplateDataDetails.Tables[0].Rows.Count > 0)
                    {
                        if (common.myInt(hdnServiceID.Value) > 0)
                        {
                            dsTemplateDataDetails.Tables[0].DefaultView.RowFilter = "";
                            dsTemplateDataDetails.Tables[0].DefaultView.RowFilter = "ServiceId=" + common.myInt(hdnServiceID.Value);

                            if (dsTemplateDataDetails.Tables[0].DefaultView.Count == 0)
                            {
                                strbRequiredServices.Append(((Label)gvPatientServiceDetail.Rows[i].FindControl("lblServiceName")).Text.Trim() + ", ");
                            }
                        }
                    }
                    else
                    {
                        strbRequiredServices.Append(((Label)gvPatientServiceDetail.Rows[i].FindControl("lblServiceName")).Text.Trim() + ", ");
                    }
                }
                else
                {
                    strbRequiredServices.Append(((Label)gvPatientServiceDetail.Rows[i].FindControl("lblServiceName")).Text.Trim() + ", ");
                }
            }
        }

        if (strbRequiredServices.ToString().Trim().Length > 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Investigation Specification Required for: " + strbRequiredServices;

            isSaved = false;
        }
        else
        {
            litRequiredInvestigationSpecification.Text = string.Empty;
            isSaved = true;
        }

        if (dsTemplateDataDetails.Tables.Count > 0)
        {
            if (dsTemplateDataDetails.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow DR in dsTemplateDataDetails.Tables[0].Rows)
                {
                    strTemplateDataDetailsXML.Append(DR["xmlTemplateDetails"]);
                }
            }
        }
        ViewState["vsTemplateDataDetails"] = strTemplateDataDetailsXML;

        if (common.myStr(Request.QueryString["For"]) == "SDReq"
            && common.myInt(Request.QueryString["RequestId"]) > 0)
        {
            lblMessage.Text = "";
            isSaved = true;
        }

        return isSaved;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        BaseC.clsEMR objEMR = new clsEMR(sConString);
        string strPopup = "<script language='javascript' ID='script1'>" + "window.close();" + "</script>";
        string mainDoctorId = "0";
        string doctorId = "0";
        try
        {
            try
            {
                //added by bhakti
                if (ViewState["isRequireIPBillOfflineMarking"].ToString() == "Y" && common.myStr(Session["OPIP"]).Equals("I") && common.myStr(Session["PatientCompanyType"]).Equals("C") && common.myStr(Session["IsOffline"]).Equals("True"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Patient is Offline, No transaction allow !')", true);
                    return;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            if (!isSave())
            {
                return;
            }

            if (Request.QueryString["For"] != null && Request.QueryString["DoctorId"] != null)
            {
                mainDoctorId = common.myStr(Request.QueryString["DoctorId"]);
            }
            else
            {
                mainDoctorId = common.myInt(Session["EmployeeId"]).ToString();

                if (!common.myBool(Session["IsLoginDoctor"]))
                {
                    DataSet dsDoctor = objEMR.getEncounterDoctor(common.myInt(Session["EncounterId"]));
                    if (dsDoctor.Tables[0].Rows.Count > 0)
                    {
                        mainDoctorId = common.myInt(dsDoctor.Tables[0].Rows[0]["DoctorId"]).ToString();
                    }
                }
            }

            ArrayList coll = new ArrayList();
            StringBuilder strXML = new StringBuilder();
            foreach (GridViewRow row in gvPatientServiceDetail.Rows)
            {
                //string ServiceId = row.Cells[0].Text;

                //bool Stat = common.myBool(row.Cells[8].Text);
                HiddenField hdnExcludedServices = (HiddenField)row.FindControl("hdnExcludedServices");
                HiddenField hdnPackageId = (HiddenField)row.FindControl("hdnPackageId");
                HiddenField hdnServiceID = (HiddenField)row.FindControl("hdnServiceID");
                HiddenField hdnStat = (HiddenField)row.FindControl("hdnStat");
                HiddenField HdnUrgent = (HiddenField)row.FindControl("HdnUrgent");
                HiddenField hdnFreeTest = (HiddenField)row.FindControl("hdnFreeTest");
                HiddenField hdnDocReq = (HiddenField)row.FindControl("hdnDocReq");
                RadComboBox ddlDoctor = (RadComboBox)row.FindControl("ddlDoctor");

                // HiddenField hdngvIsServiceRemarkMandatory = (HiddenField)row.FindControl("hdngvIsServiceRemarkMandatory");
                if (common.myStr(hdnServiceID.Value).Equals(string.Empty))
                {
                    Alert.ShowAjaxMsg("No Service selected", Page);
                    return;
                }

                Label lblgvIsServiceRemarkMandatory = (Label)row.FindControl("lblgvIsServiceRemarkMandatory");
                Label lblRemarks = (Label)row.FindControl("lblRemarks");
                Label lblUnits = (Label)row.FindControl("lblUnits");
                Label lblServiceName = (Label)row.FindControl("lblServiceName");
                Label lblTestDate = (Label)row.FindControl("lblTestDate");
                TextBox txtcharges = (TextBox)row.FindControl("txtcharges");
                HiddenField hdnRequestToDepartment = (HiddenField)row.FindControl("hdnRequestToDepartment");
                HiddenField hdnIsPriceEditableFromEMR = (HiddenField)row.FindControl("hdnIsPriceEditableFromEMR");
                HiddenField hdnServiceDurationId = (HiddenField)row.FindControl("hdnServiceDurationId");
                HiddenField hdnBiohazard = (HiddenField)row.FindControl("hdnBiohazard");
                HiddenField hdnAssignToEmpId = (HiddenField)row.FindControl("hdnAssignToEmpId");

                if (common.myStr(lblRemarks.Text).Equals(string.Empty) && common.myBool(lblgvIsServiceRemarkMandatory.Text))
                {
                    Alert.ShowAjaxMsg("Remarks / Rationale / Clinical Indication is mandatory for : " + common.myStr(lblServiceName.Text), Page);
                    txtInstruction.Focus();
                    return;
                }

                if (common.myBool(hdnDocReq.Value) && common.myInt(ddlDoctor.SelectedValue).Equals(0))
                {
                    Alert.ShowAjaxMsg("Doctor is mandatory for : " + common.myStr(lblServiceName.Text), Page);
                    gvPatientServiceDetail.Focus();
                    return;
                }
                doctorId = "0";
                if (common.myBool(hdnDocReq.Value))
                {
                    doctorId = common.myInt(ddlDoctor.SelectedValue).ToString();
                }
                else
                {
                    doctorId = mainDoctorId;
                }

                if (hdnRequestToDepartment.Value != "1")
                {
                    coll.Add(common.myInt(hdnServiceID.Value));//ServiceId INT   1
                    coll.Add(DBNull.Value);//VisitDate SMALLDATETIME 2
                    coll.Add(common.myInt(lblUnits.Text) > 0 ? common.myInt(lblUnits.Text) : 1);//Units SMALLINT  3
                    coll.Add(common.myInt(doctorId));//DoctorId INT    4
                    coll.Add(common.myDbl(txtcharges.Text));//ServiceAmount MONEY 5
                    coll.Add(DBNull.Value);//DoctorAmount MONEY  6
                    coll.Add(DBNull.Value);//ServiceDiscountAmount MONEY 7
                    coll.Add(DBNull.Value);//DoctorDiscountAmount MONEY  8
                    coll.Add(DBNull.Value);//AmountPayableByPatient MONEY    9
                    coll.Add(DBNull.Value);//AmountPayableByPayer MONEY  10
                    coll.Add(DBNull.Value);//ServiceDiscountPer MONEY    11
                    coll.Add(DBNull.Value);//DoctorDiscountPer MONEY 12
                    coll.Add(0);//PackageId INT   13
                    coll.Add(0);//OrderId INT 14
                    coll.Add(DBNull.Value);//UnderPackage BIT    15
                    coll.Add(hdnICDCode.Value);//ICDID VARCHAR(100)  16
                    coll.Add(DBNull.Value);//ResourceID INT  17
                    coll.Add(DBNull.Value);//SurgeryAmount MONEY 18
                    coll.Add(DBNull.Value);//ProviderPercent MONEY   19
                    coll.Add(DBNull.Value);//SeQNo INT   20
                    coll.Add(lblRemarks.Text);//Serviceremarks VARCHAR(250) NULL    21
                    coll.Add(DBNull.Value);//DetailId INT    22
                    coll.Add(0);//23
                    coll.Add(0);//24
                    coll.Add(DBNull.Value);//CoPayAmt MONEY  25
                    coll.Add(DBNull.Value);//DeductableAmount MONEY  26
                    coll.Add(DBNull.Value);//ApprovalCode VARCHAR(50)    27
                    coll.Add(common.myInt(Session["FacilityId"]));//FacilityId SMALLINT 28
                    coll.Add(common.myBool(hdnStat.Value) ? 1 : 0);//Stat BIT    29
                    coll.Add(hdnExcludedServices.Value);//IsExcluded BIT  30
                    coll.Add((common.myLen(lblTestDate.Text) > 0) ? common.myStr(lblTestDate.Text) : string.Empty);//TestDateTime SMALLDATETIME  31
                    coll.Add(common.myBool(hdnFreeTest.Value) ? 1 : 0);//FreeTest bit    32
                    coll.Add(DBNull.Value);//CPOERemark VARCHAR(300) 33
                    coll.Add(DBNull.Value);//34
                    coll.Add(DBNull.Value);//35
                    coll.Add(0);//IsDoneByAsstSurgeon BIT 36
                    coll.Add(0);//isNonDiscService Bit 37
                    coll.Add(common.myBool(hdnIsPriceEditableFromEMR.Value));//IsPriceEditableFromEMR BIT  38
                    coll.Add(common.myBool(HdnUrgent.Value) ? 1 : 0);//Urgent BIT 39
                    coll.Add(DBNull.Value);//POCRequest BIT  40
                    coll.Add(DBNull.Value);//CoPayPerc 41
                    coll.Add(hdnServiceDurationId.Value);//ServiceDurationId INT 42
                    coll.Add(DBNull.Value);//SurgeryId 43
                    coll.Add(hdnBiohazard.Value); //Biohaqard 44
                    coll.Add(DBNull.Value);//SurgeryComponentId 45
                    coll.Add(common.myInt(hdnAssignToEmpId.Value));//AssignToEmpId 46

                    strXML.Append(common.setXmlTable(ref coll));
                }
            }
            StringBuilder strXMLAleart = new StringBuilder();

            if (strXML.ToString() != "")
            {
                DataSet ds = new DataSet();
                ds = order.GetEncounterCompany(common.myInt(Session["EncounterId"]));
                string sChargeCalculationRequired = "Y";
                string stype = "P" + ds.Tables[0].Rows[0]["opip"].ToString().Trim();
                string opip = ds.Tables[0].Rows[0]["opip"].ToString().Trim();

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
                //if (Session["DuplicateCheck"].Equals(0))
                //{
                //Session["DuplicateCheck"] = 1;
                hshOut = order.saveOrders(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), strXML.ToString(),
                                    strXMLAleart.ToString(), common.myStr(txtInstruction.Text), common.myInt(Session["UserID"]),
                                    common.myInt(mainDoctorId), CompanyId, stype, "E", common.myStr(opip), InsuranceId,
                                    CardId, Convert.ToDateTime(DateTime.Now), sChargeCalculationRequired, chkAllergyReviewed.Checked, 1,
                                    RequestId, common.myStr(ViewState["vsTemplateDataDetails"]), common.myInt(Session["EntrySite"]),
                                    common.myBool(chkApprovalRequired.Checked), common.myBool(chkIsReadBack.Checked), common.myStr(txtIsReadBackNote.Text));

                if (hshOut["chvErrorStatus"].ToString().Length == 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    ViewState["OrderId"] = hshOut["intOrderId"];
                    lblMessage.Text = "Order No:" + hshOut["intOrderNo"] + " Saved Successfully";
                    lblMessage.Font.Bold = true;
                    #region Tagging Static Template with Template Field
                    if (common.myStr(Request.QueryString["POPUP"]).ToUpper().Equals("STATICTEMPLATE") && common.myInt(Request.QueryString["StaticTemplateId"]) > 0)
                    {
                        Hashtable htOut = new Hashtable();

                        htOut = objEMR.TaggingStaticTemplateWithTemplateField(common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                                           common.myInt(Request.QueryString["SectionId"]), common.myInt(Request.QueryString["TemplateFieldId"]),
                                           common.myInt(Request.QueryString["StaticTemplateId"]), common.myInt(Session["UserId"]));
                    }
                    #endregion

                    Session["TemplateDataDetails"] = null;
                    BindBlnkGrid();
                    ViewState["GridData"] = "";
                    //////if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
                    //////{
                    //////    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    //////}

                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    //lblMessage.Text = "There is some error while saving order." + common.myStr(hshOut["chvErrorStatus"]);
                    lblMessage.Text = common.myStr(hshOut["chvErrorStatus"]);
                }
                //}
                hdnIsUnSavedData.Value = "0";
                //bindBlankSelectedServices();

                //BindPatientAlert();
            }
            else
            {
                Alert.ShowAjaxMsg("No Service selected", Page);
                return;
            }

            ClearForm();

            //if (btnUpdate.Text == "Update")
            //{
            //    btnUpdate.Text = "Save";
            //}

            //////ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);

            if (lblMessage.Text.Contains("Saved Successfully"))
            //if (strMsg.Contains("successful"))
            {

                hdnIsUnSavedData.Value = "0";
                ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
                return;


            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    public void AddOrder(string from, int serviceId, int OrderSetId)
    {
        try
        {
            int CompanyId = 0, InsuranceId = 0, CardId = 0, DOCTORID = 0;

            string dnm = "";
            EMROrders order = new EMROrders(sConString);
            DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string flagEMROderDefaultDoctorSelection = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitallocationId"]), common.myInt(Session["FacilityId"]), "EMROrderSetDefaultServiceDoctor", sConString);
            DataSet dsCharge = order.GetEncounterCompany(common.myInt(Session["EncounterId"]));
            if (RadDateTimePicker1.SelectedDate != null)
            {
                if (DateTime.Compare(Convert.ToDateTime(RadDateTimePicker1.SelectedDate), DateTime.Now.Date) < 0)
                {
                    Alert.ShowAjaxMsg("please select correct investigation date", Page);
                    return;
                }
            }
            if (dsCharge.Tables.Count > 0)
            {
                if (dsCharge.Tables[0].Rows[0]["CompanyCode"].ToString().Trim() != "")
                {
                    CompanyId = common.myInt(dsCharge.Tables[0].Rows[0]["CompanyCode"].ToString().Trim());
                }
                if (dsCharge.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim() != "")
                {
                    InsuranceId = common.myInt(dsCharge.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim());
                }
                if (dsCharge.Tables[0].Rows[0]["CardId"].ToString().Trim() != "")
                {
                    CardId = common.myInt(dsCharge.Tables[0].Rows[0]["CardId"].ToString().Trim());
                }


                ///////////////////////changes by avanish////////////////////////////
                if (flagEMROderDefaultDoctorSelection.Equals("Y"))
                {
                    if (dsCharge.Tables[0].Rows[0]["DOCTORID"].ToString().Trim() != "")
                    {
                        DOCTORID = common.myInt(dsCharge.Tables[0].Rows[0]["DOCTORID"].ToString().Trim());
                        DataSet ds3 = dal.FillDataSet(CommandType.Text, "select 'Doctor_Name'=isnull(t.Name+' ','')+isnull(e.FirstName,'')+isnull(' '+e.MiddleName,'')+isnull(' '+e.LastName,'') from Employee e inner join TitleMaster t on e.TitleId=t.TitleID where e.id='" + DOCTORID + "'");
                        dnm = ds3.Tables[0].Rows[0]["Doctor_Name"].ToString();
                    }
                }
                ///////////////////////changes by avanish////////////////////////////

            }
            ///////////////////////changes by avanish////////////////////////////
            string doctorid = "0", dnm1 = "";
            int f = 0, did = 0;
            if (Request.QueryString["For"] != null && Request.QueryString["DoctorId"] != null)
            {
                doctorid = common.myStr(Request.QueryString["DoctorId"]);
                f = 1;
            }
            else
            {
                doctorid = Session["EmployeeId"].ToString();

                if (flagEMROderDefaultDoctorSelection.Equals("Y"))
                {

                    DataSet ds2 = dal.FillDataSet(CommandType.Text, "select 'Employee_Type' =EmployeeType from EmployeeType where id =(select EmployeeType from Employee where id='" + doctorid + "')");
                    string emptype = ds2.Tables[0].Rows[0]["Employee_Type"].ToString();

                    string[] drlst = { "D", "TM", "SR", "AN", "OD", "LD" };
                    if (drlst.Contains(emptype))
                    {
                        f = 1;
                        DataSet ds3 = dal.FillDataSet(CommandType.Text, "select 'Doctor_Name'=isnull(t.Name+' ','')+isnull(e.FirstName,'')+isnull(' '+e.MiddleName,'')+isnull(' '+e.LastName,'') from Employee e inner join TitleMaster t on e.TitleId=t.TitleID where e.id='" + doctorid + "'");
                        dnm1 = ds3.Tables[0].Rows[0]["Doctor_Name"].ToString();
                    }
                }
            }

            if (flagEMROderDefaultDoctorSelection.Equals("Y"))
            {
                string Drname = "";
                if (f == 1)
                {
                    Drname = dnm1;
                    did = common.myInt(doctorid);
                }
                else
                {
                    Drname = dnm;
                    did = Convert.ToInt32(DOCTORID);
                }
                Session["DN"] = Drname;
                Session["Did"] = did;
            }


            ///////////////////////changes by avanish////////////////////////////


            double dblServiceAmt = 0.0;
            bool IsLinkService = false;
            bool IsBlocked = false;


            DataSet ds = order.GetServiceDescriptionForOrderpage(common.myInt(Session["HospitallocationId"]), common.myInt(Session["FacilityId"]),
                                                    common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), serviceId,
                                                    OrderSetId, CompanyId, InsuranceId, CardId, 1, 0);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsServiceBlocked"]))
                        {
                            IsBlocked = true;
                        }
                    }
                    catch
                    {
                    }

                    if (common.myBool(IsBlocked))
                    {
                        if (!common.myBool(ViewState["Yes"]))
                        {
                            if (!common.myBool(ViewState["IsAllowToAddBlockedService"]))
                            {
                                Alert.ShowAjaxMsg("Not authorized to Add Service. Selected service is blocked for this company.", this.Page);
                                return;
                            }
                            divConfirmation.Visible = true;
                            return;
                        }
                    }

                    DataTable Newdata = new DataTable();

                    if (ViewState["GridData"] != null)
                    {
                        if (ViewState["GridData"] != string.Empty)
                        {
                            Newdata = (DataTable)ViewState["GridData"];
                        }
                    }

                    if (Newdata.Rows.Count == 0)
                    {
                        Newdata = CreateTable();
                    }

                    if (common.myInt(ViewState["EditIndx"]) > -1 && ViewState["EditIndx"] != null)
                    {
                        Newdata.Rows.RemoveAt(common.myInt(ViewState["EditIndx"]));
                    }
                    //Awadhesh
                    string provired_id = "";
                    int Service_id = 0;
                    foreach (GridViewRow row in gvPatientServiceDetail.Rows)
                    {


                        HiddenField hdnServiceID = (HiddenField)row.FindControl("hdnServiceID");
                        RadComboBox ddlDoctor = (RadComboBox)row.FindControl("ddlDoctor");
                        if (ddlDoctor.Visible)
                        {
                            Service_id = Convert.ToInt32(hdnServiceID.Value);
                            provired_id = common.myInt(ddlDoctor.SelectedValue).ToString();
                            foreach (DataRow rw in Newdata.Rows)
                            {

                                if (common.myInt(rw["ServiceId"]) == Service_id)
                                {
                                    rw["Providerid"] = provired_id;
                                    Newdata.AcceptChanges();
                                    rw.SetModified();
                                }
                            }


                        }
                    }

                    //

                    foreach (DataRow data in ds.Tables[0].Rows)
                    {
                        if (serviceId > 0)
                        {
                            Newdata.DefaultView.RowFilter = "ServiceId=" + common.myInt(serviceId);
                        }
                        else
                        {
                            Newdata.DefaultView.RowFilter = "ServiceId=" + common.myInt(data["ServiceId"]);
                        }

                        if (Newdata.DefaultView.Count == 0 || serviceId == 0)
                        {
                            hdnServiceType.Value = data["ServiceType"].ToString();
                            hdnID.Value = cmbServiceName.SelectedValue;
                            hdnServiceId.Value = data["ServiceId"].ToString();
                            //txtCPTCode.Text = data["RefServiceCode"].ToString();
                            hdnIsUnSavedData.Value = "1";
                            hdnAlertRequired.Value = data["AlertRequired"].ToString();
                            hdnAlertMessage.Value = data["AlertMessage"].ToString();
                            hdngvIsServiceRemarkMandatory.Value = data["isServiceRemarkMandatory"].ToString();

                            DataRow Dr = null;
                            Dr = Newdata.NewRow();
                            Dr["AlertRequired"] = data["AlertRequired"].ToString();
                            Dr["AlertMessage"] = data["AlertMessage"].ToString();
                            Dr["ServiceName"] = data["ServiceName"].ToString();
                            Dr["ServiceID"] = hdnServiceId.Value;
                            Dr["ICDID"] = txtICDCode.Text;
                            Dr["DoctorId"] = doctorid;
                            Dr["FacilityId"] = common.myStr(Session["FacilityId"]);
                            Dr["ServiceType"] = hdnServiceType.Value == "" ? "12" : hdnServiceType.Value;
                            Dr["Remarks"] = txtInstruction.Text;
                            Dr["Stat"] = common.myStr(chkStat.SelectedValue) == "STAT" ? 1 : 0;
                            Dr["Urgent"] = common.myStr(chkStat.SelectedValue) == "URGENT" ? 1 : 0;
                            Dr["LabStatus"] = "Not Billed";
                            Dr["OrderId"] = 0;
                            Dr["EncodedBy"] = 0;
                            Dr["IsExcluded"] = common.myBool(data["IsExcluded"]);
                            Dr["PlanTypeId"] = 0;
                            Dr["PackageId"] = 0;
                            Dr["CompanyId"] = 0;
                            Dr["ID"] = 0;
                            Dr["CPTCode"] = data["RefServiceCode"].ToString();
                            Dr["RequestToDepartment"] = false;
                            Dr["isServiceRemarkMandatory"] = common.myBool(hdngvIsServiceRemarkMandatory.Value);
                            Dr["FreeTest"] = common.myBool(chkFreeTest.Checked);
                            Dr["ServiceDurationId"] = common.myInt(ddlServiceDuration.SelectedItem.Value);
                            if (common.myInt(txtUnit.Text) > 0)
                            {
                                Dr["Charges"] = (common.myDbl(data["charges"]) * common.myDbl(txtUnit.Text)).ToString();
                            }
                            else
                            {
                                Dr["Charges"] = data["charges"].ToString();
                            }

                            Dr["result"] = data["result"].ToString();
                            Dr["Units"] = common.myInt(txtUnit.Text) > 0 ? common.myInt(txtUnit.Text) : 1;

                            if (RadDateTimePicker1.SelectedDate != null)
                            {
                                Dr["TestDate"] = common.myStr(RadDateTimePicker1.SelectedDate);
                            }
                            else
                            {
                                Dr["TestDate"] = common.myStr(DateTime.Now);//DBNull.Value;
                            }

                            Dr["DoctorRequired"] = common.myBool(data["DoctorRequired"]);
                            Dr["IsPriceEditableFromEMR"] = common.myBool(data["IsPriceEditableFromEMR"]);
                            if (!hdnServiceType.Value.ToUpper().Equals("I") && !hdnServiceType.Value.ToUpper().Equals("IS"))
                            {
                                Dr["IsBioHazard"] = false;
                                lblMessage.Text = "Biohazard is not applicable for (" + data["ServiceName"].ToString() + ")";
                            }
                            else
                            {
                                Dr["IsBioHazard"] = common.myBool(chkIsBioHazard.Checked);
                            }
                            Dr["StationId"] = common.myInt(hdnGlobleStationId.Value);

                            Dr["AssignToEmpId"] = common.myInt(ddlAssignToEmpId.SelectedValue);

                            IsLinkService = common.myBool(data["IsLinkService"]);
                            dblServiceAmt = common.myDbl(Dr["Charges"]);

                            Newdata.Rows.Add(Dr);

                            if (common.myInt(serviceId) > 0)
                            {
                                break;
                            }
                        }
                    }

                    Newdata.DefaultView.RowFilter = "";
                    //dvmain.RowFilter = "ServiceType Not in ( 'CL','VS','VF','R','RT','RF' )";
                    //ViewState["Service"] = dvmain.ToTable(); //sa
                    gvPatientServiceDetail.DataSource = Newdata;
                    gvPatientServiceDetail.DataBind();

                    //Declare an object variable. 
                    object sumObject;
                    sumObject = Newdata.Compute("Sum(Charges)", "");
                    lblTotCharges.Text = common.myStr(sumObject);
                    ViewState["GridData"] = Newdata;
                    txtUnit.Text = "1";
                    foreach (ListItem item in chkStat.Items)
                    {
                        item.Selected = false;
                    }
                    hdnStatValueContainer.Value = "0";
                    hdnIsServiceRemarkMandatory.Value = "0";
                    ViewState["EditIndx"] = null;
                    chkIsBioHazard.Checked = false;
                    if (serviceId == 0 || OrderSetId > 0)
                    {
                        IsLinkService = false;
                    }

                    if (IsLinkService)
                    {
                        addServiceFromLinkService(serviceId, common.myDbl(dblServiceAmt), CompanyId, InsuranceId, CardId);
                    }
                }
                else
                {
                    BindBlnkGrid();
                }
            }
            else
            {
                BindBlnkGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        //string c = cmbServiceName.SelectedValue;
        //string  b = common.myBool(cmbServiceName.SelectedItem.Attributes["isServiceRemarkMandatory"]);

        //if (common.myBool(cmbServiceName.SelectedItem.Attributes["isServiceRemarkMandatory"]) 
        //    && common.myStr(txtInstruction.Text).Equals(string.Empty))
        //{
        //    Alert.ShowAjaxMsg("Remarks for this service is Mandatory", Page);
        //    return;
        //}

        //if (common.myInt(Session["EMRPatientStatusId"]).Equals(184) || common.myInt(Session["EMRPatientStatusId"]).Equals(200) || common.myInt(Session["EMRPatientStatusId"]).Equals(628)
        //  || common.myInt(Session["EMRPatientStatusId"]).Equals(185) || common.myInt(Session["EMRPatientStatusId"]).Equals(199) || common.myInt(Session["EMRPatientStatusId"]).Equals(186)
        //  || common.myInt(Session["EMRPatientStatusId"]).Equals(8)
        //    )
        //{
        //    Alert.ShowAjaxMsg("Add To List not allowed as Patient status is already Sent For Billing ", Page);
        //    return;
        //}

        if (common.myBool(hdnIsServiceRemarkMandatory.Value) && common.myStr(txtInstruction.Text).Equals(string.Empty))
        {
            Alert.ShowAjaxMsg("Remarks / Rationale / Clinical Indication is Mandatory for : " + common.myStr(cmbServiceName.Text), Page);
            txtInstruction.Focus();
            return;
        }
        if (common.myBool(common.myStr(ViewState["hdngvFavisServiceRemarkMandatory"])) && common.myStr(txtInstruction.Text).Equals(string.Empty))
        {
            Alert.ShowAjaxMsg("Remarks / Rationale / Clinical Indication is Mandatory for : " + common.myStr(cmbServiceName.Text), Page);
            txtInstruction.Focus();
            return;
        }

        if (common.myInt(Session["EncounterId"]) > 0)
        {

            if (common.myStr(chkStat.SelectedValue) == "STAT")
            {
                if (!common.myBool(hdnStatValueContainer.Value).Equals(true))
                {
                    Alert.ShowAjaxMsg("This service can not be ordered as Stat", Page);
                    return;
                }
            }
            if (IsExistServiceOnSameDay(common.myInt(hdnServiceId.Value)))
            {
                dvConfirmAlreadyExistOptions.Visible = true;

                return;
            }
            string IsAutoAddFavirateInServiceOrder = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                        common.myInt(Session["FacilityId"]), "IsAutoAddFavirateInServiceOrder", sConString);
            if (IsAutoAddFavirateInServiceOrder.Equals("Y"))
            {
                btnAddToFavourite_Click(null, null);
            }
            addServices();

            visible(common.myStr(hdnServiceType.Value));

            cmbServiceName.Focus();
        }
    }

    private bool IsExistServiceOnSameDay(int serviceId)
    {
        bool isexist = false;
        try
        {
            DataSet datas = new DataSet();
            Hashtable hshInput = new Hashtable();

            hshInput.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hshInput.Add("@FacilityId", common.myInt(Session["FacilityId"]));
            hshInput.Add("@ServiceId", serviceId);
            hshInput.Add("@EncounterId", common.myInt(Session["EncounterId"]));
            hshInput.Add("@RegistrationId", common.myInt(Session["RegistrationId"]));

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            datas = dl.FillDataSet(CommandType.Text, "SELECT sd.ServiceId, i.ServiceName, ISNULL(em.FirstName,'') + ISNULL(' ' + em.MiddleName,'') + ISNULL(' ' + em.LASTNAME,'') AS EnteredBy, dbo.GetDateFormatUTC(s.EncodedDate,'DT', f.TimeZoneOffSetMinutes) OrderDate FROM ServiceOrderMain s WITH (NOLOCK) INNER JOIN ServiceOrderDetail sd WITH (NOLOCK) ON s.Id = sd.OrderId INNER JOIN ItemOfService i WITH (NOLOCK) ON sd.ServiceId = i.ServiceId INNER JOIN FacilityMaster f WITH (NOLOCK) ON S.FacilityId = f.FacilityID INNER JOIN Users us WITH (NOLOCK) ON s.EncodedBy = us.ID INNER JOIN Employee em WITH (NOLOCK) ON us.EmpID = em.ID WHERE ISNULL(s.EncounterId, 0) = @EncounterId AND s.RegistrationId = @RegistrationId AND CONVERT(VARCHAR,s.OrderDate,111) = CONVERT(VARCHAR,GETUTCDATE(),111) AND sd.ServiceId = @ServiceId And s.ACTIVE = 1 AND sd.ACTIVE = 1 AND s.FacilityId = @FacilityId AND s.HospitalLocationId = @HospitalLocationId ", hshInput);

            if (datas.Tables.Count > 0)
            {
                if (datas.Tables[0].Rows.Count > 0)
                {
                    lblServiceName.Text = common.myStr(datas.Tables[0].Rows[0]["ServiceName"]);
                    lblEnteredBy.Text = common.myStr(datas.Tables[0].Rows[0]["EnteredBy"]);
                    lblEnteredOn.Text = common.myStr(datas.Tables[0].Rows[0]["OrderDate"]);

                    isexist = true;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return isexist;
    }

    private void addServices()
    {
        AddOrder("M", common.myInt(hdnServiceId.Value), 0);
        btnUpdate.Text = "Add To List";

        if (!divConfirmation.Visible)
        {
            RadDateTimePicker1.SelectedDate = null;
            RadComboBox1.SelectedIndex = 0;
            cmbServiceName.Text = "";
            cmbServiceName.Items.Clear();
            cmbServiceName.ClearSelection();
            txtInstruction.Text = string.Empty;
            chkFreeTest.Checked = false;

            ddlSubDepartment.SelectedIndex = 0;
            ddlServiceDuration.SelectedIndex = 0;
            ddlAssignToEmpId.SelectedIndex = 0;
        }
    }

    #endregion

    #region Grid Patient Service Details
    protected void gvPatientServiceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //if (common.myStr(Request.QueryString["For"]) == "SDReq"
            //    && common.myInt(Request.QueryString["RequestId"]) > 0)
            //{
            //    e.Row.Cells[15].Visible = false;
            //}
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[0].Visible = false;
            //e.Row.Cells[3].Visible = false;
            //e.Row.Cells[4].Visible = false;
            //e.Row.Cells[5].Visible = false;
            //e.Row.Cells[6].Visible = false;
            //e.Row.Cells[7].Visible = false;
            //e.Row.Cells[8].Visible = false;
            //e.Row.Cells[9].Visible = false;
            //e.Row.Cells[10].Visible = false;

            //if (common.myStr(Request.QueryString["For"]) == "SDReq"
            //    && common.myInt(Request.QueryString["RequestId"]) > 0)
            //{
            //    e.Row.Cells[15].Visible = false;
            //}

            HiddenField hdnExcludedServices = (HiddenField)e.Row.FindControl("hdnExcludedServices");
            if (hdnExcludedServices.Value != "")
            {
                if (common.myBool(hdnExcludedServices.Value))
                {
                    e.Row.BackColor = System.Drawing.Color.Pink;
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                }
            }
            HiddenField hdnServiceID = (HiddenField)e.Row.FindControl("hdnServiceID");
            Label lblServiceName = (Label)e.Row.FindControl("lblServiceName");
            HiddenField hdnIsTemplateRequired = (HiddenField)e.Row.FindControl("hdnIsTemplateRequired");
            HiddenField hdnresult = (HiddenField)e.Row.FindControl("hdnresult");
            HiddenField hdnAlertRequired = (HiddenField)e.Row.FindControl("hdnAlertRequired");
            HiddenField hdnAlertMessage = (HiddenField)e.Row.FindControl("hdnAlertMessage");
            HiddenField hdnStat = (HiddenField)e.Row.FindControl("hdnStat");
            HiddenField hdnFreeTest = (HiddenField)e.Row.FindControl("hdnFreeTest");

            if (common.myBool(hdnAlertRequired.Value))
            {
                lblAlertMessage.Text = hdnAlertMessage.Value;
                lblAlertMessage.ForeColor = System.Drawing.Color.Red;
            }

            Label lblCharges = (Label)e.Row.FindControl("lblCharges");
            TextBox txtcharges = (TextBox)e.Row.FindControl("txtcharges");
            HiddenField hdnIsPriceEditableFromEMR = (HiddenField)e.Row.FindControl("hdnIsPriceEditableFromEMR");


            if (common.myBool(hdnIsPriceEditableFromEMR.Value))
            {
                txtcharges.Visible = true;
                lblCharges.Visible = false;
            }
            hdnIsTemplateRequired.Value = "0";
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);

            LinkButton lnkAddInvestigationSpecification = (LinkButton)e.Row.FindControl("lnkAddInvestigationSpecification");
            lnkAddInvestigationSpecification.Visible = false;

            int result = common.myInt(hdnresult.Value);

            if (result.Equals(2))
            {
                lblServiceName.ForeColor = System.Drawing.Color.Red;
                lblServiceName.Style["font-bold"] = "true";
                hdnIsTemplateRequired.Value = "2";
                lnkAddInvestigationSpecification.Visible = true;
            }
            else if (result.Equals(1))
            {
                lblServiceName.ForeColor = System.Drawing.Color.Blue; //Color.FromName("#5D0000");
                hdnIsTemplateRequired.Value = "1";
                lnkAddInvestigationSpecification.Visible = true;
            }
            if (common.myBool(hdnStat.Value) == true)
            {
                //lblServiceName.ForeColor = System.Drawing.Color.Aqua;
                e.Row.BackColor = System.Drawing.Color.Aqua;
                e.Row.ToolTip = "Stat Service";
            }
            if (common.myBool(hdnFreeTest.Value))
            {
                //lblServiceName.ForeColor = System.Drawing.Color.Aqua;
                //e.Row.BackColor = System.Drawing.Color.Coral;
                //e.Row.ToolTip = "Free Test";
                TableCell cell = e.Row.Cells[0];
                cell.BackColor = Color.Coral;
            }
            DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string flagEMROderDefaultDoctorSelection = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitallocationId"]), common.myInt(Session["FacilityId"]), "EMROrderSetDefaultServiceDoctor", sConString);

            HiddenField hdnDocReq = (HiddenField)e.Row.FindControl("hdnDocReq");
            HiddenField hdnProviderid = (HiddenField)e.Row.FindControl("hdnProviderid");

            Label lblDoctorID = (Label)e.Row.FindControl("lblDoctorID");
            RadComboBox ddlDoctor = (RadComboBox)e.Row.FindControl("ddlDoctor");

            if (common.myInt(hdnDocReq.Value).Equals(1) || common.myBool(hdnDocReq.Value))
            {
                ddlDoctor.Visible = true;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                if ((DataTable)ViewState["EmpClassi"] == null)
                {
                    BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
                    ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]), 0, 0);
                    DataView dvF = new DataView(ds.Tables[0]);
                    dvF.RowFilter = "Type IN ('D','TM','SR','AN','OD','LD')";
                    dvF = dvF.ToTable().DefaultView;
                    if (common.myStr(Session["OPIP"]).Equals("O"))
                    {
                        dvF.RowFilter = "ProvidingService IN ('O','B')";
                    }
                    else if (common.myStr(Session["OPIP"]).Equals("I"))
                    {
                        dvF.RowFilter = "ProvidingService IN ('I','B')";
                    }
                    dt = dvF.ToTable();
                    ViewState["EmpClassi"] = dt;
                }
                else
                {
                    dt = (DataTable)ViewState["EmpClassi"];
                }
                if (common.myStr(ViewState["setisAllDoctorDisplayOnAddService"]).ToUpper().Equals("N"))
                {
                    DataTable dt1 = new DataTable();
                    DataView dvDF = new DataView((DataTable)ViewState["EmpClassi"]);
                    //dvDF.RowFilter = "DepartmentId=" + common.myStr(hdnDeptId.Value);
                    dt1 = dvDF.ToTable();

                    if (dt1.Rows.Count > 0)
                    {
                        dt = dt1;
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    ddlDoctor.Items.Clear();
                    ddlDoctor.DataSource = dt;
                    ddlDoctor.DataValueField = "EmployeeId";
                    ddlDoctor.DataTextField = "EmployeeName";
                    ddlDoctor.DataBind();


                    if (flagEMROderDefaultDoctorSelection.Equals("Y"))
                    {
                        ddlDoctor.Items.Insert(0, new RadComboBoxItem(Session["DN"].ToString(), Session["Did"].ToString()));
                    }
                    else
                    {
                        ddlDoctor.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
                    }


                    ddlDoctor.SelectedIndex = 0;
                    if (hdnProviderid.Value != "")
                    {
                        ddlDoctor.SelectedValue = hdnProviderid.Value;
                    }


                }
                if (common.myStr(Session["OPIP"]).Equals("O"))
                {
                    //if (hdnIsPriceEditable.Value == "True")
                    //{
                    //    txtDoctorAmount.Enabled = true;
                    //}
                }
                //ddlDoctor.SelectedValue = common.myStr(DataBinder.Eval(e.Row.DataItem, "DoctorId"));
            }
            else
            {
                ddlDoctor.Visible = false;
            }

            lblDoctorID.Visible = false;

        }
    }
    protected void gvPatientServiceDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            HiddenField hdnServiceID = (HiddenField)gvPatientServiceDetail.Rows[e.RowIndex].FindControl("hdnServiceID");

            hdnUpdateServiceId.Value = hdnServiceID.Value;
            //hdnOrderId.Value = gvPatientServiceDetail.Rows[e.RowIndex].Cells[10].Text;
            //hdnUpdateOrderDtlId.Value = ((HiddenField)gvPatientServiceDetail.Rows[e.RowIndex].FindControl("hdnOrderDtlId")).Value;
            //hdnDepartmentRequest.Value = ((HiddenField)gvPatientServiceDetail.Rows[e.RowIndex].FindControl("hdnRequestToDepartment")).Value;
            if (hdnUpdateServiceId.Value != "&nbsp;")
            {
                divDelete.Visible = true;
            }
            else
            {
                Alert.ShowAjaxMsg("Please select Service", Page);
                return;
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion
    protected void lnkAllergyDetails_OnClick(object o, EventArgs e)
    {
        RadWindow2.NavigateUrl = "~/EMR/Allergy/PatientAllergy.aspx";
        RadWindow2.Height = 200;
        RadWindow2.Width = 520;
        RadWindow2.Top = 40;
        RadWindow2.Left = 100;
        RadWindow2.Title = "Patient Allergy";
        RadWindow2.OnClientClose = "OnClientClose";
        RadWindow2.Modal = true;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow2.VisibleStatusbar = false;
    }
    protected void cmbServiceName_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //visible(hdnServiceType.Value);
        if (cmbServiceName.SelectedValue != "")
        {
            hdnServiceId.Value = cmbServiceName.SelectedValue;
            string serviceName = common.myStr(cmbServiceName.Text);
            //if (hdnStatValueContainer.Value.Equals(""))
            //{
            //    hdnStatValueContainer.Value = "True";
            //}
            //else
            //{
            hdnStatValueContainer.Value = common.myStr(hdnStatValueContainer.Value);
            hdnIsServiceRemarkMandatory.Value = common.myStr(hdnIsServiceRemarkMandatory.Value);
            //}
            //    AddOrder("NEW", Convert.ToInt32(cmbServiceName.SelectedValue), 0);

            bindAssignToDoctor();
            visible(common.myStr(hdnServiceType.Value));

            btnUpdate.Focus();
        }
    }
    protected void cmbServiceName_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text.Length > 1)
        {
            DataTable data = BindSearchCombo("%" + e.Text);

            // BindCategoryTree();
            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.cmbServiceName.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                //RadCmbPatientSearch.Items.Add(new RadComboBoxItem(data.Rows[i]["CompanyName"].ToString(), data.Rows[i]["CompanyName"].ToString()));
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ServiceName"];
                item.Value = data.Rows[i]["ServiceID"].ToString();
                item.Attributes["CPTCode"] = data.Rows[i]["CPTCode"].ToString();
                item.Attributes["LongDescription"] = data.Rows[i]["LongDescription"].ToString();
                item.Attributes["ServiceType"] = data.Rows[i]["ServiceType"].ToString();
                item.Attributes["IsStatOrderAllowed"] = data.Rows[i]["IsStatOrderAllowed"].ToString();
                item.Attributes["isServiceRemarkMandatory"] = data.Rows[i]["isServiceRemarkMandatory"].ToString();
                item.Attributes["IsLinkService"] = data.Rows[i]["IsLinkService"].ToString();
                item.Attributes["StationId"] = data.Rows[i]["StationId"].ToString();

                this.cmbServiceName.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);

            visible(hdnServiceType.Value);
        }
    }
    protected void gvFavorites_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Del"))
            {
                try
                {
                    string FavID;
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    FavID = ((HiddenField)row.FindControl("hdnFAvID")).Value.ToString();
                    //str_FavServicID = ((LinkButton)row.FindControl("lnkFAV")).Text.ToString();

                    if (FavID != "")
                    {
                        string sResult;
                        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
                        sResult = order.DeleteFavorite(common.myInt(Session["EmployeeId"].ToString()), common.myInt(FavID), common.myInt(Session["UserId"]));
                        lblMessage.Text = sResult;
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        cmbServiceName.Text = "";
                        cmbServiceName.ClearSelection();
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Select a Service to Delete From Favorites", this);
                    }
                    bind_order_Favrioute();
                    BindSearchCombo("");
                }
                catch (Exception Ex)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Error: " + Ex.Message;
                    objException.HandleException(Ex);
                    BindBlnkGrid();
                }
            }
            else if (e.CommandName.Equals("FAVLIST"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int FavID = common.myInt(((HiddenField)row.FindControl("hdnFAvID")).Value);

                str_FavServicID = ((LinkButton)row.FindControl("lnkFAV")).Text.ToString();
                HiddenField hdnDepartmentId = (HiddenField)row.FindControl("hdnDepartmentId");
                HiddenField hdnSubDeptId = (HiddenField)row.FindControl("hdnSubDeptId");
                HiddenField hdnType = (HiddenField)row.FindControl("hdnType");
                HiddenField hdnLabType = (HiddenField)row.FindControl("hdnLabType");
                HiddenField hdnStatOrderAllowed = (HiddenField)row.FindControl("hdnStatOrderAllowed");
                HiddenField hdngvFavisServiceRemarkMandatory = (HiddenField)row.FindControl("hdngvFavisServiceRemarkMandatory");
                HiddenField hdnServiceDurationId = (HiddenField)row.FindControl("hdnServiceDurationId");
                HiddenField hdnStationId = (HiddenField)row.FindControl("hdnStationId");

                hdnServiceType.Value = hdnType.Value;
                hdnGlobleStationId.Value = hdnStationId.Value;

                cmbServiceName.Text = str_FavServicID;
                hdnServiceId.Value = FavID.ToString();
                ViewState["FAVLIST"] = "FAVLIST";
                cmbServiceName.SelectedValue = FavID.ToString();
                rdoOrder.SelectedValue = hdnLabType.Value.ToString();
                ViewState["hdngvFavisServiceRemarkMandatory"] = common.myStr(hdngvFavisServiceRemarkMandatory.Value);

                ddlServiceDuration.SelectedIndex = ddlServiceDuration.Items.IndexOf(ddlServiceDuration.Items.FindByValue(common.myInt(hdnServiceDurationId.Value).ToString()));

                //hdnStatValueContainer.Value = hdnStatOrderAllowed.Value;
                //if (hdnStatOrderAllowed.Value.Equals(""))
                //{
                //    hdnStatValueContainer.Value = "True";
                //}
                //else
                //{
                hdnStatValueContainer.Value = hdnStatOrderAllowed.Value;

                //}
                rdoOrder_OnSelectedIndexChanged(this, null);
                ddlDepartment.SelectedIndex = ddlDepartment.Items.IndexOf(ddlDepartment.FindItemByValue(hdnDepartmentId.Value.ToString()));
                ddlDepartment_OnSelectedIndexChanged(this, null);
                ddlSubDepartment.SelectedIndex = ddlSubDepartment.Items.IndexOf(ddlSubDepartment.FindItemByValue(hdnSubDeptId.Value.ToString()));

                bindAssignToDoctor();
                visible(common.myStr(hdnServiceType.Value));
            }
            else if (e.CommandName.Equals("AddToList"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int FavID = common.myInt(((HiddenField)row.FindControl("hdnFAvID")).Value);

                str_FavServicID = ((LinkButton)row.FindControl("lnkFAV")).Text.ToString();
                HiddenField hdnDepartmentId = (HiddenField)row.FindControl("hdnDepartmentId");
                HiddenField hdnSubDeptId = (HiddenField)row.FindControl("hdnSubDeptId");
                HiddenField hdnType = (HiddenField)row.FindControl("hdnType");
                HiddenField hdnLabType = (HiddenField)row.FindControl("hdnLabType");
                HiddenField hdnStationId = (HiddenField)row.FindControl("hdnStationId");

                hdnServiceType.Value = hdnType.Value;
                hdnGlobleStationId.Value = hdnStationId.Value;

                //cmbServiceName.Text = str_FavServicID;
                //hdnServiceId.Value = FavID.ToString();
                ViewState["FAVLIST"] = "FAVLIST";
                //cmbServiceName.SelectedValue = FavID.ToString();
                //ddlDepartment.SelectedIndex = ddlDepartment.Items.IndexOf(ddlDepartment.FindItemByValue(hdnDepartmentId.Value.ToString()));
                if (IsExistServiceOnSameDay(common.myInt(FavID)))
                {
                    hdnServiceId.Value = common.myStr(FavID);
                    dvConfirmAlreadyExistOptions.Visible = true;
                }
                else
                {
                    AddOrder("FAV", common.myInt(FavID), 0);
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
    private void binddrplist(string Rdoselection)
    {
        //ViewState[] 
        //DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);

        ds = order.GetFavorites(common.myInt(Session["EmployeeId"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(Session["FacilityId"]));

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvFavorites.DataSource = ds.Tables[0];
                gvFavorites.DataBind();
            }
            else
            {
                BindBlankFavGrid();
            }

            ViewState["Favorites"] = ds.Tables[0];
        }
    }
    protected void chkApprovalRequired_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkApprovalRequired.Checked)
        {
            chkIsReadBack.Visible = true;
            lblReadBackNote.Visible = true;
            txtIsReadBackNote.Visible = true;
        }
        else
        {
            chkIsReadBack.Visible = false;
            lblReadBackNote.Visible = false;
            txtIsReadBackNote.Visible = false;

            txtIsReadBackNote.Text = "";
            chkIsReadBack.Checked = false;
        }
    }
    protected DataTable BindSearchCombo(String etext)
    {
        DataTable dt = new DataTable();
        DataView DV = new DataView();
        DataView DVServiceFilter = new DataView();
        try
        {
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);


            //if (ViewState["ServiceData"] == null)
            //{
            if (Request.QueryString["ServicesForWard"] != null && !common.myStr(Request.QueryString["ServicesForWard"]).Equals(string.Empty))
            {
                if (common.myInt(Request.QueryString["ServicesForWard"]).Equals(1))
                {
                    dt = order.GetSearchServices(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue),
                                            ddlSubDepartment.SelectedValue == "" ? "0" : ddlSubDepartment.SelectedValue,
                                            common.myStr(etext, true), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]), common.myInt(Request.QueryString["ServicesForWard"]));
                }
                else
                {
                    dt = order.GetSearchServices(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue),
                                                      ddlSubDepartment.SelectedValue == "" ? "0" : ddlSubDepartment.SelectedValue,
                                                      common.myStr(etext, true), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]));
                }

            }
            else
            {

                dt = order.GetSearchServices(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue),
                                                  ddlSubDepartment.SelectedValue == "" ? "0" : ddlSubDepartment.SelectedValue,
                                                  common.myStr(etext, true), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]));

            }



            if (common.myInt(ddlDepartment.SelectedValue).Equals(0))
            {
                if (!rdoOrder.SelectedValue.Equals("O"))
                {
                    DVServiceFilter = dt.DefaultView;
                    if (ViewState["departmentids"] != null && !common.myStr(ViewState["departmentids"]).Equals(string.Empty))
                    {
                        DVServiceFilter.RowFilter = "DepartmentId in (" + common.myStr(ViewState["departmentids"]) + ")      ";
                    }
                    dt = DVServiceFilter.ToTable();
                }
            }

            if (rdoOrder.SelectedValue == "OS")
            {
                dt.Rows.Clear();
                dt.Rows.Add(dt.NewRow());
                dt.AcceptChanges();
            }

            DV = dt.DefaultView;
            if (common.myStr(Request.QueryString["For"]) == "SDReq"
                && common.myInt(Request.QueryString["RequestId"]) > 0)
            {
                ViewState["ServiceData"] = DV.ToTable();
            }
            else
            {

                DV.RowFilter = "SendRequestToDepartment=0";

                ViewState["ServiceData"] = DV.ToTable();
            }


            //if (ViewState["Edit"] != null)
            //{
            //    cmbServiceName.Items.Clear();
            //    cmbServiceName.Text = "";

            //    cmbServiceName.DataSource = null;
            //    cmbServiceName.DataBind();

            //    cmbServiceName.DataSource = (DataTable)ViewState["ServiceData"];
            //    cmbServiceName.DataTextField = "ServiceName";
            //    cmbServiceName.DataValueField = "ServiceId";
            //    cmbServiceName.DataBind();
            //    cmbServiceName.SelectedValue = hdnServiceId.Value;
            //    ViewState["Edit"] = null;

            return DV.ToTable();

            //}


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return DV.ToTable();
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    protected void btnOrderHistory_OnClick(object sender, EventArgs e)
    {
        RadWindow2.NavigateUrl = "/EMR/Orders/Servicedetails.aspx?Deptid=0&EncId=" + Session["EncounterId"] +
                                "&RegNo=" + Session["RegistrationNo"] + "&sBillId=0&PType=WD";
        RadWindow2.Height = 500;
        RadWindow2.Width = 1000;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;
    }
    protected void bind_order_Favrioute()
    {
        if (common.myStr(ViewState["ClearcmbServiceName"]) != "N0")
        {
            cmbServiceName.ClearSelection();
            cmbServiceName.Text = string.Empty;
            cmbServiceName.SelectedIndex = -1;
            cmbServiceName.Text = "";
        }


        btnAddToFavourite.Visible = false;
        binddrplist("FV");

        btnAddToFavourite.Visible = true;
        btnAddToFavourite.Visible = true;
        ltrlInvSetName.Visible = false;
        ltrlInvCategory.Visible = true;
        ddlDepartment.Visible = true;
        //ddlDepartment.SelectedValue = "0";
        ddlDepartment.Enabled = true;
        if (Request.QueryString["For"] != null && Request.QueryString["For"] == "SDReq")
        {
            ddlDepartment.Enabled = false;
        }
        else
        {
            ddlDepartment.Enabled = true;
        }
        //BindCategoryDDL();
        // binddrplist("AL");

        //ClearForm();

        btnUpdate.Text = "Add To List";

    }
    protected void rdoOrder_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //cmbServiceName.ClearSelection();
        //cmbServiceName.Text = string.Empty;
        //cmbServiceName.SelectedIndex = -1;
        //ddlDepartment.Items.Clear();
        // ddlSubDepartment.Items.Clear();
        BindCategoryTypeDDL(rdoOrder.SelectedValue);

        if (!rdoOrder.SelectedValue.Equals("X"))
        {
            btnAddRequest.Visible = false;
            btnRequestList.Visible = false;
        }
        else
        {
            btnAddRequest.Visible = true;
            btnRequestList.Visible = true;
        }

        //ClearForm();
        btnUpdate.Text = "Add To List";
    }
    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        if (hdnUpdateServiceId.Value != "" || hdnUpdateServiceId.Value != "0")
        {
            DataTable Newdata = (DataTable)ViewState["GridData"];
            Newdata.DefaultView.RowFilter = "ServiceId NOT IN (" + hdnUpdateServiceId.Value + ")";
            gvPatientServiceDetail.DataSource = Newdata.DefaultView.ToTable();
            gvPatientServiceDetail.DataBind();
            object sumObject;
            sumObject = Newdata.DefaultView.ToTable().Compute("Sum(Charges)", "");
            lblTotCharges.Text = common.myStr(sumObject);
            ViewState["GridData"] = Newdata.DefaultView.ToTable();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        }


        divDelete.Visible = false;
        //Added by Rakesh start
        if (Request.QueryString["For"] == null)
        {
            ShowHideAddInvestigationSpecification();
        }
        //Added by Rakesh end 
    }
    protected void btnNo_OnClick(object sender, EventArgs e)
    {
        divDelete.Visible = false;
    }
    protected void btnAddPackage_Click(object sender, EventArgs e)
    {
        if (common.myInt(Session["EncVisitPackageId"]) > 0)
        {
            if (txtICDCode.Text == "")
            {
                Alert.ShowAjaxMsg("Diagnosis not Available", Page);
                return;
            }
            ViewState["PackageService"] = null;
            BaseC.Package oPack = new BaseC.Package(sConString);
            DataSet ds = oPack.GetPackageServiceLimit(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["EncVisitPackageId"]), 0, common.myInt(Session["FacilityId"]));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["PackageService"] = ds.Tables[0];
                DataTable dt = ((DataTable)ViewState["PackageService"]);
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foreach (GridViewRow row in gvPatientServiceDetail.Rows)
                    {
                        if (dt.Rows[i]["ServiceId"].ToString().Trim() == row.Cells[0].Text.Trim())
                        {
                            if (count == 0)
                            {
                                ViewState["ServiceId"] = dt.Rows[i]["ServiceId"].ToString();
                                count++;
                            }
                            else
                            {
                                ViewState["ServiceId"] = ViewState["ServiceId"] + "," + dt.Rows[i]["ServiceId"].ToString();
                            }
                        }
                    }
                }

            }
            else
            {
                Alert.ShowAjaxMsg("Package Breakup Not Defined!..", Page.Page);
                return;
            }
        }
    }
    protected void lnkAlerts_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";

            RadWindow2.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&PId=" + common.myStr(Session["RegistrationId"]);
            RadWindow2.Height = 600;
            RadWindow2.Width = 600;
            RadWindow2.Top = 10;
            RadWindow2.Left = 10;
            RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow2.Modal = true;
            RadWindow2.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }
    protected void ddlFavrioute_SelectedIndexChanged(object sender, EventArgs e)
    {
        rdoOrder.SelectedValue = "FV";
        visible(hdnServiceType.Value);
    }

    //Code for Order Sets in Gridview-End
    protected void btnsearch_OnClick(object sender, EventArgs e)
    {
        SearchFavrioute(txtSearchFavrioute.Text);
    }
    protected void txtSearchFavrioute_OnTextChanged(object sender, EventArgs e)
    {
        SearchFavrioute(txtSearchFavrioute.Text);
    }
    protected void SearchFavrioute(string s)
    {
        DataTable dt = (DataTable)ViewState["Favorites"];

        if (dt.Rows.Count > 0)
        {
            if (s != "")
            {
                DataView dv = new DataView();
                dv = new DataView(dt);
                dv.RowFilter = "ServiceName like '%" + s + "%'";
                if (dv.Count > 0)
                {
                    gvFavorites.DataSource = dv.ToTable();
                    gvFavorites.DataBind();
                }
                else
                {
                    BindBlankFavGrid();

                }
            }
            else
            {
                gvFavorites.DataSource = dt;
                gvFavorites.DataBind();
            }

        }
        else
        {
            BindBlankFavGrid();

        }

    }


    private void BindBlankFavGrid()
    {
        DataTable dt = CreateFavItemTable();
        DataRow dr = dt.NewRow();

        try
        {
            dr["ServiceName"] = string.Empty;
            dr["ServiceId"] = 0;
            dr["DepartmentId"] = 0;
            dr["SubDeptId"] = 0;
            dr["ServiceType"] = string.Empty;
            dr["LabType"] = string.Empty;
            dr["IsStatOrderAllowed"] = string.Empty;
            dr["isServiceRemarkMandatory"] = string.Empty;

            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvFavorites.DataSource = dt;
            gvFavorites.DataBind();

            foreach (GridViewRow gr in gvFavorites.Rows)
            {
                ImageButton ibtnDelete1 = (ImageButton)gr.FindControl("ibtnDelete1");
                ibtnDelete1.Enabled = false;
                CheckBox chkRow = (CheckBox)gr.FindControl("chkRow");
                chkRow.Enabled = false;
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
            dt.Dispose();
        }
    }

    protected DataTable CreateFavItemTable()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("ServiceName", typeof(string));
            dt.Columns.Add("ServiceId", typeof(int));
            dt.Columns.Add("DepartmentId", typeof(int));
            dt.Columns.Add("SubDeptId", typeof(int));
            dt.Columns.Add("ServiceType", typeof(string));
            dt.Columns.Add("LabType", typeof(string));
            dt.Columns.Add("IsStatOrderAllowed", typeof(string));
            dt.Columns.Add("isServiceRemarkMandatory", typeof(string));
            dt.Columns.Add("ServiceDurationId", typeof(int));
            dt.Columns.Add("StationId", typeof(int));

            return dt;
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            return dt;
        }
        finally
        {
            dt.Dispose();
        }

    }
    /// <summary>
    /// Gridview Order rowcomands
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvorder_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("OrderLIST"))
            {

                string SetID;
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                SetID = ((HiddenField)row.FindControl("hdnProblemId")).Value.ToString();
                // ddldrpOrder.SelectedValue = gvorder.SelectedRow.Cells[1].ToString();

                //rdoOrder.SelectedValue = "OS";
                //visible(hdnServiceType.Value);
                AddOrder("OS", common.myInt(0), common.myInt(SetID));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }

    protected void btnAddRequest_Click(object sender, EventArgs e)
    {

        //StringBuilder strXML = new StringBuilder();
        //HiddenField hdnIsTemplateRequired = new HiddenField();
        //hdnIsTemplateRequired.Value = string.Empty;
        //HiddenField hdnServiceID = new HiddenField();
        //hdnServiceID.Value = string.Empty;

        //HiddenField hdnUpdateOrderDtlId = new HiddenField();

        //foreach (GridViewRow row in gvPatientServiceDetail.Rows)
        //{
        //    hdnIsTemplateRequired = (HiddenField)row.FindControl("hdnIsTemplateRequired");
        //    hdnServiceID = (HiddenField)row.FindControl("hdnServiceID");

        //    hdnUpdateOrderDtlId.Value = ((HiddenField)row.FindControl("hdnOrderDtlId")).Value;

        //    if (hdnIsTemplateRequired.Value.Equals("1") || hdnIsTemplateRequired.Value.Equals("2"))
        //    {
        //        strXML.Append("<Table1><c1>" + common.myInt(hdnUpdateOrderDtlId.Value) + "</c1></Table1>");
        //    }
        //}

        RadWindow2.NavigateUrl = "~/EMR/Templates/Default.aspx?Source=ProcedureOrder&TemplateRequiredServices=&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&TagType=D";

        RadWindow2.Height = 625;
        RadWindow2.Width = 1060;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        //RadWindow2.OnClientClose = "OnClientClose";
        RadWindow2.Modal = true;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow2.VisibleStatusbar = false;
        RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void btnRequestList_Click(object sender, EventArgs e)
    {// Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["EncounterId"]),
        RadWindow2.NavigateUrl = "~/EMR/Orders/RequestList.aspx?RegNo=" + common.myStr(Session["RegistrationNo"]);
        RadWindow2.Height = 625;
        RadWindow2.Width = 1050;
        RadWindow2.Top = 40;
        RadWindow2.Left = 100;
        //RadWindow2.OnClientClose = "OnClientClose";
        // RadWindow2.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow2.Modal = true;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow2.VisibleStatusbar = false;
    }

    protected void btnAddRequiredTemplate_Click(object sender, EventArgs e)
    {
        if (gvPatientServiceDetail.Rows.Count > 0)
        {
            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();
            HiddenField hdnIsTemplateRequired = new HiddenField();
            hdnIsTemplateRequired.Value = string.Empty;
            HiddenField hdnServiceID = new HiddenField();
            hdnServiceID.Value = string.Empty;

            HiddenField hdnUpdateOrderDtlId = new HiddenField();

            foreach (GridViewRow row in gvPatientServiceDetail.Rows)
            {
                hdnIsTemplateRequired = (HiddenField)row.FindControl("hdnIsTemplateRequired");
                hdnServiceID = (HiddenField)row.FindControl("hdnServiceID");

                hdnUpdateOrderDtlId.Value = ((HiddenField)row.FindControl("hdnOrderDtlId")).Value;

                if (hdnIsTemplateRequired.Value.Equals("1") || hdnIsTemplateRequired.Value.Equals("2"))
                {
                    coll.Add(common.myInt(hdnUpdateOrderDtlId.Value));
                    strXML.Append(common.setXmlTable(ref coll));
                }
            }
            if (strXML.ToString().Length > 0)
            {
                RadWindow2.NavigateUrl = "~/EMR/Templates/Default.aspx?Source=ProcedureOrder&TemplateRequiredServices=" + common.myStr(strXML.ToString() + "&RegNo=" + common.myStr(Session["RegistrationNo"])) + "&TagType=S";
                RadWindow2.Height = 625;
                RadWindow2.Width = 1060;
                RadWindow2.Top = 10;
                RadWindow2.Left = 10;
                //RadWindow2.OnClientClose = "OnClientClose";
                RadWindow2.Modal = true;
                RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindow2.VisibleStatusbar = false;
                RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
            }
        }
    }
    private int checkForTemplateRequiredService()
    {
        int isExist = 0;
        foreach (GridViewRow row in gvPatientServiceDetail.Rows)
        {
            HiddenField hdnIsTemplateRequired = (HiddenField)row.FindControl("hdnIsTemplateRequired");
            if (hdnIsTemplateRequired.Value.Equals("1") || hdnIsTemplateRequired.Value.Equals("2"))
            {
                isExist = 1;
                break;
            }
        }
        return isExist;
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        dvRedirect.Visible = false;
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        return;
    }


    protected void lnkServiceName_OnClick(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        LinkButton lnkServiceName = sender as LinkButton;
        string sServiceId = lnkServiceName.CommandArgument;
        BaseC.clsPharmacy bC = new BaseC.clsPharmacy(sConString);

        if (sServiceId == "0" || sServiceId == "")
            return;

        ds = bC.GetPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["RegistrationID"]),
            "", "", "");

        if (ds.Tables[0].Rows.Count > 0)
        {
            RadWindow2.NavigateUrl = "~/EMR/Orders/ServiceProfile.aspx?ServiceID=" + sServiceId
                + "&Age=" + ds.Tables[0].Rows[0]["Age"].ToString() + "&Gender=" + ds.Tables[0].Rows[0]["PatientGender"].ToString()
                + "&AgeType=" + ds.Tables[0].Rows[0]["AgeType"].ToString();
            RadWindow2.Height = 615;
            RadWindow2.Width = 900;
            RadWindow2.Top = 10;
            RadWindow2.Left = 10;
            RadWindow2.VisibleOnPageLoad = true;
            RadWindow2.Modal = true;
            RadWindow2.VisibleStatusbar = false;
        }
    }

    protected void lnkAddInvestigationSpecification_OnClick(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            LinkButton lnkServiceName = sender as LinkButton;
            int serviceId = common.myInt(lnkServiceName.CommandArgument);

            if (serviceId == 0)
            {
                return;
            }

            if (gvPatientServiceDetail.Rows.Count > 0)
            {
                StringBuilder strXML = new StringBuilder();
                HiddenField hdnIsTemplateRequired = new HiddenField();
                hdnIsTemplateRequired.Value = string.Empty;
                HiddenField hdnServiceID = new HiddenField();
                hdnServiceID.Value = string.Empty;

                HiddenField hdnUpdateOrderDtlId = new HiddenField();

                foreach (GridViewRow row in gvPatientServiceDetail.Rows)
                {
                    hdnIsTemplateRequired = (HiddenField)row.FindControl("hdnIsTemplateRequired");
                    hdnServiceID = (HiddenField)row.FindControl("hdnServiceID");

                    //hdnUpdateOrderDtlId.Value = ((HiddenField)row.FindControl("hdnOrderDtlId")).Value;

                    if (common.myInt(hdnServiceID.Value) > 0)
                    {
                        if (hdnIsTemplateRequired.Value.Equals("1") || hdnIsTemplateRequired.Value.Equals("2"))
                        {
                            if (strXML.ToString() != "")
                            {
                                strXML.Append(",");
                            }

                            strXML.Append(common.myInt(hdnServiceID.Value).ToString());
                        }
                    }
                }
                if (strXML.ToString().Length > 0)
                {
                    RadWindow2.NavigateUrl = "~/EMR/Templates/Default.aspx?Source=ProcedureOrder&TemplateRequiredServices=" + common.myStr(strXML.ToString() + "&RegNo=" + common.myStr(Session["RegistrationNo"])) + "&TagType=S";
                    RadWindow2.Height = 625;
                    RadWindow2.Width = 1060;
                    RadWindow2.Top = 10;
                    RadWindow2.Left = 10;
                    //RadWindow2.OnClientClose = "OnClientClose";
                    RadWindow2.Modal = true;
                    RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindow2.VisibleStatusbar = false;
                    RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
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
            ds.Dispose();
        }
    }

    private string BindEditor(Boolean sign)
    {
        if (Request.QueryString["ifId"] != "")
        {
            string sEREncounterId = common.myStr(Session["EncounterId"]);

            StringBuilder sbTemplateStyle = new StringBuilder();
            DataSet ds = new DataSet();
            DataSet dsTemplate = new DataSet();
            DataSet dsTemplateStyle = new DataSet();
            DataRow drTemplateStyle = null;
            DataTable dtTemplate = new DataTable();
            Hashtable hst = new Hashtable();
            string Templinespace = "";
            BaseC.DiagnosisDA fun;

            int RegId = common.myInt(Session["RegistrationID"]);
            int HospitalId = common.myInt(Session["HospitalLocationID"]);
            int EncounterId = common.myInt(Session["EncounterId"]);
            int UserId = common.myInt(Session["UserID"]);

            BindNotes bnotes = new BindNotes(sConString);
            fun = new BaseC.DiagnosisDA(sConString);

            string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));
            //ds = bnotes.GetPatientEMRData(common.myInt(Session["encounterid"]));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            clsIVF objivf = new clsIVF(sConString);

            dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, sEREncounterId);
            DataView dvTemplate = dsTemplate.Tables[0].DefaultView;

            dvTemplate.RowFilter = "Sequence=1 OR (TemplateId>0 AND ShowInOrderPage=1)";

            dtTemplate = dvTemplate.ToTable();

            sb.Append("<span style='" + Fonts + "'>");

            string strTemplatePatient = "0";

            for (int i = 0; i < dtTemplate.Rows.Count; i++)
            {
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Complaints"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10003))
                    {
                        bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                       Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                       "", "", "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>" + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                   && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                        || (common.myInt(strTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                    {
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Allergies"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    drTemplateStyle = null;// = dv[0].Row;
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10002))
                    {
                        bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                                   common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[i]["PageID"]), "", "", 0, "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Vitals"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10001))
                    {
                        bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                            Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                            "",
                                            "", 0, sEREncounterId, "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>" + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";

                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
                    && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                        || (common.myInt(strTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                    {
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Lab Test Result"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    // bnotes.BindLabTestResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
                    sb.Append(sbTemp);
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diagnosis"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10004))
                    {
                        bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                                    DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "", 0, sEREncounterId, "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Provisional Diagnosis"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10004))
                    {
                        bnotes.BindPatientProvisionalDiagnosis(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                                    DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "", "", 0, sEREncounterId, "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Prescription"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);

                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10005))
                    {
                        bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle,
                                       Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                       "",
                                       "", Session["OPIP"].ToString(), ""
                                       );

                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Current Medication"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10005))
                    {
                        bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                                        Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                        "",
                                        "", Session["OPIP"].ToString(), "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Order"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10006))
                    {
                        bnotes.BindOrders(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId,
                                Convert.ToInt16(UserId), DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                "",
                                "", sEREncounterId, "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Immunization"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10007))
                    {
                        bnotes.BindImmunization(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
                                    common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "", "",
                                    "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Doctor Progress Note"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10007))
                    {
                        bnotes.BindDoctorProgressNote(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
                                    common.myInt(Session["DoctorId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "", "",
                                    "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                        || (common.myInt(strTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                    {
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }

                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Daily Injections"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10008))
                    {
                        bnotes.BindInjection(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
                                    common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "", "",
                                    "");

                        sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                sb.Append("</span>");
            }
            drTemplateStyle = dsTemplateStyle.Tables[0].Rows[0];

            //StringBuilder temp = new StringBuilder();
            //bnotes.GetEncounterFollowUpAppointment(Session["HospitalLocationId"].ToString(), common.myInt(Session["EncounterId"]), temp, sbTemplateStyle, drTemplateStyle, Page);
            //sb.Append(temp);





            //if (sign == true)
            //{
            //    sb.Append(hdnDoctorImage.Value);
            //}
            //else if (sign == false)
            //{
            //    if (RTF1.Content != null)
            //    {
            //        if (RTF1.Content.Contains("dvDoctorImage") == true)
            //        {
            //            string signData = RTF1.Content.Replace('"', '$');
            //            string st = "<div id=$dvDoctorImage$>";
            //            int start = signData.IndexOf(@st);
            //            if (start > 0)
            //            {
            //                int End = signData.IndexOf("</div>", start);
            //                StringBuilder sbte = new StringBuilder();
            //                sbte.Append(signData.Substring(start, (End + 6) - start));
            //                StringBuilder ne = new StringBuilder();
            //                ne.Append(signData.Replace(sbte.ToString(), ""));
            //                sb.Append(ne.Replace('$', '"').ToString());
            //            }
            //        }

            //    }
            //}

        }

        return sb.ToString();
    }

    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != "" || common.myStr(item[typ + "FontSize"]) != "" || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sBegin += getDefaultFontSize(); }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            { sBegin += GetFontFamily(typ, item); }
        }

        if (common.myStr(item[typ + "Bold"]).ToUpper() == "TRUE")
        { sBegin += " font-weight: bold;"; }
        if (common.myStr(item[typ + "Italic"]).ToUpper() == "TRUE")
        { sBegin += " font-style: italic;"; }
        if (common.myStr(item[typ + "Underline"]).ToUpper() == "TRUE")
        { sBegin += " text-decoration: underline;"; }
        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        if (sBegin != "")
            sBegin += " '>";
    }

    protected void AddStr1(string type, string Saved_RTF_Content, StringBuilder sbTemp, string Lock)
    {
        //sbTemp.Append("<div id='" + type + "'><span style='color: Blue;'>");
        sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");
        //if (Lock == "0")
        //    sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");            
        //else
        //    sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");
    }

    protected void AddStr2(string type, ref string Saved_RTF_Content, StringBuilder sbTemp, string Lock, string Linespace, string ShowNote)
    {
        sbTemp.Append("</span></div>");
        if (common.myStr(sbTemp).Length > 49)
        {
            if (Linespace != "")
            {
                int ls = common.myInt(Linespace);
                for (int i = 1; i <= ls; i++)
                {
                    sbTemp.Append("<br/>");
                }
            }
            else
            {
                sbTemp.Append("<br />");
            }
        }
        if (Saved_RTF_Content == "" || Saved_RTF_Content == null)
        {
            if (common.myStr(sbTemp).Length > 62)  //if (sbTemp.ToString().Length > 68)
                sb.Append(common.myStr(sbTemp));
        }
        else
        {
            //change
            Saved_RTF_Content += sbTemp.ToString();

            //if (sbTemp.ToString().Length > 62)//if (sbTemp.ToString().Length > 68)
            //{
            if (ShowNote == "True" && (ViewState["PullForward"] == null || ViewState["DefaultTemplate"] == null))
            {
                Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
            }
            else if (ViewState["PullForward"] == null || ViewState["DefaultTemplate"] == null)
            {
                Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
            }
            else if (common.myStr(ViewState["DefaultTemplate"]) != "")
            {
                if (common.myStr(ViewState["DefaultTemplate"]).ToUpper() == "TRUE")
                {
                    Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
                }
            }
            else if (common.myStr(ViewState["PullForward"]) != "")
            {
                if (common.myStr(ViewState["PullForward"]).ToUpper() == "TRUE")
                {
                    Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
                }
            }
        }

    }
    public StringBuilder BindProblemsROS(int HospitalId, int EncounterId, StringBuilder sb, String sDisplayName, String sTemplateName, string pageID)
    {
        DataSet ds;
        DataSet dsGender;
        string strGender = "He";
        Hashtable hstInput = new Hashtable();
        Hashtable hsGender = new Hashtable();
        Hashtable hsProblems = new Hashtable();
        DataTable dtPositiveRos = new DataTable();
        DataTable dtNegativeRos = new DataTable();
        objStrTmp = new StringBuilder();
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //rafat
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        //hstInput.Add("@intTemplateId", GetTemplateId("ROS", common.myInt(Session["HospitalLocationID"])));
        hstInput.Add("@intTemplateId", common.myInt(ViewState["PageId"]));
        if (common.myInt(Session["Gender"]) == 1)
            hstInput.Add("chrGenderType", "F");
        else if (common.myInt(Session["Gender"]) == 2)
            hstInput.Add("chrGenderType", "M");
        hstInput.Add("@intFormId", common.myStr(1));
        DataSet dsFont = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        //string BeginList = "", EndList = "", BeginList2 = "", EndList2 = "", BeginList3 = "", EndList3 = "";
        DataRow drFont = dsFont.Tables[0].Rows[0] as DataRow;


        hsGender.Add("@intRegistrationId", Session["RegistrationID"]);
        string SqlQry = " Select dbo.GetGender(GENDER) as 'Gender' from registration where Id = @intRegistrationId";
        dsGender = DlObj.FillDataSet(CommandType.Text, SqlQry, hsGender);
        if (dsGender.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(dsGender.Tables[0].Rows[0]["Gender"]) == "Male")
                strGender = "He";
            else
                strGender = "She";
        }
        //Review Of Systems

        hsProblems.Add("@intEncounterId", EncounterId);
        //hsProblems.Add("@intTemplateId", GetTemplateId("ROS", common.myInt(Session["HospitalLocationID"])));
        hsProblems.Add("@intTemplateId", common.myInt(ViewState["PageId"]));
        ds = new DataSet();
        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientROS", hsProblems);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dv1 = new DataView(ds.Tables[0]);
            dv1.RowFilter = "PositiveValue <> ''";
            dtPositiveRos = dv1.ToTable();

            DataView dv2 = new DataView(ds.Tables[0]);
            dv2.RowFilter = "NegativeValue <> ''";
            dtNegativeRos = dv2.ToTable();
            //Make font start

            if (common.myStr(drFont["TemplateBold"]) != "" || common.myStr(drFont["TemplateItalic"]) != "" || common.myStr(drFont["TemplateUnderline"]) != "" || common.myStr(drFont["TemplateFontSize"]) != "" || common.myStr(drFont["TemplateForecolor"]) != "" || common.myStr(drFont["TemplateListStyle"]) != "")
            {
                string sBegin = "", sEnd = "";
                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
                if (common.myBool(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(sBegin + drFont["TemplateName"].ToString() + sEnd);
                    objStrTmp.Append(sBegin + common.myStr(sDisplayName) + sEnd);
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
                //objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
            }
            else
            {
                if (common.myBool(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(drFont["TemplateName"].ToString());//Default Setting
                    objStrTmp.Append(common.myStr(sDisplayName));//Default Setting
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
            }

            // Make Font End

            //sb.Append("<u><Strong>Review of systems</Strong></u>");

        }

        // For Positive Symptoms
        if (dtPositiveRos.Rows.Count > 0)
        {
            string strSectionId = ""; // dtPositiveRos.Rows[0]["SectionId"].ToString();
            DataTable dt = new DataTable();
            for (int i = 0; i < dtPositiveRos.Rows.Count; i++)
            {

                DataRow dr = dtPositiveRos.Rows[i] as DataRow;
                if (common.myStr(dr["SectionId"]) != strSectionId)
                {
                    string sBegin = "", sEnd = "";
                    if (common.myStr(drFont["SectionsBold"]) != "" || common.myStr(drFont["SectionsItalic"]) != "" || common.myStr(drFont["SectionsUnderline"]) != "" || common.myStr(drFont["SectionsFontSize"]) != "" || common.myStr(drFont["SectionsForecolor"]) != "" || common.myStr(drFont["SectionsListStyle"]) != "")
                    {
                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (common.myBool(drFont["SectionDisplayTitle"]))   //19June2010
                        {
                            if (i == 0)
                            {
                                objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
                            }
                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            objStrTmp.Append("<br />" + "Positive Symptoms:");
                        }
                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                    }


                    if (common.myStr(dr["FieldsBold"]) != "" || common.myStr(dr["FieldsItalic"]) != "" || common.myStr(dr["FieldsUnderline"]) != "" || common.myStr(dr["FieldsFontSize"]) != "" || common.myStr(dr["FieldsForecolor"]) != "" || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " has ");
                    }
                    else
                        objStrTmp.Append(strGender + " has ");

                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " has ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtPositiveRos);
                    dv.RowFilter = "SectionId =" + common.myStr(dr["SectionId"]);
                    dt = dv.ToTable();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        if (j == dt.Rows.Count - 1)
                        {
                            if (dt.Rows.Count == 1)
                            {
                                objStrTmp.Append("" + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                objStrTmp.Append(" and " + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                            }
                        }
                        else
                            objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ", ");
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }

        // For Negative Symptoms
        if (dtNegativeRos.Rows.Count > 0)
        {
            //if (drFont["TemplateBold"].ToString() != "" || drFont["TemplateItalic"].ToString() != "" || drFont["TemplateUnderline"].ToString() != "" || drFont["TemplateFontSize"].ToString() != "" || drFont["TemplateForecolor"].ToString() != "" || drFont["TemplateListStyle"].ToString() != "")
            //{
            //    string sBegin = "", sEnd = "";
            //    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
            //    //objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
            //    //objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
            //}
            //else
            //{
            //    objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
            //}          
            string strSectionId = ""; // 
            DataTable dt = new DataTable();
            for (int i = 0; i < dtNegativeRos.Rows.Count; i++)
            {

                DataRow dr = dtNegativeRos.Rows[i] as DataRow;
                if (common.myStr(dr["SectionId"]) != strSectionId)
                {
                    string sBegin = "", sEnd = "";
                    if (common.myStr(drFont["SectionsBold"]) != "" || common.myStr(drFont["SectionsItalic"]) != "" || common.myStr(drFont["SectionsUnderline"]) != "" || common.myStr(drFont["SectionsFontSize"]) != "" || common.myStr(drFont["SectionsForecolor"]) != "" || common.myStr(drFont["SectionsListStyle"]) != "")
                    {

                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (common.myBool(drFont["SectionDisplayTitle"]))   //19June2010
                        {
                            if (i == 0)
                            {
                                objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
                            }
                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            objStrTmp.Append("<br />" + "Negative Symptoms:");
                        }
                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                    }


                    if (common.myStr(dr["FieldsBold"]) != "" || common.myStr(dr["FieldsItalic"]) != "" || common.myStr(dr["FieldsUnderline"]) != "" || common.myStr(dr["FieldsFontSize"]) != "" || common.myStr(dr["FieldsForecolor"]) != "" || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " does not have ");
                    }
                    else
                        objStrTmp.Append(strGender + " does not have ");



                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " does not have ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtNegativeRos);
                    dv.RowFilter = "SectionId =" + common.myInt(dr["SectionId"]);
                    dt = dv.ToTable();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        if (j == dt.Rows.Count - 1)
                        {
                            if (dt.Rows.Count == 1)
                            {
                                objStrTmp.Append("" + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                objStrTmp.Append(" or " + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                            }
                        }
                        else
                            objStrTmp.Append(common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ", ");
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }
        sb.Append(objStrTmp);
        //sb.Append("<br/>");
        if (ds.Tables[0].Rows.Count > 0)
        {
            Hashtable hshtable = new Hashtable();
            StringBuilder sbDisplayName = new StringBuilder();
            BaseC.Patient bc = new BaseC.Patient(sConString);
            hshtable.Add("@intTemplateId", pageID);
            hshtable.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            string strDisplayUserName = "select DisplayUserName from EMRTemplate where ID=@intTemplateId and HospitalLocationID=@inyHospitalLocationID";
            DataSet dsDisplayName = DlObj.FillDataSet(CommandType.Text, strDisplayUserName, hshtable);
            if (dsDisplayName.Tables[0].Rows.Count > 0)
            {
                if (common.myStr(dsDisplayName.Tables[0].Rows[0]["DisplayUserName"]).ToUpper() == "TRUE")
                {
                    Hashtable hshUser = new Hashtable();
                    hshUser.Add("@UserID", common.myInt(ds.Tables[0].Rows[0]["EncodedBy"]));
                    hshUser.Add("@inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
                    string strUser = "Select ISNULL(FirstName,'') + '' + ISNULL(MiddleName,'') + '' + ISNULL(LastName,'') AS EmployeeName  FROM Employee em INNER JOIN Users us ON em.ID=us.EmpID WHERE us.ID=@UserID and em.HospitalLocationId=@inyHospitalLocationID";

                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    DataSet dsUser = dl.FillDataSet(CommandType.Text, strUser, hshUser);
                    DataTable dt = dsUser.Tables[0];
                    DataRow dr = dt.Rows[0];
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        sb.Append("<br/>");
                        string sUBegin = "", sUEnd = "";
                        MakeFontWithoutListStyle("Sections", ref sUBegin, ref sUEnd, drFont);
                        sbDisplayName.Append(sUBegin + "Entered and Verified by " + common.myStr(dsUser.Tables[0].Rows[0]["EmployeeName"]) + " " + common.myStr(Convert.ToDateTime(ds.Tables[0].Rows[0]["EncodedDate"]).Date.ToString("MMMM dd yyyy")));
                    }
                    sb.Append(sbDisplayName);
                }
            }
        }
        return sb;
    }


    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", common.myStr(Session["HospitalLocationId"]));
        if (FieldValue != "")
        {
            sFontSize = fonts.GetFont("Size", FieldValue);
            if (sFontSize != "")
                sFontSize = " font-size: " + sFontSize + ";";
        }
        return sFontSize;
    }

    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FontName = fonts.GetFont("Name", common.myStr(item[typ + "FontStyle"]));
        if (FontName != "")
            sBegin += " font-family: " + FontName + ";";
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myStr(Session["HospitalLocationId"]));
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                    sBegin += " font-family: " + FontName + ";";
            }
        }

        return sBegin;
    }

    protected void Replace(string Ttype, ref string t, string strNew, string Lock)
    {

        //if (t != null)
        //{
        //    t = t.Replace('"', '$');
        //    //if (Lock == "0")
        //    //{

        //    string st = "<div id=$" + Ttype + "$>";
        //    int RosSt = t.IndexOf(st);
        //    if (RosSt > 0 || RosSt == 0)
        //    {
        //        int RosEnd = t.IndexOf("</div>", RosSt);

        //        //// string str = t.Substring(RosSt, (RosEnd + 6) - RosSt);
        //        //string str = t.Substring(RosSt, (RosEnd) - RosSt);
        //        //string ne = t.Replace(str, strNew);
        //        //t = ne.Replace('$', '"');


        //        if ((RosEnd - RosSt) < (strNew.Length))
        //        {
        //            if ((RosEnd - RosSt) < (strNew.Length))
        //            {
        //                // string str = t.Substring(RosSt, (RosEnd + 6) - RosSt);
        //                string str = t.Substring(RosSt, (RosEnd) - RosSt);
        //                string ne = t.Replace(str, strNew);
        //                t = ne.Replace('$', '"');
        //            }
        //            //else
        //            //{
        //            //    StringBuilder  strOld = new StringBuilder();
        //            //    StringBuilder strNew1 = new StringBuilder();
        //            //    strOld.Append(t, RosSt, RosEnd);
        //            //    strOld.AppendLine(strNew);
        //            //}
        //        }
        //        else if ((RosEnd - RosSt) > (strNew.Length))
        //        {
        //            // No Action Performed (No Replacement)
        //            t = t.Replace('$', '"');
        //        }
        //    }
        //    else
        //    {
        //        //string st2 = "<div id='" + Ttype + "'>";
        //        //int RosSt2 = t.IndexOf(st2);
        //        //if (RosSt2 > 0)
        //        //{
        //        //    int RosEnd2 = t.IndexOf("</div>", RosSt2);
        //        //    string str2 = t.PadRight(20).Substring(RosSt2, (RosEnd2) - RosSt2);
        //        //    string ne2 = t.Replace(str2, strNew);
        //        //    //t = ne2.Replace('$', '"');
        //        //}
        //        //else
        //        t += strNew; // re-activated on 28 Feb 2011 by rafat
        //        t = t.Replace('$', '"');
        //    }

        //    //}
        //    //else
        //    //{
        //    //    string st = "<div id=$" + Ttype + "$><span style=$color: #000000;$>";
        //    //    int RosSt = t.IndexOf(st);

        //    //    string ne = t.Replace("<div id=$" + Ttype + "$><span style=$color: #000000;$>", "<div id=$" + Ttype + "$><span style=$color: #000000;$>");
        //    //    t = ne.Replace('$', '"');
        //}
        //// }

        if (t != null)
        {
            t = t.Replace('"', '$');
            //if (Lock == "0")
            //{
            string st = "<div id=$" + Ttype + "$>";
            int RosSt = t.IndexOf(st);
            if (RosSt > 0 || RosSt == 0)
            {
                int RosEnd = t.IndexOf("</div>", RosSt);
                // string str = t.Substring(RosSt, (RosEnd + 6) - RosSt);
                string str = t.Substring(RosSt, (RosEnd) - RosSt);
                string ne = t.Replace(str, strNew);
                t = ne.Replace('$', '"');
            }
            else
            {
                //Remarks - Case will not happen because all templates <div> tag is inserted at the time of creating encounter

            }
            //}
            //else
            //{
            //    string st = "<div id=$" + Ttype + "$><span style=$color: #000000;$>";
            //    int RosSt = t.IndexOf(st);

            //    string ne = t.Replace("<div id=$" + Ttype + "$><span style=$color: #000000;$>", "<div id=$" + Ttype + "$><span style=$color: #000000;$>");
            //    t = ne.Replace('$', '"');
            //}
        }

        t = t.Replace('$', '"');
    }
    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {

        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != "" || common.myStr(item[typ + "FontSize"]) != "" || common.myStr(item[typ + "FontStyle"]) != "")
        {
            if (common.myStr(item[typ + "FontSize"]) != "")
            { sFontSize += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sFontSize += getDefaultFontSize(); }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            { sFontSize += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            { sFontSize += GetFontFamily(typ, item); };

            if (common.myStr(item[typ + "Bold"]).ToUpper() == "TRUE")
            { sFontSize += " font-weight: bold;"; }
            if (common.myStr(item[typ + "Italic"]).ToUpper() == "TRUE")
            { sFontSize += " font-style: italic;"; }
            if (common.myStr(item[typ + "Underline"]).ToUpper() == "TRUE")
            { sFontSize += " text-decoration: underline;"; }

        }
        return sFontSize;
    }

    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType, string sectionId, string EntryType, int RecordId)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        StringBuilder objStr = new StringBuilder();
        DataView objDv = null;
        DataTable objDt = null;
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        if (objDs != null)
        {
            if (bool.Parse(TabularType) == true)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = objDs.Tables[0].Rows[0];
                    DataView dvValues = new DataView(objDs.Tables[1]);
                    dvValues.RowFilter = "FieldId='" + common.myStr(dr["FieldId"]) + "'";
                    MaxLength = dvValues.ToTable().Rows.Count;
                    DataTable dtFilter = dvValues.ToTable();

                    DataView dvFilter = new DataView(dtFilter);
                    dvFilter.RowFilter = "RowCaption='0'";
                    DataTable dtNewTable = dvFilter.ToTable();

                    if (dtNewTable.Rows.Count > 0)
                    {
                        if (MaxLength != 0)
                        {
                            if (EntryType != "M")
                            {
                                //objStr.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");


                                FieldsLength = objDs.Tables[0].Rows.Count;


                                if (common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != ""
                                    && common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != "0")
                                {
                                    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >&nbsp;</th>");

                                }
                                for (int i = 0; i < FieldsLength; i++)   // it makes table
                                {
                                    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");

                                    dr = objDs.Tables[0].Rows[i];
                                    dvValues = new DataView(objDs.Tables[1]);
                                    dvValues.RowFilter = "FieldId='" + dr["FieldId"].ToString() + "'";
                                    dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

                                    if (dvValues.ToTable().Rows.Count > MaxLength)
                                    {
                                        MaxLength = dvValues.ToTable().Rows.Count;
                                    }
                                }
                            }
                            objStr.Append("</tr>");
                            if (MaxLength == 0)
                            {
                                //objStr.Append("<tr>");
                                //for (int i = 0; i < FieldsLength; i++)
                                //{
                                //    objStr.Append("<td style=' " + sFontSize + "'> </td>");
                                //}
                                //objStr.Append("</tr></table>");
                            }
                            else
                            {
                                if (EntryType != "M")
                                {
                                    if (dsMain.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < MaxLength; i++)
                                        {
                                            objStr.Append("<tr>");
                                            if (common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) != ""
                                                && common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) != "0")
                                            {
                                                objStr.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) + "</td>");
                                            }
                                            //else
                                            //{
                                            //     objStr.Append("<td style=' " + sFontSize + "' align='right'>&nbsp;</td>");
                                            //}

                                            for (int j = 0; j < dsMain.Tables.Count; j++)
                                            {
                                                if (dsMain.Tables[j].Rows.Count > i
                                                    && dsMain.Tables[j].Rows.Count > 0)
                                                {
                                                    objStr.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dsMain.Tables[j].Rows[i]["TextValue"]) + "</td>");
                                                }
                                                else
                                                {
                                                    objStr.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Hashtable hstInput = new Hashtable();
                                    hstInput.Add("@intTemplateId", iRootId);

                                    if (common.myInt(Session["Gender"]) == 1)
                                    {
                                        hstInput.Add("chrGenderType", "F");
                                    }
                                    else if (common.myInt(Session["Gender"]) == 2)
                                    {
                                        hstInput.Add("chrGenderType", "M");
                                    }
                                    else
                                    {
                                        hstInput.Add("chrGenderType", "M");
                                    }

                                    hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
                                    hstInput.Add("@intSecId", Convert.ToInt32(sectionId));
                                    hstInput.Add("@intRecordId", RecordId);
                                    hstInput.Add("@intEncodedBy", common.myInt(Session["UserId"]));
                                    DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                                    DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);
                                    StringBuilder sbCation = new StringBuilder();

                                    if (dvRowCaption.ToTable().Rows.Count > 0)
                                    {
                                        dvRowCaption.RowFilter = "RowNum>0";
                                        DataTable dt = dvRowCaption.ToTable();
                                        if (dt.Rows.Count > 0)
                                        {
                                            sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                            int column = dt.Columns.Count;
                                            int ColumnCount = 0;
                                            int count = 1;

                                            //Added by rakesh because caption tabular template showing last column missiong start
                                            for (int k = 1; k < (column - 5); k++)
                                            //Added by rakesh because caption tabular template showing last column missiong start
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                                sbCation.Append("</td>");
                                                count++;
                                                ColumnCount++;
                                            }
                                            sbCation.Append("</tr>");

                                            DataView dvRow = new DataView(dt);
                                            DataTable dtRow = dvRow.ToTable();
                                            for (int l = 1; l <= dtRow.Rows.Count - 3; l++)
                                            {
                                                sbCation.Append("<tr>");
                                                for (int i = 1; i < ColumnCount + 1; i++)
                                                {
                                                    if (dt.Rows[1]["Col" + i].ToString() == "D")
                                                    {
                                                        DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                        if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                        {
                                                            dvDrop.RowFilter = "ValueId=" + dt.Rows[l + 1]["Col" + i].ToString();
                                                            if (dvDrop.ToTable().Rows.Count > 0)
                                                            {
                                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
                                                            }
                                                            else
                                                            {
                                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                }
                                                sbCation.Append("</tr>");
                                            }
                                        }
                                        sbCation.Append("</table>");
                                    }
                                    objStr.Append(sbCation);
                                }

                            }
                        }
                    }
                    else
                    {
                        Hashtable hstInput = new Hashtable();
                        hstInput.Add("@intTemplateId", iRootId);

                        if (common.myInt(Session["Gender"]) == 1)
                        {
                            hstInput.Add("chrGenderType", "F");
                        }
                        else if (common.myInt(Session["Gender"]) == 2)
                        {
                            hstInput.Add("chrGenderType", "M");
                        }
                        else
                        {
                            hstInput.Add("chrGenderType", "M");
                        }

                        hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
                        hstInput.Add("@intSecId", Convert.ToInt32(sectionId));
                        DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                        DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);

                        dvRowCaption.RowFilter = "RowCaptionId>0";
                        if (dvRowCaption.ToTable().Rows.Count > 0)
                        {
                            StringBuilder sbCation = new StringBuilder();
                            dvRowCaption.RowFilter = "RowNum>0";
                            DataTable dt = dvRowCaption.ToTable();
                            if (dt.Rows.Count > 0)
                            {
                                sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                int column = dt.Columns.Count;
                                int ColumnCount = 0;
                                int count = 1;
                                //Commented by rakesh because caption tabular template showing last column missiong start
                                //for (int k = 1; k < (column - 5); k++)
                                //Commented by rakesh because caption tabular template showing last column missiong start

                                //Added by rakesh because caption tabular template showing last column missiong start
                                for (int k = 1; k < (column - 4); k++)
                                //Added by rakesh because caption tabular template showing last column missiong start
                                {
                                    if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
                                        && ColumnCount == 0)
                                    {
                                        sbCation.Append("<td>");
                                        sbCation.Append(" + ");
                                        sbCation.Append("</td>");
                                    }
                                    else
                                    {
                                        sbCation.Append("<td>");
                                        sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                        sbCation.Append("</td>");
                                        count++;
                                    }
                                    ColumnCount++;
                                }
                                sbCation.Append("</tr>");

                                DataView dvRow = new DataView(dt);
                                dvRow.RowFilter = "RowCaptionId>0";
                                DataTable dtRow = dvRow.ToTable();
                                for (int l = 1; l <= dtRow.Rows.Count; l++)
                                {
                                    sbCation.Append("<tr>");
                                    for (int i = 0; i < ColumnCount; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dt.Rows[l + 1]["RowCaptionName"]) + "</td>");
                                        }
                                        else
                                        {
                                            if (dt.Rows[1]["Col" + i].ToString() == "D")
                                            {
                                                DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    dvDrop.RowFilter = "ValueId=" + dt.Rows[l + 1]["Col" + i].ToString();
                                                    if (dvDrop.ToTable().Rows.Count > 0)
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                            else
                                            {
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
                                                }
                                                else
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                        }

                                    }
                                    sbCation.Append("</tr>");
                                }
                                sbCation.Append("</table>");
                            }
                            objStr.Append(sbCation);
                        }

                    }
                }
            }
            else // For Non Tabular Templates
            {
                string BeginList = "", EndList = "";
                string sBegin = "", sEnd = "";
                int t = 0;
                string FieldId = "";
                string sStaticTemplate = "";
                string sEnterBy = "";
                string sVisitDate = "";
                foreach (DataRow item in objDs.Tables[0].Rows)
                {
                    objDv = new DataView(objDs.Tables[1]);
                    objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                    objDt = objDv.ToTable();
                    if (t == 0)
                    {
                        t = 1;
                        if (common.myStr(item["FieldsListStyle"]) == "1")
                        {
                            BeginList = "<ul>"; EndList = "</ul>";
                        }
                        else if (item["FieldsListStyle"].ToString() == "2")
                        {
                            BeginList = "<ol>"; EndList = "</ol>";
                        }
                    }
                    if (common.myStr(item["FieldsBold"]) != ""
                        || common.myStr(item["FieldsItalic"]) != ""
                        || common.myStr(item["FieldsUnderline"]) != ""
                        || common.myStr(item["FieldsFontSize"]) != ""
                        || common.myStr(item["FieldsForecolor"]) != ""
                        || common.myStr(item["FieldsListStyle"]) != "")
                    {
                        //rafat1
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);
                            if (Convert.ToBoolean(item["DisplayTitle"]))
                            {
                                // if (EntryType != "M")
                                // {
                                objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                //}
                                //else
                                //{
                                //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                //}
                                // 28/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                if (objStr.ToString() != "")
                                {
                                    objStr.Append(sEnd + "</li>");
                                }
                            }
                            BeginList = "";
                            sBegin = "";
                        }

                    }
                    else
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            if (sStaticTemplate != "<br/><br/>")
                            {
                                objStr.Append(common.myStr(item["FieldName"]));
                            }
                        }
                    }
                    if (objDs.Tables.Count > 1)
                    {

                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        objDt = objDv.ToTable();
                        DataView dvFieldType = new DataView(objDs.Tables[0]);
                        dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                        for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                        {
                            //if (EntryType == "M")
                            //{
                            //    objStr.Append("<br/>" + BeginList + sBegin + common.myStr(item["FieldName"]));
                            //}
                            if (objDt.Rows.Count > 0)
                            {
                                //objStr.Append(": " + objDt.Rows[0]["PropertyValue"].ToString());
                                string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                if (FType == "C")
                                {
                                    FType = "C";
                                }
                                if (FType == "C" || FType == "D" || FType == "B")
                                {
                                    if (FType == "B")
                                    {
                                        if (common.myStr(objDt.Rows[i]["FieldValue"]) == "1"
                                            || common.myStr(objDt.Rows[i]["FieldValue"]) == "0")
                                        {

                                            DataView dv1 = new DataView(objDs.Tables[1]);
                                            if (common.myStr(objDs.Tables[1].Rows[i]["FieldValue"]) == "1")
                                            {
                                                dv1.RowFilter = "TextValue='Yes'";
                                            }
                                            else
                                            {
                                                dv1.RowFilter = "TextValue='No'";
                                            }

                                            DataTable dt1 = dv1.ToTable();
                                            if (dt1.Rows.Count > 0)
                                            {
                                                if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != "")
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                    else
                                                    {
                                                        //if (EntryType == "M")
                                                        //{
                                                        //objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        //}
                                                        //else
                                                        objStr.Append(", " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                    else
                                                    {
                                                        //if (EntryType == "M")
                                                        //{
                                                        // objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        //}
                                                        //else
                                                        objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    //if (EntryType == "M")
                                                    //{
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    //}
                                                    //else
                                                    // objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            BindDataValue(objDs, objDt, objStr, i, FType);//check1
                                        }
                                    }
                                    else
                                    {
                                        BindDataValue(objDs, objDt, objStr, i, FType);
                                    }
                                }
                                else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            //if (EntryType == "M")
                                            //{
                                            //objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            //}
                                            //else
                                            //{
                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            // }
                                        }
                                    }
                                    else
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            //if (EntryType == "M")
                                            //{
                                            //objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            //}
                                            //else

                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                }
                                else if (FType == "L")
                                {
                                    objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
                                }
                                if (common.myStr(item["FieldsListStyle"]) == "")
                                {
                                    if (ViewState["iTemplateId"].ToString() != "163")
                                    {
                                        if (FType != "C")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                    else
                                    {
                                        if (FType != "C" && FType != "T")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                }
                            }
                            sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
                            sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
                            //if (EntryType == "M" && sEnterBy != "" && sVisitDate != "")
                            //{
                            //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
                            //}
                        }


                        // Cmt 25/08/2011
                        //if (objDt.Rows.Count > 0)
                        //{
                        //    if (objStr.ToString() != "")
                        //        objStr.Append(sEnd + "</li>");
                        //}
                    }

                    //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
                }

                if (objStr.ToString() != "")
                {
                    objStr.Append(EndList);
                }
            }
        }

        return objStr.ToString();
    }

    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            {
                sBegin += "<br/>";
            }
            else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            {
                sBegin += "; ";
            }
            else
            {
                sBegin += "<br/>";
            }
        }


        if (common.myStr(item[typ + "Forecolor"]) != "" || common.myStr(item[typ + "FontSize"]) != "" || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sBegin += getDefaultFontSize(); }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            { sBegin += GetFontFamily(typ, item); }
        }
        if (common.myStr(item[typ + "Bold"]).ToUpper() == "TRUE")
        { sBegin += " font-weight: bold;"; }
        if (common.myStr(item[typ + "Italic"]).ToUpper() == "TRUE")
        { sBegin += " font-style: italic;"; }
        if (common.myStr(item[typ + "Underline"]).ToUpper() == "TRUE")
        { sBegin += " text-decoration: underline;"; }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
            sBegin += " '>";

    }

    protected DataSet GetPageProperty(string iFormId)
    {
        if (Session["HospitalLocationID"] != null && iFormId != "")
        {
            if (Cache[common.myStr(Session["HospitalLocationID"]) + "_" + iFormId + "_FormPageSettings"] == null)
            {
                hstInput = new Hashtable();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hstInput.Add("@intFormId", 1);
                DataSet ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                return ds;
            }
            else
            {
                DataSet objDs = (DataSet)Cache[common.myStr(Session["HospitalLocationID"]) + "_" + iFormId + "_FormPageSettings"];
                return objDs;
            }
        }
        return null;
    }
    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType)
    {
        //DataView dv1 = new DataView(objDs.Tables[1]);
        //dv1.RowFilter = "ValueId='" + objDt.Rows[i]["FieldValue"].ToString() + "'";
        //DataTable dt1 = dv1.ToTable();
        //if (dt1.Rows[0]["MainText"].ToString().Trim() != "")
        //{
        //    if (i == 0)
        //        objStr.Append(" " + dt1.Rows[i]["MainText"].ToString());
        //    else
        //    {
        //        if (FType != "C")
        //            objStr.Append(", " + dt1.Rows[i]["MainText"].ToString());
        //        else
        //        {
        //            if (i == 0)
        //                objStr.Append(" " + dt1.Rows[i]["MainText"].ToString());
        //            else if (i + 1 == objDs.Tables[2].Rows.Count)
        //                objStr.Append(" and " + dt1.Rows[i]["MainText"].ToString() + ".");
        //            else
        //                objStr.Append(", " + dt1.Rows[i]["MainText"].ToString());
        //        }
        //    }
        //}
        //else
        //{
        if (i == 0)
            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
        else
        {
            if (FType != "C")
                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
            else
            {
                if (i == 0)
                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                    objStr.Append(" and " + common.myStr(objDt.Rows[i]["TextValue"]) + ".");
                else
                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
            }
        }
        //}
    }

    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        StringBuilder sbStatic = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        BaseC.DiagnosisDA fun;

        int RegId = common.myInt(Session["RegistrationID"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(Session["encounterid"]);
        int UserId = common.myInt(Session["UserID"]);

        BindNotes bnotes = new BindNotes(sConString);
        fun = new BaseC.DiagnosisDA(sConString);

        string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));

        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

        dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, "0");
        DataView dvFilterStaticTemplate = new DataView(dsTemplate.Tables[0]);
        dvFilterStaticTemplate.RowFilter = "PageId=" + StaticTemplateId;
        dtTemplate = dvFilterStaticTemplate.ToTable();

        sb.Append("<span style='" + Fonts + "'>");

        if (dtTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Allergies"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                drTemplateStyle = null;// = dv[0].Row;
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAllergies(RegId, sbStatic, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                            common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[0]["PageID"]), "", "", TemplateFieldId, "");

                // sb.Append(sbTemp + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";
            }
            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Vitals"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbStatic, sbTemplateStyle, drTemplateStyle,
                                    Page, common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "", TemplateFieldId, "0", "");

                //sb.Append(sbTemp + "<br/>" + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";

            }

            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Diagnosis"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                            DoctorId, sbStatic, sbTemplateStyle, drTemplateStyle, Page,
                            common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
                            "",
                            "", TemplateFieldId, "0", "");

                //sb.Append(sbTemp + "<br/>");

                drTemplateStyle = null;
                Templinespace = "";
            }
            //sb.Append("</span>");
        }
        return "<br/>" + sbStatic.ToString();
    }

    protected void bindData(string iFormId, string TemplateId, StringBuilder sb)
    {
        string str = "";
        string sEntryType = "V";
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();

        hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }

        hstInput.Add("@intFormId", "1");
        hstInput.Add("@bitDischargeSummary", 0);
        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            sEntryType = common.myStr(ds.Tables[0].Rows[0]["EntryType"]);
        }
        hstInput = new Hashtable();
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }
        else
        {
            hstInput.Add("chrGenderType", "M");
        }
        if (Request.QueryString["ER"] != null && Request.QueryString["ER"].ToString() == "ER")
        {
            hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
            hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
        }
        else
        {
            if (sEntryType == "S")
            {
                hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
            }
            else
            {
                hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
            }
        }

        hstInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

        //hstInput.Add("@intEREncounterId", Request.QueryString["EREncounterId"] == null ? Session["EREncounterId"].ToString() : Request.QueryString["EREncounterId"].ToString());
        hstInput.Add("@intEncodedBy", common.myInt(Session["UserId"]));
        DataSet dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);

        string BeginList = "", EndList = "", BeginList2 = "", BeginList3 = "", EndList3 = "";
        int t = 0, t2 = 0, t3 = 0;
        DataView dv = new DataView(dsAllSectionDetails.Tables[2]);

        //1234
        //if (txtFromDate.SelectedDate.Value != null && txtToDate.SelectedDate.Value != null)
        //{
        //    dv.RowFilter = "EntryDate>='" + Convert.ToDateTime(common.myDate(txtFromDate.SelectedDate.Value)).ToString("yyyy/MM/dd 00:00") +
        //        "' AND EntryDate<='" + Convert.ToDateTime(common.myDate(txtToDate.SelectedDate.Value)).ToString("yyyy/MM/dd 23:59")+"'";
        //}
        dv.Sort = "RecordId DESC";
        DataTable dtEntry = dv.ToTable(true, "RecordId");
        int iRecordId = 0;
        //dtEntry = dv.ToTable(true, "EntryDate");

        //string sEntryDate = "";

        for (int it = 0; it < dtEntry.Rows.Count; it++)
        {
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                DataTable dtFieldValue = new DataTable();
                DataView dv1 = new DataView(dsAllSectionDetails.Tables[0]);
                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                DataTable dtFieldName = dv1.ToTable();

                if (dsAllSectionDetails.Tables.Count > 2)
                {
                    DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);

                    // dv2.RowFilter = "EntryDate='" + dtEntry.Rows[it]["EntryDate"].ToString() + "'";
                    dv2.RowFilter = "RecordId=" + common.myStr(dtEntry.Rows[it]["RecordId"]);
                    dtFieldValue = dv2.ToTable();
                }

                DataSet dsAllFieldsDetails = new DataSet();
                dsAllFieldsDetails.Tables.Add(dtFieldName);
                dsAllFieldsDetails.Tables.Add(dtFieldValue);
                if (dsAllSectionDetails.Tables[0].Rows.Count > 0)
                {
                    if (dsAllSectionDetails.Tables.Count > 2)
                    {
                        if (dsAllSectionDetails.Tables[2].Rows.Count > 0) // add for education details
                        {
                            string sBegin = "", sEnd = "";

                            DataRow dr3;
                            dr3 = dsAllSectionDetails.Tables[0].Rows[0];

                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);

                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);
                            str = CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]), common.myStr(item["Tabular"]),
                                item["SectionId"].ToString(), common.myStr(item["EntryType"]), common.myInt(dtEntry.Rows[it]["RecordId"]));
                            str += " ";
                            if (sEntryType == "M" && str.Trim() != "")
                            {
                                str += "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + common.myStr(dtFieldValue.Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + ")</span>";
                            }
                            //}
                            if (iPrevId == common.myInt(item["TemplateId"]))
                            {
                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                {
                                    if (sEntryType == "M")
                                    {
                                        //objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                        objStrTmp.Append("<br/><br/><b>" + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                }
                                if (t2 == 0)
                                {
                                    if (t3 == 0)//Template
                                    {
                                        t3 = 1;
                                        if (common.myStr(item["SectionsListStyle"]) == "1")
                                        {
                                            BeginList3 = "<ul>"; EndList3 = "</ul>";
                                        }
                                        else if (common.myStr(item["SectionsListStyle"]) == "2")
                                        {
                                            BeginList3 = "<ol>"; EndList3 = "</ol>";
                                        }
                                    }
                                }

                                if (common.myStr(item["SectionsBold"]) != ""
                                    || common.myStr(item["SectionsItalic"]) != ""
                                    || common.myStr(item["SectionsUnderline"]) != ""
                                    || common.myStr(item["SectionsFontSize"]) != ""
                                    || common.myStr(item["SectionsForecolor"]) != ""
                                    || common.myStr(item["SectionsListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                        }
                                    }
                                    BeginList3 = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                        }
                                    }
                                }

                                if (common.myStr(item["SectionsListStyle"]) == "3"
                                    || common.myStr(item["TemplateListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }
                                else
                                {
                                    if (str.Trim() != "")
                                    {
                                        objStrTmp.Append(str);
                                    }
                                }
                            }
                            else
                            {

                                if (t == 0)
                                {
                                    t = 1;
                                    if (common.myStr(item["TemplateListStyle"]) == "1")
                                    {
                                        BeginList = "<ul>"; EndList = "</ul>";
                                    }
                                    else if (common.myStr(item["TemplateListStyle"]) == "2")
                                    {
                                        BeginList = "<ol>"; EndList = "</ol>";
                                    }
                                }
                                if (common.myStr(item["TemplateBold"]) != ""
                                    || common.myStr(item["TemplateItalic"]) != ""
                                    || common.myStr(item["TemplateUnderline"]) != ""
                                    || common.myStr(item["TemplateFontSize"]) != ""
                                    || common.myStr(item["TemplateForecolor"]) != ""
                                    || common.myStr(item["TemplateListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    {
                                        if (sBegin.Contains("<br/>") == true)
                                        {
                                            sBegin = sBegin.Remove(0, 5);
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                        else
                                        {
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                    }
                                    if (sEntryType == "M" && str.Trim() != "")
                                    {
                                        objStrTmp.Append("<br/><br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                    BeginList = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    {
                                        objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                    }
                                    if (sEntryType == "M" && str.Trim() != "")
                                    {
                                        // objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                        objStrTmp.Append("<br/><br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                }
                                if (common.myStr(item["TemplateListStyle"]) == "3"
                                    || common.myStr(item["TemplateListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }
                                objStrTmp.Append(EndList);
                                if (t2 == 0)
                                {
                                    t2 = 1;
                                    if (common.myStr(item["SectionsListStyle"]) == "1")
                                    {
                                        BeginList2 = "<ul>"; EndList3 = "</ul>";
                                    }
                                    else if (common.myStr(item["SectionsListStyle"]) == "2")
                                    {
                                        BeginList2 = "<ol>"; EndList3 = "</ol>";
                                    }
                                }
                                if (common.myStr(item["SectionsBold"]) != ""
                                    || common.myStr(item["SectionsItalic"]) != ""
                                    || common.myStr(item["SectionsUnderline"]) != ""
                                    || common.myStr(item["SectionsFontSize"]) != ""
                                    || common.myStr(item["SectionsForecolor"]) != ""
                                    || common.myStr(item["SectionsListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                    {
                                        if (str.Trim() != "") //add 19June2010
                                        {
                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                        }
                                    }
                                    BeginList2 = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                    {
                                        if (str.Trim() != "") //add 19June2010
                                        {
                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                        }
                                    }
                                }
                                if (common.myStr(item["SectionsListStyle"]) == "3"
                                    || common.myStr(item["SectionsListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }

                                objStrTmp.Append(str);
                            }
                            iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                            iPrevId = common.myInt(item["TemplateId"]);
                        }
                    }
                }
            }
        }

        if (t2 == 1 && t3 == 1)
        {
            objStrTmp.Append(EndList3);
        }
        else
        {
            objStrTmp.Append(EndList);
        }

        if (GetPageProperty(iFormId) != null)
        {
            objStrSettings.Append(objStrTmp.ToString());
            sb.Append(objStrSettings.ToString());
        }
        else
        {
            sb.Append(objStrTmp.ToString());
        }
    }

    protected void lnkLabHistory_OnClick(object sender, EventArgs e)
    {
        RadWindow2.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]);
        RadWindow2.Height = 600;
        RadWindow2.Width = 900;
        RadWindow2.Top = 20;
        RadWindow2.Left = 20;
        RadWindow2.OnClientClose = "";
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.Modal = true;
        RadWindow2.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow2.VisibleStatusbar = false;
    }

    protected void gvFavorites_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Find the checkbox control in header and add an attribute
            ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAllFavourite('" +
                ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //HiddenField hdnCalculationBase = (HiddenField)e.Row.FindControl("hdnCalculationBase");
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
            {

                LinkButton lnkFAV = (LinkButton)e.Row.FindControl("lnkFAV");
                LinkButton ibtnAddToList = (LinkButton)e.Row.FindControl("ibtnAddToList");
                CheckBox chkRow = (CheckBox)e.Row.FindControl("chkRow");
                ImageButton ibtnDelete1 = (ImageButton)e.Row.FindControl("ibtnDelete1");

                ibtnAddToList.Enabled = false;
                lnkFAV.Enabled = false;
                chkRow.Enabled = false;
                ibtnDelete1.Enabled = false;

            }
        }
    }

    protected void btnProceedFavourite_OnClick(object sender, EventArgs e)
    {
        try
        {
            int index = -1;
            foreach (GridViewRow gvrow in gvFavorites.Rows)
            {
                index = (int)gvFavorites.DataKeys[gvrow.RowIndex].Value;
                HiddenField hdnFAvID = (HiddenField)gvrow.FindControl("hdnFAvID");
                bool result = ((CheckBox)gvrow.FindControl("chkRow")).Checked;
                if (result)
                {
                    if (IsExistServiceOnSameDay(common.myInt(index)))
                    {
                        dvConfirmAlreadyExistOptions.Visible = true;
                        return;
                    }
                }
            }
            proceedFavourite(true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //private void proceedFavourite()
    //{
    //    try
    //    {
    //        foreach (GridViewRow dataItem in gvFavorites.Rows)
    //        {
    //            CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
    //            if (chkRow.Checked)
    //            {
    //                HiddenField hdnFAvID = (HiddenField)dataItem.FindControl("hdnFAvID");
    //                if (common.myInt(hdnFAvID.Value) > 0)
    //                {
    //                    //LinkButton lnkFAV = (LinkButton)dataItem.FindControl("lnkFAV");
    //                    //HiddenField hdnDepartmentId = (HiddenField)dataItem.FindControl("hdnDepartmentId");
    //                    //HiddenField hdnSubDeptId = (HiddenField)dataItem.FindControl("hdnSubDeptId");
    //                    //HiddenField hdnType = (HiddenField)dataItem.FindControl("hdnType");
    //                    //HiddenField hdnLabType = (HiddenField)dataItem.FindControl("hdnLabType");

    //                    hdnServiceId.Value = hdnFAvID.Value.ToString();
    //                    AddOrder("FAV", common.myInt(hdnFAvID.Value), 0);
    //                }
    //                chkRow.Checked = false;
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}

    private void proceedFavourite(bool checkResult)
    {
        try
        {
            SaveCheckedValues(checkResult);
            ArrayList arrCheckedRecordServiceId = new ArrayList();
            string SelectedServiceId = string.Empty;

            if (Session["FavId"] != null)
            {
                arrCheckedRecordServiceId = (ArrayList)Session["FavId"];

                for (int i = 0; i <= arrCheckedRecordServiceId.Count - 1; i++)
                {
                    //  SelectedServiceId = SelectedServiceId + "," + common.myStr(arrCheckedRecordServiceId[i]);

                    // HiddenField hdnFAvID = (HiddenField)dataItem.FindControl("hdnFAvID");
                    if (common.myInt(arrCheckedRecordServiceId[i]) > 0)
                    {

                        hdnServiceId.Value = arrCheckedRecordServiceId[i].ToString();
                        AddOrder("FAV", common.myInt(arrCheckedRecordServiceId[i]), 0);
                    }
                    //chkRow.Checked = false;


                }
            }
            foreach (GridViewRow gvrow in gvFavorites.Rows)
            {
                CheckBox chkRow = (CheckBox)gvrow.FindControl("chkRow");
                chkRow.Checked = false;
            }
            Session["CHECKED_ITEMS"] = null;
            //Session["FavId"] = null;

            //binddrplist("FV");


        }

        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //private void proceedFavourite()
    //{
    //    try
    //    {
    //        // PopulateCheckedValues();
    //        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
    //        if (userdetails != null && userdetails.Count > 0)
    //        {
    //            foreach (GridViewRow gvrow in gvFavorites.Rows)
    //            {
    //                int index = (int)gvFavorites.DataKeys[gvrow.RowIndex].Value;
    //                CheckBox chkRow = (CheckBox)gvrow.FindControl("chkRow");
    //                if (userdetails.Contains(index))
    //                {
    //                    chkRow.Checked = true;
    //                }
    //                if (chkRow.Checked)
    //                {
    //                    HiddenField hdnFAvID = (HiddenField)gvrow.FindControl("hdnFAvID");
    //                    if (common.myInt(hdnFAvID.Value) > 0)
    //                    {
    //                        hdnServiceId.Value = hdnFAvID.Value.ToString();
    //                        AddOrder("FAV", common.myInt(hdnFAvID.Value), 0);
    //                    }
    //                    chkRow.Checked = false;
    //                }

    //            }
    //        }
    //        //foreach (GridViewRow dataItem in gvFavorites.Rows)
    //        //{
    //        //    CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
    //        //    if (chkRow.Checked)
    //        //    {
    //        //        HiddenField hdnFAvID = (HiddenField)dataItem.FindControl("hdnFAvID");
    //        //        if (common.myInt(hdnFAvID.Value) > 0)
    //        //        {
    //        //            hdnServiceId.Value = hdnFAvID.Value.ToString();
    //        //            AddOrder("FAV", common.myInt(hdnFAvID.Value), 0);
    //        //        }
    //        //        chkRow.Checked = false;
    //        //    }
    //        //}


    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}


    //private void PopulateCheckedValues()
    //{
    //    ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
    //    if (userdetails != null && userdetails.Count > 0)
    //    {
    //        foreach (GridViewRow gvrow in gvFavorites.Rows)
    //        {
    //            int index = (int)gvFavorites.DataKeys[gvrow.RowIndex].Value;
    //            if (userdetails.Contains(index))
    //            {
    //                CheckBox chkRow = (CheckBox)gvrow.FindControl("chkRow");
    //                chkRow.Checked = true;
    //            }
    //        }
    //    }
    //}



    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //try catch and finally added by sikandar for code optimization
        StringBuilder sb = new StringBuilder();

        try
        {
            if (RadDateTimePicker1.SelectedDate == null)
            {
                RadDateTimePicker1.SelectedDate = DateTime.Now;
            }

            sb.Append(RadDateTimePicker1.SelectedDate.Value.ToString());
            sb.Remove(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
            sb.Insert(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
            RadDateTimePicker1.SelectedDate = Convert.ToDateTime(sb.ToString());
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            sb = null;
        }
    }


    protected void btnAlredyExistProceed_OnClick(object sender, EventArgs e)
    {
        try
        {

            proceedFavourite(false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        addServices();
        dvConfirmAlreadyExistOptions.Visible = false;
    }

    protected void btnAlredyExistCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmAlreadyExistOptions.Visible = false;
    }
    void bindPatientDetails(Int64 RegNo, string EncNo)
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet dsPatientDetail = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(RegNo),
                    common.myStr(EncNo), common.myInt(Session["UserId"]), 0);

        if (dsPatientDetail.Tables[0].Rows.Count > 0)
        {
            string sRegNoTitle = Resources.PRegistration.regno;
            string sDoctorTitle = Resources.PRegistration.Doctor;
            string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date :" : "Encounter Date :";
            lblPatientDetail.Text = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["GenderAge"]) + "</span>"
             + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
             + "&nbsp;Enc #:" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]) + "</span>"
             + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"
             + DateTitle + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(Session["EncounterDate"]) + "</span>"
             + "&nbsp;Bed:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["BedNo"]) + "</span>"
             + "&nbsp;Ward:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["WardName"]) + "</span>"
             + "&nbsp;Mobile:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
             + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]) + "</span>"
             + "</b>";
        }
    }

    public void ReturnToCaller()
    {

        string strPopup = "<script language='javascript' ID='script1'>" + "window.close();" + "</script>";

        ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        return;

        ////ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        ////return;


    }
    protected void gvFavorites_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            SaveCheckedValues(true);
            // proceedFavourite();
            // gvFavoritesSeletedData();
            gvFavorites.PageIndex = e.NewPageIndex;
            binddrplist("FV");
            PopulateCheckedValues();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    private void gvFavoritesSeletedData()
    {
        DataTable dt = new DataTable();
        DataTable dtFavoritesSeletedData = new DataTable();
        ArrayList arrCheckedRecordServiceId = new ArrayList();
        string SelectedServiceId = string.Empty;

        if (ViewState["Favorites"] != null)
        {
            dt = (DataTable)ViewState["Favorites"];
        }
        DataView dv = new DataView();
        if (Session["FavId"] != null)
        {
            arrCheckedRecordServiceId = (ArrayList)Session["FavId"];
        }
        try
        {

            for (int i = 0; i <= arrCheckedRecordServiceId.Count - 1; i++)
            {
                SelectedServiceId = SelectedServiceId + "," + common.myStr(arrCheckedRecordServiceId[i]);
            }
            if (SelectedServiceId.StartsWith(","))
            {
                SelectedServiceId = SelectedServiceId.Substring(1);
            }
            dv = dt.DefaultView;
            dv.RowFilter = "ServiceId in (" + SelectedServiceId + ")";
            dtFavoritesSeletedData = dv.ToTable();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvFavorites.Rows)
            {
                int index = (int)gvFavorites.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox chkRow = (CheckBox)gvrow.FindControl("chkRow");
                    chkRow.Checked = true;
                }
            }
        }
    }
    //This method is used to save the checkedstate of values
    private void SaveCheckedValues(bool checkResult)
    {
        ArrayList userdetails = new ArrayList();
        ArrayList arrFavId = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvFavorites.Rows)
        {
            index = (int)gvFavorites.DataKeys[gvrow.RowIndex].Value;
            HiddenField hdnFAvID = (HiddenField)gvrow.FindControl("hdnFAvID");
            bool result = ((CheckBox)gvrow.FindControl("chkRow")).Checked;


            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (Session["FavId"] != null)
                arrFavId = (ArrayList)Session["FavId"];
            if (result)
            {
                if (checkResult)
                    if (IsExistServiceOnSameDay(common.myInt(index)))
                    {
                        dvConfirmAlreadyExistOptions.Visible = true;

                        return;
                    }
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                if (common.myInt(hdnFAvID.Value) > 0)
                {
                    arrFavId.Add(common.myInt(hdnFAvID.Value));
                }
            }
            else
            {
                userdetails.Remove(index);
                arrFavId.Remove(common.myInt(hdnFAvID.Value));
            }
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
        if (arrFavId != null && arrFavId.Count > 0)
            Session["FavId"] = arrFavId;
    }

    protected void gvorder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        try
        {
            gvorder.PageIndex = e.NewPageIndex;
            BindDefaultControls();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnProceed_OnClick(object sender, EventArgs e)
    {
        ViewState["Yes"] = true;
        addServices();

        RadDateTimePicker1.SelectedDate = null;
        cmbServiceName.Text = "";
        cmbServiceName.Items.Clear();
        cmbServiceName.ClearSelection();
        txtInstruction.Text = string.Empty;
        chkFreeTest.Checked = false;

        divConfirmation.Visible = false;
        ViewState["Yes"] = null;
    }
    protected void btnProceedCancel_OnClick(object sender, EventArgs e)
    {
        divConfirmation.Visible = false;
    }

    public void addServiceFromLinkService(int MainServiceId, double mainServiceAmt, int CompanyId, int InsuranceId, int CardId)
    {
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        BaseC.EMROrders objEMROrders = new BaseC.EMROrders(sConString);
        Hashtable hshServiceDetail = new Hashtable();
        DataTable dt = new DataTable();
        DataTable dtLinkServies = new DataTable();
        bool IsPercentage = false;
        double Percentage = 0.0;
        double PercentageAmt = 0.0;
        decimal totPayable = 0;
        int doctorId = 0;
        try
        {
            if (Request.QueryString["For"] != null && common.myInt(Request.QueryString["DoctorId"]) > 0)
            {
                doctorId = common.myInt(Request.QueryString["DoctorId"]);
            }
            else
            {
                doctorId = common.myInt(Session["EmployeeId"]);
            }

            dtLinkServies = objEMROrders.getLinkServiceSetupDetails(common.myInt(Session["FacilityId"]), MainServiceId);

            if (ViewState["GridData"] != null)
            {
                dt = (DataTable)ViewState["GridData"];
            }

            if (dt.Rows.Count > 0)
            {
                if (common.myInt(dt.Rows[0]["ServiceId"]).Equals(0)) //If serviceid = 0 then remove row
                {
                    dt.Rows.Clear();
                }
            }

            foreach (DataRow dtr in dtLinkServies.Rows)
            {
                DataRow dr = dt.NewRow();

                IsPercentage = common.myBool(dtr["IsPercentage"]);
                Percentage = common.myDbl(dtr["Percentage"]);

                hshServiceDetail = new Hashtable();
                hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                                            common.myInt(Session["FacilityId"]),
                                            common.myInt(CompanyId),
                                            common.myInt(InsuranceId),
                                            common.myInt(CardId),
                                            common.myStr(Session["OPIP"]),
                                            common.myInt(dtr["ServiceId"]),
                                            common.myInt(Session["RegistrationId"]),
                                            common.myInt(Session["EncounterId"]), 0, 0, 0,
                                            Convert.ToDateTime(RadDateTimePicker1.SelectedDate).ToString("yyyy-MM-dd"));

                dr["ServiceId"] = common.myInt(dtr["ServiceId"]);
                dr["PackageId"] = 0;
                dr["DoctorID"] = doctorId;

                dr["ServiceType"] = common.myStr(hshServiceDetail["ServiceType"]);
                dr["ServiceName"] = common.myStr(dtr["ServiceName"]);
                dr["Units"] = 1;

                if (IsPercentage)
                {
                    if (Percentage > 0)
                    {
                        PercentageAmt = (mainServiceAmt * (Percentage / 100.00));
                    }

                    dr["Charges"] = common.myDec(PercentageAmt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["IsExcluded"] = false;
                    dr["Stat"] = false;
                    dr["Urgent"] = false;
                }
                else
                {
                    dr["Charges"] = common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["IsExcluded"] = common.myBool(hshServiceDetail["IsExcluded"]);
                    dr["Stat"] = common.myStr(chkStat.SelectedValue) == "STAT" ? 1 : 0;
                    dr["Urgent"] = common.myStr(chkStat.SelectedValue) == "URGENT" ? 1 : 0;
                }

                dr["AlertRequired"] = false;
                dr["AlertMessage"] = string.Empty;
                dr["ICDID"] = string.Empty;
                dr["FacilityId"] = common.myStr(Session["FacilityId"]);
                dr["Remarks"] = string.Empty;
                dr["LabStatus"] = "Not Billed";
                dr["OrderId"] = 0;
                dr["EncodedBy"] = 0;
                dr["PlanTypeId"] = 0;
                dr["CompanyId"] = CompanyId;
                dr["ID"] = 0;
                dr["CPTCode"] = string.Empty;
                dr["RequestToDepartment"] = false;
                dr["isServiceRemarkMandatory"] = false;
                dr["FreeTest"] = false;
                dr["result"] = 0;
                dr["TestDate"] = common.myStr(DateTime.Now);//DBNull.Value;
                dr["DoctorRequired"] = common.myStr(hshServiceDetail["DoctorRequired"]);
                dr["IsBioHazard"] = common.myBool(chkIsBioHazard.Checked);
                //dr["AssignToEmpId"] = common.myInt(ddlAssignToEmpId.SelectedValue); //StationId

                dt.Rows.Add(dr);
            }

            ViewState["GridData"] = dt;
            gvPatientServiceDetail.DataSource = dt;
            gvPatientServiceDetail.DataBind();

            lblMessage.Text = string.Empty;

            //Declare an object variable. 
            object sumObject;
            sumObject = dt.Compute("Sum(Charges)", "");
            lblTotCharges.Text = common.myStr(sumObject);
            chkIsBioHazard.Checked = false;

            ddlAssignToEmpId.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            BaseBill = null;
            objEMROrders = null;
            hshServiceDetail = null;
            dtLinkServies.Dispose();
            dt.Dispose();
        }
    }


    protected void BindServiceDuration()
    {
        clsIVF obj = new clsIVF(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = obj.GetStatusMaster("ServiceDuration");

            ddlServiceDuration.DataSource = ds.Tables[0];
            ddlServiceDuration.DataTextField = "TypeName";
            ddlServiceDuration.DataValueField = "TypeId";
            ddlServiceDuration.DataBind();

            ddlServiceDuration.Items.Insert(0, new ListItem("", "0"));
        }
        catch (Exception)
        {
        }
        finally
        {
            obj = null;
            ds.Dispose();
        }
    }


    protected void ibtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtData = null;
        DataTable dtService = null;

        try
        {

            ImageButton ibtnEdit = (ImageButton)sender;
            GridViewRow gvr = (GridViewRow)ibtnEdit.NamingContainer;
            HiddenField hdnServiceID = (HiddenField)gvr.FindControl("hdnServiceID");

            if (hdnServiceID != null)
            {
                cmbServiceName.Text = string.Empty;
                cmbServiceName.ClearSelection();
                cmbServiceName.Items.Clear();

                //----------------------------Code to Fill services in dropdownlist starts------------------------------------------------------------
                dtService = BindSearchCombo(string.Empty);

                DataView dv = new DataView(dtService);
                dv.RowFilter = "ServiceId=" + common.myInt(hdnServiceID.Value);

                int indx = common.myInt(gvr.RowIndex);
                ViewState["EditIndx"] = indx;

                if (dv.ToTable().Rows.Count > 0)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dv[0]["ServiceName"];
                    item.Value = dv[0]["ServiceID"].ToString();
                    item.Attributes["CPTCode"] = dv[0]["CPTCode"].ToString();
                    item.Attributes["LongDescription"] = dv[0]["LongDescription"].ToString();
                    item.Attributes["ServiceType"] = dv[0]["ServiceType"].ToString();
                    item.Attributes["IsStatOrderAllowed"] = dv[0]["IsStatOrderAllowed"].ToString();
                    item.Attributes["isServiceRemarkMandatory"] = dv[0]["isServiceRemarkMandatory"].ToString();
                    item.Attributes["IsLinkService"] = dv[0]["IsLinkService"].ToString();

                    this.cmbServiceName.Items.Add(item);
                    item.DataBind();
                }
                //----------------------------Code to Fill services in dropdownlist ends------------------------------------------------------------

                //----------------------------Code to Fill record based on serviceId  starts------------------------------------------------------------
                hdnServiceId.Value = hdnServiceID.Value;

                dtData = (DataTable)ViewState["GridData"];

                DataView dvdup = new DataView();
                dvdup = dtData.Copy().DefaultView;
                dvdup.RowFilter = " ServiceId = " + common.myStr(hdnServiceID.Value);


                txtInstruction.Text = common.myStr(dvdup[0]["Remarks"]);
                string stat = common.myStr(dvdup[0]["STAT"]);
                string urgent = common.myStr(dvdup[0]["URGENT"]);
                if (common.myBool(stat))
                {
                    chkStat.SelectedIndex = 0;
                }
                if (common.myBool(urgent))
                {
                    chkStat.SelectedIndex = 1;
                }
                chkIsBioHazard.Checked = common.myBool(dvdup[0]["IsBiohazard"]);
                chkFreeTest.Checked = common.myBool(dvdup[0]["FreeTest"]);
                if (common.myDate(dvdup[0]["TestDate"]) != null)
                {
                    RadDateTimePicker1.SelectedDate = common.myDate(dvdup[0]["TestDate"]);
                }
                else
                {
                    RadDateTimePicker1.SelectedDate = null;
                }
                if (!common.myStr(dvdup[0]["Units"]).Equals(string.Empty))
                {
                    txtUnit.Text = common.myStr(dvdup[0]["Units"]);
                }
                else
                {
                    txtUnit.Text = common.myStr(1);
                }
                ddlServiceDuration.SelectedIndex = ddlServiceDuration.Items.IndexOf(ddlServiceDuration.Items.FindByValue(common.myInt(dvdup[0]["ServiceDurationId"]).ToString()));

                hdnServiceType.Value = common.myStr(dvdup[0]["ServiceType"]).ToString();
                hdnGlobleStationId.Value = common.myInt(dvdup[0]["StationId"]).ToString();

                bindAssignToDoctor();
                visible(common.myStr(hdnServiceType.Value));

                ddlAssignToEmpId.SelectedIndex = ddlAssignToEmpId.Items.IndexOf(ddlAssignToEmpId.Items.FindItemByValue(common.myInt(dvdup[0]["AssignToEmpId"]).ToString()));

                //----------------------------Code to Fill record based on serviceId  Ends------------------------------------------------------------

                //----------------------------Code to remove record based on serviceId  starts------------------------------------------------------------
                //DataView DV = new DataView();
                //DV = dtData.Copy().DefaultView;
                //DV.RowFilter = " ServiceId <> " + common.myStr(hdnServiceID.Value);

                //----------------------------Code to remove record based on serviceId  Ends------------------------------------------------------------
                //dtData = DV.ToTable();

                dtData.AcceptChanges();
                ViewState["GridData"] = dtData;
                if (dtData.Rows.Count > 0)
                {
                    gvPatientServiceDetail.DataSource = dtData;
                }
                else
                {
                    gvPatientServiceDetail.DataSource = CreateTable();
                }
                gvPatientServiceDetail.DataBind();
            }
        }
        catch (Exception Ex)
        {
        }
        finally
        {
        }
    }

    protected void btnOrderSet_OnClick(object sender, EventArgs e)
    {
        Session["OrderSetServiceIds"] = null;
        StringBuilder objXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        try
        {
            for (int rowIdx = 0; rowIdx < gvPatientServiceDetail.Rows.Count; rowIdx++)
            {
                HiddenField hdnServiceID = (HiddenField)gvPatientServiceDetail.Rows[rowIdx].FindControl("hdnServiceID");

                if (common.myInt(hdnServiceID.Value) > 0)
                {
                    coll.Add(common.myInt(hdnServiceID.Value));

                    objXML.Append(common.setXmlTable(ref coll));
                }
            }

            if (common.myLen(objXML.ToString()) > 0)
            {
                Session["OrderSetServiceIds"] = objXML.ToString();

                RadWindow2.NavigateUrl = "/EMR/Orders/AddOrderSet.aspx?For=Orders&DoctorId=" + common.myInt(Session["EmployeeId"]);

                RadWindow2.Height = 600;
                RadWindow2.Width = 800;
                RadWindow2.Top = 40;
                RadWindow2.Left = 100;
                RadWindow2.OnClientClose = "AddOrderSet_OnClientClose";
                RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                                                     //RadWindow2.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow2.Modal = true;
                RadWindow2.VisibleStatusbar = false;
            }
            else
            {
                Alert.ShowAjaxMsg("Please add services in list before make order set!", this.Page);
            }
        }
        catch
        {
        }
    }

    protected void btnAddOrderSetClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindDefaultControls();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}
