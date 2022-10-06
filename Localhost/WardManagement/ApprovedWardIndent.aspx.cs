using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using BaseC;
using System.IO;
using System.Net;
using System.Configuration;
using System.Data;
using System.Text;
using System.Collections;

public partial class Pharmacy_SaleIssue_ApprovedWardIndent : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    StringBuilder strXMLItems;
    ArrayList coll;
    string IsShowIndent = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsCallback)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("/Login.aspx?Logout=1", false);
            }
        }
        Session["PreviousPageUrl"] = Request.Url;
        //if (common.myInt(Session["StoreId"]) == 0)
        //{
        //    if (!Page.IsCallback)
        //    {
        //        Response.Redirect("/Pharmacy/ChangeStore.aspx?Mpg=P335&PT=" + common.myStr(Request.Url.PathAndQuery), false);
        //    }
        //}

        if (!Page.IsPostBack)
        {
            lnkBtnCaseSheet.Visible = false;
            lnkBtnDiagnosticHistory.Visible = false;

            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            DataSet dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                    BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.InterfaceForDrugAdministered);


            if (dsInterface.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]))
                {
                    Session["IsCIMSInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]);
                    Session["CIMSDatabasePath"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                    Session["CIMSDatabasePassword"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePassword"]);
                }
            }
            dsInterface.Dispose();
            objEMR = null;
            divAppropriatenessReview.Visible = false;
            
        }
        Legend1.loadLegend("Approval", "'P','A','R'");
    }
    protected void btnRequestFromWard_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/WardManagement/PatientRequestFromWard.aspx?IsApproved=P&ShowOnlyDrugOrderForApproval=Y";
        RadWindow1.Height = 620;
        RadWindow1.Width = 1200;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientWardOnClientClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;

    }

    protected void btnbtnApproved_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/WardManagement/PatientRequestFromWard.aspx?IsApproved=A&ShowOnlyDrugOrderForApproval=Y";
        RadWindow1.Height = 620;
        RadWindow1.Width = 1200;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientWardOnClientClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
    }
    protected void btnRejected_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/WardManagement/PatientRequestFromWard.aspx?IsApproved=C&ShowOnlyDrugOrderForApproval=Y";
        RadWindow1.Height = 620;
        RadWindow1.Width = 1200;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientWardOnClientClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
    }


    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //BindPreviousData();
            lblMessage.Text = "";
            strXMLItems = new StringBuilder();
            coll = new ArrayList();

            if (e.CommandName == "Select")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //String txtDietRemarks = common.myStr(((TextBox)row.FindControl("txtDietRemarks")).Text);
                //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");

                string Itemid = common.myStr(e.CommandArgument);
                coll.Add(common.myInt(hdnIndentId.Value));
                coll.Add(common.myInt(Itemid));
                coll.Add("");
                coll.Add("");//for DietRemark
                strXMLItems.Append(common.setXmlTable(ref coll));

                ViewState["Type"] = "A";
                ViewState["SingleItem"] = true;
                divAppropriatenessReview.Visible = true;
                BindEMRPrescriptionReviewMaster();
                hdnXMLItems.Value = strXMLItems.ToString();
                hdnItemid.Value = Itemid;
                //BaseC.clsPharmacy ObjPhr = new BaseC.clsPharmacy(sConString);
                //String Result = ObjPhr.ApprovedWardIndentRequest(common.myInt(hdnIndentId.Value), common.myInt(common.myStr(Session["UserID"])), "A", "", strXMLItems.ToString(), common.myInt(ddlStore.SelectedValue));
                //lblMessage.Text = Result;
                //bindPatientWardRequest();
            }
            else if (e.CommandName == "WdCancel")
            {


                BaseC.Security objSecurity = new BaseC.Security(sConString);  //
                if (objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowRejectApprovedItemInWard") == false)
                {
                    objSecurity = null;
                    Alert.ShowAjaxMsg("You are not authorize for reject", Page);
                    return;
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //String txtRemark = common.myStr(((TextBox)row.FindControl("txtRemarks")).Text);
                string Itemid = common.myStr(e.CommandArgument);
                coll.Add(common.myInt(hdnIndentId.Value));
                coll.Add(common.myInt(Itemid));
                coll.Add("");
                coll.Add("");//for DietRemark
                strXMLItems.Append(common.setXmlTable(ref coll));


                ViewState["Type"] = "C";
                
                ViewState["SingleItem"] = true;
                divAppropriatenessReview.Visible = true;
                BindEMRPrescriptionReviewMaster();
                hdnXMLItems.Value = strXMLItems.ToString();
                hdnItemid.Value = Itemid;
                //BaseC.clsPharmacy ObjPhr = new BaseC.clsPharmacy(sConString);
                //String Result = ObjPhr.ApprovedWardIndentRequest(common.myInt(hdnIndentId.Value), common.myInt(common.myStr(Session["UserID"])), "C", "", strXMLItems.ToString(), common.myInt(ddlStore.SelectedValue));
                //lblMessage.Text = Result;
                //bindPatientWardRequest();
            }
            else if (e.CommandName.ToUpper().Equals("BRANDDETAILSCIMS"))
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");
                showBrandDetails(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName.ToUpper().Equals("MONOGRAPHCIMS"))
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");
                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName.ToUpper().Equals("INTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showIntreraction();
            }
            else if (e.CommandName.ToUpper().Equals("DIINTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showDiIntreraction();
            }
            else if (e.CommandName.ToUpper().Equals("DHINTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName.ToUpper().Equals("DAINTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction("A");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void showHealthOrAllergiesIntreraction(string HealthOrAllergies)
    {
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }
                string strXML = getHealthOrAllergiesInterationXML("B", string.Empty);
                if (!strXML.Equals(string.Empty))
                {
                    Session["CIMSXMLInputData"] = strXML;
                    openWindowsCIMS(false);
                }
            }
            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                if (common.myStr(HealthOrAllergies).Equals("H"))//Health
                {
                    int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();
                    if (commonNameGroupIds.Length > 0)
                    {
                        getDrugHealthInteractionVidal(commonNameGroupIds);
                    }
                }
                else if (common.myStr(HealthOrAllergies).Equals("A"))//Allergies
                {
                    int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();
                    if (commonNameGroupIds.Length > 0)
                    {
                        getDrugAllergyVidal(commonNameGroupIds);
                    }
                }
            }
        }
        catch
        {
        }
    }
    private void getDrugAllergyVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            //commonNameGroupIds = new int?[] { 4025, 4212, 516 };
            int?[] allergyIds = null; //new int?[] { 114 };
            int?[] moleculeIds = null;  //new int?[] { 202, 96, 36 };
            if (!common.myStr(ViewState["PatientAllergyXML"]).Equals(string.Empty))
            {
                allergyIds = (int?[])ViewState["PatientAllergyXML"];
            }
            StringBuilder sb = new StringBuilder();
            sb = objVIDAL.getVIDALDrugAllergyInteraction(commonNameGroupIds, allergyIds, moleculeIds);
            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }
            if (!sb.ToString().Equals(string.Empty))
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
                // openWindowsVIDAL("?UseFor=DA");
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
            objVIDAL = null;
        }
    }
    private void getDrugHealthInteractionVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        Hashtable collVitalItemIdFoundDH = new Hashtable();
        try
        {
            //int?[] commonNameGroupIds = new int?[] { 1524, 4025, 4212, 516, 28, 29, 30 };
            DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);//yyyy-mm-ddThh:mm:ss 
            //weight = common.myInt(lbl_Weight.Text);//In kilograms
            //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
            int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
            int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

            StringBuilder sb = objVIDAL.getVIDALDrugHealthInteraction(commonNameGroupIds, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
                    0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
                     (!common.myStr(ViewState["PatientDiagnosisXML"]).Equals(string.Empty)) ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
                    out collVitalItemIdFoundDH);
            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }
            if (!sb.ToString().Equals(string.Empty))
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
                //openWindowsVIDAL("?UseFor=HI");
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
            objVIDAL = null;
            collVitalItemIdFoundDH = null;
        }
    }
    private int?[] getVIDALCommonNameGroupIds()
    {
        int?[] commonNameGroupIds = null;
        try
        {
            List<int?> list = new List<int?>();
            foreach (GridViewRow dataItem in gvService.Rows)
            {
                HiddenField VIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
                LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                if (common.myInt(VIDALItemId.Value) > 0)
                //&& (lnkBtnInteractionVIDAL.Visible || lnkBtnDHInteractionVIDAL.Visible))
                {
                    list.Add(common.myInt(VIDALItemId.Value));
                }
            }

            commonNameGroupIds = list.ToArray();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return commonNameGroupIds;
    }
    private string getHealthOrAllergiesInterationXML(string useFor, string strNewPrescribing)
    {
        string strXML = string.Empty;
        try
        {
            //<Request>
            //    <Interaction>
            //        <Prescribing>
            //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
            //        </Prescribing>
            //        <Prescribed>
            //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
            //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
            //        </Prescribed>
            //        <Allergies>
            //            <Product reference="{8A4E15CD-ACE3-41D9-A367-55658256C2D4}" />
            //            <Product reference="{6D8F3E40-FA33-49C9-9D34-7C13F88E00FD}" />
            //        </Allergies>
            //        <HealthIssueCodes>
            //            <HealthIssueCode code="K22" codeType="ICD10" />
            //            <HealthIssueCode code="K22.0" codeType="ICD10" />
            //        </HealthIssueCodes>
            //        <References/>
            //    </Interaction>
            //</Request>
            string strPrescribing = string.Empty;
            StringBuilder ItemIds = new StringBuilder();
            if (!strNewPrescribing.Equals(string.Empty))
            {
                ItemIds.Append(strNewPrescribing);
            }
            foreach (GridViewRow dataItem in gvService.Rows)
            {
                HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                if (common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0"))
                {
                    string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                    CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                    string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                    if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                    {
                        ItemIds.Append(strPres);
                    }
                }
            }

            if (ItemIds.ToString().Equals(string.Empty))
            {
                return string.Empty;
            }
            strPrescribing = "<Prescribing>" + ItemIds.ToString() + "</Prescribing>";
            switch (useFor)
            {
                case "H"://Helth Interaction
                    strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientDiagnosisXML"]) + "<References /></Interaction></Request>";
                    break;
                case "A"://Allergies
                    strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientAllergyXML"]) + "<References /></Interaction></Request>";
                    break;
                case "B"://Both
                    strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientAllergyXML"]) + common.myStr(ViewState["PatientDiagnosisXML"]) + "<References /></Interaction></Request>";
                    break;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }
    private void showDiIntreraction()
    {
        try
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getDIInterationXML(string.Empty);
            if (!strXML.Equals(string.Empty))
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
        catch
        {
        }
    }
    private void showIntreraction()
    {
        try
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getInterationXML(string.Empty);
            if (!strXML.Equals(string.Empty))
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
        catch
        {
        }
    }
    private void showDIIntreraction()
    {
        try
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getDIInterationXML(string.Empty);
            if (!strXML.Equals(string.Empty))
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
        catch
        {
        }
    }
    private void showMonograph(string CIMSItemId, string CIMSType)
    {
        try
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            string strXML = getMonographXML(CIMSType, common.myStr(CIMSItemId));
            if (!strXML.Equals(string.Empty))
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
        catch
        { }
    }
    private void openWindowsCIMS(bool IsBrandDetails)
    {
        clsCIMS objCIMS = new clsCIMS();
        try
        {
            hdnCIMSOutput.Value = objCIMS.getCIMSFinalOutupt(IsBrandDetails);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "OpenCIMSWindow();", true);
            return;
        }
        catch
        {
        }
        finally
        {
            objCIMS = null;
        }
    }
    private void showBrandDetails(string CIMSItemId, string CIMSType)
    {
        try
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            string strXML = getBrandDetailsXMLCIMS(CIMSType, common.myStr(CIMSItemId));
            if (!strXML.Equals(string.Empty))
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(true);
            }
        }
        catch
        {
        }
    }
    private string getBrandDetailsXMLCIMS(string CIMSType, string CIMSItemId)
    {
        string strXML = string.Empty;
        try
        {
            //<Request>
            //  <Detail>
            //    <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}">
            //      <Items />
            //      <Packages />
            //      <Images />
            //      <TherapeuticClasses />
            //      <ATCCodes />
            //      <Companies />
            //      <Identifiers />
            //    </Product>
            //  </Detail>
            //</Request>
            CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
            strXML = "<Request><Detail><" + CIMSType + " reference=\"" + CIMSItemId + "\"><Items /><Packages /><Images /><TherapeuticClasses /><ATCCodes /><Companies /><Identifiers /></Product></Detail></Request>";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }
    private string getMonographXML(string CIMSType, string CIMSItemId)
    {
        string strXML = string.Empty;
        try
        {
            //<Request>
            //    <Content>
            //        <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
            //    </Content>
            //</Request>
            //strXML = "<Request><Content><Product reference=\"{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}\" /></Content></Request>";
            // <MONOGRAPH>
            CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
            strXML = "<Request><Content><" + CIMSType + " reference=\"" + CIMSItemId + "\" /></Content></Request>";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }

    private string getDIInterationXML(string strNewPrescribing)
    {
        string strXML = string.Empty;
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                //<Request>
                //    <Interaction>
                //        <Prescribing>
                //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
                //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
                //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
                //        </Prescribing>
                //        <Allergies />
                //        <DuplicateTherapy />
                //        <DuplicateIngredient />
                //        <References/>
                //    </Interaction>
                //</Request>
                string strPrescribing = string.Empty;
                StringBuilder ItemIds = new StringBuilder();
                if (!strNewPrescribing.Equals(string.Empty))
                {
                    ItemIds.Append(strNewPrescribing);
                }

                foreach (GridViewRow dataItem in gvService.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnDIInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDIInteractionCIMS");
                    if ((common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0")))
                    //&& lnkBtnInteractionCIMS.Visible
                    {
                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                        {
                            ItemIds.Append(strPres);
                        }
                    }
                }
                if (ItemIds.ToString().Equals(string.Empty))
                {
                    return string.Empty;
                }
                //strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";
                strPrescribing = "<Prescribing>" + ItemIds.ToString() + "</Prescribing>";
                strXML = "<Request><Interaction>" + strPrescribing + "<Allergies /><DuplicateTherapy /><DuplicateIngredient /><References /></Interaction></Request>";

                //strXML = "<Request><Interaction>" + strPrescribing + "<Allergies /><References /></Interaction></Request>";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }
    private string getInterationXML(string strNewPrescribing)
    {
        string strXML = string.Empty;
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                //<Request>
                //    <Interaction>
                //        <Prescribing>
                //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
                //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
                //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
                //        </Prescribing>
                //        <Allergies />
                //        <References/>
                //    </Interaction>
                //</Request>
                string strPrescribing = string.Empty;
                StringBuilder ItemIds = new StringBuilder();
                if (!strNewPrescribing.Equals(string.Empty))
                {
                    ItemIds.Append(strNewPrescribing);
                }

                foreach (GridViewRow dataItem in gvService.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                    if ((common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0")))
                    //&& lnkBtnInteractionCIMS.Visible
                    {
                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                        {
                            ItemIds.Append(strPres);
                        }
                    }
                }
                if (ItemIds.ToString().Equals(string.Empty))
                {
                    return string.Empty;
                }
                //strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";
                strPrescribing = "<Prescribing>" + ItemIds.ToString() + "</Prescribing>";
                strXML = "<Request><Interaction>" + strPrescribing + "<Allergies /><References /></Interaction></Request>";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }
    void bindPatientWardRequest()
    {
        DataSet dsItem = new DataSet();
        BaseC.WardManagement ward = new BaseC.WardManagement();
        try
        {
            if (common.myInt(hdnIndentId.Value) == 0)
            {
                return;
            }

            dsItem = ward.GetWARDPatientIndentDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]),
                common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]));
            if (dsItem.Tables.Count > 0)
            {
                if (dsItem.Tables[0].Rows.Count == 0)
                {
                    lblMessage.Text = "Patient not found !";
                    gvService.DataSource = null;
                    gvService.DataBind();
                    return;
                }
            }

            gvService.DataSource = dsItem.Tables[0];
            gvService.DataBind();
            ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(dsItem.Tables[0].Rows[0]["IndentStoreId"])));
            
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dsItem.Dispose();
            ward = null;
        }
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        foreach (GridViewRow gvr in gvService.Rows)
        {
            CheckBox chkApprove = (CheckBox)gvr.FindControl("chkCheck");
            if (chk.Checked)
            {
                chkApprove.Checked = true;
            }
            else
            {
                chkApprove.Checked = false;
            }
        }
    }


    protected void BtnApproveSelect_Click(object sender, EventArgs e)
    {
        ViewState["Type"] = "A";
        BindEMRPrescriptionReviewMaster();
        divAppropriatenessReview.Visible = true;
        #region Comments
        //BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        //strXMLItems = new StringBuilder();
        //coll = new ArrayList();
        //foreach (GridViewRow gvr in gvService.Rows)
        //{

        //    CheckBox chkApprove = (CheckBox)gvr.FindControl("chkCheck");

        //    if (chkApprove.Checked)
        //    {
        //        HiddenField hdnItemID = (HiddenField)gvr.FindControl("hdnItemId");
        //        //TextBox txtRemark = (TextBox)gvr.FindControl("txtRemarks");

        //        int indentId = common.myInt(hdnIndentId.Value);

        //        coll.Add(indentId);
        //        coll.Add(hdnItemID.Value);
        //        coll.Add("");
        //    }

        //    strXMLItems.Append(common.setXmlTable(ref coll));

        //}

        //String Result = objPharmacy.ApprovedWardIndentRequest(common.myInt(hdnIndentId.Value), common.myInt(common.myStr(Session["UserID"])), "A", "", strXMLItems.ToString(), common.myInt(ddlStore.SelectedValue));
        //lblMessage.Text = Result;
        //bindPatientWardRequest();
        //if (Convert.ToString(strXMLItems) == "")
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Checked items is not available. ";
        //    return;
        //}
        #endregion
    }
    private void ApprovedRejected(string type)
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        strXMLItems = new StringBuilder();
        coll = new ArrayList();

        StringBuilder strXMLParameters = new StringBuilder();
        ArrayList collParameters = new ArrayList();

        String Result = "";
        if (common.myBool(ViewState["SingleItem"]) == true)
        {
            if (common.myStr(hdnXMLItems.Value) != "")
            {
                foreach (GridViewRow gvr in gvPrescriptionReview.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                    HiddenField hdnId = (HiddenField)gvr.FindControl("hdnId");
                    TextBox txtRemarks = (TextBox)gvr.FindControl("txtRemarks");
                    if (chkSelect.Checked)
                    {
                        int indentId = common.myInt(hdnIndentId.Value);
                        collParameters.Add(hdnId.Value);
                        collParameters.Add(txtRemarks.Text); 
                        collParameters.Add(indentId);
                        collParameters.Add(hdnItemid.Value);
                    }
                    strXMLParameters.Append(common.setXmlTable(ref collParameters));
                }

                Result = objPharmacy.ApprovedWardIndentRequest(common.myInt(Session["HospitalLocationID"]),  common.myInt(Session["FacilityId"]),
                    common.myInt(hdnIndentId.Value), common.myInt(common.myStr(Session["UserID"])), type,
                    "",common.myStr(hdnXMLItems.Value), common.myInt(ddlStore.SelectedValue), strXMLParameters.ToString());
                ViewState["SingleItem"] = null;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Checked items is not available. ";
            }
        }
        else
        {
            foreach (GridViewRow gvr in gvService.Rows)
            {
                CheckBox chkApprove = (CheckBox)gvr.FindControl("chkCheck");
                //HiddenField hdnisapproved = (HiddenField)gvr.FindControl("hdnisapproved");
                if (chkApprove.Checked && chkApprove.Enabled)
                {
                    HiddenField hdnItemID = (HiddenField)gvr.FindControl("hdnItemId");
                    int indentId = common.myInt(hdnIndentId.Value);
                    coll.Add(indentId);
                    coll.Add(hdnItemID.Value);
                    coll.Add("");
                }
                strXMLItems.Append(common.setXmlTable(ref coll));

            }
            foreach (GridViewRow gvr in gvPrescriptionReview.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                HiddenField hdnId = (HiddenField)gvr.FindControl("hdnId");
                TextBox txtRemarks = (TextBox)gvr.FindControl("txtRemarks");
                if (chkSelect.Checked)
                {
                    int indentId = common.myInt(hdnIndentId.Value);
                    collParameters.Add(hdnId.Value);
                    collParameters.Add(txtRemarks.Text);
                    collParameters.Add(indentId);
                    collParameters.Add(DBNull.Value);
                }
                strXMLParameters.Append(common.setXmlTable(ref collParameters));
            }

            if (strXMLItems.ToString() != "")
            {
                Result = objPharmacy.ApprovedWardIndentRequest(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                    common.myInt(hdnIndentId.Value), common.myInt(common.myStr(Session["UserID"])), type, "", strXMLItems.ToString(), 
                    common.myInt(ddlStore.SelectedValue), strXMLParameters.ToString());
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Checked items is not available. ";
            }
        }
        lblMessage.Text = Result;
        bindPatientWardRequest();
    }
    protected void BtnRejectSelect_Click(object sender, EventArgs e)
    {
        ViewState["Type"] = "C";
        BindEMRPrescriptionReviewMaster();
        divAppropriatenessReview.Visible = true;
        #region Comments

        //BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        //strXMLItems = new StringBuilder();
        //coll = new ArrayList();
        //foreach (GridViewRow gvr in gvService.Rows)
        //{

        //    CheckBox chkApprove = (CheckBox)gvr.FindControl("chkCheck");

        //    if (chkApprove.Checked)
        //    {
        //        HiddenField hdnItemID = (HiddenField)gvr.FindControl("hdnItemId");
        //        //TextBox txtRemark = (TextBox)gvr.FindControl("txtRemarks");

        //        int indentId = common.myInt(hdnIndentId.Value);

        //        coll.Add(indentId);
        //        coll.Add(hdnItemID.Value);
        //        coll.Add("");
        //    }

        //    strXMLItems.Append(common.setXmlTable(ref coll));
        //}

        //String Result = objPharmacy.ApprovedWardIndentRequest(common.myInt(hdnIndentId.Value), common.myInt(common.myStr(Session["UserID"])), "C", "", strXMLItems.ToString(), common.myInt(ddlStore.SelectedValue));
        //lblMessage.Text = Result;
        //bindPatientWardRequest();
        //if (Convert.ToString(strXMLItems) == string.Empty)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Checked items is not available. ";
        //    return;
        //}
        #endregion 
    }

    protected void gvService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            // BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            //IsShowIndent = common.myStr(objBill.getHospitalSetupValue("IsShowIndentForSPS", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            //if (IsShowIndent == "Y")
            //{

            //    e.Row.Cells[3].Visible = false;
            //    e.Row.Cells[4].Visible = false;
            //    e.Row.Cells[6].Visible = false;
            //    e.Row.Cells[7].Visible = false;
            //    e.Row.Cells[8].Visible = false;
            //    e.Row.Cells[9].Visible = false;

            //    e.Row.Cells[10].Visible = false;
            //    e.Row.Cells[11].Visible = false;
            //    e.Row.Cells[12].Visible = false;
            //    e.Row.Cells[13].Visible = false;

            //    e.Row.Cells[14].Visible = false;
            //    e.Row.Cells[15].Visible = false;




            //    e.Row.Cells[18].Visible = true;
            //    e.Row.Cells[19].Visible = true;
            //    e.Row.Cells[20].Visible = true;

            //    e.Row.Cells[21].Visible = true;
            //    e.Row.Cells[22].Visible = true;
            //    e.Row.Cells[23].Visible = true;
            //    e.Row.Cells[24].Visible = true;
            //    e.Row.Cells[25].Visible = true;


            //}
            //else
            //{

            //    //e.Row.Cells[3].Visible = true;
            //    //e.Row.Cells[4].Visible = true;
            //    //e.Row.Cells[6].Visible = true;
            //    //e.Row.Cells[7].Visible = true;
            //    //e.Row.Cells[8].Visible = true;
            //    //e.Row.Cells[9].Visible = true;

            //    //e.Row.Cells[10].Visible = true;
            //    //e.Row.Cells[11].Visible = true;
            //    //e.Row.Cells[12].Visible = true;
            //    //e.Row.Cells[13].Visible = true;

            //    //e.Row.Cells[14].Visible = true;
            //    //e.Row.Cells[15].Visible = true;



            //    //e.Row.Cells[18].Visible = false;
            //    //e.Row.Cells[19].Visible = false;
            //    //e.Row.Cells[20].Visible = false;

            //    //e.Row.Cells[21].Visible = false;
            //    //e.Row.Cells[22].Visible = false;
            //    //e.Row.Cells[23].Visible = false;
            //    //e.Row.Cells[24].Visible = false;
            //    //e.Row.Cells[25].Visible = false;


            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkCheck = (CheckBox)e.Row.FindControl("chkCheck");
                HiddenField hdnisapproved = (HiddenField)e.Row.FindControl("hdnisapproved");
                LinkButton lnkApproved = (LinkButton)e.Row.FindControl("lnkApproved");
                LinkButton lnkCancel = (LinkButton)e.Row.FindControl("lnkCancel");
                HiddenField hdnApprovalStatusColor = (HiddenField)e.Row.FindControl("hdnApprovalStatusColor");
                HiddenField hdnTableString = (HiddenField)e.Row.FindControl("hdnTableString"); 
                if (common.myStr(hdnisapproved.Value) == "A")
                {
                    lnkApproved.Enabled = false;
                    chkCheck.Checked = true;
                    chkCheck.Enabled = false;
                }
                else if (common.myStr(hdnisapproved.Value) == "C")
                {
                    lnkCancel.Enabled = false;
                    lnkApproved.Enabled = true;
                }
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(hdnApprovalStatusColor.Value);
                e.Row.ToolTip = common.myStr(hdnTableString.Value);
            }
        }
        catch
        {

        }
    }

    protected void OnClick_lnkBtnCaseSheet(object sender, EventArgs e)
    {
        if (common.myInt(Session["RegistrationId"]).Equals(0) || common.myInt(Session["EncounterId"]).Equals(0))
        {
            lblMessage.Text = "Please select patient registration!";
            return;
        }

        RadWindow1.NavigateUrl = "/Editor/VisitHistory.aspx?Regid=" + common.myInt(Session["RegistrationId"]) +
                               "&RegNo=" + common.myInt(Session["RegistrationNo"]) +
                               "&EncId=" + common.myInt(Session["EncounterId"]) +
                               "&EncNo=" + common.myStr(Session["Encno"]) +
                               "&FromWard=Y&Category=PopUp";

        RadWindow1.Height = 620;
        RadWindow1.Width = 1200;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
    }


    protected void OnClick_lnkBtnDiagnosticHistory(object sender, EventArgs e)
    {
        if (common.myInt(Session["RegistrationNo"]).Equals(0))
        {
            lblMessage.Text = "Please select patient registration!";
            return;
        }
        RadWindow1.NavigateUrl = "/EMR/PatientHistory.aspx?POPUP=STATICTEMPLATE&From=EMR";
        RadWindow1.Height = 620;
        RadWindow1.Width = 1200;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
    }

    protected void btnRequestedWardItems_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        //done by rakesh for user authorisation start
        //btnSaveData.Visible = true;
        //SetPermission(btnSaveData, "N", true);
        //btnPrint.Visible = false;
        //SetPermission(btnPrint, false);
        //done by rakesh for user authorisation end
        //btnAddNewItem.Visible = true;

        showPatientDetails();


        if (common.myInt(hdnRegistrationId.Value) > 0)
        {
            lnkBtnCaseSheet.Visible = true;
            lnkBtnDiagnosticHistory.Visible = true;

            // btnFindPatient.Visible = false;
            bindPatientWardRequest();
        }
    }


    public void showPatientDetails()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();

        try
        {
            ViewState["CurrentWardId"] = "";
            ddlStore.Items.Clear();
            ddlStore.ClearSelection();
            ds = objEMR.getProvisionalAndFinalDiagnosis(common.myInt(Session["FacilityId"]), common.myInt(hdnEncounterId.Value));

            if (ds.Tables[0].Rows.Count > 0)
            {
                jlblDiagnosis.Text = common.myStr(ds.Tables[0].Rows[0]["ClinicalDiagnosis"]);
                jlblDiagnosis.ToolTip = jlblDiagnosis.Text;
            }

            ds = new DataSet();
            ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                               common.myStr(hdnRegistrationNo.Value), common.myStr(hdnEncounterNo.Value), common.myInt(Session["UserId"]), 0);

            imgAllergyAlert.Visible = false;

            if (ds.Tables[0].Rows.Count > 0)
            {
                jlblPatientname.Text = common.myStr(ds.Tables[0].Rows[0]["PatientName"]);
                jlblAGe.Text = common.myStr(ds.Tables[0].Rows[0]["AgeGender"]);
                jlblMRD.Text = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
                jlblEnc.Text = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
                jlblDoc.Text = common.myStr(ds.Tables[0].Rows[0]["DoctorName"]);
                jlblAddress.Text = common.myStr(ds.Tables[0].Rows[0]["PatientAddress"]);
                jlblBed.Text = common.myStr(ds.Tables[0].Rows[0]["BedNo"]);
                jlblMob.Text = common.myStr(ds.Tables[0].Rows[0]["MobileNo"]);
                jlblComp.Text = common.myStr(ds.Tables[0].Rows[0]["Payername"]);
                jlblWard.Text = common.myStr(ds.Tables[0].Rows[0]["WardName"]);
                jlblBMI.Text = common.myStr(ds.Tables[0].Rows[0]["LastBMI"]);
                jlblBSA.Text = common.myStr(ds.Tables[0].Rows[0]["LastBSA"]);

                jlblHeight.Text = common.myStr(ds.Tables[0].Rows[0]["Height"]);
                jlblWeight.Text = common.myStr(ds.Tables[0].Rows[0]["Weight"]);

                ViewState["CurrentWardId"] = common.myStr(ds.Tables[0].Rows[0]["CurrentWardId"]);

                imgAllergyAlert.Visible = common.myBool(ds.Tables[0].Rows[0]["AllergyAlert"]);
                BindStore();
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            objEMR = null;
            patient = null;
            ds.Dispose();
        }
    }

    protected void imgAllergyAlert_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);

            Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                   common.myStr(hdnRegistrationId.Value), common.myStr(hdnEncounterId.Value), common.myInt(Session["UserId"]), 0);
            //}
            RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&FromEMR=1&EId=" + common.myStr(hdnEncounterId.Value)
                + "&PId=" + common.myStr(hdnRegistrationId.Value) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(hdnRegistrationNo.Value)
                + "&PAG=" + common.myStr(Session["AgeGender"]) + "" + "&EncNo=" + common.myStr(hdnEncounterNo.Value) + "&SepPat=Y";
            RadWindow1.Height = 400;
            RadWindow1.Width = 1050;
            RadWindow1.Top = 20;
            RadWindow1.Left = 20;
            //RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }

    public void BindStore()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();

        try
        {
            ddlStore.Items.Clear();
            ddlStore.ClearSelection();

            DataSet dsPatientDetailPrescription = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];

            ds = objEMR.GetPrescriptionMasterList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                                     common.myInt(ViewState["RegId"]), common.myInt(0), common.myInt(Session["UserID"]),
                                                     common.myInt(0), common.myInt(Session["EmployeeId"]),
                                                     common.myInt(ViewState["CurrentWardId"]));

            if (ds.Tables.Count > 0)
            {
                ddlStore.DataSource = ds.Tables[0];
                ddlStore.DataTextField = "DepartmentName";
                ddlStore.DataValueField = "StoreId";
                ddlStore.DataBind();
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
            objEMR = null;
            ds.Dispose();

        }

    }
    private void BindEMRPrescriptionReviewMaster()
    {
        BaseC.WardManagement ward = new WardManagement();
        DataSet ds = new DataSet();
        try
        {
            ds = ward.GetEMRPrescriptionReviewMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPrescriptionReview.DataSource = ds.Tables[0];
                gvPrescriptionReview.DataBind();
            }
            ViewState["Count"] = ds.Tables[0].Rows.Count;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            ward = null;
            ds.Dispose();
        }
    }
    protected void btnSavePrescriptionReview_OnClick(object sender, EventArgs e)
    {
        int GridViewSelectCount = 0;
        foreach (GridViewRow gvRow in this.gvPrescriptionReview.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked)
            {
                GridViewSelectCount++;
            }
        }

        if (common.myStr(ViewState["Type"]) == "A")
        {
            if (GridViewSelectCount != common.myInt(ViewState["Count"]))
            {
                Alert.ShowAjaxMsg("Please select all prescription review parameters", Page);
                return;
            }
        }
        else
        {
            if (GridViewSelectCount == 0)
            {
                Alert.ShowAjaxMsg("Please select at least prescription review parameter", Page);
                return;
            }
        }
        ApprovedRejected(common.myStr(ViewState["Type"]));
        //ViewState["Count"] = null;
        //ViewState["Type"] = null;
        divAppropriatenessReview.Visible = false;
    }
    protected void btnClosePrescriptionReview_OnClick(object sender, EventArgs e)
    {
        divAppropriatenessReview.Visible = false;
    }
}
