using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Telerik.Web.UI;
using System.Collections.Generic;
using BaseC;
using System.Configuration;
using System.Xml;
using System.IO;

public partial class Login : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sKey = ConfigurationManager.ConnectionStrings["key"].ConnectionString;

    void LoginRedir(DataSet ds)
    {
        BaseC.User valUser = new BaseC.User(sConString);
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        clsIVF objivf = new clsIVF(sConString);
        BaseC.clsLISMaster objLisM = new BaseC.clsLISMaster(sConString);
        DataTable tblD = null;
        DataSet SetupDoctor = null;
        DataSet dsEmpType = null;
        try
        {
            Session["MenuDetail"] = null;
            Session["GroupDetail"] = null;
            Session["ModuleDetail"] = null;
            Session["mainMenu"] = null;
            Session["MenuData"] = null;

            ViewState["VHospitalLocationID"] = Session["HospitalLocationID"];
            ViewState["VUserID"] = Session["UserID"];
            Session["IPAddress"] = Request.ServerVariables["REMOTE_ADDR"];

            Session["SeniorCitizenAge"] = null;
            Session["SeniorCitizenCompany"] = null;
            Session["StaffCompanyId"] = null;
            Session["StaffDependentCompanyId"] = null;
            Session["DefaultHospitalCompanyId"] = null;
            Session["DefaultOPDCategory"] = null;
            Cache.Remove("SeniorCitizenAge");
            Cache.Remove("SeniorCitizenCompany");
            Cache.Remove("StaffCompanyId");
            Cache.Remove("StaffDependentCompanyId");
            Cache.Remove("DefaultHospitalCompanyId");
            Cache.Remove("DecimalPlace");
            Cache.Remove("PrintClaimXML");
            Cache.Remove("PrintAuthentication");
            Cache["PrintClaimXML"] = "";
            Cache["SeniorCitizenAge"] = "";
            Cache["SeniorCitizenCompany"] = "";
            Cache["StaffCompanyId"] = "";
            Cache["StaffDependentCompanyId"] = "";
            Cache["DefaultHospitalCompanyId"] = "";
            Cache["DecimalPlace"] = "";
            Cache["DefaultOPDCategory"] = "";

            Cache.Remove("ModuleName" + Session["UserID"] + Session["HospitalLocationID"]);
            Cache.Remove("SubOtherModuleName" + HttpContext.Current.Session["RegistrationID"] + HttpContext.Current.Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);
            Cache.Remove("SubEHRModuleName" + HttpContext.Current.Session["RegistrationID"] + HttpContext.Current.Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);
            Cache.Remove("PatientDetails" + HttpContext.Current.Session["RegistrationID"] + HttpContext.Current.Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);

            FormsAuthentication.RedirectFromLoginPage(common.myStr(ds.Tables[0].Rows[0]["UserName"]), true);

            //  Session["UserName"] = txtUserID.Text;//ds.Tables[0].Rows[0]["UserName"].ToString();
            Session["EmployeeId"] = valUser.getEmployeeId(common.myInt(Session["UserID"]));
            dsEmpType = valUser.getEmployeeTypePermission(common.myInt(Session["EmployeeId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]));
            if (dsEmpType.Tables[0].Rows.Count > 0)
            {
                Session["EmployeeTypePermission"] = Convert.ToBoolean(dsEmpType.Tables[0].Rows[0]["Admin"]);
                Session["EmployeeType"] = common.myStr(dsEmpType.Tables[0].Rows[0]["EmployeeType"]).Trim();
            }

            Cache.Remove("HospitalSetup");
            if (HttpContext.Current.Cache["HospitalSetup"] == null)
            {
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hshIn.Add("@iFacilityId", common.myInt(Session["FacilityId"]));
                DataSet dsHs = new DataSet();
                dsHs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);
                Cache.Insert("HospitalSetup", dsHs.Tables[0], null, DateTime.Now.AddHours(240), System.Web.Caching.Cache.NoSlidingExpiration);
                dsHs.Dispose();
                hshIn = null;
            }
            fillHospitalDefaultValue();
            Session["IsIVFSpecialisation"] = false;
            Session["UserSpecialisationId"] = "0";
            SetupDoctor = objivf.getUserSetupDoctor(common.myInt(Session["EmployeeId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
            if (SetupDoctor.Tables[0].Rows.Count > 0)
            {
                Session["UserSpecialisationId"] = common.myInt(SetupDoctor.Tables[0].Rows[0]["SpecialisationId"]);

                Session["IsIVFSpecialisation"] = common.myBool(SetupDoctor.Tables[0].Rows[0]["IsIVFSpecialisation"]);
            }

            tblD = objLisM.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityID"]), common.myInt(Session["UserID"]));
            Session["LoginDoctorId"] = null;
            Session["IsLoginDoctor"] = false;
            Session["LoginIsAdminGroup"] = "0";
            BaseC.Patient objPatient = new BaseC.Patient(sConString);
            Session["MainFacility"] = common.myStr(objPatient.getMainFacilityId(common.myInt(Session["FacilityID"])));
            objPatient = null;
            if (tblD.Rows.Count > 0)
            {
                Session["LoginDoctorId"] = common.myInt(tblD.Rows[0]["DoctorId"]);
                Session["IsLoginDoctor"] = common.myBool(tblD.Rows[0]["IsDoctor"]);
            }
            else
            {
                Session["LoginDoctorId"] = 0;
                Session["IsLoginDoctor"] = 0;
            }



            if (ViewState["pageurl"] != null)
            {
                string strurl = "";
                strurl = common.myStr(ViewState["pageurl"]);
                Response.Redirect(strurl, false);
            }
            else if (common.myInt(Session["URLPId"]) > 0)
            {
                string surlP = "default.aspx";
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@PageId", common.myInt(Session["URLPId"]));
                DataSet dsP = dl.FillDataSet(CommandType.Text, "SELECT IsNull(pageurl, '') PageURL FROM secmodulepages Where PageId = @PageId", hshIn);
                if (dsP.Tables.Count > 0)
                {
                    if (dsP.Tables[0].Rows.Count > 0)
                    {
                        surlP = common.myStr(dsP.Tables[0].Rows[0]["PageURL"]);
                    }
                }
                Response.Redirect(surlP, false);
            }
            else
            {
                Response.Redirect("default.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            valUser = null; dl = null; objivf = null; objLisM = null; tblD.Dispose(); SetupDoctor.Dispose(); dsEmpType.Dispose();
        }
    }
    void handelRedirection()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.User usr = new BaseC.User(sConString);
        try
        {
            if (Session["irtrf"] == null)
            {
                // Response.Redirect("Login.aspx", false);
                Response.Redirect("/Login.aspx?Logout=1", false);
                return;
            }
            string output = "";

            //DataSet ds = usr.RedirectionHandler(common.myInt(Session["UserID"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "VALIDATE", common.myStr(Session["irtrf"]), out output, Page.Session.Timeout);
            DataSet ds = usr.RedirectionHandler(common.myInt(Session["UserID"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "VALIDATED", common.myStr(Session["irtrf"]), out output, Page.Session.Timeout);

            if (!output.Contains("Success"))
            {
                Response.Redirect("/Login.aspx?Logout=1&Status=" + output, false);
                return;
            }
            if (output.Contains("Success"))
            {
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {   // check for user validation for Employee Inactive -- Saten
                        if (common.myInt(ds.Tables[0].Rows[0]["UserId"]) > 0)
                        {

                            if (ds.Tables[5].Rows.Count > 0)
                            {
                                string folderpath = common.myStr(dl.ExecuteScalar(CommandType.Text, "selecT isnull(folderpath,'') From fileserversetup with(nolock) where facilityID=" + Session["FacilityID"] + ""));
                                if (folderpath != "")
                                {
                                    Cache["folderpath"] = folderpath;
                                }
                                else
                                {
                                    Cache["folderpath"] = Server.MapPath("/PatientDocuments/");
                                }
                                if (common.myStr(ds.Tables[5].Rows[0]["IsLocked"]) != "True")
                                {
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        //DateTime? dt = Convert.ToDateTime(ds.Tables[1].Rows[0]["today"]);
                                        DateTime? dt = Convert.ToDateTime(ds.Tables[1].Rows[0]["NextExpiryDate"]);
                                        DateTime dtToday = Convert.ToDateTime(ds.Tables[1].Rows[0]["today"]);
                                        Session["EmployeeName"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeName"]);
                                        Session["EmployeeFirstName"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeFirstName"]);
                                        Session["EMPDeptIdentity"] = common.myStr(ds.Tables[1].Rows[0]["DepartmentIdendification"]);
                                        Session["EMPisresource"] = common.myStr(ds.Tables[1].Rows[0]["isresource"]);
                                        Session["LoginEmployeeType"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeType"]);
                                        Session["IsUserPostEmail"] = common.myInt(ds.Tables[1].Rows[0]["IsUserPostEmail"]);
                                        Session["EmployeeTypeID"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeTypeId"]);
                                        Session["LoginDepartmentId"] = common.myStr(ds.Tables[1].Rows[0]["DepartmentId"]);
                                        Session["UserID"] = common.myStr(ds.Tables[0].Rows[0]["UserId"]);
                                        Session["EmployeeType"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeType"]);
                                        Session["isEMRSuperUser"] = common.myStr(ds.Tables[1].Rows[0]["isEMRSuperUser"]);
                                        Session["EnablePrintCaseSheet"] = common.myStr(ds.Tables[1].Rows[0]["UnablePrintCaseSheet"]);
                                        Session["CanDownloadPatientDocument"] = common.myInt(ds.Tables[1].Rows[0]["CanDownloadPatientDocument"]);

                                        int exppwd = common.myInt(ds.Tables[1].Rows[0]["NeverExpirePwd"]);
                                        if (dt != null)
                                        {
                                            //if (dtToday < dt || common.myBool(exppwd) == true)
                                            if (dtToday < dt)
                                            {
                                                if (exppwd == 0)
                                                {
                                                    int datediff = Convert.ToInt32(ds.Tables[1].Rows[0]["dateDifference"]);
                                                    int expiryNotification = Convert.ToInt32(ds.Tables[1].Rows[0]["PasswordExpiryNotification"]);
                                                    if (datediff <= expiryNotification && datediff >= 0)
                                                    {
                                                        if (datediff == 1)
                                                            Alert.ShowAjaxMsg("Your password will expire tomorrow. Please change your password", Page);
                                                        else if (datediff == 0)
                                                            Alert.ShowAjaxMsg("Your password will expire today. Please change your password", Page);
                                                        else
                                                            Alert.ShowAjaxMsg("Your password will expire in next " + datediff + " days. Please change your password", Page);

                                                    }
                                                }

                                                if (ds.Tables[2].Rows.Count > 0)
                                                {
                                                    DataView dvf = ds.Tables[2].DefaultView;
                                                    dvf.RowFilter = "FacilityId = " + common.myStr(Session["FacilityId"]);
                                                    if (dvf.Count > 0)
                                                    {
                                                        Session["FacilityName"] = common.myStr((dvf.ToTable()).Rows[0]["FacilityName"]);
                                                    }
                                                }

                                                if (ds.Tables[4].Rows.Count > 0)
                                                {
                                                    if (common.myStr(ds.Tables[4].Rows[0]["Pageurl"]) == "")
                                                    {
                                                        ViewState["pageurl"] = "Default.aspx";
                                                    }

                                                    else
                                                    {
                                                        //  ViewState["pageurl"] = common.myStr(ds.Tables[4].Rows[0]["Pageurl"]);
                                                        ViewState["pageurl"] = common.myStr(ds.Tables[4].Rows[0]["Pageurl"])
                                                      + "&OP=" + Session["IsAdminGroup"].ToString() + "_"
                                                  + Session["LoginIsAdminGroup"].ToString() + "_"
                                                  + Session["HospitalLocationID"].ToString() + "_"
                                                  + Session["FacilityID"].ToString() + "_"
                                                  + Session["GroupID"].ToString() + "_"
                                                  + Session["FinancialYearId"].ToString() + "_"
                                                  + Session["EntrySite"].ToString() + "_"
                                                  + Session["UserId"].ToString() + "_"
                                                  + Session["UserName"].ToString().Replace(" ", "%")
                                                  + "_" + common.myInt(Session["ModuleId"]).ToString() + "_0_" + Session["FacilityName"].ToString().Replace(" ", "%")
                                                  + "_" + Session["CanDownloadPatientDocument"].ToString()
                                                  + "_" + common.myInt(Session["FacilityStateId"]).ToString();

                                                        Session["StrO"] = null;
                                                        if (common.myStr(ds.Tables[4].Rows[0]["Pageurl"]).Split('?').Length > 1)
                                                            Session["StrO"] = ((common.myStr(ds.Tables[4].Rows[0]["Pageurl"]).Split('?'))[1]).Replace("irtrf=", "");
                                                        Session["DefaultID"] = common.myStr(ds.Tables[4].Rows[0]["ModuleID"]);
                                                    }
                                                }
                                                else
                                                {
                                                    ViewState["pageurl"] = null;
                                                }
                                                LoginRedir(ds);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else    // check for user validation for Employee Inactive -- Saten
                        {
                            Alert.ShowAjaxMsg("User login details are not valid. Please enter valid details.", Page.Page);
                            return;
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
            Response.Redirect("Login.aspx", false);
        }
        finally { dl = null; ; usr = null; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["InputDateFormat"] = "dd/MM/yyyy";
        Session["OutputDateFormat"] = "dd/MM/yyyy";
        Application["OutputDateFormat"] = "dd/MM/yyyy";

        if (!IsPostBack)
        {
            if (Request.QueryString["irtrf"] == null && Request.QueryString["Logout"] == null
                && common.myInt(Request.QueryString["Logout"]).ToString() != "1" && Session["UserID"] != null
                && common.myInt(Session["UserID"]) > 0 && Session["StrO"] != null)
            {
                Response.Redirect("Default.aspx?irtrf=" + common.myStr(Session["StrO"])
                    + "&OP=" + common.myStr(Session["IsAdminGroup"])
                    + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myInt(Session["HospitalLocationID"]).ToString()
                    + "_" + common.myInt(Session["FacilityID"]).ToString()
                    + "_" + common.myInt(Session["GroupID"]).ToString()
                    + "_" + common.myInt(Session["FinancialYearId"]).ToString()
                    + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myInt(Session["UserId"]).ToString()
                    + "_" + common.myStr(Session["UserName"]).Replace(" ", "%")
                    + "_" + common.myInt(Session["ModuleId"]).ToString() + "_0_" + common.myStr(Session["FacilityName"]).Replace(" ", "%")
                    + "_" + common.myStr(Session["CanDownloadPatientDocument"])
                    + "_" + common.myInt(Session["FacilityStateId"]).ToString(), false);
                return;
            }
            txtUserID.Focus();

            txtUserID.Attributes.Add("onkeypress", "return clickEnterKey('" + txtPassword.ClientID + "', event)");
            txtPassword.Attributes.Add("onkeypress", "return clickEnterKey('" + btnLogin.ClientID + "', event)");

            ClearApplicationCache();

            if (Request.QueryString["irtrf"] != null)
            {
                Session["irtrf"] = null;
                Session["irtrf"] = Request.QueryString["irtrf"];

                if (Request.QueryString["OP"] != null && common.myStr(Request.QueryString["OP"]).Split('_').Length > 10)
                {
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
                    //Session["EmployeeName"] = null;
                    //Session["EmployeeFirstName"] = null;
                    Session["irtrf"] = common.myStr(Request.QueryString["irtrf"]);
                    Session["IsAdminGroup"] = common.myStr(Request.QueryString["OP"]).Split('_')[0];
                    Session["LoginIsAdminGroup"] = common.myStr(Request.QueryString["OP"]).Split('_')[1];
                    Session["HospitalLocationID"] = common.myStr(Request.QueryString["OP"]).Split('_')[2];
                    Session["FacilityID"] = common.myStr(Request.QueryString["OP"]).Split('_')[3];
                    Session["GroupID"] = common.myStr(Request.QueryString["OP"]).Split('_')[4];
                    Session["FinancialYearId"] = common.myStr(Request.QueryString["OP"]).Split('_')[5];
                    Session["EntrySite"] = common.myStr(Request.QueryString["OP"]).Split('_')[6];
                    Session["UserID"] = common.myStr(Request.QueryString["OP"]).Split('_')[7];
                    Session["UserName"] = common.myStr(Request.QueryString["OP"]).Split('_')[8].Replace("%", " ");
                    Session["ModuleId"] = common.myStr(Request.QueryString["OP"]).Split('_')[9];
                    Session["URLPId"] = common.myStr(Request.QueryString["OP"]).Split('_')[10];


                    if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 11)
                        Session["FacilityName"] = common.myStr(Request.QueryString["OP"]).Split('_')[11].Replace("%", " ");
                    if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 12)
                        Session["CanDownloadPatientDocument"] = common.myStr(Request.QueryString["OP"]).Split('_')[12];
                    if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 13)
                        Session["FacilityStateId"] = common.myStr(Request.QueryString["OP"]).Split('_')[13];

                    handelRedirection();
                }
            }
            else
            {
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                FormsAuthentication.SignOut();
                ClearCache();
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
                Session["EmployeeName"] = null;
                Session["EmployeeFirstName"] = null;
            }

            //Page.SetFocus(txtUserID);

            //  ClearApplicationCache();
            if (Request.QueryString["Logout"] != null)
            {
                if (common.myStr(Request.QueryString["Logout"]) == "1")
                {
                    if (Session["StrO"] != null)
                    {
                        string output = "";
                        BaseC.User usr = new BaseC.User(sConString);
                        usr.RedirectionHandler(common.myInt(Session["UserID"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "Logout", common.myStr(Session["StrO"]), out output, 0);
                    }
                    //BaseC.Security objSecurity = new BaseC.Security(sConString);
                    //if (Session["HospitalLocationID"] != null && Session["RegistrationID"] != null)
                    //{
                    //    objSecurity.DeleteFiles(Server.MapPath("/PatientDocuments/") + common.myStr(Session["HospitalLocationID"]) + "/" + common.myStr(Session["RegistrationID"]));
                    //}
                    //objSecurity = null;
                    //  objSecurity.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), 0, 0, 172, 0, Convert.ToInt32(Session["UserID"]), 0, "LOGGEDOUT", Convert.ToString(Session["IPAddress"]));
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();
                    FormsAuthentication.SignOut();
                    ClearCache();
                    Response.Redirect("/Login.aspx");
                }
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
            }
            Cache.Remove("ModuleName" + Session["UserID"] + Session["HospitalLocationID"]);
            //}
            Cache.Remove("SubOtherModuleName" + Session["RegistrationID"] + Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);

            Cache.Remove("SubEHRModuleName" + Session["RegistrationID"] + Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);
            Session["PatientDetails"] = null;
            Cache.Remove("PatientDetails" + Session["RegistrationID"] + Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);

            //Session["URLPId"] = "";
            Session.Remove("ModuleDetail");
            Session.Remove("GroupDetail");
            Session.Remove("MenuDetail");
            Session.Remove("ModuleData");
            Session.Remove("MenuData");
            Session["MenuDetail"] = null;
            Session["GroupDetail"] = null;
            Session["ModuleDetail"] = null;
            Session["mainMenu"] = null;
            Session["MenuData"] = null;

            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));

            ViewState["Counter"] = 0;

            //PopulateYear(); ;
            fillFinancialyear();
            ShowBulletin();
            //ShowBulletin();
            //  tblNewsAndEvents.Visible = false;
        }
        if (Session["CurrentNode"] != null)
        {
            Session["CurrentNode"] = null;
        }
        ShowLogo();
    }
    public void ClearApplicationCache()
    {
        List<string> keys = new List<string>();
        // retrieve application Cache enumerator
        IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();
        // copy all keys that currently exist in Cache
        while (enumerator.MoveNext())
        {
            keys.Add(common.myStr(enumerator.Key));
        }
        // delete every key from cache
        for (int i = 0; i < keys.Count; i++)
        {
            HttpRuntime.Cache.Remove(keys[i]);
        }
    }

    protected void txtPassword_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt16(ViewState["Counter"]) < 10)
            {
                if (txtUserID.Text != "" && txtPassword.Text != "")
                {
                    string strPwd = txtPassword.Text.Trim();
                    BaseC.User valUser = new BaseC.User(sConString);
                    DataSet ds = new DataSet();
                    ds = valUser.ValidateUserName(txtUserID.Text, txtPassword.Text);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {   // check for user validation for Employee Inactive -- Saten
                            if (common.myInt(ds.Tables[0].Rows[0]["UserId"]) > 0)
                            {
                                if (ds.Tables[5].Rows.Count > 0)
                                {
                                    if (common.myStr(ds.Tables[5].Rows[0]["IsLocked"]) != "True")
                                    {
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {   // check for user validation for Employee Inactive -- Saten
                                            if (!common.myBool(ds.Tables[1].Rows[0]["Active"]))
                                            {
                                                Alert.ShowAjaxMsg("User login details are not valid. Please enter valid details.", Page.Page);
                                                return;
                                            }
                                            DateTime? dt = Convert.ToDateTime(ds.Tables[1].Rows[0]["NextExpiryDate"]);
                                            DateTime dtToday = Convert.ToDateTime(ds.Tables[1].Rows[0]["today"]);
                                            Session["EmployeeName"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeName"]);
                                            Session["EmployeeFirstName"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeFirstName"]);
                                            Session["CanDownloadPatientDocument"] = common.myInt(ds.Tables[1].Rows[0]["CanDownloadPatientDocument"]);
                                            Session["EMPDeptIdentity"] = common.myStr(ds.Tables[1].Rows[0]["DepartmentIdendification"]);
                                            Session["EMPisresource"] = common.myStr(ds.Tables[1].Rows[0]["isresource"]);
                                            Session["LoginEmployeeType"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeType"]);
                                            Session["EmployeeType"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeType"]);
                                            Session["IsUserPostEmail"] = common.myInt(ds.Tables[1].Rows[0]["IsUserPostEmail"]);
                                            Session["EmployeeTypeID"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeTypeId"]);
                                            Session["LoginDepartmentId"] = common.myStr(ds.Tables[1].Rows[0]["DepartmentId"]);
                                            Session["UserID"] = common.myStr(ds.Tables[0].Rows[0]["UserId"]);
                                            Session["isEMRSuperUser"] = common.myStr(ds.Tables[1].Rows[0]["isEMRSuperUser"]);
                                            Session["EnablePrintCaseSheet"] = common.myStr(ds.Tables[1].Rows[0]["UnablePrintCaseSheet"]);



                                            if (ds.Tables[5].Rows.Count > 0)
                                            {
                                                Session["EmployeeId"] = common.myStr(ds.Tables[5].Rows[0]["EmpID"]);
                                            }
                                            else
                                            {
                                                Session["EmployeeId"] = valUser.getEmployeeId(common.myInt(Session["UserID"]));
                                            }

                                            if (common.myStr(ds.Tables[1].Rows[0]["ShowExpWarning"]).Equals("Y"))
                                            {
                                                // Alert.ShowAjaxMsg("Your password will expire soon. Please contact system administrator.", Page.Page);
                                                string expiryMessage = GetLoginExpiryMessage(0, "Y");
                                                Alert.ShowAjaxMsg(expiryMessage, Page);

                                            }

                                            int exppwd = common.myInt(ds.Tables[1].Rows[0]["NeverExpirePwd"]);
                                            if (dt != null)
                                            {
                                                if (dtToday < dt)
                                                {
                                                    if (exppwd == 0)
                                                    {
                                                        int datediff = Convert.ToInt32(ds.Tables[1].Rows[0]["dateDifference"]);
                                                        int expiryNotification = Convert.ToInt32(ds.Tables[1].Rows[0]["PasswordExpiryNotification"]);
                                                        if (datediff <= expiryNotification && datediff >= 0)
                                                        {
                                                            if (datediff == 1)
                                                                Alert.ShowAjaxMsg("Your password will expire tomorrow. Please change your password", Page);
                                                            else if (datediff == 0)
                                                                Alert.ShowAjaxMsg("Your password will expire today. Please change your password", Page);
                                                            else
                                                                Alert.ShowAjaxMsg("Your password will expire in next " + datediff + " days. Please change your password", Page);


                                                        }
                                                    }
                                                    int facilityIdx = ddlFacility.SelectedIndex;
                                                    ddlFacility.Items.Clear();
                                                    ddlFacility.DataSource = ds.Tables[2];
                                                    ddlFacility.DataValueField = "FacilityId";
                                                    ddlFacility.DataTextField = "FacilityName";
                                                    ddlFacility.DataBind();
                                                    Session["FacilityforHeadOfficeMisReports"] = ds.Tables[2];
                                                    Session["FacilityType"] = common.myStr(ds.Tables[2].Rows[0]["FacilityType"]);
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (facilityIdx.Equals(-1))
                                                        {
                                                            ddlFacility.SelectedIndex = 0;
                                                        }
                                                        else
                                                        {
                                                            ddlFacility.SelectedIndex = facilityIdx;
                                                        }
                                                    }
                                                    ddlFacility_SelectedIndexChanged(null, null);
                                                    int GroupIdIdx = dropGroup.SelectedIndex;
                                                    dropGroup.Items.Clear();
                                                    foreach (DataRow dr in ds.Tables[3].Rows)
                                                    {
                                                        RadComboBoxItem item = new RadComboBoxItem();
                                                        item.Text = (string)dr["GroupName"];
                                                        item.Value = common.myStr(dr["GroupId"]);
                                                        item.Attributes.Add("IsAdminGroup", common.myStr(dr["Admin"]));
                                                        dropGroup.Items.Add(item);
                                                        item.DataBind();
                                                    }
                                                    if (ds.Tables[3].Rows.Count > 0)
                                                    {
                                                        if (GroupIdIdx.Equals(-1))
                                                        {
                                                            dropGroup.SelectedIndex = 0;
                                                        }
                                                        else
                                                        {
                                                            dropGroup.SelectedIndex = GroupIdIdx;
                                                        }
                                                    }

                                                    //if (ds.Tables[3].Rows.Count > 0)
                                                    //{
                                                    //    dropGroup.SelectedIndex = 0;
                                                    //}
                                                    //dropGroup.SelectedIndex = groupIdx;
                                                    if (ds.Tables[4].Rows.Count > 0)
                                                    {
                                                        ViewState["pageurl"] = common.myStr(ds.Tables[4].Rows[0]["Pageurl"]);
                                                        if ((common.myStr(ds.Tables[4].Rows[0]["Pageurl"]).Split('?')).Length > 1)
                                                            Session["StrO"] = ((common.myStr(ds.Tables[4].Rows[0]["Pageurl"]).Split('?'))[1]).Replace("irtrf=", "");
                                                        if (common.myStr(ds.Tables[4].Rows[0]["DefaultURI"]).ToUpper().Equals("FALSE")
                                                            || common.myStr(ds.Tables[4].Rows[0]["DefaultURI"]).Equals("0")
                                                            || !common.myBool(ds.Tables[4].Rows[0]["DefaultURI"])
                                                            || common.myInt(ds.Tables[4].Rows[0]["DefaultURI"]).Equals(0))
                                                        {
                                                            ViewState["URI"] = common.myStr(ds.Tables[4].Rows[0]["URI"]);
                                                        }
                                                        Session["DefaultID"] = common.myStr(ds.Tables[4].Rows[0]["ModuleID"]);
                                                    }
                                                    else
                                                    {
                                                        ViewState["pageurl"] = null;
                                                    }
                                                    if (ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        hdnHspId.Value = common.myStr(ds.Tables[1].Rows[0]["HospitalLocationId"]);
                                                    }

                                                    lblMessage.Text = "";
                                                    btnLogin.Focus();
                                                }
                                                else
                                                {
                                                    if (exppwd == 0)
                                                    {
                                                        lblMessage.Text = "Your password is expired. Please change your password to continue login";
                                                        ddlFacility.SelectedIndex = -1;
                                                        dropGroup.SelectedIndex = -1;
                                                    }
                                                    else
                                                    {
                                                        int facilityIdx = ddlFacility.SelectedIndex;
                                                        ddlFacility.Items.Clear();
                                                        ddlFacility.DataSource = ds.Tables[2];
                                                        ddlFacility.DataValueField = "FacilityId";
                                                        ddlFacility.DataTextField = "FacilityName";
                                                        ddlFacility.DataBind();
                                                        Session["FacilityforHeadOfficeMisReports"] = ds.Tables[2];
                                                        Session["FacilityType"] = common.myStr(ds.Tables[2].Rows[0]["FacilityType"]);
                                                        if (ds.Tables[0].Rows.Count > 0)
                                                        {
                                                            if (facilityIdx.Equals(-1))
                                                            {
                                                                ddlFacility.SelectedIndex = 0;
                                                            }
                                                            else
                                                            {
                                                                ddlFacility.SelectedIndex = facilityIdx;
                                                            }
                                                        }
                                                        ddlFacility_SelectedIndexChanged(null, null);
                                                        int GroupIdIdx = dropGroup.SelectedIndex;
                                                        dropGroup.Items.Clear();
                                                        foreach (DataRow dr in ds.Tables[3].Rows)
                                                        {
                                                            RadComboBoxItem item = new RadComboBoxItem();
                                                            item.Text = (string)dr["GroupName"];
                                                            item.Value = common.myStr(dr["GroupId"]);
                                                            item.Attributes.Add("IsAdminGroup", common.myStr(dr["Admin"]));
                                                            dropGroup.Items.Add(item);
                                                            item.DataBind();
                                                        }
                                                        if (ds.Tables[3].Rows.Count > 0)
                                                        {
                                                            if (GroupIdIdx.Equals(-1))
                                                            {
                                                                dropGroup.SelectedIndex = 0;
                                                            }
                                                            else
                                                            {
                                                                dropGroup.SelectedIndex = GroupIdIdx;
                                                            }
                                                        }
                                                        ////dropGroup.SelectedIndex = groupIdx;
                                                        //if (ds.Tables[0].Rows.Count > 0)
                                                        //{
                                                        //    dropGroup.SelectedIndex = 0;
                                                        //}
                                                        if (Request.QueryString["RefURL"] != null)
                                                        {
                                                            // Response.Redirect(Request.QueryString["RefURL"], true);
                                                        }
                                                        if (Request.QueryString["returnURL"] != null && !common.myStr(Request.QueryString["returnURL"]).Equals("/"))
                                                        {
                                                            //  Response.Redirect(Request.QueryString["returnURL"], false);
                                                        }
                                                        else
                                                        {
                                                            if (ds.Tables[4].Rows.Count > 0)
                                                            {
                                                                ViewState["pageurl"] = common.myStr(ds.Tables[4].Rows[0]["Pageurl"]);
                                                                //if (common.myBool(common.myStr(ds.Tables[4].Rows[0]["DefaultURI"])) == false)
                                                                if (!common.myBool(ds.Tables[4].Rows[0]["DefaultURI"])
                                                            || common.myStr(ds.Tables[4].Rows[0]["DefaultURI"]).Equals("0")
                                                            || !common.myBool(ds.Tables[4].Rows[0]["DefaultURI"])
                                                            || common.myInt(ds.Tables[4].Rows[0]["DefaultURI"]).Equals(0)
                                                            )
                                                                {
                                                                    ViewState["URI"] = common.myStr(ds.Tables[4].Rows[0]["URI"]);
                                                                }
                                                                Session["DefaultID"] = common.myStr(ds.Tables[4].Rows[0]["ModuleID"]);
                                                            }
                                                            else
                                                            {
                                                                ViewState["pageurl"] = null;
                                                            }
                                                        }
                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            hdnHspId.Value = common.myStr(ds.Tables[1].Rows[0]["HospitalLocationId"]);
                                                        }


                                                        lblMessage.Text = "";

                                                        btnLogin.Focus();
                                                    }

                                                }
                                            }
                                            ViewState["count"] = 0;
                                            string isDirectLoginforSingleFacilityNGroup = common.GetFlagValueHospitalSetup(common.myInt(hdnHspId.Value), common.myInt(Session["FacilityId"]), "isDirectLoginforSingleFacilityNGroup", sConString);
                                            if (ds.Tables[2].Rows.Count == 1 && ds.Tables[3].Rows.Count == 1 && ddlEntrySite.Items.Count == 1 && isDirectLoginforSingleFacilityNGroup.Equals("Y"))
                                            {
                                                ViewState["count"] = 1;
                                                btnLogin_OnClick(sender, e);
                                            }
                                        }
                                        else
                                        {
                                            lblMessage.Text = "Username/Password you entered is incorrect.";
                                            txtUserID.Focus();
                                            hdnHspId.Value = "";
                                            ddlFacility.Items.Clear();
                                            dropGroup.Items.Clear();
                                            if (ViewState["Counter"] != null)
                                            {
                                                if (Convert.ToInt16(ViewState["Counter"]) != 0)
                                                {
                                                    if (Convert.ToInt16(ViewState["Counter"]) >= 10)
                                                    {
                                                        if (LockUser(txtUserID.Text))
                                                        {
                                                            ViewState["Counter"] = "0";
                                                            lblMessage.Text = "Your account is blocked as you have reached maximum attempt limit. Contact your Administrator.";
                                                        }
                                                    }
                                                }
                                            }
                                            ViewState["Counter"] = Convert.ToInt16(ViewState["Counter"]) + 1;
                                        }

                                    }
                                    else
                                    {
                                        ViewState["Counter"] = "0";
                                        lblMessage.Text = "Your account is blocked. Contact your Administrator.";
                                    }
                                }
                            }
                            else   // check for user validation for Employee Inactive -- Saten

                                lblMessage.Text = "Username/Password you entered is incorrect";

                            txtPassword.Attributes.Add("value", strPwd);

                        }
                        else
                        {
                            lblMessage.Text = "Please Type User Name and Password";
                            txtUserID.Focus();
                        }
                    }
                    else
                    {
                        lblMessage.Text = "You can try maximum 3 times.";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            if (lblMessage.Text == "There is no row at position 0.")
                lblMessage.Text = "Username/Password you entered is incorrect";
        }
    }
    private void FillEntrySite()
    {
        try
        {
            int FacilityId;
            FacilityId = common.myInt(ddlFacility.SelectedValue);
            Session["FacilityID"] = common.myInt(ddlFacility.SelectedValue);
            BaseC.clsEMRBilling obj = new BaseC.clsEMRBilling(sConString);
            DataSet ds = obj.getEntrySiteForLogin(Convert.ToInt16(Session["UserID"]), FacilityId);
            int EntrySiteIdx = ddlEntrySite.SelectedIndex;
            ddlEntrySite.DataSource = ds.Tables[0];
            ddlEntrySite.DataValueField = "ESId";
            ddlEntrySite.DataTextField = "ESName";
            ddlEntrySite.DataBind();
            ddlEntrySite.SelectedIndex = EntrySiteIdx;
           
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            //objException.HandleException(Ex);
        }

    }
    protected void ddlFinancial_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["FinancialYearId"] = ddlFinancial.SelectedValue;
    }
    protected void btnLogin_OnClick(object sender, EventArgs e)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        BaseC.clsLISMaster objLisM = new BaseC.clsLISMaster(sConString);
        BaseC.User valUser = new BaseC.User(sConString);
        BaseC.Security AuditCA = new BaseC.Security(sConString);
        BaseC.EMRMasters objm = new BaseC.EMRMasters(sConString);
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            Session["MenuDetail"] = null;
            Session["GroupDetail"] = null;
            Session["ModuleDetail"] = null;
            Session["mainMenu"] = null;
            Session["MenuData"] = null;

            if (common.myInt(ViewState["count"]) == 1)
            {
                ViewState["VHospitalLocationID"] = Session["HospitalLocationID"];
                ViewState["VUserID"] = Session["UserID"];


                Session["URLPId"] = "";

                if (txtUserID.Text != "" && txtPassword.Text != "")
                {
                    Session["IPAddress"] = Request.ServerVariables["REMOTE_ADDR"];
                    if (valUser.ValidateUser(txtUserID.Text, txtPassword.Text, hdnHspId.Value))
                    {
                        if (ddlFacility.SelectedValue == "" || dropGroup.SelectedValue == "")
                        {
                            lblMessage.Text = "All fields are mandatory !!!";
                            return;
                        }

                        Session["SeniorCitizenAge"] = null;
                        Session["SeniorCitizenCompany"] = null;
                        Session["StaffCompanyId"] = null;
                        Session["StaffDependentCompanyId"] = null;
                        Session["DefaultHospitalCompanyId"] = null;
                        Session["DefaultOPDCategory"] = null;


                        Cache.Remove("SeniorCitizenAge");
                        Cache.Remove("SeniorCitizenCompany");
                        Cache.Remove("StaffCompanyId");
                        Cache.Remove("StaffDependentCompanyId");
                        Cache.Remove("DefaultHospitalCompanyId");
                        Cache.Remove("DecimalPlace");
                        Cache.Remove("PrintClaimXML");
                        Cache.Remove("PrintAuthentication");
                        Cache["PrintClaimXML"] = "";
                        Cache["SeniorCitizenAge"] = "";
                        Cache["SeniorCitizenCompany"] = "";
                        Cache["StaffCompanyId"] = "";
                        Cache["StaffDependentCompanyId"] = "";
                        Cache["DefaultHospitalCompanyId"] = "";
                        Cache["DecimalPlace"] = "";
                        Cache["DefaultOPDCategory"] = "";
                        Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));

                        Cache.Remove("ModuleName" + Session["UserID"] + Session["HospitalLocationID"]);

                        Cache.Remove("SubOtherModuleName" + HttpContext.Current.Session["RegistrationID"] + HttpContext.Current.Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);

                        Cache.Remove("SubEHRModuleName" + HttpContext.Current.Session["RegistrationID"] + HttpContext.Current.Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);
                        Cache.Remove("PatientDetails" + HttpContext.Current.Session["RegistrationID"] + HttpContext.Current.Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);

                        FormsAuthentication.RedirectFromLoginPage(txtUserID.Text, true);
                        Session["HospitalLocationID"] = hdnHspId.Value;

                        //Session["UserID"] = valUser.GetUserID(txtUserID.Text);
                        Session["UserName"] = common.myStr(txtUserID.Text);
                        Session["FacilityID"] = ddlFacility.SelectedValue;
                        Session["FacilityName"] = common.myStr(ddlFacility.SelectedItem.Text);
                        Session["GroupID"] = dropGroup.SelectedValue;
                        Session["EmployeeId"] = valUser.getEmployeeId(common.myInt(Session["UserID"]));
                        Session["FinancialYearId"] = ddlFinancial.SelectedValue;
                        Session["IsAdminGroup"] = common.myStr(dropGroup.SelectedItem.Attributes["IsAdminGroup"]);
                        Session["EntrySite"] = ddlEntrySite.SelectedValue;


                        DataSet dsEmpType = valUser.getEmployeeTypePermission(common.myInt(Session["EmployeeId"]), common.myInt(hdnHspId.Value), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]));
                        if (dsEmpType.Tables[0].Rows.Count > 0)
                        {
                            Session["EmployeeTypePermission"] = Convert.ToBoolean(dsEmpType.Tables[0].Rows[0]["Admin"]);
                            Session["EmployeeType"] = common.myStr(dsEmpType.Tables[0].Rows[0]["EmployeeType"]).Trim();
                            Session["iDefaultDoctorTemplate"] = common.myInt(dsEmpType.Tables[0].Rows[0]["iDefaultDoctorTemplate"]);
                            Session["ProvidingService"] = common.myStr(dsEmpType.Tables[0].Rows[0]["ProvidingService"]).Trim();
                        }

                        if (HttpContext.Current.Cache["HospitalSetup"] == null)
                        {
                            Hashtable hshIn = new Hashtable();
                            hshIn.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                            hshIn.Add("@iFacilityId", common.myInt(Session["FacilityId"]));
                            DataSet dsHs = new DataSet();
                            dsHs = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);
                            Cache.Insert("HospitalSetup", dsHs.Tables[0], null, DateTime.Now.AddHours(240), System.Web.Caching.Cache.NoSlidingExpiration);
                        }

                        Session["IsIVFSpecialisation"] = false;

                        Application["HospitalName"] = emr.BindApplicationName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]));
                        Page.Title = common.myStr(Application["HospitalName"]);

                        Session["UserSpecialisationId"] = "0";
                        clsIVF objivf = new clsIVF(sConString);
                        DataSet SetupDoctor = objivf.getUserSetupDoctor(common.myInt(Session["EmployeeId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
                        if (SetupDoctor.Tables[0].Rows.Count > 0)
                        {
                            Session["UserSpecialisationId"] = common.myInt(SetupDoctor.Tables[0].Rows[0]["SpecialisationId"]);

                            Session["IsIVFSpecialisation"] = common.myBool(SetupDoctor.Tables[0].Rows[0]["IsIVFSpecialisation"]);
                        }

                        DataTable tblD = objLisM.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityID"]), common.myInt(Session["UserID"]));

                        Session["LoginDoctorId"] = null;
                        Session["IsLoginDoctor"] = false;
                        Session["LoginIsAdminGroup"] = "0";
                        if (common.myStr(dropGroup.SelectedItem.Attributes["IsAdminGroup"]) == "True" || common.myStr(dropGroup.SelectedItem.Attributes["IsAdminGroup"]) == "1")
                            Session["LoginIsAdminGroup"] = "1";
                        if (tblD.Rows.Count > 0)
                        {
                            Session["LoginDoctorId"] = common.myInt(tblD.Rows[0]["DoctorId"]);
                            Session["IsLoginDoctor"] = common.myBool(tblD.Rows[0]["IsDoctor"]);
                        }

                        Session["MainFacility"] = common.myStr(objPatient.getMainFacilityId(common.myInt(ddlFacility.SelectedValue)));

                        fillHospitalDefaultValue();
                        if (!common.myStr(ViewState["pageurl"]).Equals("") || !common.myStr(ViewState["URI"]).Equals(""))
                        {
                            string strurl = "";
                            strurl = common.myStr(ViewState["pageurl"]); //(string)dL.ExecuteScalar(CommandType.Text, "SELECT pageurl FROM secmodulepages where Password=@Password AND HospitalLocationId=@HospitalLocationId", hshInput);
                            if (common.myStr(ViewState["URI"]) != "")
                            {
                                BaseC.User usr = new User(sConString);
                                string StrO = "";
                                usr.RedirectionHandler(common.myInt(Session["UserId"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "START", "", out StrO);
                                if (StrO.Length > 0)
                                {
                                    if (!common.myStr(Request.Url.GetLeftPart(UriPartial.Path)).Contains(common.myStr(ViewState["URI"])))
                                    {
                                        string REdirectURI = common.myStr(ViewState["URI"]);
                                        Session["StrO"] = StrO;
                                        REdirectURI = REdirectURI + "?irtrf=" + StrO;

                                        REdirectURI = REdirectURI + "&OP="
                                            + Session["IsAdminGroup"].ToString() + "_"
                                            + Session["LoginIsAdminGroup"].ToString() + "_"
                                            + common.myInt(Session["HospitalLocationID"]).ToString() + "_"
                                            + common.myInt(Session["FacilityID"]).ToString() + "_"
                                            + common.myInt(Session["GroupID"]).ToString() + "_"
                                            + common.myInt(Session["FinancialYearId"]).ToString() + "_"
                                            + Session["EntrySite"].ToString() + "_"
                                            + common.myInt(Session["UserId"]).ToString() + "_"
                                            + Session["UserName"].ToString().Replace(" ", "%")
                                            + "_0_0_" + Session["FacilityName"].ToString().Replace(" ", "%")
                                            + "_" + Session["CanDownloadPatientDocument"].ToString()
                                            + "_" + common.myInt(Session["FacilityStateId"]).ToString();
                                        Response.Redirect(REdirectURI, false);
                                    }
                                    else
                                    {
                                        Response.Redirect("default.aspx", false);
                                    }
                                }
                                else
                                {
                                    lblMessage.Text = "Oops!Error ";
                                }
                            }
                            else
                            {
                                Response.Redirect(strurl, false);
                            }
                        }
                        else
                        {
                            Response.Redirect("default.aspx", false);
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Username/Password you entered is incorrect";
                        txtPassword.Focus();
                    }
                }
                else
                {
                    lblMessage.Text = "Please Type User Name and Password";
                    txtUserID.Focus();
                }
            }
            else
            {
                if (IsValid)
                {

                    ViewState["VHospitalLocationID"] = Session["HospitalLocationID"];
                    ViewState["VUserID"] = Session["UserID"];


                    if (txtUserID.Text != "" && txtPassword.Text != "")
                    {
                        Session["IPAddress"] = Request.ServerVariables["REMOTE_ADDR"];
                        if (valUser.ValidateUser(txtUserID.Text, txtPassword.Text, hdnHspId.Value))
                        {
                            if (ddlFacility.SelectedValue == "" || dropGroup.SelectedValue == "")
                            {
                                lblMessage.Text = "All fields are mandatory !!!";
                                return;
                            }

                            Session["SeniorCitizenAge"] = null;
                            Session["SeniorCitizenCompany"] = null;
                            Session["StaffCompanyId"] = null;
                            Session["StaffDependentCompanyId"] = null;
                            Session["DefaultHospitalCompanyId"] = null;
                            Session["DefaultOPDCategory"] = null;


                            Cache.Remove("SeniorCitizenAge");
                            Cache.Remove("SeniorCitizenCompany");
                            Cache.Remove("StaffCompanyId");
                            Cache.Remove("StaffDependentCompanyId");
                            Cache.Remove("DefaultHospitalCompanyId");
                            Cache.Remove("DecimalPlace");
                            Cache.Remove("PrintClaimXML");
                            Cache.Remove("PrintAuthentication");
                            Cache["PrintClaimXML"] = "";
                            Cache["SeniorCitizenAge"] = "";
                            Cache["SeniorCitizenCompany"] = "";
                            Cache["StaffCompanyId"] = "";
                            Cache["StaffDependentCompanyId"] = "";
                            Cache["DefaultHospitalCompanyId"] = "";
                            Cache["DecimalPlace"] = "";
                            Cache["DefaultOPDCategory"] = "";

                            Cache.Remove("ModuleName" + Session["UserID"] + Session["HospitalLocationID"]);

                            Cache.Remove("SubOtherModuleName" + HttpContext.Current.Session["RegistrationID"] + HttpContext.Current.Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);

                            Cache.Remove("SubEHRModuleName" + HttpContext.Current.Session["RegistrationID"] + HttpContext.Current.Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);
                            Cache.Remove("PatientDetails" + HttpContext.Current.Session["RegistrationID"] + HttpContext.Current.Session["encounterid"] + Session["UserID"] + Session["HospitalLocationID"]);

                            FormsAuthentication.RedirectFromLoginPage(txtUserID.Text, true);
                            Session["HospitalLocationID"] = hdnHspId.Value;

                            //Session["UserID"] = valUser.GetUserID(txtUserID.Text);
                            Session["UserName"] = common.myStr(txtUserID.Text);
                            Session["FacilityID"] = ddlFacility.SelectedValue;
                            Session["FacilityName"] = common.myStr(ddlFacility.SelectedItem.Text);
                            Session["GroupID"] = dropGroup.SelectedValue;
                            Session["EmployeeId"] = valUser.getEmployeeId(common.myInt(Session["UserID"]));
                            Session["FinancialYearId"] = ddlFinancial.SelectedValue;
                            Session["IsAdminGroup"] = common.myStr(dropGroup.SelectedItem.Attributes["IsAdminGroup"]);
                            Session["EntrySite"] = ddlEntrySite.SelectedValue;

                            DataSet dsEmpType = valUser.getEmployeeTypePermission(common.myInt(Session["EmployeeId"]), common.myInt(hdnHspId.Value), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]));
                            if (dsEmpType.Tables[0].Rows.Count > 0)
                            {
                                Session["EmployeeTypePermission"] = Convert.ToBoolean(dsEmpType.Tables[0].Rows[0]["Admin"]);
                                Session["EmployeeType"] = common.myStr(dsEmpType.Tables[0].Rows[0]["EmployeeType"]).Trim();
                                Session["iDefaultDoctorTemplate"] = common.myInt(dsEmpType.Tables[0].Rows[0]["iDefaultDoctorTemplate"]);
                               // Session["ProvidingService"] = common.myStr(dsEmpType.Tables[0].Rows[0]["ProvidingService"]).Trim();
                            }

                            if (HttpContext.Current.Cache["HospitalSetup"] == null)
                            {
                                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                                Hashtable hshIn = new Hashtable();
                                hshIn.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                                hshIn.Add("@iFacilityId", common.myInt(Session["FacilityId"]));
                                DataSet dsHs = new DataSet();
                                dsHs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);
                                Cache.Insert("HospitalSetup", dsHs.Tables[0], null, DateTime.Now.AddHours(240), System.Web.Caching.Cache.NoSlidingExpiration);
                            }
                            fillHospitalDefaultValue();

                            Session["IsIVFSpecialisation"] = false;

                            Application["HospitalName"] = emr.BindApplicationName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]));
                            Page.Title = common.myStr(Application["HospitalName"]);

                            Session["UserSpecialisationId"] = "0";
                            clsIVF objivf = new clsIVF(sConString);
                            DataSet SetupDoctor = objivf.getUserSetupDoctor(common.myInt(Session["EmployeeId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
                            if (SetupDoctor.Tables[0].Rows.Count > 0)
                            {
                                Session["UserSpecialisationId"] = common.myInt(SetupDoctor.Tables[0].Rows[0]["SpecialisationId"]);

                                Session["IsIVFSpecialisation"] = common.myBool(SetupDoctor.Tables[0].Rows[0]["IsIVFSpecialisation"]);
                            }

                            DataTable tblD = objLisM.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityID"]), common.myInt(Session["UserID"]));

                            Session["LoginDoctorId"] = null;
                            Session["IsLoginDoctor"] = false;
                            Session["LoginIsAdminGroup"] = "0";
                            if (common.myStr(dropGroup.SelectedItem.Attributes["IsAdminGroup"]) == "True" || common.myStr(dropGroup.SelectedItem.Attributes["IsAdminGroup"]) == "1")
                                Session["LoginIsAdminGroup"] = "1";
                            if (tblD.Rows.Count > 0)
                            {
                                Session["LoginDoctorId"] = common.myInt(tblD.Rows[0]["DoctorId"]);
                                Session["IsLoginDoctor"] = common.myBool(tblD.Rows[0]["IsDoctor"]);
                            }
                            Session["MainFacility"] = common.myStr(objPatient.getMainFacilityId(common.myInt(ddlFacility.SelectedValue)));
                            if (Request.QueryString["RefURL"] != null)
                            {
                                //  Response.Redirect(Request.QueryString["RefURL"], true);
                            }
                            if (Request.QueryString["returnURL"] != null && !common.myStr(Request.QueryString["returnURL"]).Equals("/"))
                            {
                                //   Response.Redirect(Request.QueryString["returnURL"], false);
                            }
                            else
                            {
                                if (!common.myStr(ViewState["pageurl"]).Equals("") || !common.myStr(ViewState["URI"]).Equals(""))
                                {
                                    string strurl = "";
                                    strurl = common.myStr(ViewState["pageurl"]); //(string)dL.ExecuteScalar(CommandType.Text, "SELECT pageurl FROM secmodulepages where Password=@Password AND HospitalLocationId=@HospitalLocationId", hshInput);
                                    if (common.myStr(ViewState["URI"]) != "")
                                    {
                                        BaseC.User usr = new User(sConString);
                                        string StrO = "";
                                        usr.RedirectionHandler(common.myInt(Session["UserId"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "START", "", out StrO, Page.Session.Timeout);
                                        if (StrO.Length > 0)
                                        {
                                            Session["StrO"] = StrO;
                                            string REdirectURI = common.myStr(ViewState["URI"]);
                                            REdirectURI = REdirectURI + "?irtrf=" + StrO;

                                            REdirectURI = REdirectURI + "&OP="
                        + Session["IsAdminGroup"].ToString() + "_"
                        + Session["LoginIsAdminGroup"].ToString() + "_"
                        + common.myInt(Session["HospitalLocationID"]).ToString() + "_"
                        + common.myInt(Session["FacilityID"]).ToString() + "_"
                        //+ Session["FacilityName"].ToString() + "_" 
                        + common.myInt(Session["GroupID"]).ToString() + "_"
                        + common.myInt(Session["FinancialYearId"]).ToString() + "_"
                        //+ Session["IsAdminGroup"].ToString() + "_" 
                        + Session["EntrySite"].ToString() + "_"
                        + common.myInt(Session["UserId"]).ToString() + "_"
                        //+ Session["CanDownloadPatientDocument"].ToString() + "_" 
                        + Session["UserName"].ToString().Replace(" ", "%")
                        + "_0_0_" + Session["FacilityName"].ToString().Replace(" ", "%")
                        + "_" + Session["CanDownloadPatientDocument"].ToString()
                        + "_" + common.myInt(Session["FacilityStateId"]).ToString();
                                            Response.Redirect(REdirectURI, false);
                                        }
                                        else
                                        {
                                            lblMessage.Text = "Oops!Error ";
                                        }
                                    }
                                    else
                                    {
                                        /*strurl = strurl //+ "&OP=" + common.myStr(Session["IsAdminGroup"]) + "_" + common.myStr(Session["LoginIsAdminGroup"]) + "_" + common.myStr(Session["HospitalLocationID"]) + "_" + common.myStr(Session["FacilityID"]) + "_" + common.myStr(Session["GroupID"]) + "_" + common.myStr(Session["FinancialYearId"]) + "_" + common.myStr(Session["EntrySite"]) + "_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["UserName"]);
                                            + "&OP=" + Session["IsAdminGroup"].ToString() + "_"
                                        + Session["LoginIsAdminGroup"].ToString() + "_"
                                        + Session["HospitalLocationID"].ToString() + "_"
                                        + Session["FacilityID"].ToString() + "_"
                                        + Session["GroupID"].ToString() + "_"
                                        + Session["FinancialYearId"].ToString() + "_"
                                        + Session["EntrySite"].ToString() + "_"
                                        + Session["UserId"].ToString() + "_"
                                        + Session["UserName"].ToString().Replace(" ", "%")
                                        + "_0_0_" + Session["FacilityName"].ToString().Replace(" ", "%")
                                        + "_" + Session["CanDownloadPatientDocument"].ToString()
                                        + "_" + common.myInt(Session["FacilityStateId"]).ToString();*/

                                        Response.Redirect(strurl, false);
                                    }

                                }
                                else
                                {
                                    Response.Redirect("default.aspx", false);
                                }
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Username/Password you entered is incorrect";
                            txtPassword.Focus();
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Please Type User Name and Password";
                        txtUserID.Focus();
                    }

                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            valUser = null;
            AuditCA = null;
            objm = null;
            objPatient = null;
            objLisM = null;
            objDl = null;
            ds.Dispose();
        }
    }
    protected void btnReset_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected bool LockUser(string userid)
    {
        BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
        DAL.DAL dL = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hsInput = new Hashtable();
        hsInput.Add("UserName", en.Encrypt(userid, en.getKey(sConString), true, ""));

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        string sQ = "update Users set IsLocked = 1 where username = @UserName";

        int i = objDl.ExecuteNonQuery(CommandType.Text, sQ, hsInput);
        if (i == 0)
            return true;
        else
            return false;
    }

    protected void fillFinancialyear()
    {
        ddlFinancial.ClearSelection();
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet ds = objPharmacy.GetFinancialyear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlFinancial.DataSource = ds;
            ddlFinancial.DataTextField = "Date";
            ddlFinancial.DataValueField = "YearId";
            ddlFinancial.DataBind();
            Session["FinancialYearId"] = common.myStr(ds.Tables[0].Rows[0]["YearId"]).Trim();
        }
    }

    private void ShowBulletin()
    {
        //    string sBulletin;
        //    DataSet ds = new DataSet();
        //    wcf_Service_SuperAdmin.ServiceClient objSuperAdmin = new wcf_Service_SuperAdmin.ServiceClient();

        //    ds = objSuperAdmin.GetBulletin();
        //    sBulletin = "<table cellpadding=\"1\">";
        //    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //    {
        //        sBulletin += "<tr><td valign=\"top\" style=\"height: 50px\">" + ds.Tables[0].Rows[i]["Description"].ToString() + "</td></tr><tr><td>&nbsp;</td></tr>";
        //    }
        //    sBulletin = sBulletin + "</table>";
        //    sBulletin = "<marquee width='300'height='400' style='cursor:hand' onmouseover='this.stop()' onmouseout='this.start()' id='Marquee1_in' name='Marquee1_in' scrollAmount=1 scrollDelay=5 behavior=\"scroll\" direction=\"up\">" + sBulletin + "</table>";
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        lblBulletin.Text = sBulletin;
        //    }
    }


    private void ShowLogo()
    {
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        string strLogo = common.myStr(objCommon.getHospitalLogo(common.myInt(Session["HospitalLocationId"])));

        Session["imgHospitalLogo"] = common.myStr(strLogo);
        if (strLogo != "")
        {
            imgHospitalLogo.ImageUrl = "~/Images/Logo/" + strLogo;
        }
        else
        {
            imgHospitalLogo.ImageUrl = "~/Images/Logo/logonotavailable.jpeg";
        }

    }

    private void fillHospitalDefaultValue()
    {
        DataView dv = null;
        DataSet ds = new DataSet();
        DataTable dtHsetup = null; //= new DataTable();

        System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

        collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                "DecimalPlaces,DefaultCaseSheetView,DefaultCurrency,DefaultHospitalCompany,DefaultOPDCategoryService,DisplayEnteredByInCaseSheet,IsInPatientIncluded,IsLISIncluded,IsSAPOrInterfaceEnabled,MUDMeasure,NewIpVisitPageEnabled,PrintClaimXML,RegistrationLabelName,SeniorCitizenAge,SeniorCitizenCompany,StaffCompanyId,StaffDependentCompanyId", sConString);

        string DecimalPlaces = string.Empty, DefaultCaseSheetView = string.Empty, DefaultCurrency = string.Empty, DefaultHospitalCompany = string.Empty, DefaultOPDCategoryService = string.Empty, DisplayEnteredByInCaseSheet = string.Empty, IsInPatientIncluded = string.Empty, IsLISIncluded = string.Empty, IsSAPOrInterfaceEnabled = string.Empty, MUDMeasure = string.Empty, NewIpVisitPageEnabled = string.Empty, PrintClaimXML = string.Empty, RegistrationLabelName = string.Empty, SeniorCitizenAge = string.Empty, SeniorCitizenCompany = string.Empty, StaffCompanyId = string.Empty, StaffDependentCompanyId = string.Empty;

        if (collHospitalSetupValues.ContainsKey("DecimalPlaces"))
            DecimalPlaces = collHospitalSetupValues["DecimalPlaces"];
        if (collHospitalSetupValues.ContainsKey("DefaultCaseSheetView"))
            DefaultCaseSheetView = collHospitalSetupValues["DefaultCaseSheetView"];
        if (collHospitalSetupValues.ContainsKey("DefaultCurrency"))
            DefaultCurrency = collHospitalSetupValues["DefaultCurrency"];
        if (collHospitalSetupValues.ContainsKey("DefaultHospitalCompany"))
            DefaultHospitalCompany = collHospitalSetupValues["DefaultHospitalCompany"];
        if (collHospitalSetupValues.ContainsKey("DefaultOPDCategoryService"))
            DefaultOPDCategoryService = collHospitalSetupValues["DefaultOPDCategoryService"];
        if (collHospitalSetupValues.ContainsKey("DisplayEnteredByInCaseSheet"))
            DisplayEnteredByInCaseSheet = collHospitalSetupValues["DisplayEnteredByInCaseSheet"];
        if (collHospitalSetupValues.ContainsKey("IsInPatientIncluded"))
            IsInPatientIncluded = collHospitalSetupValues["IsInPatientIncluded"];
        if (collHospitalSetupValues.ContainsKey("IsLISIncluded"))
            IsLISIncluded = collHospitalSetupValues["IsLISIncluded"];
        if (collHospitalSetupValues.ContainsKey("IsSAPOrInterfaceEnabled"))
            IsSAPOrInterfaceEnabled = collHospitalSetupValues["IsSAPOrInterfaceEnabled"];
        if (collHospitalSetupValues.ContainsKey("MUDMeasure"))
            MUDMeasure = collHospitalSetupValues["MUDMeasure"];
        if (collHospitalSetupValues.ContainsKey("NewIpVisitPageEnabled"))
            NewIpVisitPageEnabled = collHospitalSetupValues["NewIpVisitPageEnabled"];
        if (collHospitalSetupValues.ContainsKey("PrintClaimXML"))
            PrintClaimXML = collHospitalSetupValues["PrintClaimXML"];
        if (collHospitalSetupValues.ContainsKey("RegistrationLabelName"))
            RegistrationLabelName = collHospitalSetupValues["RegistrationLabelName"];
        if (collHospitalSetupValues.ContainsKey("SeniorCitizenAge"))
            SeniorCitizenAge = collHospitalSetupValues["SeniorCitizenAge"];
        if (collHospitalSetupValues.ContainsKey("SeniorCitizenCompany"))
            SeniorCitizenCompany = collHospitalSetupValues["SeniorCitizenCompany"];
        if (collHospitalSetupValues.ContainsKey("StaffCompanyId"))
            StaffCompanyId = collHospitalSetupValues["StaffCompanyId"];
        if (collHospitalSetupValues.ContainsKey("StaffDependentCompanyId"))
            StaffDependentCompanyId = collHospitalSetupValues["StaffDependentCompanyId"];

        collHospitalSetupValues = null;

        if (((DataTable)HttpContext.Current.Cache["HospitalSetup"]).Rows.Count > 0)
        {
            dtHsetup = (DataTable)HttpContext.Current.Cache["HospitalSetup"];
        }
        if (dtHsetup == null)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            hshIn.Add("@iFacilityId", common.myInt(Session["FacilityId"]));
            DataSet dsHs = new DataSet();
            dsHs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);
            Cache.Insert("HospitalSetup", dsHs.Tables[0], null, DateTime.Now.AddHours(240), System.Web.Caching.Cache.NoSlidingExpiration);
            dtHsetup = dsHs.Tables[0];
            dsHs.Dispose();
            hshIn = null;
            dl = null;
        }
        dtHsetup.DefaultView.RowFilter = "";
        ds.Tables.Add(dtHsetup.Copy());
        dv = ds.Tables[0].DefaultView;

        dv.RowFilter = "";
        dv.RowFilter = "Flag = 'RegistrationLabelName'";
        string strName = "UHID";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            strName = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Session["RegistrationLabelName"] = strName;
        }
        else
        {
            strName = RegistrationLabelName;
            Session["RegistrationLabelName"] = strName;
        }

        //XmlDocument loResource = new XmlDocument();
        //loResource.Load(Server.MapPath("/App_GlobalResources/PRegistration.resx"));

        //XmlNode loRoot = loResource.SelectSingleNode("root/data[@name='UHID']/value");
        //if (loRoot != null)
        //{
        //    loRoot.InnerText = strName;
        //    loResource.Save(Server.MapPath("/App_GlobalResources/PRegistration.resx"));
        //}
        //XmlNode loRoot1 = loResource.SelectSingleNode("root/data[@name='regno']/value");
        //if (loRoot1 != null)
        //{
        //    loRoot1.InnerText = strName;
        //    loResource.Save(Server.MapPath("/App_GlobalResources/PRegistration.resx"));
        //}

        dv.RowFilter = "";
        dv.RowFilter = "Flag = 'SeniorCitizenAge'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Session["SeniorCitizenAge"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Cache["SeniorCitizenAge"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Session["SeniorCitizenAge"] = SeniorCitizenAge;
            Cache["SeniorCitizenAge"] = common.myStr(Session["SeniorCitizenAge"]);
        }

        dv.RowFilter = "Flag = 'SeniorCitizenCompany'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Session["SeniorCitizenCompany"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Cache["SeniorCitizenCompany"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Session["SeniorCitizenCompany"] = SeniorCitizenCompany;
            Cache["SeniorCitizenCompany"] = common.myStr(Session["SeniorCitizenCompany"]);
        }

        dv.RowFilter = "Flag = 'StaffCompanyId'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Session["StaffCompanyId"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Cache["StaffCompanyId"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Session["StaffCompanyId"] = StaffCompanyId;
            Cache["StaffCompanyId"] = common.myStr(Session["StaffCompanyId"]);
        }

        dv.RowFilter = "Flag = 'StaffDependentCompanyId'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Session["StaffDependentCompanyId"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Cache["StaffDependentCompanyId"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Session["StaffDependentCompanyId"] = StaffCompanyId;
            Cache["StaffDependentCompanyId"] = common.myStr(Session["StaffDependentCompanyId"]);
        }

        dv.RowFilter = "Flag = 'DefaultHospitalCompany'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Session["DefaultHospitalCompanyId"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Cache["DefaultHospitalCompanyId"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Session["DefaultCompany"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Session["DefaultHospitalCompanyId"] = DefaultHospitalCompany;
            Cache["DefaultHospitalCompanyId"] = common.myStr(Session["DefaultHospitalCompanyId"]);
            Session["DefaultCompany"] = common.myStr(Session["DefaultHospitalCompanyId"]);
        }

        dv.RowFilter = "Flag = 'DefaultOPDCategoryService'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Session["DefaultOPDCategory"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Cache["DefaultOPDCategory"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Session["DefaultOPDCategory"] = DefaultOPDCategoryService;
            Cache["DefaultOPDCategory"] = common.myStr(Session["DefaultOPDCategory"]);
        }

        dv.RowFilter = "Flag = 'DecimalPlaces'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Session["DecimalPlace"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Cache["DecimalPlace"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Session["DecimalPlace"] = DecimalPlaces;
            Cache["DecimalPlace"] = common.myStr(Session["DecimalPlace"]);
        }

        dv.RowFilter = "Flag = 'NewIpVisitPageEnabled'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Session["NewIpVisitPageEnabled"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Session["NewIpVisitPageEnabled"] = NewIpVisitPageEnabled;
        }


        dv.RowFilter = "Flag = 'PrintClaimXML'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Cache["PrintClaimXML"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Cache["PrintClaimXML"] = PrintClaimXML;
        }

        dv.RowFilter = "Flag = 'DefaultCurrency'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Cache["DefaultCurrency"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Session["DefaultCurrency"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Cache["DefaultCurrency"] = DefaultCurrency;
            Session["DefaultCurrency"] = DefaultCurrency;
        }

        dv.RowFilter = "Flag = 'IsInPatientIncluded'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Application["IPIncluded"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Application["IPIncluded"] = IsInPatientIncluded;
        }

        dv.RowFilter = "Flag = 'IsLISIncluded'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Application["LISIncluded"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Application["LISIncluded"] = IsLISIncluded;
        }

        dv.RowFilter = "Flag = 'DefaultCaseSheetView'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Session["DefaultCaseSheetView"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Session["DefaultCaseSheetView"] = DefaultCaseSheetView;
        }

        dv.RowFilter = "Flag = 'DisplayEnteredByInCaseSheet'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Session["DisplayEnteredByInCaseSheet"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Session["DisplayEnteredByInCaseSheet"] = DisplayEnteredByInCaseSheet;
        }
        dv.RowFilter = "Flag = 'IsSAPOrInterfaceEnabled'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            Application["IsSAPOrInterfaceEnabled"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
            Session["IsSAPOrInterfaceEnabled"] = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            Application["IsSAPOrInterfaceEnabled"] = IsSAPOrInterfaceEnabled;
            Session["IsSAPOrInterfaceEnabled"] = IsSAPOrInterfaceEnabled;
        }

        string strMUDMeasureFlag = "";
        dv.RowFilter = "Flag = 'MUDMeasure'";
        if (dv.ToTable().Rows.Count > 0 && !common.myStr(dv.ToTable().Rows[0]["Value"]).Equals(""))
        {
            strMUDMeasureFlag = common.myStr(dv.ToTable().Rows[0]["Value"]);
        }
        else
        {
            strMUDMeasureFlag = MUDMeasure;
        }
        Session["MUDMeasure"] = strMUDMeasureFlag.Trim() != "" ? Convert.ToBoolean(strMUDMeasureFlag) : false;
        dv.RowFilter = "";


        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
        if (dsInterface.Tables.Count > 0)
        {
            if (dsInterface.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(dsInterface.Tables[0].Rows[0]["InterfaceForEMRDrugOrder"]))
                {
                    if (common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]))
                    {
                        Session["IsCIMSInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]);
                        Session["CIMSDatabasePath"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                        Session["CIMSDatabasePassword"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePassword"]);
                        Session["CIMSDatabaseName"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabaseName"]);
                    }
                    else if (common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]))
                    {
                        Session["IsVIDALInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]);
                    }
                }
                Application["InterfaceForEMRDrugOrder"] = common.myStr(dsInterface.Tables[0].Rows[0]["InterfaceForEMRDrugOrder"]);
                Application["InterfaceForWordDrugRequisition"] = common.myStr(dsInterface.Tables[0].Rows[0]["InterfaceForWordDrugRequisition"]);
                //Session["InterfaceForOPSale"] = common.myStr(dsInterface.Tables[0].Rows[0]["InterfaceForOPSale"]);
                //Session["InterfaceForIPIssue"] = common.myStr(dsInterface.Tables[0].Rows[0]["InterfaceForIPIssue"]);
            }
        }
    }

    protected void lnkChangePassword_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "../MPages/ChangePassword.aspx?From=Login";
        RadWindow1.Height = 250;
        RadWindow1.Width = 440;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void lnkSetUp_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "../HospitalSetUp/UserSetUpWO_Master.aspx";
        RadWindow1.Height = 600;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindow1.VisibleStatusbar = false;


    }

    protected void ddlFacility_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Cache.Remove("HospitalSetup");
        FillEntrySite();
    }

    public string GetMACAddressWithBrowser()
    {
        if (Session["sMacAddress"] == null && string.IsNullOrEmpty(common.myStr(Session["sMacAddress"])))
        {
            //System.Net.IPHostEntry ipHostEntry = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.UserHostAddress);
            //String sMacAddress = ipHostEntry.HostName.ToString();
            //Session["sMacAddress"] = sMacAddress + "~" + Request.Browser.Browser;
            Session["sMacAddress"] = common.myStr(Request.ServerVariables["REMOTE_ADDR"]);
        }
        return common.myStr(Session["sMacAddress"]);
    }

    private string GetLoginExpiryMessage(int daydiff, string ShowExpWarning)
    {
        BaseC.User objUser = new BaseC.User(sConString);
        DataSet ds = objUser.getLoginExpiryMessage(common.myInt(ddlFacility.SelectedValue), daydiff, ShowExpWarning);
        if (ds.Tables[0].Rows.Count > 0)
        {
            return common.myStr(ds.Tables[0].Rows[0]["MessageText"]);
        }
        else
        {
            return " ";
        }
    }
    public static void ClearCache()
    {
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetExpires(DateTime.Now);
        HttpContext.Current.Response.Cache.SetNoServerCaching();
        HttpContext.Current.Response.Cache.SetNoStore();
        HttpContext.Current.Response.Cookies.Clear();
        HttpContext.Current.Request.Cookies.Clear();
    }

    public static void clearchachelocalall()
    {
        string GooglePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\History";
        //string GooglePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Google\Chrome\User Data\Default\";
        // GooglePath = @"C:\Users\user\AppData\Local\Google\Chrome\User Data\Default\";
        string MozilaPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Mozilla\Firefox\";
        string Opera1 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Opera\Opera";
        string Opera2 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Opera\Opera";
        string Safari1 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Apple Computer\Safari";
        string Safari2 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Apple Computer\Safari";
        string IE1 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Microsoft\Intern~1";
        string IE2 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Microsoft\Windows\History";
        string IE3 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Microsoft\Windows\Tempor~1";
        string IE4 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Microsoft\Windows\Cookies";
        string Flash = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Macromedia\Flashp~1";

        //Call This Method ClearAllSettings and Pass String Array Param
        ClearAllSettings(new string[] { GooglePath, MozilaPath, Opera1, Opera2, Safari1, Safari2, IE1, IE2, IE3, IE4, Flash });

    }

    public static void ClearAllSettings(string[] ClearPath)
    {
        foreach (string HistoryPath in ClearPath)
        {
            if (Directory.Exists(HistoryPath))
            {
                DoDelete(new DirectoryInfo(HistoryPath));
            }

        }
    }

    public static void DoDelete(DirectoryInfo folder)
    {
        try
        {

            foreach (FileInfo file in folder.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch
                { }

            }
            foreach (DirectoryInfo subfolder in folder.GetDirectories())
            {
                DoDelete(subfolder);
            }
        }
        catch
        {
        }
    }
}
