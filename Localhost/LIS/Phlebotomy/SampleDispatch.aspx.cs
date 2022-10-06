using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using System.Configuration;

public partial class LIS_Phlebotomy_SampleDispatch : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["PType"]).Equals("WD"))   // Sample Collection from Ward
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
            //HyperLink1.NavigateUrl = string.Format(HyperLink1.NavigateUrl, common.myStr(Request.QueryString["PType"]));
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
            HyperLink1.NavigateUrl = "/LIS/Phlebotomy/DispatchLookup.aspx?PType="+ common.myStr(Request.QueryString["PType"]);
            ViewState["StationRequiredForPhlebotomy"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),"StationRequiredForPhlebotomy", sConString);
            if (common.myStr(ViewState["StationRequiredForPhlebotomy"]) == "Y" && common.myStr(Request.QueryString["PT"]) == "COLL" && common.myInt(Session["StationId"]) == 0)
            {
                Response.Redirect("/LIS/Phlebotomy/ChangeStation.aspx", false);
            }

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            objval = new BaseC.clsLISPhlebotomy(sConString);

            ViewState["OLD_DOCTOR_ID"] = "0";
            ViewState["SIGNATURE_PLACE"] = "";

            ddlSearch_SelectedIndexChanged(this, null);

            bindControl();
            BindBlankMachineLabNo();

            bindDetailsData();
            ddlEntrySites.Visible = false;
            lblEntrySites.Visible = false;
            ViewState["IsEnableUserAuthentication"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "UserAuthenticationForLISAndPhlebotomy", sConString);
        }

        Legend1.loadLegend("LAB", "");
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        DataView dvFilter = new DataView();
        BaseC.clsLISSampleReceivingStation objMaster = new BaseC.clsLISSampleReceivingStation(sConString);
        try
        {

            ds = objMaster.getStation(common.myInt(Session["EmployeeId"]), common.myInt(Session["GroupID"]), "LIS");

            dvFilter = new DataView(ds.Tables[0]);
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
                ddlStation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("All"));
                ddlStation.Items[0].Value = "0";
                //if (common.myInt(Session["StationId"]) != 0)
                //{
                //    ddlStation.SelectedIndex = ddlStation.Items.IndexOf(ddlStation.Items.FindItemByValue(common.myInt(Session["StationId"]).ToString()));
                //}
                //else
                //{
                    ddlStation.SelectedIndex = 0;
                //}
            }
            //BindEntrySite();


            fillSubDept();
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
            dvFilter.Dispose();
            objMaster = null;
        }
    }
    private void BindEntrySite()
    {
        DataSet ds = new DataSet();
        DataView dvFilter = new DataView();

        BaseC.clsLISSampleReceivingStation objMaster = new BaseC.clsLISSampleReceivingStation(sConString);
        try
        {
            if (ddlStation.SelectedValue == "0")
            {
                ddlEntrySites.Text = "";
                ddlEntrySites.Items.Clear();
            }
            else
            {
                if (Cache["ENTRYSITES"] == null || ((DataSet)Cache["ENTRYSITES"]).Tables[0].Rows.Count == 0)
                {
                    objval = new BaseC.clsLISPhlebotomy(sConString);
                    ds = objval.getEntrySites(common.myInt(Session["HospitalLocationID"]));
                    Cache["ENTRYSITES"] = ds;
                }
                else
                {
                    objval = new BaseC.clsLISPhlebotomy(sConString);
                    ds = objval.getEntrySites(common.myInt(Session["HospitalLocationID"]));
                    Cache["ENTRYSITES"] = ds;
                }
                DataView dvEntry = ((DataSet)Cache["ENTRYSITES"]).Tables[0].DefaultView;
                dvFilter = new DataView(dvEntry.ToTable());
                if (common.myStr(ViewState["StationRequiredForPhlebotomy"]) == "Y")
                {
                    dvFilter.RowFilter = "StationId=" + common.myInt(Session["StationId"]) + " AND FacilityId=" + common.myInt(Session["FacilityId"]);
                }
                else
                {
                    dvFilter.RowFilter = "StationId=" + common.myInt(ddlStation.SelectedValue) + " AND FacilityId=" + common.myInt(Session["FacilityId"]);
                }
                ddlEntrySites.DataSource = ((DataTable)dvFilter.ToTable());
                ddlEntrySites.DataValueField = "EntrySiteId";
                ddlEntrySites.DataTextField = "EntrySiteName";
                ddlEntrySites.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ds.Dispose();
            dvFilter.Dispose();
            objMaster = null;
        }
    }
    private void fillSubDept()
    {
        DataSet ds = new DataSet();
        try
        {
            ddlSubDepartment.Items.Clear();
            BaseC.clsLISMaster objLabRequest = new BaseC.clsLISMaster(sConString);

            int stationId = common.myInt(Session["StationId"]);
            if (common.myInt(ddlStation.SelectedValue) > 0)
            {
                stationId = common.myInt(ddlStation.SelectedValue);
            }

            ds = objLabRequest.GetSubDepartment(stationId, 0);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].DefaultView.Sort = "SubName";
                ddlSubDepartment.DataSource = ds.Tables[0].DefaultView;
                ddlSubDepartment.DataTextField = "SubName";
                ddlSubDepartment.DataValueField = "SubDeptId";
                ddlSubDepartment.DataBind();
                ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("", "0"));
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

    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";
        if (common.myStr(ViewState["StationRequiredForPhlebotomy"]) == "Y")
        {
            if (ddlStation.SelectedIndex < 0)
            {
                strmsg += "Please Select Station !<br />";
                isSave = false;
            }
        }
        if (common.myInt(ddlEntrySites.SelectedValue) < 0)
        {
            strmsg += "Please Select Entry Site !";
            isSave = false;
        }

        if (gvDetails.SelectedItems.Count < 1)
        {
            strmsg += "Please Select Sample !";
            isSave = false;
        }

        lblMessage.Text = strmsg;
        return isSave;
    }
    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordChecker.aspx?UseFor=LIS";
        RadWindowForNew.Height = 120;
        RadWindowForNew.Width = 340;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {

        lblMessage.Text = string.Empty;
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

        if (common.myInt(hdnIsValidPassword.Value) == 0)
        {
            lblMessage.Text = "Invalid Password !";
            return;
        }
        else
        {
            saveSampleDispatchData();
        }
    }

    private void saveSampleDispatchData()
    {
        lblMessage.Text = string.Empty;
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        try
        {
            StringBuilder strXml = new StringBuilder();
            ArrayList coll = new ArrayList();

            Label LBL;

            for (int idx = 0; idx < gvDetails.SelectedItems.Count; idx++)
            {
                HiddenField hdnStationId = (HiddenField)gvDetails.SelectedItems[idx].FindControl("hdnStationId");
                LBL = (Label)gvDetails.SelectedItems[idx].FindControl("lblSource");
                coll.Add(hdnStationId.Value);

                coll.Add(LBL.Text);

                LBL = (Label)gvDetails.SelectedItems[idx].FindControl("lblDiagSampleId");
                coll.Add(LBL.Text);

                strXml.Append(common.setXmlTable(ref coll));
            }

            objval = new BaseC.clsLISPhlebotomy(sConString);
            int StationId = common.myInt(ddlStation.SelectedValue) == 0 ? 0 : common.myInt(ddlStation.SelectedValue);
            int EntrySiteId = common.myInt(ddlEntrySites.SelectedValue) == 0 ? 0 : common.myInt(ddlEntrySites.SelectedValue);

            string strMsg = objval.saveSampleDispatchData(common.myInt(Session["HospitalLocationID"]),
                                StationId, EntrySiteId, strXml.ToString(), common.myInt(Session["UserID"]));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindDetailsData();
            }
            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
        }
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
            if (common.myStr(ViewState["IsEnableUserAuthentication"]) == "Y") { IsValidPassword(); } else { saveSampleDispatchData(); }

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
    }

    private void bindDetailsData()
    {
        DataSet ds = new DataSet();
        try
        {
            int labno = 0;
            //string Mlabno = "";
            //if (ddlSearch.SelectedValue == "LN")//LabNo
            //{
            //    labno = common.myInt(txtSearchCretria.Text);
            //}
            //if (ddlSearch.SelectedValue == "MLN")//LabNo
            //{
            //    Mlabno = common.myStr(txtSearchCretria.Text);
            //}
            string EncounterNo = string.Empty;
            string RegistrationNo = string.Empty;
            string PatientName = string.Empty;
            string BedNo = string.Empty;
            string wardNo = string.Empty;
            string Mlabno = string.Empty;

            switch (common.myStr(ddlSearch.SelectedValue))
            {
                case "LN": //LabNo
                    labno = common.myInt(txtSearchNumeric.Text);
                    break;
                case "MLN": //Manual Lab No
                    Mlabno = common.myStr(txtSearchCretria.Text);
                    break;
                case "RN": //RegNo
                   // RegistrationNo = common.myInt(txtSearchNumeric.Text).ToString();
                    RegistrationNo = common.myStr(txtSearchNumeric.Text);
                    break;
                case "PN": //PatientName
                    PatientName = common.myStr(txtSearchCretria.Text);
                    break;
                case "BN": //bedNo
                    BedNo = common.myStr(txtSearchCretria.Text);
                    break;
                case "WN": //WardNo
                    wardNo = common.myStr(txtSearchCretria.Text);
                    break;
                case "IP": //IPNo
                    EncounterNo = common.myStr(txtSearchCretria.Text);
                    break;
            }
            if (common.myStr(Request.QueryString["PType"]).Equals("WD")) { RegistrationNo = common.myStr(Request.QueryString["RegNo"]); }
            objval = new BaseC.clsLISPhlebotomy(sConString);
            ds = objval.getInvSamplesDispatchData(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                            common.myInt(ddlStation.SelectedValue), common.myInt(labno), Mlabno, RegistrationNo, EncounterNo,
                            PatientName, BedNo, wardNo, common.myInt(ddlSubDepartment.SelectedValue));

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }
            gvDetails.DataSource = ds;
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
            ds.Dispose();
        }
    }

    protected void gvDetails_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }
    }

    protected void gvDetails_ItemCommand(object source, GridCommandEventArgs e)
    {
    }

    protected void gvDetails_OnPreRender(object sender, EventArgs e)
    {
        //bindDetailsData();
        SetGridColor();
    }

    protected void gvDetails_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        bindDetailsData();
    }

    protected void ddlStation_SelectedIndexChanged(Object sender, EventArgs e)
    {
        fillSubDept();
        BindEntrySite();
        bindDetailsData();
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

    private void BindBlankMachineLabNo()
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

    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        if (common.myLen(txtSearchNumeric.Text) > 0 && Convert.ToInt64(txtSearchNumeric.Text) > 9223372036854775807)
        {
            Alert.ShowAjaxMsg("Value should not be more than 9223372036854775807.", this.Page);
            txtSearchNumeric.Text = txtSearchNumeric.Text.Substring(0, 12);
            //if (common.myStr(ddlSearch.SelectedValue).Equals("LN"))
            //{
            //    lblMessage.Text = "Invalid Lab No";
            //}
            //else if (common.myStr(ddlSearch.SelectedValue).Equals("RN"))
            //{
            //    lblMessage.Text = "Invalid UHID No";
            //}
            //else
            //{
            //    lblMessage.Text = "Invalid Search Criteria";
            //}
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            return;
        }
        lblMessage.Text = string.Empty;
        bindDetailsData();
    }


    protected void ddlSearch_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        txtSearchNumeric.Text = string.Empty;
        txtSearchCretria.Text = string.Empty;

        if (string.Compare(common.myStr(ddlSearch.SelectedValue), "LN") == 0
            || string.Compare(common.myStr(ddlSearch.SelectedValue), "RN") == 0)
        {
            txtSearchCretria.Visible = false;
            txtSearchNumeric.Visible = true;
        }
        else
        {
            txtSearchCretria.Visible = true;
            txtSearchNumeric.Visible = false;
        }
    }
}