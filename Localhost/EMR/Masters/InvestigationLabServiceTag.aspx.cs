using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Configuration;
using System.Text;
using System.Collections;

public partial class EMR_Masters_InvestigationLabServiceTag : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindService();
            BindLabTemplate();
            BindGrid();
            lblHeader.Text = common.myStr(Request.QueryString["FName"]);
            if (common.myStr(Request.QueryString["FType"]) == "IS")
            {
                ddlLabTemplate.Visible = false;
                Label1.Visible = false;
            }

        }
    }
    protected void ddlServiceName_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {            
            BindLabTemplate();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void BindService()
    {
        BaseC.InvestigationFormat invService = new BaseC.InvestigationFormat(sConString);
        DataSet objDs = invService.GetInvestigationServices("", "'IS','I'", common.myStr(Session["HospitalLocationID"]), common.myInt(Session["StationId"]), common.myInt(Session["FacilityId"]));
        if (objDs.Tables.Count > 0)
        {
            if (objDs.Tables[0].Rows.Count > 0)
            {
                objDs.Tables[0].DefaultView.Sort = "ServiceName";
                ddlServiceName.DataSource = objDs.Tables[0].DefaultView;
                ddlServiceName.DataValueField = "ServiceID";
                ddlServiceName.DataTextField = "ServiceNameWithCode";
                ddlServiceName.DataBind();
            }
        }
        ddlServiceName.Items.Insert(0, new RadComboBoxItem("", "0"));
        ddlServiceName.SelectedIndex = 0;
    }
    private void BindLabTemplate()
    {
        try
        {
            if (common.myStr(Request.QueryString["FType"]) == "I")
            {
                ddlLabTemplate.Items.Clear();
                BaseC.clsServiceTaging objGetServiceTag = new BaseC.clsServiceTaging(sConString);
                DataSet objDs = objGetServiceTag.GetServiceTaging(common.myInt(ddlServiceName.SelectedValue));
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    ddlLabTemplate.DataSource = objDs.Tables[0];
                    ddlLabTemplate.DataValueField = "FieldId";
                    ddlLabTemplate.DataTextField = "FieldName";
                    ddlLabTemplate.DataBind();
                }
            }
            ddlLabTemplate.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlLabTemplate.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnSaveData_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(ddlServiceName.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Service ...";
            //Alert.ShowAjaxMsg("Please Select Service ...", this.Page);
            return;
        }
        if (common.myInt(ddlLabTemplate.SelectedValue) == 0 && common.myStr(ddlLabTemplate.SelectedValue) == "I")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Lab Template ...";
            // Alert.ShowAjaxMsg("Please Select Lab Template ...", this.Page);
            return;
        }

        BaseC.clsServiceTaging objServiceTag = new BaseC.clsServiceTaging(sConString);
        string FieldId = Request.QueryString["FieldId"].Substring(1, Request.QueryString["FieldId"].Length - 1);
        string strMsg = objServiceTag.SaveTemplateFieldInvestigations(Convert.ToInt32(0), common.myInt(FieldId), common.myInt(ddlServiceName.SelectedValue), common.myInt(ddlLabTemplate.SelectedValue), common.myInt(Session["UserId"]));

        if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = "Record Saved ...";
            BindGrid();

        }
    }
    protected void gvSelectedFields_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Delete")
            {

               // GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                Label  lblId = (Label)e.Item.FindControl("lblId");
                Hashtable HshIn = new Hashtable();
              //  Hashtable HshOut = new Hashtable();
                HshIn.Add("@intFieldId", lblId.Text);
                // HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
              
                BaseC.clsServiceTaging objServiceTagging = new BaseC.clsServiceTaging(sConString);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                int i = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, "Update  EMRTemplateFieldInvestigations set Active =0 where Id=@intFieldId", HshIn));
                if (i == 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = "Record Deleted ...";
                    BindGrid();
                    return;
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
    private void BindGrid()
    {
        try
        {
            BaseC.clsServiceTaging objGetServiceTag = new BaseC.clsServiceTaging(sConString);
            string FieldId = Request.QueryString["FieldId"].Substring(1, Request.QueryString["FieldId"].Length - 1);
            DataSet objDs = objGetServiceTag.GetTemplateFieldInvestigations(common.myInt(FieldId));

            if (objDs.Tables[0].Rows.Count > 0)
            {
                gvSelectedFields.DataSource = objDs;
                gvSelectedFields.DataBind();
            }
            else
            {
                DataTable objDt = CreateTable();
                gvSelectedFields.DataSource = objDt;
                gvSelectedFields.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    private DataTable CreateTable()
    {
        DataTable objDt = new DataTable();
        objDt.Columns.Add("Id");
        objDt.Columns.Add("ServiceName");
        objDt.Columns.Add("FieldName");
        
        return objDt;
    }



}
