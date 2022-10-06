using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class MPages_EmployeeWiseSpecialRightTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    //private Hashtable hshInput;
    DAL.DAL dl = new DAL.DAL();
    BaseC.EMRBilling objBC;
    StringBuilder strXML;
    ArrayList coll;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindEmplyoee();
            BindEmptygvSpecialRightSelectionListGrid();
            BindEmptygrvTaggedSpecialRightsList();
        }
    }
    protected void bindEmplyoee()
    {
        try
        {
            objBC = new BaseC.EMRBilling(sConString);
            DataTable dt = objBC.getEmployeeList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            ddlEmployee.DataSource = dt;
            ddlEmployee.DataTextField = "EmployeeNameWithNo";
            ddlEmployee.DataValueField = "EmployeeId";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlEmployee.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
    }
    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(ddlEmployee.SelectedValue) != "")
                bindEmployeeWiseSpecialRightTagging(txtSearch.Text.Trim());
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }

    protected void bindEmployeeWiseSpecialRightTagging(string searchText)
    {
        try
        {
            //getEmployeeWiseSpecialRightTagging
            objBC = new BaseC.EMRBilling(sConString);
            DataSet ds = objBC.getEmployeeWiseSpecialRightTagging(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(ddlEmployee.SelectedValue), "N", txtSearch.Text.Trim(), txtSearchFlagRight.Text.Trim());
            gvSpecialRightSelectionList.DataSource = ds.Tables[0];
            gvSpecialRightSelectionList.DataBind();
            ViewState["gvSpecialRightSelectionList"] = ds.Tables[0];
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["flagRight"] = 1;
            SaveEmployeeWiseSpecialRightTagging();

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
    protected void SaveEmployeeWiseSpecialRightTagging()
    {
        try
        {
            lblMessage.Text = "";
            if (common.myInt(hdnisSaveChk.Value) == 0)
            {
                Alert.ShowAjaxMsg("No Data to Save!!!", Page.Page);
                return;
            }
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            objBC = new BaseC.EMRBilling(sConString);
            strXML = new StringBuilder();
            coll = new ArrayList();
            if (common.myInt(ddlEmployee.SelectedValue) == 0)
            {
                lblMessage.Text = "Please Select Employee !";
                return;
            }
            foreach (GridViewRow gvRow in grvTaggedSpecialRightsList.Rows)
            {
                //HiddenField hdnID = (HiddenField)gvRow.FindControl("hdnID");
                HiddenField hdSpecialRightId = (HiddenField)gvRow.FindControl("hdSpecialRightId");

                //if (common.myInt(hdnID.Value) > 0)
                if (common.myInt(hdSpecialRightId.Value) > 0)
                {
                    coll.Add(common.myInt(hdSpecialRightId.Value));
                    strXML.Append(common.setXmlTable(ref coll));
                }
            }
            if (strXML.ToString() == "")
            {
                lblMessage.Text = "Please Select " + ddlEmployee.SelectedItem.Text + " !";
                return;
            }
            string strmsg = objBC.SaveEmployeeWiseSpecialRightTagging(common.myInt(Session["HospitalLocationId"]), strXML.ToString(), common.myInt(ddlEmployee.SelectedValue), common.myInt(Session["FacilityId"]), common.myBool(1), common.myInt(Session["UserId"]));
            // string strmsg = objBC.SaveFacilityWiseCommonMaster(common.myInt(Session["HospitalLocationId"]), strXML.ToString(), common.myInt(ddlFacility.SelectedValue), common.myBool(1), common.myInt(Session["UserId"]), common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).Trim());
            if ((strmsg.Contains(" Update") || strmsg.Contains(" Save")) && !strmsg.Contains("usp"))
            {
                bindEmployeeWiseSpecialRightTagging(txtSearch.Text.Trim());
                bindgrvTaggedSpecialRightsList(txtSearchFlagRight.Text.Trim());
                lblMessage.Text = strmsg;
                // Alert.ShowAjaxMsg(strmsg, Page.Page);
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
    protected void btnSend4WO_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (ddlEmployee.SelectedIndex > 0)
            {
                try
                {
                    DataTable dt;
                    if (gvSpecialRightSelectionList.Rows.Count > 0)
                    {
                        grvTaggedSpecialRightsList.DataSource = null;
                        grvTaggedSpecialRightsList.DataBind();

                        if (ViewState["SpecialRightsList"] == null)
                            dt = BindBlankTableInv();
                        else
                            dt = (DataTable)ViewState["SpecialRightsList"];

                        DataView dv = dt.DefaultView;
                        dv.RowFilter = "";
                        bool chkFlag = false;
                        foreach (GridViewRow gv in gvSpecialRightSelectionList.Rows)
                        {
                            CheckBox chk = (CheckBox)gv.FindControl("chkSelect");
                            if (chk.Checked == true)
                            {
                                dv.RowFilter = "ID = " + common.myStr(((HiddenField)gv.FindControl("hdnColumnId")).Value) + "";
                                // dv.RowFilter = String.Format("ID='{0}'", common.myStr(((HiddenField)gv.FindControl("hdnColumnId")).Value));
                                if (dv.ToTable().Rows.Count == 0)
                                {
                                    chkFlag = true;
                                    DataRow dr = dt.NewRow();
                                    dr["ID"] = 0;
                                    dr["SpecialRightId"] = common.myInt(((HiddenField)gv.FindControl("hdnColumnId")).Value);
                                    dr["Flag"] = common.myStr(((Label)gv.FindControl("lblColumnName")).Text);
                                    dr["Active"] = true;
                                    //dr["EmployeeID"] = common.myInt(ddlEmployee.SelectedValue);
                                    //dr["EmployeeName"] = common.myStr(ddlEmployee.SelectedItem.Text);
                                    dt.Rows.Add(dr);
                                    dt.AcceptChanges();
                                }
                                ViewState["SpecialRightsList"] = dt;
                            }
                        }

                        Bind_SpecialRightSelectionListInGrid();

                        //if (!chkFlag)
                        //{
                        //    Alert.ShowAjaxMsg("Please! Select any flag..", Page.Page);
                        //    return;
                        //}
                        //bindEmployeeWiseSpecialRightTagging();
                        hdnisSaveChk.Value = "1";
                        dv.RowFilter = "";
                        ViewState["TaggedSpecialRightsList"] = dt;
                        grvTaggedSpecialRightsList.DataSource = dt;
                        grvTaggedSpecialRightsList.DataBind();
                        lblSelFacilityCount.Text = "Total Records : " + dt.Rows.Count.ToString();
                    }
                    if (gvSpecialRightSelectionList.Rows.Count == 0)
                        BindEmptygvSpecialRightSelectionListGrid();
                    chkUnSelect.Checked = false;
                }
                catch (Exception Ex)
                {
                    objException.HandleException(Ex);
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Please!! Select Employee..", Page.Page);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    private void Bind_SpecialRightSelectionListInGrid()
    {
        DataTable dtunslected = new DataTable();
        dtunslected = (DataTable)ViewState["gvSpecialRightSelectionList"];
        foreach (GridViewRow gvSelList in gvSpecialRightSelectionList.Rows)
        {
            CheckBox chk = (CheckBox)gvSelList.FindControl("chkSelect");
            HiddenField hdnID = (HiddenField)gvSelList.FindControl("hdnColumnId");
            if (chk.Checked == true)
            {
                foreach (DataRow row in dtunslected.Rows)
                {
                    if (common.myStr(row["ID"]) == common.myStr(hdnID.Value))
                    {
                        dtunslected.Rows.Remove(row);
                        break;
                    }
                }
                dtunslected.AcceptChanges();
            }
        }
        gvSpecialRightSelectionList.DataSource = dtunslected;
        gvSpecialRightSelectionList.DataBind();
        ViewState["gvSpecialRightSelectionList"] = dtunslected;
    }

    private void Bind_SpecialRightSelectionListInGridAfterDelete()
    {
        DataTable dtunslected = new DataTable();
        objBC = new BaseC.EMRBilling(sConString);

        DataSet ds = objBC.getEmployeeWiseSpecialRightTagging(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(ddlEmployee.SelectedValue), "N", txtSearch.Text.Trim(), txtSearchFlagRight.Text.Trim());

        dtunslected = ds.Tables[0];
        foreach (GridViewRow gvSelList in grvTaggedSpecialRightsList.Rows)
        {
            HiddenField hdSpecialRightId = (HiddenField)gvSelList.FindControl("hdSpecialRightId");
            string iiiiii = hdSpecialRightId.Value;
            foreach (DataRow row in dtunslected.Rows)
            {
                if (common.myStr(row["ID"]) == common.myStr(hdSpecialRightId.Value))
                {
                    dtunslected.Rows.Remove(row);
                    break;
                }
            }
        }
        dtunslected.AcceptChanges();
        gvSpecialRightSelectionList.DataSource = dtunslected;
        gvSpecialRightSelectionList.DataBind();
        ViewState["gvSpecialRightSelectionList"] = dtunslected;
    }

    protected void lnkDelete_OnClick(object sender, EventArgs e)
    {
        ViewState["hdTempSpecialRightId"] = 0;

        LinkButton lnkBtn = (LinkButton)sender;
        int hdnSelColumnId = common.myInt(((HiddenField)lnkBtn.FindControl("hdnID")).Value);
        if (hdnSelColumnId == 0)
        {
            int iSpecialRightId = common.myInt(((HiddenField)lnkBtn.FindControl("hdSpecialRightId")).Value);
            ViewState["hdTempSpecialRightId"] = iSpecialRightId;
        }

        ViewState["hdnSelColumnId"] = hdnSelColumnId;
        dvConfirmCancelOptions.Visible = true;
    }
    protected DataTable BindBlankTableInv()
    {
        DataTable dtInv = new DataTable();
        dtInv.Columns.Add("ID", typeof(int));
        // dtInv.Columns.Add("ID", typeof(int));
        dtInv.Columns.Add("Flag", typeof(string));
        dtInv.Columns.Add("EmployeeID", typeof(int));
        dtInv.Columns.Add("EmployeeName", typeof(string));
        dtInv.Columns.Add("SpecialRightId", typeof(int));
        dtInv.Columns.Add("Active", typeof(int));
        return dtInv;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            BindEmptygrvTaggedSpecialRightsList();
            //grvTaggedSpecialRightsList.DataSource = null;
            //grvTaggedSpecialRightsList.DataBind();
            ViewState["SpecialRightsList"] = null;
            lblSelFacilityCount.Text = "Total Records : 0";
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }

    protected void chkUnSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gv in gvSpecialRightSelectionList.Rows)
        {
            CheckBox chk = (CheckBox)gv.FindControl("chkSelect");
            chk.Checked = false;
            if (chkUnSelect.Checked == true)
                chk.Checked = true;

        }
    }
    protected void ddlTaggedFor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            bindEmployeeWiseSpecialRightTagging(txtSearch.Text.Trim());
            bindgrvTaggedSpecialRightsList(txtSearchFlagRight.Text.Trim());

        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
    }
    // Send Grid
    protected void BindEmptygvSpecialRightSelectionListGrid()
    {
        DataTable dtSpecialLeft = new DataTable();
        DataRow dr = null;
        dtSpecialLeft.Columns.Add(new DataColumn("ID", typeof(int)));
        dtSpecialLeft.Columns.Add(new DataColumn("Flag", typeof(string)));
        dtSpecialLeft.Columns.Add(new DataColumn("Description", typeof(string)));
        dr = dtSpecialLeft.NewRow();
        dtSpecialLeft.Rows.Add(dr);
        gvSpecialRightSelectionList.DataSource = dtSpecialLeft;
        gvSpecialRightSelectionList.DataBind();

    }
    //Right side
    protected void BindEmptygrvTaggedSpecialRightsList()
    {
        DataTable dtSpecialRight = new DataTable();
        DataRow drRight = null;
        dtSpecialRight.Columns.Add(new DataColumn("ID", typeof(int)));
        dtSpecialRight.Columns.Add(new DataColumn("SpecialRightId", typeof(int)));
        dtSpecialRight.Columns.Add(new DataColumn("Flag", typeof(string)));
        drRight = dtSpecialRight.NewRow();
        dtSpecialRight.Rows.Add(drRight);
        grvTaggedSpecialRightsList.DataSource = dtSpecialRight;
        grvTaggedSpecialRightsList.DataBind();
    }

    protected void bindgrvTaggedSpecialRightsList(string searchText)
    {
        string strsearchText = string.Empty;
        try
        {
            if (ddlEmployee.SelectedIndex > 0)
            {
                try
                {

                    if (common.myStr(ViewState["flagRight"]) != "1")
                    {
                        strsearchText = txtSearch.Text.Trim();
                    }
                    //getEmployeeWiseSpecialRightTagging
                    objBC = new BaseC.EMRBilling(sConString);

                    DataSet ds = objBC.getEmployeeWiseSpecialRightTagging(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(ddlEmployee.SelectedValue), "Y", strsearchText, txtSearchFlagRight.Text.Trim());
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //trRightSide.Visible = false;
                        ViewState["SpecialRightsList"] = ds.Tables[0];
                        grvTaggedSpecialRightsList.DataSource = ds.Tables[0];
                        grvTaggedSpecialRightsList.DataBind();
                        ViewState["TaggedSpecialRightsList"] = ds.Tables[0];
                        ViewState["AllTaggedSpecialRightsList"] = ds.Tables[0];
                    }
                    else
                    {
                        BindEmptygrvTaggedSpecialRightsList();
                        //grvTaggedSpecialRightsList.DataSource = null;
                        //grvTaggedSpecialRightsList.DataBind();
                    }
                    lblSelFacilityCount.Text = "Total Records : " + ds.Tables[0].Rows.Count.ToString();
                    ViewState["flagRight"] = null;
                }
                catch (Exception Ex)
                {
                    objException.HandleException(Ex);
                }
            }
            else { Alert.ShowAjaxMsg("Please!! Select Employee..", Page.Page); }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ("Error: " + Ex.Message);
            objException.HandleException(Ex);
        }
    }
    protected void btnDeleteOk_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            objBC = new BaseC.EMRBilling(sConString);

            DataTable dtSpecialRight = new DataTable();
            dtSpecialRight = (DataTable)ViewState["SpecialRightsList"];

            if (common.myInt(ViewState["hdnSelColumnId"]) > 0)
            {
                string output = objBC.UpdateEmployeeWiseSpecialRightTagging(common.myInt(ViewState["hdnSelColumnId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0, 0, common.myBool(0), common.myInt(Session["UserId"]));

                for (int i = dtSpecialRight.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtSpecialRight.Rows[i];
                    if (Convert.ToInt32(dr["ID"]) == common.myInt(ViewState["hdnSelColumnId"]))
                        dr.Delete();
                }
                dtSpecialRight.AcceptChanges();
                if (dtSpecialRight.Rows.Count > 0)
                {
                    grvTaggedSpecialRightsList.DataSource = dtSpecialRight;
                    grvTaggedSpecialRightsList.DataBind();
                }
                else
                {
                    BindEmptygrvTaggedSpecialRightsList();
                }
                bindEmployeeWiseSpecialRightTagging("");
                ViewState["SpecialRightsList"] = dtSpecialRight;
                lblSelFacilityCount.Text = "Total Records : " + dtSpecialRight.Rows.Count.ToString();
                dvConfirmCancelOptions.Visible = false;
                lblMessage.Text = output;
            }
            else if (common.myInt(ViewState["hdTempSpecialRightId"]) > 0)
            {
                foreach (DataRow row in dtSpecialRight.Rows)
                {
                    if (common.myStr(row["SpecialRightId"]) == common.myStr(ViewState["hdTempSpecialRightId"]))
                    {
                        dtSpecialRight.Rows.Remove(row);
                        break;
                    }
                }
                dtSpecialRight.AcceptChanges();

                if (dtSpecialRight.Rows.Count > 0)
                {
                    grvTaggedSpecialRightsList.DataSource = dtSpecialRight;
                    grvTaggedSpecialRightsList.DataBind();
                }

                Bind_SpecialRightSelectionListInGridAfterDelete();

                ViewState["SpecialRightsList"] = dtSpecialRight;
                lblSelFacilityCount.Text = "Total Records : " + dtSpecialRight.Rows.Count.ToString();
                dvConfirmCancelOptions.Visible = false;
                lblMessage.Text = "Record successfully deleted from grid not Database. ";
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }

    protected void btnSerchLeft_Click(object sender, EventArgs e)
    {
        if (txtSearch.Text != null || txtSearch.Text != "")
        {
            bindEmployeeWiseSpecialRightTagging(txtSearch.Text.Trim());
        }
    }
    protected void btnSearchRight_Click(object sender, EventArgs e)
    {
        ViewState["flagRight"] = 1;
        if (txtSearchFlagRight.Text != null || txtSearchFlagRight.Text != "")
        {
            bindgrvTaggedSpecialRightsList(txtSearchFlagRight.Text.Trim());
        }
    }
    protected void btnDeleteCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmCancelOptions.Visible = false;
    }
    // Sorting 
    protected void SortRecords(object sender, GridViewSortEventArgs e)
    {
        if (ViewState["gvSpecialRightSelectionList"] != null)
        {
            string sortExpression = e.SortExpression;
            string direction = string.Empty;
            if (SortDirection == SortDirection.Ascending)
            {
                SortDirection = SortDirection.Descending;
                direction = " DESC";
            }
            else
            {
                SortDirection = SortDirection.Ascending;
                direction = " ASC";
            }
            DataTable dt = (DataTable)ViewState["gvSpecialRightSelectionList"];
            dt.DefaultView.Sort = sortExpression + direction;
            gvSpecialRightSelectionList.DataSource = dt;
            gvSpecialRightSelectionList.DataBind();
        }
    }
    public SortDirection SortDirection
    {
        get
        {
            if (ViewState["SortDirection"] == null)
            {
                ViewState["SortDirection"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["SortDirection"];
        }
        set
        {
            ViewState["SortDirection"] = value;
        }
    }
    // End  Sorting  //grvTaggedSpecialRightsList  Sorting 
    protected void gvTSRL_SortRecords(object sender, GridViewSortEventArgs e)
    {
        if (ViewState["TaggedSpecialRightsList"] != null)
        {
            string sortExpression = e.SortExpression;
            string direction = string.Empty;
            if (SortDirection == SortDirection.Ascending)
            {
                SortDirection = SortDirection.Descending;
                direction = " DESC";
            }
            else
            {
                SortDirection = SortDirection.Ascending;
                direction = " ASC";
            }
            DataTable dt = (DataTable)ViewState["TaggedSpecialRightsList"];
            dt.DefaultView.Sort = sortExpression + direction;
            grvTaggedSpecialRightsList.DataSource = dt;
            grvTaggedSpecialRightsList.DataBind();
        }
    }
    //public SortDirection SortDirection
    //{
    //    get
    //    {
    //        if (ViewState["SortDirection"] == null)
    //        {
    //            ViewState["SortDirection"] = SortDirection.Ascending;
    //        }
    //        return (SortDirection)ViewState["SortDirection"];
    //    }
    //    set
    //    {
    //        ViewState["SortDirection"] = value;
    //    }
    //}
    // End  Sorting 
    protected void gvSpecialRightSelectionList_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["gvSpecialRightSelectionList"] != null)
        {
            gvSpecialRightSelectionList.DataSource = ViewState["gvSpecialRightSelectionList"] as DataTable;
            gvSpecialRightSelectionList.PageIndex = e.NewPageIndex;
            gvSpecialRightSelectionList.DataBind();
        }
    }
    protected void grvTaggedSpecialRightsList_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["TaggedSpecialRightsList"] != null)
        {
            grvTaggedSpecialRightsList.DataSource = ViewState["TaggedSpecialRightsList"] as DataTable;
            grvTaggedSpecialRightsList.PageIndex = e.NewPageIndex;
            grvTaggedSpecialRightsList.DataBind();
        }
    }
}
