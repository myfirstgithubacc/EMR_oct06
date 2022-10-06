using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;

public partial class BloodBank_SetupMaster_BloodTransfusionReactionDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.clsBb objBb;
        objBb = new BaseC.clsBb(sConString);
        DataTable table;
        table = objBb.GetPatientTransfusionList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1, common.myInt(Request.QueryString["Regid"]), common.myStr(Request.QueryString["RegNo"]), common.myInt(Request.QueryString["EncId"]), common.myInt(1), 1).Tables[0]; 
        ds = GetPatientTransfusionDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(Request.QueryString["EncounterNo"]), common.myStr(Request.QueryString["RegNo"]), common.myInt(table.Rows[0]["TransfusionId"]));

        gvTransfusionReactionDetails.DataSource = ds.Tables[1];
        gvTransfusionReactionDetails.DataBind();
    }

    public DataSet GetPatientTransfusionDetails(int HospitalLocationId, int FacilityId, string EncounterNo, string RegistrationNo, int TransfusionID)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@intTransfusionID", TransfusionID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetTransfusionDetails", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }
}