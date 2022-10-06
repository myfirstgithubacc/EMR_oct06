using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Drawing;
using System.Configuration;
using System.Data;

public partial class LIS_Format_TemplateServiceRemarks : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private bool RowSelStatus = false;
    public string IsActive(string strbil)
    {
        if (strbil.ToUpper() == "TRUE")
        {
            return "Yes";
        }
        else if (strbil.ToUpper() == "FALSE")
        {
            return "";
        }
        return "";

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblHeader.Text = "Template Service Remarks -&nbsp;" + common.myStr(Session["StationName"]);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            if (common.myInt(Session["StationId"]) == 0)
            {
                Response.Redirect("/LIS/Phlebotomy/ChangeStation.aspx?PT=TEMPLATEREMARKS", false);
            }
            bindTemplateServiceRemarksGrid();
            cmdSave.Text = "Save";
            ViewState["RemarksId"] = "0";

            editorFormat.RealFontSizes.Clear();
            editorFormat.RealFontSizes.Add("10px");
            editorFormat.RealFontSizes.Add("13px");
            editorFormat.RealFontSizes.Add("16px");
            editorFormat.RealFontSizes.Add("18px");
            editorFormat.RealFontSizes.Add("24px");
            editorFormat.RealFontSizes.Add("32px");
            editorFormat.RealFontSizes.Add("48px");
        }
    }

    protected void cmdNew_Click(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void cmdSave_Click(object sender, EventArgs e)
    {
        SaveTextTemplate();
    }

    private void SaveTextTemplate()
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!IsSave())
            {
                return;
            }

            BaseC.InvestigationFormat format = new BaseC.InvestigationFormat(sConString);
            BaseC.clsLISMaster objCls = new BaseC.clsLISMaster(sConString);
            //string Content = objCls.ChangeSpanTagToFontTag(common.myStr(editorFormat.Content));
            string strMsg = format.SaveUpdateTemplateServiceRemarks(common.myInt(ViewState["RemarksId"]),
                common.myInt(Session["HospitalLocationId"]), common.myStr(txtFormatName.Text), editorFormat.Content,
                common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserID"]), common.myInt(Session["StationId"]));

            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                ViewState["RemarksId"] = "0";
                cmdSave.Text = "Save";
                bindTemplateServiceRemarksGrid();
                txtFormatName.Text = "";
                editorFormat.Content = "";
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

    private bool IsSave()
    {
        bool issaved = true;
        string strmsg = "";

        if (common.myStr(txtFormatName.Text) == "")
        {
            strmsg += "Please Enter Format Name !<br />";
            issaved = false;
        }

        if (editorFormat.Text == "")
        {
            strmsg += "Please Enter Format Details !<br />";
            issaved = false;
        }

        if (common.myInt(ViewState["RemarksId"]) == 0 && common.myInt(ddlStatus.SelectedValue) != 1)
        {
            strmsg += "Status must be Active for New Format !<br />";
            issaved = false;
        }

        lblMessage.Text = strmsg;

        return issaved;
    }

    private void bindTemplateServiceRemarksGrid()
    {
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        try
        {
            BaseC.InvestigationFormat format = new BaseC.InvestigationFormat(sConString);
            ds = (DataSet)format.getServiceRemarksTemplate(common.myInt(Session["HospitalLocationId"]),
                                    0, common.myInt(Session["StationId"]));
            dv = ds.Tables[0].DefaultView;
            dv.Sort = "RemarksName Asc";
            gvTextFormat.DataSource = dv;
            gvTextFormat.DataBind();
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
            dv.Dispose();
        }
    }

    private DataTable GetData(int iRemarksId)
    {
        BaseC.InvestigationFormat format = new BaseC.InvestigationFormat(sConString);
        DataSet objDs = (DataSet)format.getServiceRemarksTemplate(common.myInt(Session["HospitalLocationId"]),
            iRemarksId, common.myInt(Session["StationId"]));
        return objDs.Tables[0];
    }

    protected void gvTextFormat_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        bindTemplateServiceRemarksGrid();
    }

    protected void gvTextFormat_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (common.myStr(e.CommandName) == "RowSelect")
            {
                gvTextFormat.Items[e.Item.ItemIndex].Selected = true;
                RowSelStatus = true;
                HiddenField hdnRemarksId = (HiddenField)e.Item.FindControl("hdnRemarksId");
                Label lblRemarksName = (Label)e.Item.FindControl("lblRemarksName");
                HiddenField hdnRemarksFormat = (HiddenField)e.Item.FindControl("hdnRemarksFormat");
                HiddenField hdnActive = (HiddenField)e.Item.FindControl("hdnActive");
                ViewState["RemarksId"] = hdnRemarksId.Value;
                txtFormatName.Text = lblRemarksName.Text;
                BaseC.clsLISMaster objCls = new BaseC.clsLISMaster(sConString);
                //string Content = objCls.ChangeFontToSpanTag(hdnRemarksFormat.Value);
                editorFormat.Content = hdnRemarksFormat.Value;
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindItemByValue(hdnActive.Value));
                cmdSave.Text = "Update";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void lnkRemarksService_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["RemarksId"]) != "0")
        {
            string EncodedRemarksName = txtFormatName.Text.Replace("&", "||");
            RadWindowForNew.NavigateUrl = "~/LIS/Format/TemplateServiceRemarksTagging.aspx?RemarkId=" + common.myStr(ViewState["RemarksId"]) + "&RemarksName=" + EncodedRemarksName;
            RadWindowForNew.Height = 580;
            RadWindowForNew.Width = 980;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            // RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select the format name";

        }
    }
    protected void gvTextFormat_PreRender(object sender, EventArgs e)
    {
        if (RowSelStatus == false)
        {
            bindTemplateServiceRemarksGrid();
        }
    }


}
