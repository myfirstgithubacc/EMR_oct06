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

public partial class EMR_Allergy_PatientAllergy : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RetrievePatientAllergies();
        }
    }

    protected void RetrievePatientAllergies()
    {
        try
        {
            DataSet ds = new DataSet();
            if (Session["RegistrationID"] != null)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshInput = new Hashtable();
                //hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
                hshInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientAllergies", hshInput);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataView dvAllergy = new DataView(ds.Tables[0]);
                        dvAllergy.RowFilter = "AllergyType='Drug'";
                        DataTable dtDrugAllergy = dvAllergy.ToTable();
                        if (dtDrugAllergy.Rows.Count > 0)
                        {
                            lstDrugAllergy.DataSource = dtDrugAllergy;
                            lstDrugAllergy.DataTextField = "AllergyName";
                            lstDrugAllergy.DataValueField = "Generic_Id";
                            lstDrugAllergy.DataBind();
                        }
                        dvAllergy.RowFilter = "AllergyType not in('Drug')";
                        DataTable dtOtherAllergy = dvAllergy.ToTable();
                        if (dtOtherAllergy.Rows.Count > 0)
                        {
                            lstOtherAllergy.DataSource = dtOtherAllergy;
                            lstOtherAllergy.DataTextField = "AllergyName";
                            lstOtherAllergy.DataValueField = "AllergyId";
                            lstOtherAllergy.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }

}
