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

public partial class Include_Components_EmployeeSearchCombo : System.Web.UI.UserControl
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 50;
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void RadComboBoxProduct_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadCmbEmployeeSearch.Items.Clear();
        DataTable data = GetData(e.Text);
        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;
        for (int i = itemOffset; i < endOffset; i++)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Attributes.Add("EmployeeNo", data.Rows[i]["EmployeeNo"].ToString());
            item.Text = (string)data.Rows[i]["EmployeeName"];
            item.Value = data.Rows[i]["EmployeeId"].ToString();
            item.Attributes.Add("EmployeeType", data.Rows[i]["EmployeeType"].ToString());
            item.Attributes.Add("Mobile", data.Rows[i]["Mobile"].ToString());
            item.Attributes.Add("Email", data.Rows[i]["Email"].ToString());
            //item.Value += ":" + unitPrice;
            RadCmbEmployeeSearch.Items.Add(item);
            item.DataBind();
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    private DataTable GetData(string text)
    {
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        DataSet ds = objCommon.SearchEmployeeByName(common.myInt(Session["HospitalLocationId"]), common.myStr(text));       
        return ds.Tables[0];
    }
}
