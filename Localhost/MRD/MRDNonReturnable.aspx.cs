using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web;

public partial class MRD_MRDNonReturnable : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    BaseC.RestFulAPI objMRD;// = new wcf_Service_MRD.MRDServiceClient();
    BaseC.clsPharmacy objPharmacy;//f270117
    BaseC.clsLISPhlebotomy objval;
    private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string Rootfolder = ConfigurationManager.AppSettings["FileFolderMRD"];
    string[] strdocType = { ".pdf", ".doc", ".xls", ".docx", ".xlsx", ".txt", ".zip" };

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["POPUP"]).ToUpper() == "POPUP")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BaseC.Security objSecurity = new BaseC.Security(sConString);
            ViewState["IsUserCancelMRDNonReturnable"] = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsUserCancelMRDNonReturnable").ToString();
            objSecurity = null;
            BindIssueBy();
            bindControl();
            dtpDeliveryDateTime.SelectedDate = DateTime.Now;
            dtpDeliveryDateTime.MaxDate = DateTime.Now;

        }
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            BaseC.EMRBilling objEMRBilling = new BaseC.EMRBilling(sConString);
            dt = objEMRBilling.getEmployeeList(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(Session["FacilityId"]));

            ddlDoctorList.DataSource = dt;
            ddlDoctorList.DataTextField = "EmployeeNameWithNo";
            ddlDoctorList.DataValueField = "EmployeeId";
            ddlDoctorList.DataBind();

            ddlDoctorList.Items.Insert(0, new RadComboBoxItem("", "0"));

        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!isSaved())
        {
            return;
        }
        else
        {
            SaveData();
            BindPatientHiddenDetails(common.myInt(txtUHID.Text));
        }
    }

    private bool isSaved()
    {
        bool isSave = true;
        StringBuilder strmsg = new StringBuilder();

        if (common.myInt(hdnRegistrationId.Value) == 0)
        {
            strmsg.Append("Registration not selected !");
            isSave = false;
        }
        if (common.myInt(ddlDoctorList.SelectedValue) == 0 || (chkoutside.Checked && txtoutsidePersoName.Text.Length == 0))
        {
            strmsg.Append("Please select Request By !");
            isSave = false;
        }
        if (common.myStr(txtreason.Text).Length == 0)
        {
            strmsg.Append("Reason can't be blank !");
            isSave = false;
        }
        if (common.myStr(txtRemarks.Text).Length == 0)
        {
            strmsg.Append("Purpose/Detail can't be blank !");
            isSave = false;
        }

        lblMessage.Text = strmsg.ToString();
        lblMessage.ForeColor = System.Drawing.Color.Red;
        return isSave;
    }

    private void SaveData()
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "";
            string strMsg = "";
            objMRD = new BaseC.RestFulAPI(sConString);
            string sFileName = "";
            string sFileSize;
            if (_FileUpload.FileName != "")
            {
                sFileName = CreateFileName("MRDNonReturnable", _FileUpload.FileName);
                sFileSize = _FileUpload.PostedFile.ContentLength.ToString() + " KB";
                if (_FileUpload.PostedFile.ContentLength > 20971520)
                {
                    Alert.ShowAjaxMsg("The file you uplaod is too large.", Page);
                    return;
                }
                else
                {
                    UploadtoFTP(_FileUpload, sFileName);
                }
            }

            else
            {
                Alert.ShowAjaxMsg("Please Select File", Page);
                return;
            }

            strMsg = objMRD.MRDSaveMRDNonReturnable(common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value), common.myInt(Session["FacilityId"]), txtRemarks.Text, txtreason.Text, true,
                       sFileName, txtFileName.Text, common.myInt(Session["UserId"]), common.myInt(ddlDoctorList.SelectedValue), txtoutsidePersoName.Text, txtmobileno.Text, txtDocument.Text, common.myDate(dtpDeliveryDateTime.SelectedDate), 0);


            if ((strMsg.ToUpper().Contains("SUCCESSFULLY") || strMsg.ToUpper().Contains("SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                clearControl();
            }
            lblMessage.Text = strMsg;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
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
    private void UploadtoFTP(FileUpload FileUpload1, string fileName)
    {

        //FTP Server URL.
        string ftp = ftppath.Split('!')[0].ToString();
        //Checck Directory
        if (!FtpDirectoryExists(ftppath.Split('!')[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString(), ftppath.Split('!')[1].ToString(), ftppath.Split('!')[2].ToString()))
        {
            CreateFTPDirectory(ftppath.Split('!')[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString(), ftppath.Split('!')[1].ToString(), ftppath.Split('!')[2].ToString());
        }
        //FTP Folder name. Leave blank if you want to upload to root folder.
        string ftpFolder = Rootfolder;


        try
        {

            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(ftppath.Split('!')[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString() + "/" + fileName);
            ftpClient.Credentials = new System.Net.NetworkCredential(ftppath.Split('!')[1].ToString(), ftppath.Split('!')[2].ToString());
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
    protected string CreateFileName(string sCategoryId, string sFileName)
    {
        string FileName = "";
        try
        {
            int extIndex = sFileName.IndexOf(".");
            string sExt = sFileName.Substring(extIndex, sFileName.Length - extIndex);
            BaseC.Patient objPat = new BaseC.Patient(sConString);
            string[] sTime = null;
            char[] chr = { ' ' };
            sTime = DateTime.Now.ToString().Split(chr);
            FileName = objPat.FormatDateDateMonthYear(sTime[0]) + sTime[1] + sTime[2];
            FileName = FileName.Replace("/", "");
            FileName = FileName.Replace(":", "");
            //Response.Write(objPat.FormatDateDateMonthYear(DateTime.Today.ToShortDateString()) + " " + DateTime.Now.Hour.ToString() + "_" + DateTime.Today.Now.ToString() + "_" + DateTime.Now.Second.ToString());
            if (txtUHID.Text == "")
            {
                FileName = txtUHID.Text.ToString().Trim() + "_" + FileName + "_" + sCategoryId + sExt;
            }
            else
            {
                FileName = txtUHID.Text.ToString() + "_" + FileName + "_" + sCategoryId + sExt;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return FileName;
    }


    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            //Done by Ujjwal 25 June 2015 to validate Registration No start
            int UHID;
            int.TryParse(txtUHID.Text, out UHID);
            if ((UHID > 2147483647 || UHID.Equals(0)) && !common.myLen(txtUHID.Text).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Invalid UHID";
                return;
            }
            //Done by Ujjwal 25 June 2015 to validate Registration No end
            BindPatientHiddenDetails(common.myInt(txtUHID.Text));
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }


    void BindPatientHiddenDetails(int RegistrationNo)
    {
        try
        {
            BaseC.ParseData bParse = new BaseC.ParseData();
            BaseC.Patient bC = new BaseC.Patient(sConString);

            if (RegistrationNo > 0)
            {
                DataSet ds = new DataSet();
                ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, 0, common.myInt(Session["UserId"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                    hdnEncounterNo.Value = common.myStr(dr["EncounterNo"]); ;
                    hdnEncounterId.Value = common.myStr(dr["EncounterID"]);

                    lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                    lblDob.Text = common.myStr(dr["DOB"]);
                    lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                    lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);

                    hdnGIssueId.Value = "";

                    objMRD = new BaseC.RestFulAPI(sConString);

                    ds = objMRD.GetMRDNonReturnable(common.myInt(hdnRegistrationId.Value),
                                 common.myInt(hdnEncounterId.Value), common.myInt(Session["FacilityID"]));




                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //ddlDoctorList.SelectedIndex = ddlDoctorList.Items.IndexOf(ddlDoctorList.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["Employeeid"])));
                        //txtRemarks.Text = common.myStr(ds.Tables[0].Rows[0]["NonReturnableDetails"]);
                        //txtreason.Text = common.myStr(ds.Tables[0].Rows[0]["NonReturnableDetails"]);
                        //txtFileName.Text = common.myStr(ds.Tables[0].Rows[0]["DocumentName"]);
                        //ddlRequestBy.SelectedIndex = ddlDoctorList.Items.IndexOf(ddlDoctorList.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["EncodedBy"])));
                        //btnSave.Visible = false;

                        gvData.DataSource = ds.Tables[0];
                        gvData.DataBind();
                    }


                }
                else
                {
                    Alert.ShowAjaxMsg("Patient Not Found", Page);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }
    void BindPatientHiddenDetailsIP(string IPNo)
    {
        try
        {
            BaseC.ParseData bParse = new BaseC.ParseData();
            BaseC.Patient bC = new BaseC.Patient(sConString);

            if (IPNo != "")
            {
                DataSet ds = new DataSet();
                ds = bC.getPatientDetailsEnc(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, IPNo, 0, common.myInt(Session["UserId"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                    hdnEncounterNo.Value = common.myStr(dr["EncounterNo"]); ;
                    hdnEncounterId.Value = common.myStr(dr["EncounterID"]);

                    lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                    lblDob.Text = common.myStr(dr["DOB"]);
                    lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                    lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);

                    hdnGIssueId.Value = "";

                    objMRD = new BaseC.RestFulAPI(sConString);

                    ds = objMRD.GetMRDNonReturnable(common.myInt(hdnRegistrationId.Value),
                                common.myInt(hdnEncounterId.Value), common.myInt(Session["FacilityID"]));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //ddlDoctorList.SelectedIndex = ddlDoctorList.Items.IndexOf(ddlDoctorList.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["Employeeid"])));
                        //txtRemarks.Text = common.myStr(ds.Tables[0].Rows[0]["NonReturnableDetails"]);
                        //txtreason.Text = common.myStr(ds.Tables[0].Rows[0]["NonReturnableDetails"]);
                        //txtFileName.Text = common.myStr(ds.Tables[0].Rows[0]["DocumentName"]);
                        //ddlRequestBy.SelectedIndex = ddlDoctorList.Items.IndexOf(ddlDoctorList.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["EncodedBy"])));
                        //btnSave.Visible = false;
                        gvData.DataSource = ds.Tables[0];
                        gvData.DataBind();
                    }

                }
                else
                {
                    Alert.ShowAjaxMsg("Patient Not Found", Page);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }
    protected void btnCloseIP_Click(object sender, EventArgs e)
    {
        try
        {

            //int UHID;
            //int.TryParse(txtUHID.Text, out UHID);
            //if ((UHID > 2147483647 || UHID.Equals(0)) && !common.myLen(txtUHID.Text).Equals(0))
            //{
            //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //    lblMessage.Text = "Invalid UHID";
            //    return;
            //}

            BindPatientHiddenDetailsIP(txtIPNo.Text);
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }
    protected void lbtnSearchPatientIP_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?MRDSearch=IP";
        RadWindow1.Height = 580;
        RadWindow1.Width = 900;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "SearchPatientOnClientCloseIP";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void lnkUHID_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?MRDSearch=IP";
        RadWindow1.Height = 580;
        RadWindow1.Width = 900;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }




    protected void BindIssueBy()
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = new DataSet();
            ds = new DataSet();
            ds = objPharmacy.GetEmployee(common.myInt(Session["HospitallOcationId"]));
            if (ds.Tables.Count > 0)
            {
                ddlRequestBy.DataSource = ds;
                ddlRequestBy.DataTextField = "Name";
                ddlRequestBy.DataValueField = "Id";
                ddlRequestBy.DataBind();
            }
            ddlRequestBy.SelectedValue = common.myStr(Session["EmployeeId"]);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (chkoutside.Checked)
        {
            ddlDoctorList.Visible = false;
            txtoutsidePersoName.Visible = true;
        }
        else
        {
            txtoutsidePersoName.Visible = false;
            ddlDoctorList.Visible = true;
        }
    }

    protected void btnnew_Click(object sender, EventArgs e)
    {
        clearControl();

    }

    void clearControl()
    {
        btnSave.Visible = true;
        ddlDoctorList.SelectedIndex = 0;
        ddlRequestBy.SelectedIndex = 0;
        txtreason.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        txtFileName.Text = string.Empty;
        hdnRegistrationId.Value = "";
        hdnEncounterNo.Value = "";
        hdnEncounterId.Value = "";
        lblPatientName.Text = string.Empty;
        lblDob.Text = string.Empty;
        lblEncounterNo.Text = string.Empty;
        lblAdmissionDate.Text = string.Empty;
        ddlRequestBy.Enabled = false;
    }

    protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Download")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                DataSet ds = new DataSet();
                LinkButton lblDocumentPath = (LinkButton)row.FindControl("lblDocumentPath");

                objval = new BaseC.clsLISPhlebotomy(sConString);
                BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
                string key = "Word";
                string sFileName = common.myStr(lblDocumentPath.CommandArgument);
                string sSavePath = common.myStr(ConfigurationManager.AppSettings["FileFolderMRD"] + Session["HospitalLocationID"].ToString() + "/");
                string path = sSavePath + sFileName;
                string URLPath = "/MRD/AttachementRender.aspx?FTPFolder=@FTPFolder&FileName=@FileName";
                URLPath = URLPath.Replace("@FTPFolder", ConfigurationManager.AppSettings["FileFolderMRD"] + Session["HospitalLocationID"].ToString()).Replace("@FileName", sFileName);


                lblMessage.Text = "";

                //RadWindow2.NavigateUrl = URLPath.Replace("+", "%2B");
                //RadWindow2.Height = 580;
                //RadWindow2.Width = 900;
                //RadWindow2.Top = 10;
                //RadWindow2.Left = 10;

                //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                //RadWindow2.Modal = true;
                //RadWindow2.VisibleStatusbar = false;

                //string strPopup = "<script language='javascript' ID='script1'>" + "window.open('"+ URLPath.Replace("+", "%2B") + ",'new window', 'top = 10, left = 10, width = 2000, height = 630, dependant = no, location = 0, alwaysRaised = no, menubar = no, resizeable = no, scrollbars = n, toolbar = no, status = no, center = yes')" + "</script>";
                //ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);

                string var = URLPath.Replace("+", "%2B");
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgePrint('" + var + "');", true);

            }
            else if (e.CommandName == "Del")
            {
                string strMsg = "";
                int intId = common.myInt(e.CommandArgument);
                if (intId > 0)
                {
                    strMsg = objMRD.MRDSaveMRDNonReturnable(0, 0, common.myInt(Session["FacilityId"]), string.Empty, string.Empty, false, string.Empty, string.Empty, common.myInt(Session["UserId"]), 0, string.Empty, string.Empty, string.Empty, DateTime.Now, intId);


                    if ((strMsg.ToUpper().Contains("SUCCESSFULLY") || strMsg.ToUpper().Contains("Deleted")) && !strMsg.ToUpper().Contains("USP"))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        clearControl();
                        BindPatientHiddenDetails(common.myInt(txtUHID.Text));
                        lblMessage.Text = strMsg;
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


    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ibtndaDelete = (ImageButton)e.Row.FindControl("ibtndaDelete");
            if (common.myBool(ViewState["IsUserCancelMRDNonReturnable"]))
            {
                ibtndaDelete.Visible = true;
            }
            else
            {
                ibtndaDelete.Visible = false;
            }
        }
    }
}