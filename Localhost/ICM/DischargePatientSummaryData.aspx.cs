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
public partial class ICM_DischargePatientSummaryData : System.Web.UI.Page
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
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;
            bindData();
        }
    }

    private void bindData()
    {
        DataSet ds = new DataSet();
        BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
        try
        {
            ds = ObjIcm.getDischargePatientSummaryData(common.myInt(Request.QueryString["SummaryId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                RTF1.Content = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
            }
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
        }
    }
}