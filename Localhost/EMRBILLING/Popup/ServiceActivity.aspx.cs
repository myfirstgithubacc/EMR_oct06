using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;
using Telerik.Web.UI;
using BaseC;
using System.Collections;

public partial class EMRBILLING_Popup_ServiceActivity : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        FillServiceactivity();
    }

    protected void rdoRegEnc_TextChanged(object sender, EventArgs e)
    {

    }
    public void FillServiceactivity()
    {
        BaseC.EMRBilling objval = new BaseC.EMRBilling(sConString);
        DataSet ds = new DataSet();
        try
        {
            int BillId =0;
            int EncounterId = common.myInt(Request.QueryString["encId"].ToString());
            int RegID = common.myInt(Request.QueryString["RegID"].ToString());
            int x = 0;
            if(rdoRegEnc.SelectedValue=="0")
            {
                x = 0;
            }
            else
            {
                x = 1;
            }
            ds = objval.getserviceactivity(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), RegID, EncounterId, BillId,x, "UspGetIPServiceOrderCount");
            if (ds.Tables.Count > 0)
            {
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                    gvServiceActivity.DataSource = ds.Tables[0].DefaultView;
                    gvServiceActivity.DataBind();
               // }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { objval = null; ds.Dispose(); }
    }
}