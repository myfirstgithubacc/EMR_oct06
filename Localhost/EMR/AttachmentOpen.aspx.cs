using System;
using System.IO;

public partial class EMR_AttachmentOpen : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sFileName = common.myStr(Session["AttachmentPathFileName"]);
            if (common.myLen(sFileName) > 0)
            {
                // Get the physical Path of the file
                string filepath = sFileName;

                // Create New instance of FileInfo class to get the properties of the file being downloaded
                FileInfo file = new FileInfo(filepath);

                // Checking if file exists
                if (file.Exists)
                {
                    // Clear the content of the response
                    Response.ClearContent();

                    // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                    Response.AddHeader("Content-Disposition", "inline; filename=" + file.Name);

                    // Add the file size into the response header
                    Response.AddHeader("Content-Length", file.Length.ToString());

                    // Set the ContentType
                    Response.ContentType = ReturnExtension(file.Extension.ToLower());

                    // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                    Response.TransmitFile(file.FullName);

                    // End the response
                    Response.End();
                }
            }
        }
    }

    private string ReturnExtension(string fileExtension)
    {
        switch (fileExtension)
        {
            case ".htm":
            case ".html":
            case ".log":
                return "text/HTML";
            case ".txt":
                return "text/plain";
            case ".docx":
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case ".doc":
                return "application/msword";
            case ".tiff":
            case ".tif":
                return "image/tiff";
            case ".asf":
                return "video/x-ms-asf";
            case ".avi":
                return "video/avi";
            case ".rar":
                return "application/rar";
            case ".zip":
                return "application/zip";
            case ".xls":
            case ".csv":
                return "application/vnd.ms-excel";
            case ".gif":
                return "image/gif";
            case ".jpg":
            case ".jpeg":            
                return "image/jpeg";
            case ".png":
                return "image/png";
            case ".bmp":
                return "image/bmp";
            case ".wav":
                return "audio/wav";
            case ".mp3":
                return "audio/mpeg3";
            case ".mpg":
            case ".mpeg":
                return "video/mpeg";
            case ".rtf":
                return "application/rtf";
            case ".asp":
                return "text/asp";
            case ".pdf":
                return "application/pdf";
            case ".fdf":
                return "application/vnd.fdf";
            case ".ppt":
                return "application/mspowerpoint";
            case ".dwg":
                return "image/vnd.dwg";
            case ".msg":
                return "application/msoutlook";
            case ".xml":
            case ".sdxl":
                return "application/xml";
            case ".xdp":
                return "application/vnd.adobe.xdp+xml";
            default:
                return "application/octet-stream";
        }

    }

}
