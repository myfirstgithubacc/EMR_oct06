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
using BaseC;

public partial class EMR_Dashboard_Parts_Notes : WebPartBase
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    public EMR_Dashboard_Parts_Notes()
    {
        this.Title = "Notes";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindNotes();
        if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0")
        {
            lnkNotesRedirect.Enabled = true;
        }
        else
        {
            lnkNotesRedirect.Enabled = false;
        }
        if (common.myStr(HttpContext.Current.Session["ModuleIdValue"]) == "41")
        {
            lnkNotesRedirect.Enabled = false;
        }
    }

    protected void gvNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNotes.PageIndex = e.NewPageIndex;
        BindNotes();
    }

    private void BindNotes()
    {
        try
        {
            Dashboard dsh = new Dashboard();

            DataSet dsApp = new DataSet();
            dsApp = dsh.getDashBoardValue(Convert.ToInt16(Session["HospitalLocationID"]), common.myInt(Session["registrationid"]), hdnFromDate.Text, hdnToDate.Text, hdnDateVale.Text, hdnEncounterNumber.Text, common.myInt(Session["FacilityID"]), "UspSearchForms");
            if (dsApp.Tables[0].Rows.Count > 0)
            {
                gvNotes.DataSource = dsApp;
                gvNotes.DataBind();
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

        if (strvalue.Length > 8)
        {
            ViewState["FormID"] = strvalue[8];
        }
        if (common.myInt(ViewState["FormID"]) > 0)
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
    public void setLink()
    {
        if (common.myStr(Session["FromPage"]) == "POPUP")
        {
            RadWindow1.NavigateUrl = "~/EMR/Templates/NoteSelect.aspx?From=POPUP";
            RadWindow1.Height = 550;
            RadWindow1.Width = 800;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "SearchPatientOnClientClose";//
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        else
        {
            Response.Redirect("~/EMR/Templates/NoteSelect.aspx", false);

        }
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        BindNotes();
    }
    protected void lnkNotesRedirect_OnClick(object sender, EventArgs e)
    {
        setLink();
    }
}