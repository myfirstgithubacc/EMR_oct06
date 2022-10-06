using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RisTextReport : System.Web.UI.Page
{
    string fromdate = "";
    string todate = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["AdmissionDate"] != "" && Request.QueryString["AdmissionDate"] != null)
            {
                dtpFromdate.SelectedDate = Convert.ToDateTime(Request.QueryString["AdmissionDate"]);
                dtpTodate.SelectedDate = DateTime.Now;
            }
            else
            {
                dtpFromdate.SelectedDate = DateTime.Now;
                dtpTodate.SelectedDate = DateTime.Now;
            }
            if (Request.QueryString["OpenFrom"] == "SS")
                btnUpdateSummary.Visible = false;
            else
                btnUpdateSummary.Visible = true;
           
            GetReport();
        }


    }
    protected void GetReport()
    {
        fromdate = CreateDate(Convert.ToDateTime(dtpFromdate.SelectedDate));
        todate = CreateDate(Convert.ToDateTime(dtpTodate.SelectedDate));
        ServiceReferenceForRIS.TextReport obj = new ServiceReferenceForRIS.TextReport();
        try
        {
            string responce = obj.getTextReport(common.myStr(Request.QueryString["IPno"]), common.myStr(fromdate), common.myStr(todate));

            if (responce.Length != 0)
            {
                responce = responce.Replace("{", string.Empty);
                responce = responce.Replace("}", string.Empty);
                responce = responce.Replace("<o:p>", string.Empty);
                responce = responce.Replace("</o:p>", string.Empty);
                responce = responce.Replace("&nbsp;", string.Empty);
                responce = responce.Replace("<br>", string.Empty);

                ShowData.InnerHtml = responce;

                responce = responce.Replace("Dr. Pravin Vaze", string.Empty);
                responce = responce.Replace("Sr. Consultant Radiologist ", string.Empty);
                responce = responce.Replace("DMC - 6734", string.Empty);

                responce = responce.Replace("Dr. Savinay Kapur", string.Empty);
                responce = responce.Replace("MBBS, MD, DNB, MNAMS,", string.Empty);
                responce = responce.Replace("Fellow (GI Radiology)", string.Empty);
                responce = responce.Replace("Consultant Radiologist", string.Empty);
                responce = responce.Replace("DMC No &#8211; DMC/R/8671", string.Empty);
                
                System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
                responce= rx.Replace(responce, "");

                responce = responce.Replace("TestReport:", "<br/><br/>");
                responce = responce.Replace("Date", "<br/><br/> <p> Date").Trim();
                responce = responce.Replace('"', ' ').Trim();
                responce = responce.Replace(',', ' ').Trim();
                responce = responce.Replace("Investigation_Name:", string.Empty);  



                hdnRisdata.Value = responce;
                // hdnRisdata.Value =  responce;

            }
            else
            {
                hdnRisdata.Value = "Hi Kuldeep";
               
                ShowData.InnerHtml = "Report not found";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            obj = null;
        }

    }

    protected string CreateDate(DateTime Date)
    {
        int day = Date.Day; string month = Date.ToString("MMMM"); int year = Date.Year;
        return day.ToString() + "-" + month.Substring(0, 3) + "-" + year.ToString();
    }

    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        GetReport();
    }

    protected void btnUpdateSummary_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }
}