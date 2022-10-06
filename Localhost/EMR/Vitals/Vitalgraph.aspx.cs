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
using System.Drawing.Printing;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Vitals_Vitalgraph : System.Web.UI.Page
{
   // String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();

    int YAxisCapMinValue;
    decimal YAxisInterValue;
    int YAxisCapStartvalue;
    decimal YMax;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
      //  Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          //  dtpfromDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();
          //  dtpToDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();

            if (common.myStr(Request.QueryString["Name"]) != "")
            {
                ViewState["VitalName"] = common.myStr(Request.QueryString["Name"]);
            }
            BindControl();
            //santosh
            //foreach (RadComboBoxItem cmb in ddlVital.Items)
            //{
            //    if (common.myStr(cmb.Value) == common.myStr(ViewState["VitalName"]))
            //    {
            //        lblVitalname.Text = (common.myStr(cmb.Text));
            //    }
            //}

            // lblVitalname.Text = ddlVital.SelectedItem.Text;

            // end

            btnFilter_Click(sender, e);

        }
    }
    protected void RadChart1_DataBound(object sender, EventArgs e)
    {
        //HideLabels(RadChart1);
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        ViewState["VitalName"] = ddlVital.SelectedValue;
        BindVitalGraph();
        RadChart1.Visible = true;
    }
    void BindVitalGraph()
    {
        DataSet ds = new DataSet();
        // BaseC.EMRVitals objv = new BaseC.EMRVitals(sConString);
        try
        {

            RadChart1.Clear();
            RadChartDynamic.Clear();
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
            RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 90;
            RadChart1.Visible = true;
            RadChartDynamic.Visible = false;
            pnlRadcahrt1.Visible = true;
            pnlRadChartDynamic.Visible = false;

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/VitalGraph";
            APIRootClass.VitalGraph objRoot = new global::APIRootClass.VitalGraph();
            objRoot.HospId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacId = common.myInt(Session["FacilityId"]);
            objRoot.RegId = common.myInt(Session["RegistrationId"]);
            objRoot.DiasplayName = common.myStr(ViewState["VitalName"]);
            objRoot.DateRange = common.myStr(ddldateRange.SelectedValue);
            objRoot.FDate = ddldateRange.SelectedValue == "6" ? dtpfromDate.SelectedDate.Value.ToString("yyyy/MM/dd") : "";
            objRoot.TDate = ddldateRange.SelectedValue == "6" ? dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd") : "";
            
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //ds = objv.VitalGraph(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myStr(ViewState["VitalName"]), common.myStr(ddldateRange.SelectedValue),
            //    ddldateRange.SelectedValue == "6" ? dtpfromDate.SelectedDate.Value.ToString("yyyy/MM/dd") : "", ddldateRange.SelectedValue == "6" ? dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd") : "");

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myInt(ds.Tables[1].Rows[0]["YAxisMinValue"]) >= 0 && common.myInt(ds.Tables[1].Rows[0]["YAxisMaxValue"]) > 0 && common.myDbl(ds.Tables[1].Rows[0]["YAxisCapStartValue"]) > 0 && common.myDbl(ds.Tables[1].Rows[0]["YAxisInterval"]) > 0)
                    {
                        if (common.myStr(ds.Tables[0].Rows[0]["UnitName"]) != "")
                        {
                            RadChartDynamic.ChartTitle.TextBlock.Text = common.myStr(ddlVital.SelectedItem.Text) + " (" + common.myStr(ds.Tables[0].Rows[0]["UnitName"].ToString()) + ")";
                        }
                        else
                        {
                            RadChartDynamic.ChartTitle.TextBlock.Text = common.myStr(ddlVital.SelectedItem.Text);
                        }
                        RadChart1.Visible = false;
                        RadChartDynamic.Visible = true;
                        pnlRadcahrt1.Visible = false;
                        pnlRadChartDynamic.Visible = true;


                        BindDynamicVitalGraph(ds);
                    }
                    else
                    {
                        if (common.myStr(ds.Tables[0].Rows[0]["UnitName"]) != "")
                        {
                            RadChart1.ChartTitle.TextBlock.Text = common.myStr(ddlVital.SelectedItem.Text) + " (" + common.myStr(ds.Tables[0].Rows[0]["UnitName"].ToString()) + ")";
                        }
                        else
                        {
                            RadChart1.ChartTitle.TextBlock.Text = common.myStr(ddlVital.SelectedItem.Text);
                        }
                        //RadChart1.Visible =true ;
                        //RadChartDynamic.Visible =false ;
                        pnlRadcahrt1.Visible = true;
                        pnlRadChartDynamic.Visible = false;
                        RadChart1.PlotArea.YAxis.AutoScale = true;
                        RadChart1.DataSource = ds;
                        RadChart1.PlotArea.XAxis.DataLabelsColumn = "entrydate";
                        RadChart1.PlotArea.YAxis.AxisMode = Telerik.Charting.ChartYAxisMode.Extended;
                        RadChart1.DataBind();
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
            ds.Dispose();
            //objv = null;
        }
    }
    void BindDynamicVitalGraph(DataSet ds)
    {
        RadChartDynamic.Clear();

        RadChartDynamic.Legend.Items.Clear();
        RadChartDynamic.ClearSkin();
        RadChartDynamic.Skin = "Inox";

        RadChartDynamic.DataSource = ds;
        RadChartDynamic.PlotArea.XAxis.DataLabelsColumn = "entrydate";
        RadChartDynamic.PlotArea.YAxis.AxisLabel.TextBlock.Visible = false;


        // YAxisCapInterval = common.myInt(ds.Tables[1].Rows[0]["YAxisCapInterval"]);
        YAxisCapMinValue = common.myInt(ds.Tables[1].Rows[0]["YAxisMinValue"]);
        YAxisInterValue = common.myDec(ds.Tables[1].Rows[0]["YAxisInterval"]);
        YAxisCapStartvalue = common.myInt(ds.Tables[1].Rows[0]["YAxisCapStartValue"]);
        YMax = common.myDec(common.myDbl(ds.Tables[1].Rows[0]["YAxisMaxValue"]));


        RadChartDynamic.PlotArea.YAxis.MinValue = common.myInt(ds.Tables[1].Rows[0]["YAxisMinValue"]);
        RadChartDynamic.PlotArea.YAxis.MaxValue = common.myInt(ds.Tables[1].Rows[0]["YAxisMaxValue"]);
        RadChartDynamic.PlotArea.YAxis.Step = common.myDbl(ds.Tables[1].Rows[0]["YAxisInterVal"]);
        RadChartDynamic.Chart.Appearance.Border.Color = System.Drawing.Color.Black;
        // RadChartDynamic.DefaultView.ChartArea.AxisY.Step = 5;

        ChartSeries PatientValue = new ChartSeries("VitalValue");
        //PatientValue.Type = ChartSeriesType.Line;
        //PatientValue.DataYColumn = "VitalValue";
        //RadChartDynamic.Series.Add(PatientValue);
        //PatientValue.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
        //PatientValue.Appearance.EmptyValue.Line.Color = System.Drawing.Color.Transparent;
        //PatientValue.Appearance.EmptyValue.PointMark.Visible = false;

        //PatientValue.Appearance.PointMark.Visible = true;
        // PatientValue.Appearance.PointMark.Border.Width = 2;
        //PatientValue.Appearance.PointMark.Border.Color = System.Drawing.Color.Blue;
        //PatientValue.Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(12);
        //PatientValue.Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(12);
        //PatientValue.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.DarkGray;
        //PatientValue.Appearance.PointMark.FillStyle.FillType = FillType.Solid;       
        RadChartDynamic.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 90;
        if (common.myStr(ddlgraphtype.SelectedValue) == "Line")
        {
            PatientValue.Type = ChartSeriesType.Line;
            PatientValue.DataYColumn = "VitalValue";
            RadChartDynamic.Series.Add(PatientValue);

            // RadChartDynamic.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            // PatientValue.Appearance.LineSeriesAppearance.Color = Color.Blue;
            ////PatientValue.Appearance.LineSeriesAppearance.Width = 2;
            PatientValue.Appearance.TextAppearance.TextProperties.Color = Color.Black;
        }
        else
        {
            PatientValue.Type = ChartSeriesType.Bar;
            PatientValue.DataYColumn = "VitalValue";
            RadChartDynamic.Series.Add(PatientValue);

            //RadChartDynamic.DefaultType = Telerik.Charting.ChartSeriesType.Bar;
            //PatientValue.Appearance.FillStyle.MainColor = Color.Blue;            
            //PatientValue.Appearance.LineSeriesAppearance.Width = 2;
            PatientValue.Appearance.TextAppearance.TextProperties.Color = Color.Black;


        }

        RadChartDynamic.DataBind();
        RadChartDynamic.Legend.Appearance.Visible = true;


    }
    protected void ddlgraphtype_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        BindVitalGraph();
    }
    protected void ddlVital_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        // BindControl();
        ViewState["VitalName"] = ddlVital.SelectedValue;
        //    lblVitalname.Text = ddlVital.SelectedItem.Text;
        BindVitalGraph();
    }

    void BindControl()
    {
        DataSet ds = new DataSet();
        // BaseC.EMRVitals objv = new BaseC.EMRVitals(sConString);
        try
        {
            ddlVital.Items.Clear();
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GetVitalSignName";

            APIRootClass.vital objRoot = new global::APIRootClass.vital();
            objRoot.date = common.myStr("");
            objRoot.Hospitalid = common.myInt(Session["HospitalLocationID"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
             ds = JsonConvert.DeserializeObject<DataSet>(sValue);

           // ds = objv.GetVitalSignName("", common.myInt(Session["HospitalLocationId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlVital.DataSource = ds;
                ddlVital.DataTextField = "VitalSignName";
                ddlVital.DataValueField = "DisplayName";
                ddlVital.DataBind();
            }
            ddlVital.SelectedIndex = ddlVital.Items.IndexOf(ddlVital.Items.FindItemByValue(common.myStr(ViewState["VitalName"])));
            ViewState["VitalName"] = ddlVital.SelectedValue;
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
            //objv = null;
        }
    }
    protected void ddldateRange_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        RadChart1.Visible = false;
        RadChartDynamic.Visible = false;
        pnlRadcahrt1.Visible = false;
        pnlRadChartDynamic.Visible = false;
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

            RadWindow1.NavigateUrl = "~/EMR/Vitals/PrintVitalGraph.aspx?VitalName=" + common.myStr(ddlVital.SelectedValue) +
                                    "&GraphType=" + graphType + "&DateRange=" + common.myStr(ddldateRange.SelectedValue) +
                                    "&FDate=" + fromDate + "&TDate=" + toDate;

            RadWindow1.Height = 580;
            RadWindow1.Width = 1000;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            //RadWindow1.Title = "Time Slot";
            RadWindow1.OnClientClose = "OnClientClose";
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

    //void export()
    //{
    //    RadChart1.Save(Server.MapPath("~/images/Chart.png"), System.Drawing.Imaging.ImageFormat.Png);
    //    iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 72, 72, 82, 72);
    //    MemoryStream msReport = new MemoryStream();

    //    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, msReport);
    //    document.AddAuthor("Test");
    //    document.AddSubject("Export to PDF");
    //    document.Open();
    //    iTextSharp.text.Chunk c = new iTextSharp.text.Chunk("Export chart to PDF", iTextSharp.text.FontFactory.GetFont("VERDANA", 15));
    //    iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph();
    //    p.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
    //    iTextSharp.text.Image hImage;
    //    hImage = iTextSharp.text.Image.GetInstance(MapPath("~/images/Chart.png"));

    //    float NewWidth = 500;
    //    float MaxHeight = 400;

    //    if (hImage.Width <= NewWidth) { NewWidth = hImage.Width; } float NewHeight = hImage.Height * NewWidth / hImage.Width; if (NewHeight > MaxHeight)
    //    {
    //        NewWidth = hImage.Width * MaxHeight / hImage.Height;
    //        NewHeight = MaxHeight;
    //    }

    //    float ratio = hImage.Width / hImage.Height;
    //    hImage.ScaleAbsolute(NewWidth, NewHeight);
    //    document.Add(p);
    //    document.Add(hImage);
    //    document.Close();

    //    Response.AddHeader("Content-type", "application/pdf");
    //    Response.AddHeader("Content-Disposition", "attachment; filename=chart.pdf");
    //    Response.OutputStream.Write(msReport.GetBuffer(), 0, msReport.GetBuffer().Length);

    //}

    protected void RadChartDynamic_BeforeLayout(object sender, EventArgs e)
    {
        if (YAxisCapStartvalue > 0)
        {
            for (int i = 0; i < RadChartDynamic.PlotArea.YAxis.Items.Count; i++)
            {
                //if (YAxisCapStartvalue >= YAxisInterValue)
                //{
                if (common.myInt(RadChartDynamic.PlotArea.YAxis.Items[i].TextBlock.Text) == YAxisCapMinValue)
                {
                    // RadChartDynamic.PlotArea.YAxis.Items[i].Appearance.Visible = false;
                    //if (i == 0)
                    //{
                    RadChartDynamic.PlotArea.YAxis.Items[i].Appearance.Visible = true;
                    // RadChartDynamic.PlotArea.YAxis.Items[i].TextBlock.Text = common.myStr(YAxisCapMinValue);
                    YAxisCapMinValue = YAxisCapMinValue + YAxisCapStartvalue;
                    //}

                }
                else
                {
                    RadChartDynamic.PlotArea.YAxis.Items[i].Appearance.Visible = false;
                }
                //}
            }
        }
    }
}
