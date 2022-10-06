using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Collections;
using System.Web.UI.HtmlControls;

public partial class EMR_Masters_EMRTreatmentPlan : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 10;
    BaseC.DiagnosisDA objDiag;
    DataSet ds = new DataSet();
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;
            //gvTreatmentPlan.Visible = false;
            //gvService.Visible = false;
            if (!IsPostBack)
            {
                fillEMRTreatmentPlanTemplatesmaster();
                //GetServiceData();

                //fillEquipmentServiceTagging();


                bind_ddlBrand_PrescriptionsControl();
                GetServiceData(0);
                BindDrpCategory();
                BindSubCategory("");
                fillTextboxValues(0);
                // BindGrid();
                //BindBlankItemGrid();
                //BindBlankServiceGrid();

            }
            else
            {
                //  GetServiceData(0);

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void GetServiceData()
    {
        try
        {
            string ServiceName = "%";
            string strDepartmentType = "";

            strDepartmentType = "'EQ', 'O' ";

            BaseC.RestFulAPI objCommonService = new BaseC.RestFulAPI(sConString);
            DataSet ds = objCommonService.GetHospitalServices(common.myInt(Session["HospitallocationId"]), 0, 0,
                strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]), 0, 0);
            //ddlService.DataSource = ds.Tables[0];
            //ddlService.DataTextField = "ServiceName";
            //ddlService.DataValueField = "ServiceId";
            //ddlService.DataBind();

        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    protected void GetServiceData(int TemplatePlanId)
    {
        BaseC.Hospital bHos = new BaseC.Hospital(sConString);
        try
        {
            //gvTreatmentPlan.Visible = true;
            //gvService.Visible = true;
            string ServiceName = "%";
            string strDepartmentType = "";
            gvService.Dispose();
            gvTreatmentPlan.Dispose();

            gvService.DataSource = null;
            gvService.DataBind();

            gvTreatmentPlan.DataSource = null;
            gvTreatmentPlan.DataBind();
            //gvService.Dispose();
            //gvTreatmentPlan.Dispose();


            strDepartmentType = "'EQ', 'O' ";
            DataSet ds = bHos.GetEMRTreatmentPlanSpecialities(TemplatePlanId);
            if (ds.Tables[0].Rows.Count > 0)
            {

                gvService.DataSource = ds.Tables[0];
                gvService.DataBind();

                ViewState["FirstTimegvService"] = ds.Tables[0];

                //  ViewState["totalgvTreatmentPlan"] = ds.Tables[0];

                //   ViewState["FirstTimegvTreatmentPlan"] = ds.Tables[0];

            }
            else
            {


                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);

                gvService.DataSource = ds.Tables[0];
                gvService.DataBind();
                dr = null;

            }




            if (ds.Tables[1].Rows.Count > 0)
            {
                gvTreatmentPlan.DataSource = ds.Tables[1];
                gvTreatmentPlan.DataBind();

                ViewState["FirstTimegvTreatmentPlan"] = ds.Tables[1];

                //if (common.myStr(ViewState["totalgvTreatmentPlan"]) == string.Empty || common.myStr(ViewState["totalgvTreatmentPlan"]) == null)
                //{
                //    //       ViewState["totalgvTreatmentPlan"] = ds.Tables[1];
                //}





                ViewState["totalgvTreatmentPlanINdb"] = ds.Tables[1];// temporari


            }


            else
            {

                DataRow dr = ds.Tables[1].NewRow();
                ds.Tables[1].Rows.Add(dr);

                gvTreatmentPlan.DataSource = ds.Tables[1];
                gvTreatmentPlan.DataBind();
                dr = null;
                ViewState["totalgvTreatmentPlanINdb"] = ds.Tables[1];

            }

           
            if (ds.Tables.Count > 2)
            {
                if (ds.Tables[3].Rows.Count > 0)
                {
                    gvDiagnosisDetails.DataSource = ds.Tables[3];
                    gvDiagnosisDetails.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[3].NewRow();
                    ds.Tables[3].Rows.Add(dr);
                    gvDiagnosisDetails.DataSource = ds.Tables[3];
                    gvDiagnosisDetails.DataBind();
                    dr = null;
                }
            }
            gvDiagnosisDetails.Columns[3].Visible = false;
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void fillTextboxValues(int TemplatePlanId)
    {
        BaseC.Hospital bHos = new BaseC.Hospital(sConString);
        try
        {
            DataSet ds = bHos.GetEMRTreatmentPlanSpecialities(TemplatePlanId);
            if (ds.Tables[2].Rows.Count > 0)
            {
                txtWPlanOfCare.Text = common.myStr(ds.Tables[2].Rows[0]["PlanofCare"]);
                txtWPlanOfCare.Text = common.clearHTMLTags(txtWPlanOfCare.Text);


                txtTreatmentPlanInstructions.Text = common.myStr(ds.Tables[2].Rows[0]["Instructions"]);
                txtTreatmentPlanInstructions.Text = common.clearHTMLTags(txtTreatmentPlanInstructions.Text);

                txtChiefComplaint.Text = common.myStr(ds.Tables[2].Rows[0]["ChiefComplaints"]);
                txtChiefComplaint.Text = common.clearHTMLTags(txtChiefComplaint.Text);

                txtHistory.Text = common.myStr(ds.Tables[2].Rows[0]["History"]);
                txtHistory.Text = common.clearHTMLTags(txtHistory.Text);

                txtExamination.Text = common.myStr(ds.Tables[2].Rows[0]["Examination"]);
                txtExamination.Text = common.clearHTMLTags(txtExamination.Text);

                //txtDiagnosis.Text = common.myStr(ds.Tables[2].Rows[0]["Diagnosis"]);
                //txtDiagnosis.Text = common.clearHTMLTags(txtDiagnosis.Text);

                ddlDuration.SelectedIndex = ddlDuration.Items.IndexOf(ddlDuration.Items.FindItemByValue(common.myStr(ds.Tables[2].Rows[0]["ChiefComplaintsDuration"])));
                ddlDurationType.SelectedIndex = ddlDurationType.Items.IndexOf(ddlDurationType.Items.FindItemByValue(common.myStr(ds.Tables[2].Rows[0]["ChiefComplaintsDurationType"]).Trim()));

            }

        }
        catch(Exception Ex){
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally {
            bHos = null;
            
        }
    }
    protected void fillEMRTreatmentPlanTemplatesmaster()
    {
        try
        {
            BaseC.EMR objEMR = new BaseC.EMR(sConString);
            //DataSet ds = bHos.GetEMRTreatmentPlanTemplatesmaster(common.myInt(Session["FacilityId"]), 0);
            // DataSet ds = bHos.GetEMRTreatmentPlanTemplatesmaster(common.myInt(Session["FacilityId"]),common.myInt(Session["LoginDepartmentId"]), common.myInt(Session["EmployeeId"]));
            DataSet ds = objEMR.GetEMRTreatmentPlanTemplates(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationId"]));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlPlanTemplates.DataSource = ds.Tables[0];
                    ddlPlanTemplates.DataTextField = "PlanName";
                    ddlPlanTemplates.DataValueField = "PlanId";
                    ddlPlanTemplates.DataBind();
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
    protected void fillEquipmentServiceTagging()
    {
        try
        {
            BaseC.Hospital bHos = new BaseC.Hospital(sConString);
            DataSet ds = bHos.GetOTEquipmentmaster(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 1);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvTreatmentPlan.DataSource = ds.Tables[0];
                    gvTreatmentPlan.DataBind();
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
    //protected void lnkSelect_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        LinkButton lnkSelect = (LinkButton)sender;
    //        ddlService.Text = "";
    //      //////  ddlEquipment.Text = "";
    //        //hdnMEquipmentId.Value = ((HiddenField)lnkSelect.FindControl("hdnEquipmentId")).Value;
    //        //hdnMServiceId.Value = ((HiddenField)lnkSelect.FindControl("hdnServiceId")).Value;
    //      //////  ddlEquipment.SelectedIndex = ddlEquipment.Items.IndexOf(ddlEquipment.Items.FindItemByValue(((HiddenField)lnkSelect.FindControl("hdnEquipmentId")).Value));
    //        ddlService.SelectedIndex = ddlService.Items.IndexOf(ddlService.Items.FindItemByValue(((HiddenField)lnkSelect.FindControl("hdnServiceId")).Value));

    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}



    protected void lnkDelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkDelete = (LinkButton)sender;
            //ddlService.Text = "";
            //////  ddlEquipment.Text = "";
            // HiddenField hdnItemId= ((HiddenField)lnkDelete.FindControl("hdnItemID")).Value;
            //HiddenField  hdnTemplateId= ((HiddenField)lnkDelete.FindControl("hdnTemplateId")).Value;
            //  //////  ddlEquipment.SelectedIndex = ddlEquipment.Items.IndexOf(ddlEquipment.Items.FindItemByValue(((HiddenField)lnkSelect.FindControl("hdnEquipmentId")).Value));
            //  ddlService.SelectedIndex = ddlService.Items.IndexOf(ddlService.Items.FindItemByValue(((HiddenField)lnkDelete.FindControl("hdnItemID")).Value));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Equipment Details.", Page.Page);
                return;
            }
            //if (common.myInt(ddlService.SelectedValue) == 0)
            //{
            //    Alert.ShowAjaxMsg("Please! Select Service Details.", Page.Page);
            //    return;
            //}
            BaseC.Hospital bHos = new BaseC.Hospital(sConString);
            Hashtable hsOut = bHos.uspSaveOTEquipmentmaster(common.myInt(Session["HospitallocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ddlPlanTemplates.SelectedValue), 0, 1);
            if (common.myStr(hsOut["chvErrorStatus"]).ToString().ToUpper().Contains("UPDATED"))
            {
                Alert.ShowAjaxMsg(common.myStr(hsOut["chvErrorStatus"]).ToString(), Page.Page);
                fillEquipmentServiceTagging();
                ddlPlanTemplates.SelectedIndex = -1;
                //    ddlService.SelectedIndex = -1;
                ddlPlanTemplates.Text = "";
                //     ddlService.Text = "";
            }
            else
            {
                Alert.ShowAjaxMsg(common.myStr(hsOut["chvErrorStatus"]).ToString(), Page.Page);
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
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void ddlPlanTemplates_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
        GetServiceData(plantype);
        fillTextboxValues(plantype);
    }



    public void bind_ddlBrand_PrescriptionsControl()
    {
        DataTable data = new DataTable();
        DataSet ds = new DataSet();
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet dsSearch = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        try
        {



            ds = new DataSet();
            ds = objEMR.GetUnitMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            ddlUnit.DataSource = ds.Tables[0];
            ddlUnit.DataValueField = "Id";
            ddlUnit.DataTextField = "UnitName";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlUnit.SelectedIndex = 0;


            ds = new DataSet();
            ds = objPharmacy.getFrequencyMaster();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(dr["Description"]);
                item.Value = common.myStr(common.myInt(dr["Id"]));
                item.Attributes.Add("Frequency", common.myStr(dr["Frequency"]));
                item.DataBind();
                ddlFrequencyId.Items.Add(item);
            }
            ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlFrequencyId.SelectedIndex = 0;


            ds = new DataSet();
            ds = objPharmacy.GetFood(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1);
            ddlFoodRelation.DataSource = ds.Tables[0];
            ddlFoodRelation.DataValueField = "Id";
            ddlFoodRelation.DataTextField = "FoodName";
            ddlFoodRelation.DataBind();
            ddlFoodRelation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlFoodRelation.SelectedIndex = 0;

            //   BindGrid();


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            dsSearch.Dispose();
            objEMR = null;
        }



    }


    protected void ddlFrequencyId_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

    }

    protected void ddlBrand_Prescriptions_OnItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text.Equals(string.Empty) || e.Text.Length < 2)
        {
            return;
        }
        Telerik.Web.UI.RadComboBox ddl = sender as Telerik.Web.UI.RadComboBox;
        int GenericId = 0;
        DataTable data = new DataTable();

        data = GetBrandData_Prescription(e.Text, GenericId);
        //data = GetBrandData(e.Text, GenericId);
        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
        e.EndOfItems = endOffset.Equals(data.Rows.Count);
        for (int i = itemOffset; i < endOffset; i++)
        {
            Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
            item.Text = (string)data.Rows[i]["ItemName"];
            item.Value = common.myStr(data.Rows[i]["ItemId"]);

            item.Attributes.Add("ClosingBalance", "0");
            item.Attributes.Add("CIMSItemId", "");
            item.Attributes.Add("CIMSType", "");
            item.Attributes.Add("VIDALItemId", "0");



            this.ddlBrand_Prescriptions.Items.Add(item);
            item.DataBind();
        }


        ddlBrand_Prescriptions.DataSource = data;
        ddlBrand_Prescriptions.DataTextField = "ItemName";
        ddlBrand_Prescriptions.DataValueField = "ItemId";
        ddlBrand_Prescriptions.DataBind();
        //ddlBrand_Prescriptions.Items.Insert(0, new RadComboBoxItem("", "0"));
        //ddlBrand_Prescriptions.SelectedIndex = 0;



        e.Message = GetStatusMessage(endOffset, data.Rows.Count);




    }



    private DataTable GetBrandData_Prescription(string text, int GenericId)
    {
        DataSet dsSearch = new DataSet();
        DataTable dt = new DataTable();

        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

        try
        {
            int StoreId = common.myInt(Session["StoreId"]); //common.myInt(Session["StoreId"]);
            int ItemId = 0;

            int itemBrandId = 0;
            int WithStockOnly = 0;

            int iOT = 3;

            if (Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"].ToString() == "OT"
                && Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"].ToString() == "CO")
            {
                iOT = 2;
            }
            else if (Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"].ToString() == "WARD"
                && Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"].ToString() == "CO")
            {
                iOT = 2;
            }
            else
            {
                iOT = 1;
            }

            if (common.myDbl(ViewState["QtyBal"]) > 0
                   && common.myInt(Request.QueryString["ItemId"]) > 0)
            {
                ItemId = common.myInt(ViewState["ItemId"]);
            }

            dsSearch = objPharmacy.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId, ItemId == 0 && ViewState["ItemId"] != null ? Convert.ToInt32(ViewState["ItemId"]) : ItemId, itemBrandId, GenericId,
                common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), 0,
                text.Replace("'", "''"), WithStockOnly, string.Empty, iOT);

            if (dsSearch.Tables.Count > 0)
            {
                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    dt = dsSearch.Tables[0];
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
            objPharmacy = null;
        }

        return dt;
    }

    private DataTable GetBrandData(string text, int GenericId)
    {
        DataSet dsSearch = new DataSet();
        DataTable dt = new DataTable();
        BaseC.EMRAllergy objEMRAllergy = new BaseC.EMRAllergy(sConString);
        DataView DV = new DataView();
        try
        {
            dsSearch = objEMRAllergy.getAllergyItemList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    text.Replace("'", "''"), common.myInt(Session["UserId"]));
            DV = dsSearch.Tables[0].Copy().DefaultView;
            if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
            {
                DV.RowFilter = "AllergyType IN('CIMS','Food','Others')";
            }
            else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
            {
                DV.RowFilter = "AllergyType IN('VIDAL','Food','Others')";
            }
            else
            {
                DV.RowFilter = "AllergyType IN('Generic','Food','Others')";
            }
            dt = DV.ToTable().Copy();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMRAllergy = null;
            dsSearch.Dispose();
            DV.Dispose();
        }

        return dt;
    }

    //private static string GetStatusMessage(int offset, int total)
    //{
    //    if (total <= 0)
    //        return "No matches found...";
    //    return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    //}

    private void BindGrid()
    {
        int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
        GetServiceData(plantype);
    }

    protected void btnAddListSevice_Onclick(object sender, EventArgs e)
    {
        BaseC.EMR emr = new BaseC.EMR(sConString);
        BaseC.EMRMasters emrmaster = new BaseC.EMRMasters(sConString);



        try
        {
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Template Name.", Page.Page);
                return;
            }
            if (common.myInt(cmbServiceName.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Service Name.", Page.Page);
                return;
            }


            int TemplateId = common.myInt(ddlPlanTemplates.SelectedValue);
            int Brandid = 0;
            int FrequencyId = 0;
            string DayTypes = string.Empty;
            int Day = 0;
            decimal Dose = 0;
            int Unit = 0;
            int FoodRelation = 0;
            string Instructions = string.Empty; ;

            int ServiceId = common.myInt(cmbServiceName.SelectedValue);
            int EncodedBy = common.myInt(1);
            string Type = string.Empty;
            string StrMsg = emrmaster.SaveEMRTreatmentTemplate(TemplateId, Brandid, Dose, Unit, FrequencyId, Day, DayTypes, FoodRelation, Instructions, ServiceId, EncodedBy);

            if (StrMsg == string.Empty)
            {
                lblMessage.Text = "Investigations  Added !";
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
                GetServiceData(plantype);

            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = StrMsg.ToString();


            }



            //    ddlPlanTemplates.SelectedValue = "0";

            cmbServiceName.Text = string.Empty;

        }


        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


        finally
        {

        }
    }

    protected void btnAddList_Onclick(object sender, EventArgs e)
    {

        DataTable tblItem = new DataTable();
        DataView DVItem = new DataView();
        BaseC.EMR emr = new BaseC.EMR(sConString);
        BaseC.EMRMasters emrmaster = new BaseC.EMRMasters(sConString);


        if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Treatment Template Name.", Page.Page);
            return;
        }
        if (common.myInt(ddlBrand_Prescriptions.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Brand Name.", Page.Page);
            return;
        }
        if (common.myInt(ddlFrequencyId.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Frequency.", Page.Page);
            return;
        }
        if (common.myStr(txtDuration.Text.Trim()) == string.Empty)
        {
            Alert.ShowAjaxMsg("Please! Enter Duration.", Page.Page);
            return;
        }
        if (common.myStr(txtDose.Text.Trim()) != string.Empty)
        {
            if (common.myStr(ddlUnit.SelectedValue) == string.Empty)
            {
                Alert.ShowAjaxMsg("Please! Select Dose Unit.", Page.Page);
                return;
            }
            else
            {

            }


        }






        int TemplateId = common.myInt(ddlPlanTemplates.SelectedValue);
        int Brandid = common.myInt(ddlBrand_Prescriptions.SelectedValue);
        int FrequencyId = common.myInt(ddlFrequencyId.SelectedValue);
        string DayTypes = common.myStr(ddlPeriodType.SelectedValue);
        int Day = common.myInt(txtDuration.Text);
        decimal Dose = common.myDec(txtDose.Text);
        int Unit = common.myInt(ddlUnit.SelectedValue);
        int FoodRelation = common.myInt(ddlFoodRelation.SelectedValue);
        string Instructions = common.myStr(txtInstructions.Text);

        int ServiceId = common.myInt(cmbServiceName.SelectedValue);
        if (!ServiceId.Equals(0))
        {
            ServiceId = 0;
        }
        int EncodedBy = common.myInt(1);
        string Type = string.Empty;
        //        decimal dDuration = 0;

        //switch (common.myStr(ddlPeriodType.SelectedValue))
        //{
        //    case "N":
        //        DayTypes = " Minute(s)";
        //        break;
        //    case "H":
        //        DayTypes = " Hour(s)";

        //        break;
        //    case "D":
        //        DayTypes =" Day(s)";

        //        break;
        //    case "W":
        //        DayTypes =  " Week(s)";

        //        break;
        //    case "M":
        //        DayTypes =  " Month(s)";

        //        break;
        //    case "Y":
        //        DayTypes =  " Year(s)";

        //        break;
        //}

        string StrMsg = emrmaster.SaveEMRTreatmentTemplate(TemplateId, Brandid, Dose, Unit, FrequencyId, Day, DayTypes, FoodRelation, Instructions, ServiceId, EncodedBy);


        if (StrMsg == string.Empty)
        {
            lblMessage.Text = "Medicines  Added !";
            lblMessage.ForeColor = System.Drawing.Color.Blue;

            int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
            GetServiceData(plantype);


        }
        else
        {
            lblMessage.Text = common.myStr(StrMsg);
            lblMessage.ForeColor = System.Drawing.Color.Red;

            //Alert.ShowAjaxMsg(StrMsg.ToString(), Page.Page);
            //return;           

        }






        ddlBrand_Prescriptions.ClearSelection();
        ddlBrand_Prescriptions.Text = string.Empty;
        //ddlBrand_Prescriptions.SelectedValue = "0";

        ddlFrequencyId.SelectedValue = "0";
        ddlPeriodType.SelectedValue = "D";
        //ddlPeriodType.ClearSelection();
        //ddlPeriodType.Text = string.Empty;
        txtDuration.Text = string.Empty;
        txtDose.Text = string.Empty;
        ddlUnit.SelectedValue = string.Empty;
        ddlUnit.ClearSelection();
        ddlUnit.Text = string.Empty;
        ddlFoodRelation.SelectedValue = "0";
        txtInstructions.Text = string.Empty;

        cmbServiceName.ClearSelection();
        cmbServiceName.Text = string.Empty;



        //DataTable dt = emr.GetEMRExistingMedicationOrder(common.myInt(Session["HospitalLocationid"]), common.myInt(Session["FacilityId"]),
        //       common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), common.myInt(ddlBrand_Prescriptions.SelectedValue), common.myStr(Session["OPIP"]));

        //if (dt.Rows.Count > 0)
        //{
        //    //dvConfirmAlreadyExistOptions.Visible = true;
        //    //lblItemName.Text = common.myStr(dt.Rows[0]["ItemName"]);
        //    //lblEnteredBy.Text = common.myStr(dt.Rows[0]["EnteredBy"]);
        //    //lblEnteredOn.Text = common.myStr(dt.Rows[0]["EncodedDate"]);
        //    dt.Dispose();
        //}
        //else
        //{
        //  ////  addItem();

        //}

    }



    private void addItem()
    {
        try
        {
            BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
            DataTable dtold = new DataTable();
            DataTable dt1old = new DataTable();

            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtNew = new DataTable();
            DataSet ds = new DataSet();
            DataView dv = new DataView();
            DataView dv1 = new DataView();


            DataTable dt3 = new DataTable();
            DataView dv3 = new DataView();
            DataRow DR2;

            try
            {
                DataRow DR;
                DataRow DR1;
                decimal dQty = 0;
                int countRow = 0;

                //if (common.myInt(ddlBrand_Prescriptions.SelectedValue.ToString()).Equals(0))
                //{
                //    Alert.ShowAjaxMsg("Please select drug", Page);
                //    return;
                //}


                if (common.myInt(ddlBrand_Prescriptions.SelectedValue.ToString()).Equals(0))
                {
                    Alert.ShowAjaxMsg("Please select Brand name", Page);
                    return;
                }



                if (ddlFrequencyId.SelectedValue == "0" || ddlFrequencyId.SelectedValue == string.Empty)
                {
                    Alert.ShowAjaxMsg("Please select Frequency", Page);
                    return;
                }




                if (txtDuration.Text == string.Empty || txtDuration.Text == "0"
                    || txtDuration.Text == "0." || txtDuration.Text == ".0"
                    || txtDuration.Text == ".")
                {
                    Alert.ShowAjaxMsg("Please type Duration", Page);

                    return;
                }




                if (ViewState["Item"] == null && ViewState["Edit"] == null)
                {
                    dt = CreateItemTable();
                    dt1 = CreateItemTable();
                }
                else
                {
                    dtold = (DataTable)ViewState["Item"];



                    if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true && ViewState["Item"] != null)
                    {
                        dv = new DataView(dtold);
                        dv.RowFilter = "ItemId<>" + ViewState["ItemId"].ToString();
                        dt = dv.ToTable();
                    }
                    else
                    {
                        dt = (DataTable)ViewState["Item"];
                    }

                    //#region Niraj start 
                    ////New Code Block By niraj start

                    //if (common.myStr(ViewState["Item"]) == string.Empty || common.myStr(ViewState["Item"]) == null )
                    //{
                    //    if (common.myStr(ViewState["FirstTimegvTreatmentPlan"]) != string.Empty || common.myStr(ViewState["FirstTimegvTreatmentPlan"]) != null)
                    //    {
                    //        //dt3 = CreateItemTable();
                    //      dt3= CreateItemTabledbData();

                    //        if (!dt3.Columns.Contains("Frequency"))
                    //        {
                    //            dt3.Columns.Add("Frequency", typeof(decimal));
                    //        }

                    //        //Item
                    //        DR2 = dt3.NewRow();



                    //        if (common.myInt(ddlBrand_Prescriptions.SelectedValue) == 0)
                    //        {
                    //            DR2["ItemId"] = DBNull.Value;
                    //        }
                    //        else if (ddlBrand_Prescriptions.SelectedValue != string.Empty)
                    //        {
                    //            DR2["ItemId"] = common.myInt(ddlBrand_Prescriptions.SelectedValue);
                    //        }


                    //        DR2["ItemName"] = ddlBrand_Prescriptions.Text;


                    //        DR2["Instruction"] = common.myStr(txtInstructions.Text);
                    //        DR2["DaysType"] = common.myStr(ddlPeriodType.SelectedValue);



                    //        #region Item Add


                    //        if (!dt1.Columns.Contains("DurationText"))
                    //        {
                    //            dt1.Columns.Add("DurationText", typeof(string));
                    //        }

                    //        DR1 = dt1.NewRow();
                    //        string sFrequency = "0";

                    //        if (ddlFrequencyId.SelectedValue != "0" && ddlFrequencyId.SelectedValue != string.Empty)
                    //        {
                    //            sFrequency = ddlFrequencyId.SelectedItem.Attributes["Frequency"].ToString();
                    //        }



                    //        //if (common.myInt(ddlUnit.SelectedValue).Equals(0))
                    //        //{
                    //        //    Alert.ShowAjaxMsg("Please select unit !", this.Page);
                    //        //    return;
                    //        //}



                    //        string Type = string.Empty;
                    //        decimal dDuration = 0;

                    //        switch (common.myStr(ddlPeriodType.SelectedValue))
                    //        {
                    //            case "N":
                    //                Type = txtDuration.Text + " Minute(s)";
                    //                dDuration = 1;
                    //                break;
                    //            case "H":
                    //                Type = txtDuration.Text + " Hour(s)";
                    //                dDuration = 1;
                    //                break;
                    //            case "D":
                    //                Type = txtDuration.Text + " Day(s)";
                    //                dDuration = 1;
                    //                break;
                    //            case "W":
                    //                Type = txtDuration.Text + " Week(s)";
                    //                dDuration = 7;
                    //                break;
                    //            case "M":
                    //                Type = txtDuration.Text + " Month(s)";
                    //                dDuration = 30;
                    //                break;
                    //            case "Y":
                    //                Type = txtDuration.Text + " Year(s)";
                    //                dDuration = 365;
                    //                break;
                    //        }

                    //        if (Request.QueryString["DRUGORDERCODE"] == null)
                    //        {
                    //            if (common.myBool(ViewState["ISCalculationRequired"]))
                    //            {
                    //                dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
                    //            }
                    //            else
                    //            {
                    //                dQty = 1;
                    //            }

                    //        }
                    //        else
                    //        {
                    //            dQty = 0;
                    //        }


                    //        DR2["ItemId"] = common.myInt(ddlBrand_Prescriptions.SelectedValue);

                    //        DR2["ItemName"] = common.myStr(ddlBrand_Prescriptions.Text);



                    //        DR2["Dose"] = txtDose.Text == string.Empty ? "0" : common.myStr(txtDose.Text);
                    //        DR2["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue) == 0 ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
                    //        DR2["Days"] = txtDuration.Text;
                    //        DR2["DurationText"] = Type;
                    //        DR2["Type"] = ddlPeriodType.SelectedValue;
                    //        DR2["DaysType"] = ddlPeriodType.SelectedValue;


                    //        DR2["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "0" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);
                    //        DR2["FoodRelationship"] = ddlFoodRelation.SelectedValue == "0" ? string.Empty : common.myStr(ddlFoodRelation.Text);

                    //        DR2["FoodName"] = ddlFoodRelation.SelectedValue == "0" ? string.Empty : common.myStr(ddlFoodRelation.Text);
                    //        DR2["UnitId"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? 0 : common.myInt(ddlUnit.SelectedValue);


                    //        DR2["DoseUnitID"] = common.myInt(ddlUnit.SelectedValue);
                    //        // DR1["UnitName"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? string.Empty : ddlUnit.Text;

                    //        DR2["DoseUnit"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? string.Empty : ddlUnit.Text;

                    //        DR2["Instructions"] = txtInstructions.Text;

                    //        DR2["DoseTypeId"] = 0;
                    //        DR2["DoseTypeName"] = string.Empty;

                    //        DR2["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                    //        DR1["Frequency"] = ddlFrequencyId.SelectedItem.Text;

                    //        DR2["TimeUnit"] = common.myStr("0");

                    //        DR2["Instruction"] = common.myStr(txtInstructions.Text);
                    //        DR2["Remarks"] = common.myStr(txtInstructions.Text);


                    //        DR2["Duration"] = common.myStr(txtDuration.Text);


                    //        dt3.Rows.Add(DR2);
                    //        dt3.AcceptChanges();

                    //        txtInstructions.Text = string.Empty;

                    //        #endregion
                    //        dt3.TableName = "MainItemDetail";
                    //        ////System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                    //        ////System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                    //        ////dt1.WriteXml(writer);

                    //        ////string xmlSchema = writer.ToString();
                    //        ////DR["XMLData"] = xmlSchema;





                    //        /*   */
                    //        ////dt.Rows.Add(DR);
                    //        ////dt.AcceptChanges();
                    //        DataTable dtOrignal = new DataTable();
                    //        DataTable dtall = new DataTable();
                    //        dtOrignal = ViewState["FirstTimegvTreatmentPlan"] as DataTable;
                    //        dtall = dtOrignal.Copy();

                    //        dtall.Merge(dt1, true, MissingSchemaAction.Ignore);
                    //        //dtOrignal.Merge(dt1,true,MissingSchemaAction.Ignore);  
                    //        dtall.AcceptChanges();
                    //    }

                    //    return;

                    //}

                    ////
                    //#endregion Niraj end

                    if (dt.Rows.Count > 0)
                    {
                        if (ViewState["Edit"] == null)
                        {
                            foreach (GridViewRow dataItem in gvTreatmentPlan.Rows)
                            {
                                //  TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");
                                //  dt.Rows[countRow]["Qty"] = common.myDbl(txtTotalQty.Text).ToString("F2");
                                countRow++;
                                dt.AcceptChanges();
                            }
                        }
                    }
                    if (ViewState["ItemDetail"] == null && ViewState["Edit"] == null)
                    {
                        dt1 = CreateItemTable();
                    }
                    else
                    {
                        dt1old = (DataTable)ViewState["ItemDetail"];
                        if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true && ViewState["ItemDetail"] != null)
                        {
                            dv1 = new DataView(dt1old);
                            dv1.RowFilter = "ItemId<>" + ViewState["ItemId"].ToString();
                            dt1 = dv1.ToTable();
                        }
                        else
                        {
                            dt1 = (DataTable)ViewState["ItemDetail"];
                        }
                    }
                }







                if (!dt.Columns.Contains("Frequency"))
                {
                    dt.Columns.Add("Frequency", typeof(decimal));
                }

                //Item
                DR = dt.NewRow();



                if (common.myInt(ddlBrand_Prescriptions.SelectedValue) == 0)
                {
                    DR["ItemId"] = DBNull.Value;
                }
                else if (ddlBrand_Prescriptions.SelectedValue != string.Empty)
                {
                    DR["ItemId"] = common.myInt(ddlBrand_Prescriptions.SelectedValue);
                }


                DR["ItemName"] = ddlBrand_Prescriptions.Text;


                DR["Instruction"] = common.myStr(txtInstructions.Text);
                DR["DaysType"] = common.myStr(ddlPeriodType.SelectedValue);



                #region Item Add


                if (!dt1.Columns.Contains("DurationText"))
                {
                    dt1.Columns.Add("DurationText", typeof(string));
                }

                DR1 = dt1.NewRow();
                string sFrequency = "0";

                if (ddlFrequencyId.SelectedValue != "0" && ddlFrequencyId.SelectedValue != string.Empty)
                {
                    sFrequency = ddlFrequencyId.SelectedItem.Attributes["Frequency"].ToString();
                }



                //if (common.myInt(ddlUnit.SelectedValue).Equals(0))
                //{
                //    Alert.ShowAjaxMsg("Please select unit !", this.Page);
                //    return;
                //}



                string Type = string.Empty;
                decimal dDuration = 0;

                switch (common.myStr(ddlPeriodType.SelectedValue))
                {
                    case "N":
                        Type = txtDuration.Text + " Minute(s)";
                        dDuration = 1;
                        break;
                    case "H":
                        Type = txtDuration.Text + " Hour(s)";
                        dDuration = 1;
                        break;
                    case "D":
                        Type = txtDuration.Text + " Day(s)";
                        dDuration = 1;
                        break;
                    case "W":
                        Type = txtDuration.Text + " Week(s)";
                        dDuration = 7;
                        break;
                    case "M":
                        Type = txtDuration.Text + " Month(s)";
                        dDuration = 30;
                        break;
                    case "Y":
                        Type = txtDuration.Text + " Year(s)";
                        dDuration = 365;
                        break;
                }

                if (Request.QueryString["DRUGORDERCODE"] == null)
                {
                    if (common.myBool(ViewState["ISCalculationRequired"]))
                    {
                        dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
                    }
                    else
                    {
                        dQty = 1;
                    }

                }
                else
                {
                    dQty = 0;
                }


                DR1["ItemId"] = common.myInt(ddlBrand_Prescriptions.SelectedValue);

                DR1["ItemName"] = common.myStr(ddlBrand_Prescriptions.Text);



                DR1["Dose"] = txtDose.Text == string.Empty ? "0" : common.myStr(txtDose.Text);
                DR1["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue) == 0 ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
                DR1["Days"] = txtDuration.Text;
                DR1["DurationText"] = Type;
                DR1["Type"] = ddlPeriodType.SelectedValue;
                DR1["DaysType"] = ddlPeriodType.SelectedValue;


                DR1["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "0" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);
                DR1["FoodRelationship"] = ddlFoodRelation.SelectedValue == "0" ? string.Empty : common.myStr(ddlFoodRelation.Text);

                DR1["FoodName"] = ddlFoodRelation.SelectedValue == "0" ? string.Empty : common.myStr(ddlFoodRelation.Text);
                DR1["UnitId"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? 0 : common.myInt(ddlUnit.SelectedValue);


                DR1["DoseUnitID"] = common.myInt(ddlUnit.SelectedValue);
                // DR1["UnitName"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? string.Empty : ddlUnit.Text;

                DR1["DoseUnit"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? string.Empty : ddlUnit.Text;

                DR1["Instructions"] = txtInstructions.Text;

                DR1["DoseTypeId"] = 0;
                DR1["DoseTypeName"] = string.Empty;

                DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                DR1["Frequency"] = ddlFrequencyId.SelectedItem.Text;

                DR1["TimeUnit"] = common.myStr("0");

                DR1["Instruction"] = common.myStr(txtInstructions.Text);
                DR1["Remarks"] = common.myStr(txtInstructions.Text);


                DR1["Duration"] = common.myStr(txtDuration.Text);


                dt1.Rows.Add(DR1);
                dt1.AcceptChanges();

                txtInstructions.Text = string.Empty;

                #endregion
                dt1.TableName = "ItemDetail";
                System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                dt1.WriteXml(writer);

                string xmlSchema = writer.ToString();
                DR["XMLData"] = xmlSchema;





                /*   */
                dt.Rows.Add(DR);
                dt.AcceptChanges();
                DataTable dtOrignal = new DataTable();
                DataTable dtall = new DataTable();
                dtOrignal = ViewState["FirstTimegvTreatmentPlan"] as DataTable;
                dtall = dtOrignal.Copy();

                dtall.Merge(dt1, true, MissingSchemaAction.Ignore);
                //dtOrignal.Merge(dt1,true,MissingSchemaAction.Ignore);  
                dtall.AcceptChanges();




                //

                //dt_gvAddList = (DataTable)ViewState["gvAddList"];

                //dtAll = dt_gvAddList.Copy();
                //dtAll.Merge(dt1, true, MissingSchemaAction.Ignore);
                //


                ViewState["DataTableDetail"] = null;
                ViewState["ItemDetail"] = dt1;
                ViewState["Item"] = dt1;


                gvTreatmentPlan.DataSource = null;
                gvTreatmentPlan.DataBind();

                gvTreatmentPlan.DataSource = dtall;
                gvTreatmentPlan.DataBind();

                ViewState["totalgvTreatmentPlan"] = dtall;

                //    ViewState["FirstTimegvTreatmentPlan"] = dtall;
                //gvTreatmentPlan.DataSource = dt1;
                //gvTreatmentPlan.DataBind();
                /*   */





                ddlBrand_Prescriptions.Focus();
                ddlBrand_Prescriptions.Items.Clear();
                ddlBrand_Prescriptions.Text = string.Empty;



                ddlBrand_Prescriptions.Enabled = true;
                ddlBrand_Prescriptions.SelectedValue = "0";



                txtInstructions.Text = string.Empty;
                ddlFoodRelation.SelectedValue = "0";
                ddlFoodRelation.Text = string.Empty;


                ddlFrequencyId.Enabled = true;

                txtDuration.Enabled = true;


                ddlFrequencyId.SelectedIndex = 0;
                txtDose.Text = "1"; //string.Empty;
                txtDuration.Text = string.Empty;
                ddlPeriodType.SelectedValue = "D";
                ddlUnit.SelectedValue = "0";
                txtInstructions.Text = string.Empty;

                ddlFoodRelation.SelectedValue = "0";




                ViewState["Edit"] = null;
                ViewState["ItemId"] = null;




            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;
                objException.HandleException(Ex);
            }
            finally
            {
                dtold.Dispose();
                dt1old.Dispose();
                dt.Dispose();
                dt1.Dispose();
                dtNew.Dispose();
                ds.Dispose();
                emr = null;
                dv.Dispose();
                dv1.Dispose();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    //private void addItem()
    //{
    //    try
    //    {
    //        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
    //        DataTable dtold = new DataTable();
    //        DataTable dt1old = new DataTable();

    //        DataTable dt = new DataTable();
    //        DataTable dt1 = new DataTable();
    //        DataTable dtNew = new DataTable();
    //        DataSet ds = new DataSet();
    //        DataView dv = new DataView();
    //        DataView dv1 = new DataView();
    //        try
    //        {
    //            DataRow DR;
    //            DataRow DR1;
    //            decimal dQty = 0;
    //            int countRow = 0;

    //            //if (common.myInt(ddlBrand_Prescriptions.SelectedValue.ToString()).Equals(0))
    //            //{
    //            //    Alert.ShowAjaxMsg("Please select drug", Page);
    //            //    return;
    //            //}


    //            if (common.myInt(ddlBrand_Prescriptions.SelectedValue.ToString()).Equals(0))
    //            {
    //                Alert.ShowAjaxMsg("Please select Brand name", Page);
    //                return;
    //            }



    //            if (ddlFrequencyId.SelectedValue == "0" || ddlFrequencyId.SelectedValue == string.Empty)
    //            {
    //                Alert.ShowAjaxMsg("Please select Frequency", Page);
    //                return;
    //            }




    //            if (txtDuration.Text == string.Empty || txtDuration.Text == "0"
    //                || txtDuration.Text == "0." || txtDuration.Text == ".0"
    //                || txtDuration.Text == ".")
    //            {
    //                Alert.ShowAjaxMsg("Please type Duration", Page);

    //                return;
    //            }






    //            if (ViewState["Item"] == null && ViewState["Edit"] == null)
    //            {
    //                dt = CreateItemTable();
    //                dt1 = CreateItemTable();
    //            }
    //            else
    //            {
    //                dtold = (DataTable)ViewState["Item"];



    //                if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true && ViewState["Item"] != null)
    //                {
    //                    dv = new DataView(dtold);
    //                    dv.RowFilter = "ItemId<>" + ViewState["ItemId"].ToString();
    //                    dt = dv.ToTable();
    //                }
    //                else
    //                {
    //                    dt = (DataTable)ViewState["Item"];
    //                }
    //                if (dt.Rows.Count > 0)
    //                {
    //                    if (ViewState["Edit"] == null)
    //                    {
    //                        foreach (GridViewRow dataItem in gvTreatmentPlan.Rows)
    //                        {
    //                            //  TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");
    //                            //  dt.Rows[countRow]["Qty"] = common.myDbl(txtTotalQty.Text).ToString("F2");
    //                            countRow++;
    //                            dt.AcceptChanges();
    //                        }
    //                    }
    //                }
    //                if (ViewState["ItemDetail"] == null && ViewState["Edit"] == null)
    //                {
    //                    dt1 = CreateItemTable();
    //                }
    //                else
    //                {
    //                    dt1old = (DataTable)ViewState["ItemDetail"];
    //                    if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true && ViewState["ItemDetail"] != null)
    //                    {
    //                        dv1 = new DataView(dt1old);
    //                        dv1.RowFilter = "ItemId<>" + ViewState["ItemId"].ToString();
    //                        dt1 = dv1.ToTable();
    //                    }
    //                    else
    //                    {
    //                        dt1 = (DataTable)ViewState["ItemDetail"];
    //                    }
    //                }
    //            }







    //            if (!dt.Columns.Contains("Frequency"))
    //            {
    //                dt.Columns.Add("Frequency", typeof(decimal));
    //            }

    //            //Item
    //            DR = dt.NewRow();



    //            if (common.myInt(ddlBrand_Prescriptions.SelectedValue) == 0)
    //            {
    //                DR["ItemId"] = DBNull.Value;
    //            }
    //            else if (ddlBrand_Prescriptions.SelectedValue != string.Empty)
    //            {
    //                DR["ItemId"] = common.myInt(ddlBrand_Prescriptions.SelectedValue);
    //            }


    //            DR["ItemName"] = ddlBrand_Prescriptions.Text;


    //            DR["Remarks"] = common.myStr(txtInstructions.Text);
    //         //   DR["Instruction"] = common.myStr(txtInstructions.Text);



    //            #region Item Add


    //            if (!dt1.Columns.Contains("DurationText"))
    //            {
    //                dt1.Columns.Add("DurationText", typeof(string));
    //            }

    //            DR1 = dt1.NewRow();
    //            string sFrequency = "0";

    //            if (ddlFrequencyId.SelectedValue != "0" && ddlFrequencyId.SelectedValue != string.Empty)
    //            {
    //                sFrequency = ddlFrequencyId.SelectedItem.Attributes["Frequency"].ToString();
    //            }



    //            //if (common.myInt(ddlUnit.SelectedValue).Equals(0))
    //            //{
    //            //    Alert.ShowAjaxMsg("Please select unit !", this.Page);
    //            //    return;
    //            //}



    //            string Type = string.Empty;
    //            decimal dDuration = 0;

    //            switch (common.myStr(ddlPeriodType.SelectedValue))
    //            {
    //                case "N":
    //                    Type = txtDuration.Text + " Minute(s)";
    //                    dDuration = 1;
    //                    break;
    //                case "H":
    //                    Type = txtDuration.Text + " Hour(s)";
    //                    dDuration = 1;
    //                    break;
    //                case "D":
    //                    Type = txtDuration.Text + " Day(s)";
    //                    dDuration = 1;
    //                    break;
    //                case "W":
    //                    Type = txtDuration.Text + " Week(s)";
    //                    dDuration = 7;
    //                    break;
    //                case "M":
    //                    Type = txtDuration.Text + " Month(s)";
    //                    dDuration = 30;
    //                    break;
    //                case "Y":
    //                    Type = txtDuration.Text + " Year(s)";
    //                    dDuration = 365;
    //                    break;
    //            }

    //            if (Request.QueryString["DRUGORDERCODE"] == null)
    //            {
    //                if (common.myBool(ViewState["ISCalculationRequired"]))
    //                {
    //                    dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
    //                }
    //                else
    //                {
    //                    dQty = 1;
    //                }

    //            }
    //            else
    //            {
    //                dQty = 0;
    //            }


    //            DR1["ItemId"] = common.myInt(ddlBrand_Prescriptions.SelectedValue);

    //            DR1["ItemName"] = common.myStr(ddlBrand_Prescriptions.Text);



    //            DR1["Dose"] = txtDose.Text == string.Empty ? "0" : common.myStr(txtDose.Text);
    //            DR1["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue) == 0 ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
    //            DR1["Days"] = txtDuration.Text;
    //            DR1["DurationText"] = Type;
    //            DR1["DaysType"] = ddlPeriodType.SelectedValue;



    //            DR1["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "0" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);
    //            DR1["FoodRelationship"] = ddlFoodRelation.SelectedValue == "0" ? string.Empty : common.myStr(ddlFoodRelation.Text);
    //            DR1["UnitId"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? 0 : common.myInt(ddlUnit.SelectedValue);
    //            DR1["UnitName"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? string.Empty : ddlUnit.Text;

    //            DR1["Instructions"] = txtInstructions.Text;

    //            DR1["DoseTypeId"] = 0;
    //            DR1["DoseTypeName"] = string.Empty;

    //            DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
    //            DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;

    //            DR1["TimeUnit"] = common.myStr("0");

    //            DR1["Instruction"] = common.myStr(txtInstructions.Text);
    //            dt1.Rows.Add(DR1);
    //            dt1.AcceptChanges();

    //            txtInstructions.Text = string.Empty;

    //            #endregion
    //            dt1.TableName = "ItemDetail";
    //            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

    //            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
    //            dt1.WriteXml(writer);

    //            string xmlSchema = writer.ToString();
    //            DR["XMLData"] = xmlSchema;


    //            /*   */
    //            dt.Rows.Add(DR);
    //            dt.AcceptChanges();

    //            ViewState["DataTableDetail"] = null;
    //            ViewState["ItemDetail"] = dt1;
    //            ViewState["Item"] = dt1;
    //            gvTreatmentPlan.DataSource = dt1;
    //            gvTreatmentPlan.DataBind();
    //            /*   */





    //            ddlBrand_Prescriptions.Focus();
    //            ddlBrand_Prescriptions.Items.Clear();
    //            ddlBrand_Prescriptions.Text = string.Empty;



    //            ddlBrand_Prescriptions.Enabled = true;
    //            ddlBrand_Prescriptions.SelectedValue = "0";



    //            txtInstructions.Text = string.Empty;
    //            ddlFoodRelation.SelectedValue = "0";
    //            ddlFoodRelation.Text = string.Empty;


    //            ddlFrequencyId.Enabled = true;

    //            txtDuration.Enabled = true;


    //            ddlFrequencyId.SelectedIndex = 0;
    //            txtDose.Text = "1"; //string.Empty;
    //            txtDuration.Text = string.Empty;
    //            ddlPeriodType.SelectedValue = "D";
    //            ddlUnit.SelectedValue = "0";
    //            txtInstructions.Text = string.Empty;

    //            ddlFoodRelation.SelectedValue = "0";




    //            ViewState["Edit"] = null;
    //            ViewState["ItemId"] = null;




    //        }
    //        catch (Exception Ex)
    //        {
    //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //            lblMessage.Text = "Error: " + Ex.Message;
    //            objException.HandleException(Ex);
    //        }
    //        finally
    //        {
    //            dtold.Dispose();
    //            dt1old.Dispose();
    //            dt.Dispose();
    //            dt1.Dispose();
    //            dtNew.Dispose();
    //            ds.Dispose();
    //            emr = null;
    //            dv.Dispose();
    //            dv1.Dispose();
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }

    //}





    protected DataTable CreateItemTable()
    {

        DataTable dt = new DataTable();

        dt.Columns.Add("ItemId", typeof(int));

        dt.Columns.Add("ItemName", typeof(string));

        dt.Columns.Add("XMLData", typeof(string));

        //dt.Columns.Add("Dose", typeof(double));
        dt.Columns.Add("Dose", typeof(string));




        dt.Columns.Add("Frequency", typeof(string));
        dt.Columns.Add("FrequencyName", typeof(string));
        dt.Columns.Add("FrequencyId", typeof(int));
        dt.Columns.Add("Duration", typeof(string));
        dt.Columns.Add("Days", typeof(double));

        dt.Columns.Add("Type", typeof(string));
        dt.Columns.Add("DaysType", typeof(string));

        dt.Columns.Add("DurationText", typeof(string));


        dt.Columns.Add("DoseUnitID", typeof(int));

        // dt.Columns.Add("DoseUnitID", typeof(string));
        dt.Columns.Add("UnitId", typeof(int));



        dt.Columns.Add("DoseUnit", typeof(string));


        ////   dt.Columns.Add("UnitName", typeof(string));
        dt.Columns.Add("FoodRelationshipId", typeof(int));
        dt.Columns.Add("FoodRelationship", typeof(string));

        dt.Columns.Add("FoodName", typeof(string));




        dt.Columns.Add("Instructions", typeof(string));
        dt.Columns.Add("DoseTypeId", typeof(int));
        dt.Columns.Add("DoseTypeName", typeof(string));

        dt.Columns.Add("TimeUnit", typeof(string));

        dt.Columns.Add("Instruction", typeof(string));
        dt.Columns.Add("Remarks", typeof(string));

        //dt.Columns.Add("Days", typeof(string));


        return dt;

    }

    protected DataTable CreateItemTabledbData()
    {

        DataTable dt = new DataTable();

        dt.Columns.Add("ItemId", typeof(int));

        dt.Columns.Add("ItemName", typeof(string));
        dt.Columns.Add("Dose", typeof(string));
        dt.Columns.Add("DoseUnit", typeof(string));


        dt.Columns.Add("DoseUnitID", typeof(int));
        dt.Columns.Add("Frequency", typeof(string));
        dt.Columns.Add("FrequencyId", typeof(int));
        dt.Columns.Add("Days", typeof(double));
        dt.Columns.Add("DaysType", typeof(string));
        dt.Columns.Add("FoodName", typeof(string));
        dt.Columns.Add("FoodNameID", typeof(int));
        dt.Columns.Add("Remarks", typeof(int));
        return dt;
    }

    //protected DataTable CreateItemTabledbData()
    //{

    //    DataTable dt = new DataTable();





    //    dt.Columns.Add("ItemId", typeof(int));

    //    dt.Columns.Add("ItemName", typeof(string));



    //    dt.Columns.Add("Dose", typeof(double));
    //    dt.Columns.Add("DoseUnit", typeof(string));

    //    dt.Columns.Add("Frequency", typeof(decimal));

    //    dt.Columns.Add("Days", typeof(double));
    //    dt.Columns.Add("DaysType", typeof(string));
    //    dt.Columns.Add("FoodName", typeof(string));
    //    dt.Columns.Add("Remarks", typeof(string));




    //    return dt;

    //}
    protected DataTable CreateServiceTable()
    {

        DataTable dt = new DataTable();



        dt.Columns.Add("ServiceId", typeof(int));
        dt.Columns.Add("ServiceName", typeof(String));




        return dt;

    }
    protected void gvTreatmentPlan_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

            //e.Row.Cells[8].Visible = false;
            //e.Row.Cells[9].Visible = false;
            //e.Row.Cells[10].Visible = false;
            //e.Row.Cells[11].Visible = false;
            //e.Row.Cells[12].Visible = false;

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[8].Visible = false;
            //e.Row.Cells[9].Visible = false;
            //e.Row.Cells[10].Visible = false;
            //e.Row.Cells[11].Visible = false;
            //e.Row.Cells[12].Visible = false;
        }

    }


    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.EMRMasters emrMaster = new BaseC.EMRMasters(sConString);
        DataTable dtDel = new DataTable();
        DataTable dtService = new DataTable();

        DataView dvService = new DataView();

        DataView dvDel = new DataView();
        string strDeleteMessage = string.Empty;
        if (e.CommandName == "ItemDelete")
        {
            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            // int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
            // int IndentId = common.myInt(((HiddenField)row.FindControl("hdnIndentId")).Value);

            //////Label lblBrand_Prescriptions_ID = (Label)row.FindControl("lblItemName");
            HiddenField hdnServiceId = (HiddenField)row.FindControl("hdnServiceiD");

            int ServiceId = common.myInt(common.myStr(hdnServiceId.Value));


            int EncodedBy = 1;

            strDeleteMessage = emrMaster.DeleteEMRTreatmentTemplate(common.myInt(ddlPlanTemplates.SelectedValue), 0, common.myInt(ServiceId), EncodedBy,0);

            if (strDeleteMessage == "Service deleted!")
            {
                lblMessage.Text = "Investigations  deleted!";
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
                GetServiceData(plantype);

            }
            else
            {
                lblMessage.Text = strDeleteMessage;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                //Alert.ShowAjaxMsg(strDeleteMessage.ToString(), Page.Page);
                //return;

            }




            ////dtService = CreateServiceTable();
            ////dtService = (DataTable)ViewState["FirstTimegvService"];
            ////dtDel = (DataTable)ViewState["FirstTimegvService"];

            //dt.Columns.Add("ServiceId", typeof(int));
            //dt.Columns.Add("ServiceName", typeof(String));
            ////dvDel = dtDel.Copy().DefaultView;
            ////if (ServiceId > 0)
            ////{

            ////    dvService.RowFilter = "ISNULL(ServiceId,0) <> " + ServiceId;// +" AND IndentId<>" + IndentId;

            ////    dvDel.RowFilter = "ISNULL(ServiceId,0) <> " + ServiceId;// +" AND IndentId<>" + IndentId;
            ////}

            ////dtDel = dvDel.ToTable();


            ////gvService.DataSource = dtDel;
            ////gvService.DataBind();
            ////ViewState["FirstTimegvService"] = dtDel;

        }

    }

    protected void gvDiagnosisDetails_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.EMRMasters emrMaster = new BaseC.EMRMasters(sConString);
        DataTable dtDel = new DataTable();
        DataTable dtService = new DataTable();

        DataView dvService = new DataView();

        DataView dvDel = new DataView();
        string strDeleteMessage = string.Empty;
        if (e.CommandName == "Del")
        {
            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            // int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
            // int IndentId = common.myInt(((HiddenField)row.FindControl("hdnIndentId")).Value);

            //////Label lblBrand_Prescriptions_ID = (Label)row.FindControl("lblItemName");
            Label lblId = (Label)row.FindControl("lblId");

            int EncodedBy = 1;

            strDeleteMessage = emrMaster.DeleteEMRTreatmentTemplate(common.myInt(ddlPlanTemplates.SelectedValue), 0, 0, EncodedBy,common.myInt(lblId.Text));

            if (strDeleteMessage == "Diagnosis deleted!")
            {
                lblMessage.Text = "Diagnosis  deleted!";
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
                GetServiceData(plantype);
            }
            else
            {
                lblMessage.Text = strDeleteMessage;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                //Alert.ShowAjaxMsg(strDeleteMessage.ToString(), Page.Page);
                //return;
            }




            ////dtService = CreateServiceTable();
            ////dtService = (DataTable)ViewState["FirstTimegvService"];
            ////dtDel = (DataTable)ViewState["FirstTimegvService"];

            //dt.Columns.Add("ServiceId", typeof(int));
            //dt.Columns.Add("ServiceName", typeof(String));
            ////dvDel = dtDel.Copy().DefaultView;
            ////if (ServiceId > 0)
            ////{

            ////    dvService.RowFilter = "ISNULL(ServiceId,0) <> " + ServiceId;// +" AND IndentId<>" + IndentId;

            ////    dvDel.RowFilter = "ISNULL(ServiceId,0) <> " + ServiceId;// +" AND IndentId<>" + IndentId;
            ////}

            ////dtDel = dvDel.ToTable();


            ////gvService.DataSource = dtDel;
            ////gvService.DataBind();
            ////ViewState["FirstTimegvService"] = dtDel;

        }

    }
    protected void gvTreatmentPlant_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        BaseC.EMRMasters emrmaster = new BaseC.EMRMasters(sConString);


        DataSet dsCalisreq = new DataSet();

        DataSet dsXml = new DataSet();
        DataTable dtDetail = new DataTable();
        DataView DV = new DataView();
        DataView DV1 = new DataView();
        DataTable dt = new DataTable();
        DataView dvFilter = new DataView();
        DataTable tbl = new DataTable();
        DataTable tbl1 = new DataTable();

        DataTable dtDel = new DataTable();
        DataView dvDel = new DataView();
        string StrDelMessage = string.Empty;

        if (e.CommandName == "ItemDelete")
        {
            //int ItemId = common.myInt(e.CommandArgument);

            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);


            Label lblBrand_Prescriptions_ID = (Label)row.FindControl("lblBrand_Prescriptions_ID");

            int ItemId = common.myInt(common.myStr(lblBrand_Prescriptions_ID.Text));
            int EncodedBy = 1;

            StrDelMessage = emrmaster.DeleteEMRTreatmentTemplate(common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ItemId), 0, EncodedBy,0);

            if (StrDelMessage == "Brand name deleted!")
            {
                lblMessage.Text = StrDelMessage;
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
                GetServiceData(plantype);

            }
            else

            {
                lblMessage.Text = StrDelMessage;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                //Alert.ShowAjaxMsg(StrDelMessage.ToString(), Page.Page);
                //return;            

            }

            //  HiddenField hdnCustomMedication = (HiddenField)row.FindControl("hdnCustomMedication");

            if (ViewState["ItemDetail"] != null && (ItemId > 0))
            {
                tbl = (DataTable)ViewState["Item"];
                tbl1 = (DataTable)ViewState["ItemDetail"];
                DV = tbl.Copy().DefaultView;
                DV1 = tbl1.Copy().DefaultView;

                dtDel = (DataTable)ViewState["totalgvTreatmentPlan"];
                dvDel = dtDel.Copy().DefaultView;



                //if (common.myBool(hdnCustomMedication.Value))
                //{
                //    Label lblItemName = (Label)row.FindControl("lblItemName");
                //    DV.RowFilter = "CustomMedication=0 AND ISNULL(ItemName,0) <> '" + common.myStr(lblItemName.Text) + "'";
                //    DV1.RowFilter = "CustomMedication=0 AND ISNULL(ItemName,0) <> '" + common.myStr(lblItemName.Text) + "'";
                //}
                //else
                //{
                if (ItemId > 0)
                {

                    DV.RowFilter = "ISNULL(ItemId,0) <> " + ItemId;// +" AND IndentId<>" + IndentId;
                    DV1.RowFilter = "ISNULL(ItemId,0) <> " + ItemId;// +" AND IndentId<>" + IndentId;

                    dvDel.RowFilter = "ISNULL(ItemId,0) <> " + ItemId;// +" AND IndentId<>" + IndentId;
                }

                dtDel = dvDel.ToTable();
                //else
                //{
                //    DV.RowFilter = "ISNULL(GenericId, 0) <> " + GenericId;
                //    DV1.RowFilter = "ISNULL(GenericId, 0) <> " + GenericId;
                //}
                //}

                tbl = DV.ToTable();

                ViewState["GridDataItem"] = DV1.ToTable();
                ViewState["ItemDetail"] = DV1.ToTable();
                ViewState["Item"] = DV1.ToTable();
                if (tbl.Rows.Count == 0)
                {
                    DataRow DR = tbl.NewRow();
                    tbl.Rows.Add(DR);
                    tbl.AcceptChanges();
                }

                // gvTreatmentPlan.DataSource = DV1.ToTable(); temproray commented by niraj 

                //added by niraj strat
                gvTreatmentPlan.DataSource = dtDel;
                gvTreatmentPlan.DataBind();
                ViewState["totalgvTreatmentPlan"] = dtDel;
                //added by niraj end

                if (dtDel.Rows.Count == 0)
                {
                    // BindBlankItemGrid();
                    gvTreatmentPlan.DataSource = null;
                    gvTreatmentPlan.DataBind();
                }

                // commented by niraj
                //if (DV1.ToTable().Rows.Count == 0)
                //{
                //    // BindBlankItemGrid();
                //    gvTreatmentPlan.DataSource = null;
                //    gvTreatmentPlan.DataBind();
                //}
                //added by niraj end

                ViewState["StopItemDetail"] = null;
                ViewState["Edit"] = null;
                ViewState["ItemId"] = 0;
                //setVisiblilityInteraction();
            }



            //added by niraj start 
            else
            {
                DataTable dttempdel = new DataTable();
                DataView dvtempdel = new DataView();
                dttempdel = (DataTable)ViewState["totalgvTreatmentPlanINdb"];
                //   dtDel = (DataTable)ViewState["totalgvTreatmentPlan"];
                dvtempdel = dttempdel.Copy().DefaultView;

                if (ItemId > 0)
                {


                    dvtempdel.RowFilter = "ISNULL(ItemId,0) <> " + ItemId;// +" AND IndentId<>" + IndentId;
                }

                dttempdel = dvtempdel.ToTable();
                ViewState["totalgvTreatmentPlanINdb"] = dttempdel;

                gvTreatmentPlan.DataSource = dttempdel;
                gvTreatmentPlan.DataBind();
                //    ViewState["totalgvTreatmentPlan"] = dtDel;
                //added by niraj end

            }
        }
        else if (e.CommandName == "Select")
        {
            dsXml = new DataSet();
            dtDetail = new DataTable();
            DV = new DataView();
            dt = new DataTable();
            dvFilter = new DataView();
            int ItemId_Select = common.myInt(e.CommandArgument);

            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            //int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);




            // HiddenField hdnStrengthValue = (HiddenField)row.FindControl("hdnStrengthValue");

            Label lblItemName = (Label)row.FindControl("lblItemName");

            hdnItemId.Value = ItemId_Select.ToString();

            ViewState["ItemId"] = ItemId_Select;
            ddlBrand_Prescriptions.Text = lblItemName.Text;
            ddlBrand_Prescriptions.SelectedValue = ItemId_Select.ToString();
            ddlBrand_Prescriptions.Enabled = false;




            dt = (DataTable)ViewState["ItemDetail"];

            // ADDED BY NIRAJ START
            // dt = (DataTable)ViewState["ItemDetail"];
            //  dt = (DataTable)ViewState["totalgvTreatmentPlan"];

            // ADDED BY NIRAJ END



            DV = new DataView(dt);

            if (common.myStr(ViewState["ItemDetail"]) != string.Empty)
            {
                DV.RowFilter = "ItemId=" + ItemId_Select;
                if (DV.ToTable().Rows.Count > 0)
                {
                    ViewState["StopItemDetail"] = DV.ToTable();

                    //  dvFilter = new DataView(dsXml.Tables[0]);
                    dvFilter = new DataView(dt);
                    dvFilter.RowFilter = "ISNULL(ItemId,0)=" + ItemId_Select;
                    dtDetail = dvFilter.ToTable();

                    if (dtDetail.Rows.Count > 0)
                    {
                        ddlFrequencyId.SelectedValue = dtDetail.Rows[0]["FrequencyId"].ToString();
                        txtDose.Text = dtDetail.Rows[0]["Dose"].ToString();
                        txtDuration.Text = dtDetail.Rows[0]["Duration"].ToString();
                        if (common.myLen(dtDetail.Rows[0]["Type"]).Equals(0))
                        {
                            ddlPeriodType.SelectedValue = "D";
                        }
                        else
                        {
                            ddlPeriodType.SelectedValue = dtDetail.Rows[0]["Type"].ToString();
                        }

                        txtInstructions.Text = dtDetail.Rows[0]["Instructions"].ToString();


                        ddlFoodRelation.SelectedValue = dtDetail.Rows[0]["FoodRelationshipID"].ToString();

                        ddlUnit.SelectedValue = common.myInt(dtDetail.Rows[0]["UnitId"]).ToString();

                        ddlFrequencyId.SelectedValue = common.myStr(dtDetail.Rows[0]["FrequencyId"]);

                        txtDose.Text = common.myStr(dtDetail.Rows[0]["Dose"]);


                    }
                }

                // added by niraj start 

                else
                {

                    DataTable dttempdel = new DataTable();
                    DataView dvtempdel = new DataView();
                    dttempdel = (DataTable)ViewState["totalgvTreatmentPlanINdb"];
                    //   dtDel = (DataTable)ViewState["totalgvTreatmentPlan"];
                    dvtempdel = dttempdel.Copy().DefaultView;

                    if (ItemId_Select > 0)
                    {


                        dvtempdel.RowFilter = "ISNULL(ItemId,0) <> " + ItemId_Select;// +" AND IndentId<>" + IndentId;
                    }

                    dttempdel = dvtempdel.ToTable();
                    ////ViewState["totalgvTreatmentPlanINdb"] = dttempdel;

                    ////gvTreatmentPlan.DataSource = dttempdel;
                    ////gvTreatmentPlan.DataBind();
                    //    ViewState["totalgvTreatmentPlan"] = dtDel;
                    //added by niraj end



                    if (dttempdel.Rows.Count > 0)
                    {
                        ddlFrequencyId.SelectedValue = dttempdel.Rows[0]["FrequencyId"].ToString();
                        txtDose.Text = dttempdel.Rows[0]["Dose"].ToString();
                        txtDuration.Text = dttempdel.Rows[0]["Duration"].ToString();
                        if (common.myLen(dttempdel.Rows[0]["Type"]).Equals(0))
                        {
                            ddlPeriodType.SelectedValue = "D";
                        }
                        else
                        {
                            ddlPeriodType.SelectedValue = dttempdel.Rows[0]["Type"].ToString();
                        }

                        txtInstructions.Text = dttempdel.Rows[0]["Instructions"].ToString();


                        ddlFoodRelation.SelectedValue = dttempdel.Rows[0]["FoodRelationshipID"].ToString();

                        ddlUnit.SelectedValue = common.myInt(dttempdel.Rows[0]["UnitId"]).ToString();

                        ddlFrequencyId.SelectedValue = common.myStr(dttempdel.Rows[0]["FrequencyId"]);

                        txtDose.Text = common.myStr(dttempdel.Rows[0]["Dose"]);


                    }

                }

                // added by niraj  end




            }

            else
            {
                DataTable dttempdel = new DataTable();
                DataView dvtempdel = new DataView();
                dttempdel = (DataTable)ViewState["totalgvTreatmentPlanINdb"];
                //   dtDel = (DataTable)ViewState["totalgvTreatmentPlan"];
                dvtempdel = dttempdel.Copy().DefaultView;

                if (ItemId_Select > 0)
                {


                    dvtempdel.RowFilter = "ISNULL(ItemId,0) = " + ItemId_Select;// +" AND IndentId<>" + IndentId;
                }

                dttempdel = dvtempdel.ToTable();
                ////ViewState["totalgvTreatmentPlanINdb"] = dttempdel;

                ////gvTreatmentPlan.DataSource = dttempdel;
                ////gvTreatmentPlan.DataBind();
                //    ViewState["totalgvTreatmentPlan"] = dtDel;
                //added by niraj end



                if (dttempdel.Rows.Count > 0)
                {
                    ddlFrequencyId.SelectedValue = dttempdel.Rows[0]["FrequencyId"].ToString();
                    txtDose.Text = dttempdel.Rows[0]["Dose"].ToString();
                    txtDuration.Text = dttempdel.Rows[0]["Days"].ToString();
                    if (common.myLen(dttempdel.Rows[0]["DaysType"]).Equals(0))
                    {
                        ddlPeriodType.SelectedValue = "D";
                    }
                    else
                    {
                        ddlPeriodType.SelectedValue = dttempdel.Rows[0]["DaysType"].ToString();
                    }

                    txtInstructions.Text = dttempdel.Rows[0]["Remarks"].ToString();


                    ddlFoodRelation.SelectedValue = dttempdel.Rows[0]["FoodNameID"].ToString();

                    ddlUnit.SelectedValue = common.myInt(dttempdel.Rows[0]["DoseUnitId"]).ToString();

                    ddlFrequencyId.SelectedValue = common.myStr(dttempdel.Rows[0]["FrequencyId"]);

                    txtDose.Text = common.myStr(dttempdel.Rows[0]["Dose"]);


                }

            }

            //BindStopItemDetail();

            dsCalisreq = objPharmacy.ISCalculationRequired(ItemId_Select);
            if (dsCalisreq.Tables[0].Rows.Count > 0)
            {
                ViewState["ISCalculationRequired"] = common.myBool(dsCalisreq.Tables[0].Rows[0]["CalculationRequired"]);
            }
            ViewState["Edit"] = true;
        }
    }

    private void BindBlankItemGrid()
    {

        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();

        dr["ItemId"] = 0;
        dr["ItemName"] = string.Empty;


        dr["ItemName"] = string.Empty;

        dr["XMLData"] = string.Empty;

        dr["Dose"] = 0;
        dr["Days"] = 0;


        dr["Frequency"] = 0;
        dr["FrequencyName"] = string.Empty;
        dr["FrequencyId"] = 0;
        dr["Duration"] = string.Empty;
        dr["Type"] = string.Empty;
        dr["DurationText"] = string.Empty;
        dr["UnitId"] = 0;
        dr["UnitName"] = string.Empty;
        dr["FoodRelationshipId"] = 0;
        dr["FoodRelationship"] = string.Empty;

        dr["Instructions"] = string.Empty;
        dr["DoseTypeId"] = 0;
        dr["DoseTypeName"] = string.Empty;

        dr["TimeUnit"] = string.Empty;

        dr["Instruction"] = string.Empty;


        dt.Rows.Add(dr);
        dt.AcceptChanges();
        ViewState["ItemDetail"] = null;
        gvTreatmentPlan.DataSource = dt;
        gvTreatmentPlan.DataBind();

        ViewState["DataTableItem"] = dt;
    }

    //private void BindBlankItemGrid()
    //{

    //    DataTable dt = CreateItemTable();
    //    DataRow dr = dt.NewRow();

    //    dr["ItemId"] = 0;
    //    dr["ItemName"] = string.Empty;
    //    dr["Dose"] = 0;
    //    dr["DoseUnit"] = string.Empty;
    //    dr["Frequency"] = 0;
    //    dr["Days"] = 0;
    //    dr["DaysType"] = string.Empty;
    //    dr["FoodName"] = string.Empty;
    //    dr["Remarks"] = string.Empty;

    ////    dr["DoseName"] = string.Empty;



    //    dt.Rows.Add(dr);
    //    dt.AcceptChanges();
    //    ViewState["ItemDetail"] = null;
    //    gvTreatmentPlan.DataSource = dt;
    //    gvTreatmentPlan.DataBind();

    //    ViewState["DataTableItem"] = dt;
    //}
    private void BindBlankServiceGrid()
    {

        DataTable dt = CreateServiceTable();
        DataRow dr = dt.NewRow();

        dr["ServiceiD"] = 0;
        dr["ServiceName"] = "N/A";


        //    dr["DoseName"] = string.Empty;



        dt.Rows.Add(dr);
        dt.AcceptChanges();
        ViewState["ServiceDetail"] = null;
        gvService.DataSource = dt;
        gvService.DataBind();

        ViewState["DataTableService"] = dt;
    }

    //protected void ddlPlanTemplates_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
    //    GetServiceData(plantype);
    //}
    protected void gvTreatmentPlan_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int i = 0;
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                // CheckBox chkboxgvTreatmentPlan = (CheckBox)e.Row.FindControl("chkboxgvTreatmentPlan");

                //if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
                //{
                //    ibtnDelete.Visible = false;
                //}
                //if (i == 0)
                //{
                //    chkboxgvTreatmentPlan.Visible = false;
                //}

                Label lblDose = (Label)e.Row.FindControl("lblDose");
                Label lblDoseUnit = (Label)e.Row.FindControl("lblDoseUnit");
                lblDose.Text = lblDose.Text + "/" + lblDoseUnit.Text;

                Label Days = (Label)e.Row.FindControl("lblDays");
                Label DaysType = (Label)e.Row.FindControl("lblDaysType");
                Days.Text = Days.Text + "/" + DaysType.Text;



                //LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumProvisionalDiagnosis.Edit].Controls[0];

                //if (common.myInt(hdnEncodedById.Value) > 0)
                //{
                //    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                //    {
                //        lnkEdit.Visible = false;
                //        ibtnDelete.Visible = false;
                //    }
                //}


            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }


    protected void cmbServiceName_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text.Length > 2)
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
                this.cmbServiceName.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);

            //  visible(hdnServiceId.Value);
        }
    }


    protected DataTable BindSearchCombo(String etext)
    {
        DataTable dt = new DataTable();
        DataView DV = new DataView();
        try
        {
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);


            //if (ViewState["ServiceData"] == null)
            //{
            dt = order.GetSearchServices(common.myInt(Session["HospitalLocationId"]), common.myInt(0), "0", etext, common.myInt(Session["FacilityId"]), 0, 0);
            //if (rdoOrder.SelectedValue == "OS")
            //{
            //    dt.Rows.Clear();
            //    dt.Rows.Add(dt.NewRow());
            //    dt.AcceptChanges();
            //}

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


    protected void cmbServiceName_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //visible(hdnServiceType.Value);
        if (cmbServiceName.SelectedValue != "")
        {
            ////  hdnServiceId.Value = cmbServiceName.SelectedValue;
            //    AddOrder("NEW", Convert.ToInt32(cmbServiceName.SelectedValue), 0);
        }
    }



    protected void btnPlanofcare_Onclick(object sender, EventArgs e)
    {
        BaseC.EMR emr = new BaseC.EMR(sConString);
        BaseC.EMRMasters emrmaster = new BaseC.EMRMasters(sConString);

        if (ddlPlanTemplates.SelectedValue == "0" || ddlPlanTemplates.SelectedValue == string.Empty)
        {
            Alert.ShowAjaxMsg("Please select template", Page);
            return;
        }

        if (!txtWPlanOfCare.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
        {
            //string  strPlanOfCare = strPlanOfCare.Replace("\n", "<br/>");



            int Templateid = common.myInt(ddlPlanTemplates.SelectedValue);
            int Brandid = 0;

            string Dose = string.Empty;
            int DoseUnitId = 0;
            int FrequencyId = 0;
            int Day = 0;
            string DaysType = string.Empty;
            int FoodRelationId = 0;
            string Remarks = string.Empty;
            int ServiceId = 0;
            int EncodedBy = 1;

            string strPlanOfCare = txtWPlanOfCare.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
            strPlanOfCare = strPlanOfCare.Replace("\n", "<br/>");

            string strInstructions = txtTreatmentPlanInstructions.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
            strInstructions = strInstructions.Replace("\n", "<br/>");

            string strChiefComplaint = txtChiefComplaint.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
            strChiefComplaint = strChiefComplaint.Replace("\n", "<br/>");
            string strHistory = txtHistory.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
            strHistory = strHistory.Replace("\n", "<br/>");
            string strExamination = txtExamination.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
            strExamination = strExamination.Replace("\n", "<br/>");

            string Diagnosis = common.myStr(ddlDiagnosiss.SelectedValue);

            int intChiefComplaintsDuration = common.myInt(ddlDuration.SelectedValue);
            string strChiefComplaintsDurationType = common.myStr(ddlDurationType.SelectedValue);


            int ICDID = common.myInt(ddlDiagnosiss.SelectedValue);
           
            string ICDDescription = common.myStr(ddlDiagnosiss.Text);
            string ICDCode = "";
           // string ICDCode = common.myStr(ddlDiagnosiss.SelectedItem.Attributes["ICDCode"]);


           string StrMsg = emrmaster.UpdateEMRTreatmentTemplate(Templateid, Brandid, DoseUnitId, FrequencyId, Day, DaysType, FoodRelationId, Remarks, ServiceId, EncodedBy, strPlanOfCare, strInstructions
                , strChiefComplaint, strHistory, strExamination, Diagnosis, intChiefComplaintsDuration, strChiefComplaintsDurationType
                , ICDCode, ICDID, ICDDescription);

            if (StrMsg == string.Empty)
            {
                //  lblMessage.Text = "Plan of care  Updated !";
                lblMessage.Text = "Updated successfully !";
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
                //  GetServiceData(plantype);
            }

        }

        else
        {

            Alert.ShowAjaxMsg("Please enter Text", Page);
            return;
        }

    }
    public void EnableForm(ControlCollection ctrls)
    {
        foreach (Control ctrl in ctrls)
        {
            if (ctrl is TextBox)
                ((TextBox)ctrl).Enabled = true;
            if (ctrl is Button)
                ((Button)ctrl).Enabled = true;
            else if (ctrl is DropDownList)
                ((DropDownList)ctrl).Enabled = true;
            else if (ctrl is CheckBox)
                ((CheckBox)ctrl).Enabled = true;
            else if (ctrl is RadioButton)
                ((RadioButton)ctrl).Enabled = true;
            else if (ctrl is HtmlInputButton)
                ((HtmlInputButton)ctrl).Disabled = false;
            else if (ctrl is HtmlInputText)
                ((HtmlInputText)ctrl).Disabled = false;
            else if (ctrl is HtmlSelect)
                ((HtmlSelect)ctrl).Disabled = false;
            else if (ctrl is HtmlInputCheckBox)
                ((HtmlInputCheckBox)ctrl).Disabled = false;
            else if (ctrl is HtmlInputRadioButton)
                ((HtmlInputRadioButton)ctrl).Disabled = false;


            if (ctrl is ImageButton)
                ((ImageButton)ctrl).Enabled = true;

            if (ctrl is LinkButton)
                ((LinkButton)ctrl).Enabled = true;

            EnableForm(ctrl.Controls);



        }
    }

    protected void btnDisableControl_OnClick(object sender, EventArgs e)
    {
        try
        {
            DisableForm(Page.Controls);
            btnEnableControl.Enabled = true;


        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }



    }

    protected void btnEnableControl_OnClick(object sender, EventArgs e)
    {
        try
        {
            EnableForm(Page.Controls);
            fillEMRTreatmentPlanTemplatesmaster();
            //if (hdnButtonId.Value != "")
            //{

            //    string ButtonId = hdnButtonId.Value;

            //    switch (ButtonId)
            //    {
            //        case "btnAddPrescriptionsClose":
            //            fillEMRTreatmentPlanTemplatesmaster();
            //            break;                                   


            //    }
            //}



            //btnAddPrescriptionsClose.Enabled = true;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }



    }
    public void DisableForm(ControlCollection ctrls)
    {
        foreach (Control ctrl in ctrls)
        {
            if (ctrl is TextBox)
                ((TextBox)ctrl).Enabled = false;
            if (ctrl is Button)
                ((Button)ctrl).Enabled = false;
            else if (ctrl is DropDownList)
                ((DropDownList)ctrl).Enabled = false;
            else if (ctrl is CheckBox)
                ((CheckBox)ctrl).Enabled = false;
            else if (ctrl is RadioButton)
                ((RadioButton)ctrl).Enabled = false;
            else if (ctrl is HtmlInputButton)
                ((HtmlInputButton)ctrl).Disabled = true;
            else if (ctrl is HtmlInputText)
                ((HtmlInputText)ctrl).Disabled = true;
            else if (ctrl is HtmlSelect)
                ((HtmlSelect)ctrl).Disabled = true;
            else if (ctrl is HtmlInputCheckBox)
                ((HtmlInputCheckBox)ctrl).Disabled = true;
            else if (ctrl is HtmlInputRadioButton)
                ((HtmlInputRadioButton)ctrl).Disabled = true;


            if (ctrl is ImageButton)
                ((ImageButton)ctrl).Enabled = false;

            if (ctrl is LinkButton)
                ((LinkButton)ctrl).Enabled = false;

            DisableForm(ctrl.Controls);

        }
    }

    protected void btnAddPlaneName_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMR/Masters/AddTreatmentPlan.aspx?Source=TreatmentMasters";
        RadWindow1.Height = 550;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "btnAddPlaneNameClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        //  RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }

    public void ddlDiagnosiss_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

        try
        {
            DataTable data = new DataTable();
            data = CreateTable();
            data = PopulateAllDiagnosis(e.Text);



            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                //ddlDiagnosiss = null;
               // this.ddlDiagnosiss.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                //RadCmbPatientSearch.Items.Add(new RadComboBoxItem(data.Rows[i]["CompanyName"].ToString(), data.Rows[i]["CompanyName"].ToString()));
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ICDDescription"];
                item.Value = data.Rows[i]["ICDID"].ToString();
                item.Attributes["ICDCode"] = data.Rows[i]["ICDCode"].ToString();

                this.ddlDiagnosiss.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
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
        DataColumn dc = new DataColumn("ID");
        dc.DefaultValue = 0;
        Dt.Columns.Add(dc);
        Dt.Columns.Add("ICDID");
        Dt.Columns.Add("IcdCode");
        Dt.Columns.Add("ICDDescription");
        Dt.Columns.Add("OnsetDate");
        Dt.Columns.Add("OnsetDateWithoutFormat");
        Dt.Columns.Add("LocationId");
        Dt.Columns.Add("PrimaryDiagnosis");
        Dt.Columns.Add("IsChronic");
        Dt.Columns.Add("IsQuery");
        Dt.Columns.Add("ConditionIds");
        Dt.Columns.Add("TypeId");
        Dt.Columns.Add("IsResolved");
        Dt.Columns.Add("DoctorId");
        Dt.Columns.Add("FacilityId");
        Dt.Columns.Add("Remarks");
        Dt.Columns.Add("TemplateFieldId");
        Dt.Columns.Add("IsFinalDiagnosis");
        Dt.Columns.Add("EncodedBy", System.Type.GetType("System.Int32"));

        return Dt;
    }

    private DataTable PopulateAllDiagnosis(string txt)
    {
        DataTable DT = new DataTable();
        BaseC.DiagnosisDA objDiag;
        DataSet ds = new DataSet();
        try
        {

            ViewState["BTN"] = "ALL";

            //if (Session["encounterid"] != null)
            //{
            objDiag = new BaseC.DiagnosisDA(sConString);
            ds = new DataSet();

            string strSearchCriteria = string.Empty;

            strSearchCriteria = "%" + txt + "%";
            ds = objDiag.BindDiagnosis(common.myInt(ddlCategory.SelectedValue), common.myInt(ddlSubCategory.SelectedValue), strSearchCriteria);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns.Add("Id");
                dt.Columns.Add("EncounterDate");
                DT = dt;
            }
            else
            {
                BindBlankGrid();
            }
            //  }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return DT;
    }

    private void BindDrpCategory()
    {
        try
        {
            ddlCategory.Items.Clear();

            objDiag = new BaseC.DiagnosisDA(sConString);

            ds = objDiag.BindCategory();

            RadComboBoxItem lsts;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                lsts = new RadComboBoxItem();
                lsts.Text = dr["GroupName"].ToString();
                lsts.Value = dr["GroupId"].ToString();
                lsts.Attributes.Add("GroupType", dr["GroupType"].ToString());
                ddlCategory.Items.Add(lsts);
            }
            ddlCategory.Items.Insert(0, new RadComboBoxItem("All", "0"));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindSubCategory(string GroupId)
    {
        try
        {
            ddlSubCategory.Items.Clear();
            if (GroupId != "")
            {
                objDiag = new BaseC.DiagnosisDA(sConString);
                ds = new DataSet();
                ds = objDiag.BindSubCategory(common.myStr(GroupId));

                ddlSubCategory.Items.Clear();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlSubCategory.DataSource = ds.Tables[0];
                    ddlSubCategory.DataValueField = "SubGroupId";
                    ddlSubCategory.DataTextField = "SubGroupName";
                    ddlSubCategory.DataBind();
                    ddlSubCategory.Items.Insert(0, new RadComboBoxItem("All", "0"));
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

    protected void ddlCategory_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            BindSubCategory(ddlCategory.SelectedValue);
            Hashtable hstInput = new Hashtable();
            hstInput.Add("@intGroupId", ddlCategory.SelectedValue);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDiagnosisList", hstInput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns.Add("Id");
                dt.Columns.Add("EncounterDate");

            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlSubCategory_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            Hashtable hstInput = new Hashtable();
            hstInput.Add("@intGroupId", ddlCategory.SelectedValue);
            hstInput.Add("@intSubGroupId", ddlSubCategory.SelectedValue);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDiagnosisList", hstInput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns.Add("Id");
                dt.Columns.Add("EncounterDate");

            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private DataTable BindBlankGrid()
    {
        DataTable dT = new DataTable();
        try
        {

            dT.Columns.Add("ICDID");
            dT.Columns.Add("ICDCode");
            dT.Columns.Add("ICDDescription");
            dT.Columns.Add("Id");
            dT.Columns.Add("EncounterDate");
            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ICDID"] = 0;
                if (Convert.ToString(ViewState["BTN"]) == "ALL")
                {
                    dr["ICDDescription"] = "No Data Found";

                }

                else if (Convert.ToString(ViewState["BTN"]) == "FAV")
                {
                    dr["ICDDescription"] = "No Favorite Found";

                }
                else
                {
                    dr["ICDDescription"] = "No Data Found";

                }
                dT.Rows.Add(dr);
            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dT;
    }

}
