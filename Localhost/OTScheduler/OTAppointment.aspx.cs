using System;
using System.Text;
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
using System.Data.SqlClient;
using Telerik.Web.UI.Calendar;

public partial class OT_Scheduler_OTAppointment : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.RestFulAPI objwcfOt;//= new BaseC.RestFulAPI();
    BaseC.RestFulAPI objwcfcm;//= new wcf_Service_Common.CommonMasterClient();
    private const int ItemsPerRequest = 50;
    StringBuilder strSQLQ = new StringBuilder();
    string strSQL;
    DAL.DAL dl;
    DataTable dtTemp;
    DataSet Statusdt;
    string strApptID;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";

        }
        else
        {
            this.MasterPageFile = "~/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["EditCheckIN"] = null;
            objwcfcm = new BaseC.RestFulAPI(sConString);
            objwcfOt = new BaseC.RestFulAPI(sConString);

            if (Session["UserID"] == null || Session["HospitalLocationID"] == null || Session["FacilityId"] == null)
            {
                Response.Redirect("~/Login.aspx?Logout=1", false);
                return;
            }

            if (!IsPostBack)
            {
                BaseC.HospitalSetup objHosp = new BaseC.HospitalSetup(sConString);


                ViewState["IsBillClearanceByPassForOT"] = objHosp.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsBillClearanceByPassForOT", common.myInt(Session["FacilityId"]));

                if (common.myStr(ViewState["IsBillClearanceByPassForOT"]) == "")
                {
                    ViewState["IsBillClearanceByPassForOT"] = "N";
                }

                BaseC.Security obj = new BaseC.Security(sConString);
                if (!obj.CheckUserRights(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowOTCheckInTime"))
                {
                    dtDateTimeForOT.Enabled = false;
                }
                else
                {
                    dtDateTimeForOT.Enabled = true;

                }
                #region UnConf status
                BaseC.clsLISPhlebotomy objlis = new BaseC.clsLISPhlebotomy(sConString);
                DataSet dsStatus = objlis.getStatus(common.myInt(Session["HospitalLocationId"]), "Appointment", "U");

                ViewState["UnConfColor"] = "#FFFBC7";
                if (dsStatus.Tables[0].Rows.Count > 0)
                {
                    ViewState["UnConfColor"] = (common.myStr(dsStatus.Tables[0].Rows[0]["StatusColor"]) == "123") ? "#FFFBC7" : common.myStr(dsStatus.Tables[0].Rows[0]["StatusColor"]);
                }
                #endregion

                RadScheduler1.Width = Unit.Pixel(1150);
                PopulateFacility();
                ddlFacility.SelectedValue = Session["FacilityId"].ToString();
                PopulateOTName();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //Hashtable hsinput = new Hashtable();
                // hsinput.Add("@intfacilityid", Convert.ToInt32(Session["FacilityID"]));
                //string strtimezone = "Select TimeZoneOffSetMinutes  from FacilityMaster where FacilityID=@intfacilityid";
                DataSet ds = new DataSet();
                // ds = dl.FillDataSet(CommandType.Text, strtimezone, hsinput);
                ds = objwcfOt.GetTimeZone(common.myInt(Session["FacilityId"]));
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int timezone = Convert.ToInt32(ds.Tables[0].Rows[0]["TimeZoneOffSetMinutes"].ToString());

                    ViewState["SELECTED_DATE"] = common.myDate(DateTime.UtcNow).AddMinutes(timezone);
                }
                else
                {

                    ViewState["SELECTED_DATE"] = common.myDate(DateTime.UtcNow);
                    Alert.ShowAjaxMsg("Please Enter Offset Time in Facilitymaster Table in Database", Page);
                    return;
                }

                dtpDate.SelectedDate = common.myDate(ViewState["SELECTED_DATE"]);
                BindGroupTaggingMenu();
                btnRefresh_OnClick(sender, e);
                populatColorList();
            }
            Legend1.loadLegend("OT", "");
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }

    public void FillReasonBlockType()
    {
        BaseC.EMRBilling objEmrBilling = new BaseC.EMRBilling(sConString);
        DataSet ds = new DataSet();
        try
        {
            ddlReason.Items.Clear();

            ds = objEmrBilling.GetReasontype("OTReasonUncheckin");

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlReason.DataSource = ds.Tables[0];
            //    ddlReason.DataTextField = "Reason";
            //    ddlReason.DataValueField = "Id";
            //    ddlReason.DataBind();
            //}
            //ddlReason.Items.Insert(0, new RadComboBoxItem(" Select ", "0"));
            //ddlReason.Items.Insert(1, new RadComboBoxItem(" Other ", "1"));
            //ddlReason.SelectedIndex = 0;

            int cnt = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cnt++;
                    item = new RadComboBoxItem();
                    item.Text = common.myStr(dr["Reason"]);
                    item.Value = common.myInt(dr["Id"]).ToString();

                    ddlReason.Items.Add(item);
                }
            }

            ddlReason.Items.Insert(0, new RadComboBoxItem(" Select ", "0"));
            if (cnt > 0)
            {
                cnt++;
                ddlReason.Items.Insert(1, new RadComboBoxItem(" Other ", cnt.ToString()));
            }
            else
            {
                ddlReason.Items.Insert(1, new RadComboBoxItem(" Other ", "1"));
            }

            ddlReason.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
        }
    }

    public void GetCancelReason()
    {
        BaseC.EMRBilling objEmrBilling = new BaseC.EMRBilling(sConString);
        DataSet ds = new DataSet();
        try
        {
            ddlReason.Items.Clear();
            ds = objEmrBilling.GetReasontype("OTCancelReason");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlReason.DataSource = ds.Tables[0];
                ddlReason.DataTextField = "Reason";
                ddlReason.DataValueField = "Id";
                ddlReason.DataBind();
            }
            ddlReason.Items.Insert(0, new RadComboBoxItem(" Select ", "0"));
            ddlReason.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
        }
    }

    protected void ddlReason_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        txtCancelRemarks.Text = "";
        if (common.myStr(ddlReason.SelectedItem.Text).Trim().ToUpper().Equals("SELECT"))
        {
            txtCancelRemarks.Text = "";
            txtCancelRemarks.ReadOnly = true;
        }
        else if (common.myStr(ddlReason.SelectedItem.Text).Trim().ToUpper().Equals("OTHER"))
        {
            txtCancelRemarks.ReadOnly = false;
        }
        else if (common.myStr(ddlReason.SelectedItem.Text).Trim().ToUpper() != ("OTHER") && common.myStr(ddlReason.SelectedItem.Text) != "")
        {
            txtCancelRemarks.Text = "";
            txtCancelRemarks.ReadOnly = false;
        }
        else
        {
            txtCancelRemarks.Text = common.myStr(ddlReason.SelectedItem.Text).Trim();
            txtCancelRemarks.ReadOnly = true;
        }
    }

    private void BindGroupTaggingMenu()
    {
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                common.myInt(Session["ModuleId"]), "OTSCHEDULER");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                SchedulerAppointmentContextMenu.DataSource = ds.Tables[0];
                SchedulerAppointmentContextMenu.DataTextField = "OptionName";
                SchedulerAppointmentContextMenu.DataValueField = "OptionCode";
                SchedulerAppointmentContextMenu.DataBind();
            }

            ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
               common.myInt(Session["ModuleId"]), "Blank_OTAppointment");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                SchedulerTimeSlotContextMenu.DataSource = ds.Tables[0];
                SchedulerTimeSlotContextMenu.DataTextField = "OptionName";
                SchedulerTimeSlotContextMenu.DataValueField = "OptionCode";
                SchedulerTimeSlotContextMenu.DataBind();
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

    private void populatColorList()
    {
        try
        {
            RadComboBoxItem lst;
            RadComboBoxItem lstStatusColor;
            DataSet ds = new DataSet();
            Hashtable HashIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet Colordt = new DataSet();

            //  Colordt = dl.FillDataSet(CommandType.Text, "select Status,StatusColor, StatusId from GetStatus(" + Session["HospitalLocationId"] + ",'OT') order by SequenceNo asc");
            Colordt = objwcfOt.PopulatColorList(common.myInt(Session["HospitalLocationId"]));
            if (Colordt.Tables.Count > 0)
            {
                for (int i = 0; i < Colordt.Tables[0].Rows.Count; i++)
                {
                    lst = new RadComboBoxItem();
                    lstStatusColor = new RadComboBoxItem();
                    lst.Attributes.Add("style", "background-color:" + Colordt.Tables[0].Rows[i].ItemArray[1].ToString() + ";");
                    lst.Text = Colordt.Tables[0].Rows[i].ItemArray[0].ToString();
                    lstStatusColor.Text = Colordt.Tables[0].Rows[i].ItemArray[1].ToString();
                    lst.Value = Colordt.Tables[0].Rows[i].ItemArray[2].ToString();
                    lstStatusColor.Value = Colordt.Tables[0].Rows[i].ItemArray[0].ToString();
                    ddlColor.Items.Add(lst);
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
    protected void ddlPatient_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = sender as RadComboBox;
        GridViewRow row = ddl.NamingContainer as GridViewRow;
        //if (e.Text != "")
        //{
        DataTable data = GetSearchedData(e.Text);

        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;
        try
        {
            ddlPatient.Focus();
            ddlPatient.ClearSelection();
            ddlPatient.Items.Clear();
            ddlPatient.Text = string.Empty;
            ddlPatient.SelectedValue = null;

            for (int i = itemOffset; i < endOffset; i++)
            {
                //ddl.Items.Add(new RadComboBoxItem(data.Rows[i]["ServiceName"].ToString(), data.Rows[i]["ServiceId"].ToString()));
                RadComboBoxItem item = new RadComboBoxItem();

                switch ((string)data.Rows[i]["SearchedColumn"])
                {
                    case "Specialisation":
                        {
                            item.Text = "Specialisation : " + (string)data.Rows[i]["Specialisation"];
                            item.Value = data.Rows[i]["SpecialisationID"].ToString();
                            break;
                        }
                    case "Doctor":
                        {

                            item.Text = "Doctor :" + (string)data.Rows[i]["Doctor"];
                            item.Value = data.Rows[i]["DoctorID"].ToString();
                            break;
                        }
                    case "Surgery":
                        {
                            item.Text = "Surgery :" + (string)data.Rows[i]["Surgery"];
                            item.Value = data.Rows[i]["SurgeryID"].ToString();
                            break;
                        }
                    default:
                        {
                            item.Text = "Patient :" + (string)data.Rows[i]["PatientName"];
                            item.Value = data.Rows[i]["RegistrationNo"].ToString();
                            break;
                        }
                }


                item.Attributes.Add("Account", data.Rows[i]["RegistrationNo"].ToString());
                item.Attributes.Add("Specialisation", data.Rows[i]["Specialisation"].ToString());
                item.Attributes.Add("SpecialisationID", data.Rows[i]["SpecialisationID"].ToString());

                item.Attributes.Add("Doctor", data.Rows[i]["Doctor"].ToString());
                item.Attributes.Add("DoctorID", data.Rows[i]["DoctorID"].ToString());

                item.Attributes.Add("PatientName", data.Rows[i]["PatientName"].ToString());
                string surgery = data.Rows[i]["Surgery"].ToString();
                if (surgery.Length > 20)
                {
                    surgery = surgery.Substring(0, 20) + "...";
                }
                item.Attributes.Add("Surgery", surgery);
                item.Attributes.Add("SurgeryID", data.Rows[i]["SurgeryID"].ToString());

                item.Attributes.Add("SearchedColumn", data.Rows[i]["SearchedColumn"].ToString());

                this.ddlPatient.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            data.Dispose();
        }
        //}
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches found";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    private DataTable GetSearchedData(string text)
    {
        DataSet ds = new DataSet();
        DataTable data = new DataTable();
        try
        {
            String sKeyword = text;
            BaseC.clsOTBooking otb = new BaseC.clsOTBooking(sConString);

            ds = otb.SearchOTrelatedPatientByName(Convert.ToInt16(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), sKeyword, common.myDate(dtpDate.SelectedDate));

            data = new DataTable();
            if (ds.Tables.Count > 0)
                data = ds.Tables[0];

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return data;
    }
    protected void chkOptionView_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkOptionView.Checked == true)
            {
                RadScheduler1.FirstDayOfWeek = DayOfWeek.Monday;
                RadScheduler1.LastDayOfWeek = DayOfWeek.Friday;
            }
            else
            {
                RadScheduler1.FirstDayOfWeek = DayOfWeek.Sunday;
                RadScheduler1.LastDayOfWeek = DayOfWeek.Saturday;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    DataSet GetOTStatus()
    {
        DataSet Reasondt = new DataSet();

        try
        {
            Hashtable HashIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            // Reasondt = dl.FillDataSet(CommandType.Text, "select StatusId,Status from GetStatus(" + Session["HospitalLocationId"] + ",'OT') order by SequenceNo asc");
            Reasondt = objwcfOt.PopulatColorList(common.myInt(Session["HospitalLocationId"]));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return Reasondt;
    }
    protected void RadScheduler1_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
    {
        WebControl rsAptInner = (WebControl)e.Container.Parent;
        rsAptInner.Style["background"] = e.Appointment.Attributes["StatusColor"];
    }

    protected void gvLegend_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label colorstatus = new Label();
                colorstatus = (Label)e.Row.FindControl("ColorStatus");
                colorstatus.BackColor = System.Drawing.ColorTranslator.FromHtml(colorstatus.Text);
                e.Row.Cells[1].BackColor = System.Drawing.ColorTranslator.FromHtml(colorstatus.Text);
                colorstatus.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void RadScheduler1_TimeSlotCreated(object sender, Telerik.Web.UI.TimeSlotCreatedEventArgs e)
    {
        RadScheduler scheduler = (RadScheduler)sender;

        //if (scheduler.SelectedView == SchedulerViewType.DayView || scheduler.SelectedView == SchedulerViewType.MonthView || scheduler.SelectedView == SchedulerViewType.WeekView)
        //{
        //    e.TimeSlot.CssClass += "Disabled";
        //}
        //else
        //{
        try
        {
            int count = e.TimeSlot.Appointments.Count;

            if (count >= 1)
            {
                e.TimeSlot.CssClass += "Disabled";
            }
            else
            {
                e.TimeSlot.CssClass += "Enabled";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //}
    }

    protected void RadScheduler1_AppointmentDataBound(object sender, SchedulerEventArgs e)
    {
        try
        {
            e.Appointment.ToolTip = e.Appointment.Attributes["TooTipText"];

            e.Appointment.ToolTip = e.Appointment.Attributes["TooTipText"];

            if (common.myStr(e.Appointment.Attributes["Type"]).Equals("Break"))
            {
                e.Appointment.ContextMenuID = "BreakContextMenu";
            }
            else
            {
                e.Appointment.ContextMenuID = "SchedulerAppointmentContextMenu";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    protected void btnToday_Click(object sender, EventArgs e)
    {

        dtpDate.SelectedDate = System.DateTime.Now;
        //ViewState["TodayDate"]= dtpDate.SelectedDate.Value.ToString("yyyy/MM/dd");
        btnRefresh_OnClick(sender, e);
    }

    protected void ddlFacility_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            PopulateOTName();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnRefresh_OnClick(Object sender, EventArgs e)
    {
        try
        {


            DataSet ds = new DataSet();
            Hashtable HashIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder xmlTheaterIds = new StringBuilder();
            string strTheaterIds = common.GetCheckedItems(ddlTheater);
            string[] arTheaterIds = strTheaterIds.Split(',');
            string searchColumn = ddlPatient.Text;
            string Searchkeyworkdvalue = "0";

            int intRegistrationId = 0;
            if (searchColumn.Trim() != "")
            {
                Searchkeyworkdvalue = ddlPatient.SelectedValue;

                DataTable tblPatient = GetSearchedData(string.Empty);
                DataView dvPatient = tblPatient.DefaultView;

                try
                {
                    if (tblPatient != null)
                    {
                        switch (searchColumn.Split(':')[0].ToString().Trim())
                        {
                            case "Specialisation":
                                {
                                    dvPatient.RowFilter = "SpecialisationID=" + common.myInt(ddlPatient.SelectedValue);
                                    break;
                                }
                            case "Doctor":
                                {
                                    dvPatient.RowFilter = "DoctorID=" + common.myInt(ddlPatient.SelectedValue);

                                    break;
                                }
                            case "Surgery":
                                {
                                    dvPatient.RowFilter = "SurgeryID=" + common.myInt(ddlPatient.SelectedValue);
                                    break;
                                }
                            default:
                                {
                                    dvPatient.RowFilter = "RegistrationNo=" + common.myInt(ddlPatient.SelectedValue);
                                    break;
                                }
                        }

                        if (dvPatient.ToTable().Rows.Count > 0)
                        {
                            intRegistrationId = common.myInt(dvPatient.ToTable().Rows[0]["RegistrationId"]);
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    tblPatient.Dispose();
                    dvPatient.Dispose();
                }
            }
            ArrayList col1 = new ArrayList();
            foreach (string Id in arTheaterIds)
            {
                if (Id != "")
                {
                    col1.Add(common.myInt(Id)); // TheaterId
                    xmlTheaterIds.Append(common.setXmlTable(ref col1));
                }
            }

            //String value = String.Empty;
            //CheckBox checkbox = new CheckBox();
            //foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
            //{
            //    value = currentItem.Value;
            //    checkbox = (CheckBox)currentItem.FindControl("chk1");
            //    if (checkbox.Checked == true)
            //    {
            //        strProvider = strProvider + "<Table1><c1>" + value + "</c1></Table1>";
            //    }
            //}

            if (xmlTheaterIds.ToString() == "")
            {
                lablecraete.Visible = true;
                btnlink.Visible = true;
                RadScheduler1.DataSource = null;
                RadScheduler1.DataBind();
                return;
            }
            else
            {
                lablecraete.Visible = false;
                btnlink.Visible = false;
            }
            HashIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            if (ddlFacility.SelectedValue == "0")
            {
                HashIn.Add("@intFacilityId", "0");

            }
            else
            {
                HashIn.Add("@intFacilityId", ddlFacility.SelectedValue);
            }
            //HashIn.Add("@xmlOTTheaterIds", xmlTheaterIds.ToString());
            //HashIn.Add("@chrForDate", dtpDate.SelectedDate.Value.ToString("yyyy/MM/dd"));
            //HashIn.Add("@chrDateOption", ViewState["CurrentView"] == null ? "D" : ViewState["CurrentView"]);
            //HashIn.Add("@intStatusId", ddlColor.SelectedValue);
            //HashIn.Add("@filteron", searchColumn.Split(':')[0].ToString());
            //HashIn.Add("@filtervalue", Searchkeyworkdvalue);

            //  ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetOTSchedule", HashIn);

            BaseC.clsOTBooking otb = new BaseC.clsOTBooking(sConString);
            ds = otb.GetOTScheduler(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue), xmlTheaterIds.ToString(),
                                common.myDate(dtpDate.SelectedDate), common.myStr(ViewState["CurrentView"]), common.myInt(ddlColor.SelectedValue),
                                searchColumn.Split(':')[0].ToString().Trim(), common.myInt(Searchkeyworkdvalue).ToString(), intRegistrationId);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Session["printds"] = ds.Tables[0];
                RadScheduler1.SelectedDate = dtpDate.SelectedDate.Value;
                RadScheduler1.WorkDayStartTime = TimeSpan.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
                RadScheduler1.WorkDayEndTime = TimeSpan.Parse(ds.Tables[0].Rows[0].ItemArray[1].ToString());

                RadScheduler1.DayStartTime = TimeSpan.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
                RadScheduler1.DayEndTime = TimeSpan.Parse(ds.Tables[0].Rows[0].ItemArray[1].ToString());

                if (common.myInt(ds.Tables[0].Rows[0].ItemArray[2].ToString()) > 0)
                {
                    RadScheduler1.MinutesPerRow = common.myInt(ds.Tables[0].Rows[0].ItemArray[2].ToString());
                    RadScheduler1.TimeLabelRowSpan = 30 / common.myInt(ds.Tables[0].Rows[0].ItemArray[2].ToString());
                }
                else
                {
                    RadScheduler1.MinutesPerRow = 10;
                    RadScheduler1.TimeLabelRowSpan = 30 / 10;
                }

                RadScheduler1.DataKeyField = "AppointmentId";
                RadScheduler1.DataStartField = "FromTime";
                RadScheduler1.DataEndField = "ToTime";
                RadScheduler1.DataSubjectField = "Subject";



                RadScheduler1.GroupBy = "Resource";
                //RadScheduler1.DataRecurrenceField = "RecurrenceRule";
                //RadScheduler1.DataRecurrenceParentKeyField = "RecurrenceParentId";
                RadScheduler1.ResourceTypes.Clear();
                ResourceType rt = new ResourceType("Resource");
                rt.DataSource = ds.Tables[2];
                rt.KeyField = "ResourceID";
                if (ddlFacility.SelectedValue == "0")
                {
                    rt.ForeignKeyField = "FacilityId";
                }
                else
                {
                    rt.ForeignKeyField = "TheaterId";
                }
                rt.TextField = "ResourceName";
                RadScheduler1.ResourceTypes.Add(rt);
                Session["Printapp"] = ds.Tables[1];
                RadScheduler1.DataSource = ds.Tables[1];
                RadScheduler1.DataBind();
            }
            else
            {
                Alert.ShowAjaxMsg("There is no time define in selected provider and facility.", Page);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void PopulateOTName()
    {
        try
        {
            DataSet ds1 = new DataSet();
            DataSet ds = new DataSet();
            ds1 = objwcfOt.GetEmployeeId(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue));
            int employeeId = 0;
            string empid = null;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {

                empid = dr["Empid"].ToString();
            }
            employeeId = common.myInt(empid);
            if (employeeId == 0)
            {

                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);




                //Hashtable HashIn = new Hashtable();
                //HashIn.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                //HashIn.Add("@intFacilityID", ddlFacility.SelectedValue);
                //ds= dl.FillDataSet(CommandType.StoredProcedure, "uspGetOTs", HashIn);

                ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue));

                if (ds.Tables.Count > 0)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    //  dv.RowFilter = "Active=1";

                    ddlTheater.DataSource = dv.ToTable();
                    ddlTheater.DataTextField = "TheatreName";
                    ddlTheater.DataValueField = "TheatreID";
                    ddlTheater.DataBind();
                    common.CheckAllItems(ddlTheater);


                    ddlAnotherOT.DataSource = dv.ToTable();
                    ddlAnotherOT.DataTextField = "TheatreName";
                    ddlAnotherOT.DataValueField = "TheatreID";
                    ddlAnotherOT.DataBind();
                    ddlAnotherOT.Items.Insert(0, new RadComboBoxItem("---Select OT---", "0"));
                    ddlAnotherOT.SelectedValue = "0";


                }
            }
            else
            {



                ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue), employeeId);

                if (ds.Tables.Count > 0)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    // dv.RowFilter = "Active=1";

                    ddlTheater.DataSource = dv.ToTable();
                    ddlTheater.DataTextField = "TheatreName";
                    ddlTheater.DataValueField = "TheatreID";
                    ddlTheater.DataBind();
                    common.CheckAllItems(ddlTheater);

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

    private void PopulateFacility()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = objwcfcm.GetFacilityList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]), common.myInt(Session["GroupID"]), common.myInt(Session["UserID"]));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlFacility.DataSource = ds;
                ddlFacility.DataTextField = "FacilityName";
                ddlFacility.DataValueField = "FacilityID";
                ddlFacility.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private string FindCurrentDate(string outputCurrentDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string firstCurrentDate = "";
        string newCurrentDate = "";
        try
        {

            string currentdate = formatdate.FormatDateDateMonthYear(System.DateTime.Today.ToShortDateString());
            string strformatCurrDate = formatdate.FormatDate(currentdate, "dd/MM/yyyy", "yyyy/MM/dd");
            firstCurrentDate = strformatCurrDate.Remove(4, 1);
            newCurrentDate = firstCurrentDate.Remove(6, 1);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return newCurrentDate;
    }

    private string FindFutureDate(string outputFutureDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);

        string NewApptDate = "";
        try
        {
            string strDate = Convert.ToDateTime(outputFutureDate).Date.ToShortDateString();
            string firstApptDate = "";
            string strDateAppointment = formatdate.FormatDateDateMonthYear(strDate);
            string strformatApptDate = formatdate.FormatDate(strDateAppointment, "dd/MM/yyyy", "yyyy/MM/dd");
            firstApptDate = strformatApptDate.Remove(4, 1);
            NewApptDate = firstApptDate.Remove(6, 1);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return NewApptDate;
    }

    protected void RadScheduler1_TimeSlotContextMenuItemClicked(object sender, TimeSlotContextMenuItemClickedEventArgs e)
    {
        BaseC.clsOTBooking objB = new BaseC.clsOTBooking(sConString);
        BaseC.HospitalSetup baseHs = new BaseC.HospitalSetup(sConString);
        try
        {
            if (common.myInt(FindFutureDate(common.myStr(e.TimeSlot.Start.ToString("yyyy/MM/dd")))) < common.myInt(FindCurrentDate(common.myStr(System.DateTime.Today))))
            {
                Alert.ShowAjaxMsg("Warning, Back date/time appointment not allowed.", Page);
                return;
            }

            if (baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "InSameDayBackTimeshouldRequiredforOTBooking", common.myInt(Session["FacilityId"])).Equals("N"))
            {
                if (common.myInt(FindFutureDate(common.myStr(e.TimeSlot.Start.ToString("yyyy/MM/dd")))) == common.myInt(FindCurrentDate(common.myStr(System.DateTime.Today))))
                {
                    if (e.TimeSlot.Start.Hour < DateTime.Now.Hour)
                    {
                        Alert.ShowAjaxMsg("Warning, Back time appointment not allowed.", Page);
                        return;
                    }
                }
            }

            if (e.MenuItem.Value == "OTBOOK")
            {
                int iCheck = objB.ExistOTBooking(common.myInt(ddlFacility.SelectedValue), common.myInt(common.myInt(e.TimeSlot.Resource.Key)),
                                    Convert.ToDateTime(dtpDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                    (e.TimeSlot.Start.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Hour.ToString() : e.TimeSlot.Start.Hour.ToString()) + ":" + (e.TimeSlot.Start.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Minute.ToString() : e.TimeSlot.Start.Minute.ToString()),
                                    (e.TimeSlot.End.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.End.Hour.ToString() : e.TimeSlot.End.Hour.ToString()) + ":" + (e.TimeSlot.End.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.End.Minute.ToString() : e.TimeSlot.End.Minute.ToString()));

                if (iCheck > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    Alert.ShowAjaxMsg("Already set OT Booking in this time.", this.Page);
                    lblMessage.Text = "Already set OT Booking in this time.";
                    return;
                }

                BaseC.Security baseUs = new BaseC.Security(sConString);
                bool IsAuthorizedForOTBookingAfterFreezeTime = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTBookingAfterFreezeTime");

                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                string OTFzTime = "select isnull(DatePart(HH,OTFreezeTime),'') as OTFreezeTime from OTTheatreMaster where Theatreid=" + common.myInt(common.myInt(e.TimeSlot.Resource.Key)) + " And FacilityId=" + common.myInt(Session["FacilityId"]) + " And Active=1";
                int OTFreezeTime = common.myInt(dl.ExecuteScalar(CommandType.Text, OTFzTime));

                if (OTFreezeTime > 0)
                {
                    if (e.TimeSlot.Start.Hour > OTFreezeTime || e.TimeSlot.End.Hour > OTFreezeTime)
                    {
                        if (!IsAuthorizedForOTBookingAfterFreezeTime)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                            Alert.ShowAjaxMsg("Not Authorized For OTBooking at this time.", this.Page);
                            lblMessage.Text = "Not Authorized For OTBooking at this time.";
                            return;
                        }
                    }
                }
                BaseC.clsOTBooking objOTP = new BaseC.clsOTBooking(sConString);
                string OTBookDate = e.TimeSlot.Start.ToString("yyyy/MM/dd");
                string AppTimeHr = e.TimeSlot.Start.Hour.ToString();
                string AppTimeMin = e.TimeSlot.Start.Minute.ToString();
                int Theaterid = common.myInt(e.TimeSlot.Resource.Key);
                int SlotDuration = RadScheduler1.MinutesPerRow;
                int AppTimeinMinute = (common.myInt(AppTimeHr) * 60) + common.myInt(AppTimeMin);

                Hashtable htOut = objOTP.CheckOTBookingSlot(common.myInt(AppTimeinMinute), Theaterid, OTBookDate, SlotDuration, common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

                if (common.myStr(htOut["@chvErrorStatus"]).Contains("Previous") || common.myStr(htOut["@chvErrorStatus"]).Contains("Available"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    Alert.ShowAjaxMsg(common.myStr(htOut["@chvErrorStatus"]), this.Page);
                    return;
                }

                string navstr = "";
                if (ddlFacility.SelectedValue == "0")
                {
                    if (e.TimeSlot.Start.Hour == RadScheduler1.DayEndTime.Hours)
                    {
                        navstr = "~/OTScheduler/OTBooking.aspx?FacilityId=" + e.TimeSlot.Resource.Key + "&StTime=" + e.TimeSlot.Start.Hour.ToString() + "&EndTime=" + (RadScheduler1.DayEndTime.Hours + 1).ToString() + "&OTDate=" + e.TimeSlot.Start.ToString("MM/dd/yyyy") + "&OTID=" + ddlTheater.SelectedValue + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&FromTimeHour=" + e.TimeSlot.Start.Hour.ToString() + "&FromTimeMin=" + e.TimeSlot.Start.Minute.ToString() + "&PageId=" + Request.QueryString["Mpg"];
                    }
                    else
                    {
                        navstr = "~/OTScheduler/OTBooking.aspx?FacilityId=" + e.TimeSlot.Resource.Key + "&StTime=" + e.TimeSlot.Start.Hour.ToString() + "&EndTime=" + RadScheduler1.DayEndTime.Hours.ToString() + "&OTDate=" + e.TimeSlot.Start.ToString("MM/dd/yyyy") + "&OTID=" + ddlTheater.SelectedValue + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&FromTimeHour=" + e.TimeSlot.Start.Hour.ToString() + "&FromTimeMin=" + e.TimeSlot.Start.Minute.ToString() + "&PageId=" + Request.QueryString["Mpg"];
                    }
                }
                else
                {
                    if (e.TimeSlot.Start.Hour == RadScheduler1.DayEndTime.Hours)
                    {
                        navstr = "~/OTScheduler/OTBooking.aspx?FacilityId=" + ddlFacility.SelectedValue + "&StTime=" + e.TimeSlot.Start.Hour.ToString() + "&EndTime=" + (RadScheduler1.DayEndTime.Hours + 1).ToString() + "&OTDate=" + e.TimeSlot.Start.ToString("MM/dd/yyyy") + "&OTID=" + e.TimeSlot.Resource.Key + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&FromTimeHour=" + e.TimeSlot.Start.Hour.ToString() + "&FromTimeMin=" + e.TimeSlot.Start.Minute.ToString() + "&PageId=" + Request.QueryString["Mpg"];
                    }
                    else
                    {
                        navstr = "~/OTScheduler/OTBooking.aspx?FacilityId=" + ddlFacility.SelectedValue + "&StTime=" + e.TimeSlot.Start.Hour.ToString() + "&EndTime=" + RadScheduler1.DayEndTime.Hours.ToString() + "&OTDate=" + e.TimeSlot.Start.ToString("MM/dd/yyyy") + "&OTID=" + e.TimeSlot.Resource.Key + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&FromTimeHour=" + e.TimeSlot.Start.Hour.ToString() + "&FromTimeMin=" + e.TimeSlot.Start.Minute.ToString() + "&PageId=" + Request.QueryString["Mpg"];
                    }
                }

                RadWindowForNew.NavigateUrl = navstr;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }

            else if (e.MenuItem.Value == "Paste")
            {
                if (Convert.ToString(ViewState["CopyAppId"]) != "")
                {

                    string BookingDate = e.TimeSlot.Start.ToString();
                    string FromTime = e.TimeSlot.Start.Hour.ToString() + ":" + e.TimeSlot.Start.Minute.ToString();
                    int Theaterid = common.myInt(e.TimeSlot.Resource.Key);
                    int OTBookingId = common.myInt(ViewState["CopyAppId"]);
                    if (common.myStr(ViewState["MenuOption"]) == "Copy")
                    {
                        Hashtable htOut = objwcfOt.CopyOTBooking(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(OTBookingId), BookingDate, FromTime, Theaterid, common.myInt(Session["UserID"]));

                        if (common.myStr(htOut["chvErrorStatus"]).Contains("Saved"))
                        {
                            btnRefresh_OnClick(sender, e);
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = common.myStr(htOut["chvErrorStatus"]);
                        }
                    }
                    else if (common.myStr(ViewState["MenuOption"]) == "Cut")
                    {
                        BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
                        Hashtable hsOut = objOT.UpdateOTBookingTheater(OTBookingId, Theaterid, common.myInt(Session["UserId"]), FromTime, Convert.ToDateTime(BookingDate).ToString("yyyy/MM/dd"), common.myInt(Session["FacilityId"]));
                        if (common.myStr(hsOut["cErrorStatus"]).Contains("Paste Successfully!"))
                        {
                            btnRefresh_OnClick(sender, e);
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = common.myStr(hsOut["cErrorStatus"]);
                        }
                    }
                }
            }
            else if (e.MenuItem.Value == "Break")
            {
                if (common.myInt(e.TimeSlot.Resource.Key) > 0)
                {

                    int BreakId = objB.ExistOTBreak(common.myInt(ddlFacility.SelectedValue), common.myInt(common.myInt(e.TimeSlot.Resource.Key)),
                                    Convert.ToDateTime(dtpDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                    (e.TimeSlot.Start.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Hour.ToString() : e.TimeSlot.Start.Hour.ToString()) + ":" + (e.TimeSlot.Start.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Minute.ToString() : e.TimeSlot.Start.Minute.ToString()),
                                    (e.TimeSlot.End.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.End.Hour.ToString() : e.TimeSlot.End.Hour.ToString()) + ":" + (e.TimeSlot.End.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.End.Minute.ToString() : e.TimeSlot.End.Minute.ToString()));

                    if (BreakId > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        Alert.ShowAjaxMsg("Already set Break in this time.", this.Page);
                        lblMessage.Text = "Already set Break in this time.";
                        return;
                    }

                    RadWindowForNew.NavigateUrl = "~/Appointment/BreakAndBlock.aspx?" +
                                                    "Category=PopUp" +
                                                    "&FacilityId=" + common.myInt(ddlFacility.SelectedValue) +
                                                    "&StTime=" + e.TimeSlot.Start.Hour.ToString() +
                                                    "&EndTime=" + RadScheduler1.DayEndTime.Hours.ToString() +
                                                    "&appDate=" + e.TimeSlot.Start.ToString("MM/dd/yyyy") +
                                                    "&appid=0&DoctorId=0" +
                                                    "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() +
                                                    "&FromTimeHour=" + e.TimeSlot.Start.Hour.ToString() +
                                                    "&FromTimeMin=" + e.TimeSlot.Start.Minute.ToString() +
                                                    "&PageId=" + common.myStr(Request.QueryString["Mpg"]) +
                                                    "&recrule=" + null +
                                                    "&IsDoctor=" +
                                                    "&TheaterId=" + common.myInt(e.TimeSlot.Resource.Key) +
                                                    "&UseFor=OT";

                    RadWindowForNew.Height = 630;
                    RadWindowForNew.Width = 730;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void OnSelectedDateChanged_dtpDate(object sender, EventArgs e)
    {
        try
        {
            if (dtpDate.SelectedDate.Value.ToString("dd/MM/yyyy") == "01/01/0001")
            {
                dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
            }
            ViewState["SELECTED_DATE"] = dtpDate.SelectedDate;

            btnRefresh_OnClick(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void RadScheduler1_AppointmentContextMenuItemClicked(object sender, AppointmentContextMenuItemClickedEventArgs e)
    {
        ddlReason.Visible = false;
        txtCancelRemarks.ReadOnly = false;
        txtCancelRemarks.Text = String.Empty;

        //Done by ujjwal 17March2015 to change the time interval to the time interval of schedular start

        BaseC.HospitalSetup baseHs = new BaseC.HospitalSetup(sConString);
        string IsFillDefaultChekinTime = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsFillDefaultChekinTimeinScheduler", common.myInt(Session["FacilityId"]));
        if (common.myStr(IsFillDefaultChekinTime).Equals("Y"))
        {
            dtDateTimeForOT.TimeView.Interval = new TimeSpan(0, common.myInt(common.myStr(RadScheduler1.MinutesPerRow)), 0);
        }
        //Done by ujjwal 17March2015 to change the time interval to the time interval of schedular start

        btnSubmit.Enabled = true;
        txtCancelRemarks.ReadOnly = false;
        btnDeleteThisApp.Enabled = true;
        try
        {
            if (!common.myStr(e.MenuItem.Value).Equals("Delete")
                && !common.myStr(e.MenuItem.Value).Equals("OT-CANCEL")
                && common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("Delete"))
            {
                Alert.ShowAjaxMsg("Unable to change status from Delete!", this.Page);
                return;
            }
            BaseC.Patient BC = new BaseC.Patient(sConString);
            string[] strAppId = e.Appointment.ID.ToString().Split('_');
            Boolean isBreak = false;

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            if (common.myStr(e.MenuItem.Value).Equals("Edit"))
            {
                if (common.myStr(e.Appointment.Attributes["Type"]).Equals("Break"))
                {
                    RadWindowForNew.NavigateUrl = "~/Appointment/BreakAndBlock.aspx?" +
                                                    "Category=PopUp" +
                                                    "&DoctorID=0" +
                                                    "&ID=" + common.myStr(e.Appointment.ID) +
                                                    "&appDate=" + e.Appointment.Start.Date.ToString("dd/MM/yyyy") +
                                                    "&RecRule=" +
                                                    "&recparentid=" +
                                                    "&appId=" + common.myStr(e.Appointment.ID) +
                                                    "&TimeInterval=" + common.myStr(RadScheduler1.MinutesPerRow) +
                                                    "&StTime=" + common.myStr(RadScheduler1.DayStartTime.Hours) +
                                                    "&EndTime=" + common.myStr(RadScheduler1.DayEndTime.Hours) +
                                                    "&FacilityId=" + common.myInt(ddlFacility.SelectedValue) +
                                                    "&TheaterId=" + common.myInt(e.Appointment.Resources[0].Key) +
                                                    "&UseFor=OT&Edit=Edit";

                    RadWindowForNew.Height = 550;
                    RadWindowForNew.Width = 730;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                }
                else
                {
                    bool IsDoctor = false;
                    if (ViewState["IsDoctor"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["IsDoctor"];
                        DataView dvDoctor = new DataView(dt);
                        dvDoctor.RowFilter = "ResourceID='" + common.myInt(e.Appointment.Resources[0].Key) + "'";
                        DataTable dtDoctor = dvDoctor.ToTable();
                        if (dtDoctor.Rows.Count > 0)
                        {
                            IsDoctor = common.myBool(dtDoctor.Rows[0]["IsDoctor"]);
                        }
                    }

                    if (RadScheduler1.DayStartTime.Hours == RadScheduler1.DayEndTime.Hours)
                    {
                        RadWindowForNew.NavigateUrl = "~/OTScheduler/OTBooking.aspx?StTime=" + common.myStr(RadScheduler1.DayStartTime.Hours)
                                                    + "&EndTime=" + common.myStr((RadScheduler1.DayEndTime.Hours + 1))
                                                    + "&appDate=" + e.Appointment.Start.Date.ToString("MM/dd/yyyy")
                                                    + "&appid=" + common.myStr(strAppId[0])
                                                    + "&OTID=" + common.myStr(e.Appointment.Resources[0].Key)
                                                    + "&TimeInterval=" + common.myStr(RadScheduler1.MinutesPerRow)
                                                    + "&AppId1=" + common.myStr(e.Appointment.ID)
                                                    + "&PageId=3"
                                                    + "&IsDoctor=" + IsDoctor
                                                    + "&FacilityId=" + common.myStr(ddlFacility.SelectedValue);
                    }
                    else
                    {
                        RadWindowForNew.NavigateUrl = "~/OTScheduler/OTBooking.aspx?StTime=" + common.myStr(RadScheduler1.DayStartTime.Hours)
                                                + "&EndTime=" + common.myStr(RadScheduler1.DayEndTime.Hours)
                                                + "&appDate=" + e.Appointment.Start.Date.ToString("MM/dd/yyyy")
                                                + "&appid=" + common.myStr(strAppId[0])
                                                + "&OTID=" + common.myStr(e.Appointment.Resources[0].Key)
                                                + "&TimeInterval=" + common.myStr(RadScheduler1.MinutesPerRow)
                                                + "&AppId1=" + common.myStr(e.Appointment.ID)
                                                + "&PageId=3"
                                                + "&IsDoctor=" + IsDoctor
                                                + "&FacilityId=" + common.myStr(ddlFacility.SelectedValue);
                    }

                    RadWindowForNew.Height = 630;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            else if (common.myStr(e.MenuItem.Value).Equals("Print"))
            {

                RadWindowForNew.NavigateUrl = "/EMRReports/AppointmentSlip.aspx?RegistrationId=" + common.myInt(e.Appointment.Attributes["RegistrationId"]) + "&AppointmentId=" + common.myInt(strAppId[0]);
                RadWindowForNew.Height = 580;
                RadWindowForNew.Width = 750;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("RA"))
            {
                string mode = "N";
                if (common.myInt(e.Appointment.Attributes["RegistrationNo"]) > 0)
                {
                    mode = "E";
                    Alert.ShowAjaxMsg("Patient already register !", this.Page);
                    return;
                }

                RadWindowForNew.NavigateUrl = "/Pregistration/Demographics.aspx?RNo=" + common.myInt(e.Appointment.Attributes["RegistrationNo"]) + "&OTBookingId=" + common.myStr(strAppId[0]) + "&Category=PopUp&mode=" + mode;
                RadWindowForNew.Height = 580;
                RadWindowForNew.Width = 750;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("ConForm"))
            {

                RadWindowForNew.NavigateUrl = "~/EMR/Templates/TemplateNotesPrint.aspx?RegistrationNo=" + common.myInt(e.Appointment.Attributes["RegistrationNo"]) + "&OT=" + common.myInt(2) + "&RegistrationId=" + common.myStr(e.Appointment.Attributes["RegistrationId"]); // Add RegistrationId Akshay_19072022_Tirathram
                RadWindowForNew.Height = 580;
                RadWindowForNew.Width = 750;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("RA"))
            {
                string mode = "N";
                if (common.myInt(e.Appointment.Attributes["RegistrationNo"]) > 0)
                {
                    mode = "E";
                    Alert.ShowAjaxMsg("Patient already register !", this.Page);
                    return;
                }

                RadWindowForNew.NavigateUrl = "/Pregistration/Demographics.aspx?RNo=" + common.myInt(e.Appointment.Attributes["RegistrationNo"]) + "&OTBookingId=" + common.myStr(strAppId[0]) + "&Category=PopUp&mode=" + mode;
                RadWindowForNew.Height = 580;
                RadWindowForNew.Width = 750;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("ATT"))
            {

                if (common.myInt(e.Appointment.Attributes["RegistrationNo"]) <= 0)
                {
                    Alert.ShowAjaxMsg("Please Register Patient !", this.Page);
                    return;
                }

                RadWindowForNew.NavigateUrl = "/emr/AttachDocumentFTP.aspx?Category=PopUp&RNo=" + common.myInt(e.Appointment.Attributes["RegistrationNo"]);
                RadWindowForNew.Height = 580;
                RadWindowForNew.Width = 750;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("Post"))
            {
                int iCompanyID = 0;
                int BillClearance = 0;
                string OPIP = "O";
                string EditPostedOTSurgery = "N";
                int RegId = 0;
                int EncId = 0;
                int InsuranceId = 0;
                int CardId = 0;
                int iCurrentBedCategory = 0;
                int isPackage = 0;
                int IsPackageSurgery = 0;
                int OrderId = common.myInt(e.Appointment.Attributes["OrderId"]);
                string RegNo = string.Empty;
                string EncounterNo = string.Empty;
                int DoctorId = 0;


                bool IsEmergency = common.myBool(e.Appointment.Attributes["IsEmergency"]);
                BaseC.Security baseUs = new BaseC.Security(sConString);
                bool IsAuthorizedForEditPostedOTSurgery = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForEditPostedOTSurgery");

                if (OrderId > 0)
                {
                    if (!IsAuthorizedForEditPostedOTSurgery)
                    {
                        Alert.ShowAjaxMsg("Surgery already posted !", this.Page);
                        return;
                    }
                    else if (IsAuthorizedForEditPostedOTSurgery)
                    {
                        EditPostedOTSurgery = "Y";
                    }
                }
                #region comment
                //Added on 09-02-2014 Start Naushad Ali Comment as logic changed
                //if (e.Appointment.Attributes["StatusCode"].ToString().Trim() != "OT-CHKIN")
                //{
                //    Alert.ShowAjaxMsg("Please Check-in, Posting cannot be done !", this.Page);
                //    return;
                //}
                //Added on 09-02-2014 End Naushad Ali


                // BaseC.ATD objATD = new BaseC.ATD(sConString);
                //// objATD.

                // DataSet dsCheck = objATD.CheckOTChecklist(common.myInt(strAppId[0]));
                // if (dsCheck.Tables.Count == 1)
                // {
                //     if (dsCheck.Tables[0].Rows.Count == 0)
                //     {
                //         Session["RegistrationId"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                //         Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);
                //         lblMessage.Text = "";
                //         // RadWindowForNew.NavigateUrl = "~/OTScheduler/OTChecklist.aspx?IpNo=" + hdnIpno.Value + "&Pname=" + hdnPatientname.Value + "&Surgery=" + hdnSurgeryname.Value.Replace("&", "@") + "&ward=" + hdnWardno.Value + "&BookingId=" + hdnBookinId.Value + "&BedId=" + hdnBedId.Value + "&SurgeryId=" + hdnSurgeryId.Value + "  ";
                //         RadWindowForNew.NavigateUrl = "~/OTScheduler/OTChecklist.aspx?BookingId=" + common.myInt(strAppId[0]);
                //         RadWindowForNew.Height = 550;
                //         RadWindowForNew.Width = 900;
                //         RadWindowForNew.Top = 40;
                //         RadWindowForNew.Left = 100;
                //         //RadWindowForNew.OnClientClose = "OnClientClose";
                //         RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                //         RadWindowForNew.Modal = true;
                //         RadWindowForNew.VisibleStatusbar = false;
                //         return;
                //     }
                // }
                #endregion

                if (common.myStr(e.Appointment.Attributes["BillClearance"]).Equals("True") || common.myStr(e.Appointment.Attributes["BillClearance"]).Equals("1"))
                {
                    BillClearance = 1;
                }
                if (BillClearance == 0 && common.myStr(ViewState["IsBillClearanceByPassForOT"]) == "N")
                {
                    Alert.ShowAjaxMsg("Bill clearance is pending, Posting cannot be done !", this.Page);
                    return;
                }
                Hashtable htIn = new Hashtable();
                RegId = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                EncId = common.myInt(e.Appointment.Attributes["EncounterId"]);
                isPackage = common.myInt(e.Appointment.Attributes["IsPackage"]);
                RegNo = common.myStr(e.Appointment.Attributes["RegistrationNo"]);
                EncounterNo = common.myStr(e.Appointment.Attributes["EncounterNo"]);
                DoctorId = common.myInt(e.Appointment.Attributes["DoctorId"]);

                if (common.myInt(RegId).Equals(0))
                {
                    Alert.ShowAjaxMsg("Patient is not registered, please register before posting !", this.Page);
                    return;
                }
                else if (common.myInt(EncId).Equals(0))
                {
                    Alert.ShowAjaxMsg("Patient encounter not found, please create encounter before posting !", this.Page);
                    return;
                }
                else
                {
                    int HospId = common.myInt(Session["HospitalLocationID"]);
                    int FacilityId = common.myInt(Session["FacilityId"]);
                    int EncodedBy = common.myInt(Session["UserId"]);
                    DataSet ds = new DataSet();
                    BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
                    ds = objCommon.GetPatientSummary(HospId, FacilityId, RegId, EncId, EncodedBy);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (common.myStr(ds.Tables[0].Rows[0]["OPIP"]).Equals("I"))
                        {
                            OPIP = "I";
                            iCompanyID = common.myInt(ds.Tables[0].Rows[0]["CompanyCode"]);
                            InsuranceId = common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"]);
                            CardId = common.myInt(ds.Tables[0].Rows[0]["CardId"]);
                            iCurrentBedCategory = common.myInt(ds.Tables[0].Rows[0]["CurrentBedCategory"]);
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Surgery posting for OP is not available right now !", this.Page);
                            return;
                        }
                    }
                    if (common.myInt(iCompanyID).Equals(0))
                    {
                        Alert.ShowAjaxMsg("Patient company and bed category not available, please check admission !", this.Page);
                        return;
                    }
                    if (common.myInt(iCurrentBedCategory).Equals(0))
                    {
                        OPIP = "O";
                    }

                    ViewState["OTBookingid"] = common.myInt(strAppId[0]);
                    BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
                    DataSet ds1 = objOT.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));

                    if (ds1.Tables.Count > 0)
                    {
                        if (common.myStr(ds1.Tables[0].Rows[0]["OTCheckintime"]).Equals(""))
                        {
                            Alert.ShowAjaxMsg("Please update OT check in time !", this.Page);
                            return;
                        }

                        if (common.myStr(ds1.Tables[0].Rows[0]["OTCheckouttime"]).Equals(""))
                        {
                            Alert.ShowAjaxMsg("Please update OT check out time !", this.Page);
                            return;
                        }
                    }
                    ds.Dispose(); ds1.Dispose(); objOT = null; objCommon = null;
                }

                BaseC.clsOTBooking objOTP = new BaseC.clsOTBooking(sConString);
                DataSet ds2 = objOTP.GetPackageSurgery(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(strAppId[0]), common.myInt(iCompanyID), common.myInt(iCurrentBedCategory));
                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    if (common.myInt(ds2.Tables[0].Rows[0]["PackageID"]).Equals(0) || common.myStr(ds2.Tables[0].Rows[0]["PackageID"]).Equals(""))
                    {
                        IsPackageSurgery = 0;
                    }
                    else
                    {
                        IsPackageSurgery = 1;
                    }
                }
                ds2.Dispose(); objOTP = null;


                if (common.myInt(isPackage).Equals(0))
                {
                    if (common.myInt(IsPackageSurgery).Equals(0))
                    {
                        RadWindowForNew.NavigateUrl = "~/EMRBILLING/Popup/AddSurgeryV1.aspx?RegNo=" + RegNo + "&RegId=" + RegId + "&OTBookingId=" + common.myInt(strAppId[0]) + "&EncId=" + EncId + "&CompanyId=" + iCompanyID + "&CardId=" + CardId + "&PayerType=0&InsuranceId=" + InsuranceId + "&OP_IP=" + OPIP + "&EditPostedOTSurgery=" + EditPostedOTSurgery + "&OrderId=" + OrderId + "&IsEmergency=" + IsEmergency + "&DoctorId=" + DoctorId + "&EncounterNo=" + EncounterNo;
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Surgery is tagged to Package, so Package Posting will be done.", this.Page);

                        RadWindowForNew.NavigateUrl = "~/EMRBILLING/Popup/AddPackagesV1.aspx?RegNo=0&RegId=" + RegId + "&OTBookingId=" + common.myInt(strAppId[0]) + "&EncId=" + EncId + "&CompanyId=" + iCompanyID + "&CardId=" + CardId + "&PayerType=0&InsuranceId=" + InsuranceId + "&OP_IP=" + OPIP + "&EditPostedOTSurgery=" + EditPostedOTSurgery + "&OrderId=" + OrderId;
                    }
                }
                else
                {
                    RadWindowForNew.NavigateUrl = "~/EMRBILLING/Popup/AddPackagesV1.aspx?RegNo=0&RegId=" + RegId + "&OTBookingId=" + common.myInt(strAppId[0]) + "&EncId=" + EncId + "&CompanyId=" + iCompanyID + "&CardId=" + CardId + "&PayerType=0&InsuranceId=" + InsuranceId + "&OP_IP=" + OPIP + "&EditPostedOTSurgery=" + EditPostedOTSurgery + "&OrderId=" + OrderId;
                }

                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;

            }
            else if (common.myStr(e.MenuItem.Value).Equals("OTC"))// OT Charge
            {
                int RegId = 0;
                int EncId = 0;
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int EncodedBy = common.myInt(Session["UserId"]);
                string OPIP = "O";
                int iCompanyID = 0;
                int InsuranceId = 0;
                int CardId = 0;
                string RegNo = "";
                string EncNo = "";
                DataSet ds = new DataSet();
                RegId = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                EncId = common.myInt(e.Appointment.Attributes["EncounterId"]);

                BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
                ds = objCommon.GetPatientSummary(HospId, FacilityId, RegId, EncId, EncodedBy);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    iCompanyID = common.myInt(ds.Tables[0].Rows[0]["CompanyCode"]);
                    InsuranceId = common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"]);
                    RegNo = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
                    EncNo = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
                    CardId = common.myInt(ds.Tables[0].Rows[0]["CardId"]);

                    OPIP = "I";
                }
                ds.Dispose();
                objCommon = null;
                RadWindowForNew.NavigateUrl = "~/OTScheduler/AddChargeTimewisOT.aspx?RegId=" + common.myStr(e.Appointment.Attributes["RegistrationId"]) + "&EncId=" + common.myStr(e.Appointment.Attributes["EncounterId"]) + "&RegNo=" + RegNo + "&EncNo=" + EncNo + "&CardId=" + CardId + "&CompanyId=" + iCompanyID + "&InsuranceId=" + InsuranceId + "&OP_IP=" + OPIP + "&OTId=" + common.myInt(strAppId[0]);
                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 900;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("Template"))
            {
                RadWindowForNew.NavigateUrl = "~/OTScheduler/OTClinicalTemplate.aspx?RegistrationID=" + common.myStr(e.Appointment.Attributes["RegistrationId"]) + "&EncounterID=" + common.myStr(e.Appointment.Attributes["EncounterId"]);
                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }

            else if (common.myStr(e.MenuItem.Value).Equals("SR"))
            {
                BaseC.WardManagement objwm = new BaseC.WardManagement();
                DataSet ds = new DataSet();
                ds = objwm.GetDoctor(common.myInt(e.Appointment.Attributes["RegistrationId"]), common.myInt(e.Appointment.Attributes["EncounterId"]));

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Session["OPIP"] = "I";
                    Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                    Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);
                    Session["DoctorID"] = common.myInt(ds.Tables[0].Rows[0]["AdmittingDoctorId"]);

                    //RadWindowForNew.NavigateUrl = "/EMRBILLING/Popup/AddSurgeryV1.aspx?Regid=" + common.myInt(Session["RegistrationID"]) +
                    //    "&RegNo=" + common.myInt(common.myInt(e.Appointment.Attributes["RegistrationNo"])) +
                    //    "&EncId=" + common.myInt(Session["EncounterId"]) +
                    //    "&EncNo=" + common.myStr(common.myInt(e.Appointment.Attributes["EncounterNo"])) +
                    //    "&OP_IP=I&FromWard=Y&CompanyId=0&InsuranceId=0&CardId=0&PayerType=&BType=";
                    RadWindowForNew.NavigateUrl = "/EMRBILLING/Popup/AddServicesV1.aspx?Regid=" + common.myInt(Session["RegistrationID"]) +
                      "&RegNo=" + common.myLong(common.myLong(e.Appointment.Attributes["RegistrationNo"])) +
                      "&EncId=" + common.myInt(Session["EncounterId"]) +
                      "&EncNo=" + common.myStr(common.myInt(e.Appointment.Attributes["EncounterNo"])) +
                      "&OP_IP=I&FromWard=Y&CompanyId=0&InsuranceId=0&CardId=0&PayerType=&BType=";

                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
                ds.Dispose(); objwm = null;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("Copy") || common.myStr(e.MenuItem.Value).Equals("Cut"))
            {

                if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CHKIN"))
                {
                    Alert.ShowAjaxMsg("Not allow this action after check-in.", Page);
                    return;
                }

                if (isBreak != true)
                {
                    ViewState["CopyAppId"] = strAppId[0];
                    ViewState["MenuOption"] = common.myStr(e.MenuItem.Value);
                }
            }
            else if (common.myStr(e.MenuItem.Value).Equals("DO"))
            {
                Session["OPIP"] = "I";
                Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);

                if (common.myInt(Session["EncounterId"]) == 0)
                {
                    Alert.ShowAjaxMsg("Please create encounter to order drug", Page);
                    return;
                }

                int iDoctorId = common.myInt(e.Appointment.Attributes["DoctorId"]);
                if (iDoctorId == 0)
                {
                    iDoctorId = common.myInt(Session["EmployeeId"]);
                }
                DataSet dsStatus = new DataSet();
                BaseC.clsPharmacy status = new BaseC.clsPharmacy(sConString);
                dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "CO", 0);

                Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                Session["PatientDetailString"] = null;
                BaseC.Patient patient = new BaseC.Patient(sConString);

                DataSet dsPatientDetails = new DataSet();

                dsPatientDetails = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                           common.myStr(e.Appointment.Attributes["RegistrationNo"]), common.myStr(e.Appointment.Attributes["EncounterNo"]),
                                           common.myInt(Session["UserId"]), 0);

                Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = dsPatientDetails;

                if (dsPatientDetails.Tables[0].Rows.Count > 0)
                {
                    Session["EncounterStatus"] = common.myStr(dsPatientDetails.Tables[0].Rows[0]["EncounterStatus"]);
                }

                if (dsStatus.Tables.Count > 0 && dsStatus.Tables[0].Rows.Count > 0)
                {
                    RadWindowForNew.NavigateUrl = "/EMR/Medication/PrescribeMedication.aspx?Regno="
                        + common.myInt(e.Appointment.Attributes["RegistrationNo"])
                        + "&Encno=" + common.myInt(e.Appointment.Attributes["EncounterNo"])
                        + "&RegId=" + common.myInt(e.Appointment.Attributes["RegistrationId"])
                        + "&EncId=" + common.myInt(e.Appointment.Attributes["EncounterId"])
                        + "&POPUP=POPUP&DRUGORDERTYPE=" + common.myStr(dsStatus.Tables[0].Rows[0]["StatusID"])
                        + "&DRUGORDERCODE=" + common.myStr(dsStatus.Tables[0].Rows[0]["Code"])
                        + "&DoctorId=" + iDoctorId + "&LOCATION=OT";

                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
                dsStatus.Dispose(); status = null;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("DGO"))
            {
                Session["OPIP"] = "I";
                Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);

                if (common.myInt(Session["EncounterId"]) == 0)
                {
                    Alert.ShowAjaxMsg("Please create encounter to order drug", Page);
                    return;
                }

                int iDoctorId = common.myInt(e.Appointment.Attributes["DoctorId"]);
                if (iDoctorId == 0)
                {
                    iDoctorId = common.myInt(Session["EmployeeId"]);
                }
                DataSet dsStatus = new DataSet();
                BaseC.clsPharmacy status = new BaseC.clsPharmacy(sConString);
                dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "CO", 0);


                Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                Session["PatientDetailString"] = null;
                BaseC.Patient patient = new BaseC.Patient(sConString);

                Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                           common.myStr(e.Appointment.Attributes["RegistrationNo"]), common.myStr(e.Appointment.Attributes["EncounterNo"]), common.myInt(Session["UserId"]), 0);



                if (dsStatus.Tables.Count > 0 && dsStatus.Tables[0].Rows.Count > 0)
                {

                    Session["EncounterDate"] = common.myDate(e.Appointment.Attributes["EncounterDate"]).ToString("dd/MM/yyyy");
                    Session["DoctorID"] = common.myInt(iDoctorId);
                    Session["RegistrationNo"] = common.myInt(e.Appointment.Attributes["RegistrationNo"]);
                    Session["Regno"] = common.myStr(e.Appointment.Attributes["RegistrationNo"]);
                    Session["Encno"] = common.myStr(e.Appointment.Attributes["EncounterNo"]);
                    Session["InvoiceId"] = 0;


                    RadWindowForNew.NavigateUrl = "/EMR/Medication/PrescribeMedicationNew.aspx?POPUP=POPUP&Regno="
                        + common.myInt(e.Appointment.Attributes["RegistrationNo"])
                        + "&Encno=" + common.myInt(e.Appointment.Attributes["EncounterNo"])
                        + "&RegId=" + common.myInt(e.Appointment.Attributes["RegistrationId"])
                        + "&EncId=" + common.myInt(e.Appointment.Attributes["EncounterId"])
                        //+ "&DRUGORDERTYPE=" + common.myStr(dsStatus.Tables[0].Rows[0]["StatusID"])
                        //+ "&DRUGORDERCODE=" + common.myStr(dsStatus.Tables[0].Rows[0]["Code"])
                        + "&DoctorId=" + iDoctorId
                        //+ "&LOCATION=OT"
                        ;

                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
                dsStatus.Dispose(); status = null;
            }
            #region Only Consumable Items
            else if (common.myStr(e.MenuItem.Value).Equals("COONLY"))
            {
                Session["OPIP"] = "I";
                Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);

                if (common.myInt(Session["EncounterId"]) == 0)
                {
                    Alert.ShowAjaxMsg("Please create encounter to order drug", Page);
                    return;
                }

                int iDoctorId = common.myInt(e.Appointment.Attributes["DoctorId"]);
                if (iDoctorId == 0)
                {
                    iDoctorId = common.myInt(Session["EmployeeId"]);
                }
                DataSet dsStatus = new DataSet();
                BaseC.clsPharmacy status = new BaseC.clsPharmacy(sConString);
                dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "CO", 0);


                Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                Session["PatientDetailString"] = null;
                BaseC.Patient patient = new BaseC.Patient(sConString);

                Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                           common.myStr(e.Appointment.Attributes["RegistrationNo"]), common.myStr(e.Appointment.Attributes["EncounterNo"]), common.myInt(Session["UserId"]), 0);



                RadWindowForNew.NavigateUrl = "/EMR/Medication/ConsumableOrder.aspx?Regno=" + common.myInt(e.Appointment.Attributes["RegistrationNo"])
                                          + "&Encno=" + common.myInt(e.Appointment.Attributes["EncounterNo"])
                        + "&RegId=" + common.myInt(e.Appointment.Attributes["RegistrationId"])
                        + "&EncId=" + common.myInt(e.Appointment.Attributes["EncounterId"]) +
                                        "&POPUP=POPUP&DRUGORDERTYPE=" + common.myStr(dsStatus.Tables[0].Rows[0]["StatusID"]) +
                                        "&DRUGORDERCODE=" + common.myStr(dsStatus.Tables[0].Rows[0]["Code"]) +
                                        "&DoctorId=" + common.myStr(iDoctorId) +
                                        "&LOCATION=WARD";
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.Height = 600;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = string.Empty;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
            #endregion
            else if (common.myStr(e.MenuItem.Value).Equals("DR"))
            {
                Session["OPIP"] = "I";
                Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);

                RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/IPDItemReturn.aspx?MASTER=No&Regno=" + common.myInt(e.Appointment.Attributes["RegistrationNo"]) + "&Encno=" + common.myStr(e.Appointment.Attributes["EncounterNo"]);
                RadWindowForNew.Height = 610;
                RadWindowForNew.Width = 940;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.OnClientClose = ""; //"SearchPatientOnClientClose";//
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("OTIT"))
            {
                BaseC.Security baseUs = new BaseC.Security(sConString);
                bool IsAuthorize = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTCheckIn ");
                bool IsAuthorizeToEdit = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForEditOTCheckInOutTime");

                if (!IsAuthorize)
                {
                    Alert.ShowAjaxMsg("You are not authozied to enter OT Check-in Time !", this.Page);
                    return;
                }

                int OrderId = common.myInt(e.Appointment.Attributes["OrderId"]);
                int BillClearance = 0;

                if (!IsAuthorizeToEdit)
                {
                    if (OrderId > 0)
                    {
                        Alert.ShowAjaxMsg("Surgery already posted !", this.Page);
                        return;
                    }
                }

                if (IsAuthorizeToEdit && OrderId > 0)
                {
                    ViewState["EditCheckIN"] = "Y";
                }
                else
                {
                    ViewState["EditCheckIN"] = "N";
                }

                if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CONF")
                        || common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-Bill"))
                {
                    Alert.ShowAjaxMsg("Please First do check in!", this.Page);
                    return;
                }

                //Added on 02-09-2014 Start Naushad
                if (common.myStr(e.Appointment.Attributes["BillClearance"]) == "True" || common.myStr(e.Appointment.Attributes["BillClearance"]) == "1")
                {
                    BillClearance = 1;
                }
                if (common.myInt(BillClearance).Equals(0) && common.myStr(ViewState["IsBillClearanceByPassForOT"]) == "N")
                {
                    Alert.ShowAjaxMsg("Bill clearance is pending, Posting cannot be done !", this.Page);
                    return;
                }

                lblMsg.Text = string.Empty;
                //bindminut();
                BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
                int x = 0;
                Hashtable ht = new Hashtable();
                DateTime dt = new DateTime();

                StringBuilder strDateTime = new StringBuilder();
                strDateTime.Append(RadScheduler1.SelectedDate);

                dt = common.myDate(RadScheduler1.SelectedDate);

                //  hdnOTBookingdate.Value = dt.ToString("yyyy/MM/dd");
                hdnOTBookingdate.Value = Convert.ToString((e.Appointment.Start.Date.AddHours(e.Appointment.Start.Hour)).AddMinutes(e.Appointment.Start.Minute));
                Session["GetDateTime"] = hdnOTBookingdate.Value;

                ht = objRest.GetIsunPlannedReturnToOt(common.myInt(e.Appointment.Attributes["RegistrationId"]), common.myInt(Session["FacilityId"]), Convert.ToDateTime(hdnOTBookingdate.Value));

                Session["myid"] = e.Appointment.Attributes["RegistrationId"];
                DataSet ds = new DataSet();

                string IsOTChargeableOT = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsUnplannedOTChargeable", common.myInt(Session["FacilityId"]));

                if (common.myBool(ht["@bitIsUnPlannedReturnToOT"]) || common.myStr(ht["@chvLastCheckoutTime"]).Trim() == "")
                {
                    if (Convert.ToInt64(strAppId[0]) > 0)
                    {
                        ViewState["OTBookingid"] = common.myInt(strAppId[0]);
                        BaseC.clsOTBooking objOTchk = new BaseC.clsOTBooking(sConString);
                        DataSet dschk = objOTchk.GetCheckITOT(common.myStr(common.myInt(strAppId[0])), common.myInt(Session["FacilityId"]));

                        if (dschk.Tables.Count > 0)
                        {
                            if ((common.myBool(dschk.Tables[0].Rows[0]["IsUnplannedReturnToOT"]) == true))
                            {
                                lblIsUnplannedSurgery.Visible = true;
                                rblIsUnplannedOT.Visible = true;
                                lblIschargeAbleOt.Visible = true;
                                rblIsChargeAbleOT.Visible = true;
                                Label6.Visible = true;
                                ddlAnotherOT.Visible = true;
                                lblUnplannedSurgeryRemarks.Visible = true;
                                txtUnplannedSurgeryRemarks.Visible = true;
                                rblIsUnplannedOT.SelectedValue = "1";
                                rblIsChargeAbleOT.SelectedValue = "1";
                            }
                            else if (common.myBool(dschk.Tables[0].Rows[0]["IsUnplannedReturnToOT"]) == false)
                            {
                                rblIsUnplannedOT.SelectedValue = "0";
                                rblIsChargeAbleOT.SelectedValue = "0";
                                if (common.myStr(ht["@chvLastCheckoutTime"]) == "")
                                {
                                    lblCheckoutTime.Style.Add("display", "none");
                                    lblLastChkOutTime.Style.Add("display", "none");
                                    lblTimeDiff.Style.Add("display", "none");
                                    lblDiffTime.Style.Add("display", "none");
                                }
                                else
                                {
                                    lblCheckoutTime.Style.Add("display", "block");
                                    lblLastChkOutTime.Style.Add("display", "block");
                                    lblTimeDiff.Style.Add("display", "block");
                                    lblDiffTime.Style.Add("display", "block");
                                }
                                hideControl();
                            }
                            else
                            {
                                lblIsUnplannedSurgery.Visible = true;
                                rblIsUnplannedOT.Visible = true;
                                lblIschargeAbleOt.Visible = true;
                                rblIsChargeAbleOT.Visible = true;
                                Label6.Visible = true;
                                ddlAnotherOT.Visible = true;
                                lblUnplannedSurgeryRemarks.Visible = true;
                                txtUnplannedSurgeryRemarks.Visible = true;
                                rblIsUnplannedOT.SelectedValue = "1";
                                rblIsChargeAbleOT.SelectedValue = "1";
                            }
                        }
                    }

                    if (common.myStr(IsOTChargeableOT).Equals("Y"))
                    {
                        if (common.myStr(ht["@chvLastCheckoutTime"]) != null)
                        {
                            lblCheckoutTime.Visible = true;
                            lblDiffTime.Visible = true;
                            lblLastChkOutTime.Text = common.myStr(ht["@chvLastCheckoutTime"]);
                            StringBuilder strbuild = new StringBuilder();
                            strbuild.Append(common.myStr(ht["@intTimeDiffInMin"]));
                            strbuild.Append("");
                            strbuild.Append("hr.");
                            lblTimeDiff.Text = strbuild.ToString();
                        }
                    }
                }
                else
                {
                    //lblIsUnplannedSurgery.Visible = false;
                    //rblIsUnplannedOT.Visible = false;
                    //lblIschargeAbleOt.Visible = false;
                    //rblIsChargeAbleOT.Visible = false;
                    //lblCheckoutTime.Visible = false;
                    //lblDiffTime.Visible = false;
                    rblIsUnplannedOT.SelectedValue = "1";
                    rblIsChargeAbleOT.SelectedValue = "1";
                    hideControl();
                }

                //Added By Ujjwal to change the date of the dtDateTimeForOT to the otBooking Time start
                if (common.myStr(IsFillDefaultChekinTime).Equals("Y"))
                {
                    dtDateTimeForOT.SelectedDate = e.Appointment.Start;
                }
                else
                {
                    dtDateTimeForOT.SelectedDate = null;
                }
                //Added By Ujjwal to change the date of the dtDateTimeForOT to the otBooking Time end

                // start -- check in and  check out Time should be taken current systems time
                string IsFillDefaultSystemChekinCheckoutTimeInOT = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsFillDefaultSystemChekinCheckoutTimeInOT", common.myInt(Session["FacilityId"]));

                if (common.myStr(IsFillDefaultSystemChekinCheckoutTimeInOT).Equals("Y"))
                {
                    dtDateTimeForOT.SelectedDate = DateTime.Now;
                }

                ///  end---

                divOTCheckinCheckoutTime.Visible = true;
                Label4.Text = "OT Check In Time";
                ViewState["inot"] = "1";
                ViewState["OTBookingid"] = common.myInt(strAppId[0]);

                BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
                DataSet ds1 = objOT.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0
                    && !string.IsNullOrEmpty(common.myStr(ds1.Tables[0].Rows[0]["OTCheckintime"])))
                {
                    dtDateTimeForOT.SelectedDate = common.myDate(ds1.Tables[0].Rows[0]["OTCheckintime"]);

                    if (!IsAuthorizeToEdit)
                    {
                        btnSubmit.Enabled = false;
                    }
                    else
                    {
                        btnSubmit.Enabled = true;
                    }

                }

                ds1.Dispose(); objOT = null;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("OTOT"))
            {
                hdnTempData.Value = "0";
                hideControl();

                BaseC.Security baseUs = new BaseC.Security(sConString);
                bool IsAuthorize = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTCheckOut");
                bool IsAuthorizeToEdit = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForEditOTCheckInOutTime ");

                if (!IsAuthorize)
                {
                    Alert.ShowAjaxMsg("You are not authozied to enter OT Check-out Time !", this.Page);
                    return;
                }
                int OrderId = common.myInt(e.Appointment.Attributes["OrderId"]);

                if (!IsAuthorizeToEdit)
                {
                    if (OrderId > 0)
                    {
                        Alert.ShowAjaxMsg("Surgery already posted !", this.Page);
                        return;
                    }
                }

                if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CONF")
                        || common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-Bill")
                        || common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-UNCONF"))
                {
                    Alert.ShowAjaxMsg("Please First do check in!", this.Page);
                    return;
                }
                else
                {
                    BaseC.clsOTBooking objOTintime = new BaseC.clsOTBooking(sConString);
                    DataSet dsintime = objOTintime.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));
                    if (dsintime != null && dsintime.Tables.Count > 0 && dsintime.Tables[0].Rows.Count > 0
                        && string.IsNullOrEmpty(common.myStr(dsintime.Tables[0].Rows[0]["OTCheckintime"])))
                    {
                        Alert.ShowAjaxMsg("Please First do check in time!", this.Page);
                        return;
                    }
                }


                lblMsg.Text = string.Empty;
                // bindminut();
                //Added By Ujjwal to change the date of the dtDateTimeForOT to the otBooking Time start
                if (common.myStr(IsFillDefaultChekinTime) == "Y")
                {
                    dtDateTimeForOT.SelectedDate = e.Appointment.End;
                }
                else
                {
                    dtDateTimeForOT.SelectedDate = null;
                }
                //Added By Ujjwal to change the date of the dtDateTimeForOT to the otBooking Time end


                // start -- check in and  check out Time should be taken current systems time
                string IsFillDefaultSystemChekinCheckoutTimeInOT = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsFillDefaultSystemChekinCheckoutTimeInOT", common.myInt(Session["FacilityId"]));

                if (common.myStr(IsFillDefaultSystemChekinCheckoutTimeInOT).Equals("Y"))
                {
                    dtDateTimeForOT.SelectedDate = DateTime.Now;
                }

                ///  end---
                ///  
                divOTCheckinCheckoutTime.Visible = true;

                Label4.Text = "OT Check Out Time";
                if (Label4.Text == "OT Check Out Time")

                {
                    lblCheckoutTime.Visible = false;
                    lblLastChkOutTime.Visible = false;
                    lblDiffTime.Visible = false;
                    lblTimeDiff.Visible = false;
                }


                ViewState["inot"] = "2";
                ViewState["OTBookingid"] = common.myInt(strAppId[0]);

                BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
                DataSet ds1 = objOT.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));

                if (ds1.Tables.Count > 0 && !common.myStr(ds1.Tables[0].Rows[0]["OTCheckouttime"]).Equals(""))
                {
                    dtDateTimeForOT.SelectedDate = common.myDate(ds1.Tables[0].Rows[0]["OTCheckouttime"]);

                    if (!IsAuthorizeToEdit)
                    {
                        btnSubmit.Enabled = false;
                    }
                    else
                    {
                        btnSubmit.Enabled = true;
                    }
                }
                ds1.Dispose(); objOT = null;
            }
            else
            {
                ViewState["StatusCode"] = common.myStr(e.MenuItem.Value);

                if (common.myStr(e.MenuItem.Value).Equals("Delete"))//cancel
                {
                    if (common.myInt(e.Appointment.Attributes["OrderId"]) > 0)
                    {
                        Alert.ShowAjaxMsg("Surgery already posted, Cannot cancel this appointment !", this.Page);
                        return;
                    }
                    btnDeleteThisApp.CommandArgument = "DELETE";
                    lblDeleteMessage.Text = "Are you sure you want to cancel this Break ?";
                    ViewState["AppId"] = common.myStr(e.Appointment.ID);
                    trCancelReason.Visible = true;
                    txtCancelRemarks.Text = "";
                    dvDelete.Visible = true;
                }
                else if (common.myStr(e.MenuItem.Value).Equals("OT-CANCEL")) // Cancel
                {
                    BaseC.Security baseUs = new BaseC.Security(sConString);
                    BaseC.clsOTBooking baseOT = new BaseC.clsOTBooking(sConString);

                    bool isAuthorizeToCancelOTBooking = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "isAuthorizeToCancelOTBooking");

                    if (isAuthorizeToCancelOTBooking)
                    {
                        GetCancelReason();
                        int x = common.myInt(Session["UserId"]);
                        ViewState["OTBookingid"] = common.myInt(strAppId[0]);

                        int y = baseOT.GetEncodedBy(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["OTBookingid"]));

                        bool IsAuthorize = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "isAuthorizeToCancelAllOTBooking");

                        if (!IsAuthorize && (x != y))// added by Umesh Maurya 3-Nov-2016 
                        {
                            Alert.ShowAjaxMsg("You are not authozied to cancel other users OT Booking !", this.Page);
                            return;
                        }
                        else
                        {

                            if (common.myInt(e.Appointment.Attributes["OrderId"]) > 0)
                            {
                                Alert.ShowAjaxMsg("Surgery already posted, Cannot cancel this appointment !", this.Page);
                                return;
                            }
                            btnDeleteThisApp.CommandArgument = "OT-CANCEL";
                            lblDeleteMessage.Text = "Are you sure you want to cancel this booking ?";
                            ViewState["AppId"] = e.Appointment.ID.ToString();
                            trCancelReason.Visible = true;
                            txtCancelRemarks.Text = "";
                            ddlReason.Visible = true;
                            dvDelete.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("You are not authozied to cancel OT Booking !", this.Page);
                        return;

                    }
                }

                else if (common.myStr(e.MenuItem.Value).Equals("OT-PAC")) // Cancel
                {
                    btnDeleteThisApp.CommandArgument = "OT-PAC";
                    lblDeleteMessage.Text = "Are you sure you want PAC Clearance" + "<br />" + " Of this booking ?";
                    ViewState["AppId"] = e.Appointment.ID.ToString();
                    trCancelReason.Visible = true;
                    txtCancelRemarks.Text = "";
                    ViewState["OTBookingid"] = common.myInt(strAppId[0]);
                    BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
                    DataSet ds1 = objOT.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));

                    if (ds1.Tables.Count > 0 && !common.myStr(ds1.Tables[0].Rows[0]["PACClearanceRemarks"]).Equals(""))
                    {
                        txtCancelRemarks.Text = common.myStr(ds1.Tables[0].Rows[0]["PACClearanceRemarks"]);
                        txtCancelRemarks.ReadOnly = true;
                        btnDeleteThisApp.Enabled = false;
                    }
                    dvDelete.Visible = true;
                }

                else if (common.myStr(e.MenuItem.Value).Equals("OT-CONF")) // Confirm
                {
                    string isPACReqInOTSchedule = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsPACRequiredOnOTAppointmentScreen", common.myInt(Session["FacilityId"]));
                    if (common.myStr(isPACReqInOTSchedule).Equals("Y"))
                    {
                        string origrule = "Select top 1 isnull(PACClearance,'0') from OTBooking where OTBookingID='" + common.myStr(strAppId[0]) + "'";
                        string OrigRecRule = common.myStr(dl.ExecuteScalar(CommandType.Text, origrule));
                        if (common.myBool(OrigRecRule).Equals(false))
                        {
                            Alert.ShowAjaxMsg("Please First do PAC Clearance !", this.Page);
                            return;
                        }
                    }


                    if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-UNCONF"))
                    {
                        if (common.myStr(isPACReqInOTSchedule).Equals("Y"))
                        {
                            string origrule = "Select top 1 isnull(PACClearance,'0') from OTBooking where OTBookingID='" + common.myStr(strAppId[0]) + "'";
                            string OrigRecRule = common.myStr(dl.ExecuteScalar(CommandType.Text, origrule));
                            if (common.myBool(OrigRecRule).Equals(false))
                            {
                                Alert.ShowAjaxMsg("Please First do PAC Clearance !", this.Page);
                                return;
                            }
                        }
                        else
                        {

                            btnDeleteThisApp.CommandArgument = "OT-CONF";
                            lblDeleteMessage.Text = "Are you sure you want to confirm this booking ?";
                            ViewState["AppId"] = common.myStr(e.Appointment.ID);
                            trCancelReason.Visible = false;
                            dvDelete.Visible = true;
                            return;
                        }
                    }
                    else if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CONF")
                        || common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CHKIN")
                        || common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-POST")
                        || common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-Bill")
                        )
                    {
                        Alert.ShowAjaxMsg("Patient already confirm !", this.Page);
                        return;
                    }
                    else if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-PAC"))
                    {
                        ViewState["OTBookingid"] = common.myInt(strAppId[0]);
                        if (common.myInt(e.Appointment.Attributes["OrderId"]) > 0)
                        {
                            Alert.ShowAjaxMsg("Surgery already posted, Cannot change status !", this.Page);
                            return;
                        }

                        btnDeleteThisApp.CommandArgument = "OT-CONF";
                        lblDeleteMessage.Text = "Are you sure you want to confirm this booking ?";
                        ViewState["AppId"] = common.myStr(e.Appointment.ID);
                        trCancelReason.Visible = false;
                        dvDelete.Visible = true;
                        return;

                    }
                }
                else if (common.myStr(e.MenuItem.Value).Equals("OT-CHKIN")) // check in
                {
                    BaseC.Security baseUs = new BaseC.Security(sConString);
                    bool IsAuthorize = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTCheckIn");
                    ViewState["OTBookingid"] = common.myInt(strAppId[0]);
                    BaseC.clsOTBooking objckOT = new BaseC.clsOTBooking(sConString);
                    DataSet dsck = objckOT.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));


                    if (!IsAuthorize)
                    {
                        Alert.ShowAjaxMsg("You are not authozied to OT Check-in !", this.Page);
                        return;
                    }

                    if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-UNCONF"))
                    {
                        Alert.ShowAjaxMsg("Please First do PAC Clearance !", this.Page);
                        return;
                    }
                    else if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-PAC") && common.myStr(ViewState["IsBillClearanceByPassForOT"]) == "N")
                    {
                        Alert.ShowAjaxMsg("Please First do Confirm !", this.Page);
                        //Alert.ShowAjaxMsg("Please First do OT Clearance !", this.Page);
                        return;
                    }
                    else if (((common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CONF") || common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-PAC")))
                        && common.myBool(dsck.Tables[0].Rows[0]["IsUnplannedReturnToOT"]) == false)
                    {
                        if (common.myStr(ViewState["IsBillClearanceByPassForOT"]) == "Y")
                        {
                            btnDeleteThisApp.CommandArgument = "OT-CHKIN";
                            lblDeleteMessage.Text = "Are you sure you want to checkin this booking ?";
                            ViewState["AppId"] = common.myStr(e.Appointment.ID);
                            trCancelReason.Visible = false;
                            dvDelete.Visible = true;
                            return;
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Please First do OT Clearance !", this.Page);
                            return;
                        }
                    }
                    else if ((common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CONF")) && common.myBool(dsck.Tables[0].Rows[0]["IsUnplannedReturnToOT"]) == true)
                    {
                        btnDeleteThisApp.CommandArgument = "OT-CHKIN";
                        lblDeleteMessage.Text = "Are you sure you want to checkin this booking ?";
                        ViewState["AppId"] = common.myStr(e.Appointment.ID);
                        trCancelReason.Visible = false;
                        dvDelete.Visible = true;
                        return;
                    }
                    else if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-Bill"))
                    {
                        btnDeleteThisApp.CommandArgument = "OT-CHKIN";
                        lblDeleteMessage.Text = "Are you sure you want to checkin this booking ?";
                        ViewState["AppId"] = common.myStr(e.Appointment.ID);
                        trCancelReason.Visible = false;
                        dvDelete.Visible = true;
                        return;
                    }
                    else if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CHKIN"))
                    {
                        Alert.ShowAjaxMsg("Patient already checkin...", this.Page);
                        return;
                    }
                }
                else if (common.myStr(e.MenuItem.Value).Equals("OT-UNCHKIN")) // uncheck in
                {
                    ViewState["OTBookingid"] = common.myInt(strAppId[0]);
                    BaseC.clsOTBooking objUnckOT = new BaseC.clsOTBooking(sConString);
                    DataSet dsunck = objUnckOT.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));

                    if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-UNCONF"))
                    {
                        Alert.ShowAjaxMsg("Please First do PAC Clearance !", this.Page);
                        return;
                    }
                    else if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-PAC") && common.myStr(ViewState["IsBillClearanceByPassForOT"]) == "N")
                    {
                        Alert.ShowAjaxMsg("Please First do Confirm !", this.Page);
                        return;
                    }
                    else if ((common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CONF") || common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-PAC"))
                        && common.myBool(dsunck.Tables[0].Rows[0]["IsUnplannedReturnToOT"]) == false)
                    {
                        if (common.myStr(ViewState["IsBillClearanceByPassForOT"]) == "Y")
                        {
                            btnDeleteThisApp.CommandArgument = "OT-CHKIN";
                            lblDeleteMessage.Text = "Are you sure you want to checkin this booking ?";
                            ViewState["AppId"] = common.myStr(e.Appointment.ID);
                            trCancelReason.Visible = false;
                            dvDelete.Visible = true;
                            return;
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Please First do OT Clearance !", this.Page);
                            return;
                        }
                    }
                    else if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-Bill"))
                    {
                        Alert.ShowAjaxMsg("Please First do OT Check-In !", this.Page);
                        return;
                    }
                    else if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-UNCHKIN"))
                    {
                        Alert.ShowAjaxMsg("OT Already Aborted !", this.Page);
                        return;
                    }
                    else if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-POST"))
                    {
                        Alert.ShowAjaxMsg("Surgery already posted, Cannot change status !", this.Page);
                        return;
                    }
                    else if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CHKIN"))
                    {
                        btnDeleteThisApp.CommandArgument = "OT-UNCHKIN";
                        lblDeleteMessage.Text = "Are you sure you want to Abort this  booking ?";
                        ViewState["AppId"] = common.myStr(e.Appointment.ID);
                        trCancelReason.Visible = true;
                        dvDelete.Visible = true;
                        ddlReason.Visible = true;
                        FillReasonBlockType();
                        txtCancelRemarks.ReadOnly = true;
                        return;
                    }
                }
                else if (common.myStr(e.MenuItem.Value).Equals("OT-ANB"))
                {
                    RadWindowForNew.NavigateUrl = "~/OTScheduler/OTAntibioticEntry.aspx?&OTBookingId=" + common.myStr(strAppId[0]);
                    RadWindowForNew.Height = 630;
                    RadWindowForNew.Width = 900;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.ReloadOnShow = true;
                }

                else if (common.myStr(e.MenuItem.Value).Equals("CLNDT"))
                {
                    Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                    Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);
                    Session["EncounterDate"] = common.myDate(e.Appointment.Attributes["EncounterDate"]);
                    Session["OPIP"] = common.myStr(e.Appointment.Attributes["OPIP"]);
                    Session["formId"] = "1";
                    Session["InvoiceId"] = 0;
                    if (common.myInt(common.myStr(e.Appointment.Attributes["EncounterId"])) == 0)
                    {
                        Alert.ShowAjaxMsg("Encounter not found for this patient", Page);
                        return;
                    }
                    RadWindowForNew.NavigateUrl = "/EMR/Templates/Default.aspx?From=POPUP&Type=OT&EREncounterId=" + common.myInt(e.Appointment.Attributes["EncounterId"]) +
                                                   "&AdmissionDate=" + common.myDate(e.Appointment.Attributes["EncounterDate"]).ToString("MM/dd/yyyy");
                    RadWindowForNew.Height = 610;
                    RadWindowForNew.Width = 880;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(e.MenuItem.Value).Equals("CSSHT"))
                {
                    Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                    Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);
                    Session["DoctorID"] = common.myInt(e.Appointment.Attributes["DoctorId"]);
                    Session["EncounterDate"] = common.myDate(e.Appointment.Attributes["EncounterDate"]).ToString("yyyy/MM/dd hh:mm:00tt");
                    Session["OPIP"] = common.myStr(e.Appointment.Attributes["OPIP"]);
                    if (common.myInt(common.myStr(e.Appointment.Attributes["EncounterId"])) == 0)
                    {
                        Alert.ShowAjaxMsg("Encounter not found for this patient", Page);
                        return;
                    }
                    RadWindowForNew.NavigateUrl = "/Editor/VisitHistory.aspx?Regid=" + common.myInt(e.Appointment.Attributes["RegistrationId"]) +
                                             "&RegNo=" + common.myInt(e.Appointment.Attributes["RegistrationNo"]) +
                                             "&EncId=" + common.myInt(e.Appointment.Attributes["EncounterId"]) +
                                             "&EncNo=" + common.myInt(e.Appointment.Attributes["EncounterNo"]) +
                                             "&FromWard=Y&Category=PopUp";
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 990;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(e.MenuItem.Value).Equals("MEDILLU"))
                {
                    Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                    Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);
                    Session["RegistrationNo"] = common.myInt(e.Appointment.Attributes["RegistrationNo"]);
                    Session["EncounterNo"] = common.myInt(e.Appointment.Attributes["EncounterNo"]);
                    Session["DoctorID"] = common.myInt(e.Appointment.Attributes["DoctorId"]);
                    Session["EncounterDate"] = common.myDate(e.Appointment.Attributes["EncounterDate"]).ToString("yyyy/MM/dd hh:mm:00tt");
                    Session["OPIP"] = common.myStr(e.Appointment.Attributes["OPIP"]);
                    if (common.myInt(common.myStr(e.Appointment.Attributes["EncounterId"])) == 0)
                    {
                        Alert.ShowAjaxMsg("Encounter not found for this patient", Page);
                        return;
                    }
                    RadWindowForNew.NavigateUrl = "/Editor/MedicalIllustration.aspx?RegId=" + common.myInt(e.Appointment.Attributes["RegistrationId"]) +
                                             "&EncId=" + common.myInt(e.Appointment.Attributes["EncounterId"]) +
                                             "&From=POPUP";
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 990;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(e.MenuItem.Value).Equals("FR"))
                {

                    RadWindowForNew.NavigateUrl = "/OTScheduler/OTBookingFollowupRemarks.aspx?RegNo=" + common.myInt(e.Appointment.Attributes["RegistrationNo"])
                        + "&AppId=" + common.myInt(strAppId[0])
                        + "&AppNo=" + common.myStr(e.Appointment.Attributes["AppointmentNo"])
                        + "&EncNo=" + common.myStr(e.Appointment.Attributes["EncounterNo"])
                        + "&OTBDt=" + Convert.ToString((e.Appointment.Start.Date.AddHours(e.Appointment.Start.Hour)).AddMinutes(e.Appointment.Start.Minute));
                    RadWindowForNew.Height = 300;
                    RadWindowForNew.Width = 500;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.ReloadOnShow = true;
                }
                else if (common.myStr(e.MenuItem.Value).Equals("CounEstimate"))
                {
                    RadWindowForNew.NavigateUrl = "/ATD/SearchCounselingDetails.aspx?RegId=" + common.myInt(e.Appointment.Attributes["RegistrationId"]);
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                }

                // Akshay
                else if (e.MenuItem.Value == "HKR")
                {
                    RadWindowForNew.NavigateUrl = "/OTScheduler/HousekeepingRequestOT.aspx?" +
                        "Category=PopUp" + "&FacilityId=" + common.myInt(ddlFacility.SelectedValue) + "&OTID=" + common.myStr(e.Appointment.Resources[0].Key);

                    RadWindowForNew.Title = "Housekeeping Request OT";
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 500;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.OnClientClose = string.Empty;
                    RadWindowForNew.VisibleOnPageLoad = true;
                    RadWindowForNew.Behaviors = WindowBehaviors.Close;
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
                #region Consignment Order
                else if (common.myStr(e.MenuItem.Value).Equals("CONOD"))
                {
                    Session["OPIP"] = "I";
                    Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                    Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);

                    if (common.myInt(Session["EncounterId"]) == 0)
                    {
                        Alert.ShowAjaxMsg("Please create encounter to order drug", Page);
                        return;
                    }

                    int iDoctorId = common.myInt(e.Appointment.Attributes["DoctorId"]);
                    if (iDoctorId == 0)
                    {
                        iDoctorId = common.myInt(Session["EmployeeId"]);
                    }
                    DataSet dsStatus = new DataSet();
                    BaseC.clsPharmacy status = new BaseC.clsPharmacy(sConString);
                    dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "CO", 0);


                    Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                    Session["PatientDetailString"] = null;
                    BaseC.Patient patient = new BaseC.Patient(sConString);

                    Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                               common.myStr(e.Appointment.Attributes["RegistrationNo"]), common.myStr(e.Appointment.Attributes["EncounterNo"]), common.myInt(Session["UserId"]), 0);



                    RadWindowForNew.NavigateUrl = "/EMR/Medication/ConsumableOrder.aspx?Regno=" + common.myInt(e.Appointment.Attributes["RegistrationNo"])
                                              + "&Encno=" + common.myInt(e.Appointment.Attributes["EncounterNo"])
                            + "&RegId=" + common.myInt(e.Appointment.Attributes["RegistrationId"])
                            + "&EncId=" + common.myInt(e.Appointment.Attributes["EncounterId"]) +
                                            "&POPUP=POPUP&DRUGORDERTYPE=" + common.myStr(dsStatus.Tables[0].Rows[0]["StatusID"]) +
                                            "&DRUGORDERCODE=" + common.myStr(dsStatus.Tables[0].Rows[0]["Code"]) +
                                            "&DoctorId=" + common.myStr(iDoctorId) +
                                            "&LOCATION=WARD" +
                                            "&IsConsignment=true&OTBookingId=" + strAppId[0];
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = string.Empty;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
                #endregion
                else
                {
                    if (common.myInt(e.Appointment.Attributes["OrderId"]) > 0)
                    {
                        Alert.ShowAjaxMsg("Surgery already posted, Cannot change status !", this.Page);
                        return;
                    }
                }
                //objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(strAppId[0]), common.myStr(e.MenuItem.Value), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]));
            }
            btnRefresh_OnClick(this, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void oldRadScheduler1_AppointmentContextMenuItemClicked(object sender, AppointmentContextMenuItemClickedEventArgs e)
    {
        ddlReason.Visible = false;
        txtCancelRemarks.ReadOnly = false;
        txtCancelRemarks.Text = String.Empty;

        //Done by ujjwal 17March2015 to change the time interval to the time interval of schedular start

        BaseC.HospitalSetup baseHs = new BaseC.HospitalSetup(sConString);
        string IsFillDefaultChekinTime = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsFillDefaultChekinTimeinScheduler", common.myInt(Session["FacilityId"]));
        if (common.myStr(IsFillDefaultChekinTime).Equals("Y"))
        {
            dtDateTimeForOT.TimeView.Interval = new TimeSpan(0, common.myInt(common.myStr(RadScheduler1.MinutesPerRow)), 0);
        }
        //Done by ujjwal 17March2015 to change the time interval to the time interval of schedular start

        try
        {
            BaseC.Patient BC = new BaseC.Patient(sConString);
            string[] strAppId = e.Appointment.ID.ToString().Split('_');
            Boolean isBreak = false;

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            if (common.myStr(e.MenuItem.Value).Equals("Edit"))
            {
                if (common.myStr(e.Appointment.Attributes["Type"]).Equals("Break"))
                {
                    RadWindowForNew.NavigateUrl = "~/Appointment/BreakAndBlock.aspx?" +
                                                    "Category=PopUp" +
                                                    "&DoctorID=0" +
                                                    "&ID=" + common.myStr(e.Appointment.ID) +
                                                    "&appDate=" + e.Appointment.Start.Date.ToString("dd/MM/yyyy") +
                                                    "&RecRule=" +
                                                    "&recparentid=" +
                                                    "&appId=" + common.myStr(e.Appointment.ID) +
                                                    "&TimeInterval=" + common.myStr(RadScheduler1.MinutesPerRow) +
                                                    "&StTime=" + common.myStr(RadScheduler1.DayStartTime.Hours) +
                                                    "&EndTime=" + common.myStr(RadScheduler1.DayEndTime.Hours) +
                                                    "&FacilityId=" + common.myInt(ddlFacility.SelectedValue) +
                                                    "&TheaterId=" + common.myInt(e.Appointment.Resources[0].Key) +
                                                    "&UseFor=OT&Edit=Edit";

                    RadWindowForNew.Height = 550;
                    RadWindowForNew.Width = 730;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                }
                else
                {
                    bool IsDoctor = false;
                    if (ViewState["IsDoctor"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["IsDoctor"];
                        DataView dvDoctor = new DataView(dt);
                        dvDoctor.RowFilter = "ResourceID='" + common.myInt(e.Appointment.Resources[0].Key) + "'";
                        DataTable dtDoctor = dvDoctor.ToTable();
                        if (dtDoctor.Rows.Count > 0)
                        {
                            IsDoctor = common.myBool(dtDoctor.Rows[0]["IsDoctor"]);
                        }
                    }

                    if (RadScheduler1.DayStartTime.Hours == RadScheduler1.DayEndTime.Hours)
                    {
                        RadWindowForNew.NavigateUrl = "~/OTScheduler/OTBooking.aspx?StTime=" + common.myStr(RadScheduler1.DayStartTime.Hours)
                                                    + "&EndTime=" + common.myStr((RadScheduler1.DayEndTime.Hours + 1))
                                                    + "&appDate=" + e.Appointment.Start.Date.ToString("MM/dd/yyyy")
                                                    + "&appid=" + common.myStr(strAppId[0])
                                                    + "&OTID=" + common.myStr(e.Appointment.Resources[0].Key)
                                                    + "&TimeInterval=" + common.myStr(RadScheduler1.MinutesPerRow)
                                                    + "&AppId1=" + common.myStr(e.Appointment.ID)
                                                    + "&PageId=3"
                                                    + "&IsDoctor=" + IsDoctor
                                                    + "&FacilityId=" + common.myStr(ddlFacility.SelectedValue);
                    }
                    else
                    {
                        RadWindowForNew.NavigateUrl = "~/OTScheduler/OTBooking.aspx?StTime=" + common.myStr(RadScheduler1.DayStartTime.Hours)
                                                + "&EndTime=" + common.myStr(RadScheduler1.DayEndTime.Hours)
                                                + "&appDate=" + e.Appointment.Start.Date.ToString("MM/dd/yyyy")
                                                + "&appid=" + common.myStr(strAppId[0])
                                                + "&OTID=" + common.myStr(e.Appointment.Resources[0].Key)
                                                + "&TimeInterval=" + common.myStr(RadScheduler1.MinutesPerRow)
                                                + "&AppId1=" + common.myStr(e.Appointment.ID)
                                                + "&PageId=3"
                                                + "&IsDoctor=" + IsDoctor
                                                + "&FacilityId=" + common.myStr(ddlFacility.SelectedValue);
                    }

                    RadWindowForNew.Height = 630;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            else if (common.myStr(e.MenuItem.Value).Equals("Print"))
            {

                RadWindowForNew.NavigateUrl = "/EMRReports/AppointmentSlip.aspx?RegistrationId=" + common.myInt(e.Appointment.Attributes["RegistrationId"]) + "&AppointmentId=" + common.myInt(strAppId[0]);
                RadWindowForNew.Height = 580;
                RadWindowForNew.Width = 750;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
                RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("Post"))
            {
                int iCompanyID = 0;
                int BillClearance = 0;
                string OPIP = "O";
                int RegId = 0;
                int EncId = 0;
                int InsuranceId = 0;
                int CardId = 0;
                int iCurrentBedCategory = 0;
                int isPackage = 0;
                int OrderId = common.myInt(e.Appointment.Attributes["OrderId"]);
                if (OrderId > 0)
                {
                    Alert.ShowAjaxMsg("Surgery already posted !", this.Page);
                    return;
                }

                //Added on 09-02-2014 Start Naushad Ali Comment as logic changed
                //if (e.Appointment.Attributes["StatusCode"].ToString().Trim() != "OT-CHKIN")
                //{
                //    Alert.ShowAjaxMsg("Please Check-in, Posting cannot be done !", this.Page);
                //    return;
                //}
                //Added on 09-02-2014 End Naushad Ali


                // BaseC.ATD objATD = new BaseC.ATD(sConString);
                //// objATD.

                // DataSet dsCheck = objATD.CheckOTChecklist(common.myInt(strAppId[0]));
                // if (dsCheck.Tables.Count == 1)
                // {
                //     if (dsCheck.Tables[0].Rows.Count == 0)
                //     {
                //         Session["RegistrationId"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                //         Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);
                //         lblMessage.Text = "";
                //         // RadWindowForNew.NavigateUrl = "~/OTScheduler/OTChecklist.aspx?IpNo=" + hdnIpno.Value + "&Pname=" + hdnPatientname.Value + "&Surgery=" + hdnSurgeryname.Value.Replace("&", "@") + "&ward=" + hdnWardno.Value + "&BookingId=" + hdnBookinId.Value + "&BedId=" + hdnBedId.Value + "&SurgeryId=" + hdnSurgeryId.Value + "  ";
                //         RadWindowForNew.NavigateUrl = "~/OTScheduler/OTChecklist.aspx?BookingId=" + common.myInt(strAppId[0]);
                //         RadWindowForNew.Height = 550;
                //         RadWindowForNew.Width = 900;
                //         RadWindowForNew.Top = 40;
                //         RadWindowForNew.Left = 100;
                //         //RadWindowForNew.OnClientClose = "OnClientClose";
                //         RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                //         RadWindowForNew.Modal = true;
                //         RadWindowForNew.VisibleStatusbar = false;
                //         return;
                //     }
                // }

                if (common.myStr(e.Appointment.Attributes["BillClearance"]).Equals("True") || common.myStr(e.Appointment.Attributes["BillClearance"]).Equals("1"))
                {
                    BillClearance = 1;
                }
                if (BillClearance == 0 && common.myStr(ViewState["IsBillClearanceByPassForOT"]) == "N")
                {
                    Alert.ShowAjaxMsg("Bill clearance is pending, Posting cannot be done !", this.Page);
                    return;
                }
                Hashtable htIn = new Hashtable();
                RegId = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                EncId = common.myInt(e.Appointment.Attributes["EncounterId"]);
                isPackage = common.myInt(e.Appointment.Attributes["IsPackage"]);

                if (common.myInt(RegId).Equals(0))
                {
                    Alert.ShowAjaxMsg("Patient is not registered, please register before posting !", this.Page);
                    return;
                }
                else if (common.myInt(EncId).Equals(0))
                {
                    Alert.ShowAjaxMsg("Patient encounter not found, please create encounter before posting !", this.Page);
                    return;
                }
                else
                {
                    int HospId = common.myInt(Session["HospitalLocationID"]);
                    int FacilityId = common.myInt(Session["FacilityId"]);
                    int EncodedBy = common.myInt(Session["UserId"]);
                    DataSet ds = new DataSet();
                    BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
                    ds = objCommon.GetPatientSummary(HospId, FacilityId, RegId, EncId, EncodedBy);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (common.myStr(ds.Tables[0].Rows[0]["OPIP"]).Equals("I"))
                        {
                            OPIP = "I";
                            iCompanyID = common.myInt(ds.Tables[0].Rows[0]["CompanyCode"]);
                            InsuranceId = common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"]);
                            CardId = common.myInt(ds.Tables[0].Rows[0]["CardId"]);
                            iCurrentBedCategory = common.myInt(ds.Tables[0].Rows[0]["CurrentBedCategory"]);
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Surgery posting for OP is not available right now !", this.Page);
                            return;
                        }
                    }
                    if (common.myInt(iCompanyID).Equals(0))
                    {
                        Alert.ShowAjaxMsg("Patient company and bed category not available, please check admission !", this.Page);
                        return;
                    }
                    if (common.myInt(iCurrentBedCategory).Equals(0))
                    {
                        OPIP = "O";
                    }

                    ViewState["OTBookingid"] = common.myInt(strAppId[0]);
                    BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
                    DataSet ds1 = objOT.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));

                    if (ds1.Tables.Count > 0)
                    {
                        if (common.myStr(ds1.Tables[0].Rows[0]["OTCheckintime"]).Equals(""))
                        {
                            Alert.ShowAjaxMsg("Please update OT check in time !", this.Page);
                            return;
                        }

                        if (common.myStr(ds1.Tables[0].Rows[0]["OTCheckouttime"]).Equals(""))
                        {
                            Alert.ShowAjaxMsg("Please update OT check out time !", this.Page);
                            return;
                        }
                    }
                    ds.Dispose(); ds1.Dispose(); objOT = null; objCommon = null;

                }
                if (common.myInt(isPackage).Equals(0))
                    RadWindowForNew.NavigateUrl = "~/EMRBILLING/Popup/AddServicesV1.aspx?RegNo=0&RegId=" + RegId + "&OTBookingId=" + common.myInt(strAppId[0]) + "&EncId=" + EncId + "&CompanyId=" + iCompanyID + "&CardId=" + CardId + "&PayerType=0&InsuranceId=" + InsuranceId + "&OP_IP=" + OPIP;
                else
                    RadWindowForNew.NavigateUrl = "~/EMRBILLING/Popup/AddPackagesV1.aspx?RegNo=0&RegId=" + RegId + "&OTBookingId=" + common.myInt(strAppId[0]) + "&EncId=" + EncId + "&CompanyId=" + iCompanyID + "&CardId=" + CardId + "&PayerType=0&InsuranceId=" + InsuranceId + "&OP_IP=" + OPIP;


                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
                RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;

            }
            else if (common.myStr(e.MenuItem.Value).Equals("OTC"))// OT Charge
            {
                int RegId = 0;
                int EncId = 0;
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int EncodedBy = common.myInt(Session["UserId"]);
                string OPIP = "O";
                int iCompanyID = 0;
                int InsuranceId = 0;
                int CardId = 0;
                string RegNo = "";
                string EncNo = "";
                DataSet ds = new DataSet();
                RegId = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                EncId = common.myInt(e.Appointment.Attributes["EncounterId"]);

                BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
                ds = objCommon.GetPatientSummary(HospId, FacilityId, RegId, EncId, EncodedBy);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    iCompanyID = common.myInt(ds.Tables[0].Rows[0]["CompanyCode"]);
                    InsuranceId = common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"]);
                    RegNo = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
                    EncNo = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
                    CardId = common.myInt(ds.Tables[0].Rows[0]["CardId"]);

                    OPIP = "I";
                }
                ds.Dispose();
                objCommon = null;
                RadWindowForNew.NavigateUrl = "~/OTScheduler/AddChargeTimewisOT.aspx?RegId=" + common.myStr(e.Appointment.Attributes["RegistrationId"]) + "&EncId=" + common.myStr(e.Appointment.Attributes["EncounterId"]) + "&RegNo=" + RegNo + "&EncNo=" + EncNo + "&CardId=" + CardId + "&CompanyId=" + iCompanyID + "&InsuranceId=" + InsuranceId + "&OP_IP=" + OPIP + "&OTId=" + common.myInt(strAppId[0]);
                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 900;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("Template"))
            {
                RadWindowForNew.NavigateUrl = "~/OTScheduler/OTClinicalTemplate.aspx?RegistrationID=" + common.myStr(e.Appointment.Attributes["RegistrationId"]) + "&EncounterID=" + common.myStr(e.Appointment.Attributes["EncounterId"]);
                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.ReloadOnShow = true;
                RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            }

            else if (common.myStr(e.MenuItem.Value).Equals("SR"))
            {
                BaseC.WardManagement objwm = new BaseC.WardManagement();
                DataSet ds = new DataSet();
                ds = objwm.GetDoctor(common.myInt(e.Appointment.Attributes["RegistrationId"]), common.myInt(e.Appointment.Attributes["EncounterId"]));

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Session["OPIP"] = "I";
                    Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                    Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);
                    Session["DoctorID"] = common.myInt(ds.Tables[0].Rows[0]["AdmittingDoctorId"]);

                    RadWindowForNew.NavigateUrl = "/EMRBILLING/Popup/AddServicesV1.aspx?Regid=" + common.myInt(Session["RegistrationID"]) +
                        "&RegNo=" + common.myLong(common.myLong(e.Appointment.Attributes["RegistrationNo"])) +
                        "&EncId=" + common.myInt(Session["EncounterId"]) +
                        "&EncNo=" + common.myStr(common.myInt(e.Appointment.Attributes["EncounterNo"])) +
                        "&OP_IP=I&FromWard=Y&CompanyId=0&InsuranceId=0&CardId=0&PayerType=&BType=";

                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
                ds.Dispose(); objwm = null;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("Copy") || common.myStr(e.MenuItem.Value).Equals("Cut"))
            {

                if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CHKIN"))
                {
                    Alert.ShowAjaxMsg("Not allow this action after check-in.", Page);
                    return;
                }

                if (isBreak != true)
                {
                    ViewState["CopyAppId"] = strAppId[0];
                    ViewState["MenuOption"] = common.myStr(e.MenuItem.Value);
                }
            }
            else if (common.myStr(e.MenuItem.Value).Equals("DO"))
            {
                Session["OPIP"] = "I";
                Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);

                if (common.myInt(Session["EncounterId"]) == 0)
                {
                    Alert.ShowAjaxMsg("Please create encounter to order drug", Page);
                    return;
                }

                int iDoctorId = common.myInt(e.Appointment.Attributes["DoctorId"]);
                if (iDoctorId == 0)
                {
                    iDoctorId = common.myInt(Session["EmployeeId"]);
                }
                DataSet dsStatus = new DataSet();
                BaseC.clsPharmacy status = new BaseC.clsPharmacy(sConString);
                dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "CO", 0);

                if (dsStatus.Tables.Count > 0 && dsStatus.Tables[0].Rows.Count > 0)
                {
                    RadWindowForNew.NavigateUrl = "/EMR/Medication/PrescribeMedication.aspx?Regno="
                        + common.myInt(e.Appointment.Attributes["RegistrationNo"])
                        + "&Encno=" + common.myInt(e.Appointment.Attributes["EncounterNo"])
                        + "&RegId=" + common.myInt(e.Appointment.Attributes["RegistrationId"])
                        + "&EncId=" + common.myInt(e.Appointment.Attributes["EncounterId"])
                        + "&POPUP=POPUP&DRUGORDERTYPE=" + common.myStr(dsStatus.Tables[0].Rows[0]["StatusID"])
                        + "&DRUGORDERCODE=" + common.myStr(dsStatus.Tables[0].Rows[0]["Code"])
                        + "&DoctorId=" + iDoctorId + "&LOCATION=OT";

                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
                dsStatus.Dispose(); status = null;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("DR"))
            {
                Session["OPIP"] = "I";
                Session["RegistrationID"] = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                Session["EncounterId"] = common.myInt(e.Appointment.Attributes["EncounterId"]);

                RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/IPDItemReturn.aspx?MASTER=No&Regno=" + common.myInt(e.Appointment.Attributes["RegistrationNo"]) + "&Encno=" + common.myStr(e.Appointment.Attributes["EncounterNo"]);
                RadWindowForNew.Height = 610;
                RadWindowForNew.Width = 940;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.OnClientClose = ""; //"SearchPatientOnClientClose";//
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("OTIT"))
            {
                BaseC.Security baseUs = new BaseC.Security(sConString);
                bool IsAuthorize = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTCheckIn ");

                if (!IsAuthorize)
                {
                    Alert.ShowAjaxMsg("You are not authozied to enter OT Check-in Time !", this.Page);
                    return;
                }

                int OrderId = common.myInt(e.Appointment.Attributes["OrderId"]);
                int BillClearance = 0;
                if (OrderId > 0)
                {
                    Alert.ShowAjaxMsg("Surgery already posted !", this.Page);
                    return;
                }

                //Added on 02-09-2014 Start Naushad
                if (common.myStr(e.Appointment.Attributes["BillClearance"]) == "True" || common.myStr(e.Appointment.Attributes["BillClearance"]) == "1")
                {
                    BillClearance = 1;
                }
                if (common.myInt(BillClearance).Equals(0) && common.myStr(ViewState["IsBillClearanceByPassForOT"]) == "N")
                {
                    Alert.ShowAjaxMsg("Bill clearance is pending, Posting cannot be done !", this.Page);
                    return;
                }

                lblMsg.Text = string.Empty;
                //bindminut();

                //Added By Ujjwal to change the date of the dtDateTimeForOT to the otBooking Time start
                if (common.myStr(IsFillDefaultChekinTime).Equals("Y"))
                {
                    dtDateTimeForOT.SelectedDate = e.Appointment.Start;
                }
                else
                {
                    dtDateTimeForOT.SelectedDate = null;
                }
                //Added By Ujjwal to change the date of the dtDateTimeForOT to the otBooking Time end
                divOTCheckinCheckoutTime.Visible = true;
                Label4.Text = "OT Check In Time";
                ViewState["inot"] = "1";
                ViewState["OTBookingid"] = common.myInt(strAppId[0]);

                BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
                DataSet ds1 = objOT.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));
                if (ds1.Tables.Count > 0)
                {
                    if (ds1.Tables[0].Rows[0]["OTCheckintime"].ToString() != "")
                    {
                        dtDateTimeForOT.SelectedDate = common.myDate(ds1.Tables[0].Rows[0]["OTCheckintime"].ToString());
                    }
                }
                ds1.Dispose(); objOT = null;
            }
            else if (common.myStr(e.MenuItem.Value).Equals("OTOT"))
            {
                BaseC.Security baseUs = new BaseC.Security(sConString);
                bool IsAuthorize = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTCheckOut");

                if (!IsAuthorize)
                {
                    Alert.ShowAjaxMsg("You are not authozied to enter OT Check-out Time !", this.Page);
                    return;
                }


                int OrderId = common.myInt(e.Appointment.Attributes["OrderId"]);
                if (OrderId > 0)
                {
                    Alert.ShowAjaxMsg("Surgery already posted !", this.Page);
                    return;
                }

                lblMsg.Text = string.Empty;
                // bindminut();
                //Added By Ujjwal to change the date of the dtDateTimeForOT to the otBooking Time start
                if (common.myStr(IsFillDefaultChekinTime) == "Y")
                {
                    dtDateTimeForOT.SelectedDate = e.Appointment.End;
                }
                else
                {
                    dtDateTimeForOT.SelectedDate = null;
                }
                //Added By Ujjwal to change the date of the dtDateTimeForOT to the otBooking Time end


                divOTCheckinCheckoutTime.Visible = true;
                Label4.Text = "OT Check Out Time";
                ViewState["inot"] = "2";
                ViewState["OTBookingid"] = common.myInt(strAppId[0]);

                BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
                DataSet ds1 = objOT.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));

                if (ds1.Tables.Count > 0 && !common.myStr(ds1.Tables[0].Rows[0]["OTCheckouttime"]).Equals(""))
                {
                    dtDateTimeForOT.SelectedDate = common.myDate(ds1.Tables[0].Rows[0]["OTCheckouttime"]);
                }
                ds1.Dispose(); objOT = null;
            }
            else
            {
                ViewState["StatusCode"] = common.myStr(e.MenuItem.Value);

                if (common.myStr(e.MenuItem.Value).Equals("Delete"))//cancel
                {
                    btnDeleteThisApp.CommandArgument = "DELETE";
                    lblDeleteMessage.Text = "Are you sure you want to cancel this Break ?";
                    ViewState["AppId"] = common.myStr(e.Appointment.ID);
                    trCancelReason.Visible = true;
                    txtCancelRemarks.Text = "";
                    dvDelete.Visible = true;
                }
                else if (common.myStr(e.MenuItem.Value).Equals("OT-CANCEL")) // Cancel
                {
                    BaseC.Security baseUs = new BaseC.Security(sConString);
                    BaseC.clsOTBooking baseOT = new BaseC.clsOTBooking(sConString);

                    bool isAuthorizeToCancelOTBooking = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "isAuthorizeToCancelOTBooking");

                    if (isAuthorizeToCancelOTBooking)
                    {
                        int x = common.myInt(Session["UserId"]);
                        ViewState["OTBookingid"] = common.myInt(strAppId[0]);

                        int y = baseOT.GetEncodedBy(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["OTBookingid"]));

                        bool IsAuthorize = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "isAuthorizeToCancelAllOTBooking");

                        if (!IsAuthorize && (x != y))// added by Umesh Maurya 3-Nov-2016 
                        {
                            Alert.ShowAjaxMsg("You are not authozied to cancel other users OT Booking !", this.Page);
                            return;
                        }
                        else
                        {

                            if (common.myInt(e.Appointment.Attributes["OrderId"]) > 0)
                            {
                                Alert.ShowAjaxMsg("Surgery already posted, Cannot cancel this appointment !", this.Page);
                                return;
                            }
                            btnDeleteThisApp.CommandArgument = "OT-CANCEL";
                            lblDeleteMessage.Text = "Are you sure you want to cancel this booking ?";
                            ViewState["AppId"] = e.Appointment.ID.ToString();
                            trCancelReason.Visible = true;
                            txtCancelRemarks.Text = "";
                            dvDelete.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("You are not authozied to cancel OT Booking !", this.Page);
                        return;

                    }
                }

                else if (common.myStr(e.MenuItem.Value).Equals("OT-PAC")) // Cancel
                {
                    btnDeleteThisApp.CommandArgument = "OT-PAC";
                    lblDeleteMessage.Text = "Are you sure you want PAC Clearance" + "<br />" + " Of this booking ?";
                    ViewState["AppId"] = e.Appointment.ID.ToString();
                    trCancelReason.Visible = true;
                    txtCancelRemarks.Text = "";
                    dvDelete.Visible = true;
                }

                else if (common.myStr(e.MenuItem.Value).Equals("OT-CONF")) // Confirm
                {
                    string isPACReqInOTSchedule = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsPACRequiredOnOTAppointmentScreen", common.myInt(Session["FacilityId"]));
                    if (common.myStr(isPACReqInOTSchedule).Equals("Y"))
                    {
                        string origrule = "Select top 1 isnull(PACClearance,'0') from OTBooking where OTBookingID='" + common.myStr(strAppId[0]) + "'";
                        string OrigRecRule = common.myStr(dl.ExecuteScalar(CommandType.Text, origrule));
                        if (common.myBool(OrigRecRule).Equals(false))
                        {
                            Alert.ShowAjaxMsg("Please First do PAC Clearance !", this.Page);
                            return;
                        }
                    }
                    if (common.myInt(e.Appointment.Attributes["OrderId"]) > 0)
                    {
                        Alert.ShowAjaxMsg("Surgery already posted, Cannot change status !", this.Page);
                        return;
                    }
                    btnDeleteThisApp.CommandArgument = "OT-CONF";
                    lblDeleteMessage.Text = "Are you sure you want to confirm this booking ?";
                    ViewState["AppId"] = common.myStr(e.Appointment.ID);
                    trCancelReason.Visible = false;
                    dvDelete.Visible = true;
                    return;
                }
                else if (common.myStr(e.MenuItem.Value).Equals("OT-CHKIN")) // check in
                {
                    BaseC.Security baseUs = new BaseC.Security(sConString);
                    bool IsAuthorize = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTCheckIn  ");

                    if (!IsAuthorize)
                    {
                        Alert.ShowAjaxMsg("You are not authozied to OT Check-in !", this.Page);
                        return;
                    }

                    if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CONF"))
                    {
                        Alert.ShowAjaxMsg("Please Confirm, Cannot change status !", this.Page);
                        return;
                    }
                }
                else if (common.myStr(e.MenuItem.Value).Equals("OT-UNCHKIN")) // uncheck in
                {
                    if (common.myStr(e.Appointment.Attributes["StatusCode"]).Trim().Equals("OT-CHKIN"))
                    {
                        btnDeleteThisApp.CommandArgument = "OT-UNCHKIN";
                        lblDeleteMessage.Text = "Are you sure you want to uncheck-in this booking ?";
                        ViewState["AppId"] = common.myStr(e.Appointment.ID);
                        trCancelReason.Visible = true;
                        dvDelete.Visible = true;
                        ddlReason.Visible = true;
                        FillReasonBlockType();
                        txtCancelRemarks.ReadOnly = true;

                        return;
                    }
                }
                else
                {
                    if (common.myInt(e.Appointment.Attributes["OrderId"]) > 0)
                    {
                        Alert.ShowAjaxMsg("Surgery already posted, Cannot change status !", this.Page);
                        return;
                    }
                }
                objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(strAppId[0]), common.myStr(e.MenuItem.Value), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]));
            }
            btnRefresh_OnClick(this, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnUpdateStatus_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (txtCancelationReasons.Text.Trim() == "")
            {
                Alert.ShowAjaxMsg("Please type reason", Page);
                return;
            }

            if (Convert.ToInt32(ViewState["MenuItemValue"]) == 34)
            {

                DateTime OrigTime = DateTime.Now;
                string[] strAppId = ViewState["MenuAppID"].ToString().Split('_');
                //string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                //ViewState["MenuSdate"] = e.Appointment.Start.Date;
                //string[] strAppId = e.Appointment.ID.ToString().Split('_');
                DateTime dtStart = Convert.ToDateTime(ViewState["MenuSdate"].ToString());
                ////DateTime dt = e.Appointment.Start;
                //Added by vineet for cancel of single appointment

                ////strAppId = e.Appointment.ID.ToString().Split('_');
                Hashtable hshIn = new Hashtable();

                string origrule = "Select isnull(RecurrenceRule,'') as RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (OrigRecRule.Trim() != null && OrigRecRule.Trim() != "")
                {

                    ////DateTime dtStart = e.Appointment.Start;
                    //Hashtable hshInput = new Hashtable();
                    //Hashtable hshOutput = new Hashtable();
                    //hshInput.Add("@intParentAppointmentID", strAppId[0]);
                    //hshInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                    //hshInput.Add("@intFacilityID", Session["FacilityID"]);
                    //hshOutput.Add("@intStatusID", SqlDbType.Int);

                    //hshInput.Add("@chrAppointmentDate", dtStart.ToString("MM/dd/yyyy"));

                    //hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRGetRecurringAppointment", hshInput, hshOutput);
                    Hashtable hshOutput = objwcfOt.GetRecurringAppointment(common.myInt(strAppId[0]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myDate(dtStart));

                    if (hshOutput["@intStatusID"].ToString() == "34")
                    {
                        //btnRefresh_OnClick(sender, e);
                    }
                    else
                    {
                        ////Hashtable hshIn = new Hashtable();
                        //Hashtable hshtableout = new Hashtable();
                        //hshIn.Add("@intAppointmentId", strAppId[0]);
                        //hshIn.Add("@intEncodedBy", Session["UserId"]);
                        //hshIn.Add("@intStatusId", Convert.ToInt32(ViewState["MenuItemValue"]));
                        //hshtableout.Add("@intRecAppointmentId", SqlDbType.Int);
                        //hshIn.Add("@dtAppointmentDate", dtStart.Date);


                        //hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspChangeRecurringAppointmentStatus", hshIn, hshtableout);
                        Hashtable hshtableout = objwcfOt.ChangeRecurringAppointmentStatus(common.myInt(strAppId[0]), common.myInt(Session["UserId"]), common.myInt(ViewState["MenuItemValue"]), common.myDate(dtStart));
                        string strUpdateRecID = hshtableout["@intRecAppointmentId"].ToString();
                        string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                        OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                        string strOrigDate = "select AppointmentDate from DoctorAppointment where appointmentid='" + Convert.ToInt32(hshtableout["intRecAppointmentId"]) + "'";

                        //DateTime dtStart = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                        string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";

                        //if (Convert.ToInt64(Request.QueryString["appid"]) != 0)
                        //{                

                        if (!OrigRecRule.Contains("EXDATE"))
                        {
                            string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                            string strUpdateNote = "Update doctorappointment set Note= '" + txtCancelationReasons.Text + "' where appointmentid='" + strUpdateRecID + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateNote);
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        }
                        else
                        {
                            string UpdateRule = OrigRecRule + "," + exdate;
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                            string strUpdateNote = "Update doctorappointment set  Note='" + txtCancelationReasons.Text + "' where appointmentid='" + strUpdateRecID + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateNote);
                        }

                        ////string OrigRecRule = "";
                        ////DataSet Ds = dl.(CommandType.Text, origrule).ToString();
                        //DateTime OrigRecTime = dt.Date;//e.Appointment.Start.Date;
                        //string strOrigRecTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                        //OrigRecTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigRecTime));

                        //string UpdateRule = "";
                        //DateTime dtStart = dt.Date;//e.Appointment.Start.Date;
                        ////string exdate1 = dtStart.AddDays(1).ToString("yyyyMMdd") + "T" + OrigRecTime.ToString("HHmmss") + "Z";



                        //string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigRecTime.ToString("HHmmss") + "Z";
                        //string query = "Insert into DoctorAppointment([HospitalLocationId] ,[RegistrationId],[FacilityID],[DoctorId],[RoomId],[VisitTypeId],[RegistrationNo],[AppointmentDate],[FromTime],[ToTime],[TitleId],[FirstName],[MiddleName],[LastName],[DateofBirth],[AgeYears] ,[AgeMonths],[AgeDays],[Gender],[ResidencePhone],[Mobile],[Email],[ReferredType],[ReferringPhysician],[Remarks],[WalkInPatient],[PaymentCaseId] ,[AuthorizationNo] ,[StatusId] ,[Active],[EncodedBy],[EncodedDate],[LastChangedBy],[LastChangedDate],[RecurrenceRule],[RecurrenceParentId], [Note]) SELECT [HospitalLocationId] ,[RegistrationId],[FacilityID],[DoctorId],[RoomId],[VisitTypeId],[RegistrationNo],'" + dtStart + "',[FromTime],[ToTime],[TitleId],[FirstName],[MiddleName],[LastName],[DateofBirth],[AgeYears] ,[AgeMonths],[AgeDays],[Gender],[ResidencePhone],[Mobile],[Email],[ReferredType],[ReferringPhysician],[Remarks],[WalkInPatient],[PaymentCaseId] ,[AuthorizationNo] ,34 ,[Active],[EncodedBy],[EncodedDate],[LastChangedBy],[LastChangedDate],'',[RecurrenceParentId], [Note] + '" + txtCancelationReasons.Text.Replace("'", " ") + "' FROM DoctorAppointment  where AppointmentId=" + strAppId[0] + "";
                        //dl.ExecuteNonQuery(CommandType.Text, query);


                        //// OrigRecRule = OrigRecRule.Replace(exdate, exdate1);


                        //if (!OrigRecRule.Contains("EXDATE"))
                        //{
                        //    UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                        //    string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' ,  Note= '" + txtCancelationReasons.Text + "' where appointmentid='" + strAppId[0] + "'";
                        //    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        //}
                        //else
                        //{
                        //    UpdateRule = OrigRecRule + "," + exdate;
                        //    string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' ,  Note= '" + txtCancelationReasons.Text + "' where appointmentid='" + strAppId[0] + "'";
                        //    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        //}
                    }
                }
                else
                {
                    ////Hashtable hshIn = new Hashtable();
                    //hshIn.Add("@intAppointmentId", strAppId[0]);
                    //hshIn.Add("@intEncodedBy", Session["UserId"]);
                    ////hshIn.Add("@intStatusId", common.myStr(e.MenuItem.Attributes["Code"]));
                    //hshIn.Add("@intStatusId", ViewState["MenuItemValue"]);
                    //dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);

                    objwcfOt.ChangeAppointmentStatus(common.myInt(strAppId[0]), common.myInt(Session["UserId"]), common.myInt(ViewState["MenuItemValue"]));

                    string strUpdateRecRule = "Update doctorappointment set Note='" + txtCancelationReasons.Text + "' where appointmentid='" + strAppId[0] + "'";
                    int i = dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                }

                ViewState["MenuAppID"] = null;
                ViewState["MenuItemValue"] = null;
                ViewState["MenuSdate"] = null;

                txtCancelationReasons.Text = "";
                dvUpdateStatus.Visible = false;

                //btnRefresh_OnClick(sender, e);
            }
            if (Convert.ToInt32(ViewState["MenuItemValue"]) == 6)
            {
                //ViewState["MenuItemValue"] = common.myStr(e.MenuItem.Attributes["Code"]);
                //ViewState["MenuAppID"] = e.Appointment.ID.ToString();
                //ViewState["MenuRecurrenceParentID"] = e.Appointment.RecurrenceParentID.ToString();
                //ViewState["MenuRecurrenceState"] = e.Appointment.RecurrenceState;

                string recparentid = "";
                DateTime OrigTime = DateTime.Now;
                DateTime OrigDate = Convert.ToDateTime(ViewState["MenuSdate"].ToString());
                string[] strAppId = ViewState["MenuAppID"].ToString().Split('_');

                string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                //if ((e.Appointment.RecurrenceState == RecurrenceState.Occurrence || e.Appointment.RecurrenceState == RecurrenceState.Exception) && OrigRecRule.Trim() != "")
                if ((ViewState["MenuRecurrenceState"].ToString() == "Occurrence" || ViewState["MenuRecurrenceState"].ToString() == "Exception") && OrigRecRule.Trim() != "")
                {
                    if (ViewState["MenuAppID"].ToString().Contains('_'))
                    {
                        //recparentid = e.Appointment.RecurrenceParentID.ToString();
                        recparentid = ViewState["MenuRecurrenceParentID"].ToString();
                    }
                    DateTime dtStart = Convert.ToDateTime(ViewState["MenuSdate"].ToString());

                    ////DateTime dtStart = e.Appointment.Start;
                    //Hashtable hshInput = new Hashtable();
                    //Hashtable hshOutput = new Hashtable();
                    //hshInput.Add("@intParentAppointmentID", strAppId[0]);
                    //hshInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                    //hshInput.Add("@intFacilityID", Session["FacilityID"]);
                    //hshOutput.Add("@intStatusID", SqlDbType.Int);

                    //hshInput.Add("@chrAppointmentDate", dtStart.ToString("MM/dd/yyyy"));

                    //hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRGetRecurringAppointment", hshInput, hshOutput);

                    Hashtable hshOutput = objwcfOt.GetRecurringAppointment(common.myInt(strAppId[0]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myDate(dtStart));

                    if (hshOutput["@intStatusID"].ToString() == "6")
                    {
                        //btnRefresh_OnClick(sender, e);
                    }
                    else
                    {
                        //Hashtable hshIn = new Hashtable();
                        //Hashtable hshtableout = new Hashtable();
                        //hshIn.Add("@intAppointmentId", strAppId[0]);
                        //hshIn.Add("@intEncodedBy", Session["UserId"]);
                        ////hshIn.Add("@intStatusId", common.myStr(e.MenuItem.Attributes["Code"]));
                        //hshIn.Add("@intStatusId", ViewState["MenuItemValue"].ToString());
                        //hshtableout.Add("@intRecAppointmentId", SqlDbType.Int);
                        //hshIn.Add("@dtAppointmentDate", dtStart);


                        //hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspChangeRecurringAppointmentStatus", hshIn, hshtableout);
                        Hashtable hshtableout = objwcfOt.ChangeRecurringAppointmentStatus(common.myInt(strAppId[0]), common.myInt(Session["UserId"]), common.myInt(ViewState["MenuItemValue"]), common.myDate(dtStart));
                        string strUpdateRecID = hshtableout["@intRecAppointmentId"].ToString();
                        string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                        OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                        string strOrigDate = "select AppointmentDate from DoctorAppointment where appointmentid='" + Convert.ToInt32(hshtableout["intRecAppointmentId"]) + "'";

                        //DateTime dtStart = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                        string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";

                        //if (Convert.ToInt64(Request.QueryString["appid"]) != 0)
                        //{                

                        if (!OrigRecRule.Contains("EXDATE"))
                        {
                            string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                            // yashlok  //string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                            //string strUpdateNote = "Update doctorappointment set Note= '" + txtCancelationReasons.Text + "' where appointmentid='" + strUpdateRecID + "'";
                            //dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                            //dl.ExecuteNonQuery(CommandType.Text, strUpdateNote);                            
                            Hashtable hshIn = new Hashtable();
                            strSQLQ = new StringBuilder();
                            hshIn.Add("@strUpdateRule", UpdateRule);
                            hshIn.Add("@strAppId", strAppId[0]);
                            strSQLQ.Append("Update doctorappointment set RecurrenceRule=@strUpdateRule where appointmentid=@strAppId");
                            dl.ExecuteNonQuery(CommandType.Text, strSQLQ.ToString(), hshIn);
                            Hashtable hshInN = new Hashtable();

                            hshInN.Add("@strCancelationReasons", txtCancelationReasons.Text);
                            hshInN.Add("@strUpdateRecID", strUpdateRecID);
                            strSQLQ.Append("Update doctorappointment set Note=@strCancelationReasons  where appointmentid=@strUpdateRecID");
                            dl.ExecuteNonQuery(CommandType.Text, strSQLQ.ToString(), hshInN);
                        }
                        else
                        {
                            string UpdateRule = OrigRecRule + "," + exdate;
                            //string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                            //string strUpdateNote = "Update doctorappointment set  Note='" + txtCancelationReasons.Text + "' where appointmentid='" + strUpdateRecID + "'";
                            //dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                            //dl.ExecuteNonQuery(CommandType.Text, strUpdateNote);
                            Hashtable hshIn = new Hashtable();
                            strSQLQ = new StringBuilder();
                            hshIn.Add("@strUpdateRule", UpdateRule);
                            hshIn.Add("@strAppId", strAppId[0]);
                            strSQLQ.Append("Update doctorappointment set RecurrenceRule=@strUpdateRule where appointmentid=@strAppId");
                            dl.ExecuteNonQuery(CommandType.Text, strSQLQ.ToString(), hshIn);
                            Hashtable hshInN = new Hashtable();
                            hshInN.Add("@strCancelationReasons", txtCancelationReasons.Text);
                            hshInN.Add("@strUpdateRecID", strUpdateRecID);
                            strSQLQ.Append("");
                            dl.ExecuteNonQuery(CommandType.Text, "Update doctorappointment set Note=@strCancelationReasons  where appointmentid=@strUpdateRecID", hshInN);

                        }
                        // btnRefresh_OnClick(sender, e);
                    }
                }
                else
                {
                    //Hashtable hshIn = new Hashtable();
                    //hshIn.Add("@intAppointmentId", strAppId[0]);
                    //hshIn.Add("@intEncodedBy", Session["UserId"]);
                    ////hshIn.Add("@intStatusId", common.myStr(e.MenuItem.Attributes["Code"]));
                    //hshIn.Add("@intStatusId", ViewState["MenuItemValue"]);
                    //dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);

                    objwcfOt.ChangeAppointmentStatus(common.myInt(strAppId[0]), common.myInt(Session["UserId"]), common.myInt(ViewState["MenuItemValue"]));

                    ////string strUpdateRecRule = "Update doctorappointment set Note='" + txtCancelationReasons.Text + "' where appointmentid='" + strAppId[0] + "'";
                    //// int i = dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);                   
                    Hashtable hshIn = new Hashtable();
                    hshIn.Add("@strCancelationReasons", txtCancelationReasons.Text);
                    hshIn.Add("@strAppId", strAppId[0]);
                    int i = dl.ExecuteNonQuery(CommandType.Text, "Update doctorappointment set Note=@strCancelationReasons where appointmentid=@strAppId", hshIn);
                    // btnRefresh_OnClick(sender, e);
                }

                //btnRefresh_OnClick(sender, e);

                ViewState["MenuItemValue"] = null;
                ViewState["MenuAppID"] = null;
                ViewState["MenuRecurrenceParentID"] = null;
                ViewState["MenuRecurrenceState"] = null;
            }

            btnRefresh_OnClick(sender, null);
            txtCancelationReasons.Text = "";
            dvUpdateStatus.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCancelUpdateStatus_Click(object sender, EventArgs e)
    {
        ViewState["MenuAppID"] = null;
        ViewState["MenuItemValue"] = null;
        ViewState["MenuSdate"] = null;
        ViewState["MenuRecurrenceParentID"] = null;
        ViewState["MenuRecurrenceState"] = null;
        txtCancelationReasons.Text = "";
        dvUpdateStatus.Visible = false;
    }

    protected void RadScheduler1_NavigationCommand(object sender, SchedulerNavigationCommandEventArgs e)
    {
        try
        {
            if (e.Command == SchedulerNavigationCommand.SwitchToDayView)
            {
                ViewState["CurrentView"] = "D";
            }
            else if (e.Command == SchedulerNavigationCommand.SwitchToWeekView)
            {
                ViewState["CurrentView"] = "W";
            }
            else if (e.Command == SchedulerNavigationCommand.SwitchToMonthView)
            {
                ViewState["CurrentView"] = "M";
            }
            btnRefresh_OnClick(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnbtnprint_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            //foreach (RadListBoxItem Item in RadLstDoctor.Items)
            //{
            //    if (Item.Checked == true)
            //    {
            //        if (Convert.ToString(ViewState["DocId"]) == "")
            //            ViewState["DocId"] = Item.Value;
            //        else
            //            ViewState["DocId"] += "," + Item.Value;
            //        ViewState["docname"] = Item.Text;

            //    }
            //}
            String value = String.Empty;
            CheckBox checkbox = new CheckBox();
            foreach (RadComboBoxItem currentItem in ddlTheater.Items)
            {
                value = currentItem.Value;
                string strName = currentItem.Text;
                checkbox = (CheckBox)currentItem.FindControl("chk1");
                if (checkbox.Checked == true)
                {
                    if (Convert.ToString(ViewState["DocId"]) == "")
                        ViewState["DocId"] = value;
                    else
                        ViewState["DocId"] += "," + value;
                    ViewState["docname"] = strName;
                }
            }
            //if (i > 1)
            //{
            //    Alert.ShowAjaxMsg("Please Select Only One Provider At a Time", Page);
            //    return;
            //}
            //else
            //{
            #region Log is CPT entered is E&M code and clinical summary is printed or patient portal is active
            // Hashtable logHash = new Hashtable();
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //logHash.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));
            //logHash.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
            //logHash.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
            //logHash.Add("@intDoctorId", Convert.ToInt32(Session["DoctorID"]));
            //logHash.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
            //logHash.Add("@intFacilityID", Convert.ToInt32(Session["FacilityID"]));
            //objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogCPTInE&MAndClinicalSummary", logHash);

            objwcfOt.EMRMUDLogCPTInEAndMAndClinicalSummary(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]), common.myInt(Session["DoctorID"]), common.myInt(Session["UserID"]), common.myInt(Session["FacilityID"]));
            #endregion

            RadWindowForNew.NavigateUrl = "/Appointment/PrintAppointmentscheduleV1.aspx?FacilityName=" + ddlFacility.SelectedItem.Text + "&ProviderName=" + Convert.ToString(ViewState["docname"]) + "&Appdate=" + dtpDate.SelectedDate.Value.ToString("yyyy/MM/dd") + "&DocId=" + Convert.ToString(ViewState["DocId"]) + "&FacId=" + ddlFacility.SelectedValue + "";
            RadWindowForNew.Height = 540;
            RadWindowForNew.Width = 800;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.ReloadOnShow = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnDeleteAllBreak_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string[] strAppId = ViewState["AppId"].ToString().Split('_');
            DateTime origAppTime = DateTime.Now;
            DateTime origAppDate = DateTime.Today;
            if (ViewState["AppId"].ToString().Contains('_') || ViewState["Rec"] != "")
            {
                DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
                //string recrule1 = e.Appointment.Attributes["RecurrenceRule"];                
                ////string strOrigTime = "select BreakDate from BreakList where ID='" + strAppId[0] + "'";
                ////origAppTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                Hashtable hshIn = new Hashtable();
                strSQLQ = new StringBuilder();
                hshIn.Add("@strAppId", strAppId[0]);
                strSQLQ.Append("select BreakDate from BreakList where ID=@strAppId");
                origAppTime = Convert.ToDateTime(dl.ExecuteNonQuery(CommandType.Text, strSQLQ.ToString(), hshIn));

                ////string strOrigDate = "select BreakDate from BreakList where ID='" + strAppId[0] + "'";
                ////origAppDate = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                Hashtable hshInN = new Hashtable();
                strSQLQ = new StringBuilder();
                hshIn.Add("@strAppId", strAppId[0]);
                strSQLQ.Append("select BreakDate from BreakList where ID=@strAppId");
                origAppDate = Convert.ToDateTime(dl.ExecuteNonQuery(CommandType.Text, strSQLQ.ToString(), hshInN));

                TimeSpan daysCount = dtStart.Subtract(origAppDate);

                string exdate = "UNTIL=" + dtStart.ToString("yyyyMMdd") + "T000000Z;";

                ////string origrule = "Select RecurrenceRule from BreakList where ID='" + strAppId[0] + "'";
                //// string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                Hashtable hshInV = new Hashtable();
                strSQLQ = new StringBuilder();
                hshIn.Add("@strAppId", strAppId[0]);
                strSQLQ.Append("Select RecurrenceRule from BreakList where ID=@strAppId");
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, strSQLQ.ToString(), hshInV).ToString();

                if (!OrigRecRule.Contains("COUNT"))
                {
                    if (!OrigRecRule.Contains("UNTIL"))
                    {
                        //int newrule = OrigRecRule.IndexOf("INTERVAL");
                        string newRecRule = OrigRecRule.Insert(OrigRecRule.IndexOf("INTERVAL"), exdate);
                        ////  string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newRecRule + "' where ID='" + strAppId[0] + "'";
                        //// dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        Hashtable hshInV1 = new Hashtable();
                        strSQLQ = new StringBuilder();
                        hshInV1.Add("@strnewRecRule", newRecRule);
                        hshInV1.Add("@strAppId", strAppId[0]);
                        strSQLQ.Append("Update BreakList set RecurrenceRule=@strnewRecRule where ID=@strAppId");
                        dl.ExecuteScalar(CommandType.Text, strSQLQ.ToString(), hshInV1);
                    }
                    else
                    {
                        //int newrule = OrigRecRule.IndexOf("UNTIL");
                        string newrule = OrigRecRule.Insert(OrigRecRule.IndexOf("UNTIL"), exdate);
                        ////string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newrule + "' where ID='" + strAppId[0] + "'";
                        ////dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        Hashtable hshInV2 = new Hashtable();
                        strSQLQ = new StringBuilder();
                        hshInV2.Add("@strNewrule", newrule);
                        hshInV2.Add("@strAppId", strAppId[0]);
                        strSQLQ.Append("Update BreakList set RecurrenceRule=@strNewrule where ID=@strAppId");
                        dl.ExecuteScalar(CommandType.Text, strSQLQ.ToString(), hshInV2);
                        Hashtable hshInV4 = new Hashtable();
                        strSQLQ = new StringBuilder();
                        hshInV4.Add("@strNewrule", newrule);
                        hshInV4.Add("@strAppId", strAppId[0]);
                        strSQLQ.Append("Update BreakList set RecurrenceRule=@strNewrule where ID=@strAppId");
                        dl.ExecuteScalar(CommandType.Text, strSQLQ.ToString(), hshInV4);
                    }
                }
                else
                {
                    if (OrigRecRule.Contains("COUNT"))
                    {
                        int noOfChars = OrigRecRule.IndexOf("INTERVAL") - OrigRecRule.IndexOf("COUNT");
                        string repUntil = OrigRecRule.Remove(OrigRecRule.IndexOf("COUNT"), noOfChars);
                        string newRecRule = repUntil.Insert(repUntil.IndexOf("INTERVAL"), exdate);
                        ////string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newRecRule + "' where ID='" + strAppId[0] + "'";
                        ////dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        Hashtable hshInV3 = new Hashtable();
                        strSQLQ = new StringBuilder();
                        hshInV3.Add("@strNewRule", newRecRule);
                        hshInV3.Add("@strAppId", strAppId[0]);
                        // strSQLQ.Append("Update BreakList set RecurrenceRule=@strNewrule where ID=@strAppId");
                        dl.ExecuteScalar(CommandType.Text, "Update BreakList set RecurrenceRule=@strNewRule where ID=@strAppId", hshInV3);
                    }
                }
            }
            else
            {
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@intBreakId", strAppId[0]);
                hshIn.Add("@intLastChangedBy", Session["UserId"]);
                hshIn.Add("@chvLastChangedDate", DateTime.Now);
                dl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd Update BreakList Set Active=0,LastChangedBy=@intLastChangedBy, LastChangedDate=@chvLastChangedDate where Id=@intBreakId", hshIn);
            }
            divDeleteBreak.Visible = false;
            btnRefresh_OnClick(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        divDeleteBreak.Visible = false;
    }

    protected void btnDeleteBreak_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string[] strAppId = ViewState["AppId"].ToString().Split('_');
            Hashtable hshIn = new Hashtable();
            if (ViewState["AppId"].ToString().Contains('_') || ViewState["Rec"] != null)
            {
                //SchedulerNavigationCommand.SwitchToSelectedDay
                DateTime OrigRecTime = DateTime.Now;
                ////  string strOrigRecTime = "select StartTime from BreakList where ID='" + strAppId[0] + "'";
                hshIn = new Hashtable();
                hshIn.Add("@strAppId", strAppId[0]);
                OrigRecTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, "select StartTime from BreakList where ID=@strAppId", hshIn));

                DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
                string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigRecTime.ToString("HHmmss") + "Z";

                // // string origrule = "Select RecurrenceRule from BreakList where ID='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, "Select RecurrenceRule from BreakList where ID=@strAppId", hshIn).ToString();
                if (!OrigRecRule.Contains("EXDATE"))
                {
                    string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                    ////string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + UpdateRule + "' where ID='" + strAppId[0] + "'";
                    hshIn = new Hashtable();
                    hshIn.Add("@strUpdateRule", UpdateRule);
                    hshIn.Add("@strAppId", strAppId[0]);
                    dl.ExecuteNonQuery(CommandType.Text, "Update BreakList set RecurrenceRule=@strUpdateRule  where ID=@strAppId", hshIn);
                }
                else
                {
                    string UpdateRule = OrigRecRule + "," + exdate;
                    //// string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + UpdateRule + "' where ID='" + strAppId[0] + "'";
                    hshIn = new Hashtable();
                    hshIn.Add("@strUpdateRule", UpdateRule);
                    hshIn.Add("@strAppId", strAppId[0]);
                    dl.ExecuteNonQuery(CommandType.Text, "Update BreakList set RecurrenceRule=@strUpdateRule where ID=@strAppId", hshIn);
                }
            }
            else
            {
                //Hashtable hshIn = new Hashtable();
                hshIn.Add("@intBreakId", strAppId[0]);
                hshIn.Add("@intLastChangedBy", Session["UserId"]);
                hshIn.Add("@chvLastChangedDate", DateTime.Now);
                dl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd Update BreakList Set Active=0,LastChangedBy=@intLastChangedBy, LastChangedDate=@chvLastChangedDate where Id=@intBreakId", hshIn);
            }
            divDeleteBreak.Visible = false;
            btnRefresh_OnClick(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnDeleteThisApp_Click(object sender, EventArgs e)
    {
        objwcfOt = new BaseC.RestFulAPI(sConString);
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string[] strAppId = ViewState["AppId"].ToString().Split('_');
            Hashtable hshIn = new Hashtable();
            if (btnDeleteThisApp.CommandArgument == "DELETE")
            {
                if (txtCancelRemarks.Text == "")
                {
                    Alert.ShowAjaxMsg("Please type Cancel Reason", Page);
                    return;
                }
                objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(strAppId[0]), common.myStr(ViewState["StatusCode"]), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]));
                BaseC.clsOTBooking objO = new BaseC.clsOTBooking(sConString);
                objO.cancelOTBreak(common.myInt(strAppId[0]), txtCancelRemarks.Text, common.myInt(Session["UserId"]));
            }
            if (btnDeleteThisApp.CommandArgument == "OT-CANCEL")
            {
                if (txtCancelRemarks.Text == "")
                {
                    Alert.ShowAjaxMsg("Please type Cancel Reason", Page);
                    return;
                }
                objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(strAppId[0]), common.myStr(ViewState["StatusCode"]), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]));
                objwcfOt.CancelThisApp(common.myInt(strAppId[0]), txtCancelRemarks.Text, common.myInt(Session["UserId"]), common.myInt(ddlReason.SelectedValue));
            }
            if (btnDeleteThisApp.CommandArgument == "OT-UNCHKIN")
            {
                if (ddlReason.SelectedIndex == 0)
                {
                    Alert.ShowAjaxMsg("Please Select Uncheck-in Reason", Page);
                    return;
                }
                else if (txtCancelRemarks.Text == "")
                {
                    Alert.ShowAjaxMsg("Please Enter Uncheck-in Reason", Page);
                    return;
                }
                // string CancelRemarks = common.myStr(ddlFacility.SelectedValue);
                int inResult = objwcfOt.OTUncheckinThisApp(common.myInt(strAppId[0]), txtCancelRemarks.Text, common.myInt(Session["UserId"]));
                if (inResult == 0)
                {
                    objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(strAppId[0]), common.myStr(ViewState["StatusCode"]), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]), txtCancelRemarks.Text, txtUnplannedSurgeryRemarks.Text);
                }
            }
            if (btnDeleteThisApp.CommandArgument == "OT-PAC")
            {

                if (txtCancelRemarks.Text == "")
                {
                    Alert.ShowAjaxMsg("Please Enter Remarks", Page);
                    return;
                }
                StringBuilder strXML = new StringBuilder();
                ArrayList coll = new ArrayList();
                coll.Add(common.myInt(strAppId[0]));
                coll.Add(1);
                strXML.Append(common.setXmlTable(ref coll));

                //objwcfOt.CancelThisApp(common.myInt(strAppId[0]), txtCancelRemarks.Text, common.myInt(Session["UserId"]));
                lblMessage.Text = objwcfOt.SaveOTBookingBillClearance(common.myInt(Session["HospitalLocationID"]), strXML.ToString(), common.myInt(Session["UserID"]), "PAC", common.myStr(txtCancelRemarks.Text));

                if (common.myStr(lblMessage.Text).ToUpper().Contains("SAVE"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                }

            }
            else if (btnDeleteThisApp.CommandArgument.Equals("OT-CONF") || btnDeleteThisApp.CommandArgument.Equals("OT-CHKIN"))
            {
                objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(strAppId[0]), ViewState["StatusCode"].ToString(), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]));

                ////   objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(strAppId[0]), "OT-CHKIN", common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]));

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
    protected void oldbtnDeleteThisApp_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string[] strAppId = ViewState["AppId"].ToString().Split('_');
            Hashtable hshIn = new Hashtable();
            if (btnDeleteThisApp.CommandArgument == "DELETE")
            {
                if (txtCancelRemarks.Text == "")
                {
                    Alert.ShowAjaxMsg("Please type Cancel Reason", Page);
                    return;
                }
                BaseC.clsOTBooking objO = new BaseC.clsOTBooking(sConString);
                objO.cancelOTBreak(common.myInt(strAppId[0]), txtCancelRemarks.Text, common.myInt(Session["UserId"]));
            }
            if (btnDeleteThisApp.CommandArgument == "OT-CANCEL")
            {
                if (txtCancelRemarks.Text == "")
                {
                    Alert.ShowAjaxMsg("Please type Cancel Reason", Page);
                    return;
                }
                objwcfOt.CancelThisApp(common.myInt(strAppId[0]), txtCancelRemarks.Text, common.myInt(Session["UserId"]), common.myInt(ddlReason.SelectedValue));
            }
            if (btnDeleteThisApp.CommandArgument == "OT-UNCHKIN")
            {
                if (ddlReason.SelectedIndex == 0)
                {
                    Alert.ShowAjaxMsg("Please Select Uncheck-in Reason", Page);
                    return;
                }
                else if (txtCancelRemarks.Text == "")
                {
                    Alert.ShowAjaxMsg("Please Enter Uncheck-in Reason", Page);
                    return;
                }

                int inResult = objwcfOt.OTUncheckinThisApp(common.myInt(strAppId[0]), txtCancelRemarks.Text, common.myInt(Session["UserId"]));
                if (inResult == 0)
                {
                    objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(strAppId[0]), ViewState["StatusCode"].ToString(), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]));
                }
            }
            if (btnDeleteThisApp.CommandArgument == "OT-PAC")
            {

                if (txtCancelRemarks.Text == "")
                {
                    Alert.ShowAjaxMsg("Please Enter Remarks", Page);
                    return;
                }
                StringBuilder strXML = new StringBuilder();
                ArrayList coll = new ArrayList();
                coll.Add(common.myInt(strAppId[0]));
                coll.Add(1);
                strXML.Append(common.setXmlTable(ref coll));

                //objwcfOt.CancelThisApp(common.myInt(strAppId[0]), txtCancelRemarks.Text, common.myInt(Session["UserId"]));
                objwcfOt.SaveOTBookingBillClearance(common.myInt(Session["HospitalLocationID"]), strXML.ToString(), common.myInt(Session["UserID"]), "PAC", common.myStr(txtCancelRemarks.Text));
            }
            else if (btnDeleteThisApp.CommandArgument == "OT-CONF")
            {
                objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(strAppId[0]), ViewState["StatusCode"].ToString(), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]));

                ////   objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(strAppId[0]), "OT-CHKIN", common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]));

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

    protected void btnCancelApp_Click(object sender, EventArgs e)
    {
        dvDelete.Visible = false;
    }

    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        setdate();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            lblMsg.Text = string.Empty;
            if (dtDateTimeForOT.SelectedDate == null)
            {

                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMsg.Text = "Please select date !";
                return;
            }
            else
            {

                string sd = Convert.ToDateTime(dtDateTimeForOT.SelectedDate).ToString("HH");

                if (common.myInt(sd) <= 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "Please select date with time!";
                    return;
                }
            }

            BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
            DataSet ds1 = objOT.GetCheckITOT(common.myStr(ViewState["OTBookingid"]), common.myInt(Session["FacilityId"]));
            if (ds1.Tables.Count > 0)
            {

                if (ds1.Tables[0].Rows[0]["OTCheckintime"].ToString() == "" && common.myInt(ViewState["inot"]) == 2)
                {
                    Alert.ShowAjaxMsg("Please update OT check in time !", this.Page);
                    return;
                }

                if (ds1.Tables[0].Rows[0]["OTCheckintime"].ToString() != "" && common.myInt(ViewState["inot"]) == 2)
                {

                    if (Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckintime"]) > Convert.ToDateTime(dtDateTimeForOT.SelectedDate))
                    {
                        Alert.ShowAjaxMsg("OT check out time must be greater than check in time!", this.Page);
                        return;
                    }

                    if (common.myStr(hdnCheckoutTimecheckIgnored.Value) != "Y")
                    {
                        if (Convert.ToDateTime(dtDateTimeForOT.SelectedDate).Day != Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckintime"]).Day)
                        {
                            //Alert.ShowAjaxMsg("OT Check-In Date And Check-Out Date are Different, Please Check! ", this.Page);
                            hdnCheckoutTimecheckIgnored.Value = "Y";
                            // return;
                        }
                    }

                }

                if (ds1.Tables[0].Rows[0]["OTCheckouttime"].ToString() != "" && common.myInt(ViewState["inot"]) == 1)
                {

                    if (Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckouttime"]) < Convert.ToDateTime(dtDateTimeForOT.SelectedDate))
                    {
                        Alert.ShowAjaxMsg("OT check in time must be less than to check out time!", this.Page);
                        return;
                    }

                }
            }



            string strVal = "";
            if (common.myInt(ViewState["inot"]) == 1)
            {
                strVal = "In";
            }
            if (common.myInt(ViewState["inot"]) == 2)
            {
                strVal = "Out";
            }

            DateTime dt = DateTime.Today;
            if (Convert.ToDateTime(dtDateTimeForOT.SelectedDate) > dt.AddDays(1))
            {

                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMsg.Text = "OT Check " + strVal + " datetime should not be greater than current datetime!";
                return;
            }

            if (common.myInt(ViewState["inot"]).Equals(1)) /*|| common.myInt(ViewState["inot"]).Equals(2))*/
            {
                ;
                if (Convert.ToDateTime(dtDateTimeForOT.SelectedDate) > DateTime.Now)
                {

                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMsg.Text = "OT Check " + strVal + " datetime should not be greater than current datetime!";
                    return;
                }

            }

            BaseC.clsOTBooking objOTEncision = new BaseC.clsOTBooking(sConString);
            DataSet dsen = new DataSet();
            dsen = objOTEncision.GetSurgeryOtherDetails(common.myInt(ViewState["OTBookingid"]));

            if (dsen.Tables.Count > 0)
            {


                if (dsen.Tables[0].Rows[0]["IntubationStartTime"].ToString() != "" && common.myInt(ViewState["inot"]) == 2)
                {

                    if (common.myDate(dsen.Tables[0].Rows[0]["IntubationStartTime"]) > common.myDate(dtDateTimeForOT.SelectedDate))
                    {
                        Alert.ShowAjaxMsg("OT check out time must be greater than Intubation Start Time!", this.Page);
                        return;
                    }
                }

                if (dsen.Tables[0].Rows[0]["IncisionTime"].ToString() != "" && common.myInt(ViewState["inot"]) == 2)
                {

                    if (common.myDate(dsen.Tables[0].Rows[0]["IncisionTime"]) > common.myDate(dtDateTimeForOT.SelectedDate))
                    {
                        Alert.ShowAjaxMsg("OT check out time must be greater than Incision Time!", this.Page);
                        return;
                    }

                }

                if (dsen.Tables[0].Rows[0]["AntibioticProphylacticTime"].ToString() != "" && common.myInt(ViewState["inot"]) == 2)
                {
                    if (common.myDate(dsen.Tables[0].Rows[0]["AntibioticProphylacticTime"]) > common.myDate(dtDateTimeForOT.SelectedDate))
                    {
                        Alert.ShowAjaxMsg("OT check out time must be greater than Antibiotic Prophylactic Time!", this.Page);
                        return;
                    }
                }

                if (dsen.Tables[0].Rows[0]["BallooningTime"].ToString() != "" && common.myInt(ViewState["inot"]) == 2)
                {
                    if (common.myDate(dsen.Tables[0].Rows[0]["BallooningTime"]) > common.myDate(dtDateTimeForOT.SelectedDate))
                    {
                        Alert.ShowAjaxMsg("OT check out time must be greater than Ballooning Time!", this.Page);
                        return;
                    }
                }

            }


            if (ViewState["inot"] != null && ViewState["OTBookingid"] != null)
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshIn = new Hashtable();

                if (common.myInt(ViewState["inot"]).Equals(1))
                {
                    Hashtable hshInCHK = new Hashtable();
                    StringBuilder sbCHk = new StringBuilder();
                    DataSet dsCHK = new DataSet();

                    bool IsAllow = true;
                    string strAdmissionDateTime = string.Empty;
                    try
                    {


                        hshInCHK.Add("@dtOTDateTime", Convert.ToDateTime(dtDateTimeForOT.SelectedDate).ToString("yyyy-MM-dd HH:mm:00"));
                        hshInCHK.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
                        hshInCHK.Add("@intOTBookingId", common.myInt(ViewState["OTBookingid"]));

                        sbCHk.Append("SELECT(CASE WHEN ot.EncounterID IS NOT NULL AND DATEADD(MINUTE, -1 * fm.TimeZoneOffSetMinutes, @dtOTDateTime) < enc.EncounterDate then 0 ELSE 1 END) AS IsAllow,");
                        sbCHk.Append(" dbo.GetDateFormatUTC(enc.EncounterDate, 'DT', fm.TimeZoneOffSetMinutes) as AdmissionDateTime");
                        sbCHk.Append(" FROM OTBooking ot");
                        sbCHk.Append(" INNER JOIN Encounter enc ON ot.EncounterID = enc.Id AND enc.Active = 1");
                        sbCHk.Append(" INNER JOIN FacilityMaster fm ON enc.FacilityId = fm.FacilityId AND fm.Active = 1");
                        sbCHk.Append(" WHERE ot.OTBookingId = @intOTBookingId");
                        sbCHk.Append(" AND ot.FacilityID = @intFacilityId");
                        sbCHk.Append(" AND enc.FacilityID = @intFacilityId");

                        dsCHK = dl.FillDataSet(CommandType.Text, sbCHk.ToString(), hshInCHK);
                        if (dsCHK.Tables.Count > 0)
                        {
                            if (dsCHK.Tables[0].Rows.Count > 0)
                            {


                                if (!common.myBool(dsCHK.Tables[0].Rows[0]["IsAllow"]).Equals(true))
                                {

                                    IsAllow = false;
                                    strAdmissionDateTime = common.myStr(dsCHK.Tables[0].Rows[0]["AdmissionDateTime"]);
                                }

                            }
                        }

                        if (!IsAllow)
                        {
                            Alert.ShowAjaxMsg("OT Check In datetime should be greater than admission datetime (" + strAdmissionDateTime + ")!", this.Page);
                            return;
                        }
                    }
                    catch (Exception Ex)
                    {

                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMsg.Text = "Error: " + Ex.Message;
                        objException.HandleException(Ex);

                    }
                    finally
                    {
                        hshInCHK = null;
                        sbCHk = null;
                        dsCHK.Dispose();
                    }
                }

                DataSet ds = objwcfOt.GetTimeZone(common.myInt(Session["FacilityId"]));
                int timezone = 0;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    timezone = Convert.ToInt32(ds.Tables[0].Rows[0]["TimeZoneOffSetMinutes"].ToString());
                }

                hshIn.Add("@inoutDate", Convert.ToDateTime(dtDateTimeForOT.SelectedDate).AddMinutes(-1 * timezone));
                hshIn.Add("@OTBookingid", ViewState["OTBookingid"]);
                int x = 0;
                if (rblIsUnplannedOT.SelectedIndex == 0)
                {
                    hshIn.Add("@IsUnplannedReturn", common.myStr(1));
                }
                else
                {
                    hshIn.Add("@IsUnplannedReturn", common.myStr(0));
                    x = 1;
                }
                if (x == 0)
                {
                    if (rblIsChargeAbleOT.SelectedIndex == 0)
                    {
                        hshIn.Add("@IsOtchargeAble", common.myStr(1));
                    }
                    else
                    {
                        hshIn.Add("@IsOtchargeAble", common.myStr(0));
                    }
                }
                else
                {
                    rblIsChargeAbleOT.SelectedIndex = 0;
                    hshIn.Add("@IsOtchargeAble", common.myStr(1));
                }


                hshIn.Add("@AnotherTheaterId", common.myStr(ddlAnotherOT.SelectedValue));
                hshIn.Add("@UnplannedSurgeryRemarks", common.myStr(txtUnplannedSurgeryRemarks.Text));

                if (common.myStr(ViewState["inot"]) == "1")
                {

                    int intResult = dl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd update OTBooking set OTCheckintime=@inoutDate,IsUnplannedReturnToOT=@IsUnplannedReturn,IsUnplannedOTChargeable=@IsOtchargeAble, AnotherTheaterId=@AnotherTheaterId, UnplannedSurgeryRemarks=@UnplannedSurgeryRemarks where OTBookingID=@OTBookingid", hshIn);

                    if (intResult == 0)
                    {
                        // Patient Chek-In 
                        if ((common.myStr(ViewState["EditCheckIN"]) == "N") || (ViewState["EditCheckIN"] == null))
                        {
                            objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(ViewState["OTBookingid"]), "OT-CHKIN", common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]), "", txtUnplannedSurgeryRemarks.Text);
                        }
                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblMsg.Text = "Check in time updated";
                    }
                    else
                    {
                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMsg.Text = "Some Errror...";
                    }
                }
                else if (common.myStr(ViewState["inot"]) == "2")
                {

                    int intResult = dl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd update OTBooking set OTCheckouttime=@inoutDate, UnplannedSurgeryRemarks=@UnplannedSurgeryRemarks  where OTBookingID=@OTBookingid", hshIn);

                    if (intResult == 0)
                    {
                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblMsg.Text = "Check out time updated";
                    }
                    else
                    {
                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMsg.Text = "Some Errror...";
                    }
                }

                //    objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(ViewState["OTBookingid"]), common.myStr(e.MenuItem.Attributes["Code"]), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["UserID"]));

            }

            btnRefresh_OnClick(this, null);
            hdnCheckoutTimecheckIgnored.Value = "N";
            //----------- Added these lines to auto close div after pressing submit button
            divOTCheckinCheckoutTime.Visible = false;
            ddlAnotherOT.ClearSelection();

        }
        catch (Exception Ex)
        {

            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divOTCheckinCheckoutTime.Visible = false;
        ddlAnotherOT.ClearSelection();
    }

    //private void bindminut()
    //{
    //    ddlminut.Items.Clear();  
    //    for(int i=0; i<60; i++)
    //    {
    //        if(i<10)
    //            ddlminut.Items.Add("0"+common.myStr(i));    
    //        else
    //            ddlminut.Items.Add(common.myStr(i));    

    //    }


    //}

    //protected void ddlminut_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    setdate();

    //}

    protected void setdate()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(dtDateTimeForOT.SelectedDate.Value.ToString());
            sb.Remove(dtDateTimeForOT.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
            sb.Insert(dtDateTimeForOT.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.SelectedItem.Text);
            dtDateTimeForOT.SelectedDate = Convert.ToDateTime(sb.ToString());
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnPatientSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            btnRefresh_OnClick(null, null);
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    protected void rblIsUnplannedOT_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblIsUnplannedOT.SelectedIndex == 0)
        {
            lblIschargeAbleOt.Visible = true;
            rblIsChargeAbleOT.Visible = true;
            rblIsChargeAbleOT.SelectedIndex = 0;
            lblUnplannedSurgeryRemarks.Visible = true;
            txtUnplannedSurgeryRemarks.Visible = true;
            Label6.Visible = true;
            ddlAnotherOT.Visible = true;
        }
        else
        {
            lblIschargeAbleOt.Visible = false;
            rblIsChargeAbleOT.Visible = false;
            rblIsChargeAbleOT.SelectedIndex = 1;
            //lblUnplannedSurgeryRemarks.Visible = false;
            //txtUnplannedSurgeryRemarks.Visible = false;
            Label6.Visible = false;
            ddlAnotherOT.Visible = false;
        }
    }

    protected void dtDateTimeForOT_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {

        if (common.myInt(hdnTempData.Value) == 0)
        {
            hideControl();
        }
        else
        {
            rblIsUnplannedOT.SelectedIndex = 1;
            rblIsChargeAbleOT.SelectedIndex = 1;
            BaseC.HospitalSetup baseHs = new BaseC.HospitalSetup(sConString);
            BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
            int x = 0;
            Hashtable ht = new Hashtable();

            //ht = objRest.GetIsunPlannedReturnToOt(common.myInt(Session["myid"]), common.myInt(Session["FacilityId"]), common.myDate(dtDateTimeForOT.SelectedDate));
            ht = objRest.GetIsunPlannedReturnToOt(common.myInt(Session["myid"]), common.myInt(Session["FacilityId"]), Convert.ToDateTime(hdnOTBookingdate.Value));
            string IsOTChargeableOT = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsUnplannedOTChargeable", common.myInt(Session["FacilityId"]));
            if (common.myStr(IsOTChargeableOT).Equals("Y"))
            {

                if (common.myBool(ht["@bitIsUnPlannedReturnToOT"]))
                {
                    lblIsUnplannedSurgery.Visible = true;
                    rblIsUnplannedOT.Visible = true;
                    lblIschargeAbleOt.Visible = true;
                    rblIsChargeAbleOT.Visible = true;
                    rblIsUnplannedOT.SelectedValue = "1";
                    rblIsChargeAbleOT.SelectedValue = "1";
                    lblUnplannedSurgeryRemarks.Visible = true;
                    txtUnplannedSurgeryRemarks.Visible = true;
                    if (!String.IsNullOrEmpty(common.myStr(ht["@chvLastCheckoutTime"])))
                    {
                        lblCheckoutTime.Visible = true;
                        lblDiffTime.Visible = true;
                        lblLastChkOutTime.Text = common.myStr(ht["@chvLastCheckoutTime"]);
                        lblTimeDiff.Text = common.myStr(ht["@intTimeDiffInMin"]) + " " + "hr.";
                    }
                }
                else
                {
                    hideControl();
                }

            }

        }
    }

    private void hideControl()
    {
        lblIsUnplannedSurgery.Visible = false;
        rblIsUnplannedOT.Visible = false;
        lblIschargeAbleOt.Visible = false;
        rblIsChargeAbleOT.Visible = false;
        lblCheckoutTime.Visible = false;
        lblDiffTime.Visible = false;
        Label6.Visible = false;
        ddlAnotherOT.Visible = false;
        //lblUnplannedSurgeryRemarks.Visible = false;
        //txtUnplannedSurgeryRemarks.Visible = false;
    }

    //added by bhakti
    protected void btnOTReqList_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/OTScheduler/OTRequestList.aspx?From=POPUP";
            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            //RadWindowForNew.OnClientClose = "OtRequestOnClientClose";
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}
