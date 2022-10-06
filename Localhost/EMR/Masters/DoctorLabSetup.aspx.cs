using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMR_Masters_DoctorLabSetup : System.Web.UI.Page
{
    string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;//connectionstring
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateDoctor();
            PopulateServices();
            PopulateFields();
            PopulateFieldData();
        }
    }

    protected void ddlDoctor_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        PopulateFieldData();
        PopulateFields();
        lblMessage.Text = "";
    }

    private void PopulateDoctor()
    {
        try
        {
            ddlDoctor.Enabled = true;
            ddlDoctor.Enabled = true;

            Hashtable hstInput;
            hstInput = new Hashtable();
            hstInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", hstInput);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].DefaultView.RowFilter = "EmployeeType='D'";
                ddlDoctor.DataSource = ds.Tables[0].DefaultView;

                ddlDoctor.DataTextField = "EmployeeName";
                ddlDoctor.DataValueField = "EmployeeId";
                ddlDoctor.DataBind();
                ddlDoctor.SelectedIndex = 0;
                ddlDoctor.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
                ddlDoctor.SelectedIndex = 0;
            }
            //ddlDoctor.Items.Insert(0, new ListItem("[Select]", "0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlServices_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlDoctor.SelectedIndex != 0)
        {
            PopulateFields();
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select Doctor First !";
        }
    }
    private void PopulateServices()
    {
        BaseC.EMRMasters objM = new BaseC.EMRMasters(sConString);
        DataSet dsService = objM.GetService(common.myInt(Session["HospitalLocationID"]), string.Empty, string.Empty, common.myInt(Session["FacilityID"]));

        if (dsService.Tables[0].Rows.Count > 0)
        {
            ddlServices.DataSource = dsService.Tables[0];
            ddlServices.DataValueField = "ServiceID";
            ddlServices.DataTextField = "ServiceName";
            ddlServices.DataBind();
        }
    }
    private void PopulateFields()
    {
        BaseC.clsLISPhlebotomy objclsLISPhlebotomy = new BaseC.clsLISPhlebotomy(sConString);
        DataSet dsFields;
        try
        {

            dsFields = objclsLISPhlebotomy.getLISFormat(common.myInt(Session["HospitalLocationID"]), "", 0, common.myInt(Session["FacilityId"]), common.myInt(ddlServices.SelectedValue), common.myInt(ddlDoctor.SelectedValue), common.myBool(1));
            if (common.myInt(dsFields.Tables[0].Rows.Count) > 0)
            {
                gvFields.DataSource = dsFields.Tables[0];
                gvFields.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                gvFields.DataSource = dt;
                gvFields.DataBind();
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
            objclsLISPhlebotomy = null;
            dsFields = null;
        }
    }
    private void PopulateFieldData()
    {
        BaseC.clsEMR objclsEMR = new BaseC.clsEMR(sConString);
        DataSet dsFieldsData;
        try
        {
            dsFieldsData = objclsEMR.getDiagDoctorOutsideLab(common.myInt(Session["FacilityId"]), common.myInt(ddlDoctor.SelectedValue));

            if (common.myInt(dsFieldsData.Tables[0].Rows.Count) > 0)
            {
                gvFieldData.DataSource = dsFieldsData.Tables[0];
                gvFieldData.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                gvFieldData.DataSource = dt;
                gvFieldData.DataBind();
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
            objclsEMR = null;
            dsFieldsData = null;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objclsEMR = new BaseC.clsEMR(sConString);
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        Hashtable hsh;
        try
        {
            if (common.myInt(ddlDoctor.SelectedValue) > 0 && common.myInt(ddlServices.SelectedValue) > 0)
            {
                if (gvFields.Rows != null)
                {
                    if (gvFields.Rows.Count > 0)
                    {
                        btnSave.Visible = false;
                        foreach (GridViewRow row in gvFields.Rows)
                        {
                            HiddenField hdnFieldID = (HiddenField)row.FindControl("hdnFieldID");
                            CheckBox cbSelect = (CheckBox)row.FindControl("cbSelect");
                            if (cbSelect.Checked)
                            {
                                coll.Add(common.myInt(hdnFieldID.Value));//FieldId INT,
                                strXML.Append(common.setXmlTable(ref coll));
                            }
                        }

                        if (strXML.ToString() != "")
                        {
                            hsh = objclsEMR.SaveEMROutsideLabResultSetup(common.myInt(Session["FacilityId"]), strXML.ToString(), common.myInt(ddlDoctor.SelectedValue), common.myInt(ddlServices.SelectedValue), common.myInt(Session["UserID"]), 0);
                            lblMessage.Text = hsh["@chvErrorOutPut"].ToString();
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                            PopulateFieldData();
                        }
                        btnSave.Visible = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            btnSave.Visible = true;
        }
        finally
        {
            objclsEMR = null;
            strXML = null;
            coll = null;
            hsh = null;
        }
    }

    protected void gvFields_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFields.PageIndex = e.NewPageIndex;
        PopulateFields();
    }

    protected void ibtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        BaseC.clsEMR objclsEMR = new BaseC.clsEMR(sConString);
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        Hashtable hsh;
        try
        {
            ImageButton ibtn = (ImageButton)sender;
            GridViewRow gvr = (GridViewRow)ibtn.NamingContainer;
            HiddenField hdnLabResultSetupId = (HiddenField)gvr.FindControl("hdnLabResultSetupId");


            if (common.myInt(hdnLabResultSetupId.Value) > 0)
            {
                hsh = objclsEMR.SaveEMROutsideLabResultSetup(common.myInt(Session["FacilityId"]), strXML.ToString(), common.myInt(0), common.myInt(0), common.myInt(Session["UserID"]), common.myInt(hdnLabResultSetupId.Value));
                lblMessage.Text = hsh["@chvErrorOutPut"].ToString();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                PopulateFieldData();
                PopulateFields();
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
            objclsEMR = null;
            strXML = null;
            coll = null;
            hsh = null;
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void gvFieldData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFieldData.PageIndex = e.NewPageIndex;
        PopulateFieldData();
    }

}