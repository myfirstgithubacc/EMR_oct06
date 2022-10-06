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
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;
using Telerik.Web.UI;

public partial class EMR_Dashboard_PatientParts_PatientLabDashboard : System.Web.UI.UserControl
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
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            clearControl();
            tblDate.Visible = false;
        }

        bindTestData();

        legend();
    }

    public void legend()
    {
        Label LBL;
        TableRow tr = new TableRow();
        TableCell td;
        int colIdx = 0;

        td = new TableCell();
        LBL = new Label();
        LBL.BorderWidth = Unit.Pixel(1);
        LBL.ID = "LabelStatusColor" + colIdx;
        LBL.BackColor = System.Drawing.Color.LightYellow;
        LBL.SkinID = "label";
        LBL.Width = Unit.Pixel(18);
        LBL.Height = Unit.Pixel(14);

        td.Controls.Add(LBL);
        tr.Cells.Add(td);

        td = new TableCell();
        LBL = new Label();
        LBL.ID = "LabelStatus" + colIdx;
        LBL.Text = "Result Abnormal";
        LBL.Font.Size = 8;

        LBL.SkinID = "label";

        td.Controls.Add(LBL);
        tr.Cells.Add(td);
        colIdx++;


        td = new TableCell();
        LBL = new Label();
        LBL.BorderWidth = Unit.Pixel(1);
        LBL.ID = "LabelStatusColor" + colIdx;
        LBL.BackColor = System.Drawing.Color.LightGreen;
        LBL.SkinID = "label";
        LBL.Width = Unit.Pixel(18);
        LBL.Height = Unit.Pixel(14);

        td.Controls.Add(LBL);
        tr.Cells.Add(td);

        td = new TableCell();
        LBL = new Label();
        LBL.ID = "LabelStatus" + colIdx;
        LBL.Text = "Result Reviewed";
        LBL.Font.Size = 8;

        LBL.SkinID = "label";

        td.Controls.Add(LBL);
        tr.Cells.Add(td);
        colIdx++;

        tblLegend.Rows.Add(tr);
    }

    private void clearControl()
    {
        lblMessage.Text = "";

        txtFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
        txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        txtToDate.SelectedDate = DateTime.Now;
        txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    public void bindTestData()
    {
        try
        {
            int encounterid = 0;

            if (common.myInt(hdnEncounterNumber.Text) > 0)
            {
                encounterid = common.myInt(hdnEncounterNumber.Text);
            }
           

            setDate();

            BaseC.clsLISLabOther objval = new BaseC.clsLISLabOther(sConString);
                            //objval.GetLabTestResultIP
            DataSet ds = objval.GetLabTestResult(common.myInt(Session["HospitalLocationID"]),
                            common.myInt(Session["FacilityID"]), 0, common.myInt(txtOrderId.Text), common.myInt(Session["RegistrationId"]), 
                            0, encounterid, common.myInt(ddlReviewedStatus.SelectedValue), common.myDate(txtFromDate.SelectedDate),
                            common.myDate(txtToDate.SelectedDate), common.myInt(Session["UserID"]));

            gvDetails.DataSource = ds.Tables[0];
            gvDetails.DataBind();
            if (Session["MUDMeasure"] != null && Convert.ToBoolean(Session["MUDMeasure"]) == true)
            {
                #region Log if a test result is received in encounter
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int labCount = gvDetails.Rows.Count;
                    Hashtable logHash = new Hashtable();
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    logHash.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));
                    logHash.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                    logHash.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
                    logHash.Add("@intDoctorId", Convert.ToInt32(Session["DoctorID"]));
                    logHash.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
                    logHash.Add("@intLabCount", Convert.ToInt32(labCount));
                    objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogLabResultTest", logHash);
                }
                #endregion
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
  
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        bindTestData();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int rowIdx = 0;
            foreach (GridViewRow item in gvDetails.Rows)
            {
                //TextBox txt = (TextBox)item.FindControl("txtDisplayTitle");
                CheckBox chk = (CheckBox)item.FindControl("chkSelect");

                if (chk.Checked)
                {
                    LinkButton lnkSelect = (LinkButton)gvDetails.Rows[rowIdx].FindControl("lnkSelect");
                    int ResultId = common.myInt(lnkSelect.CommandArgument);
                }
                rowIdx++;
            }
        }
        catch
        { }
    }

    protected void btnAttachment_OnClick(Object sender, EventArgs e)
    {
    }

    protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDetails.PageIndex = e.NewPageIndex;
        bindTestData();
    }

    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblMinValue = (Label)e.Row.Cells[0].FindControl("lblMinValue");
            Label lblSymbol = (Label)e.Row.Cells[0].FindControl("lblSymbol");
            Label lblMaxValue = (Label)e.Row.Cells[0].FindControl("lblMaxValue");
            Label lblReferenceRange = (Label)e.Row.Cells[0].FindControl("lblReferenceRange");
            Label lblAbnormalValue = (Label)e.Row.Cells[0].FindControl("lblAbnormalValue");
            Label lblReviewedStatus = (Label)e.Row.Cells[0].FindControl("lblReviewedStatus");
            Label lblFieldType = (Label)e.Row.Cells[0].FindControl("lblFieldType");

            string ReferenceRange = "";

            if (common.myDbl(lblMinValue.Text) == 0
                && common.myDbl(lblMaxValue.Text) == 0)
            {
                ReferenceRange = "(Range: Undefined)";
            }

            if (common.myBool(lblAbnormalValue.Text) == true)
            {
                e.Row.BackColor = System.Drawing.Color.LightYellow;
            }

            if (common.myDbl(lblMinValue.Text) != 0
                && common.myDbl(lblMaxValue.Text) != 0)
            {
                ReferenceRange += "(" + common.myDbl(lblMinValue.Text) + " " + common.myStr(lblSymbol.Text) + " " + common.myDbl(lblMaxValue.Text) + ")";
            }

            if (common.myStr(lblFieldType.Text) == "N")
            {
                lblReferenceRange.Text = ReferenceRange;
            }

            if (common.myInt(lblReviewedStatus.Text) > 0)
            {
                e.Row.BackColor = System.Drawing.Color.LightGreen;
            }
        }
    }

    protected void gvDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        LinkButton lnkSelect = (LinkButton)gvDetails.Rows[e.NewSelectedIndex].FindControl("lnkSelect");
        int ResultId = common.myInt(lnkSelect.CommandArgument);

        if (ResultId > 0)
        {
            RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/LabTestResultReview.aspx?ResultId=" + ResultId;

            RadWindowPopup.Height = 370;
            RadWindowPopup.Width = 500;
            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            RadWindowPopup.OnClientClose = "OnClientCloseReviewed";
            RadWindowPopup.VisibleOnPageLoad = true;
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleStatusbar = false;

        }
    }

    protected void btnCloseReviewed_OnClick(object sender, EventArgs e)
    {
        bindTestData();
    }

    void setDate()
    {
        try
        {
            tblDate.Visible = false;

            switch (common.myStr(ddlTime.SelectedValue))
            {
                case "Today":
                    txtFromDate.SelectedDate = DateTime.Now;
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastWeek":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastTwoWeeks":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-14);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastOneMonth":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastThreeMonths":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastYear":
                    txtFromDate.SelectedDate = DateTime.Now.AddYears(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "DateRange":
                    //txtFromDate.SelectedDate = DateTime.Now;
                    //txtToDate.SelectedDate = DateTime.Now;

                    tblDate.Visible = true;
                    break;
            }
        }
        catch
        {
        }
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindTestData();
    }
}