using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using Telerik.Web.UI;
using Resources;
using System.Configuration;
using System.Collections.Generic;
using BaseC;

public partial class EMRBILLING_ChangeDoctorInInvoice : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog exlog = new clsExceptionLog();
    EMRMasters bMstr;
    clsEMRBilling baseEBill;
    clsLabRequest objLabRequest;
    EMROrders objEMROrders;
    DAL.DAL dl;
    private const int ItemsPerRequest = 10;
    EMRBilling.clsOrderNBill BaseBill;
    clsExceptionLog objException = new clsExceptionLog();



    protected void Page_PreInit(object sender, System.EventArgs e)
    {
         if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
           // Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet dsStatus = new DataSet();
        try
        {
            if (!IsPostBack)
            {
                if (Session["InvoiceNo"] != null)
                {
                    if (common.myStr(Session["InvoiceNo"]) != string.Empty)
                    {
                        txtInvoicenoNo.Text = common.myStr(Session["InvoiceNo"]);
                    }
                }
                bindgrid();
              

                ddlPayParty.Enabled = false;
                ddlCompany.Enabled = false;
                ddlSponsor.Enabled = false;
                ddlCardId.Enabled = false;
                lnkInsuranceDetails.Enabled = false;
                gvService.Enabled = true;

                Session["chkOrd"] = 0;

                dsStatus = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "OPBILL", "");
                if (dsStatus != null)
                {
                    if (dsStatus.Tables[0].Rows.Count > 0)
                    {
                        dsStatus.Tables[0].DefaultView.RowFilter = "Code='BRS'";
                        ViewState["BRSColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                        dsStatus.Tables[0].DefaultView.RowFilter = "Code='BES'";
                        ViewState["BESColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                        dsStatus.Tables[0].DefaultView.RowFilter = "Code='BAR'";
                        ViewState["BARColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                    }
                }
                if ((common.myStr(Request.QueryString["regForDetails"]) != "") || (common.myStr(Request.QueryString["regNo"]) != ""))
                {
                    txtInvoicenoNo.Text = common.myStr(Request.QueryString["regForDetails"]);
                    if (common.myStr(Request.QueryString["regNo"]) != "")
                    {
                        txtInvoicenoNo.Text = common.myStr(Request.QueryString["regNo"]);
                    }
                    txtInvoicenoNo.Enabled = false;
                    btnSearchByUHID_OnClick(null, null);
                }
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Font.Bold = commonLabelSetting.cBold;
                if (commonLabelSetting.cFont != "")
                {
                    lblMessage.Font.Name = commonLabelSetting.cFont;
                }
                if (Request.QueryString["Mpg"] != null)
                {
                    Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                    string pid = Session["CurrentNode"].ToString();
                    int len = pid.Length;
                    ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, len - 1);
                }
                else
                {
                    ViewState["PageId"] = "0";
                }
                if (Request.QueryString["OPIP"] != null)
                {
                    hdnOPIP.Value = common.myStr(Request.QueryString["OPIP"]);
                }
                else
                {
                    hdnOPIP.Value = "O";
                }
                baseEBill = new clsEMRBilling(sConString);


                BindComboBox();

                dvConfirmPrintingOptions.Visible = false;
                txtInvoicenoNo.Focus();
            }

        }
        catch (Exception Ex)
        {
            PrintExeception(Ex);
        }
        finally
        {

            objval = null;
            dsStatus.Dispose();
            baseEBill = null;

        }
    }
    public DataSet DoctorBind()
    {
        DataSet ds = new DataSet();
        Hospital baseHosp = new Hospital(sConString);
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {

            if (common.myStr(ViewState["doctors"]) == "")
            {

                ds = baseHosp.fillDoctorCombo(Convert.ToInt16(Session["HospitalLocationID"].ToString()), 0, Convert.ToInt16(Session["FacilityId"].ToString()));
                ViewState["doctors"] = ds;
            }
            else
            {
                ds = (DataSet)ViewState["doctors"];
            }
            return ds;
        }
        catch (Exception Ex)
        {

            PrintExeception(Ex);
            return ds;
        }
        finally
        {
            ds.Dispose();
            dl = null;
            baseHosp = null;
        }

    }
    protected void btnSearchByUHID_OnClick(object sender, EventArgs e)
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataRow dr;
        try
        {
            int HospId = common.myInt(Session["HospitalLocationID"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int RegId = 0;
            int EncounterId = 0;
            int EncodedBy = common.myInt(Session["UserId"]);
            lblMessage.Text = "";
            if (txtInvoicenoNo.Text != "")
            {

                Hashtable hshIn = new Hashtable();
                Hashtable hshOut = new Hashtable();

                hshIn.Add("@facilityid", common.myInt(Session["FacilityId"]));
                hshIn.Add("@Hosplocationid", Convert.ToInt32(Session["HospitalLocationID"]));
                if (common.myStr(ddlSelect.SelectedValue) == "1")
                {
                    hshIn.Add("@invoiceno", txtInvoicenoNo.Text);
                }
                else
                {
                    hshIn.Add("@orderNo", txtInvoicenoNo.Text);
                }


                ds1 = dl.FillDataSet(CommandType.StoredProcedure, "FillServicesForDoctorChange", hshIn, hshOut);

                if (ds1.Tables[0].Rows.Count > 0)
                {


                    ds = bC.getPatientDetails(HospId, FacilityId, common.myInt(ds1.Tables[0].Rows[0]["RegistrationId"]), common.myInt(ds1.Tables[0].Rows[0]["RegistrationNo"]), common.myInt(ds1.Tables[0].Rows[0]["EncounterId"]), EncodedBy);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            dr = ds.Tables[0].Rows[0];
                            if ((common.myStr(dr["CardValidDate"]) != "" && common.myBool(dr["IsInsuranceCompany"]) == true))
                            {
                                int j = DateTime.Compare(common.myDate(dr["CardValidDate"]), DateTime.Now);
                                if (j < 0)
                                {
                                    hdnCardValidDate.Value = common.myStr(dr["CardValidDate"]);
                                    ViewState["CardValidDate"] = hdnCardValidDate.Value;
                                    Alert.ShowAjaxMsg("Patient Insurance Card Date is Not valid", Page.Page);
                                }
                            }
                            hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                            hdnPatientName.Value = common.myStr(dr["PatientName"]);
                            hdnAgeGender.Value = common.myStr(dr["GenderAge"]);
                            hdnDOB.Value = common.myStr(dr["DOB"]);
                            hdnPhoneHome.Value = common.myStr(dr["PhoneHome"]);
                            hdnMobileNo.Value = common.myStr(dr["MobileNo"]);
                            hdnEncounterId.Value = common.myStr(dr["EncounterId"]);
                            hdnEncounterNo.Value = common.myStr(dr["EncounterNo"]);
                            hdnEncounterDate.Value = common.myStr(dr["EncounterDate"]);


                            foreach (DataRowView drEncounter in ds.Tables[1].DefaultView)
                            {
                                RadComboBoxItem item = new RadComboBoxItem();
                                item.Text = (string)drEncounter["EncounterNo"];
                                item.Value = drEncounter["Id"].ToString();
                                item.Attributes.Add("DoctorId", common.myStr(drEncounter["DoctorId"]));
                                item.DataBind();

                            }



                            if (common.myStr(dr["CompanyCode"]) != "")
                            {
                                hdnCompanyCode.Value = common.myStr(dr["CompanyCode"]);
                                hdnInsCode.Value = common.myStr(dr["InsuranceCode"]);
                                hdnSponsorName.Value = common.myStr(dr["SponsorName"]);
                                hdnPayerName.Value = common.myStr(dr["PayerName"]);
                                hdnCompanyType.Value = common.myStr(dr["EncounterCompanyType"]);
                            }
                            else
                            {
                                hdnCompanyCode.Value = common.myStr(dr["PatientSponsorId"]);
                                hdnInsCode.Value = common.myStr(dr["PatientPayorId"]);
                                hdnSponsorName.Value = common.myStr(dr["PatientSponsorName"]);
                                hdnPayerName.Value = common.myStr(dr["PatientPayername"]);
                                hdnCompanyType.Value = common.myStr(dr["PatientCompanyType"]);
                            }
                            ShowPatientCompany(ds);
                            PatientQView1.ShowPatientDetails(Convert.ToInt32(hdnRegistrationId.Value));
                            //BindGrid();
                            gvService.DataSource = ds1.Tables[0];
                            gvService.DataBind();
                            txtInvoicenoNo.ReadOnly = true;

                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                            hdnRegistrationId.Value = "0";
                            hdnPatientName.Value = "";
                            hdnAgeGender.Value = "";
                            hdnDOB.Value = "";
                            hdnPhoneHome.Value = "";
                            hdnMobileNo.Value = "";
                            hdnEncounterId.Value = "0";
                            hdnEncounterNo.Value = "";
                            hdnEncounterDate.Value = "";
                            hdnCompanyCode.Value = "0";
                            hdnInsCode.Value = "0";
                            hdnSponsorName.Value = "";
                            hdnPayerName.Value = "";
                            lblMessage.Text = ddlSelect.SelectedItem.Text + " not found";
                            return;
                        }
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                        hdnRegistrationId.Value = "0";
                        hdnPatientName.Value = "";
                        hdnAgeGender.Value = "";
                        hdnDOB.Value = "";
                        hdnPhoneHome.Value = "";
                        hdnMobileNo.Value = "";
                        hdnEncounterId.Value = "0";
                        hdnEncounterNo.Value = "";
                        hdnEncounterDate.Value = "";
                        hdnCompanyCode.Value = "0";
                        hdnInsCode.Value = "0";
                        hdnSponsorName.Value = "";
                        hdnPayerName.Value = "";
                        lblMessage.Text = ddlSelect.SelectedItem.Text + " not found";
                        return;
                    }

                }
                else
                {
                    lblMessage.Text = ddlSelect.SelectedItem.Text + " not found";
                }
                ds1.Dispose();
            }
        }
        catch (Exception Ex)
        {
            PrintExeception(Ex);
        }
        finally
        {

            bParse = null;
            bC = null;
            ds.Dispose();
            dr = null;

        }
    }
    void bindgrid()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds2 = new DataSet();
        Hashtable hshIn = new Hashtable();
        Hashtable hshOut = new Hashtable();

        hshIn.Add("@facilityid", common.myInt(Session["FacilityId"]));
        hshIn.Add("@Hosplocationid", Convert.ToInt32(Session["HospitalLocationID"]));
        if (common.myStr(ddlSelect.SelectedValue) == "1")
        {
            hshIn.Add("@invoiceno", txtInvoicenoNo.Text);
        }
        else
        {
            hshIn.Add("@orderNo", txtInvoicenoNo.Text);
        }



        ds2 = dl.FillDataSet(CommandType.StoredProcedure, "FillServicesForDoctorChange", hshIn, hshOut);

        if (ds2.Tables[0].Rows.Count > 0)
        {
            gvService.DataSource = ds2.Tables[0];
            gvService.DataBind();

        }
        ds2.Dispose();
    }
    void ShowPatientCompany(DataSet ds)
    {

        try
        {
            int DefaulltComapny = common.myInt(Cache["DefaultHospitalCompanyId"]);
            if (ds.Tables[0].Rows.Count > 0)
            {

                if (common.myBool(ds.Tables[0].Rows[0]["IsInsuranceCompany"]) == true)
                {
                    ddlPayParty.SelectedIndex = 2;

                }
                else if (common.myInt(ds.Tables[0].Rows[0]["PatientPayorId"]) == DefaulltComapny)
                {
                    ddlPayParty.SelectedIndex = 0;
                }
                else
                {
                    ddlPayParty.SelectedIndex = 1;
                }

                ddlPayParty_OnSelectedIndexChanged(null, null);
                ddlCompany.Text = "";
                ddlSponsor.Text = "";
                ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["PatientPayorId"])));
                ddlCompany_OnSelectedIndexChanged(null, null);
                ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["PatientSponsorId"])));
                if (ddlCardId.Items.IndexOf(ddlCardId.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["RegCardId"]))) < 0)
                {
                    ddlCardId.SelectedIndex = -1;
                }
                else
                {
                    ddlCardId.SelectedIndex = ddlCardId.Items.IndexOf(ddlCardId.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["RegCardId"])));
                }

            }

        }

        catch (Exception Ex)
        {
            PrintExeception(Ex);
        }



    }
    public void fillInsuredCompanyCardList()
    {
        DataSet ds = new DataSet();
        ManageInsurance miObj = new ManageInsurance();
        try
        {

            ddlCardId.Items.Clear();
            ddlCardId.Text = "";

            ds = miObj.fillCardtype(ddlCompany.SelectedValue, common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCardId.Items.Clear();
                ddlCardId.Text = "";
                ds.Tables[0].DefaultView.RowFilter = "Active='ACTIVE'";
                ddlCardId.Items.Add(new RadComboBoxItem(""));
                foreach (DataRowView dsCard in ds.Tables[0].DefaultView)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dsCard["CardType"];
                    item.Value = dsCard["CardId"].ToString();
                    item.Attributes.Add("CoPaymentMaxLimit", common.myStr(dsCard["CoPaymentMaxLimit"]));
                    item.Attributes.Add("DeductableMaxLimit", common.myStr(dsCard["DeductableMaxLimit"]));
                    ddlCardId.Items.Add(item);
                    item.DataBind();

                }

                ddlCardId.SelectedIndex = 0;
                ViewState["CoPaymentMaxLimit"] = common.myStr(ddlCardId.SelectedItem.Attributes["CoPaymentMaxLimit"]);
                ViewState["DeductableMaxLimit"] = common.myStr(ddlCardId.SelectedItem.Attributes["DeductableMaxLimit"]);
            }
            else
            {
                ddlCardId.Items.Clear();
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
            miObj = null;
        }
    }

    protected void ddlPayParty_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCompany.Text = "";
            ddlSponsor.Text = "";
            if (ddlPayParty.SelectedValue.ToString() == "0")
            {
                InsurancecompanyBind("C", 0, false);


            }
            else if (ddlPayParty.SelectedValue.ToString() == "1")
            {
                InsurancecompanyBind("C", 0, true);
            }
            else
            {
                InsurancecompanyBind("I", 0, false);
                lnkInsuranceDetails.Visible = true;
                ddlCardId.Width = Unit.Percentage(50);

            }

        }
        catch (Exception Ex)
        {
            PrintExeception(Ex);
        }



    }
    void InsurancecompanyBind(string sType, Int32 iRegID, bool companyCase)
    {
        DataSet ds = new DataSet();
        clsEMRBilling baseEBill = new clsEMRBilling(sConString);
        try
        {
            ddlCompany.Items.Clear();
            ddlSponsor.Items.Clear();
            ddlCompany.Items.Clear();
            ddlSponsor.Text = "";


            ds = baseEBill.getCompanyList(Convert.ToInt32(Session["HospitalLocationId"]), sType, 0, common.myInt(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (companyCase)
                    {
                        int DefaulltComapny = common.myInt(Cache["DefaultHospitalCompanyId"]);
                        ds.Tables[0].DefaultView.RowFilter = "CompanyId <>" + DefaulltComapny;
                    }
                }
            }

            foreach (DataRowView drReferal in ds.Tables[0].DefaultView)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)drReferal["Name"];
                item.Value = drReferal["CompanyId"].ToString();
                item.Attributes.Add("PaymentType", common.myStr(drReferal["PaymentType"]));
                item.Attributes.Add("StatusId", common.myStr(drReferal["StatusId"]));
                ddlCompany.Items.Add(item);
                item.DataBind();

            }

            if (ddlCompany.Items.Count == 0)
            {
                ddlCompany.Items.Clear();
                ddlCompany.Text = "";
            }
            ViewState["Company"] = ds.Tables[0];
            ddlCompany.SelectedIndex = 0;
            ddlCompany_OnSelectedIndexChanged(null, null);




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
            baseEBill = null;
        }

    }
    protected void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fillSponsor();
            fillInsuredCompanyCardList();

        }
        catch (Exception Ex)
        {
            PrintExeception(Ex);
        }
    }
    protected void ddlCardId_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["CoPaymentMaxLimit"] = common.myStr(ddlCardId.SelectedItem.Attributes["CoPaymentMaxLimit"]);
        ViewState["DeductableMaxLimit"] = common.myStr(ddlCardId.SelectedItem.Attributes["DeductableMaxLimit"]);

    }
    public void fillSponsor()
    {
        DataSet ds = new DataSet();
        Hashtable hsInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        clsEMRBilling baseEBill = new clsEMRBilling(sConString);
        try
        {
            ViewState["CoPaymentMaxLimit"] = common.myStr(ddlCompany.SelectedItem.Attributes["CoPaymentMaxLimit"]);
            ViewState["DeductableMaxLimit"] = common.myStr(ddlCompany.SelectedItem.Attributes["DeductableMaxLimit"]);
            if (ddlPayParty.SelectedValue.ToString() == "2")
            {
                ddlSponsor.Text = "";
                ddlSponsor.Items.Clear();

                BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                ds = objBill.getCompanyList(common.myInt(Session["HospitalLocationID"]), "IC", common.myInt(ddlCompany.SelectedValue), common.myInt(Session["FacilityId"]));

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlSponsor.SelectedIndex = -1;
                        ddlSponsor.Text = "";
                        ddlSponsor.Items.Clear();
                        ddlSponsor.DataSource = ds;
                        ddlSponsor.DataTextField = "Name";
                        ddlSponsor.DataValueField = "CompanyId";
                        ddlSponsor.DataBind();
                    }
                }

            }
            else if (ddlPayParty.SelectedValue.ToString() == "0")
            {

                ddlCompany.SelectedValue = baseEBill.getDefaultCompany(Convert.ToInt32(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])).ToString();
                ddlSponsor.Text = "";
                ddlSponsor.Items.Clear();
                ddlSponsor.DataSource = (DataTable)ViewState["Company"];
                ddlSponsor.DataTextField = "Name";
                ddlSponsor.DataValueField = "CompanyId";
                ddlSponsor.DataBind();
                ddlSponsor.SelectedValue = ddlCompany.SelectedValue.ToString();

            }
            else if (ddlPayParty.SelectedValue.ToString() == "1")
            {

                ddlSponsor.Text = "";
                ddlSponsor.Items.Clear();
                ddlSponsor.DataSource = (DataTable)ViewState["Company"];
                ddlSponsor.DataTextField = "Name";
                ddlSponsor.DataValueField = "CompanyId";
                ddlSponsor.DataBind();
                ddlSponsor.SelectedValue = ddlCompany.SelectedValue.ToString();
                ddlSponsor.Enabled = true;

            }
        }

        catch (Exception ex)
        {

            PrintExeception(ex);
        }
        finally
        {
            ds.Dispose();
            hsInput = null;
            dl = null;
            baseEBill = null;

        }



    }
    protected void btnSearchPatient_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        btnSearchByUHID_OnClick(null, null);
    }
    protected void BindComboBox()
    {
        DataSet ds = new DataSet();
        Hospital baseHosp = new Hospital(sConString);
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {

            bMstr = new BaseC.EMRMasters(sConString);
            ds = new DataSet();
            DataTable dt=new DataTable(); 
            if (common.myStr(ViewState["doctors"]) == "")
            {

                ds = baseHosp.fillDoctorCombo(Convert.ToInt16(Session["HospitalLocationID"].ToString()), 0, Convert.ToInt16(Session["FacilityId"].ToString()));
                ViewState["doctors"] = ds.Tables[0];
            }
            else
            {
                if (ViewState["doctors"] != null)
                {
                    //ds = ViewState["doctors"];
                    dt = (DataTable)ViewState["doctors"];

                }
            }

        }
        catch (Exception Ex)
        {
            PrintExeception(Ex);
        }
        finally
        {
            ds.Dispose();
            dl = null;
            baseHosp = null;
        }


    }

    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataView dvdup = new DataView();
        BaseC.RestFulAPI objBilling = new BaseC.RestFulAPI(sConString);

        EMRBilling.clsOrderNBill BaseBill = new EMRBilling.clsOrderNBill(sConString);
        Hashtable hshServiceDetail = new Hashtable();

        try
        {

            int maxId = 0;
            string[] stringSeparators_ShowDia = new string[] { "##" };


            dt = (DataTable)ViewState["OPServicesInv_"];
            if (dt.Rows.Count > 0)
            {
                if (common.myInt(dt.Rows[0][2]) == 0) //If serviceid = 0 then remove row
                {
                    dt.Rows.Clear();
                }
                else
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "Sno Desc";
                    maxId = common.myInt(dv[0]["Sno"]);
                }
                //Check duplicate service------------------------------------------------------------------

                dvdup = dt.Copy().DefaultView;



                if (dvdup.ToTable().Rows.Count > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Service already exist !";


                    return;
                }
            }

            ViewState["OPServicesInv_"] = dt;
            if (dt.Rows.Count > 0)
            {
                ddlPayParty.Enabled = false;
                ddlCompany.Enabled = false;
                ddlSponsor.Enabled = false;
                ddlCardId.Enabled = false;
                lnkInsuranceDetails.Enabled = false;
            }

            gvService.DataSource = dt;
            gvService.DataBind();

            lblMessage.Text = "";
            dvConfirmPrintingOptions.Visible = false;

        }
        catch (Exception Ex)
        {
            PrintExeception(Ex);
        }
        finally
        {
            dt.Dispose();
            dvdup.Dispose();
            objBilling = null;
            BaseBill = null;
            hshServiceDetail = null;
        }
    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmPrintingOptions.Visible = false;
    }
    protected void gvService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        try
        {

            if (e.Row.RowType != DataControlRowType.Pager)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#5EA0F4";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RadComboBox ddlDoctor = (RadComboBox)e.Row.FindControl("ddlDoctor");
                ddlDoctor.Visible = true;

                if ((DataTable)ViewState["doctors"] == null)
                {
                    Hospital baseHosp = new Hospital(sConString);
                    ds = baseHosp.fillDoctorCombo(Convert.ToInt16(Session["HospitalLocationID"].ToString()), 0, Convert.ToInt16(Session["FacilityId"].ToString()));

                    ViewState["doctors"] = ds.Tables[0];
                }
                else
                    dt = (DataTable)ViewState["doctors"];

                if (dt.Rows.Count > 0)
                {
                    ddlDoctor.Items.Clear();
                    ddlDoctor.DataSource = dt;
                    ddlDoctor.DataValueField = "DoctorId";
                    ddlDoctor.DataTextField = "DoctorName";
                    ddlDoctor.DataBind();
                    ddlDoctor.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
                    ddlDoctor.SelectedIndex = 0;
                }

            }

        }




        catch (Exception Ex)
        {
            PrintExeception(Ex);
        }
        finally
        {
            ds.Dispose();
            dt.Dispose();

        }
    }
    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {


        }
        catch (Exception Ex)
        {
            PrintExeception(Ex);
        }
        finally
        {
            objDl = null;


        }


    }

    protected void PrintExeception(Exception Ex)
    {
        clsExceptionLog exlog = new clsExceptionLog();
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        lblMessage.Text = "Error: " + Ex.Message;
        exlog.HandleException(Ex);
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        #region CheckUserPermission

        UserAuthorisations ua = new UserAuthorisations();

        string message = string.Empty;
        if (!ua.CheckPermission("N", Request.Url.AbsolutePath, ref message))
        {
            Alert.ShowAjaxMsg(message, Page.Page);
            return;
        }
        ua.Dispose();
        #endregion

        try
        {

            if (common.myStr(txtInvoicenoNo.Text) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select a patient !";
                return;
            }



            Hashtable hshInput = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            StringBuilder strXML = new StringBuilder();
            ArrayList colService = new ArrayList();
            foreach (GridViewRow row in gvService.Rows)
            {
                int serviceid = 0;
                int id = 0;
                HiddenField hdn = (HiddenField)row.FindControl("hdnServiceId");
                HiddenField hdnid = (HiddenField)row.FindControl("hdnid");
                RadComboBox ddlDoctor = (RadComboBox)row.FindControl("ddlDoctor");

                HiddenField hdnINVOICEID = (HiddenField)row.FindControl("hdnINVOICEID");
                HiddenField hdnolddoctor = (HiddenField)row.FindControl("hdnolddoctor");
                HiddenField hdnOrderId = (HiddenField)row.FindControl("hdnOrderId");
                TextBox txtSoc = (TextBox)row.FindControl("txtSoc");


                serviceid = common.myInt(hdn.Value);
                id = common.myInt(hdnid.Value);
                if ((serviceid != 0 & common.myInt(ddlDoctor.SelectedValue) > 0) || (common.myInt(txtSoc.Text) > 0))
                {
                    colService.Add(common.myInt(serviceid)); //ServiceId
                    colService.Add(common.myStr(ddlDoctor.SelectedValue)); //DoctorId
                    colService.Add(row.RowIndex + 1); //SeQNo

                    colService.Add(common.myInt(id));//id
                    colService.Add(common.myInt(hdnINVOICEID.Value));//iinvoiceid
                    colService.Add(common.myInt(hdnolddoctor.Value));//olddoctor
                    colService.Add(common.myInt(hdnOrderId.Value));//olddoctor
                    colService.Add(common.myInt(txtSoc.Text));//txtSoc
                    strXML.Append(common.setXmlTable(ref colService));

                }
            }
            if (strXML.ToString().Trim().Length > 1)
            {

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();


                HshIn.Add("@intHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
                HshIn.Add("@intFacilityId", common.myInt(ViewState["FacilityId"]));
                HshIn.Add("@intEncodedBy", common.myInt(Session["UserId"]));
                HshIn.Add("@XMLService", strXML.ToString());

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspsaveChangeDoctorInInvoice", HshIn, HshOut);

                string strmsg = HshOut["@chvErrorStatus"].ToString();

                if ((strmsg.Contains(" Update") || strmsg.Contains(" Save")) && !strmsg.Contains("usp"))
                {

                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                }

                lblMessage.Text = strmsg;
                bindgrid();
            }

        }
        catch (Exception Ex)
        {

            PrintExeception(Ex);
        }
        finally
        {
            BaseBill = null;
        }

    }
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        try
        {
            #region CheckUserPermission

            UserAuthorisations ua = new UserAuthorisations();

            string message = string.Empty;
            if (!ua.CheckPermission("N", Request.Url.AbsolutePath, ref message))
            {
                Alert.ShowAjaxMsg(message, Page.Page);
                return;
            }
            ua.Dispose();
            #endregion
            Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
        }
        catch (Exception Ex)
        {
            PrintExeception(Ex);
        }
    }


    protected void btnFillInsurance_Click(object sender, EventArgs e)
    {
        if (common.myStr(hdnCardValidDate.Value) == "")
        {
            hdnCardValidDate.Value = common.myStr(ViewState["CardValidDate"]);
        }
        else
        {
            ViewState["CardValidDate"] = hdnCardValidDate.Value;
        }
        if (common.myInt(hdnPayer.Value) != 0)
        {
            ddlPayParty.SelectedValue = "2";
            ddlCompany.SelectedValue = hdnPayer.Value;
            ddlCompany_OnSelectedIndexChanged(null, null);
            ddlSponsor.SelectedValue = hdnSponsor.Value;
            ddlCardId.SelectedValue = hdnCardId.Value;
        }
    }

}
