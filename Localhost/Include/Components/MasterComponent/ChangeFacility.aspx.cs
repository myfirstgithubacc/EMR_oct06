using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using Newtonsoft.Json;

public partial class Include_Components_MasterComponent_ChangeFacility : System.Web.UI.Page
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            PopulateFacility();
        }
    }
    protected void BtnChangeFacility_onClick(object sender, EventArgs e)
    {
        Cache["ENTRYSITES"] = "";

        Session["FacilityID"] = ddlFacility.SelectedValue;
        Session["FacilityName"] = common.myStr(ddlFacility.SelectedItem.Text);
        Session["EncounterId"] = null;
        Session["RegistrationId"] = null;
        string URL = "";
        fillHospitalDefaultValue();
        if (hdnParentPageURL.Value.Contains("?"))
            URL = hdnParentPageURL.Value.Substring(0, hdnParentPageURL.Value.IndexOf("?"));
        else
            URL = hdnParentPageURL.Value.Substring(0, hdnParentPageURL.Value.Length);
        hdnParentPageURL.Value = URL + "?irtrf=" + common.myStr(Session["StrO"])
                    + "&OP=" + common.myStr(Session["IsAdminGroup"]) + "_"
               + common.myStr(Session["LoginIsAdminGroup"]) + "_"
               + common.myStr(Session["HospitalLocationID"]) + "_"
               + common.myStr(Session["FacilityID"]) + "_"
               + common.myStr(Session["GroupID"]) + "_"
               + common.myStr(Session["FinancialYearId"]) + "_"
               + common.myStr(Session["EntrySite"]) + "_"
               + common.myStr(Session["UserId"]) + "_"
               + common.myStr(Session["UserName"]).Replace(" ", "$") + "_"
               + common.myInt(Session["ModuleId"]).ToString()
                                                + "_0_"
               + common.myStr(Session["FacilityName"]).Replace(" ", "$") + "_"
               + common.myStr(Session["CanDownloadPatientDocument"]) + "_"
                     + common.myInt(Session["FacilityStateId"]) + "_"
               + common.myInt(Session["EmployeeId"]) + "_"
               + common.myInt(Session["UserSpecialisationId"])
                   + "_" + common.myStr(Session["EmployeeName"])
                 + "_" + common.myBool(Session["IsEMRSupperUser"])
                + "_" + common.myBool(Session["ExpandFindPatient"])
                 + "_" + common.myStr(Session["EmployeeType"]);

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent()", true);
    }

    private string getHospitalSetupValueMultiple(int HospitalLocationId, int FacilityId, string flag)
    {
        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetFlagValueHospitalSetup";
        APIRootClass.getHospitalSetupValueMultiple objRoot = new global::APIRootClass.getHospitalSetupValueMultiple();
        objRoot.HospitalLocationId = HospitalLocationId;
        objRoot.FacilityId = FacilityId;
        objRoot.Flag = flag;
        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        return sValue;
    }

    private void fillHospitalDefaultValue()
    {
        Session["SeniorCitizenAge"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "SeniorCitizenAge");
        Session["SeniorCitizenCompany"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "SeniorCitizenCompany");
        Session["StaffCompanyId"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "StaffCompanyId");
        Session["StaffDependentCompanyId"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "StaffDependentCompanyId");
        Session["DefaultHospitalCompanyId"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultHospitalCompany");
        Session["DecimalPlace"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DecimalPlaces");
        Cache["PrintClaimXML"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "PrintClaimXML");
        Session["DefaultOPDCategory"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultOPDCategoryService");
        Session["DefaultCurrency"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultCurrency");
        string strMUDMeasureFlag = getHospitalSetupValueMultiple(Convert.ToInt16(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "MUDMeasure");
        Session["MUDMeasure"] = strMUDMeasureFlag.Trim() != "" ? Convert.ToBoolean(strMUDMeasureFlag) : false;
        Session["IPIncluded"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsInPatientIncluded");
        Session["QMSINTERFACED"] = getHospitalSetupValueMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "QMSINTERFACED");
        if (Cache["ENTRYSITES"] == "")
        {
            //BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
            //DataSet ds = objval.getEntrySites(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]));
            string ServiceURL1 = WebAPIAddress.ToString() + "api/Login/getEntrySites";
            APIRootClass.Allergy objRoot1 = new global::APIRootClass.Allergy();
            objRoot1.HospId = common.myInt(Session["HospitalLocationID"]);
            objRoot1.FacilityId = common.myInt(Session["FacilityId"]);
            //  objRoot.interfaceFor = common.myInt(common.enumCIMSorVIDALInterfaceFor.None);
            WebClient client1 = new WebClient();
            client1.Headers["Content-type"] = "application/json";
            client1.Encoding = Encoding.UTF8;
            string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
            string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
            sValue1 = JsonConvert.DeserializeObject<string>(sValue1);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue1);
            Cache["ENTRYSITES"] = ds;
        }

        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //DataSet dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));

        string ServiceURL2 = WebAPIAddress.ToString() + "api/Login/getFacilityInterfaceDetails";
        APIRootClass.Allergy objRoot2 = new global::APIRootClass.Allergy();
        objRoot2.HospId = common.myInt(Session["HospitalLocationId"]);
        objRoot2.FacilityId = common.myInt(Session["FacilityId"]);
        //  objRoot.interfaceFor = common.myInt(common.enumCIMSorVIDALInterfaceFor.None);
        WebClient client2 = new WebClient();
        client2.Headers["Content-type"] = "application/json";
        client2.Encoding = Encoding.UTF8;
        string inputJson2 = (new JavaScriptSerializer()).Serialize(objRoot2);
        string sValue2 = client2.UploadString(ServiceURL2, inputJson2);
        sValue2 = JsonConvert.DeserializeObject<string>(sValue2);
        DataSet dsInterface = JsonConvert.DeserializeObject<DataSet>(sValue2);


        if (dsInterface.Tables.Count > 0)
        {
            if (dsInterface.Tables[0].Rows.Count > 0)
            {
                Session["IsCIMSInterfaceActive"] = common.myStr(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]);
                Session["CIMSDatabasePath"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                Session["CIMSDatabasePassword"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePassword"]);
                Session["CIMSDatabaseName"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabaseName"]);
                Session["IsVIDALInterfaceActive"] = common.myStr(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]);
                Application["InterfaceForEMRDrugOrder"] = common.myStr(dsInterface.Tables[0].Rows[0]["InterfaceForEMRDrugOrder"]);
                Application["InterfaceForWordDrugRequisition"] = common.myStr(dsInterface.Tables[0].Rows[0]["InterfaceForWordDrugRequisition"]);
                Session["InterfaceForOPSale"] = common.myStr(dsInterface.Tables[0].Rows[0]["InterfaceForOPSale"]);
                Session["InterfaceForIPIssue"] = common.myStr(dsInterface.Tables[0].Rows[0]["InterfaceForIPIssue"]);
            }
        }


    }
    private void PopulateFacility()
    {
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Hashtable hshIn = new Hashtable();
            //hshIn.Add("@inyHospitalLocationId", Convert.ToInt32(Session["HospitalLocationID"]));
            //hshIn.Add("@intUserId", Session["UserID"]);
            ////hshIn.Add("@intGroupId", Session["GroupID"]);
            //DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspMasterGetUserFacilityList", hshIn);
            string ServiceURL2 = WebAPIAddress.ToString() + "api/EMRAPI/MasterGetUserFacilityList";
            APIRootClass.EMRModel objRoot2 = new global::APIRootClass.EMRModel();
            objRoot2.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot2.UserId = common.myInt(Session["UserID"]);
            //  objRoot.interfaceFor = common.myInt(common.enumCIMSorVIDALInterfaceFor.None);
            WebClient client2 = new WebClient();
            client2.Headers["Content-type"] = "application/json";
            client2.Encoding = Encoding.UTF8;
            string inputJson2 = (new JavaScriptSerializer()).Serialize(objRoot2);
            string sValue2 = client2.UploadString(ServiceURL2, inputJson2);
            sValue2 = JsonConvert.DeserializeObject<string>(sValue2);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue2);

            ddlFacility.DataSource = ds;
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataValueField = "FacilityId";
            ddlFacility.DataBind();

            ddlFacility.SelectedIndex = ddlFacility.Items.FindItemIndexByValue(common.myStr(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    ddlFacility.Enabled = false;
                }

            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

        }
    }
}
