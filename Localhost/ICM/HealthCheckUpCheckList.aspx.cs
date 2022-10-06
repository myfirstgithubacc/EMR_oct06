using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;
using System.Configuration;

public partial class ICM_HealthCheckUpCheckList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hdnReportId.Value = common.myStr(Request.QueryString["ReportId"]);
            hdnEncounterId.Value = common.myStr(Session["EncounterId"]);
            hdnRegistrationId.Value = common.myStr(Session["RegistrationId"]);
            BindHealthCheckLists();
            if (common.myInt(Request.QueryString["PrintAllowed"]) == 0)
            {
                btnPrint.Visible = false;
            }
        }
    }
    private void BindHealthCheckLists()
    {
        DataSet ds = new DataSet();
        DataView dvFilter = new DataView();
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        try
        {
            ds = emr.getHealthCheckUpCheckLIsts(common.myInt(hdnEncounterId.Value), common.myInt(hdnRegistrationId.Value), common.myInt(hdnReportId.Value), common.myInt(Session["DoctorId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //dvFilter = new DataView(ds.Tables[0]);
                    //dvFilter.RowFilter = "IsData=0";
                    //if (dvFilter.ToTable().Rows.Count > 0)
                    //{
                    //    hdnAllowPrint.Value = "0";
                    //}

                    gvCheckListsTemplates.DataSource = ds.Tables[0];
                    gvCheckListsTemplates.DataBind();
                }
            }
            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    //dvFilter = new DataView(ds.Tables[1]);
                    //dvFilter.RowFilter = "IsData=0";
                    //if (dvFilter.ToTable().Rows.Count > 0)
                    //{
                    //    hdnAllowPrint.Value = "0";
                    //}
                    gvCheckListsStaticTemplates.DataSource = ds.Tables[1];
                    gvCheckListsStaticTemplates.DataBind();
                }
            }
            if (ds.Tables.Count > 2)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    //dvFilter = new DataView(ds.Tables[2]);
                    //dvFilter.RowFilter = "IsData=0";
                    //if (dvFilter.ToTable().Rows.Count > 0)
                    //{
                    //    hdnAllowPrint.Value = "0";
                    //}
                    gvCheckListsSections.DataSource = ds.Tables[2];
                    gvCheckListsSections.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            emr = null;
        }
    }
    protected void gvCheckLists_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkTemplate = (CheckBox)e.Row.FindControl("chkTemplate");
                HiddenField hdnIsData = (HiddenField)e.Row.FindControl("hdnIsData");
                if (common.myBool(hdnIsData.Value) == true)
                {
                    chkTemplate.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }
    protected void gvCheckListsStaticTemplates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkTemplate = (CheckBox)e.Row.FindControl("chkTemplate");
                HiddenField hdnIsData = (HiddenField)e.Row.FindControl("hdnIsData");
                if (common.myBool(hdnIsData.Value) == true)
                {
                    chkTemplate.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }
    protected void gvCheckListsSections_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkTemplate = (CheckBox)e.Row.FindControl("chkTemplate");
                HiddenField hdnIsData = (HiddenField)e.Row.FindControl("hdnIsData");
                if (common.myBool(hdnIsData.Value) == true)
                {
                    chkTemplate.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }
    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(hdnAllowPrint.Value) == 1)
        {
            RadWindow1.NavigateUrl = "/ICM/PrintHealthCheckUp.aspx?page=Ward&EncId=" + hdnEncounterId.Value
                    + "&DoctorId=" + hdnDoctorId.Value + "&RegId=" + hdnRegistrationId.Value + "&ReportId=" + hdnReportId.Value + "&HC=HC";
            RadWindow1.Height = 600;
            RadWindow1.Width = 800;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.Modal = true;
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.Behaviors = Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Minimize | Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Pin;
        }
        else
        {
            Alert.ShowAjaxMsg("Please enter all unchecked templates", Page);
            return;
        }
    }
    protected void btnPrintAdvice_OnClick(object sender, EventArgs e)
    {
        //if (common.myInt(hdnAllowPrint.Value) == 1)
        //{
            RadWindow1.NavigateUrl = "/ICM/HealthCheckUpCheckListPrintAdvice.aspx?Master=NO&PrintAllowed=0&ReportId=" + hdnReportId.Value;
            RadWindow1.Height = 700;
            RadWindow1.Width = 900;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.Modal = true;
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.Behaviors = Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Minimize | Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Pin;
        //}
        //else
        //{
        //    Alert.ShowAjaxMsg("Please enter all unchecked templates", Page);
        //    return;
        //}
    }
}
