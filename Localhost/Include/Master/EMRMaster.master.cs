using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web;
using Telerik.Web.UI;
using System.IO;
using System.Drawing;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;
using Newtonsoft.Json;

public partial class Include_Master_EMRMaster : System.Web.UI.MasterPage
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    clsExceptionLog objException = new clsExceptionLog();
	string Localhost = ConfigurationManager.AppSettings["Localhost"];
    protected void Page_Init(object sender, System.EventArgs e)
    {
        try
        {
            /// Work of abhishek
            if (!Page.IsCallback)
            {
                if (Session["StrO"] == null)
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=Session Expired", false);
                    return;
                }
                else if (Session["StrO"] != null && common.myStr(Request.QueryString["irtrf"]) != "" && common.myStr(Session["StrO"]) != common.myStr(Request.QueryString["irtrf"]))
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=Invalid URL", false);
                }
                else if (Session["StrO"] != null)
                {
                    string output = "";
                    BaseC.User usr = new BaseC.User(sConString);
                    usr.RedirectionHandler(common.myInt(Session["UserID"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "VALIDATED", common.myStr(Session["StrO"]), out output);
                    if (output.Contains("Expired") || output.Contains("Invalid"))
                    {
                        Session["UserID"] = null;
                        Response.Redirect("/Login.aspx?Logout=1&Status=" + output, false);
                        return;
                    }
                    usr = null;
                }
                if (Session["UserID"] == null)
                {
                    Response.Redirect("/Login.aspx?Logout=1", false);
                    return;
                }
            }

            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            }
            if (Request.QueryString["EncId"] != null)
            {
                Session["EncounterId"] = Request.QueryString["EncId"];
            }
            if (Request.QueryString["mdlId"] != null)
            {
                Session["ModuleId"] = Request.QueryString["mdlId"];
            }

            if (common.myStr(Request.QueryString["irtrf"]) != ""
                && Request.QueryString["OP"] != null && common.myStr(Request.QueryString["OP"]).Split('_').Length > 10)
            {
                Session["irtrf"] = null;
                Session["IsAdminGroup"] = null;
                Session["LoginIsAdminGroup"] = null;
                Session["HospitalLocationID"] = null;
                Session["FacilityID"] = null;
                Session["GroupID"] = null;
                Session["FinancialYearId"] = null;
                Session["EntrySite"] = null;
                Session["UserID"] = null;
                Session["UserName"] = null;
                Session["ModuleId"] = null;
                Session["URLPId"] = null;
                Session["irtrf"] = Request.QueryString["irtrf"];

                Session["IsAdminGroup"] = common.myStr(Request.QueryString["OP"].Split('_')[0]);
                Session["LoginIsAdminGroup"] = common.myStr(Request.QueryString["OP"].Split('_')[1]);
                Session["HospitalLocationID"] = common.myStr(Request.QueryString["OP"].Split('_')[2]);
                Session["FacilityID"] = common.myStr(Request.QueryString["OP"].Split('_')[3]);
                Session["GroupID"] = common.myStr(Request.QueryString["OP"].Split('_')[4]);
                Session["FinancialYearId"] = common.myStr(Request.QueryString["OP"].Split('_')[5]);
                Session["EntrySite"] = common.myStr(Request.QueryString["OP"].Split('_')[6]);
                Session["UserID"] = common.myStr(Request.QueryString["OP"].Split('_')[7]);
                Session["UserName"] = common.myStr(Request.QueryString["OP"].Split('_')[8]).Replace("%", " ");
                Session["ModuleId"] = common.myStr(Request.QueryString["OP"].Split('_')[9]);
                Session["URLPId"] = common.myStr(Request.QueryString["OP"].Split('_')[10]);
                // abhishek work
                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 11)
                    Session["FacilityName"] = common.myStr(Request.QueryString["OP"]).Split('_')[11].Replace("%", " ");
                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 12)
                    Session["CanDownloadPatientDocument"] = common.myStr(Request.QueryString["OP"]).Split('_')[12];
                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 13)
                    Session["FacilityStateId"] = common.myStr(Request.QueryString["OP"]).Split('_')[13];


                string output = "";
                BaseC.User usr = new BaseC.User(sConString);
                usr.RedirectionHandler(common.myInt(Session["UserID"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "VALIDATED", common.myStr(Session["StrO"]), out output);
                if (output.Contains("Expired") || output.Contains("Invalid"))
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=" + output, false);
                }

                usr = null;

                if (Session["UserID"] == null)
                {
                    Response.Redirect("/Login.aspx?Logout=1", false);
                }
            }

        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "PageInit");
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["InputDateFormat"] = "dd/MM/yyyy";
        Session["OutputDateFormat"] = "dd/MM/yyyy";
        Application["OutputDateFormat"] = "dd/MM/yyyy";

        if (!IsPostBack)
        {
            //// Check Sesstion Time Out
            //Session["Reset"] = true;
            //Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.Config");
            //SessionStateSection section = (SessionStateSection)config.GetSection("system.web/sessionState");
            //int timeout = (int)section.Timeout.TotalMinutes * 1000 * 60;
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);

            //// End Check Sesstion Time Out
            try
            {
                System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

                collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                         "EMRTriageModuleId", sConString);

                if (collHospitalSetupValues.ContainsKey("EMRTriageModuleId"))
                    ViewState["EMRTriageModuleId"] = collHospitalSetupValues["EMRTriageModuleId"];

                BaseC.User valUser = new BaseC.User(sConString);
                DataSet dsEmpType = new DataSet();
                if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsAllowValidationforBiometric", sConString).ToUpper().Contains("Y"))
                {
                    dvBioMenu.Visible = true;
                }
                if (Session["UserID"] == null)
                {
                    Response.Redirect("/Login.aspx?Logout=1&RefURL=" + Request.UrlReferrer, false);
                }
                if (common.myInt(Session["EmployeeId"]).Equals(0))
                {
                    Session["EmployeeId"] = valUser.getEmployeeId(common.myInt(Session["UserID"]));
                }
                if (
 common.myStr(Session["EmployeeTypePermission"]) == "" || common.myStr(Session["EmployeeType"]) == ""
             || common.myStr(Session["isEMRSuperUser"]) == "" || common.myStr(Session["IsLoginDoctor"]).Equals(string.Empty)

             || common.myStr(Session["IsIVFSpecialisation"]).Equals(string.Empty) || common.myInt(Session["UserSpecialisationId"]).Equals(0) || common.myStr(Session["LoginDoctorId"]).Equals(string.Empty)
              || common.myStr(Session["EmployeeName"]).Equals(string.Empty) || common.myStr(Session["LoginEmployeeType"]).Equals(string.Empty) || common.myStr(Session["EmployeeTypeID"]).Equals(string.Empty)
                 || common.myStr(Session["LoginDepartmentId"]).Equals(string.Empty) || common.myStr(Session["EnablePrintCaseSheet"]).Equals(string.Empty) || common.myStr(Session["CanDownloadPatientDocument"]).Equals(string.Empty)
                  || common.myStr(Session["DecimalPlace"]).Equals(string.Empty) || common.myStr(Session["NewIpVisitPageEnabled"]).Equals(string.Empty) || common.myStr(Session["DefaultCaseSheetView"]).Equals(string.Empty)
                 || common.myStr(Session["DisplayEnteredByInCaseSheet"]).Equals(string.Empty) || common.myStr(Session["IsCIMSInterfaceActive"]).Equals(string.Empty) || common.myStr(Session["CIMSDatabasePath"]).Equals(string.Empty)
                 || common.myStr(Session["CIMSDatabasePassword"]).Equals(string.Empty) || common.myStr(Session["IsVIDALInterfaceActive"]).Equals(string.Empty)
        || Session["AllowEditDoctorProgressNote"].Equals(string.Empty)
        || common.myStr(Session["FacilityName"]).Equals(string.Empty)
        )
                {
                    BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
                    dsEmpType = valUser.getEmployeeTypePermission(common.myInt(Session["EmployeeId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]));
                    DataSet dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));

                    if (dsEmpType.Tables[0].Rows.Count > 0)
                    {
                        Session["EmployeeTypePermission"] = common.myBool(dsEmpType.Tables[0].Rows[0]["Admin"]);
                        Session["EmployeeType"] = common.myStr(dsEmpType.Tables[0].Rows[0]["EmployeeType"]).Trim();
                        Session["isEMRSuperUser"] = common.myStr(dsEmpType.Tables[0].Rows[0]["isEMRSuperUser"]).Trim();
                        Session["IsLoginDoctor"] = common.myBool(dsEmpType.Tables[0].Rows[0]["IsDoctor"]);
                        Session["IsIVFSpecialisation"] = false;
                        Session["UserSpecialisationId"] = common.myStr(dsEmpType.Tables[0].Rows[0]["SpecialisationId"]).Trim();
                        Session["LoginDoctorId"] = common.myStr(dsEmpType.Tables[0].Rows[0]["DoctorId"]).Trim();
                        Session["EmployeeName"] = common.myStr(dsEmpType.Tables[0].Rows[0]["EmployeeFirstName"]).Trim();
                        Session["LoginEmployeeType"] = common.myStr(dsEmpType.Tables[0].Rows[0]["EmployeeType"]).Trim();
                        Session["EmployeeTypeID"] = common.myStr(dsEmpType.Tables[0].Rows[0]["EmployeeTypeID"]).Trim();
                        Session["LoginDepartmentId"] = common.myStr(dsEmpType.Tables[0].Rows[0]["DepartmentId"]).Trim();
                        Session["EnablePrintCaseSheet"] = common.myStr(dsEmpType.Tables[0].Rows[0]["UnablePrintCaseSheet"]).Trim();
                        Session["CanDownloadPatientDocument"] = common.myStr(dsEmpType.Tables[0].Rows[0]["CanDownloadPatientDocument"]).Trim();
                        Session["DecimalPlace"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DecimalPlaces", sConString);
                        Session["NewIpVisitPageEnabled"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "NewIpVisitPageEnabled", sConString);

                        Session["DefaultCaseSheetView"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultCaseSheetView", sConString);

                        Session["DisplayEnteredByInCaseSheet"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DisplayEnteredByInCaseSheet", sConString);
                        Session["AllowEditDoctorProgressNote"] = common.myBool(dsEmpType.Tables[0].Rows[0]["AllowEditDoctorProgressNote"]);
                        Session["FacilityName"] = common.myStr(dsEmpType.Tables[0].Rows[0]["FacilityName"]);
                    }

                    if (dsInterface.Tables.Count > 0)
                    {
                        if (dsInterface.Tables[0].Rows.Count > 0)
                        {
                            if (common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]))
                            {
                                Session["IsCIMSInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]);
                                Session["CIMSDatabasePath"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                                Session["CIMSDatabasePassword"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePassword"]);
                            }
                            else if (common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]))
                            {
                                Session["IsVIDALInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]);
                            }
                        }
                    }
                    dsEmpType = null;
                    objEMR = null;
                    dsInterface.Dispose();
                }
                string key = "Biomenu" + common.myStr(Session["RegistrationId"]);
                if (Cache[key] != null)
                {
                    string key1 = "Biomenupat" + common.myStr(common.myStr(Session["RegistrationId"]));
                    imgbiomenu.Enabled = true;
                    imgbiomenu.ToolTip = imgbiomenu.ToolTip + "BioAuth PAtient" + Cache[key1].ToString();
                }
                else
                {
                    imgbiomenu.Enabled = false;
                    imgbiomenu.ToolTip = "This Session is without BioAuth... This option only will available with BioAuth patients.";
                }
                //if (common.myStr(Session["EmployeeType"]) == "D")
                //{
                //    if (common.myInt(Session["ModuleId"]).Equals(3))
                //    {
                //        if (!common.myBool(Session["FindPatientExpanded"]))
                //        {
                //            Radslidingzone2.ExpandedPaneId = "rdpAppList";
                //            Session["FindPatientExpanded"] = true;
                //        }
                //        else
                //        {
                //            Radslidingzone2.ExpandedPaneId = "";
                //        }
                //    }
                //    else
                //    {
                //        Radslidingzone2.ExpandedPaneId = "";
                //    }
                //}

                //if (common.myStr(Session["EmployeeType"]) == "D")
                //{
                //        if (!common.myBool(Session["FindPatientExpanded"]))
                //        {
                //            Radslidingzone2.ExpandedPaneId = "rdpAppList";
                //            Session["FindPatientExpanded"] = true;
                //        }
                //        else
                //        {
                //            Radslidingzone2.ExpandedPaneId = "";
                //        }
                //}


                string facilityTitleName = common.myStr(Session["FacilityName"]);
                if (common.myStr(Session["FacilityName"]).Equals(string.Empty))
                {
                    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);

                    facilityTitleName = emr.BindApplicationName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]));
                    emr = null;
                }

                Page.Header.Title = facilityTitleName;



                if (common.myStr(Session["RegistrationLabelName"]).Equals(""))
                {
                    hdnRegistrationLabelName.Value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "RegistrationLabelName", sConString);
                }


                if ((Session["MenuDetail"] == null) || (Session["GroupDetail"] == null) || (Session["ModuleDetail"] == null) || (Session["mainMenu"] == null) || (Session["MenuData"] == null) || common.myInt(Request.QueryString["irtrf"]).Equals(1))
                {
                    ExtractUserMenuFromSession();
                }
                BindModuleGrid();
                if (Session["ModuleId"] != null && common.myInt(Session["ModuleId"]) > 0)
                {
                    int mid = 0;
                    int ModIndex = 0;

                    for (ModIndex = 0; ModIndex < gvModules.Rows.Count; ModIndex++)
                    {
                        if (gvModules.Rows[ModIndex].Cells[0].Text == common.myStr(Session["ModuleId"]))
                        {
                            mid = ModIndex;
                            break;
                        }
                    }
                    if (mid == 0)
                    {
                        gvModules.SelectedIndex = 0;
                        Session["ModuleId"] = gvModules.Rows[0].Cells[0].Text;
                    }
                    else
                    {
                        gvModules.SelectedIndex = mid;
                    }
                }
                else
                {
                    gvModules.SelectedIndex = 0;
                    Session["ModuleId"] = gvModules.Rows[0].Cells[0].Text;
                }
                if (gvModules.Rows.Count > 0)
                {
                    Session["ModuleName"] = gvModules.SelectedRow.Cells[1].Text;
                    Session["ModuleFlag"] = gvModules.SelectedRow.Cells[3].Text;
                    BindTree();
                    Session["SetQueryString"] = "0";
                    Session["SetQueryStringValue"] = string.Empty;
                    DoSelectCurrentNode(tvCategory);
                }
                //if (Session["RegistrationId"] != null && Session["RegistrationId"].ToString() != "" && common.myStr(Session["EncounterID"]) != null && Session["FormID"] != null)
                //{
                //    BaseC.Patient objPatient = new BaseC.Patient(sConString);
                //    if (common.myStr(Session["RegistrationID"]) != "&nbsp;")
                //    {
                //        SqlDataReader objDr = (SqlDataReader)objPatient.getPatientShortDetail(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"]));
                //        if (objDr.Read())
                //        {
                //            BaseC.clsLISPhlebotomy Objstatus = new BaseC.clsLISPhlebotomy(sConString);
                //            string result = string.Empty;

                //            if (Session["OPIP"].ToString().Equals("O"))
                //            {
                //                result = Objstatus.GetPatientHasCriticalParameterForOP(Convert.ToString(Session["encounterno"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), 0, 0);
                //            }
                //            else if (Session["OPIP"].ToString().Equals("I"))
                //            {
                //                result = Objstatus.GetPatientHasCriticalParameter(Convert.ToString(Session["encounterno"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), 0, 0);
                //            }
                //            if (result.Equals("1"))
                //            {
                //                Radslidingpane4.Visible = true;
                //                //Radslidingpane4.Title = "<b><font size='2' name='Tahoma'>" + objDr["Name"].ToString() + "</font><span style= 'color: #FF0000;font-weight: bold;font-size:20px;valign:top'>*</span></b>";// +objDr["AgeAndFormName"].ToString();
                //                Radslidingpane4.Title = objDr["Name"].ToString();// +objDr["AgeAndFormName"].ToString();
                //                lnkShowCriticalResults.Visible = true;
                //            }
                //            else if (result.Equals("0"))
                //            {
                //                Radslidingpane4.Visible = true;
                //                //Radslidingpane4.Title = "<b><font size='2' name='Tahoma'>" + objDr["Name"].ToString() + "</font></b>";//, " + objDr["AgeAndFormName"].ToString();
                //                Radslidingpane4.Title = objDr["Name"].ToString();//, " + objDr["AgeAndFormName"].ToString();
                //                lnkShowCriticalResults.Visible = false;
                //            }
                //        }
                //        objDr.Close();
                //    }
                //}
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getPatientShortDetail";

                APIRootClass.getPatientShortDetail objRoot = new global::APIRootClass.getPatientShortDetail();

                objRoot.RegistrationId = common.myInt(Session["RegistrationID"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                DataSet objDs = new DataSet();
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
                if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
                {
                    Radslidingpane4.Visible = true;
                    if (common.myLen(objDs.Tables[0].Rows[0]["Name"]) > 30)
                    {
                        Radslidingpane4.Title = common.myStr(objDs.Tables[0].Rows[0]["Name"]).Trim().Substring(0, 30) + " , " + common.myStr(objDs.Tables[0].Rows[0]["Payor"]);
                    }
                    else
                    {
                        Radslidingpane4.Title = common.myStr(objDs.Tables[0].Rows[0]["Name"]).Trim() + " , " + common.myStr(objDs.Tables[0].Rows[0]["Payor"]);
                    }
                }


                ShowMenuItems();



                if (common.myInt(Session["ModuleId"]).Equals(3) || common.myInt(Session["ModuleId"]).Equals(30) || common.myInt(Session["ModuleId"]).Equals(5) || common.myInt(Session["ModuleId"]).Equals(44) || common.myInt(Session["ModuleId"]).Equals(common.myInt(ViewState["EMRTriageModuleId"])))
                {
                    Radslidingzone2.Visible = true;
                    lnkDiagnosticHistory.Visible = true;
                }
                else
                {
                    Radslidingzone2.Visible = false;
                    lnkDiagnosticHistory.Visible = false;
                }

                // Comment to testing the applicaion
                //if (common.myStr(Session["FacilityName"]).ToUpper().Contains("PRACHI"))
                //{
                //    Radslidingzone2.Visible = false;
                //}
            }
            catch (Exception Ex)
            {
                objException.HandleExceptionWithMethod(Ex, "PageLoad");
            }
        }
    }

    protected void BindModuleGrid()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataSet dsModule = new DataSet();
        try
        {
            if (common.myStr(Session["mainMenu"]) != "")
            {
                dt = (DataTable)Session["mainMenu"];
                gvModules.DataSource = dt;
                gvModules.DataBind();

                DataRow[] foundisEMR = dt.Select("ModuleId in (3,44," + common.myInt(ViewState["EMRTriageModuleId"]) + ")");
                if (foundisEMR.Length > 0)
                {
                    Session["isEMR"] = "1";
                }
                else
                {
                    Session["isEMR"] = "0";
                }

                if (Session["ModuleData"] == null)
                {
                    ds.Tables.Add(dt);
                    Session["ModuleData"] = ds;
                }
                else
                {
                    dsModule = (DataSet)Session["ModuleData"];
                    if (dsModule == null || dsModule.Tables.Count == 0)
                    {
                        Session["ModuleData"] = ds;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "BindModuleGrid");
        }
        finally
        {
            ds.Dispose();
            dsModule.Dispose();
            dt.Dispose();
        }
    }

    protected void DoSelectNodeViaOnClickMasterPage()
    {
        try
        {
            foreach (TreeNode node in tvCategory.Nodes)
            {
                node.Expand();
                if (node.ChildNodes.Count > 0)
                {
                    node.ChildNodes[0].Select();
                    node.ChildNodes[0].Text = "<b>" + node.ChildNodes[0].Text + "</b>";
                }
                else
                {
                    node.Select();
                    node.Text = "<b>" + node.Text + "</b>";
                }
                string[] stringSeparators_ShowDia = new string[] { "showDia('" };
                string[] stringSeparators_aspx = new string[] { "') href" };
                string[] result1 = node.Text.Split(stringSeparators_ShowDia, StringSplitOptions.None);
                string[] result2 = { "" };
                if (result1.Length > 1)
                    result2 = result1[1].Split(stringSeparators_aspx, StringSplitOptions.None);
                Session["CurrentNode"] = node.Value;
                if (result1.Length > 1)
                {
                    Response.Redirect(result2[0], false);
                    break;
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "DoSelectNodeViaOnClickMasterPage");
        }
    }
    protected void gvModules_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["StationId"] = 0;
            Session["OldModuleId"] = Session["ModuleId"];
            Session["ModuleId"] = gvModules.SelectedRow.Cells[0].Text;
            Session["ModuleName"] = gvModules.SelectedRow.Cells[1].Text;
            Session["ModuleFlag"] = gvModules.SelectedRow.Cells[3].Text;
            BindTree();
            DoSelectNodeViaOnClickMasterPage();
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "gvModules_SelectedIndexChanged");
        }

    }
    protected void BindTree()
    {
        try
        {
            sModuleName.Text = common.myStr(Session["ModuleName"]);
            BindPages(Convert.ToInt16(gvModules.SelectedRow.Cells[0].Text), tvCategory);
        }
        catch (Exception ex)
        {
            objException.HandleExceptionWithMethod(ex, "BindTree");
        }
    }
    protected void tvCategory_SelectedNodeChanged(Object sender, EventArgs e)
    {

    }
    protected void gvModules_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvModules, "Select$" + e.Row.RowIndex);
            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "gvModules_RowDataBound");
        }
    }
    public void BindPages(Int16 iModuleId, TreeView tvCategory)
    {

        try
        {
            DataTable objDs1 = new DataTable();
            DataSet dsPageCheck = new DataSet();
            String strTickSign = "";
            tvCategory.Nodes.Clear();
            DataView dv = null;

            if (common.myStr(Session["MenuData"]) != "")
            {
                objDs1 = (DataTable)Session["MenuData"];
                if (objDs1.Rows.Count > 0)
                {
                    DataSet objDs = new DataSet();
                    dv = objDs1.DefaultView;
                    dv.RowFilter = "ModuleId =" + iModuleId;
                    DataTable dt = new DataTable();
                    dt = dv.ToTable().Copy();

                    objDs.Tables.Add(dt);

                    for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
                    {
                        string modulelocation = common.myStr(common.myStr(objDs.Tables[0].Rows[i]["ModuleLocation"]));
                        if (common.myStr(Localhost).Length > 0) { modulelocation = Localhost; }
                        
                        
                        AddNodes(tvCategory, common.myInt(objDs.Tables[0].Rows[i]["PageId"]), common.myInt(objDs.Tables[0].Rows[i]["ParentId"]), common.myStr(objDs.Tables[0].Rows[i]["PageName"]), common.myStr(objDs.Tables[0].Rows[i]["PageUrl"]), common.myStr(modulelocation), strTickSign, common.myInt(objDs.Tables[0].Rows[i]["ModuleId"]));
                    }
                    if (tvCategory.Nodes.Count > 0)
                    {
                        tvCategory.ExpandAll();
                        tvCategory.PopulateNodesFromClient = true;
                        tvCategory.ShowLines = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "BindPages");
        }
    }
    public void DoSelectCurrentNode(TreeView tvCategory)
    {
        try
        {
            if (HttpContext.Current.Session["CurrentNode"] != null)
            {
                foreach (TreeNode node in tvCategory.Nodes)
                {
                    if (node.Value == HttpContext.Current.Session["CurrentNode"].ToString())
                    {
                        node.Expand();
                        if (node.ChildNodes.Count > 0)
                        {
                            node.ChildNodes[0].Select();
                            node.ChildNodes[0].Text = "<b>" + node.ChildNodes[0].Text + "</b>";
                        }
                        else
                        {
                            node.Select();
                            node.Text = "<b>" + node.Text + "</b>";
                        }
                        break;
                    }
                    foreach (TreeNode n in node.ChildNodes)
                    {
                        if (HttpContext.Current.Session["CurrentNode"] != null)
                        {
                            if (n.Value == HttpContext.Current.Session["CurrentNode"].ToString())
                            {
                                n.Expand();
                                n.Text = n.Text;
                                n.Select();
                                n.Parent.Expand();
                                n.Text = "<b>" + n.Text + "</b>";
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "DoSelectCurrentNode");
        }
    }
    public void AddNodes(TreeView tvName, Int32 iNodeID, Int32 iParentID, String sNodeText, string sPageUrl, string sModuleLocation, String sShowTickSign, int iModuelId)
    {
        try
        {
            string str = string.Empty;


            if (!common.myInt(Session["SetQueryString"]).Equals(1))
            {

                if (iModuelId.Equals(3))
                {
                    if (!common.myInt(Session["OldModuleId"]).Equals(3))
                    {
                        if (common.myStr(sPageUrl).Contains("PatientDashboardForDoctor.aspx"))
                        {
                            str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);
                                Get(common.myStr(iModuelId), common.myStr(iNodeID));
                            Session["OldModuleId"] = "3";
                            Session["SetQueryString"] = "1";
                            Session["SetQueryStringValue"] = str;
                        }
                        else
                        {
                            str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);
                                Get(common.myStr(iModuelId), common.myStr(iNodeID));
                        }
                    }
                    else
                    {
                        if (common.myStr(sPageUrl).Contains("PatientDashboardForDoctor.aspx"))
                        {
                            str = "&EMRSingleScreenPageUrl=1&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);
                                Get(common.myStr(iModuelId), common.myStr(iNodeID));
                        }
                        else
                        {
                            str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);
                                Get(common.myStr(iModuelId), common.myStr(iNodeID));
                        }
                    }
                }
                else
                {
                    str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);
                        Get(common.myStr(iModuelId), common.myStr(iNodeID));
                }
            }
            else
            {
                str = common.myStr(Session["SetQueryStringValue"]);
            }
            if (iParentID == 0)
            {
                TreeNode masternode;
                if (sPageUrl != "")
                {
                    if (!sPageUrl.Contains("?"))
                    {
                        masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", "", "");
                        masternode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + "?Mpg=" + "P" + iNodeID.ToString() + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                    }
                    else
                    {
                        masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "P" + iNodeID.ToString(), "");
                        masternode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + "&Mpg=" + "P" + iNodeID.ToString() + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                    }
                }
                else
                {
                    if (!sPageUrl.Contains("?"))
                        masternode = new TreeNode(sNodeText.ToString() + sShowTickSign, "P" + iNodeID.ToString(), "", "", "");
                    else
                        masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", "", "");
                }
                tvName.Nodes.Add(masternode);
            }
            else
            {
                TreeNode masternode = new TreeNode();
                masternode = tvName.FindNode("P" + iParentID.ToString());
                if (masternode != null)
                {
                    TreeNode childNode;
                    if (!sPageUrl.Contains("?"))
                    {
                        childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", "", "");
                        childNode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + "?Mpg=" + "C" + iNodeID.ToString() + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                    }
                    else
                    {
                        childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "C" + iNodeID.ToString(), "");
                        childNode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + "&Mpg=" + "C" + iNodeID.ToString() + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                    }
                    masternode.ChildNodes.Add(childNode);
                }
                else
                {
                    CallRecursive(tvName, iNodeID, "C" + iParentID, sNodeText, sPageUrl);
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "DoSelectCurrentNode");
        }
    }
    private void CallRecursive(TreeView tvName, Int32 iNodeID, String sParentID, String sNodeText, string sPageUrl)
    {
        try
        {
            TreeNodeCollection nodes = tvName.Nodes;
            foreach (TreeNode n in nodes)
            {
                ReCallRecursive(n, iNodeID, sParentID, sNodeText, sPageUrl);
            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "CallRecursive");
        }
    }
    private void ReCallRecursive(TreeNode treeNode, Int32 iNodeID, String sParentID, String sNodeText, string sPageUrl)
    {
        try
        {
            foreach (TreeNode tn in treeNode.ChildNodes)
            {
                if (tn.Value == sParentID.ToString())
                {
                    TreeNode childNode;
                    if (!sPageUrl.Contains("?"))
                        childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl + "?Mpg=" + "C" + iNodeID.ToString(), "");
                    else
                        childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "C" + iNodeID.ToString(), "");
                    tn.ChildNodes.Add(childNode);
                }
                ReCallRecursive(tn, iNodeID, sParentID, sNodeText, sPageUrl);
            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "ReCallRecursive");
        }
    }
    public void ShowMenuItems()
    {
        try
        {
            if (common.myStr(Session["MenuData"]) != "" && Session["MenuData"] != null)
            {
                DataTable dt = (DataTable)Session["MenuData"];
                if (dt != null)
                {
                    // string str = "&irtrf=1&OP=" + common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                    string str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                       Get(common.myInt(Session["ModuleId"]).ToString(), "0");
                    if (dt.Rows.Count > 0)
                    {
                        DataView DV = dt.DefaultView;

                        DV.RowFilter = "";
                        DV.Sort = "";

                        DV.RowFilter = "ModuleId=" + common.myInt(Session["ModuleId"]);
                        DV.Sort = "Hierarchy,Sequence ASC";

                        DataTable tbl = DV.ToTable().Copy();

                        if (tbl.Rows.Count > 0)
                        {
                            tbl.AcceptChanges();

                            for (int rIdx = 0; rIdx < tbl.Rows.Count; rIdx++)
                            {
                                if (common.myInt(tbl.Rows[rIdx]["ParentId"]) < 1)
                                {
                                    tbl.Rows[rIdx]["ParentId"] = DBNull.Value;
                                    tbl.AcceptChanges();
                                }
                                //Done By Ujjwal 16 June 2015 to add page id in the url when clicked from rad menu start
                                string CurrentPageURL = common.myStr(tbl.Rows[rIdx]["PageURL"]).Trim();
                                string ModuleLocation = common.myStr(tbl.Rows[rIdx]["ModuleLocation"]).Trim();
                                if (common.myStr(Localhost).Length > 0) { ModuleLocation = Localhost; }

                                if (common.myLen(CurrentPageURL) > 0)
                                {
                                    if (!common.myStr(tbl.Rows[rIdx]["PageURL"]).ToUpper().Contains("MPG"))
                                    {
                                        if (common.myStr(tbl.Rows[rIdx]["PageURL"]).Contains("?"))
                                        {
                                            tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + "&Mpg=P" + common.myInt(tbl.Rows[rIdx]["PageId"]) + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);
                                        }
                                        else
                                        {
                                            tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + "?Mpg=P" + common.myInt(tbl.Rows[rIdx]["PageId"]) + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);
                                        }
                                    }
                                    else
                                    {
                                        // tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);
                                        if (common.myInt(Session["ModuleId"]).Equals(3))
                                        {
                                            if (!common.myInt(Session["OldModuleId"]).Equals(3))
                                            {
                                                if (common.myStr(tbl.Rows[rIdx]["PageUrl"]).Contains("PatientDashboardForDoctor.aspx"))
                                                {
                                                    str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" +
                                                        //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                                        Get(common.myInt(Session["ModuleId"]).ToString(), "0");
                                                    Session["OldModuleId"] = "3";
                                                    Session["SetQueryString"] = "1";
                                                    Session["SetQueryStringValue"] = str;
                                                    tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);

                                                }
                                                else
                                                {
                                                    str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" +
                                                        //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                                        Get(common.myInt(Session["ModuleId"]).ToString(), "0");
                                                    tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);

                                                }
                                            }
                                            else
                                            {
                                                if (common.myStr(tbl.Rows[rIdx]["PageUrl"]).Contains("PatientDashboardForDoctor.aspx"))
                                                {
                                                    str = "&EMRSingleScreenPageUrl=1&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" +
                                                        //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                                        Get(common.myInt(Session["ModuleId"]).ToString(), "0");
                                                    tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);

                                                }
                                                else
                                                {
                                                    str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" +
                                                        //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                                        Get(common.myInt(Session["ModuleId"]).ToString(), "0");
                                                    tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);

                                                }
                                            }
                                        }


                                    }
                                }
                                //Done By Ujjwal 16 June 2015 to add page id in the url when clicked from rad menu end

                            }

                            RadMenu1.DataSource = tbl;
                            //Establish hierarchy: 
                            RadMenu1.DataFieldID = "PageId";
                            RadMenu1.DataFieldParentID = "ParentId";

                            //Set Text, Value, and NavigateUrl:  
                            RadMenu1.DataTextField = "PageName";
                            RadMenu1.DataValueField = "PageId";
                            RadMenu1.DataNavigateUrlField = "PageUrl";
                            RadMenu1.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "ShowMenuItems");
        }
        finally
        {

        }
    }

    protected void RadMenu1_ItemDataBound(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
        if (e.Item.NavigateUrl == Request.Url.PathAndQuery)
        {
            e.Item.ForeColor = System.Drawing.Color.White;
            e.Item.BackColor = System.Drawing.Color.Fuchsia;
        }
    }
    public void ExtractUserMenuFromSession()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        try
        {
            string[] columnNames = new string[1];

            if ((Session["MenuDetail"] == null) || (Session["GroupDetail"] == null) || (Session["ModuleDetail"] == null))
            {
                DataSet objDs = new DataSet();
                objDs = objSecurity.getSecurityUserPages(common.myInt(Session["GroupID"]), Request.Url.Host, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

                if (objDs.Tables.Count > 2)
                {
                    Session["ModuleDetail"] = objDs.Tables[0].Copy();
                    Session["GroupDetail"] = objDs.Tables[1].Copy();
                    Session["MenuDetail"] = objDs.Tables[2].Copy();
                }
            }

            DataTable dtMenuDetail = new DataTable();
            DataTable dtGroupDetail = new DataTable();
            DataTable dtModuleDetail = new DataTable();
            DataTable dtDistinctModuleId = new DataTable();
            DataTable dtDistinctModuleDetail = new DataTable();
            DataTable dtDistinctPageId = new DataTable();
            DataTable dtDistinctPageDetail = new DataTable();
            dtMenuDetail = (DataTable)Session["MenuDetail"];
            dtGroupDetail = (DataTable)Session["GroupDetail"];
            dtModuleDetail = (DataTable)Session["ModuleDetail"];
            if ((dtMenuDetail.Rows.Count == 0) || (dtGroupDetail.Rows.Count == 0) || (dtModuleDetail.Rows.Count == 0))
            {
                DataSet objDs = new DataSet();
                objDs = objSecurity.getSecurityUserPages(common.myInt(Session["GroupID"]), Request.Url.Host, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

                if (objDs.Tables.Count > 2)
                {
                    Session["ModuleDetail"] = objDs.Tables[0].Copy();
                    Session["GroupDetail"] = objDs.Tables[1].Copy();
                    Session["MenuDetail"] = objDs.Tables[2].Copy();
                }
                dtMenuDetail = (DataTable)Session["MenuDetail"];
                dtGroupDetail = (DataTable)Session["GroupDetail"];
                dtModuleDetail = (DataTable)Session["ModuleDetail"];
            }
            if (common.myStr(Session["IsAdminGroup"]) == "True")
            {
                dtGroupDetail.DefaultView.RowFilter = "";
                dtGroupDetail.DefaultView.RowFilter = "ISNULL(GroupId,0)=" + common.myInt(Session["GroupId"]);
                Session["PrintAuthentication"] = dtGroupDetail.DefaultView.ToTable();
                dtGroupDetail.DefaultView.RowFilter = "";

                columnNames = new string[1];
                columnNames[0] = "ModuleId";
                dtGroupDetail.DefaultView.RowFilter = "";
                dtDistinctModuleId = dtGroupDetail.DefaultView.ToTable(true, columnNames);
                if (dtDistinctModuleId.Rows.Count > 0)
                {
                    dtDistinctModuleDetail = dtModuleDetail.Clone();
                    foreach (DataRow dr in dtDistinctModuleId.Rows)
                    {
                        DataView dvModuleDetail = dtModuleDetail.DefaultView;
                        dvModuleDetail.RowFilter = "";
                        dvModuleDetail.RowFilter = "ModuleId=" + common.myInt(dr["ModuleId"]);
                        if (dvModuleDetail.Count > 0)
                        {
                            if (dvModuleDetail.ToTable().Rows.Count > 0)
                            {
                                DataRow drSelected = dtDistinctModuleDetail.NewRow();

                                drSelected["ModuleId"] = common.myInt(dvModuleDetail[0]["ModuleId"]);
                                drSelected["ModuleName"] = common.myStr(dvModuleDetail[0]["ModuleName"]);
                                drSelected["Sequence"] = common.myInt(dvModuleDetail[0]["Sequence"]);
                                drSelected["ModuleFlag"] = common.myStr(dvModuleDetail[0]["ModuleFlag"]);
                                drSelected["ImageUrl"] = common.myStr(dvModuleDetail[0]["ImageUrl"]);

                                dtDistinctModuleDetail.Rows.Add(drSelected);
                            }
                        }
                    }
                }

                dtDistinctPageDetail = dtMenuDetail.Copy();

                if (dtDistinctModuleDetail.Rows.Count > 0)
                {
                    dtDistinctModuleDetail.DefaultView.Sort = "";
                    dtDistinctModuleDetail.DefaultView.Sort = "Sequence ASC";
                    Session["mainMenu"] = dtDistinctModuleDetail;
                }
                if (dtMenuDetail.Rows.Count > 0)
                {
                    dtMenuDetail.DefaultView.RowFilter = "IsPopupPage=False";
                    dtMenuDetail.DefaultView.Sort = "";
                    dtMenuDetail.DefaultView.Sort = "ModuleId,Hierarchy,Sequence ASC";
                    Session["MenuData"] = dtMenuDetail.DefaultView.ToTable().Copy();
                    ViewState["PharmacyIntimationRights"] = "Y";
                    ViewState["BillingIntimationRights"] = "Y";
                    ViewState["ICIntimationRights"] = "Y";
                    ViewState["BedTransferRequest"] = "Y";
                }
            }
            else
            {
                dtGroupDetail.DefaultView.RowFilter = "";
                dtGroupDetail.DefaultView.RowFilter = "GroupId=" + common.myInt(Session["GroupId"]);
                Session["PrintAuthentication"] = dtGroupDetail.DefaultView.ToTable();
                if (dtGroupDetail.Rows.Count > 0)
                {
                    columnNames = new string[1];
                    columnNames[0] = "ModuleId";
                    dtDistinctModuleId.DefaultView.RowFilter = "";
                    dtDistinctModuleId = dtGroupDetail.DefaultView.ToTable(true, columnNames);
                    if (dtDistinctModuleId.Rows.Count > 0)
                    {
                        dtDistinctModuleDetail = dtModuleDetail.Clone();
                        foreach (DataRow dr in dtDistinctModuleId.Rows)
                        {
                            DataView dvModuleDetail = dtModuleDetail.DefaultView;
                            dtModuleDetail.DefaultView.RowFilter = "";
                            dtModuleDetail.DefaultView.RowFilter = "ModuleId=" + common.myInt(dr["ModuleId"]);
                            if (dtModuleDetail.Rows.Count > 0)
                            {
                                if (dvModuleDetail.ToTable().Rows.Count > 0)
                                {
                                    DataRow drSelected = dtDistinctModuleDetail.NewRow();

                                    drSelected["ModuleId"] = common.myInt(dvModuleDetail[0]["ModuleId"]);
                                    drSelected["ModuleName"] = common.myStr(dvModuleDetail[0]["ModuleName"]);
                                    drSelected["Sequence"] = common.myInt(dvModuleDetail[0]["Sequence"]);
                                    drSelected["ModuleFlag"] = common.myStr(dvModuleDetail[0]["ModuleFlag"]);
                                    drSelected["ImageUrl"] = common.myStr(dvModuleDetail[0]["ImageUrl"]);

                                    dtDistinctModuleDetail.Rows.Add(drSelected);
                                }
                            }
                        }
                        dtGroupDetail.DefaultView.RowFilter = "";
                        dtGroupDetail.DefaultView.RowFilter = "GroupId=" + common.myInt(Session["GroupId"]);

                        if (dtGroupDetail.Rows.Count > 0)
                        {

                            dtDistinctPageDetail = dtMenuDetail.Clone();
                            foreach (DataRowView drv in dtGroupDetail.DefaultView)
                            {
                                DataView dvPageDetails = dtMenuDetail.DefaultView;
                                dvPageDetails.RowFilter = "";
                                dvPageDetails.RowFilter = "PageId=" + common.myInt(drv["PageId"]);
                                if (dvPageDetails.Count > 0)
                                {
                                    if (dvPageDetails.ToTable().Rows.Count > 0)
                                    {
                                        DataRow drSelected = dtDistinctPageDetail.NewRow();

                                        drSelected["ModuleId"] = common.myInt(dvPageDetails[0]["ModuleId"]);
                                        drSelected["PageId"] = common.myInt(dvPageDetails[0]["PageId"]);
                                        drSelected["PageName"] = common.myStr(dvPageDetails[0]["PageName"]);
                                        drSelected["ParentId"] = common.myInt(dvPageDetails[0]["ParentId"]);
                                        drSelected["Hierarchy"] = common.myStr(dvPageDetails[0]["Hierarchy"]);
                                        drSelected["Sequence"] = common.myInt(dvPageDetails[0]["Sequence"]);
                                        drSelected["PageUrl"] = common.myStr(dvPageDetails[0]["PageUrl"]);
                                        drSelected["IsPopupPage"] = common.myStr(dvPageDetails[0]["IsPopupPage"]);
                                        drSelected["ModuleLocation"] = common.myStr(dvPageDetails[0]["ModuleLocation"]);

                                        dtDistinctPageDetail.Rows.Add(drSelected);
                                    }
                                }
                            }
                        }
                    }
                    if (dtDistinctModuleDetail.Rows.Count > 0)
                    {
                        dtDistinctModuleDetail.DefaultView.Sort = "Sequence Asc";
                        Session["mainMenu"] = dtDistinctModuleDetail;
                    }
                    if (dtDistinctPageDetail.Rows.Count > 0)
                    {
                        dtDistinctPageDetail.DefaultView.RowFilter = "IsPopupPage=False";
                        dtDistinctPageDetail.DefaultView.Sort = "";
                        dtDistinctPageDetail.DefaultView.Sort = "ModuleId,Hierarchy,Sequence Asc";
                        Session["MenuData"] = dtDistinctPageDetail.DefaultView.ToTable().Copy();

                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=PHR%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            ViewState["PharmacyIntimationRights"] = "Y";
                        }
                        else
                        {
                            ViewState["PharmacyIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=BILL%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            ViewState["BillingIntimationRights"] = "Y";
                        }
                        else
                        {
                            ViewState["BillingIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=IC%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            ViewState["ICIntimationRights"] = "Y";
                        }
                        else
                        {
                            ViewState["ICIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%/ATD/BedTransferRequest.aspx%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            ViewState["BedTransferRequest"] = "Y";
                        }
                        else
                        {
                            ViewState["BedTransferRequest"] = "N";
                        }

                        dtDistinctPageDetail.DefaultView.RowFilter = "";
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            string lineNumber = Ex.StackTrace.Substring(Ex.StackTrace.Length - 7, 7);
            objException.HandleExceptionWithMethod(Ex, "ExtractUserMenuFromSession" + lineNumber);
        }

    }
    public void btnReset_OnClick(object sender, EventArgs e)
    {

    }
    protected void lnkShowCriticalResults_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?MPG=P22295&CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&PageSource=Ward";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 20;
        RadWindow1.Left = 20;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void lnkDiagnosticHistory_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMR/PatientHistory.aspx?Master=BLANK";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 20;
        RadWindow1.Left = 20;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void imgbiomenu_Click(object sender, System.EventArgs e)
    {
        string key = "Biomenu" + common.myStr(Session["RegistrationId"]);
        if (Cache[key] != null)
        {
            Response.Redirect(common.myStr(Cache[key]));
        }
    }
    public void ibtnRecentscan_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Bioauth/?REHIS=Yes");
    }
    // Abhishek work
    public string Get(string iModuelId, string iNodeID)
    {
        return common.myStr(Session["IsAdminGroup"]) + "_"
                + common.myStr(Session["LoginIsAdminGroup"]) + "_"
                + common.myStr(Session["HospitalLocationID"]) + "_"
                + common.myStr(Session["FacilityID"]) + "_"
                + common.myStr(Session["GroupID"]) + "_"
                + common.myStr(Session["FinancialYearId"]) + "_"
                + common.myStr(Session["EntrySite"]) + "_"
                + common.myStr(Session["UserId"]) + "_"
                + common.myStr(Session["UserName"]).Replace(" ", "%") + "_"
                + common.myInt(iModuelId) + "_"
                + common.myInt(iNodeID) + "_"
                + common.myStr(Session["FacilityName"]).Replace(" ", "%") + "_"
                + common.myStr(Session["CanDownloadPatientDocument"]) + "_"
                + common.myInt(Session["FacilityStateId"]).ToString();
    }
    // Abhishek work

    protected void ibtnNotification_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(hdnNotification.Value) == 0)
        {
            hdnNotification.Value = "1";
            dvNotification.Visible = true;
        }
        else if (common.myInt(hdnNotification.Value) == 1)
        {
            hdnNotification.Value = "0";
            dvNotification.Visible = false;
        }
    }

    protected void btnCloseDiv_OnClick(object sender, EventArgs e)
    {
        dvNotification.Visible = false;
        hdnNotification.Value = "0";
    }
    protected void grdViewNotification_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandArgument.ToString() == "OpenLink")
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdnLink = (HiddenField)row.FindControl("hdnLinkPage");
            HiddenField hdnType = (HiddenField)row.FindControl("hdnType");

            if (common.myStr(hdnLink.Value) != "")
            {
                if (common.myStr(hdnType.Value) == "BT")
                {
                    hdnNotification.Value = "0";
                    RadWindow1.NavigateUrl = common.myStr(hdnLink.Value);
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(hdnType.Value) == "BP")
                {
                    hdnNotification.Value = "0";
                    RadWindow1.NavigateUrl = common.myStr(hdnLink.Value);
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(hdnType.Value) == "FR")
                {
                    hdnNotification.Value = "0";
                    RadWindow1.NavigateUrl = common.myStr(hdnLink.Value);
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(hdnType.Value) == "PC")
                {

                    RadWindow1.NavigateUrl = common.myStr(hdnLink.Value);
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(hdnType.Value) == "OV")
                {
                    RadWindow1.NavigateUrl = common.myStr(hdnLink.Value);
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                //dvNotification.Visible = false;
            }
        }
    }


    protected void imgQms_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("/QMS");
    }

}
