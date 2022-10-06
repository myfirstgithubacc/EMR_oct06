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
using Telerik.Web.UI;


public partial class EMRBILLING_Popup_UnacknowledgedServices : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //ddlFilter_OnSelectedIndexChanged(null, null);
            BindgvUnacknowledgedServices();
            BindgvIPPharmacyStore();
            BindgvNonDrugOrder();
        }
    }
    private void BindgvNonDrugOrder()
    {
        BaseC.ICM objICM = new BaseC.ICM(sConString);
        DataSet ds = new DataSet();
        ds = objICM.NonDrugOrderForWard((common.myInt(Session["HospitalLocationID"])), common.myInt(Session["FacilityId"]),
            common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvNonDrugOrder.DataSource = ds.Tables[0];
            gvNonDrugOrder.DataBind();

        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvNonDrugOrder.DataSource = ds.Tables[0];
            gvNonDrugOrder.DataBind();
            gvNonDrugOrder.Rows[0].Visible = false;
        }
    }
    private void BindgvIPPharmacyStore()
    {
        int EncounterId = common.myInt(Request.QueryString["EncId"].ToString());
        string EncounterNo = common.myStr(Request.QueryString["EncounterNo"].ToString());
        string storeId = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);
        DataSet dsForIPPharmacyStore = new DataSet();
        dsForIPPharmacyStore = getIPPatientRequest(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), EncounterNo, common.myInt(storeId), "C");

        if (dsForIPPharmacyStore.Tables[0].Rows.Count > 0)
        {
            gvIPPharmacyStore.DataSource = dsForIPPharmacyStore.Tables[0];
            gvIPPharmacyStore.DataBind();
        }
        else
        {
            dsForIPPharmacyStore.Tables[0].Rows.Add(dsForIPPharmacyStore.Tables[0].NewRow());
            gvIPPharmacyStore.DataSource = dsForIPPharmacyStore.Tables[0];
            gvIPPharmacyStore.DataBind();
            gvIPPharmacyStore.Rows[0].Visible = false;
        }
    }
    private void BindgvUnacknowledgedServices()
    {
        int EncounterId = common.myInt(Request.QueryString["EncId"].ToString());
        string EncounterNo = common.myStr(Request.QueryString["EncounterNo"].ToString());
        DataSet ds = new DataSet();
        if (common.myStr(ddlFilter.SelectedValue) != "")
        {
            string serviceType = string.Empty;
            if (ddlFilter.SelectedValue.Equals("0"))
                serviceType = string.Empty;
            else if (ddlFilter.SelectedValue.Equals("1"))
                serviceType = "G";
            else if (ddlFilter.SelectedValue.Equals("2"))
                serviceType = "X";
            ds = getUnacknowledgeServices(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), EncounterId, serviceType);
            string storeId = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvUnacknowledgedServices.DataSource = ds;
                gvUnacknowledgedServices.DataBind();
                lblPatientName.Text = (ds.Tables[0].Rows[0]["PatientName"]).ToString();
                lblRegNo.Text = (ds.Tables[0].Rows[0]["RegistrationNo"]).ToString();
                lblEncounterNo.Text = (ds.Tables[0].Rows[0]["EncounterNo"]).ToString();
                lblBedNo.Text = (ds.Tables[0].Rows[0]["BedNo"]).ToString();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvUnacknowledgedServices.DataSource = ds.Tables[0];
                gvUnacknowledgedServices.DataBind();
                gvUnacknowledgedServices.Rows[0].Visible = false;
            }
            ds.Dispose();
        }
    }
    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        //    try
        //    {
        //        BaseC.EMRBilling objval = new BaseC.EMRBilling(sConString);
        //        int EncounterId = common.myInt(Request.QueryString["EncId"].ToString());
        //        string EncounterNo = common.myStr(Request.QueryString["EncounterNo"].ToString()); 
        //        //int RegistrationId = common.myInt(Request.QueryString["RegId"].ToString());
        //        DataSet ds = new DataSet();

        //        if (common.myStr(ddlFilter.SelectedValue) != "")
        //        {
        //            string serviceType=string.Empty;
        //            if (ddlFilter.SelectedValue.Equals("0"))
        //                serviceType = string.Empty;
        //            else if (ddlFilter.SelectedValue.Equals("1"))
        //                serviceType = "G";
        //            else if (ddlFilter.SelectedValue.Equals("2"))
        //                serviceType = "X";

        //            ds = getUnacknowledgeServices(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), EncounterId, serviceType);

        //            //BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        //            //DataSet ds = new DataSet();
        //            string storeId = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);

        //            DataSet dsForIPPharmacyStore = new DataSet();
        //            dsForIPPharmacyStore = getIPPatientRequest(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), EncounterNo, common.myInt(storeId),"C");


        //            //lblCremrk.Text = "";
        //            //txtRemark.Visible = false;
        //            //btnCancelUnPerform.Visible = false;
        //            //if (common.myStr(ddlFilter.SelectedIndex) == "0")
        //            //{
        //            //    if (ds.Tables[0].Rows.Count > 0)
        //            //    {
        //            //        if (ddlFilter.SelectedIndex == 0)
        //            //        {
        //            //            lblCremrk.Text = "Cancel all service ";
        //            //            txtRemark.Visible = true;
        //            //            btnCancelUnPerform.Visible = true;
        //            //        }
        //            //    }
        //            //}
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                gvUnacknowledgedServices.DataSource = ds;
        //                gvUnacknowledgedServices.DataBind();
        //                lblPatientName.Text = (ds.Tables[0].Rows[0]["PatientName"]).ToString();
        //                lblRegNo.Text = (ds.Tables[0].Rows[0]["RegistrationNo"]).ToString();
        //                lblEncounterNo.Text = (ds.Tables[0].Rows[0]["EncounterNo"]).ToString();
        //                lblBedNo.Text = (ds.Tables[0].Rows[0]["BedNo"]).ToString();
        //            }
        //            else
        //            {
        //                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
        //                gvUnacknowledgedServices.DataSource = ds.Tables[0];
        //                gvUnacknowledgedServices.DataBind();
        //                gvUnacknowledgedServices.Rows[0].Visible = false;
        //                //gvUnacknowledgedServices.DataSource = null;
        //                //gvUnacknowledgedServices.DataBind();
        //            }
        //            if (dsForIPPharmacyStore.Tables[0].Rows.Count > 0)
        //            {
        //                gvIPPharmacyStore.DataSource = dsForIPPharmacyStore.Tables[0];
        //                gvIPPharmacyStore.DataBind();
        //            }
        //            else
        //            {
        //                dsForIPPharmacyStore.Tables[0].Rows.Add(dsForIPPharmacyStore.Tables[0].NewRow());
        //                gvIPPharmacyStore.DataSource = dsForIPPharmacyStore.Tables[0];
        //                gvIPPharmacyStore.DataBind();
        //                gvIPPharmacyStore.Rows[0].Visible = false;

        //                //gvIPPharmacyStore.DataSource = null;
        //                //gvIPPharmacyStore.DataBind();
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        // lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //        objException.HandleException(Ex);
        //    }
    }
    protected void ddlFilter_OnSelectedIndexChanged(object sender, EventArgs e)
    {
       // btnFilter_OnClick(null, null);
    }

    protected void gvIPPharmacyStore_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvIPPharmacyStore.PageIndex = e.NewPageIndex;
        BindgvIPPharmacyStore();
    }
    protected void gvUnacknowledgedServices_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUnacknowledgedServices.PageIndex = e.NewPageIndex;
        BindgvUnacknowledgedServices();
    }


    protected void gvNonDrugOrder_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNonDrugOrder.PageIndex = e.NewPageIndex;
        BindgvNonDrugOrder();
    }

    //It should be in BaseC - Wardmanagement
    public DataSet getUnacknowledgeServices(int iHospID, int iLoginFacilityId, int EncounterId,string type)
    {
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@inyHospitalLocationId", iHospID);
        HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
        HshIn.Add("@EncounterId", EncounterId);
        HshIn.Add("@type", type);
        
        DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIPUnperformedServicesForWard", HshIn);


        return ds;
    }
   
    public DataSet getIPPatientRequest(int HospId, int FacilityId, string EncounterNo, int StoreId, string PendingStatus)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        
        HshIn.Add("@inyHospitalLocationId", HospId);
        HshIn.Add("@intFacilityId", FacilityId);        
        HshIn.Add("@chvEncounterNo", EncounterNo);
        HshIn.Add("@intStoreId", StoreId);
        HshIn.Add("@chrPendingStatus", PendingStatus);        
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPPatientRequest", HshIn, HshOut);
        return ds;
    }

}
