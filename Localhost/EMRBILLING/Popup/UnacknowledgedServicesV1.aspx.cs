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
            BindControl();
        }
    }

    protected void BindControl()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            Hashtable hsIn = new Hashtable();
            hsIn.Add("@intHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            hsIn.Add("@insFacilityId", common.myInt(Session["FacilityId"]));

            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetInvoiceDisplayActivities", hsIn);
            if (ds.Tables.Count > 0)
            {
                ddlFilter.DataSource = ds;
                ddlFilter.DataTextField = "DisplayName";
                ddlFilter.DataValueField = "ProcedureName";
                ddlFilter.DataBind();
                if (common.myStr(Request.QueryString["Type"]) == "Unperformed")
                {
                    //ddlFilter.SelectedIndex = 2;
                    ddlFilter.SelectedValue = "uspGetIPUnperformedServices";
                    ddlFilter.Enabled = false;
                    btnFilter.Visible = false;
                }
                else
                {
                    ddlFilter.SelectedIndex = 0;
                    ddlFilter.Enabled = true;
                    btnFilter.Visible = true;
                }
                ddlFilter_OnSelectedIndexChanged(null, null);
            }
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
        BaseC.EMRBilling objval = new BaseC.EMRBilling(sConString);
        BaseC.clsBb objBb = new BaseC.clsBb(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (!common.myStr(ddlFilter.SelectedValue).Equals(""))
            {
                if (common.myStr(ddlFilter.SelectedValue).Equals("uspBBGetComponentRequisition"))
                {
                    //ds = objval.getPatientTransefers(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["encId"]), common.myStr(ddlFilter.SelectedValue), 0);
                    ds = objBb.GetComponentRequisition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, 0, "", 1, common.myInt(Request.QueryString["encId"]), "", "", "", "", "RUK", "A");

                    DataTable tempDT = new DataTable();
                    tempDT = ds.Tables[0].DefaultView.ToTable(true, "RequisitionNo", "RequestType", "RequestDate", "PatientBloodGroup", "ConsentTakenBy");
                    lblCremrk.Text = "";
                    txtRemark.Visible = false;
                    btnCancelUnPerform.Visible = false;
                    gvUnacknowledgedServices.DataSource = tempDT;
                    gvUnacknowledgedServices.DataBind();
                    objBb = null;
                }
                else if (common.myStr(ddlFilter.SelectedValue).Equals("USPGetPhrDocumentDetails"))
                {
                    string sEncounterNo = string.Empty, sFromDate = string.Empty, sToDate = string.Empty, sRegNo = string.Empty;
                    int iSaleSetUp = 203;// "SELECT StatusId FROM StatusMaster WHERE StatusType = 'INVENTORYTRANS' AND Code = 'IP-RET'"
                    sEncounterNo = common.myStr(Request.QueryString["EncounterNo"]);
                    sRegNo = common.myStr(Request.QueryString["RegNo"]);
                    sToDate = DateTime.Now.ToString("yyyy-MM-dd");
                    sFromDate = common.myStr(Convert.ToDateTime(Request.QueryString["AdmissionDate"]).ToString("yyyy-MM-dd"));
                    BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

                    ds = objPharmacy.GetDocumentDetails(common.myInt(Session["HospitalLocationId"]), 0, iSaleSetUp, "", "", sRegNo, "", sEncounterNo, "",
                    common.myDate(sFromDate), common.myDate(sToDate), "R", common.myInt(Session["UserID"]), common.myInt(Session["FacilityId"]), "I", "O");

                    DataTable tempDT = new DataTable();
                    tempDT = ds.Tables[0].DefaultView.ToTable(true, "IssueNo", "IssueDate", "LoginStoreName", "NetAmount", "CurrentBedNo", "EncodedBy",	"EncodedDate");
                    lblCremrk.Text = "";
                    txtRemark.Visible = false;
                    btnCancelUnPerform.Visible = false;
                    gvUnacknowledgedServices.DataSource = tempDT;
                    gvUnacknowledgedServices.DataBind();
                    objPharmacy = null;
                }
                else
                {
                    ds = objval.getPatientTransefers(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["encId"]), common.myStr(ddlFilter.SelectedValue), 0);

                    lblCremrk.Text = "";
                    txtRemark.Visible = false;
                    btnCancelUnPerform.Visible = false;
                    if (common.myStr(ddlFilter.SelectedIndex) == "0")
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ddlFilter.SelectedIndex == 0)
                            {
                                lblCremrk.Text = "Cancel all service ";
                                txtRemark.Visible = true;
                                btnCancelUnPerform.Visible = true;
                            }
                        }
                    }
                    gvUnacknowledgedServices.DataSource = ds;
                    gvUnacknowledgedServices.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objval = null; ds.Dispose(); }
    }
   
    protected void gvUnacknowledgedServices_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {   

            if (common.myStr(e.Row.RowType).Equals(DataControlRowType.DataRow))
            {
                if (common.myStr(ddlFilter.SelectedValue).ToUpper() == "USPGETPATIENTTRANSFERDETAILS")
                {
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[11].Visible = false;
                }
                
            }
            if (common.myStr(e.Row.RowType).Equals(DataControlRowType.Header))
            {
                if (common.myStr(ddlFilter.SelectedValue).ToUpper() == "USPGETPATIENTTRANSFERDETAILS")
                {
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[11].Visible = false;
                }
               
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void ddlFilter_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btnFilter_OnClick(null, null);
    }
    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            if (common.myInt(ddlFilter.SelectedIndex).Equals(0))
            {
                if (!common.myStr(txtRemark.Text).Equals(""))
                {
                    Hashtable hsIn = new Hashtable();
                    Hashtable hsOut = new Hashtable();
                    hsIn.Add("inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
                    hsIn.Add("intFacilityId", common.myInt(Session["FacilityId"]));
                    hsIn.Add("intRegistrationId", common.myInt(Request.QueryString["RegId"].ToString()));
                    hsIn.Add("intEncounterId", common.myInt(common.myInt(Request.QueryString["encId"])));
                    hsIn.Add("chvRemarks", common.myStr(txtRemark.Text));
                    hsIn.Add("intEncodedBy", common.myInt(Session["UserId"]));
                    hsOut.Add("chvErrorStatus", SqlDbType.VarChar);
                    hsOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelUnperformIPServices", hsIn, hsOut);
                    btnFilter_OnClick(null, null);
                    if (hsOut["chvErrorStatus"].ToString().Contains("Succeeded"))
                    {
                        txtRemark.Text = "";
                        Alert.ShowAjaxMsg("Sucessfully cancelled all unperformed services...", Page.Page);
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Please enter cancel remark!...", Page.Page);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { dl = null; }
    }
}
