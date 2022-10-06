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
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Vitals_PatientVitaldetails : System.Web.UI.Page
{
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();

    //string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    // DAL.DAL dl = new DAL.DAL();
    DL_Funs fun = new DL_Funs(); 
    //BaseC.ParseData bcText = new BaseC.ParseData();
    //BaseC.EMRVitals objv;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
       // Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindGridViewCancel();

            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
            {
                ibtnCancelOk.Visible = false;
            }

            SetPermission();
        }
    }

    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations("");
        try
        {
            ibtnCancelOk.Enabled = ua1.CheckPermissionsForEMRModule("C", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));

            ViewState["IsAllowEdit"] = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
        }
        catch (Exception Ex)
        {
        }
        finally
        {
            ua1.Dispose();
        }
    }

    protected void gvvitalsCancel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvvitalsCancel.PageIndex = e.NewPageIndex;
        bindGridViewCancel();
    }

    void bindGridViewCancel()
    {
        try
        {
            //objv = new BaseC.EMRVitals(sConString);
            bool bitVitalValues = false;
            //populate Ward drop down control
           // DataSet objDs = new DataSet();

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GetPatientEncounterVitals";
            APIRootClass.vital vital = new global::APIRootClass.vital();
            vital.Encounterid = common.myInt(Session["EncounterId"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(vital);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


            //objDs = (DataSet)objv.GetPatientEncounterVitals(common.myInt(Session["EncounterId"]));
            gvvitalsCancel.DataSource = objDs;
            if (objDs.Tables[0].Rows.Count == 0)
            {
                DataTable tbl = new DataTable();
                tbl.Columns.Add("SlNo");
                tbl.Columns.Add("VitalDate");
                tbl.Columns.Add("VitalSignName");
                tbl.Columns.Add("VitalValue");
                tbl.Columns.Add("Billable");

                tbl.Columns.Add("Remarks");
                tbl.Columns.Add("EnteredBy");
                tbl.Columns.Add("EncodedBy");
                tbl.Columns.Add("Encoded Date");
                tbl.Columns.Add("VitalId");
                tbl.Columns.Add("Id");
                tbl.Columns.Add("Active");
                tbl.Columns.Add("AbNormal");
                tbl.Columns.Add("DisplayName");
                tbl.Columns.Add("IsInvestigation");

                DataRow dr = tbl.NewRow();
                tbl.Rows.Add(dr);
                gvvitalsCancel.DataSource = tbl;

            }
            gvvitalsCancel.DataBind();

            #region Log each encounter when these vitals(o Height o Weight o Blood pressure o Calculate and display BMI o Plot and display growth charts for children 2-20 years, including BMI) are entered
            for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
            {
                if (objDs.Tables[0].Rows.Count < 5)
                {
                    return;
                }
                if (objDs.Tables[0].Rows[i]["VitalSignName"].ToString().Trim() == "Height (HT)")
                {
                    bitVitalValues = true;
                    continue;
                }


                else if (objDs.Tables[0].Rows[i]["VitalSignName"].ToString().Trim() == "Weight (WT)")
                {
                    bitVitalValues = true;
                    continue;
                }

                else if (objDs.Tables[0].Rows[i]["VitalSignName"].ToString().Trim() == "BP Systolic (BPS)")
                {
                    bitVitalValues = true;
                    continue;
                }

                else if (objDs.Tables[0].Rows[i]["VitalSignName"].ToString().Trim() == "BP Diastolic (BPD)")
                {
                    bitVitalValues = true;
                    continue;
                }

                else if (objDs.Tables[0].Rows[i]["VitalSignName"].ToString().Trim() == "BMI (BMI)")
                {
                    bitVitalValues = true;
                    continue;
                }

            }
            if (bitVitalValues == true)
            {

                ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/SaveEMRMUDLogVitalValues";
                APIRootClass.SaveEMRMUDLogVitalValues objRoot = new global::APIRootClass.SaveEMRMUDLogVitalValues();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationID"]);
                objRoot.EncounterId = common.myInt(Session["encounterid"]);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.UserId = common.myInt(Session["UserID"]);
                objRoot.VitalValues = bitVitalValues;
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(vital);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                string RetMsg = JsonConvert.DeserializeObject<string>(sValue);

                //objv.SaveEMRMUDLogVitalValues(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]), common.myInt(Session["DoctorID"]),
                //    common.myInt(Session["UserID"]), bitVitalValues);
            }
            #endregion
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ibtnCancelOk_Click(object sender, EventArgs e)
    {
        try
        {
           /// objv = new BaseC.EMRVitals(sConString);
            string strsave = "";

            int Flag = 0;
            String xmlstring = "";



            if (gvvitalsCancel.Rows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < gvvitalsCancel.Rows.Count; i++)
            {
                CheckBox chkCancel = new CheckBox();
                chkCancel = (CheckBox)gvvitalsCancel.Rows[i].FindControl("chkCancel");
                string sUserId = common.myStr(((HiddenField)gvvitalsCancel.Rows[i].FindControl("hdnEncodedBy")).Value);
                string sVitalName = common.myStr(gvvitalsCancel.Rows[i].Cells[2].Text);

                if (chkCancel.Visible && chkCancel.Checked)
                {
                    Flag = 1;

                    xmlstring = xmlstring + "<Table1><c1>" + gvvitalsCancel.Rows[i].Cells[8].Text + "</c1></Table1>";

                    //if (common.myStr(Session["UserId"]) == sUserId)
                    //{
                    //    xmlstring = xmlstring + "<Table1><c1>" + gvvitalsCancel.Rows[i].Cells[8].Text + "</c1></Table1>";
                    //}
                    //else if (common.myStr(Session["IsAdminGroup"]) == "True" || common.myStr(Session["IsAdminGroup"]) == "1")
                    //{
                    //    xmlstring = xmlstring + "<Table1><c1>" + gvvitalsCancel.Rows[i].Cells[8].Text + "</c1></Table1>";
                    //}
                    //else
                    //{
                    //    Alert.ShowAjaxMsg("Vital Value " + sVitalName + " Cannot Be Deleted. Vital Given By Other User", Page.Page);
                    //    Flag = 0;
                    //    return;
                    //}
                }
            }

            if (Flag == 1)
            {

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/CancelPatientVitals";
                APIRootClass.CancelVital vital = new global::APIRootClass.CancelVital();
                vital.CancelremarkId = common.myInt(rblCancelRemarks.SelectedValue);
                vital.RegId = common.myInt(Session["RegistrationId"]);
                vital.EncId = common.myInt(Session["encounterid"]);
                vital.HospId = common.myInt(Session["HospitalLocationID"]);
                vital.FacilityId = common.myInt(Session["FacilityID"]);
                vital.PageId = common.myInt(ViewState["PageId"]);
                vital.Encodedby = common.myInt(Session["UserId"]);
                vital.xmlstring = xmlstring.ToString();
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(vital);
                string sValue = client.UploadString(ServiceURL, inputJson);
                strsave = JsonConvert.DeserializeObject<string>(sValue);
                //strsave = JsonConvert.DeserializeObject<string>(sValue);


               // strsave = objv.CancelPatientVitals(common.myInt(rblCancelRemarks.SelectedValue), common.myInt(Session["RegistrationId"]), common.myInt(Session["encounterid"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(ViewState["PageId"]), common.myInt(Session["UserId"]), xmlstring.ToString());
                if (strsave == "")
                {
                    lblMessage.Text = "Selected Vital(s) cancelled.";
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }
                else
                {
                    lblMessage.Text = strsave;
                }
                bindGridViewCancel();
            }
            else
            {
                lblMessage.Text = "Select one of them!";
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void chkheader_OnCheckedChanged(object sender, EventArgs e)
    {
        GridViewRow Row = ((GridViewRow)((Control)sender).Parent.Parent);
        CheckBox chkheader = (CheckBox)Row.FindControl("chkheader");
        if (chkheader.Checked == true)
        {
            for (int i = 0; i < gvvitalsCancel.Rows.Count; i++)
            {
                CheckBox chkCancel = new CheckBox();
                chkCancel = (CheckBox)gvvitalsCancel.Rows[i].FindControl("chkCancel");
                chkCancel.Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < gvvitalsCancel.Rows.Count; i++)
            {
                CheckBox chkCancel = new CheckBox();
                chkCancel = (CheckBox)gvvitalsCancel.Rows[i].FindControl("chkCancel");
                chkCancel.Checked = false;
            }
        }
    }

    protected void gvvitalsCancel_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////OnPageIndexChanging="gvvitalsCancel_PageIndexChanging"  PageSize="12" AllowPaging="true" 
        //Byte A;
        //Byte R;
        //Byte G;
        //Byte B;
        //string htmlHexColorValue = "";
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
            if (e.Row.Cells[10].Text == "True")
            {
                e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
            }
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[11].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnIsInvestigation = (HiddenField)e.Row.FindControl("hdnIsInvestigation");

            if (common.myStr(e.Row.Cells[3].Text) != "")
            {
                e.Row.Cells[3].Attributes.Add("onclick", "setValue('" + e.Row.Cells[3].Text + "','" + e.Row.Cells[11].Text + "','" + hdnIsInvestigation.Value + "');");
            }

        }
    }

    public string Billable(string strbil)
    {
        if (strbil.ToUpper() == "TRUE")
        {
            return "Yes";
        }
        else if (strbil.ToUpper() == "FALSE")
        {
            return "";
        }
        return "";

    }

    protected void gvvitalsCancel_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("VitalEdit"))
            {
                int VitalDetailsId = common.myInt(e.CommandArgument);


                if (VitalDetailsId > 0)
                {
                    if (!common.myBool(ViewState["IsAllowEdit"]))
                    {
                        Alert.ShowAjaxMsg("Not authorized to edit vital details!", this.Page);
                        return;
                    }

                    hdnVitalDetailsId.Value = VitalDetailsId.ToString();
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    HiddenField hdnFVitalDate = (HiddenField)row.FindControl("hdnFVitalDate");

                    hdnVitalDate.Value = hdnFVitalDate.Value;

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    return;
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
        }
    }

}
