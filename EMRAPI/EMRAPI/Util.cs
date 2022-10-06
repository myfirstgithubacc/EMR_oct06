using EMRAPI.Classes;
using EMRAPI.Models;
using Microsoft.Reporting.WebForms;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;


public sealed class Util
{
    private static string sConString = Util.GetConnectionString();
    private static string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private static string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private static string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private static string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private static string SysDomain = ConfigurationManager.AppSettings["SysDomain"];

    public static string GetConnectionString()
    {
        return ConfigurationManager.ConnectionStrings["akl"].ConnectionString; 
    }

    public static byte[] GetSSRSReport(ReportParameter[] parameter, string ReportName)
    {
        byte[] bytes;
        try
        {
            string ReportServerPath = "http://" + reportServer + "/ReportServer";
            var reportViewer = new ReportViewer();

            reportViewer.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
            reportViewer.ServerReport.ReportServerCredentials = irsc;
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            reportViewer.ShowCredentialPrompts = false;
            reportViewer.ShowFindControls = false;
            reportViewer.ShowParameterPrompts = false;
            reportViewer.ServerReport.ReportPath = "/" + reportFolder + "/" + ReportName;
            reportViewer.ServerReport.SetParameters(parameter);
            reportViewer.ServerReport.Refresh();


            string[] streamids = null;
            Warning[] warnings;
            string mimeType;
            string encoding;
            string extension;
            reportViewer.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource(ReportServerPath));
            bytes = reportViewer.ServerReport.Render(
              "PDF", null, out mimeType, out encoding,
               out extension,
              out streamids, out warnings);
            return bytes;
        }
        catch (Exception ex)
        {

            var log = new clsExceptionLog(GetConnectionString());
            log.HandleException(ex);
            return Encoding.ASCII.GetBytes(ex.Message);
        }

    }

    public static string GetReportByName(string Name)
    {
        return ConfigurationManager.AppSettings[Name].ToString();
    }

    public static ReportInByteArray GetFtpReport(string FileName)
    {
        FileName = GetStringFromBase64Data(FileName);
        var reportData = new ReportInByteArray();
        try
        {
            // FileName = GetStringFromBase64Data(FileName);
           // FileName = "231_766.pdf";
            string ftppath = ConfigurationManager.AppSettings["FTP"];

            var FTPLocation = ftppath.Split('!')[0].ToString();
            var FTPUserName = ftppath.Split('!')[1].ToString();
            var FTPPassword = ftppath.Split('!')[2].ToString();
            var FTPFolder = ConfigurationManager.AppSettings["LabResultPath"];

            var filePath = FTPLocation + FTPFolder + "/" + FileName;
            //Create FTP Request.
            var request = (FtpWebRequest)WebRequest.Create(filePath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            //Enter FTP Server credentials.
            request.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;
            var index = FileName.LastIndexOf('.');
            string fileExtension = FileName.Substring(index).Replace(".", "");


            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        if (responseStream != null) responseStream.CopyTo(memoryStream);
                        //fileBytes = memoryStream.ToArray();
                        reportData.Report = memoryStream.ToArray();
                        reportData.Message = "OK";
                        reportData.Status = true;
                    }
                }
            }
           
            request.Abort();

        }
        catch (Exception Ex)
        {
            reportData.Message = Ex.Message;

        }

        return reportData;
    }
    public static string GetStringFromBase64Data(string Base64String)
    {
        try
        {
            var result = Encoding.UTF8.GetString(Convert.FromBase64String(Base64String));
            return result;
        }
        catch
        {

            return string.Empty;
        }


    }
}
