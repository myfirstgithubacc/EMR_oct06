using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMRReports_customreportsnew : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog except = new clsExceptionLog();
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;

    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // BindMISReportName();
            ddlFiltertype.SelectedIndex = 3;
            ddlFiltertype.Visible = false;
            ddlFiltertype_SelectedIndexChanged(null, null);
            ddlYearYear.SelectedIndex = 2;
            ddlYearYear_SelectedIndexChanged(null, null);
            //ShowReport();
        }
    }

    protected void ddlFiltertype_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            //if (ddlFiltertype.SelectedValue == "D")
            //{
            //    trDateWise.Visible = true;
            //    trMonthWise.Visible = false;
            //    trQuatrlyWise.Visible = false;
            //    trYearWise.Visible = false;

            //}
            //else if (ddlFiltertype.SelectedValue == "M")
            //{
            //    trDateWise.Visible = false;
            //    trMonthWise.Visible = true;
            //    trQuatrlyWise.Visible = false;
            //    trYearWise.Visible = false;
            //    ddlYearMonth.DataSource = null;
            //    // ds = GetYearData(rdoYearAs.SelectedValue, "y");
            //    if (ds.Tables.Count > 0)
            //    {
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            ddlYearMonth.Items.Clear();
            //            ddlYearMonth.DataSource = ds;
            //            ddlYearMonth.DataValueField = "ID";
            //            ddlYearMonth.DataTextField = "Name";
            //            ddlYearMonth.DataBind();
            //        }
            //    }
            //    // ds1 = GetYearData(rdoYearAs.SelectedValue, ddlFiltertype.SelectedValue);
            //    ddlMonthMonth.DataSource = null;
            //    if (ds1.Tables.Count > 0)
            //    {
            //        if (ds1.Tables[0].Rows.Count > 0)
            //        {
            //            ddlMonthMonth.Items.Clear();
            //            ddlMonthMonth.DataSource = ds1;
            //            ddlMonthMonth.DataValueField = "ID";
            //            ddlMonthMonth.DataTextField = "Name";
            //            ddlMonthMonth.DataBind();
            //        }
            //    }


            //}
            //else if (ddlFiltertype.SelectedValue == "Q")
            //{
            //    trDateWise.Visible = false;
            //    trMonthWise.Visible = false;
            //    trQuatrlyWise.Visible = true;
            //    trYearWise.Visible = false;
            //    ddlYearQuatrly.DataSource = null;
            //    // ds = GetYearData(rdoYearAs.SelectedValue, "y");
            //    if (ds.Tables.Count > 0)
            //    {
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            ddlYearQuatrly.Items.Clear();
            //            ddlYearQuatrly.DataSource = ds;
            //            ddlYearQuatrly.DataValueField = "ID";
            //            ddlYearQuatrly.DataTextField = "Name";
            //            ddlYearQuatrly.DataBind();
            //        }
            //    }
            //    //   ds1 = GetYearData(rdoYearAs.SelectedValue, ddlFiltertype.SelectedValue);
            //    if (ds1.Tables.Count > 0)
            //    {
            //        if (ds1.Tables[0].Rows.Count > 0)
            //        {
            //            ddlMonthQuatrly.Items.Clear();
            //            ddlMonthQuatrly.DataSource = ds1;
            //            ddlMonthQuatrly.DataValueField = "ID";
            //            ddlMonthQuatrly.DataTextField = "Name";
            //            ddlMonthQuatrly.DataBind();
            //        }
            //    }
            //}
            if (ddlFiltertype.SelectedValue == "Y")
            {
                trDateWise.Visible = false;
                trMonthWise.Visible = false;
                trQuatrlyWise.Visible = false;
                trYearWise.Visible = true;

                // ds = GetYearData(rdoYearAs.SelectedValue, "y");
                ds = GetYearData("Y", "y");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlYearYear.Items.Clear();
                        ddlYearYear.DataSource = ds;
                        ddlYearYear.DataValueField = "ID";
                        ddlYearYear.DataTextField = "Name";
                        ddlYearYear.DataBind();
                    }
                }

            }

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            except.HandleException(ex);
        }
    }

    public DataSet GetYearData(string typeofYear, string ReportType)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@IsFy", typeofYear);
            HshIn.Add("@Type", ReportType);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetYearMonthQuaterData", HshIn);
            return ds;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            except.HandleException(ex);
            return ds;
        }
        finally
        {
            ds = null;
        }
    }

    protected void ShowReport()
    {
        if (common.myInt(ddlYearYear.SelectedIndex) >= 0)
        {
            if (Session["IsLoginDoctor"] != null)
            {
                if (common.myBool(Session["IsLoginDoctor"]))

                {
                    if (Session["FacilityID"] != null && Session["LoginDoctorId"] != null)
                    {

                        string ReportServerPath = "http://" + reportServer + "/ReportServer";
                        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
                        #region MisDashBoardProviderAdmission

                        ReportViewerAdmission.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
                        ReportViewerAdmission.ServerReport.ReportServerCredentials = irsc;
                        ReportViewerAdmission.ProcessingMode = ProcessingMode.Remote;
                        ReportViewerAdmission.ShowCredentialPrompts = false;
                        ReportViewerAdmission.ShowFindControls = false;
                        ReportViewerAdmission.ShowParameterPrompts = false;


                        ReportParameter[] p = new ReportParameter[7];
                        ReportViewerAdmission.ServerReport.ReportPath = "/" + reportFolder + "/MisDashBoardProviderAdmission";
                        p[0] = new ReportParameter("Fdate", common.myStr(ddlYearYear.SelectedValue));
                        p[1] = new ReportParameter("Tdate", common.myStr("0"));
                        p[2] = new ReportParameter("FilterType", common.myStr("Y"));
                        p[3] = new ReportParameter("DoctorId", common.myStr(Session["LoginDoctorId"]));
                      //  p[3] = new ReportParameter("DoctorId", "1252");
                        p[4] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityID"]));
                        p[5] = new ReportParameter("DeptId", "A");
                        p[6] = new ReportParameter("SpecialityId", "A");
                        ReportViewerAdmission.ServerReport.SetParameters(p);
                        ReportViewerAdmission.ServerReport.Refresh();
                        #endregion

                        #region MisDashBoardProviderOPOrderPrescription
                        ReportViewerOrder.ServerReport.ReportServerUrl = new Uri(ReportServerPath);

                        ReportViewerOrder.ServerReport.ReportServerCredentials = irsc;
                        ReportViewerOrder.ProcessingMode = ProcessingMode.Remote;
                        ReportViewerOrder.ShowCredentialPrompts = false;
                        ReportViewerOrder.ShowFindControls = false;
                        ReportViewerOrder.ShowParameterPrompts = false;

                        ReportParameter[] p2 = new ReportParameter[7];
                        ReportViewerOrder.ServerReport.ReportPath = "/" + reportFolder + "/MisDashBoardProviderOPOrderPrescription";
                        p2[0] = new ReportParameter("Fdate", common.myStr(ddlYearYear.SelectedValue));
                        p2[1] = new ReportParameter("Tdate", common.myStr("0"));
                        p2[2] = new ReportParameter("FilterType", common.myStr("Y"));
                         p2[3] = new ReportParameter("DoctorId", common.myStr(Session["LoginDoctorId"]));
                       // p2[3] = new ReportParameter("DoctorId", "1252");
                        p2[4] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityID"]));
                        p2[5] = new ReportParameter("DeptId", "A");
                        p2[6] = new ReportParameter("SpecialityId", "A");
                        ReportViewerOrder.ServerReport.SetParameters(p2);
                        ReportViewerOrder.ServerReport.Refresh();
                        #endregion

                        #region MisDashBoardProviderOPVisit
                        ReportViewerOPVisit.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
                        ReportViewerOPVisit.ServerReport.ReportServerCredentials = irsc;
                        ReportViewerOPVisit.ProcessingMode = ProcessingMode.Remote;
                        ReportViewerOPVisit.ShowCredentialPrompts = false;
                        ReportViewerOPVisit.ShowFindControls = false;
                        ReportViewerOPVisit.ShowParameterPrompts = false;

                        ReportParameter[] p3 = new ReportParameter[7];
                        ReportViewerOPVisit.ServerReport.ReportPath = "/" + reportFolder + "/MisDashBoardProviderOPVisit";
                        p3[0] = new ReportParameter("Fdate", common.myStr(ddlYearYear.SelectedValue));
                        p3[1] = new ReportParameter("Tdate", common.myStr("0"));
                        p3[2] = new ReportParameter("FilterType", common.myStr("Y"));
                        p3[3] = new ReportParameter("DoctorId", common.myStr(Session["LoginDoctorId"]));
                       // p3[3] = new ReportParameter("DoctorId", "1252");
                        p3[4] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityID"]));
                        p3[5] = new ReportParameter("DeptId", "A");
                        p3[6] = new ReportParameter("SpecialityId", "A");
                        ReportViewerOPVisit.ServerReport.SetParameters(p3);
                        ReportViewerOPVisit.ServerReport.Refresh();
                        #endregion

                        #region MisDashBoardProviderRevenue
                        ReportViewerRevenueComparision.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
                        ReportViewerRevenueComparision.ServerReport.ReportServerCredentials = irsc;
                        ReportViewerRevenueComparision.ProcessingMode = ProcessingMode.Remote;
                        ReportViewerRevenueComparision.ShowCredentialPrompts = false;
                        ReportViewerRevenueComparision.ShowFindControls = false;
                        ReportViewerRevenueComparision.ShowParameterPrompts = false;

                        ReportParameter[] p4 = new ReportParameter[7];
                        ReportViewerRevenueComparision.ServerReport.ReportPath = "/" + reportFolder + "/MisDashBoardProviderRevenue";
                        p4[0] = new ReportParameter("Fdate", common.myStr(ddlYearYear.SelectedValue));
                        p4[1] = new ReportParameter("Tdate", common.myStr("0"));
                        p4[2] = new ReportParameter("FilterType", common.myStr("Y"));
                         p4[3] = new ReportParameter("DoctorId", common.myStr(Session["LoginDoctorId"]));
                       // p4[3] = new ReportParameter("DoctorId", "1252");
                        p4[4] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityID"]));
                        p4[5] = new ReportParameter("DeptId", "A");
                        p4[6] = new ReportParameter("SpecialityId", "A");
                        ReportViewerRevenueComparision.ServerReport.SetParameters(p4);
                        ReportViewerRevenueComparision.ServerReport.Refresh();

                        #endregion
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Doctor Details Not Available";

                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Doctor Details Not Available";
            }
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select the year";
        }

    }
    protected void ddlYearYear_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ShowReport();
    }



}