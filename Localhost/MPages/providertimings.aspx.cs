using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using Telerik.Web.UI;

public partial class MPages_providertimings : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.Patient pt;
    DAL.DAL dl = new DAL.DAL();
    String sqlstr = "";
    BaseC.ParseData bc = new BaseC.ParseData();
    Boolean bStatus = false;

    private enum GridDoctorVisitDuration : byte
    {
        ID = 0,
        SerialNo = 1,
        VisitType = 2,
        Duration = 3,
        Status = 4,
        Deactivate = 5,
        Edit = 6,
        Active = 7,
        VisitTypeID = 8
    }

    private Int32 DoctorId
    {
        get
        {
            return (ViewState["DoctorId"] == null) ? 0 : Convert.ToInt32(ViewState["DoctorId"]);
        }
        set
        {
            ViewState["DoctorId"] = value;
        }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        if (Session["UserID"] == null)
        {
            Response.Redirect("~/login.aspx?Logout=1", false);
        }
        if (Request.QueryString["Mpg"] != null)
            Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();

        if (!IsPostBack)
        {

            btnsavedoctortime.Text = "Save";
            rdpFromDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            rdpToDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();

            populateFacility();
            PopulateProviders();
            bindBlank();
            ViewState["Mode"] = "New";

            if (Request.QueryString["EmpNo"] != null)
            {
                ddlDoctor.SelectedIndex = ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindItemByValue(common.myStr(Request.QueryString["EmpNo"])));
                ddlDoctor_SelectedIndexChanged(this, null);
                DoctorId = Convert.ToInt32(Request.QueryString["EmpNo"]);
            }
            BindVisitDurationProviderDropDown();
            BindDoctorVisitDurationGrid();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (gvDoctorVisitDuration.EditIndex == -1)
        {
            if (bStatus == false)
            {
                foreach (GridViewRow gvr in gvDoctorVisitDuration.Rows)
                {
                    String strID = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridDoctorVisitDuration.ID)].Text.ToString().Trim());
                    TextBox txtDuration = (TextBox)gvr.Cells[Convert.ToByte(GridDoctorVisitDuration.Duration)].FindControl("txtDoctorVisitDurationGrid1");
                    if (ddlProvider.SelectedValue != "0" && ddlSetDefaultTime.SelectedValue != "0")
                    {
                        txtDuration.Text = ddlSetDefaultTime.SelectedValue.ToString();
                    }

                }
            }
        }
    }

    void PopulateProviders()
    {
        try
        {
            ddlDoctor.Items.Clear();
            Hashtable hshInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInput.Add("@intFacilityId", ddlfacility.SelectedValue);
            DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hshInput);

            if (objDs.Tables[0].Rows.Count > 0)
            {
                ddlDoctor.DataSource = objDs;
                ddlDoctor.DataValueField = "DoctorId";
                ddlDoctor.DataTextField = "DoctorName";
                ddlDoctor.DataBind();
                RadComboBoxItem rci = new RadComboBoxItem("Select");
                ddlDoctor.Items.Insert(0, rci);
            }
            if (ddlDoctor.Items.Count > 0)
            {
                ddlDoctor.SelectedIndex = 0;
                //filldoctortime();
                //if (gvDoctorTime.Rows.Count > 0)
                //{
                //    DoctorId = 0;
                //    btndoctortimeedit_Click(this, null);
                //    if (gvDoctorTime.SelectedIndex != 0)
                //        gvDoctorTime.SelectedIndex = 0;
                //    gvDoctorTime_OnSelectedIndexChanged(this, null);
                //}
                ddlDoctor_SelectedIndexChanged(this, null);
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btndoctortimenew_Click(object sender, EventArgs e)
    {
        try
        {
            cleardoctortime(0);
            ViewState["Mode"] = "New";
            if (gvDoctorTime.SelectedIndex != -1)
                gvDoctorTime.SelectedIndex = -1;
            //txtfromdate.Focus();
            if (hdnNextAppDate.Value != "")
                rdpFromDate.SelectedDate = Convert.ToDateTime(hdnNextAppDate.Value);
            rdpToDate.SelectedDate = null;
            bindBlank();
            btnsavedoctortime.Text = "Save";
            chkNoEnd.Checked = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDoctorTime_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (gvDoctorTime.Rows.Count > 0)
            {
                //string sqlstr;
                Hashtable hshIn = new Hashtable();

                string fromDate = gvDoctorTime.SelectedRow.Cells[2].Text;
                string toDate = gvDoctorTime.SelectedRow.Cells[3].Text;

                String[] sql = gvDoctorTime.SelectedRow.Cells[0].Text.Split(' ');
                cleardoctortime(0);
                hshIn.Add("@HospitalLocationID", Convert.ToInt32(Session["HospitalLocationID"]));
                if (DoctorId > 0)
                    hshIn.Add("@DoctorId", DoctorId);
                else
                    hshIn.Add("@DoctorId", ddlDoctor.SelectedValue);
                hshIn.Add("@DateFrom", common.myDate(fromDate).ToString("yyyy/MM/dd")); //common.myDate(sql[0])
                hshIn.Add("@DateTo", common.myDate(toDate).ToString("yyyy/MM/dd")); //common.myDate(sql[4])
                hshIn.Add("@intFacilityId", Convert.ToInt16(ddlfacility.SelectedValue));
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                if (sql[6].ToString() != "No")
                {
                    sqlstr = "select day, RIGHT(CONVERT(VARCHAR, shiftfirstfrom, 100),7) as shiftfirstfrom, RIGHT(CONVERT(VARCHAR, shiftfirstto, 100),7) as shiftfirstto from doctortime where DoctorId=@DoctorId and datefrom=@DateFrom and isnull(dateto,'')=@DateTo AND FacilityId=@intFacilityId and Active=1";
                    rdpToDate.SelectedDate = common.myDate(toDate); //common.myDate(sql[4]);
                    chkNoEnd.Checked = false;
                    rdpToDate.Enabled = true;
                }
                else
                {
                    sqlstr = "select day, RIGHT(CONVERT(VARCHAR, shiftfirstfrom, 100),7) as shiftfirstfrom, RIGHT(CONVERT(VARCHAR, shiftfirstto, 100),7) as shiftfirstto from doctortime where DoctorId=@DoctorId and datefrom=@DateFrom and dateto is Null AND FacilityId=@intFacilityId and Active=1";
                    rdpToDate.SelectedDate = null;
                    chkNoEnd.Checked = true;
                    rdpToDate.Enabled = false;
                }
                DataSet objDs = dl.FillDataSet(CommandType.Text, sqlstr, hshIn);
                pt = new BaseC.Patient(sConString);
                //string strFromDate = pt.FormatDateDateMonthYear(sql[0]);
                rdpFromDate.SelectedDate = common.myDate(fromDate); //common.myDate(sql[0]);

                if (ddlDoctor.SelectedValue != "")
                {
                    int i = AppointmentSlot(Convert.ToInt32(ddlDoctor.SelectedValue));
                    txtslottiming.Text = i.ToString();
                }
                DataTable objDt = new DataTable();
                objDt.Columns.Add("Day");
                objDt.Columns.Add("shiftfirstfrom");
                objDt.Columns.Add("shiftfirstto");
                objDt.Columns.Add("IsActive");
                //objDt.Columns.Add("shiftsecondto");
                DataRow datarow = objDt.NewRow();
                datarow["Day"] = "MON";
                datarow["IsActive"] = "True";
                objDt.Rows.Add(datarow);
                datarow = objDt.NewRow();
                datarow["Day"] = "TUE";
                datarow["IsActive"] = "True";
                objDt.Rows.Add(datarow);
                datarow = objDt.NewRow();
                datarow["Day"] = "WED";
                datarow["IsActive"] = "True";
                objDt.Rows.Add(datarow);
                datarow = objDt.NewRow();
                datarow["Day"] = "THU";
                datarow["IsActive"] = "True";
                objDt.Rows.Add(datarow);
                datarow = objDt.NewRow();
                datarow["Day"] = "FRI";
                datarow["IsActive"] = "True";
                objDt.Rows.Add(datarow);
                datarow = objDt.NewRow();
                datarow["Day"] = "SAT";
                datarow["IsActive"] = "True";
                objDt.Rows.Add(datarow);
                datarow = objDt.NewRow();
                datarow["Day"] = "SUN";
                datarow["IsActive"] = "True";
                objDt.Rows.Add(datarow);

                if (objDs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < objDt.Rows.Count; i++)
                    {
                        for (int j = 0; j < objDs.Tables[0].Rows.Count; j++)
                        {
                            if (common.myStr(objDt.Rows[i]["Day"]).ToUpper().Equals(common.myStr(objDs.Tables[0].Rows[j]["Day"]).ToUpper()))
                            {
                                objDt.Rows[i]["shiftfirstfrom"] = common.myStr(objDs.Tables[0].Rows[j]["shiftfirstfrom"]);
                                objDt.Rows[i]["shiftfirstto"] = common.myStr(objDs.Tables[0].Rows[j]["shiftfirstto"]);
                                objDt.Rows[i]["IsActive"] = "False";
                            }
                        }
                    }
                }
                gvTiming.DataSource = objDt;
                gvTiming.DataBind();
                btnsavedoctortime.Text = "Update";
                ViewState["Mode"] = "Update";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void populateFacility()
    {
        try
        {
            DataView dv;

            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            DataSet ds = objEMRFacility.GetFacility(Convert.ToInt16(Session["HospitalLocationID"]));
            dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "Active = 1 ";
            ddlfacility.DataSource = dv;
            ddlfacility.DataTextField = "Name";
            ddlfacility.DataValueField = "FacilityId";
            ddlfacility.DataBind();
            //   ddlfacility.Items.Insert(0, new ListItem("[Select]", "0"));
            ddlfacility.Items.Insert(0, new RadComboBoxItem("[Select]", "0"));
            //ddlfacility.Items[0].Value = "0";
            ddlfacility.SelectedIndex = ddlfacility.Items.IndexOf(ddlfacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnsavedoctortime_Click(object sender, EventArgs e)
    {
        try
        {
            string strdoctortime = "";
            DateTime? dtFromDate;
            DateTime? dtToDate;
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            pt = new BaseC.Patient(sConString);
            Hashtable hshtableout = new Hashtable();
            Hashtable hshtablein = new Hashtable();

            if (!rdpFromDate.IsEmpty)
            {
                dtFromDate = (DateTime)rdpFromDate.SelectedDate.Value;
                hshtablein.Add("@chvDateFrom", common.myDate(dtFromDate.Value).ToString("dd/MM/yyyy"));
            }
            else
            {
                Alert.ShowAjaxMsg("Please Select From Date", Page);
                return;
            }
            if (!rdpToDate.IsEmpty)
            {
                if (DateTime.Compare(Convert.ToDateTime(rdpToDate.SelectedDate), Convert.ToDateTime(rdpFromDate.SelectedDate)) == -1)
                {
                    Alert.ShowAjaxMsg("To Date should be greater then from date.", Page);
                    return;
                }

                dtToDate = (DateTime)rdpToDate.SelectedDate.Value;
                hshtablein.Add("@chvDateTo", common.myDate(dtToDate.Value).ToString("dd/MM/yyyy"));
            }
            else
            {
                hshtablein.Add("@chvDateTo", "");
            }






            DataTable dt = new DataTable();

            dt.Columns.Add("Day", typeof(string));
            dt.Columns.Add("FromTime", typeof(string));
            dt.Columns.Add("ToTime", typeof(string));
            //dt.Columns.Add("IsActive", typeof(string));
            //dt.Columns.Add("EveTo", typeof(string));

            DataRow dr;
            foreach (GridViewRow item in gvTiming.Rows)
            {
                dr = dt.NewRow();
                TextBox txtshiftfirstfrom = (TextBox)item.FindControl("txtFromTime");
                TextBox txtshiftfirstto = (TextBox)item.FindControl("txtToTime");
                CheckBox chkIsActive = (CheckBox)item.FindControl("chkIsActive");
                if (chkIsActive.Checked == false)
                {
                    if (txtshiftfirstfrom.Text.Trim() != "" && txtshiftfirstto.Text.Trim() != "")
                    {

                        if (DateTime.Compare(Convert.ToDateTime("1900/01/01 " + txtshiftfirstto.Text.Trim()), Convert.ToDateTime("1900/01/01 " + txtshiftfirstfrom.Text.Trim())) == -1)
                        {
                            Alert.ShowAjaxMsg("To Time should be greater then From Time.", Page);
                            return;
                        }



                        dr["Day"] = item.Cells[0].Text;
                        dr["FromTime"] = txtshiftfirstfrom.Text;
                        dr["ToTime"] = txtshiftfirstto.Text;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Please Enter Start and End Time ! ", Page);
                        return;
                    }

                }
            }
            strdoctortime = bc.MakeXMLString(dt);

            if (ViewState["Mode"].ToString() == "Update")
            {
                hshtablein.Add("@chvDateFromOld", common.myDate(gvDoctorTime.SelectedRow.Cells[2].Text).ToString("yyyy/MM/dd"));
            }
            else
            {
                hshtablein.Add("@chvDateFromOld", "");
            }

            if (DoctorId > 0)
            {
                hshtablein.Add("@intDoctorId", DoctorId);
            }
            else
            {
                if (ddlDoctor.SelectedValue != "" && ddlDoctor.SelectedValue != "0")
                {
                    hshtablein.Add("@intDoctorId", ddlDoctor.SelectedValue);
                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select Doctor", Page);
                    return;
                }
            }

            hshtablein.Add("@intEncodedBy", Session["UserID"]);
            hshtablein.Add("@XMLDoctorTime", strdoctortime);
            if (txtOpdCardString.Text.Length > 0)
            {
                hshtablein.Add("@OPDCardString", txtOpdCardString.Text.Trim());
            }
            else
            {
                hshtablein.Add("@OPDCardString", DBNull.Value);
            }

            if (ddlfacility.SelectedValue != "" && ddlfacility.SelectedValue != "0")
            {
                hshtablein.Add("@intFacilityId", ddlfacility.SelectedValue);
            }
            else
            {
                Alert.ShowAjaxMsg("Please Select Facility", Page);
                return;
            }

            if (Convert.ToInt16(txtslottiming.Text) <= 60)
            {
                hshtablein.Add("@SlotTime", txtslottiming.Text);
            }
            else
            {
                Alert.ShowAjaxMsg("Please enter Appointment Slot less then 60 minutes", Page);
                return;
            }
            hshtableout.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshtablein.Add("@inyHospitalLocationId", Session["HospitalLocationID"].ToString());
            hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateDoctorTime", hshtablein, hshtableout);


            lblMessage.Text = hshtableout["@chvErrorStatus"].ToString();
            filldoctortime();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private int AppointmentSlot(int iDoctorId)
    {
        int iSlot = 0;
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@doctorid", iDoctorId);
            DataSet ds = dl.FillDataSet(CommandType.Text, "select AppointmentSlot from  DoctorDetails where DoctorId =@doctorid", hshIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                iSlot = common.myInt(ds.Tables[0].Rows[0]["AppointmentSlot"]);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return iSlot;
    }

    void filldoctortime()
    {
        try
        {
            DataSet ds = new DataSet();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@doctorid", ddlDoctor.SelectedValue);
            hshIn.Add("@FacilityId", ddlfacility.SelectedValue);
            //populate Bed Category drop down control
            int i;
            if (ddlDoctor.SelectedValue != "0")
            {


                string strSql = "select Distinct dbo.GetDateFormat(DateFrom,'D') + '  To  ' + IsNull(dbo.GetDateFormat(dateto,'D'),'No End date') as DoctorDate ,convert(varchar(10),DateFrom,111) DateFrom,Convert(varchar(10),DateTo,111) DateTo  from doctortime where doctorid = @doctorid And FacilityId = @FacilityId and Active=1";
                strSql = strSql + " select Distinct dbo.GetDateFormat( max(Dateto)+1,'D') as NextAppDate from doctortime where doctorid = @doctorid And FacilityId = @FacilityId";
                strSql = strSql + " select opdCardString from doctorDetails where doctorid = @doctorid ";
                ds = dl.FillDataSet(CommandType.Text, strSql, hshIn);
                i = AppointmentSlot(DoctorId);
            }

            else
            {

                string strSql = "select Distinct dbo.GetDateFormat(DateFrom,'D') + '  To  ' + IsNull(dbo.GetDateFormat(dateto,'D'),'No End date') as DoctorDate ,convert(varchar(10),DateFrom,111) DateFrom,Convert(varchar(10),DateTo,111) DateTo from doctortime where doctorid = @doctorid  And FacilityId = @FacilityId and Active=1";
                strSql = strSql + " select Distinct dbo.GetDateFormat( max(Dateto)+1,'D') as NextAppDate from doctortime where doctorid = @doctorid  And FacilityId = @FacilityId";
                strSql = strSql + " select opdCardString from doctorDetails where doctorid = @doctorid ";
                ds = dl.FillDataSet(CommandType.Text, strSql, hshIn);
                i = AppointmentSlot(Convert.ToInt32(ddlDoctor.SelectedValue));
            }
            hdnNextAppDate.Value = Convert.ToString(ds.Tables[1].Rows[0]["NextAppDate"]);
            //rdpFromDate.SelectedDate = Convert.ToDateTime(ds.Tables[1].Rows[0]["NextAppDate"]);
            gvDoctorTime.DataSource = ds.Tables[0];
            //lstdoctortime.DataTextField = "DoctorDate";
            gvDoctorTime.DataBind();
            //dr.Close();
            if (ds.Tables.Count == 3)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    txtOpdCardString.Text = common.myStr(ds.Tables[2].Rows[0]["OPDcardString"]);
                }
            }
            if (gvDoctorTime.Rows.Count > 0)
            {
                cleardoctortime(0);
                btnsavedoctortime.Enabled = true;
            }
            else
            {
                bindBlank();
                // gvTiming.Columns[3].Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void bindBlank()
    {
        try
        {
            DataTable objDt = new DataTable();
            objDt.Columns.Add("Day");
            objDt.Columns.Add("shiftfirstfrom");
            objDt.Columns.Add("shiftfirstto");
            objDt.Columns.Add("IsActive");

            //objDt.Columns.Add("shiftsecondto");
            DataRow datarow = objDt.NewRow();
            datarow["Day"] = "MON";
            datarow["shiftfirstfrom"] = "08:00 AM";
            datarow["shiftfirstto"] = "06:00 PM";
            datarow["IsActive"] = "False";
            objDt.Rows.Add(datarow);
            datarow = objDt.NewRow();
            datarow["Day"] = "TUE";
            datarow["shiftfirstfrom"] = "08:00 AM";
            datarow["shiftfirstto"] = "06:00 PM";
            datarow["IsActive"] = "False";
            objDt.Rows.Add(datarow);
            datarow = objDt.NewRow();
            datarow["Day"] = "WED";
            datarow["shiftfirstfrom"] = "08:00 AM";
            datarow["shiftfirstto"] = "06:00 PM";
            datarow["IsActive"] = "False";
            objDt.Rows.Add(datarow);
            datarow = objDt.NewRow();
            datarow["Day"] = "THU";
            datarow["shiftfirstfrom"] = "08:00 AM";
            datarow["shiftfirstto"] = "06:00 PM";
            datarow["IsActive"] = "False";
            objDt.Rows.Add(datarow);
            datarow = objDt.NewRow();
            datarow["Day"] = "FRI";
            datarow["shiftfirstfrom"] = "08:00 AM";
            datarow["shiftfirstto"] = "06:00 PM";
            datarow["IsActive"] = "False";
            objDt.Rows.Add(datarow);
            datarow = objDt.NewRow();
            datarow["Day"] = "SAT";
            datarow["shiftfirstfrom"] = "08:00 AM";
            datarow["shiftfirstto"] = "06:00 PM";
            datarow["IsActive"] = "False";
            objDt.Rows.Add(datarow);
            datarow = objDt.NewRow();
            datarow["Day"] = "SUN";
            datarow["shiftfirstfrom"] = "08:00 AM";
            datarow["shiftfirstto"] = "06:00 PM";
            datarow["IsActive"] = "False";
            objDt.Rows.Add(datarow);
            gvTiming.DataSource = objDt;
            gvTiming.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void cleardoctortime(int index)
    {
        try
        {
            if (index != 0)
            {
                TextBox txtFrom1 = (TextBox)gvTiming.Rows[index].FindControl("txtFromTime");
                TextBox txtTo1 = (TextBox)gvTiming.Rows[index].FindControl("txtToTime");
                txtFrom1.Text = "";
                txtTo1.Text = "";
                // txtTo2.Text = "";
            }
            else
            {
                foreach (GridViewRow item in gvTiming.Rows)
                {
                    TextBox txtFrom1 = (TextBox)item.FindControl("txtFromTime");
                    TextBox txtTo1 = (TextBox)item.FindControl("txtToTime");
                    txtFrom1.Text = "";
                    txtTo1.Text = "";
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

    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (gvTiming.SelectedRow == null)
            cleardoctortime(0);
        else
            cleardoctortime(gvTiming.SelectedRow.RowIndex);
    }

    protected void btndoctortimeedit_Click(object sender, EventArgs e)
    {
        if (DoctorId > 0)
        {
            ViewState["Mode"] = "Edit";
            cleardoctortime(0);
            //lstdoctortime.Focus();
            filldoctortime();
        }
    }

    protected void ddlfacility_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateProviders();
    }

    protected void ddlDoctor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDoctor.SelectedValue != "")
        {
            DoctorId = Convert.ToInt32(ddlDoctor.SelectedValue);
        }
        btndoctortimeedit_Click(sender, e);
        if (gvDoctorTime.Rows.Count > 0)
        {
            if (gvDoctorTime.SelectedIndex != 0)
            {
                gvDoctorTime.SelectedIndex = 0;
            }
            gvDoctorTime_OnSelectedIndexChanged(this, null);
        }
    }

    //protected void Tab_SelectionChanged(object sender, EventArgs e)
    //{
    //    if (TabContainer1.ActiveTabIndex == 0)
    //    {
    //        btnDoctorVisitDuration.Visible = false;
    //        btnsavedoctortime.Visible = true;
    //    }
    //    else
    //    {
    //        btnDoctorVisitDuration.Visible = true;
    //        btnsavedoctortime.Visible = false;
    //    }
    //}

    protected void ddlSetDefaultTime_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (gvDoctorVisitDuration.EditIndex == -1)
        {
            foreach (GridViewRow gvr in gvDoctorVisitDuration.Rows)
            {
                String strID = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridDoctorVisitDuration.ID)].Text.ToString().Trim());
                TextBox txtDuration = (TextBox)gvr.Cells[Convert.ToByte(GridDoctorVisitDuration.Duration)].FindControl("txtDoctorVisitDurationGrid1");
                if (ddlProvider.SelectedValue != "0")
                {
                    txtDuration.Text = ddlSetDefaultTime.SelectedValue.ToString();
                }
                else
                {
                    Alert.ShowAjaxMsg("Please select Provider", Page);
                    ddlSetDefaultTime.SelectedValue = "0";
                }
            }
        }
    }

    private void BindDoctorVisitDurationGrid()
    {
        try
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL Dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("inyHospitalLocationID", Session["HospitalLocationID"]);
            if (ddlProvider.SelectedValue != "0")
            {

                HshIn.Add("intDoctorID", ddlProvider.SelectedValue);
                DataSet ds = Dl.FillDataSet(CommandType.StoredProcedure, "UspGetDoctorVisitDuration", HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvDoctorVisitDuration.DataSource = ds;
                    gvDoctorVisitDuration.DataBind();
                }
                else
                {
                    ViewState["BlankGrid"] = "True";
                    BindBlankGrid();
                    ViewState["BlankGrid"] = null;
                }
            }
            else
            {
                ViewState["BlankGrid"] = "True";
                BindBlankGrid();
                ViewState["BlankGrid"] = null;
            }

            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Javascript", "Tab_SelectionChanged(1)", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindBlankGrid()
    {
        try
        {
            DataTable dtDoctorVisitDuration = new DataTable();
            dtDoctorVisitDuration.Columns.Add("ID");
            dtDoctorVisitDuration.Columns.Add("VisitTypeID");
            dtDoctorVisitDuration.Columns.Add("Type");
            dtDoctorVisitDuration.Columns.Add("Status");
            dtDoctorVisitDuration.Columns.Add("Active");
            dtDoctorVisitDuration.Columns.Add("SerialNo");
            dtDoctorVisitDuration.Columns.Add("Duration");
            for (int i = 0; i < 5; i++)
            {
                DataRow Dr = dtDoctorVisitDuration.NewRow();
                Dr["ID"] = "";
                Dr["VisitTypeID"] = "";
                Dr["Type"] = "";
                Dr["Status"] = "";
                Dr["Active"] = "";
                //  Dr["SerialNo"] = "";
                Dr["Duration"] = "";
                dtDoctorVisitDuration.Rows.Add(Dr);
            }
            gvDoctorVisitDuration.DataSource = dtDoctorVisitDuration;
            gvDoctorVisitDuration.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDoctorVisitDuration_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.ID)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Active)].Visible = false;
            if (ViewState["BlankGrid"] != null)
            {
                e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Deactivate)].Visible = false;
                e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Edit)].Visible = false;
            }
            e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.VisitTypeID)].Visible = false;
        }

        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
        {
            String strID = HttpUtility.HtmlDecode(e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.ID)].Text.ToString().Trim());

            Literal ltDuration = (Literal)e.Row.FindControl("ltrlGridDuration");
            if (ltDuration.Text == "0")
            {
                ltDuration.Text = "";
            }
            TextBox txtVisitDuration = (TextBox)e.Row.FindControl("txtDoctorVisitDurationGrid1");
            if (txtVisitDuration.Text == "0")
            {
                txtVisitDuration.Text = "";
            }

            if (strID.ToString().Trim() != "0")
            {
                TextBox txt = (TextBox)e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Status)].FindControl("txtDoctorVisitDurationGrid1");
                Literal ltrl = (Literal)e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.VisitType)].FindControl("ltrlGridVisitType");
                ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
                img.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you want to De-Activate this record :  " + ltrl.Text + "')");
                if (e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Active)].Text.ToString().Trim() == "0")
                {
                    img.Visible = false;
                }
                txt.Visible = false;
            }
            else
            {
                Label ltrlStatus = (Label)e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.VisitType)].FindControl("lblGridActive1");
                ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Edit)].FindControl("lnkEdit");
                ltrlStatus.Visible = false;
                img.Visible = false;
                lnkEdit.Visible = false;
            }
        }

        if (e.Row.RowState == DataControlRowState.Edit || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate))
        {
            DropDownList ddlGridStatus = (DropDownList)e.Row.FindControl("ddlGridStatus");
            Label lblGridAct = (Label)e.Row.FindControl("lblGridActive2");
            String strStatus = e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Active)].Text.ToString().Trim();

            if (strStatus == "0")
            {
                ddlGridStatus.Visible = true;
                lblGridAct.Visible = false;
            }
            else
            {
                ddlGridStatus.Visible = false;
                lblGridAct.Visible = true;

            }
        }
    }

    protected void gvDoctorVisitDuration_OnRowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeActivate")
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable HshIn = new Hashtable();
                HshIn.Add("@TableID", e.CommandArgument.ToString().Trim());
                Int32 i = objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE DoctorVisitDuration SET Active = '0' WHERE ID=@TableID", HshIn);

                if (i == 0)
                {
                    lblMessage.Text = "Record De-Actived...";
                    //Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);
                    gvDoctorVisitDuration.EditIndex = -1;
                    BindDoctorVisitDurationGrid();
                }
                else
                {
                    Alert.ShowAjaxMsg("Error In De-Activation... ", this.Page);
                    gvDoctorVisitDuration.EditIndex = -1;
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

    protected void gvDoctorVisitDuration_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvDoctorVisitDuration.EditIndex = -1;
        BindDoctorVisitDurationGrid();
    }

    protected void gvDoctorVisitDuration_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            String ID = gvDoctorVisitDuration.Rows[e.RowIndex].Cells[Convert.ToByte(GridDoctorVisitDuration.ID)].Text;
            DropDownList ddlVisitGrid = (DropDownList)gvDoctorVisitDuration.Rows[e.RowIndex].Cells[Convert.ToByte(GridDoctorVisitDuration.VisitType)].FindControl("ddlVisitGrid");
            TextBox txtDuration = (TextBox)gvDoctorVisitDuration.Rows[e.RowIndex].Cells[Convert.ToByte(GridDoctorVisitDuration.Duration)].FindControl("txtDoctorVisitDurationGrid");
            string strActive = gvDoctorVisitDuration.Rows[e.RowIndex].Cells[Convert.ToByte(GridDoctorVisitDuration.Active)].Text;
            String strStatus = "";
            if (strActive == "0")
            {
                DropDownList strDrop = ((DropDownList)gvDoctorVisitDuration.Rows[e.RowIndex].Cells[Convert.ToByte(GridDoctorVisitDuration.Status)].FindControl("ddlGridStatus"));
                strStatus = strDrop.SelectedItem.Value;
            }
            else if (strActive == "1")
            {
                strStatus = "1";
            }


            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@VistTypeID", ddlVisitGrid.SelectedItem.Value);
            HshIn.Add("@TableId", ID);

            HshIn.Add("@Duration", bc.ParseQ(txtDuration.Text.ToString().Trim()));
            HshIn.Add("@Active", strStatus);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Int32 i = objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE DoctorVisitDuration SET VisitTypeID=@VistTypeID,Duration=@Duration,Active=@Active WHERE ID=@TableID", HshIn);
            if (i == 0)
            {
                lblMessage.Text = "Record(s) Updated...";
                //Alert.ShowAjaxMsg("Record(s) Updated... ", this.Page);
                gvDoctorVisitDuration.EditIndex = -1;
                BindDoctorVisitDurationGrid();
            }
            else
            {
                lblMessage.Text = "Error In Updation...";
                //Alert.ShowAjaxMsg("Error In Updation... ", this.Page);
                gvDoctorVisitDuration.EditIndex = -1;
            }
            ViewState["ModeTime"] = "Edit";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDoctorVisitDuration_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvDoctorVisitDuration.EditIndex = -1;
        gvDoctorVisitDuration.PageIndex = e.NewPageIndex;
        BindDoctorVisitDurationGrid();
    }

    protected void gvDoctorVisitDuration_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        gvDoctorVisitDuration.EditIndex = e.NewEditIndex;
        BindDoctorVisitDurationGrid();
        LinkButton lnk = (LinkButton)gvDoctorVisitDuration.Rows[e.NewEditIndex].FindControl("lnkUpdate");
        lnk.ForeColor = System.Drawing.Color.White;
        lnk = (LinkButton)gvDoctorVisitDuration.Rows[e.NewEditIndex].FindControl("lnkCancel");
        lnk.ForeColor = System.Drawing.Color.White;
        bStatus = true;
    }

    private string getEmpID()
    {
        string EmpId = "";
        try
        {
            BaseC.EMRMasters.EMRColorLegends EMRColorLegends = new BaseC.EMRMasters.EMRColorLegends(sConString);
            SqlDataReader dr = EMRColorLegends.getEmployeeId(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(Session["UserID"]));

            if (dr.HasRows == true)
            {
                dr.Read();
                String strSelID = dr[0].ToString();
                if (strSelID != "")
                {
                    char[] chArray = { ',' };
                    string[] serviceIdXml = strSelID.Split(chArray);
                    EmpId = serviceIdXml[0].ToString();

                }
            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return EmpId;
    }

    protected void SaveDoctorVisitDuration_OnClick(Object sender, EventArgs e)
    {
        try
        {
            System.Text.StringBuilder strXML = new System.Text.StringBuilder();
            foreach (GridViewRow gvr in gvDoctorVisitDuration.Rows)
            {
                String strID = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridDoctorVisitDuration.ID)].Text.ToString().Trim());
                if (strID.ToString().Trim() == "0")
                {
                    TextBox txtDuration = (TextBox)gvr.Cells[Convert.ToByte(GridDoctorVisitDuration.Duration)].FindControl("txtDoctorVisitDurationGrid1");
                    if (txtDuration.Text.ToString().Trim().Length > 0)
                    {
                        String strVisitTypeID = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridDoctorVisitDuration.VisitTypeID)].Text.ToString().Trim());
                        strXML.Append("<Table><c1>");
                        strXML.Append(strVisitTypeID);
                        strXML.Append("</c1><c2>");
                        strXML.Append(bc.ParseQ(txtDuration.Text.ToString().Trim()));
                        strXML.Append("</c2></Table>");
                    }
                }

            }
            if (strXML.ToString().Length > 0)
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                System.Text.StringBuilder str = new System.Text.StringBuilder(2000);
                Hashtable hshIn = new Hashtable();
                Hashtable hshOut = new Hashtable();
                hshIn.Add("inyHospitalLocationId", Convert.ToInt32(Session["HospitalLocationId"]));
                hshIn.Add("intDoctorID", Convert.ToInt32(ddlProvider.SelectedValue));
                hshIn.Add("xmlDoctorDuration", strXML.ToString());
                hshIn.Add("intEncodedBy", Session["UserID"]);
                hshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveDoctorVisitDuration", hshIn, hshOut);
                if (hshOut["chvErrorStatus"].ToString().ToUpper() == "SAVED")
                {
                    lblMessage.Text = "Doctor Duration Has Been Saved...";
                }
                BindDoctorVisitDurationGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkEmployee_OnClick(object sender, EventArgs e)
    {
        if (DoctorId > 0)
        {
            Response.Redirect("/MPages/employee.aspx?EmpNo=" + DoctorId, false);
        }
        else
        {
            Response.Redirect("/MPages/employee.aspx", false);
        }
    }

    protected void lnkProviderDetails_OnClick(object sender, EventArgs e)
    {
        if (DoctorId > 0)
        {
            Response.Redirect("/MPages/ProviderDetails.aspx?EmpNo=" + DoctorId, false);
        }
        else
        {
            Response.Redirect("/MPages/ProviderDetails.aspx", false);
        }
    }

    protected void lnkProviderProfile_OnClick(object sender, EventArgs e)
    {
        if (DoctorId > 0)
        {
            Response.Redirect("/MPages/DoctorProfileMaster.aspx?EmpNo=" + DoctorId, false);
        }
        else
        {
            Response.Redirect("/MPages/DoctorProfileMaster.aspx", false);
        }
    }

    protected void gvDoctorTime_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
        }
    }

    protected void gvTiming_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtFromTime = (TextBox)e.Row.FindControl("txtFromTime");
            txtFromTime.Attributes.Add("onfocus", "if(this.value == '' || this.value == '__:__ AM') this.value = '08:00 AM';");
            TextBox txtToTime = (TextBox)e.Row.FindControl("txtToTime");
            txtToTime.Attributes.Add("onfocus", "if(this.value == '' || this.value == '__:__ PM') this.value = '05:00 PM';");
        }
    }

    protected void gvTiming_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnCopyNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvTiming.SelectedRow != null)
            {
                TextBox txtFrom1 = (TextBox)gvTiming.Rows[gvTiming.SelectedRow.RowIndex].FindControl("txtFromTime");
                TextBox txtTo1 = (TextBox)gvTiming.Rows[gvTiming.SelectedRow.RowIndex].FindControl("txtToTime");

                for (int i = gvTiming.SelectedRow.RowIndex + 1; i < gvTiming.Rows.Count; i++)
                {
                    TextBox txtFrom1Inner = (TextBox)gvTiming.Rows[i].FindControl("txtFromTime");
                    TextBox txtTo1Inner = (TextBox)gvTiming.Rows[i].FindControl("txtToTime");
                    txtFrom1Inner.Text = txtFrom1.Text;
                    txtTo1Inner.Text = txtTo1.Text;
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

    // modification 

    private void BindVisitDurationProviderDropDown()
    {
        try
        {
            Hashtable hshInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInput.Add("intFacilityId", common.myInt(Session["FacilityId"]));
            DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hshInput);
            ddlProvider.DataSource = objDs;
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlProvider_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDoctorVisitDurationGrid();
    }

    protected void chkNoEnd_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkNoEnd.Checked == true)
        {
            rdpToDate.Enabled = false;
            rdpToDate.SelectedDate = null;
        }
        else
        {
            rdpToDate.Enabled = true;
            rdpToDate.SelectedDate = DateTime.Now.Date;
        }
    }

    protected void lnkEmployeeLookup_OnClick(object sebder, EventArgs e)
    {
        Response.Redirect("~/mpages/EmployeeLockUp.aspx", false);
    }
    protected void lnkClassification_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("/MPages/EmployeeClassification.aspx?EmpId=" + common.myInt(ViewState["emp"]), false);
    }

    protected void lnkPeriodicTemplate_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("/MPages/ProviderTimingsPeriodic.aspx", false);
    }

}