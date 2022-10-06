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

public partial class EMR_Assessment_PatientMedicationCharges : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bC = new BaseC.ParseData();
    static DataTable DrugDt = new DataTable();
    DataSet ds = new DataSet();
    DataSet objDs;
    string pageId = "";

    public string Billable(string strbil)
    {
        if (strbil.ToUpper() == "TRUE")
        {
            return "Yes";
        }
        else if (strbil.ToUpper() == "FALSE")
        {
            return "";
        }
        return "";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Mpg"] != null)
        {
            pageId = Request.QueryString["Mpg"];
        }
        if (!IsPostBack)
        {
            hdnGridClientId.Value = icd.GridClientId;
            Cache.Remove("SB_Prescription_" + Session["UserId"]);
            ViewState["BTN"] = null;
            ViewState["BTN"] = "FAV";
            SearchDrug("", this, null);
            getCurrentICDCodes();
            BindICDPanel();
            getPatientDrugHistory();
            if (Request.QueryString["Id"] != "")
            {
                int Id = Convert.ToInt32(Request.QueryString["Id"]);
                FillMedicationChargesById(Id);
            }
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dsCompany = new DataSet();
            Hashtable hshin = new Hashtable();
            hshin.Add("@intEncounterId", Session["EncounterId"]);
            hshin.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
            Hashtable hshOutput = new Hashtable();
            hshOutput.Add("intCompanyId", SqlDbType.Int);
            hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "GetPatientCompanyCode", hshin, hshOutput);
            if (hshOutput["intCompanyId"].ToString() != "")
            {
                ViewState["CompanyCode"] = hshOutput["intCompanyId"].ToString();
            }
            else
            {
                ViewState["CompanyCode"] = 0;
            }
        }
    }

    #region All Data Bind Private Method
    private void BindICDPanel()
    {
        try
        {
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
                    string sql = "";
                    sql = "SELECT ICDID, ICDCode, Description FROM ICD9SubDisease  Where  "
                    + "(ICDCode = @Diagnosis and Active = 1)";
                    Hashtable hshIn = new Hashtable();
                    hshIn.Add("@Diagnosis", item.ToString());
                    DataSet dsTemp = new DataSet();
                    dsTemp = objSave.FillDataSet(CommandType.Text, sql, hshIn);
                    drdt["ICDCodes"] = item.ToString();
                    drdt["Description"] = dsTemp.Tables[0].Rows[0]["Description"].ToString();
                    dt.Rows.Add(drdt);
                }
                if (dt.Rows.Count == 1)
                {
                    txtICDCode.Text = dt.Rows[0]["ICDCodes"].ToString();
                }
                else if (dt.Rows.Count > 1)// saten change for multi diagnosis 
                {
                    txtICDCode.Text = ViewState["ICDCodes"].ToString();
                }

                //rptDiagnosis.DataSource = dt;
                //rptDiagnosis.DataBind();
                //cblICDCodes.DataSource = dt;
                //cblICDCodes.DataValueField = "ID";
                //cblICDCodes.DataTextField = "ICDCodes";
                //cblICDCodes.DataBind();
                //imgOk.Attributes.Add("onclick", "javascript:HidePanelOKClick('" + pnlICDCodes.ClientID + "','" + cblICDCodes.ClientID + "','" + txtICDCode.ClientID + "','" + hdnICDCode.ClientID + "')");
                //imgClose.Attributes.Add("onclick", "javascript:HideICDPanel('" + pnlICDCodes.ClientID + "')");

                txtICDCode.Attributes.Add("onclick", "javascript:ShowICDPanel('" + pnlICDCodes.ClientID + "', this )");
                PopUnit.Enabled = true;
            }
            else
            {
                txtICDCode.ReadOnly = true;
                txtICDCode.Attributes.Remove("onclick");
                PopUnit.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion

    #region  Private Method
    private void getCurrentICDCodes()
    {
        try
        {
            if (Session["EncounterID"] != null && Session["registrationId"] != null)
            {
                DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@inyHospitalLocationID", Session["HospitalLocationID"].ToString());
                hshIn.Add("@intRegistrationId", Session["RegistrationID"].ToString());
                hshIn.Add("@intEncounterId", Session["EncounterId"].ToString());

                DataSet ds = objSave.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hshIn);
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
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion

    private void BindBlankGrid()
    {
        try
        {
            DataTable dt = CreateDataTable();

            for (int i = 0; i < 5; i++)
            {
                DataRow dr = dt.NewRow();
                dr["DRUG_ID"] = "";
                dr["GENPRODUCT_ID"] = "";
                dr["DRUG_SYN_ID"] = "";
                dr["DISPLAY_NAME"] = "";
                dt.Rows.Add(dr);
            }
            ViewState["BlankGrid"] = "True";
            gvDrug.DataSource = dt;
            gvDrug.DataBind();
            ViewState["BlankGrid"] = null;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private DataTable CreateDataTable()
    {
        DataTable datatable = new DataTable();
        datatable.Columns.Add("DRUG_ID");
        datatable.Columns.Add("GENPRODUCT_ID");
        datatable.Columns.Add("DRUG_SYN_ID");
        datatable.Columns.Add("DISPLAY_NAME");
        return datatable;
    }

    protected void lnkDiagnosis_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PatientDiagnosisCharges.aspx?ID=0", false);
    }

    protected void rbonamefilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        SearchDrug("", this, null);
        gvDrug.SelectedIndexes.Clear();
    }

    protected void btnAllRx_OnClick(object sender, EventArgs e)
    {
        string strSRText = GetSearchText();
        PopulateAllDrug(bC.ParseQ(strSRText.Trim()));
        gvDrug.SelectedIndexes.Clear();
        lbl_Msg.Text = "";
    }

    protected void btnPastRx_OnClick(object sender, EventArgs e)
    {
        string strSRText = GetSearchText();
        PopulatePastDrug(bC.ParseQ(strSRText.Trim()));
        gvDrug.SelectedIndexes.Clear();
        lbl_Msg.Text = "";
    }

    protected void btnPastPatientRx_OnClick(object sender, EventArgs e)
    {
        string strSRText = GetSearchText();
        PopulatePastPatientDrug(bC.ParseQ(strSRText.Trim()));
        gvDrug.SelectedIndexes.Clear();
        lbl_Msg.Text = "";
    }

    protected void btn_FavouriteRx_OnClick(object sender, EventArgs e)
    {
        string strSRText = GetSearchText();
        PopulateFavouriteDrug(bC.ParseQ(strSRText.Trim()));
        gvDrug.SelectedIndexes.Clear();
        lbl_Msg.Text = "";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvDrug.SelectedIndexes.Clear();
        string strtext = string.Empty;
        strtext = GetSearchText();
        PopulateAllDrug(strtext);
    }

    public string GetSearchText()
    {
        string strSearchCriteria = string.Empty;
        int iSelectedIndex = ddlOption.SelectedIndex;
        switch (iSelectedIndex)
        {
            case 0:
                strSearchCriteria = "%" + txtSearch.Text.ToString().Trim() + "%";
                break;
            case 1:
                strSearchCriteria = txtSearch.Text.ToString().Trim() + "%";
                break;
            case 2:
                strSearchCriteria = "%" + txtSearch.Text.ToString().Trim();
                break;
        }
        return strSearchCriteria;
    }

    protected void SearchDrug(string name, object sender, EventArgs e)
    {
        if (Convert.ToString(ViewState["BTN"]) == "ALL")
        {
            PopulateAllDrug(name);
        }
        else if (Convert.ToString(ViewState["BTN"]) == "DOC")
        {
            PopulatePastDrug(name);
        }
        else if (Convert.ToString(ViewState["BTN"]) == "PAT")
        {
            PopulatePastPatientDrug(name);
        }
        else if (Convert.ToString(ViewState["BTN"]) == "FAV")
        {
            PopulateFavouriteDrug(name);
        }
    }

    private void PopulateAllDrug(string name)
    {
        try
        {
            ViewState["BTN"] = "ALL";
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshin = new Hashtable();
            hshin.Add("@intSynonym_Type_Id", rbonamefilter.SelectedValue);
            //hshin.Add("@chvSearchCriteria", "%%");
            if (name.Length > 0)
            {
                hshin.Add("@chvSearchCriteria", name);
            }
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDrugList", hshin);
            DrugDt = null;
            DrugDt = ds.Tables[0];
            gvDrug.DataSource = ds.Tables[0];
            gvDrug.DataBind();
            lbl_Msg.Text = "";
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateFilteredDrug(string type)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshin = new Hashtable();
            hshin.Add("@intSynonym_Type_Id", rbonamefilter.SelectedValue);
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDrugList", hshin);
            DrugDt = null;
            DrugDt = ds.Tables[0];
            gvDrug.DataSource = ds.Tables[0];
            gvDrug.DataBind();
            lbl_Msg.Text = "";
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulatePastDrug(string name)
    {
        try
        {
            if (Session["DoctorId"] != null)
            {
                ViewState["BTN"] = "DOC";
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshin = new Hashtable();
                if (name.Length > 0)
                {
                    hshin.Add("@chvSearchCriteria", name);
                }
                hshin.Add("@intSynonym_Type_Id", rbonamefilter.SelectedValue);
                hshin.Add("@inyHospitalLocationID", Session["HospitalLocationId"]);
                hshin.Add("@intDoctorID", Session["DoctorId"]);
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDoctorDrug", hshin);
                DrugDt = null;
                DrugDt = ds.Tables[0];
                gvDrug.DataSource = ds.Tables[0];
                gvDrug.DataBind();
                lbl_Msg.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulatePastPatientDrug(string name)
    {
        try
        {
            if (Session["HospitalLocationId"] != null && Session["registrationId"] != null)
            {
                ViewState["BTN"] = "PAT";
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshin = new Hashtable();
                if (name.Length > 0)
                {
                    hshin.Add("@chvSearchCriteria", name);
                }
                hshin.Add("@intSynonym_Type_Id", rbonamefilter.SelectedValue);
                hshin.Add("@inyHospitalLocationID", Session["HospitalLocationId"]);
                hshin.Add("@intRegistrationId", Session["RegistrationId"]);
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDrug", hshin);
                DrugDt = null;
                DrugDt = ds.Tables[0];
                gvDrug.DataSource = ds.Tables[0];
                gvDrug.DataBind();
                lbl_Msg.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateFavouriteDrug(string name)
    {
        try
        {
            if (Session["DoctorId"] != null)
            {
                ViewState["BTN"] = "FAV";
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshin = new Hashtable();
                if (name.Length > 0)
                {
                    hshin.Add("@chvSearchCriteria", name);
                }
                hshin.Add("@intSynonym_Type_Id", rbonamefilter.SelectedValue);
                hshin.Add("@intDoctorID", Session["DoctorId"]);
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavoriteDrug", hshin);
                DrugDt = null;
                DrugDt = ds.Tables[0];

                gvDrug.DataSource = ds.Tables[0];
                gvDrug.DataBind();
                lbl_Msg.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDrug_PageIndexChanging(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvDrug.SelectedIndexes.Clear();

        string strSRText = GetSearchText();
        SearchDrug(bC.ParseQ(strSRText), this, null);
    }

    protected void gvDrug_OnRowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Header)
        {
            if (rbonamefilter.SelectedValue == "60")
            {
                if (Convert.ToString(ViewState["BTN"]) == "ALL")
                {
                    e.Item.Cells[2].Text = "Master List of Brand(All)";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "DOC")
                {
                    e.Item.Cells[2].Text = "Master List Of Brand(Current Doctor)";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "PAT")
                {
                    e.Item.Cells[2].Text = "Master List Of Brand(Current Patient)";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "FAV")
                {
                    e.Item.Cells[2].Text = "Master List Of Brand(My Favourites)";
                }
            }
            else if (rbonamefilter.SelectedValue == "59")
            {
                if (Convert.ToString(ViewState["BTN"]) == "ALL")
                {
                    e.Item.Cells[2].Text = "Master List of Generic(All)";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "DOC")
                {
                    e.Item.Cells[2].Text = "Master List Of Generic(Current Doctor)";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "PAT")
                {
                    e.Item.Cells[2].Text = "Master List Of Generic(Current Patient)";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "FAV")
                {
                    e.Item.Cells[2].Text = "Master List Of Generic(My Favourites)";
                }
            }
            else
            {
                if (Convert.ToString(ViewState["BTN"]) == "ALL")
                {
                    e.Item.Cells[2].Text = "Master List of Generic/Brand(All)";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "DOC")
                {
                    e.Item.Cells[2].Text = "Master List Of Generic/Brand(Current Doctor)";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "PAT")
                {
                    e.Item.Cells[2].Text = "Master List Of Generic/Brand(Current Patient)";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "FAV")
                {
                    e.Item.Cells[2].Text = "Master List Of Generic/Brand(My Favourites)";
                }
            }
        }
    }

    private void BindCharge(string iServiceId, TextBox txt1)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            hshIn.Add("@intRegistrationID", Convert.ToInt32(Session["registrationID"]));
            hshIn.Add("@intServiceId", Convert.ToInt32("0"));
            hshIn.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));
            hshIn.Add("@intCompanyid", Convert.ToInt16(ViewState["CompanyCode"]));
            hshIn.Add("@intSpecialisationId", Convert.ToInt32("0"));
            hshIn.Add("@intDoctorId", Convert.ToInt32("0"));
            hshIn.Add("@chvJCode", ViewState["JCode"].ToString());
            hshIn.Add("@intFacilityId", getFacilityIdbyEncounterId());
            hshOut.Add("@NChr", SqlDbType.Money);
            hshOut.Add("@DNchr", SqlDbType.Money);
            hshOut.Add("@DiscountNAmt", SqlDbType.Money);
            hshOut.Add("@DiscountDNAmt", SqlDbType.Money);
            hshOut.Add("@PatientNPayable ", SqlDbType.Money);
            hshOut.Add("@PayorNPayable", SqlDbType.Money);
            hshOut.Add("@DiscountPerc", SqlDbType.Money);
            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetServiceChargeOPOneService", hshIn, hshOut);
            txt1.Text = (Convert.ToDouble(hshOut["@NChr"]) + Convert.ToDouble(hshOut["@DNchr"]) - (Convert.ToDouble(hshOut["@DiscountDNAmt"]) + Convert.ToDouble(hshOut["@DiscountNAmt"]))).ToString();
            hdnServiceAmount.Value = hshOut["@NChr"].ToString();
            hdnDoctorAmount.Value = hshOut["@DNchr"].ToString();
            hdnDoctorDiscountAmount.Value = hshOut["@DiscountDNAmt"].ToString();
            hdnServiceDiscountAmount.Value = hshOut["@DiscountNAmt"].ToString();

            if (Convert.ToDouble(txt1.Text) == 0)
            {
                txt1.Text = string.Empty;
                txtUnitCharge.ReadOnly = false;
            }
            else
                txtUnitCharge.ReadOnly = true;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private string getFacilityIdbyEncounterId()
    {
        try
        {
            if (Session["EncounterId"] != null)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hsInput = new Hashtable();
                hsInput.Add("EncounterId", Session["EncounterId"].ToString());
                string FacilityId = (string)objDl.ExecuteScalar(CommandType.Text, "select Convert(varchar,FacilityID) FacilityId from Encounter where Id=@EncounterId", hsInput);
                return FacilityId;
            }
            else
                return "0";
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return "0";
        }
    }

    protected void gvDrug_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenField lblDRUG_ID = ((HiddenField)(gvDrug.SelectedItems[0].FindControl("hdnDRUG_ID")));
            HiddenField lblGENPRODUCT_ID = ((HiddenField)(gvDrug.SelectedItems[0].FindControl("hdnGENPRODUCT_ID")));
            HiddenField lblDRUG_SYN_ID = ((HiddenField)(gvDrug.SelectedItems[0].FindControl("hdnDRUG_SYN_ID")));
            Label lblDISPLAY_NAME = ((Label)(gvDrug.SelectedItems[0].FindControl("lbl_DISPLAY_NAME")));
            HiddenField lblGENERIC_NAME = ((HiddenField)(gvDrug.SelectedItems[0].FindControl("hdnGENERIC_NAME")));
            HiddenField hdnJCode = ((HiddenField)(gvDrug.SelectedItems[0].FindControl("hdnJCode")));
            hdn_DRUG_ID.Value = lblDRUG_ID.Value;
            hdn_GENPRODUCT_ID.Value = lblGENPRODUCT_ID.Value;
            hdn_DRUG_SYN_ID.Value = lblDRUG_SYN_ID.Value;
            lbl_GENERIC_NAME.Text = lblGENERIC_NAME.Value;
            lbl_DISPLAY_NAME.Text = lblDISPLAY_NAME.Text;
            ltrId.Value = "0";
            lblCodeJ.Text = hdnJCode.Value;
            ViewState["JCode"] = hdnJCode.Value;
            cmbNDCList.Text = "";
            DataTable data = BindSearchDrugCombo(lblDISPLAY_NAME.Text.Trim());

            int itemOffset = 0;
            if (itemOffset == 0)
            {
                this.cmbDrugList.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            //e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = 0; i <= data.Rows.Count - 1; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["Display_Name"];
                item.Value = data.Rows[i]["GENPRODUCT_ID"].ToString();
                item.Attributes["DRUG_SYN_ID"] = data.Rows[i]["DRUG_SYN_ID"].ToString();
                item.Attributes["Drug_Id"] = data.Rows[i]["Drug_Id"].ToString();
                item.Attributes["SYNONYM_TYPE_ID"] = data.Rows[i]["SYNONYM_TYPE_ID"].ToString();
                item.Attributes["ROUTE_ID"] = data.Rows[i]["ROUTE_ID"].ToString();
                item.Attributes["ROUTE_DESCRIPTION"] = data.Rows[i]["ROUTE_DESCRIPTION"].ToString();
                item.Attributes["DOSEFORM_ID"] = data.Rows[i]["DOSEFORM_ID"].ToString();
                item.Attributes["DOSEFORM_DESCRIPTION"] = data.Rows[i]["DOSEFORM_DESCRIPTION"].ToString();
                this.cmbDrugList.Items.Add(item);
                item.DataBind();
            }

            //cmbDrugList.Text = lblDISPLAY_NAME.Text;
            cmbDrugList.Text = "";
            cmbDrugList.SelectedIndex = cmbDrugList.Items.IndexOf(cmbDrugList.Items.FindItemByText(lblDISPLAY_NAME.Text));
            lblInfoDrug.Visible = false;
            lblInfoTopGebericName.Visible = true;
            lbl_DISPLAY_NAME.Visible = false;
            lbl_GENERIC_NAME.Visible = true;
            cmdAddtoList.Text = "Add to List";
            if (gvDrug.SelectedItems != null)
            {
                gvDrug.SelectedIndexes.Clear();
            }
            if (gvAddedDrug.SelectedItems != null)
            {
                gvAddedDrug.SelectedIndexes.Clear();
            }
            BindCharge(Convert.ToString(ViewState["JCode"].ToString()), txtUnitCharge);
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected string getNDCTradeName(string Pkg_ProductId)
    {
        string strName = "";
        try
        {
            StringBuilder objStr = new StringBuilder();
            Hashtable hsInput = new Hashtable();
            hsInput.Add("productId", Pkg_ProductId);
            objStr.Append("SELECT distinct NPP.PKG_PRODUCT_ID + ' - ' + NTN.TRADE_NAME as DisplayName");
            objStr.Append(" FROM dbo.NDC_PKG_PRODUCT AS NPP ");
            objStr.Append(" INNER JOIN dbo.NDC_TRADE_NAME AS NTN ON NPP.TRADE_NAME_ID = NTN.TRADE_NAME_ID where NPP.PKG_PRODUCT_ID = @productId");
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strName = (string)objDl.ExecuteScalar(CommandType.Text, objStr.ToString(), hsInput);
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strName;
    }

    protected void gvAddedDrug_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (gvDrug.SelectedItems != null)
            {
                gvDrug.SelectedIndexes.Clear();
            }
            int RowIndex = 0;
            RowIndex = Convert.ToInt32(gvAddedDrug.SelectedItems[0].ItemIndex);
            ViewState["RowIndex"] = RowIndex;
            HiddenField ltrId1 = (HiddenField)gvAddedDrug.SelectedItems[0].FindControl("ltrId");
            Label lblDISPLAY_NAME = (Label)gvAddedDrug.SelectedItems[0].FindControl("lblDISPLAY_NAME");
            Label lblQtyAmount = (Label)gvAddedDrug.SelectedItems[0].FindControl("lblQtyAmount");
            HiddenField lblGENPRODUCT_ID = (HiddenField)gvAddedDrug.SelectedItems[0].FindControl("lblGENPRODUCT_ID");
            HiddenField lblDRUG_ID = (HiddenField)gvAddedDrug.SelectedItems[0].FindControl("lblDRUG_ID");
            HiddenField lblDRUG_SYN_ID = (HiddenField)gvAddedDrug.SelectedItems[0].FindControl("lblDRUG_SYN_ID");
            HiddenField lblGENERIC_NAME = (HiddenField)gvAddedDrug.SelectedItems[0].FindControl("lblGENERIC_NAME");
            Label lblICDCode = (Label)gvAddedDrug.SelectedItems[0].FindControl("lblICDCode");
            Label lblUnit_Charge = (Label)gvAddedDrug.SelectedItems[0].FindControl("lblUnitCharge");
            Label lblBill_Patient = (Label)gvAddedDrug.SelectedItems[0].FindControl("lblBillPatient");
            Label lblNDCCode = (Label)gvAddedDrug.SelectedItems[0].FindControl("lblNDCCode");
            HiddenField lblPrescriptionMode = (HiddenField)gvAddedDrug.SelectedItems[0].FindControl("lblPrescriptionMode");
            if (lblPrescriptionMode.Value == "A")
            {
                ddlModeType.SelectedIndex = 0;
            }
            else
            {
                ddlModeType.SelectedIndex = 1;
            }
            ltrId.Value = ltrId1.Value;
            lblInfoDrug.Visible = false;
            lblInfoTopGebericName.Visible = true;
            lbl_DISPLAY_NAME.Visible = false;
            lbl_GENERIC_NAME.Visible = true;

            hdn_GENPRODUCT_ID.Value = lblGENPRODUCT_ID.Value;
            hdn_DRUG_ID.Value = lblDRUG_ID.Value;
            hdn_DRUG_SYN_ID.Value = lblDRUG_SYN_ID.Value;
            lbl_GENERIC_NAME.Text = lblGENERIC_NAME.Value;
            lbl_DISPLAY_NAME.Text = lblDISPLAY_NAME.Text;
            cmbDrugList.Text = lblDISPLAY_NAME.Text;
            cmbNDCList.Text = getNDCTradeName(lblNDCCode.Text);
            txtUnits.Text = lblQtyAmount.Text;
            txtICDCode.Text = lblICDCode.Text;

            if (lblBill_Patient.Text.ToString().Trim() == "")
            {
                cbBillPatient.Checked = false;
            }
            else
            {
                cbBillPatient.Checked = true;
            }

            txtUnitCharge.Text = lblUnit_Charge.Text;
            cmdAddtoList.Text = "Update List";
            lbl_Msg.Text = "";
            lblPrescriptionMode.Value = "";
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvAddedDrug_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            Label lblServiceAmount = (Label)e.Item.FindControl("lblUnitCharge");
            HiddenField hdnServiceAmount = (HiddenField)e.Item.FindControl("hdnServiceAmount");
            HiddenField hdnServiceDiscountAmount = (HiddenField)e.Item.FindControl("hdnServiceDiscountAmount");
            HiddenField hdnDoctorAmount = (HiddenField)e.Item.FindControl("hdnDoctorAmount");
            HiddenField hdnDoctorDiscountAmount = (HiddenField)e.Item.FindControl("hdnDoctorDiscountAmount");

            if (lblServiceAmount.Text.Trim().ToString() == "0.00" || lblServiceAmount.Text.Trim().ToString() == "")
            {
                lblServiceAmount.Text = "$0.00";
            }
            else
            {
                lblServiceAmount.Text = "$" + (Convert.ToDecimal(lblServiceAmount.Text));
            }
        }
    }

    protected void gvAddedDrug_RowCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Del")
            {
                if (Session["FacilityId"] != null && Session["HospitalLocationID"] != null && Session["RegistrationId"] != null && Session["EncounterId"] != null)
                {
                    HiddenField lblId = (HiddenField)e.Item.FindControl("ltrId");
                    string strId = lblId.Value;

                    if (!string.IsNullOrEmpty(strId))
                    {
                        //string strQuery = "UPDATE OPPrescriptionDetail SET Active = 0 WHERE Id = @Id";
                        if (pageId != "")
                            pageId = pageId.Substring(1, pageId.Length - 1);
                        else
                            pageId = "0";

                        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        Hashtable hstIn = new Hashtable();
                        hstIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                        hstIn.Add("@intLoginFacilityId", Session["FacilityId"]);
                        hstIn.Add("@intRegistrationId", Session["RegistrationId"].ToString());
                        hstIn.Add("@intEncounterId", Session["EncounterId"].ToString());
                        hstIn.Add("@intPageId", pageId);
                        hstIn.Add("@intPrescriptionId", strId);
                        hstIn.Add("@intEncodedBy", Session["UserId"].ToString());
                        dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeletePatientPrescription", hstIn);
                    }
                    DataTable dt = new DataTable();
                    dt = (DataTable)Cache["SB_Prescription_" + Session["UserId"]];
                    if (dt != null)
                    {
                        dt.Rows.RemoveAt(e.Item.ItemIndex);
                        DataView ldv = new DataView(dt);
                        //ldv.Sort = "Id desc";
                        if (gvAddedDrug.SelectedItems != null)
                        {
                            gvAddedDrug.SelectedIndexes.Clear();
                        }

                        gvAddedDrug.DataSource = ldv;
                        gvAddedDrug.DataBind();
                        Cache["SB_Prescription_" + Session["UserId"]] = ldv.ToTable();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void cmdAddtoList_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (CheckPatientDrugs(hdn_GENPRODUCT_ID.Value, false) && cmdAddtoList.Text != "Update List")
            {
                Alert.ShowAjaxMsg("Drug is already in prescription", Page);
                return;
            }

            if (txtICDCode.Text.Trim() != "")
            {
                if (cmbDrugList.Text.Trim() != "")
                {
                    if (cmbNDCList.Text.Trim() != "")
                    {
                        if (txtUnits.Text.Trim() != "" && txtUnitCharge.Text.Trim() != "")
                        {
                            if (Convert.ToDouble(txtUnits.Text) > 0)
                            {
                                if (txtUnitCharge.Text == "$0" || txtUnitCharge.Text.Trim().ToString() == "0")
                                {
                                    Alert.ShowAjaxMsg("You are entering the unit charge($0).", Page);
                                    goto SaveLabel;
                                }
                                else
                                {
                                    goto SaveLabel;
                                }
                            SaveLabel:
                                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                                Hashtable hshTable = new Hashtable();
                                String xmlstring = "";

                                xmlstring = xmlstring + "<Table1><c1>" + hdn_GENPRODUCT_ID.Value + "</c1>";

                                xmlstring = xmlstring + "<c2>" + hdn_DRUG_SYN_ID.Value + "</c2>";
                                xmlstring = xmlstring + "<c3>" + DBNull.Value + "</c3>";
                                xmlstring = xmlstring + "<c4>" + DBNull.Value + "</c4>";
                                xmlstring = xmlstring + "<c5>" + DBNull.Value + "</c5>";
                                xmlstring = xmlstring + "<c6>" + DBNull.Value + "</c6>";
                                xmlstring = xmlstring + "<c7>" + txtICDCode.Text + "</c7>";
                                xmlstring = xmlstring + "<c8>" + DBNull.Value + "</c8>";
                                xmlstring = xmlstring + "<c9>" + DBNull.Value + "</c9>";
                                xmlstring = xmlstring + "<c10>" + txtUnits.Text + "</c10>";
                                xmlstring = xmlstring + "<c11>" + DBNull.Value + "</c11>";
                                xmlstring = xmlstring + "<c12>" + DBNull.Value + "</c12>";
                                xmlstring = xmlstring + "<c13>" + DBNull.Value + "</c13>";
                                xmlstring = xmlstring + "<c14>" + "35" + "</c14>";
                                xmlstring = xmlstring + "<c15>" + DBNull.Value + "</c15>";
                                //xmlstring = xmlstring + "<c16>" + DBNull.Value + "</c16>";
                                //xmlstring = xmlstring + "<c17>" + "0" + "</c17>";
                                //xmlstring = xmlstring + "<c18>" + "0" + "</c18>";
                                xmlstring = xmlstring + "<c16>" + ddlModeType.SelectedValue + "</c16>";
                                xmlstring = xmlstring + "<c17>" + DBNull.Value + "</c17>";
                                xmlstring = xmlstring + "<c18>" + DBNull.Value + "</c18>";
                                xmlstring = xmlstring + "<c19>" + DBNull.Value + "</c19>";
                                xmlstring = xmlstring + "<c20>" + DBNull.Value + "</c20>";
                                xmlstring = xmlstring + "<c21>" + DBNull.Value + "</c21>";
                                xmlstring = xmlstring + "<c22>" + DBNull.Value + "</c22>";
                                xmlstring = xmlstring + "<c23>" + DBNull.Value + "</c23>";
                                if (txtUnitCharge.Text == "$0.00" || txtUnitCharge.Text == "")
                                {
                                    xmlstring = xmlstring + "<c24>" + "$0.00" + "</c24>";
                                }
                                else
                                {
                                    xmlstring = xmlstring + "<c24>" + txtUnitCharge.Text + "</c24>";
                                }
                                if (cmbNDCList.Text != "")
                                {
                                    string[] str = cmbNDCList.Text.Split(new char[] { '-' });
                                    xmlstring = xmlstring + "<c25>" + str[0].Trim() + "</c25>";
                                }
                                else
                                    xmlstring = xmlstring + "<c25>" + DBNull.Value + "</c25>";

                                if (cbBillPatient.Checked)
                                    xmlstring = xmlstring + "<c26>1</c26>";
                                else
                                    xmlstring = xmlstring + "<c26>0</c26>";

                                xmlstring = xmlstring + "<c27>" + ltrId.Value + "</c27>";
                                xmlstring = xmlstring + "<c28>" + DBNull.Value + "</c28>";
                                xmlstring = xmlstring + "<c29>" + DBNull.Value + "</c29></Table1>";
                                if (xmlstring != "")
                                {
                                    hshTable.Add("@intRegistrationId", Session["RegistrationID"]);
                                    hshTable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                                    hshTable.Add("@intEncounterId", Session["encounterid"]);
                                    //hshTable.Add("@intDoctorId", Session["DoctorID"]);
                                    hshTable.Add("@intLoginFacilityId", Session["facilityId"]);

                                    if (pageId != "")
                                    {
                                        hshTable.Add("@intPageId", pageId.Substring(1, pageId.Length - 1));
                                    }
                                    else
                                        hshTable.Add("@intPageId", 0);

                                    hshTable.Add("@intEncodedBy", Session["UserId"]);
                                    hshTable.Add("@bitEMRPrescription", "0");
                                    hshTable.Add("XMLData", xmlstring);
                                    objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPEMRSaveMedication", hshTable);
                                    lbl_Msg.Text = "Record(s) Has Been Saved...";

                                    hdn_GENPRODUCT_ID.Value = String.Empty;
                                    hdn_DRUG_ID.Value = String.Empty;
                                    hdn_DRUG_SYN_ID.Value = String.Empty;
                                    lbl_GENERIC_NAME.Text = String.Empty;
                                    lbl_DISPLAY_NAME.Text = String.Empty;
                                    cmbDrugList.Text = string.Empty;

                                    txtICDCode.Text = String.Empty;
                                    txtUnitCharge.Text = String.Empty;
                                    cmdAddtoList.Text = "Add to List";
                                    ltrId.Value = "0";
                                    //lbl_Msg.Text = "";
                                    getPatientDrugHistory();
                                    if (gvDrug.SelectedItems != null)
                                    {
                                        gvDrug.SelectedIndexes.Clear();
                                    }
                                }
                            }
                            //    else
                            //    {
                            //        Alert.ShowAjaxMsg("Unit Charge Should be greater then 0", Page);
                            //        return;
                            //    }
                            //}
                            else
                            {
                                Alert.ShowAjaxMsg("Unit Should be greater then 0", Page);
                                return;
                            }
                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Select NDC before proceed", Page);
                        return;
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Select drug before proceed", Page);
                    return;
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Please Enter ICD Code", Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //try
        //{
        //    if (Convert.ToString(txtUnits.Text) == "")
        //    {
        //        Alert.ShowAjaxMsg("Please Enter Unit", Page);
        //        return;
        //    }
        //    if (Convert.ToString(txtUnitCharge.Text) == "")
        //    {
        //        Alert.ShowAjaxMsg("Please Enter Unit Charge", Page);
        //        return;
        //    }
        //    if (cmdAddtoList.Text == "Add to List")
        //    {
        //        if (hdn_DRUG_SYN_ID.Value != "" || gvDrug.SelectedItems != null)
        //        {
        //            if (txtUnits.Text.Trim() != "" && txtUnitCharge.Text.Trim() != "")
        //            {
        //                if (Convert.ToDouble(txtUnits.Text) > 0)
        //                {
        //                    if (Convert.ToDouble(txtUnitCharge.Text) > 0)
        //                    {
        //                        DataTable dt1 = new DataTable();
        //                        dt1.Columns.Add("Id");
        //                        dt1.Columns["Id"].DataType = typeof(int);

        //                        dt1.Columns.Add("DISPLAY_NAME");
        //                        dt1.Columns.Add("QtyAmount");
        //                        dt1.Columns.Add("DRUG_ID");
        //                        dt1.Columns.Add("PrescriptionMode");
        //                        dt1.Columns.Add("GENPRODUCT_ID");
        //                        dt1.Columns.Add("DRUG_SYN_ID");
        //                        dt1.Columns.Add("GENERIC_NAME");
        //                        dt1.Columns.Add("UNIT_CHARGE");
        //                        dt1.Columns.Add("IsBillable");
        //                        dt1.Columns.Add("ICDCode");
        //                        dt1.Columns.Add("NDC");

        //                        DataRow[] dr = null;
        //                        DataRow r;

        //                        foreach (Telerik.Web.UI.GridItem item in gvAddedDrug.Items)
        //                        {
        //                            r = dt1.NewRow();
        //                            dr = dt1.Select("DRUG_SYN_ID=" + ((HiddenField)item.FindControl("lblDRUG_SYN_ID")).Value);
        //                            if (dr.Length == 0)
        //                            {
        //                                r["DISPLAY_NAME"] = ((Label)item.FindControl("lblDISPLAY_NAME")).Text;
        //                                r["QtyAmount"] = ((Label)item.FindControl("lblQtyAmount")).Text;
        //                                r["DRUG_ID"] = ((HiddenField)item.FindControl("lblDRUG_ID")).Value;
        //                                r["GENPRODUCT_ID"] = ((HiddenField)item.FindControl("lblGENPRODUCT_ID")).Value;
        //                                r["DRUG_SYN_ID"] = ((HiddenField)item.FindControl("lblDRUG_SYN_ID")).Value;
        //                                r["GENERIC_NAME"] = ((HiddenField)item.FindControl("lblGENERIC_NAME")).Value;
        //                                r["UNIT_CHARGE"] = ((Label)item.FindControl("lblUnitCharge")).Text;
        //                                r["IsBillable"] = ((Label)item.FindControl("lblBillPatient")).Text;
        //                                r["ICDCode"] = ((Label)item.FindControl("lblICDCode")).Text;
        //                                r["PrescriptionMode"] = ((HiddenField)item.FindControl("lblPrescriptionMode")).Value;
        //                                r["Id"] = ((HiddenField)item.FindControl("ltrId")).Value;
        //                                r["NDC"] = "";
        //                                dt1.Rows.Add(r);
        //                            }
        //                        }

        //                        r = dt1.NewRow();
        //                        dr = dt1.Select("DRUG_SYN_ID=" + hdn_DRUG_SYN_ID.Value);
        //                        if (dr.Length == 0)
        //                        {
        //                            r["DISPLAY_NAME"] = cmbDrugList.Text;
        //                            r["QtyAmount"] = Convert.ToString(txtUnits.Text);
        //                            r["DRUG_ID"] = hdn_DRUG_ID.Value;
        //                            r["GENPRODUCT_ID"] = hdn_GENPRODUCT_ID.Value;
        //                            r["DRUG_SYN_ID"] = hdn_DRUG_SYN_ID.Value;
        //                            r["GENERIC_NAME"] = lbl_GENERIC_NAME.Text;
        //                            r["UNIT_CHARGE"] = Convert.ToString(txtUnitCharge.Text);
        //                            r["IsBillable"] = Convert.ToString(cbBillPatient.Checked);
        //                            r["ICDCode"] = Convert.ToString(txtICDCode.Text);
        //                            r["Id"] = "0";
        //                            r["PrescriptionMode"] = ddlModeType.SelectedValue;
        //                            r["NDC"] = Convert.ToString(cmbDrugList.Text);

        //                            dt1.Rows.Add(r);
        //                        }
        //                        DataView ldv = new DataView(dt1);
        //                        ldv.Sort = "Id desc";
        //                        gvAddedDrug.DataSource = ldv;
        //                        gvAddedDrug.DataBind();
        //                        Cache["SB_Prescription_" + Session["UserId"]] = ldv.ToTable();
        //                        lbl_Msg.Text = "";
        //                        hdn_DRUG_SYN_ID.Value = "";
        //                    }
        //                    else
        //                    {
        //                        Alert.ShowAjaxMsg("Unit Charge Should be greater then 0", Page);
        //                        return;
        //                    }
        //                }
        //                else
        //                {
        //                    Alert.ShowAjaxMsg("Unit Should be greater then 0", Page);
        //                    return;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Alert.ShowAjaxMsg("Please select a drug to add", Page);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        if (gvAddedDrug.SelectedItems[0] != null)
        //        {
        //            if (txtUnits.Text.Trim() != "" && txtUnitCharge.Text.Trim() != "")
        //            {
        //                if (Convert.ToDouble(txtUnits.Text) > 0)
        //                {
        //                    if (Convert.ToDouble(txtUnitCharge.Text) > 0)
        //                    {
        //                        DataTable dt = new DataTable();
        //                        DataRow drow;
        //                        drow = dt.NewRow();
        //                        if (Cache["SB_Prescription_" + Session["UserId"]] != null)
        //                        {
        //                            dt = (DataTable)Cache["SB_Prescription_" + Session["UserId"]];
        //                        }
        //                        drow = dt.NewRow();
        //                        drow.BeginEdit();
        //                        dt.Rows[Convert.ToInt32(ViewState["RowIndex"])]["QtyAmount"] = Convert.ToString(txtUnits.Text);
        //                        dt.Rows[Convert.ToInt32(ViewState["RowIndex"])]["UNIT_CHARGE"] = Convert.ToString(txtUnitCharge.Text);
        //                        dt.Rows[Convert.ToInt32(ViewState["RowIndex"])]["BILL_PATIENT"] = Convert.ToString(cbBillPatient.Checked);
        //                        dt.Rows[Convert.ToInt32(ViewState["RowIndex"])]["ICDCode"] = Convert.ToString(txtICDCode.Text);
        //                        dt.Rows[Convert.ToInt32(ViewState["RowIndex"])]["NDC"] = Convert.ToString(cmbDrugList.Text);

        //                        drow.EndEdit();
        //                        dt.AcceptChanges();
        //                        DataView ldv = new DataView(dt);
        //                        ldv.Sort = "Id desc";
        //                        gvAddedDrug.DataSource = ldv;
        //                        gvAddedDrug.DataBind();
        //                        Cache["SB_Prescription_" + Session["UserId"]] = ldv.ToTable();
        //                        lbl_Msg.Text = "";
        //                    }
        //                    {
        //                        Alert.ShowAjaxMsg("Unit Charge Should be greater then 0", Page);
        //                        return;
        //                    }
        //                }
        //                else
        //                {
        //                    Alert.ShowAjaxMsg("Unit Should be greater then 0", Page);
        //                    return;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Alert.ShowAjaxMsg("Please select a drug to modify", Page);
        //            return;
        //        }
        //    }

        //    hdn_GENPRODUCT_ID.Value = String.Empty;
        //    hdn_DRUG_ID.Value = String.Empty;
        //    hdn_DRUG_SYN_ID.Value = String.Empty;
        //    lbl_GENERIC_NAME.Text = String.Empty;
        //    lbl_DISPLAY_NAME.Text = String.Empty;
        //    cmbDrugList.Text = string.Empty;
        //    txtUnits.Text = String.Empty;
        //    txtICDCode.Text = String.Empty;
        //    txtUnitCharge.Text = String.Empty;
        //    cmdAddtoList.Text = "Add to List";
        //    lbl_Msg.Text = "";
        //    if (gvDrug.SelectedItems != null)
        //    {
        //        gvDrug.SelectedIndexes.Clear();
        //    }
        //    cmdSave.Focus();
        //}
        //catch (Exception ex)
        //{
        //    lbl_Msg.Text = ex.Message;
        //}
    }

    protected void cmdSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshTable = new Hashtable();
            String xmlstring = "";
            int i = 0;
            if (gvAddedDrug.Items.Count == 0)
            {
                Alert.ShowAjaxMsg("Please add a drug before saving.", Page);
                return;
            }
            for (i = 0; i < gvAddedDrug.Items.Count; i++)
            {
                HiddenField lblSvGENPRODUCT_ID = (HiddenField)gvAddedDrug.Items[i].FindControl("lblGENPRODUCT_ID");
                HiddenField lblSvDRUG_SYN_ID = (HiddenField)gvAddedDrug.Items[i].FindControl("lblDRUG_SYN_ID");
                Label lblSvQtyAmount = (Label)gvAddedDrug.Items[i].FindControl("lblQtyAmount");
                Label lblSvICDCode = (Label)gvAddedDrug.Items[i].FindControl("lblICDCode");
                Label lblSvUnitCharge = (Label)gvAddedDrug.Items[i].FindControl("lblUnitCharge");
                Label lblSvBillPatient = (Label)gvAddedDrug.Items[i].FindControl("lblBillPatient");
                HiddenField lblPrescriptionMode = (HiddenField)gvAddedDrug.Items[i].FindControl("lblPrescriptionMode");
                HiddenField ltrId = (HiddenField)gvAddedDrug.Items[i].FindControl("ltrId");

                xmlstring = xmlstring + "<Table1><c1>" + lblSvGENPRODUCT_ID.Value + "</c1>";
                xmlstring = xmlstring + "<c2>" + lblSvDRUG_SYN_ID.Value + "</c2>";
                xmlstring = xmlstring + "<c3>" + DBNull.Value + "</c3>";
                xmlstring = xmlstring + "<c4>" + DBNull.Value + "</c4>";
                xmlstring = xmlstring + "<c5>" + DBNull.Value + "</c5>";
                xmlstring = xmlstring + "<c6>" + DBNull.Value + "</c6>";
                xmlstring = xmlstring + "<c7>" + lblSvICDCode.Text + "</c7>";
                xmlstring = xmlstring + "<c8>" + DBNull.Value + "</c8>";
                xmlstring = xmlstring + "<c9>" + DBNull.Value + "</c9>";
                xmlstring = xmlstring + "<c10>" + lblSvQtyAmount.Text + "</c10>";
                xmlstring = xmlstring + "<c11>" + DBNull.Value + "</c11>";
                xmlstring = xmlstring + "<c12>" + DBNull.Value + "</c12>";
                xmlstring = xmlstring + "<c13>" + DBNull.Value + "</c13>";
                xmlstring = xmlstring + "<c14>" + "1" + "</c14>";
                xmlstring = xmlstring + "<c15>" + Session["UserId"] + "</c15>";
                xmlstring = xmlstring + "<c16>" + DBNull.Value + "</c16>";
                xmlstring = xmlstring + "<c17>" + "0" + "</c17>";
                xmlstring = xmlstring + "<c18>" + "0" + "</c18>";
                xmlstring = xmlstring + "<c19>" + lblPrescriptionMode.Value + "</c19>";
                xmlstring = xmlstring + "<c20>" + DBNull.Value + "</c20>";
                xmlstring = xmlstring + "<c21>" + DBNull.Value + "</c21>";
                xmlstring = xmlstring + "<c22>" + DBNull.Value + "</c22>";
                xmlstring = xmlstring + "<c23>" + DBNull.Value + "</c23>";
                xmlstring = xmlstring + "<c24>" + DBNull.Value + "</c24>";
                xmlstring = xmlstring + "<c25>" + DBNull.Value + "</c25>";
                xmlstring = xmlstring + "<c26>" + DBNull.Value + "</c26>";
                xmlstring = xmlstring + "<c27>" + lblSvUnitCharge.Text + "</c27>";
                xmlstring = xmlstring + "<c28>" + cmbNDCList.SelectedValue + "</c28>";
                xmlstring = xmlstring + "<c29>" + DBNull.Value + "</c29>";
                xmlstring = xmlstring + "<c30>" + DBNull.Value + "</c30>";
                xmlstring = xmlstring + "<c31>" + ltrId.Value + "</c31></Table1>";
            }
            if (xmlstring != "")
            {
                hshTable.Add("@intRegistrationId", Session["RegistrationID"]);
                hshTable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                hshTable.Add("@intEncounterId", Session["encounterid"]);
                hshTable.Add("@intDoctorId", Session["DoctorID"]);
                hshTable.Add("@chvPrescriptionDate", Convert.ToString(DateTime.Today.ToString("dd/MM/yyyy")));
                hshTable.Add("@intEncodedBy", Session["UserId"]);
                hshTable.Add("@bitEMRPrescription", "0");
                hshTable.Add("XMLData", xmlstring);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPSaveMedication", hshTable);
                lbl_Msg.Text = "Record(s) Has Been Saved...";
                getPatientDrugHistory();
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void cmbDrugList_DataBound(object sender, EventArgs e)
    {
        //set the initial footer label
        ((Literal)cmbDrugList.Footer.FindControl("RadComboItemsCount")).Text = Convert.ToString(cmbDrugList.Items.Count);
    }

    //protected void cmbDrugList_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    //{
    //    e.Item.Text = ((DataRowView)e.Item.DataItem)["ContactName"].ToString();
    //    e.Item.Value = ((DataRowView)e.Item.DataItem)["CustomerID"].ToString(); 
    //}

    public void cmbDrugList_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = BindSearchDrugCombo(e.Text);

        int itemOffset = e.NumberOfItems;
        if (itemOffset == 0)
        {
            this.cmbDrugList.Items.Clear();
        }
        int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;

        for (int i = itemOffset; i < endOffset; i++)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = (string)data.Rows[i]["Display_Name"];
            item.Value = data.Rows[i]["GENPRODUCT_ID"].ToString();
            item.Attributes["DRUG_SYN_ID"] = data.Rows[i]["DRUG_SYN_ID"].ToString();
            item.Attributes["Drug_Id"] = data.Rows[i]["Drug_Id"].ToString();
            item.Attributes["SYNONYM_TYPE_ID"] = data.Rows[i]["SYNONYM_TYPE_ID"].ToString();
            item.Attributes["ROUTE_ID"] = data.Rows[i]["ROUTE_ID"].ToString();
            item.Attributes["ROUTE_DESCRIPTION"] = data.Rows[i]["ROUTE_DESCRIPTION"].ToString();
            item.Attributes["DOSEFORM_ID"] = data.Rows[i]["DOSEFORM_ID"].ToString();
            item.Attributes["DOSEFORM_DESCRIPTION"] = data.Rows[i]["DOSEFORM_DESCRIPTION"].ToString();
            this.cmbDrugList.Items.Add(item);
            item.DataBind();
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);
    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    protected DataTable BindSearchDrugCombo(String etext)
    {
        DataSet ds = new DataSet();
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshin = new Hashtable();
            hshin.Add("@intSynonym_Type_Id", rbonamefilter.SelectedValue);
            if (etext.ToString().Trim() != "")
            {
                hshin.Add("@chvSearchCriteria", "%" + etext + "%");
            }
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDrugList", hshin);
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return ds.Tables[0];
    }

    protected void lnkENMCodes_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("ENMCodes.aspx", false);
    }

    private void getPatientDrugHistory()
    {
        try
        {
            if (Session["EncounterID"] != null && Session["registrationId"] != null)
            {
                string sQ = "exec UspEMRGetPatientDrugHistory " + Session["HospitalLocationId"] + "," + Session["RegistrationId"] + "," + Session["EncounterId"] + ",0 ,'', 0, 0, 0, 0, 0,0,0, '', '%%', '', '', '', 0";
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                objDs = objDl.FillDataSet(CommandType.Text, sQ);
                DataView objDv = objDs.Tables[0].DefaultView;
                //objDv.RowFilter = "Unit_Charge not like '0*'";
                if (objDv.Count > 0)
                {
                    gvAddedDrug.DataSource = objDv.ToTable();
                    gvAddedDrug.DataBind();
                }
                Cache["SB_Prescription_" + Session["UserId"]] = objDv.ToTable();
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void FillMedicationChargesById(int Id)
    {
        try
        {
            if (ds.Tables.Count > 0)
            {
                DataTable dtMedicationCharges = objDs.Tables[0];
                DataView dv = new DataView(dtMedicationCharges);
                dv.RowFilter = "PrescriptionID=" + Id;
                DataTable dt = new DataTable();
                if (dv.Count > 0)
                {
                    dt = dv.ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        hdn_DRUG_ID.Value = dt.Rows[0]["Drug_ID"].ToString();
                        hdn_GENPRODUCT_ID.Value = dt.Rows[0]["GENPRODUCT_ID"].ToString();
                        hdn_DRUG_SYN_ID.Value = dt.Rows[0]["DRUG_SYN_ID"].ToString();
                        lbl_GENERIC_NAME.Text = dt.Rows[0]["GENERIC_NAME"].ToString();
                        lbl_DISPLAY_NAME.Text = dt.Rows[0]["DISPLAY_NAME"].ToString();
                        lblCodeJ.Text = dt.Rows[0]["JCODE"].ToString();
                        txtUnitCharge.Text = dt.Rows[0]["Unit_Charge"].ToString();
                        cmbDrugList.Text = dt.Rows[0]["DISPLAY_NAME"].ToString();
                        cmbNDCList.Text = getNDCTradeName(dt.Rows[0]["NDCCode"].ToString());
                        ddlModeType.SelectedIndex = ddlModeType.Items.IndexOf(ddlModeType.Items.FindItemByValue("PrescriptionMode"));
                        txtICDCode.Text = dt.Rows[0]["ICDCode"].ToString(); ;
                        lblInfoDrug.Visible = false;
                        ltrId.Value = Id.ToString();
                        lblInfoTopGebericName.Visible = true;
                        lbl_DISPLAY_NAME.Visible = false;
                        lbl_GENERIC_NAME.Visible = true;
                        cmdAddtoList.Text = "Update List";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void cmbDrugList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

    }

    protected void cmbNDCList_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        // if (cmbDrugList.SelectedIndex != -1)
        // {
        DataTable data = BindSearchNDCCombo(e.Text);

        int itemOffset = e.NumberOfItems;
        if (itemOffset == 0)
        {
            this.cmbNDCList.Items.Clear();
        }
        int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;

        for (int i = itemOffset; i < endOffset; i++)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = (string)data.Rows[i]["DisplayName"];
            item.Value = data.Rows[i]["PKG_PRODUCT_ID"].ToString();
            this.cmbNDCList.Items.Add(item);
            item.DataBind();
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        //}
    }

    protected DataTable BindSearchNDCCombo(String etext)
    {
        DataSet ds = new DataSet();
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder objStr = new StringBuilder();
            Hashtable hsInput = new Hashtable();
            hsInput.Add("intGenProductId", cmbDrugList.SelectedValue);
            if (etext.Trim() == "")
                hsInput.Add("chvSearchCriteria", "'%%'");
            else
                hsInput.Add("chvSearchCriteria", etext);
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetNDCCodes", hsInput);
            ds = dl.FillDataSet(CommandType.Text, "Exec UspEMRGetNDCCodes @intGenProductId, '%" + etext + "%' ", hsInput);
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return ds.Tables[0];
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("PatientMedicationCharges.aspx", false);
    }

    private bool CheckPatientDrugs(string GenProductId, bool IsCurrent)
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataView objDv;
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            hshInput.Add("@intEncounterId", Convert.ToString(Session["encounterid"]));
            hshInput.Add("@intRegistrationId", Session["RegistrationId"]);
            hshInput.Add("@bitEMRPrescription", "1");

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDrugHistory", hshInput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (IsCurrent == false)
                {
                    objDv = ds.Tables[0].DefaultView;
                    objDv.RowFilter = "GENPRODUCT_ID ='" + GenProductId + "'";
                    if (objDv.Count > 0)
                        return true;
                    else
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return false;
        }
    }

}