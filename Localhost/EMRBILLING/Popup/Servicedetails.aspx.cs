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
                    BindPatientHiddenDetails(common.myInt(Request.QueryString["RegNo"]));
                }
                if (!common.myInt(Request.QueryString["AuthorizedPer"]).Equals(0))
                {
                    hdnAuthorizedDiscPercent.Value = common.myStr(Request.QueryString["AuthorizedPer"]);
                    hdnAuthorizedRemark.Value = common.myStr(Request.QueryString["Remarks"]);
                }
                if (!common.myStr(Request.QueryString["DeptName"]).Equals(""))
                {
                    Page.Title = "Service Details ( " + common.myStr(Request.QueryString["DeptName"]) + " )";
                }
                if (!common.myStr(Request.QueryString["Decimal"]).Equals(""))
                {
                    hdnDecimalPlaces.Value = common.myStr(Request.QueryString["Decimal"]);
                }

                clsEMRBilling objclsEMRBilling = new clsEMRBilling(sConString);
                ViewState["CancelUnPerformServiceRightsWise"] = common.myStr(objclsEMRBilling.getHospitalSetupValue("CancelUnPerformServiceRightsWise", common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"])));
                objclsEMRBilling = null;

                BaseC.Security objSecurity = new BaseC.Security(sConString);
                ViewState["IsUserCancelUnbilledOrder"] = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsUserCancelUnbilledOrder").ToString();
                objSecurity = null;

                if ((!common.myStr(Request.QueryString["DeptId"]).Equals("")) || (!common.myStr(Request.QueryString["DeptType"]).Equals("")))
                {
                    hdnDepartmentId.Value = common.myStr(Request.QueryString["DeptId"]);
                    GetServiceIDFromRegnEncID(common.myInt(hdnRegId.Value), common.myInt(Request.QueryString["EncId"]), common.myInt(hdnDepartmentId.Value));
                }

                BindDepartment();

                //IsAllowDelete();

                SetPermission();
                CheckUserRights();
                FillServiceCancelReason();
                ViewState["IsAccessibleNursesRequestForms"] = common.myBool(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                    common.myInt(Session["FacilityId"]), "IsAccessibleNursesRequestForms", sConString));

                if (common.myLen(Request.QueryString["Department"]) > 0)
                {
                    ddlDepartment.SelectedIndex = ddlDepartment.Items.IndexOf(ddlDepartment.Items.FindItemByText(common.myStr(Request.QueryString["Department"]).ToUpper()));
                    ddlDepartment_OnSelectedIndexChanged(null, null);
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
            ua1.Dispose();
        }
    }
    private void CheckUserRights()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        boolCheckUserRights = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForBillDiscount");

    }

    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations(sConString);
        try
        {
            ViewState["IsAllowCancel"] = ua1.CheckPermissionsForEMRModule("C", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        { ua1.Dispose(); }
    }

    private void GetServiceIDFromRegnEncID(Int32 iRegID, Int32 iEncID, Int32 iDeptId)
    {
        DataView view = new DataView();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        clsEMR objBill = new clsEMR(sConString);
        BaseC.RestFulAPI objwcfBill = new BaseC.RestFulAPI(sConString);
        try
        {
            gvService.DataSource = null;
            gvService.DataBind();
            if (common.myStr(Request.QueryString["PType"]).Equals("WD"))
            {
                ds = objBill.GetEMRServiceOrderDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), iRegID, iEncID, common.myInt(Request.QueryString["BillId"]), iDeptId, common.myStr(Request.QueryString["DeptType"]));
            }
            else
            {
                ds = objwcfBill.GetIPServiceOrderDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), iRegID, iEncID, common.myInt(Request.QueryString["BillId"]), iDeptId, common.myStr(Request.QueryString["DeptType"]));
            }
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    hdnDetailType.Value = common.myStr(ds.Tables[1].Rows[0]["DetailType"]);
                }
                view = new DataView(ds.Tables[0]);
                ViewState["dtServices"] = ds.Tables[0];

                if (common.myStr(Request.QueryString["PType"]).Equals("WD"))
                {
                    //view.RowFilter = "ServiceType NOT IN('R','VF','CL','RF','IPP')";
                    //view.RowFilter = "ServiceType NOT IN ('R','VF','CL','RF','IPP','O')";

                    view.RowFilter = "IsAllowDepartmentType=1";
                }
                if (view != null)
                {
                    ViewState["dtServices"] = view.ToTable();
                    gvService.DataSource = view.ToTable();
                    gvService.DataBind();
                }
                if (common.myInt(Request.QueryString["BillId"]) > 0)
                {
                    btnSave.Visible = false;
                }

                view.Sort = "Service";
                if (view.ToTable().Rows.Count > 0)
                {
                    dt = view.ToTable(true, "Service", "ServiceId");
                }

                if (dt.Rows.Count > 0)
                {
                    RadComboBox ddlS = (RadComboBox)gvService.HeaderRow.FindControl("ddlService");
                    ViewState["dtDdlServices"] = dt;

                    ddlS.DataSource = dt;
                    ddlS.DataTextField = "Service";
                    ddlS.DataValueField = "ServiceId";
                    ddlS.DataBind();
                    ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
                }


                dt = view.ToTable(true, "DoctorName", "DoctorId");
                if (dt.Rows.Count > 0)
                {
                    RadComboBox ddlP = (RadComboBox)gvService.HeaderRow.FindControl("ddlProvider");
                    ViewState["dtDdlProvider"] = dt;
                    ddlP.DataSource = dt;
                    ddlP.DataTextField = "DoctorName";
                    ddlP.DataValueField = "DoctorId";
                    ddlP.DataBind();
                    ddlP.Items.Insert(0, new RadComboBoxItem(" --- All Provider --- ", "0"));
                }
            }
            else
            {
                gvService.DataSource = null;
                gvService.DataBind();
            }

            IsAllowDelete();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            view.Dispose();
            ds.Dispose();
            dt.Dispose();
            objBill = null;
            objwcfBill = null;
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
            hdnTotServiceAmount.Value = "0.00";
            hdnTotDoctorAmount.Value = "0.00";
            hdnTotGrossAmt.Value = "0.00";
            hdnTotDiscountAmount.Value = "0.00";
            hdnAmountPayableByPatient.Value = "0.00";
            hdnAmountPayableByPayer.Value = "0.00";

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
                dt = new DataTable();
                dt = (DataTable)ViewState["dtDdlServices"];
                if (dt.Rows.Count > 0)
                {
                    ddlS.DataSource = dt;
                    ddlS.DataTextField = "Service";
                    ddlS.DataValueField = "ServiceId";
                    ddlS.DataBind();
                    ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
                    ddlS.SelectedValue = sValue;
                }
            }

            RadComboBox ddlP = (RadComboBox)gvService.HeaderRow.FindControl("ddlProvider");
            if (ddlP != null)
            {
                dt = new DataTable();
                dt = dv.ToTable(true, "DoctorName", "DoctorId");
                if (dt.Rows.Count > 0)
                {
                    ddlP.DataSource = dt;
                    ddlP.DataTextField = "DoctorName";
                    ddlP.DataValueField = "DoctorId";
                    ddlP.DataBind();
                    ddlP.Items.Insert(0, new RadComboBoxItem(" --- All Provider --- ", "0"));
                }
            }

            IsAllowDelete();
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

        IsAllowDelete();
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

        IsAllowDelete();
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
    protected void ddlProvider_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            if (ViewState["dtServices"] == null)
                return;

            lblMessage.Text = "";
            hdnTotServiceAmount.Value = "0.00";
            hdnTotDoctorAmount.Value = "0.00";
            hdnTotGrossAmt.Value = "0.00";
            hdnTotDiscountAmount.Value = "0.00";
            hdnAmountPayableByPatient.Value = "0.00";
            hdnAmountPayableByPayer.Value = "0.00";

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
                        dv.RowFilter = "DoctorId = " + common.myStr(e.Value);
                    }
                    else
                    {
                        dv.RowFilter = "ServiceId = " + common.myStr(ddlS.SelectedValue) + " and DoctorId = " + common.myStr(e.Value);
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
                dt = new DataTable();
                dt = (DataTable)ViewState["dtDdlServices"];
                if (dt.Rows.Count > 0)
                {
                    ddlS.DataSource = dt;
                    ddlS.DataTextField = "Service";
                    ddlS.DataValueField = "ServiceId";
                    ddlS.DataBind();
                    ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
                    ddlS.SelectedValue = SelectedService;
                }
            }

            RadComboBox ddlP = (RadComboBox)gvService.HeaderRow.FindControl("ddlProvider");
            if (ddlP != null)
            {
                dt = new DataTable();
                dt = (DataTable)ViewState["dtDdlProvider"];
                if (dt.Rows.Count > 0)
                {
                    ddlP.DataSource = dt;
                    ddlP.DataTextField = "DoctorName";
                    ddlP.DataValueField = "DoctorId";
                    ddlP.DataBind();
                    ddlP.Items.Insert(0, new RadComboBoxItem(" --- All Provider --- ", "0"));
                    ddlP.SelectedValue = sValue;
                }
            }

            IsAllowDelete();
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
    private void GetDepartmenttype(Int32 iRegID, Int32 iEncID, Int32 iBillNo)
    {
        EMRBilling.clsOrderNBill baseBill = new EMRBilling.clsOrderNBill(sConString);
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        try
        {
            ds = baseBill.getServiceOrderDeptTypeWise(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(iEncID), "", "", 0, 1, "");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dv = new DataView(ds.Tables[0]);
                if (!common.myStr(Request.QueryString["PType"]).Equals("WD"))
                {
                    dv.RowFilter = "DepartmentID =" + common.myStr(hdnDepartmentId.Value) + "";
                }
                if (dv.ToTable().Rows.Count > 0)
                {
                    gvService.DataSource = dv.ToTable();
                    gvService.DataBind();
                }
            }

            IsAllowDelete();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            baseBill = null;
            ds.Dispose();
            dv.Dispose();
        }
    }
    public DataSet DoctorBind()
    {
        DataSet ds = new DataSet();
        Hospital baseHosp = new Hospital(sConString);
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            ds = baseHosp.fillDoctorCombo(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), 0, Convert.ToInt16(common.myInt(Session["FacilityId"])));
            return ds;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
            return ds;
        }
        finally
        {
            ds.Dispose();
            baseHosp = null;
            dl = null;
        }
    }
    protected void gvService_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.Header) || e.Row.RowType.Equals(DataControlRowType.Footer) || e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                if (common.myStr(Request.QueryString["PType"]).Equals("WD"))
                {
                    //e.Row.Cells[5].Visible = false;
                    //e.Row.Cells[6].Visible = false;
                    e.Row.Cells[6].Enabled = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[11].Visible = false;
                }
                else if (common.myStr(hdnDetailType.Value).Equals("PH"))
                {
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[7].Width = Unit.Pixel(0);
                }
                else
                {
                    e.Row.Cells[8].Visible = false;
                }
            }

            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                Label lblSno = (Label)e.Row.FindControl("lblSno");
                Label lblOrderNo = (Label)e.Row.FindControl("lblOrderNo");
                Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
                LinkButton lblServiceName = (LinkButton)e.Row.FindControl("lblServiceName");
                Label lblUnit = (Label)e.Row.FindControl("lblUnit");
                TextBox txtServiceAmount = (TextBox)e.Row.FindControl("txtServiceAmount");
                TextBox txtDoctorAmount = (TextBox)e.Row.FindControl("txtDoctorAmount");
                TextBox txtGrossAmt = (TextBox)e.Row.FindControl("txtGrossAmt");

                TextBox txtPercentDiscount = (TextBox)e.Row.FindControl("txtPercentDiscount");
                TextBox txtDiscountAmt = (TextBox)e.Row.FindControl("txtDiscountAmt");

                Label lblAmountPayableByPayer = (Label)e.Row.FindControl("lblAmountPayableByPayer");
                Label lblAmountPayableByPatient = (Label)e.Row.FindControl("lblAmountPayableByPatient");
                Label lblPayableAmt = (Label)e.Row.FindControl("lblPayableAmt");
                HiddenField hdnDetailId = (HiddenField)e.Row.FindControl("hdnDetailId");
                HiddenField hdnServiceId = (HiddenField)e.Row.FindControl("hdnServiceId");
                HiddenField hdnUPack = (HiddenField)e.Row.FindControl("hdnUnderPack");
                HiddenField hdnPackage = (HiddenField)e.Row.FindControl("hdnPackageId");
                HiddenField hdnServType = (HiddenField)e.Row.FindControl("hdnServiceType");
                HiddenField hdnDeptId = (HiddenField)e.Row.FindControl("hdnDeptId");
                HiddenField hdnDocReq = (HiddenField)e.Row.FindControl("hdnDocReq");
                HiddenField hdnDoctorID = (HiddenField)e.Row.FindControl("hdnDoctorID");
                HiddenField hdnDeptName = (HiddenField)e.Row.FindControl("hdnDeptName");

                HiddenField hdnIsPackageMain = (HiddenField)e.Row.FindControl("hdnIsPackageMain");
                HiddenField hdnIsPackageService = (HiddenField)e.Row.FindControl("hdnIsPackageService");
                HiddenField hdnActive = (HiddenField)e.Row.FindControl("hdnActive");
                ImageButton ibtndaDelete = (ImageButton)e.Row.FindControl("ibtndaDelete");
                HiddenField hdnPriceEditable = (HiddenField)e.Row.FindControl("hdnPriceEditable");
                HiddenField hdnIsDiscountable = (HiddenField)e.Row.FindControl("hdnIsDiscountable");

                HiddenField hdnEncodedBy = (HiddenField)e.Row.FindControl("hdnEncodedBy");
                HiddenField hdnEncodedDate = (HiddenField)e.Row.FindControl("hdnEncodedDate");
                HiddenField hdnLastChangedBy = (HiddenField)e.Row.FindControl("hdnLastChangedBy");
                HiddenField hdnLastChangedDate = (HiddenField)e.Row.FindControl("hdnLastChangedDate");
                HiddenField hdnStatusCode = (HiddenField)e.Row.FindControl("hdnStatusCode");
                HiddenField hdnBillCategory = (HiddenField)e.Row.FindControl("hdnBillCategory");
                HiddenField hdnClinicalDetailFound = (HiddenField)e.Row.FindControl("hdnClinicalDetailFound");
                HiddenField hdnManualEntryNo = (HiddenField)e.Row.FindControl("hdnManualEntryNo");
                HiddenField hdLabSampleNotes = (HiddenField)e.Row.FindControl("hdLabSampleNotes");
                ImageButton ibtnForNotes = (ImageButton)e.Row.FindControl("ibtnForNotes");
                ibtndaDelete.Visible = true;

                if (common.myInt(hdLabSampleNotes.Value) > 0)
                {
                    ibtnForNotes.Visible = true;
                }
                int IsDiscountable = common.myInt(hdnIsDiscountable.Value);

                if (common.myStr(hdnStatusCode.Value).Equals("SNC"))
                {
                    for (int index = 0; index < e.Row.Cells.Count; index++)
                    {
                        e.Row.Cells[index].BackColor = System.Drawing.Color.Bisque;
                    }
                    //e.Row.BackColor = System.Drawing.Color.Bisque;
                }
                else if (common.myStr(hdnStatusCode.Value).Equals("SC"))
                {
                    for (int index = 0; index < e.Row.Cells.Count; index++)
                    {
                        e.Row.Cells[index].BackColor = System.Drawing.Color.Bisque;
                    }
                    //e.Row.BackColor = System.Drawing.Color.Bisque; 
                }
                else if (common.myStr(hdnStatusCode.Value).Equals("SD"))
                {
                    for (int index = 0; index < e.Row.Cells.Count; index++)
                    {
                        e.Row.Cells[index].BackColor = System.Drawing.Color.Bisque;
                    }
                    //e.Row.BackColor = System.Drawing.Color.Bisque; 
                }

                if (common.myInt(hdnClinicalDetailFound.Value).Equals(1))
                { lblServiceName.ForeColor = System.Drawing.Color.Blue; }
                else
                { lblServiceName.ForeColor = System.Drawing.Color.Black; }

                if (common.myStr(hdnActive.Value).Equals("False") || common.myStr(hdnActive.Value).Equals("0"))
                {
                    lblServiceName.ForeColor = System.Drawing.Color.Gray;
                    lblUnit.ForeColor = System.Drawing.Color.Gray;
                    lblSno.ForeColor = System.Drawing.Color.Gray;
                    lblOrderDate.ForeColor = System.Drawing.Color.Gray;
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Gray;
                    e.Row.Cells[10].ForeColor = System.Drawing.Color.Gray;
                    ibtndaDelete.Visible = false;
                }
                string serviceType = common.myStr(hdnServType.Value);
                if ((common.myInt(hdnDocReq.Value).Equals(1) || common.myStr(hdnDocReq.Value).Equals("True")) && common.myInt(hdnDoctorID.Value) > 0)
                {
                    if (serviceType.Equals("P") || serviceType.Equals("I") || serviceType.Equals("IS") || serviceType.Equals("C") || serviceType.Equals("O"))
                    { txtDoctorAmount.Enabled = true; }
                }
                if (common.myInt(hdnIsPackageMain.Value).Equals(1))
                {
                    lblServiceName.Font.Bold = true;
                }
                if (common.myStr(hdnServType.Value).Equals("PH"))
                {
                    ibtndaDelete.Visible = false;
                }

                if (common.myStr(hdnStatusCode.Value).Equals("DA")
                    || common.myStr(hdnStatusCode.Value).Equals("RE")
                    || common.myStr(hdnStatusCode.Value).Equals("RP")
                    || common.myStr(hdnStatusCode.Value).Equals("RF"))
                {
                    ibtndaDelete.Visible = false;
                }

                if (common.myDec(hdnAuthorizedDiscPercent.Value) > 0 && (common.myStr(hdnActive.Value).Equals("True") || common.myStr(hdnActive.Value).Equals("1")) && common.myInt(IsDiscountable).Equals(1) && boolCheckUserRights.Equals("True"))
                {
                    txtPercentDiscount.Enabled = true;
                    txtDiscountAmt.Enabled = true;
                }
                else
                {
                    txtPercentDiscount.Enabled = false;
                    txtDiscountAmt.Enabled = false;
                }
                if (common.myStr(hdnActive.Value).Equals("False") || common.myStr(hdnActive.Value).Equals("0") || common.myStr(hdnServType.Value).Equals("PH") || common.myInt(hdnPriceEditable.Value).Equals(0))
                {
                    txtServiceAmount.Enabled = false;
                    txtDoctorAmount.Enabled = false;
                }
                if (!common.myInt(hdnIsPackageService.Value).Equals(1) && (common.myStr(hdnActive.Value).Equals("True") || common.myStr(hdnActive.Value).Equals("1")))
                {
                    hdnTotUnit.Value = common.myStr(common.myDec(hdnTotUnit.Value) + common.myDec(lblUnit.Text));
                    hdnTotServiceAmount.Value = common.myStr(common.myDec(hdnTotServiceAmount.Value) + common.myDec(txtServiceAmount.Text));
                    hdnTotDoctorAmount.Value = common.myStr(common.myDec(hdnTotDoctorAmount.Value) + common.myDec(txtDoctorAmount.Text));
                    hdnTotGrossAmt.Value = common.myStr(common.myDec(hdnTotGrossAmt.Value) + common.myDec(txtGrossAmt.Text));
                    hdnTotDiscountAmount.Value = common.myStr(common.myDec(hdnTotDiscountAmount.Value) + common.myDec(txtDiscountAmt.Text));
                    hdnAmountPayableByPatient.Value = common.myStr(common.myDec(hdnAmountPayableByPatient.Value) + common.myDec(lblAmountPayableByPatient.Text));
                    hdnAmountPayableByPayer.Value = common.myStr(common.myDec(hdnAmountPayableByPayer.Value) + common.myDec(lblAmountPayableByPayer.Text));
                }

                txtPercentDiscount.Attributes.Add("onChange", "javascript:CalculateAmount('" + txtServiceAmount.ClientID + "','" + txtDoctorAmount.ClientID + "','" + lblUnit.ClientID + "','" + txtPercentDiscount.ClientID + "','" + txtDiscountAmt.ClientID + "','" + lblPayableAmt.ClientID + "','" + hdnServiceId.ClientID + "','" + lblAmountPayableByPatient.ClientID + "','" + lblAmountPayableByPayer.ClientID + "');");
                txtDiscountAmt.Attributes.Add("onChange", "javascript:CalculatePercentage('" + txtServiceAmount.ClientID + "','" + txtDoctorAmount.ClientID + "','" + lblUnit.ClientID + "','" + txtPercentDiscount.ClientID + "','" + txtDiscountAmt.ClientID + "','" + lblPayableAmt.ClientID + "','" + hdnServiceId.ClientID + "','" + lblAmountPayableByPatient.ClientID + "','" + lblAmountPayableByPayer.ClientID + "');");

                txtPercentDiscount.Attributes.Add("ondblclick", "javascript:CopyPercentToAllAndCalculate('" + txtPercentDiscount.ClientID + "');");

                txtServiceAmount.Attributes.Add("onkeyup", "javascript:CalculateAmountWhenUpdateServiceAmount('" + txtServiceAmount.ClientID + "','" + txtDoctorAmount.ClientID + "','" + lblUnit.ClientID + "','" + txtPercentDiscount.ClientID + "','" + txtDiscountAmt.ClientID + "','" + lblPayableAmt.ClientID + "','" + hdnServiceId.ClientID + "','" + lblAmountPayableByPatient.ClientID + "','" + lblAmountPayableByPayer.ClientID + "');");
                txtDoctorAmount.Attributes.Add("onkeyup", "javascript:CalculateAmountWhenUpdateServiceAmount('" + txtServiceAmount.ClientID + "','" + txtDoctorAmount.ClientID + "','" + lblUnit.ClientID + "','" + txtPercentDiscount.ClientID + "','" + txtDiscountAmt.ClientID + "','" + lblPayableAmt.ClientID + "','" + hdnServiceId.ClientID + "','" + lblAmountPayableByPatient.ClientID + "','" + lblAmountPayableByPayer.ClientID + "');");
                e.Row.Attributes.Add("onclick", "javascript:ShowEncodDetails('" + lblServiceName.ClientID + "','" + hdnEncodedBy.ClientID +
                            "','" + hdnEncodedDate.ClientID + "','" + hdnLastChangedBy.ClientID +
                            "','" + hdnLastChangedDate.ClientID + "','" + lblSno.ClientID +
                            "','" + lblOrderNo.ClientID + "','" + lblOrderDate.ClientID + "','" + hdnBillCategory.ClientID + "','" + hdnManualEntryNo.ClientID + "');");
            }
            else if (e.Row.RowType.Equals(DataControlRowType.Footer))
            {
                Label lblTotServiceAmount = (Label)e.Row.FindControl("lblTotServiceAmount");
                Label lblTotDoctorAmount = (Label)e.Row.FindControl("lblTotDoctorAmount");
                Label lblTotDiscountAmount = (Label)e.Row.FindControl("lblTotDiscountAmount");
                Label lblAmountPayableByPatientFooter = (Label)e.Row.FindControl("lblAmountPayableByPatientFooter");
                Label lblAmountPayableByPayerFooter = (Label)e.Row.FindControl("lblAmountPayableByPayerFooter");
                Label lblAmountPayableByPayer = (Label)e.Row.FindControl("lblAmountPayableByPayer");
                Label lblTotGrossAmt = (Label)e.Row.FindControl("lblTotGrossAmt");

                lblTotServiceAmount.Text = common.myDec(hdnTotServiceAmount.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                lblTotDoctorAmount.Text = common.myDec(hdnTotDoctorAmount.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                lblTotGrossAmt.Text = common.myDec(hdnTotGrossAmt.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                lblTotDiscountAmount.Text = common.myDec(hdnTotDiscountAmount.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                lblAmountPayableByPatientFooter.Text = common.myDec(hdnAmountPayableByPatient.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                lblAmountPayableByPayerFooter.Text = common.myDec(hdnAmountPayableByPayer.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    void BindPatientHiddenDetails(int RegistrationNo)
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
                    ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, 0, common.myInt(Session["UserId"]));
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        hdnRegId.Value = common.myStr(dr["RegistrationId"]);
                        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                        lblDob.Text = common.myStr(dr["DOB"]);
                        lblMobile.Text = common.myStr(dr["MobileNo"]);
                        Label3.Text = "OP No. : ";
                        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                        lblAdmissionDate.Visible = false;
                        hdnBilltype.Value = "";
                    }
                }
                else  //I
                {
                    ds = objIPBill.GetPatientDetailsIP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, common.myInt(Session["UserId"]), 0, "");

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
                            hdnBilltype.Value = common.myStr(dr["EncounterCompanyType"]);
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
    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.EMROrders objOrders = new BaseC.EMROrders(sConString);
        try
        {
            if (e.CommandName.Equals("sel"))
            {
                string Flag = "LIS";

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                ImageButton hdnDetailId = (ImageButton)row.FindControl("ibtnForNotes");


                lblMessage.Text = "";

                //if (common.myInt(((Label)lnk.FindControl("lblLabNo")).Text) == 0)
                //{
                //    if (Flag.ToString() == "RIS")
                //        lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                //    else
                //        lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                //    return;
                //}

                string Source = "IPD";
                //common.myStr(ViewState["Source"]);
                //if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
                //{
                //    Source = "OPD";
                //}

                RadWindow3.NavigateUrl = "~/LIS/Format/LISNotes.aspx?MD=" + Flag +
                                        "&SOURCE=" + common.myStr(Source) +
                                        "&eno=" + common.myStr(lblEncounterNo.Text) +
                                        "&RegNo=" + common.myStr(Request.QueryString["RegNo"]) +
                                        "&pn=" + common.myStr(lblPatientName.Text) +
                                        "&LABNO=" +
                                        "&Servicedetails=" + 1 +
                                        "&OrderId=" + hdnDetailId.CommandArgument;

                RadWindow3.Height = 580;
                RadWindow3.Width = 900;
                RadWindow3.Top = 10;
                RadWindow3.Left = 10;
                RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow3.Modal = true;
                RadWindow3.VisibleStatusbar = false;

            }
            if (e.CommandName.Equals("Del"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                if (common.myInt(Session["ModuleId"]).Equals(3) || common.myInt(Session["ModuleId"]).Equals(30))
                {
                    if (!common.myBool(ViewState["IsAllowCancel"]))
                    {
                        Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);

                        IsAllowDelete();
                        return;
                    }
                }

                if (common.myInt(ddlServiceCancelReason.SelectedValue).Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please Select cancellation reason !";
                    ddlServiceCancelReason.Focus();

                    IsAllowDelete();
                    return;
                }

                HiddenField hdnDetailId = (HiddenField)row.FindControl("hdnDetailId");
                HiddenField hdnServiceId = (HiddenField)row.FindControl("hdnServiceId");
                HiddenField hdnOrderId = (HiddenField)row.FindControl("hdnOrderId");
                HiddenField hdnSODAID = (HiddenField)row.FindControl("hdnSODAID");

                if ((common.myInt(hdnServiceId.Value) > 0) && (common.myInt(hdnOrderId.Value) > 0))
                {
                    string strmsg = objOrders.DeActivateOrderServices(common.myInt(hdnSODAID.Value), common.myInt(hdnServiceId.Value),
                        common.myInt(hdnOrderId.Value), common.myInt(hdnDetailId.Value),
                        common.myStr(txtCancelationRemarks.Text, true).Trim(),
                        common.myInt(Session["HospitalLocationId"]),
                        common.myInt(Session["FacilityId"]),
                        common.myInt(Session["UserId"]), ddlServiceCancelReason.Text);
                    if (strmsg.Contains("Succeeded"))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblMessage.Text = strmsg;
                        hdnTotServiceAmount.Value = "0.00";
                        hdnTotDoctorAmount.Value = "0.00";
                        hdnTotGrossAmt.Value = "0.00";
                        hdnTotDiscountAmount.Value = "0.00";
                        hdnAmountPayableByPatient.Value = "0.00";
                        hdnAmountPayableByPayer.Value = "0.00";
                        GetServiceIDFromRegnEncID(common.myInt(hdnRegId.Value), common.myInt(Request.QueryString["EncId"]), common.myInt(hdnDepartmentId.Value));
                        //Change palendra
                        ddlServiceCancelReason.SelectedIndex = 0;
                        txtCancelationRemarks.Text = "";
                        //Change palendra

                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = strmsg;

                        IsAllowDelete();
                        return;
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Service order not saved !";
                }

                IsAllowDelete();
            }
            if (e.CommandName == "PrintOrder")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnOrderId = (HiddenField)row.FindControl("hdnOrderId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                RadWindow3.NavigateUrl = "~/EMR/Orders/PrintOrder.aspx?rptName=PODSave&encounterId=" + common.myStr(hdnEncounterId.Value) + "&RegistrationId=" + common.myStr(hdnRegId.Value) + "&OrderId=" + common.myStr(hdnOrderId.Value);
                RadWindow3.Height = 600;
                RadWindow3.Width = 900;
                RadWindow3.Top = 10;
                RadWindow3.Left = 20;
                RadWindow3.OnClientClose = string.Empty;//"OnClientClose";
                RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow3.Modal = true;
                RadWindow3.InitialBehaviors = WindowBehaviors.Maximize;
                RadWindow3.VisibleStatusbar = false;
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objOrders = null; }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hashtable hshInput = new Hashtable();
        Hashtable hshOutput = new Hashtable();
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        EMRBilling objBilling = new EMRBilling(sConString);
        try
        {
            foreach (GridViewRow item in gvService.Rows)
            {
                HiddenField hdnDetailId = (HiddenField)item.FindControl("hdnDetailId");
                HiddenField hdnOrderId = (HiddenField)item.FindControl("hdnOrderId");
                HiddenField hdnServiceId = (HiddenField)item.FindControl("hdnServiceId");
                HiddenField hdnServiceDiscountPercentage = (HiddenField)item.FindControl("hdnServiceDiscountPercentage");
                TextBox txtServiceAmount = (TextBox)item.FindControl("txtServiceAmount");
                TextBox txtDoctorAmount = (TextBox)item.FindControl("txtDoctorAmount");
                TextBox txtPercentDiscount = (TextBox)item.FindControl("txtPercentDiscount");
                Label lblPayableAmt = (Label)item.FindControl("lblPayableAmt");
                Label lblUnit = (Label)item.FindControl("lblUnit");
                int AuthorisedBy = common.myInt(Request.QueryString["AuthorizedBy"]);
                string Remarks = common.myStr(Request.QueryString["Remarks"]);
                HiddenField hdnBatchId = (HiddenField)item.FindControl("hdnBatchId");
                TextBox txtDiscountAmt = (TextBox)item.FindControl("txtDiscountAmt");

                coll.Add(common.myInt(hdnDetailId.Value)); //ServiceOrderDetailId INT,                                                          
                coll.Add(common.myInt(hdnOrderId.Value)); //OrderId INT,                                                                      
                coll.Add(common.myInt(hdnServiceId.Value)); //ServiceId INT,    
                coll.Add(common.myInt(hdnBatchId.Value)); //BatchId INT,    
                coll.Add(common.myDec(txtServiceAmount.Text)); //ServiceAmount MONEY,                        
                coll.Add(common.myDec(txtDoctorAmount.Text)); //DoctorAmount MONEY,                                      
                if ((common.myDec(txtPercentDiscount.Text) != common.myDec(hdnServiceDiscountPercentage.Value)) && common.myStr(hdnAuthorizedRemark.Value) == "")
                {
                    Alert.ShowAjaxMsg("Please provide discount authorized Remark...!", Page.Page);
                    return;
                }
                else
                {
                    coll.Add(common.myDec(txtPercentDiscount.Text)); //ServiceDiscountPercentage MONEY, 
                    coll.Add(common.myDec(txtPercentDiscount.Text)); //DoctorDiscountPercentage MONEY, 
                }
                coll.Add(common.myDec(lblPayableAmt.Text)); //AmountPayable INT,
                coll.Add(common.myInt(AuthorisedBy)); //DiscountAuthorizedById INT,
                coll.Add(common.myStr(Remarks)); //Remarks varchar(200)
                coll.Add(common.myStr(common.myInt(lblUnit.Text))); //unit INT
                coll.Add(common.myDec(txtDiscountAmt.Text)); //Discount Amount MONEY, 

                strXML.Append(common.setXmlTable(ref coll));
            }
            if (strXML.ToString().Trim().Length > 1)
            {
                lblMessage.Text = common.myStr(objBilling.InsertIPServiceDiscount(common.myInt(Request.QueryString["EncId"]), common.myStr(hdnDetailType.Value), common.myInt(Session["UserId"]), strXML.ToString()));
                if (lblMessage.Text.ToUpper().Contains("SUCCESSFULLY"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    hdnTotServiceAmount.Value = "0.00";
                    hdnTotDoctorAmount.Value = "0.00";
                    hdnTotGrossAmt.Value = "0.00";
                    hdnTotDiscountAmount.Value = "0.00";
                    hdnAmountPayableByPatient.Value = "0.00";
                    hdnAmountPayableByPayer.Value = "0.00";
                    GetServiceIDFromRegnEncID(common.myInt(hdnRegId.Value), common.myInt(Request.QueryString["EncId"]), common.myInt(hdnDepartmentId.Value));
                }
                else
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { hshInput = null; hshOutput = null; strXML = null; coll = null; objBilling = null; }
    }
    protected void lblServiceName_OnClick(object sender, EventArgs e)
    {
        try
        {

            LinkButton lnkName = (LinkButton)sender;
            string sServiceId = ((HiddenField)lnkName.FindControl("hdnServiceId")).Value.ToString().Trim();

            HiddenField hdnServiceDetailId = (HiddenField)lnkName.FindControl("hdnDetailId");
            HiddenField hdnEncounterId = (HiddenField)lnkName.FindControl("hdnEncounterId");
            HiddenField hdnClinicalDetailFound = (HiddenField)lnkName.FindControl("hdnClinicalDetailFound");

            HiddenField hdnTemplateTaggingService = (HiddenField)lnkName.FindControl("hdnTemplateTaggingService");
            HiddenField hdnIsMandatory = (HiddenField)lnkName.FindControl("hdnIsMandatory");
            HiddenField hdnDetailId = (HiddenField)lnkName.FindControl("hdnDetailId");
            HiddenField hdnSubDeptId = (HiddenField)lnkName.FindControl("hdnSubDeptId");
            if ((ViewState["IsAccessibleNursesRequestForms"] != null && common.myBool(ViewState["IsAccessibleNursesRequestForms"])) && common.myBool(hdnTemplateTaggingService.Value))
            {
                RadWindow3.NavigateUrl = "~/EMR/Orders/TemplateRequestForm.aspx?MASTER=No&CF=LAB&ServId=" + sServiceId + "&ServName=" + lnkName.Text + "&ServiceOrderId="
                             + common.myStr(hdnDetailId.Value) + "&EncounterId=" + common.myStr(hdnEncounterId.Value)//common.myStr(Session["EncounterId"]) 
                             + "&TagType=L&TemplateRequiredServices=" + sServiceId + "&SourceForLIS=LIS&sOrdDtlId=" + common.myStr(hdnDetailId.Value) + "&ManualLabNo="
                             + common.myStr(ViewState["ManualLabNo"]) + "&Finalized=" + common.myStr(ViewState["Finalized"]);
                RadWindow3.Height = 625;
                RadWindow3.Width = 1060;
                RadWindow3.Top = 10;
                RadWindow3.Left = 10;
                RadWindow3.Modal = true;
                RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindow3.VisibleStatusbar = false;
                RadWindow3.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else
            {

                lblSno.Text = ((Label)lnkName.FindControl("lblSno")).Text;
                lblOrderNo.Text = ((Label)lnkName.FindControl("lblOrderNo")).Text;
                lblOrderDate.Text = ((Label)lnkName.FindControl("lblOrderDate")).Text;
                lblServiceName.Text = ((LinkButton)lnkName.FindControl("lblServiceName")).Text;
                lblEncodedBy.Text = ((HiddenField)lnkName.FindControl("hdnEncodedBy")).Value;
                lblEncodedDate.Text = ((HiddenField)lnkName.FindControl("hdnEncodedDate")).Value;
                lblLastChangedBy.Text = ((HiddenField)lnkName.FindControl("hdnLastChangedBy")).Value;
                lblLastChangedDate.Text = ((HiddenField)lnkName.FindControl("hdnLastChangedDate")).Value;
                lblBillCatgory.Text = ((HiddenField)lnkName.FindControl("hdnBillCategory")).Value;
                lblManualEntry.Text = ((HiddenField)lnkName.FindControl("hdnManualEntryNo")).Value;


                if (sServiceId != null && hdnServiceDetailId != null && hdnEncounterId != null && hdnClinicalDetailFound != null)
                {
                    if (common.myInt(sServiceId) > 0
                    && common.myInt(hdnClinicalDetailFound.Value).Equals(1))
                    {
                        Session["EncounterId"] = hdnEncounterId.Value;
                        Session["RegistrationId"] = hdnRegId.Value;
                        RadWindow3.NavigateUrl = "~/EMR/Orders/ViewDetails.aspx?MASTER=No&ServId=" + common.myStr(sServiceId) + "&ServName=" + lnkName.Text
                            + "&From=POPUP&RegNo=" + common.myStr(Request.QueryString["RegNo"]) + "&EncounterId=" + common.myStr(hdnEncounterId.Value) + "&sOrdDtlId="
                            + common.myStr(hdnServiceDetailId.Value);// +"&TagType=D";
                        RadWindow3.Height = 600;
                        RadWindow3.Width = 900;
                        RadWindow3.Top = 20;
                        RadWindow3.Left = 20;
                        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindow3.Modal = true;
                        RadWindow3.VisibleStatusbar = false;
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
    }

    protected void BindDepartment()
    {
        BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            ddlDepartment.Items.Clear();
            ds = bMstr.GetDepartmentAccordingEncounter(common.myInt(Session["HospitalLocationID"].ToString()), common.myInt(Session["FacilityId"]), common.myInt(hdnRegId.Value), common.myInt(Request.QueryString["EncId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlDepartment.DataSource = ds.Tables[0];
                    ddlDepartment.DataTextField = "DepartmentName";
                    ddlDepartment.DataValueField = "DepartmentID";
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
                    ddlDepartment.SelectedIndex = 0;
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
            bMstr = null;
            ds.Dispose();
        }
    }
    protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!common.myStr(ddlDepartment.SelectedValue).Equals("-1"))
            {
                lblMessage.Text = "";
                hdnTotServiceAmount.Value = "0.00";
                hdnTotDoctorAmount.Value = "0.00";
                hdnTotGrossAmt.Value = "0.00";
                hdnTotDiscountAmount.Value = "0.00";
                hdnAmountPayableByPatient.Value = "0.00";
                hdnAmountPayableByPayer.Value = "0.00";
                //palendra
                if (common.myInt(ddlDepartment.SelectedValue) > 0)
                {
                    hdnDepartmentId.Value = ddlDepartment.SelectedValue;
                }

                GetServiceIDFromRegnEncID(common.myInt(hdnRegId.Value), common.myInt(Request.QueryString["EncId"]), common.myInt(hdnDepartmentId.Value));
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void gvService_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["dtServices"] != null)
        {
            int selectedValue = 0;
            try
            {
                RadComboBox ddlS = (RadComboBox)gvService.HeaderRow.FindControl("ddlService");

                selectedValue = common.myInt(ddlS.SelectedValue);
            }
            catch
            {
            }

            if (common.myInt(selectedValue) > 0)
            {
                ddlServiceSelectedIndexChanged(common.myInt(selectedValue), e.NewPageIndex);
            }
            else
            {
                gvService.DataSource = ViewState["dtServices"] as DataTable;

                gvService.PageIndex = e.NewPageIndex;
                gvService.DataBind();

                IsAllowDelete();
            }

        }
    }

    private void IsAllowDelete()
    {

        try
        {
            foreach (GridViewRow gvr in gvService.Rows)
            {
                ImageButton ibtndaDelete = (ImageButton)gvr.FindControl("ibtndaDelete");
                HiddenField hdnStatusId = (HiddenField)gvr.FindControl("hdnStatusId");
                HiddenField hdnStatusCode = (HiddenField)gvr.FindControl("hdnStatusCode");
                HiddenField hdnServType = (HiddenField)gvr.FindControl("hdnServiceType");
                HiddenField hdnActive = (HiddenField)gvr.FindControl("hdnActive");

                if (
                    (!common.myStr(hdnStatusCode.Value).Equals("SNC")
                    && !common.myStr(hdnStatusCode.Value).Equals("SC")
                    && !common.myStr(hdnStatusCode.Value).Equals("SD"))
                    &&
                    (!common.myStr(hdnStatusCode.Value).Equals(string.Empty)
                    && !common.myStr(hdnStatusCode.Value).Equals(string.Empty)
                    && !common.myStr(hdnStatusCode.Value).Equals(string.Empty))
                    )
                {
                    ibtndaDelete.Visible = false;

                    if (common.myBool(ViewState["IsUserCancelUnbilledOrder"])
                        && !common.myStr(hdnServType.Value).Equals("PH")
                        && common.myBool(hdnActive.Value))
                    {
                        ibtndaDelete.Visible = true;
                    }
                }
                else if (common.myStr(hdnStatusCode.Value).Equals("SNC")
                    || common.myStr(hdnStatusCode.Value).Equals("SC")
                    || common.myStr(hdnStatusCode.Value).Equals("SD"))
                {
                    if (common.myStr(ViewState["CancelUnPerformServiceRightsWise"]).Equals("Y"))
                    {
                        ibtndaDelete.Visible = false;

                        if (common.myBool(ViewState["IsUserCancelUnbilledOrder"])
                            && !common.myStr(hdnServType.Value).Equals("PH")
                            && common.myBool(hdnActive.Value))
                        {
                            ibtndaDelete.Visible = true;
                        }
                    }
                }

                if (common.myStr(hdnStatusCode.Value).Equals("DA")
                    || common.myStr(hdnStatusCode.Value).Equals("RE")
                    || common.myStr(hdnStatusCode.Value).Equals("RP")
                    || common.myStr(hdnStatusCode.Value).Equals("RF"))
                {
                    ibtndaDelete.Visible = false;
                }

            }
        }
        catch
        {
        }
    }

    protected void lbtnNotes_OnClick(object sender, EventArgs e)
    {
        string Flag = "LIS";
        try
        {
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void ddlServiceSelectedIndexChanged(int ServiceId, int PageIndex)
    {
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            if (ServiceId == 0)
            {
                return;
            }

            if (ViewState["dtServices"] == null)
                return;

            lblMessage.Text = "";
            hdnTotServiceAmount.Value = "0.00";
            hdnTotDoctorAmount.Value = "0.00";
            hdnTotGrossAmt.Value = "0.00";
            hdnTotDiscountAmount.Value = "0.00";
            hdnAmountPayableByPatient.Value = "0.00";
            hdnAmountPayableByPayer.Value = "0.00";

            dt = (DataTable)ViewState["dtServices"];
            string sValue = ServiceId.ToString();
            if (dt.Rows.Count > 0)
            {
                dv = dt.DefaultView;
                if (common.myStr(ServiceId).Equals("0"))
                {
                    gvService.DataSource = dt;
                    gvService.PageIndex = PageIndex;
                    gvService.DataBind();
                }
                else
                {
                    dv.RowFilter = "ServiceId=" + common.myInt(ServiceId).ToString();
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        gvService.DataSource = dv.ToTable();
                        gvService.PageIndex = PageIndex;
                        gvService.DataBind();
                    }
                }
            }
            RadComboBox ddlS = (RadComboBox)gvService.HeaderRow.FindControl("ddlService");
            if (ddlS != null)
            {
                dt = new DataTable();
                dt = (DataTable)ViewState["dtDdlServices"];
                if (dt.Rows.Count > 0)
                {
                    ddlS.DataSource = dt;
                    ddlS.DataTextField = "Service";
                    ddlS.DataValueField = "ServiceId";
                    ddlS.DataBind();
                    ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
                    ddlS.SelectedValue = sValue;
                }
            }

            RadComboBox ddlP = (RadComboBox)gvService.HeaderRow.FindControl("ddlProvider");
            if (ddlP != null)
            {
                dt = new DataTable();
                dt = dv.ToTable(true, "DoctorName", "DoctorId");
                if (dt.Rows.Count > 0)
                {
                    ddlP.DataSource = dt;
                    ddlP.DataTextField = "DoctorName";
                    ddlP.DataValueField = "DoctorId";
                    ddlP.DataBind();
                    ddlP.Items.Insert(0, new RadComboBoxItem(" --- All Provider --- ", "0"));
                }
            }

            IsAllowDelete();
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

    public void FillServiceCancelReason()
    {
        try
        {
            BaseC.EMRBilling objEmrBilling = new BaseC.EMRBilling(sConString);
            DataSet ds = new DataSet();
            ds = objEmrBilling.GetReasontype("ServiceOrderCancel");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlServiceCancelReason.DataSource = ds.Tables[0];
                ddlServiceCancelReason.DataTextField = "Reason";
                ddlServiceCancelReason.DataValueField = "Id";
                ddlServiceCancelReason.DataBind();
            }
            ddlServiceCancelReason.Items.Insert(0, new RadComboBoxItem(" Select ", "0"));
            ddlServiceCancelReason.Items.Insert(1, new RadComboBoxItem(" Other ", "1"));

            ddlServiceCancelReason.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(Ex, true);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + (Ex.ToString() + " " + "Line: " + trace.GetFrame(0).GetFileLineNumber());
        }
    }


}
