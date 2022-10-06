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
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using System.Net;


public partial class EMR_ReferralPatientHistory : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    String sqlstr = "";
    BaseC.ParseData bc = new BaseC.ParseData();
    BaseC.Hospital baseHc;
    BaseC.ATD objbc;
    BaseC.WardManagement objwd;
    DataSet ds;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["MASTER"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        baseHc = new BaseC.Hospital(sConString);
        objbc = new BaseC.ATD(sConString);
        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            dtpfromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
            dtpfromDate.SelectedDate = DateTime.Now.AddDays(-15);
            dtpToDate.SelectedDate = DateTime.Now;
            BindGrid();
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        BindGrid();
    }
    public void BindGrid()
    {
        objwd = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        ds = objwd.GetReferralDetailForSerach(common.myInt(Session["UserId"]), 0, "B", common.myInt(Session["FacilityId"]), ddlName.SelectedValue == "R" ? txtSearch.Text : "", common.myInt(ddlStatus.SelectedValue),
            0, ddlName.SelectedValue == "N" ? txtSearch.Text : "", common.myInt(ddlReferral.SelectedValue), common.myDate(dtpfromDate.SelectedDate), common.myDate(dtpToDate.SelectedDate));


        if (ds.Tables[0].Rows.Count == 0)
        {
            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dr);
        }
        gvDetails.DataSource = ds;
        gvDetails.DataBind();
    }

    protected void gvDetails_OnItemDataBound(object source, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.SelectedItem)
        {
            Label txtUrgent = (Label)e.Item.FindControl("txtUrgent");

            Label txConcludeReferral = (Label)e.Item.FindControl("txConcludeReferral");

            if (txtUrgent.Text.ToUpper() == "STAT")
            {
                e.Item.BackColor = System.Drawing.Color.LightGreen;
            }

        }
    }
    protected void gvDetails_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        BindGrid();
    }



}
