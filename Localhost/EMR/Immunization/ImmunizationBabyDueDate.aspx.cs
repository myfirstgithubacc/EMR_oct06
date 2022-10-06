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
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;
using Telerik.Web.UI;

public partial class EMR_Immunization_ImmunizationBabyDueDate : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();
    DAL.DAL Dl;
    DataSet ds;
    BaseC.RestFulAPI commonObj;
    string flagAutoManualDueDate = string.Empty;

    private enum GridColgvDueDate : byte
    {

        COL0 = 0,
        COL1 = 1,
        COL2 = 2,
        COL3 = 3,
        COL4 = 4,
        COL5 = 5,
        COL6 = 6,
        COL7 = 7,
        COL8 = 8,
        COL9 = 9,
        COL10 = 10,
        COL11 = 11



    }

    private enum GridImmunizationDueDate : byte
    {
        ParentID = 0,
        ChildID = 1,
        ImmunizationScheduleBabyID = 2,
        ImmunizationId = 3,
        SerialNo = 4,
        ImmunizationName = 5,
        DueDate = 6,
        GivenOn = 7,
        GivenBy = 8,
        Edit = 9,
        FromTransactionTable = 10
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Request.QueryString["From"] != null && Request.QueryString["From"].ToString() == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        commonObj = new BaseC.RestFulAPI(sConString);
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet dsPatientDetail = new DataSet();
        try
        {

            if (!IsPostBack)
            {
                ViewState["RegistrationID"] = Session["RegistrationID"];
                ViewState["RegistrationNo"] = Session["RegistrationNo"];
                ViewState["EncounterNo"] = Session["EncounterNo"];
                ViewState["IsMedicalAlert"] = Session["IsMedicalAlert"];
                SetPermission();
                if (!common.myStr(ViewState["RegistrationNo"]).Trim().Equals(string.Empty))
                {
                    dsPatientDetail = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                       common.myStr(ViewState["RegistrationNo"]), common.myStr(ViewState["EncounterNo"]), common.myInt(Session["UserId"]), 0); // "0" encounter no

                    if (dsPatientDetail.Tables.Count > 0)
                    {
                        if (dsPatientDetail.Tables[0].Rows.Count > 0)
                        {

                            string sRegNoTitle = Resources.PRegistration.regno;
                            string sDoctorTitle = Resources.PRegistration.Doctor;
                            string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date :" : "Encounter Date :";
                            ViewState["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["GenderAge"]) + "</span>"
                                            + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                                            + "&nbsp;Enc #:" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]) + "</span>"
                                            + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"
                                            //+ DateTitle + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(Session["EncounterDate"]) + "</span>"
                                            + "&nbsp;Bed:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["BedNo"]) + "</span>"
                                            + "&nbsp;Ward:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["WardName"]) + "</span>"
                                            + "&nbsp;Mobile:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                                            + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]) + "</span>"
                                            + "</b>";

                            txtAccountNo.Text = common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]);
                            Session["EncounterId"] = common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterId"]);
                        }
                    }
                }
                if (ViewState["PatientDetailString"] != null)
                {
                    lblPatientDetail.Text = ViewState["PatientDetailString"].ToString();
                }
                gvDue.Columns[(byte)GridColgvDueDate.COL11].Visible = false;

                flagAutoManualDueDate = common.GetFlagValueHospitalSetup(Convert.ToInt16(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsAutoManualImmunizationDueDate", sConString); //A OR M
                lblImmunizationDD.Text = flagAutoManualDueDate;


                //if (Convert.ToString(Session["encounterid"]) == "")
                //{
                //    Response.Redirect("/default.aspx?RegNo=0", false);
                //}
                Hashtable hashIn = new Hashtable();
                if (Request.QueryString["Mpg"] != null)
                {
                    Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                    string pid = Session["CurrentNode"].ToString();
                    int len = pid.Length;
                    ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, len - 1);
                }
                else
                    ViewState["PageId"] = "0";

                if (common.myStr(ViewState["IsMedicalAlert"]) == "")
                {
                    lnkAlerts.Enabled = false;
                    lnkAlerts.CssClass = "blinkNone";
                    lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
                }
                else if (common.myStr(ViewState["IsMedicalAlert"]).ToUpper() == "YES")
                {
                    lnkAlerts.Enabled = true;
                    lnkAlerts.Font.Bold = true;
                    lnkAlerts.Font.Size = 11;
                    lnkAlerts.CssClass = "blink";
                    lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
                }
                if (Request.QueryString["From"] != null && Request.QueryString["From"].ToString() == "POPUP")
                {
                    btnClosePage.Visible = true;
                }

                hdnCountDueDate.Value = common.GetFlagValueHospitalSetup(Convert.ToInt16(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ImmunizationDueDate", sConString);

                hdnCurrentDate.Value = DateTime.Today.ToString("dd/MM/yyyy");

                BaseC.EMRImmunization immu = new BaseC.EMRImmunization(sConString);
                if (ViewState["RegistrationID"] != null && common.myStr(ViewState["RegistrationID"].ToString()) != "" && Convert.ToInt32(ViewState["RegistrationID"]) > 0)
                {
                    lbldob.Text = immu.GetPatientDOB(Convert.ToInt32(ViewState["RegistrationID"]), Convert.ToInt16(Session["HospitalLocationID"]));
                    BindgvDueDateGrid();
                }
                if (common.myStr(Request.QueryString["CloseButtonShow"]) == "No")
                {
                    btnClosePage.Visible = false;
                }
                btnGetInfo_Click(sender, e);

            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void SetPermission()
    {
        UserAuthorisations objUA = new UserAuthorisations(sConString);
        try
        {
            ViewState["IsAllowToCancel"] = objUA.CheckPermissionsForEMRModule("C", Request.Url.AbsolutePath, 0);
        }
        catch
        {
        }
        finally
        {
            objUA.Dispose();
        }
    }
    void BindgvDueDateGrid()
    {
        ds = new DataSet();
        Hashtable hashIn = new Hashtable();
        BaseC.EMRImmunization Immu = new BaseC.EMRImmunization(sConString);
        BaseC.Security AuditCA = new BaseC.Security(sConString);
        Dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet dsColor = new DataSet();
        try
        {


            dsColor = Immu.GetImmunizationStatus(Convert.ToInt16(Session["HospitalLocationId"]));
            if (dsColor.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsColor.Tables[0].Rows.Count; i++)
                {
                    if (dsColor.Tables[0].Rows[i]["Code"].ToString() == "DD")
                    {
                        lblDueColor.BackColor = System.Drawing.Color.FromName(dsColor.Tables[0].Rows[i]["ColorName"].ToString());
                        hdnDueDateColor.Value = dsColor.Tables[0].Rows[i]["ColorName"].ToString();
                    }
                    else if (dsColor.Tables[0].Rows[i]["Code"].ToString() == "GD")
                    {
                        lblGivenColor.BackColor = System.Drawing.Color.FromName(dsColor.Tables[0].Rows[i]["ColorName"].ToString());
                        hdnGivenDateColor.Value = dsColor.Tables[0].Rows[i]["ColorName"].ToString();
                    }
                    else if (dsColor.Tables[0].Rows[i]["Code"].ToString() == "R")
                    {
                        lblRejected.BackColor = System.Drawing.Color.FromName(dsColor.Tables[0].Rows[i]["ColorName"].ToString());
                        hdnRejected.Value = dsColor.Tables[0].Rows[i]["ColorName"].ToString();
                    }
                }
            }
            ds = Immu.GetPatientImmunizationDueDates(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), common.myInt(ViewState["RegistrationID"]), common.myStr(lbldob.Text));
            gvDue.DataSource = ds.Tables[0];
            gvDue.DataBind();

            /* TEMP KABIR */
            //string flagAutoManualDueDate = string.Empty; // A or M
            //flagAutoManualDueDate = "M";
            /* TEMP KABIR */
            if (common.myStr(lblImmunizationDD.Text).Equals("M"))
            {
                MergeGridManual(gvDue);

            }
            else if (common.myStr(lblImmunizationDD.Text).Equals("A"))
            {
                MergeGrid(gvDue);
            }

            //MergeGrid(gvDue);
            if (ds.Tables[0].Rows.Count > 0)
                if (!IsPostBack)
                    AuditCA.AuditCommonAccess(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), common.myInt(Session["FacilityID"]),
                        common.myInt(ViewState["RegistrationID"]), common.myInt(0), common.myInt(ViewState["PageId"]), 0,
                        common.myInt(Session["UserID"]), 0, "ACCESSED", common.myStr(Session["IPAddress"]));
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            hashIn = null;
            Immu = null;
            AuditCA = null;
            Dl = null;
            dsColor.Dispose(); ;
        }
    }
    private void MergeGrid(GridView gvReminder)
    {
        try
        {
            for (int rowIndex = gvDue.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow gvRow = gvDue.Rows[rowIndex];
                GridViewRow gvPreviousRow = gvDue.Rows[rowIndex + 1];
                Label lblCAge = (Label)gvRow.Cells[0].FindControl("lblAge");
                Label lblPAge = (Label)gvPreviousRow.Cells[0].FindControl("lblAge");
                if (lblCAge.Text == lblPAge.Text)
                {
                    gvRow.Cells[0].RowSpan = gvDue.Rows[rowIndex + 1].Cells[0].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[0].RowSpan + 1;
                    gvDue.Rows[rowIndex + 1].Cells[0].Visible = false;

                    //gvRow.Cells[1].RowSpan = gvDue.Rows[rowIndex + 1].Cells[1].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[1].RowSpan + 1;
                    //gvDue.Rows[rowIndex + 1].Cells[1].Visible = false;

                    gvRow.Cells[3].RowSpan = gvDue.Rows[rowIndex + 1].Cells[3].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[3].RowSpan + 1;
                    gvDue.Rows[rowIndex + 1].Cells[3].Visible = false;
                }
            }
        }


        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void MergeGridManual(GridView gvReminder)
    {
        try
        {
            for (int rowIndex = gvDue.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow gvRow = gvDue.Rows[rowIndex];
                GridViewRow gvPreviousRow = gvDue.Rows[rowIndex + 1];
                Label lblCAge = (Label)gvRow.Cells[0].FindControl("lblAge");
                Label lblPAge = (Label)gvPreviousRow.Cells[0].FindControl("lblAge");
                if (lblCAge.Text == lblPAge.Text)
                {
                    gvRow.Cells[0].RowSpan = gvDue.Rows[rowIndex + 1].Cells[0].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[0].RowSpan + 1;
                    gvDue.Rows[rowIndex + 1].Cells[0].Visible = false;

                    //gvRow.Cells[1].RowSpan = gvDue.Rows[rowIndex + 1].Cells[1].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[1].RowSpan + 1;
                    //gvDue.Rows[rowIndex + 1].Cells[1].Visible = false;

                    //gvRow.Cells[3].RowSpan = gvDue.Rows[rowIndex + 1].Cells[3].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[3].RowSpan + 1;
                    //gvDue.Rows[rowIndex + 1].Cells[3].Visible = false;
                }
            }
        }


        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindBlankGrid()
    {
        DataTable dtImmunization = new DataTable();
        try
        {

            dtImmunization.Columns.Add("ParentID");
            dtImmunization.Columns.Add("ChildID");
            dtImmunization.Columns.Add("ImmunizationScheduleBabyID");
            dtImmunization.Columns.Add("ImmunizationId");
            dtImmunization.Columns.Add("ImmunizationName");
            dtImmunization.Columns.Add("DueDate");
            dtImmunization.Columns.Add("GivenOn");
            dtImmunization.Columns.Add("GivenBy");

            for (int i = 0; i < 5; i++)
            {
                DataRow Dr = dtImmunization.NewRow();
                Dr["ParentID"] = "0";
                Dr["ChildID"] = "0";
                Dr["ImmunizationScheduleBabyID"] = "0";
                Dr["ImmunizationId"] = "0";
                Dr["ImmunizationName"] = "";
                Dr["DueDate"] = "";
                Dr["GivenOn"] = "";
                Dr["GivenBy"] = "";
                dtImmunization.Rows.Add(Dr);
            }

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {

            dtImmunization.Dispose();
        }
    }
    protected void gvDue_OnDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Visible = false;
                Label lblGivenDate = (Label)e.Row.FindControl("lblGivenDate");
                HiddenField hdnImmunizationDueDateCount = (HiddenField)e.Row.FindControl("hdnImmunizationDueDateCount");
                HiddenField hdnRejectedByPatient = (HiddenField)e.Row.FindControl("hdnRejectedByPatient");
                ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");
                ImageButton ibtnDeleteDueDate = (ImageButton)e.Row.FindControl("ibtnDeleteDueDate");
                //CheckBox chkDueDate = (CheckBox)e.Row.FindControl("chkDueDate");

                Label lblDueDate = (Label)e.Row.FindControl("lblDueDate");
                RadDatePicker RadExpiryDate = (RadDatePicker)e.Row.FindControl("RadExpiryDate");

                HiddenField hdnGivenStatus = (HiddenField)e.Row.FindControl("hdnGivenStatus");

                HiddenField hdnDaysFrom = (HiddenField)e.Row.FindControl("hdnDaysFrom");
                HiddenField hdnDaysTo = (HiddenField)e.Row.FindControl("hdnDaysTo");



                /* TEMP KABIR */
                //string flagAutoManualDueDate = string.Empty; // A or M
                //flagAutoManualDueDate = "M";
                /* TEMP KABIR */

                if (common.myStr(hdnGivenStatus.Value).ToUpper().Equals("NO") && hdnImmunizationDueDateCount.Value == "1" &&
                   !common.myBool(hdnRejectedByPatient.Value))
                {
                    e.Row.BackColor = System.Drawing.Color.FromName(hdnDueDateColor.Value);


                    //e.Row.Cells[0].BackColor = System.Drawing.Color.White; yogesh old
                    e.Row.Cells[0].BackColor = System.Drawing.Color.FromName(hdnDueDateColor.Value); //yogesh new


                    // e.Row.Cells[3].BackColor = System.Drawing.Color.White;

                    if (common.myStr(lblImmunizationDD.Text).Equals("M"))
                    {
                        lblDueDate.Visible = false;
                        RadExpiryDate.Visible = true;
                        // chkDueDate.Visible = true;

                        if (RadExpiryDate.SelectedDate == null)
                        {
                            e.Row.BackColor = System.Drawing.Color.White;
                        }
                    }
                    else if (common.myStr(lblImmunizationDD.Text).Equals("A"))
                    {
                        lblDueDate.Visible = true;
                        RadExpiryDate.Visible = false;
                        //  chkDueDate.Visible = false;
                    }
                }
                else if (common.myStr(hdnGivenStatus.Value).ToUpper().Equals("YES") && !common.myBool(hdnRejectedByPatient.Value))
                {
                    e.Row.BackColor = System.Drawing.Color.FromName(hdnGivenDateColor.Value);


                    //e.Row.Cells[0].BackColor = System.Drawing.Color.White; // yogesh old
                    e.Row.Cells[0].BackColor = System.Drawing.Color.FromName(hdnGivenDateColor.Value); //yogesh new
                    lblDueDate.Visible = true;
                    RadExpiryDate.Visible = false;
                    //  chkDueDate.Visible = false;
                }
                else if (common.myStr(hdnGivenStatus.Value).ToUpper().Equals("NO") && common.myBool(hdnRejectedByPatient.Value))
                {
                    e.Row.BackColor = System.Drawing.Color.FromName(hdnRejected.Value);


                   // e.Row.Cells[0].BackColor = System.Drawing.Color.White; //yogesh old

                    e.Row.Cells[0].BackColor = System.Drawing.Color.FromName(hdnRejected.Value); //yogesh new
                    lblDueDate.Visible = true;
                    RadExpiryDate.Visible = false;
                }
                if (common.myStr(hdnGivenStatus.Value).ToUpper().Equals("NO") && !common.myBool(hdnRejectedByPatient.Value))
                {
                    ibtnDelete.Visible = false;
                    ibtnDeleteDueDate.Visible = false;
                }
                else if (common.myBool(hdnRejectedByPatient.Value))
                {
                    ibtnDelete.Visible = true;
                    ibtnDeleteDueDate.Visible = false;
                }
                else
                {
                    ibtnDelete.Visible = true;
                    ibtnDeleteDueDate.Visible = false;
                }
                /***********************For Due date *************/
                if (common.myStr(hdnGivenStatus.Value).ToUpper().Equals("NO") && hdnImmunizationDueDateCount.Value == "1"
                    && !common.myBool(hdnRejectedByPatient.Value))
                {
                    if (common.myStr(lblImmunizationDD.Text).Equals("M"))
                    {
                        ibtnDeleteDueDate.Visible = true;
                        lblDueDate.Visible = false;
                        RadExpiryDate.Visible = true;
                        //  chkDueDate.Visible = true;

                        if (RadExpiryDate.SelectedDate == null)
                        {
                            ibtnDeleteDueDate.Visible = false;
                        }
                    }
                }
                /************************************/
                
                if (common.myStr(lblImmunizationDD.Text).Equals("M"))
                {
                    lblDueDate.Visible = false;
                }
                else if (common.myStr(lblImmunizationDD.Text).Equals("A"))
                {
                    lblDueDate.Visible = true;
                }

                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
                {
                    ibtnDelete.Visible = false;
                    ibtnDeleteDueDate.Visible = false;
                }

                try
                {
                    int yearFrom = common.myInt(common.myDbl(common.myDbl(common.myDbl(hdnDaysFrom.Value) / 30.0) / 12.0));
                    int yearTo = common.myInt(common.myDbl(common.myDbl(common.myDbl(hdnDaysTo.Value) / 30.0) / 12.0));

                    if (yearFrom > 11 && yearTo > 11)
                    {
                        e.Row.Cells[0].BackColor = System.Drawing.Color.LightBlue;
                    }
                }
                catch
                {
                }

            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvDue_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DEACTIVATE"))
            {
                if (common.myBool(ViewState["IsAllowToCancel"]))
                {
                    BaseC.EMRImmunization immu = new BaseC.EMRImmunization(sConString);

                    int i = immu.InActivePatientImmunization(Convert.ToInt16(e.CommandArgument), Convert.ToInt32(Session["UserId"]), DateTime.Now, txtcancel.Text, "G");
                    if (i == 0)
                    {
                        lblmsg.Text = "Record De-Actived.";
                    }
                    txtcancel.Text = "";
                    BindgvDueDateGrid();
                }
                else
                {
                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblmsg.Text = "No permission to deactivate!";
                    Alert.ShowAjaxMsg(lblmsg.Text, this);
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("DEACTIVATEDUEDATE"))
            {
                if (common.myBool(ViewState["IsAllowToCancel"]))
                {
                    BaseC.EMRImmunization immu = new BaseC.EMRImmunization(sConString);

                    int i = immu.InActivePatientImmunization(Convert.ToInt16(e.CommandArgument), Convert.ToInt32(Session["UserId"]), DateTime.Now, txtcancel.Text, string.Empty);
                    // int i = immu.InActivePatientImmunization(Convert.ToInt16(e.CommandArgument), Convert.ToInt32(Session["UserId"]), DateTime.Now, txtcancel.Text);

                    if (i == 0)
                    {
                        lblmsg.Text = "Record De-Actived.";
                    }
                    txtcancel.Text = "";
                    BindgvDueDateGrid();
                }
                else
                {
                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblmsg.Text = "No permission to deactivate!";
                    Alert.ShowAjaxMsg(lblmsg.Text, this);
                    return;
                }
            }

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnclose_OnClick(object sender, EventArgs e)
    {
        BindgvDueDateGrid();
    }
    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            Button btn = sender as Button;

            GridViewRow row = btn.NamingContainer as GridViewRow;
            Label lblImmId = row.FindControl("lblImmunizationId") as Label;
            Label lblImmName = row.FindControl("lblImmunizationName") as Label;
            HiddenField lblScheduleID = row.FindControl("lblScheduleID") as HiddenField;

            HiddenField lblID = row.FindControl("lblID") as HiddenField;
            HiddenField hdnBrandId = (HiddenField)row.FindControl("hdnBrandId");
            string Type = row.Cells[4].Text == "No" ? "N" : "Y";

            //   Session["ImmuRegistrationID"] = ViewState["RegistrationID"];

            RadWindow1.NavigateUrl = "/EMR/Immunization/ImmunizationTrackingDialog.aspx?Id=" + lblID.Value + "&ImmId=" + lblImmId.Text + "&ImmName=" + lblImmName.Text + "&SchId=" + lblScheduleID.Value + "&Type=" + Type + "&Bid=" + common.myInt(hdnBrandId.Value) + "&RegId=" + common.myInt(ViewState["RegistrationID"]) + "&Mpg=" + common.myStr(Request.QueryString["Mpg"]);
            RadWindow1.Height = 500;
            RadWindow1.Width = 600;
            RadWindow1.Top = 20;
            RadWindow1.Left = 20;
            RadWindow1.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void lnkAlerts_OnClick(object sender, EventArgs e)
    {
        try
        {


            RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&PId=" + common.myStr(ViewState["RegistrationID"]);
            RadWindow1.Height = 600;
            RadWindow1.Width = 600;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.EMRImmunization Immu = new BaseC.EMRImmunization(sConString);


            ArrayList coll = new ArrayList();
            StringBuilder strXML = new StringBuilder();
            foreach (GridViewRow row in gvDue.Rows)
            {
                //string ServiceId = row.Cells[0].Text;
                int io = gvDue.Rows.Count;
                //bool Stat = common.myBool(row.Cells[8].Text);
                HiddenField lblScheduleID = (HiddenField)row.FindControl("lblScheduleID");
                Label lblImmunizationId = (Label)row.FindControl("lblImmunizationId");
                Label lblDueDate = (Label)row.FindControl("lblDueDate");
                RadDatePicker RadExpiryDate = (RadDatePicker)row.FindControl("RadExpiryDate");



                Label lblGivenDate = (Label)row.FindControl("lblGivenDate");
                Label lblDueId = (Label)row.FindControl("lblDueId");
                HiddenField hdnImmunizationDueDateCount = (HiddenField)row.FindControl("hdnImmunizationDueDateCount");
                HiddenField hdnRejectedByPatient = (HiddenField)row.FindControl("hdnRejectedByPatient");
                ImageButton ibtnDelete = (ImageButton)row.FindControl("ibtnDelete");
                HiddenField hdnGivenStatus = (HiddenField)row.FindControl("hdnGivenStatus");



                /* TEMP KABIR */
                //string flagAutoManualDueDate = string.Empty; // A or M
                //flagAutoManualDueDate = "M";
                /* TEMP KABIR */

                //   if (flagAutoManualDueDate.Equals("M"))
                //   {
                //       //lblDueDate.Visible = false;
                //       //RadExpiryDate.Visible = true;
                //     Immu.SaveEMRPatientDueImmunizationDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["RegistrationID"]),common.myStr ( strXML), common.myStr ( RadExpiryDate.SelectedDate) , true, common.myInt(Session["UserId"]), common.myInt(Session["UserId"]));


                //   }
                //   else if (flagAutoManualDueDate.Equals("A"))
                //   {
                //       //lblDueDate.Visible = true;
                //       //RadExpiryDate.Visible = false;
                //Immu.SaveEMRPatientDueImmunizationDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["RegistrationID"]), common.myStr(strXML), common.myStr(lblDueDate.Text ), true, common.myInt(Session["UserId"]), common.myInt(Session["UserId"]));

                //   }


                if (hdnGivenStatus.Value == "No" &&
                     hdnRejectedByPatient.Value == "False")
                {
                    if (common.myStr(lblImmunizationDD.Text).Equals("M"))
                    {

                        // Immu.SaveEMRPatientDueImmunizationDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["RegistrationID"]),common.myStr ( strXML), common.myStr ( RadExpiryDate.SelectedDate) , true, common.myInt(Session["UserId"]), common.myInt(Session["UserId"]));

                        if (lblScheduleID.Value != string.Empty && common.myStr(lblImmunizationId.Text) != string.Empty)
                        {

                            if (RadExpiryDate.SelectedDate != null)
                            {
                                // coll.Add(Convert.ToDateTime (RadExpiryDate.SelectedDate));
                                coll.Add(common.myInt(lblScheduleID.Value));
                                coll.Add(common.myInt(lblImmunizationId.Text));
                                coll.Add(Convert.ToDateTime(RadExpiryDate.SelectedDate).ToString("dd/MM/yyyy"));
                                coll.Add(common.myInt(lblDueId.Text));

                            }
                            else
                            {
                                // coll.Add(common.myStr("01-01-1900"));
                            }

                            strXML.Append(common.setXmlTable(ref coll));
                        }
                    }


                }



            }
            string chvErrorStatus = Immu.SaveEMRPatientDueImmunizationDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["RegistrationID"]), common.myStr(strXML), common.myInt(Session["UserId"]));


            if (chvErrorStatus.Contains("Saved"))
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblmsg.Text = chvErrorStatus;
                BindgvDueDateGrid();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);

        }
    }

    protected void lbtnSearchPatient_Click(object sender, EventArgs e)
    {
        //ClearFields();
        txtAccountNo.Text = string.Empty;
        RadWindow1.NavigateUrl = "/EMR/PatientDetails.aspx?OPIP=O&RegEnc=5&PageFrom=ImmunizationBabyDueDate";
        RadWindow1.Height = 500;
        RadWindow1.Width = 970;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }


    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        BaseC.EMRImmunization immu = new BaseC.EMRImmunization(sConString);
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet dsPatientDetail = new DataSet();
        DataTable dtRegistrationID = new DataTable();
        try
        {


            ViewState["RegistrationID"] = hdnRegistrationId.Value;
            ViewState["IsMedicalAlert"] = hdnIsMedicalAlert.Value;
            if (common.myStr(txtAccountNo.Text).Equals(string.Empty))
            {
                ViewState["RegistrationNo"] = hdnRegistrationNo.Value;
                ViewState["RegistrationID"] = hdnRegistrationId.Value;
            }
            else
            {
                dtRegistrationID = immu.GetImmuRegistrationId(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(txtAccountNo.Text));
                ViewState["RegistrationNo"] = txtAccountNo.Text;
                if (common.myInt(dtRegistrationID.Rows.Count) > 0)
                {
                    ViewState["RegistrationID"] = common.myStr(dtRegistrationID.Rows[0]["RegistrationId"]);
                    Session["RegistrationID1"] = ViewState["RegistrationID"];
                }
            }

            ViewState["EncounterNo"] = hdnEncounterNo.Value;


            if (ViewState["RegistrationID"] != null && common.myStr(ViewState["RegistrationID"].ToString()) != "" && Convert.ToInt32(ViewState["RegistrationID"]) > 0)
            {
                lbldob.Text = immu.GetPatientDOB(Convert.ToInt32(ViewState["RegistrationID"]), Convert.ToInt16(Session["HospitalLocationID"]));
                BindgvDueDateGrid();
            }
            if (!common.myStr(ViewState["RegistrationNo"]).Trim().Equals(string.Empty))
            {
                dsPatientDetail = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                         common.myStr(ViewState["RegistrationNo"]), common.myStr(ViewState["EncounterNo"]), common.myInt(Session["UserId"]), 0); // "0" encounter no

                if (dsPatientDetail.Tables.Count > 0)
                {
                    if (dsPatientDetail.Tables[0].Rows.Count > 0)
                    {
                        string sRegNoTitle = Resources.PRegistration.regno;
                        string sDoctorTitle = Resources.PRegistration.Doctor;
                        string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date :" : "Encounter Date :";
                        ViewState["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["GenderAge"]) + "</span>"
                                        + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                                        + "&nbsp;Enc #:" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]) + "</span>"
                                        + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"
                                        //+ DateTitle + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(Session["EncounterDate"]) + "</span>"
                                        + "&nbsp;Bed:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["BedNo"]) + "</span>"
                                        + "&nbsp;Ward:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["WardName"]) + "</span>"
                                        + "&nbsp;Mobile:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                                        + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]) + "</span>"
                                        + "</b>";
                        lblPatientDetail.Text = common.myStr(ViewState["PatientDetailString"]);
                        txtAccountNo.Text = common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]);
                    }
                }
            }
            //if (common.myStr(ViewState["IsMedicalAlert"]) == "")
            //{
            //    lnkAlerts.Enabled = false;
            //    lnkAlerts.CssClass = "blinkNone";
            //    lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
            //}
            //else if (common.myStr(ViewState["IsMedicalAlert"]).ToUpper() == "YES")
            //{
            //    lnkAlerts.Enabled = true;
            //    lnkAlerts.Font.Bold = true;
            //    lnkAlerts.Font.Size = 11;
            //    lnkAlerts.CssClass = "blink";
            //    lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
            //}

            if (common.myBool(ViewState["IsMedicalAlert"]))
            {
                lnkAlerts.Enabled = true;
                lnkAlerts.Font.Bold = true;
                lnkAlerts.Font.Size = 11;
                lnkAlerts.CssClass = "blink";
                lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
            }
            else
            {
                lnkAlerts.Enabled = false;
                lnkAlerts.CssClass = "blinkNone";
                lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
            }

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            immu = null;
            patient = null;
            dsPatientDetail.Dispose();
        }

    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        RadWindow3.NavigateUrl = "/EMR/Immunization/PrintPdf.aspx?PrintHeader=1&EncId=" + common.myInt(ViewState["EncounterId"]) + "&RegistrationID=" + Convert.ToInt32(Session["RegistrationID1"]) + "&RegistrationNo=" + common.myStr(ViewState["RegistrationNo"]);
        RadWindow3.Height = 598;
        RadWindow3.Width = 900;
        RadWindow3.Top = 10;
        RadWindow3.Left = 10;
        RadWindow3.Modal = true;
        RadWindow3.OnClientClose = "OnClientClose";
        RadWindow3.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow3.VisibleStatusbar = false;
        //BaseC.EMRImmunization Immu = new BaseC.EMRImmunization(sConString);
        //string dob = string.Empty;
        //try
        //{

        //    if (Session["RegistrationID"] != null && common.myInt(Session["RegistrationID"]) > 0)
        //    {
        //        string Reg = Session["RegistrationID"].ToString();
        //        dob = Immu.GetPatientDOB(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt16(Session["HospitalLocationID"]));

        //    }
        //}
        //catch { }

        //    RadWindow2.NavigateUrl = "/EMRReports/PrintReport.aspx?dob=" + dob
        //            + "&ReportName=Immunization";

        //RadWindow2.Height = 480;
        //RadWindow2.Width = 850;
        //RadWindow2.Top = 40;
        //RadWindow2.Left = 100;
        //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindow2.Modal = true;
        ////  RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        //RadWindow2.VisibleStatusbar = false;
    }
    //protected void onchkDueDate(object sender, EventArgs e)
    //{
    //    GridViewRow row = (GridViewRow)(((CheckBox)sender).NamingContainer);

    //    RadDatePicker RadExpiryDate = (RadDatePicker)row.FindControl("RadExpiryDate");

    //       CheckBox chkDueDate = (CheckBox)sender;

    //    if(chkDueDate.Checked)
    //    {
    //        RadExpiryDate.SelectedDate = null;
    //    }
    //    else
    //    {
    //        BindgvDueDateGrid();

    //    }

    //}

    protected void lnkDailyInjection_OnClick(object sender, EventArgs e)
    {
        txtAccountNo.Text = string.Empty;
        RadWindow1.NavigateUrl = "/EMR/Immunization/DailyInjection.aspx?MASTER=NO";
        RadWindow1.Height = 530;
        RadWindow1.Width = 1200;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = string.Empty;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }

}

