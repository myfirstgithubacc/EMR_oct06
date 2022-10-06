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

public partial class MRD_ICDFlagMaster : System.Web.UI.Page
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
            BindICDFlagMaster();
        }
    }
    private void BindICDFlagMaster()
    {
        try
        {
            objMRD = new BaseC.clsMRD(sConString);
            DataSet ds = objMRD.GetICDFlagMaster();

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvItemFlagMaster.DataSource = ds.Tables[0];
            }
            else
            {
                gvItemFlagMaster.DataSource = ds.Tables[0].Clone();
            }
            gvItemFlagMaster.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void gvItemFlagMaster_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Label lblICDFlagName = (Label)gvItemFlagMaster.SelectedItems[0].FindControl("lblICDFlagName");
            txtICDFlagName.Text = common.myStr(lblICDFlagName.Text);

            Label lblStatus = (Label)gvItemFlagMaster.SelectedItems[0].FindControl("lblStatus");
            ddlstatus.SelectedIndex = ddlstatus.Items.IndexOf(ddlstatus.Items.FindItemByText(common.myStr(lblStatus.Text)));

            HiddenField hdnICDFlagId = (HiddenField)gvItemFlagMaster.SelectedItems[0].FindControl("hdnICDFlagId");
            ViewState["ICDFlagId"] = hdnICDFlagId.Value;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void gvItemFlagMaster_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        BindICDFlagMaster();
    }
    protected bool isSave()
    {
        lblMessage.Text = "&nbsp;";
        bool isValid = true;
        if (txtICDFlagName.Text == "")
        {
            lblMessage.Text = "Item Flag can't be blank !";
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

            string strMsg = objMRD.SaveICDFlagMaster(common.myInt(ViewState["ICDFlagId"]), common.myStr(txtICDFlagName.Text, true).Trim(), common.myInt(Session["HospitalLocationID"]), common.myInt(ddlstatus.SelectedValue), common.myInt(Session["UserID"]));

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
            txtICDFlagName.Text = "";
            ViewState["ICDFlagId"] = null;
            ddlstatus.SelectedIndex = ddlstatus.Items.IndexOf(ddlstatus.Items.FindItemByValue("1"));
            BindICDFlagMaster();
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
