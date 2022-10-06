using System;
using System.Data;
using System.Collections;
using System.Configuration;

public partial class EMR_DescriptionFormTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    int count = 0;
    string path = string.Empty;


    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownBind();
            BindGrid();
        }

    }

    protected void DropDownBind()
    {
        try
        {
            BaseC.ClsForm objEMR = new BaseC.ClsForm(sConString);
            //   DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = objEMR.getFormTagging(); //(DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "Usp_DescriptionFormtagging");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlRoute.DataSource = ds.Tables[0];
                ddlRoute.DataTextField = "RouteName";
                ddlRoute.DataValueField = "ID";
                ddlRoute.DataBind();
                ddlRoute.Items.Insert(0, " Select Route");
            }
            else
            {
                ddlRoute.Items.Clear();
                ddlRoute.Items.Insert(0, " Select Route");
            }
            if (ds.Tables[1].Rows.Count > 0)
            {

                ddlUnit.DataSource = ds.Tables[1];
                ddlUnit.DataTextField = "UnitName";
                ddlUnit.DataValueField = "ID";
                ddlUnit.DataBind();
                ddlUnit.Items.Insert(0, " Select Dose Unit");
            }
            else
            {
                ddlUnit.Items.Clear();
                ddlUnit.Items.Insert(0, " Select Dose Unit");
            }
            if (ds.Tables[2].Rows.Count > 2)
            {

                ddlForm.DataSource = ds.Tables[2];
                ddlForm.DataTextField = "FormulationName";
                ddlForm.DataValueField = "FormulationId";
                ddlForm.DataBind();
                ddlForm.Items.Insert(0, " Select Form");
            }
            else
            {
                ddlForm.Items.Clear();
                ddlForm.Items.Insert(0, " Select Form");
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindGrid()
    {
        DataSet DsSelect = new DataSet();
        BaseC.ClsForm objEMR = new BaseC.ClsForm(sConString);
        DsSelect = objEMR.Select_FormTagging();
        if (DsSelect.Tables[0].Rows.Count > 0)
        {
            GridViewData.DataSource = DsSelect.Tables[0];
            GridViewData.DataBind();
        }
        else
        {
            GridViewData.DataSource = null;
            GridViewData.DataBind();
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.ClsForm objEMR = new BaseC.ClsForm(sConString);
            byte Default = 0;
            int k;
            if (ddlForm.SelectedIndex == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Form";
                return;
            }
            if (ddlRoute.SelectedIndex == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Route";
                return;
            }
          
            if (txtDoes.Text.ToString().Trim().Length <= 0)
            {

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Enter Dose";
                return;
            }
            if (ddlUnit.SelectedIndex == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Dose Unit";
                return;
            }
            if (Defaultcheck.Checked == true)
            {
                Default = 1;
            }
            string Encodedby = common.myStr(Session["UserId"]).ToString();
            if (btnsave.Text == "Save")
            {
                DataSet Ds_Dulicate = new DataSet();
                Ds_Dulicate = objEMR.DuplicateFormTagging(Convert.ToInt16(ddlForm.SelectedValue), Convert.ToInt16(ddlRoute.SelectedValue));
                if (Ds_Dulicate.Tables[0].Rows[0]["DulicateCount"].ToString() != "0")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Duplicate Value Not Allowed";
                    return;
                }
                objEMR.InsertFormTaggingData
                (Convert.ToInt16(ddlForm.SelectedValue)
                , Convert.ToInt16(ddlRoute.SelectedValue)
                , Convert.ToInt16(ddlUnit.SelectedValue)
                , Convert.ToDecimal(txtDoes.Text), Default, Encodedby);
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Record Saved";
            }
            else
            {
                
                int Editid = 0;
                if(ViewState["ID"]!=null)
                {
                    Editid = Convert.ToInt32(ViewState["ID"]);
                }
                objEMR.UpdateFormTaggingData(Editid, Convert.ToInt16(ddlForm.SelectedValue)
                , Convert.ToInt16(ddlRoute.SelectedValue)
                , Convert.ToInt16(ddlUnit.SelectedValue)
                , Convert.ToDecimal(txtDoes.Text), Default, Encodedby);
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Record Updated";


            }
            BindGrid();
            Reset_Field(); ;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GridViewData_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            BaseC.ClsForm objEMR = new BaseC.ClsForm(sConString);
            int id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditRow")
            {
               
                ViewState["ID"] = id;
                btnsave.Text = "Update";
             
                DataSet dsEdit = new DataSet();
                dsEdit = objEMR.Edit_FormTagging(id);
                if (dsEdit.Tables[0].Rows.Count > 0)
                {
                    ddlForm.SelectedValue = dsEdit.Tables[0].Rows[0]["FormId"].ToString();
                    ddlRoute.SelectedValue = dsEdit.Tables[0].Rows[0]["RouteId"].ToString();
                    ddlUnit.SelectedValue = dsEdit.Tables[0].Rows[0]["DoseUnit"].ToString();
                    txtDoes.Text = dsEdit.Tables[0].Rows[0]["Dose"].ToString();
                    bool checkDefault = Convert.ToBoolean(dsEdit.Tables[0].Rows[0]["IsDefault"].ToString());
                    Defaultcheck.Checked = false;
                    ddlForm.Enabled = false;
                    ddlRoute.Enabled = false;

                    if (checkDefault == true)
                    {
                        Defaultcheck.Checked = true;
                    }
                }

                else
                {
                    Reset_Field();
                }
            }
            else
            {
                if (e.CommandName == "Del")
                {
                    objEMR.InactiveFormTaggingData(id);
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Record InActive Successfully";
                    BindGrid();
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
  protected void Reset_Field()
    {
        ddlForm.SelectedIndex = 0;
        ddlRoute.SelectedIndex = 0;
        ddlUnit.SelectedIndex = 0;
        txtDoes.Text = "";
        Defaultcheck.Checked = false;
        ddlForm.Enabled = true;
        ddlRoute.Enabled = true;
        btnsave.Text = "Save";
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlForm.SelectedIndex = 0;
        ddlRoute.SelectedIndex = 0;
        ddlUnit.SelectedIndex = 0;
        txtDoes.Text = "";
        Defaultcheck.Checked = false;
        btnsave.Text = "Save";
        ddlForm.Enabled = true;
        ddlRoute.Enabled = true;


    }


}
