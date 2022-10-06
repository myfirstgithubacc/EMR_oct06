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
using BaseC;
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class EMRBILLING_Popup_Servicedetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || Session["HospitalLocationID"] == null || Session["FacilityId"] == null)
        {
            Response.Redirect("~/Login.aspx?Logout=1", false);
            return;
        }
        try
        {
            if (!IsPostBack)
            {

                if (common.myStr(Request.QueryString["RegNo"]) != "")
                {
                    BindPatientHiddenDetails(common.myInt(Request.QueryString["RegNo"]));
                }
                if (common.myInt(Request.QueryString["AuthorizedPer"]) != 0)
                {
                    hdnAuthorizedDiscPercent.Value = common.myStr(Request.QueryString["AuthorizedPer"]);
                    hdnAuthorizedRemark.Value = common.myStr(Request.QueryString["Remarks"]);
                }
                if (common.myStr(Request.QueryString["DeptName"]) != "")
                {
                    Page.Title = "Service Details ( " + common.myStr(Request.QueryString["DeptName"]) + " )";
                }
                if (common.myStr(Request.QueryString["Decimal"]) != "")
                {
                    hdnDecimalPlaces.Value = common.myStr(Request.QueryString["Decimal"]);
                }

                if ((common.myStr(Request.QueryString["DeptId"]) != "") || (common.myStr(Request.QueryString["DeptType"]) != ""))
                {
                    hdnDepartmentId.Value = common.myStr(Request.QueryString["DeptId"]);
                    GetServiceIDFromRegnEncID(common.myInt(hdnRegId.Value), common.myInt(Request.QueryString["EncId"]),
                        common.myInt(hdnDepartmentId.Value));
                    //GetDepartmenttype(common.myInt(hdnRegId.Value), common.myInt(Request.QueryString["EncId"]), 0);
                }
                BindDepartment();
                foreach (GridViewRow gvr in gvService.Rows)
                {
                    ImageButton btnDelete = (ImageButton)gvr.FindControl("ibtndaDelete");
                    HiddenField hdnStatusId = (HiddenField)gvr.FindControl("hdnStatusId");
                    HiddenField hdnStatusCode = (HiddenField)gvr.FindControl("hdnStatusCode");

                    if (common.myStr(hdnStatusCode.Value) != "SNC" && common.myStr(hdnStatusCode.Value) != "SC" && common.myStr(hdnStatusCode.Value) != "SD")
                    {

                        UserAuthorisations ua1 = new UserAuthorisations();
                        ua1.DisableEnableControl(btnDelete, false);

                        if (ua1.CheckPermissions("C", Request.Url.AbsolutePath))
                        {
                            ua1.DisableEnableControl(btnDelete, true);

                        }     // }
                        else
                        {
                            ua1.DisableEnableControl(btnDelete, false);

                        }
                        ua1.Dispose();
                    }
                }

                SetPermission();

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
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
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ua1.Dispose();
        }
    }

    private void GetServiceIDFromRegnEncID(Int32 iRegID, Int32 iEncID, Int32 iDeptId)
    {
        DataSet ds = new DataSet();
        DataTable dtService = new DataTable();
        clsEMR objBill = new clsEMR(sConString);
        BaseC.RestFulAPI objwcfBill = new BaseC.RestFulAPI(sConString);
        DataView dv = new DataView();
        try
        {
            if (Request.QueryString["PType"].ToString() == "WD")
            {
                ds = objBill.GetEMRServiceOrderDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), iRegID, iEncID, common.myInt(Request.QueryString["BillId"]), iDeptId, common.myStr(Request.QueryString["DeptType"]));
            }
            else
            {
                ds = objwcfBill.GetIPServiceOrderDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), iRegID, iEncID, common.myInt(Request.QueryString["BillId"]), iDeptId, common.myStr(Request.QueryString["DeptType"]));
            }
            if (ds.Tables.Count > 0)
            {
                dv = new DataView(ds.Tables[0]);
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    hdnDetailType.Value = common.myStr(ds.Tables[1].Rows[0]["DetailType"]);
                }
                if (common.myStr(Request.QueryString["PType"]).Equals("WD"))
                {
                    //dv.RowFilter = "ServiceType IN('I','IS','P','OS','C','O','OPP')";
                    //dv.RowFilter = "ServiceType NOT IN('R','VF','CL','RF','IPP','S')";

                    /*CHANGED ON 27-FEB-2017 */
                    //dv.RowFilter = "ServiceType NOT IN('R','VF','CL','RF','IPP')";
                    dv.RowFilter = "ServiceType NOT IN('R','VF','RF','IPP')";
                    /*CHANGED ON 27-FEB-2017 */
                    if (dv != null && dv.ToTable().Rows.Count > 0)
                    {
                        gvService.DataSource = dv.ToTable();
                        gvService.DataBind();

                        ViewState["dtServices"] = dv.ToTable().Copy();
                    }

                }
                else
                {
                    gvService.DataSource = ds;
                    gvService.DataBind();
                    ViewState["dtServices"] = ds.Tables[0].Copy();
                }
                if (common.myInt(Request.QueryString["BillId"]) > 0)
                {
                    //gvService.Enabled = false;
                    btnSave.Visible = false;
                }


                dv.Sort = "Service";
                //dv.RowFilter = "";
                dtService = dv.ToTable(true, "Service", "ServiceId");
                if (dtService.Rows.Count > 0)
                {
                    if (gvService.Rows.Count > 0)
                    {
                        RadComboBox ddlS = (RadComboBox)gvService.HeaderRow.FindControl("ddlService");
                        if (ddlS != null)
                        {
                            ViewState["dtDdlServices"] = dtService;

                            ddlS.DataSource = dtService;
                            ddlS.DataTextField = "Service";
                            ddlS.DataValueField = "ServiceId";
                            ddlS.DataBind();
                            ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
                        }
                    }
                }

                dtService = dv.ToTable(true, "DoctorName", "DoctorId");
                if (dtService.Rows.Count > 0)
                {
                    if (gvService.Rows.Count > 0)
                    {
                        RadComboBox ddlP = (RadComboBox)gvService.HeaderRow.FindControl("ddlProvider");
                        if (ddlP != null)
                        {
                            ViewState["dtDdlProvider"] = dtService;
                            ddlP.DataSource = dtService;
                            ddlP.DataTextField = "DoctorName";
                            ddlP.DataValueField = "DoctorId";
                            ddlP.DataBind();
                            ddlP.Items.Insert(0, new RadComboBoxItem(" --- All Provider --- ", "0"));
                        }
                    }
                }

            }
            else
            {
                gvService.DataSource = null;
                gvService.DataBind();
                ViewState["dtServices"] = null;
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
            dv.Dispose();
            ds.Dispose();
            dtService.Dispose();
            objBill = null;
            objwcfBill = null;
        }
    }
    protected void ddlService_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        if (ViewState["dtServices"] != null)
        {
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            try
            {
                lblMessage.Text = "";
                hdnTotServiceAmount.Value = "0.00";
                hdnTotDoctorAmount.Value = "0.00";
                hdnTotGrossAmt.Value = "0.00";
                hdnTotDiscountAmount.Value = "0.00";
                hdnAmountPayableByPatient.Value = "0.00";
                hdnAmountPayableByPayer.Value = "0.00";
                dt = ViewState["dtServices"] as DataTable;
                dv = dt.DefaultView;
                RadComboBox ddlP = (RadComboBox)gvService.HeaderRow.FindControl("ddlProvider");
                string sValue = e.Value;
                if (e.Value == "0")
                {
                    gvService.DataSource = dt;
                    gvService.DataBind();
                }
                else
                {
                    dv.RowFilter = "ServiceId = " + e.Value;
                    gvService.DataSource = dv.ToTable();
                    gvService.DataBind();
                }

                dt = (DataTable)ViewState["dtDdlServices"];
                RadComboBox ddlS = (RadComboBox)gvService.HeaderRow.FindControl("ddlService");
                if (ddlS != null)
                {
                    ddlS.DataSource = dt;
                    ddlS.DataTextField = "Service";
                    ddlS.DataValueField = "ServiceId";
                    ddlS.DataBind();
                    ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
                    ddlS.SelectedValue = sValue;
                }


                RadComboBox ddlP1 = (RadComboBox)gvService.HeaderRow.FindControl("ddlProvider");
                if (ddlP1 != null)
                {
                    dt = dv.ToTable(true, "DoctorName", "DoctorId");
                    ViewState["dtDdlProvider"] = dt;
                    ddlP1.DataSource = dt;
                    ddlP1.DataTextField = "DoctorName";
                    ddlP1.DataValueField = "DoctorId";
                    ddlP1.DataBind();
                    ddlP1.Items.Insert(0, new RadComboBoxItem(" --- All Provider --- ", "0"));
                }
            }
            catch (Exception ex)
            {
                objException.HandleException(ex);
            }
            finally
            {
                dt.Dispose();
                dv.Dispose();
            }
        }
    }
    protected void ddlProvider_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ViewState["dtServices"] == null)
            return;
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            lblMessage.Text = "";
            hdnTotServiceAmount.Value = "0.00";
            hdnTotDoctorAmount.Value = "0.00";
            hdnTotGrossAmt.Value = "0.00";
            hdnTotDiscountAmount.Value = "0.00";
            hdnAmountPayableByPatient.Value = "0.00";
            hdnAmountPayableByPayer.Value = "0.00";


            dt = (DataTable)ViewState["dtServices"];
            RadComboBox ddlS = (RadComboBox)gvService.HeaderRow.FindControl("ddlService");
            if (ddlS != null)
            {
                string SelectedService = ddlS.SelectedValue;
                string sValue = e.Value;
                dv = dt.DefaultView;
                if (e.Value == "0")
                {
                    if (common.myInt(ddlS.SelectedValue) == 0)
                    {
                        gvService.DataSource = dt;
                    }
                    else
                    {
                        dv.RowFilter = "ServiceId = " + SelectedService;
                        gvService.DataSource = dv.ToTable();
                    }
                    gvService.DataBind();
                }
                else
                {

                    if (common.myInt(ddlS.SelectedValue) == 0)
                    {
                        dv.RowFilter = "DoctorId = " + e.Value;
                    }
                    else
                    {
                        dv.RowFilter = "ServiceId = " + ddlS.SelectedValue + " and DoctorId = " + e.Value;
                    }
                    gvService.DataSource = dv.ToTable();
                    gvService.DataBind();
                }

                dt = (DataTable)ViewState["dtDdlServices"];
                ddlS.DataSource = dt;
                ddlS.DataTextField = "Service";
                ddlS.DataValueField = "ServiceId";
                ddlS.DataBind();
                ddlS.Items.Insert(0, new RadComboBoxItem(" --- All Services --- ", "0"));
                ddlS.SelectedValue = SelectedService;


                dt = (DataTable)ViewState["dtDdlProvider"];
                RadComboBox ddlP = (RadComboBox)gvService.HeaderRow.FindControl("ddlProvider");
                if (ddlP != null)
                {
                    ddlP.DataSource = dt;
                    ddlP.DataTextField = "DoctorName";
                    ddlP.DataValueField = "DoctorId";
                    ddlP.DataBind();
                    ddlP.Items.Insert(0, new RadComboBoxItem(" --- All Provider --- ", "0"));
                    ddlP.SelectedValue = sValue;
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            dv.Dispose();
        }
    }
    private void BindDropDownServiceAndProvider()
    {

    }
    private void GetDepartmenttype(Int32 iRegID, Int32 iEncID, Int32 iBillNo)
    {
        EMRBilling.clsOrderNBill baseBill = new EMRBilling.clsOrderNBill(sConString);
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        try
        {

            ds = baseBill.getServiceOrderDeptTypeWise(Convert.ToInt32(Session["HospitalLocationID"].ToString()),
                Convert.ToInt32(Session["FacilityId"].ToString()), Convert.ToInt32(iEncID), "", "", 0, 1, "");

            //hdnRegId.Value
            dv = new DataView(ds.Tables[0]);
            if (Request.QueryString["PType"].ToString() != "WD")
            {
                dv.RowFilter = "DepartmentID =" + hdnDepartmentId.Value + "";
            }

            gvService.DataSource = dv.ToTable();
            gvService.DataBind();

        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            dv.Dispose();
        }
    }
    public DataSet DoctorBind()
    {
        DataSet ds = new DataSet();
        Hospital baseHosp = new Hospital(sConString);
        try
        {
            ds = baseHosp.fillDoctorCombo(Convert.ToInt16(Session["HospitalLocationID"].ToString()), 0, Convert.ToInt16(Session["FacilityId"].ToString()));
            return ds;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
            return ds;
        }
        finally
        {
            ds.Dispose();
            baseHosp = null;
        }
    }
    protected void gvService_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.Footer || e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Request.QueryString["PType"].ToString() == "WD")
                {
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[11].Visible = false;
                }
                else if (common.myStr(hdnDetailType.Value) == "PH")
                {
                    e.Row.Cells[4].Visible = false;
                    // e.Row.Cells[7].Visible = false;
                    e.Row.Cells[7].Width = Unit.Pixel(0);
                }
                else
                {
                    e.Row.Cells[8].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSno = (Label)e.Row.FindControl("lblSno");
                Label lblOrderNo = (Label)e.Row.FindControl("lblOrderNo");
                Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
                // Label lblServiceName = (Label)e.Row.FindControl("lblServiceName");
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


                //DropDownList ddlDoctor = (DropDownList)e.Row.FindControl("ddlDoctor");
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

                int IsDiscountable = common.myInt(hdnIsDiscountable.Value);

                //lblPayableAmt.Text = common.myStr(common.myDec(txtServiceAmount.Text) + common.myDec(txtDoctorAmount.Text));
                //lblPayableAmt.Text = common.myDec(lblPayableAmt.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                if (common.myStr(hdnStatusCode.Value) == "SNC")
                    e.Row.BackColor = System.Drawing.Color.Bisque;
                else if (common.myStr(hdnStatusCode.Value) == "SC")
                    e.Row.BackColor = System.Drawing.Color.Bisque;
                else if (common.myStr(hdnStatusCode.Value) == "SD")
                    e.Row.BackColor = System.Drawing.Color.Bisque;

                if (common.myInt(hdnClinicalDetailFound.Value) == 1)
                    lblServiceName.ForeColor = System.Drawing.Color.Blue;
                else
                    lblServiceName.ForeColor = System.Drawing.Color.Black;

                if (common.myStr(hdnActive.Value) == "False" || common.myStr(hdnActive.Value) == "0")
                {
                    lblServiceName.ForeColor = System.Drawing.Color.Gray;
                    lblUnit.ForeColor = System.Drawing.Color.Gray;
                    lblSno.ForeColor = System.Drawing.Color.Gray;
                    lblOrderDate.ForeColor = System.Drawing.Color.Gray;
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Gray;
                    //e.Row.Cells[3].ForeColor = System.Drawing.Color.Gray;
                    e.Row.Cells[10].ForeColor = System.Drawing.Color.Gray;
                    ibtndaDelete.Visible = false;
                }
                string serviceType = common.myStr(hdnServType.Value);
                if ((common.myInt(hdnDocReq.Value) == 1 || common.myStr(hdnDocReq.Value) == "True") && common.myInt(hdnDoctorID.Value) > 0)
                {
                    if (serviceType == "P" || serviceType == "I" || serviceType == "IS" || serviceType == "C" || serviceType == "O")
                    {
                        txtDoctorAmount.Enabled = true;
                    }
                }
                if (common.myInt(hdnIsPackageMain.Value) == 1)
                {
                    lblServiceName.Font.Bold = true;
                }
                if (hdnServType.Value == "PH")
                {
                    ibtndaDelete.Visible = false;
                }
                if (common.myDec(hdnAuthorizedDiscPercent.Value) > 0 && (common.myStr(hdnActive.Value) == "True" || common.myStr(hdnActive.Value) == "1") && IsDiscountable == 1)
                {
                    txtPercentDiscount.Enabled = true;
                    txtDiscountAmt.Enabled = true;
                }
                else
                {
                    txtPercentDiscount.Enabled = false;
                    txtDiscountAmt.Enabled = false;
                }
                if (common.myStr(hdnActive.Value) == "False" || common.myStr(hdnActive.Value) == "0" || hdnServType.Value == "PH" || common.myInt(hdnPriceEditable.Value) == 0)
                {
                    txtServiceAmount.Enabled = false;
                    txtDoctorAmount.Enabled = false;
                }
                if (common.myInt(hdnIsPackageService.Value) != 1 && (common.myStr(hdnActive.Value) == "True" || common.myStr(hdnActive.Value) == "1"))
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

                txtServiceAmount.Attributes.Add("onkeyup", "javascript:CalculateAmountWhenUpdateServiceAmount('" + txtServiceAmount.ClientID + "','" + txtDoctorAmount.ClientID + "','" + lblUnit.ClientID + "','" + txtPercentDiscount.ClientID + "','" + txtDiscountAmt.ClientID + "','" + lblPayableAmt.ClientID + "','" + hdnServiceId.ClientID + "');");

                e.Row.Attributes.Add("onclick", "javascript:ShowEncodDetails('" + lblServiceName.ClientID + "','" + hdnEncodedBy.ClientID +
                            "','" + hdnEncodedDate.ClientID + "','" + hdnLastChangedBy.ClientID +
                            "','" + hdnLastChangedDate.ClientID + "','" + lblSno.ClientID +
                            "','" + lblOrderNo.ClientID + "','" + lblOrderDate.ClientID + "','" + hdnBillCategory.ClientID + "','" + hdnManualEntryNo.ClientID + "');");

                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
                {
                    ibtndaDelete.Enabled = false;

                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
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
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void BindPatientHiddenDetails(int RegistrationNo)
    {
        DataSet ds = new DataSet();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
        try
        {
            if (RegistrationNo > 0)
            {
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int RegId = 0;
                int EncodedBy = common.myInt(Session["UserId"]);


                if (common.myStr(Session["OPIP"]) == "O")
                {
                    ds = bC.getPatientDetails(HospId, FacilityId, RegId, RegistrationNo, 0, EncodedBy);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        hdnRegId.Value = common.myStr(dr["RegistrationId"]);
                        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                        lblDob.Text = common.myStr(dr["DOB"]);
                        lblMobile.Text = common.myStr(dr["MobileNo"]);
                        Label3.Text = "OP No. : ";
                        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                        lblAdmissionDate.Visible = false;
                    }
                }
                else  //I
                {
                    ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, RegistrationNo, EncodedBy, 0, "");

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
                            //   lblMessage.Text = "";
                        }
                    }
                    else
                    {
                        //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        //lblMessage.Text = "Patient not found !";
                        return;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
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
        try
        {
            if (e.CommandName == "Del")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                if (common.myInt(Session["ModuleId"]).Equals(3))
                {
                    if (!common.myBool(ViewState["IsAllowCancel"]))
                    {
                        Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                        return;
                    }

                    //HiddenField hdnEnteredBy = (HiddenField)row.FindControl("hdnEnteredBy");
                    //if (!common.myInt(Session["UserId"]).Equals(common.myInt(hdnEnteredBy.Value)))
                    //{
                    //    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                    //    return;
                    //}
                }

                if (common.myStr(txtCancelationRemarks.Text) == "")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please enter cancellation notes !";
                    txtCancelationRemarks.Focus();
                    return;
                }

                HiddenField hdnDetailId = (HiddenField)row.FindControl("hdnDetailId");
                HiddenField hdnServiceId = (HiddenField)row.FindControl("hdnServiceId");
                HiddenField hdnOrderId = (HiddenField)row.FindControl("hdnOrderId");
                HiddenField hdnSODAID = (HiddenField)row.FindControl("hdnSODAID");

                if ((common.myInt(hdnServiceId.Value) > 0) && (common.myInt(hdnOrderId.Value) > 0))
                {
                    BaseC.EMROrders objOrders = new BaseC.EMROrders(sConString);
                    string strmsg = objOrders.DeActivateOrderServices(common.myInt(hdnSODAID.Value), common.myInt(hdnServiceId.Value),
                        common.myInt(hdnOrderId.Value), common.myInt(hdnDetailId.Value),
                        common.myStr(txtCancelationRemarks.Text, true).Trim(),
                        common.myInt(Session["HospitalLocationId"]),
                        common.myInt(Session["FacilityId"]),
                        common.myInt(Session["UserId"]));
                    if (strmsg.Contains("Succeeded"))
                    {
                        hdnTotServiceAmount.Value = "0.00";
                        hdnTotDoctorAmount.Value = "0.00";
                        hdnTotGrossAmt.Value = "0.00";
                        hdnTotDiscountAmount.Value = "0.00";
                        hdnAmountPayableByPatient.Value = "0.00";
                        hdnAmountPayableByPayer.Value = "0.00";
                        GetServiceIDFromRegnEncID(common.myInt(hdnRegId.Value), common.myInt(Request.QueryString["EncId"]), common.myInt(hdnDepartmentId.Value));
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = strmsg;
                        return;
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Service order not saved !";
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
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

                strXML.Append(common.setXmlTable(ref coll));
            }
            if (strXML.ToString().Trim().Length > 1)
            {

                lblMessage.Text = objBilling.InsertIPServiceDiscount(common.myInt(Request.QueryString["EncId"]), common.myStr(hdnDetailType.Value), common.myInt(Session["UserId"]), strXML.ToString());
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
            objException.HandleException(Ex);
        }
        finally
        {
            hshInput = null;
            hshOutput = null;
            strXML = null;
            coll = null;
            objBilling = null;
        }
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
            if (common.myInt(sServiceId) > 0
            && common.myInt(hdnClinicalDetailFound.Value) == 1)
            {
                Session["EncounterId"] = hdnEncounterId.Value;
                Session["RegistrationId"] = hdnRegId.Value;

                RadWindow3.NavigateUrl = "~/EMR/Orders/ViewDetails.aspx?MASTER=No&ServId=" + sServiceId + "&ServName=" +
                   lnkName.Text + "&From=POPUP&RegNo=" + common.myStr(Request.QueryString["RegNo"]) + "&EncounterId=" +
                   common.myStr(hdnEncounterId.Value) + "&sOrdDtlId=" + common.myStr(hdnServiceDetailId.Value);// +"&TagType=D";

                //RadWindowForNew.NavigateUrl = "~/EMR/Orders/ViewDetails.aspx?MASTER=No&ServId=" + sServiceId + "&ServName=" +
                //lnkName.Text + "&From=POPUP&RegNo=" + common.myStr(Request.QueryString["RegNo"]) + "&EncounterId=" +
                //common.myStr(hdnEncounterId.Value) + "&PatientType=" + common.myStr(ViewState["Source"]) + "&RegID=" + common.myStr(ViewState["RegistrationId"]) +
                //"&sOrdDtlId=" + common.myStr(hdnServiceDetailId.Value) + "&FacliityID=" + common.myStr(ViewState["FacilityId"]);// +"&TagType=D";
                RadWindow3.Height = 600;
                RadWindow3.Width = 900;
                RadWindow3.Top = 20;
                RadWindow3.Left = 20;
                // RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow3.Modal = true;
                // RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow3.VisibleStatusbar = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void BindDepartment()
    {
        DataSet ds = new DataSet();
        BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);
        try
        {
            ddlDepartment.Items.Clear();

            ds = bMstr.GetDepartmentAccordingEncounter(common.myInt(Session["HospitalLocationID"].ToString()), common.myInt(Session["FacilityId"]), common.myInt(hdnRegId.Value), common.myInt(Request.QueryString["EncId"]));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlDepartment.DataSource = ds.Tables[0];
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataBind();
            }
            ddlDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
            ddlDepartment.SelectedIndex = 0;
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
            bMstr = null;
        }
    }
    protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlDepartment.SelectedValue != "-1")
            {
                lblMessage.Text = "";
                hdnTotServiceAmount.Value = "0.00";
                hdnTotDoctorAmount.Value = "0.00";
                hdnTotGrossAmt.Value = "0.00";
                hdnTotDiscountAmount.Value = "0.00";
                hdnAmountPayableByPatient.Value = "0.00";
                hdnAmountPayableByPayer.Value = "0.00";
                GetServiceIDFromRegnEncID(common.myInt(hdnRegId.Value), common.myInt(Request.QueryString["EncId"]), common.myInt(ddlDepartment.SelectedValue));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvService_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["dtServices"] == null)
            return;
        //   DataSet myDataSet = (DataSet)ViewState["dtServices"];
        gvService.DataSource = ViewState["dtServices"] as DataTable;

        gvService.PageIndex = e.NewPageIndex;
        gvService.DataBind();
    }

}
