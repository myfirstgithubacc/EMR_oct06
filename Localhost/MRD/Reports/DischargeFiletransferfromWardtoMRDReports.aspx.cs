using BaseC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class MRD_Reports_DischargeFiletransferfromWardtoMRDReports : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsEMRBilling objVal;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds = new DataSet();//dataset objetc
    ManageInsurance miObj = new ManageInsurance();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            chklimittime.Text = "Limit time " + common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "SetMRDFiletransferLimiteTime", sConString)) + " Hr.";
            bindWardName();
            fillInsuranceCompany();
        }
    }

    protected void ddlSearchOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRegNo.Text = "";
        txtSearch.Text = "";
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
           
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        setDate();

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
        RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + txtFromDate.SelectedDate.ToString() + "&Todate=" + txtToDate.SelectedDate + "&Status=" 
            + ddlStatus.SelectedValue + "&FacilityId=" + common.myStr(Session["FacilityId"]) + "&HospitalLocationId="+common.myStr(Session["HospitalLocationID"])
            + "&RegNo="+RegNo + "&EncNo="+ EncNo +"&PatientName="+PatientName+ "&RackNo="+ RackNo + "&chklimittime="+ chklimittime.Checked+ "&Export="+chkExport.Checked+ "&Ward="+ShowCheckedItems(ddlWard) + "&Companyd=" + ShowCheckedItems(ddlInsuranceCompany) +"&ReportName=DischargeFileReports";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1020;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        setDate();
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
}