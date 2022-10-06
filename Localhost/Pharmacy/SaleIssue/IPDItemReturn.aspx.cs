using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using Telerik.Web.UI;
using System.Text;
using System.Configuration;
using BaseC;

public partial class Pharmacy_SaleIssue_IPDItemReturn : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //BaseC.clsPharmacy objPharmacy;

    //clsExceptionLog objException = new clsExceptionLog();
    //private const int ItemsPerRequest = 100;

    private enum GridItemDetails : byte
    {
        IssueDate,
        Item,
        BatchNo,
        MRP,
        DiscPerc,
        IssueQty,
        ReturnedQty,
        BalanceQty,
        OnLineReturnQty,
        ReturnQty,
        EnterReturnQty,
        NetAmt,
        Delete
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("NO")
            && !common.myBool(Request.QueryString["IsFromNurse"]))
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["SaveDuplicate"] = "0";
            //done by rakesh for user authorisation start
            SetPermission();
            //done by rakesh for user authorisation end
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            clsExceptionLog objException = new clsExceptionLog();

            ViewState["SearchFrom"] = "E";
            ViewState["SelectedItemIds"] = "";
            ViewState["GridData"] = null;
            hdnItemId.Value = "0";
            ddlItem.Enabled = true;
            //done by rakesh for user authorisation start
            //btnPrint.Visible = false;
            SetPermission(btnPrint, false);
            //btnSaveData.Visible = true;
            if (common.myStr(Request.QueryString["MASTER"]) != "No")
            {
                SetPermission(btnSaveData, "N", true);
            }
            //btnPostData.Visible = false;             
            SetPermission(btnPostData, false);
            //done by rakesh for user authorisation end

            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);

            ViewState["IsSaleInPackUnit"] = common.myStr(objBill.getHospitalSetupValue("IsSaleInPackUnitWard", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            dvConfirmPrint.Visible = false;

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            clearControl();
            if (common.myStr(Request.QueryString["MASTER"]) == "No")
            {
                //ViewState["StoreId"] = "0";

                btnOpenPatientWnd.Visible = false;
                btnGoReturnNo.Visible = true;
                btnGoReturnNo.Text = "Previous Returns";
                hdnRegistrationId.Value = common.myStr(Session["RegistrationId"]);
                hdnEncounterId.Value = common.myStr(Session["EncounterId"]);
                txtRegistrationNo.Text = common.myStr(Request.QueryString["Regno"]);
                txtEncounterNo.Text = common.myStr(Request.QueryString["Encno"]);

                if (common.myBool(Request.QueryString["IsFromNurse"]))
                {
                    txtRegistrationNo.Text = common.myStr(Session["RegistrationNo"]);
                    txtEncounterNo.Text = common.myStr(Session["EncounterNo"]);
                }

                txtEncounterNo.Enabled = false;
                txtRegistrationNo.Enabled = false;
                txtReturnNo.Enabled = false;
                bindControl();
                ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);
                ViewState["StoreId"] = ddlStore.SelectedValue;
                tblStore.Visible = true;
                //done by rakesh for user authorisation start
                //btnNew.Visible = false;
                SetPermission(btnNew, false);
                //btnPrint.Visible = false;
                SetPermission(btnPrint, false);
                //done by rakesh for user authorisation end
                btnCloseW.Visible = true;

                btnSearchPatient_OnClick(null, null);
            }
            else
            {
                if (common.myInt(Session["StoreId"]) == 0)
                {
                    Response.Redirect("/Pharmacy/ChangeStore.aspx?Mpg=P335&PT=" + Convert.ToString(Page.AppRelativeVirtualPath), false);
                    return;
                }
                ViewState["StoreId"] = common.myInt(Session["StoreId"]);
            }

            ViewState["DefCompId"] = common.myInt(objBill.getDefaultCompany(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));

            bindData();


            if (Request.QueryString["docId"] != null)
            {
                hdnReturnId.Value = common.myStr(Request.QueryString["docId"]);
                fillReturnDetails();
            }

            NoofMarkfordischare();

            hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);
            if (common.myBool(hdnIsPasswordRequired.Value))
                hdnPasswordScreenType.Value = PasswordRequiredHelper.CheckPasswordScreenType(30, "DR", sConString);

            System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

            collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsEMRReasonMandatory", sConString);


            if (collHospitalSetupValues.ContainsKey("IsEMRReasonMandatory"))
                ViewState["IsEMRReasonMandatory"] = common.myStr(collHospitalSetupValues["IsEMRReasonMandatory"]);

            if (common.myStr(ViewState["IsEMRReasonMandatory"]).ToUpper().Equals("Y"))
            {
                dvReason.Visible = true;
                BindReasonMaster();
            }
        }
    }

    protected void ddlStore_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            ViewState["StoreId"] = common.myInt(ddlStore.SelectedValue);
            gvStore.DataSource = BindBlankGrid();
            gvStore.DataBind();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private DataTable BindBlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ItemName");
        dt.Columns.Add("IssueNo");
        dt.Columns.Add("IssuedBy");
        dt.Columns.Add("IndentBy");
        dt.Columns.Add("IndentDate");
        dt.Columns.Add("BatchNo");
        dt.Columns.Add("ItemExpiryDate");
        dt.Columns.Add("MRP");
        dt.Columns.Add("ItemId");
        dt.Columns.Add("BatchId");
        dt.Columns.Add("IssueId");
        dt.Columns.Add("CostPrice");
        dt.Columns.Add("SaleTaxPerc");
        dt.Columns.Add("BalanceQty");
        dt.Columns.Add("OnLineReturnQty");
        dt.Columns.Add("AdvisingDoctorId");
        dt.Columns.Add("EmpAmt");
        dt.Columns.Add("CompAmt");
        dt.Columns.Add("CopayPerc");
        dt.Columns.Add("DiscPerc");
        dt.Columns.Add("Qty");
        dt.Columns.Add("ReturnedQty");
        dt.Columns.Add("IssueDate");
        dt.Columns.Add("ReturnQty");
        dt.Columns.Add("EnterReturnQty");
        dt.Columns.Add("NetAmt");
        dt.Columns.Add("RowNo");
        dt.Columns.Add("IssueDetailsId");
        dt.Columns.Add("ConversionFactor");

        DataRow dr = dt.NewRow();

        dr["ItemName"] = "";
        dr["IssueNo"] = "";
        dr["IssuedBy"] = "";
        dr["IndentBy"] = "";
        dr["IndentDate"] = "";
        dr["BatchNo"] = "";
        dr["ItemExpiryDate"] = "";
        dr["MRP"] = "";
        dr["ItemId"] = "";
        dr["BatchId"] = "";
        dr["IssueId"] = "";
        dr["ItemExpiryDate"] = "";
        dr["CostPrice"] = "";
        dr["SaleTaxPerc"] = "";
        dr["BalanceQty"] = "";
        dr["OnLineReturnQty"] = "";
        dr["AdvisingDoctorId"] = "";
        dr["EmpAmt"] = "";
        dr["CompAmt"] = "";
        dr["CopayPerc"] = "";
        dr["DiscPerc"] = "";
        dr["Qty"] = "";
        dr["ReturnedQty"] = "";
        dr["IssueDate"] = "";
        dr["ReturnQty"] = "";
        dr["EnterReturnQty"] = "";
        dr["NetAmt"] = "";
        dr["RowNo"] = "";
        dr["IssueDetailsId"] = "";
        dr["ConversionFactor"] = "";
        dt.Rows.Add(dr);
        return dt;
    }
    private void bindControl()
    {
        try
        {
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = objPharmacy.GetStoteForWard(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), txtEncounterNo.Text, common.myInt(hdnRegistrationId.Value));

            ddlStore.DataSource = ds.Tables[0];
            ddlStore.DataTextField = "DepartmentName";
            ddlStore.DataValueField = "StoreId";
            ddlStore.DataBind();

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindData()
    {
        try
        {
            //StringBuilder strXML;
            StringBuilder strXMLItemIds;
            ArrayList coll;
            #region ItemIds
            strXMLItemIds = new StringBuilder();
            coll = new ArrayList();

            string allItemIds = common.myStr(ViewState["SelectedItemIds"]).Trim();
            if (allItemIds.Length != 0)
            {
                string[] strItemIds = allItemIds.Split(',');

                foreach (string itemId in strItemIds)
                {
                    coll.Add(common.myInt(itemId));
                    strXMLItemIds.Append(common.setXmlTable(ref coll));
                }
            }

            #endregion

            int fromWard = 0;
            if (common.myStr(Request.QueryString["MASTER"]) == "No")
            {
                fromWard = 1;
            }

            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

            //DataSet ds = objPharmacy.getIPIssueItemDetails(0, common.myStr(txtEncounterNo.Text), common.myInt(ViewState["StoreId"]),
            //                        common.myInt(Session["FacilityID"]), common.myStr(strXMLItemIds.ToString()), common.myInt(Session["FacilityID"]), fromWard);

            DataSet ds = objPharmacy.getIPIssueItemDetails(common.myInt(hdnEncounterId.Value), common.myStr(txtEncounterNo.Text), common.myInt(ViewState["StoreId"]),
                                 common.myInt(Session["FacilityID"]), common.myStr(strXMLItemIds.ToString()), common.myInt(Session["FacilityID"]), fromWard);

            if (ds.Tables.Count > 0)
            {
                if (!ds.Tables[0].Columns.Contains("EnterReturnQty"))
                {
                    ds.Tables[0].Columns.Add(new DataColumn("EnterReturnQty"));
                    ds.Tables[0].AcceptChanges();
                }
                if (!ds.Tables[0].Columns.Contains("ConversionFactor"))
                {
                    ds.Tables[0].Columns.Add(new DataColumn("ConversionFactor"));
                    ds.Tables[0].AcceptChanges();
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    DataRow DR = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(DR);
                }

                int countRow = 0;
                if (gvStore.Rows.Count != 0)
                {
                    foreach (GridViewRow dataItem in gvStore.Rows)
                    {
                        try
                        {
                            TextBox txtReturnQty = (TextBox)dataItem.FindControl("txtReturnQty");
                            ds.Tables[0].Rows[countRow]["EnterReturnQty"] = common.myDbl(txtReturnQty.Text).ToString("F2");
                        }
                        catch
                        {
                        }

                        countRow++;
                        ds.Tables[0].AcceptChanges();
                    }
                }

                gvStore.DataSource = ds.Tables[0];
                gvStore.DataBind();

                ViewState["GridData"] = ds.Tables[0];

                if (common.myStr(ddlItem.Text) != "")
                {
                    DataView DV = ds.Tables[0].DefaultView;
                    DV.RowFilter = "ItemId = " + common.myInt(hdnItemId.Value);

                    if (DV.ToTable().Rows.Count == 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                        lblMessage.Text = "Record not found for Item ( " + common.myStr(ddlItem.Text) + " )";
                    }
                }

                setFocus();
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

    void setFocus()
    {
        for (int i = 0; i < gvStore.Rows.Count - 1; i++)
        {
            TextBox curReturnQty = gvStore.Rows[i].Cells[0].FindControl("txtReturnQty") as TextBox;
            if (gvStore.Rows.Count - 1 >= i + 1)
            {
                TextBox nexReturnQty = gvStore.Rows[i + 1].Cells[0].FindControl("txtReturnQty") as TextBox;
                curReturnQty.Attributes.Add("onkeypress", "return clickEnterInGrid('" + nexReturnQty.ClientID + "', event)");
            }
        }
        if (gvStore.Rows.Count > 0)
        {
            TextBox curReturnQty = gvStore.Rows[gvStore.Rows.Count - 1].Cells[0].FindControl("txtReturnQty") as TextBox;
            curReturnQty.Attributes.Add("onkeypress", "return clickEnterInGrid('" + btnSaveData.ClientID + "', event)");
        }
    }

    private bool isSaved()
    {
        bool isSave = true;
        bool FractionIssue = false;
        string strmsg = "";

        if (common.myInt(hdnEncounterId.Value) == 0 || common.myStr(txtEncounterNo.Text) == "")
        {
            strmsg += "IP. No. cann't be blank !";
            isSave = false;
        }
        if (common.myInt(hdnRegistrationId.Value) == 0 || common.myStr(txtRegistrationNo.Text) == "")
        {
            strmsg += "Registration No. cann't be blank !";
            isSave = false;
        }
        if (common.myInt(ViewState["StoreId"]) == 0)
        {
            strmsg += "Store not selected !";
            isSave = false;
        }
        if (common.myInt(Session["FacilityID"]) == 0)
        {
            strmsg += "Facility not selected !";
            isSave = false;
        }

        FractionIssue = CheckFractionIssue(); //Added By Manoj Puri
        //if (CheckFractionIssue())
        if (common.myBool(FractionIssue).Equals(true))  //Added By Manoj Puri
        {
            isSave = false;
            strmsg += "Decimal value of  Quantity is not allow! ";
        }

        lblMessage.Text = strmsg;
        return isSave;
    }
    private bool CheckFractionIssue()
    {
        bool FractionIssue = false;
        try
        {
            string itemid = "";
            foreach (GridViewRow dataItem in gvStore.Rows)
            {
                HiddenField hditemid = (HiddenField)dataItem.FindControl("hdnItemId");
                TextBox txtqty = (TextBox)dataItem.FindControl("txtReturnQty");
                string[] decimalqty = txtqty.Text.Split('.');
                if (decimalqty.Length > 1)
                {
                    if (common.myInt(decimalqty[1]) > 0)
                    {
                        if (itemid == "")
                        {
                            itemid = hditemid.Value;
                        }
                        else
                        {
                            itemid = itemid + "," + hditemid.Value;
                        }
                    }
                }
            }

            if (itemid != "")
            {
                BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
                if (objPharmacy.getItemFraction(itemid).Tables[0].Rows.Count > 0)
                {
                    FractionIssue = true;
                }
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            FractionIssue = true;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return FractionIssue;
    }
    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        if (common.myStr(ViewState["IsEMRReasonMandatory"]).ToUpper().Equals("Y"))
        {
            if (common.myInt(ddlReason.SelectedValue).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Reason not selected !";
                return;
            }
        }

        if (common.myBool(hdnIsPasswordRequired.Value))
            IsValidUserNameAndPassword();
        else
            saveRecord();
    }
    private void saveRecord()
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "";

            if (!isSaved())
            {
                return;
            }
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();

            #region list of qty

            int iSno = 0;
            int AdvisingDoctorId = 0;

            foreach (GridViewRow dataItem in gvStore.Rows)
            {
                Label lblMRP = (Label)dataItem.FindControl("lblMRP");
                HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                HiddenField hdnBatchId = (HiddenField)dataItem.FindControl("hdnBatchId");
                HiddenField hdnIssueId = (HiddenField)dataItem.FindControl("hdnIssueId");
                HiddenField hdnItemExpiryDate = (HiddenField)dataItem.FindControl("hdnItemExpiryDate");
                HiddenField hdnCostPrice = (HiddenField)dataItem.FindControl("hdnCostPrice");
                HiddenField hdnSaleTaxPerc = (HiddenField)dataItem.FindControl("hdnSaleTaxPerc");

                HiddenField hdnEmpAmt = (HiddenField)dataItem.FindControl("hdnEmpAmt");
                HiddenField hdnCompAmt = (HiddenField)dataItem.FindControl("hdnCompAmt");
                HiddenField hdnCopayPerc = (HiddenField)dataItem.FindControl("hdnCopayPerc");



                Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                Label lblBatchNo = (Label)dataItem.FindControl("lblBatchNo");
                Label lblDiscPerc = (Label)dataItem.FindControl("lblDiscPerc");
                Label lblQty = (Label)dataItem.FindControl("lblQty");
                Label lblReturnedQty = (Label)dataItem.FindControl("lblReturnedQty");
                Label lblBalanceQty = (Label)dataItem.FindControl("lblBalanceQty");
                TextBox txtReturnQty = (TextBox)dataItem.FindControl("txtReturnQty");
                // Label lblIssueDetailsId = (Label)dataItem.FindControl("lblIssueDetailsId");

                AdvisingDoctorId = common.myInt(((HiddenField)dataItem.FindControl("hdnAdvisingDoctorId")).Value);

                if (common.myInt(hdnItemId.Value) == 0)
                {
                    break;
                }

                if (common.myDbl(txtReturnQty.Text) == 0)
                {
                    continue;
                }

                double qty = common.myDbl(txtReturnQty.Text);
                double rate = common.myDbl(lblMRP.Text);
                double discPerc = common.myDbl(lblDiscPerc.Text);
                double discAmt = 0;

                if (common.myDbl(lblBalanceQty.Text) < qty)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Return quantity cann't be greater then balance quantity !" + Environment.NewLine +
                                     "At Item [ " + common.myStr(lblItemName.Text) + "(" + common.myStr(lblBatchNo.Text) + ") ]";
                    return;
                }

                discAmt = (qty * rate) * (discPerc / 100);

                double netAmt = 0;
                netAmt = (qty * rate) - discAmt;

                iSno++;

                coll.Add(iSno);//SequenceNo
                coll.Add(common.myInt(hdnItemId.Value));//ItemId
                coll.Add(common.myInt(0));//RequestedItemId
                coll.Add(common.myInt(hdnBatchId.Value));//BatchId
                coll.Add(common.myStr(lblBatchNo.Text));//BatchNo
                coll.Add(qty);//Qty
                // coll.Add(rate);//Rate

                double payerAmt = qty * rate;
                decimal CopayPercentage = 0, CopayAmt = 0;
                decimal totalAmount = 0, TotalAmount = 0;
                TotalAmount = common.myDec(payerAmt);
                //////CopayPercentage = common.myDec(hdnCopayPerc.Value);
                //////CopayAmt = CopayPercentage / 100 * (TotalAmount);
                //////decimal AmountPayableByPatient = CopayAmt;
                //////decimal AmountPayableByPayer = TotalAmount - common.myDec(CopayAmt);


                decimal AmountPayableByPatient = 0;
                decimal AmountPayableByPayer = 0;
                if (txtPaymentMode.Text.ToUpper() == "CASH")
                {
                    AmountPayableByPatient = TotalAmount;
                    AmountPayableByPayer = 0;
                }
                else
                {
                    CopayPercentage = common.myDec(hdnCopayPerc.Value);
                    if (CopayPercentage > 0)
                    {
                        CopayAmt = CopayPercentage / 100 * (TotalAmount);
                    }
                    AmountPayableByPatient = CopayAmt;
                    AmountPayableByPayer = TotalAmount - common.myDec(CopayAmt);
                }


                //if (common.myInt(ViewState["DefCompId"]) == common.myInt(hdnSponsorId.Value))
                //{
                //    coll.Add(common.myDbl(payerAmt).ToString("F4"));//EmpAmt
                //    coll.Add(0);//CompAmt
                //}
                //else
                //{
                //    coll.Add(0);//EmpAmt
                //    coll.Add(common.myDbl(payerAmt).ToString("F4"));//CompAmt
                //}

                coll.Add(AmountPayableByPatient);//EmpAmt
                coll.Add(common.myDbl(AmountPayableByPayer).ToString("F4"));//CompAmt

                coll.Add(discPerc);//DiscPerc
                coll.Add(discAmt);//DiscAmt
                coll.Add(common.myDbl(hdnSaleTaxPerc.Value));//SaleTaxPerc
                coll.Add(0);//SaleTaxAmt
                coll.Add(common.myDbl(hdnCostPrice.Value));//CostPrice
                coll.Add(rate);//MRP
                coll.Add(netAmt);//NetAmt
                // coll.Add(0);//IsSupplimentary
                coll.Add(common.myInt(hdnIssueId.Value));//RefIssueId //Reference Issue Id  = 0 when Issue and Issue Id When Return
                coll.Add(common.myStr(hdnItemExpiryDate.Value));//ItemExpiryDate
                //  coll.Add(common.myStr(lblIssueDetailsId.Text));//
                coll.Add(common.myDec(hdnCopayPerc.Value));//CoPayPerc INT
                strXML.Append(common.setXmlTable(ref coll));
            }

            #endregion

            if (strXML.ToString() == "")
            {
                lblMessage.Text = "Quantity can't be blank !";
                return;
            }

            string docNo = "";
            int returnId = 0;
            string ProcessStatus = "P";
            string UseFor = "P"; // P or W , w means data go from wardmanagement

            if (common.myStr(Request.QueryString["MASTER"]) == "No")
            {
                ProcessStatus = "O";
                UseFor = "W";
            }

            if (common.myStr(Session["SaveDuplicate"]) == "0")
            {
                bool bitError = false;
                var employeeId = (common.myBool(hdnIsPasswordRequired.Value) && hdnPasswordScreenType.Value.Equals("U") ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserID"]));

                string strMsg = objPharmacy.SaveSaleIssue(common.myInt(Session["HospitalLocationId"]),
                                common.myInt(Session["FacilityId"]), 203, common.myDate(txtReturnDate.SelectedDate),
                                 common.myInt(ViewState["StoreId"]), common.myInt(hdnRegistrationId.Value),
                                common.myInt(hdnEncounterId.Value), common.myStr(txtPatientName.Text).Trim(),
                                "", "", "", common.myInt(hdnSponsorId.Value), 0, "", "R", ProcessStatus, 0,
                                employeeId, "", 0, employeeId, common.myDate(txtReturnDate.SelectedDate), txtRemarks.Text, strXML.ToString(),
                                "", 0, true, employeeId, "IP-RET", out docNo, 0, out returnId,
                                txtRegistrationNo.Text, "", 0, 0, UseFor, 0, "N", "", 0, 0,
                                common.myStr(System.Web.HttpContext.Current.Request.UserHostAddress), out bitError, common.myInt(ddlReason.SelectedValue));

                if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
                {
                    Session["SaveDuplicate"] = "1";
                    hdnReturnId.Value = "";
                    ViewState["SelectedItemIds"] = "";
                    hdnRegistrationId.Value = "0";
                    hdnItemId.Value = "0";
                    hdnEncounterId.Value = "0";
                    hdnItemId.Value = "0";
                    hdnSponsorId.Value = "0";

                    bindData();

                    clearItem();
                    ddlItem.Enabled = false;

                    hdnReturnId.Value = common.myStr(returnId);
                    txtReturnNo.Text = common.myStr(docNo);

                    fillReturnDetails();

                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                    //dvConfirmPrint.Visible = true;
                    strMsg += " Return No : " + common.myStr(docNo);

                    if (btnPrint.Visible)
                        Print();
                }


                lblMessage.Text = strMsg;
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

    private bool isPosted()
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myInt(hdnReturnId.Value) == 0)
        {
            strmsg += "Return No not selected !";
            isSave = false;
        }
        if (common.myInt(ViewState["StoreId"]) == 0)
        {
            strmsg += "Store not selected !";
            isSave = false;
        }
        if (common.myInt(Session["FacilityID"]) == 0)
        {
            strmsg += "Facility not selected !";
            isSave = false;
        }

        lblMessage.Text = strmsg;
        return isSave;
    }

    protected void btnPostData_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "";

            if (!isPosted())
            {
                return;
            }
            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            #region list of qty

            int iSno = 0;

            foreach (GridViewRow dataItem in gvStore.Rows)
            {
                Label lblMRP = (Label)dataItem.FindControl("lblMRP");
                HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                HiddenField hdnBatchId = (HiddenField)dataItem.FindControl("hdnBatchId");
                HiddenField hdnIssueId = (HiddenField)dataItem.FindControl("hdnIssueId");
                HiddenField hdnItemExpiryDate = (HiddenField)dataItem.FindControl("hdnItemExpiryDate");
                HiddenField hdnCostPrice = (HiddenField)dataItem.FindControl("hdnCostPrice");
                HiddenField hdnSaleTaxPerc = (HiddenField)dataItem.FindControl("hdnSaleTaxPerc");

                Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                Label lblBatchNo = (Label)dataItem.FindControl("lblBatchNo");
                Label lblDiscPerc = (Label)dataItem.FindControl("lblDiscPerc");
                Label lblQty = (Label)dataItem.FindControl("lblQty");
                Label lblReturnedQty = (Label)dataItem.FindControl("lblReturnedQty");
                Label lblBalanceQty = (Label)dataItem.FindControl("lblBalanceQty");
                TextBox txtReturnQty = (TextBox)dataItem.FindControl("txtReturnQty");
                Label lblIssueDetailsId = (Label)dataItem.FindControl("lblIssueDetailsId");

                Label lblOnLineReturnQty = (Label)dataItem.FindControl("lblOnLineReturnQty");

                if (common.myInt(hdnItemId.Value) == 0)
                {
                    break;
                }

                //if (common.myDbl(txtReturnQty.Text) == 0)
                //{
                //    continue;
                //}

                double qty = common.myDbl(txtReturnQty.Text);
                double rate = common.myDbl(lblMRP.Text);
                double discPerc = common.myDbl(lblDiscPerc.Text);
                double discAmt = 0;

                if (common.myDbl(lblOnLineReturnQty.Text) < qty)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Return quantity cann't be greater then balance quantity !" + Environment.NewLine +
                                     "At Item [ " + common.myStr(lblItemName.Text) + "(" + common.myStr(lblBatchNo.Text) + ") ]";
                    return;
                }

                discAmt = (qty * rate) * (discPerc / 100);

                double netAmt = 0;
                netAmt = (qty * rate) - discAmt;

                iSno++;


                coll.Add(common.myInt(hdnItemId.Value));//ItemId               
                coll.Add(common.myInt(hdnBatchId.Value));//BatchId               
                coll.Add(qty);//Qty
                coll.Add(rate);//MRP
                coll.Add(discAmt);//DiscAmt
                double payerAmt = qty * rate;
                if (common.myInt(ViewState["DefCompId"]) == common.myInt(hdnSponsorId.Value))
                {
                    coll.Add(common.myDbl(payerAmt).ToString("F4"));//EmpAmt
                    coll.Add(0);//CompAmt
                }
                else
                {
                    coll.Add(0);//EmpAmt
                    coll.Add(common.myDbl(payerAmt).ToString("F4"));//CompAmt
                }
                coll.Add(netAmt);//NetAmt     
                coll.Add(lblIssueDetailsId.Text);//IssueDetailsId     
                strXML.Append(common.setXmlTable(ref coll));
            }

            #endregion
            if (strXML.ToString() == "")
            {
                lblMessage.Text = "Quantity can't be blank !";
                return;
            }

            string strMsg = objPharmacy.PostIPWardReturn(common.myInt(hdnReturnId.Value), common.myInt(ViewState["StoreId"]),
                common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserId"]),
                Convert.ToString(strXML), common.myStr("I"));

            if (strMsg.Contains(" Post") && !strMsg.Contains("usp"))
            {
                // hdnReturnId.Value = "";
                ViewState["SelectedItemIds"] = "";
                hdnRegistrationId.Value = "0";
                hdnItemId.Value = "0";
                hdnEncounterId.Value = "0";
                hdnItemId.Value = "0";
                hdnSponsorId.Value = "0";

                clearItem();
                ddlItem.Enabled = false;
                //done by rakesh for user authorisation start
                //btnPostData.Visible = false;
                SetPermission(btnPostData, false);
                //done by rakesh for user authorisation end
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                //dvConfirmPrint.Visible = true;
                Print();
            }

            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    private void clearControl()
    {
        //Session["SaveDuplicate"] = "0";
        lblMessage.Text = "&nbsp;";

        txtWard.Text = "";
        txtReturnDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
        txtReturnDate.SelectedDate = DateTime.Now;
        txtPaymentMode.Text = "";
        txtBedNo.Text = "";
        txtPatientName.Text = "";
        txtPatientName.ToolTip = "";
        txtNetAmount.Text = "";
        hdnNetAmount.Value = "";
        txtRemarks.Text = "";


        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    protected void gvStore_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblMRP = (Label)e.Row.FindControl("lblMRP");
                Label lblDiscPerc = (Label)e.Row.FindControl("lblDiscPerc");
                Label lblQty = (Label)e.Row.FindControl("lblQty");
                Label lblReturnedQty = (Label)e.Row.FindControl("lblReturnedQty");
                Label lblBalanceQty = (Label)e.Row.FindControl("lblBalanceQty");
                HiddenField hdnBalanceQty = (HiddenField)e.Row.FindControl("hdnBalanceQty");
                TextBox txtReturnQty = (TextBox)e.Row.FindControl("txtReturnQty");
                TextBox txtNetAmt = (TextBox)e.Row.FindControl("txtNetAmt");
                HiddenField hdnMRP = (HiddenField)e.Row.FindControl("hdnMRP");
                Label lblOnLineReturnQty = (Label)e.Row.FindControl("lblOnLineReturnQty");
                Label lblItemName = (Label)e.Row.FindControl("lblItemName");
                HiddenField hdnIndentBy = (HiddenField)e.Row.FindControl("hdnIndentBy");
                HiddenField hdnIndentDate = (HiddenField)e.Row.FindControl("hdnIndentDate");
                HiddenField hdnIssuedBy = (HiddenField)e.Row.FindControl("hdnIssuedBy");
                HiddenField hdnIssueNo = (HiddenField)e.Row.FindControl("hdnIssueNo");
                TextBox txtPackSize = (TextBox)e.Row.FindControl("txtPackSize");
                HiddenField hdnConversionFactor = (HiddenField)e.Row.FindControl("hdnConversionFactor");


                String strToolTip = null;

                if (common.myStr(hdnIndentBy.Value) != string.Empty)
                    strToolTip = "Item Indent By  : " + common.myStr(hdnIndentBy.Value) + System.Environment.NewLine + "Item Indent On : " + common.myStr(hdnIndentDate.Value) + System.Environment.NewLine;

                strToolTip = strToolTip + "Item Issued No : " + common.myStr(hdnIssueNo.Value) + System.Environment.NewLine + "Item Issued By  : " + common.myStr(hdnIssuedBy.Value) + System.Environment.NewLine;
                lblItemName.ToolTip = strToolTip;

                HiddenField hdnOnLineReturnQty = (HiddenField)e.Row.FindControl("hdnOnLineReturnQty");

                Label lblReturnQty = (Label)e.Row.FindControl("lblReturnQty");
                lblMRP.Text = common.myDbl(lblMRP.Text).ToString("F2");
                lblDiscPerc.Text = common.myDbl(lblDiscPerc.Text).ToString("F2");
                lblQty.Text = common.myDbl(lblQty.Text).ToString("F2");
                lblReturnedQty.Text = common.myDbl(lblReturnedQty.Text).ToString("F2");
                lblBalanceQty.Text = common.myDbl(lblBalanceQty.Text).ToString("F2");
                txtNetAmt.Text = common.myDbl(txtNetAmt.Text).ToString("F2");
                lblReturnQty.Text = common.myDbl(lblReturnQty.Text).ToString("F2");
                lblOnLineReturnQty.Text = common.myDbl(lblOnLineReturnQty.Text).ToString("F2");

                if (common.myStr(ViewState["SearchFrom"]) == "R")
                {
                    //lblReturnedQty.Text = lblQty.Text;
                    txtReturnQty.Attributes.Add("onkeyup", "javascript:calcChkQty('" + txtReturnQty.ClientID + "','" + hdnBalanceQty.ClientID + "','" + hdnMRP.ClientID + "','" + txtNetAmt.ClientID + "');");
                    if (hdnDocumentNoStatus.Value == "O")
                    {
                        // txtNetAmt.Text = "0.00";

                        txtReturnQty.Text = lblOnLineReturnQty.Text;
                        txtNetAmt.Text = (common.myDbl(txtReturnQty.Text) * common.myDbl(lblMRP.Text)).ToString("F2");
                        txtReturnQty.Attributes.Add("onkeyup", "javascript:calcChkQty('" + txtReturnQty.ClientID + "','" + hdnOnLineReturnQty.ClientID + "','" + hdnMRP.ClientID + "','" + txtNetAmt.ClientID + "');");
                    }

                }
                else
                {
                    txtNetAmt.Text = "0.00";
                    txtReturnQty.Attributes.Add("onkeyup", "javascript:calcChkQty('" + txtReturnQty.ClientID + "','" + hdnBalanceQty.ClientID + "','" + hdnMRP.ClientID + "','" + txtNetAmt.ClientID + "');");
                }

                if (common.myStr(ViewState["IsSaleInPackUnit"]) == "Y")
                {
                    if (hdnDocumentNoStatus.Value != "P")
                    {
                        gvStore.Columns[11].Visible = true;
                    }
                    else
                    {
                        gvStore.Columns[11].Visible = false;
                    }

                    txtPackSize.Attributes.Add("onkeyup", "javascript:GetQty('" + txtPackSize.ClientID + "','" + hdnConversionFactor.ClientID + "','" + txtReturnQty.ClientID + "','" + hdnBalanceQty.ClientID + "','" + hdnMRP.ClientID + "','" + txtNetAmt.ClientID + "');");
                    //txtReturnQty.Attributes.Add("readonly" ,);
                    //txtReturnQty.Attributes.Add("readonly", "readonly"); // Akshay
                    txtPackSize.Focus();

                    txtPackSize.Enabled = false; //Akshay
                }

                hdnNetAmount.Value = common.myStr(common.myDbl(hdnNetAmount.Value) + common.myDbl(txtNetAmt.Text));
                //txtReturnQty.Attributes.Add("onkeyup", "javascript:calcChkQty('" + txtReturnQty.ClientID + "','" + hdnBalanceQty.ClientID + "','" + hdnMRP.ClientID + "','" + txtNetAmt.ClientID + "');");
                ////txtNetAmt.Text = hdnNetAmount.Value;               

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                txtNetAmount.Text = common.FormatNumber(common.myStr(hdnNetAmount.Value), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"])); ;
            }
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                if (common.myStr(ViewState["SearchFrom"]) == "R")
                {
                    // e.Row.Cells[Convert.ToByte(GridItemDetails.IssueQty)].Visible = false;
                    //e.Row.Cells[Convert.ToByte(GridItemDetails.BalanceQty)].Visible = false;
                    //e.Row.Cells[Convert.ToByte(GridItemDetails.ReturnQty)].Visible = false;
                    //e.Row.Cells[Convert.ToByte(GridItemDetails.Delete)].Visible = false;
                    if (hdnDocumentNoStatus.Value == "O")//open
                    {
                        if (common.myStr(ViewState["IsSaleInPackUnit"]) == "Y")
                        {
                            e.Row.Cells[Convert.ToByte(GridItemDetails.ReturnQty)].Visible = true;
                            e.Row.Cells[Convert.ToByte(GridItemDetails.EnterReturnQty)].Visible = true;

                        }
                        else
                        {
                            e.Row.Cells[Convert.ToByte(GridItemDetails.EnterReturnQty)].Visible = true;
                            e.Row.Cells[Convert.ToByte(GridItemDetails.ReturnQty)].Visible = false;
                        }
                    }
                    else // post
                    {
                        e.Row.Cells[Convert.ToByte(GridItemDetails.Delete)].Visible = false;
                        e.Row.Cells[Convert.ToByte(GridItemDetails.ReturnQty)].Visible = true;
                        e.Row.Cells[Convert.ToByte(GridItemDetails.EnterReturnQty)].Visible = false;
                    }
                }
                else
                {
                    e.Row.Cells[Convert.ToByte(GridItemDetails.ReturnQty)].Visible = false;
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

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Session["SaveDuplicate"] = "0";
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void btnOpenPatientWnd_OnClick(Object sender, EventArgs e)
    {
        string strpatienttype = "IP"; //ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();

        RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?SalType=" + strpatienttype + "&RegEnc=1"; ;
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        Session["SaveDuplicate"] = "0";
    }

    protected void btnGoEnter_OnClick(Object sender, EventArgs e)
    {
        try
        {
            hdnReturnId.Value = "";
            txtReturnNo.Text = "";

            BaseC.clsEMRBilling objVal = new BaseC.clsEMRBilling(sConString);

            //DataSet ds = objVal.getOPIPRegEncDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
            //                           common.myStr("I"), 0, 0, 1, "", common.myStr(txtEncounterNo.Text).Trim(),
            //                           "", common.myDate("1753-01-01"), common.myDate("9999-12-31"), "", 0, 0, common.myInt(Session["UserId"]), 0, "");
            DataSet ds = objVal.getOPIPRegEncDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myStr("I"), 0, 0, common.myInt(1), "", common.myStr(txtEncounterNo.Text).Trim(),
                                    common.escapeCharString(common.myStr(txtPatientName.Text), false), common.myDate("1950-01-01"),
                                    common.myDate("2059-12-31"), "F", 10, common.myInt(10), common.myInt(Session["UserId"]), 0,
                                    "", 0, "", "", "", "", "", "", "", "", "", 0, string.Empty, string.Empty, string.Empty, 0, 0, 0, false, string.Empty, 0);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow DR = ds.Tables[0].Rows[0];

                hdnRegistrationId.Value = common.myStr(DR["REGID"]);
                txtRegistrationNo.Text = common.myStr(DR["RegistrationNo"]);
                hdnEncounterId.Value = common.myStr(DR["ENCID"]);
                txtEncounterNo.Text = common.myStr(DR["EncounterNo"]);
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Record not Exists !";

                hdnRegistrationId.Value = "";
                txtRegistrationNo.Text = "";
                hdnEncounterId.Value = "";
                txtEncounterNo.Text = "";
            }

            fillData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    protected void btnSearchPatient_OnClick(object sender, EventArgs e)
    {
        try
        {
            txtReturnNo.Text = "";
            lblMessage.Text = "";
            fillData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    void fillData()
    {
        try
        {
            ViewState["SearchFrom"] = "E";
            ViewState["SelectedItemIds"] = "";
            //done by rakesh for user authorisation start
            //btnPrint.Visible = false;
            SetPermission(btnPrint, false);
            //btnSaveData.Visible = true;
            SetPermission(btnSaveData, "N", true);
            //btnPostData.Visible = false;
            SetPermission(btnPostData, false);
            //done by rakesh for user authorisation end
            ddlItem.Enabled = true;

            hdnItemId.Value = "0";
            if (common.myInt(hdnRegistrationId.Value) > 0)
            {
                BindPatientDetails();

                clearItem();
                bindData();
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

    void BindPatientDetails()
    {
        try
        {
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = new DataSet();

            clearControl();

            if (common.myInt(hdnRegistrationId.Value) > 0 || txtEncounterNo.Text != "")
            {
                ds = objPharmacy.GetPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(hdnRegistrationId.Value), "I", "", common.myStr(txtEncounterNo.Text));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = ds.Tables[0].Rows[0];

                    txtRegistrationNo.Text = common.myStr(DR["RegistrationNo"]).Trim();
                    hdnEncounterId.Value = common.myStr(DR["EncounterId"]);
                    txtEncounterNo.Text = common.myStr(DR["EncounterNo"]).Trim();
                    ViewState["RegistrationId"] = common.myStr(DR["RegistrationId"]).Trim();
                    hdnRegistrationId.Value = common.myStr(DR["RegistrationId"]).Trim();
                    hdnSponsorId.Value = common.myStr(common.myInt(DR["SponsorId"]));

                    txtReturnDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                    txtReturnDate.SelectedDate = DateTime.Now;

                    txtPaymentMode.Text = common.myStr(DR["PaymentMode"]);
                    txtBedNo.Text = common.myStr(DR["BedNo"]);
                    txtWard.Text = common.myStr(DR["WardName"]);
                    txtPatientName.Text = common.myStr(DR["Name"]).Trim();
                    txtPatientName.ToolTip = common.myStr(DR["Name"]).Trim();
                    txtEncounterNo.Enabled = false;
                    txtRegistrationNo.Enabled = false;

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

    public void clearItem()
    {
        this.ddlItem.Text = "";
        this.ddlItem.Items.Clear();
    }

    protected void ddlItem_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            int ItemsPerRequest = 100;
            RadComboBox ddl = sender as RadComboBox;

            //if (e.Text != "" && e.Text.Length > 2)
            //{
            DataTable data = GetData(e.Text);
            ViewState["ItemData"] = data;
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                ddl.Items.Add(new RadComboBoxItem(data.Rows[i]["ItemName"].ToString(), data.Rows[i]["ItemId"].ToString()));
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    private DataTable GetData(string text)
    {
        DataSet ds = new DataSet();
        try
        {
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

            ds = objPharmacy.getIPIssueItemList(common.myInt(hdnEncounterId.Value), common.myStr(txtEncounterNo.Text), common.myInt(ViewState["StoreId"]),
                                                common.myInt(Session["FacilityId"]), common.myStr(text));

            if (common.myInt(hdnEncounterId.Value) == 0)
            {
                return ds.Tables[0].Clone();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }

        return ds.Tables[0];
    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
        {
            return "No matches";
        }

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            string allItemIds = common.myStr(ViewState["SelectedItemIds"]);

            if (!allItemIds.Contains(common.myStr(common.myInt(hdnItemId.Value))))
            {
                if (common.myStr(ViewState["SelectedItemIds"]).Trim().Length != 0)
                {
                    ViewState["SelectedItemIds"] += "," + common.myStr(common.myInt(hdnItemId.Value));
                }
                else
                {
                    ViewState["SelectedItemIds"] = common.myStr(common.myInt(hdnItemId.Value));
                }
            }
            #region For_BatchID
            StringBuilder strXMLItemIds;
            ArrayList coll;

            strXMLItemIds = new StringBuilder();
            coll = new ArrayList();

            int fromWard = 0;
            if (common.myStr(Request.QueryString["MASTER"]) == "No")
            {
                fromWard = 1;
            }

            string ItemIds = common.myStr(common.myInt(hdnItemId.Value));
            if (ItemIds.Length != 0)
            {
                string[] strItemIds = ItemIds.Split(',');

                foreach (string itemId in strItemIds)
                {
                    coll.Add(common.myInt(itemId));
                    strXMLItemIds.Append(common.setXmlTable(ref coll));
                }
            }
            DataSet ds = objPharmacy.getIPIssueItemDetails(common.myInt(hdnEncounterId.Value), common.myStr(txtEncounterNo.Text), common.myInt(ViewState["StoreId"]),
                                 common.myInt(Session["FacilityID"]), common.myStr(strXMLItemIds.ToString()), common.myInt(Session["FacilityID"]), fromWard);
            string batchID = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    ViewState["BatchID"] = common.myStr(ds.Tables[0].Rows[0]["BatchID"]);
                }
                else
                {
                    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        batchID += common.myStr(ds.Tables[0].Rows[i]["BatchID"]) + ",";
                    }
                    ViewState["BatchID"] = batchID.Substring(0, batchID.Length - 1);
                }
            }


            setBIDs();            // added for adding batch
            #endregion
            // bindData();
            bindDataWithBarCode();
            ddlItem.Text = "";
            ddlItem.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    protected void btnOpenReturnNoWnd_OnClick(Object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["MASTER"]) == "No")
        {
            if (common.myInt(ViewState["StoreId"]) != 0)
            {
                RadWindowForNew.NavigateUrl = "/WardManagement/CancelDrugReturn.aspx?StoreId=" + common.myStr(ViewState["StoreId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]);
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 1200;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true;
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                Session["SaveDuplicate"] = "0";
            }
            else
            {
                Alert.ShowAjaxMsg("Please Select The Store!!", Page);
                return;
            }
        }
        else
        {
            lblMessage.Text = "";
            RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/ViewDocument.aspx?SetupId=" + common.myStr("203") + "&DocNo=" + common.myStr(txtReturnNo.Text) + "&OPIP=I&UseFor=R";
            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 950;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
    }

    protected void btnGoReturnNoEnter_OnClick(Object sender, EventArgs e)
    {
        if (common.myStr(txtReturnNo.Text).Trim().Length > 0)
        {
            fillReturnDetails();
        }
    }

    protected void btnReturnDetails_OnClick(Object sender, EventArgs e)
    {
        fillReturnDetails();
    }

    void fillReturnDetails()
    {
        try
        {
            ViewState["SearchFrom"] = "R";
            //ViewState["GridData"] = null;
            ////done by rakesh for user authorisation start
            btnPrint.Visible = true;
            //SetPermission(btnPrint, "P", true);
            ////btnSaveData.Visible = false;
            SetPermission(btnSaveData, false);
            ////btnPostData.Visible = false;
            //SetPermission(btnPostData, false);
            ////done by rakesh for user authorisation end
            ddlItem.Enabled = false;

            lblMessage.Text = "";
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

            DataSet ds = new DataSet();
            if (common.myInt(hdnReturnId.Value) > 0 || common.myStr(txtReturnNo.Text).Trim().Length > 0)
            {
                clearControl();

                ds = objPharmacy.GetphrSaleIssueItem("R", common.myInt(Session["HospitalLocationId"]),
                                            common.myInt(ViewState["StoreId"]), common.myInt(Session["FacilityID"]),
                                            common.myInt(203), common.myInt(hdnReturnId.Value),
                                            common.myStr(txtReturnNo.Text), "I");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = ds.Tables[0].Rows[0];
                    hdnDocumentNoStatus.Value = common.myStr(DR["ProcessStatus"]);
                    btnCancelReturn.Visible = false;
                    if (common.myStr(DR["ProcessStatus"]) == "O")
                    {
                        //done by rakesh for user authorisation start
                        //btnCancelReturn.Visible = true;
                        SetPermission(btnCancelReturn, "C", true);
                        //done by rakesh for user authorisation end
                        if (common.myStr(Request.QueryString["MASTER"]) != "No")
                        {
                            //done by rakesh for user authorisation start
                            //btnPostData.Visible = true;
                            SetPermission(btnPostData, "N", true);
                            //done by rakesh for user authorisation end
                        }
                    }

                    hdnReturnId.Value = common.myStr(DR["IssueId"]);
                    txtReturnNo.Text = common.myStr(DR["IssueNo"]);
                    ViewState["Status"] = common.myStr(DR["ProcessStatus"]);
                    hdnRegistrationId.Value = common.myStr(common.myInt(DR["RegistrationId"]));
                    txtRegistrationNo.Text = common.myStr(DR["RegistrationNo"]);
                    hdnEncounterId.Value = common.myStr(DR["EncounterId"]);//
                    txtEncounterNo.Text = common.myStr(DR["EncounterNo"]);

                    hdnSponsorId.Value = common.myStr(common.myInt(DR["SponsorId"]));

                    txtReturnDate.SelectedDate = common.myDate(DR["IssueDate"]);

                    txtPaymentMode.Text = common.myStr(DR["PaymentMode"]);
                    txtBedNo.Text = common.myStr(DR["BedNo"]);
                    txtWard.Text = common.myStr(DR["WardName"]);
                    txtPatientName.Text = common.myStr(DR["PatientName"]);
                    txtPatientName.ToolTip = common.myStr(DR["PatientName"]);
                    txtRemarks.Text = common.myStr(DR["Remarks"]);
                    txtEncounterNo.Enabled = false;
                    txtRegistrationNo.Enabled = false;

                    ds.Tables[1].Columns.Add(new DataColumn("EnterReturnQty"));
                    ds.Tables[1].AcceptChanges();

                    gvStore.DataSource = ds.Tables[1].DefaultView;
                    gvStore.DataBind();
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
    private void Print()
    {
        if (common.myInt(hdnReturnId.Value) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Return No not selected !";
            dvConfirmPrint.Visible = false;
            return;
        }

        RadWindowForNew.NavigateUrl = "~/Pharmacy/Reports/IPSaleDocReport.aspx?IssueId=" + common.myInt(hdnReturnId.Value) + "&SetupId=203&UseFor=DOC&IssueReturn=R&StoreId=" + ddlStore.SelectedValue;
        RadWindowForNew.Height = 500;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(hdnReturnId.Value) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Return No not selected !";
            dvConfirmPrint.Visible = false;
            return;
        }

        RadWindowForNew.NavigateUrl = "~/Pharmacy/Reports/IPSaleDocReport.aspx?IssueId=" + common.myInt(hdnReturnId.Value) + "&SetupId=203&UseFor=DOC&IssueReturn=R&StoreId=" + ddlStore.SelectedValue + "&Export=" + chkExport.Checked;
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;

        dvConfirmPrint.Visible = false;
    }

    protected void gvStore_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ItemDelete")
            {
                int RowNo = common.myInt(e.CommandArgument);

                if (ViewState["GridData"] != null && RowNo > 0)
                {
                    DataTable tbl = (DataTable)ViewState["GridData"];

                    int rIdx = 0;
                    foreach (GridViewRow dataItem in gvStore.Rows)
                    {
                        HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                        HiddenField hdnBatchId = (HiddenField)dataItem.FindControl("hdnBatchId");
                        TextBox txtReturnQty = (TextBox)dataItem.FindControl("txtReturnQty");

                        if (common.myDbl(txtReturnQty.Text) > 0)
                        {
                            if (common.myInt(tbl.Rows[rIdx]["ItemId"]) == common.myInt(hdnItemId.Value)
                                && common.myInt(tbl.Rows[rIdx]["BatchId"]) == common.myInt(hdnBatchId.Value))
                            {
                                tbl.Rows[rIdx]["EnterReturnQty"] = common.myDbl(txtReturnQty.Text);
                            }
                        }
                        rIdx++;
                    }
                    ////-------------- Code for Deleted record should not show again
                    DataView DVFilter = tbl.Copy().DefaultView;
                    DVFilter.RowFilter = "RowNo = " + RowNo;

                    DataTable myDeletedDT = DVFilter.ToTable();
                    ViewState["DeletedID"] = common.myStr(myDeletedDT.Rows[0]["ItemID"]);
                    ViewState["DeletedBatchID"] = common.myStr(myDeletedDT.Rows[0]["BatchID"]);
                    RemoveIds();
                    ////---- Done---------------

                    DataView DV = tbl.Copy().DefaultView;
                    DV.RowFilter = "RowNo <> " + RowNo;

                    tbl = DV.ToTable();

                    if (tbl.Rows.Count == 0)
                    {
                        DataRow DR = tbl.NewRow();
                        tbl.Rows.Add(DR);
                        tbl.AcceptChanges();
                    }

                    gvStore.DataSource = tbl;
                    gvStore.DataBind();

                    ViewState["GridData"] = tbl;

                    setFocus();
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

    protected void btnCancelPrint_OnClick(object sender, EventArgs e)
    {
        dvConfirmPrint.Visible = false;
    }

    protected void btnSearchByUHID_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["SearchFrom"] = "E";
            ViewState["SelectedItemIds"] = "";
            //done by rakesh for user authorisation start
            //btnPrint.Visible = false;
            SetPermission(btnPrint, false);
            //btnSaveData.Visible = true;
            SetPermission(btnSaveData, "N", true);
            //btnPostData.Visible = false;
            SetPermission(btnPostData, false);
            //done by rakesh for user authorisation end
            ddlItem.Enabled = true;

            hdnItemId.Value = "0";
            if (common.myInt(txtRegistrationNo.Text) > 0)
            {
                DataSet dsR = new DataSet();
                BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
                dsR = objPharmacy.getEncBasedOnRegNo(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(txtRegistrationNo.Text));
                if (dsR.Tables[0].Rows.Count > 0)
                {
                    hdnEncounterId.Value = common.myStr(dsR.Tables[0].Rows[0]["EncounterId"]);
                    txtEncounterNo.Text = common.myStr(dsR.Tables[0].Rows[0]["EncounterNo"]);
                }
                BindPatientDetails();

                clearItem();
                bindData();
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

    protected void btnCancelReturn_OnClick(object sender, EventArgs e)
    {
        if (common.myBool(hdnIsPasswordRequired.Value))
            IsValidPassword();
        else
            saveCancelReturn();

    }

    //done by rakesh for user authorisation start
    string masterPaveActive = string.Empty;
    private void SetPermission()
    {

        if (common.myStr(Request.QueryString["MASTER"]).Equals("No"))
            masterPaveActive = "?Master=No";
        else
            masterPaveActive = "?Master=Yes";


        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnSaveData, false);
        ua1.DisableEnableControl(btnPostData, false);
        ua1.DisableEnableControl(btnCancelReturn, false);
        ua1.DisableEnableControl(btnNew, false);
        ua1.DisableEnableControl(btnPrint, false);

        if (ua1.CheckPermissions("N", Request.Url.AbsolutePath + masterPaveActive))
        {
            ua1.DisableEnableControl(btnSaveData, true);
            ua1.DisableEnableControl(btnNew, true);
            ua1.DisableEnableControl(btnPostData, true);
        }
        if (ua1.CheckPermissions("C", Request.Url.AbsolutePath + masterPaveActive))
        {
            ua1.DisableEnableControl(btnCancelReturn, true);
        }
        if (ua1.CheckPermissions("P", Request.Url.AbsolutePath + masterPaveActive))
        {
            ua1.DisableEnableControl(btnPrint, true);
        }
        ua1.Dispose();
    }
    private void SetPermission(Button btnID, string mode, bool action)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnID, false);

        if (common.myStr(Request.QueryString["MASTER"]).Equals("No"))
            masterPaveActive = "?Master=No";
        else
            masterPaveActive = "?Master=Yes";

        if (ua1.CheckPermissions(mode, Request.Url.AbsolutePath + masterPaveActive))
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

    public void btnMarkedForDischarge_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMRBILLING/Popup/ExpectedDateOfDischarge.aspx";
        RadWindowForNew.Height = 610;
        RadWindowForNew.Width = 1050;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
    }

    public void NoofMarkfordischare()
    {
        BaseC.WardManagement objwm = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        ds = objwm.GetDischargeStatusNo(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblNoOfDischarge.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["mfd"]) + ")";
        }
        else
        {
            lblNoOfDischarge.Text = "(" + 0 + ")";
        }
    }

    private void saveCancelReturn()
    {
        try
        {
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            objPharmacy.CancelReturn(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "C", common.myInt(hdnReturnId.Value),
                 (common.myBool(hdnIsPasswordRequired.Value) && hdnPasswordScreenType.Value.Equals("U") ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserID"])));
            lblMessage.Text = "Reurned Cancelled Succesfully For Return No - " + txtReturnNo.Text;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    //------------------------ Functionality to get Medicine using BarCode (Sushil Saini)
    #region "BarCode"
    protected void btnBarCodeValue_OnClick(object sender, EventArgs e)
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

        DataSet ds = new DataSet();
        DataSet dsItem = new DataSet();
        string itemID = string.Empty;
        try
        {

            if (common.myLen(txtPatientName.Text).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Patient or Enter Patient Name ! ";

                return;
            }
            if (common.myLen(txtBedNo.Text).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Enter Bed No ! ";

                return;
            }

            if (common.myStr(txtBarCodeValue.Text) != "")
            {
                //dsItem = objPharmacy.getItemByBarCode(common.myStr(txtBarCodeValue.Text).Trim());
                //dsItem = objPharmacy.getItemMasterBarCode(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                //                          common.myInt(ViewState["StoreId"]), common.myStr(txtBarCodeValue.Text).Trim(), common.myInt(Session["UserId"]));

                dsItem = objPharmacy.getIPDItemBarCode(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                          common.myInt(ViewState["StoreId"]), common.myStr(txtEncounterNo.Text).Trim(), common.myStr(txtBarCodeValue.Text).Trim(), common.myInt(Session["UserId"]));
                if (dsItem.Tables[0].Rows.Count > 0)
                {
                    itemID = common.myStr(dsItem.Tables[0].Rows[0]["ItemID"]);
                    ViewState["BatchID"] = common.myStr(dsItem.Tables[0].Rows[0]["BatchID"]);

                    if (!IsItemExistInCombo(itemID))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Record not found for Barcode ( " + common.myStr(txtBarCodeValue.Text) + " )";

                        txtBarCodeValue.Text = string.Empty; txtBarCodeValue.Focus();
                        return;
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Record not found for Barcode ( " + common.myStr(txtBarCodeValue.Text) + " )";

                    txtBarCodeValue.Text = string.Empty; txtBarCodeValue.Focus();
                    return;
                }

            }

            setIDs();
            setBIDs();
            bindDataWithBarCode();
            //bindData();
            txtBarCodeValue.Text = string.Empty;
            txtBarCodeValue.Focus();


        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPharmacy = null;
            ds.Dispose();
        }
    }

    private void RemoveIds()
    {
        try
        {
            if (!String.IsNullOrEmpty(common.myStr(ViewState["DeletedID"])))
            {
                DataTable dtTemp = (DataTable)ViewState["GridData"];
                string find = "ItemId = '" + common.myStr(ViewState["DeletedID"]) + "'";

                if (dtTemp.Select(find).Length > 1)
                {
                    RemoveBatchID();
                }
                else
                {
                    RemoveItemID();
                    RemoveBatchID();

                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    void RemoveItemID()
    {
        string ItemIds = common.myStr(ViewState["SelectedItemIds"]);
        if (!String.IsNullOrEmpty(common.myStr(ViewState["DeletedID"])))
        {
            if (ItemIds.Contains(common.myStr(ViewState["DeletedID"])))
            {
                ItemIds = ItemIds.Replace(common.myStr(ViewState["DeletedID"]), "");
            }
            ViewState["SelectedItemIds"] = ItemIds;
        }
    }

    void RemoveBatchID()
    {
        string ItemIds = common.myStr(ViewState["SelectedBatchIds"]);
        if (!String.IsNullOrEmpty(common.myStr(ViewState["DeletedBatchID"])))
        {
            if (ItemIds.Contains(common.myStr(ViewState["DeletedBatchID"])))
            {
                ItemIds = ItemIds.Replace(common.myStr(ViewState["DeletedBatchID"]), "");
            }
            ViewState["SelectedBatchIds"] = ItemIds;
        }
    }


    private string setIDs()
    {
        string allItemIds = common.myStr(ViewState["SelectedItemIds"]);

        if (!allItemIds.Contains(common.myStr(common.myInt(hdnItemId.Value))))
        {
            if (common.myStr(ViewState["SelectedItemIds"]).Trim().Length != 0)
            {
                ViewState["SelectedItemIds"] += "," + common.myStr(common.myInt(hdnItemId.Value));
            }
            else
            {
                ViewState["SelectedItemIds"] = common.myStr(common.myInt(hdnItemId.Value));
            }
        }
        return allItemIds;
    }
    private string setBIDs()
    {
        string allItemIds = common.myStr(ViewState["SelectedBatchIds"]);

        if (!allItemIds.Contains(common.myStr(ViewState["BatchID"])))
        {
            if (common.myStr(ViewState["SelectedBatchIds"]).Trim().Length != 0)
            {
                ViewState["SelectedBatchIds"] += "," + common.myStr(ViewState["BatchID"]);
            }
            else
            {
                ViewState["SelectedBatchIds"] = common.myStr(ViewState["BatchID"]);
            }
        }
        return allItemIds;
    }
    private bool IsItemExistInCombo(string value)
    {
        bool isExist = false;
        if (string.IsNullOrEmpty(value)) return isExist;
        try
        {
            //int index = -1;
            DataTable data = GetData("");
            DataView DV = data.DefaultView;
            DV.RowFilter = "ItemId = " + common.myInt(value);

            if (DV.ToTable().Rows.Count > 0)
            {
                hdnItemId.Value = value;
                isExist = true;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return (isExist);
    }
    //--------------- Function to Fill Grid Using Barcode 
    private void bindDataWithBarCode()
    {
        try
        {
            StringBuilder strXMLItemIds;
            StringBuilder strXMLBatchIds;
            ArrayList coll;
            ArrayList Bcoll;
            #region ItemIds
            strXMLItemIds = new StringBuilder();
            coll = new ArrayList();

            string allItemIds = common.myStr(ViewState["SelectedItemIds"]).Trim();
            if (allItemIds.Length != 0)
            {
                string[] strItemIds = allItemIds.Split(',');

                foreach (string itemId in strItemIds)
                {
                    coll.Add(common.myInt(itemId));
                    strXMLItemIds.Append(common.setXmlTable(ref coll));
                }
            }

            #endregion
            #region BatchIds
            strXMLBatchIds = new StringBuilder();
            Bcoll = new ArrayList();

            string allBatchIds = common.myStr(ViewState["SelectedBatchIds"]).Trim();
            if (allBatchIds.Length != 0)
            {
                string[] strBatchIds = allBatchIds.Split(',');

                foreach (string itemId in strBatchIds)
                {
                    Bcoll.Add(common.myInt(itemId));
                    strXMLBatchIds.Append(common.setXmlTable(ref Bcoll));
                }
            }

            #endregion

            int fromWard = 0;
            if (common.myStr(Request.QueryString["MASTER"]) == "No")
            {
                fromWard = 1;
            }

            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            //getIPIssueItemDetailsBatch
            //DataSet ds = objPharmacy.getIPIssueItemDetails(common.myInt(hdnEncounterId.Value), common.myStr(txtEncounterNo.Text), common.myInt(ViewState["StoreId"]),
            //                     common.myInt(Session["FacilityID"]), common.myStr(strXMLItemIds.ToString()), common.myInt(Session["FacilityID"]), fromWard);

            DataSet ds = objPharmacy.getIPIssueItemDetailsBatch(common.myInt(hdnEncounterId.Value), common.myStr(txtEncounterNo.Text), common.myInt(ViewState["StoreId"]),
                                 common.myInt(Session["FacilityID"]), common.myStr(strXMLItemIds.ToString()), common.myStr(strXMLBatchIds.ToString()), common.myInt(Session["FacilityID"]), fromWard);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    DataRow DR = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(DR);
                }

                if (!ds.Tables[0].Columns.Contains("EnterReturnQty"))
                {
                    ds.Tables[0].Columns.Add(new DataColumn("EnterReturnQty"));
                    ds.Tables[0].AcceptChanges();
                }

                int countRow = 0;
                if (gvStore.Rows.Count != 0)
                {
                    foreach (GridViewRow dataItem in gvStore.Rows)
                    {
                        try
                        {
                            TextBox txtReturnQty = (TextBox)dataItem.FindControl("txtReturnQty");
                            ds.Tables[0].Rows[countRow]["EnterReturnQty"] = common.myDbl(txtReturnQty.Text).ToString("F2");
                        }
                        catch
                        {
                        }

                        countRow++;
                        ds.Tables[0].AcceptChanges();
                    }
                }

                //Condition to check if a batch id already exists in datatable 
                //DataTable dt = new DataTable("Table");
                //string find = "BatchID = '"+ common.myStr(ViewState["BatchID"] )+"'";
                //if (ds.Tables[0].Select(find).Length > 1)
                //{
                //    dt = (DataTable)ViewState["GridData"];
                //}
                //else
                //{
                //    dt = ds.Tables[0].Copy();
                //}

                //gvStore.DataSource = dt;
                gvStore.DataSource = ds.Tables[0];
                gvStore.DataBind();

                ViewState["GridData"] = ds.Tables[0];

                setFocus();
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



    #endregion

    #region Validate UserId & Password
    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";

        if (hdnPasswordScreenType.Value.Equals("U"))
            RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP";
        else
            RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordChecker.aspx?UseFor=IPDItemReturn";

        RadWindowForNew.Height = 120;
        RadWindowForNew.Width = 340;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

        if (common.myInt(hdnIsValidPassword.Value) == 0)
        {
            lblMessage.Text = "Invalid Password !";
            return;
        }
        saveCancelReturn();

    }
    private void IsValidUserNameAndPassword()
    {
        hdnIsValidPassword.Value = "0";

        if (hdnPasswordScreenType.Value.Equals("U"))
            RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP";
        else
            RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerV1.aspx?UseFor=OPIP";


        RadWindowForNew.Height = 120;
        RadWindowForNew.Width = 340;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientIsValidUserNameAndPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnIsValidUserNameAndPassword_OnClick(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        if (!common.myBool(hdnIsValidPassword.Value))
        {
            lblMessage.Text = "Invalid Username/Password !";
            return;
        }
        if (common.myInt(hdnIsValidPassword.Value).Equals(1))
        {
            lblMessage.Text = "";
            saveRecord();
        }
    }
    #endregion

    private void BindReasonMaster()
    {
        WardManagement objwd = new BaseC.WardManagement();
        DataTable dt = new DataTable();
        try
        {
            dt = objwd.GetReasonMasterList(1, common.myInt(Session["FacilityId"]), "IPR");
            if (common.myInt(dt.Rows.Count) > 0)
            {
                ddlReason.DataSource = dt;
                ddlReason.DataTextField = "ReasonName";
                ddlReason.DataValueField = "ReasonId";
                ddlReason.DataBind();
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            objwd = null;
            dt.Dispose();
        }
    }

}