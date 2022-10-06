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
using Telerik.Web.UI;


public partial class EMR_Dashboard_VisitRequiredReport : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string ddlAppointmentStatusColor = string.Empty;
    clsExceptionLog objException = new clsExceptionLog();
    private Hashtable hshInput;
    string strSQL;
    DAL.DAL dl;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        dtpfromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
        dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
        if (!Page.IsCallback)
        {
            if (Session["UserID"] == null)
            {
                //Response.Redirect("/Login.aspx?Logout=1", false);
            }
        }
        if (!IsPostBack)
        {
            dtpfromDate.SelectedDate = System.DateTime.Now.AddDays(-7);
            dtpToDate.SelectedDate = System.DateTime.Now;

            bindSearchData();
        }
    }


    protected void btnFillData_OnClick(object sender, EventArgs e)
    {
        btnSearch.Enabled = false;
        btn_ClearFilter.Enabled = false;

        try
        {
            btnSearch.Enabled = false;
            bindSearchData();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            btnSearch.Enabled = true;
            btn_ClearFilter.Enabled = true;
        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindSearchData();
    }

    protected void bindSearchData()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);

        try
        {
            string EnNo = string.Empty, Reg = string.Empty;

            if (ddlName.SelectedValue.Equals("R"))
            {
                Reg = txtSearchN.Text;
            }
            else if (ddlName.SelectedValue.Equals("ENC"))
            {
                EnNo = txtSearch.Text;
            }

            ds = emr.getPatientVisitRequiredReport(common.myInt(Session["FacilityId"]), dtpfromDate.SelectedDate.Value.ToString("yyyy-MM-dd"),
                                                dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd"), common.myInt(ddlSurgeryRequired.SelectedValue),
                                                common.myInt(ddlAdmissionRequired.SelectedValue), EnNo, Reg);
            gvEncounter.DataSource = ds;
            gvEncounter.DataBind();
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
            emr = null;
        }
    }

    protected void btn_ClearFilter_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        txtSearchN.Text = string.Empty;

        ddlName.SelectedIndex = 0;
        hdnSummaryIDToDispatch.Value = string.Empty;
        Label2.Visible = false;
        Label3.Visible = false;
        Label4.Visible = false;
        Label5.Visible = false;
        Label6.Visible = false;

        lblBillNo.Text = string.Empty;
        lblEncNo.Text = string.Empty;
        lblPatienName.Text = string.Empty;
        lblSummaryId.Text = string.Empty;
        lblRegNo.Text = string.Empty;


        bindSearchData();
    }

    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        txtSearchN.Visible = false;
        txtSearch.Visible = false;

        if (ddlName.SelectedValue.Equals("R"))
        {
            txtSearchN.Visible = true;
        }
        else if (ddlName.SelectedValue.Equals("ENC"))
        {
            txtSearch.Visible = true;
        }
    }

    protected void gvEncounter_OnPreRender(object sender, EventArgs e)
    {

    }

    protected void gvEncounter_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            gvEncounter.CurrentPageIndex = e.NewPageIndex;
            bindSearchData();
            Session["FindPatientPageIndexChanged"] = 1;

        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void gvEncounter_OnItemDataBound(object sender, GridItemEventArgs e)
    {

    }



    protected void rblDispatchStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindSearchData();

        hdnSummaryIDToDispatch.Value = string.Empty;
        Label2.Visible = false;
        Label3.Visible = false;
        Label4.Visible = false;
        Label5.Visible = false;
        Label6.Visible = false;

        lblBillNo.Text = string.Empty;
        lblEncNo.Text = string.Empty;
        lblPatienName.Text = string.Empty;
        lblSummaryId.Text = string.Empty;
        lblRegNo.Text = string.Empty;
    }
}