using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Text;
using System.Configuration;
using BaseC;

/* Manoj Puri 15012021 : Added try catch block and free objects memory allocation etc. for code improvement */

public partial class LIS_Phlebotomy_SampleCollection : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsLISPhlebotomy objval;
    string lblEncodedDate = "";

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = common.myStr(Request.QueryString["Show"]).Equals("Ack") ? "Sample Acknowledge" :
            (common.myStr(Request.QueryString["Show"]).Equals("ResultEntry") ? "Result Entry" : "Sample Collection Details");
        lblEncodedDate = common.myStr(Request.QueryString["lblEncodedDate"]);
        if (!IsPostBack)
        {
            LoadFlagHospitalsetup();
            if (Request.QueryString["RegNo"] != null && Request.QueryString["PName"] != null)
            {
                this.lblPatientDetails.Text = HttpContext.GetGlobalResourceObject("PRegistration", "regno") + ": " +
                    common.myStr(Request.QueryString["RegNo"]) + " | Patient Name: " + common.myStr(Request.QueryString["PName"]).Trim();
            }
            //Awadhesh
            if (Request.QueryString["IsAllowBackDateSampleCollection"] != null)
            {
                if (common.myStr(Request.QueryString["IsAllowBackDateSampleCollection"]).Equals("Y"))
                {
                    txtDate.Enabled = true;
                    RadComboBox1.Enabled = true;
                }
            }
            //

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
                lblMessage.Font.Name = commonLabelSetting.cFont;

            txtDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]) + " hh:mm tt";
            txtDate.SelectedDate = DateTime.Now;

            if (common.myStr(Request.QueryString["Show"]).Equals("Ack") || common.myStr(Request.QueryString["Show"]).Equals("ResultEntry"))
            {
                if (Request.QueryString["IsEnableUserAuthentication"] == null ||
                        !common.myStr(Request.QueryString["IsEnableUserAuthentication"]).Equals("Y"))
                {
                    // ViewState["IsEnableUserAuthentication"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),common.myInt(Session["FacilityId"]), "UserAuthenticationForLISAndPhlebotomy", sConString);
                    //ViewState["IsEnableUserAuthentication"] = ViewState["IsEnableUserAuthentication"];
                }
                else if (common.myStr(Request.QueryString["IsEnableUserAuthentication"]).Equals("Y"))
                {
                    ViewState["IsEnableUserAuthentication"] = "Y";
                }

                Label2.Text = common.myStr(Request.QueryString["Show"]).Equals("Ack") ? "Acknowledged By" : "Result Enter By";
                Label5.Text = common.myStr(Request.QueryString["Show"]).Equals("Ack") ? "Acknowledge Date" : "Result Entry";

            }
            else
            {
                //objval = new clsLISPhlebotomy(sConString);

                ViewState["SOURCE"] = "OPD";
                // ViewState["IsEnableUserAuthentication"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),common.myInt(Session["FacilityId"]), "UserAuthenticationForLISAndPhlebotomy", sConString);

                // ViewState["VacutainerMandatoryInCollection"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),common.myInt(Session["FacilityId"]), "VacutainerMandatoryInCollection", sConString);

                if (Request.QueryString["SOURCE"] != null)
                {
                    ViewState["SOURCE"] = common.myStr(Request.QueryString["SOURCE"]);
                }
            }

            bindControl();
            BindVacutainer();

            if (common.myStr(Request.QueryString["Gender"]).Equals("M"))
            {
                rbdPregnant.Visible = false;
                LblPregnant.Visible = false;
                Span4.Visible = false;
            }
        }
    }
    private void LoadFlagHospitalsetup()
    {
        string hospitalSetupFlags = string.Empty;
        HospitalSetup objHospitalSetUP = new HospitalSetup(sConString);
        DataSet dsFlags = new DataSet();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            hospitalSetupFlags =
                "'UserAuthenticationForLISAndPhlebotomy','VacutainerMandatoryInCollection','IsValidationOfPregnancy-Phlebotomy'";
            dsFlags = objHospitalSetUP.getHospitalSetupValueMultiple(hospitalSetupFlags, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            if (dsFlags.Tables[0].Rows.Count > 0)
            {
                if (dsFlags.Tables[0].Select("Flag = 'UserAuthenticationForLISAndPhlebotomy'").Length > 0)
                {
                    ViewState["IsEnableUserAuthentication"] = common.myStr(dsFlags.Tables[0].Select("Flag = 'UserAuthenticationForLISAndPhlebotomy'")[0].ItemArray[1]);
                }
                if (dsFlags.Tables[0].Select("Flag = 'VacutainerMandatoryInCollection'").Length > 0)
                {
                    ViewState["VacutainerMandatoryInCollection"] = common.myStr(dsFlags.Tables[0].Select("Flag = 'VacutainerMandatoryInCollection'")[0].ItemArray[1]);

                    if (common.myStr(ViewState["VacutainerMandatoryInCollection"]) == "Y")
                    {
                        divVacutainer.Visible = true;
                    }
                    else { divVacutainer.Visible = false; }
                }
                else { divVacutainer.Visible = false; }

                if (dsFlags.Tables[0].Select("Flag = 'IsValidationOfPregnancy-Phlebotomy'").Length > 0)
                {
                    //   ViewState["IsValidationOfPregnancy-Phlebotomy"] 
                    string IsValidate = common.myStr(dsFlags.Tables[0].Select("Flag = 'IsValidationOfPregnancy-Phlebotomy'")[0].ItemArray[1]);
                    if (IsValidate == "Y")
                    {
                        divPregnant.Visible = true;
                    }
                    else { divPregnant.Visible = false; }
                }
                else { divPregnant.Visible = false; }

            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objHospitalSetUP = null;
            dsFlags.Dispose();
            objException = null;
        }
    }
    private void bindControl()
    {
        clsExceptionLog objException = new clsExceptionLog();
        StringBuilder strType = new StringBuilder();
        ArrayList coll = new ArrayList();
        clsLISMaster objMaster = new clsLISMaster(sConString);
        DataSet ds = new DataSet();
        try
        {
            coll.Add("LT");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("LIC");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("LS");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("LD");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("LDIR");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("N");
            strType.Append(common.setXmlTable(ref coll));

            int iHospId = common.myInt(Session["HospitalLocationID"]);
            int iStationId = 0;
            ds = objMaster.getEmployeeData(iHospId, iStationId, 0, strType.ToString(), "", 0, common.myInt(Session["UserId"]), "", 0);

            //DataView DV = ds.Tables[0].Copy().DefaultView;
            //DV.RowFilter = " EmployeeType = 'LT' ";

            ddlCollectedBy.DataSource = ds.Tables[0]; //DV.ToTable();
            ddlCollectedBy.DataValueField = "EmployeeId";
            ddlCollectedBy.DataTextField = "EmployeeNameWithNo";
            ddlCollectedBy.DataBind();

            ddlCollectedBy.Items.Insert(0, new RadComboBoxItem("", "0"));

            ddlCollectedBy.SelectedIndex = ddlCollectedBy.Items.IndexOf(ddlCollectedBy.Items.FindItemByValue(common.myStr(common.myInt(Session["EmployeeId"]))));
            ddlCollectedBy.Enabled = common.myStr(ViewState["IsEnableUserAuthentication"]).Equals("Y");
            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                {
                    RadComboBox1.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    RadComboBox1.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
            }
            int iMinute = DateTime.Now.Minute;
            RadComboBoxItem rcbItem = (RadComboBoxItem)RadComboBox1.Items.FindItemByText(iMinute.ToString());
            if (rcbItem != null)
            {
                rcbItem.Selected = true;
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
            objException = null;
            strType = null;
            coll = null;
            objMaster = null;
            ds.Dispose();
        }
    }
    private void BindVacutainer()
    {
        string strSelectedServices = string.Empty;
        strSelectedServices = common.myStr(Request.QueryString["Services"]);
        clsExceptionLog objException = new clsExceptionLog();
        clsOtherServiceDetails objVacutainer = new clsOtherServiceDetails(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objVacutainer.GetVacutainer(0, common.myInt(Session["HospitalLocationID"]), strSelectedServices);

            DataView objDv = ds.Tables[0].DefaultView;
            objDv.RowFilter = "Active = 1";
            objDv.Sort = "VacutainerName";
            if (objDv.Count > 0)
            {
                rcbVacutainer.DataSource = objDv;
                rcbVacutainer.DataValueField = "VacutainerId";
                rcbVacutainer.DataTextField = "VacutainerName";
                rcbVacutainer.DataBind();
                if (common.myInt(ds.Tables[0].Rows.Count) > 1)
                {
                    rcbVacutainer.Items.Insert(0, new RadComboBoxItem("", "0"));
                    rcbVacutainer.SelectedIndex = 0;
                }
                else
                {
                    rcbVacutainer.SelectedIndex = 0;
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
            objException = null;
            objVacutainer = null;
            ds.Dispose();
        }
    }
    protected void btnAddVacutainer_OnClick(Object sender, EventArgs e)
    {
        if (common.myInt(rcbVacutainer.SelectedValue) > 0)
        {
            DataTable dt;
            if (ViewState["Vacutainer"] == null)
            {
                dt = new DataTable();
                dt.Columns.Add("VacutainerId");
                dt.Columns.Add("VacutainerName");
                dt.Columns.Add("VacutainerQuantity");
            }
            else
            {
                dt = (DataTable)ViewState["Vacutainer"];
            }
            DataRow dr = dt.NewRow();
            dr["VacutainerId"] = common.myInt(rcbVacutainer.SelectedValue);
            dr["VacutainerName"] = common.myStr(rcbVacutainer.SelectedItem.Text);
            dr["VacutainerQuantity"] = common.myInt(txtVacutainerQuantity.Text);
            dt.Rows.Add(dr);
            ViewState["Vacutainer"] = dt;
            this.BindGrid();
            rcbVacutainer.SelectedIndex = 0;
            txtVacutainerQuantity.Text = "1";
            lblMessage.Text = string.Empty;
            dt.Dispose();
        }
        else { lblMessage.Text = "Please Select Vacutainer Type !"; }
    }

    protected void BindGrid()
    {
        gvVacutainer.DataSource = (DataTable)ViewState["Vacutainer"];
        gvVacutainer.DataBind();
    }

    private bool isSave()
    {
        bool isSave = true;
        string msg = "";

        if (common.myLen(Session["SampleCollectionIds"]) == 0)
        {
            msg += "Please Select Sample !";
            isSave = false;
        }
        if (common.myInt(ddlCollectedBy.SelectedValue) < 1)
        {
            msg += "Please Select Sample Collected By !";
            isSave = false;
        }


        if (common.myStr(Request.QueryString["IsAllowBackDateSampleCollection"]).Equals("Y"))
        {
            //Awadhesh
            if (common.myDate(common.myDate(txtDate.SelectedDate).ToString("dd/MM/yyyy hh:MM"))
               > common.myDate(DateTime.Now.ToString("dd/MM/yyyy hh:MM")))
            {
                msg += " Date can't be exceed current date !";
                isSave = false;
            }
            TimeSpan timeS = common.myDate(txtDate.SelectedDate.Value.ToString("dd/MM/yyyy hh:mm"))
                - common.myDate(common.myDate(Request.QueryString["lblEncodedDate"].Trim()).ToString("dd/MM/yyyy hh:mm"));

            //msg += timeS.TotalMinutes + "selected: " + common.myDate(txtDate.SelectedDate.Value).ToString("dd/MM/yyyy HH:mm") + " order Date: " + common.myDate(Request.QueryString["lblEncodedDate"].Trim()).ToString("dd/MM/yyyy HH:mm");
            if (timeS.TotalMinutes < 2)
            {
                msg += " Collection Date time can't be before Order date !";
                isSave = false;

            }

            //
        }
        else
        {
            if (common.myDate(common.myDate(txtDate.SelectedDate).ToString("dd/MM/yyyy hh:MM"))
                > common.myDate(DateTime.Now.ToString("dd/MM/yyyy hh:MM")))
            {
                msg += " Date can't be exceed current date !";
                isSave = false;
            }
        }

        lblMessage.Text = msg;

        return isSave;
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (common.myStr(Request.QueryString["Show"]).Equals("Ack") || common.myStr(Request.QueryString["Show"]).Equals("ResultEntry"))
            {
                if (ViewState["IsEnableUserAuthentication"].Equals("Y"))
                {
                    IsValidPasswordNew();
                }
            }
            else
            {
                if (!isSave())
                {
                    return;
                }
                //IsValidPassword();
                if (ViewState["IsEnableUserAuthentication"].Equals("Y"))
                {
                    IsValidPasswordNew();
                }
                else
                {
                    SaveData();
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
            objException = null;
        }
    }

    private void IsValidPasswordNew()
    {
        hdnIsValidPassword.Value = "0";
        RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=Sample&OtherUserId=" + ddlCollectedBy.SelectedValue + "&UserName=" + ddlCollectedBy.Text;
        RadWindowForNew.Height = 120;
        RadWindowForNew.Width = 340;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    //private void IsValidPassword()
    //{
    //    hdnIsValidPassword.Value = "0";
    //    RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=Sample";
    //    RadWindowForNew.Height = 120;
    //    RadWindowForNew.Width = 340;
    //    RadWindowForNew.Top = 10;
    //    RadWindowForNew.Left = 10;
    //    RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
    //    RadWindowForNew.VisibleOnPageLoad = true;
    //    RadWindowForNew.Modal = true;
    //    RadWindowForNew.VisibleStatusbar = false;
    //}

    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(hdnIsValidPassword.Value) == 0)
            {
                lblMessage.Text = "Invalid Password !";
                return;
            }
            if (common.myInt(hdnIsValidPassword.Value) == 1 && (common.myStr(Request.QueryString["Show"]).Equals("Ack") ||
                                                                common.myStr(Request.QueryString["Show"]).Equals("ResultEntry")))
            {
                lblMessage.Text = string.Empty;
                hdnDailySerialNo.Value = common.myStr(common.myInt(ddlCollectedBy.SelectedValue));
                hdnIsValidPassword.Value = "1";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "backToParent();", true);
                return;
            }
            else if (common.myInt(hdnIsValidPassword.Value) == 1)
            {
                lblMessage.Text = string.Empty;
                SaveData();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
            //return false;
        }
        finally
        {
            objException = null;
        }
    }
    private void SaveData()
    {
        clsExceptionLog objException = new clsExceptionLog();
        StringBuilder strXml = new StringBuilder();
        ArrayList coll = new ArrayList();
        objval = new clsLISPhlebotomy(sConString);
        try
        {
            int iLabNo = common.myInt(Request.QueryString["labno"]);
            hdnIsValidPassword.Value = "0";
            int DailySerialNo;

            string sFlagValue = common.myStr(ViewState["VacutainerMandatoryInCollection"]);

            if (sFlagValue.ToUpper().Equals("Y") && ViewState["Vacutainer"] == null)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select Vacutainer type and quantity";
                return;
            }
            else
            {
                if (ViewState["Vacutainer"] != null)
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["Vacutainer"];
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                        {
                            coll.Add(dtCurrentTable.Rows[i][0].ToString());
                            coll.Add(dtCurrentTable.Rows[i][2].ToString());
                            coll.Add(iLabNo);
                            strXml.Append(common.setXmlTable(ref coll));
                        }
                    }
                }
            }

            string strMsg = objval.saveSampleCollectionData(common.myStr(ViewState["SOURCE"]),
                common.myStr(Session["SampleCollectionIds"]), common.myInt(Session["FacilityID"]),
                common.myDate(txtDate.SelectedDate), common.myInt(common.myStr(Session["HospitalLocationID"])),
                common.myInt(ddlCollectedBy.SelectedValue), common.myInt(Session["UserID"]),
                out DailySerialNo, common.myStr(strXml), common.myStr(rbdPregnant.SelectedValue));

            if ((strMsg.ToUpper().Contains("RECORD SAVED...") || strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                hdnDailySerialNo.Value = Convert.ToString(DailySerialNo);
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                hdnIsValidPassword.Value = "1";
                Session["SampleCollectionIds"] = null;
                //Alert.ShowAjaxMsg(strMsg, this.Page);
                // ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                return;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }

            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objException = null;
            strXml = null;
            coll = null;
            objval = null;
        }
    }

    protected void txtDate_SelectedDateChanged(System.Object sender, System.EventArgs e)
    {
        txtDate.DateInput.DateFormat = "dd/MM/yyyy hh:mm tt";
    }

    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(txtDate.SelectedDate.Value.ToString());
        sb.Remove(txtDate.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(txtDate.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
        txtDate.SelectedDate = Convert.ToDateTime(sb.ToString());
    }
}