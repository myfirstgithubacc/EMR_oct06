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
using System.Text;
using System.IO;

public partial class EMR_Dashboard_DrugFavourate : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 10;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

          //  Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");
            dvConfirmCancelOptions.Visible = false;
            if (!IsPostBack)
            {
                GetEMRDrugSetmaster();
                GetFavourateDrug(common.myInt(Session["DoctorID"]));
                GetSetdetails(0);

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
                strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]),0,0);
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



    protected void GetFavourateDrug(int DoctorId)
    {
        BaseC.Hospital bHos = new BaseC.Hospital(sConString);
        try
        {


            string ServiceName = "%";
            string strDepartmentType = "";
            gvService.Dispose();
            strDepartmentType = "'EQ', 'O' ";
            DataSet ds = bHos.GetEMRFavourateDetails(DoctorId);

            Session["FevNoofRow"] = ds.Tables[0].Rows.Count;

            if (ds.Tables[0].Rows.Count > 0)
            {

                gvProblemDetails.DataSource = ds.Tables[0];
                gvProblemDetails.DataBind();

            }


            else
            {


                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
                gvProblemDetails.DataSource = ds.Tables[0];
                gvProblemDetails.DataBind();

                GridViewRow gv1 = gvProblemDetails.Rows[0];
                //gv1 = gvProblemDetails.Rows[0];
                CheckBox chkboxgvTreatmentPlan = (CheckBox)gv1.FindControl("chkboxgvTreatmentPlan");
                chkboxgvTreatmentPlan.Enabled = false;
                //chkboxgvTreatmentPlan.Visible = false;

                ImageButton ibtnDelete = (ImageButton)gv1.FindControl("ibtnDelete");
                ibtnDelete.Enabled = false;
               // ibtnDelete.Visible = false;
                dr = null;


            }


         


        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void GetEMRDrugSetmaster()
    {
        try
        {
            BaseC.Hospital bHos = new BaseC.Hospital(sConString);
            DataSet ds = bHos.GetEMRDrugSetmaster(common.myInt(Session["HospitalLocationId"]), 0);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlPlanTemplates.DataSource = ds.Tables[0];
                    ddlPlanTemplates.DataTextField = "SetName";
                    ddlPlanTemplates.DataValueField = "SetId";
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
                    //// gvTreatmentPlan.DataSource = ds.Tables[0];
                    ////  gvTreatmentPlan.DataBind();
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




    protected void lnkDelete_OnClick(object sender, EventArgs e)
    {
        try
        {


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
        
        string gv1Value = string.Empty;
        string gv2Value = string.Empty;
        int counter = 0;
        int FavoriteCheckedCounter = 0;
        int SetCheckedCounter = 0;
        string strItemName = string.Empty;
        string strItemNameList = string.Empty;
        List<string> ItemList = new List<string>();
        StringBuilder sbItemList = new StringBuilder();
        string strPopup = "<script language='javascript' ID='script1'>" + "window.close();" + "</script>";

        try
        {
            //iterate second gridview
            foreach (GridViewRow row1 in gvProblemDetails.Rows)
            {
                if (row1.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (CheckBox)row1.FindControl("chkboxgvTreatmentPlan");

                    if (chk != null & chk.Checked)
                    {

                        HiddenField hdBrand_Prescriptions_ID = (HiddenField)row1.FindControl("hdnItemID");
                        gv1Value = hdBrand_Prescriptions_ID.Value;
                        counter++;
                        FavoriteCheckedCounter++;
                      
                    }

                    //iterate first gridview
                    foreach (GridViewRow row2 in gvService.Rows)
                    {
                        if (row2.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSet = (CheckBox)row2.FindControl("chkboxgvSet");
                            if (chkSet != null & chkSet.Checked)
                            {
                                HiddenField hdBrandiDSET = (HiddenField)row2.FindControl("hdnItemIDSet");
                                gv2Value = hdBrandiDSET.Value;
                                Label lblItemName = (Label)row2.FindControl("lblItemName");
                                strItemName = lblItemName.Text;
                                counter++;
                                SetCheckedCounter++;
                            }
                            //do comparison here
                            if (gv1Value.Contains(gv2Value))
                            {
                              ItemList.Add(common.myStr(strItemName.Trim()));
                            }
                            ItemList = ItemList.Distinct().ToList();
                        }
                    }
                }
            }
            if (ItemList.Count > 0 && common.myStr(ItemList).Trim() !="")
            {
                int BrandCount = 0;

                foreach (string BrandName in ItemList)
                {
                    if (BrandCount == 0)
                    {
                        //sbItemList.Append(BrandName);
                        strItemNameList = strItemNameList + BrandName;
                        BrandCount++;
                    }
                    else
                    {
                        strItemNameList = strItemNameList + "," + BrandName;
                        
                    }

                 }

                if (FavoriteCheckedCounter == 0 && SetCheckedCounter == 0)
                {
                    Alert.ShowAjaxMsg("Select the Medicine   ", Page.Page);
                    return;
                }

                else
                {
                    if (strItemNameList.Length != 0)
                    {

                        Alert.ShowAjaxMsg(strItemNameList + "   is Duplicate   ", Page.Page);
                        return;
                    }
                    else
                    {
                        saveDataNew();

                        
                        ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
                        return;

                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParentPage();", true);
                       
                    
                    }
                
                }

              
            }
            else
            {

                if (FavoriteCheckedCounter == 0 && SetCheckedCounter == 0)
                { 
                    Alert.ShowAjaxMsg("Select the Medicine   ", Page.Page);
                    return;
                }
                else
                {
                    saveDataNew();
                    
                    ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
                    return;

                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParentPage();", true);
                  
                
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
            SetCheckedCounter =0;
            FavoriteCheckedCounter = 0;

           

          //  BrandCount = 0;
        }
         
        

    }


    protected void ddlPlanTemplates_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        int setid = common.myInt(ddlPlanTemplates.SelectedValue);
        GetSetdetails(setid);
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

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches found...";
        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    private void BindGrid()
    {
        int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
       // GetServiceData(plantype);
    }



    protected void btnAddList_Onclick(object sender, EventArgs e)
    {

        DataTable tblItem = new DataTable();
        DataView DVItem = new DataView();
        BaseC.EMR emr = new BaseC.EMR(sConString);

        DataTable dt = emr.GetEMRExistingMedicationOrder(common.myInt(Session["HospitalLocationid"]), common.myInt(Session["FacilityId"]),
               common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, common.myStr(Session["OPIP"]));
        if (dt.Rows.Count > 0)
        {
            //dvConfirmAlreadyExistOptions.Visible = true;
            //lblItemName.Text = common.myStr(dt.Rows[0]["ItemName"]);
            //lblEnteredBy.Text = common.myStr(dt.Rows[0]["EnteredBy"]);
            //lblEnteredOn.Text = common.myStr(dt.Rows[0]["EncodedDate"]);
            dt.Dispose();
        }

    }


 

    


    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
     


    }





    protected void ddlPlanTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
        int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
     //   GetServiceData(plantype);
    }
    protected void gvProblemDetails_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        //int i = 0;
        try
        {

            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))

            //if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkboxgvTreatmentPlan = (CheckBox)e.Row.FindControl("chkboxgvTreatmentPlan");
                HiddenField ItemID = (HiddenField)e.Row.FindControl("hdnItemID");



                //if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
                //{
                //    ibtnDelete.Visible = false;
                //}


                //Label lblDose = (Label)e.Row.FindControl("lblDose");
                //Label lblDoseUnit = (Label)e.Row.FindControl("lblDoseUnit");

                //string dosecombiled = lblDose.Text + "  " + lblDoseUnit.Text;

                //lblDoseUnit.Visible = false;

                //Label Days = (Label)e.Row.FindControl("lblDays");
                //Label DaysType = (Label)e.Row.FindControl("lblDaysType");


                //DaysType.Visible = false;

                if (e.Row.RowType != DataControlRowType.Header)
                {
                    //if (common.myInt(ItemID.Value) == 0)
                    //{
                    //    chkboxgvTreatmentPlan.Checked = false;

                    //}
                    //else
                    //{
                    //    chkboxgvTreatmentPlan.Checked = true;

                    //}

                    //if (lblDose.Text == string.Empty)
                    //{
                    //    lblDose.Text = string.Empty;

                    //}
                    //else
                    //{

                    //    lblDose.Text = dosecombiled.ToString();
                    //}
                    //if (Days.Text == string.Empty)
                    //{
                    //    Days.Text = string.Empty;

                    //}
                    //else
                    //{
                    //    Days.Text = Days.Text + "  " + DaysType.Text;
                    //}
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





    protected void gvService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
           || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))

        //if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkboxgvSet = (CheckBox)e.Row.FindControl("chkboxgvSet");
            HiddenField SetID = (HiddenField)e.Row.FindControl("hdnSetIdSet");



            if (e.Row.RowType != DataControlRowType.Header)
            {
                if (common.myInt(SetID.Value) == 0)
                {
                    chkboxgvSet.Checked = false;

                }
                else
                {
                    chkboxgvSet.Checked = true;

                }
            }
        }


    }


    protected void chkboxgvTreatmentPlan_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gr = (GridViewRow)chk.Parent.Parent;
        // lblmsg.Text = gvService.DataKeys[gr.RowIndex].Value.ToString();

    }

   
    protected void saveDataNew()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.WardManagement objwd = new BaseC.WardManagement();
        string totalduration = string.Empty;
        string blankspace=" ";
        DataSet dsXml = new DataSet();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        StringBuilder strXML = new StringBuilder();
        StringBuilder strXML1 = new StringBuilder();
        ArrayList coll = new ArrayList();
        ArrayList coll1 = new ArrayList();

        StringBuilder strXMLFre = new StringBuilder();
        ArrayList collFre = new ArrayList();
        StringBuilder xmlTemplateDetails = new StringBuilder();
        StringBuilder strNonTabularP = new StringBuilder();

        try
        {


            if (Session["OPIP"] != null && (Session["OPIP"].ToString() == "O" || Session["OPIP"].ToString() == "E"))
            {
                #region OP Drug
       

                foreach (GridViewRow dataItem in gvProblemDetails.Rows)
                {
                    CheckBox chk = (CheckBox)dataItem.FindControl("chkboxgvTreatmentPlan");
                    if (chk != null & chk.Checked)
                    {


                        HiddenField hdnDose = (HiddenField)dataItem.FindControl("hdnDose");
                      
                        HiddenField hdnDurationText = (HiddenField)dataItem.FindControl("hdnDays");
                        HiddenField hdnDaysType = (HiddenField)dataItem.FindControl("hdnDaysType");
                        string strFinalDays = hdnDurationText.Value; //+ blankspace + hdnDaysType.Value;

                        HiddenField hdnRemarks = (HiddenField)dataItem.FindControl("hdnRemarks");

                        HiddenField hdBrand_Prescriptions_ID = (HiddenField)dataItem.FindControl("hdnItemID");
                        HiddenField hdnFrequencyId = (HiddenField)dataItem.FindControl("hdnFrequencyID");
                        HiddenField hdnPeriodType = (HiddenField)dataItem.FindControl("hdnDTypeId");
                        HiddenField hdnUnitID = (HiddenField)dataItem.FindControl("hdnDoseUnitID");
                        HiddenField hdnFoodRelation = (HiddenField)dataItem.FindControl("hdnFoodNameID");
                        HiddenField hdnDetailsId = (HiddenField)dataItem.FindControl("hdnDetailsId");
                        HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");

                        if (common.myInt(hdnDetailsId.Value) == 0 && common.myInt(hdnIndentId.Value) == 0)
                        {
                            //dataItem.FindControl("lblItemName")
                            coll.Add(common.myInt(common.myStr(hdBrand_Prescriptions_ID.Value)));//Brand TINYINT, 
                            coll.Add(common.myInt(common.myStr(hdnFrequencyId.Value)));//FrequencyId TINYINT,
                            coll.Add(common.myDec(Convert.ToDecimal(common.myStr(hdnDose.Value))));//Dose DECIMAL(10,3),
                            coll.Add(common.myStr(common.myStr(strFinalDays)));//Duration VARCHAR(20)
                            coll.Add(common.myStr(common.myStr(hdnPeriodType.Value)));//DURATION TYPE CHAR(1),
                            coll.Add(common.myStr(common.myStr(hdnRemarks.Value)));//INSTRUCTIONID VARCHAR(1000),
                            coll.Add(common.myInt(common.myStr(hdnUnitID.Value)));//UNITID INT,
                            coll.Add(common.myInt(common.myStr(hdnFoodRelation.Value)));//FoodRelationship INT, 
                            coll.Add(string.Empty);// variable sequence no     
                            coll.Add(false);
                            strXML.Append(common.setXmlTable(ref coll));

                        }
                    }

                }


                foreach (GridViewRow dataItem in gvService.Rows)
                {
                    CheckBox chk = (CheckBox)dataItem.FindControl("chkboxgvSet");
                    if (chk != null & chk.Checked)
                    {



                        HiddenField hdnSetId = (HiddenField)dataItem.FindControl("hdnSetIdSet");
                        HiddenField hdBrand_Prescriptions_ID = (HiddenField)dataItem.FindControl("hdnItemIDSet");
                        HiddenField hdnFrequencyIDSet = (HiddenField)dataItem.FindControl("hdnFrequencyIDSet");
                        HiddenField hdnFrequencySet = (HiddenField)dataItem.FindControl("hdnFrequencySet");

                        HiddenField hdnDTypeIdSet = (HiddenField)dataItem.FindControl("hdnDTypeIdSet");
                        HiddenField hdnDaysSet = (HiddenField)dataItem.FindControl("hdnDaysSet");

                        HiddenField hdnDaysDurationSet = (HiddenField)dataItem.FindControl("hdnDaysDurationSet");
                        totalduration = hdnDaysSet.Value + blankspace + hdnDaysDurationSet.Value;

                        Label lblDetailSET = (Label)dataItem.FindControl("lblDetailSET");

                        HiddenField hdnUnitIDSET = (HiddenField)dataItem.FindControl("hdnUnitIDSET");
                        HiddenField hdnFoodRelationSET = (HiddenField)dataItem.FindControl("hdnFoodRelationSET");

                        //if (common.myInt(hdnDetailsIdSet.Value) == 0 && common.myInt(hdnIndentIdSet.Value) == 0)
                        //{

                        coll.Add(common.myInt(common.myStr(hdBrand_Prescriptions_ID.Value)));//Brand TINYINT, 
                        coll.Add(common.myInt(common.myStr(hdnFrequencyIDSet.Value)));//FrequencyId TINYINT,
                        coll.Add(common.myDec(Convert.ToDecimal(common.myStr(hdnFrequencySet.Value))));//Dose DECIMAL(10,3),
                        coll.Add(common.myStr(common.myStr(totalduration)));//Duration VARCHAR(20)
                        coll.Add(common.myStr(common.myStr(hdnDTypeIdSet.Value)));//DURATION TYPE CHAR(1),
                        //coll.Add(common.myStr(common.myStr(lblRemarksSET.Text)));//INSTRUCTIONID VARCHAR(1000),
                        coll.Add(common.myStr(common.myStr(string.Empty)));//INSTRUCTIONID VARCHAR(1000),
                        coll.Add(common.myInt(common.myStr(hdnUnitIDSET.Value)));//UNITID INT,
                        coll.Add(common.myInt(common.myStr(hdnFoodRelationSET.Value)));//FoodRelationship INT, 
                        coll.Add(string.Empty);// variable sequence no     
                        coll.Add(false);
                        strXML.Append(common.setXmlTable(ref coll));

                        //  }


                    }
                }


                #endregion
            }
            else
            {
                #region IP Drug

                foreach (GridViewRow dataItem in gvProblemDetails.Rows)
                {
                        CheckBox chk = (CheckBox)dataItem.FindControl("chkboxgvService");
                        if (chk != null & chk.Checked)
                        {

                            HiddenField hdnDose = (HiddenField)dataItem.FindControl("hdnDose");
                         // string finaldosevalue = common.myStr(hdnDose.Value);
                            HiddenField hdnDurationText = (HiddenField)dataItem.FindControl("hdnDays");
                            Label lblInstruction = (Label)dataItem.FindControl("lblRemarks");
                            HiddenField hdBrand_Prescriptions_ID = (HiddenField)dataItem.FindControl("hdnItemID");
                            HiddenField hdnFrequencyId = (HiddenField)dataItem.FindControl("hdnFrequencyID");
                            HiddenField hdnPeriodType = (HiddenField)dataItem.FindControl("hdnDTypeId");
                            HiddenField hdnUnitID = (HiddenField)dataItem.FindControl("hdnDoseUnitID");
                            HiddenField hdnFoodRelation = (HiddenField)dataItem.FindControl("hdnFoodNameID");
                            HiddenField hdnDetailsId = (HiddenField)dataItem.FindControl("hdnDetailsId");
                            HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");

                            if (common.myInt(hdnDetailsId.Value) == 0 && common.myInt(hdnIndentId.Value) == 0)
                            {                                
                                //dataItem.FindControl("lblItemName")
                                coll.Add(common.myInt(common.myStr(hdBrand_Prescriptions_ID.Value)));//Brand TINYINT, 
                                coll.Add(common.myInt(common.myStr(hdnFrequencyId.Value)));//FrequencyId TINYINT,
                                coll.Add(common.myDec(Convert.ToDecimal(common.myStr(hdnDose.Value))));//Dose DECIMAL(10,3),
                                coll.Add(common.myStr(common.myStr(hdnDurationText.Value)));//Duration VARCHAR(20)
                                coll.Add(common.myStr(common.myStr(hdnPeriodType.Value)));//DURATION TYPE CHAR(1),
                                coll.Add(common.myStr(common.myStr(lblInstruction.Text)));//INSTRUCTIONID VARCHAR(1000),
                                coll.Add(common.myInt(common.myStr(hdnUnitID.Value)));//UNITID INT,
                                coll.Add(common.myInt(common.myStr(hdnFoodRelation.Value)));//FoodRelationship INT, 
                                coll.Add(string.Empty);// variable sequence no     
                                coll.Add(false);
                                strXML.Append(common.setXmlTable(ref coll));

                            }
                        }
                }

                foreach (GridViewRow dataItem in gvService.Rows)
                {
                    CheckBox chk = (CheckBox)dataItem.FindControl("chkboxgvSet");
                if (chk != null & chk.Checked)
                {



                    HiddenField hdnSetId = (HiddenField)dataItem.FindControl("hdnSetIdSet");
                    HiddenField hdBrand_Prescriptions_ID = (HiddenField)dataItem.FindControl("hdnItemIDSet");
                    HiddenField hdnFrequencyIDSet = (HiddenField)dataItem.FindControl("hdnFrequencyIDSet");
                    HiddenField hdnFrequencySet = (HiddenField)dataItem.FindControl("hdnFrequencySet");

                    HiddenField hdnDTypeIdSet = (HiddenField)dataItem.FindControl("hdnDTypeIdSet");
                    HiddenField hdnDaysSet = (HiddenField)dataItem.FindControl("hdnDaysSet");
                    Label lblDetailSET = (Label)dataItem.FindControl("lblDetailSET");

                    HiddenField hdnUnitIDSET = (HiddenField)dataItem.FindControl("hdnUnitIDSET");
                    HiddenField hdnFoodRelationSET = (HiddenField)dataItem.FindControl("hdnFoodRelationSET");

                    //if (common.myInt(hdnDetailsIdSet.Value) == 0 && common.myInt(hdnIndentIdSet.Value) == 0)
                    //{

                        coll.Add(common.myInt(common.myStr(hdBrand_Prescriptions_ID.Value)));//Brand TINYINT, 
                        coll.Add(common.myInt(common.myStr(hdnFrequencyIDSet.Value)));//FrequencyId TINYINT,
                        coll.Add(common.myDec(Convert.ToDecimal(common.myStr(hdnFrequencySet.Value))));//Dose DECIMAL(10,3),
                        coll.Add(common.myStr(common.myStr(hdnDaysSet.Value)));//Duration VARCHAR(20)
                        coll.Add(common.myStr(common.myStr(hdnDTypeIdSet.Value)));//DURATION TYPE CHAR(1),
                        coll.Add(common.myStr(common.myStr(string.Empty)));//INSTRUCTIONID VARCHAR(1000),
                        coll.Add(common.myInt(common.myStr(hdnUnitIDSET.Value)));//UNITID INT,
                        coll.Add(common.myInt(common.myStr(hdnFoodRelationSET.Value)));//FoodRelationship INT, 
                        coll.Add(string.Empty);// variable sequence no     
                        coll.Add(false);
                        strXML.Append(common.setXmlTable(ref coll));

                  //  }


                }
                }



                #endregion
            }

            if (strXML.ToString() == string.Empty)
            {
                lblMessage.Text = "Please add medicine before saving !";
                return;
            }
            #region PlanofCare

            coll = new ArrayList();
            bindVisitRecord(5895);
            if (!txtWPlanOfCare.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strPlanOfCare = txtWPlanOfCare.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strPlanOfCare = strPlanOfCare.Replace("\n", "<br/>");

                coll = new ArrayList();
                coll.Add(83911); //coll.Add(item2.Cells[0].Text);
                coll.Add("W");
                coll.Add(strPlanOfCare);
                coll.Add("0");
                coll.Add(0); //coll.Add(RowCaptionId);
                if (!common.myStr(hdnPlanOfCareRecordId.Value).Equals(string.Empty))
                {
                    coll.Add(common.myInt(hdnPlanOfCareRecordId.Value));
                }
                else
                {
                    coll.Add(common.myInt(ViewState["RecordId"]));
                }
                coll.Add(22735);
                strNonTabularP.Append(common.setXmlTable(ref coll));
            }


            xmlTemplateDetails.Append(strNonTabularP.ToString());
            #endregion
            
            Hashtable hshOutput = new Hashtable();
            if (Session["OPIP"] != null && Session["OPIP"].ToString() == "I")
            {


          

                hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                   common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), 0, 0,
                   0, strXML.ToString(), common.myInt(Session["UserId"]), xmlTemplateDetails.ToString(), common.myStr(Session["OPIP"]),string.Empty,0,string.Empty);


              
            }
            else
            {

                //hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                //                  common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), 0, 0,
                //                  0, strXML.ToString(), common.myInt(Session["UserId"]), xmlTemplateDetails.ToString(), common.myStr(Session["OPIP"]));

                hshOutput =
objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"])
, common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]), common.myInt(Session["UserId"]), 0, strXML.ToString(), 0);
            }



            if (hshOutput["@chvErrorStatus"].ToString() == "Data Saved!")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Data Saved Successfully";
            }
            else
            {

            }



        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }




    }


    private void bindVisitRecord(int TemplateId)
    {
        clsIVF objivf = new clsIVF(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objivf.getEMRTemplateVisitRecoreds(common.myInt(Session["EncounterId"]), common.myInt(TemplateId), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                ViewState["RecordId"] = 1;
            }
            else
            {
                int rCount = ds.Tables[0].Rows.Count;
                if (rCount > 0)
                {
                    ViewState["RecordId"] = common.myInt(ds.Tables[0].Rows[rCount - 1]["RecordId"]) + 1;
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
            objivf = null;
            ds.Dispose();
        }
    }


    protected void GetSetdetails(int SetId)
    { 
        DataSet ds=new DataSet();
        BaseC.Hospital bHos = new BaseC.Hospital(sConString);
        try
        {

            ds = bHos.GetEMRDrugSetdetails(common.myInt(Session["HospitalLocationId"]), SetId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvService.DataSource = ds.Tables[0];
                gvService.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
                gvService.DataSource = ds.Tables[0];
                gvService.DataBind();

                GridViewRow gv1 =gvService.Rows[0];
                //gv1 = gvProblemDetails.Rows[0];
                CheckBox chkgvset = (CheckBox)gv1.FindControl("chkboxgvSet");
                chkgvset.Enabled = false;
                //chkgvset.Style.Add(HtmlTextWriterStyle.Display, "none");
              //  chkgvset.Visible = false;
                dr = null;
            
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        
    }

    protected void gvProblemDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {

         DataSet ds=new DataSet();
        BaseC.clsEMR clemr = new BaseC.clsEMR(sConString);
        string StrDelMessage=string.Empty;
        string  FormularyType=string.Empty;
         if (e.CommandName == "ItemDelete")
         {
            //int ItemId = common.myInt(e.CommandArgument);
            dvConfirmCancelOptions.Visible = true;
            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);    
            HiddenField hdnItemID = (HiddenField)row.FindControl("hdnItemID");
            int ItemId = common.myInt(common.myStr(hdnItemID.Value));
            ViewState["ItemIdforDelete"] = ItemId;          
      
         }
     }


    protected void ButtonOk_OnClick(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR clemr = new BaseC.clsEMR(sConString);
        string StrDelMessage = string.Empty;
        string FormularyType = string.Empty;
        BaseC.EMRProblems objbc2 = new BaseC.EMRProblems(sConString);
      
          StrDelMessage = clemr.DeleteFavoriteDrugs(common.myInt(Session["DoctorID"]), common.myInt(ViewState["ItemIdforDelete"]), 
                                FormularyType, common.myInt(Session["UserId"]), 0);

        if (StrDelMessage == "Favourite drug deleted")
        {
            lblMessage.Text = StrDelMessage;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
            GetFavourateDrug(common.myInt(Session["DoctorID"]));
          
        }
        else
        {
            lblMessage.Text = StrDelMessage;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //Alert.ShowAjaxMsg(StrDelMessage.ToString(), Page.Page);
            //return;            

        }
      
        dvConfirmCancelOptions.Visible = false;
        ViewState["ItemIdforDelete"] = 0;
    }
    protected void ButtonCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmCancelOptions.Visible = false;
    }

}

