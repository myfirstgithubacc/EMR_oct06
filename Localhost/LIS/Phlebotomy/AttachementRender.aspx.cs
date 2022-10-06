using System;
using System.Web;
using System.Net;
using System.IO;
using System.Configuration;

public partial class LIS_Phlebotomy_AttachementRender : System.Web.UI.Page
{
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    //add by shaivee
    string FileFolder = ConfigurationManager.AppSettings["FileFolder"];
    //end
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        string Splitter = ConfigurationManager.AppSettings["Split"];
        var csplitter = Splitter.ToCharArray();
        string ftp = ftppath.Split(csplitter)[0].ToString();

        RenderAttachement(ftppath.Split(csplitter)[0].ToString(), ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString(), common.myStr(Request.QueryString["FTPFolder"]), common.myStr(Request.QueryString["FileName"]));
    }

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