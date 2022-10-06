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


public partial class ICM_DischargeSummaryDeFinalisationAuthentication : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["IsDefinalizedSuccess"] = "0";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            bindControl();
        }
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        BaseC.User objUser = new BaseC.User(sConString);
        BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
        try
        {
            if (common.myLen(Session["UserName"]).Equals(0))
            {
                Session["UserName"] = objUser.GetUserName(common.myInt(Session["UserId"]));
            }

            lblUserName.Text = common.myStr(Session["UserName"]);

            //////////////////////

            ds = ObjIcm.GetICMSignDoctors(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            DV = ds.Tables[0].DefaultView;
            DV.RowFilter = "IsDoctor=1";

            ddlDeFinalizeRecommendBy.DataSource = DV.ToTable();
            ddlDeFinalizeRecommendBy.DataTextField = "DoctorName";
            ddlDeFinalizeRecommendBy.DataValueField = "ID";
            ddlDeFinalizeRecommendBy.DataBind();

            ddlDeFinalizeRecommendBy.Items.Insert(0, new RadComboBoxItem("", ""));
            ddlDeFinalizeRecommendBy.SelectedIndex = ddlDeFinalizeRecommendBy.Items.IndexOf(ddlDeFinalizeRecommendBy.Items.FindItemByValue(common.myInt(Request.QueryString["FinalizeBy"]).ToString()));
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            ObjIcm = null;
            ds.Dispose();
            DV.Dispose();
        }
    }

    protected bool checkPassword()
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        BaseC.User objUser = new BaseC.User(sConString);

        DataSet ds = new DataSet();
        bool IsValidPassword = false;
        try
        {

            ds = objUser.ValidateUserName(common.myStr(lblUserName.Text), common.myStr(txtPassword.Text));

            if (ds != null)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    IsValidPassword = true;
                }
            }

            if (!IsValidPassword)
            {
                lblMessage.Text = "Invalid Password !";
                txtPassword.Text = string.Empty;
                txtPassword.Focus();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }

        return IsValidPassword;
    }

    private bool isSaved()
    {
        bool isSave = true;
        if (common.myInt(ddlDeFinalizeRecommendBy.SelectedValue).Equals(0))
        {
            lblMessage.Text = "Please selectd Recommend By (Doctor) !";
            isSave = false;
            return isSave;
        }
        if (common.myLen(txtDeFinalizeReason.Text).Equals(0))
        {
            lblMessage.Text = "Reason can't be blank !";
            isSave = false;
            return isSave;
        }
        if (common.myLen(txtPassword.Text).Equals(0))
        {
            lblMessage.Text = "Please enter password !";
            isSave = false;
            return isSave;
        }

        isSave = checkPassword();

        return isSave;
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        BaseC.ICM objIcm = new BaseC.ICM(sConString);
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }

            string strMsg = objIcm.SaveDischargeSummaryAuditTrail(common.myInt(Request.QueryString["SummaryId"]), common.myInt(Session["UserID"]),
                                                common.myInt(ddlDeFinalizeRecommendBy.SelectedValue), common.myStr(txtDeFinalizeReason.Text).Trim());
            string Type = common.myStr(Request.QueryString["Type"]);
            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                objIcm.UpdateICMFinalize(common.myInt(Session["HospitalLocationID"]), common.myInt(Request.QueryString["SummaryId"]), false,
                                                common.myInt(Request.QueryString["FinalizeBy"]), common.myInt(Session["UserID"]), Type);

                Session["IsDefinalizedSuccess"] = "1";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Discharge summary Definalized";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);

                return;
            }
            Type = string.Empty;
            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
}