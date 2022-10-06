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

public partial class EMR_Assessment_CNMCodes : System.Web.UI.Page
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

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bC = new BaseC.ParseData();
    static DataTable DrugDt = new DataTable();
    DataSet ds = new DataSet();
    string pageId = "";
    

    public string Billable(string strbil)
    {
        if (strbil.ToUpper() == "TRUE")
        {
            return "Yes";
        }
        else if (strbil.ToUpper() == "FALSE")
        {
            return "";
        }
        return "";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Mpg"] != null)
        {
            pageId = Request.QueryString["Mpg"];
        }
        if (!IsPostBack)
        {
            hdnGridClientId.Value = icd.GridClientId;
            if (Session["OutputDateFormat"] != null)
            {
                rdpFrom.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                rdpTo.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            }
            string strEncounterDate = "";
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
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
            getCurrentICDCodes();
            BindICDPanel();

            BindFacility();
            BindProvider();
            BindFavorite("");
            //PopulateAllDrug("");
            BindAddServiceGrid();
            ViewState["BTN"] = "FAV";
            ModifierTable();
            hdnFavorite.Value = "0";
            btnDeleteFavorite.Visible = false;
            if (Request.QueryString["ID"] != "")
            {
                FillAddChargesPageControlsById(Convert.ToInt32(Request.QueryString["ID"]));
            }
        }
    }

    protected void lnkMedication_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PatientMedicationCharges.aspx", false);
    }

    protected void lnkDiagnosis_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PatientDiagnosisCharges.aspx?ID=0", false);
    }

    //protected void gvDrug_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gvDrug.SelectedIndex = -1;
    //    gvDrug.PageIndex = e.NewPageIndex;
    //    string strSRText = GetSearchText();
    //    PopulateAllDrug("");
    //}

    private void BindAddServiceGrid()
    {
        try
        {
            if (Session["EncounterID"] != null && Session["registrationId"] != null && Session["HospitalLocationID"] != null)
            {
                DAL.DAL dlSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
                hshIn.Add("@intRegistrationId", Convert.ToInt32(Session["registrationId"].ToString()));
                hshIn.Add("@intEncounterId", Convert.ToInt32(Session["EncounterID"].ToString()));
                //hshIn.Add("@intDoctorID", ddlProviders.SelectedValue);
                hshIn.Add("@intFacilityId", ddlFacility.SelectedValue);
                DataSet dsCPT = dlSave.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", hshIn);
                if (dsCPT.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsCPT.Tables[0];
                    DataView dv = dsCPT.Tables[0].DefaultView;
                    dv.RowFilter = "ServiceType = 'CL'";
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
                dr["ServiceName"] = DBNull.Value;
                dr["ModifierCode"] = DBNull.Value;
                dr["Units"] = DBNull.Value;
                dr["ServiceAmount"] = "0";
                dr["ServiceDiscountAmount"] = "0";
                dr["DoctorAmount"] = "0";
                dr["DoctorDiscountAmount"] = "0";
                dr["ICDID"] = DBNull.Value;
                dr["FromDate"] = DBNull.Value;
                dr["ToDate"] = DBNull.Value;
                dr["IsBillable"] = true;
                dr["SubName"] = DBNull.Value;
                dr["PullForwardEMCodes"] = DBNull.Value;
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

    protected void btnAddToList_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dlSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            hshIn.Add("@intRegistrationId", Convert.ToInt32(Session["registrationId"].ToString()));
            hshIn.Add("@intEncounterId", Convert.ToInt32(Session["EncounterID"].ToString()));
            hshIn.Add("@intDoctorID", ddlProviders.SelectedValue);
            hshIn.Add("@intFacilityId", ddlFacility.SelectedValue);
            DataSet dsCPT = dlSave.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", hshIn);
            if (dsCPT.Tables[0].Rows.Count > 0)
            {
                if (btnAddToList.Text != "Update List")
                {
                    for (int i = 0; i < dsCPT.Tables[0].Rows.Count; i++)
                    {
                        if (txtCPT.Text.ToString().Trim() == dsCPT.Tables[0].Rows[i]["CPTCode"].ToString().Trim())
                        {
                            Alert.ShowAjaxMsg("Service is already exist", Page);
                            return;
                        }
                    }
                }
            }
            if (Session["facilityId"] != null)
            {
                if (txtICDCode.Text.Trim() != "")
                {
                    if (txtCPT.Text.Trim() != "")
                    {
                        if (txtUnit.Text.Trim() != "")
                        {
                            if (Convert.ToDouble(txtUnit.Text) > 0)
                            {
                                if (txtUnitCharge.Text == "$0" || txtUnitCharge.Text.Trim().ToString() == "0")
                                {
                                    Alert.ShowAjaxMsg("You are entering the unit charge($0).", Page);
                                    goto SaveLabel;
                                }
                                else
                                {
                                    goto SaveLabel;
                                }
                            SaveLabel:
                                DL_Funs oDLF = new DL_Funs();
                                BaseC.Patient bc = new BaseC.Patient(sConString);

                                BaseC.EMROrders objEmrOrders = new BaseC.EMROrders(sConString);
                                StringBuilder objXML = new StringBuilder();

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
                                    // objXML.Append(txt.Text);
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

                                if (pageId != "")
                                {
                                    pageId = pageId.Substring(1, pageId.Length - 1);
                                }
                                else
                                    pageId = "0";

                                string sOrderID = "";
                                String strMsg = (string)objEmrOrders.SaveOPOrder(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"].ToString()), Convert.ToInt32(Session["DoctorID"].ToString()), Convert.ToInt16(Session["HospitalLocationId"]), DateTime.Now.ToString("MM/dd/yyyy"), objXML.ToString(), Convert.ToInt32(Session["UserId"]), false, Session["facilityId"].ToString(), pageId, chkPullaSuperbill.Checked, out sOrderID, 0);

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
                                //    }
                                //    else
                                //        Alert.ShowAjaxMsg("Unit Charge Should be greater than 0", Page);
                            }
                            else
                                Alert.ShowAjaxMsg("Unit Should be greater than 0", Page);
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

    private void ClearServiceDetailControls()
    {
        ViewState["ServiceId"] = null;
        txtCPT.Text = String.Empty;
        txtDescription.Text = String.Empty;
        txtICDCode.Text = String.Empty;
        txtModifier.Text = string.Empty;
        //rdpFrom.SelectedDate = DateTime.Now;
        //rdpTo.SelectedDate = DateTime.Now;
        txtSearch.Text = String.Empty;
        txtUnit.Text = String.Empty;
        txtUnitCharge.Text = String.Empty;
        chkIsBillable.Checked = true;
        btnAddToList.Text = "Add To List";
        chkPullaSuperbill.Checked = false;
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
            if (Convert.ToDouble(txtUnit.Text) > 0)
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
            else
            {
                Alert.ShowAjaxMsg("Unit Should be greater than 0", Page);
                return false;
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

    private DataTable CreateTable()
    {
        DataTable Dt = new DataTable();
        Dt.Columns.Add("Id", typeof(int));
        Dt.Columns.Add("ServiceID", typeof(int));
        Dt.Columns.Add("CPTCode", typeof(string));
        Dt.Columns.Add("ServiceName", typeof(string));
        Dt.Columns.Add("ModifierCode", typeof(string));
        Dt.Columns.Add("Units", typeof(Int16));
        Dt.Columns.Add("ServiceAmount", typeof(string));
        Dt.Columns.Add("ServiceDiscountAmount", typeof(string));
        Dt.Columns.Add("DoctorAmount", typeof(string));
        Dt.Columns.Add("DoctorDiscountAmount", typeof(string));
        Dt.Columns.Add("ICDID", typeof(string));
        Dt.Columns.Add("FromDate", typeof(string));
        Dt.Columns.Add("ToDate", typeof(string));
        Dt.Columns.Add("IsBillable", typeof(bool));
        Dt.Columns.Add("SubName", typeof(string));
        Dt.Columns.Add("PullForwardEMCodes", typeof(bool));
        return Dt;
    }

    protected void SearchDrug(string name, object sender, EventArgs e)
    {

    }

    //protected void gvDrug_OnRowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType != DataControlRowType.Pager)
    //        e.Row.Cells[0].Visible = false;
    //}

    private string getFacilityIdbyEncounterId()
    {
        string FacilityId = "0";
        try
        {
            if (Session["EncounterId"] != null)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hsInput = new Hashtable();
                hsInput.Add("EncounterId", Session["EncounterId"].ToString());
                FacilityId = (string)objDl.ExecuteScalar(CommandType.Text, "select Convert(varchar,FacilityID) FacilityId from Encounter where Id=@EncounterId", hsInput);
            }
            else
                return "0";
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return FacilityId;
    }

    private void BindCharge(Int32 iServiceId, TextBox txt1)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hshCompany = new Hashtable();
            Hashtable outputCompany = new Hashtable();
            hshCompany.Add("@intEncounterId", Session["EncounterId"]);
            hshCompany.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));

            outputCompany.Add("intCompanyId", SqlDbType.Int);
            outputCompany = dl.getOutputParametersValues(CommandType.StoredProcedure, "GetPatientCompanyCode", hshCompany, outputCompany);
            if (outputCompany["intCompanyId"].ToString() != "")
            {
                ViewState["CompanyCode"] = outputCompany["intCompanyId"].ToString();
            }
            else
            {
                ViewState["CompanyCode"] = 0;
            }

            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            hshIn.Add("@intRegistrationId", Convert.ToInt32(Session["registrationID"]));
            hshIn.Add("@intServiceId", iServiceId);
            hshIn.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));
            hshIn.Add("@intFacilityId", getFacilityIdbyEncounterId());
            // hshIn.Add("@intDoctorId", Convert.ToInt32("0"));
            hshIn.Add("@intDoctorId", ddlProviders.SelectedValue);
            hshIn.Add("@intSpecialisationId", Convert.ToInt32("0"));
            hshIn.Add("@chvModifierCode", txtModifier.Text);
            hshIn.Add("@intCompanyid", Convert.ToInt16(ViewState["CompanyCode"]));
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

    protected void gvDrug_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        string strtext = string.Empty;
        strtext = GetSearchText();
        if (ViewState["BTN"].ToString() == "ALL")
        {
            PopulateAllDrug(strtext);
        }
        else if (ViewState["BTN"].ToString() == "FAV")
        {
            BindFavorite(strtext);
        }
    }

    protected void gvDrug_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        //if (e.Item.ItemType == GridItemType.Item)
        //{
        //    HiddenField lblServiceIDGrid = (HiddenField)e.Item.FindControl("hdnServiceId");
        //}
    }

    protected void gvDrug_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtDescription.Text = gvDrug.SelectedItems[0].Cells[5].Text;
        txtDescription.Text = ((Label)gvDrug.SelectedItems[0].FindControl("lblServiceName")).Text;
        //ViewState["ServiceId"] = gvDrug.SelectedItems[0].Cells[3].Text;
        ViewState["ServiceId"] = ((HiddenField)gvDrug.SelectedItems[0].FindControl("hdnServiceId")).Value;
        txtCPT.Text = gvDrug.SelectedItems[0].Cells[3].Text;
        txtModifier.Text = String.Empty;
        txtSearch.Text = String.Empty;
        txtUnit.Text = "1";
        hdnID.Value = "0";
        txtUnitCharge.Text = String.Empty;
        //////rdpFrom.SelectedDate = DateTime.Now;
        //rdpTo.SelectedDate = DateTime.Now;
        chkIsBillable.Checked = true;
        btnAddToList.Text = "Add To List";
        BindCharge(Convert.ToInt32(ViewState["ServiceId"]), txtUnitCharge);
        chkPullaSuperbill.Checked = false;
        //if (txtICDCode.Text == "")
        //{
        //    if (cblICDCodes.Items.Count > 0)
        //    {
        //        txtICDCode.Focus();
        //    }
        //}
    }

    protected void gvAddService_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                if (Session["FacilityId"] != null && Session["HospitalLocationID"] != null && Session["RegistrationId"] != null && Session["EncounterId"] != null)
                {
                    HiddenField lblId = (HiddenField)e.Item.FindControl("hdnId");
                    string strId = lblId.Value;

                    if (!string.IsNullOrEmpty(strId))
                    {

                        Hashtable hstIn = new Hashtable();
                        if (pageId != "")
                            pageId = pageId.Substring(1, pageId.Length - 1);
                        else
                            pageId = "0";

                        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                        hstIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                        hstIn.Add("@intLoginFacilityId", Session["FacilityId"]);
                        hstIn.Add("@intRegistrationId", Session["RegistrationId"].ToString());
                        hstIn.Add("@intEncounterId", Session["EncounterId"].ToString());
                        hstIn.Add("@intPageId", pageId);
                        hstIn.Add("@intOrderId", strId);
                        hstIn.Add("@intEncodedBy", Session["UserId"].ToString());
                        dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeletePatientOrders", hstIn);
                    }
                    DataTable dt = new DataTable();
                    dt = (DataTable)Cache["ServiceDetail"];
                    dt.Rows.RemoveAt(e.Item.ItemIndex);
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
            }

            else if (e.CommandName == "Edit1")
            {
                HiddenField lblServiceIDGrid = (HiddenField)e.Item.FindControl("hdnServiceId");
                Label lblServiceName = (Label)e.Item.FindControl("lblServiceName");
                HiddenField lblModifierCodeGrid = (HiddenField)e.Item.FindControl("hdnModifierCode");
                Label lblUnits = (Label)e.Item.FindControl("lblUnits");
                Label lblUnitAmount = (Label)e.Item.FindControl("lblUnitAmount");
                HiddenField lblICDId = (HiddenField)e.Item.FindControl("hdnICDId");
                HiddenField chkIsBillableGrid = (HiddenField)e.Item.FindControl("hdnIsBillable");
                HiddenField lblFromDateGrid = (HiddenField)e.Item.FindControl("hdnFromDate");
                HiddenField lblToDateGrid = (HiddenField)e.Item.FindControl("hdnToDate");
                Label lblCPTCode = (Label)e.Item.FindControl("lblCPTCode");
                HiddenField hdnId1 = (HiddenField)e.Item.FindControl("hdnId");

                hdnID.Value = hdnId1.Value;
                ViewState["ServiceId"] = lblServiceIDGrid.Value.Trim();
                txtDescription.Text = lblServiceName.Text.Trim();
                txtModifier.Text = lblModifierCodeGrid.Value.Trim();
                txtCPT.Text = lblCPTCode.Text;
                txtICDCode.Text = lblICDId.Value.Trim();
                txtUnit.Text = lblUnits.Text.Trim();
                txtUnitCharge.Text = lblUnitAmount.Text.Trim();
                rdpFrom.SelectedDate = Convert.ToDateTime(lblFromDateGrid.Value.Trim());
                chkPullaSuperbill.Checked = Convert.ToBoolean(((HiddenField)e.Item.FindControl("hdnPullForward")).Value);


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

    protected void gvAddService_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        //if (e.Item.ItemType == GridItemType.Item)
        if (e.Item is GridDataItem)
        {
            HiddenField lblServiceIDGrid = (HiddenField)e.Item.FindControl("hdnServiceId");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkEdit");
            ImageButton ibtnDelete = (ImageButton)e.Item.FindControl("ibtnDelete");
            Label lblServiceNameGrid = (Label)e.Item.FindControl("lblServiceName");
            if (lblServiceNameGrid != null)
            {
                lblServiceNameGrid.ToolTip = lblServiceNameGrid.Text;
            }

            if (lblServiceIDGrid.Value == "0" || lblServiceIDGrid.Value == "")
            {
                lb.Enabled = false;
                ibtnDelete.Enabled = false;
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
                    sql = "SELECT ICDID, ICDCode, Description FROM ICD9SubDisease WHERE (ICDCode = @Diagnosis)";
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
            if (Session["EncounterId"] != null && Session["RegistrationID"] != null)
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
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void cmdSave_OnClick(object sender, EventArgs e)
    {
        try
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
                    objXML.Append(bc.FormatDate(datarow["FromDate"].ToString().Trim(), Session["OutputDateFormat"].ToString(), "dd/MM/yyyy"));
                    //objXML.Append(datarow["FromDate"].ToString());
                    objXML.Append("</c5><c6>");
                    objXML.Append(bc.FormatDate(datarow["ToDate"].ToString().Trim(), Session["OutputDateFormat"].ToString(), "dd/MM/yyyy"));

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
                String strMsg = "";
                // strMsg = (string)objEmrOrders.SaveOPOrder(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"].ToString()), Convert.ToInt32(Session["DoctorID"].ToString()), Convert.ToInt16(Session["HospitalLocationId"]), DateTime.Now.ToString("MM/dd/yyyy"), objXML.ToString(), Convert.ToInt32(Session["UserId"]), false);

                if (strMsg == "Saved")
                {
                    lbl_Msg.Text = "Record Saved!";
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
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void FillAddChargesPageControlsById(Int32 iID)
    {
        try
        {
            if (Cache["ServiceDetail"] != null)
            {

                DataTable dtServiceDetails = (DataTable)Cache["ServiceDetail"];
                DataView dv = new DataView(dtServiceDetails);
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
                    txtUnitCharge.Text = dt.Rows[0]["ServiceAmount"].ToString();
                    txtICDCode.Text = dt.Rows[0]["ICDID"].ToString();

                    hdnID.Value = iID.ToString();
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strtext = string.Empty;
        strtext = GetSearchText();

        if (ViewState["BTN"].ToString() == "ALL")
        {
            PopulateAllDrug(strtext);
        }
        else if (ViewState["BTN"].ToString() == "FAV")
        {
            BindFavorite(strtext);
        }
    }

    public string GetSearchText()
    {
        string strSearchCriteria = string.Empty;
        int iSelectedIndex = ddlOption.SelectedIndex;
        switch (iSelectedIndex)
        {
            case 0:
                strSearchCriteria = "%" + txtSearch.Text.ToString().Trim() + "%";
                break;
            case 1:
                strSearchCriteria = txtSearch.Text.ToString().Trim() + "%";
                break;
            case 2:
                strSearchCriteria = "%" + txtSearch.Text.ToString().Trim();
                break;
        }
        return strSearchCriteria;
    }

    private void PopulateAllDrug(string name)
    {
        try
        {
            if (Session["HospitalLocationId"] != null)
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshin = new Hashtable();
                hshin.Add("@inyHospitalLocationId", Session["HospitalLocationId"].ToString());
                hshin.Add("@chrType", "'CL'");
                hshin.Add("intFacilityId", common.myInt(Session["FacilityId"]));
                if (name.Length > 0)
                {
                    hshin.Add("@chvSearchCriteria", name);
                }
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServices", hshin);
                DrugDt = null;
                DrugDt = ds.Tables[0];
                gvDrug.DataSource = ds.Tables[0];
                gvDrug.DataBind();
                lbl_Msg.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnAllServices_Click(object sender, EventArgs e)
    {
        ViewState["BTN"] = "ALL";
        PopulateAllDrug("");
        hdnFavorite.Value = "0";
        lbl_Msg.Text = "";
        btnAddFavorite.Visible = true;
        btnDeleteFavorite.Visible = false;
    }

    protected void btnFavorites_Click(object sender, EventArgs e)
    {
        ViewState["BTN"] = "FAV";
        BindFavorite("");
    }

    private void BindFavorite(string name)
    {
        try
        {
            if (Session["DoctorId"] != null)
            {
                btnAddFavorite.Visible = false;
                btnDeleteFavorite.Visible = true;
                bool Exists = false;
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hsInput = new Hashtable();
                hsInput.Add("@intDoctorId", Session["DoctorId"].ToString());
                //hsInput.Add("@chvSearchCriteria", "%%");
                if (name.Length > 0)
                {
                    hsInput.Add("@chvSearchCriteria", name);
                }
                DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteServices", hsInput);
                foreach (DataColumn col in objDs.Tables[0].Columns)
                {
                    if (col.ColumnName == "SubName")
                    {
                        Exists = true;
                        break;
                    }
                }
                if (Exists == false)
                    objDs.Tables[0].Columns.Add("SubName", typeof(string));
                DataView objDv = objDs.Tables[0].DefaultView;
                objDv.RowFilter = "ServiceType = 'CL'";
                if (objDv.Count > 0)
                {
                    gvDrug.DataSource = objDv.ToTable();
                    gvDrug.DataBind();
                    hdnFavorite.Value = "1";
                    lbl_Msg.Text = "";
                }
                else
                {
                    gvDrug.DataSource = objDv.ToTable();
                    gvDrug.DataBind();
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

    // To Add in Favorite List
    protected void btnAddFavorite_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvDrug.SelectedItems.Count > 0)
            {
                if (hdnFavorite.Value != "1")
                {
                    HiddenField hdnServiceId = (HiddenField)gvDrug.SelectedItems[0].FindControl("hdnServiceId");
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    Hashtable hsInput = new Hashtable();
                    hsInput.Add("@intDoctorId", Session["DoctorId"].ToString());
                    hsInput.Add("@intServiceId", hdnServiceId.Value);
                    hsInput.Add("@intEncodedBy", Session["UserID"].ToString());
                    int i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRSaveFavService", hsInput);
                    if (i == 0)
                        lbl_Msg.Text = "Service Saved to Favourites";
                }
                else
                    Alert.ShowAjaxMsg("Already, available in favorite list", Page);

            }
            else
                Alert.ShowAjaxMsg("Select a Service to Add in Favourtes", Page);
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnDeleteFavorite_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvDrug.SelectedItems.Count > 0)
            {
                HiddenField hdnServiceId = (HiddenField)gvDrug.SelectedItems[0].FindControl("hdnServiceId");
                Hashtable hstInput = new Hashtable();
                hstInput.Add("@intDoctorId", Session["DoctorID"].ToString());
                hstInput.Add("@intServiceId", hdnServiceId.Value);
                hstInput.Add("@intEncodedBy", Session["UserID"]);
                DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                if (dal.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeleteFavService", hstInput) == 0)
                {
                    lbl_Msg.Text = "Service Deleted from Favourites";
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Select a service to delete from faavorites", this);
            }
            btnFavorites_Click(sender, null);
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ENMCodes.aspx", false);
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

    protected void BindFacility()
    {
        try
        {
            Hashtable hshInputs = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInputs.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInputs.Add("@chvFacilityType", "O");
            hshInputs.Add("@intUserId", Session["UserID"]);
            hshInputs.Add("@intGroupId", Session["GroupID"]);

            DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityList", hshInputs);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            ddlFacility.Items.Clear();
            ddlFacility.DataSource = objDs1;
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataBind();
            //ddlFacility.Items.Insert(0, new RadComboBoxItem("", "0"));
            RadComboBoxItem rcbFacilityId = (RadComboBoxItem)ddlFacility.Items.FindItemByText(Convert.ToString(Session["Facility"]));
            if (rcbFacilityId != null)
                rcbFacilityId.Selected = true;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindProvider()
    {
        try
        {
            Hashtable hshInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInput.Add("@intFacilityId", ddlFacility.SelectedValue);
            DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hshInput);

            ddlProviders.Items.Clear();
            ddlProviders.DataSource = objDs;
            ddlProviders.DataValueField = "DoctorId";
            ddlProviders.DataTextField = "DoctorName";
            ddlProviders.DataBind();
            //ddlProviders.Items.Insert(0, new RadComboBoxItem("", "0"));
            RadComboBoxItem rcbDoctorId = (RadComboBoxItem)ddlProviders.Items.FindItemByValue(Convert.ToString(Session["DoctorID"]));
            if (rcbDoctorId != null)
                rcbDoctorId.Selected = true;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlFacility_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindProvider();
    }

}