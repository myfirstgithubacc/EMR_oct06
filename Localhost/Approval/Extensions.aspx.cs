using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class Newpreauth : System.Web.UI.Page
{
    private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];

    //string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["RegistrationNo"] != null)
                {
                    txtUHID.Text = common.myStr(Session["RegistrationNo"]);
                    txtUHID_TextChanged(null, null);
                    btnsubmit.Text = "Save";
                }
                if (common.myStr(Request.QueryString["Call"]) == "Nurse")
                {

                    rbpending.SelectedIndex = common.myInt(Request.QueryString["opt"]);
                    btnHistory_Click(null, null);
                }
                //if (Session["Preauth"] == null)
                //{
                //    Alert.ShowAjaxMsg("Error! Please Check Input Data", this);
                //    return;
                //}
            }
        }
        catch(Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
    }
    void filldatapayer(string EncounteRID)
    {
        DataSet objDs = new DataSet();
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet dt = dl.FillDataSet(CommandType.Text, "select Distinct CompanyID,Name from company with(nolock) where active=1 and IsInsuranceCompany=1 ORder by Name asc; exec uspGetEMRdataforPreauth @encounterID=" + common.myInt(EncounteRID).ToString());
            string strSql = "select Distinct CompanyID,Name from company with(nolock) where active=1 and IsInsuranceCompany=1 ORder by Name asc; exec uspGetEMRdataforPreauth @encounterID=" + common.myInt(EncounteRID).ToString();
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                ddlpayer.DataSource = objDs.Tables[0];
                ddlpayer.DataValueField = "CompanyID";
                ddlpayer.DataTextField = "Name";
                ddlpayer.DataBind();

                ddlsponsor_SelectedIndexChanged(null, null);
                ddlpayer_SelectedIndexChanged(null, null);

                txtcardNo.Text = common.myStr(objDs.Tables[2].Rows[0]["CardNo"]);
                txtreatmentdetail.Text = common.myStr(objDs.Tables[1].Rows[0]["Description"]);
                //ddlsponsor.SelectedValue = common.myStr(dt.Tables[2].Rows[0]["SponsorId"]);
                ddlpayer.SelectedValue = common.myStr(objDs.Tables[2].Rows[0]["PayerId"]);
                ddlCard.SelectedValue = common.myStr(objDs.Tables[2].Rows[0]["CardId"]);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            objDs.Dispose();
        }
    }
    private DataTable getCurrentICDCodes()
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        try
        {

            //BaseC.EMROrders
            //order = new BaseC.EMROrders(sConString);
            //DataSet ds = order.GetPatientDiagnosis(Convert.ToInt32(hdnregid.Value), Convert.ToInt16(Session["HospitalLocationID"]),
            //   common.myInt(hdnencounter.Value));
            String sICDCodes = "";

            dt.Columns.Add("DiagnosisID");
            dt.Columns.Add("DiagnosisName");
            dt.AcceptChanges();

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientDiagnosis";
            APIRootClass.GetPatientDiagnosis objRoot = new global::APIRootClass.GetPatientDiagnosis();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = 0;
            objRoot.RegistrationId = common.myInt(hdnregid.Value);
            objRoot.EncounterId = common.myInt(hdnencounter.Value);
            objRoot.DoctorId = 0;
            objRoot.DiagnosisGroupId = 0;
            objRoot.DiagnosisSubGroupId = 0;
            objRoot.DateRange = "";
            objRoot.FromDate = "";
            objRoot.ToDate = "";
            objRoot.SearchKeyword = "%%";
            objRoot.IsDistinct = false;
            objRoot.StatusId = 0;
            objRoot.VisitType = "";
            objRoot.IsChronic = false;
            objRoot.DiagnosisId = 0;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["DiagnosisID"] = ds.Tables[0].Rows[i]["ICDId"];
                dr["DiagnosisName"] = ds.Tables[0].Rows[i]["ICDDescription"];
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }
            GvDiagnosis.DataSource = dt;
            GvDiagnosis.DataBind();

        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            ds.Dispose();
        }
        return dt;
    }
    void binddata()
    {
        DataTable dt = Session["DtServ"] as DataTable;
        GvServices.DataSource = dt;
        GvServices.DataBind();
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (uptfiles.PostedFiles.Count > 0)
        {
            btnupload_Click(sender, e);
        }
        if (txtMobileNo.Text.Length < 9 || txtMobileNo.Text.Length > 10)
        {
            lblerror.Text = "Please Enter Valid UAE MobileNo";
            lblerror.ForeColor = System.Drawing.Color.Red;
            txtMobileNo.Focus();
            return;
        }
        if (txtName.Text.Length < 5)
        {
            lblerror.Text = "Please Enter Valid Patient Name";
            lblerror.ForeColor = System.Drawing.Color.Red;
            txtName.Focus();
            return;
        }
        //if (txtcardNo.Text.Length < 3)
        //{
        //    lblerror.Text = "Please Enter Valid Card Number";
        //    lblerror.ForeColor = System.Drawing.Color.Red;
        //    txtcardNo.Focus();
        //    return;
        //}
        //DateTime dtx;

        //DateTime.TryParse(txtEdd.Text.ToString().Replace("-", "/"), out dtx);

        //if (dtx < DateTime.Now)
        //{
        //    lblerror.Text = "Please Enter Valid EDD";
        //    lblerror.ForeColor = System.Drawing.Color.Red;
        //    txtEdd.Focus();
        //    return;
        //}
        //if ((Session["DtDiagnosis"] as DataTable).Rows.Count == 0)
        //{
        //    lblerror.Text = "Please Select Valid ICD";
        //    lblerror.ForeColor = System.Drawing.Color.Red;
        //    RadDiagnosis.Focus();
        //    return;
        //}
        //if ((Session["DtServ"] as DataTable).Rows.Count == 0)
        //{
        //    lblerror.Text = "Please Select Valid CPT's";
        //    lblerror.ForeColor = System.Drawing.Color.Red;
        //    RadServices.Focus();
        //    return;
        //}

        btnSave_Click(null, null);
        //dvfinalconfirmation.Visible = true;
    }

    protected void btnencounterxxlose_Click(object sender, EventArgs e)
    {
        //dvfinalconfirmation.Visible = false;
    }

    protected void grdfiles_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void txtEmirateID_TextChanged(object sender, EventArgs e)
    {
        DataSet objDs = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataTable dt = dl.FillDataSet(CommandType.Text, "select RegistrationNo,MobileNo, EmiratesID,FirstName+Isnull(' '+Middlename,'')+isnull(' '+LastNAme,'') as Name,PayorId from Registration with(nolock) where replace(emirateID,'-','')='" + txtEmirateID.Text + "'").Tables[0];
            string strSql = "select RegistrationNo,MobileNo, EmiratesID,FirstName+Isnull(' '+Middlename,'')+isnull(' '+LastNAme,'') as Name,PayorId from Registration with(nolock) where replace(emirateID,'-','')='" + txtEmirateID.Text + "'";
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                dt = objDs.Tables[0];
            }
            if (dt.Rows.Count > 0)
            {
                txtMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                txtUHID.Text = dt.Rows[0]["RegistrationNo"].ToString();
                txtName.Text = dt.Rows[0]["Name"].ToString();
                ddlpayer.SelectedValue = dt.Rows[0]["PayorId"].ToString();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            dt.Dispose();
            objDs.Dispose();
        }
    }

    protected void txtUHID_TextChanged(object sender, EventArgs e)
    {
        DataSet objDs = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataTable dt = dl.FillDataSet(CommandType.Text, "UspGetPtdetailsForExt @regNo =" + common.myInt(txtUHID.Text)).Tables[0];

            string strSql = "UspGetPtdetailsForExt @regNo =" + common.myInt(txtUHID.Text);
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                dt = objDs.Tables[0];
            }
            if (dt.Rows.Count > 0)
            {
                filldatapayer(dt.Rows[0]["EncounterID"].ToString());
                txtEmirateID.Text = dt.Rows[0]["EmiratesID"].ToString();
                txtMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                txtName.Text = dt.Rows[0]["Name"].ToString();
                txtEdd.Text = common.myDate(dt.Rows[0]["AdmissionDate"]).ToString("dd/MM/yyyy");
                txtIPno.Text = dt.Rows[0]["EncounterNo"].ToString();
                
                //ddlpayer.SelectedValue = dt.Rows[0]["PayorId"].ToString();
                hdnregid.Value = common.myInt(dt.Rows[0]["RegistrationID"].ToString()).ToString();
                hdnencounter.Value = common.myInt(dt.Rows[0]["EncounterID"].ToString()).ToString();
                Session["EncounterID"] = common.myInt(dt.Rows[0]["EncounterID"].ToString()).ToString();
                //Session["DtServ"] = (Session["Preauth"] as DataTable);
                //GetsetPAtDAta();
                binddata();
                Session["DtDiagnosis"] = getCurrentICDCodes();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            dt.Dispose();
            objDs.Dispose();
        }
    }

    protected void txtMobileNo_TextChanged(object sender, EventArgs e)
    {
        DataSet objDs = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataTable dt = dl.FillDataSet(CommandType.Text, "select RegistrationNo,MobileNo, EmiratesID,FirstName+Isnull(' '+Middlename,'')+isnull(' '+LastNAme,'') as Name,PayorId from Registration with(nolock) where replace(Mobileno,'-','')='" + txtMobileNo.Text + "'").Tables[0];

            string strSql = "select RegistrationNo,MobileNo, EmiratesID,FirstName+Isnull(' '+Middlename,'')+isnull(' '+LastNAme,'') as Name,PayorId from Registration with(nolock) where replace(Mobileno,'-','')='" + txtMobileNo.Text + "'";
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                dt = objDs.Tables[0];
            }
            if (dt.Rows.Count > 0)
            {
                txtEmirateID.Text = dt.Rows[0]["EmiratesID"].ToString();
                txtUHID.Text = dt.Rows[0]["RegistrationNo"].ToString();
                txtName.Text = dt.Rows[0]["Name"].ToString();
                ddlpayer.SelectedValue = dt.Rows[0]["PayorId"].ToString();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            dt.Dispose();
            objDs.Dispose();
        }
    }

    protected void btnAddtoList_Click(object sender, EventArgs e)
    {
        DataTable dt;
        if (Session["DtDiagnosis"] == null)
        {
            dt = new DataTable();
            dt.Columns.Add("DiagnosisID");
            dt.Columns.Add("DiagnosisName");
            Session["DtDiagnosis"] = dt;
        }
        dt = Session["DtDiagnosis"] as DataTable;

        int found = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["DiagnosisID"].ToString() == RadDiagnosis.SelectedValue)
            {
                GvDiagnosis.DataSource = dt;
                GvDiagnosis.DataBind();
                found = 1;
                break;
            }
        }
        if (found != 1)
        {
            DataRow dr = dt.NewRow();
            dr["DiagnosisID"] = RadDiagnosis.SelectedValue;
            dr["DiagnosisName"] = RadDiagnosis.Text;

            dt.Rows.Add(dr);
            GvDiagnosis.DataSource = dt;
            GvDiagnosis.DataBind();
        }
    }

    protected void btnAddServ_Click(object sender, EventArgs e)
    {
        DataTable dt;
        if (Session["DtServ"] == null)
        {
            dt = new DataTable();
            dt.Columns.Add("ServiceID");
            dt.Columns.Add("ServiceName");
            Session["DtServ"] = dt;
        }

        dt = Session["DtServ"] as DataTable;
        int found = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["ServiceID"].ToString() == RadServices.SelectedValue)
            {
                found = 1;
                break;
            }
        }
        if (found != 1)
        {
            DataRow dr = dt.NewRow();
            dr["ServiceID"] = RadServices.SelectedValue;
            dr["ServiceName"] = RadServices.Text;
            dt.Rows.Add(dr);
            GvServices.DataSource = dt;
            GvServices.DataBind();
        }
    }

    protected void RadDiagnosis_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        DataSet objDs = new DataSet();
        try
        {
            if (e.Text.Length > 1)
            {
                //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //DataSet dt = dl.FillDataSet(CommandType.Text, "select top 50 ICDId,Description +'['+ICDCode+']' ICDName  From ICD9SubDisease with(nolock) where Description +'['+ICDCode+']' like '%" + e.Text + "%'");

                string strSql = "select top 50 ICDId,Description +'['+ICDCode+']' ICDName  From ICD9SubDisease with(nolock) where Description +'['+ICDCode+']' like '%" + e.Text + "%'";
                //objDs = dl.FillDataSet(CommandType.Text, strSql);
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
                APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
                objRoot.Query = strSql;
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
                if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
                {
                    Session["DT"] = objDs;
                    RadDiagnosis.DataSource = objDs.Tables[0];
                    RadDiagnosis.DataValueField = "ICDId";
                    RadDiagnosis.DataTextField = "ICDName";
                    RadDiagnosis.DataBind();
                }

               
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            objDs.Dispose();
        }
    }

    protected void RadServices_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        DataSet objDs = new DataSet();
        try
        {
            if (e.Text.Length > 1)
            {
                //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //DataSet dt = dl.FillDataSet(CommandType.Text, "select top 50 ServiceId,ServiceName+'['+CPTCode+']' as ServiceName From ItemOfService with(nolock) where Active =1 and ServiceId in (select ServiceId from FacilityWiseItemOfService where FacilityId =9) and ServiceName+'['+CPTCode+']' like '%" + e.Text + "%' order by ServiceName ");
                string strSql = "select top 50 ServiceId,ServiceName+'['+CPTCode+']' as ServiceName From ItemOfService with(nolock) where Active =1 and ServiceId in (select ServiceId from FacilityWiseItemOfService where FacilityId =9) and ServiceName+'['+CPTCode+']' like '%" + e.Text + "%' order by ServiceName ";
                //objDs = dl.FillDataSet(CommandType.Text, strSql);
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
                APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
                objRoot.Query = strSql;
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
                if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
                {
                    Session["DT"] = objDs;
                    RadServices.DataSource = objDs.Tables[0];
                    RadServices.DataValueField = "ServiceId";
                    RadServices.DataTextField = "ServiceName";
                    RadServices.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            objDs.Dispose();
        }

    }

    protected void ddlsponsor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet objDs = new DataSet();
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet dt = dl.FillDataSet(CommandType.Text, "selecT NAme, ShortName, CardId, CardType, NetworkType  From VwInsuranceNetworkCategory with(nolock) where CompanyId = " + ddlpayer.SelectedValue.ToString() + "");
            string strSql = "selecT NAme, ShortName, CardId, CardType, NetworkType  From VwInsuranceNetworkCategory with(nolock) where CompanyId = " + ddlpayer.SelectedValue.ToString() + "";
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                ddlCard.DataSource = objDs.Tables[0];
                ddlCard.DataValueField = "CardId";
                ddlCard.DataTextField = "CardType";
                ddlCard.DataBind();
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            objDs.Dispose();
        }
    }
    protected void ddlpayer_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet objDs = new DataSet();
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet dt = dl.FillDataSet(CommandType.Text, "EXEC [uspGetCompanyList] 1,'IC'," + ddlpayer.SelectedValue.ToString() + ", 9");


            string strSql = "EXEC [uspGetCompanyList] 1,'IC'," + ddlpayer.SelectedValue.ToString() + ", " + common.myInt(Session["FacilityId"]).ToString();
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                ddlsponsor.DataSource = objDs.Tables[0];
                ddlsponsor.DataTextField = "Name";
                ddlsponsor.DataValueField = "CompanyID";
                ddlsponsor.DataBind();
                ddlsponsor_SelectedIndexChanged(null, null);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            objDs.Dispose();
        }
    }
    private bool CreateFTPDirectory(string directory, string username, string pwd)
    {

        try
        {
            //create the directory
            FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(directory));
            requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
            requestDir.Credentials = new NetworkCredential(username, pwd);
            requestDir.UsePassive = true;
            requestDir.UseBinary = true;
            requestDir.KeepAlive = false;
            FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
            Stream ftpStream = response.GetResponseStream();

            ftpStream.Close();
            response.Close();

            return true;
        }
        catch (WebException ex)
        {
            FtpWebResponse response = (FtpWebResponse)ex.Response;
            if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
            {
                response.Close();
                return true;
            }
            else
            {
                response.Close();
                return false;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
            return false;
        }
    }
    public bool FtpDirectoryExists(string directoryPath, string ftpUser, string ftpPassword)
    {
        bool IsExists = true;
        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryPath);
            request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == FtpStatusCode.DataAlreadyOpen)
                    IsExists = true;
            }


        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    IsExists = false;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        return IsExists;
    }
    private void UploadtoFTP(FileUpload FileUpload1, string fileName)
    {
        Rootfolder = Rootfolder+"IPExtAtt/";
        //FTP Server URL.
        string ftp = ftppath.Split('!')[0].ToString();
        //Checck Directory
        if (!FtpDirectoryExists(ftppath.Split('!')[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString() + "/" + txtUHID.Text, ftppath.Split('!')[1].ToString(), ftppath.Split('!')[2].ToString()))
        {
            CreateFTPDirectory(ftppath.Split('!')[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString() + "/" + txtUHID.Text + "/", ftppath.Split('!')[1].ToString(), ftppath.Split('!')[2].ToString());
        }
        //FTP Folder name. Leave blank if you want to upload to root folder.
        string ftpFolder = Rootfolder;


        try
        {
            for (int i = 0; i < FileUpload1.PostedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = FileUpload1.PostedFiles[i];
                FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(ftppath.Split('!')[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString() + "/" + txtUHID.Text + "/" + fileName +Request.Files[i].FileName);
                ftpClient.Credentials = new System.Net.NetworkCredential(ftppath.Split('!')[1].ToString(), ftppath.Split('!')[2].ToString());
                ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
                ftpClient.UseBinary = true;
                ftpClient.KeepAlive = true;

                ftpClient.ContentLength = FileUpload1.PostedFiles[i].ContentLength;
                byte[] buffer = new byte[4097];
                // int bytes = 0;
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(Request.Files[i].InputStream))
                {
                    fileData = binaryReader.ReadBytes(Request.Files[i].ContentLength);
                }
                int total_bytes = (int)fileData.Length;
                //byte[] filebyte = FileUpload1.PostedFiles[i].ContentLength;
                using (Stream requestStream = ftpClient.GetRequestStream())
                {
                    requestStream.Write(fileData, 0, fileData.Length);
                    requestStream.Close();
                }

                FtpWebResponse response = (FtpWebResponse)ftpClient.GetResponse();

                FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
                string value = uploadResponse.StatusDescription;
                uploadResponse.Close();

                lblMessage.Text += fileName + " uploaded.<br />";
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
    }
    protected void btnupload_Click(object sender, EventArgs e)
    {

        DataTable dtx = new DataTable();
        try
        {
            DateTime dtn = DateTime.Now;
            if (uptfiles.PostedFiles.Count == 0)
            {
                return;
            }
            for (int i = 0; i < uptfiles.PostedFiles.Count; i++)
            {
                if (uptfiles.PostedFiles[i].FileName.Length > 0)
                {
                    if (ViewState["Filenames"] == null)
                    {
                        dtx = new DataTable();
                        dtx.Columns.Add("Filename");
                        dtx.Columns.Add("Filenpath");
                        ViewState["Filenames"] = dtx;
                    }
                    DataTable dt = (ViewState["Filenames"] as DataTable);
                    int found = 0;
                    for (int ix = 0; ix < dt.Rows.Count; ix++)
                    {
                        if (dt.Rows[ix]["Filename"].ToString() == uptfiles.PostedFile.FileName)
                        {
                            found = 1;
                            break;
                        }
                    }
                    if (found == 0)
                    {
                        DataRow dr = dt.NewRow();
                        string fln = uptfiles.PostedFiles[i].FileName;
                        string filename = uptfiles.PostedFiles[i].FileName;//Server.MapPath("/PatientDocuments/PreAuth") + "/" + fln;
                        //uptfiles.SaveAs(filename);
                        dr["Filename"] = dtn.ToString("ddMMyyyyHHMM")+"_"+fln;
                        dr["Filenpath"] = dtn.ToString("ddMMyyyyHHMM") + "_" + filename;
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                        ViewState["Filenames"] = dt;
                    }
                    grdfiles.DataSource = dt;
                    grdfiles.DataBind();
                }
            }
            UploadtoFTP(uptfiles, dtn.ToString("ddMMyyyyHHMM") + "_");
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {

            dtx.Dispose();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataSet objDs = new DataSet();
        try
        {
            //if ((ViewState["Filenames"] as DataTable).Rows.Count == 0)
            //{
            //    lblmessages.Text = "Please Add Files";
            //    return;
            //}
            DataTable dt = Session["DtDiagnosis"] as DataTable;
            //string ICDcode = "";
            //string serviN = "";
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (ICDcode == "")
            //    {
            //        ICDcode = dt.Rows[i][0].ToString();
            //    }
            //    else
            //    {
            //        ICDcode = ICDcode + "," + dt.Rows[i][0].ToString();
            //    }

            //}
            //dt = Session["DtServ"] as DataTable;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (serviN == "")
            //    {
            //        serviN = dt.Rows[i][0].ToString();
            //    }
            //    else
            //    {
            //        serviN = serviN + "," + dt.Rows[i][0].ToString();
            //    }

            //}
            string Filename = "";
             
                dt = ViewState["Filenames"] as DataTable;
            
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Filename == "")
                        {
                            Filename = dt.Rows[i][1].ToString();
                        }
                        else
                        {
                            Filename = Filename + "!" + dt.Rows[i][1].ToString();
                        }

                    }
                }

            //string str = "";
            //str = "exec uspSaveExtension";
            //str = str + " @extID=" + common.myInt(Session["ExtID"]) + ",@EncounterID='" + common.myInt(Session["EncounterID"]) + "',@Reason_for_Exnsion='" + txtExtensionReason.Text.Replace("'", "''") + "',@Chief_Complaint='" + txtreatmentdetail.Text.Replace("'", "''") + "',@filename='" + Filename + "',@EncodedBy='" + Session["UserId"].ToString() + "',@ExtensionFor=" + common.myInt(txtDays.Text) + ",@ExtensionForType=" + common.myInt(ddlextfor.SelectedValue) ;
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet ds = dl.FillDataSet(CommandType.Text, str);
            ////dl.ExecuteNonQuery(CommandType.Text, str);

            ////Response.Redirect("Status");
            ////btnSave.Enabled = false;

            string strSql = "exec uspSaveExtension @extID=" + common.myInt(Session["ExtID"]) + ",@EncounterID='" + common.myInt(Session["EncounterID"]) + "',@Reason_for_Exnsion='" + txtExtensionReason.Text.Replace("'", "''") + "',@Chief_Complaint='" + txtreatmentdetail.Text.Replace("'", "''") + "',@filename='" + Filename + "',@EncodedBy='" + Session["UserId"].ToString() + "',@ExtensionFor=" + common.myInt(txtDays.Text) + ",@ExtensionForType=" + common.myInt(ddlextfor.SelectedValue);
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
                Response.Write(objDs.Tables[0].Rows[0][0].ToString());

        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            objDs.Dispose();
        }
    }
    static string myStr(object strVal)
    {
        string retVal = string.Empty;
        try
        {
            if (strVal != null && strVal != DBNull.Value)
            {
                retVal = Convert.ToString(strVal);
            }
        }
        catch
        {
        }
        return retVal;
    }
    static bool isNumeric(object num)
    {
        try
        {
            double retNum;
            return Double.TryParse(myStr(num), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        }
        catch
        {
        }
        return false;
    }
    static double myDbl(object num)
    {
        try
        {
            string strVal = myStr(num).Trim();
            if (isNumeric(strVal))
            {
                return Convert.ToDouble(strVal);
            }
        }
        catch
        {
        }
        return 0;
    }
    static int myInt(object num)
    {
        try
        {

            string strVal = myStr(num).ToString();
            if (isNumeric(strVal))
            {
                return int.Parse(Convert.ToString((int)myDbl(strVal)));
            }
        }
        catch
        {
        }
        return 0;
    }
    protected void btnHistory_Click(object sender, EventArgs e)
    {
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //DataTable dt=dl.FillDataSet(CommandType.Text, "Exec UspGetPtdetailssavedExtHistory @UHID="+common.myInt(Session["RegistrationNo"]) + ",@status=" + rbpending.SelectedIndex.ToString()).Tables[0];
        DataSet objDs = new DataSet();
        try
        {
            string strSql = "Exec UspGetPtdetailssavedExtHistory @UHID=" + common.myInt(Session["RegistrationNo"]) + ",@status=" + rbpending.SelectedIndex.ToString();
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                gvhistory.DataSource = objDs.Tables[0];
                gvhistory.DataBind();
                dvHistory.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            objDs.Dispose();
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {

    }
    protected void btnselect_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Session["ExtID"] = common.myInt((sender as Button).CommandName).ToString();
            hdnextensionid.Value = common.myInt((sender as Button).CommandName).ToString();
            //DataSet ds= dl.FillDataSet(CommandType.Text, "UspGetPtdetailssavedExt @extID =" + common.myInt((sender as Button).CommandName));

            string strSql = "UspGetPtdetailssavedExt @extID =" + common.myInt((sender as Button).CommandName);
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
            {
                dt = ds.Tables[2];
                if (dt.Rows.Count > 0)
                {
                    txtUHID.Text = dt.Rows[0]["RegistrationNo"].ToString();
                    filldatapayer(dt.Rows[0]["EncounterID"].ToString());
                    txtEmirateID.Text = dt.Rows[0]["EmiratesID"].ToString();
                    txtMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                    txtName.Text = dt.Rows[0]["Name"].ToString();
                    txtEdd.Text = common.myDate(dt.Rows[0]["AdmissionDate"]).ToString("dd/MM/yyyy");
                    txtIPno.Text = dt.Rows[0]["EncounterNo"].ToString();
                    //ddlpayer.SelectedValue = dt.Rows[0]["PayorId"].ToString();
                    hdnregid.Value = common.myInt(dt.Rows[0]["RegistrationID"].ToString()).ToString();
                    hdnencounter.Value = common.myInt(dt.Rows[0]["EncounterID"].ToString()).ToString();
                    Session["EncounterID"] = common.myInt(dt.Rows[0]["EncounterID"].ToString()).ToString();
                    //Session["DtServ"] = (Session["Preauth"] as DataTable);
                    //GetsetPAtDAta();
                    //binddata();
                    txtreatmentdetail.Text = ds.Tables[0].Rows[0]["Chief_Complaint"].ToString();
                    txtExtensionReason.Text = ds.Tables[0].Rows[0]["Reason_for_Exnsion"].ToString();
                    txtDays.Text = ds.Tables[0].Rows[0]["ExtensionFor"].ToString();
                    ddlextfor.SelectedValue = ds.Tables[0].Rows[0]["ExtensionForType"].ToString();
                    Session["DtDiagnosis"] = getCurrentICDCodes();
                    dvHistory.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objexp = new clsExceptionLog();
            objexp.HandleException(Ex);
            objexp = null;
        }
        finally
        {
            ds.Dispose();
            dt.Dispose();
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        dvHistory.Visible = false;
    }
    protected void rbpending_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnHistory_Click(null, null);
    }
}