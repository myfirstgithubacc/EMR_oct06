using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Telerik.Web.UI;
using System.Collections;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;

public partial class EMR_Medication_StopMedication : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    clsExceptionLog objException = new clsExceptionLog();
    clsCIMS objCIMS = new clsCIMS();

    private enum enumColumns : byte
    {
        Sno = 0,
        Select = 1,
        DrugName = 2,
        IndentType = 3,
        TotalQty = 4,
        PrescriptionDetail = 5,
        StopRemarks = 6,
        StartDate = 7,
        EndDate = 8,
        StopDate = 9,
        StopBy=10,
        BrandDetailsCIMS = 11,
        MonographCIMS = 12,
        InteractionCIMS = 13,
        DHInteractionCIMS = 14,
        DAInteractionCIMS = 15,
        BrandDetailsVIDAL = 16,
        MonographVIDAL = 17,
        InteractionVIDAL = 18,
        DHInteractionVIDAL = 19,
        DAInteractionVIDAL = 20
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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

            BindGrid(common.myInt(Request.QueryString["EncId"]));
        }
        setGridColor();
    }

    protected void BindGrid(int EncId)
    {
        try
        {
            DataSet ds = new DataSet();
            objPharmacy = new BaseC.clsPharmacy(sConString);
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                ds = objPharmacy.getPreviousMedicines(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), EncId, 0, 0, "S", "", "", "");
            }
            else
            {
                ds = objEMR.getOPMedicines(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                    common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, 0, "S");
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["GridDataDetail"] = ds.Tables[1];
                gvStop.DataSource = ds.Tables[0];
                gvStop.DataBind();

                setVisiblilityInteraction();
            }
            else
            {
                BindBlankItemGrid();
            }
            ds.Dispose();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvStop_OnRowCreated(object sender, GridViewRowEventArgs e)
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
                Cell_Header.Text = "";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 9;
                Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
                HeaderRow.Cells.Add(Cell_Header);

                Cell_Header = new TableCell();
                Cell_Header.Text = strInterface + " Information";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 5;
                Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
                Cell_Header.ForeColor = System.Drawing.Color.Red;
                Cell_Header.Font.Bold = true;
                HeaderRow.Cells.Add(Cell_Header);

                Cell_Header = new TableCell();
                Cell_Header.Text = string.Empty;
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 1;
                Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
                HeaderRow.Cells.Add(Cell_Header);

                gvStop.Controls[0].Controls.AddAt(0, HeaderRow);
            }

        }
    }


    protected void gvStop_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Visible = false;
        }
            
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = false;

            gvStop.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = false;
            gvStop.Columns[(byte)enumColumns.MonographCIMS].Visible = false;
            gvStop.Columns[(byte)enumColumns.InteractionCIMS].Visible = false;
            gvStop.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = false;
            gvStop.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = false;

            gvStop.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = false;
            gvStop.Columns[(byte)enumColumns.MonographVIDAL].Visible = false;
            gvStop.Columns[(byte)enumColumns.InteractionVIDAL].Visible = false;
            gvStop.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = false;
            gvStop.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = false;

            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                gvStop.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = true;
                gvStop.Columns[(byte)enumColumns.MonographCIMS].Visible = true;
                gvStop.Columns[(byte)enumColumns.InteractionCIMS].Visible = true;
                gvStop.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = true;
                gvStop.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = true;

                HiddenField hdnCIMSItemId = (HiddenField)e.Row.FindControl("hdnCIMSItemId");
                LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsCIMS");
                LinkButton lnkBtnMonographCIMS = (LinkButton)e.Row.FindControl("lnkBtnMonographCIMS");
                LinkButton lnkBtnInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnInteractionCIMS");
                LinkButton lnkBtnDHInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionCIMS");
                LinkButton lnkBtnDAInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionCIMS");

                lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                if (common.myStr(hdnCIMSItemId.Value).Trim().Length == 0
                    || common.myStr(hdnCIMSItemId.Value).Trim() == "0")
                {
                    lnkBtnBrandDetailsCIMS.Visible = false;
                    lnkBtnMonographCIMS.Visible = false;
                    lnkBtnInteractionCIMS.Visible = false;
                    lnkBtnDHInteractionCIMS.Visible = false;
                    lnkBtnDAInteractionCIMS.Visible = false;
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
                gvStop.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = true;
                gvStop.Columns[(byte)enumColumns.MonographVIDAL].Visible = true;
                gvStop.Columns[(byte)enumColumns.InteractionVIDAL].Visible = true;
                gvStop.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = true;
                gvStop.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = true;

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

            Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");
            HiddenField hdnGItemId = (HiddenField)e.Row.FindControl("hdnItemId");
            HiddenField hdnGGenericId = (HiddenField)e.Row.FindControl("hdnGenericId");
            HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");
            CheckBox chkRow = (CheckBox)e.Row.FindControl("chkRow");

            if (common.myInt(hdnGGenericId.Value) > 0 && common.myInt(hdnGItemId.Value) <= 0)
            {
                chkRow.Visible = false;
                e.Row.BackColor = System.Drawing.Color.LightBlue;
            }

            if (ViewState["GridDataDetail"] != null)
            {
                DataTable dt = (DataTable)ViewState["GridDataDetail"];
                DataView dv = new DataView(dt);
                dv.RowFilter = "IndentId=" + common.myInt(hdnIndentId.Value) + " AND ItemId=" + common.myInt(hdnItemId.Value);
                if (dv.ToTable().Rows.Count > 0)
                {
                    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
                    lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(dv.ToTable());
                }
                else
                {
                    DataView dv1 = new DataView(dt);
                    dv1.RowFilter = "ISNULL(IndentId,0)=" + hdnIndentId.Value;
                    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
                    if (dv1.ToTable().Rows.Count > 0)
                    {
                        lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(dv1.ToTable());
                    }
                }
            }

            Label lblTotalQty = (Label)e.Row.FindControl("lblTotalQty");
            lblTotalQty.Text = common.myDbl(lblTotalQty.Text).ToString("F0");
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

        return tbl;
    }

    protected void btnReOrder_Onclick(object sender, EventArgs e)
    {
        DataTable tbl = new DataTable();
        string sOPIPs = string.Empty, sOPIP = string.Empty;
        string sDetailsIds = string.Empty, sDetailsId = string.Empty;
        string sIndentIds = string.Empty, sIndentId = string.Empty;
        string sItemIds = string.Empty, sItemId = string.Empty;
        int countOPIPId = 0, countDetailsId = 0, countIndentId = 0, countItemId = 0;
        try
        {
            tbl = getReorderOverrideComments();

            foreach (GridViewRow row in gvStop.Rows)
            {
                CheckBox chkRow = (CheckBox)row.FindControl("chkRow");
                if (chkRow.Checked)
                {
                    HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
                    HiddenField hdnGenericId = (HiddenField)row.FindControl("hdnGenericId");
                    HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                    HiddenField hdnDetailsId = (HiddenField)row.FindControl("hdnDetailsId");
                    Label lblSource = (Label)row.FindControl("lblSource");

                    if (common.myBool(Session["IsCIMSInterfaceActive"])
                        || common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        HiddenField hdnCommentsDrugAllergy = (HiddenField)row.FindControl("hdnCommentsDrugAllergy");
                        HiddenField hdnCommentsDrugToDrug = (HiddenField)row.FindControl("hdnCommentsDrugToDrug");
                        HiddenField hdnCommentsDrugHealth = (HiddenField)row.FindControl("hdnCommentsDrugHealth");

                        if (common.myInt(hdnItemId.Value) > 0
                            || common.myInt(hdnGenericId.Value) > 0)
                        {
                            DataRow DR = tbl.NewRow();
                            DR["ItemId"] = common.myStr(hdnItemId.Value);
                            DR["GenericId"] = common.myStr(hdnGenericId.Value);
                            DR["CommentsDrugAllergy"] = common.myStr(hdnCommentsDrugAllergy.Value);
                            DR["CommentsDrugToDrug"] = common.myStr(hdnCommentsDrugToDrug.Value);
                            DR["CommentsDrugHealth"] = common.myStr(hdnCommentsDrugHealth.Value);

                            tbl.Rows.Add(DR);
                        }
                    }

                    if (common.myInt(hdnIndentId.Value) > 0 && common.myInt(hdnItemId.Value) > 0)
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
                        else if (posItem == 0)
                        {
                            Alert.ShowAjaxMsg("Duplicate items found!", this.Page);
                            return;
                        }
                    }
                }
            }
            if (sIndentId == string.Empty || sItemIds == string.Empty)
            {
                Alert.ShowAjaxMsg("Please select Item to Reorder", Page);
                return;
            }

            hndPageOPIPSource.Value = sOPIPs;
            hdnPageDetailsIds.Value = sDetailsIds;
            hdnPageIndentIds.Value = sIndentIds;
            hdnPageItemIds.Value = sItemIds;
            Session["TblReorderOverrideComments"] = tbl;

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

    private DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("DetailsId", typeof(int));
        dt.Columns.Add("IndentTypeId", typeof(int));
        dt.Columns.Add("IndentType", typeof(string));
        dt.Columns.Add("GenericId", typeof(int));
        dt.Columns.Add("GenericName", typeof(string));
        dt.Columns.Add("ItemId", typeof(int));
        dt.Columns.Add("IndentId", typeof(int));
        dt.Columns.Add("ItemName", typeof(string));
        dt.Columns.Add("FormulationId", typeof(int));
        dt.Columns.Add("RouteId", typeof(int));
        dt.Columns.Add("StrengthId", typeof(int));
        dt.Columns.Add("CIMSItemId", typeof(string));
        dt.Columns.Add("CIMSType", typeof(string));
        dt.Columns.Add("VIDALItemId", typeof(int));
        dt.Columns.Add("Qty", typeof(decimal));
        dt.Columns.Add("PrescriptionDetail", typeof(string));
        dt.Columns.Add("ReferanceItemId", typeof(int));
        dt.Columns.Add("CustomMedication", typeof(string));
        dt.Columns.Add("XMLData", typeof(String));
        dt.Columns.Add("StopRemarks", typeof(String));
        dt.Columns.Add("StartDate", typeof(string));
        dt.Columns.Add("EndDate", typeof(string));
        dt.Columns.Add("StopDate", typeof(string));
        dt.Columns.Add("Source", typeof(string));

        dt.Columns.Add("OverrideComments", typeof(string));
        dt.Columns.Add("OverrideCommentsDrugToDrug", typeof(string));
        dt.Columns.Add("OverrideCommentsDrugHealth", typeof(string));
        dt.Columns.Add("StopBy", typeof(string)); 
        return dt;

    }
    private void BindBlankItemGrid()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();

        dr["DetailsId"] = 0;
        dr["IndentTypeId"] = 0;
        dr["IndentType"] = "";
        dr["GenericId"] = 0;
        dr["ItemId"] = 0;
        dr["IndentId"] = 0;
        dr["GenericName"] = "";
        dr["ItemName"] = "";
        dr["FormulationId"] = 0;
        dr["RouteId"] = 0;
        dr["StrengthId"] = 0;
        dr["CIMSItemId"] = "";
        dr["CIMSType"] = "";
        dr["VIDALItemId"] = 0;
        dr["Qty"] = 0.00;
        dr["PrescriptionDetail"] = "";
        dr["ReferanceItemId"] = 0;
        dr["XMLData"] = "";
        dr["CustomMedication"] = "";
        dr["StopRemarks"] = "";
        dr["StartDate"] = "";
        dr["EndDate"] = "";
        dr["StopDate"] = "";
        dr["StopBy"] = ""; 
        dt.Rows.Add(dr);
        dt.AcceptChanges();
        ViewState["ItemDetail"] = null;
        gvStop.DataSource = dt;
        gvStop.DataBind();
        ViewState["DataTableItem"] = dt;

        setVisiblilityInteraction();
    }

    protected void chkRow_OnCheckedChanged(object sender, EventArgs e)
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
            foreach (GridViewRow dataItem in gvStop.Rows)
            {
                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsCIMS");
                    LinkButton lnkBtnMonographCIMS = (LinkButton)dataItem.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                    LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");

                    lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
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
                foreach (GridViewRow dataItem in gvStop.Rows)
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

                string outputValuesDD = string.Empty;
                string outputValuesDH = string.Empty;
                string outputValuesDA = string.Empty;

                if (common.myLen(strXMLDD) > 0 || common.myLen(strXMLDH) > 0 || common.myLen(strXMLDA) > 0)
                {
                    outputValuesDD = objCIMS.getFastTrack5Output(strXMLDD);
                    outputValuesDH = objCIMS.getFastTrack5Output(strXMLDH);
                    outputValuesDA = objCIMS.getFastTrack5Output(strXMLDA);

                    foreach (GridViewRow dataItem in gvStop.Rows)
                    {
                        HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");

                        if (common.myLen(hdnCIMSItemId.Value) > 2)
                        {
                            string strCIMSItemPatternMatch = "<" + ((common.myLen(hdnCIMSType.Value) > 0) ? common.myStr(hdnCIMSType.Value) : "Product") + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                            LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                            lnkBtnInteractionCIMS.Visible = false;
                            lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

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

                    foreach (GridViewRow dataItem in gvStop.Rows)
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

                foreach (GridViewRow dataItem in gvStop.Rows)
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

            foreach (GridViewRow dataItem in gvStop.Rows)
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

            foreach (GridViewRow dataItem in gvStop.Rows)
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
        //RadWindow1.NavigateUrl = "/EMR/Medication/MonographVidal.aspx" + parameters;
        //RadWindow1.Height = 550;
        //RadWindow1.Width = 800;
        //RadWindow1.Top = 10;
        //RadWindow1.Left = 10;
        ////RadWindow1.OnClientClose = "";
        //RadWindow1.VisibleOnPageLoad = true;
        //RadWindow1.Modal = true;
        //if (parameters.Contains("UseFor=MO"))
        //{
        //    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        //}
        //RadWindow1.VisibleStatusbar = false;
    }

}
