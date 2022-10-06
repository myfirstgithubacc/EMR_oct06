using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using Telerik.Web.UI;
using System.Text;
public partial class ICM_PatientVitalHistory : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl = new DAL.DAL();
    BaseC.EMRVitals objv;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["GridData"] = null;
            ViewState["VitalData"] = null;
            dtpfromDate.DateInput.DateFormat = common.myStr(Session["OutputDateformat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Session["OutputDateformat"]);
            BindVitalSignType();
            bindPreviousVitals();
        }
    }
    void BindVitalSignType()
    {
        try
        {
            objv = new BaseC.EMRVitals(sConString);
            DataSet ds = new DataSet();
            ds = objv.GetVitalSignType();
            ddlVitalSigntype.DataSource = ds;
            ddlVitalSigntype.DataTextField = "VitalSignName";
            ddlVitalSigntype.DataValueField = "VitalId";
            ddlVitalSigntype.DataBind();
            ddlVitalSigntype.Items.Insert(0, new RadComboBoxItem("Select All", "0"));
            ddlVitalSigntype.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void bindPreviousVitals()
    {
        try
        {
            DataSet objDs = new DataSet();
            DataTable dt1 = new DataTable();
            DateTime FrmDate = common.myDate(dtpfromDate.SelectedDate);
            DateTime ToDate = common.myDate(dtpToDate.SelectedDate);
            objv = new BaseC.EMRVitals(sConString);
            if (common.myStr(dtpfromDate.SelectedDate) != "" && common.myStr(dtpToDate.SelectedDate) != "")
            {

                objDs = objv.GetPatientPreviousVitals(common.myInt(Session["HospitalLocationId"]),
                                  common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["RegId"]),
                                  0, 2, "", common.myStr(FrmDate.ToString("yyyy/MM/dd")),
                                  common.myStr(ToDate.ToString("yyyy/MM/dd")),
                                  common.myInt(ddlVitalSigntype.SelectedValue), common.myBool(chkAbNormal.Checked));
            }
            else
            {
                objDs = objv.GetPatientPreviousVitals(common.myInt(Session["HospitalLocationId"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["RegId"]),
                                0, 2, common.myStr(ddldateRange.SelectedValue), "", "",
                                common.myInt(ddlVitalSigntype.SelectedValue), common.myBool(chkAbNormal.Checked));
            }
            if (objDs.Tables[1].Rows.Count > 0)
            {

                ViewState["TemperatureSymbol"] = common.myStr(objDs.Tables[1].Rows[0]["Symbol"]);
            }
            if (objDs.Tables[0].Rows.Count > 0)
            {
                lblMessage.Text = "";
                gvPrevious.DataSource = objDs.Tables[0];
                gvPrevious.DataBind();
                ViewState["GridData"] = objDs.Tables[0];
                lblNoOfRows.Text = "Total Record(s): " + objDs.Tables[0].Rows.Count.ToString();
                ChangeColour();
            }
            else
            {
                objDs = new DataSet();
                DataTable dt = new DataTable();
                DataColumn dc1 = new DataColumn("Vital Date");
                DataColumn dc3 = new DataColumn("HT");
                DataColumn dc4 = new DataColumn("WT");
                DataColumn dc5 = new DataColumn("HC");
                DataColumn dc6 = new DataColumn("T");
                DataColumn dc7 = new DataColumn("R");
                DataColumn dc8 = new DataColumn("P");
                DataColumn dc9 = new DataColumn("BPS");
                DataColumn dc10 = new DataColumn("BPD");
                DataColumn dc11 = new DataColumn("MAC");
                DataColumn dc12 = new DataColumn("BMI");
                DataColumn dc13 = new DataColumn("BSA");
                DataColumn dc15 = new DataColumn("SPO2");
                DataColumn dc16 = new DataColumn("Enterd By");
                DataColumn dc17 = new DataColumn("T_ABNORMAL_VALUE");
                DataColumn dc18 = new DataColumn("R_ABNORMAL_VALUE");
                DataColumn dc19 = new DataColumn("P_ABNORMAL_VALUE");
                DataColumn dc20 = new DataColumn("Entered BY");

                dt.Columns.Add(dc1);
                dt.Columns.Add(dc3);
                dt.Columns.Add(dc4);
                dt.Columns.Add(dc5);
                dt.Columns.Add(dc6);
                dt.Columns.Add(dc7);
                dt.Columns.Add(dc8);
                dt.Columns.Add(dc9);
                dt.Columns.Add(dc10);
                dt.Columns.Add(dc11);
                dt.Columns.Add(dc12);
                dt.Columns.Add(dc13);
                dt.Columns.Add(dc15);
                dt.Columns.Add(dc16);
                dt.Columns.Add(dc17);
                dt.Columns.Add(dc18);
                dt.Columns.Add(dc19);
                dt.Columns.Add(dc20);
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                gvPrevious.DataSource = dt;
                gvPrevious.DataBind();
                ViewState["GridData"] = dt;
                lblNoOfRows.Text = lblNoOfRows.Text = "Total Record(s): 0";
                dt.Dispose();
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
    protected void ddldateRange_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (ddldateRange.SelectedValue == "6")
            {
                pnlDatarng.Visible = true;
                dtpfromDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                dtpfromDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
                dtpfromDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
                dtpToDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                dtpToDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
                dtpToDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
            }
            else
            {
                pnlDatarng.Visible = false;
                dtpfromDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                dtpfromDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
                dtpfromDate.DateInput.Text = "";
                dtpfromDate.SelectedDate = null;
                dtpToDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                dtpToDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
                dtpToDate.DateInput.Text = "";
                dtpToDate.SelectedDate = null;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void imgOkPrevValue_Click(object sender, EventArgs e)
    {
        bindPreviousVitals();
    }
    protected void gvPrevious_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.Header) || e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                if (e.Row.RowType.Equals(DataControlRowType.DataRow))
                {
                    string HeaderColumn = string.Empty;

                    Label lblHT = (Label)e.Row.FindControl("lblHT");
                    Label lblWT = (Label)e.Row.FindControl("lblWT");
                    Label lblHC = (Label)e.Row.FindControl("lblHC");
                    Label lblT = (Label)e.Row.FindControl("lblT");
                    Label lblR = (Label)e.Row.FindControl("lblR");
                    Label lblP = (Label)e.Row.FindControl("lblP");
                    Label lblBPS = (Label)e.Row.FindControl("lblBPS");
                    Label lblBPD = (Label)e.Row.FindControl("lblBPD");
                    Label lblMAC = (Label)e.Row.FindControl("lblMAC");
                    Label lblSpO2 = (Label)e.Row.FindControl("lblSpO2");
                    Label lblBMI = (Label)e.Row.FindControl("lblBMI");
                    Label lblBSA = (Label)e.Row.FindControl("lblBSA");
                    Label lblEnteredBy = (Label)e.Row.FindControl("lblEnteredBy");
                    e.Row.BorderColor = System.Drawing.Color.LightGray;
                    if (common.myLen(lblHT.Text) > 0)
                    {
                        lblHT.Attributes.Add("onclick", "setValue('" + common.myStr(lblHT.Text) + "','HT');");
                    }
                    if (common.myLen(lblWT.Text) > 0)
                    {
                        lblWT.Attributes.Add("onclick", "setValue('" + common.myStr(lblWT.Text) + "','WT');");
                    }
                    if (common.myLen(lblHC.Text) > 0)
                    {
                        lblHC.Attributes.Add("onclick", "setValue('" + common.myStr(lblHC.Text) + "','HC');");
                    }
                    if (common.myLen(lblT.Text) > 0)
                    {
                        lblT.Attributes.Add("onclick", "setValue('" + common.myStr(lblT.Text) + "','T');");
                    }
                    if (common.myLen(lblR.Text) > 0)
                    {
                        lblR.Attributes.Add("onclick", "setValue('" + common.myStr(lblR.Text) + "','R');");
                    }
                    if (common.myLen(lblP.Text) > 0)
                    {
                        lblP.Attributes.Add("onclick", "setValue('" + common.myStr(lblP.Text) + "','P');");
                    }
                    if (common.myLen(lblBPS.Text) > 0)
                    {
                        lblBPS.Attributes.Add("onclick", "setValue('" + common.myStr(lblBPS.Text) + "','BPS');");
                    }
                    if (common.myLen(lblBPD.Text) > 0)
                    {
                        lblBPD.Attributes.Add("onclick", "setValue('" + common.myStr(lblBPD.Text) + "','BPD');");
                    }
                    if (common.myLen(lblMAC.Text) > 0)
                    {
                        lblMAC.Attributes.Add("onclick", "setValue('" + common.myStr(lblMAC.Text) + "','MAC');");
                    }
                    if (common.myLen(lblSpO2.Text) > 0)
                    {
                        lblSpO2.Attributes.Add("onclick", "setValue('" + common.myStr(lblSpO2.Text) + "','SPO2');");
                    }
                    if (common.myLen(lblBMI.Text) > 0)
                    {
                        lblBMI.Attributes.Add("onclick", "setValue('" + common.myStr(lblBMI.Text) + "','BMI');");
                    }
                    if (common.myLen(lblBSA.Text) > 0)
                    {
                        lblBSA.Attributes.Add("onclick", "setValue('" + common.myStr(lblBSA.Text) + "','BSA');");
                    }
                }
                if (e.Row.RowType.Equals(DataControlRowType.Header))
                {
                    Label Label32 = (Label)e.Row.FindControl("Label32");

                    if (ViewState["TemperatureSymbol"] != null)
                    {
                        if (common.myStr(ViewState["TemperatureSymbol"]) != string.Empty)
                        {
                            Label32.Text = "T(" + common.myStr(ViewState["TemperatureSymbol"]) + ")";
                        }
                    }

                }
                if (e.Row.RowType.Equals(DataControlRowType.DataRow))
                {
                    HiddenField hdnT_ABNORMAL_VALUE = (HiddenField)e.Row.FindControl("hdnT_ABNORMAL_VALUE");
                    if (common.myInt(hdnT_ABNORMAL_VALUE.Value).Equals(1))
                    {
                        Label lblT = (Label)e.Row.FindControl("lblT");
                        lblT.ForeColor = System.Drawing.Color.Red;
                    }
                    HiddenField hdnR_ABNORMAL_VALUE = (HiddenField)e.Row.FindControl("hdnR_ABNORMAL_VALUE");
                    if (common.myInt(hdnR_ABNORMAL_VALUE.Value).Equals(1))
                    {
                        Label lblR = (Label)e.Row.FindControl("lblR");
                        lblR.ForeColor = System.Drawing.Color.Red;
                    }
                    HiddenField hdnP_ABNORMAL_VALUE = (HiddenField)e.Row.FindControl("hdnP_ABNORMAL_VALUE");
                    if (common.myInt(hdnP_ABNORMAL_VALUE.Value).Equals(1))
                    {
                        Label lblP = (Label)e.Row.FindControl("lblP");
                        lblP.ForeColor = System.Drawing.Color.Red;
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
    protected void gvPrevious_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = string.Empty;
        try
        {
            if (e.CommandName == "AddList")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                Label lblDetails = (Label)row.FindControl("lblDetails");
                Label lblHT = (Label)row.FindControl("lblHT");
                Label lblWT = (Label)row.FindControl("lblWT");
                Label lblHC = (Label)row.FindControl("lblHC");
                Label lblT = (Label)row.FindControl("lblT");
                HiddenField hdnT_ABNORMAL_VALUE = (HiddenField)row.FindControl("hdnT_ABNORMAL_VALUE");
                Label lblR = (Label)row.FindControl("lblR");
                HiddenField hdnR_ABNORMAL_VALUE = (HiddenField)row.FindControl("hdnR_ABNORMAL_VALUE");
                Label lblP = (Label)row.FindControl("lblP");
                HiddenField hdnP_ABNORMAL_VALUE = (HiddenField)row.FindControl("hdnP_ABNORMAL_VALUE");
                Label lblBPS = (Label)row.FindControl("lblBPS");
                Label lblBPD = (Label)row.FindControl("lblBPD");
                Label lblMAC = (Label)row.FindControl("lblMAC");
                Label lblSpO2 = (Label)row.FindControl("lblSpO2");
                Label lblBMI = (Label)row.FindControl("lblBMI");
                Label lblBSA = (Label)row.FindControl("lblBSA");
                Label lblEnteredBy = (Label)row.FindControl("lblEnteredBy");

                if (ViewState["VitalData"] == null)
                {
                    DataTable dt1 = new DataTable();

                    DataColumn dc1 = new DataColumn("VitalDate");
                    DataColumn dc2 = new DataColumn("HT");
                    DataColumn dc3 = new DataColumn("WT");
                    DataColumn dc4 = new DataColumn("HC");
                    DataColumn dc5 = new DataColumn("T");
                    DataColumn dc6 = new DataColumn("R");
                    DataColumn dc7 = new DataColumn("P");
                    DataColumn dc8 = new DataColumn("BPS");
                    DataColumn dc9 = new DataColumn("BPD");
                    DataColumn dc10 = new DataColumn("MAC");
                    DataColumn dc11 = new DataColumn("BMI");
                    DataColumn dc12 = new DataColumn("BSA");
                    DataColumn dc13 = new DataColumn("SPO2");
                    DataColumn dc14 = new DataColumn("EnteredBy");
                    DataColumn dc15 = new DataColumn("T_ABNORMAL_VALUE");
                    DataColumn dc16 = new DataColumn("R_ABNORMAL_VALUE");
                    DataColumn dc17 = new DataColumn("P_ABNORMAL_VALUE");

                    dt1.Columns.Add(dc1);
                    dt1.Columns.Add(dc2);
                    dt1.Columns.Add(dc3);
                    dt1.Columns.Add(dc4);
                    dt1.Columns.Add(dc5);
                    dt1.Columns.Add(dc6);
                    dt1.Columns.Add(dc7);
                    dt1.Columns.Add(dc8);
                    dt1.Columns.Add(dc9);
                    dt1.Columns.Add(dc10);
                    dt1.Columns.Add(dc11);
                    dt1.Columns.Add(dc12);
                    dt1.Columns.Add(dc13);
                    dt1.Columns.Add(dc14);
                    dt1.Columns.Add(dc15);
                    dt1.Columns.Add(dc16);
                    dt1.Columns.Add(dc17);
                    //DataRow dr = dt1.NewRow();
                    //dt1.Rows.Add(dr);
                    ViewState["VitalData"] = dt1;
                }
                DataTable dt = (DataTable)ViewState["VitalData"];
                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.Copy().DefaultView;
                    dv.RowFilter = "VitalDate='" + common.myStr(lblDetails.Text) + "'";
                    if (dv.ToTable().Rows.Count == 0)
                    {
                        DataTable dtDuplicate = (DataTable)ViewState["VitalData"];
                        DataView dvDuplicate = dtDuplicate.Copy().DefaultView;
                        // dvDuplicate.RowFilter = "ItemId > 0 AND Vital Dat=" + common.myInt(lblDetails.Text);
                        dvDuplicate.RowFilter = "VitalDate='" + common.myStr(lblDetails.Text) + "'";
                        if (dvDuplicate.ToTable().Rows.Count > 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "This vital is already added !";
                            return;
                        }
                        DataRow dr = dt.NewRow();

                        dr["VitalDate"] = common.myStr(lblDetails.Text);
                        dr["HT"] = common.myStr(lblHT.Text);
                        dr["WT"] = common.myStr(lblWT.Text);
                        dr["HC"] = common.myStr(lblHC.Text);
                        dr["T"] = common.myStr(lblT.Text);
                        dr["T_ABNORMAL_VALUE"] = common.myStr(hdnT_ABNORMAL_VALUE.Value);
                        dr["R"] = common.myStr(lblR.Text);
                        dr["R_ABNORMAL_VALUE"] = common.myStr(hdnR_ABNORMAL_VALUE.Value);
                        dr["P"] = common.myStr(lblP.Text);
                        dr["P_ABNORMAL_VALUE"] = common.myStr(hdnP_ABNORMAL_VALUE.Value);
                        dr["BPS"] = common.myStr(lblBPS.Text);
                        dr["BPD"] = common.myStr(lblBPD.Text);
                        dr["MAC"] = common.myStr(lblMAC.Text);
                        dr["SpO2"] = common.myStr(lblSpO2.Text);
                        dr["BMI"] = common.myStr(lblBMI.Text);
                        dr["BSA"] = common.myStr(lblBSA.Text);
                        dr["EnteredBY"] = common.myStr(lblEnteredBy.Text);
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["VitalDate"] = common.myStr(lblDetails.Text);
                    dr["HT"] = common.myStr(lblHT.Text);
                    dr["WT"] = common.myStr(lblWT.Text);
                    dr["HC"] = common.myStr(lblHC.Text);
                    dr["T"] = common.myStr(lblT.Text);
                    dr["R"] = common.myStr(lblR.Text);
                    dr["P"] = common.myStr(lblP.Text);
                    dr["BPS"] = common.myStr(lblBPS.Text);
                    dr["BPD"] = common.myStr(lblBPD.Text);
                    dr["MAC"] = common.myStr(lblMAC.Text);
                    dr["SpO2"] = common.myStr(lblSpO2.Text);
                    dr["BMI"] = common.myStr(lblBMI.Text);
                    dr["BSA"] = common.myStr(lblBSA.Text);
                    dr["EnteredBY"] = common.myStr(lblEnteredBy.Text);
                    dr["T_ABNORMAL_VALUE"] = common.myStr(hdnT_ABNORMAL_VALUE.Value);
                    dr["R_ABNORMAL_VALUE"] = common.myStr(hdnR_ABNORMAL_VALUE.Value);
                    dr["P_ABNORMAL_VALUE"] = common.myStr(hdnP_ABNORMAL_VALUE.Value);
                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
                gvVitaltoadd.DataSource = dt.Copy();
                gvVitaltoadd.DataBind();
                ViewState["VitalData"] = dt;
                ChangeColour();

                foreach (GridViewRow dr in gvVitaltoadd.Rows)
                {
                    dr.BackColor = System.Drawing.Color.White;

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
    protected void btnUpdateSummary_OnClick(object sender, EventArgs e)
    {
        hdnLabData.Value = generateMedicationTabular().ToString();

        ViewState["AddLabCart"] = null;
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }
    private string generateMedicationTabular()
    {
        StringBuilder sb = new StringBuilder();

        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataView dv = new DataView();
        StringBuilder sbTemp = new StringBuilder();
        BindSummary bnotes = new BindSummary(sConString);
        StringBuilder objXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        try
        {
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));
            dv = new DataView(dsTemplateStyle.Tables[0]);
            dv.RowFilter = "CODE='VTL'";
            if (dv.Count > 0)
            {
                drTemplateStyle = dv[0].Row;
            }


            DataTable dttabular = (DataTable)ViewState["VitalData"];

            BindVitalsTabular(dttabular, sbTemp);

            if (sbTemp.ToString() != "")
                sb.Append(sbTemp + "<br/>");
            else
                sb.Append("<br/>");

        }
        catch (Exception Ex)
        {
        }
        finally
        {
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
    public StringBuilder BindVitalsTabular(DataTable dt, StringBuilder sb)
    {
        StringBuilder vital = new StringBuilder();
        if (dt.Rows.Count > 0)
        {
            sb.Append("<br/>");
            int totColumns = 0;
            vital.Append("<table border='1' style='font-family:Arial;font-size9px;' cellspacing='0' >");
            vital.Append("<tr>");
            foreach (DataColumn col in dt.Columns)
            {
                if (totColumns < 13)
                {
                    vital.Append("<td style='font-weight:bold;' valign='top'>" + col.ColumnName.ToString() + "</td>");
                    totColumns++;
                }
            }
            vital.Append("</tr>");
            foreach (DataRow vDR in dt.Rows)
            {
                vital.Append("<tr>");
                for (int i = 0; i < totColumns; i++)
                {
                    vital.Append("<td style='valign='top'>" + common.myStr(vDR[i]) + "</td>");
                }
                vital.Append("</tr>");
            }
            vital.Append("</table>");
        }
        sb.Append(vital);
        return sb;
    }
    protected void gvVitaltoadd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Remove")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                DataTable dt = (DataTable)ViewState["VitalData"];
                Label lbltovitaldate = (Label)row.FindControl("lbltovitaldate");
                DataView dv = dt.DefaultView;
                dv.RowFilter = "VitalDate <> '" + common.myStr(lbltovitaldate.Text) + "'";

                dt = dv.ToTable();
                dt.AcceptChanges();
                gvVitaltoadd.DataSource = dt.Copy();
                gvVitaltoadd.DataBind();
                ViewState["VitalData"] = dt;
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
    void ChangeColour()
    {
        DataTable dt = (DataTable)ViewState["VitalData"];

        foreach (GridViewRow dr in gvPrevious.Rows)
        {
            if (dt != null)
            {
                Label lblDetails = (Label)dr.FindControl("lblDetails");

                DataView dv = dt.DefaultView;
                dv.RowFilter = "VitalDate='" + common.myStr(lblDetails.Text) + "'";

                if (dv.ToTable().Rows.Count > 0)
                {
                    dr.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    dr.BackColor = System.Drawing.Color.White;
                }
            }
            else
            {
                dr.BackColor = System.Drawing.Color.White;
            }
        }
    }


}