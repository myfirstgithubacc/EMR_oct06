using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMR_ImmuPatientDashBoard : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                txtSearchN.Visible = true;
                bindSearchData();
                dtpfromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
                dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
                dtpfromDate.SelectedDate = System.DateTime.Now;
                dtpToDate.SelectedDate = System.DateTime.Now;
                tblDateRange.Visible = false;
                ddlrange.SelectedIndex = 0;
            }

        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            txtSearch.Text = "";
            txtSearchN.Text = "";

            txtSearchN.Visible = false;
            txtSearch.Visible = false;

            if (common.myStr(ddlName.SelectedValue) == "R")
            {
                txtSearchN.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        // bindStatus(rblSearchCriteria.SelectedValue);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        bindSearchData();

    }
    private void bindSearchData()
    {

        DataTable dt = new DataTable();
        BaseC.EMRImmunization objImmu = new BaseC.EMRImmunization(sConString);
        string RegistrationNo = string.Empty;
        string Name = string.Empty;
        string MobileNo = string.Empty;
        string MotherName = string.Empty;
        string FromDate = string.Empty;
        string ToDate = string.Empty;
        string Status = string.Empty;
        string DateRange = string.Empty;
        string sFromDate = string.Empty;
        string sToDate = string.Empty;
        string VisitType = string.Empty;
        try
        {
            if (common.myStr(ddlName.SelectedValue).Equals("R"))
            {
                RegistrationNo = common.myStr(txtSearchN.Text);
            }
            else if (common.myStr(ddlName.SelectedValue).Equals("PN"))
            {
                Name = common.myStr(txtSearch.Text);
            }
            else if (common.myStr(ddlName.SelectedValue).Equals("MOB"))
            {
                MobileNo = common.myStr(txtSearch.Text);
            }
            else if (common.myStr(ddlName.SelectedValue).Equals("MN"))
            {
                MotherName = common.myStr(txtSearch.Text);
            }

            Status = common.myStr(ddlStatus.Text);
            DateRange = common.myStr(ddlrange.SelectedValue);
            VisitType = common.myStr(ddlVisitType.SelectedValue);

            //if (common.myStr(ddlrange.SelectedValue).Equals("T"))
            //{
            //    DateRange = "T";
            //}
            //else if (common.myStr(ddlrange.SelectedValue).Equals("Y"))
            //{
            //    DateRange = "Y";
            //}
            //else if (common.myStr(ddlrange.SelectedValue).Equals("LW"))
            //{
            //    DateRange = "LW";
            //}
            //else if (common.myStr(ddlrange.SelectedValue).Equals("LM"))
            //{
            //    DateRange = "LM";
            //}
            //else if (common.myStr(ddlrange.SelectedValue).Equals("LSM"))
            //{
            //    DateRange = "LSM";
            //}
            //else if (common.myStr(ddlrange.SelectedValue).Equals("LOY"))
            //{
            //    DateRange = "LOY";
            //}
            if (common.myStr(ddlrange.SelectedValue).Equals("DR"))
            {
                if (dtpfromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
                {
                    sFromDate = common.myStr(Convert.ToDateTime(dtpfromDate.SelectedDate.Value).ToString("yyyy-MM-dd"));
                    sToDate = common.myStr(dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                }
            }

            dt = objImmu.GetImmunPatientDashBoardDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), 
                                                        RegistrationNo, Name, MobileNo, MotherName, sFromDate, sToDate, Status, DateRange,VisitType);

            gvImmunPatientDashBoardDetails.DataSource = dt;
            gvImmunPatientDashBoardDetails.DataBind();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            dt.Dispose();
            objImmu = null;
        }

    }

    protected void ddlrange_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlrange.SelectedValue == "DR")
            {
                tblDateRange.Visible = true;
                dtpfromDate.Visible = true;
                dtpToDate.Visible = true;
                spTo.Visible = true;
            }
            else
            {
                tblDateRange.Visible = false;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }

    }
    protected void btn_ClearFilter_Click(object sender, EventArgs e)
    {
        try
        {
            ddlName.SelectedIndex = 0;
            txtSearch.Text = string.Empty;
            ddlrange.SelectedIndex = 0;
            ddlrange_SelectedIndexChanged(null, null);
            ddlStatus.SelectedIndex = 0;
            txtSearchN.Text = string.Empty;
            txtSearch.Text = string.Empty;
            bindSearchData();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {

            string RegistrationNo = string.Empty;
            string Name = string.Empty;
            string MobileNo = string.Empty;
            string MotherName = string.Empty;
            string FromDate = string.Empty;
            string ToDate = string.Empty;
            string Status = string.Empty;
            string DateRange = string.Empty;
            string sFromDate = string.Empty;
            string sToDate = string.Empty;
            string VisitType = string.Empty;



            if (common.myStr(ddlName.SelectedValue).Equals("R"))
            {
                //  txtSearchN.Visible = true;
                RegistrationNo = common.myStr(txtSearchN.Text);
            }
            else if (common.myStr(ddlName.SelectedValue).Equals("PN"))
            {
                // txtSearch.Visible = true;
                Name = common.myStr(txtSearch.Text);
            }
            else if (common.myStr(ddlName.SelectedValue).Equals("MOB"))
            {
                // txtSearch.Visible = true;
                MobileNo = common.myStr(txtSearch.Text);
            }
            else if (common.myStr(ddlName.SelectedValue).Equals("MN"))
            {
                // txtSearch.Visible = true;
                MotherName = common.myStr(txtSearch.Text);
            }

            Status = common.myStr(ddlStatus.Text);
            DateRange = common.myStr(ddlrange.SelectedValue);
            VisitType = common.myStr(ddlVisitType.SelectedValue);
            
            if (common.myStr(ddlrange.SelectedValue).Equals("DR"))
            {
                //tdDateRange.Visible = true;
                if (dtpfromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
                {
                    //sFromDate = Convert.ToDateTime(dtpfromDate.SelectedDate).ToString("yyyy-MM-dd") + " 00:00";
                    //sToDate = Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyy-MM-dd") + " 23:59";

                    sFromDate = common.myStr(Convert.ToDateTime(dtpfromDate.SelectedDate.Value).ToString("yyyy-MM-dd"));
                    sToDate = common.myStr(dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                }
            }
            
            RadWindowForNew.NavigateUrl = "/EMRReports/PatientImmunisationReport.aspx?RegistrationNo=" + common.myStr(txtSearchN.Text) + "&Name=" + Name + "&MobileNo=" + MobileNo + "&MotherName=" + MotherName + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&Status=" + Status + "&DateRange=" + DateRange + "&sFromDate=" + sFromDate + "&sToDate="+ sToDate + "&VisitType=" + VisitType;
            RadWindowForNew.Height = 590;
            RadWindowForNew.Width = 980;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;

            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void gvImmunPatientDashBoardDetails_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvImmunPatientDashBoardDetails.CurrentPageIndex = e.NewPageIndex;
        bindSearchData();
    }
}