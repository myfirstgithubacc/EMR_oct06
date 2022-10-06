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

public partial class MPages_CIMSTagging : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;//connectionstring
    clsExceptionLog objException = new clsExceptionLog();
    static string strSearchText = string.Empty;
    private const int ItemsPerRequest = 50;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        Page.MasterPageFile = Request.QueryString["Master"] == "I" ? "/Include/Master/BlankMaster.master" : "/Include/Master/EMRMaster.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            lblcimsdetail.Text = string.Empty;
            lblcimsdetails.Text = string.Empty;

            strSearchText = string.Empty;

            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            DataSet dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.None);

            if (dsInterface.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]))
                {
                    Session["IsCIMSInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]);
                    Session["CIMSDatabasePath"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                    Session["CIMSDatabasePassword"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePassword"]);
                    Session["CIMSDatabaseName"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabaseName"]);
                }
                else if (common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]))
                {
                    Session["IsVIDALInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]);
                }
            }

            BindBlankItemDetail();
        }
    }

    public void BlankPage()
    {
        lblMessage.Text = string.Empty;
        btnSave.Attributes.Remove("onclick");
        ViewState.Remove("SelectedCIMSItemId");
        ViewState.Remove("ItemData");
        cboItem.Text = "";
        hdnItemId.Value = "";
        RadCmbItemSearch.Text = "";
        hdnItemDetailId.Value = "";
        lblcimsdetail.Text = string.Empty;
        lblcimsdetails.Text = string.Empty;
        BindBlankItemDetail();
    }

    public void BindBlankItemDetail()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = BindBlankTable();
            DataRow dr_detail = dt.NewRow();
            dr_detail["CIMSItemId"] = 0;
            dr_detail["CIMSItemDesc"] = string.Empty;
            dr_detail["CIMSTYPE"] = string.Empty;
            dt.Rows.Add(dr_detail);
            dt.AcceptChanges();
            gvItemDetail.DataSource = dt;
            gvItemDetail.DataBind();
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }
    }

    public DataTable BindBlankTable()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = new DataTable();
            dt.Columns.Add("CIMSItemId", typeof(string));
            dt.Columns.Add("CIMSItemDesc", typeof(string));
            dt.Columns.Add("CIMSTYPE", typeof(string));
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }
        return dt;
    }

    public void DisplayTaggedItem()
    {
        DataSet ds = new DataSet();
        BaseC.clsPharmacy objclsPharmacy = new BaseC.clsPharmacy(sConString);
        try
        {
            ds = objclsPharmacy.getCIMSTaggingDetail(common.myInt(hdnItemId.Value), common.myStr(rdoBrandType.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblcimsdetail.Text = "Tagged CIMS Details";
                lblcimsdetails.Text = Convert.ToString(ds.Tables[0].Rows[0]["CIMSItemDesc"]);
            }
            else
            {
                lblcimsdetail.Text = string.Empty;
                lblcimsdetails.Text = string.Empty;
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            objclsPharmacy = null;
            ds.Dispose();
        }
    }


    protected void rdoUseFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        BlankPage();
        if (common.myStr(rdoUseFor.SelectedValue).Equals("I"))
        {
            lblName.Text = "Brand Name";
            pnlSelectBrand.Visible = true;
            rdoBrandType.SelectedValue = "I";
            lblCIMSName.Text = "CIMS Brand";
        }
        else if (common.myStr(rdoUseFor.SelectedValue).Equals("G"))
        {
            lblName.Text = "Generic Name";
            pnlSelectBrand.Visible = false;
            lblCIMSName.Text = "CIMS Generic";
        }
    }

    protected void rdoBrandType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["SelectedCIMSItemId"] = null;
        
        RadCmbItemSearch.Text = "";
        BindBlankItemDetail();

        if (common.myStr(rdoBrandType.SelectedValue).Equals("I"))
        {
            lblCIMSName.Text = "CIMS Brand";
        }
        else
        {
            lblCIMSName.Text = "CIMS Generic";
        }
    }


    protected void btnNew_Click(object sender, EventArgs e)
    {
        BlankPage();
        rdoUseFor.SelectedValue = "I";
        rdoBrandType.SelectedValue = "I";
        lblName.Text = "Brand Name";
        pnlSelectBrand.Visible = true;
        lblCIMSName.Text = "CIMS Brand";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        BaseC.clsPharmacy objclsPharmacy = new BaseC.clsPharmacy(sConString);
        try
        {
            string CIMSItemId = "", CIMSType = "", ItemDesc = "", CIMSItemDesc = "";
            int ItemId = common.myInt(hdnItemId.Value);
            ItemDesc = cboItem.Text;
            if (ViewState["ItemData"] != null)
            {
                dt = (DataTable)ViewState["ItemData"];
            }
            if (ItemId == 0)
            {
                Alert.ShowAjaxMsg("Please Select Name..", Page.Page);
                return;
            }

            if (ViewState["SelectedCIMSItemId"] != null)
            {
                CIMSItemId = common.myStr(ViewState["SelectedCIMSItemId"]);
                if (dt.Rows.Count > 0)
                {
                    DataRow[] dr = dt.Select("CIMSItemId='" + CIMSItemId + "'");
                    if (dr.Count() > 0)
                    {
                        CIMSItemId = common.myStr(dr[0]["CIMSItemId"]);
                        CIMSType = common.myStr(dr[0]["CIMSType"]);
                        CIMSItemDesc = common.myStr(dr[0]["CIMSItemDesc"]);
                    }
                }
            }
            lblMessage.Text = objclsPharmacy.saveMasterCIMSTagging(common.myInt(Session["HospitalLocationId"]), ItemId, common.myStr(rdoUseFor.SelectedValue),
                                    common.myInt(Session["UserId"]), CIMSItemId, CIMSType, ItemDesc, CIMSItemDesc);

            lblMessage.ForeColor = System.Drawing.Color.Green;
            DisplayTaggedItem();

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            objclsPharmacy = null;
        }
    }

    protected void btnbrandselect_Click(object sender, EventArgs e)
    {
        if (ViewState["SelectedCIMSItemId"] == null)
            btnSave.Attributes.Add("onclick", "return confirm('Do you want to save data without CIMS details ?')");
        else
            btnSave.Attributes.Remove("onclick");
        DisplayTaggedItem();
    }

    protected void btnItemSelect_Click(object sender, EventArgs e)
    {
        bindData(false);
    }
    protected void btnShowData_OnClick(object sender, EventArgs e)
    {
        bindData(true);
    }

    private void bindData(bool IsOnClick)
    {
        DataTable dt = new DataTable();
        clsCIMS objclsCIMS = new clsCIMS();
        ViewState.Remove("SelectedCIMSItemId");
        try
        {
            string val = common.myStr(strSearchText);
            if (!IsOnClick)
            {
                val = common.myStr(RadCmbItemSearch.Text);
            }

            dt = objclsCIMS.getCIMSDetails(common.myStr(rdoBrandType.SelectedValue), val);
            ViewState["ItemData"] = dt;

            gvItemDetail.DataSource = dt;
            gvItemDetail.DataBind();
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            objclsCIMS = null;
        }
    }

    public void RadComboBoxProduct_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable dt = new DataTable();
        clsCIMS objclsCIMS = new clsCIMS();
        try
        {
            if (common.myLen(e.Text) > 1)
            {
                dt = objclsCIMS.getCIMSDetails(common.myStr(rdoBrandType.SelectedValue), e.Text);

                strSearchText = common.myStr(e.Text);

                int itemOffset = e.NumberOfItems;
                if (itemOffset == 0)
                {
                    this.RadCmbItemSearch.Items.Clear();
                }
                int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                e.EndOfItems = endOffset == dt.Rows.Count;

                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dt.Rows[i]["CIMSItemDesc"];
                    item.Value = dt.Rows[i]["CIMSItemId"].ToString();
                    item.Attributes.Add("CIMSItemDesc", dt.Rows[i]["CIMSItemDesc"].ToString());
                    this.RadCmbItemSearch.Items.Add(item);
                    item.DataBind();
                }
                e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            objclsCIMS = null;
        }
    }

    public void cboItem_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        BaseC.clsPharmacy objclsPharmacy = new BaseC.clsPharmacy(sConString);
        try
        {
            if (common.myStr(e.Text).Trim().Length >= 2)
            {
                if (common.myStr(rdoBrandType.SelectedValue).Equals("I"))
                {
                    dt = GetScrolledData(e.Text);
                }
                else if (common.myStr(rdoBrandType.SelectedValue).Equals("G"))
                {
                    ds = objclsPharmacy.getPhrGenericMasterList(e.Text);
                    dt = ds.Tables[0];
                }
                if (dt.Rows.Count > 0)
                {
                    int itemOffset = e.NumberOfItems;
                    if (itemOffset == 0)
                    {
                        this.cboItem.Items.Clear();
                    }
                    int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                    e.EndOfItems = endOffset == dt.Rows.Count;

                    for (int rIdx = itemOffset; rIdx < endOffset; rIdx++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();

                        item.Text = common.myStr(dt.Rows[rIdx]["ItemName"]);
                        item.ToolTip = common.myStr(dt.Rows[rIdx]["ItemName"]);
                        item.Value = common.myStr(common.myInt(dt.Rows[rIdx]["ItemId"]));

                        item.Attributes.Add("ItemNo", common.myStr(dt.Rows[rIdx]["ItemNo"]));

                        this.cboItem.Items.Add(item);
                        item.DataBind();
                    }
                    e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            dt.Dispose();
            objclsPharmacy = null;
        }
    }

    public void GetData(string text)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        BaseC.clsPharmacy objclsPharmacy = new BaseC.clsPharmacy(sConString);
        try
        {
            this.cboItem.Items.Clear();

            if (common.myStr(text).Trim().Length >= 2)
            {
                int storeId = 0;

                ds = objclsPharmacy.getItemList(0, 0, text, storeId, common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), 1);
                DataView DV = ds.Tables[0].DefaultView;

                dt = DV.ToTable();
                foreach (DataRow DR in dt.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = common.myStr(DR["ItemName"]);
                    item.ToolTip = common.myStr(DR["ItemName"]);
                    item.Value = common.myStr(common.myInt(DR["ItemId"]));

                    item.Attributes.Add("ItemNo", common.myStr(DR["ItemNo"]));

                    this.cboItem.Items.Add(item);
                    item.DataBind();
                }
            }

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            dt.Dispose();
            objclsPharmacy = null;
        }
    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
        {
            return "No matches";
        }
        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    private DataTable GetScrolledData(string text)
    {
        DataSet ds = new DataSet();
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        try
        {
            ds = objPharmacy.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), 0, 0, 0, common.myInt(Session["UserId"]),
                        common.myInt(Session["FacilityId"]), 0, text.Replace("'", "''"), 0, string.Empty, 0);
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            objPharmacy = null;
        }
        return ds.Tables[0];
    }

    protected void gvItemDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("_SelectItem"))
        {
            ViewState.Remove("SelectedCIMSItemId");
            btnSave.Attributes.Remove("onclick");
            int SelectedRowIndex = common.myInt(common.myStr(e.CommandArgument));
            HiddenField hdnCIMSItemId = (HiddenField)gvItemDetail.Rows[SelectedRowIndex].FindControl("hdnCIMSItemId");
            foreach (GridViewRow gr in gvItemDetail.Rows)
            {
                gr.BackColor = System.Drawing.ColorTranslator.FromHtml("#F4F4F4");
                gr.Attributes.Add("style", "clsGridRow");
            }
            gvItemDetail.Rows[SelectedRowIndex].BackColor = System.Drawing.ColorTranslator.FromHtml("#C5DFFF");//#98AFC7
            if (hdnCIMSItemId != null)
            {
                ViewState["SelectedCIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
            }
        }
        else if (e.CommandName == "BrandDetailsCIMS")
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");
            showBrandDetails(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
        }
    }

    private void showBrandDetails(string CIMSItemId, string CIMSType)
    {
        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
        {
            Session["CIMSXMLInputData"] = string.Empty;
        }

        string strXML = getBrandDetailsXMLCIMS(CIMSType, common.myStr(CIMSItemId));

        if (strXML != string.Empty)
        {
            Session["CIMSXMLInputData"] = strXML;
            openWindowsCIMS(true);
        }
    }

    private string getBrandDetailsXMLCIMS(string CIMSType, string CIMSItemId)
    {
        string strXML = string.Empty;
        try
        {
            //<Request>
            //  <Detail>
            //    <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}">
            //      <Items />
            //      <Packages />
            //      <Images />
            //      <TherapeuticClasses />
            //      <ATCCodes />
            //      <Companies />
            //      <Identifiers />
            //    </Product>
            //  </Detail>
            //</Request>

            CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";

            strXML = "<Request><Detail><" + CIMSType + " reference=\"" + CIMSItemId + "\"><Items /><Packages /><Images /><TherapeuticClasses /><ATCCodes /><Companies /><Identifiers /></Product></Detail></Request>";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return strXML;
    }

    private void openWindowsCIMS(bool IsBrandDetails)
    {
        clsCIMS objCIMS = new clsCIMS();

        hdnCIMSOutput.Value = objCIMS.getCIMSFinalOutupt(IsBrandDetails);

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "OpenCIMSWindow();", true);
        return;

        //RadWindow1.NavigateUrl = "/EMR/Medication/Monograph1.aspx?IsBD=" + IsBrandDetails;
        //RadWindow1.Height = 600;
        //RadWindow1.Width = 900;
        //RadWindow1.Top = 10;
        //RadWindow1.Left = 10;
        ////RadWindow1.OnClientClose = "";
        //RadWindow1.VisibleOnPageLoad = true;
        //RadWindow1.Modal = true;
        //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        //RadWindow1.VisibleStatusbar = false;
    }
}
