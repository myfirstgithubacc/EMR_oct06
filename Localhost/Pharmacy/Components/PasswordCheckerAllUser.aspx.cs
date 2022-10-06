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

public partial class Pharmacy_Components_PasswordCheckerAllUser : System.Web.UI.Page
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
            txtUserName.Text = common.myStr(Session["UserName"]);
            lblEmployeeName.Text = common.myStr(Session["UserName"]);
            txtPassword.Focus();
            txtUserName.Enabled = false;
        }
        lblMessage.Text = "";
       
    }

    public string GetUserName(int iUserID)
    {
        string sUName = string.Empty;
        BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        HshIn.Add("@intUID", iUserID);

        string qry = " SELECT UserName, PrivateKey FROM Users u WITH (NOLOCK) INNER JOIN UserPrivateKey up WITH (NOLOCK) ON u.ID = up.UserId  ";        
        qry += " WHERE u.EmpID=@intUID ";


        DataSet ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        if (ds.Tables[0].Rows.Count > 0)
        {
            sUName = en.Decrypt(Convert.ToString(ds.Tables[0].Rows[0]["UserName"]), en.getKey(sConString), true);
        }

        return sUName;
    }

    protected bool isSave()
    {
        lblMessage.Text = "";
        bool isValid = true;
        if (txtUserName.Text == "")
        {
            lblMessage.Text = "User not entered  !";
            isValid = false;
        }
        
        if(txtPassword.Text == "")
        {
            txtPassword.Focus();
            //lblMessage.Text = "Please Enter Password !";
            isValid = false;
        }

        return isValid;
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        BaseC.User valUser = new BaseC.User(sConString);
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

            ds = objUser.ValidateUserName(common.myStr(txtUserName.Text), common.myStr(txtPassword.Text));

            if (ds != null)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                   if(!string.IsNullOrEmpty(ds.Tables[1].Rows[0][0].ToString()))
                    hdnIsValidPassword.Value = "1";

                }
            }

            if (common.myInt(hdnIsValidPassword.Value) == 1)
            {
                PasswordRequiredHelper.SetTransactionUserId(valUser.GetUserID(txtUserName.Text));
               // Session["SaveUserId"] = valUser.GetUserID(txtUserName.Text);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                return;
            }
            else
            {
                PasswordRequiredHelper.SetTransactionUserId(null);
                //Session["SaveUserId"] = null;
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
}