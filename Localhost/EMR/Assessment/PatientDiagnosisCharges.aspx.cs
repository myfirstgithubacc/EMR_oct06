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
using System.Text;
using System.Data.SqlClient;
using Telerik.Web.UI;

public partial class EMR_Assessment_PatientDiagnosisCharges : System.Web.UI.Page
{
    private enum GridAddServices : byte
    {
        Id = 0,
        ServiceID = 1,
        CPTCode = 2,
        ServiceName = 3,
        ModifierCode = 4,
        Units = 5,
        UnitAmount = 6,
        ICDCodes = 7,
        FromDate = 8,
        ToDate = 9,
        IsBillable = 10,
        Edit = 11,
        Delete = 12
    }
    string pageId = "";
    string strEncounterDate = "";
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (Request.QueryString["Mpg"] != null)
            {
                pageId = Request.QueryString["Mpg"];
            }


            if (!IsPostBack)
            {
                hdnGridClientId.Value = icd.GridClientId;
                ClearServiceDetailControls();
                Hashtable hshin = new Hashtable();
                rdpFrom.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                rdpTo.DateInput.DateFormat = Session["OutputDateFormat"].ToString();


                Hashtable hshInput = new Hashtable();
                hshInput.Add("@intEncounterID", Convert.ToInt32(Session["EncounterId"]));
                hshInput.Add("@intHospitalLocationID", Convert.ToInt32(Session["HospitalLocationId"]));
                string strEncounter = "select CONVERT(varchar(10), EncodedDate,120) as EncounterDate from encounter where ID=@intEncounterID and HospitalLocationId=@intHospitalLocationID";
                DataSet ds = dl.FillDataSet(CommandType.Text, strEncounter, hshInput);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strEncounterDate = ds.Tables[0].Rows[0]["EncounterDate"].ToString();
                }
                rdpFrom.SelectedDate = Convert.ToDateTime(strEncounterDate);
                rdpTo.SelectedDate = Convert.ToDateTime(strEncounterDate);
                //rdpFrom.SelectedDate = DateTime.Now;
                //rdpTo.SelectedDate = DateTime.Now;               
                // btnAddToList.Attributes.Add("OnClick", "return ValidateFromDate();");
                Cache.Remove("ServiceDetail");
                BindAddServiceGrid();
                ltrlInvSetName.Visible = false;
                ddlInvSetName.Visible = false;
                BindCategoryDDL();
                BindDropDownSetName();
                ViewState["BTN"] = "FAV";
                BindFavourtite();
                //btnSearch_Click(this, null);
                //PopulateDoctor();
                //fillConsultant(ddlSpecialisation.SelectedValue);
                getCurrentICDCodes();

                ModifierTable();
                ClearServiceDetailControls();
                // hdnCurrDate.Value = DateTime.Today.ToString("dd/MM/yyyy");
                BindICDPanel();
                if (Request.QueryString["ID"] != null)
                {
                    Int32 iId = 0;
                    iId = Convert.ToInt32(Request.QueryString["ID"]);
                    if (iId != 0)
                    {
                        FillAddChargesPageControlsById(iId);
                    }
                }

