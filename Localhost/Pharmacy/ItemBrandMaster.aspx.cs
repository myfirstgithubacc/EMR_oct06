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

public partial class Pharmacy_ItemBrandMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    StringBuilder strXML;
    ArrayList coll;
    clsExceptionLog objException = new clsExceptionLog();
    bool RowSelStauts = false;
    bool RowSelStautsCharge = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myBool(Session["MainFacility"]))
            {
                btnSaveData.Visible = true;
            }
            else
            {
                btnSaveData.Visible = false;
            }
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            objPharmacy = new BaseC.clsPharmacy(sConString);

            trBrand.Visible = false;
            trGeneric.Visible = false;
            trCIMSCategory.Visible = false;
            lblPacking.Visible = false;
            tdPacking.Visible = false;
            ViewState["_ID"] = "0";

            chkPurchaseClose.Enabled = true;
            chkSaleClose.Enabled = true;

            chkPurchaseClose.Checked = false;
            chkSaleClose.Checked = false;

            bindControl();
            bindDetailsData(false, "");

            //bindItem(-1);
            tdIsItem2.Visible = false;

            txtItemName.Attributes.Add("onblur", "nSat=1;");
            txtSpecification.Attributes.Add("onblur", "nSat=1;");
            //GetPackingBind();
            bindItemUnit();
        }
        lblMessage.Text = "&nbsp;";
        // bindItem();
    }
    private void bindItem()
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            string strQry = "Select isnull(ItemId,0) ItemId,isnull(ItemName,'') ItemName,HospitalLocationId,isnull(ItemSubCategoryId,0) ItemSubCategoryId   ,isnull(GenericId,0) GenericId,isnull(CIMSCategoryId,0) CIMSCategoryId,isnull(CIMSSubCategoryId,0) CIMSSubCategoryId,  isnull(ManufactureId,0) ManufactureId,ItemBrandId,StrengthId,FormulationId from PhrItemMaster   where ItemId=1";
            ds = objDl.FillDataSet(CommandType.Text, strQry);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DAL.DAL objDl1 = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


                Hashtable HshIn = new Hashtable();
                HshIn.Add("@intItemId", dr["ItemId"]);
                HshIn.Add("@chvItemName", dr["ItemName"]);
                HshIn.Add("@inyHospitalLocationId", dr["ItemId"]);
                HshIn.Add("@intItemSubCategoryId", dr["HospitalLocationId"]);
                HshIn.Add("@intGenericId", dr["GenericId"]);
                HshIn.Add("@intCIMSCategoryId", dr["CIMSCategoryId"]);
                HshIn.Add("@intCIMSSubCategoryId", dr["CIMSSubCategoryId"]);
                HshIn.Add("@intManufactureId", dr["ManufactureId"]);
                HshIn.Add("@intBrandId", dr["ItemBrandId"]);
                HshIn.Add("@intStrengthId", dr["StrengthId"]);
                HshIn.Add("@intFormulationId", dr["FormulationId"]);

                ds = objDl1.FillDataSet(CommandType.StoredProcedure, "UspItemNoGenerate", HshIn);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;


        }
    }
    private void bindItemUnit()
    {
        try
        {
            DataSet ds;
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = objPharmacy.getItemWithItemUnitTagging(common.myInt(ViewState["_ID"]), false);

            gvItemUnit.DataSource = ds.Tables[0];
            gvItemUnit.DataBind();

            //ds = objPharmacy.getItemWithItemUnitTagging(common.myInt(ViewState["_ID"]), true);

            //gvItemUnit.DataSource = ds.Tables[0];
            //gvItemUnit.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    private void bindControl()
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds;

            bindManufacture();
            bindBrand();
            bindStrength();
            bindGeneric();
            bindPacking();
            ds = objPharmacy.getItemCategoryMaster(0, common.myInt(Session["HospitalLocationId"]), 1, common.myInt(Session["UserID"]));
            ddlItemCategory.DataSource = ds.Tables[0];
            ddlItemCategory.DataTextField = "ItemCategoryName";
            ddlItemCategory.DataValueField = "ItemCategoryId";
            ddlItemCategory.DataBind();

            ddlItemCategory.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlItemCategory.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void bindBrand()
    {
        try
        {
            DataSet ds = new DataSet();
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = objPharmacy.uspPhrGetItemBrandMaster(common.myInt(Session["HospitalLocationID"]), 0, 1, "", common.myInt(Session["UserID"]));
            ddlBrand.DataSource = ds.Tables[0];
            ddlBrand.DataTextField = "ItemBrandName";
            ddlBrand.DataValueField = "ItemBrandId";
            ddlBrand.DataBind();
            ddlBrand.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlBrand.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void bindStrength()
    {
        try
        {
            DataSet ds = new DataSet();
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = objPharmacy.GetItemStrength(0, 0, common.myInt(Session["HospitalLocationID"]), 1, common.myInt(Session["UserID"]));
            ddlStrength.DataSource = ds.Tables[0];
            ddlStrength.DataTextField = "Strength";
            ddlStrength.DataValueField = "StrengthId";
            ddlStrength.DataBind();
            ddlStrength.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlStrength.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void bindGeneric()
    {
        try
        {
            DataSet ds = new DataSet();
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = objPharmacy.GetGenericDetails(0, "", 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]), 0, 0);
            ddlGeneric.DataSource = ds.Tables[0];
            ddlGeneric.DataTextField = "GenericName";
            ddlGeneric.DataValueField = "GenericId";
            ddlGeneric.DataBind();
            ddlGeneric.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlGeneric.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void bindPacking()
    {
        try
        {
            DataSet ds = new DataSet();
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = objPharmacy.GetPakingMaster(0, 1);
            ddlPacking.DataSource = ds.Tables[0];
            ddlPacking.DataTextField = "PackingName";
            ddlPacking.DataValueField = "PackingId";
            ddlPacking.DataBind();
            ddlPacking.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlPacking.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //private void bindCIMSCategory()
    //{
    //    try
    //    {
    //        DataSet ds = new DataSet();
    //        objPharmacy = new BaseC.clsPharmacy(sConString);
    //        ds = objPharmacy.GetCIMSCategory(1);

    //        ddlCIMSCategory.DataSource = ds.Tables[0];
    //        ddlCIMSCategory.DataTextField = "CIMSCategoryName";
    //        ddlCIMSCategory.DataValueField = "CIMSCategoryId";
    //        ddlCIMSCategory.DataBind();

    //        ddlCIMSCategory.Items.Insert(0, new RadComboBoxItem("", "0"));
    //        ddlCIMSCategory.SelectedIndex = 0;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    //private void bindCIMSSubCategory(int CIMSCategoryId)
    //{
    //    try
    //    {
    //        DataSet ds = new DataSet();
    //        objPharmacy = new BaseC.clsPharmacy(sConString);
    //        ds = objPharmacy.GetCIMSSubCategory(CIMSCategoryId, 1);

    //        ddlCIMSSubCategory.DataSource = ds.Tables[0];
    //        ddlCIMSSubCategory.DataTextField = "CIMSSubCategoryName";
    //        ddlCIMSSubCategory.DataValueField = "CIMSSubCategoryId";
    //        ddlCIMSSubCategory.DataBind();

    //        ddlCIMSSubCategory.Items.Insert(0, new RadComboBoxItem("", "0"));
    //        ddlCIMSSubCategory.SelectedIndex = 0;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    private void bindManufacture()
    {
        try
        {
            DataSet ds = new DataSet();
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = objPharmacy.getManufactureMasterList(0, common.myInt(Session["HospitalLocationID"]), 1, "", "", common.myInt(Session["UserID"]));

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dr["ManufactureName"];
                item.Value = dr["ManufactureId"].ToString();
                item.Attributes.Add("ManufactureShortName", common.myStr(dr["ManufactureShortName"]));
                ddlManufacture.Items.Add(item);
                item.DataBind();
            }
            ddlManufacture.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlManufacture.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    //private void BindFormulation()
    //{
    //    try
    //    {
    //        objPharmacy = new BaseC.clsPharmacy(sConString);
    //        DataSet ds = objPharmacy.GetFormulationMaster(0, common.myInt(Session["HospitalLocationID"]), 1, common.myInt(Session["UserID"]));
    //        ddlFormulation.DataSource = ds.Tables[0];
    //        ddlFormulation.DataTextField = "FormulationName";
    //        ddlFormulation.DataValueField = "FormulationId";
    //        ddlFormulation.DataBind();

    //        ddlFormulation.Items.Insert(0, new RadComboBoxItem("", "0"));
    //        ddlFormulation.SelectedIndex = 0;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    //private void BindStrengthValue()
    //{
    //    try
    //    {
    //        objPharmacy = new BaseC.clsPharmacy(sConString);
    //        DataSet ds = new DataSet();

    //        ds = objPharmacy.getTaggedItemStrength(common.myInt(ViewState["_ID"]),
    //                common.myInt(ddlFormulation.SelectedValue), false, common.myInt(Session["HospitalLocationID"]));

    //        gvStrength.DataSource = ds;
    //        gvStrength.DataBind();
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";
        trRTLDetail.Visible = false;
        ddlBrand.SelectedIndex = 0;
        ddlStrength.SelectedIndex = 0;
        ddlGeneric.SelectedIndex = 0;
        txtCIMSCategory.Text = "";
        txtCIMSSubCategory.Text = "";
        txtSearchValue.Text = "";
        txtItemName.Text = "";
        lblSubGroupName.Text = "";
        ddlManufacture.SelectedIndex = 0;
        txtSpecification.Text = "";
        ddlStatus.SelectedIndex = 0;
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //GetPackingBind();
        bindItemUnit();

        chkPurchaseClose.Enabled = true;
        chkSaleClose.Enabled = true;
        chkPurchaseClose.Checked = false;
        chkSaleClose.Checked = false;
    }

    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myInt(ViewState["SubCategoryId"]) == 0)
        {
            strmsg += "Item Sub Group not Selected !";
            isSave = false;
        }
        if (txtItemName.Text == "")
        {
            strmsg += "Item Name can't be blank !";
            isSave = false;
        }

        //if (common.myInt(rdoIsItem.SelectedValue) == 0 && ddlFormulation.SelectedIndex < 1)
        //{
        //    strmsg += "Please select Formulation !";
        //    return false;
        //}

        //if (txtCIMSCategory.Text == "")
        //{
        //    strmsg += "CIMS Category is not available !";
        //    isSave = false;
        //}
        //if (txtCIMSSubCategory.Text == "")
        //{
        //    strmsg += "CIMS Sub Category is not available !";
        //    isSave = false;
        //}
        //if (ddlManufacture.SelectedIndex < 1)
        //{
        //    strmsg += "Manufacture not Selected !";
        //   isSave = false;
        //}
        if (common.myInt(ViewState["_ID"]) == 0
            && common.myInt(ddlStatus.SelectedValue) != 1)
        {
            strmsg += "Status must be Active for New Data ! <br />";
            isSave = false;
        }

        lblMessage.Text = strmsg;
        return isSave;
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }

            #region Item Master

            strXML = new StringBuilder();
            coll = new ArrayList();

            //if (common.myInt(rdoIsItem.SelectedValue) == 0)
            //{
            //    if (gvStrength != null)
            //    {
            //        foreach (GridViewRow dataItem in gvStrength.Rows)
            //        {
            //            CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");

            //            if (common.myBool(chkRow.Checked))
            //            {
            //                HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");

            //                coll.Add(common.myInt(hdnStrengthId.Value));
            //                strXML.Append(common.setXmlTable(ref coll));
            //            }
            //        }
            //    }

            //    if (strXML.ToString() == "")
            //    {
            //        lblMessage.Text = "Item Strength not Selected  !";
            //        return;
            //    }
            //}

            // string[] StrPackingId;
            StringBuilder strXMLPackingId = new StringBuilder();
            //StrPackingId = common.GetCheckedItems(ddlPacking).Split(',');

            //for (int i = 0; i < StrPackingId.Length; i++)
            //{
            //    if (Convert.ToString(StrPackingId[i]) != "")
            //    {
            //        coll.Add(common.myInt(StrPackingId[i]));
            //        strXMLPackingId.Append(common.setXmlTable(ref coll));
            //    }
            //}


            #endregion

            #region Item Unit

            StringBuilder strXMLItemUnit = new StringBuilder();
            coll = new ArrayList();
            int IssueUnitId = 0;

            if (gvItemUnit != null)
            {
                bool IsSelectDefault = false;
                foreach (GridViewRow dataItem in gvItemUnit.Rows)
                {
                    CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");

                    if (common.myBool(chkRow.Checked))
                    {
                        HiddenField hdnItemUnitId = (HiddenField)dataItem.FindControl("hdnItemUnitId");
                        HiddenField hdnIssueUnitId = (HiddenField)dataItem.FindControl("hdnIssueUnitId");
                        RadioButton rdoDefault = (RadioButton)dataItem.FindControl("rdoDefault");

                        if (IssueUnitId > 0 && IssueUnitId != common.myInt(hdnIssueUnitId.Value))
                        {
                            lblMessage.Text = "Item Issue Unit should not be different !";
                            strXMLItemUnit = new StringBuilder();
                            return;
                        }

                        coll.Add(common.myInt(hdnItemUnitId.Value));
                        coll.Add(common.myBool(rdoDefault.Checked) ? 1 : 0);

                        strXMLItemUnit.Append(common.setXmlTable(ref coll));

                        IssueUnitId = common.myInt(hdnIssueUnitId.Value);
                        IsSelectDefault = IsSelectDefault | common.myBool(rdoDefault.Checked);
                    }
                }

                if (!IsSelectDefault)
                {
                    lblMessage.Text = "Please select default item unit !";
                    strXMLItemUnit = new StringBuilder();
                    return;
                }
            }

            if (strXMLItemUnit.ToString() == "")
            {
                lblMessage.Text = "Item Unit not Selected !";
                return;
            }

            #endregion

            objPharmacy = new BaseC.clsPharmacy(sConString);

            string strMsg = objPharmacy.SaveItemMaster(common.myInt(ViewState["_ID"]), common.escapeCharString(txtItemName.Text, false).Trim(),
                            common.myInt(Session["HospitalLocationID"]), common.myInt(ViewState["SubCategoryId"]),
                             common.myInt(ddlGeneric.SelectedValue), common.myInt(0), common.myInt(0), common.myInt(ddlManufacture.SelectedValue),
                            common.escapeCharString(txtSpecification.Text, false).Trim(), 0, strXML.ToString(),
                            common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserID"]), strXMLPackingId.ToString(),
                            strXMLItemUnit.ToString(), common.myBool(chkPurchaseClose.Checked), common.myBool(chkSaleClose.Checked), common.myInt(ddlBrand.SelectedValue), common.myInt(ddlStrength.SelectedValue), common.myInt(ddlPacking.SelectedValue), common.myInt(Session["FacilityId"]));

            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                clearControl();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                ViewState["_ID"] = "0";
                trBrand.Visible = false;
                trGeneric.Visible = false;
                trCIMSCategory.Visible = false;
                lblPacking.Visible = false;
                tdPacking.Visible = false;

                tdIsItem2.Visible = false;

                ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
                lblMessage.Text = strMsg;
            }
            else if (strMsg == string.Empty)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Record Saved Successfully";
            }

           
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
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";

            RadWindowForNew.NavigateUrl = "/Pharmacy/FindItemBrandMaster.aspx";

            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 950;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientFindClose";
            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void OnClientFindClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);

            clearControl();
            ViewState["_ID"] = "0";

            //BindStrengthValue();
            if (common.myInt(hdnItemID.Value) > 0)
            {
                DataSet dsSearch = objPharmacy.getItemList(common.myInt(hdnItemID.Value), 0, "", 0, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), 0);

                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    ViewState["_ID"] = common.myInt(hdnItemID.Value);

                    DataRow DR = dsSearch.Tables[0].Rows[0];

                    ViewState["SubCategoryId"] = common.myStr(DR["ItemSubCategoryId"]);
                    //rdoIsItem.SelectedValue = (common.myBool(DR["IsItem"]) == true) ? "1" : "0";

                    tdIsItem2.Visible = true;
                    //if (common.myBool(DR["IsItem"]))
                    //{
                    //    tdIsItem2.Visible = false;
                    //}

                    //IsItemCreationVisible(); PCAPS19ACIP018
                    ddlBrand.SelectedIndex = ddlBrand.Items.IndexOf(ddlBrand.Items.FindItemByValue(common.myStr(common.myInt(DR["ItemBrandId"]))));
                    ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myStr(common.myInt(DR["StrengthId"]))));
                    ddlGeneric.SelectedIndex = ddlGeneric.Items.IndexOf(ddlGeneric.Items.FindItemByValue(common.myStr(common.myInt(DR["GenericId"]))));
                    ddlPacking.SelectedIndex = ddlPacking.Items.IndexOf(ddlPacking.Items.FindItemByValue(common.myStr(common.myInt(DR["PackingId"]))));

                    DataSet dsItem = dsSearch;// objPharmacy.getItemList(common.myInt(hdnItemID.Value), 0, "", 0, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
                    //if (dsItem.Tables[0].Rows.Count > 0)
                    //{
                    //    ddlCIMSCategory.SelectedIndex = ddlCIMSCategory.Items.IndexOf(ddlCIMSCategory.Items.FindItemByValue(common.myStr(common.myInt(dsItem.Tables[0].Rows[0]["CIMSCategoryId"]))));

                    //    bindCIMSSubCategory(common.myInt(ddlCIMSCategory.SelectedValue));
                    //    ddlCIMSSubCategory.SelectedIndex = ddlCIMSSubCategory.Items.IndexOf(ddlCIMSSubCategory.Items.FindItemByValue(common.myStr(common.myInt(dsItem.Tables[0].Rows[0]["CIMSSubCategoryId"]))));
                    //}
                    txtCIMSCategory.Text = Convert.ToString(dsItem.Tables[0].Rows[0]["CIMSCategoryName"]);
                    txtCIMSCategory.ToolTip = Convert.ToString(dsItem.Tables[0].Rows[0]["CIMSCategoryName"]);
                    txtCIMSSubCategory.Text = Convert.ToString(dsItem.Tables[0].Rows[0]["CIMSSubCategoryName"]);
                    txtCIMSSubCategory.ToolTip = Convert.ToString(dsItem.Tables[0].Rows[0]["CIMSSubCategoryName"]);

                    ddlItemCategory.SelectedIndex = ddlItemCategory.Items.IndexOf(ddlItemCategory.Items.FindItemByValue(common.myStr(common.myInt(DR["ItemCategoryId"]))));

                    isDrug();

                    txtItemName.Text = common.myStr(DR["ItemName"]);
                    ViewState["ItemName"] = common.myStr(DR["ItemName"]);
                    lblSubGroupName.Text = common.myStr(DR["ItemSubCategoryName"]);
                    ddlManufacture.SelectedIndex = ddlManufacture.Items.IndexOf(ddlManufacture.Items.FindItemByValue(common.myStr(common.myInt(DR["ManufactureId"]))));

                    txtSpecification.Text = common.myStr(DR["Specification"]);
                    ddlStatus.SelectedValue = (common.myBool(DR["Active"]) == true) ? "1" : "0";

                    chkPurchaseClose.Checked = common.myBool(DR["ItemCloseForPurchase"]);
                    chkSaleClose.Checked = common.myBool(DR["ItemCloseForSale"]);

                    if (!common.myBool(DR["Active"]))
                    {
                        chkPurchaseClose.Enabled = false;
                        chkSaleClose.Enabled = false;
                    }

                    bindDetailsData(false, "");

                    //bindItem(0);
                    bindItemUnit();
                }
                //if (dsSearch.Tables[1].Rows.Count > 0)
                //{
                //    DataTable dt = (DataTable)dsSearch.Tables[1];
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        ddlPacking.Items.FindItemByValue(Convert.ToString(dt.Rows[i]["PackingId"])).Checked = true;

                //    }

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

    private void bindDetailsData(bool BindStatus, string SearchValue)
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);

            DataSet ds = objPharmacy.getItemCategoryDetails(0, common.parseQManually(SearchValue), common.myInt(ddlItemCategory.SelectedItem.Value), common.myInt(Session["HospitalLocationId"]),
                                                    0, common.myInt(Session["UserID"]));

            if (common.myInt(ddlItemCategory.SelectedItem.Value) == 0)
            {
                RTLDetails.DataSource = ds.Tables[0].Clone();
            }
            else
            {
                RTLDetails.DataSource = ds.Tables[0];
            }
            if (!BindStatus)
            {
                RTLDetails.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void RTLDetails_OnItemCommand(object source, Telerik.Web.UI.TreeListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                ViewState["SubCategoryId"] = common.myInt(((Label)e.Item.FindControl("lblItemSubCategoryId")).Text);
                clearControl();
                lblSubGroupName.Text = ((Label)e.Item.FindControl("lblItemSubCategoryName")).Text;
                lblSubGroupShortName.Text = ((Label)e.Item.FindControl("lblItemSubCategoryShortName")).Text;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void RTLDetails_OnItemDataBound(object source, Telerik.Web.UI.TreeListItemDataBoundEventArgs e)
    {
        try
        {
            if ((e.Item.ItemType == TreeListItemType.Item) || (e.Item.ItemType == TreeListItemType.AlternatingItem) || (e.Item.ItemType == TreeListItemType.SelectedItem))
            {
                HiddenField hdnIsLeafCat = (HiddenField)e.Item.FindControl("hdnIsLeafCat");
                LinkButton lnkBtnSelect = (LinkButton)e.Item.FindControl("lnkBtnSelect");

                if (common.myInt(hdnIsLeafCat.Value) != 1)
                {
                    lnkBtnSelect.Visible = false;
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

    protected void RTLDetails_PageIndexChanged(object source, Telerik.Web.UI.TreeListPageChangedEventArgs e)
    {
        try
        {
            RTLDetails.CurrentPageIndex = e.NewPageIndex;
            string SearchVal = "";
            if (txtSearchValue.Text != "")
            {
                SearchVal = txtSearchValue.Text.Trim();
            }
            bindDetailsData(true, SearchVal);
        }
        catch
        { }
    }

    protected void RTLDetails_NeedDataSource(object source, TreeListNeedDataSourceEventArgs e)
    {
        try
        {
            string SearchVal = "";
            if (txtSearchValue.Text != "")
            {
                SearchVal = txtSearchValue.Text.Trim();
            }
            bindDetailsData(true, SearchVal);
        }
        catch
        { }
    }

    protected void btnSearchField_OnClick(Object sender, EventArgs e)
    {
        bindDetailsData(false, common.myStr(txtSearchValue.Text).Trim());
    }

    protected void ddlItemCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindDetailsData(false, "");

        isDrug();
    }

    void isDrug()
    {
        try
        {
            trBrand.Visible = false;

            trGeneric.Visible = false;
            trCIMSCategory.Visible = false;
            lblPacking.Visible = false;
            tdPacking.Visible = false;


            objPharmacy = new BaseC.clsPharmacy(sConString);
            if (objPharmacy.isDrugsGroup(common.myInt(ddlItemCategory.SelectedValue), common.myInt(Session["HospitalLocationID"])))
            {
                trBrand.Visible = true;

                trGeneric.Visible = true;
                trCIMSCategory.Visible = true;
                lblPacking.Visible = true;
                tdPacking.Visible = true;

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void lnkItemSubDetail_OnClick(object sender, EventArgs e)
    {
        trRTLDetail.Visible = !trRTLDetail.Visible;
    }

    protected void imgBtnGeneric_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/Pharmacy/GenericMaster.aspx";
            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 650;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnGetGeneric_Click(object sender, EventArgs e)
    {
        bindGeneric();
    }

    protected void imgBtnManufacture_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/Pharmacy/SupplierMaster.aspx?MasterPage=NO&MasterType=MM";
            RadWindowForNew.Height = 580;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientManufactureClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnGetManufacture_Click(object sender, EventArgs e)
    {
        bindManufacture();
    }

    //protected void gvStrength_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        //Find the checkbox control in header and add an attribute
    //        ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAllItemStrength('" +
    //            ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
    //    }
    //}

    //protected void gvItem_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gvItem.PageIndex = e.NewPageIndex;
    //    bindItem(0);
    //}

    //protected void gvItem_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int itemId = 0;
    //        HiddenField hdnItemId = (HiddenField)gvItem.SelectedRow.FindControl("hdnItemId");
    //        if (common.myInt(hdnItemId.Value) > 0)
    //        {
    //            itemId = common.myInt(hdnItemId.Value);
    //        }

    //        ddlFormulation.SelectedIndex = 0;

    //        if (itemId > 0)
    //        {
    //            objPharmacy = new BaseC.clsPharmacy(sConString);
    //            DataSet dsSearch = objPharmacy.getItemMaster(itemId, "", "", 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

    //            if (dsSearch.Tables[0].Rows.Count > 0)
    //            {
    //                DataRow DR = dsSearch.Tables[0].Rows[0];
    //                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myStr(DR["FormulationId"])));

    //                BindStrengthValue();
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}

    //private void bindItem(int itemId)
    //{
    //    try
    //    {
    //        objPharmacy = new BaseC.clsPharmacy(sConString);

    //        DataSet dsSearch = objPharmacy.getItemList(itemId, common.myInt(ViewState["_ID"]), 0, "", common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

    //        if (dsSearch.Tables[0].Rows.Count > 0)
    //        {
    //            gvItem.DataSource = dsSearch.Tables[0];
    //        }

    //        gvItem.DataBind();
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}

    //protected void ddlFormulation_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindStrengthValue();
    //}

    //void IsItemCreationVisible()
    //{
    //    tdIsItem1.Visible = true;
    //    tdIsItem2.Visible = true;
    //    if (common.myInt(rdoIsItem.SelectedValue) == 1)
    //    {
    //        tdIsItem1.Visible = false;
    //        tdIsItem2.Visible = false;
    //    }
    //}

    //protected void rdoIsItem_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        IsItemCreationVisible();
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    protected void ddlGeneric_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // bindCIMSSubCategory(common.myInt(ddlCIMSCategory.SelectedValue));
        objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet dsItem = objPharmacy.GetGenericDetails(common.myInt(ddlGeneric.SelectedValue), "", 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), 0, 0);
        txtCIMSCategory.Text = Convert.ToString(dsItem.Tables[0].Rows[0]["CIMSCategoryName"]);
        txtCIMSSubCategory.Text = Convert.ToString(dsItem.Tables[0].Rows[0]["CIMSSubCategoryName"]);
        getItemName();
    }

    //protected void ddlCIMSCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    bindCIMSSubCategory(common.myInt(ddlCIMSCategory.SelectedValue));
    //}

    //protected void imgCIMSCategory_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        RadWindowForNew.NavigateUrl = "~/Pharmacy/CimsCategory.aspx?Master=NO";
    //        RadWindowForNew.Height = 600;
    //        RadWindowForNew.Width = 950;
    //        RadWindowForNew.Top = 10;
    //        RadWindowForNew.Left = 10;
    //        RadWindowForNew.OnClientClose = "OnClientCIMSClose";
    //        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindowForNew.Modal = true;
    //        RadWindowForNew.VisibleStatusbar = false;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    protected void btnGetCIMS_Click(object sender, EventArgs e)
    {
        //bindCIMSCategory();
        //bindCIMSSubCategory(0);
    }
    //protected void imgCIMSSubCategory_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        RadWindowForNew.NavigateUrl = "~/Pharmacy/CimsCategory.aspx?Master=NO";
    //        RadWindowForNew.Height = 600;
    //        RadWindowForNew.Width = 950;
    //        RadWindowForNew.Top = 10;
    //        RadWindowForNew.Left = 10;
    //        RadWindowForNew.OnClientClose = "OnClientCIMSClose";
    //        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindowForNew.Modal = true;
    //        RadWindowForNew.VisibleStatusbar = false;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    //private void GetPackingBind()
    //{
    //    try
    //    {
    //        BaseC.clsPharmacy objPacking = new BaseC.clsPharmacy(sConString);
    //        DataSet ds = objPacking.GetPakingMaster(0, 0);
    //        ddlPacking.DataSource = ds;
    //        ddlPacking.DataTextField = "PackingName";
    //        ddlPacking.DataValueField = "PackingId";
    //        ddlPacking.DataBind();

    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}
    //protected void btnPacking_Click(object sender, EventArgs e)
    //{
    //    GetPackingBind();
    //}     

    //protected void imgBtnPacking_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        RadWindowForNew.NavigateUrl = "~/Pharmacy/PackingMaster.aspx?MasterPage=NO&MasterType=MM";
    //        RadWindowForNew.Height = 580;
    //        RadWindowForNew.Width = 900;
    //        RadWindowForNew.Top = 10;
    //        RadWindowForNew.Left = 10;
    //        RadWindowForNew.OnClientClose = "OnClientPackingClose";
    //        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindowForNew.Modal = true;
    //        RadWindowForNew.VisibleStatusbar = false;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}
    protected void gvItemUnit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    //Find the checkbox control in header and add an attribute
        //    ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAllItemUnit('" +
        //        ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
        //}
    }

    protected void ibtnNewItemUnit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/Pharmacy/ItemUnitMaster.aspx?MasterPage=No";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 850;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.Title = "Item Unit Master";
            RadWindowForNew.OnClientClose = "OnClientItemUnitClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnItemUnitClose_OnClick(object sender, EventArgs e)
    {
        bindItemUnit();
    }
    protected void ddlBrand_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (common.myInt(ddlBrand.SelectedValue) == -5)// open later when filling is based on brand
        {
            DataSet ds = new DataSet();
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = objPharmacy.uspPhrGetItemBrandMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlBrand.SelectedValue), 1, "", common.myInt(Session["UserID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow Dr = ds.Tables[0].Rows[0];
                ddlGeneric.SelectedIndex = ddlGeneric.Items.IndexOf(ddlGeneric.Items.FindItemByValue(common.myStr(Dr["GenericId"])));
                ddlGeneric_OnSelectedIndexChanged(null, null);
                ddlManufacture.SelectedIndex = ddlManufacture.Items.IndexOf(ddlManufacture.Items.FindItemByValue(common.myStr(Dr["ManufactureId"])));
                hdnManufactureShortName.Value = common.myStr(ddlManufacture.SelectedItem.Attributes["ManufactureShortName"]);
                getItemName();
            }
        }
    }
    protected void imgBtnBrand_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/Pharmacy/BrandMaster.aspx";
            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 800;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientCloseBrand";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnBrandMst_Click(object sender, EventArgs e)
    {
        bindBrand();
    }
    protected void ddlStrength_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getItemName();
    }
    protected void imgBtnStrength_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/Pharmacy/StrengthMaster.aspx";
            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 800;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientCloseStrength";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnStrengthMst_Click(object sender, EventArgs e)
    {
        bindStrength();
    }
    protected void getItemName()
    {
        txtItemName.Text = ddlBrand.SelectedItem.Text + " " + ddlStrength.SelectedItem.Text + " " + lblSubGroupShortName.Text + " " + ddlPacking.SelectedItem.Text + " " + common.myStr(ddlManufacture.SelectedItem.Attributes["ManufactureShortName"]);
        if (txtItemName.Text.Trim() == "")
        {
            txtItemName.Text = common.myStr(ViewState["ItemName"]);
        }
    }
    protected void ddlOnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (trBrand.Visible == true)
            getItemName();
    }
    protected void btnItemchk_OnClick(object sender, EventArgs e)
    {
        getItemName();
    }
    protected void iBtnPackingMst_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/Pharmacy/PackingMaster.aspx";
            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 800;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClosePacking";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnPackingMst_Click(object sender, EventArgs e)
    {
        bindPacking();
    }
}
