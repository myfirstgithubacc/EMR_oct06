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
using System.Globalization;
using System.Data.SqlClient;

public partial class Include_Components_PaymentMode : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private int settlementchk = 0;
    public int billsettlementchk
    {
        get { return settlementchk; }
        set { settlementchk = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkMultiplePayMode();
            hdnDebitCardPercentage.Value = common.myStr(common.myDbl(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                                                    "PercentageOfServiceChargeOnDebitCard", sConString)));
            hdnCreditCardPercentage.Value = common.myStr(common.myDbl(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                                                    "PercentageOfServiceChargeOnCreditCard", sConString)));
            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        }
    }

    protected void grdPaymentMode_ItemDataBound(object sender, GridItemEventArgs e)
    {
        BaseC.HospitalSetup objHospitalSetup = new BaseC.HospitalSetup(sConString);
        ViewState["CardNoRequired"] = objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "PaymentModesCardNumberRequired");
        if (e.Item is GridHeaderItem)
        {
            if (common.myStr(ViewState["CardNoRequired"]) == "N")
            {
                e.Item.Cells[10].Visible = false;
            }

        }
        if (e.Item is GridDataItem)
        {
            if (common.myStr(ViewState["CardNoRequired"]) == "N")
            {
                e.Item.Cells[10].Visible = false;
            }
            Button btnpr = e.Item.FindControl("btnProcesspayment") as Button;
            Button btnres = e.Item.FindControl("btnCheckREsponse") as Button;
            DropDownList ddlPaymod = (DropDownList)e.Item.FindControl("ddlMode");
            TextBox txamt = e.Item.FindControl("txtAmount") as TextBox;
            TextBox itemAmount = (TextBox)e.Item.FindControl("txtAmount");
            TextBox itemBalance = (TextBox)e.Item.FindControl("txtBalance");
            HiddenField hdnAmount = (HiddenField)e.Item.FindControl("hdnAmount");
            DropDownList ddlBnk = (DropDownList)e.Item.FindControl("ddlBankName");
            DropDownList ddlCrdCard = (DropDownList)e.Item.FindControl("ddlCreditCard");
            DropDownList ddlClientBankName = (DropDownList)e.Item.FindControl("ddlClientBankName");
            itemAmount.Text = common.myStr(common.myDec(itemAmount.Text));
            HiddenField hdnBankID = (HiddenField)e.Item.FindControl("hdnBankID");
            HiddenField hdnClientBankId = (HiddenField)e.Item.FindControl("hdnClientBankId");
            HiddenField hdnMode = (HiddenField)e.Item.FindControl("hdnMode");
            TextBox txtCheque = (TextBox)e.Item.FindControl("txtChequeNo");
            RadDatePicker txtChqDate = (RadDatePicker)e.Item.FindControl("txtChequeDate");
            HiddenField hdnChkDate = (HiddenField)e.Item.FindControl("hdnChkDate");
            HiddenField hdnTypeMappingCode = (HiddenField)e.Item.FindControl("hdnTypeMappingCode");
            TextBox txtDescription = (TextBox)e.Item.FindControl("txtDescription");
            Char delimiter = '(';
            String[] substrings = null;
            if (!txtDescription.Text.Trim().Equals(string.Empty))
            {
                substrings = txtDescription.Text.Split(delimiter);
                txtDescription.Text = common.myStr(substrings[0]);
            }

            btnpr.Attributes.Add("onclick", "javascript:InitiatePayment('" + txamt.ClientID + "')");

            txtCheque.Attributes.Add("onchange", "javascript:ReplaceString('" + txtCheque.ClientID + "')");
            TextBox txtDesc = (TextBox)e.Item.FindControl("txtDescription");
            txtDesc.Attributes.Add("onchange", "javascript:ReplaceString('" + txtDesc.ClientID + "')");


            hdnAmount.Value = itemAmount.Text;
            itemAmount.Attributes.Add("onkeyup", "javascript:iteratePaymentModeGrid()");

            txtChqDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            txtChqDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            ddlBnk.SelectedIndex = -1;

            DataSet ds = GetPaymentModeAndBankMaster();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                DataView dv = ds.Tables[0].DefaultView;
                if (billsettlementchk == 0)
                {
                    dv.RowFilter = "Name<>'URC'";
                }
                dt = dv.ToTable();
                //BindControl(ddlPaymod, dt);
                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Text = (string)dr["Name"];
                    item.Value = dr["Id"].ToString();
                    item.Attributes.Add("TypeMappingCode", common.myStr(dr["TypeMappingCode"]));
                    ddlPaymod.Items.Add(item);
                    ddlPaymod.DataBind();
                }
                if (hdnMode != null)
                    ddlPaymod.SelectedIndex = ddlPaymod.Items.IndexOf(ddlPaymod.Items.FindByValue(common.myStr(hdnMode.Value)));
            }


            if (!string.IsNullOrEmpty(common.myStr(DataBinder.Eval(e.Item.DataItem, "ModeDate"))))
            {
                hdnChkDate.Value = common.myStr(DataBinder.Eval(e.Item.DataItem, "ModeDate"));
                txtChqDate.SelectedDate = common.myDate(DataBinder.Eval(e.Item.DataItem, "ModeDate"));
            }

            //  DropDownList ddlMode = (DropDownList)e.Item.FindControl("ddlMode");
            ddlClientBankName.DataSource = BindBeneficiary(common.myInt(ddlPaymod.SelectedValue));
            ddlClientBankName.DataTextField = "Name";
            ddlClientBankName.DataValueField = "ID";
            ddlClientBankName.DataBind();
            ddlClientBankName.Items.Insert(0, new ListItem("-Select-", "0"));

            if (ddlPaymod.SelectedItem.Attributes["TypeMappingCode"].Equals("1"))
            {
                ddlBnk.Enabled = false;
                ddlCrdCard.Visible = false;
                txtCheque.Enabled = false;
                txtChqDate.Enabled = false;
                ddlClientBankName.Enabled = false;
                hdnTypeMappingCode.Value = ddlPaymod.SelectedItem.Attributes["TypeMappingCode"];

            }
            else if (ddlPaymod.SelectedItem.Attributes["TypeMappingCode"].Equals("2"))
            {
                HiddenField hdnCreditID = (HiddenField)e.Item.FindControl("hdnCreditID");
                if (ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
                    BindControl(ddlCrdCard, ds.Tables[2]);
                ddlCrdCard.Visible = true;
                ddlBnk.Visible = false;
                txtCheque.Enabled = true;
                txtChqDate.Enabled = true;
                ddlClientBankName.Enabled = true;
                btnpr.Visible = true;
                btnres.Visible = true;
                ddlCrdCard.SelectedIndex = ddlCrdCard.Items.IndexOf(ddlCrdCard.Items.FindByValue(hdnCreditID.Value));
                ddlClientBankName.SelectedIndex = ddlClientBankName.Items.IndexOf(ddlClientBankName.Items.FindByValue(hdnClientBankId.Value));
                hdnTypeMappingCode.Value = ddlPaymod.SelectedItem.Attributes["TypeMappingCode"];

            }
            else if (ddlPaymod.SelectedItem.Attributes["TypeMappingCode"].Equals("3"))
            {
                HiddenField hdnCreditID = (HiddenField)e.Item.FindControl("hdnCreditID");
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    BindControl(ddlBnk, ds.Tables[1]);
                ddlCrdCard.Visible = false;
                ddlBnk.Visible = true;
                txtCheque.Enabled = true;
                txtChqDate.Enabled = true;
                ddlClientBankName.Enabled = true;
                ddlBnk.SelectedIndex = ddlBnk.Items.IndexOf(ddlBnk.Items.FindByValue(hdnBankID.Value));
                ddlClientBankName.SelectedIndex = ddlClientBankName.Items.IndexOf(ddlClientBankName.Items.FindByValue(hdnClientBankId.Value));
                hdnTypeMappingCode.Value = ddlPaymod.SelectedItem.Attributes["TypeMappingCode"];
            }
            else
            {
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    BindControl(ddlBnk, ds.Tables[1]);
                ddlBnk.Enabled = true;
                ddlCrdCard.Visible = false;
                txtCheque.Enabled = true;
                txtChqDate.Enabled = true;
                ddlClientBankName.Enabled = true;
                ddlBnk.SelectedIndex = ddlBnk.Items.IndexOf(ddlBnk.Items.FindByValue(hdnBankID.Value));
                ddlClientBankName.SelectedIndex = ddlClientBankName.Items.IndexOf(ddlClientBankName.Items.FindByValue(hdnClientBankId.Value));
                hdnTypeMappingCode.Value = ddlPaymod.SelectedItem.Attributes["TypeMappingCode"];
            }
        }
    }

    protected DataSet GetPaymentModeAndBankMaster()
    {
        if (ViewState["PaymentModeAndBankMaster"] == null)
        {
            BaseC.clsEMRBilling oclsEMRBilling = new BaseC.clsEMRBilling(sConString);
            ViewState["PaymentModeAndBankMaster"] = oclsEMRBilling.GetPaymentModeAndBankMaster(common.myInt(Session["FacilityId"]));
        }
        return (DataSet)ViewState["PaymentModeAndBankMaster"];
    }

    protected void BindControl(DropDownList ddl, DataTable dt)
    {
        try
        {
            ddl.DataSource = dt;
            ddl.DataTextField = "Name";
            ddl.DataValueField = "ID";
            ddl.DataBind();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            dt.Dispose();
        }
    }
    private DataSet BindBeneficiary(int ModeId)
    {
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@intModeId", ModeId);
        HshIn.Add("@intFacilityid", common.myInt(Session["FacilityId"]));
        string qry = "select Name,c.Id from ClientBankmaster c inner join ClientMasterModeTagging p on c.ID=p.ClientBankId " +
            " where P.ModeId =( case when  @intModeId =0 then  P.ModeId else @intModeId end ) AND c.Active=1 and c.Facilityid = @intFacilityid And p.Active=1 ORDER BY  Name  ";

        DataSet ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

        return ds;

    }

    protected void grdPaymentMode_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                int iRowIndx = e.Item.ItemIndex;

                DataTable dT = new DataTable();
                DataRow dR;
                DataTable dT1 = new DataTable();

                dT1 = (DataTable)Cache["DataTable"];
                if (dT1 == null)
                {
                    DataTable defT;
                    DataColumn defC;
                    defT = new DataTable("Payment");
                    defC = new DataColumn("ModeID", typeof(Int32));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);
                    defC = new DataColumn("BankID", typeof(Int32));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);
                    defC = new DataColumn("CreditCardId", typeof(Int32));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);
                    defC = new DataColumn("ClientBankId", typeof(Int32));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);

                    defC = new DataColumn("ModeNo", typeof(String));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);
                    defC = new DataColumn("ModeDate", typeof(DateTime));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);
                    defC = new DataColumn("Amount", typeof(String));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);
                    defC = new DataColumn("Balance", typeof(String));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);

                    defC = new DataColumn("Description", typeof(String));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);

                    defC = new DataColumn("CardSwipingValue", typeof(String));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);

                    defC = new DataColumn("TransactionRefNo", typeof(String));
                    defC.AllowDBNull = true;
                    defT.Columns.Add(defC);


                    dT1 = defT;
                    dT = dT1.Clone();
                }
                else
                    dT = dT1.Clone();

                string sAmount = "";

                decimal Tamount = common.myDec(txtNetBAmount.Text);

                foreach (GridDataItem item in grdPaymentMode.MasterTableView.Items)
                {
                    if (item.ItemIndex != iRowIndx)
                    {
                        if (item is GridDataItem)
                        {
                            dR = dT.NewRow();
                            DropDownList dropMode = (DropDownList)item.FindControl("ddlMode");
                            HiddenField hdnTypeMappingCode = (HiddenField)item.FindControl("hdnTypeMappingCode");
                            dR["ModeID"] = dropMode.SelectedValue;
                            //if (dropMode.SelectedItem.Text.ToUpper().Trim() == "CREDIT CARD")
                            if (hdnTypeMappingCode.Value.Equals("2"))
                            {
                                DropDownList dropCredit = (DropDownList)item.FindControl("ddlCreditCard");
                                dR["CreditCardId"] = common.myInt(dropCredit.SelectedValue);
                                //dR["BankID"] = 0;
                            }
                            else
                            {
                                DropDownList dropBank = (DropDownList)item.FindControl("ddlBankName");
                                dR["BankID"] = common.myInt(dropBank.SelectedValue);
                                //dR["CreditCardId"] = 0;
                            }

                            DropDownList ddlClientBankName = (DropDownList)item.FindControl("ddlClientBankName");
                            dR["ClientBankId"] = common.myInt(ddlClientBankName.SelectedValue);

                            TextBox txtModeNo = (TextBox)item.FindControl("txtChequeNo");
                            dR["ModeNo"] = txtModeNo.Text;
                            RadDatePicker txtModeDate = (RadDatePicker)item.FindControl("txtChequeDate");
                            if (dropMode.SelectedItem.Value.ToString() != "1")
                            {
                                dR["ModeDate"] = Convert.ToDateTime(txtModeDate.SelectedDate.Value);//txtModeDate.SelectedDate.Value.ToString();
                            }
                            TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
                            dR["Amount"] = common.myDec(txtAmount.Text);
                            dR["Balance"] = (Tamount - common.myDec(dR["Amount"])).ToString();
                            Tamount = Tamount - common.myDec(dR["Amount"]);
                            HiddenField hdnAmount = (HiddenField)item.FindControl("hdnAmount");
                            TextBox txtDesc = (TextBox)item.FindControl("txtDescription");
                            dR["Description"] = txtDesc.Text;

                            TextBox txtTransactionRefNo = (TextBox)item.FindControl("txtTransactionRefNo");
                            dR["TransactionRefNo"] = txtTransactionRefNo.Text;

                            dT.Rows.Add(dR);
                            sAmount = txtBalance.Text;
                        }
                    }
                }

                Cache["DataTable"] = dT;
                grdPaymentMode.DataSource = (DataTable)Cache["DataTable"];
                grdPaymentMode.DataBind();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlMode_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            decimal Tamount = 0;
            if (txtNetBAmount.Text != "")
                Tamount = common.myDec(txtNetBAmount.Text);
            DataTable dT;
            DataColumn dC;
            DataRow dR;
            dT = new DataTable("Payment");

            dC = new DataColumn("ModeID", typeof(Int32));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);
            dC = new DataColumn("BankID", typeof(Int32));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);
            dC = new DataColumn("CreditCardId", typeof(Int32));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);
            dC = new DataColumn("ClientBankId", typeof(Int32));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);
            dC = new DataColumn("ModeNo", typeof(String));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);
            dC = new DataColumn("ModeDate", typeof(DateTime));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);
            dC = new DataColumn("Amount", typeof(String));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);
            dC = new DataColumn("Balance", typeof(String));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);
            dC = new DataColumn("Description", typeof(String));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);
            dC = new DataColumn("CardSwipingValue", typeof(String));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);

            dC = new DataColumn("TransactionRefNo", typeof(String));
            dC.AllowDBNull = true;
            dT.Columns.Add(dC);

            decimal RemainingAmount = Tamount;
            foreach (GridDataItem item in grdPaymentMode.MasterTableView.Items)
            {
                if (item is GridDataItem)
                {
                    dR = dT.NewRow();
                    DropDownList dropMode = (DropDownList)item.FindControl("ddlMode");
                    HiddenField hdnTypeMappingCode = (HiddenField)item.FindControl("hdnTypeMappingCode");
                    dR["ModeID"] = common.myInt(dropMode.SelectedValue);


                    TextBox txtModeNo = (TextBox)item.FindControl("txtChequeNo");
                    dR["ModeNo"] = common.myStr(txtModeNo.Text);

                    RadDatePicker txtChqDate = (RadDatePicker)item.FindControl("txtChequeDate");

                    if (dropMode.SelectedItem.Value.ToString() != "1")
                    {
                        DropDownList dropBank = (DropDownList)item.FindControl("ddlBankName");
                        dR["BankID"] = common.myInt(dropBank.SelectedValue);

                        HiddenField hdnChkDate = (HiddenField)item.FindControl("hdnChkDate");
                        if (hdnChkDate.Value != "")
                        {
                            txtChqDate.SelectedDate = Convert.ToDateTime(hdnChkDate.Value);
                            //dR["ModeDate"] = txtChqDate.SelectedDate.Value.ToString();
                            dR["ModeDate"] = Convert.ToDateTime(txtChqDate.SelectedDate.Value);
                        }
                        // if (dropMode.SelectedItem.Value.ToString() == "3")
                        if (hdnTypeMappingCode.Value.Equals("2"))
                        {
                            DropDownList dropCredit = (DropDownList)item.FindControl("ddlCreditCard");
                            dR["CreditCardId"] = common.myInt(dropCredit.SelectedValue);
                        }
                    }
                    else
                    {
                        dR["ModeDate"] = DateTime.Now;
                    }

                    HiddenField hdnClientBankId = (HiddenField)item.FindControl("hdnClientBankId");
                    dR["ClientBankId"] = common.myInt(hdnClientBankId.Value);

                    TextBox txtAmount = (TextBox)item.FindControl("txtAmount");

                    dR["Amount"] = common.myDec(txtAmount.Text);
                    TextBox txtBalance = (TextBox)item.FindControl("txtBalance");
                    //dR["Balance"] = common.FormatNumber(txtBalance.Text, Convert.ToInt32(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                    //dR["Balance"] = common.FormatNumber((Tamount - Convert.ToDouble(dR["Amount"])).ToString(), Convert.ToInt32(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                    dR["Balance"] = (RemainingAmount - common.myDec(dR["Amount"])).ToString();
                    RemainingAmount = RemainingAmount - common.myDec(txtAmount.Text);

                    TextBox txtDesc = (TextBox)item.FindControl("txtDescription");
                    dR["Description"] = txtDesc.Text;
                    dR["CardSwipingValue"] = GetCardSwapingValue(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(dR["ModeID"]), common.myDec(dR["Amount"]));// common.FormatNumber(txtAmount.Text, Convert.ToInt32(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));

                    TextBox txtTransactionRefNo = (TextBox)item.FindControl("txtTransactionRefNo");
                    dR["TransactionRefNo"] = txtTransactionRefNo.Text;
                    dT.Rows.Add(dR);
                }
            }

            Cache["DataTable"] = dT;
            grdPaymentMode.DataSource = dT;
            grdPaymentMode.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public decimal GetCardSwapingValue(int HospId, int FacilityId, int CardTypeId, decimal Amount)
    {
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("HospitalLocationId", HospId);
        HshIn.Add("facilityId", FacilityId);
        HshIn.Add("CardType", CardTypeId);
        HshIn.Add("Amount", Amount);
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection(sConString);
        cmd.Connection = con;
        con.Open();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "Select [dbo].[GetCardSwapingValue](" + CardTypeId + "," + Amount + "," + HospId + "," + FacilityId + ",'" + common.myStr(Session["SourceCardPayment"]) + "')";
        //Double CardSwapingValue = Math.Ceiling(Convert.ToDouble(cmd.ExecuteScalar()));
        decimal CardSwapingValue = common.myDec(cmd.ExecuteScalar());
        con.Close();
        return CardSwapingValue;
    }

    protected void lnkAddRow_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dT = new DataTable();
            DataRow dR;
            DataTable dT1 = new DataTable();

            dT1 = (DataTable)Cache["DataTable"];

            if (dT1 == null)
            {
                DataTable defT;
                DataColumn defC;
                defT = new DataTable("Payment");
                defC = new DataColumn("ModeID", typeof(Int32));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);
                defC = new DataColumn("BankID", typeof(Int32));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);
                defC = new DataColumn("CreditCardId", typeof(Int32));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);
                defC = new DataColumn("ClientBankId", typeof(Int32));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);

                defC = new DataColumn("ModeNo", typeof(String));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);
                defC = new DataColumn("ModeDate", typeof(DateTime));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);
                defC = new DataColumn("Amount", typeof(String));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);
                defC = new DataColumn("Balance", typeof(String));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);

                defC = new DataColumn("Description", typeof(String));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);

                defC = new DataColumn("CardSwipingValue", typeof(String));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);

                defC = new DataColumn("TransactionRefNo", typeof(String));
                defC.AllowDBNull = true;
                defT.Columns.Add(defC);

                dT1 = defT;
                dT = dT1.Clone();
            }
            else
                dT = dT1.Clone();

            string sAmount = "";
            decimal Tamount = 0;
            if (txtNetBAmount.Text != "")
                Tamount = common.myDec(txtNetBAmount.Text);

            foreach (GridDataItem item in grdPaymentMode.MasterTableView.Items)
            {
                if (item is GridDataItem)
                {
                    dR = dT.NewRow();
                    HiddenField hdnTypeMappingCode = (HiddenField)item.FindControl("hdnTypeMappingCode");
                    DropDownList dropMode = (DropDownList)item.FindControl("ddlMode");
                    dR["ModeID"] = dropMode.SelectedValue;
                    if (hdnTypeMappingCode.Value.Equals("2"))
                    {
                        DropDownList dropCredit = (DropDownList)item.FindControl("ddlCreditCard");
                        dR["CreditCardId"] = common.myInt(dropCredit.SelectedValue);
                        dR["BankID"] = 0;
                    }
                    else
                    {
                        DropDownList dropBank = (DropDownList)item.FindControl("ddlBankName");
                        dR["BankID"] = common.myInt(dropBank.SelectedValue);
                        dR["CreditCardId"] = 0;
                    }

                    // HiddenField hdnClientBankId = (HiddenField)item.FindControl("hdnClientBankId");
                    // dR["ClientBankId"] = common.myInt(hdnClientBankId.Value);
                    DropDownList ddlClientBankName = (DropDownList)item.FindControl("ddlClientBankName");
                    dR["ClientBankId"] = common.myInt(ddlClientBankName.SelectedValue);



                    TextBox txtModeNo = (TextBox)item.FindControl("txtChequeNo");
                    dR["ModeNo"] = txtModeNo.Text;
                    RadDatePicker txtModeDate = (RadDatePicker)item.FindControl("txtChequeDate");
                    if (dropMode.SelectedItem.Value.ToString() != "1")
                        dR["ModeDate"] = Convert.ToDateTime(txtModeDate.SelectedDate.Value); //txtModeDate.SelectedDate.Value.ToString();
                    //else
                    //    dR["ModeDate"] = "";
                    TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
                    dR["Amount"] = common.myInt(txtAmount.Text);
                    dR["Balance"] = common.myStr(Tamount - common.myInt(dR["Amount"]));

                    HiddenField hdnCardSwipingValue = (HiddenField)item.FindControl("hdnCardSwipingValue");
                    dR["CardSwipingValue"] = hdnCardSwipingValue.Value;

                    Tamount = Tamount - common.myInt(dR["Amount"]);
                    HiddenField hdnAmount = (HiddenField)item.FindControl("hdnAmount");

                    TextBox txtDesc = (TextBox)item.FindControl("txtDescription");
                    dR["Description"] = txtDesc.Text;

                    TextBox txtTransactionRefNo = (TextBox)item.FindControl("txtTransactionRefNo");
                    dR["TransactionRefNo"] = txtTransactionRefNo.Text;

                    dT.Rows.Add(dR);
                    sAmount = txtBalance.Text;
                }
            }

            Cache["DataTable"] = dT;
            if (Tamount > 0)
            {
                dR = dT.NewRow();
                dR["ModeID"] = 1;
                dR["BankID"] = 0;
                dR["CreditCardId"] = 0;
                dR["ClientBankId"] = 0;
                dR["Amount"] = Tamount.ToString();
                dR["Balance"] = "0";
                dR["ModeDate"] = DateTime.Now;
                dR["CardSwipingValue"] = Tamount.ToString();
                dR["TransactionRefNo"] = "";
                dT.Rows.Add(dR);
            }
            //if (Tamount == 0)
            //{
            //    dR = dT.NewRow();
            //    dR["ModeID"] = 1;
            //    dR["BankID"] = 0;
            //    dR["CreditCardId"] = 0;
            //    dR["Amount"] = common.FormatNumber(Tamount.ToString(), Convert.ToInt32(Session["HospitalLocationID"]), sConString);
            //    dR["Balance"] = common.FormatNumber("0", Convert.ToInt32(Session["HospitalLocationID"]), sConString);
            //    dR["ModeDate"] = DateTime.Now;
            //    dT.Rows.Add(dR);
            //}
            grdPaymentMode.DataSource = (DataTable)Cache["DataTable"];
            grdPaymentMode.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void lnkMultiplePayMode()
    {
        try
        {
            grdPaymentMode.Visible = true;
            //Bind Payment mode gird

            DataTable defT;
            DataColumn defC;
            DataRow defR;
            defT = new DataTable("Payment");
            defC = new DataColumn("ModeID", typeof(Int32));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);
            defC = new DataColumn("BankID", typeof(Int32));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);
            defC = new DataColumn("CreditCardId", typeof(Int32));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);
            defC = new DataColumn("ClientBankId", typeof(Int32));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);

            defC = new DataColumn("ModeNo", typeof(String));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);
            defC = new DataColumn("ModeDate", typeof(DateTime));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);
            defC = new DataColumn("Amount", typeof(String));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);
            defC = new DataColumn("Balance", typeof(String));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);

            defC = new DataColumn("Description", typeof(String));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);

            defC = new DataColumn("CardSwipingValue", typeof(String));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);

            defC = new DataColumn("TransactionRefNo", typeof(String));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);




            defR = defT.NewRow();
            defR["ModeID"] = 1;
            defR["BankID"] = 0;
            defR["CreditCardId"] = 0;
            defR["ClientBankId"] = 0;

            defR["ModeNo"] = "";
            defR["ModeDate"] = DateTime.Now;
            defR["TransactionRefNo"] = "";
            if (txtNetBAmount.Text != "")
                defR["Amount"] = common.myDec(txtNetBAmount.Text.Trim());
            else
                defR["Amount"] = "0";
            defR["Balance"] = "0";
            defR["Description"] = "";
            defR["CardSwipingValue"] = "0";
            defR["TransactionRefNo"] = "";
            defT.Rows.Add(defR);
            Cache["DataTable"] = defT;
            grdPaymentMode.DataSource = defT;
            grdPaymentMode.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCheckREsponse_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (common.myStr(hdnrefno.Value) == "")
            {
                Alert.ShowAjaxMsg("No Pending Request Found", Page);
                return;
            }
            else
            {
                //Alert.ShowAjaxMsg(common.myStr(hdnrefno.Value),Page);
            }
            GridDataItem row = (GridDataItem)(((Button)sender).NamingContainer);
            DataSet ds = dl.FillDataSet(CommandType.Text, "select Uhid,ApprovalStatus,ApprovalCode,Remark, CardNumber, PineLabBatchID, CardHolderName+' '+'(TRAN ID- '+Uhid+')' as CardHolderName  from EDCPayment with(nolock)  where ltrim(rtrim(Uhid)) ='" + common.myStr(hdnrefno.Value).Trim() + "'");
            TextBox txtpayref = row.FindControl("txtChequeNo") as TextBox;
            TextBox txtdesc = row.FindControl("txtDescription") as TextBox;
            TextBox txtAmount = row.FindControl("txtAmount") as TextBox;
            TextBox txtTransactionRefNo = row.FindControl("txtTransactionRefNo") as TextBox;
            Button btnCheckREsponse = row.FindControl("btnCheckREsponse") as Button;
            Button btnProcesspayment = row.FindControl("btnProcesspayment") as Button;
            DropDownList ddlMode = row.FindControl("ddlMode") as DropDownList;
            //Alert.ShowAjaxMsg(common.myStr(ds.Tables[0].Rows.Count), Page);
            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["ApprovalStatus"].ToString() == "APPROVED")
                {
                    //Alert.ShowAjaxMsg(common.myStr(ds.Tables[0].Rows.Count), Page);
                    txtpayref.Text = common.myStr(ds.Tables[0].Rows[0]["PineLabBatchID"]);
                    txtdesc.Text = common.myStr(ds.Tables[0].Rows[0]["CardHolderName"]).ToString();
                    txtTransactionRefNo.Text = common.myStr(ds.Tables[0].Rows[0]["CardNumber"]).ToString();
                    btnProcesspayment.Visible = false;
                    btnCheckREsponse.Visible = false;
                    ddlMode.Enabled = false;
                    txtAmount.Enabled = false;
                    Alert.ShowAjaxMsg(common.myStr(ds.Tables[0].Rows[0]["ApprovalStatus"].ToString()), Page);
                }
                else
                {
                    btnProcesspayment.Visible = true;
                    btnCheckREsponse.Visible = true;
                    ddlMode.Enabled = true;
                    txtAmount.Enabled = true;
                    Alert.ShowAjaxMsg(ds.Tables[0].Rows[0]["Remark"].ToString(), Page);
                }

            }
            //Alert.ShowAjaxMsg("Check Print", Page);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        //Alert.ShowAjaxMsg(common.myStr(hdnrefno.Value),Page);
    }
}
