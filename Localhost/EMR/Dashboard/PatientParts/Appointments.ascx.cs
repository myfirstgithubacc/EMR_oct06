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

public partial class EMR_Dashboard_Parts_Appointments : WebPartBase
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    
    DL_Funs fun = new DL_Funs();
    DataSet dsApp = new DataSet();

    public EMR_Dashboard_Parts_Appointments()
    {
        this.Title = "Encounters";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0")
        {
            BindEncounter();
        }
        else
        {

        }

    }

    private void BindEncounter()
    {
        try
        {
            Dashboard dsh = new Dashboard();
            dsApp = dsh.getDashBoardValue(Convert.ToInt16(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]),
                                        hdnFromDate.Text, hdnToDate.Text, hdnDateVale.Text, hdnEncounterNumber.Text, common.myInt(Session["FacilityID"]), 
                                        "UspSearchEncounter");
            if (dsApp.Tables.Count >0)
            {
                GDAppointment.DataSource = dsApp;
                GDAppointment.DataBind();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lbtnName_Click(object sender, EventArgs e)
    {
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        string[] strvalue;
        LinkButton lnkPatient1 = sender as LinkButton;

        strvalue = lnkPatient1.CommandArgument.Split('|');

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
        Session["FacilityID"] = strvalue[4];
        Session["DoctorID"] = strvalue[5];
        Session["RegistrationID"] = strvalue[6];
        Session["AppointmentID"] = strvalue[7];
        Int32 intFormId = objPatient.GetPatientDefaultFormId(Convert.ToInt32(Session["encounterid"]), Convert.ToInt16(Session["HospitalLocationID"]));
        if (intFormId > 0)
        {
            Session["formId"] = Convert.ToString(intFormId);
        }
        Response.Redirect("/editor/Wordprocessor.aspx?ifId=1&Mpg=P130", false);
    }

    protected void GDAppointment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.GDAppointment, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
        }
    }

    protected void GDAppointment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDAppointment.PageIndex = e.NewPageIndex;
        BindEncounter();
    }

}
