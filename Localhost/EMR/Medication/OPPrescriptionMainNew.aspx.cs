using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;

public partial class EMR_Medication_OPPrescriptionMainNew : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    clsCIMS objCIMS = new clsCIMS();

    private enum enumColumns : byte
    {
        Sno = 0,
        Select = 1,
        storename = 2,
        OrderNo = 3,
        OrderDate = 4,
        IssuedItemName = 5,
        DrugName = 6,
        IndentType = 7,
        TotalQty = 8,
        PrescriptionDetail = 9,
        StartDate = 10,
        EndDate = 11,
        BrandDetailsCIMS = 12,
        MonographCIMS = 13,
        InteractionCIMS = 14,
        DIInteractionCIMS = 15,
        DHInteractionCIMS = 16,
        DAInteractionCIMS = 17,
        BrandDetailsVIDAL = 18,
        MonographVIDAL = 19,
        InteractionVIDAL = 20,
        DHInteractionVIDAL = 21,
        DAInteractionVIDAL = 22,
        Source = 23,
        IssueQty = 24,
        Approved = 25,
        Print = 26,
        StopBy = 27,
        StopDateTime = 28,
        StopRemarks = 29,
        Cancel = 30,
        Stop = 31,
        IndentClose = 32
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Convert.ToString(Request.QueryString["Mpg"]) == "P168")
        if (common.myStr(Request.QueryString["OPIP"]) == "O")
        {
            ViewState["OPIP"] = "O";
        }
        else
        {
            ViewState["OPIP"] = "I";
        }

        if (common.myInt(Session["EncounterId"]) == 0)
        {
            Response.Redirect("/default.aspx?RegNo=0", false);
        }

        if (!IsPostBack)
        {
            if (common.myStr(Session["OPIP"]) != "I")
            {
                var bli = rdoPreviousCurrent.Items[3];
                bli.Attributes.Add("hidden", "hidden");
            }

            ViewState["IndentDetailsId"] = string.Empty;
            ViewState["StopItemId"] = string.Empty;
            ViewState["StopIndentId"] = string.Empty;
            ViewState["CancelBrandList"] = string.Empty;
            Session["FacilityName"] = Session["FacilityName"];
            btnCancel.Visible = false;
            //Add By Himanshu On Date 17/02/2022 
            btnPrint.Visible = true;

            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                rdoPreviousCurrent.Visible = true;
            }
            else
            {
                rdoPreviousCurrent.Visible = false;
            }
            ViewState["IsCancelWithOutRemarksInEmrPreviousMedication"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                            common.myInt(Session["FacilityId"]), "IsCancelWithOutRemarksInEmrPreviousMedication", sConString);

            dvConfirmStop.Visible = false;
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtFromDate.SelectedDate = DateTime.Now.AddDays(-30);

            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.SelectedDate = DateTime.Now;

            //ViewState["GridData"] = null;

            bindControl();

            #region Interface

            objCIMS = new clsCIMS();
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

            DataSet dsInterface = new DataSet();

            setPatientInfo();

            dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.InterfaceForEMRDrugOrder);

            if (dsInterface.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]))
                {
                    Session["IsCIMSInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]);
                    Session["CIMSDatabasePath"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                    Session["CIMSDatabasePassword"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePassword"]);
                }
                else if (common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]))
                {
                    Session["IsVIDALInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]);
                }

                getLegnedColor();
            }

            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                string CIMSDatabasePath = string.Empty;
                if (dsInterface.Tables[0].Rows.Count > 0)
                {
                    CIMSDatabasePath = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                }

                if (!File.Exists(CIMSDatabasePath + "FastTrackData.mrc") && !File.Exists(CIMSDatabasePath + "FastTrackData.mr2"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                    lblMessage.Text = "CIMS database not available !";
                    //Alert.ShowAjaxMsg("CIMS database not available !", this);
                }
            }
            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                try
                {
                    //VSDocumentService.documentServiceClient objDocumentService;

                    //objDocumentService = new VSDocumentService.documentServiceClient("DocumentService" + "HttpPort", sVidalConString + "DocumentService");

                    //WebClient client = new WebClient();
                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sVidalConString + "DocumentService");
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //if (response.StatusCode != HttpStatusCode.OK)
                    //{
                    //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //    lblMessage.Text = "VIDAL web-services not running now !";

                    //    //Alert.ShowAjaxMsg(lblMessage.Text, this);
                    //}
                }
                catch
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "VIDAL web-services not running now !";

                    //Alert.ShowAjaxMsg(lblMessage.Text, this);
                }
            }

            setDiagnosis();
            setAllergiesWithInterfaceCode();

            #endregion

            bindData();
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
            {
                btnReOrder.Visible = false;
            }
        }

        setGridColor();

        if (common.myBool(Request.QueryString["EMRSingleScreen"]))
        {
            btnReOrder.Visible = false;
        }
    }

    private void bindControl()
    {
        OpPrescription objOPP = new OpPrescription(sConString);
        try
        {
            ddlStopOrder.DataSource = objOPP.GetCancelRemarks();
            ddlStopOrder.DataTextField = "Description";
            ddlStopOrder.DataValueField = "Id";
            ddlStopOrder.DataBind();

            ddlStopOrder.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlStopOrder.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objOPP = null;
        }
    }

    protected void bindData()
    {
        DataSet ds = new DataSet();
        DataView dvConsumable = new DataView();
        DataView dvConsumable1 = new DataView();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
        try
        {


            lblPendingIndent.Visible = false;
            ddlPartialPendingIndent.Visible = false;
            lblPendingIndentRemarks.Visible = false;
            txtPartialIndentRemarks.Visible = false;
            btnSavePartialPendingIndent.Visible = false;

            string fromDate = string.Empty;
            string toDate = string.Empty;

            switch (common.myStr(ddlSearchOn.SelectedValue))
            {
                case "A":
                    break;

                case "D":
                    fromDate = common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd");
                    toDate = common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd");

                    break;
            }

            if (common.myStr(Session["OPIP"]) == "I")
            {
                if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("PI"))
                {
                    ds = objPharmacy.getPreviousMedicines(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                       common.myInt(Session["EncounterId"]), 0, 0, "A", common.myStr(txtDrugName.Text).Trim(), fromDate, toDate);

                    bindPartialIndent(ds);
                }
                else if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("CL"))
                {
                    ds = objPharmacy.getPreviousMedicines(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                       common.myInt(Session["EncounterId"]), 0, 0, "A", common.myStr(txtDrugName.Text).Trim(), fromDate, toDate);

                }
                else if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("C"))
                {
                    ds = objPharmacy.getPreviousMedicinesNew(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                               common.myInt(Session["EncounterId"]), 0, 0, "P",
                               common.myStr(txtDrugName.Text).Trim(), string.Empty, string.Empty, string.Empty);

                    btnReOrder.Visible = false;
                    btnPrint.Enabled = false;//Pra
                }
                else
                {
                    ds = objPharmacy.getPreviousMedicines(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(Session["EncounterId"]), 0, 0, "A", common.myStr(txtDrugName.Text).Trim(), fromDate, toDate);
                    if (!common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                        btnReOrder.Visible = true;
                    btnPrint.Enabled = false;//Pra
                }

                if (!ds.Tables[0].Columns.Contains("IsCompoundedDrugOrder"))
                {
                    ds.Tables[0].Columns.Add("IsCompoundedDrugOrder", typeof(bool));
                }

                if (common.myBool(Request.QueryString["Consumable"]))
                {
                    dvConsumable = new DataView(ds.Tables[0]);
                    //dvConsumable.RowFilter = "ISNULL(NurseCategory,0) = 1 AND ItemCategoryShortName IN ('LAC','MCS','SUR','DEF')";
                    if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                    {
                        dvConsumable.RowFilter = "ISNULL(NurseCategory,0) = 1 AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1";// AND ItemCategoryShortName IN('LAC', 'MCS', 'SUR', 'DEF', 'MC')";
                    }
                    else
                    {
                        dvConsumable.RowFilter = "ISNULL(NurseCategory,0) = 1";// AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1";
                    }

                    dvConsumable.Sort = "IndentId DESC";
                    dvConsumable1 = new DataView(ds.Tables[1]);
                }
                else
                {
                    string flagIsShowConsumableOrderforWard = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitallocationId"]), common.myInt(Session["FacilityId"]), "IsShowConsumableOrderforWard", sConString);
                    if (flagIsShowConsumableOrderforWard.Equals("Y"))
                    {
                        if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("C"))
                        {
                            dvConsumable = new DataView(ds.Tables[0]);
                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ItemCategoryShortName IN ('MED') AND ISNULL(IsCancel,0) <> 1 AND IsCurrentMedication<>1";
                            dvConsumable.RowFilter = "ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1";
                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsCancel,0) <> 1";
                            dvConsumable.Sort = "IndentId DESC";
                            dvConsumable1 = new DataView(ds.Tables[1]);
                        }
                        else
                        {
                            dvConsumable = new DataView(ds.Tables[0]);
                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ItemCategoryShortName IN ('MED') AND ISNULL(IsCancel,0) <> 1 AND IsCurrentMedication<>1";
                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsCancel,0) <> 1 AND ISNULL(IsCurrentMedication,0) <> 1 ";

                            //dvConsumable.RowFilter = "ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1";


                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsCancel,0) <> 1";
                            dvConsumable.Sort = "IndentId DESC";
                            dvConsumable1 = new DataView(ds.Tables[1]);
                        }
                    }
                    else
                    {
                        if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("C"))
                        {
                            dvConsumable = new DataView(ds.Tables[0]);
                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ItemCategoryShortName IN ('MED') AND ISNULL(IsCancel,0) <> 1 AND IsCurrentMedication<>1";
                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1";
                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsCancel,0) <> 1";

                            if (common.myInt(Session["ModuleId"]) == 3) //EMR Mdoule
                            {
                                dvConsumable.RowFilter = "ISNULL(IsConsumable, 0) = 0";
                            }

                            dvConsumable.Sort = "IndentId DESC";
                            dvConsumable1 = new DataView(ds.Tables[1]);
                        }
                        else
                        {
                            dvConsumable = new DataView(ds.Tables[0]);
                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ItemCategoryShortName IN ('MED') AND ISNULL(IsCancel,0) <> 1 AND IsCurrentMedication<>1";
                            if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                            {
                                dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1  AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1 AND ISNULL(IsCurrentMedication,0) <> 1 "; //AND ItemCategoryShortName IN ('MED')
                                //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsCancel,0) <> 1";
                                dvConsumable.Sort = "IndentId DESC";
                            }
                            else
                            {
                                if (common.myInt(Session["ModuleId"]) == 3) //EMR Mdoule
                                {
                                    dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsCurrentMedication,0) <> 1 AND ISNULL(IsConsumable,0)=0 ";// AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1 ";
                                                                                                                                                               //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsCancel,0) <> 1";
                                    dvConsumable.Sort = "IndentId DESC";
                                }
                                else
                                {
                                    dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsCurrentMedication,0) <> 1";// AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1 ";
                                                                                                                                 //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsCancel,0) <> 1";
                                    dvConsumable.Sort = "IndentId DESC";
                                }
                            }
                            dvConsumable1 = new DataView(ds.Tables[1]);
                        }
                    }
                }
            }
            else
            {
                string sPreviousMedication = "A";

                if (!common.myBool(Request.QueryString["Consumable"]))
                {
                    if (common.myStr(ddlSearchOn.SelectedValue).Equals("D"))
                    {
                        sPreviousMedication = "";
                    }
                    else
                    {
                        sPreviousMedication = "P";
                    }
                    ds = objEMR.getPreviousMedicationOP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, 0, sPreviousMedication, common.myStr(txtDrugName.Text).Trim(), fromDate, toDate);
                }
                else
                {
                    ds = objEMR.getOPMedicinesNew(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, 0, sPreviousMedication, common.myStr(txtDrugName.Text).Trim(), fromDate, toDate);
                }

                if (!ds.Tables[0].Columns.Contains("IsCompoundedDrugOrder"))
                {
                    ds.Tables[0].Columns.Add("IsCompoundedDrugOrder", typeof(bool));
                }

                if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("C"))
                {

                    if (common.myBool(Request.QueryString["Consumable"]))
                    {
                        dvConsumable = new DataView(ds.Tables[0]);
                        //dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ItemCategoryShortName IN ('LAC','MCS','SUR','DEF')";
                        if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                        {
                            dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1  AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1 AND ItemCategoryShortName IN('LAC', 'MCS', 'SUR', 'DEF', 'MC');"; //AND ItemCategoryShortName IN('LAC', 'MCS', 'SUR', 'DEF', 'MC');
                        }
                        else
                        {
                            dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1";
                        }
                        dvConsumable.Sort = "IndentId DESC";
                        dvConsumable1 = new DataView(ds.Tables[1]);
                    }
                    else
                    {
                        dvConsumable = new DataView(ds.Tables[0]);
                        if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                        {
                            dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ItemCategoryShortName IN ('MED') AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1 ";
                        }
                        else
                        {
                            dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1 ";
                        }
                        //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ItemCategoryShortName IN ('MED') AND ISNULL(IsCancel,0) <> 1 ";

                        dvConsumable.Sort = "IndentId DESC";
                        dvConsumable1 = new DataView(ds.Tables[1]);
                    }
                }
                else
                {
                    if (common.myBool(Request.QueryString["Consumable"]))
                    {
                        dvConsumable = new DataView(ds.Tables[0]);
                        //dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ItemCategoryShortName IN ('LAC','MCS','SUR','DEF')";
                        dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1";// AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1";
                        dvConsumable.Sort = "IndentId DESC";
                        dvConsumable1 = new DataView(ds.Tables[1]);
                    }
                    else
                    {
                        dvConsumable = new DataView(ds.Tables[0]);
                        //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ItemCategoryShortName IN ('MED') AND ISNULL(IsCancel,0) <> 1 ";
                        dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1";// AND ISNULL(IsStop,0) <> 1 AND ISNULL(IsCancel,0) <> 1 ";
                        dvConsumable.Sort = "IndentId DESC";
                        dvConsumable1 = new DataView(ds.Tables[1]);
                    }
                }
            }
            //ViewState["GridData"] = dvConsumable.ToTable();
            //ViewState["GridDataDetail"] = dvConsumable1.ToTable();

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);

                gvPrevious.DataSource = ds.Tables[0];
            }
            else
            {
                DataView dvFinalConsumable = new DataView();
                dvFinalConsumable = dvConsumable;

                if (common.myStr(rdoPreviousCurrent.SelectedValue) == "S")
                {
                    dvFinalConsumable.RowFilter = " ISNULL(IsStop,0) = 1 ";
                }
                if (common.myStr(rdoPreviousCurrent.SelectedValue) == "PI")
                {
                    dvFinalConsumable.RowFilter = " ISNULL(Qty, 0) <> ISNULL(IssueQty, 0) AND ISNULL(IssueQty, 0) > 0";

                    btnReOrder.Visible = false;

                    lblPendingIndent.Visible = true;
                    ddlPartialPendingIndent.Visible = true;
                    lblPendingIndentRemarks.Visible = true;
                    txtPartialIndentRemarks.Visible = true;
                    btnSavePartialPendingIndent.Visible = true;
                }
                if (common.myStr(rdoPreviousCurrent.SelectedValue) == "CL")
                {
                    dvFinalConsumable.RowFilter = " IsCancel = 1";

                    btnReOrder.Visible = false;

                }
                gvPrevious.DataSource = dvFinalConsumable.ToTable();
                dvFinalConsumable.Dispose();
            }

            gvPrevious.DataBind();

            setVisiblilityInteraction();
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
            dvConsumable.Dispose();
            dvConsumable1.Dispose();

            objEMR = null;
        }
    }

    protected void gvPrevious_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (common.myBool(Session["IsCIMSInterfaceActive"])
                    || common.myBool(Session["IsVIDALInterfaceActive"]))
                {

                    string strInterface = "CIMS";
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    {
                        strInterface = "CIMS";
                    }
                    else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        strInterface = "VIDAL";
                    }

                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell Cell_Header = new TableCell();
                    Cell_Header.Text = string.Empty;
                    Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                    Cell_Header.ColumnSpan = 11;
                    Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
                    HeaderRow.Cells.Add(Cell_Header);

                    Cell_Header = new TableCell();
                    Cell_Header.Text = strInterface + " Information";
                    Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                    Cell_Header.ColumnSpan = 6;
                    Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
                    Cell_Header.ForeColor = System.Drawing.Color.Red;
                    Cell_Header.Font.Bold = true;
                    HeaderRow.Cells.Add(Cell_Header);

                    Cell_Header = new TableCell();
                    Cell_Header.Text = string.Empty;
                    Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                    Cell_Header.ColumnSpan = 5;
                    Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
                    HeaderRow.Cells.Add(Cell_Header);

                    gvPrevious.Controls[0].Controls.AddAt(0, HeaderRow);
                }

            }
        }
        catch
        {
        }
    }

    protected void gvPrevious_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("S"))
                {
                    e.Row.Cells[(byte)enumColumns.Select].Visible = false;
                    e.Row.Cells[(byte)enumColumns.Print].Visible = false;
                    e.Row.Cells[(byte)enumColumns.StopBy].Visible = true;
                    e.Row.Cells[(byte)enumColumns.StopDateTime].Visible = true;
                    e.Row.Cells[(byte)enumColumns.Cancel].Visible = false;
                    e.Row.Cells[(byte)enumColumns.Stop].Visible = false;
                }
                else
                {
                    e.Row.Cells[(byte)enumColumns.Print].Visible = true;
                    e.Row.Cells[(byte)enumColumns.StopBy].Visible = false;
                    e.Row.Cells[(byte)enumColumns.StopDateTime].Visible = false;
                }

                e.Row.Cells[(byte)enumColumns.IndentClose].Visible = false;
                if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("PI"))
                {
                    e.Row.Cells[(byte)enumColumns.IndentClose].Visible = true;
                    e.Row.Cells[(byte)enumColumns.Cancel].Visible = false;
                }
                if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("CL"))
                {
                    e.Row.Cells[(byte)enumColumns.Cancel].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("S"))
                {
                    e.Row.Cells[(byte)enumColumns.Select].Visible = false;
                    e.Row.Cells[(byte)enumColumns.Print].Visible = false;
                    e.Row.Cells[(byte)enumColumns.StopBy].Visible = true;
                    e.Row.Cells[(byte)enumColumns.StopDateTime].Visible = true;
                    e.Row.Cells[(byte)enumColumns.Cancel].Visible = false;
                    e.Row.Cells[(byte)enumColumns.Stop].Visible = false;
                }
                else
                {
                    e.Row.Cells[(byte)enumColumns.Print].Visible = true;
                    e.Row.Cells[(byte)enumColumns.StopBy].Visible = false;
                    e.Row.Cells[(byte)enumColumns.StopDateTime].Visible = false;
                }
                if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("C"))
                {
                    gvPrevious.Columns[(byte)enumColumns.Stop].Visible = true;
                }
                else
                {
                    gvPrevious.Columns[(byte)enumColumns.Stop].Visible = false;
                }

                e.Row.Cells[(byte)enumColumns.IndentClose].Visible = false;
                if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("PI"))
                {
                    e.Row.Cells[(byte)enumColumns.IndentClose].Visible = true;
                    e.Row.Cells[(byte)enumColumns.Cancel].Visible = false;
                }
                if (common.myStr(rdoPreviousCurrent.SelectedValue).Equals("CL"))
                {
                    e.Row.Cells[(byte)enumColumns.Cancel].Visible = false;
                }


                gvPrevious.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = false;
                gvPrevious.Columns[(byte)enumColumns.MonographCIMS].Visible = false;
                gvPrevious.Columns[(byte)enumColumns.InteractionCIMS].Visible = false;
                gvPrevious.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = false;
                gvPrevious.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = false;

                gvPrevious.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = false;
                gvPrevious.Columns[(byte)enumColumns.MonographVIDAL].Visible = false;
                gvPrevious.Columns[(byte)enumColumns.InteractionVIDAL].Visible = false;
                gvPrevious.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = false;
                gvPrevious.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = false;
                gvPrevious.Columns[(byte)enumColumns.DIInteractionCIMS].Visible = false;

                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    gvPrevious.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.MonographCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.InteractionCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.DIInteractionCIMS].Visible = true;

                    HiddenField hdnCIMSItemId = (HiddenField)e.Row.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsCIMS");
                    LinkButton lnkBtnMonographCIMS = (LinkButton)e.Row.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionCIMS");
                    LinkButton lnkBtnDAInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionCIMS");
                    LinkButton lnkBtnDIInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDIInteractionCIMS");

                    lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                    lnkBtnDIInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DuplicateIngredient"]));

                    if (common.myStr(hdnCIMSItemId.Value).Trim().Length == 0
                        || common.myStr(hdnCIMSItemId.Value).Trim() == "0")
                    {
                        lnkBtnBrandDetailsCIMS.Visible = false;
                        lnkBtnMonographCIMS.Visible = false;
                        lnkBtnInteractionCIMS.Visible = false;
                        lnkBtnDHInteractionCIMS.Visible = false;
                        lnkBtnDAInteractionCIMS.Visible = false;
                        lnkBtnDIInteractionCIMS.Visible = false;
                    }
                    else
                    {
                        lnkBtnBrandDetailsCIMS.Visible = false;

                        HiddenField hdnCIMSType = (HiddenField)e.Row.FindControl("hdnCIMSType");

                        string strXML = string.Empty;

                        if (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"))
                        {
                            strXML = getBrandDetailsXMLCIMS(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));

                            if (strXML != string.Empty)
                            {
                                string outputValues = objCIMS.getFastTrack5Output(strXML);

                                if (outputValues != null)
                                {
                                    string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" name=";
                                    if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
                                    {
                                        lnkBtnBrandDetailsCIMS.Visible = true;
                                    }
                                }
                            }
                        }

                        lnkBtnMonographCIMS.Visible = false;
                        strXML = getMonographXML(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));

                        if (strXML != string.Empty)
                        {
                            string outputValues = objCIMS.getFastTrack5Output(strXML);

                            if (outputValues != null)
                            {
                                if (outputValues.ToUpper().Contains("<MONOGRAPH>"))
                                {
                                    lnkBtnMonographCIMS.Visible = true;
                                }
                            }
                        }

                    }
                }
                else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    gvPrevious.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.MonographVIDAL].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.InteractionVIDAL].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = true;

                    HiddenField hdnVIDALItemId = (HiddenField)e.Row.FindControl("hdnVIDALItemId");
                    LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsVIDAL");
                    LinkButton lnkBtnMonographVIDAL = (LinkButton)e.Row.FindControl("lnkBtnMonographVIDAL");
                    LinkButton lnkBtnInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnInteractionVIDAL");
                    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionVIDAL");
                    LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionVIDAL");

                    lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                    if (common.myInt(hdnVIDALItemId.Value) == 0)
                    {
                        lnkBtnBrandDetailsVIDAL.Visible = false;
                        lnkBtnMonographVIDAL.Visible = false;
                        lnkBtnInteractionVIDAL.Visible = false;
                        lnkBtnDHInteractionVIDAL.Visible = false;
                        lnkBtnDAInteractionVIDAL.Visible = false;
                    }
                }

                //Label TotalQty = (Label)e.Row.FindControl("lblTotalQty");
                //TotalQty.Text = common.myDbl(TotalQty.Text).ToString("F2");
                Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");
                HiddenField hdnItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                HiddenField hdnGenericId = (HiddenField)e.Row.FindControl("hdnGenericId");
                HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");
                ViewState["IndentId"] = hdnIndentId.Value;//Pra
                HiddenField hdnCustomMedication = (HiddenField)e.Row.FindControl("hdnCustomMedication");

                HiddenField hdnIsStop = (HiddenField)e.Row.FindControl("hdnIsStop");
                HiddenField hdnIsCancel = (HiddenField)e.Row.FindControl("hdnIsCancel");

                CheckBox chkRow = (CheckBox)e.Row.FindControl("chkRow");
                ImageButton ibtnCancel = (ImageButton)e.Row.FindControl("ibtnCancel");

                ImageButton ibtnStop = (ImageButton)e.Row.FindControl("ibtnStop");
                HiddenField hdnApprovalStatusColor = (HiddenField)e.Row.FindControl("hdnApprovalStatusColor");
                HiddenField hdnIsCompoundedDrugOrder = (HiddenField)e.Row.FindControl("hdnIsCompoundedDrugOrder");
                Label lblSource = (Label)e.Row.FindControl("lblSource");

                HiddenField hdnIndentClose = (HiddenField)e.Row.FindControl("hdnIndentClose");
                Label lblIndentClose = (Label)e.Row.FindControl("lblIndentClose");

                lblIndentClose.Text = "";
                if (common.myBool(hdnIndentClose.Value))
                {
                    lblIndentClose.Text = "Yes";
                }

                #region LASA / Non-LASA Item
                HiddenField hdnItemFlagCode = (HiddenField)e.Row.FindControl("hdnItemFlagCode");
                if (common.myStr(hdnItemFlagCode.Value).Equals("LASA"))
                {
                    e.Row.Cells[(byte)enumColumns.DrugName].BackColor = System.Drawing.Color.LightGray;
                }
                else if (common.myStr(hdnItemFlagCode.Value).Equals("SC") || common.myStr(hdnItemFlagCode.Value).Equals("HighRisk") || common.myStr(hdnItemFlagCode.Value).Equals("HighValue"))
                {
                    e.Row.Cells[(byte)enumColumns.DrugName].BackColor = System.Drawing.Color.Yellow;
                }
                //Add By Himanshu On Date 17 / 02 / 2022 Given By Varsha Parshad 
                else if (common.myStr(hdnItemFlagCode.Value).Equals("NARCOTIC"))
                {
                    e.Row.Cells[(byte)enumColumns.DrugName].BackColor = System.Drawing.Color.DarkGreen;
                }
                //end
                #endregion

                if (common.myBool(hdnCustomMedication.Value))
                {
                    chkRow.Visible = false;
                    e.Row.BackColor = System.Drawing.Color.Teal;
                }
                else
                {
                    if (common.myInt(hdnGenericId.Value) > 0 && common.myInt(hdnItemId.Value) <= 0)
                    {
                        if (common.myStr(lblSource.Text).Equals("I"))
                        {
                            chkRow.Visible = false;
                        }
                        e.Row.BackColor = System.Drawing.Color.LightBlue;
                    }
                }

                //if (ViewState["GridDataDetail"] != null)
                //{
                //    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
                //    DataTable dt = (DataTable)ViewState["GridDataDetail"];
                //    DataView dv = new DataView(dt);
                //    dv.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value) + " AND ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                //    if (dv.ToTable().Rows.Count > 0)
                //    {
                //        lblPrescriptionDetail.Text = emr.GetPrescriptionDetailStringNew(dv.ToTable());
                //    }
                //    else
                //    {
                //        DataView dv1 = new DataView(dt);
                //        dv1.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value);
                //        if (dv1.ToTable().Rows.Count > 0)
                //        {
                //            lblPrescriptionDetail.Text = emr.GetPrescriptionDetailStringNew(dv1.ToTable());
                //        }
                //    }
                //    dt.Dispose();
                //    dv.Dispose();
                //}
                //HiddenField hdnIsStop = (HiddenField)e.Row.FindControl("hdnIsStop");
                //HiddenField hdnIsCancel = (HiddenField)e.Row.FindControl("hdnIsCancel");

                //CheckBox chkRow = (CheckBox)e.Row.FindControl("chkRow");
                //ImageButton ibtnCancel = (ImageButton)e.Row.FindControl("ibtnCancel");

                //ImageButton ibtnStop = (ImageButton)e.Row.FindControl("ibtnStop");

                if (common.myBool(hdnIsStop.Value) || common.myBool(hdnIsCancel.Value)
                    || common.myStr(rdoPreviousCurrent.SelectedValue).Equals("PI")
                    || common.myStr(rdoPreviousCurrent.SelectedValue).Equals("CL"))
                {
                    ibtnCancel.Visible = false;
                    ibtnStop.Visible = false;
                    chkRow.Visible = false;
                }

                if (common.myBool(hdnIndentClose.Value))
                {
                    Label lblTotalQty = (Label)e.Row.FindControl("lblTotalQty");
                    Label lblIssueQty = (Label)e.Row.FindControl("lblIssueQty");

                    if (common.myInt(lblIssueQty.Text) > 0)
                    {
                        ibtnCancel.Visible = false;
                        ibtnStop.Visible = false;
                        chkRow.Visible = false;
                    }
                }

                if (common.myBool(hdnIsStop.Value))
                {
                    e.Row.BackColor = System.Drawing.Color.Aqua;
                }
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                {
                    ibtnCancel.Enabled = false;
                }
                if (common.myStr(hdnApprovalStatusColor.Value) != "")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(common.myStr(hdnApprovalStatusColor.Value));
                }

                if (common.myBool(hdnIsCompoundedDrugOrder.Value))
                {
                    chkRow.Checked = false;
                    chkRow.Visible = false;
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

    protected void btnStopOrder_Click(object sender, EventArgs e)
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = string.Empty;

            if (common.myInt(ddlStopOrder.SelectedValue) == 0)
            {
                lblMessage.Text = "Reason for stop order not selected !";
                return;
            }

            string strIds = string.Empty;

            int rowNo = 0;
            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                if (chkRow.Visible && chkRow.Checked)
                {
                    HiddenField hdnDetailsId = (HiddenField)dataItem.FindControl("hdnDetailsId");
                    if (common.myInt(hdnDetailsId.Value) == 0)
                    {
                        continue;
                    }

                    Label lblQty = (Label)dataItem.FindControl("lblQty");
                    Label lblIssueQty = (Label)dataItem.FindControl("lblIssueQty");

                    if (common.myDbl(lblQty.Text) == common.myDbl(lblIssueQty.Text))
                    {
                        lblMessage.Text = "Fully issued quantity not allow to stop at serial no " + ++rowNo;
                        return;
                    }

                    if (strIds != string.Empty)
                    {
                        strIds += ",";
                    }
                    strIds += common.myInt(hdnDetailsId.Value);
                }
            }

            if (strIds == string.Empty)
            {
                lblMessage.Text = "Record not selected !";
                return;
            }

            string strMsg = objPharmacy.stopPrescription(strIds, common.myInt(ddlStopOrder.SelectedValue), common.myInt(Session["UserId"]));

            if (strMsg.Contains("Succeeded"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }

            bindData();

            lblMessage.Text = "Stop Order " + strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPharmacy = null;
        }
    }

    private DataTable getReorderOverrideComments()
    {
        DataTable tbl = new DataTable();

        DataColumn col = new DataColumn("ItemId");
        tbl.Columns.Add(col);

        col = new DataColumn("GenericId");
        tbl.Columns.Add(col);

        col = new DataColumn("CommentsDrugAllergy");
        tbl.Columns.Add(col);

        col = new DataColumn("CommentsDrugToDrug");
        tbl.Columns.Add(col);

        col = new DataColumn("CommentsDrugHealth");
        tbl.Columns.Add(col);
        col = new DataColumn("ItemName");
        tbl.Columns.Add(col);

        col = new DataColumn("Qty");
        tbl.Columns.Add(col);
        return tbl;
    }

    protected void btnReOrder_Onclick(object sender, EventArgs e)
    {
        DataTable tbl = new DataTable();
        DataTable previousitemtbl = new DataTable();
        string sOPIPs = string.Empty, sOPIP = string.Empty;
        string sDetailsIds = string.Empty, sDetailsId = string.Empty;
        string sIndentIds = string.Empty, sIndentId = string.Empty;
        string sItemIds = string.Empty, sItemId = string.Empty;
        string sGenericIds = string.Empty, sGenericId = string.Empty;
        int countOPIPId = 0, countDetailsId = 0, countIndentId = 0, countItemId = 0, countGenericId = 0;
        try
        {
            tbl = getReorderOverrideComments();
            previousitemtbl = getReorderOverrideComments();
            foreach (GridViewRow row in gvPrevious.Rows)
            {
                CheckBox chkRow = (CheckBox)row.FindControl("chkRow");
                if (chkRow.Visible && chkRow.Checked)
                {
                    HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
                    HiddenField hdnGenericId = (HiddenField)row.FindControl("hdnGenericId");
                    HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                    HiddenField hdnDetailsId = (HiddenField)row.FindControl("hdnDetailsId");
                    Label lblSource = (Label)row.FindControl("lblSource");
                    Label lblItemName = (Label)row.FindControl("lblItemName");
                    Label lblTotalQty = (Label)row.FindControl("lblTotalQty");
                    HiddenField hdnCommentsDrugAllergy = (HiddenField)row.FindControl("hdnCommentsDrugAllergy");
                    HiddenField hdnCommentsDrugToDrug = (HiddenField)row.FindControl("hdnCommentsDrugToDrug");
                    HiddenField hdnCommentsDrugHealth = (HiddenField)row.FindControl("hdnCommentsDrugHealth");

                    if (common.myBool(Session["IsCIMSInterfaceActive"])
                        || common.myBool(Session["IsVIDALInterfaceActive"]))
                    {

                        if (common.myInt(hdnItemId.Value) > 0
                            || common.myInt(hdnGenericId.Value) > 0)
                        {
                            DataRow DR = tbl.NewRow();
                            DR["ItemId"] = common.myStr(hdnItemId.Value);
                            DR["GenericId"] = common.myStr(hdnGenericId.Value);
                            DR["CommentsDrugAllergy"] = common.myStr(hdnCommentsDrugAllergy.Value);
                            DR["CommentsDrugToDrug"] = common.myStr(hdnCommentsDrugToDrug.Value);
                            DR["CommentsDrugHealth"] = common.myStr(hdnCommentsDrugHealth.Value);

                            DR["ItemName"] = common.myStr(lblItemName.Text);
                            DR["Qty"] = common.myStr(lblTotalQty.Text); tbl.Rows.Add(DR);
                        }
                    }

                    #region Omprakash SHARMA 05-05-2021
                    if (common.myInt(hdnItemId.Value) > 0
                            || common.myInt(hdnGenericId.Value) > 0)
                    {
                        DataRow DR = previousitemtbl.NewRow();
                        DR["ItemId"] = common.myStr(hdnItemId.Value);
                        DR["GenericId"] = common.myStr(hdnGenericId.Value);
                        DR["CommentsDrugAllergy"] = common.myStr(hdnCommentsDrugAllergy.Value);
                        DR["CommentsDrugToDrug"] = common.myStr(hdnCommentsDrugToDrug.Value);
                        DR["CommentsDrugHealth"] = common.myStr(hdnCommentsDrugHealth.Value);
                        DR["ItemName"] = common.myStr(lblItemName.Text);
                        DR["Qty"] = common.myStr(lblTotalQty.Text);
                        previousitemtbl.Rows.Add(DR);
                    }
                    #endregion                    if (common.myInt(hdnIndentId.Value) > 0 && common.myInt(hdnItemId.Value) > 0)
                    {
                        char[] delimiterChars = { ',' };

                        string[] itemList = sItemIds.Split(delimiterChars);

                        int posItem = Array.IndexOf(itemList, hdnItemId.Value);
                        if (posItem == -1)
                        {
                            string[] indentList = sIndentIds.Split(delimiterChars);

                            int posIndent = Array.IndexOf(indentList, hdnIndentId.Value);
                            if (posIndent == -1)
                            {
                                if (hdnIndentId.Value != sIndentId)
                                {
                                    sIndentId = hdnIndentId.Value;
                                    sIndentIds = countIndentId == 0 ? sIndentId : sIndentIds + "," + hdnIndentId.Value;
                                    countIndentId++;
                                }
                            }

                            if (hdnItemId.Value != sItemId)
                            {
                                sItemId = hdnItemId.Value;
                                sItemIds = countItemId == 0 ? sItemId : sItemIds + "," + hdnItemId.Value;
                                countItemId++;
                            }

                            if (hdnDetailsId.Value != sDetailsId)
                            {
                                sDetailsId = hdnDetailsId.Value;
                                sDetailsIds = countDetailsId == 0 ? sDetailsId : sDetailsIds + "," + hdnDetailsId.Value;
                                countDetailsId++;
                            }

                            if (lblSource.Text != sOPIP)
                            {
                                sOPIP = lblSource.Text;
                                sOPIPs = countOPIPId == 0 ? "'" + sOPIP + "'" : sOPIPs + ",'" + lblSource.Text + "'";
                                countOPIPId++;
                            }
                        }
                        else if (posItem >= 0)
                        {
                            Alert.ShowAjaxMsg("Duplicate items found!", this.Page);
                            return;
                        }
                    }
                    if (common.myInt(hdnIndentId.Value) > 0 && common.myInt(hdnGenericId.Value) > 0 && !(common.myInt(hdnItemId.Value) > 0))
                    {
                        char[] delimiterChars = { ',' };

                        string[] itemList = sItemIds.Split(delimiterChars);

                        int posItem = Array.IndexOf(itemList, hdnGenericId.Value);
                        if (posItem == -1)
                        {
                            string[] indentList = sIndentIds.Split(delimiterChars);

                            int posIndent = Array.IndexOf(indentList, hdnIndentId.Value);
                            if (posIndent == -1)
                            {
                                if (hdnIndentId.Value != sIndentId)
                                {
                                    sIndentId = hdnIndentId.Value;
                                    sIndentIds = countIndentId == 0 ? sIndentId : sIndentIds + "," + hdnIndentId.Value;
                                    countIndentId++;
                                }
                            }

                            if (hdnGenericId.Value != sGenericId)
                            {
                                sGenericId = hdnGenericId.Value;
                                sGenericIds = countGenericId == 0 ? sGenericId : sGenericIds + "," + hdnGenericId.Value;
                                countGenericId++;
                            }

                            if (hdnDetailsId.Value != sDetailsId)
                            {
                                sDetailsId = hdnDetailsId.Value;
                                sDetailsIds = countDetailsId == 0 ? sDetailsId : sDetailsIds + "," + hdnDetailsId.Value;
                                countDetailsId++;
                            }

                            if (lblSource.Text != sOPIP)
                            {
                                sOPIP = lblSource.Text;
                                sOPIPs = countOPIPId == 0 ? "'" + sOPIP + "'" : sOPIPs + ",'" + lblSource.Text + "'";
                                countOPIPId++;
                            }
                        }
                        else if (posItem == 0)
                        {
                            Alert.ShowAjaxMsg("Duplicate items found!", this.Page);
                            return;
                        }
                    }
                }
            }
            if (sIndentId == string.Empty && (sItemIds == string.Empty || sGenericIds == string.Empty))
            {
                Alert.ShowAjaxMsg("Please select Item to Reorder", Page);
                return;
            }

            hndPageOPIPSource.Value = sOPIPs;
            hdnPageDetailsIds.Value = sDetailsIds;
            hdnPageIndentIds.Value = sIndentIds;
            hdnPageItemIds.Value = sItemIds;
            hdnPageGenericIds.Value = sGenericIds;
            Session["TblReorderOverrideComments"] = tbl;
            Session["previousitemtbl"] = previousitemtbl;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            tbl.Dispose();
        }
    }

    protected void ddlSearchOn_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            switch (common.myStr(ddlSearchOn.SelectedValue))
            {
                case "A":
                    tblDateRange.Visible = false;

                    break;

                case "D":
                    tblDateRange.Visible = true;
                    txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-30);

                    txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                    txtToDate.SelectedDate = DateTime.Now;

                    break;
            }

            bindData();
        }
        catch
        {
        }
    }

    protected void btnFilter_Onclick(object sender, EventArgs e)
    {
        bindData();
    }

    protected void chkRow_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"])
                    || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                GridViewRow row = (GridViewRow)((DataControlFieldCell)((CheckBox)sender).Parent).Parent;
                CheckBox chkRow = (CheckBox)row.FindControl("chkRow");

                if (chkRow.Checked)
                {
                    if (dvInteraction.Visible)
                    {
                        chkRow.Checked = false;
                        Alert.ShowAjaxMsg("Please fill reason to continue or cancel message window !", this.Page);
                        return;
                    }

                    HiddenField ItemId = (HiddenField)row.FindControl("hdnItemId");
                    HiddenField GenericIdId = (HiddenField)row.FindControl("hdnGenericId");

                    HiddenField CIMSType = (HiddenField)row.FindControl("hdnCIMSType");
                    HiddenField CIMSItemId = (HiddenField)row.FindControl("hdnCIMSItemId");

                    hdnItemId.Value = ItemId.Value;
                    hdnGenericId.Value = GenericIdId.Value;

                    hdnCIMSType.Value = CIMSType.Value;
                    hdnCIMSItemId.Value = CIMSItemId.Value;

                    HiddenField hdnDetailsId = (HiddenField)row.FindControl("hdnDetailsId");
                    ViewState["DetailsId"] = hdnDetailsId.Value;

                    chkIsInteraction(0);
                }
            }
        }
        catch
        {
        }
    }
    protected void chkRow_OnCheckedChanged2(object sender, EventArgs e)
    {
        //Pra
        btnPrint.Enabled = false;
        foreach (GridViewRow row in gvPrevious.Rows)
        {
            CheckBox chkRow = (CheckBox)row.FindControl("chkRow");
            if (chkRow.Checked)
            {
                HiddenField hdnItemFlagCode = (HiddenField)row.FindControl("hdnItemFlagCode");
                if (hdnItemFlagCode.Value.Trim().ToUpper() == "NARCOTIC")
                {
                    btnPrint.Enabled = true;
                    break;
                }
            }
        }
    }
    protected void gvPrevious_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        try
        {
            if (e.CommandName.Equals("BindDetail"))
            {
                dvAntibiotics.Visible = true;
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
                Int64 IndentId = Convert.ToInt64(hdnIndentId.Value);
                Int64 ItemId = Convert.ToInt64(hdnItemId.Value);
                Hashtable HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("@IndentId", IndentId);
                HshIn.Add("@itemid", ItemId);
                DataSet dsAcnkw = new DataSet();
                dsAcnkw = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEMRAcknowDetails", HshIn);
                Gridantibiotics.DataSource = null;
                Gridantibiotics.DataBind();
                if (dsAcnkw.Tables[0].Rows.Count > 0)
                {
                    Gridantibiotics.DataSource = dsAcnkw;
                    Gridantibiotics.DataBind();
                }

            }


            if (e.CommandName.ToUpper().Equals("ITEMCANCEL"))
            {
                int ItemId = common.myInt(e.CommandArgument);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnDetailsId = (HiddenField)row.FindControl("hdnDetailsId");
                //int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                Label lblTotalQty = (Label)row.FindControl("lblTotalQty");
                Label lblIssueQty = (Label)row.FindControl("lblIssueQty");


                if (common.myInt(hdnDetailsId.Value).Equals(0))
                {
                    return;
                }

                if (!common.myInt(hdnEncounterId.Value).Equals(common.myInt(Session["EncounterId"])))
                {
                    Alert.ShowAjaxMsg("Cancellation allowed only on current selected encounter !", this.Page);
                    return;
                }
                if (common.myInt(lblTotalQty.Text) == common.myInt(lblIssueQty.Text) && common.myInt(lblIssueQty.Text) > 0)
                {
                    Alert.ShowAjaxMsg("Full issue qty not allow to cancel !", this.Page);
                    return;
                }
                ViewState["IndentDetailsId"] = common.myInt(hdnDetailsId.Value).ToString();
                ViewState["StopItemId"] = common.myInt(ItemId).ToString();
                ViewState["StopIndentId"] = common.myStr(hdnIndentId.Value);

                ViewState["CancelBrandList"] = string.Empty;
                if (common.myStr(ViewState["IsCancelWithOutRemarksInEmrPreviousMedication"]).Equals("Y"))
                {
                    txtStopRemarks.Text = string.Empty;
                    dvConfirmStop.Visible = false;
                    btnStopMedication_OnClick(null, null);
                }
                else
                {
                    txtStopRemarks.Text = string.Empty;
                    dvConfirmStop.Visible = true;
                    lblCancelStopMedicationRemarks.Text = "Cancel Medication Remarks";
                    btnStopMedication.Text = "Cancel";
                }

                //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");

                //Hashtable hshOutput = new Hashtable();

                //hshOutput = emr.StopCancelPreviousMedication(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]),
                //              Convert.ToInt32(hdnIndentId.Value), ItemId, Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["RegistrationId"]),
                //              Convert.ToInt32(Session["EncounterId"]), 1, txtRemarks.Text, Session["OPIP"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("ITEMSTOP"))
            {
                int ItemId = common.myInt(e.CommandArgument);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnDetailsId = (HiddenField)row.FindControl("hdnDetailsId");
                //int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                if (common.myInt(hdnDetailsId.Value).Equals(0))
                {
                    return;
                }

                if (!common.myInt(hdnEncounterId.Value).Equals(common.myInt(Session["EncounterId"])))
                {
                    Alert.ShowAjaxMsg("Cancellation allow only current selected encounter !", this.Page);
                    return;
                }

                ViewState["IndentDetailsId"] = common.myInt(hdnDetailsId.Value).ToString();
                ViewState["StopItemId"] = common.myInt(ItemId).ToString();
                ViewState["StopIndentId"] = common.myStr(hdnIndentId.Value);

                txtStopRemarks.Text = string.Empty;
                dvConfirmStop.Visible = true;

                lblCancelStopMedicationRemarks.Text = "Stop Medication Remarks";
                btnStopMedication.Text = "Stop";

                //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");

                //Hashtable hshOutput = new Hashtable();

                //hshOutput = emr.StopCancelPreviousMedication(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]),
                //              Convert.ToInt32(hdnIndentId.Value), ItemId, Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["RegistrationId"]),
                //              Convert.ToInt32(Session["EncounterId"]), 1, txtRemarks.Text, Session["OPIP"].ToString());
            }
            else if (e.CommandName == "BrandDetailsCIMS")
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");

                showBrandDetails(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "MonographCIMS")
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");

                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "InteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showIntreraction();
            }
            else if (e.CommandName == "DIInteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showDIIntreraction();
            }
            else if (e.CommandName == "DHInteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName == "DAInteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction("A");
            }
            else if (e.CommandName == "MonographVIDAL")
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    getMonographVidal((int?)common.myInt(e.CommandArgument));
                }
            }
            else if (e.CommandName == "InteractionVIDAL")
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();

                if (commonNameGroupIds.Length > 0)
                {
                    getDrugToDrugInteractionVidal(commonNameGroupIds);
                }
            }
            else if (e.CommandName == "DHInteractionVIDAL")
            {
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName == "DAInteractionVIDAL")
            {
                showHealthOrAllergiesIntreraction("A");
            }
            else if (e.CommandName.Equals("PRINT"))
            {
                int intPrescriptionId = common.myInt(e.CommandArgument);

                if (intPrescriptionId.Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Prescription not Selected !";
                    return;
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" + common.myInt(hdnEncounterId.Value) + "&PId=" + intPrescriptionId;

                RadWindow1.Height = 480;// Akshay
                RadWindow1.Width = 900;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.Modal = true;
                RadWindow1.VisibleOnPageLoad = true;
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

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
            emr = null;
        }
    }

    protected void btnStopMedication_OnClick(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        try
        {
            if (common.myLen(txtStopRemarks.Text) == 0
                && !common.myStr(ViewState["IsCancelWithOutRemarksInEmrPreviousMedication"]).Equals("Y"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Remarks can't be blank !";
                Alert.ShowAjaxMsg("Remarks can't be blank !", this);
                return;
            }

            if (common.myInt(ViewState["StopItemId"]) > 0 || common.myInt(ViewState["IndentDetailsId"]) > 0
                || !common.myStr(ViewState["CancelBrandList"]).Equals(string.Empty))
            {
                Hashtable hshOutput = new Hashtable();

                if (!common.myStr(ViewState["CancelBrandList"]).Equals(string.Empty))
                {
                    hshOutput = objEMR.CancelMultiplePrescriptions(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                    common.myInt(Session["RegistrationId"]), common.myStr(txtStopRemarks.Text).Trim(),
                                                    common.myStr(Session["OPIP"]), common.myStr(ViewState["CancelBrandList"]), common.myInt(Session["UserId"]));
                }
                else
                {
                    hshOutput = objEMR.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                    common.myInt(ViewState["StopIndentId"]), common.myInt(ViewState["StopItemId"]), common.myInt(Session["UserId"]),
                                                    common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                                                    common.myStr(btnStopMedication.Text).ToUpper().Equals("STOP") ? 0 : 1, common.myStr(txtStopRemarks.Text).Trim(),
                                                    common.myStr(Session["OPIP"]), common.myInt(ViewState["IndentDetailsId"]));
                }
                bindData();

                ViewState["IndentDetailsId"] = "0";
                if (common.myStr(hshOutput["@chvOutPut"]).ToUpper().Equals("SUCCESSFULL"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }

                lblMessage.Text = common.myStr(hshOutput["@chvOutPut"]);
            }
            else
            {
                Alert.ShowAjaxMsg("Drug not selected !", this);

                return;
            }

            ViewState["StopItemId"] = string.Empty;
            ViewState["StopIndentId"] = string.Empty;

            dvConfirmStop.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
        }
    }

    protected void btnStopClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            dvConfirmStop.Visible = false;
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

    ///////////////CIMS/////////////////////////////
    private void setPatientInfo()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            int? weight = null;

            ds = objEMR.getScreeningParameters(common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                //lbl_Weight.Text = string.Empty;
                //txtHeight.Text = string.Empty;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("Gender"))
                    {
                        ViewState["PatientGender"] = common.myStr(ds.Tables[0].Rows[i][1]);
                    }
                    else if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("Age"))
                    {
                        ViewState["PatientDOB"] = DateTime.Now.AddDays(-common.myInt(ds.Tables[0].Rows[i][1])).ToString("yyyy-MM-dd");
                    }
                    if (i > 1)
                    {
                        if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("WT"))// Weight
                        {
                            weight = common.myInt(ds.Tables[0].Rows[i][1]);
                            //lbl_Weight.Text = common.myStr(ds.Tables[0].Rows[i][1]);
                        }
                        else if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("HT"))// Height
                        {
                            //txtHeight.Text = common.myStr(ds.Tables[0].Rows[i][1]);
                        }
                    }
                }
            }

            ViewState["PatientWeight"] = weight;
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
            objEMR = null;
        }
    }

    private void getLegnedColor()
    {
        BaseC.clsBb objBb = new BaseC.clsBb(sConString);
        DataSet ds = new DataSet();
        try
        {
            ViewState["BrandDetailsColor"] = "#FFC48A";
            ViewState["DrugMonographColor"] = "#98AFC7";
            ViewState["DrugtoDrugInteractionColor"] = "#ECBBBB";
            ViewState["DrugHealthInteractionColor"] = "#82AB76";
            ViewState["DrugAllergyColor"] = "#82CAFA";
            ViewState["DuplicateIngredient"] = "#F36F6FD9";



            if (common.myBool(Session["IsCIMSInterfaceActive"])
                || common.myBool(Session["IsVIDALInterfaceActive"]))
            {

                ds = objBb.GetStatusMaster("CIMSInterface");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].DefaultView.RowFilter = "Code='BD'";

                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["BrandDetailsColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "Code='MO'";

                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["DrugMonographColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "Code='IN'";

                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["DrugtoDrugInteractionColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "Code='HI'";

                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["DrugHealthInteractionColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "Code='DA'";

                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["DrugAllergyColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;
                    ds.Tables[0].DefaultView.RowFilter = "Code='DI'";
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["DuplicateIngredient"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;
                }
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
            objBb = null;
            ds.Dispose();
        }
    }

    private void setGridColor()
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"])
            || common.myBool(Session["IsVIDALInterfaceActive"]))
        {
            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsCIMS");
                    LinkButton lnkBtnMonographCIMS = (LinkButton)dataItem.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                    LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");

                    LinkButton lnkBtnDIInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDIInteractionCIMS");

                    lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                    lnkBtnDIInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DuplicateIngredient"]));
                }
                else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsVIDAL");
                    LinkButton lnkBtnMonographVIDAL = (LinkButton)dataItem.FindControl("lnkBtnMonographVIDAL");
                    LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");
                    LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionVIDAL");

                    lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                }
            }

            //lnkDrugAllergy.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
        }
        LegendAR.loadLegend("Approval", "'A','R'");
        LegendAR.loadLegend("DrugType", string.Empty);
    }

    private void setDiagnosis()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            ViewState["PatientDiagnosisXML"] = string.Empty;

            //<HealthIssueCodes>
            //    <HealthIssueCode code="K22" codeType="ICD10" />
            //    <HealthIssueCode code="K22.0" codeType="ICD10" />
            //</HealthIssueCodes>

            //ViewState["PatientDiagnosisXML"] = "<HealthIssueCodes><HealthIssueCode code=\"J45\" codeType=\"ICD10\" /><HealthIssueCode code=\"N17\" codeType=\"ICD10\" /><HealthIssueCode code=\"I11\" codeType=\"ICD10\" /><HealthIssueCode code=\"F32\" codeType=\"ICD10\" /></HealthIssueCodes>";

            if (common.myBool(Session["IsCIMSInterfaceActive"])
                || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                ds = objEMR.getDiagnosis(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    {
                        StringBuilder HealthIssueCodes = new StringBuilder();
                        StringBuilder HealthCode = new StringBuilder();

                        foreach (DataRow DR in ds.Tables[0].Rows)
                        {
                            if (common.myStr(DR["ICDCode"]).Trim().Length > 0)
                            {
                                HealthCode.Append("<HealthIssueCode code=\"" + common.myStr(DR["ICDCode"]).Trim() + "\" codeType=\"ICD10\" />");
                            }
                        }

                        if (common.myLen(HealthCode) > 0)
                        {
                            HealthIssueCodes.Append("<HealthIssueCodes>" + HealthCode.ToString() + "</HealthIssueCodes>");
                        }
                        else
                        {
                            HealthIssueCodes.Append("<HealthIssueCodes />");
                        }

                        ViewState["PatientDiagnosisXML"] = HealthIssueCodes.ToString();
                    }
                    else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        List<string> list = new List<string>();

                        foreach (DataRow DR in ds.Tables[0].Rows)
                        {
                            if (common.myStr(DR["ICDCode"]).Trim().Length > 0)
                            {
                                list.Add(common.myStr(DR["ICDCode"]).Trim().Replace(".", string.Empty));
                            }
                        }
                        ViewState["PatientDiagnosisXML"] = list;
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
        finally
        {
            objEMR = null;
            ds.Dispose();
        }
    }

    private void setAllergiesWithInterfaceCode()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        DataView DV = new DataView();
        try
        {
            ViewState["PatientAllergyXML"] = string.Empty;

            if (common.myBool(Session["IsCIMSInterfaceActive"])
                || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                ds = objEMR.getDrugAllergiesInterfaceCode(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]));
                DV = ds.Tables[0].DefaultView;
                tbl = new DataTable();

                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    DV.RowFilter = "AllergyType='CIMS'";
                    tbl = DV.ToTable();

                    if (tbl.Rows.Count > 0)
                    {
                        StringBuilder Allergies = new StringBuilder();
                        StringBuilder itemsDetails = new StringBuilder();

                        foreach (DataRow DR in tbl.Rows)
                        {
                            if (common.myStr(DR["InterfaceCode"]).Trim().Length > 0)
                            {
                                itemsDetails.Append("<" + common.myStr(DR["CIMSTYPE"]).Trim() + " reference=\"" + common.myStr(DR["InterfaceCode"]).Trim() + "\" />");
                            }
                        }

                        if (common.myLen(itemsDetails) > 0)
                        {
                            Allergies.Append("<Allergies>" + itemsDetails.ToString() + "</Allergies>");
                        }
                        else
                        {
                            Allergies.Append("<Allergies />");
                        }

                        ViewState["PatientAllergyXML"] = Allergies.ToString();
                    }
                }
                else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    DV.RowFilter = "AllergyType='VIDAL'";
                    tbl = DV.ToTable();

                    if (tbl.Rows.Count > 0)
                    {
                        List<int?> list = new List<int?>();

                        foreach (DataRow DR in tbl.Rows)
                        {
                            if (common.myStr(DR["InterfaceCode"]).Trim().Length > 0)
                            {
                                list.Add(common.myInt(DR["InterfaceCode"]));
                            }
                        }

                        int?[] allergyIds = list.ToArray();

                        ViewState["PatientAllergyXML"] = allergyIds;
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
        finally
        {
            objEMR = null;
            ds.Dispose();
            tbl.Dispose();
            DV.Dispose();
        }
    }

    protected void btnBrandDetailsView_OnClick(object sender, EventArgs e)
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
        {
            if (common.myLen(hdnCIMSItemId.Value) < 2)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Generic of Brnad not selected ";
                return;
            }

            showBrandDetails(common.myStr(hdnCIMSItemId.Value).Trim(), common.myStr(hdnCIMSType.Value).Trim());
        }
    }

    protected void btnMonographView_OnClick(object sender, EventArgs e)
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
        {
            if (common.myLen(hdnCIMSItemId.Value) < 2)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Generic of Brnad not selected ";
                return;
            }

            showMonograph(common.myStr(hdnCIMSItemId.Value).Trim(), common.myStr(hdnCIMSType.Value).Trim());
        }
    }

    private void showMonograph(string CIMSItemId, string CIMSType)
    {
        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
        {
            Session["CIMSXMLInputData"] = string.Empty;
        }

        string strXML = getMonographXML(CIMSType, common.myStr(CIMSItemId));

        if (strXML != string.Empty)
        {
            Session["CIMSXMLInputData"] = strXML;
            openWindowsCIMS(false);
        }
    }

    private void showBrandDetails(string CIMSItemId, string CIMSType)
    {
        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
        {
            Session["CIMSXMLInputData"] = string.Empty;
        }

        string strXML = getBrandDetailsXMLCIMS(CIMSType, common.myStr(CIMSItemId));

        if (strXML != string.Empty)
        {
            Session["CIMSXMLInputData"] = strXML;
            openWindowsCIMS(true);
        }
    }

    protected void btnInteractionView_OnClick(object sender, EventArgs e)
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"]))
        {
            string strPrescribing = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" />";

            string strXML = getHealthOrAllergiesInterationXML("B", strPrescribing);

            if (strXML != string.Empty)
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
    }

    private void showIntreraction()
    {
        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
        {
            Session["CIMSXMLInputData"] = string.Empty;
        }

        string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getInterationXML(string.Empty);

        if (strXML != string.Empty)
        {
            Session["CIMSXMLInputData"] = strXML;
            openWindowsCIMS(false);
        }
    }

    private void showDIIntreraction()
    {
        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
        {
            Session["CIMSXMLInputData"] = string.Empty;
        }

        string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getDIInterationXML(string.Empty);

        if (strXML != string.Empty)
        {
            Session["CIMSXMLInputData"] = strXML;
            openWindowsCIMS(false);
        }
    }

    private void showHealthOrAllergiesIntreraction(string HealthOrAllergies)
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"]))
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }

            string strXML = getHealthOrAllergiesInterationXML("B", string.Empty);

            if (strXML != string.Empty)
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
        else if (common.myBool(Session["IsVIDALInterfaceActive"]))
        {
            if (common.myStr(HealthOrAllergies).Equals("H"))//Health
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();

                if (commonNameGroupIds.Length > 0)
                {
                    getDrugHealthInteractionVidal(commonNameGroupIds);
                }
            }
            else if (common.myStr(HealthOrAllergies).Equals("A"))//Allergies
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();

                if (commonNameGroupIds.Length > 0)
                {
                    getDrugAllergyVidal(commonNameGroupIds);
                }
            }
        }
    }

    private void chkIsInteraction(int ItemId)
    {
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                //DataTable tblItem = new DataTable();
                //tblItem = (DataTable)ViewState["GridDataItem"];

                //DataView DVItem = tblItem.Copy().DefaultView;

                //if (common.myInt(ItemId) > 0)
                //{
                //    DVItem.RowFilter = "ItemId = " + common.myInt(ItemId);
                //}
                ////else
                ////{
                ////    DVItem.RowFilter = "GenericId = " + common.myInt(hdnGenericId.Value);
                ////}

                spnCommentsDrugAllergy.Visible = false;
                spnCommentsDrugToDrug.Visible = false;
                spnCommentsDrugHealth.Visible = false;

                txtCommentsDrugAllergy.Enabled = false;
                txtCommentsDrugToDrug.Enabled = false;
                txtCommentsDrugHealth.Enabled = false;

                txtCommentsDrugAllergy.Text = string.Empty;
                txtCommentsDrugToDrug.Text = string.Empty;
                txtCommentsDrugHealth.Text = string.Empty;

                lblIntreactionMessage.Text = string.Empty;
                txtInteractionBetweenMessage.Text = string.Empty;

                //string strPrescribing = "<Prescribing><" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" /></Prescribing>";
                string strPrescribing = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" />";

                string strXML = getHealthOrAllergiesInterationXML("B", strPrescribing);

                if (strXML != string.Empty)
                {
                    string outputValues = objCIMS.getFastTrack5Output(strXML);

                    if (outputValues != null)
                    {
                        //Drug to Drug Interation
                        if (objCIMS.IsDrugToDrugInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValues, true) > 0)
                        {
                            string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                            if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
                            {
                                ViewState["NewPrescribing"] = strXML;

                                dvInteraction.Visible = true;

                                btnBrandDetailsView.Visible = (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"));

                                spnCommentsDrugToDrug.Visible = true;
                                txtCommentsDrugToDrug.Enabled = true;
                            }
                        }

                        //Drug Health Interation
                        /*
                        if (objCIMS.IsDrugToHealthInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValues) > 0)
                        {
                            string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                            ViewState["NewPrescribing"] = strXML;

                            dvInteraction.Visible = true;

                            btnBrandDetailsView.Visible = (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"));

                            spnCommentsDrugHealth.Visible = true;
                            txtCommentsDrugHealth.Enabled = true;
                        }
                        */

                        //Drug Allergy Interation
                        if (objCIMS.IsDrugToAllergyInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValues) > 0)
                        {
                            string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                            ViewState["NewPrescribing"] = strXML;

                            dvInteraction.Visible = true;

                            btnBrandDetailsView.Visible = (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"));

                            spnCommentsDrugAllergy.Visible = true;
                            txtCommentsDrugAllergy.Enabled = true;
                        }

                        if (dvInteraction.Visible)
                        {
                            txtInteractionBetweenMessage.Text = getInterfaceBetweenDrug(strXML);
                            txtInteractionBetweenMessage.ToolTip = txtInteractionBetweenMessage.Text;
                        }

                    }
                }

            }
            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                //return;
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private string getInterfaceBetweenDrug(string strXML)
    {
        StringBuilder sbOutput = new StringBuilder();

        Session["CIMSXMLInputData"] = strXML;
        objCIMS = new clsCIMS();
        try
        {
            string strFinalOutput = objCIMS.getCIMSFinalOutupt(false);

            using (StringReader reader = new StringReader(strFinalOutput))
            {
                string linePrevious = string.Empty;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(" vs "))
                    {
                        if (!sbOutput.ToString().Equals(string.Empty))
                        {
                            sbOutput.Append(Environment.NewLine);
                        }

                        if (line.Trim().StartsWith("vs "))
                        {
                            //D2H
                            sbOutput.Append((linePrevious.Trim() + " " + line.Trim()).Trim());
                        }
                        else
                        {
                            //D2D
                            if (line.Trim().Length < 400)
                            {
                                sbOutput.Append(line.Trim());
                            }
                        }
                    }
                    else if (line.ToUpper().Contains("PATIENT MAY BE ALLERGIC TO THE PRESCRIBING ITEM"))
                    {
                        //D2A
                        try
                        {
                            int startIdx = line.IndexOf("<a href=\"#\">") + 12;

                            string strTemp = line.Substring(startIdx, line.Length - startIdx);
                            strTemp = strTemp.Substring(0, strTemp.IndexOf("</a>"));

                            if (!sbOutput.ToString().Equals(string.Empty))
                            {
                                sbOutput.Append(Environment.NewLine);
                            }
                            sbOutput.Append(strTemp.Trim());
                        }
                        catch
                        {
                        }
                    }

                    linePrevious = line;
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            //objException.HandleException(Ex);
        }
        return sbOutput.ToString();
    }

    protected void btnInteractionContinue_OnClick(object sender, EventArgs e)
    {
        lblIntreactionMessage.Text = string.Empty;

        if (spnCommentsDrugAllergy.Visible && common.myLen(txtCommentsDrugAllergy.Text).Equals(0))
        {
            lblIntreactionMessage.Text += "Drug Allergy Comments can't be blank ! ";
        }
        if (spnCommentsDrugToDrug.Visible && common.myLen(txtCommentsDrugToDrug.Text).Equals(0))
        {
            lblIntreactionMessage.Text += "Drug To Drug Interaction Comments can't be blank ! ";
        }
        if (spnCommentsDrugHealth.Visible && common.myLen(txtCommentsDrugHealth.Text).Equals(0))
        {
            lblIntreactionMessage.Text += "Drug Health Interaction Comments can't be blank ! ";
        }

        if (common.myLen(lblIntreactionMessage.Text).Equals(0))
        {
            dvInteraction.Visible = false;

            if (common.myInt(ViewState["DetailsId"]) > 0)
            {
                foreach (GridViewRow dataItem in gvPrevious.Rows)
                {
                    HiddenField hdnDetailsId = (HiddenField)dataItem.FindControl("hdnDetailsId");
                    CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                    if (chkRow.Checked && common.myInt(hdnDetailsId.Value) > 0)
                    {
                        if (common.myInt(hdnDetailsId.Value).Equals(common.myInt(ViewState["DetailsId"])))
                        {
                            HiddenField hdnCommentsDrugAllergy = (HiddenField)dataItem.FindControl("hdnCommentsDrugAllergy");
                            HiddenField hdnCommentsDrugToDrug = (HiddenField)dataItem.FindControl("hdnCommentsDrugToDrug");
                            HiddenField hdnCommentsDrugHealth = (HiddenField)dataItem.FindControl("hdnCommentsDrugHealth");

                            hdnCommentsDrugAllergy.Value = common.myStr(txtCommentsDrugAllergy.Text).Trim();
                            hdnCommentsDrugToDrug.Value = common.myStr(txtCommentsDrugToDrug.Text).Trim();
                            hdnCommentsDrugHealth.Value = common.myStr(txtCommentsDrugHealth.Text).Trim();

                            break;
                        }
                    }
                }

                ViewState["DetailsId"] = "0";
            }

        }
    }

    protected void btnInteractionCancel_OnClick(object sender, EventArgs e)
    {
        hdnGenericId.Value = "0";
        hdnGenericName.Value = string.Empty;
        hdnItemId.Value = "0";
        hdnItemName.Value = string.Empty;

        hdnCIMSItemId.Value = string.Empty;
        hdnCIMSType.Value = string.Empty;
        hdnVIDALItemId.Value = "0";

        ViewState["DetailsId"] = "0";
        dvInteraction.Visible = false;
    }

    private void setVisiblilityInteraction()
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                //lnkDrugAllergy.Visible = false;

                string strXMLDD = getInterationXML(string.Empty);//DrugToDrug
                string strXMLDH = getHealthOrAllergiesInterationXML("H", string.Empty);//Helth
                string strXMLDA = getHealthOrAllergiesInterationXML("A", string.Empty);//Allergies
                string strXMLDI = getDIInterationXML(string.Empty);//Drug to Ingredient

                string outputValuesDD = string.Empty;
                string outputValuesDH = string.Empty;
                string outputValuesDA = string.Empty;
                string outputValuesDI = string.Empty;

                if (common.myLen(strXMLDD) > 0 || common.myLen(strXMLDH) > 0 || common.myLen(strXMLDA) > 0)
                {
                    outputValuesDD = objCIMS.getFastTrack5Output(strXMLDD);
                    outputValuesDH = objCIMS.getFastTrack5Output(strXMLDH);
                    outputValuesDA = objCIMS.getFastTrack5Output(strXMLDA);
                    outputValuesDI = objCIMS.getFastTrack5Output(strXMLDI);

                    foreach (GridViewRow dataItem in gvPrevious.Rows)
                    {
                        HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");

                        if (common.myLen(hdnCIMSItemId.Value) > 2)
                        {
                            string strCIMSItemPatternMatch = "<" + ((common.myLen(hdnCIMSType.Value) > 0) ? common.myStr(hdnCIMSType.Value) : "Product") + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                            LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                            lnkBtnInteractionCIMS.Visible = false;
                            lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

                            LinkButton lnkBtnDIInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDIInteractionCIMS");
                            lnkBtnDIInteractionCIMS.Visible = false;
                            lnkBtnDIInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DuplicateIngredient"]));


                            LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                            lnkBtnDHInteractionCIMS.Visible = false;
                            lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));

                            LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");
                            lnkBtnDAInteractionCIMS.Visible = false;
                            lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                            if (outputValuesDD != null)
                            {
                                if (objCIMS.IsDrugToDrugInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDD, false) > 0)
                                {
                                    if (outputValuesDD.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            if (outputValuesDI != null)
                            {
                                if (objCIMS.IsDuplicateInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDI, false) > 0)
                                {
                                    if (outputValuesDI.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDIInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            if (outputValuesDH != null)
                            {
                                if (objCIMS.IsDrugToHealthInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDH) > 0)
                                {
                                    if (outputValuesDH.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDHInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            if (outputValuesDA != null)
                            {
                                if (objCIMS.IsDrugToAllergyInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDA) > 0)
                                {
                                    if (outputValuesDA.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDAInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                        }
                    }

                }


                //int count = 0;
                //int rIdx = 0;
                //int rIdxDataFound = 0;

                //foreach (GridViewRow dataItem in gvStore.Rows)
                //{
                //    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");

                //    if (common.myStr(CIMSItemId.Value).Trim().Length > 0)
                //    {
                //        if (rIdxDataFound == 0)
                //        {
                //            rIdxDataFound = count;
                //        }
                //        rIdx++;
                //    }
                //    count++;
                //}

                //if (rIdx == 1)
                //{
                //    LinkButton lnkBtnInteractionCIMS = (LinkButton)gvStore.Rows[rIdxDataFound].FindControl("lnkBtnInteractionCIMS");
                //    if (lnkBtnInteractionCIMS.Visible)
                //    {
                //        lnkBtnInteractionCIMS.Visible = false;
                //    }
                //}
            }
            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();
                //lnkDrugAllergy.Visible = false;

                if (commonNameGroupIds.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    Hashtable collVitalItemIdFound = new Hashtable();

                    sb = objVIDAL.getVIDALDrugToDrugInteraction(true, commonNameGroupIds, out collVitalItemIdFound);

                    DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);  //Convert.ToDateTime("1980-01-01 00:00:00"); //yyyy-mm-ddThh:mm:ss 
                    //int? weight = common.myInt(lbl_Weight.Text);//In kilograms
                    //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
                    int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
                    int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

                    Hashtable collVitalItemIdFoundDH = new Hashtable();

                    StringBuilder sbDHI = objVIDAL.getVIDALDrugHealthInteraction(commonNameGroupIds, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
                            0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
                            (ViewState["PatientDiagnosisXML"] != string.Empty) ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
                            out collVitalItemIdFoundDH);

                    foreach (GridViewRow dataItem in gvPrevious.Rows)
                    {
                        HiddenField VIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");

                        if (common.myInt(VIDALItemId.Value) > 0)
                        {
                            LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                            LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");

                            if (collVitalItemIdFound.ContainsValue(common.myInt(VIDALItemId.Value)))
                            {
                                lnkBtnInteractionVIDAL.Visible = true;
                            }
                            else
                            {
                                lnkBtnInteractionVIDAL.Visible = false;
                            }

                            if (sbDHI.ToString().Length > 0 || collVitalItemIdFoundDH.ContainsValue(common.myInt(VIDALItemId.Value)))
                            {
                                lnkBtnDHInteractionVIDAL.Visible = true;
                            }
                            else
                            {
                                lnkBtnDHInteractionVIDAL.Visible = false;
                            }
                        }
                    }

                    int?[] allergyIds = null; //new int?[] { 114 };
                    int?[] moleculeIds = null;  //new int?[] { 202, 96, 36 };

                    if (ViewState["PatientAllergyXML"] != string.Empty)
                    {
                        allergyIds = (int?[])ViewState["PatientAllergyXML"];
                    }

                    sb = objVIDAL.getVIDALDrugAllergyInteraction(commonNameGroupIds, allergyIds, moleculeIds);

                    if (sb.ToString().Length > 0)
                    {
                        //lnkDrugAllergy.Visible = true;
                    }
                    else
                    {
                        //lnkDrugAllergy.Visible = false;
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
        finally
        {
            objVIDAL = null;
        }
    }

    private string getBrandDetailsXMLCIMS(string CIMSType, string CIMSItemId)
    {
        string strXML = string.Empty;
        try
        {
            //<Request>
            //  <Detail>
            //    <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}">
            //      <Items />
            //      <Packages />
            //      <Images />
            //      <TherapeuticClasses />
            //      <ATCCodes />
            //      <Companies />
            //      <Identifiers />
            //    </Product>
            //  </Detail>
            //</Request>

            CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";

            strXML = "<Request><Detail><" + CIMSType + " reference=\"" + CIMSItemId + "\"><Items /><Packages /><Images /><TherapeuticClasses /><ATCCodes /><Companies /><Identifiers /></Product></Detail></Request>";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return strXML;
    }

    private string getMonographXML(string CIMSType, string CIMSItemId)
    {
        string strXML = string.Empty;
        try
        {
            //<Request>
            //    <Content>
            //        <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
            //    </Content>
            //</Request>

            //strXML = "<Request><Content><Product reference=\"{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}\" /></Content></Request>";

            // <MONOGRAPH>
            CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";

            strXML = "<Request><Content><" + CIMSType + " reference=\"" + CIMSItemId + "\" /></Content></Request>";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return strXML;
    }


    private string getDIInterationXML(string strNewPrescribing)
    {
        string strXML = string.Empty;
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                //<Request>
                //    <Interaction>
                //        <Prescribing>
                //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
                //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
                //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
                //        </Prescribing>
                //        <Allergies />
                //        <References/>
                //    </Interaction>
                //</Request>

                string strPrescribing = string.Empty;

                StringBuilder ItemIds = new StringBuilder();

                ItemIds.Append(common.myStr(Session["RunningCIMSXMLInputData"]));

                if (!strNewPrescribing.Equals(string.Empty))
                {
                    if (!ItemIds.ToString().ToUpper().Contains(strNewPrescribing))
                    {
                        ItemIds.Append(strNewPrescribing);
                    }
                }

                foreach (GridViewRow dataItem in gvPrevious.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnDIInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDIInteractionCIMS");

                    if ((common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0"))
                    {

                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";

                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                        {
                            ItemIds.Append(strPres);
                        }
                    }
                }

                if (ItemIds.ToString().Equals(string.Empty))
                {
                    return string.Empty;
                }

                //strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";
                strPrescribing = "<Prescribing>" + ItemIds.ToString() + "</Prescribing>";

                strXML = "<Request><Interaction>" + strPrescribing + "<Allergies /><DuplicateTherapy /><DuplicateIngredient /><References /></Interaction></Request>";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return strXML;
    }
    private string getInterationXML(string strNewPrescribing)
    {
        string strXML = string.Empty;
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                //<Request>
                //    <Interaction>
                //        <Prescribing>
                //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
                //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
                //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
                //        </Prescribing>
                //        <Allergies />
                //        <References/>
                //    </Interaction>
                //</Request>

                string strPrescribing = string.Empty;

                StringBuilder ItemIds = new StringBuilder();

                ItemIds.Append(common.myStr(Session["RunningCIMSXMLInputData"]));

                if (!strNewPrescribing.Equals(string.Empty))
                {
                    if (!ItemIds.ToString().ToUpper().Contains(strNewPrescribing))
                    {
                        ItemIds.Append(strNewPrescribing);
                    }
                }

                foreach (GridViewRow dataItem in gvPrevious.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");

                    if ((common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0"))
                    {

                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";

                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                        {
                            ItemIds.Append(strPres);
                        }
                    }
                }

                if (ItemIds.ToString().Equals(string.Empty))
                {
                    return string.Empty;
                }

                //strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";
                strPrescribing = "<Prescribing>" + ItemIds.ToString() + "</Prescribing>";

                strXML = "<Request><Interaction>" + strPrescribing + "<Allergies /><References /></Interaction></Request>";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return strXML;
    }

    private string getHealthOrAllergiesInterationXML(string useFor, string strNewPrescribing)
    {
        string strXML = string.Empty;
        try
        {
            //<Request>
            //    <Interaction>
            //        <Prescribing>
            //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
            //        </Prescribing>
            //        <Prescribed>
            //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
            //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
            //        </Prescribed>
            //        <Allergies>
            //            <Product reference="{8A4E15CD-ACE3-41D9-A367-55658256C2D4}" />
            //            <Product reference="{6D8F3E40-FA33-49C9-9D34-7C13F88E00FD}" />
            //        </Allergies>
            //        <HealthIssueCodes>
            //            <HealthIssueCode code="K22" codeType="ICD10" />
            //            <HealthIssueCode code="K22.0" codeType="ICD10" />
            //        </HealthIssueCodes>
            //        <References/>
            //    </Interaction>
            //</Request>

            string strPrescribing = string.Empty;

            StringBuilder ItemIds = new StringBuilder();

            ItemIds.Append(common.myStr(Session["RunningCIMSXMLInputData"]));

            if (!strNewPrescribing.Equals(string.Empty))
            {
                if (!ItemIds.ToString().ToUpper().Contains(strNewPrescribing))
                {
                    ItemIds.Append(strNewPrescribing);
                }
            }

            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");

                if (common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0")
                {
                    string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                    CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";

                    string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                    if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                    {
                        ItemIds.Append(strPres);
                    }
                }
            }

            if (ItemIds.ToString().Equals(string.Empty))
            {
                return string.Empty;
            }

            strPrescribing = "<Prescribing>" + ItemIds.ToString() + "</Prescribing>";

            switch (useFor)
            {
                case "H"://Helth Interaction
                    strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientDiagnosisXML"]) + "<References /></Interaction></Request>";
                    break;
                case "A"://Allergies
                    strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientAllergyXML"]) + "<References /></Interaction></Request>";
                    break;
                case "B"://Both
                    strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientAllergyXML"]) + common.myStr(ViewState["PatientDiagnosisXML"]) + "<References /></Interaction></Request>";
                    break;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return strXML;
    }

    protected void lnkDrugAllergy_OnClick(object sender, EventArgs e)
    {
        showHealthOrAllergiesIntreraction("A");
    }

    private void openWindowsCIMS(bool IsBrandDetails)
    {
        clsCIMS objCIMS = new clsCIMS();

        hdnCIMSOutput.Value = objCIMS.getCIMSFinalOutupt(IsBrandDetails);

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "OpenCIMSWindow();", true);
        return;

        //RadWindow1.NavigateUrl = "/EMR/Medication/Monograph1.aspx?IsBD=" + IsBrandDetails;
        //RadWindow1.Height = 600;
        //RadWindow1.Width = 900;
        //RadWindow1.Top = 10;
        //RadWindow1.Left = 10;
        ////RadWindow1.OnClientClose = "";
        //RadWindow1.VisibleOnPageLoad = true;
        //RadWindow1.Modal = true;
        //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        //RadWindow1.VisibleStatusbar = false;
    }

    ///////////////VIDAL/////////////////////////////

    private int?[] getVIDALCommonNameGroupIds()
    {
        int?[] commonNameGroupIds = null;
        try
        {
            List<int?> list = new List<int?>();

            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                HiddenField VIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
                LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");

                if (common.myInt(VIDALItemId.Value) > 0)
                //&& (lnkBtnInteractionVIDAL.Visible || lnkBtnDHInteractionVIDAL.Visible))
                {
                    list.Add(common.myInt(VIDALItemId.Value));
                }
            }

            commonNameGroupIds = list.ToArray();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return commonNameGroupIds;
    }

    private void getMonographVidal(int? commonNameGroupId)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        DataTable tbl = new DataTable();
        try
        {
            tbl = objVIDAL.getVIDALMonograph(commonNameGroupId);

            if (tbl.Rows.Count > 0)
            {
                openWindowsVIDAL("?UseFor=MO&URL=" + common.myStr(tbl.Rows[0]["URL"]));
            }

            //ViewState["tblMonographVidal"] = tbl;

            //gvMonographVidal.DataSource = tbl;
            //gvMonographVidal.DataBind();

            //DivMonographVidal.Visible = false;
            //if (tbl.Rows.Count > 0)
            //{
            //    DivMonographVidal.Visible = true;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objVIDAL = null;
            tbl.Dispose();
        }
    }

    private void getDrugToDrugInteractionVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            //commonNameGroupIds = new int?[] { 15223, 15070, 1524, 4025, 4212, 516 };

            StringBuilder sb = new StringBuilder();

            Hashtable collVitalItemIdFound = new Hashtable();

            sb = objVIDAL.getVIDALDrugToDrugInteraction(false, commonNameGroupIds, out collVitalItemIdFound);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != string.Empty)
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsVIDAL("?UseFor=IN");
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
            objVIDAL = null;
        }
    }

    private void getDrugHealthInteractionVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            //int?[] commonNameGroupIds = new int?[] { 1524, 4025, 4212, 516, 28, 29, 30 };

            DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);//yyyy-mm-ddThh:mm:ss 
            //weight = common.myInt(lbl_Weight.Text);//In kilograms
            //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
            int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
            int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

            Hashtable collVitalItemIdFoundDH = new Hashtable();

            StringBuilder sb = objVIDAL.getVIDALDrugHealthInteraction(commonNameGroupIds, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
                    0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
                    (ViewState["PatientDiagnosisXML"] != string.Empty) ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
                    out collVitalItemIdFoundDH);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != string.Empty)
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsVIDAL("?UseFor=HI");
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
            objVIDAL = null;
        }
    }

    private void getDrugAllergyVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            //commonNameGroupIds = new int?[] { 4025, 4212, 516 };

            int?[] allergyIds = null; //new int?[] { 114 };
            int?[] moleculeIds = null;  //new int?[] { 202, 96, 36 };

            if (ViewState["PatientAllergyXML"] != string.Empty)
            {
                allergyIds = (int?[])ViewState["PatientAllergyXML"];
            }

            StringBuilder sb = new StringBuilder();

            sb = objVIDAL.getVIDALDrugAllergyInteraction(commonNameGroupIds, allergyIds, moleculeIds);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != string.Empty)
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsVIDAL("?UseFor=DA");
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
            objVIDAL = null;
        }
    }

    private void getWarningVidal(int? commonNameGroupId)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            StringBuilder sb = new StringBuilder();

            sb = objVIDAL.getVIDALDrugWarning(commonNameGroupId);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != string.Empty)
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsVIDAL("?UseFor=WS");
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
            objVIDAL = null;
        }
    }

    private void getSideEffectVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            StringBuilder sb = new StringBuilder();

            sb = objVIDAL.getVIDALDrugSideEffect(commonNameGroupIds);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != string.Empty)
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsVIDAL("?UseFor=SE");
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
            objVIDAL = null;
        }
    }

    private void openWindowsVIDAL(string parameters)
    {
        RadWindow1.NavigateUrl = "/EMR/Medication/MonographVidal.aspx" + parameters;
        RadWindow1.Height = 550;
        RadWindow1.Width = 800;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        //RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        if (parameters.Contains("UseFor=MO"))
        {
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        RadWindow1.VisibleStatusbar = false;
    }

    protected void gvPrevious_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPrevious.PageIndex = e.NewPageIndex;
        bindData();
    }

    protected void rdoPreviousCurrent_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnCancel.Visible = common.myStr(rdoPreviousCurrent.SelectedValue).Equals("C");
            //Add By Himanshu On Date 17/02/2022 
            btnPrint.Visible = common.myStr(rdoPreviousCurrent.SelectedValue).Equals("C") || common.myStr(rdoPreviousCurrent.SelectedValue).Equals("P");
            bindData();
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

    protected void btnCancel_Onclick(object sender, EventArgs e)
    {
        ArrayList coll = new ArrayList();
        StringBuilder strXML = new StringBuilder();

        try
        {
            foreach (GridViewRow row in gvPrevious.Rows)
            {
                CheckBox chkRow = (CheckBox)row.FindControl("chkRow");
                if (chkRow.Checked)
                {
                    HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                    HiddenField hdnDetailsId = (HiddenField)row.FindControl("hdnDetailsId");
                    HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                    HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");

                    if (common.myInt(hdnDetailsId.Value).Equals(0))
                    {
                        break;
                    }

                    if (!common.myInt(hdnEncounterId.Value).Equals(common.myInt(Session["EncounterId"])))
                    {
                        Alert.ShowAjaxMsg("Cancellation allowed only on current selected encounter !", this.Page);
                        return;
                    }

                    coll.Add(common.myInt(hdnEncounterId.Value));//EncounterId INT
                    coll.Add(common.myInt(hdnIndentId.Value));//IndentId INT
                    coll.Add(common.myInt(hdnDetailsId.Value));//IndentDetailsId INT
                    coll.Add(common.myInt(hdnItemId.Value));//ItemId INT

                    strXML.Append(common.setXmlTable(ref coll));
                }
            }

            ViewState["IndentDetailsId"] = string.Empty;
            ViewState["StopItemId"] = string.Empty;
            ViewState["StopIndentId"] = string.Empty;

            if (strXML.ToString().Equals(string.Empty))
            {
                Alert.ShowAjaxMsg("Please select brand from list !", this.Page);
                return;
            }

            ViewState["CancelBrandList"] = strXML.ToString();

            txtStopRemarks.Text = string.Empty;
            dvConfirmStop.Visible = true;

            lblCancelStopMedicationRemarks.Text = "Cancel Medication Remarks";
            btnStopMedication.Text = "Cancel";
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

    protected void bindPartialIndent(DataSet ds)
    {
        DataView dvPatrialPending = new DataView();

        try
        {
            dvPatrialPending = new DataView(ds.Tables[0]);
            dvPatrialPending.RowFilter = "ISNULL(Qty,0) <> ISNULL(IssueQty,0) AND ISNULL(IssueQty,0) > 0 AND IndentClose <> 1 ";

            DataTable distinctValues = dvPatrialPending.ToTable(true, "IndentId", "IndentNo");

            ddlPartialPendingIndent.DataSource = distinctValues;
            ddlPartialPendingIndent.DataTextField = "IndentNo";
            ddlPartialPendingIndent.DataValueField = "IndentId";
            ddlPartialPendingIndent.DataBind();
            ddlPartialPendingIndent.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlPartialPendingIndent.SelectedIndex = 0;

            lblPendingIndent.Visible = true;
            ddlPartialPendingIndent.Visible = true;
            lblPendingIndentRemarks.Visible = true;
            txtPartialIndentRemarks.Visible = true;
            btnSavePartialPendingIndent.Visible = true;
        }
        catch (Exception)
        {
        }
        finally
        {
        }
    }

    protected void btnSavePartialPendingIndent_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        if (ddlPartialPendingIndent.Visible)
        {
            if (common.myLen(txtPartialIndentRemarks.Text).Equals(0))
            {
                Alert.ShowAjaxMsg("Please enter indent close remarks!!", this.Page);
                return;
            }

            int IndentId = common.myInt(ddlPartialPendingIndent.SelectedValue);
            if (IndentId > 0)
            {
                string message = objEMR.CancelIndentFromWardPreviousMedicine(IndentId, common.myStr(txtPartialIndentRemarks.Text), common.myInt(Session["UserId"]));

                Alert.ShowAjaxMsg(message, Page);
                txtPartialIndentRemarks.Text = string.Empty;

                rdoPreviousCurrent.SelectedValue = "PI";
                bindData();
            }
            else
            {
                Alert.ShowAjaxMsg("Please select the indent first from list", Page);
            }
        }
    }

    //Add By Himanshu On Date 17/02/2022 
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //Pra
        string IndentId = string.Empty;
        foreach (GridViewRow row in gvPrevious.Rows)
        {
            CheckBox chkRow = (CheckBox)row.FindControl("chkRow");
            if (chkRow.Checked)
            {
                HiddenField hdnItemFlagCode = (HiddenField)row.FindControl("hdnItemFlagCode");
                if (hdnItemFlagCode.Value.Trim().ToUpper() == "NARCOTIC")
                {
                    IndentId = ((HiddenField)row.FindControl("hdnIndentId")).Value;
                    break;
                }
            }
        }

        if (!string.IsNullOrEmpty(IndentId))//Pra
        {
            //RadWindow1.NavigateUrl = "/EMR/Templates/PrintNarcoticsSheet.aspx?EncId = " + common.myStr(Session["EncounterId"]);//Pra
            RadWindow1.NavigateUrl = "/EMR/Templates/PrintNarcoticsSheet.aspx?EncId = " + common.myStr(Session["EncounterId"]) + "&IndentId=" + IndentId;// Pra
            RadWindow1.Height = 598;
            RadWindow1.Width = 900;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.Modal = true;
            RadWindow1.OnClientClose = string.Empty; //"OnClientClose";
            RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindow1.VisibleStatusbar = false;
        }
    }

    protected void btnAntibioticsCancel_Click(object sender, EventArgs e)
    {
        dvAntibiotics.Visible = false;
    }
}
