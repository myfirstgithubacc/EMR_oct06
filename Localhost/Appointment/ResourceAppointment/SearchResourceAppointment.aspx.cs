using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using BaseC;
using System.IO;

public partial class Appointment_ResourceAppointment_SearchResourceAppointment : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    //protected void Page_PreInit(object sender, System.EventArgs e)
    //{
    //    Page.Theme = "DefaultControls";
    //}

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
            dtpfromDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);

            dtpfromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            dtpfromDate.MaxDate = DateTime.Now.Date;
            bindControl();

            bindData();
        }
    }

    private void bindControl()
    {
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
        BaseC.ATD objadt = new BaseC.ATD(sConString);
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
            lstStatus.Value = "0";
            lstStatus.Selected = true;

            ddlAppointmentStatus.Items.Add(lstStatus);

            DataTable Statusdt = new DataTable();
            ds = getAppointmentStatus();
            //strSQL = "select StatusId, Status,StatusColor from GetStatus(" + common.myInt(Session["HospitalLocationId"]) + ",'Appointment')";
            ds = getAppointmentStatus();
            DataView DV = ds.Tables[0].DefaultView;
            DV.RowFilter = "Status not in ('Cancel')";
            Statusdt = DV.ToTable();
            for (int i = 0; i < Statusdt.Rows.Count; i++)
            {
                lstStatus = new RadComboBoxItem();

                lstStatus.Attributes.Add("style", "background-color:" + common.myStr(Statusdt.Rows[i]["StatusColor"]) + ";");
                lstStatus.Text = common.myStr(Statusdt.Rows[i]["Status"]);
                lstStatus.Value = common.myStr(Statusdt.Rows[i]["StatusId"]);

                ddlAppointmentStatus.Items.Add(lstStatus);
            }

            #endregion

            ds = objadt.GetReportTypes(common.myInt(Session["HospitalLocationId"]), "Ward", common.myInt(Session["FacilityId"]));
            ddlWard.DataSource = ds.Tables[0];
            ddlWard.DataTextField = "Name";
            ddlWard.DataValueField = "ID";
            ddlWard.DataBind();
            ddlWard.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlWard.SelectedIndex = 0;

            bindSubDepartment();
            PopulateResourceName();
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
            objadt = null;
        }
    }

    private DataSet getAppointmentStatus()
    {
        DataSet ds = new DataSet();
        BaseC.clsLISPhlebotomy objLP = new BaseC.clsLISPhlebotomy(sConString);
        try
        {
            ds = objLP.getStatus(common.myInt(Session["HospitalLocationId"]), "ResourceStatus", string.Empty);
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
                Label lbl_StatusColor = new Label();


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

  

    protected void rdbActive_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
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
                string ResourceIds = string.Empty;
                String ChvResource = string.Empty;
                Int64 RegistrationNo = 0;
                string FromDate = string.Empty;
                string ToDate = string.Empty;
                string OldRegistrationNo = string.Empty;
                string EnrolleNo = string.Empty;
                string MobileNo = string.Empty;
                string EncounterNo = string.Empty;
                foreach (RadComboBoxItem itm in ddlResource.CheckedItems)
                {
                    ResourceIds = ResourceIds + itm.Value + ',';
                }
                ChvResource = common.myStr(ddlSource.SelectedValue);
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

                    case "MN":
                        MobileNo = common.myStr(txtSearch.Text).Trim();
                        break;

                    case "IP":
                        EncounterNo = common.myStr(txtSearch.Text).Trim();
                        break;
                }

                ds = objD.getResourcePDAppointment(common.myInt(Session["HospitalLocationID"]), PatientName, ResourceIds.Trim(','),
                                            common.myInt(ddlAppointmentStatus.SelectedValue), common.myInt(ddlLocation.SelectedValue),
                                            RegistrationNo, common.myStr(ddlrange.SelectedValue), FromDate, ToDate, 
                                            common.myInt(Session["FacilityID"]), MobileNo,  "I", common.myInt(ddlWard.SelectedValue),common.myInt(rdbActive.SelectedValue), EncounterNo);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        DataRow DR = ds.Tables[0].NewRow();
                        ds.Tables[0].Rows.Add(DR);
                    }
                    else
                    {
                        ViewState["Count"] = ds.Tables[0].Rows.Count;
                        DataRow DR = ds.Tables[0].NewRow();
                        ds.Tables[0].Rows.Add(DR);
                    }
                    DataView DV = ds.Tables[0].DefaultView;
                    DV.Sort = "AppointmentId DESC";

                    gvAppointmentList.DataSource = DV.ToTable();
                    gvAppointmentList.DataBind();
                    ViewState["ResourceAppointment"] = null;
                    ViewState["ResourceAppointment"] = DV.ToTable();
                }
                else
                {
                    gvAppointmentList.DataSource = null;
                    gvAppointmentList.DataBind();
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
            objD = null;
            ds.Dispose();
        }
    }

    protected void btnExport_OnClick(Object sender, EventArgs e)
    {
        if (ViewState["ResourceAppointment"] != null)
        {
            DataTable dt = ((DataTable)ViewState["ResourceAppointment"]).Copy();

            if (dt != null)
            {

                dt.Columns["Source"].ColumnName = "Source";
                dt.Columns["Status"].ColumnName = "Status";
                dt.Columns["AppointmentDate"].ColumnName = "Appointment Date";
                dt.Columns["registrationno"].ColumnName = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "regno"));
                dt.Columns["EncounterNo"].ColumnName = "Encounter No.";
                dt.Columns["PatientName"].ColumnName = "Patient Name";
                dt.Columns["MobileNo"].ColumnName = "Mobile No.";
                dt.Columns["AgeGender"].ColumnName = "Age/Gender";
                dt.Columns["BedNo"].ColumnName = "Bed No.";
                dt.Columns["WardName"].ColumnName = "Ward";       
                dt.Columns["ServiceName"].ColumnName = "Service Name";
                dt.Columns["ResourceName"].ColumnName = "Resource Name";
                dt.Columns["Precaution"].ColumnName = "Precautions";
                dt.Columns["Remarks"].ColumnName = "Remarks";
                dt.Columns["Bookedby"].ColumnName = "Booked By";
                dt.Columns["BookedDate"].ColumnName = "Booked Date";
                dt.Columns["CheckInby"].ColumnName = "Check In By";
                dt.Columns["CheckInDate"].ColumnName = "Check In Date";
                dt.Columns["Orderedby"].ColumnName = "Ordered By";
                dt.Columns["OrderedDate"].ColumnName = "Ordered Date";

                dt.Columns.Remove("HospitalLocationID");
                dt.Columns.Remove("AppointmentID");
                dt.Columns.Remove("ResourceId");
                dt.Columns.Remove("StatusId");
                dt.Columns.Remove("FromTime");
                dt.Columns.Remove("ToTime");
                dt.Columns.Remove("Gender");
                dt.Columns.Remove("Duration");
                dt.Columns.Remove("PatientDOB");
                dt.Columns.Remove("PackageType");
                dt.Columns.Remove("Subdepartment");
                dt.Columns.Remove("FacilityId");
                dt.Columns.Remove("StatusColor");
                dt.Columns.Remove("FacilityName");
                dt.Columns.Remove("RegistrationId");
                HttpResponse response = HttpContext.Current.Response;

                // first let's clean up the response.object
                response.Clear();
                response.Charset = "";

                // set the response mime type for excel
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", "attachment;filename=ResourceAppointment.xls");

                // create a string writer
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        DataGrid dg = new DataGrid();
                        dg.DataSource = dt;
                        dg.DataBind();
                        dg.RenderControl(htw);
                        response.Flush();
                        response.Write(sw.ToString());
                        response.End();
                        response.Close();
                    }
                    Response.ClearContent();
                }
            }
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

    protected void ddlSubDepartment_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            PopulateResourceName();
            btnRefresh_OnClick(null, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateResourceName()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hospital hs = new Hospital(sConString);
        DataSet ds = new DataSet();
        try
        {

            ds = hs.GetResourceMaster(Convert.ToInt16(common.myInt(Session["HospitalLocationId"])), Convert.ToInt16(common.myInt(ddlLocation.SelectedValue)),
                                            1, 0, common.myInt(ddlSubDepartment.SelectedValue));

            ddlResource.Items.Clear();
            ddlResource.DataSource = null;
            ddlResource.DataBind();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dr["ResourceName"];
                item.Value = dr["ResourceId"].ToString();
                item.Attributes.Add("SubDeptId", common.myStr(dr["SubDeptId"]));
                ddlResource.Items.Add(item);
                item.DataBind();
            }

            common.CheckAllItems(ddlResource);
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
            hs = null;
            ds.Dispose();
        }
    }


    protected void bindSubDepartment()
    {
        BaseC.clsLISMaster objLis = new BaseC.clsLISMaster(sConString);
        DataSet ds = new DataSet();
        int stationid = 0;

        try
        {
            if (common.myInt(Session["StationId"]) > 0 && common.myInt(Session["ModuleId"]).Equals(33))
            {
                stationid = common.myInt(Session["StationId"]);

                ds = objLis.GetSubDepartment(stationid, 0);
            }
            else
            {
                ds = objLis.getRISSubDepartment(common.myInt(Session["FacilityId"]));
            }
            DataView DV = ds.Tables[0].DefaultView;
            DV.Sort = "SubName ASC";
            ddlSubDepartment.DataSource = DV.ToTable();
            ddlSubDepartment.DataTextField = "SubName";
            ddlSubDepartment.DataValueField = "SubDeptId";
            ddlSubDepartment.DataBind();
            ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("All", "0"));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlSubDepartment.SelectedIndex = 0;
            }

            PopulateResourceName();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objLis = null;
            ds.Dispose();
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
        ddlSubDepartment.SelectedIndex = 0;
        ddlSource.SelectedIndex = 0;
        ddlrange.SelectedIndex = ddlrange.Items.IndexOf(ddlrange.Items.FindItemByValue("DD0"));
        ddlrange_SelectedIndexChanged(this, null);
        ddlSubDepartment_SelectedIndexChanged(this, null);
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

            DataSet Statusds = new DataSet();
            DataTable Statusdt = new DataTable();
            Statusds = getAppointmentStatus();
            DataView DV = Statusds.Tables[0].DefaultView;
            DV.RowFilter = "Status not in ('Cancel')";
            Statusdt = DV.ToTable();
            if (Statusdt.Rows.Count > 0)
            {
                for (int counter = 0; counter <= Statusdt.Rows.Count - 1; counter++)
                {
                    if (!Statusdt.Rows[counter]["Status"].ToString().Equals("Cancel"))
                    {
                        if (ua.CheckPermissions("C", "/Appointment/AppointmentDetails.aspx", true))
                        {
                            dr = dt.NewRow();
                            dr["Text"] = Statusdt.Rows[counter]["Status"].ToString();
                            dr["Value"] = Statusdt.Rows[counter]["Code"].ToString();
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        dr = dt.NewRow();
                        dr["Text"] = Statusdt.Rows[counter]["Status"].ToString();
                        dr["Value"] = Statusdt.Rows[counter]["Code"].ToString();
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

}