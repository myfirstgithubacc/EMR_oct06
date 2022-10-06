using System;
using System.Web;
using System.Net;
using System.IO;
using System.Configuration;

public partial class LIS_Phlebotomy_AttachementRender : System.Web.UI.Page
{
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        string Splitter = ConfigurationManager.AppSettings["Split"];
        var csplitter = Splitter.ToCharArray();
        string ftp = ftppath.Split(csplitter)[0].ToString();

        RenderAttachement(ftppath.Split(csplitter)[0].ToString(), ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString(), common.myStr(Request.QueryString["FTPFolder"]),common.myStr(Request.QueryString["FileName"]));
    }

    public void RenderAttachement(string FTPLocation, string FTPUserName,string FTPPassword, string FTPFolder, string FileName)
    {
        BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
        string key = "Word";

        try {
            ////Create FTP Request.
            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPLocation+ en.Decrypt(FTPFolder, key, true) + "/" + en.Decrypt(FileName, key, true));

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPLocation +FTPFolder + "/" + FileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            //Enter FTP Server credentials.
            request.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;
            string strFileNameExact = FileName;

            int indx = FileName.Split('.').Length-1;
            string fileExtension = FileName.Split('.')[indx].Trim().ToLower();
            
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
                 if (!fileExtension.ToString().ToUpper().Equals("PDF"))
                {
                    Response.Write("<script> alert('the file format is not supported to view.') </script>");
                    Panel1.Visible = true;
                }
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
                    case "zip": Response.ContentType = "application/zip"; break;
                    case "doc": Response.ContentType = "application/msword"; break;
                    case "rtf": Response.ContentType = "application/rtf"; break;

                    case "docx": Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; break;
                    case "z": Response.ContentType = "application/x-compress"; break;
                    case "avi": Response.ContentType = "video/x-ms-wmv"; break;
                    case "rar": Response.ContentType = "application/octet-stream"; break;
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
    catch(Exception Ex)
    {
            objException.HandleException(Ex);
        }

        }
    }