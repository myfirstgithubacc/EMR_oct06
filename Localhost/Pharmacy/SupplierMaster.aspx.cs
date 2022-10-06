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
using System.Data.SqlClient;
using System.Globalization;

public partial class Pharmacy_SupplierMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    StringBuilder strXML;
    ArrayList coll;
    clsExceptionLog objException = new clsExceptionLog();
    bool RowSelStauts = false;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["MP"]).Equals("NO"))
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myBool(Session["MainFacility"]) )
            {
                btnSaveData.Visible = true;
            }
            else
            {
                btnSaveData.Visible = false;
            }
            dropLState.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
            dropLCity.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
            ddlZip.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            objPharmacy = new BaseC.clsPharmacy(sConString);

            if (common.myStr(Request.QueryString["MasterType"]) == "MM")
            {
                lblHeader.Text = "Manufacturer&nbsp;Master";
                lblHeader.ToolTip = "Manufacturer Master";
                lblDetailsHeading.Text = "Manufacturer&nbsp;Master&nbsp;Details";
                trType.Visible = false;

                lblChallanAccept.Visible = false;
                rdoChallanAccept.Visible = false;

                // trTaxDetails.Visible = false;
                tbl1.Visible = false;
                gvTax.Visible = false;

                Label20.Visible = false;
                spnVenderPosting.Visible = false;
                ddlVenderPosting.Visible = false;

                tdCheckBoxListFacility.Visible = false;
                Label7.Visible = false;
                spnFacility.Visible = false;

                trMedical.Visible = true;
                trArea.Visible = true;
                trZonal.Visible = true;
                trRegional.Visible = true;
            }

            btnClose.Visible = false;

            if (common.myStr(Request.QueryString["MasterPage"]).ToUpper() == "NO")
            {
                //btnClose.Visible = true;
                RadPane mpLeftPannel = (RadPane)Master.FindControl("LeftPnl");
                mpLeftPannel.Visible = false;
                RadSplitBar mpSplitBar = (RadSplitBar)Master.FindControl("Radsplitbar1");
                mpSplitBar.Visible = false;
                RadPane mpTopPnl = (RadPane)Master.FindControl("TopPnl");
                mpTopPnl.Visible = false;
                RadPane mpEndPane = (RadPane)Master.FindControl("EndPane");
                mpEndPane.Visible = false;
                RadMenu rdmenu = (RadMenu)Master.FindControl("RadMenu1");
                rdmenu.Visible = false;

                System.Web.UI.HtmlControls.HtmlTable tblEnd = (System.Web.UI.HtmlControls.HtmlTable)Master.FindControl("tblEnd");
                //tblEnd.Visible = false;

                btnClose.Visible = true;
                //tdHeader.Visible = false;
            }

            ViewState["_ID"] = "0";
            BindVenderPosting();
            bindControl();
            bindCountry();

            txtName.Attributes.Add("onblur", "nSat=1;");
            txtShortName.Attributes.Add("onblur", "nSat=1;");
            txtAdd1.Attributes.Add("onblur", "nSat=1;");
            txtAdd2.Attributes.Add("onblur", "nSat=1;");
            txtAdd3.Attributes.Add("onblur", "nSat=1;");

            BindFacility();
            bindMonthDay();
            if (common.myStr(Request.QueryString["MP"]).Equals("NO"))
            {
                lnkTagging.Visible = false;
            }
        }
    }
    private void BindVenderPosting()
    {
        objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet ds = objPharmacy.getVendorPostingGroup();

        ddlVenderPosting.DataSource = ds.Tables[0];
        ddlVenderPosting.DataTextField = "Description";
        ddlVenderPosting.DataValueField = "id";
        ddlVenderPosting.DataBind();
        ddlVenderPosting.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
        ddlVenderPosting.SelectedIndex = 0;
    }

    private void bindTax()
    {
        try
        {
            if (common.myStr(Request.QueryString["MasterType"]) == "SM")
            {
                objPharmacy = new BaseC.clsPharmacy(sConString);
                DataSet ds = objPharmacy.getSupCustomFieldDetails(common.myInt(ViewState["_ID"]), false, "S");

                gvTax.DataSource = ds.Tables[0];
                gvTax.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    void BindFacility()
    {
        try
        {
            if (Session["HospitalLocationID"] != null)
            {
                BaseC.Security objSec = new BaseC.Security(sConString);
                cblfacility.DataSource = objSec.GetFacilityName(Convert.ToInt16(Session["HospitalLocationID"]));
                cblfacility.DataTextField = "Name";
                cblfacility.DataValueField = "FacilityId";
                cblfacility.DataBind();
            }
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
            BaseC.clsLISPhlebotomy objPhlebotomy = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objPhlebotomy.getStatus(common.myInt(Session["HospitalLocationID"]), "SupplierType", "");

            ddlSupplierType.DataSource = ds.Tables[0];
            ddlSupplierType.DataTextField = "Status";
            ddlSupplierType.DataValueField = "StatusId";
            ddlSupplierType.DataBind();

            ddlSupplierType.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlSupplierType.SelectedIndex = 0;


            ds = objPhlebotomy.getStatus(common.myInt(Session["HospitalLocationID"]), "ItemNature", "");

            ddlSupplierCategoryType.DataSource = ds.Tables[0];
            ddlSupplierCategoryType.DataTextField = "Status";
            ddlSupplierCategoryType.DataValueField = "StatusId";
            ddlSupplierCategoryType.DataBind();

            ddlSupplierCategoryType.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlSupplierCategoryType.SelectedIndex = 0;

            bindTax();
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
        if (RowSelStauts)
        {
            bindTax();
        }
    }

    protected void bindCountry()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT countryid,countryname FROM countrymaster Where Active = 1 order by countryname");
            ddlcountryname.DataSource = dr;
            ddlcountryname.DataTextField = "countryname";
            ddlcountryname.DataValueField = "countryid";
            ddlcountryname.DataBind();

            ddlcountryname.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));

            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myLen(txtName.Text) == 0)
        {
            strmsg += "Name can't be blank !";
            isSave = false;
        }

        //if (common.myInt(ddlSupplierType.SelectedValue) == 0 )
        //{
        //    strmsg += "Select Supplier Type ! ";
        //    isSave = false;
        //}

        if (txtPeriod.Text == "" && (common.myInt(ddlSupplierType.SelectedValue) == 118 || common.myInt(ddlSupplierType.SelectedValue) == 119)) // 118 =TMP,119 = TRL in statusmaste table
        {
            strmsg += "Validity period can't be blank ! ";
            isSave = false;
        }

        if (common.myInt(ViewState["_ID"]) == 0
            && common.myInt(ddlStatus.SelectedValue) != 1)
        {
            strmsg += "Status must be Active for New Data ! ";
            isSave = false;
        }

        if (common.myStr(Request.QueryString["MasterType"]) == "SM")
        {
            if (common.myInt(ddlVenderPosting.SelectedValue) == 0)
            {
                strmsg += "Select Vender Posting Group !";
                isSave = false;
            }
        }

        lblMessage.Text = strmsg;
        return isSave;
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        try
        {
            bool IsCallfromCSSD = false;
            if (Request.QueryString["MP"] != null && Request.QueryString["P"] != null && common.myStr(Request.QueryString["MP"]).Equals("NO")
                && common.myStr(Request.QueryString["P"]).Equals("CSSD"))
                IsCallfromCSSD = true;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }
            objPharmacy = new BaseC.clsPharmacy(sConString);

            strXML = new StringBuilder();
            StringBuilder strFacilities = new StringBuilder();
            coll = new ArrayList();

            if (gvTax != null)
            {
                foreach (GridDataItem dataItem in gvTax.Items)
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

                    strXML.Append(common.setXmlTable(ref coll));
                }
                //TextBox txtTaxValue = (TextBox)dataItem.FindControl("txtTaxValue");

                //if (common.myLen(txtTaxValue.Text) > 0)
                //{
                //    Label lblTaxId = (Label)dataItem.FindControl("lblTaxId");

                //    coll.Add(common.myInt(lblTaxId.Text));
                //    coll.Add(common.myStr(txtTaxValue.Text).ToUpper().Trim());

                //    strXML.Append(common.setXmlTable(ref coll));
                //}
            }

            foreach (ListItem item in cblfacility.Items)
            {
                if (item.Selected == true)
                {
                    strFacilities.Append("<Table1>");
                    strFacilities.Append("<c1>");
                    strFacilities.Append(item.Value);
                    strFacilities.Append("</c1>");
                    strFacilities.Append("</Table1>");
                }
            }

            if (strFacilities.Length == 0 && !IsCallfromCSSD)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select Facility !";
                return;
            }
            string mobile = "";
            if (txtMobile.Text != "")
            {
                mobile = common.escapeCharString(txtMobile.Text, false);
            }

            string phone = "";
            if (txtPhone.Text != "")
            {
                phone = common.escapeCharString(txtPhone.Text, false);
            }

            string fax = "";
            if (txtFax.Text != "")
            {
                fax = common.escapeCharString(txtFax.Text, false);
            }
            string StrPaymentModeId;
            StrPaymentModeId = common.GetCheckedItems(ddlMode);


            if (common.myStr(Request.QueryString["MasterType"]) == "SM")
            {
                coll = new ArrayList();
                foreach (ListItem item in cblfacility.Items)
                {
                    if (item.Selected)
                    {
                        coll.Add(common.myInt(item.Value));
                        strFacilities.Append(common.setXmlTable(ref coll));
                    }
                }

                if (strFacilities.Length == 0 && !IsCallfromCSSD)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please select Facility !";
                    return;
                }
            }
            

            string strMsg = objPharmacy.SaveSupplierMaster(common.myStr(Request.QueryString["MasterType"]),
                                common.myInt(ViewState["_ID"]), common.myInt(Session["HospitalLocationID"]),
                                common.myStr(txtName.Text, true).Trim(), common.myStr(txtShortName.Text, true).Trim(), common.myInt(ddlSupplierType.SelectedValue), common.myInt(ddlSupplierCategoryType.SelectedValue),
                                common.myInt(rdoChallanAccept.SelectedValue), common.myStr(txtAdd1.Text, true).Trim(), common.myStr(txtAdd2.Text, true).Trim(),
                                common.myStr(txtAdd3.Text, true).Trim(), common.myStr(txtEMail.Text, true).Trim(), common.myInt(ddlcountryname.SelectedValue), common.myInt(dropLState.SelectedValue),
                                common.myInt(dropLCity.SelectedValue), common.myInt(ddlZip.SelectedValue), mobile.Trim(),
                                phone.Trim(), fax.Trim(), common.myStr(txtContactPerson.Text, true).Trim(),
                                common.myStr(txtDesignation.Text, true).Trim(), common.myStr(txtContactPersonMobile.Text, true).Trim(), strXML.ToString(),
                                common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserID"]), Convert.ToString(strFacilities), common.myInt(txtPeriod.Text),
                                StrPaymentModeId, common.myInt(ddlPaymentMonths.SelectedValue), common.myInt(ddlPaymentDays.SelectedValue), common.myInt(ddlVenderPosting.SelectedValue),
                                txtMRName.Text, txtASMgr.Text, txtZSMgr.Text, txtRSMgr.Text, txtCode.Text.Trim(), IsCallfromCSSD);

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                clearControl();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                ViewState["_ID"] = "0";
                bindTax();

                ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
            }

            lblMessage.Text = strMsg;
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
        try
        {
            lblMessage.Text = "&nbsp;";

            txtName.Text = "";
            txtCode.Text = "";
            txtShortName.Text = "";
            ddlSupplierType.SelectedIndex = 0;
            ddlSupplierCategoryType.SelectedIndex = 0;
            rdoChallanAccept.SelectedIndex = 0;
            txtAdd1.Text = "";
            txtAdd2.Text = "";
            txtAdd3.Text = "";
            txtEMail.Text = "";
            ddlcountryname.SelectedIndex = ddlcountryname.Items.IndexOf(ddlcountryname.Items.FindItemByValue("0"));
            dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindItemByValue("0"));
            dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindItemByValue("0"));
            ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindItemByValue("0"));
            txtMobile.Text = "";
            txtPhone.Text = "";
            txtFax.Text = "";
            txtContactPerson.Text = "";
            txtDesignation.Text = "";
            txtContactPersonMobile.Text = "";
            ddlStatus.SelectedIndex = 0;
            cblfacility.ClearSelection();
            txtPeriod.Text = "";
            lblValidUpTo.Text = "";
            lblValidUpTo.Visible = false;
            ddlPaymentDays.SelectedValue = "0";
            ddlPaymentMonths.SelectedValue = "0";
            ddlVenderPosting.SelectedIndex = 0;
            ddlMode.ClearCheckedItems();

            txtRSMgr.Text = "";
            txtZSMgr.Text = "";
            txtMRName.Text = "";
            txtASMgr.Text = "";

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
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

    protected void LocalCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dropLState.Items.Clear();
            //populate Local State drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName from StateMaster where StateMaster.CountryID='" + ddlcountryname.SelectedValue.ToString() + "' ORDER BY StateName");
            dropLState.DataSource = dr;
            dropLState.DataTextField = "StateName";
            dropLState.DataValueField = "StateID";
            dropLState.DataBind();
            dr.Close();
            dropLState.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
            dropLState.Focus();

            LocalState_SelectedIndexChanged(sender, e);

            dropLState.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void LocalState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dropLCity.Items.Clear();
            //populate Local City drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select CityMaster.cityID, CityMaster.cityname from CityMaster where StateId='" + dropLState.SelectedValue.ToString() + "' ORDER BY CityMaster.cityname");
            dropLCity.DataSource = dr;
            dropLCity.DataTextField = "CityName";
            dropLCity.DataValueField = "CityID";
            dropLCity.DataBind();
            dr.Close();
            dropLCity.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
            LocalCity_OnSelectedIndexChanged(sender, e);

            dropLCity.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void LocalCity_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ddlZip.Items.Clear();
            //populate Local City drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select ZIPID, ZipCode from zipmaster where CityID='" + dropLCity.SelectedValue.ToString() + "' ORDER BY ZipCode");
            ddlZip.DataSource = dr;
            ddlZip.DataTextField = "ZipCode";
            ddlZip.DataValueField = "ZIPID";
            ddlZip.DataBind();
            dr.Close();
            ddlZip.Items.Insert(0, new RadComboBoxItem("[ Select ]", "0"));
            ddlZip.Items[0].Value = "0";

            ddlZip.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";

            if (common.myStr(Request.QueryString["MasterType"]) == "SM")
            {
                RadWindowForNew.NavigateUrl = "/Pharmacy/FindSupplierMaster.aspx";
            }
            else if (common.myStr(Request.QueryString["MasterType"]) == "MM")
            {
                if (common.myStr(Request.QueryString["P"]).Equals("CSSD"))
                    RadWindowForNew.NavigateUrl = "/Pharmacy/FindManufactureMaster.aspx?P=CSSD";
                else
                    RadWindowForNew.NavigateUrl = "/Pharmacy/FindManufactureMaster.aspx";
            }
            RadWindowForNew.Height = 520;
            RadWindowForNew.Width = 880;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientFindClose";
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

    protected void OnClientFindClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            if (common.myInt(hdnMasterId.Value) == 0)
            {
                return;
            }

            objPharmacy = new BaseC.clsPharmacy(sConString);

            DataSet dsSearch;

            if (common.myStr(Request.QueryString["MasterType"]) == "SM")
            {
                dsSearch = objPharmacy.getSupplierMaster(common.myInt(Session["FacilityId"]), common.myInt(hdnMasterId.Value), common.myInt(Session["HospitalLocationID"]), 0,
                              "", "", common.myInt(Session["UserId"]));

                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    ViewState["_ID"] = common.myInt(hdnMasterId.Value);
                    bindTax();
                    if (common.myStr(Request.QueryString["MasterType"]) == "SM")
                    {
                        cblfacility.ClearSelection();
                        for (int i = 0; i < dsSearch.Tables[1].Rows.Count; i++)
                        {
                            ListItem lstFacility = (ListItem)cblfacility.Items.FindByValue(dsSearch.Tables[1].Rows[i]["FacilityId"].ToString().Trim());
                            if (lstFacility != null)
                            {
                                lstFacility.Selected = true;
                            }
                        }
                    }

                    DataRow DR = dsSearch.Tables[0].Rows[0];

                    txtName.Text = common.myStr(DR["SupplierName"]);
                    txtCode.Text = common.myStr(DR["SupplierNo"]);

                    txtShortName.Text = common.myStr(DR["SupplierShortName"]);
                    ddlSupplierType.SelectedIndex = ddlSupplierType.Items.IndexOf(ddlSupplierType.Items.FindItemByValue(common.myStr(DR["SupplierTypeId"])));
                    ddlSupplierCategoryType.SelectedIndex = ddlSupplierCategoryType.Items.IndexOf(ddlSupplierCategoryType.Items.FindItemByValue(common.myStr(DR["SupplierCategoryTypeId"])));

                    rdoChallanAccept.SelectedValue = "0";
                    if (common.myBool(DR["ChallanAccept"]))
                    {
                        rdoChallanAccept.SelectedValue = "1";
                    }

                    txtAdd1.Text = common.myStr(DR["Add1"]);
                    txtAdd2.Text = common.myStr(DR["Add2"]);
                    txtAdd3.Text = common.myStr(DR["Add3"]);
                    txtEMail.Text = common.myStr(DR["EMail"]);
                    txtMobile.Text = common.myStr(DR["Mobile"]);
                    txtPhone.Text = common.myStr(DR["Phone"]);
                    txtFax.Text = common.myStr(DR["Fax"]);

                    txtContactPerson.Text = common.myStr(DR["ContactPerson"]);
                    txtDesignation.Text = common.myStr(DR["Designation"]);
                    txtContactPersonMobile.Text = common.myStr(DR["ContactPersonMobile"]);

                    ddlcountryname.SelectedIndex = ddlcountryname.Items.IndexOf(ddlcountryname.Items.FindItemByValue(common.myStr(DR["CountryId"])));
                    LocalCountry_SelectedIndexChanged(null, null);
                    dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindItemByValue(common.myStr(DR["StateId"])));
                    LocalState_SelectedIndexChanged(null, null);
                    dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindItemByValue(common.myStr(DR["CityId"])));
                    LocalCity_OnSelectedIndexChanged(null, null);
                    ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindItemByValue(common.myStr(DR["ZipId"])));

                    ddlStatus.SelectedValue = (common.myBool(DR["Active"]) == true) ? "1" : "0";

                    ddlPaymentMonths.SelectedIndex = ddlPaymentMonths.Items.IndexOf(ddlPaymentMonths.Items.FindItemByValue(common.myStr(DR["CreditMonths"])));
                    ddlPaymentDays.SelectedIndex = ddlPaymentDays.Items.IndexOf(ddlPaymentDays.Items.FindItemByValue(common.myStr(DR["CreditDays"])));
                    ddlVenderPosting.SelectedIndex = ddlVenderPosting.Items.IndexOf(ddlVenderPosting.Items.FindItemByValue(common.myStr(DR["VendorPostingGroupId"])));
                    string[] PaymentMode = common.myStr(DR["PaymentMode"]).Split(',');

                    for (int i = 0; i < PaymentMode.Length; i++)
                    {
                        if (Convert.ToString(PaymentMode[i]).Trim() != "")
                        {
                            ddlMode.Items.FindItemByValue(Convert.ToString(PaymentMode[i])).Checked = true;
                        }
                    }

                    if (common.myInt(DR["SupplierTypeId"]) == 118 || common.myInt(DR["SupplierTypeId"]) == 119) // 118 =TMP,119 = TRL in statusmaste table
                    {
                        txtPeriod.Text = common.myStr(DR["ValidityPeriodInDays"]);
                        txtPeriod.Enabled = true;
                        lblPeriodStar.Visible = true;
                        lblValidUpTo.Visible = true;
                        if (txtPeriod.Text != "")
                        {
                            lblValidUpTo.Text = "Valid Upto " + common.myStr(DR["ValidUpto"]);
                        }
                    }
                    else
                    {
                        txtPeriod.Text = "";
                        txtPeriod.Enabled = false;
                        lblPeriodStar.Visible = false;
                        lblValidUpTo.Text = "";
                        lblValidUpTo.Visible = false;
                    }
                }
            }
            else if (common.myStr(Request.QueryString["MasterType"]) == "MM")
            {
                dsSearch = objPharmacy.getManufactureMaster(common.myInt(hdnMasterId.Value), common.myInt(Session["HospitalLocationID"]), 0,
                              "", "", common.myInt(Session["UserId"]));

                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    ViewState["_ID"] = common.myInt(hdnMasterId.Value);

                    DataRow DR = dsSearch.Tables[0].Rows[0];

                    txtName.Text = common.myStr(DR["ManufactureName"]);
                    txtCode.Text = common.myStr(DR["ManufactureNo"]);
                    txtShortName.Text = common.myStr(DR["ManufactureShortName"]);

                    txtAdd1.Text = common.myStr(DR["Add1"]);
                    txtAdd2.Text = common.myStr(DR["Add2"]);
                    txtAdd3.Text = common.myStr(DR["Add3"]);
                    txtEMail.Text = common.myStr(DR["EMail"]);
                    txtMobile.Text = common.myStr(DR["Mobile"]);
                    txtPhone.Text = common.myStr(DR["Phone"]);
                    txtFax.Text = common.myStr(DR["Fax"]);

                    txtContactPerson.Text = common.myStr(DR["ContactPerson"]);
                    txtDesignation.Text = common.myStr(DR["Designation"]);
                    txtContactPersonMobile.Text = common.myStr(DR["ContactPersonMobile"]);

                    ddlcountryname.SelectedIndex = ddlcountryname.Items.IndexOf(ddlcountryname.Items.FindItemByValue(common.myStr(DR["CountryId"])));
                    LocalCountry_SelectedIndexChanged(null, null);
                    dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindItemByValue(common.myStr(DR["StateId"])));
                    LocalState_SelectedIndexChanged(null, null);
                    dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindItemByValue(common.myStr(DR["CityId"])));
                    LocalCity_OnSelectedIndexChanged(null, null);
                    ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindItemByValue(common.myStr(DR["ZipId"])));

                    ddlStatus.SelectedValue = (common.myBool(DR["Active"]) == true) ? "1" : "0";

                    txtMRName.Text = common.myStr(DR["MedicalRepresentativesName"]);
                    txtASMgr.Text = common.myStr(DR["AreaSalesManager"]);
                    txtZSMgr.Text = common.myStr(DR["ZonalSalesManager"]);
                    txtRSMgr.Text = common.myStr(DR["RegionalSalesManager"]);
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

    protected void lnkTagging_OnClick(object Sender, EventArgs e)
    {
        try
        {
            if (common.myStr(Request.QueryString["MasterType"]) == "MM")
            {
                Response.Redirect("/Pharmacy/SupplierManufactureTagging.aspx", false);
            }
            else
            {
                Response.Redirect("/Pharmacy/SupplierManufactureTagging.aspx?SupplierId=" + common.myStr(common.myInt(ViewState["_ID"])), false);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ibtnPopup_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/Pharmacy/ItemMasterAdditionalfields.aspx?Master=S";
            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientTaxClose";
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

    protected void OnClientTaxClose_OnClick(object Sender, EventArgs e)
    {
        bindTax();
    }
    protected void gvTax_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void ddlSupplierType_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            int suppliertype = common.myInt(ddlSupplierType.SelectedValue);
            txtPeriod.Text = "";
            if (suppliertype == 118 || suppliertype == 119) // 118 =TMP,119 = TRL in statusmaste table
            {

                txtPeriod.Enabled = true;
                lblPeriodStar.Visible = true;
                lblValidUpTo.Visible = true;

            }
            else
            {

                txtPeriod.Enabled = false;
                lblPeriodStar.Visible = false;
                lblValidUpTo.Visible = false;
                lblValidUpTo.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void bindMonthDay()
    {
        try
        {
            string strIdx = "";

            ddlPaymentMonths.Items.Add(new RadComboBoxItem("", "0"));
            for (int idx = 1; idx <= 12; idx++)
            {
                strIdx = common.myStr(idx);
                ddlPaymentMonths.Items.Add(new RadComboBoxItem(strIdx, strIdx));
            }

            ddlPaymentDays.Items.Add(new RadComboBoxItem("", "0"));
            for (int idx = 1; idx <= 30; idx++)
            {
                strIdx = common.myStr(idx);
                ddlPaymentDays.Items.Add(new RadComboBoxItem(strIdx, strIdx));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

}