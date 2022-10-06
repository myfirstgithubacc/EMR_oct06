using BaseC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class WardManagement_DischargeFiletransferfromWardtoMRD : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsEMRBilling objVal;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds = new DataSet();//dataset objetc
    ManageInsurance miObj = new ManageInsurance();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ViewState["OPIP"] = "I";
            ViewState["RegEnc"] = "1";
            ViewState["SearchOn"] = "0";

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            txtFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            txtToDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            hdnRegistrationId.Value = "";
            hdnRegistrationNo.Value = "";
            hdnEncounterId.Value = "";
            hdnEncounterNo.Value = "";
            hdnCompanyCode.Value = "";
            hdnInsuranceCode.Value = "";
            hdnCardId.Value = "";
            hdnEncounterDate.Value = "";

            hdnAgeGender.Value = "";
            hdnPhoneHome.Value = "";
            hdnMobileNo.Value = "";
            hdnPatientName.Value = "";
            hdnDOB.Value = "";
            hdnAddress.Value = "";
            hdnFacilityId.Value = "";
            bindWardName();
            fillInsuranceCompany();
            btnSave.Visible = false;
            btnSaveOtherremark.Visible = false;
            objVal = new BaseC.clsEMRBilling(sConString);


            if (common.myStr(ViewState["SearchOn"]) != "")
            {
                if (common.myInt(ViewState["SearchOn"]) == 0)
                {
                    ddlSearchOn.SelectedValue = "0";
                }
                else if (common.myInt(ViewState["SearchOn"]) == 1)
                {
                    ddlSearchOn.SelectedValue = "1";
                }
            }

            if (common.myStr(ViewState["OPIP"]) == "O")
            {
                ddlSearchOn.SelectedValue = "2";
            }

            if (Convert.ToString(ViewState["SalType"]) == "IP")
            {
                ViewState["OPIP"] = "I";
            }
            else if (Convert.ToString(ViewState["SalType"]) == "OP")
            {
                ViewState["OPIP"] = "O";
            }
            CreateTable();
            if (!common.myStr(Request.QueryString["PageType"]).Equals("MRD"))
            {                
                ddlSearchOn.Items.RemoveAt(3);
                ddlStatus.Items.RemoveAt(1);
                //ddlStatus.Items.RemoveAt(2);
                ddlStatus.SelectedValue = "1";
                ddlStatus_SelectedIndexChanged(null, null);              
                btnReject.Visible = false;
            }
            else
            {
                ddlStatus.Items.RemoveAt(0);
                ddlSearchOn.Items.RemoveAt(1);
                lblHeader.Text = "File Received Ward To MRD";
                ddlStatus.SelectedValue = "2";
                ddlStatus_SelectedIndexChanged(null, null);
                btnSave.Text = "File Acknowledge";
                btnReject.Visible = true;

            }
            bindData("F", 0);

            //chklimittime.Text = "Limit time " + common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "SetMRDFiletransferLimiteTime", sConString)) + " Hr.";

        }
    }

    public void fillInsuranceCompany()
    {
        try
        {
            //getting values      
            ds = miObj.GetCompanyList(Convert.ToInt32(Session["HospitalLocationId"]), "A", 0, common.myInt(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                //populate Company drop down control
                ddlInsuranceCompany.DataSource = ds;
                ddlInsuranceCompany.DataTextField = "name";
                ddlInsuranceCompany.DataValueField = "companyid";
                ddlInsuranceCompany.DataBind();
                foreach (RadComboBoxItem currentItem in ddlInsuranceCompany.Items)
                {
                    currentItem.Checked = true;
                }
            }
        }
        catch (Exception Ex)
        {
            lblHeader.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblHeader.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void bindWardName()
    {
        DataSet ds = new DataSet();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
            //ds = objadt.GetWardStation(common.myInt(Session["FacilityId"]));
            ds = objadt.GetWard(common.myInt(Session["FacilityId"]));
            ddlWard.DataSource = ds.Tables[0];
            //ddlWard.DataTextField = "StationName";
            //ddlWard.DataValueField = "ID";
            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardId";
            ddlWard.DataBind();
            Session["WardIdCount"] = ds.Tables[0].Rows.Count;
            foreach (RadComboBoxItem currentItem in ddlWard.Items)
            {
                currentItem.Checked = true;
            }

        }
        catch (Exception ex)
        {
            //clsExceptionLog objException = new clsExceptionLog();
            //objException.HandleException(ex);
            //objException = null;
        }
        finally
        {
            objadt = null;
            ds.Dispose();
        }
    }

    private string ShowCheckedItems(RadComboBox comboBox)
    {
        string SelectedStatusid = string.Empty;
        var collection = comboBox.CheckedItems;

        if (common.myInt(Session["WardIdCount"]) == comboBox.CheckedItems.Count)
        {
            SelectedStatusid = "A";
        }
        else if (collection.Count > 0)
        {
            foreach (var item in collection)
            {
                if (SelectedStatusid.Equals(string.Empty))
                {
                    SelectedStatusid = common.myStr(item.Value);
                    item.Attributes.ToString();
                }
                else
                {
                    SelectedStatusid = SelectedStatusid + "," + common.myStr(item.Value);
                }
            }
        }
        return SelectedStatusid;
    }

    private string ShowCheckedItemsforComapny(RadComboBox comboBox)
    {
        string SelectedStatusid = string.Empty;
        var collection = comboBox.CheckedItems;


        if (collection.Count > 0)
        {
            foreach (var item in collection)
            {
                if (SelectedStatusid.Equals(string.Empty))
                {
                    SelectedStatusid = common.myStr(item.Value);
                    item.Attributes.ToString();
                }
                else
                {
                    SelectedStatusid = SelectedStatusid + "," + common.myStr(item.Value);
                }
            }
        }
        return SelectedStatusid;
    }
    void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("Name");
        dt.Columns.Add("EncounterNo");
        dt.Columns.Add("EncDate");
        dt.Columns.Add("OPIP");
        dt.Columns.Add("REGID");
        dt.Columns.Add("ENCID");
        dt.Columns.Add("CompanyCode");
        dt.Columns.Add("InsuranceCode");
        dt.Columns.Add("CardId");
        dt.Columns.Add("RowNo");
        dt.Columns.Add("GenderAge");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("PhoneHome");
        dt.Columns.Add("MobileNo");
        dt.Columns.Add("DOB");
        dt.Columns.Add("PatientAddress");
        dt.Columns.Add("CompanyName");
        dt.Columns.Add("CurrentBedNo");
        dt.Columns.Add("CurrentWard");
        dt.Columns.Add("DischargeStatus");
        dt.Columns.Add("EncounterStatus");
        dt.Columns.Add("Status");
        dt.Columns.Add("DischargeDate");
        dt.Columns.Add("FinalizeDate");
        dt.Columns.Add("AdmissionDate");
        dt.Columns.Add("SendtoMrd");
        dt.Columns.Add("SendMRDIssueDate");
        dt.Columns.Add("AcknowledmentStatus");
        dt.Columns.Add("AcknowledmentDate");
        dt.Columns.Add("SendtoMRDRemrk");
        dt.Columns.Add("SendtoMrdNew");
        dt.Columns.Add("AcknowledmentStatusNew");
        dt.Columns.Add("MoreThenTimeLimit");
        dt.Columns.Add("RackNumber");
        dt.Columns.Add("HPIRemarks");
        dt.Columns.Add("OtherRemark");
        dt.Columns.Add("OrderNo");   
        DataRow dr = dt.NewRow();

        dt.Rows.Add(dr);
        gvEncounter.DataSource = dt;
        gvEncounter.DataBind();
    }

    protected void gvEncounter_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {

        bindData("F", 0);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData("F", 0);
    }

    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblMessage.Text = "";
        txtRegNo.Text = string.Empty;
        ddlSearchOn.SelectedIndex = 0;
        bindData("F", 0);
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData("F", 0);
    }
    private void bindData(string RecordButton, int RowNo)
    {
        try
        {
            setDate();

            txtSearch.Visible = true;
            txtRegNo.Visible = false;

            objVal = new BaseC.clsEMRBilling(sConString);

            string BedNo = "";
            string EncNo = "";
            string RegNo = "";
            string PatientName = "";
            string RackNo = "";
            string Mobile = "";

            switch (common.myInt(ddlSearchOn.SelectedValue))
            {
                case 1: // EncNo
                    EncNo = common.myStr(txtSearch.Text);
                    break;
                case 2: // RegNo
                    txtSearch.Visible = false;
                    txtRegNo.Visible = true;
                    RegNo = common.myStr(txtRegNo.Text);
                    break;
                case 4: // PatientName
                    PatientName = common.myStr(txtSearch.Text);
                    break;

                case 7:
                    RackNo = common.myStr(txtSearch.Text);
                    break;
            }


            DataSet dsSearch = objVal.getOPIPRegEncDetailsDischargesForMRDFileIssue(
                                    common.myInt(Session["HospitalLocationID"]),
                                    common.myInt(Session["FacilityId"]),
                                     0, 0, RegNo, EncNo,
                                     common.escapeCharString(PatientName, false),
                                     common.myDate(txtFromDate.SelectedDate),
                                     common.myDate(txtToDate.SelectedDate),
                                     common.myInt(ddlStatus.SelectedValue), false, RackNo, ShowCheckedItems(ddlWard), ShowCheckedItemsforComapny(ddlInsuranceCompany));


            if (dsSearch.Tables[0].Rows.Count > 0)
            {
                gvEncounter.DataSource = dsSearch.Tables[0];
            }
            else
            {
                DataRow DR = dsSearch.Tables[0].NewRow();
                dsSearch.Tables[0].Rows.Add(DR);
                gvEncounter.DataSource = dsSearch.Tables[0];
            }
            //lblTotRecord.Text = "Total Record(s) Found - " + common.myStr(common.myInt(dsSearch.Tables[0].Rows[0]["TotalRecordsCount"]));
            gvEncounter.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void setDate()
    {
        try
        {
            tblDate.Visible = false;

            switch (common.myStr(ddlTime.SelectedValue))
            {
                case "All":
                    txtFromDate.SelectedDate = common.myDate("01-01-1980");
                    txtToDate.SelectedDate = common.myDate("2099-12-31");
                    break;
                case "Today":
                    txtFromDate.SelectedDate = DateTime.Now;
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastWeek":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastTwoWeeks":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-14);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastOneMonth":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastThreeMonths":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastYear":
                    txtFromDate.SelectedDate = DateTime.Now.AddYears(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "DateRange":

                    tblDate.Visible = true;

                    break;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlSearchOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRegNo.Text = "";
        txtSearch.Text = "";
        bindData("F", 0);

    }




    protected void gvEncounter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        bindData("F", 0);
        gvEncounter.PageIndex = e.NewPageIndex;
        gvEncounter.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        StringBuilder strXML = new StringBuilder();
        ArrayList col = new ArrayList();
        Hashtable HshOut = new Hashtable();
        foreach (GridViewRow row in gvEncounter.Rows)
        {
            if (((CheckBox)row.FindControl("chkAck")).Checked)
            {
                TextBox txtRemark = (TextBox)row.FindControl("txtremark");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");
                TextBox txtRackNumber = (TextBox)row.FindControl("txtRackNumber");
                TextBox txtOtherRemarks = (TextBox)row.FindControl("txtOtherRemark");


                col.Add(hdnEncounterId.Value);//EncounterId 
                col.Add(hdnRegistrationId.Value);//RegistrationId 
                col.Add(txtRemark.Text);//Remark 
                col.Add(txtRackNumber.Text);//RackNumber  
                col.Add(txtOtherRemarks.Text); // Other Remarks 
                strXML.Append(common.setXmlTable(ref col));
            }
        }

        if (common.myLen(strXML.ToString()).Equals(0))
        {
            lblMessage.Text = "Please select atleast one Patient !";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            return;
        }
        if (common.myInt(ddlStatus.SelectedValue) == 5)
            ddlStatus.SelectedValue = common.myStr("1");
        string msg = "";
        objVal = new BaseC.clsEMRBilling(sConString);

        HshOut = objVal.SaveDischargeFiletransferfromWardtoMRD(common.myInt(Session["HospitalLocationID"]),
            common.myInt(Session["FacilityId"]), strXML.ToString(), common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserId"]));

        if (HshOut["@chvErrorStatus"].ToString().Equals("Record Saved Successfully") && common.myInt(ddlStatus.SelectedValue) == 1)
        {
            lblMessage.Text = "File issue Successfully";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            bindData("F", 0);

            RadWindow1.NavigateUrl = "~/EMR/Orders/PrintOrder.aspx?rptName=mrdack&OrderId=" + HshOut["@intOrderNo"].ToString();
            RadWindow1.Height = 600;
            RadWindow1.Width = 900;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;

        }
        else if (HshOut["@chvErrorStatus"].ToString().Equals("Record Saved Successfully") && common.myInt(ddlStatus.SelectedValue) == 2)
        {
            lblMessage.Text = "File Acknowledged Successfully";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            bindData("F", 0);
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)gvEncounter.HeaderRow.FindControl("chkAll");
        foreach (GridViewRow row in gvEncounter.Rows)
        {
            ((CheckBox)row.FindControl("chkAck")).Checked = ChkBoxHeader.Checked;
        }
    }

    protected void gvEncounter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAcknowledmentStatus = (Label)e.Row.FindControl("lblAcknowledmentStatus");
            Label lblRackNumber = (Label)e.Row.FindControl("lblRackNumber");
            TextBox txtremark = (TextBox)e.Row.FindControl("txtremark");
            TextBox txtOtherRemark = (TextBox)e.Row.FindControl("txtOtherRemark");
            TextBox txtRackNumber = (TextBox)e.Row.FindControl("txtRackNumber");
            CheckBox chkAck = (CheckBox)e.Row.FindControl("chkAck");
            HiddenField hdnMoreThenTimeLimit = (HiddenField)e.Row.FindControl("hdnMoreThenTimeLimit");
            LinkButton lblPrint=(LinkButton)e.Row.FindControl("lblPrint");
            HiddenField hdnorderno = (HiddenField)e.Row.FindControl("hdnorderno");

            //if (lblAcknowledmentStatus.Text.ToUpper() == "TRUE" || common.myInt(ddlStatus.SelectedValue) == 3)
            //{
            //    chkAck.Visible = false;
            //}

            //if ((common.myStr(Request.QueryString["PageType"]).Equals("MRD")) && common.myInt(ddlStatus.SelectedValue) == 1 || (common.myStr(Request.QueryString["PageType"]) != ("MRD")) && common.myInt(ddlStatus.SelectedValue) == 2)
            //{
            //    chkAck.Visible = false;

            //}

            if (common.myInt(ddlStatus.SelectedValue) != 1)
            {
                //txtremark.Enabled = false;
                //txtOtherRemark.Enabled = false;
                if (hdnorderno.Value.Length > 1)
                {
                    lblPrint.Visible = true;

                }
                else
                {
                    lblPrint.Visible = false;
                }
            }
             

            if (common.myInt(ddlStatus.SelectedValue) == 2 && common.myStr(Request.QueryString["PageType"]).Equals("MRD"))
            {
                txtRackNumber.Visible = true;
                lblRackNumber.Visible = false;
                btnSaveOtherremark.Visible = false;


            }
            else
            {
                txtRackNumber.Visible = false;
                lblRackNumber.Visible = true;
            }

            if (common.myBool(hdnMoreThenTimeLimit.Value))
            {
                e.Row.BackColor = System.Drawing.Color.LightGray;

            }
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (common.myInt(ddlStatus.SelectedValue) == 1 || common.myInt(ddlStatus.SelectedValue) == 5)
        {
            gvEncounter.Columns[6].Visible = false;
            gvEncounter.Columns[7].Visible = false;
            gvEncounter.Columns[8].Visible = false;
            gvEncounter.Columns[9].Visible = false;
            bindData("F", 0);
            if (common.myStr(Request.QueryString["PageType"]).Equals("MRD"))
            {
                btnSave.Visible = false;
                btnSaveOtherremark.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnSaveOtherremark.Visible = true;
            }

        }
        else if (common.myInt(ddlStatus.SelectedValue) == 2)
        {
            gvEncounter.Columns[6].Visible = true;
            gvEncounter.Columns[7].Visible = true;
            gvEncounter.Columns[8].Visible = false;
            gvEncounter.Columns[9].Visible = false;
            bindData("F", 0);
            if (common.myStr(Request.QueryString["PageType"]).Equals("MRD"))
            {
                btnSave.Visible = true;
                btnSaveOtherremark.Visible = false;
            }
            else
            {
                btnSave.Visible = false;
                btnSaveOtherremark.Visible = false;
            }
        }
        else if (common.myInt(ddlStatus.SelectedValue) == 3)
        {
            gvEncounter.Columns[6].Visible = false;
            gvEncounter.Columns[7].Visible = false;
            gvEncounter.Columns[8].Visible = true;
            gvEncounter.Columns[9].Visible = true;
            bindData("F", 0);
            btnSave.Visible = false;
            btnSaveOtherremark.Visible = false;

        }
        else 
        {
            gvEncounter.Columns[6].Visible = true;
            gvEncounter.Columns[7].Visible = true;
            gvEncounter.Columns[8].Visible = true;
            gvEncounter.Columns[9].Visible = true;
            bindData("F", 0);
            btnSave.Visible = false;
            btnSaveOtherremark.Visible = false;
        }

    }

    //protected void chklimittime_CheckedChanged(object sender, EventArgs e)
    //{
    //    bindData("F", 0);
    //}

    protected void btnSaveOtherremark_Click(object sender, EventArgs e)
    {
        StringBuilder strXML = new StringBuilder();
        ArrayList col = new ArrayList();

        foreach (GridViewRow row in gvEncounter.Rows)
        {
            if (((CheckBox)row.FindControl("chkAck")).Checked)
            {
                TextBox txtOtherRemark = (TextBox)row.FindControl("txtOtherRemark");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                col.Add(hdnEncounterId.Value);//EncounterId 
                col.Add(txtOtherRemark.Text);//Other Remark  
                strXML.Append(common.setXmlTable(ref col));
            }
        }

        if (common.myLen(strXML.ToString()).Equals(0))
        {
            lblMessage.Text = "Please select atleast one Patient !";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            return;
        }

        string msg = "";
        objVal = new BaseC.clsEMRBilling(sConString);

        msg = objVal.UpdateDischargeFileRemark(common.myInt(Session["HospitalLocationID"]),
            common.myInt(Session["FacilityId"]), strXML.ToString(), common.myInt(Session["UserId"]));

        if (msg.Equals("Record Saved Successfully"))
        {
            lblMessage.Text = "Remark Updated Successfully";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            bindData("F", 0);

        }
    }

    protected void gvEncounter_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.EMROrders objOrders = new BaseC.EMROrders(sConString);
        try
        {
            if (e.CommandName.Equals("Print"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                HiddenField hdnorderno = (HiddenField)row.FindControl("hdnorderno");

                RadWindow1.NavigateUrl = "~/EMR/Orders/PrintOrder.aspx?rptName=mrdack&OrderId=" + hdnorderno.Value;
                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
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
        finally { objOrders = null; }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        //if (ddlStatus.SelectedIndex != common.myInt(4))
        //{
        //    lblMessage.Text = "Please select status reject ?";
        //    return;
        //}

        StringBuilder strXML = new StringBuilder();
        ArrayList col = new ArrayList();
        Hashtable HshOut = new Hashtable();
        foreach (GridViewRow row in gvEncounter.Rows)
        {
            if (((CheckBox)row.FindControl("chkAck")).Checked)
            {
                TextBox txtRemark = (TextBox)row.FindControl("txtremark");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");
                TextBox txtRackNumber = (TextBox)row.FindControl("txtRackNumber");
                TextBox txtOtherRemarks = (TextBox)row.FindControl("txtOtherRemark");
                col.Add(hdnEncounterId.Value);//EncounterId 
                col.Add(hdnRegistrationId.Value);//RegistrationId 
                col.Add(txtRemark.Text);//Remark 
                col.Add(txtRackNumber.Text);//RackNumber  
                col.Add(txtOtherRemarks.Text); // Other Remarks
                strXML.Append(common.setXmlTable(ref col));
            }
        }

        if (common.myLen(strXML.ToString()).Equals(0))
        {
            lblMessage.Text = "Please select atleast one Patient !";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            return;
        }
        string msg = "";
        objVal = new BaseC.clsEMRBilling(sConString);

        HshOut = objVal.SaveDischargeFiletransferfromWardtoMRD(common.myInt(Session["HospitalLocationID"]),
            common.myInt(Session["FacilityId"]), strXML.ToString(), common.myInt(5), common.myInt(Session["UserId"]));

        if (HshOut["@chvErrorStatus"].ToString().Equals("Reject File Successfully"))
        {
            lblMessage.Text = "Reject File Successfully";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            bindData("F", 0);           

        }
        
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }

    }
}
