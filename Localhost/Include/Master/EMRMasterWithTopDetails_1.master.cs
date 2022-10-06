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

public partial class Include_Master_EMRMasterWithTopDetails_1 : System.Web.UI.MasterPage
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private Hashtable hshInput;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Init(object sender, System.EventArgs e)
    {
        try
        {
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
                && common.myStr(Request.QueryString["OP"]).Split('_').Length > 10)
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

            ExtractUserMenuFromCache();

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

        BaseC.User valUser = new BaseC.User(sConString);
        DataSet dsEmpType = new DataSet();
        if (common.myInt(Session["EmployeeId"]).Equals(0))
        {
            Session["EmployeeId"] = valUser.getEmployeeId(common.myInt(Session["UserID"]));
        }
        if (common.myStr(Session["EmployeeTypePermission"]) == "" || common.myStr(Session["EmployeeType"]) == ""
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


        string notificationCheck = "Y";

        if (common.myStr(Session["EmployeeType"]) == "D")
        { notificationCheck = "N"; }
        else if (common.myStr(Session["EmployeeType"]) == "N")
        { notificationCheck = "N"; }
        else if (common.myStr(Session["EmployeeType"]) == "LD")
        { notificationCheck = "N"; }
        else if (common.myStr(Session["EmployeeType"]) == "LDIR")
        {
            notificationCheck = "N";
        }
        if (notificationCheck == "Y")
        { NotificationCheck(); }

        try
        {
            if (!IsPostBack)
            {

                System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

                collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                         "EMRTriageModuleId", sConString);

                if (collHospitalSetupValues.ContainsKey("EMRTriageModuleId"))
                    ViewState["EMRTriageModuleId"] = collHospitalSetupValues["EMRTriageModuleId"];


                dvNotification.Visible = false;
                if (common.myStr(Session["EmployeeType"]) == "D")
                {
                    string QueryStringBindPatientLists = common.myStr(Request.QueryString["QueryStringBindPatientLists"]);
                    //if (common.myInt(common.myStr(Session["ModuleId"])).Equals(3)) // emr 
                    //{
                    //    Session["BindPatientLists"] = true;
                    //}
                    if (!common.myBool(Session["BindPatientLists"]))
                    {
                        Radslidingzone2.ExpandedPaneId = "";
                        ViewState["BindEMRMenus"] = false;
                    }
                    else
                    {
                        //if (!common.myInt(QueryStringBindPatientLists).Equals(0))
                        //{
                        if (Request.QueryString["EMRSingleScreenPageUrl"] == null || common.myStr(Request.QueryString["EMRSingleScreenPageUrl"]).Equals(string.Empty))
                        {
                            Radslidingzone2.ExpandedPaneId = "rdpAppList";
                            ViewState["BindEMRMenus"] = true;
                        }

                        else
                        {
                            if (common.myInt(Request.QueryString["EMRSingleScreenPageUrl"]).Equals(1))
                            {
                                Radslidingzone2.ExpandedPaneId = "";
                                ViewState["BindEMRMenus"] = false;
                            }
                        }
                        //}


                        //if (!common.myBool(Session["FindPatientExpanded"]))
                        //{
                        //   Radslidingzone2.ExpandedPaneId = "rdpAppList";
                        //    Session["FindPatientExpanded"] = true;
                        //}
                        //else
                        //{
                        //    Radslidingzone2.ExpandedPaneId = "";
                        //}
                    }
                    Session["BindPatientLists"] = true;

                    //if (!common.myBool(Session["FindPatientExpanded"]))
                    //{
                    //    Radslidingzone2.ExpandedPaneId = "rdpAppList";
                    //    Session["FindPatientExpanded"] = true;
                    //}
                    //else
                    //{
                    //    Radslidingzone2.ExpandedPaneId = "";
                    //}
                }

                string facilityTitleName = common.myStr(Session["FacilityName"]);
                if (common.myStr(Session["FacilityName"]).Equals(string.Empty))
                {
                    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);

                    facilityTitleName = emr.BindApplicationName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]));
                    emr = null;
                }

                Page.Header.Title = facilityTitleName;




                if ((Session["MenuDetail"] == null) || (Session["GroupDetail"] == null) || (Session["ModuleDetail"] == null) || (Session["mainMenu"] == null) || (Session["MenuData"] == null))
                {
                    ExtractUserMenuFromCache();
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
                    if (common.myBool(ViewState["BindEMRMenus"]))
                    {
                        Session["ModuleName"] = "EMR";
                    }
                    else
                    {
                        Session["ModuleName"] = gvModules.SelectedRow.Cells[1].Text;
                    }
                    Session["ModuleFlag"] = gvModules.SelectedRow.Cells[3].Text;


                    BindTree();
                    Session["SetQueryString"] = "0";
                    Session["SetQueryStringValue"] = string.Empty;

                    DoSelectCurrentNode(tvCategory);
                }
                if ((Session["RegistrationId"] != null && Session["RegistrationId"].ToString() != ""
                    && common.myStr(Session["EncounterID"]) != null) &&
                    (common.myStr(Session["ModuleName"]) == "Nurse Workbench") || (common.myStr(Session["ModuleName"]) == "EMR"))
                {
                    BaseC.Patient objPatient = new BaseC.Patient(sConString);
                    if (common.myStr(Session["RegistrationID"]) != "&nbsp;")
                    {
                        SqlDataReader objDr = (SqlDataReader)objPatient.getPatientShortDetail(common.myInt(Session["RegistrationID"]), common.myInt(Session["EncounterId"]));
                        try
                        {
                            if (objDr.Read())
                            {
                                Radslidingpane4.Visible = false;
                                if (common.myLen(objDr["Name"]) > 30)
                                {
                                    Radslidingpane4.Title = "<b><font name='Tahoma'>" + common.myStr(objDr["Name"]).Trim().Substring(0, 30) + "</font></b>";
                                }
                                else
                                {
                                    Radslidingpane4.Title = "<b><font name='Tahoma'>" + common.myStr(objDr["Name"]).Trim() + "</font></b>";
                                }
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            objDr.Close();
                        }
                    }
                }
                ShowMenuItems();


                if (common.myInt(Session["ModuleId"]).Equals(3) || common.myInt(Session["ModuleId"]).Equals(30) || common.myInt(Session["ModuleId"]).Equals(5) || common.myStr(Session["EmployeeType"]) == "D" || common.myInt(Session["ModuleId"]).Equals(44) || common.myInt(Session["ModuleId"]).Equals(common.myInt(ViewState["EMRTriageModuleId"])))
                {
                    Radslidingzone2.Visible = true;
                    lnkDiagnosticHistory.Visible = true;
                }
                else
                {
                    Radslidingzone2.Visible = false;
                    lnkDiagnosticHistory.Visible = false;
                }

                if (common.myStr(Session["FacilityName"]).ToUpper().Contains("PRACHI"))
                {
                    Radslidingzone2.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            //objException.HandleExceptionWithMethod(Ex, "PageLoad");
        }
        lblNotification.Visible = false;
    }



    protected void BindModuleGrid()
    {
        DataSet objDs = new DataSet();
        DataSet ds = new DataSet();
        DataSet dsModule = new DataSet();
        DataTable dt = new DataTable();
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
            ViewState["BindEMRMenus"] = false;
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
            if (common.myBool(ViewState["BindEMRMenus"]))
            {
                BindPages(3, tvCategory);
            }
            else
            {
                BindPages(Convert.ToInt16(gvModules.SelectedRow.Cells[0].Text), tvCategory);
            }

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
                    dv = objDs1.DefaultView;
                    dv.RowFilter = "ModuleId =" + iModuleId;
                    DataTable dt = new DataTable();
                    dt = dv.ToTable();
                    DataSet objDs = new DataSet();
                    objDs.Tables.Add(dt);


                    for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
                    {
                        // AddNodes(tvCategory, common.myInt(objDs.Tables[0].Rows[i][1]), common.myInt(objDs.Tables[0].Rows[i][3]), common.myStr(objDs.Tables[0].Rows[i][2]), common.myStr(objDs.Tables[0].Rows[i][6]), strTickSign);
                        // AddNodes(tvCategory, common.myInt(objDs.Tables[0].Rows[i]["PageId"]), common.myInt(objDs.Tables[0].Rows[i]["ParentId"]), common.myStr(objDs.Tables[0].Rows[i]["PageName"]), common.myStr(objDs.Tables[0].Rows[i]["PageUrl"]), strTickSign);
                        AddNodes(tvCategory, common.myInt(objDs.Tables[0].Rows[i]["PageId"]), common.myInt(objDs.Tables[0].Rows[i]["ParentId"]), common.myStr(objDs.Tables[0].Rows[i]["PageName"]), common.myStr(objDs.Tables[0].Rows[i]["PageUrl"]), common.myStr(common.myStr(objDs.Tables[0].Rows[i]["ModuleLocation"])), strTickSign, common.myInt(objDs.Tables[0].Rows[i]["ModuleId"]));

                        //if(common.myStr(objDs.Tables[0].Rows[i]["PageUrl"]).Contains("PatientDashboardForDoctor.aspx"))
                        //{
                        //    hdnEMRSingleScreenFlag.Value = "1";
                        //}
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
                            str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myStr(iModuelId), common.myStr(iNodeID)); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);
                        }
                    }
                    else
                    {
                        if (common.myStr(sPageUrl).Contains("PatientDashboardForDoctor.aspx"))
                        {
                            str = "&EMRSingleScreenPageUrl=1&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myStr(iModuelId), common.myStr(iNodeID)); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);
                        }
                        else
                        {
                            str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myStr(iModuelId), common.myStr(iNodeID)); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);
                        }
                    }
                }
                else
                {
                    str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myStr(iModuelId), common.myStr(iNodeID)); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);
                }
            }
            else
            {
                str = common.myStr(Session["SetQueryStringValue"]);
            }
            //else if (!common.myInt(Session["OldModuleId"]).Equals(3))
            //{
            //    str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);
            //}

            //else
            //{
            //    str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(iModuelId) + "_" + common.myStr(iNodeID);

            //}

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
                string str = string.Empty;
                if (dt != null)
                {
                    //  string str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);

                    if (dt.Rows.Count > 0)
                    {
                        DataView DV = dt.DefaultView;

                        DV.RowFilter = "";
                        DV.Sort = "";

                        DV.RowFilter = "ModuleId=" + common.myInt(Session["ModuleId"]);
                        DV.Sort = "Hierarchy,Sequence ASC";

                        DataTable tblPageDetails = DV.ToTable().Copy();



                        for (int rIdxPageDetails = 0; rIdxPageDetails < tblPageDetails.Rows.Count; rIdxPageDetails++)
                        {

                            if (!common.myInt(Session["SetQueryString"]).Equals(1))
                            {

                                if (common.myInt(Session["ModuleId"]).Equals(3))
                                {
                                    if (!common.myInt(Session["OldModuleId"]).Equals(3))
                                    {
                                        if (common.myStr(tblPageDetails.Rows[rIdxPageDetails]["PageUrl"]).Contains("PatientDashboardForDoctor.aspx"))
                                        {
                                            str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myInt(Session["ModuleId"]).ToString(), "0"); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                            Session["OldModuleId"] = "3";
                                            Session["SetQueryString"] = "1";
                                            Session["SetQueryStringValue"] = str;
                                        }
                                        else
                                        {
                                            str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myInt(Session["ModuleId"]).ToString(), "0"); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                        }
                                    }
                                    else
                                    {
                                        if (common.myStr(tblPageDetails.Rows[rIdxPageDetails]["PageUrl"]).Contains("PatientDashboardForDoctor.aspx"))
                                        {
                                            str = "&EMRSingleScreenPageUrl=1&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myInt(Session["ModuleId"]).ToString(), "0"); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                        }
                                        else
                                        {
                                            str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myInt(Session["ModuleId"]).ToString(), "0"); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                str = common.myStr(Session["SetQueryStringValue"]);
                            }
                        }

                    }


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

                                        /*if (!common.myInt(Session["OldModuleId"]).Equals(3))
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
                                        { // "&EMRSingleScreenPageUrl=1
                                            if (common.myStr(tbl.Rows[rIdx]["PageUrl"]).Contains("PatientDashboardForDoctor.aspx"))
                                            {
                                                if (common.myStr(tbl.Rows[rIdx]["PageURL"]).Contains("?"))
                                                {
                                                    tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL  + "&Mpg=P" + common.myInt(tbl.Rows[rIdx]["PageId"]) + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]) + "&EMRSingleScreenPageUrl=1";
                                                }
                                                else
                                                {
                                                    tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL  + "?Mpg=P" + common.myInt(tbl.Rows[rIdx]["PageId"]) + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]) + "&EMRSingleScreenPageUrl=1";
                                                }
                                            }
                                            else
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
                                        }*/
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
                                                    str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myInt(Session["ModuleId"]).ToString(), "0"); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                                    Session["OldModuleId"] = "3";
                                                    Session["SetQueryString"] = "1";
                                                    Session["SetQueryStringValue"] = str;
                                                    tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);

                                                }
                                                else
                                                {
                                                    str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myInt(Session["ModuleId"]).ToString(), "0"); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                                    tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);

                                                }
                                            }
                                            else
                                            {
                                                if (common.myStr(tbl.Rows[rIdx]["PageUrl"]).Contains("PatientDashboardForDoctor.aspx"))
                                                {
                                                    str = "&EMRSingleScreenPageUrl=1&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myInt(Session["ModuleId"]).ToString(), "0"); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
                                                    tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);

                                                }
                                                else
                                                {
                                                    str = "&irtrf=" + common.myStr(Session["StrO"]) + "&OP=" + Get(common.myInt(Session["ModuleId"]).ToString(), "0"); //common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]) + "_" + common.myStr(Session["ModuleId"]);
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
        if (e.Item.Selected == true)
        {
            //Session["ModuleId"] = "";
        }
        //e.Item.ToolTip = "Read more about " +
        //(string)DataBinder.Eval(e.Item.DataItem, "Text");
    }
    public void ExtractUserMenuFromCache()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        try
        {
            string[] columnNames = new string[1];
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            if ((Session["MenuDetail"] == null) || (Session["GroupDetail"] == null) || (Session["ModuleDetail"] == null))
            {
                //DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspSecurityGetUserPages");
                DataSet objDs = objSecurity.getSecurityUserPages(common.myInt(Session["GroupID"]), Request.Url.Host, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
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
                //DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspSecurityGetUserPages");
                DataSet objDs = objSecurity.getSecurityUserPages(common.myInt(Session["GroupID"]), Request.Url.Host, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                if (objDs.Tables.Count > 2)
                {
                    Session["ModuleDetail"] = objDs.Tables[0].Copy();
                    Session["GroupDetail"] = objDs.Tables[1].Copy();
                    Session["MenuDetail"] = objDs.Tables[2].Copy();
                }
            }
            if (common.myBool(Session["IsAdminGroup"]))
            {
                try
                {
                    dtGroupDetail.DefaultView.RowFilter = "";
                    dtGroupDetail.DefaultView.RowFilter = "ISNULL(GroupId,0)=" + common.myInt(Session["GroupId"]);
                    Session["PrintAuthentication"] = dtGroupDetail.DefaultView.ToTable();
                    dtGroupDetail.DefaultView.RowFilter = "";
                }
                catch
                {
                }
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
                    Session["PharmacyIntimationRights"] = "Y";
                    Session["BillingIntimationRights"] = "Y";
                    Session["ICIntimationRights"] = "Y";
                    Session["BedTransferRequest"] = "Y";
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
                            Session["PharmacyIntimationRights"] = "Y";
                        }
                        else
                        {
                            Session["PharmacyIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=BILL%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            Session["BillingIntimationRights"] = "Y";
                        }
                        else
                        {
                            Session["BillingIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=IC%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            Session["ICIntimationRights"] = "Y";
                        }
                        else
                        {
                            Session["ICIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%/ATD/BedTransferRequest.aspx%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            Session["BedTransferRequest"] = "Y";
                        }
                        else
                        {
                            Session["BedTransferRequest"] = "N";
                        }

                        dtDistinctPageDetail.DefaultView.RowFilter = "";
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            //string lineNumber = Ex.StackTrace.Substring(Ex.StackTrace.Length - 7, 7);
            objException.HandleExceptionWithMethod(Ex, "ExtractUserMenuFromCache");
        }
    }

    public void btnReset_OnClick(object sender, EventArgs e)
    {

    }

    DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 1;
        dt.Columns["SNo"].AutoIncrementStep = 1;
        dt.Columns.Add("Notification");
        dt.Columns.Add("Count");
        dt.Columns.Add("LinkPage");
        dt.Columns.Add("Type");
        return dt;
    }
    protected void NotificationCheck()
    {
        try
        {
            string fileLoc = Server.MapPath("~/PatientDocuments/Notofication_SB" + common.myInt(Session["FacilityId"]) + ".txt");

            string read = common.ReadNotificationFile(fileLoc);
            DataSet ds = new DataSet("table1");
            string ShowNotification = "N";
            string PharmacyCount = "0";
            string BillingCount = "0", BPCount = "0", BTRCount = "0", OpenVisits = "0";
            string InsuranceCount = "0";
            string StatusType = "";


            if (read != "")
            {
                string xmlSchema = common.myStr(read);
                StringReader sr = new StringReader(xmlSchema);
                ds.ReadXml(sr);
                DataTable dtGrid = CreateTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumnCollection columns = ds.Tables[0].Columns;

                    if (columns.Contains("ShowNotification"))
                        ShowNotification = common.myStr(ds.Tables[0].Rows[0]["ShowNotification"]);
                    if (columns.Contains("SBCount"))
                        PharmacyCount = common.myStr(ds.Tables[0].Rows[0]["SBCount"]);
                    if (columns.Contains("PCCount"))
                        BillingCount = common.myStr(ds.Tables[0].Rows[0]["PCCount"]);
                    if (columns.Contains("ICCount"))
                        InsuranceCount = common.myStr(ds.Tables[0].Rows[0]["ICCount"]);
                    if (columns.Contains("BPCount"))
                        BPCount = common.myStr(ds.Tables[0].Rows[0]["BPCount"]);
                    if (columns.Contains("BTRCount"))
                        BTRCount = common.myStr(ds.Tables[0].Rows[0]["BTRCount"]);
                    if (columns.Contains("StatusType"))
                        StatusType = common.myStr(ds.Tables[0].Rows[0]["StatusType"]);
                    if (columns.Contains("OV" + common.myStr(Session["UserId"])))
                        OpenVisits = common.myStr(ds.Tables[0].Rows[0]["OV" + common.myStr(Session["UserId"])]);
                    int Count = 0;

                    if ((common.myStr(Session["PharmacyIntimationRights"]) == "Y") && (common.myStr(ShowNotification) == "Y") && (common.myInt(PharmacyCount) > 0))
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = " New Pharmacy Intimations Pending!";
                        dr["Count"] = common.myStr(PharmacyCount);
                        dr["LinkPage"] = "/EMRBILLING/Popup/PatientIntimationStatus.aspx?PT=PHR&From=POPUP";
                        dr["Type"] = "PC";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(PharmacyCount);
                    }
                    if ((common.myStr(Session["BillingIntimationRights"]) == "Y") && (common.myStr(ShowNotification) == "Y") && (common.myInt(BillingCount) > 0))
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = "  File Recevied Intimations Pending!";
                        dr["Count"] = common.myStr(BillingCount);
                        dr["LinkPage"] = "/EMRBILLING/Popup/PatientIntimationStatus.aspx?PT=BILL&From=POPUP";
                        dr["Type"] = "FR";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(BillingCount);
                    }

                    if ((common.myStr(Session["ICIntimationRights"]) == "Y") && (common.myStr(ShowNotification) == "Y") && (common.myInt(InsuranceCount) > 0))
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = " Insurance Clearance Intimations Pending!";
                        dr["Count"] = common.myStr(InsuranceCount);
                        dr["LinkPage"] = "/EMRBILLING/Popup/PatientIntimationStatus.aspx?PT=IC&From=POPUP";
                        dr["Type"] = "IC";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(InsuranceCount);
                    }
                    if ((common.myStr(Session["BillingIntimationRights"]) == "Y") && (common.myStr(ShowNotification) == "Y") && (common.myInt(BPCount) > 0))
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = " Bill Prepration Intimations Pending!";
                        dr["Count"] = common.myStr(BPCount);
                        dr["LinkPage"] = "";
                        dr["Type"] = "BP";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(BPCount);
                    }

                    if ((common.myStr(Session["BedTransferRequest"]) == "Y") && (common.myStr(ShowNotification) == "Y") && (common.myInt(BTRCount) > 0))
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = " Bed Transfer request Pending!";
                        dr["Count"] = common.myStr(BTRCount);
                        dr["LinkPage"] = "/ATD/BedTransferRequest.aspx?From=POPUP";
                        dr["Type"] = "BT";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(BTRCount);
                    }
                    if (common.myInt(OpenVisits) > 0)
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = " Pending Open Visits!";
                        dr["Count"] = common.myStr(OpenVisits);
                        dr["LinkPage"] = "/EMRReports/Doctorwiseprocedurerpt.aspx?Fromdate=" + Convert.ToString(Convert.ToDateTime(DateTime.Now.Date.AddDays(-7)).ToString("yyyy/MM/dd")) + "&Todate=" + Convert.ToString(Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy/MM/dd")) + "&ReportName=EPS&statusId=7&Export=0&CF=MPage&UserId=" + Convert.ToString(Session["UserId"]) + "";
                        dr["Type"] = "OV";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(OpenVisits);
                    }

                    // ibtnNotification.Visible = false;
                    lblNotification.Visible = false;
                    lblNotification.Text = "";
                    if (common.myInt(Count) > 0)
                    {
                        //   ibtnNotification.Visible = true;
                        lblNotification.Visible = true;
                        lblNotification.Text = Count.ToString();
                        grdViewNotification.DataSource = dtGrid;
                        grdViewNotification.DataBind();
                    }

                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void AddKeepAlive()
    {
        return;
        if (Session["KeepAlive"] != null)
        {
            return;
        }
        else
        {
            Session["KeepAlive"] = "Y";
        }
        int int_MilliSecondsTimeOut = (this.Session.Timeout * 60000) - 30000;
        string str_Script = @"
            <script type='text/javascript'>
            //Number of Reconnects
            var count=0;
            //Maximum reconnects setting
            var max = 5;
            function Reconnect(){

            count++;
            if (count < max)
            {
            window.status = 'Link to Server Refreshed ' + count.toString()+' time(s)' ;

            var img = new Image(1,1);

            img.src = '/RefreshSessionState.aspx';

            }
            }

            window.setInterval('Reconnect()'," + int_MilliSecondsTimeOut.ToString() + @"); //Set to length required

            </script>

            ";

        this.Page.RegisterClientScriptBlock("Reconnect", str_Script);

    }

    //protected void ibtnNotification_OnClick(object sender, EventArgs e)
    //{
    //    if (common.myInt(hdnNotification.Value) == 0)
    //    {
    //        hdnNotification.Value = "1";
    //        dvNotification.Visible = true;
    //    }
    //    else if (common.myInt(hdnNotification.Value) == 1)
    //    {
    //        hdnNotification.Value = "0";
    //        dvNotification.Visible = false;
    //    }
    //}
    protected void btnCloseDiv_OnClick(object sender, EventArgs e)
    {
        dvNotification.Visible = false;
        hdnNotification.Value = "0";
    }
    protected void grdViewNotification_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandArgument == "OpenLink")
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
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
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
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
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
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
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
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
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
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                }
                //dvNotification.Visible = false;
            }
        }
    }

    protected void lnkShowCriticalResults_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&PageSource=Ward";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 20;
        RadWindow1.Left = 20;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
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
}