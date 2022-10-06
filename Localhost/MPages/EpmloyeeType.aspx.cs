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

public partial class MPages_EmployeeType : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsEMR objGeneric;
    private bool RowSelStatus = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetTitleBind();
            if (common.myStr(Session["MainFacility"]) == "True" || common.myStr(Session["MainFacility"]).Equals("1"))
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

    private void GetTitleBind()
    {
        try
        {
            objGeneric = new BaseC.clsEMR (sConString);
            DataSet ds = objGeneric.getEmployeeType(0);
            gvEpmloyeeType.DataSource = ds;
            gvEpmloyeeType.DataBind();
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
        txtDescription.Text = "";
        ddlStatus.SelectedValue = "1";
        ViewState["Id"] = null;
        txtEpmloyeeType.Text = "";
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
            string strMsg = Convert.ToString(objGeneric.SaveEmployeeType(common.myInt(ViewState["Id"]), txtDescription.Text.Trim(),common.myStr(txtEpmloyeeType.Text )  , common.myInt(ddlStatus.SelectedValue)));
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                GetTitleBind();
                clear();
                ViewState["TitleId"] = null;
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

    protected void gvUnit_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
         GetTitleBind();
    }

    protected void gvUnit_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                RowSelStatus = true;
                ViewState["Id"] = ((Label)e.Item.FindControl("lblTitleId")).Text;
                objGeneric = new BaseC.clsEMR(sConString);
                DataSet ds = objGeneric.getEmployeeType (common.myInt(ViewState["Id"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtDescription.Text = common.myStr(ds.Tables[0].Rows[0]["Description"]);
                    ddlStatus.SelectedValue = common.myBool(ds.Tables[0].Rows[0]["Active"]) == true ? "1" : "0";
                    txtEpmloyeeType.Text = common.myStr(ds.Tables[0].Rows[0]["employeeType"]);
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

    protected void gvUnit_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void gvUnit_PreRender(object sender, EventArgs e)
    {
        if (RowSelStatus == false)
        {
             GetTitleBind();
        }
    }

    protected bool isSave()
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            bool isValid = true;

            if (txtDescription.Text == "")
            {
                lblMessage.Text = Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "Description ").ToString()) + " can't be blank !";
                isValid = false;
                return isValid;
            }


            if (txtEpmloyeeType.Text.Trim()  == "")
            {
                lblMessage.Text = Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "Employee Type ").ToString()) + " can't be blank !";
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
}
