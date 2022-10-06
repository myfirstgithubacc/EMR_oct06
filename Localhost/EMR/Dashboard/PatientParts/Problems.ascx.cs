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
using BaseC;
using Telerik.Web.UI;
public partial class EMR_Dashboard_Parts_Problems : WebPartBase
{
    
    DL_Funs fun = new DL_Funs();
    clsExceptionLog objException = new clsExceptionLog();

    public EMR_Dashboard_Parts_Problems()
    {
        this.Title = "Problems";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindProblems();
        if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0")
        {

            lnkProblemRedirect.Enabled = true;
        }
        else
        {
            lnkProblemRedirect.Enabled = false;
        }
        if (common.myStr(HttpContext.Current.Session["ModuleIdValue"]) == "41")
        {
            lnkProblemRedirect.Enabled = false;
        }
    }

    public void BindProblems()
    {
        try
        {
            Dashboard dsh = new Dashboard();

            DataSet dsApp = new DataSet();
            dsApp = dsh.getDashBoardValue(Convert.ToInt16(Session["HospitalLocationID"]), common.myInt(Session["registrationid"]),
                                    hdnFromDate.Text, hdnToDate.Text, hdnDateVale.Text, hdnEncounterNumber.Text == "" ? Session["EncounterId"].ToString() : hdnEncounterNumber.Text, common.myInt(Session["FacilityID"]),
                                    "UspEMRGetPatientProblems");
            GDProblems.DataSource = dsApp;
            GDProblems.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GDProblems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String Context = DataBinder.Eval(e.Row.DataItem, "Context") as string;
            String Duration = DataBinder.Eval(e.Row.DataItem, "Duration") as string;
            String Quality = DataBinder.Eval(e.Row.DataItem, "Quality") as string;
            String Serverity = DataBinder.Eval(e.Row.DataItem, "Severity") as string;
            String Str = "?cntx=" + Context + "&dur=" + Duration + "&qlty=" + Quality + "&svty=" + Serverity;

            Label lnkProblem = e.Row.FindControl("lnkProblem") as Label;
            lnkProblem.Text = lnkProblem.Text.PadRight(38).Substring(0, 35);

            Label lblDate = (Label)e.Row.FindControl("lblDate");
            lblDate.Text = common.myDate(lblDate.Text).ToString(common.myStr(Session["OutputDateformat"]));
        }
    }
    protected void GDProblems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDProblems.PageIndex = e.NewPageIndex;
        BindProblems();
    }
    //PostBackUrl=""
    public void setLink()
    {

    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        BindProblems();
    }
    protected void lnkProblem_OnClick(object sender, EventArgs e)
    {
        setLink();
    }
}