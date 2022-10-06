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

public partial class Pharmacy_Components_PasswordChecker : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.User objUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myStr(Session["UserName"]) == "")
            {
                objUser = new BaseC.User(sConString);
                Session["UserName"] = objUser.GetUserName(common.myInt(Session["UserId"]));
            }

            lblUserName.Text = common.myStr(Session["UserName"]);
            string UseFor = common.myStr(Request.QueryString["UseFor"]);
            txtPassword.Focus();
        }
        lblMessage.Text = "&nbsp;";
    }

    protected bool isSave()
    {
        lblMessage.Text = "&nbsp;";
        bool isValid = true;

        if (txtPassword.Text == "")
        {
            lblMessage.Text = "Please Enter Password !";
            isValid = false;
        }

        return isValid;
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        if (!isSave())
        {
            return;
        }
        try
        {
            objUser = new BaseC.User(sConString);

            hdnIsValidPassword.Value = "0";

            DataSet ds = new DataSet();
            ds = objUser.ValidateUserName(common.myStr(lblUserName.Text), common.myStr(txtPassword.Text));

            if (ds != null)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    hdnIsValidPassword.Value = "1";
                } 
            }

            if (common.myInt(hdnIsValidPassword.Value) == 1)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                return;
            }
            else
            {
                lblMessage.Text = "Invalid Password !";
                txtPassword.Text = "";
                txtPassword.Focus();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    protected void btnClose_OnClick(object sender, EventArgs e)
    {
        hdnIsValidPassword.Value = "0";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        return;
    }

}