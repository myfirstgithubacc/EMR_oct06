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

public partial class ATD_BedTransferRequest : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    DataSet ds;
    BaseC.ATD objATD;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            fillData();
            BindBillingCategory();
        }
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["MP"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    public void fillData()
    {
        objATD = new BaseC.ATD(sConString);
        ds = new DataSet();
        ds = objATD.GetBedTransferRequest(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBedTransferRequest.DataSource = ds;
            gvBedTransferRequest.DataBind();
        }
        else
        {
            BlankGrid();
        }
        Clear();
    }
    protected void lnkSelect_OnClick(object sender, EventArgs e)
    {
        foreach (GridViewRow gv in gvBedTransferRequest.Rows)
        {
            gv.BackColor = Color.White;
        }
        LinkButton lnkSelect = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)lnkSelect.NamingContainer;
        gvr.BackColor = Color.Wheat;
        HiddenField hdnToward = (HiddenField)gvr.FindControl("hdnToward");
        HiddenField hdnToBedCategory = (HiddenField)gvr.FindControl("hdnToBedCategory");
        HiddenField hdnToBedNo = (HiddenField)gvr.FindControl("hdnToBedNo");

        HiddenField hdnTowardId = (HiddenField)gvr.FindControl("hdnTowardId");
        HiddenField hdnToBedCategoryId = (HiddenField)gvr.FindControl("hdnToBedCategoryId");
        HiddenField hdnToBedNoId = (HiddenField)gvr.FindControl("hdnToBedNoId");
        HiddenField hdnFromBillingCategoryId = (HiddenField)gvr.FindControl("hdnFromBillingCategoryId");
        HiddenField hdnFromBedCategoryId = (HiddenField)gvr.FindControl("hdnFromBedCategoryId");
        HiddenField hdnFromBedId = (HiddenField)gvr.FindControl("hdnFromBedId");
        HiddenField hdnFromWardId = (HiddenField)gvr.FindControl("hdnFromWardId");
        HiddenField hdnRegistrationId = (HiddenField)gvr.FindControl("hdnRegistrationId");
        HiddenField hdnEncounterId = (HiddenField)gvr.FindControl("hdnEncounterId");
        HiddenField hdnTransferRequistionId = (HiddenField)gvr.FindControl("hdnTransferRequistionId");
        HiddenField hdnToBillingCategoryId = (HiddenField)gvr.FindControl("hdnToBillingCategoryId");
        HiddenField hdnrequestDate = (HiddenField)gvr.FindControl("hdnrequestDate");
        HiddenField hdnCode = (HiddenField)gvr.FindControl("hdnCode");
        HiddenField hdnTemporaryBed = (HiddenField)gvr.FindControl("hdnTemporaryBed");        
        dtpTransferDate.SelectedDate = Convert.ToDateTime(hdnrequestDate.Value);
        dtpActualTransferDate.MinDate = Convert.ToDateTime(hdnrequestDate.Value);
        dtpActualTransferDate.MaxDate = DateTime.Now.AddMinutes(10);
        dtpActualTransferDate.SelectedDate = DateTime.Now;
        ViewState["hdnrequestDate"] = hdnrequestDate.Value;
        ViewState["FromBillingCategoryId"] = hdnFromBillingCategoryId.Value;
        ViewState["FromBedCategoryId"] = hdnFromBedCategoryId.Value;
        ViewState["FromBedId"] = hdnFromBedId.Value;
        ViewState["hdnFromWardId"] = hdnFromWardId.Value;
        ViewState["RegistrationId"] = hdnRegistrationId.Value;
        ViewState["EncounterId"] = hdnEncounterId.Value;
        ViewState["TowardId"] = hdnTowardId.Value;
        ViewState["TransferRequistionId"] = hdnTransferRequistionId.Value;
        ViewState["Code"] = hdnCode.Value;
        fillEmptyBed(common.myInt(hdnToBedCategoryId.Value), common.myInt(hdnTowardId.Value));
        ViewState["FromWardId"] = hdnTowardId.Value;
        lbltowardname.Text = hdnToward.Value;
        ddlBillingCat.SelectedIndex = ddlBillingCat.Items.IndexOf(ddlBillingCat.Items.FindByValue(hdnToBillingCategoryId.Value));
        ddlbedcategory.Enabled = false;
        ddlbedcategory.SelectedIndex = ddlbedcategory.Items.IndexOf(ddlbedcategory.Items.FindByValue(hdnToBedCategoryId.Value));
        //ddlBillingCat.SelectedValue = hdnToBillingCategoryId.Value;
        //ddlbedcategory.SelectedValue = hdnToBedCategoryId.Value;

        if ((hdnCode.Value == "V") || (hdnCode.Value == "I"))
        {
            ddlbedcategory.Enabled = false;
            //ddlBillingCat.Enabled = false;
            //Done By Ujjwal 02April2015 Commented ddlVacantBed.Enabled = false; to allow changing the bed start
            //ddlVacantBed.Enabled = false;
            //Done By Ujjwal 02April2015 Commented ddlVacantBed.Enabled = false; to allow changing the bed end
            //ddlWard.Enabled = true;
            ddlVacantBed.SelectedValue = hdnToBedNoId.Value;
            ViewState["BedChanged"] = "N";

        }
        else
        {

            ddlbedcategory.Enabled = true;
           /// ddlBillingCat.Enabled = true;
            ddlVacantBed.Enabled = true;
            //ddlWard.Enabled = true;
            ViewState["BedChanged"] = "Y";
            Alert.ShowAjaxMsg("Requested bed is occupied.Please select any other bed!!", Page);
        }
        if (hdnCode.Value == "V" && (hdnTemporaryBed.Value.ToUpper().Equals("TRUE") || hdnTemporaryBed.Value.ToUpper().Equals("1")))
        {
            ddlbedcategory.Enabled = true;
            ddlVacantBed.Enabled = true;
        }


    }
    public void fillEmptyBed(int bedCategoryId, int wardId)
    {
        //DataSet objds = new DataSet();
        //objATD = new BaseC.ATD(sConString);
        //ds = new DataSet();
        //ds = objATD.getVacantBed(bedCategoryId, wardId);
        //ddlVacantBed.DataSource = ds;
        //ddlVacantBed.DataTextField = "BedNo";
        //ddlVacantBed.DataValueField = "BedId";
        //ddlVacantBed.DataBind();
        DataSet objds = new DataSet();
        BaseC.ATD objATD = new BaseC.ATD(sConString);
        objds = new DataSet();
        objds = objATD.GetBedDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
        if ((common.myStr(ViewState["Code"]) == "V") || (common.myStr(ViewState["Code"]) == "I"))
        {
            objds.Tables[0].DefaultView.RowFilter = "Status IN('V','I') AND BedCategoryId =" + bedCategoryId + " AND WardId=" + wardId + "";
        }
        else
        {
            objds.Tables[0].DefaultView.RowFilter = "Status IN('V') AND BedCategoryId =" + bedCategoryId + " AND WardId=" + wardId + "";
        }
        ddlVacantBed.DataSource = objds.Tables[0].DefaultView;
        ddlVacantBed.DataTextField = "BedNo";
        ddlVacantBed.DataValueField = "Id";
        ddlVacantBed.DataBind();


    }
    public void btnTransfer_OnClick(object sender, EventArgs e)
    {
        if (ddlbedcategory.SelectedIndex != -1 && ddlBillingCat.SelectedIndex != -1 && ddlVacantBed.SelectedIndex != -1)
        {
            if (dtpActualTransferDate.SelectedDate == null)
            {
                dtpActualTransferDate.MaxDate = DateTime.Now.AddMinutes(10);
                Alert.ShowAjaxMsg("Actual Bed Transfer Date is not correct, Please Re-Enter...", this);
                return;
            }
            
            if (dtpActualTransferDate.SelectedDate > DateTime.Now)
            {
                Alert.ShowAjaxMsg("Actual Bed Transfer Date Should not Greater Than Current Date", this);
                return;
            }
            objATD = new BaseC.ATD(sConString);
            string str = objATD.SaveBedTansfer(
                 common.myInt(Session["HospitalLocationID"]),
                 common.myInt(ViewState["RegistrationId"]),
                 common.myInt(ViewState["FromBillingCategoryId"]),
                 common.myInt(ViewState["FromBedCategoryId"]),
                 common.myInt(ViewState["FromBedId"]),
                 common.myInt(ViewState["hdnFromWardId"]),
                 common.myInt(ddlbedcategory.SelectedValue),
                 common.myInt(ddlBillingCat.SelectedValue),
                 common.myInt(ddlVacantBed.SelectedValue),
                 common.myInt(ViewState["TowardId"]),
                 common.myInt(Session["UserID"]),
                 common.myInt(ViewState["TransferRequistionId"]),
                 Convert.ToDateTime(dtpTransferDate.SelectedDate.Value), 
                 common.myInt(Session["FacilityId"]),
                 Convert.ToDateTime(dtpActualTransferDate.SelectedDate.Value));
            if(!str.Equals("Selected bed is Temporary, Please change Bed No."))
                fillData();           
            Alert.ShowAjaxMsg(str, Page);
        }
        else
        {
            Alert.ShowAjaxMsg("Please select to bed details", Page);
        }
    }
    protected void BindBillingCategory()
    {
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        DataSet ds = new DataSet();
        ddlbedcategory.Items.Clear();

        ds = objadt.GetBedCategory(common.myInt(Session["FacilityId"]));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ListItem ls = new ListItem();
            ls.Text = "";
            ls.Value = "0";
            ls.Selected = true;
            ddlbedcategory.AppendDataBoundItems = true;
            ddlbedcategory.Items.Add(ls);
            ddlbedcategory.DataSource = ds.Tables[0];
            ddlbedcategory.DataTextField = "BedCategoryName";
            ddlbedcategory.DataValueField = "BedCategoryId";
            ddlbedcategory.DataBind();

            
        }
        ds = objadt.GetBillingCategory("",common.myInt(Session["FacilityId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ListItem ls = new ListItem();
            ls.Text = "";
            ls.Value = "0";
            ls.Selected = true;
            ddlBillingCat.AppendDataBoundItems = true;
            ddlBillingCat.Items.Add(ls);
            ddlBillingCat.DataSource = ds.Tables[0];
            ddlBillingCat.DataTextField = "BedCategoryName";
            ddlBillingCat.DataValueField = "BedCategoryId";
            ddlBillingCat.DataBind();
        }

    }
    public void BtnClear_OnClick(object sender, EventArgs e)
    {
        Clear();
    }
    public void Clear()
    {
        ddlbedcategory.SelectedIndex = -1;
        ddlBillingCat.SelectedIndex = -1;
        ddlVacantBed.SelectedIndex = -1;
        lbltowardname.Text = "";
        ViewState["FromBillingCategoryId"] = "";
        ViewState["FromBedCategoryId"] = "";
        ViewState["FromBedId"] = "";
        ViewState["hdnFromWardId"] = "";
        ViewState["RegistrationId"] = "";
        ViewState["EncounterId"] = "";
        ViewState["TowardId"] = "";
        ViewState["TransferRequistionId"] = "";
        ddlVacantBed.Items.Clear();
    }

    protected void BtnCancelRequest_Click(object sender, EventArgs e)
    {
        if (common.myInt(ViewState["TransferRequistionId"]) != 0)
        {
            if (common.myStr(txtRemarks.Text) == "")
            {
                Alert.ShowAjaxMsg("Please enter cancellation remarks !", this.Page);
                txtRemarks.Focus();
                return;
            }
            objATD = new BaseC.ATD(sConString);
            int str = objATD.CancelBedTransferRequesrt(common.myInt(ViewState["TransferRequistionId"]), common.myStr(txtRemarks.Text));
            fillData();
            Alert.ShowAjaxMsg("Request cancelled successfully", Page);
        }
        else
        {
            Alert.ShowAjaxMsg("Please select the transfer request ", Page);
        }

    }
    private void BlankGrid()
    {

        DataTable dt = new DataTable();

        dt.Columns.Add("ToBillingCategory");
        dt.Columns.Add("ToBedCategory");
        dt.Columns.Add("ToBedNo");
        dt.Columns.Add("ToWardId");
        dt.Columns.Add("ToBedCategoryId");
        dt.Columns.Add("ToBedId");
        dt.Columns.Add("FromBillingCategory");
        dt.Columns.Add("FromBillingCategoryId");
        dt.Columns.Add("ToBillingCategoryId");
        dt.Columns.Add("FromBedCategoryId");
        dt.Columns.Add("FromBedId");
        dt.Columns.Add("RegistrationId");
        dt.Columns.Add("EncounterId");
        dt.Columns.Add("id");
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("EncounterNo");
        dt.Columns.Add("PatientName");
        dt.Columns.Add("AgeGender");
        dt.Columns.Add("requestDate");
        dt.Columns.Add("FromBedCategory");
        dt.Columns.Add("Fromward");
        dt.Columns.Add("FromWardId");
        dt.Columns.Add("Toward");
        dt.Columns.Add("FromBedNo");
        dt.Columns.Add("Code");
        dt.Columns.Add("Bedstatus");
        dt.Columns.Add("RequestRemarks");
        dt.Columns.Add("RequestDt");
        dt.Columns.Add("TemporaryBed");

        DataRow dr = null;
        dr = dt.NewRow();
        dr["id"] = "1";
        dt.Rows.Add(dr);

        gvBedTransferRequest.DataSource = dt;
        gvBedTransferRequest.DataBind();
        gvBedTransferRequest.Columns[0].Visible = false;



    }
}
