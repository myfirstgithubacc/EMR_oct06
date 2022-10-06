using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class LIS_Phlebotomy_Attachment : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;
    private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);

            if (Request.QueryString["Source"] != null)
            {
                lblSource.Text = common.myStr(Request.QueryString["Source"]);

            }
            if (Request.QueryString["RegNo"] != null && Request.QueryString["PName"] != null)
            {

                lblPatientDetails.Text = " | " + "Lab No: " + common.myStr(Request.QueryString["LabNo"]) + " | " + HttpContext.GetGlobalResourceObject("PRegistration", "regno") + ": " + common.myStr(Request.QueryString["RegNo"]) + " | Patient: " + common.myStr(Request.QueryString["PName"]).Trim();
                lblExternalName.Text = common.myStr(Request.QueryString["ExternalCenter"]);
            }

            ViewState["ServiceID"] = 0;
            if (Request.QueryString["RefServiceCode"] != null)
            {
                if (common.myInt(Request.QueryString["RefServiceCode"]) > 0)
                {
                    if (Session["LISRISAttachSelectedServiceName"] != null)
                    {
                        lblRefServiceCode.Text = common.myStr(Request.QueryString["RefServiceCode"]) + " - " + common.myStr(Session["LISRISAttachSelectedServiceName"]);
                        Session["LISRISAttachSelectedServiceName"] = null;
                    }
                    else
                    {
                        lblRefServiceCode.Text = common.myStr(Request.QueryString["RefServiceCode"]);
                    }
                }
            }
            if (lblSource.Text == "OPD")
            {
                GetFilesUploadListOP();
            }
            else
            {
                GetFilesUploadListIP();
            }
        }
    }

    void GetFilesUploadListOP()
    {
        try
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objval.FillAttachmentDownloadDropdownOP(Request.QueryString["DiagSampleId"].ToString(), "");

            ddlfilename.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dr["Description"];
                item.Value = dr["Id"].ToString();
                item.Attributes.Add("DocumentName", dr["DocumentName"].ToString());
                ddlfilename.Items.Add(item);
                item.DataBind();
            }

            ddlfilename.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlfilename.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    void GetFilesUploadListIP()
    {
        try
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objval.FillAttachmentDownloadDropdownIP(Request.QueryString["DiagSampleId"].ToString(), "");

            ddlfilename.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dr["Description"];
                item.Value = dr["Id"].ToString();
                item.Attributes.Add("DocumentName", dr["DocumentName"].ToString());
                ddlfilename.Items.Add(item);
                item.DataBind();
            }

            ddlfilename.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlfilename.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void cmdUpload_OnClick(object sender, EventArgs e)
    {

        try
        {
            if (common.myStr(ViewState["Status"]) == "O")
            {
                SaveRecord();//S = to save file 
            }
            else
            {
                if (txtFileName.Text != "")
                {
                    objval = new BaseC.clsLISPhlebotomy(sConString);
                    DataSet ds = objval.getAttachmentDownload(Request.QueryString["DiagSampleId"].ToString(), txtFileName.Text);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "This file name is exist.Please chage file name.";
                        return;
                    }
                }
                if (UploadFile("S") == true)
                {
                    SaveRecord();//S = to save file 
                    txtFileName.Text = "";
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
    bool UploadFile(string type)
    {
        string sSavePath = Session["foldername"] + ConfigurationManager.AppSettings["LabResultPath"];
        if (fUpload2.FileName != "")
        {

            string sFileName = "";
            sFileName = common.myStr(Request.QueryString["LabNo"]) + "_" + common.myStr(Request.QueryString["RefServiceCode"]) + "_" + ddlfilename.Items.Count;

            HttpPostedFile myFile = fUpload2.PostedFile;
            int nFileLen = myFile.ContentLength;
            if (nFileLen == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: The file size is zero.";
                return false;
            }
            // Read file into a data stream
            //byte[] myData = new Byte[nFileLen];
            //myFile.InputStream.Read(myData, 0, nFileLen);
            sFileName = sFileName + System.IO.Path.GetExtension(myFile.FileName).ToLower();
            bool ret = UploadtoFTP(fUpload2, sSavePath + sFileName);
            //System.IO.FileStream newFile = new System.IO.FileStream(sSavePath + sFileName , System.IO.FileMode.Create);
            //newFile.Write(myData, 0, myData.Length);
            //newFile.Close();
            ViewState["DocumentName"] = sFileName;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = "Report uploaded and " + lblMessage.Text;
            return ret;
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: Please select a file";
            return false;
        }

    }
    private bool UploadtoFTP(FileUpload FileUpload1, string fileName)
    {
        string Splitter = ConfigurationManager.AppSettings["Split"];
        if (common.myLen(Splitter).Equals(0))
        {
            Splitter = "!";
        }

        var csplitter = Splitter.ToCharArray();

        bool ret = true;
        //FTP Server URL.
        string ftp = ftppath.Split(csplitter)[0].ToString();
        //Checck Directory
        //if (!FtpDirectoryExists(ftppath.Split(csplitter)[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text, ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString()))
        //{
        //    CreateFTPDirectory(ftppath.Split(csplitter)[0].ToString() + Rootfolder + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text + "/", ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString());
        //}
        //FTP Folder name. Leave blank if you want to upload to root folder.
        string ftpFolder = Rootfolder;

        try
        {
            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(ftppath.Split(csplitter)[0].ToString() + ftpFolder + fileName);
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

            lblMessage.Text += fileName + " uploaded.<br />";
            ret = true;
        }
        catch (WebException ex)
        {
            ret = false;
            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        }
        return ret;
    }
    bool UploadFilexx(string type)
    {
        string sSavePath = ConfigurationManager.AppSettings["LabResultPath"];
        if (fUpload2.FileName != "")
        {
            string sFileName = "";
            //if (common.myStr(Request.QueryString["RefServiceCode"]) != "")
            //{
            //    sFileName = common.myStr(Request.QueryString["LabNo"]) + "_" + common.myStr(Request.QueryString["RefServiceCode"]) + "_" + ddlfilename.Items.Count;
            //}
            //else
            //{
            sFileName = common.myStr(Request.QueryString["LabNo"]) + "_" + common.myStr(Request.QueryString["DiagSampleId"]) + "_" + ddlfilename.Items.Count;

            // }

            HttpPostedFile myFile = fUpload2.PostedFile;
            int nFileLen = myFile.ContentLength;
            if (nFileLen == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: The file size is zero.";
                return false;
            }
            // Read file into a data stream
            byte[] myData = new Byte[nFileLen];
            myFile.InputStream.Read(myData, 0, nFileLen);
            sFileName = sFileName + System.IO.Path.GetExtension(myFile.FileName).ToLower();
            System.IO.FileStream newFile = new System.IO.FileStream(Server.MapPath(sSavePath + sFileName), System.IO.FileMode.Create);
            newFile.Write(myData, 0, myData.Length);
            newFile.Close();
            ViewState["DocumentName"] = sFileName;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = "Report uploaded and " + lblMessage.Text;
            return true;
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: Please select a file";
            return false;
        }

    }

    bool SaveRecord()
    {
        if (common.myStr(ViewState["Status"]) == "N")
        {
            ddlfilename.SelectedValue = "0";
        }

        objval = new BaseC.clsLISPhlebotomy(sConString);
        string strMsg = objval.SaveUpdateDiagSampleDocument(common.myInt(Request.QueryString["DiagSampleId"]),
            common.myStr(ViewState["DocumentName"]), common.myStr(txtFileName.Text)
            , common.myInt(Session["UserID"]), common.myInt(ddlfilename.SelectedValue), lblSource.Text);

        if (strMsg == "Record(s) Saved..." || strMsg == "Record(s) Updated...")
        {
            if (lblSource.Text == "OPD")
            {
                GetFilesUploadListOP();
            }
            else
            {
                GetFilesUploadListIP();
            }
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = strMsg;
            txtFileName.Text = "";
            return true;
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + strMsg;
            txtFileName.Text = "";
            return false;
        }
    }
    protected void lbtnDownload_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddlfilename.SelectedValue == "0")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Select file name.";
                return;
            }
            string Splitter = ConfigurationManager.AppSettings["Split"];
            if (common.myLen(Splitter).Equals(0))
            {
                Splitter = "!";
            }

            var csplitter = Splitter.ToCharArray();


            DataSet ds = new DataSet();
            objval = new BaseC.clsLISPhlebotomy(sConString);

            string sFileName = common.myStr(ddlfilename.SelectedItem.Attributes["DocumentName"]);
            string sSavePath = Session["foldername"] + ConfigurationManager.AppSettings["LabResultPath"];
            string path = sSavePath + sFileName;
            //Create FTP Request.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftppath.Split(csplitter)[0].ToString() + Rootfolder + ConfigurationManager.AppSettings["LabResultPath"] + "/" + sFileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            //Enter FTP Server credentials.
            request.Credentials = new NetworkCredential(ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString());
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;

            //Fetch the Response and read it into a MemoryStream object.
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            using (MemoryStream stream = new MemoryStream())
            {

                //Download the File.
                response.GetResponseStream().CopyTo(stream);
                Response.AddHeader("content-disposition", "attachment;filename=" + sFileName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                // string outfile = "";
                //common.ByteArrayToFile(FileFolderx + lnkDownLoad.CommandName, stream.ToArray(), out outfile);
                Response.BinaryWrite(stream.ToArray());
                //if (lnkDownLoad.CommandName.Contains("pdf"))
                //{
                //    Response.ContentType = "Application/pdf";
                //}
                //else if (lnkDownLoad.CommandName.Contains("jpg") || lnkDownLoad.CommandName.Contains("png") || lnkDownLoad.CommandName.Contains("bmp"))
                //{
                //    Response.ContentType = "image/png";
                //}
                //Response.WriteFile(outfile);
                Response.End();
                //System.IO.FileInfo file = new System.IO.FileInfo(path);
                //if (file.Exists)
                //{
                //    Response.Clear();
                //    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                //    Response.AddHeader("Content-Length", file.Length.ToString());
                //    Response.ContentType = "application/octet-stream";
                //    Response.WriteFile(file.FullName);
                //    // Response.End();
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //}
                //else
                //{
                //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //    lblMessage.Text = "File does not exist...";
                //}
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void lbtnDelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddlfilename.SelectedValue != "0")
            {
                objval = new BaseC.clsLISPhlebotomy(sConString);
                string strMsg = objval.DeleteSampleDocument(common.myInt(Request.QueryString["DiagSampleId"]),
                    common.myStr(txtFileName.Text), common.myInt(Session["UserID"]),
                    common.myInt(Session["HospitalLocationId"]), lblSource.Text, common.myInt(ddlfilename.SelectedValue));

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;
                txtFileName.Text = "";
                ViewState["Status"] = "";
                if (lblSource.Text == "OPD")
                {
                    GetFilesUploadListOP();
                }
                else
                {
                    GetFilesUploadListIP();
                }
                if (ddlfilename.Items.Count > 0)
                {
                    Session["RefreshParentPage"] = true;
                }
                else
                {
                    Session["RefreshParentPage"] = false;
                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Select file name.";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void lbtnDownload_OnClickx(object sender, EventArgs e)
    {
        try
        {
            if (ddlfilename.SelectedValue == "0")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Select file name.";
                return;
            }
            DataSet ds = new DataSet();
            objval = new BaseC.clsLISPhlebotomy(sConString);

            string sFileName = common.myStr(ddlfilename.SelectedItem.Attributes["DocumentName"]);
            string sSavePath = ConfigurationManager.AppSettings["LabResultPath"];
            string path = Server.MapPath(sSavePath + sFileName);
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "File does not exist...";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlfilename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlfilename.SelectedValue != "0")
            {
                txtFileName.Text = ddlfilename.SelectedItem.Text;
                string sFileName = common.myStr(ddlfilename.SelectedItem.Attributes["DocumentName"]);
                ViewState["DocumentName"] = sFileName;
                lblMessage.Text = "";
                ViewState["Status"] = "O";
            }
            else
            {
                ViewState["Status"] = "N";
                txtFileName.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

}
