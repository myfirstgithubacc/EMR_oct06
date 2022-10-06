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
using System.Drawing;
using System.IO;
using System.Xml;
using Telerik.Web.UI;

public partial class WardManagement_WardDetailsDashBoard : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["MP"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bindWardList();
            bindStation();
            fillData();
            BtnClose.Visible = false;
        }
    }
    public void fillData()
    {
        BaseC.WardManagement objW = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        try
        {
            ds = objW.GetWardDashBoard(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(ddlWard.SelectedValue), common.myInt(ddlStation.SelectedValue));

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvWardDashBoardDetails.DataSource = ds.Tables[0];
                        gvWardDashBoardDetails.DataBind();
                    }
                    else
                    {
                        BlankGrid();
                    }
                }
            }
            else
            {
                BlankGrid();
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ds.Dispose();
            objW = null;
        }
    }
    protected void lnkNotification1_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkNotification1 = (LinkButton)sender;
        HiddenField hdnFromDate = (HiddenField)lnkNotification1.FindControl("hdnFromDate");
        hdnMinFromDate.Value = hdnFromDate.Value;
        if (common.myStr(lnkNotification1.Text).Equals("Drug Orders"))
        {
            OpenPopup("Drug Orders");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Rejected Samples"))
        {
            OpenPopup("Rejected Samples");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("UnPerformed Services"))
        {
            OpenPopup("UnPerformed Services");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Blood Requests"))
        {
            OpenPopup("Blood Requests");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Non Drug Orders"))
        {
            OpenPopup("Non Drug Orders");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("OT Clearance Pending"))
        {
            OpenPopup("OT Clearance Pending");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Marked for Discharge"))
        {
            OpenPopup("Marked for Discharge");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Discharge Summary Pending"))
        {
            OpenPopup("Discharge Summary Pending");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("MLC"))
        {
            OpenPopup("MLC");
        }

        else if (common.myStr(lnkNotification1.Text).Equals("Delayed Medicines"))
        {
            OpenPopup("Delayed Medicines");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Pending for Collection"))
        {
            OpenPopup("Pending for Collection");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Pending for Dispatch"))
        {
            OpenPopup("Pending for Dispatch");
        }

        else if (common.myStr(lnkNotification1.Text).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Radiology Pending for Acknowledgement");
        }   

        else if (common.myStr(lnkNotification1.Text).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Sent For Billing"))
        {
            OpenPopup("Sent For Billing");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Pharmacy Clearance"))
        {
            OpenPopup("Pharmacy Clearance");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Bill Prepared"))
        {
            OpenPopup("Bill Prepared");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Discharge Approved"))
        {
            OpenPopup("Discharge Approved");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Bed Released"))
        {
            OpenPopup("Bed Released");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Stop Medication"))
        {
            OpenPopup("Stop Medication");
        }
        else if (common.myStr(lnkNotification1.Text).Equals("Pending for Result Finalized"))
        {
            OpenPopup("Pending for Result Finalized");
        }
    }
    protected void lnkNotification1Count_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkNotification1 = (LinkButton)sender;

        HiddenField hdnFromDate = (HiddenField)lnkNotification1.FindControl("hdnFromDate");
        hdnMinFromDate.Value = hdnFromDate.Value;

        string TitleName = lnkNotification1.CommandArgument.ToString();

        if (common.myStr(TitleName).Equals("Drug Orders"))
        {
            OpenPopup("Drug Orders");
        }
        else if (common.myStr(TitleName).Equals("Rejected Samples"))
        {
            OpenPopup("Rejected Samples");
        }
        else if (common.myStr(TitleName).Equals("UnPerformed Services"))
        {
            OpenPopup("UnPerformed Services");
        }
        else if (common.myStr(TitleName).Equals("Blood Requests"))
        {
            OpenPopup("Blood Requests");
        }
        else if (common.myStr(TitleName).Equals("Non Drug Orders"))
        {
            OpenPopup("Non Drug Orders");
        }
        else if (common.myStr(TitleName).Equals("OT Clearance Pending"))
        {
            OpenPopup("OT Clearance Pending");
        }
        else if (common.myStr(TitleName).Equals("Marked for Discharge"))
        {
            OpenPopup("Marked for Discharge");
        }
        else if (common.myStr(TitleName).Equals("Discharge Summary Pending"))
        {
            OpenPopup("Discharge Summary Pending");
        }
        else if (common.myStr(TitleName).Equals("MLC"))
        {
            OpenPopup("MLC");
        }

        else if (common.myStr(TitleName).Equals("Delayed Medicines"))
        {
            OpenPopup("Delayed Medicines");
        }

        else if (common.myStr(TitleName).Equals("Pending for Collection"))
        {
            OpenPopup("Pending for Collection");
        }
        else if (common.myStr(TitleName).Equals("Pending for Dispatch"))
        {
            OpenPopup("Pending for Dispatch");
        }
        else if (common.myStr(TitleName).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(TitleName).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }

        else if (common.myStr(TitleName).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(TitleName).Equals("Sent For Billing"))
        {
            OpenPopup("Sent For Billing");
        }
        else if (common.myStr(TitleName).Equals("Pharmacy Clearance"))
        {
            OpenPopup("Pharmacy Clearance");
        }
        else if (common.myStr(TitleName).Equals("Bill Prepared"))
        {
            OpenPopup("Bill Prepared");
        }
        else if (common.myStr(TitleName).Equals("Discharge Approved"))
        {
            OpenPopup("Discharge Approved");
        }
        else if (common.myStr(TitleName).Equals("Bed Released"))
        {
            OpenPopup("Bed Released");
        }
        else if (common.myStr(TitleName).Equals("Stop Medication"))
        {
            OpenPopup("Stop Medication");
        }
        else if (common.myStr(TitleName).Equals("Pending for Result Finalized"))
        {
            OpenPopup("Pending for Result Finalized");
        }
    }
    protected void lnkNotification2_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkNotification2 = (LinkButton)sender;
        HiddenField hdnFromDate = (HiddenField)lnkNotification2.FindControl("hdnFromDate");
        hdnMinFromDate.Value = hdnFromDate.Value;
        if (common.myStr(lnkNotification2.Text).Equals("Drug Orders"))
        {
            OpenPopup("Drug Orders");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Rejected Samples"))
        {
            OpenPopup("Rejected Samples");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("UnPerformed Services"))
        {
            OpenPopup("UnPerformed Services");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Blood Requests"))
        {
            OpenPopup("Blood Requests");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Non Drug Orders"))
        {
            OpenPopup("Non Drug Orders");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("OT Clearance Pending"))
        {
            OpenPopup("OT Clearance Pending");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Marked for Discharge"))
        {
            OpenPopup("Marked for Discharge");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Discharge Summary Pending"))
        {
            OpenPopup("Discharge Summary Pending");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("MLC"))
        {
            OpenPopup("MLC");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Delayed Medicines"))
        {
            OpenPopup("Delayed Medicines");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Pending for Collection"))
        {
            OpenPopup("Pending for Collection");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Pending for Dispatch"))
        {
            OpenPopup("Pending for Dispatch");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Sent For Billing"))
        {
            OpenPopup("Sent For Billing");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Pharmacy Clearance"))
        {
            OpenPopup("Pharmacy Clearance");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Bill Prepared"))
        {
            OpenPopup("Bill Prepared");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Discharge Approved"))
        {
            OpenPopup("Discharge Approved");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Bed Released"))
        {
            OpenPopup("Bed Released");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Stop Medication"))
        {
            OpenPopup("Stop Medication");
        }
        else if (common.myStr(lnkNotification2.Text).Equals("Acknowledged"))
        {
            OpenPopup("Radiology Pending for Result Entry");
        }
    }
    protected void lnkNotification2Count_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkNotification2 = (LinkButton)sender;
        HiddenField hdnFromDate = (HiddenField)lnkNotification2.FindControl("hdnFromDate");
        hdnMinFromDate.Value = hdnFromDate.Value;
        string TitleName = lnkNotification2.CommandArgument.ToString();

        if (common.myStr(TitleName).Equals("Drug Orders"))
        {
            OpenPopup("Drug Orders");
        }
        else if (common.myStr(TitleName).Equals("Rejected Samples"))
        {
            OpenPopup("Rejected Samples");
        }
        else if (common.myStr(TitleName).Equals("UnPerformed Services"))
        {
            OpenPopup("UnPerformed Services");
        }
        else if (common.myStr(TitleName).Equals("Blood Requests"))
        {
            OpenPopup("Blood Requests");
        }
        else if (common.myStr(TitleName).Equals("Non Drug Orders"))
        {
            OpenPopup("Non Drug Orders");
        }
        else if (common.myStr(TitleName).Equals("OT Clearance Pending"))
        {
            OpenPopup("OT Clearance Pending");
        }
        else if (common.myStr(TitleName).Equals("Marked for Discharge"))
        {
            OpenPopup("Marked for Discharge");
        }
        else if (common.myStr(TitleName).Equals("Discharge Summary Pending"))
        {
            OpenPopup("Discharge Summary Pending");
        }
        else if (common.myStr(TitleName).Equals("MLC"))
        {
            OpenPopup("MLC");
        }

        else if (common.myStr(TitleName).Equals("Delayed Medicines"))
        {
            OpenPopup("Delayed Medicines");
        }
        else if (common.myStr(TitleName).Equals("Pending for Collection"))
        {
            OpenPopup("Pending for Collection");
        }
        else if (common.myStr(TitleName).Equals("Pending for Dispatch"))
        {
            OpenPopup("Pending for Dispatch");
        }
        else if (common.myStr(TitleName).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(TitleName).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }

        else if (common.myStr(TitleName).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(TitleName).Equals("Sent For Billing"))
        {
            OpenPopup("Sent For Billing");
        }
        else if (common.myStr(TitleName).Equals("Pharmacy Clearance"))
        {
            OpenPopup("Pharmacy Clearance");
        }
        else if (common.myStr(TitleName).Equals("Bill Prepared"))
        {
            OpenPopup("Bill Prepared");
        }
        else if (common.myStr(TitleName).Equals("Discharge Approved"))
        {
            OpenPopup("Discharge Approved");
        }
        else if (common.myStr(TitleName).Equals("Bed Released"))
        {
            OpenPopup("Bed Released");
        }
        else if (common.myStr(TitleName).Equals("Stop Medication"))
        {
            OpenPopup("Stop Medication");
        }
        else if (common.myStr(TitleName).Equals("Acknowledged"))
        {
            OpenPopup("Radiology Pending for Result Entry");
        }
    }
    protected void lnkNotification3_OnClick(object sender, EventArgs e)
    {

        LinkButton lnkNotification3 = (LinkButton)sender;
        HiddenField hdnFromDate = (HiddenField)lnkNotification3.FindControl("hdnFromDate");
        hdnMinFromDate.Value = hdnFromDate.Value;
        if (common.myStr(lnkNotification3.Text).Equals("Drug Orders"))
        {
            OpenPopup("Drug Orders");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Rejected Samples"))
        {
            OpenPopup("Rejected Samples");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("UnPerformed Services"))
        {
            OpenPopup("UnPerformed Services");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Blood Requests"))
        {
            OpenPopup("Blood Requests");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Non Drug Orders"))
        {
            OpenPopup("Non Drug Orders");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("OT Clearance Pending"))
        {
            OpenPopup("OT Clearance Pending");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Marked for Discharge"))
        {
            OpenPopup("Marked for Discharge");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Discharge Summary Pending"))
        {
            OpenPopup("Discharge Summary Pending");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("MLC"))
        {
            OpenPopup("MLC");
        }

        else if (common.myStr(lnkNotification3.Text).Equals("Delayed Medicines"))
        {
            OpenPopup("Delayed Medicines");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Pending for Collection"))
        {
            OpenPopup("Pending for Collection");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Pending for Dispatch"))
        {
            OpenPopup("Pending for Dispatch");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }

        else if (common.myStr(lnkNotification3.Text).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Sent For Billing"))
        {
            OpenPopup("Sent For Billing");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Pharmacy Clearance"))
        {
            OpenPopup("Pharmacy Clearance");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Bill Prepared"))
        {
            OpenPopup("Bill Prepared");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Discharge Approved"))
        {
            OpenPopup("Discharge Approved");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Bed Released"))
        {
            OpenPopup("Bed Released");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Stop Medication"))
        {
            OpenPopup("Stop Medication");
        }
        else if (common.myStr(lnkNotification3.Text).Equals("Pending for Result Finalized"))
        {
            OpenPopup("Radiology Pending for Result Finalized");
        }
    }
    protected void lnkNotification3Count_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkNotification3 = (LinkButton)sender;
        HiddenField hdnFromDate = (HiddenField)lnkNotification3.FindControl("hdnFromDate");
        hdnMinFromDate.Value = hdnFromDate.Value;

        string TitleName = lnkNotification3.CommandArgument.ToString();

        if (common.myStr(TitleName).Equals("Drug Orders"))
        {
            OpenPopup("Drug Orders");
        }
        else if (common.myStr(TitleName).Equals("Rejected Samples"))
        {
            OpenPopup("Rejected Samples");
        }
        else if (common.myStr(TitleName).Equals("UnPerformed Services"))
        {
            OpenPopup("UnPerformed Services");
        }
        else if (common.myStr(TitleName).Equals("Blood Requests"))
        {
            OpenPopup("Blood Requests");
        }
        else if (common.myStr(TitleName).Equals("Non Drug Orders"))
        {
            OpenPopup("Non Drug Orders");
        }
        else if (common.myStr(TitleName).Equals("OT Clearance Pending"))
        {
            OpenPopup("OT Clearance Pending");
        }
        else if (common.myStr(TitleName).Equals("Marked for Discharge"))
        {
            OpenPopup("Marked for Discharge");
        }
        else if (common.myStr(TitleName).Equals("Discharge Summary Pending"))
        {
            OpenPopup("Discharge Summary Pending");
        }
        else if (common.myStr(TitleName).Equals("MLC"))
        {
            OpenPopup("MLC");
        }

        else if (common.myStr(TitleName).Equals("Delayed Medicines"))
        {
            OpenPopup("Delayed Medicines");
        }
        else if (common.myStr(TitleName).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }

        else if (common.myStr(TitleName).Equals("Pending for Collection"))
        {
            OpenPopup("Pending for Collection");
        }
        else if (common.myStr(TitleName).Equals("Pending for Dispatch"))
        {
            OpenPopup("Pending for Dispatch");
        }
        else if (common.myStr(TitleName).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(TitleName).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(TitleName).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(TitleName).Equals("Sent For Billing"))
        {
            OpenPopup("Sent For Billing");
        }
        else if (common.myStr(TitleName).Equals("Pharmacy Clearance"))
        {
            OpenPopup("Pharmacy Clearance");
        }
        else if (common.myStr(TitleName).Equals("Bill Prepared"))
        {
            OpenPopup("Bill Prepared");
        }
        else if (common.myStr(TitleName).Equals("Discharge Approved"))
        {
            OpenPopup("Discharge Approved");
        }
        else if (common.myStr(TitleName).Equals("Bed Released"))
        {
            OpenPopup("Bed Released");
        }
        else if (common.myStr(TitleName).Equals("Stop Medication"))
        {
            OpenPopup("Stop Medication");
        }
        else if (common.myStr(TitleName).Equals("Pending for Result Finalized"))
        {
            OpenPopup("Radiology Pending for Result Finalized");
        }
    }
    protected void lnkNotification4_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkNotification4 = (LinkButton)sender;

        HiddenField hdnFromDate = (HiddenField)lnkNotification4.FindControl("hdnFromDate");
        hdnMinFromDate.Value = hdnFromDate.Value;

        if (common.myStr(lnkNotification4.Text).Equals("Drug Orders"))
        {
            OpenPopup("Drug Orders");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Rejected Samples"))
        {
            OpenPopup("Rejected Samples");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("UnPerformed Services"))
        {
            OpenPopup("UnPerformed Services");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Blood Requests"))
        {
            OpenPopup("Blood Requests");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Non Drug Orders"))
        {
            OpenPopup("Non Drug Orders");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("OT Clearance Pending"))
        {
            OpenPopup("OT Clearance Pending");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Marked for Discharge"))
        {
            OpenPopup("Marked for Discharge");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Discharge Summary Pending"))
        {
            OpenPopup("Discharge Summary Pending");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("MLC"))
        {
            OpenPopup("MLC");
        }

        else if (common.myStr(lnkNotification4.Text).Equals("Delayed Medicines"))
        {
            OpenPopup("Delayed Medicines");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Pending for Collection"))
        {
            OpenPopup("Pending for Collection");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Pending for Dispatch"))
        {
            OpenPopup("Pending for Dispatch");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Sent For Billing"))
        {
            OpenPopup("Sent For Billing");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Pharmacy Clearance"))
        {
            OpenPopup("Pharmacy Clearance");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Bill Prepared"))
        {
            OpenPopup("Bill Prepared");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Discharge Approved"))
        {
            OpenPopup("Discharge Approved");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Bed Released"))
        {
            OpenPopup("Bed Released");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Stop Medication"))
        {
            OpenPopup("Stop Medication");
        }
        else if (common.myStr(lnkNotification4.Text).Equals("Care Plan"))
        {
            OpenPopup("Care Plan");
        }
    }
    protected void lnkNotification4Count_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkNotification4 = (LinkButton)sender;
        HiddenField hdnFromDate = (HiddenField)lnkNotification4.FindControl("hdnFromDate");
        hdnMinFromDate.Value = hdnFromDate.Value;
        string TitleName = lnkNotification4.CommandArgument.ToString();

        if (common.myStr(TitleName).Equals("Drug Orders"))
        {
            OpenPopup("Drug Orders");
        }
        else if (common.myStr(TitleName).Equals("Rejected Samples"))
        {
            OpenPopup("Rejected Samples");
        }
        else if (common.myStr(TitleName).Equals("UnPerformed Services"))
        {
            OpenPopup("UnPerformed Services");
        }
        else if (common.myStr(TitleName).Equals("Blood Requests"))
        {
            OpenPopup("Blood Requests");
        }
        else if (common.myStr(TitleName).Equals("Non Drug Orders"))
        {
            OpenPopup("Non Drug Orders");
        }

        else if (common.myStr(TitleName).Equals("OT Clearance Pending"))
        {
            OpenPopup("OT Clearance Pending");
        }
        else if (common.myStr(TitleName).Equals("Marked for Discharge"))
        {
            OpenPopup("Marked for Discharge");
        }
        else if (common.myStr(TitleName).Equals("Discharge Summary Pending"))
        {
            OpenPopup("Discharge Summary Pending");
        }
        else if (common.myStr(TitleName).Equals("MLC"))
        {
            OpenPopup("MLC");
        }

        else if (common.myStr(TitleName).Equals("Delayed Medicines"))
        {
            OpenPopup("Delayed Medicines");
        }
        else if (common.myStr(TitleName).Equals("Pending for Collection"))
        {
            OpenPopup("Pending for Collection");
        }
        else if (common.myStr(TitleName).Equals("Pending for Dispatch"))
        {
            OpenPopup("Pending for Dispatch");
        }
        else if (common.myStr(TitleName).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(TitleName).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }

        else if (common.myStr(TitleName).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(TitleName).Equals("Sent For Billing"))
        {
            OpenPopup("Sent For Billing");
        }
        else if (common.myStr(TitleName).Equals("Pharmacy Clearance"))
        {
            OpenPopup("Pharmacy Clearance");
        }
        else if (common.myStr(TitleName).Equals("Bill Prepared"))
        {
            OpenPopup("Bill Prepared");
        }
        else if (common.myStr(TitleName).Equals("Discharge Approved"))
        {
            OpenPopup("Discharge Approved");
        }
        else if (common.myStr(TitleName).Equals("Bed Released"))
        {
            OpenPopup("Bed Released");
        }
        else if (common.myStr(TitleName).Equals("Stop Medication"))
        {
            OpenPopup("Stop Medication");
        }
        else if (common.myStr(TitleName).Equals("Care Plan"))
        {
            OpenPopup("Care Plan");
        }
    }
    private void OpenPopup(string NotificationText)
    {

        if (NotificationText.Equals("Drug Orders"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=DrugOrder&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Rejected Samples"))
        {
            RadWindow1.NavigateUrl = "~/LIS/Phlebotomy/RejectedSampleDetails.aspx?SOURCE=" + common.myStr("IPD") +
                                       "&FDate=" + common.myDate(hdnMinFromDate.Value).ToString("yyyy-MM-dd") +//; common.myDate(DateTime.Today).ToString("yyyy-MM-dd") +
                                       "&TDate=" + common.myDate(DateTime.Today).ToString("yyyy-MM-dd") + "&CallFrom=0&WardId=" + common.myInt(ddlWard.SelectedValue);

            RadWindow1.Height = 600;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }

        else if (NotificationText.Equals("UnPerformed Services"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=Unperformed&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1125;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }

        else if (NotificationText.Equals("Blood Requests"))
        {
            RadWindow1.NavigateUrl = "/BloodBank/SetupMaster/ComponentRequisitionList.aspx?MP=NO&Regid=" + common.myInt(0)
                                    + "&RegNo=" + common.myStr("")
                                    + "&EncId=" + common.myInt(0)
                                    + "&EncNo=" + common.myStr("")
                                    + "&AckStatus=Ack"
                                    + "&Ptype=I&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 550;
            RadWindow1.Width = 1000;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            // RadWindow1.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        }

        else if (NotificationText.Equals("Non Drug Orders"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=NonDrugOrder&FromDate=" + common.myDate(hdnMinFromDate.Value).ToString("MM/dd/yyyy") + "&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }

        else if (NotificationText.Equals("OT Clearance Pending"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/OTBookingDeatils.aspx?PT=BC&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Marked for Discharge"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/PendingBillingStatus.aspx?Status=MD&WardId=" + common.myInt(ddlWard.SelectedValue) + "&TitleName=Marked for Discharge";
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }

        else if (NotificationText.Equals("Sent For Billing"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/PendingBillingStatus.aspx?Status=SB&WardId=" + common.myInt(ddlWard.SelectedValue) + "&TitleName=Sent For Billing";
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }

        else if (NotificationText.Equals("Pharmacy Clearance"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/PendingBillingStatus.aspx?Status=PC&WardId=" + common.myInt(ddlWard.SelectedValue) + "&TitleName=Pharmacy Clearance";
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }

        else if (NotificationText.Equals("Bill Prepared"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/PendingBillingStatus.aspx?Status=BP&WardId=" + common.myInt(ddlWard.SelectedValue) + "&TitleName=Bill Prepared";
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Discharge Approved"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/PendingBillingStatus.aspx?Status=DA&WardId=" + common.myInt(ddlWard.SelectedValue) + "&TitleName=Discharge Approved";
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Bed Released"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/PendingBillingStatus.aspx?Status=BR&WardId=" + common.myInt(ddlWard.SelectedValue) + "&TitleName=Bed Released";
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Discharge Summary Pending"))
        {

            RadWindow1.NavigateUrl = "/WardManagement/MLCPopup.aspx?PageName=DischargeSummaryPending&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("MLC"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/MLCPopup.aspx?PageName=MLC&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Delayed Medicines"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/DelayedMedicines.aspx?WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Pending Templates"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/PendingTemplate.aspx?WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }

        else if (NotificationText.Equals("Pending for Collection"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=G&StatusCode=SNC&FromDate=" + common.myDate(hdnMinFromDate.Value).ToString("MM/dd/yyyy") +"&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1125;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Pending for Dispatch"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=G&StatusCode=SC&FromDate=" + common.myDate(hdnMinFromDate.Value).ToString("MM/dd/yyyy")+ "&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1125;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Pending for Acknowledgement"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=X&StatusCode=SD&FromDate=" + common.myDate(hdnMinFromDate.Value).ToString("MM/dd/yyyy")+ "&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1125;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }

        else if (NotificationText.Equals("Radiology Pending for Acknowledgement"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=X&StatusCode=SD&FromDate=" + common.myDate(hdnMinFromDate.Value).ToString("MM/dd/yyyy")+ "&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1125;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Radiology Pending for Result Entry"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=X&StatusCode=DA&FromDate=" + common.myDate(hdnMinFromDate.Value).ToString("MM/dd/yyyy") + "&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1125;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Pending for Result Entry"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=G&StatusCode=DA&FromDate=" + common.myDate(hdnMinFromDate.Value).ToString("MM/dd/yyyy") + "&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1125;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Radiology Pending for Result Finalized"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=X&StatusCode=RE&FromDate=" + common.myDate(hdnMinFromDate.Value).ToString("MM/dd/yyyy") + "&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1125;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Pending for Result Finalized"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=G&StatusCode=RE&FromDate=" + common.myDate(hdnMinFromDate.Value).ToString("MM/dd/yyyy") + "&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1125;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Stop Medication"))
        {
            RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=StopMedication&WardId=" + common.myInt(ddlWard.SelectedValue);
            RadWindow1.Height = 650;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else if (NotificationText.Equals("Care Plan"))
        {
            RadWindow1.NavigateUrl = "~/EMR/ClinicalPathway/PatientTreatmentPlan.aspx?From=POPUP";
            RadWindow1.Height = 300;
            RadWindow1.Width = 500;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            //RadWindow1.OnClientClose = "OnClearClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
    }
    private void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Notification1");
        dt.Columns.Add("Notification1Count");
        dt.Columns.Add("Notification2");
        dt.Columns.Add("Notification2Count");

        DataRow dr = null;
        dr = dt.NewRow();
        //dr["id"] = "1";
        dt.Rows.Add(dr);

        gvWardDashBoardDetails.DataSource = dt;
        gvWardDashBoardDetails.DataBind();
    }
    protected void gvWardDashBoardDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvWardDashBoardDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            //e.Row.BackColor = System.Drawing.Color.White;
            //e.Row.BorderColor = System.Drawing.Color.LightGray;

            LinkButton lnkNotification1 = (LinkButton)e.Item.FindControl("lnkNotification1");
            LinkButton lnkNotification1Count = (LinkButton)e.Item.FindControl("lnkNotification1Count");

            LinkButton lnkNotification2 = (LinkButton)e.Item.FindControl("lnkNotification2");
            LinkButton lnkNotification2Count = (LinkButton)e.Item.FindControl("lnkNotification2Count");
            LinkButton lnkNotification3 = (LinkButton)e.Item.FindControl("lnkNotification3");
            LinkButton lnkNotification3Count = (LinkButton)e.Item.FindControl("lnkNotification3Count");
            LinkButton lnkNotification4 = (LinkButton)e.Item.FindControl("lnkNotification4");
            LinkButton lnkNotification4Count = (LinkButton)e.Item.FindControl("lnkNotification4Count");

            LinkButton lnkNotification5 = (LinkButton)e.Item.FindControl("lnkNotification5");
            LinkButton lnkNotification5Count = (LinkButton)e.Item.FindControl("lnkNotification5Count");

            HtmlGenericControl divlnkNotification1 = (HtmlGenericControl)e.Item.FindControl("divlnkNotification1");
            HtmlGenericControl divlnkNotification2 = (HtmlGenericControl)e.Item.FindControl("divlnkNotification2");
            HtmlGenericControl divlnkNotification3 = (HtmlGenericControl)e.Item.FindControl("divlnkNotification3");
            HtmlGenericControl divlnkNotification4 = (HtmlGenericControl)e.Item.FindControl("divlnkNotification4");
            HtmlGenericControl divlnkNotification5 = (HtmlGenericControl)e.Item.FindControl("divlnkNotification5");


            if (common.myStr(lnkNotification1.Text).Equals(string.Empty))
            {

                divlnkNotification1.Visible = false;
            }
            if (common.myStr(lnkNotification1Count.Text).Equals(string.Empty))
            {
                divlnkNotification1.Visible = false;
            }

            if (common.myStr(lnkNotification2.Text).Equals(string.Empty))
            {

                divlnkNotification2.Visible = false;
            }
            if (common.myStr(lnkNotification2Count.Text).Equals(string.Empty))
            {
                divlnkNotification2.Visible = false;
            }

            if (common.myStr(lnkNotification3.Text).Equals(string.Empty))
            {

                divlnkNotification3.Visible = false;
            }
            if (common.myStr(lnkNotification3Count.Text).Equals(string.Empty))
            {
                divlnkNotification3.Visible = false;
            }

            if (common.myStr(lnkNotification4.Text).Equals(string.Empty))
            {

                divlnkNotification4.Visible = false;
            }
            if (common.myStr(lnkNotification4Count.Text).Equals(string.Empty))
            {
                divlnkNotification4.Visible = false;
            }
            if (common.myStr(lnkNotification5Count.Text).Equals(string.Empty))
            {
                divlnkNotification5.Visible = false;
            }
        }
    }
    protected void lnkNotification5_Click(object sender, EventArgs e)
    {
        LinkButton lnkNotification5 = (LinkButton)sender;

        HiddenField hdnFromDate = (HiddenField)lnkNotification5.FindControl("hdnFromDate");
        hdnMinFromDate.Value = hdnFromDate.Value;

        if (common.myStr(lnkNotification5.Text).Equals("Drug Orders"))
        {
            OpenPopup("Drug Orders");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Rejected Samples"))
        {
            OpenPopup("Rejected Samples");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("UnPerformed Services"))
        {
            OpenPopup("UnPerformed Services");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Blood Requests"))
        {
            OpenPopup("Blood Requests");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Non Drug Orders"))
        {
            OpenPopup("Non Drug Orders");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("OT Clearance Pending"))
        {
            OpenPopup("OT Clearance Pending");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Marked for Discharge"))
        {
            OpenPopup("Marked for Discharge");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Discharge Summary Pending"))
        {
            OpenPopup("Discharge Summary Pending");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("MLC"))
        {
            OpenPopup("MLC");
        }

        else if (common.myStr(lnkNotification5.Text).Equals("Delayed Medicines"))
        {
            OpenPopup("Delayed Medicines");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Pending for Collection"))
        {
            OpenPopup("Pending for Collection");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Pending for Dispatch"))
        {
            OpenPopup("Pending for Dispatch");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Sent For Billing"))
        {
            OpenPopup("Sent For Billing");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Pharmacy Clearance"))
        {
            OpenPopup("Pharmacy Clearance");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Bill Prepared"))
        {
            OpenPopup("Bill Prepared");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Discharge Approved"))
        {
            OpenPopup("Discharge Approved");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Bed Released"))
        {
            OpenPopup("Bed Released");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Stop Medication"))
        {
            OpenPopup("Stop Medication");
        }
        else if (common.myStr(lnkNotification5.Text).Equals("Acknowledged"))
        {
            OpenPopup("Pending for Result Entry");
        }
    }
    protected void lnkNotification5Count_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkNotification5 = (LinkButton)sender;
        HiddenField hdnFromDate = (HiddenField)lnkNotification5.FindControl("hdnFromDate");
        hdnMinFromDate.Value = hdnFromDate.Value;
        string TitleName = lnkNotification5.CommandArgument.ToString();

        if (common.myStr(TitleName).Equals("Drug Orders"))
        {
            OpenPopup("Drug Orders");
        }
        else if (common.myStr(TitleName).Equals("Rejected Samples"))
        {
            OpenPopup("Rejected Samples");
        }
        else if (common.myStr(TitleName).Equals("UnPerformed Services"))
        {
            OpenPopup("UnPerformed Services");
        }
        else if (common.myStr(TitleName).Equals("Blood Requests"))
        {
            OpenPopup("Blood Requests");
        }
        else if (common.myStr(TitleName).Equals("Non Drug Orders"))
        {
            OpenPopup("Non Drug Orders");
        }

        else if (common.myStr(TitleName).Equals("OT Clearance Pending"))
        {
            OpenPopup("OT Clearance Pending");
        }
        else if (common.myStr(TitleName).Equals("Marked for Discharge"))
        {
            OpenPopup("Marked for Discharge");
        }
        else if (common.myStr(TitleName).Equals("Discharge Summary Pending"))
        {
            OpenPopup("Discharge Summary Pending");
        }
        else if (common.myStr(TitleName).Equals("MLC"))
        {
            OpenPopup("MLC");
        }

        else if (common.myStr(TitleName).Equals("Delayed Medicines"))
        {
            OpenPopup("Delayed Medicines");
        }
        else if (common.myStr(TitleName).Equals("Pending for Collection"))
        {
            OpenPopup("Pending for Collection");
        }
        else if (common.myStr(TitleName).Equals("Pending for Dispatch"))
        {
            OpenPopup("Pending for Dispatch");
        }
        else if (common.myStr(TitleName).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }
        else if (common.myStr(TitleName).Equals("Pending for Acknowledgement"))
        {
            OpenPopup("Pending for Acknowledgement");
        }

        else if (common.myStr(TitleName).Equals("Pending Templates"))
        {
            OpenPopup("Pending Templates");
        }
        else if (common.myStr(TitleName).Equals("Sent For Billing"))
        {
            OpenPopup("Sent For Billing");
        }
        else if (common.myStr(TitleName).Equals("Pharmacy Clearance"))
        {
            OpenPopup("Pharmacy Clearance");
        }
        else if (common.myStr(TitleName).Equals("Bill Prepared"))
        {
            OpenPopup("Bill Prepared");
        }
        else if (common.myStr(TitleName).Equals("Discharge Approved"))
        {
            OpenPopup("Discharge Approved");
        }
        else if (common.myStr(TitleName).Equals("Bed Released"))
        {
            OpenPopup("Bed Released");
        }
        else if (common.myStr(TitleName).Equals("Stop Medication"))
        {
            OpenPopup("Stop Medication");
        }
        else if (common.myStr(TitleName).Equals("Acknowledged"))
        {
            OpenPopup("Pending for Result Entry");
        }
    }
    private void bindWardList()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement objWD = new BaseC.WardManagement();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
            ds = objWD.GetWardTagging(common.myInt(Session["HospitalLocationID"]), true, common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                ds = objadt.GetWard(common.myInt(Session["FacilityId"]));
            }

            ds = objadt.GetWard(common.myInt(Session["FacilityId"]));
            ddlWard.DataSource = ds.Tables[0];
            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardId";
            ddlWard.DataBind();

            ddlWard.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlWard.SelectedIndex = 0;
        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            objadt = null;
            objWD = null;
            ds.Dispose();
        }
    }
    protected void ddlWard_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fillData();
        }
        catch
        {
        }
    }

    protected void ddlStation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
           
            fillData();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    private void bindStation()
    {
        DataSet ds = new DataSet();
        //BaseC.WardManagement objWD = new BaseC.WardManagement();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {

            ds = objadt.GetWardStation(common.myInt(Session["FacilityId"]), common.myInt(Session["EmployeeId"]));

            ddlStation.DataSource = ds.Tables[0];
            ddlStation.DataTextField = "StationName";
            ddlStation.DataValueField = "ID";
            ddlStation.DataBind();
            ddlStation.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlStation.SelectedIndex = 0;

        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            // objWD = null;
            objadt = null;
            ds.Dispose();
        }
    }
}