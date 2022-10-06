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

public partial class MPages_HospitalMandatoryFieldsSetup : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.HospitalSetup objHS;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["_ID"] = 0;

            bindControl();
        }
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        try
        {
            objHS = new BaseC.HospitalSetup(sConString);
            BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
            //ddlFacilityId
            ds = objlis.getFacility(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));

            ddlFacilityId.DataSource = ds.Tables[0];
            ddlFacilityId.DataTextField = "Name";
            ddlFacilityId.DataValueField = "FacilityID";
            ddlFacilityId.DataBind();

            ddlFacilityId.Items.Insert(0, new RadComboBoxItem("", ""));


            //ddlModule
            ds = objHS.getModuleMaster();

            DataView DV = ds.Tables[0].DefaultView;
            //  DV.RowFilter = "ModuleName IN('Registration', 'ATD','Administration')";

            ddlModule.DataSource = DV.ToTable();
            ddlModule.DataTextField = "ModuleName";
            ddlModule.DataValueField = "ModuleId";
            ddlModule.DataBind();

            ddlModule.Items.Insert(0, new RadComboBoxItem("", "0"));

            //ddlPage
            bindModulePages(0);

            //ds = objHS.getHospitalMandatoryPages();

            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    RadComboBoxItem item = new RadComboBoxItem();
            //    item.Text = common.myStr(dr["PageName"]);
            //    item.Value = common.myStr(dr["PageName"]);//PageId
            //    item.Attributes.Add("TableName", common.myStr(dr["TableName"]));

            //    ddlPage.Items.Add(item);
            //    item.DataBind();
            //}

            //ddlPage.Items.Insert(0, new RadComboBoxItem("", ""));

            //ddlField
            //bindField("");
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

    private void bindModulePages(int ModuleId)
    {
        DataSet ds = new DataSet();
        try
        {
            objHS = new BaseC.HospitalSetup(sConString);
            ds = objHS.getModulePages(ModuleId);

            ddlPage.DataSource = ds.Tables[0];
            ddlPage.DataTextField = "PageName";
            ddlPage.DataValueField = "PageName";
            ddlPage.DataBind();

            ddlPage.Items.Insert(0, new RadComboBoxItem("", ""));

            bindDetailsData();
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

    //private void bindField(string TableNames)
    //{
    //    DataSet ds2 = new DataSet();
    //    try
    //    {
    //        objHS = new BaseC.HospitalSetup(sConString);
    //        ds2 = objHS.getTableColumns(TableNames);

    //        ddlField.DataSource = ds2.Tables[0];
    //        ddlField.DataTextField = "ColumnName";
    //        ddlField.DataValueField = "ColumnName";
    //        ddlField.DataBind();

    //        ddlField.Items.Insert(0, new RadComboBoxItem("", ""));
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        ds2.Dispose();
    //    }
    //}


    private void bindDetailsData()
    {
        DataSet ds = new DataSet();
        try
        {
            if (common.myInt(ddlFacilityId.SelectedValue) > 0 && common.myStr(ddlModule.SelectedItem.Text) != "" && common.myStr(ddlPage.SelectedItem.Text) != "")
            {
                objHS = new BaseC.HospitalSetup(sConString);
                ds = objHS.getHospitalMandatoryFields(common.myInt(Session["HospitalLocationID"]),
                                            common.myInt(ddlFacilityId.SelectedValue),
                                            common.myStr(ddlModule.SelectedItem.Text),
                                            common.myStr(ddlPage.SelectedItem.Text));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvData.DataSource = ds;
                        gvData.DataBind();
                    }
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

    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    private void clearControl()
    {

        ddlModule.SelectedIndex = 0;
        ddlPage.SelectedIndex = 0;
    }

    protected bool isSave()
    {
        try
        {
            StringBuilder strmsg = new StringBuilder();
            bool isValid = true;

            if (common.myInt(ddlFacilityId.SelectedValue) == 0)
            {
                strmsg.Append("Facility not selected ! ");
                isValid = false;
            }

            if (common.myInt(ddlModule.SelectedValue) == 0)
            {
                strmsg.Append("Module not selected ! ");
                isValid = false;
            }
            if (common.myStr(ddlPage.SelectedValue).Trim() == "")
            {
                strmsg.Append("Page not selected ! ");
                isValid = false;
            }

            lblMessage.Text = strmsg.ToString();
            return isValid;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
            return false;
        }
    }

    protected void btnSaveData_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (!isSave())
            {
                return;
            }

            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();

            foreach (GridViewRow dataItem in gvData.Rows)
            {
                Label lblFieldName = (Label)dataItem.FindControl("lblFieldName");
                CheckBox ChkIsMandatoryField = (CheckBox)dataItem.FindControl("ChkIsMandatoryField");
                CheckBox ChkEnable = (CheckBox)dataItem.FindControl("ChkEnable");
                CheckBox ChkIsPreferableField = (CheckBox)dataItem.FindControl("ChkIsPreferableField");
                

                if (common.myStr(lblFieldName.Text) != "")
                {
                    coll.Add(common.myStr(lblFieldName.Text));
                    coll.Add(common.myStr(ChkIsMandatoryField.Checked));
                    coll.Add(common.myStr(ChkEnable.Checked));
                    coll.Add(common.myStr(1));
                    coll.Add(common.myStr(ChkIsPreferableField.Checked));
                    strXML.Append(common.setXmlTable(ref coll));
                }
            }

            objHS = new BaseC.HospitalSetup(sConString);

            string strMsg = objHS.SaveHospitalMandatoryFields(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacilityId.SelectedValue),
                                    common.myStr(ddlModule.SelectedItem.Text), common.myStr(ddlPage.SelectedValue),
                                    strXML.ToString(), common.myInt(Session["UserID"]));

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                bindDetailsData();
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

    //protected void gvData_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    //{
    //    bindDetailsData();
    //}

    //protected void gvData_OnItemCommand(object sender, GridCommandEventArgs e)
    //{
    //    DataSet ds = new DataSet();
    //    try
    //    {
    //        lblMessage.Text = string.Empty;
    //        if (e.CommandName.Equals("Select"))
    //        {
    //            RowSelStatus = true;

    //            HiddenField hdnFieldId = (HiddenField)e.Item.FindControl("hdnFieldId");
    //            ViewState["_ID"] = common.myInt(hdnFieldId.Value);

    //            objHS = new BaseC.HospitalSetup(sConString);
    //            ds = objHS.getHospitalMandatoryFields(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["_ID"]), 0);

    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["ModuleName"])));
    //                ddlPage.SelectedIndex = ddlPage.Items.IndexOf(ddlPage.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["PageName"])));
    //                ddlField.SelectedIndex = ddlField.Items.IndexOf(ddlField.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["FieldName"])));

    //                ddlStatus.SelectedValue = common.myBool(ds.Tables[0].Rows[0]["Active"]) == true ? "1" : "0";
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        ds.Dispose();
    //    }
    //}

    //protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Item is GridPagerItem)
    //        {
    //            GridPagerItem pager = (GridPagerItem)e.Item;
    //            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
    //            lbl.Visible = false;

    //            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
    //            combo.Visible = false;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    //protected void gvData_PreRender(object sender, EventArgs e)
    //{
    //    if (RowSelStatus == false)
    //    {
    //        bindDetailsData();
    //    }
    //}

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Find the checkbox control in header and add an attribute
            ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAll('" +
                ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
        }
        //else if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Label lblQty = (Label)e.Row.FindControl("lblQty");
        //    Label lblIssueQty = (Label)e.Row.FindControl("lblIssueQty");

        //    lblQty.Text = common.myDbl(lblQty.Text).ToString("F2");
        //    lblIssueQty.Text = common.myDbl(lblIssueQty.Text).ToString("F2");
        //}
    }

    protected void ddlFacilityId_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindDetailsData();
    }

    protected void ddlPage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //bindField(common.myStr(ddlPage.SelectedItem.Attributes["TableName"]));

        bindDetailsData();
    }
    protected void gvData_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {

    }

    //protected void gvData_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    //{
    //    DataSet ds = new DataSet();
    //    try
    //    {
    //        if (e.CommandName == "EditRow")
    //        {
    //            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

    //            HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");

    //            ViewState["_ID"] = common.myInt(hdnFieldId.Value);

    //            objHS = new BaseC.HospitalSetup(sConString);
    //            ds = objHS.getHospitalMandatoryFields(common.myInt(ViewState["_ID"]), common.myInt(Session["HospitalLocationID"]),
    //                                    common.myInt(Session["FacilityId"]), common.myStr(ddlModule.SelectedValue), 0);

    //            clearControl();

    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                //ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["ModuleName"]).Trim()));
    //                ddlModule.SelectedItem.Text = common.myStr(ds.Tables[0].Rows[0]["ModuleName"]).Trim();
    //                ddlModule.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["ModuleName"]).Trim();
    //                ddlPage.SelectedIndex = ddlPage.Items.IndexOf(ddlPage.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["PageName"]).Trim()));

    //                bindField(common.myStr(ddlPage.SelectedItem.Attributes["TableName"]));
    //                ddlField.SelectedIndex = ddlField.Items.IndexOf(ddlField.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["FieldName"]).Trim()));

    //                ddlStatus.SelectedValue = common.myBool(ds.Tables[0].Rows[0]["Active"]) == true ? "1" : "0";
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        ds.Dispose();
    //    }
    //}

    protected void ddlModule_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindModulePages(common.myInt(ddlModule.SelectedValue));
    }

    //public void CheckBoxChanged(object sender, EventArgs e)
    //{
    //   foreach (GridViewRow row in gvData.Rows)
    //    {            CheckBox chkRow = (CheckBox)row.FindControl("chkRow");
    //        CheckBox ChkIsMandatoryField =(CheckBox)row.FindControl("ChkIsMandatoryField");
    //        if (!chkRow.Checked)
    //        {
    //            ChkIsMandatoryField.Checked = false;
    //            //isSelectAll = false;
    //            break;
    //        }        }
    //    // cbSelectAll.Checked = isSelectAll;
    //}


    protected void ChkEnable_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        GridViewRow parentRow = cb.NamingContainer as GridViewRow;
        CheckBox ChkIsMandatoryField = parentRow.FindControl("ChkIsMandatoryField") as CheckBox;
        if (!cb.Checked)
            ChkIsMandatoryField.Checked = false;
    }

    protected void ChkIsMandatoryField_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        GridViewRow parentRow = cb.NamingContainer as GridViewRow;
        CheckBox ChkEnable = parentRow.FindControl("ChkEnable") as CheckBox;
        if (cb.Checked)
            ChkEnable.Checked = true;
    }
}
