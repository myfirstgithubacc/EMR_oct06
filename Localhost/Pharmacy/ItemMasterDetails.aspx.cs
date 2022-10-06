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
using System.IO;

public partial class Pharmacy_ItemMasterDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    StringBuilder strXMLItemUnit;
    StringBuilder strXMLAddField;
    StringBuilder strXMLCharge;
    ArrayList coll;
    clsExceptionLog objException = new clsExceptionLog();
    bool RowSelStauts = false;
    bool RowSelStautsCharge = false;
    bool RowSelStautsCurrentStock = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            lnkItemFlag.Visible = false;
            lnkItemSupplier.Visible = false;
            // if (common.myBool(Session["MainFacility"]))
          //  {
                //    btnSaveData.Visible = true;
                //}
                //else
                //{
                //    btnSaveData.Visible = false;
                //}
                EnableMainFacilityWise();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            objPharmacy = new BaseC.clsPharmacy(sConString);
            removeDropDownFromTextBox(txtShelfLifeYears);

            ViewState["_ID"] = "0";

            cboItem.IsBasedOnStoreId = false;
            cboItem.clearValue();
            bindMonthDay();
            bindControl();
            bindCharge();
            bindItemCurrentStock();
            bindAdditionalField();
            Telerik.Web.UI.RadComboBox ddlItem = (Telerik.Web.UI.RadComboBox)cboItem.FindControl("cboItem");
            ddlItem.EmptyMessage = " ";
            ddlItem.Focus();
        }
    }
    private void EnableMainFacilityWise()
    {
        Telerik.Web.UI.RadComboBox ddlItem = (Telerik.Web.UI.RadComboBox)cboItem.FindControl("cboItem");
        if (common.myBool(Session["MainFacility"]) )
        {
            btnSaveData.Text = "Save";
            ddlItem.Enabled = true;
            ddlRequestedFacility.Enabled = true;
            txtShelfLifeYears.Enabled = true;
            txtSpecification.Enabled = true;
            ddlStatus.Enabled = true;
            ddlShelfLifeDays.Enabled = true;
            ddlShelfLifeMonths.Enabled = true;
            gvAdditionalField.Enabled = true;
            ddlRecommendedBy.Enabled = true;
        }
        else
        {
            btnSaveData.Text = "Update";
            ddlItem.Enabled = false;
            ddlRequestedFacility.Enabled = false;
            txtShelfLifeYears.Enabled = false;
            txtSpecification.Enabled = false;
            ddlStatus.Enabled = false;
            ddlShelfLifeDays.Enabled = false;
            ddlShelfLifeMonths.Enabled = false;
            gvAdditionalField.Enabled = false;
            ddlRecommendedBy.Enabled = false;
        }
    }
    private void removeDropDownFromTextBox(TextBox TXT)
    {
        try
        {
            TXT.Attributes.Add("autocomplete", "off");
        }
        catch
        {
        }
    }
    private void bindMonthDay()
    {
        try
        {
            string strIdx = "";

            ddlShelfLifeMonths.Items.Add(new RadComboBoxItem("", "0"));
            for (int idx = 1; idx <= 12; idx++)
            {
                strIdx = common.myStr(idx);
                ddlShelfLifeMonths.Items.Add(new RadComboBoxItem(strIdx, strIdx));
            }

            ddlShelfLifeDays.Items.Add(new RadComboBoxItem("", "0"));
            for (int idx = 1; idx <= 30; idx++)
            {
                strIdx = common.myStr(idx);
                ddlShelfLifeDays.Items.Add(new RadComboBoxItem(strIdx, strIdx));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void bindAdditionalField()
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = objPharmacy.getItemFieldDetails(common.myInt(ViewState["_ID"]), false, "I");

            gvAdditionalField.DataSource = ds.Tables[0];
            gvAdditionalField.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvAdditionalField_PreRender(object sender, EventArgs e)
    {
        if (RowSelStauts)
        {
            bindAdditionalField();
        }
    }

    protected void gvTax_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item
            || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            TextBox txtChargeValue = (TextBox)e.Item.FindControl("txtChargeValue");
            removeDropDownFromTextBox(txtChargeValue);

            txtChargeValue.Text = common.myDbl(txtChargeValue.Text).ToString("F4");
        }
    }

    private void bindControl()
    {
        objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        try
        {
            /*******RequestedFacility*******/
            BaseC.clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);
            ds = objLISMaster.getFacility(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));

            ddlRequestedFacility.DataSource = ds.Tables[0];
            ddlRequestedFacility.DataTextField = "Name";
            ddlRequestedFacility.DataValueField = "FacilityID";
            ddlRequestedFacility.DataBind();

            ddlRequestedFacility.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlRequestedFacility.SelectedIndex = 0;

            /******RecommendedBy********/
            bindRecommendedBy();

            /*****ItemUnitTagging********/
            bindItemUnit();

            /***** Strength Value ********/
            //BindStrengthValue();

            ds = new DataSet();

            ds = objPharmacy.getDrugAttributesMasterList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));

            #region Table 0-Unit
            if (ds.Tables.Count > 0)
            {
                ddlUnit.DataSource = ds.Tables[0];
                ddlUnit.DataValueField = "Id";
                ddlUnit.DataTextField = "UnitName";
                ddlUnit.DataBind();

                ddlUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                ddlUnit.SelectedIndex = 0;
            }
            #endregion

            #region Table 1-Formulation
            if (ds.Tables.Count > 1)
            {
                Telerik.Web.UI.RadComboBoxItem item;
                foreach (DataRow DR in ds.Tables[1].Rows)
                {
                    item = new Telerik.Web.UI.RadComboBoxItem();
                    item.Text = common.myStr(DR["FormulationName"]);
                    item.Value = common.myStr(common.myInt(DR["FormulationId"]));
                    item.Attributes.Add("DefaultRouteId", common.myStr(DR["DefaultRouteId"]));

                    ddlFormulation.Items.Add(item);
                }
                ddlFormulation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                ddlFormulation.SelectedIndex = 0;
            }
            #endregion
            #region Table 2-Route
            if (ds.Tables.Count > 2)
            {
                ddlRoute.DataSource = ds.Tables[2];
                ddlRoute.DataValueField = "Id";
                ddlRoute.DataTextField = "RouteName";
                ddlRoute.DataBind();
                ddlRoute.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                ddlRoute.SelectedIndex = 0;
            }
            #endregion
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
        }
    }

    private void bindItemUnit()
    {
        try
        {
            DataSet ds;
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = objPharmacy.getItemWithItemUnitTagging(common.myInt(ViewState["_ID"]), false);

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

    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";
        txtItemNo.Text = "";

        ddlRequestedFacility.SelectedIndex = 0;

        rdoIsFractionalIssue.SelectedIndex = 1;
        rdoIsProfile.SelectedIndex = 1;
        rdoIsVatInclude.SelectedIndex = 0;
        ddlRecommendedBy.SelectedIndex = 0;
        txtShelfLifeYears.Text = "";
        ddlShelfLifeMonths.SelectedIndex = 0;
        ddlShelfLifeDays.SelectedIndex = 0;
        txtSpecification.Text = "";
        txtRack.Text = "";
        rdoIsSubstituteNotAllowed.SelectedIndex = 1;
        txtDepreciationDays.Text = "";
        txtDepreciationPerc.Text = "";

        ddlStatus.SelectedIndex = 0;

        txtDose.Text = string.Empty;
        ddlUnit.SelectedIndex = 0;
        ddlFormulation.SelectedIndex = 0;
        ddlRoute.SelectedIndex = 0;

        PatientImage.ImageUrl = "~/Images/logo/ImageNotAvailable.jpg";

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        //if (ddlRequestedFacility.SelectedIndex < 1)
        //{
        //    strmsg += "Requested Facility not Selected !";
        //    isSave = false;
        //}

        if (common.myInt(hdnItemID.Value) == 0)
        {
            strmsg += "Item Name not Selected !";
            isSave = false;
        }

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
            objPharmacy = new BaseC.clsPharmacy(sConString);

            #region Image Data

            byte[] byteImageData = null;
            string sImageName = "";
            if (common.myStr(ViewState["iMageName"]) != "")
            {
                String FilePath = Server.MapPath("/PatientDocuments/PatientImages/") + common.myStr(ViewState["iMageName"]);
                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] image = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
                byteImageData = image;

                sImageName = common.myStr(ViewState["iMageName"]);
            }

            #endregion

            #region Item Unit

            strXMLItemUnit = new StringBuilder();
            coll = new ArrayList();
            int IssueUnitId = 0;

            //if (gvItemUnit != null)
            //{
            //    bool IsSelectDefault = false;
            //    foreach (GridViewRow dataItem in gvItemUnit.Rows)
            //    {
            //        CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");

            //        if (common.myBool(chkRow.Checked))
            //        {
            //            HiddenField hdnItemUnitId = (HiddenField)dataItem.FindControl("hdnItemUnitId");
            //            HiddenField hdnIssueUnitId = (HiddenField)dataItem.FindControl("hdnIssueUnitId");
            //            RadioButton rdoDefault = (RadioButton)dataItem.FindControl("rdoDefault");

            //            if (IssueUnitId > 0 && IssueUnitId != common.myInt(hdnIssueUnitId.Value))
            //            {
            //                lblMessage.Text = "Item Issue Unit should not be different !";
            //                strXMLItemUnit = new StringBuilder();
            //                return;
            //            }

            //            coll.Add(common.myInt(hdnItemUnitId.Value));
            //            coll.Add(common.myBool(rdoDefault.Checked) ? 1 : 0);

            //            strXMLItemUnit.Append(common.setXmlTable(ref coll));

            //            IssueUnitId = common.myInt(hdnIssueUnitId.Value);
            //            IsSelectDefault = IsSelectDefault | common.myBool(rdoDefault.Checked);
            //        }
            //    }

            //    if (!IsSelectDefault)
            //    {
            //        lblMessage.Text = "Please select default item unit !";
            //        strXMLItemUnit = new StringBuilder();
            //        return;
            //    }
            //}

            //if (strXMLItemUnit.ToString() == "")
            //{
            //    lblMessage.Text = "Item Unit not Selected !";
            //    return;
            //}

            #endregion

            #region Charge Field

            strXMLCharge = new StringBuilder();
            coll = new ArrayList();

            if (gvTax != null)
            {
                foreach (GridDataItem dataItem in gvTax.Items)
                {
                    Label lblChargeId = (Label)dataItem.FindControl("lblChargeId");
                    TextBox txtChargeValue = (TextBox)dataItem.FindControl("txtChargeValue");

                    if (common.myDbl(txtChargeValue.Text) > 0)
                    {
                        coll.Add(common.myInt(lblChargeId.Text));
                        coll.Add(common.myDbl(txtChargeValue.Text));

                        strXMLCharge.Append(common.setXmlTable(ref coll));
                    }
                }
            }

            #endregion

            #region Additional Field

            strXMLAddField = new StringBuilder();
            coll = new ArrayList();

            if (gvAdditionalField != null)
            {
                foreach (GridDataItem dataItem in gvAdditionalField.Items)
                {
                    Label lblFieldId = (Label)dataItem.FindControl("lblFieldId");
                    Label lblFieldType = (Label)dataItem.FindControl("lblFieldType");

                    coll.Add(common.myInt(lblFieldId.Text));

                    if (common.myStr(lblFieldType.Text) == "N")
                    {
                        TextBox txtN = (TextBox)dataItem.FindControl("txtN");
                        coll.Add(common.myStr(common.myDbl(txtN.Text)));
                    }
                    else if (common.myStr(lblFieldType.Text) == "T")
                    {
                        TextBox txtT = (TextBox)dataItem.FindControl("txtT");
                        coll.Add(common.myStr(txtT.Text));
                    }
                    else if (common.myStr(lblFieldType.Text) == "M")
                    {
                        TextBox txtM = (TextBox)dataItem.FindControl("txtM");
                        coll.Add(common.myStr(txtM.Text));
                    }
                    else
                    {
                        coll.Add(DBNull.Value);
                    }

                    if (common.myStr(lblFieldType.Text) == "D")
                    {
                        RadComboBox ddlD = (RadComboBox)dataItem.FindControl("ddlD");
                        coll.Add(common.myStr(common.myInt(ddlD.SelectedValue)));
                    }
                    else
                    {
                        coll.Add(DBNull.Value);
                    }

                    if (common.myStr(lblFieldType.Text) == "W")
                    {

                        RadEditor txtW = (RadEditor)dataItem.FindControl("txtW");
                        coll.Add(common.myStr(txtW.Text));
                    }
                    else
                    {
                        coll.Add(DBNull.Value);
                    }

                    strXMLAddField.Append(common.setXmlTable(ref coll));
                }
            }

            #endregion

            string strMsg = objPharmacy.SaveItemMasterDetails(common.myInt(hdnItemID.Value),
                            common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                            common.myInt(ddlRequestedFacility.SelectedValue),
                            common.myInt(txtShelfLifeYears.Text), common.myInt(ddlShelfLifeMonths.SelectedValue),
                            common.myInt(ddlShelfLifeDays.SelectedValue), common.myInt(ddlRecommendedBy.SelectedValue),
                            common.myInt(rdoIsFractionalIssue.SelectedValue), common.myInt(rdoIsProfile.SelectedValue),
                            common.myInt(rdoIsVatInclude.SelectedValue), common.escapeCharString(txtSpecification.Text, false).Trim(),
                            byteImageData, sImageName.Trim(), strXMLItemUnit.ToString(), strXMLCharge.ToString(),
                            strXMLAddField.ToString(), common.myInt(ddlStatus.SelectedValue),
                            common.myInt(Session["UserID"]), chkPanelpriceRequired.Checked, chkReusable.Checked, common.myDbl(txtVatOnSale.Text),
                            common.myStr(txtRack.Text), chkConsumable.Checked, chkUseforbplpatient.Checked,
                            common.myBool(rdoIsSubstituteNotAllowed.SelectedValue), 0, 0,
                            common.myInt(txtDose.Text), common.myInt(ddlUnit.SelectedValue),
                            common.myInt(ddlFormulation.SelectedValue), common.myInt(ddlRoute.SelectedValue));

            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                clearControl();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                ViewState["_ID"] = "0";
                hdnItemID.Value = "0";

                cboItem.setValue(0, 0);
                cboItem.clearValue();

                bindItemUnit();
                bindCharge();
                bindItemCurrentStock();
                bindAdditionalField();
                fillData();
                lblMessage.Text = strMsg;
            }
            else if (strMsg == string.Empty)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Record Saved";
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

            RadWindowForNew.NavigateUrl = "/Pharmacy/FindItemMaster.aspx";

            RadWindowForNew.Height = 530;
            RadWindowForNew.Width = 475;
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

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        fillData();
    }

    protected void OnClientFindClose_OnClick(object Sender, EventArgs e)
    {
        cboItem.setValue(0, 0);
        fillData();
    }

    protected void fillData()
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);

            clearControl();
            ViewState["_ID"] = "0";

            if (common.myInt(hdnItemID.Value) > 0)
            {
                DataSet dsSearch;

                dsSearch = objPharmacy.getItemMaster(common.myInt(hdnItemID.Value), "", "", 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    ViewState["_ID"] = common.myInt(hdnItemID.Value);

                    DataRow DR = dsSearch.Tables[0].Rows[0];
                    //cboItem.clearValue();
                    //cboItem.setValue(common.myInt(ViewState["_ID"]), 0);

                    cboItem.clearValue();
                    Telerik.Web.UI.RadComboBox ddlItem = (Telerik.Web.UI.RadComboBox)cboItem.FindControl("cboItem");
                    ddlItem.Text = common.myStr(DR["ItemName"]);
                    ddlItem.ToolTip = common.myStr(DR["ItemName"]);
                    ddlItem.Attributes.Add("ItemNo", common.myStr(DR["ItemNo"]));

                    txtItemNo.Text = common.myStr(DR["ItemNo"]);
                    ddlRequestedFacility.SelectedIndex = ddlRequestedFacility.Items.IndexOf(ddlRequestedFacility.Items.FindItemByValue(common.myStr(DR["RequestedFacilityId"])));
                    rdoIsFractionalIssue.SelectedValue = common.myBool(DR["IsFractionalIssue"]) ? "1" : "0";
                    rdoIsProfile.SelectedValue = common.myBool(DR["IsProfile"]) ? "1" : "0";
                    rdoIsVatInclude.SelectedValue = common.myBool(DR["IsVatInclude"]) ? "1" : "0";
                    rdoIsSubstituteNotAllowed.SelectedValue = common.myBool(DR["IsSubstituteNotAllowed"]) ? "1" : "0";
                    bindRecommendedBy();

                    ddlRecommendedBy.SelectedIndex = ddlRecommendedBy.Items.IndexOf(ddlRecommendedBy.Items.FindItemByValue(common.myStr(DR["RecommendedBy"])));
                    txtShelfLifeYears.Text = common.myStr(common.myInt(DR["ShelfLifeYears"]));
                    ddlShelfLifeMonths.SelectedIndex = ddlShelfLifeMonths.Items.IndexOf(ddlShelfLifeMonths.Items.FindItemByValue(common.myStr(DR["ShelfLifeMonths"])));
                    ddlShelfLifeDays.SelectedIndex = ddlShelfLifeDays.Items.IndexOf(ddlShelfLifeDays.Items.FindItemByValue(common.myStr(DR["ShelfLifeDays"])));
                    txtRack.Text = common.myStr(DR["Rack"]);
                    txtSpecification.Text = common.myStr(DR["Specification"]);
                    ddlStatus.SelectedValue = (common.myBool(DR["Active"]) == true) ? "1" : "0";
                    chkPanelpriceRequired.Checked = common.myBool(DR["PanelpriceRequired"]);
                    chkReusable.Checked = common.myBool(DR["Reusable"]);
                    txtVatOnSale.Text = common.myStr(DR["VatOnSale"]);
                    chkConsumable.Checked = common.myBool(DR["Consumable"]);
                    chkUseforbplpatient.Checked = common.myBool(DR["Useforbplpatient"]);

                    txtDepreciationDays.Text = common.myStr(DR["DepreciationDays"]);
                    txtDepreciationPerc.Text = common.myStr(DR["DepreciationPercentage"]);
                    txtDose.Text = (common.myInt(DR["DrugDose"]) > 0) ? common.myInt(DR["DrugDose"]).ToString() : string.Empty;
                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(DR["UnitId"]).ToString()));
                    ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(DR["FormulationId"]).ToString()));
                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(DR["RouteId"]).ToString()));

                    if (chkPanelpriceRequired.Checked)
                    {
                        lnkPanelPriceRequired.Visible = true;
                    }
                    else
                    {
                        lnkPanelPriceRequired.Visible = false;
                    }
                    if (chkReusable.Checked)
                    {
                        lnkReusable.Visible = true;
                    }
                    else
                    {
                        lnkReusable.Visible = false;
                    }
                    //
                    Stream strm;
                    Object img = DR["ItemImage"];
                    string FileName = common.myStr(DR["ItemImageName"]).Trim();
                    if (FileName != "")
                    {
                        strm = new MemoryStream((byte[])img);
                        byte[] buffer = new byte[strm.Length];
                        int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                        FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/PatientImages/" + FileName), FileMode.Create, FileAccess.Write);
                        fs.Write(buffer, 0, byteSeq);
                        fs.Close();
                        fs.Dispose();
                        PatientImage.ImageUrl = "/PatientDocuments/PatientImages/" + FileName;
                    }

                    bindItemUnit();
                    bindCharge();
                    bindItemCurrentStock();
                    bindAdditionalField();
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

    protected void btnRemove_Click(Object sender, EventArgs e)
    {
        PatientImage.ImageUrl = "~/Images/logo/ImageNotAvailable.jpg";
        ViewState["iMageName"] = "";
    }

    protected void Upload_OnClick(Object sender, EventArgs e)
    {
        try
        {
            string sFileName = "";
            string sFileExtension = "";
            string sSavePath = "/PatientDocuments/PatientImages/";

            StringBuilder objStr = new StringBuilder();
            if (fUpload1.FileName != "" || fUpload1.PostedFile != null)
            {
                //DeleteFiles();
                HttpPostedFile myFile = fUpload1.PostedFile;
                int nFileLen = myFile.ContentLength;
                if (nFileLen == 0)
                {
                    lblMessage.Text = "Error: The file size is zero.";
                    return;
                }
                // Read file into a data stream
                byte[] myData = new Byte[nFileLen];
                myFile.InputStream.Read(myData, 0, nFileLen);
                sFileExtension = "";
                sFileExtension = System.IO.Path.GetExtension(myFile.FileName).ToLower();

                sFileName = fUpload1.FileName.Replace(" ", "_").ToLower(); //+ System.IO.Path.GetExtension(myFile.FileName).ToLower();
                System.IO.FileStream newFile = new System.IO.FileStream(Server.MapPath(sSavePath + sFileName), System.IO.FileMode.Create);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();

                ViewState["iMageName"] = sFileName;
                PatientImage.ImageUrl = "/PatientDocuments/PatientImages/" + sFileName;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void DeleteFiles()
    {
        try
        {
            string strPatientImageid = "";
            DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/PatientImages"));
            if (objDir.Exists == true)
            {
                FileInfo[] fi_array = objDir.GetFiles();
                foreach (FileInfo files in fi_array)
                {
                    if (files.Exists)
                    {
                        strPatientImageid = Path.GetFileNameWithoutExtension(Server.MapPath("/PatientDocuments/PatientImages/") + files);
                        if (Convert.ToString(Session["RegistrationId"]) == strPatientImageid)
                        {
                            File.Delete(Server.MapPath("/PatientDocuments/PatientImages/") + files);
                        }
                    }
                }
                PatientImage.ImageUrl = "/Images/patient.jpg";
            }
            else
            {
                objDir.Create();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void lnkItemFlag_OnClick(object Sender, EventArgs e)
    {
        Response.Redirect("/Pharmacy/ItemFlagTagging.aspx?ItemId=" + common.myStr(common.myInt(ViewState["_ID"])), false);
    }

    protected void lnkItemSupplier_OnClick(object Sender, EventArgs e)
    {
        Response.Redirect("/Pharmacy/ItemSupplierTagging.aspx?ItemId=" + common.myStr(common.myInt(ViewState["_ID"])), false);
    }
    protected void lnkPanelPriceRequired_OnClick(object Sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/Pharmacy/PanelPriceRequired.aspx?Master=I&ItemId=" + hdnItemID.Value;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Width = 650;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
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

    protected void ibtnAdditionalFields_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/Pharmacy/ItemMasterAdditionalfields.aspx?Master=I";
            RadWindowForNew.Height = 430;
            RadWindowForNew.Width = 450;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientAdditionalFieldClose";
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

    protected void OnClientAdditionalFieldClose_OnClick(object sender, EventArgs e)
    {
        bindAdditionalField();
    }

    protected void gvAdditionalField_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                Label lblFieldType = (Label)e.Item.FindControl("lblFieldType");

                switch (common.myStr(lblFieldType.Text))
                {
                    case "N":
                        TextBox txtN = (TextBox)e.Item.FindControl("txtN");
                        Label lblDecimalPlaces = (Label)e.Item.FindControl("lblDecimalPlaces");
                        txtN.Visible = true;
                        txtN.Text = common.myDbl(txtN.Text).ToString("F4");
                        break;
                    case "T":
                        TextBox txtT = (TextBox)e.Item.FindControl("txtT");
                        txtT.Visible = true;
                        break;
                    case "M":
                        TextBox txtM = (TextBox)e.Item.FindControl("txtM");
                        txtM.Visible = true;
                        break;
                    case "D":
                        Label lblGroupId = (Label)e.Item.FindControl("lblGroupId");
                        Label lblValueId = (Label)e.Item.FindControl("lblValueId");
                        RadComboBox ddlD = (RadComboBox)e.Item.FindControl("ddlD");
                        ddlD.Visible = true;

                        objPharmacy = new BaseC.clsPharmacy(sConString);
                        DataSet ds = objPharmacy.getGroupValueDetails(common.myInt(lblGroupId.Text), 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));

                        ddlD.DataSource = ds.Tables[0];
                        ddlD.DataTextField = "ValueName";
                        ddlD.DataValueField = "ValueId";
                        ddlD.DataBind();

                        ddlD.Items.Insert(0, new RadComboBoxItem("", "0"));

                        if (common.myInt(lblValueId.Text) == 0)
                        {
                            DataView DV = ds.Tables[0].Copy().DefaultView;
                            DV.RowFilter = "IsDefault = 1";

                            DataTable tbl = DV.ToTable();

                            ddlD.SelectedIndex = 0;
                            if (tbl.Rows.Count > 0)
                            {
                                lblValueId.Text = common.myStr(tbl.Rows[0]["ValueId"]);
                                ddlD.SelectedIndex = ddlD.Items.IndexOf(ddlD.Items.FindItemByValue(common.myStr(tbl.Rows[0]["ValueId"])));
                            }
                            else
                            {
                                ddlD.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            ddlD.SelectedIndex = ddlD.Items.IndexOf(ddlD.Items.FindItemByValue(common.myStr(lblValueId.Text)));
                        }

                        break;
                    case "W":
                        RadEditor txtW = (RadEditor)e.Item.FindControl("txtW");
                        txtW.Visible = true;
                        break;
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

    private void bindCharge()
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = objPharmacy.getItemChargeDetails(common.myInt(ViewState["_ID"]), false);

            gvTax.DataSource = ds.Tables[0];
            gvTax.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void bindItemCurrentStock()
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = objPharmacy.getItemCurrentStock(common.myInt(ViewState["_ID"]), common.myInt(Session["FacilityId"]));

            gvItemCurrentStock.DataSource = ds.Tables[0];
            gvItemCurrentStock.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvTax_PreRender(object sender, EventArgs e)
    {
        if (RowSelStautsCharge)
        {
            bindCharge();
        }
    }

    protected void gvItemUnit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Find the checkbox control in header and add an attribute
            ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAllItemUnit('" +
                ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
        }
    }

    //protected void ibtnNewItemUnit_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        RadWindowForNew.NavigateUrl = "~/Pharmacy/ItemUnitMaster.aspx?MasterPage=No";
    //        RadWindowForNew.Height = 600;
    //        RadWindowForNew.Width = 850;
    //        RadWindowForNew.Top = 40;
    //        RadWindowForNew.Left = 100;
    //        RadWindowForNew.Title = "Item Unit Master";
    //        RadWindowForNew.OnClientClose = "OnClientItemUnitClose";
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

    protected void btnItemUnitClose_OnClick(object sender, EventArgs e)
    {
        bindItemUnit();
    }

    protected void ddlRequestedFacility_OnSelectedIndexChanged(object o, EventArgs e)
    {
        bindRecommendedBy();
    }

    private void bindRecommendedBy()
    {
        try
        {
            BaseC.clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);
            DataTable tbl = objLISMaster.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(ddlRequestedFacility.SelectedValue), 0);

            ddlRecommendedBy.DataSource = tbl;
            ddlRecommendedBy.DataTextField = "DoctorName";
            ddlRecommendedBy.DataValueField = "DoctorId";
            ddlRecommendedBy.DataBind();

            ddlRecommendedBy.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlRecommendedBy.SelectedIndex = 0;
        }
        catch { }
    }

    protected void gvItemCurrentStock_PreRender(object sender, EventArgs e)
    {
        if (RowSelStautsCurrentStock)
        {
            bindItemCurrentStock();
        }
    }
    protected void lnkreusable_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/Pharmacy/ReusableItems.aspx?Master=I&ItemId=" + hdnItemID.Value;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
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

    protected void ddlFormulation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlFormulation.SelectedValue) > 0)
            {
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(ddlFormulation.SelectedItem.Attributes["DefaultRouteId"]).ToString()));
            }
            else
            {
                ddlRoute.Text = string.Empty;
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));
            }
        }
        catch
        {
        }
    }

}
