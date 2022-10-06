using System;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class EMR_Masters_DefaultTextPopup : System.Web.UI.Page
{
    clsExceptionLog objException = new clsExceptionLog();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        { 
        BindData();
        }
    }
    public void BindData()
    {
        clsIVF objIVF = new clsIVF(sConString);
        try
        {
            if (common.myInt(Request.QueryString["DetailId"]) != 0 && Request.QueryString["DetailId"] != null)
            {
                string DefaultText = string.Empty;
               
                objIVF.GetDefaultText (common.myInt(Request.QueryString["DetailId"]), out DefaultText);
                DefaultText= DefaultText.Replace("<br />", "\n");
                txtDefaultText.Text = DefaultText;
             

                //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //lblMessage.Text = strMsg;
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
            objIVF = null;
        }
    }


    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        clsIVF objIVF = new clsIVF(sConString);
        string replaceDefaultText = string.Empty;
        string txtDefaultTextValue = string.Empty;

        try
        {
            if (common.myInt(Request.QueryString["DetailId"]) != 0 && Request.QueryString["DetailId"] != null)
            {
                //txtDefaultText.Text.Replace(Environment.NewLine, "<br />");
               // txtDefaultText.Text.Replace("\n", "<br />");
               

                txtDefaultTextValue = common.myStr(txtDefaultText.Text);
                replaceDefaultText = common.myStr(txtDefaultText.Text);
                replaceDefaultText= replaceDefaultText.Replace("\n", "<br />");
                //string strMsg = objIVF.SetDefaultText(common.myInt(Request.QueryString["DetailId"]), common.myStr(txtDefaultText.Text));
                string strMsg = objIVF.SetDefaultText(common.myInt(Request.QueryString["DetailId"]), common.myStr(replaceDefaultText));
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;
               
            }
            BindData();
            txtDefaultText.Text = txtDefaultTextValue;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objIVF = null;
        }
    }
}