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

public partial class EMR_Assessment_DiagnosisDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.DiagnosisDA objDiag;
    DataSet ds;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGroup();
            BlankDiseaseGrid();
            BlankSubDiseaseGrid();
        }
    }
    private void BindGroup()
    {
        try
        {
            objDiag = new BaseC.DiagnosisDA(sConString);
            ds = new DataSet();
            ds = objDiag.BindCategory();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlGroup.DataSource = ds.Tables[0];
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "GroupId";
                ddlGroup.DataBind();
                ddlGroup_OnSelectedIndexChanged(this,null);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlGroup_OnSelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            if (ddlGroup.SelectedValue != "" || ddlGroup.SelectedValue != "0")
            {
                objDiag = new BaseC.DiagnosisDA(sConString);
                ds = new DataSet();
                ds = objDiag.BindSubCategory(ddlGroup.SelectedValue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlSubGroup.DataSource = ds.Tables[0];
                    ddlSubGroup.DataValueField = "SubGroupId";
                    ddlSubGroup.DataTextField = "SubGroupName";
                    ddlSubGroup.DataBind();
                    ddlSubGroup.Items.Insert(0, new RadComboBoxItem("All", "0"));
                    ddlSubGroup_OnSelectedIndexChanged(this,null);
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
    protected void ddlSubGroup_OnSelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            BlankSubDiseaseGrid();
            if (ddlSubGroup.SelectedValue != "" )
            {
                objDiag = new BaseC.DiagnosisDA(sConString);
                ds = new DataSet();
                ds = objDiag.GetICD10Disease(ddlGroup.SelectedValue, ddlSubGroup.SelectedValue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvDisease.DataSource = ds.Tables[0];
                    gvDisease.DataBind();
                }
                else
                {
                    BlankDiseaseGrid();
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
    private void BlankDiseaseGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DiseaseId");
        dt.Columns.Add("SubGroupId");
        dt.Columns.Add("DiseaseCode");
        dt.Columns.Add("DiseaseName");
        DataRow dr = dt.NewRow();
        dr["DiseaseId"] = "";
        dr["SubGroupId"] = "";
        dr["DiseaseCode"] = "";
        dr["DiseaseName"] = "";
        dt.Rows.Add(dr);
        gvDisease.DataSource = dt;
        gvDisease.DataBind();
    }

    private void BlankSubDiseaseGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SubGroupId");
        dt.Columns.Add("DiseaseId");
        dt.Columns.Add("DiseaseCode");
        dt.Columns.Add("ICDCode");
        dt.Columns.Add("Description");
        dt.Columns.Add("ValidForClinicalUse");
        dt.Columns.Add("ValidForPrimaryDiagnosis");
        dt.Columns.Add("AgeRange");
        dt.Columns.Add("Gender");
        dt.Columns.Add("ICDID");
        DataRow dr = dt.NewRow();
        dr["SubGroupId"] = "";
        dr["DiseaseId"] = "";
        dr["DiseaseCode"] = "";
        dr["ICDCode"] = "";
        dr["Description"] = "";
        dr["ValidForClinicalUse"]="";
        dr["ValidForPrimaryDiagnosis"] = "";
        dr["AgeRange"]="";
        dr["Gender"]="";
        dr["ICDID"] = "";
        dt.Rows.Add(dr);
        gvSubDisease.DataSource = dt;
        gvSubDisease.DataBind();
    }
    protected void gvSubDisease_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }
    }
    protected void gvDisease_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }
    }
    
    protected void gvDisease_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BlankSubDiseaseGrid();
        GridViewRow row = this.gvDisease.SelectedRow;
        Label DiseaseId = (Label)row.FindControl("lblDiseaseId");
        if (DiseaseId.Text != "" || DiseaseId.Text != "0")
        {
            objDiag = new BaseC.DiagnosisDA(sConString);
            ds = new DataSet();
            ds = objDiag.GetICD10SubDiseases(DiseaseId.Text.Trim());
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSubDisease.DataSource = ds.Tables[0];
                gvSubDisease.DataBind();
            }
        }
    }

    protected void gvSubDisease_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = this.gvSubDisease.SelectedRow;
        Label lblICDID = (Label)row.FindControl("lblICDID");
        hdnICDId.Value = lblICDID.Text;
    }

    protected void btnAddToFavourite_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.DiagnosisDA ObjDiagnosis = new BaseC.DiagnosisDA(sConString);
            if (hdnICDId.Value != "" && hdnICDId.Value != "0")
            {
                string strsave = ObjDiagnosis.SaveFavouriteDiagnosis(common.myInt(Session["DoctorID"]), common.myInt(hdnICDId.Value), common.myInt(Session["UserID"]));
                if (strsave.Contains("AlReady Exist in your favorite list"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = strsave;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strsave;

                }
            }
            else
            {
                Alert.ShowAjaxMsg("Select Diagnosis to add into favorite list", Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}
