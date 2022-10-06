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
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;

public partial class ViewDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private Hashtable hstInput;
    clsExceptionLog objException = new clsExceptionLog();
    private int iPrevId = 0;
    StringBuilder sb = new StringBuilder();
    string sFontSize = string.Empty;
    static DataSet dsStatusId = new DataSet();
    StringBuilder objStrTmp = new StringBuilder();
    BaseC.EMROrders order;

    protected void Page_Load(object sender, EventArgs e)
    {
        string RegNo = common.myStr(Request.QueryString["RegNo"]);
        PatientInfo(RegNo);

        if (!IsPostBack)
        {
            lblColorCodeForTemplateRequired.BackColor = System.Drawing.Color.Red;
            lblColorCodeForMandatoryTemplate.BackColor = System.Drawing.Color.Blue;


            if (common.myStr(Request.QueryString["ServId"]) != string.Empty)
            {
                hdnServiceID.Value = Request.QueryString["ServId"].ToString();

                string serviceName = Request.QueryString["ServName"].ToString();
                lblServName.Text = serviceName;
                bindMainTemplateListOfSelectedService(common.myInt(hdnServiceID.Value));

                if (common.myStr(Request.QueryString["TagType"]) == "D")
                    Label1.Text = "Sub Department";
                else
                    Label1.Text = "Service Name";

            }
        }
    }
    protected void lnkViewServiceAck_OnClick(object sender, EventArgs e)
    {
        string PatientType;
        string RegID;
        string ProcACID;
        string orderId;
        PatientType = Request.QueryString["PatientType"].ToString();
        orderId = Request.QueryString["orderId"].ToString();
        string sOrdDtlId = Request.QueryString["sOrdDtlId"].ToString();
        string sServiceId = Request.QueryString["ServId"].ToString();
        string servicename = Request.QueryString["ServName"].ToString();
        string EncounterId = Request.QueryString["EncounterId"].ToString();
        int sStatus = 1;
        RegID = Request.QueryString["RegID"].ToString();
        ProcACID = Request.QueryString["ProcACID"].ToString();
        int FacliityID = common.myInt(Request.QueryString["FacliityID"].ToString());

        int IsMandatoryTemplateEntered = EMRCheckOrderTemplateDetails(common.myInt(sOrdDtlId), 0);
        if (IsMandatoryTemplateEntered.Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Mandatory template details not available";
            return;
        }
        else
        {
            lblMessage.Text = string.Empty;
        }
        RadWindow3.NavigateUrl = "~/EMR/Orders/ServiceAckDetails.aspx?Type=" + PatientType +
            "&OrderID=" + orderId + "&Orderdetaild=" + sOrdDtlId + "&ServiceID=" + sServiceId +
            "&Status=" + sStatus + "&Viewtype=UA&servicename=" + servicename + "&ProcACID=" + ProcACID
            + "&RegID=" + RegID + "&FacliityID=" + FacliityID + "&sStatus=" + sStatus + "&EncounterId=" + EncounterId;
        RadWindow3.Height = 300;
        RadWindow3.Width = 500;
        RadWindow3.Top = 10;
        RadWindow3.Left = 10;
        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindow3.OnClientClose = "OnClientAcknowlegdeRemarkClose";
        RadWindow3.Behavior = WindowBehaviors.None;
        RadWindow3.Modal = true;
        RadWindow3.VisibleStatusbar = false;
    }
    public int EMRCheckOrderTemplateDetails(int orderDetailId, int checkStatus)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hstInput = new Hashtable();
        Hashtable hstOutput = new Hashtable();
        hstInput.Add("@intOrderDetailId", orderDetailId);
        hstOutput.Add("@inyCheckStatus", checkStatus);
        hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

        hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRCheckOrderTemplateDetails", hstInput, hstOutput);

        return Convert.ToInt32(hstOutput["@inyCheckStatus"].ToString());
    }
    private void bindMainTemplateListOfSelectedService(int iServiceId)
    {
        try
        {
            DataSet ds = new DataSet();

            //ds = objivf.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), EmpType, iDoctorSpecialisation, iServiceId, common.myStr(Session["OPIP"]), TemplateIdType);

            ds = getEMRTemplateForSelectedService(common.myInt(Session["HospitalLocationId"]), iServiceId);

            // change by balkishan start
            DataView view = new DataView();
            view.Table = ds.Tables[0];

            ddlTemplateMain.Items.Clear();
            ddlTemplateMain.Visible = true;

            RadComboBoxItem item;
            if (view.ToTable().Rows.Count > 0)
            {
                DataView dv = ((DataTable)view.ToTable()).DefaultView;
                if (common.myStr(Request.QueryString["Type"]) != "")
                    dv.RowFilter = "TypeName = '" + common.myStr(Request.QueryString["Type"]) + "'";

                foreach (DataRow dr in dv.ToTable().Rows)
                {
                    item = new RadComboBoxItem();
                    item.Text = common.myStr(dr["TemplateName"]);
                    item.Value = common.myInt(dr["TemplateId"]).ToString();


                    order = new BaseC.EMROrders(sConString);
                    int result = order.IsTemplateRequiredForService(2, common.myInt(hdnServiceID.Value), common.myInt(dr["TemplateId"]));
                    if (result.Equals(1))
                    {
                        item.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        item.ForeColor = System.Drawing.Color.Blue;
                    }


                    item.Attributes.Add("TemplateTypeID", common.myStr(dr["TemplateTypeID"]));
                    item.Attributes.Add("EntryType", common.myStr(dr["EntryType"]));

                    ddlTemplateMain.Items.Add(item);
                    ddlTemplateMain.DataBind();

                }
            }

            if (ddlTemplateMain.Items.Count > 0)
            {
                if (ddlTemplateMain.SelectedIndex == -1)
                {
                    ddlTemplateMain.SelectedIndex = 0;
                    ddlTemplateMain_SelectedIndexChanged(null, null);
                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    public DataSet getEMRTemplateForSelectedService(int HospId, int iServiceId)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string qry = "";
            //HshIn.Add("@intHospId", HospId);

            if (iServiceId > 0)
            {
                HshIn.Add("@intFacilityId", common.myStr(Session["FacilityId"]));
                if (common.myStr(Request.QueryString["TagType"]) == "D")// Sub Department
                {
                    HshIn.Add("@intServiceId", 0);
                    HshIn.Add("@intSubDeptId", common.myInt(Request.QueryString["SubDepId"]));
                }
                else // Service
                {
                    HshIn.Add("@intServiceId", iServiceId);
                    HshIn.Add("@intSubDeptId", 0);
                }


                //qry = "SELECT et.Id AS TemplateId, et.TemplateName, et.TemplateTypeID, et.EntryType, et.SpecialisationId, t.TypeName " +
                //      " FROM EMRTemplate et  left join EMRTemplateTypes t on t.ID = et.TemplateTypeID " +
                //      " INNER JOIN EMRTemplateServiceTagging es on es.TemplateId = et.id " +
                //      " WHERE et.Active = 1 " +
                //    //Added by rakesh start
                //      " AND es.Active = 1 " +
                //    //Added by rakesh end
                //      " AND es.Serviceid = @iServiceId " +
                //      " AND et.HospitalLocationID = @intHospId " +
                //      " ORDER BY et.TemplateName";

            }

            // ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetServiceTemplates", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }


    protected void bindData(string iFormId, string TemplateId, StringBuilder sb)
    {
        string str = "";
        string sEntryType = "V";
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();

        Hashtable hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }
        hstInput.Add("@intFormId", "1");
        hstInput.Add("@bitDischargeSummary", 0);
        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            sEntryType = common.myStr(ds.Tables[0].Rows[0]["EntryType"]);
        }
        hstInput = new Hashtable();
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }
        else
        {
            hstInput.Add("chrGenderType", "M");
        }
        if (Request.QueryString["ER"] != null && Request.QueryString["ER"].ToString() == "ER")
        {
            hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
            hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
        }
        else
        {
            if (sEntryType == "S")
            {
                hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
            }
            else
            {
                hstInput.Add("@intEncounterId", common.myStr(Request.QueryString["EncounterId"]));
            }
        }
        hstInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
        hstInput.Add("@OrderDetailId", Request.QueryString["sOrdDtlId"].ToString());

        if (common.myStr(Request.QueryString["TagType"]) == "D")// Sub Department
        {
            hstInput.Add("@intRequestId", hdnServiceID.Value);
        }


        DataSet dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);
        string BeginList = "", EndList = "", BeginList2 = "", BeginList3 = "", EndList3 = "";
        int t = 0, t2 = 0, t3 = 0;
        DataView dv = new DataView(dsAllSectionDetails.Tables[2]);

        //1234
        //if (txtFromDate.SelectedDate.Value != null && txtToDate.SelectedDate.Value != null)
        //{
        //    dv.RowFilter = "EntryDate>='" + Convert.ToDateTime(common.myDate(txtFromDate.SelectedDate.Value)).ToString("yyyy/MM/dd 00:00") +
        //        "' AND EntryDate<='" + Convert.ToDateTime(common.myDate(txtToDate.SelectedDate.Value)).ToString("yyyy/MM/dd 23:59")+"'";
        //}

        DataTable dtEntry = dv.ToTable(true, "RecordId");
        int iRecordId = 0;

        for (int it = 0; it < dtEntry.Rows.Count; it++)
        {
            foreach (DataRow item in ds.Tables[0].Rows)
            {

                DataTable dtFieldValue = new DataTable();
                DataView dv1 = new DataView(dsAllSectionDetails.Tables[0]);
                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                DataTable dtFieldName = dv1.ToTable();
                if (dsAllSectionDetails.Tables.Count > 2)
                {
                    DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);

                    dv2.RowFilter = "RecordId=" + common.myStr(dtEntry.Rows[it]["RecordId"]);
                    dtFieldValue = dv2.ToTable();
                }

                DataSet dsAllFieldsDetails = new DataSet();
                dsAllFieldsDetails.Tables.Add(dtFieldName);
                dsAllFieldsDetails.Tables.Add(dtFieldValue);
                if (dsAllSectionDetails.Tables[0].Rows.Count > 0)
                {
                    if (dsAllSectionDetails.Tables.Count > 2)
                    {
                        if (dsAllSectionDetails.Tables[2].Rows.Count > 0) // add for education details
                        {
                            string sBegin = "", sEnd = "";

                            DataRow dr3;
                            dr3 = dsAllSectionDetails.Tables[0].Rows[0];

                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);

                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);
                            str = CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]), common.myStr(item["Tabular"]), item["SectionId"].ToString());
                            str += " ";
                            //}
                            if (iPrevId == common.myInt(item["TemplateId"]))
                            {
                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                {
                                    if (sEntryType == "M")
                                    {
                                        objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                    }
                                }
                                if (t2 == 0)
                                {
                                    if (t3 == 0)//Template
                                    {
                                        t3 = 1;
                                        if (common.myStr(item["SectionsListStyle"]) == "1")
                                        {
                                            BeginList3 = "<ul>"; EndList3 = "</ul>";
                                        }
                                        else if (common.myStr(item["SectionsListStyle"]) == "2")
                                        {
                                            BeginList3 = "<ol>"; EndList3 = "</ol>";
                                        }
                                    }
                                }

                                if (common.myStr(item["SectionsBold"]) != ""
                                    || common.myStr(item["SectionsItalic"]) != ""
                                    || common.myStr(item["SectionsUnderline"]) != ""
                                    || common.myStr(item["SectionsFontSize"]) != ""
                                    || common.myStr(item["SectionsForecolor"]) != ""
                                    || common.myStr(item["SectionsListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                        }
                                    }
                                    BeginList3 = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                        }
                                    }
                                }

                                if (common.myStr(item["SectionsListStyle"]) == "3"
                                    || common.myStr(item["TemplateListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }
                                else
                                {
                                    if (str.Trim() != "")
                                    {
                                        objStrTmp.Append(str);
                                    }
                                }
                            }
                            else
                            {

                                if (t == 0)
                                {
                                    t = 1;
                                    if (common.myStr(item["TemplateListStyle"]) == "1")
                                    {
                                        BeginList = "<ul>"; EndList = "</ul>";
                                    }
                                    else if (common.myStr(item["TemplateListStyle"]) == "2")
                                    {
                                        BeginList = "<ol>"; EndList = "</ol>";
                                    }
                                }
                                if (common.myStr(item["TemplateBold"]) != ""
                                    || common.myStr(item["TemplateItalic"]) != ""
                                    || common.myStr(item["TemplateUnderline"]) != ""
                                    || common.myStr(item["TemplateFontSize"]) != ""
                                    || common.myStr(item["TemplateForecolor"]) != ""
                                    || common.myStr(item["TemplateListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    {
                                        if (sBegin.Contains("<br/>") == true)
                                        {
                                            sBegin = sBegin.Remove(0, 5);
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                        else
                                        {
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                    }
                                    if (sEntryType == "M")
                                    {
                                        objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                    }
                                    BeginList = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    {
                                        objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                    }
                                    if (sEntryType == "M")
                                    {
                                        objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                    }
                                }
                                if (common.myStr(item["TemplateListStyle"]) == "3"
                                    || common.myStr(item["TemplateListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }
                                objStrTmp.Append(EndList);
                                if (t2 == 0)
                                {
                                    t2 = 1;
                                    if (common.myStr(item["SectionsListStyle"]) == "1")
                                    {
                                        BeginList2 = "<ul>"; EndList3 = "</ul>";
                                    }
                                    else if (common.myStr(item["SectionsListStyle"]) == "2")
                                    {
                                        BeginList2 = "<ol>"; EndList3 = "</ol>";
                                    }
                                }
                                if (common.myStr(item["SectionsBold"]) != ""
                                    || common.myStr(item["SectionsItalic"]) != ""
                                    || common.myStr(item["SectionsUnderline"]) != ""
                                    || common.myStr(item["SectionsFontSize"]) != ""
                                    || common.myStr(item["SectionsForecolor"]) != ""
                                    || common.myStr(item["SectionsListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                    {
                                        if (str.Trim() != "") //add 19June2010
                                        {
                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                        }
                                    }
                                    BeginList2 = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                    {
                                        objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                    }
                                }
                                if (common.myStr(item["SectionsListStyle"]) == "3"
                                    || common.myStr(item["SectionsListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }

                                objStrTmp.Append(str);
                            }
                            iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                            iPrevId = common.myInt(item["TemplateId"]);
                        }
                    }
                }
            }
        }

        if (t2 == 1 && t3 == 1)
        {
            objStrTmp.Append(EndList3);
        }
        else
        {
            objStrTmp.Append(EndList);
        }

        if (GetPageProperty(iFormId) != null)
        {
            objStrSettings.Append(objStrTmp.ToString());
            sb.Append(objStrSettings.ToString());
        }
        else
        {
            sb.Append(objStrTmp.ToString());
        }
        if (objStrTmp.ToString().Length > 0)
        {
            literal.Text = objStrTmp.ToString();
            //RTF1.EditModes = Telerik.Web.UI.EditModes.All;
            //RTF1.Content =objStrTmp.ToString();
            //RTF1.Enabled = true;
        }
        else
        {
            //RTF1.Content = string.Empty;
            literal.Text = string.Empty;
        }
    }

    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sFontSize += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sFontSize += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sFontSize += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sFontSize += GetFontFamily(typ, item);
            };

            if (common.myStr(item[typ + "Bold"]) == "True")
            {
                sFontSize += " font-weight: bold;";
            }
            if (common.myStr(item[typ + "Italic"]) == "True")
            {
                sFontSize += " font-style: italic;";
            }
            if (common.myStr(item[typ + "Underline"]) == "True")
            {
                sFontSize += " text-decoration: underline;";
            }
        }

        return sFontSize;
    }
    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType, string sectionId)
    {
        StringBuilder objStr = new StringBuilder();
        DataView objDv = null;
        DataTable objDt = null;
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        if (objDs != null)
        {
            if (bool.Parse(TabularType) == true)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = objDs.Tables[0].Rows[0];
                    DataView dvValues = new DataView(objDs.Tables[1]);
                    dvValues.RowFilter = "FieldId='" + common.myStr(dr["FieldId"]) + "'";
                    MaxLength = dvValues.ToTable().Rows.Count;
                    DataTable dtFilter = dvValues.ToTable();
                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    DataView dvFilter = new DataView(dtFilter);
                    dvFilter.RowFilter = "RowCaption='0'";
                    DataTable dtNewTable = dvFilter.ToTable();
                    if (dtNewTable.Rows.Count > 0)
                    {
                        if (MaxLength != 0)
                        {


                            //objStr.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                            objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");


                            FieldsLength = objDs.Tables[0].Rows.Count;


                            if (common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != ""
                                && common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != "0")
                            {
                                objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >&nbsp;</th>");

                            }

                            for (int i = 0; i < FieldsLength; i++)   // it makes table
                            {
                                objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");

                                dr = objDs.Tables[0].Rows[i];
                                dvValues = new DataView(objDs.Tables[1]);
                                dvValues.RowFilter = "FieldId='" + dr["FieldId"].ToString() + "'";
                                dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

                                if (dvValues.ToTable().Rows.Count > MaxLength)
                                {
                                    MaxLength = dvValues.ToTable().Rows.Count;
                                }
                            }

                            objStr.Append("</tr>");
                            if (MaxLength == 0)
                            {
                                //objStr.Append("<tr>");
                                //for (int i = 0; i < FieldsLength; i++)
                                //{
                                //    objStr.Append("<td style=' " + sFontSize + "'> </td>");
                                //}
                                //objStr.Append("</tr></table>");
                            }
                            else
                            {
                                if (dsMain.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < MaxLength; i++)
                                    {
                                        objStr.Append("<tr>");
                                        if (common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) != ""
                                            && common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) != "0")
                                        {
                                            objStr.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) + "</td>");
                                        }
                                        //else
                                        //{
                                        //     objStr.Append("<td style=' " + sFontSize + "' align='right'>&nbsp;</td>");
                                        //}

                                        for (int j = 0; j < dsMain.Tables.Count; j++)
                                        {
                                            if (dsMain.Tables[j].Rows.Count > i
                                                && dsMain.Tables[j].Rows.Count > 0)
                                            {
                                                objStr.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dsMain.Tables[j].Rows[i]["TextValue"]) + "</td>");
                                            }
                                            else
                                            {
                                                objStr.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                            }
                                        }
                                        objStr.Append("</tr>");
                                    }
                                    objStr.Append("</table>");
                                }
                            }
                        }
                    }
                    else
                    {
                        Hashtable hstInput = new Hashtable();
                        hstInput.Add("@intTemplateId", iRootId);

                        if (common.myInt(Session["Gender"]) == 1)
                        {
                            hstInput.Add("chrGenderType", "F");
                        }
                        else if (common.myInt(Session["Gender"]) == 2)
                        {
                            hstInput.Add("chrGenderType", "M");
                        }
                        else
                        {
                            hstInput.Add("chrGenderType", "M");
                        }

                        hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
                        hstInput.Add("@intSecId", Convert.ToInt32(sectionId));

                        //Added by rakesh start
                        hstInput.Add("@OrderDetailId", Request.QueryString["sOrdDtlId"].ToString());
                        //Added by rakesh start

                        DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                        DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);

                        dvRowCaption.RowFilter = "RowCaptionId>0";
                        if (dvRowCaption.ToTable().Rows.Count > 0)
                        {
                            StringBuilder sbCation = new StringBuilder();
                            dvRowCaption.RowFilter = "RowNum>0";
                            DataTable dt = dvRowCaption.ToTable();
                            if (dt.Rows.Count > 0)
                            {
                                sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                int column = dt.Columns.Count;
                                int ColumnCount = 0;
                                int count = 1;
                                //Commented by rakesh because caption tabular template showing last column missiong start
                                //for (int k = 1; k < (column - 5); k++)
                                //Commented by rakesh because caption tabular template showing last column missiong start

                                //Added by rakesh because caption tabular template showing last column missiong start
                                for (int k = 1; k < (column - 4); k++)
                                //Added by rakesh because caption tabular template showing last column missiong start

                                {
                                    if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
                                        && ColumnCount == 0)
                                    {
                                        sbCation.Append("<td>");
                                        sbCation.Append(" + ");
                                        sbCation.Append("</td>");
                                    }
                                    else
                                    {
                                        sbCation.Append("<td>");
                                        sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                        sbCation.Append("</td>");
                                        count++;
                                    }
                                    ColumnCount++;
                                }
                                sbCation.Append("</tr>");

                                DataView dvRow = new DataView(dt);
                                dvRow.RowFilter = "RowCaptionId>0";
                                DataTable dtRow = dvRow.ToTable();
                                for (int l = 1; l <= dtRow.Rows.Count; l++)
                                {
                                    sbCation.Append("<tr>");
                                    for (int i = 0; i < ColumnCount; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dt.Rows[l + 1]["RowCaptionName"]) + "</td>");
                                        }
                                        else
                                        {
                                            if (dt.Rows[1]["Col" + i].ToString() == "D")
                                            {
                                                DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    dvDrop.RowFilter = "ValueId=" + dt.Rows[l + 1]["Col" + i].ToString();
                                                    if (dvDrop.ToTable().Rows.Count > 0)
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                            else
                                            {
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
                                                }
                                                else
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                        }

                                    }
                                    sbCation.Append("</tr>");
                                }
                                sbCation.Append("</table>");
                            }
                            objStr.Append(sbCation);
                        }

                    }
                }
            }
            else // For Non Tabular Templates
            {
                string BeginList = "", EndList = "";
                string sBegin = "", sEnd = "";
                int t = 0;
                string sStaticTemplate = "";
                foreach (DataRow item in objDs.Tables[0].Rows)
                {
                    objDv = new DataView(objDs.Tables[1]);
                    objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                    objDt = objDv.ToTable();
                    if (t == 0)
                    {
                        t = 1;
                        if (common.myStr(item["FieldsListStyle"]) == "1")
                        {
                            BeginList = "<ul>"; EndList = "</ul>";
                        }
                        else if (item["FieldsListStyle"].ToString() == "2")
                        {
                            BeginList = "<ol>"; EndList = "</ol>";
                        }
                    }
                    if (common.myStr(item["FieldsBold"]) != ""
                        || common.myStr(item["FieldsItalic"]) != ""
                        || common.myStr(item["FieldsUnderline"]) != ""
                        || common.myStr(item["FieldsFontSize"]) != ""
                        || common.myStr(item["FieldsForecolor"]) != ""
                        || common.myStr(item["FieldsListStyle"]) != "")
                    {
                        //rafat1
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);
                            if (Convert.ToBoolean(item["DisplayTitle"]))
                            {
                                objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));

                                // 28/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                if (objStr.ToString() != "")
                                {
                                    objStr.Append(sEnd + "</li>");
                                }
                            }
                            BeginList = "";
                            sBegin = "";
                        }

                    }
                    else
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            if (sStaticTemplate != "<br/><br/>")
                            {
                                objStr.Append(common.myStr(item["FieldName"]));
                            }
                        }
                    }
                    if (objDs.Tables.Count > 1)
                    {
                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        objDt = objDv.ToTable();
                        DataView dvFieldType = new DataView(objDs.Tables[0]);
                        dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                        for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                //objStr.Append(": " + objDt.Rows[0]["PropertyValue"].ToString());
                                string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                if (FType == "C")
                                {
                                    FType = "C";
                                }
                                if (FType == "C" || FType == "D" || FType == "B")
                                {
                                    if (FType == "B")
                                    {
                                        if (common.myStr(objDt.Rows[i]["FieldValue"]) == "1"
                                            || common.myStr(objDt.Rows[i]["FieldValue"]) == "0")
                                        {

                                            DataView dv1 = new DataView(objDs.Tables[1]);
                                            if (common.myStr(objDs.Tables[1].Rows[i]["FieldValue"]) == "1")
                                            {
                                                dv1.RowFilter = "TextValue='Yes'";
                                            }
                                            else
                                            {
                                                dv1.RowFilter = "TextValue='No'";
                                            }

                                            DataTable dt1 = dv1.ToTable();
                                            if (dt1.Rows.Count > 0)
                                            {
                                                if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != "")
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                    else
                                                    {
                                                        objStr.Append(", " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                    else
                                                    {
                                                        objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            BindDataValue(objDs, objDt, objStr, i, FType);//check1
                                        }
                                    }
                                    else
                                    {
                                        BindDataValue(objDs, objDt, objStr, i, FType);
                                    }
                                }
                                else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                    else
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                }
                                //else if (FType == "L")
                                //{
                                //    objStr.Append(BindStaticTemplates(Convert.ToInt32(objDt.Rows[0]["StaticTemplateId"]), Convert.ToInt32(objDt.Rows[0]["FieldId"])));
                                //}
                                if (common.myStr(item["FieldsListStyle"]) == "")
                                {
                                    if (ViewState["iTemplateId"].ToString() != "163")
                                    {
                                        if (FType != "C")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                    else
                                    {
                                        if (FType != "C" && FType != "T")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                }
                            }
                        }
                        // Cmt 25/08/2011
                        //if (objDt.Rows.Count > 0)
                        //{
                        //    if (objStr.ToString() != "")
                        //        objStr.Append(sEnd + "</li>");
                        //}
                    }
                    //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
                }

                if (objStr.ToString() != "")
                {
                    objStr.Append(EndList);
                }
            }
        }

        return objStr.ToString();
    }

    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            {
                sBegin += "<br/>";
            }
            else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            {
                sBegin += "; ";
            }
            else
            {
                sBegin += "<br/>";
            }
        }

        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }
        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }
    protected DataSet GetPageProperty(string iFormId)
    {
        if (Session["HospitalLocationID"] != null
            && iFormId != "")
        {
            if (Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"] == null)
            {
                hstInput = new Hashtable();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hstInput.Add("@intFormId", iFormId);
                DataSet ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                return ds;
            }
            else
            {
                DataSet objDs = (DataSet)Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"];
                return objDs;
            }
        }
        return null;
    }
    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", common.myStr(Session["HospitalLocationId"]));
        if (FieldValue != "")
        {
            sFontSize = fonts.GetFont("Size", FieldValue);
            if (sFontSize != "")
            {
                sFontSize = " font-size: " + sFontSize + ";";
            }
        }
        return sFontSize;
    }

    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FontName = fonts.GetFont("Name", common.myStr(item[typ + "FontStyle"]));
        if (FontName != "")
        {
            sBegin += " font-family: " + FontName + ";";
        }
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myStr(Session["HospitalLocationId"]));
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                {
                    sBegin += " font-family: " + FontName + ";";
                }
            }
        }

        return sBegin;
    }


    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType)
    {
        if (i == 0)
        {
            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
        }
        else
        {
            if (FType != "C")
            {
                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
            }
            else
            {
                if (i == 0)
                {
                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                }
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                {
                    objStr.Append(" and " + common.myStr(objDt.Rows[i]["TextValue"]) + ".");
                }
                else
                {
                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                }
            }
        }
        //}
    }


    protected void ddlTemplateMain_SelectedIndexChanged(object sender, EventArgs e)
    {

        StringBuilder sbTemp = new StringBuilder();

        bindData("1", common.myStr(ddlTemplateMain.SelectedValue), sbTemp);
    }

    private void PatientInfo(string RegNo)
    {
        try
        {
            if (RegNo != "")
            {
                DataSet ds = new DataSet();
                string sXSL = "";
                BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
                //txtAccountNo.Text = objCM.GetPatientRegistrationNo(Convert.ToInt32(txtRegNo.Text.Trim()));
                ds = objCM.getPatientDetails(Convert.ToInt64(RegNo), 1, Convert.ToInt32(Session["HospitalLocationID"]));
                sXSL = "/include/xsl/PatientInfo-US.xsl";
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() != "Patient Is Expired, No Transaction Allowed!")
                        {
                            xmlPatientInfo.DocumentContent = ds.Tables[0].Rows[0][0].ToString();
                            xmlPatientInfo.TransformSource = sXSL;
                            xmlPatientInfo.DataBind();
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = ds.Tables[0].Rows[0][0].ToString();
                            xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                            xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                            xmlPatientInfo.DataBind();
                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("No patient record found ", Page);
                        xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                        xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                        xmlPatientInfo.DataBind();
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("No patient record found ", Page);
                    xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                    xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                    xmlPatientInfo.DataBind();
                }
            }
            else
            {
                xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                xmlPatientInfo.DataBind();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


}
