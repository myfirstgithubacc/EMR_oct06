using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Configuration;

public partial class EMR_Dashboard_ProviderParts_LabResult : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;
    private bool gRefresh = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        dtpfromDate.DateInput.DateFormat = common.myStr(Session["OutputDateformat"]);
        //dtpfromDate.DateInput.DisplayDateFormat = common.myStr(Session["OutputDateformat"]);
        dtpToDate.DateInput.DateFormat = common.myStr(Session["OutputDateformat"]);
        //dtpToDate.DateInput.DisplayDateFormat = common.myStr(Session["OutputDateformat"]);
        //if (!IsPostBack)
        //{
        //dtpfromDate.SelectedDate = DateTime.Now;
        //dtpToDate.SelectedDate = DateTime.Now;
        BaseC.clsEMRBilling objval1 = new BaseC.clsEMRBilling(sConString);
        DataSet DSS = objval1.getEntrySite(common.myInt(Session["UserID"]), common.myInt(Session["FacilityId"]));
        ddlEntrySitesActual.DataSource = DSS;
        ddlEntrySitesActual.DataValueField = "ESId";
        ddlEntrySitesActual.DataTextField = "ESName";
        ddlEntrySitesActual.DataBind();
        //ddlEntrySitesDiag.Items.Insert(0, new RadComboBoxItem("", "0"));
        //ddlEntrySitesDiag.SelectedIndex = 0;
        ddlEntrySitesActual.SelectedIndex = ddlEntrySitesActual.Items.IndexOf(ddlEntrySitesActual.Items.FindItemByValue(common.myStr(Session["EntrySite"])));
        bindMainData();
        //}
        loadLegend();
       

    }
   

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        if (dtpfromDate.SelectedDate == null || dtpToDate.SelectedDate == null)
        {
            lblMessage.Text = "<b>Please Enter Date ! </b>";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }
        bindMainData();
    }

    private void bindMainData()
    {
        try
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);

            int stationID = 0;
            //stationID = common.myInt(Session["StationId"]);

            DataSet ds = objval.GetResultFinalization(common.myStr("OPD"), common.myInt(Session["HospitalLocationID"]),
                            0, common.myInt(Session["FacilityID"]), stationID, 0,
                            common.myDate(dtpfromDate.SelectedDate), common.myDate(dtpToDate.SelectedDate), "", 0, 0, "", Convert.ToInt32(Session["UserID"])
                            , 0, 0, 0, "", 0, 0, 0, string.Empty, 0, 0, "", 0, "", true, common.myInt(ddlEntrySitesActual.SelectedValue));

            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "LabNo <> 0";

            gvResultFinal.DataSource = dv.ToTable().Copy();
            gvResultFinal.DataBind();

            SetGridColor();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void SetGridColor()
    {
        try
        {
            foreach (GridDataItem dataItem in gvResultFinal.MasterTableView.Items)
            {
                if (dataItem.ItemType == GridItemType.Item
                    || dataItem.ItemType == GridItemType.AlternatingItem
                    || dataItem.ItemType == GridItemType.SelectedItem)
                {
                    Label lblStatusColor = (Label)dataItem.FindControl("lblStatusColor");
                    dataItem.BackColor = System.Drawing.Color.FromName(common.myStr(lblStatusColor.Text));
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

    protected void gvResultFinal_OnItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {

        }
    }



    protected void gvResultFinal_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            (e.Column as GridGroupSplitterColumn).ExpandImageUrl = "~/Images/Plusbox.gif";
            (e.Column as GridGroupSplitterColumn).CollapseImageUrl = "~/Images/Minubox.gif";

            e.Column.HeaderStyle.Width = Unit.Percentage(3);

            e.Column.ItemStyle.Width = Unit.Percentage(3);
            e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            e.Column.ItemStyle.VerticalAlign = VerticalAlign.Top;
        }
    }

    protected void gvResultFinal_PreRender(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["SaveSignature"]) == "Yes")
        {
            SetGridColor();
            Session["SaveSignature"] = "No";
            return;
        }

        if (gRefresh == false)
        {
            bindMainData();
            return;
        }


        SetGridColor();
    }

    protected void gvResultFinal_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item
            || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Label lblresult = (Label)e.Item.FindControl("lblresult");
            if (lblresult.Text.Trim() == "Result")
            {
                LinkButton lnkResult = (LinkButton)e.Item.FindControl("lnkResult");
                lblresult.Visible = false;
                lnkResult.Visible = true;
            }
        }
        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem groupHeader = (GridGroupHeaderItem)e.Item;
            string strCellText = groupHeader.DataCell.Text;
            string strLabNo = groupHeader.Cells[1].Text.ToString().Split(';')[0].Substring(10);
            LinkButton btn1 = new LinkButton();
            btn1.ID = "btnPrintLabResult";
            btn1.Text = strCellText;
            btn1.CommandName = "Print";
            btn1.CommandArgument = strLabNo;
            //btn1.Click += new CommandEventHandler(btnPrintLabResult_Click);
            btn1.ToolTip = "Click here to Print";

            groupHeader.DataCell.Controls.Add(btn1);
            groupHeader.DataCell.Width = Unit.Pixel(600);
        }
    }

    protected void gvResultFinal_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "lnkResult")
        {
            Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
            Label lblStationId = (Label)e.Item.FindControl("lblStationId");
            Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
            Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");

            RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/InvestigationResult.aspx?SOURCE=OPD&LABNO=" + common.myInt(lblLabNo.Text) + "&MASTER=Y" +
                                        "&StationId=" + common.myInt(lblStationId.Text) +
                                        "&SEL_DiagSampleID=" + common.myInt(lblDiagSampleID.Text) +
                                        "&SEL_ServiceId=" + common.myInt(lblServiceId.Text);

            RadWindowPopup.Height = 600;
            RadWindowPopup.Width = 924;
            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            RadWindowPopup.VisibleOnPageLoad = true;
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleStatusbar = false;
        }
        if (e.CommandName == "Print")
        {
            RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=OPD&LABNO=" + common.myInt(e.CommandArgument);

            RadWindowPopup.Height = 600;
            RadWindowPopup.Width = 924;
            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            RadWindowPopup.VisibleOnPageLoad = true;
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleStatusbar = false;
        }
    }

    private DataTable GetLegend()
    {
        DataSet ds;

        objval = new BaseC.clsLISPhlebotomy(sConString);
        ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LAB", "");
        DataView dv = new DataView(ds.Tables[0]);

        dv.RowFilter = "Code IN('RE','RF') ";
        DataTable dt = new DataTable();
        return dt = dv.ToTable();
    }

    public void loadLegend()
    {
        try
        {
            DataTable dt = GetLegend();
            if (dt.Rows.Count < 1)
            {
                return;
            }

            Label LBL;

            TableRow tr;
            TableCell td;

            int ROWS = 1;
            int COLS = dt.Rows.Count;
            DataRow DR;

            for (int rowIdx = 0; rowIdx < ROWS; rowIdx++)
            {
                tr = new TableRow();

                for (int colIdx = 0; colIdx < COLS; colIdx++)
                {
                    DR = dt.Rows[colIdx];

                    td = new TableCell();
                    LBL = new Label();
                    LBL.BorderWidth = Unit.Pixel(1);
                    LBL.ID = "LabelStatusColor" + colIdx;
                    LBL.BackColor = System.Drawing.Color.FromName(common.myStr(DR["StatusColor"]));
                    LBL.SkinID = "label";
                    LBL.Width = Unit.Pixel(22);
                    LBL.Height = Unit.Pixel(16);

                    td.Controls.Add(LBL);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    LBL = new Label();
                    LBL.ID = "LabelStatus" + colIdx;
                    LBL.Text = common.myStr(DR["Status"]).Replace(" ", "&nbsp;");
                    LBL.Font.Size = 8;

                    LBL.SkinID = "label";

                    td.Controls.Add(LBL);
                    tr.Cells.Add(td);
                }

                tblLegend.Rows.Add(tr);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
}
