using System;
using System.Collections;
using System.Collections.Generic;
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

public partial class EMR_Immunization_ImmunizationTrackingDialog : System.Web.UI.Page
{
    #region Page level variable declration section

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private Hashtable hshInput;
    BaseC.ParseData Parse = new BaseC.ParseData();
    Hashtable hsNewPage = new Hashtable();
    DL_Funs fun = new DL_Funs();
    static DataView DvPatientDiagnosis = new DataView();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        //string a=  common.myStr(RadExpiryDate.SelectedDate);
        //if (Session["encounterid"] == null)
        //{
        //    Response.Redirect("/Default.aspx?RegNo=0", false);
        //}
        if (!IsPostBack)
        {
            BindFacility();
            ImmName.Text = Request.QueryString["ImmName"];
            BindProvider();
            RadGivenDatetime.SelectedDate = DateTime.Now;
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
            txtComments.Attributes.Add("onkeyup", "javascript:maxLength('" + txtComments.ClientID + "');");
            BindBrand();
            BindDetails();
            RadGivenDatetime.MaxDate = Convert.ToDateTime(DateTime.Now.AddMinutes(20));
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
            {
                btnSave.Visible = false;
            }
            setMandatoryAndEnableFields();
        }
    }

    private void setMandatoryAndEnableFields()
    {
        BaseC.HospitalSetup hsSetup = new BaseC.HospitalSetup(sConString);
        DataSet dsPage = new DataSet();
        try
        {
            dsPage = hsSetup.GetPageMandatoryAndEnableFields(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "EMR", "ImmunizationDetails");

            if (dsPage.Tables[0].Rows.Count > 0)
            {
                spnStarBatchNo.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "BatchNo");
                txtBatchNo.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "BatchNo");
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            hsSetup = null;
            dsPage.Dispose();
        }
    }

    private bool IsMandatory(DataView dv, string FieldName)
    {
        bool isM = false;
        try
        {
            if (dv != null)
            {
                dv.RowFilter = "FieldName='" + FieldName + "'";
                if (dv.Count > 0)
                {
                    isM = common.myBool(dv.ToTable().Rows[0]["IsMandatoryField"]);
                }
                dv.RowFilter = string.Empty;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return isM;
    }

    private bool IsEnable(DataView dv, string FieldName)
    {
        bool isE = false;
        try
        {
            if (dv != null)
            {
                dv.RowFilter = "FieldName='" + FieldName + "'";
                if (dv.Count > 0)
                {
                    isE = common.myBool(dv.ToTable().Rows[0]["IsEnableField"]);
                }
                dv.RowFilter = string.Empty;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return isE;
    }

    protected void BindBrand()
    {

        hshInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hshInput.Add("@ImmunizationId", common.myInt(Request.QueryString["ImmId"]));
        DataSet objDs = dl.FillDataSet(CommandType.Text, "SELECT DISTINCT i.BrandId, b.ItemBrandName FROM EMRImmunizationBrands i WITH(NOLOCK) INNER JOIN phritembrandmaster b WITH(NOLOCK) ON i.BrandId = b.ItemBrandId WHERE i.immunizationId = @ImmunizationId AND i.active = 1 AND b.active = 1 ORDER BY b.ItemBrandName ", hshInput);

        ddlBrand.Items.Clear();
        if (objDs.Tables.Count > 0)
        {
            if (objDs.Tables[0].Rows.Count > 0)
            {
                ddlBrand.DataSource = objDs;
                ddlBrand.DataValueField = "BrandId";
                ddlBrand.DataTextField = "ItemBrandName";
                ddlBrand.DataBind();
            }
        }

        ddlBrand.Items.Insert(0, new RadComboBoxItem("Select", "0"));

        if (common.myStr(Request.QueryString["Bid"]) != "")
            ddlBrand.SelectedValue = common.myStr(Request.QueryString["Bid"]);
        else
            ddlBrand.SelectedIndex = 0;
    }


    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(RadGivenDatetime.SelectedDate.Value.ToString());
        sb.Remove(RadGivenDatetime.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(RadGivenDatetime.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
        RadGivenDatetime.SelectedDate = Convert.ToDateTime(sb.ToString());
    }
    protected void BindFacility()
    {
        try
        {
            DataSet ds = new DataSet();
            BaseC.EMRImmunization immu = new BaseC.EMRImmunization(sConString);
            Hashtable hshInputs = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInputs.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInputs.Add("@chvFacilityType", "O");
            hshInputs.Add("@intUserId", Session["UserID"]);
            hshInputs.Add("@intGroupId", Session["GroupID"]);

            DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityList", hshInputs);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            ddlFacility.Items.Clear();
            ddlFacility.DataSource = objDs1;
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataBind();
            string FacilityId = immu.GetEncounterFacilityForImmunization(common.myInt(Session["HospitalLocationId"]),common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));
            //string FacilityId = immu.GetEncounterFacilityForImmunization(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt32(Session["RegistrationId"]), 0);
            ddlFacility.SelectedValue = FacilityId;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindProvider()
    {
        try
        {
            DataSet ds = new DataSet();
            BaseC.EMRImmunization immu = new BaseC.EMRImmunization(sConString);
            ds = immu.GetImmunizationGivenBy(Convert.ToInt16(Session["HospitalLocationId"]));
            ddlProviders.Items.Clear();
            ddlProviders.DataSource = ds.Tables[0];
            ddlProviders.DataValueField = "NurseId";
            ddlProviders.DataTextField = "NurseName";
            ddlProviders.DataBind();
            if (common.myInt(Session["EmployeeId"]) > 0)
            {
                ddlProviders.SelectedIndex = ddlProviders.Items.IndexOf(ddlProviders.Items.FindItemByValue(Convert.ToString(Session["EmployeeId"])));
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sExpiryDate;
        try
        {
            if (RadGivenDatetime.SelectedDate == null || RadGivenDatetime.SelectedDate.ToString() == "")
            {
                lblMessage.Text = "Please enter Given Date...";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                Alert.ShowAjaxMsg("Please enter Given Date...", Page.Page);
                return;
            }
            string sGivenDate = Convert.ToDateTime(RadGivenDatetime.SelectedDate).ToString("yyyy/MM/dd hh:mm");

            if (common.myStr(RadExpiryDate.SelectedDate) == string.Empty)
            {
                sExpiryDate = null;
            }
            else
            {
                sExpiryDate = Convert.ToDateTime(RadExpiryDate.SelectedDate).ToString("yyyy/MM/dd");
            }

            //string pageId = common.myStr(Request.QueryString["Mpg"]);
            //pageId = pageId.Substring(1, pageId.Length - 1);

            if (!ChkPatientorGuardian.Checked && !chkVaccineGivenByOutsider.Checked)
            {
                if (spnStarBatchNo.Visible)
                {
                    if (common.myLen(txtBatchNo.Text).Equals(0))
                    {
                        lblMessage.Text = "Please enter Batch No.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        Alert.ShowAjaxMsg("Please enter Batch No...", Page.Page);
                        return;
                    }
                }
            }
            else if (ChkPatientorGuardian.Checked || !chkVaccineGivenByOutsider.Checked)
            {
                sGivenDate = null;
            }

            if (txtComments.Text.Length >= 100)
            {
                Alert.ShowAjaxMsg("Please enter 100 Characters", Page);

                return;
            }

            if (!ChkPatientorGuardian.Checked && !chkVaccineGivenByOutsider.Checked)
            {
                if (common.myInt(ddlBrand.Items.Count) > 1)
                {
                    if (common.myInt(ddlBrand.SelectedValue).Equals(0))
                    {
                        Alert.ShowAjaxMsg("Please Select Immunization Brand!!!", Page);
                        return;
                    }
                }
            }

            BaseC.EMRImmunization immu = new BaseC.EMRImmunization(sConString);

            //string hshOut = immu.SavePatientImmunization(Convert.ToInt16(Request.QueryString["Id"]), Convert.ToInt16(Session["HospitalLocationId"]),
            //    Convert.ToInt16(ddlFacility.SelectedValue), Convert.ToInt16(Request.QueryString["SchId"])
            //    , Convert.ToInt16(Request.QueryString["ImmId"]), Convert.ToInt32(Session["RegistrationID"]),
            //    Convert.ToInt32(Session["EncounterId"]), 
            //    sGivenDate, ddlProviders.SelectedValue == "" ? 0 : Convert.ToInt32(ddlProviders.SelectedValue),
            //    txtBatchNo.Text, ChkPatientorGuardian.Checked, chkVaccineGivenByOutsider.Checked,
            //    txtComments.Text, Convert.ToInt32(Session["UserId"]), 0, common.myInt(ddlBrand.SelectedValue), sExpiryDate);

            string hshOut = immu.SavePatientImmunization(Convert.ToInt16(Request.QueryString["Id"]), Convert.ToInt16(Session["HospitalLocationId"]),
               Convert.ToInt16(ddlFacility.SelectedValue), Convert.ToInt16(Request.QueryString["SchId"])
               , Convert.ToInt16(Request.QueryString["ImmId"]), common.myInt(Request.QueryString["RegId"]),
               common.myInt(Session["EncounterId"]),
               sGivenDate, ddlProviders.SelectedValue == "" ? 0 : Convert.ToInt32(ddlProviders.SelectedValue),
               txtBatchNo.Text, ChkPatientorGuardian.Checked, chkVaccineGivenByOutsider.Checked,
               txtComments.Text, Convert.ToInt32(Session["UserId"]), 0, common.myInt(ddlBrand.SelectedValue), sExpiryDate);

            lblMessage.Text = hshOut;
            lblMessage.ForeColor = System.Drawing.Color.Green;
            //if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
            //{
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
            return;
            // }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void BindDetails()
    {
        try
        {
            DataSet objDs1 = new DataSet();
            BaseC.EMRImmunization immu = new BaseC.EMRImmunization(sConString);
            //objDs1 = immu.GetPatientImmunization(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["EncounterId"]), Convert.ToInt32(Request.QueryString["SchId"]), Convert.ToInt32(Request.QueryString["ImmId"]));
            objDs1 = immu.GetPatientImmunization(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt32(Session["RegistrationID"]), 0, Convert.ToInt32(Request.QueryString["SchId"]), Convert.ToInt32(Request.QueryString["ImmId"]));

            if (objDs1.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(objDs1.Tables[0].Rows[0]["RejectedByPatient"]))
                {
                    ChkPatientorGuardian.Checked = Convert.ToBoolean(objDs1.Tables[0].Rows[0]["RejectedByPatient"].ToString());
                    ddlFacility.SelectedValue = objDs1.Tables[0].Rows[0]["FacilityId"].ToString();
                    lblid.Text = objDs1.Tables[0].Rows[0]["Id"].ToString();
                    txtComments.Text = objDs1.Tables[0].Rows[0]["Remarks"].ToString();
                    ltrlLot.Visible = false;
                    txtBatchNo.Visible = false;
                    ltrlGivenBy.Visible = false;
                    ddlProviders.Visible = false;
                    ltrlGivenDate.Visible = false;
                    RadGivenDatetime.Visible = false;
                    RadComboBox1.Visible = false;
                    ltDateTime.Visible = false;
                    spnStarBatchNo.Visible = false;
                    ltrlVaccineGivenByOutsider.Visible = false;
                    chkVaccineGivenByOutsider.Visible = false;

                }
                else if (Convert.ToBoolean(objDs1.Tables[0].Rows[0]["VaccineGivenByOutsider"]) == true)
                {
                    chkVaccineGivenByOutsider.Checked = Convert.ToBoolean(objDs1.Tables[0].Rows[0]["VaccineGivenByOutsider"].ToString());
                    ddlFacility.SelectedValue = objDs1.Tables[0].Rows[0]["FacilityId"].ToString();
                    lblid.Text = objDs1.Tables[0].Rows[0]["Id"].ToString();
                    txtComments.Text = objDs1.Tables[0].Rows[0]["Remarks"].ToString();
                    ltrlLot.Visible = false;
                    txtBatchNo.Visible = false;
                    ltrlGivenBy.Visible = false;
                    ddlProviders.Visible = false;
                    ltrlGivenDate.Visible = false;
                    RadGivenDatetime.Visible = false;
                    RadComboBox1.Visible = false;
                    ltDateTime.Visible = false;
                    spnStarBatchNo.Visible = false;
                    ltrlPatientorGuardian.Visible = false;
                    ChkPatientorGuardian.Visible = false;
                }
                else
                {
                    ltrlLot.Visible = true;
                    txtBatchNo.Visible = true;
                    ltrlGivenBy.Visible = true;
                    ddlProviders.Visible = true;
                    ltrlGivenDate.Visible = true;
                    RadGivenDatetime.Visible = true;
                    RadComboBox1.Visible = true;
                    ltDateTime.Visible = true;
                    spnStarBatchNo.Visible = true;
                    lblid.Text = objDs1.Tables[0].Rows[0]["Id"].ToString();
                    txtBatchNo.Text = objDs1.Tables[0].Rows[0]["LotNo"].ToString();
                    ddlFacility.SelectedValue = objDs1.Tables[0].Rows[0]["FacilityId"].ToString();
                    txtComments.Text = objDs1.Tables[0].Rows[0]["Remarks"].ToString();
                    if (objDs1.Tables[0].Rows[0]["GivenDateTime"].ToString() == "")
                    {
                        RadGivenDatetime.SelectedDate = null;
                    }
                    else
                    {
                        RadGivenDatetime.SelectedDate = Convert.ToDateTime(objDs1.Tables[0].Rows[0]["GivenDateTime"].ToString());
                    }

                    if (objDs1.Tables[0].Rows[0]["ExpiryDate"].ToString() == "")
                    {
                        RadExpiryDate.SelectedDate = null;
                    }
                    else
                    {

                        //dtpDOB.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["DateofBirth"].ToString());
                        //Convert.ToDateTime(RadGivenDatetime.SelectedDate).ToString("yyyy/MM/dd")

                        RadExpiryDate.SelectedDate = Convert.ToDateTime(objDs1.Tables[0].Rows[0]["ExpiryDate"].ToString());
                    }
                    ChkPatientorGuardian.Checked = Convert.ToBoolean(objDs1.Tables[0].Rows[0]["RejectedByPatient"].ToString());
                    ddlProviders.SelectedValue = objDs1.Tables[0].Rows[0]["GivenBy"].ToString();
                }
                btnSave.Text = "Update";
            }
            else
            {
                btnSave.Text = "Save";
                clear();
                lblid.Text = "0";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void clear()
    {
        lblid.Text = "";
        txtComments.Text = "";
        ChkPatientorGuardian.Checked = false;
    }
    protected void ChkPatientorGuardian_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkPatientorGuardian.Checked == true)
        {
            ltrlLot.Visible = false;
            txtBatchNo.Visible = false;
            ltrlGivenBy.Visible = false;
            ddlProviders.Visible = false;
            ltrlGivenDate.Visible = false;
            RadGivenDatetime.Visible = false;
            RadComboBox1.Visible = false;
            ltDateTime.Visible = false;
            spnStarBatchNo.Visible = false;
            ltrlVaccineGivenByOutsider.Visible = false;
            chkVaccineGivenByOutsider.Visible = false;
            ltrlBrand.Visible = false;
            ddlBrand.Visible = false;
            ltrExpiryDate.Visible = false;
            RadExpiryDate.Visible = false;
        }
        else
        {
            ltrlLot.Visible = true;
            txtBatchNo.Visible = true;
            ltrlGivenBy.Visible = true;
            ddlProviders.Visible = true;
            ltrlGivenDate.Visible = true;
            RadGivenDatetime.Visible = true;
            RadComboBox1.Visible = true;
            ltDateTime.Visible = true;
            spnStarBatchNo.Visible = true;
            ltrlVaccineGivenByOutsider.Visible = true;
            chkVaccineGivenByOutsider.Visible = true;
            ltrlBrand.Visible = true;
            ddlBrand.Visible = true;
            ltrExpiryDate.Visible = true;
            RadExpiryDate.Visible = true;
        }
    }

    protected void ChkVaccineGivenByOutsider_CheckedChanged(object sender, EventArgs e)
    {
        if (chkVaccineGivenByOutsider.Checked)
        {
            ltrlLot.Visible = false;
            txtBatchNo.Visible = false;
            ltrlGivenBy.Visible = false;
            ddlProviders.Visible = false;
            ltrlGivenDate.Visible = true;
            RadGivenDatetime.Visible = true;
            RadComboBox1.Visible = true;
            ltDateTime.Visible = true;

            spnStarBatchNo.Visible = false;
            ltrlPatientorGuardian.Visible = false;
            ChkPatientorGuardian.Visible = false;

            ltrlBrand.Visible = false;
            ddlBrand.Visible = false;

            ltrExpiryDate.Visible = false;
            RadExpiryDate.Visible = false;
        }
        else
        {
            ltrlLot.Visible = true;
            txtBatchNo.Visible = true;
            ltrlGivenBy.Visible = true;
            ddlProviders.Visible = true;
            ltrlGivenDate.Visible = true;
            RadGivenDatetime.Visible = true;
            RadComboBox1.Visible = true;
            ltDateTime.Visible = true;
            spnStarBatchNo.Visible = true;
            ltrlPatientorGuardian.Visible = true;
            ChkPatientorGuardian.Visible = true;

            ltrlBrand.Visible = true;
            ddlBrand.Visible = true;

            ltrExpiryDate.Visible = true;
            RadExpiryDate.Visible = true;
        }
    }

    protected void ddlBrand_OnSelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();

        try
        {
            txtBatchNo.Text = string.Empty;
            RadExpiryDate.SelectedDate = null;

            if (common.myInt(ddlBrand.SelectedValue) > 0)
            {
                ds = objEMR.getVaccinationStockDetails(common.myInt(Session["FacilityId"]), common.myStr(Session["OPIP"]), common.myInt(ddlBrand.SelectedValue), 0);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtBatchNo.Text = common.myStr(ds.Tables[0].Rows[0]["BatchNo"]);
                    RadExpiryDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpiryDate"]);
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            objEMR = null;
            ds.Dispose();
        }
    }

}
