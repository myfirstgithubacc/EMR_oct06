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
using System.Text;

public partial class EMR_Immunization_DailyInjection : System.Web.UI.Page
{
    #region Page level variable declration section

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData Parse = new BaseC.ParseData();
    BaseC.EMRImmunization objbc;
    DataSet ds;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["MASTER"]) == "NO")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["encounterid"] == null)
        {
            Response.Redirect("/Default.aspx?RegNo=0", false);
        }
        if (!IsPostBack)
        {
            RadGivenDatetime.Enabled = true;
            RadComboBox1.Enabled = true;
            RadGivenDatetime.SelectedDate = DateTime.Now;
            RadComboBox1.Items.Clear();

            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                {
                    RadComboBox1.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    RadComboBox1.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
            }
            int iMinute = DateTime.Now.Minute;
            RadComboBoxItem rcbItem = (RadComboBoxItem)RadComboBox1.Items.FindItemByText(iMinute.ToString());
            if (rcbItem != null)
            {
                rcbItem.Selected = true;
            }
            //---------------------------------------------------------------------------------------
            //RadExpiryDate.Enabled = true;
            RadComboBox3.Enabled = true;
            //RadExpiryDate.SelectedDate = DateTime.Now;
            RadComboBox3.Items.Clear();

            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                {
                    RadComboBox3.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    RadComboBox3.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
            }
            int iexMinute = DateTime.Now.Minute;
            RadComboBoxItem rcbItemEx = (RadComboBoxItem)RadComboBox3.Items.FindItemByText(iexMinute.ToString());
            if (rcbItemEx != null)
            {
                rcbItemEx.Selected = true;
            }

            BindImmunization();
            if (common.myStr(Session["IsMedicalAlert"]) == "")
            {
                lnkAlerts.Enabled = false;
                lnkAlerts.CssClass = "blinkNone";
                lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
            }
            else if (common.myStr(Session["IsMedicalAlert"]).ToUpper() == "YES")
            {
                lnkAlerts.Enabled = true;
                lnkAlerts.Font.Bold = true;
                lnkAlerts.Font.Size = 11;
                lnkAlerts.CssClass = "blink";
                lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
            }
            RadGivenDatetime.MaxDate = Convert.ToDateTime(DateTime.Now.AddMinutes(60));
            BindGrid();
            BindProvider();
        }
    }
    protected void BindBrand()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshInput = new Hashtable();
        hshInput.Add("@ImmunizationId", common.myInt(ddlImmunizationName.SelectedValue));
        //DataSet objDs = dl.FillDataSet(CommandType.Text, "select i.BrandId, b.ItemBrandName from EMRImmunizationBrands i with (nolock) inner join phritembrandmaster b with (nolock) on i.BrandId = b.ItemBrandId where i.immunizationId = @ImmunizationId and i.active = 1 and b.active = 1 ", hshInput);
        DataSet objDs = dl.FillDataSet(CommandType.Text, "SELECT DISTINCT b.ItemBrandId BrandId, b.ItemBrandName from PhrItemMaster i with (nolock) inner join PhrItemBrandMaster b with (nolock) on i.ItemBrandId = b.ItemBrandId where i.active = 1 and b.active = 1 and i.ItemId = @ImmunizationId", hshInput);
        ddlBrand.Items.Clear();
        if (objDs.Tables.Count > 0)
        {
            if (objDs.Tables[0].Rows.Count > 0)
            {
                ddlBrand.DataSource = objDs;
                ddlBrand.DataValueField = "BrandId";
                ddlBrand.DataTextField = "ItemBrandName";
                ddlBrand.DataBind();

            }
        }
        //  ddlBrand.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        if (common.myStr(Request.QueryString["Bid"]) != "")
            ddlBrand.SelectedValue = common.myStr(Request.QueryString["Bid"]);
        else
            ddlBrand.SelectedIndex = 0;
    }
    protected void BindProvider()
    {
        try
        {
            DataSet ds = new DataSet();
            BaseC.EMRImmunization immu = new BaseC.EMRImmunization(sConString);
            ds = immu.GetImmunizationGivenBy(Convert.ToInt16(Session["HospitalLocationId"]));
            ddlProviders.Items.Clear();
            ddlProviders.DataSource = ds.Tables[0];
            ddlProviders.DataValueField = "NurseId";
            ddlProviders.DataTextField = "NurseName";
            ddlProviders.DataBind();

            ddlProviders.SelectedIndex = ddlProviders.Items.IndexOf(ddlProviders.Items.FindItemByValue(common.myStr(Session["UserId"])));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void BindImmunization()
    {
        try
        {
            objbc = new BaseC.EMRImmunization(sConString);
            ds = new DataSet();
            // ds = objbc.GetImmunisation(common.myInt(Session["HospitalLocationId"]));
            ddlImmunizationName.Items.Clear();
            ds = objbc.getInjectionWithStock(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0, common.myInt(Session["UserId"]), "", "INJECT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlImmunizationName.DataSource = ds;
                ddlImmunizationName.DataTextField = "ItemName";
                ddlImmunizationName.DataValueField = "ItemId";
                ddlImmunizationName.DataBind();
                //   ddlImmunizationName.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlImmunizationName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindBrand();
        getVaccinationStockDetails();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "&nbsp;";


        Clear();
        BindGrid();

        Label2.Visible = false;
        txtCancelremarks.Visible = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            objbc = new BaseC.EMRImmunization(sConString);
            string strsave = "";
            if (common.myInt(ddlImmunizationName.SelectedValue) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select Injection !";
                return;
            }

            if (common.myInt(ddlProviders.SelectedValue) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select Given By !";
                return;
            }

            if (common.myStr(RadGivenDatetime.SelectedDate) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Enter Date Time !";
                return;
            }
            if (btnSave.Text == "Save")
            {
                strsave = objbc.SaveUpdatePatientDailyInjections(0, common.myInt(Session["HospitalLocationId"]), common.myInt(ddlImmunizationName.SelectedValue), common.myInt(Session["RegistrationId"]),
                    common.myInt(Session["Encounterid"]), common.myStr(RadGivenDatetime.SelectedDate), common.myInt(ddlBrand.SelectedValue), common.myStr(txtBatchNo.Text).ToUpper(),
                    common.myInt(ddlProviders.SelectedValue), common.myInt(Session["FacilityId"]), common.myStr(txtqty.Text),
                    common.myStr(txtComments.Text), "", common.myInt(Session["UserId"]),common.myStr(RadExpiryDate.SelectedDate));
                if (strsave.Contains("Save"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strsave;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = strsave;
                }

            }
            else
            {
                strsave = objbc.SaveUpdatePatientDailyInjections(common.myInt(ViewState["Id"]), common.myInt(Session["HospitalLocationId"]), common.myInt(ddlImmunizationName.SelectedValue), common.myInt(Session["RegistrationId"]),
                                  common.myInt(Session["Encounterid"]), common.myStr(RadGivenDatetime.SelectedDate), common.myInt(ddlBrand.SelectedValue), common.myStr(txtBatchNo.Text).ToUpper(),
                                  common.myInt(ddlProviders.SelectedValue), common.myInt(Session["FacilityId"]), common.myStr(txtqty.Text), common.myStr(txtComments.Text), "", common.myInt(Session["UserId"]), common.myStr(RadExpiryDate.SelectedDate));
            }
            if (strsave.Contains("Update"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strsave;
            }

            Clear();
            BindGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            sb.Append(RadGivenDatetime.SelectedDate.Value.ToString());
            sb.Remove(RadGivenDatetime.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
            sb.Insert(RadGivenDatetime.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
            RadGivenDatetime.SelectedDate = Convert.ToDateTime(sb.ToString());
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            sb = null;
        }
    }
    protected void gvDue_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            objbc = new BaseC.EMRImmunization(sConString);
            if (e.CommandName == "DeActivate")
            {
                Label2.Visible = true;
                txtCancelremarks.Visible = true;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                if (txtCancelremarks.Text != "")
                {
                    int Id = common.myInt(((HiddenField)row.FindControl("hdnId")).Value);

                    int i = objbc.DeteDailyImmunisation(Id, common.myStr(txtCancelremarks.Text));
                    if (i == 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblMessage.Text = "Delete !";
                        txtCancelremarks.Text = "";
                        Label2.Visible = false;
                        txtCancelremarks.Visible = false;
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please Enter Cancel Remarks !";
                }
                BindGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvDue_OnDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
    void BindGrid()
    {
        try
        {
            objbc = new BaseC.EMRImmunization(sConString);
            ds = new DataSet();

            ds = objbc.EMRGetPatientDailyInjections(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]));

            if (ds.Tables[0].Rows.Count == 0)
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                ds.Tables[0].AcceptChanges();
            }
            gvDue.DataSource = ds;
            gvDue.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDue_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (gvDue != null)
            {
                if (gvDue.SelectedIndex > -1)
                {
                    Label lblGivenDate = (Label)gvDue.SelectedRow.Cells[0].FindControl("lblGivenDate");
                    Label lblExpiryDate = (Label)gvDue.SelectedRow.Cells[0].FindControl("lblExpiryDate");
                    HiddenField hdnId = (HiddenField)gvDue.SelectedRow.Cells[Convert.ToByte(0)].FindControl("hdnId");
                    HiddenField hdnImmunizationId = (HiddenField)gvDue.SelectedRow.Cells[Convert.ToByte(1)].FindControl("hdnImmunizationId");
                    HiddenField hdnGivenBy = (HiddenField)gvDue.SelectedRow.Cells[Convert.ToByte(2)].FindControl("hdnGivenBy");
                    Label lblBatchno = (Label)gvDue.SelectedRow.Cells[Convert.ToByte(3)].FindControl("lblBatchno");
                    Label lblQtyGiven = (Label)gvDue.SelectedRow.Cells[Convert.ToByte(4)].FindControl("lblQtyGiven");
                    TextBox txtRemarks = (TextBox)gvDue.SelectedRow.Cells[5].FindControl("txtRemarks");
                    HiddenField hdnBrandId = (HiddenField)gvDue.SelectedRow.Cells[Convert.ToByte(6)].FindControl("hdnBrandId");
                    HiddenField hdnFacilityId = (HiddenField)gvDue.SelectedRow.Cells[Convert.ToByte(7)].FindControl("hdnFacilityId");

                    ViewState["Id"] = hdnId.Value;
                    if (common.myStr(lblBatchno.Text) != "")
                    {
                        txtBatchNo.Text = lblBatchno.Text.ToUpper();
                    }

                    //  ddlBrand.SelectedIndex = ddlBrand.Items.IndexOf(ddlBrand.Items.FindItemByValue(common.myStr(hdnBrandId.Value)));
                    ddlImmunizationName.SelectedIndex = ddlImmunizationName.Items.IndexOf(ddlImmunizationName.Items.FindItemByValue(common.myStr(hdnImmunizationId.Value)));
                    if (common.myStr(lblQtyGiven.Text) != "")
                    {
                        txtqty.Text = lblQtyGiven.Text;
                    }
                    ddlProviders.SelectedIndex = ddlProviders.Items.IndexOf(ddlProviders.Items.FindItemByValue(common.myStr(hdnGivenBy.Value)));

                    if (common.myStr(txtRemarks.Text) != "")
                    {
                        txtComments.Text = txtRemarks.Text;

                    }
                    if (common.myStr(lblGivenDate.Text) != "")
                    {
                        DateTime dt = common.myDate(lblGivenDate.Text);
                        //RadGivenDatetime.DateInput.Text =common.myStr(dt.ToString("dd/MM/yyyy hh:mm"));
                        RadGivenDatetime.DateInput.DateFormat = "dd/MM/yyyy hh:mm";
                        RadGivenDatetime.SelectedDate = dt;
                    }
                    if (common.myStr(lblExpiryDate.Text) != "")
                    {
                        DateTime dtEx = common.myDate(lblExpiryDate.Text);
                        RadExpiryDate.DateInput.DateFormat = "dd/MM/yyyy hh:mm";
                        RadExpiryDate.SelectedDate = dtEx;
                    }

                    btnSave.Text = "Update";
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

    void Clear()
    {
        txtBatchNo.Text = "";
        txtqty.Text = "";

        ddlBrand.SelectedIndex = -1;
        ddlBrand.Text = "";
        ddlBrand.EmptyMessage = "Select";

        ddlImmunizationName.SelectedIndex = -1;
        ddlImmunizationName.Text = "";
        ddlImmunizationName.EmptyMessage = "Select";

        ddlProviders.SelectedIndex = -1;
        ddlProviders.Text = "";
        ddlProviders.EmptyMessage = "Select";


        txtComments.Text = "";
        txtCancelremarks.Text = "";

        btnSave.Text = "Save";

        RadGivenDatetime.Enabled = true;
        RadComboBox1.Enabled = true;
        RadGivenDatetime.SelectedDate = DateTime.Now;
        RadGivenDatetime.MaxDate = Convert.ToDateTime(DateTime.Now.AddMinutes(60));

        int iMinute = DateTime.Now.Minute;
        RadComboBoxItem rcbItem = (RadComboBoxItem)RadComboBox1.Items.FindItemByText(iMinute.ToString());
        if (rcbItem != null)
        {
            rcbItem.Selected = true;
        }


        RadExpiryDate.Enabled = true;
        RadComboBox3.Enabled = true;
        RadExpiryDate.SelectedDate = DateTime.Now;
        RadExpiryDate.MaxDate = Convert.ToDateTime(DateTime.Now.AddMinutes(60));

        int iExMinute = DateTime.Now.Minute;
        RadComboBoxItem rcbItemEx = (RadComboBoxItem)RadComboBox3.Items.FindItemByText(iExMinute.ToString());
        if (rcbItemEx != null)
        {
            rcbItemEx.Selected = true;
        }
    }
    protected void lnkAlerts_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";

            RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&PId=" + common.myStr(Session["RegistrationId"]);
            RadWindow1.Height = 600;
            RadWindow1.Width = 600;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void getVaccinationStockDetails()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();

        try
        {
            txtBatchNo.Text = string.Empty;
            RadExpiryDate.SelectedDate = null;

            if (common.myInt(ddlImmunizationName.SelectedValue) > 0)
            {
                ds = objEMR.getVaccinationStockDetails(common.myInt(Session["FacilityId"]), common.myStr(Session["OPIP"]), 0, common.myInt(ddlImmunizationName.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtBatchNo.Text = common.myStr(ds.Tables[0].Rows[0]["BatchNo"]);
                    RadExpiryDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpiryDate"]);
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            objEMR = null;
            ds.Dispose();
        }
    }


    protected void RadComboBox3_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            if (RadExpiryDate.SelectedDate != null)
            {
                sb.Append(RadExpiryDate.SelectedDate.Value.ToString());
                sb.Remove(RadExpiryDate.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
                sb.Insert(RadExpiryDate.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox3.Text);
                RadExpiryDate.SelectedDate = Convert.ToDateTime(sb.ToString());
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
            sb = null;
        }

    }
}
