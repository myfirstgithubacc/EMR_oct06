using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using System.Configuration;

public partial class LIS_Phlebotomy_LabRequestDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLabRequest objLabRequest;
    BaseC.clsLISPhlebotomy objval;
    StringBuilder strXML = new StringBuilder();
    ArrayList coll = new ArrayList();
    string Flag = "";
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            bindMainData();
        }
    }

    private void bindMainData()
    {
        try
        {
            objLabRequest = new BaseC.clsLabRequest(sConString);
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objLabRequest.DiagOPPPackageDetais(common.myInt(Session["FacilityID"]),
                             common.myInt(Request.QueryString["RegId"]), common.myInt(Request.QueryString["EncId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvDetails.DataSource = ds.Tables[0].Copy();
                    gvDetails.DataBind();
                }
                else
                {
                    gvDetails.MasterTableView.VirtualItemCount = 0;
                    BindBlankgrid();
                }
            }
            else
            {
                gvDetails.MasterTableView.VirtualItemCount = 0;
                BindBlankgrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvDetails_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvDetails.CurrentPageIndex = e.NewPageIndex;
        bindMainData();
    }
    protected void gvDetails_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                Label LBL = new Label();
                int LabNo = 0;
                LBL = (Label)e.Item.FindControl("lblLabNo");
                HiddenField hdnDiagSampleId = (HiddenField)e.Item.FindControl("hdnDiagSampleId");
                if (LBL.Text != "")
                {
                    LabNo = common.myInt(LBL.Text);
                    objLabRequest = new BaseC.clsLabRequest(sConString);
                    string strRet = objLabRequest.CancelLabRequestService(common.myInt(hdnDiagSampleId.Value), common.myInt(Session["UserId"]));
                    if (strRet == "0")
                    {
                        bindMainData();
                    }
                }
                else
                {
                    lblMessage.Text = "Error: Lab No does not exist !";
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
    private void BindBlankgrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RegistrationId");
            dt.Columns.Add("EncounterId");
            dt.Columns.Add("LabNo");
            dt.Columns.Add("RegistrationNo");
            dt.Columns.Add("EncounterNo");
            dt.Columns.Add("PatientName");
            dt.Columns.Add("ServiceName");
            dt.Columns.Add("PackageName");
            dt.Columns.Add("Status");
            gvDetails.DataSource = dt;
            gvDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            HiddenField hdnStatusId = (HiddenField)e.Item.FindControl("hdnStatusId");
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDelete");
            if (common.myStr(hdnStatusId.Value)=="45")
            {
                imgDelete.Visible = true;
            }
            else
            {
                imgDelete.Visible = false;
            }
        }
    }
}
