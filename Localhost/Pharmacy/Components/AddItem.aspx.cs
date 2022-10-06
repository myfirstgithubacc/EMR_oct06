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

public partial class Pharmacy_Components_AddItem : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    BaseC.ParseData bc = new BaseC.ParseData();
    clsExceptionLog objException = new clsExceptionLog();
    private const int ItemsPerRequest = 50;
    BaseC.HospitalSetup objHospitalSetup;//f 
    protected void Page_Load(object sender, EventArgs e)
    {
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        if (!IsPostBack)
        {
            //if (common.myInt(Session["StoreId"]) == 0)
            //{
            //    Response.Redirect("/Pharmacy/ChangeStore.aspx?Mpg=P335&PT=" + Convert.ToString(Page.AppRelativeVirtualPath), false);
            //}

            ViewState["flag"] = 1;
            Cache.Remove("ItemBatch_" + common.myStr(Session["UserId"]));
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;

            // GVItems.Visible = false;
            //BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"]), common.myInt(HttpContext.Current.Session["FacilityId"])));
            if (common.myInt(hdnDecimalPlaces.Value) == 0)
            {
                hdnDecimalPlaces.Value = "2";
            }


            BaseC.Security objSecurity = new BaseC.Security(sConString);  //

            bool IsAllowToAddBlockedItem = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowToAddBlockedItem");
            if (IsAllowToAddBlockedItem)
            {
                if (common.myStr(Request.QueryString["RegId"]) == "" && common.myStr(Request.QueryString["IssueType"]) == "CSI" && common.myStr(Request.QueryString["SetupId"]) == "201")
                {
                    ViewState["IsAllowToAddBlockedItem"] = true;
                }
                else
                {
                    ViewState["IsAllowToAddBlockedItem"] = false;
                }
            }
            else
            {
                ViewState["IsAllowToAddBlockedItem"] = IsAllowToAddBlockedItem;
            }
            //ViewState["IsAllowToAddBlockedItem"]

            objSecurity = null;

            divConfirmation.Visible = false;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            if (Convert.ToString(Request.QueryString["ItemId"]) != null)
            {
                ViewState["ItemId"] = Request.QueryString["ItemId"].ToString().Trim();
                bindItemDetails();
            }
            else
            {
                BindBlankGrid();
            }

            hdnXmlString.Value = "";

            if (Request.QueryString["PageName"] != null)
            {
                hdnPageName.Value = common.myStr(Request.QueryString["PageName"]);
            }
            int iSupplierId = 0;

            if (Request.QueryString["SupplierId"] != null)
            {
                iSupplierId = common.myInt(Request.QueryString["SupplierId"]);
            }
            if (Request.QueryString["MS"] != null)
            {
                hdnIsMainStore.Value = common.myStr(common.myInt(Request.QueryString["MS"]));
            }

            ddlBrand.EmptyMessage = "";
            ddlBrand.Text = "";
            ddlBrand.Focus();

            ViewState["QtyBal"] = common.myDbl(Request.QueryString["QtyBal"]);

            lblBalQty.Text = "";
            if (common.myDbl(ViewState["QtyBal"]) > 0
                && common.myInt(Request.QueryString["ItemId"]) > 0)
            {
                lblBalQty.Text = "Requested Quantity : " + common.myDbl(ViewState["QtyBal"]);
                ViewState["ItemId"] = common.myInt(Request.QueryString["ItemId"]);
                setValue(common.myInt(ViewState["ItemId"]));

                ddlGeneric.Enabled = false;
                ddlBrand.Enabled = false;
                chkSubstitute.Visible = true;
                GetGenericOfItem();
                chkSubstitute.Text = "Show Substitute Item(s) For " + common.myStr(Request.QueryString["IName"]);


            }
        }
        String IsGSTApplicable = common.myStr(objBill.getHospitalSetupValue("IsGSTApplicable", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
        if (IsGSTApplicable == "Y")
        {
            gvService.Columns[13].Visible = false;
            GVItems.Columns[13].Visible = false;
        }
    }
    protected void chkSubstitute_OnCheckedChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(Request.QueryString["ItemId"]) != null)
        {
            ViewState["ItemId"] = Request.QueryString["ItemId"].ToString().Trim();
            bindItemDetails();
            ViewState["Itemdetails"] = null;
            btnSaveData_OnClick(sender, e);
        }

        if (chkSubstitute.Checked == true)
        {
            ddlBrand.Enabled = true;
            GetItemsOfGeneric();
        }
        else
        {
            ddlBrand.Enabled = false;
            setValue(common.myInt(ViewState["ItemId"]));
        }
    }

    private void GetGenericOfItem()
    {
        BaseC.clsPharmacy objphr = new BaseC.clsPharmacy(sConString);
        DataSet ds = objphr.GetGenericOfItem(common.myInt(Request.QueryString["ItemId"]));
        int GenericId = common.myInt(ds.Tables[0].Rows[0]["GenericId"]);
        ddlGeneric.ClearSelection();
        ddlGeneric.Text = "";
        ddlGeneric.Items.Clear();
        DataTable Genericdata = GetGenericData("", GenericId);
        for (int i = 0; i < Genericdata.Rows.Count; i++)
        {
            ddlGeneric.Items.Add(new RadComboBoxItem(common.myStr(Genericdata.Rows[i]["GenericName"]), common.myStr(Genericdata.Rows[i]["GenericId"])));
        }
        if (Genericdata.Rows.Count > 0)
        {
            ddlGeneric.SelectedValue = common.myStr(Genericdata.Rows[0]["GenericId"]);
        }
        else
        {
        }
    }

    protected void GetItemsOfGeneric()
    {
        BaseC.clsPharmacy objphr = new BaseC.clsPharmacy(sConString);
        DataSet ds = objphr.GetGenericOfItem(common.myInt(Request.QueryString["ItemId"]));
        int GenericId = common.myInt(ds.Tables[0].Rows[0]["GenericId"]);
        int ItemSubCategoryId = common.myInt(ds.Tables[0].Rows[0]["ItemSubCategoryId"]);

        DataTable data = GetBrandData("", GenericId, ItemSubCategoryId);
        ddlBrand.ClearSelection();
        ddlBrand.Text = "";
        ddlBrand.Items.Clear();
        for (int i = 0; i < data.Rows.Count; i++)
        {
            //ddlBrand.Items.Add(new RadComboBoxItem(data.Rows[i]["ItemWithStock"].ToString(), data.Rows[i]["ItemId"].ToString()));
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = (string)data.Rows[i]["ItemName"];
            item.Value = data.Rows[i]["ItemId"].ToString();
            item.Attributes.Add("ItemSubCategoryShortName", data.Rows[i]["ItemSubCategoryShortName"].ToString());
            item.Attributes.Add("ClosingBalance", data.Rows[i]["ClosingBalance"].ToString());

            this.ddlBrand.Items.Add(item);
            item.DataBind();
        }


    }
    protected void ddlGeneric_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = sender as RadComboBox;
        DataTable data = GetGenericData(e.Text, 0);
        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;
        for (int i = itemOffset; i < endOffset; i++)
        {
            ddl.Items.Add(new RadComboBoxItem(common.myStr(data.Rows[i]["GenericName"]), common.myStr(data.Rows[i]["GenericId"])));
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);

    }
    private DataTable GetGenericData(string text, int GenericId)
    {
        DataSet dsSearch = new DataSet();
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        int HospId = common.myInt(Session["HospitalLocationID"]);
        int EncodedBy = common.myInt(Session["UserId"]);
        // int GenericId = 0;
        int Active = 1;
        dsSearch = objPharmacy.GetGenericDetails(GenericId, text, Active, HospId, EncodedBy, 0, 0);
        return dsSearch.Tables[0];
    }


    protected void ddlBrand_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        // RadComboBox ddl = sender as RadComboBox;
        if (chkSubstitute.Checked != true)
        {
            if (e.Text != "" && e.Text.Length > 1)
            {
                int GenericId = 0;
                string selectedValue = e.Context["GenericId"].ToString();
                if (common.myInt(selectedValue) > 0)
                {
                    GenericId = common.myInt(selectedValue);
                }
                else
                {
                    GenericId = 0;
                }

                // Rahul's Change 
                DataTable data = GetBrandData(e.Text, GenericId, 0);
                objHospitalSetup = new BaseC.HospitalSetup(sConString);
                if (Request.QueryString["IssueType"] != null && common.myStr(Request.QueryString["IssueType"]) == "IPI")
                {

                }
                else
                {
                    string IsConsignmentItem = common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), "IsConsignmentItem", common.myInt(Session["FacilityId"])));
                    if (IsConsignmentItem == "Y")
                    {
                        DataView DV = data.DefaultView;
                        if (DV.Count > 0)
                        {
                            DV.RowFilter = "BitConsignment <> True";
                            data = DV.ToTable();
                            data.AcceptChanges();
                        }
                    }
                }
                int itemOffset = e.NumberOfItems;
                int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
                e.EndOfItems = endOffset == data.Rows.Count;

                for (int i = itemOffset; i < endOffset; i++)
                {
                    //ddl.Items.Add(new RadComboBoxItem(data.Rows[i]["ItemWithStock"].ToString(), data.Rows[i]["ItemId"].ToString()));
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)data.Rows[i]["ItemName"];
                    item.Value = data.Rows[i]["ItemId"].ToString();
                    item.Attributes.Add("ItemSubCategoryShortName", data.Rows[i]["ItemSubCategoryShortName"].ToString());
                    item.Attributes.Add("ClosingBalance", data.Rows[i]["ClosingBalance"].ToString());

                    this.ddlBrand.Items.Add(item);
                    item.DataBind();
                }
                e.Message = GetStatusMessage(endOffset, data.Rows.Count);
            }
        }
    }

    public void setValue(int ItemId)
    {
        try
        {
            if (ItemId < 1)
            {
                return;
            }

            this.ddlBrand.Text = "";
            this.ddlBrand.Items.Clear();

            DataTable data = GetBrandData("", 0, 0);

            if (data.Rows.Count > 0)
            {
                ////chkSubstitute.Visible = false;
                ddlBrand.EnableLoadOnDemand = true;
                //ddlBrand.Text = common.myStr(data.Rows[0]["ItemWithStock"]);
                //ddlBrand.Items.Add(new RadComboBoxItem(data.Rows[0]["ItemWithStock"].ToString(), data.Rows[0]["ItemId"].ToString()));

                ddlBrand.Text = common.myStr(data.Rows[0]["ItemName"]);
                ddlBrand.Items.Add(new RadComboBoxItem(data.Rows[0]["ItemName"].ToString(), data.Rows[0]["ItemId"].ToString()));
            }
            else
            {
                // chkSubstitute.Visible = true;                ;
                // chkSubstitute.Text ="Show Substitute Item(s) For " + common.myStr(Request.QueryString["IName"]);
                ddlBrand.EnableLoadOnDemand = false;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Stock not available !";
            }
        }
        catch
        { }
    }
    private DataTable GetBrandData(string text, int GenericId, int ItemSubCategoryId)
    {
        DataSet dsSearch = new DataSet();
        DataTable dt = new DataTable();
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        int HospId = common.myInt(Session["HospitalLocationID"]);
        int EncodedBy = common.myInt(Session["UserId"]);
        int StoreId = common.myInt(Request.QueryString["StoreId"]);
        int FacilityId = common.myInt(Session["FacilityId"]);
        int SupplierId;
        if (common.myInt(Request.QueryString["SupplierId"]) != 0)
        {
            SupplierId = common.myInt(Request.QueryString["SupplierId"]);
        }
        else
        {
            SupplierId = 0;
        }

        int ItemId = 0;
        int WithStockOnly = 1;

        if (common.myDbl(ViewState["QtyBal"]) > 0 && common.myInt(Request.QueryString["ItemId"]) > 0)
        {
            ItemId = common.myInt(Request.QueryString["ItemId"]);
        }
        //if (chkSubstitute.Checked == true)
        //{
        //    ItemId = 0;
        //}

        //string ItemSearchFor = Request.QueryString["ItemSearchFor"];

        //if (objPharmacy.IsPackagePatient(0, common.myStr(Request.QueryString["EncounterNo"])) == 1)
        //{
        //    dsSearch = objPharmacy.getItemsWithStockFormulary_PackageRestriction(common.myInt(Session["HospitalLocationID"]), StoreId, ItemId, 0, GenericId,
        //        common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), SupplierId,
        //        text.Replace("'", "''"), WithStockOnly, string.Empty, 0, "S");
        //}
        //else
        //{
        if (Request.QueryString["DeptIssue"] == "DI")
        {
            dsSearch = objPharmacy.getItemsWithStock(HospId, StoreId, ItemId, GenericId, EncodedBy, FacilityId, SupplierId, text.Replace("'", "''"), WithStockOnly, "", ItemSubCategoryId, "", "O", true);
        }
        else
        {
            if (common.myStr(hdnPageName.Value) == "SupRet")
            {
                dsSearch = objPharmacy.getItemsWithStock(HospId, StoreId, ItemId, GenericId, EncodedBy, FacilityId, SupplierId, text.Replace("'", "''"), WithStockOnly, "", ItemSubCategoryId, "", "O");
            }
            else
            {
                dsSearch = objPharmacy.getItemsWithStock(HospId, StoreId, ItemId, GenericId, EncodedBy, FacilityId, SupplierId, text.Replace("'", "''"), WithStockOnly, "", ItemSubCategoryId, "", "S");
            }
        }
        //}
        if (dsSearch.Tables.Count > 0)
        {
            if (dsSearch.Tables[0].Rows.Count > 0)
            {
                dt = dsSearch.Tables[0];
            }
        }
        return dt;
    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches found...";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    protected void btnProceed_OnClick(object sender, EventArgs e)
    {
        ViewState["Yes"] = true;
        bindItemDetails();
        divConfirmation.Visible = false;
        ViewState["Yes"] = null;
    }
    protected void btnProceedCancel_OnClick(object sender, EventArgs e)
    {
        divConfirmation.Visible = false;
    }
    private void bindItemDetails()
    {
        try
        {
            //gvService.DataSource = null;
            //gvService.DataBind();

            objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds;
            int HospId = common.myInt(Session["HospitalLocationId"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int ItemId = common.myInt(ViewState["ItemId"]);
            int StoreId = common.myInt(Request.QueryString["StoreId"]);
            int EncodedBy = common.myInt(Session["UserId"]);
            int CompanyId = common.myInt(Request.QueryString["ComId"]);
            int TransactionId = common.myInt(Request.QueryString["TrnId"]);
            int ItemWithStock = 1;
            ds = objPharmacy.GetItemBatch(HospId, ItemId, StoreId, EncodedBy, FacilityId, ItemWithStock, CompanyId, TransactionId, 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(ds.Tables[0].Rows[0]["IsBlocked"]))
                {
                    if (common.myBool(ViewState["IsAllowToAddBlockedItem"]) == false)
                    {
                        Alert.ShowAjaxMsg("Not authorized to Add Item. Selected Item is blocked for this company.", this.Page);
                        return;
                    }
                    if (common.myBool(ViewState["Yes"]) == false)
                    {
                        divConfirmation.Visible = true;
                        return;
                    }
                }
                //if (common.myBool(ds.Tables[0].Rows[0]["IsBlocked"]))
                //{
                //    ddlBrand.SelectedIndex = -1;
                //    ddlBrand.SelectedValue = "0";
                //    ddlBrand.Items.Clear();
                //    ddlBrand.Text = "";
                //    BindBlankGrid();
                //    ViewState["ItemId"] = null;
                //    Alert.ShowAjaxMsg("Selected Item is blocked, you are not allowed to select this Item", Page);
                //    return;
                //}
                gvService.DataSource = ds.Tables[0];
            }
            else
            {
                gvService.DataSource = ds.Tables[0].Clone();
            }
            //   Cache["ItemBatch_" + common.myStr(Session["UserId"])] = ds.Tables[0];
            ViewState["Servicetable"] = ds.Tables[0];
            gvService.DataBind();

            int totQty = 0;
            foreach (GridViewRow gvr in gvService.Rows)
            {
                TextBox txtStockQty = (TextBox)gvr.FindControl("txtStockQty");

                totQty = totQty + common.myInt(common.myInt(txtStockQty.Text));
            }
            lblMessage.Text = "";
            hdntotQty.Value = common.myStr(totQty);

            if (gvService.Rows.Count > 0)
            {
                TextBox txtQty = (TextBox)gvService.Rows[0].FindControl("txtQty");
                txtQty.Focus();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private DataTable DeleteRecordDataTable(DataTable dtDeleteFrom, string DeleteExpression)
    {
        DataRow[] RowtoBeDeleted;
        RowtoBeDeleted = dtDeleteFrom.Select(DeleteExpression);

        if (RowtoBeDeleted.Length > 0)
        {
            foreach (DataRow dr in RowtoBeDeleted)
            {
                dtDeleteFrom.Rows.Remove(dr);
            }

        }

        return dtDeleteFrom;

    }

    protected void btnAddToGrid_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(ddlBrand.SelectedValue) == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Brand does not exist, Please select from brand drop down list !";
            return;
        }
        ViewState["ItemId"] = common.myInt(ddlBrand.SelectedValue);
        bindItemDetails();
    }

    protected void BindBlankGrid()
    {
        gvService.DataSource = CreateTable();
        gvService.DataBind();
    }
    protected DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 1;
        dt.Columns["SNo"].AutoIncrementStep = 1;
        dt.Columns.Add("ItemId");
        dt.Columns.Add("ItemName");
        dt.Columns.Add("BatchId");
        dt.Columns.Add("BatchNo");
        dt.Columns.Add("ExpiryDate");
        dt.Columns.Add("StockQty");
        dt.Columns.Add("Qty");
        dt.Columns.Add("Units");
        dt.Columns.Add("CostPrice");
        dt.Columns.Add("SalePrice");
        dt.Columns.Add("MRP");
        dt.Columns.Add("DiscAmtPercent");
        dt.Columns.Add("Tax", typeof(double));
        dt.Columns.Add("ItemExpired");


        DataRow dr = dt.NewRow();
        dr["ItemId"] = DBNull.Value;
        dr["ItemName"] = DBNull.Value;
        dr["BatchId"] = DBNull.Value;
        dr["BatchNo"] = DBNull.Value;
        dr["ExpiryDate"] = DBNull.Value;
        dr["StockQty"] = DBNull.Value;
        dr["Qty"] = DBNull.Value;
        dr["Units"] = DBNull.Value;
        dr["CostPrice"] = DBNull.Value;
        dr["SalePrice"] = DBNull.Value;
        dr["MRP"] = DBNull.Value;
        dr["DiscAmtPercent"] = DBNull.Value;
        dr["Tax"] = DBNull.Value;
        dr["ItemExpired"] = DBNull.Value;

        dt.Rows.Add(dr);
        //}
        //   Cache["ItemBatch_" + common.myStr(Session["UserId"])] = dt;
        return dt;
    }


    protected void btnSaveData_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (CheckFractionIssue())
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Decimal value of  Quantity is not allow! ";
                return;
            }

            ViewState["flag"] = 1;
            double netAmt = 0;
            double DiscountPrecent = 0;
            int expired = 0;//f
            DataTable dtgrd = new DataTable();
            BaseC.HospitalSetup objsec = new BaseC.HospitalSetup(sConString);
            BaseC.clsPharmacy objphr = new BaseC.clsPharmacy(sConString);

            string IsVatConsiderForSPSApollo = common.myStr(objsec.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), "IsVatConsiderForSPSApollo", common.myInt(Session["FacilityId"])));

            String Code = common.myStr(Request.QueryString["IssueType"]);

            if (common.myStr(ViewState["Servicetable"]) != null && common.myStr(ViewState["Servicetable"]) != "")
            {
                dtgrd = ((DataTable)ViewState["Servicetable"]).Clone();

                dtgrd.Columns.Add("Qty");
                dtgrd.Columns.Add("DisAmt");
                dtgrd.Columns.Add("NetAmt");
                // dtgrd.Columns.Add("UnitName");

                foreach (GridViewRow gvRow in gvService.Rows)
                {
                    ViewState["Qty"] = common.myStr(((TextBox)gvRow.FindControl("txtQty")).Text.Trim());
                    objHospitalSetup = new BaseC.HospitalSetup(sConString);//f
                    if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsAllowExpiredItemForReturnToSupplier", common.myInt(Session["FacilityId"]))) == "Y")//f
                    {
                        if (common.myStr(hdnPageName.Value) == "SupRet")//f
                        {
                            expired = 0;
                        }
                        else
                        {
                            expired = common.myInt(((Label)gvRow.FindControl("lblItemExpiry")).Text.Trim());
                        }
                    }
                    else
                    {
                        expired = common.myInt(((Label)gvRow.FindControl("lblItemExpiry")).Text.Trim());
                    }

                    //if (common.myInt(hdnIsMainStore.Value) == 0 && Request.QueryString["DeptIssue"] != "DI" && common.myStr(hdnPageName.Value) != "SupRet")
                    if (Request.QueryString["DeptIssue"] == "DI")
                    {
                        if (common.myStr(Request.QueryString["AllowForExpiry"]) == "Y")
                        {
                            expired = 0;
                        }
                    }

                    netAmt = common.myDbl(((TextBox)gvRow.FindControl("txtNetAmt")).Text.Trim());
                    DiscountPrecent = common.myDbl(((TextBox)gvRow.FindControl("txtDiscountPersent")).Text.Trim());
                    //if (common.myStr(hdnPageName.Value) != "Sale")
                    //{
                    //    if (common.myStr(ViewState["Qty"]) != "" && netAmt > 0)
                    //    {
                    //        DataRow dRow = dtgrd.NewRow();
                    //        dRow["ItemId"] = common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim());
                    //        dRow["ItemName"] = common.myStr(((Label)gvRow.FindControl("lblItemName")).Text.Trim());
                    //        ViewState["ItemId1"] = common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim());
                    //        dRow["BatchId"] = common.myStr(((Label)gvRow.FindControl("lblBatchId")).Text.Trim());
                    //        dRow["BatchNo"] = common.myStr(((Label)gvRow.FindControl("lblBatchNo")).Text.Trim());
                    //        dRow["ExpiryDate"] = common.myStr(((Label)gvRow.FindControl("lblExpiryDate")).Text.Trim());
                    //        dRow["StockQty"] = common.myStr(((TextBox)gvRow.FindControl("txtStockQty")).Text.Trim());
                    //        dRow["Qty"] = common.myStr(((TextBox)gvRow.FindControl("txtQty")).Text.Trim());
                    //        dRow["SalePrice"] = common.myStr(((TextBox)gvRow.FindControl("txtCharge")).Text.Trim());
                    //        dRow["MRP"] = common.myStr(((HiddenField)gvRow.FindControl("hdnMRP")).Value.Trim());
                    //        dRow["CostPrice"] = common.myStr(((TextBox)gvRow.FindControl("txtCostPrice")).Text.Trim());
                    //        dRow["DiscAmtPercent"] = common.myStr(((TextBox)gvRow.FindControl("txtDiscountPersent")).Text.Trim());
                    //        dRow["DisAmt"] = common.myStr(((TextBox)gvRow.FindControl("txtDiscountAmt")).Text.Trim());
                    //        dRow["Tax"] = common.myStr(((TextBox)gvRow.FindControl("txtTax")).Text.Trim());
                    //        dRow["NetAmt"] = common.myStr(((TextBox)gvRow.FindControl("txtNetAmt")).Text.Trim());
                    //        dRow["ItemExpired"] = common.myInt(((Label)gvRow.FindControl("lblItemExpiry")).Text.Trim());
                    //        dtgrd.Rows.Add(dRow);
                    //    }
                    //    if (common.myStr(ViewState["Qty"]) != "" && expired == 0 && netAmt == 0 && DiscountPrecent == 100)
                    //    {
                    //        DataRow dRow = dtgrd.NewRow();
                    //        dRow["ItemId"] = common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim());
                    //        dRow["ItemName"] = common.myStr(((Label)gvRow.FindControl("lblItemName")).Text.Trim());
                    //        ViewState["ItemId1"] = common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim());
                    //        dRow["BatchId"] = common.myStr(((Label)gvRow.FindControl("lblBatchId")).Text.Trim());
                    //        dRow["BatchNo"] = common.myStr(((Label)gvRow.FindControl("lblBatchNo")).Text.Trim());
                    //        dRow["ExpiryDate"] = common.myStr(((Label)gvRow.FindControl("lblExpiryDate")).Text.Trim());
                    //        dRow["StockQty"] = common.myStr(((TextBox)gvRow.FindControl("txtStockQty")).Text.Trim());
                    //        dRow["Qty"] = common.myStr(((TextBox)gvRow.FindControl("txtQty")).Text.Trim());
                    //        dRow["SalePrice"] = common.myStr(((TextBox)gvRow.FindControl("txtCharge")).Text.Trim());
                    //        dRow["MRP"] = common.myStr(((HiddenField)gvRow.FindControl("hdnMRP")).Value.Trim());
                    //        dRow["CostPrice"] = common.myStr(((TextBox)gvRow.FindControl("txtCostPrice")).Text.Trim());
                    //        dRow["DiscAmtPercent"] = common.myStr(((TextBox)gvRow.FindControl("txtDiscountPersent")).Text.Trim());
                    //        dRow["DisAmt"] = common.myStr(((TextBox)gvRow.FindControl("txtDiscountAmt")).Text.Trim());
                    //        dRow["Tax"] = common.myStr(((TextBox)gvRow.FindControl("txtTax")).Text.Trim());
                    //        dRow["NetAmt"] = common.myStr(((TextBox)gvRow.FindControl("txtNetAmt")).Text.Trim());
                    //        dRow["ItemExpired"] = common.myInt(((Label)gvRow.FindControl("lblItemExpiry")).Text.Trim());
                    //        dtgrd.Rows.Add(dRow);
                    //    }
                    //    else if (common.myStr(ViewState["Qty"]) != "" && netAmt == 0)
                    //    {
                    //        ((TextBox)gvRow.FindControl("txtQty")).Text = "";
                    //        ((TextBox)gvRow.FindControl("txtQty")).Focus();
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    if (common.myStr(ViewState["Qty"]) != "" && expired == 0 && netAmt > 0)
                    {
                        DataRow dRow = dtgrd.NewRow();
                        dRow["ItemId"] = common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim());
                        dRow["ItemName"] = common.myStr(((Label)gvRow.FindControl("lblItemName")).Text.Trim());
                        ViewState["ItemId1"] = common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim());
                        dRow["BatchId"] = common.myStr(((Label)gvRow.FindControl("lblBatchId")).Text.Trim());
                        dRow["BatchNo"] = common.myStr(((Label)gvRow.FindControl("lblBatchNo")).Text.Trim());
                        dRow["ExpiryDate"] = common.myStr(((Label)gvRow.FindControl("lblExpiryDate")).Text.Trim());
                        dRow["StockQty"] = common.myStr(((TextBox)gvRow.FindControl("txtStockQty")).Text.Trim());
                        dRow["Qty"] = common.myStr(((TextBox)gvRow.FindControl("txtQty")).Text.Trim());
                        dRow["SalePrice"] = common.myStr(((TextBox)gvRow.FindControl("txtCharge")).Text.Trim());

                        //dRow["MRP"] = common.myStr(((HiddenField)gvRow.FindControl("hdnMRP")).Value.Trim());

                        dRow["MRP"] = common.myStr(((TextBox)gvRow.FindControl("txtCharge")).Text.Trim());

                        dRow["CostPrice"] = common.myStr(((TextBox)gvRow.FindControl("txtCostPrice")).Text.Trim());
                        dRow["DiscAmtPercent"] = common.myStr(((TextBox)gvRow.FindControl("txtDiscountPersent")).Text.Trim());
                        dRow["DisAmt"] = common.myStr(((TextBox)gvRow.FindControl("txtDiscountAmt")).Text.Trim());

                        string SupplierItemDetails = objphr.GetSupplierItemDetails(common.myInt(common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim())), common.myInt(Request.QueryString["StoreId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(common.myStr(((Label)gvRow.FindControl("lblBatchId")).Text.Trim())));

                        if (IsVatConsiderForSPSApollo == "Y" && (Code == "CSI" || Code == "IPI") && common.myInt(SupplierItemDetails) == 1)
                        {
                            dRow["Tax"] = common.myStr("0");
                        }
                        else
                        {
                            dRow["Tax"] = common.myStr(((TextBox)gvRow.FindControl("txtTax")).Text.Trim());
                        }
                        dRow["NetAmt"] = common.myStr(((TextBox)gvRow.FindControl("txtNetAmt")).Text.Trim());
                        dRow["ItemExpired"] = common.myInt(((Label)gvRow.FindControl("lblItemExpiry")).Text.Trim());
                        dtgrd.Rows.Add(dRow);
                    }
                    if (common.myStr(ViewState["Qty"]) != "" && expired == 0 && netAmt == 0 && DiscountPrecent == 100)
                    {
                        DataRow dRow = dtgrd.NewRow();
                        dRow["ItemId"] = common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim());
                        dRow["ItemName"] = common.myStr(((Label)gvRow.FindControl("lblItemName")).Text.Trim());
                        ViewState["ItemId1"] = common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim());
                        dRow["BatchId"] = common.myStr(((Label)gvRow.FindControl("lblBatchId")).Text.Trim());
                        dRow["BatchNo"] = common.myStr(((Label)gvRow.FindControl("lblBatchNo")).Text.Trim());
                        dRow["ExpiryDate"] = common.myStr(((Label)gvRow.FindControl("lblExpiryDate")).Text.Trim());
                        dRow["StockQty"] = common.myStr(((TextBox)gvRow.FindControl("txtStockQty")).Text.Trim());
                        dRow["Qty"] = common.myStr(((TextBox)gvRow.FindControl("txtQty")).Text.Trim());
                        dRow["SalePrice"] = common.myStr(((TextBox)gvRow.FindControl("txtCharge")).Text.Trim());

                        dRow["MRP"] = common.myStr(((TextBox)gvRow.FindControl("txtCharge")).Text.Trim());
                        //dRow["MRP"] = common.myStr(((HiddenField)gvRow.FindControl("hdnMRP")).Value.Trim());

                        dRow["CostPrice"] = common.myStr(((TextBox)gvRow.FindControl("txtCostPrice")).Text.Trim());
                        dRow["DiscAmtPercent"] = common.myStr(((TextBox)gvRow.FindControl("txtDiscountPersent")).Text.Trim());
                        dRow["DisAmt"] = common.myStr(((TextBox)gvRow.FindControl("txtDiscountAmt")).Text.Trim());

                        string SupplierItemDetails = objphr.GetSupplierItemDetails(common.myInt(common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim())), common.myInt(Request.QueryString["StoreId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(common.myStr(((Label)gvRow.FindControl("lblBatchId")).Text.Trim())));

                        if (IsVatConsiderForSPSApollo == "Y" && (Code == "CSI" || Code == "IPI") && common.myInt(SupplierItemDetails) == 1)
                        {
                            dRow["Tax"] = common.myStr("0");
                        }
                        else
                        {
                            dRow["Tax"] = common.myStr(((TextBox)gvRow.FindControl("txtTax")).Text.Trim());
                        }
                        dRow["NetAmt"] = common.myStr(((TextBox)gvRow.FindControl("txtNetAmt")).Text.Trim());
                        dRow["ItemExpired"] = common.myInt(((Label)gvRow.FindControl("lblItemExpiry")).Text.Trim());
                        dtgrd.Rows.Add(dRow);
                    }
                    else if (common.myStr(ViewState["Qty"]) != "" && netAmt == 0)
                    {
                        ((TextBox)gvRow.FindControl("txtQty")).Text = "";
                        ((TextBox)gvRow.FindControl("txtQty")).Focus();
                        return;
                    }
                    //}
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dtgrd);
                DataTable dtItem = (DataTable)ds.Tables[0];

                if (ViewState["Itemdetails"] != null)
                {
                    DataTable dt = (DataTable)ViewState["Itemdetails"];

                    DataView dv = dt.DefaultView;
                    if (common.myStr(hdnPageName.Value) == "Sale")
                    {
                        dv.RowFilter = "ItemID = " + common.myInt(ViewState["ItemId1"]) + " and ItemExpired = 0";
                    }
                    else
                    {
                        dv.RowFilter = "ItemID = " + common.myInt(ViewState["ItemId1"]);
                    }

                    if (dv.ToTable().Rows.Count == 0)
                    {
                        dt.Merge(dtItem);
                        dt.AcceptChanges();

                        ds.Tables[0].Dispose();
                        ds = new DataSet();
                        ds.Tables.Add(dt);
                    }
                    else
                    {
                        if (common.myDbl(ViewState["QtyBal"]) == 0)
                        {
                            ds.Dispose();
                            ds = new DataSet();
                            ds.Tables.Add(dt);
                        }
                    }
                }
                ViewState["Itemdetails"] = ds.Tables[0];
                Cache.Remove("ItemBatch_" + common.myStr(Session["UserId"]));
                //  Cache["ItemBatch_" + common.myStr(Session["UserId"])] = ds.Tables[0];
                Cache.Insert("ItemBatch_" + common.myStr(Session["UserId"]), ds.Tables[0], null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                GVItems.DataSource = ds;
                GVItems.DataBind();
                lblInfoItemBasket.Text = "Items Basket (" + common.myStr(ds.Tables[0].Rows.Count) + ")";
                //RowSelStauts = true;

                if (common.myDbl(ViewState["QtyBal"]) == 0)
                {
                    ViewState["Servicetable"] = null;
                    BindBlankGrid();
                    ddlBrand.Items.Clear();
                    ddlBrand.Text = "";
                    ddlBrand.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }

    private bool CheckFractionIssue()
    {
        bool FractionIssue = false;
        try
        {
            string itemid = "";
            foreach (GridViewRow dataItem in gvService.Rows)
            {

                TextBox txtqty = (TextBox)dataItem.FindControl("txtQty");
                string[] decimalqty = txtqty.Text.Split('.');
                if (decimalqty.Length > 1)
                {
                    if (common.myInt(decimalqty[1]) > 0)
                    {
                        itemid = common.myStr(ddlBrand.SelectedValue);

                    }
                }
            }
            objPharmacy = new BaseC.clsPharmacy(sConString);
            if (itemid != "")
            {
                if (objPharmacy.getItemFraction(itemid).Tables[0].Rows.Count > 0)
                {
                    FractionIssue = true;
                }
            }

        }
        catch (Exception Ex)
        {
            FractionIssue = true;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return FractionIssue;
    }
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {

    }
    private int _tabindex = 0;

    public int TabIndex
    {
        get
        {
            _tabindex++;
            return _tabindex;
        }
    }

    protected void gvService_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#5EA0F4";
                }

                TextBox txtStockQty = (TextBox)e.Row.FindControl("txtStockQty");
                TextBox txtQty = (TextBox)e.Row.FindControl("txtQty");
                //TextBox txtUnit = (TextBox)e.Row.FindControl("txtUnit");
                HiddenField hdnUnitId = (HiddenField)e.Row.FindControl("hdnUnitId");
                Label lblItemUnitName = (Label)e.Row.FindControl("lblItemUnitName");

                TextBox txtCharge = (TextBox)e.Row.FindControl("txtCharge");
                TextBox txtPercentDiscount = (TextBox)e.Row.FindControl("txtDiscountPersent");
                TextBox txtDiscountAmt = (TextBox)e.Row.FindControl("txtDiscountAmt");
                TextBox txtTax = (TextBox)e.Row.FindControl("txtTax");
                TextBox txtNetAmt = (TextBox)e.Row.FindControl("txtNetAmt");
                TextBox txtCostPrice = (TextBox)e.Row.FindControl("txtCostPrice");
                Label lblItemExpiry = (Label)e.Row.FindControl("lblItemExpiry");
                Label lblItemNo = (Label)e.Row.FindControl("lblItemNo");
                Label lblExpiryDate = (Label)e.Row.FindControl("lblExpiryDate");

                txtStockQty.Text = common.myDbl(txtStockQty.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtTax.Text = common.myDbl(txtTax.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtCharge.Text = common.myDbl(txtCharge.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtDiscountAmt.Text = common.myDbl(txtDiscountAmt.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtPercentDiscount.Text = common.myDbl(txtPercentDiscount.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //txtCostPrice.Text = common.myDbl(txtCostPrice.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
                if (common.myInt(lblItemNo.Text) != 0)
                {
                    DataSet ds = new DataSet();
                    ds = objPharmacy.GetUnitIssueUnit(common.myInt(lblItemNo.Text));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lblItemUnitName.Text = ds.Tables[0].Rows[0]["IssueUnitName"].ToString();
                            hdnUnitId.Value = ds.Tables[0].Rows[0]["IssueUnitId"].ToString();
                        }
                    }
                }

                // txtQty.Focus();
                //if (gvService.Rows.Count > 0)
                //{
                //    //((TextBox)gvService.Rows[e.Row.TabIndex].FindControl("txtQty")).TabIndex = Convert.ToInt16(e.Row.RowIndex+500);
                //    ((TextBox)gvService.Rows[0].FindControl("txtQty")).Focus();
                //}

                System.Drawing.Color cRed = System.Drawing.Color.FromName("Tomato");
                System.Drawing.Color cPink = System.Drawing.Color.LightPink;
                if (common.myStr(lblExpiryDate.Text) != "")
                {
                    objHospitalSetup = new BaseC.HospitalSetup(sConString);//f03012017
                    int days;

                    DataSet ds = objHospitalSetup.GetFlagValueOfHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ValueOfNearByExpiryDays");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            days = common.myInt(ds.Tables[0].Rows[0]["Value"]);

                            if (DateTime.Now.AddDays(days) >= common.myDate(lblExpiryDate.Text))
                            {
                                e.Row.Cells[4].BackColor = cPink;
                                e.Row.Cells[5].BackColor = cPink;
                                e.Row.Cells[6].BackColor = cPink;
                            }
                        }

                        else
                        {
                            if (DateTime.Now.AddMonths(3) >= common.myDate(lblExpiryDate.Text))

                            {
                                e.Row.Cells[4].BackColor = cPink;
                                e.Row.Cells[5].BackColor = cPink;
                                e.Row.Cells[6].BackColor = cPink;
                            }
                        }
                    }

                    else
                    {
                        if (DateTime.Now.AddMonths(3) >= common.myDate(lblExpiryDate.Text))

                        {
                            e.Row.Cells[4].BackColor = cPink;
                            e.Row.Cells[5].BackColor = cPink;
                            e.Row.Cells[6].BackColor = cPink;
                        }
                    }
                }
                if (common.myInt(lblItemExpiry.Text) == 1)
                {
                    e.Row.Cells[4].BackColor = cRed;
                    e.Row.Cells[5].BackColor = cRed;
                    e.Row.Cells[6].BackColor = cRed;
                    if (common.myStr(hdnPageName.Value) == "Sale")
                    {
                        txtQty.Enabled = false;
                    }
                    if (common.myInt(hdnIsMainStore.Value) == 0 && common.myStr(ViewState["DeptIssue"]) != "DI" && common.myStr(hdnPageName.Value) != "SupRet")
                    {
                        if (common.myStr(Request.QueryString["AllowForExpiry"]) != "Y")//this condition for department return 
                        {
                            txtQty.Enabled = false;
                        }
                    }

                    //  e.Row.ForeColor = System.Drawing.Color.FromName("#FFCECE");
                }


                //if (common.myStr(ViewState["DeptIssue"]) != "DI")
                //{
                txtQty.Attributes.Add("onkeyup", "javascript:chekQty('" + txtStockQty.ClientID + "','" + txtQty.ClientID + "','" + txtCharge.ClientID + "','" + txtPercentDiscount.ClientID + "','" + txtDiscountAmt.ClientID + "','" + txtNetAmt.ClientID + "');");
                //}
                //else
                //{
                //    txtQty.Attributes.Add("onkeyup", "javascript:chekQty('" + txtStockQty.ClientID + "','" + txtQty.ClientID + "','" + txtCostPrice.ClientID + "','" + txtPercentDiscount.ClientID + "','" + txtDiscountAmt.ClientID + "','" + txtNetAmt.ClientID + "');");
                //}
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void GVItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //RowSelStauts = true;
        //  ViewState["Servicetable"] = null;
        if (e.CommandName == "Del")
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (e.CommandArgument != "")
            {
                int intId = Convert.ToInt32(e.CommandArgument);
                if (intId != 0)
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    DataTable dt = new DataTable();
                    dt = (DataTable)Cache["ItemBatch_" + common.myStr(Session["UserId"])];
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows.RemoveAt(row.RowIndex);
                        dt.AcceptChanges();

                        Cache.Remove("ItemBatch_" + common.myStr(Session["UserId"]));
                        Cache["ItemBatch_" + common.myStr(Session["UserId"])] = dt;
                        // Cache.Insert("ItemBatch_" + common.myStr(Session["UserId"]), dt, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                        GVItems.DataSource = dt;
                        GVItems.DataBind();
                        ViewState["Itemdetails"] = dt;

                        ViewState["flag"] = 0;
                        ViewState["Servicetable"] = null;



                    }
                }
            }
        }
    }
    protected void GVItems_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#5EA0F4";
                }

                TextBox txtStockQty = (TextBox)e.Row.FindControl("txtStockQty");
                TextBox txtQty = (TextBox)e.Row.FindControl("txtQty");

                HiddenField hdnUnitId = (HiddenField)e.Row.FindControl("hdnUnitId");
                Label lblItemUnitName = (Label)e.Row.FindControl("lblItemUnitName");

                TextBox txtCharge = (TextBox)e.Row.FindControl("txtCharge");
                TextBox txtPercentDiscount = (TextBox)e.Row.FindControl("txtDiscountPersent");
                TextBox txtDiscountAmt = (TextBox)e.Row.FindControl("txtDiscountAmt");
                TextBox txtTax = (TextBox)e.Row.FindControl("txtTax");
                TextBox txtNetAmt = (TextBox)e.Row.FindControl("txtNetAmt");
                TextBox txtCostPrice = (TextBox)e.Row.FindControl("txtCostPrice");
                Label lblItemExpiry = (Label)e.Row.FindControl("lblItemExpiry");


                Label lblItemNo = (Label)e.Row.FindControl("lblItemNo");

                //Commented By Tony on 28-12-2015
                //txtStockQty.Text = common.myDbl(txtStockQty.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtTax.Text = common.myDbl(txtTax.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtCharge.Text = common.myDbl(txtCharge.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtDiscountAmt.Text = common.myDbl(txtDiscountAmt.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtPercentDiscount.Text = common.myDbl(txtPercentDiscount.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //txtCostPrice.Text = common.myDbl(txtCostPrice.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
                if (common.myInt(lblItemNo.Text) != 0)
                {
                    DataSet ds = new DataSet();
                    ds = objPharmacy.GetUnitIssueUnit(common.myInt(lblItemNo.Text));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lblItemUnitName.Text = ds.Tables[0].Rows[0]["IssueUnitName"].ToString();
                            hdnUnitId.Value = ds.Tables[0].Rows[0]["IssueUnitId"].ToString();
                        }
                    }
                }
                if (common.myStr(ViewState["DeptIssue"]) != "DI")
                {
                    txtQty.Attributes.Add("onkeyup", "javascript:chekQty2('" + txtStockQty.ClientID + "','" + txtQty.ClientID + "','" + txtCharge.ClientID + "','" + txtPercentDiscount.ClientID + "','" + txtDiscountAmt.ClientID + "','" + txtNetAmt.ClientID + "');");
                }
                else
                {
                    txtQty.Attributes.Add("onkeyup", "javascript:chekQty('" + txtStockQty.ClientID + "','" + txtQty.ClientID + "','" + txtPercentDiscount.ClientID + "','" + txtDiscountAmt.ClientID + "','" + txtNetAmt.ClientID + "');");
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

    protected void btnCloseW_Click(object sender, EventArgs e)
    {
        BaseC.clsPharmacy objphr = new BaseC.clsPharmacy(sConString);//my
        DataSet ds1 = new DataSet();
        ViewState["flag"] = 0;
        hdnXmlString.Value = "";
        hdnTotalQty.Value = "";

        if (common.myStr(ViewState["Itemdetails"]) != "")
        {
            DataTable dtgrd = new DataTable();
            dtgrd = ((DataTable)ViewState["Itemdetails"]).Clone();

            double enteredQty = 0;
            foreach (GridViewRow gvRow in GVItems.Rows)
            {
                ViewState["Qty"] = common.myStr(((TextBox)gvRow.FindControl("txtQty")).Text.Trim());

                if (common.myStr(ViewState["Qty"]) != "")
                {
                    DataRow dRow = dtgrd.NewRow();
                    dRow["ItemId"] = common.myStr(((Label)gvRow.FindControl("lblItemNo")).Text.Trim());
                    dRow["ItemName"] = common.myStr(((Label)gvRow.FindControl("lblItemName")).Text.Trim());
                    dRow["BatchId"] = common.myStr(((Label)gvRow.FindControl("lblBatchId")).Text.Trim());
                    dRow["BatchNo"] = common.myStr(((Label)gvRow.FindControl("lblBatchNo")).Text.Trim());
                    dRow["ExpiryDate"] = common.myDate(common.myStr(((Label)gvRow.FindControl("lblExpiryDate")).Text.Trim())).ToString("yyyy-MM-dd");
                    dRow["StockQty"] = common.myStr(((TextBox)gvRow.FindControl("txtStockQty")).Text.Trim());
                    dRow["Qty"] = common.myStr(((TextBox)gvRow.FindControl("txtQty")).Text.Trim());
                    dRow["SalePrice"] = common.myStr(((TextBox)gvRow.FindControl("txtCharge")).Text.Trim());
                    dRow["MRP"] = common.myStr(((HiddenField)gvRow.FindControl("hdnMRP")).Value.Trim());
                    dRow["CostPrice"] = common.myStr(((TextBox)gvRow.FindControl("txtCostPrice")).Text.Trim());
                    dRow["DiscAmtPercent"] = common.myStr(((TextBox)gvRow.FindControl("txtDiscountPersent")).Text.Trim());
                    dRow["DisAmt"] = common.myStr(((TextBox)gvRow.FindControl("txtDiscountAmt")).Text.Trim());
                    dRow["Tax"] = common.myStr(((TextBox)gvRow.FindControl("txtTax")).Text.Trim());
                    dRow["NetAmt"] = common.myStr(((TextBox)gvRow.FindControl("txtNetAmt")).Text.Trim());
                    ds1 = objphr.GetItemReusableValue(common.myInt(dRow["ItemId"]));//my
                    if (ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            if (ds1.Tables[0].Rows[0]["Reusable"].ToString() == common.myStr(1))
                            {
                                dRow["reusable"] = 1;
                            }
                            else
                            {
                                dRow["reusable"] = 0;
                            }

                        }
                    }
                    else
                    {
                        dRow["reusable"] = 0;

                    }

                    dtgrd.Rows.Add(dRow);

                    enteredQty += common.myDbl(dRow["Qty"]);
                }
            }

            if (common.myDbl(ViewState["QtyBal"]) > 0)
            {
                if (enteredQty > common.myDbl(ViewState["QtyBal"]))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Item batch quantity cann't be greater then balance quantity !";

                    return;
                }
            }

            Cache["ItemBatch_" + common.myStr(Session["UserId"])] = dtgrd;

            DataTable dt = new DataTable();
            dt = dtgrd;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            DataSet dsResponse = new DataSet();

            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.

            ds.WriteXml(writer);

            string xmlSchema = writer.ToString();
            hdnXmlString.Value = xmlSchema;
            hdnTotalQty.Value = common.myStr(enteredQty);

            Cache.Remove("ItemBatch_" + common.myStr(Session["UserId"]));
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Item not added in Basket !";
            return;
        }

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }

}
