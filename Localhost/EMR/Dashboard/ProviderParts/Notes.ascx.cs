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
using System.Data.SqlClient;
using Telerik.Web.UI;
using BaseC;

public partial class EMR_Dashboard_ProviderParts_Notesl : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DL_Funs fun = new DL_Funs();
    Hashtable hsTb = new Hashtable();

    public string ChangeDateFormat(string dateTime)
    {
        System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
        return dateTime;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (txtTemp.Text != "")
        {
            BindGVNotes(Convert.ToInt32(txtTemp.Text.Trim()));
        }
        else
        {
            GVNotes.DataSource = null;
            GVNotes.DataBind();
        }

    }

    public void BindGVNotes(Int32 EmpId)
    {
        try
        {
            if (EmpId != 0)
            {
                DataTable dt = new DataTable();
                BaseC.Dashboard dsh = new BaseC.Dashboard();
                dt = dsh.getNotesForProvider(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(EmpId), Convert.ToInt32(Session["FacilityID"]));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        GVNotes.DataSource = dt;
                        GVNotes.DataBind();
                    }
                }
                else
                {
                    GVNotes.DataSource = null;
                    GVNotes.DataBind();
                }
            }
            else
            {
                GVNotes.DataSource = null;
                GVNotes.DataBind();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkName_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient objPatient = new BaseC.Patient(sConString);
            string[] strvalue;
            LinkButton lnkPatient = sender as LinkButton;
            strvalue = lnkPatient.CommandArgument.Split('|');

            //Session["ModuleId"] = 3;
            int i = 0;
            DataSet ds = (DataSet)Session["ModuleData"];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (common.myStr(dr["ModuleName"]) == "EHR")
                {
                    Session["ModuleId"] = i;
                }
                i++;
            }
            Session["Gender"] = strvalue[2];
            Session["EncounterId"] = strvalue[3];
            Session["FacilityId"] = strvalue[4];
            Session["DoctorID"] = strvalue[5];
            Session["RegistrationID"] = strvalue[6];
            Session["AppointmentID"] = strvalue[7];
            ViewState["FormID"] = (strvalue.Length > 8) ? strvalue[8] : "";
            if (ViewState["FormID"].ToString() != "")
            {
                Session["formId"] = ViewState["FormID"];
            }
            else
            {
                Int32 intFormId = objPatient.GetPatientDefaultFormId(Convert.ToInt32(Session["encounterid"]), Convert.ToInt16(Session["HospitalLocationID"]));
                if (intFormId > 0)
                {
                    Session["formId"] = Convert.ToString(intFormId);
                }
            }

            Response.Redirect("/Editor/WordProcessor.aspx?ifId=1&Mpg=P130", false);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GVNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVNotes.PageIndex = e.NewPageIndex;
        BindGVNotes(Convert.ToInt32(txtTemp.Text.Trim()));
    }


}
