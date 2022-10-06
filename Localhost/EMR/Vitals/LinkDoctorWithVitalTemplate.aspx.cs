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
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;

public partial class EMR_Vitals_LinkDoctorWithVitalTemplate : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
     lblMessage.Text = "";
        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            BindDoctorGrid();
            BindVitalTemplateGrid();
        }
    }

    private void BindDoctorGrid()
    {
        try
        {
            //BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
            //  DataSet ds = objEMRVitals.GetDoctors(Convert.ToInt16(Session["HospitalLocationID"]));
            if (Session["HospitalLocationID"] != null)
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
                hshIn.Add("intFacilityId", common.myInt(Session["FacilityId"]));
                DataSet dsDoctors = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hshIn);

                if (dsDoctors.Tables[0].Rows.Count > 0)
                {
                    gvDoctors.DataSource = dsDoctors;
                    gvDoctors.DataBind();
                }
                else
                {
                    gvDoctors.DataSource = null;
                    gvDoctors.DataBind();
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

    private void BindDoctorSearchGrid(string DoctorText)
    {
        try
        {
            BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);

            DataSet ds = objEMRVitals.GetDoctorsForVitalTemplates(Convert.ToInt16(Session["HospitalLocationID"]), DoctorText);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDoctors.DataSource = ds;
                gvDoctors.DataBind();
            }
            else
            {
                gvDoctors.DataSource = null;
                gvDoctors.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindVitalTemplateGrid()
    {
        try
        {
            BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
            DataSet ds = objEMRVitals.GetVitalTemplate(Convert.ToInt16(Session["HospitalLocationID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvVitalTemplates.DataSource = ds;
                gvVitalTemplates.DataBind();
            }
            else
            {
                //gvVitalTemplates.DataSource = null;
                //gvVitalTemplates.DataBind();
                BindBlankgvTemplate();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ibtnLinkDoctorVitalTemplates_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if (gvDoctors.SelectedRow != null && gvVitalTemplates.SelectedRow != null)
            {

                BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
                String str = objEMRVitals.LinkDoctorVitalTemplate(Convert.ToInt32(gvDoctors.SelectedRow.Cells[0].Text), Convert.ToInt32(gvVitalTemplates.SelectedRow.Cells[0].Text), Convert.ToInt32(Session["UserID"]));
                if (str.Length == 5)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = "Record(s) Has Been Saved...";
                }
                else
                {
                    lblMessage.Text = "Error In Saving...";
                }
                //Alert.ShowAjaxMsg(str, this.Page);
                BindVitalTemplateGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDoctors_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvDoctors, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvDoctors_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvDoctors.PageIndex = e.NewPageIndex;
        BindDoctorGrid();
    }

    protected void gvVitalTemplates_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
    }

    protected void gvVitalTemplates_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;

            if (ViewState["BlankGrid"] != null)
            {
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvVitalTemplates, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvVitalTemplates_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
    }

    protected void BindBlankgvTemplate()
    {
        try
        {
            DataTable Dt = new DataTable();

            Dt.Columns.Add("ID");
            Dt.Columns.Add("TemplateName");

            //for (int i = 0; i < 4; i++)
            //{
            DataRow dr = Dt.NewRow();
            //  dr["SerialNo"] = "";
            dr["Id"] = "";
            dr["TemplateName"] = "";

            Dt.Rows.Add(dr);
            //}
            ViewState["BlankGrid"] = "True";
            gvVitalTemplates.DataSource = Dt;
            gvVitalTemplates.DataBind();
            ViewState["BlankGrid"] = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
