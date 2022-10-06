using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Drawing;
public partial class OTScheduler_OTRequestList : System.Web.UI.Page
{
    HiddenField hdnOTBookingID, hdnOTBookingNo, hdnOTRequestID;
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
   

    System.Drawing.Color ColorBooked, ColorRequest;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            populatColorList();
            txtSearchN.Visible = false;
            txtSearch.Visible = false;

            txtFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            txtToDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);

            txtFromDate.SelectedDate = DateTime.Now;
            txtToDate.SelectedDate = DateTime.Now.AddDays(7);

            if (common.myStr(ddlName.SelectedValue) == "R")
            {
                txtSearchN.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
            }
            BindFacility();
            bindData();
        }
        Session["SelectReq"] = null;
        Legend1.loadLegend("OTR", "");
    }

    void BindFacility()
    {
        BaseC.User valUser = new BaseC.User(sConString);
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
            ddlFacility.DataSource = ds.Tables[0];
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();

            if (common.myStr(Session["ModuleName"]) == "EMR" || common.myStr(Session["ModuleName"]) == "Ward Management")
            {
                ddlFacility.Enabled = false;
                ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
            }
            else if (common.myStr(Session["ModuleName"]) == "OT")
            {
                ddlFacility.SelectedIndex = 0;
            }
        }
        catch 
        {

        }
        finally
        {
            valUser = null;
            objMaster = null;
            ds.Dispose();
        }
    }
    private void populatColorList()
    {
        try
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet Colordt = new DataSet();
            string strsql = "select Status,StatusColor, StatusId, Code from GetStatus(" + common.myInt(Session["HospitalLocationId"]) + ",'OTR') order by SequenceNo asc";
            Colordt = objDl.FillDataSet(CommandType.Text, strsql);

            if (Colordt.Tables[0].Rows.Count > 0)
            {
                string hexBook = Colordt.Tables[0].Rows[0].ItemArray[1].ToString();
                string hexReq = Colordt.Tables[0].Rows[1].ItemArray[1].ToString();
                ColorBooked = System.Drawing.ColorTranslator.FromHtml(hexBook);
                ColorRequest = System.Drawing.ColorTranslator.FromHtml(hexReq);
                ViewState["ColorBooked"] = ColorBooked;
                ViewState["ColorRequest"] = ColorRequest;
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;

        }
    }

    protected void gvOTRequestList_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvOTRequestList.CurrentPageIndex = e.NewPageIndex;
        bindData();
    }

    protected void gvOTRequestList_PreRender(object sender, EventArgs e)
    {
        gvOTRequestList.DataSource = (DataTable)ViewState["OTRequestList"];
        gvOTRequestList.DataBind();
    }

    protected void gvOTRequestList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Book")
            {
                hdnRegistrationId.Value = common.myStr(((Label)e.Item.FindControl("lblREGID")).Text);
                hdnRegistrationNo.Value = common.myStr(((Label)e.Item.FindControl("lblRegistrationNo")).Text);
                hdnEncounterId.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterID")).Text);
                hdnEncounterNo.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", "");
                hdnDoctorId.Value = common.myStr(((Label)e.Item.FindControl("lblDoctorId")).Text);
                hdnAgeGender.Value = common.myStr(((Label)e.Item.FindControl("lblAgeGender")).Text);
                hdnMobileNo.Value = common.myStr(((Label)e.Item.FindControl("lblMobileNo")).Text);
                hdnPatientName.Value = common.myStr(((Label)e.Item.FindControl("lblName")).Text);
                hdnOTRequestID = (HiddenField)e.Item.FindControl("hdnOTRequestID");
                HiddenField hdnIsInsuranceApprovalDone = (HiddenField)e.Item.FindControl("hdnIsInsuranceApprovalDone");
                string OTRequestID = common.myStr(hdnOTRequestID.Value);

                hdnOTBookingNo = (HiddenField)e.Item.FindControl("hdnOTBookingNo");
                ViewState["hdnOTBookingNo"] = common.myStr(hdnOTBookingNo.Value);

                if (!string.IsNullOrWhiteSpace(common.myStr(ViewState["hdnOTBookingNo"])))
                {
                    Alert.ShowAjaxMsg("OT Booking is Done And Booking No is " + common.myStr(ViewState["hdnOTBookingNo"]), Page);
                    return;
                }
                else if(!common.myBool(hdnIsInsuranceApprovalDone.Value))
                {
                    Alert.ShowAjaxMsg("Insurance clearance not done. Can not proceed...", Page);
                    return;
                }
                RadWindowForNew.NavigateUrl = "/OTScheduler/OTBooking.aspx?iframe=Y&Regid=" + hdnRegistrationId.Value
                    + "&RegNo=" + hdnRegistrationNo.Value + "&EncId=" + hdnEncounterId.Value
                    + "&EncNo=" + hdnEncounterNo.Value + "&otr=" + hdnOTRequestID.Value;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else if (e.CommandName == "Consent")
            {
                
                Session["RegistrationNo"] = common.myStr(((Label)e.Item.FindControl("lblRegistrationNo")).Text);
                Session["OTRequestID"] = ((HiddenField)e.Item.FindControl("hdnOTRequestID")).Value;

                RadWindowForNew.NavigateUrl = "/EMR/Templates/TemplateNotesPrint.aspx?From=POPUP";
                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                //RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else if (e.CommandName == "Select")
            {

                hdnOTBookingNo = (HiddenField)e.Item.FindControl("hdnOTBookingNo");
                ViewState["hdnOTBookingNo"] = common.myStr(hdnOTBookingNo.Value);
                HiddenField hdnIsInsuranceApprovalDone = (HiddenField)e.Item.FindControl("hdnIsInsuranceApprovalDone");
                if (!string.IsNullOrWhiteSpace(common.myStr(ViewState["hdnOTBookingNo"])))
                {
                    Alert.ShowAjaxMsg("OT Booking is Done And Booking No is " + common.myStr(ViewState["hdnOTBookingNo"]), Page);
                    return;
                }
                else if (!common.myBool(hdnIsInsuranceApprovalDone.Value))
                {
                    Alert.ShowAjaxMsg("Insurance clearance not done. Can not proceed...", Page);
                    return;
                }
                if (common.myInt(((Label)e.Item.FindControl("lblREGID")).Text) > 0)
                {
                    hdnRegistrationId.Value = common.myStr(((Label)e.Item.FindControl("lblREGID")).Text);
                    hdnRegistrationNo.Value = common.myStr(((Label)e.Item.FindControl("lblRegistrationNo")).Text);
                    hdnEncounterId.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterID")).Text);
                    hdnEncounterNo.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", "");
                    hdnDoctorId.Value = common.myStr(((Label)e.Item.FindControl("lblDoctorId")).Text);
                    hdnAgeGender.Value = common.myStr(((Label)e.Item.FindControl("lblAgeGender")).Text);
                    hdnMobileNo.Value = common.myStr(((Label)e.Item.FindControl("lblMobileNo")).Text);
                    hdnPatientName.Value = common.myStr(((Label)e.Item.FindControl("lblName")).Text);
                    hdnOTRequestID = (HiddenField)e.Item.FindControl("hdnOTRequestID");
                    string OTRequestID = common.myStr(hdnOTRequestID.Value);

                    Session["RegistrationId"] = hdnRegistrationId.Value;
                    Session["RegistrationNo"] = hdnRegistrationNo.Value;
                    Session["EncounterId"] = hdnEncounterId.Value;
                    Session["EncounterNo"] = hdnEncounterNo.Value;
                    Session["OTRequestID"] = OTRequestID;
                    Session["SelectReq"] = 1;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    return;

                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);

                    //int facilityid = common.myInt(Session["FacilityId"]);
                    //RadWindowForNew.NavigateUrl = "~/OTScheduler/OTBooking.aspx?FacilityId=" + facilityid + "&PageId=" + Request.QueryString["Mpg"]
                    //    + "&Regid=" + hdnRegistrationId.Value + "&RegNo=" + hdnRegistrationNo.Value + "&EncId=" + hdnEncounterId.Value + "&EncNo="
                    //    + hdnEncounterNo.Value + "&OTReqID=" + hdnOTRequestID.Value;
                    //RadWindowForNew.Height = 630;
                    //RadWindowForNew.Width = 1000;
                    //RadWindowForNew.Top = 40;
                    //RadWindowForNew.Left = 100;
                    //RadWindowForNew.OnClientClose = "OnClientClose";
                    //RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindowForNew.Modal = true;
                    //RadWindowForNew.VisibleStatusbar = false;
                    //RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;

                    //return;
                }
            }
            else
                Session["SelectReq"] = 0;

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void gvOTRequestList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP"))
            {
                gvOTRequestList.Columns.FindByUniqueName("Select").Visible = true;
                gvOTRequestList.Columns.FindByUniqueName("Book").Visible = false;
            }
            else
            {
                gvOTRequestList.Columns.FindByUniqueName("Select").Visible = false;
                gvOTRequestList.Columns.FindByUniqueName("Book").Visible = true;
            }
        }

        if (e.Item is GridDataItem)
        {

            GridDataItem dataItem = e.Item as GridDataItem;
            //  HiddenField hdnStatus = (HiddenField)e.Item.FindControl("hdnStatus");
            hdnOTBookingID = (HiddenField)e.Item.FindControl("hdnOTBookingID");
            hdnOTBookingNo = (HiddenField)e.Item.FindControl("hdnOTBookingNo");
            ViewState["hdnOTBookingNo"] = common.myStr(hdnOTBookingNo.Value);

            if (!string.IsNullOrEmpty((common.myStr(hdnOTBookingNo.Value))))
            {
                e.Item.BackColor = (Color)ViewState["ColorBooked"];  //System.Drawing.Color.LightGreen ;
                LinkButton btn = (LinkButton)dataItem["Select"].Controls[0];
                LinkButton btn1 = (LinkButton)dataItem["Book"].Controls[0];
                // btn.Enabled = false;
                btn.ForeColor = System.Drawing.Color.Red;
                btn1.ForeColor = System.Drawing.Color.Red;
            }
            if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP"))
            {
                gvOTRequestList.Columns.FindByUniqueName("Select").Visible = true;
            }
            else
            {
                gvOTRequestList.Columns.FindByUniqueName("Select").Visible = false;
            }
        }
    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {

    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        bindData();
    }
    private DataTable BindBlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("OTRequestID");
        dt.Columns.Add("IsEmergency");
        dt.Columns.Add("RegistrationId");
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("EncounterId");
        dt.Columns.Add("EncounterNo");
        dt.Columns.Add("Name");
        dt.Columns.Add("DateOfBirth");
        dt.Columns.Add("AgeGender");
        dt.Columns.Add("DoctorID");
        dt.Columns.Add("PaediatricianDoctor");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("MobileNo");
        dt.Columns.Add("ServiceName");
        dt.Columns.Add("Diagnosis");
        dt.Columns.Add("OTRequestDate");
        dt.Columns.Add("OTBookingDate");
        dt.Columns.Add("FromTime");
        dt.Columns.Add("Duration");
        dt.Columns.Add("OTDurationValue");
        dt.Columns.Add("OTBookingID");
        dt.Columns.Add("OTBookingNo");
        dt.Columns.Add("BookingDate");
        dt.Columns.Add("TheatreName");
        dt.Columns.Add("ServiceRemarks");
        dt.Columns.Add("Facility");
        dt.Columns.Add("ProvisionalDiagnosis");
        dt.Columns.Add("OTDurationType");
        dt.Columns.Add("PACRequired");
        dt.Columns.Add("PACClearanceBy");
        dt.Columns.Add("FitForSurgery");
        dt.Columns.Add("PACRemarks");
        dt.Columns.Add("PACEmp");
        dt.Columns.Add("ShortProvisionalDiagnosis");
        dt.Columns.Add("InsuranceApproval");
        dt.Columns.Add("IsInsuranceApprovalDone");
        DataRow dr = dt.NewRow();

        dr["OTRequestID"] = null;
        dr["IsEmergency"] = null;
        dr["RegistrationId"] = null;
        dr["RegistrationNo"]=null;
        dr["EncounterId"] = null;
        dr["EncounterNo"] = null;
        dr["Name"] = null;
        dr["DateOfBirth"] = null;
        dr["AgeGender"] = null;
        dr["DoctorID"] = null;
        dr["PaediatricianDoctor"] = null;
        dr["DoctorName"] = null;
        dr["MobileNo"] = null;
        dr["ServiceName"] = null;
        dr["Diagnosis"] = null;
        dr["OTRequestDate"] = null;
        dr["OTBookingDate"] = null;
        dr["FromTime"] = null;
        dr["Duration"] = null;
        dr["OTDurationValue"] = null;
        dr["OTBookingID"] = null;
        dr["OTBookingNo"] = null;
        dr["BookingDate"] = null;
        dr["TheatreName"] = null;
        dr["ServiceRemarks"] = null;
        dr["Facility"] = null;
        dr["ProvisionalDiagnosis"] = null;
        dr["OTDurationType"] = null;
        dr["PACRequired"] = null;
        dr["PACClearanceBy"] = null;
        dr["FitForSurgery"] = null;
        dr["PACRemarks"] = null;
        dr["PACEmp"] = null;
        dr["IsInsuranceApprovalDone"] = null;
        dt.Rows.Add(dr);
        return dt;
    }
    private void bindData()

    {
        DataSet dsSearch = new DataSet();
        BaseC.clsEMRBilling objVal = new BaseC.clsEMRBilling(sConString);
        try
        {
            int userid = common.myInt(Session["UserId"]);

            string chvRegistrationNo = "", chvEncounterNo = "", chvPatientName = "";

            if (common.myStr(ddlName.SelectedValue) == "R")
            {
                chvRegistrationNo = txtSearchN.Text;
            }
            else if (common.myStr(ddlName.SelectedValue) == "ENC")
            {
                chvEncounterNo = txtSearch.Text;
            }
            else if (common.myStr(ddlName.SelectedValue) == "N")
            {
                chvPatientName = txtSearch.Text;
            }

            dsSearch = objVal.GetOTRequestList(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue), userid,
                chvRegistrationNo, chvEncounterNo, chvPatientName,common.myDate(txtFromDate.SelectedDate).ToString("yyyy-MM-dd"), 
                common.myDate(txtToDate.SelectedDate).ToString("yyyy-MM-dd"));
            if (dsSearch.Tables.Count > 0)
            {
                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    DataView dv = new DataView(dsSearch.Tables[0]);
                    if (common.myStr(ddlOTBookingStatus.SelectedValue) == "B")
                    {
                        dv.RowFilter = "OTBookingID>0";
                    }
                    else if (common.myStr(ddlOTBookingStatus.SelectedValue) == "P")
                    {
                        dv.RowFilter = "OTBookingID=0";
                    }
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        lblRecord.Visible = false;
                        gvOTRequestList.DataSource = dv.ToTable();
                        gvOTRequestList.DataBind();
                        ViewState["OTRequestList"] = dv.ToTable();
                    }
                    else
                    {
                        lblRecord.Visible = true;
                        lblRecord.Text = "No Records.";
                        lblRecord.Font.Bold = true;
                        lblRecord.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        ViewState["OTRequestList"] = null;
                        gvOTRequestList.DataSource = null;
                        gvOTRequestList.DataBind();

                        gvOTRequestList.DataSource = BindBlankGrid();
                        gvOTRequestList.DataBind();
                    }
                    dv.Dispose();
                }
                else
                {
                    ViewState["OTRequestList"] = null;
                    gvOTRequestList.DataSource = null;
                    gvOTRequestList.DataBind();
                    gvOTRequestList.DataSource = BindBlankGrid();
                    gvOTRequestList.DataBind();
                    
                    
                }
            }
            else
            {
                ViewState["OTRequestList"] = null;
                gvOTRequestList.DataSource = null;
                gvOTRequestList.DataBind();
                gvOTRequestList.DataSource = BindBlankGrid();
                gvOTRequestList.DataBind();
                
               
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { dsSearch.Dispose(); objVal = null; }
    }
    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        txtSearchN.Text = "";

        txtSearchN.Visible = false;
        txtSearch.Visible = false;

        if (common.myStr(ddlName.SelectedValue) == "R")
        {
            txtSearchN.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
        }
        // bindStatus(rblSearchCriteria.SelectedValue);
    }
}