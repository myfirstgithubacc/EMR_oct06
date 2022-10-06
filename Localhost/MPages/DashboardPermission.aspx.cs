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
using System.Text;

public partial class MPages_DashboardPermission : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FillBillingMisDashboardData();
            lblGroupDetails.Text = "Group Name - " + common.myStr(Request.QueryString["GroupName"]);

        }
    }
    public void FillBillingMisDashboardData()
    {
        string procedureName = "";
        if (rdlLIst.SelectedValue == "1")
        {
            procedureName = "UspDashBoardPermission";
        }
        else
        {
            procedureName = "UspPHRgetDashBoardPermission";
        }
        BaseC.clsMISDashboard clsMIS = new BaseC.clsMISDashboard(sConString);
        DataSet ds = new DataSet();
        ds = clsMIS.getMISDashBoardPermission(common.myInt(Session["HospitalLocationId"]),
            common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["GroupId"]),procedureName);
        gvPermission.DataSource = ds;
        gvPermission.DataBind();
        if (rdlLIst.SelectedValue == "1")
        {
            gvPermission.Columns[2].Visible = true;
            Label l1 = (Label)gvPermission.HeaderRow.FindControl("lblHeader1");
            l1.Text = "MIS DashBoard";
        }
        else
        {
            gvPermission.Columns[2].Visible = false;
            Label l1 = (Label)gvPermission.HeaderRow.FindControl("lblHeader1");
            l1.Text = "Pharmacy DashBoard";
        }

    }
    protected void ibtnSave_OnClick(object sender, EventArgs e)
    {
        ArrayList colService = new ArrayList();
        StringBuilder sXMLOrderServices = new StringBuilder();
        foreach (GridViewRow item in gvPermission.Rows)
        {
            HiddenField hdnpermissionId = (HiddenField)item.FindControl("hdnpermissionId");
            HiddenField hdnDashBoardType = (HiddenField)item.FindControl("hdnDashBoardType");
            CheckBox chkMisDashBoard = (CheckBox)item.FindControl("chkMisDashBoard");
            CheckBox chkBillingDashBoard = (CheckBox)item.FindControl("chkBillingDashBoard");

            if (chkBillingDashBoard.Checked)
            {
                colService.Add(common.myInt(hdnpermissionId.Value));
                colService.Add(common.myStr("B"));
                sXMLOrderServices.Append(common.setXmlTable(ref colService));
            }
            if (chkMisDashBoard.Checked)
            {
                colService.Add(common.myInt(hdnpermissionId.Value));
                if (rdlLIst.SelectedValue == "1")
                {
                    colService.Add(common.myStr("M"));
                }
                else
                {
                    colService.Add(common.myStr("P"));
                }
                sXMLOrderServices.Append(common.setXmlTable(ref colService));
            }
        }
        string procedureName = "";
        if (rdlLIst.SelectedValue == "1")
        {
            procedureName = "UspSaveDashBoardPermission";
        }
        else
        {
            procedureName = "UspSavePHRDashBoardPermission";
        }
        BaseC.Security objSec1 = new BaseC.Security(sConString);
        string msg = objSec1.SaveDashboardAccessRights(common.myInt(Request.QueryString["GroupId"]), sXMLOrderServices.ToString(),
            common.myInt(Session["userid"]), common.myInt(Session["Hospitallocationid"]), common.myInt(Session["FacilityId"]), procedureName);

    }
    protected void rdlLIst_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        FillBillingMisDashboardData();
    }
}
