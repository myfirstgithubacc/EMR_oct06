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
public partial class EMR_Templates_TemplateNotesPrint : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlTempGroup.Visible = false;

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            hdnIsUnSavedData.Value = "0";
          //  BindPatientHiddenDetails();
            //BindddlTempGroup();
            bindAllTemplateList();
            txtAllTemplateSearch.Focus();
        }
    }
    //void BindPatientHiddenDetails()
    //{
        //try
        //{
        //    if (common.myStr(Request.QueryString["RPD"]) != "")
        //    {
        //        lblPatientDetail.Text = common.myStr(Session["RelationPatientDetailString"]);
        //    }
        //    else if (Session["PatientDetailString"] != null)
        //    {
        //        lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
        //    }
        //}
        //catch
        //{
        //}
   // }
    protected void ddlTemplateTypeCode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtAllTemplateSearch.Text = "";
            lblMessage.Text = "&nbsp;";
            bindAllTemplateList();
        }
        catch
        {
        }
    }
    protected void ddlTempGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindAllTemplateList();
    }

    private void BindddlTempGroup()
    {
        DAL.DAL objFav = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();
        DataSet ds = new DataSet();
        try
        {
            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            ds = objFav.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTemplateGroup", HshIn);
            ddlTempGroup.DataSource = ds;
            ddlTempGroup.DataTextField = "GroupName";
            ddlTempGroup.DataValueField = "GroupId";
            ddlTempGroup.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
    private void bindAllTemplateList()
    {
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        BaseC.clsIVF objivf = new BaseC.clsIVF(sConString);
        try
        {
            Label1.Text = common.myStr(ddlTemplateTypeCode.SelectedItem.Text);

            ds = objivf.getEMRTemplateTypeWiseConsent(common.myInt(Session["HospitalLocationId"]), common.myStr(ddlTemplateTypeCode.SelectedValue), common.myStr(txtAllTemplateSearch.Text.Trim()), common.myInt(Session["EncounterId"]));

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
                ds.Tables[0].AcceptChanges();
            }

            gvAllTemplate.DataSource = ds.Tables[0];
            gvAllTemplate.DataBind();
            txtAllTemplateSearch.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objivf = null;
        }
    }
    protected void gvAllTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            if (common.myStr(e.CommandName).ToUpper().Equals("PRINT"))
            {
                if (common.myInt(Session["EncounterId"]).Equals(0))
                {
                    lblMessage.Text = "Patient not selected!!";
                    return;
                }

                int TemplateId = common.myInt(e.CommandArgument);
                if (TemplateId > 0)
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblTemplateName = (Label)row.FindControl("lblTemplateName");

                    //RadWindow2.NavigateUrl = "/EMR/Templates/TemplateNotesReport.aspx?TemplateId=" + TemplateId + "&TemplateName=" + common.myStr(lblTemplateName.Text).Trim();
                    RadWindow2.NavigateUrl = "/EMR/Templates/TemplateNotesReportSSRS.aspx?TemplateId=" + TemplateId;

                    RadWindow2.Height = 500;
                    RadWindow2.Width = 1000;
                    RadWindow2.Top = 10;
                    RadWindow2.Left = 10;
                    RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow2.Modal = true;
                    RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindow2.VisibleStatusbar = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
    protected void gvAllTemplate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            gvAllTemplate.PageIndex = e.NewPageIndex;
            bindAllTemplateList();
        }
        catch
        {
        }
    }
    protected void gvAllTemplate_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            //if (ddlTemplateTypeCode.SelectedIndex == 0)
            //{
            //    e.Row.Cells[1].Visible = false;
            //    e.Row.Cells[2].Visible = true;
            //}
            //else
            //{
            //    e.Row.Cells[1].Visible = true;
            //    e.Row.Cells[2].Visible = false;
            //}
        }
    }
    protected void btnAllTemplateSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindAllTemplateList();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}

