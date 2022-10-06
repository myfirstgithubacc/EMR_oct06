using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Configuration;


public partial class ICM_DischargeSummaryAuditTrail : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["DischargeSummaryAuditTrail"] = null;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

            txtFromDate.SelectedDate = DateTime.Now.AddMonths(-1);
            txtToDate.SelectedDate = DateTime.Now;

            bindControl();
            bindDetails();
        }
    }

    private void bindControl()
    {
        BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        try
        {
            ds = ObjIcm.GetICMSignDoctors(common.myInt(Session["HospitalLocationID"]),common.myInt(Session["FacilityId"]));

            DV = ds.Tables[0].DefaultView;
            DV.RowFilter = "IsDoctor=1";

            ddlDeFinalizeRecommendBy.DataSource = DV.ToTable();
            ddlDeFinalizeRecommendBy.DataTextField = "DoctorName";
            ddlDeFinalizeRecommendBy.DataValueField = "ID";
            ddlDeFinalizeRecommendBy.DataBind();

            ddlDeFinalizeRecommendBy.Items.Insert(0, new RadComboBoxItem("", ""));
            ddlDeFinalizeRecommendBy.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            ObjIcm = null;
            ds.Dispose();
            DV.Dispose();
        }
    }

    protected void btnFilter_OnClick(Object sender, EventArgs e)
    {
        bindDetails();
    }

    private void bindDetails()
    {
        BaseC.ICM objIcm = new BaseC.ICM(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objIcm.getDischargeSummaryAuditTrail(common.myInt(Session["FacilityId"]), common.myStr(txtEncounterNo.Text).Trim(), common.myLong(txtRegistrationNo.Text),
                                                    common.myStr(txtPatientName.Text.Trim(), true), common.myInt(ddlDeFinalizeRecommendBy.SelectedValue),
                                                    Convert.ToDateTime(txtFromDate.SelectedDate), Convert.ToDateTime(txtToDate.SelectedDate));

            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
                ds.Tables[0].AcceptChanges();
            }
            ViewState["DischargeSummaryAuditTrail"] = (DataTable)ds.Tables[0];
            gvDetails.DataSource = ds.Tables[0];
            gvDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objIcm = null;
            ds.Dispose();
        }
    }

    protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            if (e.CommandName.ToUpper().Equals("VIEW"))
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    int summaryId = common.myInt(e.CommandArgument);


                    RadWindow2.NavigateUrl = "~/ICM/DischargePatientSummaryData.aspx?SummaryId=" + summaryId;
                    RadWindow2.Height = 450;
                    RadWindow2.Width = 600;
                    RadWindow2.Top = 10;
                    RadWindow2.Left = 10;
                    RadWindow2.VisibleOnPageLoad = true;
                    //RadWindow2.OnClientClose = "OnClientDeFinalisationClose";
                    RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow2.Modal = true;
                    RadWindow2.VisibleStatusbar = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDetails.PageIndex = e.NewPageIndex;
        bindDetails();
    }

    protected void btnExport_OnClick(Object sender, EventArgs e)
    {
        if (ViewState["DischargeSummaryAuditTrail"] != null)
        {
            DataTable dt = ((DataTable)ViewState["DischargeSummaryAuditTrail"]).Copy();

            if (dt != null)
            {
                dt.Columns.Remove("SummaryId");
                dt.Columns["Format"].ColumnName = "Format";
                dt.Columns["RegistrationNo"].ColumnName = "Reg. No.";
                dt.Columns["EncounterNo"].ColumnName = "IP No.";
                dt.Columns["PatientName"].ColumnName = "Patient Name";
                dt.Columns["FinalizeDateTime"].ColumnName = "Finalized Date Time";
                dt.Columns["FinalizeByName"].ColumnName = "Finalized By";
                dt.Columns["DeFinalizeDateTime"].ColumnName = "Definalized Date Time";
                dt.Columns["DeFinalizeByName"].ColumnName = "Definalized By";
                dt.Columns["DeFinalizeRecommendByName"].ColumnName = "Recommend By (Doctor)";
                dt.Columns["DeFinalizeReason"].ColumnName = "Reason";

                string FileName = "DischargeSummaryAuditTrail.xls";

                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();
                dgGrid.RenderControl(hw);

                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }
    }

}