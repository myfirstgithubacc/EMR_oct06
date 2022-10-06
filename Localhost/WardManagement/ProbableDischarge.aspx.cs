using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EMRBILLING_Popup_UnacknowledgedServicesV1 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dtpEod.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpEod.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpEod.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            dtpEod.MinDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            BindControl();
        }
    }

    protected void BindControl()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            Hashtable hsIn = new Hashtable();
            hsIn.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
            hsIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hsIn.Add("@ExpectedDateOfDischarge", common.myDate(dtpEod.SelectedDate));
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspWardProbableDischargeList", hsIn);
            if (ds.Tables.Count > 0)
            {
                gvProbableDischargeDate.DataSource = ds;
                gvProbableDischargeDate.DataBind();
                lblNoOfRows.Text = "Total Record(s): " + ds.Tables[0].Rows.Count.ToString();
            }
            else { lblNoOfRows.Visible = false; }
            ds.Clear();
            ds.Dispose();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { dl = null; }
    }

    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        BindControl();
    }
  
}
