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
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;
using Telerik.Web.UI;


public partial class Appointment_OnlineAppointmentRequest : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsEMRBilling objVal;
    clsExceptionLog objException = new clsExceptionLog();

    private enum GridEncounter : byte
    {
        Select = 2,
        OPIP = 3,
        RegistrationNo = 4,
        EncounterNo = 5,
        Name = 6,
        AgeGender = 7,
        DoctorName = 8,
        CurrentBedNo = 9,
        EncDate = 10,
        DischargeStatus = 11,
        CompanyName = 13,
        PhoneHome = 14,
        MobileNo = 15,
        DOB = 16,
        Address = 17,
        REGID = 18,
        ENCID = 19,
        CompanyCode = 20,
        InsuranceCode = 21,
        CardId = 22,
        RowNo = 23,
        EncounterStatus = 12
    }

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
            dtpFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            dtpFromDate.SelectedDate=DateTime.Now.AddDays(-2);
            dtpToDate.SelectedDate = DateTime.Now;
            hdnRegistrationId.Value = "";
            hdnRegistrationNo.Value = "";
            hdnPPAppointmentId.Value = "";
            PopulateDoctor();
            bindData("F", 0);
            if (common.myStr(Request.QueryString["useFor"]) == "Display")
            {
               gvEncounter.Columns[0].Visible = false;
            }
        }
    }
    protected void btnfilter_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        bindData("F", 0);       
    } 
    private void PopulateDoctor()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            Hashtable HashIn = new Hashtable();
            HashIn.Add("@HospitalLocationId", Session["HospitalLocationID"]);
            HashIn.Add("@intFacilityId", Session["FacilityId"]);
            HashIn.Add("@intSpecialisationId", common.myInt(Request.QueryString[""]));
            DataSet dt = dl.FillDataSet(CommandType.StoredProcedure, "uspgetdoctorlist", HashIn);
            ddlProvider.DataSource = dt.Tables[0];
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataBind();

            ddlProvider.Items.Insert(0, new RadComboBoxItem(" All ", "0"));
            ddlProvider.SelectedValue = common.myStr(Request.QueryString["doctorId"]);           
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }      
        finally
        {
            dl = null;
        }
    }
    private void bindData(string RecordButton, int RowNo)
    {
        try
        {         
            objVal = new BaseC.clsEMRBilling(sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            HshIn.Add("@intHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            HshIn.Add("@intDoctorId", ddlProvider.SelectedValue);
            HshIn.Add("@dtFromDate", common.myDate(dtpFromDate.SelectedDate).ToString("yyyy-MM-dd"));
            HshIn.Add("@dtToDate", common.myDate(dtpToDate.SelectedDate).ToString("yyyy-MM-dd"));
            HshIn.Add("@inyAppointmentConfirmed", common.myInt(ddlStatus.SelectedValue));
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);            
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWebGetAppointmentRequests", HshIn, HshOut);
            gvEncounter.DataSource = ds;
            gvEncounter.DataBind();             
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvEncounter_OnItemDataBound(object sender, GridItemEventArgs e)
    {         
        if (e.Item is GridDataItem)
        {
            HiddenField hdnAppointmentConfirmed = (HiddenField)e.Item.FindControl("hdnAppointmentConfirmed");
            LinkButton IbtnSelect = (LinkButton)e.Item.FindControl("IbtnSelect");
            if (common.myInt(hdnAppointmentConfirmed.Value) > 0)
            {
                e.Item.BackColor = System.Drawing.Color.FromName("#F1DCFF");
            }
            
        }
    }
    protected void gvEncounter_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvEncounter.CurrentPageIndex = e.NewPageIndex;
        bindData("F", 0);
    }   
    protected void gvEncounter_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                if (common.myInt(((Label)e.Item.FindControl("lblREGID")).Text) > 0)
                {
                    hdnRegistrationId.Value = common.myStr(((Label)e.Item.FindControl("lblREGID")).Text);
                    hdnRegistrationNo.Value = common.myStr(((Label)e.Item.FindControl("lblRegistrationNo")).Text);
                    hdnPPAppointmentId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnPPAppointmentId")).Value);                    
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }  
    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        bindData("F", 0);
    }   
}
