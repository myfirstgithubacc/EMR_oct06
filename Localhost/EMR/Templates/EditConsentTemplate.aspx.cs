using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
public partial class EMR_Templates_EditConsentTemplate : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowTemplateData(common.myInt(Request.QueryString["FromId"]));
            if (common.myStr(Request.QueryString["VIEW"]).Equals("DISABLED"))
            {
                btnSave.Visible = false;
                btnFinalized.Visible = false;
            }
        }

     }
    protected void ShowTemplateData(int Id)
    {

        try
        {
            if (common.myInt(Request.QueryString["FromId"]) > 0)
            {
                DataSet ds = new DataSet();
                Hashtable hshIn = new Hashtable();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

               
                //hshIn.Add("intRegistrationId", common.myInt(Session["RegistrationId"]).ToString());
                hshIn.Add("intId", common.myInt(Id));

                ds = dl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientConsentDetails", hshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Form12.InnerHtml= common.myStr(ds.Tables[0].Rows[0]["ConsentFormText"]);
                }
               
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void btnSave_Click1(object sender, EventArgs e)
    {
       
        string sContentWordProcessor = string.Empty;
        StringBuilder sbTableBorderStyle = new StringBuilder();
        if (hdntext.Value != null)
        {
            sbTableBorderStyle.Append(hdntext.Value.ToString());
        }
        else
        {
            sContentWordProcessor = "";
        }
        sContentWordProcessor = Convert.ToString(sbTableBorderStyle);
        Hashtable hshIn = new Hashtable();
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        HshIn.Add("intId", common.myInt(Request.QueryString["FromId"]));
        HshIn.Add("intFacilityId", common.myInt(Session["FacilityID"]).ToString());
        HshIn.Add("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
        HshIn.Add("RegistrationId", common.myInt(Session["RegistrationId"]).ToString());
        HshIn.Add("EncounterId", common.myInt(Session["EncounterId"]).ToString());
        HshIn.Add("ConsentTemplateId", common.myInt(0));
        HshIn.Add("ConsentFormText", common.myStr(sContentWordProcessor).Trim());
        HshIn.Add("EncodedBy", common.myInt(Session["UserId"]));
        HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
        HshOut.Add("chvErrorStatus1", SqlDbType.VarChar);
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientConsentForms", HshIn, HshOut);

        SqlConnection cn = new SqlConnection(sConString);
        cn.Open();

        string sql = "UPDATE EMRPatientConsentForms SET ConsentFormText=N'" + sContentWordProcessor + "' WHERE Id = " + HshOut["chvErrorStatus1"].ToString() + "";
        SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.ExecuteNonQuery();
        cmd.Dispose();

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertmsg", "alert('" + HshOut["chvErrorStatus"].ToString() + "');", true);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        cn.Close();
        sql = string.Empty;
     
    }




    protected void btnFinalized_Click(object sender, EventArgs e)
    {
        
        string sContentWordProcessor = string.Empty;
        StringBuilder sbTableBorderStyle = new StringBuilder();
        if (hdntext.Value != null)
        {
            sbTableBorderStyle.Append(hdntext.Value.ToString());
        }
        else
        {
            sContentWordProcessor = "";
        }
        sContentWordProcessor = Convert.ToString(sbTableBorderStyle);
        Hashtable hshIn = new Hashtable();
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        HshIn.Add("intId", common.myInt(Request.QueryString["FromId"]));
        HshIn.Add("Finalize", common.myInt(1));
        HshIn.Add("intFacilityId", common.myInt(Session["FacilityID"]).ToString());
        HshIn.Add("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
        HshIn.Add("RegistrationId", common.myInt(Session["RegistrationId"]).ToString());
        HshIn.Add("EncounterId", common.myInt(Session["EncounterId"]).ToString());
        HshIn.Add("ConsentTemplateId", common.myInt(0));
        HshIn.Add("ConsentFormText", common.myStr(sContentWordProcessor).Trim());
        HshIn.Add("EncodedBy", common.myInt(Session["UserId"]));
        HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
        HshOut.Add("chvErrorStatus1", SqlDbType.VarChar);
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientConsentForms", HshIn, HshOut);

        SqlConnection cn = new SqlConnection(sConString);
        cn.Open();

        string sql = "UPDATE EMRPatientConsentForms SET ConsentFormText=N'" + sContentWordProcessor + "' WHERE Id = " + HshOut["chvErrorStatus1"].ToString() + "";
        SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.ExecuteNonQuery();
        cmd.Dispose();

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertmsg", "alert('" + HshOut["chvErrorStatus"].ToString() + "');", true);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        cn.Close();
        sql = string.Empty;
     
    }
}