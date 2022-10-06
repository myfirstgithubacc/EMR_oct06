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
using Telerik.Web.UI;
using System.Text;
using System.Data.SqlClient;

public partial class Appointment_SearchAppointment : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objException = new clsExceptionLog();

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            if (common.myStr(ddlName.SelectedValue) == "R")
            {
                txtSearch.Visible = false;
                txtSearchN.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
                txtSearchN.Visible = false;
            }

            ViewState["SelectedEnc"] = "";
            tblDateRange.Visible = false;

            clearControl();

            bindControl();

            bindData();
        }
    }

    private void bindControl()
    {
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);

        DataSet ds = new DataSet();
        try
        {
            #region Date Range
            ddlrange.Items.Clear();

            RadComboBoxItem ls = new RadComboBoxItem();
            ls.Text = "Select All";
            ls.Value = "";
            ddlrange.Items.Add(ls);

            RadComboBoxItem lst10 = new RadComboBoxItem();
            lst10.Text = "This Week";
            lst10.Value = "WW0";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW0")
            {
                lst10.Selected = true;
            }
            ddlrange.Items.Add(lst10);

            RadComboBoxItem lst = new RadComboBoxItem();
            lst.Text = "Next Week";
            lst.Value = "WW+1";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW+1")
            {
                lst.Selected = true;
            }
            ddlrange.Items.Add(lst);

            RadComboBoxItem lst1 = new RadComboBoxItem();
            lst1.Text = "Next Two Week";
            lst1.Value = "WW+2";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW+2")
            {
                lst1.Selected = true;
            }
            ddlrange.Items.Add(lst1);

            RadComboBoxItem lst2 = new RadComboBoxItem();
            lst2.Text = "Next Year";
            lst2.Value = "YY+1";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "YY+1")
            {
                lst2.Selected = true;
            }
            ddlrange.Items.Add(lst2);

            RadComboBoxItem lst3 = new RadComboBoxItem();
            lst3.Text = "Today";
            lst3.Value = "DD0";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "DD0")
            {
                lst3.Selected = true;
            }
            else if (ViewState["SelectedDate"] == null)
            {
                lst3.Selected = true;
            }

            ddlrange.Items.Add(lst3);

            RadComboBoxItem lst4 = new RadComboBoxItem();
            lst4.Text = "Last Week";
            lst4.Value = "WW-1";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW-1")
            {
                lst4.Selected = true;
            }
            ddlrange.Items.Add(lst4);

            RadComboBoxItem lst5 = new RadComboBoxItem();
            lst5.Text = "Last Two Week";
            lst5.Value = "WW-2";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW-2")
            {
                lst5.Selected = true;
            }
            ddlrange.Items.Add(lst5);

            RadComboBoxItem lst6 = new RadComboBoxItem();
            lst6.Text = "Last Year";
            lst6.Value = "YY-1";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "YY-1")
            {
                lst6.Selected = true;
            }
            ddlrange.Items.Add(lst6);

            RadComboBoxItem lst7 = new RadComboBoxItem();
            lst7.Text = "Date Range";
            lst7.Value = "4";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "4")
            {
                lst7.Selected = true;
            }
            ddlrange.Items.Add(lst7);

            RadComboBoxItem lst8 = new RadComboBoxItem();
            lst8.Text = "This Month";
            lst8.Value = "MM0";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "MM0")
            {
                lst8.Selected = true;
            }
            ddlrange.Items.Add(lst8);

            RadComboBoxItem lst9 = new RadComboBoxItem();
            lst9.Text = "Last One Month";
            lst9.Value = "MM-1";
            if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "MM-1")
            {
                lst9.Selected = true;
            }
            ddlrange.Items.Add(lst9);

            if (ddlrange.SelectedValue == "4")
            {
                tblDateRange.Visible = true;
            }
            else
            {
                tblDateRange.Visible = false;
            }

            #endregion

            #region Facility
            ds = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);

            ddlLocation.DataSource = ds.Tables[0];
            ddlLocation.DataTextField = "FacilityName";
            ddlLocation.DataValueField = "FacilityID";
            ddlLocation.DataBind();

            ddlLocation.SelectedIndex = 0;
            ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
            #endregion

            #region Status
            ddlAppointmentStatus.Items.Clear();

            RadComboBoxItem lstStatus = new RadComboBoxItem();
            lstStatus.Value = "";
            lstStatus.Text = "Select All";
            lstStatus.Selected = true;

            ddlAppointmentStatus.Items.Add(lstStatus);


            ds = getAppointmentStatus();
            //strSQL = "select StatusId, Status,StatusColor from GetStatus(" + common.myInt(Session["HospitalLocationId"]) + ",'Appointment')";

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                lstStatus = new RadComboBoxItem();

                lstStatus.Attributes.Add("style", "background-color:" + common.myStr(ds.Tables[0].Rows[i]["StatusColor"]) + ";");
                lstStatus.Text = common.myStr(ds.Tables[0].Rows[i]["Status"]);
                lstStatus.Value = common.myStr(ds.Tables[0].Rows[i]["StatusId"]);

                ddlAppointmentStatus.Items.Add(lstStatus);
            }

            #endregion

            BindSpeciliazation();

            BindProvider();

            //createTableContextMenu();
            BindGrpTagContextMenu();
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
            objMaster = null;
        }
    }

    private DataSet getAppointmentStatus()
    {
        DataSet ds = new DataSet();
        BaseC.clsLISPhlebotomy objLP = new BaseC.clsLISPhlebotomy(sConString);
        try
        {
            ds = objLP.getStatus(common.myInt(Session["HospitalLocationId"]), "Appointment", string.Empty);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objLP = null;
        }
        return ds;
    }

    private void clearControl()
    {
        try
        {
            //txtNo.Text = "";
            //ddlSupplier.SelectedIndex = 0;
            //txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            //txtFromDate.SelectedDate = DateTime.Now.AddDays(-15);

            //txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            //txtToDate.SelectedDate = DateTime.Now;

            //txtNetAmount.Text = "";
            //rdoStatus.SelectedIndex = 0;

            lblMessage.Text = "&nbsp;";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvAppointmentList_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager)
        {
            int iTotalRecords = common.myInt(ViewState["Count"]);
            int iPageSize = common.myInt(gvAppointmentList.PageSize);
            int iRowCount = common.myInt(gvAppointmentList.Rows.Count);
            if (iRowCount == iPageSize)
            {
                lblGridStatus.Text = "Showing " + ((gvAppointmentList.PageIndex *
                gvAppointmentList.PageSize) + 1) + " - " + ((gvAppointmentList.PageIndex + 1) *
                gvAppointmentList.Rows.Count) + " of " + iTotalRecords + " Record(s)";
            }
            else
            {
                lblGridStatus.Text = "Showing " + ((gvAppointmentList.PageIndex * iPageSize) + 1)
                    + " - " + iTotalRecords + " of " + iTotalRecords + " Record(s)";
            }
        }
    }

    protected void gvAppointmentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAppointmentList.PageIndex = e.NewPageIndex;

        bindData();
    }

    protected void gvAppointmentList_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet dsPatientDetail = new DataSet();
        try
        {
            //LinkButton lbt = (LinkButton)e.CommandSource;
            //GridViewRow grvRow = (GridViewRow)((DataControlFieldCell)lbt.Parent).Parent;

            //int iRegistrationId = common.myInt(((HiddenField)grvRow.FindControl("hdnRegistrationId")).Value);
            //int iAppointmentId = common.myInt(((HiddenField)grvRow.FindControl("hdnAppointmentID")).Value);
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void gvAppointmentList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Byte A;
        Byte R;
        Byte G;
        Byte B;
        String htmlHexColorValue = "";
        int sTotal = common.myInt(ViewState["Count"]);
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["PrevRow"] = null;
            ViewState["CurrRow"] = null;
            if (ViewState["Blank"] == null)
            {
                String AppStatusColor = common.myStr(((HiddenField)e.Row.FindControl("hdnStatusColor")).Value);
                string hdnPackageId = common.myStr(((HiddenField)e.Row.FindControl("hdnPackageId")).Value);
                string hdnPackageName = common.myStr(((HiddenField)e.Row.FindControl("hdnPackageName")).Value);
                string hdnVisitType = common.myStr(((HiddenField)e.Row.FindControl("hdnVisitType")).Value);
                Label lbl_StatusColor = new Label();
                HiddenField hdnMedicalAlert = (HiddenField)e.Row.FindControl("hdnMedicalAlert");
                HiddenField hdnAllergiesAlert = (HiddenField)e.Row.FindControl("hdnAllergiesAlert");

                ImageButton btnCategory = (ImageButton)e.Row.FindControl("btnCategory");
                Telerik.Web.UI.RadContextMenu menuStatus = (Telerik.Web.UI.RadContextMenu)e.Row.FindControl("menuStatus");

                //btnCategory.Attributes.Add("OnClick", "showMenu(event,'" + menuStatus.ClientID + "','" + hdnEncounterId.ClientID + "','" + hdnAdmissionStatus.ClientID + "')");
                btnCategory.Attributes.Add("OnClick", "showMenu(event,'" + menuStatus.ClientID + "')");

                menuStatus.DataSource = (DataTable)ViewState["dtable1"];
                //menuStatus.DataTextField = "Text";
                //menuStatus.DataValueField = "Value";
                menuStatus.DataTextField = "OptionName";
                menuStatus.DataValueField = "OptionCode";
                menuStatus.DataBind();

                if (hdnPackageName != "")
                {
                    e.Row.ToolTip = "Visit Details : "
                                        + System.Environment.NewLine + "Visit Type : " + hdnVisitType
                                        + System.Environment.NewLine + "Package Name : " + hdnPackageName;
                }
                else
                    e.Row.ToolTip = "Visit Details : "
                                       + System.Environment.NewLine + "Visit Type : " + hdnVisitType;

                if (common.myStr(AppStatusColor).Trim() != "Blank" && common.myStr(AppStatusColor).Trim() != "&nbsp;")
                {
                    lbl_StatusColor.BackColor = System.Drawing.ColorTranslator.FromHtml(AppStatusColor.Trim());

                    A = lbl_StatusColor.BackColor.A;
                    B = lbl_StatusColor.BackColor.B;
                    R = lbl_StatusColor.BackColor.R;
                    G = lbl_StatusColor.BackColor.G;
                    htmlHexColorValue = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(A, R, G, B));
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml(common.myStr(htmlHexColorValue));
                    }
                }
                else
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ViewState["PrevRow"] = null;
                        ViewState["CurrRow"] = null;
                    }
                }
            }
        }
    }

    protected void bindData()
    {
        BaseC.Dashboard objD = new BaseC.Dashboard();
        DataSet ds = new DataSet();
        try
        {
            if (Session["HospitalLocationID"] != null)
            {
                ViewState["Count"] = 0;
                ViewState["SelectedEncounterId"] = "";
                ViewState["SelectedEnc"] = "";

                string PatientName = string.Empty;
                string ProviderIds = string.Empty;
                Int64 RegistrationNo = 0;
                string FromDate = string.Empty;
                string ToDate = string.Empty;
                string OldRegistrationNo = string.Empty;
                string EnrolleNo = string.Empty;
                string MobileNo = string.Empty;

                ProviderIds = (common.myInt(ddlProvider.SelectedValue) > 0) ? common.myInt(ddlProvider.SelectedValue).ToString() : "A";

                if (ddlrange.SelectedValue == "4" || ddlrange.SelectedValue == "WW0" || ddlrange.SelectedValue == "MM-1" || ddlrange.SelectedValue == "MM0" ||
                     ddlrange.SelectedValue == "YD" || ddlrange.SelectedValue == "WW-1" || ddlrange.SelectedValue == "MM-6" || ddlrange.SelectedValue == "YY-1")
                {
                    string[] str = getToFromDate(ddlrange.SelectedValue);
                    string sFromDate = str[0];
                    string sToDate = str[1];

                    FromDate = sFromDate;
                    ToDate = sToDate;
                }

                switch (common.myStr(ddlName.SelectedValue))
                {
                    case "R":
                        RegistrationNo = common.myLong(txtSearchN.Text);
                        break;
                    case "N":
                        PatientName = common.myStr(txtSearch.Text).Trim();
                        break;
                    case "O":
                        OldRegistrationNo = common.myStr(txtSearch.Text).Trim();
                        break;
                    case "EN":
                        EnrolleNo = common.myStr(txtSearch.Text).Trim();
                        break;
                    case "MN":
                        MobileNo = common.myStr(txtSearch.Text).Trim();
                        break;
                }

                ds = objD.getPDAppointment(common.myInt(Session["HospitalLocationID"]), PatientName, ProviderIds,
                                            common.myInt(ddlAppointmentStatus.SelectedValue), common.myInt(ddlLocation.SelectedValue),
                                            RegistrationNo, OldRegistrationNo, common.myStr(ddlrange.SelectedValue), FromDate, ToDate,
                                            common.myInt(Session["FacilityID"]), EnrolleNo, MobileNo);

                ViewState["Count"] = ds.Tables[0].Rows.Count;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    DataRow DR = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(DR);
                }

                DataView DV = ds.Tables[0].DefaultView;
                DV.Sort = "AppointmentId DESC";

                gvAppointmentList.DataSource = DV.ToTable();
                gvAppointmentList.DataBind();
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
            objD = null;
            ds.Dispose();
        }
    }

    private string[] getToFromDate(string ddlTime)
    {
        int timezone = BindUTCTime();
        string sFromDate = "", sToDate = "";
        string[] str = new string[2];

        if (ddlTime == "4")
        {
            //tdDateRange.Visible = true;
            if (dtpfromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                //sFromDate = Convert.ToDateTime(dtpfromDate.SelectedDate).ToString("yyyy-MM-dd") + " 00:00";
                //sToDate = Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyy-MM-dd") + " 23:59";

                sFromDate = common.myStr(Convert.ToDateTime(dtpfromDate.SelectedDate.Value).ToString("yyyy-MM-dd"));
                sToDate = common.myStr(dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
            }
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "YD")
        {
            sFromDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + " 00:00";
            sToDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + " 23:59";
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "WW-1")
        {
            sFromDate = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd") + " 00:00";
            sToDate = DateTime.Now.AddDays(0).ToString("yyyy/MM/dd") + " 23:59";
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "WW0")
        {
            str = datecalculate();

        }
        else if (ddlTime == "MM0")
        {
            sFromDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01" + " 00:00";
            sToDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + " 23:59";

            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "MM-6")
        {
            sFromDate = DateTime.Now.AddMonths(-6).ToString("yyyy/MM/dd") + " 00:00";
            sToDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month).ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, (DateTime.Now.Month)) + " 23:59";

            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "YY-1")
        {
            sFromDate = DateTime.Now.AddDays(-365).ToString("yyyy/MM/dd") + " 00:00";
            sToDate = DateTime.Now.Date.ToString("yyyy/MM/dd") + " 23:59";

            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "MM-1")
        {
            if ((DateTime.Now.Month - 1) != 0)
            {
                sFromDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 1).ToString() + "/01" + " 00:00";
                sToDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 1).ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, (DateTime.Now.Month - 1)) + " 23:59";
            }
            else
            {
                sFromDate = (DateTime.Now.Year - 1).ToString() + "/" + 12.ToString() + "/01" + " 00:00";
                sToDate = (DateTime.Now.Year - 1).ToString() + "/" + 12.ToString() + "/" + DateTime.DaysInMonth((DateTime.Now.Year - 1), 12) + " 23:59";

            }
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        return str;
    }

    private string[] datecalculate()
    {
        string DayName = DateTime.Now.DayOfWeek.ToString();
        string fromdate = "";
        string todate = "";

        string[] str = new string[2];

        switch (DayName)
        {
            case "Monday":
                fromdate = DateTime.Now.Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(6).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Tuesday":
                fromdate = DateTime.Now.AddDays(-1).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(5).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Wednesday":
                fromdate = DateTime.Now.AddDays(-2).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(4).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Thursday":
                fromdate = DateTime.Now.AddDays(-3).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(3).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Friday":
                fromdate = DateTime.Now.AddDays(-4).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(2).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Saturday":
                fromdate = DateTime.Now.AddDays(-5).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Sunday":
                fromdate = DateTime.Now.AddDays(-6).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59";
                break;
        }

        str[0] = fromdate;
        str[1] = todate;
        return str;
    }

    protected int BindUTCTime()
    {
        int timezone = 0;
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@intfacilityid", common.myInt(Session["FacilityID"]));
            string strtimezone = "Select TimeZoneOffSetMinutes  from FacilityMaster where FacilityID=@intfacilityid";
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.Text, strtimezone, hsinput);
            timezone = common.myInt(ds.Tables[0].Rows[0]["TimeZoneOffSetMinutes"]);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return timezone;
    }

    protected void ddlrange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlrange.SelectedValue == "4")
        {
            tblDateRange.Visible = true;
        }
        else
        {
            tblDateRange.Visible = false;
        }
        ViewState["SelectedDate"] = ddlrange.SelectedValue;
    }

    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        txtSearchN.Text = "";

        if (common.myStr(ddlName.SelectedValue) == "R")
        {
            txtSearch.Visible = false;
            txtSearchN.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
            txtSearchN.Visible = false;
        }
    }

    protected void ddlSpecilization_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindProvider();

        if (common.myInt(ddlSpecilization.SelectedValue) > 0)
        {
            RadComboBoxItem Provider = ddlProvider.Items.FindItemByValue("0");
            if (Provider != null)
            {
                ddlProvider.Items.Remove(0);
            }
        }
    }

    private void BindProvider()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet objDs = new DataSet();
        Hashtable hsIn = new Hashtable();

        try
        {
            hsIn.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsIn.Add("@iUserId", common.myInt(Session["UserId"]));
            hsIn.Add("@intSpecializationId", common.myInt(ddlSpecilization.SelectedValue));
            hsIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeWithResource", hsIn);

            //Cache.Insert("Doctor", objDs, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
            ddlProvider.DataSource = objDs;
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorID";
            ddlProvider.DataBind();

            CheckUserDoctorOrNot();

            //if (objDs.Tables[0].Rows.Count > 1)
            //{
            //    ddlProvider.Enabled = true;
            //}
            //else
            //{
            //    ddlProvider.Enabled = false;
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
            objDl = null;
            objDs.Dispose();
        }
    }

    private void BindSpeciliazation()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsh = new Hashtable();
            hsh.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsh.Add("@intFacilityId", common.myInt(Session["facilityId"]));

            DataSet dsSpeciliazation = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorTimeSpecialisation", hsh);
            if (dsSpeciliazation.Tables[0].Rows.Count > 0)
            {
                ddlSpecilization.Items.Clear();
                ddlSpecilization.DataSource = dsSpeciliazation.Tables[0];
                ddlSpecilization.DataTextField = "NAME";
                ddlSpecilization.DataValueField = "id";
                ddlSpecilization.DataBind();
                ddlSpecilization.Items.Insert(0, new RadComboBoxItem("All", "0"));
                if (ViewState["UserSpecialisationId"] != null)
                {
                    ddlSpecilization.SelectedValue = common.myStr(ViewState["UserSpecialisationId"]);
                }
            }
            else
            {
                ddlSpecilization.Text = "";
                ddlSpecilization.Items.Clear();
                Alert.ShowAjaxMsg("Specialization not available", Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void CheckUserDoctorOrNot()
    {
        try
        {
            BaseC.EMR objEmr = new BaseC.EMR(sConString);
            if (Session["UserID"] != null)
            {
                if (common.myInt(Session["LoginIsAdminGroup"]) == 1)
                {
                    ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                    ddlProvider.Items[0].Selected = true;
                    ddlProvider.Enabled = true;
                    ViewState["IsDoctor"] = "0";
                }
                else
                {
                    SqlDataReader objDr = (SqlDataReader)objEmr.CheckUserDoctorOrNot(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));
                    if (objDr.Read())
                    {
                        if ((common.myStr(objDr[0]) != "") && (objDr[0] != null) && (common.myInt(Session["LoginIsAdminGroup"]) == 0))
                        {
                            ddlProvider.Items[0].Selected = false;
                            ddlProvider.SelectedIndex = ddlProvider.Items.IndexOf(ddlProvider.FindItemByValue(common.myStr(objDr[0])));
                            //if (ViewState["OPIP"] != null && ViewState["OPIP"] == "E")
                            //{
                            ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                            //ddlProvider.Items[0].Selected = true;
                            //}
                            ViewState["IsDoctor"] = "1";
                        }
                        else
                        {
                            ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                            ddlProvider.Items[0].Selected = true;
                            ViewState["IsDoctor"] = "0";
                        }
                    }
                    objDr.Close();
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

    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        bindData();
    }

    protected void btnResetFilter_OnClick(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        txtSearchN.Text = string.Empty;
        
        ddlAppointmentStatus.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;

        if (common.myInt(ViewState["IsDoctor"]) == 0)
        {
            ddlProvider.Enabled = true;
        }

        ddlrange.SelectedIndex = ddlrange.Items.IndexOf(ddlrange.Items.FindItemByValue("DD0"));
        ddlrange_SelectedIndexChanged(this, null);
    }

    protected void createTableContextMenu()
    {
        UserAuthorisations ua = new UserAuthorisations();
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Text", typeof(string)));
            dt.Columns.Add(new DataColumn("Value", typeof(string)));

            DataRow dr;
            if (ua.CheckPermissions("P", "/Appointment/AppointmentDetails.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "Print Appointment";
                dr["Value"] = "Print";
                dt.Rows.Add(dr);
            }
            //dr = dt.NewRow();
            //dr["Text"] = "Copy";
            //dr["Value"] = "Copy";
            //dt.Rows.Add(dr);
            if (ua.CheckPermissions("E", "/Appointment/AppointmentDetails.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "Edit";
                dr["Value"] = "Edit";
                dt.Rows.Add(dr);
            }
            if (ua.CheckPermissions("C", "/Appointment/AppointmentDetails.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "Cancel Chk-In";
                dr["Value"] = "DelChkIn";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Text"] = "Cancel Appointment";
                dr["Value"] = "Delete";
                dt.Rows.Add(dr);
            }

            dr = dt.NewRow();
            dr["Text"] = "Patient Details";
            dr["Value"] = "PatientDetails";
            dt.Rows.Add(dr);

            DataSet Statusdt = new DataSet();
            Statusdt = getAppointmentStatus();
            if (Statusdt.Tables[0].Rows.Count > 0)
            {
                for (int counter = 0; counter <= Statusdt.Tables[0].Rows.Count - 1; counter++)
                {
                    if (Statusdt.Tables[0].Rows[counter]["Status"].ToString().Equals("Cancel"))
                    {
                        if (ua.CheckPermissions("C", "/Appointment/AppointmentDetails.aspx", true))
                        {
                            dr = dt.NewRow();
                            dr["Text"] = Statusdt.Tables[0].Rows[counter]["Status"].ToString();
                            dr["Value"] = Statusdt.Tables[0].Rows[counter]["Code"].ToString();
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        dr = dt.NewRow();
                        dr["Text"] = Statusdt.Tables[0].Rows[counter]["Status"].ToString();
                        dr["Value"] = Statusdt.Tables[0].Rows[counter]["Code"].ToString();
                        dt.Rows.Add(dr);
                    }
                }
            }
            ViewState["dtable1"] = dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ua.Dispose();
        }
    }

    protected void btnDeleteApp_Click(object sender, EventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.Appointment appoint = new BaseC.Appointment(sConString);

        try
        {


            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            if (ddlRemarkss.SelectedValue == "0")
            {
                Alert.ShowAjaxMsg("Please select the cancellation reason", Page);
                return;
            }
            string Remarks = ddlRemarkss.SelectedItem.Text + ". " + txtCancel;
            if (common.myStr(ViewState["MenuStatus"]) == "CheckedIn")
            {
                hshIn.Add("@iHospitalLocationId", Session["HospitalLocationID"]);
                hshIn.Add("@iFacilityId", common.myInt(ddlLocation.SelectedValue));
                hshIn.Add("@iEncounterId", common.myInt(ViewState["CancelEncounter"]));
                hshIn.Add("@iAppointmentId", common.myInt(ViewState["CancelAppointmentId"]));
                hshIn.Add("@iRegistrationId", common.myInt(ViewState["CancelRegistrationId"]));
                hshIn.Add("@sRemarks", Remarks);
                dl.ExecuteNonQuery(CommandType.StoredProcedure, "uspCancelAppointmentCheckin", hshIn);
            }
            else
            {

                string[] strAppId = ViewState["AppId"].ToString().Split('_');
                //Hashtable hshIn = new Hashtable();
                hshIn.Add("@intAppointmentId", strAppId[0]);
                hshIn.Add("@intLastChangedBy", Session["UserId"]);
                hshIn.Add("@chvCancelRemark", Remarks);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteAppointment", hshIn, hshOut);
                Alert.ShowAjaxMsg(hshOut["@chvErrorStatus"].ToString(), Page);
            }
            ////SchedulerNavigationCommand.SwitchToSelectedDay
            //DateTime OrigRecTime = DateTime.Now;
            //string strOrigRecTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
            //OrigRecTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigRecTime));

            //DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
            //string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigRecTime.ToString("HHmmss") + "Z";

            //string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
            //string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
            //if (!OrigRecRule.Contains("EXDATE"))
            //{
            //    string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
            //    string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
            //    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
            //}
            //else
            //{
            //    string UpdateRule = OrigRecRule + "," + exdate;
            //    string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
            //    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
            //}
            dvDelete.Visible = false;
            btnRefresh_OnClick(sender, e);


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
            appoint = null;
        }
    }

    protected void btnCancelApp_Click(object sender, EventArgs e)
    {
        dvDelete.Visible = false;
    }

    protected void btnDeleteAllApp_Click(object sender, EventArgs e)
    {
        BaseC.Appointment appoint = new BaseC.Appointment(sConString);
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            //All Recurring Cancel

            string[] strAppId = ViewState["AppId"].ToString().Split('_');
            DateTime origAppTime = DateTime.Now;
            DateTime origAppDate = DateTime.Today;

            if (ddlRemarks.SelectedValue == "0")
            {
                ddlRemarks.Focus();
                Alert.ShowAjaxMsg("Please select the cancellation reason", Page);
                return;
            }
            string Remarks = ddlRemarks.SelectedItem.Text + ". " + txtRecurRemark.Text;
            if (ViewState["StatusId"].ToString() == "3")
            {
                DataSet ds = appoint.getAppointmentEncounter(Convert.ToInt32(strAppId[0]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int EncounterId = Convert.ToInt32(ds.Tables[0].Rows[0]["EncounterId"]);
                    int RegistrationId = Convert.ToInt32(ds.Tables[0].Rows[0]["RegistrationId"]);
                    DataTable dt = appoint.GetEncounterStatusToDelete(EncounterId, RegistrationId);
                    if (dt.Rows.Count > 0)
                    {
                        lblRecurringDeleteMessage.Text = "Appointment Can't Delete. EMR details found.";
                        return;
                    }
                }
            }
            if (ViewState["AppId"].ToString().Contains('_'))
            {
                DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
                //string recrule1 = e.Appointment.Attributes["RecurrenceRule"];                
                string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                origAppTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                string strOrigDate = "select AppointmentDate from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                origAppDate = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                TimeSpan daysCount = dtStart.Subtract(origAppDate);

                string exdate = "UNTIL=" + dtStart.ToString("yyyyMMdd") + "T000000Z;";

                string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (!OrigRecRule.Contains("COUNT"))
                {
                    if (!OrigRecRule.Contains("UNTIL"))
                    {
                        //int newrule = OrigRecRule.IndexOf("INTERVAL");
                        string newRecRule = OrigRecRule.Insert(OrigRecRule.IndexOf("INTERVAL"), exdate);
                        string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + newRecRule + "', CancelReason='" + Remarks + "' where appointmentid='" + strAppId[0] + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }
                    else
                    {
                        //int newrule = OrigRecRule.IndexOf("UNTIL");
                        string newrule = OrigRecRule.Insert(OrigRecRule.IndexOf("UNTIL"), exdate);
                        string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + newrule + "', CancelReason='" + Remarks + "' where appointmentid='" + strAppId[0] + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }
                }
                else
                {
                    if (OrigRecRule.Contains("COUNT"))
                    {
                        int noOfChars = OrigRecRule.IndexOf("INTERVAL") - OrigRecRule.IndexOf("COUNT");
                        string repUntil = OrigRecRule.Remove(OrigRecRule.IndexOf("COUNT"), noOfChars);
                        string newRecRule = repUntil.Insert(repUntil.IndexOf("INTERVAL"), exdate);
                        string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + newRecRule + "' where appointmentid='" + strAppId[0] + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }

                }
            }
            else
            {
                Hashtable hshIn = new Hashtable();
                Hashtable hshOut = new Hashtable();
                hshIn.Add("@intAppointmentId", strAppId[0]);
                hshIn.Add("@intLastChangedBy", Session["UserId"]);
                hshIn.Add("@chvCancelRemark", Remarks);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteAppointment", hshIn, hshOut);
                Alert.ShowAjaxMsg(hshOut["@chvErrorStatus"].ToString(), Page);
            }
            dvDelete.Visible = false;

            btnRefresh_OnClick(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dl = null;
            appoint = null;
        }
    }

    protected void btnDeleteThisApp_Click(object sender, EventArgs e)
    {
        try
        {
            //This Recurring Cancel
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string[] strAppId = ViewState["AppId"].ToString().Split('_');
            BaseC.Appointment appoint = new BaseC.Appointment(sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            if (ddlRemarks.SelectedItem.Text == "")
            {
                Alert.ShowAjaxMsg("Please type cancel reason", Page);
                return;
            }
            string Remarks = ddlRemarks.SelectedItem.Text + ". " + txtRecurRemark.Text;
            if (ViewState["AppId"].ToString().Contains('_'))
            {

                //SchedulerNavigationCommand.SwitchToSelectedDay


                DateTime OrigRecTime = DateTime.Now;
                string strOrigRecTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                OrigRecTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigRecTime));

                DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
                string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigRecTime.ToString("HHmmss") + "Z";

                string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (!OrigRecRule.Contains("EXDATE"))
                {
                    string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                    string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "', CancelReason='" + Remarks + "'  where appointmentid='" + strAppId[0] + "'";
                    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                }
                else
                {
                    string UpdateRule = OrigRecRule + "," + exdate;
                    string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "', CancelReason='" + Remarks + "' where appointmentid='" + strAppId[0] + "'";
                    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                }
            }
            else
            {
                //Hashtable hshIn = new Hashtable();
                hshIn.Add("@intAppointmentId", strAppId[0]);
                hshIn.Add("@intLastChangedBy", Session["UserId"]);
                hshIn.Add("@chvCancelRemark", Remarks);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteAppointment", hshIn, hshOut);
                Alert.ShowAjaxMsg(hshOut["@chvErrorStatus"].ToString(), Page);

            }

            dvDelete.Visible = false;

            btnRefresh_OnClick(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCancelRecurrApp_OnClick(object sender, EventArgs e)
    {
        dvDeleteRecurring.Visible = false;
    }

    protected void fillRemarks()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "AppCancel", "AppCancel");
        ddlRemarks.Items.Clear();
        ddlRemarkss.Items.Clear();

        ddlRemarks.DataSource = ds;
        ddlRemarks.DataTextField = "Status";
        ddlRemarks.DataValueField = "StatusId";
        ddlRemarks.DataBind();
        ddlRemarks.Items.Insert(0, "Select a Reason");
        ddlRemarks.Items[0].Value = "0";

        ddlRemarkss.DataSource = ds;
        ddlRemarkss.DataTextField = "Status";
        ddlRemarkss.DataValueField = "StatusId";
        ddlRemarkss.DataBind();
        ddlRemarkss.Items.Insert(0, "Select a Reason");
        ddlRemarkss.Items[0].Value = "0";
    }

    private string FindCurrentDate(string outputCurrentDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string firstCurrentDate = "";
        string newCurrentDate = "";
        string currentdate = formatdate.FormatDateDateMonthYear(System.DateTime.Today.ToShortDateString());
        string strformatCurrDate = formatdate.FormatDate(currentdate, "dd/MM/yyyy", "yyyy/MM/dd");
        firstCurrentDate = strformatCurrDate.Remove(4, 1);
        newCurrentDate = firstCurrentDate.Remove(6, 1);
        return newCurrentDate;
    }

    private string FindFutureDate(string outputFutureDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string strDate = Convert.ToDateTime(outputFutureDate).Date.ToShortDateString();
        string firstApptDate = "";
        string NewApptDate = "";
        string strDateAppointment = formatdate.FormatDateDateMonthYear(strDate);
        string strformatApptDate = formatdate.FormatDate(strDateAppointment, "dd/MM/yyyy", "yyyy/MM/dd");
        firstApptDate = strformatApptDate.Remove(4, 1);
        NewApptDate = firstApptDate.Remove(6, 1);
        return NewApptDate;
    }

    protected void menuStatus_ItemClick(object sender, RadMenuEventArgs e)
    {
        Telerik.Web.UI.RadContextMenu menuStatus = (RadContextMenu)e.Item.NamingContainer;

        //uspGetAppointments
        //Code,StatusId, ColorName, StatusColor, RegistrationId,RegistrationNo, DoesPatientOweMoney,CompanyCode, ValidPaymentCaseId, 
        //TooTipText, DoctorId, Type, EligibilityChecked, VisitType,InvoiceId,RecurrenceRule,RealDateOfBirth, NoPAForInsurancePatient, 
        //NoBillForPrivatePatient, AuthorizationNo,PackageId,PackageType

        GridViewRow gvr = (GridViewRow)menuStatus.NamingContainer;
        HiddenField hdnAppointmentID = (HiddenField)gvr.FindControl("hdnAppointmentID");
        HiddenField hdnRegistrationId = (HiddenField)gvr.FindControl("hdnRegistrationId");
        HiddenField hdnStatusCode = (HiddenField)gvr.FindControl("hdnStatusCode");
        HiddenField hdnFromTime = (HiddenField)gvr.FindControl("hdnFromTime");
        HiddenField hdnToTime = (HiddenField)gvr.FindControl("hdnToTime");
        HiddenField hdnPatientDOB = (HiddenField)gvr.FindControl("hdnPatientDOB");
        HiddenField hdnFacilityID = (HiddenField)gvr.FindControl("hdnFacilityID");
        HiddenField hdnRecurrenceParentID = (HiddenField)gvr.FindControl("hdnRecurrenceParentID");
        HiddenField hdnDoctorID = (HiddenField)gvr.FindControl("hdnDoctorID");
        HiddenField hdnPackageId = (HiddenField)gvr.FindControl("hdnPackageId");
        HiddenField hdnPackageType = (HiddenField)gvr.FindControl("hdnPackageType");
        HiddenField hdnStatusId = (HiddenField)gvr.FindControl("hdnStatusId");
        HiddenField hdnRecurrenceRule = (HiddenField)gvr.FindControl("hdnRecurrenceRule");
        HiddenField hdnNoPAForInsurancePatient = (HiddenField)gvr.FindControl("hdnNoPAForInsurancePatient");
        HiddenField hdnNoBillForPrivatePatient = (HiddenField)gvr.FindControl("hdnNoBillForPrivatePatient");
        HiddenField hdnIsDoctor = (HiddenField)gvr.FindControl("hdnIsDoctor");
        HiddenField hdnShiftFirstFrom = (HiddenField)gvr.FindControl("hdnShiftFirstFrom");
        HiddenField hdnShiftFirstTo = (HiddenField)gvr.FindControl("hdnShiftFirstTo");
        HiddenField hdnTimeTaken = (HiddenField)gvr.FindControl("hdnTimeTaken");

        Label lblAppointmentDate = (Label)gvr.FindControl("lblAppointmentDate");
        Label lblregistrationno = (Label)gvr.FindControl("lblregistrationno");

        int RegistrationId = common.myInt(hdnRegistrationId.Value);
        Int64 RegistrationNo = common.myLong(lblregistrationno.Text);
        int AppointmentId = common.myInt(hdnAppointmentID.Value);
        int FacilityId = common.myInt(hdnFacilityID.Value);
        int RecurrenceParentId = common.myInt(hdnRecurrenceParentID.Value);
        int DoctorId = common.myInt(hdnDoctorID.Value);
        int TimeTaken = common.myInt(hdnTimeTaken.Value);
        int PackageId = common.myInt(hdnPackageId.Value);
        int StatusId = common.myInt(hdnStatusId.Value);
        int ShiftFirstFromHour = 8;
        int ShiftFirstToHour = 8;

        string currentMenuValue = common.myStr(menuStatus.SelectedValue);
        string statuscode = common.myStr(hdnStatusCode.Value);
        string strFromTime = "1900/01/01 " + common.myStr(hdnFromTime.Value).Trim();
        string strToTime = "1900/01/01 " + common.myStr(hdnToTime.Value).Trim();
        string AppointmentDate = common.myStr(lblAppointmentDate.Text);
        string PatientDOB = common.myStr(hdnPatientDOB.Value);
        string PackageType = common.myStr(hdnPackageType.Value);
        string RecurrenceRule = common.myStr(hdnRecurrenceRule.Value);
        string ShiftFirstFrom = common.myStr(hdnShiftFirstFrom.Value);
        string ShiftFirstTo = common.myStr(hdnShiftFirstTo.Value);

        if (common.myLen(ShiftFirstFrom) > 0)
        {
            ShiftFirstFromHour = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + ShiftFirstFrom.Trim()).Hour;
        }
        if (common.myLen(ShiftFirstTo) > 0)
        {
            ShiftFirstToHour = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + ShiftFirstTo.Trim()).Hour;
        }

        bool NoPAForInsurancePatient = common.myBool(hdnNoPAForInsurancePatient.Value);
        bool NoBillForPrivatePatient = common.myBool(hdnNoBillForPrivatePatient.Value);
        bool isNotAllow = false;
        bool IsDoctor = common.myBool(hdnIsDoctor.Value);

        string origrule = string.Empty;
        string OrigRecRule = string.Empty;

        BaseC.Patient BC = new BaseC.Patient(sConString);
        BaseC.HospitalSetup objHp = new BaseC.HospitalSetup(sConString);

        Hashtable hshIn = new Hashtable();

        txtCancel.Text = string.Empty;
        lblMessage.Text = string.Empty;
        try
        {
            if (RegistrationId.Equals(0) && ((currentMenuValue == "A") || (currentMenuValue == "P")))
            {
                Alert.ShowAjaxMsg("Please Register the patient first", Page);
                return;
            }

            #region Region For No. Of Encounter

            DAL.DAL dlll = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dsAutNo = new DataSet();
            if (RegistrationId > 0)
            {
                dsAutNo = dlll.FillDataSet(CommandType.Text, "SELECT rid.NoOfEncounter, rid.EndDate FROM RegistrationInsuranceDetail rid inner join Registration r on rid.RegistrationId=r.Id WHERE rid.RegistrationID =" + RegistrationId.ToString() + " AND rid.InsuranceOrder = 'P' and r.defaultpaymentcaseid = 1 AND rid.Active = 1");

                if (dsAutNo.Tables[0].Rows.Count > 0)
                {
                    string NoOfEncounter = dsAutNo.Tables[0].Rows[0]["NoOfEncounter"].ToString();
                    string EndDate = dsAutNo.Tables[0].Rows[0]["EndDate"].ToString();
                    if (NoOfEncounter == "0")
                    {
                        Alert.ShowAjaxMsg("Patient maximum insurance coverage has been reached. Please address this payment issue.", Page);
                    }
                    else if (NoOfEncounter != "0" && NoOfEncounter != "")
                    {
                        Alert.ShowAjaxMsg("Patient has " + NoOfEncounter + " number of appointments remaining under their insurance plan.", Page);
                    }
                }
            }

            #endregion

            string strStatus = "select d.StatusID,s.Code from DoctorAppointment D with (nolock) inner join StatusMaster s on d.StatusId = s.StatusId where d.AppointmentID =" + AppointmentId;
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dsStatus = new DataSet();
            bool bStatus = false;
            string sStatusCode = "";
            int intStatusID = 0;
            int recparentid = 0;
            string sEncounterId = "";
            string recrule = "";
            DateTime OrigTime = DateTime.Now;
            DateTime OrigDate = DateTime.Today;

            dsStatus = dl.FillDataSet(CommandType.Text, strStatus);
            if (dsStatus.Tables[0].Rows.Count > 0)
            {
                if (common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "P" || common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "PC")
                {
                    bStatus = true;
                }
                else if (common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "PC")
                {
                    if (currentMenuValue != "Edit")
                        return;
                }
                intStatusID = Convert.ToInt32(dsStatus.Tables[0].Rows[0]["StatusID"].ToString());
                sStatusCode = common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString());
            }

            BaseC.Patient objPatient = new BaseC.Patient(sConString);
            if (RegistrationId > 0)
            {
                string strEncouter = " SELECT ENC.ID, ENC.RegistrationNo , DA.AppointmentID, DA.DoctorId, DA.FacilityID, DA.RegistrationId,  "
                                    + " DA.Gender, ENC.EncounterNo "
                                    + " FROM Encounter ENC INNER JOIN DoctorAppointment DA "
                                    + " On ENC.AppointmentID =DA.AppointmentID "
                                    + " WHERE DA.AppointmentID=" + AppointmentId + " AND DA.RegistrationId=" + RegistrationId.ToString();

                DataSet ds = new DataSet();
                ds = dl.FillDataSet(CommandType.Text, strEncouter);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int i = 0;
                    DataSet ds1 = (DataSet)Session["ModuleData"];
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        if (common.myStr(dr["ModuleName"]) == "EHR")
                        {
                            Session["ModuleId"] = i;
                        }
                        i++;
                    }
                    Session["Gender"] = ds.Tables[0].Rows[0]["Gender"].ToString();
                    Session["Facility"] = ds.Tables[0].Rows[0]["FacilityID"].ToString();
                    sEncounterId = ds.Tables[0].Rows[0]["ID"].ToString();
                    //Session["EncounterNo"] = ds.Tables[0].Rows[0]["EncounterNo"].ToString();
                    Session["DoctorID"] = ds.Tables[0].Rows[0]["DoctorId"].ToString();
                    Session["RegistrationID"] = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
                    Session["RegistrationNo"] = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                    Session["AppointmentID"] = ds.Tables[0].Rows[0]["AppointmentID"].ToString();
                }
            }

            switch (currentMenuValue)
            {
                case "Print"://Print Appointment
                    RadWindowForNew.NavigateUrl = "/EMRReports/AppointmentSlip.aspx?RegistrationId=" + common.myInt(RegistrationId) + "&AppointmentId=" + common.myInt(AppointmentId);
                    RadWindowForNew.Height = 580;
                    RadWindowForNew.Width = 750;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.ReloadOnShow = true;

                    break;

                //case "Copy"://Copy
                //    if (!isBreak)
                //    {
                //        ViewState["CopyRegistrationId"] = RegistrationId.ToString();
                //        ViewState["CopyAppId"] = AppointmentId;
                //        if (ViewState["CopyRegistrationId"].ToString().Trim().Equals(string.Empty))
                //        {
                //            ViewState["CopyRegistrationId"] = null;
                //        }
                //    }
                //    break;

                case "Edit"://Edit
                    if (statuscode == "PC")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow already billed appointment !";
                        Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                        return;
                    }
                    if (AppointmentId.ToString().Contains('_'))
                    {
                        recparentid = RecurrenceParentId;
                    }
                    else
                    {
                        recrule = "5";
                    }

                    RadWindowForNew.NavigateUrl = "~/Appointment/AppointmentDetails.aspx?StTime=" + ShiftFirstFromHour +
                                                    "&EndTime=" + ShiftFirstToHour +
                                                    "&appDate=" + common.myDate(AppointmentDate).ToString("MM/dd/yyyy") +
                                                    "&appid=" + AppointmentId + "&RecRule=" + recrule + "&recparentid=" + recparentid +
                                                    "&doctorId=" + DoctorId + "&TimeInterval=" + TimeTaken.ToString() +
                                                    "&AppId1=" + AppointmentId.ToString() + "&PageId=3" +
                                                    "&IsDoctor=" + IsDoctor + "&FacilityId=" + FacilityId;

                    RadWindowForNew.Height = 550;
                    RadWindowForNew.Width = 950;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;

                    break;

                case "DelChkIn"://Cancel Chk-In

                    if (statuscode == "PC")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow already billed appointment !";
                        Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                        return;
                    }
                    if (bStatus == true)
                    {
                        if (AppointmentId > 0)
                        {
                            if (sStatusCode == "P" || sStatusCode == "PC")
                            {
                                BaseC.Appointment appoint = new BaseC.Appointment(sConString);
                                DataSet ds = appoint.getAppointmentEncounter(Convert.ToInt32(AppointmentId));
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    int iEncounterId = common.myInt(ds.Tables[0].Rows[0]["EncounterId"]);
                                    int iRegistrationId = common.myInt(ds.Tables[0].Rows[0]["RegistrationId"]);

                                    DataView dvChk = ((DataTable)appoint.GetEncounterStatusToDelete(iEncounterId, iRegistrationId)).DefaultView;
                                    dvChk.RowFilter = "TemplateName <> 'Allergies'";
                                    DataTable dt = dvChk.ToTable();
                                    if (dt.Rows.Count > 0)
                                    {
                                        lblMessage.Text = "Check-In Status can't delete. Clinical Details Found.";
                                        Alert.ShowAjaxMsg("Check-In Status can't delete. Clinical Details Found !", Page);
                                        return;
                                    }
                                    else
                                    {
                                        ViewState["MenuStatus"] = "CheckedIn";
                                        ViewState["CancelEncounter"] = iEncounterId;
                                        ViewState["CancelAppointmentId"] = common.myInt(AppointmentId);
                                        ViewState["CancelRegistrationId"] = RegistrationId;
                                        lblDeleteApp.Text = "Cancel Appointment Chk-In";
                                        dvDelete.Visible = true;
                                        fillRemarks();
                                    }
                                }
                                else
                                {
                                    ViewState["MenuStatus"] = "CheckedIn";
                                    ViewState["CancelEncounter"] = 0;
                                    ViewState["CancelAppointmentId"] = common.myInt(AppointmentId);
                                    ViewState["CancelRegistrationId"] = common.myStr(RegistrationId);
                                    lblDeleteApp.Text = "Cancel Appointment Chk-In";
                                    dvDelete.Visible = true;
                                    fillRemarks();
                                }
                            }

                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Appointment Is Not Checked-In!", Page);
                        return;
                    }

                    break;

                case "Delete"://Cancel Appointment
                    if (statuscode == "PC")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow already billed appointment !";
                        Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                        return;
                    }
                    if (bStatus == true)
                    {
                        Alert.ShowAjaxMsg("Appointment Already Checked-In. Appointment Cannot Be Deleted.", Page.Page);
                        return;
                    }

                    isNotAllow = false;

                    if (DateTime.Compare(common.myDate(AppointmentDate).Date, common.myDate(DateTime.Now).Date) < 0)
                    {
                        isNotAllow = true;
                    }
                    if (isNotAllow)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow for back day & time appointment !";
                        Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
                        return;
                    }

                    lblDeleteApp.Text = "Delete Appointment";
                    ViewState["AppId"] = AppointmentId.ToString();
                    ViewState["WeekDateForRec"] = AppointmentDate;
                    ViewState["Rec"] = RecurrenceRule;
                    ViewState["StatusId"] = StatusId;
                    ViewState["MenuStatus"] = "Delete";
                    lblDeleteEncounterMessage.Text = "";
                    txtCancel.Text = "";

                    if (RecurrenceRule.Trim() != "")
                    {
                        dvDeleteRecurring.Visible = true;
                        fillRemarks();
                    }
                    else
                    {
                        dvDelete.Visible = true;
                        fillRemarks();
                    }

                    break;

                case "PatientDetails"://Patient Details
                    if (statuscode == "PC")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow already billed appointment !";
                        Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                        return;
                    }

                    isNotAllow = false;
                    if (DateTime.Compare(common.myDate(AppointmentDate).Date, common.myDate(DateTime.Now).Date) < 0)
                    {
                        isNotAllow = true;
                    }
                    if (isNotAllow)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow for back day & time appointment !";
                        Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
                        return;
                    }

                    if (RegistrationNo > 0)
                    {
                        RadWindowForNew.NavigateUrl = "~/PRegistration/Demographics.aspx?RNo=" + common.myLong(RegistrationNo) + "&mode=E&Mpg=P1&Category=PopUp&RealDateOfBirth=" + PatientDOB;
                    }
                    else
                    {
                        RadWindowForNew.NavigateUrl = "~/PRegistration/Demographics.aspx?AppID=" + AppointmentId + "&mode=N&Mpg=P1&Category=PopUp";
                    }

                    RadWindowForNew.Height = 630;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;

                    break;

                case "U"://UnConf
                    isNotAllow = false;
                    if (DateTime.Compare(common.myDate(AppointmentDate).Date, common.myDate(DateTime.Now).Date) < 0)
                    {
                        isNotAllow = true;
                    }
                    if (isNotAllow)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow for back day & time appointment !";
                        Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
                        return;
                    }

                    if (bStatus == true)
                    {
                        Alert.ShowAjaxMsg("Appointment Already Checked-In...", Page.Page);
                        return;
                    }

                    ViewState["MenuSdate"] = common.myDate(AppointmentDate);
                    if (currentMenuValue == "P" && Convert.ToInt32(FindFutureDate(Convert.ToString(common.myDate(AppointmentDate).Date))) > Convert.ToInt32(FindCurrentDate(Convert.ToString(System.DateTime.Today))))
                    {
                        string strMessage = "Warning. You are checking a patient in for a future appointment. You are not allowed to check the patient in or out.";
                        Alert.ShowAjaxMsg(strMessage, Page);
                        return;
                    }


                    origrule = "Select RecurrenceRule,AppointmentDate from doctorappointment where appointmentid='" + AppointmentId + "'";
                    OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();

                    hshIn = new Hashtable();
                    hshIn.Add("@intAppointmentId", AppointmentId);
                    hshIn.Add("@intEncodedBy", Session["UserId"]);
                    hshIn.Add("@chrStatusCode", currentMenuValue);
                    dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);

                    btnRefresh_OnClick(sender, e);

                    break;

                case "P"://Chk-in
                    isNotAllow = false;
                    if (DateTime.Compare(common.myDate(AppointmentDate).Date, common.myDate(DateTime.Now).Date) < 0)
                    {
                        isNotAllow = true;
                    }
                    if (isNotAllow)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow for back day & time appointment !";
                        Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
                        return;
                    }

                    if (bStatus == true)
                    {
                        Alert.ShowAjaxMsg("Appointment Already Checked-In...", Page.Page);
                        return;
                    }

                    ViewState["MenuSdate"] = common.myDate(AppointmentDate);
                    if (currentMenuValue == "P" && Convert.ToInt32(FindFutureDate(Convert.ToString(common.myDate(AppointmentDate).Date))) > Convert.ToInt32(FindCurrentDate(Convert.ToString(System.DateTime.Today))))
                    {
                        string strMessage = "Warning. You are checking a patient in for a future appointment. You are not allowed to check the patient in or out.";
                        Alert.ShowAjaxMsg(strMessage, Page);
                        return;
                    }
                    if (currentMenuValue == "P")
                    {
                        if (statuscode == "PC")
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "Activity not allow already billed appointment !";
                            Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                            return;
                        }
                        if (DateTime.Compare(common.myDate(AppointmentDate).Date, common.myDate(DateTime.Now).Date) != 0)
                        {
                            string strMessage = "Warning. You Are Only Allowed To CheckIn For Current Date.";
                            Alert.ShowAjaxMsg(strMessage, Page);
                            return;
                        }
                        BaseC.Appointment appoint = new BaseC.Appointment(sConString);

                        if (currentMenuValue == "P")
                        {
                            string sQuery = "SELECT id FROM Encounter en INNER JOIN DOCTORAPPOINTMENT DA ON EN.AppointmentId=da.AppointmentId INNER JOIN FacilityMaster fm ON en.FacilityId=fm.FacilityId INNER JOIN STATUSMASTER SM ON da.StatusId=sm.StatusId " +
                            " WHERE en.Registrationid =  @intRegistrationId AND CONVERT(VARCHAR,DATEADD(mi,fm.TIMEZONEOFFSETMINUTES,EncounterDate),111) = CONVERT(VARCHAR,DATEADD(mi,fm.TIMEZONEOFFSETMINUTES,GETUTCDATE()),111) " +
                                        " AND en.DoctorId = @intDoctorId AND EN.FacilityID = @intFacilityId AND SM.Code = @chvStatusCode AND OPIP ='O' AND EN.Active = 1";
                            Hashtable hshEncounter = new Hashtable();
                            hshEncounter.Add("@intRegistrationId", RegistrationId);
                            hshEncounter.Add("@intDoctorId", DoctorId);
                            hshEncounter.Add("@intFacilityId", FacilityId);
                            hshEncounter.Add("@chvStatusCode", currentMenuValue);
                            DataSet ds = dl.FillDataSet(CommandType.Text, sQuery, hshEncounter);
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    Alert.ShowAjaxMsg("Encounter already open", Page);
                                    return;
                                }
                            }
                        }

                        if (common.myBool(NoPAForInsurancePatient))
                        {
                            if (common.myStr(ddlSpecilization.SelectedItem.Attributes["Code"]) != "G")
                            {
                                bool IsRequiredPreAuthNo = appoint.GetPatientAccountType(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt32(RegistrationId), Convert.ToInt32(AppointmentId));
                                if (IsRequiredPreAuthNo == true)
                                {
                                    Alert.ShowAjaxMsg("Check-in not allow this patient. PA No. required.", Page);
                                    return;
                                }
                            }
                        }

                        if (AppointmentId > 0)
                        {
                            DataTable dsav = appoint.Checkvisittype(common.myInt(AppointmentId));
                            if (dsav.Rows.Count == 0)
                            {
                                Alert.ShowAjaxMsg("Check-in not allow this patient. Invalid Visit Type.", Page);
                                return;
                            }
                        }

                        hshIn = new Hashtable();
                        hshIn.Add("@intAppointmentId", AppointmentId);
                        hshIn.Add("@intEncodedBy", Session["UserId"]);
                        hshIn.Add("@chrStatusCode", currentMenuValue);
                        dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);

                        btnRefresh_OnClick(sender, e);
                    }
                    break;

                case "A"://Confirm

                    isNotAllow = false;
                    if (DateTime.Compare(common.myDate(AppointmentDate).Date, common.myDate(DateTime.Now).Date) < 0)
                    {
                        isNotAllow = true;
                    }
                    if (isNotAllow)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow for back day & time appointment !";
                        Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
                        return;
                    }

                    if (bStatus == true)
                    {
                        Alert.ShowAjaxMsg("Appointment Already Checked-In...", Page.Page);
                        return;
                    }

                    ViewState["MenuSdate"] = common.myDate(AppointmentDate);
                    if (currentMenuValue == "P" && Convert.ToInt32(FindFutureDate(Convert.ToString(common.myDate(AppointmentDate).Date))) > Convert.ToInt32(FindCurrentDate(Convert.ToString(System.DateTime.Today))))
                    {
                        string strMessage = "Warning. You are checking a patient in for a future appointment. You are not allowed to check the patient in or out.";
                        Alert.ShowAjaxMsg(strMessage, Page);
                        return;
                    }


                    origrule = "Select RecurrenceRule,AppointmentDate from doctorappointment where appointmentid='" + AppointmentId + "'";
                    OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();

                    //mmb
                    //if ((e.Appointment.RecurrenceState == RecurrenceState.Occurrence || e.Appointment.RecurrenceState == RecurrenceState.Exception) && OrigRecRule.Trim() != "")
                    //{
                    //    if (AppointmentId.ToString().Contains('_'))
                    //    {
                    //        recparentid = RecurrenceParentId;
                    //    }
                    //    DateTime dtStart = common.myDate(AppointmentDate).Date;
                    //    Hashtable hshInput = new Hashtable();
                    //    Hashtable hshOutput = new Hashtable();

                    //    hshInput.Add("@intParentAppointmentID", AppointmentId);
                    //    hshInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                    //    hshInput.Add("@intFacilityID", Session["FacilityID"]);
                    //    hshOutput.Add("@intStatusID", SqlDbType.Int);
                    //    hshInput.Add("@chrAppointmentDate", dtStart.ToString("MM/dd/yyyy"));
                    //    hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRGetRecurringAppointment", hshInput, hshOutput);
                    //    if (hshOutput["@intStatusID"].ToString() == "3")
                    //    {
                    //        btnRefresh_OnClick(sender, e);
                    //    }
                    //    else if (hshOutput["@intStatusID"].ToString() == "5")
                    //    {
                    //        btnRefresh_OnClick(sender, e);
                    //    }
                    //    else if (hshOutput["@intStatusID"].ToString() == "2")
                    //    {
                    //        btnRefresh_OnClick(sender, e);
                    //    }
                    //    else
                    //    {
                    //        Hashtable hshIn = new Hashtable();
                    //        Hashtable hshtableout = new Hashtable();
                    //        hshIn.Add("@intAppointmentId", AppointmentId);
                    //        hshIn.Add("@intEncodedBy", Session["UserId"]);
                    //        hshIn.Add("@chrStatusCode", currentMenuValue);
                    //        hshtableout.Add("@intRecAppointmentId", SqlDbType.Int);

                    //        hshIn.Add("@dtAppointmentDate", dtStart);

                    //        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspChangeRecurringAppointmentStatus", hshIn, hshtableout);

                    //        string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + AppointmentId + "'";
                    //        OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                    //        string strOrigDate = "select AppointmentDate,StatusId from DoctorAppointment where appointmentid='" + Convert.ToInt32(hshtableout["intRecAppointmentId"]) + "'";

                    //        string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";

                    //        if (!OrigRecRule.Contains("EXDATE"))
                    //        {
                    //            string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                    //            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + AppointmentId + "'";
                    //            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    //        }
                    //        else
                    //        {
                    //            string UpdateRule = OrigRecRule + "," + exdate;
                    //            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + AppointmentId + "'";
                    //            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    //        }
                    //        btnRefresh_OnClick(sender, e);
                    //    }
                    //}
                    //else
                    //{

                    hshIn = new Hashtable();
                    hshIn.Add("@intAppointmentId", AppointmentId);
                    hshIn.Add("@intEncodedBy", Session["UserId"]);
                    hshIn.Add("@chrStatusCode", currentMenuValue);
                    dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);

                    btnRefresh_OnClick(sender, e);
                    //}

                    break;

                case "PC"://Payment
                    if (statuscode == "PC")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow already billed appointment !";
                        Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                        return;
                    }

                    if (DateTime.Compare(common.myDate(strFromTime).Date, common.myDate(DateTime.Now).Date) > 0)
                    {
                        Alert.ShowAjaxMsg("Payment Cannot be taken for future appointment", Page);
                        return;
                    }

                    if ((common.myStr(statuscode) != "P"))
                    {
                        Alert.ShowAjaxMsg("Encounter not generated, Please check-in appointment !", Page);
                        return;
                    }
                    if ((common.myStr(statuscode) == "M"))
                    {
                        Alert.ShowAjaxMsg("Patient Status is No Show ! Payment Cannot be Taken", Page);
                        return;
                    }

                    if (intStatusID.ToString() != "1")
                    {
                        if (common.myInt(sEncounterId) > 0)
                        {
                            DataSet ds1 = (DataSet)Session["ModuleData"];
                            int i = 0;
                            foreach (DataRow dr in ds1.Tables[0].Rows)
                            {
                                if (common.myStr(dr["ModuleName"]) == "Billing")
                                {
                                    Session["ModuleId"] = i;
                                    Session["ModuleName"] = "Billing";
                                }
                                i++;
                            }
                            Response.Redirect("~/EMRBILLING/OPBill.aspx?DoctorId=" + DoctorId + "&RegNo=" + RegistrationNo + "&EncId=" + common.myInt(sEncounterId) + "&IsAppoint=1" + "&AppointmentId=" + AppointmentId + "&PackageId=" + common.myInt(PackageId) + "&PackageType=" + common.myStr(PackageType), false);
                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("UnConfirm appointments bill not generated, Please check-in appointment !", Page);
                        return;
                    }
                    break;

                //case "PCN"://Payment New
                //    if (statuscode == "PC")
                //    {
                //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //        lblMessage.Text = "Activity not allow already billed appointment !";
                //        Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                //        return;
                //    }

                //    if (DateTime.Compare(common.myDate(strFromTime).Date, common.myDate(DateTime.Now).Date) > 0)
                //    {
                //        Alert.ShowAjaxMsg("Payment Cannot be taken for future appointment", Page);
                //        return;
                //    }

                //    if ((common.myStr(statuscode) != "P"))
                //    {
                //        Alert.ShowAjaxMsg("Encounter not generated, Please check-in appointment !", Page);
                //        return;
                //    }
                //    if ((common.myStr(statuscode) == "M"))
                //    {
                //        Alert.ShowAjaxMsg("Patient Status is No Show ! Payment Cannot be Taken", Page);
                //        return;
                //    }

                //    if (intStatusID.ToString() != "1")
                //    {
                //        if (common.myInt(sEncounterId) > 0)
                //        {
                //            DataSet ds1 = Session["ModuleData"];
                //            int i = 0;
                //            foreach (DataRow dr in ds1.Tables[0].Rows)
                //            {
                //                if (common.myStr(dr["ModuleName"]) == "Billing")
                //                {
                //                    Session["ModuleId"] = i;
                //                    Session["ModuleName"] = "Billing";
                //                }
                //                i++;
                //            }
                //            Response.Redirect("~/EMRBILLING/OPBill.aspx?DoctorId=" + DoctorId + "&RegNo=" + RegistrationNo + "&EncId=" + common.myInt(sEncounterId) + "&IsAppoint=1" + "&AppointmentId=" + AppointmentId + "&PackageId=" + common.myInt(PackageId) + "&PackageType=" + common.myStr(PackageType), false);
                //        }
                //    }
                //    else
                //    {
                //        Alert.ShowAjaxMsg("UnConfirm appointments bill not generated, Please check-in appointment !", Page);
                //        return;
                //    }
                //    break;

                case "M"://No Show
                    isNotAllow = false;
                    if (DateTime.Compare(common.myDate(AppointmentDate).Date, common.myDate(DateTime.Now).Date) < 0)
                    {
                        isNotAllow = true;
                    }
                    if (isNotAllow)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow for back day & time appointment !";
                        Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
                        return;
                    }

                    if (bStatus == true)
                    {
                        Alert.ShowAjaxMsg("Appointment Already Checked-In...", Page.Page);
                        return;
                    }

                    ViewState["MenuSdate"] = common.myDate(AppointmentDate);
                    if (currentMenuValue == "P" && Convert.ToInt32(FindFutureDate(Convert.ToString(common.myDate(AppointmentDate).Date))) > Convert.ToInt32(FindCurrentDate(Convert.ToString(System.DateTime.Today))))
                    {
                        string strMessage = "Warning. You are checking a patient in for a future appointment. You are not allowed to check the patient in or out.";
                        Alert.ShowAjaxMsg(strMessage, Page);
                        return;
                    }

                    if (currentMenuValue == "M")
                    {
                        if (statuscode == "PC")
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "Activity not allow already billed appointment !";
                            Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                            return;
                        }
                        ViewState["MenuItemValue"] = currentMenuValue;
                        ViewState["MenuAppID"] = AppointmentId.ToString();
                        if (AppointmentId.ToString().Contains('_'))
                        {
                            ViewState["MenuRecurrenceParentID"] = Convert.ToString(RecurrenceParentId.ToString());
                        }
                        else
                        {
                            ViewState["MenuRecurrenceParentID"] = 0;
                        }
                        //ViewState["MenuRecurrenceState"] = e.Appointment.RecurrenceState;
                    }

                    break;

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
            //dl = null;
            BC = null;
            objHp = null;
        }

    }

    //protected void menuStatus_ItemClick(object sender, RadMenuEventArgs e)
    //{

    //    //    //Label lblPatientName = (Label)gvr.FindControl("lblPatientName");

    //    Telerik.Web.UI.RadContextMenu menuStatus = (RadContextMenu)e.Item.NamingContainer;

    //    GridViewRow gvr = (GridViewRow)menuStatus.NamingContainer;
    //    HiddenField hdnAppointmentID = (HiddenField)gvr.FindControl("hdnAppointmentID");
    //    HiddenField hdnRegistrationId = (HiddenField)gvr.FindControl("hdnRegistrationId");

    //    //    //int EncounterID = common.myInt(hdnEncounterId.Value);
    //    string currentMenuValue = common.myStr(menuStatus.SelectedValue);
    //    //    //string AdmStatus = common.myStr(hdnAdmStatus.Value);
    //    bool isBreak = false;
    //    int RegistrationId = common.myInt(hdnRegistrationId.Value);


    //    BaseC.Patient BC = new BaseC.Patient(sConString);
    //    BaseC.HospitalSetup objHp = new BaseC.HospitalSetup(sConString);


    //    try
    //    {
    //        //if ((common.myStr(e.Appointment.Attributes["RegistrationId"]) == "") && ((e.MenuItem.Value == "A") || (e.MenuItem.Value == "P")))
    //        //{
    //        //    Alert.ShowAjaxMsg("Please Register the patient first", Page);
    //        //    return;
    //        //}

    //        //string[] strAppId = e.Appointment.ID.ToString().Split('_');

    //        //DataTable dtLunchTable = (DataTable)ViewState["Lunch"];
    //        //DataView dv = new DataView(dtLunchTable);
    //        //dv.RowFilter = "AppointmentID=" + strAppId[0] + "";
    //        //DataTable dtBreak = dv.ToTable();
    //        //DataView dvLunch = new DataView(dtBreak);// AND Type='Break' OR Type='Block'";
    //        //dvLunch.RowFilter = " Type='Break' OR Type='Block'";
    //        //DataTable dtLunch = new DataTable();
    //        //dtLunch = dvLunch.ToTable();
    //        //int iAppointmentId = 0;
    //        //string statuscode = common.myStr(e.Appointment.Attributes["Code"]);
    //        //if (common.myInt(e.Appointment.Attributes["InvoiceId"]) > 0)
    //        //    iAppointmentId = common.myInt(e.Appointment.Attributes["InvoiceId"]);

    //        //if (dtLunch.Rows.Count > 0)
    //        //{
    //        //    isBreak = true;
    //        //}

    //        //#region Region For No. Of Encounter

    //        //DAL.DAL dlll = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        //DataSet dsAutNo = new DataSet();
    //        //if (e.Appointment.Attributes["RegistrationId"] != "")
    //        //{
    //        //    dsAutNo = dlll.FillDataSet(CommandType.Text, "SELECT rid.NoOfEncounter, rid.EndDate FROM RegistrationInsuranceDetail rid inner join Registration r on rid.RegistrationId=r.Id WHERE rid.RegistrationID =" + e.Appointment.Attributes["RegistrationId"].ToString() + " AND rid.InsuranceOrder = 'P' and r.defaultpaymentcaseid = 1 AND rid.Active = 1");

    //        //    if (dsAutNo.Tables[0].Rows.Count > 0)
    //        //    {
    //        //        hdnNoOfEncounter.Value = dsAutNo.Tables[0].Rows[0]["NoOfEncounter"].ToString();
    //        //        string EndDate = dsAutNo.Tables[0].Rows[0]["EndDate"].ToString();
    //        //        if (hdnNoOfEncounter.Value == "0")
    //        //        {
    //        //            Alert.ShowAjaxMsg("Patient maximum insurance coverage has been reached. Please address this payment issue.", Page);
    //        //        }
    //        //        else if (hdnNoOfEncounter.Value != "0" && hdnNoOfEncounter.Value != "")
    //        //        {
    //        //            Alert.ShowAjaxMsg("Patient has " + hdnNoOfEncounter.Value + " number of appointments remaining under their insurance plan.", Page);
    //        //        }
    //        //    }
    //        //}

    //        //#endregion

    //        //switch (currentMenuValue)
    //        //{
    //        //    case "Print"://Print Appointment
    //        //        break;
    //        //    //case "Copy"://Copy
    //        //    //    if (!isBreak)
    //        //    //    {
    //        //    //        ViewState["CopyRegistrationId"] = RegistrationId.ToString();
    //        //    //        ViewState["CopyAppId"] = strAppId[0];
    //        //    //        if (ViewState["CopyRegistrationId"].ToString().Trim().Equals(string.Empty))
    //        //    //        {
    //        //    //            ViewState["CopyRegistrationId"] = null;
    //        //    //        }
    //        //    //    }
    //        //    //    break;
    //        //    case "Edit"://Edit
    //        //        break;
    //        //    case "DelChkIn"://Cancel Chk-In
    //        //        break;
    //        //    case "Delete"://Cancel Appointment
    //        //        break;
    //        //    case "PatientDetails"://Patient Details
    //        //        break;
    //        //    case "U"://UnConf
    //        //        break;
    //        //    case "P"://Chk-in
    //        //        break;
    //        //    case "A"://Confirm
    //        //        break;
    //        //    case "PC"://Payment
    //        //        break;
    //        //    case "M"://No Show
    //        //        break;
    //        //}

    //        ////if (e.MenuItem.Value == "VH") // to show patient history always
    //        ////{
    //        ////    RadWindowForNew.NavigateUrl = "~/emr/Masters/PatientHistory.aspx?RegId=" + e.Appointment.Attributes["RegistrationId"].ToString() + "&RegNo=" + e.Appointment.Attributes["RegistrationNo"].ToString();
    //        ////    //RadWindowForNew.Height = 550;
    //        ////    //RadWindowForNew.Width = 730;
    //        ////    //RadWindowForNew.Top = 40;
    //        ////    //RadWindowForNew.Left = 100;
    //        ////    RadWindowForNew.OnClientClose = "OnClientClose";
    //        ////    RadWindowForNew.Modal = true;
    //        ////    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    //        ////    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //        ////    RadWindowForNew.VisibleStatusbar = false;
    //        ////}
    //        ////else if (e.MenuItem.Value == "AL")
    //        ////{
    //        ////    if (isBreak != true)
    //        ////    {
    //        ////        RadWindowForNew.NavigateUrl = "/Appointment/AppointmentList.aspx?regForDetails=" + e.Appointment.Attributes["RegistrationId"].ToString().Trim();

    //        ////        RadWindowForNew.Top = 40;
    //        ////        RadWindowForNew.Left = 100;
    //        ////        RadWindowForNew.OnClientClose = "OnClientClose";
    //        ////        RadWindowForNew.Modal = true;
    //        ////        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    //        ////        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //        ////        RadWindowForNew.VisibleStatusbar = false;
    //        ////    }
    //        ////}else 
    //        ////if (e.MenuItem.Value == "Copy")
    //        ////{
    //        ////    if (isBreak != true)
    //        ////    {
    //        ////        ViewState["CopyRegistrationId"] = e.Appointment.Attributes["RegistrationId"].ToString();
    //        ////        ViewState["CopyAppId"] = strAppId[0];
    //        ////        if (ViewState["CopyRegistrationId"].ToString().Trim() == "")
    //        ////        {
    //        ////            ViewState["CopyRegistrationId"] = null;
    //        ////        }
    //        ////    }
    //        ////}
    //        ////else 
    //        //if (e.MenuItem.Value == "Print")
    //        //{
    //        //    if (isBreak != true)
    //        //    {
    //        //        RadWindowForNew.NavigateUrl = "/EMRReports/AppointmentSlip.aspx?RegistrationId=" + common.myInt(e.Appointment.Attributes["RegistrationId"]) + "&AppointmentId=" + common.myInt(strAppId[0]);
    //        //        RadWindowForNew.Height = 580;
    //        //        RadWindowForNew.Width = 750;
    //        //        RadWindowForNew.Top = 40;
    //        //        RadWindowForNew.Left = 100;
    //        //        RadWindowForNew.OnClientClose = "OnClientClose";
    //        //        RadWindowForNew.Modal = true;
    //        //        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //        //        RadWindowForNew.VisibleStatusbar = false;
    //        //        RadWindowForNew.ReloadOnShow = true;
    //        //    }
    //        //}
    //        //else if (e.MenuItem.Value == "PatientDetails")
    //        //{
    //        //    if (statuscode == "PC")
    //        //    {
    //        //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //        lblMessage.Text = "Activity not allow already billed appointment !";
    //        //        Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
    //        //        return;
    //        //    }
    //        //    if (isBreak != true)
    //        //    {

    //        //        bool isNotAllow = false;
    //        //        if (DateTime.Compare(e.Appointment.Start.Date, DateTime.Now.Date) < 0)
    //        //        {
    //        //            isNotAllow = true;
    //        //        }
    //        //        if (isNotAllow)
    //        //        {
    //        //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //            lblMessage.Text = "Activity not allow for back day & time appointment !";
    //        //            Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
    //        //            return;
    //        //        }

    //        //        if (e.Appointment.Attributes["RegistrationId"].ToString() != "")
    //        //        {
    //        //            RadWindowForNew.NavigateUrl = "~/PRegistration/Demographics.aspx?RNo=" + common.myStr(e.Appointment.Attributes["RegistrationNo"]) + "&mode=E&Mpg=P1&Category=PopUp&RealDateOfBirth=" + e.Appointment.Attributes["RealDateOfBirth"];
    //        //            RadWindowForNew.Height = 630;
    //        //            RadWindowForNew.Width = 1000;
    //        //            RadWindowForNew.Top = 40;
    //        //            RadWindowForNew.Left = 100;
    //        //            RadWindowForNew.OnClientClose = "OnClientClose";
    //        //            RadWindowForNew.Modal = true;
    //        //            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //        //            RadWindowForNew.VisibleStatusbar = false;
    //        //            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    //        //        }
    //        //        else
    //        //        {
    //        //            RadWindowForNew.NavigateUrl = "~/PRegistration/Demographics.aspx?AppID=" + e.Appointment.ID + "&mode=N&Mpg=P1&Category=PopUp";
    //        //            RadWindowForNew.Height = 630;
    //        //            RadWindowForNew.Width = 1350;
    //        //            RadWindowForNew.Top = 40;
    //        //            RadWindowForNew.Left = 100;
    //        //            RadWindowForNew.OnClientClose = "OnClientClose";
    //        //            RadWindowForNew.Modal = true;
    //        //            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //        //            RadWindowForNew.VisibleStatusbar = false;
    //        //            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    //        //        }

    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    string strStatus = "select d.StatusID,s.Code from DoctorAppointment D with (nolock) inner join StatusMaster s on d.StatusId = s.StatusId where d.AppointmentID =" + strAppId[0];
    //        //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        //    DataSet dsStatus = new DataSet();
    //        //    bool bStatus = false;
    //        //    string sStatusCode = "";
    //        //    int intStatusID = 0;
    //        //    int recparentid = 0;
    //        //    string sEncounterId = "";
    //        //    string recrule = "";
    //        //    DateTime OrigTime = DateTime.Now;
    //        //    DateTime OrigDate = DateTime.Today;

    //        //    dsStatus = dl.FillDataSet(CommandType.Text, strStatus);
    //        //    if (dsStatus.Tables[0].Rows.Count > 0)
    //        //    {
    //        //        if (common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "P" || common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "PC")
    //        //        {
    //        //            bStatus = true;
    //        //        }
    //        //        else if (common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "PC")
    //        //        {
    //        //            if (e.MenuItem.Value != "Edit")
    //        //                return;
    //        //        }
    //        //        intStatusID = Convert.ToInt32(dsStatus.Tables[0].Rows[0]["StatusID"].ToString());
    //        //        sStatusCode = common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString());
    //        //    }

    //        //    BaseC.Patient objPatient = new BaseC.Patient(sConString);
    //        //    if (e.Appointment.Attributes["RegistrationId"] != "")
    //        //    {
    //        //        string strEncouter = " SELECT ENC.ID, ENC.RegistrationNo , DA.AppointmentID, DA.DoctorId, DA.FacilityID, DA.RegistrationId,  "
    //        //                            + " DA.Gender, ENC.EncounterNo "
    //        //                            + " FROM Encounter ENC INNER JOIN DoctorAppointment DA "
    //        //                            + " On ENC.AppointmentID =DA.AppointmentID "
    //        //                            + " WHERE DA.AppointmentID=" + strAppId[0] + " AND DA.RegistrationId=" + e.Appointment.Attributes["RegistrationId"].ToString() + " ";
    //        //        DataSet ds = new DataSet();
    //        //        ds = dl.FillDataSet(CommandType.Text, strEncouter);

    //        //        if (ds.Tables[0].Rows.Count > 0)
    //        //        {
    //        //            int i = 0;
    //        //            DataSet ds1 = Session["ModuleData"];
    //        //            foreach (DataRow dr in ds1.Tables[0].Rows)
    //        //            {
    //        //                if (common.myStr(dr["ModuleName"]) == "EHR")
    //        //                {
    //        //                    Session["ModuleId"] = i;
    //        //                }
    //        //                i++;
    //        //            }
    //        //            Session["Gender"] = ds.Tables[0].Rows[0]["Gender"].ToString();
    //        //            Session["Facility"] = ds.Tables[0].Rows[0]["FacilityID"].ToString();
    //        //            sEncounterId = ds.Tables[0].Rows[0]["ID"].ToString();
    //        //            //Session["EncounterNo"] = ds.Tables[0].Rows[0]["EncounterNo"].ToString();
    //        //            Session["DoctorID"] = ds.Tables[0].Rows[0]["DoctorId"].ToString();
    //        //            Session["RegistrationID"] = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
    //        //            Session["RegistrationNo"] = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
    //        //            Session["AppointmentID"] = ds.Tables[0].Rows[0]["AppointmentID"].ToString();
    //        //        }
    //        //    }

    //        //    if (e.MenuItem.Value == "Edit")
    //        //    {
    //        //        if (statuscode == "PC")
    //        //        {
    //        //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //            lblMessage.Text = "Activity not allow already billed appointment !";
    //        //            Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
    //        //            return;
    //        //        }
    //        //        if (e.Appointment.ID.ToString().Contains('_'))
    //        //        {
    //        //            recparentid = common.myInt(e.Appointment.RecurrenceParentID);
    //        //        }
    //        //        else
    //        //        {
    //        //            recrule = "5";
    //        //        }
    //        //        if (isBreak == true)
    //        //        {
    //        //            RadWindowForNew.NavigateUrl = "~/Appointment/BreakAndBlock.aspx?Category=PopUp&DoctorID=" + dtLunch.Rows[0]["DoctorID"].ToString() + " &ID=" + e.Appointment.ID.ToString() + "&appDate=" + e.Appointment.Start.Date.ToString("dd/MM/yyyy") + "&RecRule=" + recrule + "&recparentid=" + recparentid + "&appId=" + e.Appointment.ID.ToString() + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&StTime=" + RadScheduler1.DayStartTime.Hours.ToString() + "&EndTime=" + RadScheduler1.DayEndTime.Hours.ToString() + "&FacilityId=" + ddlFacility.SelectedValue;
    //        //            RadWindowForNew.Height = 550;
    //        //            RadWindowForNew.Width = 730;
    //        //            RadWindowForNew.Top = 40;
    //        //            RadWindowForNew.Left = 100;
    //        //            RadWindowForNew.OnClientClose = "OnClientClose";
    //        //            RadWindowForNew.Modal = true;
    //        //            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //        //            RadWindowForNew.VisibleStatusbar = false;
    //        //        }
    //        //        else
    //        //        {
    //        //            bool IsDoctor = false;
    //        //            if (ViewState["IsDoctor"] != null)
    //        //            {
    //        //                DataTable dt = (DataTable)ViewState["IsDoctor"];
    //        //                DataView dvDoctor = new DataView(dt);
    //        //                dvDoctor.RowFilter = "ResourceID='" + e.Appointment.Resources[0].Key + "'";
    //        //                DataTable dtDoctor = dvDoctor.ToTable();
    //        //                if (dtDoctor.Rows.Count > 0)
    //        //                {
    //        //                    IsDoctor = Convert.ToBoolean(dtDoctor.Rows[0]["IsDoctor"]);
    //        //                }
    //        //            }

    //        //            //RadWindowForNew.NavigateUrl = "~/Appointment/AppointmentDetails.aspx?StTime=" + RadScheduler1.DayStartTime.Hours.ToString() + "&EndTime=" + RadScheduler1.DayEndTime.Hours.ToString() + "&appDate=" + RadScheduler1.SelectedDate.Date.ToString("MM/dd/yyyy") + "&appid=" + strAppId[0] + "&RecRule=" + recrule + "&recparentid=" + recparentid + "&doctorId=" + e.Appointment.Resources[0].Key + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&PageId=" + Request.QueryString["Mpg"].ToString().Substring(1);
    //        //            RadWindowForNew.NavigateUrl = "~/Appointment/AppointmentDetails.aspx?StTime=" + RadScheduler1.DayStartTime.Hours.ToString() + "&EndTime=" + RadScheduler1.DayEndTime.Hours.ToString() + "&appDate=" + e.Appointment.Start.Date.ToString("MM/dd/yyyy") + "&appid=" + strAppId[0] + "&RecRule=" + recrule + "&recparentid=" + recparentid + "&doctorId=" + e.Appointment.Resources[0].Key + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&AppId1=" + e.Appointment.ID.ToString() + "&PageId=3" + "&IsDoctor=" + IsDoctor + "&FacilityId=" + ddlFacility.SelectedValue;
    //        //            RadWindowForNew.Height = 550;
    //        //            RadWindowForNew.Width = 950;
    //        //            RadWindowForNew.Top = 40;
    //        //            RadWindowForNew.Left = 100;
    //        //            RadWindowForNew.OnClientClose = "OnClientClose";
    //        //            RadWindowForNew.Modal = true;
    //        //            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //        //            RadWindowForNew.VisibleStatusbar = false;
    //        //            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    //        //        }
    //        //    }

    //        //    else if (e.MenuItem.Value == "PatientChart")
    //        //    {
    //        //        if (isBreak != true)
    //        //        {
    //        //            if (Convert.ToInt32(Session["encounterid"]) > 0)
    //        //            {
    //        //                Int32 intFormId = objPatient.GetPatientDefaultFormId(Convert.ToInt32(Session["encounterid"]), Convert.ToInt16(Session["HospitalLocationID"]));
    //        //                if (intFormId > 0)
    //        //                {
    //        //                    Session["formId"] = Convert.ToString(intFormId);
    //        //                }

    //        //                Response.Redirect("/emr/Dashboard/PatientDashboard.aspx?Mpg=P6", false);
    //        //            }
    //        //        }
    //        //    }
    //        //    else if (iAppointmentId > 0)
    //        //    {
    //        //        Alert.ShowAjaxMsg("Already billed appointment..", Page.Page);
    //        //        return;
    //        //    }
    //        //    else if (e.MenuItem.Value == "PC")
    //        //    {
    //        //        if (statuscode == "PC")
    //        //        {
    //        //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //            lblMessage.Text = "Activity not allow already billed appointment !";
    //        //            Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
    //        //            return;
    //        //        }
    //        //        if (isBreak != true)
    //        //        {
    //        //            if (DateTime.Compare(e.Appointment.Start.Date, DateTime.Now.Date) > 0)
    //        //            {
    //        //                Alert.ShowAjaxMsg("Payment Cannot be taken for future appointment", Page);
    //        //                return;
    //        //            }

    //        //            if ((common.myStr(e.Appointment.Attributes["Code"]) != "P"))
    //        //            {
    //        //                Alert.ShowAjaxMsg("Encounter not generated, Please check-in appointment !", Page);
    //        //                return;
    //        //            }
    //        //            if ((common.myStr(e.Appointment.Attributes["Code"]) == "M"))
    //        //            {
    //        //                Alert.ShowAjaxMsg("Patient Status is No Show ! Payment Cannot be Taken", Page);
    //        //                return;
    //        //            }

    //        //            if (intStatusID.ToString() != "1")
    //        //            {
    //        //                if (common.myInt(sEncounterId) > 0)
    //        //                {
    //        //                    DataSet ds1 = Session["ModuleData"];
    //        //                    int i = 0;
    //        //                    foreach (DataRow dr in ds1.Tables[0].Rows)
    //        //                    {
    //        //                        if (common.myStr(dr["ModuleName"]) == "Billing")
    //        //                        {
    //        //                            Session["ModuleId"] = i;
    //        //                            Session["ModuleName"] = "Billing";
    //        //                        }
    //        //                        i++;
    //        //                    }
    //        //                    string strRegNo = dl.ExecuteScalar(CommandType.Text, "Select RegistrationNo from Registration where id = " + e.Appointment.Attributes["RegistrationId"].ToString() + "").ToString();
    //        //                    Response.Redirect("~/EMRBILLING/OPBill.aspx?DoctorId=" + e.Appointment.Resources[0].Key + "&RegNo=" + strRegNo + "&EncId=" + common.myInt(sEncounterId) + "&IsAppoint=1" + "&AppointmentId=" + strAppId[0] + "&PackageId=" + common.myInt(e.Appointment.Attributes["PackageId"]) + "&PackageType=" + common.myStr(PackageType), false);
    //        //                }
    //        //            }
    //        //            else
    //        //            {
    //        //                Alert.ShowAjaxMsg("UnConfirm appointments bill not generated, Please check-in appointment !", Page);
    //        //                return;
    //        //            }
    //        //        }
    //        //    }
    //        //    else if (e.MenuItem.Value == "DelChkIn")
    //        //    {
    //        //        if (statuscode == "PC")
    //        //        {
    //        //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //            lblMessage.Text = "Activity not allow already billed appointment !";
    //        //            Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
    //        //            return;
    //        //        }
    //        //        if (bStatus == true)
    //        //        {
    //        //            if (strAppId[0].ToString() != "")
    //        //            {
    //        //                if (sStatusCode == "P" || sStatusCode == "PC")
    //        //                {
    //        //                    BaseC.Appointment appoint = new BaseC.Appointment(sConString);
    //        //                    DataSet ds = appoint.getAppointmentEncounter(Convert.ToInt32(strAppId[0]));
    //        //                    if (ds.Tables[0].Rows.Count > 0)
    //        //                    {
    //        //                        int EncounterId = Convert.ToInt32(ds.Tables[0].Rows[0]["EncounterId"]);
    //        //                        int RegistrationId = Convert.ToInt32(ds.Tables[0].Rows[0]["RegistrationId"]);

    //        //                        DataView dvChk = ((DataTable)appoint.GetEncounterStatusToDelete(EncounterId, RegistrationId)).DefaultView;
    //        //                        dvChk.RowFilter = "TemplateName <> 'Allergies'";
    //        //                        DataTable dt = dvChk.ToTable();
    //        //                        if (dt.Rows.Count > 0)
    //        //                        {
    //        //                            lblMessage.Text = "Check-In Status can't delete. Clinical Details Found.";
    //        //                            Alert.ShowAjaxMsg("Check-In Status can't delete. Clinical Details Found !", Page);
    //        //                            return;
    //        //                        }
    //        //                        else
    //        //                        {
    //        //                            ViewState["MenuStatus"] = "CheckedIn";
    //        //                            ViewState["CancelEncounter"] = EncounterId;
    //        //                            ViewState["CancelAppointmentId"] = common.myInt(strAppId[0]);
    //        //                            ViewState["CancelRegistrationId"] = RegistrationId;
    //        //                            lblDeleteApp.Text = "Cancel Appointment Chk-In";
    //        //                            dvDelete.Visible = true;
    //        //                            fillRemarks();
    //        //                        }
    //        //                    }
    //        //                    else
    //        //                    {
    //        //                        ViewState["MenuStatus"] = "CheckedIn";
    //        //                        ViewState["CancelEncounter"] = 0;
    //        //                        ViewState["CancelAppointmentId"] = common.myInt(strAppId[0]);
    //        //                        ViewState["CancelRegistrationId"] = common.myStr(e.Appointment.Attributes["RegistrationId"]);
    //        //                        lblDeleteApp.Text = "Cancel Appointment Chk-In";
    //        //                        dvDelete.Visible = true;
    //        //                        fillRemarks();
    //        //                    }
    //        //                }

    //        //            }
    //        //        }
    //        //        else
    //        //        {
    //        //            Alert.ShowAjaxMsg("Appointment Is Not Checked-In!", Page);
    //        //            return;
    //        //        }
    //        //    }
    //        //    else if (e.MenuItem.Value == "Delete")
    //        //    {
    //        //        if (statuscode == "PC")
    //        //        {
    //        //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //            lblMessage.Text = "Activity not allow already billed appointment !";
    //        //            Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
    //        //            return;
    //        //        }
    //        //        if (bStatus == true)
    //        //        {
    //        //            Alert.ShowAjaxMsg("Appointment Already Checked-In. Appointment Cannot Be Deleted.", Page.Page);
    //        //            return;
    //        //        }
    //        //        bool isNotAllow = false;
    //        //        if (DateTime.Compare(e.Appointment.Start.Date, DateTime.Now.Date) < 0)
    //        //        {
    //        //            isNotAllow = true;
    //        //        }
    //        //        if (isNotAllow)
    //        //        {
    //        //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //            lblMessage.Text = "Activity not allow for back day & time appointment !";
    //        //            Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
    //        //            return;
    //        //        }

    //        //        lblDeleteApp.Text = "Delete Appointment";
    //        //        ViewState["AppId"] = e.Appointment.ID.ToString();
    //        //        ViewState["WeekDateForRec"] = e.Appointment.Start;
    //        //        ViewState["Rec"] = e.Appointment.Attributes["RecurrenceRule"];
    //        //        ViewState["StatusId"] = e.Appointment.Attributes["StatusId"];
    //        //        ViewState["MenuStatus"] = "Delete";
    //        //        lblDeleteEncounterMessage.Text = "";
    //        //        txtCancel.Text = "";
    //        //        if (isBreak == true)
    //        //        {
    //        //            divDeleteBreak.Visible = true;
    //        //        }
    //        //        else
    //        //        {
    //        //            if (e.Appointment.Attributes["RecurrenceRule"].Trim() != "")
    //        //            {
    //        //                dvDeleteRecurring.Visible = true;
    //        //                fillRemarks();
    //        //            }
    //        //            else
    //        //            {

    //        //                dvDelete.Visible = true;
    //        //                fillRemarks();
    //        //            }
    //        //        }
    //        //    }

    //        //    else if (e.MenuItem.Value == "Superbill")
    //        //    {
    //        //        if (isBreak != true)
    //        //        {
    //        //            RadWindowForNew.NavigateUrl = "/EMRReports/PatientSuperBill.aspx?RegistrationId=" + e.Appointment.Attributes["RegistrationId"].ToString() + "&AppointmentId=" + strAppId[0];
    //        //            RadWindowForNew.Height = 600;
    //        //            RadWindowForNew.Width = 800;
    //        //            RadWindowForNew.Top = 40;
    //        //            RadWindowForNew.Left = 100;
    //        //            RadWindowForNew.OnClientClose = "OnClientClose";
    //        //            RadWindowForNew.Modal = true;
    //        //            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //        //            RadWindowForNew.VisibleStatusbar = false;
    //        //            RadWindowForNew.ReloadOnShow = true;
    //        //        }
    //        //    }

    //        //    else if (e.MenuItem.Value == "OpenClaim")
    //        //    {
    //        //        if (isBreak != true)
    //        //        {
    //        //            if (bStatus == true)
    //        //            {
    //        //                Session["ModuleId"] = 14;
    //        //                Session["ModuleName"] = "Billing";

    //        //                Hashtable hshInputData = new Hashtable();
    //        //                hshInputData.Add("@PatientID", e.Appointment.Attributes["RegistrationId"].ToString().Trim());
    //        //                hshInputData.Add("@EncounterID", common.myInt(Session["encounterid"]));
    //        //                hshInputData.Add("@EncodedBy", Session["UserID"]);
    //        //                DataSet ds1 = dl.FillDataSet(CommandType.StoredProcedure, "Bill_AddNewClaim", hshInputData);
    //        //                if (ds1.Tables[0].Rows.Count > 0)
    //        //                {
    //        //                    Response.Redirect("~/Billing/AddClaim.aspx?BillID=" + ds1.Tables[0].Rows[0]["BillID"].ToString() + "&Mpg=P143", false);
    //        //                }

    //        //            }
    //        //        }
    //        //    }
    //        //    else if (e.MenuItem.Value == "Orders")
    //        //    {
    //        //        if (isBreak != true)
    //        //        {

    //        //            RadWindowForNew.NavigateUrl = "/EMRBILLING/Order.aspx?regForDetails=" + e.Appointment.Attributes["RegistrationId"].ToString().Trim();
    //        //            RadWindowForNew.Height = 630;
    //        //            RadWindowForNew.Width = 980;
    //        //            RadWindowForNew.Top = 40;
    //        //            RadWindowForNew.Left = 100;
    //        //            RadWindowForNew.OnClientClose = "OnClientClose";
    //        //            RadWindowForNew.Modal = true;
    //        //            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //        //            RadWindowForNew.VisibleStatusbar = false;

    //        //        }
    //        //    }
    //        //    else //if (e.MenuItem.Value == "2") //Confirmed
    //        //    {
    //        //        if (isBreak != true)
    //        //        {
    //        //            bool isNotAllow = false;
    //        //            if (DateTime.Compare(e.Appointment.Start.Date, DateTime.Now.Date) < 0)
    //        //            {
    //        //                isNotAllow = true;
    //        //            }
    //        //            if (isNotAllow)
    //        //            {
    //        //                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //                lblMessage.Text = "Activity not allow for back day & time appointment !";
    //        //                Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
    //        //                return;
    //        //            }

    //        //            if (bStatus == true)
    //        //            {
    //        //                Alert.ShowAjaxMsg("Appointment Already Checked-In...", Page.Page);
    //        //                return;
    //        //            }

    //        //            ViewState["MenuSdate"] = e.Appointment.Start.Date;
    //        //            if (e.MenuItem.Value == "P" && Convert.ToInt32(FindFutureDate(Convert.ToString(e.Appointment.Start.Date))) > Convert.ToInt32(FindCurrentDate(Convert.ToString(System.DateTime.Today))))
    //        //            {
    //        //                string strMessage = "Warning. You are checking a patient in for a future appointment. You are not allowed to check the patient in or out.";
    //        //                Alert.ShowAjaxMsg(strMessage, Page);
    //        //                return;
    //        //            }
    //        //            if (e.MenuItem.Value == "P")
    //        //            {
    //        //                if (statuscode == "PC")
    //        //                {
    //        //                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //                    lblMessage.Text = "Activity not allow already billed appointment !";
    //        //                    Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
    //        //                    return;
    //        //                }
    //        //                if (DateTime.Compare(e.Appointment.Start.Date, DateTime.Now.Date) != 0)
    //        //                {
    //        //                    string strMessage = "Warning. You Are Only Allowed To CheckIn For Current Date.";
    //        //                    Alert.ShowAjaxMsg(strMessage, Page);
    //        //                    return;
    //        //                }
    //        //                BaseC.Appointment appoint = new BaseC.Appointment(sConString);

    //        //                if (e.MenuItem.Value == "P")
    //        //                {
    //        //                    string sQuery = "SELECT id FROM Encounter en INNER JOIN DOCTORAPPOINTMENT DA ON EN.AppointmentId=da.AppointmentId INNER JOIN FacilityMaster fm ON en.FacilityId=fm.FacilityId INNER JOIN STATUSMASTER SM ON da.StatusId=sm.StatusId " +
    //        //                    " WHERE en.Registrationid =  @intRegistrationId AND CONVERT(VARCHAR,DATEADD(mi,fm.TIMEZONEOFFSETMINUTES,EncounterDate),111) = CONVERT(VARCHAR,DATEADD(mi,fm.TIMEZONEOFFSETMINUTES,GETUTCDATE()),111) " +
    //        //                                " AND en.DoctorId = @intDoctorId AND EN.FacilityID = @intFacilityId AND SM.Code = @chvStatusCode AND OPIP ='O' AND EN.Active = 1";
    //        //                    Hashtable hshEncounter = new Hashtable();
    //        //                    hshEncounter.Add("@intRegistrationId", e.Appointment.Attributes["RegistrationId"]);
    //        //                    hshEncounter.Add("@intDoctorId", e.Appointment.Attributes["DoctorId"]);
    //        //                    hshEncounter.Add("@intFacilityId", ddlFacility.SelectedValue);
    //        //                    hshEncounter.Add("@chvStatusCode", e.MenuItem.Value);
    //        //                    DataSet ds = dl.FillDataSet(CommandType.Text, sQuery, hshEncounter);
    //        //                    if (ds.Tables.Count > 0)
    //        //                    {
    //        //                        if (ds.Tables[0].Rows.Count > 0)
    //        //                        {
    //        //                            Alert.ShowAjaxMsg("Encounter already open", Page);
    //        //                            return;
    //        //                        }
    //        //                    }
    //        //                }

    //        //                if (common.myStr(e.Appointment.Attributes["NoPAForInsurancePatient"]) == "False" || common.myStr(e.Appointment.Attributes["NoPAForInsurancePatient"]) == "0")
    //        //                {
    //        //                    if (common.myStr(ddlSpecilization.SelectedItem.Attributes["Code"]) != "G")
    //        //                    {
    //        //                        bool IsRequiredPreAuthNo = appoint.GetPatientAccountType(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt32(e.Appointment.Attributes["RegistrationId"]), Convert.ToInt32(e.Appointment.ID));
    //        //                        if (IsRequiredPreAuthNo == true)
    //        //                        {
    //        //                            Alert.ShowAjaxMsg("Check-in not allow this patient. PA No. required.", Page);
    //        //                            return;
    //        //                        }
    //        //                    }
    //        //                }
    //        //                if (strAppId[0] != "")
    //        //                {
    //        //                    DataTable dsav = appoint.Checkvisittype(common.myInt(strAppId[0]));
    //        //                    if (dsav.Rows.Count == 0)
    //        //                    {
    //        //                        Alert.ShowAjaxMsg("Check-in not allow this patient. Invalid Visit Type.", Page);
    //        //                        return;
    //        //                    }

    //        //                }

    //        //            }
    //        //            if (e.MenuItem.Value == "M")
    //        //            {
    //        //                if (statuscode == "PC")
    //        //                {
    //        //                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //                    lblMessage.Text = "Activity not allow already billed appointment !";
    //        //                    Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
    //        //                    return;
    //        //                }
    //        //                ViewState["MenuItemValue"] = e.MenuItem.Value;
    //        //                ViewState["MenuAppID"] = e.Appointment.ID.ToString();
    //        //                if (e.Appointment.ID.ToString().Contains('_'))
    //        //                {
    //        //                    ViewState["MenuRecurrenceParentID"] = Convert.ToString(e.Appointment.RecurrenceParentID.ToString());
    //        //                }
    //        //                else
    //        //                {
    //        //                    ViewState["MenuRecurrenceParentID"] = 0;
    //        //                }
    //        //                ViewState["MenuRecurrenceState"] = e.Appointment.RecurrenceState;

    //        //            }
    //        //            else
    //        //            {
    //        //                string origrule = "Select RecurrenceRule,AppointmentDate from doctorappointment where appointmentid='" + strAppId[0] + "'";
    //        //                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
    //        //                if ((e.Appointment.RecurrenceState == RecurrenceState.Occurrence || e.Appointment.RecurrenceState == RecurrenceState.Exception) && OrigRecRule.Trim() != "")
    //        //                {
    //        //                    if (e.Appointment.ID.ToString().Contains('_'))
    //        //                    {
    //        //                        recparentid = common.myInt(e.Appointment.RecurrenceParentID);
    //        //                    }
    //        //                    DateTime dtStart = e.Appointment.Start;
    //        //                    Hashtable hshInput = new Hashtable();
    //        //                    Hashtable hshOutput = new Hashtable();

    //        //                    hshInput.Add("@intParentAppointmentID", strAppId[0]);
    //        //                    hshInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
    //        //                    hshInput.Add("@intFacilityID", Session["FacilityID"]);
    //        //                    hshOutput.Add("@intStatusID", SqlDbType.Int);
    //        //                    hshInput.Add("@chrAppointmentDate", dtStart.ToString("MM/dd/yyyy"));
    //        //                    hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRGetRecurringAppointment", hshInput, hshOutput);
    //        //                    if (hshOutput["@intStatusID"].ToString() == "3")
    //        //                    {
    //        //                        btnRefresh_OnClick(sender, e);
    //        //                    }
    //        //                    else if (hshOutput["@intStatusID"].ToString() == "5")
    //        //                    {
    //        //                        btnRefresh_OnClick(sender, e);
    //        //                    }
    //        //                    else if (hshOutput["@intStatusID"].ToString() == "2")
    //        //                    {
    //        //                        btnRefresh_OnClick(sender, e);
    //        //                    }
    //        //                    else
    //        //                    {
    //        //                        Hashtable hshIn = new Hashtable();
    //        //                        Hashtable hshtableout = new Hashtable();
    //        //                        hshIn.Add("@intAppointmentId", strAppId[0]);
    //        //                        hshIn.Add("@intEncodedBy", Session["UserId"]);
    //        //                        hshIn.Add("@chrStatusCode", e.MenuItem.Value);
    //        //                        hshtableout.Add("@intRecAppointmentId", SqlDbType.Int);

    //        //                        hshIn.Add("@dtAppointmentDate", dtStart);

    //        //                        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspChangeRecurringAppointmentStatus", hshIn, hshtableout);

    //        //                        string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
    //        //                        OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
    //        //                        string strOrigDate = "select AppointmentDate,StatusId from DoctorAppointment where appointmentid='" + Convert.ToInt32(hshtableout["intRecAppointmentId"]) + "'";

    //        //                        string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";

    //        //                        if (!OrigRecRule.Contains("EXDATE"))
    //        //                        {
    //        //                            string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
    //        //                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
    //        //                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
    //        //                        }
    //        //                        else
    //        //                        {
    //        //                            string UpdateRule = OrigRecRule + "," + exdate;
    //        //                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
    //        //                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
    //        //                        }
    //        //                        btnRefresh_OnClick(sender, e);
    //        //                    }
    //        //                }
    //        //                else
    //        //                {

    //        //                    Hashtable hshIn = new Hashtable();
    //        //                    hshIn.Add("@intAppointmentId", strAppId[0]);
    //        //                    hshIn.Add("@intEncodedBy", Session["UserId"]);
    //        //                    hshIn.Add("@chrStatusCode", e.MenuItem.Value);
    //        //                    dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);

    //        //                    btnRefresh_OnClick(sender, e);
    //        //                }
    //        //            }
    //        //        }

    //        //    }
    //        //}
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        //dl = null;
    //        BC = null;
    //        objHp = null;
    //    }

    //}

    //Added By Ujjwal 17 June 2015 to Bind Context Menus on role based start
    private void BindGrpTagContextMenu()
    {
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        try
        {
            //1 for Registration module, 2 for Appointment Module, 3 for EMR, 30 for Ward Module
            if (common.myInt(Session["ModuleId"]).Equals(1) || common.myInt(Session["ModuleId"]).Equals(2))
            {
                ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                    common.myInt(Session["ModuleId"]), "Search_Appointment");
            }
            else if (common.myInt(Session["ModuleId"]).Equals(3) || common.myInt(Session["ModuleId"]).Equals(30))
            {
                ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                    common.myInt(Session["ModuleId"]), "Search_FollowupApp");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            else
            {
                ViewState["dtable1"] = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            user = null;
            ds.Dispose();
        }
    }
    //Added By Ujjwal 17 June 2015 to Bind Context Menus on role based end

}