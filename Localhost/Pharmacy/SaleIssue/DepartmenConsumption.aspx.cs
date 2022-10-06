using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using BaseC;
using System.IO;
using System.Configuration;
using Telerik.Web.UI;

public partial class Pharmacy_DepartmenConsumption : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    BaseC.clsEMRBilling baseEBill;
    StringBuilder objXML;
    ArrayList coll;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.HospitalSetup objHospitalSetup;//my
    bool RowSelStauts = false;
    string IsUserAllowAuthentication = "";
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).Equals("Ward"))
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ddlSearchOn.SelectedValue = "1";
        ddlSearchOn.Enabled = false;
        if (common.myStr(Request.QueryString["From"]).Equals("Ward"))
        {
            txtRegistrationNo.Enabled = false;
        }
        else
        {
            txtRegistrationNo.Enabled = true;
        }


        if (Request.QueryString["EncNo"] != null && !common.myStr(Request.QueryString["EncNo"]).Equals(string.Empty))
        {
            txtRegistrationNo.Text = common.myStr(Request.QueryString["EncNo"]);
            BindPatientHiddenDetails(common.myInt(txtRegistrationNo.Text));
        }
        if (common.myStr(Request.QueryString["From"]).Equals("Ward"))
        {
            ibtnFind.Visible = false;
        }
        //if (common.myInt(Session["StoreId"]) == 0)
        //{
        //    Response.Redirect("/Pharmacy/ChangeStore.aspx?Mpg=P335&PT=" + common.myStr(Request.Url.PathAndQuery), false);
        //}
        IsUserAllowAuthentication = "";
        objHospitalSetup = new BaseC.HospitalSetup(sConString);
        IsUserAllowAuthentication = common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), "UserAuthenticationForInventory", common.myInt(Session["FacilityId"])));

        if (!IsPostBack)
        {
            Session["DupDeptConsReqCheck"] = 0;
            //done by rakesh for user authorisation start
            SetPermission();
            //done by rakesh for user authorisation end

            objPharmacy = new BaseC.clsPharmacy(sConString);

            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            if (common.myInt(hdnDecimalPlaces.Value) == 0)
            {
                hdnDecimalPlaces.Value = "2";
            }
            //Cache.Remove("DeptConsumptionItem_" + common.myStr(hdnUniqueSessionId.Value));
            hdnUniqueSessionId.Value = common.uniqueSessionId();
            dtpDocDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            dtpDocDate.SelectedDate = DateTime.Now;

            txtFacilityName.Text = common.myStr(Session["FacilityName"]);
            txtDepartmentName.Text = common.myStr(Session["StoreName"]);
            gvService.DataSource = CreateTable();
            gvService.DataBind();
            //lbtnSearchAllDoc.Attributes.Add("onclick", "openRadWindow('../SaleIssue/DepartmentConsumptionList.aspx?DecimalPlaces=" + hdnDecimalPlaces.Value + "','GRN');return false;");

            if (Request.QueryString["docno"] != null)
            {
                txtDocNo.Text = common.myStr(Request.QueryString["docno"]);
                btnSearchByDocNo_OnClick(this, null);
            }
            else
            {
                //done by rakesh for user authorisation start
                //btnSaveAndPost.Visible = false;
                SetPermission(btnSaveAndPost, false);
                //btnCancel.Visible = false;
                SetPermission(btnCancel, false);
                //done by rakesh for user authorisation end
            }
            string IsShowPostingDate = common.myStr(objBill.getHospitalSetupValue("PostingDateInConsumptionVisible", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            if (common.myStr(IsShowPostingDate) == "Y")
            {
                lblPostingDate.Visible = true;
                RdlPostingDate.Visible = true;
                RdlPostingDate.MaxDate = DateTime.Now;
            }
            BindSearchCombo();
            BindStore();

        }
    }
    private void BindSearchCombo()
    {
        try
        {
            objHospitalSetup = new BaseC.HospitalSetup(sConString);//my
            if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IncludeOPBillinCombo", common.myInt(Session["FacilityId"]))) == "Y")//my
            {
                ddlSearchOn.Items.Insert(2, new RadComboBoxItem("OP Bill", "2"));
            }
            else
            {

            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    #region Page Controls

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Session["DupDeptConsReqCheck"] = 0;
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }
    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        ViewState["ButtonClick"] = "SAVE";

        //if (common.myStr(ddlStore.SelectedValue).Equals("0") || common.myStr(ddlStore.SelectedValue).Equals(string.Empty))
        //{
        //    lblMessage.Text = " Please select store !";
        //    ddlStore.Focus();
        //    return;
        //}

        if (IsUserAllowAuthentication == "Y")
        {
            IsValidPassword();
        }
        else
        {
            hdnIsValidPassword.Value = "1";
            btnIsValidPasswordClose_OnClick(sender, e);
        }
        //if (Save() == true)
        //{

        //}
    }

    protected void btnAddNewItem_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItem.aspx?DeptIssue=DI&StoreId=" + common.myStr(ddlStore.SelectedValue);
            RadWindow1.Height = 600;
            RadWindow1.Width = 990;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "wndAddService_OnClientClose";
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.Behaviors = WindowBehaviors.Close | WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Pin;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
            Session["DupDeptConsReqCheck"] = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    #endregion

    private void clearControl()
    {
        hdnTotCharge.Value = "0";
        hdnTotDiscAmt.Value = "0";
        hdnTotPatientAmt.Value = "0";
        hdnTotPayerAmt.Value = "0";
        hdnTotNetAmt.Value = "0";
        hdnTotQty.Value = "0";
        hdnTotUnit.Value = "0";
        hdnTotTax.Value = "0";
        txtNetAmount.Text = "0.00"; ;
        txtRemarks.Text = "";
        dtpDocDate.SelectedDate = null;
        ViewState["Servicetable"] = "";
    }

    protected void gvService_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtQty = (TextBox)e.Row.FindControl("txtQty");
                //if (common.myStr(txtDocNo.Text) != "")
                //{
                //   gvService.Columns[10].Visible = false;
                //    e.Row.Cells[10].Visible = false;
                //    ImageButton ibtndaDelete = (ImageButton)e.Row.FindControl("ibtndaDelete");
                //    ibtndaDelete.Visible = false;
                //    txtQty.Enabled = false;
                //}
                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#5EA0F4";
                }
                HiddenField hdnUnitId = (HiddenField)e.Row.FindControl("hdnUnitId");
                Label lblItemUnitName = (Label)e.Row.FindControl("lblItemUnitName");


                TextBox txtCostPrice = (TextBox)e.Row.FindControl("txtCostPrice");
                TextBox txtSellingPrice = (TextBox)e.Row.FindControl("txtSellingPrice");
                TextBox txtMRP = (TextBox)e.Row.FindControl("txtMRP");

                TextBox txtNetAmt = (TextBox)e.Row.FindControl("txtNetAmt");
                HiddenField hdnStockQty = (HiddenField)e.Row.FindControl("hdnStockQty");



                hdnStockQty.Value = common.myDbl(hdnStockQty.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtQty.Text = common.myStr(common.myDbl(txtQty.Text));// ("F" + common.myInt(hdnDecimalPlaces.Value));
                txtCostPrice.Text = common.myDbl(txtCostPrice.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtSellingPrice.Text = common.myDbl(txtSellingPrice.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtNetAmt.Text = common.myDbl(txtNetAmt.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));


                hdnTotCharge.Value = common.myStr(common.myDbl(hdnTotCharge.Value) + common.myDbl(txtCostPrice.Text));
                hdnTotDiscAmt.Value = common.myStr(common.myDbl(hdnTotDiscAmt.Value) + common.myDbl(txtMRP.Text));
                hdnTotNetAmt.Value = common.myStr(common.myDbl(hdnTotNetAmt.Value) + common.myDbl(txtNetAmt.Text));
                hdnTotQty.Value = common.myStr(common.myDbl(hdnTotQty.Value) + common.myDbl(txtQty.Text));
                txtCostPrice.Enabled = false;
                HiddenField hdnItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                //BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
                //if (common.myInt(hdnItemId.Value) != 0)
                //{
                //    DataSet ds = new DataSet();
                //    ds = objPharmacy.GetUnitIssueUnit(common.myInt(hdnItemId.Value));
                //    if (ds.Tables.Count > 0)
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            lblItemUnitName.Text = ds.Tables[0].Rows[0]["IssueUnitName"].ToString();
                //            hdnUnitId.Value = ds.Tables[0].Rows[0]["IssueUnitId"].ToString();
                //        }
                //    }
                //}
                txtQty.Attributes.Add("onkeyup", "javascript:chekQty('" + hdnStockQty.ClientID + "','" + txtQty.ClientID + "','" + txtCostPrice.ClientID + "','" + txtNetAmt.ClientID + "' );");

                ImageButton ibtndaDelete = (ImageButton)e.Row.FindControl("ibtndaDelete");
                if (common.myStr(hdnProcessStatus.Value) == "POST" || common.myStr(hdnProcessStatus.Value) == "CANCEL")
                {
                    ibtndaDelete.Enabled = false;
                    ibtndaDelete.ToolTip = "You Cannot delete item...";
                }
                //if (txtDocNo.Text != "")
                //{
                //    txtQty.Enabled = false;
                //    //btnSaveAndPost.Visible = true;
                //    //btnSaveData.Visible = false;
                //    //btnEdit.Visible = true;
                //}
                //else
                //{
                //    txtQty.Enabled = true;
                //    //btnSaveAndPost.Visible = false;
                //    //btnSaveData.Visible = true;
                //    //btnEdit.Visible = false;
                //}
                if (btnSaveData.Visible == false)
                {
                    txtQty.Enabled = false;
                }
                else
                {
                    txtQty.Enabled = true;
                }
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#104E8B";
                }
                TextBox txtTotQty = (TextBox)e.Row.FindControl("txtTotQty");
                TextBox txtTotalNetAmt = (TextBox)e.Row.FindControl("txtTotNetamt");
                txtTotQty.Text = common.myStr(hdnTotQty.Value);
                txtTotalNetAmt.Text = common.FormatNumber(common.myStr(hdnTotNetAmt.Value), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"])); ;

                txtNetAmount.Text = common.FormatNumber(common.myStr(hdnTotNetAmt.Value), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                txtTotQty.Enabled = false;
                txtTotalNetAmt.Enabled = false;
                HtmlInputButton btnCalculate = (HtmlInputButton)e.Row.FindControl("btnCalculate");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Del")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                if (common.myInt(((HiddenField)row.FindControl("hdnConsumptionDetailsId")).Value) > 0)
                {
                    BaseC.clsDepartmentConsumption objDeptCon = new BaseC.clsDepartmentConsumption(sConString);
                    Hashtable htOut = new Hashtable();
                    objDeptCon.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                    objDeptCon.UserId = common.myInt(Session["UserId"]);
                    objDeptCon.ConsumptionId = 0;
                    objDeptCon.ConsumptionDetailId = common.myInt(((HiddenField)row.FindControl("hdnConsumptionDetailsId")).Value);
                    objDeptCon.sProc = "uspCancelPhrDepartmentConsumption";
                    htOut = objDeptCon.CancelDepartmentConsumption(objDeptCon);
                    if (common.myStr(htOut["chvErrorStatus"]).Contains("Cancelled"))
                    {
                        lblMessage.Text = common.myStr("Record Deleted ");
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        btnSearchByDocNo_OnClick(sender, e);

                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = common.myStr(htOut["chvErrorStatus"]);

                    }
                }
                else
                {

                    if (e.CommandArgument != "")
                    {
                        int intId = Convert.ToInt32(e.CommandArgument);
                        if (intId != 0)
                        {
                            DataTable dt = new DataTable();
                            // dt = (DataTable)Cache["DeptConsumptionItem_" + common.myStr(hdnUniqueSessionId.Value)];
                            dt = (DataTable)ViewState["Servicetable"];

                            dt.Rows.RemoveAt(row.RowIndex);
                            if (dt.Rows.Count > 0)
                            {
                                gvService.DataSource = dt;
                            }
                            else
                            {
                                gvService.DataSource = CreateTable();
                            }
                            hdnTotCharge.Value = "0";
                            hdnTotDiscAmt.Value = "0";
                            hdnTotPatientAmt.Value = "0";
                            hdnTotPayerAmt.Value = "0";
                            hdnTotNetAmt.Value = "0";
                            hdnTotQty.Value = "0";
                            hdnTotUnit.Value = "0";
                            hdnTotTax.Value = "0";
                            gvService.DataBind();
                            ViewState["Servicetable"] = dt;
                            // Cache.Insert("DeptConsumptionItem_" + common.myStr(hdnUniqueSessionId.Value), dt, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                            //-------
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

    protected DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 1;
        dt.Columns["SNo"].AutoIncrementStep = 1;
        dt.Columns.Add("Id");
        dt.Columns.Add("ItemId");
        dt.Columns.Add("ItemName");
        dt.Columns.Add("BatchId");
        dt.Columns.Add("BatchNo");
        dt.Columns.Add("StockQty");
        dt.Columns.Add("Qty");
        dt.Columns.Add("CostPrice");
        dt.Columns.Add("SalePrice");
        dt.Columns.Add("MRP");
        dt.Columns.Add("NetAmt");
        dt.Columns.Add("SaleTaxPercent");

        DataRow dr = dt.NewRow();
        dr["Id"] = "0";
        dr["ItemId"] = DBNull.Value;
        dr["ItemName"] = DBNull.Value;
        dr["BatchId"] = DBNull.Value;
        dr["BatchNo"] = DBNull.Value;
        dr["StockQty"] = DBNull.Value;
        dr["Qty"] = DBNull.Value;
        dr["CostPrice"] = DBNull.Value;
        dr["SalePrice"] = DBNull.Value;
        dr["MRP"] = DBNull.Value;
        dr["NetAmt"] = DBNull.Value;
        dr["SaleTaxPercent"] = DBNull.Value;

        dt.Rows.Add(dr);
        // Cache.Insert("DeptConsumptionItem_" + common.myStr(hdnUniqueSessionId.Value), dt, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);

        return dt;
    }

    protected DataTable CreateTable1()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 1;
        dt.Columns["SNo"].AutoIncrementStep = 1;

        dt.Columns.Add("Id");
        dt.Columns.Add("ItemId");
        dt.Columns.Add("ItemName");
        dt.Columns.Add("BatchId");
        dt.Columns.Add("BatchNo");
        dt.Columns.Add("StockQty");
        dt.Columns.Add("Qty");
        dt.Columns.Add("CostPrice");
        dt.Columns.Add("SalePrice");
        dt.Columns.Add("MRP");
        dt.Columns.Add("NetAmt");
        dt.Columns.Add("SaleTaxPercent");
        return dt;
    }

    protected void btnBindGridWithXml_OnClick(object sender, EventArgs e)
    {
        BindGridWithXml();
    }

    void BindGridWithXml()
    {
        try
        {
            if (common.myStr(hdnxmlString.Value) != "")
            {
                hdnTotCharge.Value = "0";
                hdnTotDiscAmt.Value = "0";
                hdnTotPatientAmt.Value = "0";
                hdnTotPayerAmt.Value = "0";
                hdnTotNetAmt.Value = "0";
                hdnTotQty.Value = "0";
                hdnTotUnit.Value = "0";
                hdnTotTax.Value = "0";

                string xmlSchema = common.myStr(hdnxmlString.Value);
                StringReader sr = new StringReader(xmlSchema);
                DataSet dsXml = new DataSet();
                DataTable dtXml = new DataTable();
                dsXml.ReadXml(sr);
                if (dsXml.Tables.Count > 0)
                {
                    if (dsXml.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = new DataView(dsXml.Tables[0]);
                        dv.RowFilter = "BatchId > 0";
                        dtXml = dv.ToTable();
                    }
                    else
                        return;
                }
                else
                    return;
                //---------------------------------------
                DataTable dtPreviousServices = new DataTable();
                if (common.myStr(ViewState["Servicetable"]) == "")
                    dtPreviousServices = CreateTable1();
                else
                {
                    dtPreviousServices = ((DataTable)ViewState["Servicetable"]);
                }
                if (dtPreviousServices.Rows.Count > 0)// != null)
                {
                    //Check duplicate Item and remove start------------------------------------------------------------------
                    List<DataRow> rowsToRemove = new List<DataRow>();
                    for (int i = 0; i < dtPreviousServices.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtXml.Rows.Count; j++)
                        {
                            if (common.myStr(dtXml.Rows[j]["BatchNo"]).ToUpper() == common.myStr(dtPreviousServices.Rows[i]["BatchNo"]).ToUpper()
                                && common.myStr(dtXml.Rows[j]["ItemId"]).ToUpper() == common.myStr(dtPreviousServices.Rows[i]["ItemId"]).ToUpper())
                            {
                                rowsToRemove.Add(dtXml.Rows[j]);
                            }
                        }
                    }

                    foreach (var dr in rowsToRemove)
                    {
                        dtXml.Rows.Remove(dr);
                    }
                }
                dtXml.AcceptChanges();
                //Check duplicate Item and remove end-------------------------------------------------------------------------

                if (dtXml.Rows.Count == 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Item already exits!";
                    return;
                }
                for (int i = 0; i < dtXml.Rows.Count; i++)
                {
                    DataRow dr = dtPreviousServices.NewRow();
                    dr["ItemId"] = common.myStr(dtXml.Rows[i]["ItemId"]);
                    dr["ItemName"] = common.myStr(dtXml.Rows[i]["ItemName"]);
                    dr["BatchId"] = common.myStr(dtXml.Rows[i]["BatchId"]);
                    dr["BatchNo"] = common.myStr(dtXml.Rows[i]["BatchNo"]);
                    dr["StockQty"] = common.myStr(dtXml.Rows[i]["StockQty"]);
                    dr["Qty"] = common.myStr(dtXml.Rows[i]["Qty"]);
                    dr["CostPrice"] = common.myStr(dtXml.Rows[i]["CostPrice"]);
                    dr["SalePrice"] = common.myStr(dtXml.Rows[i]["SalePrice"]);
                    dr["MRP"] = common.myStr(dtXml.Rows[i]["MRP"]);
                    dr["NetAmt"] = common.myStr(dtXml.Rows[i]["NetAmt"]);
                    dr["SaleTaxPercent"] = common.myStr(dtXml.Rows[i]["Tax"]);
                    dr["Id"] = "0";
                    dtPreviousServices.Rows.Add(dr);
                }

                //----------------------------------------
                if (dtPreviousServices.Rows.Count > 0)
                {
                    DataView dvRecord = dtPreviousServices.DefaultView;
                    dvRecord.RowFilter = "ISNULL(ItemId, 0) <> 0";
                    //dvRecord.RowFilter = "itemid <>''";

                    dtPreviousServices = new DataTable();
                    dtPreviousServices = dvRecord.ToTable();
                }
                ViewState["Servicetable"] = dtPreviousServices;
                gvService.DataSource = dtPreviousServices;
                gvService.DataBind();
                hdnxmlString.Value = "";
            }
            else
            {
                if (common.myStr(ViewState["Servicetable"]) == "")
                {
                    gvService.DataSource = CreateTable();
                    gvService.DataBind();
                }
                lblMessage.Text = "";
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

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {

        if (common.myInt(hdnConsumptionId.Value) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Document to Print !";
            return;
        }

        RadWindow1.NavigateUrl = "~/Pharmacy/Reports/PrintReport.aspx?rptType=DepCon&ConId=" + common.myInt(hdnConsumptionId.Value);
        RadWindow1.Height = 600;
        RadWindow1.Width = 800;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        // RadWindowForNew.Title = "Time Slot";
        //RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;

    }
    protected void btnEdit_OnClick(object sender, EventArgs e)
    {


        foreach (GridViewRow gvrow in gvService.Rows)
        {
            ((TextBox)gvrow.FindControl("txtQty")).Enabled = true;
        }
        btnAddNewItem.Visible = true;
        //done by rakesh for user authorisation start
        //btnSaveAndPost.Visible = false;
        SetPermission(btnSaveAndPost, false);
        //btnSaveData.Visible = true;
        SetPermission(btnSaveData, "N", true);
        //done by rakesh for user authorisation end
        btnEdit.Visible = false;

    }
    protected void btnSearchByDocNo_OnClick(object sender, EventArgs e)
    {
        try
        {
            clearControl();
            string docno = common.myStr(txtDocNo.Text);
            if (docno == "")
            {
                return;
            }
            //else
            //{
            //    btnSaveData.Visible = false;
            //    btnEdit.Visible = true;
            //    btnSaveData.Visible = false;
            //}
            DataSet ds = new DataSet();
            BaseC.clsDepartmentConsumption objDepCon = new clsDepartmentConsumption(sConString);
            ds = objDepCon.GetDepartmentConsumptionDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["facilityId"]),
              common.myInt(ddlStore.SelectedValue), docno, common.myStr(Request.QueryString["PS"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                hdnConsumptionId.Value = common.myStr(ds.Tables[0].Rows[0]["ConsumptionId"]);
                txtRemarks.Text = common.myStr(ds.Tables[0].Rows[0]["Remarks"]);
                hdnProcessStatus.Value = common.myStr(ds.Tables[0].Rows[0]["ProcessStatus"]).ToUpper();
                if (common.myStr(ds.Tables[0].Rows[0]["ConsumptionDate"]) != "")
                    dtpDocDate.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["ConsumptionDate"]);

                if (common.myStr(ds.Tables[0].Rows[0]["PostingDate"]) != "")
                    RdlPostingDate.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["PostingDate"]);

                string Posteddate = "", cancelleddate = "";

                if (common.myStr(ds.Tables[0].Rows[0]["CancelledDate"]) != "")
                {
                    cancelleddate = " ON DATE: " + common.myStr(ds.Tables[0].Rows[0]["CancelledDate"]);
                }

                if (common.myInt(ds.Tables[0].Rows[0]["ConsType"]) == 0)
                {
                    ddlSearchOn.SelectedValue = "1";
                }
                else if (common.myInt(ds.Tables[0].Rows[0]["ConsType"]) == 1)
                {
                    ddlSearchOn.SelectedValue = "0";
                }
                else if (common.myInt(ds.Tables[0].Rows[0]["ConsType"]) == 2)
                {
                    ddlSearchOn.SelectedValue = "1";
                }
                else if (common.myInt(ds.Tables[0].Rows[0]["ConsType"]) == 3)
                {
                    ddlSearchOn.SelectedValue = "2";
                }

                if (ddlSearchOn.SelectedValue == "0")
                {
                    txtRegistrationNo.Text = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
                }
                else if (ddlSearchOn.SelectedValue == "1")
                {
                    txtRegistrationNo.Text = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
                }
                else if (ddlSearchOn.SelectedValue == "2")
                {
                    txtRegistrationNo.Text = common.myStr(ds.Tables[0].Rows[0]["InvoiceNo"]);
                }
                if (common.myStr(hdnProcessStatus.Value) == "POST")
                {
                    if (common.myStr(ds.Tables[0].Rows[0]["PostedDate"]) != "")
                    {
                        Posteddate = " ON DATE: " + common.myStr(ds.Tables[0].Rows[0]["PostedDate"]);
                    }
                    //done by rakesh for user authorisation start
                    //btnSave.Enabled = false;
                    //btnSaveData.Enabled = false;
                    //btnSaveAndPost.Enabled = false;
                    //btnCancel.Enabled = false;
                    //btnEdit.Enabled = false;
                    btnAddNewItem.Visible = false;
                    //btnSaveData.Visible = false;
                    SetPermission(btnSaveData, false);
                    //btnSaveAndPost.Visible = false;
                    SetPermission(btnSaveAndPost, false);
                    //btnCancel.Visible = false;
                    SetPermission(btnCancel, false);
                    //btnEdit.Visible = false;
                    SetPermission(btnEdit, false);
                    //done by rakesh for user authorisation end
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "(CONSUMPTION POSTED " + Posteddate + ")";
                    btnAddNewItem.Enabled = false;
                }
                else if (common.myStr(hdnProcessStatus.Value) == "CANCEL")
                {
                    //btnSaveData.Enabled = false;
                    //btnSaveAndPost.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnAddNewItem.Visible = false;
                    //done by rakesh for user authorisation start
                    //btnSaveData.Visible = false;
                    SetPermission(btnSaveData, false);
                    //btnSaveAndPost.Visible = false;
                    SetPermission(btnSaveAndPost, false);
                    //btnCancel.Visible = false;
                    SetPermission(btnCancel, false);
                    //btnEdit.Visible = false;
                    SetPermission(btnEdit, false);
                    //done by rakesh for user authorisation end

                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "(CONSUMPTION CANCELLED " + cancelleddate + ")";
                    btnAddNewItem.Enabled = false;
                }
                else
                {
                    btnAddNewItem.Visible = false;
                    //done by rakesh for user authorisation start
                    //btnSaveData.Visible = false;
                    SetPermission(btnSaveData, false);
                    //btnSaveAndPost.Visible = true;
                    SetPermission(btnSaveAndPost, "N", true);
                    //btnCancel.Visible = true;
                    SetPermission(btnCancel, "C", true);
                    //btnEdit.Visible = true;
                    SetPermission(btnEdit, "E", true);
                    //done by rakesh for user authorisation end
                    lblMessage.Text = "";
                }

                ViewState["Servicetable"] = ds.Tables[0];
                gvService.DataSource = ds.Tables[0];
                gvService.DataBind();
            }
            else
            {
                ViewState["Servicetable"] = ds.Tables[0];
                gvService.DataSource = ds.Tables[0];
                gvService.DataBind();
                lblMessage.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    bool Save()
    {
        try
        {
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            objXML = new StringBuilder();
            coll = new ArrayList();
            Hashtable htOut = new Hashtable();
            if (Convert.ToString(ViewState["Servicetable"]) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Fill Item Details !";
                return false;
            }
            foreach (GridViewRow gvRow in gvService.Rows)
            {
                TextBox txtQty = (TextBox)gvRow.FindControl("txtQty");
                Label lblSno = (Label)gvRow.FindControl("lblSno");
                Label txtItemname = (Label)gvRow.FindControl("txtItemname");
                if (common.myDbl(txtQty.Text) == 0)
                {

                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //lblMessage.Text = "Quantity is not Proper please check !.";
                    lblMessage.Text = "Quantity is zero ! Please check in (SNo: " + lblSno.Text + ", Item: " + txtItemname.Text + ")";
                    txtQty.Focus();
                    return false;
                }
                int Active = 1;
                coll.Add(common.myInt(((HiddenField)gvRow.FindControl("hdnItemId")).Value.Trim()));
                coll.Add(common.myInt(((HiddenField)gvRow.FindControl("hdnBatchId")).Value.Trim()));
                coll.Add(common.myStr(((Label)gvRow.FindControl("lblBatchNo")).Text.Trim()));
                coll.Add(common.myDbl(txtQty.Text));
                coll.Add(common.myDbl(((TextBox)gvRow.FindControl("txtCostPrice")).Text.Trim()));
                coll.Add(common.myDbl(((TextBox)gvRow.FindControl("txtSellingPrice")).Text.Trim()));
                coll.Add(common.myDbl(((TextBox)gvRow.FindControl("txtMRP")).Text.Trim()));
                coll.Add(common.myDbl(((TextBox)gvRow.FindControl("txtTax")).Text.Trim()));
                coll.Add(Active);
                coll.Add(common.myInt(((HiddenField)gvRow.FindControl("hdnConsumptionDetailsId")).Value.Trim()));
                objXML.Append(common.setXmlTable(ref coll));
            }
            if (objXML.ToString() == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Fill Item Details !";
                return false;
            }
            if (!common.myBool(Session["DupDeptConsReqCheck"]))
            {
                Session["DupDeptConsReqCheck"] = 1;
                BaseC.clsDepartmentConsumption objDeptCon = new BaseC.clsDepartmentConsumption(sConString);
                objDeptCon.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objDeptCon.LoginFacilityId = common.myInt(Session["FacilityId"]);
                objDeptCon.StoreId = common.myInt(ddlStore.SelectedValue);
                objDeptCon.UserId = common.myInt(Session["UserId"]);
                objDeptCon.ConsumptionId = common.myInt(hdnConsumptionId.Value);
                objDeptCon.ConsumptionDate = common.myStr(common.myDate(dtpDocDate.SelectedDate));
                objDeptCon.ProcessStatus = "O"; // - Always be Open till modification allowed.
                objDeptCon.Remarks = common.myStr(txtRemarks.Text);
                objDeptCon.EncounterId = common.myInt(ViewState["EncId"]);
                objDeptCon.RegistrationId = common.myInt(ViewState["Regid"]);
                objDeptCon.InvoiceId = common.myInt(ViewState["InvoiceId"]);  //my14112016 
                objDeptCon.PostingDate = common.myStr(common.myDate(RdlPostingDate.SelectedDate));
                objDeptCon.XMLItemDetails = objXML;
                objDeptCon.uniqueNo = objPharmacy.GenerateUniqueNo();
                objDeptCon.sProc = "uspSavePhrDepartmentConsumption";
                htOut = objDeptCon.SaveDepartmentConsumption(objDeptCon);
            }
            else
            {
                return false;
            }
            if (common.myStr(htOut["chvErrorStatus"]).Contains("Saved"))
            {
                txtDocNo.Text = common.myStr(htOut["chvDocumentNo"]);
                hdnConsumptionId.Value = common.myStr(htOut["intDocId"]);
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Record Saved With Document No: " + common.myStr(htOut["chvDocumentNo"]);
                return true;
            }
            else if (common.myStr(htOut["chvErrorStatus"]).Contains("Updated"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Record Updated For Document No: " + common.myStr(txtDocNo.Text);
                return true;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = common.myStr(htOut["chvErrorStatus"]);
                return false;
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ex.Message;
            return false;
        }
    }

    bool Post()
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        objXML = new StringBuilder();
        coll = new ArrayList();
        Hashtable htOut = new Hashtable();
        foreach (GridViewRow gvRow in gvService.Rows)
        {
            HiddenField hdnBatchId = (HiddenField)gvRow.FindControl("hdnBatchId");
            HiddenField hdnItemId = (HiddenField)gvRow.FindControl("hdnItemId");
            Label lblBatchNo = (Label)gvRow.FindControl("lblBatchNo");
            TextBox txtMRP = (TextBox)gvRow.FindControl("txtMRP");
            TextBox txtCostPrice = (TextBox)gvRow.FindControl("txtCostPrice");
            string strExpiryDate = DateTime.Now.ToString(); // dummy
            TextBox txtQty = (TextBox)gvRow.FindControl("txtQty");
            TextBox txtTax = (TextBox)gvRow.FindControl("txtTax");
            int sno = gvRow.RowIndex + 1;
            TextBox txtSellingPrice = (TextBox)gvRow.FindControl("txtSellingPrice");

            if (common.myDbl(txtQty.Text) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Qty Cannot be Zero !";
                txtQty.Focus();
                return false;
            }

            coll.Add(common.myInt(hdnItemId.Value));
            coll.Add(common.myStr(lblBatchNo.Text));
            coll.Add(common.myDbl(txtMRP.Text));
            coll.Add(common.myDbl(txtCostPrice.Text));
            coll.Add(common.myStr(strExpiryDate));
            coll.Add(common.myDbl(txtQty.Text));
            coll.Add(common.myDbl(txtTax.Text));
            coll.Add(common.myInt(sno)); // Sr no for relationship between tables of batch and charges  
            coll.Add(common.myInt(hdnBatchId.Value));
            coll.Add(common.myDbl(txtSellingPrice.Text));
            objXML.Append(common.setXmlTable(ref coll));
        }
        if (!common.myBool(Session["DupDeptConsReqCheck"]))
        {

            Session["DupDeptConsReqCheck"] = 1;
            BaseC.clsDepartmentConsumption objDeptCon = new BaseC.clsDepartmentConsumption(sConString);
            objDeptCon.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objDeptCon.LoginFacilityId = common.myInt(Session["FacilityId"]);
            objDeptCon.StoreId = common.myInt(ddlStore.SelectedValue);
            objDeptCon.UserId = common.myInt(Session["UserId"]);
            objDeptCon.ConsumptionId = common.myInt(hdnConsumptionId.Value);
            objDeptCon.XMLItemDetails = objXML;
            objDeptCon.uniqueNo = objPharmacy.GenerateUniqueNo();
            objDeptCon.sProc = "uspPhrPostDepartmentConsumption";
            htOut = objDeptCon.PostDepartmentConsumption(objDeptCon);
        }
        else
        {
            return false;
        }
        if (common.myStr(htOut["chvErrorStatus"]).Contains(" Posted Sucessfully"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = "Department Consumption No: " + common.myStr(txtDocNo.Text) + " Posted Successfully...";
            //done by rakesh for user authorisation start
            //btnSaveData.Enabled = false;
            SetPermission(btnSaveData, false);
            //btnSaveAndPost.Enabled = false;
            SetPermission(btnSaveAndPost, false);
            //btnCancel.Enabled = false;
            SetPermission(btnCancel, false);
            //done by rakesh for user authorisation end
            btnAddNewItem.Enabled = false;
            return true;
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = common.myStr(htOut["chvErrorStatus"]);
            return false;
        }

    }

    bool Cancel()
    {

        BaseC.clsDepartmentConsumption objDeptCon = new BaseC.clsDepartmentConsumption(sConString);
        Hashtable htOut = new Hashtable();
        if (!common.myBool(Session["DupDeptConsReqCheck"]))
        {

            Session["DupDeptConsReqCheck"] = 1;
            objDeptCon.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objDeptCon.UserId = common.myInt(Session["UserId"]);
            objDeptCon.ConsumptionId = common.myInt(hdnConsumptionId.Value);
            objDeptCon.sProc = "uspCancelPhrDepartmentConsumption";
            htOut = objDeptCon.CancelDepartmentConsumption(objDeptCon);
        }
        else
        {
            return false;
        }
        if (common.myStr(htOut["chvErrorStatus"]).Contains("Cancelled"))
        {
            lblMessage.Text = common.myStr(htOut["chvErrorStatus"]) + " For Document No: " + common.myStr(txtDocNo.Text);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            //done by rakesh for user authorisation start
            //btnSaveData.Enabled = false;
            SetPermission(btnSaveData, false);
            //btnSaveAndPost.Enabled = false;
            SetPermission(btnSaveAndPost, false);
            //btnCancel.Enabled = false;
            SetPermission(btnCancel, false);
            //done by rakesh for user authorisation end
            btnAddNewItem.Enabled = false;
            return true;
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = common.myStr(htOut["chvErrorStatus"]);
            return false;
        }

    }

    protected void btnSaveAndPost_OnClick(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        if (common.myStr(hdnProcessStatus.Value) == "POST")
        {
            lblMessage.Text = "Department Consumption Already Posted...";
            return;
        }
        else if (common.myStr(hdnProcessStatus.Value) == "CANCEL")
        {
            lblMessage.Text = "Department Consumption Cancelled...";
            return;
        }
        ViewState["ButtonClick"] = "POST";
        // Rahul Goyal

        if (IsUserAllowAuthentication == "Y")
        {
            IsValidPassword();
        }
        else
        {
            hdnIsValidPassword.Value = "1";
            btnIsValidPasswordClose_OnClick(sender, e);

        }
        //if (Post() == true)
        //{

        //}
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {


        if (txtDocNo.Text == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Document No to Cancel...";
            return;
        }
        ViewState["ButtonClick"] = "CANCEL";
        // Rahul Goyal

        if (IsUserAllowAuthentication == "Y")
        {
            IsValidPassword();
        }
        else
        {
            hdnIsValidPassword.Value = "1";
            btnIsValidPasswordClose_OnClick(sender, e);

        }
    }

    protected void lbtnSearchAllDoc_OnClick(object sender, EventArgs e)
    {
        //string OpIp = rbOpening.Checked == true ? "O" : "A";
        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/DepartmentConsumptionList.aspx?DecimalPlaces=" + hdnDecimalPlaces.Value + "&StoreId=" + common.myStr(ddlStore.SelectedValue);
        RadWindow1.Height = 550;
        RadWindow1.Width = 950;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "Search_OnClientClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.Behaviors = WindowBehaviors.Close | WindowBehaviors.Minimize | WindowBehaviors.Maximize | WindowBehaviors.Pin;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
        Session["DupDeptConsReqCheck"] = 0;
    }

    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";

        //RadWindow1.NavigateUrl = "/Pharmacy/Components/PasswordChecker.aspx?UseFor=GRN";
        RadWindow1.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=GRN";
        RadWindow1.Height = 120;
        RadWindow1.Width = 340;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(hdnIsValidPassword.Value) == 0)
            {
                lblMessage.Text = "Invalid Password !";
                return;
            }

            switch (common.myStr(ViewState["ButtonClick"]))
            {
                case "SAVE":
                    Session["DupDeptConsReqCheck"] = 0;
                    if (Save() == true)
                    {
                        // Cache.Remove("DeptConsumptionItem_" + common.myStr(hdnUniqueSessionId.Value));
                        btnSearchByDocNo_OnClick(null, null);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
                        //dvConfirm.Visible = true;
                    }
                    break;
                case "CANCEL":
                    Session["DupDeptConsReqCheck"] = 0;
                    if (Cancel() == true)
                    {
                        //Cache.Remove("DeptConsumptionItem_" + common.myStr(hdnUniqueSessionId.Value));
                        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
                        btnSearchByDocNo_OnClick(null, null);
                    }
                    break;
                case "POST":
                    Session["DupDeptConsReqCheck"] = 0;
                    if (Post() == true)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
                        //Cache.Remove("DeptConsumptionItem_" + common.myStr(hdnUniqueSessionId.Value));
                        btnSearchByDocNo_OnClick(null, null);
                        //dvConfirm.Visible = true;
                    }
                    break;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //done by rakesh for user authorisation start

    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnSaveData, false);
        ua1.DisableEnableControl(btnSaveAndPost, false);
        ua1.DisableEnableControl(btnCancel, false);
        ua1.DisableEnableControl(btnNew, false);
        ua1.DisableEnableControl(btnPrint, false);

        if (ua1.CheckPermissions("N", Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnSaveData, true);
            ua1.DisableEnableControl(btnNew, true);
            ua1.DisableEnableControl(btnSaveAndPost, true);
        }
        if (ua1.CheckPermissions("C", Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnCancel, true);
        }
        if (ua1.CheckPermissions("P", Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnPrint, true);
        }
        ua1.Dispose();
    }
    private void SetPermission(Button btnID, string mode, bool action)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnID, false);

        if (ua1.CheckPermissions(mode, Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnID, action);
        }
        else
        {
            ua1.DisableEnableControl(btnID, !action);
        }
        ua1.Dispose();
    }
    private void SetPermission(Button btnID, bool action)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnID, action);
        ua1.Dispose();
    }
    //done by rakesh for user authorisation end
    protected void btnSearchByUHID_OnClick(object sender, EventArgs e)
    {
        BindPatientHiddenDetails(common.myInt(txtRegistrationNo.Text));
    }
    void BindPatientHiddenDetails(int Regno)
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.clsEMRBilling CNote = new BaseC.clsEMRBilling(sConString); //my14112016
        int HospId = common.myInt(Session["HospitalLocationID"]);
        int FacilityId = common.myInt(Session["FacilityId"]);
        int RegId = 0;
        string sInvoiceNo = "";
        int UserId = common.myInt(Session["UserId"]), EncounterId = 0;
        int EncodedBy = common.myInt(Session["UserId"]);


        BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
        DataSet ds = new DataSet();

        if (ddlSearchOn.SelectedValue == "1")
        {
            ViewState["InvoiceId"] = null;//my14112016
            ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, 0, UserId, 0, Regno.ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["Regid"] = common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]);
                    ViewState["EncId"] = common.myStr(ds.Tables[0].Rows[0]["EncounterId"]);
                    lblDetail.Text = common.myStr(ds.Tables[0].Rows[0]["PatientName"])
                      + "  /  "
                      + common.myStr(ds.Tables[0].Rows[0]["MobileNo"]);
                }
            }
        }

        else if (ddlSearchOn.SelectedValue == "0")
        {
            ViewState["InvoiceId"] = null;//my14112016
            ds = bC.getPatientDetails(HospId, FacilityId, RegId, Regno, EncounterId, EncodedBy);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["EncId"] = 0;
                    ViewState["Regid"] = common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]);
                    lblDetail.Text = common.myStr(ds.Tables[0].Rows[0]["PatientName"])
                      + "  /  "
                      + common.myStr(ds.Tables[0].Rows[0]["MobileNo"])
                      ;
                }
            }
        }
        else if (ddlSearchOn.SelectedValue == "2")
        {
            ViewState["EncId"] = null; //my14112016
            ViewState["Regid"] = null;//my14112016

            ds = CNote.getPatientDetailsWithInvoiceNo(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), Regno.ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    ViewState["InvoiceId"] = common.myStr(ds.Tables[0].Rows[0]["InvoiceId"]);
                    lblDetail.Text = common.myStr(ds.Tables[0].Rows[0]["PatientName"])
                      + "  /  "
                      + common.myStr(ds.Tables[0].Rows[0]["MobileNo"])
                      ;
                }
            }
        }



    }
    protected void ibtnFind_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlSearchOn.SelectedValue) == 2)
            {
                RadWindow1.NavigateUrl = "/EMRBILLING/AdvanceList.aspx?AR=INVOICE";
                RadWindow1.Height = 600;
                RadWindow1.Width = 990;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                //RadWindow1.OnClientClose = "OnClientCloseSearch";
                RadWindow1.OnClientClose = "SearchPatientOnClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }
            else
            {
                //lbtnSearchPatient.Attributes.Add("onclick", "openRadWindow('/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=1');return false;");
                RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=1&SearchOn=" + common.myInt(ddlSearchOn.SelectedValue) + "";
                RadWindow1.Height = 600;
                RadWindow1.Width = 1000;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.OnClientClose = "SearchPatientOnClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindStore()
    {
        try
        {
            DataSet ds = new DataSet();
            BaseC.clsPharmacy objPharmacy = new clsPharmacy(sConString);

            if (Request.QueryString["Wardid"] != null && !common.myStr(Request.QueryString["Wardid"]).Equals(string.Empty))
            {
                ds = objPharmacy.GetWardStoreTag(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, common.myInt(Request.QueryString["Wardid"]), 3);
            }
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlStore.DataSource = ds;
                    ddlStore.DataTextField = "DepartmentName";
                    ddlStore.DataValueField = "StoreId";
                    ddlStore.DataBind();
                    //ddlStore.Items.Insert(0, new RadComboBoxItem("", "0"));
                    //ddlStore.Items[0].Value = "0";
                    ddlStore.SelectedIndex = 0;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
}
