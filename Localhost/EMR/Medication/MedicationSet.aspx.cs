using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class EMR_Medication_MedicationSet : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            }

            btnSave.Visible = false;
            txtMedicationSet.Visible = false;
            bindMedicationSet();
            BindFrequency();
            BindMedication();
            BindDoseunits();
            bindMedicationSet();
            BindFormulation();
            BindRoute();
            BindFoodRelation();
        }
    }
    private void BindFrequency()
    {
        DataSet ds = new DataSet();
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

        try
        {
            if (Cache["FrequencyData"] == null)
            {
                ds = objPharmacy.getFrequencyMaster();
                Cache.Insert("FrequencyData", ds, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                DataSet dsResult = (DataSet)Cache["FrequencyData"];
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    ds = (DataSet)Cache["FrequencyData"];
                }
                else
                {
                    ds = objPharmacy.getFrequencyMaster();
                    Cache.Insert("FrequencyData", ds, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }

            ddlFrequency.DataSource = ds.Tables[0];
            ddlFrequency.DataTextField = "Description";
            ddlFrequency.DataValueField = "Id";
            ddlFrequency.DataBind();

            ddlFrequency.Items.Insert(0, new RadComboBoxItem(string.Empty, string.Empty));
            ddlFrequency.SelectedIndex = 0;
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
            objPharmacy = null;
        }
    }
    protected void bindMedicationSet()
    {
        BaseC.EMRMasters objEMR = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objEMR.GetEMRDrugSet(common.myInt(Session["HospitalLocationID"]), 0);

            ddlMedicationSet.DataSource = ds.Tables[0];
            ddlMedicationSet.DataTextField = "SetName";
            ddlMedicationSet.DataValueField = "SetId";
            ddlMedicationSet.DataBind();
            ddlMedicationSet.SelectedIndex = ddlMedicationSet.Items.IndexOf(ddlMedicationSet.Items.FindItemByValue(common.myStr(ViewState["ddlMedicationSetSelectedValue"])));
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
            objEMR = null;
        }
    }
    void BindDoseunits()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objEMR.GetUnitMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            ddlDoseUnits.DataSource = ds.Tables[0];
            ddlDoseUnits.DataValueField = "Id";
            ddlDoseUnits.DataTextField = "UnitName";
            ddlDoseUnits.DataBind();
            ddlDoseUnits.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlDoseUnits.SelectedIndex = 0;
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
            objEMR = null;
        }
    }
    protected void ddlMedicationSet_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindMedication();
    }
    protected void btnAddToList_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlMedicationSet.SelectedValue).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select Medication Set", Page);
                return;
            }
            if (common.myInt(ddlGeneric.SelectedValue).Equals(0) && common.myInt(ddlDrugs.SelectedValue).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select generic/drug", Page);
                return;
            }
            if (common.myDbl(txtDoseDtl.Text).Equals(0))
            {
                Alert.ShowAjaxMsg("Please enter dose", Page);
                txtDoseDtl.Focus();
                return;
            }
            if (common.myInt(ddlDoseUnits.SelectedValue).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select dose unit", Page);
                ddlDoseUnits.Focus();
                return;
            }
            if (common.myInt(ddlFrequency.SelectedValue).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select frequency", Page);
                return;
            }
            if (common.myInt(txtDuration.Text).Equals(0))
            {
                Alert.ShowAjaxMsg("Please enter duration", Page);
                txtDuration.Focus();
                return;
            }
            if (common.myStr(ddlPeriodType.SelectedValue).Equals(string.Empty))
            {
                Alert.ShowAjaxMsg("Please select duration type", Page);
                ddlDoseUnits.Focus();
                return;
            }
            //if (common.myInt(ddlFormulation.SelectedValue).Equals(0))
            //{
            //    Alert.ShowAjaxMsg("Please select formulation", Page);
            //    return;
            //}
            //if (common.myInt(ddlRoute.SelectedValue).Equals(0))
            //{
            //    Alert.ShowAjaxMsg("Please select route", Page);
            //    return;
            //}

            DataTable dt = new DataTable();
            if (ViewState["Medication"] == null)
            {
                dt = CreateTable();
                DataRow Dr = dt.NewRow();

                Dr["SetID"] = common.myInt(ddlMedicationSet.SelectedValue);

                if (common.myInt(ddlDrugs.SelectedValue) > 0)
                {
                    Dr["GenericId"] = "0";
                    Dr["GenericName"] = string.Empty;

                    Dr["ItemID"] = common.myInt(ddlDrugs.SelectedValue);
                    Dr["ItemName"] = common.myStr(ddlDrugs.Text);
                }
                else
                {
                    Dr["GenericId"] = common.myInt(ddlGeneric.SelectedValue);
                    Dr["GenericName"] = common.myStr(ddlGeneric.Text);

                    Dr["ItemID"] = "0";
                    Dr["ItemName"] = string.Empty;
                }

                Dr["FrequencyId"] = common.myInt(ddlFrequency.SelectedValue);
                Dr["FrequencyName"] = common.myStr(ddlFrequency.SelectedItem.Text);
                Dr["Duration"] = txtDuration.Text;
                Dr["DTypeId"] = common.myStr(ddlPeriodType.SelectedValue);
                Dr["DType"] = common.myStr(ddlPeriodType.SelectedItem.Text);
                Dr["ExistItem"] = 0;
                Dr["Dose"] = txtDoseDtl.Text;
                Dr["DoseUnit"] = common.myStr(ddlDoseUnits.SelectedItem.Text);
                Dr["DoseUnitID"] = common.myInt(ddlDoseUnits.SelectedValue);
                Dr["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
                Dr["RouteId"] = common.myInt(ddlRoute.SelectedValue);
                Dr["FormulationName"] = common.myStr(ddlFormulation.Text);
                Dr["RouteName"] = common.myStr(ddlRoute.Text);
                Dr["FoodID"] = common.myInt(ddlfoodrelationship.SelectedValue);
                Dr["FoodName"] = common.myStr(ddlfoodrelationship.Text);
                Dr["InstructionRemarks"] = common.myStr(txtInstructions.Text);

                dt.Rows.Add(Dr);
                ViewState["Medication"] = dt;
            }
            else
            {
                DataTable dtExist = (DataTable)ViewState["Medication"];

                if (dtExist.Rows.Count > 0)
                {
                    BaseC.clsEMR clsEMR = new BaseC.clsEMR(sConString);

                    if (common.myInt(ViewState["ItemId1"]) > 0 || common.myInt(ViewState["GenericId1"]) > 0)
                    {
                        DataView DV = dtExist.Copy().DefaultView;
                        if (common.myInt(ViewState["ItemId1"]) > 0)
                        {
                            DV.RowFilter = "ItemId=" + common.myStr(ViewState["ItemId1"]);
                        }
                        if (common.myInt(ViewState["GenericId1"]) > 0)
                        {
                            DV.RowFilter = "GenericId=" + common.myStr(ViewState["GenericId1"]);
                        }

                        //dtExist = DV.ToTable();
                        if (DV.Count > 0)
                        {
                            DataRow dr;
                            if (common.myInt(ViewState["ItemId1"]) > 0)
                            {
                                dr = dtExist.AsEnumerable().Where(r => ((Int32)r["SetID"]).Equals(common.myInt(ViewState["SetId1"])) && ((Int32)r["ItemID"]).Equals(common.myInt(ViewState["ItemId1"]))).First();
                            }
                            else
                            {
                                dr = dtExist.AsEnumerable().Where(r => ((Int32)r["SetID"]).Equals(common.myInt(ViewState["SetId1"])) && ((Int32)r["GenericId"]).Equals(common.myInt(ViewState["GenericId1"]))).First();
                            }

                            dr["SetID"] = common.myInt(ddlMedicationSet.SelectedValue);

                            if (common.myInt(ddlDrugs.SelectedValue) > 0)
                            {
                                dr["GenericId"] = "0";
                                dr["GenericName"] = string.Empty;

                                dr["ItemID"] = common.myInt(ddlDrugs.SelectedValue);
                                dr["ItemName"] = common.myStr(ddlDrugs.Text);
                            }
                            else
                            {
                                dr["GenericId"] = common.myInt(ddlGeneric.SelectedValue);
                                dr["GenericName"] = common.myStr(ddlGeneric.Text);

                                dr["ItemID"] = "0";
                                dr["ItemName"] = string.Empty;
                            }

                            dr["FrequencyId"] = common.myInt(ddlFrequency.SelectedValue);
                            dr["FrequencyName"] = common.myStr(ddlFrequency.SelectedItem.Text);
                            dr["Duration"] = txtDuration.Text;
                            dr["DTypeId"] = common.myStr(ddlPeriodType.SelectedValue);
                            dr["DType"] = common.myStr(ddlPeriodType.SelectedItem.Text);
                            dr["ExistItem"] = 0;
                            dr["Dose"] = txtDoseDtl.Text;
                            dr["DoseUnit"] = common.myStr(ddlDoseUnits.SelectedItem.Text);
                            dr["DoseUnitID"] = common.myInt(ddlDoseUnits.SelectedValue);
                            dr["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
                            dr["RouteId"] = common.myInt(ddlRoute.SelectedValue);
                            dr["FormulationName"] = common.myStr(ddlFormulation.Text);
                            dr["RouteName"] = common.myStr(ddlRoute.Text);
                            dr["FoodID"] = common.myInt(ddlfoodrelationship.SelectedValue);
                            dr["FoodName"] = common.myStr(ddlfoodrelationship.Text);
                            dr["InstructionRemarks"] = common.myStr(txtInstructions.Text);

                            dtExist.AcceptChanges();
                            ViewState["SetId1"] = null;
                            ViewState["ItemId1"] = null;
                            ViewState["GenericId1"] = null;
                        }
                    }
                    else
                    {
                        DataView DV = dtExist.Copy().DefaultView;
                        if (common.myInt(ddlDrugs.SelectedValue) > 0)
                        {
                            DV.RowFilter = "ItemId = " + common.myStr(ddlDrugs.SelectedValue);
                        }
                        else
                        {
                            DV.RowFilter = "GenericId = " + common.myStr(ddlGeneric.SelectedValue);
                        }

                        if (DV.Count > 0)
                        {
                            Alert.ShowAjaxMsg("Generic or Drug Name is already Added.", Page);
                            return;
                        }
                        else
                        {
                            DataRow Dr = dtExist.NewRow();
                            Dr["SetID"] = common.myInt(ddlMedicationSet.SelectedValue);

                            if (common.myInt(ddlDrugs.SelectedValue) > 0)
                            {
                                Dr["GenericId"] = "0";
                                Dr["GenericName"] = string.Empty;

                                Dr["ItemID"] = common.myInt(ddlDrugs.SelectedValue);
                                Dr["ItemName"] = common.myStr(ddlDrugs.Text);
                            }
                            else
                            {
                                Dr["GenericId"] = common.myInt(ddlGeneric.SelectedValue);
                                Dr["GenericName"] = common.myStr(ddlGeneric.Text);

                                Dr["ItemID"] = "0";
                                Dr["ItemName"] = string.Empty;
                            }

                            Dr["FrequencyId"] = common.myInt(ddlFrequency.SelectedValue);
                            Dr["FrequencyName"] = common.myStr(ddlFrequency.SelectedItem.Text);
                            Dr["Duration"] = txtDuration.Text;
                            Dr["DTypeId"] = common.myStr(ddlPeriodType.SelectedValue);
                            Dr["DType"] = common.myStr(ddlPeriodType.SelectedItem.Text);
                            Dr["ExistItem"] = 0;
                            Dr["Dose"] = txtDoseDtl.Text;
                            Dr["DoseUnit"] = common.myStr(ddlDoseUnits.SelectedItem.Text);
                            Dr["DoseUnitID"] = common.myInt(ddlDoseUnits.SelectedValue);
                            Dr["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
                            Dr["RouteId"] = common.myInt(ddlRoute.SelectedValue);
                            Dr["FormulationName"] = common.myStr(ddlFormulation.Text);
                            Dr["RouteName"] = common.myStr(ddlRoute.Text);
                            Dr["FoodID"] = common.myInt(ddlfoodrelationship.SelectedValue);
                            Dr["FoodName"] = common.myStr(ddlfoodrelationship.Text);
                            Dr["InstructionRemarks"] = common.myStr(txtInstructions.Text);

                            dtExist.Rows.Add(Dr);
                        }
                    }
                }

                ViewState["Medication"] = dtExist;
            }
            gvMedication.DataSource = (DataTable)ViewState["Medication"];
            gvMedication.DataBind();
            ClearControl();
            ViewState["SetId1"] = null;
            ViewState["ItemId1"] = null;
            ViewState["GenericId1"] = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void ClearControl()
    {
        try
        {
            hdnGenericId.Value = string.Empty;
            ddlGeneric.Text = string.Empty;
            ddlGeneric.SelectedIndex = 0;

            ddlDrugs.Text = string.Empty;
            ddlDrugs.SelectedIndex = 0;

            txtDoseDtl.Text = string.Empty;
            ddlDoseUnits.SelectedIndex = 0;

            ddlFrequency.SelectedIndex = 0;
            ddlFrequency.Text = string.Empty;

            txtDuration.Text = string.Empty;
            ddlPeriodType.SelectedIndex = 0;

            ddlFormulation.SelectedIndex = 0;
            ddlRoute.SelectedIndex = 0;

            ddlfoodrelationship.SelectedIndex = 0;
            txtInstructions.Text = string.Empty;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        try
        {
            txtMedicationSet.Text = "";
            txtMedicationSet.Visible = true;
            //btnNew.Visible = false;
            btnSave.Visible = true;
            //btnUndo.Visible = true;
            //btnDelete.Visible = false;
            ddlMedicationSet.Visible = false;
            ddlMedicationSet.SelectedValue = "0";
            if (ViewState["Medication"] != null)
            {
                gvMedication.DataSource = (DataTable)ViewState["Medication"];
                gvMedication.DataBind();
            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

            if (txtMedicationSet.Text != "")
            {
                Hashtable hshOutPut = new Hashtable();
                hshOutPut = objEMR.SaveMedicationSet(Convert.ToInt32(0), txtMedicationSet.Text, Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["UserId"]));
                lblMessage.Text = hshOutPut["@chvErrorOutPut"].ToString();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindMedicationSet();
                txtMedicationSet.Visible = false;
                ddlMedicationSet.Visible = true;

            }
            else
            {
                Alert.ShowAjaxMsg("please enter Set Name.", this.Page);
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

    private void BindMedication()
    {
        try
        {
            ViewState["Medication"] = null;
            if (ddlMedicationSet.SelectedValue != "")
            {
                pnlMedicationOptions.Visible = true;
                BaseC.EMRMasters objEMR = new BaseC.EMRMasters(sConString);
                DataSet ds = objEMR.GetEMRDrugSetDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlMedicationSet.SelectedValue), 0);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["Medication"] = ds.Tables[0];

                    gvMedication.DataSource = ds.Tables[0];
                    gvMedication.DataBind();
                }
                else
                {
                    BindBlankGrid();
                    ddlMedicationSet.Visible = true;

                    btnSave.Visible = false;
                    txtMedicationSet.Visible = false;
                }
            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private const int ItemsPerRequest = 50;
    protected void ddlDrugs_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            int GenericId = 0;
            if (common.myStr(ddlGeneric.Text).Length > 0)
            {
                string selectedValue = common.myStr(e.Context["GenericId"]);
                if (common.myInt(selectedValue) > 0)
                {
                    GenericId = common.myInt(selectedValue);
                }
            }
            //RadComboBox ddl = sender as RadComboBox;
            if (!common.myStr(hdnDrugName.Value).Equals(string.Empty))
            {
                e.Text = hdnDrugName.Value;
                hdnDrugName.Value = null;
            }
            DataTable data = new DataTable();
            data = GetBrandData(e.Text, GenericId);
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ItemName"];
                item.Value = common.myStr(data.Rows[i]["ItemId"]);
                item.Attributes.Add("ClosingBalance", common.myStr(data.Rows[i]["ClosingBalance"]));

                this.ddlDrugs.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches found...";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    private DataTable GetBrandData(string text, int GenericId)
    {
        DataSet dsSearch = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

            int StoreId = 0; //common.myInt(hdnStoreId.Value); //common.myInt(Session["StoreId"]);
            int ItemId = 0;

            int itemBrandId = 0;
            int WithStockOnly = 0;

            if (common.myDbl(ViewState["QtyBal"]) > 0
                   && common.myInt(Request.QueryString["ItemId"]) > 0)
            {
                ItemId = common.myInt(ViewState["ItemId"]);
            }

            dsSearch = objPharmacy.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId, ItemId, itemBrandId, GenericId,
                                            common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), 0,
                                            text.Replace("'", "''"), WithStockOnly, "H");

            if (dsSearch.Tables.Count > 0)
            {
                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    dt = dsSearch.Tables[0];
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dt;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (ddlMedicationSet.Items.Count > 0)
        {
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            int i = objEMR.DeleteMedicationSet(Convert.ToInt32(ddlMedicationSet.SelectedValue), Convert.ToInt32(Session["UserId"]));
            if (i == 0)
            {
                lblMessage.Text = "Record(s) delete";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }
            bindMedicationSet();
        }
    }

    protected void btnUndo_Click(object sender, EventArgs e)
    {
        ddlMedicationSet.Visible = true;
        //btnNew.Visible = true;
        //btnDelete.Visible = true;
        btnSave.Visible = false;
        txtMedicationSet.Visible = false;
        //btnUndo.Visible = false;
    }

    #region working on Selected Service Grid
    private void BindBlankGrid()
    {
        try
        {
            DataTable dt = CreateTable();
            DataRow Dr = dt.NewRow();
            Dr["SetId"] = 0;
            Dr["ItemID"] = 0;
            Dr["ItemName"] = "";
            Dr["FrequencyId"] = 0;
            Dr["FrequencyName"] = "";
            Dr["Duration"] = "";
            Dr["DType"] = "";
            Dr["DTypeId"] = "";
            Dr["ExistItem"] = 0;
            Dr["Dose"] = "";
            Dr["DoseUnit"] = "";
            Dr["DoseUnitID"] = "";
            Dr["InstructionRemarks"] = "";
            dt.Rows.Add(Dr);
            if (ViewState["Medication"] != null)
            {
                gvMedication.DataSource = (DataTable)ViewState["Medication"];
                gvMedication.DataBind();
            }
            else
            {
                gvMedication.DataSource = dt;
                gvMedication.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected DataTable CreateTable()
    {
        DataTable Dt = new DataTable();
        Dt.Columns.Add("SetId");
        Dt.Columns.Add("ItemID");
        Dt.Columns.Add("ItemName");
        Dt.Columns.Add("FrequencyId");
        Dt.Columns.Add("FrequencyName");
        Dt.Columns.Add("Duration");
        Dt.Columns.Add("DType");
        Dt.Columns.Add("DTypeId");
        Dt.Columns.Add("ExistItem");
        Dt.Columns.Add("Dose");
        Dt.Columns.Add("DoseUnit");
        Dt.Columns.Add("DoseUnitId");
        Dt.Columns.Add("FormulationId");
        Dt.Columns.Add("RouteId");
        Dt.Columns.Add("FormulationName");
        Dt.Columns.Add("RouteName");
        Dt.Columns.Add("FoodID");
        Dt.Columns.Add("FoodName");
        Dt.Columns.Add("InstructionRemarks");
        Dt.Columns.Add("GenericId");
        Dt.Columns.Add("GenericName");
        return Dt;
    }

    protected void gvMedication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Del")
            {
                BaseC.clsEMR clsEMR = new BaseC.clsEMR(sConString);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnItemID = (HiddenField)row.FindControl("hdnItemID");
                HiddenField hdnGenericId = (HiddenField)row.FindControl("hdnGenericId");
                HiddenField hdnExistItem = (HiddenField)row.FindControl("hdnExistItem");
                HiddenField hdnSetId = (HiddenField)row.FindControl("hdnSetId");

                if (common.myBool(hdnExistItem.Value) && (common.myInt(hdnGenericId.Value) > 0 || common.myInt(hdnItemID.Value) > 0))
                {
                    int i = clsEMR.DeleteMedicationItem(common.myInt(hdnSetId.Value), common.myInt(hdnGenericId.Value), common.myInt(hdnItemID.Value), common.myInt(Session["UserId"]));
                    if (i == 0)
                    {
                        lblMessage.Text = "Record(s) delete";
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    }

                    DataView dv = new DataView();
                    if (ViewState["Medication"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["Medication"];
                        dv = new DataView(dt);
                        dv.RowFilter = "ExistItem=0";
                    }
                    pnlMedicationOptions.Visible = true;
                    BaseC.EMRMasters objEMR = new BaseC.EMRMasters(sConString);
                    DataSet ds = objEMR.GetEMRDrugSetDetail(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(ddlMedicationSet.SelectedValue), 0);
                    ViewState["Medication"] = null;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        ViewState["Medication"] = ds.Tables[0];
                        DataTable dt1 = (DataTable)ViewState["Medication"];
                        for (int j = 0; j < dv.ToTable().Rows.Count; j++)
                        {
                            DataRow Dr = dt1.NewRow();
                            Dr["SetID"] = dv.ToTable().Rows[j]["SetId"];
                            Dr["GenericId"] = dv.ToTable().Rows[j]["GenericId"];
                            Dr["GenericName"] = dv.ToTable().Rows[j]["GenericName"];
                            Dr["ItemID"] = dv.ToTable().Rows[j]["ItemID"];
                            Dr["ItemName"] = dv.ToTable().Rows[j]["ItemName"];
                            Dr["FrequencyId"] = dv.ToTable().Rows[j]["FrequencyId"];
                            Dr["FrequencyName"] = dv.ToTable().Rows[j]["FrequencyName"];
                            Dr["Duration"] = dv.ToTable().Rows[j]["Duration"];
                            Dr["DTypeId"] = dv.ToTable().Rows[j]["DTypeId"];
                            Dr["DType"] = dv.ToTable().Rows[j]["DType"];
                            Dr["ExistItem"] = 0;
                            Dr["Dose"] = dv.ToTable().Rows[j]["Dose"];
                            Dr["DoseUnit"] = dv.ToTable().Rows[j]["DoseUnit"];
                            Dr["DoseUnitId"] = dv.ToTable().Rows[j]["DoseUnitId"];
                            Dr["InstructionRemarks"] = dv.ToTable().Rows[j]["InstructionRemarks"];

                            dt1.Rows.Add(Dr);
                        }
                        if (dt1.Rows.Count > 0 && dv.ToTable().Rows.Count > 0)
                        {
                            gvMedication.DataSource = dt1;
                            gvMedication.DataBind();
                        }
                        else
                        {
                            ViewState["Medication"] = ds.Tables[0];
                            gvMedication.DataSource = ds.Tables[0];
                            gvMedication.DataBind();
                        }
                    }
                    else
                    {
                        BindMedication();
                    }
                }
                else if (!common.myBool(hdnExistItem.Value) && (common.myInt(hdnGenericId.Value) > 0 || common.myInt(hdnItemID.Value) > 0))
                {
                    DataTable dt = (DataTable)ViewState["Medication"];
                    DataView dv = new DataView(dt);
                    if (common.myInt(hdnItemID.Value) > 0)
                    {
                        dv.RowFilter = "ItemId<>" + common.myInt(hdnItemID.Value);
                    }
                    else if (common.myInt(hdnGenericId.Value) > 0)
                    {
                        dv.RowFilter = "GenericId<>" + common.myInt(hdnGenericId.Value);
                    }

                    if (dv.ToTable().Rows.Count == 0)
                    {
                        ViewState["Medication"] = null;
                        BindBlankGrid();
                    }
                    else
                    {
                        gvMedication.DataSource = dv.ToTable();
                        gvMedication.DataBind();
                        ViewState["Medication"] = dv.ToTable();
                    }
                }
            }
            if (common.myStr(e.CommandName).ToUpper().Equals("ED"))
            {
                ddlDrugs.Text = string.Empty;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                HiddenField hdnSetId = (HiddenField)row.FindControl("hdnSetId");
                HiddenField hdnItemID = (HiddenField)row.FindControl("hdnItemID");
                HiddenField hdnGenericId = (HiddenField)row.FindControl("hdnGenericId");
                HiddenField hdnFrequencyId = (HiddenField)row.FindControl("hdnFrequencyId");
                HiddenField hdnExistItem = (HiddenField)row.FindControl("hdnExistItem");
                HiddenField hdnDtypeId = (HiddenField)row.FindControl("hdnDtypeId");
                HiddenField hdnDoseUnitId = (HiddenField)row.FindControl("hdnDoseUnitId");
                HiddenField hdnFormulationId = (HiddenField)row.FindControl("hdnFormulationId");
                HiddenField hdnRouteId = (HiddenField)row.FindControl("hdnRouteId");
                HiddenField hdnFoodID = (HiddenField)row.FindControl("hdnFoodID");

                Label lblItemName = (Label)row.FindControl("lblItemName");
                Label lblGenericName = (Label)row.FindControl("lblGenericName");
                Label lblDuration = (Label)row.FindControl("lblDuration");
                Label lblDType = (Label)row.FindControl("lblDType");
                Label lblDosedtls = (Label)row.FindControl("lblDosedtls");
                Label lblInstruction = (Label)row.FindControl("lblInstructions");

                ViewState["ItemId1"] = common.myStr(hdnItemID.Value);
                ViewState["GenericId1"] = common.myStr(hdnGenericId.Value);
                ViewState["ExistItem1"] = common.myStr(hdnExistItem.Value);
                ViewState["SetId1"] = common.myStr(hdnSetId.Value);

                hdnGenericName.Value = lblGenericName.Text;
                ddlGeneric.Text = lblGenericName.Text;
                ddlGeneric.SelectedIndex = ddlGeneric.Items.IndexOf(ddlGeneric.Items.FindItemByValue(common.myStr(hdnGenericId.Value)));
                ddlGeneric.SelectedValue = common.myStr(hdnGenericId.Value);

                hdnDrugName.Value = lblItemName.Text;
                ddlDrugs.Text = lblItemName.Text;
                ddlDrugs.SelectedIndex = ddlDrugs.Items.IndexOf(ddlDrugs.Items.FindItemByValue(common.myStr(hdnItemID.Value)));
                ddlDrugs.SelectedValue = common.myStr(hdnItemID.Value);

                txtDoseDtl.Text = common.myStr(lblDosedtls.Text);
                ddlDoseUnits.SelectedIndex = ddlDoseUnits.Items.IndexOf(ddlDoseUnits.Items.FindItemByValue(common.myStr(hdnDoseUnitId.Value)));
                ddlFrequency.SelectedIndex = ddlFrequency.Items.IndexOf(ddlFrequency.Items.FindItemByValue(common.myStr(hdnFrequencyId.Value)));
                txtDuration.Text = common.myStr(lblDuration.Text);
                ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(hdnDtypeId.Value)));
                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myStr(hdnFormulationId.Value)));
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myStr(hdnRouteId.Value)));
                ddlfoodrelationship.SelectedIndex = ddlfoodrelationship.Items.IndexOf(ddlfoodrelationship.Items.FindItemByValue(common.myStr(hdnFoodID.Value)));

                txtInstructions.Text = common.myStr(lblInstruction.Text);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion

    protected void btnFormSave_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        try
        {
            if (gvMedication != null)
            {
                if (gvMedication.Rows != null)
                {
                    if (gvMedication.Rows.Count > 0 && common.myInt(ddlMedicationSet.SelectedValue) > 0)
                    {
                        txtMedicationSet.Visible = false;
                        ddlMedicationSet.Visible = true;
                        btnSave.Visible = false;
                        //btnNew.Visible = true;
                        foreach (GridViewRow row in gvMedication.Rows)
                        {
                            HiddenField hdnSetId = (HiddenField)row.FindControl("hdnSetId");
                            HiddenField hdnItemID = (HiddenField)row.FindControl("hdnItemID");
                            HiddenField hdnGenericId = (HiddenField)row.FindControl("hdnGenericId");

                            if (common.myInt(hdnItemID.Value).Equals(0) && common.myInt(hdnGenericId.Value).Equals(0))
                            {
                                lblMessage.Text = "Please add to generic or item list";
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                break;
                            }
                            if (common.myInt(hdnItemID.Value) > 0)
                            {
                                hdnGenericId.Value = "0";
                            }
                            if (common.myInt(hdnGenericId.Value) > 0)
                            {
                                hdnItemID.Value = "0";
                            }

                            HiddenField hdnFrequencyId = (HiddenField)row.FindControl("hdnFrequencyId");
                            Label lblDuration = (Label)row.FindControl("lblDuration");
                            Label lblDType = (Label)row.FindControl("lblDType");
                            HiddenField hdnDtypeId = (HiddenField)row.FindControl("hdnDtypeId");
                            Label lblDosedtls = (Label)row.FindControl("lblDosedtls");
                            HiddenField hdnDoseUnitId = (HiddenField)row.FindControl("hdnDoseUnitId");
                            HiddenField hdnFormulationId = (HiddenField)row.FindControl("hdnFormulationId");
                            HiddenField hdnRouteId = (HiddenField)row.FindControl("hdnRouteId");
                            HiddenField hdnFoodID = (HiddenField)row.FindControl("hdnFoodID");
                            Label lblInstruction = (Label)row.FindControl("lblInstructions");

                            coll.Add(common.myInt(ddlMedicationSet.SelectedValue));//SetId SMALLINT,
                            coll.Add(common.myInt(hdnItemID.Value));//ItemID INT,
                            coll.Add(common.myInt(hdnFrequencyId.Value));//FrequencyId TINYINT,
                            coll.Add(common.myInt(lblDuration.Text));//Days VARCHAR(100),
                            coll.Add(common.myStr(hdnDtypeId.Value));//DaysType CHAR(1) ,
                            coll.Add(common.myDbl(lblDosedtls.Text));//Dose decimal,
                            coll.Add(common.myStr(hdnDoseUnitId.Value));//DoseUnitId int,
                            coll.Add(common.myInt(hdnFormulationId.Value));//FormulationId int,
                            coll.Add(common.myInt(hdnRouteId.Value));//RouteId int
                            coll.Add(common.myInt(hdnFoodID.Value));//FoodID SMALLINT
                            coll.Add(common.myStr(lblInstruction.Text));//InstructionRemarks VARCHAR(1000),
                            coll.Add(common.myInt(hdnGenericId.Value));//GenericId INT

                            strXML.Append(common.setXmlTable(ref coll));
                        }
                        if (strXML.ToString() != "")
                        {
                            Hashtable hsh = objEMR.SaveMedicationItemSet(strXML.ToString(), common.myInt(ddlMedicationSet.SelectedValue), common.myInt(Session["UserID"]));
                            lblMessage.Text = hsh["@chvErrorOutPut"].ToString();
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                            BindMedication();
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
    }
    protected void BindFormulation()
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objPharmacy.GetFormulationMaster(0, common.myInt(Session["HospitalLocationID"]), 1, common.myInt(Session["UserID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = common.myStr(dr["FormulationName"]);
                    item.Value = common.myStr(common.myInt(dr["FormulationId"]));
                    item.Attributes.Add("DefaultRouteId", common.myStr(dr["DefaultRouteId"]));
                    item.DataBind();
                    ddlFormulation.Items.Add(item);
                }
                ddlFormulation.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
                ddlFormulation.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void BindRoute()
    {
        OpPrescription OpPre = new OpPrescription(sConString);
        DataSet dsRoute = new DataSet();
        try
        {
            dsRoute = (DataSet)OpPre.dsGetRouteDetails();

            ddlRoute.DataSource = dsRoute.Tables[0];
            ddlRoute.DataValueField = "Id";
            ddlRoute.DataTextField = "RouteName";
            ddlRoute.DataBind();

            ddlRoute.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlRoute.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindFoodRelation()
    {
        OpPrescription OpPre = new OpPrescription(sConString);
        DataSet dsfoodrelation = new DataSet();
        try
        {
            dsfoodrelation = (DataSet)OpPre.dsGetFoodRelationShip(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

            ddlfoodrelationship.DataSource = dsfoodrelation.Tables[0];
            ddlfoodrelationship.DataValueField = "ID";
            ddlfoodrelationship.DataTextField = "FoodName";
            ddlfoodrelationship.DataBind();

            ddlfoodrelationship.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlfoodrelationship.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void ddlFormulation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            if (ddlFormulation.SelectedValue != null && ddlFormulation.SelectedItem.Text != "")
            {
                ddlRoute.SelectedValue = ddlFormulation.SelectedItem.Attributes["DefaultRouteId"].ToString();
            }
            else
            {
                ddlRoute.Text = "";
                ddlRoute.SelectedValue = "0";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ibtnAddOrderSet_Click(object sender, ImageClickEventArgs e)
    {
        RadWindow1.NavigateUrl = "~/EMR/Orders/AddOrderSet.aspx?For=Medication";
        RadWindow1.Height = 600;
        RadWindow1.Width = 800;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;

        RadWindow1.OnClientClose = "addDiagnosisSerchOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;

        ViewState["ddlMedicationSetSelectedValue"] = common.myStr(ddlMedicationSet.SelectedValue);

    }
    protected void btnAddOrderSetClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindMedicationSet();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void ddlGeneric_OnItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            Telerik.Web.UI.RadComboBox ddl = sender as Telerik.Web.UI.RadComboBox;
            dt = GetGenericData(common.myStr(e.Text));
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 50, dt.Rows.Count);
            e.EndOfItems = endOffset.Equals(dt.Rows.Count);
            for (int i = itemOffset; i < endOffset; i++)
            {
                Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                item.Text = common.myStr(dt.Rows[i]["GenericName"]);
                item.Value = common.myStr(dt.Rows[i]["GenericId"]);
                item.Attributes.Add("CIMSItemId", common.myStr(dt.Rows[i]["CIMSItemId"]));
                item.Attributes.Add("CIMSType", common.myStr(dt.Rows[i]["CIMSType"]));
                item.Attributes.Add("VIDALItemId", common.myInt(dt.Rows[i]["VIDALItemId"]).ToString());
                ddl.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
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

    private DataTable GetGenericData(string text)
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objPhr.GetGenericDetails(0, common.myStr(text), 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPhr = null;
        }
        return ds.Tables[0];
    }

    protected void btnGetInfoGeneric_Click(object sender, EventArgs e)
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        try
        {
            lblMessage.Text = string.Empty;
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;
            //txtStrengthValue.Enabled = true;

            int StoreId = 0;
            int GenericId = common.myInt(hdnGenericId.Value);
            string CIMSItemId = common.myStr(hdnCIMSItemId.Value);
            string CIMSType = common.myStr(hdnCIMSType.Value);
            int VIDALItemId = common.myInt(hdnVIDALItemId.Value);

            int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["LoginDoctorId"]);
            }
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["EmployeeId"]);
            }

            ds = objPhr.getItemAttributes(common.myInt(Session["HospitalLocationID"]), 0, common.myInt(StoreId),
                                            common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), DoctorId, common.myInt(Session["CompanyId"]), GenericId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow DR = ds.Tables[0].Rows[0];
                ViewState["ISCalculationRequired"] = common.myBool(DR["IsCalculated"]);
            }


            if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    if (common.myLen(hdnCIMSItemId.Value) < 2)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "This Drug is not mapped with CIMS.";
                        return;
                    }
                    //chkIsInteraction(0);
                }
                else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "This Drug is not mapped with VIDAL.";
                        return;
                    }
                }
            }

            ddlDrugs.Focus();
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
            objPhr = null;
        }
    }

}
