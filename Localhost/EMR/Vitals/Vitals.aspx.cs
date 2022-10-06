using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Vitals_Vitals : System.Web.UI.Page
{
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();

    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    //clsExceptionLog objException = new clsExceptionLog();
    //DAL.DAL dl = new DAL.DAL();
    DataSet ds;
    DL_Funs fun = new DL_Funs();
    //BaseC.ParseData bcText = new BaseC.ParseData();
    //BaseC.EMRVitals objv;

    static DataTable VitalDt = new DataTable();
    string ServiceURL;

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
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        //  Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMasterWithTopDetails.master";
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(Session["OPIP"]) == "O")
            {
                if (common.myStr(Session["TokenNo"]).Length == 0)
                {
                    if (DateTime.Now.Hour > 22 && DateTime.Now.Hour < 07)
                    {

                    }
                    else
                    {
                        // Response.Redirect("~/QMS");
                    }
                }
            }
            ViewState["Tabindex"] = 2;
            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                string pid = Session["CurrentNode"].ToString();
                int len = pid.Length;
                ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, len - 1);
            }
            else
            {
                ViewState["PageId"] = "14";
            }

            //String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
            //DAL.DAL dl = new DAL.DAL();
            if (common.myStr(Request.QueryString["From"]) == "POPUP" || common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate")
            {
                // btnHistory.Visible = true;
                btnClose.Visible = true;
            }
            //Session["encounterid"] = "1347";
            //Session["RegistrationId"] = "81485";

            //if (Convert.ToString(Session["encounterid"]) == "")
            //    Response.Redirect("/default.aspx?RegNo=0", false);

            if (!IsPostBack)
            {
                if (common.myStr(Session["OPIP"]).Equals("E"))
                {
                    //DAL.DAL dlx = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    //string strtriageID = common.myInt(dlx.ExecuteScalar(CommandType.Text, "select ID from ERtoken with(nolock) where ErEncounterID=" + Session["EncounterID"])).ToString();
                    string strtriageID = string.Empty;
                    string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRVitals/ERtoken";
                    APIRootClass.vital vital1 = new global::APIRootClass.vital();
                    vital1.Encounterid = common.myInt(Session["encounterid"]);
                    WebClient client1 = new WebClient();
                    client1.Headers["Content-type"] = "application/json";
                    client1.Encoding = Encoding.UTF8;
                    string inputJson1 = (new JavaScriptSerializer()).Serialize(vital1);
                    string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
                    sValue1 = JsonConvert.DeserializeObject<string>(sValue1);
                    strtriageID = JsonConvert.DeserializeObject<string>(sValue1);

                    if (common.myInt(strtriageID) != 0)
                    {
                        lnktriageform.CommandName = common.myInt(strtriageID).ToString();
                        lnktriageform.Visible = true;
                    }
                }
                //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //DataSet dxs = dl.FillDataSet(CommandType.Text, "exec uspGEtMEWsScore @encounterID=" + common.myInt(Session["encounterid"]));

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GEtMEWsScore";
                APIRootClass.vital vital = new global::APIRootClass.vital();
                vital.Encounterid = common.myInt(Session["encounterid"]);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(vital);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                DataSet dxs = JsonConvert.DeserializeObject<DataSet>(sValue);

                if (dxs.Tables[0].Rows.Count > 0)
                {
                    txtLastMEWSscore.Text = common.myInt(dxs.Tables[0].Rows[0][0]).ToString();
                }
                RadDateTimePicker1.Enabled = true;
                RadComboBox1.Enabled = true;

                onPageLoaded(0);

                System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

                collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                         "EMRTriageModuleId", sConString);

                if (collHospitalSetupValues.ContainsKey("EMRTriageModuleId"))
                    ViewState["EMRTriageModuleId"] = collHospitalSetupValues["EMRTriageModuleId"];

                dvTriage.Visible = false; ;
                if (common.myInt(Session["ModuleId"]).Equals(common.myInt(ViewState["EMRTriageModuleId"])))
                {
                    dvTriage.Visible = true;
                    BindTriageddl();
                }
            }
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;

        }
    }


    private void onPageLoaded(int VitalDetailsId)
    {
        try
        {
            dvMandatory.Visible = false;
            for (int i = 0; i < gvvitals.Rows.Count; i++)
            {
                TextBox txtvitalvalue = new TextBox();
                txtvitalvalue = (TextBox)gvvitals.Rows[i].FindControl("txtfindings");
                // txtvitalvalue.Attributes.Add("onblur", "nSat=1;");
            }

            if (Session["RegistrationId"] != null)
            {
                bindAutoCalculate(); // complete
            }

            //DataTable dt = new DataTable();
            //dt.Columns.Add("Hours");
            //Hashtable hsinput = new Hashtable();
            //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            RadDateTimePicker1.SelectedDate = DateTime.Now;//new
            DateTime dtEnc = DateTime.Now;//new
            hdnIsUnSavedData.Value = "0";

            RadComboBox1.Items.Clear();
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
            if (common.myStr(Session["IsMedicalAlert"]) == "")
            {
                lnkAlerts.Enabled = false;
                lnkAlerts.CssClass = "blinkNone";
                lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
            }
            else if (common.myStr(Session["IsMedicalAlert"]).ToUpper() == "YES")
            {
                lnkAlerts.Enabled = true;
                lnkAlerts.Font.Bold = true;
                lnkAlerts.CssClass = "blink";
                lnkAlerts.Font.Size = 11;
                lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
            }

            BindHospitalVitalTemplate(); // complete
            currentVitalValue(); // complete

            //BindPatientHiddenDetails();// Not required
            BindVitalValue(); // Completed

            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
            {
                btnsave.Visible = false;
                // btncopydata.Visible = false;
            }

            if (((common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false
                    && !common.myInt(Session["ModuleId"]).Equals(44)) ||
                        (common.myStr(Session["BillingstatusId"]).ToUpper().Equals("8") && common.myInt(Session["ModuleId"]).Equals(44)))
                    )
            {
                btnsave.Visible = false;
                //btnNew.Visible = false;
            }
            else if (common.myBool(Session["isEMRSuperUser"]) == true
                && common.myStr(ViewState["SingleScreenTemplateCode"]) != "OTH"
                && common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
            {
                btnsave.Visible = true;
                btnsave.Enabled = true;
            }
            else if (common.myBool(Session["isEMRSuperUser"]) == true
                && common.myStr(ViewState["SingleScreenTemplateCode"]) == "OTH"
                && common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
            {
                btnsave.Visible = true;
                btnsave.Enabled = true;
            }
            else if (!(common.myStr(Session["BillingstatusId"]).ToUpper().Equals("8") && common.myInt(Session["ModuleId"]).Equals(44)))
            {
                btnsave.Visible = true;
                btnsave.Enabled = true;
            }

            SetPermission(); //Not Required
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations(sConString); // asktosuryasir
        try
        {
            btnsave.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));

            if (common.myInt(hdnVitalDetailsId.Value) > 0)
            {
                btnsave.Enabled = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ua1.Dispose();
        }
    }

    //void BindPatientHiddenDetails()
    //{
    //    try
    //    {
    //        if (Session["PatientDetailString"] != null)
    //        {
    //            lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
    //        }
    //    }
    //    catch (Exception Ex)
    //    {

    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        clsExceptionLog objException = new clsExceptionLog();
    //        objException.HandleException(Ex);
    //        objException = null;

    //    }
    //}
    protected void BindHospitalVitalTemplate()
    {
        //objv = new BaseC.EMRVitals(sConString);
        ds = new DataSet();
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GetHospitalVitalTemplate";

            APIRootClass.vital objRoot = new global::APIRootClass.vital();
            objRoot.Hospitalid = common.myInt(Session["HospitalLocationID"]);
            objRoot.UserId = common.myInt(Session["UserId"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            // ds = objv.GetHospitalVitalTemplate(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

            if (ds.Tables.Count > 0)
            {

                ddlTemplate.DataSource = ds;
                ddlTemplate.DataValueField = "Id";
                ddlTemplate.DataTextField = "TemplateName";
                ddlTemplate.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //added by sikandar 08/07/2014
        finally
        {
            //objv = null;
        }

    }
    protected void btnchart_Click(object sender, EventArgs e)
    {
        try
        {
            Button btnchart = sender as Button;
            GridViewRow row = (GridViewRow)btnchart.NamingContainer;

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/BindChart";
            APIRootClass.chart vital = new global::APIRootClass.chart();
            vital.vitalId = common.myInt(gvAutoCalculation.Rows[row.RowIndex].Cells[4].Text.Trim());
            vital.Category = common.myStr(gvAutoCalculation.Rows[row.RowIndex].Cells[0].Text.Trim());
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(vital);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //objv = new BaseC.EMRVitals(sConString);


            //DataSet ds = objv.BindChart(common.myInt(gvAutoCalculation.Rows[row.RowIndex].Cells[4].Text.Trim()), common.myStr(gvAutoCalculation.Rows[row.RowIndex].Cells[0].Text.Trim()));


            // yogesh 7-05-2022
            if (vital.Category.Equals("BSA") && ds.Tables[0].Rows.Count == 0)
            {
                ds.Tables[0].Columns.Add("Range", typeof(string));
                ds.Tables[0].Columns.Add("'+Category+'Category", typeof(string));

            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                ds.Tables[0].AcceptChanges();
            }
            gvchart.DataSource = ds;
            gvchart.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //added by sikandar 08/07/2014
        finally
        {
            //objv = null;
        }
    }
    //yogesh New
    protected void btntemprature_Click(object sender, EventArgs e)
    { //yogesh  Start 21/07/2022
        try
        {
            TextBox txtvalue1 = new TextBox();
            TextBox txtvalue2 = new TextBox();
            TextBox txtUnitId1 = new TextBox();
            TextBox txtUnitId2 = new TextBox();
            Label txtminvalue = new Label();
            Label txtmaxvalue = new Label();

            String strVitalId = "";
            for (int i = 0; i < gvvitals.Rows.Count; i++)
            {
                strVitalId = gvvitals.Rows[i].Cells[7].Text.ToString();
                if (gvvitals.Rows[i].Cells[9].Text.Trim() == "T")
                {
                    txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                    txtminvalue = (Label)gvvitals.Rows[i].FindControl("txtminvalue");
                    txtmaxvalue = (Label)gvvitals.Rows[i].FindControl("txtmaxvalue");

                    if (common.myStr(txtvalue1.Text) != "")
                    {

                        if (common.myInt(txtminvalue.Text) > common.myInt(txtvalue1.Text) ||
                            common.myInt(txtmaxvalue.Text) < common.myInt(txtvalue1.Text))
                        {
                            txtvalue1.ForeColor = System.Drawing.Color.Red;

                        }
                        else
                        {
                            txtvalue1.ForeColor = System.Drawing.Color.Black;

                        }
                    }
                }
                else if (gvvitals.Rows[i].Cells[9].Text.Trim() == "R")
                {

                    txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                    txtminvalue = (Label)gvvitals.Rows[i].FindControl("txtminvalue");
                    txtmaxvalue = (Label)gvvitals.Rows[i].FindControl("txtmaxvalue");


                    if (common.myStr(txtvalue1.Text) != "")
                    {

                        if (common.myInt(txtminvalue.Text) > common.myInt(txtvalue1.Text) ||
                            common.myInt(txtmaxvalue.Text) < common.myInt(txtvalue1.Text))
                        {

                            txtvalue1.ForeColor = System.Drawing.Color.Red;


                        }
                        else
                        {

                            txtvalue1.ForeColor = System.Drawing.Color.Black;

                        }

                    }
                }
                else if (gvvitals.Rows[i].Cells[9].Text.Trim() == "P")
                {

                    txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                    txtminvalue = (Label)gvvitals.Rows[i].FindControl("txtminvalue");
                    txtmaxvalue = (Label)gvvitals.Rows[i].FindControl("txtmaxvalue");


                    if (common.myStr(txtvalue1.Text) != "")
                    {

                        if (common.myInt(txtminvalue.Text) > common.myInt(txtvalue1.Text) ||
                            common.myInt(txtmaxvalue.Text) < common.myInt(txtvalue1.Text))
                        {

                            txtvalue1.ForeColor = System.Drawing.Color.Red;

                        }
                        else
                        {

                            txtvalue1.ForeColor = System.Drawing.Color.Black;
                        }

                    }
                }
                //else if (gvvitals.Rows[i].Cells[9].Text.Trim() == "BPS")
                //{

                //        txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                //        if (common.myStr(txtvalue1.Text) != "")
                //        {

                //            if (common.myInt(txtminvalue.Text) > common.myInt(txtvalue1.Text) ||
                //                common.myInt(txtmaxvalue.Text) < common.myInt(txtvalue1.Text))
                //            {

                //                txtvalue1.ForeColor = System.Drawing.Color.Red;

                //            }
                //            else
                //            {

                //                txtvalue1.ForeColor = System.Drawing.Color.Black;


                //            }
                //        }
                //    }
                //    else if (gvvitals.Rows[i].Cells[9].Text.Trim() == "BPD")
                //    {
                //        txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                //        if (common.myStr(txtvalue1.Text) != "")
                //        {

                //            if (common.myInt(txtminvalue.Text) > common.myInt(txtvalue1.Text) ||
                //                common.myInt(txtmaxvalue.Text) < common.myInt(txtvalue1.Text))
                //            {

                //                txtvalue1.ForeColor = System.Drawing.Color.Red;

                //            }
                //            else
                //            {

                //                txtvalue1.ForeColor = System.Drawing.Color.Black;
                //            }
                //        }
                //    }


            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //yogesh  End 21/07/2022
    }
    protected void btnRespiration_Click(object sender, EventArgs e)
    {
        try
        {
            //objv = new BaseC.EMRVitals(sConString); // asktosuryasir
            //DataSet ds1 = new DataSet();

            Hashtable ht = new Hashtable();
            TextBox txtvalue1 = new TextBox();
            TextBox txtvalue2 = new TextBox();
            TextBox txtUnitId1 = new TextBox();
            TextBox txtUnitId2 = new TextBox();
            Label txtminvalue = new Label();
            Label txtmaxvalue = new Label();

            HtmlInputHidden hd = new HtmlInputHidden();

            String strVitalId = "";
            for (int i = 0; i < gvvitals.Rows.Count; i++)
            {
                strVitalId = gvvitals.Rows[i].Cells[7].Text.ToString();
                if (gvvitals.Rows[i].Cells[9].Text.Trim() == "R")
                {

                    txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                    txtminvalue = (Label)gvvitals.Rows[i].FindControl("txtminvalue");
                    txtmaxvalue = (Label)gvvitals.Rows[i].FindControl("txtmaxvalue");


                    if (common.myStr(txtvalue1.Text) != "")
                    {

                        if (common.myInt(txtminvalue.Text) > common.myInt(txtvalue1.Text) ||
                            common.myInt(txtmaxvalue.Text) < common.myInt(txtvalue1.Text))
                        {

                            txtvalue1.ForeColor = System.Drawing.Color.Red;

                        }
                        else
                        {

                            txtvalue1.ForeColor = System.Drawing.Color.Black;
                        }

                    }
                }

            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //added by sikandar 08/07/2014
        finally
        {
            //objv = null;
        }

    }
    protected void btnpulse_Click(object sender, EventArgs e)
    {
        try
        {
            //objv = new BaseC.EMRVitals(sConString); // asktosuryasir
            //DataSet ds1 = new DataSet();

            Hashtable ht = new Hashtable();
            TextBox txtvalue1 = new TextBox();
            TextBox txtvalue2 = new TextBox();
            TextBox txtUnitId1 = new TextBox();
            TextBox txtUnitId2 = new TextBox();
            HtmlInputHidden hd = new HtmlInputHidden();
            Label txtminvalue = new Label();
            Label txtmaxvalue = new Label();



            String strVitalId = "";
            for (int i = 0; i < gvvitals.Rows.Count; i++)
            {
                strVitalId = gvvitals.Rows[i].Cells[7].Text.ToString();
                if (gvvitals.Rows[i].Cells[9].Text.Trim() == "P")
                {

                    txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                    txtminvalue = (Label)gvvitals.Rows[i].FindControl("txtminvalue");
                    txtmaxvalue = (Label)gvvitals.Rows[i].FindControl("txtmaxvalue");


                    if (common.myStr(txtvalue1.Text) != "")
                    {

                        if (common.myInt(txtminvalue.Text) > common.myInt(txtvalue1.Text) ||
                            common.myInt(txtmaxvalue.Text) < common.myInt(txtvalue1.Text))
                        {

                            txtvalue1.ForeColor = System.Drawing.Color.Red;

                        }
                        else
                        {

                            txtvalue1.ForeColor = System.Drawing.Color.Black;
                        }
                    }


                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //added by sikandar 08/07/2014
        finally
        {
            //objv = null;
        }

    }
    protected void btnBPSBPD_Click(object sender, EventArgs e)
    {
        try
        {
            //objv = new BaseC.EMRVitals(sConString); // asktosuryasir
            //DataSet ds1 = new DataSet();

            Hashtable ht = new Hashtable();
            TextBox txtvalue1 = new TextBox();
            TextBox txtvalue2 = new TextBox();
            TextBox txtUnitId1 = new TextBox();
            TextBox txtUnitId2 = new TextBox();


            Label txtminvalue = new Label();
            Label txtmaxvalue = new Label();


            HtmlInputHidden hd = new HtmlInputHidden();
            String strVitalId = "";
            for (int i = 0; i < gvvitals.Rows.Count; i++)
            {
                strVitalId = gvvitals.Rows[i].Cells[7].Text.ToString();
                txtminvalue = (Label)gvvitals.Rows[i].FindControl("txtminvalue");
                txtmaxvalue = (Label)gvvitals.Rows[i].FindControl("txtmaxvalue");


                if (gvvitals.Rows[i].Cells[9].Text.Trim() == "BPS" || gvvitals.Rows[i].Cells[9].Text.Trim() == "BPD")
                {

                    txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                    if (common.myStr(txtvalue1.Text) != "")
                    {

                        if (common.myInt(txtminvalue.Text) > common.myInt(txtvalue1.Text) ||
                            common.myInt(txtmaxvalue.Text) < common.myInt(txtvalue1.Text))
                        {

                            txtvalue1.ForeColor = System.Drawing.Color.Red;

                        }
                        else
                        {

                            txtvalue1.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                }

            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //added by sikandar 08/07/2014
        finally
        {
            //objv = null;
        }

    }
    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            //objv = new BaseC.EMRVitals(sConString); 

            Hashtable ht = new Hashtable();
            TextBox txtvalue1 = new TextBox();
            TextBox txtvalue2 = new TextBox();
            TextBox txtUnitId1 = new TextBox();
            TextBox txtUnitId2 = new TextBox();

            HtmlInputHidden hd = new HtmlInputHidden();
            String strVitalId = "";
            String xmlstring = "";
            for (int i = 0; i < gvvitals.Rows.Count; i++)
            {
                strVitalId = gvvitals.Rows[i].Cells[7].Text.ToString();
                if (gvvitals.Rows[i].Cells[9].Text.Trim() == "HT")
                {
                    txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                    txtvalue2 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings2");
                    txtUnitId1 = (TextBox)gvvitals.Rows[i].FindControl("txtUnitId1");
                    txtUnitId2 = (TextBox)gvvitals.Rows[i].FindControl("txtUnitId2");
                    if (Convert.ToString(txtvalue1.Text) == "" || txtvalue1.Text == null)
                    {
                        txtvalue1.Text = "0";
                    }
                    if (Convert.ToString(txtvalue2.Text) == "" || txtvalue2.Text == null)
                    {
                        txtvalue2.Text = "0";
                    }

                    xmlstring = xmlstring + "<Table1><c1>" + strVitalId + "</c1><c2>" + txtUnitId1.Text + "</c2>";
                    xmlstring = xmlstring + "<c3>" + txtvalue1.Text + "</c3><c4>" + txtUnitId2.Text + "</c4>";
                    xmlstring = xmlstring + "<c5>" + txtvalue2.Text + "</c5></Table1>";

                }

                else if (gvvitals.Rows[i].Cells[9].Text.Trim() == "WT")
                {
                    txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                    txtvalue2 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings2");
                    txtUnitId1 = (TextBox)gvvitals.Rows[i].FindControl("txtUnitId1");
                    txtUnitId2 = (TextBox)gvvitals.Rows[i].FindControl("txtUnitId2");
                    if (Convert.ToString(txtvalue1.Text) == "" || txtvalue1.Text == null)
                    {
                        txtvalue1.Text = string.Empty;

                    }
                    if (Convert.ToString(txtvalue2.Text) == "" || txtvalue2.Text == null)
                    {
                        txtvalue2.Text = string.Empty;

                    }
                    string v1 = string.Empty;
                    string v2 = string.Empty;
                    if (txtvalue2.Text.Equals(string.Empty))
                    {
                        v2 = "0";
                    }
                    else
                    {
                        v2 = txtvalue2.Text;
                    }
                    xmlstring = xmlstring + "<Table1><c1>" + common.myInt(strVitalId) + "</c1><c2>" + common.myInt(txtUnitId1.Text) + "</c2>";
                    xmlstring = xmlstring + "<c3>" + common.myDbl(txtvalue1.Text) + "</c3><c4>" + common.myInt(v2) + "</c4>";
                    xmlstring = xmlstring + "<c5>" + common.myDbl(txtvalue2.Text) + "</c5></Table1>";
                }
                #region omprakash sharma -- Configure vital , MAP = Diastolic BP + 1/3(Systolic BP – Diastolic BP) 

                if (gvvitals.Rows[i].Cells[9].Text.Trim() == "BPS")
                {
                    txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                    txtvalue2 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings2");
                    txtUnitId1 = (TextBox)gvvitals.Rows[i].FindControl("txtUnitId1");
                    txtUnitId2 = (TextBox)gvvitals.Rows[i].FindControl("txtUnitId2");
                    if (Convert.ToString(txtvalue1.Text) == "" || txtvalue1.Text == null)
                    {
                        txtvalue1.Text = "0";
                    }
                    if (Convert.ToString(txtvalue2.Text) == "" || txtvalue2.Text == null)
                    {
                        txtvalue2.Text = "0";
                    }

                    xmlstring = xmlstring + "<Table1><c1>" + strVitalId + "</c1><c2>" + txtUnitId1.Text + "</c2>";
                    xmlstring = xmlstring + "<c3>" + txtvalue1.Text + "</c3><c4>" + txtUnitId2.Text + "</c4>";
                    xmlstring = xmlstring + "<c5>" + txtvalue2.Text + "</c5></Table1>";

                }

                else if (gvvitals.Rows[i].Cells[9].Text.Trim() == "BPD")
                {
                    txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                    txtvalue2 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings2");
                    txtUnitId1 = (TextBox)gvvitals.Rows[i].FindControl("txtUnitId1");
                    txtUnitId2 = (TextBox)gvvitals.Rows[i].FindControl("txtUnitId2");
                    if (Convert.ToString(txtvalue1.Text) == "" || txtvalue1.Text == null)
                    {
                        txtvalue1.Text = string.Empty;

                    }
                    if (Convert.ToString(txtvalue2.Text) == "" || txtvalue2.Text == null)
                    {
                        txtvalue2.Text = string.Empty;

                    }
                    string v1 = string.Empty;
                    string v2 = string.Empty;
                    if (txtvalue2.Text.Equals(string.Empty))
                    {
                        v2 = "0";
                    }
                    else
                    {
                        v2 = txtvalue2.Text;
                    }
                    xmlstring = xmlstring + "<Table1><c1>" + common.myInt(strVitalId) + "</c1><c2>" + common.myInt(txtUnitId1.Text) + "</c2>";
                    xmlstring = xmlstring + "<c3>" + common.myDbl(txtvalue1.Text) + "</c3><c4>" + common.myInt(v2) + "</c4>";
                    xmlstring = xmlstring + "<c5>" + common.myDbl(txtvalue2.Text) + "</c5></Table1>";
                }
                #endregion
            }
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/CalculateVitalsValue";
            APIRootClass.VitalsValue vital = new global::APIRootClass.VitalsValue();
            vital.HospitalLocationID = common.myInt(Session["HospitalLocationID"]);
            vital.xmlstr = xmlstring.ToString();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(vital);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //  ds = objv.CalculateVitalsValue(common.myInt(Session["HospitalLocationID"]), xmlstring.ToString());

            if (ds.Tables.Count > 0)
            {
                ViewState["BMICatValue"] = ds.Tables[0].Rows[0]["Category"].ToString().Trim();
                gvAutoCalculation.DataSource = ds.Tables[0];
                gvAutoCalculation.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //added by sikandar 08/07/2014
        finally
        {
            ds.Dispose();
        }
    }

    protected void lnkAlerts_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";

            RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&PId=" + common.myStr(Session["RegistrationId"]);
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
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }

    }

    void bindAutoCalculate()
    {
        try
        {
            //objv = new BaseC.EMRVitals(sConString);

            //DataSet ds = objv.bindAutoCalculate(common.myInt(Session["HospitalLocationID"]));
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/bindAutoCalculate";

            APIRootClass.vital objRoot = new global::APIRootClass.vital();
            objRoot.Hospitalid = common.myInt(Session["HospitalLocationID"]);

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAutoCalculation.DataSource = ds.Tables[0];
                gvAutoCalculation.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //added by sikandar 08/07/2014
        finally
        {
            //objv = null;
        }
    }

    private DataTable creatTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("UnitId");
        dt.Columns.Add("VitalSignName");
        dt.Columns.Add("vitalid");
        dt.Columns.Add("DisplayName");
        // dt.Columns.Add("UnitId");
        return dt;
    }

    private void BindVitalValue()
    {
        try
        {
            DataSet ds = new DataSet();
            foreach (GridViewRow r in gvvitals.Rows)
            {
                string strVitalID = r.Cells[7].Text;

                if (r.Cells[11].Text == "DropDown")
                {
                    RadComboBox ddlFaceScanDrop = (RadComboBox)r.FindControl("ddlFaceScanDrop");
                    GridViewRow rowFace = ddlFaceScanDrop.NamingContainer as GridViewRow;
                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GetVitalImage";

                    APIRootClass.VitalModel objRoot = new global::APIRootClass.VitalModel();
                    objRoot.HospitalId = common.myInt(Session["HospitalLocationID"]);
                    objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                    objRoot.VitalId = common.myInt(strVitalID);

                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;

                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                    //BaseC.EMRVitals vital = new BaseC.EMRVitals(sConString);
                    //ds = vital.GetVitalImage(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]), Convert.ToInt32(strVitalID));

                    foreach (DataRow dsTitle in ds.Tables[0].Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = (string)dsTitle["ImageName"];
                        item.Value = dsTitle["Id"].ToString();
                        if (dsTitle["ImagePath"] != null)
                        {
                            item.ImageUrl = dsTitle["ImagePath"].ToString();
                        }
                        ddlFaceScanDrop.Items.Add(item);
                        item.DataBind();
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //added by sikandar 08/07/2014
        finally
        {
            //objv = null;
        }
    }

    private void currentVitalValue()
    {
        try
        {
            DataView dv = new DataView();
            //  dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // BaseC.Security AuditCA = new BaseC.Security(sConString);
            // objv = new BaseC.EMRVitals(sConString);
            DataTable dt = new DataTable();
            //ds = (DataSet)objv.GetVitalTemplateDetails(common.myInt(ddlTemplate.SelectedValue), common.myInt(Session["FacilityId"]),
            //                            common.myInt(Session["RegistrationId"]), common.myInt(hdnVitalDetailsId.Value));


            string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GetVitalTemplateDetails";

            APIRootClass.VitalTemplate objRoot = new global::APIRootClass.VitalTemplate();
            objRoot.iTemplateId = 1;// common.myInt(ddlTemplate.SelectedValue);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.Registrationid = common.myInt(Session["RegistrationId"]);
            objRoot.VitalDetailsId = common.myInt(hdnVitalDetailsId.Value);

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);



            //if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate")
            //{
            //    dv = new DataView(ds.Tables[0]);
            //    dv.RowFilter = "TemplateData=1";
            //}
            //else
            //{
            //    dv = new DataView(ds.Tables[0]);
            //    dv.RowFilter = "TemplateData=0";
            //}
            //if (dv.ToTable().Rows.Count > 0)
            //{
            //    dt = dv.ToTable();
            //}
            //else
            //{
            dt = ds.Tables[0];
            //}

            if (dt.Rows.Count > 0)
            {
                VitalDt = dt;
                ViewState["HeaderName"] = VitalDt.Rows[0]["HeaderName"].ToString().Trim();
                if (common.myStr(ViewState["HeaderName"]) == "")
                {
                    ViewState["HeaderName"] = "Last Value ";// +"(" + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ")";
                }
                gvvitals.DataSource = VitalDt;
                gvvitals.DataBind();


                ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/EMRVitalSignTypes";
                APIRootClass.vital vitals = new global::APIRootClass.vital();
                vitals.Hospitalid = common.myInt(Session["HospitalLocationID"]);
                vitals.date = "";
                vitals.UserId = common.myInt(Session["UserId"]);
                vitals.Encounterid = common.myInt(Session["Encounterid"]);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(vitals);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                DataSet dsVital = JsonConvert.DeserializeObject<DataSet>(sValue);

                //string strVitalID = "select VitalSignName + ISNULL(' (' + DisplayName + ')','') AS Vital from EMRVitalSignTypes where VitalType='D'";
                //DataSet dsVital = new DataSet();
                //dsVital = dl.FillDataSet(CommandType.Text, strVitalID);

                for (int i = 0; i < gvvitals.Rows.Count; i++)
                {

                    if (dsVital.Tables[0].Rows.Count > 0)
                    {
                        ViewState["EMRVital"] = dsVital.Tables[0].Rows[0]["Vital"].ToString();
                        RadComboBox ddlFaceScanDrop = (RadComboBox)gvvitals.Rows[i].FindControl("ddlFaceScanDrop");
                        RadDatePicker dtpdate = (RadDatePicker)gvvitals.Rows[i].FindControl("dtpdate");
                        TextBox txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                        DropDownList ddlPainScanDrop = (DropDownList)gvvitals.Rows[i].FindControl("ddlPainScanDrop");
                        if (gvvitals.Rows[i].Cells[11].Text.Trim() == "DropDown")
                        {
                            if (gvvitals.Rows[i].Cells[1].Text.Trim() == ViewState["EMRVital"].ToString().Trim())
                            {
                                txtvalue1.Visible = false;
                                ddlFaceScanDrop.Visible = true;
                                ddlPainScanDrop.Visible = false;
                            }
                            else
                            {
                                txtvalue1.Visible = false;
                                ddlPainScanDrop.Visible = true;
                                ddlFaceScanDrop.Visible = false;
                            }
                            txtvalue1.Visible = false;
                            ddlFaceScanDrop.Visible = true;
                            ddlPainScanDrop.Visible = false;
                        }
                        else if (common.myStr(gvvitals.Rows[i].Cells[11].Text.Trim()).ToUpper().Equals("DATE"))
                        {
                            dtpdate.Visible = true;
                            txtvalue1.Visible = false;
                            ddlPainScanDrop.Visible = false;
                        }
                    }
                }
                //if (!IsPostBack)
                //{
                //    // asktosuryasir
                //    AuditCA.AuditCommonAccess(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), common.myInt(Session["FacilityID"]),
                //                            common.myInt(Session["RegistrationId"]), common.myInt(Session["encounterid"]),
                //                            common.myInt(ViewState["PageId"]), 0, common.myInt(Session["UserID"]), 0,
                //                            "ACCESSED", Convert.ToString(Session["IPAddress"]));

                //}
            }
            if (gvvitals.Rows.Count == 0)
            {
                lblMessage.Text = "No Vitals Has Been Defined, Click The Link To Create New";
                trCurrentVitalTemplate.Visible = true;
            }
            else
            {
                trCurrentVitalTemplate.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            //objv = null;
            //dl = null;
        }
    }

    protected void gvvitalsview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;

        }
        //else if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //HiddenField hdnIsInvestigation = (HiddenField)e.Row.FindControl("hdnIsInvestigation");
        //}
    }

    protected void gvAutoCalculation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnchart = (Button)e.Row.FindControl("btnchart");
                if (common.myStr(ViewState["BMICatValue"]) != "Normal Weight")
                {
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                }
                if (e.Row.Cells[0].Text == "MAP")
                {
                    btnchart.Visible = false;
                }
            }

            e.Row.Cells[4].Visible = false;
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;

        }
    }

    protected void gvvitals_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[11].Visible = false;
            e.Row.Cells[12].Visible = false;

            if (e.Row.Cells[12].Text == "True")
            {
                e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnIsMandatory = (HiddenField)e.Row.FindControl("hdnIsMandatory");

                if (common.myBool(hdnIsMandatory.Value))
                {
                    e.Row.Cells[1].Text = common.myStr(e.Row.Cells[1].Text) + hdnMandatoryStar.Value;
                }
                TextBox txtfind1 = (TextBox)e.Row.FindControl("txtfindings1");
                short tabindex = Convert.ToInt16(ViewState["Tabindex"].ToString());
                txtfind1.TabIndex = tabindex;
                ViewState["Tabindex"] = ++tabindex;

                TextBox txtfind2 = (TextBox)e.Row.FindControl("txtfindings2");
                txtfind2.TabIndex = tabindex;
                ViewState["Tabindex"] = ++tabindex;

                HiddenField hdnAbNormalmin = (HiddenField)e.Row.FindControl("hdnAbNormalmin");
                HiddenField hdnAbNormalmax = (HiddenField)e.Row.FindControl("hdnAbNormalmax");
                HiddenField hdnDisplayName = (HiddenField)e.Row.FindControl("hdnDisplayName");
                HtmlInputHidden hdtxtUnitId1 = (HtmlInputHidden)e.Row.FindControl("hdtxtUnitId1");
                HiddenField hdnlValues = (HiddenField)e.Row.FindControl("hdnlValues");
                HiddenField hdnIsInvestigation = (HiddenField)e.Row.FindControl("hdnIsInvestigation");
                HiddenField hdnTemplateData = (HiddenField)e.Row.FindControl("hdnTemplateData");
                RadDatePicker dtpdate = (RadDatePicker)e.Row.FindControl("dtpdate");
                HiddenField hdnDisplayInGraph = (HiddenField)e.Row.FindControl("hdnDisplayInGraph");

                if (common.myStr(hdnDisplayName.Value).Equals("LMP") && !common.myStr(hdnlValues.Value).Equals(string.Empty))
                {
                    dtpdate.SelectedDate = common.myDate(common.myDate(hdnlValues.Value).ToString("yyyy/MM/dd"));
                }
                //if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate")
                //{
                if ((Convert.ToBoolean(hdnTemplateData.Value) == true)
                    && (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() != "StaticTemplate"))
                {
                    e.Row.Cells[5].Text = "";
                }
                else if ((Convert.ToBoolean(hdnTemplateData.Value) == false)
                    && (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate"))
                {
                    e.Row.Cells[5].Text = "";
                }
                else if (common.myBool(hdnTemplateData.Value)
                    && (Request.QueryString["POPUP"] == null))
                {
                    //e.Row.Cells[5].Text = "";
                }

                else if (common.myBool(hdnTemplateData.Value)
                    && (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate"))
                {
                    e.Row.Cells[5].Text = e.Row.Cells[5].Text;
                }


                if ((e.Row.Cells[9].Text.Trim() == "HT") || (e.Row.Cells[9].Text.Trim() == "WT"))
                {
                    txtfind1.Attributes.Add("onchange", "javascript:CalculateBMI('" + txtfind1.ClientID + "');");
                    txtfind2.Attributes.Add("onchange", "javascript:CalculateBMI('" + txtfind2.ClientID + "');");

                }
                if ((e.Row.Cells[9].Text.Trim() == "BPS") || (e.Row.Cells[9].Text.Trim() == "BPD"))
                {
                    txtfind1.Attributes.Add("onchange", "javascript:CalculateMAP('" + txtfind1.ClientID + "');");
                    txtfind2.Attributes.Add("onchange", "javascript:CalculateMAP('" + txtfind2.ClientID + "');");

                }
                else if (e.Row.Cells[9].Text.Trim() == "T"
                        || e.Row.Cells[9].Text.Trim() == "R"
                        || e.Row.Cells[9].Text.Trim() == "P"
                        || e.Row.Cells[9].Text.Trim() == "BPS"
                        || e.Row.Cells[9].Text.Trim() == "BPD"
                        || e.Row.Cells[9].Text.Trim() == "MAC"
                        || e.Row.Cells[9].Text.Trim() == "Pain"
                        || e.Row.Cells[9].Text.Trim() == "SPO2"
                        || e.Row.Cells[9].Text.Trim() == "FBS"
                        || e.Row.Cells[9].Text.Trim() == "RBS"
                        || e.Row.Cells[9].Text.Trim() == "SG"
                        || e.Row.Cells[9].Text.Trim() == "PH"
                        || e.Row.Cells[9].Text.Trim() == "LEU"
                        || e.Row.Cells[9].Text.Trim() == "NIT"
                        || e.Row.Cells[9].Text.Trim() == "PRO"
                        || e.Row.Cells[9].Text.Trim() == "GLU"
                        || e.Row.Cells[9].Text.Trim() == "KET"
                        || e.Row.Cells[9].Text.Trim() == "UBG"
                        || e.Row.Cells[9].Text.Trim() == "BIL"
                        || e.Row.Cells[9].Text.Trim() == "ERY"
                        || e.Row.Cells[9].Text.Trim() == "HB"
                   )
                {
                    //onkeypress
                    txtfind1.Attributes.Add("onkeyup", "javascript:CalculatTemprature('" + txtfind1.ClientID + "','" + hdnAbNormalmin.ClientID + "','" + hdnAbNormalmax.ClientID + "','" + hdnDisplayName.ClientID + "');");
                    txtfind2.Attributes.Add("onkeyup", "javascript:CalculatTemprature('" + txtfind2.ClientID + "','" + hdnAbNormalmin.ClientID + "','" + hdnAbNormalmax.ClientID + "','" + hdnDisplayName.ClientID + "');");

                }
                else
                {
                    txtfind1.Attributes.Add("onblur", "nSat=1;");
                }
                txtfind1.Attributes.Add("autocomplete", "off");
                txtfind2.Attributes.Add("autocomplete", "off");


                Label txtunit1 = new Label();
                txtunit1 = (Label)e.Row.FindControl("txtunit1");
                TextBox txtUnitId1 = new TextBox();
                txtUnitId1 = (TextBox)e.Row.FindControl("txtUnitId1");
                if (Convert.ToString(txtUnitId1.Text.Trim()) == "")
                {
                    txtunit1.Visible = false;
                }
                TextBox txtunit2 = new TextBox();
                txtunit2 = (TextBox)e.Row.FindControl("txtunit2");

                TextBox txtUnitId2 = new TextBox();
                txtUnitId2 = (TextBox)e.Row.FindControl("txtUnitId2");

                TextBox txtfindings2 = new TextBox();
                txtfindings2 = (TextBox)e.Row.FindControl("txtfindings2");

                if (Convert.ToString(txtUnitId2.Text.Trim()) == "")
                {
                    txtunit2.Visible = false;
                    txtfindings2.Visible = false;
                }

                //  string HeaderColumn = common.myStr(this.gvvitals.HeaderRow.Cells[5].Text);
                if (common.myStr(e.Row.Cells[5].Text) != "")
                {
                    e.Row.Cells[5].Attributes.Add("onclick", "setValue('" + e.Row.Cells[5].Text + "','" + e.Row.Cells[9].Text + "','" + hdnIsInvestigation.Value + "','" + hdnDisplayInGraph.Value + "');");
                }

                if (common.myInt(hdnIsInvestigation.Value) == 1)
                {
                    e.Row.BackColor = System.Drawing.Color.LightCyan;
                }

            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Text = common.myStr(ViewState["HeaderName"]);

            }
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;

        }

    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        //HidDateTime.Value = Calendar1.SelectedDate.ToString("dd/MM/yyyy");
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        clsIVF bcText = new clsIVF(string.Empty);
        int TriageType = 0;
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //objv = new BaseC.EMRVitals(sConString);
            string strsave = "";
            if (RadDateTimePicker1.SelectedDate == null)
            {
                //Alert.ShowAjaxMsg("Please fill vitals date !", Page);
                return;
            }
            //if (common.myStr(Session["OPIP"]) == "O" && common.myInt(Session["NoToken"]) == 0)
            //{
            //    if (common.myStr(Session["TokenNo"]).Length == 0)
            //    {
            //        if (DateTime.Now.Hour > 22 && DateTime.Now.Hour < 07)
            //        {

            //        }
            //        else
            //        {
            //            lblMandatoryMessage.Text = "Vital Takign is only possible for Queued Patients";
            //            Alert.ShowAjaxMsg("Vital Takign is only possible for Queued Patients", this.Page);
            //            return;
            //        }
            //    }
            //}
            //BaseC.Patient bc = new BaseC.Patient(sConString);
            // sqlstr = "set dateformat mdy Select EVST.vitalID, EVST.VitalSignName from EMRPatientVitalSignDetails EVSD inner join  EMRVitalSignTypes EVST on EVST.VitalId=EVSD.VitalId  Where VitalEntryDate='" + RadDateTimePicker1.SelectedDate + "'"; //txtDateTime.Text + "'";

            //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // DataSet ds = (DataSet)dl.FillDataSet(CommandType.Text, sqlstr);

            if (common.myStr(Session["pStatus"]).ToUpper().Contains("CLOSE"))
            {
                Alert.ShowAjaxMsg("Patient status closed !", Page);
                return;
            }


            string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GetVitalSignName";

            APIRootClass.vital objRoot = new global::APIRootClass.vital();
            objRoot.date = common.myStr(RadDateTimePicker1.SelectedDate);
            objRoot.Hospitalid = common.myInt(Session["HospitalLocationID"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            // DataSet ds = (DataSet)objv.GetVitalSignName(common.myStr(RadDateTimePicker1.SelectedDate), common.myInt(Session["HospitalLocationId"]));

            DataRow[] r;

            String VitalName = "";
            String xmlstring = "";

            bool bTemplateData = Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate" ? true : false;
            StringBuilder sbMandatory = new StringBuilder();
            int Flag = 0;
            for (int i = 0; i < gvvitals.Rows.Count; i++)
            {
                RadComboBox ddlFaceScanDrop = new RadComboBox();
                DropDownList ddlPainScanDrop = new DropDownList();
                TextBox txtvalue1 = new TextBox();
                RadDatePicker dtpdate = new RadDatePicker();
                HiddenField hdnminvalue = new HiddenField();
                HiddenField hdnmaxvalue = new HiddenField();
                HiddenField hdnIsMandatory = new HiddenField();

                hdnminvalue = (HiddenField)gvvitals.Rows[i].FindControl("hdnAbNormalmin");
                hdnmaxvalue = (HiddenField)gvvitals.Rows[i].FindControl("hdnAbNormalmax");

                ddlFaceScanDrop = (RadComboBox)gvvitals.Rows[i].FindControl("ddlFaceScanDrop");
                ddlPainScanDrop = (DropDownList)gvvitals.Rows[i].FindControl("ddlPainScanDrop");

                txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                dtpdate = (RadDatePicker)gvvitals.Rows[i].FindControl("dtpdate");
                hdnIsMandatory = (HiddenField)gvvitals.Rows[i].FindControl("hdnIsMandatory");
                TextBox txtvalue2 = new TextBox();
                txtvalue2 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings2");

                HtmlInputHidden hdUnitId1 = new HtmlInputHidden();
                hdUnitId1 = (HtmlInputHidden)gvvitals.Rows[i].FindControl("hdtxtUnitId1");

                HtmlInputHidden hdUnitId2 = new HtmlInputHidden();
                hdUnitId2 = (HtmlInputHidden)gvvitals.Rows[i].FindControl("hdtxtUnitId2");
                if (common.myBool(hdnIsMandatory.Value))
                {
                    if (gvvitals.Rows[i].Cells[11].Text.Trim() == "DropDown")
                    {
                        if (gvvitals.Rows[i].Cells[1].Text.Trim() == ViewState["EMRVital"].ToString().Trim())
                        {
                            ddlFaceScanDrop = (RadComboBox)gvvitals.Rows[i].FindControl("ddlFaceScanDrop");

                            if (common.myInt(ddlFaceScanDrop.SelectedValue) == 0)
                            {
                                if (sbMandatory.ToString() != string.Empty)
                                {
                                    sbMandatory.Append(", ");
                                }
                                sbMandatory.Append(gvvitals.Rows[i].Cells[1].Text);
                            }
                        }
                        else
                        {
                            ddlPainScanDrop = (DropDownList)gvvitals.Rows[i].FindControl("ddlPainScanDrop");
                            if (common.myInt(ddlPainScanDrop.SelectedValue) == 0)
                            {
                                if (sbMandatory.ToString() != string.Empty)
                                {
                                    sbMandatory.Append(", ");
                                }
                                sbMandatory.Append(gvvitals.Rows[i].Cells[1].Text);
                            }
                        }
                    }
                    else if (common.myStr(gvvitals.Rows[i].Cells[11].Text.Trim().ToUpper()).Equals("DATE"))
                    {
                        dtpdate = (RadDatePicker)gvvitals.Rows[i].FindControl("dtpdate");
                        if (common.myLen(dtpdate.SelectedDate.ToString()).Equals(0))
                        {
                            if (sbMandatory.ToString() != string.Empty)
                            {
                                sbMandatory.Append(", ");
                            }
                            sbMandatory.Append(gvvitals.Rows[i].Cells[1].Text);
                        }
                    }
                    else
                    {
                        txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                        if (common.myLen(txtvalue1.Text) == 0)
                        {
                            if (sbMandatory.ToString() != string.Empty)
                            {
                                sbMandatory.Append(", ");
                            }
                            sbMandatory.Append(gvvitals.Rows[i].Cells[1].Text);
                        }
                    }
                }



                if (txtvalue1.Text.Length > 0 || common.myInt(ddlFaceScanDrop.SelectedValue) != 0 || common.myInt(ddlPainScanDrop.SelectedValue) != 0 || common.myLen(dtpdate.SelectedDate) != 0)
                {
                    Flag = 1;
                    r = ds.Tables[0].Select(gvvitals.Rows[i].Cells[7].Text);

                    if (r.Count() == 0)
                    {

                        if (hdUnitId1.Value == "")
                        {
                            hdUnitId1.Value = "0";
                        }
                        if (hdUnitId2.Value == "")
                        {
                            hdUnitId2.Value = "0";
                        }
                        if (txtvalue2.Text == "")
                        {
                            txtvalue2.Text = "0";
                        }

                        xmlstring = xmlstring + "<Table1><c1>" + gvvitals.Rows[i].Cells[7].Text + "</c1>";
                        xmlstring = xmlstring + "<c2>" + hdUnitId1.Value + "</c2>";
                        if (gvvitals.Rows[i].Cells[11].Text.Trim() == "DropDown")
                        {
                            //if (gvvitals.Rows[i].Cells[1].Text.Trim() == ViewState["EMRVital"].ToString().Trim())
                            //{
                            ddlFaceScanDrop = (RadComboBox)gvvitals.Rows[i].FindControl("ddlFaceScanDrop");
                            xmlstring = xmlstring + "<c3>" + bcText.ParseQ(ddlFaceScanDrop.SelectedItem.Text) + "</c3>";

                            //}
                            //else
                            //{
                            //    ddlPainScanDrop = (DropDownList)gvvitals.Rows[i].FindControl("ddlPainScanDrop");
                            //    xmlstring = xmlstring + "<c3>" + bcText.ParseQ(ddlPainScanDrop.SelectedItem.Text) + "</c3>";

                            //}
                        }
                        else if (common.myStr(gvvitals.Rows[i].Cells[11].Text.Trim().ToUpper()).Equals("DATE"))
                        {
                            dtpdate = (RadDatePicker)gvvitals.Rows[i].FindControl("dtpdate");
                            xmlstring = xmlstring + "<c3>" + bcText.ParseQ(common.myStr(common.myDate(dtpdate.SelectedDate).ToString("dd/MM/yyyy"))) + "</c3>";
                        }
                        else
                        {
                            txtvalue1 = (TextBox)gvvitals.Rows[i].FindControl("txtfindings1");
                            xmlstring = xmlstring + "<c3>" + bcText.ParseQ(txtvalue1.Text) + "</c3>";
                        }


                        xmlstring = xmlstring + "<c4>" + bcText.ParseQ(hdUnitId2.Value) + "</c4>";
                        xmlstring = xmlstring + "<c5>" + bcText.ParseQ(txtvalue2.Text) + "</c5>";
                        CheckBox chkBillable = new CheckBox();//30th March 10 
                        chkBillable = (CheckBox)gvvitals.Rows[i].FindControl("chkBillable");
                        xmlstring = xmlstring + "<c6>" + chkBillable.Checked + "</c6>";

                        TextBox txtRemarks = new TextBox();
                        txtRemarks = (TextBox)gvvitals.Rows[i].FindControl("txtremarks");
                        xmlstring = xmlstring + "<c7>" + bcText.ParseQ(txtRemarks.Text) + "</c7>";

                        if (gvvitals.Rows[i].Cells[11].Text.Trim() == "DropDown")
                        {
                            if (gvvitals.Rows[i].Cells[1].Text.Trim() == ViewState["EMRVital"].ToString().Trim())
                            {
                                xmlstring = xmlstring + "<c8>" + bcText.ParseQ(ddlFaceScanDrop.SelectedValue) + "</c8>";
                            }
                            else
                            {
                                xmlstring = xmlstring + "<c8>" + bcText.ParseQ(ddlPainScanDrop.SelectedValue) + "</c8>";
                            }
                        }
                        else if (common.myStr(gvvitals.Rows[i].Cells[11].Text.Trim().ToUpper()).Equals("DATE"))
                        {
                            xmlstring = xmlstring + "<c8>" + null + "</c8>";
                        }
                        else
                        {
                            xmlstring = xmlstring + "<c8>" + null + "</c8>";
                        }
                        if (common.myInt(hdnminvalue.Value) != 0 && common.myInt(hdnmaxvalue.Value) != 0)
                        {
                            if (common.myInt(hdnminvalue.Value) > common.myInt(txtvalue1.Text) ||
                            common.myInt(hdnmaxvalue.Value) < common.myInt(txtvalue1.Text))
                            {
                                xmlstring = xmlstring + "<c9>" + true + "</c9>";
                            }
                            else
                            {
                                xmlstring = xmlstring + "<c9>" + false + "</c9>";
                            }
                        }
                        else
                        {
                            xmlstring = xmlstring + "<c9>" + false + "</c9>";
                        }
                        xmlstring = xmlstring + "<c10>" + bTemplateData + "</c10></Table1>";
                    }


                    else
                    {
                        VitalName = VitalName + r[0].ItemArray[1].ToString() + ",";
                    }
                }
            }
            if (sbMandatory.ToString() != string.Empty)
            {
                lblMessage.Text = "Please fill mandatory vital values: " + sbMandatory.ToString().Replace(hdnMandatoryStar.Value, "");
                Alert.ShowAjaxMsg(lblMessage.Text, this);
                return;
            }
            for (int i = 0; i < gvAutoCalculation.Rows.Count; i++)
            {
                if (HttpUtility.HtmlDecode(gvAutoCalculation.Rows[i].Cells[1].Text.Trim()).Trim() != "" && HttpUtility.HtmlDecode(gvAutoCalculation.Rows[i].Cells[1].Text.Trim()) != "&nbsp;")
                {
                    if (common.myStr(gvAutoCalculation.Rows[i].Cells[1].Text).Trim() != "" && gvAutoCalculation.Rows[i].Cells[1].Text != "&nbsp;")
                    {
                        if (isNumeric(gvAutoCalculation.Rows[i].Cells[1].Text, System.Globalization.NumberStyles.AllowDecimalPoint))
                        {
                            if (Convert.ToDecimal(gvAutoCalculation.Rows[i].Cells[1].Text) > 0)
                            {
                                r = ds.Tables[0].Select(gvAutoCalculation.Rows[i].Cells[4].Text);
                                if (r.Count() == 0)
                                {
                                    xmlstring = xmlstring + "<Table1><c1>" + gvAutoCalculation.Rows[i].Cells[4].Text + "</c1>";
                                    xmlstring = xmlstring + "<c2>0</c2><c3>" + gvAutoCalculation.Rows[i].Cells[1].Text + "</c3>";
                                    xmlstring = xmlstring + "<c4>0</c4><c5>0</c5>";
                                    xmlstring = xmlstring + "<c6></c6><c7></c7><c8></c8><c9>false</c9><c10>" + bTemplateData + "</c10></Table1>";
                                }
                                else
                                {
                                    VitalName = VitalName + r[0].ItemArray[1].ToString() + ",";
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Enter some value to save";
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Enter some value to save";
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Enter some value to save";
                    }
                }
            }

            ////MEWS

            if (Flag == 1)
            {
                if (xmlstring != "")
                {
                    hdnVitalDetailsId.Value = "1";

                    if (common.myInt(Session["ModuleId"]).Equals(common.myInt(ViewState["EMRTriageModuleId"])))
                    {
                        TriageType = common.myInt(ddlTriage.SelectedValue);
                    }


                    ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/SavePatientVitalSign";

                    APIRootClass.SaveVitals objSave = new global::APIRootClass.SaveVitals();
                    objSave.EntryDate = common.myDate(RadDateTimePicker1.SelectedDate).ToString("dd/MM/yyyy HH:mm");
                    objSave.RegId = common.myInt(Session["RegistrationId"]);
                    objSave.EncId = common.myInt(Session["encounterid"]);
                    objSave.HospId = common.myInt(Session["HospitalLocationID"]);
                    objSave.FacilityId = common.myInt(Session["FacilityID"]);
                    objSave.PageId = common.myInt(ViewState["PageId"]);
                    objSave.Encodedby = common.myInt(Session["UserId"]);
                    objSave.TriageTypeId = TriageType;
                    objSave.xmlstring = xmlstring.ToString();
                    objSave.TemplateFieldId = (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate") ? Convert.ToInt32(Request.QueryString["TemplateFieldId"].ToString()) : 0;


                    client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;

                    inputJson = (new JavaScriptSerializer()).Serialize(objSave);
                    sValue = client.UploadString(ServiceURL, inputJson);
                    strsave = JsonConvert.DeserializeObject<string>(sValue);
                    // ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                    //strsave = objv.SavePatientVitalSign(common.myDate(RadDateTimePicker1.SelectedDate).ToString("dd/MM/yyyy HH:mm"), common.myInt(Session["RegistrationId"]),
                    //                common.myInt(Session["encounterid"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(ViewState["PageId"]),
                    //                common.myInt(Session["UserId"]), xmlstring.ToString(),
                    //                Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate" ? Convert.ToInt32(Request.QueryString["TemplateFieldId"].ToString()) : 0,common.myInt(hdnVitalDetailsId.Value));

                    if (common.myStr(Session["TokenNo"]).Length > 0)
                    {

                        ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/CloseQMSCall";
                        APIRootClass.CloseQMSCall closeQMSCall = new global::APIRootClass.CloseQMSCall();
                        closeQMSCall.TokenNo = common.myInt(Session["encounterid"]);
                        closeQMSCall.TokenNo = common.myInt(Session["encounterid"]);
                        closeQMSCall.TokenNo = common.myInt(Session["encounterid"]);
                        client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                        sValue = client.UploadString(ServiceURL, inputJson);
                        sValue = JsonConvert.DeserializeObject<string>(sValue);
                        //  DataSet dxs = JsonConvert.DeserializeObject<DataSet>(sValue);


                        //DAL.DAL xdl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        // xdl.ExecuteNonQuery(CommandType.Text, "exec UspCloseQMSCall @TokenNo='" + common.myStr(Session["TokenNo"]) + "',@UserID=" + common.myStr(Session["UserID"]) + ",@ActivityDesc='Patient Vital Taken'");
                        Session["TokenNo"] = null;
                        //TokenNo = common.myStr(Session["TokenNo"]);
                    }

                    ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GEtMEWsScore";
                    APIRootClass.vital vital = new global::APIRootClass.vital();
                    vital.Encounterid = common.myInt(Session["encounterid"]);
                    client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    DataSet dxs = JsonConvert.DeserializeObject<DataSet>(sValue);
                    //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    //DataSet dxs = dl.FillDataSet(CommandType.Text, "exec uspGEtMEWsScore @encounterID=" + common.myInt(Session["encounterid"]));
                    if (dxs.Tables[0].Rows.Count > 0)
                    {
                        txtLastMEWSscore.Text = common.myInt(dxs.Tables[0].Rows[0][0]).ToString();
                    }
                    ///Tagging Static Template with Template Field   
                    ///// Comment this part after confimation Mr.Manmohan 
                    //if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate")
                    //{
                    //    BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
                    //    Hashtable hshOut = objEMR.TaggingStaticTemplateWithTemplateField(common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                    //    Convert.ToInt32(Request.QueryString["SectionId"].ToString()), Convert.ToInt32(Request.QueryString["TemplateFieldId"].ToString()),
                    //    Convert.ToInt32(Request.QueryString["StaticTemplateId"].ToString()), common.myInt(Session["UserId"]));
                    //}
                    ///end

                    hdnVitalDetailsId.Value = "0";
                    hdnVitalDate.Value = "";

                    RadDateTimePicker1.Enabled = true;
                    RadComboBox1.Enabled = true;


                    RadDateTimePicker1.SelectedDate = DateTime.Now;//new
                    DateTime dtEnc = DateTime.Now;//new

                    int iMinute = DateTime.Now.Minute;
                    RadComboBoxItem rcbItem = (RadComboBoxItem)RadComboBox1.Items.FindItemByText(iMinute.ToString());
                    if (rcbItem != null)
                    {
                        rcbItem.Selected = true;
                    }


                    hdnIsUnSavedData.Value = "0";
                    btnCalculate_Click(sender, e);
                    currentVitalValue();

                    if (strsave.Contains("Data Saved") || strsave == "")
                    {
                        if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
                        }
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblMessage.Text = "Data Saved.";
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = strsave.ToString();
                    }

                    if (common.myStr(Request.QueryString["From"]) == "POPUP")
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                        return;
                    }

                }

            }
            else
            {
                lblMessage.Text = "Enter Some Value!";
            }

            ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);

            BindVitalValue();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //added by sikandar 08/07/2014
        finally
        {
            //objv = null;

        }
    }

    public bool isNumeric(object val, System.Globalization.NumberStyles NumberStyle)
    {
        if (val == null)
        {
            return false;
        }
        string vv = Convert.ToString(val);

        Double result;
        return Double.TryParse(vv, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
    }

    protected void dlHours_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            HtmlInputButton btnHours = new HtmlInputButton();
            btnHours = (HtmlInputButton)e.Item.FindControl("btnHours");
            btnHours.Attributes.Add("onfocus", "javascript:colourchangeHour('" + btnHours.ClientID + "')");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void dlMin_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            HtmlInputButton btnMin = new HtmlInputButton();
            btnMin = (HtmlInputButton)e.Item.FindControl("btnMin");
            btnMin.Attributes.Add("onfocus", "javascript:colourchangeMin('" + btnMin.ClientID + "')");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void rblView_SelectedIndexChanged(object sender, EventArgs e)
    {
        // bindPreviousVitals();
    }

    //protected void ddlFaceScanDrop_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        for (int i = 0; i < gvvitals.Rows.Count; i++)
    //        {
    //            if (gvvitals.Rows[i].Cells[11].Text.Trim() == "DropDown")
    //            {
    //                RadComboBox ddlFaceScanDrop = (RadComboBox)gvvitals.Rows[i].FindControl("ddlFaceScanDrop");
    //                string strValueID = ddlFaceScanDrop.SelectedValue;
    //                string strImages = "select ImageName,ImagePath from EMRVitalValues where ValueID=" + strValueID + "";
    //                DataSet ds = dl.FillDataSet(CommandType.Text, strImages);
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    RadWindow1.NavigateUrl = "VitalDialogBox.aspx?ValueID=" + strValueID;
    //                    RadWindow1.Height = 500;
    //                    RadWindow1.Width = 600;
    //                    RadWindow1.Top = 40;
    //                    RadWindow1.Left = 100;

    //                    //RadWindow1.OnClientClose = "OnClientClose";
    //                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //                    RadWindow1.Modal = true;
    //                    RadWindow1.VisibleStatusbar = false;

    //                }
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    //added by sikandar 08/07/2014
    //    finally
    //    {
    //        dl = null;
    //        objv = null;
    //    }
    //}

    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        currentVitalValue();
    }

    //private void getEmpID()
    //{
    //    BaseC.EMRMasters.EMRColorLegends EMRColorLegends = new BaseC.EMRMasters.EMRColorLegends(sConString);
    //    try
    //    {

    //        SqlDataReader dr = EMRColorLegends.getEmployeeId(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(Session["UserID"]));
    //        if (dr.HasRows == true)
    //        {
    //            dr.Read();
    //            String strSelID = dr[0].ToString();
    //            if (strSelID != "")
    //            {
    //                char[] chArray = { ',' };
    //                string[] serviceIdXml = strSelID.Split(chArray);
    //                ViewState["DoctorID"] = serviceIdXml[0].ToString();

    //            }
    //        }
    //        dr.Close();
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    //added by sikandar 08/07/2014
    //    finally
    //    {
    //        objv = null;
    //        EMRColorLegends = null;
    //    }
    //}

    protected void lnkNewTemplate_OnClick(Object sendere, EventArgs e)
    {
        Response.Redirect("/EMR/Vitals/VitalSignTemplate.aspx?Mpg=P82", false);
    }

    protected void chkBillable_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gvr in gvvitals.Rows)
            {
                CheckBox chkBillable = (CheckBox)gvr.FindControl("chkBillable");
                TextBox txtfindings = (TextBox)gvr.FindControl("txtfindings");
                if (chkBillable.Checked == true)
                {
                    if (txtfindings == null)
                    {
                        // Alert.ShowAjaxMsg("Value is not selected, Enter value for billable", Page );
                        return;
                    }
                    else
                    {
                        if (Convert.ToString(txtfindings.Text).Trim() == "")
                        {
                            return;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //try catch and finally added by sikandar for code optimization
        StringBuilder sb = new StringBuilder();

        try
        {
            sb.Append(RadDateTimePicker1.SelectedDate.Value.ToString());
            sb.Remove(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
            sb.Insert(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
            RadDateTimePicker1.SelectedDate = Convert.ToDateTime(sb.ToString());
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            sb = null;
        }
    }


    protected void btnvitaldetal_Click(object sender, EventArgs e)
    {
        try
        {

            RadWindow1.NavigateUrl = "~/EMR/Vitals/PatientVitaldetails.aspx?From=POPUP";
            RadWindow1.Height = 520;
            RadWindow1.Width = 750;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.OnClientClose = "OnClientVitalDetailsClose";
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }


    protected void btnfind_Click(object sender, EventArgs e)
    {
        try
        {
            onPageLoaded(common.myInt(hdnVitalDetailsId.Value));

            if (common.myInt(hdnVitalDetailsId.Value) > 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "Copylastvitalvalue();", true);
            }

            if (common.myLen(hdnVitalDate.Value) > 0)
            {
                RadDateTimePicker1.SelectedDate = common.myDate(hdnVitalDate.Value);
                int intMin = common.myDate(hdnVitalDate.Value).Minute;
                RadComboBox1.SelectedIndex = RadComboBox1.Items.IndexOf(RadComboBox1.Items.FindItemByValue(intMin.ToString()));

                RadDateTimePicker1.Enabled = false;
                RadComboBox1.Enabled = false;
            }

            SetPermission();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        //Response.Redirect("~/EMR/Vitals/Vitals.aspx", false);
        //currentVitalValue();
    }
    protected void lnkcalculator_Click(object sender, EventArgs e)
    {
        try
        {

            RadWindow1.NavigateUrl = "~/EMR/Vitals/calculator.aspx";
            RadWindow1.Height = 275;
            RadWindow1.Width = 350;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void btnMandatoryOk_OnClick(object sender, EventArgs e)
    {
        dvMandatory.Visible = false;
    }

    protected void btnMandatoryCancel_OnClick(object sender, EventArgs e)
    {
        dvMandatory.Visible = false;
    }

    protected void btnCopyDDL_OnClick(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < gvvitals.Rows.Count; i++)
            {
                HiddenField hdnDisplayName = (HiddenField)gvvitals.Rows[i].FindControl("hdnDisplayName");
                if (common.myStr(hdnDisplayName.Value).ToUpper().Equals("PAIN"))
                {
                    HiddenField hdnValueID = (HiddenField)gvvitals.Rows[i].FindControl("hdnValueID");
                    HiddenField hdnlValues = (HiddenField)gvvitals.Rows[i].FindControl("hdnlValues");

                    RadComboBox ddlFaceScanDrop = (RadComboBox)gvvitals.Rows[i].FindControl("ddlFaceScanDrop");
                    ddlFaceScanDrop.SelectedIndex = ddlFaceScanDrop.Items.IndexOf(ddlFaceScanDrop.Items.FindItemByValue(common.myStr(hdnValueID.Value)));
                    ddlFaceScanDrop.Text = common.myStr(hdnlValues.Value);
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }


    protected void lnkmews_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "~/EMRreports/PrintReport.aspx?ReportName=MEWS";
        RadWindow1.Height = 600;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = true;
    }

    protected void lnkGRBS_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "~/EMRreports/PrintReport.aspx?ReportName=GRBS";
        RadWindow1.Height = 600;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = true;
    }

    protected void lnktriageform_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=TrigData&ToeknNo=" + common.myInt(lnktriageform.CommandName).ToString();
        RadWindow1.Height = 600;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = true;



    }

    protected void btnFetch_Click(object sender, EventArgs e)
    {
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet ds = dl.FillDataSet(CommandType.Text, "exec uspfetchAutoVital @EncounterID=" + Session["EncounterID"].ToString());

            string strSql = "exec uspfetchAutoVital @EncounterID=" + Session["EncounterID"].ToString();
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= ds.Tables[0].Rows.Count; i++)
                {
                    foreach (GridViewRow gvr in gvvitals.Rows)
                    {
                        HiddenField hdn = gvr.FindControl("hdnvitalid") as HiddenField;
                        TextBox tx = gvr.FindControl("txtfindings1") as TextBox;
                        if (common.myInt(hdn.Value) == common.myInt(ds.Tables[0].Rows[i]["VitalId"]))
                        {
                            tx.Text = ds.Tables[0].Rows[i]["Result"].ToString();
                            break;
                        }
                    }
                }
                lblMessage.Text = "Data Fetched , Taken On " + common.myDate(ds.Tables[0].Rows[0]["ReceivedDatetime"]);
            }
            else
            {
                lblMessage.Text = "No Auto Vital Data Found !";
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnHistory_Click(object sender, EventArgs e)
    {
        //if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate")
        //{

        RadWindow1.NavigateUrl = "~/EMR/Vitals/PreviousVitals.aspx?POPUP=StaticTemplate";
        RadWindow1.Height = 520;
        RadWindow1.Width = 750;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code
        RadWindow1.Title = "Vital History";
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;

        //}
        //else
        //{
        //    Response.Redirect("~/EMR/Vitals/PreviousVitals.aspx?Mpg=P14", false);
        //}
    }

    private void BindTriageddl()
    {
        DataSet ds = new DataSet();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            ddlTriage.Items.Clear();
            string strsql = "Select StatusId, Status from StatusMaster where statusType  = 'Triage' And Active = 1";  // Binding Dropdown list 
            strsql = strsql + " select isnull(TriageTypeId,0) as TriageTypeId from Encounter where id=" + common.myInt(Session["EncounterId"]) + " And Active = 1"; // Fetch Triage Saved value
            ds = objDl.FillDataSet(CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlTriage.DataSource = ds.Tables[0];
                ddlTriage.DataValueField = "StatusId";
                ddlTriage.DataTextField = "Status";
                ddlTriage.DataBind();
                ddlTriage.Items.Insert(0, new RadComboBoxItem("", "0"));
            }

            if (common.myInt(ds.Tables.Count) > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ddlTriage.SelectedValue = common.myStr(ds.Tables[1].Rows[0]["TriageTypeId"]);
                }
                else
                {
                    ddlTriage.SelectedIndex = 0;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            objDl = null;
        }


    }

}

