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
public partial class MRD_Master_ICD9GroupMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsMRD objMRD;
    clsExceptionLog objException = new clsExceptionLog();
    private bool RowSelStatus = false;
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
            BindICDSubGroupDetail();
            bindGroup();
        }
    }

    private void bindGroup()
    {
        DataSet ds = new DataSet();
        try
        {
            objMRD = new BaseC.clsMRD(sConString);
            ds = objMRD.getIcd9Group();

            ddlGroup.DataSource = ds.Tables[0].Copy();
            ddlGroup.DataValueField = "Id";
            ddlGroup.DataTextField = "Name";
            ddlGroup.DataBind();

            ddlGroup.Items.Insert(0, new RadComboBoxItem("", "0"));


            //bindSubGroup();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void BindICDSubGroupDetail()
    {
        try
        {
            objMRD = new BaseC.clsMRD(sConString);
            DataSet ds = objMRD.getIcd9SubGroupDetail();

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvIcdSubGroupMaster.DataSource = ds.Tables[0];
            }
            else
            {
                gvIcdSubGroupMaster.DataSource = ds.Tables[0].Clone();
            }
            gvIcdSubGroupMaster.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void gvIcdSubGroupMaster_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnGroupId = (HiddenField)gvIcdSubGroupMaster.SelectedItems[0].FindControl("hdnGroupId");
            ddlGroup.SelectedIndex = ddlGroup.Items.IndexOf(ddlGroup.Items.FindItemByValue(common.myStr(hdnGroupId.Value)));

            HiddenField hdnsubGroupId = (HiddenField)gvIcdSubGroupMaster.SelectedItems[0].FindControl("hdnsubGroupId");
            Label lblSubGroupName = (Label)gvIcdSubGroupMaster.SelectedItems[0].FindControl("lblSubGroupName");
            txtSubGroupName.Text = common.myStr(lblSubGroupName.Text);
            ViewState["SubGroupId"] = common.myInt(hdnsubGroupId.Value);

            Label lblRangeStart = (Label)gvIcdSubGroupMaster.SelectedItems[0].FindControl("lblRangeStart");
            txtRangeStart.Text = common.myStr(lblRangeStart.Text);

            Label lblRangeEnd = (Label)gvIcdSubGroupMaster.SelectedItems[0].FindControl("lblRangeEnd");
            txtRangeEnd.Text = common.myStr(lblRangeEnd.Text);

            Label lblActive = (Label)gvIcdSubGroupMaster.SelectedItems[0].FindControl("lblActive");
            ddlstatus.SelectedIndex = ddlstatus.Items.IndexOf(ddlstatus.Items.FindItemByText(common.myStr(lblActive.Text)));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void gvIcdSubGroupMaster_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        BindICDSubGroupDetail();
    }
    protected bool isSave()
    {
        lblMessage.Text = "&nbsp;";
        bool isValid = true;
        if (ddlGroup.SelectedItem.Text == "")
        {
            lblMessage.Text = "Group can't be blank !";
            isValid = false;
        }
        if (txtSubGroupName.Text == "")
        {
            lblMessage.Text = "Item Sub Group can't be blank !";
            isValid = false;
        }
        return isValid;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (!isSave())
            {
                return;
            }

            objMRD = new BaseC.clsMRD(sConString);

            string strMsg = objMRD.SaveICD9SubGroupMaster(common.myInt(ddlGroup.SelectedValue),common.myInt(ViewState["SubGroupId"]),
                common.myStr(txtSubGroupName.Text), common.myStr(txtRangeStart.Text), common.myStr(txtRangeEnd.Text),
                common.myInt(Session["HospitalLocationID"]), common.myInt(ddlstatus.SelectedValue), common.myInt(Session["UserID"]));

            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")))
            {
                ClearPropertiesFields();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
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
    protected void ClearPropertiesFields()
    {
        try
        {
            ddlstatus.SelectedIndex = ddlstatus.Items.IndexOf(ddlstatus.Items.FindItemByValue(" "));
            txtSubGroupName.Text = "";
            txtRangeStart.Text = "";
            txtRangeEnd.Text = "";
            ViewState["SubGroupId"] = null;
            ddlstatus.SelectedIndex = ddlstatus.Items.IndexOf(ddlstatus.Items.FindItemByValue("1"));
            BindICDSubGroupDetail();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "&nbsp;";
        ClearPropertiesFields();
    }
}
