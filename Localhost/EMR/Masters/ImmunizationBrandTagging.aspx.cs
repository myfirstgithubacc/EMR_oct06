using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMR_Masters_ImmunizationBrandTagging : System.Web.UI.Page
{
    string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;//connectionstring
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateImmunization();
            PopulateBrand();
        }

    }
    protected void ddlImmunization_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {

        PopulateBrand();
        lblMessage.Text = "";

    }

    private void PopulateImmunization()
    {
        try
        {
            BaseC.EMRImmunization objEMRImmunization = new BaseC.EMRImmunization(sConString);
            DataSet ds = objEMRImmunization.GetImmunizationMaster(Convert.ToInt16(Session["HospitalLocationID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].DefaultView.RowFilter = "Active = 1";
                ddlImmunization.DataSource = ds.Tables[0].DefaultView;

                ddlImmunization.DataTextField = "ImmunizationName";
                ddlImmunization.DataValueField = "ImmunizationId";
                ddlImmunization.DataBind();
                //ddlImmunization.SelectedIndex = 0;
                ddlImmunization.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
                //ddlImmunization.SelectedIndex = 0;
                string i = ddlImmunization.SelectedValue;
            }
            //ddlImmunization.Items.Insert(0, new ListItem("[Select]", "0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    private void PopulateBrand()
    {

        BaseC.EMRImmunization objEMRImmunization = new BaseC.EMRImmunization(sConString);
        DataSet dsBrand;
        try
        {
            dsBrand = objEMRImmunization.GetEMRImmuBrand(common.myInt(ddlImmunization.SelectedValue), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));

            gvBrand.DataSource = dsBrand.Tables[0];
            gvBrand.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMRImmunization = null;
            dsBrand = null;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseC.EMRImmunization objEMRImmunization = new BaseC.EMRImmunization(sConString);
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        Hashtable hsh;
        try
        {
            if (common.myInt(ddlImmunization.SelectedValue) > 0)
            {
                if (gvBrand.Rows != null)
                {
                    if (gvBrand.Rows.Count > 0)
                    {

                        foreach (GridViewRow row in gvBrand.Rows)
                        {
                            HiddenField hdnItemBrandID = (HiddenField)row.FindControl("hdnItemBrandID");
                            CheckBox cbSelect = (CheckBox)row.FindControl("cbSelect");
                            if (cbSelect.Checked)
                            {
                                coll.Add(common.myInt(hdnItemBrandID.Value));//ItemBrandId INT,
                                strXML.Append(common.setXmlTable(ref coll));
                            }
                        }

                        if (strXML.ToString() != "")
                        {
                            hsh = objEMRImmunization.SaveEMRImmunizationBrands(common.myInt(ddlImmunization.SelectedValue), strXML.ToString(), common.myInt(Session["UserID"]), common.myInt(0));
                            lblMessage.Text = hsh["@chvErrorOutPut"].ToString();
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                            PopulateBrand();
                        }

                    }
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
            objEMRImmunization = null;
            strXML = null;
            coll = null;
        }
    }

    protected void gvBrand_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBrand.PageIndex = e.NewPageIndex;
        PopulateBrand();
    }

    protected void ibtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        BaseC.EMRImmunization objclsEMRImmunization = new BaseC.EMRImmunization(sConString);
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        Hashtable hsh;
        try
        {
            ImageButton ibtn = (ImageButton)sender;
            GridViewRow gvr = (GridViewRow)ibtn.NamingContainer;
            HiddenField hdnImmuBrandId = (HiddenField)gvr.FindControl("hdnImmuBrandId");


            if (common.myInt(hdnImmuBrandId.Value) > 0)
            {
                hsh = objclsEMRImmunization.SaveEMRImmunizationBrands(common.myInt(ddlImmunization.SelectedValue), strXML.ToString(), common.myInt(Session["UserID"]), common.myInt(hdnImmuBrandId.Value));
                lblMessage.Text = hsh["@chvErrorOutPut"].ToString();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                PopulateBrand();
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
            objclsEMRImmunization = null;
            strXML = null;
            coll = null;
            hsh = null;
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void gvBrand_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox ckbSelect = (CheckBox)e.Row.FindControl("cbSelect");
            ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");
            if (ckbSelect.Checked)
            {
                ibtnDelete.Visible = true;
            }
            else
            {
                ibtnDelete.Visible = false;
            }
        }
    }
}