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

public partial class EMR_Dashboard_TreatmentPlan : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 10;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["EMRPrescriptionDoseShowInFractionalValue"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                        common.myInt(Session["FacilityId"]), "EMRPrescriptionDoseShowInFractionalValue", sConString);

                fillEMRTreatmentPlanTemplatesmaster();
                BindDiagnosis();
                GetServiceData(0);

                if (common.myStr(Request.QueryString["CloseButtonShow"]) == "No")
                {
                    btnClose.Visible = false;
                }
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                {
                    btnSave.Visible = false;
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

    protected void GetServiceData(int TemplatePlanId)
    {
        BaseC.Hospital bHos = new BaseC.Hospital(sConString);
        DataSet ds = new DataSet();
        try
        {
            gvService.Dispose();
            gvDiagnosisDetails.Dispose();
            ds = bHos.GetEMRTreatmentPlanSpecialities(TemplatePlanId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvService.DataSource = ds.Tables[0];
                gvService.DataBind();
                ViewState["FirstTimegvService"] = ds.Tables[0];
            }
            else
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);

                gvService.DataSource = ds.Tables[0];
                gvService.DataBind();
                GridViewRow gv1 = gvService.Rows[0];
                //gv1 = gvProblemDetails.Rows[0];
                HiddenField ServiceiD = (HiddenField)gv1.FindControl("hdnServiceiD");
                CheckBox chkServiceId = (CheckBox)gv1.FindControl("chkboxgvService");

                if (common.myInt(ServiceiD.Value) == 0)
                {
                    chkServiceId.Checked = false;
                }
                dr = null;
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                gvProblemDetails.DataSource = ds.Tables[1];
                gvProblemDetails.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[1].NewRow();
                ds.Tables[1].Rows.Add(dr);
                gvProblemDetails.DataSource = ds.Tables[1];
                gvProblemDetails.DataBind();
                dr = null;
            }
            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    txtWPlanOfCare.Text = common.myStr(ds.Tables[2].Rows[0]["PlanofCare"]);
                    txtWPlanOfCare.Text = common.clearHTMLTags(txtWPlanOfCare.Text);

                    txtInstructions.Text = common.myStr(ds.Tables[2].Rows[0]["Instructions"]);
                    txtInstructions.Text = common.clearHTMLTags(txtInstructions.Text);

                    txtChiefComplaint.Text = common.myStr(ds.Tables[2].Rows[0]["ChiefComplaints"]);
                    txtChiefComplaint.Text = common.clearHTMLTags(txtChiefComplaint.Text);

                    txtHistory.Text = common.myStr(ds.Tables[2].Rows[0]["History"]);
                    txtHistory.Text = common.clearHTMLTags(txtHistory.Text);


                    txtExamination.Text = common.myStr(ds.Tables[2].Rows[0]["Examination"]);
                    txtExamination.Text = common.clearHTMLTags(txtExamination.Text);



                    //txtDiagnosis.Text = common.myStr(ds.Tables[2].Rows[0]["Diagnosis"]);
                    //txtDiagnosis.Text = common.clearHTMLTags(txtDiagnosis.Text);

                    //  ddlDiagnosiss.SelectedIndex = ddlDuration.Items.IndexOf(ddlDuration.Items.FindItemByValue(common.myStr(ds.Tables[2].Rows[0]["Diagnosis"])));

                    ddlDuration.SelectedIndex = ddlDuration.Items.IndexOf(ddlDuration.Items.FindItemByValue(common.myStr(ds.Tables[2].Rows[0]["ChiefComplaintsDuration"])));
                    ddlDurationType.SelectedIndex = ddlDurationType.Items.IndexOf(ddlDurationType.Items.FindItemByValue(common.myStr(ds.Tables[2].Rows[0]["ChiefComplaintsDurationType"]).Trim()));


                }
                if (ds.Tables.Count > 2)
                {
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        gvDiagnosisDetails.DataSource = ds.Tables[3];
                        gvDiagnosisDetails.DataBind();
                        foreach (GridViewRow row in gvDiagnosisDetails.Rows)
                        {
                            CheckBox chkgvDiagnosisDetails = (CheckBox)row.FindControl("chkgvDiagnosisDetails");
                            chkgvDiagnosisDetails.Checked = true;
                        }
                    }
                    else
                    {
                        DataRow dr = ds.Tables[3].NewRow();
                        ds.Tables[3].Rows.Add(dr);
                        gvDiagnosisDetails.DataSource = ds.Tables[3];
                        gvDiagnosisDetails.DataBind();

                        foreach (GridViewRow row in gvDiagnosisDetails.Rows)
                        {
                            CheckBox chkgvDiagnosisDetails = (CheckBox)row.FindControl("chkgvDiagnosisDetails");
                            chkgvDiagnosisDetails.Checked = false;
                        }
                        dr = null;
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
            ds.Dispose();
            bHos = null;
        }
    }
    protected void fillEMRTreatmentPlanTemplatesmaster()
    {
        BaseC.Hospital bHos = new BaseC.Hospital(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = bHos.GetEMRTreatmentPlanTemplatesmaster(common.myInt(Session["FacilityId"]), common.myInt(Session["LoginDepartmentId"]),
                                                        common.myInt(Session["EmployeeId"]));

            if (ds.Tables.Count > 0)
            {
                ddlPlanTemplates.DataSource = ds.Tables[0];
                ddlPlanTemplates.DataTextField = "TemplateName";
                ddlPlanTemplates.DataValueField = "TemplateId";
                ddlPlanTemplates.DataBind();
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
            bHos = null;
            ds.Dispose();
        }
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        string stype = string.Empty, opip = string.Empty;
        int CompanyId = 0, InsuranceId = 0;

        try
        {
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Treatment Template Name.", Page.Page);
                return;
            }

            ////foreach (GridViewRow gvrow in gvProblemDetails.Rows)
            ////{
            ////    CheckBox chk = (CheckBox)gvrow.FindControl("chkboxgvTreatmentPlan");
            ////    if (chk != null & chk.Checked)
            ////    {
            ////        HiddenField ItemID = (HiddenField)gvrow.FindControl("hdnItemID");
            ////        Label ItemName = (Label)gvrow.FindControl("lblItemName");
            ////        HiddenField DoseUnitID = (HiddenField)gvrow.FindControl("hdnDoseUnitID");
            ////        Label Dose = (Label)gvrow.FindControl("lblDose");
            ////        Label DoseUnit = (Label)gvrow.FindControl("lblDoseUnit");

            ////        HiddenField FrequencyID = (HiddenField)gvrow.FindControl("hdnFrequencyID");
            ////        Label Frequency = (Label)gvrow.FindControl("lblFrequency");
            ////        Label Days = (Label)gvrow.FindControl("lblDays");
            ////        Label DaysType = (Label)gvrow.FindControl("lblDaysType");
            ////        HiddenField FoodNameID = (HiddenField)gvrow.FindControl("hdnFoodNameID");
            ////        Label FoodName = (Label)gvrow.FindControl("lblFoodName");
            ////        Label Remarks = (Label)gvrow.FindControl("lblRemarks");
            ////    }
            ////}

            ////foreach (GridViewRow gvrow in gvService.Rows)
            ////{
            ////    CheckBox chk = (CheckBox)gvrow.FindControl("chkboxgvService");
            ////    if (chk != null & chk.Checked)
            ////    {
            ////        HiddenField ServiceiD = (HiddenField)gvrow.FindControl("hdnServiceiD");
            ////        Label ServiceName = (Label)gvrow.FindControl("lblServiceName");
            ////    }
            ////}

            #region added by niraj
            //added ny niraj for saving Orders Start
            try
            {
                //////if (!isSave())
                //////{
                //////    return;
                //////}

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

                //StringBuilder strXMLDiagnosisDetails = new StringBuilder();
                //ArrayList col = new ArrayList();
                //foreach (GridViewRow row in gvDiagnosisDetails.Rows)
                //{
                //    CheckBox chkgvDiagnosisDetails = (CheckBox)row.FindControl("chkgvDiagnosisDetails");
                //    CheckBox lblICDCode = (CheckBox)row.FindControl("lblICDCode");
                //    if (common.myBool(chkgvDiagnosisDetails.Checked) && chkgvDiagnosisDetails != null)
                //    {
                //        col.Add(common.myInt(lblICDCode.Text));
                //    }
                //}
                //strXMLDiagnosisDetails.Append(common.setXmlTable(ref col));

                StringBuilder strXML = new StringBuilder();
                foreach (GridViewRow row in gvService.Rows)
                {
                    //string ServiceId = row.Cells[0].Text;

                    CheckBox chkboxgvService = (CheckBox)row.FindControl("chkboxgvService");

                    HiddenField hdnServiceID = (HiddenField)row.FindControl("hdnServiceiD");
                    //if (common.myStr(hdnServiceID.Value).Equals(string.Empty))
                    //{
                    //    Alert.ShowAjaxMsg("No Service selected", Page);
                    //    return;
                    //}


                    //Added By Abhishek Goel



                    //Added By Abhishek Goel
                    //////HiddenField hdnRequestToDepartment = (HiddenField)row.FindControl("hdnRequestToDepartment");
                    HiddenField hdnRequestToDepartment = new HiddenField();
                    hdnRequestToDepartment.Value = "0";
                    if (common.myBool(chkboxgvService.Checked) && chkboxgvService != null)
                    {
                        if (hdnRequestToDepartment.Value != "1")
                        {
                            strXML.Append("<Table1><c1>");
                            strXML.Append(common.myInt(hdnServiceID.Value));
                            strXML.Append("</c1><c2>");
                            strXML.Append("</c2><c3>");
                            // strXML.Append(1);
                            //////strXML.Append(common.myInt(lblUnits.Text) > 0 ? common.myInt(lblUnits.Text) : 1);//Units
                            strXML.Append(common.myInt(1));//Units
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

                            //////strXML.Append(hdnICDCode.Value);
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
                            //////if (chkStat.Checked)
                            //////    strXML.Append(1);
                            //////else
                            //////    strXML.Append(0);
                            strXML.Append(0);
                            strXML.Append("</c29><c30>");
                            //////strXML.Append(hdnExcludedServices.Value);
                            strXML.Append(string.Empty);

                            // Added By Abhishek Goel
                            // strXML.Append("</c30></Table1>");                    
                            strXML.Append("</c30><c31>");
                            //////if (!string.IsNullOrEmpty(lblTestDate.Text))
                            //////    strXML.Append(lblTestDate.Text);
                            //////else
                            strXML.Append(DBNull.Value);
                            strXML.Append("</c31></Table1>");
                            // Added By Abhishek Goel
                        }
                    }
                }
                ArrayList coll = new ArrayList();
                StringBuilder strXMLAleart = new StringBuilder();

                if (strXML.ToString() != "")
                {
                    DataSet ds = new DataSet();
                    ds = order.GetEncounterCompany(common.myInt(Session["EncounterId"]));
                    string sChargeCalculationRequired = "Y";
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (common.myInt(ds.Tables[0].Rows.Count) > 0)
                            {
                                stype = "P" + ds.Tables[0].Rows[0]["opip"].ToString().Trim();
                                opip = ds.Tables[0].Rows[0]["opip"].ToString().Trim();
                                CompanyId = common.myInt(ds.Tables[0].Rows[0]["CompanyCode"]);
                                InsuranceId = common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"]);
                                ViewState["DOCTORID"] = common.myInt(ds.Tables[0].Rows[0]["DOCTORID"]);
                            }
                        }
                    }

                    //int CompanyId = 0, InsuranceId = 0, CardId = 0;
                    int CardId = 0;

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
                //else
                //{
                //    Alert.ShowAjaxMsg("No Service selected", Page);
                //    return;
                //}

                // ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
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

            saveDataNew();

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


    protected void ddlPlanTemplates_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
        GetServiceData(plantype);
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
            return dt;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return dt;
        }
        finally
        {
            objEMRAllergy = null;
            dsSearch.Dispose();
            DV.Dispose();
            dt.Dispose();
        }
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
        GetServiceData(plantype);
    }

    protected void btnAddList_Onclick(object sender, EventArgs e)
    {
        DataTable tblItem = new DataTable();
        DataView DVItem = new DataView();
        DataTable dt = new DataTable();
        BaseC.EMR emr = new BaseC.EMR(sConString);
        try
        {
            dt = emr.GetEMRExistingMedicationOrder(common.myInt(Session["HospitalLocationid"]), common.myInt(Session["FacilityId"]),
                  common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, common.myStr(Session["OPIP"]));
            if (dt.Rows.Count > 0)
            {
                //dvConfirmAlreadyExistOptions.Visible = true;
                //lblItemName.Text = common.myStr(dt.Rows[0]["ItemName"]);
                //lblEnteredBy.Text = common.myStr(dt.Rows[0]["EnteredBy"]);
                //lblEnteredOn.Text = common.myStr(dt.Rows[0]["EncodedDate"]);

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
            tblItem.Dispose();
            DVItem.Dispose();
            emr = null;
            dt.Dispose();
        }
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
    protected DataTable CreateServiceTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ServiceId", typeof(int));
        dt.Columns.Add("ServiceName", typeof(String));
        return dt;
    }

    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dtDel = new DataTable();
        DataTable dtService = new DataTable();
        DataView dvService = new DataView();
        DataView dvDel = new DataView();

        try
        {
            if (e.CommandName == "ItemDelete")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                // int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                // int IndentId = common.myInt(((HiddenField)row.FindControl("hdnIndentId")).Value);

                //////Label lblBrand_Prescriptions_ID = (Label)row.FindControl("lblItemName");
                HiddenField hdnServiceId = (HiddenField)row.FindControl("hdnServiceiD");

                int ServiceId = common.myInt(common.myStr(hdnServiceId.Value));

                dtService = CreateServiceTable();
                dtService = (DataTable)ViewState["FirstTimegvService"];
                dtDel = (DataTable)ViewState["FirstTimegvService"];

                //dt.Columns.Add("ServiceId", typeof(int));
                //dt.Columns.Add("ServiceName", typeof(String));
                dvDel = dtDel.Copy().DefaultView;
                if (ServiceId > 0)
                {
                    dvService.RowFilter = "ISNULL(ServiceId,0) <> " + ServiceId;// +" AND IndentId<>" + IndentId;

                    dvDel.RowFilter = "ISNULL(ServiceId,0) <> " + ServiceId;// +" AND IndentId<>" + IndentId;
                }
                dtDel = dvDel.ToTable();
                gvService.DataSource = dtDel;
                gvService.DataBind();
                ViewState["FirstTimegvService"] = dtDel;
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
            dtDel.Dispose();
            dtService.Dispose();
            dvService.Dispose();
            dvDel.Dispose();
        }
    }

    private void BindBlankItemGrid()
    {

        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();

        dr["ItemId"] = 0;
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
        ViewState["DataTableItem"] = dt;
    }

    private void BindBlankServiceGrid()
    {
        DataTable dt = CreateServiceTable();
        DataRow dr = dt.NewRow();

        dr["ServiceiD"] = 0;
        dr["ServiceName"] = "N/A";
        dt.Rows.Add(dr);
        dt.AcceptChanges();
        ViewState["ServiceDetail"] = null;

        ViewState["DataTableService"] = dt;
    }

    protected void ddlPlanTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
        int plantype = common.myInt(ddlPlanTemplates.SelectedValue);
        GetServiceData(plantype);
    }
    protected void gvProblemDetails_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        // int i = 0;
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

                Label lblDose = (Label)e.Row.FindControl("lblDose");
                Label lblDoseUnit = (Label)e.Row.FindControl("lblDoseUnit");

                string dosecombiled = lblDose.Text + "  " + lblDoseUnit.Text;

                lblDoseUnit.Visible = false;

                Label Days = (Label)e.Row.FindControl("lblDays");
                Label DaysType = (Label)e.Row.FindControl("lblDaysType");

                DaysType.Visible = false;

                if (e.Row.RowType != DataControlRowType.Header)
                {
                    if (common.myInt(ItemID.Value) == 0)
                    {
                        chkboxgvTreatmentPlan.Checked = false;
                    }
                    else
                    {
                        chkboxgvTreatmentPlan.Checked = true;
                    }

                    if (lblDose.Text == string.Empty)
                    {
                        lblDose.Text = string.Empty;
                    }
                    else
                    {
                        lblDose.Text = dosecombiled.ToString();
                    }
                    if (Days.Text == string.Empty)
                    {
                        Days.Text = string.Empty;
                    }
                    else
                    {
                        Days.Text = Days.Text + "  " + DaysType.Text;
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


    protected void gvProblemDetails1_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        // int i = 0;
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

                Label lblDose = (Label)e.Row.FindControl("lblDose");
                Label lblDoseUnit = (Label)e.Row.FindControl("lblDoseUnit");

                string dosecombiled = lblDose.Text + "  " + lblDoseUnit.Text;

                lblDoseUnit.Visible = false;

                Label Days = (Label)e.Row.FindControl("lblDays");
                Label DaysType = (Label)e.Row.FindControl("lblDaysType");

                DaysType.Visible = false;

                if (e.Row.RowType != DataControlRowType.Header)
                {
                    if (common.myInt(ItemID.Value) == 0)
                    {
                        chkboxgvTreatmentPlan.Checked = false;
                    }
                    else
                    {
                        chkboxgvTreatmentPlan.Checked = true;
                    }

                    if (lblDose.Text == string.Empty)
                    {
                        lblDose.Text = string.Empty;
                    }
                    else
                    {
                        lblDose.Text = dosecombiled.ToString();
                    }
                    if (Days.Text == string.Empty)
                    {
                        Days.Text = string.Empty;
                    }
                    else
                    {
                        Days.Text = Days.Text + "  " + DaysType.Text;
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
        }
    }


    protected void gvService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
           || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
        //if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkboxgvService = (CheckBox)e.Row.FindControl("chkboxgvService");
            HiddenField ServiceID = (HiddenField)e.Row.FindControl("hdnServiceiD");

            if (e.Row.RowType != DataControlRowType.Header)
            {
                if (common.myInt(ServiceID.Value) == 0)
                {
                    chkboxgvService.Checked = false;
                }
                else
                {
                    chkboxgvService.Checked = true;
                }
            }
        }
    }

    protected void gvDiagnosisDetails_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        // int i = 0;
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            //if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkgvDiagnosisDetails = (CheckBox)e.Row.FindControl("chkgvDiagnosisDetails");
                HiddenField hdnICDID = (HiddenField)e.Row.FindControl("hdnICDID");
                if (common.myInt(hdnICDID.Value) > 0)
                {
                    chkgvDiagnosisDetails.Checked = true;
                }
                else
                {
                    chkgvDiagnosisDetails.Checked = false;
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

    protected void chkboxgvTreatmentPlan_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gr = (GridViewRow)chk.Parent.Parent;
        // lblmsg.Text = gvService.DataKeys[gr.RowIndex].Value.ToString();
    }

    #region parameter save data

    protected void saveData(string XMLData)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.WardManagement objwd = new BaseC.WardManagement();

        DataSet dsXml = new DataSet();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try

        {

            StringBuilder strXML = new StringBuilder();
            StringBuilder strXML1 = new StringBuilder();
            ArrayList coll = new ArrayList();
            ArrayList coll1 = new ArrayList();

            StringBuilder strXMLFre = new StringBuilder();
            ArrayList collFre = new ArrayList();

            //if (common.myInt(Session["DrugReqSavecheck"]) == 0)
            //{
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                lblMessage.Text = "Patient has no appointment !";
                return;
            }
            //if (common.myInt(ddlAdvisingDoctor.SelectedValue) == 0)
            //{
            //    lblMessage.Text = "Advising Doctor not selected!";
            //    return;
            //}

            double iConversionFactor = 0;
            double sQuantity = 0;
            if (Session["OPIP"] != null && (Session["OPIP"].ToString() == "O" || Session["OPIP"].ToString() == "E"))
            {
                #region OP Drug
                foreach (GridViewRow dataItem in gvProblemDetails.Rows)
                {
                    HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                    HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");

                    if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                    {
                        hdnIndentId.Value = "0";
                    }

                    if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
                        && common.myInt(txtTotalQty.Text) == 0)
                    {
                        Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
                        return;
                    }

                    if (hdnIndentId.Value == "0" || hdnIndentId.Value == string.Empty)
                    {
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnStrengthValue = (HiddenField)dataItem.FindControl("hdnStrengthValue");
                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");

                        hdnXMLData.Value = common.myStr(XMLData);

                        HiddenField hdnStartDate = (HiddenField)dataItem.FindControl("hdnStartDate");

                        Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

                        coll1.Add(common.myInt(hdnGenericId.Value));//GenericId int
                        if (common.myInt(hdnItemId.Value) != 0)
                        {
                            coll1.Add(common.myInt(hdnItemId.Value));//ItemId INT,
                            coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                        }
                        else
                        {
                            coll1.Add(DBNull.Value);//ItemId INT,
                            coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
                        }
                        coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
                        coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
                        coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT

                        dsXml = new DataSet();
                        string xmlSchema = string.Empty;
                        dt = new DataTable();
                        if (hdnXMLData.Value == string.Empty)
                        {
                            Alert.ShowAjaxMsg("Please add drug to save", Page);
                            return;
                        }
                        xmlSchema = common.myStr(hdnXMLData.Value);
                        StringReader sr = new StringReader(xmlSchema);

                        dsXml.ReadXml(sr);
                        dv = new DataView(dsXml.Tables[0]);
                        //// dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value) + " And StartDate = '" + hdnStartDate.Value + "'";
                        // dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                        dt = dv.ToTable();
                        if (dt.Rows.Count > 0)
                        {
                            RadDatePicker dtStartTime = new RadDatePicker();
                            RadDatePicker dtEndTime = new RadDatePicker();
                            string sStartDate = string.Empty;
                            string sEndDate = string.Empty;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int iFrequencyId = Convert.ToInt16(dt.Rows[i]["FrequencyId"]);
                                string sDose = dt.Rows[i]["Dose"].ToString();

                                //string sFrequency = dt.Rows[i]["Frequency"].ToString();
                                string sDuration = dt.Rows[i]["Duration"].ToString();
                                string sType = dt.Rows[i]["Type"].ToString();

                                ////if (common.myDbl(sDose) <= 0)
                                ////{
                                ////    Alert.ShowAjaxMsg("Quantity should not be Zero, Please Edit Drug Details !", this.Page);
                                ////    return;
                                ////}
                                if (common.myStr(dt.Rows[i]["XMLFrequencyTime"]) != "")//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    string xmlFrequencySchema = common.myStr(dt.Rows[i]["XMLFrequencyTime"]);
                                    StringReader srFrequency = new StringReader(xmlFrequencySchema);
                                    DataSet dsXmlFrequency = new DataSet();
                                    dsXmlFrequency.ReadXml(srFrequency);
                                    dv = new DataView(dsXmlFrequency.Tables[0]);
                                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                                    if (dv.ToTable().Rows.Count > 0)
                                    {
                                        for (int fr = 0; fr < dv.ToTable().Rows.Count; fr++)
                                        {
                                            string FrequencyId = common.myStr(dv.ToTable().Rows[fr]["FrequencyId"]);
                                            int ItemId = common.myInt(dv.ToTable().Rows[fr]["ItemId"]);
                                            string DoseTime = common.myStr(dv.ToTable().Rows[fr]["DoseTime"]);
                                            int FrequencyDetailId = common.myInt(dv.ToTable().Rows[fr]["FrequencyDetailId"]);
                                            bool DoseEnable = common.myBool(dv.ToTable().Rows[fr]["DoseEnable"]);
                                            collFre.Add(common.myInt(ItemId));//ItemId INT,
                                            collFre.Add(common.myInt(FrequencyId));//FrequencyId INT, 
                                            collFre.Add(common.myStr(DoseTime));//DoseTime String 
                                            collFre.Add(common.myStr(FrequencyDetailId));//FrequencyDetailId INT
                                            collFre.Add(DoseEnable);//FrequencyDetailId INT
                                            strXMLFre.Append(common.setXmlTable(ref collFre));
                                        }
                                    }
                                    dsXmlFrequency.Dispose();
                                }
                                if (common.myStr(dt.Rows[i]["XMLVariableDose"]) != "")//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    StringReader srVariableDose = new StringReader(common.myStr(dt.Rows[i]["XMLVariableDose"]));
                                    DataSet dsVariableDose = new DataSet();
                                    dsVariableDose.ReadXml(srVariableDose);

                                    for (int row = 0; row < common.myInt(dsVariableDose.Tables[0].Rows.Count); row++)
                                    {
                                        for (int Col = 1; Col < common.myInt(dsVariableDose.Tables[0].Columns.Count); Col++)
                                        {
                                            string variableDoseDate = common.myStr(dsVariableDose.Tables[0].Rows[row]["Date"]);
                                            string variableDose = common.myStr(dsVariableDose.Tables[0].Rows[row][Col]);

                                            if (!dt.Columns.Contains("UnitId"))
                                            {
                                                dt.Columns.Add("UnitId", typeof(int));

                                            }
                                            int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                            if (i == 0)
                                            {
                                                if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                                {
                                                    sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    sStartDate = dt.Rows[i]["StartDate"].ToString();
                                                    // sEndDate = dt.Rows[i]["EndDate"].ToString();
                                                }
                                            }
                                            if (i == dt.Rows.Count - 1)
                                            {
                                                if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                                {
                                                    sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToInt16(sDuration))).ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    sEndDate = dt.Rows[i]["EndDate"].ToString();
                                                    //   sStartDate = dt.Rows[i]["StartDate"].ToString();
                                                }
                                            }
                                            string sInstructions = dt.Rows[i]["Instructions"].ToString();
                                            int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                            int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                            int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);

                                            string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                            int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                            string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                            string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                            string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                            string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                            string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                            bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                            coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
                                            coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                            //  coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                            coll.Add(common.myDec(variableDose));
                                            coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                            coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                            coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                            coll.Add(common.myInt(iUnit));//UNITID INT,
                                            coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                            coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                            coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,

                                            coll.Add(Volume);//
                                            coll.Add(VolumeUnitId);//
                                            coll.Add(InfusionTime);//
                                            coll.Add(TimeUnit);//
                                            coll.Add(TotalVolume);//
                                            coll.Add(FlowRate);//
                                            coll.Add(FlowRateUnit);//

                                            coll.Add(common.myDate(variableDoseDate).ToString("yyyy-MM-dd"));//
                                            coll.Add(Col);// variable sequence no         
                                            coll.Add(bitIsSubstituteNotAllow);
                                            strXML.Append(common.setXmlTable(ref coll));
                                        }
                                    }
                                }
                                else
                                {
                                    if (!dt.Columns.Contains("UnitId"))
                                    {
                                        dt.Columns.Add("UnitId", typeof(int));
                                    }
                                    int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                    if (i == 0)
                                    {
                                        if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                        {
                                            sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                        }
                                        else
                                        {
                                            sStartDate = dt.Rows[i]["StartDate"].ToString();
                                            // sEndDate = dt.Rows[i]["EndDate"].ToString();
                                        }
                                    }
                                    if (i == dt.Rows.Count - 1)
                                    {
                                        if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                        {
                                            sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToInt16(sDuration))).ToString("dd/MM/yyyy");
                                        }
                                        else
                                        {
                                            sEndDate = dt.Rows[i]["EndDate"].ToString();
                                            //   sStartDate = dt.Rows[i]["StartDate"].ToString();
                                        }
                                    }
                                    string sInstructions = dt.Rows[i]["Instructions"].ToString();
                                    int iReferanceItemId = Convert.ToInt16(dt.Rows[i]["ReferanceItemId"]);
                                    int iFoodRelationshipID = Convert.ToInt16(dt.Rows[i]["FoodRelationshipID"]);
                                    int iDoseTypeId = Convert.ToInt16(dt.Rows[i]["DoseTypeId"]);

                                    string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                    int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                    string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                    string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                    string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                    string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                    string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                    bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                    //if (common.myInt(iUnit) == 0)
                                    //{
                                    //    Alert.ShowAjaxMsg("Quantity should not be Zero !", this.Page);
                                    //    return;
                                    //}

                                    coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
                                    coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                    coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                    coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                    coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                    coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                    coll.Add(common.myInt(iUnit));//UNITID INT,
                                    coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                    coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                    coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,

                                    coll.Add(Volume);//
                                    coll.Add(VolumeUnitId);//
                                    coll.Add(InfusionTime);//
                                    coll.Add(TimeUnit);//
                                    coll.Add(TotalVolume);//
                                    coll.Add(FlowRate);//
                                    coll.Add(FlowRateUnit);//

                                    coll.Add(string.Empty);//
                                    coll.Add(string.Empty);// variable sequence no     
                                    coll.Add(bitIsSubstituteNotAllow);
                                    strXML.Append(common.setXmlTable(ref coll));
                                }
                            }

                            HiddenField hdnCommentsDrugAllergy = (HiddenField)dataItem.FindControl("hdnCommentsDrugAllergy");
                            HiddenField hdnCommentsDrugToDrug = (HiddenField)dataItem.FindControl("hdnCommentsDrugToDrug");
                            HiddenField hdnCommentsDrugHealth = (HiddenField)dataItem.FindControl("hdnCommentsDrugHealth");

                            coll1.Add(sStartDate != string.Empty ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
                            coll1.Add(sEndDate != string.Empty ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,
                            coll1.Add(common.myDec(txtTotalQty.Text));//TotalQty INT
                            coll1.Add(DBNull.Value);//ICD CODE VARCHAR
                            coll1.Add(common.myInt(0));//Refill INT
                            coll1.Add(common.myBool(0));//Is Override BIT
                            coll1.Add(hdnCommentsDrugAllergy.Value);//OverrideComments VARCHAR
                            coll1.Add(DBNull.Value);//DrugAllergyScreeningResult VARCHAR
                            coll1.Add(common.myInt(424));//PrescriptionModeId INT
                            coll1.Add(hdnNotToPharmcy.Value);//NotToPharmcy BIT
                            coll1.Add(lblPrescriptionDetail.Text);//Prescription detail VARCHAR
                            coll1.Add(hdnCommentsDrugToDrug.Value); //OverrideCommentsDrugToDrug VARCHAR(500),
                            coll1.Add(hdnCommentsDrugHealth.Value); //OverrideCommentsDrugHealth VARCHAR(500)
                            coll1.Add(hdnStrengthValue.Value); //StrengthValue varchar(255)

                            strXML1.Append(common.setXmlTable(ref coll1));
                        }

                    }
                }
                #endregion
            }
            else
            {
                #region IP Drug
                foreach (GridViewRow dataItem in gvProblemDetails.Rows)
                {
                    HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                    HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");
                    HiddenField hdnStartDate = (HiddenField)dataItem.FindControl("hdnStartDate");

                    if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                    {
                        hdnIndentId.Value = "0";
                    }

                    if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
                        && common.myInt(txtTotalQty.Text) == 0)
                    {
                        Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
                        return;
                    }

                    if (hdnIndentId.Value == "0" || hdnIndentId.Value == string.Empty)
                    {
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnStrengthValue = (HiddenField)dataItem.FindControl("hdnStrengthValue");
                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");

                        hdnXMLData.Value = common.myStr(XMLData);

                        Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

                        coll1.Add(common.myInt(hdnIndentId.Value));//FrequencyId TINYINT,
                        if (common.myInt(hdnItemId.Value) != 0)
                        {
                            coll1.Add(common.myInt(hdnItemId.Value));//ItemId INT,
                            coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                        }
                        else
                        {
                            coll1.Add(DBNull.Value);//ItemId INT,
                            coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
                        }
                        coll1.Add(common.myInt(hdnGenericId.Value));//GenericId int
                        coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
                        coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
                        coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT
                        coll1.Add(common.myDec(txtTotalQty.Text));//TotalQty INT

                        dsXml = new DataSet();
                        string xmlSchema = string.Empty;
                        dt = new DataTable();
                        if (hdnXMLData.Value == string.Empty)
                        {
                            Alert.ShowAjaxMsg("Please add drug to save", Page);
                            return;
                        }
                        xmlSchema = common.myStr(hdnXMLData.Value);
                        StringReader sr = new StringReader(xmlSchema);

                        dsXml.ReadXml(sr);
                        dv = new DataView(dsXml.Tables[0]);
                        // dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value) ;
                        ////  dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value) + " And StartDate = '" + hdnStartDate.Value + "'";
                        dt = dv.ToTable();

                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Columns.Count > 0)
                                {
                                    if (!dt.Columns.Contains("FrequencyId"))
                                    {
                                        dt.Columns.Add("FrequencyId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("Dose"))
                                    {
                                        dt.Columns.Add("Dose", typeof(double));
                                    }
                                    if (!dt.Columns.Contains("Duration"))
                                    {
                                        dt.Columns.Add("Duration", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Type"))
                                    {
                                        dt.Columns.Add("Type", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Instructions"))
                                    {
                                        dt.Columns.Add("Instructions", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("ReferanceItemId"))
                                    {
                                        dt.Columns.Add("ReferanceItemId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FoodRelationshipID"))
                                    {
                                        dt.Columns.Add("FoodRelationshipID", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("DoseTypeId"))
                                    {
                                        dt.Columns.Add("DoseTypeId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("UnitId"))
                                    {
                                        dt.Columns.Add("UnitId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FormulationId"))
                                    {
                                        dt.Columns.Add("FormulationId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FrequencyId"))
                                    {
                                        dt.Columns.Add("FrequencyId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("RouteId"))
                                    {
                                        dt.Columns.Add("RouteId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("InfusionTime"))
                                    {
                                        dt.Columns.Add("InfusionTime", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("TotalVolume"))
                                    {
                                        dt.Columns.Add("TotalVolume", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Volume"))
                                    {
                                        dt.Columns.Add("Volume", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("VolumeUnitId"))
                                    {
                                        dt.Columns.Add("VolumeUnitId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("TimeUnit"))
                                    {
                                        dt.Columns.Add("TimeUnit", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("FlowRate"))
                                    {
                                        dt.Columns.Add("FlowRate", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("FlowRateUnit"))
                                    {
                                        dt.Columns.Add("FlowRateUnit", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("XmlVariableDose"))
                                    {
                                        dt.Columns.Add("XmlVariableDose", typeof(string));
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (dt.Rows.Count > 0)
                        {
                            RadDatePicker dtStartTime = new RadDatePicker();
                            RadDatePicker dtEndTime = new RadDatePicker();
                            string sStartDate = string.Empty;
                            string sEndDate = string.Empty;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int iFrequencyId = Convert.ToInt16(dt.Rows[i]["FrequencyId"]);
                                string sDose = dt.Rows[i]["Dose"].ToString();
                                string sDuration = dt.Rows[i]["Duration"].ToString();
                                string sType = dt.Rows[i]["Type"].ToString();

                                //if (common.myDbl(sDose) <= 0)
                                //{
                                //    Alert.ShowAjaxMsg("Quantity should not be Zero, Please Edit Drug Details !", this.Page);
                                //    return;
                                //}
                                if (common.myStr(dt.Rows[i]["XMLFrequencyTime"]) != "")//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    string xmlFrequencySchema = common.myStr(dt.Rows[i]["XMLFrequencyTime"]);
                                    StringReader srFrequency = new StringReader(xmlFrequencySchema);
                                    DataSet dsXmlFrequency = new DataSet();
                                    dsXmlFrequency.ReadXml(srFrequency);
                                    dv = new DataView(dsXmlFrequency.Tables[0]);
                                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                                    if (dv.ToTable().Rows.Count > 0)
                                    {
                                        for (int fr = 0; fr < dv.ToTable().Rows.Count; fr++)
                                        {
                                            string FrequencyId = common.myStr(dv.ToTable().Rows[fr]["FrequencyId"]);
                                            int ItemId = common.myInt(dv.ToTable().Rows[fr]["ItemId"]);
                                            string DoseTime = common.myStr(dv.ToTable().Rows[fr]["DoseTime"]);
                                            int FrequencyDetailId = common.myInt(dv.ToTable().Rows[fr]["FrequencyDetailId"]);
                                            bool DoseEnable = common.myBool(dv.ToTable().Rows[fr]["DoseEnable"]);
                                            collFre.Add(common.myInt(ItemId));//ItemId INT,
                                            collFre.Add(common.myInt(FrequencyId));//FrequencyId INT, 
                                            collFre.Add(common.myStr(DoseTime));//DoseTime String 
                                            collFre.Add(common.myStr(FrequencyDetailId));//FrequencyDetailId INT
                                            collFre.Add(DoseEnable);//FrequencyDetailId INT
                                            strXMLFre.Append(common.setXmlTable(ref collFre));
                                        }
                                    }
                                }
                                if (common.myStr(dt.Rows[i]["XMLVariableDose"]) != "")//&& (common.myStr( dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    StringReader srVariableDose = new StringReader(common.myStr(dt.Rows[i]["XMLVariableDose"]));
                                    DataSet dsVariableDose = new DataSet();
                                    dsVariableDose.ReadXml(srVariableDose);

                                    for (int row = 0; row < common.myInt(dsVariableDose.Tables[0].Rows.Count); row++)
                                    {
                                        for (int Col = 1; Col < common.myInt(dsVariableDose.Tables[0].Columns.Count); Col++)
                                        {
                                            string variableDoseDate = common.myStr(dsVariableDose.Tables[0].Rows[row]["Date"]);
                                            string variableDose = common.myStr(dsVariableDose.Tables[0].Rows[row][Col]);

                                            int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                            if (i == 0)
                                            {
                                                if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                                {
                                                    sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    sStartDate = dt.Rows[i]["StartDate"].ToString();
                                                }
                                            }
                                            if (i == dt.Rows.Count - 1)
                                            {
                                                if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                                {
                                                    if (!common.myLen(sDuration).Equals(0))
                                                    {
                                                        sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToInt16(sDuration))).ToString("dd/MM/yyyy");
                                                    }
                                                    else
                                                    {
                                                        sEndDate = sStartDate;
                                                    }
                                                }
                                                else
                                                {
                                                    sEndDate = dt.Rows[i]["EndDate"].ToString();
                                                }
                                            }
                                            string sInstructions = dt.Rows[i]["Instructions"].ToString();
                                            int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                            int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                            int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);

                                            string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                            int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                            string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                            string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                            string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                            string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                            string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                            bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                            coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
                                            coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                            // coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                            coll.Add(common.myDec(variableDose));
                                            coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                            coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                            coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                            coll.Add(common.myInt(iUnit));//UNITID INT,
                                            coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                            coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                            coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,

                                            coll.Add(Volume);//
                                            coll.Add(VolumeUnitId);//
                                            coll.Add(InfusionTime);//
                                            coll.Add(TimeUnit);//
                                            coll.Add(TotalVolume);//
                                            coll.Add(FlowRate);//
                                            coll.Add(FlowRateUnit);//

                                            coll.Add(common.myDate(variableDoseDate).ToString("yyyy-MM-dd"));//
                                            coll.Add(bitIsSubstituteNotAllow);
                                            coll.Add(Col);// variable sequence no                                             

                                            strXML.Append(common.setXmlTable(ref coll));
                                        }
                                    }


                                }
                                else// without variable dose
                                {

                                    int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                    if (i == 0)
                                    {
                                        if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                        {
                                            sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                        }
                                        else
                                        {
                                            sStartDate = dt.Rows[i]["StartDate"].ToString();
                                        }
                                    }
                                    if (i == dt.Rows.Count - 1)
                                    {
                                        if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                        {
                                            if (!common.myLen(sDuration).Equals(0))
                                            {
                                                sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToInt16(sDuration))).ToString("dd/MM/yyyy");
                                            }
                                            else
                                            {
                                                sEndDate = sStartDate;
                                            }
                                        }
                                        else
                                        {
                                            sEndDate = dt.Rows[i]["EndDate"].ToString();
                                        }
                                    }
                                    string sInstructions = dt.Rows[i]["Instructions"].ToString();
                                    int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                    int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                    int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);

                                    string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                    int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                    string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                    string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                    string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                    string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                    string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                    bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                    coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
                                    coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                    coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                    coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                    coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                    coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                    coll.Add(common.myInt(iUnit));//UNITID INT,
                                    coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                    coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                    coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,

                                    coll.Add(Volume);//
                                    coll.Add(VolumeUnitId);//
                                    coll.Add(InfusionTime);//
                                    coll.Add(TimeUnit);//
                                    coll.Add(TotalVolume);//
                                    coll.Add(FlowRate);//
                                    coll.Add(FlowRateUnit);//
                                    coll.Add(string.Empty);//                                    
                                    coll.Add(bitIsSubstituteNotAllow);
                                    //coll.Add(string.Empty);// variable sequence no 

                                    strXML.Append(common.setXmlTable(ref coll));
                                }
                            }

                            coll1.Add(sStartDate != string.Empty ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
                            coll1.Add(sEndDate != string.Empty ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,
                            coll1.Add(hdnNotToPharmcy.Value);//NotToPharmcy BIT

                            HiddenField hdnCommentsDrugAllergy = (HiddenField)dataItem.FindControl("hdnCommentsDrugAllergy");
                            HiddenField hdnCommentsDrugToDrug = (HiddenField)dataItem.FindControl("hdnCommentsDrugToDrug");
                            HiddenField hdnCommentsDrugHealth = (HiddenField)dataItem.FindControl("hdnCommentsDrugHealth");

                            coll1.Add(hdnCommentsDrugAllergy.Value); //OverrideComments VARCHAR(500),
                            coll1.Add(hdnCommentsDrugToDrug.Value); //OverrideCommentsDrugToDrug VARCHAR(500),
                            coll1.Add(hdnCommentsDrugHealth.Value); //OverrideCommentsDrugHealth VARCHAR(500)
                            coll1.Add(lblPrescriptionDetail.Text);//Prescription detail VARCHAR(1000)
                            coll1.Add(hdnStrengthValue.Value); //StrengthValue varchar(255)

                            strXML1.Append(common.setXmlTable(ref coll1));

                        }
                        else
                        {
                            return;
                        }
                        if (common.myBool(ViewState["ConversioinFactor"]))
                        {
                            ds = objwd.GetItemConversionFactor(common.myInt(hdnItemId.Value));

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                iConversionFactor = common.myDbl(ds.Tables[0].Rows[0]["ConversionFactor2"]);
                            }
                            if (iConversionFactor != 0)
                            {
                                for (int i = 1; i <= iConversionFactor * common.myDbl(txtTotalQty.Text); i++)
                                {
                                    if (common.myDbl(txtTotalQty.Text) <= iConversionFactor * i)
                                    {
                                        sQuantity = iConversionFactor * i;
                                        break;
                                    }
                                }
                            }
                        }
                        if (common.myBool(ViewState["ConversioinFactor"]) && iConversionFactor != 0)
                        {
                            sQuantity = Convert.ToDouble(sQuantity.ToString("F2"));
                        }
                        else
                        {
                            sQuantity = Convert.ToDouble(txtTotalQty.Text);
                        }
                    }
                }
                #endregion
            }

            if (strXML.ToString() == string.Empty)
            {
                lblMessage.Text = "Please add medicine before saving !";
                return;
            }
            bool isConsumable = common.myStr(Request.QueryString["DRUGORDERCODE"]) == "CO" ? true : false;
            string sXMLFre = strXMLFre != null ? strXMLFre.ToString() : "";
            Hashtable hshOutput = new Hashtable();
            if (common.myStr(Session["DrugOrderDuplicateCheck"]) == "0")
            {
                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    hshOutput = objwd.SaveEMRMedicine(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                        common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(0), common.myInt(0),
                        common.myInt(0), strXML1.ToString(), strXML.ToString(), string.Empty, Request.QueryString["DRUGORDERTYPE"] != null ? common.myInt(Request.QueryString["DRUGORDERTYPE"]) : 0,
                        common.myInt(Session["UserId"]), isConsumable, sXMLFre, string.Empty, 0, false, false);
                }
                else
                {
                    hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                        common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]),
                        common.myInt(0), common.myInt(0), common.myInt(0),
                        0, 0, strXML1.ToString(), strXML.ToString(), string.Empty, common.myInt(Session["UserId"]), sXMLFre, isConsumable);
                }
                if ((hshOutput["@chvErrorStatus"].ToString().ToUpper().Contains("UPDATE") || hshOutput["@chvErrorStatus"].ToString().ToUpper().Contains("SAVED"))
                    && !hshOutput["@chvErrorStatus"].ToString().ToUpper().Contains("USP"))
                {

                    Session["DrugOrderDuplicateCheck"] = "1";
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                    ////hdnGenericId.Value = "0";
                    ////hdnGenericName.Value = string.Empty;
                    hdnItemId.Value = "0";
                    hdnItemName.Value = string.Empty;
                    ////ddlIndentType.SelectedValue = "0";

                    ////hdnCIMSItemId.Value = string.Empty;
                    ////hdnCIMSType.Value = string.Empty;
                    ////hdnVIDALItemId.Value = "0";

                    ////clearItemDetails();

                    ////dvPharmacistInstruction.Visible = false;
                    ////lnkPharmacistInstruction.Visible = false;

                    if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                        return;
                    }
                }
                lblMessage.Text = hshOutput["@chvErrorStatus"].ToString();
                //////    BindGrid(string.Empty, string.Empty);
                BindBlankItemGrid();
                ViewState["Item"] = null;
                ViewState["Stop"] = null;
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
            objEMR = null;
            objwd = null;

            dsXml.Dispose();
            ds.Dispose();
            dt.Dispose();
            dv.Dispose();
            Session["DrugOrderDuplicateCheck"] = "0";
        }
    }
    #endregion

    protected void saveData()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.WardManagement objwd = new BaseC.WardManagement();

        DataSet dsXml = new DataSet();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            StringBuilder strXML = new StringBuilder();
            StringBuilder strXML1 = new StringBuilder();
            ArrayList coll = new ArrayList();
            ArrayList coll1 = new ArrayList();

            StringBuilder strXMLFre = new StringBuilder();
            ArrayList collFre = new ArrayList();

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                lblMessage.Text = "Patient has no appointment !";
                return;
            }

            double iConversionFactor = 0;
            double sQuantity = 0;
            if (Session["OPIP"] != null && (Session["OPIP"].ToString() == "O" || Session["OPIP"].ToString() == "E"))
            {
                #region OP Drug
                foreach (GridViewRow dataItem in gvProblemDetails.Rows)
                {
                    HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                    HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");

                    if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                    {
                        hdnIndentId.Value = "0";
                    }

                    if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
                        && common.myInt(txtTotalQty.Text) == 0)
                    {
                        Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
                        return;
                    }

                    if (hdnIndentId.Value == "0" || hdnIndentId.Value == string.Empty)
                    {
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnStrengthValue = (HiddenField)dataItem.FindControl("hdnStrengthValue");
                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
                        HiddenField hdnStartDate = (HiddenField)dataItem.FindControl("hdnStartDate");

                        Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

                        coll1.Add(common.myInt(hdnGenericId.Value));//GenericId int
                        if (common.myInt(hdnItemId.Value) != 0)
                        {
                            coll1.Add(common.myInt(hdnItemId.Value));//ItemId INT,
                            coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                        }
                        else
                        {
                            coll1.Add(DBNull.Value);//ItemId INT,
                            coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
                        }
                        coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
                        coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
                        coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT

                        dsXml = new DataSet();
                        string xmlSchema = string.Empty;
                        dt = new DataTable();
                        if (hdnXMLData.Value == string.Empty)
                        {
                            Alert.ShowAjaxMsg("Please add drug to save", Page);
                            return;
                        }
                        xmlSchema = common.myStr(hdnXMLData.Value);
                        StringReader sr = new StringReader(xmlSchema);

                        dsXml.ReadXml(sr);
                        dv = new DataView(dsXml.Tables[0]);
                        dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value) + " And StartDate = '" + hdnStartDate.Value + "'";
                        // dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                        dt = dv.ToTable();
                        if (dt.Rows.Count > 0)
                        {
                            RadDatePicker dtStartTime = new RadDatePicker();
                            RadDatePicker dtEndTime = new RadDatePicker();
                            string sStartDate = string.Empty;
                            string sEndDate = string.Empty;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int iFrequencyId = Convert.ToInt16(dt.Rows[i]["FrequencyId"]);
                                string sDose = dt.Rows[i]["Dose"].ToString();

                                //string sFrequency = dt.Rows[i]["Frequency"].ToString();
                                string sDuration = dt.Rows[i]["Duration"].ToString();
                                string sType = dt.Rows[i]["Type"].ToString();

                                if (common.myDbl(sDose) <= 0)
                                {
                                    Alert.ShowAjaxMsg("Quantity should not be Zero, Please Edit Drug Details !", this.Page);
                                    return;
                                }
                                if (common.myStr(dt.Rows[i]["XMLFrequencyTime"]) != "")//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    string xmlFrequencySchema = common.myStr(dt.Rows[i]["XMLFrequencyTime"]);
                                    StringReader srFrequency = new StringReader(xmlFrequencySchema);
                                    DataSet dsXmlFrequency = new DataSet();
                                    dsXmlFrequency.ReadXml(srFrequency);
                                    dv = new DataView(dsXmlFrequency.Tables[0]);
                                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                                    if (dv.ToTable().Rows.Count > 0)
                                    {
                                        for (int fr = 0; fr < dv.ToTable().Rows.Count; fr++)
                                        {
                                            string FrequencyId = common.myStr(dv.ToTable().Rows[fr]["FrequencyId"]);
                                            int ItemId = common.myInt(dv.ToTable().Rows[fr]["ItemId"]);
                                            string DoseTime = common.myStr(dv.ToTable().Rows[fr]["DoseTime"]);
                                            int FrequencyDetailId = common.myInt(dv.ToTable().Rows[fr]["FrequencyDetailId"]);
                                            bool DoseEnable = common.myBool(dv.ToTable().Rows[fr]["DoseEnable"]);
                                            collFre.Add(common.myInt(ItemId));//ItemId INT,
                                            collFre.Add(common.myInt(FrequencyId));//FrequencyId INT, 
                                            collFre.Add(common.myStr(DoseTime));//DoseTime String 
                                            collFre.Add(common.myStr(FrequencyDetailId));//FrequencyDetailId INT
                                            collFre.Add(DoseEnable);//FrequencyDetailId INT
                                            strXMLFre.Append(common.setXmlTable(ref collFre));
                                        }
                                    }
                                    dsXmlFrequency.Dispose();
                                }
                                if (common.myStr(dt.Rows[i]["XMLVariableDose"]) != "")//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    StringReader srVariableDose = new StringReader(common.myStr(dt.Rows[i]["XMLVariableDose"]));
                                    DataSet dsVariableDose = new DataSet();
                                    dsVariableDose.ReadXml(srVariableDose);

                                    for (int row = 0; row < common.myInt(dsVariableDose.Tables[0].Rows.Count); row++)
                                    {
                                        for (int Col = 1; Col < common.myInt(dsVariableDose.Tables[0].Columns.Count); Col++)
                                        {
                                            string variableDoseDate = common.myStr(dsVariableDose.Tables[0].Rows[row]["Date"]);
                                            string variableDose = common.myStr(dsVariableDose.Tables[0].Rows[row][Col]);

                                            if (!dt.Columns.Contains("UnitId"))
                                            {
                                                dt.Columns.Add("UnitId", typeof(int));

                                            }
                                            int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                            if (i == 0)
                                            {
                                                if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                                {
                                                    sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    sStartDate = dt.Rows[i]["StartDate"].ToString();
                                                    // sEndDate = dt.Rows[i]["EndDate"].ToString();
                                                }
                                            }
                                            if (i == dt.Rows.Count - 1)
                                            {
                                                if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                                {
                                                    sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToInt16(sDuration))).ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    sEndDate = dt.Rows[i]["EndDate"].ToString();
                                                    //   sStartDate = dt.Rows[i]["StartDate"].ToString();
                                                }
                                            }
                                            string sInstructions = dt.Rows[i]["Instructions"].ToString();
                                            int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                            int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                            int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);

                                            string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                            int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                            string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                            string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                            string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                            string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                            string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                            bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                            coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
                                            coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                            //  coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                            coll.Add(common.myDec(variableDose));
                                            coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                            coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                            coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                            coll.Add(common.myInt(iUnit));//UNITID INT,
                                            coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                            coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                            coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,

                                            coll.Add(Volume);//
                                            coll.Add(VolumeUnitId);//
                                            coll.Add(InfusionTime);//
                                            coll.Add(TimeUnit);//
                                            coll.Add(TotalVolume);//
                                            coll.Add(FlowRate);//
                                            coll.Add(FlowRateUnit);//

                                            coll.Add(common.myDate(variableDoseDate).ToString("yyyy-MM-dd"));//
                                            coll.Add(Col);// variable sequence no         
                                            coll.Add(bitIsSubstituteNotAllow);
                                            strXML.Append(common.setXmlTable(ref coll));
                                        }
                                    }
                                }
                                else
                                {
                                    if (!dt.Columns.Contains("UnitId"))
                                    {
                                        dt.Columns.Add("UnitId", typeof(int));
                                    }
                                    int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                    if (i == 0)
                                    {
                                        if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                        {
                                            sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                        }
                                        else
                                        {
                                            sStartDate = dt.Rows[i]["StartDate"].ToString();
                                            // sEndDate = dt.Rows[i]["EndDate"].ToString();
                                        }
                                    }
                                    if (i == dt.Rows.Count - 1)
                                    {
                                        if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                        {
                                            sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToInt16(sDuration))).ToString("dd/MM/yyyy");
                                        }
                                        else
                                        {
                                            sEndDate = dt.Rows[i]["EndDate"].ToString();
                                            //   sStartDate = dt.Rows[i]["StartDate"].ToString();
                                        }
                                    }
                                    string sInstructions = dt.Rows[i]["Instructions"].ToString();
                                    int iReferanceItemId = Convert.ToInt16(dt.Rows[i]["ReferanceItemId"]);
                                    int iFoodRelationshipID = Convert.ToInt16(dt.Rows[i]["FoodRelationshipID"]);
                                    int iDoseTypeId = Convert.ToInt16(dt.Rows[i]["DoseTypeId"]);

                                    string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                    int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                    string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                    string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                    string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                    string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                    string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                    bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                    //if (common.myInt(iUnit) == 0)
                                    //{
                                    //    Alert.ShowAjaxMsg("Quantity should not be Zero !", this.Page);
                                    //    return;
                                    //}

                                    coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
                                    coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                    coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                    coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                    coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                    coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                    coll.Add(common.myInt(iUnit));//UNITID INT,
                                    coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                    coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                    coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,

                                    coll.Add(Volume);//
                                    coll.Add(VolumeUnitId);//
                                    coll.Add(InfusionTime);//
                                    coll.Add(TimeUnit);//
                                    coll.Add(TotalVolume);//
                                    coll.Add(FlowRate);//
                                    coll.Add(FlowRateUnit);//

                                    coll.Add(string.Empty);//
                                    coll.Add(string.Empty);// variable sequence no     
                                    coll.Add(bitIsSubstituteNotAllow);
                                    strXML.Append(common.setXmlTable(ref coll));
                                }
                            }

                            HiddenField hdnCommentsDrugAllergy = (HiddenField)dataItem.FindControl("hdnCommentsDrugAllergy");
                            HiddenField hdnCommentsDrugToDrug = (HiddenField)dataItem.FindControl("hdnCommentsDrugToDrug");
                            HiddenField hdnCommentsDrugHealth = (HiddenField)dataItem.FindControl("hdnCommentsDrugHealth");

                            coll1.Add(sStartDate != string.Empty ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
                            coll1.Add(sEndDate != string.Empty ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,
                            coll1.Add(common.myDec(txtTotalQty.Text));//TotalQty INT
                            coll1.Add(DBNull.Value);//ICD CODE VARCHAR
                            coll1.Add(common.myInt(0));//Refill INT
                            coll1.Add(common.myBool(0));//Is Override BIT
                            coll1.Add(hdnCommentsDrugAllergy.Value);//OverrideComments VARCHAR
                            coll1.Add(DBNull.Value);//DrugAllergyScreeningResult VARCHAR
                            coll1.Add(common.myInt(424));//PrescriptionModeId INT
                            coll1.Add(hdnNotToPharmcy.Value);//NotToPharmcy BIT
                            coll1.Add(lblPrescriptionDetail.Text);//Prescription detail VARCHAR
                            coll1.Add(hdnCommentsDrugToDrug.Value); //OverrideCommentsDrugToDrug VARCHAR(500),
                            coll1.Add(hdnCommentsDrugHealth.Value); //OverrideCommentsDrugHealth VARCHAR(500)
                            coll1.Add(hdnStrengthValue.Value); //StrengthValue varchar(255)

                            strXML1.Append(common.setXmlTable(ref coll1));
                        }

                    }
                }
                #endregion
            }
            else
            {
                #region IP Drug
                foreach (GridViewRow dataItem in gvProblemDetails.Rows)
                {
                    HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                    HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");
                    HiddenField hdnStartDate = (HiddenField)dataItem.FindControl("hdnStartDate");

                    if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                    {
                        hdnIndentId.Value = "0";
                    }

                    if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
                        && common.myInt(txtTotalQty.Text) == 0)
                    {
                        Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
                        return;
                    }

                    if (hdnIndentId.Value == "0" || hdnIndentId.Value == string.Empty)
                    {
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnStrengthValue = (HiddenField)dataItem.FindControl("hdnStrengthValue");
                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
                        Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

                        coll1.Add(common.myInt(hdnIndentId.Value));//FrequencyId TINYINT,
                        if (common.myInt(hdnItemId.Value) != 0)
                        {
                            coll1.Add(common.myInt(hdnItemId.Value));//ItemId INT,
                            coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                        }
                        else
                        {
                            coll1.Add(DBNull.Value);//ItemId INT,
                            coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
                        }
                        coll1.Add(common.myInt(hdnGenericId.Value));//GenericId int
                        coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
                        coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
                        coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT
                        coll1.Add(common.myDec(txtTotalQty.Text));//TotalQty INT

                        dsXml = new DataSet();
                        string xmlSchema = string.Empty;
                        dt = new DataTable();
                        if (hdnXMLData.Value == string.Empty)
                        {
                            Alert.ShowAjaxMsg("Please add drug to save", Page);
                            return;
                        }
                        xmlSchema = common.myStr(hdnXMLData.Value);
                        StringReader sr = new StringReader(xmlSchema);

                        dsXml.ReadXml(sr);
                        dv = new DataView(dsXml.Tables[0]);
                        // dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value) ;
                        dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value) + " And StartDate = '" + hdnStartDate.Value + "'";
                        dt = dv.ToTable();

                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Columns.Count > 0)
                                {
                                    if (!dt.Columns.Contains("FrequencyId"))
                                    {
                                        dt.Columns.Add("FrequencyId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("Dose"))
                                    {
                                        dt.Columns.Add("Dose", typeof(double));
                                    }
                                    if (!dt.Columns.Contains("Duration"))
                                    {
                                        dt.Columns.Add("Duration", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Type"))
                                    {
                                        dt.Columns.Add("Type", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Instructions"))
                                    {
                                        dt.Columns.Add("Instructions", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("ReferanceItemId"))
                                    {
                                        dt.Columns.Add("ReferanceItemId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FoodRelationshipID"))
                                    {
                                        dt.Columns.Add("FoodRelationshipID", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("DoseTypeId"))
                                    {
                                        dt.Columns.Add("DoseTypeId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("UnitId"))
                                    {
                                        dt.Columns.Add("UnitId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FormulationId"))
                                    {
                                        dt.Columns.Add("FormulationId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FrequencyId"))
                                    {
                                        dt.Columns.Add("FrequencyId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("RouteId"))
                                    {
                                        dt.Columns.Add("RouteId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("InfusionTime"))
                                    {
                                        dt.Columns.Add("InfusionTime", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("TotalVolume"))
                                    {
                                        dt.Columns.Add("TotalVolume", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Volume"))
                                    {
                                        dt.Columns.Add("Volume", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("VolumeUnitId"))
                                    {
                                        dt.Columns.Add("VolumeUnitId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("TimeUnit"))
                                    {
                                        dt.Columns.Add("TimeUnit", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("FlowRate"))
                                    {
                                        dt.Columns.Add("FlowRate", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("FlowRateUnit"))
                                    {
                                        dt.Columns.Add("FlowRateUnit", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("XmlVariableDose"))
                                    {
                                        dt.Columns.Add("XmlVariableDose", typeof(string));
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (dt.Rows.Count > 0)
                        {
                            RadDatePicker dtStartTime = new RadDatePicker();
                            RadDatePicker dtEndTime = new RadDatePicker();
                            string sStartDate = string.Empty;
                            string sEndDate = string.Empty;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int iFrequencyId = Convert.ToInt16(dt.Rows[i]["FrequencyId"]);
                                string sDose = dt.Rows[i]["Dose"].ToString();
                                string sDuration = dt.Rows[i]["Duration"].ToString();
                                string sType = dt.Rows[i]["Type"].ToString();

                                if (common.myDbl(sDose) <= 0)
                                {
                                    Alert.ShowAjaxMsg("Quantity should not be Zero, Please Edit Drug Details !", this.Page);
                                    return;
                                }
                                if (common.myStr(dt.Rows[i]["XMLFrequencyTime"]) != "")//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    string xmlFrequencySchema = common.myStr(dt.Rows[i]["XMLFrequencyTime"]);
                                    StringReader srFrequency = new StringReader(xmlFrequencySchema);
                                    DataSet dsXmlFrequency = new DataSet();
                                    dsXmlFrequency.ReadXml(srFrequency);
                                    dv = new DataView(dsXmlFrequency.Tables[0]);
                                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                                    if (dv.ToTable().Rows.Count > 0)
                                    {
                                        for (int fr = 0; fr < dv.ToTable().Rows.Count; fr++)
                                        {
                                            string FrequencyId = common.myStr(dv.ToTable().Rows[fr]["FrequencyId"]);
                                            int ItemId = common.myInt(dv.ToTable().Rows[fr]["ItemId"]);
                                            string DoseTime = common.myStr(dv.ToTable().Rows[fr]["DoseTime"]);
                                            int FrequencyDetailId = common.myInt(dv.ToTable().Rows[fr]["FrequencyDetailId"]);
                                            bool DoseEnable = common.myBool(dv.ToTable().Rows[fr]["DoseEnable"]);
                                            collFre.Add(common.myInt(ItemId));//ItemId INT,
                                            collFre.Add(common.myInt(FrequencyId));//FrequencyId INT, 
                                            collFre.Add(common.myStr(DoseTime));//DoseTime String 
                                            collFre.Add(common.myStr(FrequencyDetailId));//FrequencyDetailId INT
                                            collFre.Add(DoseEnable);//FrequencyDetailId INT
                                            strXMLFre.Append(common.setXmlTable(ref collFre));
                                        }
                                    }
                                }
                                if (common.myStr(dt.Rows[i]["XMLVariableDose"]) != "")//&& (common.myStr( dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    StringReader srVariableDose = new StringReader(common.myStr(dt.Rows[i]["XMLVariableDose"]));
                                    DataSet dsVariableDose = new DataSet();
                                    dsVariableDose.ReadXml(srVariableDose);

                                    for (int row = 0; row < common.myInt(dsVariableDose.Tables[0].Rows.Count); row++)
                                    {
                                        for (int Col = 1; Col < common.myInt(dsVariableDose.Tables[0].Columns.Count); Col++)
                                        {
                                            string variableDoseDate = common.myStr(dsVariableDose.Tables[0].Rows[row]["Date"]);
                                            string variableDose = common.myStr(dsVariableDose.Tables[0].Rows[row][Col]);

                                            int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                            if (i == 0)
                                            {
                                                if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                                {
                                                    sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    sStartDate = dt.Rows[i]["StartDate"].ToString();
                                                }
                                            }
                                            if (i == dt.Rows.Count - 1)
                                            {
                                                if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                                {
                                                    if (!common.myLen(sDuration).Equals(0))
                                                    {
                                                        sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToInt16(sDuration))).ToString("dd/MM/yyyy");
                                                    }
                                                    else
                                                    {
                                                        sEndDate = sStartDate;
                                                    }
                                                }
                                                else
                                                {
                                                    sEndDate = dt.Rows[i]["EndDate"].ToString();
                                                }
                                            }
                                            string sInstructions = dt.Rows[i]["Instructions"].ToString();
                                            int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                            int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                            int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);

                                            string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                            int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                            string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                            string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                            string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                            string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                            string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                            bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                            coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
                                            coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                            // coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                            coll.Add(common.myDec(variableDose));
                                            coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                            coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                            coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                            coll.Add(common.myInt(iUnit));//UNITID INT,
                                            coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                            coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                            coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,

                                            coll.Add(Volume);//
                                            coll.Add(VolumeUnitId);//
                                            coll.Add(InfusionTime);//
                                            coll.Add(TimeUnit);//
                                            coll.Add(TotalVolume);//
                                            coll.Add(FlowRate);//
                                            coll.Add(FlowRateUnit);//

                                            coll.Add(common.myDate(variableDoseDate).ToString("yyyy-MM-dd"));//
                                            coll.Add(bitIsSubstituteNotAllow);
                                            coll.Add(Col);// variable sequence no                                             

                                            strXML.Append(common.setXmlTable(ref coll));
                                        }
                                    }


                                }
                                else// without variable dose
                                {

                                    int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                    if (i == 0)
                                    {
                                        if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                        {
                                            sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                        }
                                        else
                                        {
                                            sStartDate = dt.Rows[i]["StartDate"].ToString();
                                        }
                                    }
                                    if (i == dt.Rows.Count - 1)
                                    {
                                        if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
                                        {
                                            if (!common.myLen(sDuration).Equals(0))
                                            {
                                                sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToInt16(sDuration))).ToString("dd/MM/yyyy");
                                            }
                                            else
                                            {
                                                sEndDate = sStartDate;
                                            }
                                        }
                                        else
                                        {
                                            sEndDate = dt.Rows[i]["EndDate"].ToString();
                                        }
                                    }
                                    string sInstructions = dt.Rows[i]["Instructions"].ToString();
                                    int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                    int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                    int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);

                                    string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                    int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                    string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                    string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                    string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                    string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                    string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                    bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                    coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
                                    coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                    coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                    coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                    coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                    coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                    coll.Add(common.myInt(iUnit));//UNITID INT,
                                    coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                    coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                    coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,

                                    coll.Add(Volume);//
                                    coll.Add(VolumeUnitId);//
                                    coll.Add(InfusionTime);//
                                    coll.Add(TimeUnit);//
                                    coll.Add(TotalVolume);//
                                    coll.Add(FlowRate);//
                                    coll.Add(FlowRateUnit);//
                                    coll.Add(string.Empty);//                                    
                                    coll.Add(bitIsSubstituteNotAllow);
                                    //coll.Add(string.Empty);// variable sequence no 

                                    strXML.Append(common.setXmlTable(ref coll));
                                }
                            }

                            coll1.Add(sStartDate != string.Empty ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
                            coll1.Add(sEndDate != string.Empty ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,
                            coll1.Add(hdnNotToPharmcy.Value);//NotToPharmcy BIT

                            HiddenField hdnCommentsDrugAllergy = (HiddenField)dataItem.FindControl("hdnCommentsDrugAllergy");
                            HiddenField hdnCommentsDrugToDrug = (HiddenField)dataItem.FindControl("hdnCommentsDrugToDrug");
                            HiddenField hdnCommentsDrugHealth = (HiddenField)dataItem.FindControl("hdnCommentsDrugHealth");

                            coll1.Add(hdnCommentsDrugAllergy.Value); //OverrideComments VARCHAR(500),
                            coll1.Add(hdnCommentsDrugToDrug.Value); //OverrideCommentsDrugToDrug VARCHAR(500),
                            coll1.Add(hdnCommentsDrugHealth.Value); //OverrideCommentsDrugHealth VARCHAR(500)
                            coll1.Add(lblPrescriptionDetail.Text);//Prescription detail VARCHAR(1000)
                            coll1.Add(hdnStrengthValue.Value); //StrengthValue varchar(255)

                            strXML1.Append(common.setXmlTable(ref coll1));

                        }
                        else
                        {
                            return;
                        }
                        if (common.myBool(ViewState["ConversioinFactor"]))
                        {
                            ds = objwd.GetItemConversionFactor(common.myInt(hdnItemId.Value));

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                iConversionFactor = common.myDbl(ds.Tables[0].Rows[0]["ConversionFactor2"]);
                            }
                            if (iConversionFactor != 0)
                            {
                                for (int i = 1; i <= iConversionFactor * common.myDbl(txtTotalQty.Text); i++)
                                {
                                    if (common.myDbl(txtTotalQty.Text) <= iConversionFactor * i)
                                    {
                                        sQuantity = iConversionFactor * i;
                                        break;
                                    }
                                }
                            }
                        }
                        if (common.myBool(ViewState["ConversioinFactor"]) && iConversionFactor != 0)
                        {
                            sQuantity = Convert.ToDouble(sQuantity.ToString("F2"));
                        }
                        else
                        {
                            sQuantity = Convert.ToDouble(txtTotalQty.Text);
                        }
                    }
                }
                #endregion
            }

            if (strXML.ToString() == string.Empty)
            {
                lblMessage.Text = "Please add medicine before saving !";
                return;
            }
            bool isConsumable = common.myStr(Request.QueryString["DRUGORDERCODE"]) == "CO" ? true : false;
            string sXMLFre = strXMLFre != null ? strXMLFre.ToString() : "";
            Hashtable hshOutput = new Hashtable();
            if (common.myStr(Session["DrugOrderDuplicateCheck"]) == "0")
            {
                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    hshOutput = objwd.SaveEMRMedicine(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                        common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(0), common.myInt(0),
                        common.myInt(0), strXML1.ToString(), strXML.ToString(), string.Empty,
                        Request.QueryString["DRUGORDERTYPE"] != null ? common.myInt(Request.QueryString["DRUGORDERTYPE"]) : 0, common.myInt(Session["UserId"]),
                        isConsumable, sXMLFre, string.Empty, 0, false, false);
                }
                else
                {
                    hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]),
                                        common.myInt(0), common.myInt(0), common.myInt(0),
                                        0, 0, strXML1.ToString(), strXML.ToString(), string.Empty, common.myInt(Session["UserId"]), sXMLFre, isConsumable);
                }
                if ((hshOutput["@chvErrorStatus"].ToString().ToUpper().Contains("UPDATE") || hshOutput["@chvErrorStatus"].ToString().ToUpper().Contains("SAVED"))
                    && !hshOutput["@chvErrorStatus"].ToString().ToUpper().Contains("USP"))
                {

                    Session["DrugOrderDuplicateCheck"] = "1";
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    hdnItemId.Value = "0";
                    hdnItemName.Value = string.Empty;

                    if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                        return;
                    }
                }
                lblMessage.Text = hshOutput["@chvErrorStatus"].ToString();
                //////    BindGrid(string.Empty, string.Empty);
                BindBlankItemGrid();
                ViewState["Item"] = null;
                ViewState["Stop"] = null;
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
            objEMR = null;
            objwd = null;

            dsXml.Dispose();
            ds.Dispose();
            dt.Dispose();
            dv.Dispose();
            Session["DrugOrderDuplicateCheck"] = "0";
        }
    }
    protected void saveDataNew()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.WardManagement objwd = new BaseC.WardManagement();

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
                //foreach (GridViewRow dataItem in gvItem.Rows)
                //{
                HiddenField hdnGenericId;

                HiddenField hdnItemId;
                TextBox txtTotalQty;

                foreach (GridViewRow dataItem in gvProblemDetails.Rows)
                {
                    Label lblDose = (Label)dataItem.FindControl("lblDose");

                    string strText = lblDose.Text;
                    string[] strArr = strText.Split(' ');
                    string finaldosevalue = common.myStr(strArr[0]);
                    Label lblDurationText = (Label)dataItem.FindControl("lblDays");
                    Label lblInstruction = (Label)dataItem.FindControl("lblRemarks");
                    HiddenField hdBrand_Prescriptions_ID = (HiddenField)dataItem.FindControl("hdnItemID");
                    HiddenField hdnFrequencyId = (HiddenField)dataItem.FindControl("hdnFrequencyID");
                    Label lblPeriodType = (Label)dataItem.FindControl("lblDaysType");
                    HiddenField hdnUnitID = (HiddenField)dataItem.FindControl("hdnDoseUnitID");
                    HiddenField hdnFoodRelation = (HiddenField)dataItem.FindControl("hdnFoodNameID");
                    HiddenField hdnDetailsId = (HiddenField)dataItem.FindControl("hdnDetailsId");
                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    CheckBox chkboxgvTreatmentPlan = (CheckBox)dataItem.FindControl("chkboxgvTreatmentPlan");
                    HiddenField hdnDoseUnitID = (HiddenField)dataItem.FindControl("hdnDoseUnitID");

                    if (common.myBool(chkboxgvTreatmentPlan.Checked) && chkboxgvTreatmentPlan != null)
                    {
                        if (common.myInt(hdnDetailsId.Value) == 0 && common.myInt(hdnIndentId.Value) == 0)
                        {
                            //dataItem.FindControl("lblItemName")
                            coll.Add(common.myInt(common.myStr(hdBrand_Prescriptions_ID.Value)));//Brand TINYINT, 
                            coll.Add(common.myInt(common.myStr(hdnFrequencyId.Value)));//FrequencyId TINYINT,

                            if (common.myStr(finaldosevalue) != "")
                            {
                                coll.Add(common.myDec(Convert.ToDecimal(common.myStr(finaldosevalue))));//Dose DECIMAL(10,3),
                            }
                            else
                            {

                                coll.Add(common.myDec(Convert.ToDecimal(common.myStr(0))));//Dose DECIMAL(10,3),
                            }
                            coll.Add(common.myStr(common.myStr(lblDurationText.Text)));//Duration VARCHAR(20)
                            coll.Add(common.myStr(common.myStr(lblPeriodType.Text)));//DURATION TYPE CHAR(1),
                            coll.Add(common.myStr(common.myStr(lblInstruction.Text)));//INSTRUCTIONID VARCHAR(1000),
                            coll.Add(common.myInt(common.myStr(hdnUnitID.Value)));//UNITID INT,
                            coll.Add(common.myInt(common.myStr(hdnFoodRelation.Value)));//FoodRelationship INT, 
                                                                                        //coll.Add(string.Empty);// variable sequence no     
                            coll.Add(common.myInt(common.myStr(hdnDoseUnitID.Value)));//DoseTypeId INT
                            coll.Add(false);
                            strXML.Append(common.setXmlTable(ref coll));

                        }
                    }

                }

                //dsXml = new DataSet();


                //DataView DV1 = new DataView();
                //DataTable tbl1 = new DataTable();
                //tbl1 = (DataTable)ViewState["gvAddList"];


                //DV1 = tbl1.Copy().DefaultView;
                //DV1.RowFilter = "ISNULL(MergedUniqueId,'') <> '" + common.myStr(MergedUniqueId) + "'";

                /*******************************************/




                /******************************************/




                #endregion
            }
            else
            {
                #region IP Drug

                foreach (GridViewRow dataItem in gvProblemDetails.Rows)
                {
                    Label lblDose = (Label)dataItem.FindControl("lblDose");
                    HiddenField hdnItemID = (HiddenField)dataItem.FindControl("hdnItemID");

                    string strText = lblDose.Text;
                    string[] strArr = strText.Split(' ');
                    string finaldosevalue = common.myStr(strArr[0]);

                    Label lblDurationText = (Label)dataItem.FindControl("lblDays");
                    Label lblInstruction = (Label)dataItem.FindControl("lblRemarks");
                    HiddenField hdBrand_Prescriptions_ID = (HiddenField)dataItem.FindControl("hdnItemID");
                    HiddenField hdnFrequencyId = (HiddenField)dataItem.FindControl("hdnFrequencyID");
                    Label lblPeriodType = (Label)dataItem.FindControl("lblDaysType");
                    HiddenField hdnUnitID = (HiddenField)dataItem.FindControl("hdnDoseUnitID");
                    HiddenField hdnFoodRelation = (HiddenField)dataItem.FindControl("hdnFoodNameID");
                    HiddenField hdnDetailsId = (HiddenField)dataItem.FindControl("hdnDetailsId");
                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    HiddenField hdnDoseUnitID = (HiddenField)dataItem.FindControl("hdnDoseUnitID");
                    CheckBox chkboxgvTreatmentPlan = (CheckBox)dataItem.FindControl("chkboxgvTreatmentPlan");

                    if (common.myBool(chkboxgvTreatmentPlan.Checked) && chkboxgvTreatmentPlan != null)
                    {
                        if (common.myInt(hdnDetailsId.Value) == 0 && common.myInt(hdnIndentId.Value) == 0)
                        {
                            //dataItem.FindControl("lblItemName")
                            coll.Add(common.myInt(common.myStr(hdnItemID.Value)));//Item Id, 
                            coll.Add(common.myInt(common.myStr(hdnFrequencyId.Value)));//FrequencyId TINYINT,
                            coll.Add(common.myInt(common.myStr(lblDose.Text)));//Dose;
                            coll.Add(common.myStr(common.myStr(lblDurationText.Text)));//Duration VARCHAR(10)
                            coll.Add(string.Empty);//Type
                            coll.Add(common.myStr(common.myStr(lblInstruction.Text)));//INSTRUCTIONID VARCHAR(1000),
                            coll.Add(common.myInt(common.myStr(hdnUnitID.Value)));//UNITID INT,
                            coll.Add(0);//ReferanceItemId
                            coll.Add(common.myInt(common.myStr(hdnFoodRelation.Value)));//FoodRelationship INT, 
                            coll.Add(common.myInt(common.myStr(hdnDoseUnitID.Value)));//DoseTypeId INT, 
                            coll.Add(string.Empty);//Volume,
                            coll.Add(0);//Valueid,
                            coll.Add(string.Empty);//InfusionTime,
                            coll.Add(string.Empty);//TimeUnit,
                            coll.Add(string.Empty);//TotalVolume,
                            coll.Add(string.Empty);//FlowRate,
                            coll.Add(0);//FlowRateUnit,                          
                            coll.Add(DateTime.Now);//VariableDoseDate,
                            coll.Add(false);//IsSubstituteNotAllow
                            coll.Add(string.Empty);//ICDCode


                            //coll.Add(common.myInt(common.myStr(hdBrand_Prescriptions_ID.Value)));//Brand TINYINT, 

                            //if (!finaldosevalue.Equals(string.Empty))
                            //{
                            //    coll.Add(common.myDec(Convert.ToDecimal(common.myStr(finaldosevalue))));//Dose DECIMAL(10,3),
                            //}
                            //else
                            //{
                            //    coll.Add(0);//Dose DECIMAL(10,3),
                            //}

                            //coll.Add(common.myStr(common.myStr(lblPeriodType.Text)));//DURATION TYPE CHAR(1),




                            //coll.Add(string.Empty);// variable sequence no     
                            //coll.Add(false);

                            strXML.Append(common.setXmlTable(ref coll));//Save Mediciens
                        }
                    }
                }



                #endregion
            }

            //if (strXML.ToString() == string.Empty)
            //{
            //    lblMessage.Text = "Please add medicine before saving !";
            //    return;
            //}
            #region PlanofCare

            coll = new ArrayList();
            if (chkPlanOfCare.Checked)
            {
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

                    Session["PlanOfCare"] = strPlanOfCare;
                }
            }


            //  xmlTemplateDetails.Append(strNonTabularP.ToString());
            #endregion

            #region Instructions
            coll = new ArrayList();
            if (chkInstructions.Checked)
            {
                bindVisitRecord(2);
                if (!txtInstructions.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
                {
                    string strInstructions = txtInstructions.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                    strInstructions = strInstructions.Replace("\n", "<br/>");

                    coll = new ArrayList();
                    coll.Add(2); //coll.Add(item2.Cells[0].Text);
                    coll.Add("W");
                    coll.Add(strInstructions);
                    coll.Add("0");
                    coll.Add(0); //coll.Add(RowCaptionId);
                                 //if (!common.myStr(hdnPlanOfCareRecordId.Value).Equals(string.Empty))
                                 //{
                                 //    coll.Add(common.myInt(hdnPlanOfCareRecordId.Value));
                                 //}
                                 //else
                                 //{
                    coll.Add(common.myInt(ViewState["RecordId"]));
                    //}
                    coll.Add(2);
                    strNonTabularP.Append(common.setXmlTable(ref coll));

                    // Session["Instructions"] = strInstructions;
                }
            }


            #endregion

            #region  Problem
            coll = new ArrayList();
            StringBuilder objXMLProblem = new StringBuilder();
            BaseC.ParseData Parse = new BaseC.ParseData();
            if (chkChiefComplaint.Checked)
            {
                if (!txtChiefComplaint.Text.Equals(string.Empty))
                {
                    // string editID;
                    //if (!(txtedit.Text.Trim().Equals(string.Empty)))
                    //{
                    //    editID = txtedit.Text;
                    //}
                    //else
                    //{
                    //    editID = string.Empty;
                    //}
                    string TemplateId = Request.QueryString["POPUP"] != null && common.myStr(Request.QueryString["POPUP"]).Equals("StaticTemplate") ? common.myStr(Request.QueryString["TemplateFieldId"]) : "0";

                    string strProblem = Parse.ParseQ(txtChiefComplaint.Text.Trim()).Replace("\n", "<br/>");
                    //  string strProblem = editorProblem.Text.Trim().Replace("\r\n", "<br/>");
                    if (common.myLen(strProblem) > 4000)
                    {
                        Alert.ShowAjaxMsg("Chief complaints (free text) length must be less than 4000 character!", this.Page);
                        return;
                    }
                    coll.Add(0);//Id
                    coll.Add(0);//ProblemId
                    coll.Add(strProblem);//Problem
                    coll.Add(0);//DurationID
                    coll.Add(string.Empty);//Duration
                    coll.Add(0);//ContextID
                    coll.Add(string.Empty);//Context
                    coll.Add(0);//SeverityId
                    coll.Add(string.Empty);//Severity
                    coll.Add(0);//IsPrimary
                    coll.Add(0);//IsChronic
                    coll.Add(common.myStr(Session["DoctorID"]));//DoctorId
                    coll.Add(common.myStr(Session["FacilityId"]));//FacilityId
                    coll.Add(0);//SCTId
                    coll.Add(string.Empty);//QualityIDs
                    coll.Add(0);//LocationID
                    coll.Add(string.Empty);//Location
                    coll.Add(0);//OnsetID
                    coll.Add(0);//AssociatedProblemId1
                    coll.Add(string.Empty);//AssociatedProblem1
                    coll.Add(0);//AssociatedProblemId2
                    coll.Add(string.Empty);//AssociatedProblem2
                    coll.Add(0);//AssociatedProblemId3
                    coll.Add(string.Empty);//AssociatedProblem3
                    coll.Add(0);//AssociatedProblemId4
                    coll.Add(string.Empty);//AssociatedProblem4
                    coll.Add(0);//AssociatedProblemId5
                    coll.Add(string.Empty);//AssociatedProblem5
                    coll.Add(string.Empty);//Side
                    coll.Add(0);//ConditionId
                    coll.Add(0);//Percentage
                    coll.Add(common.myInt(ddlDuration.SelectedValue));//Durations
                    coll.Add(common.myStr(ddlDurationType.SelectedValue));//DurationType
                    coll.Add(TemplateId);//TemplateFieldId
                    coll.Add(0);//ComplaintSearchId
                    objXMLProblem.Append(common.setXmlTable(ref coll));
                }
            }
            #endregion

            #region History
            if (chkHistory.Checked)
            {
                if (common.myLen(txtHistory.Text) > 0)
                {
                    bindVisitRecord(6048);
                    string strHistory = txtHistory.Text.Replace("\n", "<br/>");
                    coll = new ArrayList();
                    coll.Add(122195);
                    coll.Add("W");
                    coll.Add(strHistory);
                    coll.Add("0");
                    coll.Add(0);
                    coll.Add(common.myInt(ViewState["RecordId"]));
                    coll.Add(27230);
                    strNonTabularP.Append(common.setXmlTable(ref coll));
                }
            }

            #endregion

            #region Examination

            if (chkExamination.Checked)
            {
                if (common.myLen(txtExamination.Text) > 0)
                {
                    bindVisitRecord(6049);
                    string strExamination = txtExamination.Text.Replace("\n", "<br/>");

                    coll = new ArrayList();
                    coll.Add(122196);
                    coll.Add("W");
                    coll.Add(strExamination);
                    coll.Add("0");
                    coll.Add(0);
                    coll.Add(common.myInt(ViewState["RecordId"]));
                    coll.Add(27231);
                    strNonTabularP.Append(common.setXmlTable(ref coll));

                }
            }
            xmlTemplateDetails.Append(strNonTabularP.ToString());
            #endregion

            #region Diagnosis Details
            StringBuilder strXMLDiagnosisDetails = new StringBuilder();
            ArrayList col = new ArrayList();

            foreach (GridViewRow row in gvDiagnosisDetails.Rows)
            {
                CheckBox chkgvDiagnosisDetails = (CheckBox)row.FindControl("chkgvDiagnosisDetails");
                HiddenField hdnICDID = (HiddenField)row.FindControl("hdnICDID");
                if (common.myBool(chkgvDiagnosisDetails.Checked) && chkgvDiagnosisDetails != null)
                {
                    // col.Add(common.myInt(hdnICDID.Value));
                    strXMLDiagnosisDetails.Append("<Table1><c1>");
                    strXMLDiagnosisDetails.Append(common.myInt(hdnICDID.Value));
                    strXMLDiagnosisDetails.Append("</c1>");
                    strXMLDiagnosisDetails.Append("</Table1>");
                }

            }
            //   strXMLDiagnosisDetails.Append(common.setXmlTable(ref col));
            #endregion


            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            DataSet dsDOCTORID = new DataSet();
            ds = order.GetEncounterCompany(common.myInt(Session["EncounterId"]));

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    ViewState["DOCTORID"] = common.myInt(ds.Tables[0].Rows[0]["DOCTORID"]);
                }
            }

            Hashtable hshOutput = new Hashtable();
            if (common.myStr(Session["OPIP"]).Equals("I"))
            {

                //hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                //   common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), 0, 0,
                //   0, strXML.ToString(), common.myInt(Session["UserId"]));


                hshOutput = objwd.SaveEMRMedicine(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                          common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(0), common.myInt(0),
                          common.myInt(0), strXML1.ToString(), strXML.ToString(), string.Empty, 0, common.myInt(Session["UserId"]),
                          false, string.Empty, string.Empty, 0, false, false); //common.myInt(Session["DoctorID"])

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "Data Saved Successfully";
                lblMessage.Font.Bold = true;
            }
            else
            {
                hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                  common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), 0, 0,
                                  common.myInt(ViewState["DOCTORID"]), strXML.ToString(), common.myInt(Session["UserId"]),
                                  xmlTemplateDetails.ToString(), common.myStr(Session["OPIP"]), objXMLProblem.ToString(), common.myInt(Session["DoctorID"])
                                  , common.myStr(strXMLDiagnosisDetails.ToString()));

                lblMessage.ForeColor = System.Drawing.Color.Green;

                lblMessage.Text = "Data Saved Successfully";
                lblMessage.Font.Bold = true;
            }

            if (common.myStr(hshOutput).Contains(""))
            {
                string strPopup = "<script language='javascript' ID='script1'>" + "window.close();" + "</script>";

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

        finally
        {
            objEMR = null;
            objwd = null;
            dsXml.Dispose();
            ds.Dispose();
            dt.Dispose();
            dv.Dispose();
            strXML = null;
            strXML1 = null;
            coll = null;
            coll1 = null;
            strXMLFre = null;
            collFre = null;
            xmlTemplateDetails = null;
            strNonTabularP = null;
        }
    }

    private void addItem()
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
        try
        {
            DataRow DR;
            DataRow DR1;
            decimal dQty = 0;
            int countRow = 0;
            int IDummy = 0;
            ////if (common.myInt(hdnItemId.Value).Equals(0)
            ////    && common.myInt(hdnGenericId.Value).Equals(0)
            ////    && common.myLen(txtCustomMedication.Text).Equals(0))
            ////{
            ////    Alert.ShowAjaxMsg("Please select drug", Page);
            ////    return;
            ////}

            ////if (!chkCustomMedication.Checked)
            if (IDummy == 0)
            {
                DataTable tblItem = new DataTable();
                tblItem = (DataTable)ViewState["ItemDetail"];

                if (tblItem != null)
                {
                    DataView dvRemoveBlank = tblItem.Copy().DefaultView;
                    dvRemoveBlank.RowFilter = "ISNULL(GenericId,0)>0 OR ISNULL(ItemId,0)>0 OR CustomMedication<>True";
                    tblItem = dvRemoveBlank.ToTable();

                    DataView DVItem = tblItem.Copy().DefaultView;

                    if (common.myInt(hdnItemId.Value) > 0)
                    {
                        DVItem.RowFilter = "ItemId=" + common.myInt(hdnItemId.Value);
                    }
                    //else
                    //{
                    //    DVItem.RowFilter = "GenericId=" + common.myInt(hdnGenericId.Value);
                    //}

                    if (!common.myBool(ViewState["Edit"]))
                    {
                        if (DVItem.ToTable().Rows.Count > 0)
                        {
                            Alert.ShowAjaxMsg("Item already added", Page);
                            return;
                        }
                    }
                }
            }
            if (ViewState["Item"] == null && ViewState["Edit"] == null)
            {
                dt = CreateItemTable();
                dt1 = CreateItemTable();
            }
            else
            {
                dtold = (DataTable)ViewState["Item"];

                //BaseC.clsIndent objIndent = new BaseC.clsIndent(sConString);
                //if (objIndent.IsAllergyWithPrescribingMedicine(common.myInt(Session["EncounterId"]), common.myInt(ViewState["ItemId"])))
                //{
                //    return;
                //}

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
                if (dt.Rows.Count > 0)
                {
                    if (ViewState["Edit"] == null)
                    {
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

            DR = dt.NewRow();
            DR1 = dt1.NewRow();
            foreach (GridViewRow row in gvProblemDetails.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkboxgvTreatmentPlan");
                if (chk != null & chk.Checked)
                {
                    if (!dt.Columns.Contains("Frequency"))
                    {
                        dt.Columns.Add("Frequency", typeof(decimal));
                    }
                    //Item
                    ////if (ddlIndentType.SelectedValue == string.Empty)
                    ////{
                    ////    Alert.ShowAjaxMsg("Please select Indent Type", Page);
                    ////    return;
                    ////}

                    ////DR["IndentTypeId"] = Convert.ToInt16(ddlIndentType.SelectedValue);
                    DR["IndentTypeId"] = Convert.ToInt16(0);
                    ////DR["IndentType"] = ddlIndentType.SelectedValue == string.Empty ? string.Empty : ddlIndentType.Text;
                    DR["IndentType"] = string.Empty;
                    DR["IndentId"] = 0;
                    ////DR["GenericId"] = ddlGeneric.SelectedValue == string.Empty ? 0 : common.myInt(ddlGeneric.SelectedValue);
                    DR["GenericId"] = common.myInt(0);
                    ////DR["ItemId"] = common.myInt(ddlBrand.SelectedValue);
                    HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemID");
                    DR["ItemId"] = common.myInt(hdnItemId.Value);
                    ////DR["GenericName"] = ddlGeneric.SelectedValue == string.Empty ? string.Empty : ddlGeneric.Text;
                    DR["GenericName"] = string.Empty;
                    ////DR["ItemName"] = (!chkCustomMedication.Checked && common.myInt(ddlBrand.SelectedValue) != 0) ? ddlBrand.Text : txtCustomMedication.Text;
                    Label lblItemname = (Label)row.FindControl("lblItemName");
                    DR["ItemName"] = common.myStr(lblItemname.Text);
                    ////DR["StrengthValue"] = common.myStr(txtStrengthValue.Text);
                    DR["StrengthValue"] = string.Empty;
                    ////DR["RouteId"] = common.myInt(ddlRoute.SelectedValue);
                    DR["RouteId"] = common.myInt(0);
                    ////DR["CustomMedication"] = chkCustomMedication.Checked;
                    DR["CustomMedication"] = false;
                    ////DR["NotToPharmacy"] = chkNotToPharmacy.Checked;
                    DR["NotToPharmacy"] = false;
                    ////DR["IsInfusion"] = hdnInfusion.Value == "1" ? true : false;
                    DR["IsInfusion"] = true;
                    ////DR["IsInfusion"] = common.myBool(hdnIsInjection.Value);
                    DR["IsInfusion"] = common.myBool(0);
                    ////DR["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
                    DR["FormulationId"] = common.myInt(0);

                    ////if (ddlFormulation.SelectedItem != null)
                    ////{
                    ////    DR["FormulationName"] = common.myStr(ddlFormulation.SelectedItem.Text);
                    ////}

                    DR["FormulationName"] = string.Empty;
                    ////DR["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
                    ////DR["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");
                    DR["StartDate"] = string.Empty;
                    DR["EndDate"] = string.Empty;

                    ///Item Detail
                    // TextBox txtInstructions = new TextBox();
                    //RadComboBox ddlFoodRelation = new RadComboBox();
                    #region Item Add
                    //foreach (GridViewRow dataItem in gvItemDetails.Rows)
                    //{

                    if (!dt1.Columns.Contains("DurationText"))
                    {
                        dt1.Columns.Add("DurationText", typeof(string));
                    }


                    string sFrequency = "0";


                    //Label lblTotalQty = (Label)dataItem.FindControl("lblTotalQty");
                    string Type = string.Empty;
                    decimal dDuration = 0;

                    ////switch (common.myStr(ddlPeriodType.SelectedValue))
                    ////{
                    ////    case "N":
                    ////        Type = txtDuration.Text + " Minute(s)";
                    ////        dDuration = 1;
                    ////        break;
                    ////    case "H":
                    ////        Type = txtDuration.Text + " Hour(s)";
                    ////        dDuration = 1;
                    ////        break;
                    ////    case "D":
                    ////        Type = txtDuration.Text + " Day(s)";
                    ////        dDuration = 1;
                    ////        break;
                    ////    case "W":
                    ////        Type = txtDuration.Text + " Week(s)";
                    ////        dDuration = 7;
                    ////        break;
                    ////    case "M":
                    ////        Type = txtDuration.Text + " Month(s)";
                    ////        dDuration = 30;
                    ////        break;
                    ////    case "Y":
                    ////        Type = txtDuration.Text + " Year(s)";
                    ////        dDuration = 365;
                    ////        break;
                    ////}

                    if (Request.QueryString["DRUGORDERCODE"] == null)
                    {
                        if (common.myBool(ViewState["ISCalculationRequired"]))
                        {
                            //////    dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
                        }
                        else
                        {
                            dQty = 1;
                        }
                        ////DR1["RouteId"] = ddlRoute.SelectedValue;
                        ////DR1["RouteName"] = ddlRoute.SelectedItem.Text;
                        DR1["RouteId"] = 0;
                        DR1["RouteName"] = string.Empty;

                    }
                    else
                    {
                        dQty = 0;
                    }

                    ////DR1["GenericId"] = ddlGeneric.SelectedValue == string.Empty ? 0 : common.myInt(ddlGeneric.SelectedValue);
                    DR1["GenericId"] = common.myInt(0);

                    ////DR1["ItemId"] = ddlBrand.SelectedValue == string.Empty ? 0 : common.myInt(ddlBrand.SelectedValue);
                    HiddenField hdnItemId1 = (HiddenField)row.FindControl("hdnItemId");


                    DR1["ItemId"] = common.myInt(hdnItemId1.Value);

                    ////DR1["GenericName"] = ddlGeneric.SelectedValue == string.Empty ? string.Empty : ddlGeneric.Text;
                    ////DR1["GenericName"] = ddlGeneric.SelectedValue == string.Empty ? string.Empty : ddlGeneric.Text;

                    DR1["GenericName"] = string.Empty;

                    ////DR1["ItemName"] = (!chkCustomMedication.Checked && common.myInt(ddlBrand.SelectedValue) != 0) ? ddlBrand.Text : txtCustomMedication.Text;

                    Label lblItemname1 = (Label)row.FindControl("lblItemName");
                    DR1["ItemName"] = common.myStr(lblItemname1.Text);


                    ////DR1["StrengthValue"] = common.myStr(txtStrengthValue.Text);
                    DR1["StrengthValue"] = string.Empty;



                    ////DR1["Dose"] = txtDose.Text;
                    Label Dose = (Label)row.FindControl("lblDose");
                    DR1["Dose"] = common.myDec(Dose.Text);

                    ////DR1["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue) == 0 ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
                    HiddenField FrequencyID = (HiddenField)row.FindControl("hdnFrequencyID");


                    DR1["Frequency"] = common.myInt(FrequencyID.Value);

                    ////DR1["Duration"] = txtDuration.Text;
                    Label Days = (Label)row.FindControl("lblDays");

                    DR1["Duration"] = common.myDec(Days.Text);

                    DR1["DurationText"] = Type;
                    ////DR1["Type"] = ddlPeriodType.SelectedValue;
                    Label DaysType = (Label)row.FindControl("lblDaysType");

                    DR1["Type"] = common.myStr(DaysType.Text);


                    ////DR1["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
                    DR1["StartDate"] = common.myDate("01/01/2015").ToString("dd/MM/yyyy");

                    ////DR1["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");
                    DR1["EndDate"] = common.myDate("01/01/2016").ToString("dd/MM/yyyy");


                    ////DR1["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "0" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);

                    HiddenField FoodNameID = (HiddenField)row.FindControl("hdnFoodNameID");



                    DR1["FoodRelationshipId"] = common.myInt(FoodNameID.Value);

                    ////DR1["FoodRelationship"] = ddlFoodRelation.SelectedValue == "0" ? string.Empty : common.myStr(ddlFoodRelation.Text);


                    Label FoodName = (Label)row.FindControl("lblFoodName");
                    DR1["FoodRelationship"] = common.myStr(FoodName.Text);

                    //// DR1["UnitId"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? 0 : common.myInt(ddlUnit.SelectedValue);
                    HiddenField DoseUnitID = (HiddenField)row.FindControl("hdnDoseUnitID");

                    DR1["UnitId"] = common.myInt(DoseUnitID.Value);

                    ////DR1["UnitName"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? string.Empty : ddlUnit.Text;
                    Label DoseUnit = (Label)row.FindControl("lblDoseUnit");

                    DR1["UnitName"] = common.myStr(DoseUnit.Text);

                    //DR1["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
                    DR1["CIMSItemId"] = common.myStr(string.Empty);

                    ////DR1["CIMSType"] = common.myStr(hdnCIMSType.Value);
                    DR1["CIMSType"] = common.myStr(string.Empty);

                    ////DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
                    DR1["VIDALItemId"] = common.myInt(0);

                    DR1["PrescriptionDetail"] = string.Empty;
                    ////DR1["CustomMedication"] = chkCustomMedication.Checked;
                    DR1["CustomMedication"] = false;

                    ////DR1["Instructions"] = txtInstructions.Text;
                    Label Remarks = (Label)row.FindControl("lblRemarks");

                    DR1["Instructions"] = common.myStr(Remarks.Text);

                    ////DR1["ReferanceItemId"] = ddlReferanceItem.SelectedValue == string.Empty || ddlReferanceItem.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlReferanceItem.SelectedValue);
                    DR1["ReferanceItemId"] = Convert.ToInt32(0);

                    ////DR1["ReferanceItemName"] = ddlReferanceItem.SelectedValue == string.Empty || ddlReferanceItem.SelectedValue == "0" ? string.Empty : ddlReferanceItem.Text;
                    DR1["ReferanceItemName"] = string.Empty;

                    ////DR1["DoseTypeId"] = common.myInt(ddlDoseType.SelectedValue);

                    HiddenField DoseUnitID1 = (HiddenField)row.FindControl("hdnDoseUnitID");
                    DR1["DoseTypeId"] = common.myInt(DoseUnitID1.Value);

                    ////DR1["DoseTypeName"] = ddlDoseType.SelectedValue == string.Empty || ddlDoseType.SelectedValue == "0" ? string.Empty : ddlDoseType.Text;
                    Label DoseUnit1 = (Label)row.FindControl("lblDoseUnit");

                    DR1["DoseTypeName"] = common.myStr(DoseUnit1.Text);

                    ////DR1["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
                    DR1["FormulationId"] = common.myInt(0);

                    //if (ddlFormulation.SelectedItem != null)
                    //{
                    //    DR1["FormulationName"] = common.myStr(ddlFormulation.SelectedItem.Text);
                    //}

                    DR1["FormulationName"] = string.Empty;

                    ////if (hdnInfusion.Value == "1" || (ddlReferanceItem.SelectedItem != null && common.myBool(ddlReferanceItem.SelectedItem.Attributes["IsInfusion"]) == true))
                    ////{
                    ////    DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                    ////    DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
                    ////    DR1["IsInfusion"] = true;
                    ////    DR["IsInfusion"] = true;
                    ////}
                    ////else if (ddlDoseType.SelectedValue != "0")
                    ////{
                    ////    DR1["IsInfusion"] = false;
                    ////    DR1["FrequencyId"] = 0;
                    ////    DR1["FrequencyName"] = string.Empty;
                    ////}
                    ////else
                    ////{
                    ////    DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                    ////    DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
                    ////    DR1["IsInfusion"] = false;
                    ////}


                    HiddenField FrequencyID1 = (HiddenField)row.FindControl("hdnFrequencyID");

                    Label Frequency1 = (Label)row.FindControl("lblFrequency");
                    DR1["FrequencyId"] = common.myInt(FrequencyID1.Value);
                    DR1["FrequencyName"] = common.myStr(Frequency1.Text);
                    DR1["IsInfusion"] = false;

                    ////DR1["Volume"] = common.myStr(txtVolume.Text);
                    DR1["Volume"] = string.Empty;

                    ////DR1["VolumeUnitId"] = common.myInt(ddlVolumeUnit.SelectedValue);
                    DR1["VolumeUnitId"] = common.myInt(0);

                    ////DR1["InfusionTime"] = common.myStr(txtTimeInfusion.Text);
                    DR1["InfusionTime"] = common.myStr(string.Empty);

                    ////DR1["TimeUnit"] = common.myStr(ddlTimeUnit.SelectedValue);
                    DR1["TimeUnit"] = common.myStr(string.Empty);

                    ////DR1["TotalVolume"] = common.myStr(txtTotalVolumn.Text);
                    DR1["TotalVolume"] = common.myStr(string.Empty);

                    ////DR1["FlowRate"] = common.myStr(txtFlowRateUnit.Text);
                    DR1["FlowRate"] = common.myStr(string.Empty);

                    ////DR1["FlowRateUnit"] = common.myInt(ddlFlowRateUnit.SelectedValue);
                    DR1["FlowRateUnit"] = common.myInt(0);

                    ////DR1["XMLVariableDose"] = hdnXmlVariableDoseString.Value;
                    DR1["XMLVariableDose"] = string.Empty;

                    ////DR1["XMLFrequencyTime"] = hdnXmlFrequencyTime.Value;
                    DR1["XMLFrequencyTime"] = string.Empty;

                    ////DR1["OverrideComments"] = common.myStr(txtCommentsDrugAllergy.Text);
                    DR1["OverrideComments"] = common.myStr(string.Empty);

                    ////DR1["OverrideCommentsDrugToDrug"] = common.myStr(txtCommentsDrugToDrug.Text);
                    DR1["OverrideCommentsDrugToDrug"] = common.myStr(string.Empty);

                    ////DR1["OverrideCommentsDrugHealth"] = common.myStr(txtCommentsDrugHealth.Text);
                    DR1["OverrideCommentsDrugHealth"] = common.myStr(string.Empty);
                    ////DR1["IsSubstituteNotAllowed"] = chkSubstituteNotAllow.Checked;
                    DR1["IsSubstituteNotAllowed"] = true;
                    //dt1.Rows.Add(DR1);
                    //dt1.AcceptChanges();


                    //dt1.TableName = "ItemDetail";
                    //System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                    //System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                    //dt1.WriteXml(writer);

                    //string xmlSchema = writer.ToString();
                    //DR["XMLData"] = xmlSchema;


                    //saveData(xmlSchema);

                    //dt1 = null;
                    //DR = null;
                    //DR1 = null;



                }
            }

            dt1.Rows.Add(DR1);
            dt1.AcceptChanges();


            ////ddlReferanceItem.SelectedValue = "0";
            ////txtInstructions.Text = string.Empty;
            // }
            #endregion
            dt1.TableName = "ItemDetail";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
            dt1.WriteXml(writer);

            string xmlSchema = writer.ToString();

            GridViewRow gv1 = gvProblemDetails.Rows[0];
            gv1 = gvProblemDetails.Rows[0];
            HiddenField XMLData = (HiddenField)gv1.FindControl("hdnXMLData");

            XMLData.Value = xmlSchema;
            DR["XMLData"] = xmlSchema;
            saveData(xmlSchema);
            DR["PrescriptionDetail"] = emr.GetPrescriptionDetailStringNew(dt1, common.myStr(Session["EMRPrescriptionDoseShowInFractionalValue"]));
            //DR["Qty"] = dQty.ToString("F2");

            DR["Qty"] = 0;
            dt.Rows.Add(DR);
            dt.AcceptChanges();

            ViewState["DataTableDetail"] = null;
            ViewState["ItemDetail"] = dt1;
            ViewState["Item"] = dt1;
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
    private DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("IndentNo", typeof(int));
        dt.Columns.Add("IndentDate", typeof(string));
        dt.Columns.Add("IndentTypeId", typeof(int));
        dt.Columns.Add("IndentType", typeof(string));
        dt.Columns.Add("GenericId", typeof(int));
        dt.Columns.Add("GenericName", typeof(string));
        dt.Columns.Add("ItemId", typeof(int));
        dt.Columns.Add("IndentId", typeof(int));
        dt.Columns.Add("ItemName", typeof(string));
        dt.Columns.Add("FormulationId", typeof(int));
        dt.Columns.Add("RouteId", typeof(int));
        dt.Columns.Add("StrengthId", typeof(int));
        dt.Columns.Add("StrengthValue", typeof(string));
        dt.Columns.Add("CIMSItemId", typeof(string));
        dt.Columns.Add("CIMSType", typeof(string));
        dt.Columns.Add("VIDALItemId", typeof(int));
        dt.Columns.Add("Qty", typeof(decimal));
        dt.Columns.Add("PrescriptionDetail", typeof(string));
        dt.Columns.Add("ReferanceItemId", typeof(int));
        dt.Columns.Add("StartDate", typeof(string));
        dt.Columns.Add("EndDate", typeof(string));
        dt.Columns.Add("CustomMedication", typeof(bool));
        dt.Columns.Add("NotToPharmacy", typeof(bool));
        dt.Columns.Add("XMLData", typeof(string));
        dt.Columns.Add("IsInfusion", typeof(bool));
        dt.Columns.Add("IsInjection", typeof(bool));
        dt.Columns.Add("Dose", typeof(double));
        dt.Columns.Add("Days", typeof(double));

        dt.Columns.Add("RouteName", typeof(string));
        dt.Columns.Add("Frequency", typeof(decimal));
        dt.Columns.Add("FrequencyName", typeof(string));
        dt.Columns.Add("FrequencyId", typeof(int));
        dt.Columns.Add("Duration", typeof(string));
        dt.Columns.Add("Type", typeof(string));
        dt.Columns.Add("DurationText", typeof(string));
        dt.Columns.Add("UnitId", typeof(int));
        dt.Columns.Add("UnitName", typeof(string));
        dt.Columns.Add("FoodRelationshipId", typeof(int));
        dt.Columns.Add("FoodRelationship", typeof(string));
        dt.Columns.Add("ReferanceItemName", typeof(string));
        dt.Columns.Add("Instructions", typeof(string));
        dt.Columns.Add("DoseTypeId", typeof(int));
        dt.Columns.Add("DoseTypeName", typeof(string));
        dt.Columns.Add("Volume", typeof(string));
        dt.Columns.Add("VolumeUnitId", typeof(int));
        dt.Columns.Add("InfusionTime", typeof(string));
        dt.Columns.Add("TimeUnit", typeof(string));
        dt.Columns.Add("TotalVolume", typeof(string));
        dt.Columns.Add("FlowRate", typeof(string));
        dt.Columns.Add("FlowRateUnit", typeof(int));
        dt.Columns.Add("XMLVariableDose", typeof(string));
        dt.Columns.Add("XMLFrequencyTime", typeof(string));
        dt.Columns.Add("FormulationName", typeof(string));
        dt.Columns.Add("DetailsId", typeof(int));

        dt.Columns.Add("OverrideComments", typeof(string));
        dt.Columns.Add("OverrideCommentsDrugToDrug", typeof(string));
        dt.Columns.Add("OverrideCommentsDrugHealth", typeof(string));
        dt.Columns.Add("IsSubstituteNotAllowed", typeof(bool));
        return dt;
    }

    //private DataTable CreateItemTable()
    //{
    //    DataTable dt = new DataTable();
    //    return dt;

    //}
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

    //public void ddlDiagnosiss_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    //{

    //    try
    //    {
    //        DataTable data = new DataTable();
    //        data = CreateTable();
    //        data = PopulateAllDiagnosis(e.Text);



    //        int itemOffset = e.NumberOfItems;
    //        if (itemOffset == 0)
    //        {
    //            this.ddlDiagnosiss.Items.Clear();
    //        }
    //        int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
    //        e.EndOfItems = endOffset == data.Rows.Count;

    //        for (int i = itemOffset; i < endOffset; i++)
    //        {
    //            //RadCmbPatientSearch.Items.Add(new RadComboBoxItem(data.Rows[i]["CompanyName"].ToString(), data.Rows[i]["CompanyName"].ToString()));
    //            RadComboBoxItem item = new RadComboBoxItem();
    //            item.Text = (string)data.Rows[i]["ICDDescription"];
    //            item.Value = data.Rows[i]["ICDID"].ToString();
    //            item.Attributes["ICDID"] = data.Rows[i]["ICDID"].ToString();
    //            item.Attributes["ICDCode"] = data.Rows[i]["ICDCode"].ToString();
    //            item.Attributes["ICDDescription"] = data.Rows[i]["ICDDescription"].ToString();

    //            this.ddlDiagnosiss.Items.Add(item);
    //            item.DataBind();
    //        }
    //        e.Message = GetStatusMessage(endOffset, data.Rows.Count);
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }



    //}
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

            if (Session["encounterid"] != null)
            {
                objDiag = new BaseC.DiagnosisDA(sConString);
                ds = new DataSet();

                string strSearchCriteria = string.Empty;

                strSearchCriteria = "%" + txt + "%";
                ds = objDiag.BindDiagnosis(common.myInt(0), common.myInt(0), strSearchCriteria);

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
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return DT;
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

    private void BindDiagnosis()
    {
        try
        {
            //if ((sTemplateName.Equals("Diagnosis") && sTemplateType.Equals("S")))
            //{
            //if (ds.Tables[11].Rows.Count.Equals(0))
            //{
            //    dvFilterDignosis = new DataView(ds.Tables[11].Copy());
            //    DataTable dtDiagnosisDetails = new DataTable();
            //    dtDiagnosisDetails = dvFilterDignosis.ToTable();
            //    DataRow dr = dtDiagnosisDetails.NewRow();
            //    dtDiagnosisDetails.Rows.InsertAt(dr, 0);

            //    ViewState["IsibtnDelete"] = "1";
            //    gvDiagnosisDetails.DataSource = dtDiagnosisDetails;
            //    gvDiagnosisDetails.DataBind();
            //}
            //else
            //{
            //    ViewState["IsibtnDelete"] = "0";
            //    gvDiagnosisDetails.DataSource = ds.Tables[11];
            //    gvDiagnosisDetails.DataBind();
            //}
            // }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }




}
