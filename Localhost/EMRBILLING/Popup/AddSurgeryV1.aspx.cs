using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMRBILLING_Popup_AddSurgeryV1 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //clsExceptionLog objException = new clsExceptionLog();
    //DataSet ds;
    //Hashtable hshIn;
    //Hashtable hshOut;
    //BaseC.EMRBilling.clsOrderNBill BaseBill;
    //BaseC.clsLISMaster objLISMaster;
    private const int ItemsPerRequest = 50;
    private static string noofsurgeryflag = null, DefaultOPDCategoryService = "", DecimalPlaces = "2", Ishiddingnoofsurgeryforallclients = "N",
        IsGenerateAdvanceAgainstOrder = "N", isHighRiskSurgeryRequired = "N", HighRiskFlagDisplayName = "High Risk", IsAllowToEditSurgeryAmountOnlyForCash = "N",
        setisAllDoctorDisplayOnAddService = "N", IsSurgeryChargesForNanavati = "N", DefaultAnServiceId = "0", isShowOTRoomSelectedOnAddSurgery = "N",
        DefaultHospitalCompany = "", EnableSurgeryBedCategoryForIP = "Y",
        isAnesthetistCalculateAfterAddSurgeonAndAssisSur = "N", isRecalculateOTChargeOnIPSurgeryFrontEnd = "Y", isAssistantSurgeonCalculateonAmount = "N", IsAllowAnesthesiaTypeWisePerc = "N",
        isAllDoctorDisplayOnAddService = "N", isShowEmergencyCheckBoxForEmergencyCharge = "N";
    //DAL.DAL dl;
    protected void Page_Load(object sender, EventArgs e)
    {
        BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        try
        {
            if (!IsPostBack)
            {
                getHospitalFlagValue();
                hdnisAnesthetistCalculateAfterAddSurgeonAndAssistantSurgeon.Value = isAnesthetistCalculateAfterAddSurgeonAndAssisSur;
                hdnisCalculateOT.Value = isRecalculateOTChargeOnIPSurgeryFrontEnd;
                hdnisAssistantSurgeonCalculateonAmount.Value = isAssistantSurgeonCalculateonAmount;
                BaseC.Security objSecurityUser = new BaseC.Security(sConString);
                var valid = objSecurityUser.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowBackDateOrder");
                if (valid)
                {
                    dtOrderDate.DateInput.Enabled = true;
                    dtOrderDate.DatePopupButton.Visible = true;
                }
                else
                {
                    dtOrderDate.DateInput.Enabled = false;
                    dtOrderDate.DatePopupButton.Visible = false;
                    dtOrderDate.TimePopupButton.Visible = false;
                }
                objSecurityUser = null;

                hdnDefaultCompanyId.Value = DefaultHospitalCompany;

                hdnDecimalPlaces.Value = common.myStr(DecimalPlaces);
                dtOrderDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtOrderDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtOrderDate.SelectedDate = common.myDate(DateTime.Now);
                setControlHospitalbased();
                BindMinutesor();
                BindSurgeryDepartment();
                BindMinutes();
                BindAnaesthesia();
                BindOtherResource();
                BindRoom();
                FillAnesthesiaType();
                if (common.myStr(Ishiddingnoofsurgeryforallclients).Equals("Y"))
                {
                    Label1.Visible = true;
                    radCmbsurgeryClassification.Visible = true;
                }
                BindBedCategory();
                BindOTEquipment();
                chkIsGenerateAdvance.Visible = false;
                if ((Request.QueryString["OP_IP"] != null && common.myStr(Request.QueryString["OP_IP"]).Equals("O")) && common.myStr(Request.QueryString["OTBookingId"]).Equals(""))
                {
                    btnProceed.Text = "Proceed";
                    btnProceed.ToolTip = "Click here to proceed to OP Bill Page with Surgery Data...";
                    radCmbBedCategory.SelectedValue = DefaultOPDCategoryService;
                    radCmbBedCategory.Enabled = false;
                }
                else
                {
                    if (common.myStr(IsGenerateAdvanceAgainstOrder).Equals("Y"))
                        chkIsGenerateAdvance.Visible = true;
                    btnProceed.Text = "Save";
                    btnProceed.ToolTip = "Click here to save Surgery Data...";
                }

                Session["SurSvchk"] = 0;
                if (Request.QueryString["RegNo"] != null && !common.myStr(Request.QueryString["RegNo"]).Equals(""))
                {
                    int RegId = common.myInt(Request.QueryString["RegId"]);
                    int EncId = common.myInt(Request.QueryString["EncId"]);
                    BindPatientHiddenDetails(common.myStr(Request.QueryString["RegNo"]), RegId, EncId);
                }

                DateTime OTCheckinTime = DateTime.Now;
                DateTime OTCheckoutTime = DateTime.Now;

                string OTChkInTimeStr = string.Empty;
                string OTChkOutTimeStr = string.Empty;

                hdnPayerType.Value = common.myStr(Request.QueryString["PayerType"]);
                if (Request.QueryString["OTBookingId"] != null && !common.myStr(Request.QueryString["OTBookingId"]).Equals(""))
                {
                    ViewState["DatefromOt"] = 0;
                    DataSet ds1 = objOT.GetCheckITOT(common.myStr(Request.QueryString["OTBookingId"]), common.myInt(Session["FacilityId"]));
                    if (ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            ViewState["DatefromOt"] = 1;
                            if (!common.myStr(ds1.Tables[0].Rows[0]["OTCheckintime"]).Equals(""))
                            {
                                OTCheckinTime = Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckintime"]);
                            }
                            if (!common.myStr(ds1.Tables[0].Rows[0]["OTCheckouttime"]).Equals(""))
                            {
                                OTCheckoutTime = Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckouttime"]);
                            }

                            if (!common.myStr(ds1.Tables[0].Rows[0]["OTChkInTimeString"]).Equals(""))
                            {
                                OTChkInTimeStr = common.myStr(ds1.Tables[0].Rows[0]["OTChkInTimeString"]);
                            }
                            if (!common.myStr(ds1.Tables[0].Rows[0]["OTChkOutTimeString"]).Equals(""))
                            {
                                OTChkOutTimeStr = common.myStr(ds1.Tables[0].Rows[0]["OTChkOutTimeString"]);
                            }
                        }
                    }
                    ds1.Dispose();
                }
                ViewState["OTCheckintime"] = OTCheckinTime;
                ViewState["OTCheckouttime"] = OTCheckoutTime;

                ViewState["OTChkInTimeStr"] = OTChkInTimeStr;
                ViewState["OTChkOutTimeStr"] = OTChkOutTimeStr;

                rdtpOtStartTime.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpOtStartTime.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpOtStartTime.SelectedDate = common.myDate(ViewState["OTCheckintime"]);
                //lblTimeChkIn.Text ="("+ (common.myDate(ViewState["OTCheckintime"])).ToString("dd/MM/yyyy HH:mm tt") + ")";
                lblTimeChkIn.Text = "(" + common.myStr(ViewState["OTChkInTimeStr"]) + ")";
                rdtpOtEndTime.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpOtEndTime.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpOtEndTime.SelectedDate = common.myDate(ViewState["OTCheckouttime"]);

                lblTimeChkOut.Text = "(" + common.myStr(ViewState["OTChkOutTimeStr"]) + ")";

                rdtpAstartTime.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpAstartTime.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpAstartTime.SelectedDate = common.myDate(DateTime.Now);

                rdtpAEndTime.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpAEndTime.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                rdtpAEndTime.SelectedDate = common.myDate(DateTime.Now);

                gvSurgery.Visible = false;
                btnUp.Visible = false;
                btnDown.Visible = false;

                if (!common.myStr(Request.QueryString["OTBookingId"]).Equals(""))
                {
                    chkUnClean.Visible = true;
                    BindOTSurgeryDetails(common.myInt(Request.QueryString["OTBookingId"]));
                }
                if (!common.myStr(Request.QueryString["OP_IP"]).Equals("") && common.myStr(Request.QueryString["OP_IP"]).Equals("O"))
                {
                    dtOrderDate.Enabled = false;
                    ddlOrderMinutes.Enabled = false;
                }
                else
                {
                    dtOrderDate.Enabled = true;
                    ddlOrderMinutes.Enabled = true;
                }

                chkIsHighRiskSurgery.Visible = true;
                if (common.myStr(isHighRiskSurgeryRequired).Equals("N"))
                {
                    chkIsHighRiskSurgery.Visible = false;
                    chkIsHighRiskState.Visible = false;
                }
                else
                {
                    chkIsHighRiskSurgery.Text = HighRiskFlagDisplayName;
                    chkIsHighRiskState.Text = HighRiskFlagDisplayName + " Stat";
                }

                if (!string.IsNullOrEmpty(common.myStr(Request.QueryString["ServiceID"])))
                {
                    BindPackageSurgeryFlagBased(common.myStr(Request.QueryString["ServiceID"]));
                }

                hdnIsAllowToEditSurgeryAmountOnlyForCash.Value = IsAllowToEditSurgeryAmountOnlyForCash;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objOT = null; objBill = null; BaseBill = null; }
    }

    private void BindOTSurgeryDetails(int OTBookingId)
    {
        BaseC.clsOTBooking objOTBooking = new BaseC.clsOTBooking(sConString);
        DataSet ds = new DataSet();
        try
        {
            string OTBookingNo = "";
            ds = objOTBooking.getOTBookingDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), OTBookingId, OTBookingNo);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime dtFromDate, FromTime, ToTime, dtFromDateTime, dtToDateTime;
                DataRow dr;

                int i = 0;
                bool ismain = false;

                if (!common.myStr(ViewState["IsSurgeryChargesForNanavati"]).Equals(""))
                {

                    if (common.myStr(ds.Tables[0].Rows[0]["IsEmergency"]) == "True")
                    {
                        chkIsEmergency.Checked = true;
                    }
                }

                foreach (DataRow drService in ds.Tables[0].Rows)
                {
                    if (i == 0)
                        ismain = true;
                    else
                        ismain = false;

                    radCmbDepartment.SelectedValue = common.myStr(drService["DepartmentID"]);
                    radCmbDepartment_OnSelectedIndexChanged(this, null);
                    radCmbSubDepartment.SelectedValue = common.myStr(drService["SubDepartmentId"]);
                    radCmbSubDepartment_OnSelectedIndexChanged(this, null);
                    radCmbSurgeryServices.SelectedValue = common.myStr(drService["ServiceID"]);
                    radCmbSurgeryServices.Text = common.myStr(drService["ServiceName"]);
                    cbMainSurgery.Checked = ismain;
                    ViewState["IncisionTime"] = common.myDate(drService["IncisionTime"]);
                    btnAddServicetoGrid_OnClick(this, null);
                    i++;
                    //Session["OTChargeable"] = common.myStr(drService["IsUnplannedOTChargeable"]);
                }

                dr = ds.Tables[0].Rows[0];
                radCmbOtRoom.SelectedValue = common.myStr(dr["TheaterId"]);
                radCmbAnaesthesia.SelectedValue = common.myStr(dr["AnesthesiaId"]);
                dtFromDate = Convert.ToDateTime(dr["OTBookingDate"]);
                FromTime = common.myDate(common.myStr(dr["FromTime"]));
                ToTime = common.myDate(common.myStr(dr["ToTime"]));
                dtFromDateTime = common.myDate(dtFromDate.ToShortDateString() + " " + FromTime.TimeOfDay);
                dtToDateTime = common.myDate(dtFromDate.ToShortDateString() + " " + ToTime.TimeOfDay);
                rdtpOtStartTime.SelectedDate = common.myDate(ViewState["OTCheckintime"]);
                rdtpOtEndTime.SelectedDate = common.myDate(ViewState["OTCheckouttime"]);

                //lblTimeChkIn.Text ="("+ (common.myDate(ViewState["OTCheckintime"])).ToString("dd/MM/yyyy HH:mm tt") + ")";
                //lblTimeChkOut.Text = "("+(common.myDate(ViewState["OTCheckouttime"])).ToString("dd/MM/yyyy HH:mm tt") + ")";

                lblTimeChkIn.Text = "(" + common.myStr(ViewState["OTChkInTimeStr"]) + ")";
                lblTimeChkOut.Text = "(" + common.myStr(ViewState["OTChkOutTimeStr"]) + ")";

                rdtpAstartTime.SelectedDate = common.myDate(dtFromDateTime.ToString());
                rdtpAEndTime.SelectedDate = common.myDate(dtToDateTime.ToString());
                radCmbSurgeryServices.SelectedValue = common.myStr(dr["ServiceID"]);
                radCmbSurgeryServices.Text = common.myStr(dr["ServiceName"]);
                Session["OTChargeable"] = common.myStr(dr["IsUnplannedOTChargeable"]);
            }


            if (ds.Tables[1].Rows.Count > 0)
            {
                ViewState["dtResource"] = ds.Tables[1];
                ds.Tables[1].Columns[0].ColumnName = "ID";
                ds.Tables[1].AcceptChanges();
                DataView dv = new DataView(ds.Tables[1]);
                dv.RowFilter = "ResourceType IN ('AS')";
                DataTable dtNew = new DataTable();
                dtNew = dv.ToTable();
                List<DataRow> rowsToRemove = new List<DataRow>();
                string strRes = "";
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    if (!common.myStr(dtNew.Rows[i]["ResourceType"]).Equals(strRes))
                    {
                        strRes = common.myStr(dtNew.Rows[i]["ResourceType"]);
                    }
                    else
                    {
                        rowsToRemove.Add(dtNew.Rows[i]);
                    }
                }
                foreach (var dr in rowsToRemove)
                {
                    dtNew.Rows.Remove(dr);
                }
                dtNew.AcceptChanges();

                gvResourceList.DataSource = dtNew;// dtResource.DefaultView;
                gvResourceList.DataBind();
                BtnCalculateCharges_OnClick(this, null);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objOTBooking = null; ds.Dispose(); }
    }

    public void setControlHospitalbased()
    {
        ViewState["IsSurgeryChargesForNanavati"] = IsSurgeryChargesForNanavati;
        IsSurgeryChargesForNanavati = "";
        ViewState["IsSurgeryChargesForNanavati"] = "";
        if (IsSurgeryChargesForNanavati != "")
        {
            chkIsEmergency.Visible = true;
            tdResource.Visible = false;
            tdResource1.Visible = false;
        }
        else
        {
            chkIsEmergency.Visible = false;
            // gvSurgery.MasterTableView.GetColumn("SurgeryComponent").Display = false;
            //gvSurgery.MasterTableView.GetColumn("DoctorShare").Display = false;
        }
        chkIsEmergency.Visible = isShowEmergencyCheckBoxForEmergencyCharge.Equals("Y");
    }

    private void BindSurgeryDepartment()
    {
        BaseC.EMRMasters objSurgery = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            radCmbDepartment.Items.Clear();
            //ds = objSurgery.GetHospitalDepartment(common.myInt(Session["HospitalLocationID"].ToString()), "'S'");
            ds = objSurgery.GetHospitalDepartment(common.myInt(Session["HospitalLocationID"].ToString()), common.myStr("'S'"));
            if (ds.Tables[0].Rows.Count > 0)
            {
                radCmbDepartment.DataSource = ds.Tables[0];
                radCmbDepartment.DataTextField = "DepartmentName";
                radCmbDepartment.DataValueField = "DepartmentId";
                radCmbDepartment.DataBind();

                radCmbDepartment.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
                radCmbDepartment.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objSurgery = null; ds.Dispose(); }
    }
    private void BindSurgerySubDepartment()
    {
        BaseC.EMRMasters objSurgery = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objSurgery.GetHospitalSubDepartment(common.myInt(Session["HospitalLocationID"].ToString()), common.myInt(radCmbDepartment.SelectedValue), common.myStr("'S'"), 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                radCmbSubDepartment.Items.Clear();
                radCmbSubDepartment.DataSource = ds.Tables[0];
                radCmbSubDepartment.DataTextField = "SubName";
                radCmbSubDepartment.DataValueField = "SubDeptId";
                radCmbSubDepartment.DataBind();

                radCmbSubDepartment.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
                radCmbSubDepartment.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objSurgery = null; ds.Dispose(); }
    }
    private void BindMinutes()
    {
        try
        {
            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                {

                    radCmbOtStartTimeM.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                    radCmbOtEndTimeM.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                    radCmbAEndTimeM.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                    radCmbAstartTimeM.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                    //ddlSurgeryMinutes.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    radCmbOtStartTimeM.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                    radCmbOtEndTimeM.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                    radCmbAEndTimeM.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                    radCmbAstartTimeM.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                    //ddlSurgeryMinutes.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
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
    private void BindAnaesthesia()
    {
        //BaseC.EMRMasters objSurgery = new BaseC.EMRMasters(sConString);
        BaseC.RestFulAPI objSurgery = new BaseC.RestFulAPI(sConString);
        DataSet ds = new DataSet();
        try
        {
            //ds = objSurgery.GetService((common.myInt(Session["HospitalLocationID"].ToString())), "", "'AN','SR'", common.myInt(Session["FacilityId"]));
            ds = objSurgery.GetHospitalServices(common.myInt(Session["HospitalLocationID"]), 0, 0, "'AN','SR','OT','O'", "", common.myInt(Session["FacilityId"]), "N", 0, "1");

            if (ds.Tables[0].Rows.Count > 0)
            {
                radCmbAnaesthesia.DataSource = ds.Tables[0];
                radCmbAnaesthesia.DataTextField = "ServiceName";
                radCmbAnaesthesia.DataValueField = "ServiceID";
                radCmbAnaesthesia.DataBind();

                //radCmbAnaesthesia.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
                //radCmbAnaesthesia.SelectedIndex = 0;
            }
            if (common.myInt(DefaultAnServiceId) > 0)
            {
                RadComboBoxItem cbox = (RadComboBoxItem)radCmbAnaesthesia.FindItemByValue(DefaultAnServiceId);
                if (cbox != null)
                    cbox.Checked = true;
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objSurgery = null; ds.Dispose(); }
    }
    private void BindRoom()
    {
        BaseC.Surgery objSurgery = new BaseC.Surgery(sConString);
        DataSet ds = new DataSet();
        try
        {
            radCmbOtRoom.Items.Clear();
            ds = objSurgery.GetOTRoom(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                radCmbOtRoom.DataSource = ds.Tables[0];
                radCmbOtRoom.DataTextField = "TheatreName";
                radCmbOtRoom.DataValueField = "TheatreID";
                radCmbOtRoom.DataBind();

                if (common.myStr(isShowOTRoomSelectedOnAddSurgery).Equals("N"))
                {
                    radCmbOtRoom.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
                }
                radCmbOtRoom.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objSurgery = null; ds.Dispose(); }
    }
    private void BindOtherResource()
    {
        BaseC.Surgery objDoctorClassification = new BaseC.Surgery(sConString);
        DataSet ds = new DataSet();
        try
        {
            // ds = objDoctorClassification.GetDoctorClassification((common.myInt(Session["HospitalLocationID"])));
            ds = objDoctorClassification.GetResourcecharges(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["CompanyId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Type IN ('SR','AN','AS','OD')";
                foreach (DataRow dr in dv.ToTable().Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dr["Name1"];
                    item.Value = dr["ID"].ToString();
                    item.Attributes.Add("Type", dr["Type"].ToString());
                    this.radCmbDoctorClassification.Items.Add(item);
                    item.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objDoctorClassification = null; ds.Dispose(); }
    }
    private void BindBedCategory()
    {
        BaseC.EMRMasters objSurgery = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objSurgery.GetService((common.myInt(Session["HospitalLocationID"])), "0", "'R'", common.myInt(Session["FacilityId"]));

            radCmbBedCategory.Items.Clear();

            if (ds.Tables[0].Rows.Count > 0)
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
        finally { objSurgery = null; ds.Dispose(); }
    }
    private void BindOTEquipment()
    {
        BaseC.EMRMasters objSurgery = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objSurgery.GetService((common.myInt(Session["HospitalLocationID"].ToString())), "0", "'EQ'", common.myInt(Session["FacilityId"]));//Equipments
            ddlOTEquipments.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlOTEquipments.DataSource = ds.Tables[0];
                ddlOTEquipments.DataTextField = "ServiceName";
                ddlOTEquipments.DataValueField = "ServiceID";
                ddlOTEquipments.DataBind();

                ddlOTEquipments.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
                ddlOTEquipments.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { objSurgery = null; ds.Dispose(); }
    }
    void BindPatientHiddenDetails(String RegistrationNo, Int32 RegistrationId, Int32 EncounterId)
    {
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);
        BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
        DataSet ds = new DataSet();
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        try
        {
            if (!RegistrationNo.Equals(""))
            {
                if (!common.myStr(Request.QueryString["OP_IP"]).Equals("") && common.myStr(Request.QueryString["OP_IP"]).Equals("O"))
                {
                    ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), RegistrationId, common.myInt(RegistrationNo), EncounterId, common.myInt(Session["UserId"]));
                    lblInfoEncNo.Visible = false;
                    lblInfoAdmissionDt.Visible = false;
                    lblEncounterNo.Visible = false;
                    lblAdmissionDate.Visible = false;
                }
                else
                {
                    ds = objIPBill.GetPatientDetailsIP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), RegistrationId, common.myInt(RegistrationNo), common.myInt(Session["UserId"]), 0, "");
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        hdnPayerType.Value = common.myStr(dr["EncounterCompanyType"]);
                        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                        lblDob.Text = common.myStr(dr["DOB"]);
                        lblMobile.Text = common.myStr(dr["MobileNo"]);

                        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                        lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                        lblUHID.Text = common.myStr(dr["RegistrationNo"]);//Pra
                        lblPayerCompany.Text = common.myStr(dr["Payername"]);//Pra
                        radCmbBedCategory.Enabled = false;
                        if (!common.myStr(Request.QueryString["OP_IP"]).Equals("") && common.myStr(Request.QueryString["OP_IP"]).Equals("I"))
                        {
                            radCmbBedCategory.SelectedIndex = radCmbBedCategory.Items.IndexOf(radCmbBedCategory.Items.FindItemByValue(common.myStr(dr["CurrentBillCategory"])));
                            if (EnableSurgeryBedCategoryForIP.Equals("Y"))
                                radCmbBedCategory.Enabled = true;
                        }
                        else if (!common.myStr(Request.QueryString["OP_IP"]).Equals("") && common.myStr(Request.QueryString["OP_IP"]).Equals("O"))
                        {
                            lblEncounterNo.Visible = false;
                            lblAdmissionDate.Visible = false;

                            radCmbBedCategory.SelectedIndex = radCmbBedCategory.Items.IndexOf(radCmbBedCategory.Items.FindItemByValue(DefaultOPDCategoryService));
                        }
                        else
                        {
                            if (EnableSurgeryBedCategoryForIP.Equals("Y"))
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
        finally { bParse = null; bC = null; objLISMaster = null; objIPBill = null; objCommon = null; ds.Dispose(); }
    }
    protected void radCmbDepartment_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            BindSurgerySubDepartment();

            radCmbSurgeryServices.Items.Clear();
            radCmbSurgeryServices.Text = "";
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void radCmbSubDepartment_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        if (radCmbSubDepartment.SelectedIndex == -1)
            return;
        else
        {
            radCmbSurgeryServices.Items.Clear();
            radCmbSurgeryServices.Text = "";
        }
    }

    protected void radCmbOtStartTimeM_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(rdtpOtStartTime.SelectedDate.Value.ToString());
        sb.Remove(rdtpOtStartTime.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(rdtpOtStartTime.SelectedDate.Value.ToString().IndexOf(":") + 1, radCmbOtStartTimeM.Text);
        rdtpOtStartTime.SelectedDate = Convert.ToDateTime(sb.ToString());
    }
    protected void radCmbOtEndTimeM_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(rdtpOtEndTime.SelectedDate.Value.ToString());
        sb.Remove(rdtpOtEndTime.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(rdtpOtEndTime.SelectedDate.Value.ToString().IndexOf(":") + 1, radCmbOtEndTimeM.Text);
        rdtpOtEndTime.SelectedDate = Convert.ToDateTime(sb.ToString());
    }
    protected void radCmbAEndTimeM_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(rdtpAEndTime.SelectedDate.Value.ToString());
        sb.Remove(rdtpAEndTime.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(rdtpAEndTime.SelectedDate.Value.ToString().IndexOf(":") + 1, radCmbAEndTimeM.Text);
        rdtpAEndTime.SelectedDate = Convert.ToDateTime(sb.ToString());
    }
    protected void radCmbAstartTime_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(rdtpAstartTime.SelectedDate.Value.ToString());
        sb.Remove(rdtpAstartTime.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(rdtpAstartTime.SelectedDate.Value.ToString().IndexOf(":") + 1, radCmbAstartTimeM.Text);
        rdtpAstartTime.SelectedDate = Convert.ToDateTime(sb.ToString());
    }

    protected void ddlService_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            RadComboBox ddl = sender as RadComboBox;
            GridViewRow row = ddl.NamingContainer as GridViewRow;

            DataTable data = GetData(e.Text);

            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                ddl.Items.Add(new RadComboBoxItem(common.myStr(data.Rows[i]["ServiceName"]), common.myStr(data.Rows[i]["ServiceId"])));
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
            data.Dispose();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    void BindPackageSurgeryFlagBased(string ServiceId)
    {
        DataTable data = new DataTable();
        try
        {

            radCmbSurgeryServices.Items.Clear();
            radCmbSurgeryServices.Text = "";
            data = GetData(ServiceId.ToString());

            foreach (DataRowView dr in data.DefaultView)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(dr["ServiceName"]) + " " + common.myStr(dr["RefServiceCode"]) + " " + common.myStr(dr["CPTCode"]);
                item.Value = dr["ServiceId"].ToString();
                this.radCmbSurgeryServices.Items.Add(item);
                item.DataBind();
            }

            radCmbSurgeryServices.SelectedIndex = radCmbSurgeryServices.Items.IndexOf(radCmbSurgeryServices.FindItemByValue(ServiceId));
            radCmbSurgeryServices.Enabled = false;
            radCmbDepartment.Enabled = false;
            radCmbSubDepartment.Enabled = false;
            radCmbSurgeryServices.ForeColor = System.Drawing.Color.Black;
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
        DataTable data = new DataTable();
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        DataSet ds = new DataSet();
        try
        {
            string ServiceName = "%" + text + "%";
            string strDepartmentType = "'S'";
            ds = objCommon.GetHospitalServices(common.myInt(Session["HospitalLocationId"]), common.myInt(radCmbDepartment.SelectedValue), common.myInt(radCmbSubDepartment.SelectedValue), strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Columns.Contains("ApplicableFor"))
                {
                    DataView dvF = new DataView(ds.Tables[0]);
                    dvF.RowFilter = "ApplicableFor IN ('" + common.myStr(Request.QueryString["OP_IP"]) + "','B')";
                    dvF = dvF.ToTable().DefaultView;
                    data = dvF.ToTable();
                    dvF.RowFilter = string.Empty;
                }
                else
                { data = ds.Tables[0]; }
            }
            objCommon = null; ds.Dispose();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        return data;

    }
    protected void btnAddOtherSuregon_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (radCmbDoctorClassification.SelectedItem.Value == "0")
            {
                Alert.ShowAjaxMsg("Please Select Other Resource...", this.Page);
                return;
            }

            gvResourceList.DataSource = GetOtherResourceList(true);
            gvResourceList.DataBind();
            lblMsg.Text = "";
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    private DataTable GetOtherResourceList(Boolean bBlankRow)
    {
        DataTable dtResourceList = new DataTable();
        try
        {
            DataColumn auto = new DataColumn("ID", typeof(System.Int32));
            dtResourceList.Columns.Add(auto);
            // specify it as auto increment field
            auto.AutoIncrement = true;
            auto.AutoIncrementSeed = 1;
            auto.ReadOnly = true;
            int flage = 0;

            dtResourceList.Columns.Add("ResourceID");
            dtResourceList.Columns.Add("ResourceName");
            dtResourceList.Columns.Add("ResourceType");
            dtResourceList.Columns.Add("ProviderID");
            dtResourceList.Columns.Add("SurgeryNo");


            if (common.myStr(Ishiddingnoofsurgeryforallclients).Equals("Y"))
            {
                if (gvResourceList.Rows.Count > 0)
                {
                    foreach (GridViewRow gRow in gvResourceList.Rows)
                    {
                        DataRow drResource = dtResourceList.NewRow();
                        drResource["ResourceID"] = ((HiddenField)gRow.FindControl("hdnResourceID")).Value;
                        drResource["ResourceType"] = ((HiddenField)gRow.FindControl("hdnResourceType")).Value;
                        drResource["ResourceName"] = ((Label)gRow.FindControl("lblResourceName")).Text;
                        drResource["SurgeryNo"] = ((HiddenField)gRow.FindControl("hdnSurgeryNo")).Value;
                        string surgeryno = ((HiddenField)gRow.FindControl("hdnSurgeryNo")).Value;
                        dtResourceList.Rows.Add(drResource);
                        if (flage == 0)
                        {
                            if (common.myInt(((HiddenField)gRow.FindControl("hdnResourceID")).Value) == common.myInt(radCmbDoctorClassification.SelectedItem.Value) && surgeryno == common.myStr(radCmbsurgeryClassification.SelectedItem.Value))
                            {
                                flage = 1;
                            }
                        }
                    }

                    if (bBlankRow == true && flage == 0)
                    {
                        DataRow drResource = dtResourceList.NewRow();
                        drResource["ResourceID"] = radCmbDoctorClassification.SelectedItem.Value;
                        drResource["ResourceType"] = radCmbDoctorClassification.SelectedItem.Attributes["Type"].ToString();
                        drResource["ResourceName"] = radCmbsurgeryClassification.SelectedItem.Text + "  -  " + radCmbDoctorClassification.SelectedItem.Text;
                        drResource["SurgeryNo"] = radCmbsurgeryClassification.SelectedValue;
                        drResource["ProviderID"] = 0;
                        dtResourceList.Rows.Add(drResource);
                    }
                }
                else
                {
                    DataRow drResource = dtResourceList.NewRow();
                    drResource["ResourceID"] = radCmbDoctorClassification.SelectedItem.Value;
                    drResource["ResourceType"] = radCmbDoctorClassification.SelectedItem.Attributes["Type"].ToString();
                    drResource["ResourceName"] = radCmbsurgeryClassification.SelectedItem.Text + "  -  " + radCmbDoctorClassification.SelectedItem.Text;
                    drResource["SurgeryNo"] = radCmbsurgeryClassification.SelectedValue;
                    drResource["ProviderID"] = 0;
                    dtResourceList.Rows.Add(drResource);
                }
                ViewState["DuplicateResourceID"] = flage;
            }

            else
            {
                if (gvResourceList.Rows.Count > 0)
                {
                    foreach (GridViewRow gRow in gvResourceList.Rows)
                    {
                        DataRow drResource = dtResourceList.NewRow();
                        drResource["ResourceID"] = ((HiddenField)gRow.FindControl("hdnResourceID")).Value;
                        drResource["ResourceType"] = ((HiddenField)gRow.FindControl("hdnResourceType")).Value;
                        drResource["ResourceName"] = ((Label)gRow.FindControl("lblResourceName")).Text;
                        drResource["SurgeryNo"] = 0;

                        dtResourceList.Rows.Add(drResource);
                        if (flage == 0)
                        {
                            if (common.myInt(((HiddenField)gRow.FindControl("hdnResourceID")).Value) == common.myInt(radCmbDoctorClassification.SelectedItem.Value))
                            {
                                flage = 1;
                            }
                        }
                    }

                    if (bBlankRow == true && flage == 0)
                    {
                        DataRow drResource = dtResourceList.NewRow();
                        drResource["ResourceID"] = radCmbDoctorClassification.SelectedItem.Value;
                        drResource["ResourceType"] = radCmbDoctorClassification.SelectedItem.Attributes["Type"].ToString();
                        drResource["ResourceName"] = radCmbDoctorClassification.SelectedItem.Text;
                        drResource["SurgeryNo"] = 0;
                        drResource["ProviderID"] = 0;
                        dtResourceList.Rows.Add(drResource);
                    }
                }
                else
                {
                    DataRow drResource = dtResourceList.NewRow();
                    drResource["ResourceID"] = radCmbDoctorClassification.SelectedItem.Value;
                    drResource["ResourceType"] = radCmbDoctorClassification.SelectedItem.Attributes["Type"].ToString();
                    drResource["ResourceName"] = radCmbDoctorClassification.SelectedItem.Text;
                    drResource["SurgeryNo"] = 0;
                    drResource["ProviderID"] = 0;
                    dtResourceList.Rows.Add(drResource);
                }
                ViewState["DuplicateResourceID"] = flage;
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        return dtResourceList;
    }
    protected void gvResourceList_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void gvResourceList_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Deleted")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int rIndex = row.RowIndex;

                DataTable dtResource = new DataTable();
                dtResource = GetOtherResourceList(false);
                dtResource.Rows[Convert.ToInt32(rIndex)].Delete();
                dtResource.AcceptChanges();

                gvResourceList.DataSource = dtResource;
                gvResourceList.DataBind();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void btnAddServicetoGrid_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(radCmbSurgeryServices.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Surgery already added !", Page);
                radCmbSurgeryServices.Text = "";
                radCmbSurgeryServices.Focus();
                return;
            }
            DataTable dt = new DataTable();
            if (ViewState["ServiceDetail"] == null)
            {
                dt = CreateTableService();
            }
            else
            {
                dt = (DataTable)ViewState["ServiceDetail"];
            }
            DataRow dr = dt.NewRow();
            string IsSurgeryMain = "";
            //string ServiceId = radCmbSurgeryServices.SelectedValue;
            if (dt.Rows.Count > 0)
            {
                DataRow[] drDuplicateServiceId = dt.Select("ServiceId=" + common.myInt(radCmbSurgeryServices.SelectedValue));
                if (drDuplicateServiceId.Length > 0)
                {
                    Alert.ShowAjaxMsg("Surgery already added !", Page);
                    radCmbSurgeryServices.ClearSelection();
                    radCmbSurgeryServices.Text = string.Empty;
                    radCmbSurgeryServices.Focus();
                    return;
                }

                dr["SNo"] = common.myInt(dt.Rows.Count + 1);
                DataRow[] drMainSurgery = dt.Select("IsSurgeryMain='Yes'");
                if (drMainSurgery.Length > 0)
                    dr["IsSurgeryMain"] = common.myStr("");
                else
                    dr["IsSurgeryMain"] = IsSurgeryMain; // False;
                cbMainSurgery.Checked = false;
            }
            else
            {
                if (common.myBool(cbMainSurgery.Checked) == false)
                {
                    Alert.ShowAjaxMsg("First Surgery should always be Main Surgery !", Page);
                    cbMainSurgery.Focus();
                    return;
                }
                else
                {
                    IsSurgeryMain = "Yes";
                    dr["SNo"] = common.myInt(1);
                    dr["IsSurgeryMain"] = IsSurgeryMain;
                }
            }

            dr["ServiceId"] = common.myInt(radCmbSurgeryServices.SelectedValue);
            dr["ServiceName"] = common.myStr(radCmbSurgeryServices.Text);
            dr["IncisionTime"] = ViewState["IncisionTime"];
            dt.Rows.Add(dr);

            dt.AcceptChanges();
            ViewState["ServiceDetail"] = dt;
            gvSurgery.DataSource = dt;
            gvSurgery.DataBind();
            gvSurgery.Visible = true;
            if (dt.Rows.Count > 1)
            {
                btnUp.Visible = true;
                btnDown.Visible = true;
            }
            radCmbSurgeryServices.ClearSelection();
            radCmbSurgeryServices.Text = string.Empty;
            radCmbSurgeryServices.Focus();
            lblMsg.Text = "";
            rbAnesthesiaType.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void ibtnCancel_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), false);
    }
    protected DataTable CreateTableService()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns.Add("ServiceId");
        dt.Columns.Add("ServiceName");
        dt.Columns.Add("IsSurgeryMain");//IncisionTime
        dt.Columns.Add("IncisionTime");
        //Only  for Nanavati
        dt.Columns.Add("SurgoenShare");
        dt.Columns.Add("SurgeryComponent");
        dt.Columns.Add("Surgery");
        // dt.Columns.Add("");
        //Only  for Nanavati
        return dt;
    }


    protected void btnUp_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (gvSurgery.SelectedItems.Count > 0)
            {
                if (gvSurgery.SelectedItems[0].RowIndex != 0)
                {
                    GridDataItem selectedItem = (GridDataItem)gvSurgery.SelectedItems[0];
                    if (selectedItem.ItemIndex > 0)
                    {
                        HiddenField hdnServiceId = (HiddenField)selectedItem.FindControl("hdnServiceId");
                        Label lblServiceName = (Label)selectedItem.FindControl("lblServiceName");

                        String strServiceIdUp, strServiceNameUp;

                        HiddenField hdnServiceId_Up = (HiddenField)gvSurgery.Items[selectedItem.ItemIndex - 1].FindControl("hdnServiceId");
                        strServiceIdUp = hdnServiceId_Up.Value;
                        hdnServiceId_Up.Value = hdnServiceId.Value;

                        Label lblServiceName_Up = (Label)gvSurgery.Items[selectedItem.ItemIndex - 1].FindControl("lblServiceName");
                        strServiceNameUp = lblServiceName_Up.Text;
                        lblServiceName_Up.Text = lblServiceName.Text;

                        hdnServiceId.Value = strServiceIdUp;
                        lblServiceName.Text = strServiceNameUp;

                        gvSurgery.SelectedIndexes.Clear();
                        gvSurgery.SelectedIndexes.Add(gvSurgery.Items[selectedItem.ItemIndex - 1].ItemIndex);
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

    protected void btnDown_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (gvSurgery.SelectedItems.Count > 0)
            {
                if (gvSurgery.SelectedItems[0].ItemIndex != gvSurgery.Items.Count - 1)
                {
                    GridDataItem selectedItem = (GridDataItem)gvSurgery.SelectedItems[0];

                    HiddenField hdnServiceId = (HiddenField)selectedItem.FindControl("hdnServiceId");
                    Label lblServiceName = (Label)selectedItem.FindControl("lblServiceName");

                    String strServiceIdDn, strServiceNameDn;

                    HiddenField hdnServiceId_Dn = (HiddenField)gvSurgery.Items[selectedItem.ItemIndex + 1].FindControl("hdnServiceId");
                    strServiceIdDn = hdnServiceId_Dn.Value;
                    hdnServiceId_Dn.Value = hdnServiceId.Value;

                    Label lblServiceName_Dn = (Label)gvSurgery.Items[selectedItem.ItemIndex + 1].FindControl("lblServiceName");
                    strServiceNameDn = lblServiceName_Dn.Text;
                    lblServiceName_Dn.Text = lblServiceName.Text;

                    hdnServiceId.Value = strServiceIdDn;
                    lblServiceName.Text = strServiceNameDn;

                    gvSurgery.SelectedIndexes.Clear();
                    gvSurgery.SelectedIndexes.Add(gvSurgery.Items[selectedItem.ItemIndex + 1].ItemIndex);
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

    protected void gvSurgery_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (e.Item.ItemIndex == 0)
            {
                LinkButton btnSelect = (LinkButton)e.Item.FindControl("btnSelect");
                btnSelect.Visible = false;
            }
            LinkButton btnAddSurgeryComponent = (LinkButton)e.Item.FindControl("btnAddSurgeryComponent");
            btnAddSurgeryComponent.Visible = true;
            if (common.myStr(ViewState["IsSurgeryChargesForNanavati"]).Equals(""))
            {
                LinkButton btnDoctorShare = (LinkButton)e.Item.FindControl("btnDoctorShare");
                // LinkButton btnAddSurgeryComponent = (LinkButton)e.Item.FindControl("btnAddSurgeryComponent");
                //btnDoctorShare.Visible = false;

            }
        }
    }

    protected void gvSurgery_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                if (common.myInt(((HiddenField)e.Item.FindControl("hdnServiceId")).Value) > 0)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)ViewState["ServiceDetail"];
                    HiddenField hdnServiceId = (HiddenField)e.Item.FindControl("hdnServiceId");
                    Label lnlMain = (Label)e.Item.FindControl("lnlMain");
                    if (common.myStr(lnlMain.Text) == "Yes")
                    {
                        ViewState["ServiceDetail"] = null;
                        gvSurgery.DataSource = null;
                        gvSurgery.DataBind();
                        gvSurgery.Visible = false;
                        btnUp.Visible = false;
                        btnDown.Visible = false;
                    }
                    else
                    {
                        DataView dv = new DataView(dt);
                        dv.RowFilter = "ServiceId<>" + hdnServiceId.Value;
                        ViewState["ServiceDetail"] = dv.ToTable();
                        gvSurgery.DataSource = dv.ToTable();
                        gvSurgery.DataBind();
                        gvSurgery.Visible = true;
                        if (dt.Rows.Count > 1)
                        {
                            btnUp.Visible = true;
                            btnDown.Visible = true;
                        }
                        else
                        {
                            btnUp.Visible = false;
                            btnDown.Visible = false;
                        }
                    }
                    return;
                }
            }
            if (e.CommandName == "Sharing")
            {
                int ServiceId = common.myInt(((HiddenField)e.Item.FindControl("hdnServiceId")).Value);
                string SurgoenShare = common.myStr(((HiddenField)e.Item.FindControl("hndxmlSurgoenShare")).Value);
                RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddShareandComponent.aspx?ServiceId=" + ServiceId + "&SurgoenShare" + SurgoenShare;
                RadWindow1.Height = 400;
                RadWindow1.Width = 900;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = "wndAddService_OnClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
            if (e.CommandName == "AddComponent")
            {
                int ServiceId = common.myInt(((HiddenField)e.Item.FindControl("hdnServiceId")).Value);
                string SurgoenComponent = common.myStr(((HiddenField)e.Item.FindControl("hndxmlSurgeryComponent")).Value);
                RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddSurgeryComponent.aspx?ServiceId=" + ServiceId + "&SurgoenShare" + SurgoenComponent;
                RadWindow1.Height = 400;
                RadWindow1.Width = 900;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = "wndSurgeonComponent_OnClientClose";

                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    //Added only for Surgory Start
    protected void btnBindGridWithXml_OnClick(object sender, EventArgs e)
    {
        DataTable objdt = (DataTable)ViewState["ServiceDetail"];

        foreach (DataRow Dr in objdt.Rows)
        {
            if (common.myInt(Dr["ServiceId"]).Equals(common.myInt(hdnSurServiceID.Value)))
            {
                Dr["SurgoenShare"] = hdnXmlSurgoenShare.Value;
            }
        }
        objdt.AcceptChanges();
        gvSurgery.DataSource = objdt;
        gvSurgery.DataBind();
        ViewState["ServiceDetail"] = objdt;
        BtnCalculateCharges_OnClick(null, null);
    }

    protected void btnBindGridWithXmlsurgeon_OnClick(object sender, EventArgs e)
    {
        DataTable objdt = (DataTable)ViewState["ServiceDetail"];

        foreach (DataRow Dr in objdt.Rows)
        {
            if (common.myInt(Dr["ServiceId"]).Equals(common.myInt(hdnSurServiceID.Value)))
            {
                Dr["SurgeryComponent"] = hdnXmlSurgeryComponent.Value;

            }
        }
        objdt.AcceptChanges();
        gvSurgery.DataSource = objdt;
        gvSurgery.DataBind();
        ViewState["ServiceDetail"] = objdt;
        BtnCalculateCharges_OnClick(null, null);
    }
    //Added only for Surgeory End

    protected void BtnCalculateCharges_OnClick(object sender, EventArgs e)
    {
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
        try
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //if (common.myInt(radCmbAnaesthesia.SelectedValue) == 0)
            //{
            //    lblMsg.Text = "Please select Anaesthesia !";
            //    return;
            //}
            if (common.myInt(radCmbBedCategory.SelectedValue) == 0)
            {
                lblMsg.Text = "Please select bed category !";
                return;
            }
            if (common.myInt(radCmbOtRoom.SelectedValue) == 0)
            {
                lblMsg.Text = "Please select ot room !";
                return;
            }

            if (hdnISAnesthesiaTypeAndAnaesthesiaOtherChargeMandatory.Value.Equals("Y"))
            {

                if (common.myInt(rbAnesthesiaType.SelectedValue).Equals(0))//added by ankit ojha
                {
                    lblMsg.Text = "Please select Anesthesia Type !";
                    return;
                }

                string AnesthesiaServiceId1 = string.Empty;////added by ankit ojha

                foreach (RadComboBoxItem currentItem in radCmbAnaesthesia.Items)////added by ankit ojha
                {
                    if (currentItem.Checked == true)
                    {
                        if (AnesthesiaServiceId1.Equals(""))
                            AnesthesiaServiceId1 = currentItem.Value;
                        else
                            AnesthesiaServiceId1 = AnesthesiaServiceId1 + "," + currentItem.Value;
                    }
                }

                if (AnesthesiaServiceId1.Equals(""))////added by ankit ojha
                {
                    lblMsg.Text = "Please select Anaesthesia/Other Charges !";
                    return;
                }
            }

            StringBuilder sbSurgery = new StringBuilder();
            ArrayList colSurgery = new ArrayList();
            int MainServiceId = 0;
            int count = 0;
            foreach (GridDataItem grow in gvSurgery.Items)
            {
                int IsMain = 0;
                HiddenField hdnServiceId = (HiddenField)grow.FindControl("hdnServiceId");
                Label lnlMain = (Label)grow.FindControl("lnlMain");
                if (common.myStr(lnlMain.Text) == "Yes")
                {
                    IsMain = 1;
                    MainServiceId = common.myInt(hdnServiceId.Value);
                }
                colSurgery.Add(common.myInt(hdnServiceId.Value));
                colSurgery.Add(common.myStr("SR"));
                colSurgery.Add(common.myInt(IsMain));
                colSurgery.Add(0);
                if (common.myStr(ViewState["IncisionTime"]) != "")
                    colSurgery.Add(ViewState["IncisionTime"]);
                else
                    colSurgery.Add(rdtpAstartTime.SelectedDate);
                colSurgery.Add(0);
                colSurgery.Add(common.myInt(rbAnesthesiaType.SelectedValue.Trim()));
                sbSurgery.Append(common.setXmlTable(ref colSurgery));
                count = 1;
            }
            if (count == 0)
            {
                lblMsg.Text = "Please add atleast one surgery !";
                return;
            }
            foreach (GridViewRow grow in gvResourceList.Rows)
            {
                int IsMain = 0;
                int serviceId2 = 0;
                int serviceId3 = 0;
                int serviceId4 = 0;
                HiddenField hdnResouceID = (HiddenField)grow.FindControl("hdnResourceID");
                HiddenField hdnResourceType = (HiddenField)grow.FindControl("hdnResourceType");
                HiddenField hdnID = (HiddenField)grow.FindControl("hdnID");
                HiddenField hdnSurgeryNo = (HiddenField)grow.FindControl("hdnSurgeryNo");



                foreach (GridDataItem grow1 in gvSurgery.Items)
                {
                    HiddenField hdnSNO = (HiddenField)grow1.FindControl("hdnSNO");
                    HiddenField hdnServiceId = (HiddenField)grow1.FindControl("hdnServiceId");
                    if (common.myInt(hdnSNO.Value) == 1)
                    {
                        MainServiceId = common.myInt(hdnServiceId.Value);
                    }
                    else if (common.myInt(hdnSNO.Value) == 2)
                    {
                        serviceId2 = common.myInt(hdnServiceId.Value);
                    }
                    else if (common.myInt(hdnSNO.Value) == 3)
                    {
                        serviceId3 = common.myInt(hdnServiceId.Value);
                    }
                    else if (common.myInt(hdnSNO.Value) == 4)
                    {
                        serviceId4 = common.myInt(hdnServiceId.Value);
                    }

                }
                if (common.myInt(hdnSurgeryNo.Value) == 1)
                {
                    colSurgery.Add(common.myInt(MainServiceId));
                }
                else if (common.myInt(hdnSurgeryNo.Value) == 2)
                {
                    colSurgery.Add(common.myInt(serviceId2));
                }
                else if (common.myInt(hdnSurgeryNo.Value) == 3)
                {
                    colSurgery.Add(common.myInt(serviceId3));
                }
                else if (common.myInt(hdnSurgeryNo.Value) == 4)
                {
                    colSurgery.Add(common.myInt(serviceId4));
                }
                else
                {
                    colSurgery.Add(common.myInt(MainServiceId));
                }
                //colSurgery.Add(common.myInt(MainServiceId));
                //   colSurgery.Add(common.myInt(hdnID.Value));
                colSurgery.Add(common.myStr(hdnResourceType.Value));
                colSurgery.Add(common.myInt(IsMain));
                colSurgery.Add(common.myInt(hdnResouceID.Value));
                if (common.myStr(ViewState["IncisionTime"]) != "")
                    colSurgery.Add(ViewState["IncisionTime"]);
                else
                    colSurgery.Add(rdtpAstartTime.SelectedDate);
                colSurgery.Add(common.myInt(hdnSurgeryNo.Value));
                sbSurgery.Append(common.setXmlTable(ref colSurgery));



            }

            DataSet ds = new DataSet();
            int HospId = common.myInt(Session["HospitalLocationId"]);
            int FacilityIdId = common.myInt(Session["FacilityId"]);
            int RegId = common.myInt(Request.QueryString["RegId"]);
            int EncId = common.myInt(Request.QueryString["EncId"]);
            int CompId = common.myInt(Request.QueryString["CompanyId"]);
            int InsId = common.myInt(Request.QueryString["InsuranceId"]);
            int CardId = common.myInt(Request.QueryString["CardId"]);
            string OPIP = common.myStr(Request.QueryString["OP_IP"]);

            int BedCatId = common.myInt(radCmbBedCategory.SelectedValue);
            int OTServiceId = common.myInt(radCmbOtRoom.SelectedValue);
            // int AnesthesiaServiceId = common.myInt(radCmbAnaesthesia.SelectedValue);
            string AnesthesiaServiceId = "";

            foreach (RadComboBoxItem currentItem in radCmbAnaesthesia.Items)
            {
                if (currentItem.Checked == true)
                {
                    if (AnesthesiaServiceId.Equals(""))
                        AnesthesiaServiceId = currentItem.Value;
                    else
                        AnesthesiaServiceId = AnesthesiaServiceId + "," + currentItem.Value;
                }
            }
            DateTime dtOrderDate = Convert.ToDateTime(rdtpOtStartTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            Hashtable hshIn = new Hashtable();


            if (common.myInt(ViewState["DatefromOt"]) == 0)
            {
                ViewState["OTCheckintime"] = rdtpOtStartTime.SelectedDate;

                ViewState["OTCheckouttime"] = rdtpOtEndTime.SelectedDate;
            }

            if (!common.myStr(ViewState["IsSurgeryChargesForNanavati"]).Equals(""))  //For Nanavalti only
            {
                StringBuilder xmlSurgoenShare = new StringBuilder();
                StringBuilder xmlSurgeryComponent = new StringBuilder();
                bool IsEmergency = false, IsHighRiskSurgery = chkIsHighRiskSurgery.Visible && chkIsHighRiskSurgery.Checked,
                      IsHighRiskStat = chkIsHighRiskState.Visible && chkIsHighRiskState.Checked, Stat = chkStat.Checked;
                if (chkStat.Checked == true)
                    IsEmergency = true;

                if (chkIsEmergency.Checked == true)
                {
                    IsEmergency = true;
                }

                foreach (GridDataItem grow in gvSurgery.Items)
                {
                    HiddenField hndxmlSurgoenShare = (HiddenField)grow.FindControl("hndxmlSurgoenShare");
                    HiddenField hndxmlSurgeryComponent = (HiddenField)grow.FindControl("hndxmlSurgeryComponent");
                    xmlSurgoenShare.Append(hndxmlSurgoenShare.Value);
                    xmlSurgeryComponent.Append(hndxmlSurgeryComponent.Value);
                }

                ds = objOT.GetServiceChargeSurgeryWithMultipleANServiceId(HospId, FacilityIdId, RegId, EncId, CompId, InsId, CardId, BedCatId,
                                           OPIP, OTServiceId, AnesthesiaServiceId, sbSurgery, dtOrderDate, common.myDate(ViewState["OTCheckintime"]),
                                           common.myDate(ViewState["OTCheckouttime"]), common.myInt(rdoIncision.SelectedValue), xmlSurgoenShare.ToString(), xmlSurgeryComponent.ToString(), IsEmergency, false, common.myInt(Request.QueryString["OTBookingId"]), IsHighRiskStat, common.myInt(ddlOTEquipments.SelectedValue));
                ViewState["IsDelete"] = null;
            }
            else
            {
                StringBuilder xmlSurgoenShare = new StringBuilder();
                StringBuilder xmlExtraSurgeryComponent = new StringBuilder();
                xmlExtraSurgeryComponent.Append("");
                foreach (GridDataItem grow in gvSurgery.Items)
                {
                    HiddenField hndxmlSurgoenShare = (HiddenField)grow.FindControl("hndxmlSurgoenShare");
                    HiddenField hndxmlSurgeryComponent = (HiddenField)grow.FindControl("hndxmlSurgeryComponent");
                    xmlSurgoenShare.Append(hndxmlSurgoenShare.Value);
                    xmlExtraSurgeryComponent.Append(hndxmlSurgeryComponent.Value);
                }

                bool IsEmergency = false, IsHighRiskSurgery = chkIsHighRiskSurgery.Visible && chkIsHighRiskSurgery.Checked,
                      IsHighRiskStat = chkIsHighRiskState.Visible && chkIsHighRiskState.Checked, Stat = chkStat.Checked;
                if (chkStat.Checked == true)
                    IsEmergency = true;

                IsEmergency = (!IsEmergency && chkIsEmergency.Visible && chkIsEmergency.Checked) ? true : false;

                //foreach (GridDataItem grow in gvSurgery.Items)
                //{
                //    HiddenField hndxmlSurgoenShare = (HiddenField)grow.FindControl("hndxmlSurgoenShare");
                //    xmlSurgoenShare.Append(hndxmlSurgoenShare.Value);
                //}

                ds = objOT.GetServiceChargeSurgeryWithMultipleANServiceId(HospId, FacilityIdId, RegId, EncId, CompId, InsId, CardId, BedCatId, OPIP, OTServiceId, AnesthesiaServiceId, sbSurgery, dtOrderDate, common.myDate(ViewState["OTCheckintime"]), common.myDate(ViewState["OTCheckouttime"]), common.myInt(rdoIncision.SelectedValue), xmlSurgoenShare.ToString(), xmlExtraSurgeryComponent.ToString(), IsEmergency, IsHighRiskSurgery, common.myInt(Request.QueryString["OTBookingId"]), IsHighRiskStat, common.myInt(ddlOTEquipments.SelectedValue));
            }

            if (common.myStr(Session["OTChargeable"]) == "False")
            {
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["ServiceCharge"] = "0.00";
                        dt.Rows[i]["ChargePercentage"] = "0.00";
                        dt.Rows[i]["OTBedChargePerc"] = "0.00";
                        dt.Rows[i]["NetCharge"] = "0.00";
                        dt.Rows[i]["PayerAmount"] = "0";
                        dt.Rows[i]["PatientAmount"] = "0.00";

                    }

                    ViewState["AddedSurgery"] = dt;

                    gvAddedSurgery.DataSource = dt;
                    gvAddedSurgery.DataBind();

                    dt.Dispose();
                }

            }
            else
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["AddedSurgery"] = ds.Tables[0];
                        gvAddedSurgery.DataSource = ds.Tables[0];
                        gvAddedSurgery.DataBind();
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
        finally { objOT = null; }
    }

    protected void gvAddedSurgery_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        try
        {
            BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
            Hashtable ht = new Hashtable();
            // ht = objRest.GetIsunPlannedReturnToOt(common.myInt(Session["myid"]), common.myInt(Session["FacilityId"]), common.myDate(dtDateTimeForOT.SelectedDate));
            // use otbooking isOTCharable =0 then did


            if (e.Item is GridDataItem)
            {
                if (common.myStr(ViewState["IsSurgeryChargesForNanavati"]).Equals(""))
                {
                    HiddenField hdnActualGrossCharge = (HiddenField)e.Item.FindControl("hdnActualGrossCharge");
                    HiddenField hdnActualNetCharge = (HiddenField)e.Item.FindControl("hdnActualNetCharge");
                    HiddenField hdnSurgeryComponentId = (HiddenField)e.Item.FindControl("hdnSurgeryComponentId");
                    HiddenField hdnIsEmergency = (HiddenField)e.Item.FindControl("hdnIsEmergency");
                    HiddenField hdnEmergencyPerc = (HiddenField)e.Item.FindControl("hdnEmergencyPerc");

                    gvAddedSurgery.Columns.Remove("hdnActualGrossCharge");
                    gvAddedSurgery.Columns.Remove("hdnActualNetCharge");
                    gvAddedSurgery.Columns.Remove("hdnSurgeryComponentId");
                    gvAddedSurgery.Columns.Remove("hdnIsEmergency");
                    gvAddedSurgery.Columns.Remove("hdnEmergencyPerc");

                }

                HiddenField hdnIsMainSurgery = (HiddenField)e.Item.FindControl("hdnIsMainSurgery");
                if (common.myInt(hdnIsMainSurgery.Value) == 1)
                {
                    e.Item.BackColor = System.Drawing.Color.LightYellow;
                }
                HiddenField hdnResourceId = (HiddenField)e.Item.FindControl("hdnResourceId");
                HiddenField hdnDoctorRequired = (HiddenField)e.Item.FindControl("hdnDoctorRequired");
                RadComboBox ddlResourceName = (RadComboBox)e.Item.FindControl("ddlResourceName");
                HiddenField hdnSurgeonType = (HiddenField)e.Item.FindControl("hdnSurgeonType");
                HiddenField hdnIsPriceEditable = (HiddenField)e.Item.FindControl("hdnIsPriceEditable");
                TextBox txtServiceCharge = (TextBox)e.Item.FindControl("txtServiceCharge");
                HiddenField hdnServiceId = (HiddenField)e.Item.FindControl("hdnServiceId");
                HiddenField hdnServiceActualAmount = (HiddenField)e.Item.FindControl("hdnServiceActualAmount");
                HiddenField NewhdnServiceType = (HiddenField)e.Item.FindControl("hdnServiceType");

                HiddenField hdnChargePercentage = (HiddenField)e.Item.FindControl("hdnChargePercentage");
                HiddenField hdnlblGrossCharge = (HiddenField)e.Item.FindControl("hdnlblGrossCharge");
                HiddenField hdnlblNetCharge = (HiddenField)e.Item.FindControl("hdnlblNetCharge");
                HiddenField hdnlblPayerAmount = (HiddenField)e.Item.FindControl("hdnlblPayerAmount");
                HiddenField hdnlblPatientAmount = (HiddenField)e.Item.FindControl("hdnlblPatientAmount");

                HiddenField hdnServiceDiscount = (HiddenField)e.Item.FindControl("hdnServiceDiscount");

                Label lblGrossCharge = (Label)e.Item.FindControl("lblGrossCharge");
                Label lblNetCharge = (Label)e.Item.FindControl("lblNetCharge");
                Label lblPatientAmount = (Label)e.Item.FindControl("lblPatientAmount");
                Label lblPayerAmount = (Label)e.Item.FindControl("lblPayerAmount");

                txtServiceCharge.ReadOnly = true;


                if (common.myStr(NewhdnServiceType.Value) == "S")
                {
                    if (common.myInt(hdnIsPriceEditable.Value) == 1)
                    {
                        txtServiceCharge.ReadOnly = false;
                        txtServiceCharge.Attributes.Add("onChange", "javascript:CalculateAmount('" + txtServiceCharge.ClientID + "','" + hdnServiceActualAmount.ClientID + "');");
                    }

                    if (common.myStr(hdnIsAllowToEditSurgeryAmountOnlyForCash.Value) == "Y")
                    {
                        if (hdnPayerType.Value == "C")
                        {
                            txtServiceCharge.ReadOnly = false;
                            txtServiceCharge.Attributes.Add("onChange", "javascript:CalculateAmount('" + txtServiceCharge.ClientID + "','" + hdnServiceActualAmount.ClientID + "');");
                        }
                        else
                        {
                            txtServiceCharge.ReadOnly = true;
                        }
                    }

                }
                else if (common.myInt(hdnChargePercentage.Value) == 0)
                {

                    if (common.myInt(hdnIsPriceEditable.Value) == 1)
                    {
                        txtServiceCharge.ReadOnly = false;
                        txtServiceCharge.Attributes.Add("onChange", "javascript:CalculateAmountNew('" + txtServiceCharge.ClientID + "','" + hdnServiceActualAmount.ClientID
                            + "','" + hdnChargePercentage.ClientID + "','" + NewhdnServiceType.ClientID + "','" + hdnlblGrossCharge.ClientID + "','"
                           + hdnlblNetCharge.ClientID + "','" + hdnlblPatientAmount.ClientID + "','" + hdnlblPayerAmount.ClientID + "','"

                           + lblGrossCharge.ClientID + "','" + lblNetCharge.ClientID + "','" + lblPatientAmount.ClientID + "','"
                           + lblPayerAmount.ClientID + "','" + hdnServiceDiscount.ClientID + "','" + e.Item.ItemIndex + "');");
                    }

                    if (common.myStr(hdnIsAllowToEditSurgeryAmountOnlyForCash.Value) == "Y")
                    {
                        if (hdnPayerType.Value == "C")
                        {
                            txtServiceCharge.ReadOnly = false;
                            txtServiceCharge.Attributes.Add("onChange", "javascript:CalculateAmountNew('" + txtServiceCharge.ClientID + "','"
                                + hdnServiceActualAmount.ClientID + "','" + hdnChargePercentage.ClientID + "','"
                                + NewhdnServiceType.ClientID + "','" + "','" + hdnlblGrossCharge.ClientID + "','"
                           + hdnlblNetCharge.ClientID + "','" + hdnlblPatientAmount.ClientID + "','" + hdnlblPayerAmount.ClientID + "','"

                          + lblGrossCharge.ClientID + "','" + lblNetCharge.ClientID + "','" + lblPatientAmount.ClientID + "','"
                           + lblPayerAmount.ClientID + "','" + hdnServiceDiscount.ClientID + "','" + e.Item.ItemIndex + "');");
                        }
                        else
                        {
                            txtServiceCharge.ReadOnly = true;
                        }
                    }
                }

                if (common.myInt(hdnDoctorRequired.Value) == 1)
                {
                    ddlResourceName.Visible = true;
                    DataSet ds = new DataSet();
                    if (ViewState["EmpClassi"] == null)
                    {
                        ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]));
                        ViewState["EmpClassi"] = ds;
                    }
                    else
                    { ds = (DataSet)ViewState["EmpClassi"]; }
                    DataView dvF = new DataView(ds.Tables[0]);
                    if (common.myStr(setisAllDoctorDisplayOnAddService).ToUpper() == "N")
                    {
                        if (Request.QueryString["OP_IP"] != null && Request.QueryString["OP_IP"].Equals("O"))
                        {
                            dvF.RowFilter = "Type='" + common.myStr(hdnSurgeonType.Value) + "' AND ProvidingService IN ('O','B')";
                        }
                        else
                        {
                            dvF.RowFilter = "Type='" + common.myStr(hdnSurgeonType.Value) + "'";
                        }
                        if (dvF.ToTable().Rows.Count == 0)
                        {
                            dvF.RowFilter = "Type IN ('D','TM','SR','AS','AN','OD','LD')";

                            if (Request.QueryString["OP_IP"] != null && Request.QueryString["OP_IP"].Equals("O"))
                            {
                                dvF.RowFilter = "Type='" + common.myStr(hdnSurgeonType.Value) + "' AND ProvidingService IN ('O','B')";
                            }
                        }
                    }
                    else
                    {
                        dvF.RowFilter = "Type IN ('D','TM','SR','AS','AN','OD','LD')";
                    }

                    if (isAllDoctorDisplayOnAddService.Equals("N"))
                    {
                        if (!hdnSurgeonType.Value.Equals("AN"))
                        {
                            HiddenField hdnDepartmentId = (HiddenField)e.Item.FindControl("hdnDepartmentId");
                            if (hdnDepartmentId != null)
                            {
                                dvF.RowFilter = "DepartmentId=" + common.myStr(hdnDepartmentId.Value);
                            }
                        }
                    }


                    if (dvF.ToTable().Rows.Count > 0)
                    {
                        ddlResourceName.Items.Clear();
                        ddlResourceName.DataSource = dvF.ToTable();
                        ddlResourceName.DataValueField = "EmployeeId";
                        ddlResourceName.DataTextField = "EmployeeName";
                        ddlResourceName.DataBind();

                    }
                    ddlResourceName.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                    if (Request.QueryString["OTBookingId"] != null && ViewState["dtResource"] != null)
                    {
                        DataTable dtResource = (DataTable)ViewState["dtResource"];
                        if (dtResource.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dtResource);
                            dv.RowFilter = "ResourceType='" + common.myStr(hdnSurgeonType.Value) + "' and id=" + common.myStr(hdnServiceId.Value);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                ddlResourceName.SelectedIndex = ddlResourceName.Items.IndexOf(ddlResourceName.Items.FindItemByValue(common.myStr(dv.ToTable().Rows[0]["ResourceId"])));
                                //ddlResourceName.SelectedValue = common.myStr(dv.ToTable().Rows[0]["ResourceId"]);


                                //For Nanavati Hospital Start
                                //if (common.myStr(hdnResourceId.Value) != "0")
                                //{
                                //    ddlResourceName.SelectedValue = common.myStr(hdnResourceId.Value);
                                //}
                                //else
                                //{
                                //    ddlResourceName.SelectedValue = common.myStr(dv.ToTable().Rows[0]["ResourceId"]);
                                //}
                                //For Nanavati Hospital End
                            }
                            else  //For Nanavati Only
                            {
                                if (hdnResourceId.Value != "0")
                                {
                                    ddlResourceName.SelectedValue = common.myStr(hdnResourceId.Value);

                                }
                            }
                        }
                    }
                    if (ViewState["IsDelete"] != null && common.myStr(ViewState["IsDelete"]) == "Yes")
                    {
                        ddlResourceName.SelectedIndex = ddlResourceName.Items.IndexOf(ddlResourceName.Items.FindItemByValue(common.myStr(hdnResourceId.Value)));
                    }
                    ds.Dispose();
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
        finally { objCommon = null; }
    }

    protected void gvAddedSurgery_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                ViewState["IsDelete"] = "Yes";
                if (common.myInt(((HiddenField)e.Item.FindControl("hdnServiceId")).Value) > 0)
                {
                    HiddenField hdnId = (HiddenField)e.Item.FindControl("hdnId");
                    HiddenField hdnIsMainSurgery = (HiddenField)e.Item.FindControl("hdnIsMainSurgery");
                    if (common.myStr(hdnIsMainSurgery.Value) == "True" || common.myStr(hdnIsMainSurgery.Value) == "1")
                    {
                        ViewState["AddedSurgery"] = null;
                        gvAddedSurgery.DataSource = null;
                        gvAddedSurgery.DataBind();
                    }
                    else
                    {
                        UpdateDataTableAddedSurgery();
                        DataTable dt = new DataTable();
                        if (ViewState["AddedSurgery"] != null)
                        {
                            dt = (DataTable)ViewState["AddedSurgery"];
                        }
                        DataView dv = new DataView(dt);
                        dv.RowFilter = "Id<>" + hdnId.Value;
                        ViewState["AddedSurgery"] = dv.ToTable();
                        gvAddedSurgery.DataSource = dv.ToTable();
                        gvAddedSurgery.DataBind();
                        dt.Dispose();
                    }
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
    }
    private void UpdateDataTableAddedSurgery()
    {
        try
        {
            DataTable dt = new DataTable();
            if (ViewState["AddedSurgery"] != null)
            {
                dt = (DataTable)ViewState["AddedSurgery"];
            }
            foreach (GridDataItem grow in gvAddedSurgery.Items)
            {
                HiddenField hdnId = (HiddenField)grow.FindControl("hdnId");
                RadComboBox ddlResourceName = (RadComboBox)grow.FindControl("ddlResourceName");
                dt.DefaultView.RowFilter = "";
                dt.DefaultView.RowFilter = "Id = " + hdnId.Value;
                if (dt.DefaultView.Count > 0)
                {
                    dt.DefaultView[0]["ResourceId"] = common.myInt(ddlResourceName.SelectedValue);
                    dt.AcceptChanges();
                    dt.DefaultView.RowFilter = "";
                    dt.AcceptChanges();
                }
            }
            ViewState["AddedSurgery"] = dt;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void ddlResourceName_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        //foreach (GridDataItem gitem in gvAddedSurgery.Items)
        //{
        //    HiddenField hdnDepartmentId = (HiddenField)gitem.FindControl("hdnDepartmentId");
        //    HiddenField hdnSurgeonTypeId = (HiddenField)gitem.FindControl("hdnSurgeonTypeId");
        //    RadComboBox ddlResourceName = (RadComboBox)gitem.FindControl("ddlResourceName");

        //    HiddenField hdnResourceType = (HiddenField)gitem.FindControl("hdnResourceType");

        //    //if (common.myStr(hdnResourceType.Value) == "Surgeon")
        //    //{
        //    //    Hashtable HshIn = new Hashtable();
        //    //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    //    HshIn.Add("@intId", ddlResourceName.SelectedValue);

        //    //    string qry = "select departmentid from Employee where id=@intId";

        //    //    DataSet ds = (DataSet)objDl.FillDataSet(CommandType.Text, qry, HshIn);

        //    //    if (ds.Tables[0].Rows[0][0] != hdnDepartmentId.Value)
        //    //    {
        //    //        Alert.ShowAjaxMsg("Doctor Department and Service Department Both Are Different", this.Page);
        //    //        return;
        //    //    }
        //    //}

        //}
    }
    protected void btnProceed_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(Session["SurSvchk"]) == 0)
        {
            BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
            DataTable dt = new DataTable();

            try
            {

                //foreach (GridDataItem gitem in gvAddedSurgery.Items)
                //{
                //    HiddenField hdnDepartmentId = (HiddenField)gitem.FindControl("hdnDepartmentId");
                //    HiddenField hdnSurgeonTypeId = (HiddenField)gitem.FindControl("hdnSurgeonTypeId");
                //    RadComboBox ddlResourceName = (RadComboBox)gitem.FindControl("ddlResourceName");

                //    HiddenField hdnResourceType = (HiddenField)gitem.FindControl("hdnResourceType");

                //    if (common.myStr(hdnResourceType.Value) == "Surgeon")
                //    {
                //        Hashtable HshIn = new Hashtable();
                //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //        HshIn.Add("@intId", ddlResourceName.SelectedValue);

                //        string qry = "select departmentid from Employee where id=@intId";

                //        DataSet ds = (DataSet)objDl.FillDataSet(CommandType.Text, qry, HshIn);

                //        if (ds.Tables[0].Rows[0][0] != hdnDepartmentId.Value)
                //        {
                //            string confirmValue = Request.Form["confirm_value"];
                //            ddlResourceName.Attributes.Add("onChange", "javascript:confirm_value();");
                //            Alert.ShowAjaxMsg("Doctor Department and Service Department Both Are Different", this.Page);
                //            return;
                //        }
                //    }

                //}


                dt = UpdateDataTable();

                if (dt == null)
                {
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "Select Surgery...";
                    return;
                }
                if (common.myInt(radCmbOtRoom.SelectedValue) == 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "Select OT Room...";
                    return;
                }
                if (common.myBool(ViewState["ResourcenameEmpty"]))
                {
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "Select Resource Name...";
                    return;
                }
                //if (common.myInt(radCmbAnaesthesia.SelectedValue) == 0)
                //{
                //    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //    lblMsg.Text = "Select Anaesthesia...";
                //    return;
                //}.

                //int i = 0, flag = 0;

                //foreach (GridDataItem gitem in gvAddedSurgery.Items)
                //{
                //    RadComboBox ddlResourceName = (RadComboBox)gitem.FindControl("ddlResourceName");
                //    i = 0;
                //    foreach (GridDataItem gitem1 in gvAddedSurgery.Items)
                //    {
                //        RadComboBox ddlResourceName1 = (RadComboBox)gitem1.FindControl("ddlResourceName");
                //        if (ddlResourceName.SelectedIndex > 0 && ddlResourceName1.SelectedIndex > 0)
                //        {
                //            if (common.myInt(ddlResourceName.SelectedValue) == common.myInt(ddlResourceName1.SelectedValue))
                //            {
                //                i++;
                //            }
                //        }
                //    }

                //    if (i > 1)
                //    {
                //        flag = 1;
                //        break;
                //    }
                //}

                //if (flag == 1)
                //{
                //    Alert.ShowAjaxMsg("Same resource name cannot be selected multiple time...", this.Page);
                //    return;

                //}

                if (common.myStr(Request.QueryString["OP_IP"]) == "O" && common.myStr(Request.QueryString["OTBookingId"]) == "")
                {
                    DataSet xmlDS = new DataSet();
                    xmlDS.Tables.Add(dt.Copy());
                    System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                    xmlDS.WriteXml(writer);
                    string xmlSchema = writer.ToString();
                    hdnXmlString.Value = xmlSchema;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                }
                else if (common.myStr(Request.QueryString["OP_IP"]) == "I" || common.myStr(Request.QueryString["OTBookingId"]) != "")
                {
                    // method for saving in case of IP

                    Hashtable hshIn = new Hashtable();
                    Hashtable hshOut = new Hashtable();
                    StringBuilder strXML = new StringBuilder();
                    ArrayList coll = new ArrayList();

                    foreach (DataRow dr in dt.Rows)
                    {
                        coll.Add(common.myInt(dr["ServiceId"])); //ServiceId INT,1
                        coll.Add(DBNull.Value); //VisitDate SMALLDATETIME,   common.myDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:00") 2
                        coll.Add(common.myInt(dr["Units"])); //Units TINYINT,3
                        coll.Add(common.myInt(dr["DoctorID"])); //DoctorId INT, 4
                                                                // coll.Add(common.myDec(dr["Charge"])); //ServiceAmount MONEY, (Charge - After calculating by %) 5
                        coll.Add(common.myDec(dr["GrossCharge"]));
                        coll.Add(common.myDec(dr["DoctorAmount"])); //DoctorAmount MONEY,  6 
                        coll.Add(common.myDec(dr["ServiceDiscountAmount"])); //ServiceDiscountAmount MONEY,  7
                        coll.Add(common.myDec(dr["DoctorDiscountAmount"])); //DoctorDiscountAmount MONEY,8
                        coll.Add(common.myDec(dr["AmountPayableByPatient"])); //AmountPayableByPatient MONEY,9
                        coll.Add(common.myDec(dr["AmountPayableByPayer"])); //AmountPayableByPayer MONEY,10
                        coll.Add(common.myDec(dr["ServiceDiscountPercentage"])); //ServiceDiscountPer MONEY,11
                        coll.Add(common.myDec(dr["DoctorDiscountPercentage"])); //DoctorDiscountPer MONEY,12
                        coll.Add(common.myInt(dr["PackageId"])); //PackageId INT,  13
                        coll.Add(common.myInt(dr["OrderId"])); //OrderId INT, 14
                        coll.Add(common.myInt(dr["UnderPackage"])); //UnderPackage BIT,     15           
                        coll.Add(DBNull.Value); //ICDID VARCHAR(100),  16

                        if (common.myInt(dr["ResourceId"]) > 0)
                            coll.Add(common.myInt(dr["ResourceId"])); //ResourceID INT, 17
                        else
                            coll.Add(DBNull.Value); //ResourceID INT, 17

                        if (!common.myStr(ViewState["IsSurgeryChargesForNanavati"]).Equals(""))//For Nanavati only
                        {
                            coll.Add(common.myDec(dr["ActualNetCharge"])); //SurgeryAmount MONEY,  18
                        }
                        else
                        {
                            coll.Add(DBNull.Value); //SurgeryAmount MONEY,  18

                        }
                        // coll.Add(common.myDec(dr["ChargePercentage"])); //ProviderPercent MONEY, 19
                        // coll.Add(DBNull.Value); //SurgeryAmount MONEY,  20
                        coll.Add(common.myDec(dr["ChargePercentage"])); //ProviderPercent MONEY, 19
                        coll.Add(common.myInt(dr["SNo"])); //SeQNo INT,  20
                        coll.Add(DBNull.Value); //Serviceremarks Varchar(150) 21
                        coll.Add(0); // DetailId  22

                        if (!common.myStr(ViewState["IsSurgeryChargesForNanavati"]).Equals(""))
                        {
                            coll.Add(0); // CoPayAmt 23 For Nanavati only
                            coll.Add(0); // CoPayAmt 24 For Nanavati only
                            coll.Add(0); // CoPayAmt 25 For Nanavati only
                            coll.Add(0); // DeductableAmount 26  For Nanavati only
                            coll.Add(0); // ApprovalCode 27 For Nanavati only
                            coll.Add(0); // ApprovalCode 28 For Nanavati only
                            coll.Add(0); // FacilityId 29  For Nanavati only
                            coll.Add(0); // Stat 30  For Nanavati only

                            coll.Add(common.myDec(dr["ActualGrossCharge"])); //For Nanavati only 31
                            coll.Add(common.myDec(dr["ActualNetCharge"])); //For Nanavati only 32
                            coll.Add(common.myInt(dr["SurgeryComponentId"])); // For Nanavati only 33
                            coll.Add(common.myInt(dr["IsEmergency"])); // For Nanavati only 34
                            coll.Add(common.myInt(dr["EmergencyPerc"])); // For Nanavati only 35
                        }

                        coll.Add(0); //23
                        coll.Add(0); //24
                        coll.Add(0);//25
                        coll.Add(0); //26
                        coll.Add(DBNull.Value); //27
                        coll.Add(0); //28
                        coll.Add(common.myStr(dr["Stat"])); //29
                        coll.Add(0); //30
                        coll.Add(DBNull.Value); //31
                        coll.Add(0); //32
                        coll.Add(DBNull.Value);//33
                        coll.Add(0); //34
                        coll.Add(DBNull.Value); //35
                        coll.Add(DBNull.Value); //36
                        coll.Add(DBNull.Value); //37
                        coll.Add(DBNull.Value); //38
                        coll.Add(DBNull.Value); //39
                        coll.Add(DBNull.Value); //40                        
                        coll.Add(DBNull.Value); //41
                        coll.Add(common.myInt(dr["SurgeryComponentId"])); //42
                        coll.Add(common.myInt(dr["SurgeryId"])); //43
                        coll.Add(common.myInt(dr["HighRisk"]));

                        coll.Add(DBNull.Value);
                        coll.Add(DBNull.Value);
                        coll.Add(DBNull.Value);
                        coll.Add(DBNull.Value);
                        coll.Add(DBNull.Value);
                        coll.Add(common.myInt(dr["TariffId"]));

                        strXML.Append(common.setXmlTable(ref coll));
                    }
                    if (strXML.ToString().Trim().Length > 1)
                    {
                        int HospId = common.myInt(Session["HospitalLocationId"]);
                        int FacilityId = common.myInt(Session["FacilityId"]);
                        int UserId = common.myInt(Session["UserId"]);
                        int RegId = common.myInt(Request.QueryString["RegId"]);
                        int EncId = common.myInt(Request.QueryString["EncId"]);
                        int CompId = common.myInt(Request.QueryString["CompanyId"]);
                        int InsId = common.myInt(Request.QueryString["InsuranceId"]);
                        int CardId = common.myInt(Request.QueryString["CardId"]);
                        int PayerType = common.myInt(Request.QueryString["PayerType"]);
                        string OPIP = common.myStr(Request.QueryString["OP_IP"]);
                        int BedCatId = common.myInt(radCmbBedCategory.SelectedValue);
                        int OTServiceId = common.myInt(radCmbOtRoom.SelectedValue);
                        //   int AnesthesiaServiceId = common.myInt(radCmbAnaesthesia.SelectedValue);
                        int EncounterDoctorId = 0;
                        bool Clean = common.myBool(chkUnClean.Checked);
                        string sOrderType = "SI"; // +Request.QueryString["OP_IP"].ToUpper().ToString();
                        string sChargeCalculationRequired = "N";

                        //for Nanavati only hospital Start
                        //hdnActualGrossCharge

                        //for Nanavati only hospital End
                        bool isGenerateAdvance = false;
                        isGenerateAdvance = chkIsGenerateAdvance.Checked;
                        int IsEmergency = (chkIsEmergency.Visible && chkIsEmergency.Checked) ? 1 : 0;
                        hshOut = BaseBill.saveOrders(HospId, FacilityId, RegId, EncId, strXML.ToString(), "", UserId, EncounterDoctorId, CompId,
                                                    sOrderType, common.myStr(PayerType), OPIP, InsId, CardId, common.myStr(hdnXmlSurgery.Value),
                                                    Convert.ToDateTime(dtOrderDate.SelectedDate), sChargeCalculationRequired,
                                                    common.myBool(common.myInt(rdoIncision.SelectedValue)), common.myInt(Session["EntrySite"]), IsEmergency, isGenerateAdvance);

                        if (common.myStr(hshOut["chvErrorStatus"]).Length == 0)
                        {
                            if (common.myInt(hshOut["intOrderId"]) > 0)
                            {
                                if (Request.QueryString["OTBookingId"] != null)
                                {
                                    int OTBookingId = common.myInt(Request.QueryString["OTBookingId"]);
                                    int OrderId = common.myInt(hshOut["intOrderId"]);
                                    BaseC.RestFulAPI objOT = new BaseC.RestFulAPI(sConString);
                                    string Result = objOT.UpdateOTBookingWithOrderId(HospId, FacilityId, OTBookingId, OrderId, UserId);
                                    if (Result.Contains("Error"))
                                    {
                                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                        lblMsg.Text = Result;
                                        return;
                                    }
                                    objOT = null;
                                }
                            }
                            Session["SurSvchk"] = 1;
                            //ClearForm();
                            //lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                            //lblMsg.Text = "Order saved successfully...";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                        }
                        else
                        {
                            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMsg.Text = "There is some error while saving order. " + hshOut["chvErrorStatus"].ToString();
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
                BaseBill = null; dt.Dispose();
            }
        }
        else if (common.myInt(Session["SurSvchk"]) == 1)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
        }
    }

    private DataTable UpdateDataTable()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CreateTableForProceed();
            int MainSurgeryId = 0;
            foreach (GridDataItem gitem in gvAddedSurgery.Items)
            {
                HiddenField hdnServiceId = (HiddenField)gitem.FindControl("hdnServiceId");
                HiddenField hdnServiceType = (HiddenField)gitem.FindControl("hdnServiceType");
                HiddenField hdnDoctorRequired = (HiddenField)gitem.FindControl("hdnDoctorRequired");
                HiddenField hdnDepartmentId = (HiddenField)gitem.FindControl("hdnDepartmentId");
                HiddenField hdnDoctorCharge = (HiddenField)gitem.FindControl("hdnDoctorCharge");
                HiddenField hdnDoctorDiscount = (HiddenField)gitem.FindControl("hdnDoctorDiscount");
                HiddenField hdnMainSurgeryId = (HiddenField)gitem.FindControl("hdnMainSurgeryId");
                HiddenField hdnIsMainSurgery = (HiddenField)gitem.FindControl("hdnIsMainSurgery");
                HiddenField hdnIsSurgeryService = (HiddenField)gitem.FindControl("hdnIsSurgeryService");
                HiddenField hdnSurgeonTypeId = (HiddenField)gitem.FindControl("hdnSurgeonTypeId");
                HiddenField hdnResourceType = (HiddenField)gitem.FindControl("hdnResourceType");
                HiddenField hdnServiceDiscountPerc = (HiddenField)gitem.FindControl("hdnServiceDiscountPerc");

                HiddenField hdntxtServiceCharge = (HiddenField)gitem.FindControl("hdntxtServiceCharge");
                HiddenField hdnServiceDiscount = (HiddenField)gitem.FindControl("hdnServiceDiscount");
                HiddenField hdnlblNetCharge = (HiddenField)gitem.FindControl("hdnlblNetCharge");
                HiddenField hdnlblPatientAmount = (HiddenField)gitem.FindControl("hdnlblPatientAmount");
                HiddenField hdnlblPayerAmount = (HiddenField)gitem.FindControl("hdnlblPayerAmount");
                HiddenField hdnlblGrossCharge = (HiddenField)gitem.FindControl("hdnlblGrossCharge");
                HiddenField hdnHighRiskSurgery = (HiddenField)gitem.FindControl("hdnHighRiskSurgery");
                HiddenField hdnHighRiskStat = (HiddenField)gitem.FindControl("hdnHighRiskStat");
                HiddenField hdnStat = (HiddenField)gitem.FindControl("hdnStat");

                //Added for Nanavati only  Start 
                HiddenField hdnActualGrossCharge = (HiddenField)gitem.FindControl("hdnActualGrossCharge");
                HiddenField hdnActualNetCharge = (HiddenField)gitem.FindControl("hdnActualNetCharge");
                HiddenField hdnSurgeryComponentId = (HiddenField)gitem.FindControl("hdnSurgeryComponentId");
                HiddenField hdnIsEmergency = (HiddenField)gitem.FindControl("hdnIsEmergency");
                HiddenField hdnEmergencyPerc = (HiddenField)gitem.FindControl("hdnEmergencyPerc");
                HiddenField hdnSurgeryId = (HiddenField)gitem.FindControl("hdnSurgeryId");
                HiddenField hdnTariffId = (HiddenField)gitem.FindControl("hdnTariffId");
                //Added for  Nanavati only End





                Label lblServiceName = (Label)gitem.FindControl("lblServiceName");
                Label lblNetCharge = (Label)gitem.FindControl("lblNetCharge");
                TextBox txtServiceCharge = (TextBox)gitem.FindControl("txtServiceCharge");
                Label lblPatientAmount = (Label)gitem.FindControl("lblPatientAmount");
                Label lblPayerAmount = (Label)gitem.FindControl("lblPayerAmount");
                RadComboBox ddlResourceName = (RadComboBox)gitem.FindControl("ddlResourceName");

                Label lblChargePercentage = (Label)gitem.FindControl("lblChargePercentage");

                MainSurgeryId = common.myInt(hdnMainSurgeryId.Value);
                string IsDoctorRequired = "False";
                if (common.myInt(hdnDoctorRequired.Value) == 1)
                {
                    IsDoctorRequired = "True";
                }
                ViewState["ResourcenameEmpty"] = 0;
                if (ddlResourceName.Visible && ddlResourceName.SelectedValue.Equals(""))
                {
                    ViewState["ResourcenameEmpty"] = 1;
                    return dt;
                }

                DataRow dr = dt.NewRow();
                dr["SNo"] = gitem.ItemIndex + 1;
                dr["DetailId"] = common.myInt(0);
                dr["OrderId"] = common.myInt(0);
                dr["ServiceId"] = common.myInt(hdnServiceId.Value);
                dr["UnderPackage"] = common.myInt(0);
                dr["PackageId"] = common.myInt(0);
                dr["DoctorID"] = common.myInt(ddlResourceName.SelectedValue);
                dr["DoctorRequired"] = common.myStr(IsDoctorRequired);
                dr["DepartmentId"] = common.myInt(hdnDepartmentId.Value);
                dr["ServiceType"] = common.myStr(hdnServiceType.Value);
                dr["ServiceName"] = common.myStr(lblServiceName.Text) + " ( " + hdnResourceType.Value + " ) ";
                dr["ServiceRemarks"] = common.myStr("  ");
                dr["Units"] = common.myInt(1);
                // dr["Charge"] = common.myDec(lblNetCharge.Text);
                dr["Charge"] = common.myDec(hdnlblNetCharge.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["GrossCharge"] = common.myDec(hdnlblGrossCharge.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                // dr["ServiceAmount"] = common.myDec(txtServiceCharge.Text);
                dr["ServiceAmount"] = common.myDec(hdntxtServiceCharge.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["DoctorAmount"] = common.myDec(hdnDoctorCharge.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["ServiceDiscountPercentage"] = common.myDec(hdnServiceDiscountPerc.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["ServiceDiscountAmount"] = common.myDec(hdnServiceDiscount.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["DoctorDiscountPercentage"] = common.myInt(0);
                dr["DoctorDiscountAmount"] = common.myDec(hdnDoctorDiscount.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["TotalDiscount"] = common.myDec(common.myDec(hdnServiceDiscount.Value) + common.myDec(hdnDoctorDiscount.Value)).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //dr["AmountPayableByPatient"] = common.myDec(lblPatientAmount.Text);
                //dr["AmountPayableByPayer"] = common.myDec(lblPayerAmount.Text);
                dr["AmountPayableByPatient"] = common.myDec(hdnlblPatientAmount.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["AmountPayableByPayer"] = common.myDec(hdnlblPayerAmount.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["IsPackageMain"] = common.myInt(0);
                dr["IsPackageService"] = common.myInt(0);
                dr["MainSurgeryId"] = common.myInt(hdnMainSurgeryId.Value);
                dr["IsSurgeryMain"] = common.myInt(hdnIsMainSurgery.Value);
                dr["IsSurgeryService"] = common.myInt(hdnIsSurgeryService.Value);
                dr["ResourceId"] = common.myInt(hdnSurgeonTypeId.Value);
                dr["ChargePercentage"] = common.myDec(lblChargePercentage.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["ServiceStatus"] = "  ";
                dr["IsExcluded"] = "  ";
                dr["IsApprovalReq"] = "  ";
                dr["CopayAmt"] = "0";
                dr["CopayPerc"] = "0";
                dr["IsCoPayOnNet"] = "0";
                dr["DeductableAmount"] = "0";

                dr["ActualGrossCharge"] = common.myInt(hdnActualGrossCharge.Value);// for Nanavati only
                dr["ActualNetCharge"] = common.myInt(hdnActualNetCharge.Value);// for Nanavati only
                dr["SurgeryComponentId"] = common.myInt(hdnSurgeryComponentId.Value);// for Nanavati only
                dr["IsEmergency"] = common.myBool(hdnIsEmergency.Value);// for Nanavati only
                dr["EmergencyPerc"] = common.myInt(hdnEmergencyPerc.Value);// for Nanavati only

                dr["ResourceType"] = common.myStr(hdnServiceType.Value) == "OT" ? hdnServiceType.Value : common.myStr(hdnServiceType.Value) == "AN" ? hdnServiceType.Value : common.myStr(hdnResourceType.Value);
                if (common.myStr(hdnHighRiskStat.Value) == "1")
                {
                    dr["HighRisk"] = "1";
                    dr["Stat"] = "1";
                }
                else
                {
                    dr["HighRisk"] = common.myInt(hdnHighRiskSurgery.Value);
                    dr["Stat"] = common.myInt(hdnStat.Value);
                }
                dr["SurgeryId"] = common.myInt(hdnSurgeryId.Value);
                dr["TariffId"] = common.myInt(hdnTariffId.Value);
                dt.Rows.Add(dr);
            }
            StringBuilder sbOT = new StringBuilder();
            ArrayList coll = new ArrayList();
            coll.Add(common.myInt(radCmbOtRoom.SelectedValue));
            coll.Add(common.myStr(rdtpOtStartTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            coll.Add(common.myStr(rdtpOtEndTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            coll.Add(common.myInt(radCmbAnaesthesia.SelectedValue));
            coll.Add(common.myStr(rdtpAstartTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            coll.Add(common.myStr(rdtpAEndTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            coll.Add(common.myInt(radCmbBedCategory.SelectedValue));
            coll.Add(common.myInt(MainSurgeryId));
            coll.Add(common.myBool(chkUnClean.Checked));
            sbOT.Append(common.setXmlTable(ref coll));
            hdnXmlSurgery.Value = sbOT.ToString();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        return dt;
    }

    protected DataTable CreateTableForProceed()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 1;
        dt.Columns["SNo"].AutoIncrementStep = 1;

        dt.Columns.Add("DetailId");
        dt.Columns.Add("ServiceId");//
        dt.Columns.Add("OrderId");
        dt.Columns.Add("UnderPackage");
        dt.Columns.Add("PackageId");
        dt.Columns.Add("ServiceType");//
        dt.Columns.Add("DoctorID");//
        dt.Columns.Add("DoctorRequired");//
        dt.Columns.Add("DepartmentId");//
        dt.Columns.Add("ServiceName");//
        dt.Columns.Add("Units");
        dt.Columns.Add("Charge");//
        dt.Columns.Add("ServiceAmount");//
        dt.Columns.Add("DoctorAmount");//
        dt.Columns.Add("ServiceDiscountPercentage");
        dt.Columns.Add("ServiceDiscountAmount");//
        dt.Columns.Add("DoctorDiscountPercentage");
        dt.Columns.Add("DoctorDiscountAmount");//
        dt.Columns.Add("TotalDiscount");//
        dt.Columns.Add("AmountPayableByPatient");//
        dt.Columns.Add("AmountPayableByPayer");//
        dt.Columns.Add("IsPackageMain");
        dt.Columns.Add("IsPackageService");
        dt.Columns.Add("MainSurgeryId");
        dt.Columns.Add("IsSurgeryMain");
        dt.Columns.Add("IsSurgeryService");
        dt.Columns.Add("ResourceId");
        dt.Columns.Add("ChargePercentage");
        dt.Columns.Add("ServiceStatus");
        dt.Columns.Add("IsExcluded");
        dt.Columns.Add("IsApprovalReq");
        dt.Columns.Add("ServiceRemarks");
        dt.Columns.Add("CopayAmt");
        dt.Columns.Add("CopayPerc");
        dt.Columns.Add("IsCoPayOnNet");
        dt.Columns.Add("DeductableAmount");

        dt.Columns.Add("ActualGrossCharge");//For Nanavati only
        dt.Columns.Add("ActualNetCharge");//For Nanavati only
        dt.Columns.Add("SurgeryComponentId");//For Nanavati only
        dt.Columns.Add("IsEmergency");//For Nanavati only
        dt.Columns.Add("EmergencyPerc");//For Nanavati only   
        dt.Columns.Add("GrossCharge");
        dt.Columns.Add("ResourceType");
        dt.Columns.Add("SurgeryId");
        dt.Columns.Add("HighRisk");
        dt.Columns.Add("Stat");
        dt.Columns.Add("TariffId");
        return dt;
    }

    private void ClearForm()
    {
        radCmbOtRoom.SelectedValue = "0";
        radCmbAnaesthesia.SelectedValue = "0";
        ddlOTEquipments.SelectedValue = "0";

        rdtpOtStartTime.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
        rdtpOtStartTime.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
        rdtpOtStartTime.SelectedDate = common.myDate(ViewState["OTCheckintime"]);

        rdtpOtEndTime.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
        rdtpOtEndTime.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
        rdtpOtEndTime.SelectedDate = common.myDate(ViewState["OTCheckouttime"]);

        lblTimeChkIn.Text = "(" + common.myStr(ViewState["OTChkInTimeStr"]) + ")";
        lblTimeChkOut.Text = "(" + common.myStr(ViewState["OTChkOutTimeStr"]) + ")";

        rdtpAstartTime.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
        rdtpAstartTime.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
        rdtpAstartTime.SelectedDate = common.myDate(DateTime.Now);

        rdtpAEndTime.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
        rdtpAEndTime.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
        rdtpAEndTime.SelectedDate = common.myDate(DateTime.Now);

        gvSurgery.DataSource = null;
        gvSurgery.DataBind();

        gvSurgery.Visible = false;
        btnUp.Visible = false;
        btnDown.Visible = false;

        gvAddedSurgery.DataSource = null;
        gvAddedSurgery.DataBind();

        gvResourceList.DataSource = null;
        gvResourceList.DataBind();

    }

    protected void rdtpOtStartTime_OnSelectedDateChanged(object sender, EventArgs e)
    {
        DateTime dtCurrentDate = DateTime.Now;
        DateTime dtSelectedDate = Convert.ToDateTime(rdtpOtStartTime.SelectedDate);
        lblInfoBillCategory.Text = "";
        if (dtSelectedDate < dtCurrentDate)
        {
            lblInfoBillCategory.Text = "(Billing category applicable as per the order date)";
        }
    }
    protected void radCmbDoctorClassification_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btnAddOtherSuregon_OnClick(sender, e);

        if (common.myInt(ViewState["DuplicateResourceID"]) == 1)
        {
            Alert.ShowAjaxMsg("Same resource cannot be selected multiple time...", this.Page);
            return;
        }

    }

    protected void ddlOrderMinutes_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(dtOrderDate.SelectedDate.Value.ToString());
        sb.Remove(dtOrderDate.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(dtOrderDate.SelectedDate.Value.ToString().IndexOf(":") + 1, ddlOrderMinutes.Text);
        dtOrderDate.SelectedDate = Convert.ToDateTime(sb.ToString());
    }

    private void BindMinutesor()
    {
        try
        {
            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                {
                    ddlOrderMinutes.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    ddlOrderMinutes.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
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

    protected void rdoIncision_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            BtnCalculateCharges_OnClick(null, null);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void radCmbsurgeryClassification_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        btnAddOtherSuregon_OnClick(sender, e);

        if (common.myInt(ViewState["DuplicateResourceID"]) == 1)
        {
            Alert.ShowAjaxMsg("Same resource cannot be selected multiple time...", this.Page);
            return;
        }
    }
    private void getHospitalFlagValue()
    {
        // get all flag value 
        BaseC.HospitalSetup objHospitalSetup = new BaseC.HospitalSetup(sConString);

        string flags = "'noofsurgeryflag','DefaultOPDCategoryService','DecimalPlaces','Ishiddingnoofsurgeryforallclients','IsGenerateAdvanceAgainstOrder','isHighRiskSurgeryRequired','HighRiskFlagDisplayName','IsAllowToEditSurgeryAmountOnlyForCash','setisAllDoctorDisplayOnAddService','IsSurgeryChargesForNanavati','DefaultAnServiceId','isShowOTRoomSelectedOnAddSurgery','DefaultHospitalCompany','EnableSurgeryBedCategoryForIP','isAnesthetistCalculateAfterAddSurgeonAndAssisSur','isRecalculateOTChargeOnIPSurgeryFrontEnd','isAssistantSurgeonCalculateonAmount','IsAllowAnesthesiaTypeWisePerc','isAllDoctorDisplayOnAddService','ISAnesthesiaTypeAndAnaesthesiaOtherChargeMandatory','isShowEmergencyCheckBoxForEmergencyCharge'";

        DataSet dsFlags = objHospitalSetup.getHospitalSetupValueMultiple(flags, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
        if (dsFlags.Tables.Count > 0)
        {
            if (dsFlags.Tables[0].Select("Flag = 'noofsurgeryflag'").Length > 0)
            {
                noofsurgeryflag = common.myStr(dsFlags.Tables[0].Select("Flag = 'noofsurgeryflag'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'DefaultOPDCategoryService'").Length > 0)
            {
                DefaultOPDCategoryService = common.myStr(dsFlags.Tables[0].Select("Flag = 'DefaultOPDCategoryService'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'DecimalPlaces'").Length > 0)
            {
                DecimalPlaces = common.myStr(dsFlags.Tables[0].Select("Flag = 'DecimalPlaces'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'Ishiddingnoofsurgeryforallclients'").Length > 0)
            {
                Ishiddingnoofsurgeryforallclients = common.myStr(dsFlags.Tables[0].Select("Flag = 'Ishiddingnoofsurgeryforallclients'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'IsGenerateAdvanceAgainstOrder'").Length > 0)
            {
                IsGenerateAdvanceAgainstOrder = common.myStr(dsFlags.Tables[0].Select("Flag = 'IsGenerateAdvanceAgainstOrder'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'isHighRiskSurgeryRequired'").Length > 0)
            {
                isHighRiskSurgeryRequired = common.myStr(dsFlags.Tables[0].Select("Flag = 'isHighRiskSurgeryRequired'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'HighRiskFlagDisplayName'").Length > 0)
            {
                HighRiskFlagDisplayName = common.myStr(dsFlags.Tables[0].Select("Flag = 'HighRiskFlagDisplayName'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'IsAllowToEditSurgeryAmountOnlyForCash'").Length > 0)
            {
                IsAllowToEditSurgeryAmountOnlyForCash = common.myStr(dsFlags.Tables[0].Select("Flag = 'IsAllowToEditSurgeryAmountOnlyForCash'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'setisAllDoctorDisplayOnAddService'").Length > 0)
            {
                setisAllDoctorDisplayOnAddService = common.myStr(dsFlags.Tables[0].Select("Flag = 'setisAllDoctorDisplayOnAddService'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'IsSurgeryChargesForNanavati'").Length > 0)
            {
                IsSurgeryChargesForNanavati = common.myStr(dsFlags.Tables[0].Select("Flag = 'IsSurgeryChargesForNanavati'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'DefaultAnServiceId'").Length > 0)
            {
                DefaultAnServiceId = common.myStr(dsFlags.Tables[0].Select("Flag = 'DefaultAnServiceId'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'isShowOTRoomSelectedOnAddSurgery'").Length > 0)
            {
                isShowOTRoomSelectedOnAddSurgery = common.myStr(dsFlags.Tables[0].Select("Flag = 'isShowOTRoomSelectedOnAddSurgery'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'DefaultHospitalCompany'").Length > 0)
            {
                DefaultHospitalCompany = common.myStr(dsFlags.Tables[0].Select("Flag = 'DefaultHospitalCompany'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'EnableSurgeryBedCategoryForIP'").Length > 0)
            {
                EnableSurgeryBedCategoryForIP = common.myStr(dsFlags.Tables[0].Select("Flag = 'EnableSurgeryBedCategoryForIP'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'isAnesthetistCalculateAfterAddSurgeonAndAssisSur'").Length > 0)
            {
                isAnesthetistCalculateAfterAddSurgeonAndAssisSur = common.myStr(dsFlags.Tables[0].Select("Flag = 'isAnesthetistCalculateAfterAddSurgeonAndAssisSur'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'isRecalculateOTChargeOnIPSurgeryFrontEnd'").Length > 0)
            {
                isRecalculateOTChargeOnIPSurgeryFrontEnd = common.myStr(dsFlags.Tables[0].Select("Flag = 'isRecalculateOTChargeOnIPSurgeryFrontEnd'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'isAssistantSurgeonCalculateonAmount'").Length > 0)
            {
                isAssistantSurgeonCalculateonAmount = common.myStr(dsFlags.Tables[0].Select("Flag = 'isAssistantSurgeonCalculateonAmount'")[0].ItemArray[1]);
            }

            if (dsFlags.Tables[0].Select("Flag = 'IsAllowAnesthesiaTypeWisePerc'").Length > 0)
            {
                IsAllowAnesthesiaTypeWisePerc = common.myStr(dsFlags.Tables[0].Select("Flag = 'IsAllowAnesthesiaTypeWisePerc'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'isAllDoctorDisplayOnAddService'").Length > 0)
            {
                isAllDoctorDisplayOnAddService = common.myStr(dsFlags.Tables[0].Select("Flag = 'isAllDoctorDisplayOnAddService'")[0].ItemArray[1]);
            }

            if (dsFlags.Tables[0].Select("Flag = 'ISAnesthesiaTypeAndAnaesthesiaOtherChargeMandatory'").Length > 0)
            {
                hdnISAnesthesiaTypeAndAnaesthesiaOtherChargeMandatory.Value = common.myStr(dsFlags.Tables[0].Select("Flag = 'ISAnesthesiaTypeAndAnaesthesiaOtherChargeMandatory'")[0].ItemArray[1]);
            }
            if (dsFlags.Tables[0].Select("Flag = 'isShowEmergencyCheckBoxForEmergencyCharge'").Length > 0)
            {
                isShowEmergencyCheckBoxForEmergencyCharge = common.myStr(dsFlags.Tables[0].Select("Flag = 'isShowEmergencyCheckBoxForEmergencyCharge'")[0].ItemArray[1]);
            }


        }
        objHospitalSetup = null;
    }

    public void FillAnesthesiaType()
    {
        if (string.Equals(IsAllowAnesthesiaTypeWisePerc.ToUpper(), "Y"))
        {
            lblAnesthesiaType.Visible = true;
            rbAnesthesiaType.Visible = true;
            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            DataSet ds = objBill.GetAnesthesiaType(common.myInt(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                rbAnesthesiaType.DataSource = ds.Tables[0];
                rbAnesthesiaType.DataTextField = "Name";
                rbAnesthesiaType.DataValueField = "ID";
                rbAnesthesiaType.DataBind();
                rbAnesthesiaType.Items.Insert(0, new RadComboBoxItem(" Select ", "0"));
                rbAnesthesiaType.SelectedIndex = 0;
            }
        }
        else
        {
            lblAnesthesiaType.Visible = false;
            rbAnesthesiaType.Visible = false;
        }
    }
}
