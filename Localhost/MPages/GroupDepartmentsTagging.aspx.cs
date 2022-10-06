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
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Text;
using System.Drawing;

public partial class MPages_GroupDepartmentsTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private bool RowSelStatus = false;
    clsExceptionLog objException = new clsExceptionLog();

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

            ViewState["GroupId"] = common.myInt(Request.QueryString["GroupId"]);
            lblGroupName.Text = common.myStr(Request.QueryString["GroupName"]);

            bindFields();
            bindSelectedFields();
        }
    }

    private void bindFields()
    {
        try
        {
            BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
            DataSet ds = objPhr.getGroupDepartmentTagged(common.myInt(ViewState["GroupId"]),common.myInt(Session["FacilityId"]), false);

            gvFields.DataSource = ds.Tables[0];
            gvFields.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void bindSelectedFields()
    {
        try
        {
            if (common.myInt(ViewState["GroupId"]) > 0)
            {
                BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
                DataSet ds = objPhr.getGroupDepartmentTagged(common.myInt(ViewState["GroupId"]), true);

                gvSelectedFields.DataSource = ds.Tables[0];
                gvSelectedFields.DataBind();

                ViewState["SelectedDepartment"] = ds.Tables[0];
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private DataTable CreateTable()
    {
        DataTable objDt = new DataTable();
        objDt.Columns.Add("DepartmentId");
        objDt.Columns.Add("DepartmentName");
        objDt.Columns.Add("SequenceNo");
        return objDt;
    }

    protected void gvFields_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        bindFields();
    }

    protected void gvFields_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddToList")
            {
                lblMessage.Text = "&nbsp;";
                RowSelStatus = true;
                gvFields.Items[e.Item.ItemIndex].Selected = true;

                HiddenField hdnDepartmentId = (HiddenField)e.Item.FindControl("hdnDepartmentId");
                Label lblDepartmentName = (Label)e.Item.FindControl("lblDepartmentName");

                DataTable dt = (DataTable)ViewState["SelectedDepartment"];

                DataView DV = dt.Copy().DefaultView;

                DV.RowFilter = " DepartmentId = " + common.myInt(hdnDepartmentId.Value);

                if (DV.ToTable().Rows.Count > 0)
                {
                    lblMessage.Text = "Department Name '" + lblDepartmentName.Text + "' Already Exists !";
                }
                else
                {
                    DataRow DR = dt.NewRow();
                    DR["DepartmentId"] = common.myInt(hdnDepartmentId.Value);
                    DR["DepartmentName"] = common.myStr(lblDepartmentName.Text);
                    dt.Rows.Add(DR);

                    gvSelectedFields.DataSource = dt;
                    gvSelectedFields.DataBind();

                    ViewState["SelectedDepartment"] = dt;
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

    protected void gvFields_PreRender(object sender, EventArgs e)
    {
        if (RowSelStatus == false)
        {
            bindFields();
        }
    }

    protected void gvFields_SortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        bindFields();
    }

    protected void gvFields_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void cmdNew_Click(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void cmdSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(ViewState["GroupId"]) > 0 && gvSelectedFields.Items.Count != 0)
            {
                int GroupId = common.myInt(ViewState["GroupId"]);

                StringBuilder strXml = new StringBuilder();
                ArrayList coll = new ArrayList();

                foreach (Telerik.Web.UI.GridItem item in gvSelectedFields.Items)
                {
                    HiddenField hdnDepartmentId = (HiddenField)item.FindControl("hdnDepartmentId");

                    if (common.myInt(hdnDepartmentId.Value) > 0)
                    {
                        coll.Add(hdnDepartmentId.Value);
                        strXml.Append(common.setXmlTable(ref coll));
                    }
                }

                BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);

                string strMsg = objPhr.SaveGroupDepartments(GroupId, strXml.ToString(), common.myInt(Session["UserID"]));

                if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                    bindFields();
                    bindSelectedFields();
                }

                lblMessage.Text = strMsg;
            }
            else
            {
                lblMessage.Text = "Record Not Saved...";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvSelectedFields_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            int selectedIdx = e.Item.ItemIndex;

            if (e.CommandName == "Delete1")
            {
                DataTable dt = (DataTable)ViewState["SelectedDepartment"];

                gvSelectedFields.Items[selectedIdx].Selected = true;

                HiddenField hdnDepartmentId = (HiddenField)gvSelectedFields.SelectedItems[0].FindControl("hdnDepartmentId");

                DataView DV = dt.Copy().DefaultView;
                DV.RowFilter = " DepartmentId <> " + common.myInt(hdnDepartmentId.Value);

                gvSelectedFields.DataSource = DV.ToTable();
                gvSelectedFields.DataBind();

                ViewState["SelectedDepartment"] = DV.ToTable();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvSelectedFields_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            HiddenField hdnDepartmentId = (HiddenField)e.Item.FindControl("hdnDepartmentId");
            ImageButton imgBtn = (ImageButton)e.Item.FindControl("imgDelete");

            if (common.myInt(hdnDepartmentId.Value) < 1)
            {
                imgBtn.Visible = false;
            }
        }
    }
}
