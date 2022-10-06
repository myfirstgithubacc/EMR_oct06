using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Configuration;
using BaseC;

public partial class WardManagement_ServiceActivityDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ViewState["IsPrintProvisional"] = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowToPrintProvisionalResult", sConString));
            FillData();
            BindPatientHiddenDetails(common.myInt(Request.QueryString["RegNo"]));

        }
    }
    protected void BindPatientHiddenDetails(int RegistrationNo)
    {
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);
        BaseC.RestFulAPI objIPBill = new RestFulAPI(sConString);
        if (RegistrationNo > 0)
        {
            int HospId = common.myInt(Session["HospitalLocationID"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int RegId = 0;
            int EncounterId = 0;
            int EncodedBy = common.myInt(Session["UserId"]);
            DataSet ds = new DataSet();

            ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, RegistrationNo, EncodedBy, 0, "");

            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {

            //        DataRow dr = ds.Tables[0].Rows[0];

            //        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
            //        lblDob.Text = common.myStr(dr["DOB"]);
            //        lblMobile.Text = common.myStr(dr["MobileNo"]);
            //        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
            //        lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);



            //    }
            //   }

        }
    }
    public void FillData()
    {
        BaseC.WardManagement wm = new BaseC.WardManagement();
        DataSet ds = wm.GetIPServiceAcitivityDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            common.myInt(Request.QueryString["Regid"]), common.myInt(Request.QueryString["EncounterId"]), common.myInt(ddlService.SelectedValue),
            common.myInt(ddlDepartment.SelectedValue),
            common.myStr(""), common.myInt(ddlLabStatus.SelectedValue), common.myInt(ddlStatus.SelectedValue));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dvFilterDoctorwiseService = new DataView(ds.Tables[0]);
                // dvFilterDoctorwiseService.RowFilter = "DoctorId='" + common.myInt(Session["LoginDoctorId"]) + "'";
                // if (common.myInt(Session["LoginDoctorId"])==0)

                //{               
                gvServiceDetails.DataSource = ds;
                gvServiceDetails.DataBind();
                BindLabStatus(ds.Tables[0]);
                BindService(ds.Tables[0]);
                BindDepartment(ds.Tables[0]);
                //}
                // else
                //{
                //    if (dvFilterDoctorwiseService.ToTable().Rows.Count > 0)
                //    {

                //        gvServiceDetails.DataSource = dvFilterDoctorwiseService.ToTable();
                //        gvServiceDetails.DataBind();
                //        BindLabStatus(dvFilterDoctorwiseService.ToTable());
                //        BindService(dvFilterDoctorwiseService.ToTable());
                //        BindDepartment(dvFilterDoctorwiseService.ToTable());

                //    }

                //}

            }
        }
    }

    protected void BindDepartment(DataTable dt)
    {
        try
        {
            ddlDepartment.Items.Clear();
            BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);

            DataTable dtDepartment = dt.DefaultView.ToTable(true, "Departmentname", "DepartmentID");

            ddlDepartment.DataSource = dtDepartment;
            ddlDepartment.DataTextField = "Departmentname";
            ddlDepartment.DataValueField = "DepartmentID";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new RadComboBoxItem("--- All Department ---", ""));
            ddlDepartment.SelectedIndex = 0;


        }
        catch (Exception Ex)
        {

        }
    }
    public void BtnRefresh_OnClick(object sender, EventArgs e)
    {
        BaseC.WardManagement wm = new BaseC.WardManagement();
        DataSet ds = wm.GetIPServiceAcitivityDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            common.myInt(Request.QueryString["Regid"]), common.myInt(Request.QueryString["EncounterId"]), common.myInt(ddlService.SelectedValue)
            , common.myInt(ddlDepartment.SelectedValue),
            common.myStr(""), common.myInt(ddlLabStatus.SelectedValue), common.myInt(ddlStatus.SelectedValue));
        if (ds.Tables.Count > 0)
        {
            //////if (ds.Tables[0].Rows.Count > 0)
            //////{
            //////    gvServiceDetails.DataSource = ds;
            //////    gvServiceDetails.DataBind();
            //////    BindLabStatus(ds.Tables[0]);
            //////    BindService(ds.Tables[0]);
            //////}

            if (ds.Tables[0].Rows.Count > 0)
            {
                //DataView dvFilterDoctorwiseService = new DataView(ds.Tables[0]);
                //dvFilterDoctorwiseService.RowFilter = "DoctorId='" + common.myInt(Session["LoginDoctorId"]) + "'";
                //if (common.myInt(Session["LoginDoctorId"]) == 0)
                //{
                gvServiceDetails.DataSource = ds;
                gvServiceDetails.DataBind();
                BindLabStatus(ds.Tables[0]);
                BindService(ds.Tables[0]);
                //BindDepartment(ds.Tables[0]);
                //}
                //else
                //{
                //    if (dvFilterDoctorwiseService.ToTable().Rows.Count > 0)
                //    {

                //        gvServiceDetails.DataSource = dvFilterDoctorwiseService.ToTable();
                //        gvServiceDetails.DataBind();
                //        BindLabStatus(dvFilterDoctorwiseService.ToTable());
                //        BindService(dvFilterDoctorwiseService.ToTable());
                //        //BindDepartment(dvFilterDoctorwiseService.ToTable());

                //    }

                //}

            }
        }
    }
    public void BindService(DataTable dt)
    {
        DataTable dtService = dt.DefaultView.ToTable(true, "ServiceName", "ServiceId");
        if (dtService.Rows.Count > 0)
        {
            ddlService.DataSource = dtService;
            ddlService.DataTextField = "ServiceName";
            ddlService.DataValueField = "ServiceId";
            ddlService.DataBind();
            ddlService.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
        }
    }
    public void BindLabStatus(DataTable dt)
    {
        DataTable dtService = dt.DefaultView.ToTable(true, "ServiceStatus", "StatusId");
        if (dtService.Rows.Count > 0)
        {
            ddlLabStatus.DataSource = dtService;
            ddlLabStatus.DataTextField = "ServiceStatus";
            ddlLabStatus.DataValueField = "StatusId";
            ddlLabStatus.DataBind();
            ddlLabStatus.Items.Insert(0, new RadComboBoxItem(" --- All Status --- ", "0"));
        }
    }
    protected void lnkResult_OnClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridDataItem di = (GridDataItem)btn.NamingContainer;
        HiddenField hdnOrderId = (HiddenField)di.FindControl("hdnOrderId");
        HiddenField hdnServiceID = (HiddenField)di.FindControl("hdnServiceID");
        HiddenField hdnLabNo = (HiddenField)di.FindControl("hdnLabNo");
        HiddenField hdnResultHTML = (HiddenField)di.FindControl("hdnResultHTML");
        HiddenField hdnDiagSampleId = (HiddenField)di.FindControl("hdnDiagSampleId");
        string sStatus = common.myStr(btn.Text) == "Result Provisional" ? "RP" : "RF";
        if (common.myInt(hdnResultHTML.Value) != 1)
        {
            //RadWindow2.NavigateUrl = "~/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=" + common.myStr("IPD") + "&LABNO=" + hdnLabNo.Value
            //       + "&ServiceIds=" + hdnServiceID.Value + "&StationId=" + common.myInt(Session["StationId"])
            //       + "&Flag=" + "LIS";
            
            RadWindow2.NavigateUrl = "~/LIS/Phlebotomy/InvestigationResult.aspx?SOURCE=IPD&LABNO=" +
                                        common.myInt(hdnLabNo.Value) +"&MASTER=Y" +
                                        "&SEL_DiagSampleID=" + common.myInt(hdnDiagSampleId.Value) +
                                        "&SEL_ServiceId=" + common.myInt(hdnServiceID.Value) +
                                        //"&SEL_ResultRemarksId=" + common.myInt(lblResultRemarksId.Text) +
                                        "&SEL_StatusCode=" + common.myStr(sStatus) +
                                        "&Page=SCD&FromMaster=" + common.myStr("WARD") +
                                        "&MD=" + common.myStr(Request.QueryString["MD"]);
            RadWindow2.Height = 570;
            RadWindow2.Width = 850;
            RadWindow2.Top = 10;
            RadWindow2.Left = 10;
            RadWindow2.VisibleOnPageLoad = true;
            RadWindow2.Modal = true;
            RadWindow2.VisibleStatusbar = false;
            RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else
        {
            RadWindow2.NavigateUrl = "/EMRReports/PrintPdfForHTML.aspx?SOURCE=" + common.myStr("IPD") +
                                "&LABNO=" + hdnLabNo.Value + "&ServiceIds=" + common.myInt(hdnServiceID.Value) +
                                "&StationId=" + common.myInt(Session["StationId"]) + "&Flag=" + "LIS"
                                + "&RegId=" + common.myInt(ViewState["RegistrationId"])
                                + "&DiagSampleId=" + common.myInt(hdnDiagSampleId.Value) +
                                "&EncId=" + common.myInt(ViewState["EncounterId"]);
        }
        RadWindow2.Height = 550;
        RadWindow2.Width = 800;
        RadWindow2.Top = 45;
        RadWindow2.Left = 10;
        RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;
    }
    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridDataItem di = (GridDataItem)btn.NamingContainer;
        HiddenField hdnOrderId = (HiddenField)di.FindControl("hdnOrderId");
        if (common.myInt(hdnOrderId.Value) > 0)
        {
            Session["encounterId"] = common.myInt(Request.QueryString["EncounterId"]).ToString();
            Session["RegistrationId"] = common.myInt(Request.QueryString["Regid"]);
            Session["OrderID"] = hdnOrderId.Value.ToString();


            RadWindow2.NavigateUrl = "~/EMR/Orders/PrintOrder.aspx?rptName=POD";

            RadWindow2.Height = 580;
            RadWindow2.Width = 1000;
            RadWindow2.Top = 40;
            RadWindow2.Left = 100;
            RadWindow2.Title = "Print Order";
            //RadWindow2.OnClientClose = "OnClientCloseNextSlot";
            RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow2.Modal = true;
            RadWindow2.VisibleStatusbar = false;
        }
    }
    protected void gvServiceDetails_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem di = e.Item as GridDataItem;
            HiddenField hdnColorCode = (HiddenField)di.FindControl("hdnColorCode");
            HiddenField hdnServiceCode = (HiddenField)di.FindControl("hdnServiceCode");
            LinkButton lnkResult = (LinkButton)di.FindControl("lnkResult");
            ImageButton ibtnForNotes = (ImageButton)di.FindControl("ibtnForNotes");
            HiddenField hdnLabSampleNotes = (HiddenField)di.FindControl("hdnLabSampleNotes");
            di.Cells[10].BackColor = System.Drawing.Color.FromName(hdnColorCode.Value);

            if(common.myInt(hdnLabSampleNotes.Value)>0)
            {
                ibtnForNotes.Visible = true;
            }
            if ((common.myStr(hdnServiceCode.Value).Equals("RF")) || (common.myStr(hdnServiceCode.Value).Equals("RP"))) //&& common.myStr(ViewState["IsPrintProvisional"]).Equals("Y")))
            {
                lnkResult.Enabled = true;
            }
            else
            {
                lnkResult.Enabled = false;
                lnkResult.Attributes.Add("style", "text-decoration:none;");
            }
        }
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {

        Session["encounterId"] = common.myInt(Request.QueryString["EncounterId"]).ToString();
        Session["RegistrationId"] = common.myInt(Request.QueryString["Regid"]);
        RadWindow2.NavigateUrl = "~/EMR/Orders/PrintOrder.aspx?rptName=INR";
        RadWindow2.Height = 580;
        RadWindow2.Width = 1000;
        RadWindow2.Top = 40;
        RadWindow2.Left = 100;
        RadWindow2.Title = "Investigation List";
        //RadWindow2.OnClientClose = "OnClientCloseNextSlot";
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;
    }




    protected void gvServiceDetails_EditCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void ibtnForNotes_Click(object sender, ImageClickEventArgs e)
    {
        string Flag = "LIS";
        string Source = "IPD";
        ImageButton btn = (ImageButton)sender;
        GridDataItem di = (GridDataItem)btn.NamingContainer;
        ImageButton ibtnForNotes = (ImageButton)di.FindControl("ibtnForNotes");
        RadWindow2.NavigateUrl = "~/LIS/Format/LISNotes.aspx?MD=" + Flag +
                                   "&SOURCE=" + common.myStr(Source) +
                                   "&eno=" + common.myStr(Request.QueryString["EncounterId"]) +
                                   "&RegNo=" + common.myStr(Request.QueryString["RegNo"]) +
                                   "&LABNO=" +
                                   "&Servicedetails=" + 1 +
                                   "&OrderId=" + ibtnForNotes.CommandArgument;

        RadWindow2.Height = 580;
        RadWindow2.Width = 900;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;

    }
}
