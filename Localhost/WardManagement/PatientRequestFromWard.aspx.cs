using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections;
using System.Configuration;

public partial class Pharmacy_SaleIssue_PatientRequestFromWard : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    clsExceptionLog objException = new clsExceptionLog();

    //private enum GridEncounter : byte
    //{
    //    Select = 0
    //}

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);


            //txtFromDate.SelectedDate = System.DateTime.Now;
            //txtToDate.SelectedDate = System.DateTime.Now;

            ViewState["IsApproved"] = common.myStr(Request.QueryString["IsApproved"]);

            objPharmacy = new BaseC.clsPharmacy(sConString);

            tblDate.Visible = false;

            if (common.myStr(ddlSearchOn.SelectedValue) == "2")
            {
                txtSearch.Visible = false;
                txtSearchRegNo.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
                txtSearchRegNo.Visible = false;
            }
            ddlTime.SelectedValue = "DateRange";
            tblDate.Visible = true;
            bindControl();
            setDate();
            bindData();
            bindstore();
        }
    }

    private void bindstore()
    {
        try
        {
            DataSet ds = new DataSet();
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = objPharmacy.GETAllStore(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));
            ddlStore.DataSource = ds.Tables[0];

            ddlStore.DataValueField = "StoreId";
            ddlStore.DataTextField = "DepartmentName";
            ddlStore.DataBind();
            ddlStore.Items.Insert(0, new RadComboBoxItem("All", "0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    private void bindControl()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            string strsql = "select Id,IndentType from IndentTypeMaster where FacilityId=" + common.myInt(Session["FacilityId"]) + " and HospitalLocationId =" + common.myInt(Session["HospitalLocationId"]) + " And Active=1";
            ds = dl.FillDataSet(CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlOrderType.DataSource = ds;
                ddlOrderType.DataTextField = "IndentType";
                ddlOrderType.DataValueField = "Id";
                ddlOrderType.DataBind();
            }
            ddlOrderType.Items.Insert(0, new RadComboBoxItem("ALL"));
            //ddlOrderType.Items[0].Value = "255";
            //ddlOrderType.SelectedValue = "255";

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void clearControl()
    {
        txtSearch.Text = "";
        lblMessage.Text = "&nbsp;";
        ddlTime.SelectedIndex = 0;
        ddlOrderStatus.SelectedIndex = 0;
        ddlOrderType.SelectedValue = "255";
        tblDate.Visible = false;
    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        bindData();
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        clearControl();
    }

    protected void gvEncounter_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }
        if (e.Item is GridHeaderItem)
        {
            if (common.myStr(ViewState["IsApproved"]) == "P")
            {
                e.Item.Cells[13].Visible = false;
            }
            else if (common.myStr(ViewState["IsApproved"]) == "C")
            {
                e.Item.Cells[13].Text = "Rejected By";
                e.Item.Cells[13].Visible = true;
            }
            else if (common.myStr(ViewState["IsApproved"]) == "A")
            {
                e.Item.Cells[13].Text = "Approved By";
                e.Item.Cells[13].Visible = true;
            }
        }
        if (e.Item is GridDataItem)
        {
            HiddenField hdnColorCode = (HiddenField)e.Item.FindControl("hdnColorCode");
            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml(hdnColorCode.Value);

            HiddenField hdnEncounterStatusCode = (HiddenField)e.Item.FindControl("hdnEncounterStatusCode");

            HiddenField hdnIsIndentMorethan1Hour = (HiddenField)e.Item.FindControl("hdnIsIndentMorethan1Hour");
            HiddenField hdnIsStat = (HiddenField)e.Item.FindControl("hdnIsStat");
            HiddenField hdnIsApproved = (HiddenField)e.Item.FindControl("hdnIsApproved");
            HiddenField hdnPending = (HiddenField)e.Item.FindControl("hdnPending");

            LinkButton btnSelect = (LinkButton)e.Item.FindControl("btnSelect");
            Label lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");
            Label lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");


            HiddenField hdnIsConsumable = (HiddenField)e.Item.FindControl("hdnIsConsumable");
            HiddenField hdnIsPickListPrinted = (HiddenField)e.Item.FindControl("hdnIsPickListPrinted");

            LinkButton lblPickList = (LinkButton)e.Item.FindControl("lblPickList");
            if (common.myStr(ViewState["IsApproved"]) == "P")
            {
                e.Item.Cells[13].Visible = false;
            }

            if (common.myBool(hdnIsPickListPrinted.Value) == true)
            {
                lblPickList.Text = "Pick List - Printed";
            }
            else
            {
                lblPickList.Text = "Pick List - Not Printed";
            }

            if (common.myStr(hdnEncounterStatusCode.Value) == "PC")
            {
                btnSelect.Visible = false;
            }
            GridDataItem dataItem = (GridDataItem)e.Item;

            if (common.myStr(hdnEncounterStatusCode.Value) == "MD")
            {
                TableCell cell = dataItem["RegistrationNo"];
                TableCell cell1 = dataItem["EncounterNo"];

                cell.BackColor = System.Drawing.Color.LightSkyBlue;
                cell1.BackColor = System.Drawing.Color.LightSkyBlue;
            }
            if (common.myStr(hdnIsConsumable.Value).ToUpper() == "TRUE")
            {
                TableCell cell = dataItem["IndentNo"];

                cell.BackColor = System.Drawing.Color.Cyan;
            }
            if (common.myStr(hdnIsConsumable.Value).ToUpper() == "FALSE")
            {
                TableCell cell = dataItem["IndentNo"];

                cell.BackColor = System.Drawing.Color.DarkSeaGreen;
            }

            if (common.myInt(hdnIsIndentMorethan1Hour.Value) == 1)
            {
                TableCell cell = dataItem["IndentDate"];

                cell.BackColor = System.Drawing.Color.Yellow;
            }

            if (common.myInt(hdnPending.Value) == 1)
            {
                TableCell cell = dataItem["PatientName"];

                cell.BackColor = System.Drawing.Color.SpringGreen;
            }
            //if (common.myInt(hdnIsStat.Value)==1)
            //{
            //    TableCell cell = dataItem["PatientName"];

            //    cell.BackColor = System.Drawing.Color.PaleVioletRed;
            //}
        }
    }
    protected void gvEncounter_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvEncounter.CurrentPageIndex = e.NewPageIndex;
        bindData();
    }

    protected void gvEncounter_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                if (common.myInt(((HiddenField)e.Item.FindControl("hdnIndentId")).Value) > 0)
                {
                    hdnIndentId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnIndentId")).Value);
                    hdnIndentNo.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnIndentNo")).Value);
                    hdnFacilityId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnFacilityID")).Value);
                    hdnEncounterId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value);
                    hdnRegistrationId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value);

                    hdnEncounterNo.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", "");
                    hdnRegistrationNo.Value = common.myStr(((Label)e.Item.FindControl("lblRegistrationNo")).Text);

                    Session["EncounterId"] = common.myInt(hdnEncounterId.Value);
                    Session["RegistrationId"] = common.myInt(hdnRegistrationId.Value);
                    Session["Encno"] = common.myStr(hdnEncounterNo.Value);
                    Session["RegistrationNo"] = common.myInt(hdnRegistrationNo.Value);
                    Session["Regno"] = common.myInt(hdnRegistrationNo.Value);

                    
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);

                    BaseC.Patient patient = new BaseC.Patient(sConString);
                    DataSet dst = new DataSet();

                    dst = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                            common.myInt(hdnRegistrationNo.Value).ToString(), common.myStr(hdnEncounterNo.Value), common.myInt(Session["UserId"]), 0);
                    Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = dst;

                    return;
                }
            }
            else if (e.CommandName == "Print")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "";

                //if (common.myInt(ViewState["PrecriptioNo"]) == 0)
                //{
                //    lblMessage.Text = "Prescription No not selected !";
                //    return;
                //}
                if (common.myInt(((HiddenField)e.Item.FindControl("hdnIndentId")).Value) > 0)
                {
                    string IndentId = common.myStr(((HiddenField)e.Item.FindControl("hdnIndentId")).Value);
                    //hdnFacilityId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnFacilityID")).Value);
                    string EncId = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value);
                    //  hdnRegistrationId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value);

                    // hdnEncounterNo.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", "");
                    //   string IndentNo = common.myStr(((Label)e.Item.FindControl("lblIndentNo")).Text);


                    RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=I&PId=" + common.myInt(IndentId) +
                        "&EncId=" + EncId;
                    RadWindow1.Height = 650;
                    RadWindow1.Width = 900;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    //RadWindow1.OnClientClose = "BindDrugsBySubstitute";
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleOnPageLoad = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;

                }
            }
            else if (e.CommandName == "PickList")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "";

                //if (common.myInt(ViewState["PrecriptioNo"]) == 0)
                //{
                //    lblMessage.Text = "Prescription No not selected !";
                //    return;
                //}
                if (common.myInt(((HiddenField)e.Item.FindControl("hdnIndentId")).Value) > 0)
                {
                    string IndentId = common.myStr(((HiddenField)e.Item.FindControl("hdnIndentId")).Value);
                    //hdnFacilityId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnFacilityID")).Value);
                    string EncId = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value);
                    //  hdnRegistrationId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value);

                    // hdnEncounterNo.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", "");
                    //   string IndentNo = common.myStr(((Label)e.Item.FindControl("lblIndentNo")).Text);


                    RadWindow1.NavigateUrl = "/WardManagement/PrintPickList.aspx?PT=I&PId=" + common.myInt(IndentId) +
                        "&EncId=" + EncId;
                    RadWindow1.Height = 650;
                    RadWindow1.Width = 900;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    //RadWindow1.OnClientClose = "PickListPrintClose";
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleOnPageLoad = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;

                    objPharmacy = new BaseC.clsPharmacy(sConString);

                    if (common.myBool(objPharmacy.PickListprintStatus(common.myInt(IndentId))) == false)
                    {
                        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        Hashtable HshIn;
                        HshIn = new Hashtable();
                        HshIn.Add("@intIndentId", Convert.ToInt32(common.myInt(IndentId)));
                        objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspUpdatePickListPrintStatus", HshIn);
                        bindData();
                    }
                }
            }

            else if (e.CommandName == "CaseSheet")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "";

                //if (common.myInt(ViewState["PrecriptioNo"]) == 0)
                //{
                //    lblMessage.Text = "Prescription No not selected !";
                //    return;
                //}
                if (common.myInt(((HiddenField)e.Item.FindControl("hdnIndentId")).Value) > 0)
                {
                    string IndentId = common.myStr(((HiddenField)e.Item.FindControl("hdnIndentId")).Value);
                    //hdnFacilityId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnFacilityID")).Value);
                    string EncId = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value);
                    string sRegId = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value);
                    string sRegNo = common.myStr(((Label)e.Item.FindControl("lblRegistrationNo")).Text);
                    string sEncNo = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", "");
                    BaseC.Patient patient = new BaseC.Patient(sConString);
                    DataSet dst = new DataSet();

                    dst = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                            sRegNo, sEncNo, common.myInt(Session["UserId"]), 0);
                    Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = dst;





                    //  hdnRegistrationId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value);

                    // hdnEncounterNo.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", "");
                    //   string IndentNo = common.myStr(((Label)e.Item.FindControl("lblIndentNo")).Text);


                    //RadWindow1.NavigateUrl = "/Pharmacy/Reports/PrintPickList.aspx?PT=I&PId=" + common.myInt(IndentId) +"&EncId=" + EncId;
                    Session["EncounterId"] = EncId;
                    Session["RegistrationId"] = sRegId;
                    Session["Encno"] = sEncNo;
                    Session["RegistrationNo"] = sRegNo;
                    Session["Regno"] = sRegNo;

                    /*
                      Session["EncounterId"] = common.myInt(sEncId);
            Session["RegistrationID"] = common.myInt(sRegId);
            Session["EncounterDate"] = common.myStr(sEncDate);
            Session["DoctorID"] = common.myInt(sDoctorId);
            Session["RegistrationNo"] = common.myInt(sRegNo);
            Session["Regno"] = common.myStr(sRegNo);
            Session["Encno"] = common.myStr(sEncNo).Trim();
            Session["InvoiceId"] = common.myStr(sinvoiceid).Trim();
                     */



                    string s = common.myStr(Session["UserId"]);
                    RadWindow1.NavigateUrl = "/Editor/VisitHistory.aspx?Regid=" + common.myInt(sRegId) +
                                           "&RegNo=" + common.myInt(sRegNo) +
                                           "&EncId=" + common.myInt(EncId) +
                                           "&EncNo=" + common.myStr(sEncNo) +
                                           "&FromWard=Y&Category=PopUp";
                    RadWindow1.Height = 650;
                    RadWindow1.Width = 900;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    //RadWindow1.OnClientClose = "PickListPrintClose";
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleOnPageLoad = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;

                    objPharmacy = new BaseC.clsPharmacy(sConString);

                    if (common.myBool(objPharmacy.PickListprintStatus(common.myInt(IndentId))) == false)
                    {
                        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        Hashtable HshIn;
                        HshIn = new Hashtable();
                        HshIn.Add("@intIndentId", Convert.ToInt32(common.myInt(IndentId)));
                        objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspUpdatePickListPrintStatus", HshIn);
                        bindData();
                    }
                }
            }

            else if (e.CommandName == "DiagnosticHistory")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "";

                //if (common.myInt(ViewState["PrecriptioNo"]) == 0)
                //{
                //    lblMessage.Text = "Prescription No not selected !";
                //    return;
                //}
                if (common.myInt(((HiddenField)e.Item.FindControl("hdnIndentId")).Value) > 0)
                {
                    string IndentId = common.myStr(((HiddenField)e.Item.FindControl("hdnIndentId")).Value);
                    //hdnFacilityId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnFacilityID")).Value);
                    string EncId = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value);
                    //  hdnRegistrationId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value);

                    // hdnEncounterNo.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", "");
                    //   string IndentNo = common.myStr(((Label)e.Item.FindControl("lblIndentNo")).Text);
                    string sRegId = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value);
                    Session["RegistrationNo"] = sRegId;
                    RadWindow1.NavigateUrl = "~/EMR/PatientHistory.aspx?POPUP=STATICTEMPLATE&From=EMR";
                    RadWindow1.Height = 650;
                    RadWindow1.Width = 900;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    //RadWindow1.OnClientClose = "PickListPrintClose";
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleOnPageLoad = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;

                    objPharmacy = new BaseC.clsPharmacy(sConString);

                    if (common.myBool(objPharmacy.PickListprintStatus(common.myInt(IndentId))) == false)
                    {
                        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        Hashtable HshIn;
                        HshIn = new Hashtable();
                        HshIn.Add("@intIndentId", Convert.ToInt32(common.myInt(IndentId)));
                        objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspUpdatePickListPrintStatus", HshIn);
                        bindData();
                    }
                }
            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //protected void btnPickListPrint_OnClick(object sender, EventArgs e)
    //{
    //    bindData();
    //}
    private void bindData()
    {
        try
        {
            string EncounterNo = string.Empty;
            int RegistrationNo = 0;
            string BedNo = string.Empty;
            string PatientName = string.Empty;

            switch (common.myInt(ddlSearchOn.SelectedValue))
            {
                case 1: // EncNo
                    EncounterNo = common.myStr(txtSearch.Text);
                    break;
                case 2: // RegNo
                    RegistrationNo = common.myInt(txtSearchRegNo.Text);
                    break;
                case 3: // BedNo
                    BedNo = common.myStr(txtSearch.Text);
                    break;
                case 4: // PatientName
                    PatientName = "%" + common.myStr(txtSearch.Text) + "%";
                    break;
            }
            Session["StoreId"] = common.myInt(ddlStore.SelectedValue);
           
            string sMarkForDischarge = chkMarkForDischarge.Checked == true ? "MD" : string.Empty;

            

            BaseC.WardManagement ward = new BaseC.WardManagement();
            DataSet ds = new DataSet();
            DataSet dsSearch = new DataSet();

            ds = ward.getWardGetIPPatientApprovalPendingLists(0, common.myInt(Session["HospitalLocationID"]),
                                    common.myInt(Session["FacilityID"]), 0, 0, RegistrationNo, EncounterNo, BedNo, PatientName,
                                    common.myDate(txtFromDate.SelectedDate), common.myDate(txtToDate.SelectedDate),
                                    common.myInt(Session["UserId"]), common.myInt(ddlOrderType.SelectedValue),
                                    common.myStr(ddlOrderStatus.SelectedValue), common.myInt(Session["StoreId"]), sMarkForDischarge, common.myStr(ddlPatientType.SelectedValue), common.myStr(ViewState["IsApproved"]), common.myInt(ddlWard.SelectedValue));
           
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvEncounter.Visible = true;
                lblMessage.Visible = false;
                Session["DoctorId"] = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorId"]);
                Session["EncounterDate"] = common.myStr(ds.Tables[0].Rows[0]["EncounterDate"]);
                string aa = common.myStr(Session["UserId"].ToString());
                DataView dv = new DataView(ds.Tables[0]);
                lblTotalCountConsumable.Text = "";
                lblTotalCountNonConsumable.Text = "";
                if (common.myStr(ddlDrugOrderType.SelectedValue) != "0")
                {
                    if (common.myStr(ddlDrugOrderType.SelectedValue) == "CO")
                    {
                        dv.RowFilter = "IsConsumable=1";
                        lblTotalCountConsumable.Text = " Total Consumable Order(s) = " + common.myStr(dv.ToTable().Rows.Count);
                    }
                    else if (common.myStr(ddlDrugOrderType.SelectedValue) == "DO")
                    {
                        dv.RowFilter = "IsConsumable=0";
                        lblTotalCountNonConsumable.Text = " Total Drug Order(s)= " + common.myStr(dv.ToTable().Rows.Count);
                    }
                }
                else
                {
                    if (common.myStr(ddlDrugOrderType.SelectedValue) == "0")
                    {
                        DataView dv1 = new DataView(dv.ToTable());
                        dv1.RowFilter = "IsConsumable=1";
                        lblTotalCountConsumable.Text = " Total Consumable Order(s) = " + common.myStr(dv1.ToTable().Rows.Count);

                        DataView dv2 = new DataView(dv.ToTable());
                        dv2.RowFilter = "IsConsumable=0";
                        lblTotalCountNonConsumable.Text = " Total Drug Order(s) = " + common.myStr(dv2.ToTable().Rows.Count);

                        dv1.Dispose();
                        dv2.Dispose();
                    }
                }
                dsSearch.Tables.Add(dv.ToTable());
                lblTotalRowCount.Text = " Total Request(s) From Ward = " + common.myStr(dsSearch.Tables[0].Rows.Count);
                gvEncounter.CurrentPageIndex = 0;
                dv.Dispose();

                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    gvEncounter.DataSource = dsSearch.Tables[0];
                }
                else
                {
                    DataRow DR = dsSearch.Tables[0].NewRow();
                    dsSearch.Tables[0].Rows.Add(DR);

                    gvEncounter.DataSource = dsSearch.Tables[0];
                }
                gvEncounter.DataBind();
                ds.Dispose(); dsSearch.Dispose();
            }
            else
            {
                gvEncounter.DataSource = null;
                gvEncounter.Visible = false;
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "No Record Found";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Visible = true;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        tblDate.Visible = false;
        if (ddlTime.SelectedValue == "DateRange")
        {
            tblDate.Visible = true;
        }
    }

    void setDate()
    {
        try
        {
            switch (common.myStr(ddlTime.SelectedValue))
            {
                case "All":
                    txtFromDate.SelectedDate = common.myDate("2000-01-01");
                    txtToDate.SelectedDate = common.myDate("2099-12-31");
                    break;
                case "Today":
                    txtFromDate.SelectedDate = DateTime.Now;
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastWeek":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastTwoWeeks":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-14);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastOneMonth":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastThreeMonths":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastYear":
                    txtFromDate.SelectedDate = DateTime.Now.AddYears(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "DateRange":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-1);
                    txtToDate.SelectedDate = DateTime.Now; ;
                    break;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }


    protected void ddlSearchOn_OnTextChanged(object sender, EventArgs e)
    {
        txtSearchRegNo.Text = "";
        txtSearch.Text = "";
        ddlWard.Visible = false;
        ddlWard.SelectedIndex = 0;
        if (common.myStr(ddlSearchOn.SelectedValue) == "2")
        {
            txtSearch.Visible = false;
            txtSearchRegNo.Visible = true;
        }
        else if (common.myStr(ddlSearchOn.SelectedValue) == "5")
        {
            txtSearch.Visible = false;
            txtSearchRegNo.Visible = false;
            ddlWard.Visible = true;
            BaseC.ATD objadt = new BaseC.ATD(sConString);
            if (ViewState["WardData"] == null)
            {
                ViewState["WardData"] = objadt.GetWard(common.myInt(Session["FacilityId"]));
            }
            DataSet ds = (DataSet)ViewState["WardData"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlWard.DataSource = ds.Tables[0];
                ddlWard.DataTextField = "WardName";
                ddlWard.DataValueField = "WardId";
                ddlWard.DataBind();
                ddlWard.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
                ddlWard.SelectedIndex = 0;
            }
            objadt = null;
            ds.Dispose();
        }
        else
        {
            txtSearch.Visible = true;
            txtSearchRegNo.Visible = false;
        }

    }


    protected void ddlStore_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //try
        //{
        //    Session["StoreId"] = common.myInt(ddlStore.SelectedValue);
        //    bindData();
        //}
        //catch
        //{

        //}
    }
}
