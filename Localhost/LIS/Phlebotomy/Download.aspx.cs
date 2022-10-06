using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using Telerik.Web.UI;
using System.Web.UI;
using System.Net;
using System.Web;



public partial class LIS_Phlebotomy_Download : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;
    private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];

    protected void Page_Load(object sender, EventArgs e)
    {
        bindfiles();
    }

    void bindfiles()
    {
        try
        {
            if (Request.QueryString["SampleId"] != null)
            {
                if ((common.myStr(Request.QueryString["Source"]) == "OPD") || (common.myStr(Request.QueryString["Source"]) == "PACKAGE") || (common.myStr(Request.QueryString["Source"]) == "ER"))
                {
                    objval = new BaseC.clsLISPhlebotomy(sConString);
                    DataSet ds = objval.FillAttachmentDownloadDropdownOP(Request.QueryString["SampleId"].ToString(), "");
                    gvDownload.DataSource = ds;
                    gvDownload.DataBind();
                }
                else
                {
                    objval = new BaseC.clsLISPhlebotomy(sConString);
                    DataSet ds = objval.FillAttachmentDownloadDropdownIP(Request.QueryString["SampleId"].ToString(), "");
                    gvDownload.DataSource = ds;
                    gvDownload.DataBind();
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

    protected void gvDownload_OnItemCommandx(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Download")
            {
                Label lblDocumentName = (Label)e.Item.FindControl("lblDocumentName");
                string sFileName = lblDocumentName.Text;
                string sSavePath = ConfigurationManager.AppSettings["LabResultPath"];
                string path = Server.MapPath(sSavePath + sFileName);
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (file.Exists)
                {
                    this.Response.Clear();
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    this.Response.AddHeader("Content-Length", file.Length.ToString());
                    this.Response.ContentType = "application/octet-stream";
                    this.Response.BinaryWrite(File.ReadAllBytes(file.FullName));
                    //  this.Response.End();
                    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "File does not exist...";
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
    protected void gvDownload_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
            string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
            string ftppath = ConfigurationManager.AppSettings["FTP"];
            string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];

            if (e.CommandName == "Download")
            {
                bool isOutsourceResult = true;
                //add by shaivee
                if (isOutsourceResult)
                {
                    string key = "Word";
                    Label lblDocumentName = (Label)e.Item.FindControl("lblDocumentName");
                    string sFileName = lblDocumentName.Text;
                    string sSavePath = Rootfolder.ToString() + "LabResult/";// ConfigurationManager.AppSettings["LabResultPath"];
                    string path = sSavePath + sFileName;

                    string URLPath = "AttachementRender.aspx?FTPFolder=@FTPFolder&FileName=@FileName";
                    URLPath = URLPath.Replace("@FTPFolder", en.Encrypt(ConfigurationManager.AppSettings["LabResultPath"], key, true, string.Empty)).Replace("@FileName", en.Encrypt(sFileName, key, true, string.Empty));

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + URLPath.Replace("+", "%2B") + "','_blank')", true);
                    //end by shaivee                   
                }
                else
                {
                    Label lblDocumentName = (Label)e.Item.FindControl("lblDocumentName");
                    string sFileName = lblDocumentName.Text;
                    string sSavePath = Rootfolder.ToString() + "LabResult/";// ConfigurationManager.AppSettings["LabResultPath"];
                    string path = sSavePath + sFileName;
                    System.IO.FileInfo file = new System.IO.FileInfo(path);

                    //string sFileName = ds.Tables[0].Rows[0]["DocumentName"].ToString();
                    //string sSavePath = Session["foldername"] + ConfigurationManager.AppSettings["LabResultPath"];
                    //string path = sSavePath + sFileName;
                    ////Create FTP Request.


                    string Splitter = ConfigurationManager.AppSettings["Split"];
                    if (common.myLen(Splitter).Equals(0))
                    {
                        Splitter = "!";
                    }

                    var csplitter = Splitter.ToCharArray();

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

                        Response.BinaryWrite(stream.ToArray());

                        Response.End();

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
}
