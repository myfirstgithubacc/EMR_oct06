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
using Telerik.Web.UI;
using Telerik.Charting;
using Telerik.Charting.Styles;
using System.Drawing;
using System.Text;
using System.IO;
using System.Drawing.Imaging;

public partial class LIS_Phlebotomy_LabResultGraph : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;
    public string sUHID = string.Empty;
    public string sPatientName=string.Empty;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["AllData"] = null;
            objval = new BaseC.clsLISPhlebotomy(sConString);

            dtpfromDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();
            dtpToDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();

            ViewState["VitalName"] = common.myStr("Lab Result Graph");

            //bindTemplate();

            btnFilter_Click(sender, e);
            getPataientDetails();
            BindPatientHeader();

        }
        
        
    }
    private void BindPatientHeader()
    {
        clsIVF objIVF = new clsIVF(sConString);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            string Patientheader = string.Empty;
            ds = objEMR.getReportFormatDetails(common.myInt(Session["DoctorId"]));

            if (common.myStr(Request.QueryString["RegistrationId"]) != "" && common.myStr(Session["RegistrationId"]) == "")
            {
                Session["RegistrationId"] = Request.QueryString["RegistrationId"];
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                Patientheader = objIVF.getCustomizedPatientReportHeader(common.myInt(ds.Tables[0].Rows[0]["headerid"]));
            }
            else
            {
                Patientheader = objIVF.getCustomizedPatientReportHeader(1);

            }
            Patientheader = Patientheader.Replace("10pt", "12pt;padding-top: 0px;");
            string GraphHeader = "<table width='100%' style='border-collapse:collapse;font-size:18px'><tr><td align ='center'><strong>INVESTIGATION RESULT GRAPH</string></td></tr></table> ";
            litItem.Text =GraphHeader+ Patientheader;

        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objIVF = null;
            objEMR = null;
            ds.Dispose();
        }



    }

    public void getPataientDetails()
    {
        try
        {
            //
            if (Request.QueryString["RegNo"] != null)
            {
                BaseC.Patient objMaster = new BaseC.Patient(sConString);
                
                DataSet objds = objMaster.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]), 
                    common.myStr(Request.QueryString["RegNo"]),"", common.myInt(Session["UserId"]), 0);
                if (objds.Tables.Count > 0)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {

                        lblPatientName.Text = common.myStr(objds.Tables[0].Rows[0]["PatientName"]);
                        lblUHID.Text = common.myStr(Request.QueryString["RegNo"]);
                        lblAgeGender.Text = common.myStr(objds.Tables[0].Rows[0]["Age"]) + "/" + common.myStr(objds.Tables[0].Rows[0]["Gender"]);

                        //txtDob.Text = common.myStr(objds.Tables[0].Rows[0]["DateofBirth"]);
                        //txtMartialStatus.Text = common.myStr(objds.Tables[0].Rows[0]["MaritalStatus"]);
                        //txtGender.Text = common.myStr(objds.Tables[0].Rows[0]["Gender"]);

                    }
                    else
                    {
                        //lblUHID.Text = common.myStr(Request.QueryString["RegNo"]);
                        //lblAgeGender.Text = common.myStr(Request.QueryString["AgeInDays"]);
                        //Label4.Visible = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }

    }
    protected void RadChart1_DataBound(object sender, EventArgs e)
    {
        //HideLabels(RadChart1);
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        ViewState["VitalName"] = common.myStr(ddlField.SelectedValue);
        bindTemplate();
        bindGraph();        
        RadChart1.Visible = true;
    }

    protected void ddlgraphtype_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        bindGraph();
    }

    protected void ddlField_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        ViewState["VitalName"] = common.myStr(ddlField.SelectedValue);
        bindGraph();
    }

    protected void ddldateRange_OnSelectedIndexChanged(Object sender, EventArgs e)
    {

        if (ddldateRange.SelectedValue == "6")
        {
            pnlDatarng.Visible = true;


            dtpfromDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

            dtpToDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpToDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpToDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
        }
        else
        {
            pnlDatarng.Visible = false;

            dtpfromDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromDate.DateInput.Text = "";
            dtpfromDate.SelectedDate = null;

            dtpToDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpToDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpToDate.DateInput.Text = "";
            dtpToDate.SelectedDate = null;
        }
        bindGraph();
    }

    protected void bindTemplate()
    {
        try
        {
            string[] ageGender = common.myStr(Request.QueryString["AgeInDays"]).Split('/');
            string StatusCode = common.myStr(Request.QueryString["StatusCode"]);
            ViewState["StatusCode"] = StatusCode;
            string ServiceId = common.myStr(Request.QueryString["SERVICEID"]);
            string DIAG_SAMPLEID = common.myStr(Request.QueryString["DIAG_SAMPLEID"]);
            string SOURCE = common.myStr(Request.QueryString["SOURCE"]);
            string sFieldId = "";
            if (common.myStr(Request.QueryString["FieldId"]) != "")
                sFieldId = common.myStr(Request.QueryString["FieldId"]);

            string DIAG_SAMPLEID_S = common.myStr(Request.QueryString["DIAG_SAMPLEID_S"]);
            string sRegNo = common.myStr(Request.QueryString["RegNo"]);
            if (DIAG_SAMPLEID_S == "")
            {
                DIAG_SAMPLEID_S = DIAG_SAMPLEID;
            }

            ViewState["DIAG_SAMPLEID_S"] = DIAG_SAMPLEID_S;


            DataTable dt = new DataTable();
            objval = new BaseC.clsLISPhlebotomy(sConString);
            //Checking Whether the Global List collLabTemplate Is Stored in ViewState or Not

            /***************** get data for fields *****************/

            DataSet objDs = getLabNoInvFormatsHistoryForGraph(common.myStr(SOURCE),
                            common.myInt(Session["FacilityID"]),
                            common.myInt(DIAG_SAMPLEID),
                            common.myInt(ServiceId),
                            common.myStr(StatusCode),
                            common.myInt(Session["HospitalLocationId"]),
                            common.myInt(sRegNo),
                            common.myStr(ddldateRange.SelectedValue), ddldateRange.SelectedValue == "6" ? dtpfromDate.SelectedDate.Value.ToString("yyyy/MM/dd") : "",
                            ddldateRange.SelectedValue == "6" ? dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd") : ""

                            );


            ViewState["AllData"] = objDs;

            ddlField.Items.Clear();

            //Numeric	N--
            //Text Single Line	T
            //Text MultiLine	M
            //Text Formats	W
            //List of Choices (Single Selection)	D
            //Heading	H
            //Tabular	TA
            //Formula	F--
            //List of Choices (Multi Selection)	C
            //Organisms	O
            //Enzymes	E
            //Sensitivity	SN
            //Time	TM

            if (objDs.Tables.Count > 0)
            {
                foreach (DataRow DR in objDs.Tables[0].Rows)
                {
                    if (common.myStr(DR["FieldType"]) == "N"
                        || common.myStr(DR["FieldType"]) == "F")
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = common.myStr(DR["FieldName"]);
                        item.Value = common.myStr(DR["FieldId"]);

                        this.ddlField.Items.Add(item);
                        item.DataBind();

                        ddlField.DataBind();
                    }
                }
                if (common.myInt(sFieldId) > 0)
                {
                    ddlField.SelectedValue = sFieldId.ToString();
                    ddlField.Enabled = false;
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

    void bindGraph()
    {
        try
        {
            DataSet ds = new DataSet();

            RadChart1.Clear();
            if (common.myStr(ddlgraphtype.SelectedValue) == "Line")
            {
                RadChart1.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            }
            else
            {
                RadChart1.DefaultType = Telerik.Charting.ChartSeriesType.Bar;
            }

            RadChart1.Legend.Items.Clear();
            RadChart1.ClearSkin();
            // RadChart1.Skin = ddlSkin.SelectedValue.ToString();
            RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

            //ds = objv.VitalGraph(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            //            common.myInt(Session["RegistrationId"]), common.myStr(ViewState["VitalName"]),
            //            common.myStr(ddldateRange.SelectedValue), ddldateRange.SelectedValue == "6" ? dtpfromDate.SelectedDate.Value.ToString("yyyy/MM/dd") : "",
            //            ddldateRange.SelectedValue == "6" ? dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd") : "");

            if (ViewState["AllData"] != null)
            {
                ds = (DataSet)ViewState["AllData"];
            }

            if (ds.Tables.Count > 0)
            {
                DataView DV = ds.Tables[1].DefaultView;
                DV.RowFilter = "FieldId=" + common.myInt(ddlField.SelectedValue);
                    //"DiagSampleId IN (" + common.myStr(ViewState["DIAG_SAMPLEID_S"]) + ") " +
                                //" AND FieldId=" + common.myInt(ddlField.SelectedValue);
                DataTable DT = DV.ToTable(false, new string[] { "FieldValue", "EntryDate" });

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ddlField.SelectedItem != null)
                    {
                        DataView dv1 = ds.Tables[0].DefaultView;
                        dv1.RowFilter = "FieldId=" + common.myInt(ddlField.SelectedValue) +"";
                        DataTable dt0 = dv1.ToTable();

                        if (common.myStr(dt0.Rows[0]["UnitName"].ToString()) != "")
                        {
                            RadChart1.ChartTitle.TextBlock.Text = common.myStr(ddlField.SelectedItem.Text) + " (" + common.myStr(dt0.Rows[0]["UnitName"].ToString()) + ")";
                        }
                        else
                        {
                            RadChart1.ChartTitle.TextBlock.Text = common.myStr(ddlField.SelectedItem.Text);
                        }

                        if (common.myStr(dt0.Rows[0]["MinValue"].ToString()) != "" && common.myStr(dt0.Rows[0]["MaxValue"].ToString()) != "")
                        {
                            RadChart1.ChartTitle.TextBlock.Text += " (" + common.myDbl(dt0.Rows[0]["MinValue"]).ToString("F2") + common.myStr(dt0.Rows[0]["Symbol"]) + common.myDbl(dt0.Rows[0]["MaxValue"]).ToString("F2") + ")";
                        }
                    }

                    RadChart1.DataSource = DT;
                    RadChart1.PlotArea.XAxis.DataLabelsColumn = "EntryDate";
                    RadChart1.DataBind();
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


    public DataSet getLabNoInvFormatsHistoryForGraph(string Source, int iLoginFacilityId,
          int iDiagSampleId, int iServiceID, string StatusCode, int iHospID, Int64 iRegNo, string sDateRange, string sDateFrom, string sDateTo)
    {
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        HshIn.Add("@chrSource", Source);
        HshIn.Add("@inyHospitalLocationId", iHospID);
        HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
        HshIn.Add("@intDiagSampleId", iDiagSampleId);
        HshIn.Add("@intServiceID", iServiceID);
        HshIn.Add("@chvCode", StatusCode);
        HshIn.Add("@intRegistrationNo", iRegNo);
        HshIn.Add("@chvDateRange", sDateRange);
        HshIn.Add("@chvDateFrom", sDateFrom);
        HshIn.Add("@chvDateTo", sDateTo);
        DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabNoInvFormatsForGraph", HshIn);
        return ds;
    }
    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        try
        {
            string graphType = "L";

            switch (common.myStr(ddlgraphtype.SelectedValue))
            {
                case "Line":
                    graphType = "L";
                    break;
                case "Bar":
                    graphType = "B";
                    break;
            }



            string fromDate = (ddldateRange.SelectedValue == "6") ? dtpfromDate.SelectedDate.Value.ToString("yyyy/MM/dd") : "";
            string toDate = (ddldateRange.SelectedValue == "6") ? dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd") : "";

            //RadWindow1.NavigateUrl = "PrintLabResultGraph.aspx?VitalName=" + common.myStr(ddlField.SelectedValue) +
            //                        "&GraphType=" + graphType + "&ServiceId=" + common.myStr(Request.QueryString["SERVICEID"]) +
            //                        "&DiagSamId="+common.myStr(Request.QueryString["DIAG_SAMPLEID"])+ "&FieldId="+ common.myStr( ddlField.SelectedValue) +
            //                        "&Code=" + common.myStr(Request.QueryString["StatusCode"]) + "&Source=" + common.myStr(Request.QueryString["SOURCE"])
            //                        + "&HeadName=" + ddlField.SelectedItem.Text;

            RadWindow1.NavigateUrl = "PrintLabResultGraph.aspx?VitalName=" + common.myStr(ddlField.SelectedValue) +
                                 "&GraphType=" + graphType + "&ServiceId=" + common.myStr(Request.QueryString["SERVICEID"]) +
                                 "&DiagSamId=" + common.myStr(ViewState["DIAG_SAMPLEID_S"]) + "&FieldId=" + common.myStr(ddlField.SelectedValue) +
                                 "&Code=" + common.myStr(Request.QueryString["StatusCode"]) + "&Source=" + common.myStr(Request.QueryString["SOURCE"])
                                 + "&HeadName=" + ddlField.SelectedItem.Text;


            RadWindow1.Height = 580;
            RadWindow1.Width = 1000;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            //RadWindow1.Title = "Time Slot";
            // RadWindow1.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;


            //MemoryStream imageStream = new MemoryStream();

            //RadChart1.Save(imageStream, ImageFormat.Png);

            //Byte[] imageContent = new Byte[imageStream.Length];
            //imageStream.Position = 0;
            //imageStream.Read(imageContent, 0, (Int32)imageStream.Length);

            //Response.Clear();
            //Response.ContentType = "image/png";
            ////Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.MapPath("~/images/ChartPrint.png"));
            //Response.BufferOutput = true;
            //Response.AddHeader("Content-Length", imageStream.Length.ToString());
            //Response.BinaryWrite(imageContent);

            //imageStream.Close();
            //Response.End();

            //export();
        }
        catch
        {
        }
    }

}
