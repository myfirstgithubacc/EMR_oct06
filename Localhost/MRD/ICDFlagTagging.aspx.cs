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
public partial class MRD_ICDFlagTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsMRD objMRD;
    StringBuilder strXML;
    ArrayList coll;
    clsExceptionLog objException = new clsExceptionLog();
    private const int ItemsPerRequest = 50;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindICDFlag();
            bindTaggedFlag();
        }
    }

   
    protected void bindICDFlag()
    {
        try
        {
            objMRD = new BaseC.clsMRD(sConString);
            DataSet ds = objMRD.GetICDFlag(common.myInt(ddlIcd.SelectedValue));
            gvTax.DataSource = ds;
            gvTax.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void chkUnchk_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (gvTax != null)
            {
                foreach (GridDataItem dataItem in gvTax.Items)
                {
                    CheckBox chkCollection = (CheckBox)dataItem.FindControl("chkCollection");

                    chkCollection.Checked = chkUnchk.Checked;
                    dataItem.Selected = chkUnchk.Checked;
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
    protected void imgBtnChargeType_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/MRD/ICDFlagMaster.aspx";
            RadWindowForNew.Height = 500;
            RadWindowForNew.Width = 600;
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
    private void bindTaggedFlag()
    {
        try
        {
            objMRD = new BaseC.clsMRD(sConString);
            DataSet ds = objMRD.GetTaggedICDFlag(common.myInt(ddlIcd.SelectedValue));
            gvTagged.DataSource = ds;
            gvTagged.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            objMRD = new BaseC.clsMRD(sConString);

            strXML = new StringBuilder();
            coll = new ArrayList();

            foreach (GridDataItem dataItem in gvTax.Items)
            {
                CheckBox chkCollection = (CheckBox)dataItem.FindControl("chkCollection");

                if (chkCollection.Checked == true)
                {
                    Label lblItemFlagId = (Label)dataItem.FindControl("lblItemFlagId");
                    if (common.myInt(lblItemFlagId.Text) > 0)
                    {
                        coll.Add(common.myInt(lblItemFlagId.Text));
                        strXML.Append(common.setXmlTable(ref coll));
                    }
                }
            }

            string strMsg = objMRD.SaveICDFlagTagging(common.myInt(ddlIcd.SelectedValue), strXML.ToString(), common.myInt(Session["HospitalLocationID"]),
                                            common.myInt(Session["UserID"]));
            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                clearControl();
                bindICDFlag();
                bindTaggedFlag();

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
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
        lblMessage.Text = "&nbsp;";

        chkUnchk.Checked = false;
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }
    
    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }
    protected void RadComboBoxProduct_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        ddlIcd.Items.Clear();
        DataTable data = GetData(e.Text);
        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;
        for (int i = itemOffset; i < endOffset; i++)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Attributes.Add("ICDCode", data.Rows[i]["ICDCode"].ToString());
            item.Text = (string)data.Rows[i]["Description"];
            item.Value = data.Rows[i]["ICDId"].ToString();
            item.Attributes.Add("Description", data.Rows[i]["Description"].ToString());
            ddlIcd.Items.Add(item);
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
        objMRD = new BaseC.clsMRD(sConString);
        DataSet ds = objMRD.GetSearchICDByName(common.myStr(text));
        return ds.Tables[0];
    }
    protected void ddlIcd_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        bindICDFlag();
        bindTaggedFlag();
    }
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        bindICDFlag();
        bindTaggedFlag();
    }
}