                DataSet dsCompany = new DataSet();
                hshin.Add("@intEncounterId", Session["EncounterId"]);
                hshin.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
                Hashtable hshOutput = new Hashtable();
                hshOutput.Add("intCompanyId", SqlDbType.Int);
                hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "GetPatientCompanyCode", hshin, hshOutput);
                if (hshOutput["intCompanyId"].ToString() != "")
                {
                    ViewState["CompanyCode"] = hshOutput["intCompanyId"].ToString();
                }
                else
                {
                    ViewState["CompanyCode"] = 0;
                }
            }
        }
        catch
        { }
    }

    private void FillAddChargesPageControlsById(Int32 iID)
    {
        try
        {
            DataTable dtServiceDetails = (DataTable)Cache["ServiceDetail"];
            DataView dv = new DataView(dtServiceDetails);
            if (dv.Table != null)
            {
                dv.RowFilter = "Id=" + iID;
                DataTable dt = new DataTable();
                dt = dv.ToTable();
                if (dt.Rows.Count > 0)
                {
                    ViewState["ServiceId"] = dt.Rows[0]["ServiceID"].ToString();
                    txtCPT.Text = dt.Rows[0]["CPTCode"].ToString();
                    txtDescription.Text = dt.Rows[0]["ServiceName"].ToString();
                    txtModifier.Text = dt.Rows[0]["ModifierCode"].ToString();
                    txtUnit.Text = dt.Rows[0]["Units"].ToString();
                    // txtUnitCharge.Text = (Convert.ToDouble(dt.Rows[0]["ServiceAmount"]) + Convert.ToDouble(dt.Rows[0]["DoctorAmount"]) - (Convert.ToDouble(dt.Rows[0]["DoctorDiscountAmount"]) + Convert.ToDouble(dt.Rows[0]["ServiceDiscountAmount"]))).ToString();
                    txtUnitCharge.Text = (Convert.ToDecimal(dt.Rows[0]["ServiceAmount"]) + Convert.ToDecimal(dt.Rows[0]["DoctorAmount"]) - (Convert.ToDecimal(dt.Rows[0]["DoctorDiscountAmount"]) + Convert.ToDecimal(dt.Rows[0]["ServiceDiscountAmount"]))).ToString();
                    hdnServiceAmount.Value = dt.Rows[0]["ServiceAmount"].ToString();
                    hdnDoctorAmount.Value = dt.Rows[0]["DoctorAmount"].ToString();
                    hdnDoctorDiscountAmount.Value = dt.Rows[0]["DoctorDiscountAmount"].ToString();
                    hdnServiceDiscountAmount.Value = dt.Rows[0]["ServiceDiscountAmount"].ToString();
                    hdnID.Value = iID.ToString();
                    txtICDCode.Text = dt.Rows[0]["ICDID"].ToString();

                    // txtFrom.Text = dt.Rows[0]["FromDate"].ToString();
                    // txtTo.Text = dt.Rows[0]["ToDate"].ToString();
                    rdpFrom.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FromDate"].ToString());
                    rdpTo.SelectedDate = Convert.ToDateTime(dt.Rows[0]["ToDate"].ToString());
                    chkIsBillable.Checked = dt.Rows[0]["IsBillable"].ToString().ToLower() == "true" ? true : false;
                    btnAddToList.Text = "Update List";
                }
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //*******************************Page Events**********************************************
    #region // Page Events
    protected void btnPast_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["BTN"] = "DOC";
            PopulatePastService();
        }
        catch
        { }
    }
    protected void btnAll_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["BTN"] = "ALL";
            PopulateAllService();
        }
        catch
        {
        }
    }
    protected void btnPastPatient_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["BTN"] = "PAT";
            PopulatePastPatientService();
        }
        catch
        {
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //To Do
            ltrlInvSetName.Visible = false;
            ddlInvSetName.Visible = false;
            ltrlInvCategory.Visible = true;
            ddlSubDepartment.Visible = true;
            //BindServiceGrid(1);
            if (Convert.ToString(ViewState["BTN"]) == "ALL")
            {
                PopulateAllService();
            }
            else if (Convert.ToString(ViewState["BTN"]) == "DOC")
            {
                PopulatePastService();
            }
            else if (Convert.ToString(ViewState["BTN"]) == "PAT")
            {
                PopulatePastPatientService();
            }
            else if (Convert.ToString(ViewState["BTN"]) == "FAV")
            {
                BindFavourtite();
            }
            else if (Convert.ToString(ViewState["BTN"]) == "ORDER")
            {
                ddlInvSetName_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "window.close();", true);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["facilityId"] != null)
            {
                DL_Funs oDLF = new DL_Funs();
                BaseC.Patient bc = new BaseC.Patient(sConString);
                if (Cache["ServiceDetail"] == null)
                {
                    Alert.ShowAjaxMsg("Please add service list", this.Page);
                    return;
                }
                else
                {
                    BaseC.EMROrders objEmrOrders = new BaseC.EMROrders(sConString);
                    StringBuilder objXML = new StringBuilder();
                    DataTable SelectedServicDetails = (DataTable)Cache["ServiceDetail"];
                    foreach (DataRow datarow in SelectedServicDetails.Rows)
                    {

                        objXML.Append("<Table><c1>");
                        objXML.Append(datarow["ServiceId"].ToString());
                        objXML.Append("</c1><c2>");
                        objXML.Append(datarow["ICDID"]);
                        objXML.Append("</c2><c3>");
                        objXML.Append(0);
                        objXML.Append("</c3><c4>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c4><c5>");
                        objXML.Append(bc.FormatDate(Convert.ToDateTime(datarow["FromDate"]).Date.ToShortDateString(), Session["OutputDateFormat"].ToString(), "dd/MM/yyyy"));                        //objXML.Append(datarow["FromDate"].ToString());
                        objXML.Append("</c5><c6>");
                        objXML.Append(bc.FormatDate(Convert.ToDateTime(datarow["ToDate"]).Date.ToShortDateString(), Session["OutputDateFormat"].ToString(), "dd/MM/yyyy"));

                        // objXML.Append(datarow["ToDate"].ToString());
                        objXML.Append("</c6><c7>");
                        objXML.Append(datarow["Units"]);
                        objXML.Append("</c7><c8>");
                        objXML.Append(Convert.ToString(Session["DoctorId"]));
                        objXML.Append("</c8><c9>");
                        objXML.Append(Convert.ToString(Session["FacilityId"]));
                        objXML.Append("</c9><c10>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c10><c11>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c11><c12>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c12><c13>");
                        objXML.Append(0);
                        objXML.Append("</c13><c14>");

                        objXML.Append(0);

                        objXML.Append("</c14><c15>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c15><c16>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c16><c17>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c17><c18>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c18><c19>");

                        objXML.Append(0);

                        objXML.Append("</c19><c20>");

                        objXML.Append(0);

                        objXML.Append("</c20><c21>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c21><c22>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c22><c23>");
                        objXML.Append(datarow["Id"]);
                        objXML.Append("</c23><c24>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c24><c25>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c25><c26>");
                        objXML.Append(string.Empty);
                        objXML.Append("</c26><c27>");
                        objXML.Append(datarow["ModifierCode"]);
                        objXML.Append("</c27><c28>");


                        if (datarow["IsBillable"].ToString().ToUpper() == "TRUE")
                        {
                            objXML.Append(1);
                        }
                        else
                        {
                            objXML.Append(0);
                        }
                        objXML.Append("</c28>");
                        TextBox txt = new TextBox();
                        BindCharge(Convert.ToInt32(datarow["ServiceId"]), txt);

                        if (txt.Text.Trim() != "")
                        {
                            objXML.Append("<c29>");
                            //objXML.Append(txt.Text);
                            objXML.Append(hdnServiceAmount.Value);
                            objXML.Append("</c29>");
                        }
                        else
                        {
                            objXML.Append("<c29>");
                            objXML.Append(datarow["ServiceAmount"]);
                            objXML.Append("</c29>");
                        }

                        if (pageId != "")
                        {
                            pageId = pageId.Substring(1, pageId.Length - 1);
                        }
                        else
                            pageId = "0";

                        objXML.Append("<c30>");
                        objXML.Append(datarow["ServiceDiscountAmount"]);
                        objXML.Append("</c30>");
                        objXML.Append("<c31>");
                        objXML.Append(datarow["DoctorAmount"]);
                        objXML.Append("</c31>");
                        objXML.Append("<c32>");
                        objXML.Append(datarow["DoctorDiscountAmount"]);
                        objXML.Append("</c32>");
                        objXML.Append("</Table>");
                    }
                    //String strMsg = (string)objEmrOrders.SaveOPOrder(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"].ToString()), Convert.ToInt32(Session["DoctorID"].ToString()), Convert.ToInt16(Session["HospitalLocationId"]), DateTime.Now.ToString("MM/dd/yyyy"), objXML.ToString(), Convert.ToInt32(Session["UserId"]), false, Session["facilityId"].ToString(), pageId,false);
                    // Add OutPut Par By Ram
                    string sOrderID = "";
                    String strMsg = (string)objEmrOrders.SaveOPOrder(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"].ToString()), Convert.ToInt32(Session["DoctorID"].ToString()), Convert.ToInt16(Session["HospitalLocationId"]), DateTime.Now.ToString("MM/dd/yyyy"), objXML.ToString(), Convert.ToInt32(Session["UserId"]), false, Session["facilityId"].ToString(), pageId, false, out sOrderID, 0);

                    if (strMsg == "Saved")
                    {
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "RefreshParentPage();", true);
                        //Telerik.Web.UI.RadWindow obj =(RadWindow )Parent .Page .FindControl ("Radwindow1");
                        // RadWindowManager obj = (RadWindowManager)Page.FindControl("RadWindowManager1");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CloseWindow", "Close();", true);
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Some Problem has encountered,please try again.", this.Page);
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlSubDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindServiceGrid(0);
            ViewState["BTN"] = "ALL";
            //gvServices.HeaderRow.Cells[2].Text = "All Services Name";
        }
        catch
        { }
    }
    protected void gvServices_OnPageIndexChanging(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        try
        {
            if (ViewState["BTN"].ToString() == "FAV")
            {
                BindFavourtite();
            }
            else if (ViewState["BTN"].ToString() == "DOC")
            {
                PopulatePastService();
            }
            else if (txtSearch.Text == "")
            {
                BindServiceGrid(0);
            }
            else
            {
                BindServiceGrid(1);
            }
        }
        catch { }
    }
    protected void gvServices_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtDescription.Text = ((Label)(gvServices.SelectedItems[0].FindControl("lblServiceName"))).Text;
            ViewState["ServiceId"] = ((HiddenField)(gvServices.SelectedItems[0].FindControl("hdnServiceID"))).Value;
            txtCPT.Text = ((Label)gvServices.SelectedItems[0].FindControl("lblCPTCode")).Text;
            if (hdnModifierID.Value != "" && hdnModifierID.Value != "0")
            {
                txtModifier.Text = hdnModifierID.Value;
            }
            else
            {
                txtModifier.Text = "";
            }
            txtSearch.Text = String.Empty;
            // txtUnit.Text = String.Empty;
            txtUnitCharge.Text = String.Empty;
            hdnID.Value = "0";
            //rdpFrom.SelectedDate = DateTime.Now;
            //rdpTo.SelectedDate = DateTime.Now;
            chkIsBillable.Checked = true;
            btnAddToList.Text = "Add To List";
            BindCharge(Convert.ToInt32(ViewState["ServiceId"]), txtUnitCharge);
            //BindCharge(Convert.ToInt32(ViewState["ServiceId"]), ddlSpecialisation);
            //BindCharge(Convert.ToInt32(ViewState["ServiceId"]), txtUnitCharge);
            //if (txtICDCode.Text == "")
            //{
            //    if (cblICDCodes.Items.Count > 0)
            //    {
            //        txtICDCode.Focus();
            //    }
            //}
            if (gvAddService.SelectedItems != null)
            {
                gvAddService.SelectedIndexes.Clear();
            }
        }
        catch (Exception)
        { }
    }

    private string getFacilityIdbyEncounterId()
    {
        try
        {
            if (Session["EncounterId"] != null)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hsInput = new Hashtable();
                hsInput.Add("EncounterId", Session["EncounterId"].ToString());
                string FacilityId = (string)objDl.ExecuteScalar(CommandType.Text, "select Convert(varchar,FacilityID) FacilityId from Encounter where Id=@EncounterId", hsInput);
                return FacilityId;
            }
            else
                return "0";
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return "0";
        }
    }

    private void BindCharge(Int32 iServiceId, TextBox txt1)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            hshIn.Add("@intRegistrationID", Convert.ToInt32(Session["registrationID"]));
            hshIn.Add("@intServiceId", iServiceId);
            hshIn.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));
            hshIn.Add("@intCompanyid", Convert.ToInt16(ViewState["CompanyCode"]));
            hshIn.Add("@intSpecialisationId", Convert.ToInt32("0"));
            hshIn.Add("@intDoctorId", Convert.ToInt32("0"));
            hshIn.Add("@chvModifierCode", txtModifier.Text);
            hshIn.Add("@intFacilityId", getFacilityIdbyEncounterId());
            hshOut.Add("@NChr", SqlDbType.Money);
            hshOut.Add("@DNchr", SqlDbType.Money);
            hshOut.Add("@DiscountNAmt", SqlDbType.Money);
            hshOut.Add("@DiscountDNAmt", SqlDbType.Money);
            hshOut.Add("@PatientNPayable ", SqlDbType.Money);
            hshOut.Add("@PayorNPayable", SqlDbType.Money);
            hshOut.Add("@DiscountPerc", SqlDbType.Money);
            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetServiceChargeOPOneService", hshIn, hshOut);
            txt1.Text = (Convert.ToDouble(hshOut["@NChr"]) + Convert.ToDouble(hshOut["@DNchr"]) - (Convert.ToDouble(hshOut["@DiscountDNAmt"]) + Convert.ToDouble(hshOut["@DiscountNAmt"]))).ToString();
            hdnServiceAmount.Value = hshOut["@NChr"].ToString();
            hdnDoctorAmount.Value = hshOut["@DNchr"].ToString();
            hdnDoctorDiscountAmount.Value = hshOut["@DiscountDNAmt"].ToString();
            hdnServiceDiscountAmount.Value = hshOut["@DiscountNAmt"].ToString();

            if (Convert.ToDouble(txt1.Text) == 0)
            {
                txt1.Text = string.Empty;
                txtUnitCharge.ReadOnly = false;
            }
            else
                txtUnitCharge.ReadOnly = true;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvServices_OnRowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            //if (e.Row.RowType != DataControlRowType.Pager)
            //{
            //    e.Row.Cells[0].Visible = false;
            //    e.Row.Cells[3].Visible = false;
            //}
            //if (e.Item.ItemType == GridItemType.Item)
            //{
            //    if (((HiddenField)e.Item.Cells[0].FindControl("hdnServiceID")).Value == "")
            //    {
            //        LinkButton lb = ((LinkButton)e.Item.Cells[4].Controls[0]);
            //        lb.Enabled = false;
            //    }
            //    Label LongDescription = (Label)e.Row.FindControl("lblServiceNameLongDescription");
            //    if (LongDescription.Text != "")
            //    {
            //        e.Row.Cells[2].ToolTip = LongDescription.Text;
            //        e.Row.Cells[1].ToolTip = LongDescription.Text;
            //    }

            //    e.Row.Attributes["ondblclick"] = ClientScript.GetPostBackClientHyperlink(this.gvServices, "Select$" + e.Row.RowIndex);
            //}
            if (e.Item.ItemType == GridItemType.Header)
            {
                if (Convert.ToString(ViewState["BTN"]) == "ALL")
                {
                    e.Item.Cells[3].Text = "All Services Name";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "DOC")
                {
                    e.Item.Cells[3].Text = "Past Services Name";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "PAT")
                {
                    e.Item.Cells[3].Text = "Past Patient Services Name";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "FAV")
                {
                    e.Item.Cells[3].Text = "Favorite Services Name";
                }
                else if (Convert.ToString(ViewState["BTN"]) == "ORDER")
                {
                    e.Item.Cells[3].Text = "Order Set Services Name";
                }
            }
            //if (e.Row.RowState == DataControlRowState.Selected)
            //{
            //    e.Row.Cells[2].Text = System.Drawing.KnownColor.Aqua.ToString();
            //}
        }
        catch
        { }
    }
    protected void btnOrderSets_Click(object sender, EventArgs e)
    {
        if (Session["DoctorID"] != null)
        {
            try
            {
                ltrlInvSetName.Visible = true;
                ddlInvSetName.Visible = true;
                ltrlInvCategory.Visible = false;
                ddlSubDepartment.Visible = false;
                ddlInvSetName_SelectedIndexChanged(sender, e);
            }
            catch (Exception Ex)
            {
                lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lbl_Msg.Text = "Error: " + Ex.Message;
                objException.HandleException(Ex);
            }
        }
    }
    protected void ddlInvSetName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["BTN"] = "ORDER";
            Hashtable hstInput = new Hashtable();
            hstInput.Add("@intSetId", ddlInvSetName.SelectedValue);

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetOrderSetDetails", hstInput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv;
                if (txtSearch.Text.Trim() != "")
                {
                    string Service = "%%";
                    if (ddlSearchCriteria.SelectedValue == "1")//any where
                        Service = "%" + txtSearch.Text.Trim() + "%";
                    if (ddlSearchCriteria.SelectedValue == "2")//starts with
                        Service = txtSearch.Text.Trim() + "%";
                    if (ddlSearchCriteria.SelectedValue == "3")//ends with
                        Service = "%" + txtSearch.Text.Trim();
                    dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "ServiceName like '" + Service + "'";
                    DataTable objDt = dv.ToTable();
                    if (objDt.Rows.Count > 0)
                    {
                        gvServices.DataSource = objDt;
                        gvServices.DataBind();
                    }
                    else
                    {
                        BindBlankServiceGrid();
                    }
                }
                else
                {
                    gvServices.DataSource = ds;
                    gvServices.DataBind();
                }
            }
            else
            {
                BindBlankServiceGrid();
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnFavourites_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["BTN"] = "FAV";
            BindFavourtite();
            ltrlInvSetName.Visible = false;
            ddlInvSetName.Visible = false;
            ltrlInvCategory.Visible = true;
            ddlSubDepartment.Visible = true;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnAddToList_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dlSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (btnAddToList.Text != "Update List")
            {
                SqlDataReader drServiceDuplicate;
                drServiceDuplicate = (SqlDataReader)dlSave.ExecuteReader(CommandType.Text, "select * FROM OPServiceOrderDetail osdd INNER JOIN ItemOfService ios ON osdd.ServiceId = ios.ServiceId INNER JOIN DepartmentSub s ON s.SubDeptId = ios.SubDeptId INNER JOIN Encounter en ON en.Id = osdd.EncounterId   where osdd.EncounterId ='" + Convert.ToInt32(Session["EncounterID"].ToString()) + "' AND osdd.ServiceId='" + Convert.ToInt32(ViewState["ServiceId"]) + "'AND ServiceAmount not like '0.00' AND s.Type <> 'CL' and osdd.Active=1");
                //drServiceDuplicate.Read();
                if (drServiceDuplicate.HasRows == true)
                {
                    Alert.ShowAjaxMsg("Service already exist.", Page);
                    return;
                }
            }
            if (Session["facilityId"] != null)
            {
                if (txtICDCode.Text.Trim() != "")
                {
                    if (txtCPT.Text.Trim() != "")
                    {
                        if (txtUnit.Text != "")
                        {
                            if (Convert.ToDouble(txtUnit.Text) > 0)
                            {
                                if (txtUnitCharge.Text.Trim().ToString() == "$0" || txtUnitCharge.Text.Trim().ToString() == "0")
                                {
                                    Alert.ShowAjaxMsg("You are entering the unit charge($0).", Page);
                                    goto SaveLabel;
                                }
                                else
                                {
                                    goto SaveLabel;
                                }
                            SaveLabel:
                                //if (Convert.ToDouble(txtUnitCharge.Text) > 0)
                                //{
                                DL_Funs oDLF = new DL_Funs();
                                BaseC.Patient bc = new BaseC.Patient(sConString);

                                BaseC.EMROrders objEmrOrders = new BaseC.EMROrders(sConString);
                                StringBuilder objXML = new StringBuilder();
                                //char[] chr = { ',' };
                                //string[] strmodifier = txtModifier.Text.Split(chr);
                                //for (int i = 0; i < strmodifier.Length; i++)
                                //{
                                objXML.Append("<Table><c1>");
                                objXML.Append(ViewState["ServiceId"].ToString());
                                objXML.Append("</c1><c2>");
                                objXML.Append(txtICDCode.Text);
                                objXML.Append("</c2><c3>");
                                objXML.Append(0);
                                objXML.Append("</c3><c4>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c4><c5>");
                                objXML.Append(bc.FormatDate(rdpFrom.SelectedDate.Value.ToShortDateString(), Session["OutputDateFormat"].ToString(), "dd/MM/yyyy"));
                                //objXML.Append(datarow["FromDate"].ToString());
                                objXML.Append("</c5><c6>");
                                objXML.Append(bc.FormatDate(rdpTo.SelectedDate.Value.ToShortDateString(), Session["OutputDateFormat"].ToString(), "dd/MM/yyyy"));

                                // objXML.Append(datarow["ToDate"].ToString());
                                objXML.Append("</c6><c7>");
                                objXML.Append(txtUnit.Text.Trim());
                                objXML.Append("</c7><c8>");
                                objXML.Append(Convert.ToString(Session["DoctorId"]));
                                objXML.Append("</c8><c9>");
                                objXML.Append(Convert.ToString(Session["FacilityId"]));
                                objXML.Append("</c9><c10>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c10><c11>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c11><c12>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c12><c13>");
                                objXML.Append(0);
                                objXML.Append("</c13><c14>");

                                objXML.Append(0);

                                objXML.Append("</c14><c15>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c15><c16>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c16><c17>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c17><c18>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c18><c19>");

                                objXML.Append(0);

                                objXML.Append("</c19><c20>");

                                objXML.Append(0);

                                objXML.Append("</c20><c21>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c21><c22>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c22><c23>");
                                objXML.Append(hdnID.Value);
                                objXML.Append("</c23><c24>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c24><c25>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c25><c26>");
                                objXML.Append(string.Empty);
                                objXML.Append("</c26><c27>");
                                objXML.Append(txtModifier.Text.Trim());
                                // objXML.Append(strmodifier[i]);
                                objXML.Append("</c27><c28>");


                                if (chkIsBillable.Checked)
                                {
                                    objXML.Append(1);
                                }
                                else
                                {
                                    objXML.Append(0);
                                }
                                objXML.Append("</c28>");
                                TextBox txt = new TextBox();
                                BindCharge(Convert.ToInt32(ViewState["ServiceId"]), txt);

                                if (txt.Text.Trim() != "")
                                {
                                    objXML.Append("<c29>");
                                    //objXML.Append(txt.Text);
                                    objXML.Append(hdnServiceAmount.Value);
                                    objXML.Append("</c29>");
                                }
                                else
                                {
                                    objXML.Append("<c29>");
                                    objXML.Append(txtUnitCharge.Text);
                                    objXML.Append("</c29>");
                                }

                                objXML.Append("<c30>");
                                objXML.Append(hdnServiceDiscountAmount.Value);
                                objXML.Append("</c30>");
                                objXML.Append("<c31>");
                                objXML.Append(hdnDoctorAmount.Value);
                                objXML.Append("</c31>");
                                objXML.Append("<c32>");
                                objXML.Append(hdnDoctorDiscountAmount.Value);
                                objXML.Append("</c32>");
                                objXML.Append("</Table>");
                                //}
                                if (pageId != "")
                                {
                                    pageId = pageId.Substring(1, pageId.Length - 1);
                                }
                                else
                                    pageId = "0";
                                //String strMsg = (string)objEmrOrders.SaveOPOrder(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"].ToString()), Convert.ToInt32(Session["DoctorID"].ToString()), Convert.ToInt16(Session["HospitalLocationId"]), DateTime.Now.ToString("MM/dd/yyyy"), objXML.ToString(), Convert.ToInt32(Session["UserId"]), false, Session["facilityId"].ToString(), pageId, false);
                                // Add OutPut Para By Ram
                                String sOrderID = "";
                                String strMsg = (string)objEmrOrders.SaveOPOrder(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"].ToString()), Convert.ToInt32(Session["DoctorID"].ToString()), Convert.ToInt16(Session["HospitalLocationId"]), DateTime.Now.ToString("MM/dd/yyyy"), objXML.ToString(), Convert.ToInt32(Session["UserId"]), false, Session["facilityId"].ToString(), pageId, false, out sOrderID, 0);

                                if (strMsg == "Saved")
                                {
                                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "RefreshParentPage();", true);
                                    //Telerik.Web.UI.RadWindow obj =(RadWindow )Parent .Page .FindControl ("Radwindow1");
                                    // RadWindowManager obj = (RadWindowManager)Page.FindControl("RadWindowManager1");
                                    lbl_Msg.Text = "Record Saved!";
                                    ClearServiceDetailControls();


                                    BindAddServiceGrid();
                                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CloseWindow", "Close();", true);

                                }
                                else
                                {
                                    Alert.ShowAjaxMsg("Some Problem has encountered,please try again.", this.Page);
                                    return;
                                }
                                //}

                                //else
                                //{
                                //    Alert.ShowAjaxMsg("Unit Charge Should be greater than 0", Page);
                                //    return;
                                //}
                            }
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Unit Should be greater than 0", Page);
                            return;
                        }
                    }

                    else
                    {
                        Alert.ShowAjaxMsg("Please Enter CPT Code", Page);
                        return;
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Please Enter ICD Code", Page);
                    return;
                }
            }
            else
            {
                return;
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return;
        }
        txtCPT.Text = "";
    }

    #endregion

    //******************************Private Methods(Used in Page events)***********************
    #region // Private Methods
    private void ClearServiceDetailControls()
    {
        ViewState["ServiceId"] = null;
        txtCPT.Text = String.Empty;
        txtDescription.Text = String.Empty;
        txtICDCode.Text = String.Empty;
        txtModifier.Text = string.Empty;
        //txtFrom.Text = DateTime.Now.ToShortDateString();
        //txtTo.Text = DateTime.Now.ToShortDateString();

        //rdpFrom.SelectedDate = DateTime.Now;
        //rdpTo.SelectedDate = DateTime.Now;
        txtSearch.Text = String.Empty;
        //txtUnit.Text = String.Empty;
        txtUnitCharge.Text = String.Empty;
        chkIsBillable.Checked = true;

        btnAddToList.Text = "Add To List";
    }
    private DataTable CreateTable()
    {
        DataTable Dt = new DataTable();
        Dt.Columns.Add("Id", typeof(int));
        Dt.Columns.Add("ServiceID", typeof(int));
        Dt.Columns.Add("CPTCode", typeof(string));
        Dt.Columns.Add("LongDescription", typeof(string));
        Dt.Columns.Add("ServiceName", typeof(string));
        Dt.Columns.Add("ModifierCode", typeof(string));
        Dt.Columns.Add("Units", typeof(Int16));
        Dt.Columns.Add("ServiceAmount", typeof(string));
        Dt.Columns.Add("ICDID", typeof(string));
        Dt.Columns.Add("FromDate", typeof(string));
        Dt.Columns.Add("ToDate", typeof(string));
        Dt.Columns.Add("IsBillable", typeof(bool));
        Dt.Columns.Add("ServiceDiscountAmount", typeof(string));
        Dt.Columns.Add("DoctorAmount", typeof(string));
        Dt.Columns.Add("DoctorDiscountAmount", typeof(string));
        return Dt;
    }
    bool BindServiceGridDetail(Int32 iServiceId)
    {
        try
        {
            DataTable DT = (DataTable)Cache["ServiceDetail"];
            if (DT == null)
            {
                DT = CreateTable();
            }
            DataRow[] datarow = DT.Select("ServiceId=" + iServiceId);
            if (txtUnit.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtUnit.Text) > 0)
                {
                    if (txtUnitCharge.Text.Trim() != "")
                    {
                        if (Convert.ToDouble(txtUnitCharge.Text) > 0)
                        {
                            if (datarow.Length > 0)
                            {
                                datarow[0].BeginEdit();
                                datarow[0]["ServiceID"] = iServiceId;
                                datarow[0]["CPTCode"] = txtCPT.Text.Trim();
                                datarow[0]["ServiceName"] = txtDescription.Text.Trim();
                                datarow[0]["ModifierCode"] = txtModifier.Text.Trim();
                                if (!string.IsNullOrEmpty(txtUnit.Text.Trim()))
                                {
                                    datarow[0]["Units"] = txtUnit.Text.Trim();
                                }
                                else
                                {
                                    datarow[0]["Units"] = DBNull.Value;
                                }
                                if (txtUnitCharge.Text != ((Convert.ToDouble(hdnServiceAmount.Value) + Convert.ToDouble(hdnDoctorAmount.Value)) - (Convert.ToDouble(hdnServiceDiscountAmount.Value) + Convert.ToDouble(hdnDoctorDiscountAmount.Value))).ToString())
                                {
                                    datarow[0]["ServiceAmount"] = txtUnitCharge.Text;
                                    datarow[0]["ServiceDiscountAmount"] = 0;
                                    datarow[0]["DoctorAmount"] = 0;
                                    datarow[0]["DoctorDiscountAmount"] = 0;
                                }
                                else
                                {
                                    datarow[0]["ServiceAmount"] = hdnServiceAmount.Value;
                                    datarow[0]["ServiceDiscountAmount"] = hdnServiceDiscountAmount.Value;
                                    datarow[0]["DoctorAmount"] = hdnDoctorAmount.Value;
                                    datarow[0]["DoctorDiscountAmount"] = hdnDoctorDiscountAmount.Value;
                                }
                                datarow[0]["ICDID"] = txtICDCode.Text.Trim();
                                datarow[0]["FromDate"] = rdpFrom.SelectedDate;
                                datarow[0]["ToDate"] = rdpTo.SelectedDate;
                                datarow[0]["IsBillable"] = chkIsBillable.Checked;

                                datarow[0].EndEdit();
                            }
                            else
                            {
                                DataRow dr;
                                dr = DT.NewRow();
                                dr["ServiceID"] = iServiceId;
                                dr["CPTCode"] = txtCPT.Text.Trim();
                                dr["ServiceName"] = txtDescription.Text.Trim();
                                dr["ModifierCode"] = txtModifier.Text.Trim();

                                if (!string.IsNullOrEmpty(txtUnit.Text.Trim()))
                                    dr["Units"] = txtUnit.Text;
                                else
                                    dr["Units"] = DBNull.Value;

                                if (txtUnitCharge.Text != ((Convert.ToDouble(hdnServiceAmount.Value) + Convert.ToDouble(hdnDoctorAmount.Value)) - (Convert.ToDouble(hdnServiceDiscountAmount.Value) + Convert.ToDouble(hdnDoctorDiscountAmount.Value))).ToString())
                                {
                                    dr["ServiceAmount"] = txtUnitCharge.Text;
                                    hdnServiceAmount.Value = txtUnitCharge.Text;
                                    dr["ServiceDiscountAmount"] = 0;
                                    dr["DoctorAmount"] = 0;
                                    dr["DoctorDiscountAmount"] = 0;
                                }
                                else
                                {
                                    dr["ServiceAmount"] = hdnServiceAmount.Value;
                                    dr["ServiceDiscountAmount"] = hdnServiceDiscountAmount.Value;
                                    dr["DoctorAmount"] = hdnDoctorAmount.Value;
                                    dr["DoctorDiscountAmount"] = hdnDoctorDiscountAmount.Value;
                                }

                                dr["ICDID"] = txtICDCode.Text.Trim();
                                dr["FromDate"] = rdpFrom.SelectedDate;
                                dr["ToDate"] = rdpTo.SelectedDate;
                                dr["IsBillable"] = chkIsBillable.Checked;

                                DT.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Unit Charge Should be greater than 0", Page);
                            return false;
                        }
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Unit Should be greater than 0", Page);
                    return false;
                }

            }
            Cache.Insert("ServiceDetail", DT, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            gvAddService.DataSource = DT;
            gvAddService.DataBind();
            return true;
            //lbl_Msg.Text = "Service added successfully.";
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return false;
        }
    }
    private void BindBlankServiceDetailGrid()
    {
        try
        {
            DataTable dT = CreateTable();
            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["Id"] = 0;
                dr["ServiceID"] = 0;
                dr["CPTCode"] = DBNull.Value;
                dr["LongDescription"] = DBNull.Value;
                dr["ServiceName"] = DBNull.Value;
                dr["ModifierCode"] = DBNull.Value;
                dr["Units"] = DBNull.Value;
                dr["ServiceAmount"] = "0";
                dr["ICDID"] = DBNull.Value;
                dr["FromDate"] = DBNull.Value;
                dr["ToDate"] = DBNull.Value;
                dr["IsBillable"] = true;
                dr["ServiceDiscountAmount"] = "0";
                dr["DoctorAmount"] = "0";
                dr["DoctorDiscountAmount"] = "0";
                dT.Rows.Add(dr);

            }
            gvAddService.DataSource = dT;
            gvAddService.DataBind();
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindCategoryDDL()
    {
        try
        {
            if (Session["HospitalLocationId"] != null)
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                StringBuilder objStr = new StringBuilder();
                Hashtable hsInput = new Hashtable();
                hsInput.Add("HospitalLocationID", Session["HospitalLocationId"].ToString());
                objStr.Append("SELECT DISTINCT dm.DepartmentID, dm.DepartmentName FROM DepartmentMain dm");
                objStr.Append(" INNER JOIN DepartmentSub ds ON dm.DepartmentID = ds.DepartmentID AND ");
                objStr.Append(" ds.Type NOT IN ('VS','VF','CL','R','RT') AND ds.Active = 1");
                objStr.Append(" WHERE (dm.HospitalLocationId = @HospitalLocationID OR dm.HospitalLocationId IS NULL) ");
                objStr.Append(" AND dm.Active = 1 ORDER BY DepartmentName ");
                DataSet dsObj = dl.FillDataSet(CommandType.Text, objStr.ToString(), hsInput);
                ddlSubDepartment.DataSource = dsObj;
                ddlSubDepartment.DataValueField = "DepartmentID";
                ddlSubDepartment.DataTextField = "DepartmentName";
                ddlSubDepartment.DataBind();
            }
            //ddlSubDepartment.Items.Add(new ListItem("ALL","0"));
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindDropDownSetName()
    {
        try
        {
            BaseC.EMROrders objEMROrders = new BaseC.EMROrders(sConString);
            SqlDataReader objDr = objEMROrders.populateInvestigationSetMain(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["DoctorID"].ToString()));
            if (objDr.HasRows == true)
            {
                ddlInvSetName.DataSource = objDr;
                ddlInvSetName.DataValueField = "SetID";
                ddlInvSetName.DataTextField = "SetName";
                ddlInvSetName.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulatePastPatientService()
    {
        try
        {
            Hashtable hstInput = new Hashtable();
            hstInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"].ToString());
            hstInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
            hstInput.Add("@bitDistinctRecords", 1);
            if (txtSearch.Text != string.Empty)
            {
                string ServiceName = string.Empty;
                if (ddlSearchCriteria.SelectedValue == "1")//any where
                    ServiceName = "%" + txtSearch.Text.Trim() + "%";
                if (ddlSearchCriteria.SelectedValue == "2")//starts with
                    ServiceName = txtSearch.Text.Trim() + "%";
                if (ddlSearchCriteria.SelectedValue == "3")//ends with
                    ServiceName = "%" + txtSearch.Text.Trim();
                hstInput.Add("@chvSearchCriteria", ServiceName);
            }
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", hstInput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                hdnModifierID.Value = ds.Tables[0].Rows[0]["ModifierCode"].ToString();
                DataView objDv = ds.Tables[0].DefaultView;
                objDv.RowFilter = "ServiceType not in ('VS','VF','CL','R','RT')";
                if (objDv != null)
                {
                    gvServices.DataSource = objDv;
                    gvServices.DataBind();
                }
            }
            else
            {
                BindBlankServiceGrid();
            }

            ltrlInvSetName.Visible = false;
            ddlInvSetName.Visible = false;
            ltrlInvCategory.Visible = true;
            ddlSubDepartment.Visible = true;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulatePastService()
    {
        try
        {
            if (Session["HospitalLocationId"] != null && Session["DoctorID"] != null)
            {
                Hashtable hstInput = new Hashtable();
                hstInput.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));
                //hstInput.Add("@intDoctorId", ViewState["DoctorID"].ToString());
                hstInput.Add("@intDoctorID", Convert.ToInt32(Session["DoctorID"]));
                hstInput.Add("@bitDistinctRecords", 1);
                if (txtSearch.Text != string.Empty)
                {
                    string ServiceName = string.Empty;
                    if (ddlSearchCriteria.SelectedValue == "1")//any where
                        ServiceName = "%" + txtSearch.Text.Trim() + "%";
                    if (ddlSearchCriteria.SelectedValue == "2")//starts with
                        ServiceName = txtSearch.Text.Trim() + "%";
                    if (ddlSearchCriteria.SelectedValue == "3")//ends with
                        ServiceName = "%" + txtSearch.Text.Trim();
                    hstInput.Add("@chvSearchCriteria", ServiceName);
                }
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", hstInput);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView objDv = ds.Tables[0].DefaultView;
                    objDv.RowFilter = "ServiceType not in ('VS','VF','CL','R','RT')";
                    if (objDv != null)
                    {
                        gvServices.DataSource = objDv;
                        gvServices.DataBind();
                    }
                }
                else
                {
                    BindBlankServiceGrid();
                }
            }

            ltrlInvSetName.Visible = false;
            ddlInvSetName.Visible = false;
            ltrlInvCategory.Visible = true;
            ddlSubDepartment.Visible = true;
            // ddlInvSetName.Visible = false;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateAllService()
    {
        if (txtSearch.Text == string.Empty)
        {
            BindServiceGrid(0);
        }
        else
        {
            BindServiceGrid(1);
        }

        ltrlInvSetName.Visible = false;
        ddlInvSetName.Visible = false;
        ltrlInvCategory.Visible = true;
        ddlSubDepartment.Visible = true;
        // ddlInvSetName.Visible = false;
    }

    private void BindServiceGrid(int intSearch)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder strSQL = new StringBuilder();
            Hashtable hshIn = new Hashtable();
            //hshIn.Add("@HospID", Session["HospitalLocationID"]);
            //hshIn.Add("@Active", 1);
            string ServiceName = "%%";
            if (intSearch == 1)
            {
                if (txtSearch.Text.Trim() != "")
                {
                    if (ddlSearchCriteria.SelectedValue == "1")//any where
                        ServiceName = "%" + txtSearch.Text.Trim() + "%";
                    if (ddlSearchCriteria.SelectedValue == "2")//starts with
                        ServiceName = txtSearch.Text.Trim() + "%";
                    if (ddlSearchCriteria.SelectedValue == "3")//ends with
                        ServiceName = "%" + txtSearch.Text.Trim();
                }
                hshIn.Add("@chvSearchCriteria", ServiceName);
            }

            hshIn.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
            hshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            string storeProcedurnme = string.Empty;

            if (ddlSubDepartment.SelectedItem.Value == "0")
                hshIn.Add("@intDepartmentId", string.Empty);
            else
                hshIn.Add("@intDepartmentId", ddlSubDepartment.SelectedItem.Value);

            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServices", hshIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView objDv = ds.Tables[0].DefaultView;
                objDv.RowFilter = "ServiceType not in ('VS','VF','CL','R','RT')";
                if (objDv != null)
                {
                    gvServices.DataSource = objDv;
                    gvServices.DataBind();
                }
            }
            else
            {
                BindBlankServiceGrid();
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindBlankServiceGrid()
    {
        try
        {
            DataTable Dt = new DataTable();
            Dt.Columns.Add("ServiceID");
            Dt.Columns.Add("SerialNo");
            Dt.Columns.Add("ServiceName");
            Dt.Columns.Add("SpecialPrecaution");
            Dt.Columns.Add("LongDescription");
            Dt.Columns.Add("CPTCode");
            Dt.Columns["SerialNo"].AutoIncrement = true;
            Dt.Columns["SerialNo"].AutoIncrementSeed = 1;
            Dt.Columns["SerialNo"].AutoIncrementStep = 1;

            for (int i = 1; i < 2; i++)
            {
                DataRow Dr = Dt.NewRow();

                Dr["ServiceID"] = DBNull.Value;
                Dr["SerialNo"] = DBNull.Value;

                if (Convert.ToString(ViewState["BTN"]) == "ALL")
                {
                    Dr["ServiceName"] = "No Service Found.";

                }
                else if (Convert.ToString(ViewState["BTN"]) == "DOC")
                {
                    Dr["ServiceName"] = "No Past Service Found.";

                }
                else if (Convert.ToString(ViewState["BTN"]) == "PAT")
                {
                    Dr["ServiceName"] = "No Past Patient Service Found.";

                }
                else if (Convert.ToString(ViewState["BTN"]) == "FAV")
                {
                    Dr["ServiceName"] = "No Favorite Found";

                }
                else if (Convert.ToString(ViewState["BTN"]) == "ORDER")
                {
                    Dr["ServiceName"] = "No Service Found";

                }
                else
                {
                    Dr["ServiceName"] = "No Service Found";

                }


                Dr["SpecialPrecaution"] = DBNull.Value;

                Dt.Rows.Add(Dr);
            }

            gvServices.DataSource = Dt;
            gvServices.DataBind();
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindICDPanel()
    {
        try
        {
            if (ViewState["ICDCodes"] != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("ICDCodes");
                dt.Columns.Add("Description");
                dt.Columns["ID"].AutoIncrement = true;
                dt.Columns["ID"].AutoIncrementSeed = 1;
                dt.Columns["ID"].AutoIncrementStep = 1;

                char[] chArray = { ',' };
                string[] serviceIdXml = ViewState["ICDCodes"].ToString().Split(chArray);
                DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                foreach (string item in serviceIdXml)
                {
                    DataRow drdt = dt.NewRow();
                    string sql = "";
                    sql = "SELECT ICDID, ICDCode, Description FROM ICD9SubDisease "
                    + "WHERE (ICDCode = @Diagnosis)";
                    Hashtable hshIn = new Hashtable();
                    hshIn.Add("@Diagnosis", item.ToString());
                    DataSet dsTemp = new DataSet();
                    dsTemp = objSave.FillDataSet(CommandType.Text, sql, hshIn);
                    drdt["ICDCodes"] = item.ToString();
                    drdt["Description"] = dsTemp.Tables[0].Rows[0]["Description"].ToString();
                    dt.Rows.Add(drdt);
                    //txtICDCode.Text = ViewState["ICDCodes"].ToString();
                }
                if (dt.Rows.Count == 1)
                {
                    txtICDCode.Text = dt.Rows[0]["ICDCodes"].ToString();
                }
                else if (dt.Rows.Count > 1)// saten change for multi diagnosis 
                {
                    txtICDCode.Text = ViewState["ICDCodes"].ToString();
                }


                //rptDiagnosis.DataSource = dt;
                //rptDiagnosis.DataBind();
                //cblICDCodes.DataSource = dt;
                //cblICDCodes.DataValueField = "ID";
                //cblICDCodes.DataTextField = "ICDCodes";
                //cblICDCodes.DataBind();
                //imgOk.Attributes.Add("onclick", "javascript:HidePanelOKClick('" + pnlICDCodes.ClientID + "','" + cblICDCodes.ClientID + "','" + txtICDCode.ClientID + "','" + hdnICDCode.ClientID + "')");
                //imgClose.Attributes.Add("onclick", "javascript:HideICDPanel('" + pnlICDCodes.ClientID + "')");
                txtICDCode.Attributes.Add("onclick", "javascript:ShowICDPanel('" + pnlICDCodes.ClientID + "', this )");

            }
            else
            {
                txtICDCode.ReadOnly = true;
                txtICDCode.ReadOnly = true;
                //imgOk.Attributes.Remove("onclick");
                //imgClose.Attributes.Remove("onclick");
                txtICDCode.Attributes.Remove("onclick");
                //imgOk.Attributes.Add("onclick", "javascript:HidePanelOKClick('" + pnlICDCodes.ClientID + "','" + cblICDCodes.ClientID + "','" + txtICDCode.ClientID + "','" + hdnICDCode.ClientID + "')");
                //imgClose.Attributes.Add("onclick", "javascript:HideICDPanel('" + pnlICDCodes.ClientID + "')");
            }

            //imgOk.Attributes.Remove("onclick");
            //imgClose.Attributes.Remove("onclick");
            //imgOk.Attributes.Add("onclick", "javascript:HidePanelOKClick('" + pnlICDCodes.ClientID + "','" + cblICDCodes.ClientID + "','" + txtICDCode.ClientID + "','" + hdnICDCode.ClientID + "')");
            //imgClose.Attributes.Add("onclick", "javascript:HideICDPanel('" + pnlICDCodes.ClientID + "')");

            txtICDCode.Attributes.Add("onclick", "javascript:ShowICDPanel('" + pnlICDCodes.ClientID + "', this )");
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void getCurrentICDCodes()
    {
        try
        {
            DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@inyHospitalLocationID", Session["HospitalLocationID"].ToString());
            hshIn.Add("@intRegistrationId", Session["RegistrationID"].ToString());//
            hshIn.Add("@intEncounterId", Session["EncounterId"].ToString());

            DataSet ds = objSave.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hshIn);
            String sICDCodes = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (sICDCodes == "")
                    {
                        sICDCodes += ds.Tables[0].Rows[i]["ICDCode"].ToString();
                    }
                    else
                    {
                        sICDCodes += "," + ds.Tables[0].Rows[i]["ICDCode"].ToString();
                    }
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ICDCodes"] = sICDCodes;
            }
            else
            {
                ViewState["ICDCodes"] = null;
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void ModifierTable()
    {
        try
        {
            string strQuery = "select ModifierCode,ModifierCode+'  ('+ Description+')' as Description from ModifierList";
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.Text, strQuery);
            ddlModifier.DataSource = ds.Tables[0];
            ddlModifier.DataTextField = "Description";
            ddlModifier.DataValueField = "ModifierCode";

            ddlModifier.DataBind();
            //ddlModifier.Attributes.Remove("onchange");
            //ddlModifier.Attributes.Add("onchange", "javascript:ShowModifierPanelOnChangeDropDown('" + ddlModifier.ClientID + "','" + txtModifier.ClientID + "','" + pnlModifierCode.ClientID + "');");

            txtModifier.Attributes.Add("onclick", "javascript:showModifierPanel('" + pnlModifierCode.ClientID + "');");
            foreach (ListItem _listItem in this.ddlModifier.Items)
            { _listItem.Attributes.Add("title", _listItem.Text); }
            ddlModifier.Attributes.Add("onmouseover", "this.title=this.options[this.selectedIndex].title");

            imgCloseModifier.Attributes.Add("onclick", "javascript:HideICDPanel('" + pnlModifierCode.ClientID + "');");
            imgOkModifier.Attributes.Add("onclick", "javascript:ShowModifierPanelOnChangeDropDown('" + ddlModifier.ClientID + "','" + txtModifier.ClientID + "','" + pnlModifierCode.ClientID + "');");
            BindCharge(Convert.ToInt32(ViewState["ServiceId"]), txtUnitCharge);
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindFavourtite()
    {
        try
        {
            if (Session["DoctorID"] != null)
            {
                Hashtable hstInput = new Hashtable();
                hstInput.Add("@intDoctorId", Session["DoctorID"].ToString());
                if (txtSearch.Text != string.Empty)
                {
                    string ServiceName = string.Empty;
                    if (ddlSearchCriteria.SelectedValue == "1")//any where
                        ServiceName = "%" + txtSearch.Text.Trim() + "%";
                    if (ddlSearchCriteria.SelectedValue == "2")//starts with
                        ServiceName = txtSearch.Text.Trim() + "%";
                    if (ddlSearchCriteria.SelectedValue == "3")//ends with
                        ServiceName = "%" + txtSearch.Text.Trim();
                    hstInput.Add("@chvSearchCriteria", ServiceName);
                }
                //hstInput.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteServices", hstInput);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView objDv = ds.Tables[0].DefaultView;
                    objDv.RowFilter = "ServiceType not in ('VS','VF','CL','R','RT')";
                    if (objDv != null)
                    {
                        gvServices.DataSource = objDv;
                        gvServices.DataBind();
                    }
                }
                else
                {
                    BindBlankServiceGrid();
                }
            }
            else
            {
                BindBlankServiceGrid();
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion

    private void BindAddServiceGrid()
    {
        try
        {
            DAL.DAL dlSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            hshIn.Add("@intRegistrationId", Convert.ToInt32(Session["registrationId"].ToString()));
            hshIn.Add("@intEncounterId", Convert.ToInt32(Session["EncounterID"].ToString()));
            DataSet dsCPT = dlSave.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", hshIn);

            if (dsCPT.Tables[0].Rows.Count > 0)
            {
                DataTable dt = dsCPT.Tables[0];
                DataView dv = dsCPT.Tables[0].DefaultView;
                dv.RowFilter = "ServiceType <> 'CL'";
                if (dv.Count > 0)
                {
                    gvAddService.DataSource = dv.ToTable();
                    gvAddService.DataBind();
                    Cache.Insert("ServiceDetail", dv.ToTable(), null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            else
            {
                BindBlankServiceDetailGrid();
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvAddService_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (gvAddService.SelectedIndexes != null)
            {
                //ViewState["ServiceId"] = gvAddService.DataKeys[gvAddService.SelectedIndex].Values[0].ToString();
                HiddenField lblServiceIDGrid = (HiddenField)gvAddService.SelectedItems[0].FindControl("lblServiceID");
                Label lblServiceName = (Label)gvAddService.SelectedItems[0].FindControl("lblServiceName");
                HiddenField lblModifierCodeGrid = (HiddenField)gvAddService.SelectedItems[0].FindControl("lblModifierCode");
                Label lblUnits = (Label)gvAddService.SelectedItems[0].FindControl("lblUnits");
                Label lblUnitAmount = (Label)gvAddService.SelectedItems[0].FindControl("lblUnitAmount");
                HiddenField lblICDId = (HiddenField)gvAddService.SelectedItems[0].FindControl("lblICDId");
                HiddenField chkIsBillableGrid = (HiddenField)gvAddService.SelectedItems[0].FindControl("chkIsBillable");
                HiddenField lblFromDateGrid = (HiddenField)gvAddService.SelectedItems[0].FindControl("lblFromDate");
                HiddenField lblToDateGrid = (HiddenField)gvAddService.SelectedItems[0].FindControl("lblToDate");
                Label lblCPTCode = (Label)gvAddService.SelectedItems[0].FindControl("lblCPTCode");
                HiddenField hdnId1 = (HiddenField)gvAddService.SelectedItems[0].FindControl("lblId");

                hdnID.Value = hdnId1.Value;
                ViewState["ServiceId"] = lblServiceIDGrid.Value.Trim();
                txtDescription.Text = lblServiceName.Text.Trim();
                txtModifier.Text = lblModifierCodeGrid.Value.Trim();
                txtCPT.Text = lblCPTCode.Text.Trim();
                txtICDCode.Text = lblICDId.Value.Trim();
                txtUnit.Text = lblUnits.Text.Trim();
                txtUnitCharge.Text = lblUnitAmount.Text.Trim();
                hdnServiceAmount.Value = txtUnitCharge.Text;
                hdnServiceDiscountAmount.Value = "0";
                hdnDoctorAmount.Value = "0";
                hdnDoctorDiscountAmount.Value = "0";
                if (lblFromDateGrid.Value.Trim() != "")
                    rdpFrom.SelectedDate = Convert.ToDateTime(lblFromDateGrid.Value.Trim());

                if (lblToDateGrid.Value.Trim() != "")
                    rdpTo.SelectedDate = Convert.ToDateTime(lblToDateGrid.Value.Trim());

                chkIsBillable.Checked = chkIsBillableGrid.Value.ToLower() == "true" ? true : false;
                btnAddToList.Text = "Update List";
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvAddService_RowCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (Session["FacilityId"] != null && Session["HospitalLocationID"] != null && Session["RegistrationId"] != null && Session["EncounterId"] != null)
            {
                if (e.CommandName == "Del")
                {
                    Telerik.Web.UI.GridDataItem row = (GridDataItem)(((ImageButton)e.CommandSource).NamingContainer);

                    HiddenField lblId = (HiddenField)row.FindControl("lblId");
                    string strId = lblId.Value;

                    if (!string.IsNullOrEmpty(strId))
                    {

                        Hashtable hstIn = new Hashtable();
                        if (pageId != "")
                            pageId = pageId.Substring(1, pageId.Length - 1);
                        else
                            pageId = "0";

                        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                        hstIn.Add("inyHospitalLocationId", Session["HospitalLocationID"]);
                        hstIn.Add("intLoginFacilityId", Session["FacilityId"]);
                        hstIn.Add("intRegistrationId", Session["RegistrationId"].ToString());
                        hstIn.Add("intEncounterId", Session["EncounterId"].ToString());
                        hstIn.Add("intPageId", pageId);
                        hstIn.Add("intOrderId", strId);
                        hstIn.Add("intEncodedBy", Session["UserId"].ToString());
                        dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeletePatientOrders", hstIn);
                    }
                    DataTable dt = new DataTable();
                    dt = (DataTable)Cache["ServiceDetail"];
                    dt.Rows.RemoveAt(row.ItemIndex);
                    dt.AcceptChanges();

                    if (dt.Rows.Count > 0)
                    {
                        gvAddService.DataSource = dt;
                        gvAddService.DataBind();

                    }
                    else
                    {

                        BindBlankServiceDetailGrid();
                        Cache.Remove("ServiceDetail");
                    }
                    ClearServiceDetailControls();

                }
                btnAddToList.Text = "Add To List";
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvAddService_ItemDeleted(object sender, Telerik.Web.UI.GridDeletedEventArgs e)
    {
        e.ExceptionHandled = true;
        // try
        // {
        //         HiddenField hdnId = (HiddenField)e.Item.FindControl("lblId");

        //         string strId = hdnId.Value;

        //         if (!string.IsNullOrEmpty(strId))
        //         {

        //             string strQuery = "update OPServiceOrderDetail set Active=0 where Id=@Id";
        //             DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //             Hashtable hstIn = new Hashtable();
        //             hstIn.Add("@Id", strId);
        //             dl.ExecuteNonQuery(CommandType.Text, strQuery, hstIn);

        //         }
        //         DataTable dt = new DataTable();
        //         dt = (DataTable)Cache["ServiceDetail"];
        //         dt.Rows.RemoveAt(e.Item.ItemIndex);
        //         dt.AcceptChanges();

        //         if (dt.Rows.Count > 0)
        //         {
        //             gvAddService.DataSource = dt;
        //             gvAddService.DataBind();

        //         }
        //         else
        //         {

        //             BindBlankServiceDetailGrid();
        //             Cache.Remove("ServiceDetail");
        //         }
        //         ClearServiceDetailControls();


        //}
        // catch (Exception ex)
        // {
        //     Alert.ShowAjaxMsg(ex.Message, Page);
        // }
    }

    protected void gvAddService_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        //if (e.Row.RowType != DataControlRowType.Pager)
        //{
        //    e.Row.Cells[Convert.ToByte(GridAddServices.Id)].Visible = false;
        //    e.Row.Cells[Convert.ToByte(GridAddServices.ServiceID)].Visible = false;
        //    e.Row.Cells[Convert.ToByte(GridAddServices.ModifierCode)].Visible = false;
        //    e.Row.Cells[Convert.ToByte(GridAddServices.IsBillable)].Visible = false;
        //    e.Row.Cells[Convert.ToByte(GridAddServices.ICDCodes)].Visible = false;
        //    e.Row.Cells[Convert.ToByte(GridAddServices.FromDate)].Visible = false;
        //    e.Row.Cells[Convert.ToByte(GridAddServices.ToDate)].Visible = false;
        //    e.Row.Cells[13].Visible = false;
        //    e.Row.Cells[14].Visible = false;
        //    e.Row.Cells[15].Visible = false;
        //}
        ////if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
        ////{

        ////}
        // if (e.Item.ItemType == GridItemType.Item)
        //  if (e.Item.ItemType != GridItemType.Header && e.Item.ItemType != GridItemType.Footer && e.Item.ItemType != GridItemType.Pager && e.Item.ItemType != GridItemType.EditFormItem)
        if (e.Item is GridDataItem)
        {
            HiddenField lblServiceIDGrid = (HiddenField)e.Item.FindControl("lblServiceID");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkEdit");
            ImageButton ibtnDelete = (ImageButton)e.Item.FindControl("ibtnDelete");
            if (lblServiceIDGrid.Value == "0" || lblServiceIDGrid.Value == "")
            {
                lb.Enabled = false;
                ibtnDelete.Enabled = false;
            }

            Label lblServiceNameGrid = (Label)e.Item.FindControl("lblServiceName");
            if (lblServiceNameGrid != null)
            {
                lblServiceNameGrid.ToolTip = lblServiceNameGrid.Text;
            }
            Label lblServiceAmount = (Label)e.Item.FindControl("lblUnitAmount");
            HiddenField hdnServiceAmount = (HiddenField)e.Item.FindControl("hdnServiceAmount");
            HiddenField hdnServiceDiscountAmount = (HiddenField)e.Item.FindControl("hdnServiceDiscountAmount");
            HiddenField hdnDoctorAmount = (HiddenField)e.Item.FindControl("hdnDoctorAmount");
            HiddenField hdnDoctorDiscountAmount = (HiddenField)e.Item.FindControl("hdnDoctorDiscountAmount");
            if (lblServiceAmount.Text.Trim().ToString() == "0.00" || lblServiceAmount.Text.Trim().ToString() == "")
            {
                lblServiceAmount.Text = "$0.00";
            }
            else
            {
                lblServiceAmount.Text = "$" + ((Convert.ToDecimal(hdnServiceAmount.Value) + Convert.ToDecimal(hdnDoctorAmount.Value)) - (Convert.ToDecimal(hdnServiceDiscountAmount.Value) + Convert.ToDecimal(hdnDoctorDiscountAmount.Value))).ToString();
            }
            //lblServiceAmount.Text = 
        }
    }

    //protected void fillConsultant(string SpecilityId)
    //{
    //    BaseC.ParseData objParse = new BaseC.ParseData();
    //    Hashtable hstInput = new Hashtable();
    //    hstInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
    //    hstInput.Add("@intSpecialisationId", SpecilityId);
    //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    DataSet objDs;
    //    if (ddlSpecialisation.SelectedItem.Text != "All")
    //    {
    //        if (ddlSpecialisation.SelectedItem.Text != "[ Select ]")
    //        {
    //            objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hstInput);
    //            ddlOwnerName.DataSource = objDs;
    //            ddlOwnerName.DataTextField = "DoctorName";
    //            ddlOwnerName.DataValueField = "DoctorId";
    //            ddlOwnerName.DataBind();
    //            ddlOwnerName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Group"));
    //            ddlOwnerName.Items[0].Value = "0";

    //            if (objDs.Tables[0].Rows.Count > 0)
    //                ddlOwnerName.Items[ddlOwnerName.Items.IndexOf(ddlOwnerName.Items.FindItemByValue("0"))].Selected = true;
    //        }
    //        else
    //        {
    //            ddlOwnerName.Items.Clear();
    //            ddlOwnerName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("All", "0"));

    //        }
    //    }
    //    else
    //    {
    //        ddlOwnerName.Items.Clear();
    //        ddlOwnerName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("All", "0"));

    //    }
    //}
    //private void PopulateDoctor()
    //{
    //    Hashtable hshIn = new Hashtable();
    //    //hshIn.Add("@Active", "1");
    //    //hshIn.Add("@HospitalLocationID", Convert.ToInt32(Session["HospitalLocationID"]));
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "Select Id, Name from SpecialisationMaster where Active=1 order by name");
    //    ddlSpecialisation.DataSource = dr;
    //    ddlSpecialisation.DataTextField = "Name";
    //    ddlSpecialisation.DataValueField = "Id";
    //    ddlSpecialisation.DataBind();
    //    ddlSpecialisation.Items.Insert(0, new RadComboBoxItem("All", "0"));
    //}

    protected void lnkMedication_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PatientMedicationCharges.aspx", false);
    }

    protected void lnkENMCodes_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("ENMCodes.aspx", false);
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("PatientDiagnosisCharges.aspx", false);
    }

    protected void ddlModifier_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtModifier.Text == "" || txtModifier.Text == "0")
            txtModifier.Text = ddlModifier.SelectedValue;
        else
            txtModifier.Text = txtModifier.Text + "," + ddlModifier.SelectedValue;

        if (txtModifier.Text != "")
        {
            BindCharge(Convert.ToInt32(ViewState["ServiceId"]), txtUnitCharge);
        }
    }

}