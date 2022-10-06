using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
public partial class EMR_Problems_HPI : System.Web.UI.Page
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    
    string ProblemIdString = "";
    DL_Funs fun = new DL_Funs();
    private Hashtable hstInput;

    //protected void Page_PreInit(object sender, System.EventArgs e) { Page.Theme = "DefaultControls"; }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
       
        if (common.myStr(Request.QueryString["Popup"]).Equals("Yes"))
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
        else 
        {
            Page.Theme = "DefaultControls";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Mpg"] != null)
        {
            Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            string pid = Session["CurrentNode"].ToString();
            int len = pid.Length;
            ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, len - 1);
        }
        else
        {
            ViewState["PageId"] = "0";
        }

        if (Convert.ToString(Session["encounterid"]) == "")
            Response.Redirect("/default.aspx?RegNo=0", false);

        if (Request.QueryString["ProbId"] != null)
            ProblemIdString = Request.QueryString["ProbId"].ToString();
        dtpOnsetDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
        dtpPriorHistoryDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();

        if (!IsPostBack)
        {
            try
            {
                BindProblems();
                if (Request.QueryString["ProblemId"] != null)
                {
                    RadComboBoxItem rcbItem = (RadComboBoxItem)ddlProblems.Items.FindItemByValue(Request.QueryString["ProblemId"].ToString());
                    if (rcbItem != null)
                        rcbItem.Selected = true;
                    ViewState["Page"] = Request.QueryString["Page"].ToString();


                }
                ShowHPIDetails();

            }
            catch (Exception ex)
            {
                clsExceptionLog objException = new clsExceptionLog();
                objException.HandleException(ex);
                objException = null;
                lbl_Msg.Text = ex.Message;
            }
        }
    }

    private void BindProblems()
    {
        DataSet dsObj = new DataSet();
        try
        {
            hstInput = new Hashtable();
            hstInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
            hstInput.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            
            string strSQL = "";
            strSQL = "Select Id, ProblemDescription from EMRPatientProblemDetails where RegistrationId = " + common.myInt(Session["RegistrationID"]).ToString() + " and EncounterId = " + common.myInt(Session["encounterid"]).ToString() + " and Active=1";
            
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet objDs = dl.FillDataSet(CommandType.Text, strSql);

            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSQL;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            dsObj = JsonConvert.DeserializeObject<DataSet>(sValue);

            //dsObj = objDl.FillDataSet(CommandType.Text, strSQL, hstInput);
            ddlProblems.DataSource = dsObj;
            ddlProblems.DataValueField = "Id";
            ddlProblems.DataTextField = "ProblemDescription";
            ddlProblems.DataBind();
            ddlProblems.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            dsObj.Dispose();
        }
    }

    protected void ddlProblems_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ProblemIdString = ddlProblems.SelectedValue.ToString();
        ShowHPIDetails();
    }

    private void ShowHPIDetails()
    {
        DataSet ds = new DataSet();
        try
        {
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //BaseC.Security AuditCA = new BaseC.Security(sConString);
            //hstInput = new Hashtable();
            //hstInput.Add("@intProblemId", Convert.ToInt32(ddlProblems.SelectedValue));
            //DataSet ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetProblemHPIDetails", hstInput);

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetProblemHPIDetails";
            APIRootClass.GetProblemHPIDetails objRoot = new global::APIRootClass.GetProblemHPIDetails();
            objRoot.ProblemId = common.myInt(ddlProblems.SelectedValue);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"]), Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), 0, "ACCESSED", Convert.ToString(Session["IPAddress"]));

                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/AuditCommonAccess";
                APIRootClass.Security obj = new global::APIRootClass.Security();
                obj.iHospID = Convert.ToInt16(Session["HospitalLocationID"]);
                obj.iFacilityID = common.myInt(Session["FacilityID"]);
                obj.iRegId = common.myInt(ViewState["RegistrationId"]);
                obj.iEncountId = common.myInt(ViewState["EncounterId"]);
                obj.iPageId = common.myInt(ViewState["PageId"]);
                obj.iTemplateId = 0;
                obj.iEncodedBy = common.myInt(Session["UserID"]);
                obj.iEmpId = 0;
                obj.iAuditStatus = "ACCESSED";
                obj.chvIPAddres = common.myStr(Session["IPAddress"]);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(obj);
                sValue = client.UploadString(ServiceURL, inputJson);

                if (Convert.ToString(ds.Tables[0].Rows[0]["OnsetDate"]) != "")
                {
                    dtpOnsetDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["OnsetDate"]);
                }
                else
                {
                    dtpOnsetDate.SelectedDate = null;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["NoOfOccurrence"]) != "")
                {
                    ddlNoofOccurances.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["NoOfOccurrence"]);
                }
                else
                {
                    ddlNoofOccurances.SelectedIndex = 0;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["PriorIllnessDate"]) != "")
                {
                    dtpPriorHistoryDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PriorIllnessDate"]);
                }
                else
                {
                    dtpPriorHistoryDate.SelectedDate = null;
                }
                txtRelievingFactors.Text = Convert.ToString(ds.Tables[0].Rows[0]["RelievingFactors"]);
                txtAggrevatingFactors.Text = Convert.ToString(ds.Tables[0].Rows[0]["AggravatingFactors"]);
                if (Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptomsId1"]) != "")
                {
                    cmbAdd1.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptomsId1"]);
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptomsId2"]) != "")
                {
                    cmbAdd2.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptomsId2"]);
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptomsId3"]) != "")
                {
                    cmbAdd3.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptomsId3"]);
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptomsId4"]) != "")
                {
                    cmbAdd4.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptomsId4"]);
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptomsId5"]) != "")
                {
                    cmbAdd5.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptomsId5"]);
                }
                cmbAdd1.Text = Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptoms1"]);
                cmbAdd2.Text = Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptoms2"]);
                cmbAdd3.Text = Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptoms3"]);
                cmbAdd4.Text = Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptoms4"]);
                cmbAdd5.Text = Convert.ToString(ds.Tables[0].Rows[0]["DeniesSymptoms5"]);
            }
            else
            {
                dtpOnsetDate.SelectedDate = null;
                ddlNoofOccurances.SelectedIndex = 0;
                dtpPriorHistoryDate.SelectedDate = null;
                txtRelievingFactors.Text = "";
                txtAggrevatingFactors.Text = "";
                cmbAdd1.Text = "";
                cmbAdd2.Text = "";
                cmbAdd3.Text = "";
                cmbAdd4.Text = "";
                cmbAdd5.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void btnBack_OnClick(object sender, EventArgs e)
    {
        //if (ViewState["Page"] != null)
        //{
        //    if (ViewState["Page"].ToString().ToLower() == "oldcomplaint")
        //    {
        //        Response.Redirect("PatientOldComplaints.aspx", false);
        //    }
        //}
        //else
        //{
        //    Response.Redirect("Default.aspx", false);
        //}
        if (common.myStr(Request.QueryString["MP"]).ToUpper().Equals("NO"))
        {
            Response.Redirect("/EMR/Problems/Default.aspx?From=POPUP");
        }
        else
        {
            Response.Redirect("/EMR/Problems/Default.aspx");
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //BaseC.Patient bC = new BaseC.Patient(sConString);
            //hstInput = new Hashtable();
            //Hashtable hshOutput = new Hashtable();
            //hstInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            //hstInput.Add("@intProblemID", ddlProblems.SelectedValue);
            //if (dtpOnsetDate.DateInput.Text != "")
            //{
            //    hstInput.Add("@chvOnsetDate", Convert.ToString(dtpOnsetDate.SelectedDate.Value.ToString("dd/MM/yyyy")));
            //}
            //hstInput.Add("@inyNoOfOccurrence", ddlNoofOccurances.SelectedValue);
            //if (dtpPriorHistoryDate.DateInput.Text != "")
            //{
            //    hstInput.Add("@chvPriorIllnessDate", Convert.ToString(dtpPriorHistoryDate.SelectedDate.Value.ToString("dd/MM/yyyy")));
            //}
            //hstInput.Add("@chvRelievingFactors", Convert.ToString(txtRelievingFactors.Text).Trim());
            //hstInput.Add("@chvAggravatingFactors", Convert.ToString(txtAggrevatingFactors.Text).Trim());
            //if (cmbAdd1.SelectedValue != "" || cmbAdd1.Text != "")
            //{
            //    hstInput.Add("@chvDeniesSymptomsText1", cmbAdd1.Text);
            //}
            //if (cmbAdd2.SelectedValue != "" || cmbAdd2.Text != "")
            //{
            //    hstInput.Add("@chvDeniesSymptomsText2", cmbAdd2.Text);
            //}
            //if (cmbAdd3.SelectedValue != "" || cmbAdd3.Text != "")
            //{

            //    hstInput.Add("@chvDeniesSymptomsText3", cmbAdd3.Text);
            //}
            //if (cmbAdd4.SelectedValue != "" || cmbAdd4.Text != "")
            //{
            //    hstInput.Add("@chvDeniesSymptomsText4", cmbAdd4.Text);
            //}
            //if (cmbAdd5.SelectedValue != "" || cmbAdd5.Text != "")
            //{
            //    hstInput.Add("@chvDeniesSymptomsText5", cmbAdd5.Text);
            //}
            //hstInput.Add("@intEncodedBy", Session["UserId"]);
            //hstInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));//@intRegistrationId
            //hstInput.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));

            //hstInput.Add("@intLoginFacilityId", Session["FacilityID"]);
            //hstInput.Add("@intPageId", ViewState["PageId"]);
            //hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            //hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveHPIProblems", hstInput, hshOutput);

            //if (Convert.ToString(hshOutput["intEncounterID"]) != "")
            //{
            //    lbl_Msg.Text = hshOutput["intEncounterID"].ToString();
            //}
            //else
            //{
            //    lbl_Msg.Text = "Data Saved Successfully.";
            //}


            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveHPIProblems";
            APIRootClass.SaveHPIProblems objRoot = new global::APIRootClass.SaveHPIProblems();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.ProblemId = common.myInt(ddlProblems.SelectedValue);
            
            if (dtpOnsetDate.DateInput.Text != "")
            {
                objRoot.OnsetDate = Convert.ToString(dtpOnsetDate.SelectedDate.Value.ToString("dd/MM/yyyy"));
            }
            objRoot.NoOfOccurrence = ddlNoofOccurances.SelectedValue;
            if (dtpPriorHistoryDate.DateInput.Text != "")
                objRoot.PriorIllnessDate = Convert.ToString(dtpPriorHistoryDate.SelectedDate.Value.ToString("dd/MM/yyyy"));
            objRoot.RelievingFactors = txtRelievingFactors.Text;
            objRoot.AggravatingFactors = txtAggrevatingFactors.Text;
            if (cmbAdd1.SelectedValue != "" || cmbAdd1.Text != "")
                objRoot.DeniesSymptomsText1 = cmbAdd1.Text;
            if (cmbAdd2.SelectedValue != "" || cmbAdd2.Text != "")
                objRoot.DeniesSymptomsText2 = cmbAdd2.Text;
            if (cmbAdd3.SelectedValue != "" || cmbAdd3.Text != "")
                objRoot.DeniesSymptomsText3 = cmbAdd3.Text;
            if (cmbAdd4.SelectedValue != "" || cmbAdd4.Text != "")
                objRoot.DeniesSymptomsText4 = cmbAdd4.Text;
            if (cmbAdd5.SelectedValue != "" || cmbAdd5.Text != "")
                objRoot.DeniesSymptomsText5 = cmbAdd5.Text;
            objRoot.UserId = common.myInt(Session["UserId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationID"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.PageId = common.myInt(Session["PageId"]);

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            if (sValue.Contains("Saved"))
            {
                lbl_Msg.ForeColor  = System.Drawing.Color.Green;
                lbl_Msg.Text = sValue;
            }
            else
            {
                lbl_Msg.Text = sValue;
            }

        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void cmb_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = sender as RadComboBox;
        GridViewRow row = ddl.NamingContainer as GridViewRow;

        DataTable data = BindSearchProblemCombo(e.Text);

        int itemOffset = e.NumberOfItems;
        if (itemOffset == 0)
        {
            ddl.Items.Clear();
        }
        int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;

        for (int i = itemOffset; i < endOffset; i++)
        {
            //RadCmbPatientSearch.Items.Add(new RadComboBoxItem(data.Rows[i]["CompanyName"].ToString(), data.Rows[i]["CompanyName"].ToString()));
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = (string)data.Rows[i]["ProblemDescription"];
            item.Value = data.Rows[i]["ProblemId"].ToString();
            //item.Attributes["SNOMEDCode"] = data.Rows[i]["SNOMEDCode"].ToString();
            ddl.Items.Add(item);
            item.DataBind();
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);
    }

    protected DataTable BindSearchProblemCombo(String etext)
    {
        DataSet ds = new DataSet();
        try
        {
            //string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Hashtable hshin = new Hashtable();
            //if (etext.ToString().Trim() != "")
            //{

            //    hshin.Add("@chvSearchCriteria", "%" + etext + "%");
            //}
            //hshin.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetProblemsList", hshin);

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetProblemsList";
            APIRootClass.GetProblemsList objRoot = new global::APIRootClass.GetProblemsList();
            objRoot.SearchText = "%" + etext + "%";
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        return ds.Tables[0];
    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

}
