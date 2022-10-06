using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using Telerik.Web.UI;
using Telerik.Charting;
using Telerik.Charting.Styles;

public partial class EMR_ICCAChartView : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindChart();
        }
    }
    private void BindChart()
    {
        
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = dl.FillDataSet(CommandType.Text, "[uspGetChartParameter] " + common.myInt(Session["RegistrationID"]).ToString() + ",'" + common.myStr(Request.QueryString["FromDate"]) + "','" + common.myStr(Request.QueryString["ToDate"]) + "'");
        gvchartdata.DataSource = ds.Tables[0];
        gvchartdata.DataBind();
    }
    protected void btnClosex1_Onclick(object sender, EventArgs e)
    {
       
        Chart.DataSource = null;
        Chart.DataBind();
    }
    DataTable ChartDAta(int strtrow)
    {
        DataTable dt = new DataTable("Chart");
        if (ViewState["ChartData"] != null)
        {
            dt.Columns.Add("Parameter");
            dt.Columns.Add("DAtetime");
            dt.Columns.Add("Value");
        }
        DataSet ds = (DataSet)ViewState["ChartData"];
        if (strtrow + 5 >= ds.Tables[0].Rows.Count)
        {
            strtrow = 0;
        }
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (i >= strtrow && i <= (strtrow + 5))
            {
                DataRow dr = dt.NewRow();
                dr["Parameter"] = ds.Tables[0].Rows[i]["Parameter"];
                dr["DAtetime"] = ds.Tables[0].Rows[i]["DAtetime"];
                dr["Value"] = ds.Tables[0].Rows[i]["Value"];
                dt.Rows.Add(dr);
                ViewState["CurRow"] = i.ToString();

            }

        }
        return dt;
    }
    protected void BtnPlay_Onclick(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.Text.ToString().ToUpper() == "PLAY")
        {
            timer.Enabled = true;
            btnPlayChart.Visible = false;
            btnPlStop.Visible = true;
        }
        else
        {
            btnPlayChart.Visible = true;
            btnPlStop.Visible = false;
            timer.Enabled = false;

            Chart.Appearance.FillStyle.SecondColor=System.Drawing.Color.Blue;

            //Chart.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            Chart.DataSource = ChartDAta(0);
            Chart.PlotArea.XAxis.DataLabelsColumn = "OutputDate";
            //Chart.PlotArea.XAxis.DataLabelsColumn = "OutputDate";
            Chart.DataBind();

        }
    }
    public void ItemDataBound(object sender, ChartItemDataBoundEventArgs e)
    {
        
        try
        {
            e.ChartSeries.Appearance.FillStyle.MainColor =System.Drawing.Color.FromName(common.myStr("Blue"));
            e.ChartSeries.PlotArea.YAxis.MaxValue = 300;
           // Chart.PlotArea.YAxis.MaxValue = 300;
        }
        catch
        {
        }
        finally
        {
           
        }
    }
    protected void timmer_tick(object sender, EventArgs e)
    {
        playchart(common.myStr(ViewState["Title"]));
    }
    void playchart(string header)
    {
        //BtnPlay_Onclick
        Chart.DataSource = null;
        Chart.DataBind();

        Chart.Legend.Items.Clear();
        Chart.ClearSkin();
        // RadChart1.Skin = ddlSkin.SelectedValue.ToString();

        Chart.DefaultType = Telerik.Charting.ChartSeriesType.Line;
        Chart.ChartTitle.TextBlock.Text = common.myStr(header);
        Chart.DataSource = ChartDAta(common.myInt(ViewState["CurRow"]));
        Chart.PlotArea.XAxis.DataLabelsColumn = "OutputDate";
        //Chart.PlotArea.XAxis.DataLabelsColumn = "OutputDate";
        Chart.PlotArea.YAxis.AxisMode = Telerik.Charting.ChartYAxisMode.Extended;

        Chart.DataBind();
    }
    protected void btnXview_Onclock(object sender, EventArgs e)
    {
        Chart.DataSource = null;
        Chart.DataBind();
        Button btn = (Button)sender;
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = dl.FillDataSet(CommandType.Text, "[uspGetICCAChartData] " + common.myInt(Session["RegistrationID"]).ToString() + ",'" + common.myStr(Request.QueryString["FromDate"]) + "','" + common.myStr(Request.QueryString["ToDate"]) + "'," + common.myInt(btn.CommandName.ToString()).ToString() + "," + common.myInt(ddltimeinterv.SelectedValue));
        //ds.Tables[0].DefaultView.RowFilter="RowNo between 1 to 5";
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ChartData"] = ds;
                ViewState["Title"] = btn.Text;

                Chart.DefaultType = Telerik.Charting.ChartSeriesType.Line;
                Chart.ChartTitle.TextBlock.Text = common.myStr(btn.Text);
                Chart.DataSource = ChartDAta(0);
                Chart.PlotArea.XAxis.DataLabelsColumn = "OutputDate";
                //Chart.PlotArea.XAxis.DataLabelsColumn = "OutputDate";
                Chart.DataBind();
            }
        }

    }
    
}