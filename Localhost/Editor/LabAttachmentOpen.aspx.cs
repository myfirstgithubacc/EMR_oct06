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
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;
using Ionic.Zip;
using System.Net;


public partial class Editor_LabAttachmentOpen : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
   

    //private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
   
    //private string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region comment
            ////if (common.myStr(Cache["folderpath"]).Trim().Equals(""))
            ////{
            ////    //BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
            ////    //DAL.DAL dlf = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ////    //string folderpath = common.myStr(dlf.ExecuteScalar(CommandType.Text, "selecT isnull(folderpath,'') From fileserversetup with(nolock) where facilityID=" + common.myStr(Session["FacilityId"]) + ""));
            ////    if (folderpath.Trim() != "")
            ////    {
            ////        Cache["folderpath"] = folderpath;
            ////    }
            ////    else
            ////    {
            ////        Cache["folderpath"] = Server.MapPath("/PatientDocuments/");
            ////        folderpath = Server.MapPath("/PatientDocuments/");
            ////    }
            ////    //dlf = null;
            ////    //emr = null; 
            ////}

            //string sFileName = string.Empty;
            //if (common.myStr(Request.QueryString["AttachmentOption"]).ToUpper().Equals("EMR"))
            //{
            //    sFileName = common.myStr(Request.QueryString["DocumentName"]);
            //}
            //else
            //{
            //    sFileName = common.myStr(folderpath) + common.myStr(@"LabResult\") + common.myStr(Request.QueryString["DocumentName"]);
            //}

            //if (common.myLen(sFileName) > 0)
            //{
            //    // Get the physical Path of the file
            //    string filepath = sFileName;
            //    //filepath = ".." + filepath;
            //  //  filepath = "/PatientDocuments/1/647/647_09122016100446AM_36.png";

            //           // Create New instance of FileInfo class to get the properties of the file being downloaded
            //           FileInfo file = new FileInfo(filepath);

            //    // Checking if file exists
            //    if (File.Exists(Server.MapPath(filepath)))
            //    {
            //        //if (file.Exists)
            //        //{
            //        // Clear the content of the response
            //        Response.ClearContent();

            //        // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
            //        Response.AddHeader("Content-Disposition", "inline; filename=" + file.Name);

            //        // Add the file size into the response header
            //        Response.AddHeader("Content-Length", Server.MapPath(filepath).Length.ToString());

            //        // Set the ContentType
            //        Response.ContentType = ReturnExtension(file.Extension.ToLower());

            //        // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
            //        filepath = "~" + filepath;
            //        Response.TransmitFile(Server.MapPath(filepath));

            //        // End the response
            //     //Response.End();
            //    }
            //}

            #endregion
            string Splitter = ConfigurationManager.AppSettings["Split"];
            if (common.myLen(Splitter).Equals(0))
            {
                Splitter = "!";
            }

            var csplitter = Splitter.ToCharArray();
            string ftp = ftppath.Split(csplitter)[0].ToString();

            RenderAttachement(ftppath.Split(csplitter)[0].ToString(), ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString(), common.myStr(Request.QueryString["FTPFolder"]) , common.myStr(Request.QueryString["FileName"]));
        }
    }

    //private string ReturnExtension(string fileExtension)
    //{
    //    switch (fileExtension)
    //    {
    //        case ".htm":
    //        case ".html":
    //        case ".log":
    //            return "text/HTML";
    //        case ".txt":
    //            return "text/plain";
    //        case ".docx":
    //            return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    //        case ".doc":
    //            return "application/msword";
    //        case ".tiff":
    //        case ".tif":
    //            return "image/tiff";
    //        case ".asf":
    //            return "video/x-ms-asf";
    //        case ".avi":
    //            return "video/avi";
    //        case ".rar":
    //            return "application/rar";
    //        case ".zip":
    //            return "application/zip";
    //        case ".xls":
    //        case ".csv":
    //            return "application/vnd.ms-excel";
    //        case ".gif":
    //            return "image/gif";
    //        case ".jpg":
    //        case ".jpeg":            
    //            return "image/jpeg";
    //        case ".png":
    //            return "image/png";
    //        case ".bmp":
    //            return "image/bmp";
    //        case ".wav":
    //            return "audio/wav";
    //        case ".mp3":
    //            return "audio/mpeg3";
    //        case ".mpg":
    //        case ".mpeg":
    //            return "video/mpeg";
    //        case ".rtf":
    //            return "application/rtf";
    //        case ".asp":
    //            return "text/asp";
    //        case ".pdf":
    //            return "application/pdf";
    //        case ".fdf":
    //            return "application/vnd.fdf";
    //        case ".ppt":
    //            return "application/mspowerpoint";
    //        case ".dwg":
    //            return "image/vnd.dwg";
    //        case ".msg":
    //            return "application/msoutlook";
    //        case ".xml":
    //        case ".sdxl":
    //            return "application/xml";
    //        case ".xdp":
    //            return "application/vnd.adobe.xdp+xml";
    //        default:
    //            return "application/octet-stream";
    //    }

    //}
    public void RenderAttachement(string FTPLocation, string FTPUserName, string FTPPassword, string FTPFolder, string FileName)
    {
        BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
        string key = "Word";

        try
        {
            ////Create FTP Request.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPLocation + en.Decrypt(FTPFolder, key, true) + "/" + en.Decrypt(FileName, key, true));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            //Enter FTP Server credentials.
            request.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;
            string strFileNameExact = en.Decrypt(FileName, key, true);

            int indx = en.Decrypt(FileName, key, true).Split('.').Length - 1;
            string fileExtension = en.Decrypt(FileName, key, true).Split('.')[indx].Trim().ToLower();

            //Fetch the Response and read it into a MemoryStream object Ujjwal Sinha.
            byte[] fileBytes = null;

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        if (responseStream != null) responseStream.CopyTo(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }
                }
            }
            if (fileBytes.Length > 0)
            {
                switch (fileExtension)
                {
                    case "pdf": Response.ContentType = "application/pdf"; break;
                    case "swf": Response.ContentType = "application/x-shockwave-flash"; break;
                    case "png": Response.ContentType = "image/png"; break;
                    case "gif": Response.ContentType = "image/gif"; break;
                    case "jpeg": Response.ContentType = "image/jpg"; break;
                    case "jpg": Response.ContentType = "image/jpg"; break;
                    case "mp4": Response.ContentType = "video/mp4"; break;
                    case "mpeg": Response.ContentType = "video/mpeg"; break;
                    case "mov": Response.ContentType = "video/quicktime"; break;
                    case "wmv":
                    case "avi": Response.ContentType = "video/x-ms-wmv"; break;
                    //and so on          
                    default: Response.ContentType = "application/octet-stream"; break;
                }

                Response.AddHeader("Content-Length", fileBytes.Length.ToString());
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(fileBytes);
                Response.End();
            }

            request.Abort();

            ApplicationInstance.CompleteRequest();
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }

    }
}
