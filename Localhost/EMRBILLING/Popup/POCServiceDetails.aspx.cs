using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseC;
using Telerik.Web.UI;

public partial class EMRBILLING_Popup_Servicedetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    bool boolCheckUserRights;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        try
        {
            if (Session["UserID"] == null || Session["HospitalLocationID"] == null || Session["FacilityId"] == null)
            {
                Response.Redirect("~/Login.aspx?Logout=1", false);
                return;
            }

            if (!IsPostBack)
            {
                if (!common.myStr(Request.QueryString["RegNo"]).Equals(""))
                {
                    BindPatientHiddenDetails(common.myStr(Request.QueryString["RegNo"]));
                    GetPOCServiceOrderDetails();
                }

                if (!common.myStr(Request.QueryString["DeptName"]).Equals(""))
                {
                    Page.Title = "Service Details ( " + common.myStr(Request.QueryString["DeptName"]) + " )";
                }

                // CheckUserRights();
                //BindServices();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ua1.Dispose();
        }
    }

    protected void ddlStatus_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            if (ViewState["dtServices"] == null)
                return;

            lblMessage.Text = "";

            RadComboBox ddlS = (RadComboBox)gvService.HeaderRow.FindControl("ddlService");
            string SelectedService = ddlS.SelectedValue;
            string sValue = e.Value;
            dt = (DataTable)ViewState["dtServices"];
            if (dt.Rows.Count > 0)
            {
                dv = dt.DefaultView;

                if (e.Value == "0")
                {
                    if (common.myInt(ddlS.SelectedValue) == 0)
                    {
                        gvService.DataSource = dt;
                        gvService.DataBind();
                    }
                    else
                    {
                        dv.RowFilter = "ServiceId = " + common.myStr(SelectedService);
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            gvService.DataSource = dv.ToTable();
                            gvService.DataBind();
                        }
                    }

                }
                else
                {

                    if (ddlS != null && common.myInt(ddlS.SelectedValue) == 0)
                    {
                        dv.RowFilter = "StatusId = " + common.myStr(e.Value);
                    }
                    else
                    {
                        dv.RowFilter = "ServiceId = " + common.myStr(ddlS.SelectedValue) + " and StatusId = " + common.myStr(e.Value);
                    }
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        gvService.DataSource = dv.ToTable();
                        gvService.DataBind();
                    }
                }
            }

            if (ddlS != null)
            {
                //dt = new DataTable();
                //dt = (DataTable)ViewState["dtServices"];
                if (dt.Rows.Count > 0)
                {
                    ddlS.DataSource = dt.DefaultView.ToTable(true, "ServiceId", "serviceName"); ;
                    ddlS.DataTextField = "serviceName";
                    ddlS.DataValueField = "ServiceId";
                    ddlS.DataBind();
                    ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
                    ddlS.SelectedValue = SelectedService;
                }
            }

            RadComboBox ddlP = (RadComboBox)gvService.HeaderRow.FindControl("ddlStatus");
            if (ddlP != null)
            {
                //dt = new DataTable();
                //dt = (DataTable)ViewState["dtServices"];
                if (dt.Rows.Count > 0)
                {
                    ddlP.DataSource = dt.DefaultView.ToTable(true, "StatusId", "Status");
                    ddlP.DataTextField = "Status";
                    ddlP.DataValueField = "StatusId";
                    ddlP.DataBind();
                    ddlP.Items.Insert(0, new RadComboBoxItem(" --- All Status --- ", "0"));
                    ddlP.SelectedValue = sValue;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            dt.Dispose();
            dv.Dispose();
        }
    }
    protected void ddlService_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            if (ViewState["dtServices"] == null)
                return;

            lblMessage.Text = "";


            dt = (DataTable)ViewState["dtServices"];
            string sValue = e.Value;
            if (dt.Rows.Count > 0)
            {
                dv = dt.DefaultView;
                if (common.myStr(e.Value).Equals("0"))
                {
                    gvService.DataSource = dt;
                    gvService.DataBind();
                }
                else
                {
                    dv.RowFilter = "ServiceId=" + common.myInt(e.Value).ToString();
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        gvService.DataSource = dv.ToTable();
                        gvService.DataBind();
                    }
                }
            }
            RadComboBox ddlS = (RadComboBox)gvService.HeaderRow.FindControl("ddlService");
            if (ddlS != null)
            {
                //dt = new DataTable();
                //dt = (DataTable)ViewState["dtServices"];
                if (dt.Rows.Count > 0)
                {
                    ddlS.DataSource = dt.DefaultView.ToTable(true, "ServiceId", "serviceName"); ;
                    ddlS.DataTextField = "serviceName";
                    ddlS.DataValueField = "ServiceId";
                    ddlS.DataBind();
                    ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
                    ddlS.SelectedValue = sValue;
                }
            }

            RadComboBox ddlP = (RadComboBox)gvService.HeaderRow.FindControl("ddlStatus");
            if (ddlP != null)
            {
                dt = new DataTable();
                dt = dv.ToTable(true, "Status", "StatusId");
                if (dt.Rows.Count > 0)
                {
                    ddlP.DataSource = dt;
                    ddlP.DataTextField = "Status";
                    ddlP.DataValueField = "StatusId";
                    ddlP.DataBind();
                    ddlP.Items.Insert(0, new RadComboBoxItem(" --- All Status --- ", "0"));
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            dt = null;
            dv.Dispose();
        }
    }
    //protected void ddlService_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    DataTable dt = new DataTable();
    //    DataView dv = new DataView();
    //    RadComboBox ddlS = (RadComboBox)gvService.HeaderRow.FindControl("ddlService");
    //    dt = (DataTable)ViewState["dtServices"];
    //    string sValue = e.Value;

    //    if (ddlS != null)
    //    {
    //        if (ViewState["dtServices"] == null)
    //            return;
    //        lblMessage.Text = "";
    //        //dt = new DataTable();
    //        //dt = (DataTable)ViewState["dtServices"];
    //        if (dt.Rows.Count > 0)
    //        {
    //            dv = dt.DefaultView;
    //            if (common.myStr(e.Value).Equals("0"))
    //            {
    //                gvService.DataSource = dt;
    //                gvService.DataBind();
    //            }
    //            else
    //            {
    //                dv.RowFilter = "ServiceId=" + common.myInt(e.Value).ToString();
    //                if (dv.ToTable().Rows.Count > 0)
    //                {
    //                    gvService.DataSource = dv.ToTable();
    //                    gvService.DataBind();
    //                }
    //            }

    //            //string sValue = e.Value;
    //            ddlS.DataSource = dt;
    //            ddlS.DataTextField = "ServiceName";
    //            ddlS.DataValueField = "ServiceId";
    //            ddlS.DataBind();
    //            ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
    //            ddlS.SelectedValue = sValue;
    //        }
    //    }
    //}
    private void GetPOCServiceOrderDetails()
    {
        DataSet ds = new DataSet();
        clsEMR objBill = new clsEMR(sConString);
        int iRegID = 0, iEncID = 0;
        iRegID = common.myInt(Request.QueryString["RegNo"]);
        iEncID = common.myInt(Request.QueryString["EncId"]);
        try
        {
            if (common.myStr(Request.QueryString["PType"]).Equals("WD"))
            {
                ds = objBill.GetPOCServiceOrderDetails(common.myInt(Session["HospitalLocationID"]),
                    common.myInt(Session["FacilityId"]), iRegID, iEncID, common.myInt(Request.QueryString["BillId"]), 0, common.myStr(Request.QueryString["DeptType"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["dtServices"] = ds.Tables[0];
                    gvService.DataSource = ds.Tables[0].DefaultView;
                    gvService.DataBind();
                    gvService.FooterRow.Cells[2].Text = "Total : ";
                    gvService.FooterRow.Cells[3].Text = common.myStr(ds.Tables[0].Rows.Count);

                    RadComboBox ddlS = (RadComboBox)gvService.HeaderRow.FindControl("ddlService");
                    ddlS.DataSource = ds.Tables[0].DefaultView.ToTable(true, "ServiceId", "serviceName");
                    ddlS.DataTextField = "serviceName";
                    ddlS.DataValueField = "ServiceId";
                    ddlS.DataBind();
                    ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));

                    RadComboBox ddlSt = (RadComboBox)gvService.HeaderRow.FindControl("ddlStatus");
                    ddlSt.DataSource = ds.Tables[0].DefaultView.ToTable(true, "StatusId", "Status");
                    ddlSt.DataTextField = "Status";
                    ddlSt.DataValueField = "StatusId";
                    ddlSt.DataBind();
                    ddlSt.Items.Insert(0, new RadComboBoxItem(" --- All Status --- ", "0"));
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            objBill = null;
        }
    }

    private void CheckUserRights()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        boolCheckUserRights = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForBillDiscount");

    }

    //   Sorting 
    protected void SortRecords(object sender, EventArgs e)
    {
        string sortExpression = "ServiceName";
        string direction = string.Empty;
        if (SortDirection == SortDirection.Ascending)
        {
            SortDirection = SortDirection.Descending;
            direction = " DESC";
        }
        else
        {
            SortDirection = SortDirection.Ascending;
            direction = " ASC";
        }
        DataTable dt = (DataTable)ViewState["dtServices"];
        dt.DefaultView.Sort = sortExpression + direction;
        gvService.DataSource = dt;
        gvService.DataBind();

    }
    protected void SortRecords(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;
        string direction = string.Empty;
        if (SortDirection == SortDirection.Ascending)
        {
            SortDirection = SortDirection.Descending;
            direction = " DESC";
        }
        else
        {
            SortDirection = SortDirection.Ascending;
            direction = " ASC";
        }
        DataTable dt = (DataTable)ViewState["dtServices"];
        dt.DefaultView.Sort = sortExpression + direction;
        gvService.DataSource = dt;
        gvService.DataBind();


    }
    public SortDirection SortDirection
    {
        get
        {
            if (ViewState["SortDirection"] == null)
            {
                ViewState["SortDirection"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["SortDirection"];
        }
        set
        {
            ViewState["SortDirection"] = value;
        }
    }

    // End  Sorting 
    protected void gvService_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["dtServices"] != null)
        {
            gvService.DataSource = ViewState["dtServices"] as DataTable;

            gvService.PageIndex = e.NewPageIndex;
            gvService.DataBind();
        }
    }

    void BindPatientHiddenDetails(String RegistrationNo)
    {
        BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
        BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (!RegistrationNo.Equals(""))
            {
                if (common.myStr(Session["OPIP"]).Equals("O"))
                {
                    ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, common.myInt(RegistrationNo), 0, common.myInt(Session["UserId"]));
                    
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        hdnRegId.Value = common.myStr(dr["RegistrationId"]);
                        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                        lblDob.Text = common.myStr(dr["DOB"]);
                        lblMobile.Text = common.myStr(dr["MobileNo"]);
                        Label3.Text = "OP No. : ";
                        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);

                    }
                }
                else  //I
                {
                    ds = objIPBill.GetPatientDetailsIP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, common.myInt(RegistrationNo), common.myInt(Session["UserId"]), 0, "");

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            hdnRegId.Value = common.myStr(dr["RegistrationId"]);
                            lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                            lblDob.Text = common.myStr(dr["DOB"]);
                            lblMobile.Text = common.myStr(dr["MobileNo"]);
                            lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                            lblAdmissionDate.Text = common.myStr(dr["AdmissionDate"]);

                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objIPBill = null;
            bC = null;
            ds.Dispose();
        }
    }

    protected void gvService_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                LinkButton lnkResult = (LinkButton)e.Row.FindControl("lnkResult");
                LinkButton lnkPrint = (LinkButton)e.Row.FindControl("lnkPrint");
                Label lblStatusCode = (Label)e.Row.FindControl("lblStatusCode");
                if (common.myStr(lblStatusCode.Text) == "RF") { lnkResult.Visible = false; lnkPrint.Visible = true; }
                else { lnkResult.Visible = true; lnkPrint.Visible = false; }
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int diagSampleId = 0;

        if (e.CommandName.Equals("PrintLabel"))
        {
            //GridViewRow row = (GridViewRow)(((DataControlFieldCell)e.CommandSource).NamingContainer);
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            StringBuilder strXml = new StringBuilder();
            ArrayList coll = new ArrayList();
            Label lblDiagSampleId = (Label)row.FindControl("lblDiagSampleId");
            coll.Add("");
            coll.Add(lblDiagSampleId.Text);
            if (common.myStr(lblDiagSampleId.Text) != "")
            {
                diagSampleId = common.myInt(lblDiagSampleId.Text);
            }
            string Act = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "PrintlblPRN", sConString);

            string sMD = string.Empty;

            if (Request.QueryString["MD"] != null)
            {
                sMD = common.myStr(Request.QueryString["MD"]);
            }

            string Source = "IPD"; //common.myStr(ViewState["Source"]);

            if (Act == "Y" && sMD != "RIS")
            {
                string Str = common.myInt(Session["FacilityId"]) + "$" + common.myStr(ViewState["LabNo"]) + "$" + diagSampleId + "$" + "PrintPRN_Lab";
                Str = "asplprint:" + Str;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
                return;
            }
            else
            {
                RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/PrintInvestigationLabelsReport.aspx?SOURCE=" + common.myStr(Source) +
                                                            "&LABNO=" + common.myInt(ViewState["LabNo"]) +
                                                            "&LoginFacilityId=" + common.myInt(Session["FacilityId"]) +
                                                            "&DSNo=&STATION=&DiagSampleId=" + diagSampleId;
                RadWindowForNew.Height = 580;
                RadWindowForNew.Width = 830;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }

            lblMessage.Text = "";
        }
        else if (e.CommandName.Equals("Result"))
        {

            try
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblStatusCode = (Label)row.FindControl("lblStatusCode");
                Label lblDiagSampleId = (Label)row.FindControl("lblDiagSampleId");
                Label lblServiceId = (Label)row.FindControl("lblServiceId");
                Label lblEncounterNo = (Label)row.FindControl("lblEncounterNo");
                Label lblLabNo = (Label)row.FindControl("lblLabNo");
                Label lblStationId = (Label)row.FindControl("lblStationId");
                Label lblServiceName = (Label)row.FindControl("lblServiceName");
                string sRegNo = common.myStr(Request.QueryString["RegNo"]);
                Session["EncounterNo"] = common.myStr(lblEncounterNo.Text);
                RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/POCOutSource.aspx?SEL_DiagSampleID=" + common.myInt(lblDiagSampleId.Text) + "&callfrom=POC&From=POPUP&CloseButtonShow=No&SEL_ServiceId=" + common.myInt(lblServiceId.Text) + "&SEL_StatusCode=" + common.myStr(lblStatusCode.Text) + "&LabNo=" + common.myInt(lblLabNo.Text) + "&RegNo=" + sRegNo + "&StationId=" + common.myInt(lblStationId.Text)+"&ServiceName="+
                   common.myStr(lblServiceName.Text);
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true;
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.OnClientClose = "OnClientCloseServices";
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + ex.Message;
            }
        }
        else if (e.CommandName.Equals("Print"))
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            Label lblDiagSampleId = (Label)row.FindControl("lblDiagSampleId");
            Label lblServiceId = (Label)row.FindControl("lblServiceId");
            Label lblLabNo = (Label)row.FindControl("lblLabNo");
            Label lblStationId = (Label)row.FindControl("lblStationId");
            PrintLabReport(common.myInt(lblStationId.Text), common.myInt(lblLabNo.Text), common.myInt(lblDiagSampleId.Text), common.myInt(lblServiceId.Text));
        }
    }
    private void PrintLabReport(int iStationId, int iLabNo, int iDiagSampleId, int iServiceId)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            string strServiceIds = "";
            lblMessage.Text = "";
            if (iLabNo == 0)
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }
            //ArrayList coll = new ArrayList();
            //StringBuilder strXml = new StringBuilder();
            //StringBuilder objXML = new StringBuilder();
            //System.Text.StringBuilder str = new System.Text.StringBuilder();

            //Label lblResultHTML = (Label)item.FindControl("lblResultHTML");
            //coll.Add(common.myStr(iServiceId));
            //coll.Add(common.myStr(iDiagSampleId));
            //strXml.Append(common.setXmlTable(ref coll));

            //coll.Add(common.myStr(iServiceId));
            //coll.Add(common.myStr(iDiagSampleId));
            //coll.Add("IPD");
            //coll.Add(iLabNo);            
            //objXML.Append(common.setXmlTable(ref coll));
            if (strServiceIds.Length == 0)
            {
                strServiceIds = common.myStr(iServiceId).Trim();
            }
            if (strServiceIds.Length == 0)
            {
                lblMessage.Text = "Please Select Service(s) !";
                return;
            }
            else
            {
                string sPriview = string.Empty;
                RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=IPD&LABNO=" + iLabNo + "&DiagSampleId=" + iDiagSampleId
                       + "&ServiceIds=" + strServiceIds + "&StationId=" + common.myInt(Session["StationId"]) + "&Flag=&preview=" + sPriview + "&iAddHeader=0";

                RadWindowForNew.Height = 750;
                RadWindowForNew.Width = 800;
                RadWindowForNew.Top = 45;
                RadWindowForNew.Left = 10;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                //RadWindowForNew.OnClientClose = "OnClientClose";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            //objException.HandleException(Ex);
        }
    }

    protected void btnPrintLabel_OnClick(object sender, EventArgs e)
    {
        int diagSampleId = 0;

        StringBuilder strXml = new StringBuilder();
        ArrayList coll = new ArrayList();

        LinkButton lnk = (LinkButton)sender;
        GridDataItem gdi = (GridDataItem)lnk.NamingContainer;

        Label lblDiagSampleId = (Label)gdi.FindControl("lblDiagSampleId");
        coll.Add("");
        coll.Add(lblDiagSampleId.Text);
        if (common.myStr(lblDiagSampleId.Text) != "")
        {
            diagSampleId = common.myInt(lblDiagSampleId.Text);

        }

        string Act = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "PrintlblPRN", sConString);

        string sMD = string.Empty;

        if (Request.QueryString["MD"] != null)
        {
            sMD = common.myStr(Request.QueryString["MD"]);
        }


        string Source = "IPD"; //common.myStr(ViewState["Source"]);

        if (Act == "Y" && sMD != "RIS")
        {
            string Str = common.myInt(Session["FacilityId"]) + "$" + common.myStr(ViewState["LabNo"]) + "$" + diagSampleId + "$" + "PrintPRN_Lab";
            Str = "asplprint:" + Str;
            ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
            return;
        }
        else
        {
            RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/PrintInvestigationLabelsReport.aspx?SOURCE=" + common.myStr(Source) +
                                                        "&LABNO=" + common.myInt(ViewState["LabNo"]) +
                                                        "&LoginFacilityId=" + common.myInt(Session["FacilityId"]) +
                                                        "&DSNo=&STATION=&DiagSampleId=" + diagSampleId;
            RadWindowForNew.Height = 580;
            RadWindowForNew.Width = 830;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
    }

    protected void btnUpdateToLIS_Click(object sender, EventArgs e)
    {

        //?SOURCE = IPD & LABNO = " + common.myInt(ViewState["LabNo"]) +"&LoginFacilityId=" + common.myInt(Session["FacilityId"]) +
        //                                            "&DSNo=&STATION=&DiagSampleId=";
        
        RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/LabDevicesResult.aspx";
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        GetPOCServiceOrderDetails();
    }
}