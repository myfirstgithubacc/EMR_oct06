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
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.IO;

public partial class WardManagement_ProgressNote : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["MP"]).ToUpper().Equals("NO"))
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
            Response.Redirect("/Login.aspx?Logout=1", false);

        if (!IsPostBack)
        {
            if (Session["EncounterId"] == null)
            {
                Response.Redirect("/default.aspx?RegNo=0", false);
                return;
            }

            ddlrange_SelectedIndexChanged(sender, e);
            BindProvider();
            SetControls();
            dtpFromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = System.DateTime.Now;
            if (common.myInt(Session["RegistrationId"]) > 0 && common.myInt(Session["EncounterId"]) > 0)
            {
                BindGrid();
                BindPatientHiddenDetails();
            }

            if (common.myBool(Session["isEMRSuperUser"]))
            {
                btnSaveProgressNote.Visible = true;
            }
            EnableSaving();
            SetPermission();
            BindGrid();
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
            {
                btnSaveProgressNote.Visible = false;
                txtWProgressNote.Enabled = false;
            }

        }
        IsCopyCaseSheetAuthorized();
    }

    protected void EnableSaving()
    {
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataTable dt = new DataTable();
        BaseC.Dashboard objDash = new BaseC.Dashboard();
        BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
        try
        {


            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE").Equals(true) && common.myBool(Session["isEMRSuperUser"]).Equals(true))
            {
                tblProviderDetails.Visible = true;
                ds = objCM.GetDoctorList(Convert.ToInt16(Session["HospitalLocationID"]), 0, Convert.ToInt16(Session["FacilityId"]));
                ddlRendringProvider.DataSource = ds;
                ddlRendringProvider.DataValueField = "DoctorID";
                ddlRendringProvider.DataTextField = "DoctorName";
                ddlRendringProvider.DataBind();
                ds = objDash.getPatientEncounterDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(Session["RegistrationId"]));
                dv = ds.Tables[0].DefaultView;
                dv.RowFilter = " Id = " + common.myInt(Session["encounterid"]);
                dt = dv.ToTable();
                dtpChangeDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
                dtpChangeDate.MinDate = common.myDate(dt.Rows[0]["EncounterDate"].ToString());
                if (common.myStr(dt.Rows[0]["OPIP"]).Equals("O"))
                {
                    dtpChangeDate.MaxDate = common.myDate(dt.Rows[0]["EncounterDate"].ToString()).AddDays(3);
                    ViewState["DischargeDate"] = common.myDate(dt.Rows[0]["DischargeDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                }
                else if (common.myStr(dt.Rows[0]["OPIP"]).Equals("E"))
                {
                    dtpChangeDate.MaxDate = common.myDate(dt.Rows[0]["DischDate"].ToString());
                    ViewState["DischargeDate"] = common.myDate(dt.Rows[0]["DischDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                }
                else
                {
                    dtpChangeDate.MaxDate = common.myDate(dt.Rows[0]["DischargeDate"].ToString());
                    ViewState["DischargeDate"] = common.myDate(dt.Rows[0]["DischargeDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                }
                ViewState["AdmissionDate"] = common.myDate(dt.Rows[0]["EncounterDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                lblRange.Text = "Date Range: Admission Date(" + common.myStr(ViewState["AdmissionDate"]) + ") and Discharge Date(" + common.myStr(ViewState["DischargeDate"]) + ")";
                RadTimeFrom.TimeView.Interval = new TimeSpan(0, 15, 0);
                RadTimeFrom.TimeView.StartTime = new TimeSpan(0, 0, 0);
                RadTimeFrom.TimeView.EndTime = new TimeSpan(23, 59, 59);
                ddlMinute.Items.Clear();
                for (int i = 0; i < 60; i++)
                {
                    if (i.ToString().Length == 1)
                        ddlMinute.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                    else
                        ddlMinute.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
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
            ds.Dispose();
            dv.Dispose();
            dt.Dispose(); ;
            objDash = null;
        }
    }

    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations(sConString);
        try
        {
            if (!common.myBool(Session["isEMRSuperUser"]))
            {
                btnSaveProgressNote.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
                ViewState["IsAllowEdit"] = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
                ViewState["IsAllowCancel"] = ua1.CheckPermissionsForEMRModule("C", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
            }
            else
            {
                btnSaveProgressNote.Enabled = true;
                ViewState["IsAllowEdit"] = true;
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
            ua1.Dispose();
        }
    }


    private void BindPatientHiddenDetails()
    {
        try
        {
            if (Session["PatientDetailString"] != null)
            {
                //  lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void SetControls()
    {
        RadComboBoxItem item = new RadComboBoxItem();
        try
        {
            ddlrange.Items.Clear();
            item = new RadComboBoxItem();
            item.Text = "Select All";
            item.Value = string.Empty;
            item.Selected = true;
            ddlrange.Items.Add(item);

            item = new RadComboBoxItem();
            item.Text = "Today";
            item.Value = "DD0";
            ddlrange.Items.Add(item);

            //item = new RadComboBoxItem();
            //item.Text = "This Week";
            //item.Value = "WW0";
            //ddlrange.Items.Add(item);

            item = new RadComboBoxItem();
            item.Text = "Last Week";
            item.Value = "WW-1";
            ddlrange.Items.Add(item);

            item = new RadComboBoxItem();
            item.Text = "Last Two Week";
            item.Value = "WW-2";
            ddlrange.Items.Add(item);

            //item = new RadComboBoxItem();
            //item.Text = "This Month";
            //item.Value = "MM0";
            //ddlrange.Items.Add(item);

            item = new RadComboBoxItem();
            item.Text = "Last One Month";
            item.Value = "MM-1";
            ddlrange.Items.Add(item);

            item = new RadComboBoxItem();
            item.Text = "Last Year";
            item.Value = "YY-1";
            ddlrange.Items.Add(item);

            item = new RadComboBoxItem();
            item.Text = "Date Range";
            item.Value = "4";
            ddlrange.Items.Add(item);

            if (common.myStr(Session["IsMedicalAlert"]).Equals(string.Empty))
            {
                lnkAlerts.Enabled = false;
                lnkAlerts.CssClass = "blinkNone";
                lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
            }
            else if (common.myStr(Session["IsMedicalAlert"]).ToUpper().Equals("YES"))
            {
                lnkAlerts.Enabled = true;
                lnkAlerts.Font.Bold = true;
                lnkAlerts.CssClass = "blink";
                lnkAlerts.Font.Size = 11;
                lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
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
            item = null;
        }
    }

    private void BindProvider()
    {
        DataSet ds = new DataSet();
        BaseC.clsLISMaster lis = new BaseC.clsLISMaster(sConString);
        try
        {
            ds = lis.getDoctorList(0, "", common.myInt(Session["HospitalLocationId"]), 0, common.myInt(Session["FacilityId"]), 0, 0);

            ddlProvider.DataSource = ds;
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataBind();

            ddlProvider.Items.Insert(0, new RadComboBoxItem("All", "0"));

            ddlProvider.SelectedIndex = ddlProvider.Items.IndexOf(ddlProvider.Items.FindItemByValue(common.myInt(Session["DoctorId"]).ToString()));
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
            lis = null;
        }
    }

    //private DataTable BindBlankGrid()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("ProgressNoteId");
    //    dt.Columns.Add("HospitalLocationId");
    //    dt.Columns.Add("OPIP");
    //    dt.Columns.Add("EncounterId");
    //    dt.Columns.Add("EncounterDate");
    //    dt.Columns.Add("EncounterNo");
    //    dt.Columns.Add("FacilityName");
    //    dt.Columns.Add("DoctorName");
    //    dt.Columns.Add("ProgressNote");
    //    dt.Columns.Add("RegistrationId");
    //    dt.Columns.Add("RegistrationNo");
    //    dt.Columns.Add("DoctorId");
    //    dt.Columns.Add("EncodedBy");
    //    dt.Columns.Add("EncodedDate");
    //    DataRow dr = dt.NewRow();
    //    dr["ProgressNoteId"] = DBNull.Value;
    //    dr["HospitalLocationId"] = DBNull.Value;
    //    dr["OPIP"] = DBNull.Value;
    //    dr["EncounterId"] = DBNull.Value;
    //    dr["EncounterDate"] = DBNull.Value;
    //    dr["EncounterNo"] = DBNull.Value;
    //    dr["DoctorName"] = DBNull.Value;
    //    dr["FacilityName"] = DBNull.Value;
    //    dr["DoctorId"] = DBNull.Value;
    //    dr["ProgressNote"] = DBNull.Value;
    //    dr["RegistrationId"] = DBNull.Value;
    //    dr["RegistrationNo"] = DBNull.Value;
    //    dr["EncodedBy"] = DBNull.Value;
    //    dr["EncodedDate"] = DBNull.Value;
    //    dt.Rows.Add(dr);
    //    return dt;
    //}

    private void BindGrid()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
        DataRow DR;
        string fromDate = string.Empty;
        string toDate = string.Empty;

        try
        {
            if (!common.myStr(ddlrange.SelectedValue).Equals(string.Empty))
            {
                fromDate = dtpFromDate.SelectedDate.Value.ToString("yyyy/MM/dd");
                toDate = dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd");
            }

            ds = objEmr.GetDoctorProgressNote(0, common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                common.myInt(Session["RegistrationId"]), common.myInt(ddlProvider.SelectedValue), common.myStr(ddlrange.SelectedValue),
                                fromDate, toDate, common.myInt(Session["EncounterId"]), "N");

            if (ds.Tables[0].Rows.Count == 0)
            {
                DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvDoctorProgressNote.DataSource = ds;
            gvDoctorProgressNote.DataBind();
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
            objEmr = null;
            DR = null;
            fromDate = string.Empty;
            toDate = string.Empty;
        }
    }

    protected void btnSaveProgressNote_OnClick(object sender, EventArgs e)
    {


        BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
        BaseC.User objuser = new BaseC.User(sConString);
        Hashtable hshOut = new Hashtable();

        try
        {
            if (txtWProgressNote.Content.Equals(string.Empty))
            {
                Alert.ShowAjaxMsg("Please enter Progress Note", Page);
                return;
            }
            int intProviderId = common.myInt(ddlRendringProvider.SelectedValue);
            string Hourtime = string.Empty;
            if (RadTimeFrom.SelectedDate != null)
                Hourtime = RadTimeFrom.SelectedDate.Value.TimeOfDay.ToString();
            DateTime? dtChangeDate = null;
            if (dtpChangeDate.SelectedDate != null)
            {
                dtChangeDate = common.myDate(dtpChangeDate.SelectedDate);
                if (!string.IsNullOrEmpty(common.myStr(dtChangeDate)) && !string.IsNullOrEmpty(Hourtime))
                    dtChangeDate = Convert.ToDateTime(common.myStr((common.myStr(dtChangeDate).Split(' '))[0]) + " " + Hourtime);
            }

            if (tblProviderDetails.Visible == true)
            {
                if (ddlRendringProvider.SelectedIndex < 1)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please fill Provider";
                    return;
                }
                if (string.IsNullOrEmpty(dtpChangeDate.SelectedDate.ToString()))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please select change date";
                    return;
                }
                if (string.IsNullOrEmpty(RadTimeFrom.SelectedDate.ToString()))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please select change Time";
                    return;
                }
                if (dtChangeDate > common.myDate(ViewState["DischargeDate"]) || dtChangeDate < common.myDate(ViewState["AdmissionDate"]))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Change Date should be between Admission date and Discharge date.";
                    return;
                }
            }



            //int DoctorId = objuser.getEmployeeId(common.myInt(Session["UserId"])); 

            int DoctorId = common.myInt(ddlProvider.SelectedValue);


            hshOut = objEmr.SaveDoctorProgressNote(common.myInt(ViewState["ProgressNoteId"]), common.myInt(Session["HospitalLocationId"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                                DoctorId, txtWProgressNote.Content, common.myInt(Session["UserId"]), intProviderId, dtChangeDate, 'N');

            //if (common.myStr(Request.QueryString["OPIP"]) == "I")
            //{
            //    BaseC.User objuser = new BaseC.User(sConString);
            //    int DoctorId = objuser.getEmployeeId(common.myInt(Session["UserId"]));

            //    hshOut = objEmr.SaveDoctorProgressNote(Convert.ToInt32(ViewState["ProgressNoteId"] != null ? ViewState["ProgressNoteId"] : 0), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            //       common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
            //       DoctorId, txtWProgressNote.Content, common.myInt(Session["UserId"]));

            //}
            //else
            //{
            //    hshOut = objEmr.SaveDoctorProgressNote(Convert.ToInt32(ViewState["ProgressNoteId"] != null ? ViewState["ProgressNoteId"] : 0), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            //       common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
            //       Convert.ToInt32(Session["DoctorId"]), txtWProgressNote.Content, common.myInt(Session["UserId"]));
            //}

            lblMessage.Text = common.myStr(hshOut["@chvErrorOutput"]);
            dtpChangeDate.SelectedDate = null;
            ddlRendringProvider.SelectedValue = null;
            BindGrid();

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            ViewState["ProgressNoteId"] = null;
            txtWProgressNote.Content = string.Empty;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEmr = null;
            objuser = null;
            hshOut = null;
        }
    }

    protected void gvDoctorProgressNote_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnProgressNote = (HiddenField)e.Row.FindControl("hdnProgressNote");
                RadEditor editProgressNote = (RadEditor)e.Row.FindControl("editProgressNote");
                HiddenField hdnEncodedBy = (HiddenField)e.Row.FindControl("hdnEncodedBy");
                LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");

                //editProgressNote.Content = hdnProgressNote.Value;

                //if (common.myInt(hdnEncodedBy.Value).Equals(common.myInt(Session["UserId"])) || common.myBool(Session["isEMRSuperUser"]))
                //{
                //    e.Row.Enabled = true;
                //}
                //else
                //{
                //    e.Row.Enabled = false;
                //}

                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
                {
                    ibtnDelete.Visible = false;
                }

                if (common.myInt(hdnEncodedBy.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedBy.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                        ibtnDelete.Visible = false;
                    }
                }
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
                {
                    lnkEdit.Enabled = false;
                    ibtnDelete.Enabled = false;
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

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void ddlrange_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (common.myStr(ddlrange.SelectedValue).Equals("4"))
        {
            tblDateRange.Visible = true;

            dtpFromDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpFromDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpFromDate.SelectedDate = DateTime.Now;

            dtpToDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpToDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpToDate.SelectedDate = DateTime.Now;
        }
        else
        {
            tblDateRange.Visible = false;

            dtpFromDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpToDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();

            dtpFromDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpToDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();

            dtpFromDate.DateInput.Text = string.Empty;
            dtpToDate.DateInput.Text = string.Empty;

            if (common.myStr(ddlrange.SelectedValue).Equals("DD0")) //Today
            {
                dtpFromDate.SelectedDate = DateTime.Now;
                dtpToDate.SelectedDate = DateTime.Now;
            }
            else if (common.myStr(ddlrange.SelectedValue).Equals("WW-1")) //Last Week
            {
                dtpFromDate.SelectedDate = DateTime.Now.AddDays(-7);
                dtpToDate.SelectedDate = DateTime.Now;
            }
            else if (common.myStr(ddlrange.SelectedValue).Equals("WW-2")) //Last Two Week
            {
                dtpFromDate.SelectedDate = DateTime.Now.AddDays(-14);
                dtpToDate.SelectedDate = DateTime.Now;
            }
            else if (common.myStr(ddlrange.SelectedValue).Equals("MM-1")) //Last One Month
            {
                dtpFromDate.SelectedDate = DateTime.Now.AddMonths(-1);
                dtpToDate.SelectedDate = DateTime.Now;
            }
            else if (common.myStr(ddlrange.SelectedValue).Equals("YY-1")) //Last Year
            {
                dtpFromDate.SelectedDate = DateTime.Now.AddYears(-1);
                dtpToDate.SelectedDate = DateTime.Now;
            }
        }
    }

    //protected void gvDoctorProgressNote_OnItemCommand(object sender, GridCommandEventArgs e)
    //{
    //    BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
    //    try
    //    {
    //        HiddenField hdnProgressNoteId = (HiddenField)e.Item.FindControl("hdnProgressNoteId");
    //        TextBox txtProgressNote = (TextBox)e.Item.FindControl("txtProgressNote");
    //        if (common.myStr(e.CommandName).ToUpper().Equals("SELECT"))
    //        {
    //            ViewState["ProgressNoteId"] = hdnProgressNoteId.Value;
    //            txtWProgressNote.Content = txtProgressNote.Text;
    //        }
    //        else if (common.myStr(e.CommandName).ToUpper().Equals("PROGRESSNOTEDELETE"))
    //        {
    //            if (common.myInt(e.CommandArgument) > 0)
    //            {
    //                int vv = objEmr.inActiveDoctorProgressNote(common.myInt(e.CommandArgument), common.myInt(Session["UserId"]));

    //                ViewState["ProgressNoteId"] = string.Empty;
    //                txtWProgressNote.Content = string.Empty;

    //                BindGrid();

    //                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
    //                lblMessage.Text = "Deleted Successfully ...";
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        objEmr = null;
    //    }
    //}

    protected void gvDoctorProgressNote_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
        try
        {
            if (common.myStr(e.CommandName).ToUpper().Equals("ITEMSELECT"))
            {
                if (!common.myBool(ViewState["IsAllowEdit"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                    return;
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblDate = (Label)row.FindControl("lblDate");
                HiddenField hdnProviderId = (HiddenField)row.FindControl("hdnProviderId");
                HiddenField hdnProgressNoteId = (HiddenField)row.FindControl("hdnProgressNoteId");
                HiddenField hdnProgressNote = (HiddenField)row.FindControl("hdnProgressNote");
                RadTimeFrom.SelectedDate = Convert.ToDateTime(Convert.ToDateTime(common.myStr(lblDate.Text.Substring(common.myLen(lblDate.Text) - 7).Trim())).ToShortTimeString());
                dtpChangeDate.SelectedDate = common.myDate(lblDate.Text);
                if (common.myInt(hdnProviderId.Value) > 0)
                {
                    ddlRendringProvider.SelectedIndex = ddlRendringProvider.Items.IndexOf(ddlRendringProvider.Items.FindItemByValue(common.myStr(hdnProviderId.Value)));
                }
                ViewState["ProgressNoteId"] = hdnProgressNoteId.Value;
                txtWProgressNote.Content = hdnProgressNote.Value;
            }
            else if (common.myStr(e.CommandName).ToUpper().Equals("PROGRESSNOTEDELETE"))
            {
                if (!common.myBool(ViewState["IsAllowCancel"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                    return;
                }

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                HiddenField hdnProgressNoteId = (HiddenField)row.FindControl("hdnProgressNoteId");
                HiddenField hdnProgressNote = (HiddenField)row.FindControl("hdnProgressNote");

                if (common.myInt(e.CommandArgument) > 0)
                {
                    int vv = objEmr.inActiveDoctorProgressNote(common.myInt(e.CommandArgument), common.myInt(Session["UserId"]));

                    ViewState["ProgressNoteId"] = string.Empty;
                    txtWProgressNote.Content = string.Empty;

                    BindGrid();

                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = "Deleted Successfully ...";
                }
            }
            if (e.CommandName == "NoteView")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblProgressNote = (Label)row.FindControl("lblProgressNote");
                Label lblDate = (Label)row.FindControl("lblDate");
                Label lblDoctor = (Label)row.FindControl("lblDoctor");
                dvProgressNoteView.Visible = true;
                lblResultHistoryPatientName.Text = "Entered By: " + common.myStr(lblDoctor.Text.Trim()) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Date: " + common.myStr(lblDate.Text);
                lblProgressNoteView.Text = common.myStr(lblProgressNote.Text);
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
            objEmr = null;
        }
    }

    //protected void gvDoctorProgressNote_OnItemCommand(object sender, GridCommandEventArgs e)
    //{
    //    BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
    //    try
    //    {
    //        HiddenField hdnProgressNoteId = (HiddenField)e.Item.FindControl("hdnProgressNoteId");
    //        HiddenField hdnProgressNote = (HiddenField)e.Item.FindControl("hdnProgressNote");
    //        if (common.myStr(e.CommandName).ToUpper().Equals("SELECT"))
    //        {
    //            ViewState["ProgressNoteId"] = hdnProgressNoteId.Value;
    //            txtWProgressNote.Content = hdnProgressNote.Value;
    //        }
    //        else if (common.myStr(e.CommandName).ToUpper().Equals("PROGRESSNOTEDELETE"))
    //        {
    //            if (common.myInt(e.CommandArgument) > 0)
    //            {
    //                int vv = objEmr.inActiveDoctorProgressNote(common.myInt(e.CommandArgument), common.myInt(Session["UserId"]));

    //                ViewState["ProgressNoteId"] = string.Empty;
    //                txtWProgressNote.Content = string.Empty;

    //                BindGrid();

    //                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
    //                lblMessage.Text = "Deleted Successfully ...";
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        objEmr = null;
    //    }
    //}


    protected void lnkAlerts_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;
            RadWindow3.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&PId=" + common.myInt(Session["RegistrationId"]);
            RadWindow3.Height = 600;
            RadWindow3.Width = 600;
            RadWindow3.Top = 10;
            RadWindow3.Left = 10;
            RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow3.Modal = true;
            RadWindow3.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDoctorProgressNote_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDoctorProgressNote.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void ddlMinute_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();

        try
        {
            if (RadTimeFrom.SelectedDate != null)
            {
                sb.Append(RadTimeFrom.SelectedDate.Value.ToString());
                sb.Remove(RadTimeFrom.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
                sb.Insert(RadTimeFrom.SelectedDate.Value.ToString().IndexOf(":") + 1, ddlMinute.Text);
                RadTimeFrom.SelectedDate = Convert.ToDateTime(sb.ToString());
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select time.";
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
            sb = null;
        }
    }
    protected void btnResultHistoryClose_OnClick(object sender, EventArgs e)
    {
        dvProgressNoteView.Visible = false;
    }
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        BindGrid();
        ViewState["ProgressNoteId"] = null;
        txtWProgressNote.Content = string.Empty;
    }
    public void IsCopyCaseSheetAuthorized()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        bool IsAuthorized = objSecurity.IsCopyCaseSheetAuthorized(common.myInt(Session["UserID"]), common.myInt(Session["HospitalLocationID"]));
        hdnIsCopyCaseSheetAuthorized.Value = common.myStr(IsAuthorized);
    }
}
