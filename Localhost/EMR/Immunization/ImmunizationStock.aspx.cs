using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Telerik.Web.UI;
using System.Text;

public partial class EMR_Immunization_ImmunizationStock : System.Web.UI.Page
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

            //  populateImmunizationDropDown();
            // binddrugname();
            //   BindDropdown();
            fillunit();
            // BindImmunizationMain();
            ddlImmunizationName.Focus();
        }
        //if (Session["mainid"] != null)
        //{
        //    Hashtable hshInputs = new Hashtable();
        //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    hshInputs.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
        //    hshInputs.Add("@ImmunizationId",0);
        //    hshInputs.Add("@Type", 1);// data comming for EMRImmunizationStockReceivingDetails table
        //    hshInputs.Add("@MainId", Session["MainId"]);
        //    DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "USPEMRGETIMMUNIZATIONSTOCK", hshInputs);
        //    GvStockList.DataSource = objDs1;
        //    GvStockList.DataBind();
        //    GvStockList.Visible = true;
        //    clear();
        //    Session["mainid"] = null;
        //    ViewState["StockList"] = objDs1.Tables[0];

        //}
    }

    private void populateImmunizationDropDown(string txtsearch)
    {
        try
        {
            // BaseC.EMRImmunization EMRImmunization = new BaseC.EMRImmunization(sConString);
            //SqlDataReader dr = EMRImmunization.getImmunizationName(Convert.ToInt16(Session["HospitalLocationID"]));
            Hashtable HshIn = new Hashtable();
            DAL.DAL Dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // HshIn.Add("@Text", "'"+ddlImmunizationName.Text +"'");
            SqlDataReader dr = (SqlDataReader)Dl.ExecuteReader(CommandType.Text, "select immunizationid,immunizationname from emrimmunizationmaster where Active=1 and immunizationname like '%" + txtsearch + "%' ");
            if (dr.HasRows == true)
            {
                ddlImmunizationName.DataSource = dr;
                ddlImmunizationName.DataTextField = "ImmunizationName";
                ddlImmunizationName.DataValueField = "ImmunizationID";
                ddlImmunizationName.DataBind();
                //ddlImmunizationName.Items.Insert(0, new ListItem("Select", "0"));
            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //protected void cmbDrugList_DataBound(object sender, EventArgs e)
    //{
    //    //set the initial footer label
    //    ((Literal)cmbDrugList.Footer.FindControl("RadComboItemsCount")).Text = Convert.ToString(cmbDrugList.Items.Count);
    //}
    //public void cmbDrugList_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    //{
    //   DataTable data = BindSearchDrugCombo(e.Text);
    //    //this.td cmbDrugList.Items[2].FindControl("hdnDRUG_SYN_ID").ToString()
    //    int itemOffset = e.NumberOfItems;
    //    if (itemOffset == 0)
    //    {
    //        this.cmbDrugList.Items.Clear();
    //    }
    //    int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
    //    e.EndOfItems = endOffset == data.Rows.Count;

    //    for (int i = itemOffset; i < endOffset; i++)
    //    {
    //        RadComboBoxItem item = new RadComboBoxItem();
    //        item.Text = (string)data.Rows[i]["Display_Name"];
    //        item.Value = data.Rows[i]["GENPRODUCT_ID"].ToString();
    //        item.Attributes["DRUG_SYN_ID"] = data.Rows[i]["DRUG_SYN_ID"].ToString();
    //        item.Attributes["Drug_Id"] = data.Rows[i]["Drug_Id"].ToString();
    //        item.Attributes["SYNONYM_TYPE_ID"] = data.Rows[i]["SYNONYM_TYPE_ID"].ToString();
    //        item.Attributes["ROUTE_ID"] = data.Rows[i]["ROUTE_ID"].ToString();
    //        item.Attributes["ROUTE_DESCRIPTION"] = data.Rows[i]["ROUTE_DESCRIPTION"].ToString();
    //        item.Attributes["DOSEFORM_ID"] = data.Rows[i]["DOSEFORM_ID"].ToString();
    //        item.Attributes["DOSEFORM_DESCRIPTION"] = data.Rows[i]["DOSEFORM_DESCRIPTION"].ToString();
    //        this.cmbDrugList.Items.Add(item);
    //        item.DataBind();
    //    }
    //    //e.Message = GetStatusMessage(endOffset, data.Rows.Count);
    //}

    protected void cmbNDCList_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            // if (cmbDrugList.SelectedIndex != -1)
            // {
            DataTable data = BindSearchNDCCombo(e.Text);

            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.cmbNDCList.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["DisplayName"];
                item.Value = data.Rows[i]["PKG_PRODUCT_ID"].ToString();
                this.cmbNDCList.Items.Add(item);
                item.DataBind();
            }
            // e.Message = GetStatusMessage(endOffset, data.Rows.Count);
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected DataTable BindSearchNDCCombo(String etext)
    {
        DataSet ds = new DataSet();
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder objStr = new StringBuilder();
            Hashtable hsInput = new Hashtable();
            // hsInput.Add("intGenProductId", ddlmedication.SelectedValue);// cmbDrugList.SelectedValue);
            hsInput.Add("intGenProductId", RadCmbMedication.SelectedValue);
            if (etext.Trim() == "")
                hsInput.Add("chvSearchCriteria", "");
            else
                hsInput.Add("chvSearchCriteria", etext);
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetNDCCodes", hsInput);
            ds = (DataSet)dl.FillDataSet(CommandType.Text, "Exec UspEMRGetNDCCodes @intGenProductId, '%" + etext + "%' ", hsInput);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return ds.Tables[0];
    }

    //protected DataTable BindSearchDrugCombo(String etext)
    //{
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    Hashtable hshin = new Hashtable();
    //    hshin.Add("@intSynonym_Type_Id",0);// rbonamefilter.SelectedValue);
    //    if (etext.ToString().Trim() != "")
    //    {
    //        hshin.Add("@chvSearchCriteria", "%" + etext + "%");
    //    }
    //    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    DataSet ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "uspGetDrugList", hshin);

    //    return ds.Tables[0];
    //}

    //protected void BindDropdown()
    //{
    //    Hashtable hshInputs = new Hashtable();
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    hshInputs.Add("@To", 1);  //1 for medication records
    //    hshInputs.Add("@GENPRODUCT_ID", ddlImmunizationName.SelectedValue);
    //    DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "UspEmrDrugDetails", hshInputs);

    //    ddlmedication.DataSource = objDs1.Tables[0];
    //    ddlmedication.DataValueField = "genproduct_id";
    //    ddlmedication.DataTextField = "generic_product_name";
    //    ddlmedication.DataBind();
    //    ddlmedication.Items.Insert(0, new ListItem("Select", "0"));

    //}
    //protected void ddlmedication_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //bindmedication(ddlmedication.SelectedValue);
    //    bindmedication(RadCmbMedication.SelectedValue);

    //    ddlunit.SelectedIndex = 0;
    //    cmbNDCList.Text = "";
    //    if (ddlmanufacturer.SelectedIndex != -1)
    //        ddlmanufacturer.Items.RemoveAt(0);     
    //}

    void bindmedication(string medicationvalue)
    {
        //Hashtable hshInputs = new Hashtable();
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshInputs.Add("@To", 2);
        //hshInputs.Add("@GENPRODUCT_ID", medicationvalue);
        //DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "UspEmrDrugDetails", hshInputs);
        //ddlrout.Items.Clear();
        //ddldose.Items.Clear();

        //ddlrout.DataSource = objDs1.Tables[0];
        //ddlrout.DataValueField = "ROUTE_Id";
        //ddlrout.DataTextField = "ROUTE_DESCRIPTION";
        //ddlrout.DataBind();
        //// ddlrout.Items.Insert(0, new ListItem("Select", "0"));

        //ddldose.DataSource = objDs1.Tables[1];
        //ddldose.DataValueField = "DOSEFORM_Id";
        //ddldose.DataTextField = "DOSEFORM_DESCRIPTION";
        //ddldose.DataBind();
    }

    void fillunit()
    {
        try
        {
            DataSet ds = new DataSet();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string str = "";
            str = "SELECT UNIT_ID,UNIT_ABBREVIATION FROM Core_Unit ORDER BY UNIT_ABBREVIATION";
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = (DataSet)dl.FillDataSet(CommandType.Text, str);
            ddlunit.DataSource = ds.Tables[0];
            ddlunit.DataTextField = "UNIT_ABBREVIATION";
            ddlunit.DataValueField = "UNIT_ID";
            ddlunit.DataBind();
            ddlunit.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("ImmunizationStock.aspx", false);
    }

    protected void SaveImmunizationStock_OnClick(Object sender, EventArgs e)
    {
        if (GvStockList.Rows.Count == 0)
        {
            lblMessage.Text = "Please Firstly Select Add To List Item .";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }
        //if (ddlImmunizationName.SelectedIndex == 0)
        //{
        //    lblMessage.Text = "Please Select Immunization Name .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}
        //if (txtLot.Text == "")
        //{
        //    lblMessage.Text = "Please Ente Lot .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}
        //if (RadExpiryDate.DbSelectedDate == null)
        //{
        //    lblMessage.Text = "Please Select Expiry Date .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}
        //if (txtqty.Text == "")
        //{
        //    lblMessage.Text = "Please Enter Quantity .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}
        //if (ddlmedication.SelectedValue == "")
        //{
        //    lblMessage.Text = "Please Select Medication .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}
        //if (ddlunit.SelectedValue == "0")
        //{
        //    lblMessage.Text = "Please Select Unit .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}


        ////if (cmbDrugList.Text == "")
        ////{
        ////    lblMessage.Text = "Please Select Druge Name .";
        ////    lblMessage.ForeColor = System.Drawing.Color.Red;
        ////    return;
        ////}

        //if (cmbNDCList.Text == "")
        //{
        //    lblMessage.Text = "Please Select NDC Code .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}
        //if (ddlmanufacturer.SelectedValue == "")
        //{
        //    lblMessage.Text = "Please Select NDC Code .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}   
        try
        {
            StringBuilder strSQL = new StringBuilder();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            foreach (GridViewRow gvr in GvStockList.Rows)
            {
                Label lblImmuId = (Label)gvr.FindControl("lblImmuId");
                Label lblGENProdID = (Label)gvr.FindControl("lblGENProdID");
                // Label lblDoseFormId = (Label)gvr.FindControl("lblDoseFormId");
                //  Label lblRouteId = (Label)gvr.FindControl("lblRouteId");
                Label lblUnitId = (Label)gvr.FindControl("lblUnitId");
                Label lblNDCCode = (Label)gvr.FindControl("lblNDCCode");
                // Label lblManufacturerId = (Label)gvr.FindControl("lblManufacturerId");
                Label lblLotNo = (Label)gvr.FindControl("lblLotNo");
                Label lblquantity = (Label)gvr.FindControl("lblquantity");
                Label lblexpirydate = (Label)gvr.FindControl("lblexpirydate");
                Label lblId = (Label)gvr.FindControl("lblId");

                strSQL.Append("<Table1><c1>");
                strSQL.Append(lblImmuId.Text);
                strSQL.Append("</c1><c2>");
                strSQL.Append(lblGENProdID.Text);
                strSQL.Append("</c2><c3>");
                strSQL.Append("");//drug id
                strSQL.Append("</c3><c4>");
                strSQL.Append("0");//lblDoseFormId.Text);
                strSQL.Append("</c4><c5>");
                strSQL.Append("0");//lblRouteId.Text);
                strSQL.Append("</c5><c6>");
                strSQL.Append(lblUnitId.Text);
                strSQL.Append("</c6><c7>");
                strSQL.Append(lblNDCCode.Text);
                strSQL.Append("</c7><c8>");
                strSQL.Append("0");//lblManufacturerId.Text);
                strSQL.Append("</c8><c9>");
                strSQL.Append(lblLotNo.Text);//Convert.ToInt32(bc.ParseQ(lblChargeTableID.Text.ToString().Trim()))
                strSQL.Append("</c9><c10>");
                strSQL.Append(lblquantity.Text);
                strSQL.Append("</c10><c11>");
                strSQL.Append(lblexpirydate.Text);
                strSQL.Append("</c11><c12>");
                strSQL.Append(lblId.Text);
                strSQL.Append("</c12></Table1>");
            }
            Hashtable hshInput = new Hashtable();
            Hashtable hsOutput = new Hashtable();
            //hshInput.Add("@StockId", ViewState["StockId"] == null ? "0" : ViewState["StockId"]);
            hshInput.Add("@Id", ViewState["Id"] == null ? "0" : ViewState["Id"]);
            hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInput.Add("@Status", "O");      // o=open ,p= post    
            hshInput.Add("@XMLStockDetails", strSQL.ToString());
            hshInput.Add("@EncodedBy", Session["UserId"]);
            hsOutput.Add("@chvErrorStatus", SqlDbType.VarChar);


            hsOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEmrSaveUpdateIStockReceivingMain", hshInput, hsOutput);
            //hsOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdateImmunizationStock", hshInput, hsOutput);
            if (ViewState["Id"].ToString() == "" || ViewState["Id"] == null)
            {
                lblMessage.Text = "Record is saved.";
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                ViewState["Id"] = null;
            }
            else
            {
                lblMessage.Text = "Record is updated.";
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                ViewState["Id"] = null;
            }
            BindImmunizationMain();
            GvStockList.DataSource = null;
            GvStockList.DataBind();

            clear();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void clear()
    {
        //ddlImmunizationName.Text = null;
        //GvStockList.DataSource = null;
        //GvStockList.Visible = false;
        //ViewState["Id"] = null;
        ViewState["StockId"] = null;
        btnSaveImmunizationStock.Text = "Save";
        RadExpiryDate.DbSelectedDate = null;
        ddlunit.SelectedIndex = 0;
        txtqty.Text = "";
        txtLot.Text = "";
        // ddlmedication.SelectedIndex=0;
        RadCmbMedication.Text = null;
        cmbNDCList.Text = null;
        // ddlrout.SelectedItem.Text="";
        // ddlrout.DataSource = null;
        // ddldose.DataSource = null;
        //// ddldose.SelectedItem.Text = "";
        // ddlmanufacturer.DataSource = null;
        // ddlmanufacturer.DataBind();
        ViewState["updateSelIndex"] = "";
        btnaddtolist.Visible = true;
        btnupdatelist.Visible = false;
    }

    protected void btnaddtolist_OnClick(object sender, EventArgs e)
    {
        if (ddlImmunizationName.Text == "")
        {
            lblMessage.Text = "Please Select Immunization Name .";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }
        if (txtLot.Text == "")
        {
            lblMessage.Text = "Please Enter Lot .";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }
        if (RadExpiryDate.DbSelectedDate == null)
        {
            lblMessage.Text = "Please Select Expiry Date .";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }
        if (txtqty.Text == "")
        {
            lblMessage.Text = "Please Enter Quantity .";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        //if (ddlmedication.SelectedValue == "")
        //{
        //    lblMessage.Text = "Please Select Medication .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}
        if (RadCmbMedication.Text == "")
        {
            lblMessage.Text = "Please Select Medication .";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }
        if (ddlunit.SelectedValue == "0")
        {
            lblMessage.Text = "Please Select Unit .";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        //if (cmbDrugList.Text == "")
        //{
        //    lblMessage.Text = "Please Select Druge Name .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}

        if (cmbNDCList.Text == "")
        {
            lblMessage.Text = "Please Select NDC Code .";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }
        //if (ddlmanufacturer.SelectedValue == "")
        //{
        //    lblMessage.Text = "Please Select NDC Code .";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}

        if (GvStockList.Rows.Count > 0)
        {
            foreach (GridViewRow gvr in GvStockList.Rows)
            {
                string immuId = ((Label)gvr.FindControl("lblImmuId")).Text;
                string lot = ((Label)gvr.FindControl("lblLotNo")).Text;
                string medicationId = ((Label)gvr.FindControl("lblGENProdID")).Text;
                //if(ddlImmunizationName.SelectedValue==immuId && txtLot.Text.Trim()==lot && ddlmedication.SelectedValue==medicationId)
                if (ddlImmunizationName.SelectedValue == immuId && txtLot.Text.Trim() == lot && RadCmbMedication.SelectedValue == medicationId)
                {
                    lblMessage.Text = "This Medicine Already Added In List .";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }

        }

        try
        {
            DataTable dt1 = new DataTable();
            // DataRow[] dr = null;
            DataRow r;

            if (GvStockList.Rows.Count > 0)
            //if (ViewState["StockList"]!=null)
            {
                //for (int i = 0; i < GvStockList.Rows.Count; i++)
                //{
                //    ((Label)GvStockList.Rows[i].FindControl("lblsno")).Text = (i + 1).ToString();
                //}
                dt1 = (DataTable)ViewState["StockList"];
                r = dt1.NewRow();
                //r["Sno"] =  GvStockList.Rows.Count + 1;

            }
            else
            {
                r = dt1.NewRow();
                //dt1.Columns.Add("Sno");
                dt1.Columns.Add("Id");
                dt1.Columns.Add("ImmunizationId");
                dt1.Columns.Add("ImmunizationName");
                dt1.Columns.Add("GENPRODUCT_ID");
                dt1.Columns.Add("Medication");

                dt1.Columns.Add("UnitId");
                dt1.Columns.Add("UnitName");
                //dt1.Columns.Add("RouteId");
                //dt1.Columns.Add("RoutName");
                //dt1.Columns.Add("DoseFormId");
                //dt1.Columns.Add("DoseName");
                dt1.Columns.Add("NDCCode");
                dt1.Columns.Add("NDCName");
                dt1.Columns.Add("LotNo");
                //dt1.Columns.Add("DRUG_ID");
                dt1.Columns.Add("ExpiryDate");//,typeof(DateTime));
                dt1.Columns.Add("QtyReceived");
                //dt1.Columns.Add("Manufacturer");
                //dt1.Columns.Add("ManufacturerId");

                //r["Sno"] = 1;

            }

            r["Id"] = "0";
            r["ImmunizationName"] = ddlImmunizationName.Text;
            r["ImmunizationId"] = ddlImmunizationName.SelectedValue;
            r["Medication"] = RadCmbMedication.Text;//ddlmedication.SelectedItem.Text;
            r["GENPRODUCT_ID"] = RadCmbMedication.SelectedValue;// ddlmedication.SelectedValue;
            r["UnitId"] = ddlunit.SelectedValue;
            r["UnitName"] = ddlunit.SelectedItem.Text;
            //r["RouteId"] = "";// ddlrout.SelectedValue;
            //r["RoutName"] = "";// ddlrout.SelectedItem.Text;
            //r["DoseFormId"] = "";// ddldose.SelectedValue;
            //r["DoseName"] = "";// ddldose.SelectedItem.Text;
            r["NDCCode"] = cmbNDCList.SelectedValue;
            r["NDCName"] = cmbNDCList.Text;
            r["LotNo"] = txtLot.Text.Trim();
            r["ExpiryDate"] = RadExpiryDate.SelectedDate.Value.ToString("MM/dd/yyyy");
            r["QtyReceived"] = txtqty.Text;
            r["QtyReceived"] = txtqty.Text;
            //r["Manufacturer"] = ddlImmunizationName.Text;
            //r["ManufacturerId"] = "";// ddlmanufacturer.SelectedValue;
            dt1.Rows.Add(r);
            ViewState["StockList"] = dt1;
            //DataView ldv = new DataView(dt1);
            //ldv.Sort = "Id desc";
            GvStockList.DataSource = dt1;
            GvStockList.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        clear();
    }

    protected void btnupdatelist_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddlImmunizationName.Text == "")
            {
                lblMessage.Text = "Please Select Immunization Name .";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (txtLot.Text == "")
            {
                lblMessage.Text = "Please Ente Lot .";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (RadExpiryDate.DbSelectedDate == null)
            {
                lblMessage.Text = "Please Select Expiry Date .";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (txtqty.Text == "")
            {
                lblMessage.Text = "Please Enter Quantity .";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            //if (ddlmedication.SelectedValue == "")
            //{
            //    lblMessage.Text = "Please Select Medication .";
            //    lblMessage.ForeColor = System.Drawing.Color.Red;
            //    return;
            //}

            if (RadCmbMedication.Text == "")
            {
                lblMessage.Text = "Please Select Medication .";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (ddlunit.SelectedValue == "0")
            {
                lblMessage.Text = "Please Select Unit .";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (cmbNDCList.Text == "")
            {
                lblMessage.Text = "Please Select NDC Code .";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            //if (ddlmanufacturer.SelectedValue == "")
            //{
            //    lblMessage.Text = "Please Select NDC Code .";
            //    lblMessage.ForeColor = System.Drawing.Color.Red;
            //    return;
            //}

            if (GvStockList.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in GvStockList.Rows)
                {
                    string immuId = ((Label)gvr.FindControl("lblImmuId")).Text;
                    string lot = ((Label)gvr.FindControl("lblLotNo")).Text;
                    string medicationId = ((Label)gvr.FindControl("lblGENProdID")).Text;
                    if (gvr.RowIndex != Convert.ToInt32(ViewState["updateSelIndex"]))
                    {
                        //if (ddlImmunizationName.SelectedValue == immuId && txtLot.Text.Trim() == lot && ddlmedication.SelectedValue == medicationId)
                        if (ddlImmunizationName.SelectedValue == immuId && txtLot.Text.Trim() == lot && RadCmbMedication.SelectedValue == medicationId)
                        {
                            lblMessage.Text = "This Medicine Already Added In List .";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }
                int updIndex = Convert.ToInt32(ViewState["updateSelIndex"]);
                DataTable dt1 = (DataTable)ViewState["StockList"];
                dt1.Rows[updIndex]["ImmunizationName"] = ddlImmunizationName.Text;
                dt1.Rows[updIndex]["ImmunizationId"] = ddlImmunizationName.SelectedValue;
                dt1.Rows[updIndex]["Medication"] = RadCmbMedication.Text;// ddlmedication.SelectedItem.Text;
                dt1.Rows[updIndex]["GENPRODUCT_ID"] = RadCmbMedication.SelectedValue;// ddlmedication.SelectedValue;
                dt1.Rows[updIndex]["UnitId"] = ddlunit.SelectedValue;
                dt1.Rows[updIndex]["UnitName"] = ddlunit.SelectedItem.Text;
                //dt1.Rows[updIndex]["RouteId"] = "";// ddlrout.SelectedValue;
                //dt1.Rows[updIndex]["RoutName"] = "";// ddlrout.SelectedItem.Text;
                //dt1.Rows[updIndex]["DoseFormId"] = "";//ddldose.SelectedValue;
                //dt1.Rows[updIndex]["DoseName"] = "";// ddldose.SelectedItem.Text;
                dt1.Rows[updIndex]["NDCCode"] = cmbNDCList.SelectedValue;
                dt1.Rows[updIndex]["NDCName"] = cmbNDCList.Text;
                dt1.Rows[updIndex]["LotNo"] = txtLot.Text.Trim();
                dt1.Rows[updIndex]["ExpiryDate"] = RadExpiryDate.SelectedDate.Value.ToString("MM/dd/yyyy");
                dt1.Rows[updIndex]["QtyReceived"] = txtqty.Text;
                dt1.Rows[updIndex]["QtyReceived"] = txtqty.Text;
                //dt1.Rows[updIndex]["Manufacturer"] = ddlImmunizationName.Text;
                //dt1.Rows[updIndex]["ManufacturerId"] = "";// ddlmanufacturer.SelectedValue;
                //dt1.Rows.Add(r);
                ViewState["StockList"] = dt1;
                GvStockList.DataSource = dt1;
                GvStockList.DataBind();
                clear();
                ViewState["StockList"] = dt1;
            }

            ViewState["updateSelIndex"] = "";
            btnaddtolist.Visible = true;
            btnupdatelist.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GvStockList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlImmunizationName.SelectedValue = ((Label)GvStockList.Rows[GvStockList.SelectedIndex].FindControl("lblImmuId")).Text;
            txtLot.Text = ((Label)GvStockList.Rows[GvStockList.SelectedIndex].FindControl("lblLotNo")).Text;
            RadExpiryDate.DbSelectedDate = ((Label)GvStockList.Rows[GvStockList.SelectedIndex].FindControl("lblexpirydate")).Text;
            //ddlmedication.SelectedValue = ((Label)GvStockList.Rows[GvStockList.SelectedIndex].FindControl("lblGENProdID")).Text;
            RadCmbMedication.Text = ((Label)GvStockList.Rows[GvStockList.SelectedIndex].FindControl("lblDISPLAYNAME")).Text;
            RadCmbMedication.SelectedValue = ((Label)GvStockList.Rows[GvStockList.SelectedIndex].FindControl("lblGENProdID")).Text;
            ddlunit.SelectedValue = ((Label)GvStockList.Rows[GvStockList.SelectedIndex].FindControl("lblUnitId")).Text;
            //bindmedication(ddlmedication.SelectedValue);
            bindmedication(RadCmbMedication.SelectedValue);
            cmbNDCList.Text = ((Label)GvStockList.Rows[GvStockList.SelectedIndex].FindControl("lblNDCName")).Text;
            cmbNDCList.SelectedValue = ((Label)GvStockList.Rows[GvStockList.SelectedIndex].FindControl("lblNDCCode")).Text;
            cmbNDCList_OnSelectedIndexChanged(sender, e);
            txtqty.Text = ((Label)GvStockList.Rows[GvStockList.SelectedIndex].FindControl("lblquantity")).Text;
            btnaddtolist.Visible = false;
            btnupdatelist.Visible = true;
            ViewState["updateSelIndex"] = GvStockList.SelectedIndex;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ibtnListDelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            ImageButton btn = sender as ImageButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            DataTable dt1 = (DataTable)ViewState["StockList"];

            dt1.Rows.RemoveAt(row.RowIndex);
            ViewState["StockList"] = dt1;
            GvStockList.DataSource = dt1;
            GvStockList.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void cmbNDCList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //DataSet ds = new DataSet();
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //string str = "";
        //str = "SELECT nl.LABELER_NAME,nl.LABELER_ID FROM dbo.NDC_PKG_PRODUCT AS NPP INNER JOIN NDC_LABELER nl ON nl.LABELER_ID = npp.LABELER_ID WHERE  NPP.PKG_PRODUCT_ID='" + cmbNDCList.SelectedValue + "'";//NPP.GENPRODUCT_ID = '" + ddldose.SelectedValue + "' and
        //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //ds = (DataSet)dl.FillDataSet(CommandType.Text, str);
        //ddlmanufacturer.DataSource = ds.Tables[0];
        //ddlmanufacturer.DataTextField = "LABELER_NAME";
        //ddlmanufacturer.DataValueField = "LABELER_ID";
        //ddlmanufacturer.DataBind();
    }

    //protected void btnShow_OnClick(object sender, EventArgs e)
    //{
    //    //Button btn = sender as Button;
    //    //GridViewRow row = btn.NamingContainer as GridViewRow;

    //    RadWindow1.NavigateUrl = "ImmunizationMain.aspx?ImmId=" + ddlImmunizationName.SelectedValue;
    //    RadWindow1.Height = 500;
    //    RadWindow1.Width = 750;
    //    RadWindow1.Top = 20;
    //    RadWindow1.Left = 20;
    //    // RadWindowForNew.Title = "Time Slot";
    //    RadWindow1.OnClientClose = "OnClientClosewindow";
    //    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //    RadWindow1.Modal = true;
    //    RadWindow1.VisibleStatusbar = false;
    //    Session["pevpageclose"] = 1;

    //}

    protected void btnclose_OnClick(object sender, EventArgs e)
    {
    }

    protected void RadCmbMedication_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            Hashtable hshInputs = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInputs.Add("@To", 1);  //1 for medication records
            hshInputs.Add("@GENPRODUCT_ID", ddlImmunizationName.SelectedValue);

            if (e.Text.Trim() == "")
                hshInputs.Add("@chvSearchCriteria", "");
            else
                hshInputs.Add("@chvSearchCriteria", e.Text.Trim());
            DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "UspEmrDrugDetails", hshInputs);

            RadCmbMedication.DataSource = objDs1.Tables[0];
            RadCmbMedication.DataValueField = "genproduct_id";
            RadCmbMedication.DataTextField = "generic_product_name";
            RadCmbMedication.DataBind();
            // RadCmbMedication.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void RadCmbMedication_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindmedication(RadCmbMedication.SelectedValue);
        ddlunit.SelectedIndex = 0;
        cmbNDCList.Text = "";
        //if (ddlmanufacturer.SelectedIndex != -1)
        //    ddlmanufacturer.Items.RemoveAt(0); 
    }

    protected void ddlImmunizationName_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        populateImmunizationDropDown(e.Text);
    }

    protected void ddlImmunizationName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindImmunizationMain();
    }

    //--------------immunization main coding----------------------
    private void BindImmunizationMain()
    {
        try
        {
            Hashtable hshInputs = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInputs.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInputs.Add("@ImmunizationId", ddlImmunizationName.SelectedValue);// Request.QueryString["ImmId"]);
            hshInputs.Add("@Type", 0);// data comming from EMRImmunizationStockReceivingMain table
            hshInputs.Add("@Status", ddlstatus.SelectedValue);

            DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "USPEMRGETIMMUNIZATIONSTOCK", hshInputs);

            gvImmunizationStock.DataSource = objDs1;
            gvImmunizationStock.DataBind();

            if (gvImmunizationStock.Rows.Count > 0)
                lblImmunizationName.Text = "Stock Details of " + ddlImmunizationName.Text;
            else
                lblImmunizationName.Text = "";

            foreach (GridViewRow rw in gvImmunizationStock.Rows)
            {
                if (((Label)rw.FindControl("lblStatus")).Text == "Post")
                {
                    ((LinkButton)rw.FindControl("btnview")).Enabled = false;
                    ((LinkButton)rw.FindControl("btnStockPost")).Enabled = false;
                    ((ImageButton)rw.FindControl("ibtnDelete")).Visible = false;
                }
                else
                {
                    ((LinkButton)rw.FindControl("btnview")).Enabled = true;
                    ((LinkButton)rw.FindControl("btnStockPost")).Enabled = true;
                    ((ImageButton)rw.FindControl("ibtnDelete")).Visible = true;
                }
            }
            //if (ddlstatus.SelectedValue == "P")
            //{
            //    gvImmunizationStock.Columns[10].Visible = false;
            //    gvImmunizationStock.Columns[9].Visible = false;
            //    gvImmunizationStock.Columns[8].Visible = false;
            //}
            //else if (ddlstatus.SelectedValue == "O")
            //{
            //    gvImmunizationStock.Columns[10].Visible = true;
            //    gvImmunizationStock.Columns[9].Visible = true;
            //    gvImmunizationStock.Columns[8].Visible = true;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ImageButton lbtn = sender as ImageButton;
            GridViewRow dr = lbtn.NamingContainer as GridViewRow;

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            // HshIn.Add("@DocumentNo", ((Label)dr.FindControl("lblDocumentNo")).Text);
            HshIn.Add("@DetailId", ((Label)dr.FindControl("lblDetailid")).Text);
            // HshIn.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            HshIn.Add("@EncodedBy", Session["UserId"]);
            //Int32 i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE EMRImmunizationStockReceivingDetails set Active=0 where MainId in (select Id from EMRImmunizationStockReceivingMain where Active=1 and DocumentNo=@DocumentNo and HospitalLocationId=@HospitalLocationId)  update EMRImmunizationStockReceivingMain set Active=0 where DocumentNo=@DocumentNo and HospitalLocationId=@HospitalLocationId", HshIn);
            Int32 i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE EMRImmunizationStockReceivingDetails set Active=0 where Id=@DetailId ", HshIn);
            if (i == 0)
            {
                lblMessage.Text = "Record Deleted...";
                //Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);
                BindImmunizationMain();
            }
            else
            {
                lblMessage.Text = "Error In Deleted...";
                //Alert.ShowAjaxMsg("Error In De-Activation... ", this.Page);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvImmunizationStock_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvImmunizationStock.EditIndex = -1;
        BindImmunizationMain();
    }

    protected void gvImmunizationStock_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvImmunizationStock.PageIndex = e.NewPageIndex;
        BindImmunizationMain();
    }

    protected void gvImmunizationStock_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //LinkButton lb = (LinkButton)gvImmunizationStock.Rows[gvImmunizationStock.SelectedIndex].FindControl("btnview");
            // Session["mainid"] = ((Label)gvImmunizationStock.Rows[gvImmunizationStock.SelectedIndex].FindControl("lblId")).Text;
            // lb.Attributes.Add("onclick", "javascript:window.close();");

            Hashtable hshInputs = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInputs.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            //hshInputs.Add("@ImmunizationId", 0);
            hshInputs.Add("@Type", 1);// data will come from EMRImmunizationStockReceivingDetails table
            hshInputs.Add("@MainId", ((Label)gvImmunizationStock.Rows[gvImmunizationStock.SelectedIndex].FindControl("lblId")).Text);
            DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "USPEMRGETIMMUNIZATIONSTOCK", hshInputs);
            GvStockList.DataSource = objDs1;
            GvStockList.DataBind();
            GvStockList.Visible = true;
            // clear();
            // Session["mainid"] = null;
            ViewState["StockList"] = objDs1.Tables[0];
            ViewState["Id"] = ((Label)gvImmunizationStock.Rows[gvImmunizationStock.SelectedIndex].FindControl("lblId")).Text;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnStockPost_OnClick(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            LinkButton lbtn = sender as LinkButton;
            GridViewRow dr = lbtn.NamingContainer as GridViewRow;

            Hashtable hshInput = new Hashtable();
            Hashtable hsOutput = new Hashtable();
            try
            {
                string status = ((Label)dr.FindControl("lblStatus")).Text == "Open" ? "O" : "P";
                //hshInput.Add("@StockId", ViewState["StockId"] == null ? "0" : ViewState["StockId"]);
                // hshInput.Add("@Id",);
                hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
                hshInput.Add("@DocumentNo", ((Label)dr.FindControl("lblDocumentNo")).Text);
                hshInput.Add("@Status", status);     // O=open ,P= post           
                hshInput.Add("@EncodedBy", Session["UserId"]);
                hsOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshInput.Add("@DetailId", ((Label)dr.FindControl("lblDetailid")).Text);
                hsOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRSaveUpdateIMMUNIZATIONSTOCK", hshInput, hsOutput);
                lblMessage.Text = "Record is saved.";
                BindImmunizationMain();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;// hsOutput["@chvErrorStatus"].ToString();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvImmunizationStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
        {
            Label lblName = (Label)e.Row.Cells[8].FindControl("lblDocumentNo");
            ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
            //img.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you want to Delete this Document No :  " + lblName.Text + "')");
            img.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you want to Delete .");
        }
    }

    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindImmunizationMain();
        lblMessage.Text = "";
    }

}
