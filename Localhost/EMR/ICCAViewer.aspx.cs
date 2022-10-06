using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Telerik.Web.UI;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
public partial class EMR_ICCAViewer : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //lblPatientDetail.Text =Session["PatientDetailString"].ToString() ;
        if (!IsPostBack)
        {
            dtpfrom.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
            dtpfrom.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
            dtpfrom.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])) + " 00:00");

            dtpTodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
            dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])) + " HH:mm");

            BindParameters();
            bindData();
        }
    }
    //protected void btnView_click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        gvICCAdata.Visible = false;
    //        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        DataSet ds = dl.FillDataSet(CommandType.Text, "[uspEMRGetVitalsFromICCASummary] " + common.myInt(Session["FacilityID"]).ToString() + ",'" + common.myDate(dtpfrom.SelectedDate).ToString("yyyy/MM/dd").Replace("-", "/") + "','" + common.myDate(dtpTodate.SelectedDate).ToString("yyyy/MM/dd").Replace("-", "/") + "','" + common.myInt(Session["RegistrationNo"]) + "'");
    //        ViewState["vdt"] = ds;
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            GVsummary.DataSource = ds.Tables[0];
    //            GVsummary.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblmessage.Text = ex.Message;
    //        //Alert.ShowAjaxMsg(ex.Message, this); 
    //    }
    //}
    //protected void GVsummary_PageIndexChanging(object sernder, GridViewPageEventArgs  e)
    //{
    //    GVsummary.PageIndex = e.NewPageIndex;
    //    DataSet ds = (DataSet)ViewState["vdt"];

    //        GVsummary.DataSource = ds.Tables[0];
    //        GVsummary.DataBind();        

    //}
    private void BindParameters()
    {
        DataSet ds = new DataSet();
        BaseC.EMRVitals vital = new BaseC.EMRVitals(sConString);
        try
        {
            ds = vital.GetVitalSignType();
            ddlParameters.DataSource = ds.Tables[0];
            ddlParameters.DataTextField = "VitalSignName";
            ddlParameters.DataValueField = "VitalId";
            ddlParameters.DataBind();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            lblmessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            vital = null;
        }
    }
    protected void gvICCAdata_PageIndexChanging(object sernder, GridViewPageEventArgs e)
    {
        gvICCAdata.PageIndex = e.NewPageIndex;
        DataSet ds = (DataSet)ViewState["ICCA"];
        gvICCAdata.DataSource = ds.Tables[0];
        gvICCAdata.DataBind();
    }

    //protected void gvICCAdata_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        CheckBox chk = (CheckBox)e.Row.FindControl("chk");
    //        HiddenField hdn = (HiddenField)e.Row.FindControl("hdnImport");
    //        if (hdn.Value != "NI")
    //        {
    //            e.Row.BackColor = System.Drawing.Color.Green;
    //            chk.Visible = false;
    //        }
    //    }
    //}

    protected void btnImportData_OnClick(object sender, EventArgs e)
    {
        try
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (GridViewRow gvr in gvICCAdata.Rows)
            {
                CheckBox chk = (CheckBox)gvr.FindControl("chk");
                if (chk.Checked == true)
                {
                    HiddenField hdn = (HiddenField)gvr.FindControl("hdnID");
                    str.Append(hdn.Value.ToString() + ",");
                }

            }
            if (str.Length > 0)
            {
                str.Remove(str.Length - 1, 1);
                BaseC.EMRVitals vital = new BaseC.EMRVitals(sConString);
                Hashtable hshOut = vital.EMRImportVitalsFromICCA(common.myInt(Session["FacilityID"]), str.ToString(), common.myInt(Session["UserId"]));
                if (!common.myStr(hshOut["@chvOutPut"]).Contains("save"))
                {
                    lblmessage.Text = "Oops!Something went Wrong, Reveiw Error message for more information.";
                    lblmessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                }
                else
                {
                    lblmessage.Text = "Record's Updated Successfully.";
                    lblmessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    bindData();
                }
                vital = null;
                hshOut = null;
            }
            else
            {
                lblmessage.Text = "Please Select Parameter To Import.";
                lblmessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            lblmessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
    }

    protected void btnView_click(object sender, EventArgs e)
    {
        try
        {
            bindData();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            lblmessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //Alert.ShowAjaxMsg(ex.Message, this); 
        }
    }
    private void bindData()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = dl.FillDataSet(CommandType.Text, "[uspEMRGetVitalsFromICCA] " + common.myInt(Session["RegistrationId"]).ToString() + ","
            + common.myInt(Session["FacilityID"]).ToString() + ",'" + common.myDate(dtpfrom.SelectedDate).ToString("yyyy/MM/dd hh:mmtt").Replace("/", "-") + "','"
            + common.myDate(dtpTodate.SelectedDate).ToString("yyyy/MM/dd hh:mmtt").Replace("/", "-") +
            "','" + common.myInt(ddlParameters.SelectedValue) + "'");
        ViewState["ICCA"] = ds;
        if (ds.Tables[0].Rows.Count > 0)
            gvICCAdata.DataSource = ds.Tables[0];
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("RegistrationNo");
            dt.Columns.Add("EncounterNo");
            dt.Columns.Add("OutputDate");
            dt.Columns.Add("MiracleParameter");
            dt.Columns.Add("ParameterName");
            dt.Columns.Add("OBX");
            dt.Columns.Add("Import");
            dt.Columns.Add("IsTransmitted");
            dt.Columns.Add("IsMapped");
            DataRow dr = dt.NewRow();
            dr["Id"] = 0;
            dr["RegistrationNo"] = string.Empty;
            dr["EncounterNo"] = string.Empty;
            dr["OutputDate"] = string.Empty;
            dr["MiracleParameter"] = string.Empty;
            dr["ParameterName"] = string.Empty;
            dr["OBX"] = string.Empty;
            dr["Import"] = string.Empty;
            dr["IsTransmitted"] = string.Empty;
            dr["IsMapped"] = false;
            dt.Rows.Add(dr);
            gvICCAdata.DataSource = dt;
            dt.Dispose();
        }
        gvICCAdata.DataBind();
        ds.Dispose();
        dl = null;
    }
    protected void btnviechartm_OnClick(object sender, EventArgs e)
    {

        RadWindowPopup.NavigateUrl = "/EMR/ICCAChartView.aspx?From=POPUP&FromDate=" + common.myDate(dtpfrom.SelectedDate).ToString("yyyy/MM/dd").Replace("/", "-")
                                            + "&ToDate=" + common.myDate(dtpTodate.SelectedDate).ToString("yyyy/MM/dd").Replace("/", "-");
        RadWindowPopup.Height = 550;
        RadWindowPopup.Width = 850;
        RadWindowPopup.Top = 10;
        RadWindowPopup.Left = 10;
        RadWindowPopup.VisibleOnPageLoad = true;
        RadWindowPopup.Modal = true;
        RadWindowPopup.VisibleStatusbar = false;
        RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void gvICCAdata_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chk = (CheckBox)e.Row.FindControl("chk");
            HiddenField hdnIsMapped = (HiddenField)e.Row.FindControl("hdnIsMapped");
            HiddenField hdnIsTransmitted = (HiddenField)e.Row.FindControl("hdnIsTransmitted");
            if (hdnIsMapped != null && !common.myBool(hdnIsMapped.Value) && hdnIsTransmitted != null && !string.IsNullOrEmpty(hdnIsTransmitted.Value))
                e.Row.BackColor = System.Drawing.Color.FromName("#D0ECBB");
            else if (hdnIsTransmitted != null && common.myBool(hdnIsTransmitted.Value))
            {
                e.Row.BackColor = System.Drawing.Color.FromName("#ECBBBB");
            }
        }
    }
}
