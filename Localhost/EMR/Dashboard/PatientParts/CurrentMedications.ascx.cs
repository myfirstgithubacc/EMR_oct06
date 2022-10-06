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
public partial class EMR_Dashboard_Parts_CurrentMedications : WebPartBase
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    Hashtable hsTb;
    DL_Funs fun = new DL_Funs();
    clsExceptionLog objException = new clsExceptionLog();
    
    

    public EMR_Dashboard_Parts_CurrentMedications()
    {
        this.Title = "Current Medication";
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            DAL.DAL DL = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsTb = new Hashtable();
            hsTb.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsTb.Add("@FacilityID", common.myInt(Session["FacilityID"]));
            hsTb.Add("@EncounterId", common.myInt(Session["EncounterId"]));
            string sCode = (string)DL.ExecuteScalar(CommandType.Text, "Select s.Code FROM Encounter E WITH(NOLOCK) inner join GetStatus(@HospitalLocationId,'Encounter') S on e.StatusId = s.StatusId WHERE E.Id = @EncounterId AND E.HospitalLocationId = @HospitalLocationId AND E.FacilityID = @FacilityID and e.active = 1", hsTb);

            if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0" && (sCode == "O" || sCode == "MD"))
            {
                hdnEncounterNumber.Text = Convert.ToString(Session["EncounterId"]);
                lnkAddMedication.Enabled = true;
            }
            else
            {
                lnkAddMedication.Enabled = false;
            }
            if (common.myStr(HttpContext.Current.Session["ModuleIdValue"]) == "41")
            {
                lnkAddMedication.Enabled = false;
            }
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
            int EncNO = hdnEncounterNumber.Text == "" ? common.myInt(HttpContext.Current.Session["EncounterId"]) : common.myInt(hdnEncounterNumber.Text);
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

    protected void GDMedications_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDMedications.PageIndex = e.NewPageIndex;
        BindMedications();
    }

    protected void lnkAddMedication_Click(object sender, EventArgs e)
    {
        setLink();
    }
    public void setLink()
    {

        //RadWindow1.NavigateUrl = "~/EMR/Medication/Medication.aspx?From=POPUP";
        ////RadWindow1.NavigateUrl = "/WardManagement/Medication.aspx?Regno=" + lblregno.Text.Trim() + "&Encno=" + lblencno.Text.Trim() + "";
        //RadWindow1.Height = 560;
        //RadWindow1.Width = 930;
        //RadWindow1.Top = 10;
        //RadWindow1.Left = 10;
        //RadWindow1.OnClientClose = "SearchPatientOnClientClose";//
        //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindow1.Modal = true;
        //RadWindow1.VisibleStatusbar = false;

    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        BindMedications();
    }
    protected void lnkDiagnosis_OnClick(object sender, EventArgs e)
    {
        setLink();
    }

}