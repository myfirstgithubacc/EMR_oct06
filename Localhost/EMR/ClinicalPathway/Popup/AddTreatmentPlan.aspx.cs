using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.IO;

public partial class EMR_ClinicalPathway_AddTreatmentPlan : System.Web.UI.Page
{
    private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string Splitter = ConfigurationManager.AppSettings["Split"];
    private string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPlanTypeDuration();
            BindDropDownServiceCategories();
            bindEMRGetTreatmentPlanList();
            BindTemplate();
            bindBlankGrid();
            rdoPlanType_SelectedIndexChanged(this, null);
        }
    }

    private void BindProvider()
    {
        BaseC.EMROrders objEMR_Order = new BaseC.EMROrders(sConString);
        DataSet objDs = new DataSet();
        try
        {
            ddlProvider.Items.Clear();
            objDs = objEMR_Order.fillDoctorCombo(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(Session["FacilityID"]), 0);
            ddlProvider.DataSource = objDs.Tables[0];
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataBind();
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            objDs.Dispose();
            objEMR_Order = null;
        }
    }


    private void BindTemplate()
    {
        DataSet ds = new DataSet();
        clsIVF emr = new clsIVF(sConString);
        try
        {
            ds=emr.getEMRTemplateTypeWise(Convert.ToInt32(Session["HospitalLocationID"]), "CP", "");
            if(ds.Tables[0].Rows.Count>0)
            {
                ddlTemplateName.DataSource = ds.Tables[0];
                ddlTemplateName.DataTextField = "TemplateName";
                ddlTemplateName.DataValueField = "TemplateId";
                ddlTemplateName.DataBind();
            }
        }
        catch(Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            emr = null; ;
        }
    }
    private void BindDropDownServiceCategories()
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = order.GetServiceCategories(Convert.ToInt32(Session["HospitalLocationID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDepartment.DataSource = ds.Tables[0];
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            order = null;
            ds.Dispose();
        }
    }
    protected void lnkEdit_OnClik(object sender, EventArgs e)
    {
        try
        {
            GridDataItem row = (GridDataItem)(((LinkButton)sender).NamingContainer);
            HiddenField hdnPlanId = (HiddenField)row.FindControl("hdnPlanId");
            Label lblPlanName = (Label)row.FindControl("lblPlanName");
            HiddenField hdnDepartmentId = (HiddenField)row.FindControl("hdnDepartmentId");
            HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
            HiddenField hdnOPIP = (HiddenField)row.FindControl("hdnOPIP");
            HiddenField hdnDaysNO = (HiddenField)row.FindControl("hdnDays");

            
                 HiddenField hdnIsSurgical = (HiddenField)row.FindControl("hdnIsSurgical");
            HiddenField hdnTemplateId = (HiddenField)row.FindControl("hdnTemplateId");  
            Label lblDoctorName = (Label)row.FindControl("lblDoctorName");


            ddlDepartment.SelectedValue = hdnDepartmentId.Value;

            txtPlanName.Text = lblPlanName.Text;
            ViewState["SelectedPlanId"] = hdnPlanId.Value;

            
            BindTemplate();
            ddlTemplateName.SelectedValue = hdnTemplateId.Value;

            rdoPlanType.SelectedValue = hdnOPIP.Value;
            BindProvider();
            ddlProvider.SelectedValue = hdnDoctorId.Value;
            ddlProvider.Text = lblDoctorName.Text;
            chkIsSurgical.Checked = common.myBool(hdnIsSurgical.Value);
            chkIsSurgical_CheckedChanged(this, null);
            bindEMRGetTreatmentPlanDiagnosis(common.myInt(ViewState["SelectedPlanId"]));
            BindPlanTypeDuration();
        }
        catch (Exception ex)
        {

        }
        finally
        {
        }
    }

    protected void bindEMRGetTreatmentPlanDiagnosis(int PlanId)
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        try
        {
            bindBlankGrid();

         
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanDiagnosis";
          
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.PlanId = PlanId;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    ViewState["PlanDiagnosis"] = ds.Tables[0];
                    gvPlanDiagnosis.DataSource = ds.Tables[0];
                    gvPlanDiagnosis.DataBind();
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
            client = null;
            objRoot = null;
        }
    }

    protected void bindEMRGetTreatmentPlanList()
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        try
        {

           
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanList";
          
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvPlanNameLists.DataSource = ds.Tables[0];
                    gvPlanNameLists.DataBind();
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
            client = null;
            objRoot = null;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseC.Hospital bHos = new BaseC.Hospital(sConString);
        Hashtable hshOut = new Hashtable();
        ArrayList arr = new ArrayList();
        StringBuilder sb = new StringBuilder();

        ArrayList arrDia = new ArrayList();
        StringBuilder sbDia = new StringBuilder();

        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();

        try
        {
            if (txtPlanName.Text == "")
            {
                Alert.ShowAjaxMsg("Please type Plan Name", Page);
                return;
            }
            if (common.myStr(ddlTemplateName.SelectedValue) == "")
            {
                Alert.ShowAjaxMsg("Please select template name", Page);
                return;
            }
            foreach (GridViewRow item in gvPlanTypeDuration.Rows)
            {
                HiddenField hdnDayId = (HiddenField)item.FindControl("hdnDayId");
                TextBox txtDays = (TextBox)item.FindControl("txtDays");
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                HiddenField hdnCode = (HiddenField)item.FindControl("hdnCode");
                if (chkSelect.Checked)
                {
                    arr.Add(common.myInt(hdnDayId.Value));
                    arr.Add(common.myInt(ViewState["SelectedPlanId"]));
                    if (common.myStr(hdnCode.Value) == "DC")
                    {
                        arr.Add(1);
                    }
                    else
                    {
                        if (common.myStr(hdnCode.Value) == "AD" && common.myInt(txtDays.Text) == 0)
                        {
                            txtDays.Text = "1";
                            arr.Add(1);
                        }
                        else
                        {
                            arr.Add(common.myStr(txtDays.Text));
                        }
                    }

                    if (common.myInt(txtDays.Text) == 0 && common.myStr(hdnCode.Value) != "DC")
                    {
                        Alert.ShowAjaxMsg("Please type Days", Page);
                        return;
                    }
                    arr.Add(true);
                }
                else
                {
                    //arr.Add(false);
                }
                sb.Append(common.setXmlTable(ref arr));
            }

            foreach (GridViewRow item in gvPlanDiagnosis.Rows)
            {
                HiddenField hdnDiagnosisId = (HiddenField)item.FindControl("hdnDiagnosisId");
                arrDia.Add(common.myInt(hdnDiagnosisId.Value));
                arrDia.Add(common.myInt(ViewState["SelectedPlanId"]));
                sbDia.Append(common.setXmlTable(ref arrDia));
            }
            if (sbDia.ToString() == "")
            {
                Alert.ShowAjaxMsg("Please enter Diagnosis", Page);
                return;
            }

            if (sb.ToString() == "")
            {
                Alert.ShowAjaxMsg("Please enter Days", Page);
                return;
            }
            if (fFileUpload.FileName != "")
            {
                if (Path.GetExtension(fFileUpload.FileName) != ".pdf")
                {
                    Alert.ShowAjaxMsg("Please upload pdf file only", Page);
                    return;
                }
            }

            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveEMRTreatmentPlanName";

            objRoot.Id = common.myInt(ViewState["SelectedPlanId"]);
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.DepartmentId = common.myInt(ddlDepartment.SelectedValue);
            objRoot.PlanName = common.myStr(txtPlanName.Text);
            objRoot.TemplateId = common.myStr(ddlTemplateName.SelectedValue);

            objRoot.DoctorId = common.myInt(ddlProvider.SelectedValue);
            objRoot.PlanType = common.myStr(rdoPlanType.SelectedValue);
            objRoot.DocumentName = fFileUpload.FileName;

            objRoot.xmlDays = sb.ToString();
            objRoot.xmlDiagnosis = sbDia.ToString();

            

            objRoot.IsSurgical = common.myBool(chkIsSurgical.Checked);
            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            string[] message = sValue.Split('_');

            lblMessage.Text = message[0];
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            

            if (message[0].ToString().ToUpper().Contains("SAVE") || message[0].ToString().ToUpper().Contains("UPDATE"))
            {

                ViewState["PlanId"] = common.myStr(message[1]);

                if (fFileUpload.FileName != "" && common.myInt(ViewState["PlanId"]) > 0)
                {
                    UploadtoFTP(fFileUpload, fFileUpload.FileName, common.myStr(ViewState["PlanId"]));
                }

                lblMessage.Text = sValue;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindEMRGetTreatmentPlanList();
                BindBlankPlanTypeDuration();
                bindBlankGrid();
                txtPlanName.Text = "";
                ddlDepartment.SelectedIndex = -1;
                ddlDepartment.SelectedValue = "0";
                ddlDepartment.Text = "";
                ddlProvider.SelectedValue = "0";
                ddlProvider.Text = "";
                ddlDiagnosis.ClearSelection();
                ddlDiagnosis.Text = "";

                
                txtPlanName.Text = "";

                ddlTemplateName.SelectedValue = "0";
                ddlTemplateName.Text = "";
                ViewState["PlanDiagnosis"] = null;

            }
            ViewState["SelectedPlanId"] = 0;
            ViewState["PlanId"] = 0;
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
            hshOut = null;
            arr = null;
            sb = null;
        }
    }
    private void BindBlankPlanTypeDuration()
    {
        DataSet ds = new DataSet();
        try
        {
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetPlanTypeDurationMaster";
            APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
           // objRoot.PlanId = common.myInt(ViewState["SelectedPlanId"]);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPlanTypeDuration.DataSource = ds.Tables[0];
                gvPlanTypeDuration.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ds.Dispose();
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
        return IsExists;
    }
    private void UploadtoFTP(FileUpload FileUpload1, string fileName, string PlanId)
    {

        //FTP Server URL.
        var csplitter = Splitter.ToCharArray();
        string ftp = ftppath.Split(csplitter)[0].ToString();

        //Checck Directory
        if (!FtpDirectoryExists(ftppath.Split(csplitter)[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString() + "/" + PlanId, ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString()))
        {
            CreateFTPDirectory(ftppath.Split(csplitter)[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString() + "/" + PlanId + "/", ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString());
        }
        //FTP Folder name. Leave blank if you want to upload to root folder.
        //string ftpFolder = Rootfolder;


        try
        {

            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(ftppath.Split(csplitter)[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString() + "/" + PlanId + "/" + fileName);
            ftpClient.Credentials = new System.Net.NetworkCredential(ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString());
            ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            ftpClient.UseBinary = true;
            ftpClient.KeepAlive = true;

            ftpClient.ContentLength = FileUpload1.FileBytes.Length;
            byte[] buffer = new byte[4097];
            // int bytes = 0;
            int total_bytes = (int)FileUpload1.FileBytes.Length;

            using (Stream requestStream = ftpClient.GetRequestStream())
            {
                requestStream.Write(FileUpload1.FileBytes, 0, FileUpload1.FileBytes.Length);
                requestStream.Close();
            }

            FtpWebResponse response = (FtpWebResponse)ftpClient.GetResponse();

            FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
            string value = uploadResponse.StatusDescription;
            uploadResponse.Close();

            //lblMessage.Text += fileName + " uploaded.<br />";

        }
        catch (WebException ex)
        {
            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        }
    }
    protected void gvPlanNameLists_RowCommand(object sender, GridCommandEventArgs e)
    {

        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        try
        {
            if (e.CommandName == "Del")
            {
                HiddenField hdnPlanId = (HiddenField)e.Item.FindControl("hdnPlanId");

                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeleteTreatmentPlan";


                objRoot.EncodedBy= common.myInt(Session["UserID"]);
                objRoot.PlanId = common.myInt(hdnPlanId.Value);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                if (sValue == "1")
                {
                    lblMessage.Text = "Delete successfull";
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }
                bindEMRGetTreatmentPlanList();
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
            client = null;
            objRoot = null;
        }
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            BindProvider();
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
    public void ddlDiagnosiss_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            data = PopulateAllDiagnosis(e.Text);
            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ICDDescription"];
                item.Value = data.Rows[i]["ICDID"].ToString();
                item.Attributes["ICDCode"] = data.Rows[i]["ICDCode"].ToString();

                this.ddlDiagnosis.Items.Add(item);
                item.DataBind();
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
            data.Dispose();
        }

    }
    private DataTable PopulateAllDiagnosis(string txt)
    {
        BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        string strSearchCriteria = string.Empty;
        strSearchCriteria = "%" + txt + "%";
        return objDiag.BindDiagnosis(common.myInt(0), common.myInt(0), strSearchCriteria).Tables[0];
    }

    private void BindPlanTypeDuration()
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        try
        {
            
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetPlanTypeDurationDetails";
           
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.PlanId = common.myInt(ViewState["SelectedPlanId"]);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPlanTypeDuration.DataSource = ds.Tables[0];
                gvPlanTypeDuration.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ds.Dispose();
            client = null;
            objRoot = null;
        }
    }


    protected void gvPlanTypeDuration_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnCode = (HiddenField)e.Row.FindControl("hdnCode");
            TextBox txtDays = (TextBox)e.Row.FindControl("txtDays");
            HiddenField hdnPlanId = (HiddenField)e.Row.FindControl("hdnPlanId");
            CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
            if (common.myStr(hdnCode.Value) == "DC")
            {
                txtDays.Enabled = false;
            }
            chkSelect.Checked = false;
            if (common.myInt(hdnPlanId.Value) > 0)
            {
                chkSelect.Checked = true;
            }
            if (common.myStr(rdoPlanType.SelectedValue) == "I")
            {
                chkIsSurgical.Visible = true;
                if (hdnCode.Value == "AD" || hdnCode.Value == "DC")
                {
                    txtDays.Enabled = true;
                    chkSelect.Enabled = true;
                }
                if (hdnCode.Value == "PO")
                {
                    txtDays.Enabled = false;
                    chkSelect.Enabled = false;
                    if(chkIsSurgical.Checked)
                    {
                        txtDays.Enabled = true;
                        chkSelect.Enabled = true;
                    }
                }

                if (hdnCode.Value == "OP")
                {
                    txtDays.Enabled = false;
                    chkSelect.Enabled = false;
                    chkSelect.Checked = false;
                    txtDays.Text = "";
                }
            }
            else
            {
                chkIsSurgical.Visible = false;
                if (hdnCode.Value == "AD" || hdnCode.Value == "PO" || hdnCode.Value == "DC")
                {
                    txtDays.Enabled = false;
                    chkSelect.Enabled = false;

                }
                if (hdnCode.Value == "OP")
                {
                    txtDays.Text = "1";
                    chkSelect.Checked = true;
                    chkSelect.Enabled = true;

                }
            }
        }
    }

    private void bindBlankGrid()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("Description");
            dt.Columns.Add("DiagnosisId");
            DataRow dr = dt.NewRow();
            dr["Description"] = "";
            dr["DiagnosisId"] = "";
            dt.Rows.Add(dr);
            dt.AcceptChanges();
            gvPlanDiagnosis.DataSource = dt;
            gvPlanDiagnosis.DataBind();
        }
        catch (Exception ex)
        { }
        finally
        {
            dt.Dispose();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            if (ViewState["PlanDiagnosis"] != null)
            {
                dt = (DataTable)ViewState["PlanDiagnosis"];
                dv = new DataView(dt);
                dv.RowFilter = "DiagnosisId=" + common.myStr(ddlDiagnosis.SelectedValue);
                if (dv.ToTable().Rows.Count > 0)
                {
                    Alert.ShowAjaxMsg("Already added", Page);
                    return;
                }
            }
            else
            {
                dt.Columns.Add("Description");
                dt.Columns.Add("DiagnosisId");
            }
            DataRow dr = dt.NewRow();
            dr["Description"] = common.myStr(ddlDiagnosis.Text);
            dr["DiagnosisId"] = common.myStr(ddlDiagnosis.SelectedValue);
            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ViewState["PlanDiagnosis"] = dt;
            gvPlanDiagnosis.DataSource = dt;
            gvPlanDiagnosis.DataBind();

            ddlDiagnosis.SelectedValue = "0";
            ddlDiagnosis.Text = "";
        }
        catch (Exception ex)
        {

        }
        finally
        {
            dt.Dispose();
            dv.Dispose();
        }
    }

    

    protected void gvPlanDiagnosis_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        try
        {
            if (e.CommandName == "Del")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnDiagnosisId = (HiddenField)row.FindControl("hdnDiagnosisId");
                dt = (DataTable)ViewState["PlanDiagnosis"];
                dv = new DataView(dt);
                dv.RowFilter = "DiagnosisId<>" + common.myStr(hdnDiagnosisId.Value);
                ViewState["PlanDiagnosis"] = dv.ToTable();
                if (dv.ToTable().Rows.Count > 0)
                {
                    gvPlanDiagnosis.DataSource = dv.ToTable();
                    gvPlanDiagnosis.DataBind();
                }
                else
                {
                    bindBlankGrid();
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
            objEMR = null;
            dt.Dispose();
            dv.Dispose();
        }
    }

    protected void gvPlanNameLists_PreRender(object sender, EventArgs e)
    {
        bindEMRGetTreatmentPlanList();
    }

    protected void rdoPlanType_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkIsSurgical.Visible = false;

        if (common.myStr(rdoPlanType.SelectedValue) == "I")
        {
            chkIsSurgical.Visible = true;
            foreach (GridViewRow row in gvPlanTypeDuration.Rows)
            {
                HiddenField hdnCode = (HiddenField)row.FindControl("hdnCode");
                TextBox txtDays = (TextBox)row.FindControl("txtDays");
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (hdnCode.Value == "AD" || hdnCode.Value == "DC")
                {
                    txtDays.Enabled = true;
                    chkSelect.Enabled = true;
                }
                if (hdnCode.Value == "PO")
                {
                    txtDays.Enabled = false;
                    chkSelect.Enabled = false;
                }
                
                if (hdnCode.Value == "OP")
                {
                    txtDays.Enabled = false;
                    chkSelect.Enabled = false;
                    chkSelect.Checked = false;
                    txtDays.Text = "";
                }
            }
        }
        else
        {
            chkIsSurgical.Checked = false;
            foreach (GridViewRow row in gvPlanTypeDuration.Rows)
            {
                HiddenField hdnCode = (HiddenField)row.FindControl("hdnCode");
                TextBox txtDays = (TextBox)row.FindControl("txtDays");
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (hdnCode.Value == "AD" ||  hdnCode.Value == "PO" || hdnCode.Value == "DC")
                {
                    txtDays.Enabled = false;
                    chkSelect.Enabled = false;
                    
                }
                if (hdnCode.Value == "OP")
                {
                    txtDays.Text = "1";
                    chkSelect.Checked = true;
                    chkSelect.Enabled = true;

                }
            }
        }
    }

    protected void chkIsSurgical_CheckedChanged(object sender, EventArgs e)
    {
        bool Btrue = false;
        if (chkIsSurgical.Checked)
        {
            Btrue = true;
        }
        else
        {
            Btrue = false;
        }

        foreach (GridViewRow row in gvPlanTypeDuration.Rows)
        {
            HiddenField hdnCode = (HiddenField)row.FindControl("hdnCode");
            TextBox txtDays = (TextBox)row.FindControl("txtDays");
            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
            if (hdnCode.Value == "PO")
            {
                txtDays.Enabled = Btrue;
                chkSelect.Enabled = Btrue;
            }
        }
    }
}

