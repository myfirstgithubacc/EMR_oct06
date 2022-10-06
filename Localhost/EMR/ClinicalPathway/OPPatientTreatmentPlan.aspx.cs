using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using System.Net;
using Telerik.Web.UI;
using System.Collections;
using System.Text;
using System.Configuration;

public partial class EMR_ClinicalPathway_OPPatientTreatmentPlan : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 10;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();

    private string cCtlType = string.Empty;
    DataSet objDs = new DataSet();
    DataTable dt = new DataTable();
    

    private enum enumNonT : byte
    {
        FieldId = 0,
        FieldName = 1,
        FieldType = 2,
        Values = 3,
        Remarks = 4,
        ParentId = 5,
        ParentValue = 6,
        Hierarchy = 7,
        SectionId = 8,
        DataObjectId = 9,
        IsMandatory = 10,
        MandatoryType = 11,
        EmployeeTypeId = 12
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP"))
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            lblMessage.Text = string.Empty;

            if (!IsPostBack)
            {
               
                bindEMRGetPatientTreatmentPlanList();
                GetServiceData(0, 0, 0, "");
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



    protected void btnSave_Click(object sender, EventArgs e)
    {
        StringBuilder strNonTabular = new StringBuilder();
        ArrayList collNonTabular = new ArrayList();


        StringBuilder strNonTabularP = new StringBuilder();
        ArrayList collFre = new ArrayList();

        ArrayList coll1 = new ArrayList();
        StringBuilder strXML = new StringBuilder();

        ArrayList coll = new ArrayList();
        StringBuilder strXML1 = new StringBuilder();

        ArrayList collChief = new ArrayList();
        StringBuilder objXMLProblem = new StringBuilder();
        BaseC.ParseData Parse = new BaseC.ParseData();


        StringBuilder xmlTemplateDetails = new StringBuilder();

        StringBuilder strXMLService = new StringBuilder();

        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        DataSet ds = new DataSet();

        clsExceptionLog objException = new clsExceptionLog();

        DataSet dsD = new DataSet();
        DataView dvD = new DataView();

        WebClient clientD = new WebClient();
        APIRootClass.ClinicalPath objRootD = new global::APIRootClass.ClinicalPath();
        try
        {


            clientD.Headers["Content-type"] = "application/json";
            clientD.Encoding = Encoding.UTF8;
            string ServiceURLD = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetDefaultTemplateForCarePlan";


            objRootD.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRootD.FacilityId = common.myInt(Session["FacilityId"]);

            string inputJsonD = (new JavaScriptSerializer()).Serialize(objRootD);
            string sValueD = clientD.UploadString(ServiceURLD, inputJsonD);
            sValueD = JsonConvert.DeserializeObject<string>(sValueD);
            dsD = JsonConvert.DeserializeObject<DataSet>(sValueD);



            #region  Problem
            if (!txtChiefComplaints.Text.Equals(string.Empty))
            {
                string TemplateId = Request.QueryString["POPUP"] != null && common.myStr(Request.QueryString["POPUP"]).Equals("StaticTemplate") ? common.myStr(Request.QueryString["TemplateFieldId"]) : "0";

                string strProblem = Parse.ParseQ(txtChiefComplaints.Text.Trim()).Replace("\n", "<br/>");

                if (common.myLen(strProblem) > 4000)
                {
                    Alert.ShowAjaxMsg("Chief complaints (free text) length must be less than 4000 character!", this.Page);
                    return;
                }
                collChief.Add(0);//Id
                collChief.Add(0);//ProblemId
                collChief.Add(strProblem);//Problem
                collChief.Add(0);//DurationID
                collChief.Add(string.Empty);//Duration
                collChief.Add(0);//ContextID
                collChief.Add(string.Empty);//Context
                collChief.Add(0);//SeverityId
                collChief.Add(string.Empty);//Severity
                collChief.Add(0);//IsPrimary
                collChief.Add(0);//IsChronic
                collChief.Add(common.myStr(Session["DoctorID"]));//DoctorId
                collChief.Add(common.myStr(Session["FacilityId"]));//FacilityId
                collChief.Add(0);//SCTId
                collChief.Add(string.Empty);//QualityIDs
                collChief.Add(0);//LocationID
                collChief.Add(string.Empty);//Location
                collChief.Add(0);//OnsetID
                collChief.Add(0);//AssociatedProblemId1
                collChief.Add(string.Empty);//AssociatedProblem1
                collChief.Add(0);//AssociatedProblemId2
                collChief.Add(string.Empty);//AssociatedProblem2
                collChief.Add(0);//AssociatedProblemId3
                collChief.Add(string.Empty);//AssociatedProblem3
                collChief.Add(0);//AssociatedProblemId4
                collChief.Add(string.Empty);//AssociatedProblem4
                collChief.Add(0);//AssociatedProblemId5
                collChief.Add(string.Empty);//AssociatedProblem5
                collChief.Add(string.Empty);//Side
                collChief.Add(0);//ConditionId
                collChief.Add(0);//Percentage
                collChief.Add(common.myInt(1));//Durations
                collChief.Add(common.myStr('D'));//DurationType
                collChief.Add(TemplateId);//TemplateFieldId
                collChief.Add(0);//ComplaintSearchId
                objXMLProblem.Append(common.setXmlTable(ref collChief));

            }
            #endregion


            #region History

            if (common.myLen(txtHistory.Text) > 0)
            {

                dvD = new DataView(dsD.Tables[0]);
                dvD.RowFilter = "Code='HIS'";
                if (dvD.ToTable().Rows.Count > 0)
                {
                    int iTemplateId = common.myInt(dvD.ToTable().Rows[0]["TemplateId"]);
                    bindVisitRecord(iTemplateId);
                }
               

                string strHistory = txtHistory.Text.Replace("\n", "<br/>");
                collFre = new ArrayList();
                collFre.Add(common.myInt(dvD.ToTable().Rows[0]["FieldId"]));
                collFre.Add("W");
                collFre.Add(strHistory);
                collFre.Add("0");
                collFre.Add(0);
                collFre.Add(common.myInt(ViewState["RecordId"]));
                collFre.Add(4);
                strNonTabularP.Append(common.setXmlTable(ref collFre));
            }


            #endregion

            #region Examination


            if (common.myLen(txtExamination.Text) > 0)
            {

                dvD = new DataView(dsD.Tables[0]);
                dvD.RowFilter = "Code='EXM'";
                if (dvD.ToTable().Rows.Count > 0)
                {
                    int iTemplateId = common.myInt(dvD.ToTable().Rows[0]["TemplateId"]);
                    bindVisitRecord(iTemplateId);
                }
                
                //bindVisitRecord(iTemplateId);
                string strExamination = txtExamination.Text.Replace("\n", "<br/>");

                collFre = new ArrayList();
                collFre.Add(common.myInt(dvD.ToTable().Rows[0]["FieldId"]));
                collFre.Add("W");
                collFre.Add(strExamination);
                collFre.Add("0");
                collFre.Add(0);
                collFre.Add(common.myInt(ViewState["RecordId"]));
                collFre.Add(5);
                strNonTabularP.Append(common.setXmlTable(ref collFre));

            }


            #endregion

            #region PlanofCare




            if (!txtPlanOfCare.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strPlanOfCare = txtPlanOfCare.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strPlanOfCare = strPlanOfCare.Replace("\n", "<br/>");


                dvD = new DataView(dsD.Tables[0]);
                dvD.RowFilter = "Code='POC'";
                if (dvD.ToTable().Rows.Count > 0)
                {
                    int iTemplateId = common.myInt(dvD.ToTable().Rows[0]["TemplateId"]);
                    bindVisitRecord(iTemplateId);
                }
                
                collFre = new ArrayList();
                collFre.Add(common.myInt(dvD.ToTable().Rows[0]["FieldId"]));
                collFre.Add("W");
                collFre.Add(strPlanOfCare);
                collFre.Add("0");
                collFre.Add(0); //coll.Add(RowCaptionId);

                collFre.Add(common.myInt(ViewState["RecordId"]));

                collFre.Add(3);
                strNonTabularP.Append(common.setXmlTable(ref collFre));


            }

            #endregion

            #region Instructions

            if (!txtFreeInstruction.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strInstructions = txtFreeInstruction.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strInstructions = strInstructions.Replace("\n", "<br/>");


                dvD = new DataView(dsD.Tables[0]);
                dvD.RowFilter = "Code='IN'";
                if (dvD.ToTable().Rows.Count > 0)
                {
                    int iTemplateId = common.myInt(dvD.ToTable().Rows[0]["TemplateId"]);
                    bindVisitRecord(iTemplateId);
                }
               

                collFre = new ArrayList();
                collFre.Add(common.myInt(dvD.ToTable().Rows[0]["FieldId"]));
                collFre.Add("W");
                collFre.Add(strInstructions);
                collFre.Add("0");
                collFre.Add(0); //coll.Add(RowCaptionId);

                collFre.Add(common.myInt(ViewState["RecordId"]));

                collFre.Add(2);
                strNonTabularP.Append(common.setXmlTable(ref collFre));

            }

            #endregion

            xmlTemplateDetails.Append(strNonTabularP.ToString());

            #region Service


            foreach (GridViewRow row in gvService.Rows)
            {
                CheckBox chkService = (CheckBox)row.FindControl("chkService");
                HiddenField hdnServiceId = (HiddenField)row.FindControl("hdnServiceId");
                if (chkService.Checked)
                {

                    decimal ServiceAmount = 0;
                    ds = order.GetServiceDescriptionForOrderpage(common.myInt(Session["HospitallocationId"]), common.myInt(Session["FacilityId"]),
                                                            common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(hdnServiceId.Value),
                                                            0, common.myInt(Session["CompanyId"]), common.myInt(Session["CompanyId"]), 0, 1, 0);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        ServiceAmount = common.myDec(ds.Tables[0].Rows[0]["serviceamount"]);
                    }

                    strXMLService.Append("<Table1><c1>");
                    strXMLService.Append(common.myInt(hdnServiceId.Value));
                    strXMLService.Append("</c1><c2>");
                    strXMLService.Append("</c2><c3>");

                    strXMLService.Append(common.myInt(1));//Units
                    strXMLService.Append("</c3><c4>");
                    strXMLService.Append(common.myInt(Session["DoctorId"]));
                    strXMLService.Append("</c4><c5>");
                    strXMLService.Append(ServiceAmount);
                    strXMLService.Append("</c5><c6>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c6><c7>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c7><c8>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c8><c9>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c9><c10>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c10><c11>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c11><c12>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c12><c13>");
                    strXMLService.Append(common.myInt(0));
                    strXMLService.Append("</c13><c14>");
                    strXMLService.Append("0");
                    strXMLService.Append("</c14><c15>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c15><c16>");
                    strXMLService.Append("0");
                    strXMLService.Append("</c16><c17>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c17><c18>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c18><c19>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c19><c20>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c20><c21>");
                    strXMLService.Append(string.Empty);

                    strXMLService.Append("</c21><c22>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c22><c23>");
                    strXMLService.Append(0);
                    strXMLService.Append("</c23><c24>");
                    strXMLService.Append(0);
                    strXMLService.Append("</c24><c25>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c25><c26>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c26><c27>");
                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c27><c28>");
                    strXMLService.Append(common.myInt(Session["FacilityId"]));
                    strXMLService.Append("</c28><c29>");

                    strXMLService.Append(0);
                    strXMLService.Append("</c29><c30>");

                    strXMLService.Append(string.Empty);

                    strXMLService.Append("</c30><c31>");

                    strXMLService.Append(DBNull.Value);
                    strXMLService.Append("</c31></Table1>");

                }
            }
            #endregion

            #region OP Drug
            foreach (GridViewRow dataItem in gvPrescription.Rows)
            {
                CheckBox chkPrescription = (CheckBox)dataItem.FindControl("chkPrescription");
                if (chkPrescription.Checked)
                {
                    HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");

                    HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    HiddenField hdnQty = (HiddenField)dataItem.FindControl("hdnQty");
                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");

                    HiddenField hdnDoseUnitID = (HiddenField)dataItem.FindControl("hdnDoseUnitID");
                    HiddenField hdnFrequencyID = (HiddenField)dataItem.FindControl("hdnFrequencyID");
                    HiddenField hdnFoodRelationId = (HiddenField)dataItem.FindControl("hdnFoodRelationId");
                    Label lblIntructions = (Label)dataItem.FindControl("lblIntructions");
                    Label lblDose = (Label)dataItem.FindControl("lblDose");
                    Label lblDays = (Label)dataItem.FindControl("lblDays");
                    Label lblDaysType = (Label)dataItem.FindControl("lblDaysType");

                    Label lblFrequency = (Label)dataItem.FindControl("lblFrequency");
                    Label lblRouteName = (Label)dataItem.FindControl("lblRouteName");


                    string sPrescriptionDetail = lblFrequency.Text + " for " + lblDays.Text + " " + lblDaysType.Text + "(" + lblRouteName.Text + ")";

                    coll1.Add(common.myInt(hdnGenericId.Value));//GenericId int

                    coll1.Add(common.myInt(hdnItemId.Value));//ItemId INT,
                    coll1.Add(DBNull.Value);//CustomMedication varchar(1000)


                    coll1.Add(DBNull.Value);//FormulationId int
                    coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
                    coll1.Add(DBNull.Value);//StrengthId INT




                    string sStartDate = string.Empty;

                    sStartDate = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");



                    coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
                    coll.Add(common.myInt(hdnFrequencyID.Value));//FrequencyId TINYINT,
                    coll.Add(common.myDec(lblDose.Text));//Dose DECIMAL(10,3),
                    coll.Add(common.myStr(lblDays.Text));//Duration VARCHAR(20)
                    coll.Add(common.myStr(lblDaysType.Text));//DURATION TYPE CHAR(1),
                    coll.Add(lblIntructions.Text);//INSTRUCTIONID VARCHAR(1000),
                    coll.Add(common.myInt(hdnDoseUnitID.Value));//UNITID INT,
                    coll.Add(DBNull.Value);//ReferanceItem INT,
                    coll.Add(common.myInt(hdnFoodRelationId.Value));//FoodRelationship INT,
                    coll.Add(null);//iDoseTypeId INT,

                    coll.Add(DBNull.Value);//
                    coll.Add(DBNull.Value);//
                    coll.Add(DBNull.Value);//
                    coll.Add(DBNull.Value);//
                    coll.Add(DBNull.Value);//
                    coll.Add(DBNull.Value);//
                    coll.Add(DBNull.Value);//

                    coll.Add(string.Empty);//
                    coll.Add(string.Empty);// variable sequence no     
                    coll.Add(DBNull.Value);
                    strXML.Append(common.setXmlTable(ref coll));



                    coll1.Add(sStartDate);//StartDate SMALLDATETIME,
                    coll1.Add(sStartDate);//EndDate SMALLDATETIME,
                    coll1.Add(common.myDec(hdnQty.Value));//TotalQty INT
                    coll1.Add(DBNull.Value);//ICD CODE VARCHAR
                    coll1.Add(common.myInt(0));//Refill INT
                    coll1.Add(common.myBool(0));//Is Override BIT
                    coll1.Add(DBNull.Value);//OverrideComments VARCHAR
                    coll1.Add(DBNull.Value);//DrugAllergyScreeningResult VARCHAR
                    coll1.Add(common.myInt(424));//PrescriptionModeId INT
                    coll1.Add(0);//NotToPharmcy BIT
                    coll1.Add(sPrescriptionDetail);//Prescription detail VARCHAR
                    coll1.Add(DBNull.Value); //OverrideCommentsDrugToDrug VARCHAR(500),
                    coll1.Add(DBNull.Value); //OverrideCommentsDrugHealth VARCHAR(500)
                    coll1.Add(DBNull.Value); //StrengthValue varchar(255)

                    strXML1.Append(common.setXmlTable(ref coll1));
                }

            }
            #endregion

            #region Non Tabular
            //bindVisitRecord(common.myInt(ddlPlanTemplates.SelectedItem.Attributes["TemplateId"]));
            //ViewState["RecordId"] = 1;
            foreach (GridViewRow item2 in gvSelectedServices.Rows)
            {
                if (item2.RowType.Equals(DataControlRowType.DataRow))
                {
                    HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");
                    HiddenField hdnSectionId = (HiddenField)item2.FindControl("hdnSectionId");
                    HiddenField hdnFieldId = (HiddenField)item2.FindControl("hdnFieldId");
                    // SectionId = common.myInt(hdnSectionId.Value);

                    if (common.myStr(hdnFieldType.Value).Equals("T"))
                    {
                        TextBox txtT = (TextBox)item2.FindControl("txtT");

                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("T");
                        coll.Add(txtT.Text);


                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);



                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    if (common.myStr(hdnFieldType.Value).Equals("I"))
                    {
                        TextBox txtT = (TextBox)item2.FindControl("txtT");
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("I");
                        coll.Add(txtT.Text);

                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);


                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    if (common.myStr(hdnFieldType.Value).Equals("IS"))
                    {
                        TextBox txtM = (TextBox)item2.FindControl("txtM");
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("IS");
                        if (common.myStr(txtM.Text).Contains("<"))
                        {
                            txtM.Text = txtM.Text.Replace("<", "&lt;");
                        }
                        if (common.myStr(txtM.Text).Contains(">"))
                        {
                            txtM.Text = common.myStr(txtM.Text).Replace(">", "&gt;");
                        }
                        coll.Add(txtM.Text);

                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);



                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("M"))
                    {
                        TextBox txtM = (TextBox)item2.FindControl("txtM");
                        //if (txtM.Text.Trim() != "")
                        //{
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("M");
                        if (common.myStr(txtM.Text).Contains("<"))
                        {
                            txtM.Text = txtM.Text.Replace("<", "&lt;");
                        }
                        if (common.myStr(txtM.Text).Contains(">"))
                        {
                            txtM.Text = common.myStr(txtM.Text).Replace(">", "&gt;");
                        }

                        coll.Add(txtM.Text);



                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);



                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("W")) // For Word Processor
                    {
                        RadEditor txtW = (RadEditor)item2.FindControl("txtW");
                        //if (txtW.Content != null)
                        //{
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("W");
                        coll.Add(txtW.Content);


                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);



                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("D"))
                    {
                        DropDownList ddl = (DropDownList)item2.FindControl("D");
                        // if (ddl.SelectedItem.Text != "Select")
                        // {
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("D");
                        coll.Add(ddl.SelectedValue);


                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);



                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("IM"))
                    {
                        RadComboBox ddl = (RadComboBox)item2.FindControl("IM");
                        // if (ddl.SelectedItem.Text != "Select")
                        // {
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("IM");
                        coll.Add(ddl.SelectedValue);


                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);

                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("R"))
                    {
                        RadioButtonList ddl = (RadioButtonList)item2.FindControl("R");
                        // if (ddl.SelectedItem.Text != "Select")
                        // {
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("R");
                        coll.Add(ddl.SelectedValue);


                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);


                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("B"))
                    {
                        RadioButtonList ddlB = (RadioButtonList)item2.FindControl("B");
                        if (ddlB.SelectedItem != null)
                        {
                            if (!ddlB.SelectedItem.Text.Equals("Select"))
                            {
                                coll.Add(common.myInt(hdnFieldId.Value));
                                coll.Add("B");
                                coll.Add((common.myInt(ddlB.SelectedValue).Equals(1)) ? 1 : 0);


                                coll.Add("0");
                                coll.Add(0);
                                coll.Add(1);
                                coll.Add(null);



                                strNonTabular.Append(common.setXmlTable(ref coll));
                            }
                            else
                            {
                                coll.Add(common.myInt(hdnFieldId.Value));
                                coll.Add("B");
                                coll.Add(null);


                                coll.Add("0");
                                coll.Add(0);
                                coll.Add(1);
                                coll.Add(null);



                                strNonTabular.Append(common.setXmlTable(ref coll));
                            }
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("C"))
                    {
                        DataList rptC = (DataList)item2.FindControl("C");
                        string sCheckedValues = string.Empty;
                        foreach (DataListItem rptItem in rptC.Items)
                        {
                            CheckBox chk = (CheckBox)rptItem.FindControl("C");
                            HiddenField hdn = (HiddenField)rptItem.FindControl("hdnCV");
                            HtmlTextArea CT = (HtmlTextArea)rptItem.FindControl("CT");

                            sCheckedValues = chk.Checked == true ? hdn.Value : "";
                            coll.Add(common.myInt(hdnFieldId.Value));
                            coll.Add("C");
                            coll.Add(sCheckedValues);
                            coll.Add("0");

                            coll.Add(0);
                            coll.Add(1);
                            coll.Add(null);

                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                        sCheckedValues = string.Empty;
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("S"))
                    {
                        TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");

                        if (!txtDate.Text.Trim().Equals("__/__/____") && !txtDate.Text.Trim().Equals(string.Empty))
                        {

                            coll.Add(common.myInt(hdnFieldId.Value));
                            coll.Add("S");

                            coll.Add(common.myStr(txtDate.Text).Trim());


                            coll.Add("0");
                            coll.Add(0);
                            coll.Add(1);
                            coll.Add(null);



                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("ST"))
                    {
                        TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");


                        if (tpTime.SelectedDate != null && common.myStr(tpTime.SelectedDate) != string.Empty)
                        {

                            coll.Add(common.myInt(hdnFieldId.Value));
                            coll.Add("S");

                            DateTime d = tpTime.SelectedDate.Value;
                            string time = d.ToString("HH:mm:ss");

                            coll.Add(common.myStr("01/01/1900").Trim() + " " + common.myStr(time));

                            coll.Add("0");
                            coll.Add(0);
                            coll.Add(1);
                            coll.Add(null);


                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }

                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("SB"))
                    {
                        TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");

                        if (tpTime.SelectedDate != null && common.myStr(tpTime.SelectedDate) != string.Empty)
                        {

                            coll.Add(common.myInt(hdnFieldId.Value));
                            coll.Add("S");

                            DateTime d = tpTime.SelectedDate.Value;
                            string time = d.ToString("HH:mm:ss");

                            coll.Add(common.myStr(txtDate.Text).Trim() + " " + common.myStr(time));


                            coll.Add("0");
                            coll.Add(0);
                            coll.Add(1);
                            coll.Add(null);



                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("H"))
                    {
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("H");
                        coll.Add("-");
                        coll.Add("0");


                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);

                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("O"))
                    {
                        DropDownList ddl = (DropDownList)item2.FindControl("D");
                        TextBox txtT = (TextBox)item2.FindControl("txtT");
                        int DataObjectId = common.myInt(item2.Cells[(byte)enumNonT.DataObjectId].Text);

                        clsIVF objivf = new clsIVF(sConString);
                        DataTable dtObject = objivf.getTemplateDataObjectQuery(DataObjectId);

                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("O");
                        if (common.myStr(dtObject.Rows[0]["ObjectFieldType"]) == "D")
                        {
                            coll.Add(ddl.SelectedValue);
                        }
                        else
                        {
                            coll.Add(common.myStr(txtT.Text));
                        }
                        coll.Add("0");


                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);

                        objivf = null;
                        dtObject.Dispose();

                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }

                }
            }
            #endregion

            xmlTemplateDetails.Append(strNonTabular.ToString());
            //if (strNonTabular.ToString() == "")
            //{
            //    Alert.ShowAjaxMsg("Please select Template option", Page);
            //    return;
            //}

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRSavePatientTreatmentPlanTemplates";
            APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);

            objRoot.xmlServiceDetails = strXMLService.ToString();

            objRoot.xmlProblems = objXMLProblem.ToString();
            objRoot.xmlItems = strXML1.ToString();

            objRoot.xmlItemDetail = strXML.ToString();
            objRoot.xmlTemplateDetails = xmlTemplateDetails.ToString();

            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            objRoot.DoctorId = common.myInt(Session["DoctorId"]);



            objRoot.PlanId = common.myInt(ddlPlanTemplates.SelectedValue);

            objRoot.DayId = common.myInt(ViewState["DayId"]);
            objRoot.DayDetailId = common.myInt(ViewState["DayDetailId"]);


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = sValue;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);




            //GetServiceData(common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]), common.myInt(ViewState["DayDetailId"]), "");
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            strNonTabular = null;
            collNonTabular = null;
            strNonTabularP = null;
            collFre = null;
            coll1 = null;
            strXML = null;
            coll = null;
            strXML1 = null;
            collChief = null;
            objXMLProblem = null;
            Parse = null;
            xmlTemplateDetails = null;
            strXMLService = null;
            order = null;
            ds.Dispose();
            objException = null;

            dsD.Dispose();
            dvD.Dispose();

            clientD = null;
            objRootD = null;
        }
    }


    private void bindVisitRecord(int TemplateId)
    {
        clsIVF objivf = new clsIVF(sConString);
        DataSet ds = new DataSet();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            ds = objivf.getEMRTemplateVisitRecoreds(common.myInt(Session["EncounterId"]), common.myInt(TemplateId), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                ViewState["RecordId"] = 1;
            }
            else
            {
                int rCount = ds.Tables[0].Rows.Count;
                if (rCount > 0)
                {
                    ViewState["RecordId"] = common.myInt(ds.Tables[0].Rows[rCount - 1]["RecordId"]) + 1;
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
            objivf = null;
            ds.Dispose();
            objException = null;
        }
    }



    protected void GetServiceData(int TemplatePlanId, int DayId, int DayDetailId, string TemplateType)
    {
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        DataView dv = new DataView();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {

            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanDetails";

          

            objRoot.TemplateId = common.myStr(TemplatePlanId);
            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;
            objRoot.TemplateType = TemplateType;
            objRoot.IsCopy = false;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            objDs = ds;
            if (TemplateType == "SP")
            {
                //if (ds.Tables[2].Rows.Count > 0)
                //{
                //    gvSpecialsation.DataSource = ds.Tables[2];
                //    gvSpecialsation.DataBind();
                //}
                //else
                //{
                //    DataRow dr = ds.Tables[2].NewRow();
                //    ds.Tables[2].Rows.InsertAt(dr, 0);
                //    gvSpecialsation.DataSource = ds.Tables[2];
                //    gvSpecialsation.DataBind();
                //}
            }
            else if (TemplateType == "SR")
            {
                if (ds.Tables[3].Rows.Count > 0)
                {
                    gvService.DataSource = ds.Tables[3];
                    gvService.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[3].NewRow();
                    ds.Tables[3].Rows.InsertAt(dr, 0);
                    gvService.DataSource = ds.Tables[3];
                    gvService.DataBind();
                }
            }
            else if (TemplateType == "DC")
            {
                if (ds.Tables[4].Rows.Count > 0)
                {
                    gvDrugClass.DataSource = ds.Tables[4];
                    gvDrugClass.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[4].NewRow();
                    ds.Tables[4].Rows.InsertAt(dr, 0);

                    gvDrugClass.DataSource = ds.Tables[4];
                    gvDrugClass.DataBind();
                }
            }
            else if (TemplateType == "DR")
            {
                if (ds.Tables[5].Rows.Count > 0)
                {
                    gvPrescription.DataSource = ds.Tables[5];
                    gvPrescription.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[5].NewRow();
                    ds.Tables[5].Rows.InsertAt(dr, 0);

                    gvPrescription.DataSource = ds.Tables[5];
                    gvPrescription.DataBind();
                }
            }
            else if (TemplateType == "TM")
            {
                //if (ds.Tables[6].Rows.Count > 0)
                //{
                //    gvTemplateLis.DataSource = ds.Tables[6];
                //    gvTemplateLis.DataBind();
                //}
                //else
                //{
                //    DataRow dr = ds.Tables[6].NewRow();
                //    ds.Tables[6].Rows.InsertAt(dr, 0);

                //    gvTemplateLis.DataSource = ds.Tables[6];
                //    gvTemplateLis.DataBind();
                //}
            }
            else if (TemplateType == "TF")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvSelectedServices.DataSource = ds.Tables[0];
                    gvSelectedServices.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    //ds.Tables[0].Rows.InsertAt(dr, 0);
                    gvSelectedServices.DataSource = BindBlankTable();
                    gvSelectedServices.DataBind();
                }
            }
            else if (TemplateType == "CH" || TemplateType == "IN" || TemplateType == "POC" || TemplateType == "HIS" || TemplateType == "EXM")
            {
                txtChiefComplaints.Text = "";
                txtFreeInstruction.Text = "";
                txtPlanOfCare.Text = "";
                txtHistory.Text = "";
                txtExamination.Text = "";
                if (ds.Tables[7].Rows.Count > 0)
                {
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='CH'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtChiefComplaints.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='IN'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtFreeInstruction.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='POC'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtPlanOfCare.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='HIS'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtHistory.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='EXM'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtExamination.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvSelectedServices.DataSource = ds.Tables[0];
                    gvSelectedServices.DataBind();
                }
                else
                {
                    if (TemplatePlanId != 0)
                    {
                        //added by bhakti
                        //DataRow dr = ds.Tables[0].NewRow();
                        //ds.Tables[0].Rows.InsertAt(dr, 0);

                        gvSelectedServices.DataSource = BindBlankTable();
                        gvSelectedServices.DataBind();
                    }
                }

                //if (ds.Tables[2].Rows.Count > 0)
                //{
                //    gvSpecialsation.DataSource = ds.Tables[2];
                //    gvSpecialsation.DataBind();
                //}
                //else
                //{
                //    DataRow dr = ds.Tables[2].NewRow();
                //    ds.Tables[2].Rows.InsertAt(dr, 0);
                //    gvSpecialsation.DataSource = ds.Tables[2];
                //    gvSpecialsation.DataBind();
                //}
                if (ds.Tables[3].Rows.Count > 0)
                {
                    gvService.DataSource = ds.Tables[3];
                    gvService.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[3].NewRow();
                    ds.Tables[3].Rows.InsertAt(dr, 0);
                    gvService.DataSource = ds.Tables[3];
                    gvService.DataBind();
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    gvDrugClass.DataSource = ds.Tables[4];
                    gvDrugClass.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[4].NewRow();
                    ds.Tables[4].Rows.InsertAt(dr, 0);

                    gvDrugClass.DataSource = ds.Tables[4];
                    gvDrugClass.DataBind();
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    gvPrescription.DataSource = ds.Tables[5];
                    gvPrescription.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[5].NewRow();
                    ds.Tables[5].Rows.InsertAt(dr, 0);

                    gvPrescription.DataSource = ds.Tables[5];
                    gvPrescription.DataBind();
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                   // gvTemplateLis.DataSource = ds.Tables[6];
                   // gvTemplateLis.DataBind();
                }
                else
                {
                    //DataRow dr = ds.Tables[6].NewRow();
                    //ds.Tables[6].Rows.InsertAt(dr, 0);

                    //gvTemplateLis.DataSource = ds.Tables[6];
                    //gvTemplateLis.DataBind();
                }
                if (ds.Tables[7].Rows.Count > 0)
                {
                    txtChiefComplaints.Text = "";
                    txtFreeInstruction.Text = "";
                    txtPlanOfCare.Text = "";
                    txtHistory.Text = "";
                    txtExamination.Text = "";
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='CH'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtChiefComplaints.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='IN'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtFreeInstruction.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='POC'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtPlanOfCare.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='HIS'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtHistory.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='EXM'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtExamination.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
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
            objRoot = null;
            client = null;
            ds.Dispose();
            dv.Dispose();
            objException = null;
        }
    }
    protected void gvSelectedServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        DataView ddv = new DataView();
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

                DataView objDv = null;
                DataView objDvValue;
                DataTable objDt = null;
                // HtmlTextArea txtMulti = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                HiddenField hdnFieldType = (HiddenField)e.Row.FindControl("hdnFieldType");
                HiddenField hdnSectionId = (HiddenField)e.Row.FindControl("hdnSectionId");
                HiddenField hdnFieldId = (HiddenField)e.Row.FindControl("hdnFieldId");
                HiddenField hdnColumnNosToDisplay = (HiddenField)e.Row.FindControl("hdnColumnNosToDisplay");

                if (objDs != null && objDs.Tables.Count > 1 && objDs.Tables[1].Rows.Count > 0)
                {
                    objDv = objDs.Tables[1].DefaultView;
                    objDv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                }
                ddv = new DataView(dt); // To Be Check

                if (ddv.Count > 0)
                {
                    ddv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                    objDt = ddv.ToTable();
                    if (objDt.Rows.Count > 0)
                    {
                        e.Row.Visible = true;
                    }
                }
                else
                {
                    if (objDs.Tables.Count > 1)
                    {
                        if (objDs.Tables[1].Rows.Count > 0)
                        {
                            objDvValue = objDs.Tables[1].DefaultView;
                            if (objDvValue.Table.Columns["FieldId"] != null)
                            {
                                objDvValue.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            }
                            objDt = objDvValue.ToTable();
                            if (objDt.Rows.Count > 0)
                            {
                                e.Row.Visible = true;
                            }
                        }
                    }
                }


                if (!common.myStr(hdnFieldType.Value).Trim().Equals(string.Empty))
                {

                    #region Single TextBox Type
                    if (common.myStr(hdnFieldType.Value).Equals("T"))
                    {
                        cCtlType = "T";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        txtT.Enabled = true;
                        txtW.Visible = false;

                        string maxLength = common.myStr(txtT.MaxLength);

                        txtT.Visible = true;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtT.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                txtT.ToolTip = txtT.Text;
                            }

                        }


                    }
                    #endregion
                    #region start for Field type I
                    if (common.myStr(hdnFieldType.Value).Trim().Equals("I"))
                    {
                        cCtlType = "I";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");

                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");


                        txtT.Enabled = true;
                        txtW.Visible = false;
                        txtT.Visible = true;
                    }
                    #endregion

                    #region start for Field type IS
                    if (common.myStr(hdnFieldType.Value).Trim().Equals("IS"))
                    {
                        cCtlType = "IS";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        HyperLink Hy_LinkUrl = (HyperLink)e.Row.FindControl("Hy_LinkUrl");
                        txtT.Enabled = false;
                        txtW.Visible = false;
                        txtT.Visible = false;
                        Hy_LinkUrl.Visible = false;
                        txtM.Visible = true;

                    }
                    #endregion
                    #region Mutiple Text Type
                    else if (common.myStr(hdnFieldType.Value).Equals("M"))
                    {
                        cCtlType = "M";
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        txtM.Visible = true;

                        txtM.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txtM.ClientID + "');");
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtM.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                txtM.ToolTip = txtM.Text;
                            }
                        }
                    }
                    #endregion
                    #region WordProcessor Type
                    else if (common.myStr(hdnFieldType.Value).Trim().Equals("W")) // For WordProcessor
                    {
                        cCtlType = "W";
                        DropDownList ddl = (DropDownList)e.Row.Cells[(byte)enumNonT.Values].FindControl("ddlTemplateFieldFormats");
                        ddl.Visible = true;
                        ddl.DataSource = BindFieldFormats(common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text).ToString());
                        ddl.DataTextField = "FormatName";
                        ddl.DataValueField = "FormatId";
                        ddl.DataBind();
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = true;

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtW.Content = common.myStr(objDt.Rows[0]["FieldValue"]);
                            }
                        }
                    }
                    #endregion

                    #region CheckBox Type
                    else if (common.myStr(hdnFieldType.Value).Equals("C"))
                    {
                        cCtlType = "C";
                        DataList list = (DataList)e.Row.FindControl("C");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        txtW.Visible = false;
                        tbl1.Visible = false;
                        list.Visible = true;

                        list.DataSource = objDv;
                        list.DataBind();

                        //if (hdnColumnNosToDisplay.Value != null)
                        //{
                        //    list.RepeatColumns = common.myInt(hdnColumnNosToDisplay.Value);
                        list.RepeatDirection = RepeatDirection.Horizontal;

                        //}

                        HiddenField hdnCV = (HiddenField)list.FindControl("hdnCV");
                        foreach (DataListItem item in list.Items)
                        {
                            HtmlTextArea CT = (HtmlTextArea)item.FindControl("CT");
                            CT.Attributes.Add("onkeypress", "javascript:return AutoChange('" + CT.ClientID + "');");
                            CT.Attributes.Add("onkeydown", "javascript:return AutoChange('" + CT.ClientID + "');");
                            HiddenField hdn = (HiddenField)item.FindControl("hdnCV");
                            CheckBox chk = (CheckBox)item.FindControl("C");
                            chk.Checked = false;
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    foreach (DataRow drow in objDt.Rows)
                                    {
                                        if (common.myStr(drow["FieldValue"]).Trim().Equals(common.myStr(hdn.Value).Trim()))
                                        {
                                            chk.Checked = true;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Boolean Type
                    else if (common.myStr(hdnFieldType.Value).Equals("B"))
                    {
                        cCtlType = "B";
                        RadioButtonList B = (RadioButtonList)e.Row.FindControl("B");

                        B.Attributes.Add("onclick", "radioMe(event,'" + B.ClientID + "');");


                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        B.Visible = true;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                objDvValue = objDt.DefaultView;
                                if (objDvValue.Table.Columns["FieldId"] != null)
                                {
                                    objDvValue.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                                }

                                objDt = objDvValue.ToTable();

                                if (objDt.Rows.Count > 0)
                                {
                                    if (common.myInt(objDt.Rows[0]["FieldValue"]).Equals(1))//Yes
                                    {
                                        B.SelectedValue = "1";
                                    }
                                    else if (common.myInt(objDt.Rows[0]["FieldValue"]).Equals(0))//No
                                    {
                                        B.SelectedValue = "0";
                                    }
                                    else
                                    {
                                        B.SelectedValue = "-1";  //Select
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region DropDown Type
                    else if (common.myStr(hdnFieldType.Value).Equals("D"))
                    {
                        cCtlType = "D";
                        DropDownList ddl = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;

                        ddl.DataSource = objDv;
                        ddl.DataTextField = "ValueName";
                        ddl.DataValueField = "ValueId";
                        ddl.DataBind();

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])) != null)
                                {
                                    ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])).Selected = true;
                                }
                            }
                        }

                    }
                    #endregion
                    #region DropDown Image Type
                    else if (common.myStr(hdnFieldType.Value).Equals("IM"))
                    {
                        cCtlType = "IM";
                        RadComboBox ddl = (RadComboBox)e.Row.Cells[(byte)enumNonT.Values].FindControl("IM");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        //ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        //btnAdd.Visible = false;
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;


                        foreach (DataRow drImage in objDv.ToTable().Rows)
                        {
                            RadComboBoxItem item = new RadComboBoxItem();
                            item.Text = (string)drImage["ValueName"];
                            item.Value = drImage["ValueId"].ToString();
                            item.ImageUrl = drImage["ImagePath"].ToString();
                            ddl.Items.Add(item);
                            item.DataBind();
                        }
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (common.myStr(objDt.Rows[0]["FieldValue"]) != null)
                                {
                                    ddl.SelectedValue = common.myStr(objDt.Rows[0]["FieldValue"]);
                                }
                            }
                        }

                    }
                    #endregion

                    #region RadioButton Type
                    else if (common.myStr(hdnFieldType.Value).Equals("R"))
                    {
                        cCtlType = "R";
                        RadioButtonList ddl = (RadioButtonList)e.Row.Cells[(byte)enumNonT.Values].FindControl("R");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        //  HiddenField hdnColumnNosToDisplay = (HiddenField)e.Row.FindControl("hdnColumnNosToDisplay");

                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;

                        if (hdnColumnNosToDisplay.Value != null)
                        {
                            ddl.RepeatColumns = common.myInt(hdnColumnNosToDisplay.Value);
                            ddl.RepeatDirection = RepeatDirection.Horizontal;

                        }
                        ddl.DataSource = objDv;
                        ddl.DataTextField = "ValueName";
                        ddl.DataValueField = "ValueId";
                        ddl.DataBind();

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])) != null)
                                {
                                    ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])).Selected = true;
                                }
                            }
                        }

                    }
                    #endregion
                    #region Date Type
                    else if (common.myStr(hdnFieldType.Value).Equals("S"))//For Date
                    {
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;

                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        AjaxControlToolkit.CalendarExtender cal = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CalendarExtender3");
                        txtW.Visible = false;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtDate.Text = common.myStr(objDt.Rows[0]["FieldValue"]).Trim();
                            }
                        }
                        tblDate.Visible = true;
                    }
                    #endregion
                    #region Header Type
                    else if (common.myStr(hdnFieldType.Value).Equals("H"))//For Heading
                    {
                        cCtlType = "H";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        //btnAdd.Visible = false;

                        txtT.Visible = false;
                        txtM.Visible = false;
                        //btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;
                        e.Row.Cells[(byte)enumNonT.FieldName].Font.Bold = true;
                    }
                    #endregion
                    #region Static Template Type
                    else if (common.myStr(hdnFieldType.Value).Equals("L"))
                    {
                        cCtlType = "L";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");

                        LinkButton lnkStaticTemplate = (LinkButton)e.Row.FindControl("lnkStaticTemplate");

                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");

                        string sStaticTemplateId = common.myStr(lnkStaticTemplate.CommandArgument);
                        lnkStaticTemplate.Visible = true;


                        //BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
                        //DataSet dsStatic = master.GetAllTypeTemplates(common.myInt(Session["HospitalLocationId"]), "S");

                        WebClient client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetAllTypeTemplates";
                        APIRootClass.GetAllTypeTemplates objRoot = new global::APIRootClass.GetAllTypeTemplates();
                        objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                        objRoot.sType = "S";

                        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                        string sValue = client.UploadString(ServiceURL, inputJson);
                        sValue = JsonConvert.DeserializeObject<string>(sValue);
                        DataSet dsStatic = JsonConvert.DeserializeObject<DataSet>(sValue);

                        DataView dvStatic = new DataView(dsStatic.Tables[0]);
                        dvStatic.RowFilter = "DisplayInTemplate=1 AND SectionId=" + sStaticTemplateId;
                        DataTable dtStatic = dvStatic.ToTable();
                        if (dtStatic.Rows.Count > 0)
                        {
                            HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                            HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                            //if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                            //{
                            //    lnkStaticTemplate.Attributes.Add("onclick", "openStaticTemplateWindow('" + common.myStr(dtStatic.Rows[0]["PageUrl"]) + "',' " + sStaticTemplateId + "',' " + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.FieldId].Text).Trim()) + "',' " + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.SectionId].Text).Trim()) + "')");
                            lnkStaticTemplate.Enabled = true;
                            //}
                            //else
                            //{
                            //    lnkStaticTemplate.Enabled = false;
                            //}

                            lnkStaticTemplate.Text = common.myStr(dtStatic.Rows[0]["SectionName"]);
                        }
                        // btnAdd.Visible = false;

                        txtT.Visible = false;
                        txtM.Visible = false;
                        // btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;
                        e.Row.Cells[(byte)enumNonT.FieldName].Font.Bold = true;



                    }
                    #endregion
                    #region Patient Data Object
                    else if (common.myStr(hdnFieldType.Value).Equals("O"))//Patient Data Object
                    {
                        cCtlType = "O";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        // btnAdd.Visible = false;
                        txtT.Visible = false;
                        txtM.Visible = false;
                        // btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;

                        int DataObjectId = common.myInt(e.Row.Cells[(byte)enumNonT.DataObjectId].Text);

                        //clsIVF objivf = new clsIVF(sConString);
                        clsIVF objivf = new clsIVF(string.Empty);

                        string strOutput = objivf.getDataObjectValue(DataObjectId);

                        if (strOutput.Length > 50)
                        {
                            txtM.Visible = true;
                            txtM.Text = strOutput;
                            txtM.ToolTip = strOutput;
                            txtM.Enabled = false;
                        }
                        else
                        {
                            txtT.Visible = true;
                            txtT.Text = strOutput;
                            txtT.ToolTip = strOutput;
                            txtT.Enabled = false;
                            txtT.Columns = strOutput.Length + 2;

                        }

                    }
                    #endregion
                }
                else
                {
                    e.Row.Cells[(byte)enumNonT.Values].Text = "No Record Found!";
                }
                if (cCtlType.Equals("T") || cCtlType.Equals("M"))
                {
                    RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                    txtW.Visible = false;
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
            ddv.Dispose();
            objException = null;
        }
    }

    protected void ddlTemplateFieldFormats_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        DataSet ds = new DataSet();
        try
        {
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            RadEditor txtW = (RadEditor)row.FindControl("txtW");

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetFormatText";
            APIRootClass.GetFormatText objRoot = new global::APIRootClass.GetFormatText();
            objRoot.FormatId = common.myInt(ddl.SelectedValue);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtW.Content = common.myStr(ds.Tables[0].Rows[0]["FormatText"]);
            }
            else
            {
                txtW.Content = String.Empty;
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
            ds.Dispose();
        }
    }
    public DataTable BindFieldFormats(String strFieldId)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetTemplateFieldFormats";
            APIRootClass.GetTemplateFieldFormats objRoot = new global::APIRootClass.GetTemplateFieldFormats();
            objRoot.FieldId = common.myInt(strFieldId);
            objRoot.SpecialisationId = common.myInt(ViewState["DoctorSpecialisationId"]);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr;
                dr = dt.NewRow();
                dr["FormatId"] = 0;
                dr["FormatName"] = "--Options--";
                dt.Rows.InsertAt(dr, 0);
            }
            ds.Dispose();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dt;
    }

    protected void bindEMRGetPatientTreatmentPlanList()
    {
        clsExceptionLog objException = new clsExceptionLog();
        DataSet ds = new DataSet();

        DataView dv = new DataView();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        try
        {
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanList";

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            dv = new DataView(ds.Tables[0]);
            if(dv.Count>0)
            dv.RowFilter = "PlanType='OPD'";
            
            if (dv.ToTable().Rows.Count > 0)
            {
                for (int i = 0; i < dv.ToTable().Rows.Count; i++)
                {
                    Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                    item.Text = (string)dv.ToTable().Rows[i]["PlanName"];
                    item.Value = common.myStr(dv.ToTable().Rows[i]["PlanId"]);
                    item.Attributes["TemplateId"] = dv.ToTable().Rows[i]["TemplateId"].ToString();

                    this.ddlPlanTemplates.Items.Add(item);
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
            ds.Dispose();
            objException = null;
            dv.Dispose();
        }
    }
   

    protected void ddlPlanTemplates_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DataSet ds = new DataSet();
        string LastChangedDate = string.Empty;
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Template Name.", Page.Page);
                return;
            }
            
            ViewState["DayId"] = 1;
            ViewState["DayDetailId"] = 1;
            GetServiceData(common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]), common.myInt(ViewState["DayDetailId"]), "");
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
            ds.Dispose();
            LastChangedDate = string.Empty;
        }
    }

    
    

    protected void gvService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
            CheckBox chkService = (CheckBox)e.Row.FindControl("chkService");
            if (common.myInt(hdnId.Value) > 0)
            {
                chkService.Checked = true;
            }
        }
    }

    protected void gvDrugClass_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
            HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
            CheckBox chkDrugClass = (CheckBox)e.Row.FindControl("chkDrugClass");
            if (common.myInt(hdnId.Value) > 0)
            {
                chkDrugClass.Checked = true;
            }
           
        }
    }

    protected void gvPrescription_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
            HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
            CheckBox chkPrescription = (CheckBox)e.Row.FindControl("chkPrescription");
            if (common.myInt(hdnId.Value) > 0)
            {
                chkPrescription.Checked = true;
            }
            
        }
    }

    
    protected void chkAllService_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox gv = (CheckBox)sender;
        CheckBox chkAllService = (CheckBox)gv.FindControl("chkAllService");

        bool bTrue = false;
        if (chkAllService.Checked)
        {
            bTrue = true;
        }
        else
        {
            bTrue = false;
        }

        foreach (GridViewRow item in gvService.Rows)
        {
            CheckBox chkService = (CheckBox)item.FindControl("chkService");
            chkService.Checked = bTrue;
        }

     
    }

    protected void chkAllDrugClass_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox gv = (CheckBox)sender;
        CheckBox chkAllDrugClass = (CheckBox)gv.FindControl("chkAllDrugClass");

        bool bTrue = false;
        if (chkAllDrugClass.Checked)
        {
            bTrue = true;
        }
        else
        {
            bTrue = false;
        }

        foreach (GridViewRow item in gvDrugClass.Rows)
        {
            CheckBox chkDrugClass = (CheckBox)item.FindControl("chkDrugClass");
            chkDrugClass.Checked = bTrue;
        }

        
    }

    protected void chkAllPrescription_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox gv = (CheckBox)sender;
        CheckBox chkAllPrescription = (CheckBox)gv.FindControl("chkAllPrescription");

        bool bTrue = false;
        if (chkAllPrescription.Checked)
        {
            bTrue = true;
        }
        else
        {
            bTrue = false;
        }
        foreach (GridViewRow item in gvPrescription.Rows)
        {
            CheckBox chkPrescription = (CheckBox)item.FindControl("chkPrescription");
            chkPrescription.Checked = bTrue;
        }

        

    }

   

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        bool bTrue = false;
        if (chkSelectAll.Checked)
        {
            bTrue = true;
        }
        else
        {
            bTrue = false;
        }
        foreach (GridViewRow item2 in gvSelectedServices.Rows)
        {
            HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");
            if (common.myStr(hdnFieldType.Value).Equals("C"))
            {
                DataList rptC = (DataList)item2.FindControl("C");
                foreach (DataListItem rptItem in rptC.Items)
                {
                    CheckBox chk = (CheckBox)rptItem.FindControl("C");
                    chk.Checked = bTrue;
                }
            }
        }
        
    }

    //added by bhakti
    protected DataTable BindBlankTable()
    {
        DataTable dtInv = new DataTable();

        dtInv.Columns.Add("FieldName", typeof(string));
        dtInv.Columns.Add("FieldType", typeof(string));
        dtInv.Columns.Add("SectionId", typeof(string));
        dtInv.Columns.Add("FieldID", typeof(string));
        dtInv.Columns.Add("ColumnNosToDisplay", typeof(string));
        dtInv.Columns.Add("MaxLength", typeof(string));
        dtInv.Columns.Add("ValueId", typeof(string));
        dtInv.Columns.Add("ValueName", typeof(string));
        return dtInv;

    }
}