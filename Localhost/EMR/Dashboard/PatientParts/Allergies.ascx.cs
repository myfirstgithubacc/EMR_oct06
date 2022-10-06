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
using Telerik.Web.UI;
using BaseC;

public partial class EMR_Dashboard_Parts_Allergies : WebPartBase
{
    
    DL_Funs fun = new DL_Funs();
    string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    public EMR_Dashboard_Parts_Allergies()
    {
        this.Title = "Allergies (Active)";
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        BindGDAllergies();
        if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0")
        {
            lnkAllergyRedirect.Enabled = true;
        }
        else
        {
            lnkAllergyRedirect.Enabled = false;

        }
    }

    public void BindGDAllergies()
    {
        try
        {
            Dashboard dsh = new Dashboard();

            DataSet dsApp = new DataSet();
            dsApp = dsh.getAllergies(common.myInt(Session["registrationid"]));

            if (dsApp.Tables[1].Rows.Count > 0)
            {
                if (common.myStr(dsApp.Tables[1].Rows[0]["NoAllergies"]) == "True")
                {
                    lblEmptyData.Visible = true;
                }
                else
                {
                    GDAllergy.DataSource = dsApp;
                    GDAllergy.DataBind();
                }
            }
            else
            {
                GDAllergy.DataSource = dsApp;
                GDAllergy.DataBind();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ibtnPlus_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnPlus = sender as ImageButton;
        GridViewRow row = (GridViewRow)btnPlus.NamingContainer;
        HtmlTableRow tdMinus = row.FindControl("tdMinus") as HtmlTableRow;
        HtmlTableRow tdPlus = row.FindControl("tdPlus") as HtmlTableRow;

        DataList dlAllergy = row.FindControl("dlAllergy") as DataList;
        tdMinus.Visible = true;
        dlAllergy.Visible = true;
        tdPlus.Visible = false;
    }

    protected void lnkPlusAllergyType_Click(object sender, EventArgs e)
    {
        LinkButton btnPlus = sender as LinkButton;
        GridViewRow row = (GridViewRow)btnPlus.NamingContainer;
        HtmlTableRow tdMinus = row.FindControl("tdMinus") as HtmlTableRow;
        HtmlTableRow tdPlus = row.FindControl("tdPlus") as HtmlTableRow;

        DataList dlAllergy = row.FindControl("dlAllergy") as DataList;
        tdMinus.Visible = true;
        dlAllergy.Visible = true;
        tdPlus.Visible = false;
    }

    protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnMinus = sender as ImageButton;
        GridViewRow row = (GridViewRow)btnMinus.NamingContainer;
        HtmlTableRow tdMinus = row.FindControl("tdMinus") as HtmlTableRow;
        HtmlTableRow tdPlus = row.FindControl("tdPlus") as HtmlTableRow;

        DataList dlAllergy = row.FindControl("dlAllergy") as DataList;
        tdMinus.Visible = false;
        dlAllergy.Visible = false;
        tdPlus.Visible = true;
    }

    protected void lnkMinusAllergyType_Click(object sender, EventArgs e)
    {
        LinkButton btnMinus = sender as LinkButton;
        GridViewRow row = (GridViewRow)btnMinus.NamingContainer;
        HtmlTableRow tdMinus = row.FindControl("tdMinus") as HtmlTableRow;
        HtmlTableRow tdPlus = row.FindControl("tdPlus") as HtmlTableRow;

        DataList dlAllergy = row.FindControl("dlAllergy") as DataList;
        tdMinus.Visible = false;
        dlAllergy.Visible = false;
        tdPlus.Visible = true;
    }

    protected void GDAllergy_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void GDAllergy_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDAllergy.PageIndex = e.NewPageIndex;
        BindGDAllergies();
    }
    public void setLink()
    {
       // Response.Redirect("~/EMR/Allergy/Allergy.aspx");

        //if (common.myStr(Request.QueryString["From"]) == "POPUP")
        //{
        //    RadWindow1.NavigateUrl = "/EMR/Allergy/Allergy.aspx?Regno=" + Request.QueryString["Regno"] + "&Encno=" + Request.QueryString["Encno"] + "&From=POPUP&Source=IPD";
        //    RadWindow1.Height = 560;
        //    RadWindow1.Width = 930;
        //    RadWindow1.Top = 10;
        //    RadWindow1.Left = 10;
        //    RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        //    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //    RadWindow1.Modal = true;
        //    RadWindow1.VisibleStatusbar = false;
        //    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        //}
        //else
        //{
        //    Response.Redirect("~/EMR/Allergy/Allergy.aspx", false);
        //}
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        BindGDAllergies();
    }
    protected void lnkAllergy_OnClick(object sender, EventArgs e)
    {
        setLink();
    }

}
