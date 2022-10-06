using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;

public partial class ICM_MedicalOncologySchedule : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    string path = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        string url = HttpContext.Current.Request.Url.AbsoluteUri;
        path = url.Replace("http://", "");
        path = "http://" + path.Substring(0, path.IndexOf("/") + 1);
        if (!IsPostBack)
        {

            if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "")
            {
                if (common.myInt(Session["RegistrationID"]) > 0)
                {

                    BindCycle();
                    FillPatientData();
                    BindOncologyDetails();
                    OnCologyGridViewRow();
                }
            }
            else
            {
                BindCycle();
                OnCologyGridViewRow();
            }
            if (Request.QueryString["From"] != null && !common.myStr(Request.QueryString["From"]).Equals(string.Empty))
            {
                if (common.myStr(Request.QueryString["From"]).Equals("ward"))
                {
                    lblHeading.Text = "Chemotheraphy Schedule";
                }
            }

        }

    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["POPUP"]) == "1")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void FillPatientData()
    {
        DataSet dsPatientDetail = new DataSet();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (Request.QueryString["AlertType"] != null)
            {
                if (common.myInt(Session["EncounterId"]) != common.myInt(Request.QueryString["EId"]) && common.myInt(Request.QueryString["EId"]) != 0)
                {
                    dsPatientDetail = (DataSet)Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])];
                }
                else
                {
                    dsPatientDetail = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];
                }
            }
            else
            {
                dsPatientDetail = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];
            }

            if (dsPatientDetail != null)
            {

                if (dsPatientDetail.Tables.Count > 0)
                {
                    if (dsPatientDetail.Tables[0].Rows.Count > 0)
                    {
                        string[] arrSex;
                        txtName.Text = dsPatientDetail.Tables[0].Rows[0]["PatientName"].ToString();
                        txtAgeSex.Text = dsPatientDetail.Tables[0].Rows[0]["AgeGender"].ToString();
                        txtMRDNo.Text = dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"].ToString();
                        txtIPNo.Text = dsPatientDetail.Tables[0].Rows[0]["EncounterNo"].ToString();
                        txtAdmittingDoctor.Text = dsPatientDetail.Tables[0].Rows[0]["DoctorName"].ToString();
                        txtAdmissionDate.Text = dsPatientDetail.Tables[0].Rows[0]["AdmissionDate"].ToString();
                        hdnEncounterId.Value = dsPatientDetail.Tables[0].Rows[0]["EncounterId"].ToString();
                        hdnRegistrationId.Value = dsPatientDetail.Tables[0].Rows[0]["RegistrationId"].ToString();
                        arrSex = txtAgeSex.Text.Split('/');
                        hdnGender.Value = arrSex[1].ToString().ToUpper().Trim();
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
            if (dsPatientDetail != null)
            {
                dsPatientDetail.Dispose();
            }

        }
    }
    protected void cmbCycle_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindOncologyDetails();
        OnCologyGridViewRow();
    }
    private void BindOncologyDetails()
    {
        string chvName = string.Empty;
        string MobileNo = string.Empty;
        Int64 chvRegistrationNo = 0;

        BaseC.clsEMR objIcm = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            string FromDate = "1900/01/01", ToDate = "2079/01/01";
            int Cycle = 0;
            if (!chkAll.Checked)
            {
                Cycle = common.myInt(cmbCycle.SelectedValue);
                cmbCycle.Enabled = true;
            }
            else
            {
                cmbCycle.Enabled = false;
            }
            //ds = objIcm.GetEMRChemoTherapyCycleDetails(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), common.myInt(Cycle), 
            //                                            common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value), 
            //                                            chvName, MobileNo, chvRegistrationNo);
            ds = objIcm.GetEMRChemoTherapyCycleDetails(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), common.myInt(Cycle),
                                                      common.myInt(hdnRegistrationId.Value), 0,
                                                      chvName, MobileNo, chvRegistrationNo);
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
                ds.Tables[0].AcceptChanges();
            }
            ViewState["CurrentTable"] = (DataTable)ds.Tables[0];

            if (!ds.Tables[1].Equals(null) && ds.Tables[1].Rows.Count > 0)
            {

                txtHeight.Text = common.myStr(ds.Tables[1].Rows[0]["Height"]);
                txtWeight.Text = common.myStr(ds.Tables[1].Rows[0]["Weight"]);
                txtBSA.Text = common.myStr(ds.Tables[1].Rows[0]["BSA"]);
                txtChemoProtocol.Text = common.myStr(ds.Tables[1].Rows[0]["ChemoTherayProtocol"]);
                txtDiagnosis.Text = common.myStr(ds.Tables[1].Rows[0]["Diagnosis"]);
            }
            else
            {

                txtHeight.Text = "0";
                txtWeight.Text = "0";
                txtBSA.Text = "0.00";
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
            objIcm = null;
            ds.Dispose();
        }
    }
    private void OnCologyGridViewRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("Cycle", typeof(string)));
        dt.Columns.Add(new DataColumn("DAY", typeof(string)));
        dt.Columns.Add(new DataColumn("Date", typeof(string)));
        dt.Columns.Add(new DataColumn("Chemoshedule", typeof(string)));
        dt.Columns.Add(new DataColumn("Id", typeof(string)));
        dt.Columns.Add(new DataColumn("Cancel", typeof(string)));
        dr = dt.NewRow();
        dr["Cycle"] = string.Empty;
        dr["DAY"] = string.Empty;
        dr["Date"] = string.Empty;
        dr["Chemoshedule"] = string.Empty;
        dr["Id"] = 0;
        dr["Cancel"] = string.Empty;
        dt.Rows.Add(dr);
        if (ViewState["CurrentTable"] != null)
        {
            dt = (DataTable)ViewState["CurrentTable"];
        }
        else
        {
            ViewState["CurrentTable"] = dt;
        }
        grvOncologySchedule.DataSource = dt;
        grvOncologySchedule.DataBind();
    }
    private void BindCycle()
    {
        cmbCycle.Items.Clear();
        for (int i = 1; i <= 20; i++)
        {
            cmbCycle.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
        }
        cmbCycle.Items.Insert(0, new RadComboBoxItem("All", "0"));
        cmbCycle.SelectedIndex = 0;

    }

    private void AddNewRow()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    DropDownList ddlCycle = (DropDownList)grvOncologySchedule.Rows[rowIndex].Cells[1].FindControl("ddlCycle");
                    TextBox txtDay = (TextBox)grvOncologySchedule.Rows[rowIndex].Cells[2].FindControl("txtDay");
                    TextBox txtDate = (TextBox)grvOncologySchedule.Rows[rowIndex].Cells[3].FindControl("txtDate");
                    TextBox txtChemoSchedule = (TextBox)grvOncologySchedule.Rows[rowIndex].Cells[4].FindControl("txtChemoSchedule");
                    Label lblSheduleDetailId = (Label)grvOncologySchedule.Rows[rowIndex].Cells[5].FindControl("lblSheduleDetailId");
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows[i - 1]["Cycle"] = ddlCycle.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["DAY"] = txtDay.Text;
                    dtCurrentTable.Rows[i - 1]["Date"] = txtDate.Text;
                    dtCurrentTable.Rows[i - 1]["Chemoshedule"] = txtChemoSchedule.Text;
                    if (common.myInt(lblSheduleDetailId.Text) > 0)
                    {
                        dtCurrentTable.Rows[i - 1]["Id"] = lblSheduleDetailId.Text;
                    }
                    else
                    {
                        dtCurrentTable.Rows[i - 1]["Id"] = 0;
                    }
                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                grvOncologySchedule.DataSource = dtCurrentTable;
                grvOncologySchedule.DataBind();

            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
        SetPreviousOncologyData();
    }
    private void SetPreviousOncologyData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DropDownList ddlCycle = (DropDownList)grvOncologySchedule.Rows[rowIndex].Cells[0].FindControl("ddlCycle");
                    TextBox txtDay = (TextBox)grvOncologySchedule.Rows[rowIndex].Cells[1].FindControl("txtDay");
                    TextBox txtDate = (TextBox)grvOncologySchedule.Rows[rowIndex].Cells[2].FindControl("txtDate");
                    TextBox txtChemoSchedule = (TextBox)grvOncologySchedule.Rows[rowIndex].Cells[3].FindControl("txtChemoSchedule");
                    Label lblSheduleDetailId = (Label)grvOncologySchedule.Rows[rowIndex].Cells[4].FindControl("lblSheduleDetailId");
                    ddlCycle.SelectedValue = common.myStr(dt.Rows[i]["Cycle"]);
                    txtDay.Text = common.myStr(dt.Rows[i]["DAY"]);
                    txtDate.Text = common.myStr(dt.Rows[i]["Date"]);
                    txtChemoSchedule.Text = common.myStr(dt.Rows[i]["Chemoshedule"]);
                    lblSheduleDetailId.Text = common.myStr(dt.Rows[i]["Id"]);
                    rowIndex++;
                }
            }
        }
    }
    private void SetRowData()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i < dtCurrentTable.Rows.Count; i++)
                {
                    DropDownList ddlCycle = (DropDownList)grvOncologySchedule.Rows[rowIndex].Cells[0].FindControl("ddlCycle");
                    TextBox txtDay = (TextBox)grvOncologySchedule.Rows[rowIndex].Cells[1].FindControl("txtDay");
                    TextBox txtDate = (TextBox)grvOncologySchedule.Rows[rowIndex].Cells[2].FindControl("txtDate");
                    TextBox txtChemoSchedule = (TextBox)grvOncologySchedule.Rows[rowIndex].Cells[3].FindControl("txtChemoSchedule");
                    Label lblSheduleDetailId = (Label)grvOncologySchedule.Rows[rowIndex].Cells[4].FindControl("lblSheduleDetailId");
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows[i - 1]["Cycle"] = ddlCycle.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["DAY"] = txtDay.Text;
                    dtCurrentTable.Rows[i - 1]["Date"] = txtDate.Text;
                    dtCurrentTable.Rows[i - 1]["Chemoshedule"] = txtChemoSchedule.Text;
                    dtCurrentTable.Rows[i - 1]["Id"] = lblSheduleDetailId.Text;
                    rowIndex++;
                }

                ViewState["CurrentTable"] = dtCurrentTable;
            }
        }


    }

    protected void btnAddrow_Click(object sender, EventArgs e)
    {
        AddNewRow();
    }
    protected void grvOncologySchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlCycle = (DropDownList)e.Row.FindControl("ddlCycle");
            Label lblSheduleDetailId = (Label)e.Row.FindControl("lblSheduleDetailId");
            Button btnRemoveRow = (Button)e.Row.FindControl("btnRemoveRow");
            CheckBox ChkCancel = (CheckBox)e.Row.FindControl("ChkCancel");
            Label lblCycle = (Label)e.Row.FindControl("lblCycle");

            if (ddlCycle != null)
            {
                ddlCycle.Items.Clear();
                for (int i = 1; i <= 20; i++)
                {
                    ddlCycle.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            if (common.myInt(lblSheduleDetailId.Text) > 0)
            {
                ChkCancel.Visible = true;
                btnRemoveRow.Visible = false;
            }
            else
            {
                ChkCancel.Visible = false;
                btnRemoveRow.Visible = true;
            }
            if (common.myInt(lblCycle.Text) > 0)
            {
                ddlCycle.SelectedValue = lblCycle.Text.Trim();
            }

        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveData();
    }
    private void SaveData()
    {
        BaseC.clsEMR ObjEmr = new BaseC.clsEMR(sConString);
        DataTable table = ViewState["CurrentTable"] as DataTable;
        StringBuilder builder = new StringBuilder();
        string strMsg = String.Empty;
        String Status = string.Empty;
        string strDate = string.Empty;
        int DoctorCode = 0;
        try
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select Patient";
                return;
            }
            if (string.IsNullOrEmpty(txtChemoProtocol.Text))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please enter ChemioTherapy Protocol";
                return;
            }
            SetRowData();
            if (table != null)
            {

                builder.Append("<Root>");
                foreach (GridViewRow row in grvOncologySchedule.Rows)
                {
                    String strStatus = string.Empty;
                    Label lblSheduleDetailId = (Label)row.FindControl("lblSheduleDetailId");
                    DropDownList ddlCycle = (DropDownList)row.FindControl("ddlCycle");
                    TextBox txtDay = (TextBox)row.FindControl("txtDay");
                    TextBox txtDate = (TextBox)row.FindControl("txtDate");
                    TextBox txtChemoSchedule = (TextBox)row.FindControl("txtChemoSchedule");
                    CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
                    strStatus = ChkCancel.Checked ? "0" : "1";
                    strDate = !(txtDate.Text.Equals(string.Empty)) ? common.myDate(txtDate.Text).ToString("yyyy-MM-dd") : null;
                    builder.Append("<ChemoTherapy>");
                    builder.Append("<Id>" + common.myInt(lblSheduleDetailId.Text) + "</Id>");
                    builder.Append("<Cycle>" + common.myInt(ddlCycle.SelectedValue) + "</Cycle>");
                    builder.Append("<CycleInterval>" + txtDay.Text + "</CycleInterval>");
                    builder.Append("<CycleIntervalDate>" + strDate + "</CycleIntervalDate>");
                    builder.Append("<ChemoChedule>" + txtChemoSchedule.Text + "</ChemoChedule>");
                    builder.Append("<Status>" + strStatus + "</Status>");
                    builder.Append("</ChemoTherapy>");
                }

                builder.Append("</Root>");
                string sbXML = builder.ToString();
                Status = "1";

                strMsg = ObjEmr.EMRSaveUpdateChemoTherapySchedule(common.myInt(Session["HospitalLocationID"]), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value),
                                       DoctorCode, common.myInt(cmbCycle.SelectedValue), common.myInt(txtWeight.Text), common.myInt(txtHeight.Text), common.myStr(string.IsNullOrEmpty(txtBSA.Text) ? "0.00" : txtBSA.Text), common.myStr(txtChemoProtocol.Text),
                                       common.myStr(txtDiagnosis.Text), Status, common.myInt(Session["UserID"]), sbXML);
            }
            if ((strMsg.ToUpper().Contains("RECORD(S) SAVED...")))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;
                BindOncologyDetails();
                OnCologyGridViewRow();
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strMsg;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            ObjEmr = null;
            builder = null;
            table.Dispose();
        }
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        cmbCycle_SelectedIndexChanged(null, null);
    }

    protected void btnCalender_Click(object sender, EventArgs e)
    {

        DateTime CurrentDate = Convert.ToDateTime(hdnCurrentDate.Value);
        string strpreviousdate = string.Empty;
        DateTime PreviousDate = common.myDate("1900/01/01");
        if (!common.myStr(hdnPreviousDate.Value).Equals("__/__/____") && !common.myStr(hdnPreviousDate.Value).Equals(string.Empty))
        {
            strpreviousdate = common.myDate(hdnPreviousDate.Value).ToString("yyyy-MM-dd");
            PreviousDate = Convert.ToDateTime(strpreviousdate);
        }
        FillDay(CurrentDate, PreviousDate);
    }
    //private void FillDay(DateTime CurrentDate, DateTime PreviousDate)
    //{
    //    try
    //    {
    //        int I = 0;
    //        DateTime FirstDate;
    //        double Days = 0;
    //        int Cycles = 0;
    //        ((TextBox)grvOncologySchedule.Rows[0].Cells[1].FindControl("txtDay")).Text = "1";
    //        Cycles = common.myInt(((DropDownList)grvOncologySchedule.Rows[0].Cells[0].FindControl("ddlCycle")).SelectedValue);
    //        ViewState["Cycles"] = Cycles;
    //        double Daychabges = 0;
    //        Daychabges = CurrentDate.Subtract(Convert.ToDateTime(PreviousDate)).TotalDays;
    //        string FDate = common.myDate(common.myStr(((TextBox)grvOncologySchedule.Rows[0].Cells[2].FindControl("txtDate")).Text)).ToString("yyyy-MM-dd");
    //        FirstDate = common.myDate(FDate);
    //        if (ViewState["CurrentTable"] != null)
    //        {
    //            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
    //            if (dtCurrentTable.Rows.Count > 0 &&  common.myInt(hdnRowIndex.Value)>1)
    //            {
    //                int ckIndex = common.myInt(hdnRowIndex.Value) - 1;
    //                for (I = 1; I < dtCurrentTable.Rows.Count; I++)
    //                {
    //                    DateTime CycleDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text).ToString("dd/MM/yyyy"));
    //                    if (!string.IsNullOrEmpty(common.myStr(CycleDate)))
    //                    {
    //                        int CKCycle = common.myInt(((DropDownList)grvOncologySchedule.Rows[I].Cells[0].FindControl("ddlCycle")).SelectedValue);
    //                        if (common.myInt(ViewState["Cycles"])!=CKCycle)
    //                        {

    //                            FirstDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));
    //                            DateTime fromSecondrowDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));
    //                          //  DateTime ckChangeDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[common.myInt(hdnRowIndex.Value)].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));
    //                            Days = FirstDate.Subtract(fromSecondrowDate).TotalDays;

    //                            ((TextBox)grvOncologySchedule.Rows[I].Cells[1].FindControl("txtDay")).Text = common.myStr(Days + 1);
    //                            if (!PreviousDate.Equals(common.myDate("1900/01/01")) && !ckIndex.Equals(I))
    //                              {
    //                                    string strDate = common.myStr(fromSecondrowDate.AddDays(Daychabges).ToString("dd/MM/yyyy"));
    //                                    ((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text = strDate;
    //                            }
    //                        }
    //                        ViewState["Cycles"] = common.myInt(((DropDownList)grvOncologySchedule.Rows[I].Cells[0].FindControl("ddlCycle")).SelectedValue);
    //                    }

    //                }
    //            }
    //            hdnRowIndex.Value ="0";
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    //private void FillDay(DateTime CurrentDate, DateTime PreviousDate)
    //{
    //    try
    //    {
    //        string currentdate = hdnCurrentDate.Value;
    //        int I = 0;
    //        DateTime FirstDate;
    //        int PreviousDay = 0;
    //        int currentDay = 0;
    //        double Days = 0;
    //        int Cycles = 0;
    //        ((TextBox)grvOncologySchedule.Rows[0].Cells[1].FindControl("txtDay")).Text = "1";
    //        Cycles = common.myInt(((DropDownList)grvOncologySchedule.Rows[0].Cells[0].FindControl("ddlCycle")).SelectedValue);
    //       ViewState["Cycles"] = Cycles;
    //        double Daychabges = 0;
    //        Daychabges = CurrentDate.Subtract(Convert.ToDateTime(PreviousDate)).TotalDays;
    //        string FDate = common.myDate(common.myStr(((TextBox)grvOncologySchedule.Rows[0].Cells[2].FindControl("txtDate")).Text)).ToString("yyyy-MM-dd");
    //        FirstDate = common.myDate(FDate);
    //        if (ViewState["CurrentTable"] != null)
    //        {
    //            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
    //            if (dtCurrentTable.Rows.Count > 0 && common.myInt(hdnRowIndex.Value) > 0)
    //            {
    //                int ckIndex = common.myInt(hdnRowIndex.Value) - 1;
    //                for (I = 1; I < dtCurrentTable.Rows.Count; I++)
    //                {
    //                    DateTime CycleDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text).ToString("dd/MM/yyyy"));
    //                    if (!string.IsNullOrEmpty(common.myStr(CycleDate)))
    //                    {
    //                        int CKCycle = common.myInt(((DropDownList)grvOncologySchedule.Rows[I].Cells[0].FindControl("ddlCycle")).SelectedValue);
    //                        //if (common.myInt(ViewState["Cycles"]) != CKCycle)
    //                        //{
    //                        if (common.myInt(ViewState["Cycles"]).Equals(CKCycle))
    //                        {
    //                            FirstDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I - 1].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));
    //                            DateTime fromSecondrowDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));

    //                            PreviousDay = common.myInt((int)common.myInt(((TextBox)grvOncologySchedule.Rows[I - 1].Cells[2].FindControl("txtDay")).Text));
    //                            currentDay = common.myInt((int)common.myInt(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDay")).Text));
    //                            //FirstDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I - 1].Cells[2].FindControl("txtDay")).Text).ToString("yyyy-MM-dd"));
    //                            //DateTime fromSecondrowDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));

    //                            //  DateTime ckChangeDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[common.myInt(hdnRowIndex.Value)].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));
    //                            //Days = FirstDate.Subtract(fromSecondrowDate).TotalDays;
    //                            Days = fromSecondrowDate.Subtract(FirstDate).TotalDays;

    //                            ((TextBox)grvOncologySchedule.Rows[I].Cells[1].FindControl("txtDay")).Text = common.myStr(Days + 1);

    //                            PreviousDay = common.myInt((int)common.myInt(((TextBox)grvOncologySchedule.Rows[I - 1].Cells[2].FindControl("txtDay")).Text));
    //                            currentDay = common.myInt((int)common.myInt(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDay")).Text));

    //                            ((TextBox)grvOncologySchedule.Rows[I].Cells[1].FindControl("txtDay")).Text =common.myStr( PreviousDay + currentDay - 1);
    //                            //if (!PreviousDate.Equals(common.myDate("1900/01/01")) && !ckIndex.Equals(I))
    //                            //{
    //                            //    string strDate = common.myStr(fromSecondrowDate.AddDays(Daychabges).ToString("dd/MM/yyyy"));
    //                            //    ((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text = strDate;
    //                            //}
    //                        }
    //                        else
    //                        {
    //                            ((TextBox)grvOncologySchedule.Rows[I].Cells[1].FindControl("txtDay")).Text = "1";
    //                        }
    //                        //ViewState["Cycles"] = common.myInt(((DropDownList)grvOncologySchedule.Rows[I].Cells[0].FindControl("ddlCycle")).SelectedValue);
    //                        ViewState["Cycles"] = common.myInt(((DropDownList)grvOncologySchedule.Rows[I].Cells[0].FindControl("ddlCycle")).SelectedValue);

    //                    }

    //                }
    //            }
    //            hdnRowIndex.Value = "0";
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}


    private void FillDay(DateTime CurrentDate, DateTime PreviousDate)
    {
        try
        {
            string currentdate = hdnCurrentDate.Value;
            int I = 0;
            DateTime FirstDate;
            DateTime FirstDateEdit;
            int PreviousDay = 0;
            int currentDay = 0;
            double Days = 0;
            double DaysEdit = 0;
            int Cycles = 0;
            ((TextBox)grvOncologySchedule.Rows[0].Cells[1].FindControl("txtDay")).Text = "1";
            Cycles = common.myInt(((DropDownList)grvOncologySchedule.Rows[0].Cells[0].FindControl("ddlCycle")).SelectedValue);
            ViewState["Cycles"] = Cycles;
            double Daychabges = 0;
            Daychabges = CurrentDate.Subtract(Convert.ToDateTime(PreviousDate)).TotalDays;
            string FDate = common.myDate(common.myStr(((TextBox)grvOncologySchedule.Rows[0].Cells[2].FindControl("txtDate")).Text)).ToString("yyyy-MM-dd");
            FirstDate = common.myDate(FDate);
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                if (dtCurrentTable.Rows.Count > 0 && common.myInt(hdnRowIndex.Value) > 0)
                {
                    int ckIndex = common.myInt(hdnRowIndex.Value) - 1;
                    for (I = 1; I < dtCurrentTable.Rows.Count; I++)
                    {
                        DateTime CycleDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text).ToString("dd/MM/yyyy"));
                        if (!string.IsNullOrEmpty(common.myStr(CycleDate)))
                        {
                            int CKCycle = common.myInt(((DropDownList)grvOncologySchedule.Rows[I].Cells[0].FindControl("ddlCycle")).SelectedValue);
                            //if (common.myInt(ViewState["Cycles"]) != CKCycle)
                            //{
                            if (common.myInt(ViewState["Cycles"]).Equals(CKCycle))
                            {
                                FirstDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I - 1].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));
                                DateTime fromSecondrowDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));

                                PreviousDay = common.myInt((int)common.myInt(((TextBox)grvOncologySchedule.Rows[I - 1].Cells[2].FindControl("txtDay")).Text));
                                currentDay = common.myInt((int)common.myInt(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDay")).Text));
                                //FirstDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I - 1].Cells[2].FindControl("txtDay")).Text).ToString("yyyy-MM-dd"));
                                //DateTime fromSecondrowDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));

                                //  DateTime ckChangeDate = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[common.myInt(hdnRowIndex.Value)].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));
                                //Days = FirstDate.Subtract(fromSecondrowDate).TotalDays;
                                Days = fromSecondrowDate.Subtract(FirstDate).TotalDays;



                                if (!PreviousDate.Equals(common.myDate("1900/01/01")) && !ckIndex.Equals(I))
                                {
                                    string hdnRowIndex_val = common.myStr(hdnRowIndex.Value);
                                    string hdnCycle_val = common.myStr(hdnCycle.Value);
                                    string grvCycleValue = ((DropDownList)grvOncologySchedule.Rows[I].Cells[0].FindControl("ddlCycle")).SelectedValue;
                                    if (common.myInt(hdnRowIndex_val) <= I && hdnCycle_val.Equals(grvCycleValue))
                                    {

                                        string strDate = common.myStr(fromSecondrowDate.AddDays(Daychabges).ToString("dd/MM/yyyy"));
                                        ((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text = strDate;

                                        /*31-01-2017 */
                                        FirstDateEdit = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I - 1].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));
                                        DateTime fromSecondrowDateEdit = common.myDate((string)common.myDate(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDate")).Text).ToString("yyyy-MM-dd"));

                                        DaysEdit = fromSecondrowDateEdit.Subtract(FirstDateEdit).TotalDays;

                                        ((TextBox)grvOncologySchedule.Rows[I].Cells[1].FindControl("txtDay")).Text = common.myStr(DaysEdit + 1);
                                        PreviousDay = common.myInt((int)common.myInt(((TextBox)grvOncologySchedule.Rows[I - 1].Cells[2].FindControl("txtDay")).Text));
                                        currentDay = common.myInt((int)common.myInt(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDay")).Text));

                                        ((TextBox)grvOncologySchedule.Rows[I].Cells[1].FindControl("txtDay")).Text = common.myStr(PreviousDay + currentDay - 1);

                                        /*31-01-2017 */

                                    }



                                    //((TextBox)grvOncologySchedule.Rows[ckIndex].Cells[2].FindControl("txtDate")).Text = strDate;
                                }
                                else
                                {
                                    ((TextBox)grvOncologySchedule.Rows[I].Cells[1].FindControl("txtDay")).Text = common.myStr(Days + 1);
                                    PreviousDay = common.myInt((int)common.myInt(((TextBox)grvOncologySchedule.Rows[I - 1].Cells[2].FindControl("txtDay")).Text));
                                    currentDay = common.myInt((int)common.myInt(((TextBox)grvOncologySchedule.Rows[I].Cells[2].FindControl("txtDay")).Text));

                                    ((TextBox)grvOncologySchedule.Rows[I].Cells[1].FindControl("txtDay")).Text = common.myStr(PreviousDay + currentDay - 1);
                                }
                            }
                            else
                            {
                                ((TextBox)grvOncologySchedule.Rows[I].Cells[1].FindControl("txtDay")).Text = "1";
                            }
                            //ViewState["Cycles"] = common.myInt(((DropDownList)grvOncologySchedule.Rows[I].Cells[0].FindControl("ddlCycle")).SelectedValue);
                            ViewState["Cycles"] = common.myInt(((DropDownList)grvOncologySchedule.Rows[I].Cells[0].FindControl("ddlCycle")).SelectedValue);

                        }

                    }
                }
                hdnRowIndex.Value = "0";
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void grvOncologySchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("RemoveRow"))
        {
            SetRowData();
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                int rowIndex = Convert.ToInt32(RowIndex);
                if (dt.Rows.Count > 1)
                {
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["CurrentTable"] = dt;
                    grvOncologySchedule.DataSource = dt;
                    grvOncologySchedule.DataBind();

                    SetPreviousOncologyData();
                }
            }
        }
    }

    protected void btnAppointment_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/ICM/OnCologyScheduleRegister.aspx?MRDNO=" + hdnRegistrationId.Value + "&Encounter=" + hdnEncounterId.Value;
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1100;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void btnPrintData_OnClick(object sender, EventArgs e)
    {
        string Schedulerpt = "Schedule";
        RadWindowForNew.NavigateUrl = "/EMRReports/OncologySchedulerpt.aspx?RegId=" + hdnRegistrationId.Value + "&Encounter=" + hdnEncounterId.Value + "&Schedulerpt=" + Schedulerpt;
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 950;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindowForNew.VisibleStatusbar = false;
    }
}