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
using Telerik.Web.UI;



public partial class EMRBILLING_Popup_ExpectedDateOfDischarge : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    DataSet ds;
    BaseC.WardManagement objwd;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tblDateRange.Visible = false;

            dtpfromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);

            dtpfromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = System.DateTime.Now;
            BindGrid();
        }
    }

    void BindGrid()
    {
        objwd = new BaseC.WardManagement();
        ds = new DataSet();
        BaseC.Patient pt = new BaseC.Patient(sConString);
        int count=0;
        string FDate = pt.FormatDateYearMonthDate(common.myStr(dtpfromDate.SelectedDate).Trim());
        string TDate = pt.FormatDateYearMonthDate(common.myStr(dtpToDate.SelectedDate).Trim());

        ds = objwd.GetExpectedDateOfDischargePatient(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 
                                                common.myStr(ddlrange.SelectedValue), FDate, TDate, string.Empty, 0);
        if (ds.Tables[0].Rows.Count == 0)
        {
            count = 1;
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.AcceptChanges();

        }
        gvAdmission.DataSource = ds;
        gvAdmission.DataBind();
        if (count == 0)
        {
            lblNoOfDischarge.Text = "(" + common.myStr(ds.Tables[0].Rows.Count) + ")";
        }
        else
        {
            lblNoOfDischarge.Text = "(" + 0 + ")";
        }
    }
    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        exportToExcell();
    }
    private void exportToExcell()
    {
        objwd = new BaseC.WardManagement();
        ds = new DataSet();
        BaseC.Patient pt = new BaseC.Patient(sConString);

        try
        {
            string FDate = pt.FormatDateYearMonthDate(common.myStr(dtpfromDate.SelectedDate).Trim());
            string TDate = pt.FormatDateYearMonthDate(common.myStr(dtpToDate.SelectedDate).Trim());
            ds = objwd.GetExpectedDateOfDischargePatient(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 
                                                        common.myStr(ddlrange.SelectedValue), FDate, TDate, string.Empty, 0);

            string filename = "DownloadExcelFile.xls";
            if (ds.Tables[0].Rows.Count > 0)
            {
                // string filename = "DownloadExcelFile.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = ds.Tables[0];
                dgGrid.DataBind();
                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }
        catch (Exception Ex)
        {

        }
    }
    protected void ddlrange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (common.myStr(ddlrange.SelectedValue) == "DR")
        {
            tblDateRange.Visible = true;
        }
        else
        {
            tblDateRange.Visible = false;
        }
        ViewState["SelectedDate"] = ddlrange.SelectedValue;
        BindGrid(); ;
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
}
