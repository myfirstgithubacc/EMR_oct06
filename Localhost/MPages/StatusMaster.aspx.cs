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

public partial class MPages_StatusMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsEMR objGeneric;
    private bool RowSelStatus = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindStatusType();
            GetStatusBind();
            if (common.myStr(Session["MainFacility"]) == "True" || common.myStr(Session["MainFacility"]) == "1" || common.myBool(Session["MainFacility"]))
            {
                btnSaveData.Visible = true;
            }
            else
            {
                btnSaveData.Visible = false;
            }
        }
        lblMessage.Text = "&nbsp;";
    }

    private void BindStatusType()
    {
        ddlStatusType.Items.Clear();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        objGeneric = new BaseC.clsEMR(sConString);
        DataSet ds = objGeneric.getStatusType();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlStatusType.DataSource = ds;
            ddlStatusType.DataValueField = "StatusType";
            ddlStatusType.DataTextField = "StatusType";
            ddlStatusType.DataBind();
            //ddlStatusType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(""));
        }
    }

    private void GetStatusBind()
    {
        try
        {
            objGeneric = new BaseC.clsEMR(sConString);
            DataSet ds = objGeneric.getStatus(0, ddlStatusType.SelectedValue);
            gvStatus.DataSource = ds;
            gvStatus.DataBind();
            ViewState["Id"] = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        clear();
    }

    private void clear()
    {
        ddlStatusType.SelectedValue = "";
        ddlStatusType.Text = "";
        txtStatusName.Text = "";
        txtStatusColor.Text = "";
        txtColorName.Text = "";
        txtStatusCode.Text = "";
        txtSequence.Text = "";
        ddlStatus.SelectedValue = "1";
        ViewState["Id"] = null;

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

            objGeneric = new BaseC.clsEMR(sConString);
            string strMsg = Convert.ToString(objGeneric.SaveStatus(common.myInt(ViewState["Id"]), common.myStr(ddlStatusType.SelectedValue), common.myStr(txtStatusName.Text), common.myStr(txtStatusColor.Text), common.myStr(txtColorName.Text), common.myStr(txtStatusCode.Text), common.myInt(txtSequence.Text), common.myInt(ddlStatus.SelectedValue)));
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                clear();
                GetStatusBind();
                ViewState["Id"] = null;
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

    protected void gvStatus_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        GetStatusBind();
    }

    protected void gvStatus_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                RowSelStatus = true;
                ViewState["Id"] = ((Label)e.Item.FindControl("lblStatusId")).Text;
                objGeneric = new BaseC.clsEMR(sConString);
                DataSet ds = objGeneric.getStatus(common.myInt(ViewState["Id"]), common.myStr(ddlStatusType.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtStatusName.Text = common.myStr(ds.Tables[0].Rows[0]["Status"]);
                    txtStatusColor.Text = common.myStr(ds.Tables[0].Rows[0]["StatusColor"]);
                    txtColorName.Text = common.myStr(ds.Tables[0].Rows[0]["ColorName"]);
                    txtStatusCode.Text = common.myStr(ds.Tables[0].Rows[0]["Code"]);
                    txtSequence.Text = common.myStr(ds.Tables[0].Rows[0]["Sequenceno"]);
                    ddlStatus.SelectedValue = common.myBool(ds.Tables[0].Rows[0]["Active"]) == true ? "1" : "0";

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

    protected void gvStatus_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvStatus_PreRender(object sender, EventArgs e)
    {
        if (RowSelStatus == false)
        {
            GetStatusBind();
        }
    }

    protected bool isSave()
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            bool isValid = true;

            if (txtStatusName.Text == "")
            {
                lblMessage.Text = " Status can't be blank !";
                isValid = false;
                return isValid;
            }

            if (txtStatusCode.Text == "")
            {
                lblMessage.Text = " Code can't be blank !";
                isValid = false;
                return isValid;
            }


            if (txtSequence.Text == "")
            {
                lblMessage.Text = "Sequence No can't be blank !";
                isValid = false;
                return isValid;
            }


            if (ddlStatusType.SelectedValue == "")
            {
                lblMessage.Text = " Select proper status type before saving record!";
                isValid = false;
                return isValid;
            }


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
    protected void ddlStatusType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GetStatusBind();
    }
}
