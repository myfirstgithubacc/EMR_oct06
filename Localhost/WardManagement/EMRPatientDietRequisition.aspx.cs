using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Web.UI;
public partial class Diet_EMRPatientDietRequisition : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    clsExceptionLog objException = new clsExceptionLog();
    private bool RowSelStauts = false;

    protected void Page_PreInit(Object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["Ward"]) != "Ward")
        {
            this.MasterPageFile = "~/Include/Master/EMRMaster.master";
        }
        else
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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

            Div1.Visible = false;

            if (common.myStr(Request.QueryString["Ward"]) != "Ward")
            {
                btnClose.Visible = false;
                if (common.myStr(Session["RegistrationNo"]) != "")
                {
                    BindPatientHiddenDetails();
                }

                if (common.myStr(Session["RegistrationId"]) != "")
                {
                    hdnRegId.Value = common.myStr(Session["RegistrationId"]);
                }

                if (common.myStr(Session["EncounterId"]) != "")
                {
                    hdnEncounterId.Value = common.myStr(Session["EncounterId"]);
                }
            }
            else
            {
                if (common.myStr(Request.QueryString["RegNo"]) != "")
                {
                    BindPatientHiddenDetails();
                }
                if (common.myStr(Session["RegistrationId"]) != "")
                {
                    hdnRegId.Value = common.myStr(Session["RegistrationId"]);
                }

                if (common.myStr(Request.QueryString["EncId"]) != "")
                {
                    hdnEncounterId.Value = common.myStr(Request.QueryString["EncId"]);
                }
            }
            BindPatientHeightWeight();

            //Added on 11-08-2014 Start Naushad Ali
            BindBlankGridGvDiet();
            BindDietTypeCategoryCombo();
            BindDietDtails();
            BindModeofFeedingCombo();
            BindNPOCombo();
            BindInternationalCombo();
            BindGridgvDietList(1);
            //Added on 11-08-2014 End Naushad Ali
            BindGridPrecauationFoodHabit();
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
            {
                btnSave.Visible = false;
                btnNew.Visible = false;
                gvDietDetail.Columns[4].Visible = false;
            }

            if (common.myStr(Session["IsAdminGroup"]) != "True")
            {
                SetPermission();
            }
            hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);

            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            ds = objEMR.getProvisionalAndFinalDiagnosis(common.myInt(Session["FacilityId"]), common.myInt(hdnEncounterId.Value));

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtdiagnosis.Text = common.myStr(ds.Tables[0].Rows[0]["ClinicalDiagnosis"]);
                txtdiagnosis.ToolTip = txtdiagnosis.Text;
            }
        }
    }

    private void SetPermission()
    {
        //if (!common.myInt(Session["ModuleId"]).Equals(3))
        //{
        //    return;
        //}

        UserAuthorisations ua1 = new UserAuthorisations(sConString);
        try
        {
            btnSave.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
            ViewState["IsAllowEdit"] = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
            ViewState["IsAllowCancel"] = ua1.CheckPermissionsForEMRModule("C", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
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

    protected void BindPatientHeightWeight()
    {
        BaseC.Diet objbc = new BaseC.Diet(sConString);
        DataSet ds = new DataSet();
        try
        {
            DataView dv = new DataView();

            ds = objbc.GetPatientHeightWeight(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["EncounterId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (common.myStr(ds.Tables[0].Rows[0]["Height"].ToString()) != "")
                {
                    //  txtHeight.Text = ds.Tables[0].Rows[0]["Height"].ToString().Trim();
                }
                if (common.myStr(ds.Tables[0].Rows[0]["Weight"].ToString()) != "")
                {
                    //   txtWeight.Text = ds.Tables[0].Rows[0]["Weight"].ToString().Trim();
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindDietDtails()
    {
        BaseC.clsEMR objbc = new BaseC.clsEMR(sConString);
        DataTable dt = new DataTable();

        try
        {
            dt = objbc.getEMRDietDetail(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRegId.Value), common.myInt(hdnEncounterId.Value));

            if (dt.Rows.Count == 0)
            {
                DataRow DR = dt.NewRow();
                dt.Rows.Add(DR);
            }

            gvDietDetail.DataSource = dt;
            gvDietDetail.DataBind();
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
            dt.Dispose();
        }
    }
    //Code by Jogender singh
    private void Bindgridfoodhabit()
    {
        BaseC.clsEMR objbc = new BaseC.clsEMR(sConString);
        DataTable dt = new DataTable();

        try
        {
            dt = objbc.getEMRDietDetail(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                common.myInt(hdnRegId.Value), common.myInt(hdnEncounterId.Value));

            if (dt.Rows.Count == 0)
            {
                DataRow DR = dt.NewRow();
                dt.Rows.Add(DR);
            }

            GvFoodHabit.DataSource = dt;
            GvFoodHabit.DataBind();
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
            dt.Dispose();
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
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void RefreshForm()
    {
        ViewState["DietRequestID"] = null;
        BindBlankGridGvDiet();
        txtRemakrs.Text = "";
        txtdiagnosis.Text = "";
        ddlDietTypeCategory.SelectedValue = "0";
        lblMessage.Text = "";
        ViewState["VisibleType"] = string.Empty;
        btnSave.Enabled = true;
        ddlModeofFeeding.SelectedIndex = 0;
        GvDietTypeSubCategoryDetail.DataSource = null;
        GvDietTypeSubCategoryDetail.DataBind();
        ddlNPO.SelectedIndex = 0;
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        RefreshForm();
    }


    protected void btnDelete_OnClick(object sender, EventArgs e)
    {
        BaseC.clsEMR objbc = new BaseC.clsEMR(sConString);


        try
        {

            if (common.myStr(ViewState["DietRequestID"]) != null)
            {
                string DietRequestId = common.myStr(ViewState["DietRequestID"]);
                int result = objbc.DeleteEMRDiet(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRegId.Value), common.myInt(hdnEncounterId.Value), common.myInt(DietRequestId), common.myInt(Session["UserID"]));
                lblMessage.Text = "Record Deleted";

            }
            Div1.Visible = false;

            BindDietDtails();
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
        }
    }


    protected void btnClosepopup_OnClick(object sender, EventArgs e)
    {
        Div1.Visible = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (common.myBool(hdnIsPasswordRequired.Value))
        {
            IsValidPassword();
            return;
        }

        if (common.myBool(strnpo.Visible == true) && common.myInt(ddlNPO.SelectedItem.Value) <= 0)
        {
            lblMessage.Text = "Please Select NPO Reasion";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //Alert.Show("Please select NPO reasion");
            return;
        }

        SaveData();

    }
    private void SaveData()
    {
        if (!(common.myStr(ViewState["VisibleType"]).Equals("Show")))
        {
            BaseC.Diet objbc = new BaseC.Diet(sConString);
            try
            {
                btnSave.Enabled = false;
                //string DietRequestMainID = updateDietRequsitionMaint();

                //if (DietRequestMainID == "")
                //{
                //    return;
                //}    

                ArrayList coll1 = new ArrayList();
                ArrayList coll2 = new ArrayList();
                StringBuilder objXML1 = new StringBuilder();

                Hashtable Hsout = new Hashtable();
                StringBuilder objXML = new StringBuilder();
                StringBuilder objdietxml = new StringBuilder();
                ArrayList coll = new ArrayList();
                string DietRequestID = "";
                string Remarks = "";
                string Diagnosis = "";



                BaseC.clsEMR objupdateEmrDiet = new BaseC.clsEMR(sConString);

                Remarks = txtRemakrs.Text;
                Diagnosis = txtdiagnosis.Text;

                int isblank = 0;
                if (ddlDietTypeCategory.Enabled)
                {
                    if (ddlDietTypeCategory.SelectedValue == "0")
                    {
                        lblMessage.Text = "Please Select Diet Type";
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        btnSave.Enabled = true;
                        return;
                    }
                }
                if (ddlModeofFeeding.Enabled)
                {
                    if (ddlModeofFeeding.SelectedIndex == 0)
                    {
                        if (ddlNPO.SelectedIndex.Equals(0))
                        {
                            lblMessage.Text = "Please Select Mode of Feeding";
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            btnSave.Enabled = true;
                            return;
                        }
                    }
                }
                if (txtdiagnosis.Enabled)
                {

                    if (common.myInt(txtdiagnosis.Text.Length).Equals(0))
                    {
                        lblMessage.Text = "Please Enter Diagnosis";
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        btnSave.Enabled = true;
                        return;
                    }

                }
                //   string fdprecaution = "";
                //Added on 08-11-2016 start Jogender Singh

                string comma = ",";


                string precaution = "";
                string DietrequestFoodhabit = "";
                string Diet = string.Empty;
                string DietList = string.Empty;


                if (gvPrecaution.Enabled)
                {
                    for (int count = 0; count < gvPrecaution.Items.Count; count++)
                    {
                        if (((CheckBox)gvPrecaution.Items[count].FindControl("chkDepartment")).Checked)
                        {
                            precaution = precaution + ((Label)gvPrecaution.Items[count].FindControl("lblId")).Text.ToString() + ",";
                        }
                    }
                }
                if ((precaution).Length > 0)
                    precaution = precaution.Remove((precaution).Length - 1, 1);
                //Code By Jogedner Singh 09-11-2016
                if (GvFoodHabit.Enabled)
                {
                    for (int countfood = 0; countfood < GvFoodHabit.Items.Count; countfood++)
                    {
                        if (((CheckBox)GvFoodHabit.Items[countfood].FindControl("chkDepartment")).Checked)
                        {
                            DietrequestFoodhabit = DietrequestFoodhabit + ((Label)GvFoodHabit.Items[countfood].FindControl("lblId")).Text.ToString() + ",";
                        }
                    }

                    if ((DietrequestFoodhabit).Length > 0)
                        DietrequestFoodhabit = DietrequestFoodhabit.Remove((DietrequestFoodhabit).Length - 1, 1);
                }

                if (GvDietTypeSubCategoryDetail.Enabled)
                {

                    for (int countfood = 0; countfood < GvDietTypeSubCategoryDetail.Items.Count; countfood++)
                    {
                        if (((CheckBox)GvDietTypeSubCategoryDetail.Items[countfood].FindControl("chkDepartment")).Checked)
                        {
                            //objXML1.Append("<Table1><c1>");
                            //objXML1.Append(common.myInt(((Label)GvDietTypeSubCategoryDetail.Items[countfood].FindControl("lblId")).Text.ToString()));
                            //objXML1.Append("</c1></Table1>");

                            Diet = Diet + ((Label)GvDietTypeSubCategoryDetail.Items[countfood].FindControl("lblId")).Text.ToString() + ",";
                        }

                    }
                    if ((Diet).Length > 0)
                        Diet = Diet.Remove((Diet).Length - 1, 1);
                }

                if (gvDietList.Enabled)
                {
                    for (int countfood = 0; countfood < gvDietList.Items.Count; countfood++)
                    {
                        if (((CheckBox)gvDietList.Items[countfood].FindControl("chkDepartment")).Checked)
                        {
                            DietList = DietList + ((Label)gvDietList.Items[countfood].FindControl("lblId")).Text.ToString() + ",";
                            //}


                            // precaution= ((Label)gvPrecaution.Items[count].FindControl("lblName")).Text.ToString() + ",";
                        }
                    }
                    if ((DietList).Length > 0)
                        DietList = DietList.Remove((DietList).Length - 1, 1);
                }
                if (gvDiet.Enabled)
                {
                    foreach (GridDataItem item in gvDiet.Items)
                    {
                        Label lblDietID = (Label)item.FindControl("lblId");
                        Label lblDietName = (Label)item.FindControl("lblDietName");
                        TextBox txtDietValue = (TextBox)item.FindControl("txtDietValue");


                        //added on 04-11-2016 by Jogender singh

                        if (txtDietValue.Text.Trim().Length > 0)
                        {
                            objXML.Append("<Table1><c1>");
                            objXML.Append(common.myInt(lblDietID.Text));
                            objXML.Append("</c1><c2>");
                            objXML.Append(common.myStr(txtDietValue.Text));
                            objXML.Append("</c2></Table1>");
                            isblank = 1;
                        }
                    }
                }

                DietRequestID = common.myStr(ViewState["DietRequestID"]);

                string DietRequestMainID = updateDietRequsitionMaint();
                if (DietRequestMainID != "")
                {
                    Hsout = objupdateEmrDiet.UpdateEMRDiet(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRegId.Value), common.myInt(hdnEncounterId.Value), common.myInt(DietRequestID), objXML.ToString(),
                        common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                        Remarks, Diagnosis, precaution, DietrequestFoodhabit, common.myInt(ddlDietTypeCategory.SelectedValue), common.myInt(ddlNPO.SelectedValue), Diet, DietList, common.myInt(ddlModeofFeeding.SelectedValue), common.myInt(ddlInternational.SelectedValue), false);

                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = Hsout["@chvErrorStatus"].ToString();
                    string EMRDietID = Hsout["@chvEMRDIETID"].ToString();
                    UpdateDietRequisitionMain(common.myInt(DietRequestMainID), common.myInt(EMRDietID));
                    BindGridPrecauationFoodHabit();
                    BindDietDtails();
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Oops! Error While Saving, Please Contact to Support!.";

                    //Added on 11-08-2014 End Naushad
                }
            }
            catch (Exception Ex)
            {
                btnSave.Enabled = true;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;
                objException.HandleException(Ex);
            }
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Not authorized to edit.";
        }
    }

    //Added on 27-08-2014 Start Naushad

    public int UpdateDietRequisitionMain(int DietRequestID, int EmrDietMainID)
    {

        BaseC.Diet objUpdateDietRequistionMain = new BaseC.Diet(sConString);
        int i = objUpdateDietRequistionMain.UpdateDietRequisitionMain(DietRequestID, EmrDietMainID);
        return i;

    }


    //Added on 27-08-2014 End Naushad





    //Added on 22-08-2014 Start  Naushad


    private DataTable BindGetDietDetailDefault_ForEMR()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        DataTable DtDefaultDiet = new DataTable();
        Hashtable hstInput = new Hashtable();
        try
        {

            hstInput.Add("@HospId", common.myInt(Session["HospitalLocationId"]));
            StringBuilder sbSQL = new StringBuilder();
            // sbSQL.Append("select DietID,DM.DietSlot from HospitalSetup HS INNER JOIN DietMaster DM ON DM.DietID=HS.Value   where   Flag='DietMasterDefaultItemID'");  

            //sbSQL.Append("select DietID,DM.DietSlot from HospitalSetup HS INNER JOIN DietMaster DM ON DM.DietID=HS.Value   where    HospitalLocationId="+common.myInt(Session["HospitalLocationID"]) +" and facilityId="+common.myInt(Session["FacilityId"]) +" and  Flag='DietMasterDefaultItemID'");  

            sbSQL.Append("select top 1 dts.DietDetailId 'DietID',dts.DietId 'DietSlot' from DietTypeCategory dt inner join DietTypeSubCategory dts on dt. DietId=dts.DietId where dt.DietId=" + ddlDietTypeCategory.SelectedValue);

            ds = objDl.FillDataSet(CommandType.Text, sbSQL.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DtDefaultDiet = ds.Tables[0];

            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "No Sub Category is defined for selected Diet Category ";
                //return DtDefaultDiet;
            }



            DtDefaultDiet = ds.Tables[0];
            return DtDefaultDiet;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return DtDefaultDiet;


        }
        finally
        {

            objDl = null;
            DtDefaultDiet.Dispose();
            ds.Dispose();
        }


    }

    public string updateDietRequsitionMaint()
    {
        try
        {
            lblmsg.Text = "";

            DataTable DtDietDeteail = new DataTable();
            DtDietDeteail = BindGetDietDetailDefault_ForEMR();

            if (DtDietDeteail.Rows.Count == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "No Sub Category is defined for selected Diet Category";
                //return "";
            }

            ArrayList coll1 = new ArrayList();
            ArrayList coll2 = new ArrayList();
            StringBuilder objXML1 = new StringBuilder();
            //Added new code for saving the diet properly start
            string DietList = string.Empty;

            if (GvDietTypeSubCategoryDetail.Enabled)
            {

                for (int countfood = 0; countfood < GvDietTypeSubCategoryDetail.Items.Count; countfood++)
                {
                    if (((CheckBox)GvDietTypeSubCategoryDetail.Items[countfood].FindControl("chkDepartment")).Checked)
                    {
                        objXML1.Append("<Table1><c1>");
                        objXML1.Append(common.myInt(((Label)GvDietTypeSubCategoryDetail.Items[countfood].FindControl("lblId")).Text.ToString()));
                        objXML1.Append("</c1></Table1>");
                    }

                }
            }
            //for (int countfood = 0; countfood < gvDietList.Items.Count; countfood++)
            //{
            //    if (((CheckBox)gvDietList.Items[countfood].FindControl("chkDepartment")).Checked)
            //    {
            //        //DietList = DietList + ((Label)gvDietList.Items[countfood].FindControl("lblId")).Text.ToString() + ",";
            //        objXML1.Append("<Table1><c1>");
            //        objXML1.Append(common.myInt(((Label)gvDietList.Items[countfood].FindControl("lblId")).Text.ToString()));
            //        objXML1.Append("</c1></Table1>");
            //    }
            //}
            //if ((DietList).Length > 0)
            //    DietList = DietList.Remove((DietList).Length - 1, 1);

            //Added new code for saving the diet properly start

            //if (DtDietDeteail.Rows.Count > 0)
            //{
            //    objXML1.Append("<Table1><c1>");
            //    objXML1.Append(common.myInt(DtDietDeteail.Rows[0]["DietId"]));
            //    objXML1.Append("</c1></Table1>");
            //}

            BaseC.Diet Objdt = new BaseC.Diet(sConString);

            Hashtable Hsout = Objdt.DietRequsition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRegId.Value), common.myInt(hdnEncounterId.Value), common.myInt(ddlDietTypeCategory.SelectedValue), common.myBool(0), "", "", "", common.myInt(0),
                common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                common.myStr(objXML1.ToString())
                        , common.myBool(0), "", "", common.myInt(0), common.myInt("0"), common.myBool(0));

            string DietMainID = common.myStr(Hsout["@chrintDietMainID"]);
            //string DietMainID = string.Empty;
            return DietMainID;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
            return "";

        }
    }

    //Added on 22-08-2014 End Naushad

    protected void lnkPreviousDiet_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/Diet/PatientPreviousDiet.aspx?EncId=" + common.myInt(hdnEncounterId.Value);
        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 780;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    //Added on 11-08-2014 Start  By Naushad Ali

    protected void gvDietDetail_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        BaseC.clsEMR objbc = new BaseC.clsEMR(sConString);
        DataTable dt = new DataTable();
        try
        {
            Label lblDietRequestId = (Label)e.Item.FindControl("lblId");

            LinkButton lbtnEdit = (LinkButton)e.Item.FindControl("lbtnEdit");


            ViewState["DietRequestID"] = common.myInt(lblDietRequestId.Text);
            if (e.CommandName == "Select")
            {
                if (common.myStr(Session["IsAdminGroup"]) != "True")
                {
                    if (!common.myBool(ViewState["IsAllowEdit"]))
                    {
                        Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                        return;
                    }
                }
                dt = objbc.getEMRDietDetail(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(hdnRegId.Value), common.myInt(hdnEncounterId.Value), common.myInt(lblDietRequestId.Text));

                if (dt.Rows.Count > 0)
                {
                    gvDiet.DataSource = dt;
                    gvDiet.DataBind();
                    txtRemakrs.Text = common.myStr(dt.Rows[0]["Remarks"]);

                    //Code By Jogender
                    txtdiagnosis.Text = common.myStr(dt.Rows[0]["Diagnosis"]);
                    ddlDietTypeCategory.SelectedValue = common.myStr(dt.Rows[0]["DietCatergoryID"]);

                }
                if (lbtnEdit.Text.Equals("Edit"))
                {
                    ViewState["VisibleType"] = "Edit";
                }
                else
                {
                    ViewState["VisibleType"] = "Show";
                }
            }

            if (e.CommandName == "Delete")
            {
                if (!common.myBool(ViewState["IsAllowCancel"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);

                    BindDietDtails();

                    return;
                }

                //int result = objbc.DeleteEMRDiet(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRegId.Value), common.myInt(hdnEncounterId.Value), common.myInt(lblDietRequestId.Text), common.myInt(Session["UserID"]));
                ViewState["DietRequestID"] = common.myInt(lblDietRequestId.Text);
                Div1.Visible = true;
                BindDietDtails();
            }



            Label lblAcknowledgeBy = (Label)e.Item.FindControl("lblAcknowledgeBy");
            Label lblAcktime = (Label)e.Item.FindControl("lblAcktime");
            HiddenField hdndietorderstatus = (HiddenField)e.Item.FindControl("hdndietorderstatus");
            Label lblStatus = (Label)e.Item.FindControl("lblStatus");


            if (e.CommandName == "Cancel")
            {
                if (lblAcknowledgeBy.Text != "" && lblAcktime.Text != "" && hdndietorderstatus.Value != "0")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Order is Acknowledge !";
                    BindDietDtails();
                    return;
                }

                if (lblAcknowledgeBy.Text == "" && lblAcktime.Text == "" && hdndietorderstatus.Value == "0" && lblStatus.Text == "InActive")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Order is already cancel !";
                    BindDietDtails();
                    return;
                }

                if (lblAcknowledgeBy.Text == "" && lblAcktime.Text == "" && hdndietorderstatus.Value != "0")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Diet order status is not UnAcknowledge !";
                    BindDietDtails();
                    return;
                }

                if (lblAcknowledgeBy.Text == "" && lblAcktime.Text == "" && hdndietorderstatus.Value == "0")
                {
                    string result = objbc.CancelEMRDiet(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRegId.Value), common.myInt(hdnEncounterId.Value), common.myInt(lblDietRequestId.Text), common.myInt(Session["UserID"]));
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = result;
                    ViewState["DietRequestID"] = common.myInt(lblDietRequestId.Text);
                    BindDietDtails();
                }
                BindDietDtails();
            }
            //Finish
        }
        catch (Exception ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }

    void BindBlankGridGvDiet()
    {
        BaseC.clsEMR ObjDietMaster = new BaseC.clsEMR(sConString);
        DataTable objdtDietMaster = new DataTable();
        try
        {
            objdtDietMaster = ObjDietMaster.getEMRDietMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (objdtDietMaster.Rows.Count > 0)
            {
                gvDiet.DataSource = objdtDietMaster;
                gvDiet.DataBind();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objdtDietMaster = null;
        }
    }


    private void BindDietTypeCategoryCombo()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hstInput = new Hashtable();
        hstInput.Add("@HospId", common.myInt(Session["HospitalLocationId"]));
        StringBuilder sbSQL = new StringBuilder();
        sbSQL.Append("SELECT DISTINCT DietId, DietName, DietStatus, DietType FROM DietTypeCategory WITH(NOLOCK) WHERE DietStatus=1 AND HospitalLocationId = @HospId ORDER BY DietName");
        ds = objDl.FillDataSet(CommandType.Text, sbSQL.ToString(), hstInput);

        ddlDietTypeCategory.DataSource = ds;
        ddlDietTypeCategory.DataTextField = "DietName";
        ddlDietTypeCategory.DataValueField = "DietId";
        ddlDietTypeCategory.DataBind();

        ddlDietTypeCategory.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
        ddlDietTypeCategory.SelectedIndex = 0;
    }

    private void BindModeofFeedingCombo()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hstInput = new Hashtable();
        hstInput.Add("@HospId", common.myInt(Session["HospitalLocationId"]));
        StringBuilder sbSQL = new StringBuilder();
        sbSQL.Append("select ID,FeedingName from DietModeofFeeding");
        ds = objDl.FillDataSet(CommandType.Text, sbSQL.ToString(), hstInput);

        ddlModeofFeeding.DataSource = ds;
        ddlModeofFeeding.DataTextField = "FeedingName";
        ddlModeofFeeding.DataValueField = "ID";
        ddlModeofFeeding.DataBind();

        ddlModeofFeeding.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
        ddlModeofFeeding.SelectedIndex = 0;
    }
    private void BindNPOCombo()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hstInput = new Hashtable();
        hstInput.Add("@HospId", common.myInt(Session["HospitalLocationId"]));
        StringBuilder sbSQL = new StringBuilder();
        sbSQL.Append("select id,NPOName from DietNPO");
        ds = objDl.FillDataSet(CommandType.Text, sbSQL.ToString(), hstInput);

        ddlNPO.DataSource = ds;
        ddlNPO.DataTextField = "NPOName";
        ddlNPO.DataValueField = "Id";
        ddlNPO.DataBind();

        ddlNPO.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
        ddlNPO.SelectedIndex = 0;
    }
    private void BindInternationalCombo()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hstInput = new Hashtable();
        hstInput.Add("@HospId", common.myInt(Session["HospitalLocationId"]));
        StringBuilder sbSQL = new StringBuilder();
        sbSQL.Append("select id,MealName from DietInternationalMealType");
        ds = objDl.FillDataSet(CommandType.Text, sbSQL.ToString(), hstInput);

        ddlInternational.DataSource = ds;
        ddlInternational.DataTextField = "MealName";
        ddlInternational.DataValueField = "ID";
        ddlInternational.DataBind();

        ddlInternational.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
        ddlInternational.SelectedIndex = 0;
    }
    protected void ddlNPO_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNPO.SelectedIndex != 0)
        {
            //ddlDietTypeCategory.SelectedValue = "144";
            ddlModeofFeeding.SelectedIndex = 0;
            ddlModeofFeeding.Enabled = false;
            //ddlDietTypeCategory.SelectedIndex = 0;
            //ddlDietTypeCategory.Enabled = false;
            gvPrecaution.Enabled = false;
            GvFoodHabit.Enabled = false;
            gvDietList.Enabled = false;
            GvDietTypeSubCategoryDetail.Enabled = false;
            ClearGridSelection();
            /*
            ddltypecategory
ddlModeoffeeding
gvprecaution
gvfoodhabi
gvdietlist
*/
        }
        else
        {
            ddlModeofFeeding.SelectedIndex = 0;
            ddlModeofFeeding.Enabled = true;
            //ddlDietTypeCategory.SelectedIndex = 0;
            ddlDietTypeCategory.Enabled = true;
            gvPrecaution.Enabled = true;
            GvFoodHabit.Enabled = true;
            gvDietList.Enabled = true;
            GvDietTypeSubCategoryDetail.Enabled = true;
            ddlModeofFeeding.Enabled = true;
        }
    }

    public void ClearGridSelection()
    {
        string comma = ",";
        string precaution = "";
        string DietrequestFoodhabit = "";
        string Diet = string.Empty;
        string DietList = string.Empty;

        if (gvPrecaution.Enabled == false)
        {
            for (int count = 0; count < gvPrecaution.Items.Count; count++)
            {
                if (((CheckBox)gvPrecaution.Items[count].FindControl("chkDepartment")).Checked)
                {
                    ((CheckBox)gvPrecaution.Items[count].FindControl("chkDepartment")).Checked = false;
                }
            }
        }

        if (GvFoodHabit.Enabled == false)
        {
            for (int countfood = 0; countfood < GvFoodHabit.Items.Count; countfood++)
            {
                if (((CheckBox)GvFoodHabit.Items[countfood].FindControl("chkDepartment")).Checked)
                {
                    ((CheckBox)GvFoodHabit.Items[countfood].FindControl("chkDepartment")).Checked = false;
                }
            }
        }

        if (GvDietTypeSubCategoryDetail.Enabled == false)
        {

            for (int countfood = 0; countfood < GvDietTypeSubCategoryDetail.Items.Count; countfood++)
            {
                if (((CheckBox)GvDietTypeSubCategoryDetail.Items[countfood].FindControl("chkDepartment")).Checked)
                {
                    ((CheckBox)GvDietTypeSubCategoryDetail.Items[countfood].FindControl("chkDepartment")).Checked = false;
                }

            }
        }

        if (gvDietList.Enabled == false)
        {
            for (int countfood = 0; countfood < gvDietList.Items.Count; countfood++)
            {
                if (((CheckBox)gvDietList.Items[countfood].FindControl("chkDepartment")).Checked)
                {
                    ((CheckBox)gvDietList.Items[countfood].FindControl("chkDepartment")).Checked = false;
                }
            }
        }

    }

    protected void ddlDietTypeCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlDietTypeCategory.SelectedItem.Text.Equals("NPO"))
        //{
        //    ddlModeofFeeding.Enabled = false;

        //}
        //else
        //{
        Session["NPOFlagBased"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
            common.myInt(Session["FacilityId"]), "NPOFlagbased", sConString);
        if (common.myStr(Session["NPOFlagBased"]) == "Y")
        {
            if (ddlDietTypeCategory.Text == "NPO")
            {

                strnpo.Visible = true;
            }
            else
            {
                strnpo.Visible = false;
            }
        }

        BindGridgvDietList(1);


        DataSet dspre = new DataSet();
        //try
        //{
        dspre = GetDietTypeSubCategoryDetailList(common.myInt(ddlDietTypeCategory.SelectedValue));
        if (dspre.Tables.Count > 0)
        {
            GvDietTypeSubCategoryDetail.DataSource = dspre;
            GvDietTypeSubCategoryDetail.DataBind();
            GvDietTypeSubCategoryDetail.Visible = true;
        }
        //}
    }
    protected void gvDietDetail_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;

                if (item != null)
                {
                    HiddenField hdnEncodedById = (HiddenField)e.Item.FindControl("hdnEncodedById");
                    LinkButton lbtnEdit = (LinkButton)e.Item.FindControl("lbtnEdit");
                    ImageButton imbtndelete = (ImageButton)e.Item.FindControl("imbtndelete");
                    ImageButton imbtnCancel = (ImageButton)e.Item.FindControl("imbtnCancel");

                    if (hdnEncodedById.Value == "")
                    {
                        imbtnCancel.Visible = false;
                    }
                    if (common.myInt(hdnEncodedById.Value) > 0)
                    {
                        if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                        {
                            //lbtnEdit.Visible = false;
                            //imbtndelete.Visible = false;
                            imbtndelete.Visible = false;
                            //lbtnEdit.Visible = true;
                            lbtnEdit.Text = "Show";

                        }
                        else
                        {
                            lbtnEdit.Text = "Edit";
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
    public DataSet GetDietTypeSubCategoryDetailList(int intDietID)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        HshIn.Add("@intDietID", intDietID);

        ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietTypeSubCategoryDetailList", HshIn);
        return ds;
    }
    public DataSet GetDietCurrentDietDetails(int intDietID)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        HshIn.Add("@intDietID", intDietID);

        ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietCurrentDietDetails", HshIn);
        return ds;
    }
    void BindGridPrecauationFoodHabit()
    {
        BaseC.Diet Objdt = new BaseC.Diet(sConString);
        DataSet dspre = new DataSet();
        try
        {
            dspre = Objdt.GetPrecaution();
            if (dspre.Tables.Count > 0)
            {
                gvPrecaution.DataSource = dspre;
                gvPrecaution.DataBind();
            }
            dspre.Clear();
            dspre = new DataSet();
            dspre = Objdt.GetFoodHabit();
            if (dspre.Tables.Count > 0)
            {
                GvFoodHabit.DataSource = dspre;
                GvFoodHabit.DataBind();
            }
            dspre.Clear();
        }
        catch (Exception Ex)
        {

            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            Objdt = null;
            dspre = null;
        }
    }

    //void BindGridDietTypeSubCategoryDetail()
    //{
    //    BaseC.Diet Objdt = new BaseC.Diet(sConString);
    //    DataSet dspre = new DataSet();
    //    try
    //    {
    //        dspre = Objdt.GetDietTypeSubCategoryDetail();
    //        if (dspre.Tables.Count > 0)
    //        {
    //            gvPrecaution.DataSource = dspre;
    //            gvPrecaution.DataBind();
    //        }
    //        dspre.Clear();
    //        dspre = new DataSet();
    //        dspre = Objdt.GetFoodHabit();
    //        if (dspre.Tables.Count > 0)
    //        {
    //            GvFoodHabit.DataSource = dspre;
    //            GvFoodHabit.DataBind();
    //        }
    //        dspre.Clear();
    //    }
    //    catch (Exception Ex)
    //    {

    //        lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblmsg.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        Objdt = null;
    //        dspre = null;
    //    }
    //}
    void BindGridgvDietList(int DietTypeID)
    {
        BaseC.Diet Objdt = new BaseC.Diet(sConString);
        DataSet dspre = new DataSet();
        try
        {
            dspre = GetDietCurrentDietDetails(1);
            if (dspre.Tables.Count > 0)
            {
                gvDietList.DataSource = dspre;
                gvDietList.DataBind();
                gvDietList.Visible = true;
            }
        }
        catch (Exception Ex)
        {

            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            Objdt = null;
            dspre = null;
        }
    }
    protected void gvPrecaution_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (e.Item is GridDataItem)
            {

                Label lblId = (Label)e.Item.FindControl("lblId");
                CheckBox chkDepartment = (CheckBox)e.Item.FindControl("chkDepartment");

                if (common.myStr(ViewState["FoodHabit"]) != "")
                {
                    dt = (DataTable)ViewState["FoodHabit"];
                    dt.DefaultView.RowFilter = "IdentityType='FP' AND LinkId=" + common.myInt(lblId.Text);
                    if (dt.DefaultView.Count > 0)
                    {

                        if (common.myInt(lblId.Text) == common.myInt(dt.DefaultView[0]["LinkId"]))
                        {
                            chkDepartment.Checked = true;

                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {

            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dt = null;
        }

    }

    protected void GvFoodHabit_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (e.Item is GridDataItem)
            {

                Label lblId = (Label)e.Item.FindControl("lblId");
                CheckBox chkDepartment = (CheckBox)e.Item.FindControl("chkDepartment");

                if (common.myStr(ViewState["FoodHabit"]) != "")
                {
                    dt = (DataTable)ViewState["FoodHabit"];
                    dt.DefaultView.RowFilter = "IdentityType='FH' AND LinkId=" + common.myInt(lblId.Text);
                    if (dt.DefaultView.Count > 0)
                    {
                        if (common.myInt(lblId.Text) == common.myInt(dt.DefaultView[0]["LinkId"]))
                        {
                            chkDepartment.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {

            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dt = null;
        }
    }
    protected void gvDietList_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (e.Item is GridDataItem)
            {

                Label lblId = (Label)e.Item.FindControl("lblId");
                CheckBox chkDepartment = (CheckBox)e.Item.FindControl("chkDepartment");

                if (common.myStr(ViewState["FoodHabit"]) != "")
                {
                    dt = (DataTable)ViewState["FoodHabit"];
                    dt.DefaultView.RowFilter = "IdentityType='FP' AND LinkId=" + common.myInt(lblId.Text);
                    if (dt.DefaultView.Count > 0)
                    {

                        if (common.myInt(lblId.Text) == common.myInt(dt.DefaultView[0]["LinkId"]))
                        {
                            chkDepartment.Checked = true;

                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {

            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dt = null;
        }

    }
    #region Transaction password validation
    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP";
        RadWindowForNew.Height = 120;
        RadWindowForNew.Width = 340;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
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

            SaveData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
        }
    }


    #endregion

    protected void imgAllergyAlert_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);

            Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                   common.myStr(Session["RegistrationID"]), common.myStr(Session["EncounterId"]), common.myInt(Session["UserId"]), 0);
            //}
            RadWindowForNew.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&FromEMR=1&EId=" + common.myStr(Session["EncounterId"])
                + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"])
                + "&PAG=" + common.myStr(Session["AgeGender"]) + "" + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&SepPat=Y";
            RadWindowForNew.Height = 400;
            RadWindowForNew.Width = 1050;
            RadWindowForNew.Top = 20;
            RadWindowForNew.Left = 20;
            //RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;

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

            RadWindowForNew.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&FromEMR=1&EId=" + common.myStr(Session["EncounterId"])
               + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"])
               + "&PAG=" + common.myStr(Session["AgeGender"]) + "" + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&SepPat=Y";


            RadWindowForNew.Height = 400;
            RadWindowForNew.Width = 1050;
            RadWindowForNew.Top = 20;
            RadWindowForNew.Left = 20;
            //RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }

}
