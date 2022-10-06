using System;
using System.Configuration;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMR_Masters_SearchTemplates : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindControl();
            bindDetailsData(false);

        }
    }

    protected void ddlSpecialisation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindDetailsData(false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlTemplateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindDetailsData(false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlApplicableFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindDetailsData(false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void rdoTemplateType_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            bindDetailsData(false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindDetailsData(false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindDetailsData(bool BindStatus)
    {
        try
        {
            DataSet ds = getTemplateData();

            gvResultFinal.DataSource = ds.Tables[0];
            gvResultFinal.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindDetailsData(false);
            txtSearchValue.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindControl()
    {
        try
        {
            DataSet ds = getSpecialisationMaster();
            ddlSpecialisation.DataSource = ds;
            ddlSpecialisation.DataTextField = "Name";
            ddlSpecialisation.DataValueField = "Id";
            ddlSpecialisation.DataBind();

            ddlSpecialisation.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlSpecialisation.SelectedIndex = 0;



            StringBuilder strSQL = new StringBuilder();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL.Append("select ID, TypeName from EMRTemplateTypes order by TypeName");
            ds = dl.FillDataSet(CommandType.Text, strSQL.ToString());

            ddlTemplateType.DataSource = ds;
            ddlTemplateType.DataTextField = "TypeName";
            ddlTemplateType.DataValueField = "ID";
            ddlTemplateType.DataBind();

            ddlTemplateType.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlTemplateType.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public DataSet getSpecialisationMaster()
    {
        DL_Funs fun = new DL_Funs();
        StringBuilder sb = new StringBuilder();
        sb.Append("Select Id, Name from SpecialisationMaster Where Active = 1 Order By Name");

        DataSet dsSpecialisation = new DataSet();
        if (Cache["Specialisation"] != null)
        {
            dsSpecialisation = Cache["Specialisation"] as DataSet;
        }
        else
        {
            dsSpecialisation = fun.ExecuteSql(sb);
            Cache.Insert("Specialisation", dsSpecialisation, null, System.DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        return dsSpecialisation;
    }

    private DataSet getTemplateData()
    {
        DataSet dsTemplate = new DataSet();
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            if (rdoTemplateType.SelectedValue == "2")
            {
                string qry = "SELECT Row_Number() Over (Order By PageName) SerialNo, PageID as TemplateId, " +
                    "PageName as TemplateName , '' as type, '' as Code ,'' as Specialisation, 0 as Active, " +
                    "0 as Status, '' as TemplateTypeID, '' as DisplayTitle, 1 as TemplateSpaceNumber, " +
                    "'V' AS EntryType, 'B' ApplicableFor, SpecialisationId, 0 MenuSequence " +
                    "from SecModulePages " +
                    "WHERE ModuleID = 3 AND Hierarchy = 0 AND StaticPage = 0 AND Active = 1";

                dsTemplate = dl.FillDataSet(CommandType.Text, qry);
            }
            else
            {
                StringBuilder strSQL = new StringBuilder();
                Hashtable hshIn = new Hashtable();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                hshIn.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
                if (txtSearchValue.Text.Trim() != "")
                {
                    hshIn.Add("@chvTemplateName", "%" + common.myStr(txtSearchValue.Text).Trim() + "%");
                }

                hshIn.Add("@inyTemplateTypeId", common.myInt(ddlTemplateType.SelectedValue));

                hshIn.Add("@intStatusType", common.myInt(ddlStatus.SelectedValue));
                hshIn.Add("@intSpecialisationId", common.myInt(ddlSpecialisation.SelectedValue));

                if (common.myStr(Session["ModuleName"]) == "OT Module")
                {
                    hshIn.Add("@intModuleId", common.myInt(Session["ModuleId"]));
                }
                hshIn.Add("@chvApplicableFor", common.myStr(ddlApplicableFor.SelectedValue));

                dsTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspGetTemplate", hshIn);

                if (dsTemplate.Tables[0].Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    DataColumn dcTemplateName = new DataColumn("TemplateName");
                    DataColumn dcTemplateId = new DataColumn("TemplateId");
                    DataColumn dcSpecialisation = new DataColumn("Specialisation");
                    DataColumn dcActive = new DataColumn("Active");
                    DataColumn dcSerialNo = new DataColumn("SerialNo");
                    DataColumn dcType = new DataColumn("Type");
                    DataColumn dcCode = new DataColumn("Code");
                    DataColumn dcStatus = new DataColumn("Status");
                    DataColumn dcTemplateTypeID = new DataColumn("TemplateTypeID");
                    DataColumn dcDisplayTitle = new DataColumn("DisplayTitle");
                    DataColumn dcTemplateSpace = new DataColumn("TemplateSpaceNumber");
                    DataColumn dcEntryType = new DataColumn("EntryType");
                    DataColumn dcApplicableFor = new DataColumn("ApplicableFor");
                    DataColumn dcSpecialisationId = new DataColumn("SpecialisationId");
                    DataColumn dcMenuSequence = new DataColumn("MenuSequence");

                    dt.Columns.Add(dcTemplateName);
                    dt.Columns.Add(dcTemplateId);
                    dt.Columns.Add(dcSpecialisation);
                    dt.Columns.Add(dcActive);
                    dt.Columns.Add(dcSerialNo);
                    dt.Columns.Add(dcType);
                    dt.Columns.Add(dcCode);
                    dt.Columns.Add(dcStatus);
                    dt.Columns.Add(dcTemplateTypeID);
                    dt.Columns.Add(dcDisplayTitle);
                    dt.Columns.Add(dcTemplateSpace);
                    dt.Columns.Add(dcEntryType);
                    dt.Columns.Add(dcApplicableFor);
                    dt.Columns.Add(dcSpecialisationId);
                    dt.Columns.Add(dcMenuSequence);

                    DataRow drow = dt.NewRow();

                    drow[0] = "";
                    drow[1] = "";
                    drow[2] = "";
                    drow[3] = "";
                    drow[4] = "";
                    drow[5] = "";
                    drow[6] = "";
                    drow[7] = "";
                    drow[8] = "";
                    drow[9] = "";
                    drow[10] = "";
                    drow[11] = "V";
                    drow[12] = "B";
                    drow[13] = "";
                    drow[14] = "";

                    dt.Rows.Add(drow);

                    dsTemplate.Tables.Add(dt);

                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return dsTemplate;
    }

    protected void gvResultFinal_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            (e.Column as GridGroupSplitterColumn).ExpandImageUrl = "../../Images/Plusbox.gif";
            (e.Column as GridGroupSplitterColumn).CollapseImageUrl = "../../Images/Minubox.gif";
            e.Column.HeaderStyle.Width = Unit.Percentage(3);
            e.Column.ItemStyle.Width = Unit.Percentage(3);
            e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            e.Column.ItemStyle.VerticalAlign = VerticalAlign.Top;
        }
    }

    //protected void gvResultFinal_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    //{
    //    gvResultFinal.CurrentPageIndex = e.NewPageIndex;
    //    bindDetailsData(false);
    //}

    protected void gvResultFinal_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }

        if (e.Item.ItemType == GridItemType.Item
            || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Label lblApplicableFor = (Label)e.Item.FindControl("lblApplicableFor");
            HiddenField hdnApplicableFor = (HiddenField)e.Item.FindControl("hdnApplicableFor");

            if (common.myStr(hdnApplicableFor.Value) == "O")
            {
                lblApplicableFor.Text = "OP";
            }
            else if (common.myStr(hdnApplicableFor.Value) == "I")
            {
                lblApplicableFor.Text = "IP";
            }
            else
            {
                lblApplicableFor.Text = "Both";
            }
        }
    }

    protected void gvResultFinal_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                HiddenField hdnTemplateTypeId = (HiddenField)e.Item.FindControl("hdnTemplateTypeId");

                hdnRTemplateId.Value = common.myStr(e.CommandArgument);
                hdnRTemplateTypeId.Value = common.myStr("1");

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                return;

                //Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                //Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                //Label lblResultRemarksId = (Label)e.Item.FindControl("lblResultRemarksId");
                //Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");

                //string source = ddlSource.SelectedValue;
                //if ((source == "PACKAGE") || (source == "PACKAGE"))
                //    source = "OPD";

                //RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/InvestigationResult.aspx?SOURCE=" + source +
                //                            "&LABNO=" + common.myInt(lblLabNo.Text) + "&MASTER=Y" +
                //                            "&SEL_DiagSampleID=" + common.myInt(lblDiagSampleID.Text) +
                //                            "&SEL_ServiceId=" + common.myInt(lblServiceId.Text) +
                //                            "&SEL_ResultRemarksId=" + common.myInt(lblResultRemarksId.Text) +
                //                            "&SEL_StatusCode=" + common.myStr(lblStatusCode.Text) +
                //                            "&Page=RF&FromMaster=" + common.myStr(Request.QueryString["MASTER"]) +
                //                            "&MD=" + Flag;

                //RadWindowPopup.Height = 570;
                //RadWindowPopup.Width = 850;
                //RadWindowPopup.Top = 10;
                //RadWindowPopup.Left = 10;
                //RadWindowPopup.VisibleOnPageLoad = true;
                //RadWindowPopup.Modal = true;
                //RadWindowPopup.OnClientClose = "OnClientResultEntryClose";
                //RadWindowPopup.VisibleStatusbar = false;
            }
            else if (e.CommandName == "Preview")
            {
                int templateId = common.myInt(e.CommandArgument);
                if (templateId > 0)
                {
                    RadWindow1.NavigateUrl = "/EMR/Masters/PrintTemplate.aspx?TemplateId=" + templateId;
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 950;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    //  RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                    RadWindow1.VisibleStatusbar = false;
                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select Template", Page);
                    return;
                }
            }
            else if (e.CommandName == "TempLink")
            {
                int templateId = common.myInt(e.CommandArgument);
                if (templateId > 0)
                {
                    RadWindow1.NavigateUrl = "/EMR/Masters/TemplateMaster1.aspx?tempid=" + templateId + "&MP=NO";
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 950;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                    RadWindow1.VisibleStatusbar = false;
                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select Template", Page);
                    return;
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

    protected void gvResultFinal_OnPreRender(object sender, EventArgs e)
    {
        foreach (GridGroupHeaderItem groupHeader in gvResultFinal.MasterTableView.GetItems(GridItemType.GroupHeader))
        {
            GridItem[] children = groupHeader.GetChildItems();
            int count = common.myInt(children.Count());
            if (common.myStr(groupHeader.DataCell.Text).Contains("; Count:"))
            {
                groupHeader.DataCell.Text = common.myStr(groupHeader.DataCell.Text).Substring(0, common.myStr(groupHeader.DataCell.Text).IndexOf(";"));
            }
            groupHeader.DataCell.Text += "; Count: " + count.ToString();
            groupHeader.DataCell.Attributes["style"] = "color:#0033FF;";
            count = 0;
        }
    }
}
