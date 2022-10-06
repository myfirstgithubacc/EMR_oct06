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
using Telerik.Web.UI;
using System.Xml.Linq;
using BaseC;
public partial class EMR_Dashboard_Parts_Medications : WebPartBase
{
    Hashtable hsTb;
    DL_Funs fun = new DL_Funs();
    clsExceptionLog objException = new clsExceptionLog();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    public EMR_Dashboard_Parts_Medications()
    {
        this.Title = "Prescription";
    }

    public string ToDate
    {
        get
        {
            return hdnToDate.Value;
        }
        set
        {
            hdnToDate.Value = value;
        }
    }

    public string FromDate
    {
        get
        {
            return hdnFromDate.Value;
        }
        set
        {
            hdnFromDate.Value = value;
        }
    }

    public string EncounterNumber
    {
        get
        {
            return hdnEncounterNumber.Value;
        }
        set
        {
            hdnEncounterNumber.Value = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            EncounterNumber = common.myStr(HttpContext.Current.Session["EncounterId"]);
            BindMedications();
        }
    }

    public void BindMedications()
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet dsApp = new DataSet();
        try
        {
            GDMedications.DataSource = null;
            GDMedications.DataBind();
            int EncNO = hdnEncounterNumber.Value == "" ? common.myInt(HttpContext.Current.Session["EncounterId"]) : common.myInt(hdnEncounterNumber.Value);

            string OPIP = common.myStr(HttpContext.Current.Session["OPIP"]);
            if (OPIP == "I")
            {
                dsApp = objPharmacy.getPreviousMedicines(common.myInt(HttpContext.Current.Session["HospitalLocationID"]),
                    common.myInt(HttpContext.Current.Session["FacilityId"]), EncNO);
            }
            else
            {
                dsApp = objEMR.getOPMedicines(common.myInt(HttpContext.Current.Session["HospitalLocationID"]), common.myInt(HttpContext.Current.Session["FacilityId"]),
                     EncNO, common.myInt(HttpContext.Current.Session["RegistrationId"]), 0, 0, "P");
            }

            if (dsApp.Tables[1].Rows.Count > 0)
            {
                GDMedications.DataSource = dsApp.Tables[1];
                GDMedications.DataBind();
            }
            else
            {
                DataRow dr = dsApp.Tables[1].NewRow();
                dsApp.Tables[1].Rows.Add(dr);
                GDMedications.DataSource = dsApp.Tables[1];
                GDMedications.DataBind();
                GDMedications.Visible = false;
                lblMsg.Visible = true;
                lblMsg.Text = "&nbsp;None";
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPharmacy = null;
            objEMR = null;
            dsApp.Dispose();
        }
    }

    public void BindMedicationsByEncounter()
    {
        try
        {
            if (hdnEncounterNumber.Value != "")
            {
                //Dashboard dsh = new Dashboard();
                //GDMedications.DataSource = dsh.getMedication(Convert.ToInt16(Session["HospitalLocationID"]), hdnEncounterNumber.Value);
                //GDMedications.DataBind();

                BindMedications();
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GDMedications_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
    }

    protected void GDMedications_onpageindexchanging(object sender, GridViewPageEventArgs e)
    {
        GDMedications.PageIndex = e.NewPageIndex;
        BindMedicationsByEncounter();
    }

    protected void lnkAddMedication_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/EMR/Medication/Medication.aspx?Mpg=" + "C153", false);
    }
    public void setLink()
    {

        //RadWindow1.NavigateUrl = "/EMR/Medication/MedicineOrder.aspx?Regid=" + common.myInt(Session["RegistrationID"])
        //    + "&RegNo=" + common.myInt(Session["Regno"])
        //    + "&EncId=" + common.myInt(Session["encounterid"])
        //    + "&EncNo=" + common.myInt(Session["Encno"]);

        ////RadWindow1.NavigateUrl = "/emr/medication/Medication1.aspx?Mpg=P153";

        ////RadWindow1.NavigateUrl = "/WardManagement/Medication.aspx?Regno=" + lblregno.Text.Trim() + "&Encno=" + lblencno.Text.Trim() + "";
        //RadWindow1.Height = 580;
        //RadWindow1.Width = 930;
        //RadWindow1.Top = 10;
        //RadWindow1.Left = 10;
        //RadWindow1.OnClientClose = "SearchPatientOnClientClose";//
        //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindow1.Modal = true;
        //RadWindow1.VisibleStatusbar = false;
        //RadWindow1.InitialBehavior = WindowBehaviors.Maximize;

    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        BindMedicationsByEncounter();
    }
    protected void lnkAddMedication_OnClick(object sender, EventArgs e)
    {
        setLink();
    }
}