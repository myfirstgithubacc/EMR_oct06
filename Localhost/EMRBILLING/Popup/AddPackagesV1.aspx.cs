using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseC;
using Telerik.Web.UI;

public partial class EMRBILLING_Popup_AddPackagesV1 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    private const int ItemsPerRequest = 50;
    protected void Page_Load(object sender, EventArgs e)
    {
        clsEMRBilling objBill = new clsEMRBilling(sConString);
        try
        {
            if (!IsPostBack)
            {
                hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));
                if (common.myInt(hdnDecimalPlaces.Value) == 0)
                {
                    hdnDecimalPlaces.Value = "2";
                }
                ViewState["GridData"] = null;
                Session["PkgSvChk"] = 0;
                dtOrderDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtOrderDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtOrderDate.SelectedDate = common.myDate(DateTime.Now);
                BindMinutes();
                rdtpOtStartTime.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpOtStartTime.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpOtStartTime.SelectedDate = common.myDate(DateTime.Now);

                rdtpOtEndTime.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpOtEndTime.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpOtEndTime.SelectedDate = common.myDate(DateTime.Now);

                rdtpAstartTime.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpAstartTime.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpAstartTime.SelectedDate = common.myDate(DateTime.Now);

                rdtpAEndTime.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpAEndTime.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpAEndTime.SelectedDate = common.myDate(DateTime.Now);

                if (common.myStr(Request.QueryString["OP_IP"]).Equals("O"))
                {
                    dtOrderDate.Enabled = false;
                    ddlOrderMinutes.Enabled = false;
                }
                else
                {
                    dtOrderDate.Enabled = true;
                    ddlOrderMinutes.Enabled = true;
                }
                clearAll();

                txtRegID.Text = common.myStr(Request.QueryString["RegId"]);
                txtRegNo.Text = common.myStr(Request.QueryString["RegNo"]);
                txtEncId.Text = common.myStr(Request.QueryString["EncId"]);

                ViewState["IPNumber"] = txtEncId.Text.Trim();
                BindBedCategory();
                if (common.myStr(Request.QueryString["OP_IP"]).Equals("O") && common.myStr(Request.QueryString["OTBookingId"]).Equals(""))
                {
                    radCmbBedCategory.SelectedValue = common.myStr(Cache["DefaultOPDCategory"]);
                    radCmbBedCategory.Enabled = false;
                }
                if (!common.myStr(Request.QueryString["RegNo"]).Equals(""))
                {
                    int RegId = common.myInt(Request.QueryString["RegId"]);
                    int EncId = common.myInt(Request.QueryString["EncId"]);

                    BindPatientHiddenDetails(common.myInt(Request.QueryString["RegNo"]), RegId, EncId);
                }

                hdnPaymentTypePyr.Value = common.myStr(Request.QueryString["PayType"]);

                if (common.myStr(Request.QueryString["OP_IP"]).Equals("I"))
                {
                    ibtnSave.Text = "Save";
                    ibtnSave.ToolTip = "Click to save";
                }
                ViewState["GridData"] = null;
                BindDepartment();
                cmbDept_OnSelectedIndexChanged(sender, e);
                if (!common.myStr(Request.QueryString["OTBookingId"]).Equals(""))
                {
                    chkUnClean.Visible = true;
                    BindOTPackageDetails(common.myInt(Request.QueryString["OTBookingId"]));
                }

                if (!string.IsNullOrEmpty(common.myStr(Request.QueryString["ServiceID"])))
                {
                    BindPackageSurgeryFlagBased(common.myStr(Request.QueryString["ServiceID"]));
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
            objBill = null;
        }
    }
    private void BindOTPackageDetails(int OTBookingId)
    {
        clsOTBooking objOTBooking = new clsOTBooking(sConString);
        DataSet ds = new DataSet();
        try
        {
            string OTBookingNo = "";
            ds = objOTBooking.getOTBookingDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), OTBookingId, OTBookingNo);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime dtFromDate, FromTime, ToTime, dtFromDateTime, dtToDateTime;
                    DataRow drService = ds.Tables[0].Rows[0];
                    hdnOTRoomId.Value = common.myStr(common.myInt(drService["TheaterId"]));
                    cmbDept.SelectedIndex = cmbDept.Items.IndexOf(cmbDept.FindItemByValue(common.myStr(drService["DepartmentID"])));
                    cmbDept_OnSelectedIndexChanged(this, null);
                    cmbSubDept.SelectedIndex = cmbSubDept.Items.IndexOf(cmbSubDept.FindItemByValue(common.myStr(drService["SubDepartmentId"])));
                    cmbSubDept_OnSelectedIndexChanged(this, null);
                    hdnOTServiceId.Value = (common.myInt(drService["ServiceID"])).ToString();
                    cmbPackages.SelectedIndex = cmbPackages.Items.IndexOf(cmbPackages.FindItemByValue(common.myStr(drService["ServiceID"])));
                    cmbPackages.Text = common.myStr(drService["ServiceName"]);
                    cbMainPackage.Checked = true;

                    dtFromDate = Convert.ToDateTime(drService["OTBookingDate"]);
                    FromTime = common.myDate(common.myStr(drService["FromTime"]));
                    ToTime = common.myDate(common.myStr(drService["ToTime"]));
                    dtFromDateTime = common.myDate(dtFromDate.ToShortDateString() + " " + FromTime.TimeOfDay);
                    dtToDateTime = common.myDate(dtFromDate.ToShortDateString() + " " + ToTime.TimeOfDay);
                    rdtpOtStartTime.SelectedDate = common.myDate(dtFromDateTime);
                    rdtpOtEndTime.SelectedDate = common.myDate(dtToDateTime);
                    rdtpAstartTime.SelectedDate = common.myDate(dtFromDateTime);
                    rdtpAEndTime.SelectedDate = common.myDate(dtToDateTime);
                }
            }
            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ViewState["dtResource"] = (DataTable)ds.Tables[1];
                }
            }
            btnAddToGrid_Click(this, null);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objOTBooking = null;
            ds.Dispose();
        }
    }
    private void BindBedCategory()
    {
        EMRMasters objSurgery = new EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objSurgery.GetService((common.myInt(Session["HospitalLocationID"])), "0", "'R'", common.myInt(Session["FacilityId"]));
            radCmbBedCategory.Items.Clear();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                radCmbBedCategory.DataSource = ds.Tables[0];
                radCmbBedCategory.DataValueField = "ServiceID";
                radCmbBedCategory.DataTextField = "ServiceName";
                radCmbBedCategory.DataBind();

                radCmbBedCategory.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
                radCmbBedCategory.SelectedIndex = 0;
            }
            else
            {
                radCmbBedCategory.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
                radCmbBedCategory.SelectedIndex = 0;
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
            objSurgery = null;
        }
    }
    private void BindDepartment()
    {
        DataSet ds = new DataSet();
        EMRMasters objSurgery = new EMRMasters(sConString);
        try
        {
            cmbDept.Items.Clear();
            ds = objSurgery.GetHospitalDepartment(common.myInt(Session["HospitalLocationID"]), "'IPP'");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmbDept.DataSource = ds.Tables[0];
                    cmbDept.DataTextField = "DepartmentName";
                    cmbDept.DataValueField = "DepartmentId";
                    cmbDept.DataBind();
                }
            }
            cmbDept.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
            cmbDept.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objSurgery = null;
            ds.Dispose();
        }
    }
    private void BindSubDepartment()
    {
        EMRMasters objSurgery = new EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objSurgery.GetHospitalSubDepartment(common.myInt(Session["HospitalLocationID"]), common.myInt(cmbDept.SelectedValue), common.myStr("'IPP'"), 0);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmbSubDept.Items.Clear();
                    cmbSubDept.DataSource = ds.Tables[0];
                    cmbSubDept.DataTextField = "SubName";
                    cmbSubDept.DataValueField = "SubDeptId";
                    cmbSubDept.DataBind();
                }
            }
            cmbSubDept.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
            cmbSubDept.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objSurgery = null;
            ds.Dispose();
        }
    }
    void BindPatientHiddenDetails(int RegistrationNo, Int32 RegistrationId, Int32 EncounterId)
    {
        Patient bC = new Patient(sConString);
        BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (RegistrationId > 0)
            {
                if (common.myStr(Request.QueryString["OP_IP"]).Equals("O"))
                {
                    ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), RegistrationId, RegistrationNo, EncounterId, common.myInt(Session["UserId"]));
                    lblInfoEncNo.Visible = false;
                    lblInfoAdmissionDt.Visible = false;
                    lblEncounterNo.Visible = false;
                    lblAdmissionDate.Visible = false;
                }
                else
                {
                    ds = objIPBill.GetPatientDetailsIP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), RegistrationId, RegistrationNo, common.myInt(Session["UserId"]), 0, "");
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                        lblDob.Text = common.myStr(dr["DOB"]);
                        lblMobile.Text = common.myStr(dr["MobileNo"]);
                        txtEncNo.Text = common.myStr(dr["EncounterNo"]);
                        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                        lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);

                        if (common.myStr(Request.QueryString["OP_IP"]).Equals("I"))
                        {
                            radCmbBedCategory.SelectedIndex = radCmbBedCategory.Items.IndexOf(radCmbBedCategory.Items.FindItemByValue(common.myStr(dr["CurrentBillCategory"])));
                            radCmbBedCategory.Enabled = false;
                        }
                        else if (common.myStr(Request.QueryString["OP_IP"]).Equals("O"))
                        {
                            lblEncounterNo.Visible = false;
                            lblAdmissionDate.Visible = false;
                            string sValue = objCommon.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), "DefaultOPDCategoryService"); // Default OP Bill Category.
                            radCmbBedCategory.SelectedIndex = radCmbBedCategory.Items.IndexOf(radCmbBedCategory.Items.FindItemByValue(sValue));
                        }
                        else
                        {
                            radCmbBedCategory.Enabled = true;
                        }
                        lblMsg.Text = "";
                    }
                }
                else
                {
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "Patient not found !";
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
        finally
        {
            bC = null;
            objIPBill = null;
            objCommon = null;
            ds.Dispose();
        }
    }

    protected void ddlService_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            RadComboBox ddl = sender as RadComboBox;
            GridViewRow row = ddl.NamingContainer as GridViewRow;

            data = GetData(e.Text);
            if (data.Rows.Count > 0)
            {
                int itemOffset = e.NumberOfItems;
                int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
                e.EndOfItems = endOffset == data.Rows.Count;

                for (int i = itemOffset; i < endOffset; i++)
                {
                    ddl.Items.Add(new RadComboBoxItem(common.myStr(data.Rows[i]["ServiceName"]), common.myStr(data.Rows[i]["ServiceId"])));
                }
                e.Message = GetStatusMessage(endOffset, data.Rows.Count);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { data.Dispose(); }
    }

    void BindPackageSurgeryFlagBased(string ServiceId)
    {
        DataTable data = new DataTable();
        try
        {

            cmbPackages.Items.Clear();
            cmbPackages.Text = "";
            data = GetData(ServiceId.ToString());     

            foreach (DataRowView dr in data.DefaultView)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(dr["ServiceName"]) + " " + common.myStr(dr["RefServiceCode"]) + " " + common.myStr(dr["CPTCode"]);
                item.Value = dr["ServiceId"].ToString();
                this.cmbPackages.Items.Add(item);
                item.DataBind();
            }

            cmbPackages.SelectedIndex = cmbPackages.Items.IndexOf(cmbPackages.FindItemByValue(ServiceId));
            cmbPackages.Enabled = false;
            cmbDept.Enabled = false;
            cmbSubDept.Enabled = false;
            cmbPackages.ForeColor = System.Drawing.Color.Black;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { data.Dispose(); }
    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    private DataTable GetData(string text)
    {
        DataSet ds = new DataSet();
        DataTable data = new DataTable();
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        try
        {
            string ServiceName = text + "%";
            string strDepartmentType = "'IPP'";
            ds = objCommon.GetHospitalServices(common.myInt(Session["HospitalLocationId"]), common.myInt(cmbDept.SelectedValue), common.myInt(cmbSubDept.SelectedValue), strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]),0,0);

            data = ds.Tables[0];
            return data;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
            return data;
        }
        finally { ds.Dispose(); objCommon = null; }
    }
    private void BindMinutes()
    {
        try
        {
            for (int i = 0; i < 60; i++)
            {
                if (common.myStr(i).Length == 1)
                {
                    ddlOrderMinutes.Items.Add(new RadComboBoxItem("0" + common.myStr(i), "0" + common.myStr(i)));
                    radCmbOtStartTimeM.Items.Add(new RadComboBoxItem("0" + common.myStr(i), "0" + common.myStr(i)));
                    radCmbOtEndTimeM.Items.Add(new RadComboBoxItem("0" + common.myStr(i), "0" + common.myStr(i)));
                    radCmbAEndTimeM.Items.Add(new RadComboBoxItem("0" + common.myStr(i), "0" + common.myStr(i)));
                    radCmbAstartTimeM.Items.Add(new RadComboBoxItem("0" + common.myStr(i), "0" + common.myStr(i)));
                }
                else
                {
                    ddlOrderMinutes.Items.Add(new RadComboBoxItem(common.myStr(i), common.myStr(i)));
                    radCmbOtStartTimeM.Items.Add(new RadComboBoxItem(common.myStr(i), common.myStr(i)));
                    radCmbOtEndTimeM.Items.Add(new RadComboBoxItem(common.myStr(i), common.myStr(i)));
                    radCmbAEndTimeM.Items.Add(new RadComboBoxItem(common.myStr(i), common.myStr(i)));
                    radCmbAstartTimeM.Items.Add(new RadComboBoxItem(common.myStr(i), common.myStr(i)));
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
    protected void radCmbOtStartTimeM_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            sb.Append(common.myStr(rdtpOtStartTime.SelectedDate.Value));
            sb.Remove(common.myStr(rdtpOtStartTime.SelectedDate.Value).IndexOf(":") + 1, 2);
            sb.Insert(common.myStr(rdtpOtStartTime.SelectedDate.Value).IndexOf(":") + 1, radCmbOtStartTimeM.Text);
            rdtpOtStartTime.SelectedDate = Convert.ToDateTime(common.myStr(sb));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { sb = null; }
    }
    protected void radCmbOtEndTimeM_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            sb.Append(common.myStr(rdtpOtEndTime.SelectedDate.Value));
            sb.Remove(common.myStr(rdtpOtEndTime.SelectedDate.Value).IndexOf(":") + 1, 2);
            sb.Insert(common.myStr(rdtpOtEndTime.SelectedDate.Value).IndexOf(":") + 1, radCmbOtEndTimeM.Text);
            rdtpOtEndTime.SelectedDate = Convert.ToDateTime(common.myStr(sb));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { sb = null; }
    }
    protected void radCmbAEndTimeM_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        StringBuilder sb = new StringBuilder();
        try
        {
            sb.Append(common.myStr(rdtpAEndTime.SelectedDate.Value));
            sb.Remove(common.myStr(rdtpAEndTime.SelectedDate.Value).IndexOf(":") + 1, 2);
            sb.Insert(common.myStr(rdtpAEndTime.SelectedDate.Value).IndexOf(":") + 1, radCmbAEndTimeM.Text);
            rdtpAEndTime.SelectedDate = Convert.ToDateTime(common.myStr(sb));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { sb = null; }
    }
    protected void radCmbAstartTime_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            sb.Append(common.myStr(rdtpAstartTime.SelectedDate.Value));
            sb.Remove(common.myStr(rdtpAstartTime.SelectedDate.Value).IndexOf(":") + 1, 2);
            sb.Insert(common.myStr(rdtpAstartTime.SelectedDate.Value).IndexOf(":") + 1, radCmbAstartTimeM.Text);
            rdtpAstartTime.SelectedDate = Convert.ToDateTime(common.myStr(sb));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { sb = null; }
    }
    protected void ddlOrderMinutes_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            sb.Append(common.myStr(dtOrderDate.SelectedDate.Value));
            sb.Remove(common.myStr(dtOrderDate.SelectedDate.Value).IndexOf(":") + 1, 2);
            sb.Insert(common.myStr(dtOrderDate.SelectedDate.Value).IndexOf(":") + 1, ddlOrderMinutes.Text);
            dtOrderDate.SelectedDate = Convert.ToDateTime(common.myStr(sb));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { sb = null; }
    }
    protected void ibtSave_OnClick(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        Hashtable hshInput = new Hashtable();
        Hashtable hshOutput = new Hashtable();
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        StringBuilder sbOT = new StringBuilder();
        ArrayList coll2 = new ArrayList();
        clsEMRBilling baseEBill = new clsEMRBilling(sConString);
        EMRBilling.clsOrderNBill BaseBill = new EMRBilling.clsOrderNBill(sConString);
        BaseC.RestFulAPI objOT = new BaseC.RestFulAPI(sConString);
        try
        {
            if (common.myInt(Session["PkgSvChk"]).Equals(0))
            {
                dt = UpdateDataTable();
                if (dt == null)
                {
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "Add package to save...";
                    return;
                }

                int iDefaultCompanyId = baseEBill.getDefaultCompany(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]));
                int iPatientCompanyId = common.myInt(Request.QueryString["CompanyId"]);
                int MainPackageId = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (common.myInt(dr["DoctorRequired"]) == 1)
                    {
                        if (common.myInt(dr["DoctorID"]) == 0)
                        {
                            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMsg.Text = "Surgeon name not define in surgory order...";
                            return;
                        }
                    }
                    coll.Add(common.myInt(dr["ServiceId"])); //ServiceId INT,
                    coll.Add(DBNull.Value); //VisitDate SMALLDATETIME,   common.myDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:00")
                    coll.Add(common.myInt(1)); //Units TINYINT,
                    coll.Add(common.myInt(dr["DoctorID"])); //DoctorId INT, 
                    coll.Add(common.myDec(dr["ServiceCharge"])); //ServiceAmount MONEY, (Charge - After calculating by %)
                    coll.Add(common.myDec(0)); //DoctorAmount MONEY,  
                    coll.Add(common.myDec(dr["ServiceDiscountAmt"])); //ServiceDiscountAmount MONEY, 
                    coll.Add(common.myDec(0)); //DoctorDiscountAmount MONEY,
                    coll.Add(common.myDec(dr["PayableByPatient"])); //AmountPayableByPatient MONEY,
                    coll.Add(common.myDec(dr["PayableByPayer"])); //AmountPayableByPayer MONEY,
                    coll.Add(common.myDec(dr["ServiceDiscountPerc"])); //ServiceDiscountPer MONEY,
                    coll.Add(common.myDec(0)); //DoctorDiscountPer MONEY,
                    coll.Add(common.myInt(dr["PackageId"])); //PackageId INT,  
                    coll.Add(common.myInt(0)); //OrderId INT,
                    if (common.myStr(dr["ServiceType"]) != "IPP")
                    {
                        coll.Add(common.myInt(1)); //UnderPackage BIT,   
                    }
                    else
                    {
                        coll.Add(common.myInt(0)); //UnderPackage BIT,   
                    }
                    coll.Add(DBNull.Value); //ICDID VARCHAR(100), 

                    if (common.myInt(dr["ResourceId"]) > 0)
                        coll.Add(common.myInt(dr["ResourceId"])); //ResourceID INT,
                    else
                        coll.Add(DBNull.Value); //ResourceID INT,

                    coll.Add(DBNull.Value); //SurgeryAmount MONEY, 
                    coll.Add(common.myDec(dr["ChargePercentage"])); //ProviderPercent MONEY,
                    coll.Add(common.myInt(dr["Id"])); //SeQNo INT, 
                    coll.Add(DBNull.Value); //Serviceremarks Varchar(150)
                    coll.Add(0); // DetailId

                    if (common.myInt(dr["IsMainPackage"]) == 1)
                    {
                        MainPackageId = common.myInt(dr["PackageId"]);
                    }
                    strXML.Append(common.setXmlTable(ref coll));
                }

                if (common.myInt(hdnOTRoomId.Value) > 0)
                    coll2.Add(common.myInt(hdnOTRoomId.Value));
                else
                    coll2.Add(DBNull.Value);

                coll2.Add(common.myStr(rdtpOtStartTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                coll2.Add(common.myStr(rdtpOtEndTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                coll2.Add(DBNull.Value);
                coll2.Add(common.myStr(rdtpAstartTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                coll2.Add(common.myStr(rdtpAEndTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                coll2.Add(common.myInt(radCmbBedCategory.SelectedValue));
                coll2.Add(common.myInt(MainPackageId));
                coll2.Add(common.myBool(chkUnClean.Checked));
                sbOT.Append(common.setXmlTable(ref coll2));

                if (strXML.ToString().Trim().Length > 1)
                {
                    int HospId = common.myInt(Session["HospitalLocationId"]);
                    int FacilityId = common.myInt(Session["FacilityId"]);
                    int UserId = common.myInt(Session["UserId"]);
                    int RegId = common.myInt(txtRegID.Text);
                    int EncId = common.myInt(txtEncId.Text);
                    int CompId = common.myInt(Request.QueryString["CompanyId"]);
                    int InsId = common.myInt(Request.QueryString["InsuranceId"]);
                    int CardId = common.myInt(Request.QueryString["CardId"]);
                    string PayerType = common.myStr(Request.QueryString["PayerType"]);
                    string OPIP = common.myStr(Request.QueryString["OP_IP"]);


                    string stype = "P" + common.myStr(Request.QueryString["OP_IP"]).ToUpper();
                    string sChargeCalculationRequired = "N";
                    Hashtable hshOut = BaseBill.saveOrders(HospId, FacilityId, RegId, EncId, strXML.ToString(), "",
                                                        UserId, 0, CompId, stype, PayerType, OPIP, InsId, CardId, common.myStr(sbOT),
                                                        Convert.ToDateTime(dtOrderDate.SelectedDate), sChargeCalculationRequired, false,
                                                        common.myInt(Session["EntrySite"]),common.myInt(Session["ModuleId"]));

                    if (common.myStr(hshOut["chvErrorStatus"]).Length == 0)
                    {
                        if (common.myInt(hshOut["intOrderId"]) > 0)
                        {
                            if (Request.QueryString["OTBookingId"] != null)
                            {
                                int OTBookingId = common.myInt(Request.QueryString["OTBookingId"]);
                                int OrderId = common.myInt(hshOut["intOrderId"]);

                                string Result = objOT.UpdateOTBookingWithOrderId(HospId, FacilityId, OTBookingId, OrderId, UserId, string.Empty, 0, string.Empty);
                                if (Result.Contains("Error"))
                                {
                                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                    lblMsg.Text = Result;
                                    return;
                                }
                            }
                        }
                        Session["PkgSvChk"] = 1;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                    }
                    else
                    {
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "There is some error while saving order." + common.myStr(hshOut["chvErrorStatus"]);
                    }
                }
            }
            else if (common.myInt(Session["PkgSvChk"]) == 1)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
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
            if(dt!=null)
            {
            dt.Dispose();
            }
            hshInput = null;
            hshOutput = null;
            strXML = null;
            coll = null;
            sbOT = null;
            coll2 = null;
            baseEBill = null;
            BaseBill = null;
            objOT = null;
        }
    }
    protected void cmbDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbDept.SelectedValue != "")
            {
                BindSubDepartment();
                cmbPackages.Text = "";
                cmbPackages.Items.Clear();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void cmbSubDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        cmbPackages.Text = "";
        cmbPackages.Items.Clear();
    }
    protected void btnAddToGrid_Click(object sender, EventArgs e)
    {
        rdoIncision.Enabled = false;
        DataTable dtAddedPackage = new DataTable();
        DataSet dsPackage = new DataSet();
        EMRBilling objBilling = new EMRBilling(sConString);
        lblMsg.Text = "";
        try
        {
            if (common.myInt(cmbPackages.SelectedValue) > 0 || common.myInt(hdnOTServiceId.Value) > 0)
            {
                dtAddedPackage = UpdateDataTable();
                if (dtAddedPackage != null)
                {
                    if (dtAddedPackage.Rows.Count > 0)
                    {
                        if (dtAddedPackage.Select("PackageId=" + common.myStr(cmbPackages.SelectedValue) + "").Length > 0)
                        {
                            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMsg.Text = "Package already added...";
                            return;
                        }
                    }
                    else
                    {
                        if (common.myBool(cbMainPackage.Checked) == false)
                        {
                            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMsg.Text = "First Package should always be Main Package !";
                            cbMainPackage.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    if (common.myBool(cbMainPackage.Checked) == false)
                    {
                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMsg.Text = "First Package should always be Main Package !";
                        cbMainPackage.Focus();
                        return;
                    }
                }
                int IsMainPackage = 0;
                if (cbMainPackage.Checked == true)
                    IsMainPackage = 1;
                hdnOTServiceId.Value = common.myInt(hdnOTServiceId.Value) == 0 ? (common.myInt(cmbPackages.SelectedValue)).ToString() : common.myInt(hdnOTServiceId.Value).ToString();
                dsPackage = objBilling.GetPackageSurgeryToFillOrderGrid(Convert.ToInt16(common.myInt(Session["HospitalLocationId"])), common.myInt(Session["FacilityId"]),
                    common.myInt(txtEncId.Text), common.myInt(hdnOTServiceId.Value), common.myInt(Request.QueryString["CompanyId"]), IsMainPackage, Convert.ToDateTime(dtOrderDate.SelectedDate),
                    common.myInt(ViewState["packageCount"]), common.myInt(rdoIncision.SelectedValue));
                if (dsPackage.Tables.Count > 0)
                {
                    if (dsPackage.Tables[0].Rows.Count > 0)
                    {
                        if (dtAddedPackage != null)
                        {
                            if (dtAddedPackage.Rows.Count > 0)
                            {
                                int MaxId = common.myInt(dtAddedPackage.Compute("max(Id)", string.Empty));
                                foreach (DataRow dr in dsPackage.Tables[0].Rows)
                                {
                                    MaxId = MaxId + 1;
                                    dr["Id"] = MaxId;
                                }
                                dsPackage.AcceptChanges();
                                dtAddedPackage.Merge(dsPackage.Tables[0]);
                            }
                            else
                            {
                                dsPackage.Tables[0].DefaultView.RowFilter = "ServiceType = 'IPP'";
                                dsPackage.Tables[0].DefaultView[0]["IsMainPackage"] = "1";
                                dsPackage.Tables[0].AcceptChanges();
                                dsPackage.Tables[0].DefaultView.RowFilter = "";
                                dtAddedPackage.Merge(dsPackage.Tables[0]);
                                cbMainPackage.Checked = false;
                                cbMainPackage.Enabled = false;
                            }
                        }
                        else
                        {
                            dsPackage.Tables[0].DefaultView.RowFilter = "ServiceType = 'IPP'";
                            dsPackage.Tables[0].DefaultView[0]["IsMainPackage"] = "1";
                            dsPackage.Tables[0].AcceptChanges();
                            dsPackage.Tables[0].DefaultView.RowFilter = "";
                            dtAddedPackage = dsPackage.Tables[0];
                            cbMainPackage.Checked = false;
                            cbMainPackage.Enabled = false;
                        }
                        hdnOTServiceId.Value = "0";
                        gvAddedSurgery.DataSource = dtAddedPackage;
                        gvAddedSurgery.DataBind();
                        bindDetailGrids(common.myInt(cmbPackages.SelectedValue));
                        ViewState["GridData"] = dtAddedPackage;
                        ViewState["packageCount"] = common.myStr((common.myInt(ViewState["packageCount"]) + 1));
                    }
                    else
                    {
                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMsg.Text = "Package breakup not defined";
                        return;
                    }
                }
                lblMsg.Text = "";
                cmbPackages.Text = "";
                cmbPackages.SelectedValue = "";
                cmbPackages.Items.Clear();
                RadTabStrip1.SelectedIndex = 0;
                RadMultiPage1.SelectedIndex = 0;
                rdoIncision.Enabled = false;
                IdPanel.Enabled = false;
                //rdoIncision.Attributes.Remove("onClick");
                
            }
            else
            {
                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMsg.Text = "Select a package to get charges...";
                return;
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
            dtAddedPackage.Dispose();
            dsPackage.Dispose();
            objBilling = null;
        }
    }
    protected void gvAddedSurgery_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        DataTable dtResource = new DataTable();
        try
        {
            if (e.Item is GridDataItem)
            {
                HiddenField hdnIsMainSurgery = (HiddenField)e.Item.FindControl("hdnIsMainSurgery");
                HiddenField hdnServiceType = (HiddenField)e.Item.FindControl("hdnServiceType");
                HiddenField hdnDoctorRequired = (HiddenField)e.Item.FindControl("hdnDoctorRequired");
                RadComboBox ddlResourceName = (RadComboBox)e.Item.FindControl("ddlResourceName");
                HiddenField hdnSurgeonType = (HiddenField)e.Item.FindControl("hdnSurgeonType");
                HiddenField hdnPriceEditable = (HiddenField)e.Item.FindControl("hdnPriceEditable");
                LinkButton lbtnSelect = (LinkButton)e.Item.FindControl("lbtnSelect");
                ImageButton ibtnDelete = (ImageButton)e.Item.FindControl("ibtnDelete");

                TextBox txtServiceCharge = (TextBox)e.Item.FindControl("txtServiceCharge");
                Label lblServiceDiscountPerc = (Label)e.Item.FindControl("lblServiceDiscountPerc");
                Label lblServiceDiscountAmt = (Label)e.Item.FindControl("lblServiceDiscountAmt");
                Label lblNetCharge = (Label)e.Item.FindControl("lblNetCharge");
                Label lblPayableByPatient = (Label)e.Item.FindControl("lblPayableByPatient");
                Label lblPayableByPayer = (Label)e.Item.FindControl("lblPayableByPayer");

                txtServiceCharge.Attributes.Add("onkeyup", "javascript:CalculateChargesOnModifyServiceCharge('" + txtServiceCharge.ClientID + "','" + lblServiceDiscountPerc.ClientID + "','" + lblServiceDiscountAmt.ClientID + "','" + lblNetCharge.ClientID + "','" + lblPayableByPatient.ClientID + "','" + lblPayableByPayer.ClientID + "');");

                if (common.myStr(hdnServiceType.Value).Equals("IPP"))
                {
                    e.Item.Cells[3].ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lbtnSelect.Visible = false;
                    //ibtnDelete.Visible = false;
                }
                if (hdnPriceEditable.Value == "1" || hdnPriceEditable.Value == "True")
                    txtServiceCharge.Enabled = true;
                else
                    txtServiceCharge.Enabled = false;

                if (common.myInt(hdnDoctorRequired.Value) == 1)
                {
                    ddlResourceName.Visible = true;

                    if ((DataSet)ViewState["EmpClassi"] == null)
                    {
                        ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]), 0, 0);
                        ViewState["EmpClassi"] = ds;
                    }
                    else
                        ds = (DataSet)ViewState["EmpClassi"];

                    DataView dvF = new DataView(ds.Tables[0]);
                    if (hdnServiceType.Value != "IPP")
                    {
                        dvF.RowFilter = "Type='" + common.myStr(hdnSurgeonType.Value) + "'";
                    }
                    //else
                    //{
                    //    dvF.RowFilter = "EmployeeTypeCode='" + common.myStr("D") + "'";
                    //}
                    
                    if (dvF.ToTable().Rows.Count > 0)
                    {
                        ddlResourceName.Items.Clear();
                        ddlResourceName.DataSource = dvF.ToTable();
                        ddlResourceName.DataValueField = "EmployeeId";
                        ddlResourceName.DataTextField = "EmployeeName";
                        ddlResourceName.DataBind();
                        RadComboBoxItem lst = new RadComboBoxItem("..Select..", "0");
                        ddlResourceName.Items.Insert(0, lst);
                        ddlResourceName.SelectedIndex = 0;
                    }

                    if (Request.QueryString["OTBookingId"] != null && ViewState["dtResource"] != null)
                    {
                        dtResource = (DataTable)ViewState["dtResource"];
                        if (dtResource.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dtResource);
                            dv.RowFilter = "ResourceType='" + common.myStr(hdnSurgeonType.Value) + "'";
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                ddlResourceName.SelectedIndex = ddlResourceName.Items.IndexOf(ddlResourceName.Items.FindItemByValue(common.myStr(dv.ToTable().Rows[0]["ResourceId"])));
                            }
                        }
                    }
                }
                else
                { ddlResourceName.Visible = false; }

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
            objCommon = null;
            dtResource.Dispose();
        }
    }

    protected void gvAddedSurgery_OnItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (e.CommandName == "Select")
            {
                UpdateDataTable();
                bindDetailGrids(common.myInt(((HiddenField)e.Item.FindControl("hdnPackageId")).Value));
                string sPackageName = common.myStr(((Label)e.Item.FindControl("lblServiceName")).Text);
                RadTabStrip1.SelectedIndex = 1;
                RadMultiPage1.SelectedIndex = 1;
                RadTabStrip1.Tabs[1].Text = "Package Breakup (" + sPackageName + ")";
            }
            else if (e.CommandName == "Delete")
            {
                dt = UpdateDataTable();
                List<DataRow> rowsToRemove = new List<DataRow>();

                //dt = (DataTable)ViewState["GridData"];
                HiddenField hdnPackageId = (HiddenField)e.Item.FindControl("hdnPackageId");
                HiddenField hdnIsMainPackage = (HiddenField)e.Item.FindControl("hdnIsMainPackage");
                int isMainPackage = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    if (common.myInt(dr["IsMainPackage"]) == 1 && common.myInt(dr["Id"]) == common.myInt(common.myStr(e.CommandArgument)))
                    {
                        rowsToRemove.Add(dr);
                        isMainPackage = 1;
                    }
                    else if (isMainPackage == 1)
                    {
                        rowsToRemove.Add(dr);
                    }
                    else if (common.myInt(dr["Id"]) == common.myInt(common.myStr(e.CommandArgument)))
                    {
                        rowsToRemove.Add(dr);
                    }
                }

                foreach (var dr in rowsToRemove)
                {
                    dt.Rows.Remove(dr);
                }
                dt.AcceptChanges();
                gvAddedSurgery.DataSource = dt;
                gvAddedSurgery.DataBind();
                ViewState["GridData"] = dt;
                ViewState["packageCount"] = common.myStr(common.myInt(ViewState["packageCount"]) - 1);
                if (common.myInt(hdnIsMainPackage.Value) == 1)
                {
                    cbMainPackage.Checked = true;
                    cbMainPackage.Enabled = true;
                    ViewState["packageCount"] = "0";
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { dt.Dispose(); }
    }
    /// <summary>
    /// Clear All Controls on Load
    /// </summary>
    protected void clearAll()
    {
        txtEncId.Text = null;
        txtEncNo.Text = null;
        txtRegID.Text = null;
        txtRegNo.Text = null;

        gvAddedSurgery.DataSource = null;
        gvAddedSurgery.DataBind();

        gvPackService.DataSource = null;
        gvPackService.DataBind();
        lblMsg.Text = "";
    }
    /// <summary>
    /// To Fill All Combo Box
    /// </summary>
    protected void FillDefaultData()
    {
        Package BasePack = new Package(sConString);
        try
        {
            ViewState["PackService"] = (DataSet)BasePack.GetPackageServiceLimit(common.myInt(Session["HospitalLocationID"]), 0, 0, common.myInt(Session["FacilityId"]));
            ViewState["PackMedical"] = (DataSet)BasePack.getPackageMedicineLimit(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0, 0, 0, 1);
            ViewState["PackDepart"] = (DataSet)BasePack.GetPackageDepartmentLimit(common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityId"]));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { BasePack = null; }
    }

    private DataTable CreateTable()
    {

        DataTable dtTmp = new DataTable();

        DataColumn dcId = dtTmp.Columns.Add("ID", typeof(Int32));
        DataColumn dcPackageId = dtTmp.Columns.Add("PackageId", typeof(Int32));
        DataColumn dcServiceId = dtTmp.Columns.Add("ServiceId", typeof(Int32));
        DataColumn dcResourceId = dtTmp.Columns.Add("ResourceId", typeof(Int32));
        DataColumn dcDoctorId = dtTmp.Columns.Add("DoctorId", typeof(Int32));
        DataColumn dcServiceType = dtTmp.Columns.Add("ServiceType", typeof(string));
        DataColumn dcMainSurgeryId = dtTmp.Columns.Add("MainSurgeryId", typeof(Int32));
        DataColumn dcIsMainSurgery = dtTmp.Columns.Add("IsMainSurgery", typeof(Int32));
        DataColumn dcIsSurgeryService = dtTmp.Columns.Add("IsSurgeryService", typeof(Int32));
        DataColumn dcSurgeonType = dtTmp.Columns.Add("SurgeonType", typeof(string));
        DataColumn dcDoctorRequired = dtTmp.Columns.Add("DoctorRequired", typeof(Int32));
        DataColumn dcDepartmentTypeId = dtTmp.Columns.Add("DepartmentTypeId", typeof(Int32));
        DataColumn dcServiceName = dtTmp.Columns.Add("ServiceName", typeof(String));
        DataColumn dcResourceType = dtTmp.Columns.Add("ResourceType", typeof(String));
        DataColumn dcServiceActualCharge = dtTmp.Columns.Add("ServiceActualCharge", typeof(decimal));
        DataColumn dcServiceCharge = dtTmp.Columns.Add("ServiceCharge", typeof(decimal));
        DataColumn dcChargePercentage = dtTmp.Columns.Add("ChargePercentage", typeof(decimal));
        DataColumn dcServiceDiscountPerc = dtTmp.Columns.Add("ServiceDiscountPerc", typeof(decimal));
        DataColumn dcServiceDiscountAmt = dtTmp.Columns.Add("ServiceDiscountAmt", typeof(decimal));
        DataColumn dcMaterialLimit = dtTmp.Columns.Add("MaterialLimit", typeof(decimal));
        DataColumn dcNetCharge = dtTmp.Columns.Add("NetCharge", typeof(decimal));
        DataColumn dcPayableByPatient = dtTmp.Columns.Add("PayableByPatient", typeof(decimal));
        DataColumn dcPayableByPayer = dtTmp.Columns.Add("PayableByPayer", typeof(decimal));

        return dtTmp;

    }
    private void bindDetailGrids(Int32 PackageId)
    {
        DataSet dsPackageBreakup = new DataSet();
        BaseC.RestFulAPI objBilling = new BaseC.RestFulAPI(sConString);
        Package BasePack = new Package(sConString);
        try
        {
            if (PackageId != 0)
            {
                dsPackageBreakup = objBilling.GetPackageBreakup(common.myInt(Session["HospitalLocationID"]), PackageId, common.myInt(Request.QueryString["CompanyId"]), common.myInt(Session["FacilityId"]));

                if (dsPackageBreakup.Tables.Count > 0 && dsPackageBreakup.Tables[0].Rows.Count > 0)
                {
                    gvPackageDetails.DataSource = dsPackageBreakup.Tables[1];
                }
                else
                { gvPackageDetails.DataSource = null; }
                gvPackageDetails.DataBind();

                //gvPackageDetails.DataSource = 

                gvPackService.DataSource = (DataSet)BasePack.GetPackageServiceLimit(common.myInt(Session["HospitalLocationID"]), common.myInt(PackageId), common.myInt(Request.QueryString["CompanyId"]), common.myInt(Session["FacilityId"]));
                gvPackService.DataBind();

                gvPackMedicineLimit.DataSource = (DataSet)BasePack.getPackageMedicineLimit(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0, 0, PackageId, 1);
                gvPackMedicineLimit.DataBind();

                gvPackDeptLimit.DataSource = (DataSet)BasePack.GetPackageDepartmentLimit(common.myInt(Session["HospitalLocationID"]), PackageId, common.myInt(Session["FacilityId"]));
                gvPackDeptLimit.DataBind();
            }
            else
            {
                gvPackService.DataSource = null;
                gvPackService.DataBind();

                gvPackMedicineLimit.DataSource = null;
                gvPackMedicineLimit.DataBind();

                gvPackDeptLimit.DataSource = null;
                gvPackDeptLimit.DataBind();
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
            dsPackageBreakup.Dispose();
            objBilling = null;
            BasePack = null;
        }
    }

    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(HttpContext.Current.Request.Url), false);
    }

    private DataTable UpdateDataTable()
    {

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["GridData"];

        foreach (GridDataItem gvrow in gvAddedSurgery.Items)
        {
            HiddenField hdnId = (HiddenField)gvrow.FindControl("hdnId");
            HiddenField hdnPackageId = (HiddenField)gvrow.FindControl("hdnPackageId");
            HiddenField hdnDoctorRequired = (HiddenField)gvrow.FindControl("hdnDoctorRequired");
            RadComboBox ddlResourceName = (RadComboBox)gvrow.FindControl("ddlResourceName");
            TextBox txtServiceCharge = (TextBox)gvrow.FindControl("txtServiceCharge");
            HiddenField hdnServiceType = (HiddenField)gvrow.FindControl("hdnServiceType");

            dt.DefaultView.RowFilter = "Id = " + common.myInt(hdnId.Value);
            if (dt.DefaultView.Count > 0)
            {
                dt.DefaultView[0]["ServiceCharge"] = common.myStr(common.myDec(txtServiceCharge.Text));
                if (common.myInt(hdnDoctorRequired.Value) == 1)
                {
                    if (common.myInt(ddlResourceName.SelectedValue).Equals(0) && common.myStr(hdnServiceType.Value).Equals("IPP"))
                    {
                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMsg.Text = "Doctor name not define order";
                        dt = null;
                        return dt;
                    }
                    dt.DefaultView[0]["DoctorId"] = common.myInt(ddlResourceName.SelectedValue);
                }
                else if (common.myStr(hdnServiceType.Value) == "S")
                {
                    if (common.myStr(ddlResourceName.SelectedValue) == "" && common.myInt(hdnDoctorRequired.Value) == 1)  // Added By Akshay && common.myInt(hdnDoctorRequired.Value) == 1    and remove   common.myStr(ddlResourceName.SelectedValue) == "0"
                    {
                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMsg.Text = "Surgeon name not define in surgory order";
                        dt = null;
                        return dt;
                    }
                }
                dt.AcceptChanges();
                dt.DefaultView.RowFilter = "";
                dt.AcceptChanges();
            }
        }
        ViewState["GridData"] = dt;
        return dt;

    }
    protected void dtOrderDate_OnSelectedDateChanged(object sender, EventArgs e)
    {
        DateTime dtCurrentDate = DateTime.Now;
        DateTime dtSelectedDate = Convert.ToDateTime(rdtpOtStartTime.SelectedDate);
        lblInfoBillCategory.Text = "";
        if (dtSelectedDate < dtCurrentDate)
        {
            lblInfoBillCategory.Text = "<br/>(Billing category applicable as per the order date)";
        }
    }
}
