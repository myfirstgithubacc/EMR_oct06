using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using System.Configuration;
using System.Web;
public partial class LIS_Phlebotomy_DispatchLookup : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;
    string Flag = "";
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["PType"]).Equals("WD"))   // Sample Collection from Ward
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
        if (!IsPostBack)
        {
            HyperLink1.NavigateUrl = "/LIS/Phlebotomy/SampleDispatch.aspx?PType=" + common.myStr(Request.QueryString["PType"]);
            ViewState["StationRequiredForPhlebotomy"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                           "StationRequiredForPhlebotomy", sConString);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtFromDate.SelectedDate = DateTime.Now;
            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.SelectedDate = DateTime.Now;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            btnSaveData.Attributes.Add("onclick", "return ConfirmCancelDispatch();");

            objval = new BaseC.clsLISPhlebotomy(sConString);
            Flag = common.myStr(Request.QueryString["MD"]);
            if (Flag == "")
            {
                Flag = common.myStr(Request.QueryString["MD"]);
            }
            bindControl();
            BindBlankGrid();
        }
        if (Flag.ToString() == "RIS")
            Legend1.loadLegend("LAB", "'DA','RE','RP','RF'");
        else
            Legend1.loadLegend("LAB", "");
    }

    private void bindControl()
    {
        DataView dvFilter = new DataView();
        DataSet dsStation = new DataSet();
        BaseC.clsLISSampleReceivingStation objMaster = new BaseC.clsLISSampleReceivingStation(sConString);
        try
        {
            dsStation = objMaster.getStation(common.myInt(Session["EmployeeId"]),
                common.myInt(Session["GroupID"]), "LIS");
            dvFilter = new DataView(dsStation.Tables[0]);
            if (common.myStr(ViewState["StationRequiredForPhlebotomy"]) == "N")
            {
                dvFilter.RowFilter = "LabType='G'";
            }
            if (dvFilter.ToTable().Rows.Count > 0)
            {
                ddlStation.DataSource = dvFilter.ToTable();
                ddlStation.DataValueField = "StationId";
                ddlStation.DataTextField = "StationName";
                ddlStation.DataBind();
                if (common.myStr(ViewState["StationRequiredForPhlebotomy"]) == "N")
                {
                    ddlStation.SelectedIndex = 0;
                }
            }
            BindEntrySite();
            BaseC.clsEMRBilling objval1 = new BaseC.clsEMRBilling(sConString);
            DataSet DSS = objval1.getEntrySite(common.myInt(Session["UserID"]), common.myInt(Session["FacilityId"]));
            ddlEntrySitesActual.DataSource = DSS;
            ddlEntrySitesActual.DataValueField = "ESId";
            ddlEntrySitesActual.DataTextField = "ESName";
            ddlEntrySitesActual.DataBind();
            //ddlEntrySitesDiag.Items.Insert(0, new RadComboBoxItem("", "0"));
            //ddlEntrySitesDiag.SelectedIndex = 0;

            ddlEntrySitesActual.SelectedIndex = 0;
            //ddlEntrySitesActual.SelectedIndex = ddlEntrySitesActual.Items.IndexOf(ddlEntrySitesActual.Items.FindItemByValue(common.myStr(Session["EntrySite"])));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            dsStation.Dispose();
            objMaster = null;
        }
    }
    protected void ddlStation_SelectedIndexChanged(Object sender, EventArgs e)
    {
        BindEntrySite();
    }
    private void BindEntrySite()
    {
        DataSet ds = new DataSet();
        DataView dvEntry = new DataView();
        try
        {
            if (Cache["ENTRYSITES"] == null || ((DataSet)Cache["ENTRYSITES"]).Tables[0].Rows.Count == 0)
            {
                objval = new BaseC.clsLISPhlebotomy(sConString);
                ds = objval.getEntrySites(common.myInt(Session["HospitalLocationID"]));
                Cache["ENTRYSITES"] = ds;
            }

            dvEntry = new DataView(((DataSet)Cache["ENTRYSITES"]).Tables[0]);
            dvEntry = new DataView(dvEntry.ToTable());
            if (common.myStr(ViewState["StationRequiredForPhlebotomy"]) == "Y")
            {
                dvEntry.RowFilter = "StationId=" + common.myInt(Session["StationId"]) + " AND FacilityId=" + common.myInt(Session["FacilityId"]);
            }
            else
            {
                dvEntry.RowFilter = "StationId=" + common.myInt(ddlStation.SelectedValue) + " AND FacilityId=" + common.myInt(Session["FacilityId"]);
            }
            ddlEntrySites.DataSource = ((DataTable)dvEntry.ToTable());
            ddlEntrySites.DataValueField = "EntrySiteId";
            ddlEntrySites.DataTextField = "EntrySiteName";
            ddlEntrySites.DataBind();
            if (common.myStr(ViewState["StationRequiredForPhlebotomy"]) == "N")
            {
                ddlEntrySites.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            ds.Dispose();
            dvEntry.Dispose();
        }
    }
    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";
        if (gvDetails.SelectedItems.Count < 1)
        {
            strmsg += "Please Select Sample !<br />";
            isSave = false;
        }

        lblMessage.Text = strmsg;
        return isSave;
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }

            StringBuilder strXml = new StringBuilder();
            ArrayList coll = new ArrayList();

            Label LBL;

            for (int idx = 0; idx < gvDetails.SelectedItems.Count; idx++)
            {
                LBL = (Label)gvDetails.SelectedItems[idx].FindControl("lblSource");
                coll.Add(LBL.Text);

                LBL = (Label)gvDetails.SelectedItems[idx].FindControl("lblDiagSampleId");
                coll.Add(LBL.Text);

                strXml.Append(common.setXmlTable(ref coll));
            }

            objval = new BaseC.clsLISPhlebotomy(sConString);
            string strMsg = objval.cancelInvSamplesDispatchData(common.myInt(Session["HospitalLocationID"]),
                               strXml.ToString());

            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                clearControl();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindDispatchedDetailsData();
            }
            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void clearControl()
    {
        lblMessage.Text = "";
        //ddlStation.SelectedIndex = 0;       
    }

    protected void btnRefresh_OnClick(Object sender, EventArgs e)
    {
        if (ddlStation.SelectedValue == "")
        {
            Alert.ShowAjaxMsg("Please Select Station !", this.Page);
            return;
        }
        if (ddlEntrySites.SelectedValue == "")
        {
            Alert.ShowAjaxMsg("Please Select Entry Site !", this.Page);
            return;
        }

        bindDispatchedDetailsData();
    }

    private void bindDispatchedDetailsData()
    {
        try
        {
            clearControl();
            int labno = 0;
            string Mlabno = "";
            if (ddlSearch.SelectedValue == "LN")//LabNo
            {
                labno = common.myInt(txtSearchCretria.Text);
            }
            if (ddlSearch.SelectedValue == "MLN")//LabNo
            {
                Mlabno = common.myStr(txtSearchCretria.Text);
            }

            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objval.getInvSamplesDispatchedDetail(common.myInt(Session["HospitalLocationID"]),
                    common.myInt(ddlStation.SelectedValue), common.myInt(ddlEntrySites.SelectedValue),
                    common.myInt(Session["FacilityId"]), common.myDate(txtFromDate.SelectedDate),
                    common.myDate(txtToDate.SelectedDate), labno, Mlabno, "", "", 0, common.myInt(ddlEntrySitesActual.SelectedValue));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = ds.Tables[0].DefaultView;
                    dv.Sort = "SCD Desc";
                    gvDetails.DataSource = dv;
                    gvDetails.DataBind();

                    SetGridColor();
                }
                else
                {
                    BindBlankGrid();
                }
            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvDetails_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        bindDispatchedDetailsData();
    }

    protected void gvDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }
        if (e.Item.ItemType == GridItemType.Header)
        {
            if (Flag.ToString() == "RIS")
                ((Label)e.Item.FindControl("lblLabHeader")).Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "RadiologyNo"));
            else
                ((Label)e.Item.FindControl("lblLabHeader")).Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "LabNo"));
        }

    }

    protected void gvDetails_OnPreRender(object sender, EventArgs e)
    {
        bindDispatchedDetailsData();
        SetGridColor();
    }

    private void SetGridColor()
    {
        try
        {
            foreach (GridDataItem dataItem in gvDetails.MasterTableView.Items)
            {
                if (dataItem.ItemType == GridItemType.Item
                    || dataItem.ItemType == GridItemType.AlternatingItem
                    || dataItem.ItemType == GridItemType.SelectedItem)
                {
                    Label lblStatusColor = (Label)dataItem.FindControl("lblStatusColor");
                    dataItem.BackColor = System.Drawing.Color.FromName(common.myStr(lblStatusColor.Text));
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvDetails_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            (e.Column as GridGroupSplitterColumn).ExpandImageUrl = "../../Images/Plusbox.gif";
            (e.Column as GridGroupSplitterColumn).CollapseImageUrl = "../../Images/Minubox.gif";

            //e.Column.HeaderStyle.Width = Unit.Pixel(3);
            //e.Column.ItemStyle.Width = Unit.Pixel(3);
            e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            e.Column.ItemStyle.VerticalAlign = VerticalAlign.Top;

            //e.Column.ItemStyle.Font.Size = FontUnit.Point(1);
            //e.Column.Resizable = false; 
        }
    }

    void BindBlankGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Source");
            dt.Columns.Add("LabNo");
            dt.Columns.Add("RegistrationNo");
            dt.Columns.Add("PatientName");
            dt.Columns.Add("ServiceName");
            dt.Columns.Add("DiagSampleId");
            dt.Columns.Add("ServiceId");
            dt.Columns.Add("SubDeptId");
            dt.Columns.Add("SubName");
            dt.Columns.Add("StatusColor");
            dt.Columns.Add("DispatchedDate");
            gvDetails.DataSource = dt;
            gvDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

}