using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;
using Telerik.Web.UI;

public partial class EMRBILLING_PatientHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Convert.ToString(Request.QueryString["Popup"]) != "Y")
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        dtpFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
        dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
        if (!Page.IsPostBack)
        {
            if (common.myStr(Request.QueryString["Popup"]) == "Y")
            {
                txtRegNo.Text = common.myStr(Request.QueryString["RegNo"]);
                FillPatientDetail();
            }
            dtpFromDate.SelectedDate = System.DateTime.Now.Date;
            dtpToDate.SelectedDate = System.DateTime.Now.Date;
            FillReasonDispatchType();
        }
    }
    public void FillPatientDetail()
    {
        BaseC.EMRBilling objEmrBilling = new BaseC.EMRBilling(sConString);
        DataSet ds = new DataSet();
        try
        {
            //common.myLong(txtRegNo.Text) <= 0 ||
            //Added by ujjwal 24 June 2015 to validate UHID start
            int UHID;
            int.TryParse(txtRegNo.Text, out UHID);
            if ((UHID > 2147483647 || UHID.Equals(0)) && !common.myLen(txtRegNo.Text).Equals(0))
            {
                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMsg.Text = "Invalid UHID";
                return;
            }
            //Added by ujjwal 24 June 2015 to validate UHID start

            ds = objEmrBilling.GetFillPatientVisitHistory(common.myInt(Session["HospitallocationId"]), common.myInt(Session["FacilityId"]), txtRegNo.Text,
                        Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate));
            if (ds.Tables.Count > 0)
            {
                if (common.myStr(Request.QueryString["Popup"]) == "Y")
                {
                    ds.Tables[1].DefaultView.RowFilter = "StoreId > 0";
                    grvPatientHistory.DataSource = ds.Tables[1].DefaultView;
                }
                else
                {
                    grvPatientHistory.DataSource = ds.Tables[1];
                }


                grvPatientHistory.DataBind();
                lblDetail.Text = common.myStr(ds.Tables[0].Rows[0]["PatientName"])
                    + "  /  "
                    + common.myStr(ds.Tables[0].Rows[0]["AgeGender"])
                    + "  /  "
                    + common.myStr(ds.Tables[0].Rows[0]["PatientAddress"])
                    + "  /  "
                    + common.myStr(ds.Tables[0].Rows[0]["MobileNo"])
                    ;

            }
            else
            {
                grvPatientHistory.DataSource = null;
                grvPatientHistory.DataBind();
                lblDetail.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            lblMsg.Text = ex.ToString();
        }
        finally
        {
            ds.Dispose();
            objEmrBilling = null;
        }
    }
    protected void BtnSearch_OnClick(object sender, EventArgs e)
    {
        FillPatientDetail();
    }
    protected void lnkPrint_OnClick(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        GridDataItem grv = (GridDataItem)lnk.NamingContainer;
        HiddenField InvoiceId = (HiddenField)grv.FindControl("hdnInvoiceId");
        HiddenField YearId = (HiddenField)grv.FindControl("hdnYearId");
        HiddenField BillType = (HiddenField)grv.FindControl("hdnBillType");
        HiddenField hdnOPIP = (HiddenField)grv.FindControl("hdnOPIP");
        HiddenField hdnEncounterId = (HiddenField)grv.FindControl("hdnEncounterId");
        HiddenField hdninvoiceDate = (HiddenField)grv.FindControl("hdninvoiceDate");
        HiddenField hdnAdmissionDate = (HiddenField)grv.FindControl("hdnAdmissionDate");
        HiddenField hdnRegistrationId = (HiddenField)grv.FindControl("hdnRegistrationId");
        HiddenField hdnStoreId = (HiddenField)grv.FindControl("hdnStoreId");
        HiddenField hdnSaleSetupId = (HiddenField)grv.FindControl("hdnSaleSetupId");
        HiddenField hdnSaleType = (HiddenField)grv.FindControl("hdnSaleType");
        HiddenField hdnIssueId = (HiddenField)grv.FindControl("hdnIssueId");




        if (hdnOPIP.Value == "OPD")
        {
            RadWindow1.NavigateUrl = "/EMRREPORTS/PrintOPDReceipt.aspx?InvId=" + InvoiceId.Value + "&YearId="
            + YearId.Value + "&PrintType=Bill&Billtype=" + BillType.Value + "&CurrencySymbol=' '";
        }
        else if (hdnOPIP.Value == "IPD")
        {
            string strShowAdvance = "Y", strShowDiscount = "Y";
            int iShowOtherService = 0;
            int IsFilterByDate = 0;
            RadWindow1.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?EncId=" + common.myInt(hdnEncounterId.Value)
                + "&RptName=IPBill&RptType=D"
                + "&BillId=" + common.myInt(InvoiceId.Value)
                + "&AdmDt=" + common.myStr(hdnAdmissionDate.Value)
                + "&Adv=" + strShowAdvance
                + "&Disc=" + strShowDiscount + "&RegId="
                + common.myInt(hdnRegistrationId.Value)
                + "&FromDate="
                + common.myDate(hdnAdmissionDate.Value).ToString("dd/MM/yyyy HH:mm")
                + "&ToDate=" + common.myDate(hdninvoiceDate.Value).ToString("dd/MM/yyyy HH:mm")
                + "&IsFilterByDate=" + IsFilterByDate
                + "&ReportType=D"
                + "&ShowOtherService=" + common.myInt(iShowOtherService);
        }
        else if (hdnOPIP.Value == "OP Pharmacy Return")
        {
            Session["StoreId"] = common.myInt(hdnStoreId.Value);
            RadWindow1.NavigateUrl = "~/Pharmacy/Reports/ReturnDocReport.aspx?IssueId="
              + common.myInt(hdnIssueId.Value)
              + "&StoreId=" + common.myInt(hdnStoreId.Value)
              + "&SetupId=" + common.myInt(hdnSaleSetupId.Value);
        }
        else if (hdnOPIP.Value == "OP Pharmacy")
        {
            RadWindow1.NavigateUrl = "~/Pharmacy/Reports/CashSaleReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value)
                 + "&SetupId=" + common.myInt(hdnSaleSetupId.Value)
                 + "&seltype=" + common.myStr(hdnSaleType.Value)
                 + "&RptHeader=" + common.myStr(hdnSaleType.Value);
        }
        else if ((hdnOPIP.Value == "OP Refund") || (hdnOPIP.Value == "IP Refund"))
        {
            RadWindow1.NavigateUrl = "/EMRREPORTS/PrintOPIPRefund.aspx?RefundNo="
                + common.myStr(lnk.Text)
                + "&YearId=" + common.myInt(YearId.Value)
                + "&RefundId=" + common.myInt(InvoiceId.Value)
                + "&RefundType=2";

        }

        RadWindow1.Height = 600;
        RadWindow1.Width = 750;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindow1.VisibleStatusbar = false;
    }


    protected void grvPatientHistory_CustomAggregate(object sender, GridCustomAggregateEventArgs e)
    {
        if (((Telerik.Web.UI.GridBoundColumn)e.Column).UniqueName == "RefundAmt")
        {

            Double rooms = 0;
            foreach (GridDataItem item in grvPatientHistory.MasterTableView.Items)
            {
                if (Convert.ToDouble(item["InvoiceAmount"].Text) > 0)
                    rooms += Convert.ToDouble(item["RefundAmt"].Text);
            }
            e.Result = rooms;
        }
    }

    protected void grvPatientHistory_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            TableCell cellINVStatus = (TableCell)item["INVStatus"];
            string INVStatus = cellINVStatus.Text;

            Button btn = e.Item.FindControl("btnMarkDet") as Button;
            HiddenField hdnRowType = (HiddenField)e.Item.FindControl("hdnRowType");
            HiddenField hdnDetectableID = (HiddenField)e.Item.FindControl("hdnDetectableID");
            LinkButton lnkPrint = (LinkButton)e.Item.FindControl("lnkPrint");
            
            TableCell cellInvoiceAmount = (TableCell)item["InvoiceAmount"];
            decimal InvoiceAmount = common.myDec(cellInvoiceAmount.Text);

            if (INVStatus.ToUpper().Equals("CANCELLED") && InvoiceAmount < 0)
            {
                lnkPrint.Enabled = false;
                btn.Visible = false;
            }
            if (common.myStr(hdnRowType.Value).Equals("REC"))
            {
                lnkPrint.Enabled = false;
                btn.Visible = false;
            }
            if (common.myInt(hdnDetectableID.Value) > 0)
            {
                lnkPrint.ForeColor = Color.Red;
                lnkPrint.Enabled = false;
                lnkPrint.Font.Bold = true;
            }

        }
    }


    protected void btnMarkDet_Click(object sender, EventArgs e)
    {
        try
        {
            dvx.Visible = true;
            btnupdate.CommandName = (sender as Button).CommandName;
            BaseC.EMRBilling bill = new BaseC.EMRBilling(sConString);
            DataSet ds = bill.Getdetectabledetails(common.myInt((sender as Button).CommandName));
            chkactive.Checked = true;
            txtreason.Text = "";
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtreason.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
                    chkactive.Checked = (common.myBool(ds.Tables[0].Rows[0]["Active"].ToString()));
                }
            }
            ViewState["defaultreasonvalue"] = txtreason.Text;
            if (rndreasoninvoicedispatch.SelectedItem.Text == "Select")
            {
                txtreason.ReadOnly = true;
            }
            else if(rndreasoninvoicedispatch.SelectedItem.Text == "")
            {
                txtreason.ReadOnly = false;
            }
        }
        catch (Exception exx)
        {
            lblMsg.Text = exx.Message;
        }
        rndreasoninvoicedispatch.SelectedIndex = 0;
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (!common.myStr(txtreason.Text).Equals(""))
            {
                BaseC.EMRBilling bill = new BaseC.EMRBilling(sConString);
                string str = bill.updateDetactable(common.myInt(btnupdate.CommandName), txtreason.Text.Replace("'", ""), (chkactive.Checked) == true ? 1 : 0, common.myInt(Session["UserID"]));
                lblMsg.Text = common.myStr(str);
                dvx.Visible = false;
                FillPatientDetail();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }

    protected void btnclose_Click(object sender, EventArgs e)
    {
        dvx.Visible = false;
        txtreason.Text = "";
        rndreasoninvoicedispatch.SelectedIndex = 0;
    }

    //public void FillReasonDispatchType()
    //{
    //    StringBuilder strSQL = new StringBuilder();
    //    strSQL.Append("select * from ReasonMaster Where ReasonType = 'InvoiceDispatch' and Active=1");
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

    //    DataSet ds = dl.FillDataSet(CommandType.Text, strSQL.ToString());
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        rndreasoninvoicedispatch.DataSource = ds.Tables[0];
    //        rndreasoninvoicedispatch.DataTextField = "Reason";
    //        rndreasoninvoicedispatch.DataValueField = "Id";
    //        rndreasoninvoicedispatch.DataBind();
    //        rndreasoninvoicedispatch.Items.Insert(0, new RadComboBoxItem(" Select ", "0"));
    //        rndreasoninvoicedispatch.Items.Insert(1, new RadComboBoxItem(" Other ", "1"));
    //        rndreasoninvoicedispatch.SelectedIndex = 0;
    //    }
    //}

    public void FillReasonDispatchType()
    {
        BaseC.EMRBilling objEmrBilling = new BaseC.EMRBilling(sConString);
        DataSet ds = new DataSet();
    
        ds = objEmrBilling.GetReasontype("InvoiceDetectable");
        if (ds.Tables[0].Rows.Count > 0)
        {
            rndreasoninvoicedispatch.DataSource = ds.Tables[0];
            rndreasoninvoicedispatch.DataTextField = "Reason";
            rndreasoninvoicedispatch.DataValueField = "Id";
            rndreasoninvoicedispatch.DataBind();
        }
        rndreasoninvoicedispatch.Items.Insert(0, new RadComboBoxItem(" Select ", "0"));
        rndreasoninvoicedispatch.Items.Insert(1, new RadComboBoxItem(" Other ", "1"));
        rndreasoninvoicedispatch.SelectedIndex = 0;
    }

    protected void rndreasoninvoicedispatch_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
       
        txtreason.Text = "";
        if(rndreasoninvoicedispatch.SelectedItem.Text == " Select ")
        {
            txtreason.Text = common.myStr(ViewState["defaultreasonvalue"]);
            txtreason.ReadOnly = true;
        }
      else if(rndreasoninvoicedispatch.SelectedItem.Text == " Other ")
        {
            txtreason.ReadOnly =false;
        }
        else
        {
            txtreason.Text = rndreasoninvoicedispatch.SelectedItem.Text;
            txtreason.ReadOnly = true;
        }
    }
}
