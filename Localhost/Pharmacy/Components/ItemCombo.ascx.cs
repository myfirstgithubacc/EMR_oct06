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

public partial class Pharmacy_Components_ItemCombo : System.Web.UI.UserControl
{
    private static int _ItemId = 0;
    private static int _SupplierId = 0;
    private static bool _IsBasedOnStoreId = true;
    private static bool _IsItemCloseForPurchase = false;

    private string strText = "";
    private static string pStatus = "A";

    private string strSearchText
    {
        get
        {
            return strText;
        }
        set
        {
            strText = value;
        }
    }

    private const int ItemsPerRequest = 50;

    public bool IsBasedOnStoreId
    {
        set
        {
            _IsBasedOnStoreId = value;
        }
    }

    public bool IsItemCloseForPurchase
    {
        set
        {
            _IsItemCloseForPurchase = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void cboItem_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            if (common.myStr(e.Text).Trim().Length >= 2)
            {
                DataTable tbl = GetScrolledData(e.Text);

                int itemOffset = e.NumberOfItems;
                if (itemOffset == 0)
                {
                    this.cboItem.Items.Clear();
                }
                int endOffset = Math.Min(itemOffset + ItemsPerRequest, tbl.Rows.Count);
                e.EndOfItems = endOffset == tbl.Rows.Count;

                for (int rIdx = itemOffset; rIdx < endOffset; rIdx++)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = common.myStr(tbl.Rows[rIdx]["ItemName"]);
                    item.ToolTip = common.myStr(tbl.Rows[rIdx]["ItemName"]);
                    item.Value = common.myStr(common.myInt(tbl.Rows[rIdx]["ItemId"]));

                    item.Attributes.Add("ItemNo", common.myStr(tbl.Rows[rIdx]["ItemNo"]));

                    this.cboItem.Items.Add(item);
                    item.DataBind();
                }
                e.Message = GetStatusMessage(endOffset, tbl.Rows.Count);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void GetData(string text)
    {
        try
        {
            this.cboItem.Items.Clear();

            if (common.myStr(text).Trim().Length >= 2)
            {
                string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
                BaseC.clsPharmacy objPharmacy;

                objPharmacy = new BaseC.clsPharmacy(sConString);

                //if (_ItemId < 1 && _SupplierId < 1)
                //{
                //    if (common.myLen(text) < 3)
                //    {
                //        return;
                //    }
                //}

                int storeId = 0;
                if (_IsBasedOnStoreId)
                {
                    storeId = common.myInt(Session["StoreId"]);
                }

                DataSet ds = objPharmacy.getItemList(_ItemId, _SupplierId, text, storeId, common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), 1);
                DataView DV = ds.Tables[0].DefaultView;

                if (_IsItemCloseForPurchase)
                {
                    DV.RowFilter = "ItemCloseForPurchase = 0";
                }

                DataTable tbl = DV.ToTable();
                foreach (DataRow DR in tbl.Rows)
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
        catch (Exception)
        {
            throw;
        }
    }

    public void clearValue()
    {
        _ItemId = 0;
        _SupplierId = 0;
    }

    public void setValue(int ItemId, int SupplierId)
    {
        try
        {
            _ItemId = ItemId;
            _SupplierId = SupplierId;

            this.cboItem.Text = "";
            this.cboItem.Items.Clear();

            if (ItemId < 1 && SupplierId < 1)
            {
                return;
            }

            string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
            BaseC.clsPharmacy objPharmacy;

            int storeId = 0;
            if (_IsBasedOnStoreId)
            {
                storeId = common.myInt(Session["StoreId"]);
            }

            objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = objPharmacy.getItemList(ItemId, SupplierId, "", storeId, common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), 1);

            DataView DV = ds.Tables[0].DefaultView;

            if (_IsItemCloseForPurchase)
            {
                DV.RowFilter = "ItemCloseForPurchase = 0";
            }

            DataTable tbl = DV.ToTable();

            foreach (DataRow DR in tbl.Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(DR["ItemName"]);
                item.ToolTip = common.myStr(DR["ItemName"]);
                item.Value = common.myStr(common.myInt(DR["ItemId"]));

                item.Attributes.Add("ItemNo", common.myStr(DR["ItemNo"]));

                this.cboItem.Items.Add(item);
                item.DataBind();
            }

            this.cboItem.SelectedIndex = this.cboItem.Items.IndexOf(this.cboItem.Items.FindItemByValue(common.myStr(common.myInt(ItemId))));

        }
        catch (Exception)
        {
            throw;
        }

    }

    public void ClearRadGrid()
    {
        this.cboItem.Text = "";
        this.cboItem.ToolTip = "";
        this.cboItem.SelectedValue = "";
    }

    public void SelectRadGrid(string Id)
    {
        this.cboItem.SelectedValue = Id;
    }

    public string GetSelectedItemText()
    {
        string strname = "";
        if (cboItem.SelectedIndex != -1)
        {
            strname = this.cboItem.SelectedItem.Text;
        }
        return strname;
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
        string sConStr = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConStr);

        int storeId = 0;
        if (_IsBasedOnStoreId)
        {
            storeId = common.myInt(Session["StoreId"]);
        }

        // DataSet ds = objPhr.getItemList(_ItemId, _SupplierId, text.Trim(), storeId, common.myInt(HttpContext.Current.Session["HospitalLocationId"]), common.myInt(HttpContext.Current.Session["UserID"]), 1);
        DataSet ds = objPhr.getItemList(_ItemId, _SupplierId, text.Trim(), storeId, common.myInt(HttpContext.Current.Session["HospitalLocationId"]), common.myInt(HttpContext.Current.Session["UserID"]), 1);

        DataView DV = ds.Tables[0].DefaultView;

        if (_IsItemCloseForPurchase)
        {
            DV.RowFilter = "ItemCloseForPurchase = 0";
        }

        DataTable tbl = DV.ToTable();

        return tbl;
    }

    //protected void RadCmbPatientSearch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (cboItem.SelectedValue != "")
    //    {
    //        HiddenField hdnddlid = (HiddenField)Page.FindControl("hdnddlid");
    //        if (hdnddlid != null)
    //        {
    //            hdnddlid.Value = cboItem.SelectedValue;
    //        }
    //    }
    //}
}