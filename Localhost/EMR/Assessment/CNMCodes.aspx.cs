using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EMR_Assessment_CNMCodes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkMedication_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PatientMedicationCharges.aspx", false);
    }

    protected void lnkDiagnosis_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PatientDiagnosisCharges.aspx?ID=0", false);
    }
}