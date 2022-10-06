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

public partial class EMR_Templates_AddFieldValue : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds;
    DAL.DAL objDl;

    protected void Page_Load(object sender, EventArgs e)
    {
        objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            if (!IsPostBack)
            {
                lblTemplate.Text = "";
                lblSection.Text = "";
                lblField.Text = "";

                if (Request.QueryString["ID"] != null)
                {
                    hdControlId.Value = Request.QueryString["ID"].ToString().Trim();
                }
                if (Request.QueryString["ControlType"] != null)
                {
                    hdControlType.Value = Request.QueryString["ControlType"].ToString().Trim();
                }
                if (Request.QueryString["SectionId"] != null)
                {
                    hdnSectionId.Value = Request.QueryString["SectionId"].ToString().Trim();
                }
                if (Request.QueryString["FieldId"] != null)
                {
                    hdnFieldId.Value = Request.QueryString["FieldId"].ToString().Trim();
                }
                BindTemplateValueGridView();
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
            objDl = null;
        }
       
    }
    private void BindBlankGrid()
    {
        DataTable dt = new DataTable();
        try
        {

            dt.Columns.Add("FieldId");
            dt.Columns.Add("ValueId");
            dt.Columns.Add("ValueName");
            dt.Columns.Add("FieldType");
            dt.Columns.Add("Active");
            DataRow dr = dt.NewRow();
            dr["FieldId"] = "";
            dr["ValueId"] = "";
            dr["FieldType"] = "";
            dr["ValueName"] = "";
            dr["Active"] = "";
            dt.Rows.Add(dr);
            gvTemplateField.DataSource = dt;
            gvTemplateField.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }

    }
    protected void BindTemplateValueGridView()
    {
        ds = new DataSet();
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        try
        {
            gvTemplateField.DataSource = null;
            gvTemplateField.DataBind();


            ds = objEMR.GetTemplateFieldValue(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(hdnSectionId.Value),
                Convert.ToInt32(hdnFieldId.Value));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTemplateField.DataSource = ds;
                gvTemplateField.DataBind();
                lblTemplate.Text = ds.Tables[0].Rows[0]["TemplateName"].ToString();
                lblSection.Text = ds.Tables[0].Rows[0]["SectionName"].ToString();
                lblField.Text = ds.Tables[0].Rows[0]["FieldName"].ToString();
            }
            else
            {
                BindBlankGrid();
                lblMessage.Text = "No matching record(s) found.";
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
            objEMR =null;
        }
    }
    protected void btnAddValue_Click(object sender, EventArgs e)
    {
        Hashtable hshOut = new Hashtable();
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        try
        {
            if (txtValueName.Text != "")
            {

                hshOut = objEMR.SaveTemplateValues(0, Convert.ToInt32(hdnFieldId.Value), txtValueName.Text, Convert.ToBoolean(1),
                    false, Convert.ToInt32(Session["UserId"]));
                lblMessage.Text = hshOut["@chvErrorOutput"].ToString();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                BindTemplateValueGridView();
                txtValueName.Text = "";
            }
            else
            {
                Alert.ShowAjaxMsg("Please type Value Name", Page);
                return;
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
            hshOut =null;
            objEMR =null;
        }
    }
    protected void btnStore_OnClick(object sender, EventArgs e)
    {
        Session["CValueId"] = hdnValueId.Value;
    }
    protected void gvTemplateField_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = this.gvTemplateField.SelectedRow;
            HiddenField ValueId = (HiddenField)row.FindControl("hdnValueId");
            HiddenField hdnFieldType = (HiddenField)row.FindControl("hdnFieldType");
            hdnValueId.Value = ValueId.Value;
            hdControlType.Value = hdnFieldType.Value;
            Session["ValueId"] = ValueId.Value;
            lblField.Text = "";
            lblSection.Text = "";
            lblTemplate.Text = "";
            gvTemplateField.DataSource = null;
            gvTemplateField.DataBind();
            txtValueName.Text = "";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //hdControlType.Value = "";
       // hdnSectionId.Value = "";
    }
    protected void gvTemplateField_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblActive = (Label)e.Row.FindControl("lblActive");
                if (lblActive != null)
                {
                    if (lblActive.Text == "True")
                    {
                        e.Row.Cells[3].Visible = true;
                    }
                    else
                    {
                        e.Row.Cells[3].Visible = false;
                    }
                }
                //DropDownList ddlActive = (DropDownList)e.Row.FindControl("ddlActive");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvTemplateField_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        BaseC.EMR emr = new BaseC.EMR(sConString);
        Hashtable hshOutput = new Hashtable();
        try
        {
           
            HiddenField hdnValueId = (HiddenField)gvTemplateField.Rows[e.RowIndex].FindControl("hdnValueId");
            HiddenField hdnFieldId = (HiddenField)gvTemplateField.Rows[e.RowIndex].FindControl("hdnFieldId");
            TextBox txtValueName = (TextBox)gvTemplateField.Rows[e.RowIndex].FindControl("txtValueName");
            DropDownList ddlActive = (DropDownList)gvTemplateField.Rows[e.RowIndex].FindControl("ddlActive");

             hshOutput = emr.SaveTemplateValues(Convert.ToInt32(hdnValueId.Value), Convert.ToInt32(hdnFieldId.Value), txtValueName.Text, Convert.ToBoolean(ddlActive.SelectedValue),
                    true, Convert.ToInt32(Session["UserId"]));
            lblMessage.Text = hshOutput["@chvErrorOutput"].ToString();
            lblMessage.ForeColor = System.Drawing.Color.Green;
            gvTemplateField.EditIndex = -1;
            BindTemplateValueGridView();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            emr = null;
            hshOutput =null;
        
        }

    }
    protected void gvTemplateField_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            gvTemplateField.EditIndex = e.NewEditIndex;
            BindTemplateValueGridView();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void gvTemplateField_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvTemplateField.EditIndex = -1;
            BindTemplateValueGridView();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnclose_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblField.Text = "";
            lblSection.Text = "";
            lblTemplate.Text = "";
            gvTemplateField.DataSource = null;
            gvTemplateField.DataBind();
            txtValueName.Text = "";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
       // hdControlType.Value = "";
       // hdnSectionId.Value = "";
    }
}
