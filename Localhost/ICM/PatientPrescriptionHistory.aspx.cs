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
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;
using BaseC;

public partial class ICM_PatientPrescriptionHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["AddLabCart"] = null;
            Session["IsDischargeMedictionEntered"] = "0";

            ViewState["RegistrationId"] = common.myInt(Request.QueryString["RegId"]);
            ViewState["EncounterId"] = common.myInt(Request.QueryString["EncId"]);

            if (common.myInt(ViewState["RegistrationId"]).Equals(0))
            {
                ViewState["RegistrationId"] = common.myInt(Session["RegistrationId"]);
            }
            if (common.myInt(ViewState["EncounterId"]).Equals(0))
            {
                ViewState["EncounterId"] = common.myInt(Session["EncounterId"]);
            }

            bindPrescription();
            BindBlankLabCartGrid();
        }
    }

    void BindBlankLabCartGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IndentDetailId", typeof(int));
            dt.Columns.Add("IndentId", typeof(int));
            dt.Columns.Add("ItemId", typeof(int));
            dt.Columns.Add("IndentType");
            dt.Columns.Add("IndentDate");
            dt.Columns.Add("IndentNo");
            dt.Columns.Add("DrugName");
            dt.Columns.Add("DoseUnit");
            dt.Columns.Add("FrequencyDescription");
            dt.Columns.Add("DurationType");
            dt.Columns.Add("FoodRelationship");
            dt.Columns.Add("InstructionRemarks");

            gvLabHistoryCart.DataSource = dt;
            gvLabHistoryCart.DataBind();

            ViewState["AddLabCart"] = dt;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        bindPrescription();
    }
    protected void bindPrescription()
    {
        lblMessage.Text = "";
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
           ds = objEMR.getDischargeSummaryPrescription(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]),
                                                            common.myInt(ViewState["EncounterId"]), false,common.myBool(chkStopedMedicine.Checked));
            ViewState["LabData"] = ds.Tables[0];
            gvResultFinal.DataSource = ds.Tables[0];
            gvResultFinal.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    void ChangeColour()
    {
        DataTable dt = (DataTable)ViewState["AddLabCart"];
        foreach (GridItem dr in gvResultFinal.Items)
        {
            HiddenField hdnIndentDetailId = (HiddenField)dr.FindControl("hdnIndentDetailId");

            DataView dv = dt.DefaultView;
            dv.RowFilter = "IndentDetailId=" + common.myInt(hdnIndentDetailId.Value);

            if (dv.ToTable().Rows.Count > 0)
            {
                dr.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                dr.BackColor = System.Drawing.Color.White;
            }
        }
    }
    protected void gvResultFinal_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item
            || e.Item.ItemType == GridItemType.AlternatingItem)
        {
        }
    }
    protected void gvResultFinal_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        lblMessage.Text = string.Empty;
        try
        {
            if (e.CommandName == "AddList")
            {
                HiddenField hdnIndentDetailId = (HiddenField)e.Item.FindControl("hdnIndentDetailId");
                HiddenField hdnIndentId = (HiddenField)e.Item.FindControl("hdnIndentId");
                HiddenField hdnItemId = (HiddenField)e.Item.FindControl("hdnItemId");
                Label lblIndentType = (Label)e.Item.FindControl("lblIndentType");
                Label lblIndentDate = (Label)e.Item.FindControl("lblIndentDate");
                Label lblIndentNo = (Label)e.Item.FindControl("lblIndentNo");
                Label lblDrugName = (Label)e.Item.FindControl("lblDrugName");
                Label lblDoseUnit = (Label)e.Item.FindControl("lblDoseUnit");
                Label lblFrequencyDescription = (Label)e.Item.FindControl("lblFrequencyDescription");
                Label lblDurationType = (Label)e.Item.FindControl("lblDurationType");
                Label lblFoodRelationship = (Label)e.Item.FindControl("lblFoodRelationship");
                Label lblInstructionRemarks = (Label)e.Item.FindControl("lblInstructionRemarks");

                DataTable dt = (DataTable)ViewState["AddLabCart"];
                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.Copy().DefaultView;
                    dv.RowFilter = "IndentDetailId=" + common.myInt(hdnIndentDetailId.Value);
                    if (dv.ToTable().Rows.Count == 0)
                    {
                        DataTable dtDuplicate = (DataTable)ViewState["AddLabCart"];
                        DataView dvDuplicate = dtDuplicate.Copy().DefaultView;

                        dvDuplicate.RowFilter = "ItemId > 0 AND ItemId=" + common.myInt(hdnItemId.Value);

                        if (dvDuplicate.ToTable().Rows.Count > 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "Drug name is already added !";

                            return;
                        }

                        DataRow dr = dt.NewRow();

                        dr["IndentDetailId"] = common.myInt(hdnIndentDetailId.Value);
                        dr["IndentId"] = common.myInt(hdnIndentId.Value);
                        dr["ItemId"] = common.myInt(hdnItemId.Value);
                        dr["IndentType"] = common.myStr(lblIndentType.Text);
                        dr["IndentDate"] = common.myStr(lblIndentDate.Text);
                        dr["IndentNo"] = common.myStr(lblIndentNo.Text);
                        dr["DrugName"] = common.myStr(lblDrugName.Text);
                        dr["DoseUnit"] = common.myStr(lblDoseUnit.Text);
                        dr["FrequencyDescription"] = common.myStr(lblFrequencyDescription.Text);
                        dr["DurationType"] = common.myStr(lblDurationType.Text);
                        dr["FoodRelationship"] = common.myStr(lblFoodRelationship.Text);
                        dr["InstructionRemarks"] = common.myStr(lblInstructionRemarks.Text);

                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();

                    dr["IndentDetailId"] = common.myInt(hdnIndentDetailId.Value);
                    dr["IndentId"] = common.myInt(hdnIndentId.Value);
                    dr["ItemId"] = common.myInt(hdnItemId.Value);
                    dr["IndentType"] = common.myStr(lblIndentType.Text);
                    dr["IndentDate"] = common.myStr(lblIndentDate.Text);
                    dr["IndentNo"] = common.myStr(lblIndentNo.Text);
                    dr["DrugName"] = common.myStr(lblDrugName.Text);
                    dr["DoseUnit"] = common.myStr(lblDoseUnit.Text);
                    dr["FrequencyDescription"] = common.myStr(lblFrequencyDescription.Text);
                    dr["DurationType"] = common.myStr(lblDurationType.Text);
                    dr["FoodRelationship"] = common.myStr(lblFoodRelationship.Text);
                    dr["InstructionRemarks"] = common.myStr(lblInstructionRemarks.Text);

                    dt.Rows.Add(dr);
                }

                dt.AcceptChanges();

                gvLabHistoryCart.DataSource = dt.Copy();
                gvLabHistoryCart.DataBind();
                ViewState["AddLabCart"] = dt;

                ChangeColour();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void gvLabHistoryCart_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                DataTable dt = (DataTable)ViewState["AddLabCart"];
                HiddenField hdnIndentDetailId = (HiddenField)e.Item.FindControl("hdnIndentDetailId");
                DataView dv = dt.DefaultView;
                dv.RowFilter = "IndentDetailId <>" + common.myInt(hdnIndentDetailId.Value);

                dt = dv.ToTable();
                dt.AcceptChanges();
                gvLabHistoryCart.DataSource = dt.Copy();
                gvLabHistoryCart.DataBind();
                ViewState["AddLabCart"] = dt;
                ChangeColour();

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnUpdateSummary_OnClick(object sender, EventArgs e)
    {
        hdnLabData.Value = generateMedicationTabular().ToString();

        ViewState["AddLabCart"] = null;
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }
    private string generateMedicationTabular()
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataView dv = new DataView();
        StringBuilder sbTemp = new StringBuilder();
        BindSummary bnotes = new BindSummary(sConString);

        StringBuilder objXML = new StringBuilder();
        ArrayList coll = new ArrayList();

        try
        {
            if (common.myInt(gvLabHistoryCart.Items.Count) > 0) //&& !chkAllInvestigation.Checked)
            {
                foreach (GridDataItem dataItem in gvLabHistoryCart.Items)
                {
                    HiddenField hdnIndentDetailId = (HiddenField)dataItem.FindControl("hdnIndentDetailId");

                    coll.Add(common.myInt(hdnIndentDetailId.Value));
                    objXML.Append(common.setXmlTable(ref coll));
                }
            }
            //else
            //{
            //    DataTable dt = (DataTable)ViewState["LabData"];

            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        coll.Add(common.myInt(dr["IndentDetailId"]));
            //        objXML.Append(common.setXmlTable(ref coll));
            //    }
            //}

            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            dv = new DataView(dsTemplateStyle.Tables[0]);
            dv.RowFilter = "CODE='PRT'";
            if (dv.Count > 0)
            {
                drTemplateStyle = dv[0].Row;
            }

            if (objXML.ToString() != string.Empty)
            {
                //sbTemplateStyle.Append("<span style='font-weight: 700; font-family: Candara;font-weight:bold;'> MEDICATIONS </span> : <br/><br/>");
                sbTemplateStyle.Append("<br/>");

                bnotes.BindMedicationTabular(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["Facilityid"]),
                                            common.myInt(ViewState["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
                                            Page, string.Empty, objXML.ToString());

                if (sbTemp.ToString() != "")
                    sb.Append(sbTemp + "<br/><br/>");
                else
                    sb.Append("<br/>");
            }
        }
        catch (Exception Ex)
        {
        }
        finally
        {
            sbTemplateStyle = null;
            dsTemplateStyle.Dispose();
            drTemplateStyle = null;
            dv.Dispose();
            sbTemp = null;
            bnotes = null;
            objXML = null;
            coll = null;
        }

        return sb.ToString();
    }
    protected void btnDrugOrder_OnClick(object sender, EventArgs e)
    {
        try
        {
            Session["IsDischargeMedictionEntered"] = "0";

            RadWindowPopup.NavigateUrl = "/EMR/Medication/PrescribeMedication.aspx?Regno=" + common.myStr(Request.QueryString["RegNo"]) +
                                        "&Encno=" + common.myStr(Request.QueryString["Encno"]) +
                                        "&RegId=" + common.myStr(Request.QueryString["RegId"]) +
                                        "&EncId=" + common.myStr(Request.QueryString["EncId"]) +
                                        "&POPUP=POPUP" +
                                        "&DoctorId=" + common.myStr(Request.QueryString["DoctorId"]) +
                                        "&FixedIndentType=1";

            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            RadWindowPopup.Height = 600;
            RadWindowPopup.Width = 1000;
            RadWindowPopup.OnClientClose = "OnClientRefreshButton";
            RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleStatusbar = false;
            RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;

        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            lblMessage.Text = string.Empty;
        }
    }

    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindPrescription();
            bindPrescriptionLastIndent();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            lblMessage.Text = string.Empty;
        }
    }

    protected void bindPrescriptionLastIndent()
    {
        lblMessage.Text = "";
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            if (common.myBool(Session["IsDischargeMedictionEntered"]))
            {
                ds = objEMR.getDischargeSummaryPrescription(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]),
                                                            common.myInt(ViewState["EncounterId"]), true, common.myBool(chkStopedMedicine.Checked));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        DataTable dt = (DataTable)ViewState["AddLabCart"];

                        if (dt != null)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                DataRow dr = dt.NewRow();

                                dr["IndentDetailId"] = common.myInt(item["IndentDetailId"]);
                                dr["IndentId"] = common.myInt(item["IndentId"]);
                                dr["ItemId"] = common.myInt(item["ItemId"]);
                                dr["IndentType"] = common.myStr(item["IndentType"]);
                                dr["IndentDate"] = common.myStr(item["IndentDate"]);
                                dr["IndentNo"] = common.myStr(item["IndentNo"]);
                                dr["DrugName"] = common.myStr(item["DrugName"]);
                                dr["DoseUnit"] = common.myStr(item["DoseUnit"]);
                                dr["FrequencyDescription"] = common.myStr(item["FrequencyDescription"]);
                                dr["DurationType"] = common.myStr(item["DurationType"]);
                                dr["FoodRelationship"] = common.myStr(item["FoodRelationship"]);
                                dr["InstructionRemarks"] = common.myStr(item["InstructionRemarks"]);

                                dt.Rows.Add(dr);

                                dt.AcceptChanges();
                            }

                            gvLabHistoryCart.DataSource = dt.Copy();
                            gvLabHistoryCart.DataBind();
                            ViewState["AddLabCart"] = dt;
                        }
                    }
                }
            }

            ChangeColour();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            Session["IsDischargeMedictionEntered"] = "0";
        }
    }

    protected void chkStopedMedicine_CheckedChanged(object sender, EventArgs e)
    {
        bindPrescription();
    }
}
