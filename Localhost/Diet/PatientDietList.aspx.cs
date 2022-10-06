using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Diet_PatientDietList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    private static bool status = false;

    //protected void Page_PreInit(object sender, System.EventArgs e)
    //{
    //    Page.Theme = "DefaultControls";
     
    //     if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
    //    {
    //        this.MasterPageFile = "~/Include/Master/BlankMaster.master";
    //    }
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                dtpfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
                dtpfromdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
                BindWard();
                BindDietOrder();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindDietOrder();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        BindWard();
        BindDietOrder();
    }

    protected void gvPatientDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            Label lblMealCardStatus = (Label)item.FindControl("lblMealCardStatus");
            LinkButton lnkIpNo = (LinkButton)item.FindControl("lnkIpNo");
            HiddenField hndEMRRequestId = (HiddenField)item.FindControl("hndEMRRequestId");
            if (common.myInt(hndEMRRequestId.Value) > 0)
            {
                item.BackColor = System.Drawing.Color.Aqua;
            }
            lnkIpNo.Enabled = false;
            lnkIpNo.Attributes.Add("style", "text-decoration:none;");
        }
    }


    void BindWard()
    {
        DataSet ds = new DataSet();
        BaseC.ATD objAtd = new BaseC.ATD(sConString);
        try
        {
            ds = objAtd.getWardStationMaster(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitallocationId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlWard.DataSource = ds;
                ddlWard.DataTextField = "StationName";
                ddlWard.DataValueField = "WardStationId";
                ddlWard.DataBind();
            }
            ddlWard.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlWard.Items[0].Value = "0";
            ds.Clear();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { ds.Dispose(); objAtd = null; }
    }

    void BindDietOrder()
    {
        BaseC.Diet objbC = new BaseC.Diet(sConString);
        DataSet ds = new DataSet();
        try
        {
            DateTime dtfromdate = common.myDate(dtpfromdate.SelectedDate);
            ds = objbC.GetPatientDietListWardStationWise(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(dtfromdate.ToString("yyyy/MM/dd")), common.myInt(ddlWard.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;
                gvPatientDetails.DataSource = dv.Table;
                gvPatientDetails.DataBind();
                lblCount.Text = "Patient Count : " + gvPatientDetails.Items.Count.ToString();
            }
            else
            {
                gvPatientDetails.DataSource = CreateTable();
                gvPatientDetails.DataBind();
            }
            ds.Clear();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { ds.Dispose(); objbC = null; }
    }

    protected DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("id");
        dt.Columns.Add("BedNo");
        dt.Columns.Add("RegNo");
        dt.Columns.Add("encounterno");
        dt.Columns.Add("EncounterID");
        dt.Columns.Add("PatientName");
        dt.Columns.Add("AgeGender");
        dt.Columns.Add("Diagnosis");
        dt.Columns.Add("DietOrder");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("Height");
        dt.Columns.Add("Weight");
        dt.Columns.Add("CategoryName");
        dt.Columns.Add("EnteredBy");
        dt.Columns.Add("EnteredDate");
        dt.Columns.Add("MealCardStatus");
        dt.Columns.Add("EMRRequestId");
        dt.Columns.Add("IsVIP");
        dt.Columns.Add("VIPNarration");
        dt.Columns.Add("DieticianRemarks");
        dt.Columns.Add("AdmissionDate");

        

        DataRow dr = dt.NewRow();
        dr["id"] = DBNull.Value;
        dr["BedNo"] = DBNull.Value;
        dr["RegNo"] = DBNull.Value;
        dr["encounterno"] = DBNull.Value;
        dr["EncounterID"] = DBNull.Value;
        dr["PatientName"] = DBNull.Value;
        dr["AgeGender"] = DBNull.Value;
        dr["Diagnosis"] = DBNull.Value;
        dr["DietOrder"] = DBNull.Value;
        dr["DoctorName"] = DBNull.Value;
        dr["Height"] = DBNull.Value;
        dr["Weight"] = DBNull.Value;
        dr["CategoryName"] = DBNull.Value;
        dr["EnteredBy"] = DBNull.Value;
        dr["EnteredDate"] = DBNull.Value;
        dr["MealCardStatus"] = DBNull.Value;
        dr["EMRRequestId"] = DBNull.Value;
        dr["IsVIP"] = DBNull.Value;
        dr["VIPNarration"] = DBNull.Value;
        dr["DieticianRemarks"] = DBNull.Value;
        dr["AdmissionDate"] = DBNull.Value;


        dt.Rows.Add(dr);
        ViewState["Servicetable"] = dt;
        return dt;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkFB.Checked)
            {
                RadWindowForNew.NavigateUrl = "/Diet/Report/PatientDietlistreport.aspx?WardStnId=" + common.myInt(ddlWard.SelectedValue) + "&FDate=" + Convert.ToDateTime(dtpfromdate.SelectedDate).Date.ToString("yyyy/MM/dd  HH:mm:ss") + "&Fb=" + "FB";

                RadWindowForNew.Height = 590;
                RadWindowForNew.Width = 980;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;

                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                RadWindowForNew.VisibleStatusbar = false;
            }
            else
            {
                lblMessage.Text = "";
                RadWindowForNew.NavigateUrl = "/Diet/Report/PatientDietlistreport.aspx?WardStnId=" + common.myInt(ddlWard.SelectedValue) + "&FDate=" + Convert.ToDateTime(dtpfromdate.SelectedDate).Date.ToString("yyyy/MM/dd  HH:mm:ss") + "";

                RadWindowForNew.Height = 590;
                RadWindowForNew.Width = 980;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;

                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                RadWindowForNew.VisibleStatusbar = false;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void lnkIPNo_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkBtn = (LinkButton)sender;


            GridDataItem Grdrow = (GridDataItem)((LinkButton)sender).NamingContainer;
            HiddenField hndEMRRequestId = (HiddenField)Grdrow.FindControl("hndEMRRequestId");
            HiddenField hdnEncId = (HiddenField)Grdrow.FindControl("hdnEncId");
            HiddenField hdnIsVIP = (HiddenField)Grdrow.FindControl("hdnIsVIP");
            HiddenField hdnVIPNarration = (HiddenField)Grdrow.FindControl("hdnVIPNarration");
            HiddenField hdnDieticianRemarks = (HiddenField)Grdrow.FindControl("hdnDieticianRemarks");

            lblMessage.Text = "";

            RadWindowForNew.NavigateUrl = "/Diet/PatientDietPlan.aspx?From=POPUP&EncNo=" + common.myStr(lnkBtn.Text) + "&BedNo=" + common.myStr(Grdrow.Cells[2].Text) + "&RequestedID=" + common.myInt(hndEMRRequestId.Value) + "&EncId=" + hdnEncId.Value + "&IsVIP=" + hdnIsVIP.Value + "&VIPNarration=" + hdnVIPNarration.Value + "&DieticianRemarks=" + hdnDieticianRemarks.Value;
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 1000;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "OnClientFindClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
}
