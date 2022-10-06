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

public partial class ATD_BedClearance : System.Web.UI.Page
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
        DataView dvClearanceF = ((DataSet)ds).Tables[0].DefaultView;
        dvClearanceF.RowFilter = "";
        dvClearanceF.RowFilter = "IsICUCase = 1 AND IsICUClearanceDone = 0";
        if (dvClearanceF.ToTable().Rows.Count > 0)
        {
            gvBedTransferRequest.DataSource = dvClearanceF.ToTable();
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
        HiddenField hdnRegistrationId = (HiddenField)gvr.FindControl("hdnRegistrationId");
        HiddenField hdnEncounterId = (HiddenField)gvr.FindControl("hdnEncounterId");
        HiddenField hdnTransferRequistionId = (HiddenField)gvr.FindControl("hdnTransferRequistionId");
        Clear();
        ViewState["RegistrationId"] = hdnRegistrationId.Value;
        ViewState["EncounterId"] = hdnEncounterId.Value;
        ViewState["TransferRequistionId"] = hdnTransferRequistionId.Value;
        hdnRegistrationId1.Value = common.myStr(ViewState["RegistrationId"]);
        hdnEncounterId1.Value = common.myStr(ViewState["EncounterId"]);
        hdnTransferRequistionId1.Value = common.myStr(ViewState["TransferRequistionId"]);

    }

    public void Clear()
    {
        ViewState["RegistrationId"] = "";
        ViewState["EncounterId"] = "";
        ViewState["TransferRequistionId"] = "";
        hdnRegistrationId1.Value = "";
        hdnEncounterId1.Value = "";
        hdnTransferRequistionId1.Value = "";
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
        dt.Columns.Add("IsICUClearanceRequiredForBedTransfer");
        dt.Columns.Add("IsICUCase");
        dt.Columns.Add("IsICUClearanceDone");
        dt.Columns.Add("ICUClearanceDate");
        dt.Columns.Add("ICUClearanceGivenBy");

        DataRow dr = null;
        dr = dt.NewRow();
        dr["id"] = "1";
        dt.Rows.Add(dr);

        gvBedTransferRequest.DataSource = dt;
        gvBedTransferRequest.DataBind();
        gvBedTransferRequest.Columns[0].Visible = false;

    }

    protected void btnClearance_Click(object sender, EventArgs e)
    {
        if (common.myInt(ViewState["RegistrationId"]) > 0 && common.myInt(ViewState["EncounterId"]) > 0 && common.myInt(ViewState["TransferRequistionId"]) > 0)
        {
            objATD = new BaseC.ATD(sConString);
            string str = "";
            str = objATD.SaveBedTansferClearance(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                           common.myInt(ViewState["TransferRequistionId"]), common.myInt(ViewState["RegistrationId"]),
                           common.myInt(ViewState["EncounterId"]), common.myInt(Session["UserID"]));

            fillData();
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = str;
            //Alert.ShowAjaxMsg(str, Page);
        }
    }
}