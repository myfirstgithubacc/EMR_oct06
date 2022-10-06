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

public partial class EMR_Dashboard_OrderFavourate : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 10;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            dvConfirmCancelOptions.Visible = false;
            if (!IsPostBack)
            {
                GetEMROrderSetmaster();
                GetFavourateOrders(common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]));
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
    //protected void GetServiceData()
    //{
    //    try
    //    {
    //        string ServiceName = "%";
    //        string strDepartmentType = "";

    //        strDepartmentType = "'EQ', 'O' ";

    //        BaseC.RestFulAPI objCommonService = new BaseC.RestFulAPI(sConString);
    //        DataSet ds = objCommonService.GetHospitalServices(common.myInt(Session["HospitallocationId"]), 0, 0,
    //            strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]));
    //        //ddlService.DataSource = ds.Tables[0];
    //        //ddlService.DataTextField = "ServiceName";
    //        //ddlService.DataValueField = "ServiceId";
    //        //ddlService.DataBind();

    //    }
    //    catch (Exception Ex)
    //    {

    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}



    protected void GetFavourateOrders(int DoctorId,int FacilityId)
    {
        BaseC.Hospital bHos = new BaseC.Hospital(sConString);
        try
        {


            string ServiceName = "%";
            string strDepartmentType = "";
            gvService.Dispose();
            strDepartmentType = "'EQ', 'O' ";

          
            DataSet ds = bHos.GetEMRFavourateServiceOrdersDetails(common.myInt(DoctorId), FacilityId);

         
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
              
                CheckBox chkboxgvTreatmentPlan = (CheckBox)gv1.FindControl("chkboxgvTreatmentPlan");
                chkboxgvTreatmentPlan.Enabled = false;
          
                ImageButton ibtnDelete = (ImageButton)gv1.FindControl("ibtnDelete");
                ibtnDelete.Enabled = false;
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
    protected void GetEMROrderSetmaster()
    {
        try
        {
            BaseC.Hospital bHos = new BaseC.Hospital(sConString);
            DataSet ds = bHos.GetEMROrderSetmaster(common.myInt(Session["HospitalLocationId"]), 0);
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

                        HiddenField hdBrand_Prescriptions_ID = (HiddenField)row1.FindControl("hdnServiceId");
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
                                HiddenField hdBrandiDSET = (HiddenField)row2.FindControl("hdnServiceIdSet");
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
                    Alert.ShowAjaxMsg("Select the Service   ", Page.Page);
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
                        //return;
                    
                    }
                
                }

              
            }
            else
            {

                if (FavoriteCheckedCounter == 0 && SetCheckedCounter == 0)
                { 
                    Alert.ShowAjaxMsg("Select the Service   ", Page.Page);
                    return;
                }
                else
                {
                    saveDataNew();
                   
                    ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
                    return;


                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParentPage();", true);
                    //return;
                
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
                HiddenField ItemID = (HiddenField)e.Row.FindControl("hdnServiceId");



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
           //HiddenField SetID = (HiddenField)e.Row.FindControl("hdnSetIdSet");
            HiddenField ServiceId = (HiddenField)e.Row.FindControl("hdnServiceIdSet");
            

            if (e.Row.RowType != DataControlRowType.Header)
            {
                if (common.myInt(ServiceId.Value) == 0)
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
        try
        {
            


            #region added by niraj
        
            try
            {
              

                BaseC.EMROrders order = new BaseC.EMROrders(sConString);
                string doctorid = "0";
                if (Request.QueryString["For"] != null && Request.QueryString["DoctorId"] != null)
                {
                    doctorid = common.myStr(Request.QueryString["DoctorId"]);
                }
                else
                {
                    doctorid = Session["EmployeeId"].ToString();
                }
                StringBuilder strXML = new StringBuilder();
                foreach (GridViewRow row in gvService.Rows)
                {
                  

                     CheckBox chk = (CheckBox)row.FindControl("chkboxgvSet");
                     if (chk != null & chk.Checked)
                     {
                         HiddenField hdnServiceIDset = (HiddenField)row.FindControl("hdnServiceIdSet");



                         #region Add Set Service Order



                         HiddenField hdnRequestToDepartment = new HiddenField();
                         hdnRequestToDepartment.Value = "0";

                         if (hdnRequestToDepartment.Value != "1")
                         {
                             strXML.Append("<Table1><c1>");
                             strXML.Append(common.myInt(hdnServiceIDset.Value));
                             strXML.Append("</c1><c2>");
                             strXML.Append("</c2><c3>");
                             strXML.Append(common.myInt(0));
                             strXML.Append("</c3><c4>");
                             strXML.Append(doctorid);
                             strXML.Append("</c4><c5>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c5><c6>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c6><c7>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c7><c8>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c8><c9>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c9><c10>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c10><c11>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c11><c12>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c12><c13>");
                             strXML.Append(common.myInt(0));
                             strXML.Append("</c13><c14>");
                             strXML.Append("0");
                             strXML.Append("</c14><c15>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c15><c16>");
                             strXML.Append("0");
                             strXML.Append("</c16><c17>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c17><c18>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c18><c19>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c19><c20>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c20><c21>");
                             strXML.Append(string.Empty);
                             strXML.Append("</c21><c22>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c22><c23>");
                             strXML.Append(0);
                             strXML.Append("</c23><c24>");
                             strXML.Append(0);
                             strXML.Append("</c24><c25>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c25><c26>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c26><c27>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c27><c28>");
                             strXML.Append(common.myInt(Session["FacilityId"]));
                             strXML.Append("</c28><c29>");
                             strXML.Append(0);
                             strXML.Append("</c29><c30>");
                             strXML.Append(string.Empty);
                             strXML.Append("</c30><c31>");
                             strXML.Append(DBNull.Value);
                             strXML.Append("</c31></Table1>");

                         }
                     }
                }

                    #endregion



                #region Favorite details

                foreach (GridViewRow row in gvProblemDetails.Rows)
                {
                    //string ServiceId = row.Cells[0].Text;


                    HiddenField hdnServiceID = (HiddenField)row.FindControl("hdnServiceiD");
                    CheckBox chk = (CheckBox)row.FindControl("chkboxgvTreatmentPlan");
                    if (chk != null & chk.Checked)
                    {

                        //Added By Abhishek Goel



                        //Added By Abhishek Goel
                        //////HiddenField hdnRequestToDepartment = (HiddenField)row.FindControl("hdnRequestToDepartment");
                        HiddenField hdnRequestToDepartment = new HiddenField();
                        hdnRequestToDepartment.Value = "0";

                        if (hdnRequestToDepartment.Value != "1")
                        {
                            strXML.Append("<Table1><c1>");
                            strXML.Append(common.myInt(hdnServiceID.Value));
                            strXML.Append("</c1><c2>");
                            strXML.Append("</c2><c3>");
                            // strXML.Append(1);
                            //////strXML.Append(common.myInt(lblUnits.Text) > 0 ? common.myInt(lblUnits.Text) : 1);//Units
                            strXML.Append(common.myInt(0));//Units
                            strXML.Append("</c3><c4>");
                            strXML.Append(doctorid);
                            strXML.Append("</c4><c5>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c5><c6>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c6><c7>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c7><c8>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c8><c9>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c9><c10>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c10><c11>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c11><c12>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c12><c13>");
                            strXML.Append(common.myInt(0));
                            strXML.Append("</c13><c14>");
                            strXML.Append("0");
                            strXML.Append("</c14><c15>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c15><c16>");
                            strXML.Append("0");
                            strXML.Append("</c16><c17>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c17><c18>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c18><c19>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c19><c20>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c20><c21>");
                            strXML.Append(string.Empty);
                            //////strXML.Append(lblRemarks.Text);
                            strXML.Append("</c21><c22>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c22><c23>");
                            strXML.Append(0);
                            strXML.Append("</c23><c24>");
                            strXML.Append(0);
                            strXML.Append("</c24><c25>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c25><c26>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c26><c27>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c27><c28>");
                            strXML.Append(common.myInt(Session["FacilityId"]));
                            strXML.Append("</c28><c29>");
                            strXML.Append(0);
                            strXML.Append("</c29><c30>");
                            strXML.Append(string.Empty);

                            strXML.Append("</c30><c31>");
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c31></Table1>");

                        }
                    }
                }
                #endregion


                ArrayList coll = new ArrayList();
                StringBuilder strXMLAleart = new StringBuilder();

                if (strXML.ToString() != "")
                {
                    DataSet ds = new DataSet();
                    ds = order.GetEncounterCompany(common.myInt(Session["EncounterId"]));
                    string sChargeCalculationRequired = "Y";
                    string stype = "P" + ds.Tables[0].Rows[0]["opip"].ToString().Trim();
                    string opip = ds.Tables[0].Rows[0]["opip"].ToString().Trim();


                    //CompanyCode,InsuranceCode,CardId
                   int CompanyId=0 , InsuranceId=0 , CardId=0 ;

                    if (common.myInt(ds.Tables[0].Rows[0]["CompanyCode"]) != 0)
                    {

                        CompanyId = common.myInt(ds.Tables[0].Rows[0]["CompanyCode"]);

                    }
                    if (common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"]) != 0)
                    {

                        InsuranceId = common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"]);

                    }
                    if (common.myInt(ds.Tables[0].Rows[0]["CardId"]) != 0)
                    {

                         CardId = common.myInt(ds.Tables[0].Rows[0]["CardId"]);

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
                    hshOut = order.saveOrders(Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(Session["FacilityId"].ToString()),
                                        common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), strXML.ToString(),
                                        strXMLAleart.ToString(), common.myStr(string.Empty), Convert.ToInt32(Session["UserID"].ToString()),
                                        common.myInt(doctorid), CompanyId, stype, common.myStr("E"), common.myStr(opip), InsuranceId,
                                        CardId, Convert.ToDateTime(DateTime.Now), sChargeCalculationRequired, true, 1,
                                        RequestId, common.myStr(ViewState["vsTemplateDataDetails"]), common.myInt(Session["EntrySite"]));

                    if (hshOut["chvErrorStatus"].ToString().Length == 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        ViewState["OrderId"] = hshOut["intOrderId"];
                        lblMessage.Text = "Data Saved Successfully";
                        lblMessage.Font.Bold = true;

                        Session["TemplateDataDetails"] = null;
                        //////      BindBlnkGrid();

                        if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                        }
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "There is some error while saving order." + hshOut["chvErrorStatus"].ToString();
                    }

                    ViewState["GridData"] = "";

                }
                else
                {
                    Alert.ShowAjaxMsg("No Service selected", Page);
                    return;
                }

                // ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;
                objException.HandleException(Ex);
            }

           
            //addItem();

            // saveData();           
            //   BindGrid();
            #endregion

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

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParentPage();", true);
        return;

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

            ds = bHos.GetEMRServiceSetdetails(common.myInt(Session["HospitalLocationId"]), SetId);

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
            HiddenField hdnItemID = (HiddenField)row.FindControl("hdnServiceId");
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
      
        
        StrDelMessage = clemr.DeleteFavoriteOrder(common.myInt(Session["EmployeeId"]), common.myInt(ViewState["ItemIdforDelete"]), common.myInt(Session["UserId"]));
        if (StrDelMessage == "Service Deleted from Favourtes successfull")
        {
            lblMessage.Text = StrDelMessage;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            int plantype = common.myInt(ddlPlanTemplates.SelectedValue);         

            GetFavourateOrders(common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]));
            
          
        }
        else
        {
            lblMessage.Text = StrDelMessage;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    }
      
        dvConfirmCancelOptions.Visible = false;
        ViewState["ItemIdforDelete"] = 0;
    }
    protected void ButtonCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmCancelOptions.Visible = false;
    }
   
}

