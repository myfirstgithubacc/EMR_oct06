using System;
using System.Text;
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

public partial class ICM_SummaryFormat : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.RestFulAPI objICM ;
   
    DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
         objICM = new BaseC.RestFulAPI(sConString);
        if (!IsPostBack)
        {
            BindFormatName();
        }
    }
    private void BindFormatName()
    {
        try
        {
            ds = new DataSet();
            ds = objICM.GetICMSummaryFormatName(common.myInt(Session["HospitalLocationID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlSummaryFormat.DataSource = ds.Tables[0];
                ddlSummaryFormat.DataTextField = "FormatName";
                ddlSummaryFormat.DataValueField = "ID";
                ddlSummaryFormat.DataBind();
                ddlSummaryFormat.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(""));
                ddlSummaryFormat.Items[0].Value = "0";
                ddlSummaryFormat.SelectedValue = "0";
            }
            ds.Dispose();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
        }
    }
    protected void btnSummarySave_Click(object sender, EventArgs e)
    {
        try
        {
            string sSummaryName = "";
            string sSummaryNote = "";
            if (ddlSummaryFormat.Text != "")
            {
                sSummaryName = ddlSummaryFormat.Text;
            }
            else
            {
                Alert.ShowAjaxMsg("Please type Format Name", Page);
                return;
            }
            if (RTF1.Content != "")
            {
                sSummaryNote = RTF1.Content;
            }
            else
            {
                Alert.ShowAjaxMsg("Please type Format Name", Page);
                return;
            }

            Hashtable hshOutput = objICM.SaveICMSummaryFormat(Convert.ToInt32(FormatID.Value), Convert.ToInt16(Session["HospitalLocationID"]), sSummaryName, sSummaryNote, Convert.ToInt32(Session["UserID"]), Convert.ToString(Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMMdd")));
            lblMessage.Text = hshOutput["@chvErrorStatus"].ToString();
            FormatID.Value = "";
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
        }

    }
    protected void ddlSummaryFormat_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlSummaryFormat.SelectedValue) > 0)
            {
                RTF1.Content = string.Empty;
                if (common.myInt(ddlSummaryFormat.SelectedValue) > 0)
                {
                    ds = new DataSet();
                    FormatID.Value = ddlSummaryFormat.SelectedValue;
                    ds = objICM.GetICMSummaryFormatDetails(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(FormatID.Value));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RTF1.Content = ds.Tables[0].Rows[0]["SummaryNote"].ToString();
                    }

                }
                else
                {
                    FormatID.Value = "0";
                }
            }
            else
            {
                if (common.myLen(ddlSummaryFormat.Text).Equals(0))
                {
                    RTF1.Content = string.Empty;
                }

                FormatID.Value = "0";
            }

            RTF1.Focus();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
        }
    }
}
