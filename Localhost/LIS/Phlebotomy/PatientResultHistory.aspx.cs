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
using System.Drawing;

public partial class LIS_Phlebotomy_PatientResultHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            //txtFromDate.SelectedDate = System.DateTime.Now.AddYears(-1);
            //txtToDate.SelectedDate = System.DateTime.Now.AddYears(1);
            lblPatientName.Text = common.myStr(Request.QueryString["PName"]);
            lblAgeGender.Text = common.myStr(Request.QueryString["AgeGender"]);
            lblRegNo.Text = common.myStr(Request.QueryString["EncounterNo"]);
            lblServiceName.Text = " (" + common.myStr(Request.QueryString["ServiceName"]) + ")";
            BindResultGrid();
        }
    }
    private void BindResultGrid()
    {
        BaseC.clsLISPhlebotomy objLis = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        int HospId = common.myInt(Session["HospitalLocationId"]);
        int FacilityId = common.myInt(Session["FacilityId"]);
        int RegId = common.myInt(Request.QueryString["RegId"]);
        int ServiceId = common.myInt(Request.QueryString["ServiceId"]);
        int RecordCount = common.myInt(txtNoOfResult.Text);
        if (ddlrange.SelectedValue == "4" || ddlrange.SelectedValue == "WW-1" ||
            ddlrange.SelectedValue == "MM-1" || ddlrange.SelectedValue == "MM0"
            || ddlrange.SelectedValue == "MM-3" || ddlrange.SelectedValue == "MM-6"
            || ddlrange.SelectedValue == "YY-1")
        //
        {
            string[] str = getToFromDate(ddlrange.SelectedValue);
            txtFromDate.SelectedDate = Convert.ToDateTime(str[0]);
            txtToDate.SelectedDate = Convert.ToDateTime(str[1]);

        }
        if (ddlrange.SelectedValue == "DD0")
        {
            txtFromDate.SelectedDate = DateTime.Now.Date;
            txtToDate.SelectedDate = DateTime.Now.Date; 
        }
        //else

        ds = objLis.getPatientLabResultHistory(common.myDate(txtFromDate.SelectedDate),
            common.myDate(txtToDate.SelectedDate), RegId, ServiceId, RecordCount, "", 0);
        if (ds.Tables.Count > 0)
        {
            if (gvResult.Columns.Count > 0)
            {
                gvResult.Columns.Clear();
            }
            gvResult.DataSource = ds;
            gvResult.AutoGenerateColumns = false;
            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                BoundField objbf = new BoundField();
                objbf.DataField = dc.ColumnName;
                objbf.HeaderText = dc.ColumnName;
                gvResult.Columns.Add(objbf);
            }
            gvResult.DataBind();
        }
    }
    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        BindResultGrid();

    }
    protected void ddlrange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlrange.SelectedValue == "4")
        {
            tblDateRange.Visible = true;
        }
        else
        {
            tblDateRange.Visible = false;
        }
        //BindColorLegend();
    }
    private string[] getToFromDate(string ddlTime)
    {
        int timezone = BindUTCTime();
        string sFromDate = "", sToDate = "";
        string[] str = new string[2];

        if (ddlTime == "4")
        {
            //tdDateRange.Visible = true;
            if (txtFromDate.SelectedDate != null && txtToDate.SelectedDate != null)
            {
                sFromDate = Convert.ToString(Convert.ToDateTime(txtFromDate.SelectedDate.Value).AddDays(-1).ToString("yyyy-MM-dd hh:mm"));
                sToDate = Convert.ToString(txtToDate.SelectedDate.Value.ToString("yyyy-MM-dd hh:mm"));
            }
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "WW-1")
        {
            str = common.datecalculate();

        }
        else if (ddlTime == "MM0")
        {
            sFromDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01" + " 00:00";
            sToDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + " 23:59";

            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "YY-1")
        {
            sFromDate = (DateTime.Now.Year - 1).ToString() + "/" + DateTime.Now.Month.ToString() + "/01" + " 00:00";
            sToDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + " 23:59";

            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "MM-1")
        {
            if ((DateTime.Now.Month - 1) != 0)
            {
                sFromDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 1).ToString() + "/01" + " 00:00";
                sToDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 1).ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, (DateTime.Now.Month - 1)) + " 23:59";
            }
            else
            {
                sFromDate = (DateTime.Now.Year - 1).ToString() + "/" + 12.ToString() + "/01" + " 00:00";
                sToDate = (DateTime.Now.Year - 1).ToString() + "/" + 12.ToString() + "/" + DateTime.DaysInMonth((DateTime.Now.Year - 1), 12) + " 23:59";

            }
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "MM-3")
        {
            if ((DateTime.Now.Month - 3) != 0)
            {
                sFromDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 3).ToString() + "/01" + " 00:00";
                sToDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month-1).ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, (DateTime.Now.Month - 1)) + " 23:59";
            }
            else
            {
                sFromDate = (DateTime.Now.Year - 3).ToString() + "/" + 12.ToString() + "/01" + " 00:00";
                sToDate = (DateTime.Now.Year).ToString() + "/" + 12.ToString() + "/" + DateTime.DaysInMonth((DateTime.Now.Year - 1), 12) + " 23:59";

            }
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "MM-6")
        {
            if ((DateTime.Now.Month - 6) != 0)
            {
                sFromDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 6).ToString() + "/01" + " 00:00";
                sToDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 1).ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, (DateTime.Now.Month - 1)) + " 23:59";
            }
            else
            {
                sFromDate = (DateTime.Now.Year - 6).ToString() + "/" + 12.ToString() + "/01" + " 00:00";
                sToDate = (DateTime.Now.Year).ToString() + "/" + 12.ToString() + "/" + DateTime.DaysInMonth((DateTime.Now.Year - 1), 12) + " 23:59";

            }
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        return str;
    }
    protected int BindUTCTime()
    {
        int timezone = 0;
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@intfacilityid", Convert.ToInt32(Session["FacilityID"]));
            string strtimezone = "Select TimeZoneOffSetMinutes  from FacilityMaster where FacilityID=@intfacilityid";
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.Text, strtimezone, hsinput);
            timezone = Convert.ToInt32(ds.Tables[0].Rows[0]["TimeZoneOffSetMinutes"].ToString());
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return timezone;
    }
    protected void gvResult_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            int ColumnCount = e.Row.Cells.Count;
            for (int i = 1; i != ColumnCount; i++)
            {
                i = i + 1;
                e.Row.Cells[i].Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int ColumnCount = e.Row.Cells.Count;

            for (int i = 1; i != ColumnCount; i++)
            {
                i = i + 1;
                if (e.Row.Cells[i].Text.Contains('A').ToString() == "True")
                {
                    e.Row.Cells[i - 1].ForeColor = System.Drawing.Color.DarkViolet; 
                }
                if (e.Row.Cells[i].Text.Contains('C').ToString() == "True")
                {
                    e.Row.Cells[i - 1].ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}


