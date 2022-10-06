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
using System.Text;

public partial class EMR_Dashboard_PatientDashboardNew : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["From"]) == "POPUP")
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
            try
            {
                if (common.myInt(Session["HospitalLocationId"]) == 0 || common.myInt(Session["FacilityId"]) == 0)
                {
                    Response.Redirect("/Default.aspx", false);
                    return;
                }

                dtpFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                dtpToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                dtpFromDate.SelectedDate = common.myDate(DateTime.Now).AddYears(-3);
                dtpToDate.SelectedDate = common.myDate(DateTime.Now);

                if (common.myStr(Request.QueryString["From"]) == "POPUP")
                {
                    ComboPatientSearch.Enabled = false;
                    btnClose.Visible = true;
                }

                if (common.myInt(Session["EncounterId"]) > 0)
                {
                    imgBtnAddAllergies.Enabled = true;
                }
                else
                {
                    imgBtnAddAllergies.Enabled = false;
                }

                bindControl();

                bindPatientDetails();

                bindEncounters();

                bindDetailsData();
            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;

                objException.HandleException(Ex);
            }
            finally
            {
            }
        }
    }

    private void bindControl()
    {
        //DataSet ds = new DataSet();
        //BaseC.Patient objP = new BaseC.Patient(sConString);
        try
        {
            /******  ********/

            //ds = objPharmacy.getSupplierMasterList(common.myInt(Session["FacilityID"]), 0, common.myInt(Session["HospitalLocationID"]), 1, "", "", common.myInt(Session["UserID"]), "V");

            //ddlSupplier.DataSource = ds.Tables[0];
            //ddlSupplier.DataTextField = "SupplierName";
            //ddlSupplier.DataValueField = "SupplierId";
            //ddlSupplier.DataBind();

            //ddlSupplier.Items.Insert(0, new RadComboBoxItem("", "0"));
            //ddlSupplier.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            //ds.Dispose();
            //objPharmacy = null;
        }
    }

    private void bindPatientDetails()
    {
        DataSet ds = new DataSet();
        BaseC.Patient objP = new BaseC.Patient(sConString);
        try
        {
            /****** Patient Details ********/

            ds = objP.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), 0, common.myInt(Session["EncounterId"]), common.myInt(Session["UserId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblPatientName.Text = common.myStr(ds.Tables[0].Rows[0]["PatientName"]).Trim() + ", " + common.myStr(ds.Tables[0].Rows[0]["GenderAge"]);
                    lblPatientName.ToolTip = "Address: " + common.myStr(ds.Tables[0].Rows[0]["Address"]).Trim();
                    lblRegistrationNo.Text = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
                    lblEncounterNo.Text = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
                    lblDOB.Text = common.myStr(ds.Tables[0].Rows[0]["DOB"]);
                    lblMobileNo.Text = common.myStr(ds.Tables[0].Rows[0]["MobileNo"]).Trim();
                    //lblAddress.Text = common.myStr(ds.Tables[0].Rows[0]["Address"]).Trim();

                    patientQV.ShowPatientDetails(common.myInt(Session["RegistrationId"]));
                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Patient not found !";
            }

            /******  ********/

            //ds = objPharmacy.getSupplierMasterList(common.myInt(Session["FacilityID"]), 0, common.myInt(Session["HospitalLocationID"]), 1, "", "", common.myInt(Session["UserID"]), "V");

            //ddlSupplier.DataSource = ds.Tables[0];
            //ddlSupplier.DataTextField = "SupplierName";
            //ddlSupplier.DataValueField = "SupplierId";
            //ddlSupplier.DataBind();

            //ddlSupplier.Items.Insert(0, new RadComboBoxItem("", "0"));
            //ddlSupplier.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            //objPharmacy = null;
        }
    }

    private void clearControl()
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "&nbsp;";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    private void bindDetailsData()
    {
        try
        {
            bindChiefComplaints();
            bindAllergies();
            bindVitals();
            bindTemplates();
            bindDiagnosis();
            bindOrdersAndProcedures();
            bindLabResults();
            bindPrescriptions();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ComboPatientSearch.Text = "";

            tdDateRange.Visible = false;

            switch (common.myStr(ddlTime.SelectedValue))
            {
                case "": //Complete History
                    dtpFromDate.SelectedDate = common.myDate(DateTime.Now).AddYears(-3);
                    dtpToDate.SelectedDate = common.myDate(DateTime.Now);
                    break;
                case "DD0": //Today
                    dtpFromDate.SelectedDate = common.myDate(DateTime.Now);
                    dtpToDate.SelectedDate = common.myDate(DateTime.Now);
                    break;
                case "WW-1": //Last Week
                    dtpFromDate.SelectedDate = common.myDate(DateTime.Now).AddDays(-7);
                    dtpToDate.SelectedDate = common.myDate(DateTime.Now);
                    break;
                case "MM-1": //Last Month
                    dtpFromDate.SelectedDate = common.myDate(DateTime.Now).AddMonths(-1);
                    dtpToDate.SelectedDate = common.myDate(DateTime.Now);
                    break;
                case "YY-1": //Last Year
                    dtpFromDate.SelectedDate = common.myDate(DateTime.Now).AddYears(-1);
                    dtpToDate.SelectedDate = common.myDate(DateTime.Now);
                    break;
                case "4": //Date Range
                    tdDateRange.Visible = true;
                    dtpFromDate.SelectedDate = common.myDate(DateTime.Now);
                    dtpToDate.SelectedDate = common.myDate(DateTime.Now);
                    break;
                default:
                    dtpFromDate.SelectedDate = common.myDate(DateTime.Now).AddYears(-3);
                    dtpToDate.SelectedDate = common.myDate(DateTime.Now);
                    break;
            }

            bindDetailsData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void RadComboBoxProduct_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            //bindData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void btnClearFilter_OnClick(object sender, EventArgs e)
    {
        try
        {
            clearControl();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void btnAttachment_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (Session["RegistrationId"] != null)
            {
                //RadWindowForNew.NavigateUrl = "~/EMR/AttachDocument.aspx?Regid=" + common.myInt(Session["RegistrationId"]) +
                //                        "&RegNo=" + common.myStr(Session["RegistrationNo"]) +
                //                        "&EncId=" + common.myInt(Session["EncounterId"]) +
                //                        "&EncNo=&Category=PopUp";

                RadWindowForNew.NavigateUrl = "~/EMR/AttachDocumentFTP.aspx?Regid=" + common.myInt(Session["RegistrationId"]) +
                                        "&RegNo=" + common.myStr(Session["RegistrationNo"]) +
                                        "&EncId=" + common.myInt(Session["EncounterId"]) +
                                        "&EncNo=&Category=PopUp";

                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 990;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                // RadWindowForNew.Title = "Time Slot";
                //RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
        }
    }


    public DataSet getPatientEncounterDetails(int HospId, int FacilityId, int RegistrationId)
    {
        Hashtable hsTb = new Hashtable();

        hsTb.Add("@HospitalLocationId", HospId);
        hsTb.Add("@FacilityID", FacilityId);
        hsTb.Add("@RegistrationId", RegistrationId);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientEncounterDetails", hsTb);
        return objDs;
    }

    #region Encounters

    protected void gvEncounters_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnEncounterId = (HiddenField)e.Row.FindControl("hdnEncounterId");

                HiddenField hdnEncDate = (HiddenField)e.Row.FindControl("hdnEncDate");
                LinkButton lnkEncounterNo = (LinkButton)e.Row.FindControl("lnkEncounterNo");
                if (hdnEncounterId.Value == Session["EncounterId"].ToString())
                {
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
                }
                hdnEncDate.Value = common.myDate(hdnEncDate.Value).ToString(common.myStr(Session["OutputDateformat"]));

                lnkEncounterNo.Text = lnkEncounterNo.Text + "<br/>" + hdnEncDate.Value;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvEncounters_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            lblMessage.Text = "&nbsp;";


            if (e.CommandName == "SelectEncounter")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
                HiddenField hdnOPIP = (HiddenField)row.FindControl("hdnOPIP");

                Session["EncounterId"] = common.myInt(hdnEncounterId.Value);
                Session["EncounterNo"] = common.myStr(e.CommandArgument);
                ViewState["DoctorId"] = common.myInt(hdnDoctorId.Value);
                Session["OPIP"] = hdnOPIP.Value;

                bindPatientDetails();
                bindDetailsData();
            }
            if (e.CommandName == "CaseSheet")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
                HiddenField hdnOPIP = (HiddenField)row.FindControl("hdnOPIP");

                RadWindowForNew.NavigateUrl = "/EMR/Masters/ViewPatientHistory.aspx?RegId=" + common.myStr(Session["RegistrationId"]) +
                                            "&EncId=" + common.myInt(hdnEncounterId.Value) +
                                            "&DoctorId=" + common.myInt(hdnDoctorId.Value) + "&OPIP=" + common.myStr(hdnOPIP.Value);

                RadWindowForNew.Height = 560;
                RadWindowForNew.Width = 930;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                //RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    public void bindEncounters()
    {
        BaseC.Dashboard objCls = new BaseC.Dashboard();
        DataSet ds = new DataSet();
        try
        {
            ds = getPatientEncounterDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(Session["RegistrationId"]));

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvEncounters.DataSource = ds.Tables[0];
            gvEncounters.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }


    protected void lnkAddEncounters_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/EMR/Assessment/Encounters.aspx?From=POPUP";
            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 930;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addEncountersOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }


    protected void btnAddEncountersClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindEncounters();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    #endregion

    #region ChiefComplaints

    protected void gvChiefComplaints_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDetails = (Label)e.Row.FindControl("lblDetails");
                HiddenField hdnProblemDescription = (HiddenField)e.Row.FindControl("hdnProblemDescription");
                HiddenField hdnEncodedDate = (HiddenField)e.Row.FindControl("hdnEncodedDate");

                if (common.myLen(hdnProblemDescription.Value) > 0)
                {
                    lblDetails.Text = common.myDate(hdnEncodedDate.Value).ToString(common.myStr(Session["OutputDateformat"])) + "; " +
                                    common.myStr(hdnProblemDescription.Value).Trim();
                }
                else
                {
                    lblDetails.Text = "&nbsp;";
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

    protected void gvChiefComplaints_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            //if (e.CommandName == "AddToList")
            //{
            //    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            //    lblMessage.Text = "";
            //    int iCount = 0;
            //    string sReference = "";

            //    Label lblFieldName = (Label)row.FindControl("lblFieldName");
            //    HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");
            //    ViewState["FieldId"] = common.myInt(hdnFieldId.Value);

            //    int isCrosMaxLimit = 0;
            //    bool isAlredyExists = false;
            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        if (hdnFieldId.Value.Trim() == hdnFieldIdGrid.Value.Trim())
            //        {
            //            isAlredyExists = true;
            //        }
            //        if (hdnFieldIdGrid.Value.Trim() != "")
            //        {
            //            isCrosMaxLimit++;
            //        }
            //    }

            //    string strMsg = "";
            //    if (isAlredyExists)
            //    {
            //        strMsg += "Field Name '" + lblFieldName.Text + "' Already Exists !<br />";
            //    }

            //    //if (isCrosMaxLimit > 4)
            //    //{
            //    //    strMsg += "Maximum Field Name Limit are Five !";
            //    //    //return;
            //    //}

            //    if (strMsg != "")
            //    {
            //        lblMessage.Text = strMsg;
            //        return;
            //    }

            //    DataTable objDt = new DataTable();
            //    DataRow objDr;
            //    objDt = ((DataTable)ViewState["SelectedFieldsClone"]).Clone();
            //    if (objDt == null)
            //    {
            //        return;
            //    }

            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        Label lblFieldNameGrid = (Label)item.FindControl("lblFieldName");
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        Label lblReferenceName = (Label)item.FindControl("lblReferenceName");
            //        TextBox txtFormulaDefinition = (TextBox)item.FindControl("txtFormulaDefinition");
            //        CheckBox chkTotalCalc = (CheckBox)item.FindControl("chkTotalCalc");

            //        if (hdnFieldId.Value.Trim() != hdnFieldIdGrid.Value.Trim())
            //        {
            //            objDr = objDt.NewRow();
            //            if (hdnFieldIdGrid.Value.Trim() != "")
            //            {
            //                sReference = lblReferenceName.Text;
            //                objDr["FieldId"] = hdnFieldIdGrid.Value;
            //                objDr["FieldName"] = lblFieldNameGrid.Text;
            //                objDr["FormulaDefinition"] = txtFormulaDefinition.Text;
            //                objDr["TotalCalc"] = chkTotalCalc.Checked ? 1 : 0;

            //                if (iCount == 0)
            //                {
            //                    objDr["ReferenceName"] = "A";
            //                }
            //                else
            //                {
            //                    Label lblReferencePrevious = (Label)gvSelectedFields.Rows[iCount - 1].FindControl("lblReferenceName");
            //                    objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(lblReferencePrevious.Text)) + 1);
            //                }
            //                objDt.Rows.Add(objDr);

            //                iCount++;
            //            }
            //            else
            //            {
            //                objDr["ReferenceName"] = hdnFieldIdGrid.Value;
            //            }
            //        }
            //    }

            //    objDr = objDt.NewRow();
            //    objDr["FieldId"] = hdnFieldId.Value;
            //    objDr["FieldName"] = lblFieldName.Text;
            //    Label lblReferenceNamePrevious = (Label)gvSelectedFields.Rows[gvSelectedFields.Rows.Count - 1].FindControl("lblReferenceName");

            //    if (objDt.Rows.Count > 0)
            //    {
            //        string s = common.myStr(objDt.Rows[objDt.Rows.Count - 1]["ReferenceName"]);
            //        if (s.Trim() != "")
            //        {
            //            objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(s)) + 1);
            //            objDr["TotalCalc"] = 0;
            //        }
            //        else
            //        {
            //            objDr["ReferenceName"] = "A";
            //            objDr["TotalCalc"] = 0;
            //        }
            //    }
            //    else
            //    {
            //        objDr["ReferenceName"] = "A";
            //        objDr["TotalCalc"] = 0;
            //    }
            //    objDt.Rows.Add(objDr);


            //    gvSelectedFields.DataSource = objDt;
            //    gvSelectedFields.DataBind();
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvChiefComplaints_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvChiefComplaints.PageIndex = e.NewPageIndex;
        bindChiefComplaints();
    }

    public void bindChiefComplaints()
    {
        BaseC.Dashboard objCls = new BaseC.Dashboard();
        DataSet ds = new DataSet();
        try
        {
            ds = objCls.getDashBoardValue(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), common.myInt(Session["RegistrationId"]),
                                    common.myDate(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"), common.myDate(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                    common.myStr(ddlTime.SelectedValue), common.myInt(Session["EncounterId"]).ToString(),
                                    common.myInt(Session["FacilityID"]), "UspEMRGetPatientProblems");

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvChiefComplaints.DataSource = ds.Tables[0];
            gvChiefComplaints.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }

    protected void lnkAddChiefComplaints_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/EMR/Problems/Default.aspx?From=POPUP";
            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 930;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addChiefComplaintsOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void btnAddChiefComplaintsClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindChiefComplaints();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    #endregion

    #region Vitals

    protected void gvVitals_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int idx = 0; idx < e.Row.Cells.Count; idx++)
                {
                    if (e.Row.Cells[idx].Text.Contains("_")
                        || common.myStr(e.Row.Cells[idx].Text) == "0"
                        || common.myStr(e.Row.Cells[idx].Text) == "1"
                        || common.myStr(e.Row.Cells[idx].Text).ToUpper() == "VITALENTRYDATE1"
                        || common.myStr(e.Row.Cells[idx].Text).ToUpper() == "TEMPLATEFIELDID")
                    {
                        e.Row.Cells[idx].Visible = false;
                    }
                    else
                    {
                        e.Row.Cells[idx].Visible = true;
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[1].Visible = false;

                        if (common.myStr(this.gvVitals.HeaderRow.Cells[idx].Text).ToUpper() != "VITAL DATE"
                            && common.myStr(this.gvVitals.HeaderRow.Cells[idx].Text).ToUpper() != "ENTERED BY"
                            && common.myStr(e.Row.Cells[idx].Text).ToUpper() != "&NBSP;"
                            && common.myStr(e.Row.Cells[idx].Text).Trim() != "")
                        {
                            string HeaderColumn = common.myStr(this.gvVitals.HeaderRow.Cells[idx].Text);

                            e.Row.Cells[idx].Attributes.Add("onclick", "setVitalValue('" + common.myStr(e.Row.Cells[idx].Text) + "','" + HeaderColumn + "');");

                            if (common.myStr(e.Row.Cells[idx].Text) == "1")
                            {
                                if (ViewState["VitalsData"] != null)
                                {
                                    DataTable tbl = (DataTable)ViewState["VitalsData"];

                                    string strCol = HeaderColumn.Substring(0, HeaderColumn.IndexOf("_"));

                                    e.Row.Cells[tbl.Columns[strCol].Ordinal].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                        }

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

    protected void gvVitals_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVitals.PageIndex = e.NewPageIndex;
        bindVitals();
    }

    public void bindVitals()
    {
        BaseC.Dashboard objCls = new BaseC.Dashboard();
        DataSet ds = new DataSet();
        try
        {
            ds = objCls.getDashBoardValue(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), common.myInt(Session["RegistrationId"]),
                                        common.myDate(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"), common.myDate(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                        common.myStr(ddlTime.SelectedValue), common.myInt(Session["EncounterId"]).ToString(),
                                        common.myInt(Session["FacilityID"]), "UspEMRGetPatientPreviousVitals");

            ViewState["VitalsData"] = ds.Tables[0];

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvVitals.DataSource = ds.Tables[0];
                    gvVitals.DataBind();
                }
                else
                {
                    BindBlankGrid();
                }
            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }
    private void BindBlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("VitalDate");
        dt.Columns.Add("T");
        dt.Columns.Add("R");
        dt.Columns.Add("P");
        dt.Columns.Add("HT");
        dt.Columns.Add("WT");
        dt.Columns.Add("BPS");
        dt.Columns.Add("BPD");
        dt.Columns.Add("HC");
        dt.Columns.Add("MAC");
        dt.Columns.Add("BMI");
        dt.Columns.Add("BSA");
        dt.Columns.Add("Pain");
        dt.Columns.Add("SPO2");
        dt.Columns.Add("GRBS");
        dt.Columns.Add("Entered BY");
        DataRow dr = dt.NewRow();
        dr["VitalDate"] = "";
        dr["T"] = "";
        dr["R"] = "";
        dr["P"] = "";
        dr["HT"] = "";
        dr["WT"] = "";
        dr["BPS"] = "";
        dr["BPD"] = "";
        dr["HC"] = "";
        dr["MAC"] = "";
        dr["BMI"] = "";
        dr["BSA"] = "";
        dr["Pain"] = "";
        dr["SPO2"] = "";
        dr["GRBS"] = "";
        dr["Entered BY"] = "";
        dt.Rows.Add(dr);
        gvVitals.DataSource = dt;
        gvVitals.DataBind();
    }
    protected void lnkAddVitals_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/EMR/Vitals/Vitals.aspx?From=POPUP";
            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 930;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addVitalsOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void btnAddVitalsClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindVitals();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    #endregion

    #region Allergies

    protected void gvAllergies_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDetails = (Label)e.Row.FindControl("lblDetails");
                HiddenField hdnAllergyName = (HiddenField)e.Row.FindControl("hdnAllergyName");
                HiddenField hdnAllergyType = (HiddenField)e.Row.FindControl("hdnAllergyType");
                HiddenField hdnReaction = (HiddenField)e.Row.FindControl("hdnReaction");

                if (common.myLen(hdnAllergyName.Value) > 0)
                {
                    lblDetails.Text = common.myStr(hdnAllergyName.Value).Trim() + "; " +
                                "<strong>Type:</strong> " + common.myStr(hdnAllergyType.Value).Trim() + "; " +
                                "<strong>Reaction:</strong> " + common.myStr(hdnReaction.Value).Trim();
                }
                else
                {
                    lblDetails.Text = "&nbsp;";
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

    protected void gvAllergies_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            //if (e.CommandName == "AddToList")
            //{
            //    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            //    lblMessage.Text = "";
            //    int iCount = 0;
            //    string sReference = "";

            //    Label lblFieldName = (Label)row.FindControl("lblFieldName");
            //    HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");
            //    ViewState["FieldId"] = common.myInt(hdnFieldId.Value);

            //    int isCrosMaxLimit = 0;
            //    bool isAlredyExists = false;
            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        if (hdnFieldId.Value.Trim() == hdnFieldIdGrid.Value.Trim())
            //        {
            //            isAlredyExists = true;
            //        }
            //        if (hdnFieldIdGrid.Value.Trim() != "")
            //        {
            //            isCrosMaxLimit++;
            //        }
            //    }

            //    string strMsg = "";
            //    if (isAlredyExists)
            //    {
            //        strMsg += "Field Name '" + lblFieldName.Text + "' Already Exists !<br />";
            //    }

            //    //if (isCrosMaxLimit > 4)
            //    //{
            //    //    strMsg += "Maximum Field Name Limit are Five !";
            //    //    //return;
            //    //}

            //    if (strMsg != "")
            //    {
            //        lblMessage.Text = strMsg;
            //        return;
            //    }

            //    DataTable objDt = new DataTable();
            //    DataRow objDr;
            //    objDt = ((DataTable)ViewState["SelectedFieldsClone"]).Clone();
            //    if (objDt == null)
            //    {
            //        return;
            //    }

            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        Label lblFieldNameGrid = (Label)item.FindControl("lblFieldName");
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        Label lblReferenceName = (Label)item.FindControl("lblReferenceName");
            //        TextBox txtFormulaDefinition = (TextBox)item.FindControl("txtFormulaDefinition");
            //        CheckBox chkTotalCalc = (CheckBox)item.FindControl("chkTotalCalc");

            //        if (hdnFieldId.Value.Trim() != hdnFieldIdGrid.Value.Trim())
            //        {
            //            objDr = objDt.NewRow();
            //            if (hdnFieldIdGrid.Value.Trim() != "")
            //            {
            //                sReference = lblReferenceName.Text;
            //                objDr["FieldId"] = hdnFieldIdGrid.Value;
            //                objDr["FieldName"] = lblFieldNameGrid.Text;
            //                objDr["FormulaDefinition"] = txtFormulaDefinition.Text;
            //                objDr["TotalCalc"] = chkTotalCalc.Checked ? 1 : 0;

            //                if (iCount == 0)
            //                {
            //                    objDr["ReferenceName"] = "A";
            //                }
            //                else
            //                {
            //                    Label lblReferencePrevious = (Label)gvSelectedFields.Rows[iCount - 1].FindControl("lblReferenceName");
            //                    objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(lblReferencePrevious.Text)) + 1);
            //                }
            //                objDt.Rows.Add(objDr);

            //                iCount++;
            //            }
            //            else
            //            {
            //                objDr["ReferenceName"] = hdnFieldIdGrid.Value;
            //            }
            //        }
            //    }

            //    objDr = objDt.NewRow();
            //    objDr["FieldId"] = hdnFieldId.Value;
            //    objDr["FieldName"] = lblFieldName.Text;
            //    Label lblReferenceNamePrevious = (Label)gvSelectedFields.Rows[gvSelectedFields.Rows.Count - 1].FindControl("lblReferenceName");

            //    if (objDt.Rows.Count > 0)
            //    {
            //        string s = common.myStr(objDt.Rows[objDt.Rows.Count - 1]["ReferenceName"]);
            //        if (s.Trim() != "")
            //        {
            //            objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(s)) + 1);
            //            objDr["TotalCalc"] = 0;
            //        }
            //        else
            //        {
            //            objDr["ReferenceName"] = "A";
            //            objDr["TotalCalc"] = 0;
            //        }
            //    }
            //    else
            //    {
            //        objDr["ReferenceName"] = "A";
            //        objDr["TotalCalc"] = 0;
            //    }
            //    objDt.Rows.Add(objDr);


            //    gvSelectedFields.DataSource = objDt;
            //    gvSelectedFields.DataBind();
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvAllergies_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAllergies.PageIndex = e.NewPageIndex;
        bindAllergies();
    }

    public void bindAllergies()
    {
        BaseC.Dashboard objCls = new BaseC.Dashboard();
        DataSet ds = new DataSet();
        bool bindData = false;
        try
        {
            lblAllergiesEmptyData.Visible = false;
            ds = objCls.getAllergies(common.myInt(Session["RegistrationId"]));

            if (ds.Tables[1].Rows.Count > 0)
            {
                if (common.myBool(ds.Tables[1].Rows[0]["NoAllergies"]))
                {
                    lblAllergiesEmptyData.Visible = true;
                }
                else
                {
                    bindData = true;
                }
            }
            else
            {
                bindData = true;
            }

            if (bindData)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    DataRow DR = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(DR);
                }

                gvAllergies.DataSource = ds.Tables[0];
                gvAllergies.DataBind();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }


    protected void lnkAddAllergies_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/EMR/Allergy/Allergy.aspx?Regno=" + common.myLong(Session["RegistrationNo"]) + "&Encno=" + common.myStr(Session["EncounterNo"]) + "&From=POPUP&Source=IPD";
            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 930;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addAllergiesOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }


    protected void btnAddAllergiesClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindAllergies();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    #endregion

    #region Templates

    protected void gvTemplates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
                //Label lblDetails = (Label)e.Row.FindControl("lblDetails");
                //HiddenField hdnICDDescription = (HiddenField)e.Row.FindControl("hdnICDDescription");
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvTemplates_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "SelectTemplate")
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    RadWindowForNew.NavigateUrl = "~/EMR/Dashboard/PatientParts/TemplateData.aspx?TemplateId=" + common.myInt(e.CommandArgument);

                    RadWindowForNew.Height = 560;
                    RadWindowForNew.Width = 930;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    //RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
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

    protected void gvTemplates_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTemplates.PageIndex = e.NewPageIndex;
        bindTemplates();
    }

    public void bindTemplates()
    {
        BaseC.clsEMR objCls = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objCls.getTemplateEnteredList(common.myInt(Session["HospitalLocationId"]),
                            common.myInt(Session["RegistrationID"]), common.myInt(Session["EncounterId"]));

            DataTable tbl = new DataTable();

            DataColumn col = new DataColumn("TemplateId");
            tbl.Columns.Add(col);
            DataColumn col2 = new DataColumn("TemplateName");
            tbl.Columns.Add(col2);

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                tbl.DefaultView.RowFilter = "TemplateId=" + common.myInt(item["TemplateId"]);
                if (tbl.DefaultView.Count == 0)
                {
                    DataRow DR = tbl.NewRow();
                    DR["TemplateId"] = common.myInt(item["TemplateId"]);
                    DR["TemplateName"] = common.myStr(item["TemplateName"]);

                    tbl.Rows.Add(DR);
                }
                tbl.DefaultView.RowFilter = "";
            }

            if (tbl.Rows.Count == 0)
            {
                DataRow DR = tbl.NewRow();
                tbl.Rows.Add(DR);
            }

            gvTemplates.DataSource = tbl;
            gvTemplates.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }


    protected void lnkAddTemplates_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?From=POPUP";
            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 930;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addTemplatesOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }


    protected void btnAddTemplatesClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindTemplates();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    #endregion

    #region Diagnosis

    protected void gvDiagnosis_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDetails = (Label)e.Row.FindControl("lblDetails");
                HiddenField hdnICDDescription = (HiddenField)e.Row.FindControl("hdnICDDescription");
                HiddenField hdnICDCode = (HiddenField)e.Row.FindControl("hdnICDCode");
                HiddenField hdnDiagnosisCondition1 = (HiddenField)e.Row.FindControl("hdnDiagnosisCondition1");
                HiddenField hdnDiagnosisCondition2 = (HiddenField)e.Row.FindControl("hdnDiagnosisCondition2");
                HiddenField hdnDiagnosisCondition3 = (HiddenField)e.Row.FindControl("hdnDiagnosisCondition3");
                HiddenField hdnEntryDate = (HiddenField)e.Row.FindControl("hdnEntryDate");
                HiddenField hdnISChronic = (HiddenField)e.Row.FindControl("hdnISChronic");
                HiddenField hdnISResolved = (HiddenField)e.Row.FindControl("hdnISResolved");
                HiddenField hdnPrimaryDiagnosis = (HiddenField)e.Row.FindControl("hdnPrimaryDiagnosis");

                string strStatus = string.Empty;
                string strCondition = string.Empty;

                if (common.myBool(hdnISResolved.Value))
                {
                    strStatus = "Resolved";
                }
                else
                {
                    if (common.myBool(hdnISChronic.Value))
                    {
                        strStatus = "Chronic";
                    }
                    if (common.myBool(hdnPrimaryDiagnosis.Value))
                    {
                        strStatus = "Primary";
                    }
                }

                if (common.myLen(hdnDiagnosisCondition1.Value) > 0)
                {
                    strCondition = common.myStr(hdnDiagnosisCondition1.Value);
                }

                if (common.myLen(hdnDiagnosisCondition2.Value) > 0)
                {
                    if (strCondition != string.Empty)
                    {
                        strCondition += ", ";
                    }
                    strCondition += common.myStr(hdnDiagnosisCondition2.Value);
                }

                if (common.myLen(hdnDiagnosisCondition3.Value) > 0)
                {
                    if (strCondition != string.Empty)
                    {
                        strCondition += ", ";
                    }
                    strCondition += common.myStr(hdnDiagnosisCondition3.Value);
                }

                if (common.myLen(hdnICDDescription.Value) > 0)
                {
                    lblDetails.Text = common.myStr(hdnEntryDate.Value).Trim() + "; " +
                                common.myStr(hdnICDDescription.Value).Trim() + "; " +
                                "<strong>ICD Code:</strong> " + common.myStr(hdnICDCode.Value).Trim() + "; " +
                                "<strong>Status:</strong> " + strStatus + "; " +
                                "<strong>Condition:</strong> " + strCondition;
                }
                else
                {
                    lblDetails.Text = "&nbsp;";
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

    protected void gvDiagnosis_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            //if (e.CommandName == "AddToList")
            //{
            //    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            //    lblMessage.Text = "";
            //    int iCount = 0;
            //    string sReference = "";

            //    Label lblFieldName = (Label)row.FindControl("lblFieldName");
            //    HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");
            //    ViewState["FieldId"] = common.myInt(hdnFieldId.Value);

            //    int isCrosMaxLimit = 0;
            //    bool isAlredyExists = false;
            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        if (hdnFieldId.Value.Trim() == hdnFieldIdGrid.Value.Trim())
            //        {
            //            isAlredyExists = true;
            //        }
            //        if (hdnFieldIdGrid.Value.Trim() != "")
            //        {
            //            isCrosMaxLimit++;
            //        }
            //    }

            //    string strMsg = "";
            //    if (isAlredyExists)
            //    {
            //        strMsg += "Field Name '" + lblFieldName.Text + "' Already Exists !<br />";
            //    }

            //    //if (isCrosMaxLimit > 4)
            //    //{
            //    //    strMsg += "Maximum Field Name Limit are Five !";
            //    //    //return;
            //    //}

            //    if (strMsg != "")
            //    {
            //        lblMessage.Text = strMsg;
            //        return;
            //    }

            //    DataTable objDt = new DataTable();
            //    DataRow objDr;
            //    objDt = ((DataTable)ViewState["SelectedFieldsClone"]).Clone();
            //    if (objDt == null)
            //    {
            //        return;
            //    }

            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        Label lblFieldNameGrid = (Label)item.FindControl("lblFieldName");
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        Label lblReferenceName = (Label)item.FindControl("lblReferenceName");
            //        TextBox txtFormulaDefinition = (TextBox)item.FindControl("txtFormulaDefinition");
            //        CheckBox chkTotalCalc = (CheckBox)item.FindControl("chkTotalCalc");

            //        if (hdnFieldId.Value.Trim() != hdnFieldIdGrid.Value.Trim())
            //        {
            //            objDr = objDt.NewRow();
            //            if (hdnFieldIdGrid.Value.Trim() != "")
            //            {
            //                sReference = lblReferenceName.Text;
            //                objDr["FieldId"] = hdnFieldIdGrid.Value;
            //                objDr["FieldName"] = lblFieldNameGrid.Text;
            //                objDr["FormulaDefinition"] = txtFormulaDefinition.Text;
            //                objDr["TotalCalc"] = chkTotalCalc.Checked ? 1 : 0;

            //                if (iCount == 0)
            //                {
            //                    objDr["ReferenceName"] = "A";
            //                }
            //                else
            //                {
            //                    Label lblReferencePrevious = (Label)gvSelectedFields.Rows[iCount - 1].FindControl("lblReferenceName");
            //                    objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(lblReferencePrevious.Text)) + 1);
            //                }
            //                objDt.Rows.Add(objDr);

            //                iCount++;
            //            }
            //            else
            //            {
            //                objDr["ReferenceName"] = hdnFieldIdGrid.Value;
            //            }
            //        }
            //    }

            //    objDr = objDt.NewRow();
            //    objDr["FieldId"] = hdnFieldId.Value;
            //    objDr["FieldName"] = lblFieldName.Text;
            //    Label lblReferenceNamePrevious = (Label)gvSelectedFields.Rows[gvSelectedFields.Rows.Count - 1].FindControl("lblReferenceName");

            //    if (objDt.Rows.Count > 0)
            //    {
            //        string s = common.myStr(objDt.Rows[objDt.Rows.Count - 1]["ReferenceName"]);
            //        if (s.Trim() != "")
            //        {
            //            objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(s)) + 1);
            //            objDr["TotalCalc"] = 0;
            //        }
            //        else
            //        {
            //            objDr["ReferenceName"] = "A";
            //            objDr["TotalCalc"] = 0;
            //        }
            //    }
            //    else
            //    {
            //        objDr["ReferenceName"] = "A";
            //        objDr["TotalCalc"] = 0;
            //    }
            //    objDt.Rows.Add(objDr);


            //    gvSelectedFields.DataSource = objDt;
            //    gvSelectedFields.DataBind();
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvDiagnosis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDiagnosis.PageIndex = e.NewPageIndex;
        bindDiagnosis();
    }

    public void bindDiagnosis()
    {
        BaseC.Dashboard objCls = new BaseC.Dashboard();
        DataSet ds = new DataSet();
        try
        {
            ds = objCls.getDashBoardValue(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), common.myInt(Session["RegistrationId"]),
                                        common.myDate(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"), common.myDate(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                        common.myStr(ddlTime.SelectedValue), common.myInt(Session["EncounterId"]).ToString(),
                                        common.myInt(Session["FacilityID"]), "UspEMRGetPatientDiagnosis");


            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvDiagnosis.DataSource = ds.Tables[0];
            gvDiagnosis.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }


    protected void lnkAddDiagnosis_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/EMR/Assessment/Diagnosis.aspx?From=POPUP";
            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 930;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addDiagnosisOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }


    protected void btnAddDiagnosisClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindDiagnosis();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    #endregion

    #region OrdersAndProcedures

    protected void gvOrdersAndProcedures_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDetails = (Label)e.Row.FindControl("lblDetails");
                HiddenField hdnServiceName = (HiddenField)e.Row.FindControl("hdnServiceName");
                HiddenField hdnOrderDate = (HiddenField)e.Row.FindControl("hdnOrderDate");
                HiddenField hdnLabStatus = (HiddenField)e.Row.FindControl("hdnLabStatus");

                if (common.myLen(hdnServiceName.Value) > 0)
                {
                    lblDetails.Text = common.myStr(hdnOrderDate.Value).Trim() + "; " +
                                   common.myStr(hdnServiceName.Value).Trim() + "; " +
                                   common.myStr(hdnLabStatus.Value);
                }
                else
                {
                    lblDetails.Text = "&nbsp;";
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

    protected void gvOrdersAndProcedures_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            //if (e.CommandName == "AddToList")
            //{
            //    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            //    lblMessage.Text = "";
            //    int iCount = 0;
            //    string sReference = "";

            //    Label lblFieldName = (Label)row.FindControl("lblFieldName");
            //    HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");
            //    ViewState["FieldId"] = common.myInt(hdnFieldId.Value);

            //    int isCrosMaxLimit = 0;
            //    bool isAlredyExists = false;
            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        if (hdnFieldId.Value.Trim() == hdnFieldIdGrid.Value.Trim())
            //        {
            //            isAlredyExists = true;
            //        }
            //        if (hdnFieldIdGrid.Value.Trim() != "")
            //        {
            //            isCrosMaxLimit++;
            //        }
            //    }

            //    string strMsg = "";
            //    if (isAlredyExists)
            //    {
            //        strMsg += "Field Name '" + lblFieldName.Text + "' Already Exists !<br />";
            //    }

            //    //if (isCrosMaxLimit > 4)
            //    //{
            //    //    strMsg += "Maximum Field Name Limit are Five !";
            //    //    //return;
            //    //}

            //    if (strMsg != "")
            //    {
            //        lblMessage.Text = strMsg;
            //        return;
            //    }

            //    DataTable objDt = new DataTable();
            //    DataRow objDr;
            //    objDt = ((DataTable)ViewState["SelectedFieldsClone"]).Clone();
            //    if (objDt == null)
            //    {
            //        return;
            //    }

            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        Label lblFieldNameGrid = (Label)item.FindControl("lblFieldName");
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        Label lblReferenceName = (Label)item.FindControl("lblReferenceName");
            //        TextBox txtFormulaDefinition = (TextBox)item.FindControl("txtFormulaDefinition");
            //        CheckBox chkTotalCalc = (CheckBox)item.FindControl("chkTotalCalc");

            //        if (hdnFieldId.Value.Trim() != hdnFieldIdGrid.Value.Trim())
            //        {
            //            objDr = objDt.NewRow();
            //            if (hdnFieldIdGrid.Value.Trim() != "")
            //            {
            //                sReference = lblReferenceName.Text;
            //                objDr["FieldId"] = hdnFieldIdGrid.Value;
            //                objDr["FieldName"] = lblFieldNameGrid.Text;
            //                objDr["FormulaDefinition"] = txtFormulaDefinition.Text;
            //                objDr["TotalCalc"] = chkTotalCalc.Checked ? 1 : 0;

            //                if (iCount == 0)
            //                {
            //                    objDr["ReferenceName"] = "A";
            //                }
            //                else
            //                {
            //                    Label lblReferencePrevious = (Label)gvSelectedFields.Rows[iCount - 1].FindControl("lblReferenceName");
            //                    objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(lblReferencePrevious.Text)) + 1);
            //                }
            //                objDt.Rows.Add(objDr);

            //                iCount++;
            //            }
            //            else
            //            {
            //                objDr["ReferenceName"] = hdnFieldIdGrid.Value;
            //            }
            //        }
            //    }

            //    objDr = objDt.NewRow();
            //    objDr["FieldId"] = hdnFieldId.Value;
            //    objDr["FieldName"] = lblFieldName.Text;
            //    Label lblReferenceNamePrevious = (Label)gvSelectedFields.Rows[gvSelectedFields.Rows.Count - 1].FindControl("lblReferenceName");

            //    if (objDt.Rows.Count > 0)
            //    {
            //        string s = common.myStr(objDt.Rows[objDt.Rows.Count - 1]["ReferenceName"]);
            //        if (s.Trim() != "")
            //        {
            //            objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(s)) + 1);
            //            objDr["TotalCalc"] = 0;
            //        }
            //        else
            //        {
            //            objDr["ReferenceName"] = "A";
            //            objDr["TotalCalc"] = 0;
            //        }
            //    }
            //    else
            //    {
            //        objDr["ReferenceName"] = "A";
            //        objDr["TotalCalc"] = 0;
            //    }
            //    objDt.Rows.Add(objDr);


            //    gvSelectedFields.DataSource = objDt;
            //    gvSelectedFields.DataBind();
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvOrdersAndProcedures_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOrdersAndProcedures.PageIndex = e.NewPageIndex;
        bindOrdersAndProcedures();
    }

    public void bindOrdersAndProcedures()
    {
        BaseC.Dashboard objCls = new BaseC.Dashboard();
        DataSet ds = new DataSet();
        try
        {
            ds = objCls.getDashBoardValue(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), common.myInt(Session["RegistrationId"]),
                                        common.myDate(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"), common.myDate(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                        common.myStr(ddlTime.SelectedValue), common.myInt(Session["EncounterId"]).ToString(),
                                        common.myInt(Session["FacilityID"]), "UspEMRGetPatientServices");

            DataView DV = ds.Tables[0].DefaultView;
            DV.RowFilter = "ServiceType IN('I','IS')";

            ds.Tables.RemoveAt(0);
            ds.Tables.Add(DV.ToTable());

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvOrdersAndProcedures.DataSource = ds.Tables[0];
            gvOrdersAndProcedures.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }


    protected void lnkAddOrdersAndProcedures_OnClick(object sender, EventArgs e)
    {
        try
        {
            //RadWindowForNew.NavigateUrl = "/EMRBILLING/Popup/AddServices.aspx?Regid=" + common.myInt(Session["RegistrationID"]) +
            //            "&RegNo=" + common.myInt(Session["Regno"]) +
            //            "&EncId=" + common.myInt(Session["encounterid"]) +
            //            "&EncNo=" + common.myInt(Session["Encno"]) +
            //            "&OP_IP=I&FromWard=Y&CompanyId=0&InsuranceId=0&CardId=0&PayerType=&BType=";

            RadWindowForNew.NavigateUrl = "/EMR/Orders/Orders.aspx?From=POPUP";
            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 930;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addOrdersAndProceduresOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }


    protected void btnAddOrdersAndProceduresClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindOrdersAndProcedures();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    #endregion

    #region LabResults

    protected void gvLabResults_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDetails = (Label)e.Row.FindControl("lblDetails");

                HiddenField hdnFieldName = (HiddenField)e.Row.FindControl("hdnFieldName");

                if (common.myLen(hdnFieldName.Value) > 0)
                {
                    HiddenField hdnResultDate = (HiddenField)e.Row.FindControl("hdnResultDate");
                    HiddenField hdnResult = (HiddenField)e.Row.FindControl("hdnResult");
                    HiddenField hdnUnitName = (HiddenField)e.Row.FindControl("hdnUnitName");
                    HiddenField hdnPatientName = (HiddenField)e.Row.FindControl("hdnPatientName");
                    HiddenField hdnRegistrationNo = (HiddenField)e.Row.FindControl("hdnRegistrationNo");
                    HiddenField hdnOrderDate = (HiddenField)e.Row.FindControl("hdnOrderDate");
                    HiddenField hdnSampleCollectedDate = (HiddenField)e.Row.FindControl("hdnSampleCollectedDate");
                    HiddenField hdnLabFlagValue = (HiddenField)e.Row.FindControl("hdnLabFlagValue");
                    HiddenField hdnTestResultStatus = (HiddenField)e.Row.FindControl("hdnTestResultStatus");
                    HiddenField hdnReviewedComments = (HiddenField)e.Row.FindControl("hdnReviewedComments");
                    HiddenField hdnMinValue = (HiddenField)e.Row.FindControl("hdnMinValue");
                    HiddenField hdnSymbol = (HiddenField)e.Row.FindControl("hdnSymbol");
                    HiddenField hdnMaxValue = (HiddenField)e.Row.FindControl("hdnMaxValue");
                    HiddenField hdnAbnormalValue = (HiddenField)e.Row.FindControl("hdnAbnormalValue");
                    HiddenField hdnFieldType = (HiddenField)e.Row.FindControl("hdnFieldType");
                    HiddenField hdnReviewedStatus = (HiddenField)e.Row.FindControl("hdnReviewedStatus");

                    string strReferenceRange = string.Empty;

                    if (common.myStr(hdnFieldType.Value) == "N")
                    {
                        if (common.myLen(hdnMinValue.Value) == 0
                        && common.myLen(hdnMinValue.Value) == 0)
                        {
                            strReferenceRange = "(Range: Undefined)";
                        }

                        if (common.myLen(hdnMinValue.Value) > 0
                            && common.myLen(hdnMaxValue.Value) > 0)
                        {
                            strReferenceRange += "(" + common.myDbl(hdnMinValue.Value) + " " + common.myStr(hdnSymbol.Value) + " " + common.myDbl(hdnMaxValue.Value) + ")";
                        }
                    }

                    lblDetails.Text = common.myStr(hdnResultDate.Value).Trim() + "; " +
                                    "<strong>Test Name:</strong> " + common.myStr(hdnFieldName.Value).Trim() + "; " +
                                    "<strong>Result:</strong> " + common.myStr(hdnResult.Value).Trim() + "; " +
                                    "<strong>Unit:</strong> " + common.myStr(hdnUnitName.Value).Trim() + "; " +
                                    "<strong>Reference Range:</strong> " + strReferenceRange + "; " +
                                    "<strong>Patient Name:</strong> " + common.myStr(hdnPatientName.Value).Trim() + "; " +
                                    "<strong>Reg#:</strong> " + common.myStr(hdnRegistrationNo.Value).Trim() + "; " +
                                    "<strong>Order Date:</strong> " + common.myStr(hdnOrderDate.Value).Trim() + "; " +
                                    "<strong>Sample Collected Date:</strong> " + common.myStr(hdnSampleCollectedDate.Value).Trim() + "; " +
                                    "<strong>Lab Flag Value:</strong> " + common.myStr(hdnLabFlagValue.Value).Trim() + "; " +
                                    "<strong>Test Result Status:</strong> " + common.myStr(hdnTestResultStatus.Value).Trim() + "; " +
                                    "<strong>Comments:</strong> " + common.myStr(hdnReviewedComments.Value).Trim();

                    if (common.myBool(hdnAbnormalValue.Value))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightYellow;
                    }
                    if (common.myInt(hdnReviewedStatus.Value) > 0)
                    {
                        e.Row.BackColor = System.Drawing.Color.LightGreen;
                    }
                }
                else
                {
                    lblDetails.Text = "&nbsp;";
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

    protected void gvLabResults_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            //if (e.CommandName == "AddToList")
            //{
            //    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            //    lblMessage.Text = "";
            //    int iCount = 0;
            //    string sReference = "";

            //    Label lblFieldName = (Label)row.FindControl("lblFieldName");
            //    HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");
            //    ViewState["FieldId"] = common.myInt(hdnFieldId.Value);

            //    int isCrosMaxLimit = 0;
            //    bool isAlredyExists = false;
            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        if (hdnFieldId.Value.Trim() == hdnFieldIdGrid.Value.Trim())
            //        {
            //            isAlredyExists = true;
            //        }
            //        if (hdnFieldIdGrid.Value.Trim() != "")
            //        {
            //            isCrosMaxLimit++;
            //        }
            //    }

            //    string strMsg = "";
            //    if (isAlredyExists)
            //    {
            //        strMsg += "Field Name '" + lblFieldName.Text + "' Already Exists !<br />";
            //    }

            //    //if (isCrosMaxLimit > 4)
            //    //{
            //    //    strMsg += "Maximum Field Name Limit are Five !";
            //    //    //return;
            //    //}

            //    if (strMsg != "")
            //    {
            //        lblMessage.Text = strMsg;
            //        return;
            //    }

            //    DataTable objDt = new DataTable();
            //    DataRow objDr;
            //    objDt = ((DataTable)ViewState["SelectedFieldsClone"]).Clone();
            //    if (objDt == null)
            //    {
            //        return;
            //    }

            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        Label lblFieldNameGrid = (Label)item.FindControl("lblFieldName");
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        Label lblReferenceName = (Label)item.FindControl("lblReferenceName");
            //        TextBox txtFormulaDefinition = (TextBox)item.FindControl("txtFormulaDefinition");
            //        CheckBox chkTotalCalc = (CheckBox)item.FindControl("chkTotalCalc");

            //        if (hdnFieldId.Value.Trim() != hdnFieldIdGrid.Value.Trim())
            //        {
            //            objDr = objDt.NewRow();
            //            if (hdnFieldIdGrid.Value.Trim() != "")
            //            {
            //                sReference = lblReferenceName.Text;
            //                objDr["FieldId"] = hdnFieldIdGrid.Value;
            //                objDr["FieldName"] = lblFieldNameGrid.Text;
            //                objDr["FormulaDefinition"] = txtFormulaDefinition.Text;
            //                objDr["TotalCalc"] = chkTotalCalc.Checked ? 1 : 0;

            //                if (iCount == 0)
            //                {
            //                    objDr["ReferenceName"] = "A";
            //                }
            //                else
            //                {
            //                    Label lblReferencePrevious = (Label)gvSelectedFields.Rows[iCount - 1].FindControl("lblReferenceName");
            //                    objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(lblReferencePrevious.Text)) + 1);
            //                }
            //                objDt.Rows.Add(objDr);

            //                iCount++;
            //            }
            //            else
            //            {
            //                objDr["ReferenceName"] = hdnFieldIdGrid.Value;
            //            }
            //        }
            //    }

            //    objDr = objDt.NewRow();
            //    objDr["FieldId"] = hdnFieldId.Value;
            //    objDr["FieldName"] = lblFieldName.Text;
            //    Label lblReferenceNamePrevious = (Label)gvSelectedFields.Rows[gvSelectedFields.Rows.Count - 1].FindControl("lblReferenceName");

            //    if (objDt.Rows.Count > 0)
            //    {
            //        string s = common.myStr(objDt.Rows[objDt.Rows.Count - 1]["ReferenceName"]);
            //        if (s.Trim() != "")
            //        {
            //            objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(s)) + 1);
            //            objDr["TotalCalc"] = 0;
            //        }
            //        else
            //        {
            //            objDr["ReferenceName"] = "A";
            //            objDr["TotalCalc"] = 0;
            //        }
            //    }
            //    else
            //    {
            //        objDr["ReferenceName"] = "A";
            //        objDr["TotalCalc"] = 0;
            //    }
            //    objDt.Rows.Add(objDr);


            //    gvSelectedFields.DataSource = objDt;
            //    gvSelectedFields.DataBind();
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvLabResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLabResults.PageIndex = e.NewPageIndex;
        bindLabResults();
    }

    public void bindLabResults()
    {
        BaseC.clsLISLabOther objCls = new BaseC.clsLISLabOther(sConString);
        DataSet ds = new DataSet();
        try
        {

            ds = objCls.GetLabTestResult(common.myInt(Session["HospitalLocationID"]),
                            common.myInt(Session["FacilityID"]), 0, common.myInt(0), common.myInt(Session["RegistrationId"]),
                            0, common.myInt(Session["EncounterId"]), 2, common.myDate(dtpFromDate.SelectedDate.Value), common.myDate(dtpToDate.SelectedDate.Value),
                            common.myInt(Session["UserID"]));

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            //if (Session["MUDMeasure"] != null && Convert.ToBoolean(Session["MUDMeasure"]) == true)
            //{
            //    #region Log if a test result is received in encounter
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        int labCount = gvLabResults.Rows.Count;
            //        Hashtable logHash = new Hashtable();
            //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //        logHash.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));
            //        logHash.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
            //        logHash.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
            //        logHash.Add("@intDoctorId", Convert.ToInt32(Session["DoctorID"]));
            //        logHash.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
            //        logHash.Add("@intLabCount", Convert.ToInt32(labCount));
            //        objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogLabResultTest", logHash);
            //    }
            //    #endregion
            //}

            gvLabResults.DataSource = ds.Tables[0];
            gvLabResults.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }

    protected void btnAddLabResultsClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindLabResults();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void lnkShowLabHistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]);

            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 930;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            //RadWindowForNew.OnClientClose = "addPrescriptionsOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    #endregion

    #region Prescriptions

    protected void gvPrescriptions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDetails = (Label)e.Row.FindControl("lblDetails");
                HiddenField hdnItemName = (HiddenField)e.Row.FindControl("hdnItemName");
                HiddenField hdnStartDate = (HiddenField)e.Row.FindControl("hdnStartDate");
                HiddenField hdnDose = (HiddenField)e.Row.FindControl("hdnDose");
                HiddenField hdnUnitName = (HiddenField)e.Row.FindControl("hdnUnitName");
                HiddenField hdnQty = (HiddenField)e.Row.FindControl("hdnQty");

                if (common.myLen(hdnItemName.Value) > 0)
                {
                    lblDetails.Text = common.myStr(hdnStartDate.Value).Trim() + "; " +
                     common.myStr(hdnItemName.Value).Trim() + "; " +
                        //"<strong>Dose:</strong> " + common.myStr(hdnDose.Value).Trim() + "; " +
                        //"<strong>Units:</strong> " + 
                     common.myStr(hdnUnitName.Value).Trim() + "; " +
                     "<strong>Qty:</strong> " + common.myStr(hdnQty.Value).Trim();
                }
                else
                {
                    lblDetails.Text = "&nbsp;";
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

    protected void gvPrescriptions_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            //if (e.CommandName == "AddToList")
            //{
            //    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            //    lblMessage.Text = "";
            //    int iCount = 0;
            //    string sReference = "";

            //    Label lblFieldName = (Label)row.FindControl("lblFieldName");
            //    HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");
            //    ViewState["FieldId"] = common.myInt(hdnFieldId.Value);

            //    int isCrosMaxLimit = 0;
            //    bool isAlredyExists = false;
            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        if (hdnFieldId.Value.Trim() == hdnFieldIdGrid.Value.Trim())
            //        {
            //            isAlredyExists = true;
            //        }
            //        if (hdnFieldIdGrid.Value.Trim() != "")
            //        {
            //            isCrosMaxLimit++;
            //        }
            //    }

            //    string strMsg = "";
            //    if (isAlredyExists)
            //    {
            //        strMsg += "Field Name '" + lblFieldName.Text + "' Already Exists !<br />";
            //    }

            //    //if (isCrosMaxLimit > 4)
            //    //{
            //    //    strMsg += "Maximum Field Name Limit are Five !";
            //    //    //return;
            //    //}

            //    if (strMsg != "")
            //    {
            //        lblMessage.Text = strMsg;
            //        return;
            //    }

            //    DataTable objDt = new DataTable();
            //    DataRow objDr;
            //    objDt = ((DataTable)ViewState["SelectedFieldsClone"]).Clone();
            //    if (objDt == null)
            //    {
            //        return;
            //    }

            //    foreach (GridViewRow item in gvSelectedFields.Rows)
            //    {
            //        Label lblFieldNameGrid = (Label)item.FindControl("lblFieldName");
            //        HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            //        Label lblReferenceName = (Label)item.FindControl("lblReferenceName");
            //        TextBox txtFormulaDefinition = (TextBox)item.FindControl("txtFormulaDefinition");
            //        CheckBox chkTotalCalc = (CheckBox)item.FindControl("chkTotalCalc");

            //        if (hdnFieldId.Value.Trim() != hdnFieldIdGrid.Value.Trim())
            //        {
            //            objDr = objDt.NewRow();
            //            if (hdnFieldIdGrid.Value.Trim() != "")
            //            {
            //                sReference = lblReferenceName.Text;
            //                objDr["FieldId"] = hdnFieldIdGrid.Value;
            //                objDr["FieldName"] = lblFieldNameGrid.Text;
            //                objDr["FormulaDefinition"] = txtFormulaDefinition.Text;
            //                objDr["TotalCalc"] = chkTotalCalc.Checked ? 1 : 0;

            //                if (iCount == 0)
            //                {
            //                    objDr["ReferenceName"] = "A";
            //                }
            //                else
            //                {
            //                    Label lblReferencePrevious = (Label)gvSelectedFields.Rows[iCount - 1].FindControl("lblReferenceName");
            //                    objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(lblReferencePrevious.Text)) + 1);
            //                }
            //                objDt.Rows.Add(objDr);

            //                iCount++;
            //            }
            //            else
            //            {
            //                objDr["ReferenceName"] = hdnFieldIdGrid.Value;
            //            }
            //        }
            //    }

            //    objDr = objDt.NewRow();
            //    objDr["FieldId"] = hdnFieldId.Value;
            //    objDr["FieldName"] = lblFieldName.Text;
            //    Label lblReferenceNamePrevious = (Label)gvSelectedFields.Rows[gvSelectedFields.Rows.Count - 1].FindControl("lblReferenceName");

            //    if (objDt.Rows.Count > 0)
            //    {
            //        string s = common.myStr(objDt.Rows[objDt.Rows.Count - 1]["ReferenceName"]);
            //        if (s.Trim() != "")
            //        {
            //            objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(s)) + 1);
            //            objDr["TotalCalc"] = 0;
            //        }
            //        else
            //        {
            //            objDr["ReferenceName"] = "A";
            //            objDr["TotalCalc"] = 0;
            //        }
            //    }
            //    else
            //    {
            //        objDr["ReferenceName"] = "A";
            //        objDr["TotalCalc"] = 0;
            //    }
            //    objDt.Rows.Add(objDr);


            //    gvSelectedFields.DataSource = objDt;
            //    gvSelectedFields.DataBind();
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvPrescriptions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPrescriptions.PageIndex = e.NewPageIndex;
        bindPrescriptions();
    }

    public void bindPrescriptions()
    {
        BaseC.clsPharmacy objCls = new BaseC.clsPharmacy(sConString);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (Session["OPIP"] != null && Session["OPIP"].ToString() == "I")
            {
                ds = objCls.getPreviousMedicines(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["EncounterId"]));
            }
            else
            {
                ds = objEMR.getPreviousMedicinesOP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["EncounterId"]), 0);
            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }
            gvPrescriptions.DataSource = ds.Tables[0];
            gvPrescriptions.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }

    protected void lnkAddPrescriptions_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/EMR/Medication/PrescribeMedication.aspx?POPUP=POPUP&Regid=" + common.myInt(Session["RegistrationID"])
                                + "&RegNo=" + common.myLong(Session["RegistrationNo"])
                                + "&EncId=" + common.myInt(Session["EncounterId"])
                                + "&EncNo=" + common.myInt(Session["EncounterNo"]);
            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 930;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addPrescriptionsOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void btnAddPrescriptionsClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindPrescriptions();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    #endregion

}
