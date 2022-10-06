using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Telerik.Web.UI;

public partial class SearchCounselingDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.PatientCounseling objBb;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CreateTable();
        }
    }
    protected void gvCounselingDetails_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvCounselingDetails.CurrentPageIndex = e.NewPageIndex;
        bindData();
    }
    protected void gvCounselingDetails_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                hdnCounseling.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnCounselingId")).Value);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                return;
            }
            if (e.CommandName == "Tag")
            {

            }
            if (e.CommandName == "Print")
            {
                hdnCounseling.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnCounselingId")).Value);


                RadWindowForNew.NavigateUrl = "/EMRREPORTS/PrintOPDReceipt.aspx?PrintType=CounselingDetailReport&intCounselingId=" + common.myStr(hdnCounseling.Value);
                RadWindowForNew.Height = 500;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                //RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
                //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }

            if (e.CommandName.Equals("ItemDelete"))
            {
                hdnCounseling.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnCounselingId")).Value);
                objBb = new BaseC.PatientCounseling();
                try
                {
                    string strMsg = objBb.ChangeCounselingStatus(common.myInt(hdnCounseling.Value), 0);
                    //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    CreateTable();
                    // lblMessage.Text = strMsg;
                    Alert.ShowAjaxMsg("Counselling Status Changed Successfully.", Page);
                    //  return;            
                }
                catch (Exception Ex)
                {
                    // lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    // lblMessage.Text = "Error: " + Ex.Message;
                    objException.HandleException(Ex);
                }
            }
        }
        catch (Exception Ex)
        {
            // lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            // lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void bindData()
    {
    }
    void CreateTable()
    {
        DataSet ds;
        objBb = new BaseC.PatientCounseling();
        try
        {
            if (common.myStr(Request.QueryString["EstimationOrderLOSExp"]).Equals("WARDDETAIL"))
            {
                ds = objBb.GetCounselingDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, 1, common.myInt(Request.QueryString["RegId"]), common.myStr(Request.QueryString["EstimationOrderLOSExp"]));
                Counselling.Text = "Estimation LOS";
                EMREstimationOrder.Visible = false;
                EMREstimationOrder1.Visible = false;
            }
            else
            {
                ds = objBb.GetCounselingDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, 1, common.myInt(Request.QueryString["RegId"]));

            }
            //  ds=objBb.GetCounselingDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0,common.myStr(ddlSearchOn.SelectedValue), 1);

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            gvCounselingDetails.DataSource = ds.Tables[0];
            gvCounselingDetails.DataBind();
            lbltotcount.Text = common.myStr(ds.Tables[0].Rows.Count);

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
    protected void ddlSearchOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        CreateTable();
    }
    protected void gvCounselingDetails_PreRender(object sender, EventArgs e)
    {
        CreateTable();
    }
    protected void gvCounselingDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            HiddenField status = (HiddenField)item.FindControl("hdnStatus");
            ImageButton imgButtonDelete = (ImageButton)item.FindControl("ibtnDelete");
            LinkButton IbtnSelect = (LinkButton)item.FindControl("IbtnSelect");
            LinkButton IbtnPrint = (LinkButton)item.FindControl("IbtnPrint");

            if (common.myStr(Request.QueryString["EstimationOrderLOSExp"]).Equals("WARDDETAIL"))
            {
                this.gvCounselingDetails.Columns[0].Visible = false;
                this.gvCounselingDetails.Columns[8].Visible = false;
                //this.gvCounselingDetails.Columns[12].Visible = false;
                this.gvCounselingDetails.Columns[13].Visible = false;
                this.gvCounselingDetails.Columns[14].Visible = false;
                this.gvCounselingDetails.Columns[15].Visible = false;
                this.gvCounselingDetails.Columns[16].Visible = false;
                this.gvCounselingDetails.Columns[17].Visible = false;
                this.gvCounselingDetails.Columns[18].Visible = false;

                //this.gvCounselingDetails.Columns[11].Visible = false;
                //this.gvCounselingDetails.Columns[12].Visible = false;


            }
            if (status.Value.ToString().Trim().Equals("False"))
            {
                item.BackColor = Color.LightGray;
                imgButtonDelete.Enabled = false;
            }
        }
    }
}
