using System;
using System.Web.UI;
using System.Data;
using Telerik.Charting;
using Telerik.Web.UI;
using Telerik.Charting.Styles;
using System.Drawing;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json;
using System.Text;

public partial class EMR_Vitals_GrowthChart : System.Web.UI.Page
{
   // String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    clsExceptionLog objException = new clsExceptionLog();

    int YAxisCapInterval;
    int YCapMaxVal;
    int YAxisCapMinValue;
    decimal YAxisInterValue;
    int YAxisCapStartvalue; 
    decimal YMax;

    int XCapinterval;
    int Xinterval;
    int XMinVal;
    int XMaxval;


    int YAxisCapInterval2;
    int YCapMaxVal2;
    int YAxisCapMinValue2;
    decimal YAxisInterValue2;
    int YAxisCapStartvalue2;
    decimal YMax2;

    int XCapinterval2;
    int Xinterval2;
    int XMinVal2;
    int XMaxval2;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["MP"]).ToUpper().Equals("NO"))
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            try
            {


                //if (Request.QueryString["FromEMR"] != null)
                //{
                //    if (common.myStr(Request.QueryString["FromEMR"]) != string.Empty)
                //    {
                //        if (common.myStr(Request.QueryString["FromEMR"]).Equals("1"))
                //        {
                //            asplHeaderUDEMR.Visible = true;
                //            asplHeaderUD.Visible = false;
                //        }
                //        else
                //        {
                //            asplHeaderUDEMR.Visible = false;
                //            asplHeaderUD.Visible = true;
                //        }

                //    }
                //    else
                //    {
                //        asplHeaderUDEMR.Visible = false;
                //        asplHeaderUD.Visible = true;
                //    }

                //}
                //else
                //{
                //    asplHeaderUDEMR.Visible = false;
                //    asplHeaderUD.Visible = true;
                //}


                if (Session["RegistrationId"] == null)
                {
                    Response.Redirect("/default.aspx?RegNo=0", false);
                    return;
                }
                if (common.myStr(Request.QueryString["MP"]).ToUpper().Equals("NO"))
                {
                    btnHistory.Visible = false;
                    btnVital.Visible = false;
                    //btnClose.Visible = true;

                }
                else
                {
                    btnHistory.Visible = true;
                    btnVital.Visible = true;
                    ///btnClose.Visible = false;
                }
                // BindGrowthChartCombo();
                ddlGrowthChartType.SelectedIndex = 0;
                ddlGrowthChartType_OnSelectedIndexChanged(this, null);
                // RadChart1.Visible = false;
                GetAge();
                BindCombo();
                if (common.myStr(Request.QueryString["CloseButtonShow"]) == "No")
                {
                    // btnClose.Visible = false;
                }

                setChartColor();
            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;
                objException.HandleException(Ex);
            }
        }

    }

    protected void cmdFilter_OnClick(object sender, EventArgs e)
    {
        try
        {
            //if (common.myInt(ddlGrowthChartType.SelectedValue) == 0)
            //{
            //    lblMessage.Text = "Please Select Growth chart type !";
            //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //    ddlGrowthChartType.Focus();
            //    return;
            //}
            lblMessage.Text = string.Empty;
            RadChart1.Visible = true;
            RadChart1.DataSource = null;
            RadChart1.DataBind();
            RadChart2.DataSource = null;
            RadChart2.DataBind();
            ViewState["xaxisTypePlot"] = "M";
            if (!common.myStr(ddlGrowthChartType2.SelectedValue).Equals(string.Empty)
                && !common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("2-19 YEARS")
                && !common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("0-24 MONTHS")
                && !common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("0-6 MONTHS")
                && !common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("0-13 WEEKS")
                && !common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("2-5 YEARS 1")
                && !common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("0-2 YEARS 1")
                 )
            {
                ViewState["FDays"] = 0;
                ViewState["TDays"] = 60;
                ViewState["xaxisType"] = "M";

                if (!common.myStr(ddlGrowthChart2.SelectedValue).Equals(string.Empty))
                {
                    // For Birth 23 to 42 weeks (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    // BindGrowthChart2Yto20Y_Dynamic();
                }
                else
                {
                    //For Birth 23 to 42 weeks  (Height for Weight) growth charts
                    BindGrowthChartFor2Yto20YHTWT();
                }
            }
            else if (common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("2-19 YEARS"))
            {
                ViewState["FDays"] = 2;
                ViewState["TDays"] = 19;
                ViewState["xaxisType"] = "Y";
                ViewState["xaxisTypePlot"] = "Y";

                RadChart1.Chart.DefaultType = ChartSeriesType.Area;
                if (common.myStr(ddlGrowthChart2.SelectedValue).Equals("BMI"))
                {
                    // For Birth 23 to 42 weeks (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    BindGrowthChart2Yto19YBMI();
                    RadChart2.Visible = false;
                }
                else
                {
                    //For Birth 23 to 42 weeks  (Height for Weight) growth charts
                    RadChart2.Visible = true;

                    BindGrowthChartBirthto2YHT();
                    ViewState["TDays"] = 10;
                    BindGrowthChart2To10YWT();
                }
            }

            else if (common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("0-24 MONTHS"))
            {
                ViewState["FDays"] = 0;
                ViewState["TDays"] = 24;
                ViewState["xaxisType"] = "M";
                RadChart1.Visible = false;
                RadChart2.Visible = false;
                RadChart1.Clear();
                RadChart1.DataSource = null;
                RadChart1.DataBind();
                RadChart2.Clear();
                RadChart2.DataSource = null;
                RadChart2.DataBind();

                if (common.myStr(ddlGrowthChart2.SelectedValue).Equals("HT"))
                {
                    // For Birth 23 to 42 weeks (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    RadChart1.Visible = true;
                    RadChart2.Visible = true;
                    BindGrowthChartBirthto2YHT();
                    BindGrowthChartBirthTo2YWT();


                    //BindGrowthChartBirthTo2YWT();
                }
                else if (common.myStr(ddlGrowthChart2.SelectedValue).Equals("HC"))
                {
                    RadChart1.Visible = true;
                    RadChart2.Visible = true;
                    //For Birth 23 to 42 weeks  (Height for Weight) growth charts
                    BindGrowthChartBirthto2YHT();
                    BindGrowthChart45To110CMWT();
                }
                else
                {
                    RadChart1.Visible = false;
                    RadChart2.Visible = true;
                    BindGrowthChartBirthTo2YWT();
                }
            }
            else if (common.myStr(ddlGrowthChartType.SelectedValue).Equals("2"))
            {
                ViewState["FDays"] = 0;
                ViewState["TDays"] = 26;
                ViewState["xaxisType"] = "W";

                if (!common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HW"))
                {
                    // For Birth to 36 months (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    // BindGrowthChart2Yto20Y();
                }
                else
                {
                    //For Birth to 36 months (Height for Weight) growth charts
                    BindGrowthChartFor2Yto20YHTWT();
                }

            }
            else if (common.myStr(ddlGrowthChartType.SelectedValue).Equals("3"))
            {
                ViewState["FDays"] = 0;
                ViewState["TDays"] = 52;
                ViewState["xaxisType"] = "W";

                if (!common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HW"))
                {
                    // For Birth to 36 months (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    // BindGrowthChart2Yto20Y();
                }
                else
                {
                    //For Birth to 36 months (Height for Weight) growth charts
                    BindGrowthChartFor2Yto20YHTWT();
                }
            }
            else if (common.myStr(ddlGrowthChartType.SelectedValue).Equals("4"))
            {
                ViewState["FDays"] = 12;
                ViewState["TDays"] = 48;
                ViewState["xaxisType"] = "M";

                if (!common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HW"))
                {
                    //For Birth to 36 months (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    //BindGrowthChart2Yto20Y();
                }
                else
                {
                    //For Birth to 36 months (Height for Weight) growth charts
                    BindGrowthChartFor2Yto20YHTWT();
                }
            }

            #region Weekly Chart / Other Chart WO
            if (common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("0-13 WEEKS")) // Weekly Chart
            {
                //ViewState["FDays"] = 0;
                //ViewState["TDays"] = 26;
                //ViewState["xaxisType"] = "W";

                //if (!common.myStr(ddlGrowthChart2.SelectedValue).Equals(string.Empty))
                //{
                //    // For Birth 23 to 42 weeks (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                //    BindGrowthChart2Yto20Y_Dynamic();
                //}
                //else
                //{
                //    //For Birth 23 to 42 weeks  (Height for Weight) growth charts
                //    BindGrowthChartFor2Yto20YHTWT();
                //}


                ViewState["FDays"] = 0;
                ViewState["TDays"] = 60;
                ViewState["xaxisType"] = "M";
                ViewState["xaxisTypePlot"] = "WO";

                if (!common.myStr(ddlGrowthChart2.SelectedValue).Equals(string.Empty))
                {
                    // For Birth 23 to 42 weeks (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    // BindGrowthChart2Yto20Y_Dynamic();
                }
                else
                {
                    //For Birth 23 to 42 weeks  (Height for Weight) growth charts
                    BindGrowthChartFor2Yto20YHTWT();
                }
            }

            else if (common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("0-6 MONTHS")) // Weekly Chart
            {

                ViewState["FDays"] = 0;
                ViewState["TDays"] = 60;
                ViewState["xaxisType"] = "M";
                ViewState["xaxisTypePlot"] = "WO";

                if (!common.myStr(ddlGrowthChart2.SelectedValue).Equals(string.Empty))
                {
                    // For Birth 23 to 42 weeks (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    //BindGrowthChart2Yto20Y_Dynamic();
                }
                else
                {
                    //For Birth 23 to 42 weeks  (Height for Weight) growth charts
                    BindGrowthChartFor2Yto20YHTWT();
                }
            }

            else if (common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("2-5 YEARS 1"))
            {
                ViewState["FDays"] = 24;
                ViewState["TDays"] = 60;
                ViewState["xaxisType"] = "M";
                ViewState["xaxisTypePlot"] = "WO";

                if (!common.myStr(ddlGrowthChart2.SelectedValue).Equals(string.Empty))
                {
                    // For Birth 23 to 42 weeks (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    //BindGrowthChart2Yto20Y_Dynamic();
                }
                else
                {
                    //For Birth 23 to 42 weeks  (Height for Weight) growth charts
                    BindGrowthChartFor2Yto20YHTWT();
                }
            }

            else if (common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("0-2 YEARS 1"))
            {
                ViewState["FDays"] = 0;
                ViewState["TDays"] = 24;
                ViewState["xaxisType"] = "M";
                ViewState["xaxisTypePlot"] = "WO";

                if (!common.myStr(ddlGrowthChart2.SelectedValue).Equals(string.Empty))
                {
                    // For Birth 23 to 42 weeks (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    //BindGrowthChart2Yto20Y_Dynamic();
                }
                else
                {
                    //For Birth 23 to 42 weeks  (Height for Weight) growth charts
                    BindGrowthChartFor2Yto20YHTWT();
                }
            }
            //else if(common.myStr(ddlGrowthChartType2.SelectedValue).ToUpper().Equals("0-13 WEEKS"))
            //{
            //    ViewState["FDays"] = 0;
            //    ViewState["TDays"] = 60;
            //    ViewState["xaxisType"] = "M";
            //    ViewState["xaxisTypePlot"] = "W";

            //    if (!common.myStr(ddlGrowthChart2.SelectedValue).Equals(string.Empty))
            //    {
            //        // For Birth 23 to 42 weeks (Length for Age,Weight for Age,Head Circumference for Age) growth charts
            //        BindGrowthChart2Yto20Y_Dynamic();
            //    }
            //    else
            //    {
            //        //For Birth 23 to 42 weeks  (Height for Weight) growth charts
            //        BindGrowthChartFor2Yto20YHTWT();
            //    }
            //}

            #endregion


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void GetAge() // Get Patient Age in days
    {
       // BaseC.GrowthChart objbc = new BaseC.GrowthChart(sConString);
        DataSet objDs = new DataSet();
        try
        {
            ViewState["Age"] = null;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetAgeinDays";
            APIRootClass.GrowthChart objRoot = new global::APIRootClass.GrowthChart();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            //objDs = objbc.GetAgeinDays(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]));

            if (objDs.Tables[0].Rows.Count > 0)
            {
                ViewState["Age"] = common.myStr(objDs.Tables[0].Rows[0]["Days"]).Trim();
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
            //objbc = null;
            objDs.Dispose();
        }
    }

    void BindGrowthChartBirth23To36weeks() // For Birth 23 to 42 weeks (Length for Age,Weight for Age,Head Circumference for Age) growth charts   
    {
       // BaseC.GrowthChart objbc = new BaseC.GrowthChart(sConString);
        DataSet objDs = new DataSet();
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindGrowthChartBirthTo36Months";
            APIRootClass.GrowthChart objRoot = new global::APIRootClass.GrowthChart();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.DisplayName = common.myStr(ddlGrowthChart2.SelectedValue);
            objRoot.Measurmentsystem = common.myStr(ddlMsystem.SelectedValue);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            //objDs = objbc.BindGrowthChartBirthTo36Months(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
            //       common.myStr(ddlGrowthChart2.SelectedValue), common.myStr(ddlMsystem.SelectedValue));

            RadChart1.Clear();
            RadChart1.DefaultType = Telerik.Charting.ChartSeriesType.Line;

            RadChart1.Legend.Items.Clear();
            RadChart1.ClearSkin();
            RadChart1.Skin = common.myStr(ddlSkin.SelectedValue);
            //RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 270;

            ChartSeries PatientValue = new ChartSeries("Patient Value");
            PatientValue.Type = ChartSeriesType.Line;
            PatientValue.DataYColumn = "PatientValue";
            PatientValue.DataLabelsColumn = "PatientValue";

            RadChart1.Series.Add(PatientValue);
            PatientValue.Appearance.LabelAppearance.Visible = true;
            PatientValue.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
            PatientValue.Appearance.EmptyValue.Line.Color = System.Drawing.Color.Transparent;
            PatientValue.Appearance.EmptyValue.PointMark.Visible = false;

            PatientValue.Appearance.PointMark.Visible = true;
            PatientValue.Appearance.PointMark.Border.Width = 2;
            PatientValue.Appearance.PointMark.Border.Color = System.Drawing.Color.Black;
            PatientValue.Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue.Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.DarkGray;
            PatientValue.Appearance.PointMark.FillStyle.FillType = FillType.Solid;

            PatientValue.Appearance.LineSeriesAppearance.Color = Color.Black;
            PatientValue.Appearance.LineSeriesAppearance.Width = 1;
            PatientValue.Appearance.TextAppearance.TextProperties.Color = Color.Black;

            RadChart1.PlotArea.XAxis.DataLabelsColumn = "AgeYears";
            RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = common.myStr(ddlGrowthChart2.SelectedItem.Text);

            if (objDs.Tables[0].Rows.Count > 0)
            {
                ChartSeries p5 = new ChartSeries("P5");
                p5.Type = ChartSeriesType.Line;
                p5.DataYColumn = "P5";
                RadChart1.Series.Add(p5);
                p5.Appearance.LabelAppearance.Visible = false;
                p5.Appearance.EmptyValue.Line.Color = Color.Transparent;

                ChartSeries p10 = new ChartSeries("P10");
                p10.Type = ChartSeriesType.Line;
                p10.DataYColumn = "P10";
                RadChart1.Series.Add(p10);
                p10.Appearance.LabelAppearance.Visible = false;
                p10.Appearance.EmptyValue.Line.Color = Color.Transparent;

                ChartSeries p25 = new ChartSeries("P25");
                p25.Type = ChartSeriesType.Line;
                p25.DataYColumn = "P25";
                RadChart1.Series.Add(p25);
                p25.Appearance.LabelAppearance.Visible = false;
                p25.Appearance.EmptyValue.Line.Color = Color.Transparent;

                ChartSeries p50 = new ChartSeries("P50");
                p50.Type = ChartSeriesType.Line;
                p50.DataYColumn = "P50";
                RadChart1.Series.Add(p50);
                p50.Appearance.LabelAppearance.Visible = false;
                p50.Appearance.EmptyValue.Line.Color = Color.Transparent;

                ChartSeries p75 = new ChartSeries("P75");
                p75.Type = ChartSeriesType.Line;
                p75.DataYColumn = "P75";
                RadChart1.Series.Add(p75);
                p75.Appearance.LabelAppearance.Visible = false;
                p75.Appearance.EmptyValue.Line.Color = Color.Transparent;

                ChartSeries p90 = new ChartSeries("P90");
                p90.Type = ChartSeriesType.Line;
                p90.DataYColumn = "P90";
                RadChart1.Series.Add(p90);
                p90.Appearance.LabelAppearance.Visible = false;
                p90.Appearance.EmptyValue.Line.Color = Color.Transparent;

                ChartSeries p95 = new ChartSeries("P95");
                p95.Type = ChartSeriesType.Line;
                p95.DataYColumn = "P95";
                RadChart1.Series.Add(p95);
                p95.Appearance.LabelAppearance.Visible = false;
                p95.Appearance.EmptyValue.Line.Color = Color.Transparent;

                RadChart1.DataSource = objDs;
                RadChart1.DataBind();
                RadChart1.Legend.Appearance.Visible = true;

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Inches)";

                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                }
                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";

                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                }
                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";

                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                }
            }
            else
            {
                RadChart1.Clear();
                RadChart1.DataSource = objDs;
                RadChart1.DataBind();
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
           // objbc = null;
            objDs.Dispose();
        }
    }


    //void BindGrowthChartForBirth23To42weeksHTWT() //For Birth to 36 months (Height for Weight) growth charts
    //{
    //    BaseC.GrowthChart objbc = new BaseC.GrowthChart("");
    //    DataSet objDs = new DataSet();
    //    try
    //    {
    //        RadChart1.Clear();
    //        RadChart1.DefaultType = Telerik.Charting.ChartSeriesType.Line;
    //        RadChart1.Legend.Items.Clear();
    //        RadChart1.ClearSkin();
    //        RadChart1.Skin = common.myStr(ddlSkin.SelectedValue);
    //        RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 270;

    //        objDs = objbc.BindGrowthChartForBirth23To42weeksHTWT(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
    //                                                            common.myStr(ddlMsystem.SelectedValue));


    //        RadChart1.PlotArea.XAxis.Items.Clear();

    //        ChartSeries p5 = new ChartSeries("P5");
    //        p5.Type = ChartSeriesType.Line;
    //        foreach (DataRow dr in objDs.Tables[0].Rows)
    //        {
    //            ChartSeriesItem item = new ChartSeriesItem();
    //            item.YValue = common.myDbl(dr["P5"]);
    //            p5.Items.Add(item);
    //        }
    //        RadChart1.Series.Add(p5);
    //        p5.Appearance.LabelAppearance.Visible = false;

    //        ChartSeries p10 = new ChartSeries("P10");
    //        p10.Type = ChartSeriesType.Line;
    //        foreach (DataRow dr in objDs.Tables[0].Rows)
    //        {
    //            ChartSeriesItem item = new ChartSeriesItem();
    //            item.YValue = common.myDbl(dr["P10"]);
    //            p10.Items.Add(item);
    //        }
    //        RadChart1.Series.Add(p10);
    //        p10.Appearance.LabelAppearance.Visible = false;

    //        ChartSeries p25 = new ChartSeries("P25");
    //        p25.Type = ChartSeriesType.Line;
    //        foreach (DataRow dr in objDs.Tables[0].Rows)
    //        {
    //            ChartSeriesItem item = new ChartSeriesItem();
    //            item.YValue = common.myDbl(dr["P25"]);
    //            p25.Items.Add(item);
    //        }
    //        RadChart1.Series.Add(p25);
    //        p25.Appearance.LabelAppearance.Visible = false;
    //        ChartSeries p50 = new ChartSeries("P50");
    //        p50.Type = ChartSeriesType.Line;
    //        foreach (DataRow dr in objDs.Tables[0].Rows)
    //        {
    //            ChartSeriesItem item = new ChartSeriesItem();
    //            item.YValue = common.myDbl(dr["P50"]);
    //            p50.Items.Add(item);
    //        }
    //        RadChart1.Series.Add(p50);
    //        p50.Appearance.LabelAppearance.Visible = false;

    //        ChartSeries p75 = new ChartSeries("P75");
    //        p75.Type = ChartSeriesType.Line;
    //        foreach (DataRow dr in objDs.Tables[0].Rows)
    //        {
    //            ChartSeriesItem item = new ChartSeriesItem();
    //            item.YValue = common.myDbl(dr["P75"]);
    //            p75.Items.Add(item);
    //        }
    //        RadChart1.Series.Add(p75);
    //        p75.Appearance.LabelAppearance.Visible = false;

    //        ChartSeries p90 = new ChartSeries("P90");
    //        p90.Type = ChartSeriesType.Line;
    //        foreach (DataRow dr in objDs.Tables[0].Rows)
    //        {
    //            ChartSeriesItem item = new ChartSeriesItem();
    //            item.YValue = common.myDbl(dr["P90"]);
    //            p90.Items.Add(item);
    //        }
    //        RadChart1.Series.Add(p90);
    //        p90.Appearance.LabelAppearance.Visible = false;

    //        ChartSeries p95 = new ChartSeries("P95");
    //        p95.Type = ChartSeriesType.Line;
    //        foreach (DataRow dr in objDs.Tables[0].Rows)
    //        {
    //            ChartSeriesItem item = new ChartSeriesItem();
    //            item.YValue = common.myDbl(dr["P95"]);
    //            p95.Items.Add(item);
    //        }
    //        RadChart1.Series.Add(p95);
    //        p95.Appearance.LabelAppearance.Visible = false;

    //        ChartSeries PatientValue = new ChartSeries("PatientValue");
    //        PatientValue.Type = ChartSeriesType.Line;
    //        PatientValue.Appearance.LineSeriesAppearance.Color = Color.Black;
    //        PatientValue.Appearance.LineSeriesAppearance.Width = 1;
    //        PatientValue.Appearance.TextAppearance.TextProperties.Color = Color.Black;
    //        PatientValue.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
    //        PatientValue.Appearance.EmptyValue.Line.Color = System.Drawing.Color.Transparent;
    //        PatientValue.Appearance.EmptyValue.PointMark.Visible = false;

    //        PatientValue.Appearance.PointMark.Visible = true;
    //        PatientValue.Appearance.PointMark.Border.Width = 2;
    //        PatientValue.Appearance.PointMark.Border.Color = System.Drawing.Color.Black;
    //        PatientValue.Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(12);
    //        PatientValue.Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(12);
    //        PatientValue.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.DarkGray;
    //        PatientValue.Appearance.PointMark.FillStyle.FillType = FillType.Solid;

    //        foreach (DataRow dr in objDs.Tables[1].Rows)
    //        {
    //            ChartSeriesItem item = new ChartSeriesItem();
    //            item.YValue = common.myDbl(dr["PatientValue"]);
    //            if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
    //            {
    //                item.ActiveRegion.Tooltip = "Height (in Centimeter): " + common.myDbl(dr["Height"]) + " Weight (in Kiligram) : " + common.myDbl(dr["PatientValue"]);
    //            }
    //            else
    //            {
    //                item.ActiveRegion.Tooltip = "Height (in Inches): " + common.myDbl(dr["Height"]) + " Weight (in Pound) : " + common.myDbl(dr["PatientValue"]);
    //            }
    //            PatientValue.Items.Add(item);
    //        }
    //        RadChart1.Series.Add(PatientValue);
    //        PatientValue.Appearance.LabelAppearance.Visible = true;
    //        foreach (DataRow dr in objDs.Tables[0].Rows)
    //        {
    //            RadChart1.PlotArea.XAxis.AddItem(common.myStr(dr["Height"]));
    //        }

    //        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
    //        {
    //            RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Height (in Centimeter)";
    //            RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kiligram)";
    //        }
    //        else
    //        {
    //            RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Height (in Inches)";
    //            RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";
    //        }
    //        RadChart1.DataBind();
    //        RadChart1.Legend.Appearance.Visible = true;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        objbc = null;
    //        objDs.Dispose();
    //    }
    //}

    void BindGrowthChart2Yto19YBMI() // Get Data Length for Age and weight for age and BMI for Age.
    {
       // BaseC.GrowthChart objbc = new BaseC.GrowthChart(sConString);
        DataSet objDs = new DataSet();
        DataTable dtColumns = new DataTable();
        try
        {
            int GrowthChartId = 0;
            try
            {
                if (ViewState["GrowthData"] != null)
                {
                    DataTable DtGrowthChart = ((DataTable)ViewState["GrowthData"]).Copy();
                    if (DtGrowthChart.Rows.Count > 0)
                    {
                        DataView dvgch = DtGrowthChart.DefaultView;
                        dvgch.RowFilter = "AgeGroup='" + common.myStr(ddlGrowthChartType2.SelectedValue) + "' AND DisplayName='" + common.myStr(ddlGrowthChart2.SelectedValue) + "'";
                        if (dvgch.ToTable().Rows.Count > 0)
                        {
                            GrowthChartId = common.myInt(dvgch.ToTable().Rows[0]["GrowthChartId"]);
                        }
                        dvgch.RowFilter = string.Empty;
                    }
                }
            }
            catch
            { }

            //ddlGrowthChart2.SelectedItem.Attributes["GrowthChartId"]
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindGrowthChart2Yto20Y";
            APIRootClass.GrowthChart objRoot = new global::APIRootClass.GrowthChart();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.DisplayName = common.myStr(ddlGrowthChart2.SelectedValue);
            objRoot.FromAge = common.myInt(ViewState["FDays"]);
            objRoot.ToAge = common.myInt(ViewState["TDays"]);
            objRoot.DayType = common.myStr(ViewState["xaxisType"]);
            objRoot.GrowthChartId = common.myInt(GrowthChartId);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


            //objDs = objbc.BindGrowthChart2Yto20Y(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myStr(ddlGrowthChart2.SelectedValue),
            //common.myInt(ViewState["FDays"]), common.myInt(ViewState["TDays"]), common.myStr(ViewState["xaxisType"]), common.myInt(GrowthChartId));

            ViewState["objDs"] = objDs;
            if (objDs.Tables[0].Rows.Count > 0)
            {
                YAxisCapInterval = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapInterval"]);
                YAxisCapMinValue = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapMinValue"]);

                YAxisInterValue = common.myDec(objDs.Tables[0].Rows[0]["YAxisInterVal"]);
                YAxisCapStartvalue = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapStartvalue"]);
                YMax = common.myDec(common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]));
            }
            if (objDs.Tables[2].Rows.Count > 0)
            {
                Xinterval = common.myInt(objDs.Tables[2].Rows[0]["Xinterval"]);
                XMinVal = common.myInt(objDs.Tables[2].Rows[0]["XAxisMinValue"]);
                XMaxval = common.myInt(objDs.Tables[2].Rows[0]["XAxisMaxValue"]);
            }

            RadChart1.Clear();
            RadChart1.DefaultType = ChartSeriesType.Line;

            RadChart1.Legend.Items.Clear();
            RadChart1.ClearSkin();

            RadChart1.Skin = common.myStr(ddlSkin.SelectedValue);
            //RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 270;

            ChartSeries PatientValue = new ChartSeries("Patient Value");
            PatientValue.Type = ChartSeriesType.Line;
            PatientValue.DataYColumn = "PatientValue";
            RadChart1.Series.Add(PatientValue);
            PatientValue.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
            PatientValue.Appearance.EmptyValue.Line.Color = System.Drawing.Color.Transparent;
            PatientValue.Appearance.EmptyValue.PointMark.Visible = false;

            PatientValue.Appearance.PointMark.Visible = true;
            PatientValue.Appearance.PointMark.Border.Width = 1;
            PatientValue.Appearance.PointMark.Border.Color = System.Drawing.Color.Black;
            PatientValue.Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(5);
            PatientValue.Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(5);
            PatientValue.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.DarkGray;
            PatientValue.Appearance.PointMark.FillStyle.FillType = FillType.Solid;

            PatientValue.Appearance.LineSeriesAppearance.Color = Color.Black;
            PatientValue.Appearance.LineSeriesAppearance.Width = 1;
            PatientValue.Appearance.TextAppearance.TextProperties.Color = Color.Black;

            RadChart1.DataManager.ValuesXColumn = "AgeYears";
            RadChart1.PlotArea.XAxis.DataLabelsColumn = "AgeYearsV";

            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = common.myStr(ddlGrowthChart2.SelectedItem.Text);
            //RadChart1.PlotArea.YAxis.AutoScale = false;

            RadChart1.PlotArea.XAxis.MinValue = 61;
            RadChart1.PlotArea.XAxis.MaxValue = 240;
            RadChart1.PlotArea.XAxis.Step = 1;

            if (objDs.Tables[0].Rows.Count > 0)
            {
                RadChart1.PlotArea.XAxis.Step = 10;
                int minval = common.myInt(objDs.Tables[0].Rows[0]["minVal"]);
                RadChart1.PlotArea.YAxis.MinValue = minval;
                RadChart1.PlotArea.YAxis.MaxValue = common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]);

                //if ((common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]) - common.myInt(objDs.Tables[0].Rows[0]["minVal"])) > 50)
                //    RadChart1.PlotArea.YAxis.Step = 10;
                //else
                //    RadChart1.PlotArea.YAxis.Step = 5;

                RadChart1.PlotArea.YAxis.Step = common.myDbl(objDs.Tables[0].Rows[0]["YAxisInterVal"]);

                //Create Chart Series starts
                dtColumns = objDs.Tables[0];
                foreach (DataColumn dc in dtColumns.Columns)
                {
                    switch (dc.ColumnName)
                    {
                        case "P0.1":
                            ChartSeries p0 = new ChartSeries("P0.1");
                            p0.Type = ChartSeriesType.SplineArea;
                            p0.DataYColumn = "P0.1";
                            RadChart1.Series.Add(p0);
                            p0.Appearance.LabelAppearance.Visible = false;
                            //p0.Appearance.EmptyValue.Line.Color = Color.Red;
                            p0.Appearance.LineSeriesAppearance.Color = Color.Red;

                            p0.Appearance.LineSeriesAppearance.Width = 1;
                            break;
                        case "P1":
                            ChartSeries p1 = new ChartSeries("P1");
                            p1.Type = ChartSeriesType.SplineArea;
                            p1.DataYColumn = "P1";
                            RadChart1.Series.Add(p1);
                            p1.Appearance.LabelAppearance.Visible = false;
                            p1.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p1.Appearance.LineSeriesAppearance.Width = 1;
                            break;

                        case "P3":

                            ChartSeries p3 = new ChartSeries("P3");
                            p3.Type = ChartSeriesType.SplineArea;
                            p3.DataYColumn = "P3";
                            RadChart1.Series.Add(p3);
                            p3.Appearance.LabelAppearance.Visible = false;
                            // p3.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p3.Appearance.LineSeriesAppearance.Color = Color.Lime;
                            p3.Appearance.LineSeriesAppearance.Width = 1;
                            break;
                        case "P5":
                            ChartSeries p5 = new ChartSeries("P5");
                            p5.Type = ChartSeriesType.SplineArea;
                            p5.DataYColumn = "P5";
                            RadChart1.Series.Add(p5);
                            p5.Appearance.LabelAppearance.Visible = false;
                            p5.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p5.Appearance.LineSeriesAppearance.Width = 1;
                            break;
                        case "P10":
                            ChartSeries p10 = new ChartSeries("P10");
                            p10.Type = ChartSeriesType.SplineArea;
                            p10.DataYColumn = "P10";
                            RadChart1.Series.Add(p10);
                            p10.Appearance.LabelAppearance.Visible = false;
                            p10.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p10.Appearance.LineSeriesAppearance.Width = 1;
                            break;
                        case "P15":
                            ChartSeries p15 = new ChartSeries("P15");
                            p15.Type = ChartSeriesType.SplineArea;
                            p15.DataYColumn = "P15";
                            RadChart1.Series.Add(p15);
                            p15.Appearance.LabelAppearance.Visible = false;
                            // p15.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p15.Appearance.LineSeriesAppearance.Color = Color.Orange;
                            p15.Appearance.LineSeriesAppearance.Width = 1;
                            break;
                        case "P25":
                            ChartSeries p25 = new ChartSeries("P25");
                            p25.Type = ChartSeriesType.SplineArea;
                            p25.DataYColumn = "P25";
                            RadChart1.Series.Add(p25);
                            p25.Appearance.LabelAppearance.Visible = false;
                            p25.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p25.Appearance.LineSeriesAppearance.Width = 1;
                            break;

                        case "P50":
                            ChartSeries p50 = new ChartSeries("P50");
                            p50.Type = ChartSeriesType.SplineArea;
                            p50.DataYColumn = "P50";
                            RadChart1.Series.Add(p50);
                            p50.Appearance.LabelAppearance.Visible = false;
                            p50.Appearance.LineSeriesAppearance.Color = Color.DarkViolet;



                            //p50.Appearance.LineSeriesAppearance.= 10;

                            p50.Appearance.LineSeriesAppearance.Width = 4;


                            break;
                        case "P75":
                            ChartSeries p75 = new ChartSeries("P75");
                            p75.Type = ChartSeriesType.SplineArea;
                            p75.DataYColumn = "P75";
                            RadChart1.Series.Add(p75);
                            p75.Appearance.LabelAppearance.Visible = false;
                            p75.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p75.Appearance.LineSeriesAppearance.Width = 1;
                            break;
                        case "P90":
                            ChartSeries p90 = new ChartSeries("P90");
                            p90.Type = ChartSeriesType.SplineArea;
                            p90.DataYColumn = "P90";
                            RadChart1.Series.Add(p90);
                            p90.Appearance.LabelAppearance.Visible = false;
                            p90.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p90.Appearance.LineSeriesAppearance.Width = 1;
                            break;

                        case "P85":
                            ChartSeries p85 = new ChartSeries("P85");
                            p85.Type = ChartSeriesType.SplineArea;
                            p85.DataYColumn = "P85";
                            RadChart1.Series.Add(p85);
                            p85.Appearance.LabelAppearance.Visible = false;
                            // p85.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p85.Appearance.LineSeriesAppearance.Color = Color.Green;
                            p85.Appearance.LineSeriesAppearance.Width = 1;
                            break;


                        case "P95":
                            ChartSeries p95 = new ChartSeries("P95");
                            p95.Type = ChartSeriesType.SplineArea;
                            p95.DataYColumn = "P95";
                            RadChart1.Series.Add(p95);
                            p95.Appearance.LabelAppearance.Visible = false;
                            p95.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p95.Appearance.LineSeriesAppearance.Width = 1;
                            break;

                        case "P97":
                            ChartSeries p97 = new ChartSeries("P97");
                            p97.Type = ChartSeriesType.SplineArea;
                            p97.DataYColumn = "P97";
                            RadChart1.Series.Add(p97);
                            p97.Appearance.LabelAppearance.Visible = false;
                            // p97.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p97.Appearance.LineSeriesAppearance.Color = Color.Blue;
                            p97.Appearance.LineSeriesAppearance.Width = 1;

                            break;


                        case "P99":
                            ChartSeries p99 = new ChartSeries("P99");
                            p99.Type = ChartSeriesType.SplineArea;
                            p99.DataYColumn = "P99";
                            RadChart1.Series.Add(p99);
                            p99.Appearance.LabelAppearance.Visible = false;
                            p99.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p99.Appearance.LineSeriesAppearance.Width = 1;
                            break;

                        case "P99.9":
                            ChartSeries p999 = new ChartSeries("P99.9");
                            p999.Type = ChartSeriesType.SplineArea;
                            p999.DataYColumn = "P99.9";

                            RadChart1.Series.Add(p999);
                            p999.Appearance.LabelAppearance.Visible = false;
                            // p999.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p999.Appearance.LineSeriesAppearance.Color = Color.Yellow;
                            p999.Appearance.LineSeriesAppearance.Width = 1;

                            break;

                    }
                }

                RadChart1.DataSource = objDs;
                RadChart1.DataBind();
                RadChart1.Legend.Appearance.Visible = true;

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Inches)";

                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";

                }
                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";

                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("BMI"))
                {
                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                }
                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Inches)";

                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";
                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";

                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                }

                if (!common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals(string.Empty) && objDs.Tables[2].Rows.Count > 0)
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = common.myStr(objDs.Tables[2].Rows[0]["YAxisDisplayCap"]);
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";

                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = common.myStr(objDs.Tables[2].Rows[0]["XAxisDisplayCap"]);

                }

            }
            else
            {
                RadChart1.Clear();
                RadChart1.DataSource = objDs;
                RadChart1.DataBind();
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
           // objbc = null;
            objDs.Dispose();
            dtColumns.Dispose();
        }
    }

    void BindGrowthChartBirthto2YHT() // Get Data Length for Age and weight for age and BMI for Age.
    {
      //  BaseC.GrowthChart objbc = new BaseC.GrowthChart(sConString);
        DataSet objDs = new DataSet();
        DataTable dtColumns = new DataTable();
        try
        {
            int GrowthChartId = 0;
            try
            {
                if (ViewState["GrowthData"] != null)
                {
                    DataTable DtGrowthChart = ((DataTable)ViewState["GrowthData"]).Copy();
                    if (DtGrowthChart.Rows.Count > 0)
                    {
                        DataView dvgch = DtGrowthChart.DefaultView;
                        dvgch.RowFilter = "AgeGroup='" + common.myStr(ddlGrowthChartType2.SelectedValue) + "' AND DisplayName='" + common.myStr(ddlGrowthChart2.SelectedValue) + "'";
                        if (dvgch.ToTable().Rows.Count > 0)
                        {
                            GrowthChartId = common.myInt(dvgch.ToTable().Rows[0]["GrowthChartId"]);
                        }
                        dvgch.RowFilter = string.Empty;
                    }
                }
            }
            catch
            { }

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindGrowthChart2Yto20Y";
            APIRootClass.GrowthChart objRoot = new global::APIRootClass.GrowthChart();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.DisplayName = common.myStr(ddlGrowthChart2.SelectedValue);
            objRoot.FromAge = common.myInt(ViewState["FDays"]);
            objRoot.ToAge = common.myInt(ViewState["TDays"]);
            objRoot.DayType = common.myStr(ViewState["xaxisType"]);
            objRoot.GrowthChartId = common.myInt(GrowthChartId);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            //objDs = objbc.BindGrowthChart2Yto20Y(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myStr(ddlGrowthChart2.SelectedValue),
            //    common.myInt(ViewState["FDays"]), common.myInt(ViewState["TDays"]), common.myStr(ViewState["xaxisType"]), common.myInt(GrowthChartId));

            RadChart1.Clear();
            RadChart1.DefaultType = Telerik.Charting.ChartSeriesType.Line;

            RadChart1.Legend.Items.Clear();
            RadChart1.ClearSkin();
            RadChart1.Skin = common.myStr(ddlSkin.SelectedValue);
            //RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 270;

            ChartSeries PatientValue = new ChartSeries("Patient Value");
            PatientValue.Type = ChartSeriesType.Line;
            PatientValue.DataYColumn = "PatientValue";
            RadChart1.Series.Add(PatientValue);
            PatientValue.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
            PatientValue.Appearance.EmptyValue.Line.Color = System.Drawing.Color.Transparent;
            PatientValue.Appearance.EmptyValue.PointMark.Visible = false;

            PatientValue.Appearance.PointMark.Visible = true;
            PatientValue.Appearance.PointMark.Border.Width = 1;
            PatientValue.Appearance.PointMark.Border.Color = System.Drawing.Color.Black;
            PatientValue.Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue.Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.DarkGray;
            PatientValue.Appearance.PointMark.FillStyle.FillType = FillType.Solid;

            PatientValue.Appearance.LineSeriesAppearance.Color = Color.Black;
            PatientValue.Appearance.LineSeriesAppearance.Width = 1;
            PatientValue.Appearance.TextAppearance.TextProperties.Color = Color.Black;
            RadChart1.PlotArea.XAxis.DataLabelsColumn = "AgeYears";
            RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = common.myStr(ddlGrowthChart2.SelectedItem.Text);


            if (objDs.Tables[0].Rows.Count > 0)
            {
                RadChart1.PlotArea.XAxis.Step = 10;
                int minval = common.myInt(objDs.Tables[0].Rows[0]["minVal"]);

                RadChart1.PlotArea.YAxis.MinValue = minval;
                RadChart1.PlotArea.YAxis.MaxValue = common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]);
                if (common.myStr(ddlGrowthChart2.SelectedValue) == "HT2TO19")
                {
                    RadChart1.PlotArea.YAxis.Step = 5;
                }
                else
                {
                    if ((common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]) - common.myInt(objDs.Tables[0].Rows[0]["minVal"])) > 50)
                    {
                        RadChart1.PlotArea.YAxis.Step = 10;
                    }
                    else
                    {
                        if (common.myStr(ddlGrowthChart2.SelectedValue) == "HC")
                        {
                            RadChart1.PlotArea.YAxis.Step = 10;
                        }
                        else
                        {
                            RadChart1.PlotArea.YAxis.Step = 5;
                        }
                    }
                }
                //RadChart1.PlotArea.YAxis.Step = common.myDbl(objDs.Tables[0].Rows[0]["YAxisInterVal"]);
                //Create Chart Series starts
                dtColumns = objDs.Tables[0];
                foreach (DataColumn dc in dtColumns.Columns)
                {
                    switch (dc.ColumnName)
                    {
                        case "P0.1":
                            ChartSeries p1 = new ChartSeries("P0.1");
                            p1.Type = ChartSeriesType.Line;
                            p1.DataYColumn = "P0.1";
                            RadChart1.Series.Add(p1);
                            p1.Appearance.LabelAppearance.Visible = false;
                            p1.Appearance.LineSeriesAppearance.Color = Color.Yellow;
                            p1.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        case "P3":

                            ChartSeries p3 = new ChartSeries("P3");
                            p3.Type = ChartSeriesType.Line;
                            p3.DataYColumn = "P3";
                            RadChart1.Series.Add(p3);
                            p3.Appearance.LabelAppearance.Visible = false;
                            // p3.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p3.Appearance.LineSeriesAppearance.Color = Color.Red;
                            p3.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        //case "P5":
                        //    ChartSeries p5 = new ChartSeries("P5");
                        //    p5.Type = ChartSeriesType.Line;
                        //    p5.DataYColumn = "P5";
                        //    RadChart1.Series.Add(p5);
                        //    p5.Appearance.LabelAppearance.Visible = false;
                        //    p5.Appearance.EmptyValue.Line.Color = Color.Transparent;
                        //    break;

                        case "P10":
                            ChartSeries p10 = new ChartSeries("P10");
                            p10.Type = ChartSeriesType.Line;
                            p10.DataYColumn = "P10";
                            RadChart1.Series.Add(p10);
                            p10.Appearance.LabelAppearance.Visible = false;
                            p10.Appearance.LineSeriesAppearance.Color = Color.Orange;
                            p10.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        case "P25":

                            ChartSeries p25 = new ChartSeries("P25");
                            p25.Type = ChartSeriesType.Line;
                            p25.DataYColumn = "P25";
                            RadChart1.Series.Add(p25);
                            p25.Appearance.LabelAppearance.Visible = false;
                            //p25.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p25.Appearance.LineSeriesAppearance.Color = Color.Yellow;
                            p25.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P50":
                            ChartSeries p50 = new ChartSeries("P50");
                            p50.Type = ChartSeriesType.Line;
                            p50.DataYColumn = "P50";
                            RadChart1.Series.Add(p50);
                            p50.Appearance.LabelAppearance.Visible = false;
                            // p50.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p50.Appearance.LineSeriesAppearance.Color = Color.Green;
                            p50.Appearance.LineSeriesAppearance.Width = 5;
                            break;

                        case "P75":
                            ChartSeries p75 = new ChartSeries("P75");
                            p75.Type = ChartSeriesType.Line;
                            p75.DataYColumn = "P75";
                            RadChart1.Series.Add(p75);
                            p75.Appearance.LabelAppearance.Visible = false;
                            //p75.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p75.Appearance.LineSeriesAppearance.Color = Color.Lime;
                            p75.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        case "P85":
                            ChartSeries p85 = new ChartSeries("p85");
                            p85.Type = ChartSeriesType.Line;
                            p85.DataYColumn = "p85";
                            RadChart1.Series.Add(p85);
                            p85.Appearance.LabelAppearance.Visible = false;
                            //p75.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p85.Appearance.LineSeriesAppearance.Color = Color.DarkCyan;
                            p85.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        case "P90":
                            ChartSeries p90 = new ChartSeries("P90");
                            p90.Type = ChartSeriesType.Line;
                            p90.DataYColumn = "P90";
                            RadChart1.Series.Add(p90);
                            p90.Appearance.LabelAppearance.Visible = false;
                            p90.Appearance.LineSeriesAppearance.Color = Color.Violet;
                            p90.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P95":
                            ChartSeries p95 = new ChartSeries("P95");
                            p95.Type = ChartSeriesType.Line;
                            p95.DataYColumn = "P95";
                            RadChart1.Series.Add(p95);
                            p95.Appearance.LabelAppearance.Visible = false;
                            p95.Appearance.EmptyValue.Line.Color = Color.DeepPink;
                            p95.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P97":
                            ChartSeries p97 = new ChartSeries("P97");
                            p97.Type = ChartSeriesType.Line;
                            p97.DataYColumn = "P97";
                            RadChart1.Series.Add(p97);
                            p97.Appearance.LabelAppearance.Visible = false;
                            //p97.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p97.Appearance.LineSeriesAppearance.Color = Color.Blue;
                            p97.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P99.9":
                            ChartSeries p99 = new ChartSeries("P99.9");
                            p99.Type = ChartSeriesType.Line;
                            p99.DataYColumn = "P99.9";
                            RadChart1.Series.Add(p99);
                            p99.Appearance.LabelAppearance.Visible = false;
                            p99.Appearance.EmptyValue.Line.Color = Color.Fuchsia;
                            p99.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                    }

                }

                RadChart1.DataSource = objDs;
                RadChart1.DataBind();
                RadChart1.Legend.Appearance.Visible = true;

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Inches)";

                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";

                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT2TO19"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Inches)";

                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";

                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";

                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";
                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("BMI"))
                {
                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                }
                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";
                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).Equals("6"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                    else
                        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";
                }
            }
            else
            {
                RadChart1.Clear();
                RadChart1.DataSource = objDs;
                RadChart1.DataBind();
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
            //objbc = null;
            objDs.Dispose();
            dtColumns.Dispose();
        }


    }
    void BindGrowthChart45To110CMWT()
    {
         // BaseC.GrowthChart objbc = new BaseC.GrowthChart(sConString);
        DataSet objDs = new DataSet();
        DataTable dtColumns = new DataTable();
        try
        {
            int GrowthChartId = 0, AssociateChartId = 0;
            try
            {
                if (ViewState["GrowthData"] != null)
                {
                    DataTable DtGrowthChart = ((DataTable)ViewState["GrowthData"]).Copy();
                    if (DtGrowthChart.Rows.Count > 0)
                    {
                        DataView dvgch = DtGrowthChart.DefaultView;
                        dvgch.RowFilter = "AgeGroup='" + common.myStr(ddlGrowthChartType2.SelectedValue) + "' AND DisplayName='" + common.myStr(ddlGrowthChart2.SelectedValue) + "'";
                        if (dvgch.ToTable().Rows.Count > 0)
                        {
                            GrowthChartId = common.myInt(dvgch.ToTable().Rows[0]["GrowthChartId"]);
                            AssociateChartId = common.myInt(dvgch.ToTable().Rows[0]["AssociateChartId"]);
                        }
                        dvgch.RowFilter = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindGrowthChart2Yto20Y";
            APIRootClass.GrowthChart objRoot = new global::APIRootClass.GrowthChart();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.DisplayName = common.myStr("WFL0To2");
            objRoot.FromAge = 46;
            objRoot.ToAge = 110;
            objRoot.DayType = "";
            objRoot.GrowthChartId = common.myInt(AssociateChartId);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


            //objDs = objbc.BindGrowthChart2Yto20Y(
            //    common.myInt(Session["HospitalLocationId"]), 
            //    common.myInt(Session["RegistrationId"]), 
            //    common.myStr("WFL0To2"),
            //    46,
            //    110,
            //    "",
            //    common.myInt(AssociateChartId));

            ViewState["objDs"] = objDs;
            YAxisCapInterval2 = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapInterval"]);
            YAxisCapMinValue2 = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapMinValue"]);
            YAxisInterValue2 = common.myDec(objDs.Tables[0].Rows[0]["YAxisInterVal"]);
            YAxisCapStartvalue2 = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapStartvalue"]);
            YMax2 = common.myDec(common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]));

            Xinterval2 = common.myInt(objDs.Tables[2].Rows[0]["Xinterval"]);
            XMinVal2 = common.myInt(objDs.Tables[2].Rows[0]["XAxisMinValue"]);
            XMaxval2 = common.myInt(objDs.Tables[2].Rows[0]["XAxisMaxValue"]);

            RadChart2.Clear();
            RadChart2.DefaultType = Telerik.Charting.ChartSeriesType.Line;

            RadChart2.Legend.Items.Clear();
            RadChart2.ClearSkin();
            RadChart2.Skin = common.myStr(ddlSkin.SelectedValue);
            //RadChart2.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 270;

            ChartSeries PatientValue1 = new ChartSeries("Patient Value");
            PatientValue1.Type = ChartSeriesType.Line;
            PatientValue1.DataYColumn = "PatientValue";
            RadChart2.Series.Add(PatientValue1);
            PatientValue1.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
            PatientValue1.Appearance.EmptyValue.Line.Color = System.Drawing.Color.Transparent;
            PatientValue1.Appearance.EmptyValue.PointMark.Visible = false;

            PatientValue1.Appearance.PointMark.Visible = true;
            PatientValue1.Appearance.PointMark.Border.Width = 1;
            PatientValue1.Appearance.PointMark.Border.Color = System.Drawing.Color.Black;
            PatientValue1.Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue1.Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue1.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.DarkGray;
            PatientValue1.Appearance.PointMark.FillStyle.FillType = FillType.Solid;

            PatientValue1.Appearance.LineSeriesAppearance.Color = Color.Black;
            PatientValue1.Appearance.LineSeriesAppearance.Width = 1;
            PatientValue1.Appearance.TextAppearance.TextProperties.Color = Color.Black;
            RadChart2.DataManager.ValuesXColumn = "LengthHeight";
            RadChart2.PlotArea.XAxis.DataLabelsColumn = "LengthHeight";
            RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = common.myStr(ddlGrowthChart2.SelectedItem.Text);
            // RadChart2.PlotArea.YAxis.AutoScale = false;

            if (objDs.Tables[0].Rows.Count > 0)
            {
                //RadChart2.PlotArea.XAxis.Step = 2;
                int minval = common.myInt(objDs.Tables[0].Rows[0]["minVal"]);

                RadChart2.PlotArea.YAxis.MinValue = minval;
                RadChart2.PlotArea.YAxis.MaxValue = common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]);
                RadChart2.PlotArea.YAxis.Step = 1;

                //if ((common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]) - common.myInt(objDs.Tables[0].Rows[0]["minVal"])) > 50)
                //    RadChart2.PlotArea.YAxis.Step = 10;
                //else
                //    RadChart2.PlotArea.YAxis.Step = 5;

                //Create Chart Series starts
                dtColumns = objDs.Tables[0];
                foreach (DataColumn dc in dtColumns.Columns)
                {

                    switch (dc.ColumnName)
                    {
                        //case "P1":
                        //    ChartSeries p1 = new ChartSeries("P1");
                        //    p1.Type = ChartSeriesType.Line;
                        //    p1.DataYColumn = "P1";
                        //    RadChart1.Series.Add(p1);
                        //    p1.Appearance.LabelAppearance.Visible = false;                          
                        //    p1.Appearance.LineSeriesAppearance.Color = Color.Yellow;
                        //    p1.Appearance.LineSeriesAppearance.Width = 2;
                        //    break;
                        case "P3":

                            ChartSeries p3 = new ChartSeries("P3");
                            p3.Type = ChartSeriesType.Line;
                            p3.DataYColumn = "P3";
                            RadChart2.Series.Add(p3);
                            p3.Appearance.LabelAppearance.Visible = false;
                            // p3.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p3.Appearance.LineSeriesAppearance.Color = Color.Aqua;
                            p3.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        //case "P5":
                        //    ChartSeries p5 = new ChartSeries("P5");
                        //    p5.Type = ChartSeriesType.Line;
                        //    p5.DataYColumn = "P5";
                        //    RadChart1.Series.Add(p5);
                        //    p5.Appearance.LabelAppearance.Visible = false;
                        //    p5.Appearance.EmptyValue.Line.Color = Color.Transparent;
                        //    break;

                        case "P10":
                            ChartSeries p10 = new ChartSeries("P10");
                            p10.Type = ChartSeriesType.Line;
                            p10.DataYColumn = "P10";
                            RadChart2.Series.Add(p10);
                            p10.Appearance.LabelAppearance.Visible = false;
                            p10.Appearance.LineSeriesAppearance.Color = Color.Brown;
                            p10.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        case "P25":

                            ChartSeries p25 = new ChartSeries("P25");
                            p25.Type = ChartSeriesType.Line;
                            p25.DataYColumn = "P25";
                            RadChart2.Series.Add(p25);
                            p25.Appearance.LabelAppearance.Visible = false;
                            //p25.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p25.Appearance.LineSeriesAppearance.Color = Color.Coral;
                            p25.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P50":
                            ChartSeries p50 = new ChartSeries("P50");
                            p50.Type = ChartSeriesType.Line;
                            p50.DataYColumn = "P50";
                            RadChart2.Series.Add(p50);
                            p50.Appearance.LabelAppearance.Visible = false;
                            // p50.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p50.Appearance.LineSeriesAppearance.Color = Color.Green;
                            p50.Appearance.LineSeriesAppearance.Width = 5;
                            break;

                        case "P75":
                            ChartSeries p75 = new ChartSeries("P75");
                            p75.Type = ChartSeriesType.Line;
                            p75.DataYColumn = "P75";
                            RadChart2.Series.Add(p75);
                            p75.Appearance.LabelAppearance.Visible = false;
                            //p75.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p75.Appearance.LineSeriesAppearance.Color = Color.Lime;
                            p75.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P90":
                            ChartSeries p90 = new ChartSeries("P90");
                            p90.Type = ChartSeriesType.Line;
                            p90.DataYColumn = "P90";
                            RadChart2.Series.Add(p90);
                            p90.Appearance.LabelAppearance.Visible = false;
                            p90.Appearance.LineSeriesAppearance.Color = Color.Violet;
                            p90.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P95":
                            ChartSeries p95 = new ChartSeries("P95");
                            p95.Type = ChartSeriesType.Line;
                            p95.DataYColumn = "P95";
                            RadChart2.Series.Add(p95);
                            p95.Appearance.LabelAppearance.Visible = false;
                            p95.Appearance.EmptyValue.Line.Color = Color.DeepPink;
                            p95.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P97":
                            ChartSeries p97 = new ChartSeries("P97");
                            p97.Type = ChartSeriesType.Line;
                            p97.DataYColumn = "P97";
                            RadChart2.Series.Add(p97);
                            p97.Appearance.LabelAppearance.Visible = false;
                            //p97.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p97.Appearance.LineSeriesAppearance.Color = Color.Blue;
                            p97.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                            //case "P99":
                            //    ChartSeries p99 = new ChartSeries("P99");
                            //    p99.Type = ChartSeriesType.Line;
                            //    p99.DataYColumn = "P99";
                            //    RadChart1.Series.Add(p99);
                            //    p99.Appearance.LabelAppearance.Visible = false;
                            //    p99.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            //    break;
                    }

                }

                RadChart2.DataSource = objDs;
                RadChart2.DataBind();
                RadChart2.Legend.Appearance.Visible = true;


                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                {
                    RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                    RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                }
            }
            else
            {
                RadChart2.Clear();
                RadChart2.DataSource = objDs;
                RadChart2.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
    }
    void BindGrowthChartBirthTo2YWT()
    {
        //BaseC.GrowthChart objbc = new BaseC.GrowthChart("");
        DataSet objDs = new DataSet();
        DataTable dtColumns = new DataTable();
        try
        {
            int GrowthChartId = 0, AssociateChartId = 0;
            try
            {
                if (ViewState["GrowthData"] != null)
                {
                    DataTable DtGrowthChart = ((DataTable)ViewState["GrowthData"]).Copy();
                    if (DtGrowthChart.Rows.Count > 0)
                    {
                        DataView dvgch = DtGrowthChart.DefaultView;
                        dvgch.RowFilter = "AgeGroup='" + common.myStr(ddlGrowthChartType2.SelectedValue) + "' AND DisplayName='" + common.myStr(ddlGrowthChart2.SelectedValue) + "'";
                        if (dvgch.ToTable().Rows.Count > 0)
                        {
                            GrowthChartId = common.myInt(dvgch.ToTable().Rows[0]["GrowthChartId"]);
                            AssociateChartId = common.myInt(dvgch.ToTable().Rows[0]["AssociateChartId"]);
                        }
                        dvgch.RowFilter = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindGrowthChart2Yto20Y";
            APIRootClass.GrowthChart objRoot = new global::APIRootClass.GrowthChart();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.DisplayName = common.myStr("WT");
            objRoot.FromAge = common.myInt(ViewState["FDays"]);
            objRoot.ToAge = common.myInt(ViewState["TDays"]);
            objRoot.DayType = common.myStr(ViewState["xaxisType"]);
            objRoot.GrowthChartId = AssociateChartId;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            //objDs = objbc.BindGrowthChart2Yto20Y(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myStr("WT"),
            //common.myInt(ViewState["FDays"]), common.myInt(ViewState["TDays"]), common.myStr(ViewState["xaxisType"]), AssociateChartId);

            ViewState["objDs"] = objDs;
            YAxisCapInterval2 = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapInterval"]);
            YAxisCapMinValue2 = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapMinValue"]);
            YAxisInterValue2 = common.myDec(objDs.Tables[0].Rows[0]["YAxisInterVal"]);
            YAxisCapStartvalue2 = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapStartvalue"]);
            YMax2 = common.myDec(common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]));

            Xinterval2 = common.myInt(objDs.Tables[2].Rows[0]["Xinterval"]);
            XMinVal2 = common.myInt(objDs.Tables[2].Rows[0]["XAxisMinValue"]);
            XMaxval2 = common.myInt(objDs.Tables[2].Rows[0]["XAxisMaxValue"]);

            RadChart2.Clear();
            RadChart2.DefaultType = Telerik.Charting.ChartSeriesType.Line;

            RadChart2.Legend.Items.Clear();
            RadChart2.ClearSkin();
            RadChart2.Skin = common.myStr(ddlSkin.SelectedValue);
            //RadChart2.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 270;

            ChartSeries PatientValue1 = new ChartSeries("Patient Value");
            PatientValue1.Type = ChartSeriesType.Line;
            PatientValue1.DataYColumn = "PatientValue";
            RadChart2.Series.Add(PatientValue1);
            PatientValue1.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
            PatientValue1.Appearance.EmptyValue.Line.Color = System.Drawing.Color.Transparent;
            PatientValue1.Appearance.EmptyValue.PointMark.Visible = false;

            PatientValue1.Appearance.PointMark.Visible = true;
            PatientValue1.Appearance.PointMark.Border.Width = 1;
            PatientValue1.Appearance.PointMark.Border.Color = System.Drawing.Color.Black;
            PatientValue1.Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue1.Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue1.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.DarkGray;
            PatientValue1.Appearance.PointMark.FillStyle.FillType = FillType.Solid;

            PatientValue1.Appearance.LineSeriesAppearance.Color = Color.Black;
            PatientValue1.Appearance.LineSeriesAppearance.Width = 1;
            PatientValue1.Appearance.TextAppearance.TextProperties.Color = Color.Black;
            RadChart2.DataManager.ValuesXColumn = "AgeYears";
            RadChart2.PlotArea.XAxis.DataLabelsColumn = "AgeYearsV";
            RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = common.myStr(ddlGrowthChart2.SelectedItem.Text);
            // RadChart2.PlotArea.YAxis.AutoScale = false;

            if (objDs.Tables[0].Rows.Count > 0)
            {
                RadChart2.PlotArea.XAxis.Step = 10;
                int minval = common.myInt(objDs.Tables[0].Rows[0]["minVal"]);

                RadChart2.PlotArea.YAxis.MinValue = minval;
                RadChart2.PlotArea.YAxis.MaxValue = common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]);

                //if ((common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]) - common.myInt(objDs.Tables[0].Rows[0]["minVal"])) > 50)
                //    RadChart2.PlotArea.YAxis.Step = 10;
                //else
                RadChart2.PlotArea.YAxis.Step = 1;

                //Create Chart Series starts
                dtColumns = objDs.Tables[0];
                foreach (DataColumn dc in dtColumns.Columns)
                {

                    switch (dc.ColumnName)
                    {
                        //case "P1":
                        //    ChartSeries p1 = new ChartSeries("P1");
                        //    p1.Type = ChartSeriesType.Line;
                        //    p1.DataYColumn = "P1";
                        //    RadChart1.Series.Add(p1);
                        //    p1.Appearance.LabelAppearance.Visible = false;                          
                        //    p1.Appearance.LineSeriesAppearance.Color = Color.Yellow;
                        //    p1.Appearance.LineSeriesAppearance.Width = 2;
                        //    break;
                        case "P3":

                            ChartSeries p3 = new ChartSeries("P3");
                            p3.Type = ChartSeriesType.Line;
                            p3.DataYColumn = "P3";
                            RadChart2.Series.Add(p3);
                            p3.Appearance.LabelAppearance.Visible = false;
                            // p3.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p3.Appearance.LineSeriesAppearance.Color = Color.Red;
                            p3.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        //case "P5":
                        //    ChartSeries p5 = new ChartSeries("P5");
                        //    p5.Type = ChartSeriesType.Line;
                        //    p5.DataYColumn = "P5";
                        //    RadChart1.Series.Add(p5);
                        //    p5.Appearance.LabelAppearance.Visible = false;
                        //    p5.Appearance.EmptyValue.Line.Color = Color.Transparent;
                        //    break;

                        case "P10":
                            ChartSeries p10 = new ChartSeries("P10");
                            p10.Type = ChartSeriesType.Line;
                            p10.DataYColumn = "P10";
                            RadChart2.Series.Add(p10);
                            p10.Appearance.LabelAppearance.Visible = false;
                            p10.Appearance.LineSeriesAppearance.Color = Color.Orange;
                            p10.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        case "P25":

                            ChartSeries p25 = new ChartSeries("P25");
                            p25.Type = ChartSeriesType.Line;
                            p25.DataYColumn = "P25";
                            RadChart2.Series.Add(p25);
                            p25.Appearance.LabelAppearance.Visible = false;
                            //p25.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p25.Appearance.LineSeriesAppearance.Color = Color.Yellow;
                            p25.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P50":
                            ChartSeries p50 = new ChartSeries("P50");
                            p50.Type = ChartSeriesType.Line;
                            p50.DataYColumn = "P50";
                            RadChart2.Series.Add(p50);
                            p50.Appearance.LabelAppearance.Visible = false;
                            // p50.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p50.Appearance.LineSeriesAppearance.Color = Color.Green;
                            p50.Appearance.LineSeriesAppearance.Width = 5;
                            break;

                        case "P75":
                            ChartSeries p75 = new ChartSeries("P75");
                            p75.Type = ChartSeriesType.Line;
                            p75.DataYColumn = "P75";
                            RadChart2.Series.Add(p75);
                            p75.Appearance.LabelAppearance.Visible = false;
                            //p75.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p75.Appearance.LineSeriesAppearance.Color = Color.Lime;
                            p75.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P90":
                            ChartSeries p90 = new ChartSeries("P90");
                            p90.Type = ChartSeriesType.Line;
                            p90.DataYColumn = "P90";
                            RadChart2.Series.Add(p90);
                            p90.Appearance.LabelAppearance.Visible = false;
                            p90.Appearance.LineSeriesAppearance.Color = Color.Violet;
                            p90.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P95":
                            ChartSeries p95 = new ChartSeries("P95");
                            p95.Type = ChartSeriesType.Line;
                            p95.DataYColumn = "P95";
                            RadChart2.Series.Add(p95);
                            p95.Appearance.LabelAppearance.Visible = false;
                            p95.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p95.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P97":
                            ChartSeries p97 = new ChartSeries("P97");
                            p97.Type = ChartSeriesType.Line;
                            p97.DataYColumn = "P97";
                            RadChart2.Series.Add(p97);
                            p97.Appearance.LabelAppearance.Visible = false;
                            //p97.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p97.Appearance.LineSeriesAppearance.Color = Color.Blue;
                            p97.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                            //case "P99":
                            //    ChartSeries p99 = new ChartSeries("P99");
                            //    p99.Type = ChartSeriesType.Line;
                            //    p99.DataYColumn = "P99";
                            //    RadChart1.Series.Add(p99);
                            //    p99.Appearance.LabelAppearance.Visible = false;
                            //    p99.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            //    break;
                    }

                }

                RadChart2.DataSource = objDs;
                RadChart2.DataBind();
                RadChart2.Legend.Appearance.Visible = true;

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kg)";
                    else
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kg)";

                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";

                }
                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                    else
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";

                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";
                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("BMI"))
                {
                    RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                }
                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kg)";
                    else
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kg)";
                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";
                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).Equals("6"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                    else
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";
                }
            }
            else
            {
                RadChart2.Clear();
                RadChart2.DataSource = objDs;
                RadChart2.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
    }

    void BindGrowthChart2To10YWT()
    {
        // BaseC.GrowthChart objbc = new BaseC.GrowthChart(sConString);
        DataSet objDs = new DataSet();
        DataTable dtColumns = new DataTable();
        try
        {
            int GrowthChartId = 0;
            try
            {
                if (ViewState["GrowthData"] != null)
                {
                    DataTable DtGrowthChart = ((DataTable)ViewState["GrowthData"]).Copy();
                    if (DtGrowthChart.Rows.Count > 0)
                    {
                        DataView dvgch = DtGrowthChart.DefaultView;
                        dvgch.RowFilter = "AgeGroup='" + common.myStr(ddlGrowthChartType2.SelectedValue) + "' AND DisplayName='" + common.myStr(ddlGrowthChart2.SelectedValue) + "'";
                        if (dvgch.ToTable().Rows.Count > 0)
                        {
                            GrowthChartId = common.myInt(dvgch.ToTable().Rows[0]["GrowthChartId"]);
                        }
                        dvgch.RowFilter = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindGrowthChart2Yto20Y";
            APIRootClass.GrowthChart objRoot = new global::APIRootClass.GrowthChart();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.DisplayName = common.myStr("WT");
            objRoot.FromAge = common.myInt(ViewState["FDays"]);
            objRoot.ToAge = common.myInt(ViewState["TDays"]);
            objRoot.DayType = common.myStr(ViewState["xaxisType"]);
            objRoot.GrowthChartId = common.myInt(GrowthChartId);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


            //objDs = objbc.BindGrowthChart2Yto20Y(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myStr("WT"),
            //common.myInt(ViewState["FDays"]), common.myInt(ViewState["TDays"]), common.myStr(ViewState["xaxisType"]), common.myInt(GrowthChartId));

            ViewState["objDs"] = objDs;
            YAxisCapInterval2 = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapInterval"]);
            YAxisCapMinValue2 = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapMinValue"]);
            YAxisInterValue2 = common.myDec(objDs.Tables[0].Rows[0]["YAxisInterVal"]);
            YAxisCapStartvalue2 = common.myInt(objDs.Tables[0].Rows[0]["YAxisCapStartvalue"]);
            YMax2 = common.myDec(common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]));

            Xinterval2 = common.myInt(objDs.Tables[2].Rows[0]["Xinterval"]);
            XMinVal2 = common.myInt(objDs.Tables[2].Rows[0]["XAxisMinValue"]);
            XMaxval2 = common.myInt(objDs.Tables[2].Rows[0]["XAxisMaxValue"]);

            RadChart2.Clear();
            RadChart2.DefaultType = Telerik.Charting.ChartSeriesType.Line;

            RadChart2.Legend.Items.Clear();
            RadChart2.ClearSkin();
            RadChart2.Skin = common.myStr(ddlSkin.SelectedValue);
            //RadChart2.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 270;

            ChartSeries PatientValue1 = new ChartSeries("Patient Value");
            PatientValue1.Type = ChartSeriesType.Line;
            PatientValue1.DataYColumn = "PatientValue";
            RadChart2.Series.Add(PatientValue1);
            PatientValue1.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
            PatientValue1.Appearance.EmptyValue.Line.Color = System.Drawing.Color.Transparent;
            PatientValue1.Appearance.EmptyValue.PointMark.Visible = false;

            PatientValue1.Appearance.PointMark.Visible = true;
            PatientValue1.Appearance.PointMark.Border.Width = 1;
            PatientValue1.Appearance.PointMark.Border.Color = System.Drawing.Color.Black;
            PatientValue1.Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue1.Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue1.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.DarkGray;
            PatientValue1.Appearance.PointMark.FillStyle.FillType = FillType.Solid;

            PatientValue1.Appearance.LineSeriesAppearance.Color = Color.Black;
            PatientValue1.Appearance.LineSeriesAppearance.Width = 1;
            PatientValue1.Appearance.TextAppearance.TextProperties.Color = Color.Black;
            RadChart2.DataManager.ValuesXColumn = "AgeYears";
            RadChart2.PlotArea.XAxis.DataLabelsColumn = "AgeYearsV";
            RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = common.myStr(ddlGrowthChart2.SelectedItem.Text);
            // RadChart2.PlotArea.YAxis.AutoScale = false;




            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = common.myStr(ddlGrowthChart2.SelectedItem.Text);
            //RadChart1.PlotArea.YAxis.AutoScale = false;

            //RadChart2.PlotArea.XAxis.MinValue = 61;
            //RadChart2.PlotArea.XAxis.MaxValue = 240;
            //RadChart2.PlotArea.XAxis.Step = 1;


            if (objDs.Tables[0].Rows.Count > 0)
            {

                RadChart2.PlotArea.XAxis.Step = 10;
                int minval = common.myInt(objDs.Tables[0].Rows[0]["minVal"]);

                RadChart2.PlotArea.YAxis.MinValue = minval;
                RadChart2.PlotArea.YAxis.MaxValue = common.myDbl(objDs.Tables[0].Rows[0]["maxVal"]);

                RadChart2.PlotArea.YAxis.Step = 5;

                //RadChart2.PlotArea.YAxis.MinValue = 9;
                //RadChart2.PlotArea.YAxis.MaxValue = 110;

                dtColumns = objDs.Tables[0];
                foreach (DataColumn dc in dtColumns.Columns)
                {

                    switch (dc.ColumnName)
                    {
                        case "P0.1":
                            ChartSeries p1 = new ChartSeries("P0.1");
                            p1.Type = ChartSeriesType.Line;
                            p1.DataYColumn = "P0.1";
                            RadChart2.Series.Add(p1);
                            p1.Appearance.LabelAppearance.Visible = false;
                            p1.Appearance.LineSeriesAppearance.Color = Color.Yellow;
                            p1.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        case "P3":

                            ChartSeries p3 = new ChartSeries("P3");
                            p3.Type = ChartSeriesType.Line;
                            p3.DataYColumn = "P3";
                            RadChart2.Series.Add(p3);
                            p3.Appearance.LabelAppearance.Visible = false;
                            // p3.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p3.Appearance.LineSeriesAppearance.Color = Color.Aqua;
                            p3.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        //case "P5":
                        //    ChartSeries p5 = new ChartSeries("P5");
                        //    p5.Type = ChartSeriesType.Line;
                        //    p5.DataYColumn = "P5";
                        //    RadChart1.Series.Add(p5);
                        //    p5.Appearance.LabelAppearance.Visible = false;
                        //    p5.Appearance.EmptyValue.Line.Color = Color.Transparent;
                        //    break;

                        case "P10":
                            ChartSeries p10 = new ChartSeries("P10");
                            p10.Type = ChartSeriesType.Line;
                            p10.DataYColumn = "P10";
                            RadChart2.Series.Add(p10);
                            p10.Appearance.LabelAppearance.Visible = false;
                            p10.Appearance.LineSeriesAppearance.Color = Color.Brown;
                            p10.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P15":

                            ChartSeries p15 = new ChartSeries("P15");
                            p15.Type = ChartSeriesType.Line;
                            p15.DataYColumn = "P15";
                            RadChart2.Series.Add(p15);
                            p15.Appearance.LabelAppearance.Visible = false;
                            //p25.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p15.Appearance.LineSeriesAppearance.Color = Color.Orange;
                            p15.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                        case "P25":

                            ChartSeries p25 = new ChartSeries("P25");
                            p25.Type = ChartSeriesType.Line;
                            p25.DataYColumn = "P25";
                            RadChart2.Series.Add(p25);
                            p25.Appearance.LabelAppearance.Visible = false;
                            //p25.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p25.Appearance.LineSeriesAppearance.Color = Color.Coral;
                            p25.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P50":
                            ChartSeries p50 = new ChartSeries("P50");
                            p50.Type = ChartSeriesType.Line;
                            p50.DataYColumn = "P50";
                            RadChart2.Series.Add(p50);
                            p50.Appearance.LabelAppearance.Visible = false;
                            // p50.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p50.Appearance.LineSeriesAppearance.Color = Color.Green;
                            p50.Appearance.LineSeriesAppearance.Width = 5;
                            break;

                        case "P75":
                            ChartSeries p75 = new ChartSeries("P75");
                            p75.Type = ChartSeriesType.Line;
                            p75.DataYColumn = "P75";
                            RadChart2.Series.Add(p75);
                            p75.Appearance.LabelAppearance.Visible = false;
                            //p75.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p75.Appearance.LineSeriesAppearance.Color = Color.Lime;
                            p75.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P85":
                            ChartSeries P85 = new ChartSeries("P85");
                            P85.Type = ChartSeriesType.Line;
                            P85.DataYColumn = "P85";
                            RadChart2.Series.Add(P85);
                            P85.Appearance.LabelAppearance.Visible = false;
                            //p75.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            P85.Appearance.LineSeriesAppearance.Color = Color.DarkCyan;
                            P85.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P90":
                            ChartSeries p90 = new ChartSeries("P90");
                            p90.Type = ChartSeriesType.Line;
                            p90.DataYColumn = "P90";
                            RadChart2.Series.Add(p90);
                            p90.Appearance.LabelAppearance.Visible = false;
                            p90.Appearance.LineSeriesAppearance.Color = Color.Violet;
                            p90.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P95":
                            ChartSeries p95 = new ChartSeries("P95");
                            p95.Type = ChartSeriesType.Line;
                            p95.DataYColumn = "P95";
                            RadChart2.Series.Add(p95);
                            p95.Appearance.LabelAppearance.Visible = false;
                            p95.Appearance.EmptyValue.Line.Color = Color.DeepPink;
                            p95.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P97":
                            ChartSeries p97 = new ChartSeries("P97");
                            p97.Type = ChartSeriesType.Line;
                            p97.DataYColumn = "P97";
                            RadChart2.Series.Add(p97);
                            p97.Appearance.LabelAppearance.Visible = false;
                            //p97.Appearance.EmptyValue.Line.Color = Color.Transparent;
                            p97.Appearance.LineSeriesAppearance.Color = Color.Blue;
                            p97.Appearance.LineSeriesAppearance.Width = 3;
                            break;

                        case "P99.9":
                            ChartSeries p99 = new ChartSeries("P99.9");
                            p99.Type = ChartSeriesType.Line;
                            p99.DataYColumn = "P99.9";
                            RadChart2.Series.Add(p99);
                            p99.Appearance.LabelAppearance.Visible = false;
                            p99.Appearance.EmptyValue.Line.Color = Color.Fuchsia;
                            p99.Appearance.LineSeriesAppearance.Width = 3;
                            break;
                    }

                }

                RadChart2.DataSource = objDs;
                RadChart2.DataBind();
                RadChart2.Legend.Appearance.Visible = true;

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kg)";
                    else
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kg)";

                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";

                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT2TO19"))
                {
                    RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                    RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                }


                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("Y"))
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                    else
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";

                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";
                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("BMI"))
                {
                    RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                }
                if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kg)";
                    else
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kg)";
                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";
                }

                if (common.myStr(ddlGrowthChart2.SelectedValue).Equals("6"))
                {
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                    else
                        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                    if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                    else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";
                }
            }
            else
            {
                RadChart2.Clear();
                RadChart2.DataSource = objDs;
                RadChart2.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
    }

    public void lnkbtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/EMR/Vitals/GrowthChartDesign.aspx");
    }

    void BindGrowthchart() //New method for test Length for Age and weight for age and BMI for Age.
    {
        // BaseC.GrowthChart objbc = new BaseC.GrowthChart(sConString);
        DataSet objDs = new DataSet();
        try
        {
            int GrowthChartId = 0;
            try
            {
                if (ViewState["GrowthData"] != null)
                {
                    DataTable DtGrowthChart = ((DataTable)ViewState["GrowthData"]).Copy();
                    if (DtGrowthChart.Rows.Count > 0)
                    {
                        DataView dvgch = DtGrowthChart.DefaultView;
                        dvgch.RowFilter = "AgeGroup='" + common.myStr(ddlGrowthChartType2.SelectedValue) + "' AND DisplayName='" + common.myStr(ddlGrowthChart2.SelectedValue) + "'";
                        if (dvgch.ToTable().Rows.Count > 0)
                        {
                            GrowthChartId = common.myInt(dvgch.ToTable().Rows[0]["GrowthChartId"]);
                        }
                        dvgch.RowFilter = string.Empty;
                    }
                }
            }
            catch
            { }

            RadChart1.Clear();

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindGrowthChart2Yto20Y";
            APIRootClass.GrowthChart objRoot = new global::APIRootClass.GrowthChart();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.DisplayName = common.myStr(ddlGrowthChart2.SelectedValue);
            objRoot.FromAge = common.myInt(ViewState["FDays"]);
            objRoot.ToAge = common.myInt(ViewState["TDays"]);
            objRoot.DayType = common.myStr(ViewState["xaxisType"]);
            objRoot.GrowthChartId = common.myInt(GrowthChartId);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            //objDs = objbc.BindGrowthChart2Yto20Y(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myStr(ddlGrowthChart2.SelectedValue),
            //    common.myInt(ViewState["FDays"]), common.myInt(ViewState["TDays"]), common.myStr(ViewState["xaxisType"]), common.myInt(GrowthChartId));

            RadChart1.PlotArea.XAxis.Items.Clear();

            ChartSeries p1 = new ChartSeries("P1");
            p1.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P1"]);
                p1.Items.Add(item);
            }
            RadChart1.Series.Add(p1);
            p1.Appearance.LabelAppearance.Visible = false;

            ChartSeries p3 = new ChartSeries("P3");
            p3.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P3"]);
                p1.Items.Add(item);
            }
            RadChart1.Series.Add(p3);
            p3.Appearance.LabelAppearance.Visible = false;

            ChartSeries p5 = new ChartSeries("P5");
            p5.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P5"]);
                p5.Items.Add(item);
            }
            RadChart1.Series.Add(p5);
            p5.Appearance.LabelAppearance.Visible = false;

            ChartSeries p15 = new ChartSeries("P15");
            p15.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P15"]);
                p15.Items.Add(item);
            }
            RadChart1.Series.Add(p15);
            p15.Appearance.LabelAppearance.Visible = false;

            ChartSeries p25 = new ChartSeries("P25");
            p25.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P25"]);
                p25.Items.Add(item);
            }
            RadChart1.Series.Add(p25);
            p25.Appearance.LabelAppearance.Visible = false;

            ChartSeries p50 = new ChartSeries("P50");
            p50.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P50"]);
                p50.Items.Add(item);
            }
            RadChart1.Series.Add(p50);
            p50.Appearance.LabelAppearance.Visible = false;

            ChartSeries p75 = new ChartSeries("P75");
            p75.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P75"]);
                p75.Items.Add(item);
            }
            RadChart1.Series.Add(p75);
            p75.Appearance.LabelAppearance.Visible = false;

            ChartSeries p85 = new ChartSeries("P85");
            p85.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P85"]);
                p85.Items.Add(item);
            }
            RadChart1.Series.Add(p85);
            p85.Appearance.LabelAppearance.Visible = false;


            ChartSeries p95 = new ChartSeries("P95");
            p95.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P95"]);
                p95.Items.Add(item);
            }
            RadChart1.Series.Add(p95);
            p95.Appearance.LabelAppearance.Visible = false;

            ChartSeries p97 = new ChartSeries("P97");
            p97.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P97"]);
                p97.Items.Add(item);
            }
            RadChart1.Series.Add(p97);
            p97.Appearance.LabelAppearance.Visible = false;

            ChartSeries p99 = new ChartSeries("P99");
            p99.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P99"]);
                p99.Items.Add(item);
            }

            ChartSeries PatientValue = new ChartSeries("Patient Value");
            RadChart1.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            PatientValue.Type = ChartSeriesType.Line;
            PatientValue.DataYColumn = "PatientValue";
            RadChart1.Series.Add(PatientValue);
            PatientValue.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
            PatientValue.Appearance.EmptyValue.Line.Color = System.Drawing.Color.Transparent;
            PatientValue.Appearance.EmptyValue.PointMark.Visible = false;

            PatientValue.Appearance.PointMark.Visible = true;
            PatientValue.Appearance.PointMark.Border.Width = 2;
            PatientValue.Appearance.PointMark.Border.Color = System.Drawing.Color.Black;
            PatientValue.Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue.Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.DarkGray;
            PatientValue.Appearance.PointMark.FillStyle.FillType = FillType.Solid;

            PatientValue.Appearance.LineSeriesAppearance.Color = Color.Black;
            PatientValue.Appearance.LineSeriesAppearance.Width = 1;
            PatientValue.Appearance.TextAppearance.TextProperties.Color = Color.Black;
            RadChart1.PlotArea.XAxis.DataLabelsColumn = "AgeYears";
            RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = common.myStr(ddlGrowthChart2.SelectedItem.Text);

            foreach (DataRow dr in objDs.Tables[1].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["PatientValue"]);
                if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                {
                    item.ActiveRegion.Tooltip = "Height (in Centimeter): " + common.myDbl(dr["Height"]) + " Weight (in Kiligram) : " + common.myDbl(dr["PatientValue"]);
                }
                else
                {
                    item.ActiveRegion.Tooltip = "Height (in Inches): " + common.myDbl(dr["Height"]) + " Weight (in Pound) : " + common.myDbl(dr["PatientValue"]);
                }
                PatientValue.Items.Add(item);
            }
            if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
            {
                if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                    RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                else
                    RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Inches)";

                if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("M"))
                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Months)";
                else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("Y"))
                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                else if (common.myStr(ViewState["xaxisType"]).ToUpper().Equals("W"))
                    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Weeks)";

            }
            if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
            {
                if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                    RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                else
                    RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";

                RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
            }

            if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("BMI"))
            {
                RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
            }
            if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
            {
                if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                    RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                else
                    RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
            }

            RadChart1.DataBind();
            RadChart1.Legend.Appearance.Visible = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            //objbc = null;
            objDs.Dispose();
        }
    }

    void BindGrowthChartFor2Yto20YHTWT() //Get Data Height for weight
    {
        //BaseC.GrowthChart objbc = new BaseC.GrowthChart("");
        DataSet objDs = new DataSet();
        try
        {
            RadChart1.Clear();

            //objDs = objbc.BindGrowthChartFor2Yto20YHTWT(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]));


            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindGrowthChartFor2Yto20YHTWT";
            APIRootClass.BindGrowthChartFor2Yto20YHTWT objRoot = new global::APIRootClass.BindGrowthChartFor2Yto20YHTWT();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
           
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


            RadChart1.PlotArea.XAxis.Items.Clear();

            ChartSeries p1 = new ChartSeries("P1");
            p1.Type = ChartSeriesType.Line;
            p1.DataYColumn = "P1";
            RadChart1.Series.Add(p1);
            p1.Appearance.LabelAppearance.Visible = false;
            p1.Appearance.EmptyValue.Line.Color = Color.Transparent;

            ChartSeries p3 = new ChartSeries("P3");
            p3.Type = ChartSeriesType.Line;
            p3.DataYColumn = "P3";
            RadChart1.Series.Add(p3);
            p3.Appearance.LabelAppearance.Visible = false;
            p3.Appearance.EmptyValue.Line.Color = Color.Transparent;

            ChartSeries p5 = new ChartSeries("P5");
            p5.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P5"]);
                p5.Items.Add(item);
            }
            RadChart1.Series.Add(p5);
            p5.Appearance.LabelAppearance.Visible = false;

            ChartSeries p15 = new ChartSeries("P15");
            p15.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P15"]);
                p15.Items.Add(item);
            }
            RadChart1.Series.Add(p15);
            p15.Appearance.LabelAppearance.Visible = false;


            ChartSeries p25 = new ChartSeries("P25");
            p25.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P25"]);
                p25.Items.Add(item);
            }
            RadChart1.Series.Add(p25);
            p25.Appearance.LabelAppearance.Visible = false;

            ChartSeries p50 = new ChartSeries("P50");
            p50.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P50"]);
                p50.Items.Add(item);
            }
            RadChart1.Series.Add(p50);
            p50.Appearance.LabelAppearance.Visible = false;

            ChartSeries p75 = new ChartSeries("P75");
            p75.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P75"]);
                p75.Items.Add(item);
            }
            RadChart1.Series.Add(p75);
            p75.Appearance.LabelAppearance.Visible = false;

            ChartSeries p85 = new ChartSeries("P85");
            p85.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P85"]);
                p85.Items.Add(item);
            }
            RadChart1.Series.Add(p85);
            p85.Appearance.LabelAppearance.Visible = false;


            ChartSeries p95 = new ChartSeries("P95");
            p95.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P95"]);
                p95.Items.Add(item);
            }
            RadChart1.Series.Add(p95);
            p95.Appearance.LabelAppearance.Visible = false;

            ChartSeries p97 = new ChartSeries("P97");
            p97.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P97"]);
                p97.Items.Add(item);
            }
            RadChart1.Series.Add(p97);
            p97.Appearance.LabelAppearance.Visible = false;

            ChartSeries p99 = new ChartSeries("P99");
            p99.Type = ChartSeriesType.Line;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["P99"]);
                p99.Items.Add(item);
            }
            RadChart1.Series.Add(p99);
            p99.Appearance.LabelAppearance.Visible = false;


            ChartSeries PatientValue = new ChartSeries("PatientValue");
            PatientValue.Type = ChartSeriesType.Line;
            PatientValue.Appearance.LineSeriesAppearance.Color = Color.Black;
            PatientValue.Appearance.LineSeriesAppearance.Width = 1;
            PatientValue.Appearance.TextAppearance.TextProperties.Color = Color.Black;

            PatientValue.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
            PatientValue.Appearance.EmptyValue.Line.Color = System.Drawing.Color.Transparent;
            PatientValue.Appearance.EmptyValue.PointMark.Visible = false;

            PatientValue.Appearance.PointMark.Visible = true;
            PatientValue.Appearance.PointMark.Border.Width = 2;
            PatientValue.Appearance.PointMark.Border.Color = System.Drawing.Color.Black;
            PatientValue.Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue.Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(12);
            PatientValue.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.DarkGray;
            PatientValue.Appearance.PointMark.FillStyle.FillType = FillType.Solid;

            int minval = common.myInt(objDs.Tables[0].Rows[0]["minVal"]);

            RadChart1.PlotArea.YAxis.MinValue = minval;
            RadChart1.PlotArea.YAxis.MaxValue = common.myInt(objDs.Tables[0].Rows[0]["maxVal"]);

            RadChart1.PlotArea.XAxis.Step = 20;
            RadChart1.PlotArea.YAxis.Step = 2;

            foreach (DataRow dr in objDs.Tables[1].Rows)
            {
                ChartSeriesItem item = new ChartSeriesItem();
                item.YValue = common.myDbl(dr["PatientValue"]);
                if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                {
                    item.ActiveRegion.Tooltip = "Height (in Centimeter): " + common.myDbl(dr["Height"]) + " Weight (in Kiligram) : " + common.myDbl(dr["PatientValue"]);
                }
                else
                {
                    item.ActiveRegion.Tooltip = "Height (in Inches): " + common.myDbl(dr["Height"]) + " Weight (in Pound) : " + common.myDbl(dr["PatientValue"]);
                }
                PatientValue.Items.Add(item);
            }
            RadChart1.Series.Add(PatientValue);
            PatientValue.Appearance.LabelAppearance.Visible = true;
            foreach (DataRow dr in objDs.Tables[0].Rows)
            {
                RadChart1.PlotArea.XAxis.AddItem(common.myStr(dr["Height"]));
            }

            if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
            {
                RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Height (in Centimeter)";
                RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kiligram)";
            }
            else
            {
                RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Height (in Inches)";
                RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";
            }

            RadChart1.DataBind();
            RadChart1.Legend.Appearance.Visible = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            //objbc = null;
            objDs.Dispose();
        }
    }

    protected void btnVital_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/EMR/Vitals/Vitals.aspx?Mpg=P14", false);
    }

    protected void HideLabels(RadChart radChart)
    {
        //foreach (ChartSeries series in radChart.Series)
        //{
        //series.Appearance.TextAppearance.TextProperties.Font.Bold;
        //}
    }

    protected void RadChart1_OnItemDataBound(object sender, ChartItemDataBoundEventArgs e)
    {
        try
        {
            //e.SeriesItem.Name = (e.DataItem as DataRowView).Row[0].ToString(); 
            if (common.myStr(ddlGrowthChartType.SelectedValue).Equals("1"))
            {
                if (!common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HW"))
                {
                    // For Birth to 36 months (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Length (in Centimeter): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Inches)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Length (in Inches): " + e.SeriesItem.YValue;
                        }
                    }
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Weight (in Kilogram): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Weight (in Pound): " + e.SeriesItem.YValue;
                        }
                    }
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Head Circumference (in Centimeter): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Head Circumference (in Inches): " + e.SeriesItem.YValue;
                        }
                    }
                }
                else
                {
                    //For Birth to 36 months (Height for Weight) growth charts
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                    {
                        e.SeriesItem.ActiveRegion.Tooltip = "Height (in Centimeter): " + common.myStr(((DataRowView)e.DataItem)["Height"]) + " Weight (in Kiligram): " + e.SeriesItem.YValue;
                    }
                    else
                    {
                        e.SeriesItem.ActiveRegion.Tooltip = "Height (in Inches): " + common.myStr(((DataRowView)e.DataItem)["Height"]) + " Weight (in Pound): " + e.SeriesItem.YValue;
                    }
                }
            }
            else
            {
                if (!common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HW"))
                {
                    // For Birth to 36 months (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Length (in Centimeter): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Inches)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Length (in Inches): " + e.SeriesItem.YValue;
                        }
                    }
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Weight (in Kilogram): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Weight (in Pound): " + e.SeriesItem.YValue;
                        }
                    }
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Head Circumference (in Centimeter): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Head Circumference (in Inches): " + e.SeriesItem.YValue;
                        }
                    }
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("BMI"))
                    {
                        //RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                        e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " BMI: " + e.SeriesItem.YValue;
                    }
                }
                else
                {
                    //For Birth to 36 months (Height for Weight) growth charts
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                    {
                        //RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Height (in Centimeter)";
                        //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kiligram)";
                        e.SeriesItem.ActiveRegion.Tooltip = "Height (in Centimeter): " + common.myStr(((DataRowView)e.DataItem)["Height"]) + " Weight (in Kiligram) : " + e.SeriesItem.YValue;
                    }
                    else
                    {
                        //RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Height (in Inches)";
                        //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";
                        e.SeriesItem.ActiveRegion.Tooltip = "Height (in Inches): " + common.myStr(((DataRowView)e.DataItem)["Height"]) + " Weight (in Pound) " + e.SeriesItem.YValue;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void RadChart2_OnItemDataBound(object sender, ChartItemDataBoundEventArgs e)
    {
        try
        {
            //e.SeriesItem.Name = (e.DataItem as DataRowView).Row[0].ToString(); 
            if (common.myStr(ddlGrowthChartType.SelectedValue).Equals("1"))
            {
                if (!common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HW"))
                {
                    // For Birth to 36 months (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Length (in Centimeter): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Inches)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Length (in Inches): " + e.SeriesItem.YValue;
                        }
                    }
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Weight (in Kilogram): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Weight (in Pound): " + e.SeriesItem.YValue;
                        }
                    }
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Head Circumference (in Centimeter): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Months): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Head Circumference (in Inches): " + e.SeriesItem.YValue;
                        }
                    }
                }
                else
                {
                    //For Birth to 36 months (Height for Weight) growth charts
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                    {
                        e.SeriesItem.ActiveRegion.Tooltip = "Height (in Centimeter): " + common.myStr(((DataRowView)e.DataItem)["Height"]) + " Weight (in Kiligram): " + e.SeriesItem.YValue;
                    }
                    else
                    {
                        e.SeriesItem.ActiveRegion.Tooltip = "Height (in Inches): " + common.myStr(((DataRowView)e.DataItem)["Height"]) + " Weight (in Pound): " + e.SeriesItem.YValue;
                    }
                }
            }
            else
            {
                if (!common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HW"))
                {
                    // For Birth to 36 months (Length for Age,Weight for Age,Head Circumference for Age) growth charts
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HT"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Centimeter)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Length (in Centimeter): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Length (in Inches)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Length (in Inches): " + e.SeriesItem.YValue;
                        }
                    }
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("WT"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kilogram)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Weight (in Kilogram): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Weight (in Pound): " + e.SeriesItem.YValue;
                        }
                    }
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("HC"))
                    {
                        if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Centimeter)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Head Circumference (in Centimeter): " + e.SeriesItem.YValue;
                        }
                        else
                        {
                            //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Head Circumference (in Inches)";
                            e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " Head Circumference (in Inches): " + e.SeriesItem.YValue;
                        }
                    }
                    if (common.myStr(ddlGrowthChart2.SelectedValue).ToUpper().Equals("BMI"))
                    {
                        //RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Age (in Years)";
                        e.SeriesItem.ActiveRegion.Tooltip = "Age (in Years): " + common.myStr(((DataRowView)e.DataItem)["AgeYears"]) + " BMI: " + e.SeriesItem.YValue;
                    }
                }
                else
                {
                    //For Birth to 36 months (Height for Weight) growth charts
                    if (common.myStr(ddlMsystem.SelectedItem.Value).ToUpper().Equals("M"))
                    {
                        //RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Height (in Centimeter)";
                        //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Kiligram)";
                        e.SeriesItem.ActiveRegion.Tooltip = "Height (in Centimeter): " + common.myStr(((DataRowView)e.DataItem)["Height"]) + " Weight (in Kiligram) : " + e.SeriesItem.YValue;
                    }
                    else
                    {
                        //RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Height (in Inches)";
                        //RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Weight (in Pound)";
                        e.SeriesItem.ActiveRegion.Tooltip = "Height (in Inches): " + common.myStr(((DataRowView)e.DataItem)["Height"]) + " Weight (in Pound) " + e.SeriesItem.YValue;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void RadChart1_DataBound(object sender, EventArgs e)
    {
        //HideLabels(RadChart1);
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        if (Session["RegistrationId"] == null)
        {
            Response.Redirect("/default.aspx?RegNo=0", false);
            return;
        }
        // RadChart1.ChartTitle.TextBlock.Text = "Growth Chart: " + ddlGrowthChart2.SelectedItem.Text.ToString();
        RadChart1.ChartTitle.TextBlock.Text = "Growth Chart: " + ddlGrowthChart2.Text;
        if (common.myStr(ddlGrowthChart2.SelectedValue) == "WT")
        {
            RadChart2.ChartTitle.TextBlock.Text = "Growth Chart: " + ddlGrowthChart2.Text;
        }
        else
        {
            RadChart2.ChartTitle.TextBlock.Text = "";
        }

        //RadChart2.ChartTitle.TextBlock.Text = "Product By Categories For " + dropDownYears.SelectedItem.Text;
    }

    protected void btnHistory_Click(object sender, EventArgs e)
    {

        RadWindow2.NavigateUrl = "PreviousVitals.aspx?Mpg=P14&From=POPUP";
        RadWindow2.Height = 500;
        RadWindow2.Width = 1000;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;

    }

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {

    }

    public void BindCombo()
    {
       // BaseC.GrowthChart objbc = new BaseC.GrowthChart(sConString);
        DataTable objdt = new DataTable();
        DataView dtview = new DataView();
        DataTable dvFilter = new DataTable();
        try
        {

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindGrowthChart";
            APIRootClass.GrowthChart objRoot = new global::APIRootClass.GrowthChart();
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            objdt = ds.Tables[0];
            //objdt = objbc.BindGrowthChart(common.myInt(Session["FacilityId"]));

            ViewState["GrowthData"] = objdt.Copy();
            dtview = new DataView(objdt);
            dvFilter = dtview.ToTable(true, "AgeGroup");
            ddlGrowthChartType2.DataSource = dvFilter;
            ddlGrowthChartType2.DataTextField = "AgeGroup";
            ddlGrowthChartType2.DataValueField = "AgeGroup";
            ddlGrowthChartType2.DataBind();


            // ddlGrowthChartType2.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlGrowthChartType2.SelectedIndex = 0;
            bindddlGrowthchart2();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            //objbc = null;
            objdt.Dispose();
            dtview.Dispose();
            dvFilter.Dispose();
        }
    }

    protected void BindGrowthChartCombo()
    {
        try
        {
            ddlGrowthChart.Items.Clear();
            ddlGrowthChart.Items.Insert(0, new RadComboBoxItem("Length for Age", "HT"));
            ddlGrowthChart.Items.Insert(1, new RadComboBoxItem("Weight for Age", "WT"));
            ddlGrowthChart.Items.Insert(2, new RadComboBoxItem("Head Circumference for Age", "HC"));
            ddlGrowthChart.Items.Insert(3, new RadComboBoxItem("Height for Weight", "HW"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //protected void ddlGrowthChartType2_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DataTable DtGrowthChart = (DataTable)ViewState["GrowthData"];
    //    DataView dvGrowthChart = new DataView(DtGrowthChart);
    //    dvGrowthChart.RowFilter = "AgeGroup='" + ddlGrowthChartType2.SelectedValue + "'";
    //    DtGrowthChart = dvGrowthChart.ToTable();
    //    ddlGrowthChart2.DataSource = DtGrowthChart;
    //    ddlGrowthChart2.DataTextField = "GrowthChartName";
    //    ddlGrowthChart2.DataValueField = "GrowthChartId";
    //    ddlGrowthChart2.DataBind();
    //}

    protected void ddlGrowthChartType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(ddlGrowthChartType.SelectedValue).Equals("1"))
            {
                ddlGrowthChart.Items.Clear();
                ddlGrowthChart.Items.Insert(0, new RadComboBoxItem("Length for Age", "HT"));
                ddlGrowthChart.Items.Insert(1, new RadComboBoxItem("Weight for Age", "WT"));
                ddlGrowthChart.Items.Insert(2, new RadComboBoxItem("Head Circumference for Age", "HC"));
                ddlGrowthChart.Items.Insert(3, new RadComboBoxItem("Height for Weight", "HW"));
            }
            else
            {
                ddlGrowthChart.Items.Clear();
                ddlGrowthChart.Items.Insert(0, new RadComboBoxItem("Length for Age", "HT"));
                ddlGrowthChart.Items.Insert(1, new RadComboBoxItem("Weight for Age", "WT"));
                ddlGrowthChart.Items.Insert(2, new RadComboBoxItem("BMI for Age", "BMI"));
                ddlGrowthChart.Items.Insert(3, new RadComboBoxItem("Height for Weight", "HW"));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlGrowthChartType2_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        bindddlGrowthchart2();
        setChartColor();
    }

    protected void RadChart1_BeforeLayout(object sender, EventArgs e)
    {
        try
        {
            if (YAxisCapInterval > 0)
            {

                for (int i = 0; i < RadChart1.PlotArea.YAxis.Items.Count; i++)
                {
                    if (!(common.myInt(RadChart1.PlotArea.YAxis.Items[i].TextBlock.Text) % YAxisCapInterval).Equals(0))
                    {
                        RadChart1.PlotArea.YAxis.Items[i].Appearance.Visible = false;
                        if (i.Equals(0))
                        {
                            RadChart1.PlotArea.YAxis.Items[i].Appearance.Visible = true;
                            RadChart1.PlotArea.YAxis.Items[i].TextBlock.Text = common.myStr(YAxisCapMinValue);
                            YAxisCapMinValue = YAxisCapMinValue + YAxisCapStartvalue;
                        }

                    }
                    else
                    {
                        if (YAxisCapMinValue <= YMax)
                        {
                            RadChart1.PlotArea.YAxis.Items[i].Appearance.Visible = true;
                            if ((common.myDec(RadChart1.PlotArea.YAxis.Items[i].TextBlock.Text) % YAxisCapInterval).Equals(0))
                            {
                                RadChart1.PlotArea.YAxis.Items[i].TextBlock.Text = common.myStr(YAxisCapMinValue);
                                YAxisCapMinValue = YAxisCapMinValue + YAxisCapStartvalue;

                            }
                            else
                            {
                                RadChart1.PlotArea.YAxis.Items[i].Appearance.Visible = false;
                            }

                        }

                    }
                }
            }
            //For End For Y Y-Axis

            //For X-Axis in Rad Chart  Lable Captions
            if (common.myStr(ViewState["xaxisTypePlot"]).Equals("M")) // Months
            {
                if (Xinterval > 0)
                {
                    int CountYear = 0;
                    if (XMinVal > 0) //For Xyear Caption Start    
                    {
                        CountYear = (XMinVal / 12);
                        CountYear = CountYear + 1;
                    }
                    for (int i = 0; i < RadChart1.PlotArea.XAxis.Items.Count; i++)
                    {
                        if (!(common.myInt(RadChart1.PlotArea.XAxis.Items[i].TextBlock.Text) % Xinterval).Equals(0))
                        {
                            RadChart1.PlotArea.XAxis.Items[i].Appearance.Visible = false;
                        }

                        if (common.myInt(RadChart1.PlotArea.XAxis.Items[i].TextBlock.Text).Equals(0))
                        {
                            RadChart1.PlotArea.XAxis.Items[i].Appearance.Visible = true;
                            if (CountYear.Equals(0))
                            {
                                //RadChart1.PlotArea.XAxis.Items[i].TextBlock.Text = "1Yr.";
                                CountYear = CountYear + 1;
                            }
                            else
                            {
                                if ((XMinVal > 0 && XMinVal < 12) && (CountYear.Equals(1)))
                                {
                                    CountYear = CountYear + 1;
                                }
                                RadChart1.PlotArea.XAxis.Items[i].TextBlock.Text = common.myStr((XMinVal.Equals(0) ? CountYear : CountYear - 1)) + " Yr.";
                                CountYear = CountYear + 1;
                            }
                        }
                    }
                }
            }

            if (common.myStr(ViewState["xaxisTypePlot"]).Equals("Y")) // Months
            {
                if (Xinterval > 0)
                {
                    int CountYear = 1;
                    if (XMinVal > 0) //For Xyear Caption Start    
                    {
                        //CountYear = (XMinVal / 12);
                        CountYear = CountYear + 1;
                    }
                    for (int i = 0; i < XMaxval; i++)
                    {
                        RadChart1.PlotArea.XAxis.Items[i].TextBlock.Text = common.myStr((XMinVal.Equals(0) ? CountYear : CountYear)) + " Yr.";
                        CountYear = CountYear + 1;
                    }
                }
            }

            else if (common.myStr(ViewState["xaxisTypePlot"]).Equals("WO")) // Weeks
            {
                for (int i = 0; i < RadChart1.PlotArea.XAxis.Items.Count; i++)
                {
                    RadChart1.PlotArea.XAxis.Items[i].Appearance.Visible = true;

                    RadChart1.PlotArea.XAxis.Items[i].TextBlock.Text = common.myStr(XMinVal);
                    XMinVal = XMinVal + 1;

                    if (!(common.myInt(RadChart1.PlotArea.XAxis.Items[i].TextBlock.Text) % Xinterval).Equals(0))
                    {
                        RadChart1.PlotArea.XAxis.Items[i].Appearance.Visible = false;
                    }



                }
            }


            //End For X-axis  Chart Caption


            //For Start Y-axis Rad  Chart Caption


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void RadChart2_BeforeLayout(object sender, EventArgs e)
    {
        try
        {
            if (YAxisCapInterval2 > 0)
            {

                for (int i = 0; i < RadChart2.PlotArea.YAxis.Items.Count; i++)
                {
                    if (!(common.myInt(RadChart2.PlotArea.YAxis.Items[i].TextBlock.Text) % YAxisCapInterval2).Equals(0))
                    {
                        RadChart2.PlotArea.YAxis.Items[i].Appearance.Visible = false;
                        if (i.Equals(0))
                        {
                            RadChart2.PlotArea.YAxis.Items[i].Appearance.Visible = true;
                            RadChart2.PlotArea.YAxis.Items[i].TextBlock.Text = common.myStr(YAxisCapMinValue2);
                            YAxisCapMinValue2 = YAxisCapMinValue2 + YAxisCapStartvalue2;
                        }

                    }
                    else
                    {
                        if (YAxisCapMinValue2 <= YMax2)
                        {
                            RadChart2.PlotArea.YAxis.Items[i].Appearance.Visible = true;
                            if ((common.myDec(RadChart2.PlotArea.YAxis.Items[i].TextBlock.Text) % YAxisCapInterval2).Equals(0))
                            {
                                RadChart2.PlotArea.YAxis.Items[i].TextBlock.Text = common.myStr(YAxisCapMinValue2);
                                YAxisCapMinValue2 = YAxisCapMinValue2 + YAxisCapStartvalue2;

                            }
                            else
                            {
                                RadChart2.PlotArea.YAxis.Items[i].Appearance.Visible = false;
                            }

                        }

                    }
                }
            }
            //For End For Y Y-Axis

            //For X-Axis in Rad Chart  Lable Captions
            if (common.myStr(ViewState["xaxisTypePlot"]).Equals("M")) // Months
            {
                if (Xinterval2 > 0)
                {
                    int CountYear = 0;
                    if (XMinVal2 > 0) //For Xyear Caption Start    
                    {
                        CountYear = (XMinVal2 / 12);
                        CountYear = CountYear + 1;
                    }
                    for (int i = 0; i < RadChart2.PlotArea.XAxis.Items.Count; i++)
                    {
                        if (!(common.myInt(RadChart2.PlotArea.XAxis.Items[i].TextBlock.Text) % Xinterval2).Equals(0))
                        {
                            RadChart2.PlotArea.XAxis.Items[i].Appearance.Visible = false;
                        }

                        if (common.myInt(RadChart2.PlotArea.XAxis.Items[i].TextBlock.Text).Equals(0))
                        {
                            RadChart2.PlotArea.XAxis.Items[i].Appearance.Visible = true;
                            if (CountYear.Equals(0))
                            {
                                //RadChart1.PlotArea.XAxis.Items[i].TextBlock.Text = "1Yr.";
                                CountYear = CountYear + 1;
                            }
                            else
                            {
                                if ((XMinVal2 > 0 && XMinVal2 < 12) && (CountYear.Equals(1)))
                                {
                                    CountYear = CountYear + 1;
                                }
                                RadChart2.PlotArea.XAxis.Items[i].TextBlock.Text = common.myStr((XMinVal2.Equals(0) ? CountYear : CountYear - 1)) + " Yr.";
                                CountYear = CountYear + 1;
                            }
                        }
                    }
                }
            }

            if (common.myStr(ViewState["xaxisTypePlot"]).Equals("Y")) // Months
            {
                if (Xinterval2 > 0)
                {
                    int CountYear = 1;
                    if (XMinVal2 > 0) //For Xyear Caption Start    
                    {
                        //CountYear = (XMinVal / 12);
                        CountYear = CountYear + 1;
                    }
                    for (int i = 0; i < XMaxval2; i++)
                    {
                        RadChart2.PlotArea.XAxis.Items[i].TextBlock.Text = common.myStr((XMinVal2.Equals(0) ? CountYear : CountYear)) + " Yr.";
                        CountYear = CountYear + 1;
                    }
                }
            }

            else if (common.myStr(ViewState["xaxisTypePlot"]).Equals("WO")) // Weeks
            {
                for (int i = 0; i < RadChart2.PlotArea.XAxis.Items.Count; i++)
                {
                    RadChart2.PlotArea.XAxis.Items[i].Appearance.Visible = true;

                    RadChart2.PlotArea.XAxis.Items[i].TextBlock.Text = common.myStr(XMinVal2);
                    XMinVal2 = XMinVal2 + 1;

                    if (!(common.myInt(RadChart2.PlotArea.XAxis.Items[i].TextBlock.Text) % Xinterval2).Equals(0))
                    {
                        RadChart2.PlotArea.XAxis.Items[i].Appearance.Visible = false;
                    }



                }
            }


            //End For X-axis  Chart Caption


            //For Start Y-axis Rad  Chart Caption


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlGrowthChart2_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        cmdFilter_OnClick(null, null);
        setChartColor();
    }
    protected void bindddlGrowthchart2()
    {
        DataTable DtGrowthChart = ((DataTable)ViewState["GrowthData"]).Copy();
        DataView dvGrowthChart = new DataView(DtGrowthChart);
        try
        {
            dvGrowthChart.RowFilter = "AgeGroup='" + common.myStr(ddlGrowthChartType2.SelectedValue) + "'";
            DtGrowthChart = dvGrowthChart.ToTable();

            ddlGrowthChart2.Items.Clear();

            RadComboBoxItem item;
            foreach (DataRow dr in DtGrowthChart.Rows)
            {
                item = new RadComboBoxItem();
                item.Text = common.myStr(dr["GrowthChartName"]);
                item.Value = common.myInt(dr["DisplayName"]).ToString();
                item.Attributes.Add("GrowthChartId", common.myStr(dr["GrowthChartId"]));
                item.Attributes.Add("AssociateChartId", common.myStr(dr["AssociateChartId"]));
                ddlGrowthChart2.Items.Add(item);
                ddlGrowthChart2.DataBind();
            }

            ddlGrowthChart2.DataSource = DtGrowthChart;
            ddlGrowthChart2.DataTextField = "GrowthChartName";
            ddlGrowthChart2.DataValueField = "DisplayName";
            ddlGrowthChart2.DataBind();

            RadChart1.Clear();
            cmdFilter_OnClick(null, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            DtGrowthChart.Dispose();
            dvGrowthChart.Dispose();
        }
    }

    private void setChartColor()
    {
        try
        {
            if (common.myInt(Session["Gender"]).Equals(1))
            {
                RadChart1.Appearance.FillStyle.MainColor = Color.Pink;
                RadChart1.Appearance.FillStyle.SecondColor = Color.Pink;
                RadChart1.Appearance.FillStyle.FillType = FillType.Gradient;
                RadChart2.Appearance.FillStyle.MainColor = Color.Pink;
                RadChart2.Appearance.FillStyle.SecondColor = Color.Pink;
                RadChart2.Appearance.FillStyle.FillType = FillType.Gradient;
            }
            else if (common.myInt(Session["Gender"]).Equals(2))
            {
                RadChart1.Appearance.FillStyle.MainColor = Color.SkyBlue;
                RadChart1.Appearance.FillStyle.SecondColor = Color.SkyBlue;
                RadChart1.Appearance.FillStyle.FillType = FillType.Gradient;
                RadChart2.Appearance.FillStyle.MainColor = Color.SkyBlue;
                RadChart2.Appearance.FillStyle.SecondColor = Color.SkyBlue;
                RadChart2.Appearance.FillStyle.FillType = FillType.Gradient;
            }
            //RadChart1.PlotArea.Appearance.FillStyle.MainColor = Color.Blue;
            //RadChart1.PlotArea.Appearance.FillStyle.SecondColor = Color.Blue;
            //RadChart1.PlotArea.Appearance.FillStyle.FillType = FillType.Gradient;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


    }

}
