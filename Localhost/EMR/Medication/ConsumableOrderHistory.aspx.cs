using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
using System.Web;

public partial class EMR_Medication_ConsumableOrderHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportServer = System.Configuration.ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = System.Configuration.ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = System.Configuration.ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = System.Configuration.ConfigurationManager.AppSettings["SysDomain"];

    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["OPIP"]) == "O")
        {
            ViewState["OPIP"] = "O";
        }
        else
        {
            ViewState["OPIP"] = "I";
        }

        if (common.myInt(Session["EncounterId"]) == 0)
        {
            Response.Redirect("/default.aspx?RegNo=0", false);
        }

        if (!IsPostBack)
        {
            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtFromDate.SelectedDate = DateTime.Now.AddDays(-30);

            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.SelectedDate = DateTime.Now;

            bindData();
            if (common.myBool(Request.QueryString["IsConsignment"]))
            {
                Chktoprinter.Visible = true;
                Chktoprinter.Checked = true;
            }
        }
    }


    protected void gvPrevious_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ITEMCANCEL"))
            {
                int ItemId = common.myInt(e.CommandArgument);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnDetailsId = (HiddenField)row.FindControl("hdnDetailsId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                if (common.myInt(hdnDetailsId.Value).Equals(0))
                {
                    return;
                }

                if (!common.myInt(hdnEncounterId.Value).Equals(common.myInt(Session["EncounterId"])))
                {
                    Alert.ShowAjaxMsg("Cancellation allowed only on current selected encounter !", this.Page);
                    return;
                }

                ViewState["IndentDetailsId"] = common.myInt(hdnDetailsId.Value).ToString();
                ViewState["StopItemId"] = common.myInt(ItemId).ToString();
                ViewState["StopIndentId"] = common.myStr(hdnIndentId.Value);

                ViewState["CancelBrandList"] = string.Empty;

                txtStopRemarks.Text = string.Empty;
                dvConfirmStop.Visible = true;

                //lblCancelStopMedicationRemarks.Text = "Cancel Medication Remarks";
                //btnStopMedication.Text = "Cancel";
            }
            else if (e.CommandName.Equals("PRINT"))
            {
                int intPrescriptionId = common.myInt(e.CommandArgument);
                string DirectPRintingURL = string.Empty;

                System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

                collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                        "DirectPRintingURL", sConString);

                if (collHospitalSetupValues.ContainsKey("DirectPRintingURL"))
                    //ViewState["DirectPRintingURL"] = collHospitalSetupValues["DirectPRintingURL"];
                    DirectPRintingURL = collHospitalSetupValues["DirectPRintingURL"];

                if (intPrescriptionId.Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Prescription not Selected !";
                    return;
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                if (Chktoprinter.Checked && common.myBool(Request.QueryString["IsConsignment"]))
                {
                    System.Text.StringBuilder str = new System.Text.StringBuilder();
                    string printURL = common.myStr(DirectPRintingURL);
                    BaseC.User valUser = new BaseC.User(sConString);
                   // DataSet ds = new DataSet();
                    clsPrinter _printer = null;
                    string sUserName = string.Empty;
                    try
                    {
                        if (common.myStr(printURL) != "")
                        {
                            string[] hostName = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
                            _printer = new clsPrinter(sConString, "ConsignmentIndent", 0, common.myStr(hostName[0].ToUpper().Trim()));

                            sUserName = common.myStr(Session["EmployeeName"]).Trim();

                            str.Clear();
                            str.Append("UserName/");
                            str.Append(sUserName);
                            str.Append("!");

                            str.Append("inyHospitalLocationId/");
                            str.Append(common.myInt(Session["HospitalLocationID"]).ToString());
                            str.Append("!");

                            str.Append("intFacilityId/");
                            str.Append(common.myInt(Session["FacilityID"]).ToString());
                            str.Append("!");

                            str.Append("intEncounterId/");
                            str.Append(common.myInt(hdnEncounterId.Value));
                            str.Append("!");

                            str.Append("intIndentId/");
                            str.Append(intPrescriptionId);
                            str.Append("!");

                            str.Append("UHID/");
                            str.Append((string)HttpContext.GetGlobalResourceObject("PRegistration", "UHID"));
                            str.Append("!");

                            str.Append("intOTBookingID/");
                            str.Append(common.myInt(Request.QueryString["OTBookingId"]));

                            //string Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/" + _printer.ReportName + "$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + _printer.PageMargin;
                            string Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/ConsignmentIndent$"  + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + _printer.PageMargin;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);

                            return;
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        clsExceptionLog objException = new clsExceptionLog();
                        objException.HandleException(ex);
                        objException = null;
                    }
                    finally
                    {
                        valUser = null; ;
                    //    ds.Dispose();
                        _printer = null;
                    }
                }

                else
                {
                    if (common.myBool(Request.QueryString["IsConsignment"]))
                    {
                        RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=CONOD&EncId=" + common.myInt(hdnEncounterId.Value) + "&PId=" + intPrescriptionId + "&OTBookingId=" + common.myInt(Request.QueryString["OTBookingId"]);
                    }
                    else
                    {
                        RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" + common.myInt(hdnEncounterId.Value) + "&PId=" + intPrescriptionId;

                    }

                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = string.Empty;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;

                }
                //RadWindow1.Height = 600;
                //RadWindow1.Width = 900;
                //RadWindow1.Top = 10;
                //RadWindow1.Left = 10;
                //RadWindow1.Modal = true;
                //RadWindow1.VisibleOnPageLoad = true;
                //RadWindow1.VisibleStatusbar = false;
                //RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnStopMedication_OnClick(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        try
        {
            if (common.myLen(txtStopRemarks.Text) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Remarks can't be blank !";
                Alert.ShowAjaxMsg("Remarks can't be blank !", this);
                return;
            }

            if (common.myInt(ViewState["StopItemId"]) > 0 || common.myInt(ViewState["IndentDetailsId"]) > 0)
            {
                Hashtable hshOutput = new Hashtable();


                hshOutput = objEMR.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                common.myInt(ViewState["StopIndentId"]), common.myInt(ViewState["StopItemId"]), common.myInt(Session["UserId"]),
                                                common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                                                1, common.myStr(txtStopRemarks.Text).Trim(),
                                                common.myStr(Session["OPIP"]), common.myInt(ViewState["IndentDetailsId"]));

                bindData();

                ViewState["IndentDetailsId"] = "0";
                if (common.myStr(hshOutput["@chvOutPut"]).ToUpper().Equals("SUCCESSFULL"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }

                lblMessage.Text = common.myStr(hshOutput["@chvOutPut"]);
            }
            else
            {
                Alert.ShowAjaxMsg("Drug not selected !", this);

                return;
            }

            ViewState["StopItemId"] = string.Empty;
            ViewState["StopIndentId"] = string.Empty;

            dvConfirmStop.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
        }
    }

    protected void btnStopClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            dvConfirmStop.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    protected void bindData()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement objWard = new BaseC.WardManagement();
        try
        {
            string fromDate = null;
            string toDate = null;
            switch (common.myStr(ddlSearchOn.SelectedValue))
            {
                case "A":
                    break;

                case "D":
                    fromDate = common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd");
                    toDate = common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd");

                    break;
            }
            ds = objWard.GetConsumableOrderHistory(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                common.myInt(Session["RegistrationId"]), common.myStr(txtDrugName.Text).Trim(), fromDate, toDate,common.myStr(Session["OPIP"]));
            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
                gvPrevious.DataSource = ds.Tables[0];
            }
            else
            {
                if (common.myBool(Request.QueryString["IsConsignment"]))
                {
                    DataView dvConsignment = new DataView(ds.Tables[0]);
                    dvConsignment.RowFilter = "IndentCode='CONOD' ";
                    gvPrevious.DataSource = dvConsignment.ToTable();
                }
                else
                {
                    //DataView dvConsignment = new DataView(ds.Tables[0]);
                    //dvConsignment.RowFilter = "IndentCode<>'CONOD' ";
                    gvPrevious.DataSource = ds.Tables[0];
                }
            }
            gvPrevious.DataBind();
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
            objWard = null;
        }
    }


    protected void ddlSearchOn_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            switch (common.myStr(ddlSearchOn.SelectedValue))
            {
                case "A":
                    tblDateRange.Visible = false;

                    break;

                case "D":
                    tblDateRange.Visible = true;
                    txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-30);

                    txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                    txtToDate.SelectedDate = DateTime.Now;

                    break;
            }

            bindData();
        }
        catch
        {
        }
    }

    protected void btnFilter_Onclick(object sender, EventArgs e)
    {
        bindData();
    }
     
    protected void gvPrevious_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPrevious.PageIndex = e.NewPageIndex;
        bindData();
    }
    
}